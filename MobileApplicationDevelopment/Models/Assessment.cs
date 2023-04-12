using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApplicationDevelopment.Models
{
    
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int CourseID { get; set; }
        public string Name { get; set; }
        public string AssessmentType { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateString { get { return StartDate.ToShortDateString(); } }
        public DateTime EndDate { get; set; }
        public string EndDateString { get { return EndDate.ToShortDateString(); } }
        public bool StartDateNotification { get; set; }
        
        public bool DueDateNotification { get; set; }

        public Assessment(int courseId, string name, string assessmentType, DateTime startDate, DateTime endDate, bool startDateNotification, bool dueDateNotification)
        {
            CourseID = courseId;
            Name = name;
            AssessmentType = assessmentType;
            StartDate = startDate;
            EndDate = endDate;
            StartDateNotification = startDateNotification;
            DueDateNotification = dueDateNotification;
        }
        public Assessment() { }

    }
}
