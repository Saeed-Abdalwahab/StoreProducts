using Store_TechniaclTask.DAL.Enums;
using Store_TechniaclTask.DAL.Model.Identity;
using Store_TechniaclTask.Services.CustomAttribute;
using Store_TechniaclTask.Services.ModelServices.Abstraction.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Store_TechniaclTask.Services.HelperServices;
using Store_TechniaclTask.Services.ViewModels.DTO;
using Store_TechniaclTask.Services.Resources;
using System.Text.Json;

namespace _Store_TechniaclTask.Web.MiddleWare.Security
{
    public class AuthorizationHandler : AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LocService locService;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IApplicationUserService applicationUser;

        public AuthorizationHandler(IHttpContextAccessor httpContextAccessor, RoleManager<ApplicationRole> roleManager, IApplicationUserService applicationUser, LocService locService)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.roleManager = roleManager;
            this.applicationUser = applicationUser;
            this.locService = locService;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ManageAdminRolesAndClaimsRequirement requirement)
        {
            if (httpContextAccessor.HttpContext.Request.IsWebApiRequest())
            {
                if (context.User.Claims.Any(x => x.Type == requirement.Permission || x.Value == requirement.Permission))
                {
                    
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            else
            {
                var executingEnpoint = httpContextAccessor.HttpContext.GetEndpoint();
                var attributes = executingEnpoint.Metadata.OfType<CustomAuthorizeAttribute>().FirstOrDefault();
                var urlWithputAction = httpContextAccessor.HttpContext.Request.Path.Value.Remove(httpContextAccessor.HttpContext.Request.Path.Value.LastIndexOf("/"));
                string Permission = urlWithputAction + "/" + requirement.Permission;
                {
                    if ((await applicationUser.IsUserHasPermission(context.User, httpContextAccessor.HttpContext.Request.RouteValues["controller"].ToString(), attributes?.Permission)))
                    {
                        context.Succeed(requirement);
                    }
                }
            }

        }
    }
}
