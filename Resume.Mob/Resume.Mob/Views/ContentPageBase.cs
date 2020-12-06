using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Resume.Mob.Views
{
    public class ContentPageBase : ContentPage
    {
        protected async Task DisplayNotice(List<string> notices)
        {
            if (notices?.Any() ?? false)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await DisplayAlert("Notice", String.Join("\n", notices), "Ok");
                });
            }
        }
    }
}
