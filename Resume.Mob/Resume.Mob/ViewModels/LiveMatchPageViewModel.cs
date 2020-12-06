using Resume.Application.ViewModels;
using Resume.Mob.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Resume.Mob.ViewModels
{
    public class LiveMatchPageViewModel : ViewModelBase, IDisposable
    {
        SignalRService signalRService;
        private bool noLiveMatchAvailable;

        public bool NoLiveMatchAvailable
        {
            get { return noLiveMatchAvailable; }
            set { noLiveMatchAvailable = value; OnPropertyChanged(); }
        }

        private LiveMatchViewModel liveMatchViewModel;

        public LiveMatchViewModel LiveMatchViewModel
        {
            get { return liveMatchViewModel; }
            set { liveMatchViewModel = value; OnPropertyChanged(); }
        }

        public LiveMatchPageViewModel(Func<List<string>, Task> displayAlert) : base(displayAlert)
        {
            _ = SetSignalRService();
        }

        async Task SetSignalRService()
        {
            await SetIsLoading(true);
            signalRService = await SignalRService.GetSignalRService();
            signalRService.LiveMatchViewModelRecieved -= OnLiveViewMatchRecieved;
            signalRService.LiveMatchViewModelRecieved += OnLiveViewMatchRecieved;
            await SetIsLoading(false);
        }

        async void OnLiveViewMatchRecieved(object sender, LiveMatchViewModel liveMatchViewModel)
        {
            await SetOnMainThread(() => LiveMatchViewModel = liveMatchViewModel);
        }

        public void Dispose()
        {
            if (signalRService != null)
            {
                signalRService.LiveMatchViewModelRecieved -= OnLiveViewMatchRecieved;
            }
        }
    }
}
