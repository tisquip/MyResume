using Resume.Domain;
using Resume.Domain.Response;
using Resume.Server.Services.FootballWorkerService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resume.Server.Services.FootballWorkerService
{
    public interface IFootBallWorker
    {
        Task<Result<FootballWorkerServiceSeason>> GetCurrentSeason(string leagueId);
        Task<Result<List<FootBallMatch>>> GetMatchesForCurrentSeason(string seasonId);
        Task<Result<LiveMatchStats>> GetLiveDataForMatch(string matchId);
    }
}
