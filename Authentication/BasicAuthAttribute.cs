using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net;
using System.Text;
using System.Threading;
using System.Security.Principal;
using WebApiBasicCrud.Models;

namespace WebApiBasicCrud.Authentication
{
    public class BasicAuthAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if(actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,"Authorization header is not present");
            }
            else
            {
                string authToken = actionContext.Request.Headers.Authorization.Parameter;
                string decodedAuthToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));
                string[] authData = decodedAuthToken.Split(':');
                string username = authData[0];
                string password = authData[1];
                User user = UserAuthentication.Login(username, password);
                if (user!=null)
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(user.Role), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "The username or password is wrong");
                }
            }
        }
    }
}