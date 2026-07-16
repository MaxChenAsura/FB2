using FB2.tw.co.toyota.kuozui.bo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// WFB2DA0200BO 的摘要描述
/// </summary>
public class WFB2DA0200BO : BaseService
{
    private WFB2DA0200DL dl = null;

    public WFB2DA0200BO()
    {
        dl = new WFB2DA0200DL();
    }

    public int GetGridDataCount(int startRowIndex, int maximumRows, string SHIFT_CD,
                            string SHIFT_TIME_CD, string SHIFT_DESC, string VALID,
                            string START_DT, string END_DT, string DUTY_TIME,
                            string EAT_TIME, string REST_TIME, string WORK_SHIFT_ALLOWANCE_TYPE,string IS_IFLOW_SHOW)
    {
        return dl.GetGridDataCount(startRowIndex, maximumRows, SHIFT_CD, SHIFT_TIME_CD, SHIFT_DESC, VALID, START_DT, END_DT, DUTY_TIME, EAT_TIME, REST_TIME, WORK_SHIFT_ALLOWANCE_TYPE, IS_IFLOW_SHOW);
    }

    public DataTable GetGridData(int startRowIndex, int maximumRows, string SHIFT_CD,
                            string SHIFT_TIME_CD, string SHIFT_DESC, string VALID,
                            string START_DT, string END_DT, string DUTY_TIME,
                            string EAT_TIME, string REST_TIME, string WORK_SHIFT_ALLOWANCE_TYPE, string IS_IFLOW_SHOW,
                            string sortExpression)
    {
        return dl.GetGridData(startRowIndex, maximumRows, SHIFT_CD, SHIFT_TIME_CD, SHIFT_DESC, VALID, START_DT, END_DT, DUTY_TIME, EAT_TIME, REST_TIME, WORK_SHIFT_ALLOWANCE_TYPE, IS_IFLOW_SHOW, sortExpression);
    }

    public bool WriteBeforeCheckSHIFT_H_Data(WFB2DA0200DAO dao, out string Message)
    {
        Message = string.Empty;
        if (dl.CheckTB_D_M_SHIFT_H_DataByKey(dao, false) > 0)
        {
            Message = Resources.Resource.wfd2da_TB_D_M_SHIFT_H_Already;
            return false;
        }
        else
            return true;

    }

    public bool WriteBeforeCheckEMP_DAY_DUTY_Data(WFB2DA0200DAO dao, out string Message)
    {
        Message = string.Empty;

        if (dl.CheckTB_D_M_EMP_DAY_DUTY(dao, false) > 0)
        {
            Message = Resources.Resource.wfd2da_TB_D_M_EMP_DAY_DUTY_Already;
            return false;
        }
        else
            return true;
    }

    //新增班別主檔,班別明細檔
    public bool ActionAddData(WFB2DA0200DAO dao, out string Message)
    {
        Message = string.Empty;
        bool returnValue = true;
        try
        {
            this.BeginTransaction();
            dl.InsertTB_D_M_SHIFT_H(dao, true);
            foreach (WFB2DA0200DtlDAO dtl in dao.Dtl)
            {
                dl.InsertTB_D_M_SHIFT_D(dtl, true);
            }

            //

            //dl.UpdateDUTY_CHECK_RESULT(dao, true);

            /*
            List<WFB2DA0200EMP_DAY_DUTY_DAO> EmpDayDutyDaos = dl.GetTB_D_M_EMP_DAY_DUTY(dao, true);
            dl.ReOpenEMP_DAY_DUTY(EmpDayDutyDaos, true);
            */
            returnValue = true;
            this.Commit();

            return returnValue;
        }
        catch (Exception ex)
        {
            this.RollBack();
            Message = ex.Message;
            return false;
        }
    }

    public bool ActionAddDataByEMP_DAY_DUTY_Already(WFB2DA0200DAO dao, out string Message)
    {
        //目前需求與ActionAddDataBySHIFT_Already同樣做法，先抽出來以後若有不依樣做法可以改這
        return ActionAddDataBySHIFT_Already(dao, out Message);
    }

    public bool ActionAddDataByEMP_DAY_DUTY_And_SHIFT_Already(WFB2DA0200DAO dao, out string Message)
    {
        //目前需求與ActionAddDataBySHIFT_Already同樣做法，先抽出來以後若有不依樣做法可以改這
        return ActionAddDataBySHIFT_Already(dao, out Message);
    }



    //新增班別主檔-日勤務班表資料檔 及  檢查班別主檔
    public bool ActionAddDataBySHIFT_Already(WFB2DA0200DAO dao, out string Message)
    {
        Message = string.Empty;
        bool returnValue = true;
        try
        {
            this.BeginTransaction();
            //更新 重覆的結束日期為前一天
            dl.UpdateTB_D_M_SHIFT_H_END_DT(dao, true);//與 ActionAddData的差別

            dl.InsertTB_D_M_SHIFT_H(dao, true);

            foreach (WFB2DA0200DtlDAO dtl in dao.Dtl)
            {
                dl.InsertTB_D_M_SHIFT_D(dtl, true);
            }
            
            //dl.UpdateDUTY_CHECK_RESULT(dao, true);
            
            /*
            List<WFB2DA0200EMP_DAY_DUTY_DAO> EmpDayDutyDaos = dl.GetTB_D_M_EMP_DAY_DUTY(dao, true);
            dl.ReOpenEMP_DAY_DUTY(EmpDayDutyDaos, true);
            */ 
            returnValue = true;

            this.Commit();
            return returnValue;
        }
        catch (Exception ex)
        {
            this.RollBack();
            Message = ex.Message;
            return false;
        }
    }

