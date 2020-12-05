using Resume.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resume.Web.Components.DisplayComponents
{
    public class DisplayLiveMatchViewModelDisplayComponentOptions
    {
        public LiveMatchViewModel LiveMatchViewModel { get; set; }
        public DisplayLiveMatchViewModelDisplayComponentOptions()
        {
        }

        public DisplayLiveMatchViewModelDisplayComponentOptions(LiveMatchViewModel liveMatchViewModel)
        {
            LiveMatchViewModel = liveMatchViewModel;
        }
    }
}
