using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProselApp.Models.Const;
using ProselApp.Services.Interfaces;

namespace ProselApp.Libraries.Filter
{
    public sealed class UserAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string accessType;
        private ILoginService loginSvc;
        private ITokenService tokenSvc;
        public UserAuthorizationAttribute(string accessType = AccessType.User)
        {
            this.accessType = accessType;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            loginSvc = (ILoginService)context.HttpContext.RequestServices.GetService(typeof(ILoginService));
            tokenSvc = (ITokenService)context.HttpContext.RequestServices.GetService(typeof(ITokenService));

            var user = loginSvc.GetUser();
            bool tokenIsExpired = false;
            if (user != null)
                tokenIsExpired = tokenSvc.TokenIsExpiredAsync(user).Result;

            if (user == null || (user.AccessType == AccessType.User && accessType == AccessType.Administrator))
            {
                context.Result = new RedirectToActionResult("login", "user", null);
            }
            else if (!user.AccountStatus)
            {
                context.Result = new RedirectToActionResult("ativarconta", "user", null);
            }
            else if (user.AccessType != AccessType.Administrator && tokenIsExpired)
            {
                context.Result = new StatusCodeResult(403);
            }

        }
    }
}