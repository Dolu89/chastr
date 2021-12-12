using Chastr.ViewModels.Contacts;
using Chastr.ViewModels.Messages;
using Chastr.Views.Messages;
using Xamarin.Forms;

namespace Chastr.Views.Contacts
{
    public partial class ContactDetailPage : ContentPage
    {
        ContactDetailViewModel _viewModel;

        public ContactDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ContactDetailViewModel();
        }

        private async void btnGoToMessages_Clicked(object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(MessagesPage)}?{nameof(MessagesViewModel.ContactPublicKey)}={_viewModel.PubKey}");
        }
    }
}