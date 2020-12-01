using Resume.Domain;
using Resume.Server.Services.FootballWorkerService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resume.Server.Services.FootballWorkerService
{
    public interface IFootBallWorker
    {
        Task<FootballWorkerServiceSeason> GetCurrentSeason();
        Task<List<FootBallMatch>> GetMatchesForCurrentSeason(string seasonId);
        Task<LiveMatchStats> GetLiveDataForMatch(string matchId);
    }
}
