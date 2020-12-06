﻿using Resume.Application;
using Resume.Application.ProtoAdapters;
using Resume.Application.ViewModels;
using Resume.Grpc;
using Resume.Grpc.Protos.Football;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace Resume.Mob.Services
{
    public static class InternetServices
    {
        public static async Task<List<LiveMatchViewModel>> GetAllLiveMatches(Func<Task<bool>> internetServiceCheckAndAlertWhenNotAvailable)
        {
            if (internetServiceCheckAndAlertWhenNotAvailable == null)
                internetServiceCheckAndAlertWhenNotAvailable = () => Task.FromResult(true); //<-- It will then just throw the error via the try catch

            List<LiveMatchViewModel> vtr = new List<LiveMatchViewModel>();
            try
            {
                if (await internetServiceCheckAndAlertWhenNotAvailable())
                {
                    ProtoMessageRequestMatches protoMessageRequestMatches = new ProtoMessageRequestMatches() { TeamId = Variables.ArsenalTeamId };
                    var resultProto = await GrpcClients.GrpcClientFootballMatch(SanitizeMobileUrlForEmulator(URLS.Server)).RequestMatchesForTeamAsync(protoMessageRequestMatches);

                    resultProto.ListOfLiveMatchViewModel.ForEach(lm => vtr.Add(ProtoMessageLiveMatchViewModel_LiveMatchViewModel_Adapter.Map(lm)));
                }
            }
            catch (Exception ex)
            {
                //TODO: LAZY: Should be caught by the caller
            }
            return vtr ?? new List<LiveMatchViewModel>();
        }

        public static string SanitizeMobileUrlForEmulator(string url)
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
