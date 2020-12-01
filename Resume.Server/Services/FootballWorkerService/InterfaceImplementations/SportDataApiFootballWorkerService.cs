using Microsoft.Extensions.Configuration;
using Resume.Domain;
using Resume.Domain.Response;
using Resume.Server.Services.FootballWorkerService.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Resume.Server.Services.FootballWorkerService.InterfaceImplementations
{
    public class SportDataApiFootballWorkerService : IFootBallWorker
    {
        private readonly HttpClient httpClient;
        readonly string apiKey;
        string urlBaseSportDataApi = "https://app.sportdataapi.com/api/v1/soccer/";

        public SportDataApiFootballWorkerService(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            apiKey = configuration.GetValue<string>("SportDataApiKey"); //<-- Its in secrets manager, not in appsettings.json
        }

        public async Task<Result<FootballWorkerServiceSeason>> GetCurrentSeason(string leagueId)
        {
            Result<FootballWorkerServiceSeason> vtr = new Result<FootballWorkerServiceSeason>(true);

            try
            {
                string url = $"{urlBaseSportDataApi}seasons?apikey={apiKey}&league_id={leagueId}";
                var sportDataApiResponse_Seasons = await httpClient.GetFromJsonAsync<SportDataApiResponse_GetCurrentSeason>(url);
                if (sportDataApiResponse_Seasons == null)
                {
                    vtr.SetError($"No seasons found for league with id {leagueId}");
                }
                else
                {
                    var currentSeason = sportDataApiResponse_Seasons.data?.FirstOrDefault(d => d.is_current == 1);
                    if (currentSeason == null)
                    {
                        vtr.SetError($"No current seasons found for league with id {leagueId}");
                    }
                    else
                    {
                        vtr.SetSuccessObject(new FootballWorkerServiceSeason(currentSeason.season_id.ToString(), currentSeason.name, true, DateTime.Parse(currentSeason.start_date), DateTime.Parse(currentSeason.end_date)));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            Log.Logger.Information("result: {@result}", vtr);
            return vtr;
        }

        public async Task<Result<LiveMatchStats>> GetLiveDataForMatch(string matchId)
        {
            //https://app.sportdataapi.com/api/v1/soccer/matches/139383?apikey=ae528850-33a5-11eb-9580-5139957b12ee
            throw new NotImplementedException();
        }

        public async Task<Result<List<FootBallMatch>>> GetMatchesForSeason(string seasonId)
        {
            Result<List<FootBallMatch>> vtr = new Result<List<FootBallMatch>>(true);
            try
            {
                string url = $"{urlBaseSportDataApi}matches?apikey={apiKey}&season_id={seasonId}";

                var sportDataApiResponse_GetMatchesForSeason = await httpClient.GetFromJsonAsync<SportDataApiResponse_GetMatchesForSeason>(url);

                if (!sportDataApiResponse_GetMatchesForSeason?.data?.Any() ?? true)
                {
                    vtr.SetError($"No matches found for season with id {seasonId}");
                }
                else
                {
                    List<FootBallMatch> footBallMatches = sportDataApiResponse_GetMatchesForSeason.data.Select(d => new FootBallMatch(
                        d.match_id.ToString(), DateTime.Parse(d.match_start), d.home_team.name,
                        d.home_team.team_id.ToString(), d.home_team.logo, d.away_team.name,
                        d.away_team.team_id.ToString(), d.away_team.logo, d.venue?.name,
                        d.venue?.city))
                        ?.ToList() ?? new List<FootBallMatch>();

                    vtr.SetSuccessObject(footBallMatches);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return vtr;
        }

        public void Dispose()
        {
            if (httpClient != null)
            {
                httpClient.Dispose();
            }
        }
    }

    #region Classes pasted using pasteJson classes, theses are the responses from the api that need to be converted to the appropriate classes

    #region Responses from GetCurrentSeason

    public class SportDataApiResponse_GetCurrentSeason
    {
        public SportDataApiResponse_GetCurrentSeason_Query query { get; set; }
        public SportDataApiResponse_GetCurrentSeason_Data[] data { get; set; }
    }

    public class SportDataApiResponse_GetCurrentSeason_Query
    {
        public string apikey { get; set; }
        public string league_id { get; set; }
    }

    public class SportDataApiResponse_GetCurrentSeason_Data
    {
        public int season_id { get; set; }
        public string name { get; set; }
        public int is_current { get; set; }
        public int country_id { get; set; }
        public int league_id { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }

    #endregion

    #region Responses from GetMatchesForSeason
    //public class SportDataApiResponse_GetMatchesForSeason
    //{
    //    public SportDataApiResponse_GetMatchesForSeason_Query query { get; set; }
    //    public SportDataApiResponse_GetMatchesForSeason_Data data { get; set; }
    //}

    //public class SportDataApiResponse_GetMatchesForSeason_Query
    //{
    //    public string season_id { get; set; }
    //    public string date_from { get; set; }
    //    public string apikey { get; set; }
    //}

    //public class SportDataApiResponse_GetMatchesForSeason_Data
    //{
    //    public SportDataApiResponse_GetMatchesForSeason_Match[] matches { get; set; }
    //}

    //public class SportDataApiResponse_GetMatchesForSeason_Match
    //{
    //    public int match_id { get; set; }
    //    public int status_code { get; set; }
    //    public string status { get; set; }
    //    public string match_start { get; set; }
    //    public int league_id { get; set; }
    //    public int season_id { get; set; }
    //    public Team home_team { get; set; }
    //    public Team away_team { get; set; }
    //    public Stats stats { get; set; }
    //    public Venue venue { get; set; }
    //}
    //public class Team
    //{
    //    public int team_id { get; set; }
    //    public string name { get; set; }
    //    public string short_code { get; set; }
    //    public string logo { get; set; }
    //    public Country country { get; set; }
    //}

    //public class Country
    //{
    //    public int country_id { get; set; }
    //    public string name { get; set; }
    //    public string country_code { get; set; }
    //    public string continent { get; set; }
    //}


    //public class Stats
    //{
    //    public int home_score { get; set; }
    //    public int away_score { get; set; }
    //    public string ht_score { get; set; }
    //    public string ft_score { get; set; }
    //    public object et_score { get; set; }
    //    public object ps_score { get; set; }
    //}

    //public class Venue
    //{
    //    public int venue_id { get; set; }
    //    public string name { get; set; }
    //    public int capacity { get; set; }
    //    public string city { get; set; }
    //    public int country_id { get; set; }
    //}



    public class SportDataApiResponse_GetMatchesForSeason
    {
        public SportDataApiResponse_GetMatchesForSeason_Query query { get; set; }
        public SportDataApiResponse_GetMatchesForSeason_Data[] data { get; set; }
    }

    public class SportDataApiResponse_GetMatchesForSeason_Query
    {
        public string apikey { get; set; }
        public string season_id { get; set; }
    }

    public class SportDataApiResponse_GetMatchesForSeason_Data
    {
        public int match_id { get; set; }
        public int status_code { get; set; }
        public string status { get; set; }
        public string match_start { get; set; }
        public int? minute { get; set; }
        public int league_id { get; set; }
        public int season_id { get; set; }
        public Round round { get; set; }
        public int? referee_id { get; set; }
        public SportDataApiResponse_GetMatchesForSeason_Home_Team home_team { get; set; }
        public SportDataApiResponse_GetMatchesForSeason_Away_Team away_team { get; set; }
        public Stats stats { get; set; }
        public Venue venue { get; set; }
    }

    public class Round
    {
        public int round_id { get; set; }
        public string name { get; set; }
        public int? is_current { get; set; }
    }

    public class SportDataApiResponse_GetMatchesForSeason_Home_Team
    {
        public int team_id { get; set; }
        public string name { get; set; }
        public string short_code { get; set; }
        public string logo { get; set; }
        public SportDataApiResponse_GetMatchesForSeason_Country country { get; set; }
    }

    public class SportDataApiResponse_GetMatchesForSeason_Country
    {
        public int country_id { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string continent { get; set; }
    }

    public class SportDataApiResponse_GetMatchesForSeason_Away_Team
    {
        public int team_id { get; set; }
        public string name { get; set; }
        public string short_code { get; set; }
        public string logo { get; set; }
        public SportDataApiResponse_GetMatchesForSeason_Country1 country { get; set; }
    }

    public class SportDataApiResponse_GetMatchesForSeason_Country1
    {
        public int country_id { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string continent { get; set; }
    }

    public class Stats
    {
        public int home_score { get; set; }
        public int away_score { get; set; }
        public string ht_score { get; set; }
        public string ft_score { get; set; }
        public object et_score { get; set; }
        public object ps_score { get; set; }
    }

    public class Venue
    {
        public int venue_id { get; set; }
        public string name { get; set; }
        public int capacity { get; set; }
        public string city { get; set; }
        public int country_id { get; set; }
    }


    #endregion
    #endregion
}
