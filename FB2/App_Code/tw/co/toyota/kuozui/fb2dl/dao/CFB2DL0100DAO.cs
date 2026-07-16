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
/// CFB2DL0100DAO 的摘要描述
/// </summary>
public class CFB2DL0100DAO : BaseDAO
{
    public CFB2DL0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string EMP_ID { get; set; }
    public string COMPANY_CD { get; set; }
    public string EMP_CD { get; set; }
    public string MAIN_LEAVE_CD { get; set; }
    public string SUB_LEAVE_CD { get; set; }
    public string START_DT { get; set; }
    public string END_DT { get; set; }
    public string BASE_YEAR { get; set; }
    public string CAL_WORK_YEAR { get; set; }
    public string PAY_LEAVE_YEAR { get; set; }
    public string AVAILABLE_VALUE { get; set; }
    public string APPROVE_VALUE { get; set; }
    public string USED_PAY_LEAVE_VALUE { get; set; }
    public string ADJUST_VALUE { get; set; }
    public string ADJUST_DESC { get; set; }
    public string POLICY_PAY_LEAVE_DAY { get; set; }
    public string SALARY_SETTLE_CD { get; set; }
    public string SALARY_SETTLE_STATUS { get; set; }
    public string PAY_DT { get; set; }
    public string DATA_SOURCE { get; set; }
    public string REMARK { get; set; }
    public string ORI_DEPT_NO { get; set; }
    public string ORI_DEPT_FULL_NAME { get; set; }
    public string ORI_DIV_DEPT_FULL_NAME { get; set; }
    public string ORI_DEPT_NAME_20 { get; set; }
    public string ORI_DEPT_NAME_30 { get; set; }
    public string ORI_DEPT_NAME_40 { get; set; }
    public string DEFFER_VALUE { get; set; }
    
    //for查詢欄位
    public string ddl_SYS_CD { get; set; }

