using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProselApp.Models.Const;

namespace ProselApp.Libraries.Filter
{
    public sealed class UserAuthorizationAttribute : Attribute//, IAuthorizationFilter
    {
        // private readonly string accessType;
        // private ILoginService loginSvc;
        // public UserAuthorizationAttribute(string accessType = AccessType.User)
        // {
        //     this.accessType = accessType;
        // }
        // public void OnAuthorization(AuthorizationFilterContext context)
        // {
        //     loginSvc = (ILoginService)context.HttpContext.RequestServices.GetService(typeof(ILoginService));
        //     var usuario = loginSvc.GetUser();
        //     if (usuario == null || (usuario.AccessType == AccessType.User && accessType == AccessType.Administrator))
        //     {
        //         context.Result = new RedirectToActionResult("Login", "Usuarios", null);//StatusCodeResult(403);
        //     }
        //     else if (usuario.AccountStatus == Status.Disabled)
        //     {
        //         context.Result = new RedirectToActionResult("AtivarConta", "Usuarios", null);
        //     }
        // }
    }
}