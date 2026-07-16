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
/// CFB2DH0700DAO 的摘要描述
/// </summary>
public class CFB2DH0900DAO : BaseDAO
{
    public CFB2DH0900DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string dept_no
        ,string over_sdt,string over_edt,string leave_sdt,string leave_edt)
    {
        try
        {

            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "H.EMP_ID");
            if (sortExpression.Contains("APPLY_OVERTIME_DT"))
                sortExpression = sortExpression.Replace("APPLY_OVERTIME_DT", "H.APPLY_OVERTIME_DT");
            if (sortExpression.Contains("CALENDAR_DT"))
                sortExpression = sortExpression.Replace("CALENDAR_DT", "H.CALENDAR_DT");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(@" H.EMP_ID,E.EMP_NAME,E.DEPT_NO
                        ,Convert(varchar(10),H.APPLY_OVERTIME_DT,111) as APPLY_OVERTIME_DT
                        ,convert(decimal(7,2),(H.EXCHANGE_HOUR /60.0 ))  as EXCHANGE_HOUR	
                        ,convert(decimal(7,2),(H.REST_HOUR /60.0 ))  as REST_HOUR			
                        ,D.IFLOW_NO															
                        ,iif(D.CALENDAR_DT is null,'',Convert(varchar(10),D.CALENDAR_DT,111)) as CALENDAR_DT											        
                        ,convert(decimal(7,2),iif( isnull(D.MAPPING_HOUR,0)=0,0, D.MAPPING_HOUR/60.0)) as MAPPING_HOUR
                        ,DEPT.DEPT_FULL_NAME
                        ,H.FORM_STATUS, H.FORM_STATUS + '-' + G.SUB_DESC as FORM_STATUS_DESC
                        ");
            sb.Append(@" from TB_D_M_X0_OVERTIME_H H  with (nolock) 
                        left join  TB_D_M_X0_OVERTIME_D D   with (nolock)  on H.EMP_ID = D.EMP_ID and H.APPLY_OVERTIME_DT = D.APPLY_OVERTIME_DT and H.X0_TYPE =D.X0_TYPE
                        inner join TB_H_M_EMP E   with (nolock)  on E.EMP_ID = H.EMP_ID
                        left join TB_H_R_DEPT_DATA DEPT   with (nolock)  on DEPT.DEPT_NO = E.DEPT_NO
                         left join TB_9_M_COMM_D G on H.FORM_STATUS=G.SUB_CD and G.MAIN_CD='FORM_STATUS' and G.SYS_CD='DH' and G.IS_VALID='Y' 
                        ");

            //顯示資料權限設定
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" inner join  (	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments) )T    on H.EMP_ID=T.EMP_ID ");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }
            sb.Append(" where 1=1 ");

            if (emp_id != "")
            {
                sb.Append("and H.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (dept_no != "")
            {
                sb.Append(" and E.DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }

            if (over_sdt != "")
            {
                sb.Append(" and H.APPLY_OVERTIME_DT >= @OVERTIME_START_DT ");
                ht.Add("@OVERTIME_START_DT", over_sdt);
            }
            if (over_edt != "")
            {
                sb.Append(" and H.APPLY_OVERTIME_DT <= @OVERTIME_END_DT ");
                ht.Add("@OVERTIME_END_DT", over_edt);
            }
            if (leave_sdt != "")
            {
                sb.Append(" and D.CALENDAR_DT >= @LEAVE_START_DT ");
                ht.Add("@LEAVE_START_DT", leave_sdt);
            }
            if (leave_edt != "")
            {
                sb.Append("and D.CALENDAR_DT <= @LEAVE_END_DT ");
                ht.Add("@LEAVE_END_DT", leave_edt);
            }
            /*
            //顯示資料權限設定
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND H.EMP_ID IN( select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }
            */

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

    public int getCount(int startRowIndex, int maximumRows, string emp_id, string dept_no
        , string over_sdt, string over_edt, string leave_sdt, string leave_edt)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(@" from TB_D_M_X0_OVERTIME_H H  with (nolock) 
                        left join  TB_D_M_X0_OVERTIME_D D   with (nolock)  on H.EMP_ID = D.EMP_ID and H.APPLY_OVERTIME_DT = D.APPLY_OVERTIME_DT and H.X0_TYPE =D.X0_TYPE
                        ");
            
            //顯示資料權限設定
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" inner join  (	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments) )T    on H.EMP_ID=T.EMP_ID ");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }

            sb.Append(" where 1=1 ");

            if (emp_id != "")
            {
                sb.Append("and H.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (dept_no != "")
            {
                sb.Append(" and E.DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }

            if (over_sdt != "")
            {
                sb.Append(" and H.APPLY_OVERTIME_DT >= @OVERTIME_START_DT ");
                ht.Add("@OVERTIME_START_DT", over_sdt);
            }
            if (over_edt != "")
            {
                sb.Append(" and H.APPLY_OVERTIME_DT <= @OVERTIME_END_DT ");
                ht.Add("@OVERTIME_END_DT", over_edt);
            }
            if (leave_sdt != "")
            {
                sb.Append(" and D.CALENDAR_DT >= @LEAVE_START_DT ");
                ht.Add("@LEAVE_START_DT", leave_sdt);
            }
            if (leave_edt != "")
            {
                sb.Append("and D.CALENDAR_DT <= @LEAVE_END_DT ");
                ht.Add("@LEAVE_END_DT", leave_edt);
            }

 /*
            //顯示資料權限設定
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND H.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }
            */

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

    public DataTable getSUB_LEAVE_CD(string main_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SUB_LEAVE_CD,SUB_LEAVE_CD+'-'+SUB_LEAVE_DESC SUB_LEAVE_DESC ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_D ");
            sb.Append(" where MAIN_LEAVE_CD=@MAIN_LEAVE_CD ");
            ht.Add("@MAIN_LEAVE_CD", main_leave_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public DataTable getEmpName(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NAME ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getMAIN_LEAVE_DESC(string main_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD,MAIN_LEAVE_DESC ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_H ");
            sb.Append(" where MAIN_LEAVE_CD=@MAIN_LEAVE_CD ");
            ht.Add("@MAIN_LEAVE_CD", main_leave_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}