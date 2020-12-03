using Microsoft.AspNetCore.SignalR.Client;
using Resume.Application;
using Resume.Application.ViewModels;
using Resume.Domain;
using System;
using System.Threading.Tasks;

namespace Resume.Web.Services
{
    public class SignalRService : IDisposable
    {
        static SignalRService _instance;
        public static async Task<SignalRService> GetSignalRService()
        {
            if (_instance == null)
                _instance = new SignalRService();

            await _instance.Connect();
            return _instance;
        }


        public event EventHandler<LiveMatchViewModel> OnLiveMatchView;

        HubConnection hubConnection;

        protected SignalRService() { }
        async Task Connect()
        {
            if (hubConnection == null)
            {
                hubConnection = new HubConnectionBuilder()
                   .WithUrl($"{URLS.HubLiveMatchEndpoint}")
                   .WithAutomaticReconnect()
                   .Build();

                hubConnection.On<string>(Variables.SignalRMethodName_LiveMatch, (jsLiveMatchViewModel) => OnLiveMatchViewModelRecieved(jsLiveMatchViewModel));

                await hubConnection.StartAsync();

            }
            else if (hubConnection.State == HubConnectionState.Disconnected)
            {
                await hubConnection.StartAsync();
            }
        }
        void OnLiveMatchViewModelRecieved(string jsLiveMatchViewModel)
        {
            LiveMatchViewModel liveMatchViewModel = jsLiveMatchViewModel.DeserializeFromJson<LiveMatchViewModel>();
            if (liveMatchViewModel != null)
            {
                OnLiveMatchView?.Invoke(this, liveMatchViewModel);
            }
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
