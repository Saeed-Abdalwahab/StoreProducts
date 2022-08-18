using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.DAL.Model.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole()
        {

        }
        public ApplicationRole(string RoleName) : base(RoleName)
        {

        }
        public bool IsActive { get; set; }
        public DateTime RegistrationDate { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

    }
   
}
