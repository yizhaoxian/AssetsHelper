using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsHelper.Models
{
    public class Company
    {
        public int id { get; set; }
        public int sort { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public DateTime createAt { get; set; }
        public DateTime modiftAt { get; set; }
        public int status { get; set; }
        public int parentId { get; set; }
        public string description { get; set; }
    }
    public class CompanyNameData
    {
        public int id { get; set; } 
        public string name { get; set; } 
    }
}
