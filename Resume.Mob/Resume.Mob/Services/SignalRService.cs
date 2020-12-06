using Microsoft.AspNetCore.SignalR.Client;
using Resume.Application;
using Resume.Application.ViewModels;
using Resume.Domain.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Resume.Domain;

namespace Resume.Mob.Services
{
    public class SignalRService 
    {
        #region Singleton
        static SignalRService _instance;
        static object _lock;
        public static async Task<SignalRService> GetSignalRService()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new SignalRService();
                    }
                }
            }
                
            await _instance.InitializeIfNeedBe();
            return _instance;
        }
        #endregion


       

        HubConnection hubConnection;
        bool isSubscribed = false;
        public event EventHandler<LiveMatchViewModel> LiveMatchViewModelRecieved;
        private SignalRService()
        {
        }
        async Task Connect()
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    if (hubConnection == null)
                    {
                        hubConnection = new HubConnectionBuilder()
                           .WithUrl($"{InternetServices.SanitizeMobileUrlForEmulator(URLS.HubLiveMatchEndpoint)}")
                           .WithAutomaticReconnect()
                           .Build();

                        await hubConnection.StartAsync();

                    }
                    else if (hubConnection.State == HubConnectionState.Disconnected)
                    {
                        await hubConnection.StartAsync();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        async Task<Result> InitializeIfNeedBe()
        {
            Result vtr = new Result();
            vtr.SetError();
            try
            {
                await Connect();

                if (hubConnection != null && hubConnection.State == HubConnectionState.Connected)
                {
                    if (!isSubscribed)
                    {
                        hubConnection.On<string>(Variables.SignalRMethodName_LiveMatch, async (jsLiveMatchViewModel) =>
                        {
                            ProcessLiveMatchViewModel(jsLiveMatchViewModel);
                        });
                        isSubscribed = true;
                    }
                    vtr.SetSuccess();
                }
            }
            catch (Exception)
            {
                vtr.SetError(new List<string>() { "Could not connect to order services" } );
            }

            return vtr;
        }

        private void ProcessLiveMatchViewModel(string jsLiveMatchViewModel)
        {
            LiveMatchViewModel liveMatchViewModel = jsLiveMatchViewModel?.DeserializeFromJson<LiveMatchViewModel>();
            if (liveMatchViewModel == null)
                return;

            LiveMatchViewModelRecieved?.Invoke(this, liveMatchViewModel);
        }
    }
}
