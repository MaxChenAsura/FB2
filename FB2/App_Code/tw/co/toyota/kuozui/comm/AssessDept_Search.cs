using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient; 
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// AssessDept_Search 的摘要描述
/// </summary>
public class AssessDept_Search : BaseDAO
{
    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string DEPT_NO { get; set; }
    public string DEPT_NAME { get; set; }
    public string HEAD_EMP_ID { get; set; }
    public int DEPT_LEVEL { get; set; }
    public string DEPT_FULL_NAME { get; set; }


    public AssessDept_Search()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public System.Data.DataTable getAssessDeptData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select DEPT_NO, DEPT_NAME from TB_H_R_DEPT_DATA_AD where 1=1 and DEPT_LEVEL='20' ");

            if (DEPT_NO != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO");
                ht.Add("@DEPT_NO", "%" + DEPT_NO + "%");
            }
            if (DEPT_NAME != "")
            {
                sb.Append(" and DEPT_NAME like @DEPT_NAME");
                ht.Add("@DEPT_NAME", "%"+DEPT_NAME+ "%");
            }
            if (DEPT_LEVEL != 0)
            {
                sb.Append(" and DEPT_LEVEL= @DEPT_LEVEL");
                ht.Add("@DEPT_LEVEL", DEPT_LEVEL);
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