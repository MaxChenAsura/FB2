using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient; 
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CommCode_Search 的摘要描述
/// </summary>
public class CommCode_Search : BaseDAO
{
    public string SYS_CD { get; set; }
    public string MAIN_CD { get; set; }
    public string SUB_CD { get; set; }
    public string SUB_DESC { get; set; }
    public string CODE_VAL1 { get; set; }

    public CommCode_Search()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public System.Data.DataTable getCommCodeData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select SUB_CD,SUB_DESC from TB_9_M_COMM_D where SYS_CD is not null and IS_VALID='Y' ");

            if (SYS_CD != "")
            {
                sb.Append(" and SYS_CD = @SYS_CD");
                ht.Add("@SYS_CD", SYS_CD);
            }
            if (MAIN_CD != "")
            {
                sb.Append(" and MAIN_CD = @MAIN_CD");
                ht.Add("@MAIN_CD", MAIN_CD);
            }
            if (SUB_CD != "")
            {
                sb.Append(" and SUB_CD like @SUB_CD");
                ht.Add("@SUB_CD", "%" + SUB_CD + "%");
            }
            if (SUB_DESC != "")
            {
                sb.Append(" and SUB_DESC like @SUB_DESC");
                ht.Add("@SUB_DESC", "%" + SUB_DESC + "%");
            }
            if (CODE_VAL1 != "")
            {
                sb.Append(" and CODE_VAL1 = @CODE_VAL1");
                ht.Add("@CODE_VAL1", CODE_VAL1);
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