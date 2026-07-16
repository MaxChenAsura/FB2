using FB2.tw.co.toyota.kuozui.bo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// CFB2DE0300BO 的摘要描述
/// </summary>
public class CFB2DE0300BO : BaseService
{
	public CFB2DE0300BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string getMANAGER_YM()
    {
        try
        {
            CFB2DE0300DAO dao = new CFB2DE0300DAO();
            return dao.getMANAGER_YM();
        }
        catch (Exception)
        {

            throw;
        }

    }

    public DataTable qry_TB_D_R_RES_MONTH_ACTURL(string MANAGER_YM, String PLANT_CD)
    {
        try
        {
            CFB2DE0300DAO dao = new CFB2DE0300DAO();
            return dao.qry_TB_D_R_RES_MONTH_ACTURL(MANAGER_YM, PLANT_CD);
        }
        catch (Exception)
        {

            throw;
        }

    }

    public DataTable qry_TB_D_R_RES_ACTURL(string MANAGER_YM, String PLANT_CD)
    {
        try
        {
            CFB2DE0300DAO dao = new CFB2DE0300DAO();
            return dao.qry_TB_D_R_RES_ACTURL(MANAGER_YM, PLANT_CD);
        }
        catch (Exception)
        {

            throw;
        }

    }

    public string Exec(CFB2DE0300DAO dao)
    {
        DataTable bf_dt = null;
        string man_ym = "", CLOCK_NO = "", CLOCK_PLANT_CD = "", RESTAURANT_CD = "", MANAGER_UNIT = "", HOLIDAY_BENTO_PEOPLE = "", VISITOR_BOND_PEOPLE = "";
        string EDU_PEOPLE = "", MONTH_BR_BOND_PEOPLE = "", MONTH_DN_BOND_PEOPLE = "", OVERTIME_BOND_PEOPLE = "", ERROE_BR_PEOPLE = "", ERROE_DN_PEOPLE = "";
        string MONTH_BR_PEOPLE = "", af_MANAGER_YM = "", af_CLOCK_NO = "", af_MANAGER_UNIT = "", MONTH_MD_PEOPLE = "", MONTH_MD_AMOUNT = "", ERROR_MD_PEOPLE = "", ERROR_MD_AMOUNT = "";
        try
        {
            //取得最後早餐出勤時間
            dao.LAST_BR_TIME = dao.getLAST_BR_TIME();

            //取得刪除前 月度用餐實績統計檔的資料
            bf_dt = dao.selectMonthData();
            dao.del_TB_D_R_RES_MONTH_ACTURL(dao.MANAGER_YM, dao.PLANT_CD);

            //取得刪除後的 月度用餐實績統計檔的資料
            DataTable af_dt = dao.selectMonthData();

            BeginTransaction();
            //讀取 用餐實績檔
            DataTable actual_dt = dao.getActualData();
            if (actual_dt.Rows.Count > 0)
            {
                for (int i = 0; i < actual_dt.Rows.Count; i++)
                {
                    man_ym = actual_dt.Rows[i]["MANAGER_DT"].ToString();
                    CLOCK_NO = actual_dt.Rows[i]["CLOCK_NO"].ToString();
                    CLOCK_PLANT_CD = actual_dt.Rows[i]["CLOCK_PLANT_CD"].ToString();
                    RESTAURANT_CD = actual_dt.Rows[i]["RESTAURANT_CD"].ToString();
                    MANAGER_UNIT = actual_dt.Rows[i]["MANAGER_UNIT"].ToString();
                    MONTH_BR_BOND_PEOPLE = actual_dt.Rows[i]["MONTH_BR_BOND_PEOPLE"].ToString();
                    MONTH_DN_BOND_PEOPLE = actual_dt.Rows[i]["MONTH_DN_BOND_PEOPLE"].ToString();
                    OVERTIME_BOND_PEOPLE = actual_dt.Rows[i]["OVERTIME_BOND_PEOPLE"].ToString();
                    ERROE_BR_PEOPLE = actual_dt.Rows[i]["ERROE_BR_PEOPLE"].ToString();
                    ERROE_DN_PEOPLE = actual_dt.Rows[i]["ERROE_DN_PEOPLE"].ToString();
                    EDU_PEOPLE = actual_dt.Rows[i]["EDU_PEOPLE"].ToString();
                    HOLIDAY_BENTO_PEOPLE = "0";
                    VISITOR_BOND_PEOPLE = "0";
                    MONTH_MD_PEOPLE = actual_dt.Rows[i]["MONTH_MD_PEOPLE"].ToString();
                    MONTH_MD_AMOUNT = actual_dt.Rows[i]["MONTH_MD_AMOUNT"].ToString();
                    ERROR_MD_PEOPLE = actual_dt.Rows[i]["ERROR_MD_PEOPLE"].ToString();
                    ERROR_MD_AMOUNT = actual_dt.Rows[i]["ERROR_MD_AMOUNT"].ToString();

                    MONTH_BR_PEOPLE = dao.getMONTH_BR_PEOPLE(man_ym, MANAGER_UNIT, RESTAURANT_CD);
                    
                    if (af_dt.Rows.Count > 0)
                    {
                        for (int j = 0; j < af_dt.Rows.Count; j++)
                        {
                            af_MANAGER_YM = af_dt.Rows[i]["MANAGER_YM"].ToString();
                            af_CLOCK_NO = af_dt.Rows[i]["CLOCK_NO"].ToString();
                            af_MANAGER_UNIT = af_dt.Rows[i]["MANAGER_UNIT"].ToString();
                            
                           
                            if (man_ym.Equals(af_MANAGER_YM) && CLOCK_NO.Equals(af_CLOCK_NO) && MANAGER_UNIT.Equals(af_MANAGER_UNIT))
                            {
                                dao.update_TB_D_R_RES_MONTH_ACTURL(man_ym, CLOCK_NO, CLOCK_PLANT_CD, RESTAURANT_CD, MANAGER_UNIT, MONTH_BR_PEOPLE, MONTH_BR_BOND_PEOPLE, MONTH_DN_BOND_PEOPLE, OVERTIME_BOND_PEOPLE,
                                 HOLIDAY_BENTO_PEOPLE, VISITOR_BOND_PEOPLE, EDU_PEOPLE, ERROE_BR_PEOPLE, ERROE_DN_PEOPLE, MONTH_MD_PEOPLE, MONTH_MD_AMOUNT, ERROR_MD_PEOPLE, ERROR_MD_AMOUNT);
                            }                                        
                        }
                    }
                    else
                    {
                        dao.insert_TB_D_R_RES_MONTH_ACTURL(man_ym, CLOCK_NO, CLOCK_PLANT_CD, RESTAURANT_CD, MANAGER_UNIT, MONTH_BR_PEOPLE, MONTH_BR_BOND_PEOPLE, MONTH_DN_BOND_PEOPLE, OVERTIME_BOND_PEOPLE,
                               HOLIDAY_BENTO_PEOPLE, VISITOR_BOND_PEOPLE, EDU_PEOPLE, ERROE_BR_PEOPLE, ERROE_DN_PEOPLE, MONTH_MD_PEOPLE, MONTH_MD_AMOUNT, ERROR_MD_PEOPLE, ERROR_MD_AMOUNT);
                    }

                    dao.update_TB_D_R_RES_ACTURL(man_ym, CLOCK_PLANT_CD, MANAGER_UNIT);
                    //EDU_PEOPLE.Substring(0, 100);//測試錯誤時rollback
                }
            }          

            Commit();             
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            insertMonth(bf_dt);
            return ex.Message;
        }
    }

