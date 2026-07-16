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
/// WFB2IA0100 的摘要描述
/// </summary>
public class CFB2SJ0410DAO : BaseDAO
{
    public string ASSESS_TYPE { get; set; }
    public string ASSESS_YEAR { get; set; }
    public string EMP_ID { get; set; }
    public string SUGGEST_SCORE { get; set; }
    public string SUGGEST_REMARK { get; set; }
    public string SUGGEST_EMP_ID { get; set; }
    public string AUDRESULT1_YN { get; set; }
    public string AUDRESULT2_YN { get; set; }
    public string AUDRESULT3_YN { get; set; }
    public string SUGGEST_FILE_NAME { get; set; }
    public string MA_EMP_ID { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SJ0410DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string emp_id, string suggest_score, string created_by)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" T.* FROM( SELECT ");
            sb.Append(@"  A.ASSESS_TYPE, A.ASSESS_TYPE+'-'+B.SUB_DESC as ASSESS_TYPE_DESC, A.EMP_ID, D.EMP_NAME, A.SUGGEST_SCORE,
                          A.SUGGEST_EMP_ID ,C.EMP_NAME SUGGEST_EMP_NAME, D.LEVEL_CD, D.WS_CD, D.PJOB_DESC, 
                          (CASE WHEN A.ASSESS_TYPE='1' then D.SCORE_1H_1 else SCORE_2H_1 end) SCORE_H_1, SCORE_DEPT, SCORE_FINAL,
                          A.AUDRESULT1_YN,  A.AUDRESULT2_YN,  A.AUDRESULT3_YN, G.EMP_NAME CREATED_NAME, A.CREATED_BY,
                          (CASE WHEN A.AUDRESULT1_YN='X' then '未審' else CASE WHEN A.AUDRESULT1_YN='Y' then '核可' else CASE WHEN A.AUDRESULT1_YN='E' THEN 'X' ELSE '不核可' END END END) AUDRESULT1_YN_DESC,
                          (CASE WHEN A.AUDRESULT2_YN='X' then '未審' else CASE WHEN A.AUDRESULT2_YN='Y' then '核可' else CASE WHEN A.AUDRESULT2_YN='E' THEN 'X' ELSE '不核可' END END END) AUDRESULT2_YN_DESC,
                          (CASE WHEN A.AUDRESULT3_YN='X' then '未審' else CASE WHEN A.AUDRESULT3_YN='Y' then '核可' else CASE WHEN A.AUDRESULT3_YN='E' THEN 'X' ELSE '不核可' END END END) AUDRESULT3_YN_DESC                         
                         from TB_S_M_ASSESS_EMP_SUGGEST A LEFT jOIN
                              TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='SJ' and B.MAIN_CD='ASSESS_TYPE' and B.SUB_CD= A.ASSESS_TYPE and B.IS_VALID='Y' left join
                              TB_H_M_EMP C on A.SUGGEST_EMP_ID=C.EMP_ID left join 
                              TB_S_M_ASSESS_TARGET D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.EMP_ID=D.EMP_ID left join
							  TB_H_R_DEPT_DATA_AD E on D.DEPT_NO=E.DEPT_NO left join
							  TB_H_R_DEPT_DATA_AD F on F.DEPT_NO=E.DEPT_NO_20 left join
                              TB_H_M_EMP G on G.EMP_ID= A.CREATED_BY  ");
            //sb.Append(" where 1=1 and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and (E.HEAD_EMP_ID=@CREATED_BY or F.HEAD_EMP_ID=@CREATED_BY) ");
            sb.Append(" where 1=1 and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE   ");
            sb.Append(@"
                        and D.DEPT_NO in(
                            select SC.DEPT_NO 
                            from TB_H_R_DEPT_DATA_AD SA left join
                                 TB_S_M_ASSESS_DEPT_LEVEL SB on SA. DEPT_NO=SB.DEPT_NO left join 
	                             TB_S_M_ASSESS_DEPT_LEVEL SC on SUBSTRING(SC.LEVEL_RATE,1,len(SB.LEVEL_RATE))=SB.LEVEL_RATE 
                            where SA.HEAD_EMP_ID=@CREATED_BY and SB.ASSESS_YEAR=D.ASSESS_YEAR and SB.ASSESS_TYPE=D.ASSESS_TYPE 
                            union
                            select Q.DEPT_NO  
	                        from TB_S_M_ASSESS_DEPT20_MA O left join   
		                         TB_S_M_ASSESS_DEPT_LEVEL P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE = P.ASSESS_TYPE and O.DEPT_NO_20=P.DEPT_NO and P.DEPT_LEVEL>='20' left join  
		                         TB_S_M_ASSESS_DEPT_LEVEL Q on P.ASSESS_YEAR=Q.ASSESS_YEAR and P.ASSESS_TYPE = Q.ASSESS_TYPE and SUBSTRING(Q.LEVEL_RATE,1,len(P.LEVEL_RATE))=P.LEVEL_RATE  
	                        where O.MA_EMP_ID=@CREATED_BY and O.ASSESS_YEAR=D.ASSESS_YEAR and O.ASSESS_TYPE=D.ASSESS_TYPE 
                                         )
                ");
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@CREATED_BY", created_by);
            sb.Append(" and A.CREATED_BY =@CREATED_BY ");
            ht.Add("@EMP_ID", emp_id);
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID =@EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            if (suggest_score != "-1")
            {
                sb.Append(" and A.SUGGEST_SCORE=@SUGGEST_SCORE ");
                ht.Add("@SUGGEST_SCORE", suggest_score);
            }

            sb.Append(" ) T ");
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

    public int getCount(int startRowIndex, int maximumRows, string assess_year, string assess_type, string emp_id, string suggest_score, string created_by)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select COUNT(*) total_record from TB_S_M_ASSESS_EMP_SUGGEST A LEFT jOIN
                              TB_S_M_ASSESS_TARGET D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.EMP_ID=D.EMP_ID left join
							  TB_H_R_DEPT_DATA_AD E on D.DEPT_NO=E.DEPT_NO left join
							  TB_H_R_DEPT_DATA_AD F on F.DEPT_NO=E.DEPT_NO_20 ");
            //sb.Append(" where 1=1 and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE and (E.HEAD_EMP_ID=@CREATED_BY or F.HEAD_EMP_ID=@CREATED_BY) ");
            sb.Append(" where 1=1 and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE  ");
            sb.Append(@"
                        and D.DEPT_NO in(
                            select SC.DEPT_NO 
                            from TB_H_R_DEPT_DATA_AD SA left join
                                 TB_S_M_ASSESS_DEPT_LEVEL SB on SA. DEPT_NO=SB.DEPT_NO left join 
	                             TB_S_M_ASSESS_DEPT_LEVEL SC on SUBSTRING(SC.LEVEL_RATE,1,len(SB.LEVEL_RATE))=SB.LEVEL_RATE 
                            where SA.HEAD_EMP_ID=@CREATED_BY and SB.ASSESS_YEAR=D.ASSESS_YEAR and SB.ASSESS_TYPE=D.ASSESS_TYPE 
                            union
                            select Q.DEPT_NO  
	                        from TB_S_M_ASSESS_DEPT20_MA O left join   
		                         TB_S_M_ASSESS_DEPT_LEVEL P on O.ASSESS_YEAR=P.ASSESS_YEAR and O.ASSESS_TYPE = P.ASSESS_TYPE and O.DEPT_NO_20=P.DEPT_NO and P.DEPT_LEVEL>='20' left join  
		                         TB_S_M_ASSESS_DEPT_LEVEL Q on P.ASSESS_YEAR=Q.ASSESS_YEAR and P.ASSESS_TYPE = Q.ASSESS_TYPE and SUBSTRING(Q.LEVEL_RATE,1,len(P.LEVEL_RATE))=P.LEVEL_RATE  
	                        where O.MA_EMP_ID=@CREATED_BY and O.ASSESS_YEAR=D.ASSESS_YEAR and O.ASSESS_TYPE=D.ASSESS_TYPE 
                                         )
                ");
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@CREATED_BY", created_by);
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID =@EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            if (suggest_score != "-1")
            {
                sb.Append(" and A.SUGGEST_SCORE=@SUGGEST_SCORE ");
                ht.Add("@SUGGEST_SCORE", suggest_score);
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
            sb.Append("select EMP_ID from TB_S_M_ASSESS_EMP_SUGGEST ");
            sb.Append(" where EMP_ID = @EMP_ID and ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);

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
                          A.ASSESS_YEAR, A.ASSESS_TYPE, A.ASSESS_TYPE+'-'+B.SUB_DESC as ASSESS_TYPE_DESC, A.EMP_ID, D.EMP_NAME, A.SUGGEST_SCORE,
                          A.SUGGEST_REMARK, A.SUGGEST_EMP_ID, A.SUGGEST_FILE_NAME, C.EMP_NAME SUGGEST_EMP_NAME, D.LEVEL_CD, D.WS_CD, D.PJOB_DESC, 
                          D.DEPT_NAME, D.AGE, D.WORK_YEARS, D.RECENT_LEVEL_WORK_YEARS, D.DISTING_REMARK,A.CREATED_BY ,E.EMP_NAME CREATED_NAME,
                          D.SCORE_1H_1, D.SCORE_1H_2, D.SCORE_1H_3, D.SCORE_2H_1, D.SCORE_2H_2, D.SCORE_2H_3,D.SCORE_DEPT,
                          isnull(D.LEAVE_O+D.LEAVE_P,0)LEAVE_OP,D.LEAVE_Q,isnull(D.LEAVE_A+D.LEAVE_B,0)LEAVE_AB,
						  F.HEAD_EMP_ID ,G.EMP_ID_DEPT20 DEPT20_EMP_ID, G.MA_EMP_ID MA_B_EMP_ID, H.MA_EMP_ID MA_A_EMP_ID,A.SUGGEST_FILE_NAME,
                           A.AUDRESULT1_YN, A.AUDRESULT2_YN, A.AUDRESULT3_YN, A.CREATED_BY,
                          (CASE WHEN A.AUDRESULT1_YN='X' then '未審' else CASE WHEN A.AUDRESULT1_YN='Y' then '核可' else CASE WHEN A.AUDRESULT1_YN='E' THEN 'X' ELSE '不核可' END END END) AUDRESULT1_YN_DESC,
                          (CASE WHEN A.AUDRESULT2_YN='X' then '未審' else CASE WHEN A.AUDRESULT2_YN='Y' then '核可' else CASE WHEN A.AUDRESULT2_YN='E' THEN 'X' ELSE '不核可' END END END) AUDRESULT2_YN_DESC,
                          (CASE WHEN A.AUDRESULT3_YN='X' then '未審' else CASE WHEN A.AUDRESULT3_YN='Y' then '核可' else CASE WHEN A.AUDRESULT3_YN='E' THEN 'X' ELSE '不核可' END END END) AUDRESULT3_YN_DESC                         
                         from TB_S_M_ASSESS_EMP_SUGGEST A LEFT jOIN
                              TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='SJ' and B.MAIN_CD='ASSESS_TYPE' and B.SUB_CD= A.ASSESS_TYPE and B.IS_VALID='Y' left join
                              TB_H_M_EMP C on A.SUGGEST_EMP_ID=C.EMP_ID left join 
                              TB_S_M_ASSESS_TARGET D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.EMP_ID=D.EMP_ID left join
                              TB_H_M_EMP E on A.CREATED_BY=E.EMP_ID  left join
							 TB_H_R_DEPT_DATA_AD F on D.DEPT_NO=F.DEPT_NO left join
							 TB_S_M_ASSESS_DEPT20_MA G on A.ASSESS_YEAR=G.ASSESS_YEAR and A.ASSESS_TYPE=G.ASSESS_TYPE and F.DEPT_NO_20=G.DEPT_NO_20 and G.MA_TYPE='A' left join
							 TB_S_M_ASSESS_DEPT20_MA H on A.ASSESS_YEAR=H.ASSESS_YEAR and A.ASSESS_TYPE=H.ASSESS_TYPE and G.DEPT_NO_20=H.DEPT_NO_20 and H.MA_TYPE='B'
            ");
           
            sb.Append(@" 
                where 1=1
                and A.EMP_ID =@EMP_ID and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE
            ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", this.EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增 TB_S_M_ASSESS_EMP_SUGGEST
    public void addEMP_SUGGEST()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_S_M_ASSESS_EMP_SUGGEST ( ");
            sb.Append(" ASSESS_YEAR, ASSESS_TYPE,EMP_ID,SUGGEST_SCORE,SUGGEST_REMARK,SUGGEST_EMP_ID, SUGGEST_FILE_NAME ");
            sb.Append(" ,AUDRESULT1_YN,AUDRESULT2_YN,AUDRESULT3_YN ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append("  @ASSESS_YEAR, @ASSESS_TYPE,@EMP_ID,@SUGGEST_SCORE,@SUGGEST_REMARK,@SUGGEST_EMP_ID, @SUGGEST_FILE_NAME ");
            sb.Append(" ,@AUDRESULT1_YN,@AUDRESULT2_YN,@AUDRESULT3_YN ");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SUGGEST_SCORE", SUGGEST_SCORE);
            ht.Add("@SUGGEST_REMARK", SUGGEST_REMARK);
            ht.Add("@SUGGEST_EMP_ID", SUGGEST_EMP_ID);
            ht.Add("@SUGGEST_FILE_NAME", SUGGEST_FILE_NAME);
            ht.Add("@AUDRESULT1_YN", AUDRESULT1_YN);
            ht.Add("@AUDRESULT2_YN", AUDRESULT2_YN);
            ht.Add("@AUDRESULT3_YN", AUDRESULT3_YN);

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

    //更新 TB_S_M_ASSESS_EMP_SUGGEST
    public void updateEMP_SUGGEST()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_ASSESS_EMP_SUGGEST ");
            sb.Append(" set SUGGEST_SCORE=@SUGGEST_SCORE,");
            sb.Append(" SUGGEST_REMARK=@SUGGEST_REMARK,SUGGEST_EMP_ID=@SUGGEST_EMP_ID,SUGGEST_FILE_NAME=@SUGGEST_FILE_NAME, ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where EMP_ID =@EMP_ID and ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE ");

            ht.Add("@SUGGEST_SCORE", SUGGEST_SCORE);
            ht.Add("@SUGGEST_REMARK", SUGGEST_REMARK);
            ht.Add("@SUGGEST_EMP_ID", SUGGEST_EMP_ID);
            ht.Add("@SUGGEST_FILE_NAME", SUGGEST_FILE_NAME);
            ht.Add("@EMP_ID", EMP_ID);
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

    //刪除 TB_S_M_ASSESS_EMP_SUGGEST
    public void deleteEMP_SUGGEST(string assess_year, string assess_type, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.AppendLine(" update TB_S_M_ASSESS_EMP_SUGGEST set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SJ0410' ");
            sb.AppendLine(" where EMP_ID = @EMP_ID and ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE ; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_S_M_ASSESS_EMP_SUGGEST ");
            sb.Append(" where EMP_ID = @EMP_ID and ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE ; ");
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@EMP_ID", emp_id);
            dbConn.ExecuteT(sb, ht, true);
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
                                A.ASSESS_YEAR, A.ASSESS_TYPE, (A.ASSESS_TYPE+'-'+C.SUB_DESC) ASSESS_TYPE_DESC , A.EMP_ID, A.EMP_NAME, isnull(A.IS_OUT,'N') IS_OUT,
	                            A.LEVEL_CD, A.WS_CD,  A.WS_CD+'-'+B.SUB_DESC as WS_CD_DESC,A.PJOB_DESC, A.DEPT_NAME,
	                            isnull((select sum(MNG_GRADE) from TB_S_M_ASSESS_SCORE WHERE ASSESS_YEAR=A.ASSESS_YEAR and ASSESS_TYPE=A.ASSESS_TYPE and EMP_ID=A.EMP_ID),0) MNG_GRADE,
	                            A.SCORE_DEPT,A.SCORE_FINAL,A.SCORE_1H_1,A.SCORE_1H_2,A.SCORE_1H_3,A.SCORE_2H_1,A.SCORE_2H_2,A.SCORE_2H_3,
	                            A.RECENT_LEVEL_WORK_YEARS, A.AGE, A.WORK_YEARS,isnull(A.DISTING_REMARK,'') DISTING_REMARK, isnull(A.RECOMM_DESC,'') RECOMM_DESC, isnull(A.COMMENTS,'') COMMENTS,
						        isnull(A.LEAVE_O+A.LEAVE_P,0)LEAVE_OP,LEAVE_Q,isnull(A.LEAVE_A+A.LEAVE_B,0)LEAVE_AB,isnull(A.LIMIT_RATE,'') LIMIT_RATE,
						        D.HEAD_EMP_ID ,E.EMP_ID_DEPT20 DEPT20_EMP_ID, F.MA_EMP_ID MA_B_EMP_ID, E.MA_EMP_ID MA_A_EMP_ID, H.LEVEL_RATE
                        FROM TB_S_M_ASSESS_TARGET A  left join
                             TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='HB' and B.MAIN_CD='WS_CD' and B.SUB_CD= A.WS_CD and B.IS_VALID='Y'  left join
							 TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='SJ' and C.MAIN_CD='ASSESS_TYPE' and C.SUB_CD= A.ASSESS_TYPE and C.IS_VALID='Y' left join
							 TB_H_R_DEPT_DATA_AD D on A.DEPT_NO=D.DEPT_NO left join
							 TB_S_M_ASSESS_DEPT20_MA E on A.ASSESS_YEAR=E.ASSESS_YEAR and A.ASSESS_TYPE=E.ASSESS_TYPE and D.DEPT_NO_20=E.DEPT_NO_20 and E.MA_TYPE='A' left join
							 TB_S_M_ASSESS_DEPT20_MA F on A.ASSESS_YEAR=F.ASSESS_YEAR and A.ASSESS_TYPE=F.ASSESS_TYPE and D.DEPT_NO_20=F.DEPT_NO_20 and F.MA_TYPE='B' left join
                             TB_S_M_ASSESS_DEPT_LEVEL G on A.ASSESS_YEAR=G.ASSESS_YEAR and A.ASSESS_TYPE=G.ASSESS_TYPE and A.DEPT_NO=G.DEPT_NO left join
                             (select P.LEVEL_RATE 
                              from TB_H_R_DEPT_DATA_AD O  join 
                                   TB_S_M_ASSESS_DEPT_LEVEL P on O.DEPT_NO=P.DEPT_NO
                               WHERE  P.ASSESS_YEAR=@ASSESS_YEAR and P.ASSESS_TYPE=@ASSESS_TYPE and O.HEAD_EMP_ID=@CREATED_BY) H on CHARINDEX(H.LEVEL_RATE,G.LEVEL_RATE)=1 
                    ");


            sb.Append(@" 
                where 1=1
                and A.ASSESS_YEAR = @ASSESS_YEAR  and  A.ASSESS_TYPE = @ASSESS_TYPE  and A.EMP_ID=@EMP_ID
            ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@CREATED_BY", CREATED_BY);
           
            if (CREATED_BY != "")
            {
                sb.Append(" and H.LEVEL_RATE is not null ");

                //ht.Add("@CREATED_BY", CREATED_BY);
            }
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int getDept20UpSignCount(string assess_year, string assess_type, string emp_id, string dept_level)
    {
        int cnt =0 ;

        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"SELECT count(*) total_record
                        FROM TB_H_M_EMP A LEFT JOIN
					         TB_H_R_DEPT_DATA_AD B on　A.DEPT_NO=B.DEPT_NO LEFT JOIN
                             TB_S_M_ASSESS_DEP20_UP_SIGN C ON B.DEPT_NO_20 = C.DEPT_NO
                        WHERE A.EMP_ID=@EMP_ID and C.ASSESS_YEAR=@ASSESS_YEAR and C.ASSESS_TYPE=@ASSESS_TYPE and C.SIGN_YN<>'Y' and C.DEPT_LEVEL=@DEPT_LEVEL ");

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@DEPT_LEVEL", dept_level);
           

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                cnt = (int)dt.Rows[0]["total_record"];
            }
            //return t;
            return cnt;
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable getFilePath()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CODE_VAL1 from TB_9_M_PARAMETER where TB_9_M_PARAMETER.SYS_CD = 'SM' and TB_9_M_PARAMETER.MAIN_CD = 'ASSESS_PATH'");

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
}