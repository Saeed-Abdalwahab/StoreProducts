using AutoMapper;
using Store_TechniaclTask.DAL.Context;
using Store_TechniaclTask.DAL.Enums;
using Store_TechniaclTask.DAL.Model.Identity;
using Store_TechniaclTask.Services.HelperServices.Abstraction;
using Store_TechniaclTask.Services.ModelServices.Abstraction.Identity;
using Store_TechniaclTask.Services.Resources;
using Store_TechniaclTask.Services.ViewModels.DTO;
using Store_TechniaclTask.Services.ViewModels.IdentityViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using Store_TechniaclTask.Services.HelperServices;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Repository.Abstraction;

namespace Store_TechniaclTask.Services.ModelServices.Implementation.Identity
{
    public class ApplicationRoleService : IApplicationRoleService
    {

        private readonly RoleManager<ApplicationRole> RoleManager;
        private readonly IDistributedCache distributedCache;
        private readonly IRepository<ApplicationRole> repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly LocService locservice;
        public ApplicationRoleService(RoleManager<ApplicationRole> RoleManager,
            IDistributedCache distributedCache, IRepository<ApplicationRole> repository,
            IUnitOfWork unitOfWork, IMapper mapper, LocService locservice
            
            )
        {
            this.RoleManager = RoleManager;
            this.distributedCache = distributedCache;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.locservice = locservice;
        }

        public async Task<CommonResponse<ApplicationRoleVM>> Create(ApplicationRoleVM VM)
        {
            VM.ID = Guid.NewGuid().ToString();
            var Obj = mapper.Map(VM, new ApplicationRole());
            var Result = await RoleManager.CreateAsync(Obj);
            //await unitOfWork.SaveChangesAsync();
            if (Result.Succeeded)
            {
                foreach (var item in VM.RoleClaims.GroupBy(x=>x.ClaimType).Select(x=>new RoleClaim {ClaimType =x.Key,ClaimValue=x.Select(xx=>xx.ClaimValue).Aggregate((a,b)=>a+","+ b)}))
                {
                        await RoleManager.AddClaimAsync(Obj, new System.Security.Claims.Claim(item.ClaimType,item.ClaimValue ));
                }
                await distributedCache.SetRecordAsync(CashingKey.ApplicationRoleClaims_ + VM.Name, VM.RoleClaims);
                return CommonResponse<ApplicationRoleVM>.GetResult(true, locservice.GetLocalizedHtmlString("SavedSuccessfully"));
            }
            else
            {
                return CommonResponse<ApplicationRoleVM>.GetResult(false, locservice.GetLocalizedHtmlString("DuplicateRoleName"));
            }
        }

