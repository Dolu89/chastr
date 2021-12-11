using Chastr.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Chastr.Views
{
    public partial class ContactDetailPage : ContentPage
    {
        public ContactDetailPage()
        {
            InitializeComponent();
            BindingContext = new ContactDetailViewModel();
        }
    }
}