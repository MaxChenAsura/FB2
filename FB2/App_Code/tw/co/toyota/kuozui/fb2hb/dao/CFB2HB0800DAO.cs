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
/// WFB2HB0800 的摘要描述
/// </summary>
public class CFB2HB0800DAO : BaseDAO
{

    public string EMP_ID { get; set; }
    public string LANGUAGE_JAPANESE { get; set; }
    public string LANGUAGE_TOEIC { get; set; }
    public string GRP_NAME { get; set; }
    public string REDEPLOY_YN { get; set; }
    public string REPORT_TYPE { get; set; }
    public string IS_CTL { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    //Dtl
    public string LEVEL_CD { get; set; }



    public CFB2HB0800DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "A.EMP_ID");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" A.EMP_ID,B.EMP_NAME,A.LANGUAGE_JAPANESE, A.LANGUAGE_TOEIC, C.EMP_STATUS ");
            sb.Append(" from TB_H_R_EMP_LANGUAGE A with (nolock) LEFT JOIN ");
            sb.Append("         TB_H_M_EMP  B with (nolock) ON A.EMP_ID=B.EMP_ID LEFT JOIN ");
            sb.Append("         TB_H_R_EMP_DATA  C with (nolock) ON A.EMP_ID=C.EMP_ID ");
             sb.Append(" where 1=1");

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

    public int getCount(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_H_R_EMP_LANGUAGE  A ");
            sb.Append(" where 1=1");
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
    //gv_result新刪修
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_H_R_EMP_LANGUAGE ");
            sb.Append(" where EMP_ID = @EMP_ID  ");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //取得修改資料
    public DataTable getUpdData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select  A.EMP_ID, B.EMP_NAME, A.LANGUAGE_JAPANESE, A.LANGUAGE_TOEIC ");
            sb.Append(" from TB_H_R_EMP_LANGUAGE A with (nolock)  LEFT JOIN ");
            sb.Append("         TB_H_M_EMP B ON A.EMP_ID=B.EMP_ID ");
           
            sb.Append(@" 
                where 1=1
                and A.EMP_ID =@EMP_ID
            ");

            ht.Add("@EMP_ID", this.EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    

    //新增 TB_S_M_ASSESS_GROUP_H
    public void addEmpLanguage()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_H_R_EMP_LANGUAGE ( ");
            sb.Append(" EMP_ID,LANGUAGE_JAPANESE, LANGUAGE_TOEIC)");
            sb.Append(" values ( ");
            sb.Append(" @EMP_ID,@LANGUAGE_JAPANESE, @LANGUAGE_TOEIC)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LANGUAGE_JAPANESE", LANGUAGE_JAPANESE);
            ht.Add("@LANGUAGE_TOEIC", LANGUAGE_TOEIC);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //更新 TB_S_M_ASSESS_GROUP_H
    public void updateEmpLanguage()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_H_R_EMP_LANGUAGE ");
            sb.Append(" set LANGUAGE_JAPANESE = @LANGUAGE_JAPANESE,LANGUAGE_TOEIC = @LANGUAGE_TOEIC");
            sb.Append(" where EMP_ID = @EMP_ID ");

            ht.Add("@LANGUAGE_JAPANESE", LANGUAGE_JAPANESE);
            ht.Add("@LANGUAGE_TOEIC", LANGUAGE_TOEIC);
            ht.Add("@EMP_ID", EMP_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    

    public void Delete(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Delete From TB_H_R_EMP_LANGUAGE ");
            sb.AppendLine(" where EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    
}