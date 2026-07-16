using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SC2300BO 的摘要描述
/// </summary>
public class CFB2SC2300BO : BaseService
{
    CFB2SC2300DAO dao = new CFB2SC2300DAO();
    public CFB2SC2300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string getSEQ_NO(string salary_id)
    {
        DataTable dtSEQ_NO = dao.getSEQ_NO(salary_id);
        if (Convert.ToString(dtSEQ_NO.Rows[0]["SEQ_NO"]) != "")
            return Convert.ToString(dtSEQ_NO.Rows[0]["SEQ_NO"]);
        else
            return "0";
    }
    #region "grid1"
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string salary_dt, string salary_type, string data_ym
                            , string company_cd, string pay_kind, string salary_name, string emp_id, string emp_name, string cfn_pay,string salary_id)
    {
        if (salary_type == "A")
        {
            return dao.getDataA(startRowIndex, maximumRows, sortExpression, salary_dt, salary_type, data_ym
                              , company_cd, pay_kind, salary_name, emp_id, emp_name, cfn_pay, salary_id);
        }
        else
        {
            return dao.getDataExceptA(startRowIndex, maximumRows, sortExpression, salary_dt, salary_type, data_ym
                            , company_cd, pay_kind, salary_name, emp_id, emp_name, cfn_pay, salary_id);
        }
    }
    public int getCount(int startRowIndex, int maximumRows, string salary_dt, string salary_type, string data_ym
                            , string company_cd, string pay_kind, string salary_name, string emp_id, string emp_name, string cfn_pay, string salary_id)
    {
        if (salary_type == "A")
        {
            return dao.getCountA(startRowIndex, maximumRows, salary_dt, salary_type, data_ym
                              , company_cd, pay_kind, salary_name, emp_id, emp_name, cfn_pay, salary_id);
        }
        else
        {
            return dao.getCountExceptA(startRowIndex, maximumRows, salary_dt, salary_type, data_ym
                            , company_cd, pay_kind, salary_name, emp_id, emp_name, cfn_pay, salary_id);
        }
    }
    public string deleteData(DataTable dtCheckData, string remark)
    {
        try
        {
            string msg = "";
            foreach (DataRow row in dtCheckData.Rows)
            {
                if (row["PAY_ID"].ToString() != "")
                    msg += "工號:" + row["EMP_ID"] + " 此筆資料已關帳,無法異動!\\n";
            }
            if (msg.Trim().Length == 0)
            {
                BeginTransaction();
                foreach (DataRow row in dtCheckData.Rows)
                {
                    DataTable dt = dao.getExcuteData1And2(row["DataKeys"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dao.SALARY_DT = Convert.ToDateTime(dt.Rows[i]["SALARY_DT"]).ToString("yyyy/MM/dd");
                            dao.SALARY_TYPE = Convert.ToString(dt.Rows[i]["SALARY_TYPE"]);
                            dao.DATA_YM = Convert.ToString(dt.Rows[i]["DATA_YM"]);
                            dao.EMP_ID = Convert.ToString(dt.Rows[i]["EMP_ID"]);
                            dao.SALARY_ID = Convert.ToString(dt.Rows[i]["SALARY_ID"]);
                            dao.PAY_KIND = Convert.ToString(dt.Rows[i]["PAY_KIND"]);
                            dao.SEQ_NO = Convert.ToString(dt.Rows[i]["SEQ_NO"]);
                            dao.REMARK = remark;
                            dao.CHG_AMT_B = Convert.ToString(dt.Rows[i]["CHG_AMT_B"]);
                            if (Convert.ToString(dt.Rows[i]["SEQ_NO"]) != "" && dt.Rows[i]["SEQ_NO"] != DBNull.Value)
                            {
                                msg = dao.delete_ofDelete_Data(dao);
                            }
                            dao.SEQ_NO = getSEQ_NO(dao.SALARY_ID);
                            msg = dao.delete_ofAdd_Data(dao);
                            
                        }
                    }
                }
                Commit();
            }
            return msg;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
    #endregion

    #region " Execute "
    public string WFB2SC2300Execute1(DataTable dtCheckData, string txt_remark)
    {
        try
        {
            string msg = "";
            foreach (DataRow row in dtCheckData.Rows)
            {
                if (dao.checkIsRepeat_CHH_STATUS(row["DataKeys"].ToString(),"C"))
                    msg += "工號:" + row["EMP_ID"] + " 此筆暫不發薪資料已存在,無法重複執行!\\n";
                if (row["PAY_ID"].ToString() != "")
                    msg += "工號:" + row["EMP_ID"] + " 此筆資料已關帳,無法異動!\\n";
                if (row["CFN_PAY"].ToString() != "Y")
                    msg += "工號:" + row["EMP_ID"] + " 此筆資料已暫不發薪!\\n";
            }
            if (msg.Trim().Length == 0)
            {
                BeginTransaction();
                foreach (DataRow row in dtCheckData.Rows)
                {
                    DataTable dt = dao.getExcuteData1And2(row["DataKeys"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dao.SALARY_DT = Convert.ToDateTime(dt.Rows[i]["SALARY_DT"]).ToString("yyyy/MM/dd");
                            dao.SALARY_TYPE = Convert.ToString(dt.Rows[i]["SALARY_TYPE"]);
                            dao.DATA_YM = Convert.ToString(dt.Rows[i]["DATA_YM"]);
                            dao.EMP_ID = Convert.ToString(dt.Rows[i]["EMP_ID"]);
                            dao.SALARY_ID = Convert.ToString(dt.Rows[i]["SALARY_ID"]);
                            dao.PAY_KIND = Convert.ToString(dt.Rows[i]["PAY_KIND"]);
                            dao.SEQ_NO = Convert.ToString(dt.Rows[i]["SEQ_NO"]);
                            dao.REMARK = Convert.ToString(dt.Rows[i]["REMARK"]);
                            dao.CHG_AMT_B = Convert.ToString(dt.Rows[i]["CHG_AMT_B"]);
                            dao.CHG_AMT_A = Convert.ToString(dt.Rows[i]["CHG_AMT_A"]);
                            if (Convert.ToString(dt.Rows[i]["SEQ_NO"]) != "" && dt.Rows[i]["SEQ_NO"] != DBNull.Value)
                                msg = dao.Execute1_SEQ_NO_isNotNULL_Data(dao, txt_remark);
                            else
                            {
                                dao.SEQ_NO = getSEQ_NO(dao.SALARY_ID);
                                msg = dao.Execute1_SEQ_NO_isNULL_Data(dao, txt_remark);
                            }
                        }
                    }
                }
                Commit();
            }
            return msg;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    public string WFB2SC2300Execute2(DataTable dtCheckData, string txt_remark)
    {
        try
        {
            string msg = "";
            foreach (DataRow row in dtCheckData.Rows)
            {
                if (dao.checkIsRepeat_CHH_STATUS(row["DataKeys"].ToString(), "R"))
                    msg += "工號:" + row["EMP_ID"] + " 此筆確認發薪資料已存在,無法重複執行!\\n";
                if (row["PAY_ID"].ToString() != "")
                    msg += "工號:" + row["EMP_ID"] + " 此筆資料已關帳,無法異動!\\n";
                if (row["CFN_PAY"].ToString() == "Y")
                    msg += "工號:" + row["EMP_ID"] + " 此筆資料已為確定發薪!\\n";
            }
            if (msg.Trim().Length == 0)
            {
                BeginTransaction();
                foreach (DataRow row in dtCheckData.Rows)
                {
                    DataTable dt = dao.getExcuteData1And2(row["DataKeys"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dao.SALARY_DT = Convert.ToDateTime(dt.Rows[i]["SALARY_DT"]).ToString("yyyy/MM/dd");
                            dao.SALARY_TYPE = Convert.ToString(dt.Rows[i]["SALARY_TYPE"]);
                            dao.DATA_YM = Convert.ToString(dt.Rows[i]["DATA_YM"]);
                            dao.EMP_ID = Convert.ToString(dt.Rows[i]["EMP_ID"]);
                            dao.SALARY_ID = Convert.ToString(dt.Rows[i]["SALARY_ID"]);
                            dao.PAY_KIND = Convert.ToString(dt.Rows[i]["PAY_KIND"]);
                            dao.SEQ_NO = Convert.ToString(dt.Rows[i]["SEQ_NO"]);
                            dao.REMARK = Convert.ToString(dt.Rows[i]["REMARK"]);
                            dao.CHG_AMT_B = Convert.ToString(dt.Rows[i]["CHG_AMT_B"]);
                            dao.CHG_AMT_A = Convert.ToString(dt.Rows[i]["CHG_AMT_A"]);
                            if (Convert.ToString(dt.Rows[i]["SEQ_NO"]) != "" && dt.Rows[i]["SEQ_NO"] != DBNull.Value)
                                msg = dao.Execute2_SEQ_NO_isNotNULL_Data(dao, txt_remark);
                            else
                            {
                                dao.SEQ_NO = getSEQ_NO(dao.SALARY_ID);
                                msg = dao.Execute2_SEQ_NO_isNULL_Data(dao, txt_remark);
                            }
                        }
                    }
                }
                Commit();
            }
            return msg;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
    public DataTable checkStatus(string emp_id)
    {
        try
        {
            CFB2SC2300DAO dao = new CFB2SC2300DAO();

            return dao.checkStatus(emp_id);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public string WFB2SC2300Execute3(DataTable dtCheckData, string txt_remark)
    {
        try
        {
            string msg = "";
            foreach (DataRow row in dtCheckData.Rows)
            {
                if (dao.checkIsRepeat_CHH_STATUS(row["DataKeys"].ToString(), "A"))
                    msg += "工號:" + row["EMP_ID"] + " 此筆轉積欠代墊資料已存在,無法重複執行!\\n";
                if (row["PAY_ID"].ToString() != "")
                    msg += "工號:" + row["EMP_ID"] + " 此筆資料已關帳,無法異動!\\n";
                if (row["LEAVE_DT"].ToString() != "")
                {
                    //在職狀態是否為  99.離職
                    DataTable STATUS_DT = dao.checkStatus(row["EMP_ID"].ToString());
                    if (STATUS_DT.Rows.Count > 0)
                    {
                        if (STATUS_DT.Rows[0]["EMP_STATUS"].ToString() == "99")
                        {
                            msg += "工號:" + row["EMP_ID"] + " 此員工已離職!\\n";
                        }                        
                    }                    
                }
                if (Convert.ToInt32(row["AMOUNT"]) >= 0)
                    msg += "工號:" + row["EMP_ID"] + " 此員工合計金額>0!\\n";
            }
            if (msg.Trim().Length == 0)
            {
                BeginTransaction();
                foreach (DataRow row in dtCheckData.Rows)
                {
                    DataTable dtExcute = dao.getExcuteData3(row["DataKeys"].ToString());
                    if (dtExcute.Rows.Count > 0)
                    {
                        int total_amount = 0;
                        foreach (DataRow rowExcute in dtExcute.Rows)
                        {
                            total_amount += Convert.ToInt32(rowExcute["AMT"]);
                        }
                        // 20150703 非國瑞時，直接取表格中的總金額
                        if (row["COMPANY_CD"].ToString() != "K")
                        {
                            total_amount = Convert.ToInt32(row["AMOUNT"].ToString());
                        }
                        dao.SALARY_DT = row["SALARY_DT"].ToString();
                        dao.DATA_YM = row["DATA_YM"].ToString();
                        dao.SALARY_TYPE = row["SALARY_TYPE"].ToString();
                        dao.EMP_ID = row["EMP_ID"].ToString();
                        dao.PAY_KIND = row["PAY_KIND"].ToString();
                        dao.CHG_AMT_A = Math.Abs(total_amount).ToString();
                        dao.REMARK = txt_remark;
                        dao.SEQ_NO = getSEQ_NO("1041");
                        msg = dao.Execute3_add_Data(dao);
                    }
                }
                Commit();
            }
            return msg;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    public string WFB2SC2300Execute4(DataTable dtCheckData, string txt_remark)
    {
        try
        {
            string msg = "";
            foreach (DataRow row in dtCheckData.Rows)
            {
                if (dao.checkIsRepeat_CHH_STATUS(row["DataKeys"].ToString(), "B"))
                    msg += "工號:" + row["EMP_ID"] + " 此筆離職轉所得資料已存在,無法重複執行!\\n";
                if (row["PAY_ID"].ToString() != "")
                    msg += "工號:" + row["EMP_ID"] + " 此筆資料已關帳,無法異動!\\n";
                DataTable dt_sec = dao.checkSecondSalary();//二次發薪的第一筆發薪日期
                if (dt_sec.Rows.Count > 0)
                {
                    if (dao.SALARY_DT == dt_sec.Rows[0]["SALARY_DT"].ToString())  //相等，表示此筆不是二次發薪
                    {
                        if (row["LEAVE_DT"].ToString() == "" || row["LEAVE_DT"].ToString() == null)
                        {
                            msg += "工號:" + row["EMP_ID"] + " 此員工未離職!無法執行此作業\\n";
                        }
                    }
                }
                
                if (Convert.ToInt32(row["AMOUNT"]) >= 0)
                    msg += "工號:" + row["EMP_ID"] + " 此員工合計金額>0!\\n";
            }
            if (msg.Trim().Length == 0)
            {
                BeginTransaction();
                foreach (DataRow row in dtCheckData.Rows)
                {
                    DataTable dtExcute = dao.getExcuteData4(row["DataKeys"].ToString());
                    if (dtExcute.Rows.Count > 0)
                    {
                        int total_amount = 0;
                        foreach (DataRow rowExcute in dtExcute.Rows)
                        {
                            total_amount += Convert.ToInt32(rowExcute["AMT"]);
                        }
                        // 20150703 非國瑞時，直接取表格中的總金額
                        if (row["COMPANY_CD"].ToString() != "K")
                        {
                            total_amount = Convert.ToInt32(row["AMOUNT"].ToString());
                        }
                        dao.SALARY_DT = row["SALARY_DT"].ToString();
                        dao.DATA_YM = row["DATA_YM"].ToString();
                        dao.SALARY_TYPE = row["SALARY_TYPE"].ToString();
                        dao.EMP_ID = row["EMP_ID"].ToString();
                        dao.PAY_KIND = row["PAY_KIND"].ToString();
                        dao.CHG_AMT_A = Math.Abs(total_amount).ToString();    //絕對值
                        dao.REMARK = txt_remark;
                        dao.SEQ_NO = getSEQ_NO("2001");
                        msg = dao.Execute4_add_Data(dao);
                    }
                }
                Commit();
            }
            return msg;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
    #endregion

    #region "grid2"
    //grid2 刪除動作
    public string deleteDtlData(DataTable dtCheckData,string remark)
    {
        try
        {
            dao.REMARK = remark;
            string msg = string.Empty;
            BeginTransaction();
            foreach (DataRow row in dtCheckData.Rows)
            {
                if (row["PROCESS_STATUS"].ToString() == "Y")
                {
                    DataTable dtSEQ_NO = dao.getSEQ_NO2(row["DataKeys"].ToString());
                    if (Convert.ToString(dtSEQ_NO.Rows[0]["SEQ_NO"]) != "")
                        row["SEQ_NO"] = Convert.ToString(dtSEQ_NO.Rows[0]["SEQ_NO"]);
                    else
                        row["SEQ_NO"] = "0";

                    dao.SALARY_DT = row["SALARY_DT"].ToString();
                    dao.DATA_YM = row["DATA_YM"].ToString();
                    dao.SALARY_TYPE = row["SALARY_TYPE"].ToString();
                    dao.EMP_ID = row["EMP_ID"].ToString();
                    dao.SALARY_ID = row["SALARY_ID"].ToString();
                    dao.PAY_KIND = row["PAY_KIND"].ToString();
                    dao.SEQ_NO = row["SEQ_NO"].ToString();
                    dao.CHG_AMT_B = row["CHG_AMT_B"].ToString();
                    dao.deleteDtl_OFadd();
                }
                else
                    dao.deleteDtl_TB_S_S_SALARY_PAY_TMP(row["DataKeys"].ToString(), row["SEQ_NO"].ToString());
            }
            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
    //新增畫面 儲存動作
    public string addDtlData(CFB2SC2300DAO dao)
    {
        try
        {
            BeginTransaction();
            string dtldatakay = dao.SALARY_DT + dao.SALARY_TYPE + dao.EMP_ID + dao.SALARY_ID + dao.PAY_KIND;
            DataTable dtSEQ_NO = dao.getSEQ_NO2(dtldatakay);
            if (Convert.ToString(dtSEQ_NO.Rows[0]["SEQ_NO"]) != "")
                dao.SEQ_NO = Convert.ToString(dtSEQ_NO.Rows[0]["SEQ_NO"]);
            else
                dao.SEQ_NO = "0";
            dao.addDtlData();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //修改畫面 取得修改資料
    public DataTable getModInitialData(string dtldatakey, string salary_type, string process_status, string seq_no)
    {
        try
        {
            if (process_status == "Y")
            {
                if (salary_type == "A")
                    return dao.getModInitialData_PROCESS_STATUS_isY_A(dtldatakey);
                else
                    return dao.getModInitialData_PROCESS_STATUS_isY_notA(dtldatakey);
            }
            else
            {
                if (salary_type == "A")
                    return dao.getModInitialData_PROCESS_STATUS_isNotY_A(dtldatakey, seq_no);
                else
                    return dao.getModInitialData_PROCESS_STATUS_isNotY_notA(dtldatakey, seq_no);
            }
        }
        catch
        {
            throw;
        }
    }
    //修改畫面 儲存動作
    public string modDtlData(CFB2SC2300DAO dao, string process_status, string dtldatakey)
    {
        try
        {
            if (process_status == "Y")
            {
                BeginTransaction();
                DataTable dtSEQ_NO = dao.getSEQ_NO2(dtldatakey);
                if (Convert.ToString(dtSEQ_NO.Rows[0]["SEQ_NO"]) != "")
                    dao.SEQ_NO = Convert.ToString(dtSEQ_NO.Rows[0]["SEQ_NO"]);
                else
                    dao.SEQ_NO = "0";
                dao.modDtlData_add();
                Commit();
            }
            else
            {
                BeginTransaction();
                dao.modDtlData_update();
                Commit();
            }
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    #endregion
}