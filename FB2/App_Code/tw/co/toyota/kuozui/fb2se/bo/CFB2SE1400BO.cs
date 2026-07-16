using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SE1400BO 的摘要描述
/// </summary>
public class CFB2SE1400BO : BaseService
{
    public CFB2SE1400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string approve(CFB2SE1400DAO fb2se, string qdatakey)
    {
        string msg = string.Empty;
        try
        {
            string rtnmessage = "";//存在檢查後的訊息
            DataTable dt = new DataTable();

            int result = fb2se.getMarkData();
            if (result > 0)
            {
                rtnmessage += "請取消【個人薪調】異常註記 \\n";

            }

            int result2 = fb2se.getMarkData2();
            if (result > 0)
            {
                rtnmessage += "請取消【3A以下調薪設定值】異常註記 \\n";

            }

            int result3 = fb2se.getMarkData3();
            if (result > 0)
            {
                rtnmessage += "請取消【2B以上調薪設定值】異常註記 \\n";

            }


            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                BeginTransaction();

                fb2se.Approve();
                Commit();
                rtnmessage = "0";
            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            RollBack();
            msg = ex.Message;
        }
        return msg;
    }

    public string reject(CFB2SE1400DAO fb2se, List<string> detailList1, List<string> detailList2, List<string> detailList3)
    {
        string msg = string.Empty;
        try
        {
            BeginTransaction();
            fb2se.rejectData_1();

            //依據異動狀態打勾的 更新 個人別調薪資料明細(TB_S_M_SALARY_ADJ_D)
            foreach (string deleteitem in detailList1)
            {
                //刪除主檔資料
                fb2se.rejectData_2(deleteitem);
            }

            fb2se.rejectData_3();

            //[考核調薪金額入力]頁籤,異動註記有打勾的資料 TB_S_M_SALARYSET_D(3A以下調薪金額明細檔)
            foreach (string deleteitem2 in detailList2)
            {
                //刪除主檔資料
                fb2se.rejectData_4(deleteitem2);
            }

            fb2se.rejectData_5();
            //更新 2B以上考核本薪設定明細檔(TB_S_M_2BSALARY_SET_D)
            foreach (string deleteitem3 in detailList3)
            {
                //刪除主檔資料
                fb2se.rejectData_6(deleteitem3);

            }

            Commit();
            msg = "0";

        }
        catch (Exception ex)
        {
            RollBack();
            msg = ex.Message;
        }
        return msg;
    }

     //一括異常註記-(Dtl)
    public string mark(List<string> keysListMark, List<string> keysList,
                       List<Tuple<string, string>> keysListMark2, List<Tuple<string, string>> keysList2,
                       List<string> keysListMark3, List<string> keysList3, CFB2SE1400DAO se140DAO)
    {
        DataTable dt = new DataTable();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    BeginTransaction();
                    //個人別調薪主檔(TB_S_M_SALARY_ADJ_H) ,備註說明 
                    se140DAO.updateSALARY_ADJ_H(now);

                    #region grig 1"
                    //先清空該頁的異常註記
                    foreach (var item in keysList)
                    {
                        //se140DAO = new CFB2SE1400DAO();
                        se140DAO.EMP_ID = item;
                        //更新 個人別調薪明細檔(TB_S_M_SALARY_ADJ_D)  的異常註記為''
                        se140DAO.updateTB_S_M_SALARY_ADJ_D(now, "");

                    }
                    foreach (var item in keysListMark)
                    {
                        //se140DAO = new CFB2SE1400DAO();
                        se140DAO.EMP_ID = item;
                        //更新 個人別調薪明細檔(TB_S_M_SALARY_ADJ_D) 的異常註記為Y
                        se140DAO.updateTB_S_M_SALARY_ADJ_D(now, "Y");
                    }
                    #endregion

                    #region grig 2"
                    //先清空該頁的異常註記
                    foreach (var item in keysList2)
                    {
                        //se140DAO = new CFB2SE1400DAO();
                        se140DAO.LEVEL_CD = item.Item1;
                        se140DAO.GRADE_CD = item.Item2;
                        //更新 考核調薪金額設定明細檔(TB_S_M_SALARYSET_D) 的異常註記為''
                        se140DAO.updateTB_S_M_SALARYSET_D(now, "");

                    }
                    foreach (var item in keysListMark2)
                    {
                        //se140DAO = new CFB2SE1400DAO();
                        se140DAO.LEVEL_CD = item.Item1;
                        se140DAO.GRADE_CD = item.Item2;
                        //更新 個人別調薪明細檔(TB_S_M_SALARY_ADJ_D) 的異常註記為Y
                        se140DAO.updateTB_S_M_SALARYSET_D(now, "Y");
                    }
                    #endregion

                    #region grig 3"
                    //先清空該頁的異常註記
                    foreach (var item in keysList3)
                    {
                        //se140DAO = new CFB2SE1400DAO();
                        se140DAO.LEVEL_CD = item;
                        //更新 2B以上考核本薪設定明細檔(TB_S_M_2BSALARY_SET_D)  的異常註記為''
                        se140DAO.updateTB_S_M_2BSALARY_SET_D(now, "");

                    }
                    foreach (var item in keysListMark3)
                    {
                       // se140DAO = new CFB2SE1400DAO();
                        se140DAO.LEVEL_CD = item;
                        //更新 2B以上考核本薪設定明細檔(TB_S_M_2BSALARY_SET_H) 的異常註記為Y
                        se140DAO.updateTB_S_M_2BSALARY_SET_D(now, "Y");
                    }
                    #endregion
                    Commit();


                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
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
}