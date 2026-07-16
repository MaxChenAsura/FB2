using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

/// <summary>
/// CFB2SC2500BO 的摘要描述
/// </summary>
public class CFB2SC2500BO : BaseService
{
    public CFB2SC2500BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string execute(CFB2SC2500DAO dao)
    {
        try
        {
            string msg = "";
            if (dao.CFN_CNT == "0")
                msg += "該月薪資 確認筆數 =0,無法執行關帳作業!!\\n";
            if (dao.PROCESS_STATUS != "2" && dao.PAY_ID != "")
                msg += "該次薪資狀態已關帳,無法執行關帳作業!!\\n";
            if (dao.PROCESS_STATUS != "2" && dao.PAY_ID == "" && Convert.ToInt32(dao.NOT_CFN_CNT) > 0)
                msg += "該次薪資狀態未有發薪筆數>0 ,無法執行關帳作業!!\\n";

            if (dao.getTB_S_S_SALARY_PAY_TMP() > 0)
            {
                msg += "該資料主管尚有未簽核資料,無法執行關帳作業!!";
            }

            if (msg.Trim().Length == 0)
            {
                dao.RunSP_S_SALARY_ABNORMAL_EXEC();
                // string SPmsg = "";
                DataTable dtSPresult = dao.checkSP("SP_S_SALARY_ABNORMAL_EXEC");
                if (dtSPresult.Rows.Count > 0)
                {
                    //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                    if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) == "Y")
                        msg = "0";
                    else
                        msg = Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]);
                }
                if (msg == "0")
                {
                    //查詢 薪資計算異常資料檔(TB_S_M_SALARY_ERROR_RPT) 若資料存在 ,則回傳訊息
                    if (dao.getTB_S_M_SALARY_ERROR_RPT() > 0)
                    {
                        msg = "該月薪資計算後有重度異常現象發生,無法執行關帳作業!!請確認薪資解析異常資料!!";
                    }
                    else
                    {
                        //取得關帳代號
                        dao.PAY_ID = getPay_ID(dao);
                        BeginTransaction();
                        //更新 薪資明細計算檔(TB_S_S_SALARY_PAY)
                        dao.updateTB_S_S_SALARY_PAY(DateTime.Now.ToString("yyyy/MM/dd"), dao.PAY_ID);
                        //新增 薪資關帳主檔(TB_S_M_SALARY_PAY_H)
                        dao.addTB_S_M_SALARY_PAY_H();
                        //更新 參數檔(TB_9_M_PARAMETER).薪資關帳代號流水號
                        dao.updateTB_9_M_PARAMETER_CODE_VAL1();
                        if (dao.PROCESS_STATUS == "2")
                        {
                            //若資料列.薪資狀態='2'(薪資計算),更新 薪資計算主檔(TB_S_M_SALARY_CAL_H)
                            dao.updateTB_S_M_SALARY_CAL_H("3");
                        }
                        //terry add
                        if (dao.SALARY_TYPE == "A")
                        {
                            // 刪除 TB_S_M_SALARY_REPORT_D
                            dao.deleteTB_S_M_SALARY_REPORT_D();
                        }
                        else
                        {
                            //刪除 TB_S_M_SALARY_REPORT_O_D
                            dao.deleteTB_S_M_SALARY_REPORT_O_D();
                        }
                        Commit();
                    }
                }
            }
            return msg;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //取得關帳代號
    public string getPay_ID(CFB2SC2500DAO dao)
    {
        try
        {
            DateTime currentDate = DateTime.Now;
            int wk_lastTwoNum = 0;
            string wk_pay_id = string.Empty;
            DataTable dtWK = dao.getPAY_ID();
            if (dtWK.Rows.Count > 0)
            {
                wk_pay_id = Convert.ToString(dtWK.Rows[0]["CODE_VAL1"]);
                if (wk_pay_id.Substring(0, 8) != currentDate.ToString("yyyyMMdd"))
                    wk_pay_id = currentDate.ToString("yyyyMMdd") + "-01";
                else
                {
                    wk_lastTwoNum = Convert.ToInt32(wk_pay_id.Substring(wk_pay_id.Length - 2, 2)) + 1;
                    wk_pay_id = currentDate.ToString("yyyyMMdd") + "-" + wk_lastTwoNum.ToString().PadLeft(2, '0');
                }
            }
            return wk_pay_id;
        }
        catch
        {
            throw;
        }
    }
    public string execute2(CFB2SC2500DAO dao)
    {
        try
        {
            string msg = "";
            if ((dao.PROCESS_STATUS != "3" && dao.PROCESS_STATUS != "4") || dao.PAY_ID == "")
                msg += "該次薪資狀態不為關帳,無法執行取消關帳作業!!\\n";

            if (msg.Trim().Length == 0)
            {
                BeginTransaction();
                //更新 薪資明細計算檔(TB_S_S_SALARY_PAY)
                //string stringNull = string.Empty;
                dao.updateTB_S_S_SALARY_PAY_byCancel();
                //20151014 Terry 擔當有時並不會再重按薪資計算，如果刪掉將會造成資料流失，以致下次再重複扣款
                //刪除 員工欠薪還款暫存檔(TB_S_M_STAFF_REPAY_TMP)
               // dao.deleteTB_S_M_STAFF_REPAY_TMP();
                
                //讀取 薪資關帳主檔(TB_S_M_SALARY_PAY_H) 關帳代號<>資料列.關帳代號 資料,若資料不存在,則執行(B).
                if (dao.getTB_S_M_SALARY_PAY_H() == 0)
                {
                    //更新 薪資計算主檔(TB_S_M_SALARY_CAL_H)
                    dao.updateTB_S_M_SALARY_CAL_H("2");
                }
                //刪除  薪資關帳主檔(TB_S_M_SALARY_PAY_H)
                dao.deleteTB_S_M_SALARY_PAY_H();
                Commit();
                msg = "0";
            }
            return msg;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
}