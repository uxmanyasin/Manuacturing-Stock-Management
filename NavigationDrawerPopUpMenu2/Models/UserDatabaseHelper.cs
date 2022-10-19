using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BizBook.Entities;

namespace BizBook.Models
{  
    class UserDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();

        public Users GetUser(string username,string password)
        {
            Users u = database.UsersDbSet.Where(s => s.UserName == username && s.Password == password).FirstOrDefault();
            return u;
        }
       
        public List<Users> GetUsers(int companyId)
        {
            var data = database.UsersDbSet.Where(s => s.CompanyId == companyId).ToList();
            return data;
        }

        public Users GetUserSet(string password, string Username)
        {
            Users u = database.UsersDbSet.Where(s => s.Password == password && s.UserName == Username).FirstOrDefault();
            return u;
        }
        public Users EditUsername(Users user,string username,string password)
        {
            Users model = GetUserSet(password,username);
            model.UserName = user.UserName;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
            
        }

        public Users EditPassword(Users user,string username,string oldpassword)
        {
            Users model = GetUserSet(oldpassword,username);
            model.Password = user.Password;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
    }
}
