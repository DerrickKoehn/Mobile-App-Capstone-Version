using MobileApplicationDevelopment.Models;
using MobileApplicationDevelopment.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApplicationDevelopment.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseDetails : ContentPage
    {
        public Course TempCourse;
        public CourseDetails(Course course)
        {
            InitializeComponent();
            TempCourse = course;
            titleTextBox.Text = course.Name;
            startDatePicker.Date = course.StartDate;
            endDatePicker.Date = course.EndDate;
            statusPicker.SelectedItem = course.CourseStatus;
            instructorNameTextBox.Text = course.CourseInstructorName;
            instructorPhoneTextBox.Text = course.CourseInstructorPhone;
            instructorEmailTextBox.Text = course.CourseInstructorEmail;
            notesTextBox.Text = course.Notes;
            startNotificationSwitch.IsToggled = course.StartNotification;
            endNotificationSwitch.IsToggled = course.EndNotification;

            RefreshAssessmentCollectionView();
            
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                TempCourse.Name = titleTextBox.Text;
                TempCourse.StartDate = startDatePicker.Date;
                TempCourse.EndDate = endDatePicker.Date;
                TempCourse.CourseStatus = statusPicker.SelectedItem.ToString();
                TempCourse.CourseInstructorName = instructorNameTextBox.Text;
                TempCourse.CourseInstructorPhone = instructorPhoneTextBox.Text;
                TempCourse.CourseInstructorEmail = instructorEmailTextBox.Text;
                TempCourse.Notes = notesTextBox.Text;
                TempCourse.StartNotification = startNotificationSwitch.IsToggled;
                TempCourse.EndNotification = endNotificationSwitch.IsToggled;

                await DatabaseServices.dbAsyncConnection.UpdateAsync(TempCourse);
                await Navigation.PopAsync();
            }
        }
        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            DatabaseServices.RemoveCourse(TempCourse);

            await Navigation.PopAsync();

        }
        private void AddAssessmentButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddAssessmentPage(TempCourse));
        }
        private bool ValidateInput()

        {
            if (string.IsNullOrWhiteSpace(titleTextBox.Text))
            {
                DisplayAlert("Missing Title", "Please enter a valid title.", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(instructorNameTextBox.Text))
            {
                DisplayAlert("Missing Instructor Name", "Please enter a valid instructor name.", "OK");
                return false;
            }
            //check whether phone number is null
            if (string.IsNullOrWhiteSpace(instructorPhoneTextBox.Text))
            {
                DisplayAlert("Missing Instructor Phone Number", "Please enter a valid instructor phone number.", "OK");
                return false;
            }
            //check whether phone number is correct length and only numbers
            if (!string.IsNullOrWhiteSpace(instructorPhoneTextBox.Text))
            {
                foreach (char c in instructorPhoneTextBox.Text)
                {
                    if (c < '0' || c > '9')
                    {
                        DisplayAlert("Invalid Phone Number", "Please enter only numbers in the instructor phone field.", "OK");
                        return false;
                    }
                }
                if (!(instructorPhoneTextBox.Text.Length == 10))
                {
                    DisplayAlert("Invalid Phone Number", "Please enter exactly 10 digits in phone number field.", "OK");
                    return false;
                }

            }
            if (string.IsNullOrWhiteSpace(instructorEmailTextBox.Text))
            {
                DisplayAlert("Missing Email", "Please enter a valid course instructor email address.", "OK");
                return false;
            }
            if (!string.IsNullOrWhiteSpace(instructorEmailTextBox.Text))
            {

                string emailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";


                Regex regex = new Regex(emailPattern);

                // Use the regex object to match the email string
                bool isValidEmail = regex.IsMatch(instructorEmailTextBox.Text);
                if (!isValidEmail)
                {
                    DisplayAlert("Invalid Email", "Please enter a valid course instructor email address.", "OK");
                    return false;
                }
            }
            if (startDatePicker.Date >= endDatePicker.Date)
            {
                DisplayAlert("Invalid Dates", "Your start date must be before your end date.", "OK");
                return false;
            }

            return true;
        }
        private void assessmentCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.CurrentSelection != null)
            {
                Navigation.PushAsync(new AssessmentEditPage((Assessment)e.CurrentSelection.FirstOrDefault(), TempCourse));
            }
        }
        private async void RefreshAssessmentCollectionView()
        {
            System.Threading.Thread.Sleep(1000);

            assessmentCollectionView.ItemsSource = await DatabaseServices.GetAssessments(TempCourse);

        }
        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            RefreshAssessmentCollectionView();
        }

        private void ShareNotesButton_Clicked(object sender, EventArgs e)
        {
            ShareTextRequest shareTextRequest = new ShareTextRequest(notesTextBox.Text);

            Share.RequestAsync(shareTextRequest);

        }
    }
}