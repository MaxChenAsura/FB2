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
/// Multi_Select 的摘要描述
/// </summary>
public class Multi_Select : BaseDAO
{


    public string TableNmae { get; set; }
    public string TextColumn { get; set; }
    public string ValueColumn { get; set; }
    public string WhereColumn { get; set; }
    public string WhereValue { get; set; }

    public Multi_Select()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //

    }

    public DataTable getSelectData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select " + ValueColumn + "+'-'+" + TextColumn + " " + TextColumn + "," + ValueColumn + " from " + TableNmae);
            sb.Append(" where 1=1 ");
            if (WhereColumn != "" && WhereValue != "")
            {
                sb.Append(" and " + WhereColumn + "=@WhereValue");
                ht.Add("@WhereValue", WhereValue);
            }

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSelectData(string fun_name, string FUNC_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            ht.Add("@FUNC_ID", FUNC_ID);
            switch (fun_name)
            {
                case "wfb2990400":

                    sb.Append(" select a.FUNC_ID, a.FUNC_ID+':'+a.FUNC_NAME as FUNC_NAME from (");
                    sb.Append(" 	select        FUNC_ID, FUNC_NAME");
                    sb.Append(" 	from            TB_S_R_LOG");
                    sb.Append(" 	union ");
                    sb.Append(" 	select        FUNC_ID, FUNC_NAME");
                    sb.Append(" 	from            TB_I_R_LOG");
                    sb.Append(" 	union ");
                    sb.Append(" 	select        FUNC_ID, FUNC_NAME");
                    sb.Append(" 	from            TB_D_R_LOG");
                    sb.Append(" 	union ");
                    sb.Append(" 	select        FUNC_ID, FUNC_NAME");
                    sb.Append(" 	from            TB_H_R_LOG");
                    sb.Append(" ) as a");
                    sb.Append(" left outer join TB_9_M_SYS_D as b on a.FUNC_ID = b.FUNCTION_ID");
                    sb.Append(" where isnull(b.FUNCTION_ID,'') = ''");

                    break;
                default:
                    break;
            }

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getAllFunc()
    {
        try
        {            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" SELECT  FUNCTION_ID FROM TB_9_M_SYS_D");                  

                      
            return dbConn.Query(sb, ht); ;

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getFuncData(string funcs)
    {
        try
        {                        
            dbConn.OtherCommStr = utilities.ACESconnstr;
            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" SELECT  SUBSTRING(SYS_FUNC_NAME,1,8) as FUNC_ID,SUBSTRING(SYS_FUNC_NAME,1,8)+':'+ SUBSTRING(SYS_FUNC_NAME,CHARINDEX(' ',SYS_FUNC_NAME,1)+1,DATALENGTH(SYS_FUNC_NAME) - CHARINDEX(' ',SYS_FUNC_NAME,1)) as FUNC_NAME ");
            sb.Append(" FROM TB_M_SYS_FUNC ");
            sb.Append(" where SYS_ITEM_CD = 'FB2' and SYS_TYPE = 'F' ");
            if (funcs.IndexOf(",")==-1 )
            {
                sb.Append(" and SUBSTRING(SYS_FUNC_NAME,1,8) <> (@funcs) ");
            }
            else
            {
                sb.Append(" and SUBSTRING(SYS_FUNC_NAME,1,8) not in (@funcs) ");
            }
            sb.Append(" order by FUNC_ID ");

            ht.Add("@funcs", funcs);

            DataTable dt1 = dbConn.Query(sb, ht);
            int tt = dt1.Rows.Count;
            dbConn.OtherCommStr = "";
            return dt1;

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getSelectData2(string fun_name, string FUNC_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            ht.Add("@FUNC_ID", FUNC_ID);
            switch (fun_name)
            {

                case "WFB2SB1100":

                    sb.Append(" select a.* from (");
                    sb.Append(" select DISTINCT a.SALARY_ID, a.SALARY_NAME,  a.SALARY_ID+'-'+a.SALARY_NAME as 'SALARY'");
                    sb.Append(" from            TB_S_M_SALARY_ITEM a");

                    sb.Append(" ) as a");


                    break;
                default:
                    break;
            }

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
}