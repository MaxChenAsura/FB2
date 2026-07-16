using FB2.tw.co.toyota.kuozui.bo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// CFB2DD0100BO 的摘要描述
/// </summary>
public class CFB2DD0100BO : BaseService
{
	public CFB2DD0100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable getEMPData(string EMP_ID)
    {
        try
        {
            CFB2DD0100DAO dao = new CFB2DD0100DAO();
            return dao.getEMPData(EMP_ID);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string checkFirst(string application_no,string emp_id)
    {
        try
        {
            CFB2DD0100DAO dao = new CFB2DD0100DAO();


            return dao.checkFirst(application_no, emp_id);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public string addEmp(CFB2DD0100DAO dao)
    {
        try
        {
            DataTable tmp = dao.checkEmp_id(dao.EMP_ID);
            if (tmp.Rows.Count > 0)
                return "員工資料重覆";
            else
            {
                try
                {
                    BeginTransaction();

                    dao.insertEmp();

                    Commit();
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }


            }
            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string updateEmp(CFB2DD0100DAO dao)
    {
        try
        {
            BeginTransaction();

            dao.updateEmp();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public DataTable getEmp_data(string emp_id)
    {
        try
        {
            CFB2DD0100DAO dao = new CFB2DD0100DAO();            


            return dao.selectEmpData(emp_id);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public DataTable getCar_data(string emp_id)
    {
        try
        {
            CFB2DD0100DAO dao = new CFB2DD0100DAO();


            return dao.selectCarData(emp_id);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    //public DataTable getData1(string sortExpression,string emp_id)
    //{
    //    try
    //    {
    //        CFB2DD0100DAO dao = new CFB2DD0100DAO();


    //        return dao.getData1(sortExpression,emp_id);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw;
    //    }
    //}

    //取得每公里補助多少錢
    public string getCode_Val(string sub_cd)
    {
        try
        {
            CFB2DD0100DAO dao = new CFB2DD0100DAO();
            DataTable tmp = dao.getCode_Val(sub_cd);
            if (tmp.Rows.Count > 0)
                return tmp.Rows[0]["CODE_VAL1"].ToString();
            else
                return "";
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string isAllow(string EMP_ID)
    {
        string st1 = "", st2 = "", st3 = "", st4 = "", errormessage="";
        try
        {
            CFB2DD0100DAO dao = new CFB2DD0100DAO();
            DataTable dt = dao.getEMP_DATA(EMP_ID);
            if (dt.Rows.Count > 0)
            {
                st1 = dt.Rows[0]["BIRTH_DT"].ToString();
                st2 = dt.Rows[0]["LEVEL_CD"].ToString();
                st3 = dt.Rows[0]["TRANSFER_REASON"].ToString();
                st4 = dt.Rows[0]["JPN_CD"].ToString();
                
                DateTime dte = DateTime.Parse( Convert.ToString(DateTime.Parse(st1).Year+61)+"/1/1");//60歲隔年1/1
                DateTime dte2 = DateTime.Parse(DateTime.Now.ToShortDateString());


                if (st2 == "2A" || st2 == "2SA" && dte > dte2)
                {
                    errormessage += "此員工年齡未符合滿60歲的隔年一月一日，2A、2SA人員請領交通津貼資格\\n";                    
                }
                if (st3 == "B09" || st4 != "")//外派或外籍人員
                {
                    errormessage += "此員工為外派或外籍人員\\n";  
                }
                
            }

            return errormessage;    
           
        }
        catch (Exception)
        {
            throw;            
        }
    }

    public string insertTRANS_ALLOWANCE_D(CFB2DD0100DAO dao)
    {
        string st = "",st1="";
        try
        {
            //編號CL or KN
            st = dao.getCol();
            //查TABLE內是否已經有此編號開頭，且為今天日期的編號
            DataTable temp = dao.checkSEQ(st);
            //create 編號
            if (temp.Rows.Count > 0)
            {
                st1 = temp.Rows[0]["APPLICATION_NO"].ToString();
                if (st1 != "" && st1 != null)
                {
                    dao.D_APPLICATION_NO = st1.Substring(0, 9) + Convert.ToString(Convert.ToInt32(st1.Substring(9)) + 1).PadLeft(4, '0');                
                }                              
            }
            if (st1 == "" || st1 == null)
            { 
                string mon = Convert.ToString(DateTime.Now.Month).Length == 2 ? Convert.ToString(DateTime.Now.Month) : "0"+Convert.ToString(DateTime.Now.Month);
                string word = Convert.ToString(DateTime.Now.Year)+ mon;
                dao.D_APPLICATION_NO = st + "-" + word + "0001";
            }

            string dayMoney = "";
            dayMoney = dao.getPrice();
            //計算日津貼
            if (dao.D_TRANSPORT_CD == "01" || dao.D_TRANSPORT_CD == "02"
                || dao.D_TRANSPORT_CD == "05" || dao.D_TRANSPORT_CD == "06")//汽車or機車 
            {
                
                if (dao.D_SINGLE_TRIP == "Y")
                {
                    dao.D_DAILY_PAY = Convert.ToString(Convert.ToInt32(dayMoney) * Convert.ToInt32(dao.D_KILOMETER_AMOUNT) * 1);
                }
                else {
                    dao.D_DAILY_PAY = Convert.ToString(Convert.ToInt32(dayMoney) * Convert.ToInt32(dao.D_KILOMETER_AMOUNT)* 2);
                }                
            }

            if (dao.D_TRANSPORT_CD == "04" || dao.D_TRANSPORT_CD == "11")//大眾工具or自行車→大眾工具→交通車
            {                
                if (dao.D_SINGLE_TRIP == "Y")
                {
                    dao.D_DAILY_PAY = Convert.ToString(Convert.ToInt32(dao.D_FARE_PRICE) * 1);
                }
                else
                {
                    dao.D_DAILY_PAY = Convert.ToString(Convert.ToInt32(dao.D_FARE_PRICE) * 2);
                }
            }

            if (dao.D_TRANSPORT_CD == "07" || dao.D_TRANSPORT_CD == "08" || dao.D_TRANSPORT_CD == "09"
                || dao.D_TRANSPORT_CD == "10" || dao.D_TRANSPORT_CD == "12" || dao.D_TRANSPORT_CD == "13")
            {
                if (dao.D_SINGLE_TRIP == "Y")
                {
                    dao.D_DAILY_PAY = Convert.ToString(Convert.ToInt32(dayMoney) * Convert.ToInt32(dao.D_KILOMETER_AMOUNT) * 1 + Convert.ToInt32(dao.D_FARE_PRICE) * 1);
                    //dao.D_DAILY_PAY = Convert.ToString(Convert.ToInt32(dao.D_FARE_PRICE) * 1);
                }
                else
                {
                    dao.D_DAILY_PAY = Convert.ToString(Convert.ToInt32(dayMoney) * Convert.ToInt32(dao.D_KILOMETER_AMOUNT) * 2 + Convert.ToInt32(dao.D_FARE_PRICE) * 2);
                    //dao.D_DAILY_PAY = Convert.ToString(Convert.ToInt32(dao.D_FARE_PRICE) * 2);
                }
            }
            if (dao.D_DAILY_PAY == "" || dao.D_DAILY_PAY == null)
            {
                dao.D_DAILY_PAY = "0";
            }
            

            BeginTransaction();

            //新增明細檔
            dao.insertTRANS_ALLOWANCE_D();
            //update主檔
            dao.updateM_New();
            //將前一筆明細檔的生效日迄押上最新一筆的生效日期起
            dao.updateD_New();

            Commit();
            
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //刪除資料
    public string deleteData(string EMP_ID,string APPLICATION_NO,string flag)
    {
        CFB2DD0100DAO dao = new CFB2DD0100DAO();
        try
        {
            BeginTransaction();
            if (flag != "1")
            {
                //更新第二筆資料.生效日迄 = 9999/12/31
                dao.updateData(EMP_ID);

                //更新主檔資料
                dao.updateMain(EMP_ID);
            }
            else { 
                //刪除主檔資料 
                //先不刪除
                //dao.delMain(EMP_ID);
            }
            
            //刪除第一筆資料
            dao.delData(APPLICATION_NO, EMP_ID);

            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }

    }


    public string CheckPara(CFB2DD0100DAO dao) {
        string errormessage = "";        

        //輸入公里數
        if (dao.D_TRANSPORT_CD == "03" || dao.D_TRANSPORT_CD == "04"
            || dao.D_TRANSPORT_CD == "11" || dao.D_TRANSPORT_CD == "14")
        {
            if (!dao.D_KILOMETER_AMOUNT.Equals("") && dao.D_KILOMETER_AMOUNT != "0")
            {
                errormessage += "交通工具非選擇到汽、機車時，不可輸入公里數 ";
            }
        }
        else
        {
            //必須輸入公里數
            //if (dao.D_KILOMETER_AMOUNT.Equals("") || dao.D_KILOMETER_AMOUNT.Equals("0"))
            if (dao.D_KILOMETER_AMOUNT.Equals(""))
            {
                errormessage += "交通工具選擇到汽、機車時，公里數不可為空白 ";
            }
            else
            {
                if (dao.D_FACTORY_CD == "1")
                {//中壢
                    
                    if (Convert.ToInt32(dao.D_KILOMETER_AMOUNT) > Convert.ToInt32(dao.CL_KM))
                    {
                        errormessage += "輸入的公里數超出上限" + dao.CL_KM + " ";
                    }
                }
                if (dao.D_FACTORY_CD == "2")//觀音
                {
                    
                    if (Convert.ToInt32(dao.D_KILOMETER_AMOUNT) > Convert.ToInt32(dao.KN_KM))
                    {
                        errormessage += "輸入的公里數超出上限" + dao.KN_KM + " ";
                    }
                }
            }
        }

        //輸入票價
        if (dao.D_TRANSPORT_CD == "01" || dao.D_TRANSPORT_CD == "02"
            || dao.D_TRANSPORT_CD == "03" || dao.D_TRANSPORT_CD == "05"
            || dao.D_TRANSPORT_CD == "06" || dao.D_TRANSPORT_CD == "14")
        {
            if (!dao.D_FARE_PRICE.Equals("") && dao.D_FARE_PRICE != "0")
            {
                errormessage += "交通工具非選擇到大眾工具時，票價不可輸入 ";
            }
        }
        else
        {
            //必須輸入票價
            //if (dao.D_FARE_PRICE.Equals("") || dao.D_FARE_PRICE.Equals("0"))
            if (dao.D_FARE_PRICE.Equals(""))
            {
                errormessage += "交通工具選擇到大眾工具時，票價不可為空白 </br>";
            }
            else
            {
                if (dao.D_FACTORY_CD == "1")//中壢
                {
                    if (Convert.ToInt32(dao.D_FARE_PRICE) > Convert.ToInt32(dao.CL_FR))
                    {
                        errormessage += "票價上限為" + dao.CL_FR + "元，輸入的票價不可大於該廠區的票價上限值 ";
                    }
                }
                if (dao.D_FACTORY_CD == "2")//觀音
                {
                    if (Convert.ToInt32(dao.D_FARE_PRICE) > Convert.ToInt32(dao.KN_FR))
                    {
                        errormessage += "票價上限為" + dao.KN_FR + "元，輸入的票價不可大於該廠區的票價上限值 ";
                    }
                }
            }
        }        

        return errormessage;
        
    }

}