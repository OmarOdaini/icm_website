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
using System.Web.Script.Serialization;

namespace icm_first.Controllers
{
    public class HomeController : Controller
    {
        // passing data in the URL bring it to ActionResult parameters  
        // int? is nullable "can be null"

        public ActionResult Index()
        {
            return View();
        }

    }
}