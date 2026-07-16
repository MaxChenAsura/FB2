using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2DD0200DAO 的摘要描述
/// </summary>
public class CFB2DD0200DAO : BaseDAO
{

    public string EMP_ID { get; set; }
    public string MANAGER_YM { get; set; }
    public string ALLOWANCE_CD { get; set; }
    public string TOTAL_PAY { get; set; }
    public string WORKING_DT { get; set; }
    public string START_DT { get; set; }
    public string BELONG_TO_DT { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    public string DAILY_PAY { get; set; }
    public string lastMonth { get; set; }
    public string LEAVE_DT { get; set; }
    public string APPLICATION_NO { get; set; }
    public string END_DT { get; set; }

    public string START_DT_TWO { get; set; }
    public string END_DT_TWO { get; set; }
    public string DAILY_PAY_TWO { get; set; }
    public string APPLICATION_NO_TWO { get; set; }

    //薪資計算
    public string YM { get; set; }
    public string SALARY_DT { get; set; }
    public string SALARY_YM { get; set; }
    public string SALARY_TYPE { get; set; }
    public string SALARY_LOCKED { get; set; }
    public string TAKE_OUT_BY { get; set; }
    public string SALARY_SDT { get; set; }
    public string SALARY_EDT { get; set; }
    public string OPERATION_ID { get; set; }
    
    
	public CFB2DD0200DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable getMaxDate()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select MAX(MANAGER_YM) MANAGER_YM From TB_D_R_TRANS_MONTH_D");
            

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getManagerDT(string MANAGER_YM)
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select distinct(CONVERT(char(10),TAKE_OUT_DT, 120)) TAKE_OUT_DT,STATUS From TB_D_R_TRANS_MONTH_D Where MANAGER_YM = @MANAGER_YM");
            ht.Add("@MANAGER_YM", MANAGER_YM);

            DataTable dt = dbConn.Query(sb, ht);
           

            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getEMP(string MANAGER_YM)
    {       
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select distinct(a.EMP_ID),CONVERT(char(10),c.JOIN_DT, 120) JOIN_DT,CONVERT(char(10),c.LEAVE_DT, 120) LEAVE_DT");
            sb.Append(" From TB_D_M_TRANS_ALLOWANCE_M a ");
            sb.Append(" left join (select distinct EMP_ID,START_DT,DAILY_PAY,END_DT from TB_D_M_TRANS_ALLOWANCE_D ");
            sb.Append("			where left(CONVERT(varchar,START_DT,112),6)<= @MANAGER_YM and left(CONVERT(varchar,END_DT,112),6) >= @MANAGER_YM ) b ");
            sb.Append(" on a.EMP_ID = b.EMP_ID");
            sb.Append(" left join TB_H_M_EMP c");
            sb.Append(" on a.EMP_ID = c.EMP_ID");            
            sb.Append(" where a.IS_CANCEL ='N' and a.IS_CALCULATE = '1'");
            sb.Append(" and b.DAILY_PAY > '0'");
            //sb.Append(" and a.EMP_ID= '11516'");

            ht.Add("@MANAGER_YM", MANAGER_YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public void insertTRANS_MONTH_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();            
            

            sb.Append(" insert into TB_D_R_TRANS_MONTH_D");
            sb.Append(" (MANAGER_YM,EMP_ID,EMP_NAME,LEVEL_CD,AGE,ALLOWANCE_CD,TOTAL_PAY,DAILY_PAY,WORKING_DT,START_DT,");
            sb.Append("FACTORY_CD,AREA_CD,TRANSPORT_CD,KILOMETER_AMOUNT,FARE_PRICE,SINGLE_TRIP,LINE_CD,STATION_CD,");
            sb.Append("BELONG_TO_DT,IS_PAY,TAKE_OUT_DT,TAKE_OUT_BY,STATUS,GIVE_SALARY_DT,CREATED_BY,CREATED_DT,UPDATED_BY,");
            sb.Append("UPDATED_DT,FUNC_ID)");
            sb.Append(" select @MANAGER_YM,@EMP_ID,b.EMP_NAME,b.LEVEL_CD,b.AGE,@ALLOWANCE_CD,@TOTAL_PAY,a.DAILY_PAY,@WORKING_DT,CONVERT(char(20),@START_DT, 120),");
            sb.Append("a.FACTORY_CD,a.AREA_CD,a.TRANSPORT_CD,a.KILOMETER_AMOUNT,a.FARE_PRICE,a.SINGLE_TRIP,a.LINE_CD,a.STATION_CD,");
            sb.Append("@BELONG_TO_DT,'Y','9999/12/31','','N','9999/12/31',@CREATED_BY,getdate(),@UPDATED_BY,");
            sb.Append("getdate(),@FUNC_ID");
            sb.Append(" from TB_D_M_TRANS_ALLOWANCE_D a ");
            sb.Append(" left join VW_H_EMP_DATA b on a.emp_id = b.emp_id");
            sb.Append(" where a.EMP_ID = @EMP_ID and APPLICATION_NO = @APPLICATION_NO ");
            sb.Append(" and CONVERT(varchar, a.START_DT,111) = @SDT1 and CONVERT(char(10),a.END_DT, 120) = @END_DT");

            ht.Add("@MANAGER_YM", MANAGER_YM);
            ht.Add("@APPLICATION_NO", APPLICATION_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@ALLOWANCE_CD", ALLOWANCE_CD);
            ht.Add("@TOTAL_PAY", TOTAL_PAY);
            ht.Add("@WORKING_DT", WORKING_DT);
            ht.Add("@START_DT", DateTime.Parse(START_DT).ToString("yyyy/MM/dd HH:mm:ss"));
            ht.Add("@BELONG_TO_DT", BELONG_TO_DT);
            ht.Add("@SDT1", DateTime.Parse(START_DT).ToString("yyyy/MM/dd"));
            ht.Add("@END_DT", END_DT);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
          

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public string getStartDT()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //之後務必先確認抓最新一筆生效日的邏輯再改
            sb.Append("Select START_DT,DAILY_PAY,CONVERT(char(10),END_DT, 120) END_DT,APPLICATION_NO,");
            sb.Append(" ROW_NUMBER() OVER (PARTITION BY EMP_ID  ORDER BY EMP_ID,CONVERT(varchar(10),START_DT, 112) + CONVERT(varchar(10),END_DT, 112) DESC) WKROW ");
            sb.Append(" From TB_D_M_TRANS_ALLOWANCE_D");
            sb.Append(" where EMP_ID = @EMP_ID and LEFT(CONVERT(varchar,START_DT,112),6) <= @MANAGER_YM");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@MANAGER_YM", MANAGER_YM);

            //sb.Append("Select b.START_DT,a.DAILY_PAY,CONVERT(char(10),a.END_DT, 120) END_DT,a.APPLICATION_NO From TB_D_M_TRANS_ALLOWANCE_D a");
            //sb.Append(" join (select MAX(START_DT)START_DT,MAX(case when isnull(END_DT,'') ='9999/12/31' then END_DT END )END_DT  from TB_D_M_TRANS_ALLOWANCE_D where EMP_ID = @EMP_ID) b ");
            //sb.Append(" on a.START_DT = b.START_DT and a.END_DT = b.END_DT");
            //sb.Append(" where EMP_ID = @EMP_ID ");
            //ht.Add("@EMP_ID", EMP_ID);

            //sb.Append("Select a.START_DT,a.DAILY_PAY,CONVERT(char(10),a.END_DT, 120) END_DT,a.APPLICATION_NO From TB_D_M_TRANS_ALLOWANCE_D a");            
            //sb.Append(" join (select MAX(case when isnull(END_DT,'') ='9999/12/31' then END_DT END )END_DT  from TB_D_M_TRANS_ALLOWANCE_D where EMP_ID = @EMP_ID) b ");
            //sb.Append(" on a.END_DT = b.END_DT");            
            //sb.Append(" where EMP_ID = @EMP_ID ");
            //ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.QueryT(sb, ht);

            if(dt.Rows.Count > 0){
                st = dt.Rows[0]["START_DT"].ToString();
                APPLICATION_NO = dt.Rows[0]["APPLICATION_NO"].ToString();
                DAILY_PAY = dt.Rows[0]["DAILY_PAY"].ToString();
                END_DT = dt.Rows[0]["END_DT"].ToString();
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public string getStartDT_TWO()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            sb.Append("Select START_DT,DAILY_PAY,CONVERT(char(10),END_DT, 120) END_DT,APPLICATION_NO,");
            sb.Append(" ROW_NUMBER() OVER (PARTITION BY EMP_ID  ORDER BY EMP_ID,CONVERT(varchar(10),START_DT, 112) + CONVERT(varchar(10),END_DT, 112) DESC) WKROW ");
            sb.Append(" From TB_D_M_TRANS_ALLOWANCE_D");
            sb.Append(" where EMP_ID = @EMP_ID and LEFT(CONVERT(varchar,START_DT,112),6) <= @MANAGER_YM");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@MANAGER_YM", MANAGER_YM);
            //sb.Append("Select a.START_DT,a.DAILY_PAY,CONVERT(char(10),a.END_DT, 120) END_DT,a.APPLICATION_NO From TB_D_M_TRANS_ALLOWANCE_D a");
            //sb.Append(" join (select MAX(START_DT)START_DT from TB_D_M_TRANS_ALLOWANCE_D where EMP_ID = @EMP_ID ");            
            //sb.Append(" and START_DT < (select max(START_DT) from TB_D_M_TRANS_ALLOWANCE_D where EMP_ID = @EMP_ID) ) b");
            //sb.Append(" on a.START_DT = b.START_DT");
            //sb.Append(" where EMP_ID = @EMP_ID ");
            //ht.Add("@EMP_ID", EMP_ID);
            

            DataTable dt = dbConn.QueryT(sb, ht);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows.Count >= 2)
                {
                    st = dt.Rows[1]["END_DT"].ToString();
                    APPLICATION_NO_TWO = dt.Rows[1]["APPLICATION_NO"].ToString();
                    DAILY_PAY_TWO = dt.Rows[1]["DAILY_PAY"].ToString();
                    START_DT_TWO = dt.Rows[1]["START_DT"].ToString();
                    
                    //END_DT_TWO = dt.Rows[0]["END_DT"].ToString();
                }
                else
                {
                    st = "";
                    APPLICATION_NO_TWO = "";
                    DAILY_PAY_TWO = "";
                    START_DT_TWO = "";
                    //END_DT_TWO = dt.Rows[1]["END_DT"].ToString();
                }
                
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public string getWORK_DT(string flag)
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT COUNT(CALENDAR_DT) DT");
            sb.Append(" FROM TB_D_M_EMP_DAY_DUTY");
            sb.Append(" where EMP_ID = @EMP_ID");
            sb.Append(" and WORK_DAY_CD = '1'");            
            sb.Append(" and (select CONVERT(char(4),YEAR(CONVERT(char(10),CALENDAR_DT, 120)))+ (select case when CONVERT(char(4),MONTH(CONVERT(char(10),CALENDAR_DT, 120)))< 10" + 
                      " then '0' + CONVERT(char(4),MONTH(CONVERT(char(10),CALENDAR_DT, 120)))"+ 
                      " else CONVERT(char(4),MONTH(CONVERT(char(10),CALENDAR_DT, 120)))"+
                      " end)) = @lastMonth");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@lastMonth", lastMonth);
            if (flag =="N")
            {
                sb.Append(" and CALENDAR_DT < @LEAVE_DT");
                ht.Add("@LEAVE_DT", LEAVE_DT);
            }           
            


            DataTable dt = dbConn.QueryT(sb, ht);

            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["DT"].ToString();                
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public string getWORK_DT_NEW(string flag)
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT COUNT(CALENDAR_DT) DT");
            sb.Append(" FROM TB_D_M_EMP_DAY_DUTY");
            sb.Append(" where EMP_ID = @EMP_ID");
            sb.Append(" and WORK_DAY_CD = '1'");
            sb.Append(" and (select CONVERT(char(4),YEAR(CONVERT(char(10),CALENDAR_DT, 120)))+ (select case when CONVERT(char(4),MONTH(CONVERT(char(10),CALENDAR_DT, 120)))< 10" +
                      " then '0' + CONVERT(char(4),MONTH(CONVERT(char(10),CALENDAR_DT, 120)))" +
                      " else CONVERT(char(4),MONTH(CONVERT(char(10),CALENDAR_DT, 120)))" +
                      " end)) = @MANAGER_YM");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@MANAGER_YM", MANAGER_YM);
            if (flag == "N")
            {
                sb.Append(" and CALENDAR_DT < @LEAVE_DT");
                ht.Add("@LEAVE_DT", LEAVE_DT);
            }



            DataTable dt = dbConn.QueryT(sb, ht);

            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["DT"].ToString();
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public string getWORK_DT_Normal(string flag,string minDT)
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT COUNT(CALENDAR_DT) DT");
            sb.Append(" FROM TB_D_M_EMP_DAY_DUTY");
            sb.Append(" where EMP_ID = @EMP_ID");
            sb.Append(" and WORK_DAY_CD = '1'");
            sb.Append(" and (select CONVERT(char(4),YEAR(CONVERT(char(10),CALENDAR_DT, 120)))+ (select case when CONVERT(char(4),MONTH(CONVERT(char(10),CALENDAR_DT, 120)))< 10" +
                      " then '0' + CONVERT(char(4),MONTH(CONVERT(char(10),CALENDAR_DT, 120)))" +
                      " else CONVERT(char(4),MONTH(CONVERT(char(10),CALENDAR_DT, 120)))" +
                      " end)) = @MANAGER_YM");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@MANAGER_YM", MANAGER_YM);
            if (flag == "N")
            {
                sb.Append(" and CALENDAR_DT < @minDT");
                ht.Add("@minDT", minDT);
            }



            DataTable dt = dbConn.QueryT(sb, ht);

            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["DT"].ToString();
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public void delOld()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Delete from TB_D_R_TRANS_MONTH_D");
            sb.Append(" where MANAGER_YM = @MANAGER_YM");

            if (MANAGER_YM != "")            {
               
                ht.Add("@MANAGER_YM", MANAGER_YM);
            }
            //if (EMP_ID != "")
            //{
            //    sb.Append(" and  EMP_ID = @EMP_ID ");
            //    ht.Add("@EMP_ID", EMP_ID);
            //}
            dbConn.ExecuteT(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string manager_ym_s, string manager_ym_e)
    {
        try
        {            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" MANAGER_YM,(select CASE WHEN CONVERT(char(10),TAKE_OUT_DT, 120) ='9999-12-31' THEN '' ELSE REPLACE(CONVERT(char(10), TAKE_OUT_DT, 120),'-','/') END) TAKE_OUT_DT,STATUS,REMARK");
            sb.Append(" from (select distinct(MANAGER_YM) MANAGER_YM,TAKE_OUT_DT,STATUS,REMARK from TB_D_R_TRANS_MONTH_D where MANAGER_YM between @manager_ym_s and @manager_ym_e ) a");
            sb.Append(" where 1=1");

            if (manager_ym_s != "" && manager_ym_e !="")
            {
                sb.Append(" and MANAGER_YM between @manager_ym_s and @manager_ym_e ");
                ht.Add("@manager_ym_s", manager_ym_s.Replace("/",""));
                ht.Add("@manager_ym_e", manager_ym_e.Replace("/", ""));
            }
           
            
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows, string manager_ym_s, string manager_ym_e)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from (select distinct(MANAGER_YM) from TB_D_R_TRANS_MONTH_D where MANAGER_YM between @manager_ym_s and @manager_ym_e) b");
            sb.Append(" where 1=1");

            if (manager_ym_s != "" && manager_ym_e != "")
            {
                sb.Append(" and MANAGER_YM between @manager_ym_s and @manager_ym_e ");
                ht.Add("@manager_ym_s", manager_ym_s.Replace("/", ""));
                ht.Add("@manager_ym_e", manager_ym_e.Replace("/", ""));
            }            

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total_record"];
            }


            return t;


        }
        catch (Exception)
        {

            throw;
        }
    }


    public DataTable getSalaryCode()
    {       
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT SALARY_YM,CONVERT(char(10),SALARY_DT, 120) SALARY_DT,SALARY_TYPE,CONVERT(char(10),SALARY_SDT, 120)SALARY_SDT,CONVERT(char(10),SALARY_EDT, 120)SALARY_EDT");
            sb.Append(" FROM TB_S_M_SALARY_CAL_H");
            sb.Append(" where SALARY_YM = @MANAGER_YM");
            sb.Append(" and SALARY_TYPE = 'A'");

            ht.Add("@MANAGER_YM", YM);
           
            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getSalaryCTL()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT SALARY_LOCKED");
            sb.Append(" FROM TB_S_M_SALARY_MONTH_CTRL");
            sb.Append(" where OPERATION_ID = 'F01' and SALARY_YM = @SALARY_YM");
            sb.Append(" and SALARY_DT = @SALARY_DT");
            sb.Append(" and SALARY_TYPE = @SALARY_TYPE");

            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable checkSalaryClose()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT SALARY_LOCKED FROM TB_S_M_SALARY_MONTH_CTRL a");
            sb.Append(" left join TB_S_M_SALARY_CAL_H b");
            sb.Append(" on a.SALARY_YM = b.SALARY_YM and a.SALARY_DT = b.SALARY_DT and a.SALARY_TYPE = b.SALARY_TYPE");
            sb.Append(" and b.SALARY_TYPE = 'A'");
            sb.Append(" where a.OPERATION_ID = 'F01' and a.SALARY_YM = @SALARY_YM");

            ht.Add("@SALARY_YM", YM);
           
            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable updateTRANS_MONTH()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_R_TRANS_MONTH_D");
            sb.Append(" set TAKE_OUT_DT = getdate(),");
            sb.Append(" TAKE_OUT_BY = @TAKE_OUT_BY,");
            sb.Append(" STATUS = 'Y',");
            sb.Append(" GIVE_SALARY_DT = @SALARY_DT,");
            sb.Append(" REMARK = '本月份已經結帳，請至【其他加扣款資料維護】上傳資料',");
            sb.Append(" UPDATED_BY = @TAKE_OUT_BY,");
            sb.Append(" UPDATED_DT = getdate()");           
            sb.Append(" where MANAGER_YM =@MANAGER_YM");


            ht.Add("@TAKE_OUT_BY", TAKE_OUT_BY);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@MANAGER_YM", YM); 

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public void deleteSALARY_MONTH_CTRL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();



            sb.Append(" delete from TB_S_M_SALARY_MONTH_CTRL");
            sb.Append(" where OPERATION_ID = 'F01' and SALARY_YM = @SALARY_YM");

            ht.Add("@SALARY_YM", MANAGER_YM);
            
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insertSALARY_MONTH_CTRL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();



            sb.Append(" insert into TB_S_M_SALARY_MONTH_CTRL");
            sb.Append(" (SALARY_TYPE,SALARY_YM,SALARY_DT,OPERATION_ID,PROCESS_DT,START_DT,END_DT,SALARY_LOCKED,LOCK_DT,FUNC_ID)");
            sb.Append(" values(@SALARY_TYPE,@SALARY_YM,@SALARY_DT,@OPERATION_ID,getdate(),@START_DT,@END_DT,'','9999/12/31',@FUNC_ID)");


            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@OPERATION_ID", OPERATION_ID);
            ht.Add("@START_DT", SALARY_SDT);
            ht.Add("@END_DT", SALARY_EDT);
            ht.Add("@FUNC_ID",FUNC_ID);
           


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable updateTRANS_MONTH_FIN()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_R_TRANS_MONTH_D");
            sb.Append(" set TAKE_OUT_DT = getdate(),");
            sb.Append(" TAKE_OUT_BY = @TAKE_OUT_BY,");
            sb.Append(" STATUS = 'Y',");
            sb.Append(" GIVE_SALARY_DT = @SALARY_DT,");          
            sb.Append(" UPDATED_BY = @TAKE_OUT_BY,");
            sb.Append(" UPDATED_DT = getdate()");
            sb.Append(" where MANAGER_YM =@MANAGER_YM");


            ht.Add("@TAKE_OUT_BY", TAKE_OUT_BY);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@MANAGER_YM", YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public void updateSALARY_MONTH_CTRL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();



            sb.Append(" update TB_S_M_SALARY_MONTH_CTRL");
            sb.Append(" set PROCESS_DT = getdate(),START_DT=@START_DT,END_DT = @END_DT,FUNC_ID=@FUNC_ID");
            sb.Append(" where SALARY_TYPE = @SALARY_TYPE and SALARY_YM = @SALARY_YM and SALARY_DT = @SALARY_DT");
            sb.Append(" and OPERATION_ID = @OPERATION_ID");
           
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@OPERATION_ID", OPERATION_ID);
            ht.Add("@START_DT", SALARY_SDT);
            ht.Add("@END_DT", SALARY_EDT);
            ht.Add("@FUNC_ID", FUNC_ID);


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getDone()
    {
       
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * From TB_D_R_TRANS_MONTH_D");        
            sb.Append(" where EMP_ID = @EMP_ID");
            sb.Append(" and MANAGER_YM = @MANAGER_YM");
           
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@MANAGER_YM", lastMonth);

            DataTable dt = dbConn.Query(sb, ht);

            return dt;
           
        }
        catch
        {
            throw;
        }
    }

    public DataTable getAllLastMonth()
    {

        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select EMP_ID From TB_D_R_TRANS_MONTH_D");
            sb.Append(" where MANAGER_YM = @MANAGER_YM");
            sb.Append(" and total_pay  <> 0");  
            
            ht.Add("@MANAGER_YM", lastMonth);

            DataTable dt = dbConn.Query(sb, ht);

            return dt;

        }
        catch
        {
            throw;
        }
    }


}