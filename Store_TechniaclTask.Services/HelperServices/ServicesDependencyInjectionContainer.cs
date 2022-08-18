
using Store_TechniaclTask.Repository.Implementation;
using Store_TechniaclTask.Services.HelperServices.Abstraction;
using Store_TechniaclTask.Services.HelperServices.Implementation;
using Store_TechniaclTask.Services.ModelServices.Abstraction;
using Store_TechniaclTask.Services.ModelServices.Abstraction.Identity;
using Store_TechniaclTask.Services.ModelServices.Implementation;
using Store_TechniaclTask.Services.ModelServices.Implementation.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Repository.Abstraction;
using Repository.Implementation;
using Store_TechniaclTask.Services.ModelProducts.Implementation;
using Store_TechniaclTask.Services.ModelProducts.Abstraction;

namespace Store_TechniaclTask.Services.HelperServices
{
    public class ServicesDependencyInjectionContainer
    {
        public static void Create(IServiceCollection services)
        {
            #region CommonServices
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IFilesServices, FilesServices>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IGloableMethodsService, GloableMethodsService>();
            services.AddScoped<IApplicationRoleService, ApplicationRoleService>();
            #endregion

            #region ModelServices
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IShoppingStoreService, ShoppingStoreService>();
        
            #endregion
        }
    }
}
