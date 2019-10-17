using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetsHelper.Models
{
    public class Dept
    {
        public int id { get; set; }
        public int companyId { get; set; }
        public string code { get; set; }
        public string createUserCode { get; set; }
        public string name { get; set; }
        public DateTime createAt { get; set; }
        public DateTime modiftAt { get; set; } 
        public int parentId { get; set; }
        public int sort { get; set; }
        public string parentStr { get; set; }
        public int status { get; set; }
    }

    public class DeptData
    {
        public int id { get; set; } 
        [Description("部门名称")]
        public string name { get; set; }
        [Description("部门编码")]
        public string code { get; set; }
        public int parentId { get; set; }
        public string parentStr { get; set; } 
        [Description("添加时间")]
        public DateTime createAt { get; set; } 
    }
}
