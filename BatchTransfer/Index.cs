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

namespace BatchTransfer
{
    public partial class Index : Form
    {
        DeptHelper _deptHelper;
        CompanyHelper _companyHelper;
        public Index()
        {
            InitializeComponent();
            _deptHelper = new DeptHelper();
            _companyHelper = new CompanyHelper();
        }

        #region 事件
        private void Index_Load(object sender, EventArgs e)
        {
            Console.WriteLine("----------- {0} 程序启动 -----------", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
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
            if (MessageBox.Show("确认转移勾选部门资产吗？", "系统提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
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
                    MessageBox.Show("请先选择要转移的部门", "系统提示");
                    return;
                }

                Console.WriteLine("-- {0} 开始转移", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                var oldCompanyId = (int)SourceCityList.SelectedValue;
                var newCompanyId = (int)GoToCityList.SelectedValue;
                var updateDeptIds = string.Join(",", checkIds);

                string tips = $"-- 将{this.SourceCityList.Text}【{oldCompanyId}】下的部门【{updateDeptIds}】以及资产、资产分类转移到{this.GoToCityList.Text}【{newCompanyId}】下";
                Console.WriteLine(tips);

                /* 
                 * 1、更新部门城市id, 并更新parentId\parentStr 
                 * 2、新增资产分类 , 并更新parentId\parentStr 
                 * 3、将原来的资产分类改为新增分类 
                 */

                for (int i = 0; i < checkIds.Count; i++)
                {
                    var dept = _deptHelper.GetById<Dept>(gotoDeptId);
                    // 1、转移部门
                    _deptHelper.UpdateCompanyId(oldCompanyId, newCompanyId, checkIds[i], gotoDeptId, dept.parentStr + "," + checkIds[i]);
                }


                //2、新城市增加\资产分类，并更新分类的parentId\parentStr 

                //3、将原来的资产分类改为新增分类 

            }
        }

        #endregion


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
            var companyList2 = new List<CompanyNameData>();
            companyList2.AddRange(companyList);
            this.GoToCityList.DataSource = companyList2;
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
    }
}
