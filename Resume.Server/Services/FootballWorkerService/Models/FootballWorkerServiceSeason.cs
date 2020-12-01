using System;

namespace Resume.Server.Services.FootballWorkerService.Models
{
    public class FootballWorkerServiceSeason
    {
        public string SeasonId { get; set; }
        public string Name { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        protected FootballWorkerServiceSeason()
        {
        }

        public FootballWorkerServiceSeason(string seasonId, string name, bool isCurrent, DateTime startDate, DateTime endDate)
        {
            SeasonId = seasonId ?? throw new ArgumentNullException(nameof(seasonId));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IsCurrent = isCurrent;
            StartDate = startDate;
            EndDate = endDate;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
