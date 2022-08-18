using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.Services.ViewModels.Common
{
   public class CommonPageingRequest
    {
        public CommonPageingRequest()
        {
           

        }
        public int pageSize { get; set; }
        public int pageIndex { get; set; }
        public int pageNumber { get; set; }
        public string Search { get; set; }
    }
}
