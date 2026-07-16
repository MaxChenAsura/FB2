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
/// CFB2SI0250DAO 的摘要描述
/// </summary>
public class WFB2SI0250DAO : BaseDAO
{
    public WFB2SI0250DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string BONUS_YEAR { get; set; }
    public string BONUS_ROUND { get; set; }
    public string FREEZE_FLAG { get; set; } //凍結註記

    public string EMP_ID { get; set; } //工號
    public string BONUS_AMT { get; set; } //年獎金額
    	


    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    

    //取得最新的年度及類型
    public  void getBonusData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"SELECT top 1 convert(varchar,BONUS_YEAR) BONUS_YEAR,BONUS_ROUND  FROM TB_S_M_BONUS_H
            ORDER BY BONUS_YEAR desc, BONUS_ROUND DESC ");

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                BONUS_YEAR = dt.Rows[0]["BONUS_YEAR"].ToString();
                BONUS_ROUND = dt.Rows[0]["BONUS_ROUND"].ToString();
            }
            else {
                BONUS_YEAR ="";
                BONUS_ROUND ="";
            }
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    //取得是否凍結中
    public string getFreeze_flag()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"SELECT  FREEZE_FLAG FROM TB_S_M_BONUS_H 
            where BONUS_YEAR= @BONUS_YEAR
            and BONUS_ROUND = @BONUS_ROUND
            ");

            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@BONUS_ROUND", BONUS_ROUND);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                FREEZE_FLAG = (string)dt.Rows[0]["FREEZE_FLAG"];
            }
            else
            {
                FREEZE_FLAG = "E";
            }
            return FREEZE_FLAG;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //紅利匯入檢核(工號)
    public int chkEMP_ID(string empid)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"
                IF EXISTS 
                (	SELECT 1 FROM TB_S_M_BONUS_D WHERE BONUS_YEAR=@BONUS_YEAR AND BONUS_COUNT=@BONUS_ROUND AND EMP_ID=@EMP_ID  and left(LEVEL_CD,1) in ('1','2')  )
                  SELECT 1 AS resultCount
                ELSE
                  SELECT 0 AS resultCount

            ");
            //PK值
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@BONUS_ROUND", BONUS_ROUND);
            ht.Add("@EMP_ID", empid);

            //dbConn.ExecuteT(sb, ht, true);
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
    //紅利匯入檢核(工號+姓名)
    public int chkEMP_ID_NAME(string empid, string emp_name)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"
                IF EXISTS 
                (	SELECT 1 FROM TB_S_M_BONUS_D WHERE BONUS_YEAR=@BONUS_YEAR AND BONUS_COUNT=@BONUS_ROUND AND EMP_ID=@EMP_ID  and EMP_NAME=@EMP_NAME  )
                  SELECT 1 AS resultCount
                ELSE
                  SELECT 0 AS resultCount

            ");
            //PK值
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@BONUS_ROUND", BONUS_ROUND);
            ht.Add("@EMP_ID", empid);
            ht.Add("@EMP_NAME", emp_name);

            //dbConn.ExecuteT(sb, ht, true);
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

    //年獎金額更新
    public void updateAward_DM()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_BONUS_D ");
            sb.Append(" set  ");
            sb.Append("  BONUS_AMT = @BONUS_AMT");
            sb.Append(" ,BONUS_TAX = convert(decimal(10,2),@BONUS_AMT) * 0.05 ");
            sb.Append(" ,BONUS_AMT_R = convert(decimal(10,2),@BONUS_AMT)- convert(decimal(10,2),@BONUS_AMT) * 0.05");
            sb.Append(" ,APPROVE_FLAG = @APPROVE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = getdate()");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR ");
            sb.Append("  and BONUS_COUNT = @BONUS_ROUND");
            sb.Append("  and EMP_ID = @EMP_ID");

            //set值
            ht.Add("@BONUS_AMT", BONUS_AMT);
            ht.Add("@PRIMEVAL_FLAG", "Y");
            ht.Add("@APPROVE_FLAG", "N");
            //PK值
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@BONUS_ROUND", BONUS_ROUND);
            ht.Add("@EMP_ID", EMP_ID);
            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //更新還原提出核可、年獎對象及金額、支付狀態(年獎維護檔)
    public void updateAward_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //總金額
            StringBuilder sb_AMT = new StringBuilder();
            sb_AMT.Append(" , BONUS_TOTAL_AMOUNT = ( select isnull( sum(BONUS_AMT),0) from  TB_S_M_BONUS_D  ");
            sb_AMT.Append(" where BONUS_YEAR = @BONUS_YEAR ");
            sb_AMT.Append("  and BONUS_ROUND = @BONUS_ROUND ");
            sb_AMT.Append("  and CHG_STATUS <> @CHG_STATUS ");
            sb_AMT.Append("  ) ");
            //總人數
            StringBuilder sb_NUM = new StringBuilder();
            sb_NUM.Append(" , BONUS_TOTAL_DECIMAL = ( select   count(EMP_ID) from  TB_S_M_BONUS_D ");
            sb_NUM.Append(" where BONUS_YEAR = @BONUS_YEAR ");
            sb_NUM.Append("  and BONUS_ROUND = @BONUS_ROUND ");
            sb_NUM.Append("  and CHG_STATUS <> @CHG_STATUS ");
            sb_NUM.Append("  ) ");


            sb.Append(" update TB_S_M_BONUS_H ");
            sb.Append(" set RELEASE_DT = @RELEASE_DT ");
            sb.Append(sb_AMT);
            sb.Append(sb_NUM);
            sb.Append(" ,RELEASE_BY = @RELEASE_BY");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = getdate()");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR ");
            sb.Append("  and BONUS_ROUND = @BONUS_ROUND");

            //set值
            ht.Add("@RELEASE_DT", DBNull.Value);
            ht.Add("@RELEASE_BY", "");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@FREEZE_FLAG", "N");
            ht.Add("@CHG_STATUS", "D");
            //PK值
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@BONUS_ROUND", BONUS_ROUND);
            ht.Add("@EMP_ID", EMP_ID);

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }



}