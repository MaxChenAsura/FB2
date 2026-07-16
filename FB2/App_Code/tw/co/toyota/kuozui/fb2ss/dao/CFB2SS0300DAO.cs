using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2DH0700DAO 的摘要描述
/// </summary>
public class CFB2SS0300DAO : BaseDAO
{

    public string JOIN_SDT   { get; set; }
    public string JOIN_EDT   { get; set; }
    public string BE_EMP_SDT { get; set; }
    public string BE_EMP_EDT { get; set; }
    public string LEAVE_SDT  { get; set; }
    public string LEAVE_EDT  { get; set; }
    public string IS_LEAVE   { get; set; }
    public string IS_BE_EMP  { get; set; }

    public CFB2SS0300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //EXCEL匯出
    public DataTable getExcelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"select 
                         A.EMP_ID 
                        ,A.EMP_NAME
                        ,CONVERT(VARCHAR(10),A.JOIN_DT,111) as JOIN_DT
                        ,CONVERT(VARCHAR(10),A.LEAVE_DT,111)  as LEAVE_DT
                        ,CONVERT(VARCHAR(10),dateadd(DAY,-1,A.LEAVE_DT),111) as FIRED_DT
                        ,CONVERT(VARCHAR(10),A.BE_EMP_DT ,111) as BE_EMP_DT
                        ,A.LEAVE_REASON+'-'+B.HR_CHG_DESC as  LEAVE_REASON_DESC
                        ,B.IS_LEAVE
                        ,CONVERT(VARCHAR(10),B1.SALARY_DT ,111) as B1_SALARY_DT
                        ,CONVERT(VARCHAR(10),F1.SALARY_DT ,111) as F1_SALARY_DT
                        ,CONVERT(VARCHAR(10),B2.SALARY_DT ,111) as B2_SALARY_DT
                        from TB_H_M_EMP A
                        left join TB_H_M_HR_CHANGE_CODE B on A.LEAVE_REASON = B.HR_CHG_CD
                        left join
                        (
                        --第1次激勵金
                        select EMP_ID, SALARY_DT from TB_S_M_INCENTIVE_PAY_D  with (nolock) where INCENTIVE_TYPE='B1'
                        )B1 on A.EMP_ID = B1.EMP_ID
                        left join
                        (
                        ----1st資遺費	
                        select EMP_ID, SALARY_DT from TB_S_M_INCENTIVE_PAY_D  with (nolock)  where INCENTIVE_TYPE='F1'
                        )F1 on A.EMP_ID = F1.EMP_ID
                        left join
                        (
                        --第2次激勵金
                        select EMP_ID, SALARY_DT from TB_S_M_INCENTIVE_PAY_D  with (nolock)  where INCENTIVE_TYPE='B2'
                        )B2 on A.EMP_ID = B2.EMP_ID
                        ");

            sb.Append("  where 1=1 ");
            //查詢條件
            if (JOIN_SDT != "") 
            {
                sb.Append(" and A.JOIN_DT >= @JOIN_SDT ");
                ht.Add("@JOIN_SDT", JOIN_SDT);
            }
            if (JOIN_EDT != "")
            {
                sb.Append(" and A.JOIN_DT <= @JOIN_EDT ");
                ht.Add("@JOIN_EDT", JOIN_EDT);
            }
            if (LEAVE_SDT != "")
            {
                sb.Append(" and A.LEAVE_DT >= @LEAVE_SDT ");
                ht.Add("@LEAVE_SDT", LEAVE_SDT);
            }
            if (LEAVE_EDT != "")
            {
                sb.Append(" and A.LEAVE_DT <= @LEAVE_EDT ");
                ht.Add("@LEAVE_EDT", LEAVE_EDT);
            }
            if (BE_EMP_SDT != "")
            {
                sb.Append(" and A.BE_EMP_DT >= @BE_EMP_SDT ");
                ht.Add("@BE_EMP_SDT", BE_EMP_SDT);
            }
            if (BE_EMP_EDT != "")
            {
                sb.Append(" and A.BE_EMP_DT <= @BE_EMP_EDT ");
                ht.Add("@BE_EMP_EDT", BE_EMP_EDT);
            }
            if (IS_LEAVE == "Y")
            {
                sb.Append(" and A.LEAVE_DT is not null ");
            }
            if (IS_LEAVE == "N")
            {
                sb.Append(" and A.LEAVE_DT is null ");
            }
            //轉正社員,才有值
            if (IS_BE_EMP == "Y")
            {
                sb.Append(" and A.BE_EMP_DT is not null ");
            }
            //沒轉正社員,則沒有值
            if (IS_BE_EMP == "N")
            {
                sb.Append(" and A.BE_EMP_DT is null ");
            }
            sb.Append(" order by EMP_ID ");
            
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
   
}