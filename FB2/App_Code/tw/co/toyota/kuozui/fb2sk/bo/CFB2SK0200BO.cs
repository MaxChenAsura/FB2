using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.Text;
using log4net;

/// <summary>
/// CFB2SK0200BO 的摘要描述
/// </summary>
public class CFB2SK0200BO : BaseService
{

    public CFB2SK0200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public void insertData()
    {
        try
        {
            CFB2SK0200DAO sk020DAO = new CFB2SK0200DAO();
            //刪除舊的資料
            BeginTransaction();
            sk020DAO.Delete_TB_S_MUTUAL_EMP();
            Commit();

            BeginTransaction();
            sk020DAO.insert_TB_S_MUTUAL_EMP();
            Commit();
        }
        catch (Exception ex)
        {
            RollBack();
            throw;
        }
    }


    public void insertData(CFB2SK0200DAO fb2sk)
    {
        try
        {
            //刪除舊的資料
            BeginTransaction();
            fb2sk.Delete_TB_S_MUTUAL_EMP();
            Commit();

            DataTable dt = fb2sk.VW_H_EMP_DATA();
            BeginTransaction();
            int test = 0;
            string str = "";
            foreach (DataRow dr in dt.Rows)
            {

                fb2sk.EMP_ID = Convert.ToString(dr["EMP_ID"]);
                fb2sk.DEPT_NAME_20 = Convert.ToString(dr["DEPT_NAME_20"]);
                fb2sk.DEPT_NAME_30 = Convert.ToString(dr["DEPT_NAME_30"]);
                fb2sk.DEPT_NAME_40 = Convert.ToString(dr["DEPT_NAME_40"]);
                fb2sk.DEPT_NAME_50 = Convert.ToString(dr["DEPT_NAME_50"]);
                fb2sk.DEPT_NAME_60 = Convert.ToString(dr["DEPT_NAME_60"]);
                fb2sk.DEPT_NAME_70 = Convert.ToString(dr["DEPT_NAME_70"]);
                fb2sk.PLANT_CD = Convert.ToString(dr["PLANT_CD"]);

                if (Convert.ToString(dr["WORK_SHIFT_CD"]) != "" && Convert.ToString(dr["WORK_SHIFT_CD"]).Length >= 2)
                    fb2sk.LINE_CD = Convert.ToString(dr["WORK_SHIFT_CD"]).Substring(1, 1);
                else
                    fb2sk.LINE_CD = "";


                fb2sk.DEPT_NO = Convert.ToString(dr["DEPT_NO"]);
                fb2sk.WS_CD = Convert.ToString(dr["WS_CD"]);
                fb2sk.EMP_NAME = Convert.ToString(dr["EMP_NAME"]);
                fb2sk.JPN_CD = Convert.ToString(dr["JPN_CD"]);
                fb2sk.PJOB_CD = Convert.ToString(dr["PJOB_CD"]);
                fb2sk.PJOB_DESC = Convert.ToString(dr["PJOB_DESC"]);
                fb2sk.JOIN_DT = Convert.ToString(dr["JOIN_DT"]);
                fb2sk.BIRTH_DT = Convert.ToString(dr["BIRTH_DT"]);
                fb2sk.LICENSE_ID = Convert.ToString(dr["LICENSE_ID"]);
                fb2sk.SCHOOL_NAME = Convert.ToString(dr["SCHOOL_NAME"]);
                fb2sk.EMP_UPDATETIMED_DT = Convert.ToString(dr["UPDATETIMED_DT"]);
                fb2sk.EMP_CD = Convert.ToString(dr["EMP_CD"]);
                fb2sk.EMP_CHG_CD = Convert.ToString(dr["EMP_CHG_CD"]);
                fb2sk.CONTACT_TEL = Convert.ToString(dr["CONTACT_TEL"]);
                fb2sk.REGISTER_ADDR = Convert.ToString(dr["REGISTER_ADDR"]);
                fb2sk.CONTACT_ADDR = Convert.ToString(dr["CONTACT_ADDR"]);
                fb2sk.SALARY_ACCOUNT_NO = Convert.ToString(dr["SALARY_ACCOUNT_NO"]);
                fb2sk.LEVEL_CD = Convert.ToString(dr["LEVEL_CD"]);
                fb2sk.GRADE_CD = Convert.ToString(dr["GRADE_CD"]);
                fb2sk.EMP_STATUS = Convert.ToString(dr["EMP_STATUS"]);
                fb2sk.SHIFT_CD = Convert.ToString(dr["SHIFT_CD"]);

                fb2sk.Add_TB_S_MUTUAL_EMP();
                test += 1;
                if (test % 500 == 0)
                {
                    str = "";
                    Commit();
                    BeginTransaction();
                }
                //if (test == 500)
                //{
                //    break;
                //}
            }
            Commit();
        }
        catch (Exception ex)
        {
            RollBack();
            throw;
        }
    }

