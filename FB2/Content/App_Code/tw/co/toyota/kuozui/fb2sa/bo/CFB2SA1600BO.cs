using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;

/// <summary>
/// CFB2SJ040BO 的摘要描述
/// </summary>
public class CFB2SA1600BO : BaseService
{
    public CFB2SA1600BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    #region 新增作業
    //取得薪資項目
    public DataTable getAllSALARY_ID()
    {
        CFB2SA1600DAO dao = new CFB2SA1600DAO();
        try
        {
            return dao.getAllSALARY_ID();
        }
        catch (Exception)
        {

            throw;
        }
    }

   

    //新增-儲存
    public string addSave(CFB2SA1600DAO sa160DAO)
    {
        try
        {
            //檢核
            string errMsg = "";
            errMsg = checkAdds(sa160DAO);
            if (errMsg != "") {
                return errMsg;   
            }
            BeginTransaction();
            //修改 職務類別敘薪檔
            sa160DAO.addSave();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }

    }
   

    //新增 檢核
    public string checkAdds(CFB2SA1600DAO sa160DAO)
    {
        try
        {
            string errMsg = "";
            int chkint = 0;
            //1.職務代碼是否存在
            chkint= sa160DAO.getPJOBCount();
            if (chkint == 0) {
                return "職務代碼不存在!";
            }
            //2-1.是否有重疊的資料
            chkint = sa160DAO.chekOver("1");
            if (chkint>0)
            {
                return "職務+類別+薪資項目生效期間重疊!";
            }
            //2-3.是否有重疊的資料
            chkint = sa160DAO.chekOver("2");
            if (chkint > 0)
            {
                return "職務+類別+薪資項目生效期間重疊!";
            }
            //3.PK值有無重覆
            chkint = sa160DAO.chekPK();
            if (chkint > 0)
            {
                return "職務+類別+薪資項目+生效期間已存在!";
            }


            return errMsg;


        }
        catch (Exception ex)
        {
            throw;
            //return ex.Message;
        }
    }
    #endregion

    #region 修改作業
    //取得修改資料
    public DataTable getUpdData(CFB2SA1600DAO dao)
    {
        try
        {
            return dao.getUpdData();
        }
        catch (Exception)
        {
            throw;
        }
    }

    //修改-儲存
    public string updSave(CFB2SA1600DAO sa160DAO)
    {
        try
        {
            //檢核
            string errMsg = "";
            errMsg = checkUpds(sa160DAO);
            if (errMsg != "")
            {
                return errMsg;
            }
            BeginTransaction();
            //新增 職務類別敘薪檔
            sa160DAO.updSave();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }


    //修改 檢核
    public string checkUpds(CFB2SA1600DAO sa160DAO)
    {
        try
        {
            string errMsg = "";
            int chkint = 0;
            //2-1.是否有重疊的資料
            chkint = sa160DAO.chekUpdOver();
            if (chkint > 0)
            {
                return "職務+類別+薪資項目生效期間重疊!";
            }
            return errMsg;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    //一括修改 檢核
    public string checkAllUpds(CFB2SA1600DAO sa160DAO)
    {
        try
        {
            string errMsg = "0";
            int chkint = 0;
            //1.挑選資料需生效中！
            chkint = sa160DAO.chekIsVaild();
            if (chkint == 0)
            {
                return "挑選資料需生效中!";
            }
            //2.無符合人員，無法處理！
            chkint = sa160DAO.chekEmp(sa160DAO.getHireType());
            if (chkint == 0)
            {
                return "挑選資料無符合在職人員!";
            }
            return errMsg;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    #endregion


    //刪除
    public string delSave(List<Tuple<string, string, string, string>> keysList)
    {
        CFB2SA1600DAO sa160DAO = new CFB2SA1600DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                BeginTransaction();
                foreach (var item in keysList)
                {
                    sa160DAO.PJOB_CD = item.Item1;
                    sa160DAO.SALARY_ID = item.Item2;
                    sa160DAO.HIRE_TYPE = item.Item3;
                    sa160DAO.START_DT = item.Item4;
                   
                    sa160DAO.delSave();
                }
                Commit();
                return "0";
            }
            else
            {
                return rtnmessage;
            }
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //一括對象生成
    public string execSP_S_SA160_GEN(CFB2SA1600DAO sa160DAO)
    {
        string rtnmessage = "";//檢查後的訊息
        try
        {
            sa160DAO.execSP_S_SA160_GEN();
            rtnmessage += utilities.getSPLOG("SP_S_SA160_GEN");
            if (rtnmessage != "")
            {
                return rtnmessage;
            }
            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //一括對象提出簽核
    public string execSP_S_SA160_SEND(CFB2SA1600DAO sa160DAO)
    {
        string rtnmessage = "";//檢查後的訊息
        try
        {
            sa160DAO.execSP_S_SA160_SEND();
            rtnmessage += utilities.getSPLOG("execSP_S_SA160_SEND");
            if (rtnmessage != "")
            {
                return rtnmessage;
            }
            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

}