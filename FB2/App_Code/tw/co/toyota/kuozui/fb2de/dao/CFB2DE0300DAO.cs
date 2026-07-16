using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// wfd2de 的摘要描述
/// </summary>
public class CFB2DE0300DAO : BaseDAO
{
    public string MANAGER_YM { get; set; }
    public string PLANT_CD { get; set; }
    public string emp_id { get; set; }
    public string MONTH_MANAGER_YM { get; set; }
    public string MONTH_CLOCK_NO { get; set; }
    public string MONTH_MANAGER_UNIT { get; set; }
    public string LAST_BR_TIME { get; set; }

    public CFB2DE0300DAO()
    {


    }
    public string getMANAGER_YM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string MANAGER_YM = "";
            string sql = @"   select  MAX(MANAGER_YM) MANAGER_YM from TB_D_R_RES_MONTH_ACTURL";
            sb.Append(sql);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                MANAGER_YM = dt.Rows[0]["MANAGER_YM"].ToString();
            }
            return MANAGER_YM;
        }
        catch
        {
            throw;
        }
    }
    public DataTable qry_TB_D_R_RES_MONTH_ACTURL(string MANAGER_YM, string PLANT_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string sql = @"   select  * from TB_D_R_RES_MONTH_ACTURL where MANAGER_YM=@MANAGER_YM  ";
            ht.Add("@MANAGER_YM", MANAGER_YM);

            if (!PLANT_CD.Equals(" "))
            {
                sql += "and CLOCK_PLANT_CD = @CLOCK_PLANT_CD";
                ht.Add("@CLOCK_PLANT_CD", PLANT_CD);
            }
            sb.Append(sql);
            
            

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable qry_TB_D_R_RES_ACTURL(string MANAGER_YM, string PLANT_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string sql = @"   select  * from TB_D_R_RES_ACTURL where left(Convert(varchar, MANAGER_DT,112),6)=@MANAGER_YM  ";
            ht.Add("@MANAGER_YM", MANAGER_YM);

            if (!PLANT_CD.Equals(" "))
            {
                sql += "and CLOCK_PLANT_CD = @CLOCK_PLANT_CD";
                ht.Add("@CLOCK_PLANT_CD", PLANT_CD);
            }
            sb.Append(sql);



            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public void del_TB_D_R_RES_MONTH_ACTURL(string MANAGER_YM, string PLANT_CD)
    {
        try
        {
            //BeginTransaction();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string sql = @"   delete TB_D_R_RES_MONTH_ACTURL where MANAGER_YM=@MANAGER_YM ";

            ht.Add("@MANAGER_YM", MANAGER_YM);

            if (!PLANT_CD.Equals(" "))
            {
                sql += "and CLOCK_PLANT_CD = @CLOCK_PLANT_CD";
                ht.Add("@CLOCK_PLANT_CD", PLANT_CD);
            }
            sb.Append(sql);
           
            
            dbConn.Execute(sb, ht, true);
            // Commit();

        }
        catch
        {
            throw;
        }


    }
    //取得教育人數
    public string getEDU_PEOPLE(string MANAGER_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string sql = @"  select count(*) as EDU_PEOPLE from TB_D_R_RES_ACTURL
                            where left(Convert(varchar, MANAGER_DT,112),6)=@MANAGER_YM and RESTAURANT_ERROR_CD='7'";
            sb.Append(sql);
            ht.Add("@MANAGER_YM", MANAGER_YM);
            DataTable dt = dbConn.Query(sb, ht);
            string EDU_PEOPLE = "";
            if (dt.Rows.Count > 0)
                EDU_PEOPLE = dt.Rows[0]["EDU_PEOPLE"].ToString();
            else
                EDU_PEOPLE = "0";
            return EDU_PEOPLE;
        }
        catch
        {
            throw;
        }
    }
    //執行月結
//    public bool Execute(string MANAGER_YM, string PLANT_CD, string emp_id)
//    {
//        try
//        {

//            int insert = 0, update_1 = 0, update_2 = 0;
//            StringBuilder sb = new StringBuilder();
//            Hashtable ht = new Hashtable();
//            string sql = @"  select left(Convert(varchar, MANAGER_DT,112),6) as MANAGER_YM,* from TB_D_R_RES_ACTURL
//                            where left(Convert(varchar, MANAGER_DT,112),6)=@MANAGER_YM ";
//            if (!PLANT_CD.Equals(" "))
//                sql += " and CLOCK_PLANT_CD=@CLOCK_PLANT_CD ";
//            sb.Append(sql);
//            ht.Add("@MANAGER_YM", MANAGER_YM);
//            ht.Add("@CLOCK_PLANT_CD", PLANT_CD);
//            DataTable dt = dbConn.Query(sb, ht);
//            if (dt.Rows.Count > 0)
//            {
//                for (int i = 0; i < dt.Rows.Count; i++)
//                {

//                    string MONTH_BR_PEOPLE = getMONTH_BR_PEOPLE(dt.Rows[i]["MANAGER_YM"].ToString(), dt.Rows[i]["MANAGER_UNIT"].ToString(), dt.Rows[i]["RESTAURANT_CD"].ToString());
//                    string man_ym = dt.Rows[i]["MANAGER_YM"].ToString();
//                    string CLOCK_NO = dt.Rows[i]["CLOCK_NO"].ToString();
//                    string CLOCK_PLANT_CD = dt.Rows[i]["CLOCK_PLANT_CD"].ToString();
//                    string RESTAURANT_CD = dt.Rows[i]["RESTAURANT_CD"].ToString();
//                    string MANAGER_UNIT = dt.Rows[i]["MANAGER_UNIT"].ToString();
//                    string HOLIDAY_BENTO_PEOPLE = "0";
//                    string VISITOR_BOND_PEOPLE = "0";
//                    string EDU_PEOPLE = getEDU_PEOPLE(MANAGER_YM);
//                    string MONTH_BR_BOND_PEOPLE = "0";
//                    string MONTH_DN_BOND_PEOPLE = "0";
//                    string OVERTIME_BOND_PEOPLE = "0";
//                    string ERROE_BR_PEOPLE = "0";
//                    string ERROE_DN_PEOPLE = "0";
//                    DataTable dt_month = checkdata(dt.Rows[i]["MANAGER_YM"].ToString(), dt.Rows[i]["MANAGER_UNIT"].ToString(), CLOCK_PLANT_CD);
//                    if (dt_month.Rows.Count > 0)
//                    {
//                        MONTH_MANAGER_YM = dt_month.Rows[0]["MANAGER_YM"].ToString();
//                        MONTH_CLOCK_NO = dt_month.Rows[0]["CLOCK_NO"].ToString();
//                        MONTH_MANAGER_UNIT = dt_month.Rows[0]["MANAGER_UNIT"].ToString();
//                        MONTH_BR_BOND_PEOPLE = dt_month.Rows[0]["MONTH_BR_BOND_PEOPLE"].ToString();
//                        MONTH_DN_BOND_PEOPLE = dt_month.Rows[0]["MONTH_DN_BOND_PEOPLE"].ToString();
//                        OVERTIME_BOND_PEOPLE = dt_month.Rows[0]["OVERTIME_BOND_PEOPLE"].ToString();
//                        ERROE_BR_PEOPLE = dt_month.Rows[0]["ERROE_BR_PEOPLE"].ToString();
//                        ERROE_DN_PEOPLE = dt_month.Rows[0]["ERROE_DN_PEOPLE"].ToString();
//                    }

//                    if (dt.Rows[i]["EXPENSE_CD"].ToString().Equals("1"))
//                    {
//                        if (dt.Rows[i]["MEALSHIFT"].ToString().Equals("A"))
//                            MONTH_BR_BOND_PEOPLE = (Convert.ToInt32(MONTH_BR_BOND_PEOPLE) + 1).ToString();
//                        if (dt.Rows[i]["MEALSHIFT"].ToString().Equals("B"))
//                            MONTH_DN_BOND_PEOPLE = (Convert.ToInt32(MONTH_DN_BOND_PEOPLE) + 1).ToString();
//                        if (dt.Rows[i]["MEALSHIFT"].ToString().Equals("C"))
//                            OVERTIME_BOND_PEOPLE = (Convert.ToInt32(OVERTIME_BOND_PEOPLE) + 1).ToString();
//                    }


//                    if (dt.Rows[i]["EXPENSE_CD"].ToString().Equals("2"))
//                    {
//                        if (dt.Rows[i]["MEALSHIFT"].ToString().Equals("A"))
//                            ERROE_BR_PEOPLE = (Convert.ToInt32(ERROE_BR_PEOPLE) + 1).ToString();
//                        else
//                            ERROE_DN_PEOPLE = (Convert.ToInt32(ERROE_DN_PEOPLE) + 1).ToString();

//                    }
//                    if (dt_month.Rows.Count > 0)
//                    {
//                        //update_1 += update_TB_D_R_RES_MONTH_ACTURL(MANAGER_YM, CLOCK_NO, CLOCK_PLANT_CD, RESTAURANT_CD, MANAGER_UNIT, MONTH_BR_PEOPLE, MONTH_BR_BOND_PEOPLE, MONTH_DN_BOND_PEOPLE, OVERTIME_BOND_PEOPLE,
//                        //         HOLIDAY_BENTO_PEOPLE, VISITOR_BOND_PEOPLE, EDU_PEOPLE, ERROE_BR_PEOPLE, ERROE_DN_PEOPLE);

//                    }
//                    else
//                    {
//                        //insert += insert_TB_D_R_RES_MONTH_ACTURL(MANAGER_YM, CLOCK_NO, CLOCK_PLANT_CD, RESTAURANT_CD, MANAGER_UNIT, MONTH_BR_PEOPLE, MONTH_BR_BOND_PEOPLE, MONTH_DN_BOND_PEOPLE, OVERTIME_BOND_PEOPLE,
//                        //        HOLIDAY_BENTO_PEOPLE, VISITOR_BOND_PEOPLE, EDU_PEOPLE, ERROE_BR_PEOPLE, ERROE_DN_PEOPLE);

//                    }

//                    //update_2 += update_TB_D_R_RES_ACTURL( MANAGER_YM, CLOCK_PLANT_CD, MANAGER_UNIT);
//                }
//            }
//            if ((insert > 0 || update_1 > 0) && update_2 > 0)
//                return true;
//            else
//                return false;


//        }
//        catch
//        {
//            throw;
//        }


//    }

    public DataTable checkdata(string MANAGER_YM, string MANAGER_UNIT, string CLOCK_PLANT_CD)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string sql = @" select  MANAGER_YM,CLOCK_NO,MANAGER_UNIT,isnull(MONTH_BR_BOND_PEOPLE,0) MONTH_BR_BOND_PEOPLE ,isnull(MONTH_DN_BOND_PEOPLE,0) MONTH_DN_BOND_PEOPLE,isnull(OVERTIME_BOND_PEOPLE,0) OVERTIME_BOND_PEOPLE
                                    ,isnull(ERROE_BR_PEOPLE,0) ERROE_BR_PEOPLE,isnull(ERROE_DN_PEOPLE,0) ERROE_DN_PEOPLE
                                from TB_D_R_RES_MONTH_ACTURL where  MANAGER_YM =@MANAGER_YM and MANAGER_UNIT=@MANAGER_UNIT and CLOCK_PLANT_CD=@CLOCK_PLANT_CD";
            sb.Append(sql);
            ht.Add("@MANAGER_YM", MANAGER_YM);
            ht.Add("@MANAGER_UNIT", MANAGER_UNIT);
            ht.Add("@CLOCK_PLANT_CD", CLOCK_PLANT_CD);
            DataTable dt = dbConn.Query(sb, ht);

            return dt;

        }
        catch
        {
            throw;
        }
    }

    //取得早餐出勤人數
    public string getMONTH_BR_PEOPLE(string MANAGER_YM, string MANAGER_UNIT, string RESTAURANT_CD)
    {
        try
        {
            string MONTH_BR_PEOPLE = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" 
                        select COST_DEPT_NO,RESTAURANT_CD,SUM(case when a.EMP_ID != '' then 1 else 0 end)ct from (
                        select a.EMP_ID,f.CODE_VAL1 as RESTAURANT_CD, d.COST_DEPT_NO,
	                        (CASE WHEN c.END_DT IS NULL THEN '9999/12/31' ELSE c.END_DT END) NEWEND 
	                        from TB_D_M_EMP_DUTY_CHECK_STATUS a 
	                        left join TB_H_M_EMP b on a.EMP_ID = b.EMP_ID 
	                        left join TB_H_M_DEPT c on b.DEPT_NO = c.DEPT_NO and c.START_DT <= getdate() 
	                        left join TB_H_M_DEPT_ACC d on c.ACC_DEPT_NO = d.ACC_DEPT_NO and d.IS_VALID = 'Y' 
	                        left join TB_D_M_CLOCK_RECORD e on e.CARD_NO = '00'+a.EMP_ID+'0' and e.CLOCK_DT = a.CLOCK_IN_DT  
	                        left join TB_9_M_COMM_D f on e.CLOCK_NO = f.CODE_VAL2 and f.SYS_CD = 'DE' and f.MAIN_CD = 'RES_D_CLOCK' 
	                        where left(Convert(varchar, a.CALENDAR_DT,112),6) = @MANAGER_YM and DUTY_CHECK_RESULT = 'Y'  
	                        and convert(int,CONVERT(varchar(2),DatePart(hour, a.CLOCK_IN_DT), 101)+ CONVERT(varchar(2),DatePart(minute, a.CLOCK_IN_DT), 101))  
	                        <= convert(int,  @LAST_BR_TIME) and COST_DEPT_NO = @MANAGER_UNIT and f.CODE_VAL1 = @RESTAURANT_CD 
	                        )a where  a.NEWEND > getdate()
	                        group by  COST_DEPT_NO,RESTAURANT_CD");


            //sb.Append(" select COST_DEPT_NO,RESTAURANT_CD,case when isnull(SUM(ct),'') ='' then 0 else SUM(ct) end ct from(");
            //sb.Append(" Select CALENDAR_DT,COST_DEPT_NO,RESTAURANT_CD,count(EMP_ID) ct");
            //sb.Append(" from(");
            //sb.Append(" select a.EMP_ID,CONVERT(char(10),a.CALENDAR_DT, 120)CALENDAR_DT,f.CODE_VAL1 RESTAURANT_CD,");
            //sb.Append(" d.COST_DEPT_NO,(CASE WHEN c.END_DT IS NULL THEN '9999/12/31' ELSE c.END_DT END) NEWEND");
            //sb.Append(" from TB_D_M_EMP_DUTY_CHECK_STATUS a");
            //sb.Append(" left join TB_H_M_EMP b on a.EMP_ID = b.EMP_ID");
            //sb.Append(" left join TB_H_M_DEPT c on b.DEPT_NO = c.DEPT_NO and c.START_DT <= getdate()");
            //sb.Append(" left join TB_H_M_DEPT_ACC d on c.ACC_DEPT_NO = d.ACC_DEPT_NO and d.IS_VALID = 'Y'");
            //sb.Append(" left join TB_D_M_CLOCK_RECORD e on e.CARD_NO = '00'+a.EMP_ID+'0' and e.CLOCK_DT = a.CLOCK_IN_DT ");            
            //sb.Append(" left join TB_9_M_COMM_D f on e.CLOCK_NO = f.CODE_VAL2 and f.SYS_CD = 'DE' and f.MAIN_CD = 'RES_D_CLOCK'");
            //sb.Append(" where left(Convert(varchar, a.CALENDAR_DT,112),6) = @MANAGER_YM and DUTY_CHECK_RESULT = 'Y' ");
            //sb.Append(" and convert(int,CONVERT(varchar(2),DatePart(hour, a.CLOCK_IN_DT), 101)+");
            //sb.Append(" CONVERT(varchar(2),DatePart(minute, a.CLOCK_IN_DT), 101))  <= convert(int,  @LAST_BR_TIME) and COST_DEPT_NO = @MANAGER_UNIT and f.CODE_VAL1 = @RESTAURANT_CD");
            //sb.Append(" ) a");
            //sb.Append(" where  NEWEND > getdate() ");
            //sb.Append(" group by CALENDAR_DT,COST_DEPT_NO,RESTAURANT_CD");
            //sb.Append(" )b");
            //sb.Append(" group by COST_DEPT_NO,RESTAURANT_CD");
            //sb.Append(" order by COST_DEPT_NO");

            //sb.Append("select sum(BR_PEOPLE)ct from TB_D_R_RES_DAY_ATTEND");
            //sb.Append(" where left( convert(varchar,MANAGER_DT,112),6) =@MANAGER_YM and MANAGER_UNIT= @MANAGER_UNIT and RESTAURANT_CD = @RESTAURANT_CD");           
            //sb.Append(" group by MANAGER_UNIT,RESTAURANT_CD");
            
            
            ht.Add("@MANAGER_YM", MANAGER_YM);
            ht.Add("@LAST_BR_TIME", LAST_BR_TIME);
            ht.Add("@MANAGER_UNIT", MANAGER_UNIT);
            ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            DataTable dt = dbConn.QueryT(sb, ht);
            if (dt.Rows.Count > 0)
            {
                MONTH_BR_PEOPLE = dt.Rows[0]["ct"].ToString();

            }
            else
            {
                MONTH_BR_PEOPLE = "0";
            }
            return MONTH_BR_PEOPLE;
        }
        catch
        {
            throw;
        }

    }
    public string getLAST_BR_TIME()
    {
        try
        {
            string LAST_BR_TIME = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select LAST_BR_TIME from TB_D_M_RES_PARA");        
            
            
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                LAST_BR_TIME = dt.Rows[0]["LAST_BR_TIME"].ToString();

            }
            else
            {
                LAST_BR_TIME = "0000";
            }
            return LAST_BR_TIME;
        }
        catch
        {
            throw;
        }

    }
    
    //新增月度用餐實績統計檔
    public void insert_TB_D_R_RES_MONTH_ACTURL(string MANAGER_YM, string CLOCK_NO, string CLOCK_PLANT_CD, string RESTAURANT_CD, string MANAGER_UNIT, string MONTH_BR_PEOPLE,
                                                string MONTH_BR_BOND_PEOPLE, string MONTH_DN_BOND_PEOPLE, string OVERTIME_BOND_PEOPLE, string HOLIDAY_BENTO_PEOPLE, string VISITOR_BOND_PEOPLE,
                                                string EDU_PEOPLE, string ERROE_BR_PEOPLE, string ERROE_DN_PEOPLE, string MONTH_MD_PEOPLE, string MONTH_MD_AMOUNT, string ERROR_MD_PEOPLE, string ERROR_MD_AMOUNT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string sql = @"insert into TB_D_R_RES_MONTH_ACTURL (MANAGER_YM, CLOCK_NO, CLOCK_PLANT_CD, RESTAURANT_CD, MANAGER_UNIT, MONTH_BR_PEOPLE, MONTH_BR_BOND_PEOPLE, MONTH_DN_BOND_PEOPLE,
                            OVERTIME_BOND_PEOPLE,HOLIDAY_BENTO_PEOPLE, VISITOR_BOND_PEOPLE, EDU_PEOPLE, ERROE_BR_PEOPLE, ERROE_DN_PEOPLE, MONTH_MD_PEOPLE, MONTH_MD_AMOUNT, ERROR_MD_PEOPLE, ERROR_MD_AMOUNT,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) 
                            values (@MANAGER_YM, @CLOCK_NO, @CLOCK_PLANT_CD,@RESTAURANT_CD, @MANAGER_UNIT, @MONTH_BR_PEOPLE,@MONTH_BR_BOND_PEOPLE, @MONTH_DN_BOND_PEOPLE, @OVERTIME_BOND_PEOPLE,
                            @HOLIDAY_BENTO_PEOPLE, @VISITOR_BOND_PEOPLE, @EDU_PEOPLE, @ERROE_BR_PEOPLE, @ERROE_DN_PEOPLE, @MONTH_MD_PEOPLE, @MONTH_MD_AMOUNT, @ERROR_MD_PEOPLE, @ERROR_MD_AMOUNT,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID)";
            sb.Append(sql);
            ht.Add("@MANAGER_YM", MANAGER_YM);
            ht.Add("@CLOCK_NO", CLOCK_NO);
            ht.Add("@CLOCK_PLANT_CD", CLOCK_PLANT_CD);
            ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            ht.Add("@MANAGER_UNIT", MANAGER_UNIT);
            ht.Add("@MONTH_BR_PEOPLE", MONTH_BR_PEOPLE);
            ht.Add("@MONTH_BR_BOND_PEOPLE", MONTH_BR_BOND_PEOPLE);
            ht.Add("@MONTH_DN_BOND_PEOPLE", MONTH_DN_BOND_PEOPLE);
            ht.Add("@OVERTIME_BOND_PEOPLE", OVERTIME_BOND_PEOPLE);
            ht.Add("@HOLIDAY_BENTO_PEOPLE", HOLIDAY_BENTO_PEOPLE);
            ht.Add("@VISITOR_BOND_PEOPLE", VISITOR_BOND_PEOPLE);
            ht.Add("@EDU_PEOPLE", EDU_PEOPLE);
            ht.Add("@ERROE_BR_PEOPLE", ERROE_BR_PEOPLE);
            ht.Add("@ERROE_DN_PEOPLE", ERROE_DN_PEOPLE);
            ht.Add("@MONTH_MD_PEOPLE", MONTH_MD_PEOPLE);
            ht.Add("@MONTH_MD_AMOUNT", MONTH_MD_AMOUNT);
            ht.Add("@ERROR_MD_PEOPLE", ERROR_MD_PEOPLE);
            ht.Add("@ERROR_MD_AMOUNT", ERROR_MD_AMOUNT);
            ht.Add("@CREATED_BY", emp_id);
            //ht.Add("@CREATED_DT", DateTime.Now);
            ht.Add("@UPDATED_BY", emp_id);
            //ht.Add("@UPDATED_DT", DateTime.Now);
            ht.Add("@FUNC_ID", "FB2DE030");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }

    }
    //更新月度用餐實績統計檔
    public void update_TB_D_R_RES_MONTH_ACTURL(string MANAGER_YM, string CLOCK_NO, string CLOCK_PLANT_CD, string RESTAURANT_CD, string MANAGER_UNIT, string MONTH_BR_PEOPLE,
                                                string MONTH_BR_BOND_PEOPLE, string MONTH_DN_BOND_PEOPLE, string OVERTIME_BOND_PEOPLE, string HOLIDAY_BENTO_PEOPLE, string VISITOR_BOND_PEOPLE,
                                                string EDU_PEOPLE, string ERROE_BR_PEOPLE, string ERROE_DN_PEOPLE, string MONTH_MD_PEOPLE, string MONTH_MD_AMOUNT, string ERROR_MD_PEOPLE, string ERROR_MD_AMOUNT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string sql = @" update TB_D_R_RES_MONTH_ACTURL set MANAGER_YM=@MANAGER_YM, CLOCK_NO=@CLOCK_NO, CLOCK_PLANT_CD=@CLOCK_PLANT_CD, RESTAURANT_CD=@RESTAURANT_CD, MANAGER_UNIT=@MANAGER_UNIT
                            , MONTH_BR_PEOPLE=@MONTH_BR_PEOPLE, MONTH_BR_BOND_PEOPLE=@MONTH_BR_BOND_PEOPLE, MONTH_DN_BOND_PEOPLE=@MONTH_DN_BOND_PEOPLE,OVERTIME_BOND_PEOPLE=@OVERTIME_BOND_PEOPLE
                            ,HOLIDAY_BENTO_PEOPLE=@HOLIDAY_BENTO_PEOPLE, VISITOR_BOND_PEOPLE=@VISITOR_BOND_PEOPLE, EDU_PEOPLE=@EDU_PEOPLE, ERROE_BR_PEOPLE=@ERROE_BR_PEOPLE, ERROE_DN_PEOPLE=@ERROE_DN_PEOPLE
                            ,MONTH_MD_PEOPLE = @MONTH_MD_PEOPLE,MONTH_MD_AMOUNT=@MONTH_MD_AMOUNT,ERROR_MD_PEOPLE=@ERROR_MD_PEOPLE,ERROR_MD_AMOUNT = @ERROR_MD_AMOUNT
                            ,UPDATED_BY=@UPDATED_BY,UPDATED_DT= getdate(),FUNC_ID=@FUNC_ID where MANAGER_YM=@MANAGER_YM and CLOCK_NO=@CLOCK_NO and  MANAGER_UNIT=@MANAGER_UNIT";
            sb.Append(sql);
            ht.Add("@MANAGER_YM", MANAGER_YM);
            ht.Add("@CLOCK_NO", CLOCK_NO);
            ht.Add("@CLOCK_PLANT_CD", CLOCK_PLANT_CD);
            ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            ht.Add("@MANAGER_UNIT", MANAGER_UNIT);
            ht.Add("@MONTH_BR_PEOPLE", MONTH_BR_PEOPLE);
            ht.Add("@MONTH_BR_BOND_PEOPLE", MONTH_BR_BOND_PEOPLE);
            ht.Add("@MONTH_DN_BOND_PEOPLE", MONTH_DN_BOND_PEOPLE);
            ht.Add("@OVERTIME_BOND_PEOPLE", OVERTIME_BOND_PEOPLE);
            ht.Add("@HOLIDAY_BENTO_PEOPLE", HOLIDAY_BENTO_PEOPLE);
            ht.Add("@VISITOR_BOND_PEOPLE", VISITOR_BOND_PEOPLE);
            ht.Add("@EDU_PEOPLE", EDU_PEOPLE);
            ht.Add("@ERROE_BR_PEOPLE", ERROE_BR_PEOPLE);
            ht.Add("@ERROE_DN_PEOPLE", ERROE_DN_PEOPLE);
            ht.Add("@MONTH_MD_PEOPLE", MONTH_MD_PEOPLE);
            ht.Add("@MONTH_MD_AMOUNT", MONTH_MD_AMOUNT);
            ht.Add("@ERROR_MD_PEOPLE", ERROR_MD_PEOPLE);
            ht.Add("@ERROR_MD_AMOUNT", ERROR_MD_AMOUNT);
            ht.Add("@UPDATED_BY", emp_id);
            //ht.Add("@UPDATED_DT", DateTime.Now);
            ht.Add("@FUNC_ID", "FB2DE030");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }

    }

    //更新用餐實績檔
    public void update_TB_D_R_RES_ACTURL(string MANAGER_YM, string CLOCK_PLANT_CD, string MANAGER_UNIT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string sql = @" update TB_D_R_RES_ACTURL set MONTH_CLOSE_DT=getdate(),MONTH_CLOSE_BY=@MONTH_CLOSE_BY,UPDATED_BY=@UPDATED_BY,UPDATED_DT=getdate(),FUNC_ID=@FUNC_ID
                                where left(Convert(varchar, MANAGER_DT,112),6)=@MANAGER_YM and CLOCK_PLANT_CD=@CLOCK_PLANT_CD and  MANAGER_UNIT=@MANAGER_UNIT ";
            sb.Append(sql);
            ht.Add("@MANAGER_YM", MANAGER_YM);
            ht.Add("@CLOCK_PLANT_CD", CLOCK_PLANT_CD);
            ht.Add("@MANAGER_UNIT", MANAGER_UNIT);
            //ht.Add("@MONTH_CLOSE_DT", DateTime.Now);
            ht.Add("@MONTH_CLOSE_BY", emp_id);
            ht.Add("@UPDATED_BY", emp_id);
            //ht.Add("@UPDATED_DT", DateTime.Now);
            ht.Add("@FUNC_ID", "FB2DE030");

            dbConn.ExecuteT(sb, ht, true);

        }
        catch
        {
            throw;
        }

    }

    public DataTable selectMonthData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            //sb.Append(" select MANAGER_YM,CLOCK_NO,CLOCK_PLANT_CD,RESTAURANT_CD,MANAGER_UNIT,MONTH_BR_PEOPLE,MONTH_BR_BOND_PEOPLE,");
            //sb.Append(" MONTH_DN_BOND_PEOPLE,OVERTIME_BOND_PEOPLE,HOLIDAY_BENTO_PEOPLE,VISITOR_BOND_PEOPLE,EDU_PEOPLE,");
            //sb.Append(" ERROE_BR_PEOPLE,ERROE_DN_PEOPLE,CREATED_BY,CONVERT(char(10),CREATED_DT, 120) CREATED_DT,UPDATED_BY,CONVERT(char(10),UPDATED_DT, 120) UPDATED_DT,FUNC_ID");
            sb.Append(" select *");
            sb.Append(" from TB_D_R_RES_MONTH_ACTURL");
            sb.Append(" where MANAGER_YM = @MANAGER_YM");

            ht.Add("@MANAGER_YM", MANAGER_YM);

            if (PLANT_CD != " ")
            {
                sb.Append(" and CLOCK_PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", PLANT_CD);
            }

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getActualData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            sb.Append("select left(Convert(varchar, MANAGER_DT,112),6) MANAGER_DT,MANAGER_UNIT,");
            sb.Append(" case when RESTAURANT_CD = '1' then '109' when RESTAURANT_CD = '2' then '111' ");
            sb.Append(" when RESTAURANT_CD = '3' then '205' when RESTAURANT_CD = '4' then '211' end as CLOCK_NO,");
            sb.Append(" CLOCK_PLANT_CD,RESTAURANT_CD,");
            sb.Append(" sum(case when EXPENSE_CD = '1' and MEALSHIFT ='A' then 1 else 0 end) MONTH_BR_BOND_PEOPLE,");
            sb.Append(" sum(case when EXPENSE_CD = '1' and MEALSHIFT ='B' then 1 else 0 end) MONTH_DN_BOND_PEOPLE,");
            sb.Append(" sum(case when EXPENSE_CD = '1' and MEALSHIFT ='C' then 1 else 0 end) OVERTIME_BOND_PEOPLE,");
            sb.Append(" sum(case when EXPENSE_CD = '2' and MEALSHIFT ='A' then 1 else 0 end) ERROE_BR_PEOPLE,");
            sb.Append(" sum(case when EXPENSE_CD = '2' and (MEALSHIFT ='B' or MEALSHIFT ='C') then 1 else 0 end) ERROE_DN_PEOPLE,");            
            sb.Append(" sum(case when RESTAURANT_ERROR_CD = '7' then 1 else 0 end) EDU_PEOPLE,");
            sb.Append(" sum(case when isnull(RESTAURANT_ERROR_CD,'') = ''and MEALSHIFT ='D' then 1 else 0 end) MONTH_MD_PEOPLE, /*午餐人數*/");
            sb.Append(" sum(case when isnull(RESTAURANT_ERROR_CD,'') = ''and MEALSHIFT ='D' then MEAL_AMOUNT else 0 end) MONTH_MD_AMOUNT, /*午餐金額*/");
            sb.Append(" sum(case when isnull(RESTAURANT_ERROR_CD,'') != ''and MEALSHIFT ='D' then 1 else 0 end) ERROR_MD_PEOPLE, /*異常人數（午餐）*/");
            sb.Append(" sum(case when isnull(RESTAURANT_ERROR_CD,'') != ''and MEALSHIFT ='D' then MEAL_AMOUNT else 0 end) ERROR_MD_AMOUNT /*異常金額（午餐）*/");
            sb.Append(" from TB_D_R_RES_ACTURL");
            sb.Append(" where left(Convert(varchar, MANAGER_DT,112),6)=@MANAGER_YM");    

            ht.Add("@MANAGER_YM", MANAGER_YM);
            if (PLANT_CD != " ")
            {
                sb.Append(" and CLOCK_PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", PLANT_CD);
            }

            sb.Append(" group by left(Convert(varchar, MANAGER_DT,112),6),MANAGER_UNIT,CLOCK_PLANT_CD,RESTAURANT_CD");
            sb.Append(" order by left(Convert(varchar, MANAGER_DT,112),6),RESTAURANT_CD,MANAGER_UNIT");

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insertMonth(string MANAGER_YM, string CLOCK_NO, string CLOCK_PLANT_CD, string RESTAURANT_CD, string MANAGER_UNIT, string MONTH_BR_PEOPLE,
                            string MONTH_BR_BOND_PEOPLE, string MONTH_DN_BOND_PEOPLE, string OVERTIME_BOND_PEOPLE, string HOLIDAY_BENTO_PEOPLE, string VISITOR_BOND_PEOPLE,
                            string EDU_PEOPLE, string ERROE_BR_PEOPLE, string ERROE_DN_PEOPLE,string CREATED_BY, string CREATED_DT, string UPDATED_BY,string UPDATED_DT, string FUNC_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("insert into TB_D_R_RES_MONTH_ACTURL (MANAGER_YM, CLOCK_NO, CLOCK_PLANT_CD, RESTAURANT_CD, MANAGER_UNIT, MONTH_BR_PEOPLE, MONTH_BR_BOND_PEOPLE, MONTH_DN_BOND_PEOPLE,");
            sb.Append("OVERTIME_BOND_PEOPLE,HOLIDAY_BENTO_PEOPLE, VISITOR_BOND_PEOPLE, EDU_PEOPLE, ERROE_BR_PEOPLE, ERROE_DN_PEOPLE,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values (@MANAGER_YM, @CLOCK_NO, @CLOCK_PLANT_CD,@RESTAURANT_CD, @MANAGER_UNIT, @MONTH_BR_PEOPLE,@MONTH_BR_BOND_PEOPLE, @MONTH_DN_BOND_PEOPLE, @OVERTIME_BOND_PEOPLE,");
            sb.Append(" @HOLIDAY_BENTO_PEOPLE, @VISITOR_BOND_PEOPLE, @EDU_PEOPLE, @ERROE_BR_PEOPLE, @ERROE_DN_PEOPLE,@CREATED_BY,@CREATED_DT,@UPDATED_BY,@UPDATED_DT,@FUNC_ID)");

            ht.Add("@MANAGER_YM", MANAGER_YM);
            ht.Add("@CLOCK_NO", CLOCK_NO);
            ht.Add("@CLOCK_PLANT_CD", CLOCK_PLANT_CD);
            ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            ht.Add("@MANAGER_UNIT", MANAGER_UNIT);
            ht.Add("@MONTH_BR_PEOPLE", MONTH_BR_PEOPLE);
            ht.Add("@MONTH_BR_BOND_PEOPLE", MONTH_BR_BOND_PEOPLE);
            ht.Add("@MONTH_DN_BOND_PEOPLE", MONTH_DN_BOND_PEOPLE);
            ht.Add("@OVERTIME_BOND_PEOPLE", OVERTIME_BOND_PEOPLE);
            ht.Add("@HOLIDAY_BENTO_PEOPLE", HOLIDAY_BENTO_PEOPLE);
            ht.Add("@VISITOR_BOND_PEOPLE", VISITOR_BOND_PEOPLE);
            ht.Add("@EDU_PEOPLE", EDU_PEOPLE);
            ht.Add("@ERROE_BR_PEOPLE", ERROE_BR_PEOPLE);
            ht.Add("@ERROE_DN_PEOPLE", ERROE_DN_PEOPLE);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@CREATED_DT", CREATED_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", UPDATED_DT);
            ht.Add("@FUNC_ID", FUNC_ID);
            

           
           dbConn.Execute(sb, ht);
           
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string getCal(string MANAGER_YM, string PLANT_CD)
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select  * from TB_D_R_RES_ACTURL ");
            sb.Append(" where left(Convert(varchar, MANAGER_DT,112),6)=@MANAGER_YM");

            ht.Add("@MANAGER_YM", MANAGER_YM);
            if (PLANT_CD != " ")
            {
                sb.Append(" and CLOCK_PLANT_CD = @PLANT_CD");
                ht.Add("@PLANT_CD", PLANT_CD);
            }
            

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


}