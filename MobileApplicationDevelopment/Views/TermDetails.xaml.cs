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
    public partial class TermDetails : ContentPage
    {
        public Term tempTerm;
        public TermDetails(Term term)
        {
            InitializeComponent();
            tempTerm = term;
            titleTextBox.Text = term.Title;
            startDatePicker.Date = term.StartDate;
            endDatePicker.Date = term.EndDate;
            RefreshCourseCollectionView();
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                tempTerm.Title = titleTextBox.Text;
                tempTerm.StartDate = startDatePicker.Date;
                tempTerm.EndDate = endDatePicker.Date;

                await DatabaseServices.dbAsyncConnection.UpdateAsync(tempTerm);
                //navigate back to dashboard
                await Navigation.PopAsync();
            }
        }

        private void AddCourseButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddCoursePage(tempTerm));
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(titleTextBox.Text))
            {
                DisplayAlert("Missing Title", "Please enter a valid title.", "OK");
                return false;
            }
            if (startDatePicker.Date >= endDatePicker.Date)
            {
                DisplayAlert("Invalid Dates", "Your start date must be before your end date.", "OK");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void DeleteButton_Clicked(object sender, EventArgs e)
        {
            DatabaseServices.RemoveTerm(tempTerm);
            Navigation.PopAsync();
        }

        private void courseCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                Navigation.PushAsync(new CourseDetails( (Course)e.CurrentSelection.FirstOrDefault() ));
            }
        }
        private async void RefreshCourseCollectionView()
        {
            System.Threading.Thread.Sleep(1000);
            courseCollectionView.ItemsSource = await DatabaseServices.GetCourses(tempTerm);
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            RefreshCourseCollectionView();
        }

        private  async void courseSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {

            List<Course> searchResults = new List<Course>();

            foreach (Course item in await DatabaseServices.GetCourses(tempTerm))
            {
                if (item.Name.Contains(courseSearchBar.Text))
                {
                    searchResults.Add(item);
                }
            }
            courseCollectionView.ItemsSource = searchResults;
        }
    }
}