<%@ WebHandler Language="C#" Class="WFB2SJ0260GetEmpData" %>

using System;
using System.Web;

public class WFB2SJ0260GetEmpData : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string ASSESS_YEAR = context.Request.QueryString["ASSESS_YEAR"];
        string ASSESS_TYPE = context.Request.QueryString["ASSESS_TYPE"];
        string EMP_ID = context.Request.QueryString["EMP_ID"];
        
        EMP_DATA json = new EMP_DATA();
        try
        {
            if (EMP_ID != "")
            {
                CFB2SJ0260DAO sj0260Dao = new CFB2SJ0260DAO();
                sj0260Dao.ASSESS_YEAR = ASSESS_YEAR;
                sj0260Dao.ASSESS_TYPE = ASSESS_TYPE;
                sj0260Dao.EMP_ID = EMP_ID;
                System.Data.DataTable dt = sj0260Dao.getEmpDeptData();
                
                if (dt.Rows.Count > 0)
                {
                    json.EMP_NAME = dt.Rows[0]["EMP_NAME"].ToString().Trim();
                    json.DEPT_NO = dt.Rows[0]["DEPT_NO"].ToString();
                    json.DEPT_NAME = dt.Rows[0]["DEPT_NAME"].ToString();
                    json.DEPT_FULL_NAME = dt.Rows[0]["DEPT_FULL_NAME"].ToString();
                    json.HEAD_EMP_ID = dt.Rows[0]["HEAD_EMP_ID"].ToString();
                    json.HEAD_EMP_NAME = dt.Rows[0]["HEAD_EMP_NAME"].ToString();
                    json.errMsg = "";
                }
                else
                {
                    json.errMsg = "此工號未存於人事主檔";
                }

            }
            else
            {
                json.errMsg = "此工號未存於人事主檔";

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