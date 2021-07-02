using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using WebApiBasicCrud.Data;
using WebApiBasicCrud.Models;

namespace WebApiBasicCrud.Controllers
{
    public class UsersController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage Login([FromBody] User user) 
        {
            //if (!ModelState.IsValid)
            //{
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, "Data entered is wrong");
            //}
           using(ProductContext context = new ProductContext())
            {
                var savedUser = context.Users
                    .FirstOrDefault
                    (u => u.Username.ToLower() == user.Username.ToLower() && u.Password == user.Password);
                if (savedUser == null)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Wrong username or password");
                }
                string basicAuthToken = $"{savedUser.Username}:{savedUser.Password}";
                byte[] bA = Encoding.ASCII.GetBytes(basicAuthToken);
                basicAuthToken = Convert.ToBase64String(bA);
                var userDataToReturn = new { savedUser.Id, savedUser.Role, savedUser.Username, basicAuthToken };
                return Request.CreateResponse(HttpStatusCode.OK, userDataToReturn); 

            }
        }
        [HttpPost]
        [Route("api/Users/Register")]
        public HttpResponseMessage Register([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Data entered is wrong");
            }
            using(ProductContext context = new ProductContext())
            {
                bool isUsernameExists = context.Users.Any(u => u.Username.ToLower() == user.Username.ToLower());
                if (isUsernameExists)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "UserName Already Exists");
                }
                context.Users.Add(user);
                context.SaveChanges();
                var resUser = new { user.Id, user.Username,user.Role };
                var msg = Request.CreateResponse(HttpStatusCode.Created, resUser);
                msg.Headers.Location = new Uri(Request.RequestUri.ToString() + resUser.Id);
                return msg;
            }
            

        }
        [HttpGet]
        [Route("api/Users/AuthenticateUser")]
        public HttpResponseMessage AuthenticateUser()
        {
            if (Request.Headers.Authorization == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Not Autherized");
            }
            string authToken = Request.Headers.Authorization.Parameter;
            string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
            string[] authData = decodedToken.Split(':');
            string username = authData[0].ToLower();
            string password = authData[1];
            using(ProductContext context = new ProductContext())
            {
                User user = context.Users.FirstOrDefault(u => u.Username.ToLower() == username && u.Password==password);
                if (user == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Not Autherized");
                }
                var userDataToReturn = new { user.Id, user.Role, user.Username, basicAuthToken=authToken };
                return Request.CreateResponse(HttpStatusCode.OK, userDataToReturn);
            }
        }
    }
}