    public DataTable getData(CFB2SK0200DAO fb2sk)
    {
        try
        {
            DataTable dt2 = fb2sk.TB_S_MUTUAL_EMP();
            return dt2;
        }
        catch (Exception ex)
        {
            RollBack();
            return null;
        }
    }

    /*
    //utf8轉big5 移到utilities
    public string convertBig5(string strUtf) {
        string result = "";
        Encoding uft81 = Encoding.GetEncoding("UTF-8");
        Encoding big51 = Encoding.GetEncoding("BIG5");

        byte[] strUtf81 =  uft81.GetBytes(strUtf.Trim());
        byte[] strBig51 = Encoding.Convert(uft81, big51, strUtf81);
        char[] big5Chars1 =new char[big51.GetCharCount(strBig51, 0, strBig51.Length)];
        big51.GetChars(strBig51, 0, strBig51.Length, big5Chars1, 0);
        result = new string(big5Chars1);
        return result;
    }
    */

    public void Download(DataTable dt2, string toPath)
    {
        try
        {
            CFB2SK0200DAO sk020DAO = new CFB2SK0200DAO();

            DataTable dt = new DataTable();

            string EMP_ID = "";
            string DEPT_NAME_20 = "";
            string DEPT_NAME_30 = "";
            string DEPT_NAME_40 = "";
            string DEPT_NAME_50 = "";
            string DEPT_NAME_60 = "";
            string DEPT_NAME_70 = "";
            string PLANT_CD = "";
            string LINE_CD = "";
            string DEPT_NO = "";
            string WS_CD = "";
            string EMP_NAME = "";
            string PJOB_DESC = "";
            string JOIN_DT = "";
            string BIRTH_DT = "";
            string LICENSE_ID = "";
            string SCHOOL_NAME = "";
            string EMP_UPDATETIMED_DT = "";
            string EMP_CD = "";
            string EMP_CHG_CD = "";
            string CONTACT_TEL = "";
            string REGISTER_ADDR = "";
            string CONTACT_ADDR = "";
            string SALARY_ACCOUNT_NO = "";
            string LEVEL_CD = "";
            string GRADE_CD = "";
            string END_DT = "";
            string GRADE = "";
            string SHIFT_CD = "";
            //MemoryStream fileStream = new MemoryStream();
            using (StreamWriter sw = new StreamWriter(toPath, false, Encoding.GetEncoding("big5")))
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    //先清空
                    EMP_ID = "";
                    DEPT_NAME_20 = "";
                    DEPT_NAME_30 = "";
                    DEPT_NAME_40 = "";
                    DEPT_NAME_50 = "";
                    DEPT_NAME_60 = "";
                    DEPT_NAME_70 = "";
                    PLANT_CD = "";
                    LINE_CD = "";
                    DEPT_NO = "";
                    WS_CD = "";
                    EMP_NAME = "";
                    PJOB_DESC = "";
                    JOIN_DT = "";
                    BIRTH_DT = "";
                    LICENSE_ID = "";
                    SCHOOL_NAME = "";
                    EMP_UPDATETIMED_DT = "";
                    EMP_CD = "";
                    EMP_CHG_CD = "";
                    CONTACT_TEL = "";
                    REGISTER_ADDR = "";
                    CONTACT_ADDR = "";
                    SALARY_ACCOUNT_NO = "";
                    LEVEL_CD = "";
                    GRADE_CD = "";
                    END_DT = "";
                    GRADE = "";
                    SHIFT_CD = "";

                    // Add some text to the file.
                    #region MyRegion
                    EMP_ID = dt2.Rows[i]["EMP_ID"].ToString();
                    if (EMP_ID.Length > 6)
                        EMP_ID = EMP_ID.Substring(0, 6);
                    DEPT_NAME_20 = dt2.Rows[i]["DEPT_NAME_20"].ToString();
                    if (DEPT_NAME_20.Length > 7)
                        DEPT_NAME_20 = DEPT_NAME_20.Substring(0, 7);
                    DEPT_NAME_30 = dt2.Rows[i]["DEPT_NAME_30"].ToString();
                    if (DEPT_NAME_30.Length > 7)
                        DEPT_NAME_30 = DEPT_NAME_30.Substring(0, 7);
                    DEPT_NAME_40 = dt2.Rows[i]["DEPT_NAME_40"].ToString();
                    if (DEPT_NAME_40.Length > 7)
                        DEPT_NAME_40 = DEPT_NAME_40.Substring(0, 7);
                    DEPT_NAME_50 = dt2.Rows[i]["DEPT_NAME_50"].ToString();
                    if (DEPT_NAME_50.Length > 7)
                        DEPT_NAME_50 = DEPT_NAME_50.Substring(0, 7);
                    DEPT_NAME_60 = dt2.Rows[i]["DEPT_NAME_60"].ToString();
                    if (DEPT_NAME_60.Length > 11)
                        DEPT_NAME_60 = DEPT_NAME_60.Substring(0, 11);
                    DEPT_NAME_70 = dt2.Rows[i]["DEPT_NAME_70"].ToString();
                    if (DEPT_NAME_70.Length > 11)
                        DEPT_NAME_70 = DEPT_NAME_70.Substring(0, 11);
                    PLANT_CD = dt2.Rows[i]["PLANT_CD"].ToString();
                    if (PLANT_CD.Length > 1)
                        PLANT_CD = PLANT_CD.Substring(0, 1);
                    LINE_CD = dt2.Rows[i]["LINE_CD"].ToString();
                    if (LINE_CD.Length > 1)
                        LINE_CD = LINE_CD.Substring(0, 1);
                    DEPT_NO = dt2.Rows[i]["DEPT_NO"].ToString();
                    if (DEPT_NO.Length > 7)
                        DEPT_NO = DEPT_NO.Substring(0, 7);
                    WS_CD = dt2.Rows[i]["WS_CD"].ToString();
                    if (WS_CD.Length > 1)
                        WS_CD = WS_CD.Substring(0, 1);
                    EMP_NAME = dt2.Rows[i]["EMP_NAME"].ToString();
                    if (EMP_NAME.Length > 8)
                        EMP_NAME = EMP_NAME.Substring(0, 8);
                    PJOB_DESC = dt2.Rows[i]["PJOB_DESC"].ToString();
                    if (PJOB_DESC.Length > 6)
                        PJOB_DESC = PJOB_DESC.Substring(0, 6);
                    if (dt2.Rows[i]["JOIN_DT"] != null && dt2.Rows[i]["JOIN_DT"] != DBNull.Value)
                        JOIN_DT = Convert.ToDateTime(dt2.Rows[i]["JOIN_DT"]).ToString("yyyyMMdd");
                    else
                        JOIN_DT = dt2.Rows[i]["JOIN_DT"].ToString();
                    if (JOIN_DT.Length > 8)
                        JOIN_DT = JOIN_DT.Substring(0, 8);
                    if (dt2.Rows[i]["BIRTH_DT"] != null && dt2.Rows[i]["BIRTH_DT"] != DBNull.Value)
                        BIRTH_DT = Convert.ToDateTime(dt2.Rows[i]["BIRTH_DT"]).ToString("yyyyMMdd");
                    else
                        BIRTH_DT = dt2.Rows[i]["BIRTH_DT"].ToString();
                    if (BIRTH_DT.Length > 8)
                        BIRTH_DT = BIRTH_DT.Substring(0, 8);
                    LICENSE_ID = dt2.Rows[i]["LICENSE_ID"].ToString();
                    if (LICENSE_ID.Length > 10)
                        LICENSE_ID = LICENSE_ID.Substring(0, 10);
                    SCHOOL_NAME = dt2.Rows[i]["SCHOOL_NAME"].ToString();
                    if (SCHOOL_NAME.Length > 8)
                        SCHOOL_NAME = SCHOOL_NAME.Substring(0, 8);
                    if (dt2.Rows[i]["EMP_UPDATETIMED_DT"] != null && dt2.Rows[i]["EMP_UPDATETIMED_DT"] != DBNull.Value)
                        EMP_UPDATETIMED_DT = Convert.ToDateTime(dt2.Rows[i]["EMP_UPDATETIMED_DT"]).ToString("yyyyMMdd");
                    else
                        EMP_UPDATETIMED_DT = dt2.Rows[i]["EMP_UPDATETIMED_DT"].ToString();
                    if (EMP_UPDATETIMED_DT.Length > 8)
                        EMP_UPDATETIMED_DT = EMP_UPDATETIMED_DT.Substring(0, 8);
                    EMP_CD = dt2.Rows[i]["EMP_CD"].ToString();
                    if (EMP_CD.Length > 1)
                        EMP_CD = EMP_CD.Substring(0, 1);
                    EMP_CHG_CD = dt2.Rows[i]["EMP_CHG_CD"].ToString();
                    if (EMP_CHG_CD == "5")
                    {
                        int result = 0;
                        dt.Clear();
                        dt = sk020DAO.getCHANGE_CODE_B07(EMP_ID);
                        if (dt.Rows.Count > 0)
                        {
                            result = (int)dt.Rows[0]["resultCount"];
                        }
                        if (result > 0)
                        {
                            EMP_CHG_CD = "";
                        }
                    }

                    CONTACT_TEL = dt2.Rows[i]["CONTACT_TEL"].ToString();
                    if (CONTACT_TEL.Length > 11)
                        CONTACT_TEL = CONTACT_TEL.Substring(0, 11);
                    REGISTER_ADDR = dt2.Rows[i]["REGISTER_ADDR"].ToString();
                    if (REGISTER_ADDR.Length > 35)
                        REGISTER_ADDR = REGISTER_ADDR.Substring(0, 35);
                    CONTACT_ADDR = dt2.Rows[i]["CONTACT_ADDR"].ToString();
                    if (CONTACT_ADDR.Length > 35)
                        CONTACT_ADDR = CONTACT_ADDR.Substring(0, 35);
                    SALARY_ACCOUNT_NO = dt2.Rows[i]["SALARY_ACCOUNT_NO"].ToString();
                    if (SALARY_ACCOUNT_NO.Length > 14)
                        SALARY_ACCOUNT_NO = SALARY_ACCOUNT_NO.Substring(0, 14);
                    LEVEL_CD = dt2.Rows[i]["LEVEL_CD"].ToString();
                    if (LEVEL_CD.Length > 3)
                        LEVEL_CD = LEVEL_CD.Substring(0, 3);
                    GRADE_CD = dt2.Rows[i]["GRADE_CD"].ToString();
                    if (GRADE_CD.Length > 1)
                        GRADE_CD = GRADE_CD.Substring(0, 1);
                    if (dt2.Rows[i]["END_DT"] != null && dt2.Rows[i]["END_DT"] != DBNull.Value)
                        END_DT = Convert.ToDateTime(dt2.Rows[i]["END_DT"]).ToString("yyyyMMdd");
                    else
                        END_DT = dt2.Rows[i]["END_DT"].ToString();
                    if (END_DT.Length > 8)
                        END_DT = END_DT.Substring(0, 8);
                    GRADE = dt2.Rows[i]["GRADE"].ToString();
                    if (GRADE.Length > 1)
                        GRADE = GRADE.Substring(0, 1);
                    SHIFT_CD = dt2.Rows[i]["SHIFT_CD"].ToString();
                    if (SHIFT_CD.Length > 2)
                        SHIFT_CD = SHIFT_CD.Substring(0, 2);
                    #endregion


                    sw.Write(string.Format("{0,-6}", EMP_ID));
                    
                    //sw.Write(utilities.toWide(string.Format("{0,-7}", DEPT_NAME_20)));
                    //sw.Write(utilities.toWide(string.Format("{0,-7}", DEPT_NAME_30)));
                    //sw.Write(utilities.toWide(string.Format("{0,-7}", DEPT_NAME_40)));
                    //sw.Write(utilities.toWide(string.Format("{0,-11}", DEPT_NAME_50)));
                    //sw.Write(utilities.toWide(string.Format("{0,-11}", DEPT_NAME_60)));
                    //sw.Write(utilities.toWide(string.Format("{0,-11}", DEPT_NAME_70)));
                    sw.Write(utilities.toWide(string.Format("{0,-7}", utilities.convertBig5(DEPT_NAME_20).Replace("?", "？"))));//20170222 與江素玲確認，編碼不為BIG5的問號改為全型問號，她會再自己蒐尋出來調整 
                    sw.Write(utilities.toWide(string.Format("{0,-7}", utilities.convertBig5(DEPT_NAME_30).Replace("?", "？"))));//20170222 與江素玲確認，編碼不為BIG5的問號改為全型問號，她會再自己蒐尋出來調整 
                    sw.Write(utilities.toWide(string.Format("{0,-7}", utilities.convertBig5(DEPT_NAME_40).Replace("?", "？"))));//20170222 與江素玲確認，編碼不為BIG5的問號改為全型問號，她會再自己蒐尋出來調整             
                    sw.Write(utilities.toWide(string.Format("{0,-11}", utilities.convertBig5(DEPT_NAME_50).Replace("?", "？"))));//20170222 與江素玲確認，編碼不為BIG5的問號改為全型問號，她會再自己蒐尋出來調整 
                    sw.Write(utilities.toWide(string.Format("{0,-11}", utilities.convertBig5(DEPT_NAME_60).Replace("?", "？"))));//20170222 與江素玲確認，編碼不為BIG5的問號改為全型問號，她會再自己蒐尋出來調整 
                    sw.Write(utilities.toWide(string.Format("{0,-11}", utilities.convertBig5(DEPT_NAME_70).Replace("?", "？"))));//20170222 與江素玲確認，編碼不為BIG5的問號改為全型問號，她會再自己蒐尋出來調整 
                    sw.Write(string.Format("{0,-1}", PLANT_CD));
                    sw.Write(string.Format("{0,-1}", LINE_CD));
                    sw.Write(string.Format("{0,-7}", DEPT_NO));
                    sw.Write(string.Format("{0,-1}", WS_CD));
                    sw.Write(utilities.toWide(string.Format("{0,-8}", utilities.convertBig5(EMP_NAME).Replace("?", " "))));
                    sw.Write(utilities.toWide(string.Format("{0,-6}", PJOB_DESC)));
                    sw.Write(string.Format("{0,-8}", JOIN_DT));
                    sw.Write(string.Format("{0,-8}", BIRTH_DT));
                    sw.Write(string.Format("{0,-10}", LICENSE_ID));
                    sw.Write(utilities.toWide(string.Format("{0,-8}", SCHOOL_NAME)));
                    sw.Write(string.Format("{0,-8}", EMP_UPDATETIMED_DT));
                    sw.Write(string.Format("{0,-1}", EMP_CD));
                    sw.Write(string.Format("{0,-1}", EMP_CHG_CD));
                    sw.Write(string.Format("{0,-11}", CONTACT_TEL));
                    sw.Write(utilities.toWide(string.Format("{0,-35}", utilities.convertBig5(REGISTER_ADDR).Replace("?", " "))));
                    sw.Write(utilities.toWide(string.Format("{0,-35}", utilities.convertBig5(CONTACT_ADDR).Replace("?", " "))));
                    sw.Write(string.Format("{0,-14}", SALARY_ACCOUNT_NO));
                    sw.Write(string.Format("{0,-3}", LEVEL_CD));
                    sw.Write(string.Format("{0,-1}", GRADE_CD));
                    if (dt2.Rows[i]["END_DT"] != null && dt2.Rows[i]["END_DT"] != DBNull.Value)
                        sw.Write(Convert.ToDateTime(dt2.Rows[i]["END_DT"]).ToString("yyyyMMdd"));
                    else
                        sw.Write(string.Format("{0,-8}", END_DT));
                    sw.Write(string.Format("{0,-1}", GRADE));
                    sw.WriteLine(string.Format("{0,-1}", SHIFT_CD));
                }
                sw.Flush();
            }
            //return fileStream;
        }
        catch
        {
            throw;
        }
    }
}