using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssetsHelper.DBHelper;
using AssetsHelper.Models;
using NLog;

namespace BatchTransfer
{
    public partial class Index : Form
    {
        DeptHelper _deptHelper;
        CompanyHelper _companyHelper;
        AssetCategoryHelper _assetCategoryHelper;
        AssetHelper _assetHelper;
        ILogger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 需要新增的分类
        /// </summary>
        IList<AssetCategory> assetCategoriesTemp = new List<AssetCategory>();
        /// <summary>
        /// 当前需要转移的部门和子部门
        /// </summary>
        IList<int> deptIdsTemp = new List<int>();
        /// <summary>
        /// 新城市的所有资产分类
        /// </summary>
        IList<AssetCategory> assetCategorieAllNewCompany = new List<AssetCategory>();

        public Index()
        {
            InitializeComponent();
            _deptHelper = new DeptHelper();
            _companyHelper = new CompanyHelper();
            _assetCategoryHelper = new AssetCategoryHelper();
            _assetHelper = new AssetHelper();
        }

        #region 事件

        private void Index_Load(object sender, EventArgs e)
        {
            Console.WriteLine($"-----------{DateTime.Now} 资产转移执行日志 -----------");
            //城市列表 
            var firstCompanyId = InitCompany();
            InitDept(firstCompanyId);
        }

        /// <summary>
        /// 转移城市下拉事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SourceCityList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var companyid = 0;
            int.TryParse(this.SourceCityList.SelectedValue.ToString(), out companyid);
            if (companyid > 0)
            {
                InitDept(companyid);
            }
            this.SourceCityName.Text = "城市Id:" + companyid;
        }

