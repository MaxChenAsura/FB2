<%@ WebHandler Language="C#" Class="WFB2DD0100_calculateMoney" %>

using System;
using System.Web;

public class WFB2DD0100_calculateMoney : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string FACTORY_CD = context.Request.QueryString["FACTORY_CD"];
        string TRANSPORT_CD = context.Request.QueryString["TRANSPORT_CD"];
        string KILOMETER_AMOUNT = context.Request.QueryString["KILOMETER_AMOUNT"];
        string FARE_PRICE = context.Request.QueryString["FARE_PRICE"];
        string SINGLE_TRIP = context.Request.QueryString["SINGLE_TRIP"];
        string trans = "";
        System.Data.DataTable dt = new System.Data.DataTable();
        System.Data.DataTable dt1 = new System.Data.DataTable();
        EMP_DATA json = new EMP_DATA();
                
        try
        {

            if (TRANSPORT_CD == "01" || TRANSPORT_CD == "05" || TRANSPORT_CD == "07" || TRANSPORT_CD == "09" || TRANSPORT_CD == "12")
            {
                trans = "01";
            }

            else if (TRANSPORT_CD == "02" || TRANSPORT_CD == "06" || TRANSPORT_CD == "08" || TRANSPORT_CD == "10" || TRANSPORT_CD == "13")
            {
                trans = "02";
            }

            CFB2DD0100DAO dao = new CFB2DD0100DAO();
            dt = dao.getKM("DD", "ALLOWANCE_LTD_CD", trans, "", "");
            if (dt.Rows.Count > 0)
            {
                dao.CL_KM = dt.Rows[0]["CODE_VAL1"].ToString();//中壢  
                dao.KN_KM = dt.Rows[0]["CODE_VAL2"].ToString();//觀音 
            }
            dt1 = dao.getKM("DD", "ALLOWANCE_LTD_CD", "03", "", "");
            if (dt1.Rows.Count > 0)
            {
                dao.CL_FR = dt1.Rows[0]["CODE_VAL1"].ToString();//中壢  
                dao.KN_FR = dt1.Rows[0]["CODE_VAL2"].ToString();//觀音 
            }

            CFB2DD0100BO service = new CFB2DD0100BO();

            //塞資料進行檢核        
            dao.D_FACTORY_CD = FACTORY_CD;
            dao.D_TRANSPORT_CD = TRANSPORT_CD;
            dao.D_KILOMETER_AMOUNT = KILOMETER_AMOUNT == "" ? "0" : KILOMETER_AMOUNT;
            dao.D_FARE_PRICE = FARE_PRICE == "" ? "0" : FARE_PRICE;
            dao.D_SINGLE_TRIP = SINGLE_TRIP;

            string msg = service.CheckPara(dao);
            

            if (msg == "")
            {
                json.errMsg = msg;
                string dayMoney = "";
                dayMoney = dao.getPrice();
                //計算日津貼
                if (dao.D_TRANSPORT_CD == "01" || dao.D_TRANSPORT_CD == "02"
                    || dao.D_TRANSPORT_CD == "05" || dao.D_TRANSPORT_CD == "06")//汽車or機車 
                {

                    if (dao.D_SINGLE_TRIP == "Y")
                    {
                        json.DAILY_PAY = Convert.ToString(Convert.ToInt32(dayMoney) * Convert.ToInt32(dao.D_KILOMETER_AMOUNT) * 1);
                    }
                    else
                    {
                        json.DAILY_PAY = Convert.ToString(Convert.ToInt32(dayMoney) * Convert.ToInt32(dao.D_KILOMETER_AMOUNT) * 2);
                    }
                }

                if (dao.D_TRANSPORT_CD == "04" || dao.D_TRANSPORT_CD == "11")//大眾工具or自行車→大眾工具→交通車
                {
                    if (dao.D_SINGLE_TRIP == "Y")
                    {
                        json.DAILY_PAY = Convert.ToString(Convert.ToInt32(dao.D_FARE_PRICE) * 1);
                    }
                    else
                    {
                        json.DAILY_PAY = Convert.ToString(Convert.ToInt32(dao.D_FARE_PRICE) * 2);
                    }
                }

                if (dao.D_TRANSPORT_CD == "07" || dao.D_TRANSPORT_CD == "08" || dao.D_TRANSPORT_CD == "09"
                    || dao.D_TRANSPORT_CD == "10" || dao.D_TRANSPORT_CD == "12" || dao.D_TRANSPORT_CD == "13")
                {
                    if (dao.D_SINGLE_TRIP == "Y")
                    {
                        json.DAILY_PAY = Convert.ToString(Convert.ToInt32(dayMoney) * Convert.ToInt32(dao.D_KILOMETER_AMOUNT) * 1 + Convert.ToInt32(dao.D_FARE_PRICE) * 1);
                        //dao.D_DAILY_PAY = Convert.ToString(Convert.ToInt32(dao.D_FARE_PRICE) * 1);
                    }
                    else
                    {
                        json.DAILY_PAY = Convert.ToString(Convert.ToInt32(dayMoney) * Convert.ToInt32(dao.D_KILOMETER_AMOUNT) * 2 + Convert.ToInt32(dao.D_FARE_PRICE) * 2);
                        //dao.D_DAILY_PAY = Convert.ToString(Convert.ToInt32(dao.D_FARE_PRICE) * 2);
                    }
                }
                if (json.DAILY_PAY == "" || json.DAILY_PAY == null)
                {
                    json.DAILY_PAY = "0";
                }
            }
            else
            {
                json.errMsg = msg;
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    private class EMP_DATA
    {
        public string DAILY_PAY { get; set; }       
        public string errMsg { get; set; }
        
    }

}