using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using icm_first.Models;
using System.Net.Mail;
using System.Net;
using MongoDB.Driver;


namespace icm_first.Controllers
{
    public class HomeController : Controller
    {
        static HttpClient client = new HttpClient();

        public ActionResult Index()
        {
            return View();
        }

        public string GetParagraphs(ParagraphModel model)
        {
            var client = new MongoClient(); // add online connection 
            var db = client.GetDatabase("icm_website");
            var collection = db.GetCollection<ParagraphModel>("paragraphs");
            var paragraph = collection.Find(x => x.title =="History");
            return paragraph.ToString();
        }

        public ActionResult About(ParagraphModel model)
        {
            ViewBag.para = "Not Working !!";

            if (ModelState.IsValid)
            {

                ViewBag.para = GetParagraphs(model);
                return View();
            }
            else
            {
                return View();
            }
        }

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
                SendEmail(model.Name, model.Email, model.Message);
                TempData["notice"] = "Thank you, We got your message";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }

        }

        private void SendEmail(string name, string Email, string message)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("onabil4@gmail.com", "619619619619");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress("onabil4@gmail.com",name);
            mail.Body = message;
            mail.Subject = name+"'s Message";
            mail.To.Add(new MailAddress("onmh97@gmail.com"));
            smtpClient.Send(mail);
        }



        public async System.Threading.Tasks.Task<ActionResult> ListDates()
        {

            int monthdays = 31, prayersCount = 6;
            string[,] monthPrayers = new string[monthdays, prayersCount];
            string[] dates = new string[monthdays];
            string[] days = new string[monthdays];
            string[] hijri = new string[monthdays];
            string month = "", today = "";
            string TodayHijri = "Couldn't Copy";
            JObject api;
            string Fajr = "", Sunrise = "", Dhuhr = "", Asr = "", Maghrib = "", Isha = "";
            string url = "http://api.aladhan.com/v1/calendar?latitude=33.4504&longitude=-88.8184&method=4&month=7&year=2018";
            int usableIndex = 0;
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage response = null;


            using (client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    api = Newtonsoft.Json.Linq.JObject.Parse(data);

                    foreach (var result in api["data"])
                    {
                        dates[usableIndex] = api["data"][usableIndex]["date"]["readable"].ToString();
                        dates[usableIndex] = dates[usableIndex].Replace(" Jul 2018", string.Empty);

                        days[usableIndex] = api["data"][usableIndex]["date"]["gregorian"]["weekday"]["en"].ToString();

                        hijri[usableIndex] = api["data"][usableIndex]["date"]["hijri"]["day"].ToString();

                        // FIND BETTER SOLUTION DUMMY
                        if(DateTime.Now.Day < 10)
                        {
                            if (dates[usableIndex] == "0"+ DateTime.Now.Day.ToString())
                            {
                                TodayHijri = api["data"][usableIndex]["date"]["hijri"]["month"]["ar"].ToString() + " " + api["data"][usableIndex]["date"]["hijri"]["day"].ToString() + " " + api["data"][usableIndex]["date"]["hijri"]["weekday"]["ar"].ToString() + " " + api["data"][usableIndex]["date"]["hijri"]["month"]["en"].ToString();
                                today = api["data"][usableIndex]["date"]["gregorian"]["weekday"]["en"].ToString() + " " + api["data"][usableIndex]["date"]["readable"].ToString();
                            }
                        }
                        else
                        {
                            if (dates[usableIndex] == DateTime.Now.Day.ToString())
                            {
                                TodayHijri = api["data"][usableIndex]["date"]["hijri"]["month"]["ar"].ToString() + " " + api["data"][usableIndex]["date"]["hijri"]["day"].ToString() + " " + api["data"][usableIndex]["date"]["hijri"]["weekday"]["ar"].ToString() + " " + api["data"][usableIndex]["date"]["hijri"]["month"]["en"].ToString();
                                today = api["data"][usableIndex]["date"]["gregorian"]["weekday"]["en"].ToString() + " " + api["data"][usableIndex]["date"]["readable"].ToString();
                            }
                        }
                        

                        Fajr = api["data"][usableIndex]["timings"]["Fajr"].ToString();
                        Fajr = Fajr.Replace("(CDT)", string.Empty);
                        monthPrayers[usableIndex, 0] = Fajr;

                        Sunrise = api["data"][usableIndex]["timings"]["Sunrise"].ToString();
                        Sunrise = Sunrise.Replace("(CDT)", string.Empty);
                        monthPrayers[usableIndex, 1] = Sunrise;

                        Dhuhr = api["data"][usableIndex]["timings"]["Dhuhr"].ToString();
                        Dhuhr = Dhuhr.Replace("(CDT)", string.Empty);
                        monthPrayers[usableIndex, 2] = Dhuhr;

                        Asr = api["data"][usableIndex]["timings"]["Asr"].ToString();
                        Asr = Asr.Replace("(CDT)", string.Empty);
                        monthPrayers[usableIndex, 3] = Asr;

                        Maghrib = api["data"][usableIndex]["timings"]["Maghrib"].ToString();
                        Maghrib = Maghrib.Replace("(CDT)", string.Empty);
                        monthPrayers[usableIndex, 4] = Maghrib;

                        Isha = api["data"][usableIndex]["timings"]["Isha"].ToString();
                        Isha = Isha.Replace("(CDT)", string.Empty);
                        monthPrayers[usableIndex, 5] = Isha;

                        usableIndex++;
                    }
                    month = api["data"][0]["date"]["readable"].ToString();
                    month = month.Replace(" 2018", string.Empty);
                    month = month.Replace("01 ", string.Empty);

                    ViewBag.hijriDay = TodayHijri;
                    ViewBag.month = month;
                    ViewBag.hijri = hijri;
                    ViewBag.days = days;
                    ViewBag.dates = dates;
                    ViewBag.times = monthPrayers;
                    ViewBag.today = today;
                }

            }


            return View();
        }





    }
}