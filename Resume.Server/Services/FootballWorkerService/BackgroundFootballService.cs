using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Resume.Domain;
using Resume.Domain.Interfaces;
using Resume.Server.Data;
using Resume.Server.Services.ExceptionNotifierServices;
using Resume.Server.Services.FootballWorkerService;
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
        private readonly ResumeBackgroundServiceDbContext resumeBackgroundServiceDbContext;
        IExceptionNotifierSingleton exceptionNotifier;
        #region Cheating
        //Because its a sample, and the api key can only afford 500 calls free per month, limitation is going to be on asernal team, my wifes favorite, and the english premier league. These id's are limited to the SportDataApi service. I did not want to restrict the actual footballWorker to those id's from within the class itself, I wanted it to be from the client (this), so that its getting exactly what its calling for and it know this - the ids themselves cause this class to be coupled, in a different world, it might be best to place them in a config file, so it can be changed easily if the IFootBallWorker is changed or was dynamically despatched 
        string arsenalId = "2522";
        string englishPremierLeagueId = "352";
        #endregion

        Timer timerPopulateNewSeason;
        Timer timerLiveMatch;
        List<FootBallMatch> matchesForLiveBroadcast = new List<FootBallMatch>();


        public BackgroundFootballService(IExceptionNotifierSingleton exceptionNotifier, IFootBallWorker footBallWorker, ResumeBackgroundServiceDbContext resumeBackgroundServiceDbContext)
        {
            this.exceptionNotifier = exceptionNotifier;
            this.footBallWorker = footBallWorker;
            this.resumeBackgroundServiceDbContext = resumeBackgroundServiceDbContext;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                timerPopulateNewSeason = new Timer(PopulateNewSeason, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));
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

        private async void PopulateNewSeason(object state)
        {
            try
            {
                var resultCurrentSeason = await footBallWorker.GetCurrentSeason(englishPremierLeagueId);
                FootballTeam arsernal = await GetArsernal();
                if (arsernal == null)
                {
                    var resultOfTeam = await footBallWorker.GetTeam(arsenalId);
                    if (resultOfTeam.Succeeded)
                    {
                        arsernal = resultOfTeam.ReturnedObject;
                        resumeBackgroundServiceDbContext.FootballTeam.Add(arsernal);
                        await resumeBackgroundServiceDbContext.SaveChangesAsync();
                        resumeBackgroundServiceDbContext.ChangeTracker.Clear();
                    }
                }

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
                            //var insertResult = arsernal.InsertNewMatchesForSeason(exceptionNotifier, , newSeasonId, matchesWhereArsenalIsPlaying);

                            //if (insertResult.Succedded)
                            //{
                            //    matchesForLiveBroadcast = matchesWhereArsenalIsPlaying;
                            //    int sixteyDaysInMilliSeconds = TimeSpan.FromDays(60).Milliseconds;
                            //    timerPopulateNewSeason.Change(sixteyDaysInMilliSeconds, 0);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await exceptionNotifier.Notify(ex);
            }
        }

        async Task<FootballTeam> GetArsernal()
        {
            return await resumeBackgroundServiceDbContext.FootballTeam.AsNoTracking().FirstOrDefaultAsync(t => t.TeamId == arsenalId);
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
