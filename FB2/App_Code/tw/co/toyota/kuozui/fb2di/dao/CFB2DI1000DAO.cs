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
public class CFB2DI1000DAO : BaseDAO
{

    public string YM_S { get; set; }
    public string YM_E { get; set; }
    public string EMP_ID { get; set; }
    public string DEPT_NO { get; set; }
    public string WS_CD { get; set; }

    public CFB2DI1000DAO()
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
                         ROW_NUMBER() OVER(ORDER BY YM,EMP_ID ) As ROWNO
                        ,A.*
                        from TB_D_M_OVERTIME_REPORT  A  with (nolock)  
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
            if (YM_S != "")
            {
                sb.Append(" and A.YM >= @YM_S ");
                ht.Add("@YM_S", YM_S);
            }
            if (YM_E != "")
            {
                sb.Append(" and A.YM <= @YM_E ");
                ht.Add("@YM_E", YM_E);
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