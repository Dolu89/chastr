using Chastr.Utils;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Chastr.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            Setup();
        }

        private async void Setup()
        {
            var privateKey = await SecureStorage.GetAsync(Constants.PRIVATE_KEY);
            if (string.IsNullOrEmpty(privateKey))
            {
                await Navigation.PushModalAsync(new InitPage());
            }
        }

    }
}