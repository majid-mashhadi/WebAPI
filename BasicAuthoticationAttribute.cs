using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Text;
using System.Threading;
using System.Security.Principal;
using System.Net.Http;
using System.Net;

namespace Helper.Attributes
{
    public class BasicAuthoticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                string authorization = actionContext.Request.Headers.Authorization.Parameter;
                authorization = Encoding.UTF8.GetString(Convert.FromBase64String(authorization));

                string[] userNamePassword = authorization.Split(':');
                if (userNamePassword.Length == 2)
                {
                    string userName = userNamePassword[0];
                    string password = userNamePassword[1];
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userName), null);
                }
                else
                {
                    base.OnAuthorization(actionContext);
                }

            }
        }
    }
}