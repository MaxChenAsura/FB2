using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2DH0700DAO 的摘要描述
/// </summary>
public class CFB2DL0400DAO : BaseDAO
{

    public string YEAR_S { get; set; }
    public string YEAR_E { get; set; }
    public string EMP_ID { get; set; }
    public string DEPT_NO { get; set; }
    public string WS_CD { get; set; }

    public CFB2DL0400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //EXCEL匯出
    public DataTable getExcelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" Select 
                         ROW_NUMBER() OVER(ORDER BY LEAVE_YEAR,EMP_ID ) As ROWNO
                        ,A.*
                        from TB_D_M_ANNUAL_LEAVE_REPORT  A  with (nolock)  
                        ");
           
            sb.Append(" where 1=1 ");

            //權限條件
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND a.EMP_ID IN(select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }          

            //查詢條件
            if (YEAR_S != "")
            {
                sb.Append(" and A.LEAVE_YEAR >= @YEAR_S ");
                ht.Add("@YEAR_S", YEAR_S);
            }
            if (YEAR_E != "")
            {
                sb.Append(" and A.LEAVE_YEAR <= @YEAR_E ");
                ht.Add("@YEAR_E", YEAR_E);
            }          
            if (EMP_ID != "")
            {
                sb.Append(" and A.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID + "%");
            }
           
            if (DEPT_NO != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", DEPT_NO + "%");
            }
            if (WS_CD != "-1")
            {
                sb.Append(" and A.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", WS_CD);
            }
            //sb.Append(" order by YM,EMP_ID ");
            
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
   
}