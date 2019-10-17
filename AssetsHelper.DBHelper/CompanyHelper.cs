using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using AssetsHelper.IDBHelper;
using AssetsHelper.Models;

namespace AssetsHelper.DBHelper
{
    public class CompanyHelper : BaseDBHelper
    {
        public IEnumerable<Company> LoadAll()
        {
            var sql = "SELECT * FROM dbo.Company where [status]=1";
            return UsingConnectionQueryList<Company>(sql);
        }
        public IList<CompanyNameData> LoadAllName()
        {
            var sql = "SELECT id,name FROM dbo.Company where [status]=1";
            return UsingConnectionQueryList<CompanyNameData>(sql);
        }
    }
}
