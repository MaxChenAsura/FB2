using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2IB0300DAO 的摘要描述
/// </summary>
public class CFB2IB0300DAO : BaseDAO
{
    //screen PARA
    public string G9YM { get; set; }
    //insert PARA
    public string YM { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string LICENSE_ID { get; set; }
    public string PAYMENT_DATE { get; set; }
    public string BILL_NO { get; set; }
    public string ITEM_SEQ { get; set; }
    public string VCHID { get; set; }
    public string VCH_NAME { get; set; }
    public string TAX_FORMAT { get; set; }
    public string CODE_CD { get; set; }
    public string AMOUNT { get; set; }
    public string INS_COST { get; set; }   
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

	public CFB2IB0300DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string ym, string emp_id,
                             string license_id, string tax_format)
    {
        try
        {
            //if (sortExpression.Contains("SALARY_YM"))
            //    sortExpression = sortExpression.Replace("SALARY_YM", "t.SALARY_YM");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber, ");
            sb.AppendLine("              YM,EMP_ID,EMP_NAME,LICENSE_ID,PAYMENT_DATE,BILL_NO,ITEM_SEQ,VCHID,VCH_NAME,TAX_FORMAT,CODE_CD,AMOUNT,INS_COST ");            
            sb.AppendLine("        from TB_S_R_INS2_UPD_DATA                                                                                     ");
            //sb.AppendLine("        left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD  ");           
            sb.AppendLine(" where YM = @YM                                                                                                      ");

            if (emp_id != "")
            {
                sb.AppendLine(" and EMP_ID = @EMP_ID  ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (license_id != "")
            {
                sb.AppendLine(" and LICENSE_ID = @LICENSE_ID  ");
                ht.Add("@LICENSE_ID", license_id.ToUpper());
            }
            if (tax_format != "")
            {
                sb.AppendLine(" and TAX_FORMAT = @TAX_FORMAT ");
                ht.Add("@TAX_FORMAT", tax_format);
            }           

            sb.AppendLine("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@YM", ym.Replace("/",""));
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int getCount(int startRowIndex, int maximumRows, string ym, string emp_id,
                             string license_id, string tax_format)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine(" from TB_S_R_INS2_UPD_DATA                                                                                      ");
            sb.AppendLine(" where YM = @YM                                                                                                      ");

            if (emp_id != "")
            {
                sb.AppendLine(" and EMP_ID = @EMP_ID  ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (license_id != "")
            {
                sb.AppendLine(" and LICENSE_ID = @LICENSE_ID  ");
                ht.Add("@LICENSE_ID", license_id.ToUpper());
            }
            if (tax_format != "")
            {
                sb.AppendLine(" and TAX_FORMAT = @TAX_FORMAT ");
                ht.Add("@TAX_FORMAT", tax_format);
            }

            ht.Add("@YM", ym.Replace("/", ""));
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

    public DataTable selectData(string YM)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * from TB_S_M_INS2_COMPANY_BILL");
            sb.Append(" where YM = @YM");

            ht.Add("@YM", YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public void insertCOMPANY_BILL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            sb.Append(" insert into TB_S_R_INS2_UPD_DATA");
            sb.Append(" (YM,EMP_ID,EMP_NAME,LICENSE_ID,PAYMENT_DATE,");
            sb.Append(" BILL_NO,ITEM_SEQ,VCHID,VCH_NAME,TAX_FORMAT,");
            sb.Append(" CODE_CD,AMOUNT,INS_COST,CREATED_BY,CREATED_DT,");
            sb.Append(" UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values(@YM,@EMP_ID,@EMP_NAME,@LICENSE_ID,@PAYMENT_DATE,");
            sb.Append(" @BILL_NO,@ITEM_SEQ,@VCHID,@VCH_NAME,@TAX_FORMAT,");
            sb.Append(" @CODE_CD,@AMOUNT,@INS_COST,@CREATED_BY,getdate(),");
            sb.Append(" @UPDATED_BY,getdate(),@FUNC_ID)");
                       
            ht.Add("@YM", YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@PAYMENT_DATE", PAYMENT_DATE);
            ht.Add("@BILL_NO", BILL_NO);
            ht.Add("@ITEM_SEQ", ITEM_SEQ);
            ht.Add("@VCHID", VCHID);
            ht.Add("@VCH_NAME", VCH_NAME);
            ht.Add("@TAX_FORMAT", TAX_FORMAT);
            ht.Add("@CODE_CD", CODE_CD);           
            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@INS_COST", INS_COST);           
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

    public DataTable selectG90()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
        try
        {             
            
            ////查詢AS400資料
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "Select G9YM,G9CDNO,G9CNO,G9BCC,G9CACC,G9ACCN,G9CDC,G9CSCD,G9AMT1,G9MEMO,G9VRCD,G9PID,G9NAME,G9CASE,G9HDAT,G9ICFM";
            ocomm.CommandText += " from DATLIB.DB3KG90";
            ocomm.CommandText += " where G9YM = ?";

            ocomm.Parameters.AddWithValue("", G9YM);


            DataTable tmp = odbc.getDataTable(ocomm);



            return tmp;

        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            odbc.connectionClose();
        }
    }

    public void deleteCOMPANY_BILL(string dYM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();



            //sb.Append(" delete from TB_S_M_INS2_COMPANY_BILL");
            //sb.Append(" where YM = @dYM");
            sb.Append(" delete from TB_S_R_INS2_UPD_DATA");
            sb.Append(" where YM = @dYM");
            ht.Add("@dYM", dYM);          



            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    

}