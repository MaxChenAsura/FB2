using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;

/// <summary>
/// CFB2IA4100BO 的摘要描述
/// </summary>
public class CFB2IA4100BO : BaseService
{
	public CFB2IA4100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    //保費計算資料
    public string Calculate_InsFees(string pa_ins_type_a,string def_ym)
    {
        string msg = "0";
        bool successed = true;
        try
        {
            CFB2IA4100DAO fbsIA = new CFB2IA4100DAO();
            
            BeginTransaction();
                //1.刪除[TB_S_M_SALARY_MONTH_CTRL 薪資月結控制檔] 條件:發薪類別='A'//月薪 
                successed = fbsIA.Delete_TB_S_M_SALARY_MONTH_CTRL(pa_ins_type_a, def_ym);
                //2.刪除[TB_I_R_FEES_MONTH月份保費代扣資料檔 ]
                if (pa_ins_type_a == "0" || pa_ins_type_a != "D")
                {
                    successed = fbsIA.Delete_TB_I_R_FEES_MONTH(pa_ins_type_a, def_ym);
                   
                }
                //3.刪除[TB_I_R_GROUP_MONTH 月份團保代扣資料檔
                if (pa_ins_type_a == "0" || pa_ins_type_a == "D")
                {
                    successed = fbsIA.Delete_TB_I_R_GROUP_MONTH(pa_ins_type_a, def_ym);
                }
            Commit();
            BeginTransaction();
             //4.計算保費
               if ((pa_ins_type_a=="A" || pa_ins_type_a=="0") && successed ){  //勞保
                   fbsIA.Computer_FeeA(def_ym);
                   DataTable dtSPresult = fbsIA.checkSP("SP_I_COMPUTER_A_MONTH_FEES");
                   if (dtSPresult.Rows.Count > 0)
                   {
                       //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                       if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) != "Y")
                       {
                           msg += Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]) + "\\n";
                           return msg;
                       }
                   }
                   else
                   {
                       msg += "保費計算失敗" + "\\n";
                       return msg;
                   }
               }
               if ((pa_ins_type_a=="B" || pa_ins_type_a=="0") && successed){ //健保
                   fbsIA.Computer_FeeB(def_ym);
                   DataTable dtSPresult = fbsIA.checkSP("SP_I_COMPUTER_B_MONTH_FEES");
                   if (dtSPresult.Rows.Count > 0)
                   {
                       //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                       if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) != "Y")
                       {
                           msg += Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]) + "\\n";
                           return msg;
                       }
                   }
                   else
                   {
                       msg += "保費計算失敗" + "\\n";
                       return msg;
                   }
               }
               if ((pa_ins_type_a=="C" || pa_ins_type_a=="0") && successed){ //勞退
                 fbsIA.Computer_FeeC(def_ym);
                 DataTable dtSPresult = fbsIA.checkSP("SP_I_COMPUTER_C_MONTH_FEES");
                 if (dtSPresult.Rows.Count > 0)
                 {
                     //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                     if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) != "Y")
                     {
                         msg += Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]) + "\\n";
                         return msg;
                     }
                 }
                 else
                 {
                     msg += "保費計算失敗" + "\\n";
                     return msg;
                 }
               }
               if ((pa_ins_type_a=="D" || pa_ins_type_a=="0") && successed){ //團保
                  fbsIA.Computer_FeeD(def_ym);
                  DataTable dtSPresult = fbsIA.checkSP("SP_I_COMPUTER_D_MONTH_FEES");
                  if (dtSPresult.Rows.Count > 0)
                  {
                      //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                      if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) != "Y")
                      {
                          msg += Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]) + "\\n";
                          return msg;
                      }
                  }
                  else
                  {
                      msg += "保費計算失敗" + "\\n";
                      return msg;
                  }
               }
            Commit();
            return msg;
        }
        catch (Exception)
        {
            throw;
        }
    }
    // 
    //檢查資料是否鎖定
    public string CheckLock(string pa_ins_type_a,string def_ym)
    {
        string rtnmessage = "0";
        try
        {
            CFB2IA4100DAO fb2IA = new CFB2IA4100DAO();
            DataTable dt = fb2IA.getS_M_CRTL(pa_ins_type_a, def_ym);
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                rtnmessage = "此計算保費月份資料,已鎖定,不允重新計算。 \\n";
            }
            dt.Clear();
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    
    //取得最近一次薪資計算年月
    public string getLast_SALARY_YM()
    {
        string retVal = "";
        CFB2IA4100DAO fb2IA = new CFB2IA4100DAO();
        try
        {

            retVal = fb2IA.getLast_SALARY_YM();

            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }

   

  
}