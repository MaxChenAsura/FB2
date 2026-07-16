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
/// WFB2SJ0150 的摘要描述
/// </summary>
public class CFB2SJ0150DAO : BaseDAO
{

    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string GRP_CD { get; set; }
    public string WS_CD { get; set; }
    public string GRP_NAME { get; set; }
    public string REDEPLOY_YN { get; set; }
    public string REPORT_TYPE { get; set; }
    public string IS_CTL { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    //Dtl
    public string LEVEL_CD { get; set; }



    public CFB2SJ0150DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type)
    {
        try
        {
           /**
            if (sortExpression.Contains("ASSESS_YEAR"))
                sortExpression = sortExpression.Replace("ASSESS_YEAR", "A.ASSESS_YEAR");

            if (sortExpression.Contains("ASSESS_TYPE"))
                sortExpression = sortExpression.Replace("ASSESS_TYPE", "A.ASSESS_TYPE");

            if (sortExpression.Contains("GRP_CD"))
                sortExpression = sortExpression.Replace("GRP_CD", "A.GRP_CD");
           **/

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" A.ASSESS_YEAR , A.ASSESS_TYPE, A.ASSESS_TYPE+'-'+D.SUB_DESC as ASSESS_TYPE_DESC, A.GRP_CD , A.WS_CD , A.GRP_NAME , A.REDEPLOY_YN, A.REPORT_TYPE, (A.REPORT_TYPE+'-'+E.SUB_DESC) as REPORT_TYPE_DESC, ");
            sb.Append(" A.IS_CTL, CASE WHEN ISNULL(A.IS_CTL,'Y')='Y' THEN 'Y' ELSE 'N' END IS_CTL_DESC ");
            sb.Append(" from TB_S_M_ASSESS_GROUP_H A with (nolock) left join ");
            sb.Append("      TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='SJ' and D.MAIN_CD='ASSESS_TYPE' and D.SUB_CD= A.ASSESS_TYPE and D.IS_VALID='Y' left join ");
            sb.Append("      TB_9_M_COMM_D E  with (nolock)  on E.SYS_CD='SJ' and E.MAIN_CD='REPORT_TYPE' and E.SUB_CD= A.REPORT_TYPE and E.IS_VALID='Y'  ");
            sb.Append(" where 1=1");

            if (assess_year != "")
            {
                sb.Append(" and A.ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", assess_year);
            }
            if (assess_type != "")
            {
                sb.Append(" and A.ASSESS_TYPE = @ASSESS_TYPE ");
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

    public int getCount(int startRowIndex, int maximumRows, string assess_year, string assess_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_ASSESS_GROUP_H  A ");
            sb.Append(" where 1=1");

            if (assess_year != "")
            {
                sb.Append(" and  A.ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", assess_year);
            }
            if (assess_type != "")
            {
                sb.Append(" and A.ASSESS_TYPE = @ASSESS_TYPE ");
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
    //gv_result新刪修
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_S_M_ASSESS_GROUP_H");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE and GRP_CD=@GRP_CD ");
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@GRP_CD", GRP_CD);

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
            
            sb.Append("select  A.ASSESS_YEAR , A.ASSESS_TYPE, A.ASSESS_TYPE+'-'+D.SUB_DESC as ASSESS_TYPE_DESC, A.GRP_CD , A.WS_CD , A.GRP_NAME , A.REDEPLOY_YN ,A.IS_CTL ");
            sb.Append(" from TB_S_M_ASSESS_GROUP_H A with (nolock) left join ");
            sb.Append("      TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='SJ' and D.MAIN_CD='ASSESS_TYPE' and D.SUB_CD= A.ASSESS_TYPE and D.IS_VALID='Y'  ");
           
            sb.Append(@" 
                where 1=1
                and A.ASSESS_YEAR =@ASSESS_YEAR
                and A.ASSESS_TYPE =@ASSESS_TYPE
                and A.GRP_CD =@GRP_CD
            ");

            ht.Add("@ASSESS_YEAR", this.ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", this.ASSESS_TYPE);
            ht.Add("@GRP_CD", this.GRP_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getDataDtl(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string grp_cd)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" ASSESS_YEAR, ASSESS_TYPE, GRP_CD, LEVEL_CD ");
            sb.Append(" from TB_S_M_ASSESS_GROUP_D ");
            sb.Append(" where 1=1");

            if (assess_year != "")
            {
                sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", assess_year);
            }

            if (assess_type != "")
            {
                sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
            }

            if (grp_cd != "")
            {
                sb.Append(" and GRP_CD = @GRP_CD ");
                ht.Add("@GRP_CD", grp_cd);
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

    public int getCountDtl(int startRowIndex, int maximumRows, string assess_year, string assess_type, string grp_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_ASSESS_GROUP_D ");
            sb.Append(" where 1=1");

            if (assess_year != "")
            {
                sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", assess_year);
            }

            if (assess_type != "")
            {
                sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
            }

            if (grp_cd != "")
            {
                sb.Append(" and GRP_CD = @GRP_CD ");
                ht.Add("@GRP_CD", grp_cd);
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
    public DataTable getExistDataDtl()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select B.ASSESS_YEAR ,B.ASSESS_TYPE, B.GRP_CD , B.LEVEL_CD  
                        from 
                              TB_S_M_ASSESS_GROUP_D B left join 
                              TB_S_M_ASSESS_GROUP_H A on B.ASSESS_YEAR=A.ASSESS_YEAR and B.ASSESS_TYPE=A.ASSESS_TYPE and B.GRP_CD=A.GRP_CD
                         ");
            sb.Append(" where B.ASSESS_YEAR = @ASSESS_YEAR and B.ASSESS_TYPE = @ASSESS_TYPE and A.WS_CD = @WS_CD and B.LEVEL_CD = @LEVEL_CD ");
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增 TB_S_M_ASSESS_GROUP_H
    public void addGROUP_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_S_M_ASSESS_GROUP_H ( ");
            sb.Append(" ASSESS_YEAR , ASSESS_TYPE, GRP_CD , WS_CD , GRP_NAME , REDEPLOY_YN, REPORT_TYPE, IS_CTL, ");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @ASSESS_YEAR , @ASSESS_TYPE, @GRP_CD , @WS_CD , @GRP_NAME , @REDEPLOY_YN, @REPORT_TYPE,@IS_CTL,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@GRP_CD", GRP_CD);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@GRP_NAME", GRP_NAME);
            ht.Add("@REDEPLOY_YN", REDEPLOY_YN);
            ht.Add("@REPORT_TYPE", REPORT_TYPE);
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

    //更新 TB_S_M_ASSESS_GROUP_H
    public void updateGROUP_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_ASSESS_GROUP_H ");
            sb.Append(" set WS_CD = @WS_CD,GRP_NAME = @GRP_NAME,REDEPLOY_YN = @REDEPLOY_YN,REPORT_TYPE = @REPORT_TYPE,IS_CTL=@IS_CTL,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE and GRP_CD = @GRP_CD");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@GRP_CD", GRP_CD);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@GRP_NAME", GRP_NAME);
            ht.Add("@REDEPLOY_YN", REDEPLOY_YN);
            ht.Add("@REPORT_TYPE", REPORT_TYPE);
            ht.Add("@IS_CTL", IS_CTL);

            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增 TB_S_M_ASSESS_GROUP_D
    public void addGROUP_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_S_M_ASSESS_GROUP_D ( ");
            sb.Append(" ASSESS_YEAR , ASSESS_TYPE, GRP_CD , LEVEL_CD,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @ASSESS_YEAR , @ASSESS_TYPE, @GRP_CD , @LEVEL_CD,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@GRP_CD", GRP_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);

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

    public void Delete(string assess_year, string assess_type, string grp_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Delete From TB_S_M_ASSESS_GROUP_H ");
            sb.AppendLine(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.AppendLine(" and ASSESS_TYPE = @ASSESS_TYPE ");
            sb.AppendLine(" and GRP_CD = @GRP_CD");
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@GRP_CD", grp_cd);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void DeleteDtl(string assess_year, string assess_type, string grp_cd, string level_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Delete From TB_S_M_ASSESS_GROUP_D ");
            sb.AppendLine(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.AppendLine(" and ASSESS_TYPE = @ASSESS_TYPE ");
            sb.AppendLine(" and GRP_CD = @GRP_CD ");
            if (level_cd != "")
            {

                sb.AppendLine(" and LEVEL_CD = @LEVEL_CD");
                ht.Add("@LEVEL_CD", level_cd);
            }
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@GRP_CD", grp_cd);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得LEVEL資料
    public DataTable getLevelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select LEVEL_CD from TB_H_M_LEVEL where GETDATE() >=start_dt and GETDATE() <=end_dt and is_union_member='Y' order by level_cd ");
            

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //
    public DataTable getGroupH(String assess_year, String assess_type, String ws_cd, String grp_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select  A.ASSESS_YEAR , A.ASSESS_TYPE, A.ASSESS_TYPE+'-'+D.SUB_DESC as ASSESS_TYPE_DESC, A.GRP_CD , A.WS_CD , A.GRP_NAME , A.REDEPLOY_YN , A.IS_CTL, A.REPORT_TYPE ");
            sb.Append(" from TB_S_M_ASSESS_GROUP_H A with (nolock) left join ");
            sb.Append("      TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='SJ' and D.MAIN_CD='ASSESS_TYPE' and D.SUB_CD= A.ASSESS_TYPE and D.IS_VALID='Y'  ");

            sb.Append(@" 
                where 1=1
                and A.ASSESS_YEAR =@ASSESS_YEAR
                and A.ASSESS_TYPE =@ASSESS_TYPE
            ");
            if (ws_cd != "-1")
            {
                sb.Append("      and A.WS_CD=@WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }

            if (grp_cd != "-1")
            {
                sb.Append("      and A.GRP_CD=@grp_cd ");
                ht.Add("@GRP_CD", grp_cd);
            }
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);


            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得修改資料
    public DataTable getGRPData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select H.ASSESS_YEAR,H.ASSESS_TYPE,H.GRP_CD, H.GRP_NAME, H.WS_CD,isnull(D.LEVEL_CD,'')LEVEL_CD 
                         from TB_S_M_ASSESS_GROUP_H H left join
                              TB_S_M_ASSESS_GROUP_D D on H.ASSESS_YEAR=D.ASSESS_YEAR and  H.ASSESS_TYPE=D.ASSESS_TYPE and H.GRP_CD=D.GRP_CD ");


            sb.Append(@" 
                where 1=1
                and H.ASSESS_YEAR = @ASSESS_YEAR  and  H.ASSESS_TYPE = @ASSESS_TYPE and H.WS_CD=@WS_CD and D.LEVEL_CD=@LEVEL_CD ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getNoSetDtlGroupH(string assessYear,string assessType)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select H.* from TB_S_M_ASSESS_GROUP_H H left join TB_S_M_ASSESS_GROUP_D D on H.ASSESS_YEAR=D.ASSESS_YEAR and H.ASSESS_TYPE=D.ASSESS_TYPE and H.GRP_CD=D.GRP_CD  ");
            sb.Append("where H.ASSESS_YEAR=@ASSESS_YEAR and H.ASSESS_TYPE=@ASSESS_TYPE and D.GRP_CD is null ");
            ht.Add("@ASSESS_YEAR", assessYear);
            ht.Add("@ASSESS_TYPE", assessType);
            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
}