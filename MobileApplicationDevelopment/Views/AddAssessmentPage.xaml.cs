using MobileApplicationDevelopment.Models;
using MobileApplicationDevelopment.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApplicationDevelopment.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAssessmentPage : ContentPage
    {
        public Course tempCourse;
        public AddAssessmentPage(Course course)
        {
            InitializeComponent();
            tempCourse = course;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            //validate form input
            if (ValidateInput())
            {

                
                    Assessment tempAssessment = new Assessment(tempCourse.ID, nameTextBox.Text, assessmentTypePicker.Text,
                        startDatePicker.Date, endDatePicker.Date, startNotificationSwitch.IsToggled, endNotificationSwitch.IsToggled);
                    
                    await DatabaseServices.dbAsyncConnection.InsertAsync(tempAssessment);

                    await Navigation.PopAsync();
                
            }

            
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(nameTextBox.Text))
            {
                DisplayAlert("Missing Name", "Please enter a valid assessment name.", "OK");
                return false;
            }
            if (startDatePicker.Date >= endDatePicker.Date)
            {
                DisplayAlert("Invalid Dates", "Your start date must be before your due date.", "OK");
                return false;
            }

            return true;
        }
    }
}