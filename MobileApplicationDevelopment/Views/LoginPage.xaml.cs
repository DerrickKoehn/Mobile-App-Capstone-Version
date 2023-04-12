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
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            if (Settings.FirstRun)
            {
                instructionsLabel.Text = "Please create a new username and password below.";
                submitButton.Text = "Save";
            }
        }
        private async void submitButton_Clicked(object sender, EventArgs e)
        {
            // If the application is being run for the first time, clicking submitButton will attempt to create
            // a new username and password. Otherwise it will be used to authenticate the user.
            if (Settings.FirstRun)
            {
                // The ValidateInput() method is called to check whether the user has entered a valid
                // username and password.
                if (ValidateInput())
                {
                    //if the input is valid, the app will proceed to create a new user from the inputs.
                    await CreateNewUser();
                    await Navigation.PopAsync();
                }
                
            }
            // If the application is not being run for the first time, the login form will be used for authentication.
            else
            {
                if (await DatabaseServices.ValidateUser(userNameTextBox.Text, passwordTextBox.Text))
                {
                    Navigation.InsertPageBefore(new Dashboard(), this);
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Invalid Credentials", "Please check your username and password and try again.", "Okay");
                }
            }
        }
        private async Task CreateNewUser()
        {
            await DatabaseServices.InitializeDB();
            await DatabaseServices.dbAsyncConnection.InsertAsync(new User(userNameTextBox.Text, passwordTextBox.Text));
            Settings.FirstRun = false;
            LoginPage loginPage = new LoginPage();
            Navigation.InsertPageBefore(loginPage, this);
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(userNameTextBox.Text)) // Was a username not provided?
            {
                DisplayAlert("Missing Username", "Please enter a valid username.", "OK");
                return false;
            }
            if (string.IsNullOrWhiteSpace(passwordTextBox.Text)) // Was a password not provided?
            {
                DisplayAlert("Missing Password", "Please enter a valid password.", "OK");
                return false;
            }
            if (passwordTextBox.Text.Length < 6) // Is the password less than six characters long?
            {
                DisplayAlert("Invalid Password", "Please enter password that is at least " +
                    "six characters in length.", "OK");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}