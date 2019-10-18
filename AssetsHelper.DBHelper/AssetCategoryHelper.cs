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
    public class AssetCategoryHelper : BaseDBHelper
    {
        public IList<AssetCategory> LoadAll(int companyid)
        {
            var sql = $"SELECT * FROM dbo.AssetCategory where companyid={companyid} and [status]=1";
            return UsingConnectionQueryList<AssetCategory>(sql);
        }

        public IList<AssetCategory> LoadAll(string deptids)
        {
            var sql = $@"SELECT * FROM dbo.AssetCategory 
 WHERE id IN(SELECT categoryId FROM dbo.Assets WHERE usingDeptId IN({deptids}) GROUP BY categoryId)";
            return UsingConnectionQueryList<AssetCategory>(sql);
        }

        public IList<AssetCategoryData> LoadAllName()
        {
            var sql = "SELECT id,name FROM dbo.AssetCategory where [status]=1";
            return UsingConnectionQueryList<AssetCategoryData>(sql);
        }


        public int UpdateParent(int id, int parentId, string parentStr)
        {
            var sql = $"UPDATE dbo.AssetCategory SET parentId={parentId},parentStr='{parentStr}' WHERE id={id}";
            return UsingConnectionExecute(sql);
        }

        /// <summary>
        /// 复制到新城市，最后返回新分类id
        /// </summary>
        /// <param name="copyid"></param>
        /// <param name="newCompnayid"></param>
        /// <returns></returns>
        public int InsertCopy(int copyid, int newCompnayid)
        {
            var param = new DynamicParameters();
            param.Add("@copyId", copyid, DbType.Int32);
            param.Add("@newcompanyId", newCompnayid, DbType.Int32); 
            return UsingConnectionExecuteProc<int>("usp_copy_AssetCategory", param, DbType.Int32);
        }  
    }
}
