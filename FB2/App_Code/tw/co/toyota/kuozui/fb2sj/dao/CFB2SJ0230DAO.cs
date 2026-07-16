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
/// WFB2SJ0230 的摘要描述
/// </summary>
public class CFB2SJ0230DAO : BaseDAO
{
    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string DEPT_NO_20 { get; set; }
    public string DEPT_NAME_20 { get; set; }
    public string WS_CD { get; set; }
    public string LEVEL_CD { get; set; }
    public string SCORE_LEVEL_GROUP { get; set; }
    public string IS_MERGER { get; set; }
    public int BASE_A { get; set; }
    public int BASE_B { get; set; }
    public int BASE_C { get; set; }
    public int BASE_D { get; set; }
    public int BASE_E { get; set; }
    public int BASE_TOT { get; set; }
    public int REAL_A { get; set; }
    public int REAL_B { get; set; }
    public int REAL_C { get; set; }
    public int REAL_D { get; set; }
    public int REAL_E { get; set; }
    public int REAL_TOTAL { get; set; }
    public string CHECK_OK { get; set; }
    public string CHECK_REMARK { get; set; }
    public int OUT_REAL_A { get; set; }
    public int OUT_REAL_B { get; set; }
    public int OUT_REAL_C { get; set; }
    public int OUT_REAL_D { get; set; }
    public int OUT_REAL_E { get; set; }
    public int OUT_REAL_TOTAL { get; set; }


