using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

/// <summary>
/// CFB2DH0400BO 的摘要描述
/// </summary>
public class CFB2DH0400BO : BaseService
{
    public List<CFB2DH0400DAO> listDH040DAO_class = new List<CFB2DH0400DAO>();
    public CFB2DH0400DAO fb2dh040DAO_class = new CFB2DH0400DAO();

    public CFB2DH0400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getSubLeaveCD()
    {
        CFB2DH0400DAO wfb2dh = new CFB2DH0400DAO();
        try
        {
            return wfb2dh.getSUB_LEAVE_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getCheckStatus(string p)
    {
        CFB2DH0400DAO wfb2dh = new CFB2DH0400DAO();
        try
        {
            return wfb2dh.getCHECK_STATUS(p);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string delete_LeaveData(List<Tuple<string, string, string, string, string, string>> editindex)
    {
        CFB2DH0400DAO fb2dh040 = new CFB2DH0400DAO();
        string rtnmessage = "";
        try
        {
            foreach (var item in editindex)
            {
                //檢查是否已計薪且發薪日期不為空白
                DataTable dt = fb2dh040.getSalaryStatus(item.Item1, item.Item2);
                if (dt.Rows.Count > 0 && item.Item6 != "X0")
                {
                    rtnmessage += "申請單號" + item.Item2 + "已計薪 且 發薪日期不為空白,不可刪除";
                }
            }

            //檢查OK逐筆刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in editindex)
                    {
                        fb2dh040.deleteLeaveData(item.Item1, item.Item2);

                        fb2dh040.deleteLeaveDayData(item.Item1, item.Item2);

                        fb2dh040.APPLY_LEAVE_SDT = item.Item3;
                        fb2dh040.APPLY_LEAVE_EDT = item.Item4;
                        fb2dh040.APPLY_OVERTIME_DT = item.Item5;   
                        fb2dh040.UPDATED_BY = SessionHandle.Current.emp_id;
                        fb2dh040.FUNC_ID = "FB2DH040";

                        //3.日勤務狀態reopen
                        fb2dh040.update_TB_D_M_EMP_DUTY_CHECK_STATUS();
                    }


                    Commit();

                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }

                
                //啟動重新刷卡比對
                foreach (var item in editindex)
                {
                    //註銷假日換休加班分配單
                    fb2dh040.EMP_ID = item.Item1;
                    fb2dh040.IFLOW_NO = item.Item2;
                    fb2dh040.APPLY_LEAVE_STIME = item.Item3;
                    fb2dh040.APPLY_LEAVE_ETIME = item.Item4;
                    fb2dh040.APPLY_OVERTIME_DT = item.Item5;
                    fb2dh040.SUB_LEAVE_CD = item.Item6;        //子假別
                    fb2dh040.UPDATED_BY = SessionHandle.Current.emp_id;
                    fb2dh040.FUNC_ID = "FB2DH040";
                    if (item.Item6 == "X0")
                    {
                        fb2dh040.SP_D_X0_MAPPING("D");
                    }
                    
                    fb2dh040.EMP_ID = item.Item1;
                    fb2dh040.APPLY_LEAVE_SDT = item.Item3;
                    fb2dh040.APPLY_LEAVE_EDT = item.Item4;                    
                    DataTable dt = fb2dh040.getEMP_DAY_DUTY();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        fb2dh040.SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN(item.Item1, dt.Rows[i]["CALENDAR_DT"].ToString());
                    }
                }

                return "0";
            }
            else
                return rtnmessage;

        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
  
    public DataTable getTIMEUNIT(string leave_cd)
    {
        CFB2DH0400DAO wfb2dh = new CFB2DH0400DAO();
        try
        {
            return wfb2dh.getTIMEUNIT(leave_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getIS_DUTY_CHECK(string emp_id)
    {
        CFB2DH0400DAO wfb2dh = new CFB2DH0400DAO();
        try
        {
            return wfb2dh.getIS_DUTY_CHECK(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSubLeaveCD(string MAIN_LEAVE_CD)
    {
        CFB2DH0400DAO wfb2dh = new CFB2DH0400DAO();
        try
        {
            return wfb2dh.getSUB_LEAVE_CD(MAIN_LEAVE_CD);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增畫面-新增請假檔
    public string addLEAVE(CFB2DH0400DAO dh040DAO)
    {
        try
        {
            List<string> duty_days = new List<string>();
            DataTable dt = null;
            dt = dh040DAO.getEMP_DATA();
            if (dt.Rows.Count > 0)
            {
                dh040DAO.DEPT_NO = dt.Rows[0]["DEPT_NO"].ToString();
                dh040DAO.EMP_CD = dt.Rows[0]["EMP_CD"].ToString();
                dh040DAO.UNION_PJOB_CD = dt.Rows[0]["UNION_PJOB_CD"].ToString();
                dh040DAO.LEVEL_CD = dt.Rows[0]["LEVEL_CD"].ToString();
            }
            //20191112先取得IFLOWNO
            dt = dh040DAO.getIFLOW_NO();
            if (dt.Rows.Count > 0)
            {
                dh040DAO.IFLOW_NO = dt.Rows[0]["IFLOW_NO"].ToString();
            }


            try
            {

              
                BeginTransaction();

                dt = dh040DAO.getEMP_DAY_DUTY();
                if (dt.Rows.Count > 0)
                {
                    //明細資料
                    string START_DATE_TIME = "";
                    string END_DATE_TIME = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        duty_days.Add(dt.Rows[i]["CALENDAR_DT"].ToString());
                        dh040DAO.SHIFT_CD = dt.Rows[i]["SHIFT_CD"].ToString();
                        if (dt.Rows[i]["CALENDAR_DT"].ToString() == dh040DAO.APPLY_LEAVE_SDT)
                        {
                            START_DATE_TIME = dh040DAO.APPLY_LEAVE_STIME;
                        }
                        else if (dt.Rows[i]["CALENDAR_DT"].ToString() == dh040DAO.APPLY_LEAVE_EDT)
                        {
                            END_DATE_TIME = dh040DAO.APPLY_LEAVE_ETIME;
                        }

                        //20150804 若請假起迄是同一天
                        if (dh040DAO.APPLY_LEAVE_SDT == dh040DAO.APPLY_LEAVE_EDT) {
                            END_DATE_TIME = dh040DAO.APPLY_LEAVE_ETIME;
                        }

                        if (START_DATE_TIME == "")
                        {
                            START_DATE_TIME = dt.Rows[i]["DUTY_STIME"].ToString();
                        }
                        if (END_DATE_TIME == "")
                        {
                            END_DATE_TIME = dt.Rows[i]["DUTY_ETIME"].ToString();
                        }

                        TimeSpan tmp = DateTime.Parse(END_DATE_TIME) - DateTime.Parse(START_DATE_TIME);
                        double min = tmp.TotalMinutes;
                        double totalMinus = 0;
                        //取得每日休息時間
                        DataTable GET_SHIFT = dh040DAO.getD_GET_SHIFT(dh040DAO.EMP_ID, DateTime.Parse(START_DATE_TIME).ToString("yyyy/MM/dd"));
                        for (int shiftRow = 0; shiftRow < GET_SHIFT.Rows.Count; shiftRow++)
                        {
                            DateTime tmpStartTime = DateTime.Parse(GET_SHIFT.Rows[shiftRow]["START_TIME"].ToString());
                            DateTime tmpEndTime = DateTime.Parse(GET_SHIFT.Rows[shiftRow]["END_TIME"].ToString());
                            //起訖大於該休息時段
                            if (DateTime.Parse(START_DATE_TIME) < tmpStartTime && DateTime.Parse(END_DATE_TIME) > tmpEndTime)
                            {
                                totalMinus += (tmpEndTime - tmpStartTime).TotalMinutes;
                            }
                        }
                        min = tmp.TotalMinutes - totalMinus;
                        if (dh040DAO.IS_INCLUDE_HOLIDAY == "N" && dt.Rows[i]["WORK_DAY_CD"].ToString() == "2")
                        {
                            //do nothing
                        }
                        else
                        {
                            //新增-請假資料日檔
                            dh040DAO.addLEAVE_DAY(dt.Rows[i]["CALENDAR_DT"].ToString(), START_DATE_TIME, END_DATE_TIME, min);                            
                        }

                        START_DATE_TIME = "";
                        END_DATE_TIME = "";

                    }
                }
                //新增請假資料檔
                dh040DAO.addLEAVE();
                //3.日勤務狀態reopen
                dh040DAO.update_TB_D_M_EMP_DUTY_CHECK_STATUS();
                
                Commit();

                //20190812 先執行分配作業,若失敗回傳錯誤訊息
                if (dh040DAO.SUB_LEAVE_CD == "X0")
                {
                    string errMsg = "";
                    errMsg = dh040DAO.SP_D_X0_MAPPING("U");  //U.不存在新增,存在修改
                    string rtn_flag = errMsg.Split(';')[0];
                    string rtn_msg = errMsg.Split(';')[1];

                    if (rtn_flag != "Y")
                        return errMsg;
                }

            }
            catch (Exception ex)
            {
                RollBack();
                return ex.Message;
            }

            //啟動刷卡比對
            //啟動重新刷卡比對
            foreach (var item in duty_days)
            {
                dh040DAO.SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN(dh040DAO.EMP_ID, item);
            }

            return "0";


        }
        catch (Exception ex)
        {

            return ex.Message;
        }
    }


    //假日換休註銷,修改檢核 檢核
    public string checkX0_Valid(List<Tuple<string, string, string, string, string, string>> leave_apply)
    {
        try
        {
            CFB2DH0400DAO dh040DAO = new CFB2DH0400DAO();
            string errMsg = "";
            string rtnMsg = "";
            foreach (var item in leave_apply)
            {
                //假日換休才檢查
                if (item.Item6 != "X0")
                    continue;

                dh040DAO.EMP_ID = item.Item1;
                dh040DAO.IFLOW_NO = item.Item2;
                dh040DAO.APPLY_LEAVE_SDT = item.Item3;
                dh040DAO.APPLY_LEAVE_EDT = item.Item4;
                dh040DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                dh040DAO.FUNC_ID = "FB2DH040";
                errMsg = dh040DAO.SP_DH_LEAVE_DELUPD_CHK_X0();
                if (errMsg != "")
                {
                    rtnMsg += dh040DAO.EMP_ID + " " + dh040DAO.APPLY_LEAVE_SDT + " " + errMsg + ";\\n";
                }
            }

            return rtnMsg;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }


    public string checkValid(CFB2DH0400DAO dh040DAO, string emp_id = "", bool checkDup = true)
    {
        try
        {
            string errMsg = "";
            errMsg = dh040DAO.SP_DH_LEAVE_CHK();
            return errMsg;          
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public DataTable getData(string emp_id, string iflow_no)
    {
        try
        {
            CFB2DH0400DAO fb2dh040 = new CFB2DH0400DAO();
            return fb2dh040.getData(emp_id, iflow_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //修改增畫面-修改請假檔
    public string updateLEAVE(CFB2DH0400DAO fb2dh040)
    {
        try
        {
            if (fb2dh040.SUB_LEAVE_CD == "X0")
            {
                //執行修改 假日換休請假單分配
                string errMsg = "";
                errMsg = fb2dh040.SP_D_X0_MAPPING("U");  //U.不存在新增,存在修改
                string rtn_flag = errMsg.Split(';')[0];
                string rtn_msg = errMsg.Split(';')[1];

                if (rtn_flag != "Y")
                    return errMsg;
            }
           


            BeginTransaction();
            //修改畫面-更新 請假資料檔
            fb2dh040.updateLEAVE();

            //刪除舊的明細資料
            fb2dh040.deleteLEAVE_DAY();

            string START_DATE_TIME = "";
            string END_DATE_TIME = "";
            List<string> duty_days = new List<string>();


            DataTable dt = fb2dh040.getEMP_DAY_DUTY();

            //以下欄位與請假資料檔相同
            DataTable dt_main = fb2dh040.getUpdateMainData();
            if (dt_main.Rows.Count > 0)
            {
                fb2dh040.IS_CONFIRM_CHECK = dt_main.Rows[0]["IS_CONFIRM_CHECK"].ToString();  //確認刷卡比對
                fb2dh040.CHECK_STATUS = dt_main.Rows[0]["CHECK_STATUS"].ToString();  //刷卡比對狀態
                fb2dh040.IS_CONFIRM_CLOSE = dt_main.Rows[0]["IS_CONFIRM_CLOSE"].ToString();  //確認勤務月結
                fb2dh040.SALARY_SETTLE_STATUS = dt_main.Rows[0]["SALARY_SETTLE_STATUS"].ToString();  //計薪狀態
                fb2dh040.FORM_STATUS = dt_main.Rows[0]["FORM_STATUS"].ToString();//表單狀態
                fb2dh040.DEPT_NO = dt_main.Rows[0]["DEPT_NO"].ToString();  //部門代號
                fb2dh040.EMP_CD = dt_main.Rows[0]["EMP_CD"].ToString();                //員工區分
                fb2dh040.UNION_PJOB_CD = dt_main.Rows[0]["UNION_PJOB_CD"].ToString();  // UNION_PJOB_CD 工會職務代碼
                fb2dh040.LEVEL_CD = dt_main.Rows[0]["LEVEL_CD"].ToString();// LEVEL_CD
                fb2dh040.SHIFT_CD = dt_main.Rows[0]["SHIFT_CD"].ToString();//SHIFT_CD	班別代碼
            }
            //明細資料
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                duty_days.Add(dt.Rows[i]["CALENDAR_DT"].ToString());
                if (dt.Rows[i]["CALENDAR_DT"].ToString() == fb2dh040.APPLY_LEAVE_SDT)
                {
                    START_DATE_TIME = fb2dh040.APPLY_LEAVE_STIME;
                }
                else if (dt.Rows[i]["CALENDAR_DT"].ToString() == fb2dh040.APPLY_LEAVE_EDT)
                {
                    END_DATE_TIME = fb2dh040.APPLY_LEAVE_ETIME;
                }

                //20150804 若請假起迄是同一天
                if (fb2dh040.APPLY_LEAVE_SDT == fb2dh040.APPLY_LEAVE_EDT)
                {
                    END_DATE_TIME = fb2dh040.APPLY_LEAVE_ETIME;
                }

                if (START_DATE_TIME == "")
                {
                    START_DATE_TIME = dt.Rows[i]["DUTY_STIME"].ToString();
                }
                if (END_DATE_TIME == "")
                {
                    END_DATE_TIME = dt.Rows[i]["DUTY_ETIME"].ToString();
                }
                TimeSpan tmp = DateTime.Parse(END_DATE_TIME) - DateTime.Parse(START_DATE_TIME);
                double min = tmp.TotalMinutes;
                double totalMinus = 0;
                //取得每日休息時間
                DataTable GET_SHIFT = fb2dh040.getD_GET_SHIFT(fb2dh040.EMP_ID, DateTime.Parse(START_DATE_TIME).ToString("yyyy/MM/dd"));
                for (int shiftRow = 0; shiftRow < GET_SHIFT.Rows.Count; shiftRow++)
                {
                    DateTime tmpStartTime = DateTime.Parse(GET_SHIFT.Rows[shiftRow]["START_TIME"].ToString());
                    DateTime tmpEndTime = DateTime.Parse(GET_SHIFT.Rows[shiftRow]["END_TIME"].ToString());
                    //起訖大於該休息時段
                    if (DateTime.Parse(START_DATE_TIME) < tmpStartTime && DateTime.Parse(END_DATE_TIME) > tmpEndTime)
                    {
                        totalMinus += (tmpEndTime - tmpStartTime).TotalMinutes;
                    }
                }
                min = tmp.TotalMinutes - totalMinus;
                //修改畫面-新增 請假資料日檔
                fb2dh040.insertLEAVE_DAY(dt.Rows[i]["CALENDAR_DT"].ToString(), START_DATE_TIME, END_DATE_TIME, min);

                //3.日勤務狀態reopen
                fb2dh040.update_TB_D_M_EMP_DUTY_CHECK_STATUS();

                START_DATE_TIME = "";
                END_DATE_TIME = "";

            }
            Commit();

            //啟動刷卡比對
            //啟動重新刷卡比對
            //fb2dh040.SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN(fb2dh040.EMP_ID, fb2dh040.APPLY_LEAVE_SDT);
            
            foreach (var item in duty_days)
            {
                fb2dh040.SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN(fb2dh040.EMP_ID, item);
            }
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }


    public DataTable getPlantCD()
    {
        CFB2DH0400DAO wfb2dh = new CFB2DH0400DAO();
        try
        {
            return wfb2dh.getPlantCD();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getWsCD()
    {
        CFB2DH0400DAO wfb2dh = new CFB2DH0400DAO();
        try
        {
            return wfb2dh.getWsCD();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getWorkCD()
    {
        CFB2DH0400DAO wfb2dh = new CFB2DH0400DAO();
        try
        {
            return wfb2dh.getWorkCD();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string Confirm_empid(List<Tuple<string, string>> emp_id, CFB2DH0400DAO fb2di040)
    {
        CFB2DH0400DAO fb2dh040 = new CFB2DH0400DAO();

        string rtnmessage = "";
        //檢查OK逐筆修改
        if (rtnmessage == "")
        {
            try
            {
                BeginTransaction();
                foreach (var item in emp_id)
                {
                    fb2dh040.Confirm_empid(item.Item1, item.Item2, fb2di040.IS_CONFIRM_CHECK);
                    fb2dh040.Confirm_empid_day(item.Item1, item.Item2, fb2di040.IS_CONFIRM_CHECK);

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
        else
            return rtnmessage;
    }

    //一括請假
    public string execSP_D_LEAVE_BATCH(CFB2DH0400DAO fb2dh040, List<string> emp_data)
    {

        string rtnmessage = "";//檢查後的訊息
        string emp_long = "";
        try
        {
            //將工號轉成字串
            foreach (var emp_id in emp_data)
            {
                emp_long += emp_id + ",";
            }
            emp_long = emp_long.Substring(0, emp_long.Length - 1);

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                fb2dh040.execSP_D_LEAVE_BATCH(emp_long);
                rtnmessage += utilities.getSPLOG("SP_D_LEAVE_BATCH");
                if (rtnmessage != "")
                {
                    return rtnmessage;
                }

                return "0";
            }
            else
            {
                return rtnmessage;
            }

        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string addBatchLEAVE_back(CFB2DH0400DAO fb2dh040, List<string> emp_data)
    {
        try
        {
            //List<CFB2DH0400DAO> listfb2dh040 = new List<CFB2DH0400DAO>();
            listDH040DAO_class = new List<CFB2DH0400DAO>();
            string errMsg = "";
            string cur_apply_leave_stime = "";
            string cur_apply_leave_etime = "";
            cur_apply_leave_stime = fb2dh040.APPLY_LEAVE_STIME;
            cur_apply_leave_etime = fb2dh040.APPLY_LEAVE_ETIME;

            string emp_long = "";
            foreach (var emp_id in emp_data)
            {
                emp_long += emp_id + ",";
            }
            emp_long = emp_long.Substring(0, emp_long.Length - 1);


            CFB2DH0400DAO item = new CFB2DH0400DAO();
            foreach (var emp_id in emp_data)
            {
                item = new CFB2DH0400DAO();
                item.EMP_ID = emp_id;
                item.SUB_LEAVE_CD = fb2dh040.SUB_LEAVE_CD;
                item.APPLY_LEAVE_SDT = fb2dh040.APPLY_LEAVE_SDT;
                item.TOTAL_TIME_APPROVE = fb2dh040.TOTAL_TIME_APPROVE;
                item.BatchStatus = "";
                if (fb2dh040.SUB_LEAVE_CD == "D2" || fb2dh040.SUB_LEAVE_CD == "10" || fb2dh040.SUB_LEAVE_CD == "20")
                    item.getBatchStatus1();
                if (fb2dh040.SUB_LEAVE_CD == "S0")
                    errMsg += item.getBatchStatus2();
                if (item.BatchStatus == "X")
                {
                    //若一括申請控管檔中設定為"X",者, 應檢核是否重覆申請
                    DataTable dupData = fb2dh040.getDupData(item.EMP_ID);
                    if (dupData.Rows.Count > 0)
                        errMsg += emp_id + " 該工號請假日期時間已存在，不可重複申請 ! \\n";
                }
                DataTable empData = item.getEMP_DATA();
                if (empData.Rows.Count > 0)
                {
                    item.DEPT_NO = empData.Rows[0]["DEPT_NO"].ToString();
                    item.EMP_CD = empData.Rows[0]["EMP_CD"].ToString();
                    item.UNION_PJOB_CD = empData.Rows[0]["UNION_PJOB_CD"].ToString();
                    item.LEVEL_CD = empData.Rows[0]["LEVEL_CD"].ToString();
                }
                listDH040DAO_class.Add(item);

            }
            //errMsg += "test";

            if (errMsg != "")
            {
                return errMsg;
            }

            BeginTransaction();
            if (errMsg == "")
            {
                DataTable dt = new DataTable();
                foreach (CFB2DH0400DAO emp in listDH040DAO_class)
                {
                    //若設定為"N"者則不處理(不存檔)!! 
                    if (emp.BatchStatus != "N")
                    {
                        bool canLeave = false;
                        //fb2dh040.EMP_ID = emp_id;
                        fb2dh040.EMP_ID = emp.EMP_ID;
                        fb2dh040.old_TOTAL_TIME_APPROVE = emp.old_TOTAL_TIME_APPROVE;
                        fb2dh040.old_IFLOW_NO = emp.old_IFLOW_NO;
                        fb2dh040.BatchStatus = emp.BatchStatus;
                        fb2dh040.DEPT_NO = emp.DEPT_NO;
                        fb2dh040.EMP_CD = emp.EMP_CD;
                        fb2dh040.UNION_PJOB_CD = emp.EMP_CD;
                        fb2dh040.LEVEL_CD = emp.LEVEL_CD;
                        dt = fb2dh040.getEMP_DAY_DUTY();

                        //明細資料
                        string START_DATE_TIME = cur_apply_leave_stime;
                        string END_DATE_TIME = cur_apply_leave_etime;
                        double totalMin = 0;
                        double min = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (fb2dh040.IS_ALL_DAY)
                            {
                                min = 480;
                                totalMin += min;
                                canLeave = true;
                            }
                            else if (dt.Rows[i]["WORK_DAY_CD"].ToString() == "1" &&
                                (DateTime.Parse(dt.Rows[i]["DUTY_STIME"].ToString()) <= DateTime.Parse(cur_apply_leave_etime)))
                            {
                                canLeave = true;

                                fb2dh040.SHIFT_CD = dt.Rows[i]["SHIFT_CD"].ToString();
                                if (DateTime.Parse(dt.Rows[i]["DUTY_STIME"].ToString()) >= DateTime.Parse(cur_apply_leave_stime))
                                {
                                    START_DATE_TIME = dt.Rows[i]["DUTY_STIME"].ToString();
                                }
                                else
                                {
                                    START_DATE_TIME = cur_apply_leave_stime;
                                }
                                if (DateTime.Parse(dt.Rows[i]["DUTY_ETIME"].ToString()) >= DateTime.Parse(cur_apply_leave_etime))
                                {
                                    END_DATE_TIME = cur_apply_leave_etime;
                                }
                                else
                                {
                                    END_DATE_TIME = dt.Rows[i]["DUTY_ETIME"].ToString();
                                }

                                TimeSpan tmp = DateTime.Parse(END_DATE_TIME) - DateTime.Parse(START_DATE_TIME);
                                min = tmp.TotalMinutes;
                                double totalMinus = 0;
                                //取得每日休息時間
                                DataTable GET_SHIFT = fb2dh040.getD_GET_SHIFT(fb2dh040.EMP_ID, DateTime.Parse(START_DATE_TIME).ToString("yyyy/MM/dd"));
                                for (int shiftRow = 0; shiftRow < GET_SHIFT.Rows.Count; shiftRow++)
                                {
                                    DateTime tmpStartTime = DateTime.Parse(GET_SHIFT.Rows[shiftRow]["START_TIME"].ToString());
                                    DateTime tmpEndTime = DateTime.Parse(GET_SHIFT.Rows[shiftRow]["END_TIME"].ToString());
                                    //起迄大於該休息時段
                                    if (DateTime.Parse(START_DATE_TIME) < tmpStartTime && DateTime.Parse(END_DATE_TIME) > tmpEndTime)
                                    {
                                        totalMinus += (tmpEndTime - tmpStartTime).TotalMinutes;

                                    }
                                    if (tmpStartTime < DateTime.Parse(START_DATE_TIME) && DateTime.Parse(START_DATE_TIME) < tmpEndTime && DateTime.Parse(END_DATE_TIME) > tmpEndTime)
                                    {
                                        totalMinus += (tmpEndTime - DateTime.Parse(START_DATE_TIME)).TotalMinutes;
                                    }
                                    if (DateTime.Parse(START_DATE_TIME) < tmpStartTime && tmpStartTime < DateTime.Parse(END_DATE_TIME) && tmpEndTime > DateTime.Parse(END_DATE_TIME))
                                    {
                                        totalMinus += (DateTime.Parse(END_DATE_TIME) - tmpEndTime).TotalMinutes;
                                    }
                                }
                                min = tmp.TotalMinutes - totalMinus;
                                totalMin += min;
                            }

                            if (totalMin > 480)
                                totalMin = 480;
                            if (canLeave)
                            {
                                if (fb2dh040.SUB_LEAVE_CD == "D2" || fb2dh040.SUB_LEAVE_CD == "10" || fb2dh040.SUB_LEAVE_CD == "20")
                                {
                                    if (fb2dh040.BatchStatus == "1")
                                    {
                                        //原假別取消：原假別假單取消(表單狀態註記為"N")；另新增建立公司停工假單(=停工或特休休假時數)
                                        fb2dh040.cancelOld();

                                    }
                                    if (fb2dh040.BatchStatus == "2")
                                    {
                                        //原假別不變, 另建立停工假單：原假別保留不變；另新增建立公司停工假單(=停工或特休休假時數)

                                    }
                                    if (fb2dh040.BatchStatus == "3")
                                    {
                                        //扣臨時停工時數：(原假別請假時數－臨時停工時數)，若不足最小單位則進位至最小單位值，且另新增建立公司停工假單(=停工或特休休假時數)
                                        fb2dh040.updateOld();

                                    }
                                    if (fb2dh040.BatchStatus == "4")
                                    {
                                        //依原假別時數判別：若 原假別請假時數＜公司休假時數時，原假別假單取消(表單狀態註記為"N")，且另新增建立公司休假別假單
                                        if (int.Parse(fb2dh040.old_TOTAL_TIME_APPROVE) < int.Parse(fb2dh040.TOTAL_TIME_APPROVE))
                                            fb2dh040.cancelOld();
                                    }
                                    //新增 請假資料日檔
                                    if (fb2dh040.BatchStatus != "5")
                                    {
                                        fb2dh040.addLEAVE_DAY(dt.Rows[i]["CALENDAR_DT"].ToString(), START_DATE_TIME, END_DATE_TIME, totalMin);
                                    }
                                }
                                else if (fb2dh040.SUB_LEAVE_CD == "S0")
                                {
                                    if (int.Parse(fb2dh040.TOTAL_TIME_APPROVE) == 480)
                                    {
                                        if (fb2dh040.BatchStatus == "1")
                                        {
                                            //原假別取消：原假別假單取消(表單狀態註記為"N")；另新增建立公司停工假單(=臨時停工時數) (表單狀態註記為"Y")
                                            fb2dh040.cancelOld();
                                        }
                                        if (fb2dh040.BatchStatus == "2")
                                        {
                                            //原假別不變, 另建立停工假單：原假別保留不變；另建立公司停工假單(=臨時停工時數)
                                        }
                                        if (fb2dh040.BatchStatus == "3")
                                        {
                                            //扣臨時停工時數：原假別請假時數=(原假別請假時數－臨時停工時數)，若不足最小單位則進位至最小單位值，且另建立公司停工假單(=臨時停工時數) (表單狀態註記為"Y")
                                            fb2dh040.updateOld();
                                        }
                                        if (fb2dh040.BatchStatus == "4")
                                        {
                                            //依原假別時數判別：若 原假別請假時數＜公司休假時數時，原假別假單取消(表單狀態註記為"N")，且另建立公司休假別假單
                                            if (int.Parse(fb2dh040.old_TOTAL_TIME_APPROVE) < int.Parse(fb2dh040.TOTAL_TIME_APPROVE))
                                                fb2dh040.cancelOld();
                                        }
                                        //新增 請假資料日檔
                                        if (fb2dh040.BatchStatus != "5")
                                        {
                                            fb2dh040.addLEAVE_DAY(dt.Rows[i]["CALENDAR_DT"].ToString(), START_DATE_TIME, END_DATE_TIME, totalMin);
                                        }
                                    }
                                    else if (int.Parse(fb2dh040.TOTAL_TIME_APPROVE) < 480)
                                    {
                                        if (fb2dh040.BatchStatus == "1")
                                        {
                                            //原假別取消：原假別假單取消(表單狀態註記為"N")，另建立公司停工假單(=臨時停工時數)(表單狀態註記為"Y")
                                            fb2dh040.cancelOld();
                                        }
                                        if (fb2dh040.BatchStatus == "2")
                                        {
                                            //原假別不變, 另建立停工假單：原假別保留不變；另建立公司停工假單(=臨時停工時數
                                        }
                                        if (fb2dh040.BatchStatus == "3")
                                        {
                                            //扣臨時停工時數：原假別請假時數=(原假別請假時數－臨時停工時數)，若不足最小單位則進位至最小單位值，且另建立公司停工假單(=臨時停工時數)
                                            fb2dh040.updateOld();
                                        }
                                        if (fb2dh040.BatchStatus == "4")
                                        {
                                            //依原假別時數判別：若 原假別請假時數＜公司休假時數時，假單取消(表單狀態註記為"N")，且另建立公司休假別假單
                                            if (int.Parse(fb2dh040.old_TOTAL_TIME_APPROVE) < int.Parse(fb2dh040.TOTAL_TIME_APPROVE))
                                                fb2dh040.cancelOld();
                                        }
                                        if (fb2dh040.BatchStatus != "5")
                                            fb2dh040.addLEAVE_DAY(dt.Rows[i]["CALENDAR_DT"].ToString(), START_DATE_TIME, END_DATE_TIME, totalMin);
                                    }
                                }
                                else
                                {
                                    fb2dh040.addLEAVE_DAY(dt.Rows[i]["CALENDAR_DT"].ToString(), START_DATE_TIME, END_DATE_TIME, totalMin);
                                }
                            }//end of canLeave


                        }
                        //主檔
                        //if (fb2dh040.SUB_LEAVE_CD != "D2" && fb2dh040.SUB_LEAVE_CD != "10" && fb2dh040.SUB_LEAVE_CD != "20" && fb2dh040.SUB_LEAVE_CD != "S0")
                        //{
                        fb2dh040.APPLY_LEAVE_STIME = START_DATE_TIME;
                        fb2dh040.APPLY_LEAVE_ETIME = END_DATE_TIME;
                        if (canLeave)
                        {
                            fb2dh040.TOTAL_TIME_APPROVE = totalMin.ToString();  //請假申請合計
                            fb2dh040.addLEAVE();
                        }
                        START_DATE_TIME = "";
                        END_DATE_TIME = "";
                        //}
                    }
                }
                Commit();
            }
            else
            {
                RollBack();
                return errMsg;
            }
            //啟動刷卡比對
            //啟動重新刷卡比對
            foreach (CFB2DH0400DAO emp in listDH040DAO_class)
            {
                fb2dh040.SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN(emp.EMP_ID, fb2dh040.APPLY_LEAVE_SDT);
            }
            //為產生excel,故要有BO層的listDH040DAO_class,及fb2dh040DAO_class
            fb2dh040DAO_class = fb2dh040;
            //createExcel2(listDH040DAO_class, fb2dh040);
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public IWorkbook createExcel2(CFB2DH0400DAO dh040DAO)
    {
        try
        {
            //判斷是否有資料
            bool hasItem = false;
            DataTable dt = dh040DAO.getLeaveBatchExcelData();
            
            BeginTransaction();
            dh040DAO.dropLeaveBatchTable();
            Commit();
            
            if (dt.Rows.Count > 0)
            {
                hasItem = true;
            }
            else
            {
                return null;
            }


            IWorkbook workbook;
            ISheet sheet;
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            ICellStyle style4;
            workbook = new XSSFWorkbook();
            sheet = workbook.CreateSheet("一括申請請假差異記錄");
            style1 = (XSSFCellStyle)workbook.CreateCellStyle();

            IFont font1 = workbook.CreateFont();
            font1.FontName = "新細明體";
            font1.FontHeightInPoints = 12;

            IFont font2 = workbook.CreateFont();
            font2.FontName = "新細明體";
            font2.FontHeightInPoints = 14;

            //標題 樣式
            style3 = (XSSFCellStyle)workbook.CreateCellStyle();
            style3.SetFont(font2);
            style3.Alignment = HorizontalAlignment.Center;
            style3.VerticalAlignment = VerticalAlignment.Center;

            //grid header 樣式
            style4 = (XSSFCellStyle)workbook.CreateCellStyle();
            ((XSSFCellStyle)style4).SetFillForegroundColor(new XSSFColor(Color.LightGray));
            ((XSSFCellStyle)style4).FillPattern = FillPattern.SolidForeground;
            ((XSSFCellStyle)style4).BorderBottom = BorderStyle.Thin;
            ((XSSFCellStyle)style4).BorderLeft = BorderStyle.Thin;
            ((XSSFCellStyle)style4).BorderRight = BorderStyle.Thin;
            ((XSSFCellStyle)style4).BorderTop = BorderStyle.Thin;
            style4.SetFont(font1);
            style4.Alignment = HorizontalAlignment.Center;
            style4.VerticalAlignment = VerticalAlignment.Center;

            style1.SetFont(font1);

            IRow row = sheet.CreateRow(0);
            ICell cell;
            cell = row.CreateCell(1);
            cell.SetCellValue("一括申請請假差異記錄");
            cell.CellStyle = style3;
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1, 13));

            row = sheet.CreateRow(1);

            cell = row.CreateCell(1);
            cell.CellStyle = style4;
            cell.SetCellValue("序號");

            cell = row.CreateCell(2);
            cell.CellStyle = style4;
            cell.SetCellValue("工號");

            cell = row.CreateCell(3);
            cell.CellStyle = style4;
            cell.SetCellValue("姓名");

            cell = row.CreateCell(4);
            cell.CellStyle = style4;
            cell.SetCellValue("請假日期");

            cell = row.CreateCell(5);
            cell.CellStyle = style4;
            cell.SetCellValue("原假單編號");

            cell = row.CreateCell(6);
            cell.CellStyle = style4;
            cell.SetCellValue("原主假別");

            cell = row.CreateCell(7);
            cell.CellStyle = style4;
            cell.SetCellValue("原子假別");

            cell = row.CreateCell(8);
            cell.CellStyle = style4;
            cell.SetCellValue("原假單時數");

            cell = row.CreateCell(9);
            cell.CellStyle = style4;
            cell.SetCellValue("新主假別");

            cell = row.CreateCell(10);
            cell.CellStyle = style4;
            cell.SetCellValue("新子假別");

            cell = row.CreateCell(11);
            cell.CellStyle = style4;
            cell.SetCellValue("新假單時數");

            cell = row.CreateCell(12);
            cell.CellStyle = style4;
            cell.SetCellValue("建立日期時間");

            cell = row.CreateCell(13);
            cell.CellStyle = style4;
            cell.SetCellValue("建立者");

            int itemrow = 0;

            style2 = workbook.CreateCellStyle();
            ((XSSFCellStyle)style2).BorderBottom = BorderStyle.Thin;
            ((XSSFCellStyle)style2).BorderLeft = BorderStyle.Thin;
            ((XSSFCellStyle)style2).BorderRight = BorderStyle.Thin;
            ((XSSFCellStyle)style2).BorderTop = BorderStyle.Thin;
            style2.SetFont(font1);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                itemrow = i+2;
                row = sheet.CreateRow(itemrow);
                cell = row.CreateCell(1);
                cell.CellStyle = style2;
                cell.SetCellValue(i+1);

                cell = row.CreateCell(2);
                cell.CellStyle = style2;
                cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());

                cell = row.CreateCell(3);
                cell.CellStyle = style2;
                cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());

                cell = row.CreateCell(4);
                cell.CellStyle = style2;
                cell.SetCellValue(Convert.ToDateTime(dt.Rows[i]["APPLY_LEAVE_DT"].ToString()).ToString("yyyy/MM/dd"));

                cell = row.CreateCell(5);
                cell.CellStyle = style2;
                cell.SetCellValue(dt.Rows[i]["IFLOW_NO"].ToString());

                cell = row.CreateCell(6);
                cell.CellStyle = style2;
                cell.SetCellValue(dt.Rows[i]["MAIN_LEAVE_DESC"].ToString());

                cell = row.CreateCell(7);
                cell.CellStyle = style2;
                cell.SetCellValue(dt.Rows[i]["SUB_LEAVE_DESC"].ToString());

                cell = row.CreateCell(8);
                cell.CellStyle = style2;
                cell.SetCellValue(dt.Rows[i]["TOTAL_TIME_APPROVE"].ToString());

                cell = row.CreateCell(9);
                cell.CellStyle = style2;
                cell.SetCellValue(dt.Rows[i]["NEW_MAIN_LEAVE_DESC"].ToString());

                cell = row.CreateCell(10);
                cell.CellStyle = style2;
                cell.SetCellValue(dt.Rows[i]["NEW_SUB_LEAVE_DESC"].ToString());

                cell = row.CreateCell(11);
                cell.CellStyle = style2;
                cell.SetCellValue(dt.Rows[i]["NEW_TOTAL_TIME_APPROVE"].ToString());

                cell = row.CreateCell(12);
                cell.CellStyle = style2;
                cell.SetCellValue(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                cell = row.CreateCell(13);
                cell.CellStyle = style2;
                cell.SetCellValue(SessionHandle.Current.emp_name);

            }

            sheet.AutoSizeColumn(1);
            sheet.AutoSizeColumn(2);
            sheet.AutoSizeColumn(3);
            sheet.AutoSizeColumn(4);
            sheet.AutoSizeColumn(5);
            sheet.AutoSizeColumn(6);
            sheet.AutoSizeColumn(7);
            sheet.AutoSizeColumn(8);
            sheet.AutoSizeColumn(9);
            sheet.AutoSizeColumn(10);
            sheet.AutoSizeColumn(11);
            sheet.AutoSizeColumn(12);
            sheet.AutoSizeColumn(13);
            //ExcelHandle.exportExcel(workbook, "FB2DH040_ERR_1.xlsx");
            return workbook;


        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSalaryStatus(string emp_id, string iflow_no)
    {
        CFB2DH0400DAO wfb2dh = new CFB2DH0400DAO();
        try
        {
            return wfb2dh.getSalaryStatus(emp_id, iflow_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDayDuty(CFB2DH0400DAO dao)
    {

        try
        {
            return dao.getEMP_DAY_DUTY();

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getD_GET_SHIFT(CFB2DH0400DAO dao, string emp_id, string CALENDAR_DT)
    {
        try
        {
            return dao.getD_GET_SHIFT(emp_id, CALENDAR_DT);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEMP_DATA(string emp_id)
    {
        try
        {
            CFB2DH0400DAO dao = new CFB2DH0400DAO();
            return dao.getEMP_DATA(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDEPT_DATA(string dept_no)
    {
        try
        {
            CFB2DH0400DAO dao = new CFB2DH0400DAO();
            return dao.getDEPT_DATA(dept_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getWORK_SHIFT_CD(string WORK_SHIFT_CD)
    {
        try
        {
            CFB2DH0400DAO dao = new CFB2DH0400DAO();
            return dao.getWORK_SHIFT_CD(WORK_SHIFT_CD);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEmpData(string emp_id)
    {
        try
        {
            CFB2DH0400DAO dao = new CFB2DH0400DAO();
            return dao.getEmpData(emp_id);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSHIFT_DATA(string WORK_SHIFT_CD, string APPLY_LEAVE_SDT)
    {
        try
        {
            CFB2DH0400DAO dao = new CFB2DH0400DAO();
            return dao.getSHIFT_DATA(WORK_SHIFT_CD, APPLY_LEAVE_SDT);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getCHECK_STATUS(string check_status)
    {
        CFB2DH0400DAO wfb2dh = new CFB2DH0400DAO();
        try
        {
            return wfb2dh.getCHECK_STATUS2(check_status);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getFORM_STATUS(string form_status)
    {
        CFB2DH0400DAO wfb2dh = new CFB2DH0400DAO();
        try
        {
            return wfb2dh.getFORM_STATUS(form_status);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEMP_ID(string plant_cd, string dept_no, string ws_cd, string work_cd, string work_shift_cd, string start_dt,string shift_cd)
    {
        CFB2DH0400DAO wfb2dh = new CFB2DH0400DAO();
        try
        {
            return wfb2dh.getEMP_ID(plant_cd, dept_no, ws_cd, work_cd, work_shift_cd, start_dt, shift_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }
}