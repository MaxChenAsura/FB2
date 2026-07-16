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
/// CFB2SA1500DAO 的摘要描述
/// </summary>
public class CFB2SA1500DAO : BaseDAO
{
    public string DATA_YEAR { get; set; }
    public string SALARY_YM { get; set; }
    public string SALARY_DT { get; set; }
    public string SALARY_SDT { get; set; }
    public string SALARY_EDT { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    public string EMP_ID { get; set; }
    





	public CFB2SA1500DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable select_Salary()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select convert(varchar,SALARY_DT,111)SALARY_DT,SALARY_YM,convert(varchar,SALARY_SDT,111)SALARY_SDT,convert(varchar,SALARY_EDT,111)SALARY_EDT ");
        sb.Append(" from TB_S_M_SALARY_CAL_H where SALARY_TYPE ='A' and PROCESS_STATUS='4' and SALARY_SDT >= (select cast(@DATA_YEAR+p.CODE_VAL1 as date)  from TB_9_M_PARAMETER p ");
        sb.Append(" where p.SYS_CD='SA' and  p.MAIN_CD='HIRING_SALARY_CHG_SDT'  )");
        sb.Append(" and SALARY_EDT <= getdate()");

        ht.Add("@DATA_YEAR", DATA_YEAR);

        return dbConn.QueryT(sb, ht);

    }

