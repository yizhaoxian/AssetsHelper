using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Reflection;

namespace AssetsHelper.DBHelper
{
    public class BaseDBHelper : IDBHelper
    {
        private static string connectionStr = ConfigurationManager.AppSettings["ConnectionString"];


        public T GetById<T>(int id) where T : class, new()
        {
            string sql = $"select top 1 * from {typeof(T).Name} where id={id}";

            return UsingConnectionQuery<T>(sql);
        }


        public static IList<T> UsingConnectionQueryList<T>(string sql)
        {
            return UsingConnection(connection => connection.Query<T>(sql).ToList());
        }

        public static T UsingConnectionQuery<T>(string sql)
        {
            return UsingConnection(connection => connection.QueryFirstOrDefault<T>(sql));
        }

        public static int UsingConnectionExecute(string sql)
        {
            return UsingConnection(conn => conn.Execute(sql));
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procname"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static TResult UsingConnectionExecuteProc<TResult>(string procname, DynamicParameters param, DbType resultDbType)
        {

            return UsingConnection(conn =>
            { 
                param.Add("@RESULT", dbType: resultDbType, direction: ParameterDirection.ReturnValue);
                var newid = conn.Execute(procname, param, commandType: CommandType.StoredProcedure);
                return param.Get<TResult>("@RESULT");
            });
        }

        private static TResult UsingConnection<TResult>(Func<IDbConnection, TResult> action)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(connectionStr))
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open();
                    }
                    return action(connection);
                }
            }
            catch
            {
                throw;//抛出异常
            }
        }  

        /*
                public void Insert()
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
                */
    }
}
