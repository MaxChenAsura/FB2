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
/// WFB2SJ3300 的摘要描述
/// </summary>
public class CFB2SJ3300DAO : BaseDAO
{
    public string ASSESS_TYPE { get; set; }
    public int RATE_A { get; set; }
    public int RATE_B { get; set; }
    public int RATE_C { get; set; }
    public int RATE_D { get; set; }
    public int RATE_E { get; set; }
    public int RATE_F { get; set; }
    public int RATE_G { get; set; }
    public string IS_CTL { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SJ3300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string assess_type)
    {
        try
        {
            if (sortExpression.Contains("ASSESS_TYPE"))
                sortExpression = sortExpression.Replace("ASSESS_TYPE", "A.ASSESS_TYPE");

         

           
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(@" A.ASSESS_TYPE, A.ASSESS_TYPE+'-'+D.SUB_DESC as ASSESS_TYPE_DESC,  A.WS_CD,
                         A.RATE_A, A.RATE_B, A.RATE_C, A.RATE_D, A.RATE_E ,ISNULL(A.IS_CTL,'Y') IS_CTL, CASE WHEN ISNULL(A.IS_CTL,'Y') ='Y' THEN 'Y' ELSE 'N' END AS IS_CTL_DESC
                         from TB_S_M_FOREIGN_RATE A left join ");
            sb.Append("       TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='SJ' and D.MAIN_CD='ASSESS_TYPE' and D.SUB_CD= A.ASSESS_TYPE and D.IS_VALID='Y'  ");
            sb.Append(" where 1=1 ");

            if (assess_type != "-1")
            {
                sb.Append(" and ASSESS_TYPE =@ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
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

    public int getCount(int startRowIndex, int maximumRows, string assess_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record from TB_S_M_FOREIGN_RATE A ");
            sb.Append(" where 1=1 ");

            if (assess_type != "-1")
            {
                sb.Append(" and A.ASSESS_TYPE =@ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
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
            sb.Append("select ASSESS_TYPE from TB_S_M_FOREIGN_RATE ");
            sb.Append(" where ASSESS_TYPE = @ASSESS_TYPE  ");
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);

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
             A.ASSESS_TYPE
            ,A.ASSESS_TYPE+'-'+D.SUB_DESC as ASSESS_TYPE_DESC
            ,A.RATE_A
            ,A.RATE_B
            ,A.RATE_C
            ,A.RATE_D
            ,A.RATE_E
            ,A.RATE_F
            ,A.RATE_G
            ,ISNULL(A.IS_CTL,'Y') IS_CTL
            ");
            sb.Append(@" from TB_S_M_FOREIGN_RATE A    left join 
                              TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='SJ' and D.MAIN_CD='ASSESS_TYPE' and D.SUB_CD= A.ASSESS_TYPE and D.IS_VALID='Y'  ");
            sb.Append(" where ASSESS_TYPE = @ASSESS_TYPE  and LEVEL_CD = @LEVEL_CD ");
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增 TB_S_M_FOREIGN_RATE
    public void addRATE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_S_M_FOREIGN_RATE( ");
            sb.Append(" ASSESS_TYPE , RATE_A , RATE_B , RATE_C , RATE_D , RATE_E , RATE_F , RATE_G ,IS_CTL ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @ASSESS_TYPE,@RATE_A,@RATE_B,@RATE_C,@RATE_D,@RATE_E,@RATE_F,@RATE_G,@IS_CTL ");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@RATE_A", RATE_A);
            ht.Add("@RATE_B", RATE_B);
            ht.Add("@RATE_C", RATE_C);
            ht.Add("@RATE_D", RATE_D);
            ht.Add("@RATE_E", RATE_E);
            ht.Add("@RATE_F", RATE_F);
            ht.Add("@RATE_G", RATE_G);
            ht.Add("@IS_CTL", IS_CTL);


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

    //更新 TB_S_M_FOREIGN_RATE
    public void updateRATE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_FOREIGN_RATE ");
            sb.Append(" set RATE_A = @RATE_A ,RATE_B = @RATE_B ,RATE_C = @RATE_C ,RATE_D = @RATE_D ,RATE_E =@RATE_E , ");
            sb.Append("     RATE_F = @RATE_F ,RATE_G = @RATE_G, IS_CTL=@IS_CTL, UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where ASSESS_TYPE = @ASSESS_TYPE   ");

            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@RATE_A", RATE_A);
            ht.Add("@RATE_B", RATE_B);
            ht.Add("@RATE_C", RATE_C);
            ht.Add("@RATE_D", RATE_D);
            ht.Add("@RATE_E", RATE_E);
            ht.Add("@RATE_F", RATE_F);
            ht.Add("@RATE_G", RATE_G);
            ht.Add("@IS_CTL", IS_CTL);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除 TB_S_M_FOREIGN_RATE
    public void deleteRATE(string assess_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.AppendLine(" update TB_S_M_FOREIGN_RATE set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SJ3300' ");
            sb.Append(" where ASSESS_TYPE = @ASSESS_TYPE   ;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_S_M_FOREIGN_RATE ");
            sb.Append(" where ASSESS_TYPE = @ASSESS_TYPE  ;");
            ht.Add("@ASSESS_TYPE", assess_type);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}