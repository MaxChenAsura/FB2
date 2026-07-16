<%@ WebHandler Language="C#" Class="WFB2SG0300_GetEmpData" %>

using System;
using System.Web;

public class WFB2SG0300_GetEmpData : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string emp_id = context.Request.QueryString["EMP_ID"];
        CFB2SG0300BO sg030BO = new CFB2SG0300BO();
        EMP_DATA json = new EMP_DATA();
        try
        {
            if (emp_id != "")
            {
                System.Data.DataTable dt = sg030BO.getEmpData(emp_id);
                if (dt.Rows.Count > 0)
                {
                    json.EMP_ID = dt.Rows[0]["EMP_ID"].ToString();
                    json.EMP_NAME = dt.Rows[0]["EMP_NAME"].ToString();
                    json.EMP_CD = dt.Rows[0]["EMP_CD"].ToString();
                    json.EMP_CD_DESC = dt.Rows[0]["EMP_CD_DESC"].ToString();
                    json.LEVEL_CD = dt.Rows[0]["LEVEL_CD"].ToString();
                    json.JOIN_DT = dt.Rows[0]["JOIN_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["JOIN_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                    json.WORK_DAYS = dt.Rows[0]["WORK_DAYS"].ToString();
                    json.EMP_CHG_CD = dt.Rows[0]["EMP_CHG_CD"].ToString();
                    json.EMP_CHG_CD_DESC = dt.Rows[0]["EMP_CHG_CD_DESC"].ToString();
                    json.PJOB_CD = dt.Rows[0]["PJOB_CD"].ToString();                    
                    json.errMsg = "";
                    
                }
                else
                {
                    json.EMP_ID = "";
                    json.errMsg = "此工號不存在";
                }

            }
            else
            {
                json.EMP_ID = "";
                json.errMsg = "此工號不存在";

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

    private class EMP_DATA
    {
        //取回來的資料
        public string EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        public string EMP_CD { get; set; }
        public string EMP_CD_DESC { get; set; }
        public string LEVEL_CD { get; set; }
        public string JOIN_DT { get; set; }
        public string WORK_DAYS { get; set; }
        public string EMP_CHG_CD { get; set; }
        public string EMP_CHG_CD_DESC { get; set; }
        public string PJOB_CD { get; set; }
        //顯示的錯誤訊息
        public string errMsg { get; set; }
    }

}