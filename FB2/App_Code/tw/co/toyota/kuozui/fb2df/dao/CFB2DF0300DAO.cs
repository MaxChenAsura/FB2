using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2DF0300DAO 的摘要描述
/// </summary>
public class CFB2DF0300DAO : BaseDAO
{
    //Session
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    //畫面
    public string MANAGER_YM { get; set; }
    public string MANAGER_YM_S { get; set; }
    public string MANAGER_YM_E { get; set; }
    //主檔
    public string EMP_ID { get; set; }
    public string START_DT { get; set; }
    public string END_DT { get; set; }
    public string AMOUNT { get; set; }
    public string OTHER_AMOUNT { get; set; }
    //計算後住宿費
    public string NEW_AMOUNT { get; set; }
    public string NEW_OTHER_AMOUNT { get; set; }
    public string TOTAL_AMOUNT { get; set; }
    public string TEMP_AMOUNT { get; set; }
    public string TEMP_OTHER_AMOUNT { get; set; }
    //薪資計算
    public string YM { get; set; }
    public string SALARY_DT { get; set; }
    public string SALARY_YM { get; set; }
    public string SALARY_TYPE { get; set; }
    public string SALARY_LOCKED { get; set; }
    public string TAKE_OUT_BY { get; set; }
    public string SALARY_SDT { get; set; }
    public string SALARY_EDT { get; set; }
    public string OPERATION_ID { get; set; }
    

