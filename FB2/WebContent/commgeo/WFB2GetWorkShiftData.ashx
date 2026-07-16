<%@ WebHandler Language="C#" Class="WFB2GetWorkShiftData" %>

using System;
using System.Web;

public class WFB2GetWorkShiftData : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string work_shift_cd = context.Request.QueryString["WORK_SHIFT_CD"];
        COMMGEOBO service = new COMMGEOBO();
        WORK_SHIFT_DATA json = new WORK_SHIFT_DATA();        
        try
        {
            if (work_shift_cd != "")
            {
                System.Data.DataTable dt = service.getWORKSHIFTDATA(work_shift_cd);
                if (dt.Rows.Count > 0)
                {
                    json.WORK_SHIFT_CD = dt.Rows[0]["WORK_SHIFT_CD"].ToString();
                    json.WORK_SHIFT_DESC = dt.Rows[0]["WORK_SHIFT_DESC"].ToString();
                    json.CALENDAR_CD = dt.Rows[0]["CALENDAR_CD"].ToString();
                    json.CALENDAR_DESC = dt.Rows[0]["CALENDAR_CD"].ToString() +" "+dt.Rows[0]["CALENDAR_DESC"].ToString();
                    json.errMsg = "";
                }
                else
                {
                    json.WORK_SHIFT_DESC = "";
                    json.errMsg = "此輪值表代號不存在";
                }

            }
            else
            {
                json.WORK_SHIFT_DESC = "";
                json.errMsg = "此輪值表代號不存在";

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

    private class WORK_SHIFT_DATA
    {
        //取回來的資料
        public string WORK_SHIFT_CD { get; set; }
        public string WORK_SHIFT_DESC { get; set; }
        public string CALENDAR_CD { get; set; }
        public string CALENDAR_DESC { get; set; }
        //顯示的錯誤訊息
        public string errMsg { get; set; }
    }

}