using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2DC0500DAO 的摘要描述
/// </summary>
public class CFB2DC0500DAO : BaseDAO
{
    public string CARD_NO { get; set; }
    public string reopen_START_DT { get; set; }
    public string reopen_END_DT { get; set; }
    public string START_DT { get; set; }
    public string START_DT_PK { get; set; } 
    public string END_DT { get; set; }
    public string BORROW_TYPE { get; set; }
    public string PERSON_ID { get; set; }
    public string BORROW_REASON_CD { get; set; }
    public string BORROW_STATUS { get; set; }
    public string IS_RE_MAKE { get; set; }
    public string RETURN_DT { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    public string END_DT_REAL { get; set; }

    public string SYSCODEATT { get; set; }

    public string cardHandleCD { get; set; }

    public CFB2DC0500DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,
        string card_no, string borrow_type, string person_id, string borrow_status, string start_dt_s,
        string start_dt_e, string is_re_make, string end_dt_s, string end_dt_e)
    {
        try
        {
            if (sortExpression.Contains("BORROW_TYPE"))
                sortExpression = sortExpression.Replace("BORROW_TYPE", "a.BORROW_TYPE");
            if (sortExpression.Contains("PERSON_ID"))
                sortExpression = sortExpression.Replace("PERSON_ID", "a.PERSON_ID");

            if (sortExpression.Contains("PERSON_NAME"))
                sortExpression = sortExpression.Replace("PERSON_NAME", "e.EMP_NAME");

            if (sortExpression.Contains("PERSON_DC"))
                sortExpression = sortExpression.Replace("PERSON_DC", "e.DEPT_NAME");

            if (sortExpression.Contains("IS_RE_MAKE"))
                sortExpression = sortExpression.Replace("IS_RE_MAKE", "a.IS_RE_MAKE");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(@" case when a.BORROW_TYPE='1' then b.SUB_DESC+'-'+h.SUB_DESC 
                              ELSE b.SUB_DESC
                        END BORROW_TYPE_NAME
                        ");
            sb.Append(" ,b.SUB_DESC BORROW_TYPE_NAME2,a.BORROW_TYPE,a.PERSON_ID,");
            ////如果 A.借卡人員別=員工(1) 則
            //if (borrow_type == "1")
            //{
            //    sb.Append(" e.EMP_NAME PERSON_NAME,e.DEPT_NAME PERSON_DC,");
            //}
            //else
            //{
            //    //如果 A.借卡人員別=廠商(2) 則 
            //    sb.Append(" f.VENDOR_MEMBER_NAME PERSON_NAME,g.VENDOR_NAME PERSON_DC,");
            //}

            sb.Append(" CASE WHEN a.BORROW_TYPE ='1' THEN e.EMP_NAME ELSE f.VENDOR_MEMBER_NAME END PERSON_NAME,");
            sb.Append(@" CASE WHEN a.BORROW_TYPE ='1' THEN  e.DEPT_NO+' '+e.DIV_DEPT_FULL_NAME
                                                       ELSE g.VENDOR_NAME END PERSON_DC,");

            sb.Append(" a.CARD_NO,a.START_DT,a.END_DT,a.RETURN_DT,c.SUB_CD+'-'+c.SUB_DESC BORROW_STATUS,d.SUB_CD+'-'+d.SUB_DESC BORROW_REASON_CD,a.IS_RE_MAKE");
            sb.Append(" from TB_D_M_TEMP_CARD_RECORD a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DC' and b.MAIN_CD='BORROW_TYPE' and b.SUB_CD=a.BORROW_TYPE");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='DC' and c.MAIN_CD='BORROW_STATUS' and c.SUB_CD=a.BORROW_STATUS");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='DC' and d.MAIN_CD='BORROW_REASON_CD' and d.SUB_CD=a.BORROW_REASON_CD");
            sb.Append(" left join VW_H_EMP_DATA e on e.EMP_ID=a.PERSON_ID");
            sb.Append(" left join TB_9_M_COMM_D h on h.SYS_CD='HB' and h.MAIN_CD='EMP_CD' and h.SUB_CD=e.EMP_CD");
            sb.Append(" left join TB_D_M_VENDOR_D f on f.VENDOR_MEMBER_NO=a.PERSON_ID");
            sb.Append(" left join TB_D_M_VENDOR_H g on g.VENDOR_NO in");
            sb.Append(" (select VENDOR_NO from TB_D_M_VENDOR_D where VENDOR_MEMBER_NO=a.PERSON_ID)");
            sb.Append(" where 1=1 ");

            if (card_no != "")
            {
                sb.Append(" and a.CARD_NO LIKE @CARD_NO ");
                ht.Add("@CARD_NO", card_no + "%");
            }

            if (person_id != "")
            {
                sb.Append(" and a.PERSON_ID LIKE @PERSON_ID ");
                ht.Add("@PERSON_ID", person_id + "%");
            }

            if (borrow_status != "-1" && borrow_status != null)
            {
                sb.Append(" and a.BORROW_STATUS = @BORROW_STATUS ");
                ht.Add("@BORROW_STATUS", borrow_status);
            }

            if (start_dt_s != "")
            {
                if (start_dt_e != "")
                {
                    sb.Append(" and a.START_DT >= CONVERT(datetime,@start_dt_s) and a.START_DT <= CONVERT(datetime,@start_dt_e)");
                    ht.Add("@start_dt_s", start_dt_s + " 00:00:00");
                    ht.Add("@start_dt_e", start_dt_e + " 23:59:59");
                }
                else
                {
                    sb.Append(" and a.START_DT >= CONVERT(datetime,@start_dt_s) ");
                    ht.Add("@start_dt_s", start_dt_s);
                }
            }
            else if (start_dt_e != "")
            {
                sb.Append(" and a.START_DT <= CONVERT(datetime,@start_dt_e) ");
                ht.Add("@start_dt_e", start_dt_e + " 23:59:59");
            }

            if (is_re_make != "-1" && is_re_make != null)
            {
                sb.Append(" and a.IS_RE_MAKE = @is_re_make ");
                ht.Add("@is_re_make", is_re_make);
            }

            if (end_dt_s != "")
            {
                if (end_dt_e != "")
                {
                    sb.Append(" and a.END_DT >= CONVERT(datetime,@end_dt_s) and a.END_DT <= CONVERT(datetime,@end_dt_e)");
                    ht.Add("@end_dt_s", end_dt_s + " 00:00:00");
                    ht.Add("@end_dt_e", end_dt_e + " 23:59:59");
                }
                else
                {
                    sb.Append(" and a.END_DT >= CONVERT(datetime,@end_dt_s) ");
                    ht.Add("@end_dt_s", end_dt_s + " 00:00:00");
                }
            }
            else if (end_dt_e != "")
            {
                sb.Append(" and a.END_DT <= CONVERT(datetime,@end_dt_e) ");
                ht.Add("@end_dt_e", end_dt_e + " 23:59:59");
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

    public int getCount(int startRowIndex, int maximumRows,
        string card_no, string borrow_type, string person_id, string borrow_status, string start_dt_s,
        string start_dt_e, string is_re_make, string end_dt_s, string end_dt_e)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_TEMP_CARD_RECORD a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DC' and b.MAIN_CD='BORROW_TYPE' and b.SUB_CD=a.BORROW_TYPE");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='DC' and c.MAIN_CD='BORROW_STATUS' and c.SUB_CD=a.BORROW_STATUS");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='DC' and d.MAIN_CD='BORROW_REASON_CD' and d.SUB_CD=a.BORROW_REASON_CD");
            sb.Append(" left join VW_H_EMP_DATA e on e.EMP_ID=a.PERSON_ID");
            sb.Append(" left join TB_D_M_VENDOR_D f on f.VENDOR_MEMBER_NO=a.PERSON_ID");
            sb.Append(" left join TB_D_M_VENDOR_H g on g.VENDOR_NO in");
            sb.Append(" (select VENDOR_NO from TB_D_M_VENDOR_D where VENDOR_MEMBER_NO=a.PERSON_ID)");
            sb.Append(" where 1=1 ");

            if (card_no != "")
            {
                sb.Append(" and a.CARD_NO LIKE @CARD_NO ");
                ht.Add("@CARD_NO", card_no + "%");
            }

            if (person_id != "")
            {
                sb.Append(" and a.PERSON_ID LIKE @PERSON_ID ");
                ht.Add("@PERSON_ID", person_id + "%");
            }

            if (borrow_status != "-1" && borrow_status != null)
            {
                sb.Append(" and a.BORROW_STATUS = @BORROW_STATUS ");
                ht.Add("@BORROW_STATUS", borrow_status);
            }

            if (start_dt_s != "")
            {
                if (start_dt_e != "")
                {
                    sb.Append(" and a.START_DT >= CONVERT(datetime,@start_dt_s) and a.START_DT <= CONVERT(datetime,@start_dt_e)");
                    ht.Add("@start_dt_s", start_dt_s + " 00:00:00");
                    ht.Add("@start_dt_e", start_dt_e + " 23:59:59");
                }
                else
                {
                    sb.Append(" and a.START_DT >= CONVERT(datetime,@start_dt_s) ");
                    ht.Add("@start_dt_s", start_dt_s);
                }
            }
            else if (start_dt_e != "")
            {
                sb.Append(" and a.START_DT <= CONVERT(datetime,@start_dt_e) ");
                ht.Add("@start_dt_e", start_dt_e + " 23:59:59");
            }

            if (is_re_make != "-1" && is_re_make != null)
            {
                sb.Append(" and a.IS_RE_MAKE = @is_re_make ");
                ht.Add("@is_re_make", is_re_make);
            }

            if (end_dt_s != "")
            {
                if (end_dt_e != "")
                {
                    sb.Append(" and a.END_DT >= CONVERT(datetime,@end_dt_s) and a.END_DT <= CONVERT(datetime,@end_dt_e)");
                    ht.Add("@end_dt_s", end_dt_s + " 00:00:00");
                    ht.Add("@end_dt_e", end_dt_e + " 23:59:59");
                }
                else
                {
                    sb.Append(" and a.END_DT >= CONVERT(datetime,@end_dt_s) ");
                    ht.Add("@end_dt_s", end_dt_s + " 00:00:00");
                }
            }
            else if (end_dt_e != "")
            {
                sb.Append(" and a.END_DT <= CONVERT(datetime,@end_dt_e) ");
                ht.Add("@end_dt_e", end_dt_e + " 23:59:59");
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

    public DataTable getPKData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount ");
            sb.Append(" from TB_D_M_TEMP_CARD_RECORD ");
            sb.Append(" where CARD_NO = @CARD_NO and START_DT = @PK_START_DT; ");
            ht.Add("@CARD_NO", CARD_NO);
            ht.Add("@PK_START_DT", START_DT);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //刪除 TB_D_M_TEMP_CARD_RECORD
    public void deleteCARD_RECORD(string card_no, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_TEMP_CARD_RECORD set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DC050' ");
            sb.Append(" where CARD_NO = @CARD_NO and START_DT = @START_DT; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_TEMP_CARD_RECORD ");
            sb.Append(" where CARD_NO = @CARD_NO and START_DT = @START_DT; ");
            ht.Add("@CARD_NO", card_no);
            ht.Add("@START_DT", Convert.ToDateTime(start_dt));
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得員工照片資料
    public DataTable getPHOTOData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CODE_VAL1 + @EMP_ID + '.jpg' PHOTO_PATH");
            sb.Append(" from TB_9_M_PARAMETER where SYS_CD = 'HB' and MAIN_CD = 'PHOTO_PATH'");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select * from TB_D_M_TEMP_CARD_RECORD ");
            sb.Append(" where CARD_NO=@CARD_NO  ");
            sb.Append(" and END_DT_REAL >= @START_DT and START_DT <= @END_DT ");
            sb.Append(" and BORROW_STATUS<>'Y' ");
            ht.Add("@CARD_NO", CARD_NO);
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getCardHandle()
    {
        try
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select IIF(SUBSTRING(CODE_VAL2,1,1)='','1',SUBSTRING(CODE_VAL2,1,1)) as CardHandle, * from TB_9_M_COMM_D ");
            sb.Append(" where SYS_CD='DC' and MAIN_CD='BORROW_REASON_CD'  ");
            sb.Append(" and sub_cd=@sub_cd ");
            ht.Add("@sub_cd", BORROW_REASON_CD);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = (string)dt.Rows[0]["CardHandle"];
            }
            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //借用
    public void addTEMP_CARD_RECORD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_TEMP_CARD_RECORD(");
            sb.Append(" CARD_NO,START_DT,END_DT,BORROW_TYPE,PERSON_ID,BORROW_REASON_CD,BORROW_STATUS,IS_RE_MAKE,END_DT_REAL,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values (");
            sb.Append(" @CARD_NO,@START_DT,@END_DT,@BORROW_TYPE,@PERSON_ID,@BORROW_REASON_CD,@BORROW_STATUS,@IS_RE_MAKE,@END_DT_REAL,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@CARD_NO", CARD_NO);
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@BORROW_TYPE", BORROW_TYPE);
            ht.Add("@PERSON_ID", PERSON_ID);
            ht.Add("@BORROW_REASON_CD", BORROW_REASON_CD);
            ht.Add("@BORROW_STATUS", BORROW_STATUS);
            ht.Add("@IS_RE_MAKE", IS_RE_MAKE);
            ht.Add("@END_DT_REAL", END_DT_REAL);
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

    //取得修改頁面的顯示資料
    public DataTable getiniData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select b.SUB_DESC BORROW_TYPE,a.PERSON_ID, ");
            ////如果 A.借卡人員別=員工(1) 則
            //if (BORROW_TYPE == "1")
            //{
            //    //姓名 部門/廠商別
            //    sb.Append(" e.EMP_NAME PERSON_NAME,e.DEPT_NAME PERSON_DC,");
            //}
            //else
            //{
            //    //如果 A.借卡人員別=廠商(2) 則 
            //    sb.Append(" f.VENDOR_MEMBER_NAME PERSON_NAME,g.VENDOR_NAME PERSON_DC,");
            //}

            sb.Append(" CASE WHEN a.BORROW_TYPE ='1' THEN e.EMP_NAME ELSE f.VENDOR_MEMBER_NAME END PERSON_NAME,");
            sb.Append(" CASE WHEN a.BORROW_TYPE ='1' THEN e.DEPT_NAME ELSE g.VENDOR_NAME END PERSON_DC,");

            sb.Append(" h.SUB_CD+'-'+h.SUB_DESC TEMP_CARD_CD,a.BORROW_REASON_CD,a.BORROW_STATUS,a.IS_RE_MAKE,a.END_DT,a.RETURN_DT");
            sb.Append(" from TB_D_M_TEMP_CARD_RECORD a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DC' and b.MAIN_CD='BORROW_TYPE' and b.SUB_CD=a.BORROW_TYPE");
            sb.Append(" left join VW_H_EMP_DATA e on e.EMP_ID=a.PERSON_ID");
            sb.Append(" left join TB_D_M_VENDOR_D f on f.VENDOR_MEMBER_NO=a.PERSON_ID");
            sb.Append(" left join TB_D_M_VENDOR_H g on g.VENDOR_NO in");
            sb.Append(" (select VENDOR_NO from TB_D_M_VENDOR_D where VENDOR_MEMBER_NO=a.PERSON_ID)");
            sb.Append(" left join TB_9_M_COMM_D h on h.SYS_CD='DC' and h.MAIN_CD='TEMP_CARD_CD'");
            sb.Append(" and h.SUB_CD=(select t.TEMP_CARD_CD from TB_D_M_CARD t where t.CARD_NO=a.CARD_NO)");
            sb.Append(" where a.CARD_NO=@CARD_NO and a.START_DT=@START_DT ");

            ht.Add("@CARD_NO", CARD_NO);
            ht.Add("@START_DT", Convert.ToDateTime(START_DT));

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //修改
    public void updateTEMP_CARD_RECORD()
    {
        try
        {
            //  20150615 實際還卡時間(RETURN_DT),此功能主要是讓擔當變更借用結束日期,故 實際還卡時間(RETURN_DT)不能 修改
            //  20150615 當實際還卡時間(RETURN_DT) 不為 null時,不能修改
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_TEMP_CARD_RECORD ");
            sb.Append(" set START_DT=@START_DT,END_DT=@END_DT,BORROW_REASON_CD=@BORROW_REASON_CD,");
            sb.Append(" BORROW_STATUS=@BORROW_STATUS,IS_RE_MAKE=@IS_RE_MAKE,END_DT_REAL=@END_DT_REAL, ");
            sb.Append(" RETURN_DT=IIF(RETURN_DT is null,@RETURN_DT,RETURN_DT), ");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where CARD_NO=@CARD_NO and START_DT=@PK_START_DT");

            ht.Add("@CARD_NO", CARD_NO);
            ht.Add("@START_DT", START_DT);
            ht.Add("@PK_START_DT", START_DT_PK);
            ht.Add("@END_DT", END_DT);
            ht.Add("@BORROW_REASON_CD", BORROW_REASON_CD);
            ht.Add("@BORROW_STATUS", BORROW_STATUS);
            ht.Add("@IS_RE_MAKE", IS_RE_MAKE);
            if (RETURN_DT != "")
            {
                ht.Add("@RETURN_DT", RETURN_DT);
            }
            else
            {
                ht.Add("@RETURN_DT", DBNull.Value);
            }
            ht.Add("@END_DT_REAL", END_DT_REAL);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //修改借用迄日(實際)
    public void updateTEMP_CARD_RECORD_REAL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_TEMP_CARD_RECORD ");
            sb.Append(@" set END_DT_REAL=
                        case 
                        when RETURN_DT is null then END_DT
                        else IIF(END_DT>=RETURN_DT,RETURN_DT,END_DT)
                        end
                     ");
            sb.Append(" ,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where CARD_NO=@CARD_NO and START_DT=@PK_START_DT");

            ht.Add("@CARD_NO", CARD_NO);
            ht.Add("@PK_START_DT", START_DT_PK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //修改 勤務刷卡明細暫存檔 TB_D_M_CLOCK_RECORD_TEMP 的 人事系統更新日期時間(HR_UPDATED_DT)為null 
    public void updateCLOCK_RECORD_TEM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"update T
                        set T.HR_UPDATED_DT=NULL,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID
                        from TB_D_M_CLOCK_RECORD_TEMP T
                        left join TB_D_M_TEMP_CARD_RECORD	 R on T.CARD_NO=R.CARD_NO and R.START_DT=@START_DT
                        where R.CARD_NO=@CARD_NO and CLOCK_DT>= dateadd(day,1, R.START_DT) and CLOCK_DT<=R.END_DT_REAL
                         ");
            ht.Add("@CARD_NO", CARD_NO);
            ht.Add("@START_DT", START_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    //歸還
    public void updateTEMP_CARD_RECORD2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_TEMP_CARD_RECORD ");
            sb.Append(" set END_DT=@END_DT,");
            sb.Append(" BORROW_STATUS=@BORROW_STATUS,RETURN_DT=@RETURN_DT,END_DT_REAL=@END_DT_REAL,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where CARD_NO=@CARD_NO and START_DT=@START_DT");

            ht.Add("@CARD_NO", CARD_NO);
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@BORROW_STATUS", BORROW_STATUS);
            if (RETURN_DT != "")
                ht.Add("@RETURN_DT", RETURN_DT);
            else
                ht.Add("@RETURN_DT", DBNull.Value);

            ht.Add("@END_DT_REAL", END_DT_REAL);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得介面查詢必要的資料(歸還)
    public DataTable getCARD_NO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CARD_NO,START_DT,BORROW_TYPE ");
            sb.Append(" from TB_D_M_TEMP_CARD_RECORD ");
            sb.Append(" where CARD_NO=@CARD_NO and BORROW_STATUS = 'N' ");
            ht.Add("@CARD_NO", CARD_NO);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得 部門/廠商別
    public DataTable getPERSON_DC()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //如果 A.借卡人員別=員工(1) 則
            if (BORROW_TYPE == "1")
            {
                sb.Append("select DEPT_NAME PERSON_DC ");
                sb.Append(" from VW_H_EMP_DATA where EMP_ID=@PERSON_ID ");
            }
            else
            {
                //如果 A.借卡人員別=廠商(2) 則
                sb.Append("select VENDOR_NAME PERSON_DC ");
                sb.Append(" from TB_D_M_VENDOR_H where VENDOR_NO in ");
                sb.Append(" (select VENDOR_NO from TB_D_M_VENDOR_D where VENDOR_MEMBER_NO=@PERSON_ID)");
            }
            ht.Add("@PERSON_ID", PERSON_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得臨時卡區分
    public DataTable getTEMP_CARD_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select SUB_CD ,SUB_CD+'-'+SUB_DESC SUB_DESC From TB_9_M_COMM_D Where MAIN_CD = @MAIN_CD ");
            sb.Append(" and SYS_CD=@SYS_CD");
            if (SYSCODEATT != "")
            {
                sb.Append(" and SUB_CD in (@SUB_CD)");
                ht.Add("@SUB_CD", SYSCODEATT.Split(','));
            }

            ht.Add("@MAIN_CD", "TEMP_CARD_CD");
            ht.Add("@SYS_CD", "DC");

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

    public DataTable getBORROW_END_DT(string emp_id, DateTime stime)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select dbo.FN_D_GET_BORROW_END_DT(@emp_id,@stime) BORROW_END_DT ");
            ht.Add("@emp_id", emp_id);
            ht.Add("@stime", stime);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //借用卡號(查詢)
    public DataTable getCARD_NAME(string card_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.CARD_NO,a.CARD_NAME ");
            sb.Append(" from TB_D_M_CARD a ");  //卡片資料檔
            sb.Append(" left join TB_D_M_CARD_TYPE b on a.CARD_TYPE=b.CARD_TYPE ");  //卡片屬性設定檔
            sb.Append(" where a.TEMP_CARD_CD != '' ");  //臨時卡區分
            sb.Append(" and GETDATE() >= a.START_DT and GETDATE() <= a.END_DT and b.CARD_USED_CD = 'C' ");    //卡片使用對象代碼  A.社內  B.社外  C.共用
            sb.Append(" and a.CARD_NO=@CARD_NO ");

            ht.Add("@CARD_NO", card_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //借用卡號(借用)
    public DataTable getCARD_NAME2(string card_no, string temp_card_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.CARD_NO,a.CARD_NAME ");
            sb.Append(" from TB_D_M_CARD a ");
            sb.Append(" left join TB_D_M_CARD_TYPE b on b.CARD_TYPE=a.CARD_TYPE ");
            sb.Append(" where a.CARD_NO not in( ");
            sb.Append(" select c.CARD_NO from TB_D_M_TEMP_CARD_RECORD c where c.BORROW_STATUS != 'Y' ) ");
            sb.Append(" and GETDATE() >= a.START_DT and GETDATE() <= a.END_DT and b.CARD_USED_CD='C' ");
            sb.Append(" and a.TEMP_CARD_CD in (@TEMP_CARD_CD) ");
            sb.Append(" and a.CARD_NO=@CARD_NO ");

            ht.Add("@TEMP_CARD_CD", temp_card_cd.Split(','));
            ht.Add("@CARD_NO", card_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //借用卡號(歸還)
    public DataTable getCARD_NAME3(string card_no, string temp_card_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.CARD_NO,a.CARD_NAME ");
            sb.Append(" from TB_D_M_CARD a ");
            sb.Append(" left join TB_D_M_CARD_TYPE b on b.CARD_TYPE=a.CARD_TYPE ");
            sb.Append(" where a.CARD_NO in( ");
            sb.Append(" select c.CARD_NO from TB_D_M_TEMP_CARD_RECORD c where c.BORROW_STATUS ='N' ) ");
            sb.Append(" and GETDATE() >= a.START_DT and GETDATE() <= a.END_DT and b.CARD_USED_CD='C' ");
            sb.Append(" and a.TEMP_CARD_CD in (@TEMP_CARD_CD) ");
            sb.Append(" and a.CARD_NO=@CARD_NO ");

            ht.Add("@TEMP_CARD_CD", temp_card_cd.Split(','));
            ht.Add("@CARD_NO", card_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    /*
    //維護卡片資料檔(製新卡)
    public void SP_D_UPD_CARD_DATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_UPD_CARD_DATA");
            ht.Add("@pHandleCd", "I2");
            ht.Add("@pEmpId", PERSON_ID);
            ht.Add("@pCardUsedCd", "A");
            ht.Add("@pStartDt", DateTime.Now.ToString("yyyy/MM/dd"));
            ht.Add("@pEndDt", "9999/12/31");
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2DC050");
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    */
    //維護卡片資料檔(重新製卡及重新卡)
    public void SP_D_UPD_CARD_DATA_RE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_UPD_CARD_DATA");

            //卡片處理3,重製卡, 1:製新卡
            if (cardHandleCD == "3")
            {
                ht.Add("@pHandleCd", "I3");
            }
            else {
                ht.Add("@pHandleCd", "I2");
            }
            ht.Add("@pEmpId", PERSON_ID);
            //員工是A,廠商是B
            if (BORROW_TYPE == "1")
            {
                ht.Add("@pCardUsedCd", "A");
            }
            else {
                ht.Add("@pCardUsedCd", "B");
            }
            
            
            ht.Add("@pStartDt", DateTime.Now.ToString("yyyy/MM/dd"));
            ht.Add("@pEndDt", "9999/12/31");
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2DC050");
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //卡片續用(一)
    public void SP_D_UPD_CARD_DATA1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_UPD_CARD_DATA1");
            ht.Add("@pCardNo", CARD_NO);
            ht.Add("@pEndDt", "9999/12/31");
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2DC050");
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //將 日勤務狀態檔 比對結果 改為 N
    internal void SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN(DateTime PocessDate)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN");
            ht.Add("@pEmpId", PERSON_ID);
            ht.Add("@pCalendarDt", PocessDate);
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2DC050");

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //修改
    public void updateCLOCK_RECORD_TEMP(DateTime PocessDate)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"update TB_D_M_TEMP_CARD_RECORD 



                       ");

            ht.Add("@CARD_NO", CARD_NO);
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@BORROW_REASON_CD", BORROW_REASON_CD);
            ht.Add("@BORROW_STATUS", BORROW_STATUS);
            ht.Add("@IS_RE_MAKE", IS_RE_MAKE);
            if (RETURN_DT != "")
                ht.Add("@RETURN_DT", RETURN_DT);
            else
                ht.Add("@RETURN_DT", DBNull.Value);

            ht.Add("@END_DT_REAL", END_DT_REAL);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //該員工卡片是否有效
    public string isVaildCard()
    {
        try
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" 

                         if not exists(
		                    select 1 from TB_D_M_CARD_CONTROL
		                    where 1=1
                            and EMP_ID =  @EMP_ID
                            and CALENDAR_DT = CONVERT(VARCHAR(10),GETDATE(),111)
	                    )
                            select 'D' as IsVaild

                        if exists(
		                    select 1 from TB_D_M_CARD_CONTROL
		                    where 1=1
                            and EMP_ID =  @EMP_ID
		                    and IS_VALID='Y'
                            and CALENDAR_DT = CONVERT(VARCHAR(10),GETDATE(),111)
	                    )
                            select 'Y' as IsVaild

                        if exists(
		                    select 1 from TB_D_M_CARD_CONTROL
		                    where 1=1
                            and EMP_ID =  @EMP_ID
		                    and IS_VALID='N'
                             and CALENDAR_DT = CONVERT(VARCHAR(10),GETDATE(),111)
	                    )
                            select 'N' as IsVaild
                       

            ");
            ht.Add("@EMP_ID", PERSON_ID);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = (string)dt.Rows[0]["IsVaild"];
            }
            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

}