using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;

/// <summary>
/// CFB2SE1100BO 的摘要描述
/// </summary>
public class CFB2SE1100BO : BaseService
{
	public CFB2SE1100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    //考核調薪試算
    public bool SalaryUpComputer(string paEffect_ym)
    {
        bool successed = true;
        try
        {
            CFB2SE1100DAO fbSE = new CFB2SE1100DAO();
            BeginTransaction();
            fbSE.SalaryUpComputer_dao(paEffect_ym);
          
            Commit();
            return successed;
        }
        catch (Exception)
        {
            throw;
        }
    }
    //檢查資料是否鎖定
    public string CheckReleas( string effect_ym)
    {
        string rtnmessage = "0";
        try
        {
            CFB2SE1100DAO fb2SE = new CFB2SE1100DAO();

            DataTable dt = fb2SE.get_TB_S_M_2BSALARY_SET_H(effect_ym);
            if ((int)dt.Rows[0]["RESULTCOUNT"] == 0)
            {
                rtnmessage = "此生效年月之2B以上本薪調整表單未設定。 \\n";
                
                return rtnmessage;
            }
            dt.Clear();
            dt = fb2SE.get_TB_S_M_SALARYSET_H(effect_ym);
            if ((int)dt.Rows[0]["RESULTCOUNT"] == 0)
            {
                rtnmessage = "此生效年月之3A以下調薪金額表單未設定。 \\n";
                return rtnmessage;
            }
            dt.Clear();
            dt = fb2SE.getS_M_SALARY_ADJ_H(effect_ym);
            if ((int)dt.Rows[0]["RESULTCOUNT"] > 0)
            {
                rtnmessage = "此生效年月已提出申請不允重新計算。 \\n";
            }
            dt.Clear();
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public DataTable getNoDataEmp_Id(string year, string firDay, string midDay)
    {        
        try
        {
            CFB2SE1100DAO fb2SE = new CFB2SE1100DAO();

            DataTable dt = fb2SE.getNoDataEmp_Id(year, firDay, midDay);
            return dt;
            
        }
        catch (Exception)
        {
            throw;
        }
    }


}