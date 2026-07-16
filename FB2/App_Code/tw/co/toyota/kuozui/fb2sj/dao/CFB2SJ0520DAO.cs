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
/// WFB2SJ0520 的摘要描述
/// </summary>
public class CFB2SJ0520DAO : BaseDAO
{
    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string ASSESS_TYPE_DESC { get; set; }
    public string DEPT_NO { get; set; }
    public string DEPT_NAME { get; set; }
    public string EMP_ID { get; set; }
    public string WS_CD { get; set; }
    public string GRP_CD { get; set; }
    public string LEVEL_CD { get; set; }
    public string ITEM_CD { get; set; }
    public int MNG_GRADE { get; set; }
    public string RECOMM_DESC { get; set; }
    public string SUGGEST_DESC { get; set; }
    public string COMMENTS { get; set; }
    public string SCORE_FINAL { get; set; }
    public string SCORE_DEPT { get; set; }
    public string IS_OUT { get; set; }
    public string IS_DR { get; set; }
    public string SIGN_YN { get; set; }
    public string IS_DEPT_20 { get; set; }
    public string SCORE_LEVEL_GROUP { get; set; }
    public string MA_EMP_ID { get; set; }
    public string MA_EMP_NAME { get; set; }
    public string MA_TYPE { get; set; }

