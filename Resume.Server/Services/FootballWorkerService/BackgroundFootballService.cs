using Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Resume.Application.ViewModels;
using Resume.Domain;
using Resume.Domain.Interfaces;
using Resume.Server.Data;
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
                timerPopulateNewSeason = new Timer(LiveMatchResults, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            }
            catch (Exception ex)
            {
                await exceptionNotifier.Notify(ex);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            DisposeAll();
            return Task.CompletedTask;
        }

        bool isProcessingMatch = false;
        public async void LiveMatchResults(object state)
        {
            if (!isProcessingMatch)
            {
                isProcessingMatch = true;
                try
                {
                    
                    string barcelonaChampionsLeagueMatchId = "237668";
                    var response = await footBallWorker.GetLiveDataForMatch(barcelonaChampionsLeagueMatchId);
                    if (response.Succeeded)
                    {
                        await SendMatchUpdate(response.ReturnedObject);
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

        public async Task SendMatchUpdate(LiveMatchStats liveMatchStats)
        {
            
            if (liveMatchStats != null)
            {
                FootBallMatch footBallMatch = matchesForLiveBroadcast.FirstOrDefault(f => f.MatchId == liveMatchStats.MatchId);

                footBallMatch = new FootBallMatch(237668.ToString(), DateTime.UtcNow, "Ferencvarosi TC", 3462.ToString(), "https://cdn.sportdataapi.com/images/soccer/teams/100/66.png", "FC Barcelona", 6404.ToString(), "https://cdn.sportdataapi.com/images/soccer/teams/100/99.png", "Groupama Arena", "Budapest");

                if (footBallMatch != null)
                {
                    LiveMatchViewModel liveMatchViewModel = new LiveMatchViewModel(footBallMatch, liveMatchStats);

                    
                    await hubContextMatchHub.Clients.All.SendAsync("SendMatchUpdate", liveMatchViewModel.SerializeToJson());
                }
                
            }
        }


        bool isProcessing = false;
        
        private async void PopulateNewSeason(object state)
        {
            if (!isProcessing)
            {
                try
                {
                    isProcessing = true;
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
                        && !resultCurrentSeason.ReturnedObject.SeasonId.Equals(arsernal?.SeasonIdForFootballMatches ?? "", StringComparison.OrdinalIgnoreCase))
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
                                    timerPopulateNewSeason.Change((int)TimeSpan.FromDays(20).TotalMilliseconds, 0);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    await exceptionNotifier.Notify(ex);
                }
                finally
                {
                    isProcessing = false;
                }
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
