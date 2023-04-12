using MobileApplicationDevelopment.Models;
using MobileApplicationDevelopment.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace MobileApplicationDevelopment.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Dashboard : ContentPage
    {
        public bool notificationsShown;
         public Dashboard()
        {
            
            InitializeComponent();
            
            
            
        }

        private void termCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.CurrentSelection != null)
            {
                Navigation.PushAsync(new TermDetails((Term)e.CurrentSelection.FirstOrDefault()));
            }
            
        }

        async private  void ContentPage_Appearing(object sender, EventArgs e)
        {
            if (!Settings.FirstRun)
            {
                if(notificationsShown != true)
                {
                    await DatabaseServices.ShowCurrentNotifications();
                    notificationsShown = true;
                }
                
            }
            else
            {
                
                
                await RefreshTermCollectionView();
                await DatabaseServices.ShowCurrentNotifications();
                notificationsShown = true;
            }


            await RefreshTermCollectionView();
            
        }

        private void AddTermButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new TestPage());
        }
        async private Task RefreshTermCollectionView()
        {
            System.Threading.Thread.Sleep(1000);
            termCollectionView.ItemsSource = await DatabaseServices.GetTerms();
        }

        private async void LoadDataButton_Clicked(object sender, EventArgs e)
        {
            await DatabaseServices.PopulateDB();
            await RefreshTermCollectionView();
        }

        private async void ClearDatabaseButton_Clicked(object sender, EventArgs e)
        {
            await DatabaseServices.ClearDB();
            RefreshTermCollectionView();
        }
    }
}