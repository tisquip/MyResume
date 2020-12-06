using Microsoft.AspNetCore.SignalR.Client;
using Resume.Application;
using Resume.Application.ViewModels;
using Resume.Domain.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Resume.Domain;

namespace Resume.Mob.Services
{
    public class SignalRService : IDisposable
    {
        #region Singleton
        static SignalRService _instance;
        static object _lock = new object();
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
        public event EventHandler<LiveMatchViewModel> LiveMatchViewModelRecieved;
        private SignalRService()
        {
        }
        async Task Connect()
        {
            try
            {
                if (await InternetServices.HasInternet((list) => Task.CompletedTask))
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
                vtr = ListenToEndPoint();
               
            }
            catch (Exception)
            {
                vtr.SetError(new List<string>() { "Could not connect to order services" } );
            }

            return vtr;
        }

        public Result ListenToEndPoint()
        {
            Result vtr = new Result(true);
            if (hubConnection != null && hubConnection.State == HubConnectionState.Connected && Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                hubConnection.Remove(Variables.SignalRMethodName_LiveMatch);
                hubConnection.On<string>(Variables.SignalRMethodName_LiveMatch, (jsLiveMatchViewModel) =>
                {
                    ProcessLiveMatchViewModel(jsLiveMatchViewModel);
                });
                vtr.SetSuccess();
            }
            return vtr;
        }

        public static async Task Initialize()
        {
            try
            {
                _ = await GetSignalRService();
            }
            catch (Exception)
            {
            }
        }

        public static void StopListeningToEndPoint()
        {
            if (_instance == null)
                return;

            _instance.StopListeningToEndPointPrivate();
        }

        void StopListeningToEndPointPrivate()
        {
            Result vtr = new Result(true);
            if (hubConnection != null && hubConnection.State == HubConnectionState.Connected)
            {
                hubConnection.Remove(Variables.SignalRMethodName_LiveMatch);
            }
        }

        private void ProcessLiveMatchViewModel(string jsLiveMatchViewModel)
        {
            LiveMatchViewModel liveMatchViewModel = jsLiveMatchViewModel?.DeserializeFromJson<LiveMatchViewModel>();
            if (liveMatchViewModel == null)
                return;

            LiveMatchViewModelRecieved?.Invoke(this, liveMatchViewModel);
        }

        public void Dispose()
        {
            if (hubConnection != null)
            {
                _ = hubConnection.DisposeAsync();
            }
        }
    }
}
