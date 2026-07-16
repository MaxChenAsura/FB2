using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WebContent_fb2sf_WFB2SF0100_Qry : BasePage
{
    CFB2SF0100BO service = new CFB2SF0100BO();

    #region gv_result新刪修
    protected void WFB2SF0100Add_Click(object sender, EventArgs e)
    {
        try
        {
            //ViewState["Queryble"] = true;
            gv_result.PagerSettings.Visible = false;
            //grid1Button
            WFB2SF0100Search.Enabled = false;
            btn_clear.Enabled = false;

            WFB2SF0100OK.Visible = true;
            btn_cancel.Visible = true;

            WFB2SF0100Add.Visible = false;
            WFB2SF0100Edit.Visible = false;
            WFB2SF0100Delete.Visible = false;
            //grid2Button
            gv_result2.Visible = false;
            WFB2SF0101Add.Visible = false;
            WFB2SF0101Edit.Visible = false;
            WFB2SF0102Edit.Visible = false;
            WFB2SF0101Delete.Visible = false;
            OnePage2.Visible = false;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("EMP_ID", 0, 10);

            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = true;
            }
            else
            {
                gv_result.ShowFooter = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SF0100Delete_Click(object sender, EventArgs e)
    {
        try
        {
            List<int> delindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check_gv1")).Checked)
                {
                    delindex.Add(i);
                }
            }
            for (int i = 0; i < delindex.Count; i++)
            {
                if (((Label)gv_result.Rows[delindex[i]].FindControl("lb_APPROVE_STATUS_DESC")).Text.IndexOf("Y") != -1)
                {
                    ScriptManager.RegisterClientScriptBlock(WFB2SF0100Search, this.GetType(), "error", "alert('" + hid_wfb2sf_AlreadyCheckMessage.Value + "');", true);
                    return;
                }

            }

            ScriptManager.RegisterClientScriptBlock(WFB2SF0100Search, this.GetType(), "error", "checkDelClick();", true);

            //grid2Button
            gv_result2.Visible = false;
            WFB2SF0101Add.Visible = false;
            WFB2SF0101Edit.Visible = false;
            WFB2SF0102Edit.Visible = false;
            WFB2SF0101Delete.Visible = false;
            //OnePage2.Visible = false;
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void Del_AfterConfirm_Click(object sender, EventArgs e)
    {
        //檢查勾選項目
        List<string> delitem_list = new List<string>();
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            if (((CheckBox)gv_result.Rows[i].FindControl("cb_check_gv1")).Checked)
            {
                delitem_list.Add(gv_result.DataKeys[i].Value.ToString());
            }
        }
        if (delitem_list.Count() == 0)
        {
            return;
        }
        else
        {
            string msg = service.Delete(delitem_list);

            if (msg != "0")
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
            else
                showMessage("deleteSuccessMessage");

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView(ViewState["SortExpression"].ToString(), 0, 10);
        }
    }
    protected void WFB2SF0100Edit_Click(object sender, EventArgs e)
    {
        try
        {


            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                if (((CheckBox)gv_result.Rows[i].FindControl("cb_check_gv1")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() == 1)
            {
                if (((Label)gv_result.Rows[editindex[0]].FindControl("lb_APPROVE_STATUS_DESC")).Text.IndexOf("Y") != -1)
                {
                    ScriptManager.RegisterClientScriptBlock(WFB2SF0100Search, this.GetType(), "error", "alert('" + hid_wfb2sf_AlreadyCheckMessage_Mod.Value + "');", true);
                    return;
                }
                else
                {
                    HID_Freeze.Value = "N";
                    gv_result.EditIndex = editindex[0];
                }
            }
            else
            {
                return;
            }
            gv_result.PagerSettings.Visible = false;
            //disable查詢清除按鈕
            WFB2SF0100Search.Enabled = false;
            btn_clear.Enabled = false;
            WFB2SF0100OK.Visible = true;
            btn_cancel.Visible = true;
            //gridButton
            WFB2SF0100Add.Visible = false;
            WFB2SF0100Edit.Visible = false;
            WFB2SF0100Delete.Visible = false;
            //grid2Button
            gv_result2.Visible = false;
            WFB2SF0101Add.Visible = false;
            WFB2SF0101Edit.Visible = false;
            WFB2SF0102Edit.Visible = false;
            WFB2SF0101Delete.Visible = false;
            //OnePage2.Visible = false;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SF0100OK_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SF0100DAO fb2sf = new CFB2SF0100DAO();
            //新增且沒有資料
            if (gv_result.Rows.Count == 0)
            {
                TextBox txt_NEW_EMP_ID = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_ID");
                TextBox txt_NEW_DOC_NO = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_DOC_NO");
                TextBox txt_NEW_START_DT = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_START_DT");
                TextBox txt_NEW_SALARY_RATE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_SALARY_RATE");
                TextBox txt_NEW_BONUS_RATE = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_BONUS_RATE");

                fb2sf.data_key = txt_NEW_EMP_ID.Text + txt_NEW_DOC_NO.Text;
                fb2sf.EMP_ID = txt_NEW_EMP_ID.Text;
                fb2sf.DOC_NO = txt_NEW_DOC_NO.Text;
                fb2sf.START_DT = txt_NEW_START_DT.Text;
                fb2sf.SALARY_RATE = txt_NEW_SALARY_RATE.Text;
                fb2sf.BONUS_RATE = txt_NEW_BONUS_RATE.Text;
                string msg = service.Add(fb2sf);
                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    gv_result.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                }
            }
            else
            {
                //新增有資料
                if (gv_result.EditIndex == -1)
                {
                    fb2sf.data_key = HID_NEW_EMP_ID.Value + HID_NEW_DOC_NO.Value;
                    fb2sf.EMP_ID = HID_NEW_EMP_ID.Value;
                    fb2sf.DOC_NO = HID_NEW_DOC_NO.Value;
                    fb2sf.START_DT = HID_NEW_START_DT.Value;
                    fb2sf.SALARY_RATE = HID_NEW_SALARY_RATE.Value;
                    fb2sf.BONUS_RATE = HID_NEW_BONUS_RATE.Value;
                    string msg = service.Add(fb2sf);
                    if (msg != "0")
                    {
                        HID_Freeze.Value = "N";
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        gv_result.PagerSettings.Visible = false;
                        showMessage("addFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                        return;
                    }
                    else
                    {
                        HID_Freeze.Value = "Y";
                        showMessage("addSuccessMessage");
                    }
                }
                else
                {

                    //更新
                    TextBox txt_EDIT_START_DT = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_START_DT");
                    TextBox txt_EDIT_SALARY_RATE = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_SALARY_RATE");
                    TextBox txt_EDIT_BONUS_RATE = (TextBox)gv_result.Rows[gv_result.EditIndex].FindControl("txt_EDIT_BONUS_RATE");
                    fb2sf.data_key = gv_result.DataKeys[gv_result.EditIndex].Value.ToString();
                    fb2sf.START_DT = txt_EDIT_START_DT.Text;
                    fb2sf.SALARY_RATE = txt_EDIT_SALARY_RATE.Text;
                    fb2sf.BONUS_RATE = txt_EDIT_BONUS_RATE.Text;
                    string msg = service.Update(fb2sf);
                    if (msg != "0")
                    {
                        HID_Freeze.Value = "N";
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        gv_result.PagerSettings.Visible = false;
                        showMessage("modFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                        return;
                    }
                    else
                    {
                        HID_Freeze.Value = "Y";
                        showMessage("modSuccessMessage");
                    }
                }
            }

            ViewState["NewPageIndex"] = gv_result.PageIndex;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
            else
                gv_result.PageSize = 10;

            //gv_result.DataSourceID = "ods1";
            //gv_result.DataKeyNames = new string[] { "qdatakey" };
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView(ViewState["SortExpression"].ToString(), 0, 10);
            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            //enable查詢清除按鈕
            WFB2SF0100Search.Enabled = true;
            btn_clear.Enabled = true;

            WFB2SF0100OK.Visible = false;
            btn_cancel.Visible = false;
            WFB2SF0100Add.Visible = true;
            WFB2SF0100Edit.Visible = true;
            WFB2SF0100Delete.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_cancel_Click(object sender, EventArgs e)
    {
        HID_Freeze.Value = "Y";
        WFB2SF0100Search.Enabled = true;
        btn_clear.Enabled = true;

        gv_result.EditIndex = -1;
        gv_result.ShowFooter = false;
        if (gv_result.Rows.Count == 0)
        {
            gv_result.Visible = false;
        }

        WFB2SF0100OK.Visible = false;
        btn_cancel.Visible = false;
        WFB2SF0100Add.Visible = true;
        WFB2SF0100Edit.Visible = true;
        WFB2SF0100Delete.Visible = true;
    }
    #endregion

    #region gv_result2新刪修
    protected void WFB2SF0101Add_Click(object sender, EventArgs e)
    {
        try
        {
            gv_result2.PagerSettings.Visible = false;
            //disable查詢清除按鈕
            WFB2SF0100Search.Enabled = false;
            btn_clear.Enabled = false;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                ((Button)gv_result.Rows[i].FindControl("WFB2SF0100Dtl")).Enabled = false;
                ((Button)gv_result.Rows[i].FindControl("WFB2SF0100DataCheck")).Enabled = false;
            }

            //gridButton
            WFB2SF0100Add.Enabled = false;
            WFB2SF0100Edit.Enabled = false;
            WFB2SF0100Delete.Enabled = false;
            //grid2Button
            WFB2SF0101Add.Visible = false;
            WFB2SF0101Edit.Visible = false;
            WFB2SF0102Edit.Visible = false;
            WFB2SF0101Delete.Visible = false;
            WFB2SF0101OK.Visible = true;
            btn_cancel2.Visible = true;

            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                GetGridView2("CHG_STATUS", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                GetGridView2("CHG_STATUS", 0, 10);

            if (gv_result2.Rows.Count == 0)
            {
                gv_result2.Visible = true;
            }
            else
            {
                gv_result2.ShowFooter = true;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SF0101Edit_Click(object sender, EventArgs e)
    {
        try
        {


            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result2.Rows.Count; i++)
            {
                if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check_gv2")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() == 1)
            {
                if (((Label)gv_result2.Rows[editindex[0]].FindControl("lb_IS_VAILD")).Text.IndexOf("N") != -1)
                {
                    ScriptManager.RegisterClientScriptBlock(WFB2SF0100Search, this.GetType(), "error", "alert('" + hid_wfb2sf_NotAllowEditMessage.Value + "');", true);
                    return;
                }
                else
                {
                    gv_result2.EditIndex = editindex[0];
                }
            }
            else
            {
                return;
            }
            gv_result2.PagerSettings.Visible = false;
            //disable查詢清除按鈕
            WFB2SF0100Search.Enabled = false;
            btn_clear.Enabled = false;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                ((Button)gv_result.Rows[i].FindControl("WFB2SF0100Dtl")).Enabled = false;
                ((Button)gv_result.Rows[i].FindControl("WFB2SF0100DataCheck")).Enabled = false;
            }

            //gridButton
            WFB2SF0100Add.Enabled = false;
            WFB2SF0100Edit.Enabled = false;
            WFB2SF0100Delete.Enabled = false;
            //grid2Button
            WFB2SF0101Add.Visible = false;
            WFB2SF0101Edit.Visible = false;
            WFB2SF0102Edit.Visible = false;
            WFB2SF0101Delete.Visible = false;
            WFB2SF0101OK.Visible = true;
            btn_cancel2.Visible = true;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SF0101Delete_Click(object sender, EventArgs e)
    {
        try
        {
            //檢查勾選項目
            List<string> delitem_list = new List<string>();
            List<string> doc_no_item_list = new List<string>();
            List<string> amountitem_list = new List<string>();
            List<string> chg_statusitem_list = new List<string>();
            for (int i = 0; i < this.gv_result2.Rows.Count; i++)
            {
                if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check_gv2")).Checked)
                {
                    delitem_list.Add(gv_result2.DataKeys[i].Value.ToString());
                    doc_no_item_list.Add(((Label)gv_result2.Rows[i].FindControl("lb_DOC_NO")).Text);
                    amountitem_list.Add(((Label)gv_result2.Rows[i].FindControl("lb_AMOUNT")).Text);
                    chg_statusitem_list.Add(((Label)gv_result2.Rows[i].FindControl("lb_CHG_STATUS_DESC")).Text);
                }
            }
            if (delitem_list.Count() == 0)
            {
                return;
            }
            else
            {
                string msg = service.Delete_Dtl(delitem_list, doc_no_item_list, amountitem_list, chg_statusitem_list, HID_EMP_ID.Value);

                if (msg != "0")
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
                else
                    showMessage("deleteSuccessMessage");

                if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                    GetGridView2("CHG_STATUS", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
                else
                    GetGridView2("CHG_STATUS", 0, 10);
                //todo(更新gv_result1)
                gv_result.DataSourceID = "ods1";
                gv_result.DataKeyNames = new string[] { "qdatakey" };
            }
        }

        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SF0101OK_Click(object sender, EventArgs e)
    {
        try
        {
            CFB2SF0100DAO fb2sf = new CFB2SF0100DAO();
            //新增且沒有資料
            if (gv_result2.Rows.Count == 0)
            {
                DropDownList txt_NEW_PAY_TARGET_DESC = (DropDownList)gv_result2.Controls[0].Controls[0].FindControl("ddl_PAY_TARGET_DESC");
                DropDownList txt_NEW_IS_VAILD = (DropDownList)gv_result2.Controls[0].Controls[0].FindControl("ddl_IS_VAILD");
                TextBox txt_NEW_DOC_NO2 = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_DOC_NO");
                TextBox txt_NEW_CREDITOR = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_CREDITOR");
                TextBox txt_NEW_VENDOR_ID = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_VENDOR_ID");
                TextBox txt_NEW_AMOUNT = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_AMOUNT");
                Label txt_NEW_RATIO = (Label)gv_result2.Controls[0].Controls[0].FindControl("lb_RATIO");
                Label txt_NEW_EFFECT_SDT = (Label)gv_result2.Controls[0].Controls[0].FindControl("lb_EFFECT_SDT");
                Label txt_NEW_EFFECT_EDT = (Label)gv_result2.Controls[0].Controls[0].FindControl("lb_EFFECT_EDT");
                TextBox txt_NEW_MEMO = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_MEMO");
                TextBox txt_NEW_MEMODESC = (TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_MEMODESC");

                fb2sf.data_key = HID_EMP_ID.Value + txt_NEW_DOC_NO2.Text;
                fb2sf.EMP_ID = HID_EMP_ID.Value;
                fb2sf.CHG_STATUS = "N";
                fb2sf.DOC_NO = txt_NEW_DOC_NO2.Text;
                fb2sf.PAY_TARGET = txt_NEW_PAY_TARGET_DESC.Text;
                fb2sf.CREDITOR = txt_NEW_CREDITOR.Text;
                fb2sf.VENDOR_ID = txt_NEW_VENDOR_ID.Text;
                fb2sf.AMOUNT = txt_NEW_AMOUNT.Text;
                fb2sf.RATIO = txt_NEW_RATIO.Text;
                fb2sf.EFFECT_SDT = txt_NEW_EFFECT_SDT.Text;
                fb2sf.EFFECT_EDT = txt_NEW_EFFECT_EDT.Text;
                fb2sf.IS_VAILD = txt_NEW_IS_VAILD.Text;
                fb2sf.MEMO = txt_NEW_MEMO.Text;
                fb2sf.MEMODESC = txt_NEW_MEMODESC.Text;
                string msg = service.Add_Dtl(fb2sf, HID_EMP_ID.Value, HID_NEW_DOC_NO2.Value);
                if (msg != "0")
                {
                    msg = msg.Replace("\r\n", "");
                    msg = msg.Replace("'", "");
                    gv_result2.PagerSettings.Visible = false;
                    showMessage("addFailMessage", msg);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                    return;
                }
                else
                {
                    showMessage("addSuccessMessage");
                }
            }
            else
            {
                //新增有資料
                if (gv_result2.EditIndex == -1)
                {
                    fb2sf.data_key = HID_EMP_ID.Value + HID_NEW_DOC_NO2.Value;
                    fb2sf.EMP_ID = HID_EMP_ID.Value;
                    fb2sf.CHG_STATUS = "N";
                    fb2sf.DOC_NO = HID_NEW_DOC_NO2.Value;
                    fb2sf.PAY_TARGET = HID_PAY_TARGET_DESC.Value;
                    fb2sf.CREDITOR = HID_NEW_CREDITOR.Value;
                    fb2sf.VENDOR_ID = HID_NEW_VENDOR_ID.Value;
                    fb2sf.AMOUNT = HID_NEW_AMOUNT.Value;
                    fb2sf.RATIO = HID_NEW_RATIO.Value;
                    fb2sf.EFFECT_SDT = HID_NEW_EFFECT_SDT.Value;
                    fb2sf.EFFECT_EDT = HID_NEW_EFFECT_EDT.Value;
                    fb2sf.IS_VAILD = HID_IS_VAILD.Value;
                    fb2sf.MEMO = HID_NEW_MEMO.Value;
                    fb2sf.MEMODESC = HID_NEW_MEMODESC.Value;

                    string msg = service.Add_Dtl(fb2sf, HID_EMP_ID.Value, HID_NEW_DOC_NO2.Value);
                    if (msg != "0")
                    {
                        HID_Freeze2.Value = "N";
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        gv_result2.PagerSettings.Visible = false;
                        showMessage("addFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                        return;
                    }
                    else
                    {
                        HID_Freeze2.Value = "Y";
                        showMessage("addSuccessMessage");
                    }
                }
                else
                {

                    //更新
                    Label lb_DOC_NO = (Label)gv_result2.Rows[gv_result2.EditIndex].FindControl("lb_DOC_NO");
                    DropDownList ddl_PAY_TARGET_DESC = (DropDownList)gv_result2.Rows[gv_result2.EditIndex].FindControl("ddl_PAY_TARGET_DESC");
                    TextBox txt_EDIT_CREDITOR = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_EDIT_CREDITOR");
                    Label txt_EDIT_VENDOR_ID = (Label)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_EDIT_VENDOR_ID");
                    TextBox txt_EDIT_AMOUNT = (TextBox)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_EDIT_AMOUNT");
                    Label lb_RATIO = (Label)gv_result2.Rows[gv_result2.EditIndex].FindControl("lb_RATIO");
                    Label lb_EFFECT_EDT = (Label)gv_result2.Rows[gv_result2.EditIndex].FindControl("lb_EFFECT_EDT");
                    DropDownList ddl_IS_VAILD = (DropDownList)gv_result2.Rows[gv_result2.EditIndex].FindControl("ddl_IS_VAILD");
                    Label txt_EDIT_MEMO = (Label)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_EDIT_MEMO");
                    Label txt_EDIT_MEMODESC = (Label)gv_result2.Rows[gv_result2.EditIndex].FindControl("txt_EDIT_MEMODESC");

                    fb2sf.data_key = gv_result2.DataKeys[gv_result2.EditIndex].Value.ToString();
                    fb2sf.EMP_ID = HID_EMP_ID.Value;
                    fb2sf.DOC_NO = lb_DOC_NO.Text;
                    fb2sf.CHG_STATUS = "U";
                    fb2sf.PAY_TARGET = ddl_PAY_TARGET_DESC.Text;
                    fb2sf.CREDITOR = txt_EDIT_CREDITOR.Text;
                    fb2sf.VENDOR_ID = txt_EDIT_VENDOR_ID.Text;
                    fb2sf.AMOUNT = txt_EDIT_AMOUNT.Text;
                    fb2sf.RATIO = lb_RATIO.Text;
                    fb2sf.EFFECT_EDT = lb_EFFECT_EDT.Text;
                    fb2sf.IS_VAILD = ddl_IS_VAILD.Text;
                    fb2sf.MEMO = txt_EDIT_MEMO.Text;
                    fb2sf.MEMODESC = txt_EDIT_MEMODESC.Text;
                    string msg = service.Update_Dtl(fb2sf);
                    if (msg != "0")
                    {
                        msg = msg.Replace("\r\n", "");
                        msg = msg.Replace("'", "");
                        gv_result2.PagerSettings.Visible = false;
                        showMessage("modFailMessage", msg);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                        return;
                    }
                    else
                    {
                        showMessage("modSuccessMessage");
                    }
                }
            }

            ViewState["NewPageIndex2"] = gv_result2.PageIndex;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
            else
                gv_result2.PageSize = 10;

            //gv_result2.DataSourceID = "ods2";
            //gv_result2.DataKeyNames = new string[] { "qdatakey2" };
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                GetGridView2("CHG_STATUS", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
            else
                GetGridView2("CHG_STATUS", 0, 10);
            gv_result2.EditIndex = -1;
            gv_result2.ShowFooter = false;

            //todo(更新gv_result1)
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };

            //按鈕控制
            WFB2SF0100Search.Enabled = true;
            btn_clear.Enabled = true;
            for (int i = 0; i < this.gv_result.Rows.Count; i++)
            {
                ((Button)gv_result.Rows[i].FindControl("WFB2SF0100Dtl")).Enabled = true;
                ((Button)gv_result.Rows[i].FindControl("WFB2SF0100DataCheck")).Enabled = true;
            }
            //gv_result2.EditIndex = -1;
            //gv_result2.ShowFooter = false;
            //if (gv_result2.Rows.Count == 0)
            //{
            //    gv_result2.Visible = false;
            //}
            //gridButton
            WFB2SF0100Add.Enabled = true;
            WFB2SF0100Edit.Enabled = true;
            WFB2SF0100Delete.Enabled = true;
            //grid2Button
            WFB2SF0101Add.Visible = true;
            WFB2SF0101Edit.Visible = true;
            WFB2SF0102Edit.Visible = true;
            WFB2SF0101Delete.Visible = true;
            WFB2SF0101OK.Visible = false;
            btn_cancel2.Visible = false;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_cancel2_Click(object sender, EventArgs e)
    {
        HID_Freeze2.Value = "Y";
        WFB2SF0100Search.Enabled = true;
        btn_clear.Enabled = true;
        for (int i = 0; i < this.gv_result.Rows.Count; i++)
        {
            ((Button)gv_result.Rows[i].FindControl("WFB2SF0100Dtl")).Enabled = true;
            ((Button)gv_result.Rows[i].FindControl("WFB2SF0100DataCheck")).Enabled = true;
        }
        gv_result2.EditIndex = -1;
        gv_result2.ShowFooter = false;
        if (gv_result2.Rows.Count == 0)
        {
            gv_result2.Visible = false;
        }
        //gridButton
        WFB2SF0100Add.Enabled = true;
        WFB2SF0100Edit.Enabled = true;
        WFB2SF0100Delete.Enabled = true;
        //grid2Button
        WFB2SF0101Add.Visible = true;
        WFB2SF0101Edit.Visible = true;
        WFB2SF0102Edit.Visible = true;
        WFB2SF0101Delete.Visible = true;
        WFB2SF0101OK.Visible = false;
        btn_cancel2.Visible = false;
    }
    #endregion







    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["Queryble"] = false;
        if (!IsPostBack)
        {
            realeaseConditions();
        }
        gv_result.PagerSettings.Visible = true;
        //gv_result2.PagerSettings.Visible = true;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
        string event_target = Request.Form.Get("__EVENTTARGET");
        string event_argu = Request.Form.Get("__EVENTARGUMENT");
        string value = HID_VALUE.Value;
        string type = HID_TYPE.Value;
        if (event_target == "question")
        {
            if (event_argu == "true")
            {
                empCheck(value, type);
            }
        }
        if (HID_PageRow.Value != "")
        {
            if (ViewState["SortExpression"] != null && ViewState["SortExpression"].ToString() != "")
                GetGridView(ViewState["SortExpression"].ToString(), 0, Convert.ToInt32(HID_PageRow.Value));
            else
                GetGridView("EMP_ID", 0, Convert.ToInt32(HID_PageRow.Value));

        }
        if (HID_PageRow2.Value != "")
        {
            if (ViewState["SortExpression2"] != null && ViewState["SortExpression2"].ToString() != "")
                GetGridView2(ViewState["SortExpression2"].ToString(), 0, Convert.ToInt32(HID_PageRow2.Value));
            else
                GetGridView2("CHG_STATUS", 0, Convert.ToInt32(HID_PageRow2.Value));
        }
    }
    private void GetGridView(string SortExpression, int pageindex, Int32 pagesize)
    {
        try
        {
            if (txt_EMP_ID.Text == "")
                txt_EMP_NAME.Text = "";
            if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value && HID_PageRow.Value != ""))
                //if (ViewState["PerPageRow"] == null || (ViewState["PerPageRow"] != null && ViewState["PerPageRow"] != HID_PageRow.Value))
                ViewState["PerPageRow"] = HID_PageRow.Value;

            ViewState["NewPageIndex"] = pageindex;
            if (ViewState["SortExpression"] == null)
                getSortDirection("EMP_ID");
            gv_result.Visible = true;
            gv_result.PageIndex = pageindex;
            gv_result.PageSize = pagesize;
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            gv_result.DataBind();
            if (gv_result.Rows.Count == 0)
            {
                gv_result.Visible = false;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }
            HID_PageRow.Value = "";
            Session["SF0100_ddlPerPageRow1"] = ViewState["PerPageRow"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    private void GetGridView2(string SortExpression, int pageindex, Int32 pagesize2)
    {
        try
        {
            if (txt_EMP_ID.Text == "")
                txt_EMP_NAME.Text = "";
            if (ViewState["PerPageRow2"] == null || (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"] != HID_PageRow2.Value && HID_PageRow2.Value != ""))
                ViewState["PerPageRow2"] = HID_PageRow2.Value;

            ViewState["NewPageIndex2"] = pageindex;
            if (ViewState["SortExpression2"] == null)
                getSortDirection2("CHG_STATUS");
            gv_result2.Visible = true;
            //gv_result2.PageIndex = pageindex;
            gv_result2.PageSize = pagesize2;

            gv_result2.DataSourceID = "ods2";
            gv_result2.DataKeyNames = new string[] { "qdatakey2" };
            gv_result2.DataBind();



            if (gv_result2.Rows.Count == 0)
            {
                gv_result2.Visible = false;
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }
            HID_PageRow2.Value = "";
            Session["SF0100_ddlPerPageRow2"] = ViewState["PerPageRow2"];
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void empCheck(string EMP_ID, string type)
    {
        try
        {
            if (type != "search")
            {
                gv_result.PagerSettings.Visible = false;
                HID_Freeze.Value = "N";
            }

            //string EMP_ID = txt_EMP_ID.Text;
            CFB2SF0100DAO fb2ia = new CFB2SF0100DAO();
            DataTable dt = fb2ia.emp(EMP_ID);
            string msg = "輸入代碼不存在!";
            if (dt.Rows.Count == 0)
            {
                if (type == "search")
                {
                    txt_EMP_ID.Text = "";
                    txt_EMP_NAME.Text = "";
                }
                else
                {
                    if (gv_result.Rows.Count > 0)
                    {
                        TextBox emp_id_add = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_ID");
                        TextBox emp_name_add = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_NAME");
                        emp_id_add.Text = "";
                        emp_name_add.Text = "";
                    }
                    else
                    {
                        TextBox emp_id = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_ID");
                        TextBox emp_name = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_NAME");
                        emp_id.Text = "";
                        emp_name.Text = "";
                    }
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + msg + "');", true);
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (type == "search")
                    {
                        txt_EMP_NAME.Text = Convert.ToString(dr["EMP_NAME"]);
                    }
                    else
                    {
                        if (gv_result.Rows.Count > 0)
                        {
                            TextBox emp_name_add = (TextBox)gv_result.FooterRow.FindControl("txt_NEW_EMP_NAME");
                            emp_name_add.Text = Convert.ToString(dr["EMP_NAME"]);
                        }
                        else
                        {
                            TextBox emp_name = (TextBox)gv_result.Controls[0].Controls[0].FindControl("txt_NEW_EMP_NAME");
                            emp_name.Text = Convert.ToString(dr["EMP_NAME"]);
                        }
                    }

                }
            }
            HID_TYPE.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void WFB2SF0100Search_Click(object sender, EventArgs e)
    {

        try
        {

            ViewState["Queryble"] = true;
            //keepConditions(true);
            ViewState["SetPerRow"] = true;
            ViewState["SortExpression"] = null;
            ViewState["SortDirection"] = null;

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                GetGridView("EMP_ID", 0, Convert.ToInt32(ViewState["PerPageRow"]));
            else
                GetGridView("EMP_ID", 0, 10);
            gv_result2.Visible = false;
            OnePage2.Visible = false;

            gv_result.EditIndex = -1;
            gv_result.ShowFooter = false;

            if (gv_result.Rows.Count > 0)
            {
                WFB2SF0100Add.Visible = true;
                WFB2SF0100Edit.Visible = true;
                WFB2SF0100Delete.Visible = true;
                WFB2SF0101Add.Visible = false;
                WFB2SF0101Edit.Visible = false;
                WFB2SF0102Edit.Visible = false;
                WFB2SF0101Delete.Visible = false;
            }
            else
            {
                WFB2SF0100Add.Visible = true;
                WFB2SF0100Edit.Visible = false;
                WFB2SF0100Delete.Visible = false;
                WFB2SF0101Add.Visible = false;
                WFB2SF0101Edit.Visible = false;
                WFB2SF0102Edit.Visible = false;
                WFB2SF0101Delete.Visible = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount1"] = e.ReturnValue;
    }
    protected void ods1_Selected2(object sender, ObjectDataSourceStatusEventArgs e)
    {
        base.ods1_Selected(sender, e);
        ViewState["TotalCount2"] = e.ReturnValue;
    }
    protected void obs1_Selecting2(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //if (!IsPostBack)
        //{
        //    //e.Cancel = true;
        //}
        if (ViewState["SortExpression2"] != null && ViewState["SortDirection2"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression2"] + " " + ViewState["SortDirection2"];
    }
    //設定排序
    protected string getSortDirection2(string column, string sort = "ASC")
    {
        //ViewState["Queryble"] = false;
        string sortDirection = sort;
        string sortExpression = ViewState["SortExpression2"] as string;

        if (sortExpression != null)
        {
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection2"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }
        ViewState["SortDirection2"] = sortDirection;
        ViewState["SortExpression2"] = column;
        return sortDirection;
    }
    protected void gv_result_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex"] = e.NewPageIndex;
        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;

        gv_result.DataSourceID = "ods1";
        gv_result.DataKeyNames = new string[] { "qdatakey" };

    }
    protected void gv_result2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ViewState["NewPageIndex2"] = e.NewPageIndex;

        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;


        gv_result2.DataSourceID = "ods2";
        gv_result2.DataKeyNames = new string[] { "qdatakey2" };
    }
    protected void gv_result_RowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Pager && gv_result.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount1"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            ddllist.ID = "ddlPerPageRow";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow.Value != "")
                ddllist.SelectedValue = HID_PageRow.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
        }
    }
    protected void gv_result2_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //設定新增列的下拉選單值
        if (e.Row.RowType == DataControlRowType.EmptyDataRow || e.Row.RowType == DataControlRowType.Footer || e.Row.RowState.HasFlag(DataControlRowState.Edit))
        {
            DropDownList ddl_PAY_TARGET_DESC = (DropDownList)e.Row.FindControl("ddl_PAY_TARGET_DESC");
            DropDownList ddl_IS_VAILD = (DropDownList)e.Row.FindControl("ddl_IS_VAILD");
            if (ddl_PAY_TARGET_DESC != null)
            {
                DataTable dt = new DataTable();
                dt = utilities.getCommCode("SF", "PAY_TARGET", "", "");
                ddl_PAY_TARGET_DESC.Items.Add(new ListItem("", "-1"));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ddl_PAY_TARGET_DESC.Items.Add(new ListItem(dt.Rows[i]["sub_desc"].ToString(), dt.Rows[i]["sub_cd"].ToString()));
                    }
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.Pager && gv_result2.PageCount > 1)
        {
            TableCell tc = new TableCell();
            tc.HorizontalAlign = HorizontalAlign.Right;
            tc.Text = " 總筆數：" + ViewState["TotalCount2"].ToString();
            Table t = (Table)e.Row.Cells[0].Controls[0];
            t.HorizontalAlign = HorizontalAlign.Left;
            TableCell tc2 = new TableCell();
            DropDownList ddllist = new DropDownList();
            ddllist.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            ddllist.ID = "ddlPerPageRow2";
            ddllist.Items.Add(new ListItem("每頁10筆", "10"));
            ddllist.Items.Add(new ListItem("每頁20筆", "20"));
            ddllist.Items.Add(new ListItem("每頁30筆", "30"));
            ddllist.Items.Add(new ListItem("每頁40筆", "40"));
            ddllist.Items.Add(new ListItem("每頁50筆", "50"));
            if (HID_PageRow2.Value != "")
                ddllist.SelectedValue = HID_PageRow2.Value;
            ddllist.Attributes["onchange"] = "javascript:ShowRecord('ddlPerPageRow2')";
            ddllist.AutoPostBack = true;
            if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                ddllist.SelectedValue = ViewState["PerPageRow2"].ToString();
            tc2.Controls.Add(ddllist);
            TableRow tr = (TableRow)e.Row.Cells[0].Controls[0].Controls[0];
            tr.HorizontalAlign = HorizontalAlign.Right;
            tr.Cells.Add(tc);
            tr.Cells.AddAt(0, tc2);
        }
    }
    protected void gv_result_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result.PageIndex = (int)ViewState["NewPageIndex"];

        if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
            gv_result.PageSize = Convert.ToInt32(ViewState["PerPageRow"]);
        else
            gv_result.PageSize = 10;
        if (((GridView)sender).ID == "gv_result")
        {
            gv_result.DataSourceID = "ods1";
            gv_result.DataKeyNames = new string[] { "qdatakey" };
            getSortDirection(e.SortExpression);
        }
    }
    protected void gv_result2_Sorting(object sender, GridViewSortEventArgs e)
    {
        gv_result2.PageIndex = (int)ViewState["NewPageIndex2"];

        if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
            gv_result2.PageSize = Convert.ToInt32(ViewState["PerPageRow2"]);
        else
            gv_result2.PageSize = 10;
        if (((GridView)sender).ID == "gv_result2")
        {
            gv_result2.DataSourceID = "ods2";
            gv_result2.DataKeyNames = new string[] { "qdatakey2" };
            getSortDirection2(e.SortExpression);
            //gv_result2.ShowFooter = false;
            //gv_result2.EditIndex = -1;
        }

    }
    protected void gv_result_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  (e.Row.RowState == DataControlRowState.Alternate ||
                   e.Row.RowState == DataControlRowState.Selected))
            e.Row.CssClass = "alternate";

        foreach (TableCell tc in e.Row.Cells)
        {
            tc.Attributes["style"] = "border-style:solid;border-width:1px; border-color:#FFFFFF";


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
    protected void gv_result2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView DataRow = (DataRowView)e.Row.DataItem;

        #region 修改的檢核與設定
        if (e.Row.RowState.HasFlag(DataControlRowState.Edit))
        {
            //下拉顯示  
            ((DropDownList)e.Row.FindControl("ddl_PAY_TARGET_DESC")).SelectedValue = Convert.ToString(DataRow["PAY_TARGET_DESC"]).Substring(0, 1);
            ((DropDownList)e.Row.FindControl("ddl_IS_VAILD")).SelectedValue = Convert.ToString(DataRow["IS_VAILD"]);
            //if子GRIDE.償還金額>0,PAY_TARGET.CREDITOR.AMOUNT不允修改
            Label lb_TOTAL_AMT = (Label)e.Row.FindControl("lb_TOTAL_AMT");
            string TOTAL_AMT = lb_TOTAL_AMT.Text.Replace(",", "");
            if (Convert.ToInt32(TOTAL_AMT) > 0)
            {
                ((DropDownList)e.Row.FindControl("ddl_PAY_TARGET_DESC")).Enabled = false;
                ((TextBox)e.Row.FindControl("txt_EDIT_CREDITOR")).Enabled = false;
                ((TextBox)e.Row.FindControl("txt_EDIT_AMOUNT")).Enabled = false;
            }
            if (((DropDownList)e.Row.FindControl("ddl_PAY_TARGET_DESC")).SelectedValue == "E")
            {
                ((TextBox)e.Row.FindControl("txt_EDIT_CREDITOR")).Enabled = false;
            }
            //  if GRIDE.支付對象="A"  //政府 
            //    GRIDE.債權比例 =100
            //else
            //    GRIDE.債權比例 =0
            if (((DropDownList)e.Row.FindControl("ddl_PAY_TARGET_DESC")).SelectedValue == "A")
            {
                ((Label)e.Row.FindControl("lb_RATIO")).Text = "100";
            }
            else
            {
                ((Label)e.Row.FindControl("lb_RATIO")).Text = "0";
            }

        }
        #endregion
        #region 新增的檢核與設定
        if (e.Row.RowType.HasFlag(DataControlRowType.EmptyDataRow) || e.Row.RowType.HasFlag(DataControlRowType.Footer))
        {
            //下拉顯示  
            ((DropDownList)e.Row.FindControl("ddl_PAY_TARGET_DESC")).SelectedValue = "A";
            //EFFECT_SDT=系統日
            ((Label)e.Row.FindControl("lb_EFFECT_SDT")).Text = DateTime.Now.Date.ToString("yyyy/MM/dd");

            //  if GRIDE.支付對象="A"  //政府 
            //    GRIDE.債權比例 =100
            //else
            //    GRIDE.債權比例 =0
            if (((DropDownList)e.Row.FindControl("ddl_PAY_TARGET_DESC")).SelectedValue == "A")
            {
                ((Label)e.Row.FindControl("lb_RATIO")).Text = "100";
            }
            else
            {
                ((Label)e.Row.FindControl("lb_RATIO")).Text = "0";
            }

            //if 明細GRIDE.是否生效 ='Y'
            //   明細GRIDE.生效日期迄=''
            //else 
            //   明細GRIDE.生效日期迄=系統日期
            //end
            if (((DropDownList)e.Row.FindControl("ddl_IS_VAILD")).SelectedValue == "Y")
            {
                ((Label)e.Row.FindControl("lb_EFFECT_EDT")).Text = "";
            }
            else
            {
                ((Label)e.Row.FindControl("lb_EFFECT_EDT")).Text = DateTime.Now.Date.ToString("yyyy/MM/dd");
            }
        }
        if (e.Row.RowType == DataControlRowType.Header)
            e.Row.CssClass = "header";

        //Add CSS class on normal row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  e.Row.RowState == DataControlRowState.Normal)
            e.Row.CssClass = "normal";
        //Add CSS class on alternate row.
        if (e.Row.RowType == DataControlRowType.DataRow &&
                  (e.Row.RowState == DataControlRowState.Alternate ||
                   e.Row.RowState == DataControlRowState.Selected))
            e.Row.CssClass = "alternate";

        foreach (TableCell tc in e.Row.Cells)
        {
            tc.Attributes["style"] = "border-style:solid;border-width:1px; border-color:#FFFFFF";


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
        #endregion
    }
    protected void gv_result_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            #region 明細
            if (e.CommandName == "ToDtl")
            {
                ViewState["SetPerRow"] = true;
                ViewState["SortExpression2"] = null;
                ViewState["SortDirection2"] = null;
                int index = Convert.ToInt32(e.CommandArgument);
                Label EMP_ID = (Label)gv_result.Rows[index].FindControl("lb_EMP_ID");
                Label EMP_NAME = (Label)gv_result.Rows[index].FindControl("lb_EMP_NAME");
                HID_EMP_ID.Value = EMP_ID.Text;
                HID_EMP_NAME.Value = EMP_NAME.Text;

                Session["SF0100_emp_id_dtl"] = HID_EMP_ID.Value;
                Session["SF0100_checkIndex"] = index.ToString();
                Session["SF0100_sender"] = sender;
                Session["SF0100_e"] = e;

                if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                    GetGridView2("CHG_STATUS", 0, Convert.ToInt32(ViewState["PerPageRow2"]));
                else
                    GetGridView2("CHG_STATUS", 0, 10);

                gv_result2.EditIndex = -1;
                gv_result2.ShowFooter = false;

                if (gv_result2.Rows.Count > 0)
                {
                    WFB2SF0101Add.Visible = true;
                    WFB2SF0101Edit.Visible = true;
                    WFB2SF0102Edit.Visible = true;
                    WFB2SF0101Delete.Visible = true;
                }
                else
                {
                    WFB2SF0101Add.Visible = true;
                    WFB2SF0101Edit.Visible = false;
                    WFB2SF0102Edit.Visible = false;
                    WFB2SF0101Delete.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('查無資料!');", true);
                }
            }
            #endregion

            #region 資料確認
            if (e.CommandName == "ToDataCheck")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                Label EMP = (Label)gv_result.Rows[index].FindControl("lb_EMP_ID");
                Label DOC_NO = (Label)gv_result.Rows[index].FindControl("lb_DOC_NO");
                Label SURE_YN = (Label)gv_result.Rows[index].FindControl("lb_SURE_YN");
                HID_EMP_ID.Value = EMP.Text;
                HID_DOC_NO.Value = DOC_NO.Text;
                HID_SURE_YN.Value = SURE_YN.Text;
                string msg = Resources.Resource.wfb2sf_DataCheck_AlreadyCheckMessage;   //資料已確認,不須重複確認
                string msg2 = Resources.Resource.wfb2sf_DataCheck_RequiredEMessage;  //支付對象為本人的資料,只能有一筆
                string msg3 = Resources.Resource.wfb2sf_DataCheck_OnlyOneMessage;  //分配對象必須建立一筆支付對象為本人的資料

                if (HID_SURE_YN.Value == "Y")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('" + msg + "');", true);
                    gv_result2.Visible = false;
                    WFB2SF0101Add.Visible = false;
                    WFB2SF0101Edit.Visible = false;
                    WFB2SF0102Edit.Visible = false;
                    WFB2SF0101Delete.Visible = false;
                }
                else
                {
                    CFB2SF0100DAO fb2sf = new CFB2SF0100DAO();
                    fb2sf.EMP_ID = HID_EMP_ID.Value;
                    fb2sf.DOC_NO = HID_DOC_NO.Value;
                    int count = fb2sf.Get_TB_S_M_ARREARS_TARGET_Count();
                    if (count == 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('" + msg2 + "');", true);
                        gv_result2.Visible = false;
                        WFB2SF0101Add.Visible = false;
                        WFB2SF0101Edit.Visible = false;
                        WFB2SF0102Edit.Visible = false;
                        WFB2SF0101Delete.Visible = false;
                    }
                    else if (count > 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('" + msg3 + "');", true);
                        gv_result2.Visible = false;
                        WFB2SF0101Add.Visible = false;
                        WFB2SF0101Edit.Visible = false;
                        WFB2SF0102Edit.Visible = false;
                        WFB2SF0101Delete.Visible = false;
                    }
                    else
                    {
                        //20160907 檢核 法扣分配對象檔若 除對象為本人以外的'是否生效'如果都是N，且對象為本人的'是否生效'為Y時，顯示錯誤訊息:此案號已無債權人，請將支付對象為本人的是否生效改為N
                        bool b = service.checkData(fb2sf);
                        string msg4 = "此案號已無債權人，請將支付對象為本人的是否生效改為N";
                        if (!b )
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "empty", "alert('" + msg4 + "');", true);
                            gv_result2.Visible = false;
                            WFB2SF0101Add.Visible = false;
                            WFB2SF0101Edit.Visible = false;
                            WFB2SF0102Edit.Visible = false;
                            WFB2SF0101Delete.Visible = false;
                        }
                        else
                        {
                            string result = service.Update_TB_S_M_ARREARS_COURT_H(fb2sf, HID_EMP_ID.Value, HID_DOC_NO.Value);
                            if (result != "0")
                            {
                                result = result.Replace("\r\n", "");
                                result = result.Replace("'", "");
                                showMessage("DataCheckFailMessage", result);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "init", "iniForm();", true);
                                return;
                            }
                            else
                            {
                                showMessage("DataCheckSuccessMessage");   //資料確認作業完成
                                gv_result.DataSourceID = "ods1";
                                gv_result.DataKeyNames = new string[] { "qdatakey" };
                                gv_result2.DataSourceID = "ods2";
                                gv_result2.DataKeyNames = new string[] { "qdatakey2" };
                            }
                        }                        
                    }
                }
            }
            #endregion

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void gv_result_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result.PageCount == 1)
            {
                lb_TotalCount.Text = "頁數：1   總筆數：" + ViewState["TotalCount1"].ToString();
                //if (HID_PageRow.Value != "")
                //    ddlPerPageRow.SelectedValue = HID_PageRow.Value;
                if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                    ddlPerPageRow.SelectedValue = ViewState["PerPageRow"].ToString();
                OnePage.Visible = true;
            }
            else
                OnePage.Visible = false;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }
    protected void gv_result2_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gv_result2.PageCount == 1)
            {
                lb_TotalCount2.Text = "頁數：1   總筆數：" + ViewState["TotalCount2"].ToString();
                //if (HID_PageRow2.Value != "")
                //    ddlPerPageRow2.SelectedValue = HID_PageRow2.Value;
                if (ViewState["PerPageRow2"] != null && ViewState["PerPageRow2"].ToString() != "")
                    ddlPerPageRow2.SelectedValue = ViewState["PerPageRow2"].ToString();
                OnePage2.Visible = true;
            }
            else
                OnePage2.Visible = false;

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }

    }

    protected void ddl_PAY_TARGET_DESC_SelectedIndexChanged(object sender, EventArgs e)
    {
        gv_result2.PagerSettings.Visible = false;
        DropDownList ddl = sender as DropDownList;
        GridViewRow row = ddl.NamingContainer as GridViewRow; //取得是哪一列的DropDownList
        int rowIndex = row.RowIndex;
        HID_PAY_TARGET_DESC.Value = ddl.SelectedValue;
        #region 新增empty
        if (gv_result2.Rows.Count == 0)
        {
            //  if GRIDE.支付對象="A"  //政府 
            //    GRIDE.債權比例 =100
            //else
            //    GRIDE.債權比例 =0
            if (ddl.SelectedValue == "A")
            {
                ((Label)gv_result2.Controls[0].Controls[0].FindControl("lb_RATIO")).Text = "100";
            }
            else
            {
                ((Label)gv_result2.Controls[0].Controls[0].FindControl("lb_RATIO")).Text = "0";
            }

            //if 明細grid.支付對象='E' then CREDITOR=預設主GRID.姓名,且不能修改 END
            if (ddl.SelectedValue == "E")
            {
                ((TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_CREDITOR")).Text = HID_EMP_NAME.Value;
                ((TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_CREDITOR")).Enabled = false;
                //if 明細grid.支付對象='E' then VENDOR_ID=預設主GRID.工號,且不能修改 END
                ((TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_VENDOR_ID")).Text = HID_EMP_ID.Value;
                ((TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_VENDOR_ID")).Enabled = false;
            }
            else
            {
                ((TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_CREDITOR")).Text = "";
                ((TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_CREDITOR")).Enabled = true;
                //if 明細grid.支付對象='E' then VENDOR_ID=預設主GRID.工號,且不能修改 END
                ((TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_VENDOR_ID")).Text = "";
                ((TextBox)gv_result2.Controls[0].Controls[0].FindControl("txt_NEW_VENDOR_ID")).Enabled = true;
            }
        }
        #endregion
        #region 新增footer
        if (rowIndex == -1 && gv_result2.Rows.Count > 0)
        {
            //  if GRIDE.支付對象="A"  //政府 
            //    GRIDE.債權比例 =100
            //else
            //    GRIDE.債權比例 =0
            if (ddl.SelectedValue == "A")
            {
                ((Label)gv_result2.FooterRow.FindControl("lb_RATIO")).Text = "100";
            }
            else
            {
                ((Label)gv_result2.FooterRow.FindControl("lb_RATIO")).Text = "0";
            }

            //if 明細grid.支付對象='E' then CREDITOR=預設主GRID.姓名,且不能修改 END
            if (ddl.SelectedValue == "E")
            {
                ((TextBox)gv_result2.FooterRow.FindControl("txt_NEW_CREDITOR")).Text = HID_EMP_NAME.Value;
                ((TextBox)gv_result2.FooterRow.FindControl("txt_NEW_CREDITOR")).Enabled = false;
                //if 明細grid.支付對象='E' then VENDOR_ID=預設主GRID.工號,且不能修改 END
                ((TextBox)gv_result2.FooterRow.FindControl("txt_NEW_VENDOR_ID")).Text = HID_EMP_ID.Value;
                ((TextBox)gv_result2.FooterRow.FindControl("txt_NEW_VENDOR_ID")).Enabled = false;
            }
            else
            {
                ((TextBox)gv_result2.FooterRow.FindControl("txt_NEW_CREDITOR")).Text = "";
                ((TextBox)gv_result2.FooterRow.FindControl("txt_NEW_CREDITOR")).Enabled = true;
                //if 明細grid.支付對象='E' then VENDOR_ID=預設主GRID.工號,且不能修改 END
                ((TextBox)gv_result2.FooterRow.FindControl("txt_NEW_VENDOR_ID")).Text = "";
                ((TextBox)gv_result2.FooterRow.FindControl("txt_NEW_VENDOR_ID")).Enabled = true;
            }
        }
        #endregion
        #region 修改
        if (rowIndex != -1)
        {
            //  if GRIDE.支付對象="A"  //政府 
            //    GRIDE.債權比例 =100
            //else
            //    GRIDE.債權比例 =0
            if (ddl.SelectedValue == "A")
            {
                ((Label)gv_result2.Rows[rowIndex].FindControl("lb_RATIO")).Text = "100";
            }
            else
            {
                ((Label)gv_result2.Rows[rowIndex].FindControl("lb_RATIO")).Text = "0";
            }
            //if 明細grid.支付對象='E' then CREDITOR=預設主GRID.姓名,且不能修改 END
            if (ddl.SelectedValue == "E")
            {
                ((TextBox)gv_result2.Rows[rowIndex].FindControl("txt_EDIT_CREDITOR")).Text = HID_EMP_NAME.Value;
                ((TextBox)gv_result2.Rows[rowIndex].FindControl("txt_EDIT_CREDITOR")).Enabled = false;
                //if 明細grid.支付對象='E' then VENDOR_ID=預設主GRID.工號,且不能修改 END
                ((Label)gv_result2.Rows[rowIndex].FindControl("txt_EDIT_VENDOR_ID")).Text = HID_EMP_ID.Value;
                ((Label)gv_result2.Rows[rowIndex].FindControl("txt_EDIT_VENDOR_ID")).Enabled = false;
            }
            else
            {
                ((TextBox)gv_result2.Rows[rowIndex].FindControl("txt_EDIT_CREDITOR")).Text = "";
                ((TextBox)gv_result2.Rows[rowIndex].FindControl("txt_EDIT_CREDITOR")).Enabled = true;
                ((Label)gv_result2.Rows[rowIndex].FindControl("txt_EDIT_VENDOR_ID")).Text = "";
                ((Label)gv_result2.Rows[rowIndex].FindControl("txt_EDIT_VENDOR_ID")).Enabled = true;
            }
        }
        #endregion


    }
    protected void ddl_IS_VAILD_SelectedIndexChanged(object sender, EventArgs e)
    {
        gv_result2.PagerSettings.Visible = false;
        DropDownList ddl = sender as DropDownList;
        GridViewRow row = ddl.NamingContainer as GridViewRow; //取得是哪一列的DropDownList
        int rowIndex = row.RowIndex;
        HID_IS_VAILD.Value = ddl.SelectedValue;
        #region 新增empty
        if (gv_result2.Rows.Count == 0)
        {
            //if 明細GRIDE.是否生效 ='Y'
            //   明細GRIDE.生效日期迄=''
            //else 
            //   明細GRIDE.生效日期迄=系統日期
            //end
            if (((DropDownList)gv_result2.Controls[0].Controls[0].FindControl("ddl_IS_VAILD")).SelectedValue == "Y")
            {
                ((Label)gv_result2.Controls[0].Controls[0].FindControl("lb_EFFECT_EDT")).Text = "";
            }
            else
            {
                ((Label)gv_result2.Controls[0].Controls[0].FindControl("lb_EFFECT_EDT")).Text = DateTime.Now.Date.ToString("yyyy/MM/dd");
            }
        }
        #endregion
        #region 新增footer
        if (rowIndex == -1 && gv_result2.Rows.Count > 0)
        {
            //if 明細GRIDE.是否生效 ='Y'
            //   明細GRIDE.生效日期迄=''
            //else 
            //   明細GRIDE.生效日期迄=系統日期
            //end
            if (((DropDownList)gv_result2.FooterRow.FindControl("ddl_IS_VAILD")).SelectedValue == "Y")
            {
                ((Label)gv_result2.FooterRow.FindControl("lb_EFFECT_EDT")).Text = "";
            }
            else
            {
                ((Label)gv_result2.FooterRow.FindControl("lb_EFFECT_EDT")).Text = DateTime.Now.Date.ToString("yyyy/MM/dd");
            }
        }
        #endregion
        #region 修改
        if (rowIndex != -1)
        {
            //if 明細GRIDE.是否生效 ='Y'
            //   明細GRIDE.生效日期迄=''
            //else 
            //   明細GRIDE.生效日期迄=系統日期
            //end
            if (((DropDownList)gv_result2.Rows[rowIndex].FindControl("ddl_IS_VAILD")).SelectedValue == "Y")
            {
                ((Label)gv_result2.Rows[rowIndex].FindControl("lb_EFFECT_EDT")).Text = "";
            }
            else
            {
                ((Label)gv_result2.Rows[rowIndex].FindControl("lb_EFFECT_EDT")).Text = DateTime.Now.Date.ToString("yyyy/MM/dd");
            }
        }
        #endregion
        //修改

        //新增


    }
    protected void WFB2SF0102Edit_Click(object sender, EventArgs e)
    {
        try
        {
            keepConditions(true);
            int EditIndex = 0;
            //檢查勾選項目
            List<int> editindex = new List<int>();
            for (int i = 0; i < this.gv_result2.Rows.Count; i++)
            {
                if (((CheckBox)gv_result2.Rows[i].FindControl("cb_check_gv2")).Checked)
                {
                    editindex.Add(i);
                }
            }
            if (editindex.Count() == 1)
            {
                EditIndex = editindex[0];
            }
            else
            {
                return;
            }
            Label lb_DOC_NO = (Label)gv_result2.Rows[EditIndex].FindControl("lb_DOC_NO");
            Label ddl_PAY_TARGET_DESC = (Label)gv_result2.Rows[EditIndex].FindControl("lb_PAY_TARGET_DESC");
            Label txt_EDIT_CREDITOR = (Label)gv_result2.Rows[EditIndex].FindControl("lb_CREDITOR");
            Label txt_EDIT_VENDOR_ID = (Label)gv_result2.Rows[EditIndex].FindControl("lb_VENDOR_ID");
            Label txt_EDIT_MEMO = (Label)gv_result2.Rows[EditIndex].FindControl("lb_MEMO");
            Label txt_EDIT_MEMODESC = (Label)gv_result2.Rows[EditIndex].FindControl("lb_MEMODESC");
            Label lb_EFFECT_EDT = (Label)gv_result2.Rows[EditIndex].FindControl("lb_EFFECT_EDT");
            Label ddl_IS_VAILD = (Label)gv_result2.Rows[EditIndex].FindControl("lb_IS_VAILD");

            string data_key = gv_result2.DataKeys[EditIndex].Value.ToString();
            //string EMP_ID = HID_EMP_ID.Value;
            string DOC_NO = lb_DOC_NO.Text;
            string PAY_TARGET = ddl_PAY_TARGET_DESC.Text.Substring(0, 1);
            string PAY_TARGET_DESC = ddl_PAY_TARGET_DESC.Text;
            string CREDITOR = txt_EDIT_CREDITOR.Text;
            string VENDOR_ID = txt_EDIT_VENDOR_ID.Text;
            string EFFECT_EDT = lb_EFFECT_EDT.Text;
            string IS_VAILD = ddl_IS_VAILD.Text;
            string MEMO = txt_EDIT_MEMO.Text;
            string MEMODESC = txt_EDIT_MEMODESC.Text;
            Response.Redirect("WFB2SF0100_Edit.aspx?data_key=" + data_key + "&DOC_NO=" + DOC_NO + "&PAY_TARGET=" + PAY_TARGET + "&CREDITOR=" + CREDITOR
                + "&VENDOR_ID=" + VENDOR_ID + "&EFFECT_EDT=" + EFFECT_EDT + "&IS_VAILD=" + IS_VAILD + "&MEMO=" + MEMO + "&MEMODESC=" + MEMODESC + "&PAY_TARGET_DESC=" + PAY_TARGET_DESC);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }

    #region 查詢條件保留
    protected void keepConditions(bool clear)
    {
        if (clear)
        {
            string A = Session["SF0100_emp_id_dtl"].ToString();
            string B = Session["SF0100_checkIndex"].ToString();

            Session["SF0100_txt_EMP_ID"] = txt_EMP_ID.Text;
            Session["SF0100_txt_EMP_NAME"] = txt_EMP_NAME.Text;
            Session["SF0100_txt_LEAVE_DT_S"] = UCDateTimeRange.StartDateText;
            Session["SF0100_txt_LEAVE_DT_E"] = UCDateTimeRange.EndDateText;
            Session["SF0100_txt_DOC_NO"] = txt_DOC_NO.Text;

            Session["SF0100_pageIndex"] = ViewState["NewPageIndex"].ToString();

            if (ViewState["PerPageRow"] != null && ViewState["PerPageRow"].ToString() != "")
                Session["SF0100_pageRow"] = ViewState["PerPageRow"].ToString();
            else
                Session["SF0100_pageRow"] = "10";
            //Session["SF0100_Is_Search"] = "Y";
        }
        else
        {
            //Session["SF0100_txt_EMP_ID"] = null;
            //Session["SF0100_txt_EMP_NAME"] = null;
            //Session["SF0100_txt_LEAVE_DT_S"] = null;
            //Session["SF0100_txt_LEAVE_DT_E"] = null;
            //Session["SF0100_txt_DOC_NO"] = null;
            Session["SF0100_Is_Search"] = "N";
            //明細
            //Session["SF0100_IS_DTL"] = "N";
            //Session["SF0100_INDEX"] = null;
            //Session["SF0100_DTL_EMP_ID"] = null;
            //Session["SF0100_DTL_EMP_NAME"] = null;
        }
    }

    protected void realeaseConditions()
    {
        try
        {
            if (Session["SF0100_Is_Search"] == "Y")
            {
                ViewState["Queryble"] = true;
                txt_EMP_ID.Text = Session["SF0100_txt_EMP_ID"].ToString();
                txt_EMP_NAME.Text = Session["SF0100_txt_EMP_NAME"].ToString();
                UCDateTimeRange.StartDateText = Session["SF0100_txt_LEAVE_DT_S"].ToString();
                UCDateTimeRange.EndDateText = Session["SF0100_txt_LEAVE_DT_E"].ToString();
                txt_DOC_NO.Text = Session["SF0100_txt_DOC_NO"].ToString();
                ViewState["PerPageRow"] = Session["SF0100_ddlPerPageRow1"].ToString();
                ViewState["PerPageRow2"] = Session["SF0100_ddlPerPageRow2"].ToString();
                WFB2SF0100Search_Click(null, null);
                gv_result_RowCommand((object)Session["SF0100_sender"], (GridViewCommandEventArgs)Session["SF0100_e"]);

                int index = Convert.ToInt32(Session["SF0100_checkIndex"].ToString());
                HID_EMP_ID.Value = Session["SF0100_emp_id_dtl"].ToString();

                keepConditions(false);
            }
        }
        catch { }
    }

    #endregion

}