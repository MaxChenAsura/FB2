<%@ WebHandler Language="C#" Class="WFBCOMM" %>

using System;
using System.Web;

public class WFBCOMM : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string COMPANY_CD = context.Request.QueryString["COMPANY_CD"];
        COMMBO service = new COMMBO();
        TB_H_M_COMPANY json = new TB_H_M_COMPANY();
        try
        {
            if (COMPANY_CD != "")
            {
                System.Data.DataTable dt = service.getCOMPANY(COMPANY_CD);
                if (dt.Rows.Count > 0)
                {
                    json.COMPANY_CD = dt.Rows[0]["COMPANY_CD"].ToString();
                    json.COMPANY_SNAME = dt.Rows[0]["COMPANY_SNAME"].ToString();
                    json.HEALTH_ORG_ID = dt.Rows[0]["HEALTH_ORG_ID"].ToString();
                    json.LABOR_ORG_ID = dt.Rows[0]["LABOR_ORG_ID"].ToString();
                    json.errMsg = "";
                }
                else
                {
                    json.errMsg = "此代碼未存於公司檔";
                }

            }
            else
            {
                json.errMsg = "此代碼未存於公司檔";

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

    private class TB_H_M_COMPANY
    {
        public string COMPANY_CD { get; set; }
        public string COMPANY_SNAME { get; set; }
        public string HEALTH_ORG_ID { get; set; }
        public string LABOR_ORG_ID { get; set; }
       
        public string errMsg { get; set; }
    }

}