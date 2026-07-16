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
/// CFB2SP0200DAO 的摘要描述
/// </summary>
public class CFB2SP0200DAO : BaseDAO
{
    //dj030基本欄位
    public string EMP_ID { get; set; }
    public string COMPUTER_TYPE { get; set; }
    public string CLOSE_YN { get; set; }
    public string RETIRE_SDT { get; set; }
    public string RETIRE_EDT { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    //ACES
    bool isSuper { get; set; }  //是否為supervisor(擔當)

    ACESLib.ACES aces;
    public CFB2SP0200DAO()
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
            sb.Append(" ,H.COMPUTER_TYPE + '-' + b.SUB_DESC COMPUTER_TYPE_DESC    ");
            sb.Append(" ,H.*  ");
            sb.Append(" from TB_S_M_OLDRETIRE_H H ");
            sb.Append(" left join TB_H_M_EMP A on H.EMP_ID = A.EMP_ID ");
            sb.Append(" left join TB_9_M_COMM_D b on  H.COMPUTER_TYPE = b.SUB_CD and b.MAIN_CD = 'COMPUTER_TYPE'  and b.IS_VALID='Y'  and b.SYS_CD='SP'  ");
            sb.Append(" where 1=1 ");
            sb.Append(" and H.EMP_ID=@EMP_ID  ");
            sb.Append(" and COMPUTER_TYPE=@COMPUTER_TYPE  ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@COMPUTER_TYPE", COMPUTER_TYPE);
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
            sb.Append(" update TB_S_M_OLDRETIRE_H ");
            sb.Append(" set CLOSE_YN=@CLOSE_YN ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = GETDATE()");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where 1=1");
            sb.Append(" and EMP_ID = @EMP_ID ");
            sb.Append(" and COMPUTER_TYPE = @COMPUTER_TYPE ");

            ht.Add("@CLOSE_YN", CLOSE_YN);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@COMPUTER_TYPE", COMPUTER_TYPE);

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
            sb.Append(" ,b.EMP_NAME,b.JOIN_DT, H.RETIRE_DT     ");
            sb.Append(" from TB_S_M_OLDRETIRE_D a ");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID ");
            sb.Append(" left join  TB_S_M_OLDRETIRE_H H on a.EMP_ID=H.EMP_ID and a.COMPUTER_TYPE=H.COMPUTER_TYPE ");
            sb.Append(" where 1=1 ");
            sb.Append(" and a.COMPUTER_TYPE = @COMPUTER_TYPE ");
            sb.Append(" and a.EMP_ID = @EMP_ID ");
            ht.Add("@COMPUTER_TYPE", COMPUTER_TYPE);
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
            sb.Append(" *   ");
            sb.Append(" ,Convert(int,round((AVG_PAY*RETIRE_BASE_MONTH ),0)) as RETIRE_PAY_EMP  ");
            sb.Append(" from TB_S_M_OLDRETIRE_H H ");
            sb.Append(" where 1=1 ");
            sb.Append(" and H.COMPUTER_TYPE = @COMPUTER_TYPE ");
            sb.Append(" and H.EMP_ID = @EMP_ID ");
            ht.Add("@COMPUTER_TYPE", COMPUTER_TYPE);
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }


    #region Qry Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
            , string computer_type, string retire_SDT, string retire_EDT, string emp_id, string close_YN
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
            sb.Append(" ,b.EMP_NAME  ");
            sb.Append(" from TB_S_M_OLDRETIRE_H a ");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID ");
            sb.Append(" where 1=1 ");

            //顯示資料權限設定
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
            }

            //查詢條件
            if (computer_type != "-1")
            {
                sb.Append(" and a.COMPUTER_TYPE = @COMPUTER_TYPE ");
                ht.Add("@COMPUTER_TYPE", computer_type);
            }

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }

            if (close_YN != "-1")
            {
                sb.Append(" and a.CLOSE_YN=@CLOSE_YN ");
                ht.Add("@CLOSE_YN", close_YN);
            }

            if (retire_SDT != "" && retire_EDT != "")
            {
                sb.Append(" and a.RETIRE_DT between @retire_SDT and @retire_EDT ");
                ht.Add("@retire_SDT", retire_SDT);
                ht.Add("@retire_EDT", retire_EDT);
            }
            else if (string.IsNullOrEmpty(retire_SDT) && retire_EDT != "")
            {
                sb.Append(" and a.RETIRE_DT <= @retire_EDT ");
                ht.Add("@retire_EDT", retire_EDT);
            }
            else if (retire_SDT != "" && string.IsNullOrEmpty(retire_EDT))
            {
                sb.Append(" and a.RETIRE_DT >= @retire_SDT ");
                ht.Add("@retire_SDT", retire_SDT);
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
            , string computer_type, string retire_SDT, string retire_EDT, string emp_id, string close_YN)
    {
        try
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_OLDRETIRE_H  a");
            sb.Append(" where 1=1 ");

            //顯示資料權限設定
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
            }

            //查詢條件
            if (computer_type != "-1")
            {
                sb.Append(" and a.COMPUTER_TYPE = @COMPUTER_TYPE ");
                ht.Add("@COMPUTER_TYPE", computer_type);
            }

            if (emp_id != "")
            {
                sb.Append(" and EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }

            if (close_YN != "")
            {
                sb.Append(" and a.CLOSE_YN=@CLOSE_YN ");
                ht.Add("@CLOSE_YN", close_YN);
            }

            if (retire_SDT != "" && retire_EDT != "")
            {
                sb.Append(" and a.RETIRE_DT between @retire_SDT and @retire_EDT ");
                ht.Add("@retire_SDT", retire_SDT);
                ht.Add("@retire_EDT", retire_EDT);
            }
            else if (string.IsNullOrEmpty(retire_SDT) && retire_EDT != "")
            {
                sb.Append(" and a.RETIRE_DT <= @retire_EDT ");
                ht.Add("@retire_EDT", retire_EDT);
            }
            else if (retire_SDT != "" && string.IsNullOrEmpty(retire_EDT))
            {
                sb.Append(" and a.RETIRE_DT >= @retire_SDT ");
                ht.Add("@retire_SDT", retire_SDT);
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



    #region Dtl Gridview 資料
    //Gridview 查詢資料
    public DataTable getDtlData(int startRowIndex, int maximumRows, string sortExpression
            , string computer_type, string emp_id
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
            sb.Append(" from TB_S_M_OLDRETIRE_D a ");
            sb.Append(" where 1=1 ");


            //查詢條件
            if (computer_type != "-1")
            {
                sb.Append(" and a.COMPUTER_TYPE = @COMPUTER_TYPE ");
                ht.Add("@COMPUTER_TYPE", computer_type);
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
             , string computer_type, string emp_id)
    {
        try
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_OLDRETIRE_D  a");
            sb.Append(" where 1=1 ");


            //查詢條件
            if (computer_type != "-1")
            {
                sb.Append(" and a.COMPUTER_TYPE = @COMPUTER_TYPE ");
                ht.Add("@COMPUTER_TYPE", computer_type);
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




}