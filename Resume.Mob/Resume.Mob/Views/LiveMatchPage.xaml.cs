using Resume.Mob.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Resume.Mob.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LiveMatchPage : ContentPageBase
    {
        public LiveMatchPageViewModel ViewModel { get; set; }
        public LiveMatchPage()
        {
            InitializeComponent();
            ViewModel = new LiveMatchPageViewModel(DisplayNotice);
            BindingContext = ViewModel;
        }
    }
}