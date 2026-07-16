<%@ WebHandler Language="C#" Class="WFB2GetPjobData" %>

using System;
using System.Web;

public class WFB2GetPjobData : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string pjob_cd = context.Request.QueryString["PJOB_CD"];
        string start_dt = context.Request.QueryString["START_DT"];
        COMMGEOBO service = new COMMGEOBO();
        PJOB_DATA json = new PJOB_DATA();        
        try
        {
            if (pjob_cd != "")
            {
                System.Data.DataTable dt = service.getPJOBDATA(pjob_cd, start_dt);
                if (dt.Rows.Count > 0)
                {
                    json.PJOB_CD = dt.Rows[0]["PJOB_CD"].ToString();
                    json.PJOB_DESC = dt.Rows[0]["PJOB_DESC"].ToString();
                    json.LEVEL_CD = dt.Rows[0]["LEVEL_CD"].ToString();
                    json.WS_CD = dt.Rows[0]["WS_CD"].ToString();
                    json.errMsg = "";
                }
                else
                {
                    json.PJOB_DESC = "";
                    json.errMsg = "此職務代號不存在";
                }

            }
            else
            {
                json.PJOB_DESC = "";
                json.errMsg = "此職務代號不存在";

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

    private class PJOB_DATA
    {
        //取回來的資料
        public string PJOB_CD { get; set; }
        public string PJOB_DESC { get; set; }
        public string LEVEL_CD { get; set; }
        public string WS_CD { get; set; }
        
        //顯示的錯誤訊息
        public string errMsg { get; set; }
    }

}