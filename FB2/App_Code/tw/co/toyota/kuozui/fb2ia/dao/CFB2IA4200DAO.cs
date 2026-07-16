using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2IA4200DAO 的摘要描述
/// </summary>
public class CFB2IA4200DAO : BaseDAO
{
    public int seq_no { get; set; }
    public string emp_id { get; set; }
    public string emp_name { get; set; }
    public string salary_id { get; set; }
    public string trace_amt { get; set; }
    public string is_plus { get; set; }
    public string is_tax { get; set; }
    public string remark { get; set; }
    public string salary_ym { get; set; }
    public string ins_type { get; set; }
    public string identity_kind { get; set; }
    public string license_id { get; set; }
    public string START_DT { get; set; }
    public string END_DT { get; set; }
    public CFB2IA4200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable TB_S_M_SALARY_CAL_H(string SALARY_YM)
    {
        try
        {
            // day(SALARY_SDT)=1 -->取薪資計算日為每月1號的那一筆,避免一個月算二次薪水的問題
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select SALARY_DT,PROCESS_STATUS From TB_S_M_SALARY_CAL_H");
            sb.Append(" where SALARY_YM=@SALARY_YM and SALARY_TYPE='A' and day(SALARY_SDT)=1");
            ht.Add("@SALARY_YM", SALARY_YM);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable CheckDataNotExist_notE(string SALARY_YM, string INS_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (INS_TYPE != "E")
            {
                sb.AppendLine("select * from TB_S_M_SALARY_MONTH_CTRL");
                sb.AppendLine("where SALARY_TYPE='A' and SALARY_YM=@SALARY_YM and SALARY_LOCKED='Y'");
                if (INS_TYPE == "0")
                    sb.AppendLine("      and OPERATION_ID in ('C01','C02','C03','C04')");
                if (INS_TYPE == "A")
                    sb.AppendLine("      and OPERATION_ID in ('C01')");
                if (INS_TYPE == "B")
                    sb.AppendLine("      and OPERATION_ID in ('C02')");
                if (INS_TYPE == "C")
                    sb.AppendLine("      and OPERATION_ID in ('C03')");
                if (INS_TYPE == "D")
                    sb.AppendLine("      and OPERATION_ID in ('C04')");
            }
            ht.Add("@SALARY_YM", SALARY_YM);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable FEES_MONTH_CHECK(string SALARY_YM, string INS_TYPE)
    {
        try
        {            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (INS_TYPE != "D")
            {
                sb.Append(" Select count(*) total From TB_I_R_FEES_MONTH");
                sb.Append(" where SALARY_YM=@SALARY_YM and INS_TYPE=@INS_TYPE ");
                ht.Add("@SALARY_YM", SALARY_YM);
                ht.Add("@INS_TYPE", INS_TYPE);
            }
            else
            {
                sb.Append(" Select count(*) total From TB_I_R_GROUP_MONTH");
                sb.Append(" where SALARY_YM=@SALARY_YM  ");
                ht.Add("@SALARY_YM", SALARY_YM);                
            }
            
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable FEES_MONTH_CHECK_A(string SALARY_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select count(*) total From TB_I_R_FEES_MONTH");
            sb.Append(" where SALARY_YM=@SALARY_YM and INS_TYPE='A' ");
            ht.Add("@SALARY_YM", SALARY_YM);
            
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable FEES_MONTH_CHECK_B(string SALARY_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select count(*) total From TB_I_R_FEES_MONTH");
            sb.Append(" where SALARY_YM=@SALARY_YM and INS_TYPE='B' ");
            ht.Add("@SALARY_YM", SALARY_YM);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable FEES_MONTH_CHECK_C(string SALARY_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select count(*) total From TB_I_R_FEES_MONTH");
            sb.Append(" where SALARY_YM=@SALARY_YM and INS_TYPE='C' ");
            ht.Add("@SALARY_YM", SALARY_YM);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable FEES_MONTH_CHECK_D(string SALARY_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select count(*) total From TB_I_R_GROUP_MONTH");
            sb.Append(" where SALARY_YM=@SALARY_YM ");
            ht.Add("@SALARY_YM", SALARY_YM);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable CheckDataNotExist(string SALARY_YM, string INS_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (INS_TYPE == "E" || INS_TYPE == "0")
            {
                sb.AppendLine("select * from TB_S_M_SUBSIDY_DEDUCTIONS_1 ");
                sb.AppendLine("where SALARY_STATUS='Y' and DATA_YM=@SALARY_YM and SALARY_ID in ('3024','2024','2027','3027','2025','3025')");
            }
            ht.Add("@SALARY_YM", SALARY_YM);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable CheckDataCount(string INS_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (INS_TYPE == "E" || INS_TYPE == "0")
            {
                sb.AppendLine("select count(1) bb from TB_I_M_FEES_TRACEBACK where APPROVE_STATUS<>'Y' ");
            }
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    #region SA取消2014/10/08
    //public void Delete_TB_S_M_SUBSIDY_DEDUCTIONS_1(string SALARY_YM)
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();

    //        sb.AppendLine("delete from TB_S_M_SUBSIDY_DEDUCTIONS_1 ");
    //        sb.AppendLine("where DATA_YM=@SALARY_YM and SALARY_ID in ('3024','2024','2027','3027','2025','3025')");

    //        ht.Add("@SALARY_YM", SALARY_YM);
    //        dbConn.ExecuteT(sb, ht, true);
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
    //public DataTable Get_TB_I_M_FEES_TRACEBACK()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.AppendLine("select t1.*,c.IS_PLUS,c.IS_TAX");
    //        sb.AppendLine("from (");
    //        sb.AppendLine("    select a.*,b.EMP_NAME,");
    //        sb.AppendLine("      case when a.INS_TYPE='A' then case when a.TRACE_TYPE='A' then '3025' when a.TRACE_TYPE='B' then '2025' end");
    //        sb.AppendLine("           when a.INS_TYPE='B' then case when a.TRACE_TYPE='A' then '3027' when a.TRACE_TYPE='B' then '2027' end");
    //        sb.AppendLine("           when a.INS_TYPE='C' then case when a.TRACE_TYPE='A' then '3024' when a.TRACE_TYPE='B' then '2024' end");
    //        sb.AppendLine("      end as SALARY_ID");
    //        sb.AppendLine("    from TB_I_M_FEES_TRACEBACK a");
    //        sb.AppendLine("    left join TB_H_M_EMP b on a.EMP_ID = b.EMP_ID");
    //        sb.AppendLine("    where a.APPROVE_STATUS='Y' and a.IS_YN='N' ) t1");
    //        sb.AppendLine("left join TB_S_M_SALARY_ITEM c on t1.SALARY_ID=c.SALARY_ID");
    //        sb.AppendLine("order by t1.EMP_ID,t1.SALARY_ID,t1.SALARY_YM");

    //        return dbConn.Query(sb, ht);
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
    //public void Add_TB_S_M_SUBSIDY_DEDUCTIONS_1(string SALARY_YM)
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();

    //        sb.AppendLine("insert into TB_S_M_SUBSIDY_DEDUCTIONS_1  ");
    //        sb.AppendLine("(DATA_YM,EMP_ID,EMP_NAME,SALARY_ID,SEQ_NO,AMOUNT,IS_PLUS,IS_TAX,REMARK,SALARY_STATUS");
    //        sb.AppendLine(",CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
    //        sb.AppendLine("values (@DATA_YM,@EMP_ID,@EMP_NAME,@SALARY_ID,@SEQ_NO,@AMOUNT,@IS_PLUS,@IS_TAX,@REMARK,@SALARY_STATUS");
    //        sb.AppendLine("        ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

    //        ht.Add("@DATA_YM", SALARY_YM);
    //        ht.Add("@EMP_ID", emp_id);
    //        ht.Add("@EMP_NAME", emp_name);
    //        ht.Add("@SALARY_ID", salary_id);
    //        ht.Add("@SEQ_NO", seq_no);
    //        ht.Add("@AMOUNT", trace_amt);
    //        ht.Add("@IS_PLUS", is_plus);
    //        ht.Add("@IS_TAX", is_tax);
    //        ht.Add("@REMARK", remark);
    //        ht.Add("@SALARY_STATUS", "N");
    //        ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
    //        ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
    //        ht.Add("@FUNC_ID", "FB2IA420");
    //        dbConn.ExecuteT(sb, ht, true);
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
    #endregion
    
    public void Update_TB_I_M_FEES_TRACEBACK(string SALARY_YM, string SALARY_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("Update TB_I_M_FEES_TRACEBACK ");
            sb.AppendLine(" Set IS_YN = @IS_YN,SALARY_DT = @SALARY_DT,SALARY_YM1=@SALARY_YM1,OP_DT=GETDATE(),UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where IS_YN = 'N' and APPROVE_STATUS='Y' ");

            ht.Add("@IS_YN", "Y");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_YM1", SALARY_YM);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA420");
            //ht.Add("@SALARY_YM", salary_ym);
            //ht.Add("@EMP_ID", emp_id);
            //ht.Add("@INS_TYPE", ins_type);
            //ht.Add("@IDENTITY_KIND", identity_kind);
            //ht.Add("@LICENSE_ID", license_id);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Delete_TB_S_M_SALARY_MONTH_CTRL_A(string SALARY_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("delete from TB_S_M_SALARY_MONTH_CTRL ");
            sb.AppendLine("where SALARY_TYPE=@SALARY_TYPE and SALARY_DT=@SALARY_DT and OPERATION_ID=@OPERATION_ID");

            ht.Add("@SALARY_TYPE", "A");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@OPERATION_ID", "C01");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Add_TB_S_M_SALARY_MONTH_CTRL_A(string SALARY_YM, string SALARY_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("insert into TB_S_M_SALARY_MONTH_CTRL  ");
            sb.AppendLine("(SALARY_TYPE,SALARY_YM,SALARY_DT,OPERATION_ID,PROCESS_DT,START_DT,END_DT,FUNC_ID)");
            sb.AppendLine("values (@SALARY_TYPE,@SALARY_YM,@SALARY_DT,@OPERATION_ID,GETDATE(),@START_DT,@END_DT,@FUNC_ID)");

            ht.Add("@SALARY_TYPE", "A");
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@OPERATION_ID", "C01");
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@FUNC_ID", "FB2IA420");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Update_TB_I_R_FEES_MONTH_A(string SALARY_YM, string SALARY_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("Update TB_I_R_FEES_MONTH ");
            sb.AppendLine(" Set IS_YN=@IS_YN,SALARY_DT=@SALARY_DT,OP_DT = GETDATE(),UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where SALARY_YM = @SALARY_YM and INS_TYPE = @INS_TYPE");

            ht.Add("@IS_YN", "Y");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA420");
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@INS_TYPE", "A");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Delete_TB_S_M_SALARY_MONTH_CTRL_B(string SALARY_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("delete from TB_S_M_SALARY_MONTH_CTRL ");
            sb.AppendLine("where SALARY_TYPE=@SALARY_TYPE and SALARY_DT=@SALARY_DT and OPERATION_ID=@OPERATION_ID");

            ht.Add("@SALARY_TYPE", "A");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@OPERATION_ID", "C02");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Add_TB_S_M_SALARY_MONTH_CTRL_B(string SALARY_YM, string SALARY_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("insert into TB_S_M_SALARY_MONTH_CTRL  ");
            sb.AppendLine("(SALARY_TYPE,SALARY_YM,SALARY_DT,OPERATION_ID,PROCESS_DT,START_DT,END_DT,FUNC_ID)");
            sb.AppendLine("values (@SALARY_TYPE,@SALARY_YM,@SALARY_DT,@OPERATION_ID,GETDATE(),@START_DT,@END_DT,@FUNC_ID)");

            ht.Add("@SALARY_TYPE", "A");
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@OPERATION_ID", "C02");
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@FUNC_ID", "FB2IA420");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Update_TB_I_R_FEES_MONTH_B(string SALARY_YM, string SALARY_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("Update TB_I_R_FEES_MONTH ");
            sb.AppendLine(" Set IS_YN=@IS_YN,SALARY_DT=@SALARY_DT,OP_DT = GETDATE(),UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where SALARY_YM = @SALARY_YM and INS_TYPE = @INS_TYPE");

            ht.Add("@IS_YN", "Y");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA420");
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@INS_TYPE", "B");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Delete_TB_S_M_SALARY_MONTH_CTRL_C(string SALARY_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("delete from TB_S_M_SALARY_MONTH_CTRL ");
            sb.AppendLine("where SALARY_TYPE=@SALARY_TYPE and SALARY_DT=@SALARY_DT and OPERATION_ID=@OPERATION_ID");

            ht.Add("@SALARY_TYPE", "A");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@OPERATION_ID", "C03");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Add_TB_S_M_SALARY_MONTH_CTRL_C(string SALARY_YM, string SALARY_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("insert into TB_S_M_SALARY_MONTH_CTRL  ");
            sb.AppendLine("(SALARY_TYPE,SALARY_YM,SALARY_DT,OPERATION_ID,PROCESS_DT,START_DT,END_DT,FUNC_ID)");
            sb.AppendLine("values (@SALARY_TYPE,@SALARY_YM,@SALARY_DT,@OPERATION_ID,GETDATE(),@START_DT,@END_DT,@FUNC_ID)");

            ht.Add("@SALARY_TYPE", "A");
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@OPERATION_ID", "C03");
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@FUNC_ID", "FB2IA420");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Update_TB_I_R_FEES_MONTH_C(string SALARY_YM, string SALARY_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("Update TB_I_R_FEES_MONTH ");
            sb.AppendLine(" Set IS_YN=@IS_YN,SALARY_DT=@SALARY_DT,OP_DT = GETDATE(),UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where SALARY_YM = @SALARY_YM and INS_TYPE = @INS_TYPE");

            ht.Add("@IS_YN", "Y");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA420");
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@INS_TYPE", "C");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Delete_TB_S_M_SALARY_MONTH_CTRL_D(string SALARY_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("delete from TB_S_M_SALARY_MONTH_CTRL ");
            sb.AppendLine("where SALARY_TYPE=@SALARY_TYPE and SALARY_DT=@SALARY_DT and OPERATION_ID=@OPERATION_ID");

            ht.Add("@SALARY_TYPE", "A");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@OPERATION_ID", "C04");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Add_TB_S_M_SALARY_MONTH_CTRL_D(string SALARY_YM, string SALARY_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("insert into TB_S_M_SALARY_MONTH_CTRL  ");
            sb.AppendLine("(SALARY_TYPE,SALARY_YM,SALARY_DT,OPERATION_ID,PROCESS_DT,START_DT,END_DT,FUNC_ID)");
            sb.AppendLine("values (@SALARY_TYPE,@SALARY_YM,@SALARY_DT,@OPERATION_ID,GETDATE(),@START_DT,@END_DT,@FUNC_ID)");

            ht.Add("@SALARY_TYPE", "A");
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@OPERATION_ID", "C04");
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@FUNC_ID", "FB2IA420");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Update_TB_I_R_GROUP_MONTH_D(string SALARY_YM, string SALARY_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("Update TB_I_R_GROUP_MONTH ");
            sb.AppendLine(" Set IS_YN=@IS_YN,SALARY_DT=@SALARY_DT,OP_DT = GETDATE(),UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where SALARY_YM = @SALARY_YM");

            ht.Add("@IS_YN", "Y");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA420");
            ht.Add("@SALARY_YM", SALARY_YM);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}