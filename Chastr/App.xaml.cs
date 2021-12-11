using Chastr.Models;
using Chastr.Services;
using Xamarin.Forms;

namespace Chastr
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<DataStore<Item>>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            //RelaysPool.Startup();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
