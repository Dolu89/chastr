using Chastr.Utils;
using Chastr.ViewModels.Messages;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Chastr.Views.Messages
{
    public partial class MessagesPage : ContentPage
    {
        MessagesViewModel _viewModel;

        public MessagesPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new MessagesViewModel();
            Setup();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
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