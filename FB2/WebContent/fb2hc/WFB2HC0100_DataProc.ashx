<%@ WebHandler Language="C#" Class="WFB2HC0100" %>

using System;
using System.Web;
using System.Collections;

public class WFB2HC0100 : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string DATA_TYPE = context.Request.QueryString["DATA_TYPE"];
        //取得[員工姓名]
        if (DATA_TYPE == "Qry_GET_EMP_NAME")
        {
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            do_Qry_GET_EMP_NAME(context, EMP_ID);
        }
        //取得[人事異動代碼說明]
        else if (DATA_TYPE == "Qry_GET_HR_CHG_DESC")
        {
            string HR_CHG_CD = context.Request.QueryString["HR_CHG_CD"];
            do_Qry_GET_HR_CHG_DESC(context, HR_CHG_CD);
        }
        //取得[員工姓名]-人事異動對象的員工姓名
        else if (DATA_TYPE == "GET_EMP_NAME")
        {
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            do_GET_EMP_NAME(context, EMP_ID);
        }
        //異動生效日不可以是已薪結日期    
        else if (DATA_TYPE == "Check_FN_S_SALARY_YM") {
            string START_DT = context.Request.QueryString["START_DT"];
            do_Check_FN_S_SALARY_YM(context, START_DT);
        }
        //相同工號、人事異動代碼、異動生效日期判別
        else if (DATA_TYPE == "CHECK_SAME_DATA1")
        {
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            string START_DT = context.Request.QueryString["START_DT"];
            string HR_CHG_CD = context.Request.QueryString["HR_CHG_CD"];
            do_CHECK_SAME_DATA1(context, EMP_ID, START_DT, HR_CHG_CD);
        }
        //相同異動生效日存在 XXXXX, XXXXX 的人事異動單判別
        else if (DATA_TYPE == "CHECK_SAME_DATA2")
        {
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            string START_DT = context.Request.QueryString["START_DT"];            
            do_CHECK_SAME_DATA2(context, EMP_ID, START_DT);
        }
        //[保險預計處理日] 檢核    
        else if (DATA_TYPE == "CHECK_INS_PLAN_PROC_DT")
        {
            string HR_CHG_CD = context.Request.QueryString["HR_CHG_CD"];
            string INS_PLAN_PROC_DT = context.Request.QueryString["INS_PLAN_PROC_DT"];
            string START_DT = context.Request.QueryString["START_DT"];
            do_CHECK_INS_PLAN_PROC_DT(context, HR_CHG_CD,INS_PLAN_PROC_DT,START_DT);
        }
        //取得預設 [狀態預計結束日]
        else if (DATA_TYPE == "GET_PLAN_END_DT")
        {
            string HR_CHG_CD = context.Request.QueryString["HR_CHG_CD"];            
            string START_DT = context.Request.QueryString["START_DT"];
            do_GET_PLAN_END_DT(context, HR_CHG_CD, START_DT);
        }
        //[狀態預計結束日] 檢核
        else if (DATA_TYPE == "CHECK_PLAN_END_DT")
        {
            string HR_CHG_CD = context.Request.QueryString["HR_CHG_CD"];
            string START_DT = context.Request.QueryString["START_DT"];
            string PLAN_END_DT = context.Request.QueryString["PLAN_END_DT"];
            do_CHECK_PLAN_END_DT(context, HR_CHG_CD, PLAN_END_DT, START_DT);
        }
        //取得 [狀態結束]
        else if (DATA_TYPE == "GET_IS_END")
        {
            string HR_CHG_CD = context.Request.QueryString["HR_CHG_CD"];
            bool IS_END = Convert.ToBoolean(context.Request.QueryString["IS_END"]);
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            string START_DT = context.Request.QueryString["START_DT"];
            do_GET_IS_END(context, HR_CHG_CD, IS_END, EMP_ID, START_DT);
        }
        //取得 [異動主編號]
        else if (DATA_TYPE == "GET_MAIN_HR_CHG_NO")
        {            
            bool IS_END = Convert.ToBoolean(context.Request.QueryString["IS_END"]);
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            string START_DT = context.Request.QueryString["START_DT"];
            do_GET_MAIN_HR_CHG_NO(context, IS_END, EMP_ID, START_DT);
        }
        //取得受入公司下拉選單資料    
        else if (DATA_TYPE == "GET_TRANSFER_COMPANY_CD")
        {
            string HR_CHG_CD = context.Request.QueryString["HR_CHG_CD"];
            do_GET_TRANSFER_COMPANY_CD(context, HR_CHG_CD);
        }
        //取得 [兼任部門] 名稱
        else if (DATA_TYPE == "ADJUNCT_GET_DEPT_NAME")
        {
            string DEPT_NO = context.Request.QueryString["DEPT_NO"];
            string START_DT = context.Request.QueryString["START_DT"];
            do_ADJUNCT_GET_DEPT_NAME(context, DEPT_NO, START_DT);
        }
        //取得 [兼任職務] 名稱
        else if (DATA_TYPE == "ADJUNCT_GET_PJOB_DESC")
        {
            string PJOB_CD = context.Request.QueryString["PJOB_CD"];
            string START_DT = context.Request.QueryString["START_DT"];
            do_ADJUNCT_GET_PJOB_DESC(context, PJOB_CD, START_DT);
        }
        //取得兼任以外的人事異動項目清單
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_List")
        {
            string HR_CHG_CD = context.Request.QueryString["HR_CHG_CD"];
            do_Get_HR_CHG_ITEM_List(context, HR_CHG_CD);
        }
        //取得異動項目01-異動前代碼、異動前代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_01_BEFORE")
        {
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            do_Get_HR_CHG_ITEM_N_BEFORE(context, 1, EMP_ID);
        }
        //取得異動項目01-異動後代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_01_AFTER")
        {
            string COMPANY_CD = context.Request.QueryString["COMPANY_CD"];
            do_Get_HR_CHG_ITEM_01_AFTER(context, COMPANY_CD);
        }
        //取得異動項目02-異動前代碼、異動前代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_02_BEFORE")
        {
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            do_Get_HR_CHG_ITEM_N_BEFORE(context, 2, EMP_ID);
        }
        //取得異動項目02-異動後代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_02_AFTER")
        {
            string PLANT_CD = context.Request.QueryString["PLANT_CD"];
            do_Get_HR_CHG_ITEM_02_AFTER(context, PLANT_CD);
        }
        //取得異動項目03-異動前代碼、異動前代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_03_BEFORE")
        {
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            do_Get_HR_CHG_ITEM_N_BEFORE(context, 3, EMP_ID);
        }
        //取得異動項目03-異動後代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_03_AFTER")
        {
            string WS_CD = context.Request.QueryString["WS_CD"];
            do_Get_HR_CHG_ITEM_03_AFTER(context, WS_CD);
        }
        //取得異動項目04-異動前代碼、異動前代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_04_BEFORE")
        {
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            do_Get_HR_CHG_ITEM_N_BEFORE(context, 4, EMP_ID);
        }
        //取得異動項目04-異動後代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_04_AFTER")
        {
            string EMP_CD = context.Request.QueryString["EMP_CD"];
            do_Get_HR_CHG_ITEM_04_AFTER(context, EMP_CD);
        }
        //取得異動項目05-異動前代碼、異動前代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_05_BEFORE")
        {
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            do_Get_HR_CHG_ITEM_N_BEFORE(context, 5, EMP_ID);
        }
        //取得異動項目05-異動後代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_05_AFTER")
        {
            string DEPT_NO = context.Request.QueryString["DEPT_NO"];
            string START_DT = context.Request.QueryString["START_DT"];
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            do_Get_HR_CHG_ITEM_05_AFTER(context, DEPT_NO, START_DT, EMP_ID);
        }
        //取得異動項目06-異動前代碼、異動前代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_06_BEFORE")
        {
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            do_Get_HR_CHG_ITEM_N_BEFORE(context, 6, EMP_ID);
        }
        //取得異動項目06-異動後代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_06_AFTER")
        {
            string LEVEL_CD = context.Request.QueryString["LEVEL_CD"];
            string START_DT = context.Request.QueryString["START_DT"];
            do_Get_HR_CHG_ITEM_06_AFTER(context, LEVEL_CD, START_DT);
        }
        //取得異動項目07-異動前代碼、異動前代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_07_BEFORE")
        {
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            do_Get_HR_CHG_ITEM_N_BEFORE(context, 7, EMP_ID);
        }
        //取得異動項目07-異動後代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_07_AFTER")
        {
            string GRADE_CD = context.Request.QueryString["GRADE_CD"];
            string LEVEL_CD = context.Request.QueryString["LEVEL_CD"];
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            do_Get_HR_CHG_ITEM_07_AFTER(context, GRADE_CD, LEVEL_CD, EMP_ID);
        }
        //取得異動項目07-異動後代碼說明
        else if (DATA_TYPE == "Get_Add_batch_HR_CHG_ITEM_07_AFTER")
        {
            string GRADE_CD = context.Request.QueryString["GRADE_CD"];            
            do_Get_Add_batch_HR_CHG_ITEM_07_AFTER(context, GRADE_CD);
        }    
        //取得異動項目08-異動前代碼、異動前代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_08_BEFORE")
        {
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            do_Get_HR_CHG_ITEM_N_BEFORE(context, 8, EMP_ID);
        }
        //取得異動項目08-異動後代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_08_AFTER")
        {
            string PJOB_CD = context.Request.QueryString["PJOB_CD"];
            string START_DT = context.Request.QueryString["START_DT"];
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            do_Get_HR_CHG_ITEM_08_AFTER(context, PJOB_CD, START_DT, EMP_ID);
        }
        //取得異動項目08-異動後代碼說明
        else if (DATA_TYPE == "Get_Add_batch_HR_CHG_ITEM_08_AFTER")
        {
            string PJOB_CD = context.Request.QueryString["PJOB_CD"];            
            do_Get_Add_batch_HR_CHG_ITEM_08_AFTER(context, PJOB_CD);
        }
        //取得異動項目09-異動前代碼、異動前代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_09_BEFORE")
        {
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            do_Get_HR_CHG_ITEM_N_BEFORE(context, 9, EMP_ID);
        }
        //取得異動項目09-異動後代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_09_AFTER")
        {
            string WORK_SHIFT_CD = context.Request.QueryString["WORK_SHIFT_CD"];
            do_Get_HR_CHG_ITEM_09_AFTER(context, WORK_SHIFT_CD);
        }
        //取得異動項目10-異動前代碼、異動前代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_10_BEFORE")
        {
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            do_Get_HR_CHG_ITEM_N_BEFORE(context, 10, EMP_ID);
        }
        //取得異動項目10-異動後代碼說明
        else if (DATA_TYPE == "Get_HR_CHG_ITEM_10_AFTER")
        {
            string WORK_CD = context.Request.QueryString["WORK_CD"];
            do_Get_HR_CHG_ITEM_10_AFTER(context, WORK_CD);
        }
        //Add_batch [保險預計處理日] 檢核    
        else if (DATA_TYPE == "CHECK_ADD_BATCH_INS_PLAN_PROC_DT")
        {
            string HR_CHG_CD = context.Request.QueryString["HR_CHG_CD"];
            string INS_PLAN_PROC_DT = context.Request.QueryString["INS_PLAN_PROC_DT"];
            string START_DT = context.Request.QueryString["START_DT"];
            do_CHECK_ADD_BATCH_INS_PLAN_PROC_DT(context, HR_CHG_CD, INS_PLAN_PROC_DT, START_DT);
        }
    }

    //取得[員工姓名]
    private void do_Qry_GET_EMP_NAME(HttpContext context, string emp_id)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();

        try
        {
            ArrayList data = service.Qry_Get_EMP_NAME(emp_id);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";                    
                    json.strEMP_NAME = ((string[])(data[0]))[1];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得[人事異動代碼說明]
    private void do_Qry_GET_HR_CHG_DESC(HttpContext context, string hr_chg_cd)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();

        try
        {
            ArrayList data = service.Qry_Get_HR_CHG_DESC(hr_chg_cd);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strHR_CHG_DESC = ((string[])(data[0]))[1];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得[員工姓名]
    private void do_GET_EMP_NAME(HttpContext context, string emp_id)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();        
        DATA json = new DATA();
        
        try
        {            
            ArrayList data = service.Get_EMP_NAME(emp_id);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strEMP_CD = ((string[])(data[0]))[1];
                    json.strEMP_NAME = ((string[])(data[0]))[2];
                    json.strPLAN_DESPATCH_DT = ((string[])(data[0]))[3];
                    json.strPLAN_DESPATCH_NEXT_DT = ((string[])(data[0]))[6];
                    json.strEMP_CD = ((string[])(data[0]))[4];
                    json.strLEVEL_CD = ((string[])(data[0]))[5];
                }
                else {
                    json.errMsg = ((string[])(data[0]))[0];
                }                
            }                                                                    
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //異動生效日不可以是已薪結日期
    private void do_Check_FN_S_SALARY_YM(HttpContext context, string start_dt)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {

           ArrayList data = service.Check_FN_S_SALARY_YM(start_dt);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";                    
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //相同工號、人事異動代碼、異動生效日期判別
    private void do_CHECK_SAME_DATA1(HttpContext context, string emp_id, string start_dt, string hr_chg_cd)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Check_Same_Data1(emp_id,start_dt,hr_chg_cd);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";                    
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //相同異動生效日存在 XXXXX, XXXXX 的人事異動單判別
    private void do_CHECK_SAME_DATA2(HttpContext context, string emp_id, string start_dt)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Check_Same_Data2(emp_id, start_dt);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //[保險預計處理日] 檢核
    private void do_CHECK_INS_PLAN_PROC_DT(HttpContext context, string hr_chg_cd, string ins_plan_proc_dt,string start_dt)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Check_INS_PLAN_PROC_DT(hr_chg_cd, ins_plan_proc_dt,start_dt);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strIS_INS_EARLIER = ((string[])(data[0]))[1];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得預設 [狀態預計結束日]
    private void do_GET_PLAN_END_DT(HttpContext context, string hr_chg_cd, string start_dt)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_PLAN_END_DT(hr_chg_cd, start_dt);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strIS_TEMP = ((string[])(data[0]))[1];
                    json.strEMP_CHG_STATUS = ((string[])(data[0]))[2];
                    json.strPLAN_END_DT = ((string[])(data[0]))[3];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //[狀態預計結束日] 檢核
    private void do_CHECK_PLAN_END_DT(HttpContext context, string hr_chg_cd, string plan_end_dt, string start_dt)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Check_PLAN_END_DT(hr_chg_cd, plan_end_dt, start_dt);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strIS_TEMP = ((string[])(data[0]))[1];                    
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得 [狀態結束]
    private void do_GET_IS_END(HttpContext context, string hr_chg_cd, bool is_end, string emp_id, string start_dt)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_IS_END(hr_chg_cd, is_end, emp_id, start_dt);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.bolIS_END = Convert.ToBoolean(((string[])(data[0]))[1]);
                    json.bolIS_END_disabled = Convert.ToBoolean(((string[])(data[0]))[2]);
                    json.strHR_CHG_NO = ((string[])(data[0]))[3];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得 [異動主編號]
    private void do_GET_MAIN_HR_CHG_NO(HttpContext context, bool is_end, string emp_id, string start_dt)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_MAIN_HR_CHG_NO(is_end, emp_id, start_dt);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.bolIS_END = Convert.ToBoolean(((string[])(data[0]))[1]);
                    json.bolIS_END_disabled = Convert.ToBoolean(((string[])(data[0]))[2]);
                    json.strHR_CHG_NO = ((string[])(data[0]))[3];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                    if (((string[])(data[0])).Length == 3)
                    {
                        json.bolIS_END = Convert.ToBoolean(((string[])(data[0]))[1]);
                        json.bolIS_END_disabled = Convert.ToBoolean(((string[])(data[0]))[2]);
                        json.strHR_CHG_NO = ((string[])(data[0]))[3];
                    }
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得受入公司下拉選單資料
    private void do_GET_TRANSFER_COMPANY_CD(HttpContext context, string hr_chg_cd)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_TRANSFER_COMPANY_CD(hr_chg_cd);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.arlTRANSFER_COMPANY_CD = (ArrayList)data[1];                    
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];                    
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得 [兼任部門] 名稱
    private void do_ADJUNCT_GET_DEPT_NAME(HttpContext context, string dept_no, string start_dt)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Adjunct_Get_DEPT_NAME(dept_no,start_dt);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strDEPT_NAME = ((string[])(data[0]))[1];                    
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];                    
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得 [兼任職務] 名稱
    private void do_ADJUNCT_GET_PJOB_DESC(HttpContext context, string pjob_cd, string start_dt)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Adjunct_Get_PJOB_DESC(pjob_cd, start_dt);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strPJOB_DESC = ((string[])(data[0]))[1];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得兼任以外的人事異動項目清單
    private void do_Get_HR_CHG_ITEM_List(HttpContext context, string hr_chg_cd)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_HR_CHG_ITEM_List(hr_chg_cd);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.arlHR_CHG_ITEM = (ArrayList)data[1];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得異動項目01~10 - 異動前代碼、異動前代碼說明
    private void do_Get_HR_CHG_ITEM_N_BEFORE(HttpContext context, int index, string emp_id)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = new ArrayList();
            switch (index) {
                case 1:data = service.Get_HR_CHG_ITEM_01_BEFORE(emp_id);
                    break;
                case 2: data = service.Get_HR_CHG_ITEM_02_BEFORE(emp_id);
                    break;
                case 3: data = service.Get_HR_CHG_ITEM_03_BEFORE(emp_id);
                    break;
                case 4: data = service.Get_HR_CHG_ITEM_04_BEFORE(emp_id);
                    break;
                case 5: data = service.Get_HR_CHG_ITEM_05_BEFORE(emp_id);
                    break;
                case 6: data = service.Get_HR_CHG_ITEM_06_BEFORE(emp_id);
                    break;
                case 7: data = service.Get_HR_CHG_ITEM_07_BEFORE(emp_id);
                    break;
                case 8: data = service.Get_HR_CHG_ITEM_08_BEFORE(emp_id);
                    break;
                case 9: data = service.Get_HR_CHG_ITEM_09_BEFORE(emp_id);
                    break;
                case 10: data = service.Get_HR_CHG_ITEM_10_BEFORE(emp_id);
                    break;
            }
            
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strSUB_CD = ((string[])(data[0]))[1];
                    json.strSUB_DESC = ((string[])(data[0]))[2];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得異動項目01-異動後代碼說明
    private void do_Get_HR_CHG_ITEM_01_AFTER(HttpContext context, string company_cd)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_HR_CHG_ITEM_01_AFTER(company_cd);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strSUB_CD = ((string[])(data[0]))[1];
                    json.strSUB_DESC = ((string[])(data[0]))[2];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得異動項目02-異動後代碼說明
    private void do_Get_HR_CHG_ITEM_02_AFTER(HttpContext context, string plant_cd)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_HR_CHG_ITEM_02_AFTER(plant_cd);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strSUB_CD = ((string[])(data[0]))[1];
                    json.strSUB_DESC = ((string[])(data[0]))[2];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得異動項目03-異動後代碼說明
    private void do_Get_HR_CHG_ITEM_03_AFTER(HttpContext context, string ws_cd)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_HR_CHG_ITEM_03_AFTER(ws_cd);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strSUB_CD = ((string[])(data[0]))[1];
                    json.strSUB_DESC = ((string[])(data[0]))[2];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得異動項目04-異動後代碼說明
    private void do_Get_HR_CHG_ITEM_04_AFTER(HttpContext context, string emp_cd)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_HR_CHG_ITEM_04_AFTER(emp_cd);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strSUB_CD = ((string[])(data[0]))[1];
                    json.strSUB_DESC = ((string[])(data[0]))[2];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得異動項目05-異動後代碼說明
    private void do_Get_HR_CHG_ITEM_05_AFTER(HttpContext context, string dept_no, string start_dt,string emp_id)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_HR_CHG_ITEM_05_AFTER(dept_no,start_dt,emp_id);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strSUB_CD = ((string[])(data[0]))[1];
                    json.strSUB_DESC = ((string[])(data[0]))[2];
                    if (data.Count > 1)
                    {
                        json.checkPlantMsg = ((string[])(data[1]))[0];
                    }
                    else {
                        json.checkPlantMsg = "";
                    }
                    
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                    json.checkPlantMsg = ""; 
                }
                
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得異動項目06-異動後代碼說明
    private void do_Get_HR_CHG_ITEM_06_AFTER(HttpContext context, string level_cd, string start_dt)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_HR_CHG_ITEM_06_AFTER(level_cd,start_dt);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strSUB_CD = ((string[])(data[0]))[1];
                    json.strSUB_DESC = ((string[])(data[0]))[2];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得異動項目07-異動後代碼說明
    private void do_Get_HR_CHG_ITEM_07_AFTER(HttpContext context, string grade_cd, string level_cd, string emp_id)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_HR_CHG_ITEM_07_AFTER(grade_cd,level_cd,emp_id);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strSUB_CD = ((string[])(data[0]))[1];
                    json.strSUB_DESC = ((string[])(data[0]))[2];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得異動項目07-異動後代碼說明
    private void do_Get_Add_batch_HR_CHG_ITEM_07_AFTER(HttpContext context, string grade_cd)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_Add_batch_HR_CHG_ITEM_07_AFTER(grade_cd);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strSUB_CD = ((string[])(data[0]))[1];
                    json.strSUB_DESC = ((string[])(data[0]))[2];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得異動項目08-異動後代碼說明
    private void do_Get_HR_CHG_ITEM_08_AFTER(HttpContext context, string pjob_cd, string start_dt,string emp_id)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            //ArrayList data = service.Get_HR_CHG_ITEM_08_AFTER(pjob_cd,start_dt,emp_id);
            ArrayList data = service.Get_Add_batch_HR_CHG_ITEM_08_AFTER(pjob_cd);                        
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strSUB_CD = ((string[])(data[0]))[1];
                    json.strSUB_DESC = ((string[])(data[0]))[2];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得異動項目08-異動後代碼說明
    private void do_Get_Add_batch_HR_CHG_ITEM_08_AFTER(HttpContext context, string pjob_cd)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_Add_batch_HR_CHG_ITEM_08_AFTER(pjob_cd);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strSUB_CD = ((string[])(data[0]))[1];
                    json.strSUB_DESC = ((string[])(data[0]))[2];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得異動項目09-異動後代碼說明
    private void do_Get_HR_CHG_ITEM_09_AFTER(HttpContext context, string work_shift_cd)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_HR_CHG_ITEM_09_AFTER(work_shift_cd);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strSUB_CD = ((string[])(data[0]))[1];
                    json.strSUB_DESC = ((string[])(data[0]))[2];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //取得異動項目10-異動後代碼說明
    private void do_Get_HR_CHG_ITEM_10_AFTER(HttpContext context, string work_cd)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Get_HR_CHG_ITEM_10_AFTER(work_cd);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strSUB_CD = ((string[])(data[0]))[1];
                    json.strSUB_DESC = ((string[])(data[0]))[2];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    //Add_batch [保險預計處理日] 檢核
    private void do_CHECK_ADD_BATCH_INS_PLAN_PROC_DT(HttpContext context, string hr_chg_cd, string ins_plan_proc_dt, string start_dt)
    {
        CFB2HC0100BO service = new CFB2HC0100BO();
        DATA json = new DATA();
        try
        {
            ArrayList data = service.Check_Add_batch_INS_PLAN_PROC_DT(hr_chg_cd, ins_plan_proc_dt, start_dt);
            if (data.Count > 0)
            {
                json.bolSTATUS = true;
                if (((string[])(data[0]))[0] == "")
                {
                    json.errMsg = "";
                    json.strIS_INS_EARLIER = ((string[])(data[0]))[1];
                }
                else
                {
                    json.errMsg = ((string[])(data[0]))[0];
                }
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }
    
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    
    private class DATA
    {
        public string errMsg { get; set; }
        public string checkPlantMsg { get; set; }
        public bool bolSTATUS { get; set; }
        public string strEMP_ID { get; set; }
        public string strEMP_NAME { get; set; }
        public string strPLAN_DESPATCH_DT { get; set; }
        public string strPLAN_DESPATCH_NEXT_DT { get; set; }
        public string strEMP_CD { get; set; }
        public string strHR_CHG_CD { get; set; }
        public string strHR_CHG_DESC { get; set; }
        public string strIS_INS_EARLIER { get; set; }
        public string strIS_TEMP { get; set; }
        public string strEMP_CHG_STATUS { get; set; }
        public string strPLAN_END_DT { get; set; }
        public bool bolIS_END { get; set; }
        public bool bolIS_END_disabled { get; set; }
        public string strHR_CHG_NO { get; set; }
        public ArrayList arlTRANSFER_COMPANY_CD { get; set; }
        public string strDEPT_NAME { get; set; }
        public string strPJOB_DESC { get; set; }
        public ArrayList arlHR_CHG_ITEM { get; set; }
        public string strSUB_CD { get; set; }
        public string strSUB_DESC { get; set; }
        public string strLEVEL_CD { get; set; }
    }

}