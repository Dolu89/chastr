using Chastr.Models;
using Chastr.ViewModels.Contacts;
using Xamarin.Forms;

namespace Chastr.Views.Contacts
{
    public partial class NewContactPage : ContentPage
    {
        public Contact Contact { get; set; }

        public NewContactPage()
        {
            InitializeComponent();
            BindingContext = new NewContactViewModel();
        }
    }
}