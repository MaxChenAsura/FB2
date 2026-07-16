<%@ WebHandler Language="C#" Class="WFB2SH0300_ExecuteCheck" %>

using System;
using System.Web;
using System.Data;

public class WFB2SH0300_ExecuteCheck : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string remit_dt = context.Request.QueryString["remit_dt"];
        string pay_kind = context.Request.QueryString["pay_kind"];
        string salary_account_bank = context.Request.QueryString["salary_account_bank"];
        string pay_id = context.Request.QueryString["pay_id"];
        CFB2SD1200BO service = new CFB2SD1200BO();
        
        JSON_DATA json = new JSON_DATA();
        try
        {

            CFB2SD1200DAO fb2sd = new CFB2SD1200DAO();
            //檢查資料
            DataTable dt = fb2sd.chkHaveData(remit_dt, pay_kind, salary_account_bank, pay_id);
            if (dt.Rows.Count > 0 && Convert.ToInt16(dt.Rows[0]["CNT"].ToString())>0)
                json.errMsg = service.CheckData(fb2sd, remit_dt, pay_kind);
            else
                json.errMsg = "無匯出資料!";

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

    private class JSON_DATA
    {

        //顯示的錯誤訊息
        public string errMsg { get; set; }
    }

}