using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2SC3100DAO 的摘要描述
/// </summary>
public class CFB2SC3100DAO : BaseDAO
{
    public string PROCESS_STATUS { get; set; }
    public string GROUP_NAME { get; set; }
    public string ACC_CD { get; set; }
    public string ACC_DEPT_NO { get; set; }
    public string JPN_CD { get; set; }
    public string DEPT_NO { get; set; }
    public string DEPT_NAME_20 { get; set; }
    public string DEPT_NAME_30 { get; set; }
    public string DEPT_NAME_40 { get; set; }
    public string PAY_TYPE { get; set; }
    public string AMT { get; set; }
    public string COMPANY_CD { get; set; }
    public string PLANT_CD { get; set; }
    public string SALARY_TYPE { get; set; }
    public string PAY_KIND { get; set; }
    public string EMP_ID { get; set; }
    public string PAY_ID { get; set; }
    public string SALARY_DT { get; set; }
    public string SALARY_YM { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public string PAY_KIND_DESC { get; set; }    

    public CFB2SC3100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string salary_ym,
                         string salary_sdt, string salary_edt, string salary_type, string process_status)
    {
        try
        {
            if (sortExpression.Contains("SALARY_YM"))
                sortExpression = sortExpression.Replace("SALARY_YM", "t.SALARY_YM");
            if (sortExpression.Contains("SALARY_TYPE"))
                sortExpression = sortExpression.Replace("SALARY_TYPE", "t.SALARY_TYPE");
            if (sortExpression.Contains("SALARY_DT"))
                sortExpression = sortExpression.Replace("SALARY_DT", "p.SALARY_DT");
            if (sortExpression.Contains("PAY_KIND"))
                sortExpression = sortExpression.Replace("PAY_KIND", "t.PAY_KIND");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber, ");
            sb.AppendLine(" CONVERT(VARCHAR(10), t.SALARY_DT,111 ) as SALARY_DT,t.SALARY_EDT ,t.DUTY_SDT,t.DUTY_EDT ");
            sb.AppendLine(" ,t.SALARY_YM, t.SALARY_SDT, t.SALARY_TYPE, t.PAY_KIND, t.PROCESS_STATUS ");
            sb.AppendLine(" ,t.PROCESS_STATUS as hid_PROCESS_STATUS ");
            sb.AppendLine(" ,t.PROCESS_STATUS+'-'+d.SUB_DESC as PROCESS_STATUS_DESC ");
            sb.AppendLine(" ,isnull(p.PAY_ID,'') PAY_ID,CONVERT(VARCHAR(10), p.PAY_DT,111 )as PAY_DT ");
            sb.AppendLine(" ,t.SALARY_TYPE as hid_SALARY_TYPE,t.PAY_KIND as hid_PAY_KIND ");
            sb.AppendLine(" ,t.SALARY_TYPE+'-'+e.SUB_DESC as SALARY_TYPE_DESC ");
            sb.AppendLine(" ,CASE WHEN t.PAY_KIND ='9999' then '9999-月薪資' else t.PAY_KIND+'-'+ s.SALARY_NAME end as PAY_KIND_DESC ");
            sb.AppendLine(" from TB_S_M_SALARY_CAL_H t ");
            sb.AppendLine(" left join TB_S_M_SALARY_ITEM s on  t.PAY_KIND = s.SALARY_ID ");
            sb.AppendLine(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD ");
            sb.AppendLine(" left join TB_S_M_SALARY_PAY_H p on  p.SALARY_DT= t.SALARY_DT and p.SALARY_TYPE = t.SALARY_TYPE and p.PAY_KIND = t.PAY_KIND ");
            sb.AppendLine(" left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and e.MAIN_CD='SALARY_TYPE' and  t.SALARY_TYPE = e.SUB_CD ");
            sb.AppendLine(" where 1=1 ");

            if (salary_sdt != "")
            {
                sb.AppendLine(" and t.SALARY_DT >= @SALARY_SDT ");
                ht.Add("@SALARY_SDT", salary_sdt);
            }
            if (salary_edt != "")
            {
                sb.AppendLine(" and t.SALARY_DT <= @SALARY_EDT ");
                ht.Add("@SALARY_EDT", salary_edt);
            }
            if (salary_ym != "")
            {
                sb.AppendLine(" and t.SALARY_YM = LEFT(@SALARY_YM,4)+RIGHT(@SALARY_YM,2) ");
                ht.Add("@SALARY_YM", salary_ym);
            }
            if (salary_type != "-1")
            {
                sb.AppendLine(" and t.SALARY_TYPE =@SALARY_TYPE ");
                ht.Add("@SALARY_TYPE", salary_type);
            }
            if (process_status != "-1")
            {
                sb.AppendLine(" and t.PROCESS_STATUS =@PROCESS_STATUS ");
                ht.Add("@PROCESS_STATUS", process_status);
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
    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows, string salary_ym,
                         string salary_sdt, string salary_edt, string salary_type, string process_status)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record ");
            sb.AppendLine(" from TB_S_M_SALARY_CAL_H t ");
            sb.AppendLine(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD ");
            sb.AppendLine(" left join TB_S_M_SALARY_PAY_H p on  p.SALARY_DT= t.SALARY_DT and p.SALARY_TYPE = t.SALARY_TYPE and p.PAY_KIND = t.PAY_KIND ");
            sb.AppendLine(" left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and e.MAIN_CD='SALARY_TYPE' and  t.SALARY_TYPE = e.SUB_CD ");
            sb.AppendLine(" where 1=1 ");

            if (salary_sdt != "")
            {
                sb.AppendLine(" and t.SALARY_DT >= @SALARY_SDT ");
                ht.Add("@SALARY_SDT", salary_sdt);
            }
            if (salary_edt != "")
            {
                sb.AppendLine(" and t.SALARY_DT <= @SALARY_EDT ");
                ht.Add("@SALARY_EDT", salary_edt);
            }
            if (salary_ym != "")
            {
                sb.AppendLine(" and t.SALARY_YM = LEFT(@SALARY_YM,4)+RIGHT(@SALARY_YM,2) ");
                ht.Add("@SALARY_YM", salary_ym);
            }
            if (salary_type != "-1")
            {
                sb.AppendLine(" and t.SALARY_TYPE =@SALARY_TYPE ");
                ht.Add("@SALARY_TYPE", salary_type);
            }
            if (process_status != "-1")
            {
                sb.AppendLine(" and t.PROCESS_STATUS =@PROCESS_STATUS ");
                ht.Add("@PROCESS_STATUS", process_status);
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
    public DataTable getSALARY_TYPE(string sys_cd, string main_cd, string is_valid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select sub_cd ,sub_desc From TB_9_M_COMM_D ");
            sb.AppendLine("Where sys_cd = @sys_cd ");
            sb.AppendLine("and main_cd = @main_cd ");
            sb.AppendLine("and is_valid = @is_valid ");
            ht.Add("@sys_cd", sys_cd);
            ht.Add("@main_cd", main_cd);
            ht.Add("@is_valid", is_valid);
            string a = sb.ToString();
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getPROCESS_STATUS(string sys_cd, string main_cd, string is_valid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select sub_cd ,sub_desc From TB_9_M_COMM_D ");
            sb.AppendLine("Where sys_cd = @sys_cd ");
            sb.AppendLine("and main_cd = @main_cd ");
            sb.AppendLine("and is_valid = @is_valid ");
            ht.Add("@sys_cd", sys_cd);
            ht.Add("@main_cd", main_cd);
            ht.Add("@is_valid", is_valid);
            string a = sb.ToString();
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getCOMPANY_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("SELECT COMPANY_CD, COMPANY_SNAME FROM TB_H_M_COMPANY ");
            string a = sb.ToString();
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    //20201026
    public string getCOMPANY_SNAME(string COMPANY_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("SELECT COMPANY_CD, COMPANY_SNAME FROM TB_H_M_COMPANY ");
            sb.AppendLine("where  COMPANY_CD = @COMPANY_CD ");
            ht.Add("@COMPANY_CD", COMPANY_CD);
            DataTable dt =  dbConn.Query(sb, ht);

            string company_sname="";
            if(dt.Rows.Count>0)
                company_sname = dt.Rows[0]["COMPANY_SNAME"].ToString();
            else
                 company_sname =COMPANY_CD+"公司";

            return company_sname;
        }
        catch
        {
            throw;
        }
    }

    internal void delete_sm(string salary_type, string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("delete from  TB_S_M_SALARY_MONTH ");
            sb.AppendLine(" where ");
            //sb.AppendLine(" SALARY_TYPE=@SALARY_TYPE and ");
            sb.AppendLine(" SALARY_DT = @SALARY_DT");
            //ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void delete_srd(string salary_type, string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("delete from  TB_S_M_SALARY_REPORT_D ");
            sb.AppendLine(" where ");
            //sb.AppendLine(" PAY_TYPE=@PAY_TYPE and ");
            sb.AppendLine(" SALARY_DT = @SALARY_DT");
            ht.Add("@SALARY_DT", salary_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void delete_so(string salary_type, string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("delete from  TB_S_M_SALARY_OTHER ");
            sb.AppendLine(" where SALARY_TYPE=@SALARY_TYPE and SALARY_DT = @SALARY_DT");
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void delete_srod(string salary_type, string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("delete from  TB_S_M_SALARY_REPORT_O_D ");
            sb.AppendLine(" where SALARY_TYPE=@SALARY_TYPE and SALARY_DT = @SALARY_DT");
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal DataTable count_sm(string salary_type, string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select COUNT(0) smcount from TB_S_M_SALARY_MONTH ");
            sb.AppendLine(" where ");
            //sb.AppendLine(" SALARY_TYPE=@SALARY_TYPE and ");
            sb.AppendLine(" SALARY_DT = @SALARY_DT ");
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable count_srd(string salary_type, string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select COUNT(0) srdcount from TB_S_M_SALARY_REPORT_D ");
            sb.AppendLine(" where ");
            //sb.AppendLine(" SALARY_TYPE=@SALARY_TYPE and ");
            sb.AppendLine(" SALARY_DT = @SALARY_DT ");
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable count_srod(string salary_type, string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select COUNT(0) srodcount from TB_S_M_SALARY_REPORT_O_D ");
            sb.AppendLine(" where SALARY_TYPE=@SALARY_TYPE and SALARY_DT = @SALARY_DT");
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable count_so(string salary_type, string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select COUNT(0) socount from TB_S_M_SALARY_OTHER ");
            sb.AppendLine(" where SALARY_TYPE=@SALARY_TYPE and SALARY_DT = @SALARY_DT");
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getEmpResult(string salary_type, string salary_dt, string pay_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("SELECT distinct t1.SALARY_DT,t1.SALARY_YM,t1.ACC_CD,t1.ACC_DEPT_NO,t1.EMP_ID,t1.COMPANY_CD,t1.JPN_CD");
            sb.AppendLine("      ,t1.DEPT_NO,vm.DEPT_NAME_20,vm.DEPT_NAME_30,vm.DEPT_NAME_40,t1.PLANT_CD                       ");
            sb.AppendLine("      ,CASE WHEN p.PAY_TYPE ='T' then 'T'  WHEN p.PAY_TYPE ='G' then 'G' else 'Y' end as PAY_TYPE   ");
            sb.AppendLine("FROM TB_S_S_SALARY_PAY p                                                                            ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT t1 on t1.SALARY_YM = p.DATA_YM and t1.EMP_ID = p.EMP_ID and p.SALARY_DT = t1.SALARY_DT ");
            sb.AppendLine("left join VW_H_DEPT_DATA vm on t1.DEPT_NO = vm.DEPT_NO                                              ");
            sb.AppendLine("where p.SALARY_DT = @SALARY_DT and p.SALARY_TYPE = @SALARY_TYPE and p.PAY_KIND = left(@PAY_KIND,4)  ");
            sb.AppendLine("Order By t1.EMP_ID                                                                                  ");

            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@PAY_KIND", pay_kind);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getEmpResultTemp(string salary_type, string salary_dt, string pay_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("SELECT t1.SALARY_DT,t1.SALARY_YM,t1.ACC_CD,t1.ACC_DEPT_NO,t1.EMP_ID,t1.COMPANY_CD,t1.JPN_CD								");
            sb.AppendLine("      ,t1.DEPT_NO,vm.DEPT_NAME_20,vm.DEPT_NAME_30,vm.DEPT_NAME_40,t1.PLANT_CD 											");
            sb.AppendLine("      ,CASE WHEN p.PAY_TYPE ='T' then 'T'  WHEN p.PAY_TYPE ='G' then 'G' else 'Y' end as PAY_TYPE         				");
            sb.AppendLine("FROM TB_S_M_EMP_RESULT_TMP t1 																							");
            sb.AppendLine("left join VW_H_DEPT_DATA vm on t1.DEPT_NO = vm.DEPT_NO																	");
            sb.AppendLine("left join TB_S_S_SALARY_PAY p on t1.SALARY_DT = p.SALARY_DT  and t1.SALARY_TYPE = p.SALARY_TYPE and t1.EMP_ID = p.EMP_ID ");
            sb.AppendLine("where t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE and t1.PAY_KIND = left(@PAY_KIND,4)					");
            sb.AppendLine("Order By t1.EMP_ID                                                                                                       ");

            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@PAY_KIND", pay_kind);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getSalaryPay(string salary_type, string salary_dt, string pay_kind, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("SELECT h1.GROUP_ID,isnull(SUM(t1.AMOUNT),0) AS AMT1,isnull(SUM(t1.AMOUNT * t1.IS_PLUS),0) AS AMT2,h1.CLASSIFY,h1.LEVEL       ");
            sb.AppendLine(" from TB_S_M_SALARY_GROUP_H h1                                                                                               ");
            sb.AppendLine(" left join TB_S_M_SALARY_GROUP_D d1 on h1.KIND_CD =d1.KIND_CD and h1.GROUP_TYPE = d1.GROUP_TYPE and h1.GROUP_ID = d1.GROUP_ID");
            sb.AppendLine(" left join TB_S_S_SALARY_PAY t1 on t1.SALARY_TYPE = h1.GROUP_TYPE and t1.SALARY_ID = d1.SUB_GROUP_ID                         ");
            sb.AppendLine("       and t1.SALARY_DT = @SALARY_DT and t1.PAY_KIND = left(@PAY_KIND,4)  and t1.EMP_ID = @EMP_ID                            ");
            sb.AppendLine("WHERE h1.KIND_CD ='B' and h1.LEVEL = 0  and h1.GROUP_TYPE=@GROUP_TYPE                                                        ");
            sb.AppendLine("GROUP By  h1.GROUP_ID,h1.CLASSIFY,h1.LEVEL                                                                                   ");

            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@GROUP_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@PAY_KIND", pay_kind);
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getSalaryGroupH(string salary_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" SELECT t1.KIND_CD,t1.GROUP_TYPE,t1.GROUP_ID,t1.GROUP_NAME,t1.LEVEL, ");
            sb.AppendLine(" t1.ORDER_SEQ,t1.CLASSIFY ");
            sb.AppendLine(" FROM TB_S_M_SALARY_GROUP_H t1 ");
            sb.AppendLine(" WHERE t1.KIND_CD ='B' and t1.GROUP_TYPE =@GROUP_TYPE and t1.LEVEL >=1 ");
            sb.AppendLine(" Order By t1.ORDER_SEQ ");
            ht.Add("@GROUP_TYPE", salary_type);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getSalaryPay2(string salary_type, string salary_dt, string pay_kind, string emp_id, string group_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" SELECT SUM(t1.AMOUNT) AS AMT1,SUM(t1.AMOUNT * t1.IS_PLUS) AS AMT2 ");
            //sb.AppendLine(" ,CASE WHEN t1.PAY_TYPE ='T' then 'T'  WHEN t1.PAY_TYPE ='G' then 'G' else 'Y' end as PAY_TYPE ");
            sb.AppendLine(" from TB_S_S_SALARY_PAY t1 ");
            sb.AppendLine(" where t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE and t1.PAY_KIND = left(@PAY_KIND,4) and t1.EMP_ID = @EMP_ID  ");
            sb.AppendLine(" and t1.SALARY_ID in ( ");
            sb.AppendLine(" SELECT SUB_GROUP_ID as SALARY_ID FROM TB_S_M_SALARY_GROUP_D where KIND_CD ='B' and GROUP_TYPE =@GROUP_TYPE  and GROUP_ID IN (  ");
            sb.AppendLine(" SELECT SUB_GROUP_ID FROM TB_S_M_SALARY_GROUP_D where KIND_CD ='B'  and  GROUP_TYPE =@GROUP_TYPE  and GROUP_ID =@GROUP_ID)) ");
            sb.AppendLine(" GROUP By t1.PAY_TYPE");
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@GROUP_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@PAY_KIND", pay_kind);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@GROUP_ID", group_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getSalaryPay3(string salary_type, string salary_dt, string pay_kind, string emp_id, string group_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" SELECT SUM(t1.AMOUNT) AS AMT1,SUM(t1.AMOUNT * t1.IS_PLUS) AS AMT2 ");
            //sb.AppendLine(" ,CASE WHEN t1.PAY_TYPE ='T' then 'T'  WHEN t1.PAY_TYPE ='G' then 'G' else 'Y' end as PAY_TYPE ");
            sb.AppendLine(" from TB_S_S_SALARY_PAY t1 ");
            sb.AppendLine(" where t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE and t1.PAY_KIND = @PAY_KIND and t1.EMP_ID = @EMP_ID  ");
            sb.AppendLine(" and t1.SALARY_ID in ( ");
            sb.AppendLine(" SELECT SUB_GROUP_ID as SALARY_ID FROM TB_S_M_SALARY_GROUP_D where KIND_CD ='B' and GROUP_TYPE =@GROUP_TYPE  and GROUP_ID IN (  ");
            sb.AppendLine(" SELECT SUB_GROUP_ID FROM TB_S_M_SALARY_GROUP_D where KIND_CD ='B'  and  GROUP_TYPE =@GROUP_TYPE  and GROUP_ID IN (  ");
            sb.AppendLine(" SELECT SUB_GROUP_ID FROM TB_S_M_SALARY_GROUP_D where KIND_CD ='B'  and  GROUP_TYPE =@GROUP_TYPE and GROUP_ID =@GROUP_ID)))  ");
            sb.AppendLine(" GROUP By t1.PAY_TYPE");
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@GROUP_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@PAY_KIND", pay_kind);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@GROUP_ID", group_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void addData(string salary_type, string salary_dt, string pay_kind, string AMT_B)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("insert into TB_S_M_SALARY_REPORT_D (PAY_TYPE,PAY_ID,SALARY_DT,DATA_YM,ACC_CD,ACC_DEPT_NO,EMP_ID,");
            sb.AppendLine("COMPANY_CD,JPN_CD,DEPT_NO,");
            sb.AppendLine("DEPT_NAME_20,DEPT_NAME_30,DEPT_NAME_40,PLANT_CD," + AMT_B + ",");
            sb.AppendLine("CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");

            sb.AppendLine(" values (@PAY_TYPE,@PAY_ID,@SALARY_DT,@SALARY_YM,@ACC_CD,@ACC_DEPT_NO,@EMP_ID,");
            sb.AppendLine("@COMPANY_CD,@JPN_CD,@DEPT_NO,");
            sb.AppendLine("@DEPT_NAME_20,@DEPT_NAME_30,@DEPT_NAME_40,@PLANT_CD,@AMT, ");
            sb.AppendLine("@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            if (PAY_ID == "&nbsp;")
                ht.Add("@PAY_ID", "");
            else
                ht.Add("@PAY_ID", PAY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@PAY_TYPE", PAY_TYPE);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@ACC_CD", ACC_CD);
            ht.Add("@ACC_DEPT_NO", ACC_DEPT_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@JPN_CD", JPN_CD);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@DEPT_NAME_20", DEPT_NAME_20);
            ht.Add("@DEPT_NAME_30", DEPT_NAME_30);
            ht.Add("@DEPT_NAME_40", DEPT_NAME_40);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@AMT", AMT);
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
    internal void addData2(string salary_type, string salary_dt, string pay_kind, string AMT_B)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("insert into TB_S_M_SALARY_REPORT_O_D (SALARY_TYPE,PAY_KIND,PAY_TYPE,PAY_ID,SALARY_DT,DATA_YM,ACC_CD,ACC_DEPT_NO,EMP_ID,");
            sb.AppendLine("COMPANY_CD,JPN_CD,DEPT_NO,");
            sb.AppendLine("DEPT_NAME_20,DEPT_NAME_30,DEPT_NAME_40,PLANT_CD," + AMT_B + ",");
            sb.AppendLine("CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");

            sb.AppendLine(" values (@SALARY_TYPE,@PAY_KIND,@PAY_TYPE,@PAY_ID,@SALARY_DT,@SALARY_YM,@ACC_CD,@ACC_DEPT_NO,@EMP_ID,");
            sb.AppendLine("@COMPANY_CD,@JPN_CD,@DEPT_NO,");
            sb.AppendLine("@DEPT_NAME_20,@DEPT_NAME_30,@DEPT_NAME_40,@PLANT_CD,@AMT,");
            sb.AppendLine("@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            if (PAY_ID == "&nbsp;")
                ht.Add("@PAY_ID", "");
            else
                ht.Add("@PAY_ID", PAY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@PAY_TYPE", PAY_TYPE);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@ACC_CD", ACC_CD);
            ht.Add("@ACC_DEPT_NO", ACC_DEPT_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@JPN_CD", JPN_CD);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@DEPT_NAME_20", DEPT_NAME_20);
            ht.Add("@DEPT_NAME_30", DEPT_NAME_30);
            ht.Add("@DEPT_NAME_40", DEPT_NAME_40);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@AMT", AMT);
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
    internal void insertSALARY_MONTH(string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" INSERT INTO TB_S_M_SALARY_MONTH(SALARY_DT,DATA_YM,ACC_CD,ACC_DEPT_NO,COMPANY_CD,PLANT_CD, ");
            sb.AppendLine(" AMT_BA001,AMT_BA002,AMT_BA003,AMT_BA004,AMT_BA005,AMT_BA006,AMT_BA007,AMT_BA008,AMT_BA009,AMT_BA010,AMT_BA011,AMT_BA012,AMT_BA013,AMT_BA014,AMT_BA015,AMT_BA016,AMT_BA017,AMT_BA018,AMT_BA019,AMT_BA020,AMT_BA021,AMT_BA022,AMT_BA023,AMT_BA024,AMT_BA025,AMT_BA026,AMT_BA027,AMT_BA028,AMT_BA029,AMT_BA030, ");
            sb.AppendLine(" AMT_BA031,AMT_BA032,AMT_BA033,AMT_BA034,AMT_BA035,AMT_BA036,AMT_BA037,AMT_BA038,AMT_BA039,AMT_BA040,AMT_BA041,AMT_BA042,AMT_BA043,AMT_BA044,AMT_BA045,AMT_BA046,AMT_BA047,AMT_BA048,AMT_BA049,AMT_BA050,AMT_BA051,AMT_BA052,AMT_BA053,AMT_BA054,AMT_BA055,AMT_BA056,AMT_BA057,AMT_BA058,AMT_BA059,AMT_BA060, ");
            sb.AppendLine(" AMT_BA061,AMT_BA062,AMT_BA063,AMT_BA064,AMT_BA065,AMT_BA066,AMT_BA067,AMT_BA068,AMT_BA069,AMT_BA070,AMT_BA071,AMT_BA072,AMT_BA073,AMT_BA074,AMT_BA075,AMT_BA076,AMT_BA077,AMT_BA078,AMT_BA079,AMT_BA080,AMT_BA081,AMT_BA082,AMT_BA083,AMT_BA084,AMT_BA085,AMT_BA086,AMT_BA087,AMT_BA088,AMT_BA089,AMT_BA090, ");
            sb.AppendLine(" AMT_BA091,AMT_BA092,AMT_BA093,AMT_BA094,AMT_BA095,AMT_BA096,AMT_BA097,AMT_BA098,AMT_BA099,AMT_BA100,AMT_BA101,AMT_BA102,AMT_BA103,AMT_BA104,AMT_BA105,AMT_BA106,AMT_BA107,AMT_BA108,AMT_BA109,AMT_BA110,AMT_BA111,AMT_BA112,AMT_BA113,AMT_BA114,AMT_BA115,AMT_BA116,AMT_BA117,AMT_BA118,AMT_BA119,AMT_BA120, ");
            sb.AppendLine(" AMT_BAA01,AMT_BAA02,AMT_BAA03,AMT_BAA04,AMT_BAA05,AMT_BAA06,AMT_BAA07,AMT_BAA08,AMT_BAA09,AMT_BAA10,AMT_BAB01,AMT_BAB02,AMT_BAC01,AMT_BAC02,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID ) ");
            sb.AppendLine(" SELECT  t1.SALARY_DT,t1.DATA_YM,t1.ACC_CD,t1.ACC_DEPT_NO,t1.COMPANY_CD,t1.PLANT_CD, ");
            sb.AppendLine(" SUM(t1.AMT_BA001) as AMT_BA001,SUM(t1.AMT_BA002) as AMT_BA002,SUM(t1.AMT_BA003) as AMT_BA003,SUM(t1.AMT_BA004) as AMT_BA004,SUM(t1.AMT_BA005) as AMT_BA005,SUM(t1.AMT_BA006) as AMT_BA006,SUM(t1.AMT_BA007) as AMT_BA007,SUM(t1.AMT_BA008) as AMT_BA008,SUM(t1.AMT_BA009) as AMT_BA009,SUM(t1.AMT_BA010) as AMT_BA010,SUM(t1.AMT_BA011) as AMT_BA011,SUM(t1.AMT_BA012) as AMT_BA012,SUM(t1.AMT_BA013) as AMT_BA013,SUM(t1.AMT_BA014) as AMT_BA014,SUM(t1.AMT_BA015) as AMT_BA015,SUM(t1.AMT_BA016) as AMT_BA016,SUM(t1.AMT_BA017) as AMT_BA017,SUM(t1.AMT_BA018) as AMT_BA018,SUM(t1.AMT_BA019) as AMT_BA019,SUM(t1.AMT_BA020) as AMT_BA020,SUM(t1.AMT_BA021) as AMT_BA021,SUM(t1.AMT_BA022) as AMT_BA022,SUM(t1.AMT_BA023) as AMT_BA023,SUM(t1.AMT_BA024) as AMT_BA024,SUM(t1.AMT_BA025) as AMT_BA025,SUM(t1.AMT_BA026) as AMT_BA026,SUM(t1.AMT_BA027) as AMT_BA027,SUM(t1.AMT_BA028) as AMT_BA028,SUM(t1.AMT_BA029) as AMT_BA029,SUM(t1.AMT_BA030) as AMT_BA030, ");
            sb.AppendLine(" SUM(t1.AMT_BA031) as AMT_BA031,SUM(t1.AMT_BA032) as AMT_BA032,SUM(t1.AMT_BA033) as AMT_BA033,SUM(t1.AMT_BA034) as AMT_BA034,SUM(t1.AMT_BA035) as AMT_BA035,SUM(t1.AMT_BA036) as AMT_BA036,SUM(t1.AMT_BA037) as AMT_BA037,SUM(t1.AMT_BA038) as AMT_BA038,SUM(t1.AMT_BA039) as AMT_BA039,SUM(t1.AMT_BA040) as AMT_BA040,SUM(t1.AMT_BA041) as AMT_BA041,SUM(t1.AMT_BA042) as AMT_BA042,SUM(t1.AMT_BA043) as AMT_BA043,SUM(t1.AMT_BA044) as AMT_BA044,SUM(t1.AMT_BA045) as AMT_BA045,SUM(t1.AMT_BA046) as AMT_BA046,SUM(t1.AMT_BA047) as AMT_BA047,SUM(t1.AMT_BA048) as AMT_BA048,SUM(t1.AMT_BA049) as AMT_BA049,SUM(t1.AMT_BA050) as AMT_BA050,SUM(t1.AMT_BA051) as AMT_BA051,SUM(t1.AMT_BA052) as AMT_BA052,SUM(t1.AMT_BA053) as AMT_BA053,SUM(t1.AMT_BA054) as AMT_BA054,SUM(t1.AMT_BA055) as AMT_BA055,SUM(t1.AMT_BA056) as AMT_BA056,SUM(t1.AMT_BA057) as AMT_BA057,SUM(t1.AMT_BA058) as AMT_BA058,SUM(t1.AMT_BA059) as AMT_BA059,SUM(t1.AMT_BA060) as AMT_BA060, ");
            sb.AppendLine(" SUM(t1.AMT_BA061) as AMT_BA061,SUM(t1.AMT_BA062) as AMT_BA062,SUM(t1.AMT_BA063) as AMT_BA063,SUM(t1.AMT_BA064) as AMT_BA064,SUM(t1.AMT_BA065) as AMT_BA065,SUM(t1.AMT_BA066) as AMT_BA066,SUM(t1.AMT_BA067) as AMT_BA067,SUM(t1.AMT_BA068) as AMT_BA068,SUM(t1.AMT_BA069) as AMT_BA069,SUM(t1.AMT_BA070) as AMT_BA070,SUM(t1.AMT_BA071) as AMT_BA071,SUM(t1.AMT_BA072) as AMT_BA072,SUM(t1.AMT_BA073) as AMT_BA073,SUM(t1.AMT_BA074) as AMT_BA074,SUM(t1.AMT_BA075) as AMT_BA075,SUM(t1.AMT_BA076) as AMT_BA076,SUM(t1.AMT_BA077) as AMT_BA077,SUM(t1.AMT_BA078) as AMT_BA078,SUM(t1.AMT_BA079) as AMT_BA079,SUM(t1.AMT_BA080) as AMT_BA080,SUM(t1.AMT_BA081) as AMT_BA081,SUM(t1.AMT_BA082) as AMT_BA082,SUM(t1.AMT_BA083) as AMT_BA083,SUM(t1.AMT_BA084) as AMT_BA084,SUM(t1.AMT_BA085) as AMT_BA085,SUM(t1.AMT_BA086) as AMT_BA086,SUM(t1.AMT_BA087) as AMT_BA087,SUM(t1.AMT_BA088) as AMT_BA088,SUM(t1.AMT_BA089) as AMT_BA089,SUM(t1.AMT_BA090) as AMT_BA090, ");
            sb.AppendLine(" SUM(t1.AMT_BA091) as AMT_BA091,SUM(t1.AMT_BA092) as AMT_BA092,SUM(t1.AMT_BA093) as AMT_BA093,SUM(t1.AMT_BA094) as AMT_BA094,SUM(t1.AMT_BA095) as AMT_BA095,SUM(t1.AMT_BA096) as AMT_BA096,SUM(t1.AMT_BA097) as AMT_BA097,SUM(t1.AMT_BA098) as AMT_BA098,SUM(t1.AMT_BA099) as AMT_BA099,SUM(t1.AMT_BA100) as AMT_BA100,SUM(t1.AMT_BA101) as AMT_BA101,SUM(t1.AMT_BA102) as AMT_BA102,SUM(t1.AMT_BA103) as AMT_BA103,SUM(t1.AMT_BA104) as AMT_BA104,SUM(t1.AMT_BA105) as AMT_BA105,SUM(t1.AMT_BA106) as AMT_BA106,SUM(t1.AMT_BA107) as AMT_BA107,SUM(t1.AMT_BA108) as AMT_BA108,SUM(t1.AMT_BA109) as AMT_BA109,SUM(t1.AMT_BA110) as AMT_BA110,SUM(t1.AMT_BA111) as AMT_BA111,SUM(t1.AMT_BA112) as AMT_BA112,SUM(t1.AMT_BA113) as AMT_BA113,SUM(t1.AMT_BA114) as AMT_BA114,SUM(t1.AMT_BA115) as AMT_BA115,SUM(t1.AMT_BA116) as AMT_BA116,SUM(t1.AMT_BA117) as AMT_BA117,SUM(t1.AMT_BA118) as AMT_BA118,SUM(t1.AMT_BA119) as AMT_BA119,SUM(t1.AMT_BA120) as AMT_BA120, ");

            sb.AppendLine(" SUM(t1.AMT_BAA01) as AMT_BAA01,SUM(t1.AMT_BAA02) as AMT_BAA02,SUM(t1.AMT_BAA03) as AMT_BAA03,SUM(t1.AMT_BAA04) as AMT_BAA04,SUM(t1.AMT_BAA05) as AMT_BAA05,SUM(t1.AMT_BAA06) as AMT_BAA06,SUM(t1.AMT_BAA07) as AMT_BAA07,SUM(t1.AMT_BAA08) as AMT_BAA08,SUM(t1.AMT_BAA09) as AMT_BAA09,SUM(t1.AMT_BAA10) as AMT_BAA10,SUM(t1.AMT_BAB01) as AMT_BAB01,SUM(t1.AMT_BAB02) as AMT_BAB02,SUM(t1.AMT_BAC01) as AMT_BAC01,SUM(t1.AMT_BAC02) as AMT_BAC02, ");
            sb.AppendLine(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID");
            sb.AppendLine(" FROM   TB_S_M_SALARY_REPORT_D t1 ");
            sb.AppendLine(" WHERE  t1.SALARY_DT = @SALARY_DT ");
            sb.AppendLine(" GROUP BY t1.SALARY_DT,t1.DATA_YM,t1.ACC_CD,t1.ACC_DEPT_NO,t1.COMPANY_CD,t1.PLANT_CD  ");

            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            ht.Add("@SALARY_DT", salary_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void insertSALARY_OTHER(string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" INSERT INTO TB_S_M_SALARY_OTHER(PAY_ID,SALARY_TYPE,PAY_KIND,PLANT_CD,SALARY_DT,DATA_YM,ACC_CD,ACC_DEPT_NO,COMPANY_CD,PERSON_CNT, ");
            sb.AppendLine(" AMT_BC001,AMT_BC002,AMT_BC003,AMT_BC004,AMT_BC005,AMT_BC006,AMT_BC007,AMT_BC008,AMT_BC009,AMT_BC010,AMT_BC011,AMT_BC012,AMT_BC013,AMT_BC014,AMT_BC015,AMT_BC016,AMT_BC017,AMT_BC018,AMT_BC019,AMT_BC020,AMT_BCA01,AMT_BCB01,AMT_BCC01,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID ) ");
            sb.AppendLine(" SELECT  isnull(t1.PAY_ID,'') PAY_ID,t1.SALARY_TYPE,t1.PAY_KIND,t1.PLANT_CD,t1.SALARY_DT,t1.DATA_YM,t1.ACC_CD,t1.ACC_DEPT_NO,t1.COMPANY_CD,COUNT(t1.EMP_ID)as PERSON_CNT, ");
            sb.AppendLine(" SUM(t1.AMT_BC001) as AMT_BC001,SUM(t1.AMT_BC002) as AMT_BC002,SUM(t1.AMT_BC003) as AMT_BC003,SUM(t1.AMT_BC004) as AMT_BC004,SUM(t1.AMT_BC005) as AMT_BC005,SUM(t1.AMT_BC006) as AMT_BC006,SUM(t1.AMT_BC007) as AMT_BC007,SUM(t1.AMT_BC008) as AMT_BC008,SUM(t1.AMT_BC009) as AMT_BC009,SUM(t1.AMT_BC010) as AMT_BC010,SUM(t1.AMT_BC011) as AMT_BC011,SUM(t1.AMT_BC012) as AMT_BC012,SUM(t1.AMT_BC013) as AMT_BC013,SUM(t1.AMT_BC014) as AMT_BC014,SUM(t1.AMT_BC015) as AMT_BC015, ");
            sb.AppendLine(" SUM(t1.AMT_BC016) as AMT_BC016,SUM(t1.AMT_BC017) as AMT_BC017,SUM(t1.AMT_BC018) as AMT_BC018,SUM(t1.AMT_BC019) as AMT_BC019,SUM(t1.AMT_BC020) as AMT_BC020,SUM(t1.AMT_BCA01) as AMT_BCA10,SUM(t1.AMT_BCB01) as AMT_BCB01,SUM(t1.AMT_BCC01) as AMT_BCC01,  ");
            sb.AppendLine(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID");

            sb.AppendLine(" FROM   TB_S_M_SALARY_REPORT_O_D t1 ");
            sb.AppendLine(" WHERE  t1.SALARY_DT = @SALARY_DT ");
            sb.AppendLine(" GROUP BY t1.SALARY_DT,t1.DATA_YM,t1.ACC_CD,t1.ACC_DEPT_NO,t1.COMPANY_CD,t1.SALARY_TYPE,t1.PAY_KIND,t1.PLANT_CD,t1.PAY_ID  ");

            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            ht.Add("@SALARY_DT", salary_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getSalaryGroupH_xls_header(string salary_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" SELECT t1.KIND_CD,t1.GROUP_TYPE,t1.GROUP_ID,t1.GROUP_NAME,");
            sb.AppendLine(" t1.ORDER_SEQ,t1.LEVEL,t1.CLASSIFY ");
            sb.AppendLine(" FROM TB_S_M_SALARY_GROUP_H t1 ");
            sb.AppendLine(" WHERE t1.KIND_CD ='B' and t1.GROUP_TYPE =@GROUP_TYPE ");
            sb.AppendLine(" Order By t1.ORDER_SEQ ");

            ht.Add("@GROUP_TYPE", salary_type);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getSalaryMonth_xls1_header(string company_cd, string AMT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" SELECT isnull(SUM(t1." + AMT + "),0) as  AMT  ");
            sb.AppendLine(" FROM TB_S_M_SALARY_MONTH t1 ");
            sb.AppendLine(" WHERE 1=1 ");
            sb.AppendLine(" and t1.SALARY_DT =@SALARY_DT ");
            sb.AppendLine(" and t1.COMPANY_CD = @COMPANY_CD ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@COMPANY_CD", company_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getSalaryMonth_xls1_body(string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" SELECT  t1.SALARY_DT,t1.DATA_YM,t1.ACC_CD,t1.ACC_DEPT_NO,t1.ACC_DEPT_NAME,d.SUB_DESC as DESC1,t1.COMPANY_CD,SUM(t1.PERSON_CNT) as CNT, ");
            sb.AppendLine(" SUM(t1.AMT_BA001) as AMT_BA001,SUM(t1.AMT_BA002) as AMT_BA002,SUM(t1.AMT_BA003) as AMT_BA003,SUM(t1.AMT_BA004) as AMT_BA004,SUM(t1.AMT_BA005) as AMT_BA005,SUM(t1.AMT_BA006) as AMT_BA006,SUM(t1.AMT_BA007) as AMT_BA007,SUM(t1.AMT_BA008) as AMT_BA008,SUM(t1.AMT_BA009) as AMT_BA009,SUM(t1.AMT_BA010) as AMT_BA010,SUM(t1.AMT_BA011) as AMT_BA011,SUM(t1.AMT_BA012) as AMT_BA012,SUM(t1.AMT_BA013) as AMT_BA013,SUM(t1.AMT_BA014) as AMT_BA014,SUM(t1.AMT_BA015) as AMT_BA015,SUM(t1.AMT_BA016) as AMT_BA016,SUM(t1.AMT_BA017) as AMT_BA017,SUM(t1.AMT_BA018) as AMT_BA018,SUM(t1.AMT_BA019) as AMT_BA019,SUM(t1.AMT_BA020) as AMT_BA020,SUM(t1.AMT_BA021) as AMT_BA021,SUM(t1.AMT_BA022) as AMT_BA022,SUM(t1.AMT_BA023) as AMT_BA023,SUM(t1.AMT_BA024) as AMT_BA024,SUM(t1.AMT_BA025) as AMT_BA025,SUM(t1.AMT_BA026) as AMT_BA026,SUM(t1.AMT_BA027) as AMT_BA027,SUM(t1.AMT_BA028) as AMT_BA028,SUM(t1.AMT_BA029) as AMT_BA029,SUM(t1.AMT_BA030) as AMT_BA030, ");
            sb.AppendLine(" SUM(t1.AMT_BA031) as AMT_BA031,SUM(t1.AMT_BA032) as AMT_BA032,SUM(t1.AMT_BA033) as AMT_BA033,SUM(t1.AMT_BA034) as AMT_BA034,SUM(t1.AMT_BA035) as AMT_BA035,SUM(t1.AMT_BA036) as AMT_BA036,SUM(t1.AMT_BA037) as AMT_BA037,SUM(t1.AMT_BA038) as AMT_BA038,SUM(t1.AMT_BA039) as AMT_BA039,SUM(t1.AMT_BA040) as AMT_BA040,SUM(t1.AMT_BA041) as AMT_BA041,SUM(t1.AMT_BA042) as AMT_BA042,SUM(t1.AMT_BA043) as AMT_BA043,SUM(t1.AMT_BA044) as AMT_BA044,SUM(t1.AMT_BA045) as AMT_BA045,SUM(t1.AMT_BA046) as AMT_BA046,SUM(t1.AMT_BA047) as AMT_BA047,SUM(t1.AMT_BA048) as AMT_BA048,SUM(t1.AMT_BA049) as AMT_BA049,SUM(t1.AMT_BA050) as AMT_BA050,SUM(t1.AMT_BA051) as AMT_BA051,SUM(t1.AMT_BA052) as AMT_BA052,SUM(t1.AMT_BA053) as AMT_BA053,SUM(t1.AMT_BA054) as AMT_BA054,SUM(t1.AMT_BA055) as AMT_BA055,SUM(t1.AMT_BA056) as AMT_BA056,SUM(t1.AMT_BA057) as AMT_BA057,SUM(t1.AMT_BA058) as AMT_BA058,SUM(t1.AMT_BA059) as AMT_BA059,SUM(t1.AMT_BA060) as AMT_BA060, ");
            sb.AppendLine(" SUM(t1.AMT_BA061) as AMT_BA061,SUM(t1.AMT_BA062) as AMT_BA062,SUM(t1.AMT_BA063) as AMT_BA063,SUM(t1.AMT_BA064) as AMT_BA064,SUM(t1.AMT_BA065) as AMT_BA065,SUM(t1.AMT_BA066) as AMT_BA066,SUM(t1.AMT_BA067) as AMT_BA067,SUM(t1.AMT_BA068) as AMT_BA068,SUM(t1.AMT_BA069) as AMT_BA069,SUM(t1.AMT_BA070) as AMT_BA070,SUM(t1.AMT_BA071) as AMT_BA071,SUM(t1.AMT_BA072) as AMT_BA072,SUM(t1.AMT_BA073) as AMT_BA073,SUM(t1.AMT_BA074) as AMT_BA074,SUM(t1.AMT_BA075) as AMT_BA075,SUM(t1.AMT_BA076) as AMT_BA076,SUM(t1.AMT_BA077) as AMT_BA077,SUM(t1.AMT_BA078) as AMT_BA078,SUM(t1.AMT_BA079) as AMT_BA079,SUM(t1.AMT_BA080) as AMT_BA080,SUM(t1.AMT_BA081) as AMT_BA081,SUM(t1.AMT_BA082) as AMT_BA082,SUM(t1.AMT_BA083) as AMT_BA083,SUM(t1.AMT_BA084) as AMT_BA084,SUM(t1.AMT_BA085) as AMT_BA085,SUM(t1.AMT_BA086) as AMT_BA086,SUM(t1.AMT_BA087) as AMT_BA087,SUM(t1.AMT_BA088) as AMT_BA088,SUM(t1.AMT_BA089) as AMT_BA089,SUM(t1.AMT_BA090) as AMT_BA090, ");
            sb.AppendLine(" SUM(t1.AMT_BA091) as AMT_BA091,SUM(t1.AMT_BA092) as AMT_BA092,SUM(t1.AMT_BA093) as AMT_BA093,SUM(t1.AMT_BA094) as AMT_BA094,SUM(t1.AMT_BA095) as AMT_BA095,SUM(t1.AMT_BA096) as AMT_BA096,SUM(t1.AMT_BA097) as AMT_BA097,SUM(t1.AMT_BA098) as AMT_BA098,SUM(t1.AMT_BA099) as AMT_BA099,SUM(t1.AMT_BA100) as AMT_BA100,SUM(t1.AMT_BA101) as AMT_BA101,SUM(t1.AMT_BA102) as AMT_BA102,SUM(t1.AMT_BA103) as AMT_BA103,SUM(t1.AMT_BA104) as AMT_BA104,SUM(t1.AMT_BA105) as AMT_BA105,SUM(t1.AMT_BA106) as AMT_BA106,SUM(t1.AMT_BA107) as AMT_BA107,SUM(t1.AMT_BA108) as AMT_BA108,SUM(t1.AMT_BA109) as AMT_BA109,SUM(t1.AMT_BA110) as AMT_BA110,SUM(t1.AMT_BA111) as AMT_BA111,SUM(t1.AMT_BA112) as AMT_BA112,SUM(t1.AMT_BA113) as AMT_BA113,SUM(t1.AMT_BA114) as AMT_BA114,SUM(t1.AMT_BA115) as AMT_BA115,SUM(t1.AMT_BA116) as AMT_BA116,SUM(t1.AMT_BA117) as AMT_BA117,SUM(t1.AMT_BA118) as AMT_BA118,SUM(t1.AMT_BA119) as AMT_BA119,SUM(t1.AMT_BA120) as AMT_BA120, ");

            sb.AppendLine(" SUM(t1.AMT_BAA01) as AMT_BAA01,SUM(t1.AMT_BAA02) as AMT_BAA02,SUM(t1.AMT_BAA03) as AMT_BAA03,SUM(t1.AMT_BAA04) as AMT_BAA04,SUM(t1.AMT_BAA05) as AMT_BAA05,SUM(t1.AMT_BAA06) as AMT_BAA06,SUM(t1.AMT_BAA07) as AMT_BAA07,SUM(t1.AMT_BAA08) as AMT_BAA08,SUM(t1.AMT_BAA09) as AMT_BAA09,SUM(t1.AMT_BAA10) as AMT_BAA10,SUM(t1.AMT_BAB01) as AMT_BAB01,SUM(t1.AMT_BAB02) as AMT_BAB02,SUM(t1.AMT_BAC01) as AMT_BAC01,SUM(t1.AMT_BAC02) as AMT_BAC02 ");
            sb.AppendLine(" FROM (select * from TB_S_M_SALARY_MONTH where SALARY_DT =@SALARY_DT and COMPANY_CD=@COMPANY_CD   )t1 ");
            sb.AppendLine(" left join TB_9_M_COMM_D d on  d.SYS_CD ='HA' and  d.MAIN_CD='ACC_CD' and  t1.ACC_CD = d.SUB_CD  ");
            sb.AppendLine(" WHERE 1=1 ");
            sb.AppendLine(" and t1.SALARY_DT =@SALARY_DT  ");
            sb.AppendLine(" and t1.COMPANY_CD=@COMPANY_CD ");

            sb.AppendLine(" GROUP BY t1.SALARY_DT,t1.DATA_YM,t1.ACC_CD,t1.ACC_DEPT_NO,t1.ACC_DEPT_NAME,t1.COMPANY_CD,d.SUB_DESC ");
            sb.AppendLine(" order By t1.COMPANY_CD,t1.ACC_CD,t1.ACC_DEPT_NO");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@COMPANY_CD", company_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getSalaryMonth_xls2_header(string company_cd, string AMT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" SELECT isnull(SUM(t1." + AMT + "),0) as  AMT  ");
            sb.AppendLine(" FROM TB_S_M_SALARY_MONTH t1 ");
            sb.AppendLine(" WHERE 1=1 ");
            sb.AppendLine(" and t1.DATA_YM =@DATA_YM ");
            sb.AppendLine(" and t1.COMPANY_CD = @COMPANY_CD ");
            ht.Add("@DATA_YM", SALARY_YM);
            ht.Add("@COMPANY_CD", company_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getSalaryMonth_xls2_body(string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" SELECT  t1.SALARY_DT,t1.DATA_YM,t1.ACC_CD,t1.ACC_DEPT_NO,t1.ACC_DEPT_NAME,d.SUB_DESC as DESC1,t1.COMPANY_CD,SUM(t1.PERSON_CNT) as CNT, ");
            sb.AppendLine(" SUM(t1.AMT_BA001) as AMT_BA001,SUM(t1.AMT_BA002) as AMT_BA002,SUM(t1.AMT_BA003) as AMT_BA003,SUM(t1.AMT_BA004) as AMT_BA004,SUM(t1.AMT_BA005) as AMT_BA005,SUM(t1.AMT_BA006) as AMT_BA006,SUM(t1.AMT_BA007) as AMT_BA007,SUM(t1.AMT_BA008) as AMT_BA008,SUM(t1.AMT_BA009) as AMT_BA009,SUM(t1.AMT_BA010) as AMT_BA010,SUM(t1.AMT_BA011) as AMT_BA011,SUM(t1.AMT_BA012) as AMT_BA012,SUM(t1.AMT_BA013) as AMT_BA013,SUM(t1.AMT_BA014) as AMT_BA014,SUM(t1.AMT_BA015) as AMT_BA015,SUM(t1.AMT_BA016) as AMT_BA016,SUM(t1.AMT_BA017) as AMT_BA017,SUM(t1.AMT_BA018) as AMT_BA018,SUM(t1.AMT_BA019) as AMT_BA019,SUM(t1.AMT_BA020) as AMT_BA020,SUM(t1.AMT_BA021) as AMT_BA021,SUM(t1.AMT_BA022) as AMT_BA022,SUM(t1.AMT_BA023) as AMT_BA023,SUM(t1.AMT_BA024) as AMT_BA024,SUM(t1.AMT_BA025) as AMT_BA025,SUM(t1.AMT_BA026) as AMT_BA026,SUM(t1.AMT_BA027) as AMT_BA027,SUM(t1.AMT_BA028) as AMT_BA028,SUM(t1.AMT_BA029) as AMT_BA029,SUM(t1.AMT_BA030) as AMT_BA030, ");
            sb.AppendLine(" SUM(t1.AMT_BA031) as AMT_BA031,SUM(t1.AMT_BA032) as AMT_BA032,SUM(t1.AMT_BA033) as AMT_BA033,SUM(t1.AMT_BA034) as AMT_BA034,SUM(t1.AMT_BA035) as AMT_BA035,SUM(t1.AMT_BA036) as AMT_BA036,SUM(t1.AMT_BA037) as AMT_BA037,SUM(t1.AMT_BA038) as AMT_BA038,SUM(t1.AMT_BA039) as AMT_BA039,SUM(t1.AMT_BA040) as AMT_BA040,SUM(t1.AMT_BA041) as AMT_BA041,SUM(t1.AMT_BA042) as AMT_BA042,SUM(t1.AMT_BA043) as AMT_BA043,SUM(t1.AMT_BA044) as AMT_BA044,SUM(t1.AMT_BA045) as AMT_BA045,SUM(t1.AMT_BA046) as AMT_BA046,SUM(t1.AMT_BA047) as AMT_BA047,SUM(t1.AMT_BA048) as AMT_BA048,SUM(t1.AMT_BA049) as AMT_BA049,SUM(t1.AMT_BA050) as AMT_BA050,SUM(t1.AMT_BA051) as AMT_BA051,SUM(t1.AMT_BA052) as AMT_BA052,SUM(t1.AMT_BA053) as AMT_BA053,SUM(t1.AMT_BA054) as AMT_BA054,SUM(t1.AMT_BA055) as AMT_BA055,SUM(t1.AMT_BA056) as AMT_BA056,SUM(t1.AMT_BA057) as AMT_BA057,SUM(t1.AMT_BA058) as AMT_BA058,SUM(t1.AMT_BA059) as AMT_BA059,SUM(t1.AMT_BA060) as AMT_BA060, ");
            sb.AppendLine(" SUM(t1.AMT_BA061) as AMT_BA061,SUM(t1.AMT_BA062) as AMT_BA062,SUM(t1.AMT_BA063) as AMT_BA063,SUM(t1.AMT_BA064) as AMT_BA064,SUM(t1.AMT_BA065) as AMT_BA065,SUM(t1.AMT_BA066) as AMT_BA066,SUM(t1.AMT_BA067) as AMT_BA067,SUM(t1.AMT_BA068) as AMT_BA068,SUM(t1.AMT_BA069) as AMT_BA069,SUM(t1.AMT_BA070) as AMT_BA070,SUM(t1.AMT_BA071) as AMT_BA071,SUM(t1.AMT_BA072) as AMT_BA072,SUM(t1.AMT_BA073) as AMT_BA073,SUM(t1.AMT_BA074) as AMT_BA074,SUM(t1.AMT_BA075) as AMT_BA075,SUM(t1.AMT_BA076) as AMT_BA076,SUM(t1.AMT_BA077) as AMT_BA077,SUM(t1.AMT_BA078) as AMT_BA078,SUM(t1.AMT_BA079) as AMT_BA079,SUM(t1.AMT_BA080) as AMT_BA080,SUM(t1.AMT_BA081) as AMT_BA081,SUM(t1.AMT_BA082) as AMT_BA082,SUM(t1.AMT_BA083) as AMT_BA083,SUM(t1.AMT_BA084) as AMT_BA084,SUM(t1.AMT_BA085) as AMT_BA085,SUM(t1.AMT_BA086) as AMT_BA086,SUM(t1.AMT_BA087) as AMT_BA087,SUM(t1.AMT_BA088) as AMT_BA088,SUM(t1.AMT_BA089) as AMT_BA089,SUM(t1.AMT_BA090) as AMT_BA090, ");
            sb.AppendLine(" SUM(t1.AMT_BA091) as AMT_BA091,SUM(t1.AMT_BA092) as AMT_BA092,SUM(t1.AMT_BA093) as AMT_BA093,SUM(t1.AMT_BA094) as AMT_BA094,SUM(t1.AMT_BA095) as AMT_BA095,SUM(t1.AMT_BA096) as AMT_BA096,SUM(t1.AMT_BA097) as AMT_BA097,SUM(t1.AMT_BA098) as AMT_BA098,SUM(t1.AMT_BA099) as AMT_BA099,SUM(t1.AMT_BA100) as AMT_BA100,SUM(t1.AMT_BA101) as AMT_BA101,SUM(t1.AMT_BA102) as AMT_BA102,SUM(t1.AMT_BA103) as AMT_BA103,SUM(t1.AMT_BA104) as AMT_BA104,SUM(t1.AMT_BA105) as AMT_BA105,SUM(t1.AMT_BA106) as AMT_BA106,SUM(t1.AMT_BA107) as AMT_BA107,SUM(t1.AMT_BA108) as AMT_BA108,SUM(t1.AMT_BA109) as AMT_BA109,SUM(t1.AMT_BA110) as AMT_BA110,SUM(t1.AMT_BA111) as AMT_BA111,SUM(t1.AMT_BA112) as AMT_BA112,SUM(t1.AMT_BA113) as AMT_BA113,SUM(t1.AMT_BA114) as AMT_BA114,SUM(t1.AMT_BA115) as AMT_BA115,SUM(t1.AMT_BA116) as AMT_BA116,SUM(t1.AMT_BA117) as AMT_BA117,SUM(t1.AMT_BA118) as AMT_BA118,SUM(t1.AMT_BA119) as AMT_BA119,SUM(t1.AMT_BA120) as AMT_BA120, ");

            sb.AppendLine(" SUM(t1.AMT_BAA01) as AMT_BAA01,SUM(t1.AMT_BAA02) as AMT_BAA02,SUM(t1.AMT_BAA03) as AMT_BAA03,SUM(t1.AMT_BAA04) as AMT_BAA04,SUM(t1.AMT_BAA05) as AMT_BAA05,SUM(t1.AMT_BAA06) as AMT_BAA06,SUM(t1.AMT_BAA07) as AMT_BAA07,SUM(t1.AMT_BAA08) as AMT_BAA08,SUM(t1.AMT_BAA09) as AMT_BAA09,SUM(t1.AMT_BAA10) as AMT_BAA10,SUM(t1.AMT_BAB01) as AMT_BAB01,SUM(t1.AMT_BAB02) as AMT_BAB02,SUM(t1.AMT_BAC01) as AMT_BAC01,SUM(t1.AMT_BAC02) as AMT_BAC02 ");
            sb.AppendLine(" FROM (select * from TB_S_M_SALARY_MONTH  where DATA_YM =@SALARY_YM  and COMPANY_CD=@COMPANY_CD  )t1 ");
            sb.AppendLine(" left join TB_9_M_COMM_D d on  d.SYS_CD ='HA' and  d.MAIN_CD='ACC_CD' and  t1.ACC_CD = d.SUB_CD  ");
            sb.AppendLine(" WHERE 1=1 ");
            sb.AppendLine(" and t1.DATA_YM =@SALARY_YM  ");
            sb.AppendLine(" and t1.COMPANY_CD=@COMPANY_CD ");

            sb.AppendLine(" GROUP BY t1.SALARY_DT,t1.DATA_YM,t1.ACC_CD,t1.ACC_DEPT_NO,t1.ACC_DEPT_NAME,t1.COMPANY_CD,d.SUB_DESC ");
            sb.AppendLine(" order By t1.COMPANY_CD,t1.ACC_CD,t1.ACC_DEPT_NO");

            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@COMPANY_CD", company_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getSalaryOther_xls1_header(string salary_type, string company_cd, string AMT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" SELECT isnull(SUM(t1." + AMT + "),0) as AMT ");
            sb.AppendLine(" FROM TB_S_M_SALARY_OTHER t1 ");
            sb.AppendLine(" WHERE 1=1 and t1.SALARY_TYPE=@SALARY_TYPE ");
            sb.AppendLine(" and t1.SALARY_DT =@SALARY_DT and t1.PAY_KIND = @PAY_KIND and t1.COMPANY_CD = @COMPANY_CD ");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@COMPANY_CD", company_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getSalaryOther_xls1_body(string salary_type, string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" SELECT  t1.SALARY_DT,t1.DATA_YM,t1.ACC_CD,t1.ACC_DEPT_NO,a.ACC_DEPT_NAME,d.SUB_DESC as DESC1,t1.COMPANY_CD,SUM(t1.PERSON_CNT) as CNT, ");
            sb.AppendLine(" SUM(t1.AMT_BC001) as AMT_BC001,SUM(t1.AMT_BC002) as AMT_BC002,SUM(t1.AMT_BC003) as AMT_BC003,SUM(t1.AMT_BC004) as AMT_BC004,SUM(t1.AMT_BC005) as AMT_BC005,SUM(t1.AMT_BC006) as AMT_BC006,SUM(t1.AMT_BC007) as AMT_BC007,SUM(t1.AMT_BC008) as AMT_BC008,SUM(t1.AMT_BC009) as AMT_BC009,SUM(t1.AMT_BC010) as AMT_BC010,SUM(t1.AMT_BC011) as AMT_BC011,SUM(t1.AMT_BC012) as AMT_BC012,SUM(t1.AMT_BC013) as AMT_BC013,SUM(t1.AMT_BC014) as AMT_BC014,SUM(t1.AMT_BC015) as AMT_BC015, ");
            sb.AppendLine(" SUM(t1.AMT_BC016) as AMT_BC016,SUM(t1.AMT_BC017) as AMT_BC017,SUM(t1.AMT_BC018) as AMT_BC018,SUM(t1.AMT_BC019) as AMT_BC019,SUM(t1.AMT_BC020) as AMT_BC020,SUM(t1.AMT_BCA01) as AMT_BCA10,SUM(t1.AMT_BCB01) as AMT_BCB01,SUM(t1.AMT_BCC01) as AMT_BCC01  ");
            sb.AppendLine(" FROM TB_S_M_SALARY_OTHER t1 ");
            sb.AppendLine(" left join TB_9_M_COMM_D d on  d.SYS_CD ='HA' and  d.MAIN_CD='ACC_CD' and  t1.ACC_CD = d.SUB_CD  ");
            sb.AppendLine(" left join TB_H_M_DEPT_ACC a on t1.ACC_DEPT_NO = a.ACC_DEPT_NO ");
            sb.AppendLine(" WHERE 1=1 and t1.SALARY_TYPE =@SALARY_TYPE ");
            sb.AppendLine(" and t1.SALARY_DT =@SALARY_DT and t1.PAY_KIND = @PAY_KIND  ");
            sb.AppendLine(" and t1.COMPANY_CD=@COMPANY_CD ");
                
            sb.AppendLine(" GROUP BY t1.SALARY_DT,t1.DATA_YM,t1.ACC_CD,t1.ACC_DEPT_NO,a.ACC_DEPT_NAME,t1.COMPANY_CD,d.SUB_DESC ");
            sb.AppendLine(" order By t1.COMPANY_CD,t1.ACC_CD,t1.ACC_DEPT_NO");
            ht.Add("@COMPANY_CD", company_cd);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@PAY_KIND", PAY_KIND);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
 

    internal DataTable eachSalary_Month(string company_cd,string AMT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" SELECT  isnull(SUM(t1.AMT_" + AMT + "),0) as AMT ");
            sb.AppendLine(" FROM TB_S_M_SALARY_MONTH t1  left join TB_9_M_COMM_D d on  d.SYS_CD ='HA' and  d.MAIN_CD='ACC_CD' and  t1.ACC_CD = d.SUB_CD   ");
            sb.AppendLine(" WHERE 1=1  and t1.SALARY_DT =@SALARY_DT and t1.COMPANY_CD = @COMPANY_CD  ");
            sb.AppendLine(" GROUP BY t1.SALARY_DT,t1.DATA_YM,t1.ACC_CD,t1.ACC_DEPT_NO,t1.ACC_DEPT_NAME,t1.COMPANY_CD,d.SUB_DESC  ");
            sb.AppendLine(" order By t1.COMPANY_CD,t1.ACC_CD,t1.ACC_DEPT_NO ");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@COMPANY_CD", company_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable eachSalary_Month_2(string company_cd,string AMT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" SELECT  isnull(SUM(t1.AMT_" + AMT + "),0) as AMT ");
            sb.AppendLine(" FROM TB_S_M_SALARY_MONTH t1  left join TB_9_M_COMM_D d on  d.SYS_CD ='HA' and  d.MAIN_CD='ACC_CD' and  t1.ACC_CD = d.SUB_CD   ");
            sb.AppendLine(" WHERE 1=1  and t1.DATA_YM =@DATA_YM  and t1.COMPANY_CD = @COMPANY_CD    ");
            sb.AppendLine(" GROUP BY t1.SALARY_DT,t1.DATA_YM,t1.ACC_CD,t1.ACC_DEPT_NO,t1.ACC_DEPT_NAME,t1.COMPANY_CD,d.SUB_DESC  ");
            sb.AppendLine(" order By t1.COMPANY_CD,t1.ACC_CD,t1.ACC_DEPT_NO ");

            ht.Add("@DATA_YM", SALARY_YM);
            ht.Add("@COMPANY_CD", company_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable eachSalary_Other(string salary_type, string company_cd, string AMT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" SELECT  isnull(SUM(t1.AMT_BC" + AMT + "),0) as AMT ");
            sb.AppendLine(" FROM TB_S_M_SALARY_OTHER t1  left join TB_9_M_COMM_D d on  d.SYS_CD ='HA' and  d.MAIN_CD='ACC_CD' and  t1.ACC_CD = d.SUB_CD   ");
            sb.AppendLine(" WHERE 1=1 and t1.SALARY_TYPE =@SALARY_TYPE  and t1.SALARY_DT =@SALARY_DT and t1.PAY_KIND = @PAY_KIND  and t1.COMPANY_CD = @COMPANY_CD    ");
            sb.AppendLine(" GROUP BY t1.SALARY_DT,t1.DATA_YM,t1.ACC_CD,t1.ACC_DEPT_NO,t1.COMPANY_CD,d.SUB_DESC  ");
            sb.AppendLine(" order By t1.COMPANY_CD,t1.ACC_CD,t1.ACC_DEPT_NO ");
            ht.Add("@COMPANY_CD", company_cd);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@PAY_KIND", PAY_KIND);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }

    }
    //internal DataTable eachSalary_Other_2(string salary_type, string AMT)
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.AppendLine(" SELECT  SUM(t1.AMT_BC" + AMT + ") as AMT ");
    //        sb.AppendLine(" FROM TB_S_M_SALARY_OTHER t1  left join TB_9_M_COMM_D d on  d.SYS_CD ='HA' and  d.MAIN_CD='ACC_CD' and  t1.ACC_CD = d.SUB_CD   ");
    //        sb.AppendLine(" WHERE 1=1 and t1.SALARY_TYPE =@SALARY_TYPE  and t1.DATA_YM =@DATA_YM   ");
    //        sb.AppendLine(" GROUP BY t1.SALARY_DT,t1.DATA_YM,t1.ACC_CD,t1.ACC_DEPT_NO,t1.COMPANY_CD,d.SUB_DESC  ");
    //        sb.AppendLine(" order By t1.COMPANY_CD,t1.ACC_CD,t1.ACC_DEPT_NO ");

    //        ht.Add("@DATA_YM", SALARY_YM);
    //        ht.Add("@SALARY_TYPE", salary_type);

    //        return dbConn.Query(sb, ht);
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }

    //}

    internal DataTable getExistSrd(string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("	select COUNT(0) srd_count from TB_S_M_SALARY_REPORT_D ");
            sb.AppendLine("	where  SALARY_DT=@SALARY_DT and EMP_ID=@EMP_ID and PAY_TYPE = @PAY_TYPE ");
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@PAY_TYPE", PAY_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }

    }

    internal DataTable getExistSm(string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("	select COUNT(0) sm_count from TB_S_M_SALARY_MONTH ");
            sb.AppendLine("	where SALARY_DT=@SALARY_DT and ACC_CD = @ACC_CD and ACC_DEPT_NO=@ACC_DEPT_NO and COMPANY_CD=@COMPANY_CD  ");    //and PLANT_CD=@PLANT_CD
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@ACC_CD", ACC_CD);
            ht.Add("@ACC_DEPT_NO", ACC_DEPT_NO);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            //ht.Add("@PLANT_CD", PLANT_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable getExistSrod(string salary_type, string salary_dt, string pay_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("	select COUNT(0) srod_count from TB_S_M_SALARY_REPORT_O_D ");
            sb.AppendLine("	where  SALARY_DT=@SALARY_DT and PAY_KIND=@PAY_KIND and SALARY_TYPE=@SALARY_TYPE and EMP_ID=@EMP_ID and PAY_TYPE = @PAY_TYPE ");
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@PAY_KIND", pay_kind);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@PAY_TYPE", PAY_TYPE);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable getExistSo(string salary_type, string salary_dt, string pay_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("	select COUNT(0) so_count from TB_S_M_SALARY_OTHER ");
            sb.AppendLine("	where SALARY_TYPE=@SALARY_TYPE and SALARY_DT=@SALARY_DT and PAY_KIND=@PAY_KIND and ACC_CD = @ACC_CD and ACC_DEPT_NO=@ACC_DEPT_NO and COMPANY_CD=@COMPANY_CD  ");    //and PLANT_CD=@PLANT_CD
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@PAY_KIND", pay_kind);
            ht.Add("@ACC_CD", ACC_CD);
            ht.Add("@ACC_DEPT_NO", ACC_DEPT_NO);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            //ht.Add("@PLANT_CD", PLANT_CD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }


    //print1(SP)
    public void RunSP_S_WFB2SC310()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_WFB2SC310");
            if (string.IsNullOrEmpty(SALARY_TYPE))
                ht.Add("@pSalaryType", DBNull.Value);
            else
                ht.Add("@pSalaryType", SALARY_TYPE);

            if (string.IsNullOrEmpty(PAY_KIND))
                ht.Add("@pPaykind", DBNull.Value);
            else
                ht.Add("@pPaykind", PAY_KIND);

            if (string.IsNullOrEmpty(SALARY_DT))
                ht.Add("@pSalaryDate", DBNull.Value);
            else
                ht.Add("@pSalaryDate", SALARY_DT);

            if (string.IsNullOrEmpty(SALARY_YM))
                ht.Add("@pSalaryYM", DBNull.Value);
            else
                ht.Add("@pSalaryYM", SALARY_YM);

            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2SC310");

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //print2(SP)
    public void RunSP_S_SALARY_REPORT_WFB2SC310()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_SALARY_REPORT_WFB2SC310");
            if (string.IsNullOrEmpty(SALARY_TYPE))
                ht.Add("@pSalaryType", DBNull.Value);
            else
                ht.Add("@pSalaryType", SALARY_TYPE);

            if (string.IsNullOrEmpty(PAY_KIND))
                ht.Add("@pPayKind", DBNull.Value);
            else
                ht.Add("@pPayKind", PAY_KIND);

            if (string.IsNullOrEmpty(SALARY_DT))
                ht.Add("@pSalaryDate", DBNull.Value);
            else
                ht.Add("@pSalaryDate", SALARY_DT);

            if (string.IsNullOrEmpty(SALARY_YM))
                ht.Add("@pSalaryYM", DBNull.Value);
            else
                ht.Add("@pSalaryYM", SALARY_YM);

            if (COMPANY_CD == "-1")
                ht.Add("@pCompanyCD", DBNull.Value);
            else
                ht.Add("@pCompanyCD", COMPANY_CD);

            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2SC310");

            dbConn.ExecuteSP(sb, ht, true);
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
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    #region 薪資明細表列印- WFB2C3200Print2

    #region 月薪資類表頭
    internal DataTable getSalaryItem1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select  SALARY_ID,SALARY_NAME from TB_S_M_SALARY_ITEM ");
            sb.AppendLine("where SALARY_CD<>'3' AND IS_PLUS=1 AND IS_TAX='Y' ");
            sb.AppendLine("order by SALARY_ID ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable getSalaryItem2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select  SALARY_ID,SALARY_NAME from TB_S_M_SALARY_ITEM ");
            sb.AppendLine("where SALARY_CD<>'3' AND IS_PLUS=-1 AND IS_TAX='Y' ");
            sb.AppendLine("order by SALARY_ID ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable getSalaryItem3()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select  SALARY_ID,SALARY_NAME from TB_S_M_SALARY_ITEM ");
            sb.AppendLine("where SALARY_CD<>'3' AND IS_PLUS=1 AND IS_TAX='N' ");
            sb.AppendLine("order by SALARY_ID ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable getSalaryItem4()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select  SALARY_ID,SALARY_NAME from TB_S_M_SALARY_ITEM ");
            sb.AppendLine("where SALARY_CD<>'3' AND IS_PLUS=-1 AND IS_TAX='N' ");
            sb.AppendLine("order by SALARY_ID ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable getSalaryItem5()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select SUB_CD,SUB_DESC from TB_9_M_COMM_D  ");
            sb.AppendLine("where SYS_CD='SC' and MAIN_CD='OVERTIME_PAY_TYPE' AND IS_VALID='Y' ");
            sb.AppendLine("order by SUB_CD ");
            return dbConn.Query(sb, ht);
        }

        catch (Exception)
        {
            throw;
        }

    }

    internal DataTable getSalaryItem6()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select SUB_CD,concat(SUB_DESC,+'天數')as SUB_DESC from TB_9_M_COMM_D  ");
            sb.AppendLine("where SYS_CD='SC' and MAIN_CD='SHIFT_TIME_CD' AND IS_VALID='Y' ");
            sb.AppendLine("order by SUB_CD ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable getSalaryItem7()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select ENV_ALLOWANCE_TYPE ,concat(ENV_ALLOWANCE_VALUE,+'元天數')as ENV_ALLOWANCE_VALUE from TB_D_M_ENV_ALLOWANCE_TYPE ");
            sb.AppendLine("where @SALARY_DT between START_DT and END_DT ");
            sb.AppendLine("order by ENV_ALLOWANCE_VALUE ");
            ht.Add("@SALARY_DT", SALARY_DT);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }

    }

    internal DataTable getSalaryItem8()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select SUB_LEAVE_CD,                                   ");
            sb.AppendLine("       case SUB_LEAVE_DESC                             ");
            sb.AppendLine("       when '遲到' then concat(SUB_LEAVE_DESC,+'次數') ");
            sb.AppendLine("       when '早退' then concat(SUB_LEAVE_DESC,+'次數') ");
            sb.AppendLine("       else concat(SUB_LEAVE_DESC,+'時數')             ");
            sb.AppendLine("       end  SUB_LEAVE_DESC                             ");
            sb.AppendLine("from TB_D_M_LEAVE_TYPE_D                               ");
            sb.AppendLine("where IS_USED='Y'                                      ");
            sb.AppendLine("order by MAIN_LEAVE_CD,SUB_LEAVE_CD                    ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable getSalaryItem9()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select SUB_CD,concat(SUB_DESC,+'時數')as SUB_DESC from TB_9_M_COMM_D ");
            sb.AppendLine("where SYS_CD='SC' and MAIN_CD='LEAVE_ALLOWANCE_TYPE' and IS_VALID='Y' ");
            sb.AppendLine("order by SUB_CD ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion



    internal DataTable getTITLE(string i)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("SELECT * from TB_S_R_SALARY_FB2SC310_H  ");
            sb.AppendLine("WHERE 1=1 and CREATED_BY=@CREATED_BY ");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            if (i != "")
            {
                sb.AppendLine(" and TITLE_TYPE=@TITLE_TYPE ");
                ht.Add("@TITLE_TYPE", i);
            }
            sb.AppendLine("order by REPORT_COLUMN_ID ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal int getmaxAMT()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select max(REPORT_COLUMN_ID) count from TB_S_R_SALARY_FB2SC310_H");
            sb.AppendLine("WHERE CREATED_BY=@CREATED_BY ");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            t = Convert.ToInt32(dt.Rows[0]["count"].ToString());

            return t;
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable getContent(string salary_type, string salary_dt, int maxAMT, string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("SELECT a.EMP_ID,d.DEPT_NO+':'+a.EMP_NAME EMP_NAME,a.WORK_DAYS_MONTH ");
            sb.AppendLine(" ,CASE WHEN ISNULL(b.JPN_CD,'') = '' THEN a.EMP_CHG_CD + '-非外籍' ELSE a.EMP_CHG_CD + '-' + c.SUB_DESC END as EMP_CHG_CD ");
            for (int i = 1; i <= maxAMT; i++)
            {
                sb.AppendLine(",AMOUNT_" + i.ToString().PadLeft(3, '0'));
            }

            sb.AppendLine("from TB_S_R_SALARY_FB2SC310_D a");
            sb.AppendLine(" left join TB_H_M_EMP b on a.EMP_ID = b.EMP_ID ");
            sb.AppendLine(" left join TB_9_M_COMM_D c on b.JPN_CD  = c.SUB_CD and c.SYS_CD = 'HB' and c.MAIN_CD = 'JPN_CD' and c.IS_VALID = 'Y'");
            sb.AppendLine(" left join TB_H_R_EMP_DATA_MONTH d on a.EMP_ID = d.EMP_ID  and d.YM = @salary_ym");
            sb.AppendLine("WHERE SALARY_TYPE=@SALARY_TYPE and SALARY_DT=@SALARY_DT and a.CREATED_BY=@CREATED_BY and a.COMPANY_CD = @COMPANY_CD");
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@COMPANY_CD", company_cd);
            ht.Add("@salary_ym", SALARY_YM);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //月薪資類:發薪狀態='2'(薪資計算)或'3'(關帳)
    internal DataTable getSalaryItem0(string salary_type, string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select A.EMP_ID,A.EMP_NAME,A.WORK_DAYS_MONTH,A.EMP_CHG_CD,B.SALARY_ID as SALARY_ID_1,B.SALARY_NAME as SALARY_NAME_1,B.AMOUNT as AMOUNT_1,C.SALARY_ID as SALARY_ID_2 ,C.SALARY_NAME as SALARY_NAME_2,C.AMOUNT as AMOUNT_2,D.SALARY_ID as SALARY_ID_3,D.SALARY_NAME as SALARY_NAME_3,D.AMOUNT as AMOUNT_3,E.SALARY_ID as SALARY_ID_4,E.SALARY_NAME as SALARY_NAME_4, E.AMOUNT as AMOUNT_4,F.OVERTIME_PAY_TYPE,F.TOTAL_HOURS as TOTAL_HOURS_1,G.SHIFT_TIME_CD,G.SHIFT_DAYS,H.ENV_ALLOWANCE_VALUE,H.SAPPLY_HOUR,I.SUB_LEAVE_CD,I.TOTAL_HOURS_2,J.LEAVE_ALLOWANCE_TYPE,J.TOTAL_HOURS_3  ");            
            sb.AppendLine("from( ");
            sb.AppendLine("select c.EMP_ID,c.EMP_NAME,c.WORK_DAYS_MONTH ");
            sb.AppendLine(" ,CASE WHEN ISNULL(c.JPN_CD,'') = '' THEN d.SUB_DESC + '-非外籍' ELSE d.SUB_DESC + '-' + p.SUB_DESC END as EMP_CHG_CD ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT  ");
            sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CHG_CD' and d.SUB_CD=c.EMP_CHG_CD ");
            sb.AppendLine("left join TB_9_M_COMM_D p on c.JPN_CD  = p.SUB_CD and p.SYS_CD = 'HB' and p.MAIN_CD = 'JPN_CD' and p.IS_VALID = 'Y'");
            sb.AppendLine("where a.SALARY_DT = @SALARY_DT   AND a.SALARY_TYPE= @SALARY_TYPE ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and c.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine(" )A left join  ");
            sb.AppendLine("(select b.EMP_ID,b.SALARY_ID,d.SALARY_NAME,b.AMOUNT ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_S_SALARY_PAY b on a.SALARY_TYPE=b.SALARY_TYPE and a.SALARY_DT=b.SALARY_DT ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID ");
            sb.AppendLine("left join TB_S_M_SALARY_ITEM d on b.SALARY_ID=d.SALARY_ID  ");
            sb.AppendLine("where a.SALARY_DT = @SALARY_DT  AND a.SALARY_TYPE= @SALARY_TYPE   and d.IS_TAX='Y'  ");
            sb.AppendLine("and d.IS_PLUS=1 and d.SALARY_CD<>'5' ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and c.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine(" )B on A.EMP_ID=B.EMP_ID left join ");
            sb.AppendLine("(select b.EMP_ID,b.SALARY_ID,d.SALARY_NAME,b.AMOUNT ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_S_SALARY_PAY b on a.SALARY_TYPE=b.SALARY_TYPE and a.SALARY_DT=b.SALARY_DT ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID ");
            sb.AppendLine("left join TB_S_M_SALARY_ITEM d on b.SALARY_ID=d.SALARY_ID  ");
            sb.AppendLine("where a.SALARY_DT = @SALARY_DT  AND a.SALARY_TYPE= @SALARY_TYPE  and d.IS_TAX='Y'  ");
            sb.AppendLine("and d.IS_PLUS=-1 and d.SALARY_CD<>'5' ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and c.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine("  ) C on A.EMP_ID=C.EMP_ID left join ");
            sb.AppendLine("(select b.EMP_ID,b.SALARY_ID,d.SALARY_NAME,b.AMOUNT ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_S_SALARY_PAY b on a.SALARY_TYPE=b.SALARY_TYPE and a.SALARY_DT=b.SALARY_DT ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID ");
            sb.AppendLine("left join TB_S_M_SALARY_ITEM d on b.SALARY_ID=d.SALARY_ID  ");
            sb.AppendLine("where a.SALARY_DT = @SALARY_DT  AND a.SALARY_TYPE= @SALARY_TYPE  and d.IS_TAX='N' ");
            sb.AppendLine("and d.IS_PLUS=1 and d.SALARY_CD<>'5' ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and c.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine("   )D on A.EMP_ID=D.EMP_ID left join ");
            sb.AppendLine("(select b.EMP_ID,b.SALARY_ID,d.SALARY_NAME,b.AMOUNT ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_S_SALARY_PAY b on a.SALARY_TYPE=b.SALARY_TYPE and a.SALARY_DT=b.SALARY_DT ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID ");
            sb.AppendLine("left join TB_S_M_SALARY_ITEM d on b.SALARY_ID=d.SALARY_ID  ");
            sb.AppendLine("where a.SALARY_DT = @SALARY_DT  AND a.SALARY_TYPE= @SALARY_TYPE  and d.IS_TAX='N'  ");
            sb.AppendLine("and d.IS_PLUS=-1  and d.SALARY_CD<>'5' ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and c.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            //if (PLANT_CD != "-1")
            //{
            //    sb.AppendLine(" and c.PLANT_CD=@PLANT_CD ");
            //    ht.Add("@PLANT_CD", PLANT_CD);
            //}
            sb.AppendLine(" )E on A.EMP_ID=E.EMP_ID left join ");
            sb.AppendLine("(select b.EMP_ID,b.OVERTIME_PAY_TYPE,b.TOTAL_HOURS ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_M_OVERTIME_RESULT_D b on a.SALARY_DT=b.SALARY_DT ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID ");
            sb.AppendLine("where a.SALARY_DT = @SALARY_DT  AND a.SALARY_TYPE= @SALARY_TYPE ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and c.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine("  )F on A.EMP_ID=F.EMP_ID left join ");
            sb.AppendLine("(select c.EMP_ID,c.SHIFT_TIME_CD,count(*) as SHIFT_DAYS ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT b on a.SALARY_YM=b.SALARY_YM and a.SALARY_DT=b.SALARY_DT  ");
            sb.AppendLine("left join TB_D_M_EMP_DAY_DUTY c on b.EMP_ID=c.EMP_ID and c.CALENDAR_DT>= a.DUTY_SDT and  c.CALENDAR_DT<=  a.DUTY_EDT  ");
            sb.AppendLine("where a.SALARY_TYPE= @SALARY_TYPE and  a.SALARY_DT= @SALARY_DT  and  a.SALARY_YM=@SALARY_YM ");
            sb.AppendLine("and a.DUTY_SDT <=c.CALENDAR_DT and a.DUTY_EDT >=c.CALENDAR_DT and c.WORK_DAY_CD='1' ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and b.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine(" group by c.EMP_ID,c.SHIFT_TIME_CD)G on A.EMP_ID=G.EMP_ID left join ");
            sb.AppendLine("(select a.EMP_ID,a.ENV_ALLOWANCE_VALUE,sum(APPLY_HOUR) as SAPPLY_HOUR  ");
            sb.AppendLine("from TB_D_M_ENV_ALLOWANCE_APPLY a ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT b on b.SALARY_DT=a.SALARY_DT and  a.EMP_ID=b.EMP_ID ");
            sb.AppendLine("where a.SALARY_DT= @SALARY_DT  and a.ENV_CHECK_STATUS='Y'  ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and b.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            //if (PLANT_CD != "-1")
            //{
            //    sb.AppendLine(" and b.PLANT_CD=@PLANT_CD ");
            //    ht.Add("@PLANT_CD", PLANT_CD);
            //}
            sb.AppendLine("group by a.EMP_ID,a.ENV_ALLOWANCE_VALUE)H on A.EMP_ID=H.EMP_ID left join ");
            sb.AppendLine("(select a.EMP_ID,a.SUB_LEAVE_CD,a.TOTAL_HOURS as TOTAL_HOURS_2 ");
            sb.AppendLine("from TB_S_M_LEAVE_RESULT_D a ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT b on b.SALARY_DT=a.SALARY_DT and  a.EMP_ID=b.EMP_ID ");
            sb.AppendLine("where  a.SALARY_DT= @SALARY_DT ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and b.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine("  )I on A.EMP_ID=I.EMP_ID left join ");
            sb.AppendLine("(select b.EMP_ID,b.LEAVE_ALLOWANCE_TYPE,b.TOTAL_HOURS as TOTAL_HOURS_3 ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_M_AVAILABLE_LEAVE_D b on a.SALARY_DT=b.SALARY_DT ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID ");
            sb.AppendLine("where a.SALARY_DT = @SALARY_DT  AND a.SALARY_TYPE= @SALARY_TYPE   ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and c.COMPANY_CD=@COMPANY_CD ");
                ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine("   )J on A.EMP_ID=J.EMP_ID ");
            sb.AppendLine("order by b.EMP_ID,b.SALARY_ID,f.OVERTIME_PAY_TYPE,G.SHIFT_TIME_CD,J.LEAVE_ALLOWANCE_TYPE ");

            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_TYPE", salary_type);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //月薪資類:發薪狀態='4'(月結)
    internal DataTable getSalaryItem02(string salary_type, string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select A.EMP_ID,A.EMP_NAME,A.WORK_DAYS_MONTH,A.EMP_CHG_CD,B.SALARY_ID as SALARY_ID_1,B.SALARY_NAME as SALARY_NAME_1,B.AMOUNT as AMOUNT_1,C.SALARY_ID as SALARY_ID_2 ,C.SALARY_NAME as SALARY_NAME_2,C.AMOUNT as AMOUNT_2,D.SALARY_ID as SALARY_ID_3,D.SALARY_NAME as SALARY_NAME_3,D.AMOUNT as AMOUNT_3,E.SALARY_ID as SALARY_ID_4,E.SALARY_NAME as SALARY_NAME_4, E.AMOUNT as AMOUNT_4,F.OVERTIME_PAY_TYPE,F.TOTAL_HOURS as TOTAL_HOURS_1,G.SHIFT_TIME_CD,G.SHIFT_DAYS,H.ENV_ALLOWANCE_VALUE,H.SAPPLY_HOUR,I.SUB_LEAVE_CD,I.TOTAL_HOURS_2,J.LEAVE_ALLOWANCE_TYPE,J.TOTAL_HOURS_3  ");
            sb.AppendLine("from( ");
            sb.AppendLine("select c.EMP_ID,c.EMP_NAME,c.WORK_DAYS_MONTH ");
            sb.AppendLine(" ,CASE WHEN ISNULL(c.JPN_CD,'') = '' THEN d.SUB_DESC + '-非外籍' ELSE d.SUB_DESC + '-' + p.SUB_DESC END as EMP_CHG_CD ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT  ");
            sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CHG_CD' and d.SUB_CD=c.EMP_CHG_CD ");
            sb.AppendLine(" left join TB_9_M_COMM_D p on c.JPN_CD  = p.SUB_CD and p.SYS_CD = 'HB' and p.MAIN_CD = 'JPN_CD' and p.IS_VALID = 'Y'");
            sb.AppendLine("where a.SALARY_DT = @SALARY_DT   AND a.SALARY_TYPE= @SALARY_TYPE ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and c.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine(" )A left join  ");
            sb.AppendLine("(select b.EMP_ID,b.SALARY_ID,d.SALARY_NAME,b.AMOUNT ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_M_SALARY_PAY b on a.SALARY_TYPE=b.SALARY_TYPE and a.SALARY_DT=b.SALARY_DT ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID ");
            sb.AppendLine("left join TB_S_M_SALARY_ITEM d on b.SALARY_ID=d.SALARY_ID  ");
            sb.AppendLine("where a.SALARY_DT = @SALARY_DT AND a.SALARY_TYPE= @SALARY_TYPE and d.IS_TAX='Y'  ");
            sb.AppendLine("and d.IS_PLUS=1 and d.SALARY_CD<>'5' ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and c.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine(" )B on A.EMP_ID=B.EMP_ID left join ");
            sb.AppendLine("(select b.EMP_ID,b.SALARY_ID,d.SALARY_NAME,b.AMOUNT ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_M_SALARY_PAY b on a.SALARY_TYPE=b.SALARY_TYPE and a.SALARY_DT=b.SALARY_DT ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID ");
            sb.AppendLine("left join TB_S_M_SALARY_ITEM d on b.SALARY_ID=d.SALARY_ID  ");
            sb.AppendLine("where a.SALARY_DT = @SALARY_DT  AND a.SALARY_TYPE= @SALARY_TYPE  and d.IS_TAX='Y'  ");
            sb.AppendLine("and d.IS_PLUS=-1 and d.SALARY_CD<>'5' ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and c.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine("  ) C on A.EMP_ID=C.EMP_ID left join ");
            sb.AppendLine("(select b.EMP_ID,b.SALARY_ID,d.SALARY_NAME,b.AMOUNT ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_M_SALARY_PAY b on a.SALARY_TYPE=b.SALARY_TYPE and a.SALARY_DT=b.SALARY_DT ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID ");
            sb.AppendLine("left join TB_S_M_SALARY_ITEM d on b.SALARY_ID=d.SALARY_ID  ");
            sb.AppendLine("where a.SALARY_DT = @SALARY_DT  AND a.SALARY_TYPE= @SALARY_TYPE  and d.IS_TAX='N' ");
            sb.AppendLine("and d.IS_PLUS=1 and d.SALARY_CD<>'5' ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and c.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine("   )D on A.EMP_ID=D.EMP_ID left join ");
            sb.AppendLine("(select b.EMP_ID,b.SALARY_ID,d.SALARY_NAME,b.AMOUNT ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_M_SALARY_PAY b on a.SALARY_TYPE=b.SALARY_TYPE and a.SALARY_DT=b.SALARY_DT ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID ");
            sb.AppendLine("left join TB_S_M_SALARY_ITEM d on b.SALARY_ID=d.SALARY_ID  ");
            sb.AppendLine("where a.SALARY_DT = @SALARY_DT  AND a.SALARY_TYPE= @SALARY_TYPE  and d.IS_TAX='N'  ");
            sb.AppendLine("and d.IS_PLUS=-1  and d.SALARY_CD<>'5' ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and c.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            //if (PLANT_CD != "-1")
            //{
            //    sb.AppendLine(" and c.PLANT_CD=@PLANT_CD ");
            //    ht.Add("@PLANT_CD", PLANT_CD);
            //}
            sb.AppendLine(" )E on A.EMP_ID=E.EMP_ID left join ");
            sb.AppendLine("(select b.EMP_ID,b.OVERTIME_PAY_TYPE,b.TOTAL_HOURS ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_M_OVERTIME_RESULT_D b on a.SALARY_DT=b.SALARY_DT ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID ");
            sb.AppendLine("where a.SALARY_DT = @SALARY_DT  AND a.SALARY_TYPE= @SALARY_TYPE ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and c.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine("  )F on A.EMP_ID=F.EMP_ID left join ");
            sb.AppendLine("(select c.EMP_ID,c.SHIFT_TIME_CD,count(*) as SHIFT_DAYS ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT b on a.SALARY_YM=b.SALARY_YM and a.SALARY_DT=b.SALARY_DT  ");
            sb.AppendLine("left join TB_D_M_EMP_DAY_DUTY c on b.EMP_ID=c.EMP_ID and c.CALENDAR_DT>= a.DUTY_SDT and  c.CALENDAR_DT<=  a.DUTY_EDT  ");
            sb.AppendLine("where a.SALARY_TYPE= @SALARY_TYPE and  a.SALARY_DT= @SALARY_DT  and  a.SALARY_YM=@SALARY_YM ");
            sb.AppendLine("and a.DUTY_SDT <=c.CALENDAR_DT and a.DUTY_EDT >=c.CALENDAR_DT and c.WORK_DAY_CD='1' ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and b.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine(" group by c.EMP_ID,c.SHIFT_TIME_CD)G on A.EMP_ID=G.EMP_ID left join ");
            sb.AppendLine("(select a.EMP_ID,a.ENV_ALLOWANCE_VALUE,sum(APPLY_HOUR) as SAPPLY_HOUR  ");
            sb.AppendLine("from TB_D_M_ENV_ALLOWANCE_APPLY a ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT b on b.SALARY_DT=a.SALARY_DT and  a.EMP_ID=b.EMP_ID ");
            sb.AppendLine("where a.SALARY_DT= @SALARY_DT  and a.ENV_CHECK_STATUS='Y'  ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and b.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            //if (PLANT_CD != "-1")
            //{
            //    sb.AppendLine(" and b.PLANT_CD=@PLANT_CD ");
            //    ht.Add("@PLANT_CD", PLANT_CD);
            //}
            sb.AppendLine("group by a.EMP_ID,a.ENV_ALLOWANCE_VALUE)H on A.EMP_ID=H.EMP_ID left join ");
            sb.AppendLine("(select a.EMP_ID,a.SUB_LEAVE_CD,a.TOTAL_HOURS as TOTAL_HOURS_2 ");
            sb.AppendLine("from TB_S_M_LEAVE_RESULT_D a ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT b on b.SALARY_DT=a.SALARY_DT and  a.EMP_ID=b.EMP_ID ");
            sb.AppendLine("where  a.SALARY_DT= @SALARY_DT ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and b.COMPANY_CD=@COMPANY_CD ");
                //ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine("  )I on A.EMP_ID=I.EMP_ID left join ");
            sb.AppendLine("(select b.EMP_ID,b.LEAVE_ALLOWANCE_TYPE,b.TOTAL_HOURS as TOTAL_HOURS_3 ");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H a ");
            sb.AppendLine("left join TB_S_M_AVAILABLE_LEAVE_D b on a.SALARY_DT=b.SALARY_DT ");
            sb.AppendLine("left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID ");
            sb.AppendLine("where a.SALARY_DT = @SALARY_DT  AND a.SALARY_TYPE= @SALARY_TYPE   ");
            if (COMPANY_CD != "-1")
            {
                sb.AppendLine(" and c.COMPANY_CD=@COMPANY_CD ");
                ht.Add("@COMPANY_CD", COMPANY_CD);
            }
            sb.AppendLine("   )J on A.EMP_ID=J.EMP_ID ");
            sb.AppendLine("order by b.EMP_ID,b.SALARY_ID,f.OVERTIME_PAY_TYPE,G.SHIFT_TIME_CD,J.LEAVE_ALLOWANCE_TYPE ");

            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_TYPE", salary_type);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //其他類薪資:發薪狀態='2'(薪資計算)或'3'(關帳)
    internal DataTable getSheet2(string salary_type, string salary_dt, string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select b.SALARY_DT,b.SALARY_TYPE,b.EMP_ID,b.DEPT_NO+':'+b.EMP_NAME EMP_NAME,b.SALARY_ACCOUNT_NO,SUM(a.AMOUNT) as AMT , ISNULL(sum(c.AMOUNT),0) as TAX	");
            sb.AppendLine("     ,ABS(ISNULL(sum(d.AMOUNT),0)) as INS2_AMT,ISNULL(sum(e.REPAY_AMT),0) as REPAY,ABS(ISNULL(sum(f.AMOUNT),0)) as AMT2 ");            
            sb.AppendLine("      ,CASE WHEN ISNULL(b.JPN_CD,'') = '' THEN b.COMPANY_CD + '-非外籍' ELSE b.COMPANY_CD + '-' + p.SUB_DESC END as COMPANY_CD");
            sb.AppendLine("      ,b.PLANT_CD,vm.WS_CD,vm.PJOB_CD,vm.PJOB_DESC,b.DEPT_NO,vm.DEPT_NAME_20,vm.DEPT_NAME_30,vm.DEPT_NAME_40,vm.LEVEL_CD	");
            sb.AppendLine("	     ,IIF(b.COMPANY_CD<>'K', SUM(a.AMOUNT)-ABS(ISNULL(sum(c.AMOUNT),0))-ABS(ISNULL(sum(e.REPAY_AMT),0)) ");
            sb.AppendLine("      ,SUM(a.AMOUNT)-ABS(ISNULL(sum(c.AMOUNT),0))-ABS(ISNULL(sum(d.AMOUNT),0))-ABS(ISNULL(sum(e.REPAY_AMT),0))-ABS(ISNULL(sum(f.AMOUNT),0))) as total ");
            sb.AppendLine("from TB_S_M_EMP_RESULT_TMP b	");
            sb.AppendLine("left join VW_H_EMP_DATA vm on vm.EMP_ID=b.EMP_ID 																								");
            /* 20160219 優退金如果手動在SC230多一種項目 (3068:假日換休減項)，原寫法的應付金額.稅額.補充保費都會錯誤 TERRYMODIFY */
            sb.AppendLine("left join ( select SALARY_DT,SALARY_TYPE,EMP_ID,PAY_KIND,SUM(AMOUNT * IS_PLUS) as AMOUNT from  TB_S_S_SALARY_PAY 								");
            sb.AppendLine("where SALARY_ID not in ('4001','1036','2036','3036','1040','1052','2052','3052') 	                                                            ");
            sb.AppendLine("and SALARY_DT =@SALARY_DT and SALARY_TYPE=@SALARY_TYPE and pay_kind = @SALARY_ID	                                                                ");
            sb.AppendLine("group by SALARY_DT,SALARY_TYPE,EMP_ID,PAY_KIND	                                                                                                ");
            sb.AppendLine(")a on a.SALARY_DT =b.SALARY_DT and a.SALARY_TYPE = b.SALARY_TYPE and a.EMP_ID=b.EMP_ID and a.PAY_KIND = b.PAY_KIND	                            ");
            /* 原寫法 */
            //sb.AppendLine("left join TB_S_S_SALARY_PAY a on a.SALARY_DT =b.SALARY_DT and a.SALARY_TYPE = b.SALARY_TYPE and a.EMP_ID=b.EMP_ID 								");
            //sb.AppendLine("         and a.PAY_KIND = b.PAY_KIND and  a.SALARY_ID not in ('4001','1036','2036','3036','1040','1052','2052','3052') 		                    "); /*4001 代扣所得稅,1052 債務清償,1040 員工欠款收回 */
            
            sb.AppendLine("left join TB_S_S_SALARY_PAY c on c.SALARY_DT =b.SALARY_DT and c.SALARY_TYPE = b.SALARY_TYPE and c.EMP_ID=b.EMP_ID and c.PAY_KIND = b.PAY_KIND 	");
            sb.AppendLine("         and  c.SALARY_ID = '4001'  																												");
            sb.AppendLine("left join (select emp_id ,SALARY_DT,SALARY_TYPE,PAY_KIND,sum(amount*is_plus) 	amount															");
            sb.AppendLine("from  TB_S_S_SALARY_PAY where salary_id in ('1036','2036','3036') and salary_dt = @SALARY_DT and SALARY_TYPE=@SALARY_TYPE "); /*1036 補充保費*/
            sb.AppendLine("group by emp_id ,SALARY_DT,SALARY_TYPE,PAY_KIND	");
            sb.AppendLine(")d on d.SALARY_DT =b.SALARY_DT and d.SALARY_TYPE = b.SALARY_TYPE and d.EMP_ID=b.EMP_ID and d.PAY_KIND = b.PAY_KIND 	");
            //sb.AppendLine("left join TB_S_M_STAFF_REPAY_TMP e on e.EMP_ID =b.EMP_ID and b.SALARY_DT = e.SALARY_DT and b.SALARY_TYPE = e.SALARY_TYPE and b.SALARY_YM = e.REPAY_YM "); /* *員工積欠/
            //sb.AppendLine(" and b.PAY_KIND = e.SALARY_ID ");
            sb.AppendLine("left join (select sum(REPAY_AMT) REPAY_AMT,SALARY_DT,SALARY_TYPE,REPAY_YM,SALARY_ID,EMP_ID from TB_S_M_STAFF_REPAY_TMP group by SALARY_DT,SALARY_TYPE,REPAY_YM,SALARY_ID,EMP_ID )e ");
            sb.AppendLine(" on e.EMP_ID =b.EMP_ID and b.SALARY_DT = e.SALARY_DT and b.SALARY_TYPE = e.SALARY_TYPE and b.SALARY_YM = e.REPAY_YM and b.PAY_KIND = e.SALARY_ID ");
            sb.AppendLine(" left join (select emp_id ,SALARY_DT,SALARY_TYPE,PAY_KIND,sum(amount*is_plus) 	amount");
            sb.AppendLine(" from  TB_S_S_SALARY_PAY where salary_id in ('1052','2052','3052') and salary_dt = @SALARY_DT and SALARY_TYPE=@SALARY_TYPE"); /*1052 債務清償*/
            sb.AppendLine(" group by emp_id ,SALARY_DT,SALARY_TYPE,PAY_KIND");
            sb.AppendLine(") f on f.SALARY_DT =b.SALARY_DT and f.SALARY_TYPE = b.SALARY_TYPE and f.EMP_ID=b.EMP_ID and f.PAY_KIND = b.PAY_KIND 	");
            sb.AppendLine(" left join TB_9_M_COMM_D p on b.JPN_CD  = p.SUB_CD and p.SYS_CD = 'HB' and p.MAIN_CD = 'JPN_CD' and p.IS_VALID = 'Y'");
            sb.AppendLine("where b.SALARY_DT =@SALARY_DT AND b.SALARY_TYPE=@SALARY_TYPE																						");

          //  sb.AppendLine(" and b.COMPANY_CD=@COMPANY_CD  and a.SALARY_ID = @SALARY_ID"); BY eva MARK 2015/08/04
            sb.AppendLine(" and b.COMPANY_CD=@COMPANY_CD  and a.pay_kind = @SALARY_ID");
            ht.Add("@COMPANY_CD", company_cd);

            sb.AppendLine("Group By b.EMP_ID,b.EMP_NAME,b.SALARY_ACCOUNT_NO,b.COMPANY_CD,b.PLANT_CD,vm.WS_CD,vm.PJOB_CD,vm.PJOB_DESC,b.DEPT_NO  ");
            sb.AppendLine("        ,vm.DEPT_NAME_20,vm.DEPT_NAME_30,vm.DEPT_NAME_40,b.SALARY_DT,b.SALARY_TYPE,b.JPN_CD,p.SUB_DESC,vm.LEVEL_CD ");

            
            ht.Add("@SALARY_ID", PAY_KIND);
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //其他類薪資:發薪狀態='4'(月結)
    internal DataTable getSheet2_2(string salary_type, string salary_dt, string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select b.SALARY_DT,b.SALARY_TYPE,b.EMP_ID,b.DEPT_NO+':'+b.EMP_NAME EMP_NAME,b.SALARY_ACCOUNT_NO,SUM(a.AMOUNT) as AMT , ISNULL(SUM(c.AMOUNT),0) as TAX,ISNULL(SUM(d.AMOUNT),0) as INS2_AMT,ISNULL(SUM(e.REPAY_AMT),0) as REPAY,ABS(ISNULL(SUM(f.AMOUNT),0)) as AMT2			");            
            sb.AppendLine("      ,CASE WHEN ISNULL(b.JPN_CD,'') = '' THEN b.COMPANY_CD + '-非外籍' ELSE b.COMPANY_CD + '-' + p.SUB_DESC END as COMPANY_CD																												");
            sb.AppendLine("      ,b.PLANT_CD,vm.WS_CD,vm.PJOB_CD,vm.PJOB_DESC,b.DEPT_NO,vm.DEPT_NAME_20,vm.DEPT_NAME_30,vm.DEPT_NAME_40,vm.LEVEL_CD																													");
          //sb.AppendLine("      ,SUM(a.AMOUNT)-ISNULL(SUM(c.AMOUNT),0)-ISNULL(SUM(d.AMOUNT),0)-ISNULL(SUM(e.AMOUNT),0)-ABS(ISNULL(SUM(f.AMOUNT),0)) as total                                                                                                                                        ");
            sb.AppendLine("	     ,IIF(b.COMPANY_CD<>'K', SUM(a.AMOUNT)-ABS(ISNULL(sum(c.AMOUNT),0))-ABS(ISNULL(sum(e.REPAY_AMT),0)) ");//才庫 只扣稅額 和 薪資積欠
            sb.AppendLine("      ,SUM(a.AMOUNT)-ABS(ISNULL(sum(c.AMOUNT),0))-ABS(ISNULL(sum(d.AMOUNT),0))-ABS(ISNULL(sum(e.REPAY_AMT),0))-ABS(ISNULL(sum(f.AMOUNT),0))) as total ");
            sb.AppendLine("from TB_S_M_EMP_RESULT_TMP b																																																					");
            sb.AppendLine("left join VW_H_EMP_DATA vm on vm.EMP_ID=b.EMP_ID 																																															");

            /* 20160219 優退金如果手動在SC230多一種項目 (3068:假日換休減項)，原寫法的應付金額.稅額.補充保費都會錯誤 TERRYMODIFY */
            sb.AppendLine("left join ( select SALARY_DT,SALARY_TYPE,EMP_ID,PAY_KIND,SUM(AMOUNT * IS_PLUS) as AMOUNT from  TB_S_M_SALARY_PAY 								");
            sb.AppendLine("where SALARY_ID not in ('4001','1036','2036','3036','1040','1052','2052','3052') 	                                                            ");
            sb.AppendLine("and SALARY_DT =@SALARY_DT and SALARY_TYPE=@SALARY_TYPE and pay_kind = @SALARY_ID	                                                                ");
            sb.AppendLine("group by SALARY_DT,SALARY_TYPE,EMP_ID,PAY_KIND	                                                                                                ");
            sb.AppendLine(")a on a.SALARY_DT =b.SALARY_DT and a.SALARY_TYPE = b.SALARY_TYPE and a.EMP_ID=b.EMP_ID and a.PAY_KIND = b.PAY_KIND	                            ");
            /* 原寫法 */            
            //sb.AppendLine("left join TB_S_M_SALARY_PAY a on a.SALARY_DT =b.SALARY_DT and a.SALARY_TYPE = b.SALARY_TYPE and a.EMP_ID=b.EMP_ID 																															");
            //sb.AppendLine("         and a.PAY_KIND = b.PAY_KIND and  a.SALARY_ID not in ('4001','1036','2036','3036','1040','1052','2052','3052') 																																							");
            
            sb.AppendLine("left join TB_S_M_SALARY_PAY c on c.SALARY_DT =b.SALARY_DT and c.SALARY_TYPE = b.SALARY_TYPE and c.EMP_ID=b.EMP_ID and c.PAY_KIND = b.PAY_KIND 																								");
            sb.AppendLine("         and  c.SALARY_ID = '4001' ");
            sb.AppendLine("left join (select emp_id ,SALARY_DT,SALARY_TYPE,PAY_KIND,sum(amount*is_plus) 	amount																											");
            sb.AppendLine("from  TB_S_M_SALARY_PAY where salary_id in ('1036','2036','3036') and salary_dt = @SALARY_DT and SALARY_TYPE=@SALARY_TYPE ");
            sb.AppendLine("group by emp_id ,SALARY_DT,SALARY_TYPE,PAY_KIND	");
            sb.AppendLine(")d on d.SALARY_DT =b.SALARY_DT and d.SALARY_TYPE = b.SALARY_TYPE and d.EMP_ID=b.EMP_ID and d.PAY_KIND = b.PAY_KIND 	");
            sb.AppendLine("left join (select EMP_ID,SALARY_DT,SALARY_TYPE,REPAY_YM,SALARY_ID,sum(repay_amt) as repay_amt  from TB_S_M_STAFF_REPAYMENT_D group by EMP_ID,SALARY_DT,SALARY_TYPE,REPAY_YM,SALARY_ID )e on e.EMP_ID =b.EMP_ID and b.SALARY_DT = e.SALARY_DT and b.SALARY_TYPE = e.SALARY_TYPE and b.SALARY_YM = e.REPAY_YM and b.PAY_KIND = e.SALARY_ID  ");
            sb.AppendLine(" left join (select emp_id ,SALARY_DT,SALARY_TYPE,PAY_KIND,sum(amount*is_plus) 	amount");
            sb.AppendLine(" from  TB_S_M_SALARY_PAY where salary_id in ('1052','2052','3052') and salary_dt = @SALARY_DT and SALARY_TYPE=@SALARY_TYPE");
            sb.AppendLine(" group by emp_id ,SALARY_DT,SALARY_TYPE,PAY_KIND");
            sb.AppendLine(") f on f.SALARY_DT =b.SALARY_DT and f.SALARY_TYPE = b.SALARY_TYPE and f.EMP_ID=b.EMP_ID and f.PAY_KIND = b.PAY_KIND 	");
            sb.AppendLine(" left join TB_9_M_COMM_D p on b.JPN_CD  = p.SUB_CD and p.SYS_CD = 'HB' and p.MAIN_CD = 'JPN_CD' and p.IS_VALID = 'Y'");
            sb.AppendLine("where b.SALARY_DT =@SALARY_DT AND b.SALARY_TYPE=@SALARY_TYPE																																													");
            sb.AppendLine(" and b.COMPANY_CD=@COMPANY_CD and a.pay_kind = @SALARY_ID");
            ht.Add("@COMPANY_CD", company_cd);

            sb.AppendLine("Group By b.EMP_ID,b.EMP_NAME,b.SALARY_ACCOUNT_NO,b.COMPANY_CD,b.PLANT_CD,vm.WS_CD,vm.PJOB_CD ");
            sb.AppendLine(" ,vm.PJOB_DESC,b.DEPT_NO,vm.DEPT_NAME_20,vm.DEPT_NAME_30,vm.DEPT_NAME_40,b.SALARY_DT,b.SALARY_TYPE,b.JPN_CD,p.SUB_DESC,vm.LEVEL_CD  ");

            ht.Add("@SALARY_ID", PAY_KIND);
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

}