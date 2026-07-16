<%@ WebHandler Language="C#" Class="WFBCOMMGEO" %>

using System;
using System.Web;

public class WFBCOMMGEO : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string EMP_ID = context.Request.QueryString["EMP_ID"];
        COMMGEOBO service = new COMMGEOBO();
        EMP_DATA json = new EMP_DATA();
        try
        {
            if (EMP_ID != "")
            {
                System.Data.DataTable dt = service.getEMPFile(EMP_ID);
                if (dt.Rows.Count > 0)
                {
                    json.EMP_NAME = dt.Rows[0]["EMP_NAME"].ToString().Trim();
                    json.EMP_CD = dt.Rows[0]["EMP_CD"].ToString();
                    json.SUB_DESC = dt.Rows[0]["SUB_DESC"].ToString();
                    json.DEPT_NO = dt.Rows[0]["DEPT_NO"].ToString();
                    json.DEPT_NAME = dt.Rows[0]["DEPT_NAME"].ToString();
                    json.PJOB_DESC = dt.Rows[0]["PJOB_DESC"].ToString();
                    json.LEVEL_CD = dt.Rows[0]["LEVEL_CD"].ToString();
                    json.WORK_SHIFT_CD = dt.Rows[0]["WORK_SHIFT_CD"].ToString();
                    json.WORK_SHIFT_NAME = dt.Rows[0]["WORK_SHIFT_NAME"].ToString();
                    json.JOIN_DT = dt.Rows[0]["JOIN_DT"].ToString();
                    json.REGISTER_ADDR = dt.Rows[0]["REGISTER_ADDR"].ToString();
                    json.CONTACT_ADDR = dt.Rows[0]["CONTACT_ADDR"].ToString();
                    json.MOBILE_TEL_1 = dt.Rows[0]["MOBILE_TEL_1"].ToString();
                    json.CONTACT_TEL = dt.Rows[0]["CONTACT_TEL"].ToString();
                    json.AGE = dt.Rows[0]["AGE"].ToString();
                    json.PLANT_CD = dt.Rows[0]["PLANT_CD"].ToString();
                    json.PLANT_NAME = dt.Rows[0]["PLANT_NAME"].ToString();
                    json.LICENSE_ID = dt.Rows[0]["LICENSE_ID"].ToString();
                    json.BIRTH_DT = dt.Rows[0]["BIRTH_DT"].ToString();
                    json.LEAVE_DT = dt.Rows[0]["LEAVE_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["LEAVE_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                    json.LINE_CD = dt.Rows[0]["LINE_CD"].ToString();
                    json.LINE_NAME = dt.Rows[0]["LINE_NAME"].ToString();
                    json.EMP_STATUS = dt.Rows[0]["EMP_STATUS"].ToString().Trim();
                    json.EMP_STATUS_DESC = dt.Rows[0]["EMP_STATUS_DESC"].ToString().Trim();
                    json.DEPT_NAME_20 = dt.Rows[0]["DEPT_NAME_20"].ToString();
                    json.DEPT_NAME_30 = dt.Rows[0]["DEPT_NAME_30"].ToString();
                    json.DEPT_NAME_40 = dt.Rows[0]["DEPT_NAME_40"].ToString();
                    json.DEPT_FULL_NAME = dt.Rows[0]["DEPT_FULL_NAME"].ToString();
                    json.DIV_DEPT_FULL_NAME = dt.Rows[0]["DIV_DEPT_FULL_NAME"].ToString();
                    json.WORK_CD = dt.Rows[0]["WORK_CD"].ToString();
                    json.WORK_CD_DESC = dt.Rows[0]["WORK_CD_DESC"].ToString();
                    json.errMsg = "";
                }
                else
                {
                    json.errMsg = "此工號未存於人事主檔";
                }

            }
            else
            {
                json.errMsg = "此工號未存於人事主檔";

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

    private class EMP_DATA
    {
        public string EMP_NAME { get; set; }
        public string EMP_CD { get; set; }
        public string SUB_DESC { get; set; }
        public string DEPT_NO { get; set; }
        public string DEPT_NAME { get; set; }
        public string PJOB_DESC { get; set; }
        public string LEVEL_CD { get; set; }
        public string WORK_SHIFT_CD { get; set; }
        public string JOIN_DT { get; set; }
        public string REGISTER_ADDR { get; set; }
        public string CONTACT_ADDR { get; set; }
        public string MOBILE_TEL_1 { get; set; }
        public string CONTACT_TEL { get; set; }
        public string AGE { get; set; }
        public string PLANT_CD { get; set; }
        public string PLANT_NAME { get; set; }
        public string WORK_SHIFT_NAME { get; set; }
        public string LICENSE_ID { get; set; }
        public string BIRTH_DT { get; set; }
        public string LEAVE_DT { get; set; }
        public string LINE_CD { get; set; }
        public string LINE_NAME { get; set; }
        public string EMP_STATUS { get; set; }
        public string EMP_STATUS_DESC { get; set; }
        public string DEPT_NAME_20 { get; set; }
        public string DEPT_NAME_30 { get; set; }
        public string DEPT_NAME_40 { get; set; }
        public string DEPT_FULL_NAME { get; set; }
        public string DIV_DEPT_FULL_NAME { get; set; }
        public string WORK_CD { get; set; }

        public string WORK_CD_DESC { get; set; }
        public string errMsg { get; set; }
    }

}