    public DataTable select_Excel_Data()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" SELECT t1.EMP_ID,t3.EMP_NAME,convert(varchar,t3.JOIN_DT,111) JOIN_DT,convert(varchar,t3.BE_EMP_DT,111) BE_EMP_DT,t4.ABILITY_PAY_A,t3.WORK_DAYS_MONTH");
        sb.Append(" ,t1.SALARY_YM,t1.ABILITY_REPAY,t1.NO_TAX_OVERTIME_REPAY,t1.TAX_OVERTIME_REPAY,t1.LEAVE_REPAY,t1.WORK_SHIFT_REPAY");
        sb.Append(" FROM TB_S_M_HIRING_SALARY_REPAY_MEM t1");
        sb.Append(" left join TB_S_M_EMP_RESULT t3 on t1.SALARY_YM = t3.SALARY_YM and t1.EMP_ID= t3.EMP_ID ");
        sb.Append(" left join TB_S_M_HIRING_SALARY_MEM_D t4 on t1.EMP_ID = t4.EMP_ID");
        sb.Append(" where t1.DATA_YEAR = @DATA_YEAR");
        sb.Append(" order By t1.EMP_ID,t1.SALARY_YM");

        ht.Add("@DATA_YEAR", DATA_YEAR);

        return dbConn.Query(sb, ht);

    }
    public DataTable select_title1()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select MAX(distinct SALARY_YM) maxValue,MIN(distinct SALARY_YM) minValue from TB_S_M_SALARY_CAL_H where SALARY_TYPE ='A' and PROCESS_STATUS='4' ");
        sb.Append(" and SALARY_SDT >=  (select cast(@DATA_YEAR+p.CODE_VAL1 as date)   from TB_9_M_PARAMETER p ");
        sb.Append(" where p.SYS_CD='SA' and  p.MAIN_CD='HIRING_SALARY_CHG_SDT'  )  and SALARY_EDT <= getdate()");

        ht.Add("@DATA_YEAR", DATA_YEAR);
        return dbConn.Query(sb, ht);

    }
    public DataTable select_newJoin()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" SELECT COUNT(*) ct FROM TB_S_M_HIRING_SALARY_MEM_D where DATA_YEAR = @DATA_YEAR and BE_EMP_DT is null ");
       
        ht.Add("@DATA_YEAR", DATA_YEAR);
        return dbConn.Query(sb, ht);

    }
    public DataTable select_BE_EMP()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" SELECT COUNT(*) ct FROM TB_S_M_HIRING_SALARY_MEM_D where DATA_YEAR = @DATA_YEAR and BE_EMP_DT is not null ");

        ht.Add("@DATA_YEAR", DATA_YEAR);
        return dbConn.Query(sb, ht);

    }
    public DataTable select_OverTime()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select ROW_NUMBER() OVER(ORDER BY right( '0'+d.SUB_CD,2) ) As RowNumber,d.SUB_CD,d.SUB_DESC from TB_9_M_COMM_D d");
        sb.Append(" where   d.SYS_CD ='SC' and  d.MAIN_CD='OVERTIME_PAY_TYPE'");
        sb.Append(" AND d.IS_VALID='Y'");     

        return dbConn.Query(sb, ht);

    }
    public DataTable select_WorkShift()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select ROW_NUMBER() OVER(ORDER BY right( '0'+d.SUB_CD,2) ) As RowNumber, d.SUB_CD,d.SUB_DESC from TB_9_M_COMM_D d ");
        sb.Append(" where  d.SYS_CD ='SC' and  d.MAIN_CD='WORK_SHIFT_ALLOWANCE_TYPE'");
        sb.Append(" AND d.IS_VALID='Y'");

        return dbConn.Query(sb, ht);

    }
    public DataTable select_LEAVE_TYPE()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select ROW_NUMBER() OVER(ORDER BY right( '0'+d.SUB_LEAVE_CD,2) ) As RowNumber, d.SUB_LEAVE_CD,d.SUB_LEAVE_DESC from TB_D_M_LEAVE_TYPE_D d ");
        sb.Append(" where d.IS_USED='Y' and d.LEAVE_PAY_RATE<>1 and d.SUB_LEAVE_CD<>'Y0'");
        

        return dbConn.Query(sb, ht);

    }
    public DataTable select_REPAY_TYPE2()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select t1.REPAY_SUB_ID,SUM(t1.UNITS) as TOTAL,k.RowNumber");
        sb.Append(" from TB_S_M_HIRING_SALARY_REPAY_D t1");
        sb.Append(" left join (");
        sb.Append(" select ROW_NUMBER() OVER(ORDER BY right( '0'+d.SUB_CD,2) ) As RowNumber,d.SUB_CD,d.SUB_DESC from TB_9_M_COMM_D d");
        sb.Append(" where  d.SYS_CD ='SC' and  d.MAIN_CD='OVERTIME_PAY_TYPE' AND d.IS_VALID='Y'");
        sb.Append(" ) k on t1.REPAY_SUB_ID = k.SUB_CD");
        sb.Append(" where t1.DATA_YEAR =@DATA_YEAR  and t1.EMP_ID =@EMP_ID and t1.SALARY_YM=@SALARY_YM and t1.REPAY_TYPE ='2'");
        sb.Append(" Group by t1.REPAY_SUB_ID,k.RowNumber");
        sb.Append(" order by t1.REPAY_SUB_ID");

        ht.Add("@DATA_YEAR", DATA_YEAR);
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SALARY_YM", SALARY_YM);

        return dbConn.Query(sb, ht);

    }

    public DataTable select_REPAY_TYPE(string type)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select t1.REPAY_SUB_ID,SUM(t1.UNITS) as TOTAL,k.RowNumber");
        sb.Append(" from TB_S_M_HIRING_SALARY_REPAY_D t1");
        sb.Append(" left join (");
        if (type == "2")
        {
            sb.Append(" select ROW_NUMBER() OVER(ORDER BY right( '0'+d.SUB_CD,2) ) As RowNumber,d.SUB_CD,d.SUB_DESC from TB_9_M_COMM_D d");
            sb.Append(" where  d.SYS_CD ='SC' and  d.MAIN_CD='OVERTIME_PAY_TYPE' AND d.IS_VALID='Y'");
            sb.Append(" ) k on t1.REPAY_SUB_ID = k.SUB_CD");
        }
        if (type == "3")
        {
            sb.Append(" select ROW_NUMBER() OVER(ORDER BY right( '0'+d.SUB_CD,2) ) As RowNumber, d.SUB_CD,d.SUB_DESC from TB_9_M_COMM_D d ");
            sb.Append(" where  d.SYS_CD ='SC' and  d.MAIN_CD='WORK_SHIFT_ALLOWANCE_TYPE' AND d.IS_VALID='Y'");
            sb.Append(" ) k on t1.REPAY_SUB_ID = k.SUB_CD");
        }
        if (type == "1")
        {
            sb.Append(" select ROW_NUMBER() OVER(ORDER BY right( '0'+d.SUB_LEAVE_CD,2) ) As RowNumber, d.SUB_LEAVE_CD,d.SUB_LEAVE_DESC from TB_D_M_LEAVE_TYPE_D d ");
            sb.Append(" where d.IS_USED='Y' and d.LEAVE_PAY_RATE<>1 and d.SUB_LEAVE_CD<>'Y0'");
            sb.Append(" ) k on t1.REPAY_SUB_ID = k.SUB_LEAVE_CD");
        }        

        sb.Append(" where t1.DATA_YEAR =@DATA_YEAR  and t1.EMP_ID =@EMP_ID and t1.SALARY_YM=@SALARY_YM and t1.REPAY_TYPE =@type");
        sb.Append(" Group by t1.REPAY_SUB_ID,k.RowNumber");
        sb.Append(" order by t1.REPAY_SUB_ID");

        ht.Add("@DATA_YEAR", DATA_YEAR);
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SALARY_YM", SALARY_YM);
        ht.Add("@type", type);

        return dbConn.Query(sb, ht);

    }

    public void delete_HIRING_SALARY_REPAY_D()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();


        sb.Append(" delete from TB_S_M_HIRING_SALARY_REPAY_D where DATA_YEAR = @DATA_YEAR");       

        ht.Add("@DATA_YEAR", DATA_YEAR);
       
        dbConn.ExecuteT(sb, ht, true);

    }
    public void delete_HIRING_SALARY_REPAY_MEM()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();


        sb.Append(" delete from TB_S_M_HIRING_SALARY_REPAY_MEM where DATA_YEAR = @DATA_YEAR");

        ht.Add("@DATA_YEAR", DATA_YEAR);

        dbConn.ExecuteT(sb, ht, true);

    }
    public void delete_SUBSIDY_DEDUCTIONS_1()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();


        sb.Append(" delete from TB_S_M_SUBSIDY_DEDUCTIONS_1 where REMARK like @REMARK");

        ht.Add("@REMARK", DATA_YEAR + "FB2SA150%");

        dbConn.ExecuteT(sb, ht, true);

    }
    public void insert_HIRING()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        

        sb.Append(" insert into TB_S_M_HIRING_SALARY_REPAY_D (");
        sb.Append(" DATA_YEAR,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,SALARY_YM,EMP_ID,REPAY_TYPE,REPAY_SUB_ID,UNITS,BASE_VALUE) ");
        sb.Append("  select @DATA_YEAR,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID,");
        sb.Append(" @SALARY_YM,t1.EMP_ID,'2' as REPAY_TYPE,t1.OVERTIME_PAY_TYPE as REPAY_SUB_ID,t1.TOTAL_HOURS,cast(isnull(d.CODE_VAL1,'0') as decimal(4,2)) as BASE_AMT ");
        sb.Append(" from TB_S_M_OVERTIME_RESULT_D t1");
        sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='OVERTIME_PAY_TYPE' and  t1.OVERTIME_PAY_TYPE = d.SUB_CD");
        sb.Append(" where t1.SALARY_DT = convert(varchar,@SALARY_DT,111) ");
        sb.Append(" and t1.EMP_ID in (SELECT EMP_ID from TB_S_M_HIRING_SALARY_MEM_D where DATA_YEAR = @DATA_YEAR)");
        sb.Append(" UNION ALL");
        sb.Append(" SELECT @DATA_YEAR,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID,");
        sb.Append(" @SALARY_YM,t1.EMP_ID,t1.REPAY_TYPE,t1.REPAY_SUB_ID as REPAY_SUB_ID,t1.UNITS,t1.BASE_VALUE");
        sb.Append(" from TB_S_M_OVERTIME_REPAY t1");
        sb.Append(" where t1.REPAY_TYPE='2' and t1.REPAY_DT >= convert(varchar,@SALARY_SDT,111) and t1.REPAY_DT <= convert(varchar,@SALARY_EDT,111) and t1.PROCESS_STATUS ='Y' ");
        sb.Append(" and t1.EMP_ID in (SELECT EMP_ID from TB_S_M_HIRING_SALARY_MEM_D where DATA_YEAR = @DATA_YEAR)");        

        ht.Add("@CREATED_BY", CREATED_BY);
        ht.Add("@UPDATED_BY", UPDATED_BY);
        ht.Add("@FUNC_ID", FUNC_ID);
        ht.Add("@SALARY_YM", SALARY_YM);
        ht.Add("@SALARY_DT", SALARY_DT);
        ht.Add("@DATA_YEAR", DATA_YEAR);
        ht.Add("@SALARY_SDT", SALARY_SDT);
        ht.Add("@SALARY_EDT", SALARY_EDT);
        dbConn.ExecuteT(sb, ht, true);

    }
    public void insert_HIRING2()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" insert into TB_S_M_HIRING_SALARY_REPAY_D (");
        sb.Append(" DATA_YEAR,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,SALARY_YM,EMP_ID,REPAY_TYPE,REPAY_SUB_ID,UNITS,BASE_VALUE) ");
        sb.Append(" select @DATA_YEAR,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID,");
        sb.Append(" @SALARY_YM,t1.EMP_ID,'1' as REPAY_TYPE,t1.SUB_LEAVE_CD as REPAY_SUB_ID,t1.TOTAL_HOURS,( 1- t1.LEAVE_PAY_RATE) as BASE_AMT ");
        sb.Append(" from TB_S_M_LEAVE_RESULT_D t1");
        sb.Append(" where t1.SALARY_DT = @SALARY_DT and t1.LEAVE_PAY_RATE<>1 and t1.SUB_LEAVE_CD<>'Y0'  /*排除欠勤扣款*/");
        sb.Append(" and t1.EMP_ID in (SELECT EMP_ID from TB_S_M_HIRING_SALARY_MEM_D where DATA_YEAR = @DATA_YEAR)");       
        sb.Append(" UNION ALL");
        sb.Append(" select @DATA_YEAR,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID,");
        sb.Append(" @SALARY_YM,t1.EMP_ID,t1.REPAY_TYPE,t1.REPAY_SUB_ID as REPAY_SUB_ID,t1.UNITS,t1.BASE_VALUE");
        sb.Append(" from TB_S_M_OVERTIME_REPAY t1");
        sb.Append(" where t1.REPAY_TYPE='1' and t1.REPAY_DT >= @SALARY_SDT and t1.REPAY_DT <= @SALARY_EDT and t1.PROCESS_STATUS ='Y' and t1.REPAY_SUB_ID<>'Y0'  /*排除欠勤扣款*/");
        sb.Append(" and t1.EMP_ID in (SELECT EMP_ID from TB_S_M_HIRING_SALARY_MEM_D where DATA_YEAR =  @DATA_YEAR)");
        
        ht.Add("@CREATED_BY", CREATED_BY);
        ht.Add("@UPDATED_BY", UPDATED_BY);
        ht.Add("@FUNC_ID", FUNC_ID);
        ht.Add("@SALARY_YM", SALARY_YM);
        ht.Add("@SALARY_DT", SALARY_DT);
        ht.Add("@DATA_YEAR", DATA_YEAR);
        ht.Add("@SALARY_SDT", SALARY_SDT);
        ht.Add("@SALARY_EDT", SALARY_EDT);
        dbConn.ExecuteT(sb, ht, true);

    }
    public void insert_HIRING3()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" insert into TB_S_M_HIRING_SALARY_REPAY_D (");
        sb.Append(" DATA_YEAR,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,SALARY_YM,EMP_ID,REPAY_TYPE,REPAY_SUB_ID,UNITS,BASE_VALUE) ");
        sb.Append(" select @DATA_YEAR,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID,");
        sb.Append(" @SALARY_YM,t1.EMP_ID,'3' as REPAY_TYPE,t1.WORK_SHIFT_ALLOWANCE_TYPE as REPAY_SUB_ID,t1.TOTAL_DAYS,SUBSTRING(d.CODE_VAL1,0,charindex('+',d.CODE_VAL1))  ");
        sb.Append(" from TB_S_M_WORK_SHIFT_ALLOWANCE_D t1");
        sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='WORK_SHIFT_ALLOWANCE_TYPE' and  t1.WORK_SHIFT_ALLOWANCE_TYPE = d.SUB_CD");
        sb.Append(" where t1.SALARY_DT = @SALARY_DT and t1.EMP_ID in (SELECT EMP_ID from TB_S_M_HIRING_SALARY_MEM_D where DATA_YEAR = @DATA_YEAR)");
        sb.Append(" UNION ALL");
        sb.Append(" select @DATA_YEAR,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID,");
        sb.Append(" @SALARY_YM,t1.EMP_ID,t1.REPAY_TYPE,t1.REPAY_SUB_ID as REPAY_SUB_ID,t1.UNITS,t1.BASE_VALUE");
        sb.Append(" from TB_S_M_OVERTIME_REPAY t1");
        sb.Append(" where t1.REPAY_TYPE='3' and t1.REPAY_DT >= @SALARY_SDT and t1.REPAY_DT <= @SALARY_EDT  and t1.PROCESS_STATUS ='Y'");
        sb.Append(" and t1.EMP_ID in (SELECT EMP_ID from TB_S_M_HIRING_SALARY_MEM_D where DATA_YEAR = @DATA_YEAR)");
        
        ht.Add("@CREATED_BY", CREATED_BY);
        ht.Add("@UPDATED_BY", UPDATED_BY);
        ht.Add("@FUNC_ID", FUNC_ID);
        ht.Add("@SALARY_YM", SALARY_YM);
        ht.Add("@SALARY_DT", SALARY_DT);
        ht.Add("@DATA_YEAR", DATA_YEAR);
        ht.Add("@SALARY_SDT", SALARY_SDT);
        ht.Add("@SALARY_EDT", SALARY_EDT);
        dbConn.ExecuteT(sb, ht, true);

    }

    public void insert_HIRING4()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        

        sb.Append(" insert into TB_S_M_HIRING_SALARY_REPAY_MEM (");
        sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,");
        sb.Append(" DATA_YEAR,SALARY_YM,EMP_ID,ABILITY_REPAY,NO_TAX_OVERTIME_REPAY,TAX_OVERTIME_REPAY,LEAVE_REPAY,WORK_SHIFT_REPAY) ");
        sb.Append(" select @CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID,");
        sb.Append("  z.DATA_YEAR,z.SAYARY_YM,z.EMP_ID,ROUND(ISNULL(z.ABILITY_REPAY,0),0),ROUND(ISNULL(z.HOURLY_WAGE_GAP*notax_hourbase,0),0) as over_amt_notax");
        sb.Append(" ,ROUND(ISNULL(z.HOURLY_WAGE_GAP*z.tax_hourbase,0),0) as over_amt_tax,ROUND(ISNULL(z.HOURLY_WAGE_GAP*LEAVE_HOURBASE,0),0) as LEAVE_HOURAMT");
        sb.Append(" ,ROUND(ISNULL(z.HOURLY_WAGE_GAP*z.WORK_SHIFT_HOURBASE,0),0) as WORK_SHIFT_HOURBASE");
        sb.Append(" from(");
        sb.Append(" select t1.DATA_YEAR,@SALARY_YM as SAYARY_YM ,t1.EMP_ID ,t2.HOURLY_WAGE as HOURLY_WAGE_B , /*原時薪*/");
        sb.Append("  (t1.ABILITY_PAY_A - t1.ABILITY_PAY_B )*(t2.WORK_DAYS_MONTH / (DATEDIFF(D, @SALARY_SDT, @SALARY_EDT) + 1 ) ) as ABILITY_REPAY,   /*職能俸追溯 = 新舊職能俸差 * 當月在職比例*/");
        sb.Append(" Round( (t1.ABILITY_PAY_A - t1.ABILITY_PAY_B )/240,5) as HOURLY_WAGE_GAP  /*時薪差*/,e1.notax_hourbase ,e2.tax_hourbase,e3.LEAVE_HOURBASE");
        sb.Append("  ,e4.WORK_SHIFT_HOURBASE");
        sb.Append(" from TB_S_M_HIRING_SALARY_MEM_D t1");
        sb.Append(" left join TB_S_M_EMP_RESULT t2 on t2.SALARY_DT = @SALARY_DT  and t1.EMP_ID = t2.EMP_ID");
        sb.Append(" left join (select y1.data_year,y1.salary_ym,y1.emp_id,sum(y1.UNITS*y1.BASE_VALUE) AS notax_hourbase from TB_S_M_HIRING_SALARY_REPAY_D y1");
        sb.Append("            left join TB_9_M_COMM_D y2 on y2.sys_cd='SC' and y2.main_cd='OVERTIME_PAY_TYPE' and y2.sub_cd=y1.REPAY_SUB_ID");
        sb.Append("            where y2.code_val2<>'1013' and y1.REPAY_TYPE='2' and y1.data_year=@DATA_YEAR and y1.salary_ym=@SALARY_YM");
        sb.Append("            group by y1.data_year,y1.salary_ym,y1.emp_id ) e1 on e1.data_year=t1.data_year and t1.emp_id=e1.emp_id");
        sb.Append(" left join (select y3.data_year,y3.salary_ym,y3.emp_id,sum(y3.UNITS*y3.BASE_VALUE) AS tax_hourbase from TB_S_M_HIRING_SALARY_REPAY_D y3");
        sb.Append(" left join TB_9_M_COMM_D y4 on y4.sys_cd='SC' and y4.main_cd='OVERTIME_PAY_TYPE' and y4.sub_cd=y3.REPAY_SUB_ID");
        sb.Append(" where y4.code_val2='1013' and y3.REPAY_TYPE='2' and y3.data_year=@DATA_YEAR and y3.salary_ym=@SALARY_YM");
        sb.Append(" group by y3.data_year,y3.salary_ym,y3.emp_id ) e2 on e2.data_year=t1.data_year and t1.emp_id=e2.emp_id");
        sb.Append(" left join (select y5.data_year,y5.salary_ym,y5.emp_id,sum(y5.UNITS*y5.BASE_VALUE) as LEAVE_HOURBASE from TB_S_M_HIRING_SALARY_REPAY_D y5  where y5.REPAY_TYPE='1' and y5.data_year=@DATA_YEAR and y5.salary_ym=@SALARY_YM group by y5.data_year,y5.salary_ym,y5.emp_id ) e3 on e3.data_year=t1.data_year and t1.emp_id=e3.emp_id");
        sb.Append(" left join (select y6.data_year,y6.salary_ym,y6.emp_id,sum(y6.UNITS*y6.BASE_VALUE) as WORK_SHIFT_HOURBASE from TB_S_M_HIRING_SALARY_REPAY_D y6  where y6.REPAY_TYPE='3' and y6.data_year=@DATA_YEAR and y6.salary_ym=@SALARY_YM group by y6.data_year,y6.salary_ym,y6.emp_id) e4 on e4.data_year=t1.data_year and t1.emp_id=e4.emp_id");
        sb.Append(" where t1.DATA_YEAR = @DATA_YEAR ");
        sb.Append("  ) z");        


        ht.Add("@CREATED_BY", CREATED_BY);
        ht.Add("@UPDATED_BY", UPDATED_BY);
        ht.Add("@FUNC_ID", FUNC_ID);
        ht.Add("@SALARY_YM", SALARY_YM);
        ht.Add("@SALARY_DT", SALARY_DT);
        ht.Add("@DATA_YEAR", DATA_YEAR);
        ht.Add("@SALARY_SDT", SALARY_SDT);
        ht.Add("@SALARY_EDT", SALARY_EDT);

        dbConn.ExecuteT(sb, ht, true);

    }

    public void insert_Gen1()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();        
        
        sb.Append(" insert into TB_S_M_SUBSIDY_DEDUCTIONS_1 (");
        sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,");
        sb.Append(" DATA_YM,EMP_ID,EMP_NAME,SALARY_ID,SEQ_NO,");
        sb.Append(" AMOUNT,IS_PLUS,IS_TAX,REMARK ) ");
        sb.Append("  select @CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID,(select left(CONVERT(varchar,DATEADD(MM,1,CONVERT(date,dbo.FN_S_SALARY_YM()+'01')),112),6)), ");
        //sb.Append(" left(convert(varchar,getdate(),112),6),"); 
        sb.Append(" z.EMP_ID,z.EMP_NAME,'2001',");
        sb.Append(" (select isnull(max(SEQ_NO),1) from TB_S_M_SUBSIDY_DEDUCTIONS_1  where SALARY_ID = '2001' ");
        sb.Append(" and  DATA_YM = left(convert(varchar,getdate(),112),6) and EMP_ID = z.EMP_ID),");
        sb.Append(" z.ABILITY_REPAY,z.IS_PLUS,z.IS_TAX,'2014FB2SA150-初任薪追溯職能俸補發' ");
        sb.Append(" from ( ");
        sb.Append(" select a.EMP_ID,b.EMP_NAME,SUM(a.ABILITY_REPAY) ABILITY_REPAY,d.IS_PLUS,d.IS_TAX");        
        sb.Append(" from  TB_S_M_HIRING_SALARY_REPAY_MEM a");
        sb.Append(" left join TB_H_M_EMP b on a.EMP_ID = b.EMP_ID");       
        sb.Append(" left join TB_S_M_SALARY_ITEM d on d.SALARY_ID = '2001'");
        sb.Append(" where a.ABILITY_REPAY > 0 and a.DATA_YEAR = @DATA_YEAR group by a.EMP_ID,b.EMP_NAME,d.IS_PLUS,d.IS_TAX");
        sb.Append(" )z");

        ht.Add("@CREATED_BY", CREATED_BY);
        ht.Add("@UPDATED_BY", UPDATED_BY);
        ht.Add("@FUNC_ID", FUNC_ID);        
        ht.Add("@DATA_YEAR", DATA_YEAR);

        dbConn.ExecuteT(sb, ht, true);

    }
    public void insert_Gen2()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" insert into TB_S_M_SUBSIDY_DEDUCTIONS_1 (");
        sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,");
        sb.Append(" DATA_YM,EMP_ID,EMP_NAME,SALARY_ID,SEQ_NO,");
        sb.Append(" AMOUNT,IS_PLUS,IS_TAX,REMARK ) ");
        sb.Append("  select @CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID,(select left(CONVERT(varchar,DATEADD(MM,1,CONVERT(date,dbo.FN_S_SALARY_YM()+'01')),112),6)), ");
        //sb.Append(" left(convert(varchar,getdate(),112),6),"); 
        sb.Append(" z.EMP_ID,z.EMP_NAME,'2014',");
        sb.Append(" (select isnull(max(SEQ_NO),1) from TB_S_M_SUBSIDY_DEDUCTIONS_1  where SALARY_ID = '2014' ");
        sb.Append(" and  DATA_YM = left(convert(varchar,getdate(),112),6) and EMP_ID = z.EMP_ID),");
        sb.Append(" z.NO_TAX_OVERTIME_REPAY,z.IS_PLUS,z.IS_TAX,'2014FB2SA150-初任薪追溯免稅加班費補發' ");
        sb.Append(" from ( ");
        sb.Append(" select a.EMP_ID,b.EMP_NAME,SUM(a.NO_TAX_OVERTIME_REPAY) NO_TAX_OVERTIME_REPAY,d.IS_PLUS,d.IS_TAX");
        sb.Append(" from  TB_S_M_HIRING_SALARY_REPAY_MEM a");
        sb.Append(" left join TB_H_M_EMP b on a.EMP_ID = b.EMP_ID");
        sb.Append(" left join TB_S_M_SALARY_ITEM d on d.SALARY_ID = '2014'");
        sb.Append(" where a.NO_TAX_OVERTIME_REPAY > 0 and a.DATA_YEAR = @DATA_YEAR group by a.EMP_ID,b.EMP_NAME,d.IS_PLUS,d.IS_TAX");
        sb.Append(" )z");

        ht.Add("@CREATED_BY", CREATED_BY);
        ht.Add("@UPDATED_BY", UPDATED_BY);
        ht.Add("@FUNC_ID", FUNC_ID);
        ht.Add("@DATA_YEAR", DATA_YEAR);

        dbConn.ExecuteT(sb, ht, true);

    }
    public void insert_Gen3()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" insert into TB_S_M_SUBSIDY_DEDUCTIONS_1 (");
        sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,");
        sb.Append(" DATA_YM,EMP_ID,EMP_NAME,SALARY_ID,SEQ_NO,");
        sb.Append(" AMOUNT,IS_PLUS,IS_TAX,REMARK ) ");
        sb.Append("  select @CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID,(select left(CONVERT(varchar,DATEADD(MM,1,CONVERT(date,dbo.FN_S_SALARY_YM()+'01')),112),6) ) ,");
        //sb.Append(" left(convert(varchar,getdate(),112),6),"); 
        sb.Append(" z.EMP_ID,z.EMP_NAME,'2013',");
        sb.Append(" (select isnull(max(SEQ_NO),1) from TB_S_M_SUBSIDY_DEDUCTIONS_1  where SALARY_ID = '2013' ");
        sb.Append(" and  DATA_YM = left(convert(varchar,getdate(),112),6) and EMP_ID = z.EMP_ID),");
        sb.Append(" z.TAX_OVERTIME_REPAY,z.IS_PLUS,z.IS_TAX,'2014FB2SA150-初任薪追溯應稅加班費補發' ");
        sb.Append(" from ( ");
        sb.Append(" select a.EMP_ID,b.EMP_NAME,SUM(a.TAX_OVERTIME_REPAY) TAX_OVERTIME_REPAY,d.IS_PLUS,d.IS_TAX");
        sb.Append(" from  TB_S_M_HIRING_SALARY_REPAY_MEM a");
        sb.Append(" left join TB_H_M_EMP b on a.EMP_ID = b.EMP_ID");
        sb.Append(" left join TB_S_M_SALARY_ITEM d on d.SALARY_ID = '2013'");
        sb.Append(" where a.TAX_OVERTIME_REPAY > 0 and a.DATA_YEAR = @DATA_YEAR group by a.EMP_ID,b.EMP_NAME,d.IS_PLUS,d.IS_TAX");
        sb.Append(" )z");

        ht.Add("@CREATED_BY", CREATED_BY);
        ht.Add("@UPDATED_BY", UPDATED_BY);
        ht.Add("@FUNC_ID", FUNC_ID);
        ht.Add("@DATA_YEAR", DATA_YEAR);

        dbConn.ExecuteT(sb, ht, true);

    }
    public void insert_Gen4()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" insert into TB_S_M_SUBSIDY_DEDUCTIONS_1 (");
        sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,");
        sb.Append(" DATA_YM,EMP_ID,EMP_NAME,SALARY_ID,SEQ_NO,");
        sb.Append(" AMOUNT,IS_PLUS,IS_TAX,REMARK ) ");
        sb.Append("  select @CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID,(select  left(CONVERT(varchar,DATEADD(MM,1,CONVERT(date,dbo.FN_S_SALARY_YM()+'01')),112),6) ),");
        //sb.Append(" left(convert(varchar,getdate(),112),6),");        
        sb.Append(" z.EMP_ID,z.EMP_NAME,'3017',");
        sb.Append(" (select isnull(max(SEQ_NO),1) from TB_S_M_SUBSIDY_DEDUCTIONS_1  where SALARY_ID = '3017' ");
        sb.Append(" and  DATA_YM = left(convert(varchar,getdate(),112),6) and EMP_ID = z.EMP_ID),");
        sb.Append(" z.LEAVE_REPAY,z.IS_PLUS,z.IS_TAX,'2014FB2SA150-初任薪追溯應稅加班費補發' ");
        sb.Append(" from ( ");
        sb.Append(" select a.EMP_ID,b.EMP_NAME,SUM(a.LEAVE_REPAY) LEAVE_REPAY,d.IS_PLUS,d.IS_TAX");
        sb.Append(" from  TB_S_M_HIRING_SALARY_REPAY_MEM a");
        sb.Append(" left join TB_H_M_EMP b on a.EMP_ID = b.EMP_ID");
        sb.Append(" left join TB_S_M_SALARY_ITEM d on d.SALARY_ID = '3017'");
        sb.Append(" where a.LEAVE_REPAY > 0 and a.DATA_YEAR = @DATA_YEAR group by a.EMP_ID,b.EMP_NAME,d.IS_PLUS,d.IS_TAX");
        sb.Append(" )z");

        ht.Add("@CREATED_BY", CREATED_BY);
        ht.Add("@UPDATED_BY", UPDATED_BY);
        ht.Add("@FUNC_ID", FUNC_ID);
        ht.Add("@DATA_YEAR", DATA_YEAR);

        dbConn.ExecuteT(sb, ht, true);

    }
    public void insert_Gen5()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" insert into TB_S_M_SUBSIDY_DEDUCTIONS_1 (");
        sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,");
        sb.Append(" DATA_YM,EMP_ID,EMP_NAME,SALARY_ID,SEQ_NO,");
        sb.Append(" AMOUNT,IS_PLUS,IS_TAX,REMARK ) ");
        sb.Append("  select @CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID,(select left(CONVERT(varchar,DATEADD(MM,1,CONVERT(date,dbo.FN_S_SALARY_YM()+'01')),112),6) ), ");
        //sb.Append(" left(convert(varchar,getdate(),112),6),");
        sb.Append(" z.EMP_ID,z.EMP_NAME,'2009',");
        sb.Append(" (select isnull(max(SEQ_NO),1) from TB_S_M_SUBSIDY_DEDUCTIONS_1  where SALARY_ID = '2009' ");
        sb.Append(" and  DATA_YM = left(convert(varchar,getdate(),112),6) and EMP_ID = z.EMP_ID),");
        sb.Append(" z.WORK_SHIFT_REPAY,z.IS_PLUS,z.IS_TAX,'2014FB2SA150-初任薪追溯應稅加班費補發' ");
        sb.Append(" from ( ");
        sb.Append(" select a.EMP_ID,b.EMP_NAME,SUM(a.WORK_SHIFT_REPAY) WORK_SHIFT_REPAY,d.IS_PLUS,d.IS_TAX");
        sb.Append(" from  TB_S_M_HIRING_SALARY_REPAY_MEM a");
        sb.Append(" left join TB_H_M_EMP b on a.EMP_ID = b.EMP_ID");
        sb.Append(" left join TB_S_M_SALARY_ITEM d on d.SALARY_ID = '2009'");
        sb.Append(" where a.WORK_SHIFT_REPAY > 0 and a.DATA_YEAR = @DATA_YEAR group by a.EMP_ID,b.EMP_NAME,d.IS_PLUS,d.IS_TAX");
        sb.Append(" )z");

        ht.Add("@CREATED_BY", CREATED_BY);
        ht.Add("@UPDATED_BY", UPDATED_BY);
        ht.Add("@FUNC_ID", FUNC_ID);
        ht.Add("@DATA_YEAR", DATA_YEAR);

        dbConn.ExecuteT(sb, ht, true);

    }
    public void update_HIRING_SALARY_TMP_H()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" update TB_S_M_HIRING_SALARY_TMP_H ");
        sb.Append(" set IS_SUBSIDY = 'Y',UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
        sb.Append(" where DATA_YEAR = @DATA_YEAR");   

        
        ht.Add("@UPDATED_BY", UPDATED_BY);
        ht.Add("@FUNC_ID", FUNC_ID);
        ht.Add("@DATA_YEAR", DATA_YEAR);

        dbConn.ExecuteT(sb, ht, true);

    }
    public void update_HIRING_SALARY_TMP_H_exec()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" update TB_S_M_HIRING_SALARY_TMP_H ");
        sb.Append(" set IS_GENERATE_REPAY = 'Y',UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
        sb.Append(" where DATA_YEAR = @DATA_YEAR");


        ht.Add("@UPDATED_BY", UPDATED_BY);
        ht.Add("@FUNC_ID", FUNC_ID);
        ht.Add("@DATA_YEAR", DATA_YEAR);

        dbConn.ExecuteT(sb, ht, true);

    }
    public DataTable select_IS_GENERATE_REPAY()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select IS_GENERATE_REPAY From TB_S_M_HIRING_SALARY_TMP_H");
        sb.Append(" where DATA_YEAR=@DATA_YEAR");
        ht.Add("@DATA_YEAR", DATA_YEAR);
        return dbConn.Query(sb, ht);

    }
    public DataTable select_SALARY_STATUS()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select distinct(SALARY_STATUS) From TB_S_M_SUBSIDY_DEDUCTIONS_1");
        sb.Append(" where DATA_YM= (select  left(CONVERT(varchar,DATEADD(MM,1,CONVERT(date,dbo.FN_S_SALARY_YM()+'01')),112),6) ) ");
        sb.Append(" and REMARK like @REMARK ");
                
        ht.Add("@REMARK", DATA_YEAR + "FB2SA150%");
        return dbConn.Query(sb, ht);

    }
    public DataTable emp(string EMP_ID)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select EMP_ID,EMP_NAME,LICENSE_ID From TB_H_M_EMP");
        sb.Append(" where EMP_ID=@EMP_ID");
        ht.Add("@EMP_ID", EMP_ID);
        return dbConn.Query(sb, ht);

    }
    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string data_year)
    {
        try
        {
            if (sortExpression.Contains("DATA_YEAR"))
                sortExpression = sortExpression.Replace("DATA_YEAR", "t.DATA_YEAR");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" t.DATA_YEAR,t.PROCESS_STATUS+'-'+d.SUB_DESC as PROCESS_STATUS,t.START_DT,t.END_DT,t.APPROVE_DT,t.RELEASE_BY");
            sb.Append(" ,t.MEM_CREATE_DT,t.MEM_CREATE_BY ,COUNT(t2.EMP_ID) as DATA_CNT,a.EMP_NAME,t.MEM_CREATE_BY+'-'+a.EMP_NAME as DESC1 ");
            sb.Append(" from TB_S_M_HIRING_SALARY_TMP_H t");
            sb.Append(" left join TB_S_M_HIRING_SALARY_MEM_D t2  on t.DATA_YEAR = t2.DATA_YEAR");  
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD='SA' and d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on  p.SYS_CD='SA' and  p.MAIN_CD='APPROVE_STATUS' and  t.APPROVE_STATUS = p.SUB_CD");
            sb.Append(" left join TB_H_M_EMP a on  a.EMP_ID=t.MEM_CREATE_BY");
            sb.Append(" where 1=1 and t.PROCESS_STATUS ='Y'");
            if (data_year.Trim() != "")
            {
                sb.Append("and t.DATA_YEAR = @data_year ");
                ht.Add("@data_year", data_year);

            }
            sb.Append(" GROUP BY t.DATA_YEAR,t.PROCESS_STATUS,d.SUB_DESC ,t.START_DT,t.END_DT");
            sb.Append(" ,t.APPROVE_DT,t.MEM_CREATE_DT,t.MEM_CREATE_BY,t.RELEASE_BY,a.EMP_NAME");

            

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int GetCount(int startRowIndex, int maximumRows, string data_year)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record");
            sb.Append(" from TB_S_M_HIRING_SALARY_TMP_H t");
            sb.Append(" where 1=1 and t.PROCESS_STATUS ='Y'");
            
            if (data_year.Trim() != "")
            {
                sb.Append("and t.DATA_YEAR = @data_year ");
                ht.Add("@data_year", data_year);

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
    #region 異動對象生成
    //檢查有無資料可新增
    public DataTable Execute_Check_TB_S_M_HIRING_SALARY_MEM_D(string DATA_YEAR)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select distinct t.EMP_ID,t.EMP_NAME,t.EMP_CD,t.LEVEL_CD,t.GRADE_CD,t.WS_CD,t.PJOB_CD,t.JOIN_DT,t.BE_EMP_DT,							");
            sb.AppendLine("                t.DEPT_NO,vm.DEPT_NAME_20,vm.DEPT_NAME_30,vm.DEPT_NAME_40, 'N' as CHG_STATUS,s1.AMOUNT as ABILITY_PAY_B,				");
            sb.AppendLine("                dbo.FN_S_ABILITY_PAY(t.WS_CD, e.EDUCATION_CD,e.GRADUATION_YEAR,t.LEVEL_CD,t.GRADE_CD,t.SEX_CD,t.ARMY_CD) as ABILITY_PAY_A	");																																																																												
            sb.AppendLine("from TB_H_M_EMP t 																																");
            sb.AppendLine("left join TB_S_M_HIRING_SALARY_TMP_H h on h.DATA_YEAR = @DATA_YEAR 																				");																																																								
            sb.AppendLine("left join TB_S_M_HIRING_SALARY_TMP_D h2 on  h.DATA_YEAR = h2.DATA_YEAR 																			");																																																										
            sb.AppendLine("left join VW_H_EMP_DATA vm on t.EMP_ID = vm.EMP_ID and vm.EMP_STATUS = '01'																		");																																																											
            sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and  d.MAIN_CD='WS_CD' and  t.WS_CD = d.SUB_CD 														");																																																													
            sb.AppendLine("left join TB_S_M_SALARY_TXN s1 on t.EMP_ID = s1.EMP_ID and s1.SALARY_ID='1001' and convert(varchar(8), s1.EFFECT_EDT,112)  = '99991231'			");																																																																										
            sb.AppendLine("left join TB_H_M_EMP_EDUCATION e on t.EMP_ID = e.EMP_ID and e.IS_SALARY_SCHOOL ='Y' 																");																																																													
            sb.AppendLine("left join TB_9_M_COMM_D m on  m.MAIN_CD='EMP_STATUS' and m.SYS_CD ='HB' and  vm.EMP_STATUS = m.SUB_CD                                      		");																																																																											
            sb.AppendLine("where 1=1 and  h.PROCESS_STATUS =  'Y' and t.EMP_CD ='1' 																						");																																																							
            sb.AppendLine("          and ((t.JOIN_DT >= ");
            sb.AppendLine("                     (select @DATA_YEAR + CODE_VAL1 from TB_9_M_PARAMETER where SYS_CD='SA' and MAIN_CD = 'HIRING_SALARY_MEM_SDT' )");
            sb.AppendLine(" and  t.JOIN_DT <=h.APPROVE_DT ) or (t.BE_EMP_DT >=");
            sb.AppendLine("                     (select @DATA_YEAR + CODE_VAL1 from TB_9_M_PARAMETER where SYS_CD='SA' and MAIN_CD = 'HIRING_SALARY_MEM_SDT' )");
            sb.AppendLine(" and t.JOIN_DT <=h.APPROVE_DT  ) )  			");
            sb.AppendLine("          and t.LEVEL_CD+t.GRADE_CD+t.WS_CD  in(select distinct LEVEL_CD+GRADE_CD+WS_CD from TB_S_M_HIRING_SALARY_TMP_D where DATA_YEAR =@DATA_YEAR)");	
            sb.AppendLine("order By t.EMP_ID                                                                                                                                ");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //新增 初任薪敘薪對象異動明細檔(TB_S_M_HIRING_SALARY_MEM_D)
    internal void Execute_Add_TB_S_M_HIRING_SALARY_MEM_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("insert into TB_S_M_HIRING_SALARY_MEM_D (DATA_YEAR,EMP_ID,EMP_NAME,EMP_CD,LEVEL_CD,GRADE_CD,WS_CD,PJOB_CD,JOIN_DT,BE_EMP_DT,DEPT_NO,DEPT_NAME_20");
            sb.AppendLine("            ,DEPT_NAME_30,DEPT_NAME_40,ABILITY_PAY_B,ABILITY_PAY_A,CHG_STATUS,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine("select distinct @DATA_YEAR as DATA_YEAR, t.EMP_ID,t.EMP_NAME,t.EMP_CD,t.LEVEL_CD,t.GRADE_CD,t.WS_CD,t.PJOB_CD,t.JOIN_DT,t.BE_EMP_DT,			  ");
            sb.AppendLine("                t.DEPT_NO,vm.DEPT_NAME_20,vm.DEPT_NAME_30,vm.DEPT_NAME_40, isnull(s1.LEVEL_PAY_OLD,0) as ABILITY_PAY_B,				");
            sb.AppendLine("                dbo.FN_S_ABILITY_PAY(t.WS_CD, e.EDUCATION_CD,e.GRADUATION_YEAR,t.LEVEL_CD,t.GRADE_CD,t.SEX_CD,t.ARMY_CD) as ABILITY_PAY_A,'N' as CHG_STATUS,	");
            sb.AppendLine("                @CREATED_BY as CREATED_BY,GETDATE() as CREATED_DT,@UPDATED_BY as UPDATED_BY,GETDATE() as UPDATED_DT,@FUNC_ID as FUNC_ID	");
            sb.AppendLine("from TB_H_M_EMP t 																																");
            sb.AppendLine("left join TB_S_M_HIRING_SALARY_TMP_H h on h.DATA_YEAR = @DATA_YEAR 																				");
            sb.AppendLine("left join TB_S_M_HIRING_SALARY_TMP_D h2 on  h.DATA_YEAR = h2.DATA_YEAR 																			");
            sb.AppendLine("left join VW_H_EMP_DATA vm on t.EMP_ID = vm.EMP_ID and vm.EMP_STATUS = '01'																		");
            sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and  d.MAIN_CD='WS_CD' and  t.WS_CD = d.SUB_CD 														");

            sb.AppendLine("left join TB_S_M_SALARY_ADJ_D2 s1 on substring(s1.EFFECT_YM,1,4) = @DATA_YEAR and s1.EMP_ID = t.EMP_ID ");
            //sb.AppendLine("left join TB_S_M_SALARY_TXN s1 on t.EMP_ID = s1.EMP_ID and s1.SALARY_ID='1001' and convert(varchar(8), s1.EFFECT_EDT,112)  = '99991231'			");
            sb.AppendLine("left join TB_H_M_EMP_EDUCATION e on t.EMP_ID = e.EMP_ID and e.IS_SALARY_SCHOOL ='Y' 																");
            sb.AppendLine("left join TB_9_M_COMM_D m on  m.MAIN_CD='EMP_STATUS' and m.SYS_CD ='HB' and  vm.EMP_STATUS = m.SUB_CD                                      		");
            sb.AppendLine("where 1=1 and  h.PROCESS_STATUS =  'Y' and t.EMP_CD ='1' 																						");
            sb.AppendLine("          and ((t.JOIN_DT >= ");
            sb.AppendLine("                     (select @DATA_YEAR + CODE_VAL1 from TB_9_M_PARAMETER where SYS_CD='SA' and MAIN_CD = 'HIRING_SALARY_MEM_SDT' )");
            sb.AppendLine(" and  t.JOIN_DT <=h.APPROVE_DT ) or (t.BE_EMP_DT >=");
            sb.AppendLine("                     (select @DATA_YEAR + CODE_VAL1 from TB_9_M_PARAMETER where SYS_CD='SA' and MAIN_CD = 'HIRING_SALARY_MEM_SDT' )");
            sb.AppendLine(" and t.JOIN_DT <=h.APPROVE_DT  ) )  			");
            sb.AppendLine("          and t.LEVEL_CD+t.GRADE_CD+t.WS_CD  in(select distinct LEVEL_CD+GRADE_CD+WS_CD from TB_S_M_HIRING_SALARY_TMP_D where DATA_YEAR =@DATA_YEAR)");
            sb.AppendLine("order By t.EMP_ID                                                                                                                                ");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SA150");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取TB_S_M_SALARY_TX的EFFECT_SDT
    public DataTable Execute_Get_TB_S_M_SALARY_TX_EFFECT_SDT()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select t.EMP_ID,t.SALARY_ID,t.AMOUNT,CASE WHEN d.BE_EMP_DT is not null then d.BE_EMP_DT else d.JOIN_DT end as EFFECT_SDT  ,t.EFFECT_EDT");
            sb.AppendLine("      ,(select CONVERT(varchar(8), @DATA_YEAR+p.CODE_VAL1, 112)                                                                            ");
            sb.AppendLine("        from TB_9_M_PARAMETER p                                                                                                        ");
            sb.AppendLine("        where p.SYS_CD='SA' and  p.MAIN_CD='HIRING_SALARY_CHG_SDT') as DATA_YEAR_0701                                                  ");
            sb.AppendLine("from TB_S_M_SALARY_TXN t                                                                                                               ");
            sb.AppendLine("inner join TB_S_M_HIRING_SALARY_MEM_D d on  d.DATA_YEAR =@DATA_YEAR and d.emp_id = t.EMP_ID                                                ");
            sb.AppendLine("left join TB_S_M_HIRING_SALARY_TMP_H h on  h.DATA_YEAR =@DATA_YEAR                                                                         ");
            sb.AppendLine("where  t.SALARY_ID = '1001'  and  t.EFFECT_EDT >=                                                                                    ");
            sb.AppendLine("    (select convert (datetime, @DATA_YEAR+p.CODE_VAL1,112)                                                                                 ");
	        sb.AppendLine("     from TB_9_M_PARAMETER p                                                                                                           ");
            sb.AppendLine("     where p.SYS_CD='SA' and  p.MAIN_CD='HIRING_SALARY_CHG_SDT' )                                                                      ");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            return dbConn.QueryT(sb, ht,true);
        }
        catch
        {
            throw;
        }
    }

    //更新 敘薪資料檔(TB_S_M_SALARY_TXN) 
    internal void Execute_Update_TB_S_M_SALARY_TXN(string EFFECT_SDT, string DATA_YEAR_DATE, string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_SALARY_TXN ");
            sb.AppendLine(" Set EFFECT_EDT = iif(EFFECT_SDT>=@EFFECT_SDT,EFFECT_SDT,@EFFECT_EDT),UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where EMP_ID = @EMP_ID and SALARY_ID = @SALARY_ID and EFFECT_EDT='9999/12/31'");  //BY EVA MARK 2015/07/29

            ht.Add("@EFFECT_SDT", EFFECT_SDT);
            ht.Add("@EFFECT_EDT", DATA_YEAR_DATE);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SA150");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", "1001");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增 敘薪資料檔(TB_S_M_SALARY_TXN) 
    internal void Execute_Add_TB_S_M_SALARY_TXN(string EFFECT_SDT, string DATA_YEAR_0701,string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("insert into TB_S_M_SALARY_TXN(EMP_ID,SALARY_ID,AMOUNT,EFFECT_SDT,EFFECT_EDT,SEQ_NO,APPROVE_BY,APPROVE_DT,REMARK");
            sb.AppendLine("                              ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine("select distinct t.EMP_ID,'1001' as SALARY_ID,t2.ABILITY_PAY_A");
            sb.AppendLine("                ,@DATA_YEAR_0701 as EFFECT_SDT ,'99991231' as EFFECT_EDT");
            sb.AppendLine("                ,isnull((select MAX(SEQ_NO)+1 from TB_S_M_SALARY_TXN where EMP_ID =@EMP_ID and SALARY_ID = '1001' and EFFECT_SDT =@DATA_YEAR_0701),1) as SEQ_NO");
            sb.AppendLine("                ,@APPROVE_BY as APPROVE_BY,GETDATE() as APPROVE_DT,'初任薪敘薪對象異動' as REMARK");
            sb.AppendLine("                ,@CREATED_BY as CREATED_BY,GETDATE() as CREATED_DT,@UPDATED_BY as UPDATED_BY,GETDATE() as UPDATED_DT,@FUNC_ID as FUNC_ID");
            sb.AppendLine("from TB_S_M_SALARY_TXN t ,TB_S_M_HIRING_SALARY_MEM_D t2");
            sb.AppendLine("where t.EMP_ID =@EMP_ID and t.SALARY_ID = '1001' and t.EFFECT_SDT =@EFFECT_SDT");
            sb.AppendLine("and t2.EMP_ID =@EMP_ID and t2.DATA_YEAR=@DATA_YEAR");

            ht.Add("@DATA_YEAR_0701", DATA_YEAR_0701);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EFFECT_SDT", EFFECT_SDT);
            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@APPROVE_BY", SessionHandle.Current.emp_id);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SA150");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新 初任薪試算主檔(TB_S_M_HIRING_SALARY_TMP_H)
    internal void Execute_Update_TB_S_M_HIRING_SALARY_TMP_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_HIRING_SALARY_TMP_H ");
            sb.AppendLine(" Set MEM_CREATE_BY = @MEM_CREATE_BY,MEM_CREATE_DT = GETDATE(),UPDATED_BY=@UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where DATA_YEAR = @DATA_YEAR");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@MEM_CREATE_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SA150");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    public DataTable GetDtlData2(int startRowIndex, int maximumRows, string sortExpression, string join_sdt, string join_edt, string emp_id, string emp_name, string data_year)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "t.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "t.EMP_NAME");
            if (sortExpression.Contains("EMP_CD"))
                sortExpression = sortExpression.Replace("EMP_CD", "t.EMP_CD");
            if (sortExpression.Contains("LEVEL_CD"))
                sortExpression = sortExpression.Replace("LEVEL_CD", "t.LEVEL_CD");
            if (sortExpression.Contains("GRADE_CD"))
                sortExpression = sortExpression.Replace("GRADE_CD", "t.GRADE_CD");
            if (sortExpression.Contains("DESC2"))
                sortExpression = sortExpression.Replace("DESC2", "t.PJOB_CD");
            if (sortExpression.Contains("DESC3"))
                sortExpression = sortExpression.Replace("DESC3", "p.EMP_STATUS");
            if (sortExpression.Contains("JOIN_DT"))
                sortExpression = sortExpression.Replace("JOIN_DT", "t.JOIN_DT");
            if (sortExpression.Contains("BE_EMP_DT"))
                sortExpression = sortExpression.Replace("BE_EMP_DT", "t.BE_EMP_DT");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.AppendLine("t.EMP_ID,t.EMP_NAME,t.EMP_CD,t.EMP_CD+'-'+ e.SUB_DESC as DESC1 ,t.LEVEL_CD,t.GRADE_CD,t.PJOB_CD,t.PJOB_CD+'-'+p.PJOB_DESC as DESC2");
            sb.AppendLine("      ,p.EMP_STATUS,p.EMP_STATUS+'-'+w.SUB_DESC as DESC3,t.JOIN_DT,t.BE_EMP_DT,t.ABILITY_PAY_B,t.ABILITY_PAY_A"); 
            sb.AppendLine("from TB_S_M_HIRING_SALARY_MEM_D t"); 
            sb.AppendLine("left join TB_9_M_COMM_D e on e.SYS_CD='HB' and  e.MAIN_CD='EMP_CD' and  t.EMP_CD = e.SUB_CD"); 
            sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and  d.MAIN_CD='WS_CD' and  t.WS_CD = d.SUB_CD"); 
            sb.AppendLine("left join VW_H_EMP_DATA p on t.EMP_ID = p.EMP_ID");
            sb.AppendLine("left join TB_9_M_COMM_D w on w.SYS_CD='HB' and  w.MAIN_CD='EMP_STATUS' and  p.EMP_STATUS = w.SUB_CD");
            sb.AppendLine("where 1=1 and  t.DATA_YEAR =@DATA_YEAR");
            ht.Add("@DATA_YEAR", data_year);

            if (join_sdt.Trim() != "")
            {
                sb.AppendLine("and t.JOIN_DT >=@JOIN_SDT");
                ht.Add("@JOIN_SDT", join_sdt);
            }
            if (join_edt.Trim() != "")
            {
                sb.AppendLine("and t.JOIN_DT <=@JOIN_EDT");
                ht.Add("@JOIN_EDT", join_edt);
            }
            if (emp_id.Trim() != "")
            {
                sb.AppendLine("and t.EMP_ID like '%'+@EMP_ID+'%' ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name.Trim() != "")
            {
                sb.AppendLine("and t.EMP_NAME like '%'+@EMP_NAME+'%' ");
                ht.Add("@EMP_NAME", emp_name);
            }


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
    public int GetDtlCount(int startRowIndex, int maximumRows, string join_sdt, string join_edt, string emp_id, string emp_name, string data_year)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record");
            sb.AppendLine("from TB_S_M_HIRING_SALARY_MEM_D t");
            sb.AppendLine("left join TB_9_M_COMM_D e on e.SYS_CD='HB' and  e.MAIN_CD='EMP_CD' and  t.EMP_CD = e.SUB_CD");
            sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and  d.MAIN_CD='WS_CD' and  t.WS_CD = d.SUB_CD");
            sb.AppendLine("left join VW_H_EMP_DATA p on t.EMP_ID = p.EMP_ID");
            sb.AppendLine("left join TB_9_M_COMM_D w on w.SYS_CD='HB' and  w.MAIN_CD='EMP_STATUS' and  p.EMP_STATUS = w.SUB_CD");
            sb.AppendLine("where 1=1 and  t.DATA_YEAR =@DATA_YEAR");
            ht.Add("@DATA_YEAR", data_year);

            if (join_sdt.Trim() != "")
            {
                sb.AppendLine("and t.JOIN_DT >=@JOIN_SDT");
                ht.Add("@JOIN_SDT", join_sdt);
            }
            if (join_edt.Trim() != "")
            {
                sb.AppendLine("and t.JOIN_DT <=@JOIN_EDT");
                ht.Add("@JOIN_EDT", join_edt);
            }
            if (emp_id.Trim() != "")
            {
                sb.AppendLine("and t.EMP_ID like '%'+@EMP_ID+'%' ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name.Trim() != "")
            {
                sb.AppendLine("and t.EMP_NAME like '%'+@EMP_NAME+'%' ");
                ht.Add("@EMP_NAME", emp_name);
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
}