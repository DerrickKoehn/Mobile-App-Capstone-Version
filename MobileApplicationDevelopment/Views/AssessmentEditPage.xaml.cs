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
    public partial class AssessmentEditPage : ContentPage
    {
        public Assessment TempAssessment;
        public Course TempCourse;
        public AssessmentEditPage(Assessment assessment, Course course)
        {
            InitializeComponent();
            TempCourse = course;
            TempAssessment = assessment;
            nameTextBox.Text = TempAssessment.Name;
            startDatePicker.Date = TempAssessment.StartDate;
            endDatePicker.Date = TempAssessment.EndDate;
            assessmentTypePicker.Text = TempAssessment.AssessmentType;
            startNotificationSwitch.IsToggled = TempAssessment.StartDateNotification;
            endNotificationSwitch.IsToggled = TempAssessment.DueDateNotification;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                
                
                    TempAssessment.Name = nameTextBox.Text;
                    TempAssessment.StartDate = startDatePicker.Date;
                    TempAssessment.EndDate = endDatePicker.Date;
                    TempAssessment.AssessmentType = assessmentTypePicker.Text;
                    TempAssessment.StartDateNotification = startNotificationSwitch.IsToggled;
                    TempAssessment.DueDateNotification = endNotificationSwitch.IsToggled;


                    await DatabaseServices.dbAsyncConnection.UpdateAsync(TempAssessment);

                    await Navigation.PopAsync();
                
            }
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            DatabaseServices.RemoveAssessment(TempAssessment);
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