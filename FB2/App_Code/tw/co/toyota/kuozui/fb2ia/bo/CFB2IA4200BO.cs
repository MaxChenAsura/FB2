using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2IA4200BO 的摘要描述
/// </summary>
public class CFB2IA4200BO : BaseService
{
    public CFB2IA4200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //檢核資料是否存在
    public string CheckDataNotExist(string SALARY_YM, string INS_TYPE)
    {
        try
        {
            CFB2IA4200DAO fb2ia = new CFB2IA4200DAO();

            
            if (INS_TYPE != "E")
            {
                DataTable dt = fb2ia.CheckDataNotExist_notE(SALARY_YM, INS_TYPE);
                if (dt.Rows.Count > 0)
                {
                    return "此保費年月,已轉薪資作業,不允重新轉入";
                }
                dt.Clear();

                //ins_type 不是全部
                if (INS_TYPE != "0")
                {
                    dt = fb2ia.FEES_MONTH_CHECK(SALARY_YM, INS_TYPE);
                    if (dt.Rows.Count > 0)
                    {
                        string total_result = dt.Rows[0]["total"].ToString();
                        if (total_result == "0")
                        {
                            return "此保費年月,尚未計算,不允轉入薪資";
                        }                        
                    }
                }
                else
                {                    
                    //ins_type = 全部
                    dt = fb2ia.FEES_MONTH_CHECK_A(SALARY_YM);
                    string total_result = dt.Rows[0]["total"].ToString();
                    if (total_result == "0")
                    {
                        return "此保費年月,勞保費尚未計算,不允轉入薪資";
                    }
                    dt.Clear();

                    dt = fb2ia.FEES_MONTH_CHECK_B(SALARY_YM);
                    total_result = dt.Rows[0]["total"].ToString();
                    if (total_result == "0")
                    {
                        return "此保費年月,健保費尚未計算,不允轉入薪資";
                    }
                    dt.Clear();

                    dt = fb2ia.FEES_MONTH_CHECK_C(SALARY_YM);
                    total_result = dt.Rows[0]["total"].ToString();
                    if (total_result == "0")
                    {
                        return "此保費年月,勞退尚未計算,不允轉入薪資";
                    }
                    dt.Clear();

                    dt = fb2ia.FEES_MONTH_CHECK_D(SALARY_YM);
                    total_result = dt.Rows[0]["total"].ToString();
                    if (total_result == "0")
                    {
                        return "此保費年月,團保尚未計算,不允轉入薪資";
                    }
                }             

               
            }
            if (INS_TYPE == "E" || INS_TYPE == "0")
            {
                DataTable dt2 = fb2ia.CheckDataNotExist(SALARY_YM, INS_TYPE);
                if (dt2.Rows.Count > 0)
                {
                    return "此保費年月,已轉薪資作業,不允重新轉入";
                }
                DataTable dt3 = fb2ia.CheckDataCount(INS_TYPE);
                if (Convert.ToInt32(dt3.Rows[0]["bb"]) > 0)
                {
                    return "尚有主管未審核之保費追溯資料,不允執行此功能";
                }

                if (INS_TYPE == "0")
                {
                    DataTable dt = new DataTable();

                    dt = fb2ia.FEES_MONTH_CHECK_A(SALARY_YM);
                    string total_result = dt.Rows[0]["total"].ToString();
                    if (total_result == "0")
                    {
                        return "此保費年月,勞保費尚未計算,不允轉入薪資";
                    }
                    dt.Clear();

                    dt = fb2ia.FEES_MONTH_CHECK_B(SALARY_YM);
                    total_result = dt.Rows[0]["total"].ToString();
                    if (total_result == "0")
                    {
                        return "此保費年月,健保費尚未計算,不允轉入薪資";
                    }
                    dt.Clear();

                    dt = fb2ia.FEES_MONTH_CHECK_C(SALARY_YM);
                    total_result = dt.Rows[0]["total"].ToString();
                    if (total_result == "0")
                    {
                        return "此保費年月,勞退尚未計算,不允轉入薪資";
                    }
                    dt.Clear();

                    dt = fb2ia.FEES_MONTH_CHECK_D(SALARY_YM);
                    total_result = dt.Rows[0]["total"].ToString();
                    if (total_result == "0")
                    {
                        return "此保費年月,團保尚未計算,不允轉入薪資";
                    }
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
    //轉入薪資
    public string Process(string SALARY_YM, string INS_TYPE, string SALARY_DT)
    {
        try
        {
            CFB2IA4200DAO fb2ia = new CFB2IA4200DAO();
            BeginTransaction();
            //資料期間(起) and 資料期間(迄)
            string START_DT = SALARY_YM + "01";
            int year = Convert.ToInt32(SALARY_YM.Substring(0, 4));
            int month = Convert.ToInt32(SALARY_YM.Substring(4, 2));
            string END_DT = SALARY_YM + DateTime.DaysInMonth(year, month);
            fb2ia.START_DT = START_DT;
            fb2ia.END_DT = END_DT;

            if (INS_TYPE == "E" || INS_TYPE == "0")
            {
                
                #region SA取消2014/10/08
                //fb2ia.Delete_TB_S_M_SUBSIDY_DEDUCTIONS_1(SALARY_YM);
                //DataTable dt = fb2ia.Get_TB_I_M_FEES_TRACEBACK();
                //foreach (DataRow dr in dt.Rows)
                //{
                //    string EMP_ID = Convert.ToString(dr["EMP_ID"]);
                //    string SALARY_ID = Convert.ToString(dr["SALARY_ID"]);
                //    if (EMP_ID == fb2ia.emp_id && SALARY_ID == fb2ia.salary_id)
                //        fb2ia.seq_no ++;
                //    else
                //        fb2ia.seq_no =1;
                //    fb2ia.emp_id = Convert.ToString(dr["EMP_ID"]);
                //    fb2ia.emp_name = Convert.ToString(dr["EMP_NAME"]);
                //    fb2ia.salary_id = Convert.ToString(dr["SALARY_ID"]);
                //    fb2ia.trace_amt = Convert.ToString(dr["TRACE_AMT"]);
                //    fb2ia.is_plus = Convert.ToString(dr["IS_PLUS"]);
                //    fb2ia.is_tax = Convert.ToString(dr["IS_TAX"]);
                //    fb2ia.remark = Convert.ToString(dr["REMARK"]);
                //    fb2ia.salary_ym = Convert.ToString(dr["SALARY_YM"]);
                //    fb2ia.ins_type = Convert.ToString(dr["INS_TYPE"]);
                //    fb2ia.identity_kind = Convert.ToString(dr["IDENTITY_KIND"]);
                //    fb2ia.license_id = Convert.ToString(dr["LICENSE_ID"]);
                //fb2ia.Add_TB_S_M_SUBSIDY_DEDUCTIONS_1(SALARY_YM);
                #endregion
                
                    fb2ia.Update_TB_I_M_FEES_TRACEBACK(SALARY_YM, SALARY_DT);
                //}
            }
            if (INS_TYPE == "A" || INS_TYPE == "0")
            {
                fb2ia.Delete_TB_S_M_SALARY_MONTH_CTRL_A(SALARY_DT);
                fb2ia.Add_TB_S_M_SALARY_MONTH_CTRL_A(SALARY_YM, SALARY_DT);
                fb2ia.Update_TB_I_R_FEES_MONTH_A(SALARY_YM, SALARY_DT);
            }
            if (INS_TYPE == "B" || INS_TYPE == "0")
            {
                fb2ia.Delete_TB_S_M_SALARY_MONTH_CTRL_B(SALARY_DT);
                fb2ia.Add_TB_S_M_SALARY_MONTH_CTRL_B(SALARY_YM, SALARY_DT);
                fb2ia.Update_TB_I_R_FEES_MONTH_B(SALARY_YM, SALARY_DT);
            }
            if (INS_TYPE == "C" || INS_TYPE == "0")
            {
                fb2ia.Delete_TB_S_M_SALARY_MONTH_CTRL_C(SALARY_DT);
                fb2ia.Add_TB_S_M_SALARY_MONTH_CTRL_C(SALARY_YM, SALARY_DT);
                fb2ia.Update_TB_I_R_FEES_MONTH_C(SALARY_YM, SALARY_DT);
            }
            if (INS_TYPE == "D" || INS_TYPE == "0")
            {
                fb2ia.Delete_TB_S_M_SALARY_MONTH_CTRL_D(SALARY_DT);
                fb2ia.Add_TB_S_M_SALARY_MONTH_CTRL_D(SALARY_YM, SALARY_DT);
                fb2ia.Update_TB_I_R_GROUP_MONTH_D(SALARY_YM, SALARY_DT);
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
}