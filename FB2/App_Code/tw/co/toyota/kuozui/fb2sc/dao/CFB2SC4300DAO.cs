using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;


/// <summary>
/// CFB2HA0300BO 的摘要描述
/// </summary>
public class CFB2SC430DAO : BaseDAO
{
    public CFB2SC430DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string REPAY_DT { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string HOURLY_WAGE { get; set; }
    public string REPAY_TYPE { get; set; }
    public string REPAY_SUB_ID { get; set; }
    public string BASE_VALUE { get; set; }
    public string UNITS { get; set; }
    public string AMOUNT { get; set; }
    public string REMARK { get; set; }
    public string SEQ_NO { get; set; }
    public string SALARY_ID { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    //for查詢欄位
    public string ddl_SYS_ID { get; set; }
    public string CHG_AMT_A { get; set; }
    public string OP_MSG { get; set; }
    public string DATA_YM { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string APPROVE_BY { get; set; }
    public string APPROVE_DT { get; set; }
    public string APP_REMARK { get; set; }
    public string SQE_NO1 { get; set; }


    public DataTable getSEQNO(string REPAY_DT, string REPAY_TYPE, string REPAY_SUB_ID, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT *");
            sb.Append(" FROM TB_S_M_OVERTIME_REPAY");
            sb.Append(" WHERE REPAY_DT = @REPAY_DT AND REPAY_TYPE = @REPAY_TYPE AND REPAY_SUB_ID = @REPAY_SUB_ID AND emp_id = @emp_id order by SEQ_NO desc");
            ht.Add("@REPAY_DT", REPAY_DT);
            ht.Add("@REPAY_TYPE", REPAY_TYPE);
            ht.Add("@REPAY_SUB_ID", REPAY_SUB_ID);
            ht.Add("@emp_id", emp_id);
            return dbConn.QueryT(sb,ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getCheckWORK_SHIFT_ALLOWANCE_TYPE(string REPAY_SUB_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT *");
            sb.Append(" FROM TB_9_M_COMM_D");
            sb.Append(" WHERE SYS_CD = 'SC' AND MAIN_CD = 'WORK_SHIFT_ALLOWANCE_TYPE' AND SUB_CD = '" + REPAY_SUB_ID + "'");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getCheckOVERTIME_PAY_TYPE(string REPAY_SUB_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT *");
            sb.Append(" FROM TB_9_M_COMM_D");
            sb.Append(" WHERE SYS_CD = 'SC' AND MAIN_CD = 'OVERTIME_PAY_TYPE' AND SUB_CD = '" + REPAY_SUB_ID + "'");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getCheckREPAY_SUB_ID(string REPAY_SUB_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT *");
            sb.Append(" FROM TB_D_M_LEAVE_TYPE_D");
            sb.Append(" WHERE SUB_LEAVE_CD = '" + REPAY_SUB_ID + "' ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getREPAY_TYPE(string REPAY_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT *");
            sb.Append(" FROM TB_9_M_COMM_D");
            sb.Append(" WHERE SYS_CD = 'SC' AND MAIN_CD = 'REPAY_TYPE1' AND SUB_CD = '"+ REPAY_TYPE +"'");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getEMP_NAME(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT *");
            sb.Append(" FROM TB_H_M_EMP");
            sb.Append(" WHERE EMP_ID=@EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb,ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getLastREPAY_DT(string SALARY_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CONVERT(VARCHAR(10),dbo.FN_S_DUTY_EDT('" + SALARY_TYPE + "'),111) as REPAY_DT");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getPROCESS_STATUS()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='SA' and MAIN_CD='PROCESS_STATUS'  ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getREPAY_TYPE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='SC' and MAIN_CD='REPAY_TYPE1' and IS_VALID='Y'  ");
            
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getREPAY_TYPE_hid(string ddl1)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC,CODE_VAL1,CODE_VAL2 from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='SC' and MAIN_CD='REPAY_TYPE1' and IS_VALID='Y' and SUB_CD=@SUB_CD   ");
            ht.Add("@SUB_CD", ddl1);
            return dbConn.Query(sb,ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getREPAY_SUB_ID_1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SUB_LEAVE_CD,SUB_LEAVE_DESC from TB_D_M_LEAVE_TYPE_D");
            sb.Append(" where IS_USED='Y' and LEAVE_PAY_RATE<'1' ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getREPAY_SUB_ID_1_2(string SUB_LEAVE_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.SUB_LEAVE_CD,a.SUB_LEAVE_DESC,b.LEAVE_PAY_RATE from TB_D_M_LEAVE_TYPE_D a");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_D b on a.SUB_LEAVE_CD=b.SUB_LEAVE_CD");
            sb.Append(" where a.IS_USED='Y' and a.LEAVE_PAY_RATE<'1' and b.SUB_LEAVE_CD=@SUB_LEAVE_CD");

           
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getREPAY_SUB_ID_1_3(string index, string ddl1)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select *,iif(charindex('應稅',SUB_DESC)> 0  ,'Y','N') as TAX_YN from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='SC' and MAIN_CD=@MAIN_CD and SUB_CD=@SUB_CD");
           
            ht.Add("@SUB_CD", ddl1);
            ht.Add("@MAIN_CD", index);


            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getREPAY_SUB_ID_2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where IS_VALID='Y' and SYS_CD='SC' and MAIN_CD='OVERTIME_PAY_TYPE'  ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getREPAY_SUB_ID_3()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where IS_VALID='Y' and SYS_CD='SC' and MAIN_CD='WORK_SHIFT_ALLOWANCE_TYPE'  ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getHOURLY_WAGE(string SALARY_YM_Add, string EMP_ID_Add)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select HOURLY_WAGE,EMP_NAME from TB_S_M_EMP_RESULT");
            sb.Append(" where SALARY_YM=@SALARY_YM and EMP_ID=@EMP_ID and IS_OTHER='N'");
            ht.Add("@EMP_ID", EMP_ID_Add);
            ht.Add("@SALARY_YM", SALARY_YM_Add);

            return dbConn.Query(sb,ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getSYS_ID(string SUB_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SUB_CD ,SUB_DESC from TB_9_M_COMM_D where 1=1 and sys_cd = '99' and main_cd = 'sys_id' and is_valid ='Y' and SUB_CD = @SUB_CD order by SUB_CD");
            ht.Add("@SUB_CD", SUB_CD);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getFUNC_ID(string ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT        *");
            sb.Append(" FROM            TB_9_M_SYS_M");
            sb.Append(" WHERE SYS_ID+MODE_ID = @ID");
            ht.Add("@ID", ID);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    //internal System.Data.DataTable getSYS_ID()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append(" select SUB_CD ,SUB_DESC from TB_9_M_COMM_D where 1=1 and sys_cd = '99' and main_cd = 'sys_id' and is_valid ='Y' order by SUB_CD");
    //        return dbConn.Query(sb);

    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    public DataTable get_SYS_ID_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HB' and MAIN_CD=CAR_TYPE  ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string txt_REPAY_SDT, string txt_REPAY_EDT, string ddl_PROCESS_STATUS, string ddl_REPAY_TYPE, string ddl_REPAY_SUB_ID, string txt_EMP_ID, string txt_EMP_NAME)
    {
        try
        {
            if (sortExpression == "EMP_NAME ASC" || sortExpression == "EMP_ID ASC" || sortExpression == "EMP_NAME DESC" || sortExpression == "EMP_ID DESC")
            {
                sortExpression = "vm." + sortExpression;
            }
            if (sortExpression == "SALARY_ID ASC" || sortExpression == "REMARK ASC" || sortExpression == "APP_REMARK ASC" || sortExpression == "SALARY_ID DESC" || sortExpression == "REMARK DESC" || sortExpression == "APP_REMARK DESC")
            {
                sortExpression = "t1." + sortExpression;
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From ");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,t1.REPAY_DT,t1.REPAY_TYPE,t1.REPAY_TYPE +'-'+ d.SUB_DESC as DESC1,t1.REPAY_SUB_ID,t1.REPAY_SUB_ID+'-'+");
            sb.Append(" case when t1.REPAY_TYPE ='1' then le.SUB_LEAVE_DESC");
            sb.Append(" when t1.REPAY_TYPE ='2' then ov.SUB_DESC");
            sb.Append("	when t1.REPAY_TYPE ='3' then sh.SUB_DESC else '' end as DESC2");
            sb.Append(" ,t1.SALARY_ID,t1.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME,t1.EMP_ID");
            sb.Append(" ,vm.EMP_NAME as EMP_NAME,t1.SEQ_NO");
            sb.Append(" ,t1.UNITS,t1.BASE_VALUE,Replace(Convert(Varchar(12),CONVERT(money,t1.AMOUNT),1),'.00','') as AMOUNT,t1.PROCESS_STATUS");
            //sb.Append(" ,Replace(Convert(Varchar(12),CONVERT(money,t1.HOURLY_WAGE),1),'.00','') as HOURLY_WAGE");
            sb.Append(" ,t1.HOURLY_WAGE");
            sb.Append(" ,t1.PROCESS_STATUS +'-'+ p.SUB_DESC as DESC1_1, t1.APPROVE_DT");
            sb.Append(" ,t1.APPROVE_BY,t1.APPROVE_BY + '-' + vm2.EMP_NAME as APPROVE_NAME ,t1.REMARK,t1.APP_REMARK");
            sb.Append(" from TB_S_M_OVERTIME_REPAY t1");
            sb.Append(" left join VW_H_EMP_DATA vm   on t1.EMP_ID = vm.EMP_ID");
            sb.Append(" left join VW_H_EMP_DATA vm2  on t1.APPROVE_BY = vm2.EMP_ID");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on   t1.SALARY_ID = s.SALARY_ID");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='REPAY_TYPE1' and  t1.REPAY_TYPE = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on  p.SYS_CD ='SA' and  p.MAIN_CD='PROCESS_STATUS' and  t1.PROCESS_STATUS = p.SUB_CD");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_D le on t1.REPAY_TYPE ='1' and le.SUB_LEAVE_CD = t1.REPAY_SUB_ID");
            sb.Append(" left join TB_9_M_COMM_D ov on t1.REPAY_TYPE ='2' and ov.SYS_CD ='SC' and  ov.MAIN_CD='OVERTIME_PAY_TYPE' and ov.SUB_CD = t1.REPAY_SUB_ID");
            sb.Append(" left join TB_9_M_COMM_D sh	on t1.REPAY_TYPE ='3' and sh.SYS_CD ='SC' and  sh.MAIN_CD='WORK_SHIFT_ALLOWANCE_TYPE' and sh.SUB_CD = t1.REPAY_SUB_ID");
            sb.Append(" WHERE 1=1 ");
            if (txt_REPAY_SDT != "" && txt_REPAY_EDT == "")
            {
                sb.Append(" and REPAY_DT BETWEEN @REPAY_SDT and @REPAY_EDT ");
                ht.Add("@REPAY_SDT", txt_REPAY_SDT);
                ht.Add("@REPAY_EDT", "9999/12/31");
            }
            if (txt_REPAY_SDT == "" && txt_REPAY_EDT != "")
            {
                sb.Append(" and REPAY_DT BETWEEN @REPAY_SDT and @REPAY_EDT ");
                ht.Add("@REPAY_SDT", "1900/01/01");
                ht.Add("@REPAY_EDT", txt_REPAY_EDT);
            }
            if (txt_REPAY_SDT != "" && txt_REPAY_EDT != "")
            {
                sb.Append(" and REPAY_DT BETWEEN @REPAY_SDT and @REPAY_EDT ");
                ht.Add("@REPAY_SDT", txt_REPAY_SDT);
                ht.Add("@REPAY_EDT", txt_REPAY_EDT);
            }
            if (ddl_PROCESS_STATUS != "-1")
            {
                sb.Append(" and t1.PROCESS_STATUS=@PROCESS_STATUS ");
               
                ht.Add("@PROCESS_STATUS", ddl_PROCESS_STATUS);
            }
            if (ddl_REPAY_TYPE != "-1")
            {
                sb.Append(" and t1.REPAY_TYPE=@ddl_REPAY_TYPE ");
              
                ht.Add("@ddl_REPAY_TYPE", ddl_REPAY_TYPE);
            }
            if (ddl_REPAY_SUB_ID != "-1" && ddl_REPAY_SUB_ID != "請先選擇追溯類別")
            {
                sb.Append(" and t1.REPAY_SUB_ID=@ddl_REPAY_SUB_ID ");
              
                ht.Add("@ddl_REPAY_SUB_ID", ddl_REPAY_SUB_ID);
            }
            if (txt_EMP_ID != "")
            {
                sb.Append(" and t1.EMP_ID=@EMP_ID ");
                ht.Add("@EMP_ID", txt_EMP_ID);
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and vm.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", string.Format("{0}%", txt_EMP_NAME));
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
    public int getCount(int startRowIndex, int maximumRows, string txt_REPAY_SDT, string txt_REPAY_EDT, string ddl_PROCESS_STATUS, string ddl_REPAY_TYPE, string ddl_REPAY_SUB_ID, string txt_EMP_ID, string txt_EMP_NAME)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");           
            sb.Append(" from TB_S_M_OVERTIME_REPAY t1");
            sb.Append(" left join VW_H_EMP_DATA vm   on t1.EMP_ID = vm.EMP_ID");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on   t1.SALARY_ID = s.SALARY_ID");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='REPAY_TYPE1' and  t1.REPAY_TYPE = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on  p.SYS_CD ='SA' and  p.MAIN_CD='PROCESS_STATUS' and  t1.PROCESS_STATUS = p.SUB_CD");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_D le on t1.REPAY_TYPE ='1' and le.SUB_LEAVE_CD = t1.REPAY_SUB_ID");
            sb.Append(" left join TB_9_M_COMM_D ov on t1.REPAY_TYPE ='2' and ov.SYS_CD ='SC' and  ov.MAIN_CD='WORK_SHIFT_ALLOWANCE_TYPE' and ov.SUB_CD = t1.REPAY_SUB_ID");
            sb.Append(" left join TB_9_M_COMM_D sh	on t1.REPAY_TYPE ='3' and ov.SYS_CD ='SC' and  ov.MAIN_CD='OVERTIME_PAY_TYPE' and ov.SUB_CD = t1.REPAY_SUB_ID");
            sb.Append(" WHERE 1=1 ");
            if (txt_REPAY_SDT != "" && txt_REPAY_EDT == "")
            {
                sb.Append(" and REPAY_DT BETWEEN @REPAY_SDT and @REPAY_EDT ");
                ht.Add("@REPAY_SDT", txt_REPAY_SDT);
                ht.Add("@REPAY_EDT", "9999/12/31");
            }
            if (txt_REPAY_SDT == "" && txt_REPAY_EDT != "")
            {
                sb.Append(" and REPAY_DT BETWEEN @REPAY_SDT and @REPAY_EDT ");
                ht.Add("@REPAY_SDT", "1900/01/01");
                ht.Add("@REPAY_EDT", txt_REPAY_EDT);
            }
            if (txt_REPAY_SDT != "" && txt_REPAY_EDT != "")
            {
                sb.Append(" and REPAY_DT BETWEEN @REPAY_SDT and @REPAY_EDT ");
                ht.Add("@REPAY_SDT", txt_REPAY_SDT);
                ht.Add("@REPAY_EDT", txt_REPAY_EDT);
            }
            if (ddl_PROCESS_STATUS != "-1")
            {
                sb.Append(" and t1.PROCESS_STATUS=@PROCESS_STATUS ");
                
                ht.Add("@PROCESS_STATUS", ddl_PROCESS_STATUS);
            }
            if (ddl_REPAY_TYPE != "-1")
            {
                sb.Append(" and t1.REPAY_TYPE=@ddl_REPAY_TYPE ");
            
                ht.Add("@ddl_REPAY_TYPE", ddl_REPAY_TYPE);
            }
            if (ddl_REPAY_SUB_ID != "-1" && ddl_REPAY_SUB_ID != "請先選擇追溯類別")
            {
                sb.Append(" and t1.REPAY_SUB_ID=@ddl_REPAY_SUB_ID ");
              
                ht.Add("@ddl_REPAY_SUB_ID", ddl_REPAY_SUB_ID);
            }
            if (txt_EMP_ID != "")
            {
                sb.Append(" and t1.EMP_ID=@EMP_ID ");
                ht.Add("@EMP_ID", txt_EMP_ID);
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and vm.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", string.Format("{0}%", txt_EMP_NAME));
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

    public DataTable getModeData(string id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("     select ROW_NUMBER() OVER(ORDER BY m.FUNC_ID) As RowNumber,");
            sb.Append("     d.* ");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID and m.SYS_ID+m.MODE_ID = @ID");
            sb.Append("     where 1=1");

            ht.Add("@ID", id);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getModeData(int startRowIndex, int maximumRows, string sortExpression, string id)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "MODE_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (");
            sb.Append("     select ROW_NUMBER() OVER(ORDER BY m.FUNC_ID) As RowNumber,");
            sb.Append("     d.* ");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID and m.SYS_ID+m.MODE_ID = @ID");
            sb.Append("     where 1=1");
            sb.Append(" ) god_data where RowNumber between CAST(@startRowIndex as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@ID", id);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getModeCount(int startRowIndex, int maximumRows, string id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNT(*) total_record from (");
            sb.Append(" 	select d.MODE_ID, d.FUNCTION_ID, d.FUNCTION_NAME");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID and m.SYS_ID+m.MODE_ID = @ID");
            sb.Append("     where 1=1");
            sb.Append(" ) as tb1");

            ht.Add("@ID", id);
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

    //public DataTable getData()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        sb.Append(" Select * From TB_9_M_COMM_H";
    //         sb.Append(" where 1=1";

    //        if (SYS_CD != "")
    //        {
    //             sb.Append(" and SYS_CD = @SYS_CD ";
    //            ht.Add("@SYS_CD", SYS_CD);
    //        }

    //        return dbConn.Query(sb, ht);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    public string deleteData(string deleteitem)
    {
        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        char[] ch1 = new Char[] { '|' };
        string[] split1 = deleteitem.Split(ch1);
        string REPAY_DT2 = split1[0].ToString();
        string EMP_ID2 = split1[1].ToString();
        string REPAY_TYPE2 = split1[3].ToString();
        string REPAY_SUB_ID2 = split1[2].ToString();
        string SEQ_NO2 = split1[4].ToString();

        //寫log
        sb.AppendLine(" update TB_S_M_OVERTIME_REPAY set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SC430' ");
        sb.AppendLine(" where REPAY_DT = @REPAY_DT and EMP_ID=@EMP_ID and REPAY_SUB_ID=@REPAY_SUB_ID and REPAY_TYPE=@REPAY_TYPE and SEQ_NO=@SEQ_NO; ");

        sb.Append(" Delete from TB_S_M_OVERTIME_REPAY where REPAY_DT = @REPAY_DT and EMP_ID=@EMP_ID and REPAY_SUB_ID=@REPAY_SUB_ID and REPAY_TYPE=@REPAY_TYPE and SEQ_NO=@SEQ_NO;  ");

        string x = REPAY_DT2.Replace("/", "-");
        int x2 = REPAY_DT2.IndexOf(" ");
        ht.Add("@REPAY_DT", x.Substring(0,x2));
        ht.Add("@EMP_ID", EMP_ID2);

        ht.Add("@REPAY_TYPE", REPAY_TYPE2);

        ht.Add("@REPAY_SUB_ID", REPAY_SUB_ID2);
        ht.Add("@SEQ_NO", SEQ_NO2);
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
       dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_S_M_OVERTIME_REPAY where REPAY_DT = @REPAY_DT");
            ht.Add("@REPAY_DT", REPAY_DT);
           
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void addData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_M_OVERTIME_REPAY (REPAY_DT,EMP_ID,SEQ_NO,SALARY_ID,HOURLY_WAGE,REPAY_TYPE,REPAY_SUB_ID,BASE_VALUE,UNITS,AMOUNT,REMARK,PROCESS_STATUS,APPROVE_BY,APPROVE_DT,APP_REMARK,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@REPAY_DT,@EMP_ID,@SEQ_NO,@SALARY_ID,@HOURLY_WAGE,@REPAY_TYPE,@REPAY_SUB_ID,@BASE_VALUE,@UNITS,@AMOUNT,@REMARK,@PROCESS_STATUS,@APPROVE_BY,@APPROVE_DT,@APP_REMARK,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            
            ht.Add("@REPAY_DT", REPAY_DT);
            ht.Add("@REPAY_TYPE", REPAY_TYPE);
            ht.Add("@REPAY_SUB_ID", REPAY_SUB_ID);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@UNITS", UNITS);
            ht.Add("@BASE_VALUE", BASE_VALUE);
            ht.Add("@HOURLY_WAGE", HOURLY_WAGE);
            ht.Add("@AMOUNT", AMOUNT.Replace(",",""));
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_BY", APPROVE_BY);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@REMARK", REMARK);
            ht.Add("@APP_REMARK", APP_REMARK);
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
    public DataTable addData_SQE(string REPAY_DT_SQE, string EMP_ID_SQE, string REPAY_TYPE_SQE, string REPAY_SUB_ID_SQE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT MAX(SEQ_NO )as SEQ_NO ");
            sb.Append(" FROM  TB_S_M_OVERTIME_REPAY");
            sb.Append(" WHERE REPAY_DT = @REPAY_DT and EMP_ID=@EMP_ID and REPAY_TYPE=@REPAY_TYPE and REPAY_SUB_ID=@REPAY_SUB_ID ");
            ht.Add("@REPAY_DT", REPAY_DT_SQE);
            ht.Add("@EMP_ID", EMP_ID_SQE);
            ht.Add("@REPAY_TYPE", REPAY_TYPE_SQE);
            ht.Add("@REPAY_SUB_ID", REPAY_SUB_ID_SQE);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    internal void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_OVERTIME_REPAY ");
            sb.Append(" Set UNITS=@UNITS,AMOUNT=@AMOUNT,SALARY_ID = @SALARY_ID,PROCESS_STATUS='N',REMARK=@REMARK,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE()");
            sb.Append(" where REPAY_DT = @REPAY_DT and EMP_ID=@EMP_ID and REPAY_SUB_ID=@REPAY_SUB_ID and REPAY_TYPE=@REPAY_TYPE and SEQ_NO=@SEQ_NO");
            ht.Add("@REPAY_DT", REPAY_DT);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEQ_NO", SQE_NO1);
            ht.Add("@REPAY_TYPE", REPAY_TYPE);
         
            ht.Add("@REPAY_SUB_ID", REPAY_SUB_ID);
            ht.Add("@UNITS", UNITS);
            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public string deleteTmp()
    {


        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //寫log
        sb.AppendLine(" update TB_S_S_SUBSIDY_TMP set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SC430' ");
        sb.AppendLine(" where CREATED_BY = @EMP_ID; ");
       
        sb.Append(" DELETE FROM TB_S_S_SUBSIDY_TMP");
        sb.Append(" where CREATED_BY = @EMP_ID; ");
        ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

        dbConn.ExecuteT(sb, ht, true);
        return "0";
    }
    public DataTable getSALARYFile()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select SALARY_ID,SALARY_NAME from TB_S_M_SALARY_ITEM");
            sb.Append(" WHERE SALARY_ID = @SALARY_ID");
            sb.Append(" and SALARY_ID in ( select SALARY_ID from TB_S_M_SUBSIDY_MEM_D   ");
            sb.Append("                     where  TYPE =@TYPE ");
            sb.Append("                     and EMP_ID=@EMP_ID ");
            sb.Append("                     )");
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@TYPE", "1");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getLatestSalaryYM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select dbo.FN_S_SALARY_YM() as SALARY_YM ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public DataTable getEMPFile(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select *");
            sb.Append(" FROM TB_H_M_EMP");
            sb.Append(" WHERE EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void addExcelData1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_M_OVERTIME_REPAY_TMP");
            sb.Append("                          (CREATED_BY, SEQ_NO, REPAY_DT, EMP_ID, REPAY_TYPE, REPAY_SUB_ID, UNITS, REMARK, ERR_MSG)");
            sb.Append(" VALUES        (@CREATED_BY, @SEQ_NO, @REPAY_DT, @EMP_ID, @REPAY_TYPE, @REPAY_SUB_ID, @UNITS, @REMARK, @ERR_MSG)");
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@REPAY_DT", REPAY_DT);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@REPAY_TYPE", REPAY_TYPE);
            ht.Add("@REPAY_SUB_ID", REPAY_SUB_ID);
            ht.Add("@UNITS", UNITS);
            ht.Add("@REMARK", REMARK);
            ht.Add("@ERR_MSG", OP_MSG);


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getSeqNO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select isnull(max(SEQ_NO),0) SEQ_NO from TB_S_M_SUBSIDY_DEDU_1_TMP ");
            sb.Append(" where DATA_YM =@DATA_YM and EMP_ID=@EMP_ID and SALARY_ID=@SALARY_ID ");
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getSalary_Name(string salary_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SALARY_NAME");
            sb.Append(" from TB_S_M_SALARY_ITEM");
            sb.Append(" where SALARY_ID = @SALARY_ID");
            ht.Add("@SALARY_ID", salary_id);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }
}