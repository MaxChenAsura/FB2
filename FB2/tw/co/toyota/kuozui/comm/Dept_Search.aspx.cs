using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class tw_co_toyota_kuozui_web_comm_Dept_Search : BasePage
{

    string mode;
    string emp_id;
    List<string> dept;
    string header = "N";
    string super = "N";
    string sp_dept = "";
    int level;
    List<string> spDept;
    protected void Page_Load(object sender, EventArgs e)
    {
        //取得mode:dept 只顯示部門 all 全部顯示
        mode = Request.QueryString["mode"].ToString();
        super = Request.QueryString["super"] == null ? "Y" : Request.QueryString["super"].ToString();
        HID_mode.Value = mode;
        if (!Page.IsPostBack)
        {
            if (mode == "dept")
            {
                div_emp.Visible = false;
            }
            ViewState["super"] = super;
            //產生組織樹
            createTreeView();
        }
    }

    private void createTreeView()
    {
        try
        {
            Dept_Search tv = new Dept_Search();

            //取得使用者可管理部門
            getDepts(tv);

            List<Dept_Search> treeviewList = tv.getTreeViewList();
            bindTree(treeviewList, tv_view.Nodes[0]);


        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            msg = msg.Replace("\r\n", "");
            msg = msg.Replace("'", "");
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + msg + "')", true);
        }

    }

    private void getDepts(Dept_Search tv)
    {
        try
        {
            /*
            //ACES權限
            ACESLib.ACES aces = new ACESLib.ACES();

            //取得角色資料權限
            String dbRole = aces.GetRoles();
            IList<string> role = dbRole.Split(',');

            foreach (string item in role)
            {
                
                //取得部門權限聯集
                try
                {
                    ACESLib.DEPTBean deptbean = aces.GetDEPTAuth(item.Trim());
                    sp_dept += deptbean.Departments;
                    if (header == "N")
                        header = deptbean.IsDEPT;
                    //判斷是否為super
                    string SysCode = deptbean.SysCode;  //取得部門權限聯集 「大分類代碼」
                    foreach (string code in SysCode.Split(','))
                    {
                        if (code.Trim().Equals("SUPER"))
                        {
                            super = "Y";
                            ViewState["super"] = "Y";
                        }
                    }

                }
                catch (Exception)
                {
                    
                }
                

            }
            */

            super = SessionHandle.Current.is_super;
            if (SessionHandle.Current.is_super=="Y"){
                ViewState["super"] = SessionHandle.Current.is_super;
            }

            spDept = new List<string>();
            dept = new List<string>();
            if (ViewState["super"].ToString() == "Y")
            {
                dept = tv.getHead_Dept();
            }
            else
            {
                dept = tv.getHead_Dept(SessionHandle.Current.emp_id);
            }
            if (dept.Count() == 0)  //無可選部門則只能選擇自己部門及特殊部門
                header = "N";
            else
                header = "Y";

            spDept = sp_dept.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            dept.AddRange(spDept);
            dept.Add(SessionHandle.Current.dept_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void bindTree(IEnumerable<Dept_Search> tv, TreeNode parentNode)
    {

        var nodes = tv.Where(x => parentNode == null ? x.UP_DEPT_NO == "" : x.UP_DEPT_NO == parentNode.Value).OrderBy(x => x.DEPT_NO);
        foreach (var node in nodes)
        {
            TreeNode newNode;

            newNode = new TreeNode();
            //newNode.ShowCheckBox = true;
            if (super == "N")
            {
                //是否有設定可選擇部門
                if (header == "Y")
                {
                    if (dept.Contains(node.DEPT_NO)) //部門符合則可選擇
                    {
                        newNode.Text = "<font color='blue'>" + node.DEPT_NAME + "</font>";
                        newNode.SelectAction = TreeNodeSelectAction.Select;
                    }
                    else
                    {
                        newNode.Text = "<font color='black'>" + node.DEPT_NAME + "</font>";
                        newNode.SelectAction = TreeNodeSelectAction.None;
                    }
                }
                else if ((node.DEPT_NO == SessionHandle.Current.dept_no) || spDept.Contains(node.DEPT_NO)) //未設定可選部門，部門全部符合及特別部門符合則可以點選
                {
                    newNode.Text = "<font color='blue'>" + node.DEPT_NAME + "</font>";
                    newNode.SelectAction = TreeNodeSelectAction.Select;
                }
                else
                {
                    newNode.Text = "<font color='black'>" + node.DEPT_NAME + "</font>";
                    newNode.SelectAction = TreeNodeSelectAction.None;
                }
            }
            else
            {
                newNode.Text = "<font color='blue'>" + node.DEPT_NAME + "</font>";
                newNode.SelectAction = TreeNodeSelectAction.Select;
            }

            newNode.Value = node.DEPT_NO;
            string Url = "";
            //if (node.UP_DEPT_NO != "")
            //{
            //設定部門json
            DeptJson djson = new DeptJson();
            djson.DEPT_NO = node.DEPT_NO;
            djson.DEPT_NAME = node.DEPT_NAME;
            djson.UP_DEPT_NO = node.UP_DEPT_NO;
            djson.UP_DEPT_NAME = node.UP_DEPT_NAME;
            djson.HEAD_EMP_ID = node.HEAD_EMP_ID;
            djson.HEAD_EMP_NAME = node.HEAD_EMP_NAME;

            djson.DEPT_NO_20 = node.DEPT_NO_20;
            djson.DEPT_NAME_20 = node.DEPT_NAME_20;
            djson.DEPT_NO_30 = node.DEPT_NO_30;
            djson.DEPT_NAME_30 = node.DEPT_NAME_30;
            djson.DEPT_NO_40 = node.DEPT_NO_40;
            djson.DEPT_NAME_40 = node.DEPT_NAME_40;
            djson.DEPT_NO_50 = node.DEPT_NO_50;
            djson.DEPT_NAME_50 = node.DEPT_NAME_50;
            djson.DEPT_NO_60 = node.DEPT_NO_60;
            djson.DEPT_NAME_60 = node.DEPT_NAME_60;
            djson.DEPT_NO_70 = node.DEPT_NO_70;
            djson.DEPT_NAME_70 = node.DEPT_NAME_70;
            djson.DEPT_NAME_DESC = node.DEPT_NAME_DESC;
            djson.DEPT_FULL_NAME = node.DEPT_FULL_NAME;
            djson.DIV_DEPT_FULL_NAME = node.DIV_DEPT_FULL_NAME;
            string strJson = JsonConvert.SerializeObject(djson, Formatting.None);

            if (mode == "dept")
            {

                newNode.NavigateUrl = "javascript:ReturnValue('" + strJson + "');";

            }
            if (node.UP_DEPT_NO != "")
            {
                newNode.Collapse();

            }
            else
            {
                newNode.Expand();

            }
            if (parentNode == null)
            {
                tv_view.Nodes.Add(newNode);
            }
            else
            {
                parentNode.ChildNodes.Add(newNode);
            }
            bindTree(tv, newNode);
        }
    }
    protected void tv_view_SelectedNodeChanged(object sender, EventArgs e)
    {
        try
        {
            //取得該部門人員
            string dept_no = tv_view.SelectedNode.Value;
            HID_selectDeptNo.Value = dept_no;
            Dept_Search tv = new Dept_Search();
            //所選部門=所屬部門且工號!=部門主管且不是super，只能選自己
            if (dept_no == SessionHandle.Current.dept_no && SessionHandle.Current.emp_id != SessionHandle.Current.head_emp_id && ViewState["super"].ToString() == "N")
            {
                tv.onlySelf = true;
                tv.EMP_ID = SessionHandle.Current.emp_id;
            }
            else if (dept_no != SessionHandle.Current.dept_no || ViewState["super"].ToString() == "Y")//所選部門!=所屬部門或是super，可以選別人
                tv.onlySelf = false;
            tv.DEPT_NO = dept_no;
            DataTable dt = tv.getEmpDate(super: ViewState["super"].ToString());
            gv_result.DataSource = dt;
            gv_result.DataBind();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }


    }

    protected void btn_search_Click(object sender, EventArgs e)
    {
        try
        {
            ////查詢部門人員，選擇部門後才能選擇
            //if (HID_selectDeptNo.Value != "")
            //{

            Dept_Search tv = new Dept_Search();
            getDepts(tv);

            //tv.DEPT_NO = HID_selectDeptNo.Value;

            tv.EMP_ID = txt_EMP_ID.Text;
            tv.EMP_NAME = txt_EMP_NAME.Text;

            DataTable dt = tv.getEmpDate(dept, super);

            //所選部門=所屬部門且工號!=部門主管且不是super，只能選自己
            if (dt.Rows.Count > 0)
            {
                DataTable boundTable = dt.Clone();

                var selfDept = from row in dt.AsEnumerable()
                               where row.Field<string>("DEPT_NO") == SessionHandle.Current.dept_no
                               select row;

                if (selfDept.Count() > 0)
                {
                    boundTable = selfDept.DefaultIfEmpty().CopyToDataTable<DataRow>();
                    var rowsToDelete = from r1 in dt.AsEnumerable()
                                       join r2 in boundTable.AsEnumerable()
                                            on r1.Field<Int64>("ROWID") equals r2.Field<Int64>("ROWID")
                                       select r1;
                    foreach (DataRow row in rowsToDelete.ToArray())
                    {
                        if (SessionHandle.Current.emp_id != SessionHandle.Current.head_emp_id && ViewState["super"].ToString() == "N")
                        {
                            if (row.Field<string>("EMP_ID") != SessionHandle.Current.emp_id)
                            {
                                row.Delete(); // marks row as deleted;
                            }
                        }

                    }
                    dt.AcceptChanges();
                }



                gv_result.DataSource = dt;
                gv_result.DataBind();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無資料!');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void btn_confirm_Click(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                RadioButton other = (RadioButton)gv_result.Rows[i].Cells[0].FindControl("rbl_emp_id");
                if (other != null && other.Checked)
                {
                    //取得選擇列，產生員工json資料
                    EmpJson empjson = new EmpJson();
                    empjson.DEPT_NO = gv_result.Rows[i].Cells[4].Text;
                    empjson.DEPT_NAME = gv_result.Rows[i].Cells[5].Text;
                    empjson.EMP_ID = gv_result.Rows[i].Cells[2].Text;
                    empjson.EMP_NAME = gv_result.Rows[i].Cells[3].Text;
                    empjson.EMP_CD = gv_result.Rows[i].Cells[8].Text;
                    empjson.LEVEL_CD = gv_result.Rows[i].Cells[9].Text;
                    empjson.GRADE_CD = gv_result.Rows[i].Cells[10].Text;
                    empjson.PJOB_CD = gv_result.Rows[i].Cells[7].Text;
                    empjson.JOIN_DT = gv_result.Rows[i].Cells[11].Text;
                    empjson.BE_EMP_DT = gv_result.Rows[i].Cells[12].Text;
                    empjson.WS_CD = gv_result.Rows[i].Cells[13].Text;
                    empjson.EMP_STATUS = gv_result.Rows[i].Cells[14].Text;
                    empjson.PLANT_NAME = gv_result.Rows[i].Cells[15].Text;
                    empjson.WORK_SHIFT_DESC = gv_result.Rows[i].Cells[16].Text;
                    empjson.EMP_STATUS_DESC = gv_result.Rows[i].Cells[17].Text;

                    empjson.DEPT_NO_20 = gv_result.Rows[i].Cells[18].Text;
                    empjson.DEPT_NAME_20 = gv_result.Rows[i].Cells[19].Text;
                    empjson.DEPT_NO_30 = gv_result.Rows[i].Cells[20].Text;
                    empjson.DEPT_NAME_30 = gv_result.Rows[i].Cells[21].Text;
                    empjson.DEPT_NO_40 = gv_result.Rows[i].Cells[22].Text;
                    empjson.DEPT_NAME_40 = gv_result.Rows[i].Cells[23].Text;
                    empjson.DEPT_NO_50 = gv_result.Rows[i].Cells[24].Text;
                    empjson.DEPT_NAME_50 = gv_result.Rows[i].Cells[25].Text;
                    empjson.DEPT_NO_60 = gv_result.Rows[i].Cells[26].Text;
                    empjson.DEPT_NAME_60 = gv_result.Rows[i].Cells[27].Text;
                    empjson.DEPT_NO_70 = gv_result.Rows[i].Cells[28].Text;
                    empjson.DEPT_NAME_70 = gv_result.Rows[i].Cells[29].Text;
                    empjson.DEPT_NAME_DESC = gv_result.Rows[i].Cells[30].Text;
                    empjson.DEPT_FULL_NAME = gv_result.Rows[i].Cells[31].Text;
                    empjson.DIV_DEPT_FULL_NAME = gv_result.Rows[i].Cells[32].Text;
                    empjson.PJOB_DESC = gv_result.Rows[i].Cells[33].Text;


                    string strJson = JsonConvert.SerializeObject(empjson, Formatting.None);

                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "return", "ReturnValue('" + strJson + "');", true);
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        for (int i = 0; i < e.Row.Cells.Count; i++)
        {
            if (e.Row.Cells[i].Text == "&nbsp;")
                e.Row.Cells[i].Text = "";
            if (i >= 6)
                e.Row.Cells[i].Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            //設定radiobutton不postback情況單選
            RadioButton rdo = (RadioButton)e.Row.FindControl("rbl_emp_id");

            string script = "SelectOne('gv_result.*rblg_emp_id',this)";

            rdo.Attributes.Add("onclick", script);
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";

        }

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";

        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Alternate)
            e.Row.CssClass = "alternate";

        foreach (TableCell tc in e.Row.Cells)
        {
            tc.Attributes["style"] = "border-style:solid;border-width:2px; border-color: #CDE7B6";


            if (tc.HasControls())
            {
                foreach (Control c in tc.Controls)
                {
                    if (c is CheckBox)
                    {
                        tc.Attributes["onclick"] = "event.cancelBubble=true;";
                    }
                }
            }

        }
    }
}