using Store_TechniaclTask.DAL.Enums;
using Store_TechniaclTask.Repository.Implementation;
using Store_TechniaclTask.Services.CustomAttribute;
using Store_TechniaclTask.Services.Enums;
using Store_TechniaclTask.Services.HelperServices.Abstraction;
using Store_TechniaclTask.Services.Resources;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository.Implementation;

namespace Store_TechniaclTask.Services.HelperServices.Implementation
{
 public   class GloableMethodsService: IGloableMethodsService
    {
        private readonly LocService locservice;
        private readonly IHttpContextAccessor httpContextAccessor;

        public GloableMethodsService(LocService locservice,IHttpContextAccessor httpContextAccessor)
        {
            this.locservice = locservice;
            this.httpContextAccessor = httpContextAccessor;
        }
        public  string RemoveErrorMessage<Tentity>(Tentity entity, string Name = null)
        {
            var related = Repository<object>.GetDependenciesNamesstatic(entity).ToList();
            var Message = string.Format(locservice.GetLocalizedHtmlString("PlzDeleteDataRelatedWithObjForCompleteObjectRemove"),
                       Name, related.Select(x => locservice.GetLocalizedHtmlString(x).Value).Aggregate((x, y) => x + "," + y));
            return Message;
        }
        public bool IsRangesOverlap(TimeSpan StartA, TimeSpan EndA, TimeSpan StartB, TimeSpan EndB)
        { //(StartA <= EndB) and(EndA >= StartB) Morgan Raw
            return (StartA <= EndB) && (EndA >= StartB);
        }
        public bool IsRangesOverlap(DateTime StartA, DateTime EndA, DateTime StartB, DateTime EndB)
        {
            //(StartA <= EndB) and(EndA >= StartB) Morgan Raw
            return (StartA <= EndB) && (EndA >= StartB);
        }
        public Language CurrentUserLanguage()
        {
            var Culture = httpContextAccessor.HttpContext.Request.Cookies["Usre_Culture"]?.ToLower();
            //UserCluture == null || UserCluture.ToLower() == "ar-eg"
            return Culture==null|| Culture.Contains("ar") ? Language.Arabic : Language.English;
        }  
        public string CurrentUserID()
        {
            return httpContextAccessor.HttpContext.User.GetUserID();
        } public string CurrentUserEmail()
        {
            return httpContextAccessor.HttpContext.User.GetUserEmail();
        }

     
    }
}
