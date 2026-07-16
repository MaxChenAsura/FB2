using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WebContent_comm_Multi_Select_HF010 : System.Web.UI.Page
{
    string declara_year = "";
    string emp_id = "";
    string seq = "";
    string compet_area = "";
    

    protected void Page_Load(object sender, EventArgs e)
    {
        //取得已有選取的資料
        declara_year = Request.QueryString["declara_year"].ToString();
        emp_id = Request.QueryString["emp_id"].ToString();
        seq = Request.QueryString["seq"].ToString();
        compet_area = Request.QueryString["compet_area"].ToString();
        if (!Page.IsPostBack)
        {
            //取得代碼
            getMultiData();
        }
    }

    //職能領域的多重選單
    private void getMultiData()
    {
        try
        {
            CFB2HF0100BO hf010BO = new CFB2HF0100BO();
            CFB2HF0100DAO hf010DAO = new CFB2HF0100DAO();
            hf010DAO.DECLARA_YEAR = declara_year;
            hf010DAO.EMP_ID = emp_id;
            hf010DAO.SEQ = seq;
            //將代碼繫結至listbox
            DataTable dt = new DataTable();
            string selectedCompetArea = ""; //已選擇的職務領域
            if (compet_area == "")
            {
                dt = hf010BO.getCOMPET_AREA(hf010DAO);
            }
            else {
                selectedCompetArea = compet_area;
            }
            if (dt.Rows.Count > 0)
            {
                selectedCompetArea = dt.Rows[0]["COMPET_AREA_CD"].ToString();
            }

            dt = hf010BO.getNonSelectedData(hf010DAO, selectedCompetArea);

            lb_unselect.DataSource = dt;
            lb_unselect.DataTextField = "SUB_DESC";
            lb_unselect.DataValueField = "SUB_CD";
            lb_unselect.DataBind();

            dt = hf010BO.getSelectedData(hf010DAO, selectedCompetArea);
            lb_select.DataSource = dt;
            lb_select.DataTextField = "SUB_DESC";
            lb_select.DataValueField = "SUB_CD";
            lb_select.DataBind();

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "error", "alert('" + ex.Message.Replace("\r\n", "").Replace("'", "\"") + "');", true);
        }
    }

}