using Microsoft.AspNetCore.Mvc;
using MimeKit;
using PrivateSite.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration; //Added for access to the info in appsettings.json
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using MailKit.Net.Smtp; //Added for access to SmtpClient class
using System.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace PrivateSite.Controllers
{
    public class ContactController : Controller
    {
        private readonly IConfiguration _config;
        
        public ContactController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            string message = $"You have received a new email from your site's contact form!<br />" +
                $"Sender: {cvm.Name}<br />Email: {cvm.Email}<br />Subject: {cvm.Subject}<br />Message:\n{cvm.Message}";

            var mm = new MimeMessage();

            mm.From.Add(new MailboxAddress("Sender", _config.GetValue<string>("Credentials:Email:User")));

            mm.To.Add(new MailboxAddress("Personal", _config.GetValue<string>("Credentials:Email:Recipient")));

            mm.Subject = cvm.Subject;

            mm.Body = new TextPart("HTML") { Text = message };

            mm.Priority = MessagePriority.Urgent;

            mm.ReplyTo.Add(new MailboxAddress("User", cvm.Email));

            using (var client = new SmtpClient())
            {
                client.Connect(_config.GetValue<string>("Credentials:Email:Client"), 8889);
                client.Authenticate(
                    //Username
                    _config.GetValue<string>("Credentials:Email:User"),
                    //Password
                    _config.GetValue<string>("Credentials:Email:Password")
                    );
                try
                {
                    client.Send(mm);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"There was an error processing your request. Please try again later.<br />Error Message: {ex.StackTrace}";
                    return View(cvm);

                }
            }
            return View("EmailConfirmation", cvm);
        }

    }
}
