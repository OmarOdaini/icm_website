using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.ComponentModel.DataAnnotations;

namespace icm_first.Models
{
    public class ParagraphModel
    {

        [Required]
        public string Title { set; get; }
        [Required]
        public string Pbody { set; get; }

        public string ImageURL { set; get; }

    }

}