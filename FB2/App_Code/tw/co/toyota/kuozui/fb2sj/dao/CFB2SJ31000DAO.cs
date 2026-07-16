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
/// WFB2SJ3100 的摘要描述
/// </summary>
public class CFB2SJ3100DAO : BaseDAO
{
    ///<summary>
    ///考核區分代碼
    ///</summary>
    public string DISTING_CD { get; set; }

    ///<summary>
    ///考核區分說明
    ///</summary>
    public string DISTING_DESC { get; set; }

    ///<summary>
    ///是否為外數區分
    ///</summary>
    public string IS_OUT { get; set; }

    ///<summary>
    ///備考對象
    ///</summary>
    public string IS_REMARK { get; set; }

    ///<summary>
    ///是否生效
    ///</summary>
    public string IS_VALID { get; set; }

    ///<summary>
    ///備考內容(顯示用)
    ///</summary>
    public string CONTENT { get; set; }

    ///<summary>
    ///備註
    ///</summary>
    public string REMARK { get; set; }
    public string USER_UP_YN { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SJ3100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string disting_cd, string disting_desc)
    {
        try
        {
            if (sortExpression.Contains("DISTING_CD"))
                sortExpression = sortExpression.Replace("DISTING_CD", "A.DISTING_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(@" A.DISTING_CD,A.DISTING_DESC,A.IS_OUT,A.IS_REMARK,A.IS_VALID,A.CONTENT,A.REMARK,A.USER_UP_YN
                                from TB_S_M_FOREIGN_DISTING A with (nolock) ");
            sb.Append(" where 1=1 ");

            if (disting_cd != "")
            {
                sb.Append(" and DISTING_CD  LIKE '%'+@DISTING_CD+'%' ");
                ht.Add("@DISTING_CD", disting_cd);
            }

            if (disting_desc != "")
            {
                sb.Append(" and DISTING_DESC LIKE '%'+@DISTING_DESC+'%' ");
                ht.Add("@DISTING_DESC", disting_desc);
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

    public int getCount(int startRowIndex, int maximumRows, string disting_cd, string disting_desc)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record from TB_S_M_FOREIGN_DISTING A ");
            sb.Append(" where 1=1 ");

            if (disting_cd != "")
            {
                sb.Append(" and DISTING_CD  LIKE '%'+@DISTING_CD+'%' ");
                ht.Add("@DISTING_CD", disting_cd);
            }

            if (disting_desc != "")
            {
                sb.Append(" and DISTING_DESC LIKE '%'+@DISTING_DESC+'%' ");
                ht.Add("@DISTING_DESC", disting_desc);
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

    //取得現有資料
    public DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select * from TB_S_M_FOREIGN_DISTING");
            sb.Append(" where DISTING_CD = @DISTING_CD   ");
            ht.Add("@DISTING_CD", DISTING_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
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
            sb.Append(@" Select 
                            A.DISTING_CD,A.DISTING_DESC,A.IS_OUT,A.IS_REMARK,A.IS_VALID,A.CONTENT,A.REMARK,A.USER_UP_YN
                         from TB_S_M_FOREIGN_DISTING A with (nolock) ");
            
           
            sb.Append(@" 
                where 1=1 AND      DISTING_CD = @DISTING_CD 
            ");

            ht.Add("@DISTING_CD", DISTING_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增 TB_S_M_FOREIGN_DISTING
    public void addITEM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_S_M_FOREIGN_DISTING ( ");
            sb.Append(" DISTING_CD,DISTING_DESC,IS_OUT,IS_REMARK,IS_VALID,");
            sb.Append(" CONTENT,REMARK,USER_UP_YN ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" ,@DISTING_CD,@DISTING_DESC,@IS_OUT,@IS_REMARK,@IS_VALID,");
            sb.Append("@CONTENT,@REMARK,@USER_UP_YN ");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@DISTING_CD", DISTING_CD);//VARCHAR
            ht.Add("@DISTING_DESC", DISTING_DESC);//NVARCHAR
            ht.Add("@IS_OUT", IS_OUT);//VARCHAR
            ht.Add("@IS_REMARK", IS_REMARK);//VARCHAR
            ht.Add("@IS_VALID", IS_VALID);//VARCHAR
            ht.Add("@CONTENT", CONTENT);//NVARCHAR
            ht.Add("@REMARK", REMARK);//NVARCHAR
            ht.Add("@USER_UP_YN", USER_UP_YN);//VARCHAR


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

    //更新 TB_S_M_FOREIGN_DISTING
    public void updateITEM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_FOREIGN_DISTING ");
            sb.Append(" set DISTING_DESC = @DISTING_DESC, IS_OUT = @IS_OUT, IS_REMARK = @IS_REMARK,IS_VALID = @IS_VALID, ");
            sb.Append(" CONTENT = @CONTENT, REMARK = @REMARK, USER_UP_YN = @USER_UP_YN ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where DISTING_CD = @DISTING_CD  ");

            
            ht.Add("@DISTING_DESC", DISTING_DESC);//NVARCHAR
            ht.Add("@IS_OUT", IS_OUT);//VARCHAR
            ht.Add("@IS_REMARK", IS_REMARK);//VARCHAR
            ht.Add("@IS_VALID", IS_VALID);//VARCHAR
            ht.Add("@CONTENT", CONTENT);//NVARCHAR
            ht.Add("@REMARK", REMARK);//NVARCHAR
            ht.Add("@USER_UP_YN", USER_UP_YN);//VARCHAR

            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@DISTING_CD", DISTING_CD);//VARCHAR

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除 TB_S_M_FOREIGN_DISTING
    public void deleteITEM(string disting_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.AppendLine(" update TB_S_M_FOREIGN_DISTING set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SJ3100' ");
            sb.Append(" where DISTING_CD = @DISTING_CD  ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            ht.Add("@DISTING_CD", disting_cd);

            sb.Append(" delete from TB_S_M_FOREIGN_DISTING ");
            sb.Append("  where DISTING_CD = @DISTING_CD  ");
            //ht.Add("@ASSESS_TYPE", assess_type);
            //ht.Add("@WS_CD", ws_cd);
            //ht.Add("@LEVEL_CD", level_cd);
            //ht.Add("@PJOB_TYPE", pjob_cd);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}