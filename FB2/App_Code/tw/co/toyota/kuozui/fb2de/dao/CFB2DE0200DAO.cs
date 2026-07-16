using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// WFB2DE0200DAO 的摘要描述
/// </summary>
public class CFB2DE0200DAO : BaseDAO
{

    public string SDT { get; set; }
    public string EDT { get; set; }    
    public string COMPANY_CD { get; set; }
    public string EMP_ID { get; set; }
    public string CALENDAR_DT { get; set; }
    public string WORK_DAY_CD { get; set; }
    public string WS_CD { get; set; }
    public string LEVEL_CD { get; set; }
    public string DEPT_NO { get; set; }
    public string ACC_DEPT_NO { get; set; }
    public string COST_DEPT_NO { get; set; }
    public string AddOne { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    public string IS_DUTY_CHECK { get; set; }
    public string CLOCK_OUT_DT { get; set; }
    public string CLOCK_IN_DT { get; set; }
    public string ct { get; set; }
    
    //餐廳參數設定檔
    public string LAST_BR_TIME { get; set; }
    public string BR_END { get; set; }
    public string LAST_DN_TIME { get; set; }
    public string DN_START { get; set; }
    public string BR_START { get; set; }
    public string DN_END { get; set; }
    public string COURSE_DN_TIME { get; set; }
    public string BF_AMOUNT { get; set; }
    public string DN_AMOUNT { get; set; }
      
    //用餐實績檔
    public string MANAGER_DT_ACT { get; set; }
    public string EMP_ID_ACT { get; set; }
    public string CLOCK_NO_ACT { get; set; }
    public string RESTAURANT_CD_ACT { get; set; }
    public string DEPT_NO_ACT { get; set; }
    public string WORK_SHIFT_NO_ACT { get; set; }
    public string MEAL_TIMES_ACT { get; set; }
    public string CARD_NO_ACT { get; set; }
    public string WS_CD_ACT { get; set; }
    public string MEALSHIFT { get; set; }
    public string RESTAURANT_ERROR_CD { get; set; }
    public string CLOCK_IN_DT_ACT { get; set; }
    public string CLOCK_OUT_DT_ACT { get; set; }
    public string PLANT_CD { get; set; }
    public string COMPANY_CD_ACT { get; set; }
    public string COURSE_NAME { get; set; }
    public string COURSE_DT { get; set; }
    public string LEVEL_CD_ACT { get; set; }
    public string EXPENSE_CD { get; set; }
    public string MEAL_AMOUNT { get; set; }

    public CFB2DE0200DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string getMaxDay()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CONVERT(char(10),MAX(MANAGER_DT), 120) MANAGER_DT from TB_D_R_RES_ACTURL ");

            DataTable dt = dbConn.Query(sb, ht);

            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["MANAGER_DT"].ToString();
            }           

