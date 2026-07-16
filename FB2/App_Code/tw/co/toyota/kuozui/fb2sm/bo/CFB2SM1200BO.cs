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
public class CFB2SM1200BO : BaseService
{
    public CFB2SM1200BO()
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
    #endregion

    #region Dtl
    public string updateConfirmData(List<string> confirmList,string qdatakey,string remark)
    {
        try
        {
            BeginTransaction();
            CFB2SM1200DAO dao = new CFB2SM1200DAO();
            dao.updateMasterConfirmData(qdatakey, remark);                //核可更新主檔
            foreach (string confirmListItem in confirmList)
            {
                dao.updateConfirmData(confirmListItem, qdatakey, remark);  //核可更新明細
            }
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string updateReject(List<string> remainList,List<string> rejectList, string qdatakey, string remark)
    {
        try
        {
            CFB2SM1200DAO dao = new CFB2SM1200DAO();
            BeginTransaction();
            dao.updateMasterRejectData(qdatakey, remark);//駁回更新主檔
            foreach (string remainListItem in remainList)
            {
                dao.updateRemainData(remainListItem, qdatakey, remark); //沒有勾選 異動狀態"N"
            }
            if (rejectList.Count > 0)
            {
                foreach (string rejectListItem in rejectList)
                {
                    dao.updateRejectData(rejectListItem, qdatakey, remark);  //駁回有打勾的明細檔更新 異動狀態"Y"
                }
            }
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