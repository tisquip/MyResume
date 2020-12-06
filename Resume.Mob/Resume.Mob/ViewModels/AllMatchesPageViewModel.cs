using Resume.Application.ViewModels;
using Resume.Mob.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Resume.Mob.ViewModels
{
    public class AllMatchesPageViewModel : ViewModelBase
    {
        public ObservableCollection<LiveMatchViewModel> LiveMatchViewModels { get; set; } = new ObservableCollection<LiveMatchViewModel>();
        public AllMatchesPageViewModel(Func<List<string>, Task> displayAlert) : base(displayAlert)
        { 
        }

        public async Task GetAllMatches()
        {
            if (!IsLoading)
            {
                await SetIsLoading(true);
                try
                {
                    List<LiveMatchViewModel> liveMatchViewModels = await InternetServices.GetAllLiveMatches(DisplayAlert);
                    if (liveMatchViewModels?.Any() ?? false)
                    {
                        liveMatchViewModels = liveMatchViewModels.OrderBy(lm => lm.StartTime).ToList();
                        await SetOnMainThread(() =>
                        {
                            LiveMatchViewModels.Clear();
                            liveMatchViewModels.ForEach(lm => LiveMatchViewModels.Add(lm));
                        });
                    }
                }
                catch (Exception ex)
                {
                    await LogError(ex);
                }

                await SetIsLoading(false);
            }
           
        }
    }
}
