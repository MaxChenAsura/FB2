<%@ WebHandler Language="C#" Class="WFBCOMMGEO" %>

using System;
using System.Web;

public class WFBCOMMGEO : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string SUB_CD = context.Request.QueryString["SUB_CD"];
        string MAIN_CD = context.Request.QueryString["MAIN_CD"];
        string SYS_CD = context.Request.QueryString["SYS_CD"];
        COMMGEOBO service = new COMMGEOBO();
        COMM_DATA json = new COMM_DATA();
        try
        {
            if (SUB_CD != "")
            {
                System.Data.DataTable dt = service.getCommData(SUB_CD, MAIN_CD, SYS_CD);
                if (dt.Rows.Count > 0)
                {
                    json.SYS_CD = dt.Rows[0]["SYS_CD"].ToString();
                    json.MAIN_CD = dt.Rows[0]["MAIN_CD"].ToString();
                    json.SUB_CD = dt.Rows[0]["SUB_CD"].ToString();
                    json.SUB_DESC = dt.Rows[0]["SUB_DESC"].ToString();
                    
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

    private class COMM_DATA
    {
        public string SYS_CD { get; set; }
        public string MAIN_CD { get; set; }
        public string SUB_CD { get; set; }
        public string SUB_DESC { get; set; }
        public string errMsg { get; set; }
    }

}