    public string USER_UP_YN { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SJ0230DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, 
        string dept_no_20, string ws_cd, string score_level_group, string is_merger)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(@" A.ASSESS_YEAR, A.ASSESS_TYPE ,  A.ASSESS_TYPE+'-'+D.SUB_DESC as ASSESS_TYPE_DESC, 
                         A.DEPT_NO_20, A.DEPT_NAME_20, A.WS_CD, A.WS_CD+'-'+C.SUB_DESC as WS_CD_DESC, A.LEVEL_CD, 
                         A.SCORE_LEVEL_GROUP,
                         A.IS_MERGER ,B.SUB_DESC as IS_MERGER_DESC ,
                         case when A.WS_CD='G' then 0 else A.BASE_A end as BASE_A,
                         case when A.WS_CD='G' then 0 else A.BASE_B end as BASE_B,
                         case when A.WS_CD='G' then 0 else A.BASE_C end as BASE_C,
                         case when A.WS_CD='G' then 0 else A.BASE_D end as BASE_D,
                         case when A.WS_CD='G' then 0 else A.BASE_E end as BASE_E, A.BASE_TOT 
                         from TB_S_M_ASSESS_DEP20_PEO A with (nolock)
                         left join TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='SJ' and B.MAIN_CD='IS_MERGER' and B.SUB_CD= A.IS_MERGER and B.IS_VALID='Y'
                         left join TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= A.WS_CD and C.IS_VALID='Y'
                         left join TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='SJ' and D.MAIN_CD='ASSESS_TYPE' and D.SUB_CD= A.ASSESS_TYPE and D.IS_VALID='Y' ");
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

            if (dept_no_20 != "")
            {
                sb.Append(" and A.DEPT_NO_20 = @DEPT_NO_20 ");
                ht.Add("@DEPT_NO_20", dept_no_20);
            }

            if (ws_cd != "-1")
            {
                sb.Append(" and A.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (score_level_group != "-1")
            {
                sb.Append(" and A.SCORE_LEVEL_GROUP like @SCORE_LEVEL_GROUP ");
                ht.Add("@SCORE_LEVEL_GROUP", "%"+score_level_group + "%");
            }
            if (is_merger != "-1")
            {
                sb.Append(" and A.IS_MERGER = @IS_MERGER ");
                ht.Add("@IS_MERGER", is_merger);
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

    public int getCount(int startRowIndex, int maximumRows, string assess_year, string assess_type,
        string dept_no_20, string ws_cd, string score_level_group, string is_merger)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record from TB_S_M_ASSESS_DEP20_PEO A ");
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

            if (dept_no_20 != "")
            {
                sb.Append(" and A.DEPT_NO_20 = @DEPT_NO_20 ");
                ht.Add("@DEPT_NO_20", dept_no_20);
            }

            if (ws_cd != "-1")
            {
                sb.Append(" and A.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (score_level_group != "-1")
            {
                sb.Append(" and A.SCORE_LEVEL_GROUP like @SCORE_LEVEL_GROUP ");
                ht.Add("@SCORE_LEVEL_GROUP", "%" + score_level_group + "%");
            }
            if (is_merger != "-1")
            {
                sb.Append(" and A.IS_MERGER = @IS_MERGER ");
                ht.Add("@IS_MERGER", is_merger);
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

    
    //取得修改資料
    public DataTable getUpdData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" Select 
                         A.ASSESS_YEAR, A.ASSESS_TYPE ,  A.ASSESS_TYPE+'-'+D.SUB_DESC as ASSESS_TYPE_DESC, 
                         A.DEPT_NO_20, A.DEPT_NAME_20, A.WS_CD, A.WS_CD+'-'+C.SUB_DESC as WS_CD_DESC, A.LEVEL_CD, 
                         A.SCORE_LEVEL_GROUP,
                         A.IS_MERGER ,B.SUB_DESC as IS_MERGER_DESC ,
                         A.BASE_A, A.BASE_B, A.BASE_C, A.BASE_D, A.BASE_E, A.BASE_TOT,
                         A.REAL_A, A.REAL_B, A.REAL_C, A.REAL_D, A.REAL_E, A.REAL_TOTAL
                         from TB_S_M_ASSESS_DEP20_PEO A with (nolock)
                         left join TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='SJ' and B.MAIN_CD='IS_MERGER' and B.SUB_CD= A.IS_MERGER and B.IS_VALID='Y'
                         left join TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= A.WS_CD and C.IS_VALID='Y'
                         left join TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='SJ' and D.MAIN_CD='ASSESS_TYPE' and D.SUB_CD= A.ASSESS_TYPE and D.IS_VALID='Y' ");
            
           
            sb.Append(@" 
                where 1=1
                and A.ASSESS_YEAR = @ASSESS_YEAR  and  A.ASSESS_TYPE = @ASSESS_TYPE  and A.WS_CD=@WS_CD and A.DEPT_NO_20=@DEPT_NO_20  
            ");
            if (SCORE_LEVEL_GROUP != "-1")
            {
                sb.Append(" and A.SCORE_LEVEL_GROUP=@SCORE_LEVEL_GROUP ");
                ht.Add("@SCORE_LEVEL_GROUP", SCORE_LEVEL_GROUP);
            }
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO_20", DEPT_NO_20);
            ht.Add("@WS_CD", WS_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得修改資料
    public DataTable getSLGData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" Select distinct SCORE_LEVEL_GROUP from TB_S_M_ASSESS_DEP20_PEO where  SCORE_LEVEL_GROUP <>'' ");
           
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //更新 TB_S_M_ASSESS_DEP20_PEO
    public void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_ASSESS_DEP20_PEO ");
            sb.Append(" set BASE_A=@BASE_A, BASE_B=@BASE_B,BASE_C=@BASE_C,BASE_D=@BASE_D,BASE_E=@BASE_E,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR  and  ASSESS_TYPE = @ASSESS_TYPE  and WS_CD=@WS_CD and DEPT_NO_20=@DEPT_NO_20  and SCORE_LEVEL_GROUP=@SCORE_LEVEL_GROUP ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO_20", DEPT_NO_20);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@SCORE_LEVEL_GROUP", SCORE_LEVEL_GROUP);
            ht.Add("@BASE_A", BASE_A);
            ht.Add("@BASE_B", BASE_B);
            ht.Add("@BASE_C", BASE_C);
            ht.Add("@BASE_D", BASE_D);
            ht.Add("@BASE_E", BASE_E);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getWSByDeptNo(string assess_year, string assess_type, string dept_no_20)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select distinct B.SUB_CD,B.SUB_DESC FROM TB_S_M_ASSESS_DEP20_PEO A join  ");
			sb.Append("					TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='HB' and B.MAIN_CD='WS_CD' and B.SUB_CD= A.WS_CD and B.IS_VALID='Y' ");
            sb.Append(" where 1=1 and IS_MERGER<>'A' ");


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

            if (dept_no_20 != "")
            {
                sb.Append(" and A.DEPT_NO_20 = @DEPT_NO_20 ");
                ht.Add("@DEPT_NO_20", dept_no_20);
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }

    }
    public DataTable getPEODeptNo()
    {
        try
        {
           
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select distinct A.DEPT_NO_20 from TB_S_M_ASSESS_DEP20_PEO A where 1=1 ");


            if (ASSESS_YEAR != "")
            {
                sb.Append(" and A.ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            }

            if (ASSESS_TYPE != "-1")
            {
                sb.Append(" and A.ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            }

            

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }

    }
    public DataTable getScoreGroupLevelByDeptNo(string assess_year, string assess_type, string dept_no_20)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SCORE_LEVEL_GROUP from TB_S_M_ASSESS_DEP20_PEO A ");
            sb.Append(" where 1=1 and IS_MERGER<>'A' ");


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

            if (dept_no_20 != "")
            {
                sb.Append(" and A.DEPT_NO_20 = @DEPT_NO_20 ");
                ht.Add("@DEPT_NO_20", dept_no_20);
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }

    }
    public DataTable getScoreGroupLevelData()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select A.* from TB_S_M_ASSESS_DEP20_PEO A ");
            sb.Append(" where 1=1 and A.is_MERGER<>'A' ");


            if (ASSESS_YEAR != "")
            {
                sb.Append(" and A.ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            }

            if (ASSESS_TYPE != "-1")
            {
                sb.Append(" and A.ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            }

            if (WS_CD != "")
            {
                sb.Append(" and A.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", WS_CD);
            }
            if (DEPT_NO_20 != "")
            {
                sb.Append(" and A.DEPT_NO_20 = @DEPT_NO_20 ");
                ht.Add("@DEPT_NO_20", DEPT_NO_20);
            }
            if (LEVEL_CD != "")
            {
                sb.Append(" and A.LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", LEVEL_CD);
            }
            if (SCORE_LEVEL_GROUP != "")
            {
                sb.Append(" and CHARINDEX(@SCORE_LEVEL_GROUP,A.SCORE_LEVEL_GROUP)>0 ");
                ht.Add("@SCORE_LEVEL_GROUP", SCORE_LEVEL_GROUP);
            }
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }

    }
    //呼叫部門人數重配置重配置
    internal void execSP_S_ASSESS_GEN_DEP20_PEO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_GEN_DEP20_PEO");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO", "");
            ht.Add("@USERID", UPDATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SJ023");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }
    //呼叫第二階段重配置
    internal void execSP_S_ASSESS_GEN_L2_DATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_GEN_L2_DATA");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@USERID", UPDATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SJ023");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }
    //呼叫SP,重算PEO實際值
    internal void execSP_S_ASSESS_UPD_RO_DEP20_PEO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_UPD_RO_DEP20_PEO");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO", DEPT_NO_20);
            ht.Add("@USERID", UPDATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SJ023");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }
    public DataTable getAssessData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select  A.*, B.SUB_DESC ASSESS_TYPE_DESC ");
            sb.Append("FROM TB_S_M_ASSESS_DATA A  left join ");
            sb.Append("     TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='SJ' and B.MAIN_CD='ASSESS_TYPE' and B.SUB_CD= A.ASSESS_TYPE and B.IS_VALID='Y' ");
            sb.Append("where A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
}