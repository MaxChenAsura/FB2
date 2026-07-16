using FB2.tw.co.toyota.kuozui.bo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// CFB2DE0200BO 的摘要描述
/// </summary>
public class CFB2DE0200BO : BaseService
{
	public CFB2DE0200BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string getCal(string START_DT,string END_DT)
    {
        CFB2DE0200DAO dao = new CFB2DE0200DAO();   
        try
        {
            return dao.getCal(START_DT, END_DT);  

           
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string checkData(CFB2DE0200DAO dao)
    {       
        try
        {
            string err = "0";
            DataTable dt = dao.get_DAY_RECORD_No_T();
            if (dt.Rows.Count ==0)
            {
                DataTable dt1 =dao.get_RES_MIDDLE_RECORD();
                if (dt1.Rows.Count == 0)
                {
                    err += "此日期區間沒有資料可計算\\n";                    
                    return err;
                }
            }
            return err;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public string doExec(CFB2DE0200DAO dao)
    {       
        
        try
        {
            dao.CREATED_BY = SessionHandle.Current.emp_id;
            //dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2DE020";

            
            //BeginTransaction();

            //複製教育履歷資料到TEMP TABLE
            dao.copyEduTable();

            //call sp
            dao.SP_D_RESTAURANT_DAILY();

            //刪除教育履歷資料
            dao.dropEduTable();

            //確認SP有無成功
            DataTable dtSPresult = dao.checkSP("SP_D_RESTAURANT_DAILY");
            if (dtSPresult.Rows.Count > 0)
            {
                //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) != "Y")
                    return Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]) + "\\n";                
            }

            return "0";          

        }
        catch (Exception ex)
        {               
            //刪除教育履歷資料
            dao.dropEduTable();
            return ex.Message;
        }
    }


}