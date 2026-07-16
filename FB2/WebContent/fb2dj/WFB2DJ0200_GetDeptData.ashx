<%@ WebHandler Language="C#" Class="WFB2DJ0200" %>

using System;
using System.Web;

public class WFB2DJ0200 : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string dept_no = context.Request.QueryString["DEPT_NO"];
        CFB2DJ0200BO dj020BO = new CFB2DJ0200BO();
        DEPT_DATA json = new DEPT_DATA();
        try
        {
            if (dept_no != "")
            {
                System.Data.DataTable dt = dj020BO.getDeptData(dept_no);
                if (dt.Rows.Count > 0)
                {
                    json.DEPT_NO = dt.Rows[0]["DEPT_NO"].ToString();
                    json.DEPT_NAME = dt.Rows[0]["DEPT_NAME"].ToString();
                    json.errMsg = "";
                }
                else
                {
                    json.DEPT_NAME = "";
                    json.errMsg = "此部門代號不存在";
                }

            }
            else
            {
                json.DEPT_NAME = "";
                json.errMsg = "此部門代號不存在";

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

    private class DEPT_DATA
    {
        //取回來的資料
        public string DEPT_NO { get; set; }
        public string DEPT_NAME { get; set; }

        //顯示的錯誤訊息
        public string errMsg { get; set; }
    }

}