<%@ WebHandler Language="C#" Class="WFB2HA0500_PJOB" %>

using System;
using System.Web;

public class WFB2HA0500_PJOB : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string PJOB_CD = context.Request.QueryString["PJOB_CD"];
        CFB2HA0500DAO ha050DAO = new CFB2HA0500DAO();
        JSON_DATA json = new JSON_DATA();
        try
        {

            System.Data.DataTable dt = ha050DAO.getPJOB_CD(PJOB_CD);
            if (dt.Rows.Count > 0)
            {
                json.PJOB_DESC = dt.Rows[0]["PJOB_DESC"].ToString();
                json.errMsg = "";
            }
            else
            {
                json.errMsg = "職務代號不存在";
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

    private class JSON_DATA
    {
        //取回來的資料
        public string PJOB_DESC { get; set; }

        //顯示的錯誤訊息
        public string errMsg { get; set; }
    }

}