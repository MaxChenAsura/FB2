using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Data.Odbc;

/// <summary>
/// CFB2SJ3000DAO 的摘要描述
/// </summary>
public class CFB2SJ3000DAO : BaseDAO
{
    //考核資料維護檔 欄位
    public string SN { get; set; }//序號

    public string ASSESS_TYPE {get; set;}//考核類別

    public string ITEM_NAME {get; set;}//考核項目名稱

    public string ITEM_DESC {get; set;}//考核項目說明

    public int ASSESS_SCORE {get; set;}//最高分數

    public string CREATED_BY {get; set;}//新增人員

    public DateTime? CREATED_DT {get; set;}//新增日期時間

    public string UPDATED_BY {get; set;}//更新人員

    public DateTime? UPDATED_DT {get; set;}//更新日期時間

    public string FUNC_ID { get; set; }//更新人員



    
    public CFB2SJ3000DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得
    

   

    //依PK值取得資料，
    internal DataTable getPKData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount from TB_S_M_FOREIGN_ITEM");
            sb.Append(" where SN=@SN");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE");
            ht.Add("@SN", SN);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得Dtl的表頭資料
    public void getTitleData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select  a.*   ");
            sb.Append(" , a.ASSESS_TYPE + '-' + b.SUB_DESC  ASSESS_TYPE_DESC   ");
            sb.Append(" from TB_S_M_FOREIGN_ITEM a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.ASSESS_TYPE = b.SUB_CD and b.MAIN_CD = 'FASSESS_TYPE'  and b.SYS_CD='FJ' and b.IS_VALID='Y' ");
            sb.Append(" where 1=1 ");
           
            if (ASSESS_TYPE != "")
            {
                sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            }

            DataTable dt = dbConn.Query(sb, ht);
            /**
            foreach (DataRow dr in dt.Rows)
            {
                this.ASSESS_RELEASE_DT = dr["ASSESS_RELEASE_DT"].ToString() != "" ? Convert.ToDateTime(dr["ASSESS_RELEASE_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                this.ASSESS_TYPE_DESC = Convert.ToString(dr["ASSESS_TYPE_DESC"]);
                this.APPROVE_STATUS = Convert.ToString(dr["APPROVE_STATUS"]);
                this.APPROVE_STATUS_DESC = Convert.ToString(dr["APPROVE_STATUS_DESC"]);
                this.REMARK = Convert.ToString(dr["REMARK"]);
                this.FREEZE_FLAG = Convert.ToString(dr["FREEZE_FLAG"]);
            }**/

        }
        catch
        {
            throw;
        }

    }


    //取得執行 年獎對象生成SP的錯誤訊息
    internal DataTable getSPLOG(string proc_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select proc_status, proc_log from TB_H_R_SP_LOG  ");
            sb.Append(" where PROC_ID= @PROC_ID ");
            sb.Append(" and PROC_DT=(select max(PROC_DT)  maxb from TB_H_R_SP_LOG ) ");
            ht.Add("@PROC_ID", proc_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    
    #endregion

    #region Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
                             , string assess_type
                           )
    {
        try
        {

            //if (sortExpression.Contains("ENV_ALLOWANCE_TYPE"))
            //    sortExpression = sortExpression.Replace("ENV_ALLOWANCE_TYPE", "a.ENV_ALLOWANCE_TYPE");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" a.*   ");
            sb.Append(" , a.ASSESS_TYPE + '-' + b.SUB_DESC ASSESS_TYPE_DESC   ");
            sb.Append(" from TB_S_M_FOREIGN_ITEM a with (nolock) ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.ASSESS_TYPE = b.SUB_CD and b.MAIN_CD = 'FASSESS_TYPE'  and b.IS_VALID='Y'  and b.SYS_CD='FJ'  ");
            sb.Append(" where 1=1 ");

            //查詢條件
          
            if (assess_type != "-1")
            {
                sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
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
                        , string assess_type
                       )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_FOREIGN_ITEM ");
            sb.Append(" where 1=1 ");


            //查詢條件
          
            if (assess_type != "-1")
            {
                sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
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


    #region DB存取

    //刪除_考核資料 
    public void deleteData_H(int sn, string assess_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_FOREIGN_ITEM ");
            sb.Append(" where SN = @SN and ASSESS_TYPE = @ASSESS_TYPE ");
            ht.Add("@SN", sn);
            ht.Add("@ASSESS_TYPE", assess_type);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除_考核人事資料
    public void deleteData_D(int sn, string assess_type, string tableName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from  " + tableName + " ");
            sb.Append(" where SN = @SN and ASSESS_TYPE = @ASSESS_TYPE ");
            ht.Add("@SN", sn);
            ht.Add("@ASSESS_TYPE", assess_type);
            //ht.Add("@START_DT", start_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //新增
    internal void insertData()
    {
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_M_FOREIGN_ITEM ");
            sb.Append(" ( ");
            sb.Append(" ASSESS_TYPE,ITEM_NAME,ITEM_DESC,ASSESS_SCORE  ");
            sb.Append(" ,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" ( @ASSESS_TYPE,@ITEM_NAME,@ITEM_DESC,@ASSESS_SCORE  ");
            sb.Append("   ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID )");

            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);//考核類別
            ht.Add("@ITEM_NAME", ITEM_NAME);//考核項目名稱
            ht.Add("@ITEM_DESC", ITEM_DESC);//考核項目說明
            ht.Add("@ASSESS_SCORE", ASSESS_SCORE);//最高分數

            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@CREATED_DT", now);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);



            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新 
    public void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_FOREIGN_ITEM ");
            sb.Append(" set ITEM_NAME=@ITEM_NAME,");
            sb.Append(" ITEM_DESC=@ITEM_DESC,ASSESS_SCORE=@ASSESS_SCORE, ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where SN = @SN and ASSESS_TYPE = @ASSESS_TYPE ");

            ht.Add("@SN", SN);//考核類別
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);//考核類別
            ht.Add("@ITEM_NAME", ITEM_NAME);//考核項目名稱
            ht.Add("@ITEM_DESC", ITEM_DESC);//考核項目說明
            ht.Add("@ASSESS_SCORE", ASSESS_SCORE);//最高分數

            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion







}