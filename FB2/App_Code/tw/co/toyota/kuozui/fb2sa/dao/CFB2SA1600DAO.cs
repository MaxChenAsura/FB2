using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
 
/// <summary>
/// CFB2SA160DAO 的摘要描述
/// </summary>
public class CFB2SA1600DAO : BaseDAO
{
    //職務類別敘薪檔
    public string PJOB_CD { get; set; }
    public string SALARY_ID { get; set; }
    public string HIRE_TYPE { get; set; }
    public string START_DT { get; set; }
    public string END_DT { get; set; }
    public string PAY { get; set; }
    public string REMARK { get; set; }


    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2SA1600DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
                         , string status, string hire_type
                        , string salary_id, string pjob_cd, string pjob_desc
                           )
    {
        try
        {
            if (sortExpression.Contains("PJOB_CD"))
                sortExpression = sortExpression.Replace("PJOB_CD", "A.PJOB_CD");
            if (sortExpression.Contains("SALARY_ID"))
                sortExpression = sortExpression.Replace("SALARY_ID", "A.SALARY_ID");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber, ");
            sb.Append(@"  A.PJOB_CD,A.SALARY_ID
                        ,A.HIRE_TYPE
                        ,A.HIRE_TYPE+'-'+C.SUB_DESC as HIRE_TYPE_DESC
                        ,CONVERT(VARCHAR(10),A.START_DT,111)  START_DT
                        ,CONVERT(VARCHAR(10),A.END_DT,111)  END_DT
                        ,A.PAY
                        ,B.PJOB_DESC,D.SALARY_NAME
                        ,A.REMARK
                        from TB_S_M_HIRING_SALARY_PJOB A with (nolock) 
                        left join VW_TB_H_M_PJOB B on A.PJOB_CD = B.PJOB_CD
                        left join TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='SA' and C.MAIN_CD='HIRE_TYPE' and C.SUB_CD= A.HIRE_TYPE and C.IS_VALID='Y'
                        left join TB_S_M_SALARY_ITEM D with (nolock)  on A.SALARY_ID = D.SALARY_ID and D.IS_SALARY = 'Y' ");
            sb.Append(" where 1=1 ");

            //生效中
            if (status == "Y")
            {
                sb.Append("and getdate() between START_DT and END_DT ");
            }
            //失效中
            if (status == "N")
            {
                sb.Append("and END_DT<getdate()  ");
            }
            //待生效中
            if (status == "P")
            {
                sb.Append("and  START_DT>=getdate()   ");
            }

            if (hire_type != "-1")
            {
                sb.Append(" and A.HIRE_TYPE = @HIRE_TYPE ");
                ht.Add("@HIRE_TYPE", hire_type);
            }

            if (salary_id != "-1")
            {
                sb.Append(" and A.SALARY_ID = @SALARY_ID ");
                ht.Add("@SALARY_ID", salary_id);
            }
            if (pjob_cd != "")
            {
                sb.Append(" and A.PJOB_CD like @PJOB_CD+'%' ");
                ht.Add("@PJOB_CD", pjob_cd);
            }
            if (pjob_desc != "")
            {
                sb.Append(" and B.PJOB_DESC like '%'+@PJOB_DESC+'%' ");
                ht.Add("@PJOB_DESC", pjob_desc);
            }

            
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    
    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows
                        , string status, string hire_type
                        , string salary_id, string pjob_cd, string pjob_desc)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_S_M_HIRING_SALARY_PJOB A with (nolock) 
                         left join VW_TB_H_M_PJOB B on A.PJOB_CD = B.PJOB_CD ");
            sb.Append(" where 1=1 ");

            //生效中
            if (status == "Y")
            {
                sb.Append("and getdate() between START_DT and END_DT ");
            }
            //失效中
            if (status == "N")
            {
                sb.Append("and END_DT<getdate()  ");
            }
            //待生效中
            if (status == "P")
            {
                sb.Append("and  START_DT>=getdate()   ");
            }


            if (hire_type != "-1")
            {
                sb.Append(" and A.HIRE_TYPE = @HIRE_TYPE ");
                ht.Add("@HIRE_TYPE", hire_type);
            }


            if (salary_id != "-1")
            {
                sb.Append(" and A.SALARY_ID = @SALARY_ID ");
                ht.Add("@SALARY_ID", salary_id);
            }
            if (pjob_cd != "")
            {
                sb.Append(" and A.PJOB_CD like @PJOB_CD+'%' ");
                ht.Add("@PJOB_CD", pjob_cd);
            }
            if (pjob_desc != "")
            {
                sb.Append(" and B.PJOB_DESC like '%'+@PJOB_DESC+'%' ");
                ht.Add("@PJOB_DESC", pjob_desc);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }

    }

    #endregion

    #region  一括生成 Gridview 資料
    //Gridview 查詢資料
    public DataTable getAllUpdData(int startRowIndex, int maximumRows, string sortExpression
                         , string emp_id, string pjob_cd, string hire_type, string salary_id)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "A.EMP_ID");


            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber, ");
            sb.Append(@" A.EMP_ID
                        ,B.EMP_NAME
                        ,B.JOIN_DT
                        ,A.EFFECT_SDT_B
                        ,A.EFFECT_EDT_B
                        ,A.CHG_AMT_B
                        ,A.PJOB_CD
                        ,A.SALARY_ID
                        ,A.HIRE_TYPE
                        from TB_S_M_HIRING_SALARY_PJOB_ALLUPD A with (nolock) 
                        left join (
                                select EMP_ID,EMP_NAME,JOIN_DT  from TB_H_M_EMP with (nolock) 
                        ) B on A.EMP_ID = B.EMP_ID
                        where 1=1
                        and A.GEN_DT = CONVERT(VARCHAR(8),GETDATE(),112)
                        and A.GEN_EMP_ID = @GEN_EMP_ID
                        and A.PJOB_CD = @PJOB_CD
                        and A.SALARY_ID = @SALARY_ID
                        and A.HIRE_TYPE = @HIRE_TYPE ");

            ht.Add("@GEN_EMP_ID", emp_id);
            ht.Add("@PJOB_CD", pjob_cd);
            ht.Add("@SALARY_ID", salary_id);
            ht.Add("@HIRE_TYPE", hire_type );


            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //Gridview 查詢總筆數
    public int getAllUpdCount(int startRowIndex, int maximumRows
                         , string emp_id, string pjob_cd, string hire_type, string salary_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_S_M_HIRING_SALARY_PJOB_ALLUPD ");
            sb.Append(@" where 1=1
                        and GEN_DT = CONVERT(VARCHAR(8),GETDATE(),112)
                        and GEN_EMP_ID=@GEN_EMP_ID
                        and PJOB_CD =@PJOB_CD
                        and SALARY_ID = @SALARY_ID
                        and HIRE_TYPE = @HIRE_TYPE  ");
            ht.Add("@GEN_EMP_ID", emp_id);
            ht.Add("@PJOB_CD", pjob_cd);
            ht.Add("@SALARY_ID", salary_id);
            ht.Add("@HIRE_TYPE", hire_type);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }

    }

    #endregion

    //取得修改資料
    public DataTable getUpdData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" Select 
             A.PJOB_CD
            ,A.PJOB_CD+'-'+B.PJOB_DESC as  PJOB_DESC
            ,A.SALARY_ID
            ,A.SALARY_ID+'-'+D.SALARY_NAME as SALARY_ID_DESC
            ,A.HIRE_TYPE
            ,A.HIRE_TYPE+'-'+C.SUB_DESC as HIRE_TYPE_DESC
            ,CONVERT(VARCHAR(10),A.START_DT,111)  START_DT
            ,A.END_DT
            ,A.PAY
            ,A.REMARK
            ");
            sb.Append(@" from TB_S_M_HIRING_SALARY_PJOB A   with (nolock)
                left join VW_TB_H_M_PJOB B on A.PJOB_CD = B.PJOB_CD
                left join TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='SA' and C.MAIN_CD='HIRE_TYPE' and C.SUB_CD= A.HIRE_TYPE and C.IS_VALID='Y'
                left join TB_S_M_SALARY_ITEM D with (nolock)  on A.SALARY_ID = D.SALARY_ID and  D.IS_SALARY = 'Y'");
            sb.Append(@" 
                where 1=1
                and A.PJOB_CD =@PJOB_CD 
                and A.SALARY_ID=@SALARY_ID 
                and A.HIRE_TYPE =@HIRE_TYPE
                and A.START_DT =@START_DT
            ");

            ht.Add("@PJOB_CD", this.PJOB_CD);
            ht.Add("@SALARY_ID", this.SALARY_ID);
            ht.Add("@HIRE_TYPE", this.HIRE_TYPE);
            ht.Add("@START_DT", this.START_DT);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得薪資項目
    public DataTable getAllSALARY_ID()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SALARY_ID,SALARY_NAME ");
            sb.Append(" from TB_S_M_SALARY_ITEM ");
            sb.Append(" where IS_SALARY = 'Y'");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //新增儲存
    internal void addSave()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_S_M_HIRING_SALARY_PJOB ( ");
            sb.Append(@" PJOB_CD,SALARY_ID,HIRE_TYPE,START_DT,END_DT
                        ,PAY,REMARK
                        ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID ) ");
            sb.Append(" values ( ");
            sb.Append(@" @PJOB_CD,@SALARY_ID,@HIRE_TYPE,@START_DT,@END_DT
                        ,@PAY,@REMARK
                        ,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID ) ");

            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@HIRE_TYPE", HIRE_TYPE);
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@PAY", PAY);
            ht.Add("@REMARK", REMARK);
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

    //修改儲存
    internal void updSave()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_HIRING_SALARY_PJOB  ");
            sb.Append(@"set 
                         PAY = @PAY 
                        ,END_DT = @END_DT
                        ,REMARK = @REMARK
                        ,UPDATED_BY = @UPDATED_BY
                        ,UPDATED_DT = getdate()
                        ,FUNC_ID = @FUNC_ID
                        ");
            sb.Append(" where 1=1 ");
            sb.Append(@" 
                and PJOB_CD =@PJOB_CD 
                and SALARY_ID=@SALARY_ID 
                and HIRE_TYPE =@HIRE_TYPE
                and START_DT =@START_DT
            ");
            //PK值
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@HIRE_TYPE", HIRE_TYPE);
            ht.Add("@START_DT", START_DT);

            //修改值
            ht.Add("@END_DT", END_DT);
            ht.Add("@PAY", PAY);
            ht.Add("@REMARK", REMARK);
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

    //刪除儲存
    public void delSave()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_HIRING_SALARY_PJOB ");
            sb.Append(@" 
                where 1=1
                and PJOB_CD =@PJOB_CD 
                and SALARY_ID=@SALARY_ID 
                and HIRE_TYPE =@HIRE_TYPE
                and START_DT =@START_DT
            ");
            ht.Add("@PJOB_CD", this.PJOB_CD);
            ht.Add("@SALARY_ID", this.SALARY_ID);
            ht.Add("@HIRE_TYPE", this.HIRE_TYPE);
            ht.Add("@START_DT", this.START_DT);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    #region  檢核

    //判斷職務代號
    public int getPJOBCount()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from VW_TB_H_M_PJOB  ");
            sb.Append(" where PJOB_CD=@PJOB_CD ");
            ht.Add("@PJOB_CD", this.PJOB_CD);          

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增-檢核重疊
    public int chekOver (string type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_S_M_HIRING_SALARY_PJOB
            where 1=1
            and PJOB_CD =@PJOB_CD and SALARY_ID=@SALARY_ID and HIRE_TYPE =@HIRE_TYPE
            ");
            if (type == "1")
            {
                sb.Append(" and (@START_DT between START_DT and END_DT or @END_DT  between START_DT and END_DT )  ");
            }
            else
            {
                sb.Append(" and ( START_DT between @START_DT and @END_DT or END_DT   between @START_DT and @END_DT )  ");            
            }

            ht.Add("@PJOB_CD", this.PJOB_CD);
            ht.Add("@SALARY_ID", this.SALARY_ID);
            ht.Add("@HIRE_TYPE", this.HIRE_TYPE);
            ht.Add("@START_DT", this.START_DT);
            ht.Add("@END_DT", this.END_DT);


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //修改 檢核 重疊
    public int chekUpdOver()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_S_M_HIRING_SALARY_PJOB
            where 1=1
            and PJOB_CD =@PJOB_CD and SALARY_ID=@SALARY_ID and HIRE_TYPE =@HIRE_TYPE and START_DT !=@START_DT
            ");
            sb.Append(" and @END_DT  between START_DT and END_DT  ");

            ht.Add("@PJOB_CD", this.PJOB_CD);
            ht.Add("@SALARY_ID", this.SALARY_ID);
            ht.Add("@HIRE_TYPE", this.HIRE_TYPE);
            ht.Add("@START_DT", this.START_DT);
            ht.Add("@END_DT", this.END_DT);


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }
    }

  

    //PK值
    public int chekPK()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_S_M_HIRING_SALARY_PJOB
            where 1=1
            and PJOB_CD =@PJOB_CD 
            and SALARY_ID=@SALARY_ID 
            and HIRE_TYPE =@HIRE_TYPE
            and START_DT =@START_DT
            ");

            ht.Add("@PJOB_CD", this.PJOB_CD);
            ht.Add("@SALARY_ID", this.SALARY_ID);
            ht.Add("@HIRE_TYPE", this.HIRE_TYPE);
            ht.Add("@START_DT", this.START_DT);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //判斷是否生效中
    public int chekIsVaild()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_S_M_HIRING_SALARY_PJOB
            where 1=1
            and PJOB_CD =@PJOB_CD 
            and SALARY_ID=@SALARY_ID 
            and HIRE_TYPE =@HIRE_TYPE
            and START_DT =@START_DT
            and getdate() between start_dt and end_dt
            ");

            ht.Add("@PJOB_CD", this.PJOB_CD);
            ht.Add("@SALARY_ID", this.SALARY_ID);
            ht.Add("@HIRE_TYPE", this.HIRE_TYPE);
            ht.Add("@START_DT", this.START_DT);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //判斷是何種類型(年級-GRADE, 聘用單位-COMPANY_CD)
    public string getHireType()
    {
        try
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"
            if @HIRE_TYPE in ( select SUB_CD from TB_9_M_COMM_D where SYS_CD='SA' and MAIN_CD='HIRE_TYPE' and CODE_VAL1='GRADE')
	            select 'GRADE' as result;
            if @HIRE_TYPE in ( select SUB_CD from TB_9_M_COMM_D where SYS_CD='SA' and MAIN_CD='HIRE_TYPE' and CODE_VAL1='COMPANY_CD')
	            select 'COMPANY_CD' as result;
            ");
            ht.Add("@PJOB_CD", this.PJOB_CD);
            ht.Add("@HIRE_TYPE", this.HIRE_TYPE);


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
                result = dt.Rows[0]["result"].ToString();
            else
                result = "";
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //判斷是否有符合的人員
    public int chekEmp(string type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@"from VW_H_EMP_DATA
			where 1=1
			and pjob_cd=@PJOB_CD	
			and emp_status<>'99'	 
			and join_dt<=getdate()
            ");

            //年級
            if (type == "GRADE")
            {
                sb.Append(" and GRADE = @HIRE_TYPE ");
            }
            //聘用單位
            if (type == "COMPANY_CD")
            {
                sb.Append(" and COMPANY_CD = @HIRE_TYPE  ");
            }

            ht.Add("@PJOB_CD", this.PJOB_CD);
            ht.Add("@HIRE_TYPE", this.HIRE_TYPE);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }
    }


    #endregion


    #region  SP執行
    //一括對象生成
    internal void execSP_S_SA160_GEN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_SA160_GEN");
            ht.Add("@p_PJOB_CD", PJOB_CD);
            ht.Add("@p_SALARY_ID", SALARY_ID);
            ht.Add("@p_HIRE_TYPE", HIRE_TYPE);
            ht.Add("@p_UserID", CREATED_BY);
            ht.Add("@p_FuncID", "FB2SA160");
            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //一括對象 提出簽核
    internal void execSP_S_SA160_SEND()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_SA160_SEND");
            ht.Add("@p_PJOB_CD", PJOB_CD);
            ht.Add("@p_SALARY_ID", SALARY_ID);
            ht.Add("@p_HIRE_TYPE", HIRE_TYPE);
            ht.Add("@p_START_DT", START_DT);
            ht.Add("@p_REMARK", REMARK);
            ht.Add("@p_UserID", CREATED_BY);
            ht.Add("@p_FuncID", "FB2SA160");
            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    #endregion
    

}