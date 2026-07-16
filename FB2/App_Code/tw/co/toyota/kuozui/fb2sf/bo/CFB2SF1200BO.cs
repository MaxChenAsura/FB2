using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SF1200BO 的摘要描述
/// </summary>
public class CFB2SF1200BO : BaseService
{
    public CFB2SF1200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //法扣金額分配
    public string Execute(CFB2SF1200DAO fb2sf, string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            BeginTransaction();
            int cnt = fb2sf.Check_TB_S_M_ARREARS_COURT_D(SALARY_DT, SALARY_TYPE);
            int cnt2 = fb2sf.Check_TB_S_M_ALLOCATION_D(SALARY_DT, SALARY_TYPE);
            if (cnt == 0)
                return Resources.Resource.wfb2sf_execute_message;   // "尚未執行WFB2SF110法扣金額轉入功能。"
            if (cnt2 > 0)
                return Resources.Resource.wfb2sf_execute_message2; //"法扣金額分配已有轉傳票資料,不允重新執行法扣金額分配功能。"

            string vnowemp_id = "";
            string vnowPAY_KIND = "";
            decimal vCurrentRemaining = 0;  //v目前剩餘法扣代扣額
            decimal vCurrentAmount = 0; //v目前法扣分配金額
            decimal vThisAmount = 0;    //v本次法扣金額(剩下還給本人的金額)

            string EMP_ID = "";
            string PAY_KIND = "";
            string salary_dt = "";
            string salary_type = "";
            string DEBIT_AMT = "";
            string PAY_TARGET = "";
            decimal AMOUNT = 0;
            decimal RAMOUNT = 0;
            decimal RATIO = 0;
            string DOC_NO = "";
            string SEQ = "";

            DataTable dtl_dt = fb2sf.Get_del_data(SALARY_DT, SALARY_TYPE);    //刪除資料(避免新增時重覆)
            if (dtl_dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dtl_dt.Rows)
                {
                    salary_dt = Convert.ToDateTime(dr["SALARY_DT"]).ToString("yyyyMMdd");
                    salary_type = dr["SALARY_TYPE"].ToString();
                    PAY_KIND = dr["PAY_KIND"].ToString();
                    EMP_ID = dr["EMP_ID"].ToString();
                    DOC_NO = dr["DOC_NO"].ToString();
                    SEQ = dr["SEQ"].ToString();
                    fb2sf.Del_del_data(salary_dt, salary_type, PAY_KIND, EMP_ID, DOC_NO, SEQ);
                }
            }
            DataTable dt = fb2sf.Get_TB_S_M_ARREARS_COURT_D(SALARY_DT, SALARY_TYPE);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    EMP_ID = dr["EMP_ID"].ToString();
                    PAY_KIND = dr["PAY_KIND"].ToString();
                    salary_dt = Convert.ToDateTime(dr["SALARY_DT"]).ToString("yyyyMMdd");
                    salary_type = dr["SALARY_TYPE"].ToString();
                    DEBIT_AMT = dr["DEBIT_AMT"].ToString();
                    PAY_TARGET = dr["PAY_TARGET"].ToString();
                    AMOUNT = Convert.ToDecimal(dr["AMOUNT"]);
                    RAMOUNT = Convert.ToDecimal(dr["RAMOUNT"]);
                    RATIO = Convert.ToDecimal(dr["RATIO"]);
                    DOC_NO = dr["DOC_NO"].ToString();
                    SEQ = dr["SEQ"].ToString();
                    if (vnowemp_id != EMP_ID || vnowPAY_KIND != PAY_KIND)
                    {
                        if (vnowemp_id != "" && vnowPAY_KIND != "")
                        {
                            fb2sf.SALARY_DT = salary_dt;
                            fb2sf.SALARY_TYPE = salary_type;
                            fb2sf.vnowPAY_KIND = vnowPAY_KIND;
                            fb2sf.vnowemp_id = vnowemp_id;
                            fb2sf.vCurrentRemaining = vCurrentRemaining;
                            fb2sf.AMOUNT = AMOUNT;
                            fb2sf.Update_SURE_YN();
                            if (vThisAmount > 0)    //將多扣金額,系統自動分配給本人
                            {
                                DataTable dt2 = fb2sf.Get_TB_S_M_ARREARS_TARGET(vnowemp_id);
                                if (dt2.Rows.Count > 0)
                                {
                                    foreach (DataRow dr2 in dt2.Rows)
                                    {
                                        fb2sf.DOC_NO = dr2["DOC_NO"].ToString();
                                        fb2sf.SEQ = dr2["SEQ"].ToString();
                                        fb2sf.Add_TB_S_M_ALLOCATION_D(vThisAmount); 
                                    }
                                }
                            }
                        }
                        vCurrentRemaining = Convert.ToDecimal(DEBIT_AMT);
                        vnowemp_id = EMP_ID;
                        vnowPAY_KIND = PAY_KIND;
                        vThisAmount = vCurrentRemaining;
                    }
                    if (PAY_TARGET == "A")  //政府
                    {
                        vCurrentAmount = Math.Min(vCurrentRemaining, RAMOUNT);
                        vCurrentRemaining = vCurrentRemaining - vCurrentAmount;
                        vThisAmount = vCurrentRemaining;
                    }

