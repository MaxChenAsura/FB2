using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SH0300BO 的摘要描述
/// </summary>
public class CFB2SH0300BO : BaseService
{
    public CFB2SH0300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }



    //檢查是否為凍結狀態
    public string checkFreeze(CFB2SH0300DAO sh030DAO)
    {
        DataTable dt = sh030DAO.checkFreeze();
        string rtnmessage = "";
        if (dt.Rows[0]["FREEZE_FLAG"].ToString() == "Y")
        {
            rtnmessage += "此年獎回數已無法進行計算 \\n";
        }

        dt.Clear();
        return rtnmessage;
    }


    public string execute(CFB2SH0300DAO sh030DAO)
    {
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            //若需要則要進行邏輯檢查(與DB相關的)
            string rtnmessage = this.checkFreeze(sh030DAO);

            //檢查OK更新
            if (rtnmessage != "")
            {
                return rtnmessage;
            }

            DataTable tmp = sh030DAO.getAwardEmpData();
            bool isRound2 = false;
            if (sh030DAO.AWARD_ROUND == "2")
            {
                isRound2 = true;
            }
            bool isRound3 = false;
            if (sh030DAO.AWARD_ROUND == "3")
            {
                isRound3 = true;
            }

            //if (!isRound2)
            //{
            //    BeginTransaction();
            //}
            DataTable dt = new DataTable();

            //需計算的參數
            decimal level_pay = 0;
            decimal ability_pay = 0;
            decimal pjob_pay = 0;
            decimal profession_pay = 0;
            decimal food_subsidy = 0;
            decimal level_pay_before = 0;
            decimal ability_pay_before = 0;
            decimal pjob_pay_before = 0;
            decimal profession_pay_before = 0;
            decimal food_subsidy_before = 0;
            decimal work_days = 0;    //在職天數(年獎期間)
            decimal attend_days = 0;    //勤怠扣除天數(負數)
            decimal reward_days = 0;    //獎懲加減天數
            decimal discipline_days = 0;   //紀律扣除天數

            //計算公式
            decimal award_amt_round1 = 0;   //第一回年獎
            decimal award_amt_round2_R = 0;   //第二回年獎(累計至第3回發的2回年獎)
            decimal award_days = sh030DAO.AWARD_DAYS != "" ? Convert.ToDecimal(sh030DAO.AWARD_DAYS) : 0;  //年獎發放天數
            decimal base_AMT = 0;           //發放base 
            decimal base_AMT_before = 0;    //原發放base
            decimal work_persent = 0;       //在職比例
            decimal discipline_AMT = 0;     //紀律反映的扣除金額(base*紀律反映天數)-負數或0
            decimal assess = 1;             //考績反映 
            decimal assess_before = 1;      //原考績反映 
            int wk_award_amt_tmp = 0;      //WK年獎暫存金額 
            int wk_award_amt_level = 0;      //WK年獎金額原昇格者
            int wk_award_amt = 0;           //WK年獎金額 
            int wk_award_tax = 0;           //年獎稅額
            int wk_award_amt_r = 0;           //年獎實額
            decimal award_days_person = 0;  //年獎發放天數(員工個人)
            string levelup_flag = "";  //昇格註記 V:有昇格, 空白:無昇格
            string is_leave = "";           //是否非自願性離職
            string is_retire = "";          //是否退休


            /* 
             發放公式(一、三回)：																																																																	
		        WK年獎暫存金額        = [ 發放base/日 * 在職比例 *  年獎發放天數 ] * 考績反映  ±  紀律反映*發放base/日  																																																															
		        WK年獎金額-原昇格者= [ 原發放base/日  * 在職比例 *  年獎發放天數  ] * 原考績反映  ±  紀律反映*原發放base/日																																																															
            發放公式(二回)：																																																																	
		        WK年獎暫存金額        = [ 發放base/日 * 在職比例 *  年獎發放天數 ] * 考績反映  ±  紀律反映*發放base/日   - 第一回發放獎金																																																															
		        WK年獎金額-原昇格者= [ 原發放base/日  * 在職比例 *  年獎發放天數  ] * 原考績反映  ±  紀律反映*原發放base/日 - 第一回發放獎金																																																															
																																																									

             */

            //取得參數檔-獎金類所得稅率
            DataTable dt_param = utilities.getParameter("SL", "BOUNS_TAX_RATE");
            decimal taxRate = 0;
            decimal incomeLimit = 0;
            if (dt_param.Rows.Count > 0)
            {
                taxRate = Convert.ToDecimal(dt_param.Rows[0]["CODE_VAL1"].ToString());
            }
            //取得參數檔-所得稅代扣金額下限
            dt_param = utilities.getParameter("SL", "INCOME_LIMIT_LOW");
            if (dt_param.Rows.Count > 0)
            {
                incomeLimit = Convert.ToDecimal(dt_param.Rows[0]["CODE_VAL1"].ToString());
            }
            //潤年則為 366,其餘為365
            bool isLeapYear = utilities.isLeapYear(Convert.ToInt32(sh030DAO.AWARD_YEAR));
            int yearDays = 365;
            if (isLeapYear)
            {
                yearDays = 366;
            }

            BeginTransaction();
            foreach (DataRow dr in tmp.Rows)
            {
                    
                if (isRound2)
                {
                    award_amt_round1 = Convert.ToDecimal(dr["amt1"]);
                }
				//離退人員的第2回獎金
                award_amt_round2_R = 0;
                if (isRound3)
                {
                    award_amt_round2_R = Convert.ToDecimal(dr["amt2"]);
                }

                sh030DAO.EMP_ID = Convert.ToString(dr["EMP_ID"]);
                sh030DAO.LEVELUP_FLAG = Convert.ToString(dr["LEVELUP_FLAG"]);

                ability_pay = Convert.ToDecimal(dr["ABILITY_PAY"]);
                level_pay = Convert.ToDecimal(dr["LEVEL_PAY"]);
                pjob_pay = Convert.ToDecimal(dr["PJOB_PAY"]);
                profession_pay = Convert.ToDecimal(dr["PROFESSION_PAY"]);
                food_subsidy = Convert.ToDecimal(dr["FOOD_SUBSIDY"]);

                ability_pay_before = Convert.ToDecimal(dr["ABILITY_PAY_BEFORE"]);
                level_pay_before = Convert.ToDecimal(dr["LEVEL_PAY_BEFORE"]);
                pjob_pay_before = Convert.ToDecimal(dr["PJOB_PAY_BEFORE"]);
                profession_pay_before = Convert.ToDecimal(dr["PROFESSION_PAY_BEFORE"]);
                food_subsidy_before = Convert.ToDecimal(dr["FOOD_SUBSIDY_BEFORE"]);

                work_days = Convert.ToDecimal(dr["WORK_DAYS"]);
                attend_days = Convert.ToDecimal(dr["ATTEND_DAYS"]);
                reward_days = Convert.ToDecimal(dr["REWARD_DAYS"]);
                discipline_days = Convert.ToDecimal(dr["DISCIPLINE_DAYS"]);
                assess = Convert.ToDecimal(dr["AWARD_BASE"]);
                assess_before = Convert.ToDecimal(dr["AWARD_BASE_BEFORE"]);

                //是否非自願性離職
                is_leave = Convert.ToString(dr["IS_LEAVE"]);
                //是否退休
                is_retire = Convert.ToString(dr["IS_RETIRE"]);


                //相關金額歸零
                base_AMT = 0;
                base_AMT_before = 0;
                wk_award_amt_tmp = 0;
                wk_award_amt_level = 0;
                wk_award_amt = 0;
                wk_award_tax = 0;
                wk_award_amt_r = 0;

                base_AMT = (level_pay + ability_pay + pjob_pay + profession_pay + food_subsidy);
                base_AMT_before = (level_pay_before + ability_pay_before + pjob_pay_before + profession_pay_before + food_subsidy_before);

                //反映項目-勤怠
                if (sh030DAO.AWARD_ITEM_AL == "N")
                {
                    attend_days = 0;
                }

                //反映項目-獎懲
                if (sh030DAO.AWARD_ITEM_RP == "N")
                {
                    reward_days = 0;
                }
                //反映項目-紀律
                if (sh030DAO.AWARD_ITEM_D == "N")
                {
                    discipline_days = 0;
                }
                //反映項目-考績
                if (sh030DAO.AWARD_ITEM_A == "N")
                {
                    assess = 1;
                    assess_before = 1;
                }

                //在職比例
                work_persent = (work_days + attend_days);
                //紀律反映的扣除金額(base*紀律反映天數[獎懲加減天數+紀律扣除天數] )
                discipline_AMT = base_AMT * (discipline_days + reward_days)/30;
                //計算 WK年獎暫存金額(昇格註記)   
                levelup_flag = dr["LEVELUP_FLAG"].ToString();
                //個人年獎發放天數
                award_days_person = award_days;

                /*
                  base_AMT = 0;           //發放base 
                  base_AMT_before = 0;    //原發放base
                  work_persent = 0;       //在職比例
                  discipline = 0;         //紀律反映
                  assess = 1;             //考績反映 
                  assess_before = 1;      //原考績反映 
                 */

                if (isRound2)
                {
                    wk_award_amt_tmp = Convert.ToInt32(((base_AMT * work_persent * award_days_person) * assess / (30 * yearDays) + discipline_AMT - award_amt_round1));
                   /*
                    if (levelup_flag == "V")
                    {
                        wk_award_amt_level =  Convert.ToInt32(((base_AMT_before * work_persent * award_days_person) * assess_before / (30 * yearDays) + discipline_AMT - award_amt_round1) );
                    }
                   */
                }
                else
                {
                    wk_award_amt_tmp =  Convert.ToInt32(((base_AMT * work_persent * award_days_person) * assess / (30 * yearDays) + discipline_AMT));
                     /*
                    if (levelup_flag == "V")
                    {
                        wk_award_amt_level =  Convert.ToInt32(((base_AMT_before * work_persent * award_days_person) * assess_before / (30 * yearDays) + discipline_AMT));
                    }
                     */ 
                }

                //wk_award_amt = Math.Max(wk_award_amt_tmp, wk_award_amt_level);
				
				//離退人員要加上2回沒有領的年獎
                if (is_leave == "Y" || is_retire == "Y")
                    wk_award_amt = wk_award_amt_tmp + (int)award_amt_round2_R;
                else
                    wk_award_amt = wk_award_amt_tmp;

                if (wk_award_amt < incomeLimit)
                {
                    wk_award_tax = 0;
                }
                else
                {
                    wk_award_tax = (int)(wk_award_amt * taxRate);
                    wk_award_amt_r = wk_award_amt - wk_award_tax;
                }

                //更新年獎明細維護檔
                sh030DAO.AWARD_AMT = wk_award_amt > 0 ? Convert.ToString(wk_award_amt) : "0";
                sh030DAO.AWARD_TAX = wk_award_tax > 0 ? Convert.ToString(wk_award_tax) : "0";
                sh030DAO.AWARD_AMT_R = wk_award_amt_r > 0 ? Convert.ToString(wk_award_amt_r) : "0";
                sh030DAO.AWARD_AMT_TMEP = wk_award_amt_tmp > 0 ? Convert.ToString(wk_award_amt_tmp) : "0";
                sh030DAO.AWARD_AMT_LEVEL = wk_award_amt_level > 0 ? Convert.ToString(wk_award_amt_level) : "0";
                sh030DAO.AWARD_DAYS_PERSON = award_days_person > 0 ? Convert.ToString(award_days_person) : "0";

                sh030DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                sh030DAO.FUNC_ID = "FB2SH030";
                sh030DAO.execute_D(now, "TB_S_M_AWARD_DM");
                sh030DAO.execute_D(now, "TB_S_S_AWARD_D");


            }
            Commit();
            //if (!isRound2)
            //{
            //    Commit();
            //}

            BeginTransaction();
            //更新年獎維護檔
            //sh030DAO.RELEASE_DT= null; 改直接用DBNull
            sh030DAO.RELEASE_BY = "";
            //sh030DAO.APPROVE_DT= null;
            sh030DAO.APPROVE_BY = "";
            sh030DAO.APPROVE_STATUS = "N";
            sh030DAO.FREEZE_FLAG = "N";
            sh030DAO.execute_H(now);

            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }



    }

    //檢核
    public DataTable executeCheck(string award_year, string award_round)
    {
        //檢核
        CFB2SH0300DAO sh030DAO = new CFB2SH0300DAO();
        return sh030DAO.getData_H(award_year, award_round);
    }



}