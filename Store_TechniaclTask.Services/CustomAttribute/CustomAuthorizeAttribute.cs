using Store_TechniaclTask.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Store_TechniaclTask.Services.CustomAttribute
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute( ):base()
        {

        } 
       
        public CustomAuthorizeAttribute(SharedPermissions Permission ,bool IsMainView=false) :base(Permission.ToString())
        {
            this.IsMainView = IsMainView;
            this.Permission = Permission;
        } 
     
        public CustomAuthorizeAttribute(SharedPermissions Permission ,string DisplayName,bool IsMainView=false) :base(Permission.ToString())
        {
            this.IsMainView = IsMainView;
            this.Permission = Permission;
            this.DisplayName = DisplayName;

        }
      
        public bool  IsMainView { get; private set; }
        public SharedPermissions Permission { get; private set; }
        public string DisplayName { get; private set; }
          
        //public  void OnAuthorization(AuthorizationFilterContext context)
        //{
        //    base.OnAuthorization()
        //    var user = context.HttpContext.User;

        //    if (!user.Identity.IsAuthenticated)
        //    {
        //        // it isn't needed to set unauthorized result 
        //        // as the base class already requires the user to be authenticated
        //        // this also makes redirect to a login page work properly
        //        // context.Result = new UnauthorizedResult();
        //        return;
        //    }

        //    // you can also use registered services
        //    var someService = context.HttpContext.RequestServices.GetService<ISomeService>();

        //    var isAuthorized = someService.IsUserAuthorized(user.Identity.Name, _someFilterParameter);
        //    if (!isAuthorized)
        //    {
        //        context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
        //        return;
        //    }
        //}
    }
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    //public class SystemControllerAttribute : Attribute
    //{
    //    public SystemControllerAttribute(SystemController systemController) : base()
    //    {
    //        this.systemController = systemController;
    //    }
    //    public SystemController systemController { get; private set; }

 
    //}
}

