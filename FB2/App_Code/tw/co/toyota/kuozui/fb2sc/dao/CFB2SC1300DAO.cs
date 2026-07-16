using System;
using System.Collections;
using System.Data;
using System.Text;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2SC1300DAO 的摘要描述
/// </summary>
public class CFB2SC1300DAO : BaseDAO
{
    public string SALARY_TYPE { get; set; }

    public string OPERATION_ID { get; set; }

    public string OPERATION_NAME { get; set; }

    public string SALARY_REQ { get; set; }

    public string UPDATED_BY { get; set; }

    public string FUNC_ID { get; set; }
    public CFB2SC1300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string operation_id, string operation_name)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //sb.Append(" Select * From (");
            sb.Append(" Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber, ");
            sb.Append(" t.SALARY_TYPE,e.SUB_DESC as SALARY_NAME,t.OPERATION_ID,t.OPERATION_NAME,t.SALARY_REQ,concat(t.PROC_SOUCE,'.'+d.SUB_DESC) as PROC_SOUCE ");
            sb.Append(" from TB_S_M_SALARY_CTRL t ");
            sb.Append(" left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and e.MAIN_CD='SALARY_TYPE' and  t.SALARY_TYPE = e.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and d.MAIN_CD='PROC_SOUCE' and  t.PROC_SOUCE = d.SUB_CD ");
            sb.Append(" where 1=1 ");

            if (operation_id != "")
            {
                sb.Append(" and t.OPERATION_ID like @OPERATION_ID ");
                ht.Add("@OPERATION_ID", operation_id + "%");
            }
            if (operation_name != "")
            {
                sb.Append(" and t.OPERATION_NAME like @OPERATION_NAME ");
                ht.Add("@OPERATION_NAME", operation_name + "%");
            }
            //sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            //sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

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
    public int getCount(int startRowIndex, int maximumRows, string operation_id, string operation_name)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_SALARY_CTRL t ");
            sb.Append(" left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and e.MAIN_CD='SALARY_TYPE' and  t.SALARY_TYPE = e.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and d.MAIN_CD='PROC_SOUCE' and  t.PROC_SOUCE = d.SUB_CD ");
            sb.Append(" where 1=1 ");

            if (operation_id != "")
            {
                sb.Append(" and t.OPERATION_ID like @OPERATION_ID ");
                ht.Add("@OPERATION_ID", operation_id + "%");
            }
            if (operation_name != "")
            {
                sb.Append(" and t.OPERATION_NAME like @OPERATION_NAME ");
                ht.Add("@OPERATION_NAME", operation_name + "%");
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



    internal void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_SALARY_CTRL set OPERATION_NAME = @OPERATION_NAME,SALARY_REQ = @SALARY_REQ, ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" where SALARY_TYPE = @SALARY_TYPE and OPERATION_ID = @OPERATION_ID");

            ht.Add("@OPERATION_NAME", OPERATION_NAME);
            ht.Add("@SALARY_REQ", SALARY_REQ);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@OPERATION_ID", OPERATION_ID);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
}