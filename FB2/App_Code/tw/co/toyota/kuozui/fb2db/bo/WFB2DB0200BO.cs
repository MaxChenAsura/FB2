using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.Data;

/// <summary>
/// WFB2DB0200BO 的摘要描述
/// </summary>
public class WFB2DB0200BO : BaseService
{
    private WFB2DB0200DL dl = null;

    public WFB2DB0200BO()
    {
        dl = new WFB2DB0200DL();
    }

    public DataTable GetGridData(int startRowIndex, int maximumRows, string CALENDAR_DT_Start,
                             string CALENDAR_DT_End, string PLANT_CD, string DEPT_NO,
                             string EMP_ID, string JOIN_DT_Start, string JOIN_DT_End,
                             string WORK_SHIFT_CD, string DEPTAuth, string IsDEPT, string sp_dept, string work_day_cd, string shift_cd, string sortExpression)
    {
        return dl.GetGridData(startRowIndex, maximumRows, CALENDAR_DT_Start, CALENDAR_DT_End, PLANT_CD, DEPT_NO, EMP_ID, JOIN_DT_Start, JOIN_DT_End, WORK_SHIFT_CD, DEPTAuth, IsDEPT, sp_dept, work_day_cd, shift_cd, sortExpression);
    }

    public int GetGridDataCount(int startRowIndex, int maximumRows, string CALENDAR_DT_Start,
                               string CALENDAR_DT_End, string PLANT_CD, string DEPT_NO,
                               string EMP_ID, string JOIN_DT_Start, string JOIN_DT_End,
                               string WORK_SHIFT_CD, string DEPTAuth, string IsDEPT,
                               string sp_dept, string work_day_cd,string shift_cd)
    {
        return dl.GetGridDataCount(startRowIndex, maximumRows, CALENDAR_DT_Start, CALENDAR_DT_End, PLANT_CD, DEPT_NO, EMP_ID, JOIN_DT_Start, JOIN_DT_End, WORK_SHIFT_CD, DEPTAuth, IsDEPT, sp_dept, work_day_cd, shift_cd);
    }

    public WFB2DB0200DAO GetSingleData(WFB2DB0200DAO dao)
    {
        return dl.GetSingleData(dao);
    }

    //一括異動 班別
    public string BatchEdit(List<Tuple<string, string, string>> keysList)
    {
        string msg = "0";
        try
        {
            try
            {
                WFB2DB0200DL dl = new WFB2DB0200DL();
                BeginTransaction();
                foreach (var item in keysList)
                {
                    utilities.UPD_EMP_DAY_DUTY3(dl.GetdbConn, item.Item1, item.Item3, null, Convert.ToDateTime(item.Item2), Convert.ToDateTime(item.Item2), SessionHandle.Current.emp_id);
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
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public bool UpdateData(WFB2DB0200DAO dao, out string Message)
    {

        //改寫成直接執行SQL後
        Message = string.Empty;
        this.BeginTransaction();
        try
        {
            WFB2DB0200DL dl = new WFB2DB0200DL();
            dl.UpdateData(dao);
            //utilities.UPD_EMP_DAY_DUTY3(dl.GetdbConn, dao.EMP_ID, dao.SHIFT_CD, dao.WORK_DAY_CD, dao.CALENDAR_DT, dao.CALENDAR_DT, SessionHandle.Current.emp_id);
            this.Commit();
            return true;

        }
        catch (Exception ex)
        {
            this.RollBack();
            Message = ex.Message;
            return false;
        }
    }
    public DataTable GetTB_D_M_SHIFT_H(string SHIFT_CD, DateTime CALENDAR_DT)
    {
        return dl.GetTB_D_M_SHIFT_H(SHIFT_CD, CALENDAR_DT);
    }

    //依權限取得班別
    public DataTable getSHIFT_CD(string emp_id, string calendar_dt)
    {
        return dl.getSHIFT_CD(emp_id, calendar_dt);
    }

    //依權限取得班別
    public DataTable getSHIFT_CD_ALL(string emp_id, string calendar_dt)
    {
        return dl.getSHIFT_CD_ALL(emp_id, calendar_dt);
    }

   
    public string getFN_DB020_01(string emp_id, string calendar_dt, string updateShiftCD)
    {
        try
        {
            string result = "";
            //string calendar_dt_y = calendar_dt.Split('/')[0];
            //DataTable dt = dl.getSHIFT_CD(emp_id, work_shift_cd, work_day_cd, calendar_dt_y);
            DataTable dt = dl.getSHIFT_CD_Batch(emp_id, calendar_dt, updateShiftCD);
            if (dt.Rows.Count == 0)
            {
                result = "工號:" + emp_id + ",勤務日期:" + calendar_dt + ",其班別不符合班表調整設定檔的規定;";
            }
            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }


    //檢核班表是否有間隔11小時
    public string exec_SP_DH_SHIFT_DUTY_CHK(WFB2DB0200DAO dao)
    {
        try
        {
            string result = "0";
            string errMSG = dl.exec_SP_DH_SHIFT_DUTY_CHK(dao);
            string rtn_flag = errMSG.Split('|')[0];
            string rtn_Msg = errMSG.Split('|')[1];

            //確認SP有無成功
            if (rtn_flag == "E")
                result = rtn_Msg;
            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }


}