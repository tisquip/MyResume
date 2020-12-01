using Microsoft.Extensions.Configuration;
using Resume.Domain;
using Resume.Domain.Response;
using Resume.Server.Services.FootballWorkerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Resume.Server.Services.FootballWorkerService
{
    public class SportDataApiFootballWorkerService : IFootBallWorker
    {
        private readonly HttpClient httpClient;
        string apiKey;

        public SportDataApiFootballWorkerService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            apiKey = configuration.GetValue<string>("SportDataApiKey"); //<-- Its in secrets manager, not in appsettings.json
        }

        public async Task<Result<FootballWorkerServiceSeason>> GetCurrentSeason(string leagueId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<LiveMatchStats>> GetLiveDataForMatch(string matchId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<List<FootBallMatch>>> GetMatchesForCurrentSeason(string seasonId)
        {
            throw new NotImplementedException();
        }
    }

    #region Classes pasted using pasteJson classes, theses are the responses from the api that need to be converted to the appropriate classes

    #region Responses from GetCurrentSeason
    #endregion

    #endregion
}
