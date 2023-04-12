using MobileApplicationDevelopment.Models;
using Plugin.LocalNotifications;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MobileApplicationDevelopment.Services
{
    public static class DatabaseServices
    {
        public static SQLiteAsyncConnection dbAsyncConnection;
        public static SQLiteConnection dbConnection;
        async public static Task InitializeDB()
        {
            if (dbAsyncConnection != null)
                return;

            var dbLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DataBase.db");

            dbAsyncConnection = new SQLiteAsyncConnection(dbLocation);

            

            await dbAsyncConnection.CreateTableAsync<Term>();//terms

            await dbAsyncConnection.CreateTableAsync<Course>();//courses

            await dbAsyncConnection.CreateTableAsync<Assessment>();//assessments
            await dbAsyncConnection.CreateTableAsync<User>();//users


        }
         async public static Task PopulateDB()
        {
            await InitializeDB();
            //add a term
            Term tempTerm = new Term("Term1", DateTime.Now, DateTime.Parse("05-01-2023"));
            await dbAsyncConnection.InsertAsync(tempTerm);
            //add a course using term's ID
            Course tempCourse = new Course(tempTerm.ID, "Course 1", DateTime.Now, DateTime.Now.AddDays(5), "Pending",
                "Derrick Koehn", "7157900253", "dkoehn2@wgu.edu",  "no notes yet for this one", true, true);
            await dbAsyncConnection.InsertAsync(tempCourse);
            //add an assessment using the course's ID
            Assessment tempAssessment = new Assessment(tempCourse.ID, "Assessment 1", "Objective", DateTime.Now, DateTime.Now.AddDays(5), true, true);
            await dbAsyncConnection.InsertAsync(tempAssessment);
            //add a second assessment using the course's ID
            Assessment tempAssessment1 = new Assessment(tempCourse.ID, "Assessment 2", "Performance", DateTime.Now, DateTime.Now.AddDays(5), true, true);
            await dbAsyncConnection.InsertAsync(tempAssessment1);
            User user = new User("Evaluator", "password");
            await dbAsyncConnection.InsertAsync(user);
        }
         async public static Task ClearDB()
        {
            await InitializeDB();
            await dbAsyncConnection.DropTableAsync<Term>();
            await dbAsyncConnection.DropTableAsync<Course>();
            await dbAsyncConnection.DropTableAsync<Assessment>();
            await dbAsyncConnection.DropTableAsync<User>();


            dbAsyncConnection = null;
            
            Settings.ClearSettings();
        }
         public async static Task<bool> ValidateUser(string userName, string password)
        {
            await InitializeDB();

            IList<User> matchingUsers = await dbAsyncConnection.Table<User>().Where(i => i.UserName == userName && i.Password == password).ToListAsync();
            if(matchingUsers.Count > 0)
            {
                return true;
            }

            return false;
            
        }
        async public static Task<IEnumerable<Term>> GetTerms()
        {
            await InitializeDB();

            IList<Term> terms = await dbAsyncConnection.Table<Term>().ToListAsync();

            return terms;
        }
        async public static Task<IEnumerable<Course>> GetCourses(Term term)
        {
            await InitializeDB();
            IList<Course> courses = await dbAsyncConnection.Table<Course>().Where(i => i.TermID == term.ID).ToListAsync();

            return courses;
        }
        async public static Task<IEnumerable<Course>> GetCourses()
        {
            await InitializeDB();
            IList<Course> courses = await dbAsyncConnection.Table<Course>().ToListAsync();

            return courses;
        }
        async public static Task<IEnumerable<Assessment>> GetAssessments(Course course)
        {
            await InitializeDB();
            IList<Assessment> assessments = await dbAsyncConnection.Table<Assessment>().Where(i => i.CourseID == course.ID).ToListAsync();

            return assessments;
        }
        async public static Task<IEnumerable<Assessment>> GetAssessments()
        {
            await InitializeDB();
            IList<Assessment> assessments = await dbAsyncConnection.Table<Assessment>().ToListAsync();

            return assessments;
        }
        public async static void RemoveTerm(Term term)
        {
            //Remove any courses in term
            IList<Course> tempListCourses = (IList<Course>)await GetCourses(term);
            foreach(Course item in tempListCourses)
            {
                RemoveCourse(item);
            }
            //Remove the term
            await dbAsyncConnection.DeleteAsync<Term>(term.ID);
        }
        public async static void RemoveCourse(Course course)
        {
            //Remove any assessments in Course
            IList<Assessment> tempListAssessments = (IList<Assessment>)await GetAssessments(course);

            foreach(Assessment item in tempListAssessments)
            {
                RemoveAssessment(item);
            }

            //Remove Course
            await dbAsyncConnection.DeleteAsync<Course>(course.ID);
        }
        public async static void RemoveAssessment(Assessment assessment)
        {
            await InitializeDB();
            //Remove assessment
            await dbAsyncConnection.DeleteAsync<Assessment>(assessment.ID);

        }
        public async static Task ShowCurrentNotifications()
        {
            
            int notificationNumber = 0;
            //Get a list of every course in the database.
            IList<Course> courses = (IList<Course>)await GetCourses();
            //Iterate through each course.
            foreach(Course item in courses)
            {
                //Check whether start notifications are turned on.
                if (item.StartNotification)
                {
                    //Check whether today is the start date
                    if(item.StartDate.Date == DateTime.Now.Date)
                    {
                        //Show a notification saying that the course starts today.
                        CrossLocalNotifications.Current.Show("Course Starting Today", $"A course named {item.Name} begins today.", notificationNumber);
                        notificationNumber++;
                    }
                }
                if (item.EndNotification)
                {
                    //Check whether today is the start date
                    if (item.EndDate.Date == DateTime.Now.Date)
                    {
                        //Show a notification saying that the course starts today.
                        CrossLocalNotifications.Current.Show("Course Ending Today", $"A course named {item.Name} ends today.", notificationNumber);
                        notificationNumber++;
                    }
                }
            }

            //Get a list of every assessment in the database.
            IList<Assessment> assessments = (IList<Assessment>)await GetAssessments();
            //Iterate through each assessment.
            foreach (Assessment item in assessments)
            {
                //Check whether start notifications are turned on.
                if (item.StartDateNotification)
                {
                    //Check whether today is the start date
                    if (item.StartDate.Date == DateTime.Now.Date)
                    {
                        //Show a notification saying that the course starts today.
                        CrossLocalNotifications.Current.Show("Assessment Starting Today", $"An assessment named {item.Name} begins today.", notificationNumber);
                        notificationNumber++;
                    }
                }
                if (item.DueDateNotification)
                {
                    //Check whether today is the start date
                    if (item.EndDate.Date == DateTime.Now.Date)
                    {
                        //Show a notification saying that the course starts today.
                        CrossLocalNotifications.Current.Show("Assessment Due Today", $"An assessment named {item.Name} is due today.", notificationNumber);
                        notificationNumber++;
                    }
                }
            }


        }
    }
}
