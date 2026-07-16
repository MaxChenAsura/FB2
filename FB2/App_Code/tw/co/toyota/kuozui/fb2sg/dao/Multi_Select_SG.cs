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
public class Multi_Select_SG : BaseDAO
{


    public string TableNmae { get; set; }
    public string TextColumn { get; set; }
    public string ValueColumn { get; set; }


    public Multi_Select_SG()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //

    }
    //被節金條件設定中，同一節金類別選的員工區分
    public DataTable getSelectedData(string[] containsPridCDArray)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select " + ValueColumn + "+'-'+" + TextColumn + " " + TextColumn + "," + ValueColumn + " from " + TableNmae);
            sb.Append(" where main_CD = 'EMP_CD' ");
            sb.Append(" and  SYS_CD = 'HB' ");
            sb.Append(" and SUB_CD  in  ( @containsPridCDArray ) ");
            ht.Add("@containsPridCDArray", containsPridCDArray);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }


    public DataTable getNonSelectedData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select " + ValueColumn + "+'-'+" + TextColumn + " " + TextColumn + "," + ValueColumn + " from " + TableNmae);
            sb.Append(" where main_CD = 'EMP_CD' ");
            sb.Append(" and  SYS_CD = 'HB' ");
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getNonSelectedData(string[] containsPridCDArray)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select " + ValueColumn + "+'-'+" + TextColumn + " " + TextColumn + "," + ValueColumn + " from " + TableNmae);
            sb.Append(" where main_CD = 'EMP_CD' ");
            sb.Append(" and  SYS_CD = 'HB' ");
            sb.Append(" and SUB_CD not in  ( @containsPridCDArray ) ");
            ht.Add("@containsPridCDArray", containsPridCDArray);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
}