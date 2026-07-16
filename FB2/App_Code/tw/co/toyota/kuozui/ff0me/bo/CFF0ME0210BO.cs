using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
//using FB2.tw.co.toyota.kuozui.bo;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;

/// <summary>
/// CFF0ME0210Service 的摘要描述
/// </summary>
public class CFF0ME0210BO : BaseService
{
    public CFF0ME0210BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string exec_SP_D2C_TRANS(CFF0ME0210DAO ME020DAO)
    {
        string rtnmessage = "";//處理結果說明 Y:失敗,空白:成功

        try
        {
            rtnmessage = ME020DAO.exec_SP_D2C_TRANS();
            //取得回傳訊息
            if (rtnmessage != "")
            {
                return rtnmessage.Replace("\r\n", "").Replace("'", "\"");
            }
            return "0";
        }
        catch (Exception ex)
        {
            //RollBack();
            //return ex.Message;
            throw;
        }

    }
    
}