                    else  //非政府
                    {
                        vCurrentAmount = Math.Round(vCurrentRemaining * Convert.ToDecimal(RATIO / 100), 0, MidpointRounding.AwayFromZero);
                        vThisAmount = vThisAmount - vCurrentAmount;
                        if (vThisAmount<0) {  //代表四捨五入,導致捨位誤差 
                            vCurrentAmount = vCurrentAmount+vThisAmount;
                            vThisAmount =0;
                        }
                        if (vThisAmount > 0 && vThisAmount < 5) //若每次
                        {  //代表四捨五入,導致捨位誤差 
                            vCurrentAmount = vCurrentAmount + vThisAmount;//分配都是捨去時,認定為捨位誤差,不須還給本人
                            vThisAmount = 0;
                        }
                    }
                    fb2sf.Add_TB_S_M_ALLOCATION_D2(salary_dt, salary_type, PAY_KIND, EMP_ID, DOC_NO, SEQ, vCurrentAmount);
                }
                fb2sf.Update_SURE_YN2(SALARY_DT, SALARY_TYPE, vnowPAY_KIND, vnowemp_id);
                if (vThisAmount > 0)  //for結束後還剩一筆沒做,所以還要再打一次
                {
                    DataTable dt3 = fb2sf.Get_TB_S_M_ARREARS_TARGET(vnowemp_id);
                    if (dt3.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt3.Rows)
                        {
                            fb2sf.DOC_NO = dr["DOC_NO"].ToString();
                            fb2sf.SEQ = dr["SEQ"].ToString();
                            fb2sf.Add_TB_S_M_ALLOCATION_D3(SALARY_DT, SALARY_TYPE, vnowPAY_KIND, vnowemp_id, vThisAmount);
                        }
                    }
                }
            }

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //資料確認
    public string Update_TB_S_M_ARREARS_COURT_D(CFB2SF1200DAO fb2sf)
    {
        try
        {
            BeginTransaction();
            fb2sf.Update_TB_S_M_ARREARS_COURT_D();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    #region gv_result2新刪修
    public string Add_Dtl(CFB2SF1200DAO fb2sf, string DOC_NO, string SEQ)
    {
        try
        {
            //取得現有資料(檢查重複)
            DataTable tmp = fb2sf.getExistData_Dtl();
            BeginTransaction();
            if (tmp.Rows.Count > 0)
            {
                return "發文字號:" + DOC_NO + "之對象序號:" + SEQ + "重複,不允新增";
            }
            else
            {
                fb2sf.NEW_TB_S_M_ALLOCATION_D();
                fb2sf.NEW_Update_TB_S_M_ARREARS_COURT_D();
            }

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string Update_Dtl(CFB2SF1200DAO fb2sf, string DEPT_ACCT_ID, string HID_qdatakey3)
    {
        try
        {
            if (DEPT_ACCT_ID != "")
            {
                return Resources.Resource.wfb2sf_update_message; //"資料已拋轉AS400傳票,不允執行修改功能"
            }
            BeginTransaction();
            fb2sf.Edit_TB_S_M_ALLOCATION_D();
            fb2sf.Edit_TB_S_M_ARREARS_COURT_D(HID_qdatakey3);
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string Delete_Dtl(List<string> delitem_list, List<string> qdatakey3_item_list, List<string> dept_acct_id_item_list)
    {
        CFB2SF1200DAO fb2sf = new CFB2SF1200DAO();
        try
        {
            for (int i = 0; i < delitem_list.Count; i++)
            {
                string delitem = delitem_list[i];
                string qdatakey3_item = qdatakey3_item_list[i];
                string dept_acct_id_item = dept_acct_id_item_list[i];
                BeginTransaction();
                if (dept_acct_id_item != "")
                {
                    return Resources.Resource.wfb2sf_delete_message; //資料已拋轉AS400傳票,不允執行刪除功能
                }
                else
                {
                    fb2sf.Delete_TB_S_M_ALLOCATION_D(delitem);
                    fb2sf.Update_Del_TB_S_M_ARREARS_COURT_D(qdatakey3_item);
                }
            }
            Commit();
            return "0";
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
}