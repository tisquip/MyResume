using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Resume.Application.ViewModels;
using Resume.Domain;
using Resume.Server.Data.Repositories;
using Resume.Server.Hubs;
using Resume.Server.Services.ExceptionNotifierServices;
using Resume.Server.Services.FootballWorkerService;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Server.Services
{
    public class BackgroundFootballService : IHostedService, IDisposable
    {
        private readonly IFootBallWorker footBallWorker;
        private readonly IRootAggregateRepositorySingleton<FootballTeam> rootAggregateRepositorySingletonFootballTeams;
        private readonly IHubContext<MatchHub> hubContextMatchHub;
        IExceptionNotifierSingleton exceptionNotifier;
        #region Cheating
        //Because its a sample, and the api key can only afford 500 calls free per month, limitation is going to be on asernal team, my wifes favorite, and the english premier league. These id's are limited to the SportDataApi service. I did not want to restrict the actual footballWorker to those id's from within the class itself, I wanted it to be from the client (this), so that its getting exactly what its calling for and it know this - the ids themselves cause this class to be coupled, in a different world, it might be best to place them in a config file, so it can be changed easily if the IFootBallWorker is changed or was dynamically despatched 
        string arsenalId = "2522";
        string englishPremierLeagueId = "237";
        #endregion

        Timer timerPopulateNewSeason;
        Timer timerLiveMatch;
        List<FootBallMatch> matchesForLiveBroadcast = new List<FootBallMatch>();


        public BackgroundFootballService(IExceptionNotifierSingleton exceptionNotifier, IFootBallWorker footBallWorker, IRootAggregateRepositorySingleton<FootballTeam> rootAggregateRepositorySingletonFootballTeams, IHubContext<MatchHub> hubContextMatchHub)
        {
            this.exceptionNotifier = exceptionNotifier;
            this.footBallWorker = footBallWorker;
            this.rootAggregateRepositorySingletonFootballTeams = rootAggregateRepositorySingletonFootballTeams;
            this.hubContextMatchHub = hubContextMatchHub;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                timerPopulateNewSeason = new Timer(PopulateNewSeason, null, TimeSpan.Zero, TimeSpan.FromDays(20));
                timerLiveMatch = new Timer(LiveMatchResults, null, TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(3));
            }
            catch (Exception ex)
            {
                await exceptionNotifier.Notify(ex);
                DisposeAll();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            DisposeAll();
            return Task.CompletedTask;
        }

        bool isProcessingMatch = false;
        FootBallMatch currentFootballMatch;
        public async void LiveMatchResults(object state)
        {
            if (!isProcessingMatch)
            {
                isProcessingMatch = true;
                try
                {
                    await SetPreviouslyPlayedGameResults();

                    if (currentFootballMatch == null || currentFootballMatch.TimeOfMatchPlus5Hours < DateTime.Now)
                    {
                        currentFootballMatch = null;

                        if (matchesForLiveBroadcast?.Any() ?? false)
                        {
                            matchesForLiveBroadcast = matchesForLiveBroadcast.OrderBy(m => m.TimeOfMatch).ToList();
                            currentFootballMatch = matchesForLiveBroadcast.FirstOrDefault(m => m.TimeOfMatchPlus5Hours > DateTime.UtcNow && m.FullTimeMatchStats == null);
                        }
                    }

                    if (currentFootballMatch != null && currentFootballMatch.FullTimeMatchStats == null && currentFootballMatch.TimeOfMatch <= DateTime.UtcNow)
                    {
                        var response = await footBallWorker.GetLiveDataForMatch(currentFootballMatch.MatchId);
                        if (response.Succeeded)
                        {
                            Log.Logger.Information("{@response}", response.ReturnedObject);
                            await SendMatchUpdate(response.ReturnedObject);
                            await CheckIfGameIsOverAndSetFinalResultIfNecessary(response.ReturnedObject);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ;
                }
                finally
                {
                    isProcessingMatch = false;
                }
            }
        }

        private async Task CheckIfGameIsOverAndSetFinalResultIfNecessary(LiveMatchStats liveMatchStats)
        {
            if (liveMatchStats != null && (liveMatchStats.MatchStatus == MatchStatus.ApiCancelled || liveMatchStats.MatchStatus == MatchStatus.FullTime))
            {
                var arsenal = GetArsernal();
                var resultFullTimeSave = await arsenal.AddFullTimeLiveMatchStats(exceptionNotifier, rootAggregateRepositorySingletonFootballTeams, liveMatchStats);

                if (resultFullTimeSave.Succeeded)
                {
                    matchesForLiveBroadcast = arsenal.JSListOfFootBallMatch_Matches.DeserializeFromJson<List<FootBallMatch>>();
                    currentFootballMatch = null;
                }
            }
        }

        private async Task SetPreviouslyPlayedGameResults()
        {
            Predicate<FootBallMatch> previouslyPlayedGamesWithoutResults = (f) => f.FullTimeMatchStats == null && f.TimeOfMatchPlus5Hours < DateTime.UtcNow;
            if (matchesForLiveBroadcast?.Any(m => previouslyPlayedGamesWithoutResults(m)) ?? false)
            {
                List<string> matchIdsWithUnfinishedGames = matchesForLiveBroadcast.Where(m => previouslyPlayedGamesWithoutResults(m)).Select(m => m.MatchId).ToList();
              
                foreach (var matchId in matchIdsWithUnfinishedGames)
                {
                    var result = await footBallWorker.GetLiveDataForMatch(matchId);
                    await CheckIfGameIsOverAndSetFinalResultIfNecessary(result?.ReturnedObject);
                }
            }
        }

        public async Task SendMatchUpdate(LiveMatchStats liveMatchStats)
        {
            if (liveMatchStats != null && currentFootballMatch != null)
            {
                LiveMatchViewModel liveMatchViewModel = new LiveMatchViewModel(currentFootballMatch, liveMatchStats);
                await hubContextMatchHub.Clients.All.SendAsync("SendMatchUpdate", liveMatchViewModel.SerializeToJson());
            }
        }


        private async void PopulateNewSeason(object state)
        {
            try
            {
                var resultCurrentSeason = await footBallWorker.GetCurrentSeason(englishPremierLeagueId);
                FootballTeam arsernal = GetArsernal();
                if (arsernal == null)
                {
                    var resultOfTeam = await footBallWorker.GetTeam(arsenalId);
                    if (resultOfTeam.Succeeded)
                    {
                        arsernal = resultOfTeam.ReturnedObject;
                        await rootAggregateRepositorySingletonFootballTeams.Insert(arsernal);
                    }
                }

                matchesForLiveBroadcast = arsernal?.JSListOfFootBallMatch_Matches?.DeserializeFromJson<List<FootBallMatch>>() ?? new List<FootBallMatch>();

                if ((resultCurrentSeason?.ReturnedObject?.IsCurrent ?? false)
                    && resultCurrentSeason.ReturnedObject.SeasonId != arsernal?.SeasonIdForFootballMatches)
                {
                    string newSeasonId = resultCurrentSeason.ReturnedObject.SeasonId;
                    var resultMatches = await footBallWorker.GetMatchesForSeason(newSeasonId);

                    if (resultMatches.ReturnedObject?.Any() ?? false)
                    {
                        List<FootBallMatch> matchesWhereArsenalIsPlaying = resultMatches.ReturnedObject.Where(m => m.HomeTeamId == arsenalId || m.AwayTeamId == arsenalId).OrderBy(m => m.TimeOfMatch).ToList();
                        if (matchesWhereArsenalIsPlaying?.Any() ?? false)
                        {
                            var insertResult = await arsernal.InsertNewMatchesForSeason(exceptionNotifier, rootAggregateRepositorySingletonFootballTeams, newSeasonId, matchesWhereArsenalIsPlaying);

                            if (insertResult.Succeeded)
                            {
                                matchesForLiveBroadcast = matchesWhereArsenalIsPlaying;
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                await exceptionNotifier.Notify(ex);
            }
        }

        FootballTeam GetArsernal()
        {
            return rootAggregateRepositorySingletonFootballTeams.GetDetachedFromDatabase(t => t.TeamId == arsenalId)?.ReturnedObject?.FirstOrDefault();
        }

        public void Dispose()
        {
            DisposeAll();
        }

        private void DisposeAll()
        {
            if (footBallWorker != null)
            {
                footBallWorker.Dispose();
            }

            if (timerLiveMatch != null)
            {
                timerLiveMatch.Dispose();
            }

            if (timerPopulateNewSeason != null)
            {
                timerPopulateNewSeason.Dispose();
            }
        }
    }
}
