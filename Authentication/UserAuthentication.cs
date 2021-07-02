using System;
using System.Diagnostics;
using System.Linq;

using WebApiBasicCrud.Data;
using WebApiBasicCrud.Models;

namespace WebApiBasicCrud.Authentication
{
    public class UserAuthentication
    {
        public static User Login(string username, string password)
        {

            using (ProductContext context = new ProductContext())
            {

                //var users = context.Users.ToList();
                //bool isAuth = context.Users.Any(u => u.Username==username && u.Password == password);
                //return isAuth;
                var user = context.Users.FirstOrDefault(u => u.Username.ToLower() == username && u.Password == password);
                return user;
            }

        }
    }
        
}