<%@ WebHandler Language="C#" Class="WFB2SB2300" %>

using System;
using System.Web;

public class WFB2SB2300 : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string EMP_ID = context.Request.QueryString["EMP_ID"];
        CFB2SB2300BO service = new CFB2SB2300BO();
        EMP_DATA json = new EMP_DATA();
        try
        {
            if (EMP_ID != "")
            {
                System.Data.DataTable dt = service.getEMPFile(EMP_ID);
                if (dt.Rows.Count > 0)
                {
                    json.EMP_NAME = dt.Rows[0]["EMP_NAME"].ToString();
                    json.EMP_CD = dt.Rows[0]["EMP_CD"].ToString();
                    json.EMP_CD_DESC = dt.Rows[0]["EMP_CD_DESC"].ToString();
                    json.SUB_DESC = dt.Rows[0]["SUB_DESC"].ToString();
                    json.DEPT_NO = dt.Rows[0]["DEPT_NO"].ToString();
                    json.DEPT_NAME = dt.Rows[0]["DEPT_NAME"].ToString();
                    json.PJOB_DESC = dt.Rows[0]["PJOB_DESC"].ToString();
                    json.WORK_SHIFT_CD = dt.Rows[0]["WORK_SHIFT_CD"].ToString();
                    json.JOIN_DT = dt.Rows[0]["JOIN_DT"].ToString();
                    json.REGISTER_ADDR = dt.Rows[0]["REGISTER_ADDR"].ToString();
                    json.CONTACT_ADDR = dt.Rows[0]["CONTACT_ADDR"].ToString();
                    json.MOBILE_TEL_1 = dt.Rows[0]["MOBILE_TEL_1"].ToString();
                    json.CONTACT_TEL = dt.Rows[0]["CONTACT_TEL"].ToString();
                    json.LEAVE_DT = dt.Rows[0]["LEAVE_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["LEAVE_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                    
                    json.AGE = dt.Rows[0]["AGE"].ToString();
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
        public string WORK_SHIFT_CD { get; set; }
        public string JOIN_DT { get; set; }
        public string REGISTER_ADDR { get; set; }
        public string CONTACT_ADDR { get; set; }
        public string MOBILE_TEL_1 { get; set; }
        public string CONTACT_TEL { get; set; }
        public string AGE { get; set; }
        public string EMP_CD_DESC { get; set; }
        public string LEAVE_DT { get; set; }
        
        
        public string errMsg { get; set; }
    }

}