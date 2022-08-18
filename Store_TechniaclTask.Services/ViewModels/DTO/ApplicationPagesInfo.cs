using Store_TechniaclTask.DAL.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Store_TechniaclTask.Services.ViewModels.DTO
{
    public class ApplicationPagesInfo
    {
        public string ClaimTypeName
        {
            get
            {
                var tt = $"/{Area}/{Controller}/{Action}/{Permission}";
                tt = tt.Replace("//", "/");
                return tt;
            }
        }
        public string Controller { get; set; }
        public string Permission { get; set; }
        public SharedPermissions? _Permission { get; set; }
        public string PageName { get; set; }
        public string Attributes { get; set; }
        //public SystemController systemController { get; set; }
        public string Area { get; set; }
        public string Action { get; set; }
        public bool IsMainViewPage { get; set; }
        public string Url
        {
            get
            {
                var tt = $"/{Area}/{Controller}/{Action}";
                tt = tt.Replace("//", "/").Trim();
                return tt;
            }
        }
       
    }

  
}
