using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// WFB2SJ3200 的摘要描述
/// </summary>
public class CFB2SJ3200DAO : BaseDAO
{
    

    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string EMP_ID { get; set; }
    public string DISTING_CD { get; set; }
    public string DATASOURCE { get; set; }
    public string REMARK { get; set; }
    public string ABS_SCORE { get; set; }
    public string CHG_WS_CD { get; set; }
    public string EXCEPT_E { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2SJ3200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY A.ASSESS_YEAR,A.ASSESS_TYPE,A.EMP_ID ) As RowNumber,");
            sb.Append(@" A.ASSESS_YEAR , A.ASSESS_TYPE ,  A.ASSESS_TYPE+'-'+D.SUB_DESC as ASSESS_TYPE_DESC, A.EMP_ID ,E.EMP_NAME,  
                         A.DISTING_CD ,A.DISTING_CD+'-'+B.DISTING_DESC as DISTING_CD_DESC ,
                         A.DATASOURCE , A.REMARK , 
                         A.ABS_SCORE , A.EXCEPT_E
                         from TB_S_M_FOREIGN_DISTING_EMP A with (nolock)
                         left join TB_S_M_ASSESS_DISTING B  with (nolock)  on B.DISTING_CD= A.DISTING_CD 
                         left join TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='SJ' and D.MAIN_CD='ASSESS_TYPE' and D.SUB_CD= A.ASSESS_TYPE and D.IS_VALID='Y' 
                         left join TB_H_M_EMP E   with (nolock)  on A.EMP_ID=E.EMP_ID ");
            sb.Append(" where 1=1 ");
          
            if (assess_year != "")
            {
                sb.Append(" and A.ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", assess_year);
            }
            if (assess_type != "-1")
            {
                sb.Append(" and A.ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
            }

            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID ");
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

    public int getCount(int startRowIndex, int maximumRows, string assess_year, string assess_type, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record from TB_S_M_FOREIGN_DISTING_EMP A ");
            sb.Append(" where 1=1 ");
           
            if (assess_year != "")
            {
                sb.Append(" and A.ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", assess_year);
            }
           
            if (assess_type  != "-1")
            {
                sb.Append(" and A.ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
            }

            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID ");
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

    public int isExit(string assess_year, string assess_type, string emp_id,String disting_cd,String datasource)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record from TB_S_M_FOREIGN_DISTING_EMP ");
            sb.Append(" where 1=1 ");
            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            sb.Append(" and EMP_ID = @EMP_ID ");
            sb.Append(" and DISTING_CD = @DISTING_CD ");
            if (datasource != "")
            {
                sb.Append(" and DataSource = @DATASOURCE ");
                ht.Add("@DATASOURCE", datasource);
            }
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@DISTING_CD", disting_cd);

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
    //確認工號存在
    public int checkEMPID( string empId)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select count(*) resultCount from VW_H_EMP_DATA
                        where 1=1 and EMP_ID =@EMP_ID and EMP_CHG_CD in('11','12','14','21')
                        ");

            ht.Add("@EMP_ID", empId);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }
            return t;

        }
        catch (Exception)
        {
            throw;
        }
    }
    //刪除 
    public void deleteData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" delete from TB_S_M_FOREIGN_DISTING_EMP 
                        where ASSESS_YEAR = @ASSESS_YEAR 
                        and ASSESS_TYPE = @ASSESS_TYPE
                        and EMP_ID = @EMP_ID 
                        and DISTING_CD = @DISTING_CD
                        and DATASOURCE = @DATASOURCE
                        ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DISTING_CD", DISTING_CD);
            ht.Add("@DATASOURCE", DATASOURCE);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //刪除 全部
    public void deleteAllData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" delete from TB_S_M_FOREIGN_DISTING_EMP 
                        where ASSESS_YEAR = @ASSESS_YEAR 
                        and ASSESS_TYPE = @ASSESS_TYPE
                        and DATASOURCE = @DATASOURCE
                        ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DATASOURCE", DATASOURCE);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增
    internal void insertData(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_M_FOREIGN_DISTING_EMP (ASSESS_YEAR , ASSESS_TYPE , EMP_ID , DISTING_CD, ");
            sb.Append(" DATASOURCE , REMARK , ABS_SCORE , EXCEPT_E,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@ASSESS_YEAR , @ASSESS_TYPE , @EMP_ID , @DISTING_CD , ");
            sb.Append(" @DATASOURCE , @REMARK , @ABS_SCORE , @EXCEPT_E, ");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DISTING_CD", DISTING_CD);
            ht.Add("@DATASOURCE", DATASOURCE);
            ht.Add("@REMARK", REMARK);
            ht.Add("@ABS_SCORE", ABS_SCORE);
            ht.Add("@EXCEPT_E", EXCEPT_E);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@CREATED_DT", now);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新 TB_S_M_FOREIGN_DISTING_EMP
    public void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_FOREIGN_DISTING_EMP ");
            sb.Append(" set DATASOURCE = @DATASOURCE,  ");
            sb.Append("     REMARK = @REMARK ,ABS_SCORE = @ABS_SCORE ,EXCEPT_E = @EXCEPT_E ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where  ASSESS_YEAR = @ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE  and EMP_ID = @EMP_ID  and DISTING_CD = @DISTING_CD ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DISTING_CD", DISTING_CD);
            ht.Add("@DATASOURCE", DATASOURCE);
            ht.Add("@REMARK", REMARK);
            ht.Add("@ABS_SCORE", ABS_SCORE);
            ht.Add("@EXCEPT_E", EXCEPT_E);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Add(string assess_year, string assess_type, string data_source,string cell1, string cell2, string cell3, 
        string cell4, string cell5, string cell6, string cell7)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_M_FOREIGN_DISTING_EMP (ASSESS_YEAR , ASSESS_TYPE , EMP_ID , DISTING_CD, ");
            sb.Append(" DATASOURCE , REMARK , ABS_SCORE , EXCEPT_E,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@ASSESS_YEAR , @ASSESS_TYPE , @EMP_ID , @DISTING_CD , ");
            sb.Append(" @DATASOURCE , @REMARK , @ABS_SCORE , @EXCEPT_E, ");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            if (cell1.Trim() == "")
                ht.Add("@EMP_ID", DBNull.Value);
            else
                ht.Add("@EMP_ID", cell1.Trim());
            if (cell3.Trim() == "")
                ht.Add("@DISTING_CD", DBNull.Value);
            else
                ht.Add("@DISTING_CD", cell3.Trim());
            ht.Add("@DATASOURCE", data_source);
            if (cell4.Trim() == "")
                ht.Add("@REMARK", DBNull.Value);
            else
                ht.Add("@REMARK", cell4.Trim());
            if (cell5.Trim() == "")
                ht.Add("@ABS_SCORE", DBNull.Value);
            else
                ht.Add("@ABS_SCORE", cell5.Trim());
            
            if (cell6.Trim() == "")
                ht.Add("@EXCEPT_E", DBNull.Value);
            else
                ht.Add("@EXCEPT_E", cell6.Trim());
            
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SJ3200");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    
}