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
        ///  加载所有子部门(含自己)
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public IList<DeptData> LoadAll(int companyid, int parentid)
        {
            //不允许改动字段顺序
            var sql = $@";WITH t1 AS(
	SELECT id,code,name,parentId,parentStr FROM dbo.Dept WHERE companyId={companyid} AND id ={parentid} and [status]=1
	UNION ALL
	SELECT s.id,s.code,s.name,s.parentId,s.parentStr FROM t1 AS p
	INNER JOIN dbo.Dept s ON s.parentId=p.id
	WHERE s.status=1  
 )
 SELECT id,code,name,parentId,parentStr FROM t1";
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
