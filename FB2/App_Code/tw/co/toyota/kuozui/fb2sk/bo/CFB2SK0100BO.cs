using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.Text;

/// <summary>
/// CFB2SK0100BO 的摘要描述
/// </summary>
public class CFB2SK0100BO : BaseService
{
    public CFB2SK0100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable Action(CFB2SK0100DAO fb2sk, string DATA_YM)
    {
        try
        {
            BeginTransaction();
            fb2sk.Delete_TB_S_MUTUAL_TARGET();
            Commit();

            DataTable dt = fb2sk.TB_S_M_SALARY_PAY(DATA_YM);
            foreach (DataRow dr in dt.Rows)
            {
                fb2sk.EMP_ID = Convert.ToString(dr["EMP_ID"]);
                fb2sk.WELFARE_AMT = Convert.ToString(dr["AMOUNT_1020"]);
                fb2sk.SALARY_AMT = Convert.ToString(dr["AMOUNT_1030"]);

                BeginTransaction();
                fb2sk.Add_TB_S_MUTUAL_TARGET();
                Commit();
            }
            DataTable dt2 = fb2sk.TB_S_MUTUAL_TARGET();
            return dt2;
        }
        catch (Exception ex)
        {
            RollBack();
            return null;
        }
    }
    public MemoryStream Download(DataTable dt2)
    {
        try
        {
                MemoryStream fileStream = new MemoryStream();
                using (StreamWriter sw = new StreamWriter(fileStream,Encoding.GetEncoding("big5")))
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        // Add some text to the file.
                        sw.Write(string.Format("{0,-6}", dt2.Rows[i]["EMP_ID"].ToString()));
                        sw.Write(string.Format("{0,8}", dt2.Rows[i]["WELFARE_AMT"]));
                        sw.WriteLine(string.Format("{0,8}", dt2.Rows[i]["SALARY_AMT"]));
                    }

                    sw.Flush();


                }
                return fileStream;
        }
        catch 
        {
            throw;
        }
    }
}