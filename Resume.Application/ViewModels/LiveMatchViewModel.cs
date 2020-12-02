using Resume.Domain;

namespace Resume.Application.ViewModels
{
    public class LiveMatchViewModel
    {
        public string MatchId { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamLogo { get; set; }
        public int GoalsHomeTeam { get; set; }
        public int GoalsAwayTeam { get; set; }
        public int PenaltiesScoredHomeTeam { get; set; }
        public int PenaltiesScoredAwayTeam { get; set; }
        public string MatchStatus { get; set; }
        public string Minute { get; set; }

        public LiveMatchViewModel()
        {
        }

        public LiveMatchViewModel(string matchId, string homeTeamName, string homeTeamLogo, string awayTeamName, string awayTeamLogo, int goalsHomeTeam, int goalsAwayTeam, int penaltiesScoredHomeTeam, int penaltiesScoredAwayTeam, string matchStatus, string minute)
        {
            MatchId = matchId;
            HomeTeamName = homeTeamName;
            HomeTeamLogo = homeTeamLogo;
            AwayTeamName = awayTeamName;
            AwayTeamLogo = awayTeamLogo;
            GoalsHomeTeam = goalsHomeTeam;
            GoalsAwayTeam = goalsAwayTeam;
            PenaltiesScoredHomeTeam = penaltiesScoredHomeTeam;
            PenaltiesScoredAwayTeam = penaltiesScoredAwayTeam;
            MatchStatus = matchStatus;
            Minute = minute;
        }

        public LiveMatchViewModel(FootBallMatch footBallMatch, LiveMatchStats liveMatchStats)
        {
            AwayTeamLogo = footBallMatch.AwayTeamLogoUrl;
            HomeTeamLogo = footBallMatch.HomeTeamLogoUrl;
            AwayTeamName = footBallMatch.AwayTeamName;
            HomeTeamName = footBallMatch.HomeTeamName;
            GoalsAwayTeam = liveMatchStats.GoalsScoredAwaySide;
            GoalsHomeTeam = liveMatchStats.GoalsScoredHomeSide;
            MatchId = liveMatchStats.MatchId;
            MatchStatus = liveMatchStats.MatchStatus.ToString();
            Minute = liveMatchStats.Minute;
            PenaltiesScoredAwayTeam = liveMatchStats.PenaltiesScoredAwaySide;
            PenaltiesScoredHomeTeam = liveMatchStats.PenaltiesScoredHomeSide;
        }
    }
}
