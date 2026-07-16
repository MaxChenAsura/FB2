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
/// CFB2SC1100DAO 的摘要描述
/// </summary>
public class CFB2SC1100DAO : BaseDAO
{
    public CFB2SC1100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string SALARY_ID { get; set; }
    public string SALARY_NAME { get; set; }
    public string SALARY_CD { get; set; }
    public string IS_PLUS { get; set; }
    public string IS_TAX { get; set; }
    public string TAX_FORMAT { get; set; }
    public string ORDER_SEQ { get; set; }
    public string PAY_TYPE { get; set; }
    public string PAY_OBJECT { get; set; }
    public string IS_SALARY { get; set; }
    public string IS_RATE { get; set; }
    public string IS_OVERTIME { get; set; }
    public string IS_LEAVE { get; set; }
    public string INS_A { get; set; }
    public string INS_B { get; set; }
    public string INS_C { get; set; }
    public string INS_D { get; set; }
    public string IS_ARREARS { get; set; }
    public string IS_BOUNS { get; set; }
    public string IS_RETAIR { get; set; }
    public string FORMULA { get; set; }
    public string IS_DISABLE { get; set; }
    public string IS_PREMINUS { get; set; }//是否代扣
    public string IS_PAY_LEAVE { get; set; }//是否計算特休
    public string IS_CAL_OVERTIME { get; set; }//是否計算加班
    //for查詢欄位
    public string ddl_SYS_CD { get; set; }


