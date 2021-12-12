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

            DependencyService.Register<DataStore<Models.Contact>>();
            DependencyService.Register<DataStore<MessageTag>>();
            DependencyService.Register<DataStore<Message>>();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            // Trigger CreateTable. Find a better solution
            new DataStore<Models.Contact>();
            new DataStore<MessageTag>();
            new DataStore<Message>();

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
