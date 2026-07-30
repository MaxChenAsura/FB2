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
/// WFB2SJ3800 的摘要描述
/// </summary>
public class CFB2SJ3800DAO : BaseDAO
{
    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string DEPT_NO { get; set; }
    public string EMP_ID { get; set; }
    public string LEVEL_CD { get; set; }
    public string SCORE_FINAL { get; set; }

    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SJ3800DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //取得預設主管資訊
    public DataTable getDeptDataByEmpId()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" Select distinct A.DEPT_LEVEL,A.EMP_ID, B.EMP_NAME,
                               (SELECT Count(*) from TB_S_M_FOREIGN_DEP20_UP_SIGN  where ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID and (isnull(SIGN_YN,'N')='N' or SIGN_YN='')) SIGN_COUNT
                           from TB_S_M_FOREIGN_DEP20_UP_SIGN A  left join 
                                TB_H_M_EMP B on A.EMP_ID =B.EMP_ID ");


            sb.Append(@" 
                where A.EMP_ID=@EMP_ID and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE
            ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string ma_emp_id, string level_cd, string score_final, string emp_id, string dept_no, string is_supper)
    {
        try
        {
            /**if (sortExpression.Contains("DEPT_NO"))
                sortExpression = sortExpression.Replace("DEPT_NO", "A.DEPT_NO");

            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "A.EMP_ID");

            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "A.EMP_NAME");

            if (sortExpression.Contains("SCORE_FINAL"))
                sortExpression = sortExpression.Replace("SCORE_FINAL", "A.SCORE_FINAL");

            if (sortExpression.Contains("ASSESS_YEAR"))
                sortExpression = sortExpression.Replace("ASSESS_YEAR", "A.ASSESS_YEAR");

            if (sortExpression.Contains("ASSESS_TYPE"))
                sortExpression = sortExpression.Replace("ASSESS_TYPE", "A.ASSESS_TYPE");**/

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" T.* FROM( SELECT ");
            sb.Append(@" A.ASSESS_YEAR, A.ASSESS_TYPE, A.ASSESS_TYPE+'-'+B.SUB_DESC as ASSESS_TYPE_DESC, A.EMP_ID, A.EMP_NAME, C.DEPT_NO_20, C.DEPT_NAME_20, A.SCORE_FINAL
                        FROM TB_S_M_FOREIGN_DIRECTOR_D E left join
                             TB_S_M_FOREIGN_TARGET A  on E.ASSESS_YEAR=A.ASSESS_YEAR and E.ASSESS_TYPE=A.ASSESS_TYPE and E.EMP_ID=A.EMP_ID left join
                             TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='SJ' and B.MAIN_CD='ASSESS_TYPE' and B.SUB_CD= A.ASSESS_TYPE and B.IS_VALID='Y' left join
                             TB_H_R_DEPT_DATA_AD C on A.DEPT_NO=C.DEPT_NO
                        where E.ASSESS_YEAR=@ASSESS_YEAR and E.ASSESS_TYPE= @ASSESS_TYPE  ");
            if (is_supper == "N")
            {
                sb.Append(@"
                        and A.DEPT_NO in(
                            select SC.DEPT_NO 
                            from TB_H_R_DEPT_DATA_AD SA left join
                                 TB_S_M_FOREIGN_DEPT_LEVEL SB on SA. DEPT_NO=SB.DEPT_NO left join 
	                             TB_S_M_FOREIGN_DEPT_LEVEL SC on SUBSTRING(SC.LEVEL_RATE,1,len(SB.LEVEL_RATE))=SB.LEVEL_RATE  AND SB.ASSESS_YEAR=SC.ASSESS_YEAR AND SB.ASSESS_TYPE=SC.ASSESS_TYPE
                            where SA.HEAD_EMP_ID=@MA_EMP_ID and SB.ASSESS_YEAR=E.ASSESS_YEAR and SB.ASSESS_TYPE=E.ASSESS_TYPE 
                            union
                            select Q.DEPT_NO  
	                        from TB_S_M_FOREIGN_DEPT20_MA O left join   
		                         TB_S_M_FOREIGN_DEPT_LEVEL P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE = P.ASSESS_TYPE and O.DEPT_NO_20=P.DEPT_NO and P.DEPT_LEVEL>='20' left join  
		                         TB_S_M_FOREIGN_DEPT_LEVEL Q on P.ASSESS_YEAR=Q.ASSESS_YEAR and P.ASSESS_TYPE = Q.ASSESS_TYPE and SUBSTRING(Q.LEVEL_RATE,1,len(P.LEVEL_RATE))=P.LEVEL_RATE  
	                        where O.MA_EMP_ID=@MA_EMP_ID and O.ASSESS_YEAR=E.ASSESS_YEAR and O.ASSESS_TYPE=E.ASSESS_TYPE 
                                         )
                ");
            }
            if (dept_no != "")
            {
                sb.Append(@" and A.DEPT_NO  in (
                        SELECT SB.DEPT_NO 
                        FROM TB_S_M_FOREIGN_DEPT_LEVEL SA   left join 
	                             TB_S_M_FOREIGN_DEPT_LEVEL SB on SUBSTRING(SB.LEVEL_RATE,1,len(SA.LEVEL_RATE))=SA.LEVEL_RATE  AND SA.ASSESS_YEAR=SB.ASSESS_YEAR  AND SA.ASSESS_TYPE=SB.ASSESS_TYPE
                        WHERE SA.ASSESS_YEAR=E.ASSESS_YEAR AND SA.ASSESS_TYPE=E.ASSESS_TYPE AND SA.DEPT_NO=@DEPT_NO
                )
                ");
                ht.Add("@DEPT_NO", dept_no);
            }

            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID  = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (score_final != "-1")
            {
                sb.Append(" and A.SCORE_FINAL  = @SCORE_FINAL ");
                ht.Add("@SCORE_FINAL", score_final);
            }
            if (level_cd != "-1")
            {
                sb.Append(" and A.LEVEL_CD  = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }
            sb.Append(" ) T ");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@MA_EMP_ID", ma_emp_id);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string assess_year, string assess_type,  string ma_emp_id, string level_cd, string score_final,string emp_id,string dept_no,string is_supper)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNT(*) total_record FROM  TB_S_M_FOREIGN_DIRECTOR_D E left join ");
            sb.Append("                  TB_S_M_FOREIGN_TARGET A  on E.ASSESS_YEAR=A.ASSESS_YEAR and E.ASSESS_TYPE=A.ASSESS_TYPE and E.EMP_ID=A.EMP_ID  ");
            sb.Append(" where E.ASSESS_YEAR=@ASSESS_YEAR and E.ASSESS_TYPE= @ASSESS_TYPE  ");

            if (is_supper == "N")
            {
                sb.Append(@"
                        and A.DEPT_NO in(
                            select SC.DEPT_NO 
                            from TB_H_R_DEPT_DATA_AD SA left join
                                 TB_S_M_FOREIGN_DEPT_LEVEL SB on SA. DEPT_NO=SB.DEPT_NO left join 
	                             TB_S_M_FOREIGN_DEPT_LEVEL SC on SUBSTRING(SC.LEVEL_RATE,1,len(SB.LEVEL_RATE))=SB.LEVEL_RATE  AND SB.ASSESS_YEAR=SC.ASSESS_YEAR AND SB.ASSESS_TYPE=SC.ASSESS_TYPE
                            where SA.HEAD_EMP_ID=@MA_EMP_ID and SB.ASSESS_YEAR=E.ASSESS_YEAR and SB.ASSESS_TYPE=E.ASSESS_TYPE
                            union
                            select Q.DEPT_NO  
	                        from TB_S_M_FOREIGN_DEPT20_MA O left join   
		                         TB_S_M_FOREIGN_DEPT_LEVEL P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE = P.ASSESS_TYPE and O.DEPT_NO_20=P.DEPT_NO and P.DEPT_LEVEL>='20' left join  
		                         TB_S_M_FOREIGN_DEPT_LEVEL Q on P.ASSESS_YEAR=Q.ASSESS_YEAR and P.ASSESS_TYPE = Q.ASSESS_TYPE and SUBSTRING(Q.LEVEL_RATE,1,len(P.LEVEL_RATE))=P.LEVEL_RATE  
	                        where O.MA_EMP_ID=@MA_EMP_ID and O.ASSESS_YEAR=E.ASSESS_YEAR and O.ASSESS_TYPE=E.ASSESS_TYPE 
                                         )
                ");
            }
            if (dept_no != "")
            {
                sb.Append(@" and A.DEPT_NO  in (
                        SELECT SB.DEPT_NO 
                        FROM TB_S_M_FOREIGN_DEPT_LEVEL SA   left join 
	                             TB_S_M_FOREIGN_DEPT_LEVEL SB on SUBSTRING(SB.LEVEL_RATE,1,len(SA.LEVEL_RATE))=SA.LEVEL_RATE  AND SA.ASSESS_YEAR=SB.ASSESS_YEAR  AND SA.ASSESS_TYPE=SB.ASSESS_TYPE
                        WHERE SA.ASSESS_YEAR=E.ASSESS_YEAR AND SA.ASSESS_TYPE=E.ASSESS_TYPE AND SA.DEPT_NO=@DEPT_NO
                )
                ");
                ht.Add("@DEPT_NO", dept_no);
            }

            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID  = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (score_final != "-1")
            {
                sb.Append(" and A.SCORE_FINAL  = @SCORE_FINAL ");
                ht.Add("@SCORE_FINAL", score_final);
            }
            if (level_cd != "-1")
            {
                sb.Append(" and A.LEVEL_CD  = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@MA_EMP_ID", ma_emp_id);

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
    //取得員工簽核記錄
    public DataTable getAssessLog()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" SELECT ROW_NUMBER() OVER(ORDER BY A.CREATED_DT ASC ) As RowNumber, A.ASSESS_YEAR, A.ASSESS_TYPE, A.EMP_ID, A.GRADE, A.MEMO, A.CREATED_BY, A.CREATED_DT,B.EMP_NAME
                         FROM   TB_S_M_FOREIGN_LOG A left join 
	                             TB_H_M_EMP B on A.CREATED_BY=B.EMP_ID ");


            sb.Append(@" 
                where 1=1
                and A.ASSESS_YEAR = @ASSESS_YEAR  and  A.ASSESS_TYPE = @ASSESS_TYPE  and A.EMP_ID=@EMP_ID
                ORDER BY A.CREATED_DT
            ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }
}