using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2DD0100DAO 的摘要描述
/// </summary>
public class CFB2DD0100DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string IS_CANCEL { get; set; }
    public string IS_CALCULATE { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    //明細畫面的參數
    public string D_EMP_ID { get; set; }
    public string D_APPLICATION_NO { get; set; }
    public string D_START_DT { get; set; }
    public string D_FACTORY_CD { get; set; }
    public string D_AREA_CD { get; set; }
    public string D_TRANSPORT_CD { get; set; }
    public string D_LINE_CD { get; set; }
    public string D_STATION_CD { get; set; }
    public string D_KILOMETER_AMOUNT { get; set; }
    public string D_FARE_PRICE { get; set; }
    public string D_SINGLE_TRIP { get; set; }
    public string D_DAILY_PAY { get; set; }
    public string D_REMARK { get; set; }
    public string D_ADDRESS { get; set; }
    public string D_IS_CANCEL { get; set; }
    public string D_IS_CALCULATE { get; set; }
    public string D_CHG_REASON { get; set; }
    public string DAILY_PAY { get; set; }
    public string PLANT_CD { get; set; }

    public string CL_KM { get; set; }
    public string KN_KM { get; set; }
    public string CL_FR { get; set; }
    public string KN_FR { get; set; }
    

	public CFB2DD0100DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string emp_name, string is_calculate,
                            string dept_no, string plant_cd, string is_cancel)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,b.EMP_NAME,b.DEPT_NO,b.DEPT_NO +'-'+ b.DEPT_NAME DEPT_NAME,b.PLANT_CD +'-'+ c.SUB_DESC PLANT_CD,");
            sb.Append(" (Select IS_CANCEL =CASE WHEN IS_CANCEL ='Y' THEN '是'  ELSE CASE WHEN IS_CANCEL ='N' THEN '否'  END  END) IS_CANCEL,");
            sb.Append(" (Select IS_CALCULATE =CASE WHEN IS_CALCULATE ='1' THEN '是'  ELSE CASE WHEN IS_CALCULATE ='0' THEN '否'  END  END ) IS_CALCULATE");
            sb.Append(" from TB_D_M_TRANS_ALLOWANCE_M a");
            sb.Append(" left join VW_H_EMP_DATA b");
            sb.Append(" on a.EMP_ID = b.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D c");
            sb.Append(" on b.PLANT_CD = c.SUB_CD");
            sb.Append(" and c.SYS_CD ='HB' and c.MAIN_CD = 'PLANT_CD' and c.IS_VALID ='Y'");
            sb.Append(" where 1=1");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "")
            {
                sb.Append(" and b.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", "%" + emp_name.Trim() + "%");
            }
            if (is_calculate != "")
            {
                sb.Append(" and IS_CALCULATE = @IS_CALCULATE ");
                ht.Add("@IS_CALCULATE", is_calculate);
            }

            if (dept_no != "")
            {

                sb.Append(" and b.DEPT_NO like @DEPT_NO");
                ht.Add("@DEPT_NO", dept_no+"%");

            }
            if (plant_cd != "-1")
            {
                sb.Append(" and b.PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            if (is_cancel != "")
            {
                sb.Append(" and IS_CANCEL = @IS_CANCEL ");
                ht.Add("@IS_CANCEL", is_cancel);
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
    public int getCount(int startRowIndex, int maximumRows, string emp_id, string emp_name, string is_calculate,
                            string dept_no, string plant_cd, string is_cancel)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_TRANS_ALLOWANCE_M a");            
            sb.Append(" left join VW_H_EMP_DATA b");
            sb.Append(" on a.EMP_ID = b.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D c");
            sb.Append(" on b.PLANT_CD = c.SUB_CD");
            sb.Append(" and c.SYS_CD ='HB' and c.MAIN_CD = 'PLANT_CD' and c.IS_VALID ='Y'");
            sb.Append(" where 1=1");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "")
            {
                sb.Append(" and b.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", "%" + emp_name.Trim() + "%");
            }
            if (is_calculate != "")
            {
                sb.Append(" and a.IS_CALCULATE = @IS_CALCULATE ");
                ht.Add("@IS_CALCULATE", is_calculate);
            }

            if (dept_no != "")
            {

                sb.Append(" and b.DEPT_NO like @DEPT_NO");
                ht.Add("@DEPT_NO", dept_no + "%");

            }
            if (plant_cd != "-1")
            {
                sb.Append(" and b.PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            if (is_cancel != "")
            {
                sb.Append(" and a.IS_CANCEL = @IS_CANCEL ");
                ht.Add("@IS_CANCEL", is_cancel);
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

    internal DataTable getEMPData(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select b.EMP_NAME,b.DEPT_NO,b.DEPT_NO +' '+ b.DEPT_NAME DEPT_NAME,b.PLANT_CD +' '+ c.SUB_DESC PLANT_NAME");
            sb.Append(" from VW_H_EMP_DATA b,TB_9_M_COMM_D c");
            sb.Append(" where b.PLANT_CD = c.SUB_CD");            
            sb.Append(" and c.SYS_CD ='HB' and c.MAIN_CD = 'PLANT_CD' and c.IS_VALID ='Y'");
            if (EMP_ID != "")
            {
                sb.Append(" and b.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }
           
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkEmp_id(string empid)
    {
        try
        {            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select * from TB_D_M_TRANS_ALLOWANCE_M");
            sb.Append(" where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", empid);
            
            DataTable dt = dbConn.Query(sb, ht);
            
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insertEmp()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_D_M_TRANS_ALLOWANCE_M ");


            sb.Append("(EMP_ID,IS_CANCEL,IS_CALCULATE," +
                    "CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values (@EMP_ID,@IS_CANCEL,@IS_CALCULATE,");
            sb.Append(" @CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID)");

            if (EMP_ID != "")
            {              
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (IS_CANCEL != "")
            {
                ht.Add("@IS_CANCEL", IS_CANCEL);               
            }

            if (IS_CALCULATE != "")
            {
                ht.Add("@IS_CALCULATE", IS_CALCULATE);
            }
            if (CREATED_BY != "")
            {
                ht.Add("@CREATED_BY", CREATED_BY);
            }
            if (UPDATED_BY != "")
            {
                ht.Add("@UPDATED_BY", UPDATED_BY);
            }
            if (FUNC_ID != "")
            {
                ht.Add("@FUNC_ID", FUNC_ID);
            }

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void updateEmp()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_TRANS_ALLOWANCE_M ");
            sb.Append(" set IS_CANCEL = @IS_CANCEL,IS_CALCULATE = @IS_CALCULATE,UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE()");
            sb.Append(" where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IS_CANCEL", IS_CANCEL);
            ht.Add("@IS_CALCULATE", IS_CALCULATE);
            ht.Add("@UPDATED_BY", UPDATED_BY);
           
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable selectEmpData(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select EMP_NAME,WS_CD+' '+WS_DESC WS_CD,DIV_DEPT_FULL_NAME as DEPT_FULL_NAME,EMP_CHG_CD +' '+EMP_CHG_DESC EMP_CHG_CD,EMP_CD+' '+EMP_DESC EMP_CD,REPLACE(CONVERT(char(10), JOIN_DT, 120),'-','/') JOIN_DT,WORK_SHIFT_CD+' '+WORK_SHIFT_DESC WORK_SHIFT_CD,");
            sb.Append(" LEVEL_CD,PJOB_CD+' '+PJOB_DESC PJOB_CD,AGE,CONTACT_TEL,MOBILE_TEL_1,PLANT_CD+' '+PLANT_NAME PLANT_DESC,PLANT_CD,CONTACT_ADDR,REGISTER_ADDR");
            sb.Append(" from VW_H_EMP_DATA");
            sb.Append(" where EMP_ID = @EMP_ID");
           
            ht.Add("@EMP_ID", EMP_ID);
           

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
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
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select count(*)ct from TB_D_M_TRANS_ALLOWANCE_D d");
            sb.Append(" where d.application_no = @application_no and d.START_DT =");
            sb.Append(" (select max(START_DT) from TB_D_M_TRANS_ALLOWANCE_D where emp_id = @emp_id)");

            ht.Add("@application_no", application_no);
            ht.Add("@emp_id", emp_id);


            DataTable dt = dbConn.Query(sb, ht);

            return dt.Rows[0]["ct"].ToString();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable selectCarData(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select a.CAR_NO,a.PARKING_TOOL +'-'+b.SUB_DESC PARKING_TOOL");          
            sb.Append(" from TB_D_M_PARKING_EMP_MAIN a,TB_9_M_COMM_D b");
            sb.Append(" where a.PARKING_TOOL = b.SUB_CD");           
            sb.Append(" and b.SYS_CD ='DG' and b.MAIN_CD = 'PARKING_CD' and b.IS_VALID ='Y'");
            sb.Append(" and a.EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);


            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }


    //Gridview 查詢資料
    public DataTable getData1(int startRowIndex, int maximumRows, string sortExpression, string emp_id)
    {
        try
        {
            //if (sortExpression.Contains("EMP_ID"))
            //{
            //    sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            //}

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" EMP_ID,APPLICATION_NO,REPLACE(CONVERT(char(10), START_DT, 120),'-','/') START_DT,REPLACE(CONVERT(char(10), END_DT, 120),'-','/') END_DT,(Select IS_CANCEL =CASE WHEN IS_CANCEL ='Y' THEN '是'  ELSE");
            sb.Append(" CASE WHEN IS_CANCEL ='N' THEN '否'  END  END) IS_CANCEL,(Select IS_CALCULATE =CASE WHEN IS_CALCULATE ='1' THEN '是'  ELSE");
            sb.Append(" CASE WHEN IS_CALCULATE ='0' THEN '否'  END  END ) IS_CALCULATE,FACTORY_CD+'-'+ b.SUB_DESC FACTORY_DESC,");
            sb.Append(" AREA_CD+'-'+c.SUB_DESC AREA_DESC,TRANSPORT_CD+'-'+d.SUB_DESC TRANSPORT_DESC,");
            sb.Append(" LINE_CD+'-'+e.SUB_DESC LINE_DESC,KILOMETER_AMOUNT,FARE_PRICE,SINGLE_TRIP,DAILY_PAY,ADDRESS,a.REMARK,");
            sb.Append(" IFLOW_NO,CHG_REASON+'-'+f.SUB_DESC CHG_REASON_DESC,STATION_CD,CHG_REASON,FACTORY_CD,AREA_CD,TRANSPORT_CD,LINE_CD");
            sb.Append(" from TB_D_M_TRANS_ALLOWANCE_D a");
            sb.Append(" left join TB_9_M_COMM_D b");
            sb.Append(" on a.FACTORY_CD = b.SUB_CD");                      
            sb.Append(" and b.SYS_CD ='DD' and b.MAIN_CD = 'ALLOWANCE_PLANT_CD' and b.IS_VALID ='Y'");
            sb.Append(" left join TB_9_M_COMM_D c");
            sb.Append(" on a.AREA_CD = c.SUB_CD");
            sb.Append(" and c.SYS_CD ='DD' and c.MAIN_CD = 'AREA_CD' and c.IS_VALID ='Y'");
            sb.Append(" left join TB_9_M_COMM_D d");
            sb.Append(" on a.TRANSPORT_CD = d.SUB_CD");
            sb.Append(" and d.SYS_CD ='DD' and d.MAIN_CD = 'TRANSPORT_CD' and d.IS_VALID ='Y'");
            sb.Append(" left join TB_9_M_COMM_D e");
            sb.Append(" on a.LINE_CD = e.SUB_CD");
            sb.Append(" and e.SYS_CD ='DD' and e.MAIN_CD = 'LINE_CD' and e.IS_VALID ='Y'");
            sb.Append(" left join TB_9_M_COMM_D f");
            sb.Append(" on a.CHG_REASON = f.SUB_CD");
            sb.Append(" and f.SYS_CD ='DD' and f.MAIN_CD = 'CHG_REASON' and f.IS_VALID ='Y'");

            sb.Append(" where a.EMP_ID = @EMP_ID");
            

            if (emp_id != "")
            {               
                ht.Add("@EMP_ID", emp_id);
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
    public int getCount1(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_TRANS_ALLOWANCE_D a");
            sb.Append(" where a.EMP_ID = @EMP_ID");


            if (emp_id != "")
            {
                ht.Add("@EMP_ID", emp_id);
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
    //取得每公里補助多少錢
    internal DataTable getCode_Val(string sub_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CODE_VAL1 from TB_9_M_COMM_D where MAIN_CD = 'TRANSPORT_CD' and SUB_CD = @SUB_CD ");
            ht.Add("@SUB_CD", sub_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getCommCode(string SYS_CD, string main_cd, string CODE_VAL1, string CODE_VAL2)
    {

        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select sub_cd ,sub_cd+'-'+sub_desc sub_desc From TB_9_M_COMM_D Where main_cd = @main_cd and SYS_CD=@SYS_CD");
            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@main_cd", main_cd);
            if (CODE_VAL1 != "")
            {
                sb.Append(" and CODE_VAL1 = @CODE_VAL1");
                ht.Add("@CODE_VAL1", CODE_VAL1);
            }
            if (CODE_VAL2 != "")
            {
                sb.Append(" and CODE_VAL2 = @CODE_VAL2");
                ht.Add("@CODE_VAL2", CODE_VAL2);
            }

            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }

    public DataTable getKM(string SYS_CD, string main_cd, string SUB_CD, string CODE_VAL1, string CODE_VAL2)
    {

        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select CODE_VAL1 ,CODE_VAL2 From TB_9_M_COMM_D Where main_cd = @main_cd and SYS_CD=@SYS_CD");
            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@main_cd", main_cd);
            if (SUB_CD != "")
            {
                sb.Append(" and SUB_CD=@SUB_CD");
                ht.Add("@SUB_CD", SUB_CD);
            }
            
            if (CODE_VAL1 != "")
            {
                sb.Append(" and CODE_VAL1 = @CODE_VAL1");
                ht.Add("@CODE_VAL1", CODE_VAL1);
            }
            if (CODE_VAL2 != "")
            {
                sb.Append(" and CODE_VAL2 = @CODE_VAL2");
                ht.Add("@CODE_VAL2", CODE_VAL2);
            }

            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }

    public DataTable getCOM(string SYS_CD, string main_cd, string SUB_CD, string CODE_VAL1, string CODE_VAL2)
    {

        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select sub_cd ,sub_cd+'-'+sub_desc sub_desc From TB_9_M_COMM_D Where main_cd = @main_cd and SYS_CD=@SYS_CD");
            sb.Append(" order by SUB_CD");
            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@main_cd", main_cd);
            if (SUB_CD != "")
            {
                sb.Append(" and SUB_CD = @SUB_CD");
                ht.Add("@SUB_CD", SUB_CD);
            }
            if (CODE_VAL1 != "")
            {
                sb.Append(" and CODE_VAL1 = @CODE_VAL1");
                ht.Add("@CODE_VAL1", CODE_VAL1);
            }
            if (CODE_VAL2 != "")
            {
                sb.Append(" and CODE_VAL2 = @CODE_VAL2");
                ht.Add("@CODE_VAL2", CODE_VAL2);
            }

            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }


    public DataTable getEMP_DATA(string EMP_ID)
    {

        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select REPLACE(CONVERT(char(10), BIRTH_DT, 120),'-','/') BIRTH_DT ,LEVEL_CD ,JPN_CD,b.TRANSFER_REASON ");
            sb.Append(" From VW_H_EMP_DATA a");
            sb.Append(" left join TB_H_R_EMP_TRANSFER b");
            sb.Append(" on a.EMP_ID = b.EMP_ID");
            sb.Append(" and b.START_DT = (select MAX(START_DT) from TB_H_R_EMP_TRANSFER where EMP_ID = @EMP_ID)");            
            sb.Append(" Where a.EMP_ID = @EMP_ID");
            sb.Append(" and GETDATE() > b.START_DT and GETDATE() < ISNULL(b.END_DT,'9999/12/31')");
            sb.Append(" and b.TRANSFER_REASON = 'B09'");

            ht.Add("@EMP_ID", EMP_ID);          

            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }

    public void insertTRANS_ALLOWANCE_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_D_M_TRANS_ALLOWANCE_D ");
            sb.Append("(EMP_ID,APPLICATION_NO,LICENSE_ID,START_DT,END_DT,FACTORY_CD,AREA_CD,");
            sb.Append("LINE_CD,STATION_CD,DAILY_PAY,TRANSPORT_CD,KILOMETER_AMOUNT,FARE_PRICE,SINGLE_TRIP,");
            sb.Append("ADDRESS,REMARK,IS_CANCEL,IS_CALCULATE,CHG_REASON,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @EMP_ID,@APPLICATION_NO,LICENSE_ID,@START_DT,'9999/12/31',@FACTORY_CD,@AREA_CD,");
            sb.Append("@LINE_CD,@STATION_CD,@DAILY_PAY,@TRANSPORT_CD,@KILOMETER_AMOUNT,@FARE_PRICE,@SINGLE_TRIP,");
            sb.Append("@ADDRESS,@REMARK,@IS_CANCEL,@IS_CALCULATE,@CHG_REASON,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID");
            sb.Append(" from TB_H_M_EMP");
            sb.Append(" where 1=1 and EMP_ID = @EMP_ID");

            ht.Add("@EMP_ID", D_EMP_ID);
            ht.Add("@APPLICATION_NO", D_APPLICATION_NO);
            ht.Add("@START_DT", D_START_DT);
            ht.Add("@FACTORY_CD", D_FACTORY_CD);
            ht.Add("@AREA_CD", D_AREA_CD);
            ht.Add("@LINE_CD", D_LINE_CD);
            ht.Add("@STATION_CD", D_STATION_CD);
            ht.Add("@DAILY_PAY", D_DAILY_PAY);
            ht.Add("@TRANSPORT_CD", D_TRANSPORT_CD);
            ht.Add("@KILOMETER_AMOUNT", D_KILOMETER_AMOUNT);
            ht.Add("@FARE_PRICE", D_FARE_PRICE);
            ht.Add("@SINGLE_TRIP", D_SINGLE_TRIP);
            ht.Add("@ADDRESS", D_ADDRESS);
            ht.Add("@REMARK", D_REMARK);
            ht.Add("@IS_CANCEL", D_IS_CANCEL);
            ht.Add("@IS_CALCULATE", D_IS_CALCULATE);
            ht.Add("@CHG_REASON", D_CHG_REASON);
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

    public string getCol()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select CODE_VAL1 From TB_9_M_COMM_D Where main_cd = 'PLANT_CD' and SYS_CD='HB'");
            sb.Append(" and SUB_CD =@SUB_CD");
            ht.Add("@SUB_CD", PLANT_CD);
          
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0) {
                st = dt.Rows[0]["CODE_VAL1"].ToString();
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public DataTable checkSEQ(string st)
    {           
        DBConnector dbConn = new DBConnector();
        try
        {   
            string mon = Convert.ToString(DateTime.Now.Month).Length == 2 ? Convert.ToString(DateTime.Now.Month) : "0"+Convert.ToString(DateTime.Now.Month);
            string word = Convert.ToString(DateTime.Now.Year)+ mon;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select MAX(APPLICATION_NO) APPLICATION_NO From TB_D_M_TRANS_ALLOWANCE_D Where APPLICATION_NO like @APPLICATION_NO");

            ht.Add("@APPLICATION_NO", st +"-"+ word + "%");

            DataTable dt = dbConn.Query(sb, ht);
           

            return dt;
        }
        catch
        {
            throw;
        }
    }

    public string getPrice()
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select CODE_VAL1 From TB_9_M_COMM_D Where main_cd = 'TRANSPORT_CD' and SYS_CD='DD'");
            sb.Append(" and SUB_CD =@SUB_CD");
            ht.Add("@SUB_CD", D_TRANSPORT_CD);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["CODE_VAL1"].ToString();
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getOldData(string APPLICATION_NO)
    {        
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select EMP_ID,APPLICATION_NO,REPLACE(CONVERT(char(10), START_DT, 120),'-','/') START_DT,IS_CANCEL,IS_CALCULATE,");
            sb.Append(" FACTORY_CD,AREA_CD,TRANSPORT_CD,LINE_CD,KILOMETER_AMOUNT,FARE_PRICE,SINGLE_TRIP,");
            sb.Append(" DAILY_PAY,ADDRESS,REMARK,IFLOW_NO,CHG_REASON");
            sb.Append(" From TB_D_M_TRANS_ALLOWANCE_D Where APPLICATION_NO = @APPLICATION_NO");

            ht.Add("@APPLICATION_NO", APPLICATION_NO);

            DataTable dt = dbConn.Query(sb, ht);
           
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public void delData(string APPLICATION_NO, string EMP_ID)
    {        
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_TRANS_ALLOWANCE_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DD010' ");
            sb.Append(" Where APPLICATION_NO = @APPLICATION_NO and EMP_ID= @EMP_ID ;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);            

            sb.Append(" Delete From TB_D_M_TRANS_ALLOWANCE_D Where  APPLICATION_NO=@APPLICATION_NO and EMP_ID= @EMP_ID; ");

            ht.Add("@APPLICATION_NO", APPLICATION_NO);
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);

            
        }
        catch
        {
            throw;
        }
    }

    public void updateData(string EMP_ID)
    {
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_TRANS_ALLOWANCE_D set END_DT = '9999/12/31'");
            sb.Append(" ,UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DD010'");
            sb.Append(" where START_DT = (select max(START_DT) from TB_D_M_TRANS_ALLOWANCE_D where START_DT< (select max(START_DT) from TB_D_M_TRANS_ALLOWANCE_D) and EMP_ID = @EMP_ID)");
            sb.Append(" and EMP_ID = @EMP_ID");

            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }

    public void updateMain(string EMP_ID)
    {
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //update b   set   ClientName    = a.name    from a,b    where a.id = b.id  
            sb.Append("update TB_D_M_TRANS_ALLOWANCE_M set IS_CANCEL = b.IS_CANCEL,IS_CALCULATE = b.IS_CALCULATE");
            sb.Append(" ,UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DD010'");
            sb.Append(" from TB_D_M_TRANS_ALLOWANCE_M a,TB_D_M_TRANS_ALLOWANCE_D b");
            sb.Append(" where a.EMP_ID = b.EMP_ID");
            sb.Append(" and b.START_DT = (select max(START_DT) from TB_D_M_TRANS_ALLOWANCE_D where START_DT< (select max(START_DT) from TB_D_M_TRANS_ALLOWANCE_D) and EMP_ID = @EMP_ID)");
            sb.Append(" and a.EMP_ID = @EMP_ID");

            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }

    public void delMain(string EMP_ID)
    {
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_TRANS_ALLOWANCE_M set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DD010' ");
            sb.Append(" Where EMP_ID=@EMP_ID; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" Delete From TB_D_M_TRANS_ALLOWANCE_M Where  EMP_ID=@EMP_ID; ");

            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }

    public void updateM_New()
    {
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            sb.Append("update TB_D_M_TRANS_ALLOWANCE_M set IS_CANCEL = @IS_CANCEL,IS_CALCULATE = @IS_CALCULATE");
            sb.Append(" ,UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DD010'");
            sb.Append(" where EMP_ID = @EMP_ID");

            ht.Add("@IS_CANCEL", D_IS_CANCEL);
            ht.Add("@IS_CALCULATE", D_IS_CALCULATE);
            ht.Add("@EMP_ID", D_EMP_ID);
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            dbConn.ExecuteT(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }

    public void updateD_New()
    {
        DBConnector dbConn = new DBConnector();
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_D_M_TRANS_ALLOWANCE_D set END_DT = @START_DT");
            sb.Append(" ,UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DD010'");
            sb.Append(" where START_DT = (select max(START_DT) from TB_D_M_TRANS_ALLOWANCE_D where  EMP_ID = @EMP_ID");
            sb.Append(" and START_DT< (select max(START_DT) from TB_D_M_TRANS_ALLOWANCE_D where  EMP_ID = @EMP_ID))");
            sb.Append(" and EMP_ID = @EMP_ID");

            ht.Add("@START_DT", D_START_DT);          
            ht.Add("@EMP_ID", D_EMP_ID);
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            dbConn.ExecuteT(sb, ht, true);


        }
        catch
        {
            throw;
        }
    }



}