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
/// WFB2SJ3600 的摘要描述
/// </summary>
public class CFB2SJ3600DAO : BaseDAO
{
    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string DEPT_NO { get; set; }
    public string DEPT_NO_20 { get; set; }
    public string EMP_ID { get; set; }
    public string WS_CD { get; set; }
    public string LEVEL_CD { get; set; }
    public string ITEM_NAME { get; set; }
    public int MNG_GRADE { get; set; }
    public string RECOMM_DESC { get; set; }
    public string COMMENTS { get; set; }
    public string SCORE_FINAL { get; set; }
    public string SCORE_DEPT { get; set; }
    public string IS_OUT { get; set; }
    public string IS_DR { get; set; }
    public string SIGN_YN { get; set; }
    public string MEMO { get; set; }
    public string GRADE { get; set; }

    public string USER_UP_YN { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SJ3600DAO()
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
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY A.DEPT_NO ) As RowNumber,");
            sb.Append(@" A.ASSESS_YEAR,A.ASSESS_TYPE, A.DIREC_EMP_ID, B.EMP_NAME, A.DEPT_NO, A.DEPT_FULL_NAME as DEPT_NAME, A.SIGN_YN ,(case when A.SIGN_YN='N' then '未提' else '已提' end) as SIGN_YN_DESC, A.MNG_NUM ,
	                           (Select count(*) from TB_S_M_FOREIGN_TARGET O left jOIN TB_S_M_FOREIGN_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                            WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO=A.DEPT_NO and P.DIREC_EMP_ID=A.DIREC_EMP_ID and isnull(O.SCORE_DIRC,'')<>'') as MNG_GRANT_NUM,
	                           (Select count(*) from TB_S_M_FOREIGN_TARGET O left jOIN TB_S_M_FOREIGN_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                            WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO=A.DEPT_NO and P.DIREC_EMP_ID=A.DIREC_EMP_ID and isnull(O.SCORE_DIRC,'')='A') as MNG_GRANT_A_NUM,
	                           (Select count(*) from TB_S_M_FOREIGN_TARGET O left jOIN TB_S_M_FOREIGN_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                            WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO=A.DEPT_NO and P.DIREC_EMP_ID=A.DIREC_EMP_ID and isnull(O.SCORE_DIRC,'')='B') as MNG_GRANT_B_NUM,
	                           (Select count(*) from TB_S_M_FOREIGN_TARGET O left jOIN TB_S_M_FOREIGN_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                            WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO=A.DEPT_NO and P.DIREC_EMP_ID=A.DIREC_EMP_ID and isnull(O.SCORE_DIRC,'')='C') as MNG_GRANT_C_NUM,
	                           (Select count(*) from TB_S_M_FOREIGN_TARGET O left jOIN TB_S_M_FOREIGN_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                            WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO=A.DEPT_NO and P.DIREC_EMP_ID=A.DIREC_EMP_ID and isnull(O.SCORE_DIRC,'')='D') as MNG_GRANT_D_NUM,
	                           (Select count(*) from TB_S_M_FOREIGN_TARGET O left jOIN TB_S_M_FOREIGN_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                            WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO=A.DEPT_NO and P.DIREC_EMP_ID=A.DIREC_EMP_ID and isnull(O.SCORE_DIRC,'')='E') as MNG_GRANT_E_NUM
                        from TB_S_M_FOREIGN_DIRECTOR_H A INNER JOIN TB_H_M_EMP B on A.DIREC_EMP_ID=B.EMP_ID    
                        where A.DIREC_EMP_ID=@DIREC_EMP_ID and A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE ");
           
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@DIREC_EMP_ID", SessionHandle.Current.emp_id);
            //ht.Add("@DIREC_EMP_ID", "14940");
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
            sb.Append("select COUNT(*) total_record from TB_S_M_FOREIGN_DIRECTOR_H A ");
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

                sb.Append(" and A.DIREC_EMP_ID = @DIREC_EMP_ID ");
                ht.Add("@DIREC_EMP_ID", SessionHandle.Current.emp_id);
                //ht.Add("@DIREC_EMP_ID", "14940");

            
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
    //取得員工明細資料
    public DataTable getEmpDtlData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" Select 
                         A.ASSESS_YEAR, A.ASSESS_TYPE ,
                         A.DIREC_EMP_ID,B.EMP_NAME, A.DEPT_NO, A.DEPT_FULL_NAME as DEPT_NAME, A.SIGN_YN ,(case when A.SIGN_YN='N' then '未提' else '已提' end) as SIGN_YN_DESC, A.MNG_NUM                          
                         from TB_S_M_FOREIGN_DIRECTOR_H A with (nolock)
                         left join TB_H_M_EMP B  with (nolock) on A.DIREC_EMP_ID=B.EMP_ID ");


            sb.Append(@" 
                where 1=1
                and A.ASSESS_YEAR = @ASSESS_YEAR  and  A.ASSESS_TYPE = @ASSESS_TYPE  and A.DEPT_NO=@DEPT_NO 
            ");
           
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO", DEPT_NO);
            if (EMP_ID != "")
            {
                sb.Append(@" AND A.DIREC_EMP_ID=@DIREC_EMP_ID ");
                ht.Add("@DIREC_EMP_ID", EMP_ID);
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getDataDtl(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string dept_no, string dirc_emp_id, string emp_id, string score_dept, string is_out, string level_cd, string ws_cd, string is_dr)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "T.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "T.EMP_NAME");
            if (sortExpression.Contains("IS_OUT"))
                sortExpression = sortExpression.Replace("IS_OUT", "T.IS_OUT");
            if (sortExpression.Contains("DEPT_NO"))
                sortExpression = sortExpression.Replace("DEPT_NO", "T.DEPT_NO");
            if (sortExpression.Contains("WS_CD"))
                sortExpression = sortExpression.Replace("WS_CD", "T.WS_CD");
            if (sortExpression.Contains("LEVEL_CD"))
                sortExpression = sortExpression.Replace("LEVEL_CD", "T.LEVEL_CD");
            if (sortExpression.Contains("SCORE_DEPT"))
                sortExpression = sortExpression.Replace("SCORE_DEPT", "T.SCORE_DEPT");
            if (sortExpression.Contains("PJOB_CD"))
                sortExpression = sortExpression.Replace("PJOB_CD", "T.PJOB_CD");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY T.SORT_LIMIT_RATE asc, " + sortExpression + "  ) As RowNumber, ");
            sb.Append(" T.* FROM( SELECT ");
            sb.Append(@" A.ASSESS_YEAR, A.ASSESS_TYPE, A.EMP_ID, B.EMP_NAME, isnull(B.IS_OUT,'N') IS_OUT,
	                    B.LEVEL_CD, B.WS_CD,  B.WS_CD+'-'+C.SUB_DESC as WS_CD_DESC,B.PJOB_CD, B.PJOB_DESC,
	                    isnull((select sum(MNG_GRADE) from TB_S_M_FOREIGN_SCORE WHERE ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID),0) MNG_GRADE,
                        ISNULL((SELECT top 1 GRADE FROM TB_S_M_FOREIGN_LOG WHERE ASSESS_YEAR=B.ASSESS_YEAR and ASSESS_TYPE=B.ASSESS_TYPE and EMP_ID=B.EMP_ID AND CREATED_BY=D.DIREC_EMP_ID order by created_DT),'') ORI_SCORE_DEPT,	                    
                        SCORE_DEPT,
                        SCORE_DIRC,
	                    (CASE WHEN A.ASSESS_TYPE='1' then B.SCORE_1H_1 else B.SCORE_2H_1 end) SCORE_H_1,
	                    (CASE WHEN A.ASSESS_TYPE='1' then B.SCORE_1H_2 else B.SCORE_2H_2 end) SCORE_H_2,
	                    B.RECENT_LEVEL_WORK_YEARS, B.AGE, B.WORK_YEARS,isnull(B.DISTING_REMARK,'') DISTING_REMARK,
                        (case when len(B.LIMIT_RATE) =1 then 1 else 0 end) SORT_LIMIT_RATE
                        FROM TB_S_M_FOREIGN_DIRECTOR_D A left join
	                         TB_S_M_FOREIGN_TARGET B on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.EMP_ID=B.EMP_ID left join
                             TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= B.WS_CD and C.IS_VALID='Y'    left join
                             TB_S_M_FOREIGN_DIRECTOR_H D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.DEPT_NO=D.DEPT_NO AND A.DIREC_EMP_ID=D.DIREC_EMP_ID
                        where A.DEPT_NO=@DEPT_NO AND D.DIREC_EMP_ID=@DIREC_EMP_ID and A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE  and A.EMP_ID<>D.DIREC_EMP_ID  ");
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (score_dept != "-1")
            {
                sb.Append(" and B.SCORE_DIRC = @SCORE_DEPT ");
                ht.Add("@SCORE_DEPT", score_dept);
            }
            if (is_out != "-1")
            {
                if (is_out == "Y")
                {

                    sb.Append(" and B.IS_OUT = 'Y' ");
                }
                else
                {
                    sb.Append(" and B.IS_OUT <> 'Y' ");
                }
            }
            if (level_cd != "-1")
            {
                sb.Append(" and B.LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and B.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (is_dr != "-1")
            {
                
                 if (is_dr == "Y")sb.Append(" and isnull(B.DISTING_REMARK,'') <>'' ");
                 if (is_dr == "N")sb.Append(" and isnull(B.DISTING_REMARK,'') ='' ");
                //ht.Add("@WS_CD", ws_cd);
            }
            sb.Append(" ) T ");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@DIREC_EMP_ID", dirc_emp_id);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCountDtl(int startRowIndex, int maximumRows, string assess_year, string assess_type, string dept_no, string dirc_emp_id, string emp_id, string score_dept, string is_out, string level_cd, string ws_cd, string is_dr)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(@" FROM TB_S_M_FOREIGN_DIRECTOR_D A left join
	                         TB_S_M_FOREIGN_TARGET B on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.EMP_ID=B.EMP_ID left join
                             TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= B.WS_CD and C.IS_VALID='Y'   left join
                             TB_S_M_FOREIGN_DIRECTOR_H D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.DEPT_NO=D.DEPT_NO  AND A.DIREC_EMP_ID=D.DIREC_EMP_ID 
                        where A.DEPT_NO=@DEPT_NO  AND D.DIREC_EMP_ID=@DIREC_EMP_ID and A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE  and A.EMP_ID<>D.DIREC_EMP_ID  ");
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (score_dept != "-1")
            {
                sb.Append(" and B.SCORE_DIRC = @SCORE_DEPT ");
                ht.Add("@SCORE_DEPT", score_dept);
            }
            if (is_out != "-1")
            {
                if (is_out == "Y")
                {

                    sb.Append(" and B.IS_OUT = 'Y' ");
                }
                else
                {
                    sb.Append(" and B.IS_OUT <> 'Y' ");
                }
            }
            if (level_cd != "-1")
            {
                sb.Append(" and B.LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and B.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (is_dr != "-1")
            {

                if (is_dr == "Y") sb.Append(" and isnull(B.DISTING_REMARK,'') <>'' ");
                if (is_dr == "N") sb.Append(" and isnull(B.DISTING_REMARK,'') ='' ");
                //ht.Add("@WS_CD", ws_cd);
            }
           
            sb.Append(" and A.ASSESS_YEAR = @ASSESS_YEAR ");
            ht.Add("@ASSESS_YEAR", assess_year);

            sb.Append(" and A.ASSESS_TYPE = @ASSESS_TYPE ");
            ht.Add("@ASSESS_TYPE", assess_type);

            sb.Append(" and A.DEPT_NO = @DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);

            ht.Add("@DIREC_EMP_ID", dirc_emp_id);

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
    //取得員工考核資料資料
    public DataTable getEmpTargetData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" Select 
                         D.DEPT_NO,D.DIREC_EMP_ID,E.EMP_NAME DIREC_EMP_NAME,G.DEPT_FULL_NAME,
                         A.ASSESS_YEAR, A.ASSESS_TYPE, (A.ASSESS_TYPE+'-'+F.SUB_DESC) ASSESS_TYPE_DESC , F.SUB_DESC ASSESS_TYPE_NAME, A.EMP_ID, B.EMP_NAME, isnull(B.IS_OUT,'N') IS_OUT,
	                    B.LEVEL_CD, B.WS_CD,  B.WS_CD+'-'+C.SUB_DESC as WS_CD_DESC,B.PJOB_DESC,
	                    isnull((select sum(MNG_GRADE) from TB_S_M_FOREIGN_SCORE WHERE ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID),0) MNG_GRADE,
                        ISNULL((SELECT top 1 GRADE FROM TB_S_M_FOREIGN_LOG WHERE ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID AND CREATED_BY=D.DIREC_EMP_ID order by created_DT),'') ORI_SCORE_DEPT,
	                    SCORE_DIRC, SCORE_DEPT,SCORE_FINAL,B.SCORE_1H_1,B.SCORE_1H_2,B.SCORE_1H_3,B.SCORE_2H_1,B.SCORE_2H_2,B.SCORE_2H_3,B.SCORE_DEPT,B.SCORE_FINAL,
	                    B.RECENT_LEVEL_WORK_YEARS, B.AGE, B.WORK_YEARS,isnull(B.DISTING_REMARK,'') DISTING_REMARK, isnull(B.RECOMM_DESC,'') RECOMM_DESC, isnull(B.COMMENTS,'') COMMENTS,
						isnull(B.LEAVE_O+B.LEAVE_P,0)LEAVE_OP,isnull(LEAVE_Q,0)LEAVE_Q,isnull(B.LEAVE_A+B.LEAVE_B,0)LEAVE_AB,isnull(B.LIMIT_RATE,'') LIMIT_RATE
                        FROM TB_S_M_FOREIGN_DIRECTOR_D A left join
							 TB_S_M_FOREIGN_DIRECTOR_H D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.DEPT_NO=D.DEPT_NO  AND A.DIREC_EMP_ID=D.DIREC_EMP_ID left join
	                         TB_S_M_FOREIGN_TARGET B on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.EMP_ID=B.EMP_ID left join
                             TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= B.WS_CD and C.IS_VALID='Y'  left join
							 TB_H_M_EMP E on D.DIREC_EMP_ID=E.EMP_ID left join 
							 TB_9_M_COMM_D F  with (nolock)  on F.SYS_CD='FJ' and F.MAIN_CD='FASSESS_TYPE' and F.SUB_CD= A.ASSESS_TYPE and F.IS_VALID='Y' left join
                             TB_H_R_DEPT_DATA_AD G on B.DEPT_NO=G.DEPT_NO ");


            sb.Append(@" 
                where 1=1
                and A.ASSESS_YEAR = @ASSESS_YEAR  and  A.ASSESS_TYPE = @ASSESS_TYPE  and A.EMP_ID=@EMP_ID
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

    //取得評分範圍
    public DataTable getEmpAssessRateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select RATE_A,RATE_B,RATE_C,RATE_D,RATE_E from TB_S_M_FOREIGN_RATE where ASSESS_TYPE=@ASSESS_TYPE  ");

            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            //ht.Add("@WS_CD", WS_CD);
           // ht.Add("@LEVEL_CD", LEVEL_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getEmpAssessScore(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type,  string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY ITEM_CD ) As RowNumber,");
            sb.Append(@" ASSESS_YEAR,ASSESS_TYPE, EMP_ID, SEQ, ITEM_NAME, ITEM_DESC, MAX_GRADE, MNG_GRADE 
                        from TB_S_M_FOREIGN_SCORE   
                        where EMP_ID=@EMP_ID and ASSESS_YEAR= @ASSESS_YEAR and ASSESS_TYPE= @ASSESS_TYPE ");
            
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getEmpAssessScoreCount(int startRowIndex, int maximumRows, string assess_year, string assess_type, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(@"  from TB_S_M_FOREIGN_SCORE   
                        where EMP_ID=@EMP_ID and ASSESS_YEAR= @ASSESS_YEAR and ASSESS_TYPE= @ASSESS_TYPE  ");
            

            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            ht.Add("@ASSESS_YEAR", assess_year);

            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            ht.Add("@ASSESS_TYPE", assess_type);

            sb.Append(" and EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);


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
    //更新 TB_S_M_FOREIGN_SCORE
    public void updateSCORE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_FOREIGN_SCORE ");
            sb.Append(" set MNG_GRADE = @MNG_GRADE, ");
            sb.Append("     UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE and ");
            sb.Append("       EMP_ID=@EMP_ID and ITEM_NAME =@ITEM_NAME ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@ITEM_CD", ITEM_NAME);
            ht.Add("@MNG_GRADE", MNG_GRADE);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新 TB_S_M_FOREIGN_TARGET
    public void updateTARGET()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_FOREIGN_TARGET ");
            sb.Append(" set RECOMM_DESC = @RECOMM_DESC, COMMENTS = @COMMENTS, SCORE_DIRC=@SCORE_DEPT, SCORE_DEPT=@SCORE_DEPT, SCORE_FINAL=@SCORE_FINAL, ");
            sb.Append("     UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE and ");
            sb.Append("       EMP_ID=@EMP_ID  ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@RECOMM_DESC", RECOMM_DESC);
            ht.Add("@COMMENTS", COMMENTS);
            ht.Add("@SCORE_FINAL", SCORE_FINAL);
            ht.Add("@SCORE_DEPT", SCORE_DEPT);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable selectData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select  E.EMP_NAME DIREC_EMP_NAME, "); 
		    sb.Append("      isnull((select sum(MNG_GRADE) from TB_S_M_FOREIGN_SCORE WHERE ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID),0) MNG_GRADE, ");
            sb.Append("      B.SCORE_DEPT,  B.SCORE_DIRC, isnull(B.IS_OUT,'N') IS_OUT, B.DISTING_REMARK, A.EMP_ID, B.EMP_NAME, D.DEPT_NO,  ");
		    sb.Append("      F.DEPT_NO_20, F.DEPT_NAME_20, F.DEPT_NO_30, F.DEPT_NAME_30, F.DEPT_NO_40, F.DEPT_NAME_40, F.DEPT_NO_50, F.DEPT_NAME_50, F.DEPT_NO_60, F.DEPT_NAME_60, F.DEPT_NO_70, F.DEPT_NAME_70, ");
            sb.Append("      B.WS_CD,  B.WS_CD+'-'+C.SUB_DESC as WS_CD_DESC,B.LEVEL_CD, B.PJOB_CD, B.PJOB_DESC, B.PJOB_WORK_YEAR, B.SERVICE_YEARS, B.AGE, B.EMP_CHG_CD, G.SUB_DESC EMP_CHG_CD_DESC,  ");
            sb.Append("      B.SCORE_1H_1, B.SCORE_1H_2, B.SCORE_1H_3, B.SCORE_2H_1, B.SCORE_2H_2, B.SCORE_2H_3,  ");
            sb.Append("      B.OVERTIME_HOUR_MEAN, B.LEAVE_O, B.LEAVE_P, B.LEAVE_Q, B.LEAVE_A, B.LEAVE_B, B.RETENTION_DAYS,  ");
            sb.Append("      B.THIRD_CNT_P, B.SECOND_CNT_P, B.FIRST_CNT_P, B.THIRD_CNT_M, B.SECOND_CNT_M, B.FIRST_CNT_M,  ");
            sb.Append("      B.PROPOSAL_TOTAL, B.PROPOSAL_GRADE, B.PROPOSAL_GRADE_MEAN, B.PROPOSAL_6 ");
            sb.Append("FROM TB_S_M_FOREIGN_DIRECTOR_D A left join ");
            sb.Append("     TB_S_M_FOREIGN_TARGET B on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.EMP_ID=B.EMP_ID left join ");
            sb.Append("     TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= B.WS_CD and C.IS_VALID='Y' left join ");
            sb.Append("     TB_S_M_FOREIGN_DIRECTOR_H D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.DEPT_NO = D.DEPT_NO AND A.DIREC_EMP_ID=D.DIREC_EMP_ID left join ");
            sb.Append("     TB_H_M_EMP E  with (nolock) on D.DIREC_EMP_ID=E.EMP_ID left join ");
            sb.Append("     TB_H_R_DEPT_DATA_AD F  with (nolock) on A.DEPT_NO = F.DEPT_NO  left join ");
            sb.Append("     TB_9_M_COMM_D G  with (nolock)  on G.SYS_CD='HB' and G.MAIN_CD='EMP_CHG_CD' and G.SUB_CD= B.EMP_CHG_CD and G.IS_VALID='Y' ");
            sb.Append("where A.DEPT_NO=@DEPT_NO and A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE and A.EMP_ID<>D.DIREC_EMP_ID ");
            if (EMP_ID != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (SCORE_DEPT != "-1")
            {
                sb.Append(" and B.SCORE_DIRC = @SCORE_DEPT ");
                ht.Add("@SCORE_DEPT", SCORE_DEPT);
            }
            if (IS_OUT != "-1")
            {
                sb.Append(" and B.IS_OUT = @IS_OUT ");
                ht.Add("@IS_OUT", IS_OUT);
            }
            if (LEVEL_CD != "-1")
            {
                sb.Append(" and B.LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", LEVEL_CD);
            }
            if (WS_CD != "-1")
            {
                sb.Append(" and B.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", WS_CD);
            }
            if (IS_DR != "-1")
            {

                if (IS_DR == "Y") sb.Append(" and isnull(B.DISTING_REMARK,'') <>'' ");
                if (IS_DR == "N") sb.Append(" and isnull(B.DISTING_REMARK,'') ='' ");
                //ht.Add("@WS_CD", ws_cd);
            }
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO", DEPT_NO);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable checkComplete()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select  A.EMP_ID ");
            sb.Append("FROM TB_S_M_FOREIGN_DIRECTOR_D A left join ");
            sb.Append("     TB_S_M_FOREIGN_TARGET B on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.EMP_ID=B.EMP_ID left join ");
            sb.Append("     TB_S_M_FOREIGN_DIRECTOR_H D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.DEPT_NO = D.DEPT_NO  AND A.DIREC_EMP_ID=D.DIREC_EMP_ID ");
            sb.Append("where A.DEPT_NO=@DEPT_NO AND A.DIREC_EMP_ID=@EMP_ID and A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE and A.EMP_ID<>D.DIREC_EMP_ID  and isnull(B.SCORE_DEPT,'')='' ");
            
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    //更新 TB_S_M_FOREIGN_DIRECTOR_H
    public void updateDIRECTORH()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_FOREIGN_DIRECTOR_H ");
            sb.Append(" set SIGN_YN=@SIGN_YN , ");
            sb.Append("     UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE() ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE  ");
            if (EMP_ID != "")
            {
                sb.Append(" and DIREC_EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (DEPT_NO != "")
            {
                sb.Append(" and DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            ht.Add("@SIGN_YN", SIGN_YN);
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增 TB_S_M_FOREIGN_LOG
    public void addAssessLog_Batch()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_S_M_FOREIGN_LOG ( ");
            sb.Append(" ASSESS_YEAR, ASSESS_TYPE, EMP_ID, GRADE, MEMO,  ");
            sb.Append(" CREATED_BY,CREATED_DT,FUNC_ID)");
            sb.Append("select  A.ASSESS_YEAR, A.ASSESS_TYPE, A.EMP_ID, B.SCORE_FINAL, isnull(COMMENTS,''), ");
            sb.Append("        @CREATED_BY,GETDATE(),@FUNC_ID ");
            sb.Append("FROM TB_S_M_FOREIGN_DIRECTOR_D A left join ");
            sb.Append("     TB_S_M_FOREIGN_TARGET B on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.EMP_ID=B.EMP_ID left join ");
            sb.Append("     TB_S_M_FOREIGN_DIRECTOR_H D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.DEPT_NO = D.DEPT_NO  AND A.DIREC_EMP_ID=D.DIREC_EMP_ID  ");
            sb.Append(" where A.DEPT_NO=@DEPT_NO and A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE and A.EMP_ID<>D.DIREC_EMP_ID   ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO", DEPT_NO);

            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //呼叫更新部長人數配置檔
    internal void execSP_S_ASSESS_UPD_RO_DEP20_PEO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_UPD_RO_DEP20_PEO");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO_20", DEPT_NO_20);
            ht.Add("@USERID", CREATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SJ360");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }
    //呼叫更新直屬主管數配置檔
    internal void execSP_S_ASSESS_DIREC_APPROVE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_DIREC_APPROVE");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            //ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEPT_NO", DEPT_NO);
           // ht.Add("@SCORE_DEPT", SCORE_DEPT);
           // ht.Add("@RECOMM_DESC", RECOMM_DESC);
           // ht.Add("@COMMENTS", COMMENTS);
            ht.Add("@USERID", CREATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SJ360");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }
    //新增 TB_S_M_FOREIGN_LOG
    public void addAssessLog()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_S_M_FOREIGN_LOG ( ");
            sb.Append(" ASSESS_YEAR, ASSESS_TYPE, EMP_ID, GRADE, MEMO,  ");
            sb.Append(" CREATED_BY,CREATED_DT,FUNC_ID)values(");
            sb.Append(" @ASSESS_YEAR, @ASSESS_TYPE, @EMP_ID, @GRADE, @MEMO,  ");
            sb.Append(" @CREATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@GRADE", GRADE);
            ht.Add("@MEMO", MEMO);

            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
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
            sb.Append("FROM TB_S_M_FOREIGN_DATA A  left join ");
            sb.Append("     TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='SJ' and B.MAIN_CD='ASSESS_TYPE' and B.SUB_CD= A.ASSESS_TYPE and B.IS_VALID='Y' ");
            sb.Append("where A.APPROVE_STATUS='N' ");



            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
    //檢查該部門是否完成簽核
    public DataTable getAssessDircH(string assess_year,string assess_type, string dept_no, string direc_emp_id)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select  A.* ");
            sb.Append("FROM TB_S_M_FOREIGN_DIRECTOR_H A  ");
            sb.Append("where ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE ");

            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO=@DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (direc_emp_id != "")
            {
                sb.Append(" and DIREC_EMP_ID=@DIREC_EMP_ID ");

                ht.Add("@DIREC_EMP_ID", direc_emp_id);
            }

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
}