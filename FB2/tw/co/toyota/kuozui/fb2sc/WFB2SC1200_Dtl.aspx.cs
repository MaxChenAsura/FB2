using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class WebContent_fb2sc_WFB2SC1200_Dtl : BasePage
{

    //Service 物件

    private CFB2SC1200BO service = new CFB2SC1200BO();
    private string qdatakey;
    private string key_level;
    protected void Page_Load(object sender, EventArgs e)
    {
        qdatakey = Request.QueryString["qdatakey"];
        key_level = Request.QueryString["key_level"];
        //第一次進入頁面執行
        if (!IsPostBack)
        {
            //產生header資料 主檔
            getHeader(qdatakey);
            //取得明細檔
            if (key_level == "0")
                getDtlData_LEVEL_Is0(qdatakey);
            else
                getDtlData_LEVEL_IsNot0(qdatakey);
        }

    }

    #region "Initial Page"
    private void getHeader(string qdatakey)
    {
        CFB2SC1200DAO dao = new CFB2SC1200DAO();
        DataTable dt = dao.getDtlHeader(qdatakey);
        lb_KIND_CD_TXT.Text = Convert.ToString(dt.Rows[0]["KIND_CD_name"]);
        lb_GROUP_TYPE_TXT.Text = Convert.ToString(dt.Rows[0]["GROUP_TYPE_name"]);
        lb_GROUP_ID_TXT.Text = Convert.ToString(dt.Rows[0]["GROUP_ID"]);
        lb_GROUP_NAME_TXT.Text = Convert.ToString(dt.Rows[0]["GROUP_NAME"]);
        hid_KIND_CD.Value = Convert.ToString(dt.Rows[0]["KIND_CD"]);
        hid_GROUP_ID.Value = Convert.ToString(dt.Rows[0]["GROUP_ID"]);
        hid_GROUP_TYPE.Value = Convert.ToString(dt.Rows[0]["GROUP_TYPE"]);
    }
    //取得選擇項目 明細檔level == 0
    private void getDtlData_LEVEL_Is0(string qdatakey)
    {
        CFB2SC1200DAO dao = new CFB2SC1200DAO();
        DataTable dtSelect = dao.getSelectedData_Is0(qdatakey);
        DataTable dtNonSelect = new DataTable();
        string sub_group_id = "";
        //將明細的salary_id用逗號串起來
        for (int i = 0; i < dtSelect.Rows.Count; i++)
        {
            sub_group_id += "'" + Convert.ToString(dtSelect.Rows[i]["SUB_GROUP_ID"]) + "',";
        }
        sub_group_id = sub_group_id.Trim().Trim(',');

        if (sub_group_id.Length >= 1)
        {
            lb_select.DataSource = dtSelect;
            lb_select.DataTextField = "GROUP_NAME";
            lb_select.DataValueField = "SUB_GROUP_ID";
            lb_select.DataBind();
            //取得未選擇項目 明細檔
            dtNonSelect = dao.getNonSelectedData_Is0(sub_group_id);
            lb_unselect.DataSource = dtNonSelect;
            lb_unselect.DataTextField = "GROUP_NAME";
            lb_unselect.DataValueField = "GROUP_ID";
            lb_unselect.DataBind();
        }
        else //沒有已選擇項目
        {
            dtNonSelect = dao.getNonSelectedData_Is0(sub_group_id);
            lb_unselect.DataSource = dtNonSelect;
            lb_unselect.DataTextField = "GROUP_NAME";
            lb_unselect.DataValueField = "GROUP_ID";
            lb_unselect.DataBind();
        }
    }

    //取得選擇項目 明細檔 level != 0
    private void getDtlData_LEVEL_IsNot0(string qdatakey)
    {
        CFB2SC1200DAO dao = new CFB2SC1200DAO();
        DataTable dtSelect = dao.getSelectedData_IsNot0(qdatakey);
        DataTable dtNonSelect = new DataTable();

        if (dtSelect.Rows.Count > 0)
        {
            lb_select.DataSource = dtSelect;
            lb_select.DataTextField = "GROUP_NAME";
            lb_select.DataValueField = "SUB_GROUP_ID";
            lb_select.DataBind();
            //取得未選擇項目 明細檔
            dtNonSelect = dao.getNonSelectedData_IsNot0(qdatakey,hid_KIND_CD.Value,hid_GROUP_TYPE.Value);
            lb_unselect.DataSource = dtNonSelect;
            lb_unselect.DataTextField = "GROUP_NAME";
            lb_unselect.DataValueField = "GROUP_ID";
            lb_unselect.DataBind();
        }
        else //沒有已選擇項目
        {
            dtNonSelect = dao.getNonSelectedData_IsNot0(qdatakey, hid_KIND_CD.Value, hid_GROUP_TYPE.Value);
            lb_unselect.DataSource = dtNonSelect;
            lb_unselect.DataTextField = "GROUP_NAME";
            lb_unselect.DataValueField = "GROUP_ID";
            lb_unselect.DataBind();
        }
    }
    #endregion

    #region "button event"
    //確認按鈕事件
    protected void WFB2SC1200Ok1_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SC1200BO service = new CFB2SC1200BO();
            string msg = string.Empty;
            string deletemsg = string.Empty;
            string selectedItem = "";
            //先全部刪除
            deletemsg = service.deleteDtlData(hid_KIND_CD.Value, hid_GROUP_TYPE.Value, hid_GROUP_ID.Value);
            if (deletemsg == "0")
            {
                selectedItem = hid_selectedItem.Value.Trim().Trim(',');
                msg = service.addDtlData(hid_KIND_CD.Value, hid_GROUP_TYPE.Value, hid_GROUP_ID.Value, selectedItem);
                if (msg == "0")
                {
                    //取得 明細檔
                    if (key_level == "0")
                        getDtlData_LEVEL_Is0(qdatakey);
                    else
                        getDtlData_LEVEL_IsNot0(qdatakey);
                    Session["SC1200_Is_Search"] = "Y";
                    ScriptManager.RegisterClientScriptBlock(WFB2SC1200Ok1, this.GetType(), "WFB2SC1200Ok1_modSuccessMessage", "alert('" + Resources.Resource.wfb2dl_mod_success + "');$(location).attr('href','WFB2SC1200_Qry.aspx');", true);
                }
                else
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    showMessage("modFailMessage", msg);
                    return;
                }
            }
            else
            {
                deletemsg = msg.Replace("\r\n", "");
                deletemsg = msg.Replace("'", "");
                showMessage("modFailMessage", deletemsg);
                return;
            }

            
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        Session["SC1200_Is_Search"] = "Y";
        Response.Redirect("WFB2SC1200_Qry.aspx");
    }
    #endregion
   
}

