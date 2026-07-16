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
/// CFB2SP1200DAO 的摘要描述
/// </summary>
public class CFB2SP1200DAO : BaseDAO
{
    //s1030基本欄位
    public string EMP_ID { get; set; }
    public string SALARY_SYM { get; set; }
    public string SALARY_EYM { get; set; }
    public string SALARY_DT { get; set; }
    public string PAY_BASIC { get; set; }
    public string APPROVE_REMARK { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    //ACES
    bool isSuper { get; set; }  //是否為supervisor(擔當)

    ACESLib.ACES aces;
    public CFB2SP1200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public int checkIsSalary() {
        try
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" declare @SALARY_DT datetime = (select SALARY_DT from TB_S_M_REWARD_RETIRE where EMP_ID=@EMP_ID);
                        select count(*) resultCount from  TB_S_M_FESTIVAL_H
                        where  FESTIVAL_TYPE='4'
                        and FESTIVAL_DT=@SALARY_DT
                        and FESTIVAL_PAY_DT	=@SALARY_DT
                        and EMP_CD='1'
                        and PROCESS_STATUS='Y'
                      ");
            ht.Add("@EMP_ID", EMP_ID );
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = (int)dt.Rows[0]["resultCount"];
            }
            return result;
        }
        catch (Exception)
        {

            throw;
        }
    
    }


    //駁回
    public void reject(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_REWARD_RETIRE ");
            sb.Append(" set APPROVE_DT = @APPROVE_DT ");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS");
            sb.Append(" ,APPROVE_REMARK = @APPROVE_REMARK");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where EMP_ID = @EMP_ID ");
            //set值
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_STATUS", "B");//B:駁回
            ht.Add("@APPROVE_BY", "");
            ht.Add("@APPROVE_REMARK", APPROVE_REMARK);

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //核可
    public void approve(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_REWARD_RETIRE ");
            sb.Append(" set APPROVE_DT = @APPROVE_DT ");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS");
            sb.Append(" ,APPROVE_REMARK = @APPROVE_REMARK");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where EMP_ID = @EMP_ID ");
            //set值
            ht.Add("@APPROVE_DT", now);
            ht.Add("@APPROVE_STATUS", "Y");//B:核可
            ht.Add("@APPROVE_BY", UPDATED_BY);
            ht.Add("@APPROVE_REMARK", APPROVE_REMARK);

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //新增 節金主檔
    public void insertFestivalH()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"declare @FESTIVAL_DT datetime 
                        = ( select SALARY_DT  from TB_S_M_REWARD_RETIRE where EMP_ID=@EMP_ID );
                        declare @FESTIVAL_TOTAL_AMT decimal(12,0) 
                        = ( select sum(FESTIVAL_AMT) from TB_S_M_FESTIVAL_D where  FESTIVAL_TYPE='4' and FESTIVAL_DT=@FESTIVAL_DT and emp_cd='1' and FESTIVAL_PAY_DT=@FESTIVAL_DT ) ;
                        declare @FESTIVAL_TOTAL_NUM decimal(7,0)  
                        = ( select count(*)          from TB_S_M_FESTIVAL_D where  FESTIVAL_TYPE='4' and FESTIVAL_DT=@FESTIVAL_DT and emp_cd='1' and FESTIVAL_PAY_DT=@FESTIVAL_DT ); 
                        delete from TB_S_M_FESTIVAL_H where FESTIVAL_TYPE='4' and FESTIVAL_DT=@FESTIVAL_DT and emp_cd='1' and FESTIVAL_PAY_DT=@FESTIVAL_DT
                        ");

            sb.Append(@" INSERT INTO TB_S_M_FESTIVAL_H
                       (FESTIVAL_TYPE,FESTIVAL_DT,EMP_CD,FESTIVAL_PAY_DT,FESTIVAL_DESC
                        ,FESTIVAL_TOTAL_AMT,FESTIVAL_TOTAL_NUM,APPROVE_STATUS,REMARK,TARGET_GEN_DT
                        ,RELEASE_DT,RELEASE_BY,APPROVE_DT,APPROVE_BY,SALARY_TRANS_DT
                        ,SALARY_TRANS_BY,PROCESS_STATUS,SALARY_DT,FREEZE_FLAG
                        ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)
                         select '4',A.SALARY_DT,H.EMP_CD, A.SALARY_DT,'優退金'
                        ,@FESTIVAL_TOTAL_AMT,@FESTIVAL_TOTAL_NUM,'Y','',A.SALARY_DT
                        ,GETDATE(),A.UPDATED_BY,A.APPROVE_DT,A.APPROVE_BY,GETDATE()
                        ,A.APPROVE_BY,'N',null,'Y'
                        ,A.APPROVE_BY,a.APPROVE_DT,A.APPROVE_BY,A.APPROVE_DT,@FUNC_ID
                        from TB_S_M_REWARD_RETIRE A
                        left join TB_H_M_EMP H on A.EMP_ID = H.EMP_ID
                        where A.EMP_ID=@EMP_ID 
                        ");

            //新修日期
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增 節金明細檔
    public void insertFestivalD(string tableName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" INSERT INTO "+tableName);
            sb.Append(@"( FESTIVAL_TYPE,FESTIVAL_DT,FESTIVAL_PAY_DT,EMP_ID,EMP_NAME
			            ,DEPT_NO,PLANT_CD,JPN_CD,COMPANY_CD,LEVEL_CD
			            ,GRADE_CD,PJOB_CD,JOIN_DT,WORK_YEARS,WORK_DAYS
			            ,EMP_CD,EMP_CHG_CD,WS_CD,SEX_CD,LEVEL_PAY
			            ,ABILITY_PAY,PJOB_PAY,PROFESSION_PAY,FOOD_SUBSIDY,FESTIVAL_AMT
			            ,FESTIVAL_AMT_OLD,FESTIVAL_TAX,FESTIVAL_AMT_R,PAY_TYPE,PAY_TYPE_OLD
			            ,APPROVE_FLAG,CHG_STATUS,APPROVE_MARK
			            ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)
                        select '4',A.SALARY_DT,A.SALARY_DT,A.EMP_ID,H.EMP_NAME
                        ,H.DEPT_NO,H.PLANT_CD,H.JPN_CD,H.COMPANY_CD,H.LEVEL_CD
                        ,H.GRADE_CD,H.PJOB_CD,H.JOIN_DT,0,0
                        ,H.EMP_CD,H.EMP_CHG_CD,H.WS_CD,H.SEX_CD,0
                        ,0,0,0,0,A.REWARD_PAY
                        ,0,0,A.REWARD_PAY,'Y',''
                        ,'Y','G',''
                        ,A.APPROVE_BY,a.APPROVE_DT,A.APPROVE_BY,A.APPROVE_DT,@FUNC_ID
                        from TB_S_M_REWARD_RETIRE A
                        left join VW_H_EMP_DATA H on A.EMP_ID = H.EMP_ID
                        where A.EMP_ID=@EMP_ID  
                    ");

            //新修日期
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    #region Qry Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
            , string approve_status, string emp_id
    )
    {
        try
        {

            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" a.*   ");
            sb.Append(" ,b.EMP_NAME	 EMP_NAME  ");
            sb.Append(" ,isnull(C.EMP_NAME,'')  APPROVE_NAME  ");
            sb.Append(" from TB_S_M_REWARD_RETIRE a ");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID ");
            sb.Append(" left join TB_H_M_EMP c on a.APPROVE_BY = c.EMP_ID ");
            sb.Append(" where 1=1 ");

            /*
            //測試時,註解掉
            sb.Append(@" AND a.CREATED_BY IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
            ht.Add("@loginID", SessionHandle.Current.emp_id);
            ht.Add("@departments", SessionHandle.Current.departments);
            */

            //查詢條件
            if (approve_status != "-1")
            {
                sb.Append(" and a.APPROVE_STATUS = @APPROVE_STATUS ");
                ht.Add("@APPROVE_STATUS", approve_status);
            }

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
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
            , string approve_status, string emp_id)
    {
        try
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_REWARD_RETIRE  a");
            sb.Append(" where 1=1 ");
            /*
            //測試時,註解掉
            sb.Append(@" AND a.CREATED_BY IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
            ht.Add("@loginID", SessionHandle.Current.emp_id);
            ht.Add("@departments", SessionHandle.Current.departments);
            */
            //查詢條件
            if (approve_status != "-1")
            {
                sb.Append(" and a.APPROVE_STATUS = @APPROVE_STATUS ");
                ht.Add("@APPROVE_STATUS", approve_status);
            }

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }



            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = (int)dt.Rows[0]["resultCount"];
            }
            return result;
        }
        catch (Exception)
        {

            throw;
        }

    }

    #endregion





}