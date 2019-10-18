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
    public class AssetHelper : BaseDBHelper
    {
        /// <summary>
        /// 更新资产分类id
        /// </summary>
        /// <param name="newCategoryId"></param>
        /// <param name="oldCategoryId"></param>
        /// <param name="assetusingDeptIds">所属部门 多个id用,隔开</param>
        /// <returns></returns>
        public int UpdateCategoryId(int newCategoryId, int oldCategoryId, string assetusingDeptIds)
        {
            var sql = $"UPDATE dbo.Assets SET categoryId={newCategoryId} WHERE categoryId = {oldCategoryId} AND usingDeptId IN({assetusingDeptIds})";
            return UsingConnectionExecute(sql);
        }
    }
}
