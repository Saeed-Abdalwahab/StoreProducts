using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store_TechniaclTask.Services.Resources.CulturHelper
{
    public class UserCultureProvider :RequestCultureProvider
    {
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            await Task.Yield();
          
            var UserCulture =  httpContext.Request.Cookies["Usre_Culture"];
            return new ProviderCultureResult(UserCulture?.ToLower() == "en-US".ToLower() ? "en-US" : "ar-EG");
        }
    }
}
