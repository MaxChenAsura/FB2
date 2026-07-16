<%@ WebHandler Language="C#" Class="WFB2GetSalaryData" %>

using System;
using System.Web;

public class WFB2GetSalaryData : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string SALARY_ID = context.Request.QueryString["SALARY_ID"];
        string SALARY_CD = context.Request.QueryString["SALARY_CD"];
        string EMP_ID = context.Request.QueryString["EMP_ID"];
        COMMGEOBO service = new COMMGEOBO();
        SALARY_DATA json = new SALARY_DATA();
        try
        {
            if (SALARY_ID != "")
            {
                COMMGEODAO commgeo = new COMMGEODAO();
                commgeo.SALARY_ID = SALARY_ID;
                commgeo.SALARY_CD = SALARY_CD;
                commgeo.EMP_ID = EMP_ID;
                System.Data.DataTable dt = service.getSALARYFile(commgeo);
                if (dt.Rows.Count > 0)
                {
                    json.SALARY_ID = dt.Rows[0]["SALARY_ID"].ToString();
                    json.SALARY_NAME = dt.Rows[0]["SALARY_NAME"].ToString();
                    json.errMsg = "";
                }
                else
                {
                    json.errMsg = "此薪資項目未存於薪資項目主檔或無權限";
                }

            }
            else
            {
                json.errMsg = "此薪資項目未存於薪資項目主檔或無權限";

            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    private class SALARY_DATA
    {
        public string SALARY_ID { get; set; }
        public string SALARY_CD { get; set; }
        public string SALARY_NAME { get; set; }
        public string errMsg { get; set; }
    }

}