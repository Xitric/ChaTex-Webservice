using Business.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Authentication
{
    public class RequestAuthenticator
    {
        public const string UserIdKey = "userId";
        public const string TokenKey = "token";

        private readonly RequestDelegate next;
        private readonly IAuthenticator authenticator;

        public RequestAuthenticator(RequestDelegate next, IAuthenticator authenticator)
        {
            this.next = next;
            this.authenticator = authenticator;
        }

        public async Task Invoke(HttpContext context)
        {
            string token = context.Request.Headers[TokenKey];
            int? userId = authenticator.AuthenticateGetId(token);

            context.Items[UserIdKey] = userId;

            await next.Invoke(context);
        }
    }
}