    public DataTable getCommCode(string sys_cd, string main_cd, string is_valid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select sub_cd ,sub_cd+'-'+sub_desc as sub_desc ");
            sb.Append(" from TB_9_M_COMM_D ");
            sb.Append(" where SYS_CD = @SYS_CD ");
            sb.Append(" and MAIN_CD = @MAIN_CD ");
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@MAIN_CD", main_cd);
            if (is_valid != "")
            {
                sb.Append(" and IS_VALID = @IS_VALID ");
                ht.Add("@IS_VALID", is_valid);
            }

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    #region Qry
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string salary_id, string salary_name,
                             string is_disable, string salary_cd, string is_plus)
    {
        try
        {
            if (sortExpression.Contains("ORDER_SEQ"))
                sortExpression = sortExpression.Replace("ORDER_SEQ", "t1.ORDER_SEQ");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine(" t1.SALARY_ID,t1.SALARY_NAME,t1.SALARY_CD,t1.SALARY_CD +'-'+ d.SUB_DESC as SALARY_CD_DESC ");
            sb.AppendLine(" ,t1.TAX_FORMAT,t1.TAX_FORMAT +'-'+ f.SUB_DESC as TAX_FORMAT_DESC ");
            sb.AppendLine(" ,case when t1.IS_SALARY ='Y' then 'V' else '' end as IS_SALARY ");
            sb.AppendLine(" ,case when t1.IS_OVERTIME ='Y' then 'V' else '' end as IS_OVERTIME ");
            sb.AppendLine(" ,case when t1.IS_LEAVE ='Y' then 'V' else '' end as IS_LEAVE ");
            sb.AppendLine(" ,case when t1.INS_A ='Y' then 'V' else '' end as INS_A  ");
            sb.AppendLine(" ,case when t1.INS_B ='Y' then 'V' else '' end as INS_B ");
            sb.AppendLine(" ,case when t1.INS_C ='Y' then 'V' else '' end as INS_C  ");
            sb.AppendLine(" ,case when t1.INS_D ='Y' then 'V' else '' end as INS_D ");
            sb.AppendLine(" ,case when t1.IS_ARREARS ='Y' then 'V' else '' end as IS_ARREARS ");
            sb.AppendLine(" ,case when t1.IS_RETAIR ='Y' then 'V' else '' end as IS_RETAIR ");
            sb.AppendLine(" ,case when t1.IS_RATE ='Y' then 'V' else '' end as IS_RATE ");
            sb.AppendLine(" ,case when t1.IS_PLUS ='1' then '加項' when t1.IS_PLUS ='-1' then '減項' else '' end as IS_PLUS ");
            sb.AppendLine(" ,t1.IS_TAX,t1.ORDER_SEQ ");
            //20161220  TERRY ADD
            sb.AppendLine(" ,case when t1.IS_PREMINUS ='Y' then 'V' else '' end as IS_PREMINUS ");
            //END
            //20170628  TERRY ADD
            sb.AppendLine(" ,case when t1.IS_PAY_LEAVE ='Y' then 'V' else '' end as IS_PAY_LEAVE, case when t1.IS_CAL_OVERTIME ='Y' then 'V' else '' end as IS_CAL_OVERTIME ");
            //END
            sb.AppendLine(" from TB_S_M_SALARY_ITEM t1 ");
            sb.AppendLine(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_CD' and  t1.SALARY_CD = d.SUB_CD ");
            sb.AppendLine(" left join TB_9_M_COMM_D f on  f.SYS_CD ='SC' and  f.MAIN_CD='TAX_FORMAT' and  t1.TAX_FORMAT = f.SUB_CD ");
            sb.AppendLine(" where 1=1 ");

            if (salary_id != "")
            {
                sb.AppendLine(" and t1.SALARY_ID = @SALARY_ID ");
                ht.Add("@SALARY_ID", salary_id);
            }
            if (salary_name != "")
            {
                sb.AppendLine(" and t1.SALARY_NAME like '%'+ @SALARY_NAME +'%' ");
                ht.Add("@SALARY_NAME", salary_name);
            }
            if (is_disable != "")
            {
                sb.AppendLine(" and t1.IS_DISABLE = @IS_DISABLE ");
                ht.Add("@IS_DISABLE", is_disable);
            }
            if (salary_cd != "")
            {
                sb.AppendLine(" and t1.SALARY_CD = @SALARY_CD ");
                ht.Add("@SALARY_CD", salary_cd);
            }
            if (is_plus != "")
            {
                sb.AppendLine(" and t1.IS_PLUS = @IS_PLUS ");
                ht.Add("@IS_PLUS", is_plus);
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
    public int getCount(int startRowIndex, int maximumRows, string salary_id, string salary_name,
                        string is_disable, string salary_cd, string is_plus)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine(" from TB_S_M_SALARY_ITEM");
            sb.AppendLine(" where 1=1");

            if (salary_id != "")
            {
                sb.AppendLine(" and SALARY_ID = @SALARY_ID ");
                ht.Add("@SALARY_ID", salary_id);
            }
            if (salary_name != "")
            {
                sb.AppendLine(" and SALARY_NAME like '%'+ @SALARY_NAME +'%' ");
                ht.Add("@SALARY_NAME", salary_name);
            }
            if (is_disable != "")
            {
                sb.AppendLine(" and IS_DISABLE = @IS_DISABLE ");
                ht.Add("@IS_DISABLE", is_disable);
            }
            if (salary_cd != "")
            {
                sb.AppendLine(" and SALARY_CD = @SALARY_CD ");
                ht.Add("@SALARY_CD", salary_cd);
            }
            if (is_plus != "")
            {
                sb.AppendLine(" and IS_PLUS = @IS_PLUS ");
                ht.Add("@IS_PLUS", is_plus);
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

    public DataTable checkdeleteExistData(string salary_id)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select * from TB_S_M_SALARY_PAY where SALARY_ID = @SALARY_ID ");
        ht.Add("@SALARY_ID", salary_id);
        return dbConn.Query(sb, ht);
    }
    public string deleteData(string salary_id)
    {
        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //寫log
        sb.AppendLine(" update TB_S_M_SALARY_ITEM set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SC110' ");
        sb.AppendLine(" where SALARY_ID = @SALARY_ID; ");

        sb.AppendLine(" Delete from TB_S_M_SALARY_ITEM   ");
        sb.AppendLine(" where SALARY_ID = @SALARY_ID; ");
        ht.Add("@SALARY_ID", salary_id);
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
            sb.AppendLine("Select * from TB_S_M_SALARY_ITEM where SALARY_ID = @SALARY_ID ");
            ht.Add("@SALARY_ID", SALARY_ID);
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

            sb.AppendLine("insert into TB_S_M_SALARY_ITEM ");
            sb.AppendLine(" (SALARY_ID,SALARY_NAME,SALARY_CD,IS_PLUS,IS_TAX,TAX_FORMAT,PAY_TYPE,PAY_OBJECT, ");
            sb.AppendLine(" ORDER_SEQ,IS_SALARY,IS_RATE,IS_OVERTIME,IS_LEAVE,INS_A,INS_B,INS_C,INS_D, ");
            sb.AppendLine(" IS_ARREARS,IS_BOUNS,IS_RETAIR,FORMULA,IS_DISABLE,IS_PREMINUS,IS_PAY_LEAVE , IS_CAL_OVERTIME , CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine(" values (@SALARY_ID,@SALARY_NAME,@SALARY_CD,@IS_PLUS,@IS_TAX,@TAX_FORMAT,@PAY_TYPE,@PAY_OBJECT, ");
            sb.AppendLine(" @ORDER_SEQ,@IS_SALARY,@IS_RATE,@IS_OVERTIME,@IS_LEAVE,@INS_A,@INS_B,@INS_C,@INS_D, ");
            sb.AppendLine(" @IS_ARREARS,@IS_BOUNS,@IS_RETAIR,@FORMULA,@IS_DISABLE,@IS_PREMINUS,@IS_PAY_LEAVE ,@IS_CAL_OVERTIME  , @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@SALARY_NAME", SALARY_NAME);
            ht.Add("@SALARY_CD", SALARY_CD);
            ht.Add("@IS_PLUS", IS_PLUS);
            ht.Add("@IS_TAX", IS_TAX);
            ht.Add("@TAX_FORMAT", TAX_FORMAT);
            ht.Add("@PAY_TYPE", PAY_TYPE);
            ht.Add("@PAY_OBJECT", PAY_OBJECT);
            ht.Add("@ORDER_SEQ", ORDER_SEQ);
            ht.Add("@IS_SALARY", IS_SALARY);
            ht.Add("@IS_RATE", IS_RATE);
            ht.Add("@IS_OVERTIME", IS_OVERTIME);
            ht.Add("@IS_LEAVE", IS_LEAVE);
            ht.Add("@INS_A", INS_A);
            ht.Add("@INS_B", INS_B);
            ht.Add("@INS_C", INS_C);
            ht.Add("@INS_D", INS_D);
            ht.Add("@IS_ARREARS", IS_ARREARS);
            ht.Add("@IS_BOUNS", IS_BOUNS);
            ht.Add("@IS_RETAIR", IS_RETAIR);
            ht.Add("@FORMULA", FORMULA);
            ht.Add("@IS_DISABLE", IS_DISABLE);
            ht.Add("@IS_PREMINUS", IS_PREMINUS);
            ht.Add("@IS_PAY_LEAVE", IS_PAY_LEAVE);
            ht.Add("@IS_CAL_OVERTIME", IS_CAL_OVERTIME);

            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC110");

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

            sb.AppendLine("Update TB_S_M_SALARY_ITEM ");
            sb.AppendLine(" Set SALARY_NAME = @SALARY_NAME,SALARY_CD = @SALARY_CD,IS_PLUS = @IS_PLUS,IS_TAX = @IS_TAX,TAX_FORMAT = @TAX_FORMAT, ");
            sb.AppendLine(" PAY_TYPE = @PAY_TYPE,PAY_OBJECT = @PAY_OBJECT,ORDER_SEQ = @ORDER_SEQ,IS_SALARY = @IS_SALARY,IS_RATE = @IS_RATE, ");
            sb.AppendLine(" IS_OVERTIME = @IS_OVERTIME,IS_LEAVE = @IS_LEAVE,INS_A = @INS_A,INS_B = @INS_B,INS_C = @INS_C,INS_D = @INS_D, ");
            sb.AppendLine(" IS_ARREARS =@IS_ARREARS,IS_BOUNS = @IS_BOUNS,IS_RETAIR = @IS_RETAIR,FORMULA = @FORMULA,IS_PREMINUS = @IS_PREMINUS, ");//ADD
            sb.AppendLine(" IS_PAY_LEAVE = @IS_PAY_LEAVE,IS_CAL_OVERTIME = @IS_CAL_OVERTIME,IS_DISABLE = @IS_DISABLE,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where SALARY_ID = @SALARY_ID");
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@SALARY_NAME", SALARY_NAME);
            ht.Add("@SALARY_CD", SALARY_CD);
            ht.Add("@IS_PLUS", IS_PLUS);
            ht.Add("@IS_TAX", IS_TAX);
            ht.Add("@TAX_FORMAT", TAX_FORMAT);
            ht.Add("@PAY_TYPE", PAY_TYPE);
            ht.Add("@PAY_OBJECT", PAY_OBJECT);
            ht.Add("@ORDER_SEQ", ORDER_SEQ);
            ht.Add("@IS_SALARY", IS_SALARY);
            ht.Add("@IS_RATE", IS_RATE);
            ht.Add("@IS_OVERTIME", IS_OVERTIME);
            ht.Add("@IS_LEAVE", IS_LEAVE);
            ht.Add("@INS_A", INS_A);
            ht.Add("@INS_B", INS_B);
            ht.Add("@INS_C", INS_C);
            ht.Add("@INS_D", INS_D);
            ht.Add("@IS_ARREARS", IS_ARREARS);
            ht.Add("@IS_BOUNS", IS_BOUNS);
            ht.Add("@IS_RETAIR", IS_RETAIR);
            ht.Add("@FORMULA", FORMULA);
            ht.Add("@IS_DISABLE", IS_DISABLE);
            ht.Add("@IS_PREMINUS", IS_PREMINUS);
            ht.Add("@IS_PAY_LEAVE", IS_PAY_LEAVE);
            ht.Add("@IS_CAL_OVERTIME", IS_CAL_OVERTIME);

            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC110");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得修改資料
    public DataTable getModData(string salary_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * ");
            sb.AppendLine(" from TB_S_M_SALARY_ITEM ");
            sb.AppendLine(" where SALARY_ID = @SALARY_ID ");
            ht.Add("@SALARY_ID", salary_id);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }

    }
    #endregion
}