using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// Reduce_Search 的摘要描述
/// </summary>
public class Reduce_Search : BaseDAO
{
    public string REDUCE_CD { get; set; }
    public string REDUCE_DESC { get; set; }

    public Reduce_Search()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public System.Data.DataTable getReduceData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select REDUCE_CD,EFFECT_DT,REDUCE_DESC,LAB_RATE,HEA_RATE,GOV_AMOUNT from TB_I_M_REDUCE where EFFECT_DT <= GETDATE() and UNEFFECT_DT >= GETDATE() ");

            if (REDUCE_CD != "")
            {
                sb.Append(" and REDUCE_CD like @REDUCE_CD");
                ht.Add("@REDUCE_CD", "%" + REDUCE_CD + "%");
            }
            if (REDUCE_DESC != "")
            {
                sb.Append(" and REDUCE_DESC like @REDUCE_DESC");
                ht.Add("@REDUCE_DESC", "%" + REDUCE_DESC + "%");
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