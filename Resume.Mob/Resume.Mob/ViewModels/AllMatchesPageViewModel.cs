using Resume.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Resume.Mob.ViewModels
{
    public class AllMatchesPageViewModel : ViewModelBase
    {
        public ObservableCollection<LiveMatchViewModel> LiveMatchViewModels { get; set; } = new ObservableCollection<LiveMatchViewModel>();
        public AllMatchesPageViewModel(Func<List<string>, Task> displayAlert) : base(displayAlert)
        {
        }

        async Task GetAllMatches()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                IsLoading = true;
            });

            if (await HasInternet())
            {
                try
                {

                }
                catch (Exception ex)
                {
                    await LogError(ex);
                }
            }

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                IsLoading = false;
            });
        }
    }
}
