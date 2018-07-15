using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace icm_first.Controllers
{
    public class DateController : Controller
    {

        public async System.Threading.Tasks.Task<ActionResult> Date()
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
                        if (DateTime.Now.Day < 10)
                        {
                            if (dates[usableIndex] == "0" + DateTime.Now.Day.ToString())
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