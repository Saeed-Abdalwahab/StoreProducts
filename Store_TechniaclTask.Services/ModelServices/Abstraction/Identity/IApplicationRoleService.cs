using Store_TechniaclTask.Services.ViewModels.DTO;
using Store_TechniaclTask.Services.ViewModels.IdentityViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Store_TechniaclTask.Services.ModelServices.Abstraction.Identity
{
    public interface IApplicationRoleService
    {


        Task<List<RoleClaim>> RoleClaims(string RoleName);
        Task<ApplicationRoleVM> GetData(string ID);
        Task<IEnumerable<ApplicationRoleVM>> GetData();
        Task<CommonResponse<ApplicationRoleVM>> Create(ApplicationRoleVM VM);
   
        Task<CommonResponse<ApplicationRoleVM>> Edit(ApplicationRoleVM VM);
        Task<CommonResponse<ApplicationRoleVM>> Remove(string ID);
        Task<CommonResponse<ApplicationRoleVM>> ToggleActive(string ID,bool IsActive);
        IEnumerable<ValidationResult> _ValidationResult(ApplicationRoleVM vm);

    }
}
