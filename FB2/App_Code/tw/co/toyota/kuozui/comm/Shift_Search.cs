using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// Shift_Search 的摘要描述
/// </summary>
public class Shift_Search : BaseDAO
{
    public string CALENDAR_DT { get; set; }
    public string SHIFT_TIME_CD { get; set; }
    public string SHIFT_DESC { get; set; }

    public Shift_Search()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public System.Data.DataTable getShiftData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * from VW_D_R_SHIFT_H where SHIFT_CD is not null ");
            if (CALENDAR_DT != "")
            {
                sb.Append("and @CALENDAR_DT >= START_DT and @CALENDAR_DT <= END_DT ");
                ht.Add("@CALENDAR_DT", CALENDAR_DT);
            }
            if (SHIFT_TIME_CD != "-1")
            {
                sb.Append(" and SHIFT_TIME_CD = @SHIFT_TIME_CD");
                ht.Add("@SHIFT_TIME_CD", SHIFT_TIME_CD);
            }
            if (SHIFT_DESC != "")
            {
                sb.Append(" and SHIFT_DESC like @SHIFT_DESC");
                ht.Add("@SHIFT_DESC", "%" + SHIFT_DESC + "%");
            }


            sb.Append(" order by " + sortExpression);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}