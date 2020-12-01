using Resume.Domain.BaseClasses;
using Resume.Domain.Interfaces;
using Resume.Domain.Interfaces.RepoInterfaces;
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
        public string TeamId { get; private set; }
        public string LogoUrl { get; private set; }
        public DateTime LastUpdateOfSeasonMatches { get; private set; }
        public string SeasonIdForFootballMatches { get; private set; }
        public string JSListOfFootBallMatch_Matches { get; private set; } = "[]";

        protected FootballTeam()
        {
        }

        public FootballTeam(string name, string teamId, string logoUrl, List<FootBallMatch> footBallMatches)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            TeamId = teamId;
            LogoUrl = logoUrl ?? throw new ArgumentNullException(nameof(logoUrl));
            LastUpdateOfSeasonMatches = DateTime.UtcNow;
            JSListOfFootBallMatch_Matches = footBallMatches?.SerializeToJson() ?? "[]";
        }

        public async Task<Result> AddFullTimeLiveMatchStats(IExceptionNotifier exceptionNotifier, IRootAggregateRepository<FootballTeam> rootAggregateRepositoryFootballTeam, LiveMatchStats fullTimeLiveMatchStats)
        {
            Result vtr = new Result();
            try
            {
                if (fullTimeLiveMatchStats == null || String.IsNullOrWhiteSpace(fullTimeLiveMatchStats.MatchId))
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
                        FootBallMatch matchToSet = matches.FirstOrDefault(m => m.MatchId == fullTimeLiveMatchStats.MatchId);
                        if (matchToSet == null)
                        {
                            vtr.SetError($"Match with match id {fullTimeLiveMatchStats.MatchId} was not found");
                        }
                        else
                        {
                            matchToSet.FullTimeMatchStats = fullTimeLiveMatchStats;
                            JSListOfFootBallMatch_Matches = matches.SerializeToJson();
                            vtr = await rootAggregateRepositoryFootballTeam.Update(this);
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

        public async Task<Result> InsertNewMatchesForSeason(IExceptionNotifier exceptionNotifier, IRootAggregateRepository<FootballTeam> rootAggregateRepositoryFootBallTeam, string seasonIdForFootballMatches, List<FootBallMatch> footBallMatches)
        {
            Result vtr = new Result(true);
            try
            {
                bool footballMatchesAreValid = footBallMatches?.Any() ?? false;
                bool seasonIdIsValid = !String.IsNullOrWhiteSpace(seasonIdForFootballMatches);
                if (!footballMatchesAreValid || !seasonIdIsValid)
                {
                    List<string> invalidArguments = new List<string>();
                    if (!footballMatchesAreValid)
                    {
                        invalidArguments.Add(nameof(footBallMatches));
                    }

                    if (!seasonIdIsValid)
                    {
                        invalidArguments.Add(nameof(seasonIdForFootballMatches));
                    }

                    vtr.SetErrorInvalidArguments(invalidArguments.ToArray());
                }
                else
                {
                    if (SeasonIdForFootballMatches.Equals(seasonIdForFootballMatches, StringComparison.OrdinalIgnoreCase))
                    {
                        vtr.SetError($"Matches with season id {seasonIdForFootballMatches} have already been added");
                    }
                    else
                    {
                        SeasonIdForFootballMatches = seasonIdForFootballMatches;
                        JSListOfFootBallMatch_Matches = footBallMatches.SerializeToJson();
                        vtr = await rootAggregateRepositoryFootBallTeam.Update(this);
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
