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
/// CFB2SS0200DAO 的摘要描述
/// </summary>
public class CFB2SS0200DAO : BaseDAO
{

    //ss020基本欄位
    public string EMP_ID { get; set; }
    public string PAY_YM { get; set; }
    public string COMPUTER_TYPE { get; set; }
    public string RETIRE_SDT { get; set; }
    public string RETIRE_EDT { get; set; }

    public string SALARY_DT { get; set; }
    public string FIRED_TYPE { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    //ACES
    bool isSuper { get; set; }  //是否為supervisor(擔當)

    ACESLib.ACES aces;
    public CFB2SS0200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getDetail()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select A.EMP_NAME   ");            
            sb.Append(" ,H.*  ");
            sb.Append(" from TB_S_M_FIRED_PAY_H H with (nolock) ");
            sb.Append(" left join TB_H_M_EMP A with (nolock) on H.EMP_ID = A.EMP_ID ");            
            sb.Append(" where 1=1 ");
            sb.Append(" and H.EMP_ID=@EMP_ID  ");
            sb.Append(" and H.SALARY_DT = @SALARY_DT");
            sb.Append(" and H.FIRED_TYPE = @FIRED_TYPE");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@FIRED_TYPE", FIRED_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
     

    //修改 結案否
    public void updateCLOSE_YN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(" update TB_S_M_FIRED_PAY_H ");
            sb.Append(" set CLOSE_YN=@CLOSE_YN ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = GETDATE()");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where 1=1");
            sb.Append(" and EMP_ID = @EMP_ID ");            

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@EMP_ID", EMP_ID);
            
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }


    }

    //刪除計算檔
    internal void deleteFiredPayTable(string tableName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from  ");
            sb.Append(tableName);
            sb.Append(" where 1=1 ");
            sb.Append(" and SALARY_DT = @SALARY_DT");
            sb.Append(" and FIRED_TYPE = @FIRED_TYPE");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@FIRED_TYPE", FIRED_TYPE);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }



    public DataTable geExceltData()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select ");
            sb.Append(" a.*   ");
            sb.Append(" ,b.EMP_NAME,b.JOIN_DT, H.FIRED_DT     ");
            sb.Append(" from TB_S_M_FIRED_PAY_D a ");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID ");
            sb.Append(" left join  TB_S_M_FIRED_PAY_H H on a.EMP_ID=H.EMP_ID ");
            sb.Append(" where 1=1 ");            
            sb.Append(" and a.EMP_ID = @EMP_ID ");
            
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable geExceltDataH()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select ");
            sb.Append(" *,FIRED_PAY+FORECAST_WAGES as PAY_MONEY    ");
            sb.Append(" from TB_S_M_FIRED_PAY_H H ");
            sb.Append(" where H.EMP_ID = @EMP_ID ");            
                   
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }


    #region QRY Gridview 資料
    //Gridview 查詢資料
    public DataTable getQryData(int startRowIndex, int maximumRows, string sortExpression
                         , string sdt, string edt, string status 
                           )
    {
        try
        {
            /*
            if (sortExpression.Contains("PJOB_CD"))
                sortExpression = sortExpression.Replace("PJOB_CD", "A.PJOB_CD");
            if (sortExpression.Contains("SALARY_ID"))
                sortExpression = sortExpression.Replace("SALARY_ID", "A.SALARY_ID");
            */
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber, ");
            sb.Append(@" CONVERT(VARCHAR(10),A.SALARY_DT,111)  as SALARY_DT  
                        ,A.FIRED_TYPE
                        ,A.FIRED_NUM
                        ,A.FIRED_TOTAL
                        ,A.PRE_STATUS
                        ,A.PRE_DT
                        ,A.FIRED_TYPE+ iif(isnull(B.SUB_DESC,'')='','','-'+B.SUB_DESC) as TYPE_DESC
                        ,A.PRE_STATUS + iif(isnull(C.SUB_DESC,'')='','','-'+C.SUB_DESC)  as STATUS_DESC
                        from TB_S_M_FIRED_PAY A  with (nolock) 
                        left join TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='SS' and B.main_cd='FIRED_TYPE' and B.is_valid='Y' and A.FIRED_TYPE=B.sub_cd
                        left join TB_9_M_COMM_D C   with (nolock)  on C.SYS_CD='99' and C.main_cd='IS_YN' and C.is_valid='Y'  and A.PRE_STATUS=C.sub_cd 
                         ");
            sb.Append(" where 1=1 ");

            if (sdt != "")
            {
                sb.Append(" and SALARY_DT >=@SALARY_SDT ");
                ht.Add("@SALARY_SDT", sdt);
            }
            if (edt != "")
            {
                sb.Append(" and SALARY_DT <=@SALARY_EDT ");
                ht.Add("@SALARY_EDT", edt);
            }

            if (status != "-1")
            {
                sb.Append(" and PRE_STATUS= @PRE_STATUS ");
                ht.Add("@PRE_STATUS", status);
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
    public int getQryCount(int startRowIndex, int maximumRows
                       , string sdt, string edt, string status )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_S_M_FIRED_PAY A with (nolock) ");
            sb.Append(" where 1=1 ");

            if (sdt != "")
            {
                sb.Append(" and A.SALARY_DT >=@SALARY_SDT ");
                ht.Add("@SALARY_SDT", sdt);
            }
            if (edt != "")
            {
                sb.Append(" and A.SALARY_DT <=@SALARY_EDT ");
                ht.Add("@SALARY_EDT", edt);
            }

            if (status != "-1")
            {
                sb.Append(" and A.PRE_STATUS= @PRE_STATUS ");
                ht.Add("@PRE_STATUS", status);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }

    }

    #endregion

    #region Dtl1 Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
            , string fire_SDT, string fire_EDT, string emp_id, string salary_dt, string type 
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
            sb.Append(" a.EMP_ID,a.FIRED_DT, (a.FIRED_PAY+a.FORECAST_WAGES) as  FIRED_PAY  ");
            sb.Append(" ,b.EMP_NAME  ");
            sb.Append(" from TB_S_M_FIRED_PAY_H  a with (nolock) ");
            sb.Append(" left join TB_H_M_EMP b with (nolock) on a.EMP_ID=b.EMP_ID ");
            sb.Append(" where 1=1  ");

            //顯示資料權限設定
            //if (SessionHandle.Current.is_super != "Y")
            //{
            //    sb.Append(" and a.EMP_ID = @EMP_ID ");
            //    ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
            //}

            //查詢條件            
            if (salary_dt != "")
            {
                sb.Append(" and A.SALARY_DT= @SALARY_DT ");
                ht.Add("@SALARY_DT", salary_dt); ;
            }
            if (type != "")
            {
                sb.Append(" and A.FIRED_TYPE= @FIRED_TYPE  ");
                ht.Add("@FIRED_TYPE", type);
            }
           
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            
            //資遺日
            if (fire_SDT != "" && fire_EDT != "")
            {
                sb.Append(" and a.FIRED_DT between @fire_SDT and @fire_EDT ");
                ht.Add("@fire_SDT", fire_SDT);
                ht.Add("@fire_EDT", fire_EDT);
            }
            else if (string.IsNullOrEmpty(fire_SDT) && fire_EDT != "")
            {
                sb.Append(" and a.FIRED_DT <= @fire_EDT ");
                ht.Add("@fire_EDT", fire_EDT);
            }
            else if (fire_SDT != "" && string.IsNullOrEmpty(fire_EDT))
            {
                sb.Append(" and a.FIRED_DT >= @fire_SDT ");
                ht.Add("@fire_SDT", fire_SDT);
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
            , string fire_SDT, string fire_EDT, string emp_id, string salary_dt, string type)
    {
        try
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_FIRED_PAY_H  a with (nolock)");
            sb.Append(" where 1=1   ");

            //顯示資料權限設定
            //if (SessionHandle.Current.is_super != "Y")
            //{
            //    sb.Append(" and a.EMP_ID = @EMP_ID ");
            //    ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
            //}

            //查詢條件
            if (salary_dt != "")
            {
                sb.Append(" and A.SALARY_DT= @SALARY_DT ");
                ht.Add("@SALARY_DT", salary_dt); ;
            }
            if (type != "")
            {
                sb.Append(" and A.FIRED_TYPE= @FIRED_TYPE  ");
                ht.Add("@FIRED_TYPE", type);
            }

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }


            if (fire_SDT != "" && fire_EDT != "")
            {
                sb.Append(" and a.FIRED_DT between @fire_SDT and @fire_EDT ");
                ht.Add("@fire_SDT", fire_SDT);
                ht.Add("@fire_EDT", fire_EDT);
            }
            else if (string.IsNullOrEmpty(fire_SDT) && fire_EDT != "")
            {
                sb.Append(" and a.FIRED_DT <= @fire_EDT ");
                ht.Add("@fire_EDT", fire_EDT);
            }
            else if (fire_SDT != "" && string.IsNullOrEmpty(fire_EDT))
            {
                sb.Append(" and a.FIRED_DT >= @fire_SDT ");
                ht.Add("@fire_SDT", fire_SDT);
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

    #region Dtl2 Gridview 資料
    //Gridview 查詢資料
    public DataTable getDtlData(int startRowIndex, int maximumRows, string sortExpression
            , string emp_id, string salary_dt, string type
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
            sb.Append(" from TB_S_M_FIRED_PAY_D a with (nolock) ");
            sb.Append(" where 1=1 ");

            if (salary_dt != "")
            {
                sb.Append(" and A.SALARY_DT= @SALARY_DT ");
                ht.Add("@SALARY_DT", salary_dt); ;
            }
            if (type != "")
            {
                sb.Append(" and A.FIRED_TYPE= @FIRED_TYPE  ");
                ht.Add("@FIRED_TYPE", type);
            }
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "");
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
    public int getDtlCount(int startRowIndex, int maximumRows
             , string emp_id, string salary_dt, string type)
    {
        try
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_FIRED_PAY_D  a with (nolock)");
            sb.Append(" where 1=1 ");

            if (salary_dt != "")
            {
                sb.Append(" and A.SALARY_DT= @SALARY_DT ");
                ht.Add("@SALARY_DT", salary_dt); ;
            }
            if (type != "")
            {
                sb.Append(" and A.FIRED_TYPE= @FIRED_TYPE  ");
                ht.Add("@FIRED_TYPE", type);
            }
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "");
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


    /*是否已轉傳薪資, 0:未轉,>0已轉*/
    public int chkIS_SEND()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select count(*) cnt 
                        from TB_S_M_FIRED_PAY
                        where 1=1
                        and SALARY_DT = @SALARY_DT         
                        and FIRED_TYPE = @FIRED_TYPE         
                        and PRE_STATUS = 'Y' 

            ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@FIRED_TYPE", FIRED_TYPE);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["cnt"];
            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //檢核 節金檔 P-節金檔是否已發薪,A-節金是否已存在
    public string checkFN_SS_CHK_FESTIVAL(string check_cd)
    {
        try
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select dbo.FN_SS_CHK_FESTIVAL(@SALARY_DT,@INCENTIVE_TYPE,@CHECK_CD) as result;   ");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@INCENTIVE_TYPE", FIRED_TYPE);
            ht.Add("@CHECK_CD", check_cd);      //P-節金檔是否已發薪,A-節金是否已存在
            DataTable dt = dbConn.Query(sb, ht, true);

            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["result"].ToString();
            }
            return result;
        }
        catch
        {
            throw;
        }
    }

    #region  SP執行 轉薪資

    //獎金轉至薪資(節金檔)
    internal void exec_SP_SS_SEND_FESTIVAL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_SS_SEND_FESTIVAL");
            ht.Add("@p_SALARY_DT", SALARY_DT);
            ht.Add("@p_INCENTIVE_TYPE", FIRED_TYPE);
            ht.Add("@p_UserID", CREATED_BY);
            ht.Add("@p_FuncID", "FB2SS050");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //獎金取消轉至薪資(節金檔)
    internal void exec_SP_SS_CANCEL_FESTIVAL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_SS_CANCEL_FESTIVAL");
            ht.Add("@p_SALARY_DT", SALARY_DT);
            ht.Add("@p_INCENTIVE_TYPE", FIRED_TYPE);
            ht.Add("@p_UserID", CREATED_BY);
            ht.Add("@p_FuncID", FUNC_ID);
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }


    #endregion



}