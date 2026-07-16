using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

/// <summary>
/// CFB2DI0500BO 的摘要描述
/// </summary>
public class CFB2DI0500BO : BaseService
{
    public CFB2DI0500BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getOvertimeCD(string p)
    {
        CFB2DI0500DAO wfb2di = new CFB2DI0500DAO();
        try
        {
            return wfb2di.getOvertimeCD(p);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOvertimeTimeCD(string p)
    {
        CFB2DI0500DAO wfb2di = new CFB2DI0500DAO();
        try
        {
            return wfb2di.getOvertimeTimeCD(p);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getCheckStatus(string p)
    {
        CFB2DI0500DAO wfb2di = new CFB2DI0500DAO();
        try
        {
            return wfb2di.getCheckStatus(p);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string addEmpData(CFB2DI0500DAO fb2di050)
    {
        try
        {
            //進行相關檢核

            string errMsg = checkValid(fb2di050);



            if (errMsg == "")
            {

                BeginTransaction();
                try
                {
                    fb2di050.addEmp();

                    Commit();

                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
                //啟動刷卡比對
                //啟動重新刷卡比對

                fb2di050.SP_D_EMP_DUTY_CHECK_STATUS_REOPEN(fb2di050.EMP_ID, fb2di050.APPLY_OVERTIME_DT);


                return "0";
            }
            else
                return errMsg;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string checkValid(CFB2DI0500DAO dao, string emp_id = "")
    {
        try
        {
            string errMsg = "";
            //DataTable dupdata = dao.getExistData();
            //if ((int)dupdata.Rows[0]["empcount"] > 0)
            //    errMsg += "加班日期時間已存在，不可重複申請 ! \\n";
            //DataTable leavedata = dao.getLeaveData();
            //if (leavedata.Rows.Count > 0)
            //    errMsg += "請假日期時間已存在，不可重複申請 ! \\n";

            //判斷申請加班日期：依加班類型+日期類型, 檢核日期是否符合其設定之加班日期類型, 至日勤務班表檔, 取出勤別判別
            DataTable work_day_cd = dao.getWorkDayCd(dao.EMP_ID, dao.APPLY_OVERTIME_DT);
            if (work_day_cd.Rows.Count > 0)
            {
                if (work_day_cd.Rows[0]["WORK_DAY_CD"].ToString() == "1")
                {
                    if (dao.OVERTIME_DT_TYPE.Substring(0, 1) == "2")
                        errMsg += "加班日期類型不符合日勤務班表設定之加班日期類型 \\n";
                }
                if (work_day_cd.Rows[0]["WORK_DAY_CD"].ToString() == "2")
                {
                    if (dao.OVERTIME_DT_TYPE.Substring(0, 1) == "1")
                        errMsg += "加班日期類型不符合日勤務班表設定之加班日期類型 \\n";
                }
            }
            //若加班類型之「換休適用人員」="Y 有限制"，則須參照加班換休適用人員設定檔，檢核人員是否符合。
            if (dao.OVERTIME_ALLOW_CD == "Y" && dao.IS_APPLY == "Y")
            {
                DataTable OVERTIME_ALLOW_DATA = dao.getOVERTIME_ALLOW_DATA();
                if (OVERTIME_ALLOW_DATA.Rows.Count == 0)
                {
                    errMsg += "此人員不符合換休適用人員 \\n";
                }

            }
            if (dao.IFLOW_NO == "" || dao.APPLY_OVERTIME_DT != dao.ORI_OVERTIME_APPLY_DT)
            {
                //檢核加班不可重複申請：有效之加班記錄時段不可重疊、該時段有請假記錄不可加班。
                //檢核畫面輸入之加班單與其他已存在加班單有效加班時段不可重疊  (檢核有效之加班記錄時段不可重疊)
                DataTable dupApplyData = dao.getdupApplyData();
                if (dupApplyData.Rows.Count > 0)
                {
                    if ((int)dupApplyData.Rows[0]["datacount"] > 0)
                    {
                        errMsg += "加班日期時間已存在，不可重複 ! \\n";
                    }
                }

                
            }

            if (dao.OVERTIME_CD == "G")
            {
                if (!dao.getLeaveGI("S0"))
                    errMsg += "未核准臨時停工，不允許申請此類加班 ! \\n";
            }

            if (dao.OVERTIME_CD == "I")
            {
                if (!dao.getLeaveGI("E6"))
                    errMsg += "未核准原住民假，不允許申請此類加班 !\\n";
            }
            ////代休加班(假日), 若當日出勤時段含括用餐時段→扣除 午餐1H和晚餐0.5H
            ////休出加班(假日), 若當日出勤時段含括用餐時段→扣除 午晚餐各0.5H
            //if (dao.OVERTIME_CD == "D")
            //{
            //    dao.APPLY_OVERTIME_HOUR = (int.Parse(dao.APPLY_OVERTIME_HOUR) - 90).ToString();
            //}
            //if (dao.OVERTIME_CD == "C")
            //{
            //    dao.APPLY_OVERTIME_HOUR = (int.Parse(dao.APPLY_OVERTIME_HOUR) - 60).ToString();
            //}

            ////平日加班(STAFF), 若當日出勤時段含括晚餐時段→系統直接0.5H扣除(區間落於PM7:00~7:30), 依日勤務班表之勤後餐時段扣除
            //if (dao.OVERTIME_CD == "A")
            //{
            //    DateTime after_start = DateTime.Parse(dao.AFTER_STIME);
            //    DateTime after_end = DateTime.Parse(dao.AFTER_ETIME);
            //    DateTime compare_start = DateTime.Parse(dao.APPLY_OVERTIME_DT + " " + "19:00:00");
            //    DateTime compare_end = DateTime.Parse(dao.APPLY_OVERTIME_DT + " " + "19:30:00");
            //    if ((after_start >= compare_start && after_end <= compare_end) ||
            //        (after_start <= compare_start && after_end >= compare_start) ||
            //        (after_start >= compare_start && after_end >= compare_end) ||
            //        (after_start <= compare_start && after_end >= compare_end))
            //    {
            //        dao.APPLY_OVERTIME_HOUR = (int.Parse(dao.APPLY_OVERTIME_HOUR) - 30).ToString();
            //    }
            //    //檢核平日加班時數不可大於4Hr/日; 
            //    if (int.Parse(dao.APPLY_OVERTIME_HOUR) > 240)
            //        errMsg += "平日加班時數不可大於4Hr/日";
            //}

            int TotalCompareMinute = 0;
            int new_APPLY_OVERTIME_HOUR = 0;
            //依加班管制區分, 檢核每月加班時數累計上限, 若超過上限則顯示訊息！

            DataTable ctlHour = dao.getCTLHour();
            DataTable ctlHourType1 = dao.getCTLHourType1();
            switch (dao.OVERTIME_CTL_CD == "" ? "1" : dao.OVERTIME_CTL_CD.Substring(0, 1))
            {
                //檢核管制對象為一般人員=1: 每月(平日加班+假日加班超過8Hr後時數) 不可大於46Hr/月; 計算勤前加班時數應扣除早餐用餐時間
                case "1":
                    TotalCompareMinute = 46 * 60;
                    if (int.Parse(dao.APPLY_OVERTIME_HOUR) >= 480)
                        new_APPLY_OVERTIME_HOUR = int.Parse(dao.APPLY_OVERTIME_HOUR) - 480;
                    else
                        new_APPLY_OVERTIME_HOUR = 0;
                    if (ctlHourType1.Rows.Count > 0)
                    {
                        if (int.Parse(ctlHourType1.Rows[0]["ctlsum"].ToString()) + new_APPLY_OVERTIME_HOUR > TotalCompareMinute)
                            errMsg += "管制對象為一般人員，每月(平日加班+假日加班大於8小時的時數)不可大於46Hr/月";
                    }
                    break;
                //檢核管制對象為高危險員工=2: 每月(平日加班+假日加班時數)不可大於37Hr/月; 計算勤前加班時數應扣除早餐用餐時間
                case "2":
                    TotalCompareMinute = 37 * 60;
                    if (ctlHour.Rows.Count > 0)
                    {
                        if (int.Parse(ctlHour.Rows[0]["ctlsum"].ToString()) + int.Parse(dao.APPLY_OVERTIME_HOUR) > TotalCompareMinute)
                            errMsg += "對象為高危險員工，每月(平日加班+假日加班時數)不可大於37Hr/月";
                    }
                    break;
                //檢核管制對象為高齡(60歲以上)員工=3: 每月(平日加班+假日加班時數)不可大於0Hr/月; 計算勤前加班時數應扣除早餐用餐時間
                case "3":
                    TotalCompareMinute = 0;
                    if (int.Parse(dao.APPLY_OVERTIME_HOUR) > 0)
                        errMsg += "管制對象為高齡(60歲以上)員，每月(平日加班+假日加班時數)不可大於0Hr/月";
                    break;
                //檢核管制對象為早餐勤前加班=4: 每月(平日加班+假日加班超過8Hr後時數) 不可大於46Hr/月; 計算勤前加班時數含早餐用餐時間;
                case "4":
                    TotalCompareMinute = 46 * 60;
                    if (int.Parse(dao.APPLY_OVERTIME_HOUR) >= 480)
                        new_APPLY_OVERTIME_HOUR = int.Parse(dao.APPLY_OVERTIME_HOUR) - 480;
                    else
                        new_APPLY_OVERTIME_HOUR = 0;

                    if (ctlHourType1.Rows.Count > 0)
                    {
                        if (int.Parse(ctlHourType1.Rows[0]["ctlsum"].ToString()) + new_APPLY_OVERTIME_HOUR > TotalCompareMinute)
                            errMsg += "管制對象為早餐勤前加班，每月(平日加班+假日加班大於8小時的時數)不可大於46Hr/月";
                    }
                    break;
                //檢核管制對象為早餐勤前加班+三高員工=5: 每月(平日加班+假日加班時數)不可大於37Hr/月;  計算勤前加班時數含早餐用餐時間;
                case "5":
                    TotalCompareMinute = 37 * 60;
                    if (ctlHour.Rows.Count > 0)
                    {
                        if (int.Parse(ctlHour.Rows[0]["ctlsum"].ToString()) + int.Parse(dao.APPLY_OVERTIME_HOUR) > TotalCompareMinute)
                            errMsg += "管制對象為早餐勤前加班+三高員工，每月(平日加班+假日加班時數)不可大於37Hr/月";
                    }
                    break;
                //檢核管制對象為早餐勤前加班+高齡=6: 每月(平日加班+假日加班時數)不可大於0Hr/月;  計算勤前加班時數含早餐用餐時間;
                case "6":
                    TotalCompareMinute = 0;
                    if (int.Parse(dao.APPLY_OVERTIME_HOUR) > 0)
                        errMsg += "管制對象為早餐勤前加班+高齡，每月(平日加班+假日加班時數)不可大於0Hr/月";
                    break;
                default:
                    break;
            }



            if (emp_id != "" && errMsg != "")
                errMsg = "工號:" + emp_id + "--" + errMsg;

            return errMsg;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public DataTable getOvertimeDtType(string overtime_cd)
    {
        CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
        try
        {
            return fb2di050.getOvertimeDtType(overtime_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public int chk_IS_APPLY(string empid, string overtime_cd)
    {
        CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
        try
        {
            return fb2di050.chk_IS_APPLY(empid,overtime_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getIFlowNO(string emp_id)
    {
        CFB2DI0500DAO wfb2di = new CFB2DI0500DAO();
        try
        {
            return wfb2di.getIFlowNO(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getData(string emp_id, string iflow_no)
    {
        try
        {
            CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
            return fb2di050.getDtlData(emp_id, iflow_no);
        }
        catch (Exception)
        {

            throw;
        }
    }



    public string updateEmpData(CFB2DI0500DAO fb2di050)
    {
        try
        {
            string errStr = checkValid(fb2di050);

            if (errStr == "")
            {

                BeginTransaction();
                try
                {
                    fb2di050.updateEmp();

                    Commit();


                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
                fb2di050.SP_D_EMP_DUTY_CHECK_STATUS_REOPEN(fb2di050.EMP_ID, fb2di050.APPLY_OVERTIME_DT);

                return "0";
            }
            return errStr;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public DataTable getDtlData(string emp_id, string iflow_no)
    {
        try
        {
            CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
            return fb2di050.getDtlData(emp_id, iflow_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string delete_Emp(List<Tuple<string, string, string>> editindex, CFB2DI0500DAO fb2di050)
    {
        //CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
        string rtnmessage = "";
        try
        {
            foreach (var item in editindex)
            {
                //檢查是否已計薪且發薪日期不為空白
                DataTable dt = fb2di050.getSalaryStatus(item.Item1, item.Item2);
                if (dt.Rows.Count > 0)
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
                        fb2di050.deleteEmpID(item.Item1, item.Item2);
                        //日勤務狀態reopen
                        fb2di050.deleteCHECK_STATUS(item.Item1, item.Item3);
                        //日勤務狀態reopen-代休日期
                        fb2di050.deleteCHECK_STATUS2(item.Item1, item.Item2, item.Item3);
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
                    fb2di050.SP_D_EMP_DUTY_CHECK_STATUS_REOPEN(item.Item1, item.Item3);
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
    public DataTable getShiftCD(string emp_id, string apply_overtime_dt)
    {
        CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
        try
        {
            return fb2di050.getShiftCD(emp_id, apply_overtime_dt);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getOvertimeCtlCD(string emp_id)
    {
        CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
        try
        {
            return fb2di050.getOvertimeCtlCD(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getClockTime(string emp_id, string apply_overtime_dt)
    {
        CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
        try
        {
            return fb2di050.getClockTime(emp_id, apply_overtime_dt);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOverTimeData(string emp_id, string apply_overtime_dt)
    {
        CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
        try
        {
            return fb2di050.getOverTimeData(emp_id, apply_overtime_dt);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getControlCD(string emp_id)
    {
        CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
        try
        {
            return fb2di050.getControlCD(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public string Confirm_empid(List<Tuple<string, string, string>> emp_id, CFB2DI0500DAO fb2di050)
    {
        //CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();

        string rtnmessage = "";
        //檢查OK逐筆修改
        if (rtnmessage == "")
        {
            try
            {
                BeginTransaction();
                foreach (var item in emp_id)
                {
                    fb2di050.Confirm_empid(item.Item1, item.Item2, item.Item3);
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

    public string addBatchData(CFB2DI0500DAO fb2di050, List<string> emp_data)
    {
        try
        {
            string errStr = "";
            //檢查 Grid中的工號 且 申請日期 是否已存在
            foreach (var emp_id in emp_data)
            {
                DataTable dupdata = fb2di050.getDupData(emp_id);
                if (dupdata.Rows.Count > 0)
                {
                    errStr += emp_id + dupdata.Rows[0]["EMP_NAME"].ToString().Trim() + " 該工號加班日期時間已存在，不可重複申請 ! \\n";
                }
                /*
                DataTable leavedata = fb2di050.getBatchLeaveData(emp_id);
                if (dupdata.Rows.Count > 0)
                {
                    //errStr += emp_id + dupdata.Rows[0]["EMP_NAME"].ToString().Trim() + " 該工號請假日期時間已存在，不可重複申請 ! \\n";
                }
                */
                DataTable iflow_no = fb2di050.getIFlowNO(emp_id);
                if (iflow_no.Rows.Count > 0)
                {
                    if (iflow_no.Rows[0]["IFLOW_NO"].ToString() != "")
                    {
                        fb2di050.IFLOW_NO = emp_id + iflow_no.Rows[0]["IFLOW_NO"].ToString();
                    }
                    else
                    {
                        fb2di050.IFLOW_NO = emp_id + DateTime.Now.ToString("yyyyMMdd") + "000001";
                    }
                }
                //管制對象 1.一般員工、2.高血壓(+高血脂、+心血管)、3.高齡(60歲以上)               
                DataTable IsDC = fb2di050.getOvertimeCtlCD(emp_id);
                if (IsDC.Rows.Count > 0)
                {
                    if (IsDC.Rows[0]["OVERTIME_CTL_CD"].ToString() == "1")
                    {
                        fb2di050.OVERTIME_CTL_CD = IsDC.Rows[0]["OVERTIME_CTL_CD"].ToString();
                    }
                    if (IsDC.Rows[0]["OVERTIME_CTL_CD"].ToString() == "2")
                    {
                        fb2di050.OVERTIME_CTL_CD = IsDC.Rows[0]["OVERTIME_CTL_CD"].ToString();
                    }
                    if (IsDC.Rows[0]["OVERTIME_CTL_CD"].ToString() == "3")
                    {
                        fb2di050.OVERTIME_CTL_CD = IsDC.Rows[0]["OVERTIME_CTL_CD"].ToString();
                    }
                }
                //班別
                DataTable dt = new DataTable();
                string apply_overtime_dt = fb2di050.APPLY_OVERTIME_DT;
                dt = fb2di050.getShiftCD(emp_id, apply_overtime_dt);
                if (dt.Rows.Count > 0)
                {
                    fb2di050.SHIFT_CD = dt.Rows[0]["SHIFT_CD"].ToString();
                }
                //刷卡上下班時間
                DataTable clockTime = new DataTable();
                clockTime = fb2di050.getClockTime(emp_id, apply_overtime_dt);
                if (clockTime.Rows.Count > 0)
                {
                    fb2di050.CLOCK_IN_TIME = clockTime.Rows[0]["CLOCK_IN_DT"].ToString();
                    fb2di050.CLOCK_OUT_TIME = clockTime.Rows[0]["CLOCK_OUT_DT"].ToString();

                    int i = 0, o = 0, appoh = 0;
                    if ((clockTime.Rows[0]["CLOCK_IN_DT"].ToString() != "" && clockTime.Rows[0]["CLOCK_OUT_DT"].ToString() != ""))
                    {
                        string cit = clockTime.Rows[0]["CLOCK_IN_DT"].ToString();
                        string cot = clockTime.Rows[0]["CLOCK_OUT_DT"].ToString();
                        string[] ci = cit.Split(':');
                        string[] co = cot.Split(':');

                        i = int.Parse(ci[0]) * 60 + int.Parse(ci[1]);
                        o = int.Parse(co[0]) * 60 + int.Parse(co[1]);
                        appoh = o - i;
                        fb2di050.APPROVE_OVERTIME_HOUR = appoh.ToString();
                        fb2di050.EXCHANGE_HOUR = appoh.ToString();

                    }
                    else
                    {
                        //fb2di050.APPROVE_OVERTIME_HOUR = "0";
                        fb2di050.EXCHANGE_HOUR = "0";
                    }
                }
                else
                {
                    //fb2di050.APPROVE_OVERTIME_HOUR = "0";
                    fb2di050.EXCHANGE_HOUR = "0";
                }

            }

            if (errStr == "")
            {

                try
                {
                    BeginTransaction();
                    foreach (var emp_id in emp_data)
                    {
                        fb2di050.EMP_ID = emp_id;
                        fb2di050.addBatch();
                    }
                    Commit();

                    foreach (var emp_id in emp_data)
                    {
                        fb2di050.EMP_ID = emp_id;
                        fb2di050.SP_D_EMP_DUTY_CHECK_STATUS_REOPEN(emp_id, fb2di050.APPLY_OVERTIME_DT);
                    }

                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
            }
            return errStr;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }



    public DataTable getEMP_DATA(string emp_id)
    {
        try
        {
            CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
            return fb2di050.getEMP_DATA(emp_id);
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
            CFB2DI0500DAO fb2di050 = new CFB2DI0500DAO();
            return fb2di050.getDEPT_DATA(dept_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getTIME(string emp_id, string apply_overtime_dt, string stime, string etime, string WorkDayCd, string d, string ShiftCd)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            return wfb2di.getTIME(emp_id, apply_overtime_dt, stime, etime, WorkDayCd, d, ShiftCd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getOvertimeCD(string emp_id, string apply_overtime_dt, string apply_overtime_s, string apply_overtime_e)
    {
        try
        {
            CFB2DI0500DAO wfb2di = new CFB2DI0500DAO();
            DataTable dt = wfb2di.getOvertimeCD(emp_id, apply_overtime_dt, apply_overtime_s, apply_overtime_e);
            if (dt.Rows.Count > 0)
            {
                return "2-語文課時段";
            }
            else
            {
                return "1-一般時段";
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDutyData(string emp_id, string apply_overtime_dt)
    {
        try
        {
            CFB2DI0500DAO wfb2di = new CFB2DI0500DAO();
            return wfb2di.getDutyData(emp_id, apply_overtime_dt);

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

    public DataTable getSHIFT_CD(string shift_cd)
    {
        try
        {
            CFB2DI0500DAO wfb2di = new CFB2DI0500DAO();
            return wfb2di.getSHIFT_CD(shift_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME(string emp_id, string apply_overtime_dt)
    {
        try
        {
            CFB2DI0500DAO wfb2di = new CFB2DI0500DAO();
            return wfb2di.checkOVERTIME(emp_id, apply_overtime_dt);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool checkDUTY_STIME(string emp_id, string apply_overtime_dt, string before_etime)
    {
        try
        {
            bool is_ok = false;
            CFB2DI0500DAO wfb2di = new CFB2DI0500DAO();
            DataTable tmp = wfb2di.checkDUTY_STIME(emp_id, apply_overtime_dt, before_etime);
            if (tmp.Rows.Count > 0)
            {
                is_ok = true;
            }
            return is_ok;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool checkDUTY_ETIME(string emp_id, string apply_overtime_dt, string after_stime)
    {
        try
        {
            bool is_ok = false;
            CFB2DI0500DAO wfb2di = new CFB2DI0500DAO();
            DataTable tmp = wfb2di.checkDUTY_ETIME(emp_id, apply_overtime_dt, after_stime);
            if (tmp.Rows.Count > 0)
            {
                is_ok = true;
            }
            return is_ok;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME2(string apply_overtime_dt, string shift_cd)
    {
        try
        {
            CFB2DI0500DAO wfb2di = new CFB2DI0500DAO();
            return wfb2di.checkOVERTIME2(apply_overtime_dt, shift_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME_CD(string overtime_cd)
    {
        try
        {
            CFB2DI0500DAO wfb2di = new CFB2DI0500DAO();
            return wfb2di.getOVERTIME_CD(overtime_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getCLOCK_RECORDS(string emp_id, string apply_overtime_dt)
    {
        try
        {
            CFB2DI0500DAO wfb2di = new CFB2DI0500DAO();
            wfb2di.EMP_ID = emp_id;
            wfb2di.APPLY_OVERTIME_DT = apply_overtime_dt;
            return wfb2di.getCLOCK_RECORDS();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEMP_ID(string plant_cd, string dept_no, string ws_cd, string work_cd, string work_shift_cd)
    {
        CFB2DI0500DAO wfb2dh = new CFB2DI0500DAO();
        try
        {
            return wfb2dh.getEMP_ID(plant_cd, dept_no, ws_cd, work_cd, work_shift_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string SP_DI_OVERTIME_CHK(CFB2DI0500DAO dao)
    {
        try
        {
            string result = dao.SP_DI_OVERTIME_CHK();

            return result;
        }
        catch (Exception ex)
        {
            return "E"+ex.Message;
        }
    }

    public string getHYPER_SHOUR(CFB2DI0500DAO dao,string h)
    {
        try
        {
            string result = "0";
            DataTable dt = dao.getHYPER_SHOUR();
            if (dt.Rows.Count > 0)
            {
                if (h == "1")
                    result = dt.Rows[0]["HYPER_SHOUR"].ToString();
                if (h == "2")
                    result = dt.Rows[0]["NORMAL_SHOUR"].ToString();
            }

            return result;
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    public string insertTB_D_M_OVERTIME_APPLY(CFB2DI0500DAO dao)
    {
        try
        {
            string result = "0";
            BeginTransaction();
            dao.insertTB_D_M_OVERTIME_APPLY();
            Commit();

            //啟動刷卡比對
            //啟動重新刷卡比對
            dao.SP_D_EMP_DUTY_CHECK_STATUS_REOPEN(dao.EMP_ID, dao.APPLY_OVERTIME_DT);

            return result;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public DataTable getTB_H_M_EMP(string EMP_ID)
    {
        try
        {
            CFB2DI0500DAO dao = new CFB2DI0500DAO();
            return dao.getTB_H_M_EMP(EMP_ID);
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    public string getFN_D_GET_OVERTIME_APPLY_HOUR(CFB2DI0500DAO dao, string O_START_TIME, string O_END_TIME, string SORUCE_CD)
    {
        try
        {
            string result = "0";
            DataTable dt = dao.getFN_D_GET_OVERTIME_APPLY_HOUR(O_START_TIME, O_END_TIME, SORUCE_CD);
            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["OVERTIME_APPLY_HOUR"].ToString();
            }

            return result;
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    public DataTable getEMP_NAME(string emp_id)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            return wfb2di.getEMP_NAME(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDefaultData(string emp_id, string apply_overtime_dt, string iflow_no)
    {
        try
        {
            CFB2DI0500DAO wfb2di = new CFB2DI0500DAO();
            return wfb2di.getDefaultData(emp_id, apply_overtime_dt, iflow_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSUB_DESC(string main_cd, string sys_cd, string sub_cd)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            return wfb2di.getSUB_DESC(main_cd, sys_cd, sub_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSHIFT_DESC(string shift_cd)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            return wfb2di.getSHIFT_DESC(shift_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getSHIFT_CD(string emp_id, string apply_overtime_dt)
    {
        try
        {
            string shift_cd = "";
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            DataTable tmp = wfb2di.getSHIFT_CD(emp_id, apply_overtime_dt);
            if (tmp.Rows.Count > 0)
            {
                shift_cd = tmp.Rows[0]["SHIFT_CD"].ToString();
            }

            return shift_cd;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getIFLOW_NO(string apply_overtime_dt)
    {
        try
        {
            string iflow_no = "";
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            DataTable tmp = wfb2di.getIFLOW_NO(apply_overtime_dt);
            if (tmp.Rows.Count > 0)
            {
                iflow_no = tmp.Rows[0]["IFLOW_NO"].ToString();
                int no = Convert.ToInt32(iflow_no.Substring(11));
                iflow_no = "HRO" + Convert.ToDateTime(apply_overtime_dt).ToString("yyyyMMdd") + (no + 1).ToString("00000");
            }
            else
            {
                iflow_no = "HRO" + Convert.ToDateTime(apply_overtime_dt).ToString("yyyyMMdd") + "00001";
            }
            return iflow_no;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string saveTB_D_M_OVERTIME_APPLY(CFB2DI0500DAO dao, string mod)
    {
        try
        {
            string errMsg = "";

            if (errMsg.Trim().Length == 0)
            {
                BeginTransaction();

                //更新模式
                if (mod == "mod")
                {
                    //更新
                    dao.updateOVERTIME_APPLY();
                }
                else
                {
                    //新增模式
                    dao.insertTB_D_M_OVERTIME_APPLY();
                }
                /*
                //(2)更新日勤務狀態檔- reopen
                //更新日勤務狀態資料檔及重新reopen
                dao.updateEMP_DUTY_CHECK_STATUS("0");
                if (string.IsNullOrEmpty(dao.REPLACE_DT) == false)
                {
                    dao.updateEMP_DUTY_CHECK_STATUS("1");
                }
                */
                Commit();

                //啟動刷卡比對
                //啟動重新刷卡比對
                dao.SP_D_EMP_DUTY_CHECK_STATUS_REOPEN(dao.EMP_ID, dao.APPLY_OVERTIME_DT);
                return "0";
            }
            else
                return errMsg;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public DataTable getCalendarTime(string emp_id, string apply_overtime_dt)
    {
        try
        {
            CFB2DI0500DAO wfb2di = new CFB2DI0500DAO();
            wfb2di.EMP_ID = emp_id;
            wfb2di.APPLY_OVERTIME_DT = apply_overtime_dt;
            return wfb2di.getCalendarTime();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //加班註銷檢核
    public string SP_DI_OVERTIME_X0_CHK(List<Tuple<string, string, string, string>> emp_id)
    {
        try
        {
            string result = "";
            string msg = "";
            CFB2DI0500DAO di050DAO = new CFB2DI0500DAO();

            foreach (var item in emp_id)
            {
                //有申告且非平日,才檢查
                if (item.Item3 == "Y" && item.Item4 != "1")
                {
                    msg = di050DAO.SP_DI_OVERTIME_X0_CHK(item);
                    if(msg !="")
                        result += msg + ";\\n";
                }
            }

            return result;
        }
        catch (Exception ex)
        {
            return "E" + ex.Message;
        }
    }
}