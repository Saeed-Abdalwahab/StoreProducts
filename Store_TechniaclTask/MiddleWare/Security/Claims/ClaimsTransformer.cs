
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Store_TechniaclTask.Services.ModelServices.Abstraction.Identity;
using Store_TechniaclTask.DAL.Model.Identity;

namespace _Store_TechniaclTask.Web.MiddleWare.Security.Claims
{
    public class ClaimsTransformer : IClaimsTransformation
    {
        private readonly IApplicationUserService applicationUserService;
        private readonly RoleManager<ApplicationRole> roleManager;

        public ClaimsTransformer( IApplicationUserService applicationUserService, RoleManager<ApplicationRole> roleManager)
        {
            this.applicationUserService = applicationUserService;
            this.roleManager = roleManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var existingClaimsIdentity = (ClaimsIdentity)principal.Identity;
            var currentUserName = existingClaimsIdentity.Name;

            // Initialize a new list of claims for the new identity
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, currentUserName),

            // Potentially add more from the existing claims here
        };

            // Find the user in the DB
            // Add as many role claims as they have roles in the DB
            //IdentityUser user = await _context.Users.FirstOrDefaultAsync(u => u.UserName.Equals(currentUserName, StringComparison.CurrentCultureIgnoreCase));
            //if (user != null)
            //{
            //    var rolesNames = from ur in _context.UserRoles.Where(p => p.UserId == user.Id)
            //                     from r in _context.Roles
            //                     where ur.RoleId == r.Id
            //                     select r.Name;

            //    claims.AddRange(rolesNames.Select(x => new Claim(ClaimTypes.Role, x)));
            //}

            // Build and return the new principal
            var roles = existingClaimsIdentity.Claims.Where(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Select(x => x.Value);
            foreach (var item in roles)
            {
                var role = await roleManager.FindByNameAsync(item);
                var RoleClaims = await roleManager.GetClaimsAsync(role);
                claims.AddRange(RoleClaims);
            }
            var newClaimsIdentity = new ClaimsIdentity(claims, existingClaimsIdentity.AuthenticationType);
            return new ClaimsPrincipal(newClaimsIdentity);
        }
    }
}
