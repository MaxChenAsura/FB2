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
/// CFB2SC2400BO 的摘要描述
/// </summary>
public class CFB2SC2400BO : BaseService
{
    public CFB2SC2400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string actionSendData(string approve_OR_reject, DataTable dtApprove)
    {
        try
        {
            CFB2SC2400DAO dao = new CFB2SC2400DAO();
            BeginTransaction();
            for (int i = 0; i < dtApprove.Rows.Count; i++)
            {
                dao.CHG_STATUS = Convert.ToString(dtApprove.Rows[i]["CHG_STATUS"]);
                dao.DATA_YM = Convert.ToString(dtApprove.Rows[i]["DATA_YM"]);
                dao.SALARY_DT = Convert.ToString(dtApprove.Rows[i]["SALARY_DT"]);
                dao.SALARY_TYPE = Convert.ToString(dtApprove.Rows[i]["SALARY_TYPE"]);
                dao.EMP_ID = Convert.ToString(dtApprove.Rows[i]["EMP_ID"]);
                dao.SALARY_ID = Convert.ToString(dtApprove.Rows[i]["SALARY_ID"]);
                dao.SALARY_NAME = Convert.ToString(dtApprove.Rows[i]["SALARY_NAME"]);
                dao.IS_PLUS = Convert.ToString(dtApprove.Rows[i]["IS_PLUS"]);
                dao.IS_TAX = Convert.ToString(dtApprove.Rows[i]["IS_TAX"]);
                dao.TAX_FORMAT = Convert.ToString(dtApprove.Rows[i]["TAX_FORMAT"]);
                if (dao.SALARY_ID=="4001") //代扣所得稅
                {
                    dao.TAX_FORMAT = "50";
                }
                dao.PAY_KIND = Convert.ToString(dtApprove.Rows[i]["PAY_KIND"]);
                dao.CHG_AMT_A = Convert.ToString(dtApprove.Rows[i]["CHG_AMT_A"]);
                dao.REMARK = Convert.ToString(dtApprove.Rows[i]["REMARK"]);
                dao.APP_REMARK = Convert.ToString(dtApprove.Rows[i]["APP_REMARK"]);
                dao.SEQ_NO = Convert.ToString(dtApprove.Rows[i]["SEQ_NO"]);
                dao.PAY_TYPE = Convert.ToString(dtApprove.Rows[i]["PAY_TYPE"]);

                if (approve_OR_reject == "approve")
                    aprroveFlow(dao);
                else if (approve_OR_reject == "reject")
                    rejectFlow(dao);

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
    public void aprroveFlow(CFB2SC2400DAO dao)
    {
        try
        {
            double old_amount = 0;
            //異動狀態='N'(新增)或'A'(轉積欠代墊)或'B'(離職轉所得) 
            if (dao.CHG_STATUS == "N" || dao.CHG_STATUS == "A" || dao.CHG_STATUS == "B")
            {
                DataTable dtSalary_Item =  dao.getSALARY_ITEM();
                if (dtSalary_Item.Rows.Count > 0)
                {
                    dao.SALARY_NAME = Convert.ToString(dtSalary_Item.Rows[0]["SALARY_NAME"]);
                    dao.IS_PLUS = Convert.ToString(dtSalary_Item.Rows[0]["IS_PLUS"]);
                    dao.IS_TAX = Convert.ToString(dtSalary_Item.Rows[0]["IS_TAX"]);
                    dao.TAX_FORMAT = Convert.ToString(dtSalary_Item.Rows[0]["TAX_FORMAT"]);
                }
                else
                {
                    dao.SALARY_NAME = "";
                    dao.IS_PLUS = "";
                    dao.IS_TAX = "";
                    dao.TAX_FORMAT = "";
                }

                if (dao.SALARY_ID == "4001") //代扣所得稅
                {
                    dao.TAX_FORMAT = "50";
                }

                if (dao.checkIsRepeat_InTB_S_S_SALARY_PAY(ref old_amount))
                {
                    dao.CHG_AMT_A = (Convert.ToDouble(dao.CHG_AMT_A) + old_amount).ToString();
                    dao.ApproveN_Update_SALARY_PAY();
                }
                else
                    dao.ApproveN_Add_SALARY_PAY();
            }
            else if (dao.CHG_STATUS == "U") //異動狀態(CHG_STATUS)='U'(修改)
            {
                dao.ApproveU_Update_SALARY_PAY();
            }
            else if (dao.CHG_STATUS == "D") //異動狀態(CHG_STATUS)='D'(刪除)
            {
                string process_status = dao.getPROCESS_STATUS(dao.PAY_KIND);
                if (process_status == "2")
                {
                    dao.ApproveD_Delete_SALARY_PAY();
                }
                else if (process_status == "3" || process_status == "4")
                {
                    dao.ApproveD_Update_SALARY_PAY();
                }
            }
            else if (dao.CHG_STATUS == "C") //異動狀態(CHG_STATUS)='C'(暫不發薪)
            {
                dao.ApproveCR_Update_SALARY_PAY("N");
            }
            else if (dao.CHG_STATUS == "R") //異動狀態(CHG_STATUS)='R'(確認發薪)
            {
                dao.ApproveCR_Update_SALARY_PAY("Y");
            }
            dao.Update_SALARY_PAY_TMP("Y");
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void rejectFlow(CFB2SC2400DAO dao)
    {
        try
        {
            dao.Update_SALARY_PAY_TMP("B");
        }
        catch (Exception)
        {
            throw;
        }
    }

}