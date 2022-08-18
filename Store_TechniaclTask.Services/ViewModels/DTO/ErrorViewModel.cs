using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.Services.ViewModels.DTO
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
