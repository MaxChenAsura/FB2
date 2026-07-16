using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sb_WFB2SB1100_Detail : BasePage
{
    string fun_name = "WFB2SB1100";
    string ID = string.Empty;
    string MODE_ID = string.Empty;
    string TYPE = string.Empty;
    string FUNC_ID = string.Empty;
    string emp_id = string.Empty;
    string TableName = string.Empty;
    string TextColumn = string.Empty;
    string ValueColumn = string.Empty;
    //Service 物件
    private CFB2SB1100BO service = new CFB2SB1100BO();

    public class selectedData
    {
        public String W26H13 { get; set; }
        public String W26H14 { get; set; }
        public String W26H16 { get; set; }
        public String W26H17 { get; set; }
        public String W26H20 { get; set; }
        public String W26H22 { get; set; }
        public String W26H23 { get; set; }
        public String W26H26 { get; set; }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        //取得table 顯示欄位 值
        TableName = "tb";
        TextColumn = "SALARY";
        ValueColumn = "SALARY_ID";

        if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["id"])))
        {
            ID = Convert.ToString(Request.QueryString["id"]);
            TYPE = Convert.ToString(Request.QueryString["Type"]);
        }

        //emp_id = Request.QueryString["emp_id"].ToString();


        if (!Page.IsPostBack)
        {

            DataTable dt = new DataTable();
            dt = service.getEMP_ID(ID);

            if (dt.Rows.Count > 0)
            {
                
                lb_EMP_ID.Text = Convert.ToString(dt.Rows[0]["EMP_ID"]);
                lb_EMP_NAME.Text = Convert.ToString(dt.Rows[0]["EMP_NAME"]);
                lb_SUB_CD.Text = Convert.ToString(dt.Rows[0]["TYPE"]);
            }


            //取得multi_select
            getData();

            //下方gridview
            //getGridView("FUNC_ID", 0, 10);
        }

    }

    private void getData()
    {
        try
        {
            //將代碼繫結至listbox
            Multi_Select multi = new Multi_Select();
            multi.TableNmae = TableName;
            multi.TextColumn = TextColumn;
            multi.ValueColumn = ValueColumn;
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();

            if (TYPE == "1")
            {
                dt = service.getUnSelectedData1(ID, TYPE);
                lb_unselect.DataSource = dt;
                lb_unselect.DataTextField = "SALARY_NAME";
                lb_unselect.DataValueField = "SALARY_ID";
                lb_unselect.DataBind();

                dt1 = service.getSelectedData2(ID, TYPE);
                lb_select.DataSource = dt1;
                lb_select.DataTextField = "SALARY_NAME";
                lb_select.DataValueField = "SALARY_ID";
                lb_select.DataBind();
            }
            else if (TYPE == "2")
            {
                dt = service.getUnselectedData2(TYPE);
                lb_unselect.DataSource = dt;
                lb_unselect.DataTextField = "SALARY_NAME";
                lb_unselect.DataValueField = "SALARY_ID";
                lb_unselect.DataBind();

                dt1 = service.getSelectedData2(ID, TYPE);
                lb_select.DataSource = dt1;
                lb_select.DataTextField = "SALARY_NAME";
                lb_select.DataValueField = "SALARY_ID";
                lb_select.DataBind();
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //確認按鈕
    protected void WFB2SB1100Save_Click(object sender, EventArgs e)
    {
        string msg = "";
        try
        {
            CFB2SB1100DAO wfb2sb = new CFB2SB1100DAO();
            string selectedItem = "";
            selectedItem = hid_selectedItem.Value.Trim().Trim(',');
            wfb2sb.EMP_ID = Request.QueryString["id"].ToString();
            wfb2sb.TYPE = TYPE;
           
            msg = service.doSave(wfb2sb, selectedItem);
            if (msg != "0")
            {
                msg = msg.Replace("\r\n", "");
                msg = msg.Replace("'", "");
                showMessage("updateDataFailMessage", msg);
            }
            else
            {
                getData();
                Session["SB1100_Is_Search"] = "Y";
                showMessage("updateDataSuccessMessage", "");
                ScriptManager.RegisterClientScriptBlock(WFB2SB1100Ok1, this.GetType(), "error", "reDirectHome()", true);              
            }
            
            WFB2SB1100Ok1.Visible = true;
            WFB2SB1100Cancel.Visible = true;
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    //取消按鈕(勞保)
    protected void WFB2SB1100Cancel_Click(object sender, EventArgs e)
    {
        Session["SB1100_Is_Search"] = "Y";
        Response.Redirect("WFB2SB1100_Qry.aspx");
    }

}