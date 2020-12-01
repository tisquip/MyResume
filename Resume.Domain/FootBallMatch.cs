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
        public string SignalRBroadcastId => MatchId.ToString();
        public string MatchId { get; set; }
        public DateTime TimeOfMatch { get; set; }
        public string HomeTeamName { get; set; }
        public string HomeTeamId { get; set; }
        public string HomeTeamLogoUrl { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamId { get; set; }
        public string AwayTeamLogoUrl { get; set; }
        public string VenueName { get; set; }
        public string VenueCity { get; set; }
        public LiveMatchStats FullTimeMatchStats { get; set; }

        public FootBallMatch()
        {
        }

        public FootBallMatch(string matchId, DateTime timeOfMatch, string homeTeamName, string homeTeamId, string homeTeamLogoUrl, string awayTeamName, string awayTeamId, string awayTeamLogoUrl, string venueName, string venueCity)
        {
            MatchId = matchId;
            TimeOfMatch = timeOfMatch;
            HomeTeamName = homeTeamName;
            HomeTeamId = homeTeamId;
            HomeTeamLogoUrl = homeTeamLogoUrl;
            AwayTeamName = awayTeamName;
            AwayTeamId = awayTeamId;
            AwayTeamLogoUrl = awayTeamLogoUrl;
            VenueName = venueName;
            VenueCity = venueCity;
        }
    }
}
