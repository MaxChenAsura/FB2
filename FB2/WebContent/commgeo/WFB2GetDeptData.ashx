<%@ WebHandler Language="C#" Class="WFB2GetDeptData" %>

using System;
using System.Web;

public class WFB2GetDeptData : IHttpHandler
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
                    json.HEAD_EMP_ID = dt.Rows[0]["HEAD_EMP_ID"].ToString();
                    json.HEAD_EMP_NAME = dt.Rows[0]["HEAD_EMP_NAME"].ToString();

                    json.DEPT_NO_20 = dt.Rows[0]["DEPT_NO_20"].ToString();
                    json.DEPT_NAME_20 = dt.Rows[0]["DEPT_NAME_20"].ToString();
                    json.DEPT_NO_30 = dt.Rows[0]["DEPT_NO_30"].ToString();
                    json.DEPT_NAME_30 = dt.Rows[0]["DEPT_NAME_30"].ToString();
                    json.DEPT_NO_40 = dt.Rows[0]["DEPT_NO_40"].ToString();
                    json.DEPT_NAME_40 = dt.Rows[0]["DEPT_NAME_40"].ToString();
                    json.DEPT_NO_50 = dt.Rows[0]["DEPT_NO_50"].ToString();
                    json.DEPT_NAME_50 = dt.Rows[0]["DEPT_NAME_50"].ToString();
                    json.DEPT_NO_60 = dt.Rows[0]["DEPT_NO_60"].ToString();
                    json.DEPT_NAME_60 = dt.Rows[0]["DEPT_NAME_60"].ToString();
                    json.DEPT_NO_70 = dt.Rows[0]["DEPT_NO_70"].ToString();
                    json.DEPT_NAME_70 = dt.Rows[0]["DEPT_NAME_70"].ToString();
                    json.DEPT_FULL_NAME = dt.Rows[0]["DEPT_FULL_NAME"].ToString();
                    json.DIV_DEPT_FULL_NAME = dt.Rows[0]["DIV_DEPT_FULL_NAME"].ToString();
                    json.errMsg = "";
                }
                else
                {
                    json.DEPT_NAME = "";
                    json.errMsg = "此部門代號不存在或已失效";
                }

            }
            else
            {
                json.DEPT_NAME = "";
                json.errMsg = "此部門代號不存在或已失效";

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
        public string HEAD_EMP_ID { get; set; }
        public string HEAD_EMP_NAME { get; set; }

        public string DEPT_NO_20 { get; set; }
        public string DEPT_NAME_20 { get; set; }
        public string DEPT_NO_30 { get; set; }
        public string DEPT_NAME_30 { get; set; }
        public string DEPT_NO_40 { get; set; }
        public string DEPT_NAME_40 { get; set; }
        public string DEPT_NO_50 { get; set; }
        public string DEPT_NAME_50 { get; set; }
        public string DEPT_NO_60 { get; set; }
        public string DEPT_NAME_60 { get; set; }
        public string DEPT_NO_70 { get; set; }
        public string DEPT_NAME_70 { get; set; }
        public string DEPT_FULL_NAME { get; set; }
        public string DIV_DEPT_FULL_NAME { get; set; }
        //顯示的錯誤訊息
        public string errMsg { get; set; }
    }

}