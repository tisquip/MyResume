using Resume.Domain;
using Resume.Domain.Response;
using Resume.Server.Services.FootballWorkerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resume.Server.Services.FootballWorkerService.InterfaceImplementations
{
    //To test signalR. and communication to the blazor project, so need a match and also dont want to be hiting the real api since it counts on the calls, I can hit the cdn tho, where images are
    public class MockFootballWorker : IFootBallWorker
    {
        public void Dispose()
        {
        }
        string seasonId = "QWER";
        public async Task<Result<FootballWorkerServiceSeason>> GetCurrentSeason(string leagueId)
        {
            FootballWorkerServiceSeason footballWorkerServiceSeason = new FootballWorkerServiceSeason(seasonId, "20/21", true, DateTime.UtcNow - TimeSpan.FromDays(2), DateTime.UtcNow + TimeSpan.FromDays(2));
            var vtr = new Result<FootballWorkerServiceSeason>();
            vtr.SetSuccessObject(footballWorkerServiceSeason);
            return await Task.FromResult(vtr);
        }

        string matchIdForLiveGame;
        int time = 0;
        int homeScore = 0;
        int awayScore = 0;
        MatchStatus matchStatus = MatchStatus.Started;
        public async Task<Result<LiveMatchStats>> GetLiveDataForMatch(string matchId)
        {
            if (matchId != matchIdForLiveGame)
            {
                matchIdForLiveGame = matchId;
                time = 0;
                matchStatus = MatchStatus.Started;
                homeScore = 0;
                awayScore = 0;
            }

            time++;
            switch (time)
            {
                case 2:
                    homeScore++;
                    break;
                case 3:
                    homeScore++;
                    break;
                case 4:
                    awayScore++;
                    break;
                case 5:
                    homeScore++;
                    break;
                case 6:
                    matchStatus = MatchStatus.HalfTime;
                    break;
                case 7:
                    matchStatus = MatchStatus.Started;
                    break;
                case 8:
                    awayScore++;
                    break;
                case 9:
                    homeScore++;
                    break;
                case 10:
                    matchStatus = MatchStatus.FullTime;
                    break;
                default:
                    break;
            }

            LiveMatchStats liveMatchStats = new LiveMatchStats(matchId, homeScore, awayScore, 0, 0, matchStatus, time.ToString());
            Result<LiveMatchStats> vtr = new Result<LiveMatchStats>();
            vtr.SetSuccessObject(liveMatchStats);

            return await Task.FromResult(vtr);
        }

        public async Task<Result<List<FootBallMatch>>> GetMatchesForSeason(string seasonId)
        {
            var arsernal = GetArsenal();
            var liverpool = GetLiverpool();
            var manCity = GetManCity();

            var footBallMatch1 = new FootBallMatch("2minFromNow", DateTime.UtcNow + TimeSpan.FromMinutes(2), arsernal.Name, arsernal.TeamId, arsernal.LogoUrl, liverpool.Name, liverpool.TeamId, liverpool.LogoUrl, "Emirates", "London");

            var footballMatch2 = new FootBallMatch("7minFromNow", DateTime.UtcNow + TimeSpan.FromMinutes(7), manCity.Name, manCity.TeamId, manCity.LogoUrl, arsernal.Name, arsernal.TeamId, arsernal.LogoUrl, "Stadium of Light", "Manchester");

            List<FootBallMatch> footBallMatches = new List<FootBallMatch>();

            for (int i = 0; i < 100; i++)
            {
                string num = (i + 1).ToString();
                await Task.Delay(TimeSpan.FromSeconds(2));
                if (i % 2 == 0)
                {
                    footBallMatches.Add(new FootBallMatch($"Match-{num}", DateTime.UtcNow, arsernal.Name, arsernal.TeamId, arsernal.LogoUrl, liverpool.Name, liverpool.TeamId, liverpool.LogoUrl, "Emirates", "London"));
                }
                else
                {
                    footBallMatches.Add(new FootBallMatch($"Match-{num}", DateTime.UtcNow, manCity.Name, manCity.TeamId, manCity.LogoUrl, arsernal.Name, arsernal.TeamId, arsernal.LogoUrl, "Stadium of Light", "Manchester"));
                }
            }

            Result<List<FootBallMatch>> vtr = new Result<List<FootBallMatch>>();
            vtr.SetSuccessObject(footBallMatches);

            return await Task.FromResult(vtr);
        }

        FootballTeam GetArsenal()
        {
            return new FootballTeam("Arsenal FC", "2522", "https://cdn.sportdataapi.com/images/soccer/teams/100/18.png");
        }

        FootballTeam GetManCity()
        {
            return new FootballTeam("Manchester City", "851", "https://cdn.sportdataapi.com/images/soccer/teams/100/4.png");
        }

        FootballTeam GetLiverpool()
        {
            return new FootballTeam("Liverpool FC", "2509", "https://cdn.sportdataapi.com/images/soccer/teams/100/1.png");
        }

        public async Task<Result<FootballTeam>> GetTeam(string teamId)
        {
            Result<FootballTeam> vtr = new Result<FootballTeam>();
            vtr.SetSuccessObject(GetArsenal());

            return await Task.FromResult(vtr);
        }

        
    }
}
