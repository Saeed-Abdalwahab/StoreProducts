
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Store_TechniaclTask.DAL.Enums;

namespace Store_TechniaclTask.DAL.Model.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            UserRoles = new HashSet<ApplicationUserRole>();
            ShoppingStores = new HashSet<ShoppingStore>();
    
        }
        public string FirstName { get; set; }
        public string ProfileImage { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime RegistrationDate { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ShoppingStore> ShoppingStores { get; set; }


    }
}
