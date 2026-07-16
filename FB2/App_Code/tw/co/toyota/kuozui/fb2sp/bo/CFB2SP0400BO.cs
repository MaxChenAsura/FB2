using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SP0400BO 的摘要描述
/// </summary>
public class CFB2SP0400BO : BaseService
{
    public CFB2SP0400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }




    public string execute(CFB2SP0400DAO dao)
    {
        try
        {
            string rtnmessage = "";
            DataTable dt = new DataTable();
            //邏輯檢查 
            //1.至勞保健保勞退履歷主檔(TB_I_M_3IN1_TXN)找尋此人是否為舊制或舊轉新制	
            dt = dao.get3IN1_TXN();
            if ((int)dt.Rows[0]["resultCount"] == 0)
            {
                rtnmessage += "此人非為舊制勞退人員,不允執行計算功能 \\n";
                //return rtnmessage;
            }
            //2.退休日必須大於TB_9_M_PARAMETER(參數檔) 系統上線日+6個月  2015/07/01 MARK 勿刪 BY EVA
            //dt = sp0100DAO.getOnLineDay();
            //if ((int)dt.Rows[0]["resultCount"] == 0)
            //{
            //    string on_LINE_DAY_ADDSIX_M = Convert.ToDateTime( dt.Rows[0]["ON_LINE_DAY_ADDSIX_M"].ToString()).ToString("yyyy/MM/dd");
            //    rtnmessage += "此功能必須退休日或計算日需大於" + on_LINE_DAY_ADDSIX_M + "才可使用!\\n";
            //    //return rtnmessage;
            //}
            //2-2  前六個月的薪資起算日 必須大於TB_9_M_PARAMETER(參數檔) 系統上線日
            //"前六個月的薪資起算日:"+v六月薪資起算日+",小於系統上線日:"+CH0.ON_LINE_DAY+",無法使用計算功能"
            //dt = sp0100DAO.getStartDayCompareOnLineDay(); 2015/07/01 MARK 勿刪 BY EVA
            //if ((int)dt.Rows[0]["resultCount"] == 0)
            //{
            //    string on_LINE_DAY_ADDSIX_M = Convert.ToDateTime(dt.Rows[0]["ON_LINE_DAY_ADDSIX_M"].ToString()).ToString("yyyy/MM/dd");
            //    string start_DT = Convert.ToDateTime(dt.Rows[0]["START_DT"].ToString()).ToString("yyyy/MM/dd");
            //    rtnmessage += "前六個月的薪資起算日:" + start_DT + "小於系統上線日:" + on_LINE_DAY_ADDSIX_M + ",無法使用計算功能!\\n";
            //    //return rtnmessage;
            //}

            //3.已離職不需 試算
            if (dao.COMPUTER_TYPE == "A") {
                dt = dao.getLeaveDT();
                if ((int)dt.Rows[0]["resultCount"] > 0)
                {  
                    rtnmessage += "此員工已離職不需試算!\\n";
                    //return rtnmessage;
                }
            }

            //4.計算類別="B" //精算
            //(1)至退休金主檔(TB_S_M_OLDRETIRE_H)檢核傳票號碼 <>空白 則msg不允重新精算退休金 
            if (dao.COMPUTER_TYPE == "B")
            {
                dt = dao.getOLDRETIRE();
                if ((int)dt.Rows[0]["resultCount"] > 0)
                {
                    rtnmessage += "此員工精算資料已結案,不允重新精算!\\n";
                    //return rtnmessage;
                }
            }
            //5.IF 委任經理人='Y' 時																									
	        //若委任日< 六月薪資起算日																								
            //MSG:委任日需小於 六月薪資起算日:+v六月薪資起算日																									


            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    dao.execSP_S_PENSION_COMPUTE();
                    rtnmessage += utilities.getSPLOG("SP_S_PENSION_COMPUTE");
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
            return rtnmessage;

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }

    }

}