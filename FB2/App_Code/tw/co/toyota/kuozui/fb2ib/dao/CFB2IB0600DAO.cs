using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2IB0600DAO 的摘要描述
/// </summary>
public class CFB2IB0600DAO : BaseDAO
{
    //SCREEN PARA
    public string YEAR { get; set; }
    public string C_YEAR { get; set; }

    //Excel Import
    public string PAYMENT_DATE { get; set; }
    public string EMP_NAME { get; set; }
    public string LICENSE_ID { get; set; }
    public string CODE_CD { get; set; }
    public string NT_AMOUNT { get; set; }
    public string INS_COST { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    //TXT 
    public string COMPANY_ID { get; set; }//統一編號
    public string CHAIRMAN_NAME { get; set; }//負責人
    public string HEALTH_ORG_ID { get; set; }//健保單位代號
    public string USER_EMAIL { get; set; }
    public string USER_NAME { get; set; }
    public string USER_PHONE { get; set; }
    public string MAXYM { get; set; }
    public string MINYM { get; set; }
    public string nowDate { get; set; }


    public CFB2IB0600DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public void deleteINS2_NOT_BONUS()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" delete from TB_S_M_INS2_NOT_BONUS");
            sb.Append(" where PAYMENT_DATE = @PAYMENT_DATE and LICENSE_ID = @LICENSE_ID and CODE_CD = @CODE_CD");


