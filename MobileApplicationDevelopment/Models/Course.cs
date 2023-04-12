using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApplicationDevelopment.Models
{
    
    
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int TermID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateString {
            get
            {
                return StartDate.ToShortDateString();
            }
        }
        public string EndDateString
        {
            get
            {
                return EndDate.ToShortDateString();
            }
        }
        public DateTime EndDate { get; set; }
        public string CourseStatus { get; set; }
        public string CourseInstructorName { get; set; }
        public string CourseInstructorPhone { get; set; }
        public string CourseInstructorEmail { get; set; }
        public string Notes { get; set; }
        public bool StartNotification { get; set; }
        public bool EndNotification { get; set; }
        
        public Course(int termId, string name, DateTime startDate, DateTime endDate, string courseStatus, string courseInstructorName,
            string courseInstructorPhone, string courseInstructorEmail, string notes, bool startNotification, bool endNotification)
        {
            TermID = termId;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            CourseStatus = courseStatus;
            CourseInstructorName = courseInstructorName;
            CourseInstructorPhone = courseInstructorPhone;
            CourseInstructorEmail = courseInstructorEmail;
            Notes = notes;
            StartNotification = startNotification;
            EndNotification = endNotification;

        }
        public Course() { }
        
    }
}
