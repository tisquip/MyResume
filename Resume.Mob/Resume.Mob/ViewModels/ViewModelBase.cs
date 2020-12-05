using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Resume.Mob.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Func<List<string>, Task> displayAlert;

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set 
            {
                if (isLoading == value)
                    return;

                isLoading = value;
                OnPropertyChanged(); 
            }
        }

        public ViewModelBase(Func<List<string>, Task> displayAlert)
        {
            this.displayAlert = displayAlert;
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected async Task DisplayAlert(List<string> notices)
        {
            if (displayAlert == null)
                return;

            if (notices?.Any() ?? false)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await displayAlert(notices);
                });
            }
        }

        protected async Task<bool> HasInternet(bool displayNoInternetNotice = true)
        {
            bool hasInternet = Connectivity.NetworkAccess == NetworkAccess.Internet;
            if (!hasInternet && displayNoInternetNotice)
            {
                await DisplayAlert(new List<string>(){ "An internet connection is required for some of the services to function properly" });
            }
            return hasInternet;
        }

        protected async Task LogError(Exception ex, bool displayRealError = false)
        {
            //TODO: Log error and request to send error to developer
            string errorMessage = displayRealError ? ex.Message : "An unxpected error occured";
            await DisplayAlert(new List<string>() { errorMessage });
        }
    }
}
