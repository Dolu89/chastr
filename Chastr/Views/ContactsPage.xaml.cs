using Chastr.Utils;
using Chastr.ViewModels.Contacts;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Chastr.Views
{
    public partial class ContactsPage : ContentPage
    {
        ContactsViewModel _viewModel;

        public ContactsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ContactsViewModel();
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