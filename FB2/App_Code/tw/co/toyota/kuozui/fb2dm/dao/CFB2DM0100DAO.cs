using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;


/// <summary>
/// CFB2DM0100DAO 的摘要描述
/// </summary>
public class CFB2DM0100DAO : BaseDAO
{
    public object DUTY_YM { get; set; }

    public object DUTY_SDT { get; set; }

    public object DUTY_EDT { get; set; }

    public object SALARY_DT { get; set; }

    public object CFN_FLAG1 { get; set; }

    public object CFN_FLAG2 { get; set; }

    public object CFN_FLAG3 { get; set; }
    public CFB2DM0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    internal DataTable getSalaryDT(string salary_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select top 1 CONVERT(char(10),SALARY_DT,111)SALARY_DT,SALARY_YM,CONVERT(char(10),DUTY_SDT,111)DUTY_SDT,CONVERT(char(10),DUTY_EDT,111)DUTY_EDT ");
            sb.Append("from TB_S_M_SALARY_CAL_H ");
            sb.Append("where SALARY_YM=LEFT(@SALARY_YM,4)+RIGHT(@SALARY_YM,2) ");
            sb.Append("and SALARY_TYPE='A' ");
            sb.Append("and PROCESS_STATUS<=2 ");
            sb.Append("and SALARY_CLOSED<> 'Y' ");
            sb.Append("order by SALARY_DT desc");

            ht.Add("@SALARY_YM", salary_ym);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal string SP_D_DUTY_CLOSE()
    {

        try
        {
            int rtnMessage = 0;

            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_D_DUTY_CLOSE";
                comm.Parameters.AddWithValue("@p_CLOSE_YM", DUTY_YM);
                comm.Parameters.AddWithValue("@p_SALARY_DT", SALARY_DT);
                comm.Parameters.AddWithValue("@p_CALENDAR_SDT", DUTY_SDT);
                comm.Parameters.AddWithValue("@p_CALENDAR_EDT", DUTY_EDT);
                comm.Parameters.AddWithValue("@p_SPECIAL_LEAVE", CFN_FLAG1);
                comm.Parameters.AddWithValue("@p_HOLIDAY_CHG", CFN_FLAG2);
                comm.Parameters.AddWithValue("@p_HONOR_LEAVE", CFN_FLAG3);
                comm.Parameters.AddWithValue("@p_UserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@p_FuncID", "FB2DM010");
                comm.Parameters.Add("@pErr", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
                comm.CommandTimeout = 1000;
                comm.ExecuteNonQuery();
                rtnMessage = (int)comm.Parameters["@pErr"].Value;

                conn.Close();
            }
            return rtnMessage.ToString();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }


    }


    internal DataTable getSalaryCTL(string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select * from ");
            sb.Append("(select a.SALARY_TYPE, b.SALARY_DT,b.OPERATION_ID,SALARY_LOCKED ");
            sb.Append("from TB_S_M_SALARY_CAL_H a,TB_S_M_SALARY_MONTH_CTRL b ");
            sb.Append("where b.SALARY_TYPE=a.SALARY_TYPE ");
            sb.Append("and b.SALARY_DT=@SALARY_DT ");
            sb.Append(")A join ");

            sb.Append("(select c.OPERATION_ID,c.OPERATION_NAME ");
            sb.Append("from TB_S_M_SALARY_CTRL c ");
            sb.Append("where OPERATION_ID='B01' ");
            sb.Append(")B on A.OPERATION_ID = B.OPERATION_ID ");

            ht.Add("@SALARY_DT", salary_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
}