    public string USER_UP_YN { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SJ0520DAO()
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
            sb.Append(@" Select distinct A.DEPT_LEVEL,A.EMP_ID, B.EMP_NAME,A.DEPT_NO, C.DEPT_NAME,
                               (SELECT Count(*) from TB_S_M_ASSESS_DEP20_UP_SIGN  where ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID and (isnull(SIGN_YN,'N')='N' or SIGN_YN='') AND DEPT_LEVEL<'20') SIGN_COUNT
                           from TB_S_M_ASSESS_DEP20_UP_SIGN A  left join 
                                TB_H_M_EMP B on A.EMP_ID =B.EMP_ID left join
                                TB_H_R_DEPT_DATA_AD C on A.DEPT_NO=C.DEPT_NO ");


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
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string ma_emp_id,  string ws_cd, string grp_cd, string score_final, string emp_id, string recomm_desc)
    {
        try
        {
           /** if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "A.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "A.EMP_NAME");
            if (sortExpression.Contains("IS_OUT"))
                sortExpression = sortExpression.Replace("IS_OUT", "A.IS_OUT");
            if (sortExpression.Contains("DEPT_NO"))
                sortExpression = sortExpression.Replace("DEPT_NO", "A.DEPT_NO");
            if (sortExpression.Contains("WS_CD"))
                sortExpression = sortExpression.Replace("WS_CD", "A.WS_CD");
            if (sortExpression.Contains("LEVEL_CD"))
                sortExpression = sortExpression.Replace("LEVEL_CD", "A.LEVEL_CD");
            if (sortExpression.Contains("SCORE_DEPT"))
                sortExpression = sortExpression.Replace("SCORE_DEPT", "A.SCORE_DEPT");
            if (sortExpression.Contains("ASSESS_YEAR"))
                sortExpression = sortExpression.Replace("ASSESS_YEAR", "A.ASSESS_YEAR");
            if (sortExpression.Contains("ASSESS_TYPE"))
                sortExpression = sortExpression.Replace("ASSESS_TYPE", "A.ASSESS_TYPE");**/
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY  " + sortExpression + "  ) As RowNumber,");
            sb.Append(" T.* FROM( SELECT ");
            sb.Append(@" A.ASSESS_YEAR, A.ASSESS_TYPE, A.EMP_ID, A.EMP_NAME, A.DEPT_NO, A.DEPT_NAME,isnull(A.IS_OUT,'N') IS_OUT,
	                     A.LEVEL_CD, A.WS_CD,  A.WS_CD+'-'+B.SUB_DESC as WS_CD_DESC, A.SCORE_DEPT, A.SCORE_FINAL, 
	                     isnull((select sum(MNG_GRADE) from TB_S_M_ASSESS_SCORE WHERE ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID),0) MNG_GRADE,
	                     ((SELECT case when Count(*)>0 then Count(*)-1 else Count(*) end FROM TB_S_M_ASSESS_LOG WHERE ASSESS_YEAR=A.ASSESS_YEAR and  ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID)) FIX_COUNT,
	                     (CASE WHEN A.ASSESS_TYPE='1' then A.SCORE_1H_1 else A.SCORE_2H_1 end) SCORE_H_1,
	                     (CASE WHEN A.ASSESS_TYPE='1' then A.SCORE_1H_2 else A.SCORE_2H_2 end) SCORE_H_2,
		                 A.RECENT_LEVEL_WORK_YEARS, A.AGE, A.WORK_YEARS,isnull(A.DISTING_REMARK,'') DISTING_REMARK,
		                 (A.LEAVE_O+A.LEAVE_P) LEAVE_OP, A.LEAVE_Q, (A.LEAVE_A+A.LEAVE_B) LEAVE_AB, D.SIGN_YN, A.RECOMM_DESC, F.SUGGEST_REMARK, A.SUGGEST_DESC, 
                         (isnull(F.SUGGEST_REMARK,'')+isnull(A.RECOMM_DESC,'')+isnull(A.SUGGEST_DESC,'')+isnull(A.DISTING_REMARK,'')) M_MEMO,A.PJOB_CD, A.PJOB_DESC
                        FROM TB_S_M_ASSESS_DIRECTOR_D E left join
                             TB_S_M_ASSESS_TARGET A  on E.ASSESS_YEAR=A.ASSESS_YEAR and E.ASSESS_TYPE=A.ASSESS_TYPE and E.EMP_ID=A.EMP_ID left join
                             ( select S1.ASSESS_YEAR,S1.ASSESS_TYPE ,S1.WS_CD, S1.GRP_CD,isnull(S2.LEVEL_CD ,'')LEVEL_CD 
                                               from TB_S_M_ASSESS_GROUP_H S1 left join  TB_S_M_ASSESS_GROUP_D S2 on S1.ASSESS_YEAR=S2.ASSESS_YEAR and S1.ASSESS_TYPE =S2.ASSESS_TYPE and S1.GRP_CD=S2.GRP_CD )C
                                                     on A.ASSESS_YEAR=C.ASSESS_YEAR and A.ASSESS_TYPE=C.ASSESS_TYPE and A.WS_CD=C.WS_CD and A.LEVEL_CD=C.LEVEL_CD left join
                               TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='HB' and B.MAIN_CD='WS_CD' and B.SUB_CD= A.WS_CD and B.IS_VALID='Y'  left join
                               TB_S_M_ASSESS_DIRECTOR_H D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.DEPT_NO=D.DEPT_NO left join
                               TB_S_M_ASSESS_EMP_SUGGEST F on A.ASSESS_YEAR=F.ASSESS_YEAR and A.ASSESS_TYPE=F.ASSESS_TYPE and A.EMP_ID=F.EMP_ID
                        where E.ASSESS_YEAR=@ASSESS_YEAR and E.ASSESS_TYPE= @ASSESS_TYPE  and
                              A.DEPT_NO in(
	                           select distinct Y.DEPT_NO
								FROM
										TB_S_M_ASSESS_DEPT20_MA SA left join
										(select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR, ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL ) X ON X.DEPT_NO=SA.DEPT_NO_20 AND X.ASSESS_YEAR=SA.ASSESS_YEAR AND X.ASSESS_TYPE=SA.ASSESS_TYPE left JOIN 
										TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR  and X.ASSESS_TYPE=Y.ASSESS_TYPE
 								where X.ASSESS_YEAR=E.ASSESS_YEAR and X.ASSESS_TYPE=E.ASSESS_TYPE and SA.MA_EMP_ID=@MA_EMP_ID 
	                          ) ");
            if (ws_cd != "-1")
            {
                sb.Append(" and A.WS_CD  = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }

            if (grp_cd != "-1")
            {
                sb.Append(" and C.GRP_CD  = @GRP_CD ");
                ht.Add("@GRP_CD", grp_cd);
            }
            if (score_final != "-1")
            {
                sb.Append(" and A.SCORE_FINAL  = @SCORE_FINAL ");
                ht.Add("@SCORE_FINAL", score_final);
            }
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID  = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (recomm_desc != "-1")
            {
                sb.Append(" and A.RECOMM_DESC  = @RECOMM_DESC ");
                ht.Add("@RECOMM_DESC", recomm_desc);
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

    public int getCount(int startRowIndex, int maximumRows, string assess_year, string assess_type, string ma_emp_id,  string ws_cd, string grp_cd, string score_final, string emp_id, string recomm_desc)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNT(*) total_record FROM TB_S_M_ASSESS_DIRECTOR_D E left join ");
            sb.Append("                 TB_S_M_ASSESS_TARGET A  on E.ASSESS_YEAR=A.ASSESS_YEAR and E.ASSESS_TYPE=A.ASSESS_TYPE and E.EMP_ID=A.EMP_ID left join ");
            sb.Append("                 ( select S1.ASSESS_YEAR,S1.ASSESS_TYPE ,S1.WS_CD, S1.GRP_CD,isnull(S2.LEVEL_CD ,'')LEVEL_CD ");
            sb.Append("                   from TB_S_M_ASSESS_GROUP_H S1 left join  TB_S_M_ASSESS_GROUP_D S2 on S1.ASSESS_YEAR=S2.ASSESS_YEAR and S1.ASSESS_TYPE =S2.ASSESS_TYPE and S1.GRP_CD=S2.GRP_CD )C ");
            sb.Append("                  on A.ASSESS_YEAR=C.ASSESS_YEAR and A.ASSESS_TYPE=C.ASSESS_TYPE and A.WS_CD=C.WS_CD and A.LEVEL_CD=C.LEVEL_CD ");
            sb.Append(" where E.ASSESS_YEAR=@ASSESS_YEAR and E.ASSESS_TYPE= @ASSESS_TYPE  and ");
            sb.Append(" A.DEPT_NO in( "); 
	        sb.Append(" select distinct Y.DEPT_NO "); 
			sb.Append(" FROM "); 
			sb.Append(" 		TB_S_M_ASSESS_DEPT20_MA SA left join ");
            sb.Append(" 		(select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR, ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL ) X ON X.DEPT_NO=SA.DEPT_NO_20 AND X.ASSESS_YEAR=SA.ASSESS_YEAR AND X.ASSESS_TYPE=SA.ASSESS_TYPE left JOIN ");
            sb.Append(" 		TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR  and X.ASSESS_TYPE=Y.ASSESS_TYPE ");
            sb.Append(" where X.ASSESS_YEAR=E.ASSESS_YEAR and X.ASSESS_TYPE=E.ASSESS_TYPE and SA.MA_EMP_ID=@MA_EMP_ID   "); 
	        sb.Append("    )");


            if (ws_cd != "-1")
            {
                sb.Append(" and A.WS_CD  = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }

            if (grp_cd != "-1")
            {
                sb.Append(" and C.GRP_CD  = @GRP_CD ");
                ht.Add("@GRP_CD", grp_cd);
            }
            if (score_final != "-1")
            {
                sb.Append(" and A.SCORE_FINAL  = @SCORE_FINAL ");
                ht.Add("@SCORE_FINAL", score_final);
            }
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID  = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (recomm_desc != "-1")
            {
                sb.Append(" and A.RECOMM_DESC  = @RECOMM_DESC ");
                ht.Add("@RECOMM_DESC", recomm_desc);
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
    public DataTable getSituationData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string ma_emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY A.DEPT_NO_20 ) As RowNumber,");
            sb.Append(@" A.ASSESS_YEAR,A.ASSESS_TYPE,A.DEPT_NAME_20 DEPT_NAME  ,B.EMP_NAME,
                        case when (select count(*) from TB_S_M_ASSESS_DEP20_UP_SIGN where ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and DEPT_NO=A.DEPT_NO_20 and DEPT_LEVEL>10 and isnull(SIGN_YN,'N')!='Y')>0 then '未提' else'已提' end SIGN_YN,
		                (select sum(MNG_NUM)    from TB_S_M_ASSESS_DIRECTOR_H Where  ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and DEPT_NO in(
			                select distinct P.DEPT_NO from
				                  (select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE
				                   from TB_S_M_ASSESS_DEPT_LEVEL 
			                       where DEPT_NO=A.DEPT_NO_20 and ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE)O left join
			                      TB_S_M_ASSESS_DEPT_LEVEL P on SUBSTRING(P.LEVEL_RATE,1,len(O.LEVEL_RATE))=O.LEVEL_RATE  and O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE )
		                 ) MNG_NUM,
		                 (select count(*) from TB_S_M_ASSESS_TARGET X join TB_S_M_ASSESS_DIRECTOR_D Y on X.EMP_ID=Y.EMP_ID and  X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
                          where  X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE and isnull(X.SCORE_FINAL,'')<>'' and Y.DEPT_NO in(
			                select distinct P.DEPT_NO from
				                  (select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE
				                   from TB_S_M_ASSESS_DEPT_LEVEL 
			                       where DEPT_NO=A.DEPT_NO_20 and ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE)O left join
			                      TB_S_M_ASSESS_DEPT_LEVEL P on SUBSTRING(P.LEVEL_RATE,1,len(O.LEVEL_RATE))=O.LEVEL_RATE  and O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE )
		                 )MNG_GRANT_NUM,
		                 (select count(*) from TB_S_M_ASSESS_TARGET X join TB_S_M_ASSESS_DIRECTOR_D Y on X.EMP_ID=Y.EMP_ID and  X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
                          where  X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE and isnull(X.SCORE_FINAL,'')='A' and Y.DEPT_NO in(
			                select distinct P.DEPT_NO from
				                  (select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE
				                   from TB_S_M_ASSESS_DEPT_LEVEL 
			                       where DEPT_NO=A.DEPT_NO_20 and ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE)O left join
			                      TB_S_M_ASSESS_DEPT_LEVEL P on SUBSTRING(P.LEVEL_RATE,1,len(O.LEVEL_RATE))=O.LEVEL_RATE  and O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE  )
		                 )MNG_GRANT_A_NUM,
		                 (select count(*) from TB_S_M_ASSESS_TARGET X join TB_S_M_ASSESS_DIRECTOR_D Y on X.EMP_ID=Y.EMP_ID and  X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
                          where  X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE and isnull(X.SCORE_FINAL,'')='B' and Y.DEPT_NO in(
			                select distinct P.DEPT_NO from
				                  (select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR , ASSESS_TYPE
				                   from TB_S_M_ASSESS_DEPT_LEVEL 
			                       where DEPT_NO=A.DEPT_NO_20 and ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE)O left join
			                      TB_S_M_ASSESS_DEPT_LEVEL P on SUBSTRING(P.LEVEL_RATE,1,len(O.LEVEL_RATE))=O.LEVEL_RATE  and O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE  )
		                 )MNG_GRANT_B_NUM,
		                 (select count(*) from TB_S_M_ASSESS_TARGET X join TB_S_M_ASSESS_DIRECTOR_D Y on X.EMP_ID=Y.EMP_ID and  X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
                          where  X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE and isnull(X.SCORE_FINAL,'')='C' and Y.DEPT_NO in(
			                select distinct P.DEPT_NO from
				                  (select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR, ASSESS_TYPE 
				                   from TB_S_M_ASSESS_DEPT_LEVEL 
			                       where DEPT_NO=A.DEPT_NO_20 and ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE)O left join
			                      TB_S_M_ASSESS_DEPT_LEVEL P on SUBSTRING(P.LEVEL_RATE,1,len(O.LEVEL_RATE))=O.LEVEL_RATE  and O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE  )
		                 )MNG_GRANT_C_NUM,
		                 (select count(*) from TB_S_M_ASSESS_TARGET X join TB_S_M_ASSESS_DIRECTOR_D Y on X.EMP_ID=Y.EMP_ID and  X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
                          where  X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE and isnull(X.SCORE_FINAL,'')='D' and Y.DEPT_NO in(
			                select distinct P.DEPT_NO from
				                  (select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE
				                   from TB_S_M_ASSESS_DEPT_LEVEL 
			                       where DEPT_NO=A.DEPT_NO_20 and ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE)O left join
			                      TB_S_M_ASSESS_DEPT_LEVEL P on SUBSTRING(P.LEVEL_RATE,1,len(O.LEVEL_RATE))=O.LEVEL_RATE  and O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE  )
		                 )MNG_GRANT_D_NUM,
		                 (select count(*) from TB_S_M_ASSESS_TARGET X join TB_S_M_ASSESS_DIRECTOR_D Y on X.EMP_ID=Y.EMP_ID and  X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
                          where  X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE and isnull(X.SCORE_FINAL,'')='E' and Y.DEPT_NO in(
			                select distinct P.DEPT_NO from
				                  (select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE
				                   from TB_S_M_ASSESS_DEPT_LEVEL 
			                       where DEPT_NO=A.DEPT_NO_20 and ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE)O left join
			                      TB_S_M_ASSESS_DEPT_LEVEL P on SUBSTRING(P.LEVEL_RATE,1,len(O.LEVEL_RATE))=O.LEVEL_RATE  and O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE )
		                 )MNG_GRANT_E_NUM
                from TB_S_M_ASSESS_DEPT20_MA A left join
                     TB_H_M_EMP B on A.EMP_ID_DEPT20 =B.EMP_ID
                where A.MA_EMP_ID=@MA_EMP_ID and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE");
           
          
            
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
    public int getSituationCount(int startRowIndex, int maximumRows, string assess_year, string assess_type, string ma_emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
                sb.Append(" select COUNT(A.DEPT_NO_20) total_record  ");
                sb.Append(@"  from TB_S_M_ASSESS_DEPT20_MA A left join
                                    TB_H_M_EMP B on A.EMP_ID_DEPT20 =B.EMP_ID
                              where A.MA_EMP_ID=@MA_EMP_ID and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE");
            
           
            

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
    public DataTable getApproveData(String assess_year, String assess_type, String dept_no, String ws_cd, String score_level_group)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" SELECT A.ASSESS_YEAR, A.ASSESS_TYPE, A.EMP_ID, A.EMP_NAME, isnull(A.IS_OUT,'N') IS_OUT,
	                           A.LEVEL_CD, A.WS_CD,  A.WS_CD+'-'+B.SUB_DESC as WS_CD_DESC, A.SCORE_DEPT, 
	                            isnull((select sum(MNG_GRADE) from TB_S_M_ASSESS_SCORE WHERE ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID),0) MNG_GRADE,
	                           (SELECT case when Count(*)>0 then Count(*)-1 else Count(*) end FROM TB_S_M_ASSESS_LOG WHERE ASSESS_YEAR=A.ASSESS_YEAR and  ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID) FIX_COUNT,
	                            (CASE WHEN A.ASSESS_TYPE='1' then A.SCORE_1H_1 else A.SCORE_2H_1 end) SCORE_H_1,
	                            (CASE WHEN A.ASSESS_TYPE='1' then A.SCORE_1H_2 else A.SCORE_2H_2 end) SCORE_H_2,
		                        A.RECENT_LEVEL_WORK_YEARS, A.AGE, A.WORK_YEARS,isnull(A.DISTING_REMARK,'') DISTING_REMARK,
		                        (A.LEAVE_O+A.LEAVE_P) LEAVE_OP, A.LEAVE_Q, (A.LEAVE_A+A.LEAVE_B) LEAVE_AB
                         FROM TB_S_M_ASSESS_TARGET A left join
                               TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='HB' and B.MAIN_CD='WS_CD' and B.SUB_CD= A.WS_CD and B.IS_VALID='Y'   
                         where A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE  and
                              A.DEPT_NO in(
	                           SELECT Y.DEPT_NO
		                        FROM
		                         (select LEVEL_RATE,ASSESS_YEAR,ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=@DEPT_NO and DEPT_LEVEL>='20') X left join 
		                         TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE and X.ASSESS_TYPE=Y.ASSESS_TYPE and X.ASSESS_YEAR=Y.ASSESS_YEAR 
                                 where X.ASSESS_TYPE=A.ASSESS_TYPE and X.ASSESS_YEAR=A.ASSESS_YEAR 
	                     ) ");


            if (ws_cd != "")
            {
                sb.Append(" and A.WS_CD  = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }

            if (score_level_group != "")
            {
                String[] slgArray = score_level_group.Split('/');
                sb.Append(" and (");
                for (int i = 0; i < slgArray.Length; i++)
                {
                    if (i > 0) sb.Append(" or ");
                    sb.Append(" A.LEVEL_CD='" + slgArray[i].ToString() + "'");
                }

                sb.Append(" )");
            }

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO", DEPT_NO);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    public DataTable getConfirmData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string ma_emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY A.RECOMM_DESC, A.LEVEL_CD ) As RowNumber,");
            sb.Append(@"  A.ASSESS_YEAR, A.ASSESS_TYPE, A.EMP_ID, A.DEPT_NAME, A.EMP_NAME, A.LEVEL_CD, A.PJOB_DESC,A.WS_CD, A.RECOMM_DESC,
                          isnull((SELECT sum(MNG_GRADE) FROM TB_S_M_ASSESS_SCORE WHERE ASSESS_YEAR = A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID),0) MNG_GRADE,
	                      A.SCORE_FINAL, isnull(COMMENTS,'') COMMENTS
                         FROM   TB_S_M_ASSESS_DIRECTOR_D E left join
                                TB_S_M_ASSESS_TARGET A  on E.ASSESS_YEAR=A.ASSESS_YEAR and E.ASSESS_TYPE=A.ASSESS_TYPE and E.EMP_ID=A.EMP_ID  
                         WHERE  E.ASSESS_YEAR=@ASSESS_YEAR and E.ASSESS_TYPE=@ASSESS_TYPE and isnull(A.RECOMM_DESC,'')<>'' and
                                E.DEPT_NO in(
	                                        select Q.DEPT_NO  
	                                        from TB_S_M_ASSESS_DEPT20_MA O left join   
		                                        TB_S_M_ASSESS_DEPT_LEVEL P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE = P.ASSESS_TYPE and O.DEPT_NO_20=P.DEPT_NO and P.DEPT_LEVEL>='20' left join  
		                                        TB_S_M_ASSESS_DEPT_LEVEL Q on P.ASSESS_YEAR=Q.ASSESS_YEAR and P.ASSESS_TYPE = Q.ASSESS_TYPE and SUBSTRING(Q.LEVEL_RATE,1,len(P.LEVEL_RATE))=P.LEVEL_RATE  
	                                        where O.MA_EMP_ID=@MA_EMP_ID and O.ASSESS_YEAR= E.ASSESS_YEAR and O.ASSESS_TYPE= E.ASSESS_TYPE
                         )  ");
            
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

    public int getConfirmCount(int startRowIndex, int maximumRows, string assess_year, string assess_type, string ma_emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(@" FROM TB_S_M_ASSESS_DIRECTOR_D E left join
                              TB_S_M_ASSESS_TARGET A  on E.ASSESS_YEAR=A.ASSESS_YEAR and E.ASSESS_TYPE=A.ASSESS_TYPE and E.EMP_ID=A.EMP_ID 
                         WHERE  E.ASSESS_YEAR=@ASSESS_YEAR and E.ASSESS_TYPE=@ASSESS_TYPE and isnull(A.RECOMM_DESC,'')<>'' and
                                A.DEPT_NO in(
	                                        select Q.DEPT_NO  
	                                        from TB_S_M_ASSESS_DEPT20_MA O left join   
		                                        TB_S_M_ASSESS_DEPT_LEVEL P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE = P.ASSESS_TYPE and O.DEPT_NO_20=P.DEPT_NO and P.DEPT_LEVEL>='20' left join  
		                                        TB_S_M_ASSESS_DEPT_LEVEL Q on P.ASSESS_YEAR=Q.ASSESS_YEAR and P.ASSESS_TYPE = Q.ASSESS_TYPE and SUBSTRING(Q.LEVEL_RATE,1,len(P.LEVEL_RATE))=P.LEVEL_RATE  
	                                        where O.MA_EMP_ID=@MA_EMP_ID and O.ASSESS_YEAR= E.ASSESS_YEAR and O.ASSESS_TYPE= E.ASSESS_TYPE
                         )  ");

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
    //取得員工考核資料資料
    public DataTable getEmpTargetData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" Select 
                         D.DEPT_NO,D.DIREC_EMP_ID,E.EMP_NAME DIREC_EMP_NAME,D.DEPT_FULL_NAME,
                         A.ASSESS_YEAR, A.ASSESS_TYPE, (A.ASSESS_TYPE+'-'+F.SUB_DESC) ASSESS_TYPE_DESC , A.EMP_ID, B.EMP_NAME, isnull(B.IS_OUT,'N') IS_OUT,
	                    B.LEVEL_CD, B.WS_CD,  B.WS_CD+'-'+C.SUB_DESC as WS_CD_DESC,B.PJOB_DESC,
	                    isnull((select sum(MNG_GRADE) from TB_S_M_ASSESS_SCORE WHERE ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID),0) MNG_GRADE,
	                    SCORE_DEPT,B.SCORE_1H_1,B.SCORE_1H_2,B.SCORE_1H_3,B.SCORE_2H_1,B.SCORE_2H_2,B.SCORE_2H_3,
	                    B.RECENT_LEVEL_WORK_YEARS, B.AGE, B.WORK_YEARS,isnull(B.DISTING_REMARK,'') DISTING_REMARK,
						isnull(B.LEAVE_O+B.LEAVE_P,0)LEAVE_OP,LEAVE_Q,isnull(B.LEAVE_A+B.LEAVE_B,0)LEAVE_AB,isnull(B.LIMIT_RATE,'') LIMIT_RATE
                        FROM TB_S_M_ASSESS_DIRECTOR_D A left join
							 TB_S_M_ASSESS_DIRECTOR_H D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.DEPT_NO=D.DEPT_NO left join
	                         TB_S_M_ASSESS_TARGET B on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.EMP_ID=B.EMP_ID left join
                             TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= B.WS_CD and C.IS_VALID='Y'  left join
							 TB_H_M_EMP E on D.DIREC_EMP_ID=E.EMP_ID left join 
							 TB_9_M_COMM_D F  with (nolock)  on F.SYS_CD='SJ' and F.MAIN_CD='ASSESS_TYPE' and F.SUB_CD= A.ASSESS_TYPE and F.IS_VALID='Y' ");


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
            sb.Append(@" select RATE_A,RATE_B,RATE_C,RATE_D,RATE_E from TB_S_M_ASSESS_RATE where ASSESS_TYPE=@ASSESS_TYPE and WS_CD=@WS_CD and LEVEL_CD=@LEVEL_CD
                          union
                          select RATE_A,RATE_B,RATE_C,RATE_D,RATE_E from TB_S_M_ASSESS_RATE where ASSESS_TYPE=@ASSESS_TYPE and WS_CD=@WS_CD  and LEVEL_CD=''");

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
    public DataTable getEmpAssessScore(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type,  string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY ITEM_CD ) As RowNumber,");
            sb.Append(@" ASSESS_YEAR,ASSESS_TYPE, EMP_ID, ITEM_GROUP, ITEM_CD, ITEM_DESC, MAX_GRADE, MNG_GRADE 
                        from TB_S_M_ASSESS_SCORE   
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
            sb.Append(@"  from TB_S_M_ASSESS_SCORE   
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
    //更新 TB_S_M_ASSESS_SCORE
    public void updateSCORE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_ASSESS_SCORE ");
            sb.Append(" set MNG_GRADE = @MNG_GRADE, ");
            sb.Append("     UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE and ");
            sb.Append("       EMP_ID=@EMP_ID and ITEM_CD =@ITEM_CD ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@ITEM_CD", ITEM_CD);
            ht.Add("@MNG_GRADE", MNG_GRADE);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新 TB_S_M_ASSESS_TARGET
    public void updateTARGET()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_ASSESS_TARGET ");
            sb.Append(" set RECOMM_DESC = @RECOMM_DESC, COMMENTS = @COMMENTS, SCORE_DEPT=@SCORE_DEPT, SCORE_FINAL=@SCORE_FINAL, ");
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

    public DataTable statisticsData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select distinct X.* from ");
            sb.Append("( ");
            sb.Append("select B.WS_CD,C.SUB_DESC WS_CD_DESC,A.GRP_CD, B.GRP_NAME,'b' RATE_TYPE,'基準' RATE_TYPE_DESC,BASE_A BA, BASE_B BB, BASE_C BC, BASE_D BD, BASE_E BE, BASE_TOT BTOT,");
            sb.Append("       REAL_A RA, REAL_B RB, REAL_C RC, REAL_D RD, REAL_E RE, REAL_TOTAL RTOT,'' CHECK_OK, B.IS_CTL ");
            sb.Append("from TB_S_M_ASSESS_MA_PEO A  left join ");
            sb.Append("	    TB_S_M_ASSESS_GROUP_H B  with (nolock)  on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.GRP_CD=B.GRP_CD left join ");
            sb.Append("	    TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= B.WS_CD and C.IS_VALID='Y'  ");
            sb.Append("where A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and A.MA_EMP_ID=@MA_EMP_ID  ");
            if (WS_CD != "-1")
            {
                sb.Append(" and B.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", WS_CD);
            }
            if (GRP_CD != "-1")
            {

                sb.Append(" and A.GRP_CD = @GRP_CD ");
                ht.Add("@GRP_CD", GRP_CD);
            }
            
            sb.Append(")X order by X.GRP_CD,X.RATE_TYPE ");
            
           
           
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@MA_EMP_ID", MA_EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable statisticsOutData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select distinct X.* from ");
            sb.Append("( ");
            sb.Append(" select B.WS_CD,C.SUB_DESC WS_CD_DESC,'o' RATE_TYPE,'外數' RATE_TYPE_DESC,sum(OUT_REAL_A) RA, sum(OUT_REAL_B) RB, sum(OUT_REAL_C) RC, sum(OUT_REAL_D) RD, sum(OUT_REAL_E) RE, sum(OUT_REAL_TOTAL) RTOT  ");
            sb.Append("from TB_S_M_ASSESS_MA_PEO A  left join ");
            sb.Append("	    TB_S_M_ASSESS_GROUP_H B  with (nolock)  on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.GRP_CD=B.GRP_CD left join ");
            sb.Append("	    TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= B.WS_CD and C.IS_VALID='Y'  ");
            sb.Append("where A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and A.MA_EMP_ID=@MA_EMP_ID  ");
            if (WS_CD != "-1")
            {
                sb.Append(" and B.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", WS_CD);
            }
            if (GRP_CD != "-1")
            {

                sb.Append(" and A.GRP_CD = @GRP_CD ");
                ht.Add("@GRP_CD", GRP_CD);
            }
            sb.Append(" GROUP BY B.WS_CD,C.SUB_DESC )X order by X.WS_CD ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@MA_EMP_ID", MA_EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable referData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select  E.EMP_NAME DIREC_EMP_NAME, ");
            sb.Append("      isnull((select sum(MNG_GRADE) from TB_S_M_ASSESS_SCORE WHERE ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID),0) MNG_GRADE, ");
            sb.Append("      B.SCORE_DEPT, B.SCORE_FINAL, isnull(B.IS_OUT,'N') IS_OUT, B.DISTING_REMARK, A.EMP_ID, B.EMP_NAME, D.DEPT_NO,  ");
            sb.Append("      F.DEPT_NO_20, F.DEPT_NAME_20, F.DEPT_NO_30, F.DEPT_NAME_30, F.DEPT_NO_40, F.DEPT_NAME_40, F.DEPT_NO_50, F.DEPT_NAME_50, F.DEPT_NO_60, F.DEPT_NAME_60, F.DEPT_NO_70, F.DEPT_NAME_70, ");
            sb.Append("      B.WS_CD,  B.WS_CD+'-'+C.SUB_DESC as WS_CD_DESC,B.LEVEL_CD, B.PJOB_CD, B.PJOB_DESC, B.RECENT_LEVEL_WORK_YEARS, B.WORK_YEARS, B.AGE, B.EMP_CHG_CD, G.SUB_DESC EMP_CHG_CD_DESC, ");
            sb.Append("      B.SCORE_1H_1, B.SCORE_1H_2, B.SCORE_1H_3, B.SCORE_2H_1, B.SCORE_2H_2, B.SCORE_2H_3,  ");
            sb.Append("      B.OVERTIME_HOUR_MEAN, B.LEAVE_O, B.LEAVE_P, B.LEAVE_Q, B.LEAVE_A, B.LEAVE_B, B.RETENTION_DAYS,  ");
            sb.Append("      B.THIRD_CNT_P, B.SECOND_CNT_P, B.FIRST_CNT_P, B.THIRD_CNT_M, B.SECOND_CNT_M, B.FIRST_CNT_M,  ");
            sb.Append("      B.PROPOSAL_TOTAL, B.PROPOSAL_GRADE, B.PROPOSAL_GRADE_MEAN, B.PROPOSAL_6 ");
            sb.Append("FROM TB_S_M_ASSESS_TARGET B left join ");
            sb.Append("     TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= B.WS_CD and C.IS_VALID='Y' left join ");
            sb.Append("     TB_S_M_ASSESS_DIRECTOR_D A  with (nolock)  on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.EMP_ID=B.EMP_ID left join   ");
            sb.Append("     TB_S_M_ASSESS_DIRECTOR_H D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.DEPT_NO = D.DEPT_NO left join ");
            sb.Append("     TB_H_M_EMP E  with (nolock) on D.DIREC_EMP_ID=E.EMP_ID left join ");
            sb.Append("     TB_H_R_DEPT_DATA_AD F  with (nolock) on A.DEPT_NO = F.DEPT_NO left join ");
            sb.Append("     TB_9_M_COMM_D G  with (nolock)  on G.SYS_CD='HB' and G.MAIN_CD='EMP_CHG_CD' and G.SUB_CD= B.EMP_CHG_CD and G.IS_VALID='Y' ");
            sb.Append("where B.ASSESS_YEAR=@ASSESS_YEAR and B.ASSESS_TYPE= @ASSESS_TYPE  and A.DEPT_NO IS NOT NULL and ");
            sb.Append("      B.DEPT_NO in( ");
            sb.Append(" select Q.DEPT_NO  ");
            sb.Append(" from TB_S_M_ASSESS_DEPT20_MA O left join   ");
            sb.Append(" 	 TB_S_M_ASSESS_DEPT_LEVEL P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE = P.ASSESS_TYPE and O.DEPT_NO_20=P.DEPT_NO and P.DEPT_LEVEL>='20' left join  ");
            sb.Append(" 	 TB_S_M_ASSESS_DEPT_LEVEL Q on P.ASSESS_YEAR=Q.ASSESS_YEAR and P.ASSESS_TYPE = Q.ASSESS_TYPE and SUBSTRING(Q.LEVEL_RATE,1,len(P.LEVEL_RATE))=P.LEVEL_RATE  ");
            sb.Append(" where O.MA_EMP_ID=@MA_EMP_ID and O.ASSESS_YEAR=B.ASSESS_YEAR and O.ASSESS_TYPE=B.ASSESS_TYPE  ");
            sb.Append("                        ) ");


            if (WS_CD != "-1")
            {
                sb.Append(" and B.WS_CD  = @WS_CD ");
                ht.Add("@WS_CD", WS_CD);
            }

            if (GRP_CD != "-1")
            {

                sb.Append(" and B.LEVEL_CD in( ");
               
                sb.Append(" select X.LEVEL_CD  ");
                sb.Append("  from TB_S_M_ASSESS_GROUP_D X join  ");
                sb.Append("       TB_S_M_ASSESS_GROUP_H Y on X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE and X.GRP_CD=Y.GRP_CD  ");
                sb.Append("  where X.ASSESS_YEAR=B.ASSESS_YEAR and  X.ASSESS_TYPE=B.ASSESS_TYPE and Y.WS_CD=B.WS_CD and X.GRP_CD=@GRP_CD  ");
                sb.Append(" )");
            }
            if (SCORE_FINAL != "-1")
            {
                sb.Append(" and B.SCORE_FINAL  = @SCORE_FINAL ");
                ht.Add("@SCORE_FINAL", SCORE_FINAL);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and B.EMP_ID  = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (RECOMM_DESC != "-1")
            {
                sb.Append(" and B.RECOMM_DESC  = @RECOMM_DESC ");
                ht.Add("@RECOMM_DESC", RECOMM_DESC);
            }
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@MA_EMP_ID", MA_EMP_ID);

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
            sb.Append("FROM TB_S_M_ASSESS_DIRECTOR_D A left join ");
            sb.Append("     TB_S_M_ASSESS_TARGET B on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.EMP_ID=B.EMP_ID left join ");
            sb.Append("     TB_S_M_ASSESS_DIRECTOR_H D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.DEPT_NO = D.DEPT_NO  ");
            sb.Append("where  A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE and A.EMP_ID<>D.DIREC_EMP_ID  and isnull(B.SCORE_DEPT,'')='' ");
            sb.Append("       and A.DEPT_NO in (");
            sb.Append(" select Q.DEPT_NO  ");
            sb.Append(" from TB_S_M_ASSESS_DEPT20_MA O left join   ");
            sb.Append(" 	 TB_S_M_ASSESS_DEPT_LEVEL P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE = P.ASSESS_TYPE and O.DEPT_NO_20=P.DEPT_NO and P.DEPT_LEVEL>='20' left join  ");
            sb.Append(" 	 TB_S_M_ASSESS_DEPT_LEVEL Q on P.ASSESS_YEAR=Q.ASSESS_YEAR and P.ASSESS_TYPE = Q.ASSESS_TYPE and SUBSTRING(Q.LEVEL_RATE,1,len(P.LEVEL_RATE))=P.LEVEL_RATE  ");
            sb.Append(" where O.MA_EMP_ID=@MA_EMP_ID and O.ASSESS_YEAR=B.ASSESS_YEAR and O.ASSESS_TYPE=B.ASSESS_TYPE  ");
            sb.Append(")");
            
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@MA_EMP_ID", MA_EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable getDEPT20SignStatus()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"select B.*
                        FROM TB_S_M_ASSESS_DEP20_UP_SIGN A join
                             TB_S_M_ASSESS_DEP20_UP_SIGN B on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.DEPT_NO=B.DEPT_NO and B.DEPT_LEVEL='20' 
                        Where A.EMP_ID=@MA_EMP_ID and B.SIGN_YN<>'Y' and  A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE ");
           

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@MA_EMP_ID", MA_EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable getMAWSCD(String assess_year, String assess_type, String ma_emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
           
          
            sb.Append(" SELECT distinct H.WS_CD,B.SUB_DESC WS_CD_DESC FROM TB_S_M_ASSESS_GROUP_H H join  ");
            sb.Append(" TB_S_M_ASSESS_MA_PEO P on H.GRP_CD=P.GRP_CD join ");
            sb.Append(" TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='HB' and B.MAIN_CD='WS_CD' and B.SUB_CD= H.WS_CD and B.IS_VALID='Y'  ");

            sb.Append(" where P.MA_EMP_ID=@MA_EMP_ID and P.ASSESS_YEAR=@ASSESS_YEAR and P.ASSESS_TYPE=@ASSESS_TYPE ");


            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@MA_EMP_ID", ma_emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getMAGRPCD(String assess_year, String assess_type, String ma_emp_id, String ws_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append(" SELECT distinct H.GRP_CD,H.GRP_NAME FROM TB_S_M_ASSESS_GROUP_H H join  ");
            sb.Append(" TB_S_M_ASSESS_MA_PEO P on H.GRP_CD=P.GRP_CD ");

            sb.Append(" where P.MA_EMP_ID=@MA_EMP_ID and P.ASSESS_YEAR=@ASSESS_YEAR and P.ASSESS_TYPE=@ASSESS_TYPE  and H.WS_CD=@WS_CD");


            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@MA_EMP_ID", ma_emp_id);
            ht.Add("@WS_CD", ws_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getUpdMAPeoData(String assess_year, String assess_type, String ma_emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append(" Select O.ASSESS_YEAR,O.ASSESS_TYPE, O.GRP_CD , ");
            sb.Append("        O.BASE_A,O.BASE_B, O.BASE_C,O.BASE_D, O.BASE_E, O.BASE_TOT, ");
            sb.Append("        O.REAL_A,O.REAL_B, O.REAL_C,O.REAL_D, O.REAL_E, O.REAL_TOTAL, ");
            sb.Append("        O.OUT_REAL_A,O.OUT_REAL_B, O.OUT_REAL_C,O.OUT_REAL_D, O.OUT_REAL_E, O.OUT_REAL_TOTAL, ");
            sb.Append("       (SELECT ISNULL(IS_CTL,'Y')IS_CTL FROM TB_S_M_ASSESS_GROUP_H WHERE ASSESS_YEAR= O.ASSESS_YEAR AND ASSESS_TYPE= O.ASSESS_TYPE AND GRP_CD=O.GRP_CD) IS_CTL  ");
            sb.Append(" from TB_S_M_ASSESS_MA_PEO O ");
            sb.Append(" where O.MA_EMP_ID=@MA_EMP_ID and O.ASSESS_YEAR=@ASSESS_YEAR and O.ASSESS_TYPE=@ASSESS_TYPE ");


            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@MA_EMP_ID", ma_emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新主管簽核TB_S_M_ASSESS_MA_UP_SIGN
    public void updateMA_UP_SIGN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_ASSESS_DEP20_UP_SIGN ");
            sb.Append(" set SIGN_YN=@SIGN_YN, ");
            sb.Append("     UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE and ");
            sb.Append("       EMP_ID=@EMP_ID  ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SIGN_YN", SIGN_YN);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //呼叫更新部長人數配置檔
    internal void execSP_S_ASSESS_MA_APPROVE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_MA_APPROVE");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SCORE_FINAL", SCORE_FINAL);
            ht.Add("@SUGGEST_DESC", SUGGEST_DESC);
            ht.Add("@RECOMM_DESC", RECOMM_DESC);
            ht.Add("@COMMENTS", COMMENTS);
            ht.Add("@MA_TYPE", MA_TYPE);
            ht.Add("@USERID", CREATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SJ052");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }
    //取得要望書數量
    public int getEmpSuggestCount(String assess_year, String assess_type, String ma_emp_id, String apprve_flag1, String apprve_flag2, String apprve_flag3)
    {

        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*) total_record ");
            sb.Append(" from TB_S_M_ASSESS_EMP_SUGGEST A left join ");
            sb.Append("      TB_S_M_ASSESS_DIRECTOR_D B on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.EMP_ID=B.EMP_ID ");
            sb.Append(" where A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and ");
            sb.Append(" B.DEPT_NO in( ");
            sb.Append(" SELECT Y.DEPT_NO ");
            sb.Append(" FROM ");
            sb.Append("  TB_S_M_ASSESS_DEP20_UP_SIGN Z left join ");
            sb.Append("  (select ASSESS_YEAR, ASSESS_TYPE, LEVEL_RATE, DEPT_NO from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X  on Z.ASSESS_YEAR=X.ASSESS_YEAR and Z.ASSESS_TYPE =X.ASSESS_TYPE and Z.DEPT_NO=X.DEPT_NO left join  ");
            sb.Append("     TB_S_M_ASSESS_DEPT_LEVEL Y on X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE =Y.ASSESS_TYPE AND SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  ");
            sb.Append(" WHERE Z.EMP_ID=@MA_EMP_ID and Z.ASSESS_YEAR=A.ASSESS_YEAR and Z.ASSESS_TYPE=A.ASSESS_TYPE ");
            sb.Append("    )");


            if (apprve_flag1 != "")
            {
                sb.Append(" and isnull(A.AUDRESULT1_YN ,'X') ='X'  ");
                //ht.Add("@APPRVE_FLAG1", apprve_flag1);
            }

            if (apprve_flag2 != "")
            {
                sb.Append(" and isnull(A.AUDRESULT2_YN ,'X') ='X'  ");
                //ht.Add("@APPRVE_FLAG2", apprve_flag2);
            }
            if (apprve_flag3 != "")
            {
                sb.Append(" and isnull(A.AUDRESULT3_YN ,'X') ='X' ");
                //ht.Add("@APPRVE_FLAG3", apprve_flag3);
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
    public DataTable getWSApproveData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string ma_emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select X.* From (");
            sb.Append(@" select W1.WS_CD, W2.SUB_DESC WS_CD_DESC, W1.GRP_CD, W1.GRP_NAME,
                                (W1.WS_CD+'-'+W2.SUB_DESC+'-'+W1.GRP_CD+'-'+ W1.GRP_NAME) ARG,
                               (CASE WHEN W1.WS_CD='S' THEN 1 ELSE  CASE WHEN W1.WS_CD='G' THEN 2 ELSE CASE WHEN W1.WS_CD='N' THEN 3 ELSE CASE WHEN W1.WS_CD='W' THEN 4 ELSE 5 END END END END) WS_SORT,
		                        (SELECT COUNT(T2.EMP_ID)
		                            FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                                TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                            WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@MA_EMP_ID AND
					                        T2.LEVEL_CD IN(SELECT LEVEL_CD FROM TB_S_M_ASSESS_GROUP_D WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=W1.ASSESS_TYPE AND GRP_CD=W1.GRP_CD) AND
			                                T2.WS_CD=W1.WS_CD AND
			                                T1.DEPT_NO IN(
			                                select distinct Y.DEPT_NO
					                        FROM
							                        TB_S_M_ASSESS_DEPT20_MA SA left join
							                        (select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR, ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL ) X ON X.DEPT_NO=SA.DEPT_NO_20 AND X.ASSESS_YEAR=SA.ASSESS_YEAR AND X.ASSESS_TYPE=SA.ASSESS_TYPE  left JOIN 
							                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR  and X.ASSESS_TYPE=Y.ASSESS_TYPE
 					                        where X.ASSESS_YEAR=W1.ASSESS_YEAR and X.ASSESS_TYPE=W1.ASSESS_TYPE and SA.MA_EMP_ID=@MA_EMP_ID

			                                )) EMP_TOTAL,
		                        (SELECT COUNT(T2.EMP_ID)
		                            FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                                TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                            WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@MA_EMP_ID AND
					                        T2.LEVEL_CD IN(SELECT LEVEL_CD FROM TB_S_M_ASSESS_GROUP_D WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=W1.ASSESS_TYPE AND GRP_CD=W1.GRP_CD) AND
			                                T2.WS_CD=W1.WS_CD AND T2.SCORE_FINAL='A' AND
			                                T1.DEPT_NO IN(
			                                select distinct Y.DEPT_NO
					                        FROM
							                        TB_S_M_ASSESS_DEPT20_MA SA left join
							                        (select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR, ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL ) X ON X.DEPT_NO=SA.DEPT_NO_20  AND X.ASSESS_YEAR=SA.ASSESS_YEAR AND X.ASSESS_TYPE=SA.ASSESS_TYPE left JOIN 
							                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR  and X.ASSESS_TYPE=Y.ASSESS_TYPE
 					                        where X.ASSESS_YEAR=W1.ASSESS_YEAR and X.ASSESS_TYPE=W1.ASSESS_TYPE and SA.MA_EMP_ID=@MA_EMP_ID

			                                )) SCORE_A ,
		                        (SELECT COUNT(T2.EMP_ID)
		                            FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                                TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                            WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@MA_EMP_ID AND
					                        T2.LEVEL_CD IN(SELECT LEVEL_CD FROM TB_S_M_ASSESS_GROUP_D WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=W1.ASSESS_TYPE AND GRP_CD=W1.GRP_CD) AND
			                                T2.WS_CD=W1.WS_CD AND T2.SCORE_FINAL='B' AND
			                                T1.DEPT_NO IN(
			                                select distinct Y.DEPT_NO
					                        FROM
							                        TB_S_M_ASSESS_DEPT20_MA SA left join
							                        (select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR, ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL ) X ON X.DEPT_NO=SA.DEPT_NO_20  AND X.ASSESS_YEAR=SA.ASSESS_YEAR AND X.ASSESS_TYPE=SA.ASSESS_TYPE left JOIN 
							                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR  and X.ASSESS_TYPE=Y.ASSESS_TYPE
 					                        where X.ASSESS_YEAR=W1.ASSESS_YEAR and X.ASSESS_TYPE=W1.ASSESS_TYPE and SA.MA_EMP_ID=@MA_EMP_ID

			                                )) SCORE_B ,
		                        (SELECT COUNT(T2.EMP_ID)
		                            FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                                TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                            WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@MA_EMP_ID AND
					                        T2.LEVEL_CD IN(SELECT LEVEL_CD FROM TB_S_M_ASSESS_GROUP_D WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=W1.ASSESS_TYPE AND GRP_CD=W1.GRP_CD) AND
			                                T2.WS_CD=W1.WS_CD AND T2.SCORE_FINAL='C' AND
			                                T1.DEPT_NO IN(
			                                select distinct Y.DEPT_NO
					                        FROM
							                        TB_S_M_ASSESS_DEPT20_MA SA left join
							                        (select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR, ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL ) X ON X.DEPT_NO=SA.DEPT_NO_20  AND X.ASSESS_YEAR=SA.ASSESS_YEAR AND X.ASSESS_TYPE=SA.ASSESS_TYPE  left JOIN 
							                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR  and X.ASSESS_TYPE=Y.ASSESS_TYPE
 					                        where X.ASSESS_YEAR=W1.ASSESS_YEAR and X.ASSESS_TYPE=W1.ASSESS_TYPE and SA.MA_EMP_ID=@MA_EMP_ID

			                                )) SCORE_C,
		                        (SELECT COUNT(T2.EMP_ID)
		                            FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                                TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                            WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@MA_EMP_ID AND
					                        T2.LEVEL_CD IN(SELECT LEVEL_CD FROM TB_S_M_ASSESS_GROUP_D WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=W1.ASSESS_TYPE AND GRP_CD=W1.GRP_CD) AND
			                                T2.WS_CD=W1.WS_CD AND T2.SCORE_FINAL='D' AND
			                                T1.DEPT_NO IN(
			                                select distinct Y.DEPT_NO
					                        FROM
							                        TB_S_M_ASSESS_DEPT20_MA SA left join
							                        (select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR, ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL ) X ON X.DEPT_NO=SA.DEPT_NO_20 AND X.ASSESS_YEAR=SA.ASSESS_YEAR AND X.ASSESS_TYPE=SA.ASSESS_TYPE  left JOIN 
							                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR  and X.ASSESS_TYPE=Y.ASSESS_TYPE
 					                        where X.ASSESS_YEAR=W1.ASSESS_YEAR and X.ASSESS_TYPE=W1.ASSESS_TYPE and SA.MA_EMP_ID=@MA_EMP_ID

			                                )) SCORE_D,
		                        (SELECT COUNT(T2.EMP_ID)
		                            FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                                TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                            WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@MA_EMP_ID AND
					                        T2.LEVEL_CD IN(SELECT LEVEL_CD FROM TB_S_M_ASSESS_GROUP_D WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=W1.ASSESS_TYPE AND GRP_CD=W1.GRP_CD) AND
			                                T2.WS_CD=W1.WS_CD AND T2.SCORE_FINAL='E' AND
			                                T1.DEPT_NO IN(
			                                select distinct Y.DEPT_NO
					                        FROM
							                        TB_S_M_ASSESS_DEPT20_MA SA left join
							                        (select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR, ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL ) X ON X.DEPT_NO=SA.DEPT_NO_20  AND X.ASSESS_YEAR=SA.ASSESS_YEAR AND X.ASSESS_TYPE=SA.ASSESS_TYPE left JOIN 
							                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR  and X.ASSESS_TYPE=Y.ASSESS_TYPE
 					                        where X.ASSESS_YEAR=W1.ASSESS_YEAR and X.ASSESS_TYPE=W1.ASSESS_TYPE and SA.MA_EMP_ID=@MA_EMP_ID

			                                )) SCORE_E   
                        from   TB_S_M_ASSESS_GROUP_H W1 LEFT JOIN 
                               TB_9_M_COMM_D W2 ON W1.WS_CD=W2.SUB_CD AND W2.SYS_CD='HB' and W2.MAIN_CD='WS_CD'
                        WHERE  W1.ASSESS_YEAR=@ASSESS_YEAR AND W1.ASSESS_TYPE=@ASSESS_TYPE
                    ");
            sb.Append(" )X WHERE X.EMP_TOTAL>0 ");
            sb.Append(" ORDER BY X.WS_SORT ");

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@MA_EMP_ID", ma_emp_id);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getWSApproveCount(int startRowIndex, int maximumRows, string assess_year, string assess_type, string ma_emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNt(X.GRP_CD) total_record From (");
            sb.Append(@" select W1.WS_CD, W2.SUB_DESC WS_CD_DESC, W1.GRP_CD, W1.GRP_NAME,
                                (SELECT COUNT(T2.EMP_ID)
		                            FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                                TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                            WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@MA_EMP_ID AND
					                        T2.LEVEL_CD IN(SELECT LEVEL_CD FROM TB_S_M_ASSESS_GROUP_D WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=W1.ASSESS_TYPE AND GRP_CD=W1.GRP_CD) AND
			                                T2.WS_CD=W1.WS_CD AND
			                                T1.DEPT_NO IN(
			                                select distinct Y.DEPT_NO
					                        FROM
							                        TB_S_M_ASSESS_DEPT20_MA SA left join
							                        (select DEPT_NO,LEVEL_RATE ,ASSESS_YEAR, ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL ) X ON X.DEPT_NO=SA.DEPT_NO_20  left JOIN 
							                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR  and X.ASSESS_TYPE=Y.ASSESS_TYPE
 					                        where X.ASSESS_YEAR=W1.ASSESS_YEAR and X.ASSESS_TYPE=W1.ASSESS_TYPE and SA.MA_EMP_ID=@MA_EMP_ID

			                                )) EMP_TOTAL	
                        from   TB_S_M_ASSESS_GROUP_H W1 LEFT JOIN 
                               TB_9_M_COMM_D W2 ON W1.WS_CD=W2.SUB_CD AND W2.SYS_CD='HB' and W2.MAIN_CD='WS_CD'
                        WHERE  W1.ASSESS_YEAR=@ASSESS_YEAR AND W1.ASSESS_TYPE=@ASSESS_TYPE ");
            sb.Append(" )X WHERE X.EMP_TOTAL>0 ");


            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@MA_EMP_ID", ma_emp_id);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);

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
    public DataTable statisticsData_Level_01(string reportType)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"SELECT T2.WS_CD, T3.SUB_DESC WS_CD_NAME, T1.GRP_CD, T2.GRP_NAME,
                               T1.BASE_A, T1.BASE_B, T1.BASE_C, T1.BASE_D, T1.BASE_E, T1.BASE_TOT ,T2.IS_CTL
                        FROM TB_S_M_ASSESS_MA_PEO T1  LEFT JOIN
                             TB_S_M_ASSESS_GROUP_H T2 ON T1.ASSESS_YEAR = T2.ASSESS_YEAR AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.GRP_CD=T2.GRP_CD LEFT JOIN
	                         TB_9_M_COMM_D T3  with (nolock)  on T3.SYS_CD='HB' and T3.MAIN_CD='WS_CD' and T3.SUB_CD= T2.WS_CD and T3.IS_VALID='Y' 
                        WHERE T1.MA_EMP_ID=@MA_EMP_ID AND T1.ASSESS_YEAR=@ASSESS_YEAR AND T1.ASSESS_TYPE=@ASSESS_TYPE AND T2.REPORT_TYPE=@REPORT_TYPE AND T1.BASE_TOT>0
                        ORDER BY (CASE WHEN T2.WS_CD='S' THEN 1 ELSE CASE WHEN T2.WS_CD='G' THEN 2 ELSE CASE WHEN T2.WS_CD='N' THEN 3 ELSE 4 END END END),T1.GRP_CD");

           



            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@MA_EMP_ID", MA_EMP_ID);
            ht.Add("@REPORT_TYPE", reportType);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable statisticsData_Level_02(string wsCd,string grpCd )
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"SELECT X.* FROM (
                                SELECT W1.DEPT_NAME_20, W1.DEPT_NO_20,
	                                   (SELECT COUNT(T2.EMP_ID)
		                                        FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                                            TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                                        WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE  AND isnull(T2.IS_OUT,'N')='N' AND
			                                            T2.WS_CD=@WS_CD AND T2.LEVEL_CD IN(SELECT LEVEL_CD FROM TB_S_M_ASSESS_GROUP_D WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=W1.ASSESS_TYPE AND GRP_CD=@GRP_CD) AND
			                                            T1.DEPT_NO IN(
			                                            SELECT Y.DEPT_NO
				                                        FROM
					                                        TB_H_R_DEPT_DATA Z join
						                                        (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X on Z.DEPT_NO=X.DEPT_NO  left join 
					                                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                                        where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and Z.HEAD_EMP_ID=W1.EMP_ID_DEPT20

			                                            )) EMP_TOTAL	,   
			                                            (SELECT COUNT(T2.EMP_ID)
		                                        FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                                            TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                                        WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE  AND isnull(T2.IS_OUT,'N')='N' AND
			                                            T2.WS_CD=@WS_CD AND T2.LEVEL_CD IN(SELECT LEVEL_CD FROM TB_S_M_ASSESS_GROUP_D WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=W1.ASSESS_TYPE AND GRP_CD=@GRP_CD) AND
			                                            T2.SCORE_FINAL='A' AND
			                                            T1.DEPT_NO IN(
			                                            SELECT Y.DEPT_NO
				                                        FROM
					                                        TB_H_R_DEPT_DATA Z join
						                                        (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X on Z.DEPT_NO=X.DEPT_NO  left join 
					                                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                                        where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and Z.HEAD_EMP_ID=W1.EMP_ID_DEPT20
			                                            )) SCORE_A,   
			                                            (SELECT COUNT(T2.EMP_ID)
		                                        FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                                            TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                                        WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE  AND isnull(T2.IS_OUT,'N')='N' AND
			                                            T2.WS_CD=@WS_CD AND T2.LEVEL_CD IN(SELECT LEVEL_CD FROM TB_S_M_ASSESS_GROUP_D WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=W1.ASSESS_TYPE AND GRP_CD=@GRP_CD) AND
			                                            T2.SCORE_FINAL='B' AND
			                                            T1.DEPT_NO IN(
			                                            SELECT Y.DEPT_NO
				                                        FROM
					                                        TB_H_R_DEPT_DATA Z join
						                                        (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X on Z.DEPT_NO=X.DEPT_NO  left join 
					                                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                                        where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and Z.HEAD_EMP_ID=W1.EMP_ID_DEPT20
			                                            )) SCORE_B,   
			                                            (SELECT COUNT(T2.EMP_ID)
		                                        FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                                            TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                                        WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE  AND isnull(T2.IS_OUT,'N')='N' AND
			                                            T2.WS_CD=@WS_CD AND T2.LEVEL_CD IN(SELECT LEVEL_CD FROM TB_S_M_ASSESS_GROUP_D WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=W1.ASSESS_TYPE AND GRP_CD=@GRP_CD) AND
			                                            T2.SCORE_FINAL='C' AND
			                                            T1.DEPT_NO IN(
			                                            SELECT Y.DEPT_NO
				                                        FROM
					                                        TB_H_R_DEPT_DATA Z join
						                                        (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X on Z.DEPT_NO=X.DEPT_NO  left join 
					                                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                                        where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and Z.HEAD_EMP_ID=W1.EMP_ID_DEPT20
			                                            )) SCORE_C,   
			                                            (SELECT COUNT(T2.EMP_ID)
		                                        FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                                            TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                                        WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE  AND isnull(T2.IS_OUT,'N')='N' AND
			                                            T2.WS_CD=@WS_CD AND T2.LEVEL_CD IN(SELECT LEVEL_CD FROM TB_S_M_ASSESS_GROUP_D WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=W1.ASSESS_TYPE AND GRP_CD=@GRP_CD) AND
			                                            T2.SCORE_FINAL='D' AND
			                                            T1.DEPT_NO IN(
			                                            SELECT Y.DEPT_NO
				                                        FROM
					                                        TB_H_R_DEPT_DATA Z join
						                                        (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X on Z.DEPT_NO=X.DEPT_NO  left join 
					                                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                                        where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and Z.HEAD_EMP_ID=W1.EMP_ID_DEPT20
			                                            )) SCORE_D,   
			                                            (SELECT COUNT(T2.EMP_ID)
		                                        FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                                            TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                                        WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE  AND isnull(T2.IS_OUT,'N')='N' AND
			                                            T2.WS_CD=@WS_CD AND T2.LEVEL_CD IN(SELECT LEVEL_CD FROM TB_S_M_ASSESS_GROUP_D WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=W1.ASSESS_TYPE AND GRP_CD=@GRP_CD) AND
			                                            T2.SCORE_FINAL='E' AND
			                                            T1.DEPT_NO IN(
			                                            SELECT Y.DEPT_NO
				                                        FROM
					                                        TB_H_R_DEPT_DATA Z join
						                                        (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X on Z.DEPT_NO=X.DEPT_NO  left join 
					                                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                                        where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and Z.HEAD_EMP_ID=W1.EMP_ID_DEPT20
			                                            )) SCORE_E,
                                               ISNULL( ( select TOP 1 IS_CTL from TB_S_M_ASSESS_GROUP_H    where ASSESS_YEAR=W1.ASSESS_YEAR and ASSESS_TYPE=W1.ASSESS_TYPE and GRP_CD=@GRP_CD),'N') IS_CTL 
                                FROM TB_S_M_ASSESS_DEPT20_MA W1 WHERE W1.MA_EMP_ID=@MA_EMP_ID AND W1.ASSESS_TYPE=@ASSESS_TYPE AND W1.ASSESS_YEAR=@ASSESS_YEAR
                                )X
                                WHERE X.EMP_TOTAL > 0
                                ORDER BY X.DEPT_NO_20
                                ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@MA_EMP_ID", MA_EMP_ID);
            ht.Add("@WS_CD", wsCd);
            ht.Add("@GRP_CD", grpCd);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable statisticsData_Suggest(string wsCd, string grpCd, string deptNo)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"SELECT T2.SCORE_DEPT, T1.SUGGEST_SCORE , COUNT(T1.SUGGEST_SCORE) NUMS
                        FROM   TB_S_M_ASSESS_EMP_SUGGEST  T1 LEFT JOIN
	                           TB_S_M_ASSESS_TARGET T2 ON T1.EMP_ID=T2.EMP_ID AND T1.ASSESS_YEAR=T2.ASSESS_YEAR AND T1.ASSESS_TYPE=T2.ASSESS_TYPE LEFT JOIN
	                           TB_S_M_ASSESS_DIRECTOR_D T3 ON T2.EMP_ID=T3.EMP_ID  AND T1.ASSESS_YEAR=T3.ASSESS_YEAR AND T1.ASSESS_TYPE=T3.ASSESS_TYPE 
                        WHERE T1.ASSESS_YEAR=@ASSESS_YEAR AND T1.ASSESS_TYPE=@ASSESS_TYPE AND T1.AUDRESULT1_YN='Y' AND
	                          T2.WS_CD=@WS_CD AND T2.LEVEL_CD IN(SELECT LEVEL_CD 
	                                                          FROM TB_S_M_ASSESS_GROUP_D 
									                          WHERE ASSESS_YEAR=T1.ASSESS_YEAR AND  ASSESS_TYPE=T1.ASSESS_TYPE AND GRP_CD=@GRP_CD) AND
                              T3.DEPT_NO IN(
		                        SELECT Y.DEPT_NO
		                        FROM	
			                        (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
			                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
			                        where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.DEPT_NO=@DEPT_NO
	                          )
                        GROUP BY     T2.SCORE_DEPT, T1.SUGGEST_SCORE 
                                ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO", deptNo);
            ht.Add("@WS_CD", wsCd);
            ht.Add("@GRP_CD", grpCd);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable statisticsData_Suggest_Not_Approve(string wsCd, string grpCd, string deptNo, string maType)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"SELECT COUNT(T2.EMP_ID) NUMS
                        FROM   TB_S_M_ASSESS_EMP_SUGGEST  T1 LEFT JOIN
	                           TB_S_M_ASSESS_TARGET T2 ON T1.EMP_ID=T2.EMP_ID AND T1.ASSESS_YEAR=T2.ASSESS_YEAR AND T1.ASSESS_TYPE=T2.ASSESS_TYPE LEFT JOIN
	                           TB_S_M_ASSESS_DIRECTOR_D T3 ON T2.EMP_ID=T3.EMP_ID  AND T1.ASSESS_YEAR=T3.ASSESS_YEAR AND T1.ASSESS_TYPE=T3.ASSESS_TYPE 
                        WHERE T1.ASSESS_YEAR=@ASSESS_YEAR AND T1.ASSESS_TYPE=@ASSESS_TYPE AND T1.AUDRESULT1_YN='Y' AND
	                          T2.WS_CD=@WS_CD AND T2.LEVEL_CD IN(SELECT LEVEL_CD 
	                                                          FROM TB_S_M_ASSESS_GROUP_D 
									                          WHERE ASSESS_YEAR=T1.ASSESS_YEAR AND  ASSESS_TYPE=T1.ASSESS_TYPE AND GRP_CD=@GRP_CD) AND
                              T3.DEPT_NO IN(
		                        SELECT Y.DEPT_NO
		                        FROM	
			                        (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
			                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
			                        where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.DEPT_NO=@DEPT_NO
	                          )
                        
                                ");
            if (maType == "A")
            {
                sb.Append(" AND (T1.AUDRESULT2_YN='E' OR T1.AUDRESULT2_YN='Y') AND T1.AUDRESULT3_YN='X' ");
            }else if(maType=="B"){
                sb.Append(" AND T1.AUDRESULT2_YN='X' ");
            }
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO", deptNo);
            ht.Add("@WS_CD", wsCd);
            ht.Add("@GRP_CD", grpCd);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
}