using icm_first.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace icm_first.Controllers
{
    public class ParagraphController : Controller
    {
        private string[,,] AboutParagraphs = new string[3,10,2];            //temporary solution

        private dynamic GetParagraphs(ParagraphModel model)
        {
            GetCollection(model, "paragraphs", "cat", "History",0);
            GetCollection(model, "paragraphs", "cat", "Staff",1); 
             GetCollection(model, "paragraphs", "cat", "Organization", 2);

            return AboutParagraphs;
        }

        //All parameters required
        //Fiunction fills up a global 3D array with found collections
        private string[,,] GetCollection(ParagraphModel model,string collectionName,string column, string value,int tableNumber)
        {
            //Variables Declaration
            int count = 0;                       // var used as counter
            var client = new MongoClient("mongodb://OmarOdaini:OmarOdaini1@ds235401.mlab.com:35401/icm_website");
            var db = client.GetDatabase("icm_website");
            var collection = db.GetCollection<ParagraphModel>(collectionName);
            var filter = Builders<ParagraphModel>.Filter.Eq(column, value);
            var paragraph = collection.Find(filter).ToList();

            foreach (ParagraphModel item in paragraph)
            {
                AboutParagraphs[tableNumber, count, 0] = item.title;
                AboutParagraphs[tableNumber, count, 1] = item.Pbody;
                count++;
            }
            return AboutParagraphs;
        }

        public ActionResult Paragraph(ParagraphModel model)
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

    }
}

//.AsQueryable<ParagraphModel>(); this for return all collections
//var paragraph = collection.ToList();