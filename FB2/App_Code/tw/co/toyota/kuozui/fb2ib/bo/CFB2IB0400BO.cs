using FB2.tw.co.toyota.kuozui.bo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// CFB2IB0400BO 的摘要描述
/// </summary>
public class CFB2IB0400BO : BaseService
{
	public CFB2IB0400BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable getYM()
    {        
        try
        {
            CFB2IB0400DAO dao = new CFB2IB0400DAO();
            DataTable dt = dao.getYM();
           
            return dt;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public string checkSalary(string SALARY_YM)
    {
        string errormessage = "";
        try
        {
            CFB2IB0400DAO dao = new CFB2IB0400DAO();
            DataTable dt = dao.checkSalary(SALARY_YM);

            if (dt.Rows.Count == 0)
            {
                errormessage += "目前沒有可用的薪資可供計算，請檢核月薪是否已經計算完畢\\n";
                return errormessage;                
            }

            return errormessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string checkCOMPANY_BILL(string SALARY_YM)
    {
        string errormessage = "";
        try
        {
            CFB2IB0400DAO dao = new CFB2IB0400DAO();
            DataTable dt = dao.checkCOMPANY_BILL(SALARY_YM);

            if (dt.Rows.Count == 0)
            {
                errormessage += "目前沒有可用的雇主其他非固定薪，請重新輸入計算年月\\n";
                return errormessage;
            }

            return errormessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public DataTable checkSALARY_MONTH(string SALARY_YM)
    {       
            CFB2IB0400DAO dao = new CFB2IB0400DAO();
            return dao.checkSALARY_MONTH(SALARY_YM);            
    }

    public string getBillData(CFB2IB0400DAO dao)
    {

        try
        {
            dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.FUNC_ID = "FB2IB040";

            //取得管理部門
            dao.getManageDept();

            //取得補充保險費率－雇主
            dao.getInsPara();

            /*1.有關抓取回存AS400資料的相關程式部分(程式、SP)，以後將不再進行 20160215
            //將AS400資料 存到廠商健保補充保費扣繳檔
            DataTable dt = dao.selectG50();
            BeginTransaction();

            try
            {
                if (dt.Rows.Count > 0)
                {
                    //刪除廠商健保補充保費扣繳檔資料
                    dao.deleteVANDOR_BILL();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dao.BILL_NO = dt.Rows[i]["G5CNO"].ToString().Trim();
                        dao.INVOICE_NO = dt.Rows[i]["G5CSEQ"].ToString().Trim();
                        dao.ACC_NO = dt.Rows[i]["G5CSE1"].ToString().Trim();
                        dao.DEPT_BILL_NO = dt.Rows[i]["G5CDNO"].ToString().Trim();
                        dao.BILL_CD = dt.Rows[i]["G5CTYP"].ToString().Trim();
                        dao.PLUS_MINUS_CD = dt.Rows[i]["G5CDC"].ToString().Trim();
                        dao.BUDGET_ACC = dt.Rows[i]["G5BCC"].ToString().Trim();
                        dao.ACCOUNTING_ACC = dt.Rows[i]["G5CACC"].ToString().Trim();
                        dao.MONEY_CD = dt.Rows[i]["G5CSCD"].ToString().Trim();
                        dao.CASE_CD = dt.Rows[i]["G5CASE"].ToString().Trim();
                        dao.NT_AMOUNT = dt.Rows[i]["G5AMT1"].ToString().Trim();
                        dao.INS2_INSTEAD_AMOUNT = dt.Rows[i]["G5AMT2"].ToString().Trim();
                        dao.SMMARY = dt.Rows[i]["G5MEMO"].ToString().Trim();
                        dao.GET_MONEY_DT = dt.Rows[i]["G5ATDT"].ToString().Trim();
                        if (dao.GET_MONEY_DT != "" && dao.GET_MONEY_DT.Length == 7)
                        {
                            dao.GET_MONEY_DT = Convert.ToString(Convert.ToInt32(dao.GET_MONEY_DT.Substring(0, 3)) + 1911) + "/" + dao.GET_MONEY_DT.Substring(3, 2) + "/" + dao.GET_MONEY_DT.Substring(5, 2);
                        }
                        dao.PAY_TO = dt.Rows[i]["G5VRCD"].ToString().Trim();
                        dao.PAY_TO_LICENSE_ID = dt.Rows[i]["G5PID"].ToString().Trim();
                        dao.WANT_DATE = dt.Rows[i]["G5HDAT"].ToString() == "0" ? "" : dt.Rows[i]["G5HDAT"].ToString().Trim();
                        if (dt.Rows[i]["G5TRDT"].ToString() != "" && dt.Rows[i]["G5TRTM"].ToString() != "")
                        {
                            dao.SWITCH_TO = changeDate(dt.Rows[i]["G5TRDT"].ToString(), dt.Rows[i]["G5TRTM"].ToString());
                        }
                        else
                        {
                            dao.SWITCH_TO = "";
                        }

                        //存到Table
                        dao.insertVANDOR_BILL();
                    }
                }
                Commit();
            }
            catch (Exception Ex)
            {
                RollBack();
                return Ex.Message;
            }
            */

            //取得員工月薪總額
            DataTable dt1 = dao.selectMonthData();

            BeginTransaction();
            dao.deleteTB_S_R_INS2_SALARY_MONTH();

            if(dt1.Rows.Count >0 ){
                for (int i = 0; i < dt1.Rows.Count;i++ )
                {
                    dao.SALARY_YM_M = dt1.Rows[i]["SALARY_DT"].ToString().Trim();
                    dao.ACC_CD = dt1.Rows[i]["ACC_CD"].ToString().Trim();
                    dao.ACC_WS = dt1.Rows[i]["ACC_WS"].ToString().Trim();
                    dao.SALARY_DEPT = dt1.Rows[i]["ACC_DEPT_NO"].ToString().Trim();
                    dao.PLANT_CD = dt1.Rows[i]["PLANT_CD"].ToString().Trim();
                    dao.CAR_KIND = dt1.Rows[i]["CAR_TYPE"].ToString().Trim();
                    dao.COST_DEPT_NO = dt1.Rows[i]["COST_DEPT_NO"].ToString().Trim();
                    dao.BUDGET_DEPT_NO = dt1.Rows[i]["BUDGET_DEPT_NO"].ToString().Trim();
                    dao.MONTH_S_TOTAL = dt1.Rows[i]["fixTotal"].ToString().Trim() == "" ? "0" : dt1.Rows[i]["fixTotal"].ToString().Trim();
                    dao.FLOAT_S_TOTAL = dt1.Rows[i]["floatTotal"].ToString().Trim() == "" ? "0" : dt1.Rows[i]["floatTotal"].ToString().Trim();
                    dao.TOTAL_INS = dt1.Rows[i]["INS_AMT"].ToString().Trim() == "" ? "0" : dt1.Rows[i]["INS_AMT"].ToString().Trim();
                    dao.BOSS_TAX = dt1.Rows[i]["BOSS_TAX"].ToString().Trim() == "" ? "0" : dt1.Rows[i]["BOSS_TAX"].ToString().Trim();
                    //dao.BOSS_OTHER_SALARY = dt1.Rows[i]["BOSS_OTHER_SALARY"].ToString().Trim() == "" ? "0" : dt1.Rows[i]["BOSS_OTHER_SALARY"].ToString().Trim();
                    
                    dao.INS2_BASE = Convert.ToString(Convert.ToInt32(dao.FLOAT_S_TOTAL) + Convert.ToInt32(dao.MONTH_S_TOTAL) + 0 +
                                    Convert.ToInt32(dao.BOSS_TAX)  -
                                    Convert.ToInt32(dao.TOTAL_INS));
                    dao.INS2_COST = Convert.ToString(Math.Round(Convert.ToDouble(Convert.ToInt32(dao.INS2_BASE) * Convert.ToDouble(dao.INS_RATE_COMP) / 100)));
                    //存到補充保費負擔部門月度檔
                    dao.insertM1Data();

                }
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

    public static string changeDate(string ymd,string hms)
    {
        string result = "";
        try {
            string hh = "";
            string min = "";
            string ss = "";

            if (ymd.Length == 7)
            {
                string yy = Convert.ToString(Convert.ToInt32(ymd.Substring(0, 3))+1911);
                string mm = ymd.Substring(3, 2);
                string dd = ymd.Substring(5, 2);
                
                if (hms.Length == 6)
                {
                    hh = hms.Substring(0, 2);
                    min = hms.Substring(2, 2);
                    ss = hms.Substring(4, 2);
                }
                if (hms.Length == 5)
                {
                    hh = hms.Substring(0, 1);
                    min = hms.Substring(1, 2);
                    ss = hms.Substring(3, 2);
                }
                result = yy + "/" + mm + "/" + dd + " " + hh + ":" + min + ":" + ss;
            }
            else if (ymd.Length == 6)
            {
                string yy = ymd.Substring(0, 2);
                string mm = ymd.Substring(2, 2);
                string dd = ymd.Substring(4, 2);

                if (hms.Length == 6)
                {
                    hh = hms.Substring(0, 2);
                    min = hms.Substring(2, 2);
                    ss = hms.Substring(4, 2);
                }
                if (hms.Length == 5)
                {
                    hh = hms.Substring(0, 1);
                    min = hms.Substring(1, 2);
                    ss = hms.Substring(3, 2);
                }
                result = yy + "/" + mm + "/" + dd + " " + hh + ":" + min + ":" + ss;
            }           


            return result;

        }catch(Exception){
            throw;
        }
        
    }


}