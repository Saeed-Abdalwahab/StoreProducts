using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Store_TechniaclTask.Services.HelperServices
{
    public class EmailHelper
    {
        private string SenderMail = "email@gmail.com";
        private int _port = 587;
        private string _host = "host";
        private string _pass = "pass";

        public EmailHelper( )
        {
        }




        public  Task SendEmail(string toEmailAddress, string subject, string body, bool IsBodyHtml)

        {
           return Task.Run(() =>
            {
                try
                {
                MailMessage m = new MailMessage();
                System.Net.Mail.SmtpClient sc = new System.Net.Mail.SmtpClient();
                m.From = new MailAddress(SenderMail);
                m.To.Add(toEmailAddress);
                m.Subject = subject;
                m.Body = body;
                m.IsBodyHtml = IsBodyHtml;
                sc.Host = _host;
                    sc.Port = 587;
                    sc.Credentials = new System.Net.NetworkCredential(SenderMail, _pass);
                    sc.EnableSsl = true;
                    sc.Send(m);
                }
                catch
                {

                }
            });
            
        }

    }

}
