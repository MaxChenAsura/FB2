using FB2.tw.co.toyota.kuozui.bo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// WFB2SC2100 的摘要描述
/// </summary>
public class WFB2SC2100BO : BaseService
{
    WFB2SC2100DAO dl = new WFB2SC2100DAO();
    public WFB2SC2100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public int GetDateil2GridDataCount(int startRowIndex, int maximumRows, string SALARY_TYPE, string SALARY_YM, string SALARY_DT)
    {
        return dl.GetDateil2GridDataCount(startRowIndex, maximumRows, SALARY_TYPE, SALARY_YM, SALARY_DT);
    }

    public DataTable GetDateil2GridData(int startRowIndex, int maximumRows, string SALARY_TYPE, string SALARY_YM, string SALARY_DT, string sortExpression)
    {
        return dl.GetDateil2GridData(startRowIndex, maximumRows, SALARY_TYPE, SALARY_YM, SALARY_DT, sortExpression);
    }

    public bool UnLock(List<WFB2SC2100Dateil2_UI_Data> daos)
    {
        this.BeginTransaction();
        try
        {
            foreach (WFB2SC2100Dateil2_UI_Data dao in daos)
            {

                //(1)依[薪資類別+發薪日期]讀取薪資計算主檔(TB_S_M_SALARY_CAL_H).處理狀態(PROCESS_STATUS)資料,若 處理狀態= 2(薪資計算)時,
                if (dl.GetTB_S_M_SALARY_CAL_H_PROCESS_STATUS(dao) == "2")
                {
                    dl.UpdateTB_S_M_SALARY_CAL_HByUnLock(dao);
                        //throw new Exception("資料不存在或已異動,取消鎖定失敗");
                    //(1.2)[薪資類別=A(月薪資類)+發薪日期] 刪除 薪資明細計算檔(TB_S_S_SALARY_PAY) 
                    dl.DeleteTB_S_S_SALARY_PAYByUnLock(dao);
                }
                //(2)若 資料列.月結啟動點(PROC_SOUCE) =2(薪資啟動)時,執行以下作業:
                if (dao.PROC_SOUCE == "2")
                {
                    //(2.1)若 畫面.發薪類別(SALARY_TYPE) = 'A'(月薪資類) 且資料列.前工程代號(OPERATION_ID) = G01(其他加扣月結)時,
                    if (dao.SALARY_TYPE == "A" && dao.OPERATION_ID == "G01")
                        dl.UpdateTB_S_M_SUBSIDY_DEDUCTIONS_1_Dateial2(dao, false);
                    //(2.2)若 畫面.發薪類別(SALARY_TYPE) = 'A'(月薪資類) 且資料列.前工程代號(OPERATION_ID) = J01(其他類獎金月結)時,
                    if (dao.SALARY_TYPE == "A" && dao.OPERATION_ID == "J01")
                        dl.UpdateTB_S_OTHER_BOUNS_D_Dateial2(dao, false);

                    //刪除 依[發薪類別 +發薪日期+前工程代號 ] 薪資月結控制檔(TB_S_M_SALARY_MONTH_CTRL)
                    dl.DeleteTB_S_M_SALARY_MONTH_CTRL_ByUnLock(dao);
                }
                if (dao.PROC_SOUCE == "1")
                {
                    //(3)依[發薪類別 +發薪日期+前工程代號 ]則更新 薪資月結控制檔(TB_S_M_SALARY_MONTH_CTRL),內容如下:
                    dl.UpdateTB_S_M_SALARY_MONTH_CTRL_Dateial2(dao, false);
                }
            }
            this.Commit();
            return true;
        }
        catch (Exception ex)
        {
            this.RollBack();
            throw ex;
        }
    }

