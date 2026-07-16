using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// WFB2SJ0540 的摘要描述
/// </summary>
public class CFB2SJ0540DAO : BaseDAO
{
    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string DEPT_NO { get; set; }
    public string EMP_ID { get; set; }
    public string LEVEL_CD { get; set; }
    public string SCORE_FINAL { get; set; }

    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SJ0540DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //取得協理部門資訊
    public DataTable getDept20Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"
                                SELECT A.DEPT_NO, A.DEPT_NAME, A.HEAD_EMP_ID,A.HEAD_EMP_ID_2, B.EMP_NAME HEAD_EMP_NAME_2 
                                FROM VW_S_M_TOP_SEC_HEAD A LEFT JOIN 
                                     TB_H_M_EMP B ON A.HEAD_EMP_ID_2=B.EMP_ID ORDER BY DEPT_NO
                            ");

            DataTable dt = dbConn.Query(sb, ht); 

            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    
}