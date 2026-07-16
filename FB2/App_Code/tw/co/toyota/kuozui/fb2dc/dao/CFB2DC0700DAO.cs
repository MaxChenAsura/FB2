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
/// CFB2DC0700DAO 的摘要描述
/// </summary>
public class CFB2DC0700DAO : BaseDAO
{
    public string CLOCK_DT_S { get; set; }
    public string CLOCK_DT_E { get; set; }
    public string CARD_CHECK_STATUS { get; set; }
    public string PERSON_TYPE { get; set; }
    public string PERSON_DC { get; set; }
    public string PERSON_ID { get; set; }
    public string CARD_TYPE { get; set; }
    public string CLOCK_NO { get; set; }
    public string IS_SUPER { get; set; }
    public string IS_DEPT { get; set; }
    public string DEPARTMENTS { get; set; }

    public CFB2DC0700DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string clock_dt_s, string clock_dt_e, string card_check_status,
        string person_type, string person_dc, string person_id, string card_type, string clock_no,
        string is_super, string is_dept, string departments)
    {
        try
        {

            //因應組合資料表的排序方式
            if (sortExpression.Contains("PERSON_ID"))
                sortExpression = sortExpression.Replace("PERSON_ID", "a.PERSON_ID");
            if (sortExpression.Contains("CLOCK_DT"))
                sortExpression = sortExpression.Replace("CLOCK_DT", "a.CLOCK_DT");
            if (sortExpression.Contains("CLOCK_NO"))
                sortExpression = sortExpression.Replace("CLOCK_NO", "a.CLOCK_NO");
            if (sortExpression.Contains("CARD_NO"))
                sortExpression = sortExpression.Replace("CARD_NO", "a.CARD_NO");
            if (sortExpression.Contains("PERSON_NAME"))
               sortExpression = sortExpression.Replace("PERSON_NAME", "b.CARD_NAME");

            Hashtable ht = new Hashtable();
            StringBuilder sb_CLOCK_RECORD = new StringBuilder();
            //顯示資料權限設定,若不是super則要先限縮 勤務刷卡明細資料檔以免timeout
            if (person_type == "1")
            {
                sb_CLOCK_RECORD.Append(@" select a.* from  TB_D_M_CLOCK_RECORD a  with (nolock)   ");
                if (is_super != "Y")
                {
                    sb_CLOCK_RECORD.Append(@" inner join ( select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  ) T on a.PERSON_ID=T.EMP_ID");
                    ht.Add("@loginID", SessionHandle.Current.emp_id);
                    ht.Add("@departments", departments);
                }
                sb_CLOCK_RECORD.Append(@" where 1=1 ");
            }
            else {
                sb_CLOCK_RECORD.Append(@" select * from  TB_D_M_CLOCK_RECORD_VENDOR a  with (nolock)  where 1=1 ");
            }
            


            

            #region 查詢條件
            //刷卡日期時間(必要條件,防呆用)
            if (clock_dt_s != "" && clock_dt_e != "")
            {
                sb_CLOCK_RECORD.Append(" and a.CLOCK_DT >= CONVERT(datetime,@clock_dt_s) and a.CLOCK_DT <= CONVERT(datetime,@clock_dt_e)");
                ht.Add("@clock_dt_s", clock_dt_s + " 00:00:00");
                ht.Add("@clock_dt_e", clock_dt_e + " 23:59:59");
            }
            else 
            {
                sb_CLOCK_RECORD.Append(" and 1!=1 ");
            }
            //刷卡資料處理狀態
            if (card_check_status != "-1" && card_check_status != null)
            {
                sb_CLOCK_RECORD.Append(" and a.CARD_CHECK_STATUS = @card_check_status ");
                ht.Add("@card_check_status", card_check_status);
            }
            //部門代號/廠商別
            if (person_dc != "")
            {
                //如果 A.借卡人員別=員工(1) 則
                if (person_type == "1")
                {
                    sb_CLOCK_RECORD.Append(" and a.PERSON_ID in(select EMP_ID from VW_H_EMP_DATA where DEPT_NO=@person_dc) ");
                    ht.Add("@person_dc", person_dc);
                }
                else
                {
                    //如果 A.借卡人員別=廠商(2) 則 
                    sb_CLOCK_RECORD.Append(" and a.PERSON_ID in(select VENDOR_MEMBER_NO from TB_D_M_VENDOR_D where VENDOR_NO=@person_dc) ");
                    sb_CLOCK_RECORD.Append("  and left(a.card_no,2)='14' ");
                    ht.Add("@person_dc", person_dc);
                }
            }
            else
            {
                if (person_type == "1")
                {
                    sb_CLOCK_RECORD.Append(" and a.PERSON_ID in(select EMP_ID from VW_H_EMP_DATA) ");
                }
                else
                {
                    sb_CLOCK_RECORD.Append(" and a.PERSON_ID in(select VENDOR_MEMBER_NO from TB_D_M_VENDOR_D) ");
                    sb_CLOCK_RECORD.Append(" and left(a.card_no,2)='14'");
                }
            }
            //工號/廠商人員編號
            if (person_id != "")
            {
                sb_CLOCK_RECORD.Append(" and a.PERSON_ID = @person_id ");
                ht.Add("@person_id", person_id + "");
            }

            //卡片屬性
            if (card_type != "-1" && card_type != null)
            {
                sb_CLOCK_RECORD.Append(" and SUBSTRING(a.CARD_NO,1,2) = @card_type ");
                ht.Add("@card_type", card_type);
            }
            //卡鐘編號
            if (clock_no != "")
            {
                sb_CLOCK_RECORD.Append(" and a.CLOCK_NO = @clock_no ");
                ht.Add("@clock_no", clock_no);
            }
            #endregion

            StringBuilder sb = new StringBuilder();
            sb.Append(" select * From (");
            sb.Append(" select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber   ");
            sb.Append(" ,a.PERSON_ID, a.CLOCK_NO+'-'+DMC.CLOCK_DESC as CLOCK_NO,a.CARD_NO ,a.CLOCK_DT ");
            sb.Append(" ,case when h.CARD_USED_CD = 'A' then e.DEPT_NAME ");
            sb.Append("       when h.CARD_USED_CD = 'B' then g.VENDOR_NAME ");
            sb.Append("  end PERSON_DC ");
            sb.Append(" ,a.CARD_NAME PERSON_NAME ");
            sb.Append(" ,c.SUB_CD+'-'+c.SUB_DESC CLOCK_RETURN_STATUS,d.SUB_CD+'-'+d.SUB_DESC CARD_CHECK_STATUS ");

            sb.Append(" From ");
            sb.Append("(  " + sb_CLOCK_RECORD +" ) a");
            sb.Append(" left join TB_D_M_CLOCK DMC  with (nolock)  on a.CLOCK_NO=DMC.CLOCK_NO ");
            sb.Append(" left join TB_D_M_CARD b  with (nolock) on b.CARD_NO=a.CARD_NO ");
            sb.Append(" left join TB_9_M_COMM_D c  with (nolock) on c.SYS_CD='DC' and c.MAIN_CD='CLOCK_RETURN_STATUS' and c.SUB_CD=a.CLOCK_RETURN_STATUS ");
            sb.Append(" left join TB_9_M_COMM_D d  with (nolock) on d.SYS_CD='DC' and d.MAIN_CD='CARD_CHECK_STATUS' and d.SUB_CD=a.CARD_CHECK_STATUS ");
            sb.Append(" left join VW_H_EMP_DATA e  with (nolock)  on e.EMP_ID=a.PERSON_ID ");
            sb.Append(" left join TB_D_M_VENDOR_H g  with (nolock)  on g.VENDOR_NO in ");
            sb.Append(" (select VENDOR_NO from TB_D_M_VENDOR_D  with (nolock)  where VENDOR_MEMBER_NO=a.PERSON_ID) ");
            sb.Append(" left join TB_D_M_CARD_TYPE h  with (nolock)  on substring(a.CARD_NO,1,2) = h.CARD_TYPE ");

            sb.Append(")god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
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

    public int getCount(int startRowIndex, int maximumRows, string clock_dt_s, string clock_dt_e, string card_check_status,
        string person_type, string person_dc, string person_id, string card_type, string clock_no,
        string is_super, string is_dept, string departments)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            StringBuilder sb_CLOCK_RECORD = new StringBuilder();
            //顯示資料權限設定,若不是super則要先限縮 勤務刷卡明細資料檔以免timeout
            if (person_type == "1")
            {
                sb_CLOCK_RECORD.Append(@" select a.* from  TB_D_M_CLOCK_RECORD a  with (nolock) ");
                if (is_super != "Y")
                {
                    sb_CLOCK_RECORD.Append(@" inner join ( select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  ) T on a.PERSON_ID=T.EMP_ID");
                    ht.Add("@loginID", SessionHandle.Current.emp_id);
                    ht.Add("@departments", departments);
                }
                sb_CLOCK_RECORD.Append(@" where 1=1 ");
            }
            else
            {
                sb_CLOCK_RECORD.Append(@" select * from  TB_D_M_CLOCK_RECORD_VENDOR a  with (nolock) where 1=1 ");
            }
            /*
            //顯示資料權限設定
            if (person_type == "1" && is_super != "Y")
            {
                sb_CLOCK_RECORD.Append(@" AND a.PERSON_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", departments);
            }
             * */

            #region 查詢條件
            //刷卡日期時間(必要條件,防呆用)
            if (clock_dt_s != "" && clock_dt_e != "")
            {
                sb_CLOCK_RECORD.Append(" and a.CLOCK_DT >= CONVERT(datetime,@clock_dt_s) and a.CLOCK_DT <= CONVERT(datetime,@clock_dt_e)");
                ht.Add("@clock_dt_s", clock_dt_s + " 00:00:00");
                ht.Add("@clock_dt_e", clock_dt_e + " 23:59:59");
            }
            else
            {
                sb_CLOCK_RECORD.Append(" and 1!=1 ");
            }
            //刷卡資料處理狀態
            if (card_check_status != "-1" && card_check_status != null)
            {
                sb_CLOCK_RECORD.Append(" and a.CARD_CHECK_STATUS = @card_check_status ");
                ht.Add("@card_check_status", card_check_status);
            }
            //部門代號/廠商別
            if (person_dc != "")
            {
                //如果 A.借卡人員別=員工(1) 則
                if (person_type == "1")
                {
                    sb_CLOCK_RECORD.Append(" and a.PERSON_ID in(select EMP_ID from VW_H_EMP_DATA  with (nolock) where DEPT_NO=@person_dc) ");
                    ht.Add("@person_dc", person_dc);
                }
                else
                {
                    //如果 A.借卡人員別=廠商(2) 則 
                    sb_CLOCK_RECORD.Append(" and a.PERSON_ID in(select VENDOR_MEMBER_NO from TB_D_M_VENDOR_D with (nolock)  where VENDOR_NO=@person_dc) ");
                    ht.Add("@person_dc", person_dc);
                }
            }
            else
            {
                if (person_type == "1")
                {
                    sb_CLOCK_RECORD.Append(" and a.PERSON_ID in(select EMP_ID from VW_H_EMP_DATA with (nolock)) ");
                }
                else
                {
                    sb_CLOCK_RECORD.Append(" and a.PERSON_ID in(select VENDOR_MEMBER_NO from TB_D_M_VENDOR_D with (nolock)) ");
                }
            }
            //工號/廠商人員編號
            if (person_id != "")
            {
                sb_CLOCK_RECORD.Append(" and a.PERSON_ID LIKE @person_id ");
                ht.Add("@person_id", person_id + "%");
            }

            //卡片屬性
            if (card_type != "-1" && card_type != null)
            {
                sb_CLOCK_RECORD.Append(" and SUBSTRING(a.CARD_NO,1,2) = @card_type ");
                ht.Add("@card_type", card_type);
            }
            //卡鐘編號
            if (clock_no != "")
            {
                sb_CLOCK_RECORD.Append(" and a.CLOCK_NO = @clock_no ");
                ht.Add("@clock_no", clock_no);
            }
            #endregion

            sb.Append(" select COUNT(*) total_record ");

            sb.Append(" From ");
            sb.Append("(  " + sb_CLOCK_RECORD + " ) a");

            //sb.Append(" left join TB_D_M_CARD b  with (nolock) on b.CARD_NO=a.CARD_NO ");
            //sb.Append(" left join TB_9_M_COMM_D c  with (nolock) on c.SYS_CD='DC' and c.MAIN_CD='CLOCK_RETURN_STATUS' and c.SUB_CD=a.CLOCK_RETURN_STATUS ");
            //sb.Append(" left join TB_9_M_COMM_D d  with (nolock) on d.SYS_CD='DC' and d.MAIN_CD='CARD_CHECK_STATUS' and d.SUB_CD=a.CARD_CHECK_STATUS ");
            //sb.Append(" left join VW_H_EMP_DATA e  with (nolock) on e.EMP_ID=a.PERSON_ID ");
            //sb.Append(" left join TB_D_M_VENDOR_H g  with (nolock) on g.VENDOR_NO in ");
            //sb.Append(" (select VENDOR_NO from TB_D_M_VENDOR_D  with (nolock) where VENDOR_MEMBER_NO=a.PERSON_ID) ");
            //sb.Append(" left join TB_D_M_CARD_TYPE h  with (nolock) on substring(a.CARD_NO,1,2) = h.CARD_TYPE ");


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

    public int SP_DUTY_DATA_IMPORT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_DUTY_DATA_IMPORT");
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2DC070");
            return dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable searchCLOCK_NO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select distinct a.CLOCK_NO ");
            sb.Append(" from TB_D_M_CLOCK_RECORD a ");
            sb.Append(" left join TB_D_M_CARD_TYPE h on substring(a.CARD_NO,1,2) = h.CARD_TYPE ");
            sb.Append(" where 1=1 ");

            //刷卡日期時間
            if (CLOCK_DT_S != "")
            {
                if (CLOCK_DT_E != "")
                {
                    sb.Append(" and a.CLOCK_DT >= CONVERT(datetime,@CLOCK_DT_S) and a.CLOCK_DT <= CONVERT(datetime,@CLOCK_DT_E)");
                    ht.Add("@CLOCK_DT_S", CLOCK_DT_S + " 00:00:00");
                    ht.Add("@CLOCK_DT_E", CLOCK_DT_E + " 23:59:59");
                }
                else
                {
                    sb.Append(" and a.CLOCK_DT >= CONVERT(datetime,@CLOCK_DT_S) ");
                    ht.Add("@CLOCK_DT_S", CLOCK_DT_S + " 00:00:00");
                }
            }
            else if (CLOCK_DT_E != "")
            {
                sb.Append(" and a.CLOCK_DT <= CONVERT(datetime,@CLOCK_DT_E) ");
                ht.Add("@CLOCK_DT_E", CLOCK_DT_E + " 23:59:59");
            }

            if (CARD_CHECK_STATUS != "-1" && CARD_CHECK_STATUS != null)
            {
                sb.Append(" and a.CARD_CHECK_STATUS = @CARD_CHECK_STATUS ");
                ht.Add("@CARD_CHECK_STATUS", CARD_CHECK_STATUS);
            }

            if (PERSON_DC != "")
            {
                //如果 A.借卡人員別=員工(1) 則
                if (PERSON_TYPE == "1")
                {
                    sb.Append(" and a.PERSON_ID in(select EMP_ID from VW_H_EMP_DATA where DEPT_NO=@PERSON_DC) ");
                    ht.Add("@PERSON_DC", PERSON_DC);
                }
                else
                {
                    //如果 A.借卡人員別=廠商(2) 則 
                    sb.Append(" and a.PERSON_ID in(select VENDOR_MEMBER_NO from TB_D_M_VENDOR_D where VENDOR_NO=@PERSON_DC) ");
                    ht.Add("@PERSON_DC", PERSON_DC);
                }
            }

            if (PERSON_ID != "")
            {
                sb.Append(" and a.PERSON_ID LIKE @PERSON_ID ");
                ht.Add("@PERSON_ID", PERSON_ID + "%");
            }

            //卡片屬性
            if (CARD_TYPE != "-1" && CARD_TYPE != null)
            {
                sb.Append(" and SUBSTRING(a.CARD_NO,1,2) = @CARD_TYPE ");
                ht.Add("@CARD_TYPE", CARD_TYPE);
            }

            if (CLOCK_NO != "")
            {
                sb.Append(" and a.CLOCK_NO = @CLOCK_NO ");
                ht.Add("@CLOCK_NO", CLOCK_NO);
            }


            //顯示資料權限設定
            if (IS_SUPER != "Y")
            {
                //非管理者
                sb.Append(" and a.PERSON_ID in ( ");
                if (IS_DEPT == "Y")
                {
                    //若 資料權限之「部門含以下」為Y
                    sb.Append(" select a.EMP_ID from TB_H_R_HEAD_DEPT u1 ");
                    sb.Append(" where a.DEPT_NO=u1.MNG_DEPT_NO and u1.EMP_ID=@uEMP_ID");
                    sb.Append(" UNION ");
                }
                if (DEPARTMENTS != "")
                {
                    //若 資料權限之「部門權限」不為空值
                    sb.Append(" select u2.EMP_ID from TB_H_M_EMP u2 ");
                    sb.Append(" where u2.DEPT_NO in(@uDEPT_NO) ");
                    sb.Append(" UNION ");
                    ht.Add("@uDEPT_NO", DEPARTMENTS.Replace(" ", "").Split(','));
                }
                //登入者帳號
                sb.Append(" select u3.EMP_ID from TB_H_M_EMP u3 ");
                sb.Append(" where u3.EMP_ID=@uEMP_ID ");
                //廠商編號
                sb.Append(" UNION ");
                sb.Append(" select u4.VENDOR_MEMBER_NO from TB_D_M_VENDOR_D u4 where h.CARD_USED_CD = 'B' ) ");

                ht.Add("@uEMP_ID", SessionHandle.Current.emp_id);
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable searchCLOCK_RECORD(string clock_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select ");
            //如果 B.使用對象=社內 
            sb.Append(" case when h.CARD_USED_CD = 'A' then e.DEPT_NAME ");
            //如果 B.使用對象=社外
            sb.Append(" when h.CARD_USED_CD = 'B' then g.VENDOR_NAME");
            sb.Append(" end PERSON_DC,");

            sb.Append(" a.PERSON_ID,b.CARD_NAME PERSON_NAME,a.CLOCK_NO,a.CARD_NO,a.CLOCK_DT ");

            if (PERSON_TYPE == "1")
            {
                sb.Append(" from TB_D_M_CLOCK_RECORD a ");
            }
            else
            {
                //如果 A.借卡人員別=廠商(2) 則 
                sb.Append(" from TB_D_M_CLOCK_RECORD_VENDOR a ");
            }


            sb.Append(" left join TB_D_M_CARD b on b.CARD_NO=a.CARD_NO ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='DC' and c.MAIN_CD='CLOCK_RETURN_STATUS' and c.SUB_CD=a.CLOCK_RETURN_STATUS ");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='DC' and d.MAIN_CD='CARD_CHECK_STATUS' and d.SUB_CD=a.CARD_CHECK_STATUS ");
            sb.Append(" left join VW_H_EMP_DATA e on e.EMP_ID=a.PERSON_ID ");
            sb.Append(" left join TB_D_M_VENDOR_H g on g.VENDOR_NO in ");
            sb.Append(" (select VENDOR_NO from TB_D_M_VENDOR_D where VENDOR_MEMBER_NO=a.PERSON_ID) ");
            sb.Append(" left join TB_D_M_CARD_TYPE h on substring(a.CARD_NO,1,2) = h.CARD_TYPE ");

            sb.Append(" where a.CLOCK_NO=@clock_no ");
            ht.Add("@clock_no", clock_no);

            //刷卡日期時間
            if (CLOCK_DT_S != "")
            {
                if (CLOCK_DT_E != "")
                {
                    sb.Append(" and a.CLOCK_DT >= CONVERT(datetime,@CLOCK_DT_S) and a.CLOCK_DT <= CONVERT(datetime,@CLOCK_DT_E)");
                    ht.Add("@CLOCK_DT_S", CLOCK_DT_S + " 00:00:00");
                    ht.Add("@CLOCK_DT_E", CLOCK_DT_E + " 23:59:59");
                }
                else
                {
                    sb.Append(" and a.CLOCK_DT >= CONVERT(datetime,@CLOCK_DT_S) ");
                    ht.Add("@CLOCK_DT_S", CLOCK_DT_S + " 00:00:00");
                }
            }
            else if (CLOCK_DT_E != "")
            {
                sb.Append(" and a.CLOCK_DT <= CONVERT(datetime,@CLOCK_DT_E) ");
                ht.Add("@CLOCK_DT_E", CLOCK_DT_E + " 23:59:59");
            }

            if (CARD_CHECK_STATUS != "-1" && CARD_CHECK_STATUS != null)
            {
                sb.Append(" and a.CARD_CHECK_STATUS = @CARD_CHECK_STATUS ");
                ht.Add("@CARD_CHECK_STATUS", CARD_CHECK_STATUS);
            }

            if (PERSON_DC != "")
            {
                //如果 A.借卡人員別=員工(1) 則
                if (PERSON_TYPE == "1")
                {
                    sb.Append(" and a.PERSON_ID in(select EMP_ID from VW_H_EMP_DATA where DEPT_NO=@PERSON_DC) ");
                    ht.Add("@PERSON_DC", PERSON_DC);
                }
                else
                {
                    //如果 A.借卡人員別=廠商(2) 則 
                    sb.Append(" and a.PERSON_ID in(select VENDOR_MEMBER_NO from TB_D_M_VENDOR_D where VENDOR_NO=@PERSON_DC) ");
                    ht.Add("@PERSON_DC", PERSON_DC);
                }
            }

            if (PERSON_ID != "")
            {
                sb.Append(" and a.PERSON_ID LIKE @PERSON_ID ");
                ht.Add("@PERSON_ID", PERSON_ID + "%");
            }

            //卡片屬性
            if (CARD_TYPE != "-1" && CARD_TYPE != null)
            {
                sb.Append(" and SUBSTRING(a.CARD_NO,1,2) = @CARD_TYPE ");
                ht.Add("@CARD_TYPE", CARD_TYPE);
            }

            //顯示資料權限設定
            if (IS_SUPER != "Y")
            {
                //非管理者
                sb.Append(" and a.PERSON_ID in ( ");
                if (IS_DEPT == "Y")
                {
                    //若 資料權限之「部門含以下」為Y
                    sb.Append(" select a.EMP_ID from TB_H_R_HEAD_DEPT u1 ");
                    sb.Append(" where a.DEPT_NO=u1.MNG_DEPT_NO and u1.EMP_ID=@uEMP_ID");
                    sb.Append(" UNION ");
                }
                if (DEPARTMENTS != "")
                {
                    //若 資料權限之「部門權限」不為空值
                    sb.Append(" select u2.EMP_ID from TB_H_M_EMP u2 ");
                    sb.Append(" where u2.DEPT_NO in(@uDEPT_NO) ");
                    sb.Append(" UNION ");
                    ht.Add("@uDEPT_NO", DEPARTMENTS.Replace(" ", "").Split(','));
                }
                //登入者帳號
                sb.Append(" select u3.EMP_ID from TB_H_M_EMP u3 ");
                sb.Append(" where u3.EMP_ID=@uEMP_ID ");
                //廠商編號
                sb.Append(" UNION ");
                sb.Append(" select u4.VENDOR_MEMBER_NO from TB_D_M_VENDOR_D u4 where h.CARD_USED_CD = 'B' ) ");
                sb.Append(" order by a.PERSON_ID,a.CLOCK_DT ");
                ht.Add("@uEMP_ID", SessionHandle.Current.emp_id);
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getCARD_TYPE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select distinct CARD_TYPE+'-'+CARD_TYPE_DESC CARD_TYPE_DESC,CARD_TYPE");
            sb.Append(" from TB_D_M_CARD_TYPE");
            sb.Append(" order by CARD_TYPE");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
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

    public DataTable getVENDOR_MEMBER_NAME(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select a.VENDOR_MEMBER_NO,a.VENDOR_MEMBER_NAME EMP_NAME,b.VENDOR_NAME DEPT_NAME ");
            sb.Append(" from TB_D_M_VENDOR_D a ");
            sb.Append(" left join TB_D_M_VENDOR_H b on a.VENDOR_NO=b.VENDOR_NO ");
            sb.Append(" where a.VENDOR_NO is not null and a.VENDOR_MEMBER_NO=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDEPT_NAME(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select DEPT_NO,DEPT_NAME ");
            sb.Append(" from TB_H_M_DEPT ");
            sb.Append(" where DEPT_NO=@DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getVENDOR_H_NAME(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select VENDOR_NO,VENDOR_NAME DEPT_NAME");
            sb.Append(" from TB_D_M_VENDOR_H ");
            sb.Append(" where VENDOR_NO=@VENDOR_NO ");
            ht.Add("@VENDOR_NO", dept_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getCLOCK_DESC(string clock_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CLOCK_NO,CLOCK_DESC ");
            sb.Append(" from TB_D_M_CLOCK ");
            sb.Append(" where CLOCK_NO=@CLOCK_NO ");
            ht.Add("@CLOCK_NO", clock_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

}