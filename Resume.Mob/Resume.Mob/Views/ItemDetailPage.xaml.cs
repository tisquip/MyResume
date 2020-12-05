using Resume.Mob.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Resume.Mob.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}