            ht.Add("@PAYMENT_DATE", PAYMENT_DATE);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@CODE_CD", CODE_CD);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insertINS2_NOT_BONUS()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_S_M_INS2_NOT_BONUS");
            sb.Append(" (PAYMENT_DATE, EMP_NAME, LICENSE_ID, CODE_CD, NT_AMOUNT, INS_COST,");
            sb.Append(" CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" values(@PAYMENT_DATE, @EMP_NAME, @LICENSE_ID, @CODE_CD, @NT_AMOUNT, @INS_COST,");
            sb.Append(" @CREATED_BY, getdate(), @UPDATED_BY, getdate(), @FUNC_ID)");

            ht.Add("@PAYMENT_DATE", PAYMENT_DATE);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@CODE_CD", CODE_CD);
            ht.Add("@NT_AMOUNT", NT_AMOUNT);
            ht.Add("@INS_COST", INS_COST);
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

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string PAYMENT_DATE)
    {
        try
        {
            //    if (sortExpression.Contains("EMP_ID"))
            //    {
            //        sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            //    }
            //    if (sortExpression.Contains("EMP_NAME"))
            //    {
            //        sortExpression = sortExpression.Replace("EMP_NAME", "a.EMP_NAME");
            //    }

            //因order by 條件需要再substring，故有底下這段
            PAYMENT_DATE = Convert.ToString(Convert.ToInt32(PAYMENT_DATE) + 1911);
            string st = sortExpression;
            string od = "";
            if (st.Contains("ASC"))
            {
                st = st.Replace("ASC", "").Trim();
                od = "ORDER BY left(Convert(varchar, " + st + ",112),6) ASC";
            }
            else if (st.Contains("DESC"))
            {
                st = st.Replace("DESC", "").Trim();
                od = "ORDER BY left(Convert(varchar, " + st + ",112),6) DESC";
            }



            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(" + od + ") As RowNumber,");
            sb.Append(" left(Convert(varchar, PAYMENT_DATE,112),6)PAYMENT_DATE,SUM(INS_COST)INS_COST,'62' as INS_KIND from TB_S_M_INS2_DETAIL");
            sb.Append(" where left(Convert(varchar, PAYMENT_DATE,112),4) =@PAYMENT_DATE");
            sb.Append(" and IS_NOT_CAL = 'N'");
            sb.Append(" group by left(Convert(varchar, PAYMENT_DATE,112),6)");

            ht.Add("@PAYMENT_DATE", PAYMENT_DATE);

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
    public int getCount(int startRowIndex, int maximumRows, string PAYMENT_DATE)
    {
        try
        {
            int t = 0;
            PAYMENT_DATE = Convert.ToString(Convert.ToInt32(PAYMENT_DATE) + 1911);
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select count(*) total_record From");
            sb.Append(" (Select left(Convert(varchar, PAYMENT_DATE,112),6)PAYMENT_DATE,SUM(INS_COST)INS_COST,'62' as INS_KIND");
            sb.Append(" from TB_S_M_INS2_DETAIL");
            sb.Append(" where left(Convert(varchar, PAYMENT_DATE,112),4) =@PAYMENT_DATE");
            sb.Append(" and IS_NOT_CAL = 'N'");
            sb.Append(" group by left(Convert(varchar, PAYMENT_DATE,112),6)");
            sb.Append(" )god_data ");
            ht.Add("@PAYMENT_DATE", PAYMENT_DATE);

            //sb.Append("Select COUNT(*) total_record ");
            //sb.Append(" from TB_S_M_INS2_DETAIL");
            //sb.Append(" where left(Convert(varchar, PAYMENT_DATE,112),4) =@PAYMENT_DATE");
            //sb.Append(" and IS_NOT_CAL = 'N'");
            //sb.Append(" group by left(Convert(varchar, PAYMENT_DATE,112),6)");

            //ht.Add("@PAYMENT_DATE", PAYMENT_DATE);              


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

    public DataTable getCompany()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COMPANY_ID, CHAIRMAN_NAME, HEALTH_ORG_ID");
            sb.Append(" from TB_H_M_COMPANY");
            sb.Append(" where COMPANY_CD = 'K'");


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                COMPANY_ID = dt.Rows[0]["COMPANY_ID"].ToString();
                CHAIRMAN_NAME = dt.Rows[0]["CHAIRMAN_NAME"].ToString();
                HEALTH_ORG_ID = dt.Rows[0]["HEALTH_ORG_ID"].ToString();
            }

            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getUserEmail()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select CODE_VAL1");
            sb.Append(" from TB_9_M_PARAMETER");
            sb.Append(" where SYS_CD = 'IB' and main_cd = 'INS2_USER_EMAIL' ");


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                USER_EMAIL = dt.Rows[0]["CODE_VAL1"].ToString();
            }

            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getUserName()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select CODE_VAL1");
            sb.Append(" from TB_9_M_PARAMETER");
            sb.Append(" where SYS_CD = 'IB' and main_cd = 'INS2_USER_NAME' ");


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                USER_NAME = dt.Rows[0]["CODE_VAL1"].ToString();
            }

            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getUserPHONE()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select CODE_VAL1");
            sb.Append(" from TB_9_M_PARAMETER");
            sb.Append(" where SYS_CD = 'IB' and main_cd = 'INS2_USER_PHONE' ");


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                USER_PHONE = dt.Rows[0]["CODE_VAL1"].ToString();
            }

            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getYM()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select MAX(left(Convert(varchar, PAYMENT_DATE,112),6))MAXDAY,");
            sb.Append(" MIN(left(Convert(varchar, PAYMENT_DATE,112),6))MINDAY");
            sb.Append(" from TB_S_M_INS2_DETAIL");
            sb.Append(" where left(Convert(varchar, PAYMENT_DATE,112),4) =@C_YEAR");
            sb.Append(" and IS_NOT_CAL = 'N'");


            ht.Add("@C_YEAR", C_YEAR);


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                MAXYM = Convert.ToString(Convert.ToInt32(dt.Rows[0]["MAXDAY"].ToString().Substring(0, 4)) - 1911) +
                        dt.Rows[0]["MAXDAY"].ToString().Substring(4, 2);
                MINYM = Convert.ToString(Convert.ToInt32(dt.Rows[0]["MINDAY"].ToString().Substring(0, 4)) - 1911) +
                        dt.Rows[0]["MINDAY"].ToString().Substring(4, 2);
            }

            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable get62Data()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" 
             select a.EMP_ID,CONVERT(char(10), PAYMENT_DATE, 120)PAYMENT_DATE,b.EMP_NAME,a.LICENSE_ID,
             RIGHT(REPLICATE('0',14 ) + CAST(a.ONE_TIME_AMOUNT as VARCHAR) ,14 ) ONE_TIME_AMOUNT,
             RIGHT(REPLICATE('0',10 ) + CAST(a.INS_COST as VARCHAR) ,10 ) INS_COST,
             RIGHT(REPLICATE('0',6 ) + CAST(a.INS_MONTH_AMOUNT as VARCHAR) ,6 ) INS_MONTH_AMOUNT,
             RIGHT(REPLICATE('0',10 ) + CAST(a.ACCU_AMOUNT as VARCHAR) ,10 ) ACCU_AMOUNT           
             from TB_S_M_INS2_DETAIL a
             left join TB_H_M_EMP b
             on a.EMP_ID = b.EMP_ID
             where left(Convert(varchar, PAYMENT_DATE,112),4) = @C_YEAR            
             and IS_NOT_CAL = 'N' and ONE_TIME_AMOUNT <> 0 --20151103 TERRY MODIFY
			 and a.EMP_ID in 
			     (
				    select EMP_ID from (
				     select a.EMP_ID,sum(a.INS_COST) INS_COST				        
				     from TB_S_M_INS2_DETAIL a
				     left join TB_H_M_EMP b
				     on a.EMP_ID = b.EMP_ID
				     where left(Convert(varchar, PAYMENT_DATE,112),4) = @C_YEAR				
				     and IS_NOT_CAL = 'N' and ONE_TIME_AMOUNT <> 0 --20151103 TERRY MODIFY	
				     and INS_COST <> 0
				     group by a.EMP_ID		 
				     )Q
			     )
             ORDER BY a.EMP_ID,PAYMENT_DATE,ACCU_AMOUNT ASC
            ");

            /*     
                  sb.Append("select CONVERT(char(10), PAYMENT_DATE, 120)PAYMENT_DATE,b.EMP_NAME,a.LICENSE_ID,");
                  sb.Append(" RIGHT(REPLICATE('0',14 ) + CAST(a.ONE_TIME_AMOUNT as VARCHAR) ,14 ) ONE_TIME_AMOUNT,");
                  sb.Append(" RIGHT(REPLICATE('0',10 ) + CAST(a.INS_COST as VARCHAR) ,10 ) INS_COST,");
                  sb.Append(" RIGHT(REPLICATE('0',6 ) + CAST(a.INS_MONTH_AMOUNT as VARCHAR) ,6 ) INS_MONTH_AMOUNT,");
                  sb.Append(" RIGHT(REPLICATE('0',10 ) + CAST(a.ACCU_AMOUNT as VARCHAR) ,10 ) ACCU_AMOUNT");           
                  sb.Append(" from TB_S_M_INS2_DETAIL a");
                  sb.Append(" left join TB_H_M_EMP b");
                  sb.Append(" on a.EMP_ID = b.EMP_ID");
                  sb.Append(" where left(Convert(varchar, PAYMENT_DATE,112),4) = @C_YEAR");
                  //sb.Append(" and IS_NOT_CAL = 'N' and INS_COST <> 0");
                  sb.Append(" and IS_NOT_CAL = 'N' and ONE_TIME_AMOUNT <> 0");//20151103 TERRY MODIFY
                  sb.Append(" ORDER BY a.EMP_ID,PAYMENT_DATE,ACCU_AMOUNT ASC");
      */
            ht.Add("@C_YEAR", C_YEAR);


            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public string get62DataCount()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" 
             select count(*) totalCount from TB_S_M_INS2_DETAIL a
             left join TB_H_M_EMP b
             on a.EMP_ID = b.EMP_ID
             where left(Convert(varchar, PAYMENT_DATE,112),4) = @C_YEAR            
             and IS_NOT_CAL = 'N' and ONE_TIME_AMOUNT <> 0 --20151103 TERRY MODIFY
			 and a.EMP_ID in 
			     (
				    select EMP_ID from (
				     select a.EMP_ID,sum(a.INS_COST) INS_COST				        
				     from TB_S_M_INS2_DETAIL a
				     left join TB_H_M_EMP b
				     on a.EMP_ID = b.EMP_ID
				     where left(Convert(varchar, PAYMENT_DATE,112),4) = @C_YEAR				
				     and IS_NOT_CAL = 'N' and ONE_TIME_AMOUNT <> 0 --20151103 TERRY MODIFY	
				     and INS_COST <> 0
				     group by a.EMP_ID		 
				     )Q
			     )
             
            ");
            /*
                        sb.Append("select count(*) totalCount");           
                        sb.Append(" from TB_S_M_INS2_DETAIL a");
                        sb.Append(" left join TB_H_M_EMP b");
                        sb.Append(" on a.EMP_ID = b.EMP_ID");
                        sb.Append(" where left(Convert(varchar, PAYMENT_DATE,112),4) =@C_YEAR");
                        //sb.Append(" and IS_NOT_CAL = 'N' and INS_COST <> 0");
                        sb.Append(" and IS_NOT_CAL = 'N'");
                   */

            ht.Add("@C_YEAR", C_YEAR);


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["totalCount"].ToString();
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public DataTable get63Data()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select PAYMENT_DATE ,EMP_NAME,LICENSE_ID,");
            sb.Append(" RIGHT(REPLICATE('0',14 ) + CAST(NT_AMOUNT as VARCHAR) ,14 ) NT_AMOUNT,");
            sb.Append(" RIGHT(REPLICATE('0',10 ) + CAST(INS_COST as VARCHAR) ,10 )INS_COST");
            sb.Append(" from TB_S_M_INS2_NOT_BONUS");
            sb.Append(" where substring(PAYMENT_DATE,0,4) =@YEAR and INS_COST <> 0 and CODE_CD = '63'");
            sb.Append(" order by LICENSE_ID,PAYMENT_DATE");

            ht.Add("@YEAR", YEAR);


            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public string get63DataCount()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select count(*) totalCount");
            sb.Append(" from TB_S_M_INS2_NOT_BONUS");
            sb.Append(" where substring(PAYMENT_DATE,0,4) =@YEAR and INS_COST <> 0 and CODE_CD = '63'");

            ht.Add("@YEAR", YEAR);


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["totalCount"].ToString();
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public DataTable get63YM()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select MAX(substring(PAYMENT_DATE,0,6))MAXDAY,MIN(substring(PAYMENT_DATE,0,6))MINDAY");
            sb.Append(" from TB_S_M_INS2_NOT_BONUS");
            sb.Append(" where substring(PAYMENT_DATE,0,4) =@YEAR and INS_COST <> 0 and CODE_CD = '63'");


            ht.Add("@YEAR", YEAR);


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                MAXYM = dt.Rows[0]["MAXDAY"].ToString();
                MINYM = dt.Rows[0]["MINDAY"].ToString();
            }

            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable get68Data()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select PAYMENT_DATE ,EMP_NAME,LICENSE_ID,");
            sb.Append(" RIGHT(REPLICATE('0',14 ) + CAST(NT_AMOUNT as VARCHAR) ,14 ) NT_AMOUNT,");
            sb.Append(" RIGHT(REPLICATE('0',10 ) + CAST(INS_COST as VARCHAR) ,10 )INS_COST");
            sb.Append(" from TB_S_M_INS2_NOT_BONUS");
            sb.Append(" where substring(PAYMENT_DATE,0,4) =@YEAR and INS_COST <> 0 and CODE_CD = '68'");
            sb.Append(" order by LICENSE_ID,PAYMENT_DATE");

            ht.Add("@YEAR", YEAR);


            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public string get68DataCount()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select count(*) totalCount");
            sb.Append(" from TB_S_M_INS2_NOT_BONUS");
            sb.Append(" where substring(PAYMENT_DATE,0,4) =@YEAR and INS_COST <> 0 and CODE_CD = '68'");

            ht.Add("@YEAR", YEAR);


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["totalCount"].ToString();
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public DataTable checkINS2_DETAIL()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select * from TB_S_M_INS2_DETAIL");
            sb.Append(" where left(Convert(varchar, PAYMENT_DATE,112),4) = @C_YEAR");

            ht.Add("@C_YEAR", C_YEAR);


            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable checkNOT_BONUS()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select * from TB_S_M_INS2_NOT_BONUS");
            sb.Append(" where substring(PAYMENT_DATE,0,4) = @YEAR");

            ht.Add("@YEAR", YEAR);


            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public string get65DataCount()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select count(*) totalCount");
            sb.Append(" from TB_S_M_INS2_NOT_BONUS");
            sb.Append(" where substring(PAYMENT_DATE,0,4) =@YEAR and INS_COST <> 0 and CODE_CD = '65'");

            ht.Add("@YEAR", YEAR);


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["totalCount"].ToString();
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public DataTable get65YM()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select MAX(substring(PAYMENT_DATE,0,6))MAXDAY,MIN(substring(PAYMENT_DATE,0,6))MINDAY");
            sb.Append(" from TB_S_M_INS2_NOT_BONUS");
            sb.Append(" where substring(PAYMENT_DATE,0,4) =@YEAR and INS_COST <> 0 and CODE_CD = '65'");


            ht.Add("@YEAR", YEAR);


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                MAXYM = dt.Rows[0]["MAXDAY"].ToString();
                MINYM = dt.Rows[0]["MINDAY"].ToString();
            }

            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable get65Data()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select PAYMENT_DATE ,EMP_NAME,LICENSE_ID,");
            sb.Append(" RIGHT(REPLICATE('0',14 ) + CAST(NT_AMOUNT as VARCHAR) ,14 ) NT_AMOUNT,");
            sb.Append(" RIGHT(REPLICATE('0',10 ) + CAST(INS_COST as VARCHAR) ,10 )INS_COST");
            sb.Append(" from TB_S_M_INS2_NOT_BONUS");
            sb.Append(" where substring(PAYMENT_DATE,0,4) =@YEAR and INS_COST <> 0 and CODE_CD = '65'");
            sb.Append(" order by LICENSE_ID,PAYMENT_DATE");

            ht.Add("@YEAR", YEAR);


            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable get68YM()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select MAX(substring(PAYMENT_DATE,0,6))MAXDAY,MIN(substring(PAYMENT_DATE,0,6))MINDAY");
            sb.Append(" from TB_S_M_INS2_NOT_BONUS");
            sb.Append(" where substring(PAYMENT_DATE,0,4) =@YEAR and INS_COST <> 0 and CODE_CD = '68'");


            ht.Add("@YEAR", YEAR);


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                MAXYM = dt.Rows[0]["MAXDAY"].ToString();
                MINYM = dt.Rows[0]["MINDAY"].ToString();
            }

            return dt;
        }
        catch
        {
            throw;
        }
    }




}