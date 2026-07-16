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
/// WFB2SJ0170 的摘要描述
/// </summary>
public class CFB2SJ0170DAO : BaseDAO
{
    

    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string WS_CD { get; set; }
    public string DEPT_NO_20 { get; set; }
    public string DEPT_NAME_20 { get; set; }
    public string POINT_GROUP { get; set; }
    public decimal DEPT_PEO { get; set; }
    public decimal DEPT_POINT { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2SJ0170DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(@"             A.ASSESS_YEAR , A.ASSESS_TYPE ,  A.ASSESS_TYPE+'-'+B.SUB_DESC as ASSESS_TYPE_DESC,
	                                         A.DEPT_NO_20, A.DEPT_NAME_20, A.WS_CD, A.POINT_GROUP, A.DEPT_PEO,A.DEPT_POINT
                                 FROM TB_S_M_ASSESS_DEPTPOINT A LEFT JOIN
	                                       TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='SJ' and B.MAIN_CD='ASSESS_TYPE' and B.SUB_CD= A.ASSESS_TYPE and B.IS_VALID='Y' ");
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
            sb.Append("select COUNT(*) total_record from TB_S_M_ASSESS_DEPTPOINT A ");

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

    public int getRealDeptPeo(string assess_year, string assess_type, string dept_no_20,String ws_cd,String point_group)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"SELECT COUNT(A.EMP_ID) dept_peo
                                FROM TB_S_M_ASSESS_DIRECTOR_D A LEFT JOIN
                                     TB_S_M_ASSESS_TARGET B ON A.ASSESS_YEAR=B.ASSESS_YEAR AND A.ASSESS_TYPE=B.ASSESS_TYPE AND A.EMP_ID=B.EMP_ID
                                WHERE A.ASSESS_YEAR=@ASSESS_YEAR  AND A.ASSESS_TYPE=@ASSESS_TYPE  AND B.WS_CD=@WS_CD  AND ISNULL(B.IS_OUT,'')<>'Y'  AND
                                        B.LEVEL_CD IN(SELECT LEVEL_CD FROM TB_S_M_ASSESS_POINT_YEAR WHERE ASSESS_YEAR=@ASSESS_YEAR AND ASSESS_TYPE=@ASSESS_TYPE  AND POINT_GROUP=@POINT_GROUP ) AND 
                                      A.DEPT_NO IN(SELECT SA.DEPT_NO FROM TB_S_M_ASSESS_DEPT_LEVEL SA  JOIN
	                                                           TB_S_M_ASSESS_DEPT_LEVEL SB ON SA.ASSESS_YEAR=SB.ASSESS_YEAR AND SA.ASSESS_TYPE=SB.ASSESS_TYPE AND   SUBSTRING(SA.LEVEL_RATE,1,len(SB.LEVEL_RATE))=SB.LEVEL_RATE AND SA.LEVEL_RATE<>SB.LEVEL_RATE
				                                    WHERE SA.ASSESS_YEAR=A.ASSESS_YEAR AND SA.ASSESS_TYPE=A.ASSESS_TYPE AND SB.DEPT_NO=@DEPT_NO_20 )
                               ");

       
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@DEPT_NO_20", dept_no_20);
            ht.Add("@WS_CD", ws_cd);
            ht.Add("@POINT_GROUP", point_group);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["dept_peo"];
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }

    }
    public int getTargetCount(string assess_year, string assess_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"SELECT COUNT(EMP_ID) emp_count
                                FROM TB_S_M_ASSESS_TARGET 
                                WHERE ASSESS_YEAR=@ASSESS_YEAR  AND ASSESS_TYPE=@ASSESS_TYPE 
                               ");


            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["emp_count"];
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }

    }
    public DataTable getPointGroupData(string assess_year, string assess_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT distinct WS_CD,POINT_GROUP ");
            sb.Append(" FROM TB_S_M_ASSESS_POINT_YEAR ");
            sb.Append(" where 1=1  and ASSESS_YEAR = @ASSESS_YEAR  and ASSESS_TYPE = @ASSESS_TYPE");
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            


            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getWSCD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT  WS_CD ");
            sb.Append(" FROM TB_H_M_EMP ");
            sb.Append(" where 1=1  ");
            sb.Append(" GROUP BY WS_CD ");



            return dbConn.Query(sb, ht);
        }
        catch
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
            sb.Append(@" delete from TB_S_M_ASSESS_DEPTPOINT 
                        where ASSESS_YEAR = @ASSESS_YEAR 
                        and ASSESS_TYPE = @ASSESS_TYPE
                        ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
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
            sb.Append("INSERT INTO TB_S_M_ASSESS_DEPTPOINT (ASSESS_YEAR,ASSESS_TYPE,DEPT_NO_20,DEPT_NAME_20,WS_CD,POINT_GROUP,DEPT_PEO,DEPT_POINT, ");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)values(");
            sb.Append(" @ASSESS_YEAR,@ASSESS_TYPE,@DEPT_NO_20,@DEPT_NAME_20,@WS_CD,@POINT_GROUP,@DEPT_PEO,@DEPT_POINT, ");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO_20", DEPT_NO_20);
            ht.Add("@DEPT_NAME_20", DEPT_NAME_20);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@POINT_GROUP", POINT_GROUP);
            ht.Add("@DEPT_PEO", DEPT_PEO);
            ht.Add("@DEPT_POINT", DEPT_POINT);
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
    public void Add(string assess_year, string assess_type, string cell1, string cell2, string cell3)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_M_ASSESS_WS_CHANGE (ASSESS_YEAR , ASSESS_TYPE , EMP_ID , WS_CD, ");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            if (cell1.Trim() == "")
                ht.Add("@EMP_ID", DBNull.Value);
            else
                ht.Add("@EMP_ID", cell1.Trim());
            if (cell3.Trim() == "")
                ht.Add("@WS_CD", DBNull.Value);
            else
                ht.Add("@WS_CD", cell3.Trim());
            ht.Add("@WS_CD", cell3.Trim());
            
            
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SJ0170");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    
}