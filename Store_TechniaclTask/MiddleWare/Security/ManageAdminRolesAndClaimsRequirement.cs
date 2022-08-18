using Store_TechniaclTask.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _Store_TechniaclTask.Web.MiddleWare.Security
{
    public class ManageAdminRolesAndClaimsRequirement: IAuthorizationRequirement
    {
        public ManageAdminRolesAndClaimsRequirement(string permission)
        {
            Permission = permission;
        }

        public string Permission { get; }
    }
}
