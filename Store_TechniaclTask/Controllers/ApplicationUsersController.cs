using _Store_TechniaclTask.Web.ExtensionMethods;
using Store_TechniaclTask.DAL.Enums;
using Store_TechniaclTask.DAL.Model.Identity;
using Store_TechniaclTask.Services.CustomAttribute;
using Store_TechniaclTask.Services.Enums;
using Store_TechniaclTask.Services.HelperServices;
using Store_TechniaclTask.Services.HelperServices.Abstraction;
using Store_TechniaclTask.Services.ModelServices.Abstraction;
using Store_TechniaclTask.Services.ModelServices.Abstraction.Identity;
using Store_TechniaclTask.Services.Resources;
using Store_TechniaclTask.Services.ViewModels;
using Store_TechniaclTask.Services.ViewModels.DTO;
using Store_TechniaclTask.Services.ViewModels.IdentityViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _Store_TechniaclTask.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[SystemController(SystemController.ApplicationUsers)]
    public class ApplicationUsersController : Controller
    {
        private readonly IApplicationUserService ApplicationUserService;
        private readonly IApplicationRoleService applicationRoleService;
        private readonly LocService locService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IFilesServices filesServices;

        public ApplicationUsersController(IApplicationUserService ApplicationUserService,
            IApplicationRoleService applicationRoleService  , LocService locService, IHttpContextAccessor httpContextAccessor, IFilesServices filesServices)
        {
            this.ApplicationUserService = ApplicationUserService;
            this.applicationRoleService = applicationRoleService;
            this.locService = locService;
            this.httpContextAccessor = httpContextAccessor;
            this.filesServices = filesServices;
        }
        [CustomAuthorize(SharedPermissions.BrowsPolicy,true)]

        public async Task<IActionResult> Index()
        {
            ViewBag.Roles = (await applicationRoleService.GetData()).Where(role=>!Enum.GetNames(typeof(FixedRoles)).Any(x=>x==role.Name)).Select(x => new SelectListItem { Value = x.Name, Text = x.Name });
            return View();
        }
        [HttpPost]
        [CustomAuthorize(SharedPermissions.CreatePolicy)]

        public async Task<IActionResult> Create(ApplicationUserVM model)
        {
            if (!ModelState.IsValid) return Ok(CommonResponse<ApplicationUserVM>.GetResult(locService.GetLocalizedHtmlString("InvalidModelData"), ModelState.GetModelStateErrors()));
            var Result = await ApplicationUserService.Create(model);
            return Ok(Result);
        } 
        [HttpPost]
        [CustomAuthorize(SharedPermissions.ChangePasswordPolicy)]

        public async Task<IActionResult> AdminChangePassword(ApplicationUserPasswordDTO model)
        {
            if (!ModelState.IsValid) return Ok(CommonResponse<ApplicationUserPasswordDTO>.GetResult(locService.GetLocalizedHtmlString("InvalidModelData"), ModelState.GetModelStateErrors()));
            var Result = await ApplicationUserService.AdminChangePassword(model);
            return Ok(Result);
        }
     
        [HttpPost]
        [CustomAuthorize(SharedPermissions.EditPolicy)]
        public async Task<IActionResult> Edit(ApplicationUserVM model)
        {
            if (!ModelState.IsValid) return Ok(CommonResponse<ApplicationUserVM>.GetResult(locService.GetLocalizedHtmlString("InvalidModelData"), ModelState.GetModelStateErrors()));
            var Result = await ApplicationUserService.Edit(model);
            return Ok(Result);
        }
        [HttpPost]
        [CustomAuthorize(SharedPermissions.CreatePolicy)]
        public async Task<IActionResult> toogleActive(string ID, bool IsActive)
        {
            var Result = await ApplicationUserService.ToggleActive(ID, IsActive);
            return Ok(Result);
        }
        [HttpPost]
        [CustomAuthorize(SharedPermissions.DeletePolicy)]
        public async Task<IActionResult> RemoveObj(string ID)
        {
            var Result = await ApplicationUserService.Remove(ID);
            return Ok(Result);
        }
        [HttpGet]
        [CustomAuthorize(SharedPermissions.BrowsPolicy)]
        public async Task<IActionResult> GetObj(string ID)
        {
            var Result = await ApplicationUserService.GetData(ID);
            return Ok(CommonResponse<ApplicationUserVM>.GetResult(Result));
        }
        [CustomAuthorize(SharedPermissions.BrowsPolicy)]
        public async Task<IActionResult> GetAll()
        {
            var Result = await ApplicationUserService.GetData();
            return Ok(new { data = Result });
        }
    
    }
}
