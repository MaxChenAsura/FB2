using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
/// <summary>
/// CFB2HE0300BO 的摘要描述
/// </summary>
public class CFB2HE0300BO : BaseService
{
    public CFB2HE0300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //駁回-(Dtl)
    public string reject(List<Tuple<string, string, string>> emp_ids, string approveRemark)
    {
        try
        {

            DataTable dt = new DataTable();
            string rtnmessage = "";//存在檢查後的訊息
            CFB2HE0300DAO he030DAO = new CFB2HE0300DAO();
            if (rtnmessage == "")
            {
                try
                {
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    BeginTransaction();
                    foreach (var item in emp_ids)
                    {
                        he030DAO = new CFB2HE0300DAO();
                        he030DAO.LICENSE_ID = item.Item1;
                        he030DAO.PJOB_CD = item.Item2;
                        he030DAO.APPLY_DT = item.Item3;

                        he030DAO.APPROVE_BY = SessionHandle.Current.emp_id;
                        he030DAO.APPROVE_REMARK = approveRemark;

                        he030DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        he030DAO.FUNC_ID = "FB2HE030";
                        he030DAO.reject(now);
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


    //核可
    public string approve(List<Tuple<string, string, string>> emp_ids, string approveRemark)
    {
        try
        {
            
            DataTable dt = new DataTable();
            string rtnmessage = "";//存在檢查後的訊息
            CFB2HE0300DAO he030DAO = new CFB2HE0300DAO();
            if (rtnmessage == "")
            {
                try
                {
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    BeginTransaction();
                     foreach (var item  in emp_ids)
                    {
                        he030DAO = new CFB2HE0300DAO();
                        he030DAO.LICENSE_ID = item.Item1;
                        he030DAO.PJOB_CD = item.Item2;
                        he030DAO.APPLY_DT = item.Item3;

                        he030DAO.APPROVE_BY = SessionHandle.Current.emp_id;
                        he030DAO.APPROVE_REMARK = approveRemark;

                        he030DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        he030DAO.FUNC_ID = "FB2HE030";
                        he030DAO.approve(now);
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

}