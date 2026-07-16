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
/// WFB2SJ0110 的摘要描述
/// </summary>
public class CFB2SJ0110DAO : BaseDAO
{
    public string ASSESS_TYPE { get; set; }
    public string WS_CD { get; set; }
    public string LEVEL_CD { get; set; }
    public string PJOB_TYPE { get; set; }
    public string ITEM_GROUP { get; set; }

    public string USER_UP_YN { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SJ0110DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string assess_type, string ws_cd)
    {
        try
        {
            if (sortExpression.Contains("ASSESS_TYPE"))
                sortExpression = sortExpression.Replace("ASSESS_TYPE", "A.ASSESS_TYPE");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(@" A.ASSESS_TYPE ,  A.ASSESS_TYPE+'-'+D.SUB_DESC as ASSESS_TYPE_DESC, 
                         A.WS_CD, A.WS_CD+'-'+C.SUB_DESC as WS_CD_DESC, A.LEVEL_CD, 
                         A.PJOB_TYPE, A.PJOB_TYPE+'-'+E.SUB_DESC as PJOB_TYPE_DESC,
                         A.ITEM_GROUP ,B.SUB_DESC as ITEM_GROUP_DESC 
                         from TB_S_M_ASSESS_ITEM A with (nolock)
                         left join TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='SJ' and B.MAIN_CD='CATEGORY' and B.SUB_CD= A.ITEM_GROUP and B.IS_VALID='Y'
                         left join TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= A.WS_CD and C.IS_VALID='Y'
                         left join TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='SJ' and D.MAIN_CD='ASSESS_TYPE' and D.SUB_CD= A.ASSESS_TYPE and D.IS_VALID='Y'
                         left join TB_9_M_COMM_D E  with (nolock)  on E.SYS_CD='SE' and E.MAIN_CD='PJOB_TYPE' and E.SUB_CD= A.PJOB_TYPE and D.IS_VALID='Y' ");
            sb.Append(" where 1=1 ");

            if (assess_type != "-1")
            {
                sb.Append(" and A.ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
            }

            if (ws_cd != "-1")
            {
                sb.Append(" and A.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
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

    public int getCount(int startRowIndex, int maximumRows, string assess_type, string ws_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record from TB_S_M_ASSESS_ITEM A ");
            sb.Append(" where 1=1 ");


            if (assess_type != "-1")
            {
                sb.Append(" and A.ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
            }

            if (ws_cd != "-1")
            {
                sb.Append(" and A.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
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
            sb.Append("select * from TB_S_M_ASSESS_ITEM");
            sb.Append(" where ASSESS_TYPE = @ASSESS_TYPE  and WS_CD=@WS_CD and LEVEL_CD=@LEVEL_CD and PJOB_TYPE=@PJOB_TYPE ");
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@PJOB_TYPE", PJOB_TYPE);

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
                         A.ASSESS_TYPE ,  A.ASSESS_TYPE+'-'+D.SUB_DESC as ASSESS_TYPE_DESC, 
                         A.WS_CD, A.WS_CD+'-'+C.SUB_DESC as WS_CD_DESC, A.LEVEL_CD, 
                         A.PJOB_TYPE, A.PJOB_TYPE+'-'+E.SUB_DESC as PJOB_TYPE_DESC,
                         A.ITEM_GROUP ,B.SUB_DESC as ITEM_GROUP_DESC 
                         from TB_S_M_ASSESS_ITEM A with (nolock)
                         left join TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='SJ' and B.MAIN_CD='CATEGORY' and B.SUB_CD= A.ITEM_GROUP and B.IS_VALID='Y'
                         left join TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= A.WS_CD and C.IS_VALID='Y'
                         left join TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='SJ' and D.MAIN_CD='ASSESS_TYPE' and D.SUB_CD= A.ASSESS_TYPE and D.IS_VALID='Y'
                         left join TB_9_M_COMM_D E  with (nolock)  on E.SYS_CD='SE' and E.MAIN_CD='PJOB_TYPE' and E.SUB_CD= A.PJOB_TYPE and D.IS_VALID='Y' ");
            
           
            sb.Append(@" 
                where 1=1
                and A.ASSESS_TYPE = @ASSESS_TYPE  and A.WS_CD=@WS_CD and A.LEVEL_CD=@LEVEL_CD  and A.PJOB_TYPE=@PJOB_TYPE
            ");

            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@PJOB_TYPE", PJOB_TYPE);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增 TB_S_M_ASSESS_ITEM
    public void addITEM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_S_M_ASSESS_ITEM ( ");
            sb.Append(" ASSESS_TYPE , WS_CD , LEVEL_CD , PJOB_TYPE , ITEM_GROUP");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @ASSESS_TYPE,@WS_CD,@LEVEL_CD,@PJOB_TYPE,@ITEM_GROUP");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@PJOB_TYPE", PJOB_TYPE);
            ht.Add("@ITEM_GROUP", ITEM_GROUP);

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

    //更新 TB_S_M_ASSESS_ITEM
    public void updateITEM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_ASSESS_ITEM ");
            sb.Append(" set ITEM_GROUP=@ITEM_GROUP,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where ASSESS_TYPE = @ASSESS_TYPE  and WS_CD=@WS_CD and LEVEL_CD=@LEVEL_CD and PJOB_TYPE=@PJOB_TYPE ");

            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@PJOB_TYPE", PJOB_TYPE);
            ht.Add("@ITEM_GROUP", ITEM_GROUP);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除 TB_S_M_ASSESS_ITEM
    public void deleteITEM(string assess_type, string ws_cd, string level_cd,string pjob_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.AppendLine(" update TB_S_M_ASSESS_ITEM set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SJ0110' ");
            sb.Append(" where ASSESS_TYPE = @ASSESS_TYPE  and WS_CD=@WS_CD and LEVEL_CD=@LEVEL_CD and PJOB_TYPE=@PJOB_TYPE");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@WS_CD", ws_cd);
            ht.Add("@LEVEL_CD", level_cd);
            ht.Add("@PJOB_TYPE", pjob_cd);

            sb.Append(" delete from TB_S_M_ASSESS_ITEM ");
            sb.Append(" where ASSESS_TYPE = @ASSESS_TYPE  and WS_CD=@WS_CD and LEVEL_CD=@LEVEL_CD and PJOB_TYPE=@PJOB_TYPE ");
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