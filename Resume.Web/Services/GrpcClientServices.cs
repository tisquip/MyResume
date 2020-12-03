using Resume.Application;
using Resume.Application.ProtoAdapters;
using Resume.Application.ViewModels;
using Resume.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resume.Web.Services
{
    public static class GrpcClientServices
    {
        public static async Task<List<LiveMatchViewModel>> GetGames(string teamId)
        {
            List<LiveMatchViewModel> vtr = new List<LiveMatchViewModel>();
            try
            {
                var result = await GrpcClients.GrpcClientFootballMatch(URLS.Server).RequestMatchesForTeamAsync(new Grpc.Protos.Football.ProtoMessageRequestMatches() { TeamId = teamId });
                vtr = result.ListOfLiveMatchViewModel?.Select(lm => ProtoMessageLiveMatchViewModel_LiveMatchViewModel_Adapter.Map(lm))?.ToList() ?? new List<LiveMatchViewModel>();
            }
            catch (Exception)
            {
                //TODO: Notifications or something
            }
            return vtr;
        }
    }
}