    public void insertMonth(DataTable dt)
    {
        string MANAGER_YM = "", CLOCK_NO = "", CLOCK_PLANT_CD = "", RESTAURANT_CD = "", MANAGER_UNIT = "", MONTH_BR_PEOPLE = "", MONTH_BR_BOND_PEOPLE = "",
               MONTH_DN_BOND_PEOPLE = "", OVERTIME_BOND_PEOPLE = "", HOLIDAY_BENTO_PEOPLE = "", VISITOR_BOND_PEOPLE = "", EDU_PEOPLE = "",
               ERROE_BR_PEOPLE = "", ERROE_DN_PEOPLE = "", CREATED_BY = "", CREATED_DT = "", UPDATED_BY = "", UPDATED_DT = "", FUNC_ID = "";

        try
        {
            CFB2DE0300DAO dao = new CFB2DE0300DAO();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MANAGER_YM = dt.Rows[i]["MANAGER_YM"].ToString();
                    CLOCK_NO = dt.Rows[i]["CLOCK_NO"].ToString();
                    CLOCK_PLANT_CD = dt.Rows[i]["CLOCK_PLANT_CD"].ToString();
                    RESTAURANT_CD = dt.Rows[i]["RESTAURANT_CD"].ToString();
                    MANAGER_UNIT = dt.Rows[i]["MANAGER_UNIT"].ToString();
                    MONTH_BR_PEOPLE = dt.Rows[i]["MONTH_BR_PEOPLE"].ToString();
                    MONTH_BR_BOND_PEOPLE = dt.Rows[i]["MONTH_BR_BOND_PEOPLE"].ToString();
                    MONTH_DN_BOND_PEOPLE = dt.Rows[i]["MONTH_DN_BOND_PEOPLE"].ToString();
                    OVERTIME_BOND_PEOPLE = dt.Rows[i]["OVERTIME_BOND_PEOPLE"].ToString();
                    HOLIDAY_BENTO_PEOPLE = dt.Rows[i]["HOLIDAY_BENTO_PEOPLE"].ToString();
                    VISITOR_BOND_PEOPLE = dt.Rows[i]["VISITOR_BOND_PEOPLE"].ToString();
                    EDU_PEOPLE = dt.Rows[i]["EDU_PEOPLE"].ToString();
                    ERROE_BR_PEOPLE = dt.Rows[i]["ERROE_BR_PEOPLE"].ToString();
                    ERROE_DN_PEOPLE = dt.Rows[i]["ERROE_DN_PEOPLE"].ToString();
                    CREATED_BY = dt.Rows[i]["CREATED_BY"].ToString();
                    CREATED_DT = dt.Rows[i]["CREATED_DT"].ToString();
                    CREATED_DT = DateTime.Parse(CREATED_DT).ToString("yyyy/MM/dd HH:mm:ss");
                    UPDATED_BY = dt.Rows[i]["UPDATED_BY"].ToString();                    
                    UPDATED_DT = dt.Rows[i]["UPDATED_DT"].ToString();
                    UPDATED_DT = DateTime.Parse(UPDATED_DT).ToString("yyyy/MM/dd HH:mm:ss");
                    FUNC_ID = dt.Rows[i]["FUNC_ID"].ToString();


                    dao.insertMonth(MANAGER_YM, CLOCK_NO, CLOCK_PLANT_CD, RESTAURANT_CD, MANAGER_UNIT, MONTH_BR_PEOPLE, MONTH_BR_BOND_PEOPLE, MONTH_DN_BOND_PEOPLE, OVERTIME_BOND_PEOPLE,
                                HOLIDAY_BENTO_PEOPLE, VISITOR_BOND_PEOPLE, EDU_PEOPLE, ERROE_BR_PEOPLE, ERROE_DN_PEOPLE,
                                CREATED_BY,CREATED_DT,UPDATED_BY, UPDATED_DT, FUNC_ID);
                }
            }
            
        }
        catch (Exception)
        {

            throw;
        }

    }

    public string getCal(string MANAGER_YM, string PLANT_CD)
    {
        CFB2DE0300DAO dao = new CFB2DE0300DAO();
        try
        {
            return dao.getCal(MANAGER_YM, PLANT_CD);


        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

}