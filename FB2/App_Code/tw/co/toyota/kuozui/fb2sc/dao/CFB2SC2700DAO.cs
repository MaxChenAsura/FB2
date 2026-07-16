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
/// CFB2SC2700DAO 的摘要描述
/// </summary>
public class CFB2SC2700DAO : BaseDAO
{
    public string SALARY_TYPE { get; set; }
    public string SALARY_DT { get; set; }
    public string PAY_KIND { get; set; }
    public CFB2SC2700DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string salary_ym, string salary_sdt
                            , string salary_edt, string salary_type, string pay_sdt, string pay_edt, string pay_id)
    {
        try
        {
            if (sortExpression.Contains("SALARY_YM"))
                sortExpression = sortExpression.Replace("SALARY_YM", "t.SALARY_YM");
            if (sortExpression.Contains("SALARY_TYPE"))
                sortExpression = sortExpression.Replace("SALARY_TYPE", "t.SALARY_TYPE");
            if (sortExpression.Contains("SALARY_DT"))
                sortExpression = sortExpression.Replace("SALARY_DT", "t.SALARY_DT");
            if (sortExpression.Contains("PAY_KIND"))
                sortExpression = sortExpression.Replace("PAY_KIND", "t.PAY_KIND");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine("       t.SALARY_DT,t.SALARY_YM,t.SALARY_TYPE,t.SALARY_SDT ,t.SALARY_EDT ,t.DUTY_SDT,t.DUTY_EDT,t.PROCESS_STATUS,t.PAY_KIND                     ");
            sb.AppendLine("      ,t.PAY_KIND +'-'+ f.SALARY_NAME as DESC3,t.SALARY_TYPE +'-'+ e.SUB_DESC as DESC2,t.PROCESS_STATUS +'-'+ d.SUB_DESC as DESC1");
            sb.AppendLine("      ,p.PAY_ID,p.PAY_DT,p.IS_VOUCHER,p.EMAIL_DT,p.IS_EMAIL");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H t                                                                                                         ");
            sb.AppendLine("left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD                      ");
            sb.AppendLine("left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='SALARY_TYPE' and  t.SALARY_TYPE = e.SUB_CD                            ");
            sb.AppendLine("left join TB_S_M_SALARY_PAY_H p on  p.SALARY_DT= t.SALARY_DT and p.SALARY_TYPE = t.SALARY_TYPE and p.PAY_KIND = t.PAY_KIND    ");
            sb.AppendLine("left join VW_SALARYAND9999 f on  f.SALARY_ID =t.PAY_KIND ");
            sb.AppendLine("where 1=1                                                                                                                          ");

            if (salary_ym != "")
            {
                sb.AppendLine("and t.SALARY_YM=@SALARY_YM                                                                                                     ");
                ht.Add("@SALARY_YM", salary_ym.Replace("/", ""));
            }
            if (salary_sdt != "")
            {
                sb.AppendLine("and t.SALARY_DT>=@SALARY_SDT                                                                                                    ");
                ht.Add("@SALARY_SDT", salary_sdt);
            }
            if (salary_edt != "")
            {
                sb.AppendLine("and t.SALARY_DT<=@SALARY_EDT                                                                                                    ");
                ht.Add("@SALARY_EDT", salary_edt);
            }
            if (salary_type != "-1")
            {
                sb.AppendLine("and t.SALARY_TYPE=@SALARY_TYPE                                                                                                 ");
                ht.Add("@SALARY_TYPE", salary_type);
            }
            if (pay_sdt != "")
            {
                sb.AppendLine("and p.PAY_DT>=@PAY_SDT                                                                                                 ");
                ht.Add("@PAY_SDT", pay_sdt);
            }
            if (pay_edt != "")
            {
                sb.AppendLine("and p.PAY_DT<=@PAY_EDT                                                                                                 ");
                ht.Add("@PAY_EDT", pay_edt);
            }
            if (pay_id != "")
            {
                sb.AppendLine("and p.PAY_ID=@PAY_ID                                                                                                 ");
                ht.Add("@PAY_ID", pay_id);
            }

            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            //DataTable dt = dbConn.Query(sb, ht);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount(int startRowIndex, int maximumRows, string salary_ym, string salary_sdt
                       , string salary_edt, string salary_type, string pay_sdt, string pay_edt, string pay_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H t                                                                                                         ");
            sb.AppendLine("left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD                      ");
            sb.AppendLine("left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='SALARY_TYPE' and  t.SALARY_TYPE = e.SUB_CD                            ");
            sb.AppendLine("left join TB_S_M_SALARY_PAY_H p on  p.SALARY_DT= t.SALARY_DT and p.SALARY_TYPE = t.SALARY_TYPE and p.PAY_KIND = t.PAY_KIND         ");
            sb.AppendLine("left join VW_SALARYAND9999 f on  f.SALARY_ID =t.PAY_KIND ");
            sb.AppendLine("where 1=1                                                                                                                          ");

            if (salary_ym != "")
            {
                sb.AppendLine("and t.SALARY_YM=@SALARY_YM                                                                                                     ");
                ht.Add("@SALARY_YM", salary_ym.Replace("/", ""));
            }
            if (salary_sdt != "")
            {
                sb.AppendLine("and t.SALARY_DT>=@SALARY_SDT                                                                                                    ");
                ht.Add("@SALARY_SDT", salary_sdt);
            }
            if (salary_edt != "")
            {
                sb.AppendLine("and t.SALARY_DT<=@SALARY_EDT                                                                                                    ");
                ht.Add("@SALARY_EDT", salary_edt);
            }
            if (salary_type != "-1")
            {
                sb.AppendLine("and t.SALARY_TYPE=@SALARY_TYPE                                                                                                 ");
                ht.Add("@SALARY_TYPE", salary_type);
            }
            if (pay_sdt != "")
            {
                sb.AppendLine("and p.PAY_DT>=@PAY_SDT                                                                                                 ");
                ht.Add("@PAY_SDT", pay_sdt);
            }
            if (pay_edt != "")
            {
                sb.AppendLine("and p.PAY_DT<=@PAY_EDT                                                                                                 ");
                ht.Add("@PAY_EDT", pay_edt);
            }
            if (pay_id != "")
            {
                sb.AppendLine("and p.PAY_ID=@PAY_ID                                                                                                 ");
                ht.Add("@PAY_ID", pay_id);
            }
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total_record"];
            }
            return t;

        }
        catch (Exception)
        {

            throw;
        }

    }

    public DataTable getTitleData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select t.SALARY_DT,t.SALARY_YM,t.SALARY_TYPE,t.PROCESS_STATUS,t.PAY_KIND                     ");
            sb.AppendLine("      ,t.PAY_KIND +'-'+ f.SALARY_NAME as DESC3,t.SALARY_TYPE +'-'+ e.SUB_DESC as DESC2,t.PROCESS_STATUS +'-'+ d.SUB_DESC as DESC1");
            sb.AppendLine("      ,p.PAY_ID,p.PAY_DT,p.EMAIL_DT");
            sb.AppendLine("from TB_S_M_SALARY_CAL_H t                                                                                                         ");
            sb.AppendLine("left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD                      ");
            sb.AppendLine("left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='SALARY_TYPE' and  t.SALARY_TYPE = e.SUB_CD                            ");
            sb.AppendLine("left join TB_S_M_SALARY_PAY_H p on  p.SALARY_DT= t.SALARY_DT and p.SALARY_TYPE = t.SALARY_TYPE and p.PAY_KIND = t.PAY_KIND     ");
            sb.AppendLine("left join VW_SALARYAND9999 f on  f.SALARY_ID =t.PAY_KIND ");
            sb.AppendLine("where 1=1 and t.SALARY_TYPE=@SALARY_TYPE and t.SALARY_DT=@SALARY_DT and t.PAY_KIND=@PAY_KIND                                           ");
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@PAY_KIND", PAY_KIND);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getEmailData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //(A)若發放項目(PAY_KIND) = '1031'(年獎)
            if (PAY_KIND == "1031")
            {
                sb.AppendLine(" SELECT t1.SALARY_DT,t1.SALARY_TYPE,t1.PAY_KIND,h.AWARD_YEAR,h.AWARD_ROUND                                                                     ");
                sb.AppendLine("       ,REPLACE (t3.SUB_DESC, '【資料年】', convert(varchar(4),h.AWARD_YEAR))  as DESC2,t2.SUB_DESC as DESC1                                   ");
                sb.AppendLine(" FROM TB_S_M_SALARY_CAL_H t1                                                                                                                   ");
                sb.AppendLine(" left join TB_S_M_AWARD_H h on   t1.SALARY_DT = h.SALARY_DT                                                                                    ");
                sb.AppendLine(" left join TB_9_M_COMM_D t2 on  t2.SYS_CD ='SC' and  t2.MAIN_CD='MAIL_TEXT' and  t2.CODE_VAL1 = t1.PAY_KIND and t2.CODE_VAL2 = h.AWARD_ROUND  ");
                sb.AppendLine(" left join TB_9_M_COMM_D t3 on  t3.SYS_CD ='SC' and  t3.MAIN_CD='MAIL_TITLE' and  t3.CODE_VAL1 = t1.PAY_KIND and t3.CODE_VAL2 = h.AWARD_ROUND ");
                sb.AppendLine(" where t1.PAY_KIND='1031' ");
                sb.AppendLine("  and t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE   ");
                ht.Add("@SALARY_TYPE", SALARY_TYPE);
                ht.Add("@SALARY_DT", SALARY_DT);
                return dbConn.Query(sb, ht);
            }
            //(B)若發放項目(PAY_KIND) = '1035'(節金)或 '1032'(一時金)或 '1062'(優退金) 或'1056'(遣散費)或'1070'(離職金)
            else if (PAY_KIND == "1035" || PAY_KIND == "1032" || PAY_KIND == "1062" || PAY_KIND == "1056" || PAY_KIND == "1070")
            {
                sb.AppendLine(" SELECT t1.SALARY_DT,t1.SALARY_TYPE,t1.PAY_KIND,h.FESTIVAL_DT,h.FESTIVAL_TYPE                                                                    ");
                sb.AppendLine("       ,CASE WHEN t1.PAY_KIND in('1035','1032') THEN REPLACE (t3.SUB_DESC, '【資料年】', CONVERT(varchar(4), h.FESTIVAL_DT, 112))                ");
                sb.AppendLine("             WHEN t1.PAY_KIND in('1062','1056','1070') THEN REPLACE (t3.SUB_DESC, '【員工編號】', d1.EMP_ID+vm.EMP_NAME )                        ");
                sb.AppendLine("        end as DESC2                                                                                                                              ");
                sb.AppendLine("       ,t2.SUB_DESC as DESC1                                                                                                                     ");
                sb.AppendLine(" FROM TB_S_M_SALARY_CAL_H t1                                                                                                                     ");
                sb.AppendLine(" left join TB_S_M_FESTIVAL_H h on t1.SALARY_DT = h.SALARY_DT                                                                                     ");
                sb.AppendLine(" left join TB_S_M_FESTIVAL_D d1 on h.FESTIVAL_TYPE = d1.FESTIVAL_TYPE and h.FESTIVAL_DT = d1.FESTIVAL_DT                                         ");
                sb.AppendLine("       and h.FESTIVAL_PAY_DT = d1.FESTIVAL_PAY_DT  and d1.FESTIVAL_TYPE in('4','5','6')                                                          ");
                sb.AppendLine(" left join VW_H_EMP_DATA vm on d1.EMP_ID = vm.EMP_ID                                                                                             ");
                sb.AppendLine(" left join TB_9_M_COMM_D t2 on  t2.SYS_CD ='SC' and  t2.MAIN_CD='MAIL_TEXT' and  t2.CODE_VAL1 = t1.PAY_KIND and t2.CODE_VAL2 = h.FESTIVAL_TYPE   ");
                sb.AppendLine(" left join TB_9_M_COMM_D t3 on  t3.SYS_CD ='SC' and  t3.MAIN_CD='MAIL_TITLE' and  t3.CODE_VAL1 = t1.PAY_KIND  and t3.CODE_VAL2 = h.FESTIVAL_TYPE ");
                sb.AppendLine(" where t1.PAY_KIND in ('1035', '1032' , '1062', '1056','1070')    ");
                sb.AppendLine("  and t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE ");
                ht.Add("@SALARY_TYPE", SALARY_TYPE);
                ht.Add("@SALARY_DT", SALARY_DT);
                return dbConn.Query(sb, ht);
            }
            //(C)若發放項目(PAY_KIND) = '9999'(月薪資) 或 '1061'(預付薪)或 '1038'(先發金) 或'1039'(期滿金)
            else if (PAY_KIND == "9999" || PAY_KIND == "1061" || PAY_KIND == "1038" || PAY_KIND == "1039")
            {
                sb.AppendLine(" SELECT t1.SALARY_DT,t1.SALARY_TYPE,t1.PAY_KIND                                                                                                              ");
                sb.AppendLine("       ,CASE WHEN t1.PAY_KIND in('9999','1061') THEN REPLACE (t3.SUB_DESC, '【薪資年月】', substring(t1.SALARY_YM,1,4)+'年'+substring(t1.SALARY_YM,5,2)+'月')");
                sb.AppendLine("             WHEN t1.PAY_KIND in('1038','1039') THEN REPLACE (t3.SUB_DESC, '【薪資年月】', substring(CONVERT(char(10), getdate(), 112),1,4)+'年'             ");
                sb.AppendLine(" 											                + substring(CONVERT(char(10), getdate(), 112),5,2)+'月')                                           ");
                sb.AppendLine("        end as DESC2                                                                                                                                          ");
                sb.AppendLine("       ,t2.SUB_DESC as DESC1                                                                                                                                 ");
                sb.AppendLine(" FROM TB_S_M_SALARY_CAL_H t1                                                                                                                                 ");
                sb.AppendLine(" left join TB_9_M_COMM_D t2 on  t2.SYS_CD ='SC' and  t2.MAIN_CD='MAIL_TEXT' and  t2.CODE_VAL1 = t1.PAY_KIND                                                  ");
                sb.AppendLine(" left join TB_9_M_COMM_D t3 on  t3.SYS_CD ='SC' and  t3.MAIN_CD='MAIL_TITLE' and  t3.CODE_VAL1 = t1.PAY_KIND                                                 ");
                sb.AppendLine(" where t1.PAY_KIND in ('9999', '1061' , '1038', '1039')                                                                      ");
                sb.AppendLine("  and t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE                                                                     ");
                ht.Add("@SALARY_TYPE", SALARY_TYPE);
                ht.Add("@SALARY_DT", SALARY_DT);
                return dbConn.Query(sb, ht);
            }
            //(D)若發放項目(PAY_KIND) = '1035'(節金)
            else if (PAY_KIND == "1035")
            {
                sb.AppendLine(" SELECT t1.SALARY_DT,t1.SALARY_TYPE,t1.PAY_KIND,h.FESTIVAL_DT,h.FESTIVAL_TYPE                                                                    ");
                sb.AppendLine("       ,REPLACE (t3.SUB_DESC,'【資料年】', convert(varchar(4), h.FESTIVAL_DT,112)) as DESC2                                                      ");
                sb.AppendLine("       ,t2.SUB_DESC as DESC1                                                                                                                     ");
                sb.AppendLine(" FROM TB_S_M_SALARY_CAL_H t1                                                                                                                     ");
                sb.AppendLine(" left join TB_S_M_FESTIVAL_H h on   t1.SALARY_DT = h.SALARY_DT                                                                                   ");
                sb.AppendLine(" left join TB_9_M_COMM_D t2 on  t2.SYS_CD ='SC' and  t2.MAIN_CD='MAIL_TEXT' and  t2.CODE_VAL1 = t1.PAY_KIND and  t2.CODE_VAL2 = h.FESTIVAL_TYPE  ");
                sb.AppendLine(" left join TB_9_M_COMM_D t3 on  t3.SYS_CD ='SC' and  t3.MAIN_CD='MAIL_TITLE' and  t3.CODE_VAL1 = t1.PAY_KIND  and  t3.CODE_VAL2 = h.FESTIVAL_TYPE");
                sb.AppendLine(" where t1.PAY_KIND='1035'                                                                      ");
                sb.AppendLine("  and t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE                                                                     ");
                ht.Add("@SALARY_TYPE", SALARY_TYPE);
                ht.Add("@SALARY_DT", SALARY_DT);
                return dbConn.Query(sb, ht);
            }
            //(E)若發放項目(PAY_KIND) = '1033'(紅利)
            else if (PAY_KIND == "1033")
            {
                sb.AppendLine(" SELECT t1.SALARY_DT,t1.SALARY_TYPE,t1.PAY_KIND,h.BONUS_YEAR  ");
                sb.AppendLine("       ,REPLACE (t3.SUB_DESC,'【資料年】', h.BONUS_YEAR) as DESC2  ");
                sb.AppendLine("       ,t2.SUB_DESC as DESC1 ");
                sb.AppendLine(" FROM TB_S_M_SALARY_CAL_H t1 ");
                sb.AppendLine(" left join TB_S_M_BONUS_H h on  t1.SALARY_DT = h.SALARY_DT ");
                sb.AppendLine(" left join TB_9_M_COMM_D t2 on  t2.SYS_CD ='SC' and  t2.MAIN_CD='MAIL_TEXT' and  t2.CODE_VAL1 = t1.PAY_KIND  ");
                sb.AppendLine(" left join TB_9_M_COMM_D t3 on  t3.SYS_CD ='SC' and  t3.MAIN_CD='MAIL_TITLE' and  t3.CODE_VAL1 = t1.PAY_KIND ");
                sb.AppendLine(" where t1.PAY_KIND='1033'  ");
                sb.AppendLine(" and t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE ");
                ht.Add("@SALARY_TYPE", SALARY_TYPE);
                ht.Add("@SALARY_DT", SALARY_DT);
                return dbConn.Query(sb, ht);
            }
            else
            {
                sb.Clear();
                ht.Clear();
                return null;
            }

        }
        catch
        {
            throw;
        }
    }
    public DataTable getEmpData(string SALARY_TYPE, string PAY_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //(A)若資料列.發薪類別  ='A'(月薪資類) 
            if (SALARY_TYPE == "A")
            {
                sb.AppendLine("SELECT distinct t1.EMP_ID,d1.SALARY_EMAIL                                                                                     ");
                sb.AppendLine("FROM TB_S_M_SALARY_PAY t1                                                                                                     ");
                sb.AppendLine("left join TB_S_M_SALARY_CAL_H h on t1.SALARY_DT = h.SALARY_DT and t1.SALARY_TYPE = h.SALARY_TYPE and t1.PAY_KIND = t1.PAY_KIND");
                sb.AppendLine("left join TB_S_M_EMP_RESULT m1 on m1.SALARY_YM = h.SALARY_YM  and m1.EMP_ID= t1.EMP_ID   and m1.SALARY_DT = t1.SALARY_DT      ");
                sb.AppendLine("left join TB_H_M_EMP d1 on m1.EMP_ID= d1.EMP_ID                                                                              ");
                sb.AppendLine("where t1.PAY_ID = @PAY_ID and m1.COMPANY_CD='K' and isnull(m1.JPN_CD,'') ='' and isnull(d1.SALARY_EMAIL,'') <> ''            ");
            }
            //(B)若資料列.發薪類別  <>'A'(月薪資類) 
            else
            {
                sb.AppendLine(" select a.* from (                                                                                                     ");
                sb.AppendLine(" SELECT distinct t1.EMP_ID,d1.SALARY_EMAIL,ISNULL(SUM(t1.AMOUNT),0) AMOUNT                                                        ");
                sb.AppendLine(" FROM TB_S_M_SALARY_PAY t1                                                                                                       ");
                sb.AppendLine(" left join TB_S_M_SALARY_CAL_H h on t1.SALARY_DT = h.SALARY_DT and t1.SALARY_TYPE = h.SALARY_TYPE and t1.PAY_KIND = t1.PAY_KIND  ");
                sb.AppendLine(" left join TB_S_M_EMP_RESULT_TMP m1 on m1.SALARY_DT = h.SALARY_DT and m1.SALARY_TYPE = h.SALARY_TYPE and m1.PAY_KIND = h.PAY_KIND");
                sb.AppendLine("      and m1.EMP_ID= t1.EMP_ID  and m1.SALARY_DT = t1.SALARY_DT                                                                  ");
                sb.AppendLine(" left join  TB_H_M_EMP d1 on m1.EMP_ID= d1.EMP_ID                                                                                                  ");
                sb.AppendLine(" where t1.PAY_ID = @PAY_ID and m1.COMPANY_CD='K' and isnull(m1.JPN_CD,'') ='' and isnull(d1.SALARY_EMAIL,'') <> ''               ");
                sb.AppendLine(" GROUP BY t1.EMP_ID,d1.SALARY_EMAIL ");
                sb.AppendLine(" )a where a.AMOUNT <> 0   ");
            }

            ht.Add("@PAY_ID", PAY_ID);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public void Del_TB_S_C_MAIL_BAT_H(string SALARY_TYPE, string SALARY_DT, string PAY_KIND, string EMAIL_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("Delete from TB_S_C_MAIL_BAT_H ");
            sb.AppendLine("where SALARY_TYPE=@SALARY_TYPE and SALARY_DT=@SALARY_DT and PAY_KIND=@PAY_KIND and SEND_DT=@SEND_DT");
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@SEND_DT", EMAIL_DT);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public void Del_TB_S_C_MAIL_BAT_D(string SALARY_TYPE, string SALARY_DT, string PAY_KIND, string EMAIL_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("Delete from TB_S_C_MAIL_BAT_D ");
            sb.AppendLine("where SALARY_TYPE=@SALARY_TYPE and SALARY_DT=@SALARY_DT and PAY_KIND=@PAY_KIND and SEND_DT=@SEND_DT");
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@SEND_DT", EMAIL_DT);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public void Add_TB_S_C_MAIL_BAT_H(string EMAIL_DT,string TITLE,string CONTENT,string SALARY_DT,string SALARY_TYPE,string PAY_KIND)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("insert into TB_S_C_MAIL_BAT_H(SEND_DT,MAIL_TITLE,MAIL_DESC,SALARY_DT,SALARY_TYPE,PAY_KIND,SENDTO_MAIL,CREATED_BY,CREATED_DT,FUNC_ID) ");
            sb.AppendLine("values(@SEND_DT,@MAIL_TITLE,@MAIL_DESC,@SALARY_DT,@SALARY_TYPE,@PAY_KIND,@SENDTO_MAIL,@CREATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@SEND_DT", EMAIL_DT);
            ht.Add("@MAIL_TITLE", TITLE);
            ht.Add("@MAIL_DESC", CONTENT + "註記：若您對薪資明細資料有任何疑問，請逕洽人事室勞務管理G薪資擔當查詢。");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@SENDTO_MAIL", "系統管理者");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC270");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public void Add_TB_S_C_MAIL_BAT_D(string EMAIL_DT, string SALARY_DT, string SALARY_TYPE, string PAY_KIND, string EMP_ID, string SALARY_EMAIL)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("insert into TB_S_C_MAIL_BAT_D(SEND_DT,SALARY_DT,SALARY_TYPE,PAY_KIND,EMP_ID,EMAIL,MAIL_YN,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.AppendLine("values(@SEND_DT,@SALARY_DT,@SALARY_TYPE,@PAY_KIND,@EMP_ID,@EMAIL,@MAIL_YN,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@SEND_DT", EMAIL_DT);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMAIL", SALARY_EMAIL);
            ht.Add("@MAIL_YN", "N");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC270");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public void Update_TB_S_M_SALARY_PAY_H(string EMAIL_DT, string PAY_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("update TB_S_M_SALARY_PAY_H set IS_EMAIL=@IS_EMAIL,EMAIL_DT=@EMAIL_DT");
            sb.AppendLine("     ,CREATED_BY=@CREATED_BY,CREATED_DT=GETDATE(),UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine("where PAY_ID=@PAY_ID");
            ht.Add("@IS_EMAIL", "Y");
            ht.Add("@EMAIL_DT", EMAIL_DT);
            ht.Add("@PAY_ID", PAY_ID);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC270");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
}