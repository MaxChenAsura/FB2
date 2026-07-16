using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SJ0300BO 的摘要描述
/// </summary>
public class CFB2SJ0300BO : BaseService
{
	public CFB2SJ0300BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    //一括異常註記-(Dtl)
    public string mark(List<Tuple<string, string, string>> keysListMark, List<Tuple<string, string, string>> keysList, CFB2SJ0300DAO sj030DAO)
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
                    //考核資料維護檔,備註說明 
                    sj030DAO.updateMarkData_H(now);

                    //先清空該頁的異常註記
                    foreach (var item in keysList)
                    {
                        sj030DAO = new CFB2SJ0300DAO();
                        sj030DAO.ASSESS_YEAR = item.Item1;
                        sj030DAO.ASSESS_TYPE = item.Item2;
                        sj030DAO.EMP_ID = item.Item3;

                        //更新 考核人事資料維護檔 的異常註記為空白
                        sj030DAO.updateMarkData_D(now,"");

                    }

                    foreach (var item in keysListMark)
                    {
                        sj030DAO = new CFB2SJ0300DAO();
                        sj030DAO.ASSESS_YEAR = item.Item1;
                        sj030DAO.ASSESS_TYPE = item.Item2;
                        sj030DAO.EMP_ID = item.Item3;

                        //更新 考核人事資料維護檔 的異常註記為V
                        sj030DAO.updateMarkData_D(now,"V");

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


    //駁回-(Dtl)
    public string reject(CFB2SJ0300DAO sj030DAO)
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
                    //考核資料維護檔,回復成未核可前狀態 
                    sj030DAO.updateRejectData_H(now);

                    // 因改為分頁 且  增加一括異常註記後，不需要了
                    //更新 考核人事資料維護檔,將異動註記皆變為空白
                    /*
                    sj030DAO.updateAllRejectData_D(now);

                    foreach (var item in keysList)
                    {
                        sj030DAO = new CFB2SJ0300DAO();
                        sj030DAO.ASSESS_YEAR = item.Item1;
                        sj030DAO.ASSESS_TYPE = item.Item2;
                        sj030DAO.EMP_ID = item.Item3;

                        //更新 考核人事資料維護檔 的異常註記為V
                        sj030DAO.updateRejectData_D(now);

                    }
                    */

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



    //核可-(Dtl)
    public string approve(CFB2SJ0300DAO sj030DAO)
    {
        try
        {

            string rtnmessage = "";//存在檢查後的訊息
            DataTable dt = new DataTable();

            int result = sj030DAO.getMarkData();
            if (result > 0)
            {
                rtnmessage += "請取消異常註記 \\n";

            }


            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    BeginTransaction();
                    //更新  考核資料維護檔
                    sj030DAO.updateApproveData_H(now);

                    //更新 考核人事資料維護檔
                    sj030DAO.updateAllApproveData_D(now);

                    //刪除 考核人事資料主檔
                    sj030DAO.deleteApproveData_D_H(now);

                    //新増 考核人事資料主檔
                    sj030DAO.insertApproveData_D_H(now);

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