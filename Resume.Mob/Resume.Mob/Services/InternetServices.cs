using Resume.Application;
using Resume.Application.ProtoAdapters;
using Resume.Application.ViewModels;
using Resume.Grpc;
using Resume.Grpc.Protos.Football;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace Resume.Mob.Services
{
    public static class InternetServices
    {
        public static async Task<List<LiveMatchViewModel>> GetAllLiveMatches()
        {
            List<LiveMatchViewModel> vtr = new List<LiveMatchViewModel>();
            try
            {
                ProtoMessageRequestMatches protoMessageRequestMatches = new ProtoMessageRequestMatches() { TeamId = Variables.ArsenalTeamId };
                var resultProto = await GrpcClients.GrpcClientFootballMatch(SanitizeMobileUrlForEmulator(URLS.Server)).RequestMatchesForTeamAsync(protoMessageRequestMatches);

                resultProto.ListOfLiveMatchViewModel.ForEach(lm => vtr.Add(ProtoMessageLiveMatchViewModel_LiveMatchViewModel_Adapter.Map(lm)));
            }
            catch (Exception ex)
            {
                //LAZY: Should be caught by the caller
                throw;
            }
            return vtr ?? new List<LiveMatchViewModel>();
        }

        static string SanitizeMobileUrlForEmulator(string url)
        {
            string vtr = url;
            if (vtr.Contains("localhost"))
            {
                vtr = vtr.Replace("localhost", "10.0.0.1");
            }
            return vtr;
        }
    }
}
