using Resume.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resume.Web.Components.LiveMatchViewModelDisplay
{
    public class LiveMatchViewModelDisplayComponentOptions
    {
        public LiveMatchViewModel LiveMatchViewModel { get; set; }
        public LiveMatchViewModelDisplayComponentOptions()
        {
        }

        public LiveMatchViewModelDisplayComponentOptions(LiveMatchViewModel liveMatchViewModel)
        {
            LiveMatchViewModel = liveMatchViewModel;
        }
    }
}
