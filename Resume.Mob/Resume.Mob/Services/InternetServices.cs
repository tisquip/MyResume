using Resume.Application;
using Resume.Application.ProtoAdapters;
using Resume.Application.ViewModels;
using Resume.Grpc;
using Resume.Grpc.Protos.Football;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace Resume.Mob.Services
{
    public static class InternetServices
    {
        public static async Task<List<LiveMatchViewModel>> GetAllLiveMatches(Func<List<string>, Task> displayNotice)
        {
            if (displayNotice == null)
                displayNotice = (n) => Task.CompletedTask; 

            List<LiveMatchViewModel> vtr = new List<LiveMatchViewModel>();
            try
            {
                if (await HasInternet(displayNotice))
                {
                    ProtoMessageRequestMatches protoMessageRequestMatches = new ProtoMessageRequestMatches() { TeamId = Variables.ArsenalTeamId };
                    var resultProto = await GrpcClients.GrpcClientFootballMatch(SanitizeMobileUrlForEmulator(URLS.Server)).RequestMatchesForTeamAsync(protoMessageRequestMatches);

                    resultProto.ListOfLiveMatchViewModel.ForEach(lm => vtr.Add(ProtoMessageLiveMatchViewModel_LiveMatchViewModel_Adapter.Map(lm)));
                }
            }
            catch (Exception)
            {
                await displayNotice(new List<string>() { "Unknown error occured" });
            }
            return vtr ?? new List<LiveMatchViewModel>();
        }

        public static string SanitizeMobileUrlForEmulator(string url)
        {
            string vtr = url;
            if (vtr.Contains("localhost"))
            {
                vtr = vtr.Replace("localhost", "10.0.2.2");
            }
            return vtr;
        }

        public static async Task<bool> HasInternet(Func<List<string>, Task> displayNotice)
        {
            bool hasInternet = Connectivity.NetworkAccess == NetworkAccess.Internet;
            if (!hasInternet && displayNotice != null)
            {
                await displayNotice(new List<string>() { "An internet connection is required for some of the services to function properly" });
            }
            return hasInternet;
        }
    }
}
