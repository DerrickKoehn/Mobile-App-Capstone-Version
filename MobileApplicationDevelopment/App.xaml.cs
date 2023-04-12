using MobileApplicationDevelopment.Services;
using MobileApplicationDevelopment.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApplicationDevelopment
{
    public partial class App : Application
    {

        public  App()
        {
            
            InitializeComponent();

            LoginPage loginPage = new LoginPage();
            
            MainPage = new NavigationPage(loginPage);
            
        }

        protected override void OnStart()
        {
            
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
