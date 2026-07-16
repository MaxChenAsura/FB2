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
/// CFB2SC2400DAO 的摘要描述
/// </summary>
public class CFB2SC2400DAO : BaseDAO
{
    public CFB2SC2400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string SALARY_DT { get; set; }
    public string DATA_YM { get; set; }
    public string SALARY_TYPE { get; set; }
    public string EMP_ID { get; set; }
    public string SALARY_ID { get; set; }
    public string SALARY_NAME { get; set; }
    public string IS_PLUS { get; set; }
    public string IS_TAX { get; set; }
    public string TAX_FORMAT { get; set; }
    public string PAY_KIND { get; set; }
    public string PAY_TYPE { get; set; }
    public string SEQ_NO { get; set; }
    public string CHG_AMT_B { get; set; }
    public string CHG_AMT_A { get; set; }
    public string CHG_STATUS { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string REMARK { get; set; }
    public string APP_REMARK { get; set; }

    #region "Qry"
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

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable checkSALARY_ID(string salary_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select SALARY_NAME ");
            sb.AppendLine(" from TB_S_M_SALARY_ITEM ");
            sb.AppendLine(" where SALARY_ID = @SALARY_ID ");
            ht.Add("@SALARY_ID", salary_id);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string salary_type, string salary_dt
                             , string salary_id, string process_status, string emp_id, string emp_name)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "t2.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "t.EMP_NAME");
            if (sortExpression.Contains("SALARY_ID"))
                sortExpression = sortExpression.Replace("SALARY_ID", "t2.SALARY_ID");
            if (sortExpression.Contains("REMARK"))
                sortExpression = sortExpression.Replace("REMARK", "t2.REMARK");
            if (sortExpression.Contains("UPDATED_BY_DESC"))
                sortExpression = sortExpression.Replace("UPDATED_BY_DESC", "b.EMP_NAME");
            if (sortExpression == "")
            {
                sortExpression = "t2.EMP_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select * from                                                                                                ");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,                                    ");
            sb.AppendLine("        t2.DATA_YM,t2.SALARY_DT,t2.EMP_ID as EMP_ID,t.EMP_NAME,t.EMP_CD,t2.SALARY_ID as SALARY_ID,b.DEPT_NO   ");
            sb.AppendLine(" 	   ,t2.SALARY_ID +'-'+ s1.SALARY_NAME as SALARY_NAME_DESC                                                ");
            sb.AppendLine(" 	   ,t2.CHG_AMT_A as CHG_AMT_A ,t2.CHG_AMT_B as CHG_AMT_B                                                 ");
            sb.AppendLine(" 	   ,t2.SALARY_TYPE +'-'+ e.SUB_DESC as SALARY_TYPE_DESC ,t2.SALARY_TYPE                                  ");
            sb.AppendLine(" 	   ,t2.PROCESS_STATUS +'-'+ d.SUB_DESC as PROCESS_STATUS_DESC                                            ");
            sb.AppendLine(" 	   ,t2.CHG_STATUS +'-'+  p.SUB_DESC as CHG_STATUS_DESC                                                   ");
            sb.AppendLine(" 	   ,t2.PROCESS_STATUS as PROCESS_STATUS                                                                  ");
            sb.AppendLine(" 	   ,t2.CHG_STATUS as CHG_STATUS                                                                          ");
            sb.AppendLine(" 	   ,t2.SEQ_NO as SEQ_NO                                                                                  ");
            sb.AppendLine(" 	   ,s2.IS_PLUS, s2.IS_TAX ,s2.TAX_FORMAT, s2.SALARY_NAME as SALARY_NAME                                  ");
            sb.AppendLine(" 	   ,t2.APPROVE_DT as APPROVE_DT ,t2.APPROVE_BY as APPROVE_BY                                             ");
            sb.AppendLine(" 	   ,t2.REMARK as REMARK ,t2.APP_REMARK as APP_REMARK                                                     ");
            sb.AppendLine(" 	   ,CASE WHEN t2.PAY_KIND ='9999' then '月薪資' else  s2.SALARY_NAME end as PAY_KIND_NAME                ");
            sb.AppendLine(" 	   ,t2.PAY_KIND as PAY_KIND,t2.PAY_TYPE                                                                  ");
            sb.AppendLine(" 	   ,b.EMP_NAME as UPDATED_BY_DESC                                                                        ");
            sb.AppendLine(" 	   ,CONVERT(varchar(100), t2.SALARY_DT , 111) + t2.SALARY_TYPE  as qdatakey                              ");
            sb.AppendLine("   from TB_S_S_SALARY_PAY_TMP t2                                                                              ");
            sb.AppendLine("   left join TB_H_M_EMP t on t2.EMP_ID = t.EMP_ID                                                             ");
            sb.AppendLine("   left join TB_9_M_COMM_D d                                                                                  ");
            sb.AppendLine("          on d.SYS_CD='SA' and d.MAIN_CD='PROCESS_STATUS' and t2.PROCESS_STATUS = d.SUB_CD                    ");
            sb.AppendLine("   left join TB_9_M_COMM_D p                                                                                  ");
            sb.AppendLine("          on p.SYS_CD='SC' and p.MAIN_CD='CHG_STATUS' and t2.CHG_STATUS = p.SUB_CD                            ");
            sb.AppendLine("   left join TB_9_M_COMM_D e                                                                                  ");
            sb.AppendLine("          on e.SYS_CD='SC' and e.MAIN_CD='SALARY_TYPE' and t2.SALARY_TYPE = e.SUB_CD                          ");
            sb.AppendLine("   left join TB_S_M_SALARY_ITEM s1 on t2.SALARY_ID = s1.SALARY_ID                                             ");
            sb.AppendLine("   left join TB_S_M_SALARY_ITEM s2 on t2.SALARY_ID = s2.SALARY_ID                                              ");
            sb.AppendLine("   left join TB_H_M_EMP b on b.EMP_ID = t2.UPDATED_BY                                                         ");
            sb.AppendLine("   left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO                                                    ");
            sb.AppendLine("  where 1=1 and c.EMP_ID= @CURRENT_EMP_ID                                        ");

            //and c.EMP_ID= @CURRENT_EMP_ID
            ht.Add("@CURRENT_EMP_ID", SessionHandle.Current.emp_id);

            if (salary_type != "")
            {
                sb.AppendLine(" and t2.SALARY_TYPE = @SALARY_TYPE  ");
                ht.Add("@SALARY_TYPE", salary_type);
            }
            if (salary_dt != "")
            {
                sb.AppendLine(" and CONVERT(varchar(100), t2.SALARY_DT , 111) = @SALARY_DT  ");
                ht.Add("@SALARY_DT", salary_dt);
            }
            if (salary_id != "")
            {
                sb.AppendLine(" and t2.SALARY_ID = @SALARY_ID  ");
                ht.Add("@SALARY_ID", salary_id);
            }
            if (process_status != "")
            {
                sb.AppendLine(" and t2.PROCESS_STATUS = @PROCESS_STATUS  ");
                ht.Add("@PROCESS_STATUS", process_status);
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and t2.EMP_ID like '%'+ @EMP_ID +'%' ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name != "")
            {
                sb.AppendLine(" and t.EMP_NAME like '%'+ @EMP_NAME +'%'  ");
                ht.Add("@EMP_NAME", emp_name);
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
    public int getCount(int startRowIndex, int maximumRows, string salary_type, string salary_dt
                             , string salary_id, string process_status, string emp_id, string emp_name)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(*) total_record ");
            sb.AppendLine("   from TB_S_S_SALARY_PAY_TMP t2                                                                              ");
            sb.AppendLine("   left join TB_H_M_EMP t on t2.EMP_ID = t.EMP_ID                                                             ");
            sb.AppendLine("   left join TB_9_M_COMM_D d                                                                                  ");
            sb.AppendLine("          on d.SYS_CD='SA' and d.MAIN_CD='PROCESS_STATUS' and t2.PROCESS_STATUS = d.SUB_CD                    ");
            sb.AppendLine("   left join TB_9_M_COMM_D p                                                                                  ");
            sb.AppendLine("          on p.SYS_CD='SC' and p.MAIN_CD='CHG_STATUS' and t2.CHG_STATUS = p.SUB_CD                            ");
            sb.AppendLine("   left join TB_9_M_COMM_D e                                                                                  ");
            sb.AppendLine("          on e.SYS_CD='SC' and e.MAIN_CD='SALARY_TYPE' and t2.SALARY_TYPE = e.SUB_CD                          ");
            sb.AppendLine("   left join TB_S_M_SALARY_ITEM s2 on t2.PAY_KIND = s2.SALARY_ID                                              ");
            sb.AppendLine("   left join TB_H_M_EMP b on b.EMP_ID = t2.UPDATED_BY                                                         ");
            sb.AppendLine("   left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO                                                    ");
            sb.AppendLine("  where 1=1 and c.EMP_ID= @CURRENT_EMP_ID                                       ");

            //and c.EMP_ID= @CURRENT_EMP_ID 
            ht.Add("@CURRENT_EMP_ID", SessionHandle.Current.emp_id);

            if (salary_type != "")
            {
                sb.AppendLine(" and t2.SALARY_TYPE = @SALARY_TYPE  ");
                ht.Add("@SALARY_TYPE", salary_type);
            }
            if (salary_dt != "")
            {
                sb.AppendLine(" and CONVERT(varchar(100), t2.SALARY_DT , 111) = @SALARY_DT  ");
                ht.Add("@SALARY_DT", salary_dt);
            }
            if (salary_id != "")
            {
                sb.AppendLine(" and t2.SALARY_ID = @SALARY_ID  ");
                ht.Add("@SALARY_ID", salary_id);
            }
            if (process_status != "")
            {
                sb.AppendLine(" and t2.PROCESS_STATUS = @PROCESS_STATUS  ");
                ht.Add("@PROCESS_STATUS", process_status);
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and t2.EMP_ID like '%'+ @EMP_ID +'%' ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name != "")
            {
                sb.AppendLine(" and t.EMP_NAME like '%'+ @EMP_NAME +'%'  ");
                ht.Add("@EMP_NAME", emp_name);
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

    public string getPROCESS_STATUS(string PAY_KIND)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select isnull(PROCESS_STATUS,0) as PROCESS_STATUS ");
            sb.AppendLine(" from TB_S_M_SALARY_CAL_H ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine(" and SALARY_TYPE = @SALARY_TYPE and PAY_KIND = @PAY_KIND");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND);

            DataTable dt = dbConn.QueryT(sb, ht);
            if (dt.Rows.Count == 1)
            {
                return Convert.ToString(dt.Rows[0]["PROCESS_STATUS"]);
            }
            else
                return "0";
        }
        catch
        {
            throw;
        }
    }
    public DataTable getSALARY_ITEM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select SALARY_NAME,IS_PLUS,IS_TAX,TAX_FORMAT ");
            sb.AppendLine(" from TB_S_M_SALARY_ITEM ");
            sb.AppendLine(" where SALARY_ID = @SALARY_ID ");
            ht.Add("@SALARY_ID", SALARY_ID);

            return dbConn.QueryT(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public bool checkIsRepeat_InTB_S_S_SALARY_PAY(ref double old_amount)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select AMOUNT ");
            sb.AppendLine(" from TB_S_S_SALARY_PAY ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and EMP_ID =  @EMP_ID ");
            sb.AppendLine("   and SALARY_ID = @SALARY_ID ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);

            DataTable dt = dbConn.QueryT(sb, ht, true);
            if (dt.Rows.Count > 0)
            {
                old_amount = Convert.ToDouble(dt.Rows[0]["AMOUNT"]);
                return true;
            }
            else
                return false;
        }
        catch
        {
            throw;
        }
    }
    public void ApproveN_Update_SALARY_PAY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_S_SALARY_PAY ");
            sb.AppendLine(" set AMOUNT = @AMOUNT ");
            if (CHG_STATUS == "A")
                sb.AppendLine("  , DATA_SRC = @DATA_SRC ");
            sb.AppendLine("    , UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and EMP_ID =  @EMP_ID ");
            sb.AppendLine("   and SALARY_ID = @SALARY_ID ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@AMOUNT", CHG_AMT_A);
            ht.Add("@DATA_SRC", "4");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC240");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void ApproveN_Add_SALARY_PAY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("INSERT INTO TB_S_S_SALARY_PAY (SALARY_DT, DATA_YM, SALARY_TYPE, EMP_ID, SALARY_ID, PAY_KIND, PAY_TYPE, SALARY_NAME, AMOUNT ");
            sb.AppendLine(" , DATA_SRC, FORMULA, CFN_PAY, IS_PLUS, IS_TAX, TAX_FORMAT, PAY_ID, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.AppendLine(" Values (@SALARY_DT, @DATA_YM, @SALARY_TYPE, @EMP_ID, @SALARY_ID, @PAY_KIND, @PAY_TYPE,@SALARY_NAME, @AMOUNT ");
            sb.AppendLine(" ,@DATA_SRC, @FORMULA, @CFN_PAY, @IS_PLUS, @IS_TAX, @TAX_FORMAT, @PAY_ID, @CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID)");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@PAY_TYPE", PAY_TYPE);
            ht.Add("@SALARY_NAME", SALARY_NAME);
            ht.Add("@AMOUNT", CHG_AMT_A);
            ht.Add("@DATA_SRC", "4");
            ht.Add("@FORMULA", "");
            ht.Add("@CFN_PAY", "Y");
            if (IS_PLUS == "")
                ht.Add("@IS_PLUS", 0);
            else
                ht.Add("@IS_PLUS", IS_PLUS);
            ht.Add("@IS_TAX", IS_TAX);
            ht.Add("@TAX_FORMAT", TAX_FORMAT);
            ht.Add("@PAY_ID", DBNull.Value);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC240");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void ApproveU_Update_SALARY_PAY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_S_SALARY_PAY ");
            sb.AppendLine(" set AMOUNT = @AMOUNT, DATA_SRC = @DATA_SRC");
            sb.AppendLine("    , UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and EMP_ID =  @EMP_ID ");
            sb.AppendLine("   and SALARY_ID = @SALARY_ID ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@AMOUNT", CHG_AMT_A);
            ht.Add("@DATA_SRC", "4");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC240");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void ApproveD_Delete_SALARY_PAY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" delete from TB_S_S_SALARY_PAY ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and EMP_ID =  @EMP_ID ");
            sb.AppendLine("   and SALARY_ID = @SALARY_ID ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void ApproveD_Update_SALARY_PAY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_S_SALARY_PAY ");
            sb.AppendLine(" set DEL_MARK = @DEL_MARK ,CFN_PAY = @CFN_PAY  ");
            sb.AppendLine("    , UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and EMP_ID =  @EMP_ID ");
            sb.AppendLine("   and SALARY_ID = @SALARY_ID ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@DEL_MARK", "Y");
            ht.Add("@CFN_PAY", "Y");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC240");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void ApproveCR_Update_SALARY_PAY(string isCfn_pay)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_S_SALARY_PAY ");
            sb.AppendLine(" set CFN_PAY = @CFN_PAY ");
            sb.AppendLine("    , UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and EMP_ID =  @EMP_ID ");
            sb.AppendLine("   and SALARY_ID = @SALARY_ID ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@CFN_PAY", isCfn_pay);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC240");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Update_SALARY_PAY_TMP(string item_process_status)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_S_SALARY_PAY_TMP ");
            sb.AppendLine(" set PROCESS_STATUS = @PROCESS_STATUS, APPROVE_BY = @APPROVE_BY, APPROVE_DT= GETDATE() ");
            sb.AppendLine("     ,APP_REMARK = @APP_REMARK, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("   and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   and EMP_ID =  @EMP_ID ");
            sb.AppendLine("   and SALARY_ID = @SALARY_ID ");
            sb.AppendLine("   and PAY_KIND = @PAY_KIND ");
            sb.AppendLine("   and SEQ_NO = @SEQ_NO ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@PROCESS_STATUS", item_process_status);
            ht.Add("@APPROVE_BY", SessionHandle.Current.emp_id);
            ht.Add("@APP_REMARK", APP_REMARK);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC240");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

}