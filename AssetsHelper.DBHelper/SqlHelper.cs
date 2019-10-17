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

namespace AssetsHelper.DBHelper
{
    public static class SqlHelper
    {
        private static string connectionStr = ConfigurationManager.AppSettings["ConnectionString"];

        public static IEnumerable<Assets> GetTest()
        {
            string sql = string.Format("SELECT TOP 11 id,num FROM dbo.Assets ");
            using (IDbConnection connection = new SqlConnection(connectionStr))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                return connection.Query<Assets>(sql);
            }
        }

        public static void Insert()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(connectionStr))
                {
                    int result = connection.Execute("insert into Sys_User_bak values(@Id,@Name,@Phone,@Address,@CreateDate)", new
                    {
                        //Id = collection["id"],
                        //Name = collection["name"],
                        //Phone = collection["phone"],
                        //Address = collection["address"],
                        CreateDate = DateTime.Now
                    });
                }
            }
            catch
            {

            }
        }
    }
    public class Assets
    {
        public int id { get; set; }
        public string num { get; set; }
    }
}
