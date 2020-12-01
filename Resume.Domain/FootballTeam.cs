using Resume.Domain.BaseClasses;
using System;
using System.Collections.Generic;
using System.Text;

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
            JSListOfFootBallMatch_Matches = 
        }

        public override string ToString()
        {
            return Name;
        }


    }
}
