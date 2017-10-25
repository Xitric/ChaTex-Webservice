using Business.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Authentication
{
    public class ChaTexAuthorization : ActionFilterAttribute, IAuthorizationFilter
    {
        public const string UserIdKey = "userId";
        public const string TokenKey = "token";

        private readonly IAuthenticator authenticator;

        public ChaTexAuthorization(IAuthenticator authenticator)
        {
            this.authenticator = authenticator;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers[TokenKey];
            int? userId = authenticator.AuthenticateGetId(token);

            if (userId != null)
            {
                context.HttpContext.Items[UserIdKey] = userId;
            } else
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
