﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Resume.Domain
{
    /// <summary>
    /// Minimal data, the concept is the same for getting the rest of the data, e.g number of fouls, who scored and the like
    /// </summary>
    public class LiveMatchStats
    {
        public string MatchId { get; set; }
        public int GoalsScoredHomeSide { get; set; }
        public int GoalsScoredAwaySide { get; set; }
        public int RedCardsHome { get; set; }
        public int RedCardsAway { get; set; }
        public MatchStatus MatchStatus { get; set; }

        public LiveMatchStats()
        {
        }

        public LiveMatchStats(string matchId, int goalsScoredHomeSide, int goalsScoredAwaySide, int redCardsHome, int redCardsAway, MatchStatus matchStatus)
        {
            MatchId = matchId;
            GoalsScoredHomeSide = goalsScoredHomeSide;
            GoalsScoredAwaySide = goalsScoredAwaySide;
            RedCardsHome = redCardsHome;
            RedCardsAway = redCardsAway;
            MatchStatus = matchStatus;
        }
    }

    public enum MatchStatus
    {
        NotStarted,
        Started,
        HalfTime,
        FullTime
    }
}
