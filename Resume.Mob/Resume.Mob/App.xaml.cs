using Resume.Mob.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Resume.Mob
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
