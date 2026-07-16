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
/// CFB2DC1600DAO 的摘要描述
/// </summary>
public class CFB2DC1600DAO : BaseDAO
{
    public string CLOCK_NO { get; set; }
    public string CARD_NO { get; set; }
    public string CLOCK_DT { get; set; }
    public string CLOCK_DT_YMD { get; set; }
    public string PERSON_ID { get; set; }
    public string PERSON_ID_ORI { get; set; } //舊的
    public string PLANT_CD { get; set; } 
    public string CARD_NAME { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }


    public string CLOCK_DT_S { get; set; }
    public string CLOCK_DT_E { get; set; }
    public string CARD_CHECK_STATUS { get; set; }
    public string PERSON_TYPE { get; set; }
    public string CARD_TYPE { get; set; }

    public CFB2DC1600DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string clock_dt_s, string clock_dt_e, string card_check_status,
        string person_id, string clock_no)
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
            //if (sortExpression.Contains("PERSON_NAME"))
            // sortExpression = sortExpression.Replace("PERSON_NAME", "b.CARD_NAME");

            Hashtable ht = new Hashtable();
            StringBuilder sb_CLOCK_RECORD = new StringBuilder();
            sb_CLOCK_RECORD.Append(@" select a.* from  TB_D_M_CLOCK_RECORD a  with (nolock)   ");
            sb_CLOCK_RECORD.Append(@" where 1=1 ");
            sb_CLOCK_RECORD.Append(@" and LEFT(CARD_NO,2) in (select CARD_TYPE from TB_D_M_CARD_TYPE
                                      where CLOCK_TYPE_A='Y' and CARD_USED_CD='C') ");
            
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

            //工號/廠商人員編號
            if (person_id != "")
            {
                sb_CLOCK_RECORD.Append(" and a.PERSON_ID LIKE @person_id ");
                ht.Add("@person_id", person_id + "%");
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
            sb.Append(" ,a.PERSON_ID, a.CLOCK_NO+'-'+DMC.CLOCK_DESC as CLOCK_NO_DESC,a.CARD_NO ,a.CLOCK_DT, a.CLOCK_NO ");
            sb.Append(" ,e.DEPT_NAME PERSON_DC");
            sb.Append(" ,a.CARD_NAME PERSON_NAME ");
            sb.Append(" ,c.SUB_CD+'-'+c.SUB_DESC CLOCK_RETURN_STATUS,d.SUB_CD+'-'+d.SUB_DESC CARD_CHECK_STATUS ");

            sb.Append(" From ");
            sb.Append("(  " + sb_CLOCK_RECORD + " ) a");
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
        string person_id, string clock_no)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            StringBuilder sb_CLOCK_RECORD = new StringBuilder();
            //顯示資料權限設定,若不是super則要先限縮 勤務刷卡明細資料檔以免timeout
            sb_CLOCK_RECORD.Append(@" select a.* from  TB_D_M_CLOCK_RECORD a  with (nolock) ");
            sb_CLOCK_RECORD.Append(@" where 1=1 ");
            sb_CLOCK_RECORD.Append(@" and LEFT(CARD_NO,2) in (select CARD_TYPE from TB_D_M_CARD_TYPE
                                      where CLOCK_TYPE_A='Y' and CARD_USED_CD='C') ");

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

            //工號/廠商人員編號
            if (person_id != "")
            {
                sb_CLOCK_RECORD.Append(" and a.PERSON_ID LIKE @person_id ");
                ht.Add("@person_id", person_id + "%");
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


    //修改
    public void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
//            sb.Append(@"declare @PLANT_CD varchar(1)='';
//                        declare @CARD_NAME nvarchar(20)='';
//                        select @PLANT_CD =PLANT_CD, @CARD_NAME=EMP_NAME 
//                        from TB_H_M_EMP with (nolock)  where EMP_ID=@PERSON_ID  ");

            sb.Append(" update TB_D_M_CLOCK_RECORD ");
            sb.Append(" set PERSON_ID = @PERSON_ID ");
            sb.Append(" ,CARD_NAME =@CARD_NAME");
            sb.Append(" ,PLANT_CD =@PLANT_CD");
            sb.Append(" ,CARD_CHECK_STATUS =@CARD_CHECK_STATUS");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where 1=1");
            sb.Append(" and CLOCK_NO = @CLOCK_NO ");
            sb.Append(" and CARD_NO = @CARD_NO ");
            sb.Append(" and CLOCK_DT = @CLOCK_DT ");

            if (string.IsNullOrEmpty(PERSON_ID.Trim()))
            {
                ht.Add("@PERSON_ID", DBNull.Value);
                ht.Add("@CARD_CHECK_STATUS", "S");
                ht.Add("@PLANT_CD", "");
                ht.Add("@CARD_NAME", "");
            }
            else
            {
                ht.Add("@PERSON_ID", PERSON_ID);
                ht.Add("@CARD_CHECK_STATUS", "Y");
                ht.Add("@PLANT_CD", PLANT_CD);
                ht.Add("@CARD_NAME", CARD_NAME);
            }

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            //PK值
            ht.Add("@CLOCK_NO", CLOCK_NO);
            ht.Add("@CARD_NO", CARD_NO);
            ht.Add("@CLOCK_DT", CLOCK_DT);
            //ht.Add("@START_DT", Convert.ToDateTime(START_DT));

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }


    }


    //將 日勤務狀態檔 比對結果 改為 N
    internal void SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN");
            ht.Add("@pEmpId", EMP_ID);
            ht.Add("@pCalendarDt", CLOCK_DT_YMD);
            ht.Add("@pUserID", UPDATED_BY);
            ht.Add("@pFuncID", FUNC_ID);

            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }



}