    public string Lock(List<WFB2SC2100Dateil2_UI_Data> daos)
    {
        try
        {
            int A01_index = 0;
            this.BeginTransaction();
            string msg = "";
            bool isA01_Pass = false;
            int index = 0;

            foreach (WFB2SC2100Dateil2_UI_Data dao in daos)
            {

                //(1)若 資料列.月結啟動點(PROC_SOUCE) =2(薪資啟動)時,執行以下作業:,
                if (dao.PROC_SOUCE == "2")
                {
                    //(1.1)若 畫面.發薪類別(SALARY_TYPE) =  'A'(月薪資類 )且 資料列.前工程代號(OPERATION_ID) = A01(人事月結)時,
                    if (dao.SALARY_TYPE == "A" && dao.OPERATION_ID == "A01")
                    {

                        dl.ExecSPByLock(dao);
                        //確認SP有無成功
                        DataTable dtSPresult = dl.checkSP("SP_S_EMP_DATA_MONTH_EXEC");
                        if (dtSPresult.Rows.Count > 0)
                        {
                            //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                            if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) == "Y")
                            {
                                isA01_Pass = true;
                                A01_index = index;
                            }
                            else
                                msg += Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]) + "\\n";
                        }
                    }
                    //(1.2)若 畫面.發薪類別(SALARY_TYPE) = 'A'(月薪資類) 且資料列.前工程代號(OPERATION_ID) = G01(其他加扣月結)時,
                    if (dao.SALARY_TYPE == "A" && dao.OPERATION_ID == "G01")
                    {
                        dl.UpdateTB_S_M_SUBSIDY_DEDUCTIONS_1_Dateial2(dao, true);
                        dl.UpdateTB_S_M_SUBSIDY_DEDUCTIONS_D_Dateial2(dao, true);
                        dl.InsertTB_S_M_SALARY_MONTH_CTRL_Dateial2(dao, "G01");
                    }

                    // (1.3)若 畫面.發薪類別(SALARY_TYPE)= 'A'(月薪資類) 且 資料列.前工程代號(OPERATION_ID) = I01(預付薪)時,
                    if (dao.SALARY_TYPE == "A" && dao.OPERATION_ID == "I01")
                    {
                        if (dl.CheckTB_S_M_SALARY_CAL_H(dao) > 0)
                            dl.InsertTB_S_M_SALARY_MONTH_CTRL_Dateial2(dao, "I01");
                        else
                            throw new Exception(Resources.Resource.wfb2sc_Salary_NotFound);
                    }

                    //(1.4)若 畫面.發薪類別(SALARY_TYPE) = 'A'(月薪資類) 且資料列.前工程代號(OPERATION_ID) = J01(其他類獎金月結)時,
                    if (dao.SALARY_TYPE == "A" && dao.OPERATION_ID == "J01")
                    {
                        dl.UpdateTB_S_OTHER_BOUNS_D_Dateial2(dao, true);
                        dl.InsertTB_S_M_SALARY_MONTH_CTRL_Dateial2(dao, "J01");
                    }
                    //(1.5)若 畫面.發薪類別(SALARY_TYPE) =  'B'(預付薪)且 資料列.前工程代號(OPERATION_ID) = A02(對象生成)時,
                    //if (dao.SALARY_TYPE == "B" && dao.OPERATION_ID == "A02")
                    //    dl.ExecSPByLock(dao);
                    //dl.UpdateTB_S_M_SALARY_MONTH_CTRL_Dateial2(dao, true);
                }
                //(2)若 資料列.月結啟動點(PROC_SOUCE) =1(前工程啟動)時,執行以下作業:
                if (dao.PROC_SOUCE == "1")
                {
                    //(2.1)若前工程 = 'B01'(勤務月結) 或 'H01'(互助金金額設定)時,依[發薪類別 +發薪日期+前工程代號 ]則更新 薪資月結控制檔(TB_S_M_SALARY_MONTH_CTRL),內容如下:
                    //雖然處理方式一樣，先預留邏輯之後若有加邏輯較好處理
                    if (dao.OPERATION_ID == "B01" || dao.OPERATION_ID == "H01")
                        dl.UpdateTB_S_M_SALARY_MONTH_CTRL_Dateial2(dao, true);
                    else
                        dl.UpdateTB_S_M_SALARY_MONTH_CTRL_Dateial2(dao, true);

                }
                index++;
            }
            this.Commit();

            if (isA01_Pass)
            {
                BeginTransaction();
                WFB2SC2100Dateil2_UI_Data daoA01 = daos[A01_index];
                dl.InsertTB_S_M_SALARY_MONTH_CTRL_Dateial2(daoA01, "A01");
                Commit();
            }
            return msg;
        }
        catch (Exception ex)
        {
            this.RollBack();
            throw ex;
        }
    }

}