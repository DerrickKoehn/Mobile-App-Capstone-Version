using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileApplicationDevelopment.Services;
using MobileApplicationDevelopment.Models;

namespace MobileApplicationDevelopment.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            InitializeComponent();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {

            if (ValidateInput())
            {
                //create a term using user input from the form
                Term tempTerm = new Term(titleTextBox.Text, startDatePicker.Date, endDatePicker.Date);
                //save that in the database
                await DatabaseServices.dbAsyncConnection.InsertAsync(tempTerm);
                //navigate back to dashboard
                await Navigation.PopAsync();
            }
            

        }
        private bool  ValidateInput()
        {
            if(string.IsNullOrWhiteSpace(titleTextBox.Text))
            {
                DisplayAlert("Missing Title", "Please enter a valid title.", "OK");
                return false;
            }
            if(startDatePicker.Date >= endDatePicker.Date)
            {
                DisplayAlert("Invalid Dates", "Your start date must be before your end date.", "OK");
                return false;
            }
            else
            {
                return true;
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            
        }
    }
}