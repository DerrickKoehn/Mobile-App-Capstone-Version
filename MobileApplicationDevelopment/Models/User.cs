using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApplicationDevelopment.Models
{
    class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public User(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
        public User()
        {

        }
    }
}
