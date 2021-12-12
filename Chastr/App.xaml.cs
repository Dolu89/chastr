using Chastr.Models;
using Chastr.Services;
using Chastr.Websocket;
using Xamarin.Forms;

namespace Chastr
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<DataStore<Item>>();
            DependencyService.Register<DataStore<Contact>>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            RelaysPool.Startup();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
