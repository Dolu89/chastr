using Chastr.Views.Contacts;
using Chastr.Views.Messages;
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
            Routing.RegisterRoute(nameof(MessagesPage), typeof(MessagesPage));
        }

    }
}
