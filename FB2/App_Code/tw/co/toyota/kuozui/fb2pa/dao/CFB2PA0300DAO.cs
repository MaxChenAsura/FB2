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
/// WFB2PA0300 的摘要描述
/// </summary>
public class CFB2PA0300DAO : BaseDAO
{
    
    public string YM { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }

    public string FUNC_ID { get; set; }

    public CFB2PA0300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }




    //更新 提案資料檔
    public void updateProposalData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_P_M_PROPOSAL_DATA ");
            sb.Append(" set SALARY_YM=@SALARY_YM, ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(), FUNC_ID=@FUNC_ID ");
            sb.Append(" where ISNULL(SALARY_YM,'') = ''  AND IS_YN<>'Y' ");

            ht.Add("@SALARY_YM", YM.Replace("/", ""));
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //增新 提案關帳檔
    public void insertCloseYm()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT TB_P_M_CLOSE_YM(YM, CREATED_BY, CREATED_DT, FUNC_ID)VALUES( ");
            sb.Append(" @YM, @CREATED_BY, GETDATE(),@FUNC_ID )");

            ht.Add("@YM", YM.Replace("/", ""));
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    
   
}