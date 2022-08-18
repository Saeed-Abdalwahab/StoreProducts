using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.Services.ViewModels.DTO
{
    public class EmailModel
    {
        public EmailModel(string to, string subject, string message, bool isBodyHtml)
        {
            To = to;
            Subject = subject;
            Message = message;
            IsBodyHtml = isBodyHtml;
        }
        public string To
        {
            get;
        }
        public string Subject
        {
            get;
        }
        public string Message
        {
            get;
        }
        public bool IsBodyHtml
        {
            get;
        }
    }

}
