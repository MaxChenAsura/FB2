<%@ WebHandler Language="C#" Class="WFB2GetLEAVECDData" %>

using System;
using System.Web;

public class WFB2GetLEAVECDData : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string main_leave_cd = context.Request.QueryString["MAIN_LEAVE_CD"];
        string sub_leave_cd = context.Request.QueryString["SUB_LEAVE_CD"];
        COMMGEOBO service = new COMMGEOBO();
        PJOB_DATA json = new PJOB_DATA();        
        try
        {
            if (string.IsNullOrEmpty(main_leave_cd)==false)
            {
                System.Data.DataTable dt = service.getMAIN_LEAVE_DESC(main_leave_cd);
                if (dt.Rows.Count > 0)
                {
                    json.MAIN_LEAVE_DESC = dt.Rows[0]["MAIN_LEAVE_DESC"].ToString();
                    json.errMsg = "";
                }
                else
                {
                    json.MAIN_LEAVE_DESC = "";
                    json.errMsg = "此主假別代號不存在";
                }

            }

            if (string.IsNullOrEmpty(sub_leave_cd) == false)
            {
                System.Data.DataTable dt = service.getSUB_LEAVE_DESC(sub_leave_cd);
                if (dt.Rows.Count > 0)
                {
                    json.SUB_LEAVE_DESC = dt.Rows[0]["SUB_LEAVE_DESC"].ToString();
                    json.LEAVE_TIME_UNIT = dt.Rows[0]["LEAVE_TIME_UNIT"].ToString();
                    json.errMsg = "";
                }
                else
                {
                    json.SUB_LEAVE_DESC ="";
                    json.LEAVE_TIME_UNIT = "";
                    json.errMsg = "此主假別代號不存在";
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

    private class PJOB_DATA
    {
        //取回來的資料
        public string MAIN_LEAVE_DESC { get; set; }
        public string LEAVE_TIME_UNIT { get; set; }
        public string SUB_LEAVE_DESC { get; set; }
        
        //顯示的錯誤訊息
        public string errMsg { get; set; }
    }

}