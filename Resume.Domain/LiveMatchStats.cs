using System;
using System.Collections.Generic;
using System.Text;

namespace Resume.Domain
{
    /// <summary>
    /// Minimal data, the concept is the same for getting the rest of the data, e.g number of fouls, who scored and the like
    /// </summary>
    public class LiveMatchStats
    {
        public string Minute { get; set; }
        public string MatchId { get; set; }
        public int GoalsScoredHomeSide { get; set; }
        public int GoalsScoredAwaySide { get; set; }
        public int PenaltiesScoredHomeSide { get; set; }
        public int PenaltiesScoredAwaySide { get; set; }
        //Removing RedCards, they wont add much value to this demo since the goals scored and penalties scored highlight roughly the same concept. No database migrations are needed for the additional properties as well as removing of these properties since this object is saved as a json string by its host
        //public int RedCardsHome { get; set; }
        //public int RedCardsAway { get; set; }
        public MatchStatus MatchStatus { get; set; }

        public LiveMatchStats()
        {
        }

        public LiveMatchStats(string matchId, int goalsScoredHomeSide, int goalsScoredAwaySide, int penaltiesScoredHomeSide, int penaltiesScoredAwaySide, MatchStatus matchStatus, string minute)
        {
            MatchId = matchId;
            GoalsScoredHomeSide = goalsScoredHomeSide;
            GoalsScoredAwaySide = goalsScoredAwaySide;
            PenaltiesScoredHomeSide = penaltiesScoredHomeSide;
            PenaltiesScoredAwaySide = penaltiesScoredAwaySide;
            MatchStatus = matchStatus;
            Minute = minute;
        }
    }

    public enum MatchStatus
    {
        NotStarted,
        Started,
        HalfTime,
        FullTime,
        /// <summary>
        /// Dont want to write logic for a delayed game, postponed game and the like so just going to cancel
        /// </summary>
        ApiCancelled 
    }
}
