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
/// CFB2SP1100DAO 的摘要描述
/// </summary>
public class CFB2SP1100DAO : BaseDAO
{
    //s1030基本欄位
    public string EMP_ID { get; set; }
    public string SALARY_SYM { get; set; }
    public string SALARY_EYM { get; set; }
    public string SALARY_DT { get; set; }
    public string PAY_BASIC { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    //ACES
    bool isSuper { get; set; }  //是否為supervisor(擔當)

    ACESLib.ACES aces;
    public CFB2SP1100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //檢查 該發放日期 是否 發薪狀態是否為Y	
    public int checkIsSalary()
    {
        try
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" 
                        select count(*) resultCount from  TB_S_M_FESTIVAL_H
                        where  FESTIVAL_TYPE='4'
                        and FESTIVAL_DT=@SALARY_DT
                        and FESTIVAL_PAY_DT	=@SALARY_DT
                        and EMP_CD='1'
                        and PROCESS_STATUS='Y'
                      ");
            ht.Add("@SALARY_DT", SALARY_DT);
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

    //優退金 明細檔
    public DataTable getDetail()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"  select 
                         B.EMP_NAME 
                         ,A.* from TB_S_M_REWARD_RETIRE A
                        left join TB_H_M_EMP B on B.EMP_ID = A.EMP_ID
                        where A.EMP_ID = @EMP_ID 
                     ");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //刪除 優退金檔
    public void deleteData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_REWARD_RETIRE ");
            sb.Append(" where EMP_ID = @EMP_ID  ");
            ht.Add("@EMP_ID", emp_id);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //計算
    public void execute()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();            
            sb.Append(@"
                        declare @avg_month decimal(3,0)=0;
                        declare @total_amt decimal(8,0)=0;

                        delete from TB_S_M_REWARD_RETIRE  where EMP_ID=@EMP_ID;
                        select @avg_month = convert(decimal,CODE_VAL1) from TB_9_M_PARAMETER where SYS_CD='SP' and  MAIN_CD='REWARD_RETIRE_AVG_MONTH'
                        
                        select @total_amt = SUM(Round((B.AMOUNT*B.calendar_days)/nullif(B.WORK_DAYS_MONTH,0),0)) from (
                        select A.DATA_YM,A.AMOUNT,A.WORK_DAYS_MONTH,(SELECT  Right(Convert(varchar,dateadd(ms,-3,DATEADD(mm, DATEDIFF(m,0,A.DATA_YM+'01')+1, 0)) ,112),2)) calendar_days from (
	                        select a.*,b.WORK_DAYS_MONTH  from  TB_S_M_EMP_RESULT b
	                        left join TB_S_M_SALARY_PAY a on a.DATA_YM = b.SALARY_YM and a.SALARY_DT = b.SALARY_DT and a.EMP_ID = b.EMP_ID																										
									                        where salary_type='A' 
									                        and a.emp_id=@EMP_ID	
									                        and data_ym<=REPLACE(@SALARY_EYM,'/','') and data_ym>=REPLACE(@SALARY_SYM,'/','')							
									                        and SALARY_ID in ('1001','1002','1003','1004','1012','1053') 
									                        and DEL_MARK='N' and PAY_TYPE='Y'
	                        )A
                        )B
                        --select @total_amt = isnull(sum(amount),0)  from TB_S_M_SALARY_PAY 																											
		                --        where salary_type='A' 
		                --        and emp_id=@EMP_ID	
		                --        and data_ym<=REPLACE(@SALARY_EYM,'/','') and data_ym>=REPLACE(@SALARY_SYM,'/','')							
		                --        and SALARY_ID in ('1001','1002','1003','1004','1012','1053') 
		                --        and DEL_MARK='N' and PAY_TYPE='Y'																									
                        print round(@total_amt/@avg_month,0);
                        print CEILING( round(@total_amt/@avg_month,0)*@PAY_BASIC/100)*100

                        INSERT INTO TB_S_M_REWARD_RETIRE
                                    (EMP_ID,SALARY_SYM,SALARY_EYM,SALARY_DT,PAY_TOTAL
			                        ,PAY_AVG,PAY_BASIC,REWARD_PAY,APPROVE_DT,APPROVE_BY
			                        ,APPROVE_STATUS,APPROVE_REMARK
			                        ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)
                                VALUES
                                    (@EMP_ID
                                    ,REPLACE(@SALARY_SYM,'/','')
                                    ,REPLACE(@SALARY_EYM,'/','')
                                    ,@SALARY_DT
                                    ,@total_amt
                                    ,convert(decimal(7,0), round(@total_amt/@avg_month,0) )
                                    ,@PAY_BASIC
                                    ,CEILING( round(@total_amt/@avg_month,0)*@PAY_BASIC/100)*100
                                    ,NULL
                                    ,''
                                    ,'N'
                                    ,''
                                    ,@CREATED_BY
                                    ,GETDATE()
                                    ,@CREATED_BY
                                    ,GETDATE()
                                    ,@FUNC_ID
		                     )
                    ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_SYM", SALARY_SYM);
            ht.Add("@SALARY_EYM", SALARY_EYM);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@PAY_BASIC", PAY_BASIC);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //計算 檢核1
    public DataTable checkExecute1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select 
                       (select count(*)  from TB_S_M_REWARD_RETIRE where EMP_ID=@EMP_ID
                        )  resultCount
                        ,
                      isnull( (select APPROVE_BY from TB_S_M_REWARD_RETIRE where EMP_ID=@EMP_ID
                      ),'')  APPROVE_BY
                    ");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //計算 檢核2
    public DataTable checkExecute2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" IF CONVERT(VARCHAR(6),CONVERT(DATETIME,@SALARY_DT),112) >isnull(dbo.FN_S_SALARY_YM(),'999999')
                        BEGIN
	                        SELECT 1 resultCount,dbo.FN_S_SALARY_YM() SALARY_YM
                        END
                        ELSE
                        BEGIN
	                        SELECT 0 resultCount	,dbo.FN_S_SALARY_YM()  SALARY_YM
                        END
                    ");
            ht.Add("@SALARY_DT", SALARY_DT);
            return dbConn.Query(sb, ht);

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



    #region Dtl Gridview 資料
    //Gridview 查詢資料
    public DataTable getDtlData(int startRowIndex, int maximumRows, string sortExpression
                                , string emp_id
                           )
    {
        try
        {

            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "t2.EMP_ID");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"declare  @SALARY_SYM varchar(6);
                        declare  @SALARY_EYM varchar(6);
                        select 	@SALARY_SYM=SALARY_SYM,@SALARY_EYM = SALARY_EYM from 	TB_S_M_REWARD_RETIRE where EMP_ID=@EMP_ID ");
            sb.Append(" Select * From ( ");
            sb.Append(@"
                        select  ROW_NUMBER() OVER(ORDER BY t2.DATA_YM) As RowNumber, t2.* ,(t2.s1001+t2.s1002+t2.s1003+t2.s1004+t2.s1012+t2.s1053) as TOTAL_AMT from (														
	                        select t1.DATA_YM,EMP_ID,SUM(Round((t1.s1001*t1.calendar_days)/nullif(t1.WORK_DAYS_MONTH,0),0)) as s1001, 
							SUM(Round((t1.s1002*t1.calendar_days)/nullif(t1.WORK_DAYS_MONTH,0),0)) as s1002, SUM(Round((t1.s1003*t1.calendar_days)/nullif(t1.WORK_DAYS_MONTH,0),0)) as s1003 , 
							SUM(Round((t1.s1004*t1.calendar_days)/nullif(t1.WORK_DAYS_MONTH,0),0)) as s1004, SUM(Round((t1.s1012*t1.calendar_days)/nullif(t1.WORK_DAYS_MONTH,0),0)) as s1012, 
							SUM(Round((t1.s1053*t1.calendar_days)/nullif(t1.WORK_DAYS_MONTH,0),0)) as s1053 from (	
								select DATA_YM,EMP_ID,s1001,s1002,s1003,s1004,s1012,s1053,WORK_DAYS_MONTH,(SELECT  Right(Convert(varchar,dateadd(ms,-3,DATEADD(mm, DATEDIFF(m,0,DATA_YM+'01')+1, 0)) ,112),2)) calendar_days from (																					
									--職能俸*/ 																																
									select DATA_YM,b.EMP_ID,b.WORK_DAYS_MONTH,amount as s1001 ,0 as s1002,0 as s1003,0 as s1004,0 as s1012,0 as s1053 from TB_S_M_EMP_RESULT b
									left join TB_S_M_SALARY_PAY a on a.DATA_YM = b.SALARY_YM and a.SALARY_DT = b.SALARY_DT and a.EMP_ID = b.EMP_ID 					
									where salary_type='A' and DATA_YM<=@SALARY_EYM and DATA_YM>= @SALARY_SYM and SALARY_ID='1001' and DEL_MARK='N' and PAY_TYPE='Y'			
									union all --資格俸*/																													
									select DATA_YM,b.EMP_ID,b.WORK_DAYS_MONTH,0 as s1001 ,amount as s1002,0 as s1003,0 as s1004,0 as s1012,0 as s1053 from TB_S_M_EMP_RESULT b
									left join TB_S_M_SALARY_PAY a on a.DATA_YM = b.SALARY_YM and a.SALARY_DT = b.SALARY_DT and a.EMP_ID = b.EMP_ID			
									where salary_type='A' and DATA_YM<=@SALARY_EYM and DATA_YM>= @SALARY_SYM and SALARY_ID='1002' and DEL_MARK='N' and PAY_TYPE='Y'			
									union all --職務俸*/																													
									select DATA_YM,b.EMP_ID,b.WORK_DAYS_MONTH,0 as s1001 ,0 as s1002,amount as s1003,0 as s1004,0 as s1012,0 as s1053 from TB_S_M_EMP_RESULT b
									left join TB_S_M_SALARY_PAY a on a.DATA_YM = b.SALARY_YM and a.SALARY_DT = b.SALARY_DT and a.EMP_ID = b.EMP_ID 					
									where salary_type='A' and DATA_YM<=@SALARY_EYM and DATA_YM>= @SALARY_SYM and SALARY_ID='1003' and DEL_MARK='N' and PAY_TYPE='Y'			
									union all --專業俸*/																													
									select DATA_YM,b.EMP_ID,b.WORK_DAYS_MONTH,0 as s1001 ,0 as s1002,0 as s1003,amount as s1004,0 as s1012,0 as s1053 from TB_S_M_EMP_RESULT b 
									left join TB_S_M_SALARY_PAY a on a.DATA_YM = b.SALARY_YM and a.SALARY_DT = b.SALARY_DT and a.EMP_ID = b.EMP_ID					
									where salary_type='A' and DATA_YM<=@SALARY_EYM and DATA_YM>= @SALARY_SYM and SALARY_ID='1004' and DEL_MARK='N' and PAY_TYPE='Y'			
									union all --伙食津貼*/																													
									select DATA_YM,b.EMP_ID,b.WORK_DAYS_MONTH,0 as s1001 ,0 as s1002,0 as s1003,0 as s1004,amount as s1012,0 as s1053 from TB_S_M_EMP_RESULT b 
									left join TB_S_M_SALARY_PAY a on a.DATA_YM = b.SALARY_YM and a.SALARY_DT = b.SALARY_DT and a.EMP_ID = b.EMP_ID					
									where salary_type='A' and DATA_YM<=@SALARY_EYM and DATA_YM>= @SALARY_SYM and SALARY_ID='1012' and DEL_MARK='N' and PAY_TYPE='Y'			
									union all --調整津貼*/																													
									select DATA_YM,b.EMP_ID,b.WORK_DAYS_MONTH,0 as s1001 ,0 as s1002,0 as s1003,0 as s1004,0 as s1012,amount as s1053 from TB_S_M_EMP_RESULT b
									left join TB_S_M_SALARY_PAY a on a.DATA_YM = b.SALARY_YM and a.SALARY_DT = b.SALARY_DT and a.EMP_ID = b.EMP_ID  					
									where salary_type='A' and DATA_YM<=@SALARY_EYM and DATA_YM>= @SALARY_SYM and SALARY_ID='1053' and DEL_MARK='N' and PAY_TYPE='Y'
								) n				
                        ) t1 	 where t1.EMP_ID=@EMP_ID	 group by   t1.DATA_YM,EMP_ID
                        ) t2
                        group by  t2.DATA_YM,EMP_ID, t2.s1001,t2.s1002,t2.s1003,t2.s1004,t2.s1012,t2.s1053		
                       ");
            ht.Add("@EMP_ID", emp_id );

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
    public int getDtlCount(int startRowIndex, int maximumRows
            , string emp_id)
    {
        try
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"declare  @SALARY_SYM varchar(6);
                        declare  @SALARY_EYM varchar(6);
                        select 	@SALARY_SYM=SALARY_SYM,@SALARY_EYM = SALARY_EYM from 	TB_S_M_REWARD_RETIRE where EMP_ID=@EMP_ID ");
            sb.Append(" Select COUNT(*) resultCount  from  ( ");
            sb.Append(@"
                        select  ROW_NUMBER() OVER(ORDER BY t2.DATA_YM) As RowNumber, t2.* ,(t2.s1001+t2.s1002+t2.s1003+t2.s1004+t2.s1012+t2.s1053) as TOTAL_AMT from (														
	                        select t1.DATA_YM,EMP_ID,SUM(Round((t1.s1001*t1.calendar_days)/nullif(t1.WORK_DAYS_MONTH,0),0)) as s1001, 
							SUM(Round((t1.s1002*t1.calendar_days)/nullif(t1.WORK_DAYS_MONTH,0),0)) as s1002, SUM(Round((t1.s1003*t1.calendar_days)/nullif(t1.WORK_DAYS_MONTH,0),0)) as s1003 , 
							SUM(Round((t1.s1004*t1.calendar_days)/nullif(t1.WORK_DAYS_MONTH,0),0)) as s1004, SUM(Round((t1.s1012*t1.calendar_days)/nullif(t1.WORK_DAYS_MONTH,0),0)) as s1012, 
							SUM(Round((t1.s1053*t1.calendar_days)/nullif(t1.WORK_DAYS_MONTH,0),0)) as s1053 from (	
								select DATA_YM,EMP_ID,s1001,s1002,s1003,s1004,s1012,s1053,WORK_DAYS_MONTH,(SELECT  Right(Convert(varchar,dateadd(ms,-3,DATEADD(mm, DATEDIFF(m,0,DATA_YM+'01')+1, 0)) ,112),2)) calendar_days from (																					
									--職能俸*/ 																																
									select DATA_YM,b.EMP_ID,b.WORK_DAYS_MONTH,amount as s1001 ,0 as s1002,0 as s1003,0 as s1004,0 as s1012,0 as s1053 from TB_S_M_EMP_RESULT b
									left join TB_S_M_SALARY_PAY a on a.DATA_YM = b.SALARY_YM and a.SALARY_DT = b.SALARY_DT and a.EMP_ID = b.EMP_ID 					
									where salary_type='A' and DATA_YM<=@SALARY_EYM and DATA_YM>= @SALARY_SYM and SALARY_ID='1001' and DEL_MARK='N' and PAY_TYPE='Y'			
									union all --資格俸*/																													
									select DATA_YM,b.EMP_ID,b.WORK_DAYS_MONTH,0 as s1001 ,amount as s1002,0 as s1003,0 as s1004,0 as s1012,0 as s1053 from TB_S_M_EMP_RESULT b
									left join TB_S_M_SALARY_PAY a on a.DATA_YM = b.SALARY_YM and a.SALARY_DT = b.SALARY_DT and a.EMP_ID = b.EMP_ID			
									where salary_type='A' and DATA_YM<=@SALARY_EYM and DATA_YM>= @SALARY_SYM and SALARY_ID='1002' and DEL_MARK='N' and PAY_TYPE='Y'			
									union all --職務俸*/																													
									select DATA_YM,b.EMP_ID,b.WORK_DAYS_MONTH,0 as s1001 ,0 as s1002,amount as s1003,0 as s1004,0 as s1012,0 as s1053 from TB_S_M_EMP_RESULT b
									left join TB_S_M_SALARY_PAY a on a.DATA_YM = b.SALARY_YM and a.SALARY_DT = b.SALARY_DT and a.EMP_ID = b.EMP_ID 					
									where salary_type='A' and DATA_YM<=@SALARY_EYM and DATA_YM>= @SALARY_SYM and SALARY_ID='1003' and DEL_MARK='N' and PAY_TYPE='Y'			
									union all --專業俸*/																													
									select DATA_YM,b.EMP_ID,b.WORK_DAYS_MONTH,0 as s1001 ,0 as s1002,0 as s1003,amount as s1004,0 as s1012,0 as s1053 from TB_S_M_EMP_RESULT b 
									left join TB_S_M_SALARY_PAY a on a.DATA_YM = b.SALARY_YM and a.SALARY_DT = b.SALARY_DT and a.EMP_ID = b.EMP_ID					
									where salary_type='A' and DATA_YM<=@SALARY_EYM and DATA_YM>= @SALARY_SYM and SALARY_ID='1004' and DEL_MARK='N' and PAY_TYPE='Y'			
									union all --伙食津貼*/																													
									select DATA_YM,b.EMP_ID,b.WORK_DAYS_MONTH,0 as s1001 ,0 as s1002,0 as s1003,0 as s1004,amount as s1012,0 as s1053 from TB_S_M_EMP_RESULT b 
									left join TB_S_M_SALARY_PAY a on a.DATA_YM = b.SALARY_YM and a.SALARY_DT = b.SALARY_DT and a.EMP_ID = b.EMP_ID					
									where salary_type='A' and DATA_YM<=@SALARY_EYM and DATA_YM>= @SALARY_SYM and SALARY_ID='1012' and DEL_MARK='N' and PAY_TYPE='Y'			
									union all --調整津貼*/																													
									select DATA_YM,b.EMP_ID,b.WORK_DAYS_MONTH,0 as s1001 ,0 as s1002,0 as s1003,0 as s1004,0 as s1012,amount as s1053 from TB_S_M_EMP_RESULT b
									left join TB_S_M_SALARY_PAY a on a.DATA_YM = b.SALARY_YM and a.SALARY_DT = b.SALARY_DT and a.EMP_ID = b.EMP_ID  					
									where salary_type='A' and DATA_YM<=@SALARY_EYM and DATA_YM>= @SALARY_SYM and SALARY_ID='1053' and DEL_MARK='N' and PAY_TYPE='Y'
								) n			
                        ) t1 	 where t1.EMP_ID=@EMP_ID	 group by   t1.DATA_YM,EMP_ID
                        ) t2
                        group by  t2.DATA_YM,EMP_ID, t2.s1001,t2.s1002,t2.s1003,t2.s1004,t2.s1012,t2.s1053		
                       ");
            sb.Append(" ) z");

            //查詢條件
            ht.Add("@EMP_ID", emp_id);

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