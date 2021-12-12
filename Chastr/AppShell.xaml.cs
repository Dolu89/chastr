using Chastr.Views.Contacts;
using Xamarin.Forms;

namespace Chastr
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ContactDetailPage), typeof(ContactDetailPage));
            Routing.RegisterRoute(nameof(NewContactPage), typeof(NewContactPage));
        }

    }
}
