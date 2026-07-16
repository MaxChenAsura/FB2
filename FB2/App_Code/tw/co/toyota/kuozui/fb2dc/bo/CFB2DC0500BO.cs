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
/// CFB2DC0500BO 的摘要描述
/// </summary>
public class CFB2DC0500BO : BaseService
{
    public CFB2DC0500BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //刪除 TB_D_M_TEMP_CARD_RECORD
    public string deleteCARD_RECORD(List<Tuple<string, string>> card_no)
    {
        try
        {
            CFB2DC0500DAO wfb2dc = new CFB2DC0500DAO();
            BeginTransaction();
            foreach (var item in card_no)
            {
                wfb2dc.deleteCARD_RECORD(item.Item1, item.Item2);
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

    //取得員工照片資料
    public DataTable getPHOTOData(string emp_id)
    {
        CFB2DC0500DAO wfb2dc = new CFB2DC0500DAO();
        try
        {
            return wfb2dc.getPHOTOData(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //借用
    public string addTEMP_CARD_RECORD(CFB2DC0500DAO wfb2dc)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2dc.getExistData();

            if (tmp.Rows.Count > 0)
            {
                return "該臨時卡的被借期間有重覆";
            }
            else
            {
                BeginTransaction();

                wfb2dc.addTEMP_CARD_RECORD();
                Commit();

                wfb2dc.cardHandleCD = wfb2dc.getCardHandle();
                //如果 明細畫面.借用原因 != 1(未帶卡) 且 明細畫面.重新製卡 =  是 才需要執行
                if (wfb2dc.BORROW_REASON_CD != "1" && wfb2dc.IS_RE_MAKE == "Y")
                {
                    wfb2dc.SP_D_UPD_CARD_DATA_RE();
                }

            }
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public DataTable getiniData(string card_no, string start_dt, string borrow_type)
    {
        try
        {
            CFB2DC0500DAO wfb2dc = new CFB2DC0500DAO();
            wfb2dc.CARD_NO = card_no;
            wfb2dc.START_DT = start_dt;
            wfb2dc.BORROW_TYPE = borrow_type;
            return wfb2dc.getiniData();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //修改
    public string updateTEMP_CARD_RECORD(CFB2DC0500DAO wfb2dc)
    {
        try
        {
            //0.檢查PK值有無重覆,當START_DT_PK = START_DT時, 不檢查
            string rtnmessage = "";
            if (wfb2dc.START_DT != wfb2dc.START_DT_PK)
            {
                DataTable dt = wfb2dc.getPKData();
                if ((int)dt.Rows[0]["resultCount"] > 0)
                {
                    rtnmessage += "借用卡及+生效日期 重覆 \\n";
                }
            }
            //1.應該要判斷期間是否有重疊

            if (rtnmessage != "")
            {
                return rtnmessage;
            }




            BeginTransaction();
            //  20150615 實際還卡時間(RETURN_DT),此功能主要是讓擔當變更借用結束日期,故 實際還卡時間(RETURN_DT)不能 修改
            //  20150615 當實際還卡時間(RETURN_DT) 為 null時,才能修改
            wfb2dc.updateTEMP_CARD_RECORD();
            //2015 0615 更改 借用迄日(實際) =  MIN(RETURN_DT(實際還卡時間),END_DT(借用迄日))
            wfb2dc.updateTEMP_CARD_RECORD_REAL();

            Commit();

            //如果 明細畫面.卡片狀態 = Y.已還,才須執行
            if (wfb2dc.BORROW_STATUS == "Y")
            {
                wfb2dc.SP_D_UPD_CARD_DATA1();

                //20150612  1.reopen 開始日期+1天 ~ 結束日期 的 勤務比對結果為N  
                DateTime StartDate = Convert.ToDateTime(wfb2dc.reopen_START_DT).AddDays(1);
                DateTime EndDate = Convert.ToDateTime(wfb2dc.reopen_END_DT);
                bool isSalaryDate = false;
                BeginTransaction();
                for (DateTime PocessDate = StartDate; PocessDate < EndDate.AddDays(1); PocessDate = PocessDate.AddDays(1))
                {
                    isSalaryDate = utilities.isSalaryDate(PocessDate.ToString("yyyy/MM/dd"));
                    if (isSalaryDate == false)
                    {
                        wfb2dc.SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN(PocessDate);
                        
                    }
                }
                //20150615 2.將 勤務刷卡明細暫存檔 人事系統更新日期時間 更改為null, 以利進行有關臨時卡的 勤務比對
                //卡鐘時間期間為 借卡起日+1 至 借用迄日(實際) 
                //wfb2dc.updateCLOCK_RECORD_TEM();  //20150911 註解
                Commit();

            }
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //歸還
    public string updateTEMP_CARD_RECORD2(CFB2DC0500DAO wfb2dc)
    {
        try
        {
            BeginTransaction();

            wfb2dc.updateTEMP_CARD_RECORD2();
            Commit();

            //如果 明細畫面.卡片狀態 = Y.已還,才須執行
            if (wfb2dc.BORROW_STATUS == "Y")
            {
                wfb2dc.SP_D_UPD_CARD_DATA1();
            }
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //取得介面查詢必要的資料(歸還)
    public DataTable getCARD_NO(string card_no)
    {
        try
        {
            CFB2DC0500DAO wfb2dc = new CFB2DC0500DAO();
            wfb2dc.CARD_NO = card_no;
            return wfb2dc.getCARD_NO();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得 部門/廠商別
    public DataTable getPERSON_DC(string borrow_type, string person_id)
    {
        try
        {
            CFB2DC0500DAO wfb2dc = new CFB2DC0500DAO();
            wfb2dc.BORROW_TYPE = borrow_type;
            wfb2dc.PERSON_ID = person_id;
            return wfb2dc.getPERSON_DC();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得臨時卡區分
    public DataTable getTEMP_CARD_CD(string syscodeatt)
    {
        try
        {
            CFB2DC0500DAO wfb2dc = new CFB2DC0500DAO();
            wfb2dc.SYSCODEATT = syscodeatt;
            return wfb2dc.getTEMP_CARD_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEmpName(string emp_id, string value)
    {
        try
        {
            CFB2DC0500DAO wfb2dc = new CFB2DC0500DAO();
            if (value == "1")
                return wfb2dc.getEmpName(emp_id);
            else
                return wfb2dc.getVENDOR_MEMBER_NAME(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得借用日期訖
    public string getBORROW_END_DT(string emp_id, DateTime stime)
    {
        try
        {
            string date = "";
            CFB2DC0500DAO wfb2dc = new CFB2DC0500DAO();
            //取得借用日期訖
            DataTable tmp = wfb2dc.getBORROW_END_DT(emp_id, stime);
            if (tmp.Rows.Count > 0)
            {
                DateTime dt = new DateTime();
                if (DateTime.TryParse(tmp.Rows[0]["BORROW_END_DT"].ToString(), out dt))
                    date = dt.ToString("yyyy/MM/dd");
            }
            return date;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //借用卡號(查詢)
    public DataTable getCARD_NAME(string card_no)
    {
        try
        {
            CFB2DC0500DAO wfb2dc = new CFB2DC0500DAO();
            return wfb2dc.getCARD_NAME(card_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //借用卡號(借用)
    public DataTable getCARD_NAME2(string card_no, string temp_card_cd)
    {
        try
        {
            CFB2DC0500DAO wfb2dc = new CFB2DC0500DAO();
            return wfb2dc.getCARD_NAME2(card_no, temp_card_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //借用卡號(歸還)
    public DataTable getCARD_NAME3(string card_no, string temp_card_cd)
    {
        try
        {
            CFB2DC0500DAO wfb2dc = new CFB2DC0500DAO();
            return wfb2dc.getCARD_NAME3(card_no, temp_card_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }
}