using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.RegularExpressions;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MobileApplicationDevelopment.Services;
using MobileApplicationDevelopment.Models;

namespace MobileApplicationDevelopment.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCoursePage : ContentPage
    {
        public Term tempTerm;
        public AddCoursePage(Term term)
        {
            InitializeComponent();
            tempTerm = term;
            statusPicker.SelectedItem = statusPicker.Items[0];
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            //validate input in form fields
            if (ValidateInput())
            {
                //get a list of already existing courses for the term in question
                IList<Course> tempListCourses = (IList<Course>)await DatabaseServices.GetCourses(tempTerm);

                //if there are not already six courses in the term, create a new course using the input from form fields
                if(tempListCourses.Count() < 6)
                {
                    Course tempCourse = new Course(tempTerm.ID, titleTextBox.Text, startDatePicker.Date, endDatePicker.Date,
                        statusPicker.SelectedItem.ToString(), instructorNameTextBox.Text, instructorPhoneTextBox.Text,
                        instructorEmailTextBox.Text, notesTextBox.Text, startNotificationSwitch.IsToggled, endNotificationSwitch.IsToggled );
                    //add that course to the database
                    await DatabaseServices.dbAsyncConnection.InsertAsync(tempCourse);
                    //navigate back to the term details page
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Too Many Courses", "You can only add up to six courses per term.", "OK");
                }
            }
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
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
                if(!(instructorPhoneTextBox.Text.Length == 10))
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
    }
}