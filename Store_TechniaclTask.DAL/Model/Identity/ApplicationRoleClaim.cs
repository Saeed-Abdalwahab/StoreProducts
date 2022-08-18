using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.DAL.Model.Identity
{
   public class ApplicationRoleClaim: IdentityRoleClaim<string>
    {
        public static void ConfigreUniqKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationRoleClaim>().HasIndex(p => new
            {
                p.ClaimType,
                p.RoleId,
            }).IsUnique();
        }

    }
}