    public bool ActionUpdateData(WFB2DA0200DAO dao, out string Message)
    {
        bool processState = true;
        this.BeginTransaction();
        Message = string.Empty;
        try
        {
            dl.DeleteTB_D_M_SHIFT_DByH(dao, true);
            dl.UpdateTB_D_M_SHIFT_H(dao, true);

            foreach (WFB2DA0200DtlDAO dtl in dao.Dtl)
            {
                dl.InsertTB_D_M_SHIFT_D(dtl, true);
            }

            this.Commit();
            return processState;
        }
        catch (Exception ex)
        {
            this.RollBack();
            Message = ex.Message;
            return false;
        }
    }

    public WFB2DA0200DAO GetSinglSHIFT_Data(WFB2DA0200DAO dao)
    {
        return dl.GetSinglSHIFT_Data(dao, false);
    }

    public WFB2DA0200DAO GetAddSHIFT_D_Data(string shift_cd)
    {
        return dl.GetAddSHIFT_D_Data(shift_cd);
    }

    public int CheckTB_D_M_EMP_DAY_DUTY(WFB2DA0200DAO dao)
    {
        return dl.CheckTB_D_M_EMP_DAY_DUTY(dao, false);
    }

    public int CheckTB_D_M_WORK_SHIFT_DUnValid(WFB2DA0200DAO dao)
    {
        return dl.CheckTB_D_M_WORK_SHIFT_DUnValid(dao, false);
    }

    public bool UpdateTB_D_M_SHIFT_HByUnValid(WFB2DA0200DAO dao)
    {
        return dl.UpdateTB_D_M_SHIFT_HByUnValid(dao, false);
    }

    public bool DeleteItem(List<WFB2DA0200DAO> daos, out string Message)
    {
        try
        {
            this.BeginTransaction();
            bool ProcessState = true;
            Message = string.Empty;
            foreach (WFB2DA0200DAO dao in daos)
            {
                WFB2DA0200DAO DelData = dl.GetSinglSHIFT_Data(dao, true);
                if (dl.CheckTB_D_M_WORK_SHIFT_DByDel(dao, true) > 0)
                {
                    ProcessState = false;
                    Message = Resources.Resource.wfb2da_WFB2DA0200TB_D_M_SHIFT_D_Already;
                    break;
                }
                else
                {
                    if (dl.CheckTB_D_M_EMP_DAY_DUTY_ByDel(dao, true) > 0)
                    {
                        ProcessState = false;
                        Message = Resources.Resource.wfb2da_WFB2DA0200TB_D_M_EMP_DAY_DUTY_Already;
                        break;
                    }
                    else
                    {
                       dl.DeleteTB_D_M_SHIFT_H(dao, true);
                       dl.DeleteTB_D_M_SHIFT_DByH(dao, true);
                       ProcessState = true;
                    }
                }
            }

            if (ProcessState)
            {
                this.Commit();
                return true;
            }
            else
            {
                this.RollBack();
                return false;
            }
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            this.RollBack();
            return false;
        }
    }

    public DataTable get_R_SHIFT_CD_Data()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = dl.get_R_SHIFT_CD_Data();

            return dt;
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    public DateTime FN_S_DUTY_EDT(string p)
    {
        try
        {
            return dl.FN_S_DUTY_EDT(p);
        }
        catch (Exception)
        {
            
            throw;
        }
    }
    public DateTime FN_D_DUTY_CLOSE_DT(string p)
    {
        try
        {
            return dl.FN_D_DUTY_CLOSE_DT(p);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string get_SHIFT_CD_Data(string SHIFT_CD)
    {
        try
        {
            string msg = "0";
            int i = 0;
            DataTable dt = new DataTable();
            dt = dl.get_SHIFT_CD_Data(SHIFT_CD);
            if (dt.Rows.Count > 0)
            {
                i = Convert.ToInt32(dt.Rows[0]["ct"]);
            }
            if (i > 0)
            {
                msg = "班別代碼不可存在於班別主檔(不論有無失效)";
            }
            return msg;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string normally_insert(WFB2DA0200DAO dao)
    {
        string msg = "0"; 
        
        try
        {
            this.BeginTransaction();            

            dl.InsertTB_D_M_SHIFT_H(dao, true);

            foreach (WFB2DA0200DtlDAO dtl in dao.Dtl)
            {
                dl.InsertTB_D_M_SHIFT_D(dtl, true);
            }            

            this.Commit();
            return msg;
        }
        catch (Exception ex)
        {
            this.RollBack();

            return ex.Message;
        }
    }

    //取代
    public string replace_insert(WFB2DA0200DAO dao)
    {
        string msg = "0";

        try
        {
            this.BeginTransaction();

            dl.UpdateTB_D_M_SHIFT_H_END_DT_BY_RSHIFT(dao, true);//被取代的班別代碼

            dl.InsertTB_D_M_SHIFT_H(dao, true);

            foreach (WFB2DA0200DtlDAO dtl in dao.Dtl)
            {
                dl.InsertTB_D_M_SHIFT_D(dtl, true);
            }
            //3.修改 輪值表的班別代碼
            dl.Update_TB_D_M_WORK_SHIFT_D(dao);

            //4.修改 TB_D_M_EMP_DAY_DUTY 日勤務班表資料檔
            dl.Update_TB_D_M_EMP_DAY_DUTY(dao);

            //5.日勤務狀態檔reopen
            dl.Update_TB_D_M_EMP_DUTY_CHECK_STATUS(dao);


            this.Commit();
            return msg;
        }
        catch (Exception ex)
        {
            this.RollBack();

            return ex.Message;
        }
    }

}