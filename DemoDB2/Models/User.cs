using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace DemoDB2.Models
{
    public class User
    {
        MainDB2 db = new MainDB2();

        public bool CheckUserName(string userName)
        {
            return db.Customers.Count(p => p.UserName == userName) > 0;
        }
        public bool CheckEmail(string email)
        {
            return db.Customers.Count(p => p.EmailCus == email) > 0;
        }

        public bool CheckAdmin(string userAdmin)
        {
            return userAdmin.Contains("@admin");
        }
    }
}