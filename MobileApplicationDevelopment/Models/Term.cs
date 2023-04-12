using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace MobileApplicationDevelopment.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
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

        public Term(string title, DateTime start, DateTime end)
        {
            
            Title = title;
            StartDate = start;
            EndDate = end;
            
        }
        public Term()
        {

        }
    }
}
