using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2SC320DAO 的摘要描述
/// </summary>
public class CFB2SC3200DAO : BaseDAO
{
    //WK期間工人數
    public string WKduthwker { get; set; }
    //正社員人數
    public string WKmember { get; set; }
    //薪資發放人數
    public string WKpaytotal { get; set; }
    //建教生人數
    public string WKmemofstu { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    public string SALARY_DT { get; set; }
    public string SALARY_TYPE { get; set; }
    public string SALARY_YM { get; set; }
    public CFB2SC3200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string SALARY_YM,
                          string SALARY_SDT, string SALARY_EDT, string PROCESS_STATUS)
    {
        try
        {

            //if (sortExpression.Contains("REMARK"))
            //{
            //    sortExpression = sortExpression.Replace("REMARK", "a.REMARK");
            //}

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * from");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine("	        t.SALARY_TYPE, t.SALARY_YM , CONVERT(char(10), t.SALARY_DT, 111) as SALARY_DT ");
            sb.AppendLine("	        , CONVERT(char(10), t.SALARY_SDT, 111) as SALARY_SDT ");
            sb.AppendLine("         , CONVERT(char(10), t.SALARY_EDT, 111) as SALARY_EDT ");
            sb.AppendLine("         , CONVERT(char(10), t.DUTY_SDT, 111) as DUTY_SDT ");
            sb.AppendLine("         , CONVERT(char(10), t.DUTY_EDT, 111) as DUTY_EDT ");
            sb.AppendLine("         , t.SALARY_TYPE +'-'+ d.SUB_DESC as SALARY_TYPE_DESC ");
            sb.AppendLine("         , t.PROCESS_STATUS +'-'+ e.SUB_DESC as PROCESS_STATUS_DESC,t.PROCESS_STATUS ");
            sb.AppendLine("     from TB_S_M_SALARY_CAL_H t ");
            sb.AppendLine("     left join TB_9_M_COMM_D d on d.SYS_CD = 'SC' and d.MAIN_CD = 'SALARY_TYPE' and d.IS_VALID='Y' and d.SUB_CD = t.SALARY_TYPE ");
            sb.AppendLine("     left join TB_9_M_COMM_D e on e.SYS_CD = 'SC' and e.MAIN_CD = 'PROCESS_STATUS' and e.IS_VALID='Y' and e.SUB_CD = t.PROCESS_STATUS ");
            sb.AppendLine("    where 1=1 and t.SALARY_TYPE = 'A' ");
            if (SALARY_YM != "")
            {
                sb.AppendLine(" and t.SALARY_YM = @SALARY_YM ");
                ht.Add("@SALARY_YM", SALARY_YM.Replace("/", ""));
            }
            if (SALARY_SDT != "")
            {
                sb.AppendLine(" and t.SALARY_DT >= @SALARY_SDT ");
                ht.Add("@SALARY_SDT", SALARY_SDT);
            }
            if (SALARY_EDT != "")
            {
                sb.AppendLine(" and t.SALARY_DT <= @SALARY_EDT ");
                ht.Add("@SALARY_EDT", SALARY_EDT);
            }
            if (PROCESS_STATUS != "-1" && PROCESS_STATUS != "")
            {
                sb.AppendLine(" and t.PROCESS_STATUS = @PROCESS_STATUS  ");
                ht.Add("@PROCESS_STATUS", PROCESS_STATUS);
            }
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getCount(int startRowIndex, int maximumRows, string SALARY_YM,
                          string SALARY_SDT, string SALARY_EDT, string PROCESS_STATUS)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select COUNT(*) total_record ");
            sb.AppendLine("   from TB_S_M_SALARY_CAL_H t ");
            sb.AppendLine("   left join TB_9_M_COMM_D d on d.SYS_CD = 'SC' and d.MAIN_CD = 'SALARY_TYPE' and d.IS_VALID='Y' and d.SUB_CD = t.SALARY_TYPE ");
            sb.AppendLine("   left join TB_9_M_COMM_D e on e.SYS_CD = 'SC' and e.MAIN_CD = 'PROCESS_STATUS' and e.IS_VALID='Y' and e.SUB_CD = t.PROCESS_STATUS ");
            sb.AppendLine("  where 1=1 and t.SALARY_TYPE = 'A' ");
            if (SALARY_YM != "")
            {
                sb.AppendLine(" and t.SALARY_YM = @SALARY_YM ");
                ht.Add("@SALARY_YM", SALARY_YM.Replace("/", ""));
            }
            if (SALARY_SDT != "")
            {
                sb.AppendLine(" and t.SALARY_DT >= @SALARY_SDT ");
                ht.Add("@SALARY_SDT", SALARY_SDT);
            }
            if (SALARY_EDT != "")
            {
                sb.AppendLine(" and t.SALARY_DT <= @SALARY_EDT ");
                ht.Add("@SALARY_EDT", SALARY_EDT);
            }
            if (PROCESS_STATUS != "-1" && PROCESS_STATUS != "")
            {
                sb.AppendLine(" and t.PROCESS_STATUS = @PROCESS_STATUS  ");
                ht.Add("@PROCESS_STATUS", PROCESS_STATUS);
            }
            int t = 0;
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total_record"];
            }


            return t;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getprocess_status()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select SUB_CD,SUB_DESC as PROCESS_STATUS From TB_9_M_COMM_D ");
            sb.AppendLine(" where SYS_CD = 'SC' and MAIN_CD = 'PROCESS_STATUS' and IS_VALID='Y' ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable tryPROCESS_STATUS(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select PROCESS_STATUS , SALARY_DT From TB_S_M_SALARY_CAL_H ");
            sb.AppendLine(" where SALARY_DT = @SALARY_DT and SALARY_TYPE = @SALARY_TYPE and (PROCESS_STATUS = '3' or (PROCESS_STATUS='4')) ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void deleteSIS(string SALARY_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Delete From TB_S_M_SALARY_ANALYSIS Where SALARY_DT = @SALARY_DT");

            ht.Add("@SALARY_DT", SALARY_DT);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkRESULTcnt(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" SELECT t1.EMP_CD,COUNT(t1.EMP_ID) AS CNT  ");
            sb.AppendLine(" FROM TB_S_M_EMP_RESULT t1   ");
            sb.AppendLine(" where t1.SALARY_DT = @SALARY_DT and  t1.COMPANY_CD = 'K' and t1.PJOB_CD <>'PJ50'   ");   //暫時拿掉了 t1.SALARY_TYPE = @SALARY_TYPE and
            sb.AppendLine(" Group By t1.EMP_CD  ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getSALARY_TYPE(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select SUB_CD  From TB_9_M_COMM_D ");
            sb.AppendLine(" where SYS_CD = 'SC' and MAIN_CD = 'SALARY_TYPE' and IS_VALID='Y' and SUB_DESC =@SALARY_TYPE  ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable checkRESULTcnt_equal(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" SELECT t1.EMP_CD,COUNT(t1.EMP_ID) AS CNT  ");
            sb.AppendLine(" FROM TB_S_M_EMP_RESULT t1   ");
            sb.AppendLine(" where t1.SALARY_DT = @SALARY_DT and  t1.COMPANY_CD = 'K' and t1.PJOB_CD ='PJ50'   ");  //暫時拿掉 t1.SALARY_TYPE = @SALARY_TYPE and
            sb.AppendLine(" Group By t1.EMP_CD  ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable checkRESULTcnt_total(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" SELECT COUNT(*) AS CNT  ");
            sb.AppendLine(" FROM TB_S_M_EMP_RESULT t1   ");
            sb.AppendLine(" where t1.SALARY_DT = @SALARY_DT and  t1.COMPANY_CD = 'K'  ");  //暫時拿掉 t1.SALARY_TYPE = @SALARY_TYPE and
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable check_SA_GR_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("  SELECT t1.KIND_CD,t1.GROUP_TYPE,t1.GROUP_ID,t1.GROUP_NAME  ");
            sb.AppendLine("        ,t1.ORDER_SEQ,t1.LEVEL,t1.CLASSIFY    ");
            sb.AppendLine("    FROM TB_S_M_SALARY_GROUP_H t1    ");
            sb.AppendLine("  WHERE t1.KIND_CD ='C' and t1.GROUP_TYPE ='A'   ");
            sb.AppendLine(" Order By t1.ORDER_SEQ;");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable checkPAY(string SALARY_DT, string SALARY_TYPE, string GROUP_ID, string level)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("select dbo.FN_S_GET_SC3200_AMT(@SALARY_DT,@SALARY_TYPE,@GROUP_ID,@level) as AMT ");

            /*
            if (level == "0")
            {                
                //sb.AppendLine("SELECT  SUM(t1.AMOUNT * t1.IS_PLUS) as AMT ");
                //sb.AppendLine("from TB_S_S_SALARY_PAY t1 ");
                //sb.AppendLine("left join TB_S_M_EMP_RESULT t2 on t1.EMP_ID = t2.EMP_ID and t1.SALARY_DT = t2.SALARY_DT and t2.SALARY_YM=t1.DATA_YM  ");
                //sb.AppendLine("where t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE and t2.COMPANY_CD = 'K'   ");
                //sb.AppendLine(" and t1.SALARY_ID in (SELECT SUB_GROUP_ID  as SALARY_ID FROM TB_S_M_SALARY_GROUP_D where KIND_CD ='C'  ");
                //sb.AppendLine("  and  GROUP_TYPE ='A' and GROUP_ID = @GROUP_ID ) ");
            }
            if (level == "1")
            {
                sb.AppendLine("SELECT  SUM(t1.AMOUNT * t1.IS_PLUS) as AMT ");
                sb.AppendLine("from TB_S_S_SALARY_PAY t1 ");
                sb.AppendLine("left join TB_S_M_EMP_RESULT t2 on t1.EMP_ID = t2.EMP_ID and t1.SALARY_DT = t2.SALARY_DT and t2.SALARY_YM=t1.DATA_YM  ");
                sb.AppendLine("where  t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE and t2.COMPANY_CD = 'K'   ");
                sb.AppendLine("  and t1.SALARY_ID in (SELECT SUB_GROUP_ID as SALARY_ID FROM TB_S_M_SALARY_GROUP_D where KIND_CD ='C' and  GROUP_TYPE ='A'    ");
                sb.AppendLine("  and GROUP_ID IN (SELECT SUB_GROUP_ID FROM TB_S_M_SALARY_GROUP_D where KIND_CD ='C' and  GROUP_TYPE ='A' ");
                sb.AppendLine("   and GROUP_ID =@GROUP_ID )  ) ");
            }
            if (level == "2")
            {
                sb.AppendLine("SELECT  SUM(t1.AMOUNT * t1.IS_PLUS) as AMT ");
                sb.AppendLine("from TB_S_S_SALARY_PAY t1 ");
                sb.AppendLine("left join TB_S_M_EMP_RESULT t2 on t1.EMP_ID = t2.EMP_ID and t1.SALARY_DT = t2.SALARY_DT and t2.SALARY_YM=t1.DATA_YM  ");
                sb.AppendLine("where t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE and t2.COMPANY_CD = 'K'   ");
                sb.AppendLine("  and t1.SALARY_ID in (  ");
                sb.AppendLine("     SELECT SUB_GROUP_ID as SALARY_ID FROM TB_S_M_SALARY_GROUP_D where KIND_CD ='C' and  GROUP_TYPE ='A' and GROUP_ID IN (  ");
                sb.AppendLine("  SELECT SUB_GROUP_ID FROM TB_S_M_SALARY_GROUP_D where KIND_CD ='C' and  GROUP_TYPE ='A' and GROUP_ID IN (  ");
                sb.AppendLine("   SELECT SUB_GROUP_ID FROM TB_S_M_SALARY_GROUP_D where KIND_CD ='C' and  GROUP_TYPE ='A' ");
                sb.AppendLine("     and GROUP_ID =@GROUP_ID )  ) ) ");
            }
            */
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@GROUP_ID", GROUP_ID);
            ht.Add("@level", level);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }


    public void addSALARY_ANALYSIS_NO_GROUP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("insert into TB_S_M_SALARY_ANALYSIS (SALARY_DT,SALARY_YM,TOTAL_EMPLOYEES_CNT,EMPLOYEES_CNT1,EMPLOYEES_CNT2,EMPLOYEES_CNT3 ");
            sb.AppendLine(",CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine("values (@SALARY_DT,@SALARY_YM,@WKpaytotal,@WKduthwker,@WKmember ,@WKmemofstu ");
            sb.AppendLine(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID) ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@WKpaytotal", WKpaytotal);
            ht.Add("@WKduthwker", WKduthwker);
            ht.Add("@WKmember", WKmember);
            ht.Add("@WKmemofstu", WKmemofstu);

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

    public void addSALARY_ANALYSIS_GROUP(string AMT_ID, string GROUP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update  TB_S_M_SALARY_ANALYSIS Set " + GROUP_ID + "=@AMT_ID where SALARY_DT=@SALARY_DT");
            ht.Add("@AMT_ID", AMT_ID);
            ht.Add("@SALARY_DT", SALARY_DT);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable searchSIS(int SALARY_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("  select * from  TB_S_M_SALARY_ANALYSIS where SALARY_YM = @SALARY_YM ");
            ht.Add("@SALARY_YM", SALARY_YM.ToString());
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable searchSISCount(int SALARY_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("  select count(*)count from  TB_S_M_SALARY_ANALYSIS where SALARY_YM = @SALARY_YM ");
            ht.Add("@SALARY_YM", SALARY_YM.ToString());
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable searchANALYSIS(string SALARY_YM, int count, string[] colNmae, string WKstartYM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("  select  ");
            for (int i = 2; i < count - 5; i++)  //除去發薪日期、薪資年月、CREATED_DT、CREATED_BY、UPDATED_DT、UPDATED_BY、FUNC_ID
            {
                if(i ==2)
                    sb.AppendLine("  SUM(" + colNmae[i] + ") as " + colNmae[i] + "  ");
                else
                    sb.AppendLine("  ,SUM(" + colNmae[i] + ") as " + colNmae[i] + "  ");
            }
            sb.AppendLine(" from TB_S_M_SALARY_ANALYSIS where SALARY_YM >= @WKstartYM and SALARY_YM<= @SALARY_YM ");
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@WKstartYM", WKstartYM);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getGroup_H(string group_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("  SELECT distinct t1.KIND_CD,t1.GROUP_TYPE,t1.GROUP_ID,CASE WHEN t1.LEVEL=1 then  t2.SUB_GROUP_ID else t1.GROUP_ID end as SUB_GROUP_ID ");
            sb.AppendLine("   ,CASE WHEN t1.LEVEL=1 then t3.GROUP_NAME else t1.GROUP_NAME end as GROUP_NAME,case when t1.LEVEL=1 then t3.ORDER_SEQ else t1.ORDER_SEQ end as ORDER_SEQ ");
            sb.AppendLine("  ,t1.LEVEL,t1.CLASSIFY  ");
            sb.AppendLine("  FROM TB_S_M_SALARY_GROUP_H t1  ");
            sb.AppendLine("  LEFT JOIN TB_S_M_SALARY_GROUP_D t2  on t1.KIND_CD = t2.KIND_CD and t1.GROUP_TYPE = t2.GROUP_TYPE and t1.GROUP_ID =t2.GROUP_ID ");
            sb.AppendLine("  LEFT JOIN TB_S_M_SALARY_GROUP_H t3 on t2.KIND_CD = t3.KIND_CD and t2.GROUP_TYPE = t3.GROUP_TYPE and t2.SUB_GROUP_ID =t3.GROUP_ID ");
            sb.AppendLine("  WHERE t1.KIND_CD ='C' and t1.GROUP_TYPE ='A'  and t1.LEVEL>=1 and t1.GROUP_ID = @GROUP_ID ");
            sb.AppendLine("  Order By case when t1.LEVEL=1 then t3.ORDER_SEQ else t1.ORDER_SEQ end ");
            ht.Add("@GROUP_ID", group_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable dt_sub_last_month(int SALARY_YM, int count, string[,] colu, string WKstartYM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("  select ");
            for (int j = 6; j < count - 5; j++)
            {
                if (j != count - 6)
                {
                    sb.AppendLine("" + colu[j, 0] + "1" + "-" + colu[j, 0] + "2" + " as " + colu[j, 0] + "  ,");
                }
                else
                {
                    sb.AppendLine("" + colu[j, 0] + "1" + "-" + colu[j, 0] + "2" + " as " + colu[j, 0] + "  ");
                }
            }
            sb.AppendLine(" from  ");

            sb.AppendLine("  (select 1+1 as test1 , ");
            for (int j = 6; j < count - 5; j++)
            {
                if (j != count - 6)
                {
                    sb.AppendLine("  SUM(" + colu[j, 0] + ") as " + colu[j, 0] + "1" + " , ");
                }
                else
                {
                    sb.AppendLine("  SUM(" + colu[j, 0] + ") as " + colu[j, 0] + "1" + "  ");
                }
            }
            sb.AppendLine(" from TB_S_M_SALARY_ANALYSIS where SALARY_YM = @SALARY_YM )t1 ");
            sb.AppendLine("  left join  ");
            sb.AppendLine("  (select 1+1 as test2,  ");
            for (int j = 6; j < count - 5; j++)
            {
                if (j != count - 6)
                {
                    sb.AppendLine("  SUM(" + colu[j, 0] + ") as " + colu[j, 0] + "2" + " , ");
                }
                else
                {
                    sb.AppendLine("  SUM(" + colu[j, 0] + ") as " + colu[j, 0] + "2" + "  ");
                }
            }
            sb.AppendLine(" from TB_S_M_SALARY_ANALYSIS where SALARY_YM = @SALARY_1_YM )t2 ");
            sb.AppendLine(" on t1.test1 = t2.test2 ");


            ht.Add("@SALARY_YM", SALARY_YM.ToString());
            ht.Add("@WKstartYM", WKstartYM);
            if (Convert.ToString(SALARY_YM).Substring(4, 2) == "01")
            {
                ht.Add("@SALARY_1_YM", (SALARY_YM - 89).ToString());
            }
            else
            { ht.Add("@SALARY_1_YM", (SALARY_YM - 1).ToString()); }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable dt_sub_last_rotio(int SALARY_YM, int count, string[,] colu, string WKstartYM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("  select ");
            for (int j = 6; j < count - 5; j++)
            {
                if (j != count - 6)
                {
                    sb.AppendLine("case " + colu[j, 0] + "2" + " when '0' then '0'   else ");
                    sb.AppendLine("(" + colu[j, 0] + "1" + "-" + colu[j, 0] + "2)/ " + colu[j, 0] + "2 end as " + colu[j, 0] + "  ,");
                }
                else
                {
                    sb.AppendLine("case " + colu[j, 0] + "2" + " when '0' then '0'   else ");
                    sb.AppendLine("(" + colu[j, 0] + "1" + "-" + colu[j, 0] + "2)/ " + colu[j, 0] + "2 end as " + colu[j, 0] + "  ");
                }
            }
            sb.AppendLine(" from  ");

            sb.AppendLine("  (select 1+1 as test1 , ");
            for (int j = 6; j < count - 5; j++)
            {
                if (j != count - 6)
                {
                    sb.AppendLine("  SUM(" + colu[j, 0] + ") as " + colu[j, 0] + "1" + " , ");
                }
                else
                {
                    sb.AppendLine("  SUM(" + colu[j, 0] + ") as " + colu[j, 0] + "1" + "  ");
                }
            }
            sb.AppendLine(" from TB_S_M_SALARY_ANALYSIS where SALARY_YM = @SALARY_YM )t1 ");
            sb.AppendLine("  left join  ");
            sb.AppendLine("  (select 1+1 as test2,  ");
            for (int j = 6; j < count - 5; j++)
            {
                if (j != count - 6)
                {
                    sb.AppendLine("  SUM(" + colu[j, 0] + ") as " + colu[j, 0] + "2" + " , ");
                }
                else
                {
                    sb.AppendLine("  SUM(" + colu[j, 0] + ") as " + colu[j, 0] + "2" + "  ");
                }
            }
            sb.AppendLine(" from TB_S_M_SALARY_ANALYSIS where SALARY_YM = @SALARY_1_YM )t2 ");
            sb.AppendLine(" on t1.test1 = t2.test2 ");


            ht.Add("@SALARY_YM", SALARY_YM.ToString());
            ht.Add("@WKstartYM", WKstartYM);
            if (Convert.ToString(SALARY_YM).Substring(4, 2) == "01")
            {
                ht.Add("@SALARY_1_YM", (SALARY_YM - 89).ToString());
            }
            else
            { ht.Add("@SALARY_1_YM", (SALARY_YM - 1).ToString()); }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable dt_Accounting(int SALARY_YM, int i, int count, string[,] colu, string WKstartYM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("  select  ");
            for (int j = 2; j < count - 5; j++)
            {
                if (j != count - 6)
                {
                    sb.AppendLine("  SUM(" + colu[j, i] + ") as " + colu[j, i] + " , ");
                }
                else
                {
                    sb.AppendLine("  SUM(" + colu[j, i] + ") as " + colu[j, i] + "  ");
                }
            }
            sb.AppendLine(" from TB_S_M_SALARY_ANALYSIS where SALARY_YM <= @SALARY_YM and SALARY_YM>=@WKstartYM ");
            ht.Add("@SALARY_YM", SALARY_YM.ToString());
            ht.Add("@WKstartYM", WKstartYM);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable dt_Mon_Average(int SALARY_YM, int WKCUM_MON, int count, string[,] colu, string WKstartYM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("  select  ");
            for (int j = 2; j < count - 5; j++)
            {
                if (j != count - 6)
                {
                    sb.AppendLine("  SUM(" + colu[j, 0] + ")/" + WKCUM_MON + " as " + colu[j, 0] + " , ");
                }
                else
                {
                    sb.AppendLine("  SUM(" + colu[j, 0] + ")/" + WKCUM_MON + " as " + colu[j, 0] + "  ");
                }
            }
            sb.AppendLine(" from TB_S_M_SALARY_ANALYSIS where SALARY_YM <= @SALARY_YM and SALARY_YM>=@WKstartYM ");
            ht.Add("@SALARY_YM", SALARY_YM.ToString());
            ht.Add("@WKstartYM", WKstartYM);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable dtAvg_Mon(int SALARY_YM, int WKCUM_MON, int count, string[,] colu, string WKstartYM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("  select ");
            for (int j = 6; j < count - 5; j++)
            {
                if (j != count - 6)
                {
                    sb.AppendLine("case " + colu[j, 0] + "2" + " when '0' then '0'   else ");
                    sb.AppendLine("(" + colu[j, 0] + "1" + "-" + colu[j, 0] + "2)/ " + colu[j, 0] + "2 end as " + colu[j, 0] + "  ,");
                }
                else
                {
                    sb.AppendLine("case " + colu[j, 0] + "2" + " when '0' then '0'   else ");
                    sb.AppendLine("(" + colu[j, 0] + "1" + "-" + colu[j, 0] + "2)/ " + colu[j, 0] + "2 end as " + colu[j, 0] + "  ");
                }
            }
            sb.AppendLine(" from  ");

            sb.AppendLine("  (select 1+1 as test1 , ");
            for (int j = 2; j < count - 5; j++)
            {
                if (j != count - 6)
                {
                    sb.AppendLine("  SUM(" + colu[j, 0] + ") as " + colu[j, 0] + "1" + " , ");
                }
                else
                {
                    sb.AppendLine("  SUM(" + colu[j, 0] + ") as " + colu[j, 0] + "1" + "  ");
                }
            }
            sb.AppendLine(" from TB_S_M_SALARY_ANALYSIS where SALARY_YM = @SALARY_YM)t1 ");
            sb.AppendLine("  left join  ");
            sb.AppendLine("  (select 1+1 as test2,  ");
            for (int j = 2; j < count - 5; j++)
            {
                if (j != count - 6)
                {
                    sb.AppendLine("  SUM(" + colu[j, 0] + ")/" + WKCUM_MON + " as " + colu[j, 0] + "2" + " , ");
                }
                else
                {
                    sb.AppendLine("  SUM(" + colu[j, 0] + ")/" + WKCUM_MON + " as " + colu[j, 0] + "2" + "  ");
                }
            }
            sb.AppendLine(" from TB_S_M_SALARY_ANALYSIS where SALARY_YM <= @SALARY_YM and SALARY_YM>=@WKstartYM)t2 ");
            sb.AppendLine(" on t1.test1 = t2.test2 ");


            ht.Add("@SALARY_YM", SALARY_YM.ToString());
            ht.Add("@WKstartYM", WKstartYM);
            if (Convert.ToString(SALARY_YM).Substring(4, 2) == "01")
            {
                ht.Add("@SALARY_1_YM", (SALARY_YM - 89).ToString());
            }
            else
            { ht.Add("@SALARY_1_YM", (SALARY_YM - 1).ToString()); }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

}