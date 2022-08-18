using AutoMapper;
using Store_TechniaclTask.DAL.Enums;
using Store_TechniaclTask.DAL.Model;
using Store_TechniaclTask.DAL.Model.Identity;
using Store_TechniaclTask.Services.ViewModels;
using Store_TechniaclTask.Services.ViewModels.DTO;
using Store_TechniaclTask.Services.ViewModels.IdentityViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Store_TechniaclTask.Services.ViewModels.Common;

namespace Store_TechniaclTask.Services.HelperServices
{
    public class AutoMapping : Profile
    {

        public AutoMapping()
        {
            #region Common
            _ApplicationUserVM();
            _ApplicationRoleVM();
            #endregion
            _ProdcutVM();
            _ShoppingStoreVM();
        }
        #region Common
 
        void _ApplicationRoleVM()
        {

            CreateMap<ApplicationRole, ApplicationRoleVM>()
                //.ForMember(destnition => destnition.RoleClaims, source => source.MapFrom(x => x.RoleClaims.Select(x=>new RoleClaim { 
                //ClaimType=x.ClaimType,
                //IsSelected=true,
                //Permissions=x.Permissions.Select(xx=>xx.Permission)
                //})))
                .ForMember(destnition => destnition.ID, source => source.MapFrom(x => x.Id));
            CreateMap<ApplicationRoleVM, ApplicationRole>()
                 .ForMember(destnition => destnition.Id, source => source.MapFrom(x => x.ID))
                ;

        }
        void _ApplicationUserVM()
        {

            CreateMap<ApplicationUser, ApplicationUserVM>().ForMember(destnition => destnition.ID, source => source.MapFrom(x => x.Id))
                                                           .ForMember(destnition => destnition.Role, source => source.MapFrom(x => x.UserRoles.Count > 0 ? x.UserRoles.FirstOrDefault().Role.Name : ""));
            CreateMap<ApplicationUserVM, ApplicationUser>()
                 .ForMember(destnition => destnition.Id, source => source.MapFrom(x => x.ID))
                 .ForMember(destnition => destnition.UserName, source => source.MapFrom(x => x.UserName ?? x.Email));

        }
  
        #endregion
        void _ProdcutVM()
        {
            CreateMap<Product, ProductVM>().ReverseMap();
           
        } 
        void _ShoppingStoreVM()
        {
            CreateMap<ShoppingStore, ShoppingStoreVM>()
                 .ForMember(destnition => destnition.UserName, source => source.MapFrom(x => x.User.UserName))
                 .ForMember(destnition => destnition.TotalPrices, source => source.MapFrom(x => x.Details.Sum(xx=>xx.ProductPrice)))
                 .ForMember(destnition => destnition.DetailsVM, source => source.MapFrom(x => x.Details.Select(xx=>new ShoppingStoreDetailsVM { 
                 ProductID=xx.ProductID,
                 ProductName=xx.Product.Name,
                 ProductPrice=xx.ProductPrice,
                 ShoppingStoreID=xx.ShoppingStoreID,
                 ID=xx.ID
                 }).ToList()))
                ;
            CreateMap<ShoppingStoreVM, ShoppingStore>()
                .ForMember(destnition => destnition.ShoppingStoreReminders, source => source.MapFrom(x =>x.ReminderTime==null?new List<ShoppingStoreReminder>(): new List<ShoppingStoreReminder>() { new ShoppingStoreReminder {Date=x.ReminderTime.Value } }))
                .ForMember(destnition => destnition.Details, source => source.MapFrom(x => x.DetailsVM.Select(xx => new ShoppingStoreDetails
                {
                    ProductID = xx.ProductID,
                    ProductPrice = xx.ProductPrice,
                    ShoppingStoreID = xx.ShoppingStoreID,
                    ID = xx.ID
                }).ToList()))
               ;

        }

    }
}
