using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// DOC_NO_Search 的摘要描述
/// </summary>
public class DOC_NO_Search : BaseDAO
{
    public string DOC_NO { get; set; }
    public string EMP_ID { get; set; }
    public string CREDITOR { get; set; }    

	public DOC_NO_Search()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select a.DOC_NO,a.SEQ,a.PAY_TARGET,a.PAY_TARGET+'-'+b.SUB_DESC AS PAY_TARGET_DESC,a.CREDITOR,a.AMOUNT,a.RATIO");
            sb.AppendLine("from TB_S_M_ARREARS_TARGET a");
            sb.AppendLine("left join TB_9_M_COMM_D b on b.sys_cd='SF' and b.MAIN_CD='PAY_TARGET' and b.IS_VALID='Y' and a.PAY_TARGET=b.sub_cd");
            sb.AppendLine("where a.IS_VAILD='Y'");

            if (DOC_NO != "")
            {
                sb.Append(" and a.DOC_NO like @DOC_NO");
                ht.Add("@DOC_NO", "%" + DOC_NO + "%");
            }
            if (CREDITOR != "")
            {
                sb.Append(" and a.CREDITOR like @CREDITOR");
                ht.Add("@CREDITOR", "%" + CREDITOR + "%");
            }
            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", EMP_ID);
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