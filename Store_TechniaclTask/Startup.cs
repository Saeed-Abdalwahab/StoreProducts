using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Serialization;
using Store_TechniaclTask.DAL.Context;
using Store_TechniaclTask.DAL.Model.Identity;
using Store_TechniaclTask.Services.HelperServices;
using Store_TechniaclTask.Services.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using Store_TechniaclTask.Services.Resources.CulturHelper;
using Store_TechniaclTask.DAL.Enums;
using _Store_TechniaclTask.Web.ExtensionMethods;
using Store_TechniaclTask.Services.ViewModels.DTO;
using _Store_TechniaclTask.Web.MiddleWare.Security.Claims;
using _Store_TechniaclTask.Web.MiddleWare.Security;

namespace Store_TechniaclTask.Web
{
    public class Startup
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            
            #region WebCongigration
            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddRazorRuntimeCompilation();
            services.AddControllers(x => x.AllowEmptyInputInBodyModelBinding = true).AddNewtonsoftJson(options =>
            {

                // Use the default property (Pascal) casing
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                // Configure a custom converter
                //options.SerializerSettings.Converters.Add(new CustomJsonConverter<DateTime>()); //Need To Be  Dynamic Format ?? saeed
                //options.SerializerSettings.Converters.Add(new CustomJsonConverter<Nullable<DateTime>>()); //Need To Be  Dynamic Format ?? saeed
                //options.SerializerSettings.Converters.Add(new FormatDateConverter()); //Need To Be  Dynamic Format ?? saeed
                //options.SerializerSettings.Converters.Add(new StringSanitizer()); //Need To Be  Dynamic Format ?? saeed

            });
            services.Configure<RazorViewEngineOptions>(o =>
            {
            });
            services.AddAutoMapper(typeof(AutoMapping));
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddMvc(options =>
            {

                options.EnableEndpointRouting = false;
                options.RespectBrowserAcceptHeader = true; // false by default
                //options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
                //options.Filters.Add(typeof(AuthorizeAttribute));
                //options.Conventions.Add(new ApiExplorerIgnores());

            })
                 .ConfigureApiBehaviorOptions(options =>
                 {
                     options.SuppressModelStateInvalidFilter = true;
                 }).AddMvcOptions(options =>
                 {
                     options.MaxModelValidationErrors = 999999;
                     options.MaxModelValidationErrors = 4;
                     options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                         _ => "Required");
                 }).AddViewLocalization()
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    //var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                    return factory.Create("", "");
                };
            });
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
                options.HttpOnly = HttpOnlyPolicy.Always;
            });

            services.AddDbContextPool<ApplicationDbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("AppConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = false;

                //Other options go here
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


            //services.AddHttpsRedirection(options =>
            //{
            //    options.HttpsPort = 443;
            //});
            services.AddRazorPages();
            services.AddSingleton<LocService>();
            //services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddLocalization();
            services.AddSingleton<LocalizationMiddleware>();
            services.AddDistributedMemoryCache();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();


            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                        {
                            new CultureInfo("en-US"),
                            new CultureInfo("ar-EG"),
                        };
                    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.RequestCultureProviders.Clear();
                    options.RequestCultureProviders.Add(new UserCultureProvider());
                });
            //Identity
            services.AddAuthorization(options =>
            {
                Enum.GetNames(typeof(SharedPermissions)).ToList().ForEach(Permission =>
                {
                    options.AddPolicy(Permission, policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement(Permission)));
                });
             

            });

            // override UserClaimsPrincipalFactory (to remove role claims from cookie )
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();
            services.AddScoped<IAuthorizationHandler, AuthorizationHandler>();
            ExtentionMethods.locService = CreateLocService(services);
            ServicesDependencyInjectionContainer.Create(services);
            //services.AddTransient<IClaimsTransformation, ClaimsTransformer>();
            #endregion

            //Identity
            MultipleAuthenticationSchemes(services);
            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                // enables immediate logout, after updating the user's stat.
                options.ValidationInterval = TimeSpan.FromSeconds(10);
            });
            services.AddSingleton<LocalizationMiddleware>();
            services.AddDistributedMemoryCache();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();
            services.AddScoped<IAuthorizationHandler, AuthorizationHandler>();

          

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseSerilogRequestLogging();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();

                // todo: replace with app.UseHsts(); once the feature will be stable
                app.UseRewriter(new RewriteOptions().AddRedirectToHttps(StatusCodes.Status301MovedPermanently));
            }


            //app.UseSerilogRequestLogging(); // <-- Add this line log Requests
            /// app.ConfigureExceptionHandler(logger);
            //app.ConfigureCustomExceptionMiddleware();
            //app.UseAntiXssMiddleware();

            app.UseRequestLocalization(app?.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);
            app.UseDefaultFiles();
            app.UseHttpsRedirection();
            app.UseStaticFiles(

                //    new StaticFileOptions
                //{
                //    OnPrepareResponse = (context) =>
                //    {
                //        var headers = context.Context.Response.GetTypedHeaders();

                //        headers.CacheControl = new Microsoft.Net.Http.Headers. CacheControlHeaderValue
                //        {
                //            Public = true,

                //            MaxAge = TimeSpan.FromDays(365)
                //        };
                //    }
                //}
                );
            app.UseMiddleware<LocalizationMiddleware>();

            app.UseCors(options =>
           options/*.WithOrigins(globalMethodsService.getWebSiteDomain(),globalMethodsService.getMobileAppDomain())*/
           .AllowAnyMethod().AllowAnyOrigin()
           .AllowAnyHeader());

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();
            app.UseEndpoints(endpoints =>
            {
              
                endpoints.MapControllerRoute(
         name: "default",
         pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.Use(async (HttpContext context, Func<Task> next) =>
            {
                await next.Invoke();

                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = new PathString("/index.html");
                    await next.Invoke();
                }
            });

        }
        LocService CreateLocService(IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider(); //No warning here ))
            return serviceProvider.GetService<LocService>();
        }
        UserManager<ApplicationUser> getusermanager(IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider(); //No warning here ))
            return serviceProvider.GetService<UserManager<ApplicationUser>>();
        }
      
        //Multiple Authentication Schemes Configuration coocki and bearer
        void MultipleAuthenticationSchemes(IServiceCollection services)
        {

            services.AddAuthentication(options =>
            {
            })
                .AddCookie("Cookies", options =>
                {
                    options.Events = new CookieAuthenticationEvents
                    {


                        OnRedirectToLogin = context =>
                        {
                            if (context.HttpContext.Request.IsAjaxRequest() || context.HttpContext.Request.Path.Value.Contains("api"))
                            {
                                context.Response.StatusCode = 401;
                                // Serialise using the settings provided
                                var json = JsonSerializer.Serialize(new MobileCommonResponse_GetData<object>("need to login", 401));

                                // Write to the response
                                return context.Response.WriteAsync(json);
                            }
                            else
                            {
                                context.Response.Redirect(context.RedirectUri);
                                return Task.CompletedTask;
                            }
                        },


                    };
                    options.LoginPath = $"/account/login";
                    options.LogoutPath = $"/account/logout";
                    options.AccessDeniedPath = $"/account/accessDenied";
                    //options.ExpireTimeSpan = TimeSpan.FromDays(1);
                });
        }
    }
}
