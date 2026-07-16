using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2DD0300DAO 的摘要描述
/// </summary>
public class CFB2DD0300DAO : BaseDAO
{
	public CFB2DD0300DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string MANAGER_YM, string FACTORY_CD,
                            string EMP_ID, string EMP_NAME, string ALLOWANCE_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();            

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" MANAGER_YM,EMP_ID,EMP_NAME,LEVEL_CD,AGE,ALLOWANCE_CD+'-'+b.SUB_DESC ALLOWANCE_CD,TOTAL_PAY,DAILY_PAY,WORKING_DT,REPLACE(CONVERT(char(10), START_DT, 120),'-','/')START_DT,");
            sb.Append(" FACTORY_CD+'-'+c.SUB_DESC FACTORY_CD,AREA_CD+'-'+d.SUB_DESC AREA_CD,TRANSPORT_CD+'-'+e.SUB_DESC TRANSPORT_CD,");
            sb.Append(" KILOMETER_AMOUNT,FARE_PRICE,(select CASE WHEN SINGLE_TRIP ='Y' THEN '是' ELSE '否' END) SINGLE_TRIP,");
            sb.Append(" LINE_CD+'-'+f.SUB_DESC LINE_CD,STATION_CD+'-'+g.SUB_DESC STATION_CD,BELONG_TO_DT");
            sb.Append(" from TB_D_R_TRANS_MONTH_D a");
            sb.Append(" left join TB_9_M_COMM_D b on a.ALLOWANCE_CD = b.SUB_CD");
            sb.Append(" and b.SYS_CD='DD' and b.MAIN_CD = 'ALLOWANCE_CD' and b.IS_VALID='Y'");
            sb.Append(" left join TB_9_M_COMM_D c on a.FACTORY_CD = c.SUB_CD");
            sb.Append(" and c.SYS_CD='DD' and c.MAIN_CD = 'ALLOWANCE_PLANT_CD' and c.IS_VALID='Y'");
            sb.Append(" left join TB_9_M_COMM_D d on a.AREA_CD = d.SUB_CD");
            sb.Append(" and d.SYS_CD='DD' and d.MAIN_CD = 'AREA_CD' and d.IS_VALID='Y'");
            sb.Append(" left join TB_9_M_COMM_D e on a.TRANSPORT_CD = e.SUB_CD");
            sb.Append(" and e.SYS_CD='DD' and e.MAIN_CD = 'TRANSPORT_CD' and e.IS_VALID='Y'");
            sb.Append(" left join TB_9_M_COMM_D f on a.LINE_CD = f.SUB_CD");
            sb.Append(" and f.SYS_CD='DD' and f.MAIN_CD = 'LINE_CD' and f.IS_VALID='Y'");
            sb.Append(" left join TB_9_M_COMM_D g on a.STATION_CD = g.SUB_CD");
            sb.Append(" and g.SYS_CD='DD' and g.MAIN_CD = 'STATION_CD' and g.IS_VALID='Y'");
            sb.Append(" where 1=1");

            if (MANAGER_YM != "")
            {
                sb.Append(" and MANAGER_YM = @MANAGER_YM ");
                ht.Add("@MANAGER_YM", MANAGER_YM.Replace("/", ""));               
            }
            if (FACTORY_CD != "-1")
            {
                sb.Append(" and FACTORY_CD = @FACTORY_CD ");
                ht.Add("@FACTORY_CD", FACTORY_CD);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID + "%");
            }
            if (EMP_NAME != "")
            {
                sb.Append(" and a.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", "%" + EMP_NAME.Trim() + "%");
            }
            if (ALLOWANCE_CD != "-1")
            {
                sb.Append(" and ALLOWANCE_CD = @ALLOWANCE_CD ");
                ht.Add("@ALLOWANCE_CD", ALLOWANCE_CD);
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
    public int getCount(int startRowIndex, int maximumRows, string MANAGER_YM, string FACTORY_CD,
                            string EMP_ID, string EMP_NAME, string ALLOWANCE_CD)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_D_R_TRANS_MONTH_D");
            sb.Append(" where 1=1");

            if (MANAGER_YM != "")
            {
                sb.Append(" and MANAGER_YM = @MANAGER_YM ");
                ht.Add("@MANAGER_YM", MANAGER_YM.Replace("/", ""));
            }
            if (FACTORY_CD != "-1")
            {
                sb.Append(" and FACTORY_CD = @FACTORY_CD ");
                ht.Add("@FACTORY_CD", FACTORY_CD);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID + "%");               
            }
            if (EMP_NAME != "")
            {
                sb.Append(" and EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", "%" + EMP_NAME.Trim() + "%");                
            }
            if (ALLOWANCE_CD != "-1")
            {
                sb.Append(" and ALLOWANCE_CD = @ALLOWANCE_CD ");
                ht.Add("@ALLOWANCE_CD", ALLOWANCE_CD);
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



}