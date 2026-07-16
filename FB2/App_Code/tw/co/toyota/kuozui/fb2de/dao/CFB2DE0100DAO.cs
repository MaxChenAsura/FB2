using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// wfd2de 的摘要描述
/// </summary>
public class CFB2DE0100DAO : BaseDAO
{
    public CFB2DE0100DAO()
    {

    }
    //取得COMPANY_CD
    public string getCOMPANY_CD(string emp_id)
    {
        try
        {
            string COMPANY_CD = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string sql = " Select * from TB_H_M_EMP where EMP_ID=@EMP_ID";
            sb.Append(sql);
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                COMPANY_CD = dt.Rows[0]["COMPANY_CD"].ToString();
            }
            return COMPANY_CD;
        }
        catch
        {
            throw;
        }
    }
    //取得TB_D_M_RES_PARA 的DT
    public DataTable getTB_D_M_RES_PARA(string COMPANY_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string sql = " Select * from TB_D_M_RES_PARA where COMPANY_CD=@COMPANY_CD";
            sb.Append(sql);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }

    }
    //更新資料
    public int update_TB_D_M_RES_PARA(string emp_company_cd, string getBfAountValue, string getDnAmountValue, string getBrStartValue, string getBrEndValue, string getDnStartValue,
                                       string getDnEndValue, string getLastBrTimeValue, string getLastDnTimeValue, string getCourseDnTimeValue, string emp_id, string func_id, string md_start, string md_end)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string sql = @"  Update TB_D_M_RES_PARA set BF_AMOUNT=@BF_AMOUNT,DN_AMOUNT=@DN_AMOUNT,
                         BR_START=@BR_START,BR_END=@BR_END,DN_START=@DN_START,DN_END=@DN_END, 
                         LAST_BR_TIME=@LAST_BR_TIME,LAST_DN_TIME=@LAST_DN_TIME,COURSE_DN_TIME=@COURSE_DN_TIME, 
                         UPDATED_BY=@UPDATED_BY,UPDATED_DT=@UPDATED_DT,FUNC_ID=@FUNC_ID,MD_START=@MD_START,MD_END=@MD_END where COMPANY_CD=@COMPANY_CD ";
            sb.Append(sql);
            ht.Add("@COMPANY_CD", emp_company_cd);
            ht.Add("@BF_AMOUNT", getBfAountValue);
            ht.Add("@DN_AMOUNT", getDnAmountValue);
            ht.Add("@BR_START", getBrStartValue);
            ht.Add("@BR_END", getBrEndValue);
            ht.Add("@DN_START", getDnStartValue);
            ht.Add("@DN_END", getDnEndValue);
            ht.Add("@LAST_BR_TIME", getLastBrTimeValue);
            ht.Add("@LAST_DN_TIME", getLastDnTimeValue);
            ht.Add("@COURSE_DN_TIME", getCourseDnTimeValue);
            ht.Add("@UPDATED_BY", emp_id);
            ht.Add("@UPDATED_DT", DateTime.Now);
            ht.Add("@FUNC_ID", func_id);
            ht.Add("@MD_START", md_start);
            ht.Add("@MD_END", md_end);


            return dbConn.Execute(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //新增資料
    public int InsertData_TB_D_M_RES_PARA(string emp_company_cd, string getBfAountValue, string getDnAmountValue, string getBrStartValue, string getBrEndValue, string getDnStartValue,
                                       string getDnEndValue, string getLastBrTimeValue, string getLastDnTimeValue, string getCourseDnTimeValue, string emp_id, string func_id, string md_start, string md_end)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string sql = @"  INSERT INTO TB_D_M_RES_PARA( COMPANY_CD,BF_AMOUNT,DN_AMOUNT,BR_START,BR_END,DN_START, 
                         DN_END,LAST_BR_TIME,LAST_DN_TIME,COURSE_DN_TIME,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,MD_START,MD_END)
                         values(@COMPANY_CD,@BF_AMOUNT,@DN_AMOUNT,@BR_START,@BR_END,@DN_START,@DN_END,@LAST_BR_TIME,
                         @LAST_DN_TIME,@COURSE_DN_TIME,@CREATED_BY,@CREATED_DT,@UPDATED_BY,@UPDATED_DT,@FUNC_ID,@MD_START,@MD_END)";
            sb.Append(sql);
            ht.Add("@COMPANY_CD", emp_company_cd);
            ht.Add("@BF_AMOUNT", getBfAountValue);
            ht.Add("@DN_AMOUNT", getDnAmountValue);
            ht.Add("@BR_START", getBrStartValue);
            ht.Add("@BR_END", getBrEndValue);
            ht.Add("@DN_START", getDnStartValue);
            ht.Add("@DN_END", getDnEndValue);
            ht.Add("@LAST_BR_TIME", getLastBrTimeValue);
            ht.Add("@LAST_DN_TIME", getLastDnTimeValue);
            ht.Add("@COURSE_DN_TIME", getCourseDnTimeValue);
            ht.Add("@CREATED_BY", emp_id);
            ht.Add("@CREATED_DT", DateTime.Now);
            ht.Add("@UPDATED_BY", emp_id);
            ht.Add("@UPDATED_DT", DateTime.Now);
            ht.Add("@FUNC_ID", func_id);
            ht.Add("@MD_START", md_start);
            ht.Add("@MD_END", md_end);

            return dbConn.Execute(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
}
