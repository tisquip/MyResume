using Resume.Domain;
using Resume.Domain.Response;
using Resume.Server.Services.FootballWorkerService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resume.Server.Services.FootballWorkerService
{
    public interface IFootBallWorker : IDisposable
    {
        Task<Result<FootballWorkerServiceSeason>> GetCurrentSeason(string leagueId);
        Task<Result<List<FootBallMatch>>> GetMatchesForSeason(string seasonId);
        Task<Result<LiveMatchStats>> GetLiveDataForMatch(string matchId);
        Task<Result<FootballTeam>> GetTeam(string teamId);
    }
}
