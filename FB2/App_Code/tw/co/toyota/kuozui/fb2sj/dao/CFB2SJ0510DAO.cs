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
/// WFB2SJ0510 的摘要描述
/// </summary>
public class CFB2SJ0510DAO : BaseDAO
{
    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string ASSESS_TYPE_DESC { get; set; }
    public string DEPT_NO { get; set; }
    public string DEPT_NAME { get; set; }
    public string EMP_ID { get; set; }
    public string WS_CD { get; set; }
    public string LEVEL_CD { get; set; }
    public string ITEM_CD { get; set; }
    public int MNG_GRADE { get; set; }
    public string RECOMM_DESC { get; set; }
    public string COMMENTS { get; set; }
    public string SCORE_FINAL { get; set; }
    public string SCORE_DEPT { get; set; }
    public string IS_OUT { get; set; }
    public string IS_DR { get; set; }
    public string SIGN_YN { get; set; }
    public string IS_DEPT_20 { get; set; }
    public string SCORE_LEVEL_GROUP { get; set; }
    public string DEPT_EMP_ID { get; set; }

    public string USER_UP_YN { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SJ0510DAO()
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
            sb.Append(@" Select 
                         A.DEPT_NO, D.DEPT_NAME, A.DEPT_LEVEL, A.HEAD_EMP_ID, E.EMP_NAME AS HEAD_EMP_NAME ,D.DEPT_NO_20 ,isnull(B.SIGN_YN,'X') SIGN_YN,isnull(A.SIGN_YN,'X') SIGN_YN_DEPT                                            
                         from (select * from TB_S_M_ASSESS_DEPT_LEVEL WHERE  ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE) A left join
                                TB_S_M_ASSESS_DEP20_UP_SIGN B on A.DEPT_NO=B.DEPT_NO and A.HEAD_EMP_ID=B.EMP_ID and 
                                                                A.ASSESS_YEAR=B.ASSESS_YEAR AND  A.ASSESS_TYPE=B.ASSESS_TYPE AND B.ASSESS_YEAR=@ASSESS_YEAR and B.ASSESS_TYPE=@ASSESS_TYPE LEFT JOIN
                                TB_H_R_DEPT_DATA_AD D ON A.DEPT_NO=D.DEPT_NO JOIN
                                TB_H_M_EMP E ON A.HEAD_EMP_ID=E.EMP_ID  ");


            sb.Append(@" 
                where A.HEAD_EMP_ID=@EMP_ID and A.DEPT_LEVEL>='20' order by A.DEPT_LEVEL asc
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
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string dept_no, string ws_cd, string score_level_group, string score_final, string emp_id, string recomm_desc, string dept_emp_id)
    {
        try
        {
            /**
            if (sortExpression.Contains("EMP_ID"))
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
                sortExpression = sortExpression.Replace("ASSESS_TYPE", "A.ASSESS_TYPE");
             * **/
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY   " + sortExpression + " ) As RowNumber,");
            sb.Append(" T.* FROM( SELECT ");
            sb.Append(@" A.ASSESS_YEAR, A.ASSESS_TYPE, A.EMP_ID, A.EMP_NAME, A.DEPT_NO, isnull(A.IS_OUT,'N') IS_OUT,
	                     A.LEVEL_CD, A.WS_CD,  A.WS_CD+'-'+B.SUB_DESC as WS_CD_DESC, A.SCORE_DEPT, A.SCORE_FINAL, 
	                     isnull((select sum(MNG_GRADE) from TB_S_M_ASSESS_SCORE WHERE ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID),0) MNG_GRADE,
	                     ((SELECT case when Count(*)>0 then Count(*)-1 else Count(*) end FROM TB_S_M_ASSESS_LOG WHERE ASSESS_YEAR=A.ASSESS_YEAR and  ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID and 
                            CREATED_BY NOT IN (
                            select DISTINCT MA_EMP_ID from TB_S_M_ASSESS_DEPT20_MA WHERE ASSESS_YEAR=A.ASSESS_YEAR AND ASSESS_TYPE=A.ASSESS_TYPE
                           ))) FIX_COUNT,
	                     (CASE WHEN A.ASSESS_TYPE='1' then A.SCORE_1H_1 else A.SCORE_2H_1 end) SCORE_H_1,
	                     (CASE WHEN A.ASSESS_TYPE='1' then A.SCORE_1H_2 else A.SCORE_2H_2 end) SCORE_H_2,
		                 A.RECENT_LEVEL_WORK_YEARS, A.AGE, A.WORK_YEARS,isnull(A.DISTING_REMARK,'') DISTING_REMARK,
		                 (A.LEAVE_O+A.LEAVE_P) LEAVE_OP, A.LEAVE_Q, (A.LEAVE_A+A.LEAVE_B) LEAVE_AB, D.SIGN_YN,
                            (case when len(A.LIMIT_RATE) =1 then 1 else 0 end) SORT_LIMIT_RATE,A.PJOB_CD, A.PJOB_DESC
                        FROM TB_S_M_ASSESS_DIRECTOR_D E left join
                             TB_S_M_ASSESS_TARGET A  on E.ASSESS_YEAR=A.ASSESS_YEAR and E.ASSESS_TYPE=A.ASSESS_TYPE and E.EMP_ID=A.EMP_ID left join
                               TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='HB' and B.MAIN_CD='WS_CD' and B.SUB_CD= A.WS_CD and B.IS_VALID='Y'  left join
                               TB_S_M_ASSESS_DIRECTOR_H D on E.ASSESS_YEAR=D.ASSESS_YEAR and E.ASSESS_TYPE=D.ASSESS_TYPE and E.DEPT_NO=D.DEPT_NO  AND E.DIREC_EMP_ID=D.DIREC_EMP_ID
                        where E.ASSESS_YEAR=@ASSESS_YEAR and E.ASSESS_TYPE= @ASSESS_TYPE  and A.EMP_ID<>@CREATED_BY and
                              D.LEVEL_RATE in(
	                           SELECT Y.LEVEL_RATE
		                        FROM
								     (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO, HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X left join 
									TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
								  where X.ASSESS_YEAR=E.ASSESS_YEAR and X.ASSESS_TYPE=E.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID 
	                          ) ");
            if (ws_cd != "-1")
            {
                sb.Append(" and A.WS_CD  = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }

            if (score_level_group != "-1")
            {
                //20241008-修改Mark
                /**
                String[] slgArray = score_level_group.Split('/');
                sb.Append(" and (");
                for (int i = 0; i < slgArray.Length; i++)
                {
                    if (i > 0) sb.Append(" or ");
                    sb.Append(" A.LEVEL_CD='" + slgArray[i].ToString() + "'");
                }

                sb.Append(" )");**/
                sb.Append(" AND A.WS_CD+'-'+A.LEVEL_CD IN (SELECT WS_CD+'-'+LEVEL_CD FROM TB_S_M_ASSESS_POINT_YEAR WHERE ASSESS_YEAR=E.ASSESS_YEAR AND ASSESS_TYPE=E.ASSESS_TYPE AND POINT_GROUP=@POINT_GROUP) ");
                ht.Add("@POINT_GROUP", score_level_group);
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
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@HEAD_EMP_ID", dept_emp_id);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string assess_year, string assess_type, string dept_no, string ws_cd, string score_level_group, string score_final, string emp_id, string recomm_desc, string dept_emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNT(*) total_record FROM TB_S_M_ASSESS_DIRECTOR_D E left join ");
            sb.Append("                 TB_S_M_ASSESS_TARGET A  on E.ASSESS_YEAR=A.ASSESS_YEAR and E.ASSESS_TYPE=A.ASSESS_TYPE and E.EMP_ID=A.EMP_ID left join ");
            sb.Append("                  TB_S_M_ASSESS_DIRECTOR_H D on E.ASSESS_YEAR=D.ASSESS_YEAR and E.ASSESS_TYPE=D.ASSESS_TYPE and E.DEPT_NO=D.DEPT_NO  AND E.DIREC_EMP_ID=D.DIREC_EMP_ID ");
            sb.Append(" where E.ASSESS_YEAR=@ASSESS_YEAR and E.ASSESS_TYPE= @ASSESS_TYPE  and A.EMP_ID<>@CREATED_BY and ");
            sb.Append(" D.LEVEL_RATE in( "); 
	        sb.Append(" SELECT Y.LEVEL_RATE "); 
		    sb.Append(" FROM ");
            sb.Append("	(select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO, HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X  left join  ");
			sb.Append("					TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE ");
            sb.Append("	 where X.ASSESS_YEAR=E.ASSESS_YEAR and X.ASSESS_TYPE=E.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID   "); 
	        sb.Append("    )");


            if (ws_cd != "-1")
            {
                sb.Append(" and A.WS_CD  = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }

            if (score_level_group != "-1")
            {
                //20241008-修改Mark
                /**
                String[] slgArray = score_level_group.Split('/');
                sb.Append(" and (");
                for (int i = 0; i < slgArray.Length; i++)
                {
                    if (i > 0) sb.Append(" or ");
                    sb.Append(" A.LEVEL_CD='" + slgArray[i].ToString() + "'");
                }

                sb.Append(" )");**/
                sb.Append(" AND A.WS_CD+'-'+A.LEVEL_CD IN (SELECT WS_CD+'-'+LEVEL_CD  FROM TB_S_M_ASSESS_POINT_YEAR WHERE ASSESS_YEAR=E.ASSESS_YEAR AND ASSESS_TYPE=E.ASSESS_TYPE AND POINT_GROUP=@POINT_GROUP) ");
                ht.Add("@POINT_GROUP", score_level_group);
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
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@HEAD_EMP_ID", dept_emp_id);
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
    public DataTable getSituationData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string dept_no, string is_dept_20, string dept_emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (");
            if (is_dept_20 == "N")
            {
                sb.Append(" select ROW_NUMBER() OVER(ORDER BY A.DEPT_NO ) As RowNumber,");
                sb.Append(@" A.ASSESS_YEAR,A.ASSESS_TYPE, A.DIREC_EMP_ID, B.EMP_NAME, A.DEPT_NO, A.DEPT_FULL_NAME as DEPT_NAME, A.SIGN_YN ,(case when A.SIGN_YN='N' then '未提' else '已提' end) as SIGN_YN_DESC, A.MNG_NUM ,
                            C.SIGN_YN LEVEL_SIGN_YN ,(case when C.SIGN_YN='N' then '未提' else'已提' end) as LEVEL_SIGN_YN_DESC,
	                           (Select count(*) from TB_S_M_ASSESS_TARGET O left jOIN TB_S_M_ASSESS_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                            WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO=A.DEPT_NO and isnull(O.SCORE_DEPT,'')<>'' and O.EMP_ID<>A.DIREC_EMP_ID  AND P.DIREC_EMP_ID=A.DIREC_EMP_ID ) as MNG_GRANT_NUM,
	                           (select count(*) from TB_S_M_ASSESS_TARGET O left jOIN TB_S_M_ASSESS_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                            WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO=A.DEPT_NO and isnull(O.SCORE_DEPT,'')='A' and O.EMP_ID<>A.DIREC_EMP_ID  AND P.DIREC_EMP_ID=A.DIREC_EMP_ID ) as MNG_GRANT_A_NUM,
	                           (select count(*) from TB_S_M_ASSESS_TARGET O left jOIN TB_S_M_ASSESS_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                            WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO=A.DEPT_NO and isnull(O.SCORE_DEPT,'')='B' and O.EMP_ID<>A.DIREC_EMP_ID  AND P.DIREC_EMP_ID=A.DIREC_EMP_ID ) as MNG_GRANT_B_NUM,
	                           (select count(*) from TB_S_M_ASSESS_TARGET O left jOIN TB_S_M_ASSESS_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                            WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO=A.DEPT_NO and isnull(O.SCORE_DEPT,'')='C' and O.EMP_ID<>A.DIREC_EMP_ID  AND P.DIREC_EMP_ID=A.DIREC_EMP_ID ) as MNG_GRANT_C_NUM,
	                           (select count(*) from TB_S_M_ASSESS_TARGET O left jOIN TB_S_M_ASSESS_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                            WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO=A.DEPT_NO and isnull(O.SCORE_DEPT,'')='D' and O.EMP_ID<>A.DIREC_EMP_ID  AND P.DIREC_EMP_ID=A.DIREC_EMP_ID ) as MNG_GRANT_D_NUM,
	                           (select count(*) from TB_S_M_ASSESS_TARGET O left jOIN TB_S_M_ASSESS_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                            WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO=A.DEPT_NO and isnull(O.SCORE_DEPT,'')='E' and O.EMP_ID<>A.DIREC_EMP_ID AND P.DIREC_EMP_ID=A.DIREC_EMP_ID ) as MNG_GRANT_E_NUM
                        from TB_S_M_ASSESS_DIRECTOR_H A INNER JOIN 
                             TB_H_M_EMP B on A.DIREC_EMP_ID=B.EMP_ID  JOIN
                             TB_S_M_ASSESS_DEPT_LEVEL C on A.ASSESS_YEAR=C.ASSESS_YEAR and  A.ASSESS_TYPE=C.ASSESS_TYPE and A.DEPT_NO=C.DEPT_NO and A.LEVEL_RATE=C.LEVEL_RATE
                        where  A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE  and 
						       A.LEVEL_RATE in(
								SELECT Y.LEVEL_RATE
								FROM
                                    (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO, HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
									TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
								  where X.ASSESS_YEAR=@ASSESS_YEAR and X.ASSESS_TYPE=@ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID
							   )  ");

            }
            else
            {
                sb.Append(" select ROW_NUMBER() OVER(ORDER BY T.DEPT_NO ) As RowNumber,");
                sb.Append(@" T.*
                            from
                            (
                             SELECT  A.ASSESS_YEAR,A.ASSESS_TYPE, A.DIREC_EMP_ID, B.EMP_NAME, A.DEPT_NO, A.DEPT_FULL_NAME as DEPT_NAME, A.SIGN_YN ,(case when A.SIGN_YN='N' then '未提' else '已提' end) as SIGN_YN_DESC, A.MNG_NUM ,
                                     (case when ISNULL(C.SIGN_YN,'N')='Y' OR ISNULL(D.SIGN_YN,'N')='Y' then 'Y' else 'N' end) LEVEL_SIGN_YN ,(case when ISNULL(C.SIGN_YN,'N')='Y' OR ISNULL(D.SIGN_YN,'N')='Y' then '已提' else '未提' end) as LEVEL_SIGN_YN_DESC,
	                                  (Select count(*) from TB_S_M_ASSESS_TARGET O WHERE O.ASSESS_YEAR=A.ASSESS_YEAR and O.ASSESS_TYPE=A.ASSESS_TYPE and isnull(SCORE_DEPT,'')<>'' and O.EMP_ID IN(
									SELECT P.EMP_ID FROM TB_S_M_ASSESS_DIRECTOR_D P WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DIREC_EMP_ID=A.DIREC_EMP_ID and P.DEPT_NO=A.DEPT_NO) ) as MNG_GRANT_NUM,
	                                (Select count(*) from TB_S_M_ASSESS_TARGET  O WHERE O.ASSESS_YEAR=A.ASSESS_YEAR and O.ASSESS_TYPE=A.ASSESS_TYPE and isnull(SCORE_DEPT,'')='A' and O.EMP_ID IN(
									SELECT P.EMP_ID FROM TB_S_M_ASSESS_DIRECTOR_D P WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DIREC_EMP_ID=A.DIREC_EMP_ID and P.DEPT_NO=A.DEPT_NO) ) as MNG_GRANT_A_NUM,
	                                (Select count(*) from TB_S_M_ASSESS_TARGET  O WHERE O.ASSESS_YEAR=A.ASSESS_YEAR and O.ASSESS_TYPE=A.ASSESS_TYPE and isnull(SCORE_DEPT,'')='B' and O.EMP_ID IN(
									SELECT P.EMP_ID FROM TB_S_M_ASSESS_DIRECTOR_D P WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DIREC_EMP_ID=A.DIREC_EMP_ID and P.DEPT_NO=A.DEPT_NO) ) as MNG_GRANT_B_NUM,
	                                (Select count(*) from TB_S_M_ASSESS_TARGET  O WHERE O.ASSESS_YEAR=A.ASSESS_YEAR and O.ASSESS_TYPE=A.ASSESS_TYPE and isnull(SCORE_DEPT,'')='C' and O.EMP_ID IN(
									SELECT P.EMP_ID FROM TB_S_M_ASSESS_DIRECTOR_D P WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DIREC_EMP_ID=A.DIREC_EMP_ID and P.DEPT_NO=A.DEPT_NO) ) as MNG_GRANT_C_NUM,
	                                (Select count(*) from TB_S_M_ASSESS_TARGET  O WHERE O.ASSESS_YEAR=A.ASSESS_YEAR and O.ASSESS_TYPE=A.ASSESS_TYPE and isnull(SCORE_DEPT,'')='D' and O.EMP_ID IN(
									SELECT P.EMP_ID FROM TB_S_M_ASSESS_DIRECTOR_D P WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DIREC_EMP_ID=A.DIREC_EMP_ID and P.DEPT_NO=A.DEPT_NO) ) as MNG_GRANT_D_NUM,
	                                (Select count(*) from TB_S_M_ASSESS_TARGET  O WHERE O.ASSESS_YEAR=A.ASSESS_YEAR and O.ASSESS_TYPE=A.ASSESS_TYPE and isnull(SCORE_DEPT,'')='E' and O.EMP_ID IN(
									SELECT P.EMP_ID FROM TB_S_M_ASSESS_DIRECTOR_D P WHERE P.ASSESS_YEAR=A.ASSESS_YEAR and P.ASSESS_TYPE=A.ASSESS_TYPE and P.DIREC_EMP_ID=A.DIREC_EMP_ID and P.DEPT_NO=A.DEPT_NO)  ) as MNG_GRANT_E_NUM                         
                              from TB_S_M_ASSESS_DIRECTOR_H A INNER JOIN 
                                   TB_H_M_EMP B on A.DIREC_EMP_ID=B.EMP_ID  JOIN
                                   TB_S_M_ASSESS_DEPT_LEVEL C on C.ASSESS_YEAR=A.ASSESS_YEAR and  C.ASSESS_TYPE=A.ASSESS_TYPE and C.DEPT_NO=A.DEPT_NO and C.DEPT_LEVEL<='30' and C.DEPT_LEVEL>10 and C.HEAD_EMP_ID=A.DIREC_EMP_ID LEFT JOIN
                                   TB_S_M_ASSESS_DEP20_UP_SIGN D ON A.DEPT_NO=D.DEPT_NO and A.DIREC_EMP_ID=D.EMP_ID and 
                                                                                                 A.ASSESS_YEAR=D.ASSESS_YEAR AND  A.ASSESS_TYPE=D.ASSESS_TYPE
                              where  A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE and 
						            A.DEPT_NO in(
								    SELECT Y.DEPT_NO
								    FROM
								        (select LEVEL_RATE ,ASSESS_YEAR, ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=@DEPT_NO and DEPT_LEVEL>='20') X left join 
														        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
								        where X.ASSESS_YEAR=@ASSESS_YEAR and X.ASSESS_TYPE=@ASSESS_TYPE and Y.DEPT_LEVEL<='30'
							        ) 
                            UNION
                             SELECT  A.ASSESS_YEAR,A.ASSESS_TYPE, A.DIREC_EMP_ID, B.EMP_NAME, A.DEPT_NO, A.DEPT_FULL_NAME as DEPT_NAME, A.SIGN_YN ,(case when A.SIGN_YN='N' then '未提' else '已提' end) as SIGN_YN_DESC, 
                                      (select sum(MNG_NUM) from TB_S_M_ASSESS_DIRECTOR_H where ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and DEPT_NO in(
	                                 SELECT Y.DEPT_NO
			                                FROM
				                                (select LEVEL_RATE ,ASSESS_YEAR,ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=A.DEPT_NO and DEPT_LEVEL>='20') X left join 
										                                TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
				                                where X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE and Y.DEPT_LEVEL>'20' 		  
		                                  )  )MNG_NUM ,
                                     C.SIGN_YN LEVEL_SIGN_YN ,(case when C.SIGN_YN='N' OR C.SIGN_YN='' then '未提' else '已提' end) as LEVEL_SIGN_YN_DESC,
	                                 (Select count(*) from TB_S_M_ASSESS_TARGET O join
                                                           TB_S_M_ASSESS_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                                  WHERE O.ASSESS_YEAR=A.ASSESS_YEAR and O.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO in(
		                              SELECT Y.DEPT_NO
			                            FROM
				                            (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=A.DEPT_NO and DEPT_LEVEL>='20') X left join 
										                            TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
				                            where X.ASSESS_YEAR=A.ASSESS_YEAR 	and X.ASSESS_TYPE=A.ASSESS_TYPE	  
		                              ) and isnull(O.SCORE_DEPT,'')<>'') as MNG_GRANT_NUM,
	                                    (Select count(*) from TB_S_M_ASSESS_TARGET O join
                                                              TB_S_M_ASSESS_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                                     WHERE O.ASSESS_YEAR=A.ASSESS_YEAR and O.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO in(
		                              SELECT Y.DEPT_NO
			                            FROM
				                            (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=A.DEPT_NO and DEPT_LEVEL>='20') X left join 
										                            TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
				                            where X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE  
		                              )  and isnull(O.SCORE_DEPT,'')='A') as MNG_GRANT_A_NUM,
	                                    (Select count(*) from TB_S_M_ASSESS_TARGET  O join
                                                              TB_S_M_ASSESS_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                                     WHERE O.ASSESS_YEAR=A.ASSESS_YEAR and O.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO in(
		                              SELECT Y.DEPT_NO
			                            FROM
				                            (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=A.DEPT_NO and DEPT_LEVEL>='20') X left join 
										                            TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
				                            where X.ASSESS_YEAR=A.ASSESS_YEAR  and X.ASSESS_TYPE=A.ASSESS_TYPE		  
		                              )  and isnull(O.SCORE_DEPT,'')='B') as MNG_GRANT_B_NUM,
	                                (Select count(*) from TB_S_M_ASSESS_TARGET  O join
                                                          TB_S_M_ASSESS_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                                 WHERE O.ASSESS_YEAR=A.ASSESS_YEAR and O.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO in(
		                              SELECT Y.DEPT_NO
			                            FROM
				                            (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=A.DEPT_NO and DEPT_LEVEL>='20') X left join 
										                          TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
				                            where X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE		  
		                              )  and isnull(O.SCORE_DEPT,'')='C') as MNG_GRANT_C_NUM,
	                                (Select count(*) from TB_S_M_ASSESS_TARGET  O join
                                                              TB_S_M_ASSESS_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                                WHERE O.ASSESS_YEAR=A.ASSESS_YEAR and O.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO in(
		                              SELECT Y.DEPT_NO
			                            FROM
				                            (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=A.DEPT_NO and DEPT_LEVEL>='20') X left join 
										                            TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
				                            where X.ASSESS_YEAR=A.ASSESS_YEAR  and X.ASSESS_TYPE=A.ASSESS_TYPE		  
		                              )  and isnull(O.SCORE_DEPT,'')='D') as MNG_GRANT_D_NUM,
	                                 (Select count(*) from TB_S_M_ASSESS_TARGET  O join
                                                              TB_S_M_ASSESS_DIRECTOR_D P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE=P.ASSESS_TYPE and O.EMP_ID=P.EMP_ID
	                                  WHERE O.ASSESS_YEAR=A.ASSESS_YEAR and O.ASSESS_TYPE=A.ASSESS_TYPE and P.DEPT_NO in(
		                              SELECT Y.DEPT_NO
			                            FROM
				                            (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=A.DEPT_NO and DEPT_LEVEL>='20') X left join 
										                            TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
				                            where X.ASSESS_YEAR=A.ASSESS_YEAR	and X.ASSESS_TYPE=A.ASSESS_TYPE	  
		                              ) and isnull(O.SCORE_DEPT,'')='E') as MNG_GRANT_E_NUM
                                from TB_S_M_ASSESS_DIRECTOR_H A INNER JOIN 
                                     TB_H_M_EMP B on A.DIREC_EMP_ID=B.EMP_ID  JOIN
                                     TB_S_M_ASSESS_DEPT_LEVEL C on A.ASSESS_YEAR=C.ASSESS_YEAR and  A.ASSESS_TYPE=C.ASSESS_TYPE and A.DEPT_NO=C.DEPT_NO   
                                where  A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE and
						            A.DEPT_NO in(
								        SELECT Y.DEPT_NO
								        FROM
								            (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=@DEPT_NO and DEPT_LEVEL>='20') X left join 
														            TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
								            where X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE and Y.DEPT_LEVEL='40'
							            ) 
							                               )T ");
            }
          
            
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@HEAD_EMP_ID", dept_emp_id);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getSituationCount(int startRowIndex, int maximumRows, string assess_year, string assess_type, string dept_no, string is_dept_20, string dept_emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (is_dept_20 == "N")
            {
                sb.Append(" select COUNT(A.DEPT_NO) total_record  ");
                sb.Append(@" from TB_S_M_ASSESS_DIRECTOR_H A INNER JOIN TB_H_M_EMP B on A.DIREC_EMP_ID=B.EMP_ID    
                             where  A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE and 
						            A.LEVEL_RATE in(
								    SELECT Y.LEVEL_RATE
								    FROM                                        
								        (select LEVEL_RATE ,ASSESS_YEAR, ASSESS_TYPE ,DEPT_NO ,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
										TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR  and X.ASSESS_TYPE=Y.ASSESS_TYPE
								     where X.ASSESS_YEAR=@ASSESS_YEAR and X.ASSESS_TYPE=@ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID
							   ) ");
            }
            else
            {
                sb.Append(" select COUNT(X.DID) total_record FROM (");
                sb.Append(@" select (A.DEPT_NO+'-'+A.DIREC_EMP_ID)DID from TB_S_M_ASSESS_DIRECTOR_H A INNER JOIN 
                                   TB_H_M_EMP B on A.DIREC_EMP_ID=B.EMP_ID  JOIN
                                   TB_S_M_ASSESS_DEPT_LEVEL C on C.ASSESS_YEAR=A.ASSESS_YEAR and  C.ASSESS_TYPE=A.ASSESS_TYPE and C.DEPT_NO=A.DEPT_NO and C.DEPT_LEVEL<='30' and C.DEPT_LEVEL>10 and C.HEAD_EMP_ID=A.DIREC_EMP_ID
                              where  A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE and 
						            A.DEPT_NO in(
								    SELECT Y.DEPT_NO
								    FROM
								        (select LEVEL_RATE ,ASSESS_YEAR, ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=@DEPT_NO and DEPT_LEVEL>='20') X left join 
														        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
								        where X.ASSESS_YEAR=@ASSESS_YEAR and X.ASSESS_TYPE=@ASSESS_TYPE and Y.DEPT_LEVEL<='30' and Y.DEPT_LEVEL>'10'
							        ) 
                               ");
                sb.Append("union ");
                sb.Append(@" select (A.DEPT_NO+'-'+A.DIREC_EMP_ID)DID from  TB_S_M_ASSESS_DIRECTOR_H A INNER JOIN 
                                     TB_H_M_EMP B on A.DIREC_EMP_ID=B.EMP_ID  JOIN
                                     TB_S_M_ASSESS_DEPT_LEVEL C on A.ASSESS_YEAR=C.ASSESS_YEAR and  A.ASSESS_TYPE=C.ASSESS_TYPE and A.DEPT_NO=C.DEPT_NO   
                                where  A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE and
						            A.DEPT_NO in(
								        SELECT Y.DEPT_NO
								        FROM
								            (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=@DEPT_NO and DEPT_LEVEL>='20') X left join 
														            TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
								            where X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE and Y.DEPT_LEVEL='40'
							            ) 

                    )X");
            }
            /**
            sb.Append(" select COUNT(*) total_record FROM TB_S_M_ASSESS_TARGET A ");
            sb.Append(" where A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE  and ");
            sb.Append(" A.DEPT_NO in( ");
            sb.Append(" SELECT Y.DEPT_NO ");
            sb.Append(" FROM ");
            sb.Append("  (select LEVEL_RATE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=@DEPT_NO and DEPT_LEVEL>='20') X left join  ");
            sb.Append("     TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  ");
            sb.Append("    )");
            **/
            

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@HEAD_EMP_ID", dept_emp_id);

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
	                           (SELECT Count(*) FROM TB_S_M_ASSESS_LOG WHERE ASSESS_YEAR=A.ASSESS_YEAR and  ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID) FIX_COUNT,
	                            (CASE WHEN A.ASSESS_TYPE='1' then A.SCORE_1H_1 else A.SCORE_2H_1 end) SCORE_H_1,
	                            (CASE WHEN A.ASSESS_TYPE='1' then A.SCORE_1H_2 else A.SCORE_2H_2 end) SCORE_H_2,
		                        A.RECENT_LEVEL_WORK_YEARS, A.AGE, A.WORK_YEARS,isnull(A.DISTING_REMARK,'') DISTING_REMARK,
		                        (A.LEAVE_O+A.LEAVE_P) LEAVE_OP, A.LEAVE_Q, (A.LEAVE_A+A.LEAVE_B) LEAVE_AB
                         FROM TB_S_M_ASSESS_TARGET A left join
                              TB_S_M_ASSESS_DIRECTOR_D C ON A.EMP_ID=C.EMP_ID AND A.ASSESS_YEAR=C.ASSESS_YEAR AND A.ASSESS_TYPE=B.ASSESS_TYPE left join
                               TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='HB' and B.MAIN_CD='WS_CD' and B.SUB_CD= A.WS_CD and B.IS_VALID='Y'   
                         where A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE  and
                              C.LEVEL_RATE in(
	                           SELECT Y.LEVEL_RATE
		                        FROM
		                         (select LEVEL_RATE ,ASSESS_YEAR, ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=@DEPT_NO and DEPT_LEVEL>='20') X left join 
		                         TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE and X.ASSESS_YEAR=Y.ASSESS_YEAR  and X.ASSESS_TYPE=Y.ASSESS_TYPE
                                where X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE
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
                         from TB_S_M_ASSESS_DIRECTOR_H A with (nolock)
                         left join TB_H_M_EMP B  with (nolock) on A.DIREC_EMP_ID=B.EMP_ID ");


            sb.Append(@" 
                where 1=1
                and A.ASSESS_YEAR = @ASSESS_YEAR  and  A.ASSESS_TYPE = @ASSESS_TYPE  and A.DEPT_NO=@DEPT_NO
            ");

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
    public DataTable getDataDtl(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string dept_no, string emp_id, string score_dept, string is_out, string level_cd, string ws_cd, string is_dr)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY A.EMP_ID ) As RowNumber,");
            sb.Append(@" A.ASSESS_YEAR, A.ASSESS_TYPE, A.EMP_ID, B.EMP_NAME, isnull(B.IS_OUT,'N') IS_OUT,
	                    B.LEVEL_CD, B.WS_CD,  B.WS_CD+'-'+C.SUB_DESC as WS_CD_DESC,
	                    isnull((select sum(MNG_GRADE) from TB_S_M_ASSESS_SCORE WHERE ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID),0) MNG_GRADE,
	                    SCORE_DEPT,
	                    (CASE WHEN A.ASSESS_TYPE='1' then B.SCORE_1H_1 else B.SCORE_2H_1 end) SCORE_H_1,
	                    (CASE WHEN A.ASSESS_TYPE='1' then B.SCORE_1H_2 else B.SCORE_2H_2 end) SCORE_H_2,
	                    B.RECENT_LEVEL_WORK_YEARS, B.AGE, B.WORK_YEARS,isnull(B.DISTING_REMARK,'') DISTING_REMARK
                        FROM TB_S_M_ASSESS_DIRECTOR_D A left join
	                         TB_S_M_ASSESS_TARGET B on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.EMP_ID=B.EMP_ID left join
                             TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= B.WS_CD and C.IS_VALID='Y'   
                        where A.DEPT_NO=@DEPT_NO and A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE  and A.EMP_ID<>D.DIREC_EMP_ID  ");
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (score_dept != "-1")
            {
                sb.Append(" and B.SCORE_DEPT = @SCORE_DEPT ");
                ht.Add("@SCORE_DEPT", score_dept);
            }
            if (is_out != "-1")
            {
                sb.Append(" and B.IS_OUT = @IS_OUT ");
                ht.Add("@IS_OUT", is_out);
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
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCountDtl(int startRowIndex, int maximumRows, string assess_year, string assess_type, string dept_no, string emp_id, string score_dept, string is_out, string level_cd, string ws_cd, string is_dr)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(@" FROM TB_S_M_ASSESS_DIRECTOR_D A left join
	                         TB_S_M_ASSESS_TARGET B on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.EMP_ID=B.EMP_ID left join
                             TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= B.WS_CD and C.IS_VALID='Y'   
                        where A.DEPT_NO=@DEPT_NO and A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE  and A.EMP_ID<>D.DIREC_EMP_ID  ");
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (score_dept != "-1")
            {
                sb.Append(" and B.SCORE_DEPT = @SCORE_DEPT ");
                ht.Add("@SCORE_DEPT", score_dept);
            }
            if (is_out != "-1")
            {
                sb.Append(" and B.IS_OUT = @IS_OUT ");
                ht.Add("@IS_OUT", is_out);
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
							 TB_S_M_ASSESS_DIRECTOR_H D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.DEPT_NO=D.DEPT_NO  AND A.DIREC_EMP_ID=D.DIREC_EMP_ID left join
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

            sb.Append("select X.* from ");
            sb.Append("( ");
            sb.Append("select A.WS_CD,B.SUB_DESC WS_CD_DESC,A.SCORE_LEVEL_GROUP, P.IS_CTL,'b' RATE_TYPE,'基準' RATE_TYPE_DESC,BASE_A BA, BASE_B BB, BASE_C BC, BASE_D BD, BASE_E BE, BASE_TOT BTOT,");
            sb.Append("       REAL_A RA, REAL_B RB, REAL_C RC, REAL_D RD, REAL_E RE, REAL_TOTAL RTOT,'' CHECK_OK ");
            sb.Append("from TB_S_M_ASSESS_DEP20_PEO A  left join ");
            sb.Append("	  TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='HB' and B.MAIN_CD='WS_CD' and B.SUB_CD= A.WS_CD and B.IS_VALID='Y'  ");
            sb.Append("  LEFT JOIN TB_S_M_ASSESS_RATE P  ON A.ASSESS_TYPE=P.ASSESS_TYPE AND A.WS_CD=P.WS_CD AND A.LEVEL_CD=P.LEVEL_CD  ");
            sb.Append("where A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and A.DEPT_NO_20=@DEPT_NO and A.IS_MERGER<>'A' ");
            sb.Append("  and (A.BASE_A+A.BASE_B+A.BASE_C+A.BASE_D+A.BASE_E)>0 ");
            if (WS_CD != "-1")
            {
                sb.Append(" and A .WS_CD = @WS_CD ");
                ht.Add("@WS_CD", WS_CD);
            }
            if (SCORE_LEVEL_GROUP != "-1")
            {

                sb.Append(" and A.SCORE_LEVEL_GROUP = @SCORE_LEVEL_GROUP ");
                ht.Add("@SCORE_LEVEL_GROUP", SCORE_LEVEL_GROUP);
            }
            sb.Append(")X  order by X.WS_CD, X.SCORE_LEVEL_GROUP,X.RATE_TYPE ");
            
           
           
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
    public DataTable statisticsOutData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select X.* from ");
            sb.Append("( ");
            sb.Append(" select A.WS_CD,B.SUB_DESC WS_CD_DESC,'o' RATE_TYPE,'外數' RATE_TYPE_DESC,sum(OUT_REAL_A) RA, sum(OUT_REAL_B) RB, sum(OUT_REAL_C) RC, sum(OUT_REAL_D) RD, sum(OUT_REAL_E) RE, sum(OUT_REAL_TOTAL) RTOT ");
            sb.Append("from TB_S_M_ASSESS_DEP20_PEO A  left join ");
            sb.Append("	  TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='HB' and B.MAIN_CD='WS_CD' and B.SUB_CD= A.WS_CD and B.IS_VALID='Y'  ");
            sb.Append("where A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and A.DEPT_NO_20=@DEPT_NO and A.IS_MERGER<>'A' ");
            if (WS_CD != "-1")
            {
                sb.Append(" and A .WS_CD = @WS_CD ");
                ht.Add("@WS_CD", WS_CD);
            }
            if (SCORE_LEVEL_GROUP != "-1")
            {

                sb.Append(" and A.SCORE_LEVEL_GROUP = @SCORE_LEVEL_GROUP ");
                ht.Add("@SCORE_LEVEL_GROUP", SCORE_LEVEL_GROUP);
            }
            sb.Append(" GROUP BY A.WS_CD,B.SUB_DESC )X order by X.WS_CD ");


            
           
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
    public DataTable referData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select  E.EMP_NAME DIREC_EMP_NAME, ");
            sb.Append("      isnull((select sum(MNG_GRADE) from TB_S_M_ASSESS_SCORE WHERE ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID),0) MNG_GRADE, ");
            sb.Append("      B.SCORE_DEPT, isnull(B.IS_OUT,'N') IS_OUT, B.DISTING_REMARK, A.EMP_ID, B.EMP_NAME, D.DEPT_NO,  ");
            sb.Append("      F.DEPT_NO_20, F.DEPT_NAME_20, F.DEPT_NO_30, F.DEPT_NAME_30, F.DEPT_NO_40, F.DEPT_NAME_40, F.DEPT_NO_50, F.DEPT_NAME_50, F.DEPT_NO_60, F.DEPT_NAME_60, F.DEPT_NO_70, F.DEPT_NAME_70, ");
            sb.Append("      B.WS_CD,  B.WS_CD+'-'+C.SUB_DESC as WS_CD_DESC,B.LEVEL_CD, B.PJOB_CD, B.PJOB_DESC, B.RECENT_LEVEL_WORK_YEARS, B.WORK_YEARS, B.AGE, B.EMP_CHG_CD, G.SUB_DESC EMP_CHG_CD_DESC, ");
            sb.Append("      B.SCORE_1H_1, B.SCORE_1H_2, B.SCORE_1H_3, B.SCORE_2H_1, B.SCORE_2H_2, B.SCORE_2H_3,  ");
            sb.Append("      B.OVERTIME_HOUR_MEAN, B.LEAVE_O, B.LEAVE_P, B.LEAVE_Q, B.LEAVE_A, B.LEAVE_B, B.RETENTION_DAYS,  ");
            sb.Append("      B.THIRD_CNT_P, B.SECOND_CNT_P, B.FIRST_CNT_P, B.THIRD_CNT_M, B.SECOND_CNT_M, B.FIRST_CNT_M,  ");
            sb.Append("      B.PROPOSAL_TOTAL, B.PROPOSAL_GRADE, B.PROPOSAL_GRADE_MEAN, B.PROPOSAL_6 ");
            sb.Append("FROM TB_S_M_ASSESS_TARGET B left join ");
            sb.Append("     TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= B.WS_CD and C.IS_VALID='Y' left join ");
            sb.Append("     TB_S_M_ASSESS_DIRECTOR_D A  with (nolock)  on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.EMP_ID=B.EMP_ID left join   ");
            sb.Append("     TB_S_M_ASSESS_DIRECTOR_H D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.DEPT_NO = D.DEPT_NO AND A.DIREC_EMP_ID=D.DIREC_EMP_ID  left join ");
            sb.Append("     TB_H_M_EMP E  with (nolock) on D.DIREC_EMP_ID=E.EMP_ID left join ");
            sb.Append("     TB_H_R_DEPT_DATA_AD F  with (nolock) on A.DEPT_NO = F.DEPT_NO left join ");
            sb.Append("     TB_9_M_COMM_D G  with (nolock)  on G.SYS_CD='HB' and G.MAIN_CD='EMP_CHG_CD' and G.SUB_CD= B.EMP_CHG_CD and G.IS_VALID='Y' ");
            sb.Append("where B.ASSESS_YEAR=@ASSESS_YEAR and B.ASSESS_TYPE= @ASSESS_TYPE  and A.EMP_ID<>@CREATED_BY  and A.DEPT_NO IS NOT NULL and  ");
            sb.Append("      A.LEVEL_RATE in( ");
            sb.Append(" SELECT Y.LEVEL_RATE ");
            sb.Append(" FROM ");
            sb.Append("	(select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO, HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X left join  ");
            sb.Append("					TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE ");
            sb.Append("	 where X.ASSESS_YEAR=@ASSESS_YEAR and X.ASSESS_TYPE=@ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID   ");
            sb.Append("                        ) ");


            if (WS_CD != "-1")
            {
                sb.Append(" and B.WS_CD  = @WS_CD ");
                ht.Add("@WS_CD", WS_CD);
            }

            if (SCORE_LEVEL_GROUP != "-1")
            {
                String[] slgArray = SCORE_LEVEL_GROUP.Split('/');
                sb.Append(" and (");
                for (int i = 0; i < slgArray.Length; i++)
                {
                    if (i > 0) sb.Append(" or ");
                    sb.Append(" B.LEVEL_CD='" + slgArray[i].ToString() + "'");
                }

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
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@HEAD_EMP_ID", DEPT_EMP_ID);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);

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
            sb.Append("     TB_S_M_ASSESS_DIRECTOR_H D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.DEPT_NO = D.DEPT_NO  AND A.DIREC_EMP_ID=D.DIREC_EMP_ID ");
            sb.Append("where A.DEPT_NO=@DEPT_NO and A.ASSESS_YEAR= @ASSESS_YEAR and A.ASSESS_TYPE= @ASSESS_TYPE and A.EMP_ID<>D.DIREC_EMP_ID  and isnull(B.SCORE_DEPT,'')='' ");
            
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
    //取得要望書數量
    public int getEmpSuggestCount(String assess_year, String assess_type, String dept_no, String apprve_flag1, String apprve_flag2, String apprve_flag3)
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
            sb.Append(" B.LEVEL_RATE in( ");
            sb.Append(" SELECT Y.LEVEL_RATE ");
            sb.Append(" FROM ");
            sb.Append("  (select LEVEL_RATE,ASSESS_YEAR,ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=@DEPT_NO and DEPT_LEVEL>='20') X left join  ");
            sb.Append("     TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE ");
            sb.Append("     WHERE X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE ");
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
            ht.Add("@DEPT_NO", dept_no);

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
    //取得尚未覆核子部門數量
    public int getNonSignDEPT(String assess_year, String assess_type, String dept_no, String head_emp_id)
    {

        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select count(*) total_record 
                          from TB_S_M_ASSESS_DEPT_LEVEL A LEFT JOIN
                               TB_S_M_ASSESS_DEPT_LEVEL B ON A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE  and SUBSTRING(B.LEVEL_RATE,1,len(A.LEVEL_RATE))=A.LEVEL_RATE  LEFT JOIN
							   TB_S_M_ASSESS_DIRECTOR_H C ON A.ASSESS_YEAR=C.ASSESS_YEAR and A.ASSESS_TYPE=C.ASSESS_TYPE AND B.LEVEL_RATE=C.LEVEL_RATE
                          where A.DEPT_NO=@DEPT_NO and (A.DEPT_NO<>B.DEPT_NO or (A.DEPT_NO=B.DEPT_NO and A.dept_LEVEL<>B.DEPT_LEVEL)) and A.HEAD_EMP_ID=@HEAD_EMP_ID and B.SIGN_YN<>'Y' and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and A.DEPT_LEVEL>='20' and B.HEAD_EMP_ID<>@HEAD_EMP_ID AND C.DEPT_NO IS NOT NULL ");

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@HEAD_EMP_ID", head_emp_id);

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
    //取得尚未簽核子部門數量
    public int getNonSignDirectDEPT(String assess_year, String assess_type, String dept_no)
    {

        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select count(*) total_record 
                         from TB_S_M_ASSESS_DIRECTOR_H A
                        where A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and A.SIGN_YN<>'Y' and A.LEVEL_RATE in
                        (
                        select Y.LEVEL_RATE from TB_S_M_ASSESS_DEPT_LEVEL X LEFT JOIN
                                                              TB_S_M_ASSESS_DEPT_LEVEL Y ON X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE  and SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  
                                                  where X.DEPT_NO=@DEPT_NO  AND X.HEAD_EMP_ID=@HEAD_EMP_ID and X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE and X.DEPT_LEVEL>='30' 
                        ) ");

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@HEAD_EMP_ID", SessionHandle.Current.emp_id);

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
	//取得尚未簽核子部門數量
    public int getNonSignDirectDEPTNoneLevel(String assess_year, String assess_type, String dept_no)
    {

        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select count(*) total_record 
                         from TB_S_M_ASSESS_DIRECTOR_H A
                        where A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and A.SIGN_YN<>'Y' and A.LEVEL_RATE in
                        (
                        select Y.LEVEL_RATE from TB_S_M_ASSESS_DEPT_LEVEL X LEFT JOIN
                                                              TB_S_M_ASSESS_DEPT_LEVEL Y ON X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE  and SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  
                                                  where X.DEPT_NO=@DEPT_NO  AND X.HEAD_EMP_ID=@HEAD_EMP_ID and X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE and X.DEPT_LEVEL>='20' 
                        ) ");

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@HEAD_EMP_ID", SessionHandle.Current.emp_id);

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
    public DataTable getUpdDep20PeoData(String assess_year, String assess_type, String dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
           
            String[] rateArray = { "A","B","C","D","E"};
            String[] typeArray = { "REAL","OUT"};
            String isOut = "N";
            sb.Append(" Select O.ASSESS_YEAR,O.ASSESS_TYPE, O.DEPT_NO_20,O.WS_CD, O.SCORE_LEVEL_GROUP, P.IS_CTL, ");
            sb.Append("        O.BASE_A,O.BASE_B, O.BASE_C,O.BASE_D, O.BASE_E ");
            for (int i = 0; i < typeArray.Length; i++)
            {
                isOut = "N";
                if (i == 1) isOut = "Y";
                for (int j = 0; j < rateArray.Length; j++)
                {
                    sb.Append(" ,(select count(*)  ");
                    sb.Append(" from TB_S_M_ASSESS_DIRECTOR_D B  join   ");
                    sb.Append("  TB_S_M_ASSESS_TARGET A on A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE and A.EMP_ID=B.EMP_ID  ");
                    sb.Append(" where B.ASSESS_YEAR=O.ASSESS_YEAR and B.ASSESS_TYPE=O.ASSESS_TYPE and A.WS_CD=O.WS_CD and CHARINDEX(A.LEVEL_CD,O.SCORE_LEVEL_GROUP)>0 and isnull(A.is_OUT,'N')='"+isOut+"' and A.SCORE_DEPT='"+rateArray[j]+"' and B.LEVEL_RATE in  ");
                    sb.Append(" (  ");
                    sb.Append(" 	SELECT Y.LEVEL_RATE  ");
                    sb.Append(" 	FROM  ");
                    sb.Append(" 		(select LEVEL_RATE ,ASSESS_YEAR,ASSESS_TYPE from TB_S_M_ASSESS_DEPT_LEVEL  where DEPT_NO=@DEPT_NO and DEPT_LEVEL>='20') X left join   ");
                    sb.Append(" 								TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR  and X.ASSESS_TYPE=Y.ASSESS_TYPE ");
                    sb.Append(" 		where X.ASSESS_YEAR=A.ASSESS_YEAR and X.ASSESS_TYPE=A.ASSESS_TYPE  ");
                    sb.Append(" ))"+typeArray[i]+"_"+rateArray[j]+" ");
                }
            }
            
            sb.Append(" from TB_S_M_ASSESS_DEP20_PEO O ");
            sb.Append("  LEFT JOIN TB_S_M_ASSESS_RATE P  ON O.ASSESS_TYPE=P.ASSESS_TYPE AND O.WS_CD=P.WS_CD AND O.LEVEL_CD=P.LEVEL_CD  ");
            sb.Append(" where O.DEPT_NO_20=@DEPT_NO and O.IS_MERGER<>'A' AND O.ASSESS_YEAR=@ASSESS_YEAR AND O.ASSESS_TYPE=@ASSESS_TYPE ");


            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht);
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
            sb.Append(@" SELECT (ROW_NUMBER() OVER(ORDER BY A.CREATED_DT ASC ))-1 As RowNumber, A.ASSESS_YEAR, A.ASSESS_TYPE, A.EMP_ID, A.GRADE, A.MEMO, A.CREATED_BY, A.CREATED_DT,B.EMP_NAME
                         FROM   TB_S_M_ASSESS_LOG A left join 
	                             TB_H_M_EMP B on A.CREATED_BY=B.EMP_ID 
                         WHERE 1=1 ");
            if(FUNC_ID=="SJ0510"){
                sb.Append(@" AND A.CREATED_BY NOT IN (
                            select DISTINCT MA_EMP_ID from TB_S_M_ASSESS_DEPT20_MA WHERE ASSESS_YEAR=A.ASSESS_YEAR AND ASSESS_TYPE=A.ASSESS_TYPE
                           )");
            }

            sb.Append(@"                
                and A.ASSESS_YEAR = @ASSESS_YEAR  and  A.ASSESS_TYPE = @ASSESS_TYPE  and A.EMP_ID=@EMP_ID
                ORDER BY A.CREATED_DT                
            ");//OFFSET 1 ROW  FETCH NEXT 100 ROW ONLY
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
    public DataTable getWSLevelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" SELECT distinct A.LEVEL_CD, A.WS_CD,  B.SUB_DESC as WS_CD_DESC
                        FROM TB_S_M_ASSESS_DIRECTOR_D E left join
                                TB_S_M_ASSESS_TARGET A  on E.ASSESS_YEAR=A.ASSESS_YEAR and E.ASSESS_TYPE=A.ASSESS_TYPE and E.EMP_ID=A.EMP_ID left join
                                TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='HB' and B.MAIN_CD='WS_CD' and B.SUB_CD= A.WS_CD and B.IS_VALID='Y'  left join
                                TB_S_M_ASSESS_DIRECTOR_H D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.DEPT_NO=D.DEPT_NO 
                        where E.ASSESS_YEAR=@ASSESS_YEAR and E.ASSESS_TYPE= @ASSESS_TYPE  and A.EMP_ID<>@EMP_ID and
                                E.LEVEL_RATE in(
	                            SELECT Y.LEVEL_RATE
		                        FROM
                                     (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO ,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X  left join 
			                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
			                        where X.ASSESS_YEAR=E.ASSESS_YEAR and X.ASSESS_TYPE=E.ASSESS_TYPE and X.HEAD_EMP_ID=@EMP_ID 
	                            ) ORDER BY A.WS_CD,A.LEVEL_CD ");


            
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
    public DataTable getWSLevelPointData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" SELECT distinct  A.WS_CD ,A.POINT_GROUP
                        FROM TB_S_M_ASSESS_POINT_YEAR A 
                        where A.ASSESS_YEAR=@ASSESS_YEAR and  A.ASSESS_TYPE= @ASSESS_TYPE  and A.WS_CD=@WS_CD
                                 ORDER BY A.POINT_GROUP ");



            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@WS_CD", WS_CD);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    } 
    //更新主管簽核TB_S_M_ASSESS_DEP20_UP_SIGN
    public void updateDEP20_UP_SIGN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_ASSESS_DEP20_UP_SIGN ");
            sb.Append(" set SIGN_YN=@SIGN_YN, ");
            sb.Append("     UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE and ");
            sb.Append("       EMP_ID=@EMP_ID and DEPT_NO=@DEPT_NO ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@SIGN_YN", SIGN_YN);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新主管簽核TB_S_M_ASSESS_DEPT_LEVEL
    public void updateDEPT_SIGN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_ASSESS_DEPT_LEVEL ");
            sb.Append(" set SIGN_YN=@SIGN_YN, ");
            sb.Append("     UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE and ");
            sb.Append("       LEVEL_RATE in( ");
            sb.Append(@"Select B.LEVEL_RATE from TB_S_M_ASSESS_DEPT_LEVEL A LEFT JOIN
                                      TB_S_M_ASSESS_DEPT_LEVEL B ON A.ASSESS_YEAR=B.ASSESS_YEAR and A.ASSESS_TYPE=B.ASSESS_TYPE  and SUBSTRING(B.LEVEL_RATE,1,len(A.LEVEL_RATE))=A.LEVEL_RATE  
                          where A.DEPT_NO=@DEPT_NO  and B.SIGN_YN<>'Y' and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and A.DEPT_LEVEL>='20' and B.HEAD_EMP_ID=@HEAD_EMP_ID ");
            sb.Append(") ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@SIGN_YN", SIGN_YN);
            ht.Add("@HEAD_EMP_ID", EMP_ID);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            dbConn.ExecuteT(sb, ht, true);
          
         
        }
        catch (Exception)
        {
            throw;
        }
    }
    //呼叫通知部處主管簽核信件
    internal void execSP_S_ASSESS_DEP20_NOTIFY_MAIL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_DEP20_NOTIFY_MAIL");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@USERID", EMP_ID);//EMP_ID
            ht.Add("@FUNCID", "FB2SJ051");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }
    //呼叫更新部長人數配置檔
    internal void execSP_S_ASSESS_DEP20_APPROVE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_DEP20_APPROVE");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEPT_NO_20", DEPT_NO);
            ht.Add("@SCORE_DEPT", SCORE_DEPT);
            ht.Add("@RECOMM_DESC", RECOMM_DESC);
            ht.Add("@COMMENTS", COMMENTS);
            ht.Add("@USERID", CREATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SJ051");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }
    //呼叫整批更新簽核記錄檔
    internal void execSP_S_ASSESS_UPD_SIGN_LOG()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_UPD_SIGN_LOG");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@HEAD_EMP_ID", EMP_ID);
            ht.Add("@USERID", CREATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SJ051");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }
    //取得退回部門列表資料
    public DataTable getBackData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string dept_emp_id)
    {
        try
        {
            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY   " + sortExpression + " ) As RowNumber,");
            sb.Append(" T.* FROM( SELECT ");
            sb.Append(@" X.ASSESS_YEAR, X.ASSESS_TYPE, W.DEPT_NO, W.DEPT_NAME, W.HEAD_EMP_ID, W.HEAD_EMP_NAME, ISNULL(S.SIGN_YN,'N') SIGN_YN
                        FROM
		                    (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO ,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X  left join 
	                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE and LEN(Y.LEVEL_RATE)-LEN(X.LEVEL_RATE)<=2 and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE LEFT JOIN
	                        TB_H_R_DEPT_DATA W ON Y.DEPT_NO = W.DEPT_NO LEFT JOIN
                            TB_S_M_ASSESS_DEPT_LEVEL S ON S.ASSESS_YEAR=X.ASSESS_YEAR and S.ASSESS_TYPE=X.ASSESS_TYPE AND S.DEPT_NO= W.DEPT_NO AND S.DEPT_LEVEL>='20'
                        where X.ASSESS_YEAR=@ASSESS_YEAR and X.ASSESS_TYPE=@ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID ");
            //sb.Append(" AND X.DEPT_NO<>Y.DEPT_NO ");
            sb.Append(" ) T ");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@HEAD_EMP_ID", dept_emp_id);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getBackCount(int startRowIndex, int maximumRows, string assess_year, string assess_type,  string dept_emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
           
	    sb.Append(@"SELECT  count(*) as total_record
                        FROM
		                    (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO ,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  				DEPT_LEVEL>='20') X  left join 
	                        TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE and LEN(Y.LEVEL_RATE)-LEN				(X.LEVEL_RATE)<=2 and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE LEFT JOIN
	                        TB_H_R_DEPT_DATA W ON Y.DEPT_NO = W.DEPT_NO LEFT JOIN
                            TB_S_M_ASSESS_DEPT_LEVEL S ON S.ASSESS_YEAR=X.ASSESS_YEAR and S.ASSESS_TYPE=X.ASSESS_TYPE AND S.DEPT_NO= W.DEPT_NO  AND S.DEPT_LEVEL>='20'
                        where X.ASSESS_YEAR=@ASSESS_YEAR and X.ASSESS_TYPE=@ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID  ");

        //sb.Append(" AND X.DEPT_NO<>Y.DEPT_NO ");

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@HEAD_EMP_ID", dept_emp_id);
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
    //BACK更新主管簽核TB_S_M_ASSESS_DEPT_LEVEL
    public void updateDEPT_Back()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_ASSESS_DEPT_LEVEL ");
            sb.Append(" set SIGN_YN=@SIGN_YN, ");
            sb.Append("     UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE and ");
            sb.Append("       DEPT_NO =@DEPT_NO ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@SIGN_YN", SIGN_YN);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //BACK更新主管簽核TB_S_M_ASSESS_Direct_H
    public void updateDirect_Back()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_ASSESS_DIRECTOR_H ");
            sb.Append(" set SIGN_YN=@SIGN_YN, ");
            sb.Append("     UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE and ");
            sb.Append("       DEPT_NO =@DEPT_NO ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@SIGN_YN", SIGN_YN);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getWSApproveData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string dept_no, string dept_emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select distinct X.* From (");
            sb.Append(@" SELECT W1.POINT_GROUP SCORE_LEVEL_GROUP,W1.POINT_GROUP ARG,
                            (SELECT distinct SU1.POINT_GROUP_NAME FROM TB_S_M_ASSESS_POINT_YEAR SU1 WHERE SU1.ASSESS_YEAR=W1.ASSESS_YEAR AND SU1.ASSESS_TYPE=W1.ASSESS_TYPE AND SU1.POINT_GROUP=W1.POINT_GROUP )POINT_GROUP_NAME,
                           (SELECT COUNT(T2.EMP_ID)
		                    FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                       TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                    WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@HEAD_EMP_ID AND
			                        T2.WS_CD+'-'+T2.LEVEL_CD IN(SELECT WS_CD+'-'+LEVEL_CD FROM TB_S_M_ASSESS_POINT_YEAR WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=@ASSESS_TYPE AND POINT_GROUP=W1.POINT_GROUP) AND
			                       T1.LEVEL_RATE IN(
			                       SELECT Y.LEVEL_RATE
				                    FROM					                    
						                (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
					                    TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                    where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID

			                       )) EMP_TOTAL	,   
			                        (SELECT COUNT(T2.EMP_ID)
		                    FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                       TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                    WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@HEAD_EMP_ID AND
			                       T2.WS_CD+'-'+T2.LEVEL_CD IN(SELECT WS_CD+'-'+LEVEL_CD FROM TB_S_M_ASSESS_POINT_YEAR WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=@ASSESS_TYPE AND POINT_GROUP=W1.POINT_GROUP) AND
			                       T2.SCORE_DEPT='A' AND
			                        T1.LEVEL_RATE IN(
			                       SELECT Y.LEVEL_RATE
				                    FROM					                    
						                (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
					                    TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                    where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID

			                       )) SCORE_A,   
			                        (SELECT COUNT(T2.EMP_ID)
		                    FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                       TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                    WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@HEAD_EMP_ID AND
			                        T2.WS_CD+'-'+T2.LEVEL_CD IN(SELECT WS_CD+'-'+LEVEL_CD FROM TB_S_M_ASSESS_POINT_YEAR WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=@ASSESS_TYPE AND POINT_GROUP=W1.POINT_GROUP) AND
			                       T2.SCORE_DEPT='B' AND
			                        T1.LEVEL_RATE IN(
			                       SELECT Y.LEVEL_RATE
				                    FROM					                    
						                (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
					                    TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                    where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID

			                       )) SCORE_B,   
			                        (SELECT COUNT(T2.EMP_ID)
		                    FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                       TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                    WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@HEAD_EMP_ID AND
			                        T2.WS_CD+'-'+T2.LEVEL_CD IN(SELECT WS_CD+'-'+LEVEL_CD FROM TB_S_M_ASSESS_POINT_YEAR WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=@ASSESS_TYPE AND POINT_GROUP=W1.POINT_GROUP) AND
			                       T2.SCORE_DEPT='C' AND
			                        T1.LEVEL_RATE IN(
			                       SELECT Y.LEVEL_RATE
				                    FROM					                    
						                (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
					                    TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                    where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID

			                       )) SCORE_C,   
			                        (SELECT COUNT(T2.EMP_ID)
		                    FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                       TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                    WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@HEAD_EMP_ID AND
			                      T2.WS_CD+'-'+T2.LEVEL_CD IN(SELECT WS_CD+'-'+LEVEL_CD FROM TB_S_M_ASSESS_POINT_YEAR WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=@ASSESS_TYPE AND POINT_GROUP=W1.POINT_GROUP) AND
			                       T2.SCORE_DEPT='D' AND
			                        T1.LEVEL_RATE IN(
			                       SELECT Y.LEVEL_RATE
				                    FROM					                    
						                (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
					                    TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                    where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID

			                       )) SCORE_D,   
			                        (SELECT COUNT(T2.EMP_ID)
		                    FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                       TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                    WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@HEAD_EMP_ID AND
			                       T2.WS_CD+'-'+T2.LEVEL_CD IN(SELECT WS_CD+'-'+LEVEL_CD FROM TB_S_M_ASSESS_POINT_YEAR WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=@ASSESS_TYPE AND POINT_GROUP=W1.POINT_GROUP) AND
			                       T2.SCORE_DEPT='E' AND
			                        T1.LEVEL_RATE IN(
			                       SELECT Y.LEVEL_RATE
				                    FROM					                    
						                (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
					                    TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                    where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID

			                       )) SCORE_E
                    FROM TB_S_M_ASSESS_DEPTPOINT  W1 
                    WHERE W1.ASSESS_YEAR=@ASSESS_YEAR AND W1.ASSESS_TYPE=@ASSESS_TYPE AND W1.DEPT_NO_20=@DEPT_NO
                    ");
            sb.Append(" )X WHERE X.EMP_TOTAL>0 ");
            sb.Append(" ORDER BY SCORE_LEVEL_GROUP ");

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@HEAD_EMP_ID", dept_emp_id);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getWSApproveData202410bak(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string dept_no,  string dept_emp_id)
    {
        try
        {
           
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select X.* From (");
            sb.Append(@" SELECT W2.SUB_DESC AS WS_CD_DESC,W1.WS_CD,W1.SCORE_LEVEL_GROUP,(W1.WS_CD+'-'+W1.SCORE_LEVEL_GROUP+'-'+ W2.SUB_DESC) ARG,
                           (SELECT COUNT(T2.EMP_ID)
		                    FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                       TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                    WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@HEAD_EMP_ID AND
			                       T2.WS_CD=W1.WS_CD AND (  ISNULL(W1.SCORE_LEVEL_GROUP,'')='' OR (ISNULL(W1.SCORE_LEVEL_GROUP,'')<>'' AND CHARINDEX(T2.LEVEL_CD,W1.SCORE_LEVEL_GROUP)>0 )) AND
			                       T1.LEVEL_RATE IN(
			                       SELECT Y.LEVEL_RATE
				                    FROM					                    
						                (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
					                    TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                    where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID

			                       )) EMP_TOTAL	,   
			                        (SELECT COUNT(T2.EMP_ID)
		                    FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                       TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                    WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@HEAD_EMP_ID AND
			                       T2.WS_CD=W1.WS_CD  AND (  ISNULL(W1.SCORE_LEVEL_GROUP,'')='' OR (ISNULL(W1.SCORE_LEVEL_GROUP,'')<>'' AND CHARINDEX(T2.LEVEL_CD,W1.SCORE_LEVEL_GROUP)>0 )) AND
			                       T2.SCORE_DEPT='A' AND
			                        T1.LEVEL_RATE IN(
			                       SELECT Y.LEVEL_RATE
				                    FROM					                    
						                (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
					                    TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                    where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID

			                       )) SCORE_A,   
			                        (SELECT COUNT(T2.EMP_ID)
		                    FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                       TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                    WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@HEAD_EMP_ID AND
			                       T2.WS_CD=W1.WS_CD  AND (  ISNULL(W1.SCORE_LEVEL_GROUP,'')='' OR (ISNULL(W1.SCORE_LEVEL_GROUP,'')<>'' AND CHARINDEX(T2.LEVEL_CD,W1.SCORE_LEVEL_GROUP)>0 )) AND
			                       T2.SCORE_DEPT='B' AND
			                        T1.LEVEL_RATE IN(
			                       SELECT Y.LEVEL_RATE
				                    FROM					                    
						                (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
					                    TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                    where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID

			                       )) SCORE_B,   
			                        (SELECT COUNT(T2.EMP_ID)
		                    FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                       TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                    WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@HEAD_EMP_ID AND
			                       T2.WS_CD=W1.WS_CD  AND (  ISNULL(W1.SCORE_LEVEL_GROUP,'')='' OR (ISNULL(W1.SCORE_LEVEL_GROUP,'')<>'' AND CHARINDEX(T2.LEVEL_CD,W1.SCORE_LEVEL_GROUP)>0 )) AND
			                       T2.SCORE_DEPT='C' AND
			                        T1.LEVEL_RATE IN(
			                       SELECT Y.LEVEL_RATE
				                    FROM					                    
						                (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
					                    TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                    where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID

			                       )) SCORE_C,   
			                        (SELECT COUNT(T2.EMP_ID)
		                    FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                       TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                    WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@HEAD_EMP_ID AND
			                       T2.WS_CD=W1.WS_CD  AND (  ISNULL(W1.SCORE_LEVEL_GROUP,'')='' OR (ISNULL(W1.SCORE_LEVEL_GROUP,'')<>'' AND CHARINDEX(T2.LEVEL_CD,W1.SCORE_LEVEL_GROUP)>0 )) AND
			                       T2.SCORE_DEPT='D' AND
			                        T1.LEVEL_RATE IN(
			                       SELECT Y.LEVEL_RATE
				                    FROM					                    
						                (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
					                    TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                    where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID

			                       )) SCORE_D,   
			                        (SELECT COUNT(T2.EMP_ID)
		                    FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                       TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                    WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@HEAD_EMP_ID AND
			                       T2.WS_CD=W1.WS_CD  AND (  ISNULL(W1.SCORE_LEVEL_GROUP,'')='' OR (ISNULL(W1.SCORE_LEVEL_GROUP,'')<>'' AND CHARINDEX(T2.LEVEL_CD,W1.SCORE_LEVEL_GROUP)>0 )) AND
			                       T2.SCORE_DEPT='E' AND
			                        T1.LEVEL_RATE IN(
			                       SELECT Y.LEVEL_RATE
				                    FROM					                    
						                (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
					                    TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                    where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID

			                       )) SCORE_E,
                        (CASE WHEN W1.WS_CD='S' THEN 1 ELSE  CASE WHEN W1.WS_CD='G' THEN 2 ELSE CASE WHEN W1.WS_CD='N' THEN 3 ELSE CASE WHEN W1.WS_CD='W' THEN 4 ELSE 5 END END END END) WS_SORT
                    FROM TB_S_M_ASSESS_DEP20_PEO W1 LEFT JOIN
                        TB_9_M_COMM_D W2 ON W1.WS_CD=W2.SUB_CD AND W2.SYS_CD='HB' and W2.MAIN_CD='WS_CD'
                    WHERE W1.ASSESS_YEAR=@ASSESS_YEAR AND W1.ASSESS_TYPE=@ASSESS_TYPE AND W1.DEPT_NO_20=@DEPT_NO AND W1.IS_MERGER<>'A'
                    ");
            sb.Append(" )X WHERE X.EMP_TOTAL>0 ");
            sb.Append(" ORDER BY (CASE WHEN X.WS_CD='S' THEN 1 ELSE  CASE WHEN X.WS_CD='G' THEN 2 ELSE CASE WHEN X.WS_CD='N' THEN 3 ELSE CASE WHEN X.WS_CD='W' THEN 4 ELSE 5 END END END END) ");
            
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@HEAD_EMP_ID", dept_emp_id);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getWSApproveCount(int startRowIndex, int maximumRows, string assess_year, string assess_type, string dept_no, string dept_emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNt(X.POINT_GROUP) total_record From (");
            sb.Append(@" SELECT distinct W1.POINT_GROUP,
                           (SELECT COUNT(T2.EMP_ID)
		                    FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                       TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                    WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@HEAD_EMP_ID AND
			                        T2.WS_CD+'-'+T2.LEVEL_CD IN(SELECT WS_CD+'-'+LEVEL_CD FROM TB_S_M_ASSESS_POINT_YEAR WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=@ASSESS_TYPE AND POINT_GROUP=W1.POINT_GROUP) AND
			                        T1.LEVEL_RATE IN(
			                       SELECT Y.LEVEL_RATE
				                    FROM					                    
						                (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
					                    TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                    where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID

			                       )) EMP_TOTAL	
                    FROM TB_S_M_ASSESS_DEPTPOINT  W1 
                    WHERE W1.ASSESS_YEAR=@ASSESS_YEAR AND W1.ASSESS_TYPE=@ASSESS_TYPE AND W1.DEPT_NO_20=@DEPT_NO  ");
            sb.Append(" )X WHERE X.EMP_TOTAL>0 "); 


            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@HEAD_EMP_ID", dept_emp_id);
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
    public int getWSApproveCount202410bak(int startRowIndex, int maximumRows, string assess_year, string assess_type, string dept_no, string dept_emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNt(X.SCORE_LEVEL_GROUP) total_record From (");
            sb.Append(@" SELECT W2.SUB_DESC AS WS_CD_DESC,W1.WS_CD,W1.SCORE_LEVEL_GROUP,
                           (SELECT COUNT(T2.EMP_ID)
		                    FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                       TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID
		                    WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@HEAD_EMP_ID AND
			                       T2.WS_CD=W1.WS_CD AND (  ISNULL(W1.SCORE_LEVEL_GROUP,'')='' OR (ISNULL(W1.SCORE_LEVEL_GROUP,'')<>'' AND CHARINDEX(T2.LEVEL_CD,W1.SCORE_LEVEL_GROUP)>0 )) AND
			                        T1.LEVEL_RATE IN(
			                       SELECT Y.LEVEL_RATE
				                    FROM					                    
						                (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
					                    TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                    where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID

			                       )) EMP_TOTAL	
                    FROM TB_S_M_ASSESS_DEP20_PEO W1 LEFT JOIN
                        TB_9_M_COMM_D W2 ON W1.WS_CD=W2.SUB_CD AND W2.SYS_CD='HB' and W2.MAIN_CD='WS_CD'
                    WHERE W1.ASSESS_YEAR=@ASSESS_YEAR AND W1.ASSESS_TYPE=@ASSESS_TYPE AND W1.DEPT_NO_20=@DEPT_NO AND W1.IS_MERGER<>'A' ");
            sb.Append(" )X WHERE X.EMP_TOTAL>0 ");


            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@HEAD_EMP_ID", dept_emp_id);
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
    //取得Dtl2-核計,與合計點數資料
    public DataTable getDtl2PointData(string assess_year, string assess_type, string dept_no, string dept_emp_id,string point_group)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" SELECT distinct W1.POINT_GROUP SCORE_LEVEL_GROUP,
	                            	(W1.POINT_GROUP) ARG,SUM(W1.DEPT_POINT)DEPT_POINT,
                                      ISNULL( (SELECT SUM(T3.POINT)
		                                FROM   TB_S_M_ASSESS_DIRECTOR_D T1 LEFT JOIN
			                                   TB_S_M_ASSESS_TARGET T2  ON T1.ASSESS_YEAR = T2.ASSESS_YEAR  AND T1.ASSESS_TYPE = T2.ASSESS_TYPE AND T1.EMP_ID=T2.EMP_ID LEFT JOIN
								               TB_S_M_ASSESS_GRADEPOINT T3 ON T2.ASSESS_YEAR = T3.ASSESS_YEAR  AND T2.ASSESS_TYPE = T3.ASSESS_TYPE AND T2.SCORE_FINAL=T3.RATING
		                                WHERE  T1.ASSESS_YEAR=W1.ASSESS_YEAR AND T1.ASSESS_TYPE=W1.ASSESS_TYPE AND T2.EMP_ID<>@HEAD_EMP_ID AND
			                                    ISNULL(T2.IS_OUT,'N')='N' AND T2.WS_CD+'-'+T2.LEVEL_CD IN(SELECT WS_CD+'-'+LEVEL_CD FROM TB_S_M_ASSESS_POINT_YEAR WHERE ASSESS_YEAR=W1.ASSESS_YEAR AND ASSESS_TYPE=@ASSESS_TYPE AND POINT_GROUP=W1.POINT_GROUP) AND
			                                   T1.LEVEL_RATE IN(
			                                   SELECT Y.LEVEL_RATE
				                                FROM					                    
						                            (select LEVEL_RATE ,ASSESS_YEAR ,ASSESS_TYPE, DEPT_NO,HEAD_EMP_ID from TB_S_M_ASSESS_DEPT_LEVEL  where  DEPT_LEVEL>='20') X   left join 
					                                TB_S_M_ASSESS_DEPT_LEVEL Y on SUBSTRING(Y.LEVEL_RATE,1,len(X.LEVEL_RATE))=X.LEVEL_RATE  and X.ASSESS_YEAR=Y.ASSESS_YEAR and X.ASSESS_TYPE=Y.ASSESS_TYPE
					                                where X.ASSESS_YEAR=T1.ASSESS_YEAR and X.ASSESS_TYPE=T1.ASSESS_TYPE and X.HEAD_EMP_ID=@HEAD_EMP_ID

			                                   )),0) EMP_TOTAL_POINT
                                FROM TB_S_M_ASSESS_DEPTPOINT  W1 
                                WHERE W1.ASSESS_YEAR=@ASSESS_YEAR AND W1.ASSESS_TYPE=@ASSESS_TYPE AND W1.DEPT_NO_20=@DEPT_NO ");

            if (point_group != "")
            {
                sb.Append("  AND W1.POINT_GROUP=@POINT_GROUP ");
                ht.Add("@POINT_GROUP", point_group);
            }
            sb.Append(" GROUP BY W1.POINT_GROUP,W1.ASSESS_YEAR,W1.ASSESS_TYPE,W1.DEPT_NO_20 ");

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@HEAD_EMP_ID", dept_emp_id);
            

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
}