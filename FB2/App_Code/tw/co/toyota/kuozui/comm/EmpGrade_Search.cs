using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// EmpGrade_Search 的摘要描述
/// </summary>
public class EmpGrade_Search : BaseDAO
{
    public string GRADE_CD { get; set; }
    public string GRADE_DESC { get; set; }
    public string IS_VALID { get; set; }
    public string LEVEL_CD { get; set; }
    public string EMP_ID { get; set; }

    public EmpGrade_Search()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getGradeCdData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select distinct GRADE_CD, '' GRADE_DESC from TB_H_M_LEVEL_GRADE where GRADE_CD is not null and GRADE_CD <> '' ");

            if (GRADE_CD != "")
            {
                sb.Append(" and GRADE_CD = @GRADE_CD");
                ht.Add("@GRADE_CD", GRADE_CD);
            }
            //if (GRADE_DESC != "")
            //{
            //    sb.Append(" and MAIN_LEAVE_DESC like @MAIN_LEAVE_DESC");
            //    ht.Add("@MAIN_LEAVE_DESC", "%" + MAIN_LEAVE_DESC + "%");
            //}
            if (IS_VALID != "")
            {
                sb.Append(" and IS_VALID = @IS_VALID ");
                ht.Add("@IS_VALID", IS_VALID);
            }
            if (LEVEL_CD != "")
            {
                sb.Append(" and LEVEL_CD = @LEVEL_CD");
                ht.Add("@LEVEL_CD", LEVEL_CD);
            }
            else if (EMP_ID != "")
            {
                sb.Append(@" and LEVEL_CD = (select LEVEL_CD
                                             from VW_H_EMP_DATA E 
                                             where E.EMP_ID = @EMP_ID)");
                ht.Add("@EMP_ID", EMP_ID);
            }
            else { 

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