        /// <summary>
        /// 接收城市下拉事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoToCityList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var companyid = 0;
            int.TryParse(this.GoToCityList.SelectedValue.ToString(), out companyid);
            this.GoToCityId.Text = "城市Id:" + companyid;
        }

        /// <summary>
        /// 确认转移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            #region 数据验证
            var oldCompanyId = (int)SourceCityList.SelectedValue;
            if (oldCompanyId < 1)
            {
                MessageBox.Show("请选择需要转移资产的城市", "系统提示");
                return;
            }
            var newCompanyId = (int)GoToCityList.SelectedValue;
            if (newCompanyId < 1)
            {
                MessageBox.Show("请选择接收资产的城市", "系统提示");
                return;
            }
            var gotoDeptId = 0;
            int.TryParse(GoToDeptId.Text, out gotoDeptId);
            if (gotoDeptId == 0)
            {
                MessageBox.Show("请填写接收部门Id", "系统提示");
                return;
            }
            var checkIds = GetCheckDept();
            if (checkIds.Count < 1)
            {
                MessageBox.Show("请选择要转移的部门", "系统提示");
                return;
            }
            var dept_goto = _deptHelper.GetById<Dept>(gotoDeptId);
            if (dept_goto == null)
            {
                WriteLog($"接收部门Id 【{gotoDeptId}】 不存在");
                return;
            }

            #endregion

            if (MessageBox.Show("确认转移勾选部门资产吗？", "系统提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Console.WriteLine("-- {0} 资产开始转移", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                logger.Info("资产开始转移");

                var updateDeptIds = string.Join(",", checkIds);
                string transferinfo = $"将{this.SourceCityList.Text}【{oldCompanyId}】下的部门【{updateDeptIds}】以及资产、资产分类转移到{this.GoToCityList.Text}【{newCompanyId}】下";
                WriteLog(transferinfo);

                /* 
                 * 1、更新部门城市id, 并更新parentId\parentStr 
                 * 2、新增资产分类 , 并更新parentId\parentStr 
                 * 3、将原来的资产分类改为新增分类 
                 */
                //新城市的所有分类 
                assetCategorieAllNewCompany = _assetCategoryHelper.LoadAll(newCompanyId);
                for (int i = 0; i < checkIds.Count; i++)
                {
                    deptIdsTemp.Clear();
                    /**************************** 转移部门 ****************************/
                    // 1.1、查找所有子部门(含本身)
                    var deptlist = _deptHelper.LoadAll(oldCompanyId, checkIds[i]);
                    if (deptlist != null)
                    {
                        WriteLog($"开始转移部门【{checkIds[i]}】和所有子部门");
                        // 1.2、更新所有子部门(含本身)  
                        UpdateSubDeptCompany(checkIds[i], deptlist, oldCompanyId, newCompanyId, dept_goto.id, dept_goto.parentStr);
                        /*************************** 新增资产分类并更新资产分类id *************/
                        var depts = string.Join(",", deptIdsTemp);
                        WriteLog($"部门【{checkIds[i]}】和所有子部门id，【{depts}】");
                        WriteLog($"开始复制资产分类");
                        //需要转移资产的所有分类 
                        var categroys_source = _assetCategoryHelper.LoadAll(depts);
                        if (categroys_source != null && categroys_source.Count > 0)
                        {
                            for (int c = 0; c < categroys_source.Count; c++)
                            {
                                //新增分类，并将资产分配到新分类
                                CopyCategory(categroys_source[c], newCompanyId, depts, categroys_source[c].id);
                            }
                        }
                        else
                        {
                            WriteLog($"当前转移部门没有需要复制的资产分类");
                        }
                        WriteLog($"部门【{checkIds[i]}】和所有子部门【{depts}】转移完成");
                    }
                    else
                    {
                        WriteLog($"转移部门【{checkIds[i]}】找不到");
                    }
                }
                logger.Info("--------------------------------------------------------------------------------------");
            }
        }

        #endregion

        /// <summary>
        /// 更新所有部门的新城市id,parentid,parentstr
        /// </summary>
        /// <param name="parentid"></param>
        /// <param name="deptDatas">所有子部门(含本身)</param>
        private void UpdateSubDeptCompany(int sourceParentId, IList<DeptData> deptDatas, int oldCompanyId, int newCompanyId, int newParentId, string newParentStr)
        {
            if (deptDatas == null) return;
            //转移父部门自身城市
            var newpstr = newParentStr + "," + sourceParentId;
            _deptHelper.UpdateCompanyId(oldCompanyId, newCompanyId, sourceParentId, newParentId, newpstr);
            WriteLog($"Update Dept newCompanyId={newCompanyId},newParentId={newParentId},newpstr={newpstr} where Id={sourceParentId}");
            deptIdsTemp.Add(sourceParentId);
            var sublist = deptDatas.Where(l => l.parentId == sourceParentId).ToList();
            if (sublist != null && sublist.Count > 0)
            {
                for (int i = 0; i < sublist.Count; i++)
                {
                    UpdateSubDeptCompany(sublist[i].id, deptDatas, oldCompanyId, newCompanyId, sourceParentId, newpstr);
                }
            }
        }

        /// <summary>
        /// 初始化城市下拉列表
        /// </summary>
        /// <returns></returns>
        private int InitCompany()
        {
            var companyList = _companyHelper.LoadAllName();
            this.SourceCityList.DataSource = companyList;
            this.SourceCityList.DisplayMember = "name";
            this.SourceCityList.ValueMember = "id";
            var gotoCompanyList = new List<CompanyNameData>();
            gotoCompanyList.Add(new CompanyNameData { id = 0, name = "请选择接收资产的城市" });
            gotoCompanyList.AddRange(companyList);
            this.GoToCityList.DataSource = gotoCompanyList;
            this.GoToCityList.DisplayMember = "name";
            this.GoToCityList.ValueMember = "id";
            return companyList != null ? companyList[0].id : 0;

        }

        /// <summary>
        /// 加载城市下的部门
        /// </summary>
        /// <param name="companyid"></param>
        private void InitDept(int companyid)
        {
            if (companyid < 1) return;

            //绑定数据
            var deptList = _deptHelper.LoadAll(companyid);
            this.SourceDeptTable.DataSource = deptList;
            this.SourceDeptTable.Font = new Font(FontFamily.GenericSansSerif, 12);

            //修改列名称
            var type = typeof(DeptData);
            var attrs = type.GetProperties();
            for (int i = 0; i < attrs.Length; i++)
            {
                var tx = attrs[i].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (tx.Length > 0)
                {
                    this.SourceDeptTable.Columns[attrs[i].Name].HeaderText = ((DescriptionAttribute)tx[0]).Description;
                }
            }
            //自适应宽度
            int width = 0;
            //对于DataGridView的每一个列都调整
            for (int i = 0; i < this.SourceDeptTable.Columns.Count; i++)
            {
                //将每一列都调整为自动适应模式
                this.SourceDeptTable.AutoResizeColumn(i, DataGridViewAutoSizeColumnMode.AllCells);
                //记录整个DataGridView的宽度
                width += this.SourceDeptTable.Columns[i].Width;
                if (i > 0)
                {
                    this.SourceDeptTable.Columns[i].ReadOnly = true;
                }
            }

            //判断调整后的宽度与原来设定的宽度的关系，如果是调整后的宽度大于原来设定的宽度，
            //则将DataGridView的列自动调整模式设置为显示的列即可，
            //如果是小于原来设定的宽度，将模式改为填充。
            if (width > this.SourceDeptTable.Size.Width)
            {
                this.SourceDeptTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            }
            else
            {
                this.SourceDeptTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            //冻结某列 从左开始 0，1，2
            SourceDeptTable.Columns[1].Frozen = true;

        }

        /// <summary>
        /// 获取选中的部门
        /// </summary>
        /// <returns></returns>
        private List<int> GetCheckDept()
        {
            List<int> checkIds = new List<int>();
            for (int i = 0; i < this.SourceDeptTable.Rows.Count; i++)
            {
                var checkbox = (DataGridViewCheckBoxCell)this.SourceDeptTable.Rows[i].Cells[0];
                var check = Convert.ToBoolean(checkbox.Value);
                if (check)
                {
                    checkIds.Add((int)this.SourceDeptTable.Rows[i].Cells[1].Value);
                }
            }
            return checkIds;
        }


        #region 复制资产分类

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetCategorySource"></param>
        /// <param name="newCompanyId"></param>
        /// <param name="assetusingDeptIds"></param>
        /// <param name="originalCategoryId"></param>
        /// <param name="assetCategorieGoto">新城市的所有资产分类</param>
        private void CopyCategory(AssetCategory assetCategorySource, int newCompanyId, string assetusingDeptIds, int originalCategoryId)
        {
            //查看分类是否存在
            var existsCategory = assetCategorieAllNewCompany.FirstOrDefault(l =>
                     l.name == assetCategorySource.name
                  && l.btyte == assetCategorySource.btyte
                  && l.code == assetCategorySource.code
              );
            if (existsCategory != null)
            {
                if (assetCategorySource.id == originalCategoryId)//存在相同分类，不用复制新分类，直接更新资产分类id
                {
                    WriteLog($"资产分类【{existsCategory.name},{existsCategory.id}】已存在，并且此分类为需要更新资产的分类，直接更新资产的分类id");
                    _assetHelper.UpdateCategoryId(existsCategory.id, assetCategorySource.id, assetusingDeptIds);
                }
                else
                {
                    var parentStr = existsCategory.parentStr;
                    var newCategoryId = existsCategory.id;
                    WriteLog($"资产分类【{existsCategory.name},{existsCategory.id}】已存在，开始复制子分类");
                    if (assetCategoriesTemp.Count > 0)
                    {
                        for (int i = assetCategoriesTemp.Count - 1; i >= 0; i--)
                        {
                            newCategoryId = AddAssetCategory(assetCategoriesTemp[i], newCategoryId, ref parentStr, newCompanyId, originalCategoryId, assetusingDeptIds);
                            assetCategoriesTemp.RemoveAt(i);//移除临时集合
                        }
                    }
                }
                return;
            }

            //如果相同分类不存在 新增
            if (assetCategorySource.parentId > 0)
            {
                var assetCategoryParent = _assetCategoryHelper.GetById<AssetCategory>(assetCategorySource.parentId);
                if (assetCategoryParent != null)
                {
                    assetCategoriesTemp.Add(assetCategorySource);
                    CopyCategory(assetCategoryParent, newCompanyId, assetusingDeptIds, originalCategoryId);
                }
                else
                {
                    assetCategorySource.parentId = 0;
                    CopyCategory(assetCategorySource, newCompanyId, assetusingDeptIds, originalCategoryId);
                }
            }
            else
            {
                var parentStr = "0";
                var newCategoryId = AddAssetCategory(assetCategorySource, 0, ref parentStr, newCompanyId, originalCategoryId, assetusingDeptIds);
                if (assetCategoriesTemp.Count > 0)
                {
                    for (int i = assetCategoriesTemp.Count - 1; i >= 0; i--)
                    {
                        newCategoryId = AddAssetCategory(assetCategoriesTemp[i], newCategoryId, ref parentStr, newCompanyId, originalCategoryId, assetusingDeptIds);
                        assetCategoriesTemp.RemoveAt(i);//移除临时集合
                    }
                }
            }
        }

        private int AddAssetCategory(AssetCategory assetCategorySource, int parentid, ref string parentStr, int newCompanyId, int originalCategoryId, string assetusingDeptIds)
        {
            //1、复制到新城市，最后返回新分类id
            var newCategoryId = _assetCategoryHelper.InsertCopy(assetCategorySource.id, newCompanyId);
            WriteLog($"1、复制到新城市，返回新分类【{assetCategorySource.name},{newCategoryId}】");
            //2、更新分类parentId，parentStr 
            parentStr += "," + newCategoryId;
            _assetCategoryHelper.UpdateParent(newCategoryId, parentid, parentStr);
            WriteLog($"2、更新新分类【parentid={parentid},parentStr={parentStr}】");
            //3、更新资产的分类Id
            if (assetCategorySource.id == originalCategoryId)
            {
                var assetRows = _assetHelper.UpdateCategoryId(newCategoryId, assetCategorySource.id, assetusingDeptIds);
                WriteLog($"3、更新资产的新分类【id={newCategoryId}】,使用部门【usingDeptId={assetusingDeptIds}】,资产数量【{assetRows}】");
            }
            //将新添加分类放入到临时表里，减少数据库查询
            assetCategorieAllNewCompany.Add(new AssetCategory
            {
                id = newCategoryId,
                companyId = newCompanyId,
                name = assetCategorySource.name,
                code = assetCategorySource.code,
                btyte = assetCategorySource.btyte,
                parentId = parentid,
                parentStr = parentStr,
                status = 1
            });
            return newCategoryId;
        }

        #endregion


        private void WriteLog(string msg)
        {
            Console.WriteLine(msg);
            logger.Info(msg);
        }
    }
}
