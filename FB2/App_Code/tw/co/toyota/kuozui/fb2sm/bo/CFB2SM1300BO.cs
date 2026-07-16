using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
/// <summary>
/// CFB2990100BO 的摘要描述
/// </summary>
public class CFB2SM1300BO : BaseService
{
    public CFB2SM1300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string DATA_YEAR { get; set; }
    public string DATA_SEQ { get; set; }
    public string NOTICE_DT { get; set; }
    public string NOTICE_BY { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string REMARK_DESC { get; set; }
    public string EXECUTIVE_DT { get; set; }

    # region Qry
    
    //Release更新主檔
    public string updateReleaseData(string qdatakey)
    {
        try
        {
            CFB2SM1300DAO dao = new CFB2SM1300DAO();
            BeginTransaction();
            dao.updateReleaseData(qdatakey);
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    
    #endregion





}