using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsFlashes.Models
{
    public class NewsFlash
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
    }
}