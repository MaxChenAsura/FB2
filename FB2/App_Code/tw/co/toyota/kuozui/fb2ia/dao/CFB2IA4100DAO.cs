using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2IA4100DAO 的摘要描述
/// </summary>
public class CFB2IA4100DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    

    public CFB2IA4100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,
                            string salary_ym, string emp_id, string ins_type)
    {
        try
        {
            if (sortExpression.Contains("COMPANY_CD"))
                sortExpression = sortExpression.Replace("COMPANY_CD", "a.COMPANY_CD");
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "a.EMP_NAME");
            if (sortExpression.Contains("IDENTITY_KIND_NAME"))
                sortExpression = sortExpression.Replace("IDENTITY_KIND_NAME", "c.sub_desc");
            if (sortExpression.Contains("LICENSE_ID"))
                sortExpression = sortExpression.Replace("LICENSE_ID", "u.LICENSE_ID");
            if (sortExpression.Contains("INS_AMT"))
                sortExpression = sortExpression.Replace("INS_AMT", "u.INS_AMT");
            if (sortExpression.Contains("IS_YN_NAME"))
                sortExpression = sortExpression.Replace("IS_YN_NAME", "u.is_yn");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select god_data.* from (");
            sb.Append("  select row_number() over( order by " + sortExpression + ") as RowNumber,a.COMPANY_CD,u.*,b.COMPANY_SNAME,a.EMP_NAME,c.sub_desc as IDENTITY_KIND_NAME ");
            sb.Append(" ,case u.is_yn when 'Y' then '是' else '否' end as IS_YN_NAME ");
            sb.Append("  from ");
            sb.Append("  (");
            sb.Append("   select SALARY_YM,EMP_ID,LICENSE_ID,INS_TYPE,INS_TOTAL as INS_AMT,IS_YN,IDENTITY_KIND FROM TB_I_R_FEES_MONTH where INS_TYPE='A' or INS_TYPE='B' ");
            sb.Append("   UNION ALL");
            sb.Append("   select SALARY_YM,EMP_ID,LICENSE_ID,INS_TYPE,INS_TOTAL as INS_AMT,IS_YN,IDENTITY_KIND FROM TB_I_R_FEES_MONTH where INS_TYPE='C'  and INS_TOTAL>0 ");
            sb.Append("   UNION ALL");
            sb.Append("   select SALARY_YM,EMP_ID,LICENSE_ID,'D' as INS_TYPE,sum(GFEES_SELF) as INS_AMT,IS_YN,IDENTITY_KIND FROM TB_I_R_GROUP_MONTH group by SALARY_YM,EMP_ID,LICENSE_ID,IS_YN,IDENTITY_KIND ");
            sb.Append(" )u");
            sb.Append(" left join TB_H_M_EMP a on u.EMP_ID=a.EMP_ID");
            sb.Append(" left join TB_H_M_COMPANY b on a.COMPANY_CD = b.COMPANY_CD ");
            sb.Append(" left join TB_9_M_COMM_D c on c.sys_cd='IA' and c.main_cd='IDENTITY_KIND' and u.IDENTITY_KIND=c.sub_cd");
            sb.Append(" WHERE  1=1 ");

            //保險類別
            if (ins_type != "-1")
            {
                sb.Append(" and u.INS_TYPE = @INS_TYPE ");
                ht.Add("@INS_TYPE", ins_type);
            }

            //薪調月份
            if (salary_ym != "")
            {
                sb.Append(" and u.SALARY_YM = @SALARY_YM ");
                ht.Add("@SALARY_YM", salary_ym.Replace("/", ""));
            }

            if (emp_id != "")
            {
                sb.Append(" and u.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
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

    public int getCount(int startRowIndex, int maximumRows, string salary_ym, string emp_id, string ins_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

         //   sb.Append("  select count(*) as total_record ");
            sb.Append("  select count(*) as total_record ");
            sb.Append("  from ");
            sb.Append("  (");
            sb.Append("   select SALARY_YM,EMP_ID,LICENSE_ID,INS_TYPE,INS_TOTAL,IS_YN,IDENTITY_KIND FROM TB_I_R_FEES_MONTH");
            sb.Append("   UNION ALL");
            sb.Append("   select SALARY_YM,EMP_ID,LICENSE_ID,'D' as INS_TYPE,sum(GFEES_SELF) as INS_AMT,IS_YN,IDENTITY_KIND FROM TB_I_R_GROUP_MONTH group by SALARY_YM,EMP_ID,LICENSE_ID,IS_YN,IDENTITY_KIND");
            sb.Append(" )u");
            sb.Append(" WHERE  1=1 ");

            //保險類別
            if (ins_type != "-1")
            {
                sb.Append(" and u.INS_TYPE = @INS_TYPE ");
                ht.Add("@INS_TYPE", ins_type);
            }

            //保費年月
            if (salary_ym != "")
            {
                sb.Append(" and u.SALARY_YM = @SALARY_YM ");
                ht.Add("@SALARY_YM", salary_ym.Replace("/", ""));
            }

            if (emp_id != "")
            {
                sb.Append(" and u.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);       

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

   
    public bool Delete_TB_S_M_SALARY_MONTH_CTRL(string ins_type_a,string def_ym)
    { 
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //刪除[TB_S_M_SALARY_MONTH_CTRL 薪資月結控制檔] 條件:發薪類別='A'//月薪 and 薪資年月=畫面.保費年月
            sb.Append(" delete from TB_S_M_SALARY_MONTH_CTRL where SALARY_TYPE='A' and SALARY_YM=@SALARY_YM  ");
            ht.Add("@SALARY_YM", def_ym);
            if (ins_type_a == "0")
            {
                sb.Append(" and OPERATION_ID IN ('C01','C02','C03','C04') ");
            }
            if (ins_type_a == "A")
            {
                sb.Append(" and OPERATION_ID ='C01' "); //勞保
            }
            if (ins_type_a == "B")
            {
                sb.Append(" and OPERATION_ID ='C02'"); //健保
            }
            if (ins_type_a == "C")
            {
                sb.Append(" and OPERATION_ID ='C03' "); //勞退
            }
            if (ins_type_a == "D")
            {
                sb.Append(" and OPERATION_ID ='C04' "); //團保
            }
            dbConn.ExecuteT(sb, ht, true);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public bool Delete_TB_I_R_FEES_MONTH(string ins_type_a, string def_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //刪除[TB_I_R_FEES_MONTH 月份保費代扣資料檔] 條件: 薪資月份=畫面.保費年月
            sb.Append(" delete from TB_I_R_FEES_MONTH where SALARY_YM=@SALARY_YM  ");
            ht.Add("@SALARY_YM", def_ym);
            if (ins_type_a == "A")
            {
                sb.Append(" and INS_TYPE ='A' "); //勞保
            }
            if (ins_type_a == "B")
            {
                sb.Append(" and INS_TYPE ='B'"); //健保
            }
            if (ins_type_a == "C")
            {
                sb.Append(" and INS_TYPE ='C' "); //勞退
            }
        
            dbConn.ExecuteT(sb, ht, true);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public bool Delete_TB_I_R_GROUP_MONTH(string ins_type_a, string def_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //刪除[TB_I_R_FEES_MONTH 月份保費代扣資料檔] 條件: 薪資月份=畫面.保費年月
            sb.Append(" delete from TB_I_R_GROUP_MONTH where SALARY_YM=@SALARY_YM  ");
            ht.Add("@SALARY_YM", def_ym);
            dbConn.ExecuteT(sb, ht, true);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable checkSP(string proc_id)
    {

        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select top (1) PROC_STATUS, PROC_LOG  ");
            sb.AppendLine("   from TB_H_R_SP_LOG                  ");
            sb.AppendLine("  where  PROC_ID = @PROC_ID            ");
            sb.AppendLine("  order by PROC_DT desc                ");
            ht.Add("@PROC_ID", proc_id);
            return dbConn.QueryT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public void Computer_FeeA(string def_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_I_COMPUTER_A_MONTH_FEES");
            ht.Add("@qry_salary_ym", def_ym);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
     public void Computer_FeeB( string def_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_I_COMPUTER_B_MONTH_FEES");
            ht.Add("@qry_salary_ym", def_ym);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
     public void Computer_FeeC(string def_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_I_COMPUTER_C_MONTH_FEES");
            ht.Add("@qry_salary_ym", def_ym);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
     public void Computer_FeeD(string def_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_I_COMPUTER_D_MONTH_FEES");
            ht.Add("@qry_salary_ym", def_ym);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

     //檢查計算保費之種類是否已被薪資擔當鎖定,若已鎖定不允重新計算
     internal DataTable getS_M_CRTL(string ins_type_a,string def_ym)
     {
         try
         {
             StringBuilder sb = new StringBuilder();
             Hashtable ht = new Hashtable();
             sb.Append("select count(0) resultCount from TB_S_M_SALARY_MONTH_CTRL");
             sb.Append(" where SALARY_TYPE='A' and SALARY_LOCKED='Y' and SALARY_YM=@SALARY_YM");
             ht.Add("@SALARY_YM", def_ym);
             if (ins_type_a == "0")
             {
                 sb.Append(" and OPERATION_ID IN ('C01','C02','C03','C04') ");
             }
             if (ins_type_a == "A")
             {
                 sb.Append(" and OPERATION_ID ='C01' "); //勞保
             }
             if (ins_type_a == "B")
             {
                 sb.Append(" and OPERATION_ID ='C02'"); //健保
             }
             if (ins_type_a == "C")
             {
                 sb.Append(" and OPERATION_ID ='C03' "); //勞退
             }
             if (ins_type_a == "D")
             {
                 sb.Append(" and OPERATION_ID ='C04' "); //團保
             }
            // dbConn.ExecuteT(sb, ht, true);
             return dbConn.Query(sb, ht);
         }
         catch (Exception)
         {
             throw;
         }
     }

    //取得最近一次薪資計算年月
    public string getLast_SALARY_YM()
    {
        try
        {
            string t = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select left(convert(varchar, Dateadd(month,1,convert(datetime,(select dbo.FN_S_SALARY_YM())+'01')),112),6) ");

            DataTable dt = dbConn.Query(sb);
            if (dt.Rows.Count > 0)
            {
                t = dt.Rows[0][0].ToString();
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getEmp_Name(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select EMP_NAME ");
            sb.AppendLine(" from VW_H_EMP_DATA ");
            sb.AppendLine(" where EMP_ID =@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }	
}