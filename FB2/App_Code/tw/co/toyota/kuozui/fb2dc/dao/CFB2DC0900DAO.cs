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
/// CFB2DC0900DAO 的摘要描述
/// </summary>
public class CFB2DC0900DAO : BaseDAO
{
    public CFB2DC0900DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string EMP_ID { get; set; }

    public string EMP_NAME { get; set; }

    public string CALENDAR_DT_S { get; set; }

    public string CALENDAR_DT_E { get; set; }

    public string DEPT_NO { get; set; }

    public string DEPT_NAME { get; set; }

    internal System.Data.DataTable searchResult()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append(" select a.EMP_ID,b.EMP_NAME,REPLACE(CONVERT(char(10), a.CALENDAR_DT, 120),'-','/') CALENDAR_DT,");
            sb.Append(" b.DEPT_NO+' '+b.DIV_DEPT_FULL_NAME DEPT_NAME,  ");
            sb.Append(" b.DEPT_NO,b.DEPT_NAME_20,b.DEPT_NAME_30,b.DEPT_NAME_40,  ");
            sb.Append(" REPLACE(CONVERT(char(16), a.CLOCK_IN_DT, 120),'-','/') CLOCK_IN_DT,");
            sb.Append(" REPLACE(CONVERT(char(16), a.CLOCK_OUT_DT, 120),'-','/') CLOCK_OUT_DT,");
            //sb.Append(" DUTY_CHECK_RESULT_DESC,");
            sb.Append(" case when a.COMPARE_CD = '2' and a.LACK_HOUR > 0 then '代休加班，欠勤時數＝' + convert(varchar(10),a.LACK_HOUR)");
            sb.Append(" else DUTY_CHECK_RESULT + '-' + c.SUB_DESC end as DUTY_CHECK_RESULT_DESC");
            sb.Append("  , L.APPLY_OVERTIME_DT  , O.REPLACE_DT ");//代休加班日--代休假日期
            //sb.Append(" from TB_D_M_EMP_DUTY_CHECK_STATUS a  ");
            sb.Append(" from  (  ");
            sb.Append("  select * from TB_D_M_EMP_DUTY_CHECK_STATUS a ");
            sb.Append(" where (a.DUTY_CHECK_RESULT in ('E4','E5') or (a.COMPARE_CD = '2' and a.LACK_HOUR > 0))");
            //顯示資料權限設定
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID + "%");
            }
            if (CALENDAR_DT_S != "")
            {
                sb.Append(" and a.CALENDAR_DT >= @CALENDAR_DT_S ");
                ht.Add("@CALENDAR_DT_S", CALENDAR_DT_S);
            }
            if (CALENDAR_DT_E != "")
            {
                sb.Append(" and a.CALENDAR_DT <= @CALENDAR_DT_E ");
                ht.Add("@CALENDAR_DT_E", CALENDAR_DT_E);
            }
            if (DEPT_NO != "")
            {
                sb.Append(" and exists(select EMP_ID from VW_H_EMP_DATA where VW_H_EMP_DATA.DEPT_NO = @DEPT_NO and VW_H_EMP_DATA.EMP_ID = a.EMP_ID) ");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            sb.Append(" ) a  ");

            sb.Append(" inner join VW_H_EMP_DATA b on a.EMP_ID = b.EMP_ID ");
            sb.Append(" left join TB_9_M_COMM_D c on a.DUTY_CHECK_RESULT = c.SUB_CD and c.SYS_CD = 'DC' and c.MAIN_CD = 'DUTY_CHECK_RESULT' and c.SUB_CD in ('E4','E5')");
            sb.Append(@" left join 
                         ( select EMP_ID,APPLY_LEAVE_SDT,min(APPLY_OVERTIME_DT) APPLY_OVERTIME_DT from TB_D_M_LEAVE_APPLY_DAY L
                         where  FORM_STATUS NOT IN ('N','D') and CHECK_STATUS = 'Y' and	MAIN_LEAVE_CD='R'   
                         GROUP BY 	EMP_ID,APPLY_LEAVE_SDT
                         ) L on a.EMP_ID=L.EMP_ID and L.APPLY_LEAVE_SDT=a.CALENDAR_DT 
                         left join (
                         select EMP_ID,APPLY_OVERTIME_DT,min(REPLACE_DT) REPLACE_DT from TB_D_M_OVERTIME_APPLY
                          where FORM_STATUS NOT IN ('N','D') and CHECK_STATUS = 'Y'	and OVERTIME_CD='D'	   
                         GROUP BY  EMP_ID,APPLY_OVERTIME_DT
                         ) O on O.EMP_ID=a.EMP_ID and O.APPLY_OVERTIME_DT=a.CALENDAR_DT ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    internal DataTable getEMP_NAME(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select EMP_NAME from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDEPT_NAME(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select DEPT_NO,DEPT_NAME from VW_H_DEPT_DATA ");
            sb.Append(" where DEPT_NO = @DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
}