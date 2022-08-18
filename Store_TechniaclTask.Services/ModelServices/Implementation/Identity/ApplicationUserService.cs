using AutoMapper;
using Store_TechniaclTask.DAL.Enums;
using Store_TechniaclTask.DAL.Model.Identity;
using Store_TechniaclTask.Services.ModelServices.Abstraction.Identity;
using Store_TechniaclTask.Services.Resources;
using Store_TechniaclTask.Services.ViewModels.DTO;
using Store_TechniaclTask.Services.ViewModels.IdentityViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Store_TechniaclTask.Services.ViewModels;
using Store_TechniaclTask.Services.ModelServices.Abstraction;
using Repository.Abstraction;

namespace Store_TechniaclTask.Services.ModelServices.Implementation.Identity
{
    public class ApplicationUserService : IApplicationUserService
    {

        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUnitOfWork unitOfWork;

        private readonly IRepository<ApplicationUser> repository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IApplicationRoleService applicationRoleService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly IDistributedCache distributedCache;
        private readonly LocService locservice;
        public ApplicationUserService(SignInManager<ApplicationUser> signInManager,
            IUnitOfWork unitOfWork,
             IRepository<ApplicationUser> repository,
            IHttpContextAccessor httpContextAccessor,
            RoleManager<ApplicationRole> roleManager,
            IApplicationRoleService applicationRoleService,
            UserManager<ApplicationUser> userManager,
            IMapper mapper, 
            LocService locservice,
            IDistributedCache distributedCache
          )
        {
            this.signInManager = signInManager;
            this.unitOfWork = unitOfWork;
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
            this.roleManager = roleManager;
            this.applicationRoleService = applicationRoleService;
            this.userManager = userManager;
            this.mapper = mapper;
            this.locservice = locservice;
            this.distributedCache = distributedCache;
      
        }
        public async Task<FixedRoles?> CheckUserFixedRole(ApplicationUser applicationUser)
        {
            FixedRoles? fixedRole = null;
            foreach (var item in Enum.GetNames(typeof(FixedRoles)).Select(x => new { key = (int)Enum.Parse<FixedRoles>(x), Value = x }).OrderBy(x => x.key))
            {
                if (await userManager.IsInRoleAsync(applicationUser, item.Value))
                {
                    fixedRole = Enum.Parse<FixedRoles>(item.Value);
                    break;
                }
            }
            return fixedRole;
        }
        public async Task<string> GetUserPermissionsbyControllerName(ClaimsPrincipal claimsPrincipal, string ControllerName)
        {
            
            ControllerName = ControllerName?.ToLower()?.Trim();
            string Data = "";
            var roles = claimsPrincipal.Claims.Where(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Select(x => x.Value);
            foreach (var item in roles)
            {
                var RoleClaims = await applicationRoleService.RoleClaims(item);
                var Permissions = RoleClaims.Where(x => x.ClaimType.ToLower().Trim() == ControllerName.ToLower().Trim()).ToList();
                var value = Permissions.Count > 0 ? Permissions.Select(x => x.ClaimValue).Aggregate((a, b) => a + "," + b) : "";
                Data += value;
                //Data.AddRange(RoleClaims.Where(x => x..Contains(ControllerName)).Select(x => x.Split("/").LastOrDefault()).ToList());
            }
            return Data;
        }
   

        public async Task<bool> IsUserHasPermission(ClaimsPrincipal claimsPrincipal, string Controller, SharedPermissions? Permission)
        {
            if (Permission == null) return true;
            bool Result = false;
            var roles = claimsPrincipal.Claims.Where(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Select(x => x.Value);
            foreach (var item in roles)
            {
                var RoleClaims = await applicationRoleService.RoleClaims(item);
                if (RoleClaims.Any(x => x.ClaimType.Trim().ToLower() == Controller.ToLower().Trim() && x.ClaimValue.Contains(((int)Permission).ToString())))
                {
                    Result = true;
                    break;
                }
   
            }
            return Result;
        }
        public async Task<CommonResponse<ApplicationUserVM>> Create(ApplicationUserVM VM)
        {
            VM.ID = Guid.NewGuid().ToString();
            var Obj = mapper.Map(VM, new ApplicationUser());
            var Result = await userManager.CreateAsync(Obj, VM.Password);
            //await unitOfWork.SaveChangesAsync();
            if (Result.Succeeded)
            {
                if (VM.Role != null)
                {
                    await userManager.AddToRoleAsync(Obj, VM.Role);
                }
                VM.ID = Obj.Id;
                return CommonResponse<ApplicationUserVM>.GetResult(true, locservice.GetLocalizedHtmlString("SavedSuccessfully"), VM);
            }
            else
            {
                return CommonResponse<ApplicationUserVM>.GetResult(false, Result.Errors.FirstOrDefault().Description);
            }
        }
        public async Task<CommonResponse<ApplicationUserVM>> Edit(ApplicationUserVM VM)
        {
            var Obj = await signInManager.UserManager.FindByIdAsync(VM.ID);
            var FixedRoles = Enum.GetValues(typeof(FixedRoles)).Cast<FixedRoles>().Select(x => x.ToString()).ToList();
            if (FixedRoles.Any(x => (userManager.IsInRoleAsync(Obj, x)).GetAwaiter().GetResult() == true && VM.Role != x))
                return CommonResponse<ApplicationUserVM>.GetResult(false, locservice.GetLocalizedHtmlString("CanNotEditUserRole"));
            using (var tran = await unitOfWork.CreatTransactionAsync())
            {
                try
                {
                    //   Obj = mapper.Map(VM, new ApplicationUser());
                    await ForceLogOutUsers(Obj);

                    Obj.Email = VM.Email ?? Obj.Email;

                    Obj.UserName = VM.UserName ?? Obj.UserName;
                    Obj.PhoneNumber = VM.PhoneNumber ?? Obj.PhoneNumber;
                    Obj.FirstName = VM.FirstName ?? Obj.FirstName;
                    var UserRoles = await userManager.GetRolesAsync(Obj);
                    var Result = await signInManager.UserManager.UpdateAsync(Obj);
                    if (Result.Succeeded)
                    {
                        await userManager.RemoveFromRolesAsync(Obj, UserRoles);
                        if (VM.Role != null)
                        {
                            await userManager.AddToRoleAsync(Obj, VM.Role);
                        }
                        await tran.CommitAsync();
                        return CommonResponse<ApplicationUserVM>.GetResult(true, locservice.GetLocalizedHtmlString("SavedSuccessfully"));
                    }
                    else
                    {
                        return CommonResponse<ApplicationUserVM>.GetResult(false, Result.Errors.FirstOrDefault().Description);
                    }
                }
                catch
                {
                    await tran.RollbackAsync();
                    await tran.DisposeAsync();
                    return CommonResponse<ApplicationUserVM>.GetResult(false, locservice.GetLocalizedHtmlString("InvalidData"));

                }
            }

        }

        public async Task<CommonResponse<ApplicationUserVM>> ChangePassword(ApplicationUser user, ResetPassword resetPassword)
        {
            var status = true;
            string Message = locservice.GetLocalizedHtmlString("SavedSuccessfully");

            var Result = await userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (Result.Succeeded == false)
            {
                Message = Result.Errors.FirstOrDefault()?.Description;
                status = false;
            }
            return CommonResponse<ApplicationUserVM>.GetResult(status, Message);
        }  public async Task<CommonResponse<ApplicationUserVM>> ChangePassword(ChangePasswordVM changePassword)
        {
            var status = true;
            string Message = locservice.GetLocalizedHtmlString("SavedSuccessfully");
            var user =await CurrentUser();

            var Result = await userManager.ChangePasswordAsync(user,changePassword.CurrentPassword,changePassword.Password);
            if (Result.Succeeded == false)
            {
                Message = Result.Errors.FirstOrDefault()?.Description;
                status = false;
            }
           await ForceLogOutUsers(user);
            return CommonResponse<ApplicationUserVM>.GetResult(status, Message);
        }
        public async Task<CommonResponse<ApplicationUserVM>> Create(IEnumerable<ApplicationUserVM> VM)
        {

            foreach (var item in VM)
            {
                var Obj = mapper.Map(item, new ApplicationUser());
                await userManager.CreateAsync(Obj, item.Password);
            }
            return CommonResponse<ApplicationUserVM>.GetResult(true, locservice.GetLocalizedHtmlString("SavedSuccessfully"));
        }


        public async Task<CommonResponse<ApplicationUserVM>> Remove(string ID)
        {
            var Obj = await userManager.FindByIdAsync(ID);

            //var UserRoles = await userManager.GetRolesAsync(Obj);
            using (var tran = await unitOfWork.CreatTransactionAsync())
            {
                try
                {
                    //userFCMTokenService.Remove(Obj.UserFCMTokens.Select(x=>new UserFCMToken { }))
                    //   await userManager.RemoveFromRolesAsync(Obj, UserRoles);
                    //await   unitOfWork.SaveChangesAsync();
                       var Result = await userManager.DeleteAsync(Obj);
                    if (Result.Succeeded)
                    {
                        await tran.CommitAsync();
                        return CommonResponse<ApplicationUserVM>.GetResult(true, locservice.GetLocalizedHtmlString("RemovedSuccessfully"));
                    }
                }
                catch(Exception ex)
                {
                await tran.RollbackAsync();
                await tran.DisposeAsync();
                }
            }
            return CommonResponse<ApplicationUserVM>.GetResult(false, repository.GetDependenciesNames(Obj).ToList());

            //var related = Repository.Implementation.Repository<object>.GetDependenciesNamesstatic(Obj).ToList();
            //var Message = string.Format(locservice.GetLocalizedHtmlString("PlzDeleteDataRelatedWithObjForCompleteObjectRemove"),
            //   Obj.UserName,
            //    related.Select(x => locservice.GetLocalizedHtmlString(x).Value).Aggregate((x, y) => x + "," + y));
            //return CommonResponse<ApplicationUserVM>.GetResult(false, Message);
        }
        public async Task<CommonResponse<ApplicationUserVM>> Remove(ApplicationUser User)
        {
            var UserRoles = await userManager.GetRolesAsync(User);

            try
            {
                await userManager.RemoveFromRolesAsync(User, UserRoles);
                var Result = await userManager.DeleteAsync(User);
                if (Result.Succeeded)
                {
                    return CommonResponse<ApplicationUserVM>.GetResult(true, locservice.GetLocalizedHtmlString("RemovedSuccessfully"));
                }
            }
            catch
            {
            }


            return CommonResponse<ApplicationUserVM>.GetResult(false, repository.GetDependenciesNames(User).ToList());
        }
        public async Task<ApplicationUserVM> GetData(string ID)
        {
            var obj = await userManager.FindByIdAsync(ID);
            var Data = mapper.Map(obj, new ApplicationUserVM());
            Data.Role = (await userManager.GetRolesAsync(obj)).FirstOrDefault();
            return Data;
        } 
        public async Task<IEnumerable<ApplicationUserVM>> GetData()
        {

            var obj = await userManager.Users.OrderByDescending(x => x.RegistrationDate).ToListAsync();
            var Data = mapper.Map(obj, new List<ApplicationUserVM>());
            //foreach (var item in obj)
            //{
            //    Data.First(x => x.ID == item.Id).Role = (await userManager.GetRolesAsync(item)).FirstOrDefault();
            //}
            return Data;
        } 
        public async Task<CommonResponse<ApplicationUserVM>> ToggleActive(string ID, bool IsActive)
        {
            var Obj = await userManager.FindByIdAsync(ID);
            if ((await userManager.IsInRoleAsync(Obj, FixedRoles.SuperAdmin.ToString())) == true)
                return CommonResponse<ApplicationUserVM>.GetResult(false, locservice.GetLocalizedHtmlString("CanNotEdit"));
            var Result = await userManager.UpdateAsync(Obj);
            if (Result.Succeeded)
            {
                await ForceLogOutUsers(Obj);
                return CommonResponse<ApplicationUserVM>.GetResult(true, locservice.GetLocalizedHtmlString("SavedSuccessfully"));
            }
            else
            {
                return CommonResponse<ApplicationUserVM>.GetResult(false, locservice.GetLocalizedHtmlString("InvalidData"));
            }
        }
        public IEnumerable<ValidationResult> _ValidationResult(ApplicationUserVM User)
        {
            if (string.IsNullOrEmpty(User.Password) && string.IsNullOrEmpty(User.ID))
            {
                var Message = string.Format(locservice.GetLocalizedHtmlString("Required"));
                yield return new ValidationResult(Message, new[] { nameof(User.Password) });
            }
            if (userManager.Users.Any(x => x.Id != User.ID && User.UserName != null && x.UserName.ToLower().Trim() == User.UserName.ToLower().Trim()))
            {
                var Message = string.Format(locservice.GetLocalizedHtmlString("AlreadyExist"), User.UserName);
                yield return new ValidationResult(Message, new[] { nameof(User.UserName) });
            }
            if (userManager.Users.Any(x => x.Id != User.ID && x.Email.ToLower().Trim() == User.Email.ToLower().Trim()))
            {
                var Message = string.Format(locservice.GetLocalizedHtmlString("AlreadyExist"), User.Email);
                yield return new ValidationResult(Message, new[] { nameof(User.Email) });
            }
            if (userManager.Users.Any(x => x.Id != User.ID && User.PhoneNumber != null &&x.PhoneNumber!=null&& x.PhoneNumber.ToLower().Trim() == User.PhoneNumber.ToLower().Trim()))
            {
                var Message = string.Format(locservice.GetLocalizedHtmlString("AlreadyExist"), User.PhoneNumber);
                yield return new ValidationResult(Message, new[] { nameof(User.PhoneNumber) });
            }
        }
        public async Task<CommonResponse<LoginVM>> Login(LoginVM model)
        {
            var User = (await userManager.FindByNameAsync(model.UserName.Trim()));

            User = User ?? (await userManager.FindByEmailAsync(model.UserName.Trim()));
            if (User == null) return CommonResponse<LoginVM>.GetResult(false, locservice.GetLocalizedHtmlString("PleaseEnterValidUserNameOrEmail"));
       
          
            var result = await signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                return CommonResponse<LoginVM>.GetResult(true, "");
            }
            return CommonResponse<LoginVM>.GetResult(false, locservice.GetLocalizedHtmlString("WrongPassword"));
        }
        public async Task LogOut()
        {
            await signInManager.SignOutAsync();
        }
        public async Task ForceLogOutUsers(string Role)
        {
            await roleManager.GetRoleNameAsync(new ApplicationRole() { Name = Role });
            var allUser = await userManager.GetUsersInRoleAsync(Role);

            foreach (var user in allUser)
            {
                await userManager.UpdateSecurityStampAsync(user);
            }
        }
        public async Task ForceLogOutUsers(ApplicationUser user)
        {

       var tt=     await userManager.UpdateSecurityStampAsync(user);


        }
        public async Task Initialize()
        {
            //var context = (VevaDbContext)serviceProvider.GetService(typeof(VevaDbContext));
            //var apppplicationRoleService = (IApplicationRoleService)serviceProvider.GetService(typeof(IApplicationRoleService));

            try
            {
                string[] roles = Enum.GetValues(typeof(FixedRoles)).Cast<FixedRoles>().Select(x => x.ToString()).ToArray();
                //string[] roles = new string[] { FixedRoles.SuperAdmin.ToString(), FixedRoles.SuperVisor.ToString() };
                foreach (string role in roles)
                {
                    await applicationRoleService.Create(new ApplicationRoleVM() { Name = role, RegistrationDate = DateTime.Now, RoleClaims = new List<RoleClaim>() { new RoleClaim() { ClaimType = "ApplicationRoles", ClaimValue = "1,2,3,4" } } });
                    try
                    {
                        if (role == FixedRoles.SuperAdmin.ToString())
                        {
                            var SuperAmdinRole = await roleManager.FindByNameAsync(role);
                            var SuperAmdinRoleClaims = await roleManager.GetClaimsAsync(SuperAmdinRole);
                            if (!SuperAmdinRoleClaims.Any(x => x.Type == "ApplicationRoles"))
                                await roleManager.AddClaimAsync(SuperAmdinRole, new System.Security.Claims.Claim("ApplicationRoles", "1,2,3,4"));

                        }
                    }
                    catch { }
                }
                var Deafultuser = new ApplicationUser
                {
                    FirstName = "Owner",
                    Email = "Owner@Owner.com",
                    NormalizedEmail = "Owner@Owner.com",
                    UserName = "Owner",
                    NormalizedUserName = "OWNER",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };
                var BackGroundSystem = new ApplicationUser
                {
                    FirstName = "System",
                    Email = "System@System.com",
                    NormalizedEmail = "System@System.com",
                    UserName = "System",
                    NormalizedUserName = "System",
                    PhoneNumber = "+211111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };
                await userManager.CreateAsync(BackGroundSystem);
                if (!userManager.Users.Any(u => u.UserName == Deafultuser.UserName))
                {
                    var password = new PasswordHasher<ApplicationUser>();
                    var hashed = password.HashPassword(Deafultuser, "OwnerPassword");
                    Deafultuser.PasswordHash = hashed;
                    await userManager.CreateAsync(Deafultuser);
                    //var result = userStore.CreateAsync(Deafultuser);
                }
                await AssignRoles(Deafultuser.Email, new string[] { FixedRoles.SuperAdmin.ToString() });
            }
            catch
            {

            }
            //await AssignRoles(BackGroundSystem.Email, new string[] { FixedRoles.BackGroundSystem.ToString() });
            //await SaveChangesAsync();
        }
        async Task AssignRoles(string email, string[] roles)
        {
            //UserManager<ApplicationUser> _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            ApplicationUser user = await userManager.FindByEmailAsync(email);
            IdentityResult result = new IdentityResult();
            try
            {
                result = await userManager.AddToRolesAsync(user, roles);
                //return result;

            }
            catch
            {

            }
            //return result;
        }
        public async Task<ApplicationUser> CurrentUser()
        {
            var Context = httpContextAccessor?.HttpContext;
            //if (Context == null)
            //{
            //    return await userManager.Users.FirstOrDefaultAsync(x => x.UserRoles.Any(xx => xx.Role.Name == FixedRoles.BackGroundSystem.ToString()));
            //}
            return await userManager.GetUserAsync(Context.User);
        }
        public IQueryable<ApplicationUser> GetData(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return userManager.Users.Where(predicate);

        }
        public async Task<bool> IsEmailExist(string Email, string UserID = null)
        {
            var user = await userManager.FindByEmailAsync(Email);
            return user != null && user.Id != UserID;
        }
        public async Task<CommonResponse<ApplicationUserVM>> AdminChangePassword(ApplicationUserPasswordDTO model)
        {
            try
            {
                var user = await userManager.FindByIdAsync(model.ID);
                var RemoveResult = await userManager.RemovePasswordAsync(user);
                var Result = await userManager.AddPasswordAsync(user, model.Password);
                await ForceLogOutUsers(user);
                return CommonResponse<ApplicationUserVM>.GetResult(true, locservice.GetLocalizedHtmlString("PasswordChangedSuccessfully"));

            }
            catch
            {
                return CommonResponse<ApplicationUserVM>.GetResult(false, locservice.GetLocalizedHtmlString("WrongPassword"));
            }
        }

        public async Task<string> GeneratePassResetToken(ApplicationUser user)
        {
            // generate token
            var newToken = await userManager.GeneratePasswordResetTokenAsync(user);
            return newToken;
        }






        public async Task<(bool,ApplicationUser)> IsValidUserAsync(LoginVM userdata)
        {
            if (string.IsNullOrEmpty(userdata?.UserName)) return (false, null);
            var User = (await userManager.FindByNameAsync(userdata.UserName.Trim()));
            User = User ?? (await userManager.FindByEmailAsync(userdata.UserName.Trim()));
            User = User ?? ( userManager.Users.FirstOrDefault(x=>x.PhoneNumber!=null&&x.PhoneNumber.Trim()==userdata.UserName.Trim()));
            var result = await userManager.CheckPasswordAsync(User, userdata.Password??"");
            return (result,User);
        }

        

    }
}
