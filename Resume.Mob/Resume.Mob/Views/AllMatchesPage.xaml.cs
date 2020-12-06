using Resume.Mob.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Resume.Mob.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllMatchesPage : ContentPageBase
    {
        public AllMatchesPageViewModel ViewModel;
        public AllMatchesPage()
        {
            InitializeComponent();
            ViewModel = new AllMatchesPageViewModel(DisplayNotice);
            BindingContext = ViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel != null)
            {
                _ = ViewModel.GetAllMatches();
            }
        }
    }
}