<%@ WebHandler Language="C#" Class="WFB2SI0200_ExecuteCheck" %>

using System;
using System.Web;

public class WFB2SI0200_ExecuteCheck : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string bonus_year = context.Request.QueryString["BONUS_YEAR"];
        string bonus_days = context.Request.QueryString["BONUS_DAYS"];
        CFB2SI0200DAO fb2si = new CFB2SI0200DAO();
        JSON_DATA json = new JSON_DATA();
        try
        {

            fb2si.GetData_H(bonus_year, bonus_days);
            fb2si.GetData_D(bonus_year);
            //if (dt.Rows.Count > 0)
            //{
            string H_BONUS_YEAR = fb2si.BONUS_YEAR_H;
            string D_BONUS_YEAR = fb2si.BONUS_YEAR_D;
            string H_GEN_DT = fb2si.GEN_DT;
            string H_FREEZE_FLAG = fb2si.FREEZE_FLAG;

            json.hid_H_BONUS_YEAR = H_BONUS_YEAR;
            json.hid_D_BONUS_YEAR = D_BONUS_YEAR;
            json.hid_H_GEN_DT = H_GEN_DT;
            json.hid_H_FREEZE_FLAG = H_FREEZE_FLAG;
            json.errMsg = "";

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
        //取回來的資料
        public string hid_H_BONUS_YEAR { get; set; }
        public string hid_D_BONUS_YEAR { get; set; }
        public string hid_H_FREEZE_FLAG { get; set; }
        public string hid_H_GEN_DT { get; set; }

        //顯示的錯誤訊息
        public string errMsg { get; set; }
    }

}