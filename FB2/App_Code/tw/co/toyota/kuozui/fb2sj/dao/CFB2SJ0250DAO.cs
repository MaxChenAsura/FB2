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
/// CFB2SJ0250DAO 的摘要描述
/// </summary>
public class WFB2SJ0250DAO : BaseDAO
{
    public WFB2SJ0250DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string FREEZE_FLAG { get; set; } //凍結註記

    public string EMP_ID { get; set; } //工號
    public string SCORE_DEPT { get; set; } //部門考績
    public string SCORE_FINAL { get; set; } //最終考績
    public string SCORE_FLAG { get; set; }  //考績異動註記
    	


    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    

    //取得最新的年度及類型
    public  void getAssessData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"SELECT  
            top 1 ASSESS_YEAR,ASSESS_TYPE 
            FROM TB_S_M_ASSESS_DATA 
            ORDER BY assess_year desc,assess_type desc ");

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                ASSESS_YEAR = (string)dt.Rows[0]["ASSESS_YEAR"];
                ASSESS_TYPE = (string)dt.Rows[0]["ASSESS_TYPE"];
            }
            else {
                ASSESS_YEAR ="";
                ASSESS_TYPE ="";
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
            sb.Append(@"SELECT  
            FREEZE_FLAG
            FROM TB_S_M_ASSESS_DATA 
            where ASSESS_YEAR= @ASSESS_YEAR
            and ASSESS_TYPE = @ASSESS_TYPE
            ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);

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

    //取得能力考績的範圍S~J
    public string getScore_Str(string type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" DECLARE @Columns VARCHAR(MAX)='' ");
            if(type=="1")
                sb.Append(@" SELECT @Columns = @Columns + SUB_CD FROM TB_9_M_COMM_D WHERE sys_cd='SJ' AND main_cd='ASSESS_SCORE' AND code_val1='Y' AND IS_VALID='Y' ");
            if(type=="2")
                sb.Append(@" SELECT @Columns = @Columns + SUB_CD FROM TB_9_M_COMM_D WHERE sys_cd='SJ' AND main_cd='ASSESS_SCORE' AND code_val2='Y' AND IS_VALID='Y' ");

            sb.Append(@" SELECT @Columns AS [ASSESS_SCORE] ");

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                return  (string)dt.Rows[0]["ASSESS_SCORE"];
            }
            else
            {
                return "";
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得業績考績的範圍A~E
    public string getScore_2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" DECLARE @Columns VARCHAR(MAX)='' ");
            sb.Append(@" SELECT @Columns = @Columns + SUB_CD FROM TB_9_M_COMM_D WHERE sys_cd='SJ' AND main_cd='ASSESS_SCORE' AND code_val2='Y' AND IS_VALID='Y' ");
            sb.Append(@" SELECT @Columns AS [ASSESS_SCORE_2] ");

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                return (string)dt.Rows[0]["ASSESS_SCORE_2"];
            }
            else
            {
                return "";
            }
        }
        catch (Exception)
        {

            throw;
        }
    }


    //考績一括更新
    public void updateAssessScore_ALL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_S_M_ASSESS_TARGET ");
            sb.Append(" set SCORE_DEPT =  @SCORE_DEPT  ");
            sb.Append(" ,SCORE_FINAL = @SCORE_FINAL");
            sb.Append(" ,SCORE_FLAG = @SCORE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT =  getdate() ");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");
            sb.Append("  and EMP_ID = @EMP_ID");

            //set值
            ht.Add("@SCORE_DEPT", SCORE_DEPT);
            ht.Add("@SCORE_FINAL", SCORE_FINAL);
            ht.Add("@SCORE_FLAG", SCORE_FLAG);
            //PK值
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
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