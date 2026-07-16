using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// Region_Search 的摘要描述
/// </summary>
public class Region_Search : BaseDAO 
{
    public string ZIP_CD { get; set; }
    public string COUNTY { get; set; }
    public string REGION { get; set; }


	public Region_Search()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getRegionData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select ZIP_CD,COUNTY,REGION from TB_H_M_ADMINISTRATION_REGION where ZIP_CD is not null ");

            if (ZIP_CD != "")
            {
                sb.Append(" and ZIP_CD like @ZIP_CD");
                ht.Add("@ZIP_CD", "%" + ZIP_CD + "%");
            }
            if (COUNTY != "")
            {
                sb.Append(" and COUNTY like @COUNTY");
                ht.Add("@COUNTY",  "%" + COUNTY + "%");
            }
            if (REGION != "")
            {
                sb.Append(" and REGION like @REGION");
                ht.Add("@REGION", "%" + REGION + "%");
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