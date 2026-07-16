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
public class CFB2SJ0420DAO : BaseDAO
{
    public string ASSESS_TYPE { get; set; }
    public string ASSESS_YEAR { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_MA_TYPE { get; set; }
    public string SUGGEST_SCORE { get; set; }
    public string SUGGEST_REMARK { get; set; }
    public string SUGGEST_EMP_ID { get; set; }
    public string AUDRESULT1_YN { get; set; }
    public string AUDRESULT2_YN { get; set; }
    public string AUDRESULT3_YN { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SJ0420DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string emp_id, string approve, string ma_emp_id, string suggest_score,string dept_no)
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
                          (CASE WHEN A.ASSESS_TYPE='1' then D.SCORE_1H_1 else SCORE_2H_1 end) SCORE_H_1, 
                          (CASE WHEN A.ASSESS_TYPE='1' then D.SCORE_1H_2 else SCORE_2H_2 end) SCORE_H_2,SCORE_FINAL,
                          A.AUDRESULT1_YN,  A.AUDRESULT2_YN,  A.AUDRESULT3_YN, 
						  E.HEAD_EMP_ID ,F.EMP_ID_DEPT20 DEPT20_EMP_ID, F.MA_EMP_ID MA_A_EMP_ID, G.MA_EMP_ID MA_B_EMP_ID,
                          (CASE WHEN A.AUDRESULT1_YN='X' then '未審' else CASE WHEN A.AUDRESULT1_YN='Y' then '核可' else CASE WHEN A.AUDRESULT1_YN='E' THEN '' ELSE '不核可' END END END) AUDRESULT1_YN_DESC,
                          (CASE WHEN A.AUDRESULT2_YN='X' then '未審' else CASE WHEN A.AUDRESULT2_YN='Y' then '已審核' else CASE WHEN A.AUDRESULT2_YN='E' THEN '' ELSE '已審核' END END END) AUDRESULT2_YN_DESC,
                          (CASE WHEN A.AUDRESULT3_YN='X' then '未審' else CASE WHEN A.AUDRESULT3_YN='Y' then '已審核' else CASE WHEN A.AUDRESULT3_YN='E' THEN '' ELSE '已審核' END END END) AUDRESULT3_YN_DESC                         
                         from TB_S_M_ASSESS_EMP_SUGGEST A LEFT jOIN
                              TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='SJ' and B.MAIN_CD='ASSESS_TYPE' and B.SUB_CD= A.ASSESS_TYPE and B.IS_VALID='Y' left join
                              TB_H_M_EMP C on A.SUGGEST_EMP_ID=C.EMP_ID left join 
                              TB_S_M_ASSESS_TARGET D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.EMP_ID=D.EMP_ID left join
							  TB_H_R_DEPT_DATA_AD E on E.DEPT_NO=D.DEPT_NO left join
							  TB_S_M_ASSESS_DEPT20_MA F on F.ASSESS_YEAR=A.ASSESS_YEAR and F.ASSESS_TYPE=A.ASSESS_TYPE and F.DEPT_NO_20=E.DEPT_NO_20 and F.MA_TYPE='A' left join
							  TB_S_M_ASSESS_DEPT20_MA G on G.ASSESS_YEAR=A.ASSESS_YEAR and G.ASSESS_TYPE=A.ASSESS_TYPE and G.DEPT_NO_20=E.DEPT_NO_20 and G.MA_TYPE='B' ");
            sb.Append(" where 1=1 and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE  ");
            sb.Append(@" and (
                          (F.EMP_ID_DEPT20=@MA_EMP_ID ) or
                          (F.MA_EMP_ID=@MA_EMP_ID and A.AUDRESULT1_YN='Y' and (A.AUDRESULT2_YN='Y' or A.AUDRESULT2_YN='E')) or
                          (G.MA_EMP_ID=@MA_EMP_ID and A.AUDRESULT1_YN='Y')
                        )");

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@MA_EMP_ID", ma_emp_id);
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID =@EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            if (approve != "-1")
            {
                sb.Append(" and ( ");
                sb.Append("  A.AUDRESULT1_YN=@APPROVE or  A.AUDRESULT2_YN=@APPROVE or A.AUDRESULT3_YN=@APPROVE ");
                sb.Append(" ) ");
                ht.Add("@APPROVE", approve);
            }
            if (suggest_score != "-1")
            {
                sb.Append(" and A.SUGGEST_SCORE=@SUGGEST_SCORE ");
                ht.Add("@SUGGEST_SCORE", suggest_score);
            }
            if (dept_no != "")
            {
                sb.Append(" and D.DEPT_NO=@DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
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

    public int getCount(int startRowIndex, int maximumRows, string assess_year, string assess_type, string emp_id, string approve, string ma_emp_id, string suggest_score, string dept_no)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select COUNT(*) total_record from TB_S_M_ASSESS_EMP_SUGGEST A LEFT jOIN
                              TB_S_M_ASSESS_TARGET D on A.ASSESS_YEAR=D.ASSESS_YEAR and A.ASSESS_TYPE=D.ASSESS_TYPE and A.EMP_ID=D.EMP_ID left join
							  TB_H_R_DEPT_DATA_AD E on E.DEPT_NO=D.DEPT_NO left join
							  TB_S_M_ASSESS_DEPT20_MA F on F.ASSESS_YEAR=A.ASSESS_YEAR and F.ASSESS_TYPE=A.ASSESS_TYPE and F.DEPT_NO_20=E.DEPT_NO_20 and F.MA_TYPE='A' left join
							  TB_S_M_ASSESS_DEPT20_MA G on G.ASSESS_YEAR=A.ASSESS_YEAR and G.ASSESS_TYPE=A.ASSESS_TYPE and G.DEPT_NO_20=E.DEPT_NO_20 and G.MA_TYPE='B' ");
            sb.Append(" where 1=1 and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE  ");
            sb.Append(@" and (
                          (F.EMP_ID_DEPT20=@MA_EMP_ID ) or
                          (F.MA_EMP_ID=@MA_EMP_ID and A.AUDRESULT1_YN='Y' and (A.AUDRESULT2_YN='Y' or A.AUDRESULT2_YN='E')) or
                          (G.MA_EMP_ID=@MA_EMP_ID and A.AUDRESULT1_YN='Y')
                        )");

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@MA_EMP_ID", ma_emp_id);
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID =@EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            if (approve != "-1")
            {
                sb.Append(" and ( ");
                sb.Append("  A.AUDRESULT1_YN=@APPROVE or  A.AUDRESULT2_YN=@APPROVE or A.AUDRESULT3_YN=@APPROVE ");
                sb.Append(" ) ");
                ht.Add("@APPROVE", approve);
            }

            if (suggest_score != "-1")
            {
                sb.Append(" and A.SUGGEST_SCORE=@SUGGEST_SCORE ");
                ht.Add("@SUGGEST_SCORE", suggest_score);
            }
            if (dept_no != "")
            {
                sb.Append(" and D.DEPT_NO=@DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
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
                          A.ASSESS_YEAR, A.ASSESS_TYPE, A.ASSESS_TYPE+'-'+B.SUB_DESC as ASSESS_TYPE_DESC, A.EMP_ID, D.EMP_NAME, A.SUGGEST_SCORE,
                          A.SUGGEST_REMARK, A.SUGGEST_EMP_ID, A.SUGGEST_FILE_NAME ,C.EMP_NAME SUGGEST_EMP_NAME, D.LEVEL_CD, D.WS_CD, D.PJOB_DESC, 
                          D.DEPT_NAME, F.DEPT_FULL_NAME,D.AGE, D.WORK_YEARS, D.RECENT_LEVEL_WORK_YEARS, D.DISTING_REMARK,A.CREATED_BY ,E.EMP_NAME CREATED_NAME,
                          D.SCORE_1H_1, D.SCORE_1H_2, D.SCORE_1H_3, D.SCORE_2H_1, D.SCORE_2H_2, D.SCORE_2H_3,D.SCORE_DEPT,
                          isnull(D.LEAVE_O+D.LEAVE_P,0)LEAVE_OP,D.LEAVE_Q,isnull(D.LEAVE_A+D.LEAVE_B,0)LEAVE_AB,
						  F.HEAD_EMP_ID ,G.EMP_ID_DEPT20 DEPT20_EMP_ID, G.MA_EMP_ID MA_A_EMP_ID, H.MA_EMP_ID MA_B_EMP_ID,A.SUGGEST_FILE_NAME,
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
   

    //更新 TB_S_M_ASSESS_EMP_SUGGEST
    public void updateEMP_SUGGEST()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_ASSESS_EMP_SUGGEST ");
            sb.Append(" set ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            if (AUDRESULT1_YN != "-1")
            {
                sb.Append(" , AUDRESULT1_YN=@AUDRESULT1_YN ");
                ht.Add("@AUDRESULT1_YN", AUDRESULT1_YN);
                if (AUDRESULT1_YN == "N")
                {
                    sb.Append(" , AUDRESULT3_YN='E' ");
                    sb.Append(" , AUDRESULT2_YN='E' ");
                }

            }
            if (AUDRESULT2_YN != "-1")
            {
                sb.Append(" , AUDRESULT2_YN=@AUDRESULT2_YN ");
                ht.Add("@AUDRESULT2_YN", AUDRESULT2_YN);
                if (AUDRESULT2_YN == "N")
                {
                    sb.Append(" , AUDRESULT3_YN='E' ");
                }
            }
            if (AUDRESULT3_YN != "-1")
            {
                sb.Append(" , AUDRESULT3_YN=@AUDRESULT3_YN ");
                ht.Add("@AUDRESULT3_YN", AUDRESULT1_YN);
            }
            sb.Append(" where EMP_ID =@EMP_ID and ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE ");
           
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

    
   

    
}