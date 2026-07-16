
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.UserModel;

public partial class WebContent_WFB2SJ0540_Qry : BasePage
{
    //宣告BO 物件
    private CFB2SJ0510BO sj0510BO = new CFB2SJ0510BO();
    private CFB2SJ0520BO sj0520BO = new CFB2SJ0520BO();
    private CFB2SJ0500BO sj0500BO = new CFB2SJ0500BO();
    private CFB2SJ0150BO sj0150BO = new CFB2SJ0150BO();
    private CFB2SJ0540BO sj0540BO = new CFB2SJ0540BO();
    //.NET的初始功能
    protected void Page_Load(object sender, EventArgs e)
    {
        //呼叫前端的javaScript，取消uiblock等作用
        //ScriptManager.RegisterClientScriptBlock(gv_result, this.GetType(), "init", "iniForm();", true);
       

        //第一次進入頁面執行
        if (!IsPostBack) 
        {
            
            //取得查詢條件 資料
            initialValue();
            

            

            //查詢條件及自動查詢
            
            //將Session 的workbook 匯出Excel
            this.exportExcel();
        }

       

    }

    #region DB資料取得

    //取得查詢條件資料
    private void initialValue()
    {
        try
        {

            DataTable dt = new DataTable();
            dt = utilities.getCommCode("SJ", "ASSESS_TYPE", "", "");
            ddl_ASSESS_TYPE.Items.Add(new ListItem("", "-1"));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddl_ASSESS_TYPE.Items.Add(new ListItem(dt.Rows[i]["SUB_DESC"].ToString(), dt.Rows[i]["SUB_CD"].ToString()));
                }
            }
            dt = new DataTable();
            dt = sj0540BO.getDeptNo20();
            // dt = sj0540DAO.getDept20Data();
             ddl_DEPT_NO_20.Items.Add(new ListItem("", "-1"));
             if (dt.Rows.Count > 0)
             {
                 for (int i = 0; i < dt.Rows.Count; i++)
                 {
                     ddl_DEPT_NO_20.Items.Add(new ListItem(dt.Rows[i]["DEPT_NAME"].ToString(), dt.Rows[i]["DEPT_NO"].ToString()));
                 }
             }
            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }


    #endregion


 
    #region button 事件
    
    protected void WFB2SJ0540Refer_Click(object sender, EventArgs e)
    {
        string err = "";
        CFB2SJ0520DAO dao = new CFB2SJ0520DAO();
        dao.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
        dao.ASSESS_TYPE = ddl_ASSESS_TYPE.SelectedValue;
        dao.DEPT_NO = ddl_DEPT_NO_20.SelectedValue;
        //有block
        IWorkbook workbook = sj0520BO.createReferExcel(dao, "xlsx");
        if (workbook != null)
        {

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無資料');doUnBlock();", true);
            return;
        }
        Session["workbook_SJ052_R"] = workbook;
        dwnframe.Attributes["src"] = "WFB2SJ0520_Qry.aspx?FileType_SJ052_R = excel";
        Session["FileType_SJ052_R"] = "excel";

        
    }
    protected void WFB2SJ0540Statistics51_Click(object sender, EventArgs e)
    {
        if (txt_ASSESS_YEAR.Text=="")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請輸入考核年度!');", true);
            return;
        }
        if (ddl_ASSESS_TYPE.SelectedValue == "-1")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請輸入考核類別!');", true);
            return;
        }
        if (ddl_DEPT_NO_20.SelectedValue=="-1")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇部門!');", true);
            return;
        }
        string err = "";
        CFB2SJ0510DAO dao = new CFB2SJ0510DAO();
        dao.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
        dao.ASSESS_TYPE = ddl_ASSESS_TYPE.SelectedValue;
        dao.DEPT_NO = ddl_DEPT_NO_20.SelectedValue;
        dao.EMP_ID = "";
        dao.WS_CD = "-1";
        dao.SCORE_LEVEL_GROUP = "-1";
        dao.DEPT_NAME = ddl_DEPT_NO_20.SelectedItem.Text;
        //dao.ASSESS_TYPE_DESC = txt_ASSESS_TYPE.Text;
        //有block
        IWorkbook workbook = sj0510BO.createstatisticsExcel(dao, "xlsx");
        if (workbook != null)
        {

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無資料');doUnBlock();", true);
            return;
        }
        Session["workbook_SJ051_S"] = workbook;
        dwnframe.Attributes["src"] = "WFB2SJ0540_Qry.aspx?FileType_SJ051_S = excel";
        Session["FileType_SJ051_S"] = "excel";
    }

    protected void WFB2SJ0540Statistics52_Click(object sender, EventArgs e)
    {
        if (txt_ASSESS_YEAR.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請輸入考核年度!');", true);
            return;
        }
        if (ddl_ASSESS_TYPE.SelectedValue == "-1")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請輸入考核類別!');", true);
            return;
        }
        if (txt_EMP_ID.Text=="")
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('請選擇協理!');", true);
            return;
        }
        string err = "";
        CFB2SJ0520DAO dao = new CFB2SJ0520DAO();
        dao.ASSESS_YEAR = txt_ASSESS_YEAR.Text;
        dao.ASSESS_TYPE = ddl_ASSESS_TYPE.SelectedValue;
       //dao.DEPT_NO = ddl_DEPT_NO_20.SelectedValue;
        //dao.DEPT_NAME = ddl_DEPT_NO_20.SelectedItem.Text;
       
        dao.WS_CD = "-1";
        dao.GRP_CD = "-1";
        dao.EMP_ID = "";
        dao.MA_EMP_ID = "";
        dao.DEPT_NO = "";
        DataTable dt = new DataTable();
        dt = sj0540BO.getDeptNo20();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (txt_EMP_ID.Text == dt.Rows[i]["HEAD_EMP_ID_2"].ToString())
                {
                    dao.MA_EMP_ID = dt.Rows[i]["HEAD_EMP_ID_2"].ToString();
                    dao.MA_EMP_NAME = dt.Rows[i]["HEAD_EMP_NAME_2"].ToString();
                    dao.DEPT_NO = dt.Rows[i]["DEPT_NO"].ToString();
                    dao.DEPT_NAME = dt.Rows[i]["DEPT_NAME"].ToString();
                };
                
            }
        }
        if (dao.DEPT_NO == "")
        {
            txt_EMP_ID.Text = "";
            txt_EMP_NAME.Text = "";
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('非協理工號請重新選擇!');", true);
            return;
        }
        //有block
        IWorkbook workbook = sj0520BO.createstatisticsExcel(dao, "xlsx");
        if (workbook != null)
        {

        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('無資料');doUnBlock();", true);
            return;
        }
        Session["workbook_SJ052_S"] = workbook;
        dwnframe.Attributes["src"] = "WFB2SJ0540_Qry.aspx?FileType_SJ052_S = excel";
        Session["FileType_SJ052_S"] = "excel";
    }
    
   
    protected void LB_FIX_COUNT_Click(object sender, EventArgs e)
    {
        LinkButton lbtn = (LinkButton)sender;
        String empID = lbtn.CommandArgument.ToString();

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doViewFixRec('" + empID + "');", true);

    }
    protected void btn_COMMENTS_Click(object sender, EventArgs e)
    {
        Button lbtn = (Button)sender;
        String empID = lbtn.CommandArgument.ToString();

        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "doViewComments('" + empID + "');", true);

    }
    
    #endregion
    //將Session 的workbook 匯出Excel
    public void exportExcel()
    {
        try
        {
            if (Session["FileType_SJ051_S"] != null && Session["FileType_SJ051_S"].ToString() != "")
            {
                string fileType = Session["FileType_SJ051_S"].ToString();

                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_SJ051_S"];
                    Session["FileType_SJ051_S"] = "";
                    Session["workbook_SJ051_S"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2SJ051_Statistics_1.xlsx");

                }

            }
            if (Session["FileType_SJ052_S"] != null && Session["FileType_SJ052_S"].ToString() != "")
            {
                string fileType = Session["FileType_SJ052_S"].ToString();

                if (fileType == "excel")
                {
                    IWorkbook workBook = (IWorkbook)Session["workbook_SJ052_S"];
                    Session["FileType_SJ052_S"] = "";
                    Session["workbook_SJ052_S"] = null;

                    ExcelHandle.exportExcel(workBook, "FB2SJ052_Statistics_1.xlsx");

                }

            }
        }
        catch (Exception ex)
        {

            throw;
        }

    }

}

