using FB2.tw.co.toyota.kuozui.bo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// CFB2DD0200BO 的摘要描述
/// </summary>
public class CFB2DD0200BO : BaseService
{
	public CFB2DD0200BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string getMaxDate()
    {
        string MANAGER_YM = "";
        try
        {
            CFB2DD0200DAO dao = new CFB2DD0200DAO();
            DataTable dt = dao.getMaxDate();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MANAGER_YM = dt.Rows[i]["MANAGER_YM"].ToString();                    
                }

            }

            return MANAGER_YM;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string getManagerDT(string MANAGER_YM)
    {
        string TAKE_OUT_DT = "", STATUS = "", errormessage = "";
        try
        {
            CFB2DD0200DAO dao = new CFB2DD0200DAO();
            DataTable dt = dao.getManagerDT(MANAGER_YM);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    TAKE_OUT_DT = dt.Rows[i]["TAKE_OUT_DT"].ToString().Replace("-", "/");
                    STATUS = dt.Rows[i]["STATUS"].ToString();

                    if (TAKE_OUT_DT != "9999/12/31" && STATUS == "Y")
                    {
                        errormessage = "0";
                        return errormessage;
                    }  
                }
                             
            }

            return errormessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string execTrans_Money(string MANAGER_YM)
    {
        string EMP_ID = "", JOIN_DT = "",LEAVE_DT = "", lastMonth = "" , minDT="";
        Boolean b = false;
        try
        {
            //取得前月份的年月
            lastMonth = getLastMonth(MANAGER_YM);

            CFB2DD0200DAO dao = new CFB2DD0200DAO();            
            dao.MANAGER_YM = MANAGER_YM;
            DataTable dt = dao.getEMP(MANAGER_YM);
            dao.lastMonth = lastMonth;
            dao.CREATED_BY = SessionHandle.Current.emp_id;
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2DD020";

            //如上個月已經有資料，放到List中
            List<string> EMPs = new List<string>();
            DataTable dt_Last = dao.getAllLastMonth();
            if (dt_Last.Rows.Count > 0)
	        {
                for (int i = 0; i < dt_Last.Rows.Count; i++)
                {
                    EMPs.Add(dt_Last.Rows[i]["EMP_ID"].ToString());
                }  
	        }           
           

            if (dt.Rows.Count > 0)
            {
                BeginTransaction();

                //刪除相同管理年月、工號 舊資料 
                dao.delOld();

                //刪除薪資月結控制檔
                dao.deleteSALARY_MONTH_CTRL();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    EMP_ID = dt.Rows[i]["EMP_ID"].ToString();
                    JOIN_DT = dt.Rows[i]["JOIN_DT"].ToString().Replace("-", "");
                    dao.EMP_ID = EMP_ID;
                    //離社日期
                    LEAVE_DT = dt.Rows[i]["LEAVE_DT"].ToString().Replace("-", "");
                    dao.LEAVE_DT = LEAVE_DT;
                    string st = JOIN_DT.Substring(0,6);
                    //DataTable dtb = dao.getDone();//上個月是否已經算過交通津貼                             
                    if (EMP_ID == "12004")
                    {
                        EMP_ID = "12004";
                    }
                    //清空參數
                    dao.WORKING_DT = "0";
                    dao.TOTAL_PAY = "0";
                    dao.START_DT = "";
                    dao.ALLOWANCE_CD = "";
                    dao.BELONG_TO_DT = "";
                    dao.DAILY_PAY = "0";
                    dao.END_DT = "";
                    dao.APPLICATION_NO = "";                    

                    //新人的追溯交通津貼 
                    if (Convert.ToInt32(JOIN_DT.Substring(0, 6)) != Convert.ToInt32(MANAGER_YM) && Convert.ToInt32(JOIN_DT.Substring(0, 6)) == Convert.ToInt32(lastMonth))
                    {
                        //取得生效日期
                        dao.START_DT = dao.getStartDT();
                        string sdt = DateTime.Parse(dao.START_DT).ToString("yyyyMM");

                        //如果生效日年月>畫面年月 找前一筆  
                        if (Convert.ToInt32(sdt) > Convert.ToInt32(MANAGER_YM))
                        {
                            dao.END_DT_TWO = dao.getStartDT_TWO();
                            if (dao.END_DT_TWO == "")
                            {
                                dao.END_DT = "";
                                dao.APPLICATION_NO_TWO = "";
                                dao.DAILY_PAY_TWO = "";
                                dao.START_DT_TWO = "";
                                dao.END_DT_TWO = "";
                            }
                            else
                            {
                                string sd = DateTime.Parse(dao.START_DT_TWO).ToString("yyyyMM");
                                if (Convert.ToInt32(sd) > Convert.ToInt32(MANAGER_YM))
                                {
                                    dao.END_DT = "";
                                    dao.APPLICATION_NO_TWO = "";
                                    dao.DAILY_PAY_TWO = "";
                                    dao.START_DT_TWO = "";
                                    dao.END_DT_TWO = "";
                                }
                               
                            }

                        }
                        else
                        {
                            if (!EMPs.Contains(dao.EMP_ID))
                            {
                                dao.ALLOWANCE_CD = "2";
                                dao.BELONG_TO_DT = lastMonth;

                                //取得工作日數
                                if (LEAVE_DT != null && LEAVE_DT != "")
                                {
                                    if (Convert.ToInt32(LEAVE_DT.Substring(0, 6)) < Convert.ToInt32(MANAGER_YM))
                                    {
                                        dao.WORKING_DT = dao.getWORK_DT("N");
                                    }
                                    else
                                    {
                                        dao.WORKING_DT = dao.getWORK_DT("Y");
                                    }
                                }
                                else
                                {
                                    dao.WORKING_DT = dao.getWORK_DT("Y");
                                }


                                //津貼總金額 =日津貼 * 稼動日
                                dao.TOTAL_PAY = Convert.ToString(Convert.ToInt32(dao.DAILY_PAY) * Convert.ToInt32(dao.WORKING_DT));

                                dao.insertTRANS_MONTH_D();
                            }
                        }                                                      

                    }

                    //本月新進員工
                    if (Convert.ToInt32(JOIN_DT.Substring(0, 6)) == Convert.ToInt32(MANAGER_YM))
                    {
                        dao.ALLOWANCE_CD = "1";
                        dao.BELONG_TO_DT = MANAGER_YM;

                        //取得生效日期
                        dao.START_DT = dao.getStartDT();

                        //取得工作日數
                        if (LEAVE_DT != null && LEAVE_DT !="")
                        {
                            if (Convert.ToInt32(LEAVE_DT.Substring(0, 6)) == Convert.ToInt32(MANAGER_YM))
                            {
                                dao.WORKING_DT = dao.getWORK_DT_NEW("N");
                            }
                        }                      
                        else
                        {
                            dao.WORKING_DT = dao.getWORK_DT_NEW("Y");
                        }

                        //津貼總金額 =日津貼 * 稼動日
                        dao.TOTAL_PAY = Convert.ToString(Convert.ToInt32(dao.DAILY_PAY) * Convert.ToInt32(dao.WORKING_DT));

                        dao.insertTRANS_MONTH_D();

                    }

                    //一般員工的計算(非本月新進員工)
                    if (Convert.ToInt32(JOIN_DT.Substring(0, 6)) != Convert.ToInt32(MANAGER_YM))
                    {
                        dao.ALLOWANCE_CD = "1";
                        dao.BELONG_TO_DT = MANAGER_YM;
                        //取得生效日期
                        dao.START_DT = dao.getStartDT();
                        string sdt = DateTime.Parse(dao.START_DT).ToString("yyyyMM");


                        //生效日起(年月)  <> 畫面.管理年月                       
                        if (Convert.ToInt32(Convert.ToString(DateTime.Parse(dao.START_DT).ToString("yyyyMMdd")).Substring(0, 6)) != Convert.ToInt32(MANAGER_YM))
                        {
                            //如果生效日年月>畫面年月 找前一筆  
                            if (Convert.ToInt32(sdt) > Convert.ToInt32(MANAGER_YM))
                            {
                                dao.END_DT_TWO = dao.getStartDT_TWO();
                                if (dao.END_DT_TWO == "")
                                {
                                    dao.END_DT = "";
                                    dao.APPLICATION_NO_TWO = "";
                                    dao.DAILY_PAY_TWO = "";
                                    dao.START_DT_TWO = "";
                                    dao.END_DT_TWO = "";
                                    continue;
                                }
                                else
                                {
                                    string sdt2 = DateTime.Parse(dao.START_DT_TWO).ToString("yyyyMM");
                                    if (Convert.ToInt32(sdt2) > Convert.ToInt32(MANAGER_YM))
                                    {
                                        dao.END_DT = "";
                                        dao.APPLICATION_NO_TWO = "";
                                        dao.DAILY_PAY_TWO = "";
                                        dao.START_DT_TWO = "";
                                        dao.END_DT_TWO = "";
                                        continue;
                                    }
                                    else
                                    {
                                        dao.START_DT = dao.START_DT_TWO;
                                        // Add by Terry 20160115
                                        dao.APPLICATION_NO = dao.APPLICATION_NO_TWO;
                                        dao.END_DT = dao.END_DT_TWO;
                                        dao.DAILY_PAY = dao.DAILY_PAY_TWO;

                                    }
                                }

                            } 

                            //生效日迄是空值
                            if (dao.END_DT == null || dao.END_DT == "")
                            {
                                //取得工作日數
                                if (LEAVE_DT != null && LEAVE_DT != "")
                                {
                                    if (Convert.ToInt32(LEAVE_DT.Substring(0, 6)) == Convert.ToInt32(MANAGER_YM))
                                    {
                                        dao.WORKING_DT = dao.getWORK_DT_NEW("N");
                                    }
                                    else
                                    {
                                        dao.WORKING_DT = dao.getWORK_DT_NEW("Y");
                                    }
                                }
                                else
                                {
                                    dao.WORKING_DT = dao.getWORK_DT_NEW("Y");
                                }
                                //津貼總金額 =日津貼 * 稼動日
                                dao.TOTAL_PAY = Convert.ToString(Convert.ToInt32(dao.DAILY_PAY) * Convert.ToInt32(dao.WORKING_DT));



                            }
                            else
                            {
                                //生效日迄(年月) > 畫面管理年月                                
                                if (Convert.ToInt32(dao.END_DT.Replace("-", "").Substring(0, 6)) > Convert.ToInt32(MANAGER_YM))
                                {
                                    //取得工作日數
                                    if (LEAVE_DT != null && LEAVE_DT != "")
                                    {
                                        if (Convert.ToInt32(LEAVE_DT.Substring(0, 6)) == Convert.ToInt32(MANAGER_YM))
                                        {
                                            dao.WORKING_DT = dao.getWORK_DT_NEW("N");
                                        }
                                        else
                                        {
                                            dao.WORKING_DT = dao.getWORK_DT_NEW("Y");
                                        }
                                    }
                                    else
                                    {
                                        dao.WORKING_DT = dao.getWORK_DT_NEW("Y");
                                    }
                                    //津貼總金額 =日津貼 * 稼動日
                                    dao.TOTAL_PAY = Convert.ToString(Convert.ToInt32(dao.DAILY_PAY) * Convert.ToInt32(dao.WORKING_DT));


                                }

                                //生效日迄(年月) = 畫面.管理年月
                                if (Convert.ToInt32(dao.END_DT.Replace("-", "").Substring(0, 6)) == Convert.ToInt32(MANAGER_YM))
                                {
                                    //取得工作日數
                                    if (LEAVE_DT != null && LEAVE_DT != "")
                                    {
                                        if (Convert.ToInt32(LEAVE_DT.Substring(0, 6)) == Convert.ToInt32(MANAGER_YM))
                                        {
                                            //判斷 離社日  生效日迄何者小
                                            minDT = dif(LEAVE_DT, dao.END_DT.Replace("-", ""));

                                            dao.WORKING_DT = dao.getWORK_DT_Normal("N", minDT);
                                        }
                                        else
                                        {
                                            dao.WORKING_DT = dao.getWORK_DT_Normal("N", dao.END_DT);
                                        }

                                    }
                                    else
                                    {
                                        dao.WORKING_DT = dao.getWORK_DT_Normal("N", dao.END_DT);
                                    }

                                    //津貼總金額 =日津貼 * 稼動日
                                    dao.TOTAL_PAY = Convert.ToString(Convert.ToInt32(dao.DAILY_PAY) * Convert.ToInt32(dao.WORKING_DT));


                                }

                            }

                            dao.insertTRANS_MONTH_D();
                        }else if (Convert.ToInt32(Convert.ToString(DateTime.Parse(dao.START_DT).ToString("yyyyMMdd")).Substring(0, 6)) == Convert.ToInt32(MANAGER_YM))
                        {//生效日起(年月)  = 畫面.管理年月
                            string dd = Convert.ToString(DateTime.Parse(dao.START_DT).ToString("yyyyMMdd"));
                            dd = dd.Substring(6);
                            //生效日起 >15日
                            if (Convert.ToInt32(Convert.ToString(DateTime.Parse(dao.START_DT).ToString("yyyyMMdd")).Substring(6)) > 15)
                            {
                                //取得第二大的生效日期
                                /*20141124 
                                 * 若第二大生效日的迄日不等於(最大生效日或最大生效日前一日):則以最大生效日那筆資料為準 
                                 * 若第二大生效日的迄日等於(最大生效日或最大生效日前一日):則以第二大生效日那筆資料為準 
                                 */
                                dao.END_DT_TWO = dao.getStartDT_TWO();
                                
                                if (dao.END_DT_TWO == "9999-12-31")
                                {
                                    
                                }

                                if (dao.END_DT_TWO == "")
                                {
                                    dao.END_DT = "";
                                    dao.APPLICATION_NO_TWO = "";
                                    dao.DAILY_PAY_TWO = "";
                                    dao.START_DT_TWO = "";
                                    dao.END_DT_TWO = "";
                                }
                                else
                                {
                                   //第二大生效日的迄日不等於(最大生效日或最大生效日前一日)
                                    if (dao.END_DT_TWO != "9999-12-31" && Convert.ToString(Convert.ToDateTime(dao.END_DT_TWO).AddDays(1).ToString("yyyy/MM/dd")) == Convert.ToString(DateTime.Parse(dao.START_DT).ToString("yyyy/MM/dd"))
                                        && Convert.ToString(DateTime.Parse(dao.END_DT_TWO).ToString("yyyy/MM/dd")) == Convert.ToString(DateTime.Parse(dao.START_DT).ToString("yyyy/MM/dd")))
                                    {
                                        //取得工作日數
                                        if (LEAVE_DT != null && LEAVE_DT != "")
                                        {
                                            if (Convert.ToInt32(LEAVE_DT.Substring(0, 6)) == Convert.ToInt32(MANAGER_YM))
                                            {
                                                dao.WORKING_DT = dao.getWORK_DT_NEW("N");
                                            }
                                            else
                                            {
                                                dao.WORKING_DT = dao.getWORK_DT_NEW("Y");
                                            }
                                        }
                                        else
                                        {
                                            dao.WORKING_DT = dao.getWORK_DT_NEW("Y");
                                        }
                                        //津貼總金額 =日津貼 * 稼動日
                                        dao.TOTAL_PAY = Convert.ToString(Convert.ToInt32(dao.DAILY_PAY) * Convert.ToInt32(dao.WORKING_DT));
                                    }
                                    //若第二大生效日的迄日等於(最大生效日或最大生效日前一日)
                                    else
                                    {
                                        dao.START_DT = dao.START_DT_TWO;
                                        dao.APPLICATION_NO = dao.APPLICATION_NO_TWO;
                                        dao.END_DT = dao.END_DT_TWO;
                                        dao.DAILY_PAY = dao.DAILY_PAY_TWO;

                                        //取得工作日數
                                        if (LEAVE_DT != null && LEAVE_DT != "")
                                        {
                                            if (Convert.ToInt32(LEAVE_DT.Substring(0, 6)) == Convert.ToInt32(MANAGER_YM))
                                            {
                                                dao.WORKING_DT = dao.getWORK_DT_NEW("N");
                                            }
                                            else
                                            {
                                                dao.WORKING_DT = dao.getWORK_DT_NEW("Y");
                                            }
                                        }
                                        else
                                        {
                                            dao.WORKING_DT = dao.getWORK_DT_NEW("Y");
                                        }
                                        //津貼總金額 =日津貼 * 稼動日
                                        dao.TOTAL_PAY = Convert.ToString(Convert.ToInt32(dao.DAILY_PAY) * Convert.ToInt32(dao.WORKING_DT));
                                    }
                                }
                            }
                            else //生效日起 <=15日
                            {
                                
                                //生效日迄是空值
                                if (dao.END_DT == null || dao.END_DT == "")
                                {
                                    //取得工作日數
                                    if (LEAVE_DT != null && LEAVE_DT != "")
                                    {
                                        if (Convert.ToInt32(LEAVE_DT.Substring(0, 6)) == Convert.ToInt32(MANAGER_YM))
                                        {
                                            dao.WORKING_DT = dao.getWORK_DT_NEW("N");
                                        }
                                        else
                                        {
                                            dao.WORKING_DT = dao.getWORK_DT_NEW("Y");
                                        }
                                    }
                                    else
                                    {
                                        dao.WORKING_DT = dao.getWORK_DT_NEW("Y");
                                    }
                                    //津貼總金額 =日津貼 * 稼動日
                                    dao.TOTAL_PAY = Convert.ToString(Convert.ToInt32(dao.DAILY_PAY) * Convert.ToInt32(dao.WORKING_DT));



                                }
                                else
                                {
                                    //生效日迄(年月) > 畫面管理年月                                
                                    if (Convert.ToInt32(dao.END_DT.Replace("-", "").Substring(0, 6)) > Convert.ToInt32(MANAGER_YM))
                                    {
                                        //取得工作日數
                                        if (LEAVE_DT != null && LEAVE_DT != "")
                                        {
                                            if (Convert.ToInt32(LEAVE_DT.Substring(0, 6)) == Convert.ToInt32(MANAGER_YM))
                                            {
                                                dao.WORKING_DT = dao.getWORK_DT_NEW("N");
                                            }
                                            else
                                            {
                                                dao.WORKING_DT = dao.getWORK_DT_NEW("Y");
                                            }
                                        }
                                        else
                                        {
                                            dao.WORKING_DT = dao.getWORK_DT_NEW("Y");
                                        }
                                        //津貼總金額 =日津貼 * 稼動日
                                        dao.TOTAL_PAY = Convert.ToString(Convert.ToInt32(dao.DAILY_PAY) * Convert.ToInt32(dao.WORKING_DT));


                                    }

                                    //生效日迄(年月) = 畫面.管理年月
                                    if (Convert.ToInt32(dao.END_DT.Replace("-", "").Substring(0, 6)) == Convert.ToInt32(MANAGER_YM))
                                    {
                                        //取得工作日數
                                        if (LEAVE_DT != null && LEAVE_DT != "")
                                        {
                                            if (Convert.ToInt32(LEAVE_DT.Substring(0, 6)) == Convert.ToInt32(MANAGER_YM))
                                            {
                                                //判斷 離社日  生效日迄何者小
                                                minDT = dif(LEAVE_DT, dao.END_DT.Replace("-", ""));

                                                dao.WORKING_DT = dao.getWORK_DT_Normal("N", minDT);
                                            }
                                            else
                                            {
                                                dao.WORKING_DT = dao.getWORK_DT_Normal("N", dao.END_DT);
                                            }

                                        }
                                        else
                                        {
                                            dao.WORKING_DT = dao.getWORK_DT_Normal("N", dao.END_DT);
                                        }

                                        //津貼總金額 =日津貼 * 稼動日
                                        dao.TOTAL_PAY = Convert.ToString(Convert.ToInt32(dao.DAILY_PAY) * Convert.ToInt32(dao.WORKING_DT));


                                    }

                                }


                            }

                            

                            dao.insertTRANS_MONTH_D();
                        }
                        
                    }

                    
                }
                Commit();
            }
            //Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string getLastMonth(string MANAGER_YM)
    {
        int ym = 0;
        string m = "";
        try
        {
            ym = Convert.ToInt32(MANAGER_YM);
            if (MANAGER_YM.Substring(4) != "01")
            {
                m = Convert.ToString(ym - 1);
            }
            else {
                m = Convert.ToString(Convert.ToInt32(MANAGER_YM.Substring(0, 4)) - 1) + "12";
            }

            return m;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string dif(string LEAVE_DT,string END_DT)
    {        
        string m = "";
        try
        {
           if (LEAVE_DT != "" && END_DT != "")
           {
               if (Convert.ToInt32(LEAVE_DT) > Convert.ToInt32(END_DT))
               {
                   m = END_DT;
               }
               else {
                   m = LEAVE_DT;
               }
                
           }

            return m;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public void checkSalaryClose(CFB2DD0200DAO dao)
    {
       
        try
        {
            DataTable dt = dao.checkSalaryClose();

            if (dt.Rows.Count > 0)
            {
                dao.SALARY_LOCKED = dt.Rows[0]["SALARY_LOCKED"].ToString();
            }
           
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string getSalaryCode(CFB2DD0200DAO dao)
    {
        string errormessage = "";
        
        try
        {
            DataTable dt =dao.getSalaryCode();

            if (dt.Rows.Count > 0)
            {
                dao.SALARY_DT = dt.Rows[0]["SALARY_DT"].ToString();
                dao.SALARY_YM = dt.Rows[0]["SALARY_YM"].ToString();
                dao.SALARY_TYPE = dt.Rows[0]["SALARY_TYPE"].ToString();
                dao.SALARY_SDT = dt.Rows[0]["SALARY_SDT"].ToString();
                dao.SALARY_EDT = dt.Rows[0]["SALARY_EDT"].ToString();
                 
                
            }
            else {
                errormessage += "薪資類別尚未建立最新月薪\\n";
                return errormessage; ;
            }

            return errormessage; ;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public DataTable getSalaryCTL(CFB2DD0200DAO dao)
    {
               
        try
        {
            DataTable dt = dao.getSalaryCTL();

            return dt;          
           
            
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void update_Trans_Month(CFB2DD0200DAO dao)    {
        

        try
        {
            BeginTransaction();

            dao.updateTRANS_MONTH();

            Commit();            

        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    public string insertSALARY_MONTH_CTRL(CFB2DD0200DAO dao)
    {


        try
        {
            dao.TAKE_OUT_BY = SessionHandle.Current.emp_id;
            
            BeginTransaction();

            dao.insertSALARY_MONTH_CTRL();

            //更新交通津貼月度計算結果檔
            dao.updateTRANS_MONTH_FIN();

            Commit();
            return "0";

        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }


    public string updateSALARY_MONTH_CTRL(CFB2DD0200DAO dao)
    {


        try
        {
            dao.TAKE_OUT_BY = SessionHandle.Current.emp_id;

            BeginTransaction();

            dao.updateSALARY_MONTH_CTRL();

            //更新交通津貼月度計算結果檔
            dao.updateTRANS_MONTH_FIN();

            Commit();
            return "0";

        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }


}