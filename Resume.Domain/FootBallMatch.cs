using System;
using System.Collections.Generic;
using System.Text;

namespace Resume.Domain
{
    /// <summary>
    /// In order for me to develop faster, these classes are just going to be serialized as JSON in thier Aggregate root, so public setters and the like. The only proper domain class that protects its invariants and tries to adhere to DDD is PaymentReceipt.cs
    /// </summary>
    public class FootBallMatch
    {
        public string SignalRBroadcastId => SportDataMatchId.ToString();
        public int SportDataMatchId { get; set; }
        public DateTime TimeOfMatch { get; set; }
        public string SportDataHomeTeamName { get; set; }
        public int SportDataHomeTeamId { get; set; }
        public string SportDataHomeTeamLogoUrl { get; set; }
        public string SportDataAwayTeamName { get; set; }
        public int SportDataAwayTeamId { get; set; }
        public string SportDataAwayTeamLogoUrl { get; set; }
        public string SportDataVenueName { get; set; }
        public string SportDataVenueCity { get; set; }

        public FootBallMatch()
        {
        }

        public FootBallMatch(int sportDataMatchId, DateTime timeOfMatch, string sportDataHomeTeamName, int sportDataHomeTeamId, string sportDataHomeTeamLogoUrl, string sportDataAwayTeamName, int sportDataAwayTeamId, string sportDataAwayTeamLogoUrl, string sportDataVenueName, string sportDataVenueCity)
        {
            SportDataMatchId = sportDataMatchId;
            TimeOfMatch = timeOfMatch;
            SportDataHomeTeamName = sportDataHomeTeamName;
            SportDataHomeTeamId = sportDataHomeTeamId;
            SportDataHomeTeamLogoUrl = sportDataHomeTeamLogoUrl;
            SportDataAwayTeamName = sportDataAwayTeamName;
            SportDataAwayTeamId = sportDataAwayTeamId;
            SportDataAwayTeamLogoUrl = sportDataAwayTeamLogoUrl;
            SportDataVenueName = sportDataVenueName;
            SportDataVenueCity = sportDataVenueCity;
        }
    }
}
