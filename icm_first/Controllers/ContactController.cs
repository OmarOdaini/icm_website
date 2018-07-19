using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using icm_first.Models;
using System.Net.Mail;
using System.Net.Http;

namespace icm_first.Controllers
{
    public class ContactController : Controller
    {
        static HttpClient client = new HttpClient();


        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Contact(ContactModel model)
        {
            if (ModelState.IsValid)
            {
                SendEmail(model.Name, model.Email, model.Message,model.Subject);
                TempData["notice"] = "Thank you, We got your message";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }


        private void SendEmail(string name, string Email, string message,string Subject = "")
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("onabil4@gmail.com", "619619619619");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress("onabil4@gmail.com", name);
            mail.Body = message;
            mail.Subject = Subject +"( "+Email+ " )";
            mail.To.Add(new MailAddress("onmh97@gmail.com"));
            smtpClient.Send(mail);
        }
    }
}