    public DataTable getParameter(string sys_cd, string main_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select CODE_VAL1 as WK_TRY_DAYS ");
            sb.AppendLine(" from TB_9_M_PARAMETER ");
            sb.AppendLine(" where SYS_CD = @SYS_CD ");
            sb.AppendLine(" and MAIN_CD = @MAIN_CD ");
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@MAIN_CD", main_cd);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }


    public DataTable getCommCode(string sys_cd, string main_cd, string is_valid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select sub_cd ,sub_cd+'-'+sub_desc as sub_desc ");
            sb.AppendLine(" from TB_9_M_COMM_D ");
            sb.AppendLine(" where SYS_CD = @SYS_CD ");
            sb.AppendLine(" and MAIN_CD = @MAIN_CD ");
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@MAIN_CD", main_cd);
            if (is_valid != "")
            {
                sb.AppendLine(" and IS_VALID = @IS_VALID ");
                ht.Add("@IS_VALID", is_valid);
            }

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //得一齊轉預借的生成子假別
    public DataTable getSub_Leave_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select SUB_LEAVE_CD,SUB_LEAVE_CD+'-'+SUB_LEAVE_DESC as SUB_LEAVE_DESC from TB_D_M_LEAVE_TYPE_D
                        where  IS_USED='Y'
                        and SUB_LEAVE_CD in('D3','D4')
                        ");
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getDept_name(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select DEPT_NAME ");
            sb.AppendLine(" from VW_H_DEPT_DATA ");
            sb.AppendLine(" where DEPT_NO = @DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    #region Qry
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string emp_name,
                             string dept_no, string emp_cd, string sub_leave_cd, string base_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (sortExpression.Contains("DEPT_NO"))
                sortExpression = sortExpression.Replace("DEPT_NO", "E.DEPT_NO");
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "A.EMP_ID");
            if (sortExpression.Contains("COMPANY_CD"))
                sortExpression = sortExpression.Replace("COMPANY_CD", "A.COMPANY_CD");
            if (sortExpression.Contains("EMP_CD"))
                sortExpression = sortExpression.Replace("EMP_CD", "A.EMP_CD");
            if (sortExpression.Contains("START_DT"))
                sortExpression = sortExpression.Replace("START_DT", "A.START_DT");
            if (sortExpression.Contains("END_DT"))
                sortExpression = sortExpression.Replace("END_DT", "A.END_DT");
            if (sortExpression.Contains("CAL_WORK_YEAR"))
                sortExpression = sortExpression.Replace("CAL_WORK_YEAR", "A.CAL_WORK_YEAR");
            if (sortExpression.Contains("PAY_LEAVE_YEAR"))
                sortExpression = sortExpression.Replace("PAY_LEAVE_YEAR", "A.PAY_LEAVE_YEAR");
            sb.AppendLine(" select * from");
            sb.AppendLine("     (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber ");
            sb.AppendLine("         ,A.EMP_ID as EMP_ID, E.EMP_NAME as EMP_NAME, E.DEPT_NO as DEPT_NO ");
            sb.AppendLine("         ,A.START_DT , A.END_DT, A.BASE_YEAR, A.CAL_WORK_YEAR, A.PAY_LEAVE_YEAR ");
            sb.AppendLine("         ,convert(Decimal(8,2),Round( A.AVAILABLE_VALUE/60.0,2)) as AVAILABLE_VALUE ");
            sb.AppendLine("         ,convert(Decimal(8,2),Round( A.APPROVE_VALUE/60.0,2)) as APPROVE_VALUE ");
            sb.AppendLine("         ,convert(Decimal(8,2),Round( A.USED_PAY_LEAVE_VALUE/60,2)) as USED_PAY_LEAVE_VALUE ");
            sb.AppendLine("         ,convert(Decimal(8,2),Round( A.ADJUST_VALUE/60.0,2))  as ADJUST_VALUE ");
            sb.AppendLine("         ,A.ADJUST_DESC, A.POLICY_PAY_LEAVE_DAY , A.PAY_DT ");
            sb.AppendLine("         ,E.DEPT_NO +'-'+ isnull(D.DEPT_NAME,'') as DEPT ");
            sb.AppendLine("         ,C1.COMPANY_SNAME as COMPANY_CD_DESC,  A.COMPANY_CD ");
            sb.AppendLine("         ,C2.SUB_DESC as EMP_CD_DESC ");
            sb.AppendLine("         ,L.SUB_LEAVE_DESC as SUB_LEAVE_DESC ");
            sb.AppendLine("         ,A.EMP_ID + A.MAIN_LEAVE_CD + A.SUB_LEAVE_CD + CONVERT(CHAR(8), A.START_DT, 112) as qdatakey ");
            //sb.AppendLine("         ,case when A.SALARY_SETTLE_STATUS ='Y' then '已計薪' ");
            //sb.AppendLine("               when A.SALARY_SETTLE_STATUS ='N' then '未計薪' end as SALARY_SETTLE_STATUS ");
            sb.AppendLine("         ,convert(Decimal(8,2),Round( A.DEFFER_VALUE/60.0,2))  as DEFFER_VALUE  ");
            sb.AppendLine("         ,A.SALARY_SETTLE_CD +'-'+ isnull(G.SUB_DESC,'') as SALARY_SETTLE_CD_DESC   ");
            sb.AppendLine("         ,A.SALARY_SETTLE_STATUS +'-'+ isnull(H.SUB_DESC,'') as SALARY_SETTLE_STATUS   ");
            sb.AppendLine("         from TB_D_M_EMP_AVAILABLE_LEAVE A ");
            sb.AppendLine("         left join TB_H_M_EMP E on  A.EMP_ID = E.EMP_ID ");
            sb.AppendLine("         left join VW_H_DEPT_DATA D on E.DEPT_NO = D.DEPT_NO ");
            sb.AppendLine("         left Join TB_H_M_COMPANY C1 on A.COMPANY_CD = C1.COMPANY_CD  ");
            sb.AppendLine("         left join TB_9_M_COMM_D C2 on C2.SYS_CD='HB' and C2.MAIN_CD='EMP_CD' and A.EMP_CD = C2.SUB_CD ");
            sb.AppendLine("         left join TB_D_M_LEAVE_TYPE_D L on A.MAIN_LEAVE_CD = L.MAIN_LEAVE_CD and A.SUB_LEAVE_CD = L.SUB_LEAVE_CD");
            sb.AppendLine("         left join TB_9_M_COMM_D G on G.MAIN_CD='SALARY_SETTLE_CD'  and G.SYS_CD='DH'  and G.IS_VALID='Y'  and A.SALARY_SETTLE_CD = G.SUB_CD ");
            sb.AppendLine("         left join TB_9_M_COMM_D H on H.MAIN_CD='SALARY_SETTLE_STATUS'  and H.SYS_CD='DH'  and H.IS_VALID='Y'  and A.SALARY_SETTLE_STATUS = H.SUB_CD ");
            sb.AppendLine("         where 1=1 ");

            if (emp_id != "")
            {
                sb.AppendLine(" and A.EMP_ID like '%'+ @EMP_ID +'%' ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name != "")
            {
                sb.AppendLine(" and E.EMP_NAME like '%'+ @EMP_NAME +'%' ");
                ht.Add("@EMP_NAME", emp_name);
            }
            if (dept_no != "")
            {
                sb.AppendLine(" and E.DEPT_NO like '%'+ @DEPT_NO +'%' ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_cd != "")
            {
                sb.AppendLine(" and A.EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            if (sub_leave_cd != "")
            {
                sb.AppendLine(" and A.SUB_LEAVE_CD = @SUB_LEAVE_CD ");
                ht.Add("@SUB_LEAVE_CD", sub_leave_cd);
            }
            if (base_year != "")
            {
                sb.AppendLine(" and BASE_YEAR=@BASE_YEAR ");
                //sb.AppendLine(" and year(A.START_DT) >= @BASE_YEAR and year(A.END_DT) <=@BASE_YEAR ");
                ht.Add("@BASE_YEAR", base_year);
            }

            sb.AppendLine("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public int getCount(int startRowIndex, int maximumRows, string emp_id, string emp_name,
                             string dept_no, string emp_cd, string sub_leave_cd, string base_year)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine(" from TB_D_M_EMP_AVAILABLE_LEAVE A ");
            sb.AppendLine(" left join TB_H_M_EMP E on  A.EMP_ID = E.EMP_ID ");
            sb.AppendLine(" where 1=1 ");

            if (emp_id != "")
            {
                sb.AppendLine(" and A.EMP_ID like '%'+ @EMP_ID +'%' ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name != "")
            {
                sb.AppendLine(" and E.EMP_NAME like '%'+ @EMP_NAME +'%' ");
                ht.Add("@EMP_NAME", emp_name);
            }
            if (dept_no != "")
            {
                sb.AppendLine(" and E.DEPT_NO like '%'+ @DEPT_NO +'%' ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_cd != "")
            {
                sb.AppendLine(" and A.EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            if (sub_leave_cd != "")
            {
                sb.AppendLine(" and A.SUB_LEAVE_CD = @SUB_LEAVE_CD ");
                ht.Add("@SUB_LEAVE_CD", sub_leave_cd);
            }
            if (base_year != "")
            {
                sb.AppendLine(" and BASE_YEAR=@BASE_YEAR ");
                //sb.AppendLine(" and BASE_YEAR=@BASE_YEAR "); sb.AppendLine(" and BASE_YEAR=@BASE_YEAR ");
                ht.Add("@BASE_YEAR", base_year);
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
    public string deleteData(string qdatakey)
    {
        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //寫log
        sb.Append(" update TB_D_M_EMP_AVAILABLE_LEAVE set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DL010' ");
        sb.Append(" where EMP_ID + MAIN_LEAVE_CD + SUB_LEAVE_CD + CONVERT(CHAR(8), START_DT, 112) = @QDATAKEY; ");

        sb.AppendLine(" delete from TB_D_M_EMP_AVAILABLE_LEAVE   ");
        sb.AppendLine(" where EMP_ID + MAIN_LEAVE_CD + SUB_LEAVE_CD + CONVERT(CHAR(8), START_DT, 112) = @QDATAKEY; ");
        ht.Add("@QDATAKEY", qdatakey);
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
        dbConn.ExecuteT(sb, ht, true);
        return "0";
    }


    #endregion

    #region Dtl
    //取得修改資料
    public DataTable getModData(string qdatakey)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select L.EMP_ID as EMP_ID ");
            sb.AppendLine("       , L.COMPANY_CD, L.EMP_CD, L.MAIN_LEAVE_CD, L.SUB_LEAVE_CD, L.START_DT, L.END_DT ");
            sb.AppendLine("       , L.BASE_YEAR, L.CAL_WORK_YEAR, L.PAY_LEAVE_YEAR ");
            sb.AppendLine("       ,convert(Decimal(8,2),Round( L.AVAILABLE_VALUE/60.0,2)) as AVAILABLE_VALUE ");
            sb.AppendLine("       ,convert(Decimal(8,2),Round( L.APPROVE_VALUE/60.0,2)) as APPROVE_VALUE ");
            sb.AppendLine("       ,convert(Decimal(8,2),Round( L.USED_PAY_LEAVE_VALUE/60.0,2)) as USED_PAY_LEAVE_VALUE ");
            sb.AppendLine("       ,convert(Decimal(8,2),Round( L.ADJUST_VALUE/60.0,2))  as ADJUST_VALUE ");
            sb.AppendLine("       ,convert(Decimal(8,2),Round( L.DEFFER_VALUE/60.0,2))  as DEFFER_VALUE ");
            sb.AppendLine("       ,L.ADJUST_DESC, L.POLICY_PAY_LEAVE_DAY ");
            sb.AppendLine("       , L.SALARY_SETTLE_CD, L.SALARY_SETTLE_STATUS, L.PAY_DT,DATA_SOURCE,L.REMARK, E.EMP_NAME,E.DEPT_NO ");
            sb.AppendLine("       , E.DEPT_FULL_NAME, E.COMPANY_CD, C1.COMPANY_SNAME as COMPANY_NAME,E.EMP_CD, E.EMP_DESC ");
            sb.AppendLine("       ,E.DL_GEN_DT ");
            sb.AppendLine(" from TB_D_M_EMP_AVAILABLE_LEAVE L");
            sb.AppendLine(" left join VW_H_EMP_DATA E on L.EMP_ID = E.EMP_ID");
            sb.AppendLine(" left Join TB_H_M_COMPANY C1 on E.COMPANY_CD = C1.COMPANY_CD  ");
            sb.AppendLine(" where L.EMP_ID + L.MAIN_LEAVE_CD + L.SUB_LEAVE_CD + CONVERT(CHAR(8), L.START_DT, 112) = @QDATAKEY ");
            ht.Add("@QDATAKEY", qdatakey);

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getEmpData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select V.EMP_NAME,V.DEPT_NO,V.DEPT_FULL_NAME,V.COMPANY_CD,C1.COMPANY_SNAME as COMPANY_NAME,V.EMP_CD, CONVERT(VARCHAR(10), V.DL_GEN_DT,111) as DL_GEN_DT   ");
            sb.AppendLine("       ,V.EMP_DESC,V.PJOB_CD,V.WORK_DAYS,V.DIV_DEPT_FULL_NAME,V.DEPT_NAME_20,V.DEPT_NAME_30,V.DEPT_NAME_40 ");
            sb.AppendLine(" from VW_H_EMP_DATA V ");
            sb.AppendLine(" left Join TB_H_M_COMPANY C1 on V.COMPANY_CD = C1.COMPANY_CD  ");
            sb.AppendLine(" where V.EMP_ID =@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getBonus_Plan(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select PLAN_END_DT ");
            sb.AppendLine(" from TB_H_M_BONUS_PLAN_H ");
            sb.AppendLine(" where EMP_ID =@EMP_ID and END_DT is NULL ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getPay_Leave_Days(string pay_leave_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select PAY_LEAVE_DAYS ");
            sb.AppendLine(" from TB_D_M_PAY_LEAVE_DAYS ");
            sb.AppendLine(" where PAY_LEAVE_YEAR =@PAY_LEAVE_YEAR ");
            ht.Add("@PAY_LEAVE_YEAR", pay_leave_year);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getExistData(string emp_id, string main_leave_cd, string sub_leave_cd, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(1) total ");
            sb.AppendLine(" from TB_D_M_EMP_AVAILABLE_LEAVE ");
            sb.AppendLine(" where EMP_ID + MAIN_LEAVE_CD + SUB_LEAVE_CD + CONVERT(CHAR(8), START_DT, 112) = @QDATAKEY ");
            ht.Add("@QDATAKEY", emp_id + main_leave_cd + sub_leave_cd + start_dt);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getBASE_YEAR_Repeat()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(1) total ");
            sb.AppendLine(" from TB_D_M_EMP_AVAILABLE_LEAVE ");
            sb.AppendLine(" where 1=1 ");
            sb.AppendLine(" and EMP_ID = @EMP_ID  and  MAIN_LEAVE_CD = @MAIN_LEAVE_CD ");
            sb.AppendLine(" and SUB_LEAVE_CD = @SUB_LEAVE_CD  and  BASE_YEAR = @BASE_YEAR ");
            sb.AppendLine(" and SALARY_SETTLE_CD = @SALARY_SETTLE_CD  ");
            ht.Add("@EMP_ID", EMP_ID );
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@BASE_YEAR", BASE_YEAR);
            ht.Add("@SALARY_SETTLE_CD", SALARY_SETTLE_CD);
            return dbConn.Query(sb, ht, true);
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

            sb.AppendLine(" insert into TB_D_M_EMP_AVAILABLE_LEAVE ");
            sb.AppendLine(" (EMP_ID, COMPANY_CD, EMP_CD, MAIN_LEAVE_CD, SUB_LEAVE_CD, START_DT, END_DT, BASE_YEAR, CAL_WORK_YEAR, PAY_LEAVE_YEAR, AVAILABLE_VALUE ");
            sb.AppendLine(" , APPROVE_VALUE, USED_PAY_LEAVE_VALUE, ADJUST_VALUE ,ADJUST_DESC, POLICY_PAY_LEAVE_DAY, SALARY_SETTLE_CD, SALARY_SETTLE_STATUS ");
            sb.AppendLine(" , PAY_DT,DATA_SOURCE,REMARK,ORI_DEPT_NO,ORI_DEPT_FULL_NAME,ORI_DIV_DEPT_FULL_NAME,ORI_DEPT_NAME_20,ORI_DEPT_NAME_30,ORI_DEPT_NAME_40 ");
            sb.AppendLine(" ,DEFFER_VALUE ");
            sb.AppendLine(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.AppendLine(" values (@EMP_ID,@COMPANY_CD,@EMP_CD,@MAIN_LEAVE_CD,@SUB_LEAVE_CD,@START_DT,@END_DT,@BASE_YEAR,@CAL_WORK_YEAR,@PAY_LEAVE_YEAR,@AVAILABLE_VALUE ");
            sb.AppendLine(" ,@APPROVE_VALUE,@USED_PAY_LEAVE_VALUE,@ADJUST_VALUE,@ADJUST_DESC,@POLICY_PAY_LEAVE_DAY,@SALARY_SETTLE_CD,@SALARY_SETTLE_STATUS ");
            sb.AppendLine(" ,@PAY_DT,@DATA_SOURCE,@REMARK,@ORI_DEPT_NO,@ORI_DEPT_FULL_NAME,@ORI_DIV_DEPT_FULL_NAME,@ORI_DEPT_NAME_20,@ORI_DEPT_NAME_30,@ORI_DEPT_NAME_40 ");
            sb.AppendLine(" ,@DEFFER_VALUE ");
            sb.AppendLine(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID) ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@BASE_YEAR ", BASE_YEAR);
            if (CAL_WORK_YEAR == "")
                ht.Add("@CAL_WORK_YEAR", "0");
            else
                ht.Add("@CAL_WORK_YEAR", CAL_WORK_YEAR);

            if (PAY_LEAVE_YEAR == "")
                ht.Add("@PAY_LEAVE_YEAR", "0");
            else
                ht.Add("@PAY_LEAVE_YEAR", PAY_LEAVE_YEAR);

            if (AVAILABLE_VALUE == "")
                ht.Add("@AVAILABLE_VALUE", "0");
            else
                ht.Add("@AVAILABLE_VALUE", (Convert.ToDouble(AVAILABLE_VALUE) * 60).ToString("0"));

            if (APPROVE_VALUE == "")
                ht.Add("@APPROVE_VALUE", "0");
            else
                ht.Add("@APPROVE_VALUE", (Convert.ToDouble(APPROVE_VALUE) * 60).ToString("0"));

            if (USED_PAY_LEAVE_VALUE == "")
                ht.Add("@USED_PAY_LEAVE_VALUE", "0");
            else
                ht.Add("@USED_PAY_LEAVE_VALUE", (Convert.ToDouble(USED_PAY_LEAVE_VALUE) * 60).ToString("0"));

            if (ADJUST_VALUE == "")
                ht.Add("@ADJUST_VALUE", "0");
            else
                ht.Add("@ADJUST_VALUE", (Convert.ToDouble(ADJUST_VALUE) * 60).ToString("0"));
            
            if (DEFFER_VALUE == "")
                ht.Add("@DEFFER_VALUE", "0");
            else
                ht.Add("@DEFFER_VALUE", (Convert.ToDouble(DEFFER_VALUE) * 60).ToString("0"));

            ht.Add("@ADJUST_DESC", ADJUST_DESC);

            if (POLICY_PAY_LEAVE_DAY == "")
                ht.Add("@POLICY_PAY_LEAVE_DAY", "0");
            else
                ht.Add("@POLICY_PAY_LEAVE_DAY", POLICY_PAY_LEAVE_DAY);

            

            ht.Add("@SALARY_SETTLE_CD", SALARY_SETTLE_CD);
            ht.Add("@SALARY_SETTLE_STATUS", "N");
            ht.Add("@PAY_DT", DBNull.Value);
            ht.Add("@DATA_SOURCE", "可用假維護");
            ht.Add("@REMARK", REMARK);
            ht.Add("@ORI_DEPT_NO", ORI_DEPT_NO);
            ht.Add("@ORI_DEPT_FULL_NAME", ORI_DEPT_FULL_NAME);
            ht.Add("@ORI_DIV_DEPT_FULL_NAME", ORI_DIV_DEPT_FULL_NAME);
            ht.Add("@ORI_DEPT_NAME_20", ORI_DEPT_NAME_20);
            ht.Add("@ORI_DEPT_NAME_30", ORI_DEPT_NAME_30);
            ht.Add("@ORI_DEPT_NAME_40", ORI_DEPT_NAME_40);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DL010");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void addHonor(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_H_M_EMP ");
            sb.AppendLine(" Set HONOR_YEAR = @HONOR_YEAR,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@HONOR_YEAR", BASE_YEAR);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DL010");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void updateData(string emp_id, string main_leave_cd, string sub_leave_cd, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("Update TB_D_M_EMP_AVAILABLE_LEAVE ");
            sb.AppendLine(" Set AVAILABLE_VALUE = @AVAILABLE_VALUE,APPROVE_VALUE = @APPROVE_VALUE,USED_PAY_LEAVE_VALUE = @USED_PAY_LEAVE_VALUE ");
            sb.AppendLine(" ,ADJUST_VALUE = @ADJUST_VALUE,ADJUST_DESC = @ADJUST_DESC,POLICY_PAY_LEAVE_DAY = @POLICY_PAY_LEAVE_DAY,END_DT=@END_DT ");
            sb.AppendLine(" ,DEFFER_VALUE = @DEFFER_VALUE ");
            sb.AppendLine(" ,SALARY_SETTLE_CD = @SALARY_SETTLE_CD,REMARK = @REMARK,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where EMP_ID + MAIN_LEAVE_CD + SUB_LEAVE_CD + CONVERT(CHAR(8), START_DT, 112) = @QDATAKEY");
            ht.Add("@QDATAKEY", emp_id + main_leave_cd + sub_leave_cd + start_dt);

            if (AVAILABLE_VALUE == "")
                ht.Add("@AVAILABLE_VALUE", "0");
            else
                ht.Add("@AVAILABLE_VALUE", (Convert.ToDouble(AVAILABLE_VALUE) * 60).ToString("0"));

            if (APPROVE_VALUE == "")
                ht.Add("@APPROVE_VALUE", "0");
            else
                ht.Add("@APPROVE_VALUE", (Convert.ToDouble(APPROVE_VALUE) * 60).ToString("0"));

            if (USED_PAY_LEAVE_VALUE == "")
                ht.Add("@USED_PAY_LEAVE_VALUE", "0");
            else
                ht.Add("@USED_PAY_LEAVE_VALUE", (Convert.ToDouble(USED_PAY_LEAVE_VALUE) * 60).ToString("0"));

            if (ADJUST_VALUE == "")
                ht.Add("@ADJUST_VALUE", "0");
            else
                ht.Add("@ADJUST_VALUE", (Convert.ToDouble(ADJUST_VALUE) * 60).ToString("0"));
            
            if (DEFFER_VALUE == "")
                ht.Add("@DEFFER_VALUE", "0");
            else
                ht.Add("@DEFFER_VALUE", (Convert.ToDouble(DEFFER_VALUE) * 60).ToString("0"));


            ht.Add("@ADJUST_DESC", ADJUST_DESC);

            if (POLICY_PAY_LEAVE_DAY == "")
                ht.Add("@POLICY_PAY_LEAVE_DAY", "0");
            else
                ht.Add("@POLICY_PAY_LEAVE_DAY", POLICY_PAY_LEAVE_DAY);

         

            ht.Add("@SALARY_SETTLE_CD", SALARY_SETTLE_CD);

            ht.Add("@END_DT", END_DT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DL010");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region "other"
    //特休生成
    public void RunProcSP_H_EMP_PAY_LEAVE(string Year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_H_EMP_PAY_LEAVE");
            ht.Add("@p_YEAR", Year);
            ht.Add("@p_UserID", SessionHandle.Current.emp_id);
            ht.Add("@p_FuncID", "FB2DL010");

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    /// <summary>
    /// 檢查TB_D_M_EMP_AVAILABLE_LEAVE有無一樣核假年度資料，有回傳"true"，無回傳"false"
    /// </summary>
    /// <param name="Year">核假年度</param>
    /// <returns></returns>
    public bool checkGenerateIsExsit(string Year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select COUNT(*) total           ");
            sb.AppendLine("	from TB_D_M_EMP_AVAILABLE_LEAVE ");
            sb.AppendLine("	where MAIN_LEAVE_CD = 'D'       ");
            sb.AppendLine("	and SUB_LEAVE_CD = 'D0'        ");
            sb.AppendLine("	and BASE_YEAR = @BASE_YEAR      ");
            ht.Add("@BASE_YEAR", Year);

            DataTable dt = dbConn.Query(sb, ht, true);
            if (Convert.ToInt32(dt.Rows[0]["total"]) > 0)
                return true;
            else
                return false;
        }
        catch (Exception)
        {
            throw;
        }
    }
    /// <summary>
    /// 檢查TB_D_M_EMP_AVAILABLE_LEAVE的計薪狀態只能為"N"，通過回傳"true"，否則回傳"false"
    /// </summary>
    /// <param name="Year">核假年度</param>
    /// <returns></returns>
    public bool checkStatusIsN(string Year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select COUNT(*) total           ");
            sb.AppendLine("	from TB_D_M_EMP_AVAILABLE_LEAVE ");
            sb.AppendLine("	where MAIN_LEAVE_CD = 'D'       ");
            sb.AppendLine("	and SUB_LEAVE_CD = 'D0'        ");
            sb.AppendLine("	and BASE_YEAR = @BASE_YEAR      ");
            sb.AppendLine("	and SALARY_SETTLE_STATUS <> 'N' ");
            ht.Add("@BASE_YEAR", Year);

            DataTable dt = dbConn.Query(sb, ht, true);
            if (Convert.ToInt32(dt.Rows[0]["total"]) > 0)
                return false;
            else
                return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    //一齊轉預借
    public void RunProcSP_H_EMP_POLICY_PAY_LEAVE(string Year, string sub_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_H_EMP_POLICY_PAY_LEAVE");
            ht.Add("@pYear", Year);
            ht.Add("@pDate", DateTime.Now.Date);
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2DL010");

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //確認SP執行是否成功
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
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //excel下載資料table
    public DataTable getExcelData(string base_year, string dept_no, string emp_cd, string emp_id, string join_sdt, string join_edt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select LLL.BASE_YEAR,LLL.EMP_ID,SPECIAL,HONOR  ");
            sb.AppendLine("	      ,V.PLANT_NAME,V.EMP_NAME,V.LEVEL_CD,V.GRADE_CD,V.JOIN_DT,V.DEPT_NO,V.DEPT_NAME_20 ");
            sb.AppendLine("	      ,V.DEPT_NAME_30,V.DEPT_NAME_40,V.EMP_CD ");
            sb.AppendLine("  from (select EMP_ID,BASE_YEAR ");
            sb.AppendLine("          from TB_D_M_EMP_AVAILABLE_LEAVE ");
            sb.AppendLine("         group by EMP_ID,BASE_YEAR) LLL  ");
            sb.AppendLine("	         left join (select BASE_YEAR,EMP_ID,convert(Decimal(8,2),Round(SUM((AVAILABLE_VALUE/60.0)/8.0),2)) as SPECIAL ");
            sb.AppendLine("	                      from TB_D_M_EMP_AVAILABLE_LEAVE ");
            sb.AppendLine("	                     where MAIN_LEAVE_CD = 'D' ");
            sb.AppendLine("	                       and SUB_LEAVE_CD = 'D0' ");
            sb.AppendLine("	                     group by EMP_ID,BASE_YEAR ");
            sb.AppendLine("                     ) A1 on A1.EMP_ID = LLL.EMP_ID and A1.BASE_YEAR = LLL.BASE_YEAR ");
            sb.AppendLine(" 	     left join (select BASE_YEAR,EMP_ID,convert(Decimal(8,2),Round(SUM((AVAILABLE_VALUE/60.0)/8.0),2)) as HONOR ");
            sb.AppendLine("	                      from TB_D_M_EMP_AVAILABLE_LEAVE ");
            sb.AppendLine("                      where MAIN_LEAVE_CD = 'M' ");
            sb.AppendLine("	                       and SUB_LEAVE_CD = 'M0' ");
            sb.AppendLine("	                     group by EMP_ID,BASE_YEAR  ");
            sb.AppendLine("	                    ) A2 on A2.EMP_ID = LLL.EMP_ID and A2.BASE_YEAR = LLL.BASE_YEAR ");
            sb.AppendLine("	 left join VW_H_EMP_DATA V on LLL.EMP_ID = V.EMP_ID  ");
            sb.AppendLine(" where (HONOR is not null or SPECIAL is not null) ");
            sb.AppendLine("   and LLL.BASE_YEAR = @BASE_YEAR ");
            sb.AppendLine(@"and V.EMP_STATUS='01'
                            and V.WS_CD='W'
	                        and  V.WORK_CD in ('A','B','C','T')
	                        and V.PJOB_CD in (
		                        select PJOB_CD from  VW_TB_H_M_PJOB
		                        where WS_CD='W'
		                        and PJOB_LEVEL in ('06','07','08','10')
		                        and PJOB_CD not in('MF10','MF20')
	                        )
                           
            ");

            ht.Add("@BASE_YEAR", base_year);
            if (!string.IsNullOrEmpty(dept_no))
            {
                sb.AppendLine(" and V.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + '%');
            }
            if (!string.IsNullOrEmpty(emp_cd))
            {
                sb.AppendLine(" and V.EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            if (!string.IsNullOrEmpty(emp_id))
            {
                sb.AppendLine(" and LLL.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (!string.IsNullOrEmpty(join_sdt))
            {
                sb.AppendLine(" and V.JOIN_DT >= @JOIN_SDT ");
                ht.Add("@JOIN_SDT", join_sdt);
            }
            if (!string.IsNullOrEmpty(join_edt))
            {
                sb.AppendLine(" and V.JOIN_DT <= @JOIN_EDT ");
                ht.Add("@JOIN_EDT", join_edt);
            }
            sb.AppendLine(" order by V.PLANT_CD,V.DEPT_NO,V.EMP_ID  ");
             
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
}