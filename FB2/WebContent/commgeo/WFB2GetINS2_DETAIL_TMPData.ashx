<%@ WebHandler Language="C#" Class="WFB2GetINS2_DETAIL_TMPData" %>

using System;
using System.Web;

public class WFB2GetINS2_DETAIL_TMPData : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string EMP_ID = context.Request.QueryString["EMP_ID"];
        COMMGEOBO service = new COMMGEOBO();
        SALARY_DATA json = new SALARY_DATA();
        try
        {
            COMMGEODAO commgeo = new COMMGEODAO();
            commgeo.EMP_ID = EMP_ID;
            System.Data.DataTable dt = service.getINS2_DETAIL_TMP(commgeo);
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
        public string EMP_ID { get; set; }
        public string SALARY_TYPE { get; set; }
        public string SALARY_DT { get; set; }
        public string SALARY_YM { get; set; }
        public string errMsg { get; set; }
    }

}