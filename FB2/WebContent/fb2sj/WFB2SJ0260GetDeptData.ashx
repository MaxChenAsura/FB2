<%@ WebHandler Language="C#" Class="WFB2SJ0260GetDeptData" %>

using System;
using System.Web;

public class WFB2SJ0260GetDeptData : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string DEPT_NO = context.Request.QueryString["DEPT_NO"];
        
        EMP_DATA json = new EMP_DATA();
        try
        {
            if (DEPT_NO != "")
            {
                CFB2SJ0260DAO sj0260Dao = new CFB2SJ0260DAO();
                sj0260Dao.DEPT_NO = DEPT_NO;
                System.Data.DataTable dt = sj0260Dao.getDeptData();
                
                if (dt.Rows.Count > 0)
                {
                   
                    json.DEPT_NAME = dt.Rows[0]["DEPT_NAME"].ToString();
                    json.DEPT_FULL_NAME = dt.Rows[0]["DEPT_FULL_NAME"].ToString();
                    json.HEAD_EMP_ID = dt.Rows[0]["HEAD_EMP_ID"].ToString();
                    json.HEAD_EMP_NAME = dt.Rows[0]["HEAD_EMP_NAME"].ToString();
                    json.errMsg = "";
                }
                else
                {
                    json.errMsg = "此部門編號未存於部門主檔";
                }

            }
            else
            {
                json.errMsg = "此部門編號未存於部門主檔";

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
        public string EMP_NAME { get; set; }
        public string EMP_CD { get; set; }
        public string SUB_DESC { get; set; }
        public string DEPT_NO { get; set; }
        public string DEPT_NAME { get; set; }
        public string PJOB_DESC { get; set; }
        public string LEVEL_CD { get; set; }
        public string WORK_SHIFT_CD { get; set; }
        public string JOIN_DT { get; set; }
        public string REGISTER_ADDR { get; set; }
        public string CONTACT_ADDR { get; set; }
        public string MOBILE_TEL_1 { get; set; }
        public string CONTACT_TEL { get; set; }
        public string AGE { get; set; }
        public string PLANT_CD { get; set; }
        public string PLANT_NAME { get; set; }
        public string WORK_SHIFT_NAME { get; set; }
        public string LICENSE_ID { get; set; }
        public string BIRTH_DT { get; set; }
        public string LEAVE_DT { get; set; }
        public string LINE_CD { get; set; }
        public string LINE_NAME { get; set; }
        public string EMP_STATUS { get; set; }
        public string EMP_STATUS_DESC { get; set; }
        public string DEPT_NAME_20 { get; set; }
        public string DEPT_NAME_30 { get; set; }
        public string DEPT_NAME_40 { get; set; }
        public string DEPT_FULL_NAME { get; set; }
        public string DIV_DEPT_FULL_NAME { get; set; }
        public string WORK_CD { get; set; }
        public string HEAD_EMP_ID { get; set; }
        public string HEAD_EMP_NAME { get; set; }

        public string WORK_CD_DESC { get; set; }
        public string errMsg { get; set; }
    }

}