	public CFB2DF0300DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable getManagerDT(string MANAGER_YM)
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select distinct(CONVERT(char(10),SALARY_TAKE_OUT_DT, 120)) SALARY_TAKE_OUT_DT,STATUS From TB_D_R_ACCOM_MONTH_EXP Where MANAGER_YM = @MANAGER_YM");
            ht.Add("@MANAGER_YM", MANAGER_YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable checkSalaryClose()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT SALARY_LOCKED FROM TB_S_M_SALARY_MONTH_CTRL a");
            sb.Append(" left join TB_S_M_SALARY_CAL_H b");
            sb.Append(" on a.SALARY_YM = b.SALARY_YM and a.SALARY_DT = b.SALARY_DT and a.SALARY_TYPE = b.SALARY_TYPE");
            sb.Append(" and b.SALARY_TYPE = 'A'");
            sb.Append(" where a.OPERATION_ID = 'D01' and a.SALARY_YM = @SALARY_YM");

            ht.Add("@SALARY_YM", YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
    //20150310  改抓住宿日 <= 處理年月+1 的資料
    public DataTable getAccomMain()
    {        
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select a.EMP_ID,CONVERT(char(10),a.START_DT, 120)START_DT,CONVERT(char(10),a.END_DT, 120)END_DT,a.AMOUNT,a.OTHER_AMOUNT,a.BASE_NO");
            sb.Append(" From TB_H_M_EMP b");
            sb.Append(" left join TB_D_M_ACCOM_MAIN a  ");
            sb.Append(" on a.EMP_ID = b.EMP_ID and (b.LEAVE_DT is null or left(Convert(varchar, b.LEAVE_DT,112),6) >= @MANAGER_YM)  ");
            sb.Append(" where 1=1");
            sb.Append(" and (select CONVERT(char(4),YEAR(CONVERT(char(10),START_DT, 120)))+ (select case when CONVERT(char(4),MONTH(CONVERT(char(10),START_DT, 120)))< 10" +
                      " then '0' + CONVERT(char(4),MONTH(CONVERT(char(10),START_DT, 120)))" +
                      " else CONVERT(char(4),MONTH(CONVERT(char(10),START_DT, 120)))" +
                      " end)) <= ");
            sb.Append("          case when right(Convert(varchar, @MANAGER_YM),2) = '12'");
            sb.Append("          then CONVERT(char(4), CONVERT(int, CONVERT(char(4),@MANAGER_YM)) +1 )");
            sb.Append("          else CONVERT(char(4),@MANAGER_YM) end +");
            sb.Append("          (case when right(Convert(varchar, @MANAGER_YM),2) = '12'");
            sb.Append("          then '01' else ");
            sb.Append("                         (");
            sb.Append("                           case when Convert(varchar, Convert(int,right(Convert(varchar, @MANAGER_YM),2)) +1) < 10");
            sb.Append("                           then '0' + Convert(varchar, Convert(int,right(Convert(varchar, @MANAGER_YM),2)) +1)");
            sb.Append("                           else Convert(varchar, Convert(int,right(Convert(varchar, @MANAGER_YM),2)) +1)");
            sb.Append("                           end");
            sb.Append("                          )");
            sb.Append("           end");
            sb.Append("          )");	
            
            sb.Append(" and ((select CONVERT(char(4),YEAR(CONVERT(char(10),END_DT, 120)))+ (select case when CONVERT(char(4),MONTH(CONVERT(char(10),END_DT, 120)))< 10" +
                      " then '0' + CONVERT(char(4),MONTH(CONVERT(char(10),END_DT, 120)))" +
                      " else CONVERT(char(4),MONTH(CONVERT(char(10),END_DT, 120)))" +
                      " end)) >= @MANAGER_YM");
            sb.Append(" or isnull(END_DT,'')  ='')");
            
            ht.Add("@MANAGER_YM", MANAGER_YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getHistory()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select a.EMP_ID,CONVERT(char(10),a.START_DT, 120)START_DT,CONVERT(char(10),a.END_DT, 120)END_DT,a.AMOUNT,a.BASE_NO,a.OTHER_AMOUNT,a.UPDATED_DT as udt");
            sb.Append(" From TB_H_M_EMP b");
            sb.Append(" left join TB_D_M_ACCOM_HISTORY a  ");
            sb.Append(" on a.EMP_ID = b.EMP_ID ");
            sb.Append(" where a.EMP_ID = @EMP_ID");
            sb.Append(" and a.END_DT = @START_DT ");
            sb.Append(" and a.START_DT <> a.END_DT ");
            sb.Append(" order by  udt desc");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@START_DT", START_DT);

            DataTable dt = dbConn.QueryT(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public void insertACCOM_MONTH()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            sb.Append(" insert into TB_D_R_ACCOM_MONTH_EXP");
            sb.Append(" (MANAGER_YM,EMP_ID,EMP_NAME,COMPANY_CD,DEPT_NO,START_DT,END_DT,ACCOM_CD,ROOM_NO,AMOUNT,OTHER_AMOUNT,");
            sb.Append("TOTAL_AMOUNT,REMARK,SALARY_TAKE_OUT_DT,SALARY_TAKE_OUT_BY,STATUS,GIVE_SALARY_DT,");
            sb.Append("CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @MANAGER_YM,@EMP_ID,EMP_NAME,COMPANY_CD,DEPT_NO,@START_DT,@END_DT,ACCOM_CD,ROOM_NO,@NEW_AMOUNT,@NEW_OTHER_AMOUNT,");
            sb.Append("@TOTAL_AMOUNT,'','9999/12/31','','N','9999/12/31',");
            sb.Append("@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID");
            sb.Append(" from TB_D_M_ACCOM_MAIN");
            sb.Append(" where EMP_ID = @EMP_ID");

            ht.Add("@MANAGER_YM", MANAGER_YM);           
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@NEW_AMOUNT", NEW_AMOUNT);          
            ht.Add("@NEW_OTHER_AMOUNT", NEW_OTHER_AMOUNT);
            ht.Add("@TOTAL_AMOUNT", TOTAL_AMOUNT);
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

    public void deleteACCOM_MONTH()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" delete from TB_D_R_ACCOM_MONTH_EXP");
            sb.Append(" where MANAGER_YM = @MANAGER_YM");
            //sb.Append(" where EMP_ID = @EMP_ID and MANAGER_YM = @MANAGER_YM");           
            
            ht.Add("@MANAGER_YM", MANAGER_YM);          


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable selectMonthData()
    {        
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select MANAGER_YM,a.EMP_ID,a.EMP_NAME,a.COMPANY_CD+'-'+COMPANY_SNAME COMPANY_CD,a.DEPT_NO,CONVERT(char(10),a.START_DT, 111) START_DT,CONVERT(char(10),a.END_DT, 111) END_DT,");
            sb.Append("ACCOM_CD+'-'+g.SUB_DESC ACCOM_CD,ROOM_NO,AMOUNT,OTHER_AMOUNT,TOTAL_AMOUNT,a.REMARK,CONVERT(char(10),SALARY_TAKE_OUT_DT, 111)SALARY_TAKE_OUT_DT,");
            sb.Append("d.EMP_NAME SALARY_TAKE_OUT_BY,STATUS,CONVERT(char(10),GIVE_SALARY_DT, 111)GIVE_SALARY_DT,k.EMP_CD+'-'+b.SUB_DESC EMP_CD,");
            sb.Append("k.PJOB_CD+'-'+c.PJOB_DESC PJOB_CD,h.EMP_NAME CREATED_BY,CONVERT(char(10),a.CREATED_DT, 111)CREATED_DT,e.EMP_NAME UPDATED_BY,CONVERT(char(10),a.UPDATED_DT, 111)UPDATED_DT,a.FUNC_ID");
            sb.Append(" from TB_D_R_ACCOM_MONTH_EXP a");
            sb.Append(" left join TB_H_M_EMP k");
            sb.Append(" on a.EMP_ID = k.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D b on k.EMP_CD = b.SUB_CD and b.SYS_CD = 'HB'");
            sb.Append(" and b.MAIN_CD = 'EMP_CD' and IS_VALID ='Y'");
            sb.Append(" left join TB_H_M_PJOB c on k.PJOB_CD = c.PJOB_CD and CONVERT(char(10),c.END_DT, 120) = '9999-12-31' ");
            sb.Append(" left join TB_H_M_EMP d on a.SALARY_TAKE_OUT_BY = d.EMP_ID");
            sb.Append(" left join TB_H_M_EMP e on a.UPDATED_BY = e.EMP_ID");
            sb.Append(" left join TB_H_M_COMPANY f on a.COMPANY_CD = f.COMPANY_CD");
            sb.Append(" left join TB_9_M_COMM_D g on a.ACCOM_CD = g.SUB_CD and g.main_cd='ACCOM_CD' and g.IS_VALID = 'Y' and g.SYS_CD = 'DF'");
            sb.Append(" left join TB_H_M_EMP h on a.CREATED_BY = h.EMP_ID");
            sb.Append(" where MANAGER_YM = @MANAGER_YM");
                        
            ht.Add("@MANAGER_YM", MANAGER_YM);


            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string manager_ym_s, string manager_ym_e)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" MANAGER_YM,(select CASE WHEN CONVERT(char(10),SALARY_TAKE_OUT_DT, 120) ='9999-12-31' THEN '' ELSE CONVERT(char(10),SALARY_TAKE_OUT_DT, 111) END) SALARY_TAKE_OUT_DT,STATUS,REMARK");
            sb.Append(" from (select distinct(MANAGER_YM) MANAGER_YM,SALARY_TAKE_OUT_DT,STATUS,REMARK from TB_D_R_ACCOM_MONTH_EXP where MANAGER_YM between @manager_ym_s and @manager_ym_e ) a");
            sb.Append(" where 1=1");

            if (manager_ym_s != "" && manager_ym_e != "")
            {
                sb.Append(" and a.MANAGER_YM between @manager_ym_s and @manager_ym_e ");
                ht.Add("@manager_ym_s", manager_ym_s.Replace("/", ""));
                ht.Add("@manager_ym_e", manager_ym_e.Replace("/", ""));
            }


            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows, string manager_ym_s, string manager_ym_e)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from (select distinct(MANAGER_YM) MANAGER_YM,SALARY_TAKE_OUT_DT,STATUS,REMARK from TB_D_R_ACCOM_MONTH_EXP where MANAGER_YM between @manager_ym_s and @manager_ym_e ) b");
            sb.Append(" where 1=1");

            if (manager_ym_s != "" && manager_ym_e != "")
            {
                sb.Append(" and b.MANAGER_YM between @manager_ym_s and @manager_ym_e ");
                ht.Add("@manager_ym_s", manager_ym_s.Replace("/", ""));
                ht.Add("@manager_ym_e", manager_ym_e.Replace("/", ""));
            }

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

    public DataTable getSalaryCode()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT SALARY_YM,CONVERT(char(10),SALARY_DT, 120) SALARY_DT,SALARY_TYPE,CONVERT(char(10),SALARY_SDT, 120)SALARY_SDT,CONVERT(char(10),SALARY_EDT, 120)SALARY_EDT");
            sb.Append(" FROM TB_S_M_SALARY_CAL_H");
            sb.Append(" where SALARY_YM = @MANAGER_YM");
            sb.Append(" and SALARY_TYPE = 'A'");

            ht.Add("@MANAGER_YM", YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public void deleteSALARY_MONTH_CTRL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();



            sb.Append(" delete from TB_S_M_SALARY_MONTH_CTRL");
            sb.Append(" where OPERATION_ID = 'D01' and SALARY_YM = @SALARY_YM");

            ht.Add("@SALARY_YM", MANAGER_YM);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getSalaryCTL()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SELECT SALARY_LOCKED");
            sb.Append(" FROM TB_S_M_SALARY_MONTH_CTRL");
            sb.Append(" where OPERATION_ID = 'D01' and SALARY_YM = @SALARY_YM");
            sb.Append(" and SALARY_DT = @SALARY_DT");
            sb.Append(" and SALARY_TYPE = @SALARY_TYPE");

            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable update_Month()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_R_ACCOM_MONTH_EXP");
            sb.Append(" set SALARY_TAKE_OUT_DT = getdate(),");
            sb.Append(" SALARY_TAKE_OUT_BY = @TAKE_OUT_BY,");
            sb.Append(" STATUS = 'Y',");
            sb.Append(" GIVE_SALARY_DT = @SALARY_DT,");
            sb.Append(" REMARK = '本月份已經結帳，請至【其他加扣款資料維護】上傳資料',");
            sb.Append(" UPDATED_BY = @TAKE_OUT_BY,");
            sb.Append(" UPDATED_DT = getdate()");
            sb.Append(" where MANAGER_YM =@MANAGER_YM");


            ht.Add("@TAKE_OUT_BY", TAKE_OUT_BY);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@MANAGER_YM", YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public void updateSALARY_MONTH_CTRL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();



            sb.Append(" update TB_S_M_SALARY_MONTH_CTRL");
            sb.Append(" set PROCESS_DT = getdate(),START_DT=@START_DT,END_DT = @END_DT,FUNC_ID=@FUNC_ID");
            sb.Append(" where SALARY_TYPE = @SALARY_TYPE and SALARY_YM = @SALARY_YM and SALARY_DT = @SALARY_DT");
            sb.Append(" and OPERATION_ID = @OPERATION_ID");

            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@OPERATION_ID", OPERATION_ID);
            ht.Add("@START_DT", SALARY_SDT);
            ht.Add("@END_DT", SALARY_EDT);
            ht.Add("@FUNC_ID", FUNC_ID);


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable update_MONTH_FIN()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_R_ACCOM_MONTH_EXP");
            sb.Append(" set SALARY_TAKE_OUT_DT = getdate(),");
            sb.Append(" SALARY_TAKE_OUT_BY = @TAKE_OUT_BY,");
            sb.Append(" STATUS = 'Y',");
            sb.Append(" GIVE_SALARY_DT = @SALARY_DT,");
            sb.Append(" UPDATED_BY = @TAKE_OUT_BY,");
            sb.Append(" UPDATED_DT = getdate()");
            sb.Append(" where MANAGER_YM =@MANAGER_YM");


            ht.Add("@TAKE_OUT_BY", TAKE_OUT_BY);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@MANAGER_YM", YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public void insertSALARY_MONTH_CTRL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();



            sb.Append(" insert into TB_S_M_SALARY_MONTH_CTRL");
            sb.Append(" (SALARY_TYPE,SALARY_YM,SALARY_DT,OPERATION_ID,PROCESS_DT,START_DT,END_DT,SALARY_LOCKED,LOCK_DT,FUNC_ID)");
            sb.Append(" values(@SALARY_TYPE,@SALARY_YM,@SALARY_DT,@OPERATION_ID,getdate(),@START_DT,@END_DT,'','9999/12/31',@FUNC_ID)");


            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@OPERATION_ID", OPERATION_ID);
            ht.Add("@START_DT", SALARY_SDT);
            ht.Add("@END_DT", SALARY_EDT);
            ht.Add("@FUNC_ID", FUNC_ID);



            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    


}