using Resume.Domain.BaseClasses;
using Resume.Domain.Interfaces;
using Resume.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume.Domain
{
    /// <summary>
    /// In order for me to develop faster, these classes are just going to be serialized as JSON in thier Aggregate root, so public setters and the like. The only proper domain class that protects its invariants and tries to adhere to DDD is PaymentReceipt.cs
    /// </summary>
    public class FootballTeam : RootAggregateBase
    {
        public string Name { get; private set; }
        public int SportDataTeamId { get; private set; }
        public string SportDataLogoUrl { get; private set; }
        public DateTime LastCheckedFromApi { get; set; }
        public string JSListOfFootBallMatch_Matches { get; set; } = "[]";

        protected FootballTeam()
        {
        }

        public FootballTeam(string name, int sportDataTeamId, string sportDataLogoUrl, DateTime lastCheckedFromApi, List<FootBallMatch> footBallMatches)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SportDataTeamId = sportDataTeamId;
            SportDataLogoUrl = sportDataLogoUrl ?? throw new ArgumentNullException(nameof(sportDataLogoUrl));
            LastCheckedFromApi = lastCheckedFromApi;
            JSListOfFootBallMatch_Matches = footBallMatches?.SerializeToJson() ?? "[]";
        }

        public async Task<Result> AddFullTimeLiveMatchStats(IExceptionNotifier exceptionNotifier, LiveMatchStats fullTimeLiveMatchStats)
        {
            Result vtr = new Result();
            try
            {
                if (fullTimeLiveMatchStats == null || fullTimeLiveMatchStats.SportDataMatchId <= 0)
                {
                    vtr.SetErrorInvalidArguments(nameof(fullTimeLiveMatchStats));
                }
                else
                {
                    if (fullTimeLiveMatchStats.MatchStatus != MatchStatus.FullTime)
                    {
                        vtr.SetError("Match is not over yet");
                    }
                    else
                    {
                        List<FootBallMatch> matches = JSListOfFootBallMatch_Matches.DeserializeFromJson<List<FootBallMatch>>();
                        FootBallMatch matchToSet = matches.FirstOrDefault(m => m.SportDataMatchId == fullTimeLiveMatchStats.SportDataMatchId);
                        if (matchToSet == null)
                        {
                            vtr.SetError($"Match with match id {fullTimeLiveMatchStats.SportDataMatchId} was not found");
                        }
                        else
                        {
                            matchToSet.FullTimeMatchStats = fullTimeLiveMatchStats;
                            JSListOfFootBallMatch_Matches = matches.SerializeToJson();
                            vtr.SetSuccess();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await exceptionNotifier.Notify(ex, vtr);
            }
            return vtr;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
