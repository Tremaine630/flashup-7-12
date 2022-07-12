using System;
using System.Threading.Tasks;
using tutor.pagemodels;
using tutor.services.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tutor
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }
        Task InitNavigation()
        {
            var navService = PageModelLocator.resolve<INavigationServices>();
            return navService.NavigatigateToAsync<DashboardPageModel>();
        }
        protected override async void OnStart()
        {
            await InitNavigation();
        }

        protected override void OnSleep()
        {

        }
        protected override void OnResume()
        {

        }
    }
}
