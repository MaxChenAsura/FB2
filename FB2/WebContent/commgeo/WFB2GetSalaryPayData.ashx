<%@ WebHandler Language="C#" Class="WFB2GetSalaryPayData" %>

using System;
using System.Web;

public class WFB2GetSalaryPayData : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string REMIT_DT = context.Request.QueryString["REMIT_DT"];
        string SALARY_TYPE = context.Request.QueryString["SALARY_TYPE"];
        COMMGEOBO service = new COMMGEOBO();
        SALARY_DATA json = new SALARY_DATA();
        try
        {
            COMMGEODAO commgeo = new COMMGEODAO();
            commgeo.REMIT_DT = REMIT_DT;
            commgeo.SALARY_TYPE = SALARY_TYPE;
            System.Data.DataTable dt = service.getSALARYFile(commgeo);
            if (dt.Rows.Count > 0)
            {
                json.SALARY_DT = dt.Rows[0]["SALARY_DT"].ToString();
                json.SALARY_YM = dt.Rows[0]["SALARY_YM"].ToString();
                json.errMsg = "";
            }
            else
            {
                json.errMsg = "0";
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
        public string REMIT_DT { get; set; }
        public string SALARY_TYPE { get; set; }
        public string SALARY_DT { get; set; }
        public string SALARY_YM { get; set; }
        public string errMsg { get; set; }
    }

}