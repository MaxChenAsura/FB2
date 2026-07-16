using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Collections;

/// <summary>
/// CFB2HC0100BO 的摘要描述
/// </summary>
public class CFB2HC0100BO : BaseService
{
    public ArrayList HR_CHG_NO { get; set; }
    public string HR_CHG_NO_for_Update { get; set; }
    public string HR_CHG_CD { get; set; }
    public string EMP_ID { get; set; }
    public List<string> EMP_IDs { get; set; }
    public string START_DT { get; set; }
    public ArrayList CHG_SEQ { get; set; }
    public string INS_PLAN_PROC_DT { get; set; }
    public string PLAN_END_DT { get; set; }
    //public string END_HR_CHG_NO { get; set; }
    public string IS_END { get; set; }
    public string MAIN_HR_CHG_NO { get; set; }
    public List<string> MAIN_HR_CHG_NOs { get; set; }
    public string ICT_TYPE { get; set; }
    public string TRANSFER_NATION_CD { get; set; }
    public string TRANSFER_COMPANY_CD { get; set; }
    public string TRANSFER_DEPT { get; set; }
    public string IS_PAY_SUBSIST { get; set; }
    //ORI_WS_CD
    //ORI_COMPANY_CD
    //ORI_PLANT_CD
    //ORI_DEPT_NO
    //ORI_DEPT_NAME
    //ORI_DEPT_FULL_NAME
    //ORI_DIV_DEPT_FULL_NAME
    //ORI_DEPT_NAME_20
    //ORI_DEPT_NAME_30
    //ORI_DEPT_NAME_40
    //ORI_DEPT_NAME_50
    //ORI_DEPT_NAME_60
    //ORI_DEPT_NAME_70
    //ORI_EMP_CD
    //ORI_LEVEL_CD
    //ORI_GRADE_CD
    //ORI_PJOB_CD
    //ORI_PJOB_DESC
    //ORI_WORK_SHIFT_CD
    //ORI_WORK_CD
    public string HR_CHG_PROC_STATUS { get; set; }
    //public string HR_CHG_PROC_LOG { get; set; }
    //public string HR_CHG_PROC_DT { get; set; }
    public string INS_CHG_PROC_STATUS { get; set; }
    //public string INS_CHG_PROC_LOG { get; set; }
    //public string INS_CHG_PROC_DT { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    public DataTable gv_result { get; set; }
    public DataTable gv_result2 { get; set; }

    public CFB2HC0100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //找到異動生效日的最小日(為了人事履歷資料生成用)
    public string getMinSatrtDT(List<string> start_dts)
    {
        string result = "";
        string start_dt = "";
        for (int i = 0; i < start_dts.Count; i++)
        {
            start_dt = start_dts[i];
            if (i == 0)
            {
                result = start_dts[i];
            }
            //日期 前比(後) 小=>-1, 相同=0, 大=>1
            if (result.CompareTo(start_dt) > 0)
            {
                result = start_dt;
            }
        }
        return result;
    }





