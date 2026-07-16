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
/// CFB2SA310DAO 的摘要描述
/// </summary>
public class CFB2SA3100DAO : BaseDAO
{
    // 
    public string HR_CHG_NO { get; set; }
    public string EMP_ID { get; set; }
    public string IS_MAIL { get; set; }
    public string YM { get; set; }

    public string BATCH_NO { get; set; }
    public string MAIL_TITLE { get; set; }
    public string MAIL_DESC { get; set; }
    public string photoPath { get; set; }
    public string fontsPath { get; set; }
    

    //明細檔
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2SA3100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
                         , string ym, string emp_id, string is_mail 
                           )
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "A.EMP_ID");
            if (sortExpression.Contains("HR_CHG_CD"))
                sortExpression = sortExpression.Replace("HR_CHG_CD", "A.HR_CHG_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber, ");
            sb.Append(@"H.EMP_NAME
                        ,A.HR_CHG_CD+'-'+isnull(CHG.HR_CHG_DESC,'') as HR_CHG_CD_DESC
                        ,A.PJOB_CD_OLD+'-'+A.PJOB_DESC_OLD  as PJOB_CD_OLD_DESC
                        ,A.PJOB_CD+'-'+A.PJOB_DESC  as PJOB_CD_DESC
                        ,A.IS_MAIL+'-'+D.SUB_DESC  as IS_MAIL_DESC
                        ,CONVERT(VARCHAR(10),A.START_DT,111)  as STARTDT
                        ,A.* 
                        from TB_S_M_2SCHG_SALARY_EMP  A with (nolock)
                        inner join ( select EMP_ID,EMP_NAME from TB_H_M_EMP  with (nolock) ) H  on A.EMP_ID = H.EMP_ID
                        left join (select HR_CHG_CD,HR_CHG_DESC from TB_H_M_HR_CHANGE_CODE  with (nolock)) CHG  on A.HR_CHG_CD = CHG.HR_CHG_CD
                        left join (select SUB_CD,SUB_DESC from TB_9_M_COMM_D  with (nolock) where SYS_CD='99' and MAIN_CD='IS_MAIL' ) D on A.IS_MAIL = D.SUB_CD
                        where 1=1
                         ");

            if (ym != "")
            {
                sb.Append(" and  CONVERT(VARCHAR(6),A.START_DT,112) =@YM ");
                ht.Add("@YM", ym.Replace("/",""));
            }
            if (emp_id != "")
            {
                sb.Append(" and  A.EMP_ID =@EMP_ID  ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (is_mail != "-1")
            {
                sb.Append(" and A.IS_MAIL = @IS_MAIL ");
                ht.Add("@IS_MAIL", is_mail);
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
                       , string ym, string emp_id, string is_mail 
    )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_S_M_2SCHG_SALARY_EMP A with (nolock) ");
            sb.Append(" where 1=1 ");
            if (ym != "")
            {
                sb.Append(" and  CONVERT(VARCHAR(6),A.START_DT,112) =@YM ");
                ht.Add("@YM", ym.Replace("/", ""));
            }
            if (emp_id != "")
            {
                sb.Append(" and  A.EMP_ID =@EMP_ID  ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (is_mail != "-1")
            {
                sb.Append(" and A.IS_MAIL = @IS_MAIL ");
                ht.Add("@IS_MAIL", is_mail);
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

    #region  DTL Gridview 資料
    //Gridview 查詢資料
    public DataTable getDtlData(int startRowIndex, int maximumRows, string sortExpression
                         , string ym, string emp_id)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "A.EMP_ID");
            if (sortExpression.Contains("HR_CHG_CD"))
                sortExpression = sortExpression.Replace("HR_CHG_CD", "A.HR_CHG_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber, ");
            sb.Append(@" H.EMP_NAME
                        ,A.HR_CHG_CD+'-'+isnull(CHG.HR_CHG_DESC,'') as HR_CHG_CD_DESC
                        ,A.PJOB_CD_OLD+'-'+A.PJOB_DESC_OLD  as PJOB_CD_OLD_DESC
                        ,A.PJOB_CD+'-'+A.PJOB_DESC  as PJOB_CD_DESC
                        ,A.IS_MAIL+'-'+D.SUB_DESC  as IS_MAIL_DESC
                        ,CONVERT(VARCHAR(10),A.START_DT,111)  as STARTDT
                        ,A.* 
                        from TB_S_M_2SCHG_SALARY_EMP  A with (nolock)
                        inner join ( select EMP_ID,EMP_NAME from TB_H_M_EMP  with (nolock) ) H  on A.EMP_ID = H.EMP_ID
                        left join (select HR_CHG_CD,HR_CHG_DESC from TB_H_M_HR_CHANGE_CODE  with (nolock)) CHG  on A.HR_CHG_CD = CHG.HR_CHG_CD
                        left join (select SUB_CD,SUB_DESC from TB_9_M_COMM_D  with (nolock) where SYS_CD='99' and MAIN_CD='IS_MAIL' ) D on A.IS_MAIL = D.SUB_CD
                        where 1=1
                         ");

            if (ym != "")
            {
                sb.Append(" and  CONVERT(VARCHAR(6),A.START_DT,112) =@YM ");
                ht.Add("@YM", ym.Replace("/", ""));
            }
            if (emp_id != "")
            {
                sb.Append(" and  A.EMP_ID =@EMP_ID  ");
                ht.Add("@EMP_ID", emp_id);
            }
            //未寄信
            sb.Append(" and A.IS_MAIL = @IS_MAIL ");
            ht.Add("@IS_MAIL", "N");


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
    public int getDtlCount(int startRowIndex, int maximumRows, string ym, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_S_M_2SCHG_SALARY_EMP A with (nolock) ");
            sb.Append(" where 1=1 ");
            
            if (ym != "")
            {
                sb.Append(" and  CONVERT(VARCHAR(6),A.START_DT,112) =@YM ");
                ht.Add("@YM", ym.Replace("/", ""));
            }
            
            if (emp_id != "")
            {
                sb.Append(" and  A.EMP_ID =@EMP_ID  ");
                ht.Add("@EMP_ID", emp_id);
            }

            sb.Append(" and A.IS_MAIL = @IS_MAIL ");
            ht.Add("@IS_MAIL", "N");


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


    //不寄送(修改)
    public void updSave()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"update TB_S_M_2SCHG_SALARY_EMP
                    SET 
                      IS_MAIL = @IS_MAIL 
                    , MAIL_DT = null
                    ,UPDATED_BY = @UPDATED_BY
                    ,UPDATED_DT = getdate()
                    ,FUNC_ID    = @FUNC_ID
                    ");
            sb.Append(@" 
                where 1=1
                and HR_CHG_NO =@HR_CHG_NO 
                and EMP_ID=@EMP_ID 
            ");
            ht.Add("@IS_MAIL", IS_MAIL);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@HR_CHG_NO", HR_CHG_NO);
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

 
     
    //EXCEL匯出
    public DataTable getExcelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"select H.EMP_NAME
                        ,A.HR_CHG_CD+'-'+isnull(CHG.HR_CHG_DESC,'') as HR_CHG_CD_DESC
                        ,A.PJOB_CD_OLD+'-'+A.PJOB_DESC_OLD  as PJOB_CD_OLD_DESC
                        ,A.PJOB_CD+'-'+A.PJOB_DESC  as PJOB_CD_DESC
                        ,A.IS_MAIL+'-'+D.SUB_DESC  as IS_MAIL_DESC
                        ,CONVERT(VARCHAR(10),A.START_DT,111)  as STARTDT
                        ,A.* 
                        from TB_S_M_2SCHG_SALARY_EMP  A with (nolock)
                        inner join ( select EMP_ID,EMP_NAME from TB_H_M_EMP  with (nolock) ) H  on A.EMP_ID = H.EMP_ID
                        left join (select HR_CHG_CD,HR_CHG_DESC from TB_H_M_HR_CHANGE_CODE  with (nolock)) CHG  on A.HR_CHG_CD = CHG.HR_CHG_CD
                        left join (select SUB_CD,SUB_DESC from TB_9_M_COMM_D  with (nolock) where SYS_CD='99' and MAIN_CD='IS_MAIL' ) D on A.IS_MAIL = D.SUB_CD
                        where 1=1
                         ");

            if (YM != "")
            {
                sb.Append(" and  CONVERT(VARCHAR(6),A.START_DT,112) =@YM ");
                ht.Add("@YM", YM.Replace("/", ""));
            }
            if (EMP_ID != "")
            {
                sb.Append(" and  A.EMP_ID =@EMP_ID  ");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (IS_MAIL != "-1")
            {
                sb.Append(" and A.IS_MAIL = @IS_MAIL ");
                ht.Add("@IS_MAIL", IS_MAIL);
            }

            sb.Append(" order by START_DT desc ,EMP_ID ");


            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }


    #region  SP執行

    //對象生成
    internal void exec_GEN_2SCHG_SALARY_EMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_GEN_2SCHG_SALARY_EMP");
            ht.Add("@p_YM", YM);
            ht.Add("@p_UserID", CREATED_BY);
            ht.Add("@p_FuncID", FUNC_ID);
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

   
    //新增至寄信主檔
    internal void insert_Mail_H()
    {
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" insert into TB_S_M_2SCHG_SALARY_EMP_MAIL_H
                        (BATCH_NO,MAIL_DT,MAIL_TITLE,MAIL_DESC,SENDTO_MAIL
                        ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID
                        ) 
                        select @BATCH_NO
                        ,convert(varchar(8),getdate(),112) 
                        ,@MAIL_TITLE
                        ,@MAIL_DESC
                        ,(select SALARY_EMAIL from TB_H_M_EMP with (nolock) where EMP_ID =@CREATED_BY)
                        ,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID ");
            ht.Add("@BATCH_NO", BATCH_NO);
            ht.Add("@MAIL_TITLE", MAIL_TITLE);
            ht.Add("@MAIL_DESC", MAIL_DESC);
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

    //修改寄信明細檔
    internal void update_Mail_D(string empid,string hrchgno)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" update TB_S_M_2SCHG_SALARY_EMP_MAIL_D
                        set MAIL_YN ='Y'
                        ,MAIL_DT = CONVERT(VARCHAR(8),GETDATE(),112) 
                        ,UPDATED_BY = @UPDATED_BY
                        ,UPDATED_DT = GETDATE()
                        ,FUNC_ID    = @FUNC_ID
                        where 1=1
                        and HR_CHG_NO = @HR_CHG_NO
                        and EMP_ID = @EMP_ID
                        and BATCH_NO = @BATCH_NO
                        ");
            ht.Add("@HR_CHG_NO", hrchgno);
            ht.Add("@EMP_ID", empid);
            ht.Add("@BATCH_NO", BATCH_NO);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.Execute(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //修改對象檔
    internal void update_2SCHG(string empid, string hrchgno)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" update TB_S_M_2SCHG_SALARY_EMP
                        set IS_MAIL ='Y'
                        ,MAIL_DT = GETDATE()
                        ,UPDATED_BY = @UPDATED_BY
                        ,UPDATED_DT = GETDATE()
                        ,FUNC_ID    = @FUNC_ID
                        where 1=1
                        and HR_CHG_NO = @HR_CHG_NO
                        and EMP_ID = @EMP_ID
                        ");
            ht.Add("@HR_CHG_NO", hrchgno);
            ht.Add("@EMP_ID", empid);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.Execute(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增至寄信明細檔(2S)
    internal void insert_Mail_D()
    {
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" insert into TB_S_M_2SCHG_SALARY_EMP_MAIL_D
                        (HR_CHG_NO,EMP_ID,BATCH_NO,MAIL_DT,EMAIL
                        ,MAIL_YN
                        ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID
                        )
                        select A.HR_CHG_NO,A.EMP_ID,@BATCH_NO,convert(varchar(8),getdate(),112) 
                        ,B.SALARY_EMAIL
                        ,'N'
                        ,@CREATED_BY,getdate(),@CREATED_BY,getdate(),@FUNC_ID
                        from TB_S_M_2SCHG_SALARY_EMP A with (nolock)
                        left join (select EMP_ID,SALARY_EMAIL from TB_H_M_EMP  with (nolock) ) B  on A.EMP_ID = B.EMP_ID
                        where 1=1
                        and  A.EMP_ID =iif(isnull(@EMP_ID,'')='',A.EMP_ID,@EMP_ID)
                        and  CONVERT(VARCHAR(6),A.START_DT,112) =@YM
                        and  A.IS_MAIL='N'
                         ");
            ht.Add("@BATCH_NO", BATCH_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@YM", YM);
            ht.Add("@MAIL_TITLE", MAIL_TITLE);
            ht.Add("@MAIL_DESC", MAIL_DESC);
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

    //取得收件者清單
    internal DataTable getMailData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" select 
                         MH.MAIL_TITLE
                        ,MH.MAIL_DESC
                        ,MH.SENDTO_MAIL
                        ,MH.SENDTO_MAIL   as MAIL_USER
                        ,EUR.LICENSE_ID   as PW_USER
                        ,MD.EMAIL		  as MAIL_2S
                        ,E2S.LICENSE_ID   as PW_2S
                        ,E2S.EMP_NAME
                        , CONVERT(VARCHAR(6),CHG.START_DT,112) as EFFECT_YM
                        ,CHG.HR_CHG_NO
                        ,CHG.EMP_ID
                        ,CHG.HR_CHG_CD
                        ,CHG.START_DT
                        ,CHG.DEPT_NO
                        ,CHG.DEPT_FULL_NAME
                        ,CHG.PJOB_CD_OLD
                        ,CHG.PJOB_DESC_OLD
                        ,CHG.ABILITY_PAY_OLD
                        ,CHG.PJOB_PAY_OLD
                        ,CHG.LEVEL_CD
                        ,CHG.PJOB_CD
                        ,CHG.PJOB_DESC
                        ,CHG.ABILITY_PAY
                        ,CHG.PJOB_PAY
                        ,CHG.FOOD_PAY
                        from 
                        (
	                        select * 
	                        from TB_S_M_2SCHG_SALARY_EMP_MAIL_H  with (nolock)
	                        where BATCH_NO= @BATCH_NO
                        ) MH
                        inner join 
                        (
	                        select  HR_CHG_NO,EMP_ID,BATCH_NO,EMAIL
	                        from  TB_S_M_2SCHG_SALARY_EMP_MAIL_D with (nolock) 
	                        where BATCH_NO= @BATCH_NO  and MAIL_YN ='N' 
                        ) MD on MH.BATCH_NO = MD.BATCH_NO
                        inner join 
                        (
	                        select EMP_ID,EMP_NAME,UPPER(LICENSE_ID) as LICENSE_ID from TB_H_M_EMP with (nolock) 
                        ) E2S on MD.EMP_ID = E2S.EMP_ID
                        inner join 
                        (
	                        select EMP_ID,UPPER(LICENSE_ID) as LICENSE_ID  from TB_H_M_EMP with (nolock)  where EMP_ID = @CREATED_BY
                        ) EUR on  1=1
                        inner join TB_S_M_2SCHG_SALARY_EMP CHG on CHG.HR_CHG_NO = MD.HR_CHG_NO and  CHG.EMP_ID = MD.EMP_ID  
                        where 1=1
                       ");
            ht.Add("@BATCH_NO", BATCH_NO);
            ht.Add("@CREATED_BY", CREATED_BY);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //取得上次收件的主旨及內容
    public DataTable getMailTitle()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" SELECT MAIL_TITLE,MAIL_DESC
                        FROM TB_S_M_2SCHG_SALARY_EMP_MAIL_H
                        WHERE BATCH_NO = (SELECT MAX(BATCH_NO)  FROM TB_S_M_2SCHG_SALARY_EMP_MAIL_H )

                       ");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    #endregion


}