            return st;
        }
        catch
        {
            throw;
        }
    }

    public string getCal(string START_DT, string END_DT)
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MANAGER_DT from TB_D_R_RES_ACTURL");
            sb.Append(" where MANAGER_DT between @SDT and @EDT");
            sb.Append(" and MONTH_CLOSE_DT <> '9999/12/31'");
            
            ht.Add("@SDT", START_DT);
            ht.Add("@EDT", END_DT);

            DataTable dt = dbConn.Query(sb, ht);

            if (dt.Rows.Count > 0)
            {
                st = "Y";
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public void DeleteTemp_ACTURL()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_D_R_RES_ACTURL_TEMP");
                      

            dbConn.Execute(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }

    public void DeleteTemp_DAY()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_D_R_RES_DAY_ATTEND_TEMP");


            dbConn.Execute(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }

    public void DeleteOld()
    {        
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_D_R_RES_ACTURL");
            sb.Append(" where MANAGER_DT between @SDT and @EDT");


            ht.Add("@SDT", SDT);
            ht.Add("@EDT", EDT);

            dbConn.Execute(sb, ht, true);

           
        }
        catch
        {
            throw;
        }
    }


    public void DeleteDAY()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_D_R_RES_DAY_ATTEND");
            sb.Append(" where MANAGER_DT between @SDT and @EDT");


            ht.Add("@SDT", SDT);
            ht.Add("@EDT", EDT);

            dbConn.Execute(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }

    public void getLast_BR_Time()
    {        
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select LAST_BR_TIME,BR_END,LAST_DN_TIME,DN_START,BR_START,DN_END,COURSE_DN_TIME,BF_AMOUNT,DN_AMOUNT");
            sb.Append(" from TB_D_M_RES_PARA");
            sb.Append(" where COMPANY_CD = @COMPANY_CD");

            ht.Add("@COMPANY_CD", COMPANY_CD);
          

            DataTable dt = dbConn.Query(sb, ht);

            if (dt.Rows.Count > 0)
            {
                LAST_BR_TIME = dt.Rows[0]["LAST_BR_TIME"].ToString();
                BR_END = dt.Rows[0]["BR_END"].ToString();
                LAST_DN_TIME = dt.Rows[0]["LAST_DN_TIME"].ToString();
                DN_START = dt.Rows[0]["DN_START"].ToString();
                BR_START = dt.Rows[0]["BR_START"].ToString();
                DN_END = dt.Rows[0]["DN_END"].ToString();
                COURSE_DN_TIME = dt.Rows[0]["COURSE_DN_TIME"].ToString();
                BF_AMOUNT = dt.Rows[0]["BF_AMOUNT"].ToString();
                DN_AMOUNT = dt.Rows[0]["DN_AMOUNT"].ToString();
            }
            
            
        }
        catch
        {
            throw;
        }
    }

    //取得COMPANY_CD
    public string getCOMPANY_CD(string emp_id)
    {
        try
        {
            string COMPANY_CD = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string sql = " Select * from TB_H_M_EMP where EMP_ID=@EMP_ID";
            sb.Append(sql);
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                COMPANY_CD = dt.Rows[0]["COMPANY_CD"].ToString();
            }
            return COMPANY_CD;
        }
        catch
        {
            throw;
        }
    }

    public void insert_ACTURL_TEMP()
    {        
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
           
            sb.Append("insert into TB_D_R_RES_ACTURL_TEMP");
            sb.Append(" (MANAGER_DT,MANAGER_UNIT,EMP_ID,MEALSHIFT,DEPT_NO,WS_CD,LEVEL_CD,CLOCK_NO,CLOCK_PLANT_CD,RESTAURANT_CD,");
            sb.Append(" COMPANY_CD,WORK_SHIFT_NO,RESTAURANT_ERROR_CD,ST_COURSE_ID,COURSE_DATE,EXPENSE_CD,MEAL_AMOUNT,MEAL_TIMES,");
            sb.Append(" CARD_START,CARD_END,CARD_NO,MONTH_CLOSE_DT,MONTH_CLOSE_BY,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select MANAGER_DT,MANAGER_UNIT,EMP_ID,MEALSHIFT,DEPT_NO,WS_CD,LEVEL_CD,CLOCK_NO,CLOCK_PLANT_CD,RESTAURANT_CD,");
            sb.Append(" COMPANY_CD,WORK_SHIFT_NO,RESTAURANT_ERROR_CD,ST_COURSE_ID,COURSE_DATE,EXPENSE_CD,MEAL_AMOUNT,MEAL_TIMES,");
            sb.Append(" CARD_START,CARD_END,CARD_NO,MONTH_CLOSE_DT,MONTH_CLOSE_BY,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID");
            sb.Append(" from TB_D_R_RES_ACTURL");
            sb.Append(" where MANAGER_DT between @SDT and @EDT");


            ht.Add("@SDT", SDT);
            ht.Add("@EDT", EDT);
           

            dbConn.Execute(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }

    public void insert_DAY_TEMP()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            

            sb.Append("insert into TB_D_R_RES_DAY_ATTEND_TEMP");
            sb.Append(" (MANAGER_DT,MANAGER_UNIT,BR_PEOPLE,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select MANAGER_DT,MANAGER_UNIT,BR_PEOPLE,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID");
            sb.Append(" from TB_D_R_RES_DAY_ATTEND");
            sb.Append(" where MANAGER_DT between @SDT and @EDT");


            ht.Add("@SDT", SDT);
            ht.Add("@EDT", EDT);


            dbConn.Execute(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }

    public void insert_ACTURL_main()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("insert into TB_D_R_RES_ACTURL");
            sb.Append(" (MANAGER_DT,MANAGER_UNIT,EMP_ID,MEALSHIFT,DEPT_NO,WS_CD,LEVEL_CD,CLOCK_NO,CLOCK_PLANT_CD,RESTAURANT_CD,");
            sb.Append(" COMPANY_CD,WORK_SHIFT_NO,RESTAURANT_ERROR_CD,ST_COURSE_ID,COURSE_DATE,EXPENSE_CD,MEAL_AMOUNT,MEAL_TIMES,");
            sb.Append(" CARD_START,CARD_END,CARD_NO,MONTH_CLOSE_DT,MONTH_CLOSE_BY,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select MANAGER_DT,MANAGER_UNIT,EMP_ID,MEALSHIFT,DEPT_NO,WS_CD,LEVEL_CD,CLOCK_NO,CLOCK_PLANT_CD,RESTAURANT_CD,");
            sb.Append(" COMPANY_CD,WORK_SHIFT_NO,RESTAURANT_ERROR_CD,ST_COURSE_ID,COURSE_DATE,EXPENSE_CD,MEAL_AMOUNT,MEAL_TIMES,");
            sb.Append(" CARD_START,CARD_END,CARD_NO,MONTH_CLOSE_DT,MONTH_CLOSE_BY,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID");
            sb.Append(" from TB_D_R_RES_ACTURL_TEMP");
            sb.Append(" where MANAGER_DT between @SDT and @EDT");


            ht.Add("@SDT", SDT);
            ht.Add("@EDT", EDT);


            dbConn.Execute(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }

    public void insert_DAY_main()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append("insert into TB_D_R_RES_DAY_ATTEND");
            sb.Append(" (MANAGER_DT,MANAGER_UNIT,BR_PEOPLE,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select MANAGER_DT,MANAGER_UNIT,BR_PEOPLE,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID");
            sb.Append(" from TB_D_R_RES_DAY_ATTEND_TEMP");
            sb.Append(" where MANAGER_DT between @SDT and @EDT");


            ht.Add("@SDT", SDT);
            ht.Add("@EDT", EDT);


            dbConn.Execute(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }


    public void insert_Emp_Duty()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append("insert into TB_D_R_RES_DAY_ATTEND");
            sb.Append(" values (@CALENDAR_DT,@MANAGER_UNIT,@BR_PEOPLE,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID)");
            //sb.Append(" select CALENDAR_DT,@MANAGER_UNIT,@ct,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID");
            //sb.Append(" from TB_D_M_EMP_DUTY_CHECK_STATUS");
            //sb.Append(" where CALENDAR_DT = @CALENDAR_DT");
            //sb.Append(" and EMP_ID = @EMP_ID");            

            //ht.Add("@AddOne", AddOne);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@BR_PEOPLE", ct);
            ht.Add("@MANAGER_UNIT", COST_DEPT_NO);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            //ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);

            
        }
        catch
        {
            throw;
        }
    }


    public void insert_Emp_Duty_Temp()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append("insert into TB_D_R_RES_DAY_ATTEND_TEMP");
            sb.Append(" (MANAGER_DT,MANAGER_UNIT,BR_PEOPLE,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select CALENDAR_DT,@MANAGER_UNIT,'1',@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID");
            sb.Append(" from TB_D_M_EMP_DUTY_CHECK_STATUS");
            sb.Append(" where CALENDAR_DT = @CALENDAR_DT");
            sb.Append(" and EMP_ID = @EMP_ID");

            ht.Add("@AddOne", AddOne);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@COST_DEPT_NO", COST_DEPT_NO);
            ht.Add("@MANAGER_UNIT", COST_DEPT_NO);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.Execute(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }

    public void copyEduTable()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" 
                       IF EXISTS(
	                        select * from sys.tables where name = 'tmpEDU'	
                       )
                       drop table tmpEDU;                             
            ");  
            sb.Append(" select *  into tmpEDU from " + utilities.ORACLEName + "..[TFB1U001].[VW_FLOW_COURSE]");
            sb.Append(" where COURSE_DT between @EDT and @SDT");           

            
            ht.Add("@SDT", SDT);
            ht.Add("@EDT", EDT);

           dbConn.Query(sb, ht);

           
        }
        catch
        {
            throw;
        }
    }

    public void dropEduTable()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" drop table tmpEDU");        

            dbConn.Query(sb, ht);


        }
        catch
        {
            throw;
        }
    }

    public DataTable getBF_Member()
    {        
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select CALENDAR_DT,COST_DEPT_NO,count(EMP_ID) ct from (");
            sb.Append(" Select a.EMP_ID,CONVERT(char(10),a.CALENDAR_DT, 120)CALENDAR_DT,a.WORK_DAY_CD,b.WS_CD,b.LEVEL_CD,b.DEPT_NO,CLOCK_IN_DT,DUTY_CHECK_RESULT,");
            sb.Append(" c.ACC_DEPT_NO,d.COST_DEPT_NO,(select CASE WHEN c.END_DT IS NULL THEN '9999/12/31' ELSE c.END_DT END) NEWEND");
            sb.Append(" from TB_D_M_EMP_DUTY_CHECK_STATUS a");
            sb.Append(" left join TB_H_M_EMP b");
            sb.Append(" on a.EMP_ID = b.EMP_ID");
            sb.Append(" left join TB_H_M_DEPT c");
            sb.Append(" on b.DEPT_NO = c.DEPT_NO");
            sb.Append(" and c.START_DT <= getdate()");
            sb.Append(" left join TB_H_M_DEPT_ACC d");
            sb.Append(" on c.ACC_DEPT_NO = d.ACC_DEPT_NO");
            sb.Append(" and d.IS_VALID = 'Y'");
            sb.Append(" ) a");
            sb.Append(" where a.CALENDAR_DT between @SDT and @EDT");
            sb.Append(" and DUTY_CHECK_RESULT = 'Y'");
            sb.Append(" and convert(int,CONVERT(varchar(2),DatePart(hour, a.CLOCK_IN_DT), 101)+CONVERT(varchar(2),DatePart(minute, a.CLOCK_IN_DT), 101))  <= convert(int, @LAST_BR_TIME)");
            sb.Append(" and NEWEND > getdate()  group by CALENDAR_DT,COST_DEPT_NO order by CALENDAR_DT,COST_DEPT_NO");
            
            ht.Add("@LAST_BR_TIME", LAST_BR_TIME);
            ht.Add("@SDT", SDT);
            ht.Add("@EDT", EDT);

            DataTable dt = dbConn.Query(sb, ht);

            return dt;
           
        }
        catch
        {
            throw;
        }
    }

    public void update_RES_DAY()    {
        
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_D_R_RES_DAY_ATTEND");
            sb.Append(" set BR_PEOPLE = @AddOne,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,");
            sb.Append(" UPDATED_DT = getdate()");
            
            sb.Append(" where MANAGER_DT = @CALENDAR_DT");
            sb.Append(" and MANAGER_UNIT = @COST_DEPT_NO");

            ht.Add("@AddOne", AddOne);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@COST_DEPT_NO", COST_DEPT_NO);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
           
            
        }
        catch
        {
            throw;
        }
    }

    public void update_RES_DAY_Temp()
    {

        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_D_R_RES_DAY_ATTEND_TEMP");
            sb.Append(" set BR_PEOPLE = @AddOne,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,");
            sb.Append(" UPDATED_DT = getdate()");

            sb.Append(" where MANAGER_DT = @CALENDAR_DT");
            sb.Append(" and MANAGER_UNIT = @COST_DEPT_NO");

            ht.Add("@AddOne", AddOne);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@COST_DEPT_NO", COST_DEPT_NO);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.Execute(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }

    public string select_RES_DAY()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select BR_PEOPLE from TB_D_R_RES_DAY_ATTEND_TEMP");
            sb.Append(" where MANAGER_DT = @CALENDAR_DT ");
            sb.Append(" and MANAGER_UNIT = @COST_DEPT_NO");

            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@COST_DEPT_NO", COST_DEPT_NO);


            DataTable dt = dbConn.Query(sb, ht);

            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["BR_PEOPLE"].ToString();
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public string select_RES_DAY_A()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select BR_PEOPLE from TB_D_R_RES_DAY_ATTEND");
            sb.Append(" where MANAGER_DT = @CALENDAR_DT ");
            sb.Append(" and MANAGER_UNIT = @COST_DEPT_NO");

            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@COST_DEPT_NO", COST_DEPT_NO);


            DataTable dt = dbConn.QueryT(sb, ht);

            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["BR_PEOPLE"].ToString();
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public DataTable get_DAY_RECORD_No_T()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //sb.Append(" Select * from (");
            sb.Append(" Select MANAGER_DT,EMP_ID,CLOCK_NO,RESTAURANT_CD,DEPT_NO,WORK_SHIFT_NO,CARD_NO,WS_CD");
            sb.Append(" from TB_D_M_RES_DAY_RECORD");
            if (SDT == EDT)
            {
                sb.Append(" where CAST(MANAGER_DT as DATE) = @SDT ");
                SDT = SDT.Replace("-", "");
            }
            else
            {
                sb.Append(" where CAST(MANAGER_DT as DATE) between @SDT and @EDT");
                ht.Add("@EDT", EDT);
            }


            ht.Add("@SDT", SDT);



            DataTable dt = dbConn.Query(sb, ht);

            return dt;

        }
        catch
        {
            throw;
        }
    }

    public DataTable get_RES_MIDDLE_RECORD()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //sb.Append(" Select * from (");
            sb.Append(" Select MANAGER_DT,EMP_ID,CLOCK_NO,RESTAURANT_CD,DEPT_NO,WORK_SHIFT_NO,CARD_NO,WS_CD");
            sb.Append(" from TB_D_M_RES_MIDDLE_RECORD");
            if (SDT == EDT)
            {
                sb.Append(" where CAST(MANAGER_DT as DATE) = @SDT ");
                SDT = SDT.Replace("-", "");
            }
            else
            {
                sb.Append(" where CAST(MANAGER_DT as DATE) between @SDT and @EDT");
                ht.Add("@EDT", EDT);
            }


            ht.Add("@SDT", SDT);



            DataTable dt = dbConn.Query(sb, ht);

            return dt;

        }
        catch
        {
            throw;
        }
    }

    public DataTable get_DAY_RECORD()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //sb.Append(" Select * from (");
            sb.Append(" Select MANAGER_DT,EMP_ID,CLOCK_NO,RESTAURANT_CD,DEPT_NO,WORK_SHIFT_NO,CARD_NO,WS_CD");
            sb.Append(" from TB_D_M_RES_DAY_RECORD");
            if (SDT == EDT)
            {
                sb.Append(" where CAST(MANAGER_DT as DATE) = @SDT ");
                SDT = SDT.Replace("-","");
            }
            else
            {
                sb.Append(" where CAST(MANAGER_DT as DATE) between @SDT and @EDT");
                ht.Add("@EDT", EDT);
            }
            

            ht.Add("@SDT", SDT);
            
           

            DataTable dt = dbConn.QueryT(sb, ht);

            return dt;

        }
        catch
        {
            throw;
        }
    }

    public DataTable getCard_Detail()
    {
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CARD_TYPE from TB_D_M_CARD");
            sb.Append(" where CARD_NO = @CARD_NO_ACT");
            sb.Append(" and START_DT <= @MANAGER_DT_ACT");
            sb.Append(" and ISNULL(END_DT,'9999/12/31') > @MANAGER_DT_ACT");

            ht.Add("@CARD_NO_ACT", CARD_NO_ACT);
            ht.Add("@MANAGER_DT_ACT", MANAGER_DT_ACT);          


            DataTable dt = dbConn.QueryT(sb, ht);

            return dt;


        }
        catch
        {
            throw;
        }
    }
    
    public DataTable getAct_Data()
    {
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //sb.Append("select * from TB_D_R_RES_ACTURL_TEMP");
            sb.Append("select * from TB_D_R_RES_ACTURL");
            sb.Append(" where MANAGER_DT = @MANAGER_DT_ACT");
            sb.Append(" and EMP_ID = @EMP_ID_ACT");
            sb.Append(" and RESTAURANT_ERROR_CD = ''");

            ht.Add("@MANAGER_DT_ACT", MANAGER_DT_ACT);
            ht.Add("@EMP_ID_ACT", EMP_ID_ACT);


            DataTable dt = dbConn.QueryT(sb, ht);

            return dt;


        }
        catch
        {
            throw;
        }
    }

    public DataTable check_STATUS()
    {
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CLOCK_OUT_DT,CLOCK_IN_DT from TB_D_M_EMP_DUTY_CHECK_STATUS");
            sb.Append(" where CALENDAR_DT = @MANAGER_DT_ACT");
            sb.Append(" and EMP_ID = @EMP_ID_ACT");
           

            ht.Add("@MANAGER_DT_ACT", MANAGER_DT_ACT);
            ht.Add("@EMP_ID_ACT", EMP_ID_ACT);


            DataTable dt = dbConn.QueryT(sb, ht);

            if (dt.Rows.Count > 0)
            {
                CLOCK_OUT_DT = dt.Rows[0]["CLOCK_OUT_DT"].ToString();
                CLOCK_IN_DT = dt.Rows[0]["CLOCK_IN_DT"].ToString(); 
            }

            return dt;


        }
        catch
        {
            throw;
        }
    }

    public string check_EMP_MAIN()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select IS_DUTY_CHECK from TB_H_M_EMP");
            sb.Append(" where EMP_ID = @EMP_ID_ACT");    
      
            ht.Add("@EMP_ID_ACT", EMP_ID_ACT);


            DataTable dt = dbConn.QueryT(sb, ht);

            if(dt.Rows.Count >0){
                st = "1";
                IS_DUTY_CHECK = dt.Rows[0]["IS_DUTY_CHECK"].ToString();
            }
            return st;


        }
        catch
        {
            throw;
        }
    }

    public void getCLOCK_Time()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CLOCK_IN_DT,CLOCK_OUT_DT from TB_D_M_EMP_DUTY_CHECK_STATUS");
            sb.Append(" where EMP_ID = @EMP_ID_ACT");
            sb.Append(" and CALENDAR_DT =@MANAGER_DT_ACT");

            ht.Add("@EMP_ID_ACT", EMP_ID_ACT);
            ht.Add("@MANAGER_DT_ACT", MANAGER_DT_ACT);


            DataTable dt = dbConn.QueryT(sb, ht);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["CLOCK_IN_DT"].ToString() != "")
                {
                    CLOCK_IN_DT_ACT = DateTime.Parse(dt.Rows[0]["CLOCK_IN_DT"].ToString()).ToString("HHmm");
                }
                else {
                    CLOCK_IN_DT_ACT = "";
                }
                if (dt.Rows[0]["CLOCK_OUT_DT"].ToString() != "")
                {
                    CLOCK_OUT_DT_ACT = DateTime.Parse(dt.Rows[0]["CLOCK_OUT_DT"].ToString()).ToString("HHmm");
                }
                else
                {
                    CLOCK_OUT_DT_ACT = "";
                }                
                
            }
           

        }
        catch
        {
            throw;
        }
    }

    public void getPlant_CD()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select PLANT_CD from TB_D_M_CLOCK");
            sb.Append(" where CLOCK_NO = @CLOCK_NO");


            ht.Add("@CLOCK_NO", CLOCK_NO_ACT);
          

            DataTable dt = dbConn.QueryT(sb, ht);

            if (dt.Rows.Count > 0)
            {
                PLANT_CD = dt.Rows[0]["PLANT_CD"].ToString();                
            }


        }
        catch
        {
            throw;
        }
    }

    public void getCOmpany_CD()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COMPANY_CD,LEVEL_CD from TB_H_M_EMP");
            sb.Append(" where EMP_ID = @EMP_ID_ACT");


            ht.Add("@EMP_ID_ACT", EMP_ID_ACT);


            DataTable dt = dbConn.QueryT(sb, ht);

            if (dt.Rows.Count > 0)
            {
                COMPANY_CD_ACT = dt.Rows[0]["COMPANY_CD"].ToString();
                LEVEL_CD_ACT = dt.Rows[0]["LEVEL_CD"].ToString();
            }


        }
        catch
        {
            throw;
        }
    }

    public void insert_RES_ACTURL()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();           
            
            
            sb.Append("insert into TB_D_R_RES_ACTURL");
            sb.Append(" (MANAGER_DT,MANAGER_UNIT,EMP_ID,MEALSHIFT,DEPT_NO,WS_CD,LEVEL_CD,CLOCK_NO,CLOCK_PLANT_CD,RESTAURANT_CD,COMPANY_CD,");
            sb.Append(" WORK_SHIFT_NO,RESTAURANT_ERROR_CD,ST_COURSE_ID,COURSE_DATE,EXPENSE_CD,MEAL_AMOUNT,MEAL_TIMES,CARD_START,");
            sb.Append(" CARD_END,CARD_NO,MONTH_CLOSE_DT,MONTH_CLOSE_BY,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values (@MANAGER_DT_ACT,@COST_DEPT_NO,@EMP_ID_ACT,@MEALSHIFT,@DEPT_NO_ACT,@WS_CD_ACT,@LEVEL_CD_ACT,@CLOCK_NO_ACT,@PLANT_CD,@RESTAURANT_CD_ACT,@COMPANY_CD_ACT,");
            sb.Append(" @WORK_SHIFT_NO_ACT,@RESTAURANT_ERROR_CD,@COURSE_NAME,(case when isnull(@COURSE_DT,'') ='' then null else @COURSE_DT end),@EXPENSE_CD,@MEAL_AMOUNT,@MEAL_TIMES_ACT,@CLOCK_IN_DT_ACT,");
            sb.Append(" @CLOCK_OUT_DT_ACT,@CARD_NO_ACT,'9999/12/31','',@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID)");

            
            ht.Add("@MANAGER_DT_ACT", MANAGER_DT_ACT);
            ht.Add("@COST_DEPT_NO", COST_DEPT_NO);
            ht.Add("@EMP_ID_ACT", EMP_ID_ACT);
            ht.Add("@MEALSHIFT", MEALSHIFT);
            ht.Add("@DEPT_NO_ACT", DEPT_NO_ACT);
            ht.Add("@WS_CD_ACT", WS_CD_ACT);
            ht.Add("@LEVEL_CD_ACT", LEVEL_CD_ACT);
            ht.Add("@CLOCK_NO_ACT", CLOCK_NO_ACT);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@RESTAURANT_CD_ACT", RESTAURANT_CD_ACT);
            ht.Add("@COMPANY_CD_ACT", COMPANY_CD_ACT);
            ht.Add("@WORK_SHIFT_NO_ACT", WORK_SHIFT_NO_ACT);
            ht.Add("@RESTAURANT_ERROR_CD", RESTAURANT_ERROR_CD);
            ht.Add("@COURSE_NAME", COURSE_NAME);
            ht.Add("@COURSE_DT", COURSE_DT);
            ht.Add("@EXPENSE_CD", EXPENSE_CD);
            ht.Add("@MEAL_AMOUNT", MEAL_AMOUNT);
            ht.Add("@MEAL_TIMES_ACT", MEAL_TIMES_ACT);           
            ht.Add("@CLOCK_IN_DT_ACT", CLOCK_IN_DT_ACT);           
            ht.Add("@CLOCK_OUT_DT_ACT", CLOCK_OUT_DT_ACT);
            ht.Add("@CARD_NO_ACT", CARD_NO_ACT);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            

            dbConn.ExecuteT(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }

    public void insert_RES_ACTURL_TEMP()
    {        
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("insert into TB_D_R_RES_ACTURL_TEMP");
            sb.Append(" (MANAGER_DT,MANAGER_UNIT,EMP_ID,MEALSHIFT,DEPT_NO,WS_CD,LEVEL_CD,CLOCK_NO,CLOCK_PLANT_CD,RESTAURANT_CD,COMPANY_CD,");
            sb.Append(" WORK_SHIFT_NO,RESTAURANT_ERROR_CD,ST_COURSE_ID,COURSE_DATE,EXPENSE_CD,MEAL_AMOUNT,MEAL_TIMES,CARD_START,");
            sb.Append(" CARD_END,CARD_NO,MONTH_CLOSE_DT,MONTH_CLOSE_BY,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values (@MANAGER_DT_ACT,@COST_DEPT_NO,@EMP_ID_ACT,@MEALSHIFT,@DEPT_NO_ACT,@WS_CD_ACT,@LEVEL_CD_ACT,@CLOCK_NO_ACT,@PLANT_CD,@RESTAURANT_CD_ACT,@COMPANY_CD_ACT,");
            sb.Append(" @WORK_SHIFT_NO_ACT,@RESTAURANT_ERROR_CD,@COURSE_NAME,@COURSE_DT,@EXPENSE_CD,@MEAL_AMOUNT,@MEAL_TIMES_ACT,@CLOCK_IN_DT_ACT,");
            sb.Append(" @CLOCK_OUT_DT_ACT,@CARD_NO_ACT,'9999/12/31','',@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID)");


            ht.Add("@MANAGER_DT_ACT", MANAGER_DT_ACT);
            ht.Add("@COST_DEPT_NO", COST_DEPT_NO);
            ht.Add("@EMP_ID_ACT", EMP_ID_ACT);
            ht.Add("@MEALSHIFT", MEALSHIFT);
            ht.Add("@DEPT_NO_ACT", DEPT_NO_ACT);
            ht.Add("@WS_CD_ACT", WS_CD_ACT);
            ht.Add("@LEVEL_CD_ACT", LEVEL_CD_ACT);
            ht.Add("@CLOCK_NO_ACT", CLOCK_NO_ACT);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@RESTAURANT_CD_ACT", RESTAURANT_CD_ACT);
            ht.Add("@COMPANY_CD_ACT", COMPANY_CD_ACT);
            ht.Add("@WORK_SHIFT_NO_ACT", WORK_SHIFT_NO_ACT);
            ht.Add("@RESTAURANT_ERROR_CD", RESTAURANT_ERROR_CD);
            ht.Add("@COURSE_NAME", COURSE_NAME);
            ht.Add("@COURSE_DT", COURSE_DT);
            ht.Add("@EXPENSE_CD", EXPENSE_CD);
            ht.Add("@MEAL_AMOUNT", MEAL_AMOUNT);
            ht.Add("@MEAL_TIMES_ACT", MEAL_TIMES_ACT);
            ht.Add("@CLOCK_IN_DT_ACT", CLOCK_IN_DT_ACT);
            ht.Add("@CLOCK_OUT_DT_ACT", CLOCK_OUT_DT_ACT);
            ht.Add("@CARD_NO_ACT", CARD_NO_ACT);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);


            dbConn.ExecuteT(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }


    /*
     連線至Oracle
     */
    public string getEDU()
    {
        DBConnector_OLE dbs = new DBConnector_OLE(utilities.Oleconnstr);
        OleDbCommand comm = new OleDbCommand();
        try
        {
            string st = "";
            //StringBuilder sb = new StringBuilder();
            //Hashtable ht = new Hashtable();
            //sb.Append("select ST_COURSE_YEAR,COURSE_NAME from TB_M_ST_COURSE_D_N");
            //sb.Append(" where COURSE_DT >= TO_DATE(@SDT, 'YYYY-MM-DD')");
            //sb.Append(" and COURSE_DT <= TO_DATE(@EDT, 'YYYY-MM-DD'");
            //sb.Append(" and COURSE_STIME > @COURSE_DN_TIME");

            //ht.Add("@MANAGER_DT_ACT", MANAGER_DT_ACT);
            //ht.Add("@EMP_ID_ACT", EMP_ID_ACT);
            /*
            comm.CommandText = "select a.ST_COURSE_YEAR,a.COURSE_NAME,a.COURSE_DT from TB_M_ST_COURSE_D_N a";
            comm.CommandText += " left join TB_M_ST_COURSE_MEM_N b";
            comm.CommandText += " on a.ST_COURSE_YEAR = b.ST_COURSE_YEAR";
            comm.CommandText += " and a.COURSE_NAME = b.COURSE_NAME";
            comm.CommandText += " where COURSE_DT >= TO_DATE(?, 'YYYY-MM-DD')";
            comm.CommandText += " and COURSE_STIME > ?";
            comm.CommandText += " and b.EMP_ID = ?";
            
            //comm.Parameters.AddWithValue(New OracleParameter("param1", "1234"))
            comm.Parameters.AddWithValue("", SDT);
            comm.Parameters.AddWithValue("", COURSE_DN_TIME);
            comm.Parameters.AddWithValue("", EMP_ID_ACT);
            */

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.ST_COURSE_YEAR,a.COURSE_NAME,a.COURSE_DT from " + utilities.ORACLEName + "..TFB1U001.TB_M_ST_COURSE_D_N a ");
            sb.Append(" left join  " + utilities.ORACLEName + "..TFB1U001.TB_M_ST_COURSE_MEM_N b ");
            sb.Append("  on a.ST_COURSE_YEAR = b.ST_COURSE_YEAR ");
            sb.Append("  and a.COURSE_NAME = b.COURSE_NAME  ");
            sb.Append(" where COURSE_DT >= @COURSE_DT ");
            sb.Append(" and COURSE_STIME > @COURSE_STIME ");
            sb.Append(" and b.EMP_ID = @EMP_ID");
            ht.Add("@COURSE_DT", SDT);
            ht.Add("@COURSE_STIME", COURSE_DN_TIME);
            ht.Add("@EMP_ID", EMP_ID_ACT);
            


            DataTable dt = dbs.getDataTable(comm);
            if (dt.Rows.Count > 0)
            {
                st = "0";
                COURSE_NAME = dt.Rows[0]["COURSE_NAME"].ToString();
                if (dt.Rows[0]["COURSE_DT"].ToString() != null)
                {
                    COURSE_DT = DateTime.Parse(dt.Rows[0]["COURSE_DT"].ToString()).ToString("yyyy-MM-dd");
                }
                
            }
            return st;
                        

        }
        catch
        {
            throw;
        }
    }

    internal void SP_D_RESTAURANT_DAILY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_RESTAURANT_DAILY");
            ht.Add("@SDT", SDT);
            ht.Add("@EDT", EDT);
            ht.Add("@COMPANY_CD", COMPANY_CD);           
            ht.Add("@SYS_DT", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
            ht.Add("@USERID", CREATED_BY);
            ht.Add("@FUNCID", FUNC_ID);

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkSP(string PROC_ID)
    {

        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select top (1) PROC_STATUS, PROC_LOG  ");
            sb.AppendLine("   from TB_H_R_SP_LOG                  ");
            sb.AppendLine("  where  PROC_ID = @PROC_ID            ");
            sb.AppendLine("  order by PROC_DT desc                ");
            ht.Add("@PROC_ID", PROC_ID);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

}