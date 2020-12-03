using Microsoft.Extensions.Configuration;
using Resume.Domain;
using Resume.Domain.Response;
using Resume.Server.Services.FootballWorkerService.Models;
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
            catch (Exception)
            {
                throw;
            }
            return vtr;
        }

        public async Task<Result<LiveMatchStats>> GetLiveDataForMatch(string matchId)
        {
            //Rotate key cause I accidentally exposed it when hurrying to finish up this class. Errors do happen - but naturally, keys are then supposed to be changed when going into production and/or have separate keys for dev, staging and produc

            //SportDataApiResponse_GetLiveDataForMatch

            Result<LiveMatchStats> vtr = new Result<LiveMatchStats>(true);
            try
            {
                string url = $"{urlBaseSportDataApi}matches/{matchId}?apikey={apiKey}";

                SportDataApiResponse_GetLiveDataForMatch_Alt sportDataApiResponse_GetLiveDataForMatch = null; //
                var httpResponse = await httpClient.GetAsync(url);
                if (httpResponse.IsSuccessStatusCode)
                {
                    string content = await httpResponse.Content.ReadAsStringAsync();
                    sportDataApiResponse_GetLiveDataForMatch = Newtonsoft.Json.JsonConvert.DeserializeObject<SportDataApiResponse_GetLiveDataForMatch_Alt>(content);
                }


                if (sportDataApiResponse_GetLiveDataForMatch?.data == null)
                {
                    vtr.SetError($"No live data found for match with id {matchId}");
                }
                else
                {
                    var liveMatchStatsFromApi = sportDataApiResponse_GetLiveDataForMatch.data;
                    var goalsNormalAndExtraTime = GetScoreExludingPenaltyScore(liveMatchStatsFromApi.stats);
                    var penaltyGoals = GetGoalsFromStringResult(liveMatchStatsFromApi.stats?.ps_score);


                    LiveMatchStats liveMatchStats =
                        new LiveMatchStats(liveMatchStatsFromApi.match_id.ToString(), goalsNormalAndExtraTime.HomeGoals, goalsNormalAndExtraTime.AwayGoals, penaltyGoals.HomeGoals, penaltyGoals.AwayGoals, GetMatchStatus(liveMatchStatsFromApi), liveMatchStatsFromApi.minute.ToString());

                    vtr.SetSuccessObject(liveMatchStats);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return vtr;
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
            catch (Exception)
            {
                throw;
            }
            return vtr;
        }

        public async Task<Result<FootballTeam>> GetTeam(string teamId)
        {
            Result<FootballTeam> vtr = new Result<FootballTeam>(true);
            try
            {
                string url = $"{urlBaseSportDataApi}teams/{teamId}?apikey={apiKey}";

                var sportDataApiResponse_GetTeam = await httpClient.GetFromJsonAsync<SportDataApiResponse_GetTeam>(url);

                if (sportDataApiResponse_GetTeam?.data == null)
                {
                    vtr.SetError($"No team found with id {teamId}");
                }
                else
                {
                    var teamFromApi = sportDataApiResponse_GetTeam.data;
                    FootballTeam footballTeam = new FootballTeam(teamFromApi.name, teamFromApi.team_id.ToString(), teamFromApi.logo);

                    vtr.SetSuccessObject(footballTeam);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return vtr;
        }

        MatchStatus GetMatchStatus(SportDataApiResponse_GetLiveDataForMatch_Alt_Data data)
        {
            int actualCodeFromApi = data?.status_code ?? 0; //0 = Not Started according to SportDataApi docs;
            return actualCodeFromApi switch
            {
                0 => MatchStatus.NotStarted,

                1 or 12 or 13 or 14 => MatchStatus.Started,

                11 => MatchStatus.HalfTime,

                3 or 31 or 32 or 5 => MatchStatus.FullTime,

                _ => MatchStatus.ApiCancelled //<-- Dont want to write logic for a delayed game, postponed game and the like so just going to cancel
            };
        }

        (int HomeGoals, int AwayGoals, bool WasSet) GetScoreExludingPenaltyScore(SportDataApiResponse_GetLiveDataForMatch_Alt_Stats stats)
        {
            (int HomeGoals, int AwayGoals, bool WasSet) vtr = (0, 0, false);
            if (stats?.ht_score != null)
            {
                var ht_score = GetGoalsFromStringResult(stats.ht_score);
                var ft_score = GetGoalsFromStringResult(stats.ft_score);
                var et_score = GetGoalsFromStringResult(stats.et_score);

                vtr.HomeGoals = ht_score.HomeGoals;
                vtr.AwayGoals = ht_score.AwayGoals;

                if (ft_score.HomeGoals > 0 || ft_score.AwayGoals > 0)
                {
                    vtr.HomeGoals = ft_score.HomeGoals;
                    vtr.AwayGoals = ft_score.AwayGoals;
                }

                if (et_score.HomeGoals > 0 || et_score.AwayGoals > 0)
                {
                    vtr.HomeGoals = et_score.HomeGoals;
                    vtr.AwayGoals = et_score.AwayGoals;
                }

                vtr.WasSet = true;
            }

            return vtr;
        }

        (int HomeGoals, int AwayGoals) GetGoalsFromStringResult(string result)
        {
            (int HomeGoals, int AwayGoals) vtr = (0, 0);

            if (!string.IsNullOrWhiteSpace(result) && result.Contains("-"))
            {
                result = result.Trim();
                try
                {
                    int indexOfHyphen = result.IndexOf("-");
                    string homeGoalsString = result.Substring(0, indexOfHyphen)?.Trim();
                    string awayGoalsString = result.Substring(indexOfHyphen + 1)?.Trim();

                    int.TryParse(homeGoalsString, out vtr.HomeGoals);
                    int.TryParse(awayGoalsString, out vtr.AwayGoals);
                }
                catch (Exception)
                {
                }

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
        public int? season_id { get; set; }
        public string name { get; set; }
        public int? is_current { get; set; }
        public int? country_id { get; set; }
        public int? league_id { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }

    #endregion

    #region Responses from GetMatchesForSeason
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
        public int? match_id { get; set; }
        public int? status_code { get; set; }
        public string status { get; set; }
        public string match_start { get; set; }
        public int? minute { get; set; }
        public int? league_id { get; set; }
        public int? season_id { get; set; }
        public Round round { get; set; }
        public int? referee_id { get; set; }
        public SportDataApiResponse_GetMatchesForSeason_Home_Team home_team { get; set; }
        public SportDataApiResponse_GetMatchesForSeason_Away_Team away_team { get; set; }
        public Stats stats { get; set; }
        public Venue venue { get; set; }
    }

    public class Round
    {
        public int? round_id { get; set; }
        public string name { get; set; }
        public int? is_current { get; set; }
    }

    public class SportDataApiResponse_GetMatchesForSeason_Home_Team
    {
        public int? team_id { get; set; }
        public string name { get; set; }
        public string short_code { get; set; }
        public string logo { get; set; }
        public SportDataApiResponse_GetMatchesForSeason_Country country { get; set; }
    }

    public class SportDataApiResponse_GetMatchesForSeason_Country
    {
        public int? country_id { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string continent { get; set; }
    }

    public class SportDataApiResponse_GetMatchesForSeason_Away_Team
    {
        public int? team_id { get; set; }
        public string name { get; set; }
        public string short_code { get; set; }
        public string logo { get; set; }
        public SportDataApiResponse_GetMatchesForSeason_Country1 country { get; set; }
    }

    public class SportDataApiResponse_GetMatchesForSeason_Country1
    {
        public int? country_id { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string continent { get; set; }
    }

    public class Stats
    {
        public int? home_score { get; set; }
        public int? away_score { get; set; }
        public string ht_score { get; set; }
        public string ft_score { get; set; }
        public object et_score { get; set; }
        public object ps_score { get; set; }
    }

    public class Venue
    {
        public int? venue_id { get; set; }
        public string name { get; set; }
        public int? capacity { get; set; }
        public string city { get; set; }
        public int? country_id { get; set; }
    }


    #endregion

    #region Responses from GetTeam

    public class SportDataApiResponse_GetTeam
    {
        public SportDataApiResponse_GetTeam_Query query { get; set; }
        public SportDataApiResponse_GetTeam_Data data { get; set; }
    }

    public class SportDataApiResponse_GetTeam_Query
    {
        public string apikey { get; set; }
    }

    public class SportDataApiResponse_GetTeam_Data
    {
        public int? team_id { get; set; }
        public string name { get; set; }
        public string short_code { get; set; }
        public string logo { get; set; }
        public SportDataApiResponse_GetTeam_Country country { get; set; }
    }

    public class SportDataApiResponse_GetTeam_Country
    {
        public int? country_id { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string continent { get; set; }
    }

    #endregion



    #region Response fromGetLiveDataForMatch

    public class SportDataApiResponse_GetLiveDataForMatch_Alt
    {
        public SportDataApiResponse_GetLiveDataForMatch_Alt_Query query { get; set; }
        public SportDataApiResponse_GetLiveDataForMatch_Alt_Data data { get; set; }
    }

    public class SportDataApiResponse_GetLiveDataForMatch_Alt_Query
    {
        public string apikey { get; set; }
    }

    public class SportDataApiResponse_GetLiveDataForMatch_Alt_Data
    {
        public int? match_id { get; set; }
        public int? league_id { get; set; }
        public SportDataApiResponse_GetLiveDataForMatch_Alt_Round round { get; set; }
        public int? referee_id { get; set; }
        public int? season_id { get; set; }
        public int? status_code { get; set; }
        public string match_start { get; set; }
        public int? minute { get; set; }
        public string status { get; set; }
        public SportDataApiResponse_GetLiveDataForMatch_Alt_Stats stats { get; set; }
        public SportDataApiResponse_GetLiveDataForMatch_Alt_Home_Team home_team { get; set; }
        public SportDataApiResponse_GetLiveDataForMatch_Alt_Away_Team away_team { get; set; }
        public SportDataApiResponse_GetLiveDataForMatch_Alt_Match_Events[] match_events { get; set; }
        public SportDataApiResponse_GetLiveDataForMatch_Alt_Match_Statistics[] match_statistics { get; set; }
        public SportDataApiResponse_GetLiveDataForMatch_Alt_Lineup[] lineups { get; set; }
        public SportDataApiResponse_GetLiveDataForMatch_Alt_Venue venue { get; set; }
    }

    public class SportDataApiResponse_GetLiveDataForMatch_Alt_Round
    {
        public int? round_id { get; set; }
        public string name { get; set; }
        public int? is_current { get; set; }
    }

    public class SportDataApiResponse_GetLiveDataForMatch_Alt_Stats
    {
        public string ht_score { get; set; }
        public string ft_score { get; set; }
        public string et_score { get; set; }
        public string ps_score { get; set; }
    }

    public class SportDataApiResponse_GetLiveDataForMatch_Alt_Home_Team
    {
        public int? team_id { get; set; }
        public string name { get; set; }
        public string short_code { get; set; }
        public string logo { get; set; }
        public SportDataApiResponse_GetLiveDataForMatch_Alt_Country country { get; set; }
    }

    public class SportDataApiResponse_GetLiveDataForMatch_Alt_Country
    {
        public int? country_id { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string continent { get; set; }
    }

    public class SportDataApiResponse_GetLiveDataForMatch_Alt_Away_Team
    {
        public int? team_id { get; set; }
        public string name { get; set; }
        public string short_code { get; set; }
        public string logo { get; set; }
        public SportDataApiResponse_GetLiveDataForMatch_Alt_Country1 country { get; set; }
    }

    public class SportDataApiResponse_GetLiveDataForMatch_Alt_Country1
    {
        public int? country_id { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string continent { get; set; }
    }

    public class SportDataApiResponse_GetLiveDataForMatch_Alt_Venue
    {
        public int? venue_id { get; set; }
        public string name { get; set; }
        public int? capacity { get; set; }
        public string city { get; set; }
        public int? country_id { get; set; }
    }

    public class SportDataApiResponse_GetLiveDataForMatch_Alt_Match_Events
    {
        public int? team_id { get; set; }
        public string type { get; set; }
        public int? player_id { get; set; }
        public string player_name { get; set; }
        public int? related_player_id { get; set; }
        public string related_player_name { get; set; }
        public int? minute { get; set; }
        public object extra_minute { get; set; }
        public object reason { get; set; }
        public object injured { get; set; }
        public bool own_goal { get; set; }
        public bool penalty { get; set; }
        public string result { get; set; }
    }

    public class SportDataApiResponse_GetLiveDataForMatch_Alt_Match_Statistics
    {
        public int? team_id { get; set; }
        public string team_name { get; set; }
        public int? fouls { get; set; }
        public int? injuries { get; set; }
        public int? corners { get; set; }
        public int? offsides { get; set; }
        public int? shots_total { get; set; }
        public int? shots_on_target { get; set; }
        public int? shots_off_target { get; set; }
        public int? shots_blocked { get; set; }
        public object possessiontime { get; set; }
        public object possessionpercent { get; set; }
        public int? yellowcards { get; set; }
        public int? yellowredcards { get; set; }
        public int? redcards { get; set; }
        public int? substitutions { get; set; }
        public int? goal_kick { get; set; }
        public int? goal_attempts { get; set; }
        public int? free_kick { get; set; }
        public int? throw_in { get; set; }
        public int? ball_safe { get; set; }
        public int? goals { get; set; }
        public int? penalties { get; set; }
        public int? attacks { get; set; }
        public int? dangerous_attacks { get; set; }
    }

    public class SportDataApiResponse_GetLiveDataForMatch_Alt_Lineup
    {
        public int? team_id { get; set; }
        public string formation { get; set; }
        public int? formation_confirmed { get; set; }
        public SportDataApiResponse_GetLiveDataForMatch_Alt_Player[] players { get; set; }
    }

    public class SportDataApiResponse_GetLiveDataForMatch_Alt_Player
    {
        public int? player_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string birthday { get; set; }
        public int? age { get; set; }
        public int? weight { get; set; }
        public int? height { get; set; }
        public string img { get; set; }
        public SportDataApiResponse_GetLiveDataForMatch_Alt_Country2 country { get; set; }
    }

    public class SportDataApiResponse_GetLiveDataForMatch_Alt_Country2
    {
        public int? country_id { get; set; }
        public string name { get; set; }
        public string country_code { get; set; }
        public string continent { get; set; }
    }

    #endregion

    #endregion
}
