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
/// CFB2SP0300DAO 的摘要描述
/// </summary>
public class CFB2SP0300DAO : BaseDAO
{

    //頁面資料
    public string RETIRE_YM { get; set; }
    public string REMARK { get; set; }

    //基本欄位
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    ACESLib.ACES aces;
    public CFB2SP0300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

  
    //EXCEL下載資料-考核人事資料主檔(SJ010)
    public DataTable getExcelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"Select ROW_NUMBER() OVER(ORDER BY b.ws_cd,b.emp_id ) As RowNumber
                        ,b.WS_CD,a.EMP_ID,b.EMP_NAME,a.AVG_PAY,a.OLDRETIRE_YEARS,a.RETIRE_BASE_MONTH																																																																												
                        ,Convert(int,(a.RETIRE_BASE_MONTH *a.AVG_PAY) )  as RETIRE_AMT,a.RETIRE_PAY,a.FREETAX_STAGE1,a.FREETAX_STAGE2,a.LEVY_TAX_AMT,a.RETIRE_TAX																																																																												
                        from TB_S_M_OLDRETIRE_H a																																																																												
                        left join VW_H_EMP_DATA b on a.emp_id=b.emp_id																																																																												
                        where  CONVERT(VARCHAR(7),RETIRE_DT,111) =@RETIRE_YM																																																																												
                        order by b.ws_cd,b.emp_id		
                      ");
            ht.Add("@RETIRE_YM", RETIRE_YM);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }



}