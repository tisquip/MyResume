using Resume.Application;
using Resume.Application.ViewModels;
using Resume.Mob.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Resume.Mob.ViewModels
{
    public class LiveMatchPageViewModel : ViewModelBase, IDisposable
    {
        SignalRService signalRService;

        public bool NoLiveMatchAvailable { get; set; } = true;

        public bool LiveMatchAvailable => !NoLiveMatchAvailable;

        private LiveMatchViewModel liveMatchViewModel;
        public LiveMatchViewModel LiveMatchViewModel
        {
            get { return liveMatchViewModel; }
            set
            {
                bool changeOtherProperties = liveMatchViewModel?.MatchId != value?.MatchId;

                liveMatchViewModel = value; 
                OnPropertyChanged();

                if (changeOtherProperties)
                {
                    KickOffTime = liveMatchViewModel?.StartTime.ToString();

                    HomeTeamLogo = liveMatchViewModel?.HomeTeamLogo;
                    HomeTeamName = liveMatchViewModel?.HomeTeamName;

                    AwayTeamLogo = liveMatchViewModel?.AwayTeamLogo;
                    AwayTeamName = liveMatchViewModel?.AwayTeamName;

                    OnPropertyChanged(nameof(KickOffTime));
                    OnPropertyChanged(nameof(HomeTeamLogo));
                    OnPropertyChanged(nameof(HomeTeamName));
                    OnPropertyChanged(nameof(AwayTeamLogo));
                    OnPropertyChanged(nameof(AwayTeamName));
                }

                if (NoLiveMatchAvailable && value != null)
                {
                    NoLiveMatchAvailable = false;
                    SetOnPropertChangedForMatchAvailability();
                }
                else if (!NoLiveMatchAvailable && value == null)
                {
                    NoLiveMatchAvailable = true;
                    SetOnPropertChangedForMatchAvailability();
                }

                void SetOnPropertChangedForMatchAvailability()
                {
                    OnPropertyChanged(nameof(NoLiveMatchAvailable));
                    OnPropertyChanged(nameof(LiveMatchAvailable));
                }
            }
        }

        public string HomeTeamName { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamName { get; set; }
        public string AwayTeamLogo { get; set; }
        public string KickOffTime { get; set; }

        public ICommand CommandOpenWebGoToMainProject { get; set; }
        public LiveMatchPageViewModel(Func<List<string>, Task> displayAlert) : base(displayAlert)
        {
            CommandOpenWebGoToMainProject = new Command(async () => await OpenWebGoToMainProject());
            _ = SetSignalRService();
        }

        async Task OpenWebGoToMainProject()
        {
            try
            {
                await Browser.OpenAsync(URLS.WebApp, BrowserLaunchMode.External);
            }
            catch (Exception)
            {
                //TODO: Implement
            }

        }

        async Task SetSignalRService()
        {
            await SetIsLoading(true);
            try
            {
                signalRService = await SignalRService.GetSignalRService();
                signalRService.LiveMatchViewModelRecieved -= OnLiveViewMatchRecieved;
                signalRService.LiveMatchViewModelRecieved += OnLiveViewMatchRecieved;
            }
            catch (Exception)
            {
                //TODO: Implement
            }
           
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
