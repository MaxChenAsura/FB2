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
/// Pjob_Search 的摘要描述
/// </summary>
public class Pjob_Search : BaseDAO
{

    public string PJOB_CD { get; set; }
    public string PJOB_DESC { get; set; }
    public string WS_CD { get; set; }
    public string LEVEL_CD { get; set; }
    public string START_DT { get; set; }

    public Pjob_Search()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //

    }

    public DataTable getPjobData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * from TB_H_M_PJOB where PJOB_CD is not null ");

            if (PJOB_CD != "")
            {
                sb.Append(" and PJOB_CD like @PJOB_CD");
                ht.Add("@PJOB_CD", "%" + PJOB_CD + "%");
            }
            if (PJOB_DESC != "")
            {
                sb.Append(" and PJOB_DESC like @PJOB_DESC");
                ht.Add("@PJOB_DESC", "%" + PJOB_DESC + "%");
            }
            if (WS_CD != "-1" && WS_CD != "")
            {
                sb.Append(" and WS_CD = @WS_CD");
                ht.Add("@WS_CD", WS_CD);
            }
            if (LEVEL_CD != "-1" && LEVEL_CD != "")
            {
                sb.Append(" and LEVEL_CD = @LEVEL_CD");
                ht.Add("@LEVEL_CD", LEVEL_CD);
            }
            if (START_DT != "")
            {
                sb.Append(" and @START_DT >= START_DT and @START_DT <= END_DT");
                ht.Add("@START_DT", START_DT);
            }
            if (START_DT == "")
            {
                sb.Append(" and GETDATE() >= START_DT and GETDATE() <= END_DT");
            }

            sb.Append(" order by " + sortExpression);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getLevelCD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select LEVEL_CD from TB_H_M_LEVEL where END_DT='9999/12/31' order by ORDER_SEQ ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


}