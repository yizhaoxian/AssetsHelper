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
    public class DeptHelper : BaseDBHelper
    {
        /// <summary>
        /// 加载所有部门
        /// </summary>
        /// <param name="companyid"></param>
        /// <returns></returns>
        public IList<DeptData> LoadAll(int companyid)
        {
            //不允许改动字段顺序
            var sql = $"SELECT id,companyId,code,createUserCode,name,createAt,parentId,parentStr FROM dbo.Dept WHERE companyId={companyid} AND status=1 order by parentStr";
            return UsingConnectionQueryList<DeptData>(sql);
        }

        /// <summary>
        /// 更新当前部门的所属城市，只更新传入的部门，不会去更新子部门
        /// </summary>
        /// <param name="oldCompanyId">原城市</param>
        /// <param name="newCompanyId">新城市</param>
        /// <param name="updateDeptIds">需要更新的部门id</param>
        /// <returns></returns>
        public int UpdateCompanyId(int oldCompanyId, int newCompanyId, string updateDeptIds)
        {
            string sql = $"UPDATE dbo.Dept SET companyId={newCompanyId} WHERE companyId={oldCompanyId} AND id IN({updateDeptIds})";
            return UsingConnectionExecute(sql);
        }

        /// <summary>
        /// 更新当前部门的所属城市，只更新传入的部门，不会去更新子部门
        /// </summary>
        /// <param name="oldCompanyId">原城市</param>
        /// <param name="newCompanyId">新城市</param>
        /// <param name="updateDeptIds">需要更新的部门id</param>
        /// <returns></returns>
        public int UpdateCompanyId(int oldCompanyId, int newCompanyId, int updateDeptId, int parentid, string parentstr)
        {
            string sql = $"UPDATE dbo.Dept SET companyId={newCompanyId},parentId={parentid},parentStr='{parentstr}' WHERE companyId={oldCompanyId} AND id ={updateDeptId}";
            return UsingConnectionExecute(sql);
        }

    }
}
