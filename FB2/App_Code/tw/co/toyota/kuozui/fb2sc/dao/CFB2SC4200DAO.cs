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
/// CFB2SC4200DAO 的摘要描述
/// </summary>
public class CFB2SC4200DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string DEBIT_DT { get; set; }
    public string ARREARS_SDT { get; set; }
    public string ARREARS_EDT { get; set; }
    public string AMOUNT { get; set; }
    public string TOTAL_AMT { get; set; }
    public string ARREARS_TYPE { get; set; }
    public string CAL_ORDER { get; set; }
    public string REPAY_TYPE { get; set; }
    public string VALUE { get; set; }
    public string REPAY_SRC { get; set; }
    public string OTHER_COND { get; set; }
    public string IS_VAILD { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    public string SALARY_YM { get; set; }
    public string OWE_AMOUNT { get; set; }

    public string SALARY_DT { get; set; }
    public string SALARY_TYPE { get; set; }
    public string REPAY_YM { get; set; }
    public string ORG_AMT { get; set; }
    public string REPAY_AMT { get; set; }
    public string SALARY_ID { get; set; }
    public string REPAY_DT { get; set; }

    public CFB2SC4200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
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

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    //public DataTable getCOMPANY_CD()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.AppendLine(" select sub_cd ,sub_cd+'-'+sub_desc as sub_desc ");
    //        sb.AppendLine(" from TB_9_M_COMM_D ");
    //        sb.AppendLine(" where SYS_CD = @SYS_CD ");
    //        sb.AppendLine(" and MAIN_CD = @MAIN_CD ");
    //        ht.Add("@SYS_CD", sys_cd);
    //        ht.Add("@MAIN_CD", main_cd);
    //        if (is_valid != "")
    //        {
    //            sb.AppendLine(" and IS_VALID = @IS_VALID ");
    //            ht.Add("@IS_VALID", is_valid);
    //        }
    //        return dbConn.Query(sb, ht);
    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}
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
    public DataTable getSALARY_NAME()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select SALARY_ID, SALARY_ID +'-'+ SALARY_NAME as SALARY_NAME ");
            sb.AppendLine(" from VW_SALARYAND9999 ");
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region grid
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string emp_name, string debit_sdt, string debit_edt, string arrears_type)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "t1.EMP_ID");
            if (sortExpression == "")
            {
                sortExpression = "t1.EMP_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * from                                                                                                ");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,                                    ");
            sb.AppendLine("        t1.EMP_ID, t2.EMP_NAME,t1.DEBIT_DT,t1.AMOUNT,t1.TOTAL_AMT,t1.ARREARS_TYPE,t1.CAL_ORDER                ");
            sb.AppendLine(" 	   ,t1.ARREARS_TYPE +'-'+ d.SUB_DESC as ARREARS_TYPE_DESC                                                ");
            sb.AppendLine(" 	   ,t1.REPAY_TYPE,t1.REPAY_TYPE +'-'+ e.SUB_DESC as REPAY_TYPE_DESC                                      ");
            sb.AppendLine(" 	   ,t1.REPAY_SRC ,t1.REPAY_SRC  +'-'+ f.SUB_DESC as REPAY_SRC_DESC                                       ");
            sb.AppendLine(" 	   ,t1.OTHER_COND,t1.OTHER_COND +'-'+ g.SUB_DESC as OTHER_COND_DESC                                      ");
            sb.AppendLine(" 	   ,t1.VALUE ,t1.IS_VAILD                                                                                ");
            sb.AppendLine(" 	   ,t1.EMP_ID + CONVERT(varchar(100), t1.DEBIT_DT , 111) as qdatakey                                     ");
            sb.AppendLine("   from TB_S_M_STAFF_ARREARS_H t1                                                                             ");
            sb.AppendLine("   left join TB_H_M_EMP t2  on t1.EMP_ID = t2.EMP_ID                                                          ");
            sb.AppendLine("   left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='ARREARS_TYPE' and  t1.ARREARS_TYPE = d.SUB_CD ");
            sb.AppendLine("   left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='REPAY_TYPE' and  t1.REPAY_TYPE = e.SUB_CD     ");
            sb.AppendLine("   left join TB_9_M_COMM_D f on  f.SYS_CD ='SC' and  f.MAIN_CD='REPAY_SRC' and  t1.REPAY_SRC = f.SUB_CD       ");
            sb.AppendLine("   left join TB_9_M_COMM_D g on  f.SYS_CD ='SC' and  g.MAIN_CD='OTHER_COND' and  t1.OTHER_COND = g.SUB_CD     ");
            sb.AppendLine("  where 1=1                                                                                                   ");

            if (emp_id != "")
            {
                sb.AppendLine(" and t1.EMP_ID like '%'+ @EMP_ID +'%' ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name != "")
            {
                sb.AppendLine(" and t2.EMP_NAME like '%'+ @EMP_NAME +'%' ");
                ht.Add("@EMP_NAME", emp_name);
            }
            if (debit_sdt != "")
            {
                if (debit_edt != "")
                {
                    sb.AppendLine(" and t1.DEBIT_DT >= @debit_sdt and t1.DEBIT_DT <= @debit_edt ");
                    ht.Add("@debit_sdt", debit_sdt.Replace("/", ""));
                    ht.Add("@debit_edt", debit_edt.Replace("/", ""));
                }
                else
                {
                    sb.AppendLine(" and t1.DEBIT_DT >= @debit_sdt  ");
                    ht.Add("@debit_sdt", debit_sdt.Replace("/", ""));
                }

            }
            else if (debit_edt != "")
            {
                sb.AppendLine(" and t1.DEBIT_DT <= @debit_edt  ");
                ht.Add("@debit_edt", debit_edt.Replace("/", ""));
            }

            if (arrears_type != "")
            {
                sb.AppendLine(" and t1.ARREARS_TYPE  = @ARREARS_TYPE ");
                ht.Add("@ARREARS_TYPE", arrears_type);
            }
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getCount(int startRowIndex, int maximumRows, string emp_id, string emp_name, string debit_sdt, string debit_edt, string arrears_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(*) total_record ");
            sb.AppendLine("   from TB_S_M_STAFF_ARREARS_H t1                                                                             ");
            sb.AppendLine("   left join TB_H_M_EMP t2  on t1.EMP_ID = t2.EMP_ID                                                          ");
            sb.AppendLine("   left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='ARREARS_TYPE' and  t1.ARREARS_TYPE = d.SUB_CD ");
            sb.AppendLine("   left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='REPAY_TYPE' and  t1.REPAY_TYPE = e.SUB_CD     ");
            sb.AppendLine("   left join TB_9_M_COMM_D f on  f.SYS_CD ='SC' and  f.MAIN_CD='REPAY_SRC' and  t1.REPAY_SRC = f.SUB_CD       ");
            sb.AppendLine("   left join TB_9_M_COMM_D g on  f.SYS_CD ='SC' and  g.MAIN_CD='OTHER_COND' and  t1.OTHER_COND = g.SUB_CD     ");
            sb.AppendLine("  where 1=1                                                                                                   ");

            if (emp_id != "")
            {
                sb.AppendLine(" and t1.EMP_ID like '%'+ @EMP_ID +'%' ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name != "")
            {
                sb.AppendLine(" and t2.EMP_NAME like '%'+ @EMP_NAME +'%' ");
                ht.Add("@EMP_NAME", emp_name);
            }
            if (debit_sdt != "")
            {
                if (debit_edt != "")
                {
                    sb.AppendLine(" and t1.DEBIT_DT >= @debit_sdt and t1.DEBIT_DT <= @debit_edt ");
                    ht.Add("@debit_sdt", debit_sdt.Replace("/", ""));
                    ht.Add("@debit_edt", debit_edt.Replace("/", ""));
                }
                else
                {
                    sb.AppendLine(" and t1.DEBIT_DT >= @debit_sdt  ");
                    ht.Add("@debit_sdt", debit_sdt.Replace("/", ""));
                }

            }
            else if (debit_edt != "")
            {
                sb.AppendLine(" and t1.DEBIT_DT <= @debit_edt  ");
                ht.Add("@debit_edt", debit_edt.Replace("/", ""));
            }

            if (arrears_type != "")
            {
                sb.AppendLine(" and t1.ARREARS_TYPE  = @ARREARS_TYPE ");
                ht.Add("@ARREARS_TYPE", arrears_type);
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
    public string deleteData(string deleteitem)
    {
        //刪除員工欠薪主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //寫log
        sb.AppendLine(" update TB_S_M_STAFF_ARREARS_H set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SC420' ");
        sb.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) = @qdatakey; ");

        sb.AppendLine(" delete from TB_S_M_STAFF_ARREARS_H ");
        sb.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) = @qdatakey; ");
        ht.Add("@qdatakey", deleteitem);
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
        dbConn.ExecuteT(sb, ht, true);

        //連帶員工欠薪明細檔刪除
        StringBuilder sb2 = new StringBuilder();
        Hashtable ht2 = new Hashtable();
        //寫log
        sb2.AppendLine(" update TB_S_M_STAFF_ARREARS_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SC420' ");
        sb2.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) = @qdatakey; ");

        sb2.AppendLine(" delete from TB_S_M_STAFF_ARREARS_D ");
        sb2.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) = @qdatakey; ");
        ht2.Add("@qdatakey", deleteitem);
        ht2.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
        dbConn.ExecuteT(sb2, ht2, true);

        //連帶欠薪還款明細檔刪除
        StringBuilder sb3 = new StringBuilder();
        Hashtable ht3 = new Hashtable();
        //寫log
        sb3.AppendLine(" update TB_S_M_STAFF_REPAYMENT_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SC420' ");
        sb3.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) = @qdatakey; ");

        sb3.AppendLine(" delete from TB_S_M_STAFF_REPAYMENT_D ");
        sb3.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) = @qdatakey; ");
        ht3.Add("@qdatakey", deleteitem);
        ht3.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
        dbConn.ExecuteT(sb3, ht3, true);

        return "0";
    }
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from TB_S_M_STAFF_ARREARS_H ");
            sb.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) = @EMP_ID + @DEBIT_DT");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEBIT_DT", DEBIT_DT);

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

            sb.AppendLine(" insert into TB_S_M_STAFF_ARREARS_H (EMP_ID, DEBIT_DT, AMOUNT, TOTAL_AMT, ARREARS_TYPE, CAL_ORDER, REPAY_TYPE");
            sb.AppendLine(" , VALUE, REPAY_SRC, OTHER_COND, IS_VAILD, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.AppendLine(" values (@EMP_ID, @DEBIT_DT, @AMOUNT, @TOTAL_AMT, @ARREARS_TYPE, @CAL_ORDER, @REPAY_TYPE ");
            sb.AppendLine(" , @VALUE, @REPAY_SRC, @OTHER_COND, @IS_VAILD, @CREATED_BY,GETDATE(), @UPDATED_BY,GETDATE(), @FUNC_ID) ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEBIT_DT", DEBIT_DT);
            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@TOTAL_AMT", TOTAL_AMT);
            ht.Add("@ARREARS_TYPE", ARREARS_TYPE);
            ht.Add("@CAL_ORDER", CAL_ORDER);
            ht.Add("@REPAY_TYPE", REPAY_TYPE);
            ht.Add("@VALUE", VALUE);
            ht.Add("@REPAY_SRC", REPAY_SRC);
            ht.Add("@OTHER_COND", OTHER_COND);
            ht.Add("@IS_VAILD", IS_VAILD);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC420");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
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

            sb.AppendLine(" update TB_S_M_STAFF_ARREARS_H ");
            sb.AppendLine(" set AMOUNT = @AMOUNT, TOTAL_AMT = @TOTAL_AMT, ARREARS_TYPE = @ARREARS_TYPE");
            sb.AppendLine(" , CAL_ORDER = @CAL_ORDER, REPAY_TYPE = @REPAY_TYPE, VALUE = @VALUE, REPAY_SRC = @REPAY_SRC, OTHER_COND = @OTHER_COND ");
            sb.AppendLine(" , IS_VAILD = @IS_VAILD, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) = @EMP_ID+@DEBIT_DT ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEBIT_DT", DEBIT_DT);
            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@TOTAL_AMT", TOTAL_AMT);
            ht.Add("@ARREARS_TYPE", ARREARS_TYPE);
            ht.Add("@CAL_ORDER", CAL_ORDER);
            ht.Add("@REPAY_TYPE", REPAY_TYPE);
            ht.Add("@VALUE", VALUE);
            ht.Add("@REPAY_SRC", REPAY_SRC);
            ht.Add("@OTHER_COND", OTHER_COND);
            ht.Add("@IS_VAILD", IS_VAILD);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC420");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region grid2
    public DataTable getData2(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string debit_dt)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "EMP_ID,DEBIT_DT,SALARY_YM DESC";
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * From");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,       ");
            sb.AppendLine("        EMP_ID,DEBIT_DT,SALARY_YM,AMOUNT                                         ");
            sb.AppendLine("        ,EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) + SALARY_YM as qdatakey2 ");
            sb.AppendLine("  from TB_S_M_STAFF_ARREARS_D                                                    ");
            sb.AppendLine(" where 1=1 and EMP_ID = @EMP_ID                                                  ");
            sb.AppendLine("   and CONVERT(varchar(100), DEBIT_DT , 111)= @DEBIT_DT                          ");

            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@DEBIT_DT", debit_dt);
            DataTable dt = dbConn.Query(sb, ht, true);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public int getCount2(int startRowIndex, int maximumRows, string emp_id, string debit_dt)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(*) total_record                               ");
            sb.AppendLine("  from TB_S_M_STAFF_ARREARS_D                               ");
            sb.AppendLine(" where 1=1 and EMP_ID = @EMP_ID                             ");
            sb.AppendLine("   and CONVERT(varchar(100), DEBIT_DT , 111)= @DEBIT_DT     ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@DEBIT_DT", debit_dt);
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
    public string deleteDataOwe(string deleteitem)
    {
        //刪除員工欠薪明細檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //寫log
        sb.AppendLine(" update TB_S_M_STAFF_ARREARS_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SC420' ");
        sb.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) + SALARY_YM = @qdatakey; ");

        sb.AppendLine(" delete from TB_S_M_STAFF_ARREARS_D ");
        sb.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) + SALARY_YM = @qdatakey; ");
        ht.Add("@qdatakey", deleteitem);
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
        dbConn.ExecuteT(sb, ht, true);
        return "0";
    }
    public string updateARREARS_H(string emp_id, string debit_dt, int amount)
    {
        //update 欠薪主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" update TB_S_M_STAFF_ARREARS_H set AMOUNT = @AMOUNT, UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SC420' ");
        sb.AppendLine(" where EMP_ID = @EMP_ID and CONVERT(varchar(10), DEBIT_DT , 111) = @DEBIT_DT ; ");

        ht.Add("@AMOUNT", amount);
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
        ht.Add("@EMP_ID", emp_id);
        ht.Add("@DEBIT_DT", debit_dt);

        dbConn.ExecuteT(sb, ht, true);
        return "0";
    }

    internal DataTable getExistDataOwe()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from TB_S_M_STAFF_ARREARS_D ");
            sb.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) + SALARY_YM = @EMP_ID + @DEBIT_DT + @SALARY_YM ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEBIT_DT", DEBIT_DT);
            ht.Add("@SALARY_YM", SALARY_YM);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void addDataOwe1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" insert into TB_S_M_STAFF_ARREARS_D (EMP_ID, DEBIT_DT, SALARY_YM ,AMOUNT ");
            sb.AppendLine(" , CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.AppendLine(" values (@EMP_ID, @DEBIT_DT, @SALARY_YM, @AMOUNT ");
            sb.AppendLine(" , @CREATED_BY,GETDATE(), @UPDATED_BY,GETDATE(), @FUNC_ID) ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEBIT_DT", DEBIT_DT);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@AMOUNT", OWE_AMOUNT);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC420");
            dbConn.ExecuteT(sb, ht, true);

            //更新 員工欠薪主檔(TB_S_M_STAFF_ARREARS_H)
            StringBuilder sb2 = new StringBuilder();
            Hashtable ht2 = new Hashtable();
            sb2.AppendLine(" update TB_S_M_STAFF_ARREARS_H ");
            sb2.AppendLine(" set AMOUNT = @AMOUNT, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb2.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) = @EMP_ID+@DEBIT_DT ");
            ht2.Add("@EMP_ID", EMP_ID);
            ht2.Add("@DEBIT_DT", DEBIT_DT);
            ht2.Add("@AMOUNT", Convert.ToDouble(OWE_AMOUNT));
            ht2.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht2.Add("@FUNC_ID", "FB2SC420");
            dbConn.ExecuteT(sb2, ht2, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void addDataOwe()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" insert into TB_S_M_STAFF_ARREARS_D (EMP_ID, DEBIT_DT, SALARY_YM ,AMOUNT ");
            sb.AppendLine(" , CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.AppendLine(" values (@EMP_ID, @DEBIT_DT, @SALARY_YM, @AMOUNT ");
            sb.AppendLine(" , @CREATED_BY,GETDATE(), @UPDATED_BY,GETDATE(), @FUNC_ID) ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEBIT_DT", DEBIT_DT);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@AMOUNT", OWE_AMOUNT);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC420");
            dbConn.ExecuteT(sb, ht, true);

            //更新 員工欠薪主檔(TB_S_M_STAFF_ARREARS_H)
            StringBuilder sb2 = new StringBuilder();
            Hashtable ht2 = new Hashtable();
            sb2.AppendLine(" update TB_S_M_STAFF_ARREARS_H ");
            sb2.AppendLine(" set AMOUNT = @AMOUNT, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb2.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) = @EMP_ID+@DEBIT_DT ");
            ht2.Add("@EMP_ID", EMP_ID);
            ht2.Add("@DEBIT_DT", DEBIT_DT);
            ht2.Add("@AMOUNT", Convert.ToDouble(AMOUNT) + Convert.ToDouble(OWE_AMOUNT));
            ht2.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht2.Add("@FUNC_ID", "FB2SC420");
            dbConn.ExecuteT(sb2, ht2, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region grid3
    public DataTable getData3(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string debit_dt)
    {
        try
        {
            if (sortExpression.Contains("SALARY_ID"))
                sortExpression = sortExpression.Replace("SALARY_ID", "t1.SALARY_ID");
            if (sortExpression == "")
            {
                sortExpression = "t1.EMP_ID,t1.DEBIT_DT,t1.REPAY_DT DESC";
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,                                        ");
            sb.AppendLine("       t1.EMP_ID, t1.DEBIT_DT, t1.SALARY_DT, t1.SALARY_TYPE, t1.REPAY_YM, t1.ORG_AMT, t1.REPAY_AMT, t1.REPAY_DT   ");
            sb.AppendLine("         ,t1.SALARY_ID,t1.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_ID_DESC                                                        ");
            sb.AppendLine("        ,t1.EMP_ID + CONVERT(varchar(100), t1.DEBIT_DT , 111) + CONVERT(varchar(100), t1.SALARY_DT , 111) + t1.SALARY_TYPE as qdatakey3  ");
            sb.AppendLine(" 	   ,t1.SALARY_TYPE +'-'+ d.SUB_DESC as SALARY_TYPE_DESC                                                      ");
            sb.AppendLine("    from TB_S_M_STAFF_REPAYMENT_D t1                                                                              ");
            sb.AppendLine("    left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_TYPE' and t1.SALARY_TYPE = d.SUB_CD       ");
            sb.AppendLine("    left join TB_S_M_SALARY_ITEM s on s.SALARY_ID = t1.SALARY_ID                                                  ");
            sb.AppendLine("   where 1=1 and t1.EMP_ID = @EMP_ID                                                                              ");
            sb.AppendLine("     and CONVERT(varchar(100), t1.DEBIT_DT , 111)= @DEBIT_DT                                                      ");

            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@DEBIT_DT", debit_dt);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getCount3(int startRowIndex, int maximumRows, string emp_id, string debit_dt)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select count(*) total_record                                                                 ");
            sb.AppendLine("    from TB_S_M_STAFF_REPAYMENT_D                                                            ");
            sb.AppendLine("   where 1=1 and EMP_ID = @EMP_ID                                                            ");
            sb.AppendLine("     and CONVERT(varchar(100), DEBIT_DT , 111)= @DEBIT_DT                                    ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@DEBIT_DT", debit_dt);
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
    public string deleteDataRepay(string deleteitem)
    {
        //刪除員工欠薪明細檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //寫log
        sb.AppendLine(" update TB_S_M_STAFF_REPAYMENT_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SC420' ");
        sb.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) + CONVERT(varchar(100), SALARY_DT , 111) + SALARY_TYPE = @qdatakey; ");

        sb.AppendLine(" delete from TB_S_M_STAFF_REPAYMENT_D ");
        sb.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) + CONVERT(varchar(100), SALARY_DT , 111) + SALARY_TYPE = @qdatakey; ");
        ht.Add("@qdatakey", deleteitem);
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
        dbConn.ExecuteT(sb, ht, true);
        return "0";
    }
    internal DataTable getExistDataRepay()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from TB_S_M_STAFF_REPAYMENT_D ");
            sb.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) + CONVERT(varchar(100), SALARY_DT , 111) + SALARY_TYPE = @EMP_ID + @DEBIT_DT + @SALARY_DT +@SALARY_TYPE ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEBIT_DT", DEBIT_DT);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void addDataRepay()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" insert into TB_S_M_STAFF_REPAYMENT_D (EMP_ID, DEBIT_DT, SALARY_DT , SALARY_TYPE, REPAY_YM, ORG_AMT ");
            sb.AppendLine(" , REPAY_AMT, SALARY_ID, REPAY_DT, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.AppendLine(" values (@EMP_ID, @DEBIT_DT, @SALARY_DT, @SALARY_TYPE, @REPAY_YM, @ORG_AMT ");
            sb.AppendLine(" , @REPAY_AMT, @SALARY_ID, @REPAY_DT, @CREATED_BY,GETDATE(), @UPDATED_BY,GETDATE(), @FUNC_ID) ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEBIT_DT", DEBIT_DT);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@REPAY_YM", REPAY_YM);
            ht.Add("@ORG_AMT", ORG_AMT);
            ht.Add("@REPAY_AMT", REPAY_AMT);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@REPAY_DT", REPAY_DT);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC420");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void updateDataRepay()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_M_STAFF_REPAYMENT_D ");
            sb.AppendLine(" set REPAY_YM = @REPAY_YM, ORG_AMT = @ORG_AMT, REPAY_AMT = @REPAY_AMT");
            sb.AppendLine(" , SALARY_ID = @SALARY_ID, REPAY_DT = @REPAY_DT ");
            sb.AppendLine(" , UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) + CONVERT(varchar(100), SALARY_DT , 111) + SALARY_TYPE = @EMP_ID + @DEBIT_DT + @SALARY_DT +@SALARY_TYPE ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEBIT_DT", DEBIT_DT);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@REPAY_YM", REPAY_YM);
            ht.Add("@ORG_AMT", ORG_AMT);
            ht.Add("@REPAY_AMT", REPAY_AMT);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@REPAY_DT", REPAY_DT);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC420");
            dbConn.ExecuteT(sb, ht, true);

            //更新 員工欠薪主檔(TB_S_M_STAFF_ARREARS_H)
            StringBuilder sb2 = new StringBuilder();
            Hashtable ht2 = new Hashtable();
            sb2.AppendLine(" update TB_S_M_STAFF_ARREARS_H ");
            sb2.AppendLine(" set TOTAL_AMT = @TOTAL_AMT, IS_VAILD = @IS_VAILD, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb2.AppendLine(" where EMP_ID + CONVERT(varchar(100), DEBIT_DT , 111) = @EMP_ID+@DEBIT_DT ");
            ht2.Add("@EMP_ID", EMP_ID);
            ht2.Add("@DEBIT_DT", DEBIT_DT);
            double total_amt = Convert.ToDouble(TOTAL_AMT) + Convert.ToDouble(REPAY_AMT);
            ht2.Add("@TOTAL_AMT", total_amt);

            if (Convert.ToDouble(AMOUNT) - total_amt == 0)
                ht2.Add("@IS_VAILD", "N");
            else
                ht2.Add("@IS_VAILD", "Y");

            ht2.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht2.Add("@FUNC_ID", "FB2SC420");
            dbConn.ExecuteT(sb2, ht2, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
}