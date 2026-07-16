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
public class CFB2DE0500DAO : BaseDAO
{
    public string MANAGER_YM { get; set; }
    public string PLANT_CD { get; set; }
    public string COST_DEPT_NO { get; set; }
    //public string BOND_FOR { get; set; }
    public string L_AMOUNT { get; set; }
    public string L_PRICE { get; set; }
    public string G1_AMOUNT { get; set; }
    public string G1_PRICE { get; set; }
    public string G2_AMOUNT { get; set; }
    public string G2_PRICE { get; set; }
    public string G3_AMOUNT { get; set; }
    public string G3_PRICE { get; set; }
    public string E1_AMOUNT { get; set; }
    public string E1_PRICE { get; set; }    
    public string G_TOTAL_AMOUNT { get; set; }
    public string G_TOTAL_PRICE { get; set; }   
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2DE0500DAO()
    {


    }
    public void del_Old_Res_Bond()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("delete from TB_D_R_RES_BOND_DTL");
            sb.Append(" where MANAGER_YM = @MANAGER_YM and PLANT_CD = @PLANT_CD");

            ht.Add("@MANAGER_YM", MANAGER_YM);
            ht.Add("@PLANT_CD", PLANT_CD);

            dbConn.Query(sb, ht);
            
        }
        catch
        {
            throw;
        }
    }

    public DataTable select_DEPT_ACC()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select count(*) rows from TB_H_M_DEPT_ACC");
            sb.Append(" where COST_DEPT_NO = @COST_DEPT_NO ");

            ht.Add("@COST_DEPT_NO", COST_DEPT_NO);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public void insert_Detail()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            sb.Append(" insert into TB_D_R_RES_BOND_DTL");
            sb.Append(" (MANAGER_YM,MANAGER_UNIT,PLANT_CD,L_AMOUNT,L_PRICE,G1_AMOUNT,G1_PRICE,G2_AMOUNT,G2_PRICE,G3_AMOUNT,G3_PRICE,E1_AMOUNT,E1_PRICE,");
            sb.Append(" G_TOTAL_AMOUNT,G_TOTAL_PRICE,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values(@MANAGER_YM, @COST_DEPT_NO,@PLANT_CD, @L_AMOUNT, @L_PRICE,@G1_AMOUNT,@G1_PRICE,@G2_AMOUNT,@G2_PRICE,@G3_AMOUNT,@G3_PRICE,@E1_AMOUNT,@E1_PRICE,");
            sb.Append(" @G_TOTAL_AMOUNT,@G_TOTAL_PRICE,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID)");            

            ht.Add("@MANAGER_YM", MANAGER_YM);
            ht.Add("@COST_DEPT_NO", COST_DEPT_NO);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@L_AMOUNT", L_AMOUNT);
            ht.Add("@L_PRICE", L_PRICE);
            ht.Add("@G1_AMOUNT", G1_AMOUNT);
            ht.Add("@G1_PRICE", G1_PRICE);
            ht.Add("@G2_AMOUNT", G2_AMOUNT);
            ht.Add("@G2_PRICE", G2_PRICE);
            ht.Add("@G3_AMOUNT", G3_AMOUNT);
            ht.Add("@G3_PRICE", G3_PRICE);
            ht.Add("@E1_AMOUNT", E1_AMOUNT);
            ht.Add("@E1_PRICE", E1_PRICE);
            ht.Add("@G_TOTAL_AMOUNT", G_TOTAL_AMOUNT);
            ht.Add("@G_TOTAL_PRICE", G_TOTAL_PRICE);
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

    public DataTable select_COST_DEPT_NO()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select distinct(COST_DEPT_NO) COST_DEPT_NO from TB_H_M_DEPT_ACC");
            sb.Append(" order by COST_DEPT_NO ");

            
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
}