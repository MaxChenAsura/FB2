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
/// WFB2IA1200 的摘要描述
/// </summary>
public class CFB2IA1200DAO : BaseDAO
{
    public string INS_TYPE { get; set; }
    public string EMP_ID { get; set; }
    public string IDENTITY_KIND { get; set; }
    public string LICENSE_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string BIRTH_DT { get; set; }

    public string CHG_APP_TYPE { get; set; }
    public string COMPANY_CD { get; set; }
    public string COMPANY_SNAME { get; set; }
    public string SALARY_AMT { get; set; }
    public string INS_AMT { get; set; }
    public string EFFECT_SDT { get; set; }
    public string EFFECT_EDT { get; set; }
    public string REMARK { get; set; }

    public string RC_TYPE { get; set; }
    public string HOLD_YEAR { get; set; }

    public string SLEF_RATE { get; set; }

    public string CHG_TYPE_IN { get; set; }
    public string CHG_TYPE_OUT { get; set; }
    public string CHG_REASON_CD { get; set; }
    public string SUB_DESC { get; set; }

    public string REDUCE_CD { get; set; }

    public string SALARY_YM { get; set; }
    public string TRACE_TYPE { get; set; }
    public string TRACE_AMT { get; set; }
    public string APPROVE_BY { get; set; }

    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    public string fid { get; set; }
    public string BILLS_KIND { get; set; }
    public string FEES_YM { get; set; }
    public string TRACE_KIND { get; set; }
    
