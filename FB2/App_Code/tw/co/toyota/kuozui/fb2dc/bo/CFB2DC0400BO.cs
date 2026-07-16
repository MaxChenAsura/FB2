using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2DC0400BO 的摘要描述
/// </summary>
public class CFB2DC0400BO : BaseService
{
    public CFB2DC0400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public System.Data.DataTable getCARD_TYPE()
    {
        try
        {
            CFB2DC0400DAO dao = new CFB2DC0400DAO();
            return dao.getCARD_TYPE();

        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getCARD_USED_CD(string card_type, string CARD_MID_NO)
    {
        try
        {
            CFB2DC0400DAO dao = new CFB2DC0400DAO();
            return dao.getCARD_USED_CD(card_type, CARD_MID_NO);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getEMP_DATA(string card_used_cd, string emp_id)
    {
        try
        {
            CFB2DC0400DAO dao = new CFB2DC0400DAO();
            DataTable dt = new DataTable();
            if (card_used_cd == "A")
                return dao.getVW_H_EMP_DATA(emp_id);
            else if (card_used_cd == "B")
                return dao.getTB_D_M_VENDOR_D(emp_id);
            else
                return dt;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getLoginPlantCD(string emp_id)
    {
        try
        {
            CFB2DC0400DAO dao = new CFB2DC0400DAO();
            DataTable dt = new DataTable();

            return dao.getLoginPlantCD(emp_id);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public string addCard(CFB2DC0400DAO dao)
    {
        try
        {
            DataTable dupData = dao.getDupData();
            if (dupData.Rows.Count > 0)
            {
                return "卡號重覆";
            }

            string card_used_cd = dao.CARD_USED_CD.Split('-').First();
            if (card_used_cd == "A")
            {
                DataTable existEMP = dao.getExistEMP();
                if (existEMP.Rows.Count == 0)
                    return "該工號不存在員工人事主檔";
            }
            if (card_used_cd == "B")
            {
                DataTable existV_EMP = dao.getExistV_EMP();
                if (existV_EMP.Rows.Count == 0)
                    return "該廠商人員編號不存在廠商人員明細檔";
            }
            DataTable cardData = dao.getCardData(); //C1
            DataTable cardSeqData = dao.getCardSeqData(); //C2

            try
            {
                BeginTransaction();
                //因有期間工從卡片屬性00改為10
                //若卡片屬性是 00或10時,才註銷
                if (dao.CARD_TYPE == "00" || dao.CARD_TYPE == "10")
                {
                    dao.updateCardData_EMP();
                }

                //如果 C1.筆數 > 0
                if ((int)cardData.Rows[0]["CARD_COUNT"] > 0)
                {

                    /*
                    //如果 (參數.生效日期 - 1) <= 系統日期
                    if (DateTime.Parse(dao.START_DT).AddDays(-1) <= DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")))
                    {
                        //如果 畫面.生效日期 <= 系統日
                        if (DateTime.Parse(dao.START_DT) <= DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")))
                        {
                            //刪除 有效卡片異動檔
                            dao.deleteCardUPD_CTL();
                            //新增至有效卡片異動檔
                            dao.addCardUPD_CTL();
                        }

                    }
                     *  //如果 (參數.生效日期) <= 系統日期
                    if (DateTime.Parse(dao.START_DT) <= DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")))
                    {
                        //如果 畫面.生效日期 <= 系統日
                        if (DateTime.Parse(dao.START_DT) <= DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")))
                        {
                            //刪除 有效卡片異動檔
                            dao.deleteCardUPD_CTL();
                            //新增至有效卡片異動檔
                            dao.addCardUPD_CTL();
                        }
                    }
                    */
                    //變數.流水號 = C2.流水號 + 1

                    //更新前一筆卡號的 結束日期 = 畫面.生效日期 - 1
                    dao.updateCardData(cardSeqData.Rows[0]["CARD_SEQ"].ToString());
                  
                    //新增 卡片資料檔
                    dao.addNewCard(int.Parse(cardSeqData.Rows[0]["CARD_SEQ"].ToString()) + 1);

                    //如果 畫面.生效日期 <= 系統日
                    if (DateTime.Parse(dao.START_DT) <= DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")))
                    {
                        //刪除前一張卡的有效卡片異動檔
                        dao.CARD_SEQ = cardSeqData.Rows[0]["CARD_SEQ"].ToString();
                        if (Convert.ToInt32(dao.CARD_SEQ) >= 0)
                        {
                            dao.deleteCardUPD_CTL();
                            dao.addCardUPD_CTL();
                        }
                        //新增 前一張卡的 有效卡片異動檔 卡號變更區分 為D

                        dao.CARD_SEQ = (int.Parse(cardSeqData.Rows[0]["CARD_SEQ"].ToString()) + 1).ToString();
                        //刪除 有效卡片異動檔
                        dao.deleteCardUPD_CTL();
                        //新增至有效卡片異動檔
                        dao.addCardUPD_CTL("A");
                    }

                }
                else
                {
                    //新增 卡片資料檔
                    dao.addNewCard();

                    //如果 (參數.生效日期) <= 系統日期
                    if (DateTime.Parse(dao.START_DT) <= DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")))
                    {
                        //如果 畫面.生效日期 <= 系統日
                        if (DateTime.Parse(dao.START_DT) <= DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")))
                        {
                            //刪除 有效卡片異動檔
                            dao.deleteCardUPD_CTL();
                            //新增至有效卡片異動檔
                            dao.addCardUPD_CTL("A");
                        }
                    }
                }

                Commit();


            }
            catch (Exception ex)
            {
                RollBack();
                return ex.Message;
            }

            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string updateCard(CFB2DC0400DAO dao)
    {
        try
        {
            BeginTransaction();
            //更新 卡片資料檔
            dao.updateData();
            //如果 畫面.結束日期 = 系統日
            if (DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")) >= DateTime.Parse(dao.START_DT)
                && DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd")) <= DateTime.Parse(dao.END_DT))
            {
                //刪除 有效卡片異動檔
                dao.deleteCardUPD_CTL();
                //新增至有效卡片異動檔
                dao.addCardUPD_CTL("A");
            }
            else {
                //刪除 有效卡片異動檔
                dao.deleteCardUPD_CTL();
                //新增至有效卡片異動檔
                dao.addCardUPD_CTL("D");
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

    public string delete_Card(List<Tuple<string, string, string>> card_data)
    {
        try
        {
            CFB2DC0400DAO dao = new CFB2DC0400DAO();
            BeginTransaction();
            foreach (var item in card_data)
            {
                //以 明細畫面.卡片屬性 + 明細畫面.卡號 + 明細畫面.流水號 讀取 卡片資料檔，將資料刪除
                dao.deleteCard(item.Item1, item.Item2, item.Item3);

                //刪除 有效卡片異動檔 
                dao.deleteCardUPD_CTL(item.Item1, item.Item2, item.Item3);

                //新增至有效卡片異動檔
                dao.insertCardUPD_CTL(item.Item1, item.Item2, item.Item3);

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

    public string update_CardHandle(string PLANT_CD, List<Tuple<string, string, string, string>> card_data, string card_handle)
    {
        try
        {
            CFB2DC0400DAO dao = new CFB2DC0400DAO();
            BeginTransaction();
            string cardHandle = "";//挑選資料的卡片處理
            foreach (var item in card_data)
            {
                dao.CARD_TYPE = item.Item1;
                dao.CARD_MID_NO = item.Item2;
                cardHandle = item.Item4 != "" ? item.Item4.Substring(0, 1) : "";
                //若無新增的情況下,直接按 製作新卡,則要呼叫SP
                if (card_handle == "1" && cardHandle != "1")
                {
                    dao.SP_D_UPD_CARD_DATA_RE();
                }
                if (card_handle != "1" )
                {
                    //以 明細畫面.卡片屬性 + 明細畫面.卡號 + 明細畫面.流水號 讀取 卡片資料檔，將資料更改
                    dao.update_CardHandle(PLANT_CD, item.Item1, item.Item2, item.Item3, card_handle);
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

    public string add_CARD_UPD_NOW()
    {
        try
        {
            CFB2DC0400DAO dao = new CFB2DC0400DAO();
            dao.CREATED_BY = SessionHandle.Current.emp_id;
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2DC040";
            DataTable dt = dao.getCARD_UPD_CTL(); //讀取有效卡片異動檔 A
            DataTable dt2 = dao.getCLOCK(); //讀取 卡鐘情報檔 C 條件:卡鐘類別 = 勤務

            BeginTransaction();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //如果B.卡鐘類別-勤務 = 'Y' 或 B.卡鐘類別-
                    if (dt.Rows[i]["CLOCK_TYPE_A"].ToString() == "Y" || dt.Rows[i]["CLOCK_TYPE_B"].ToString() == "Y")
                    {
                        //由項目dt2取得多筆資料，新增 有效卡號更新控制檔(即時)-將新卡號建在每個卡鐘上
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            dao.add_CARD_UPD_NOW(dt.Rows[i]["CARD_NO"].ToString(), dt2.Rows[j]["CLOCK_NO"].ToString(),
                                                dt.Rows[i]["CARD_CHANGE_CD"].ToString());
                        }

                    }
                }
            }
            dao.deleteCARD_UPD_CTL();

            Commit();
            return "0";

        }
        catch (Exception ex)
        {

            RollBack();
            return ex.Message;
        }
    }

    //匯出製卡人員
    public string ExportToMake()
    {
        try
        {
            CFB2DC0400DAO dao = new CFB2DC0400DAO();
            dao.CREATED_BY = SessionHandle.Current.emp_id;
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.PLANT_CD = getLoginPlantCD(SessionHandle.Current.emp_id);
            dao.FUNC_ID = "FB2DC040";
            DataTable dt = dao.getExportToMake();
            DataTable PARAMETER = dao.getTB_9_M_PARAMETER(); //參數檔
            string card_temp_label = Convert.ToString(PARAMETER.Rows[0]["CARD_TEMP_LABEL"]); //臨時卡顯示名稱
            if (dt.Rows.Count > 0)
            {
                MemoryStream ms = null;
                TextWriter tw = null;
                ms = new MemoryStream();
                tw = new StreamWriter(ms);

                try
                {
                    BeginTransaction();

                    //刪除 卡片燒錄檔
                    dao.deleteCARD_DATA();
                    //刪除 卡片標籤列印檔
                    dao.deleteCARD_HANDLE();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //透過項目(1)，匯出至 卡片標籤列印檔
                        int order_seq = dt.Rows[i]["LEVEL_CD"].ToString() == "" ? 0 : int.Parse(dt.Rows[i]["ORDER_SEQ"].ToString());
                        int CODE_VAL1 = int.Parse(PARAMETER.Rows[0]["CARD_LEVEL_CD"].ToString()); //變數.資格代號

                        //透過項目(1)，匯出至卡片燒錄檔
                        if (dt.Rows[i]["CARD_USED_CD"].ToString() == "A")
                        {
                            dao.addCARD_DATA(dt.Rows[i]["CARD_NO"].ToString(), dt.Rows[i]["PERSON_ID"].ToString(),
                            dt.Rows[i]["CARD_NAME"].ToString(), dt.Rows[i]["DEPT_NO"].ToString(), dt.Rows[i]["CARD_HANDLE_DESC"].ToString(),
                            dt.Rows[i]["DEPT_NAME"].ToString(), dt.Rows[i]["PJOB_DESC"].ToString(), dt.Rows[i]["LEVEL_CD"].ToString(), dt.Rows[i]["WS_CD"].ToString(), dao.PLANT_CD);

                        }else{
                            dao.addCARD_DATA(dt.Rows[i]["CARD_NO"].ToString(), dt.Rows[i]["PERSON_ID"].ToString(),
                                dt.Rows[i]["CARD_NAME"].ToString(), dt.Rows[i]["DEPT_NO"].ToString(), dt.Rows[i]["CARD_HANDLE_DESC"].ToString(),
                                dt.Rows[i]["DEPT_NAME"].ToString(), "", "", "", dao.PLANT_CD);
                        }




                        if (dao.PLANT_CD == "1")//處理擔當工廠區分
                        {
                            if (dt.Rows[i]["CARD_USED_CD"].ToString() == "A" && order_seq <= CODE_VAL1)  //主管
                            {
                                dao.addCARD_B4C("CARD_PRINT_MGR1", dt.Rows[i]["CARD_NO"].ToString(), dt.Rows[i]["CARD_MID_NO"].ToString(), dt.Rows[i]["DEPT_NAME"].ToString(), dt.Rows[i]["PJOB_DESC"].ToString(),
                                                dt.Rows[i]["CARD_NAME"].ToString(), dt.Rows[i]["CARD_MID_NO"].ToString());
                            }
                            if (dt.Rows[i]["CARD_USED_CD"].ToString() == "A" && order_seq > CODE_VAL1) //非主管
                            {
                                if (dt.Rows[i]["EMP_CD"].ToString() == "1")
                                    dao.addCARD_B4C("CARD_PRINT_NOR1", dt.Rows[i]["CARD_NO"].ToString(), dt.Rows[i]["CARD_MID_NO"].ToString(), dt.Rows[i]["DEPT_NAME"].ToString(), dt.Rows[i]["CARD_NAME"].ToString(),
                                                    dt.Rows[i]["CARD_MID_NO"].ToString(), " ");
                                else if (dt.Rows[i]["EMP_CD"].ToString() == "2" || dt.Rows[i]["EMP_CD"].ToString() == "3")
                                    dao.addCARD_B4C("CARD_PRINT_SPE1", dt.Rows[i]["CARD_NO"].ToString(), dt.Rows[i]["CARD_MID_NO"].ToString(), dt.Rows[i]["DEPT_NAME"].ToString(), dt.Rows[i]["CARD_NAME"].ToString(),
                                                dt.Rows[i]["CARD_MID_NO"].ToString(), " ");
                            }
                            if (dt.Rows[i]["CARD_USED_CD"].ToString() == "B")
                            {
                                //社外 col1  改為  廠商名稱
                                dao.addCARD_B4C("CARD_PRINT_VEN1", dt.Rows[i]["CARD_NO"].ToString(), dt.Rows[i]["CARD_MID_NO"].ToString(), dao.getVendorName(dt.Rows[i]["CARD_MID_NO"].ToString()), dt.Rows[i]["CARD_NAME"].ToString(),
                                                dt.Rows[i]["CARD_MID_NO"].ToString(), " ");


                            }
                            if (dt.Rows[i]["CARD_USED_CD"].ToString() == "C")
                            {
                                //共用- col1  改為 卡片屬性設定檔的卡片屬性名稱(CARD_TYPE_DESC),col2  改為 卡片資料檔  姓名(CARD_NAME)
                                dao.addCARD_B4C("CARD_PRINT_COMM1", dt.Rows[i]["CARD_NO"].ToString(), dt.Rows[i]["CARD_MID_NO"].ToString(), dt.Rows[i]["CARD_NAME"].ToString(), dt.Rows[i]["CARD_NAME_C"].ToString(), dt.Rows[i]["CARD_MID_NO"].ToString(), " ");
                            }
                        }
                        else if (dao.PLANT_CD == "2")
                        {
                            if (dt.Rows[i]["CARD_USED_CD"].ToString() == "A" && order_seq <= CODE_VAL1)
                            {
                                dao.addCARD_B4C("CARD_PRINT_MGR2", dt.Rows[i]["CARD_NO"].ToString(), dt.Rows[i]["CARD_MID_NO"].ToString(), dt.Rows[i]["DEPT_NAME"].ToString(), dt.Rows[i]["PJOB_DESC"].ToString(),
                                                dt.Rows[i]["CARD_NAME"].ToString(), dt.Rows[i]["CARD_MID_NO"].ToString());
                            }
                            if (dt.Rows[i]["CARD_USED_CD"].ToString() == "A" && order_seq > CODE_VAL1)
                            {
                                if (dt.Rows[i]["EMP_CD"].ToString() == "1")
                                    dao.addCARD_B4C("CARD_PRINT_NOR2", dt.Rows[i]["CARD_NO"].ToString(), dt.Rows[i]["CARD_MID_NO"].ToString(), dt.Rows[i]["DEPT_NAME"].ToString(), dt.Rows[i]["CARD_NAME"].ToString(),
                                                    dt.Rows[i]["CARD_MID_NO"].ToString(), " ");
                                else if (dt.Rows[i]["EMP_CD"].ToString() == "2" || dt.Rows[i]["EMP_CD"].ToString() == "3")
                                    dao.addCARD_B4C("CARD_PRINT_SPE2", dt.Rows[i]["CARD_NO"].ToString(), dt.Rows[i]["CARD_MID_NO"].ToString(), dt.Rows[i]["DEPT_NAME"].ToString(), dt.Rows[i]["CARD_NAME"].ToString(),
                                                dt.Rows[i]["CARD_MID_NO"].ToString(), " ");
                            }
                            if (dt.Rows[i]["CARD_USED_CD"].ToString() == "B")
                            {
                                //社外 col1  改為  廠商名稱
                                dao.addCARD_B4C("CARD_PRINT_VEN2", dt.Rows[i]["CARD_NO"].ToString(), dt.Rows[i]["CARD_MID_NO"].ToString(), dao.getVendorName(dt.Rows[i]["CARD_MID_NO"].ToString()), dt.Rows[i]["CARD_NAME"].ToString(),
                                                dt.Rows[i]["CARD_MID_NO"].ToString(), " ");
                            }
                            if (dt.Rows[i]["CARD_USED_CD"].ToString() == "C")
                            {
                                //共用- col1  改為 卡片屬性設定檔的卡片屬性名稱(CARD_TYPE_DESC),col2  改為 卡片資料檔  姓名(CARD_NAME)
                                dao.addCARD_B4C("CARD_PRINT_COMM2", dt.Rows[i]["CARD_NO"].ToString(), dt.Rows[i]["CARD_MID_NO"].ToString(), dt.Rows[i]["CARD_NAME"].ToString(), dt.Rows[i]["CARD_NAME_C"].ToString(), dt.Rows[i]["CARD_MID_NO"].ToString(), " ");
                            }
                        }

                        //透過項目(1)，逐筆產生 卡片處理歷史檔
                        dao.addCARD_HANDLE(dt.Rows[i]["CARD_NO"].ToString(), dt.Rows[i]["PERSON_ID"].ToString(),
                            dt.Rows[i]["CARD_NAME"].ToString(), dt.Rows[i]["CARD_HANDLE_DESC"].ToString());
                        //更新卡片資料檔 卡片處理 = ''(空字串)
                        dao.update_CardHandle(dao.PLANT_CD, dt.Rows[i]["CARD_TYPE"].ToString(), dt.Rows[i]["CARD_MID_NO"].ToString(),
                            dt.Rows[i]["CARD_SEQ"].ToString(), "");
                    }
                    Commit();
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
            }
            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}