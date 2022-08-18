using Store_TechniaclTask.DAL.Enums;
using Store_TechniaclTask.DAL.Model.Identity;
using Store_TechniaclTask.Services.ViewModels.DTO;
using Store_TechniaclTask.Services.ViewModels.IdentityViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Store_TechniaclTask.Services.ViewModels;

namespace Store_TechniaclTask.Services.ModelServices.Abstraction.Identity
{
    public interface IApplicationUserService
    {
        Task<string> GeneratePassResetToken(ApplicationUser user);
        Task<string> GetUserPermissionsbyControllerName(ClaimsPrincipal claimsPrincipal, string ControllerName);
        Task<bool> IsUserHasPermission(ClaimsPrincipal claimsPrincipal,string Controller, SharedPermissions? Permission);
        Task<FixedRoles?> CheckUserFixedRole(ApplicationUser applicationUser);
        Task<CommonResponse<LoginVM>> Login(LoginVM model);
        Task LogOut( );
        Task<CommonResponse<ApplicationUserVM>> ToggleActive(string ID, bool IsActive);
        Task ForceLogOutUsers(string Role);
        Task ForceLogOutUsers(ApplicationUser user);
        Task<ApplicationUser> CurrentUser();
        Task Initialize();
        Task<bool> IsEmailExist(string Email,string UserID=null);
        Task<CommonResponse<ApplicationUserVM>> ChangePassword(ApplicationUser user, ResetPassword resetPassword);
        Task<CommonResponse<ApplicationUserVM>> ChangePassword(ChangePasswordVM changePassword);
        Task<ApplicationUserVM> GetData(string ID);
        IQueryable<ApplicationUser> GetData(Expression<Func<ApplicationUser, bool>> predicate);
        Task<IEnumerable<ApplicationUserVM>> GetData( );

        Task<CommonResponse<ApplicationUserVM>> Create(ApplicationUserVM VM);
        Task<CommonResponse<ApplicationUserVM>> AdminChangePassword(ApplicationUserPasswordDTO model);
        Task<CommonResponse<ApplicationUserVM>> Create(IEnumerable<ApplicationUserVM> VM);
        Task<CommonResponse<ApplicationUserVM>> Edit(ApplicationUserVM VM);
        Task<CommonResponse<ApplicationUserVM>> Remove(string ID);
         Task<CommonResponse<ApplicationUserVM>> Remove(ApplicationUser User);
        Task<(bool IsValid, ApplicationUser User)> IsValidUserAsync(LoginVM userdata);
        IEnumerable<ValidationResult> _ValidationResult(ApplicationUserVM vm);
    }
}
