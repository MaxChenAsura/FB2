using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;



/// <summary>
/// CFB2SL2100DAO 的摘要描述
/// </summary>
public class CFB2SL2100DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string WELFARE_AMT { get; set; }
    public string SALARY_AMT { get; set; }
    public string SALARY_ID { get; set; }
    public string AMOUNT { get; set; }
    public CFB2SL2100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        // 
    }
    public DataTable getCompany_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select COMPANY_CD,COMPANY_ID ");
            sb.AppendLine("   from TB_H_M_COMPANY ");
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //資料生成
    public void RunProcSP_S_SALARY_TAX_EXEC(string year, string salary_dt_s, string salary_dt_e, string login_id, string func_id)  //2014-09-25 fixed by Stanley Chen, add 2 parameters: login.id、function.id
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_SALARY_TAX_EXEC");
            ht.Add("@pDATA_YM", year);
            ht.Add("@pSALARY_DT_S", salary_dt_s);
            ht.Add("@pSALARY_DT_E", salary_dt_e);
            ht.Add("@pUserID", login_id);
            ht.Add("@pFuncID", func_id);

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //確認SP是否執行成功
    public DataTable checkSP(string proc_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select top (1) PROC_STATUS, PROC_LOG  ");
            sb.AppendLine("   from TB_H_R_SP_LOG                  ");
            sb.AppendLine("  where  PROC_ID = @PROC_ID            ");
            sb.AppendLine("  order by PROC_DT desc                ");
            ht.Add("@PROC_ID", proc_id);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //所得人所得資料
    public DataTable getPersonData(string year,string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select C.TAX_ORG_ID, D.TAX_SEQ, C.COMPANY_ID, D.TAX_CD, D.TAX_FORMAT, D.LICENSE_ID ");
            sb.AppendLine("     , D.LICENSE_CD, D.AMOUNT, D.TAX, D.INCOME, D.EMP_ID, D.SOFTWARE_CD, D.ERROR_CD ");
            sb.AppendLine("     , D.PAY_YR, D.EMP_NAME, D.REGISTER_ADDR, D.PAY_YM_START, D.PAY_YM_END          ");
            sb.AppendLine("     , D.RETIRE_AMT, D.FORM_CD, case when D.LICENSE_CD in ('5','6','7','8','9' ) then D.DUE_183 else '' end DUE_183");
            sb.AppendLine("     , D.COUNTRY_CD, D.TAX_DEAL_CD              ");
            sb.AppendLine("  from TB_S_R_IMX_DTL D                                                             ");
            sb.AppendLine("  left join TB_S_R_IMX_COMPANY C on C.COMPANY_CD = D.COMPANY_CD  and D.DATA_YM = C.DATA_YM  ");
            sb.AppendLine(" where D. DATA_YM = @DATA_YM   and D.COMPANY_CD = @COMPANY_CD and D.AMOUNT <> 0  and D.EMP_ID <> 'XXXXX'  --XXXXX 為受益人    ");
            sb.AppendLine(" order by D.TAX_SEQ                 ");
            ht.Add("@DATA_YM", year);
            ht.Add("@COMPANY_CD", company_cd);                                                           
            return dbConn.Query(sb, ht,true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //各類所得申報資料: 所得年度、公司別、國內國外、個人非個人、所得類別、類別區分 為群組
    public DataTable getGroupData(string year, string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select C.TAX_ORG_ID                                                               ");
            sb.AppendLine("      , case when FORIEN_CD = '1' then 'ZZ000001'                                  ");
            sb.AppendLine(" 			when FORIEN_CD = '2' then 'ZZ000002' end as NUMBER                    ");
            sb.AppendLine("      , C.COMPANY_ID                                                               ");
            sb.AppendLine("      , D.FORIEN_CD                                                                ");
            sb.AppendLine("      , D.PERSON_CD                                                                ");
            sb.AppendLine("      , D.TAX_FORMAT_CD1                                                           ");
            sb.AppendLine("      , D.TAX_FORMAT_CD2                                                           ");
            sb.AppendLine("      , COUNT(D.LICENSE_ID) as LICENSE_ID_COUNT                                    ");
            sb.AppendLine("      , SUM(D.AMOUNT) as AMOUNT                                                    ");
            sb.AppendLine("      , SUM(D.TAX) as TAX                                                          ");
            sb.AppendLine("      , C.TAX_ID                                                                   ");
            sb.AppendLine("      , D.PAY_YR                                                                   ");
            sb.AppendLine("      , SUM(D.RETIRE_AMT) as RETIRE_AMT                                            ");
            sb.AppendLine("      ,( select MIN(TAX_SEQ) MIN_TAX_SEQ                                           ");
            sb.AppendLine("           from TB_S_R_IMX_DTL D2                                                  ");
            sb.AppendLine("          where D.DATA_YM = D2.DATA_YM                                             ");
            sb.AppendLine("            and D.COMPANY_CD = D2.COMPANY_CD                                       ");
            sb.AppendLine("            and D.FORIEN_CD = D2.FORIEN_CD                                         ");
            sb.AppendLine("            and D.PERSON_CD = D2.PERSON_CD                                         ");
            sb.AppendLine("            and D.TAX_FORMAT_CD1 = D2.TAX_FORMAT_CD1                               ");
            sb.AppendLine("            and D.TAX_FORMAT_CD2 = D2.TAX_FORMAT_CD2 ) MIN_TAX_SEQ                 ");
            sb.AppendLine("       ,( select MAX(TAX_SEQ) MAX_TAX_SEQ                                          ");
            sb.AppendLine("           from TB_S_R_IMX_DTL D2                                                  ");
            sb.AppendLine("          where D.DATA_YM = D2.DATA_YM                                             ");
            sb.AppendLine("            and D.COMPANY_CD = D2.COMPANY_CD                                       ");
            sb.AppendLine("            and D.FORIEN_CD = D2.FORIEN_CD                                         ");
            sb.AppendLine("            and D.PERSON_CD = D2.PERSON_CD                                         ");
            sb.AppendLine("            and D.TAX_FORMAT_CD1 = D2.TAX_FORMAT_CD1                               ");
            sb.AppendLine("            and D.TAX_FORMAT_CD2 = D2.TAX_FORMAT_CD2 ) MAX_TAX_SEQ                 ");
            sb.AppendLine("       ,E.HOUSE_TAX_ID                                                             ");//房屋稅籍編號
            sb.AppendLine("   from TB_S_R_IMX_DTL D                                                           ");
            sb.AppendLine("   left join TB_S_R_IMX_COMPANY C on C.COMPANY_CD = D.COMPANY_CD and D.DATA_YM = C.DATA_YM  ");
            sb.AppendLine("   left join TB_H_M_COMPANY E on E.COMPANY_CD = D.COMPANY_CD ");//找出公司檔的 房屋稅籍編號
            sb.AppendLine("  where D.DATA_YM = @DATA_YM and D.COMPANY_CD = @COMPANY_CD and D.EMP_ID <> 'XXXXX' ");
            sb.AppendLine("  group by D.DATA_YM, D.COMPANY_CD, D.FORIEN_CD, D.PERSON_CD, D.TAX_FORMAT_CD1     ");
            sb.AppendLine("  , D.TAX_FORMAT_CD2 ,C.TAX_ORG_ID, C.COMPANY_ID, C.TAX_ID, D.PAY_YR, E.HOUSE_TAX_ID               ");
            ht.Add("@DATA_YM", year);
            ht.Add("@COMPANY_CD", company_cd);   
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //申報單位基本資料
    public DataTable getCompanyData(string year, string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select TAX_ORG_ID, COMPANY_ID, COMPANY_NAME, COMPANY_ADDR, CHAIRMAN_NAME ");
            sb.AppendLine("      , CONTACTER_NAME, CONTACTER_TEL, COMPANY_EMAIL, TAX_ID              ");
            sb.AppendLine("   from TB_S_R_IMX_COMPANY                                                ");
            sb.AppendLine("  where  DATA_YM = @DATA_YM  and COMPANY_CD = @COMPANY_CD                 ");
            ht.Add("@DATA_YM", year);
            ht.Add("@COMPANY_CD", company_cd);   
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}