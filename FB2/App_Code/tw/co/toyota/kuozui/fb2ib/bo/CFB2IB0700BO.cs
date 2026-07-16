using FB2.tw.co.toyota.kuozui.bo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// CFB2IB0700BO 的摘要描述
/// </summary>
public class CFB2IB0700BO:BaseService
{   
	public CFB2IB0700BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public bool IsNumeric(String strNumber)
    {
        Regex NumberPattern = new Regex("[^0-9.-]");
        return !NumberPattern.IsMatch(strNumber);
    }

    public string selectPara(String PAYMENT_DATE)
    {
        CFB2IB0700DAO dao = new CFB2IB0700DAO();
        return dao.selectPara(PAYMENT_DATE);
    }

    
    public string updateINS2_DETAIL(System.Web.UI.WebControls.GridView GridView1)
    {
        CFB2IB0700DAO dao = new CFB2IB0700DAO();
        dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
        dao.FUNC_ID = "FB2IB070";
        string PAYMENT_DATE = "", DATA_SOURCE = "", SALARY_TYPE = "", SALARY_ID = "", EMP_ID="",PAY_KIND="";
        try
        {
            BeginTransaction();
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                PAYMENT_DATE = GridView1.DataKeys[i].Values[0].ToString();
                DATA_SOURCE = GridView1.DataKeys[i].Values[1].ToString();
                SALARY_TYPE = GridView1.DataKeys[i].Values[2].ToString();
                SALARY_ID = GridView1.DataKeys[i].Values[3].ToString();
                EMP_ID = GridView1.DataKeys[i].Values[4].ToString();
                PAY_KIND = GridView1.DataKeys[i].Values[5].ToString();

                dao.ONE_TIME_AMOUNT = GridView1.Rows[i].Cells[4].Text.Replace(",", "");
                dao.ACCU_AMOUNT = GridView1.Rows[i].Cells[5].Text.Replace(",", "");
                dao.ACCU_OVER_AMOUNT = GridView1.Rows[i].Cells[6].Text.Replace(",", "");
                dao.INS_COST_BASE = GridView1.Rows[i].Cells[7].Text.Replace(",", "");
                dao.INS_COST = GridView1.Rows[i].Cells[8].Text.Replace(",", "");

                dao.updateINS2_DETAIL(PAYMENT_DATE, DATA_SOURCE, SALARY_TYPE, SALARY_ID, EMP_ID, PAY_KIND);
            }

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
        
        
    }




}