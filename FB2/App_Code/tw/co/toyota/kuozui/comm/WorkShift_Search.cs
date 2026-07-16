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
/// WorkShift_Search 的摘要描述
/// </summary>
public class WorkShift_Search : BaseDAO
{

    public string WORK_SHIFT_CD { get; set; }
    public string WORK_SHIFT_DESC { get; set; }
    public string CALENDAR_CD { get; set; }
    public string CALENDAR_DESC { get; set; }
    public string WORKER_WORK_SHIFT { get; set; }

    public WorkShift_Search()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getCalendarCd()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select CALENDAR_CD + '-' + CALENDAR_DESC CALENDAR_DESC,CALENDAR_CD from TB_D_M_CALENDAR_H where IS_VALID='Y' order by CALENDAR_CD  ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getWorkShiftData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * from TB_D_M_WORK_SHIFT_H a,TB_D_M_CALENDAR_H b where a.CALENDAR_CD = b.CALENDAR_CD ");

            if (WORK_SHIFT_CD != "")
            {
                sb.Append(" and WORK_SHIFT_CD like @WORK_SHIFT_CD");
                ht.Add("@WORK_SHIFT_CD", "%" + WORK_SHIFT_CD + "%");
            }
            if (WORK_SHIFT_DESC != "")
            {
                sb.Append(" and WORK_SHIFT_DESC like @WORK_SHIFT_DESC");
                ht.Add("@WORK_SHIFT_DESC", "%" + WORK_SHIFT_DESC + "%");
            }
            if (CALENDAR_CD != "-1")
            {
                sb.Append(" and a.CALENDAR_CD = @CALENDAR_CD");
                ht.Add("@CALENDAR_CD", CALENDAR_CD);
            }
            if (CALENDAR_DESC != "")
            {
                sb.Append(" and CALENDAR_DESC like @CALENDAR_DESC");
                ht.Add("@CALENDAR_DESC", "%" + CALENDAR_DESC + "%");
            }
            if (WORKER_WORK_SHIFT == "W")
            {
                sb.Append(" and exists (select SUB_CD from  TB_9_M_COMM_D where SYS_CD = 'HC' and MAIN_CD = 'WORKER_WORK_SHIFT' and SUB_CD = a.WORK_SHIFT_CD )");
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