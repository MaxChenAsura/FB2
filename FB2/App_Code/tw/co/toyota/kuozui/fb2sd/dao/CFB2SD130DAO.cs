using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;


/// <summary>
/// CFB2SD130BO 的摘要描述
/// </summary>
public class CFB2SD130DAO : BaseDAO
{
    public CFB2SD130DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string DATA_YM { get; set; }
    public string EMP_ID { get; set; }
    public string DEPT_NO { get; set; }
    public string JPN_CD { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_CD { get; set; }
    public string SALARY_ID { get; set; }
    public string SALARY_NAME { get; set; }
    public string CHG_AMT_A { get; set; }
    public string CHG_AMT_B { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string START_DT_B { get; set; }
    public string START_DT_A { get; set; }
    public string END_DATE_B { get; set; }
    public string END_DATE_A { get; set; }
    public string CHG_STATUS { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string REMARK { get; set; }
    public string APP_REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    public string REMIT_DT { get; set; }
    public string SALARY_TYPE { get; set; }
    public string IS_PLUS { get; set; }
    public string IS_TAX { get; set; }

    public DataTable getJPN_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HB' and MAIN_CD='JPN_CD' and IS_VALID = 'Y' ");
            return dbConn.Query(sb);

        }
        catch
        {
            throw;
        }
    }

    public DataTable get_PDF_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.EMP_ID,c.EMP_NAME,c.DEPT_NO,c.PLANT_CD,c.PJOB_CD,c.JOIN_DT,c.EMP_CD,c.WS_CD,c.WORK_YEARS,e.SUB_DESC as FESTIVAL_TYPE_NAME			");
            sb.Append(" ,sum(case when SALARY_ID<>'4001' then (a.AMOUNT*a.IS_PLUS) end ) as 節金金額			");
            sb.Append(" ,sum(case when SALARY_ID='4001' then a.AMOUNT end) as 稅額,	sum(case when SALARY_ID<>'4001' then (a.AMOUNT*a.IS_PLUS) end )-sum(case when SALARY_ID='4001' then a.AMOUNT end) as '實領金額'		");
            sb.Append(" from TB_S_M_SALARY_PAY a			");
            sb.Append(" left join (select distinct zb.FESTIVAL_TYPE,zb.FESTIVAL_DT,zb.FESTIVAL_PAY_DT,zb.SALARY_DT 			");
            sb.Append("            from TB_S_M_FESTIVAL_H zb)b on a.SALARY_DT=b.SALARY_DT			");
            sb.Append(" left join TB_S_R_FESTIVAL_D c on b.FESTIVAL_TYPE=c.FESTIVAL_TYPE and b.FESTIVAL_DT=c.FESTIVAL_DT and a.SALARY_DT=b.FESTIVAL_PAY_DT and a.EMP_ID=c.EMP_ID			");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='SG' and e.MAIN_CD='FESTIVAL_TYPE' and e.SUB_CD=c.FESTIVAL_TYPE			");
            sb.Append(" where a.COMPANY_CD='T'	");
            sb.Append(" group by a.EMP_ID,c.EMP_NAME,c.DEPT_NO,c.PLANT_CD,c.PJOB_CD,c.JOIN_DT,c.EMP_CD,c.WS_CD,c.WORK_YEARS,e.SUB_DESC ");
           

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable searchResult()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select *,(select '4') as '基本資料'");
            sb.Append(" ,(select count(SALARY_NAME) from TB_S_M_SALARY_ITEM where SALARY_CD<>'3' AND IS_PLUS=1 AND IS_TAX='Y') as '應稅加項'");
            sb.Append(" ,(select count(SALARY_NAME) from TB_S_M_SALARY_ITEM where SALARY_CD<>'3' AND IS_PLUS=-1 AND IS_TAX='Y') as '應稅減項'");
            sb.Append(" ,(select count(SALARY_NAME) from TB_S_M_SALARY_ITEM where SALARY_CD<>'3' AND IS_PLUS=1 AND IS_TAX='N') as '免稅加項'");
            sb.Append(" ,(select count(SALARY_NAME) from TB_S_M_SALARY_ITEM where SALARY_CD<>'3' AND IS_PLUS=-1 AND IS_TAX='N') as '免稅減項'");
            sb.Append(" ,(select count(SUB_DESC) from TB_9_M_COMM_D where SYS_CD='SC' and MAIN_CD='OVERTIME_PAY_TYPE' AND IS_VALID='Y') as '加班資料'");
            sb.Append(" ,(select count(SUB_DESC) from TB_9_M_COMM_D where SYS_CD='SC' and MAIN_CD='SHIFT_TIME_CD' AND IS_VALID='Y') as '勤務資料'");
            sb.Append(" ,(select count(ENV_ALLOWANCE_VALUE) from TB_D_M_ENV_ALLOWANCE_TYPE) as '環境津貼資料'");
            sb.Append(" ,(select count(SUB_LEAVE_DESC) from TB_D_M_LEAVE_TYPE_D) as '假別資料'");
            sb.Append(" ,(select count(SUB_DESC) from TB_9_M_COMM_D where SYS_CD='SC' and MAIN_CD='LEAVE_ALLOWANCE_TYPE' and IS_VALID='Y') as '剩餘假別時數'");
            sb.Append("  from(");
            sb.Append(" select '基本資料' AS '名稱','工號' AS '內容'");
            sb.Append(" union all");
            sb.Append(" select '基本資料' AS '名稱','姓名' AS '內容'");
            sb.Append(" union all");
            sb.Append(" select '基本資料' AS '名稱','月終狀態' AS '內容'");
            sb.Append(" union all");
            sb.Append(" select '基本資料' AS '名稱','本月在職天數' AS '內容'");
            sb.Append(" union all");
            sb.Append(" select '應稅加項' AS '名稱',a.SALARY_NAME AS '內容'");
            sb.Append(" from TB_S_M_SALARY_ITEM a");
            sb.Append(" where a.SALARY_CD<>'3' AND a.IS_PLUS=1 AND a.IS_TAX='Y'");
            sb.Append(" ");
            sb.Append(" union all");
            sb.Append(" ");
            sb.Append(" select '應稅減項' AS '名稱',b.SALARY_NAME AS '內容'");
            sb.Append(" from TB_S_M_SALARY_ITEM b");
            sb.Append(" where b.SALARY_CD<>'3' AND b.IS_PLUS=-1 AND b.IS_TAX='Y'");
            sb.Append(" ");
            sb.Append(" union all");
            sb.Append(" ");
            sb.Append(" select '免稅加項' AS '名稱',b.SALARY_NAME AS '內容'");
            sb.Append(" from TB_S_M_SALARY_ITEM b");
            sb.Append(" where b.SALARY_CD<>'3' AND b.IS_PLUS=1 AND b.IS_TAX='N'");
            sb.Append(" ");
            sb.Append(" union all");
            sb.Append(" ");
            sb.Append(" select '免稅減項' AS '名稱',b.SALARY_NAME AS '內容'");
            sb.Append(" from TB_S_M_SALARY_ITEM b");
            sb.Append(" where b.SALARY_CD<>'3' AND b.IS_PLUS=-1 AND b.IS_TAX='N'");
            sb.Append(" ");
            sb.Append(" union all");
            sb.Append(" ");
            sb.Append(" select '加班資料' AS '名稱',SUB_DESC AS '內容'");
            sb.Append(" from TB_9_M_COMM_D ");
            sb.Append(" where SYS_CD='SC' and MAIN_CD='OVERTIME_PAY_TYPE' AND IS_VALID='Y'");
            sb.Append(" ");
            sb.Append(" union all");
            sb.Append(" ");
            sb.Append(" select '勤務資料' AS '名稱',SUB_DESC+'天數' AS '內容'");
            sb.Append(" from TB_9_M_COMM_D ");
            sb.Append(" where SYS_CD='SC' and MAIN_CD='SHIFT_TIME_CD' AND IS_VALID='Y'");
            sb.Append(" ");
            sb.Append(" union all");
            sb.Append(" ");
            sb.Append(" select '環境津貼資料' AS '名稱',cast (ENV_ALLOWANCE_VALUE as varchar)+'元天數' AS '內容'");
            sb.Append(" from TB_D_M_ENV_ALLOWANCE_TYPE ");
            sb.Append(" ");
            sb.Append(" union all");
            sb.Append(" ");
            sb.Append(" select '假別資料' AS '名稱',SUB_LEAVE_DESC+'時數' AS '內容'");
            sb.Append(" from TB_D_M_LEAVE_TYPE_D ");
            sb.Append(" ");
            sb.Append(" union all");
            sb.Append(" ");
            sb.Append(" select '剩餘假別時數' AS '名稱',SUB_DESC+'時數' AS '內容'");
            sb.Append(" from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='SC' and MAIN_CD='LEAVE_ALLOWANCE_TYPE' and IS_VALID='Y')td");
           

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable searchResult2_1()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  select a.EMP_ID,c.EMP_NAME,c.DEPT_NO,c.PLANT_CD,c.PJOB_CD,c.JOIN_DT,c.EMP_CD,c.WS_CD,c.WORK_YEARS,e.SUB_DESC as FESTIVAL_TYPE_NAME			");
            sb.Append("  ,sum(case when SALARY_ID<>'4001' then (a.AMOUNT*a.IS_PLUS) end ) as 節金金額			");
            sb.Append("  ,sum(case when SALARY_ID='4001' then a.AMOUNT end) as 稅額,	sum(case when SALARY_ID<>'4001' then (a.AMOUNT*a.IS_PLUS) end )-sum(case when SALARY_ID='4001' then a.AMOUNT end) as '實領金額'");
            sb.Append("  from TB_S_M_SALARY_PAY a			");
            sb.Append("  left join (select distinct zb.FESTIVAL_TYPE,zb.FESTIVAL_DT,zb.FESTIVAL_PAY_DT,zb.SALARY_DT 			");
            sb.Append("             from TB_S_M_FESTIVAL_H zb)b on a.SALARY_DT=b.SALARY_DT			");
            sb.Append("  left join TB_S_R_FESTIVAL_D c on b.FESTIVAL_TYPE=c.FESTIVAL_TYPE and b.FESTIVAL_DT=c.FESTIVAL_DT and a.SALARY_DT=b.FESTIVAL_PAY_DT and a.EMP_ID=c.EMP_ID			");
            sb.Append("  left join TB_9_M_COMM_D e on e.SYS_CD='SG' and e.MAIN_CD='FESTIVAL_TYPE' and e.SUB_CD=c.FESTIVAL_TYPE			");
            sb.Append("  where a.COMPANY_CD='T'	");
            sb.Append("  group by a.EMP_ID,c.EMP_NAME,c.DEPT_NO,c.PLANT_CD,c.PJOB_CD,c.JOIN_DT,c.EMP_CD,c.WS_CD,c.WORK_YEARS,e.SUB_DESC ");
            


            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable searchResult2_2()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  select a.EMP_ID,b.EMP_NAME,a.AMOUNT	");
            sb.Append("  from TB_S_M_SALARY_PAY a	");
            sb.Append("  left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID	");
            sb.Append("  where a.SALARY_ID='1038' and a.COMPANY_CD='T'");


            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable searchResult2_3()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  	     select c.HR_CHG_DESC,a.ORI_DEPT_NO,a.ORI_DIV_DEPT_FULL_NAME,a.EMP_ID,d.EMP_NAME,b.BASIC_SALARY,a.START_DT,isnull(a.PLAN_END_DT,a.END_DT) ad,a.END_DT,	");
            sb.Append("             b.WORK_DAYS,b.BOUNS_WORK_DAYS,b.LEAVE_A_HRS,b.LEAVE_B_HRS,b.LEAVE_B_DAYS,b.LEAVE_Q_HRS,b.LEAVE_Q_DAYS,b.THIRD_CNT_REWARD,b.SECOND_CNT_REWARD			");
            sb.Append("             ,b.FIRST_CNT_REWARD,b.THIRD_CNT_PUNISH,b.SECOND_CNT_PUNISH,b.FIRST_CNT_PUNISH,b.JUDGEMENT_DAYS,b.PLAN_BONUS_DAYS,ROUND(b.PLAN_BONUS_DAYS/30,1) as 'PLAN_BONUS_DAYS_2',b.PLAN_BONUS_AMT,b.PAID_CNT			");
            sb.Append("             ,b.PAID_AMT,b.BONUS_AMT");
            sb.Append("       from TB_H_M_BONUS_PLAN_H a 			");
            sb.Append("       left join TB_H_M_BONUS_PLAN_D b on a.EMP_ID=b.EMP_ID and a.START_DT = b.START_DT	");
            sb.Append("       left join TB_H_M_HR_CHANGE_CODE c on  c.HR_CHG_CD=a.HR_CHG_CD 		");
            sb.Append("       left join VW_H_EMP_DATA d on a.EMP_ID=d.EMP_ID			");
            sb.Append("       where  a.COMPANY_CD='T'");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable searchResult2()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select c.EMP_ID,c.EMP_NAME,c.WORK_DAYS_MONTH,d.SUB_DESC as 在職區分");
            sb.Append(" from TB_S_M_SALARY_PAY_H a");
            sb.Append(" left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT ");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CHG_CD' and d.SUB_CD=c.EMP_CHG_CD");
            sb.Append("    where a.REMIT_DT =@REMIT_DT AND a.SALARY_TYPE=@SALARY_TYPE and c.COMPANY_CD='T'  ");
            ht.Add("@REMIT_DT", REMIT_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE.Substring(0, 1));

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable searchResult3()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("    select b.EMP_ID,convert(varchar,b.SALARY_ID) as 'SALARY_ID',b.AMOUNT,d.SALARY_NAME");
            sb.Append("    from TB_S_M_SALARY_PAY_H a	");
            sb.Append("    left join TB_S_M_SALARY_PAY b on a.SALARY_TYPE=b.SALARY_TYPE and a.PAY_ID=b.PAY_ID	");
            sb.Append("    left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID	");
            sb.Append("    left join TB_S_M_SALARY_ITEM d on b.SALARY_ID=d.SALARY_ID 	");
            sb.Append("    where  a.REMIT_DT =@REMIT_DT AND a.SALARY_TYPE=@SALARY_TYPE and c.COMPANY_CD='T'and d.IS_TAX='Y' and d.IS_PLUS=1  and d.SALARY_CD<>'5'");
            
            sb.Append("    union all");
            sb.Append("    select b.EMP_ID,convert(varchar,b.SALARY_ID) as 'SALARY_ID',b.AMOUNT,d.SALARY_NAME");
            sb.Append("    from TB_S_M_SALARY_PAY_H a	");
            sb.Append("    left join TB_S_M_SALARY_PAY b on a.SALARY_TYPE=b.SALARY_TYPE and a.PAY_ID=b.PAY_ID	");
            sb.Append("    left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID	");
            sb.Append("    left join TB_S_M_SALARY_ITEM d on b.SALARY_ID=d.SALARY_ID 	");
            sb.Append("    where a.REMIT_DT =@REMIT_DT AND a.SALARY_TYPE=@SALARY_TYPE and c.COMPANY_CD='T'and d.IS_TAX='Y' and d.IS_PLUS=-1  and d.SALARY_CD<>'5'");
            
            sb.Append("    union all");
            sb.Append("    select b.EMP_ID,convert(varchar,b.SALARY_ID) as 'SALARY_ID',b.AMOUNT,d.SALARY_NAME");
            sb.Append("    from TB_S_M_SALARY_PAY_H a	");
            sb.Append("    left join TB_S_M_SALARY_PAY b on a.SALARY_TYPE=b.SALARY_TYPE and a.PAY_ID=b.PAY_ID	");
            sb.Append("    left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID	");
            sb.Append("    left join TB_S_M_SALARY_ITEM d on b.SALARY_ID=d.SALARY_ID 	");
            sb.Append("    where a.REMIT_DT =@REMIT_DT AND a.SALARY_TYPE=@SALARY_TYPE and c.COMPANY_CD='T'and d.IS_TAX='N' and d.IS_PLUS=1  and d.SALARY_CD<>'5'");
            
            sb.Append("     union all");
            sb.Append("    select b.EMP_ID,convert(varchar,b.SALARY_ID) as 'SALARY_ID',b.AMOUNT,d.SALARY_NAME");
            sb.Append("    from TB_S_M_SALARY_PAY_H a	");
            sb.Append("    left join TB_S_M_SALARY_PAY b on a.SALARY_TYPE=b.SALARY_TYPE and a.PAY_ID=b.PAY_ID	");
            sb.Append("    left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID	");
            sb.Append("    left join TB_S_M_SALARY_ITEM d on b.SALARY_ID=d.SALARY_ID 	");
            sb.Append("    where a.REMIT_DT =@REMIT_DT AND a.SALARY_TYPE=@SALARY_TYPE and c.COMPANY_CD='T'and d.IS_TAX='N' and d.IS_PLUS=-1  and d.SALARY_CD<>'5'");
            
            sb.Append("    union all");
            sb.Append("    select b.EMP_ID,convert(varchar,b.OVERTIME_PAY_TYPE) as 'SALARY_ID',b.TOTAL_HOURS as 'AMOUNT',t.SUB_DESC as 'SALARY_NAME'");
            sb.Append("    from TB_S_M_SALARY_PAY_H a");
            sb.Append("    left join TB_S_M_OVERTIME_RESULT_D b on  a.SALARY_DT=b.SALARY_DT");
            sb.Append("    left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID");
            sb.Append("    left join TB_9_M_COMM_D t on b.OVERTIME_PAY_TYPE= t.SUB_CD and t.SYS_CD='SC' and t.MAIN_CD='OVERTIME_PAY_TYPE' AND t.IS_VALID='Y'");
            sb.Append("    where a.REMIT_DT =@REMIT_DT AND a.SALARY_TYPE=@SALARY_TYPE and c.COMPANY_CD='T' ");
           
            sb.Append("    union all		   ");
            sb.Append("    select a.EMP_ID,convert(varchar,a.ENV_ALLOWANCE_VALUE) as 'SALARY_ID',sum(a.APPLY_HOUR) as 'AMOUNT',cast (c.ENV_ALLOWANCE_VALUE as varchar)+'元天數' AS 'SALARY_NAME'");
            sb.Append("    from TB_D_M_ENV_ALLOWANCE_APPLY a");
            sb.Append("    left join TB_S_M_EMP_RESULT b on b.SALARY_DT=a.SALARY_DT and  a.EMP_ID=b.EMP_ID");
            sb.Append("    left join TB_D_M_ENV_ALLOWANCE_TYPE c on cast (c.ENV_ALLOWANCE_TYPE as varchar)=cast (a.ENV_ALLOWANCE_VALUE as varchar)");
            sb.Append("    where a.ENV_CHECK_STATUS='Y' and b.COMPANY_CD='T'");
            sb.Append("    GROUP BY a.EMP_ID,a.ENV_ALLOWANCE_VALUE,c.ENV_ALLOWANCE_VALUE");
            sb.Append("    union all");
            sb.Append("    select b.EMP_ID,convert(varchar,b.LEAVE_ALLOWANCE_TYPE) as 'SALARY_ID',b.TOTAL_HOURS as 'AMOUNT',d.SUB_DESC+'時數' as 'SALARY_NAME'");
            sb.Append("    from TB_S_M_SALARY_PAY_H a");
            sb.Append("    left join TB_S_M_AVAILABLE_LEAVE_D b on a.SALARY_DT=b.SALARY_DT");
            sb.Append("    left join TB_S_M_EMP_RESULT c on a.SALARY_YM=c.SALARY_YM and a.SALARY_DT=c.SALARY_DT and b.EMP_ID=c.EMP_ID");
            sb.Append("    left join TB_9_M_COMM_D d on a.PAY_ID=d.SUB_CD and d.SYS_CD='SC' and d.MAIN_CD='LEAVE_ALLOWANCE_TYPE' and d.IS_VALID='Y'");
            sb.Append("    where a.REMIT_DT =@REMIT_DT AND a.SALARY_TYPE=@SALARY_TYPE and c.COMPANY_CD='T'  ");
            
            sb.Append("    union all");
            sb.Append("    select a.EMP_ID,convert(varchar,a.SUB_LEAVE_CD) as 'SALARY_ID',a.TOTAL_HOURS as 'AMOUNT',c.SUB_LEAVE_DESC+'時數' as 'SALARY_NAME'");
            sb.Append("    from TB_S_M_LEAVE_RESULT_D a");
            sb.Append("    left join TB_S_M_EMP_RESULT b on b.SALARY_DT=a.SALARY_DT and  a.EMP_ID=b.EMP_ID");
            sb.Append("    left join TB_D_M_LEAVE_TYPE_D c on c.SUB_LEAVE_CD=a.SUB_LEAVE_CD");
            sb.Append("    where b.COMPANY_CD='T'");

            ht.Add("@REMIT_DT", REMIT_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE.Substring(0, 1));
            
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void addData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_M_SALARY_REPORT_H (");
            sb.Append(" SALARY_DT, SALARY_TYPE, DATA_YM, SALARY_ID_SEQ,");
            sb.Append(" SALARY_ID, SALARY_NAME, IS_PLUS, IS_TAX,");
            sb.Append(" CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" SELECT");
            sb.Append(" S.SALARY_DT, S.SALARY_TYPE, S.DATA_YM,");
            sb.Append(" ROW_NUMBER() over(PARTITION BY S.IS_PLUS ORDER BY S.SALARY_DT, S.SALARY_TYPE, S.DATA_YM, S.IS_PLUS, S.SALARY_ID) SALARY_ID_SEQ,");
            sb.Append(" S.SALARY_ID, S.SALARY_NAME, S.IS_PLUS, S.IS_TAX,");
            sb.Append(" @login_emp_id, GETDATE(), @login_emp_idID, GETDATE(), 'FB2SC530' FUNC_ID");
            sb.Append(" FROM(");
            sb.Append(" SELECT  SALARY_DT, SALARY_TYPE, DATA_YM, SALARY_ID, SALARY_NAME, IS_PLUS, IS_TAX, COUNT(*) CNT");
            sb.Append(" FROM TB_S_M_SALARY_PAY");
            sb.Append(" WHERE P.SALARY_DT BETWEEN @DATA_YMS AND @DATA_YME");
            sb.Append(" ");
            sb.Append(" ");
            sb.Append(" GROUP BY SALARY_DT, SALARY_TYPE, DATA_YM, SALARY_ID, SALARY_NAME, IS_PLUS, IS_TAX) S");
            sb.Append(" ORDER BY S.SALARY_DT, S.SALARY_TYPE, S.DATA_YM, S.IS_PLUS, SALARY_ID_SEQ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@DEPT_NO", DEPT_NO);
            //ht.Add("@DEPT_NAME", DEPT_NAME);
            //ht.Add("@LEVEL_CD", LEVEL_CD);
            //ht.Add("@PJOB_DESC", PJOB_DESC);
            //ht.Add("@DOC_NO", DOC_NO);
            //ht.Add("@START_DT", START_DT);
            //ht.Add("@JUDGEMENT_TYPE", JUDGEMENT_TYPE);
            //ht.Add("@REASON_CD", REASON_CD);
            //ht.Add("@FIRST_CNT", FIRST_CNT);
            //ht.Add("@SECOND_CNT", SECOND_CNT);
            //ht.Add("@THIRD_CNT", THIRD_CNT);
            //ht.Add("@IS_FIRE", IS_FIRE);
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
    public string deleteData(string login_emp_id)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" Delete From TB_S_M_SALARY_REPORT_H  ");
        sb.Append(" Where CREATED_BY = @login_emp_id");
        sb.Append(" Delete From TB_S_M_SALARY_REPORT_D  ");
        sb.Append(" Where CREATED_BY = @login_emp_id");
        ht.Add("@login_emp_id", login_emp_id);
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }

}