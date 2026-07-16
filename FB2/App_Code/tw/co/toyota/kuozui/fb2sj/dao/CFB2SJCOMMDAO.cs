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
/// WFB2SJ0110 的摘要描述
/// </summary>
public class CFB2SJCOMMDAO : BaseDAO
{
    public string DEPT_NO { get; set; }

    public string USER_UP_YN { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SJCOMMDAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    
    //取得修改資料
    public DataTable getADDeptDataByDeptNo()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" Select 
                         A.*
                         from TB_H_R_DEPT_DATA_AD A ");
            
           
            sb.Append(@" 
                where 1=1
                and A.DEPT_NO=@DEPT_NO
            ");

            ht.Add("@DEPT_NO", DEPT_NO);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
   
}