    //新增儲存
    public void WFB2HC0100_Add_Save()
    {
        try
        {
            BeginTransaction();
            CFB2HC0100DAO dao = new CFB2HC0100DAO();
            dao.HR_CHG_NO = HR_CHG_NO;
            dao.HR_CHG_CD = HR_CHG_CD;
            dao.EMP_ID = EMP_ID;
            dao.START_DT = START_DT;
            dao.CHG_SEQ = CHG_SEQ;
            dao.INS_PLAN_PROC_DT = INS_PLAN_PROC_DT;
            dao.PLAN_END_DT = PLAN_END_DT;
            dao.IS_END = IS_END;
            dao.MAIN_HR_CHG_NO = MAIN_HR_CHG_NO;
            dao.ICT_TYPE = ICT_TYPE;
            dao.TRANSFER_NATION_CD = TRANSFER_NATION_CD;
            dao.TRANSFER_COMPANY_CD = TRANSFER_COMPANY_CD;
            dao.TRANSFER_DEPT = TRANSFER_DEPT;
            dao.IS_PAY_SUBSIST = IS_PAY_SUBSIST;
            dao.HR_CHG_PROC_STATUS = HR_CHG_PROC_STATUS;
            dao.INS_CHG_PROC_STATUS = INS_CHG_PROC_STATUS;
            dao.gv_result = gv_result;
            dao.gv_result2 = gv_result2;
            dao.WFB2HC0100_Add_Save();
            Commit();
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    //修改儲存
    public void WFB2HC0100_Update_Save()
    {
        try
        {
            BeginTransaction();
            CFB2HC0100DAO dao = new CFB2HC0100DAO();
            dao.HR_CHG_NO = HR_CHG_NO;
            dao.HR_CHG_NO_for_Update = HR_CHG_NO_for_Update;
            dao.HR_CHG_CD = HR_CHG_CD;
            dao.EMP_ID = EMP_ID;
            dao.START_DT = START_DT;
            dao.CHG_SEQ = CHG_SEQ;
            dao.INS_PLAN_PROC_DT = INS_PLAN_PROC_DT;
            dao.PLAN_END_DT = PLAN_END_DT;
            //dao.END_HR_CHG_NO = null;
            dao.IS_END = IS_END;
            dao.MAIN_HR_CHG_NO = MAIN_HR_CHG_NO;
            dao.ICT_TYPE = ICT_TYPE;
            dao.TRANSFER_NATION_CD = TRANSFER_NATION_CD;
            dao.TRANSFER_COMPANY_CD = TRANSFER_COMPANY_CD;
            dao.TRANSFER_DEPT = TRANSFER_DEPT;
            dao.IS_PAY_SUBSIST = IS_PAY_SUBSIST;
            //ORI_WS_CD
            //ORI_COMPANY_CD
            //ORI_PLANT_CD
            //ORI_DEPT_NO
            //ORI_DEPT_NAME
            //ORI_DEPT_FULL_NAME
            //ORI_DIV_DEPT_FULL_NAME
            //ORI_DEPT_NAME_20
            //ORI_DEPT_NAME_30
            //ORI_DEPT_NAME_40
            //ORI_DEPT_NAME_50
            //ORI_DEPT_NAME_60
            //ORI_DEPT_NAME_70
            //ORI_EMP_CD
            //ORI_LEVEL_CD
            //ORI_GRADE_CD
            //ORI_PJOB_CD
            //ORI_PJOB_DESC
            //ORI_WORK_SHIFT_CD
            //ORI_WORK_CD
            dao.HR_CHG_PROC_STATUS = HR_CHG_PROC_STATUS;
            //dao.HR_CHG_PROC_LOG = null;
            //dao.HR_CHG_PROC_DT = null;
            dao.INS_CHG_PROC_STATUS = INS_CHG_PROC_STATUS;
            //bo.INS_CHG_PROC_LOG = null;
            //bo.INS_CHG_PROC_DT = null;
            //CREATED_BY
            //CREATED_DT
            //UPDATED_BY
            //UPDATED_DT
            //FUNC_ID
            dao.gv_result = gv_result;
            dao.gv_result2 = gv_result2;
            dao.WFB2HC0100_Update_Save();
            Commit();
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    //一括異動儲存
    public void WFB2HC0100_Add_batch_Save()
    {
        try
        {
            BeginTransaction();
            CFB2HC0100DAO dao = new CFB2HC0100DAO();
            dao.HR_CHG_NO = HR_CHG_NO;
            dao.HR_CHG_CD = HR_CHG_CD;
            dao.EMP_IDs = EMP_IDs;
            dao.START_DT = START_DT;
            //dao.CHG_SEQ = CHG_SEQ;
            dao.INS_PLAN_PROC_DT = INS_PLAN_PROC_DT;
            dao.PLAN_END_DT = PLAN_END_DT;
            //dao.END_HR_CHG_NO = null;
            dao.IS_END = IS_END;
            dao.MAIN_HR_CHG_NOs = MAIN_HR_CHG_NOs;
            //dao.ICT_TYPE = ICT_TYPE;
            //dao.TRANSFER_NATION_CD = TRANSFER_NATION_CD;
            //dao.TRANSFER_COMPANY_CD = TRANSFER_COMPANY_CD;
            //dao.TRANSFER_DEPT = TRANSFER_DEPT;
            //dao.IS_PAY_SUBSIST = IS_PAY_SUBSIST;
            //ORI_WS_CD
            //ORI_COMPANY_CD
            //ORI_PLANT_CD
            //ORI_DEPT_NO
            //ORI_DEPT_NAME
            //ORI_DEPT_FULL_NAME
            //ORI_DIV_DEPT_FULL_NAME
            //ORI_DEPT_NAME_20
            //ORI_DEPT_NAME_30
            //ORI_DEPT_NAME_40
            //ORI_DEPT_NAME_50
            //ORI_DEPT_NAME_60
            //ORI_DEPT_NAME_70
            //ORI_EMP_CD
            //ORI_LEVEL_CD
            //ORI_GRADE_CD
            //ORI_PJOB_CD
            //ORI_PJOB_DESC
            //ORI_WORK_SHIFT_CD
            //ORI_WORK_CD
            dao.HR_CHG_PROC_STATUS = HR_CHG_PROC_STATUS;
            //dao.HR_CHG_PROC_LOG = null;
            //dao.HR_CHG_PROC_DT = null;
            dao.INS_CHG_PROC_STATUS = INS_CHG_PROC_STATUS;
            //bo.INS_CHG_PROC_LOG = null;
            //bo.INS_CHG_PROC_DT = null;
            //CREATED_BY
            //CREATED_DT
            //UPDATED_BY
            //UPDATED_DT
            //FUNC_ID
            dao.gv_result = gv_result;
            //dao.gv_result2 = gv_result2;
            dao.WFB2HC0100_Add_batch_Save();
            Commit();
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    //刪除
    public bool Delete(ArrayList datas)
    {
        try
        {
            BeginTransaction();
            CFB2HC0100DAO dao = new CFB2HC0100DAO();
            dao.Delete(datas);
            Commit();

            return true;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    //取得員工姓名
    public ArrayList Qry_Get_EMP_NAME(string emp_id)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (emp_id.Length > 0)
            {
                string[] tmp = new string[0];
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                tmp = dao.Qry_Get_EMP_NAME(emp_id);
                rtnval.Add(new string[] { "", tmp[0] });
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得人事異動代碼說明
    public ArrayList Qry_Get_HR_CHG_DESC(string hr_chg_cd)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (hr_chg_cd.Length > 0)
            {
                string[] tmp = new string[0];
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                tmp = dao.Qry_Get_HR_CHG_DESC(hr_chg_cd);
                rtnval.Add(new string[] { "", tmp[0] });
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得員工姓名(人事異動對象的員工姓名)
    public ArrayList Get_EMP_NAME(string emp_id)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (emp_id.Length > 0)
            {
                string[] tmp = new string[6];
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                tmp = dao.Get_EMP_NAME(emp_id);
                if (tmp[0] != null && tmp[0] != "")
                {
                    rtnval.Add(new string[] { "", tmp[0], tmp[1], tmp[2], tmp[3], tmp[4], tmp[5] });
                }
                else
                {
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_emp_id_not_exist_or_emp_id_is_self });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //異動生效日不可以是已薪結日期
    public ArrayList Check_FN_S_SALARY_YM(string start_dt)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (IsDate(start_dt))
            {
                start_dt = start_dt.Replace("/", "").Substring(0, 6);
                string tmp = "";
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                tmp = dao.Get_FN_S_SALARY_YM();
                if (tmp != null && tmp != "")
                {
                    if (start_dt.CompareTo(tmp) <= 0)
                        rtnval.Add(new string[] { Resources.Resource.wfb2hc_START_DT_Less_SALARY_YM });
                    else
                        rtnval.Add(new string[] { "" });
                }
                else
                {
                    rtnval.Add(new string[] { "" });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }
    //確認該員工是否已離職
    public string checkIsLeave(string emp_id)
    {
        string rtnmessage = "";
        try
        {
            CFB2HC0100DAO dao = new CFB2HC0100DAO();
            DataTable dt = new DataTable();
            dt = dao.checkIsLeave(emp_id);
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                rtnmessage += emp_id + Resources.Resource.wfb2hc_EMP_IS_LEAVE;
            }
            return rtnmessage;
        }
        catch (Exception)
        {
            throw;
        }
    }
    //該人事異動代碼的保險處理區分是否為N
    public bool checkHasInsurance(string hr_chg_cd)
    {
        try
        {
            CFB2HC0100DAO dao = new CFB2HC0100DAO();
            DataTable dt = new DataTable();
            dt = dao.checkHasInsurance(hr_chg_cd);
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //人事異動主檔是否已有未生效的異動單且與保險處理相關時
    public string checkIsInsurance(string emp_id)
    {
        string rtnmessage = "";
        try
        {
            CFB2HC0100DAO dao = new CFB2HC0100DAO();
            DataTable dt = new DataTable();
            dt = dao.checkIsInsurance(emp_id);
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                rtnmessage += emp_id + Resources.Resource.wfb2hc_EMP_IS_INSURANCE;
            }
            return rtnmessage;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string CheckHR_CHG_CD(string hr_chg_cd)
    {
        try
        {
            string rtnval = "";
            CFB2HC0100DAO dao = new CFB2HC0100DAO();
            rtnval = dao.CheckHR_CHG_CD(hr_chg_cd);
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //3.讀取 人事異動主檔 H
    //取得:	H.人事異動代碼
    //條件:	H.工號 = 明細畫面.工號
    //且 H.人事異動生效日 = 明細畫面.異動生效日
    //且 H.人事異動代碼 = 明細畫面.人事異動代碼
    //若讀得到資料，顯示錯誤訊息"相同工號、人事異動代碼、異動生效日期 的資料已經存在，請確認"
    //若讀不到資料，繼續作業。
    public ArrayList Check_Same_Data1(string emp_id, string start_dt, string hr_chg_cd)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (emp_id.Length > 0 && IsDate(start_dt) && hr_chg_cd.Length > 0)
            {
                int tmp = 0;
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                tmp = dao.Check_Same_Data1(emp_id, start_dt, hr_chg_cd);
                if (tmp > 0)
                {
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_EMP_ID_and_HR_CHG_CD_and_START_DT_are_same_Message });
                }
                else
                {
                    rtnval.Add(new string[] { "" });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //4.讀取 人事異動主檔 H
    //取得:	H.人事異動代碼
    //條件:	H.工號 = 明細畫面.工號
    //且 H.人事異動生效日 = 明細畫面.異動生效日
    //若讀得到資料，可能是多筆，
    //讀取 人事異動代碼檔 G1
    //取得:	G1.人事異動代碼說明
    //條件:	G1.人事異動代碼 = H.人事異動代碼
    //顯示提醒訊息"相同異動生效日存在 XXXXX, XXXXX 的人事異動單，是否繼續輸入？"	<-XXXXX 為 G1.人事異動代碼說明
    //若選擇不繼續輸入，則游標停留在異動生效日欄位。
    //若讀不到資料，繼續作業。
    public ArrayList Check_Same_Data2(string emp_id, string start_dt)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (emp_id.Length > 0 && IsDate(start_dt))
            {
                string tmp = "";
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                tmp = dao.Check_Same_Data2(emp_id, start_dt);
                if (tmp != "")
                {
                    rtnval.Add(new string[] { String.Format(Resources.Resource.wfb2hc_EMP_ID_and_START_DT_are_same_Message, tmp) });
                }
                else
                {
                    rtnval.Add(new string[] { "" });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //<保險預計處理日>
    //1.若 G.保險提前生效(IS_INS_EARLIER)為'Y'，則 明細畫面.保險預計處理日 必須< 明細畫面.異動生效日，且必須>系統日，
    //  否則顯示錯誤訊息"保險預計處理日 必須＜異動生效日 且必須＞系統日"；
    //  若 G.保險提前生效(IS_INS_EARLIER)為'N'，則 明細畫面.保險預計處理日=明細畫面.異動生效日，且不可修改，將其DISABLED。
    public ArrayList Check_INS_PLAN_PROC_DT(string hr_chg_cd, string ins_plan_proc_dt, string start_dt)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (hr_chg_cd.Length > 0 && IsDate(start_dt))
            {
                string tmp = "";
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                tmp = dao.Get_IS_INS_EARLIER(hr_chg_cd);
                //1.若 G.保險提前生效(IS_INS_EARLIER)為'Y'
                if (tmp == "Y")
                {
                    //則 明細畫面.保險預計處理日 必須< 明細畫面.異動生效日，且必須>系統日，
                    if (ins_plan_proc_dt != "" && ins_plan_proc_dt.CompareTo(start_dt) < 0 && ins_plan_proc_dt.CompareTo(DateTime.Now.ToString("yyyy/MM/dd")) >= 0)
                        rtnval.Add(new string[] { "", tmp });
                    else
                    {
                        //  否則顯示錯誤訊息"保險預計處理日 必須＜異動生效日 且必須＞系統日"；
                        rtnval.Add(new string[] { Resources.Resource.wfb2hc_INS_PLAN_PROC_DT_Error_Message });
                    }
                }
                else
                {
                    rtnval.Add(new string[] { "", tmp });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string get_IS_LEAVE(string hr_chg_cd)
    {
        try
        {
            string result = "";
            CFB2HC0100DAO hc010DAO = new CFB2HC0100DAO();
            result = hc010DAO.get_IS_LEAVE(hr_chg_cd);
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //<狀態預計結束日>
    //1.若 G.是否暫時狀態(IS_TEMP)為'Y'，
    //  若 G.人事異動身份狀態(EMP_CHG_STATUS) 為'31'(期間工)，
    //  讀取 參數檔 A
    //  取得:參數值(CODE_VAL1)
    //  條件:子作業='HB' 且參數別='KZ_CONTRACT_MONTHS'
    //  預設 明細畫面.狀態預計結束日 = 明細畫面.異動生效日 的月份 + 參數值(CODE_VAL1) ，取該月月初日，再減1天，可修改，不可清空。

    //  若 G.人事異動身份狀態(EMP_CHG_STATUS) 為'32'(派遣)，
    //  讀取 參數檔 A
    //  取得:參數值(CODE_VAL1)
    //  條件:子作業='HB' 且參數別='OTH1_CONTRACT_MONTHS'
    //  預設 明細畫面.狀態預計結束日 = 明細畫面.異動生效日 的月份 + 參數值(CODE_VAL1) ，取該月月初日，再減1天，可修改，不可清空。    
    public ArrayList Get_PLAN_END_DT(string hr_chg_cd, string start_dt)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (hr_chg_cd.Length > 0 && IsDate(start_dt))
            {
                string[] tmp = new string[3];
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                tmp = dao.Get_PLAN_END_DT(hr_chg_cd, start_dt);
                rtnval.Add(new string[] { "", tmp[0], tmp[1], tmp[2] });
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //<狀態預計結束日>
    //  若 G.是否暫時狀態(IS_TEMP)為'Y'，則 明細畫面.狀態預計結束日必須輸入，且必須>明細畫面.異動生效日，否則顯示錯誤訊息"狀態預計結束日必須輸入，且必須＞異動生效日"；
    //  否則，明細畫面.狀態預計結束日不可輸入，將其DISABLED。
    public ArrayList Check_PLAN_END_DT(string hr_chg_cd, string plan_end_dt, string start_dt, bool check_PLAN_END_DT_require = false)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (hr_chg_cd.Length > 0 && IsDate(start_dt))
            {
                string[] tmp = new string[3];
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                tmp = dao.Get_PLAN_END_DT(hr_chg_cd, start_dt);
                if (tmp[0] == "Y")
                {
                    if (check_PLAN_END_DT_require && (plan_end_dt == "" || plan_end_dt.CompareTo(start_dt) <= 0))
                    {
                        rtnval.Add(new string[] { Resources.Resource.wfb2hc_PLAN_END_DT_require_Message });
                    }
                }
                else
                {
                    rtnval.Add(new string[] { "", tmp[0] });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //<狀態結束>
    //1.若 G.是否暫時狀態(IS_TEMP)為'E'，則 明細畫面.狀態結束必須勾選，否則顯示錯誤訊息"必須勾選狀態結束"。    
    public ArrayList Check_IS_END(string hr_chg_cd, bool is_end)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (hr_chg_cd.Length > 0)
            {
                string[] tmp = new string[1];
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                tmp = dao.Get_IS_TEMP(hr_chg_cd);
                if (tmp[0] == "E")
                {
                    if (!is_end)
                    {
                        rtnval.Add(new string[] { Resources.Resource.wfb2hc_Required_IS_END });
                    }
                }
                else
                {
                    rtnval.Add(new string[] { "" });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //<狀態結束>
    //2.若 明細畫面.人事異動代碼輸入為'C'開頭(離社的異動)的代碼，且未勾選結束狀態，
    //  讀取 人事異動主檔 H
    //  取得:H.人事異動編號
    //  條件:H.工號 = 明細畫面.工號
    //  且 H.人事異動生效日 < 明細畫面.異動生效日
    //  且 H.狀態預計結束日 IS NOT NULL
    //  且 H.人事異動狀態結束編號 IS NULL
    //  且 H.生效處理狀態 = 'Y'
    //  若讀到資料，則 明細畫面.狀態結束自動為勾選，明細畫面.異動主編號 = H.人事異動編號
    //  若讀不到資料，繼續作業。
    //3.若 G.是否暫時狀態(IS_TEMP)為'Y'或'N'，則 不控制 狀態預計結束日 及 狀態結束 是否必須輸入，由人工自行控制，應受援除外(B10)。
    //4.<人事異動代碼>連動部份
    //  若  G.人事異動代碼為B10(應受援)  或   G.是否暫時狀態(IS_TEMP)為'E' 時，自動去取得該異動相關的異動主編號
    //  讀取 人事異動主檔 H
    //  取得: H.人事異動編號, H.人事異動代碼
    //  條件: H.工號 = 明細畫面.工號
    //  且 H.人事異動生效日 < 明細畫面.異動生效日
    //  且 H.狀態預計結束日 IS NOT NULL
    //  且 H.人事異動狀態結束編號 IS NULL
    //  且 H.生效處理狀態 = 'Y'
    //  若讀到資料，
    //    A.則 明細畫面.狀態結束自動為勾選，且不可修改，明細畫面.異動主編號 = H.人事異動編號
    //    B.取得 異動主編號相關的說明
    //      (a)非D04(結束兼任)的異動主編號說明
    //          讀取 人事異動代碼檔 I
    //          取得: I.人事異動代碼說明
    //          條件: I.人事異動代碼 = H. 人事異動代碼檔
    //      (b)D04(結束兼任)的異動主編號說明
    //          若H.人事異動代碼 為 D04(結束兼任)
    //          (b1)取得兼任的部門名稱
    //              讀取 人事異動明細檔 J
    //              取得: J.異動後代碼說明, J.異動後代碼說明
    //              條件: J.人事異動編號 = H. 人事異動編號
    //                    J.人事異動項目代碼 = 05 (部門)
    //          (b1)取得兼任的職務名稱
    //              讀取 人事異動明細檔 K
    //              取得: K.異動後代碼說明, K.異動後代碼說明
    //              條件: K.人事異動編號 = H. 人事異動編號
    //                    K.人事異動項目代碼 = 08 (職務)
    //    C.明細畫面.異動主編號說明為
    //      若H.人事異動代碼 為 D04(結束兼任)
    //        則 明細畫面.異動主編號說明 =  H. 人事異動代碼檔 +"  "+ I.人事異動代碼說明+" "+ J.異動後代碼說明 +" "+J.異動後代碼說明
    //      其餘
    //        則 明細畫面.異動主編號說明 =  H. 人事異動代碼檔 +"  "+ I.人事異動代碼說明
    //  若讀不到資料，則 明細畫面.狀態結束 改為未勾選，顯示提醒訊息"無人事異動單可結束狀態"。
    public ArrayList Get_IS_END(string hr_chg_cd, bool is_end, string emp_id, string start_dt)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            string emp_chg_status = "";     //身份狀態  20201028  
            if (hr_chg_cd.Length > 0 && emp_id.Length > 0 && IsDate(start_dt))
            {
                string tmp = "";
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                if (hr_chg_cd != "" && hr_chg_cd != null)
                {
                    //2.若 明細畫面.人事異動代碼輸入為'C'開頭(離社的異動)的代碼，且未勾選結束狀態，
                    //if (hr_chg_cd.Substring(0, 1) == "C" && !is_end)
                    //{
                    //    tmp = dao.Get_IS_END(emp_id, start_dt);
                    //    if (tmp != "")
                    //    {
                    //        //  若讀到資料，則 明細畫面.狀態結束自動為勾選，明細畫面.異動主編號 = H.人事異動編號
                    //        rtnval.Add(new string[] { "", "true", "false", tmp });
                    //    }
                    //}
                    
                    if (dao.Get_IS_TEMP(hr_chg_cd)[0] == "E")
                    {
                        //若  G.人事異動代碼為B10(應受援)  或   G.是否暫時狀態(IS_TEMP)為'E' 時，自動去取得該異動相關的異動主編號
                        if (hr_chg_cd == "D01") emp_chg_status = "L";  //復職=>L(留停)
                        if (hr_chg_cd == "D02") emp_chg_status = "21";  //歸建=>21(外調)
                        if (hr_chg_cd == "D03") emp_chg_status = "22";  //結束應受援=>22(應受援)
                        if (hr_chg_cd == "D04") emp_chg_status = "20";  //結束兼任 =>20(兼任)
                        if (hr_chg_cd == "B22") emp_chg_status = "33";  //返廠 =>33(返校)

                        tmp = dao.Get_IS_END_STATUS(emp_id, start_dt, emp_chg_status);
                        if (tmp != "")
                        {
                            //  若讀到資料，
                            //    A.則 明細畫面.狀態結束自動為勾選，且不可修改，明細畫面.異動主編號 = H.人事異動編號
                            rtnval.Add(new string[] { "", "true", "true", tmp });
                        }

                    }
                    /*
                    if (hr_chg_cd == "B10")
                    {
                        //若  G.人事異動代碼為B10(應受援)  或   G.是否暫時狀態(IS_TEMP)為'E' 時，自動去取得該異動相關的異動主編號
                        tmp = dao.Get_IS_END_STATUS(emp_id, start_dt, "B10");
                        if (tmp != "")
                        {
                            //  若讀到資料，
                            //    A.則 明細畫面.狀態結束自動為勾選，且不可修改，明細畫面.異動主編號 = H.人事異動編號
                            rtnval.Add(new string[] { "", "true", "true", tmp });
                        }

                    }
                    */
                    if ((hr_chg_cd == "B22") && !is_end)
                    {
                        tmp = dao.Get_IS_END_chgcd(emp_id, start_dt, "B21");
                        if (tmp != "")
                        {
                            //  若讀到資料，則 明細畫面.狀態結束自動為勾選，明細畫面.異動主編號 = H.人事異動編號
                            rtnval.Add(new string[] { "", "true", "false", tmp });
                        }
                    }
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //<異動主編號>
    //1.若 明細畫面.狀態結束有勾選，
    //  讀取 人事異動主檔 H
    //  取得: H.人事異動編號
    //  條件: H.工號 = 明細畫面.工號
    //        且 H.人事異動生效日 < 明細畫面.異動生效日
    //        且 H.狀態預計結束日 IS NOT NULL
    //        且 H.人事異動狀態結束編號 IS NULL
    //        且 H.生效處理狀態 = 'Y'
    //  若讀到資料，則 明細畫面.狀態結束自動為勾選，且不可修改，明細畫面.異動主編號 = H.人事異動編號
    //  若讀不到資料，則 明細畫面.狀態結束 改為未勾選，顯示提醒訊息"無人事異動單可結束狀態"。
    public ArrayList Get_MAIN_HR_CHG_NO(bool is_end, string emp_id, string start_dt)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (is_end && emp_id.Length > 0 && IsDate(start_dt))
            {
                string tmp = "";
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                //1.若 明細畫面.狀態結束有勾選，
                if (is_end && emp_id.Length == 5 && IsDate(start_dt))
                {
                    tmp = dao.Get_MAIN_HR_CHG_NO(emp_id, start_dt);
                    if (tmp != "")
                    {
                        //  若讀到資料，則 明細畫面.狀態結束自動為勾選，且不可修改，明細畫面.異動主編號 = H.人事異動編號
                        rtnval.Add(new string[] { "", "true", "true", tmp });
                    }
                    else
                    {
                        //  若讀不到資料，則 明細畫面.狀態結束 改為未勾選，顯示提醒訊息"無人事異動單可結束狀態"。
                        rtnval.Add(new string[] { Resources.Resource.wfb2hc_NO_HR_CHG_NO, "false", "false", tmp });
                    }
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //<異動主編號>
    //2.連動異動主編號的說明
    //    (1) 取得 異動主編號相關的說明
    //        讀取 人事異動代碼檔 I
    //        取得: I.人事異動代碼說明
    //        條件: I.人事異動代碼 = H. 人事異動代碼檔

    //    (2)D04(結束兼任)的異動主編號說明
    //        若H.人事異動代碼 為 D04(結束兼任)
    //            (2-1)取得兼任的部門名稱
    //                    讀取 人事異動明細檔 J
    //                    取得:	J.異動後代碼說明, J.異動後代碼說明
    //                    條件:	J.人事異動編號 = H. 人事異動編號
    //                          J.人事異動項目代碼 = 05 (部門)
    //            (2-2)取得兼任的職務名稱
    //                    讀取 人事異動明細檔 K
    //                    取得:	K.異動後代碼說明, K.異動後代碼說明
    //                    條件:	K.人事異動編號 = H. 人事異動編號
    //                          K.人事異動項目代碼 = 08 (職務)
    //    (3)明細畫面.異動主編號說明
    //            若H.人事異動代碼 為 D04(結束兼任)
    //                則 明細畫面.異動主編號說明 =  H. 人事異動代碼檔 +"  "+ I.人事異動代碼說明+" "+ J.異動後代碼說明 +" "+J.異動後代碼說明
    //            其餘
    //                則 明細畫面.異動主編號說明 =  H. 人事異動代碼檔 +"  "+ I.人事異動代碼說明
    public ArrayList Get_MAIN_HR_CHG_NO_DESC(string hr_chg_no, string emp_id, string hr_chr_CD)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (hr_chg_no.Length > 0 && emp_id.Length > 0)
            {
                string tmp = "";
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                tmp = dao.Get_MAIN_HR_CHG_NO_DESC(hr_chg_no, emp_id, hr_chr_CD);
                if (tmp != "")
                {
                    rtnval.Add(new string[] { "", tmp });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //［外調資料］
    //<ICT類別>
    //	1.若 明細畫面.人事異動代碼為'B08'(GCC駐在)、'B07'(國內出向支援)，則 此欄位DISABLED不可輸入。
    //  2.若 明細畫面.人事異動代碼為'B09'(ICT)，則 此欄位必須輸入，否則顯示錯誤訊息"必須輸入ICT類別"。
    //<受入國家>
    //  1.若 明細畫面.人事異動代碼為'B07'(國內出向支援)，則 此欄位預設為'TWN-台灣'，且DISABLED不可修改。
    //  2.若 明細畫面.人事異動代碼為'B09'(ICT)、'B08'(GCC駐在)，則 此欄位必須輸入，否則顯示錯誤訊息"必須輸入外調之受入國家"。
    //<受入公司>
    //  1.此欄位必須輸入，否則顯示錯誤訊息"必須輸入外調之受入公司"。
    //<受入部門>
    //  1.此欄位必須輸入，否則顯示錯誤訊息"必須輸入外調之受入部門"。
    public ArrayList Check_TRANSFER(string hr_chg_cd, string ict_type, string transfer_nation_cd, string transfer_company_cd, string transfer_dept)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            string errMsg = "";
            if (hr_chg_cd.Length > 0)
            {
                //<ICT類別>
                //  2.若 明細畫面.人事異動代碼為'B09'(ICT)，則 此欄位必須輸入，否則顯示錯誤訊息"必須輸入ICT類別[2013/1/21 'B09'(ICT)改為不檢查]"。
                /*
                if (hr_chg_cd == "B09" && ict_type == "") {
                    if (errMsg != "") 
                        errMsg += "\\n";
                    errMsg += Resources.Resource.wfb2hc_Required_ICT_TYPE;
                }
                 */
                //<受入國家>
                //  2.若 明細畫面.人事異動代碼為'B09'(ICT)、'B08'(GCC駐在)，則 此欄位必須輸入，否則顯示錯誤訊息"必須輸入外調之受入國家"。
                if ((hr_chg_cd == "B09" || hr_chg_cd == "B08") && transfer_nation_cd == "")
                {
                    if (errMsg != "")
                        errMsg += "\\n";
                    errMsg += Resources.Resource.wfb2hc_Required_TRANSFER_NATION_CD;
                }
                //<受入公司>
                //  1.此欄位必須輸入，否則顯示錯誤訊息"必須輸入外調之受入公司"。
                if ((hr_chg_cd == "B07" || hr_chg_cd == "B08" || hr_chg_cd == "B09") && transfer_company_cd == "")
                {
                    if (errMsg != "")
                        errMsg += "\\n";
                    errMsg += Resources.Resource.wfb2hc_Required_TRANSFER_COMPANY_CD;
                }
                //<受入部門>
                //  1.此欄位必須輸入，否則顯示錯誤訊息"必須輸入外調之受入部門[2014/1/21 'B09'(ICT)改為不檢查, B08(GCC) 2015/03/04 改為不檢查]"。
                if ((hr_chg_cd == "B07" /*||  hr_chg_cd == "B08" || hr_chg_cd == "B09"*/) && transfer_dept == "")
                {
                    if (errMsg != "")
                        errMsg += "\\n";
                    errMsg += Resources.Resource.wfb2hc_Required_TRANSFER_DEPT;
                }

                rtnval.Add(new string[] { errMsg });
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //◎兼任
    //<兼任部門>
    //   1.若直接輸入，
    //        讀取 部門基本資料檔 D
    //            取得:	D.部門名稱
    //            條件:	D.部門代號 = 明細畫面.部門代號
    //                  且 明細畫面.異動生效日 >= D.生效日期 且 明細畫面.異動生效日 <= D.結束日期
    //            若讀不到，顯示錯誤訊息"部門代號不存在"；
    //    2.若CLICK代號欄的BUTTON，開啟<<部門查詢視窗-清單>>傳入(無)(明細畫面.部門代號)(明細畫面.異動生效日)，選取其一之後取得代號及部門名稱，顯示於畫面上。
    public ArrayList Adjunct_Get_DEPT_NAME(string dept_no, string start_dt)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (dept_no.Length > 0 && IsDate(start_dt))
            {
                string data = "";
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                data = dao.Adjunct_Get_DEPT_NAME(dept_no, start_dt);
                if (data == "")
                {
                    //若讀不到，顯示錯誤訊息"部門代號不存在"；
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_DEPT_NAME_NotFound });
                }
                else
                {
                    rtnval.Add(new string[] { "", data });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //<兼任職務>  ※兼任是向下兼任，所以兼任職務所對應的職種與資格，跟工號本身的職種與資格無關
    //1.若直接輸入，
    //    讀取 職務檔 P
    //        取得:	P.職務名稱
    //        條件:	P.職務代號 = 明細畫面.職務代號
    //              且 明細畫面.異動生效日 >= D.生效日期 且 明細畫面.異動生效日 <= D.結束日期
    //        若讀不到，顯示錯誤訊息"職務代號不存在"；
    //2.若CLICK代號欄的BUTTON，開啟<<職務查詢視窗>>傳入(無)(無,無,明細畫面.職務代號)(明細畫面.異動生效日)，選取其一之後取得代碼及說明。
    public ArrayList Adjunct_Get_PJOB_DESC(string pjob_cd, string start_dt)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (pjob_cd.Length > 0 && IsDate(start_dt))
            {
                string data = "";
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                data = dao.Adjunct_Get_PJOB_DESC(pjob_cd, start_dt);
                if (data == "")
                {
                    //若讀不到，顯示錯誤訊息"職務代號不存在"；
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_PJOB_DESC_NotFound });
                }
                else
                {
                    rtnval.Add(new string[] { "", data });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //◎兼任以外的人事異動																																																					
    //<異動項目>																																																					
    //    讀取 人事異動代碼可異動項目檔 T																																																				
    //        取得:	T.人事異動項目代碼																																															
    //        條件:	T.人事異動代碼 = 明細畫面.人事異動代碼																																															
    //              且 T.使用中 = 'Y'																																																
    //        若讀不到資料，GRID DISABLED不可輸入。																																																			
    //        若讀得到資料，																																																			
    //            每一人事異動項目代碼，																																																		
    //            讀取 共用代碼明細檔 C																																																		
    //                取得: C.代碼名稱																																													
    //                條件:	C.子作業='HC' 且 C.類別='HR_CHG_ITEM'  且 C.IS_VALID='Y' 且 C.代碼=T.人事異動項目代碼																																													
    //        下拉選單顯示: T.人事異動項目代碼-C.代碼名稱																																																			
    public ArrayList Get_HR_CHG_ITEM_List(string hr_chg_cd)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (hr_chg_cd.Length > 0)
            {
                ArrayList data = new ArrayList();
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_List(hr_chg_cd);
                foreach (DataRow dr in dt.Rows)
                {
                    data.Add(new string[] { dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
                if (data.Count > 0)
                {
                    data.Insert(0, new string[] { "", "" });
                    rtnval.Add(new string[] { "" });
                    rtnval.Add(data);
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目01-異動前代碼、異動前代碼說明
    //1.若<異動項目> 為'01-聘用單位'，
    //    (1.1)E.聘用單位 顯示於 異動前代碼；讀取 共用代碼明細檔 以 子作業='HB' 且 類別='COMPANY_CD' 且 代碼=E.聘用單位  取得 代碼說明，顯示於 異動前代碼說明。
    public ArrayList Get_HR_CHG_ITEM_01_BEFORE(string emp_id)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (emp_id.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_01_BEFORE(emp_id);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目01-異動後代碼說明
    //1.若<異動項目> 為'01-聘用單位'，   
    //    (1.2)若直接輸入異動後代碼，
    //                讀取 共用代碼明細檔 C
    //                        取得:	C.代碼名稱
    //                        條件:	C.子作業='HB' 且 C.類別='COMPANY_CD'  且 C.IS_VALID='Y' 且 C.代碼=明細畫面.異動後代碼
    //                將C.代碼名稱，顯示於異動後代碼說明。
    //                若讀不到，顯示錯誤訊息"聘用單位代碼不存在"。
    public ArrayList Get_HR_CHG_ITEM_01_AFTER(string company_cd)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (company_cd.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_01_AFTER(company_cd);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
                else
                {
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_COMPANY_CD_is_not_exist });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目02-異動前代碼、異動前代碼說明
    //2.若<異動項目> 為'02-工廠區分'，
    //(2.1)E.工廠區分 顯示於 異動前代碼；讀取 共用代碼明細檔 以 子作業='HB' 且 類別='PLANT_CD' 且 代碼=E.工廠區分  取得 代碼說明，顯示於 異動前代碼說明。
    public ArrayList Get_HR_CHG_ITEM_02_BEFORE(string emp_id)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (emp_id.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_02_BEFORE(emp_id);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目02-異動後代碼說明
    //2.若<異動項目> 為'02-工廠區分'，
    //(2.2)若直接輸入異動後代碼，
    //            讀取 共用代碼明細檔 C
    //                    取得:	C.代碼名稱
    //                    條件:	C.子作業='HB' 且 C.類別='PLANT_CD'  且 C.IS_VALID='Y' 且 C.代碼=明細畫面.異動後代碼
    //            將C.代碼名稱，顯示於異動後代碼說明。
    //            若讀不到，顯示錯誤訊息"工廠區分不存在"。 
    public ArrayList Get_HR_CHG_ITEM_02_AFTER(string plant_cd)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (plant_cd.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_02_AFTER(plant_cd);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
                else
                {
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_PLANT_CD_is_not_exist });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目03-異動前代碼、異動前代碼說明
    //3.若<異動項目> 為'03-職種'，
    //(3.1)E.職種 顯示於 異動前代碼；讀取 共用代碼明細檔 以 子作業='HB' 且 類別='WS_CD' 且 代碼=E.職種  取得 代碼說明，顯示於 異動前代碼說明。
    public ArrayList Get_HR_CHG_ITEM_03_BEFORE(string emp_id)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (emp_id.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_03_BEFORE(emp_id);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目03-異動後代碼說明
    //3.若<異動項目> 為'03-職種'，
    //(3.2)若直接輸入異動後代碼，
    //            讀取 共用代碼明細檔 C
    //                    取得: C.代碼名稱
    //                    條件:	C.子作業='HB' 且 C.類別='WS_CD'  且 C.IS_VALID='Y' 且 C.代碼=明細畫面.異動後代碼
    //            將C.代碼名稱，顯示於異動後代碼說明。
    //            若讀不到，顯示錯誤訊息"職種不存在"。
    public ArrayList Get_HR_CHG_ITEM_03_AFTER(string ws_cd)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (ws_cd.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_03_AFTER(ws_cd);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
                else
                {
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_WS_CD_is_not_exist });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目04-異動前代碼、異動前代碼說明
    //4.若<異動項目> 為'04-員工區分'，
    //(4.1)E.員工區分 顯示於 異動前代碼；讀取 共用代碼明細檔 以 子作業='HB' 且 類別='EMP_CD' 且 代碼=E.員工區分  取得 代碼說明，顯示於 異動前代碼說明。
    public ArrayList Get_HR_CHG_ITEM_04_BEFORE(string emp_id)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (emp_id.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_04_BEFORE(emp_id);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目04-異動後代碼說明
    //4.若<異動項目> 為'04-員工區分'，
    //(4.2)若直接輸入異動後代碼，
    //            讀取 共用代碼明細檔 C
    //                    取得:	C.代碼名稱
    //                    條件:	C.子作業='HB' 且 C.類別='EMP_CD'  且 C.IS_VALID='Y' 且 C.代碼=明細畫面.異動後代碼
    //            將C.代碼名稱，顯示於異動後代碼說明。
    //            若讀不到，顯示錯誤訊息"員工區分不存在"。
    public ArrayList Get_HR_CHG_ITEM_04_AFTER(string emp_cd)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (emp_cd.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_04_AFTER(emp_cd);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
                else
                {
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_EMP_CD_is_not_exist });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目05-異動前代碼、異動前代碼說明
    //5.若<異動項目> 為'05-部門'，
    //(5.1)E.部門代號 顯示於 異動前代碼；E.部門名稱，顯示於 異動前代碼說明。
    public ArrayList Get_HR_CHG_ITEM_05_BEFORE(string emp_id)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (emp_id.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_05_BEFORE(emp_id);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目05-異動後代碼說明
    //5.若<異動項目> 為'05-部門'，
    //(5.2)若直接輸入異動後代碼，
    //            讀取 部門基本資料檔 D
    //                    取得:	D.部門名稱, D.部門層級
    //                    條件:	D.部門代號 = 明細畫面.部門代號
    //                                    且 明細畫面.異動生效日 >= D.生效日期 且 明細畫面.異動生效日 <= D.結束日期
    //                                    若 資料權限之「部門含以下」或「部門權限」任一不為空值，
    //                                        加入條件：且 D.部門代號 必須存在以下 該擔當有權限作業的部門清單中，
    //                                        UNION 以下兩者的部門清單，
    //                                        若 資料權限之「部門含以下」為Y，
    //                                                    讀取 主管可管理部門資料檔 D
    //                                                        取得: D.可管理部門代號
    //                                                        條件:	D.工號 = 登入者帳號
    //                                        若 資料權限之「部門權限」不為空值，
    //                                                    「部門權限」的內容。
    //            若讀得到，
    //                若 資料權限之「小分類」為W(各單位擔當)，
    //                    讀取 部門層級檔 L
    //                        取得:	MAX(L.部門層級)
    //                        條件:	L.層級屬性代碼 = 'H'    --「人事管理層級」
    //                    若 D.部門層級 <= MAX(L.部門層級)，顯示錯誤訊息"各單位只能輸入「課」(不含)以下的單位異動"
    //                否則，
    //                    將D.部門名稱，顯示於異動後代碼說明。
    //            若讀不到，顯示錯誤訊息"部門代號不存在，或無權限作業"。
    public ArrayList Get_HR_CHG_ITEM_05_AFTER(string dept_no, string start_dt, string emp_id)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (dept_no.Length > 0 && IsDate(start_dt))
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_05_AFTER(dept_no, start_dt, emp_id);
                DataTable dt_plant = dao.checkDefaultPlant(dept_no, start_dt, emp_id);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    //若讀得到，
                    //若 資料權限之「小分類」不為為N(管理部擔當)[即 W(各單位擔當) 或Y(管理部主管時)，
                    //若 D.部門層級 <= MAX(L.部門層級)，顯示錯誤訊息"各單位只能輸入「課」(不含)以下的單位異動"
                    //if (dao.SYSCODEATT == "W" && Convert.ToInt16(dr["DEPT_LEVEL"].ToString()) <= Convert.ToInt16(dr["MAX_DEPT_LEVEL"].ToString()))
                    if (dao.SYSCODEATT != "N" && Convert.ToInt16(dr["DEPT_LEVEL"].ToString()) <= Convert.ToInt16(dr["MAX_DEPT_LEVEL"].ToString()))
                    {
                        rtnval.Add(new string[] { Resources.Resource.wfb2hc_Each_unit_can_only_enter_Lesson_without_the_following_units_Alert });
                        rtnval.Add(new string[] { "" });
                    }
                    else
                    {
                        //否則，
                        //將D.部門名稱，顯示於異動後代碼說明。
                        rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                        if (emp_id == "")
                        {
                            rtnval.Add(new string[] { "請記得變更工廠區分!" });
                        }
                        //20150622判斷該部門是否為 該員工的工廠區分 不同
                        if (dt_plant.Rows.Count > 0)
                        {
                            string resutlCount = dt_plant.Rows[0]["resultCount"].ToString();
                            if (resutlCount == "0")
                            {
                                rtnval.Add(new string[] { "" });
                                
                            }
                            else
                            {
                                rtnval.Add(new string[] { "異動後部門代號與該員工廠別不一致，請記得變更工廠區分!" });
                            }
                        }
                    }
                }
                else
                {
                    //若讀不到，顯示錯誤訊息"部門代號不存在，或無權限作業"。
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_DEPT_NO_is_not_exist_or_no_permission_to_work });
                    rtnval.Add(new string[] { "" });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目06-異動前代碼、異動前代碼說明
    //6.若<異動項目> 為'06-資格'，
    //(6.1)E.資格代號 顯示於 異動前代碼；NULL，顯示於 異動前代碼說明。    
    public ArrayList Get_HR_CHG_ITEM_06_BEFORE(string emp_id)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (emp_id.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_06_BEFORE(emp_id);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目06-異動後代碼說明
    //6.若<異動項目> 為'06-資格'，
    //(6.2)若直接輸入異動後代碼，
    //            讀取 資格檔 L
    //                    條件:	L.資格代號=明細畫面.異動後代碼
    //                          且 明細畫面.異動生效日 >= L.生效日期 且 明細畫面.異動生效日 <= L.結束日期
    //            異動後代碼說明顯示NULL。
    //            若讀不到，顯示錯誤訊息"資格代號不存在"。
    public ArrayList Get_HR_CHG_ITEM_06_AFTER(string level_cd, string start_dt)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (level_cd.Length > 0 && IsDate(start_dt))
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_06_AFTER(level_cd, start_dt);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
                else
                {
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_LEVEL_CD_is_not_exist });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目07-異動前代碼、異動前代碼說明
    //7.若<異動項目> 為'07-級數'，
    //(7.1)E.級數代號 顯示於 異動前代碼；NULL，顯示於 異動前代碼說明。
    public ArrayList Get_HR_CHG_ITEM_07_BEFORE(string emp_id)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (emp_id.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_07_BEFORE(emp_id);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目07-異動後代碼說明
    //7.若<異動項目> 為'07-級數'，
    //(7.2)若直接輸入異動後代碼，
    //            讀取 資格級數檔 LG
    //                    條件:	LG.級數代碼=明細畫面.異動後代碼
    //                          且 使用中 = 'Y'
    //                          若有輸入 <異動項目>'06-資格'，則加入條件: LG.資格代號=異動項目:'06-資格' 之異動後代碼
    //                          若未輸入 <異動項目>'06-資格'，則加入條件: LG.資格代號=E.資格代號
    //            異動後代碼說明顯示NULL。
    //            若讀不到，顯示錯誤訊息"級數代號不存在"。
    public ArrayList Get_HR_CHG_ITEM_07_AFTER(string grade_cd, string level_cd, string emp_id)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (grade_cd.Length > 0 && (level_cd.Length > 0 || emp_id.Length > 0))
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_07_AFTER(grade_cd, level_cd, emp_id);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
                else
                {
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_LEVEL_CD_is_not_exist });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //因一括異動, 異動人員在最後儲存前才能確認, 故在取說明時, 改以下處理
    //取得異動項目07-異動後代碼說明
    //7.若<異動項目> 為'07-級數'，
    //(7.2)若直接輸入異動後代碼，
    //            讀取 資格級數檔 LG
    //                    條件:	LG.級數代碼=明細畫面.異動後代碼
    //                          且 使用中 = 'Y'
    //            異動後代碼說明顯示NULL。
    //            若讀不到，顯示錯誤訊息"級數代號不存在"。
    public ArrayList Get_Add_batch_HR_CHG_ITEM_07_AFTER(string grade_cd)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (grade_cd.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_Add_batch_HR_CHG_ITEM_07_AFTER(grade_cd);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
                else
                {
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_LEVEL_CD_is_not_exist });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目08-異動前代碼、異動前代碼說明
    //8.若<異動項目> 為'08-職務'，
    //(8.1)E.職務代號 顯示於 異動前代碼；E.職務名稱，顯示於 異動前代碼說明。    
    public ArrayList Get_HR_CHG_ITEM_08_BEFORE(string emp_id)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (emp_id.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_08_BEFORE(emp_id);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //以新的資格去取得職務主檔是否存在
    public ArrayList Get_HR_CHG_ITEM_08_AFTER_NEW_LEVEL(string pjob_cd, string start_dt, string new_level_cd)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (pjob_cd.Length > 0 && IsDate(start_dt) && new_level_cd.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_08_AFTER_NEW_LEVEL(pjob_cd, start_dt, new_level_cd);
                //DataTable dt = dao.Get_Add_batch_HR_CHG_ITEM_08_AFTER(pjob_cd);  //只檢查
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
                else
                {
                    //rtnval.Add(new string[] { Resources.Resource.wfb2hc_PJOB_CD_is_not_exist });
                    rtnval.Add(new string[] { "新資格與新職務不存在職務主檔" });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }


    //取得異動項目08-異動後代碼說明
    //8.若<異動項目> 為'08-職務'，
    //(8.2)若直接輸入異動後代碼，
    //            讀取 職務檔 P
    //                    取得: P.職務名稱
    //                    條件:	P.職務代號 = 明細畫面.異動後代碼
    //                          且 P.職種 = E.職種
    //                          且 P.資格代號 = E.資格代號
    //                          且 明細畫面.異動生效日 >= D.生效日期 且 明細畫面.異動生效日 <= D.結束日期
    //            將P.職務名稱，顯示於異動後代碼說明。
    //            若讀不到，顯示錯誤訊息"職務代號不存在"。
    public ArrayList Get_HR_CHG_ITEM_08_AFTER(string pjob_cd, string start_dt, string emp_id)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (pjob_cd.Length > 0 && IsDate(start_dt) && emp_id.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_08_AFTER(pjob_cd, start_dt, emp_id);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
                else
                {
                    //rtnval.Add(new string[] { Resources.Resource.wfb2hc_PJOB_CD_is_not_exist });
                    rtnval.Add(new string[] { "新職務對應員工資格不存在" });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //因一括異動, 異動人員在最後儲存前才能確認, 故在取說明時, 改以下處理
    //取得異動項目08-異動後代碼說明
    //8.若<異動項目> 為'08-職務'，
    //(8.2)若直接輸入異動後代碼，
    //            讀取 VW_TB_H_M_PJOB P
    //                    取得: P.職務名稱
    //                    條件:	P.職務代號 = 明細畫面.異動後代碼   
    //            將P.職務名稱，顯示於異動後代碼說明。
    //            若讀不到，顯示錯誤訊息"職務代號不存在"。
    public ArrayList Get_Add_batch_HR_CHG_ITEM_08_AFTER(string pjob_cd)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (pjob_cd.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_Add_batch_HR_CHG_ITEM_08_AFTER(pjob_cd);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
                else
                {
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_PJOB_CD_is_not_exist });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目09-異動前代碼、異動前代碼說明
    //9.若<異動項目> 為'09-輪值表'，
    //(9.1)E.輪值表代碼 顯示於 異動前代碼；E.輪值表說明，顯示於 異動前代碼說明。
    public ArrayList Get_HR_CHG_ITEM_09_BEFORE(string emp_id)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (emp_id.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_09_BEFORE(emp_id);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目09-異動後代碼說明
    //9.若<異動項目> 為'09-輪值表'，
    //(9.2)若直接輸入異動後代碼，
    //            讀取 輪值表主檔 W
    //                    取得:	W.輪值表說明
    //                    條件:	W.IS_VALID='Y'
    //                          且 W.輪值表代碼=明細畫面.異動後代碼
    //                    若 資料權限之「小分類」為W(各單位擔當)，加入以下條件，
    //                                且 W.輪值表代碼 EXISTS (讀取 共用代碼明細檔 CD
    //                                          取得:CD.代碼
    //                                          條件:CD.子作業='HC' 且 CD.類別='WORKER_WORK_SHIFT' 且 CD.IS_VALID = 'Y')
    //            若讀不到，顯示錯誤訊息"輪值表代碼不存在"，
    //            若讀得到，將W.輪值表說明，顯示於異動後代碼說明。
    public ArrayList Get_HR_CHG_ITEM_09_AFTER(string work_shift_cd)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (work_shift_cd.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_09_AFTER(work_shift_cd);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
                else
                {
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_WORK_SHIFT_CD_is_not_exist });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目10-異動前代碼、異動前代碼說明
    //10.若<異動項目> 為'10-工數區分'，
    //(10.1)E.工數區分 顯示於 異動前代碼；讀取 共用代碼明細檔 以 子作業='HB' 且 類別='WORK_CD' 且 代碼=E.工數區分  取得 代碼說明，顯示於 異動前代碼說明。    
    public ArrayList Get_HR_CHG_ITEM_10_BEFORE(string emp_id)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (emp_id.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_10_BEFORE(emp_id);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得異動項目10-異動後代碼說明
    //10.若<異動項目> 為'10-工數區分'，
    //(10.2)若直接輸入異動後代碼，
    //            讀取 共用代碼明細檔 C
    //                    取得:	C.代碼名稱
    //                    條件:	C.子作業='HB' 且 C.類別='WORK_CD'  且 C.IS_VALID='Y' 且 C.代碼=明細畫面.異動後代碼
    //            將C.代碼名稱，顯示於異動後代碼說明。
    //            若讀不到，顯示錯誤訊息"工數區分不存在"。
    public ArrayList Get_HR_CHG_ITEM_10_AFTER(string work_cd)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (work_cd.Length > 0)
            {
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                DataTable dt = dao.Get_HR_CHG_ITEM_10_AFTER(work_cd);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    rtnval.Add(new string[] { "", dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
                else
                {
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_WORK_CD_is_not_exist });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得受入公司下拉選單資料
    public ArrayList Get_TRANSFER_COMPANY_CD(string hr_chg_cd)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            ArrayList data = new ArrayList();
            DataTable dt = new DataTable();
            if (hr_chg_cd == "B07")
            {
                dt = utilities.getCommCode("HC", "SUPPORT_COMPANY_CD", "", "");
                foreach (DataRow dr in dt.Rows)
                {
                    data.Add(new string[] { dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
            }
            else if (hr_chg_cd == "B08")
            {
                dt = utilities.getCommCode("HC", "GCC_COMPANY_CD", "", "");
                foreach (DataRow dr in dt.Rows)
                {
                    data.Add(new string[] { dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
            }
            else if (hr_chg_cd == "B09")
            {
                dt = utilities.getCommCode("HC", "ICT_COMPANY_CD", "", "");
                foreach (DataRow dr in dt.Rows)
                {
                    data.Add(new string[] { dr["sub_cd"].ToString(), dr["sub_desc"].ToString() });
                }
            }
            if (data.Count > 0)
            {
                data.Insert(0, new string[] { "", "" });
                rtnval.Add(new string[] { "" });
                rtnval.Add(data);
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //(2)取得人事異動編號：
    //◎若人事異動代碼，非'B06'(兼任)，
    //    讀取 自動給號控制檔 A
    //        取得:	A.流水號(SERIAL_NO)
    //        條件:	A.給號類別='HR_CHG_NO'
    //              且 A.給號日期=明細畫面.異動生效日
    //        若讀不到，
    //            WK_人事異動編號=明細畫面.異動生效日(格式:YYYYMMDD)+'0001'
    //            UPDATE 自動給號控制檔 SET 流水號=2
    //                    條件:	A.給號類別='HR_CHG_NO'
    //                          且 A.給號日期=明細畫面.異動生效日
    //        若讀得到，
    //            WK_人事異動編號=明細畫面.異動生效日(格式:YYYYMMDD)+A.流水號(前置0補足4碼)
    //            UPDATE 自動給號控制檔 SET 流水號=流水號+1
    //                    條件:	A.給號類別='HR_CHG_NO'
    //                          且 A.給號日期=明細畫面.異動生效日

    //◎若人事異動代碼，為'B06'(兼任)，
    //    則 GRID每一筆資料，就給一個 WK_人事異動編號，取號原則同上。																																															
    //COMMIT。
    //(3)以 (1)取得的人事異動編號+明細畫面.工號 讀取 人事異動主檔， 如資料存在，則顯示錯誤訊息"人事異動編號+工號重覆"。
    public ArrayList Get_HR_CHG_NO(string emp_id, string hr_chg_cd, string start_dt, int gv_result_Rows_Count)
    {
        ArrayList rtnval = new ArrayList();
        ArrayList data = new ArrayList();
        CFB2HC0100DAO dao = new CFB2HC0100DAO();
        try
        {
            BeginTransaction();
            data = dao.Get_HR_CHG_NO(emp_id, hr_chg_cd, start_dt, gv_result_Rows_Count);
            Commit();
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
        try
        {
            rtnval.Add(new string[] { dao.checkHR_CHG_NO(data, emp_id) });
            rtnval.Add(data);
            return rtnval;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    //取得人事異動編號
    public ArrayList Get_Add_batch_HR_CHG_NO(List<string> emp_ids, string start_dt)
    {
        ArrayList rtnval = new ArrayList();
        ArrayList data = new ArrayList();
        CFB2HC0100DAO dao = new CFB2HC0100DAO();
        try
        {
            BeginTransaction();
            data.Add(dao.Get_HR_CHG_NO(start_dt));
            Commit();
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
        try
        {
            rtnval.Add(new string[] { dao.check_Add_batch_HR_CHG_NO(data, emp_ids) });
            rtnval.Add(data);
            return rtnval;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    //(4)◎若人事異動代碼，非'B06'(兼任)，
    //    讀取 人事異動主檔 A
    //        取得:	MAX(A.序號)
    //        條件:	A.工號=明細畫面.工號
    //              且 A.人事異動生效日=明細畫面.異動生效日
    //        若讀不到，
    //              WK_序號=1
    //        若讀得到，
    //              WK_序號=MAX(A.序號)+1
    //◎若人事異動代碼，為'B06'(兼任)，
    //    則 GRID每一筆資料，就給一個 WK_序號，下一筆累加1。
    public ArrayList Get_CHG_SEQ(string emp_id, string hr_chg_cd, string start_dt, int gv_result_Rows_Count)
    {
        ArrayList rtnval = new ArrayList();
        ArrayList data = new ArrayList();
        CFB2HC0100DAO dao = new CFB2HC0100DAO();
        try
        {
            data = dao.Get_CHG_SEQ(emp_id, hr_chg_cd, start_dt, gv_result_Rows_Count);
            rtnval.Add(new string[] { "" });
            rtnval.Add(data);
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //1.讀取 人事異動主檔 A
    //條件: A.人事異動編號 = 畫面.人事異動編號
    //      且 A.工號 = 畫面.工號
    //JOIN 員工人事資料VIEW(VW_H_EMP_DATA) E，條件:E.工號 = A.工號
    //JOIN 人事異動代碼檔 G，條件:G.人事異動代碼 = A.人事異動代碼
    public void Get_Master_Data(string hr_chg_no, string emp_id)
    {
        try
        {
            CFB2HC0100DAO dao = new CFB2HC0100DAO();
            dao.Get_Master_Data(hr_chg_no, emp_id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void SP_H_HR_CHG_PROC(List<string> emp_ids, string minStartDT)
    {
        try
        {
            CFB2HC0100DAO dao = new CFB2HC0100DAO();
            BeginTransaction();
            dao.SP_H_HR_CHG_PROC(emp_ids);

            CFB2HB0700DAO hb070DAO = new CFB2HB0700DAO();

            //執行共用的SP
            //呼叫-員工人事履歷生成
            hb070DAO.JOIN_DT_2 = minStartDT;
            hb070DAO.SP_H_EMP_HR_CHG_RECORD(SessionHandle.Current.emp_id, "FB2HC010");

            hb070DAO.JOIN_DT_2 = DateTime.Now.ToString("yyyy/MM/dd");
            //呼叫-部門主管更新作業
            hb070DAO.SP_H_UPD_DEPT_HEAD(SessionHandle.Current.emp_id, "FB2HC010");
            //呼叫-員工主管更新作業
            hb070DAO.SP_H_UPD_EMP_HEAD(SessionHandle.Current.emp_id, "FB2HC010");
            //呼叫-主管可管理部門資料生成
            hb070DAO.SP_H_HEAD_DEPT(SessionHandle.Current.emp_id, "FB2HC010");
            //呼叫-部門資料生成
            hb070DAO.SP_H_DEPT_DATA(SessionHandle.Current.emp_id, "FB2HC010");
            //呼叫-員工資料生成
            hb070DAO.SP_H_EMP_DATA(SessionHandle.Current.emp_id, "FB2HC010");



            Commit();
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    public bool IsDate(Object obj)
    {
        string strDate = obj.ToString();
        try
        {
            DateTime dt = DateTime.Parse(strDate);
            return true;
        }
        catch
        {
            return false;
        }
    }

    //<人事異動代碼>
    //1.人事異動代碼若有輸入必須輸入完整3碼的長度，否則顯示錯誤訊息"人事異動代碼必須輸入3碼的代碼"。
    //2.◎人事異動代碼如果直接輸入，
    //        讀取 人事異動代碼檔 G
    //            取得:	G.*
    //            條件:	G.人事異動代碼 = 明細畫面.人事異動代碼
    //                  且 G.使用中 = 'Y'
    //                  且 G.一括異動適用 = 'Y'
    //                  若 資料權限之「小分類」為N(管理部擔當)，
    //                      加入條件：且 G.人事異動代碼 必須存在於  (讀取 人事異動代碼擔當檔 F
    //                                                               取得:F.人事異動代碼
    //                                                               條件:F.工號 = 登入者帳號 且 F.使用中 = 'Y')
    //                  若 資料權限之「小分類」為W(各單位擔當)，
    //                      加入條件：且 G.權限區分 = 'D'
    //            若讀不到，顯示錯誤訊息"人事異動代碼不存在，或無權限作業"。
    public ArrayList Get_Add_batch_HR_CHG_DESC(string hr_chg_cd)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (hr_chg_cd.Length > 0)
            {
                if (hr_chg_cd.Length != 3)
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_HR_CHG_CD_Length_Error, "" });
                string tmp = "";
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                tmp = dao.Add_batch_Get_HR_CHG_DESC(hr_chg_cd);
                if (tmp == "")
                    rtnval.Add(new string[] { Resources.Resource.wfb2hc_HR_CHG_CD_does_not_exist_or_no_permission_to_work, tmp });
                else
                    rtnval.Add(new string[] { "", tmp });
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //<保險預計處理日>
    //1.若 G.保險提前生效(IS_INS_EARLIER)為'Y'，則 明細畫面.保險預計處理日 必須< 明細畫面.異動生效日，否則顯示錯誤訊息"保險預計處理日必須＜異動生效日"；
    //  否則，明細畫面.保險預計處理日=明細畫面.異動生效日，且不可修改，將其DISABLED。
    public ArrayList Check_Add_batch_INS_PLAN_PROC_DT(string hr_chg_cd, string ins_plan_proc_dt, string start_dt)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (hr_chg_cd.Length > 0 && IsDate(start_dt))
            {
                string tmp = "";
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                tmp = dao.Get_IS_INS_EARLIER(hr_chg_cd);
                //1.若 G.保險提前生效(IS_INS_EARLIER)為'Y'
                if (tmp == "Y")
                {
                    //則 明細畫面.保險預計處理日 必須< 明細畫面.異動生效日，且必須>系統日，
                    if (ins_plan_proc_dt != "" && ins_plan_proc_dt.CompareTo(start_dt) < 0 && ins_plan_proc_dt.CompareTo(DateTime.Now.ToString("yyyy/MM/dd")) >= 0)
                    {
                        rtnval.Add(new string[] { "", tmp });
                    }
                    else
                    {
                        //否則顯示錯誤訊息"保險預計處理日必須＜異動生效日"且必須＞系統日"；
                        rtnval.Add(new string[] { Resources.Resource.wfb2hc_INS_PLAN_PROC_DT_Error_Message });
                    }
                }
                else
                {
                    rtnval.Add(new string[] { "", tmp });
                }
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //<狀態結束>
    //1.若 G.是否暫時狀態(IS_TEMP)為'Y'，則 明細畫面.狀態結束不可勾選，將其DISABLED。
    //2.若 G.是否暫時狀態(IS_TEMP)為'E'，則 明細畫面.狀態結束必須勾選，否則顯示錯誤訊息"必須勾選狀態結束"。
    public ArrayList Check_Add_batch_IS_END(string hr_chg_cd, bool is_end, bool check_IS_END_require = false)
    {
        try
        {
            ArrayList rtnval = new ArrayList();
            if (hr_chg_cd.Length > 0)
            {
                string[] tmp = new string[1];
                CFB2HC0100DAO dao = new CFB2HC0100DAO();
                tmp = dao.Get_IS_TEMP(hr_chg_cd);
                rtnval.Add(new string[] { "", tmp[0] });
                /*
                if (tmp[0] == "E")
                {
                    if (!is_end)
                    {
                        rtnval.Add(new string[] { Resources.Resource.wfb2hc_Required_IS_END });
                    }
                }
                else
                {
                    rtnval.Add(new string[] { "", tmp[0] });
                }
                */
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool has_Code_Item(string hr_chg_cd)
    {
        try
        {
            bool result = false;
            CFB2HC0100DAO dao = new CFB2HC0100DAO();
            DataTable dt = dao.Get_HAS_CODE_ITEM(hr_chg_cd);
            if (dt.Rows.Count > 0)
            {
                int countNum = Convert.ToInt32(dt.Rows[0]["resultCount"]);
                if (countNum > 0)
                {
                    result = true;
                }
            }
            return result;
        }
        catch
        {
            throw;
        }

    }

    public string getEMP_NAME(string emp_id)
    {
        try
        {
            string result = "";
            if (emp_id != "")
            {
                CFB2HC0100DAO wfb2hc = new CFB2HC0100DAO();
                DataTable tmp = wfb2hc.getEMP_NAME(emp_id);
                if (tmp.Rows.Count > 0)
                {
                    result = tmp.Rows[0]["EMP_NAME"].ToString();
                }
            }

            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getHR_CHG_DESC(string hr_chg_cd)
    {
        try
        {
            string result = "";
            if (hr_chg_cd != "")
            {
                CFB2HC0100DAO wfb2hc = new CFB2HC0100DAO();
                DataTable tmp = wfb2hc.getHR_CHG_DESC(hr_chg_cd);
                if (tmp.Rows.Count > 0)
                {
                    result = tmp.Rows[0]["HR_CHG_DESC"].ToString();
                }
            }

            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得生效日之前的上班日
    public string getINS_PLAN_PROC_DT(string start_dt)
    {
        try
        {
            string result = start_dt;
            if (start_dt != "")
            {
                CFB2HC0100DAO hc010DAO = new CFB2HC0100DAO();
                DataTable tmp = hc010DAO.getINS_PLAN_PROC_DT(start_dt);
                if (tmp.Rows.Count > 0)
                {
                    result = tmp.Rows[0]["CALENDAR_DT"].ToString();
                }
            }

            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }  

}