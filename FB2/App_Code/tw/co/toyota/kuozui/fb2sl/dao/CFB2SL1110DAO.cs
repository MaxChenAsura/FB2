using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// CFB2SL1110DAO 的摘要描述
/// </summary>
/// 
public class CFB2SL1110DAO : BaseDAO
{
    public int count { get; set; }
    public Int64 RowNumber { get; set; }
    //screen PARA
    public string DATA_YEAR { get; set; }
  
    //insert PARA
    public string IDENTITY_KIND { get; set; }
    public string LICENSE_ID { get; set; }
    public string LICENSE_ID_B { get; set; }
    public string AMOUNT { get; set; }
    public string EMP_NAME { get; set; }
    public string FAMILY_NAME { get; set; } 
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SL1110DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public void Delete_TB_S_R_IMX_LABOR_UPLOAD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_R_IMX_LABOR_UPLOAD where DATA_YEAR = @DATA_YEAR");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void Insert_TB_S_R_IMX_LABOR_UPLOAD(string LICENSE_ID, string AMOUNT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"insert into  TB_S_R_IMX_LABOR_UPLOAD 
                        (DATA_YEAR,LICENSE_ID,AMOUNT,CREATED_BY,CREATED_DT,
                        UPDATED_BY,UPDATED_DT,FUNC_ID )
                        values 
                        (@DATA_YEAR,@LICENSE_ID,@AMOUNT,@CREATED_BY,getdate(),
                        @UPDATED_BY,getdate(),@FUNC_ID )                         
            ");
            
            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@LICENSE_ID", LICENSE_ID.Trim());
            ht.Add("@AMOUNT", AMOUNT.Trim());
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

    public void deleteExcelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_R_IMX_HEALTH_UPLOAD where DATA_YEAR = @DATA_YEAR");

            ht.Add("@DATA_YEAR", DATA_YEAR);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void addExcelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"insert into  TB_S_R_IMX_HEALTH_UPLOAD 
                        (DATA_YEAR,IDENTITY_KIND,LICENSE_ID,LICENSE_ID_B,AMOUNT,
                        EMP_NAME,FAMILY_NAME,CREATED_BY,CREATED_DT,UPDATED_BY,
                        UPDATED_DT,FUNC_ID )
                        values 
                        (@DATA_YEAR,@IDENTITY_KIND,@LICENSE_ID,@LICENSE_ID_B,@AMOUNT,
                        @EMP_NAME,@FAMILY_NAME,@CREATED_BY,getdate(),@UPDATED_BY,
                        getdate(),@FUNC_ID )                         
            ");


            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND.Trim());
            ht.Add("@LICENSE_ID", LICENSE_ID.Trim());
            ht.Add("@LICENSE_ID_B", LICENSE_ID_B.Trim());
            ht.Add("@AMOUNT", AMOUNT.Trim());
            ht.Add("@EMP_NAME", EMP_NAME.Trim());
            ht.Add("@FAMILY_NAME", FAMILY_NAME.Trim());
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
}