    public CFB2IA1200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string license_id, string emp_id)
    {
        try
        {
            if (sortExpression.Contains("COMPANY_CD"))
                sortExpression = sortExpression.Replace("COMPANY_CD", "a.COMPANY_CD");

            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");

            if (sortExpression.Contains("SUB_DESC"))
                sortExpression = sortExpression.Replace("SUB_DESC", "d.SUB_DESC");

            if (sortExpression.Contains("NATION_NAME"))
                sortExpression = sortExpression.Replace("NATION_NAME", "e.SUB_DESC");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.COMPANY_CD,c.COMPANY_SNAME,a.EMP_ID,a.EMP_NAME,d.SUB_DESC,a.DEPT_NO,a.DIV_DEPT_FULL_NAME,");
            sb.Append(" a.NATION_CD,e.SUB_DESC as NATION_NAME,a.EMP_CD,a.LICENSE_ID ");
            sb.Append(" from VW_H_EMP_DATA a");
            //sb.Append(" left join TB_I_M_3IN1_TXN b on b.INS_TYPE='B' and b.IDENTITY_KIND='2' and b.EMP_ID=a.EMP_ID");
            //sb.Append(" and b.LICENSE_ID=a.LICENSE_ID ");
            sb.Append(" left join TB_H_M_COMPANY c on a.COMPANY_CD= c.COMPANY_CD");
            sb.Append(" left join TB_9_M_COMM_D d on a.EMP_CD= d.SUB_CD and d.MAIN_CD='EMP_CD' AND d.IS_VALID='Y' and d.SYS_CD='HB'");
            sb.Append(" left join TB_9_M_COMM_D e on a.NATION_CD=e.SUB_CD and e.MAIN_CD='NATION_CD' AND e.IS_VALID='Y' and e.SYS_CD='HB'");
            sb.Append(" where 1=1");
            if (license_id != "")
            {
                //sb.Append(" and (a.LICENSE_ID LIKE @LICENSE_ID or b.LICENSE_ID LIKE @LICENSE_ID)");
                sb.Append(" and (a.LICENSE_ID LIKE @LICENSE_ID or ");
                sb.Append(" a.EMP_ID IN (select b.EMP_ID from TB_I_M_3IN1_TXN b where b.INS_TYPE='B' and b.IDENTITY_KIND='2' ");
                sb.Append(" and b.LICENSE_ID LIKE @LICENSE_ID) ) ");
                ht.Add("@LICENSE_ID", license_id + "%");
            }
            ////處理日期(沒有在用)
            //if (op_dt != "")
            //{
            //    sb.Append(" and a.EMP_ID in (select distinct SYS_DESC from TB_I_R_3IN1_REPORTDATA ");
            //    sb.Append(" where OP_DT >= @OP_DT +' 00:00:00' and OP_DT <= @OP_DT +' 23:59:59') ");
            //    ht.Add("@OP_DT", op_dt);
            //}

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string license_id, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from VW_H_EMP_DATA a");
            //sb.Append(" left join TB_I_M_3IN1_TXN b on b.INS_TYPE='B' and b.IDENTITY_KIND='2' and b.EMP_ID=a.EMP_ID");
            //sb.Append(" and b.LICENSE_ID=a.LICENSE_ID ");
            sb.Append(" left join TB_H_M_COMPANY c on a.COMPANY_CD= c.COMPANY_CD");
            sb.Append(" left join TB_9_M_COMM_D d on a.EMP_CD= d.SUB_CD and d.MAIN_CD='EMP_CD' AND d.IS_VALID='Y' and d.SYS_CD='HB'");
            sb.Append(" left join TB_9_M_COMM_D e on a.NATION_CD=e.SUB_CD and e.MAIN_CD='NATION_CD' AND e.IS_VALID='Y' and e.SYS_CD='HB'");
            sb.Append(" where 1=1");
            if (license_id != "")
            {
                //sb.Append(" and (a.LICENSE_ID LIKE @LICENSE_ID or b.LICENSE_ID LIKE @LICENSE_ID)");
                sb.Append(" and (a.LICENSE_ID LIKE @LICENSE_ID or ");
                sb.Append(" a.EMP_ID IN (select b.EMP_ID from TB_I_M_3IN1_TXN b where b.INS_TYPE='B' and b.IDENTITY_KIND='2' ");
                sb.Append(" and b.LICENSE_ID LIKE @LICENSE_ID) ) ");
                ht.Add("@LICENSE_ID", license_id + "%");
            }
            ////處理日期(沒有在用)
            //if (op_dt != "")
            //{
            //    sb.Append(" and a.EMP_ID in (select distinct SYS_DESC from TB_I_R_3IN1_REPORTDATA ");
            //    sb.Append(" where OP_DT >= @OP_DT +' 00:00:00' and OP_DT <= @OP_DT +' 23:59:59') ");
            //    ht.Add("@OP_DT", op_dt);
            //}

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
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

    //查詢明細表頭
    public DataTable getEmpData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.EMP_ID,a.EMP_NAME,a.LICENSE_ID,a.BIRTH_DT,b.SUB_DESC,a.DIV_DEPT_FULL_NAME,a.JOIN_DT,a.LEAVE_DT");
            sb.Append(" from VW_H_EMP_DATA a");
            sb.Append(" left join TB_9_M_COMM_D b on a.NATION_CD=b.SUB_CD and b.MAIN_CD='NATION_CD' AND b.IS_VALID='Y'");
            sb.Append(" where a.EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #region"grid 勞保"
    //勞保資料
    public DataTable getLABOR_Data(int startRowIndex, int maximumRows, string sortExpression, string emp_id)
    {
        try
        {
            if (sortExpression.Contains("CHG_APP_TYPE"))
                sortExpression = sortExpression.Replace("CHG_APP_TYPE", "b.SUB_DESC");

            if (sortExpression.Contains("COMPANY_CD"))
                sortExpression = sortExpression.Replace("COMPANY_CD", "a.COMPANY_CD");

            if (sortExpression.Contains("COMPANY_SNAME"))
                sortExpression = sortExpression.Replace("COMPANY_SNAME", "c.COMPANY_SNAME");

            if (sortExpression.Contains("EFFECT_SDT"))
                sortExpression = sortExpression.Replace("EFFECT_SDT", "a.EFFECT_SDT");

            if (sortExpression.Contains("REMARK"))
                sortExpression = sortExpression.Replace("REMARK", "a.REMARK");

            if (string.IsNullOrEmpty(sortExpression))
                sortExpression = "b.SUB_DESC,a.COMPANY_CD,c.COMPANY_SNAME,a.EFFECT_SDT";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From ( ");
            sb.Append("     select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,b.SUB_DESC CHG_APP_TYPE,a.COMPANY_CD,");
            sb.Append("     c.COMPANY_SNAME,a.SALARY_AMT,a.INS_AMT,a.EFFECT_SDT,a.EFFECT_EDT,a.REMARK");
            sb.Append("     from TB_I_M_3IN1_TXN a");
            sb.Append("     left join TB_9_M_COMM_D b on b.SYS_CD='IA' and b.MAIN_CD='CHG_APP_TYPE' and a.CHG_APP_TYPE=b.SUB_CD");
            sb.Append("     left join TB_H_M_COMPANY c on a.COMPANY_CD=c.COMPANY_CD");
            sb.Append("     where a.INS_TYPE='A' and a.EMP_ID = @EMP_ID");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //勞保資料總筆數
    public int getLABOR_Count(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select count(*) total_record ");
            sb.Append("     from TB_I_M_3IN1_TXN a");
            sb.Append("     left join TB_9_M_COMM_D b on b.SYS_CD='IA' and b.MAIN_CD='CHG_APP_TYPE' and a.CHG_APP_TYPE=b.SUB_CD");
            sb.Append("     left join TB_H_M_COMPANY c on a.COMPANY_CD=c.COMPANY_CD");
            sb.Append("     where a.INS_TYPE='A' and a.EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
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
    #endregion

    #region"grid2 勞退"
    public DataTable getPENSION_Data(int startRowIndex, int maximumRows, string sortExpression2, string emp_id)
    {
        try
        {
            if (sortExpression2.Contains("CHG_APP_TYPE"))
                sortExpression2 = sortExpression2.Replace("CHG_APP_TYPE", "b.SUB_DESC");

            if (sortExpression2.Contains("COMPANY_CD"))
                sortExpression2 = sortExpression2.Replace("COMPANY_CD", "a.COMPANY_CD");

            if (sortExpression2.Contains("COMPANY_SNAME"))
                sortExpression2 = sortExpression2.Replace("COMPANY_SNAME", "c.COMPANY_SNAME");

            if (sortExpression2.Contains("EFFECT_SDT"))
                sortExpression2 = sortExpression2.Replace("EFFECT_SDT", "a.EFFECT_SDT");

            if (sortExpression2.Contains("REMARK"))
                sortExpression2 = sortExpression2.Replace("REMARK", "a.REMARK");

            if (string.IsNullOrEmpty(sortExpression2))
                sortExpression2 = "b.SUB_DESC,a.COMPANY_CD,c.COMPANY_SNAME,a.EFFECT_SDT";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From ( ");
            sb.Append(" select ROW_NUMBER() OVER(ORDER BY " + sortExpression2 + " ) As RowNumber,b.SUB_DESC CHG_APP_TYPE,a.COMPANY_CD,c.COMPANY_SNAME,");
            sb.Append(" a.RC_TYPE,a.SALARY_AMT,a.INS_AMT,a.HOLD_YEAR,a.EFFECT_SDT,a.EFFECT_EDT,a.REMARK");
            sb.Append(" from TB_I_M_3IN1_TXN a");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='IA' and b.MAIN_CD='CHG_APP_TYPE' and a.CHG_APP_TYPE=b.SUB_CD");
            sb.Append(" left join TB_H_M_COMPANY c on a.COMPANY_CD=c.COMPANY_CD");
            sb.Append(" where a.INS_TYPE='C' and a.EMP_ID = @EMP_ID");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int getPENSION_Count(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(*) total_record ");
            sb.Append(" from TB_I_M_3IN1_TXN a");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='IA' and b.MAIN_CD='CHG_APP_TYPE' and a.CHG_APP_TYPE=b.SUB_CD");
            sb.Append(" left join TB_H_M_COMPANY c on a.COMPANY_CD=c.COMPANY_CD");
            sb.Append(" where a.INS_TYPE='C' and a.EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
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
    #endregion

    #region "grid3 勞退自提率"
    //勞退自提率資料
    public DataTable getPENSION_SELF_RATIO_Data(int startRowIndex, int maximumRows, string sortExpression3, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (string.IsNullOrEmpty(sortExpression3))
                sortExpression3 = "EFFECT_SDT,EFFECT_EDT";
            sb.Append(" select * From ( ");
            sb.Append(" select ROW_NUMBER() OVER(ORDER BY " + sortExpression3 + " ) As RowNumber,SLEF_RATE,EFFECT_SDT,EFFECT_EDT,REMARK");
            sb.Append(" from TB_I_M_RETIRE_SELFRATE");
            sb.Append(" where EMP_ID = @EMP_ID");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int getPENSION_SELF_RATIO_Count(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(*) total_record ");
            sb.Append(" from TB_I_M_RETIRE_SELFRATE");
            sb.Append(" where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
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
    #endregion

    #region "grid1 健保資料"
    public DataTable getHEALTH_Data(int startRowIndex, int maximumRows, string sortExpression1, string emp_id)
    {
        try
        {
            if (sortExpression1.Contains("SUB_DESC"))
                sortExpression1 = sortExpression1.Replace("SUB_DESC", "e.SUB_CD");

            if (sortExpression1.Contains("CHG_APP_TYPE"))
                sortExpression1 = sortExpression1.Replace("CHG_APP_TYPE", "b.SUB_DESC");

            if (sortExpression1.Contains("COMPANY_CD"))
                sortExpression1 = sortExpression1.Replace("COMPANY_CD", "a.COMPANY_CD");

            if (sortExpression1.Contains("COMPANY_SNAME"))
                sortExpression1 = sortExpression1.Replace("COMPANY_SNAME", "c.COMPANY_SNAME");

            if (sortExpression1.Contains("EFFECT_SDT"))
                sortExpression1 = sortExpression1.Replace("EFFECT_SDT", "a.EFFECT_SDT");

            if (sortExpression1.Contains("CHG_TYPE_OUT"))
                sortExpression1 = sortExpression1.Replace("CHG_TYPE_OUT", "d.SUB_CD");

            if (sortExpression1.Contains("REMARK"))
                sortExpression1 = sortExpression1.Replace("REMARK", "a.REMARK");

            if (string.IsNullOrEmpty(sortExpression1))
                sortExpression1 = "b.SUB_DESC,a.COMPANY_CD,c.COMPANY_SNAME,a.EFFECT_SDT";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From ( ");
            sb.Append(" select ROW_NUMBER() OVER(ORDER BY " + sortExpression1 + " ) As RowNumber,b.SUB_DESC CHG_APP_TYPE,a.COMPANY_CD,c.COMPANY_SNAME,");
            sb.Append(" a.INS_AMT,a.EFFECT_SDT,a.EFFECT_EDT,d.SUB_CD+'-'+d.SUB_DESC CHG_TYPE_OUT,a.CHG_REASON_CD,");
            sb.Append(" e.SUB_DESC SUB_DESC,a.REMARK,a.LICENSE_ID");
            //sb.Append(" e.SUB_DESC SUB_DESC,a.REMARK");
            sb.Append(" from TB_I_M_3IN1_TXN a");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='IA' and b.MAIN_CD='CHG_APP_TYPE' and a.CHG_APP_TYPE=b.SUB_CD");
            sb.Append(" left join TB_H_M_COMPANY c on a.COMPANY_CD=c.COMPANY_CD");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='IA' and d.MAIN_CD='CHG_TYPE_OUT' and a.CHG_TYPE_OUT=d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='IA' and e.MAIN_CD='HEA_LEAVE' and a.CHG_REASON_CD=e.SUB_CD and e.CODE_VAL1=a.CHG_TYPE_OUT");
            sb.Append(" where a.INS_TYPE='B' and a.IDENTITY_KIND='1' and a.EMP_ID = @EMP_ID");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int getHEALTH_Count(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(*) total_record ");
            sb.Append(" from TB_I_M_3IN1_TXN a");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='IA' and b.MAIN_CD='CHG_APP_TYPE' and a.CHG_APP_TYPE=b.SUB_CD");
            sb.Append(" left join TB_H_M_COMPANY c on a.COMPANY_CD=c.COMPANY_CD");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='IA' and d.MAIN_CD='CHG_TYPE_OUT' and a.CHG_TYPE_OUT=d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='IA' and e.MAIN_CD='HEA_LEAVE' and a.CHG_REASON_CD=e.SUB_CD and e.CODE_VAL1=a.CHG_TYPE_OUT");
            sb.Append(" where a.INS_TYPE='B' and a.IDENTITY_KIND='1' and a.EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
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
    #endregion

    #region "grid4 健保眷屬資料"
    public DataTable getHEALTH_FAMILY_Data(int startRowIndex, int maximumRows, string sortExpression4, string emp_id)
    {
        try
        {
            if (sortExpression4.Contains("SUB_DESC"))
                sortExpression4 = sortExpression4.Replace("SUB_DESC", "c.SUB_DESC");

            if (sortExpression4.Contains("FAMILY_NAME"))
                sortExpression4 = sortExpression4.Replace("FAMILY_NAME", "b.FAMILY_NAME");

            if (sortExpression4.Contains("FAMILY_NATION_CD"))
                sortExpression4 = sortExpression4.Replace("FAMILY_NATION_CD", "d.SUB_DESC");

            if (sortExpression4.Contains("FAMILY_BIRTH_DT"))
                sortExpression4 = sortExpression4.Replace("FAMILY_BIRTH_DT", "b.FAMILY_BIRTH_DT");

            if (sortExpression4.Contains("LICENSE_ID"))
                sortExpression4 = sortExpression4.Replace("LICENSE_ID", "a.LICENSE_ID");

            if (sortExpression4.Contains("EFFECT_SDT"))
                sortExpression4 = sortExpression4.Replace("EFFECT_SDT", "a.EFFECT_SDT");

            if (sortExpression4.Contains("CHG_TYPE_IN_NAME"))
                sortExpression4 = sortExpression4.Replace("CHG_TYPE_IN_NAME", "e.SUB_CD");

            if (sortExpression4.Contains("CHG_TYPE_OUT"))
                sortExpression4 = sortExpression4.Replace("CHG_TYPE_OUT", "f.SUB_CD");

            if (sortExpression4.Contains("CHG_REASON_CD_NAME"))
                sortExpression4 = sortExpression4.Replace("CHG_REASON_CD_NAME", "g.SUB_CD");

            if (sortExpression4.Contains("REMARK"))
                sortExpression4 = sortExpression4.Replace("REMARK", "a.REMARK");
            
            if (string.IsNullOrEmpty(sortExpression4))
                sortExpression4 = "c.SUB_DESC,a.LICENSE_ID,a.EFFECT_SDT,CHG_TYPE_IN";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From ( ");
            sb.Append(" select ROW_NUMBER() OVER(ORDER BY " + sortExpression4 + " ) As RowNumber,a.LICENSE_ID,b.FAMILY_NAME FAMILY_NAME,c.SUB_DESC SUB_DESC,d.SUB_DESC FAMILY_NATION_CD,b.FAMILY_BIRTH_DT FAMILY_BIRTH_DT,");
            sb.Append(" a.EFFECT_SDT,a.CHG_TYPE_IN,e.SUB_DESC CHG_TYPE_IN_NAME,a.EFFECT_EDT,");
            sb.Append(" f.SUB_CD+'-'+f.SUB_DESC CHG_TYPE_OUT,a.CHG_REASON_CD,g.SUB_DESC CHG_REASON_CD_NAME,a.REMARK");
            sb.Append(" from TB_I_M_3IN1_TXN a");
            sb.Append(" left join TB_H_M_EMP_FAMILY b on a.EMP_ID=b.EMP_ID and a.LICENSE_ID=b.FAMILY_LICENSE_ID ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='FAMILY_RELATION' and c.IS_VALID='Y' and c.SUB_CD=b.FAMILY_RELATION");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='NATION_CD' and d.IS_VALID='Y' and d.SUB_CD=b.FAMILY_NATION_CD");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='IA' and e.MAIN_CD='HEA_ADD' and a.CHG_TYPE_IN=e.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D f on f.SYS_CD='IA' and f.MAIN_CD='CHG_TYPE_OUT' and a.CHG_TYPE_OUT=f.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D g on g.SYS_CD='IA' and g.MAIN_CD='HEA_LEAVE' and a.CHG_REASON_CD=g.SUB_CD ");
            sb.Append(" where a.INS_TYPE='B' and a.IDENTITY_KIND='2' and a.EMP_ID = @EMP_ID");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int getHEALTH_FAMILY_Count(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(*) total_record ");
            sb.Append(" from TB_I_M_3IN1_TXN a");
            sb.Append(" left join TB_H_M_EMP_FAMILY b on a.EMP_ID=b.EMP_ID and a.LICENSE_ID=b.FAMILY_LICENSE_ID ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='FAMILY_RELATION' and c.IS_VALID='Y' and c.SUB_CD=b.FAMILY_RELATION");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='NATION_CD' and d.IS_VALID='Y' and d.SUB_CD=b.FAMILY_NATION_CD");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='IA' and e.MAIN_CD='HEA_ADD' and a.CHG_TYPE_IN=e.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D f on f.SYS_CD='IA' and f.MAIN_CD='CHG_TYPE_OUT' and a.CHG_TYPE_OUT=f.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D g on g.SYS_CD='IA' and g.MAIN_CD='HEA_LEAVE' and a.CHG_REASON_CD=g.SUB_CD ");
            sb.Append(" where a.INS_TYPE='B' and a.IDENTITY_KIND='2' and a.EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
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

    public string getCompany(string sdt,string emp_id)
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COMPANY_CD");
            sb.Append(" from TB_H_R_EMP_DATA_MONTH ");
            sb.Append(" where EMP_ID = @EMP_ID and YM = @YM");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@YM", sdt);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
	        {
		        st = dt.Rows[0]["COMPANY_CD"].ToString();
	        }

            return st;
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region "grid5 減免設定資料"
    public DataTable getREDUCE_TXN_Data(int startRowIndex, int maximumRows, string sortExpression5, string emp_id)
    {
        try
        {
            if (sortExpression5.Contains("IDENTITY_KIND"))
                sortExpression5 = sortExpression5.Replace("IDENTITY_KIND", "a.IDENTITY_KIND");

            if (sortExpression5.Contains("LICENSE_ID"))
                sortExpression5 = sortExpression5.Replace("LICENSE_ID", "a.LICENSE_ID");

            if (sortExpression5.Contains("EFFECT_SDT"))
                sortExpression5 = sortExpression5.Replace("EFFECT_SDT", "a.EFFECT_SDT");

            if (sortExpression5.Contains("REDUCE_CD"))
                sortExpression5 = sortExpression5.Replace("REDUCE_CD", "a.REDUCE_CD");

            if (sortExpression5.Contains("REMARK"))
                sortExpression5 = sortExpression5.Replace("REMARK", "a.REMARK");

            if (string.IsNullOrEmpty(sortExpression5))
                sortExpression5 = "a.IDENTITY_KIND,a.EFFECT_SDT,a.REDUCE_CD";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From ( ");
            sb.Append(" select ROW_NUMBER() OVER(ORDER BY " + sortExpression5 + " ) As RowNumber,b.SUB_CD+'-'+b.SUB_DESC IDENTITY_KIND,a.LICENSE_ID,c.EMP_NAME,");
            sb.Append(" CASE WHEN (b.SUB_CD='1') THEN '本人'	");
            sb.Append(" WHEN (b.SUB_CD='2') THEN(");
            sb.Append(" select sub_desc from TB_9_M_COMM_D where SYS_CD='HB' and MAIN_CD='FAMILY_RELATION' ");
            sb.Append(" and SUB_CD=(select FAMILY_RELATION from TB_H_M_EMP_FAMILY");
            sb.Append(" where EMP_ID=a.EMP_ID and FAMILY_LICENSE_ID=a.LICENSE_ID)");
            sb.Append(" )END AS APPELLATION,");
            sb.Append(" a.REDUCE_CD,d.REDUCE_DESC,a.EFFECT_SDT,a.EFFECT_EDT,a.REMARK");
            sb.Append(" from TB_I_M_REDUCE_TXN a");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='IA' and b.MAIN_CD='IDENTITY_KIND' and a.IDENTITY_KIND=b.SUB_CD");
            sb.Append(" left join VW_H_EMP_FAMILY_NAME c on a.EMP_ID=c.EMP_ID and a.LICENSE_ID=c.LICENSE_ID");
            sb.Append(" left join TB_I_M_REDUCE d on a.REDUCE_CD=d.REDUCE_CD and (a.EFFECT_SDT >= d.EFFECT_DT and a.EFFECT_SDT <= d.UNEFFECT_DT)");
            sb.Append(" where a.EMP_ID = @EMP_ID");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int getREDUCE_TXN_Count(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(*) total_record ");
            sb.Append(" from TB_I_M_REDUCE_TXN a");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='IA' and b.MAIN_CD='IDENTITY_KIND' and a.IDENTITY_KIND=b.SUB_CD");
            sb.Append(" left join VW_H_EMP_FAMILY_NAME c on a.EMP_ID=c.EMP_ID and a.LICENSE_ID=c.LICENSE_ID");
            sb.Append(" left join TB_I_M_REDUCE d on a.REDUCE_CD=d.REDUCE_CD and (a.EFFECT_SDT >= d.EFFECT_DT and a.EFFECT_SDT <= d.UNEFFECT_DT)");
            sb.Append(" where a.EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
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
    #endregion

    //異動類別 CHG_APP_TYPE
    public DataTable getCHG_APP_TYPE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select b.sub_cd ,b.sub_cd+'-'+b.sub_desc sub_desc");
            sb.Append(" from TB_9_M_COMM_D b");
            sb.Append(" where b.SYS_CD='IA' and b.MAIN_CD='CHG_APP_TYPE' ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //產生退保原因別資料
    public DataTable getCHG_TYPE_OUT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select b.sub_cd ,b.sub_cd+'-'+b.sub_desc sub_desc");
            sb.Append(" from TB_9_M_COMM_D b");
            sb.Append(" where b.SYS_CD='IA' and b.MAIN_CD='CHG_TYPE_OUT' ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //產生身份別資料
    public DataTable getIDENTITY_KIND()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select b.sub_cd ,b.sub_cd+'-'+b.sub_desc sub_desc");
            sb.Append(" from TB_9_M_COMM_D b");
            sb.Append(" where b.SYS_CD='IA' and b.MAIN_CD='IDENTITY_KIND'");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //新增TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 (勞保、健保、勞退、健保眷屬)
    public void add3IN1_TXN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_M_3IN1_TXN ( ");
            sb.Append(" INS_TYPE,EMP_ID,IDENTITY_KIND,LICENSE_ID,EFFECT_SDT,EFFECT_EDT,");
            sb.Append(" SALARY_AMT,INS_AMT,COMPANY_CD,CHG_APP_TYPE,CHG_TYPE_IN,CHG_TYPE_OUT,");
            sb.Append(" CHG_REASON_CD,REMARK,RC_TYPE,HOLD_YEAR,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @INS_TYPE,@EMP_ID,@IDENTITY_KIND,@LICENSE_ID,@EFFECT_SDT,@EFFECT_EDT,");
            sb.Append(" @SALARY_AMT,@INS_AMT,@COMPANY_CD,@CHG_APP_TYPE,@CHG_TYPE_IN,@CHG_TYPE_OUT,");
            sb.Append(" @CHG_REASON_CD,@REMARK,'N',0,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@INS_TYPE", INS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EFFECT_SDT", EFFECT_SDT);
            if (EFFECT_EDT == "")
                ht.Add("@EFFECT_EDT", "9999/12/31");
            else
                ht.Add("@EFFECT_EDT", EFFECT_EDT);
            ht.Add("@SALARY_AMT", SALARY_AMT);
            ht.Add("@INS_AMT", INS_AMT);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@CHG_APP_TYPE", CHG_APP_TYPE);
            ht.Add("@CHG_TYPE_IN", CHG_TYPE_IN);
            ht.Add("@CHG_TYPE_OUT", CHG_TYPE_OUT);
            ht.Add("@CHG_REASON_CD", CHG_REASON_CD);
            ht.Add("@REMARK", REMARK);
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

    //取得現有TB_I_M_3IN1_TXN資料
    public DataTable get3IN1_TXNData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID from TB_I_M_3IN1_TXN");
            sb.Append(" where INS_TYPE=@INS_TYPE and EMP_ID=@EMP_ID and IDENTITY_KIND=@IDENTITY_KIND ");
            sb.Append(" and LICENSE_ID=@LICENSE_ID and EFFECT_SDT=@EFFECT_SDT");
            ht.Add("@INS_TYPE", INS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EFFECT_SDT", EFFECT_SDT);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //更新 TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 (勞保、健保、勞退、健保眷屬)
    public void update3IN1_TXN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_I_M_3IN1_TXN ");
            sb.Append(" set SALARY_AMT=@SALARY_AMT,INS_AMT=@INS_AMT,EFFECT_EDT=@EFFECT_EDT,REMARK=@REMARK,");
            if (INS_TYPE == "B")
            {
                sb.Append(" CHG_TYPE_IN=@CHG_TYPE_IN,CHG_TYPE_OUT=@CHG_TYPE_OUT,CHG_REASON_CD=@CHG_REASON_CD,");
            }
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where INS_TYPE=@INS_TYPE and EMP_ID=@EMP_ID and IDENTITY_KIND=@IDENTITY_KIND ");
            sb.Append(" and LICENSE_ID=@LICENSE_ID and EFFECT_SDT=@EFFECT_SDT ");

            ht.Add("@SALARY_AMT", SALARY_AMT);
            ht.Add("@INS_AMT", INS_AMT);
            if (EFFECT_EDT == "")
                ht.Add("@EFFECT_EDT", "9999/12/31");
            else
                ht.Add("@EFFECT_EDT", EFFECT_EDT);

            if (INS_TYPE == "B")
            {
                ht.Add("@CHG_TYPE_IN", CHG_TYPE_IN);
                ht.Add("@CHG_TYPE_OUT", CHG_TYPE_OUT);
                ht.Add("@CHG_REASON_CD", CHG_REASON_CD);
            }
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@INS_TYPE", INS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EFFECT_SDT", EFFECT_SDT);
            if (COMPANY_CD != "")
            {
                sb.Append(" and COMPANY_CD=@COMPANY_CD ");
                ht.Add("@COMPANY_CD", COMPANY_CD);
            }

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得現有TB_I_M_RETIRE_SELFRATE資料
    public DataTable getRETIRE_SELFRATEData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID from TB_I_M_RETIRE_SELFRATE");
            sb.Append(" where EMP_ID=@EMP_ID and EFFECT_SDT>=@EFFECT_SDT ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EFFECT_SDT", DateTime.Now.AddYears(-1));
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增[TB_I_M_RETIRE_SELFRATE 勞退自提履歷檔 ](勞退自提率) 
    internal void addRETIRE_SELFRATE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_M_RETIRE_SELFRATE ( ");
            sb.Append(" EMP_ID,EFFECT_SDT,EFFECT_EDT,SLEF_RATE,REMARK,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @EMP_ID,@EFFECT_SDT,@EFFECT_EDT,@SLEF_RATE,@REMARK,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EFFECT_SDT", EFFECT_SDT);
            if (EFFECT_EDT == "")
                ht.Add("@EFFECT_EDT", "9999/12/31");
            else
                ht.Add("@EFFECT_EDT", EFFECT_EDT);

            ht.Add("@SLEF_RATE", SLEF_RATE);
            ht.Add("@REMARK", REMARK);
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

    //更新 [TB_I_M_RETIRE_SELFRATE 勞退自提履歷檔 ](勞退自提率)  
    public void updateRETIRE_SELFRATE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_I_M_RETIRE_SELFRATE ");
            sb.Append(" set EFFECT_EDT=@EFFECT_EDT,SLEF_RATE=@SLEF_RATE,REMARK=@REMARK,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EMP_ID=@EMP_ID and EFFECT_SDT=@EFFECT_SDT ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EFFECT_SDT", EFFECT_SDT);
            if (EFFECT_EDT == "")
                ht.Add("@EFFECT_EDT", "9999/12/31");
            else
                ht.Add("@EFFECT_EDT", EFFECT_EDT);

            ht.Add("@SLEF_RATE", SLEF_RATE);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得現有[TB_I_M_PERSONDATA 保險資料主檔]資料
    public DataTable getPERSONDATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID from TB_I_M_PERSONDATA");
            sb.Append(" where EMP_ID=@EMP_ID and LICENSE_ID=@LICENSE_ID ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增[TB_I_M_PERSONDATA 保險資料主檔] 
    public void addPERSONDATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_M_PERSONDATA ( ");
            sb.Append(" EMP_ID,LICENSE_ID_FIRST,LICENSE_ID,EMP_NAME,BIRTH_DT,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @EMP_ID,@LICENSE_ID_FIRST,@LICENSE_ID,@EMP_NAME,@BIRTH_DT,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID_FIRST", LICENSE_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@BIRTH_DT", BIRTH_DT);
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

    //新增[TB_I_R_DATAUPDAE_HIS 保險資料更新歷史檔]
    public void addDATAUPDAE_HIS()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_R_DATAUPDAE_HIS ( ");
            sb.Append(" EMP_ID,LICENSE_ID,CREATED_DT,LICENSE_ID_FIRST,EMP_NAME,BIRTH_DT,");
            sb.Append(" CREATED_BY,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append("@EMP_ID,@LICENSE_ID,GETDATE(),@LICENSE_ID_FIRST,@EMP_NAME,@BIRTH_DT,");
            sb.Append(" @CREATED_BY,@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@LICENSE_ID_FIRST", LICENSE_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@BIRTH_DT", BIRTH_DT);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得現有[TB_I_M_REDUCE_TXN 保險減免資料履歷檔]資料
    public DataTable getREDUCE_TXNData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID from TB_I_M_REDUCE_TXN");
            sb.Append(" where EMP_ID=@EMP_ID and IDENTITY_KIND=@IDENTITY_KIND ");
            sb.Append(" and LICENSE_ID=@LICENSE_ID and EFFECT_SDT=@EFFECT_SDT and REDUCE_CD=@REDUCE_CD");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EFFECT_SDT", EFFECT_SDT);
            ht.Add("@REDUCE_CD", REDUCE_CD);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增[TB_I_M_REDUCE_TXN 保險減免資料履歷檔 ](減免設定)	
    public void addREDUCE_TXN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_M_REDUCE_TXN ( ");
            sb.Append(" EMP_ID,IDENTITY_KIND,LICENSE_ID,EFFECT_SDT,EFFECT_EDT,REDUCE_CD,REMARK,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @EMP_ID,@IDENTITY_KIND,@LICENSE_ID,@EFFECT_SDT,@EFFECT_EDT,@REDUCE_CD,@REMARK,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EFFECT_SDT", EFFECT_SDT);
            if (EFFECT_EDT == "")
                ht.Add("@EFFECT_EDT", "9999/12/31");
            else
                ht.Add("@EFFECT_EDT", EFFECT_EDT);

            ht.Add("@REDUCE_CD", REDUCE_CD);
            ht.Add("@REMARK", REMARK);
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

    //更新[TB_I_M_REDUCE_TXN 保險減免資料履歷檔 ]
    public void updateREDUCE_TXN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_I_M_REDUCE_TXN ");
            sb.Append(" set EFFECT_EDT=@EFFECT_EDT,REMARK=@REMARK,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EMP_ID=@EMP_ID and IDENTITY_KIND=@IDENTITY_KIND ");
            sb.Append(" and LICENSE_ID=@LICENSE_ID and EFFECT_SDT=@EFFECT_SDT and REDUCE_CD=@REDUCE_CD");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EFFECT_SDT", EFFECT_SDT);
            if (EFFECT_EDT == "")
                ht.Add("@EFFECT_EDT", "9999/12/31");
            else
                ht.Add("@EFFECT_EDT", EFFECT_EDT);

            ht.Add("@REDUCE_CD", REDUCE_CD);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得現有[TB_I_M_FEES_TRACEBACK 保費追溯資料檔 ]資料
    public DataTable getFEES_TRACEBACKData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID from TB_I_M_FEES_TRACEBACK");
            sb.Append(" where SALARY_YM=@SALARY_YM and EMP_ID=@EMP_ID and INS_TYPE='B' ");
            sb.Append(" and IDENTITY_KIND='2' and LICENSE_ID=@LICENSE_ID");
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增[TB_I_M_FEES_TRACEBACK 保費追溯資料檔 ]
    public void addFEES_TRACEBACK()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_M_FEES_TRACEBACK ( ");
            sb.Append(" SALARY_YM,EMP_ID,INS_TYPE,IDENTITY_KIND,LICENSE_ID,TRACE_TYPE,TRACE_AMT,TRACE_KIND,");
            sb.Append(" REMARK,APPROVE_DT,APPROVE_BY,APPROVE_STATUS,APP_REMARK,IS_YN,SALARY_DT,SALARY_YM1,OP_DT,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @SALARY_YM,@EMP_ID,@INS_TYPE,@IDENTITY_KIND,@LICENSE_ID,@TRACE_TYPE,@TRACE_AMT,@TRACE_KIND,");
            sb.Append(" @REMARK,@APPROVE_DT,@APPROVE_BY,@APPROVE_STATUS,@APP_REMARK,@IS_YN,@SALARY_DT,@SALARY_YM1,@OP_DT,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@INS_TYPE", INS_TYPE);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@TRACE_TYPE", TRACE_TYPE);
            ht.Add("@TRACE_KIND", TRACE_KIND);
            ht.Add("@TRACE_AMT", TRACE_AMT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", APPROVE_BY);
            ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@APP_REMARK", "");
            ht.Add("@IS_YN", "N");
            ht.Add("@SALARY_DT", DBNull.Value);
            ht.Add("@SALARY_YM1", "");
            ht.Add("@OP_DT", DBNull.Value);
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
    public void update_BILLS_COMPARE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" update TB_I_M_BILLS_COMPARE");
            sb.AppendLine(" set TRACED_YN = 'Y' ");            
            sb.AppendLine(" , UPDATED_BY = @UPDATED_BY , UPDATED_DT = getdate() , FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where BILLS_KIND =@BILLS_KIND  and FEES_YM= @FEES_YM and COMPANY_CD = @COMPANY_CD and EMP_ID = @EMP_ID");
            sb.AppendLine(" and IDENTITY_KIND =@IDENTITY_KIND  and LICENSE_ID= @LICENSE_ID");
                        
            ht.Add("@BILLS_KIND", BILLS_KIND);
            ht.Add("@FEES_YM", FEES_YM);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            ht.Add("@LICENSE_ID", LICENSE_ID);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", fid);

            dbConn.QueryT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得核定人員
    public DataTable getAPPROVE_BY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select DIRECT_HEAD_EMP_ID from VW_H_EMP_DATA");
            sb.Append(" where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得眷屬出生日期(健保眷屬)
    public DataTable getFAMILY_BIRTH_DT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select FAMILY_BIRTH_DT from TB_H_M_EMP_FAMILY");
            sb.Append(" where EMP_ID=@EMP_ID and FAMILY_LICENSE_ID=@FAMILY_LICENSE_ID");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FAMILY_LICENSE_ID", LICENSE_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //刪除 TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 (勞保、健保、勞退、健保眷屬)
    public void delete3IN1_TXN(string ins_type, string emp_id, string identity_kind, string license_id, string effect_sdt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_I_M_3IN1_TXN set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2IA120' ");
            sb.Append(" where INS_TYPE=@INS_TYPE and EMP_ID=@EMP_ID and IDENTITY_KIND=@IDENTITY_KIND ");
            sb.Append(" and LICENSE_ID=@LICENSE_ID and EFFECT_SDT=@EFFECT_SDT; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_I_M_3IN1_TXN ");
            sb.Append(" where INS_TYPE=@INS_TYPE and EMP_ID=@EMP_ID and IDENTITY_KIND=@IDENTITY_KIND ");
            sb.Append(" and LICENSE_ID=@LICENSE_ID and EFFECT_SDT=@EFFECT_SDT; ");
            ht.Add("@INS_TYPE", ins_type);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IDENTITY_KIND", identity_kind);
            ht.Add("@LICENSE_ID", license_id);
            ht.Add("@EFFECT_SDT", effect_sdt);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除 [TB_I_M_RETIRE_SELFRATE 勞退自提履歷檔 ](勞退自提率) 
    public void deleteRETIRE_SELFRATE(string emp_id, string effect_sdt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_I_M_RETIRE_SELFRATE set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2IA120' ");
            sb.Append(" where EMP_ID=@EMP_ID and EFFECT_SDT=@EFFECT_SDT; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_I_M_RETIRE_SELFRATE ");
            sb.Append(" where EMP_ID=@EMP_ID and EFFECT_SDT=@EFFECT_SDT; ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@EFFECT_SDT", effect_sdt);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除[TB_I_M_REDUCE_TXN 保險減免資料履歷檔](減免設定)
    public void deleteREDUCE_TXN(string emp_id, string identity_kind, string license_id, string effect_sdt, string reduce_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_I_M_REDUCE_TXN set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2IA120' ");
            sb.Append(" where EMP_ID=@EMP_ID and IDENTITY_KIND=@IDENTITY_KIND ");
            sb.Append(" and LICENSE_ID=@LICENSE_ID and EFFECT_SDT=@EFFECT_SDT and REDUCE_CD=@REDUCE_CD; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_I_M_REDUCE_TXN ");
            sb.Append(" where EMP_ID=@EMP_ID and IDENTITY_KIND=@IDENTITY_KIND ");
            sb.Append(" and LICENSE_ID=@LICENSE_ID and EFFECT_SDT=@EFFECT_SDT and REDUCE_CD=@REDUCE_CD; ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IDENTITY_KIND", identity_kind);
            ht.Add("@LICENSE_ID", license_id);
            ht.Add("@EFFECT_SDT", effect_sdt);
            ht.Add("@REDUCE_CD", reduce_cd);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable get3IN1_TXN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SALARY_AMT,INS_AMT from TB_I_M_3IN1_TXN");
            sb.Append(" where INS_TYPE='B' and EMP_ID = @EMP_ID and IDENTITY_KIND='1'");
            sb.Append(" order by effect_sdt desc");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkINS_AMT(string ins_type, string ins_amt, string salary_amt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select INS_TYPE from TB_I_M_LEVEL");
            sb.Append(" where INS_TYPE=@INS_TYPE and INS_AMT = @INS_AMT and INS_LOW <= @SALARY_AMT and INS_TOP >= @SALARY_AMT ");
            ht.Add("@INS_TYPE", ins_type);
            ht.Add("@INS_AMT", ins_amt);
            ht.Add("@SALARY_AMT", salary_amt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable check3IN1_TXN(string ins_type, string identity_kind, string emp_id, string license_id, string effect_sdt, string effect_edt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select INS_TYPE from TB_I_M_3IN1_TXN");
            sb.Append(" where INS_TYPE=@INS_TYPE and EMP_ID = @EMP_ID and LICENSE_ID = @LICENSE_ID and IDENTITY_KIND=@IDENTITY_KIND ");
            if (effect_edt == "")
            {
                sb.Append(" and (EFFECT_SDT <= @effect_sdt and EFFECT_EDT >= @effect_sdt ) ");
            }
            else
            {
                sb.Append(" and ((EFFECT_SDT <= @effect_sdt and EFFECT_EDT >= @effect_sdt ) ");
                sb.Append(" or (EFFECT_SDT <= @effect_edt and EFFECT_EDT >= @effect_edt )) ");
            }
            ht.Add("@INS_TYPE", ins_type);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@LICENSE_ID", license_id);
            ht.Add("@effect_sdt", effect_sdt);
            ht.Add("@effect_edt", effect_edt);
            ht.Add("@IDENTITY_KIND", identity_kind);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public DataTable checkRETIRE_SELFRATE(string emp_id, string effect_sdt, string effect_edt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID from TB_I_M_RETIRE_SELFRATE");
            sb.Append(" where EMP_ID = @EMP_ID ");
            if (effect_edt == "")
            {
                sb.Append(" and (EFFECT_SDT <= @effect_sdt and EFFECT_EDT >= @effect_sdt ) ");
            }
            else
            {
                sb.Append(" and ((EFFECT_SDT <= @effect_sdt and EFFECT_EDT >= @effect_sdt ) ");
                sb.Append(" or (EFFECT_SDT <= @effect_edt and EFFECT_EDT >= @effect_edt )) ");
            }
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@effect_sdt", effect_sdt);
            ht.Add("@effect_edt", effect_edt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkREDUCE_TXN(string emp_id, string identity_kind, string license_id, string effect_sdt, string effect_edt ,string reduce_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID from TB_I_M_REDUCE_TXN");
            sb.Append(" where EMP_ID=@EMP_ID and IDENTITY_KIND=@IDENTITY_KIND and LICENSE_ID=@LICENSE_ID ");
            if (effect_edt == "")
            {
                sb.Append(" and (EFFECT_SDT <= @effect_sdt and EFFECT_EDT >= @effect_sdt ) ");
            }
            else
            {
                sb.Append(" and ((EFFECT_SDT <= @effect_sdt and EFFECT_EDT >= @effect_sdt ) ");
                sb.Append(" or (EFFECT_SDT <= @effect_edt and EFFECT_EDT >= @effect_edt )) ");
            }
            if (reduce_cd != "")
            {
                sb.Append(" and REDUCE_CD = @REDUCE_CD  ");
                ht.Add("@REDUCE_CD", reduce_cd);
            }

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@IDENTITY_KIND", identity_kind);
            ht.Add("@LICENSE_ID", license_id);
            ht.Add("@effect_sdt", effect_sdt);
            ht.Add("@effect_edt", effect_edt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEmpName(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID,EMP_NAME ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getCOMPANY_SNAME(string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COMPANY_CD,COMPANY_SNAME ");
            sb.Append(" from TB_H_M_COMPANY ");
            sb.Append(" where COMPANY_CD=@COMPANY_CD ");
            ht.Add("@COMPANY_CD", company_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //退保原因說明別1(修改)
    public DataTable getCHG_REASON_NAME(string chg_reason_cd, string chg_type_out)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SUB_CD,SUB_DESC ");
            sb.Append(" from TB_9_M_COMM_D ");
            sb.Append(" where SYS_CD = 'IA' and MAIN_CD = 'HEA_LEAVE' ");
            sb.Append(" and SUB_CD=@SUB_CD");
            if (chg_type_out != "-1" && chg_type_out != "")
            {
                sb.Append(" and CODE_VAL1=@CODE_VAL1 ");
                ht.Add("@CODE_VAL1", chg_type_out);
            }
            ht.Add("@SUB_CD", chg_reason_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //身分證號4
    public DataTable getLICENSE_ID(string emp_id, string license_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select a.FAMILY_LICENSE_ID,a.FAMILY_NAME,c.SUB_DESC FAMILY_RELATION_NAME,b.SUB_DESC FAMILY_NATION_NAME, ");
            sb.Append(" a.FAMILY_BIRTH_DT    ");
            sb.Append(" from TB_H_M_EMP_FAMILY a ");
            sb.Append(" left join TB_9_M_COMM_D b  on a.FAMILY_NATION_CD = b.SUB_CD and b.MAIN_CD = 'NATION_CD'  ");
            sb.Append(" left join TB_9_M_COMM_D c  on a.FAMILY_RELATION = c.SUB_CD and c.MAIN_CD = 'FAMILY_RELATION' ");
            sb.Append(" where EMP_ID=@EMP_ID and FAMILY_LICENSE_ID=@FAMILY_LICENSE_ID ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@FAMILY_LICENSE_ID", license_id);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //身分證號5
    public DataTable getLICENSE_ID1(string emp_id, string license_id, string identity_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("SELECT z.*");
            sb.Append(" FROM (");
            sb.Append(" SELECT '1' AS IDENTITY_KIND");
            sb.Append(" ,a.LICENSE_ID AS LICENSE_ID");
            sb.Append(" ,a.EMP_NAME");
            sb.Append(" ,'本人' AS REATION_NAME");
            sb.Append(" ,'1' AS FAMILY_RELATION");
            sb.Append(" ,a.EMP_ID");
            sb.Append(" ,CONVERT(varchar,a.BIRTH_DT,120) BIRTH_DT");
            sb.Append(" FROM TB_H_M_EMP a");
            sb.Append(" UNION ALL");
            sb.Append(" SELECT '2' AS IDENTITY_KIND");
            sb.Append(" ,a.FAMILY_LICENSE_ID AS LICENSE_ID");
            sb.Append(" ,a.FAMILY_NAME AS EMP_NAME");
            sb.Append(" ,c.SUB_DESC AS REATION_NAME");
            sb.Append(" ,a.FAMILY_RELATION");
            sb.Append(" ,a.EMP_ID");
            sb.Append(" ,CONVERT(varchar,a.FAMILY_BIRTH_DT,120) BIRTH_DT");
            sb.Append(" FROM TB_H_M_EMP_FAMILY a");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D c ON c.SYS_CD = 'HB'");
            sb.Append(" AND c.MAIN_CD = 'FAMILY_RELATION'");
            sb.Append(" AND c.SUB_CD = a.FAMILY_RELATION");
            sb.Append(" ) z");
            sb.Append(" WHERE z.IDENTITY_KIND is not NULL ");
            sb.Append(" and z.EMP_ID=@EMP_ID and z.LICENSE_ID=@LICENSE_ID");

            if (identity_kind != "-1" && identity_kind != "")
            {
                sb.Append(" and z.IDENTITY_KIND = @IDENTITY_KIND ");
                ht.Add("@IDENTITY_KIND", identity_kind);
            }
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@LICENSE_ID", license_id);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //減免代碼5
    public DataTable getREDUCE_DESC(string reduce_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select REDUCE_CD,REDUCE_DESC ");
            sb.Append(" from TB_I_M_REDUCE ");
            sb.Append(" where EFFECT_DT <= GETDATE() and UNEFFECT_DT >= GETDATE()   ");
            sb.Append(" and REDUCE_CD=@REDUCE_CD ");
            ht.Add("@REDUCE_CD", reduce_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //加保原因說明別4(修改)(健保眷屬)
    public DataTable getCHG_TYPE_IN_NAME(string chg_type_in)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SUB_CD,SUB_DESC ");
            sb.Append(" from TB_9_M_COMM_D ");
            sb.Append(" where SYS_CD = 'IA' and MAIN_CD = 'HEA_ADD'  ");
            sb.Append(" and SUB_CD=@SUB_CD ");
            ht.Add("@SUB_CD", chg_type_in);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}