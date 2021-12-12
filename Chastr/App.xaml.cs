using Chastr.Models;
using Chastr.Services;
using Chastr.Utils;
using Chastr.Websocket;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Chastr
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<DataStore<Item>>();
            DependencyService.Register<DataStore<Models.Contact>>();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            var privateKey = await SecureStorage.GetAsync(Constants.PRIVATE_KEY);
            if (!string.IsNullOrEmpty(privateKey))
            {
                RelaysPool.Startup();
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
