using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;

namespace MicroAgManager.API
{
    public class EmailServices : IEmailSender
    {
        private string _host;
        private int _port;
        private bool _enableSSL;
        private static string _userName;
        private static string _password;

        // Get our parameterized configuration
        public EmailServices(string host, int port, bool enableSSL, string userName, string password)
        {
            _host = host;
            _port = port;
            _enableSSL = enableSSL;
            _userName = userName;
            _password = password;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_host, _port)
            {
                Credentials = new NetworkCredential(_userName, _password),
                EnableSsl = _enableSSL
            };
            return client.SendMailAsync(new MailMessage(_userName, email, subject, htmlMessage) { IsBodyHtml=true });
        }
    }
}
