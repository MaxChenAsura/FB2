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
/// CFB2HD0100BO 的摘要描述
/// </summary>
public class COMMDAO : BaseDAO
{
    public COMMDAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getCOMPANY(string COMPANY_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COMPANY_CD,COMPANY_SNAME,HEALTH_ORG_ID,LABOR_ORG_ID From TB_H_M_COMPANY");
            sb.Append(" where COMPANY_CD=@COMPANY_CD");
            ht.Add("@COMPANY_CD", COMPANY_CD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

}