        public async Task<CommonResponse<ApplicationRoleVM>> Edit(ApplicationRoleVM VM)
        {
            var Obj = await RoleManager.FindByIdAsync(VM.ID);
            //Obj = mapper.Map(VM, new ApplicationRole());
            if (Enum.GetValues(typeof(FixedRoles)).Cast<FixedRoles>().Select(x => x.ToString()).ToList().Contains(VM.Name) == false)
            {
                Obj.Name = VM.Name;
            }
            if(Obj.Name== FixedRoles.SuperAdmin.ToString() && VM.RoleClaims.Any(x => x.ClaimType.ToLower() == "ApplicationRoles".ToLower()&&x.ClaimValue.Contains(((int)SharedPermissions.BrowsPolicy).ToString())) == false|| Obj.Name == FixedRoles.SuperAdmin.ToString() && VM.RoleClaims.Any(x => x.ClaimType.ToLower() == "ApplicationRoles".ToLower() && x.ClaimValue.Contains(((int)SharedPermissions.EditPolicy).ToString())) == false)
            {
                return CommonResponse<ApplicationRoleVM>.GetResult(false, locservice.GetLocalizedHtmlString("ThisGroupMustHasPermissionInGroups"));

            }
            var Result = await RoleManager.UpdateAsync(Obj);
            var RoleClaims = await RoleManager.GetClaimsAsync(Obj);
            foreach (var item in RoleClaims)
            {
                await RoleManager.RemoveClaimAsync(Obj, item);
            }
            if (Result.Succeeded)
            {
                foreach (var item in VM.RoleClaims.GroupBy(x => x.ClaimType).Select(x => new RoleClaim { ClaimType = x.Key, ClaimValue = x.Select(xx => xx.ClaimValue).Aggregate((a, b) => a + "," + b) }))
                {
                    await RoleManager.AddClaimAsync(Obj, new System.Security.Claims.Claim(item.ClaimType, item.ClaimValue ));
                }
                await distributedCache.RemoveAsync(CashingKey.ApplicationRoleClaims_ + VM.Name);
                await distributedCache.SetRecordAsync(CashingKey.ApplicationRoleClaims_ + VM.Name, VM.RoleClaims);

                return CommonResponse<ApplicationRoleVM>.GetResult(true, locservice.GetLocalizedHtmlString("SavedSuccessfully"));
            }
            else
            {
                return CommonResponse<ApplicationRoleVM>.GetResult(false, locservice.GetLocalizedHtmlString("InvalidData"));
            }
        }
        public async Task<CommonResponse<ApplicationRoleVM>> Remove(string ID)
        {
            var Message = "";
            var Status = true;
            using (var trans = await unitOfWork.CreatTransactionAsync())
            {
                var Obj = await RoleManager.FindByIdAsync(ID);
                try
                {


                    foreach (var item in (await RoleManager.GetClaimsAsync(Obj)).ToList())
                    {
                        await RoleManager.RemoveClaimAsync(Obj, item);
                    }
                    var Result = await RoleManager.DeleteAsync(Obj);
                    await unitOfWork.CommitAsync(trans);
                    if (Result.Succeeded)
                    {
                        await distributedCache.RemoveAsync(CashingKey.ApplicationRoleClaims_ + Obj.Name);

                        Message = locservice.GetLocalizedHtmlString("RemovedSuccessfully");
                    }
                    else
                    {
                        await unitOfWork.RollBackAsync(trans);
                        Status = false;
                        //var related = Repository.Implementation.Repository<object>.GetDependenciesNamesstatic(Obj).ToList();
                        //Message = string.Format(locservice.GetLocalizedHtmlString("PlzDeleteDataRelatedWithObjForCompleteObjectRemove"),
                        //    Obj.Name, locservice.GetLocalizedHtmlString("ApplicationUsers"));
                        //return CommonResponse<ApplicationRoleVM>.GetResult(false, Message);
                        return CommonResponse<ApplicationRoleVM>.GetResult(Status, repository.GetDependenciesNames(Obj).ToList());
                    }
                }
                catch
                {
                    await unitOfWork.RollBackAsync(trans);

                    Status = false;
                    //var related = Repository.Implementation.Repository<object>.GetDependenciesNamesstatic(Obj).ToList();
                    //Message = string.Format(locservice.GetLocalizedHtmlString("PlzDeleteDataRelatedWithObjForCompleteObjectRemove"),
                    //  Obj.Name,
                    //  locservice.GetLocalizedHtmlString("ApplicationUsers"));
                    return CommonResponse<ApplicationRoleVM>.GetResult(Status, repository.GetDependenciesNames(Obj).ToList());

                    //locservice.GetLocalizedHtmlString(Obj.GetType().BaseType.Name),
                    //related.Select(x => locservice.GetLocalizedHtmlString(x).Value).Aggregate((x, y) => x + "," + y));
                    //return CommonResponse<ApplicationRoleVM>.GetResult(false, locservice.GetLocalizedHtmlString("CanNotDelete"));
                }
            }
            return CommonResponse<ApplicationRoleVM>.GetResult(Status, Message);
        }
        public async Task<ApplicationRoleVM> GetData(string ID)
        {
            var obj = await RoleManager.FindByIdAsync(ID);
            if (obj == null) return null;
            var VM = mapper.Map(obj, new ApplicationRoleVM());
            var roleClams = await RoleManager.GetClaimsAsync(obj);

            VM.RoleClaims = roleClams.Select(x => new RoleClaim
            {
                ClaimValue = x.Value,
                ClaimType = x.Type,
            }).ToList();

            return VM;
        }
        public async Task<IEnumerable<ApplicationRoleVM>> GetData()
        {
            var obj = (await RoleManager.Roles.OrderByDescending(x => x.RegistrationDate).ToListAsync());
            return mapper.Map(obj, new List<ApplicationRoleVM>());
        }
        public IEnumerable<ValidationResult> _ValidationResult(ApplicationRoleVM Role)
        {
            if (RoleManager.Roles.Any(x => x.Id != Role.ID && Role.Name != null && x.Name.ToLower().Trim() == Role.Name.ToLower().Trim()))
            {
                var Message = string.Format(locservice.GetLocalizedHtmlString("AlreadyExist"), Role.Name);
                yield return new ValidationResult(Message, new[] { nameof(Role.Name) });
            }

        }

        public async Task<CommonResponse<ApplicationRoleVM>> ToggleActive(string ID, bool IsActive)
        {
            var Obj = await RoleManager.FindByIdAsync(ID);
            if (Obj.Name.ToLower() == FixedRoles.SuperAdmin.ToString().ToLower())
                return CommonResponse<ApplicationRoleVM>.GetResult(false, locservice.GetLocalizedHtmlString("CanNotEdit"));
            //Obj = mapper.Map(VM, new ApplicationRole());
            Obj.IsActive = IsActive;
            var Result = await RoleManager.UpdateAsync(Obj);
            if (Result.Succeeded)
            {

                return CommonResponse<ApplicationRoleVM>.GetResult(true, locservice.GetLocalizedHtmlString("SavedSuccessfully"));
            }
            else
            {
                return CommonResponse<ApplicationRoleVM>.GetResult(false, locservice.GetLocalizedHtmlString("InvalidData"));
            }
        }
        public async Task<List<RoleClaim>> RoleClaims(string RoleName)
        {
            var Data = await distributedCache.GetRecordAsync<List<RoleClaim>>(CashingKey.ApplicationRoleClaims_ + RoleName);
            if (Data == null)
            {
                try
                {
                    var Role = await RoleManager.FindByNameAsync(RoleName);
                    var RoleClaims = await RoleManager.GetClaimsAsync(Role);
                    Data = RoleClaims.Select(x => new RoleClaim
                    {
                        ClaimType = x.Type,
                        ClaimValue = x.Value
                    }).ToList() ;
                    await distributedCache.SetRecordAsync(CashingKey.ApplicationRoleClaims_ + RoleName, Data);
                }
                catch
                {
                    await distributedCache.RemoveAsync(CashingKey.ApplicationRoleClaims_ + RoleName);
                }
            }
            return Data.ToList();


        }
    }
}
