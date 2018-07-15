using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace icm_first.Models
{
    public class ParagraphModel
    {
        public ObjectId id { set; get; }
        public string title { set; get; }
        public string cat { set; get; }
        public string Pbody { set; get; }
        public string ImageURL { set; get; }
    }

}