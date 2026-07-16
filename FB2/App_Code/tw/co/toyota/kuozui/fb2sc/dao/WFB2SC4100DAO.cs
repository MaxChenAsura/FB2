using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

public class WFB2SC4100DtlDAO
{
    public string PAY_ID;
    public DateTime PAY_DT;
    public DateTime SALARY_DT;
    public string SALARY_YM;
    public string SALARY_TYPE;
    public decimal? DATA_CNT;
    public DateTime REMIT_DT;
    public string EMP_NAME;
    public string DEPT_NAME_40;
    public string SALARY_ACCOUNT_NO;
    public string SALARY_EMAIL;
    public string TITLE;
    public string SALARY_PAY_METHOD;
    public string SALARY_PAY_METHOD_Value;
}

/// <summary>
/// WFB2SC4100DAO 的摘要描述
/// </summary>
public class WFB2SC4100DL : BaseDAO
{
    public WFB2SC4100DL()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public int GetGridDataCount(int startRowIndex, int maximumRows, string isSuper, string SALARY_YM, string SALARY_DT_S, string SALARY_DT_E, string QryEMP_ID, string EMP_NAME, string DEPT_NO, string EMP_CHG_CD)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select count('1') total_record ");
        sb.AppendLine(" from (");
        sb.AppendLine(" SELECT distinct t1.SALARY_TYPE                                                       ");
        sb.AppendLine("                ,p.SUB_DESC as DESC1                                                  ");
        sb.AppendLine("                ,t1.SALARY_YM                                                         ");
        sb.AppendLine("                ,t1.SALARY_DT                                                         ");
        sb.AppendLine("                ,t2.EMP_ID                                                            ");
        sb.AppendLine("                ,t2.EMP_NAME                                                          ");
        sb.AppendLine("                ,t1.REMIT_DT  														 ");
        sb.AppendLine("                ,t1.PAY_KIND  														 ");
        sb.AppendLine("                ,CASE WHEN t1.PAY_KIND ='9999' then '9999-月薪資' else t1.PAY_KIND+'-'+ s.SALARY_NAME end as PAY_KIND_DESC ");
        sb.AppendLine("                ,emp.SALARY_EMAIL														 ");
        //sb.AppendLine(" FROM   TB_S_M_SALARY_PAY_H t1 														 ");
        //TERRY MODIFY :20160118 修正沒有領先發期滿金的正社員，卻出現該筆資料
        sb.AppendLine("              FROM   TB_S_M_SALARY_PAY t2 														 ");
        sb.AppendLine("              left join TB_S_M_SALARY_PAY_H t1 on t1.SALARY_DT = t2.SALARY_DT and t1.PAY_KIND = t2.PAY_KIND		 ");
        //END
        sb.AppendLine("              left join TB_S_M_SALARY_ITEM s on  t1.PAY_KIND =s.SALARY_ID                           ");
        sb.AppendLine("              left join TB_S_M_SALARY_CAL_H t3 on t1.SALARY_TYPE = t3.SALARY_TYPE ");
        sb.AppendLine("                                            and t1.SALARY_DT = t3.SALARY_DT and t1.PAY_KIND = t3.PAY_KIND ");
        //sb.AppendLine(" left join TB_S_M_SALARY_PAY t2 on t1.SALARY_TYPE = t2.SALARY_TYPE                    ");
        //sb.AppendLine("                               and t1.SALARY_DT = t2.SALARY_DT						 ");
        sb.AppendLine(" left join TB_H_M_EMP emp on emp.EMP_ID = t2.EMP_ID                    ");
        sb.AppendLine("                               and emp.COMPANY_CD = 'K' and isnull(emp.JPN_CD,'')=''			 ");
        sb.AppendLine(" left join TB_9_M_COMM_D p on p.SYS_CD='SC'                                           ");
        sb.AppendLine("                          and p.MAIN_CD='SALARY_TYPE'                                 ");
        sb.AppendLine("                          and  t1.SALARY_TYPE = p.SUB_CD 							 ");
        sb.AppendLine(" left join VW_H_EMP_DATA vm on t2.EMP_ID = vm.EMP_ID                                  ");
        sb.AppendLine("                           and vm.COMPANY_CD = 'K'                                    ");
        sb.AppendLine("                           and vm.JPN_CD=''  --聘用單位(會社別) =K國瑞 且日籍會社 ='' ");
       // sb.AppendLine(" join TB_S_M_SALARY_CAL_H tsmsch on t1.SALARY_DT=tsmsch.SALARY_DT and t1.SALARY_TYPE=tsmsch.SALARY_TYPE and t1.PAY_KIND=tsmsch.PAY_KIND ");
        sb.AppendLine(" WHERE t3.SALARY_CLOSED ='Y' and t2.SALARY_DT  <= getdate()                            ");

        //若登入者帳號取得 大分類代號<>'SUPER',  則  加入條件 ==>  and t2.COMPANY_CD ='K'  and vm.JPN_CD=空白 and t2.EMP_ID = '登入者工號'. 
        //若不為特殊權限者,只能查詢自己的資料(且只有KZ籍/本國籍才看的到)
        //if (isSuper.ToUpper() == "N")
        if (SessionHandle.Current.is_super != "Y")
        {
            sb.AppendLine("   and vm.COMPANY_CD ='K' ");
            sb.AppendLine("   and isnull(vm.JPN_CD,'')='' ");
            sb.AppendLine("   and t2.EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
        }

        //A.若薪資年月<>'' ==>  and t1.SALARY_YM =畫面.薪資年月. 
        if (!string.IsNullOrEmpty(SALARY_YM))
        {
            if (SALARY_YM.Split('/').Length == 2)
            {
                sb.AppendLine("   and t1.SALARY_YM =@SALARY_YM ");
                ht.Add("@SALARY_YM", SALARY_YM.Split('/')[0] + SALARY_YM.Split('/')[1].PadLeft(2, '0'));
            }
        }

        //B.若發薪日期起<>'' ==>  and t1.SALARY_DT >= 畫面.發薪日期起;
        if (!string.IsNullOrEmpty(SALARY_DT_S))
        {
            sb.AppendLine("   and t1.SALARY_DT >=@SALARY_DT_S ");
            ht.Add("@SALARY_DT_S", SALARY_DT_S);
        }

        //C.若發薪日期迄<>'' ==>  and t1.SALARY_DT <=畫面.發薪日期迄. 
        if (!string.IsNullOrEmpty(SALARY_DT_E))
        {
            sb.AppendLine("   and t1.SALARY_DT <=@SALARY_DT_E ");
            ht.Add("@SALARY_DT_E", SALARY_DT_E);
        }

        //D.若工號<>'' ==>  and t2.EMP_ID like '畫面.工號%'. 
        if (!string.IsNullOrEmpty(QryEMP_ID))
        {
            sb.AppendLine("   and t2.EMP_ID like @QryEMP_ID +'%' ");
            ht.Add("@QryEMP_ID", QryEMP_ID);
        }

        //E.若員工姓名<>'' ==>  and t2.EMP_NAME like '畫面.員工姓名%'. 
        if (!string.IsNullOrEmpty(EMP_NAME))
        {
            sb.AppendLine("   and t2.EMP_NAME like @EMP_NAME +'%' ");
            ht.Add("@EMP_NAME", EMP_NAME);
        }

        //F.若部門代號<>''  ==>  and vm.DEPT_NO like '畫面.部門代號%'. 
        if (!string.IsNullOrEmpty(DEPT_NO))
        {
            sb.AppendLine("   and vm.DEPT_NO like @DEPT_NO +'%' ");
            ht.Add("@DEPT_NO", DEPT_NO);
        }
        //G.若在職區分<>''  ==>  and vm.EMP_CHG_CD =畫面.在職區分'. 
        if (EMP_CHG_CD != Resources.Resource.wfb2sc_dll_PlaceChoice)
        {
            sb.AppendLine("   and vm.EMP_CHG_CD=@EMP_CHG_CD ");
            ht.Add("@EMP_CHG_CD", EMP_CHG_CD);
        }
        ht.Add("@startRowIndex", startRowIndex);
        ht.Add("@maximumRows", maximumRows);

        sb.AppendLine(" ) A ");

        Int32 ReturnValue = Convert.ToInt32(dbConn.Query(sb, ht).Rows[0]["total_record"]);
        return ReturnValue;
    }

    public DataTable GetGridData(int startRowIndex, int maximumRows
        , string isSuper, string SALARY_YM, string SALARY_DT_S, string SALARY_DT_E, string QryEMP_ID, string EMP_NAME, string DEPT_NO, string EMP_CHG_CD, string sortExpression)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select * ");
        sb.AppendLine(" from (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber ,* ");
        sb.AppendLine("      from (SELECT distinct t1.SALARY_TYPE                                                       ");
        sb.AppendLine("                             ,p.SUB_DESC as DESC1                                                  ");
        sb.AppendLine("                             ,t1.SALARY_YM                                                         ");
        sb.AppendLine("                             ,t1.SALARY_DT                                                         ");
        sb.AppendLine("                             ,t2.EMP_ID                                                            ");
        sb.AppendLine("                             ,t2.EMP_NAME                                                          ");
        sb.AppendLine("                             ,t1.REMIT_DT  														 ");
        sb.AppendLine("                             ,t1.PAY_KIND  														 ");
        sb.AppendLine("                             ,CASE WHEN t1.PAY_KIND ='9999' then '9999-月薪資' else t1.PAY_KIND+'-'+ s.SALARY_NAME end as PAY_KIND_DESC ");
        sb.AppendLine("                             ,vm.SALARY_EMAIL														 ");
        //sb.AppendLine("              FROM   TB_S_M_SALARY_PAY_H t1 														 ");
        //TERRY MODIFY :20160118 修正沒有領先發期滿金的正社員，卻出現該筆資料
        sb.AppendLine("              FROM   TB_S_M_SALARY_PAY t2 														 ");
        sb.AppendLine("              left join TB_S_M_SALARY_PAY_H t1 on t1.SALARY_DT = t2.SALARY_DT and t1.PAY_KIND = t2.PAY_KIND		 ");
        //END
        sb.AppendLine("              left join TB_S_M_SALARY_ITEM s on  t1.PAY_KIND =s.SALARY_ID                           ");
        sb.AppendLine("              left join TB_S_M_SALARY_CAL_H t3 on t1.SALARY_TYPE = t3.SALARY_TYPE ");
        sb.AppendLine("                                            and t1.SALARY_DT = t3.SALARY_DT and t1.PAY_KIND = t3.PAY_KIND ");
        //sb.AppendLine("              left join TB_S_M_SALARY_PAY t2 on t1.SALARY_TYPE = t2.SALARY_TYPE                    ");
        //sb.AppendLine("                                            and t1.SALARY_DT = t2.SALARY_DT						 ");
        sb.AppendLine("              left join TB_9_M_COMM_D p on p.SYS_CD='SC'                                           ");
        sb.AppendLine("                                       and p.MAIN_CD='SALARY_TYPE'                                 ");
        sb.AppendLine("                                       and  t1.SALARY_TYPE = p.SUB_CD 							 ");
        sb.AppendLine("              left join VW_H_EMP_DATA vm on t2.EMP_ID = vm.EMP_ID                                  ");
        sb.AppendLine("                                        and vm.COMPANY_CD = 'K'                                    ");
        sb.AppendLine("                                        and isnull(vm.JPN_CD,'') =''  --聘用單位(會社別) =K國瑞 且日籍會社 ='' ");
        //sb.AppendLine(" join TB_S_M_SALARY_CAL_H tsmsch on t1.SALARY_DT=tsmsch.SALARY_DT and t1.SALARY_TYPE=tsmsch.SALARY_TYPE and t1.PAY_KIND=tsmsch.PAY_KIND ");
        sb.AppendLine("              WHERE t3.SALARY_CLOSED ='Y' and t2.SALARY_DT  <= getdate()                            ");

        //若登入者帳號取得 大分類代號<>'SUPER',  則  加入條件 ==>  and t2.COMPANY_CD ='K'  and vm.JPN_CD=空白 and t2.EMP_ID = '登入者工號'. 
        //若不為特殊權限者,只能查詢自己的資料(且只有KZ籍/本國籍才看的到)
        //if (isSuper.ToUpper() == "N")
        if (SessionHandle.Current.is_super != "Y")
        {
            sb.AppendLine("   and vm.COMPANY_CD ='K' ");
            sb.AppendLine("   and isnull(vm.JPN_CD,'')='' ");
            sb.AppendLine("   and t2.EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
        }

        //A.若薪資年月<>'' ==>  and t1.SALARY_YM =畫面.薪資年月. 
        if (!string.IsNullOrEmpty(SALARY_YM))
        {
            if (SALARY_YM.Split('/').Length == 2)
            {
                sb.AppendLine("   and t1.SALARY_YM =@SALARY_YM ");
                ht.Add("@SALARY_YM", SALARY_YM.Split('/')[0] + SALARY_YM.Split('/')[1].PadLeft(2, '0'));
            }
        }

        //B.若發薪日期起<>'' ==>  and t1.SALARY_DT >= 畫面.發薪日期起;
        if (!string.IsNullOrEmpty(SALARY_DT_S))
        {
            sb.AppendLine("   and t1.SALARY_DT >=@SALARY_DT_S ");
            ht.Add("@SALARY_DT_S", SALARY_DT_S);
        }

        //C.若發薪日期迄<>'' ==>  and t1.SALARY_DT <=畫面.發薪日期迄. 
        if (!string.IsNullOrEmpty(SALARY_DT_E))
        {
            sb.AppendLine("   and t1.SALARY_DT <=@SALARY_DT_E ");
            ht.Add("@SALARY_DT_E", SALARY_DT_E);
        }

        //D.若工號<>'' ==>  and t2.EMP_ID like '畫面.工號%'. 
        if (!string.IsNullOrEmpty(QryEMP_ID))
        {
            sb.AppendLine("   and t2.EMP_ID like @QryEMP_ID +'%' ");
            ht.Add("@QryEMP_ID", QryEMP_ID);
        }

        //E.若員工姓名<>'' ==>  and t2.EMP_NAME like '畫面.員工姓名%'. 
        if (!string.IsNullOrEmpty(EMP_NAME))
        {
            sb.AppendLine("   and t2.EMP_NAME like @EMP_NAME +'%' ");
            ht.Add("@EMP_NAME", EMP_NAME);
        }

        //F.若部門代號<>''  ==>  and vm.DEPT_NO like '畫面.部門代號%'. 
        if (!string.IsNullOrEmpty(DEPT_NO))
        {
            sb.AppendLine("   and vm.DEPT_NO like @DEPT_NO +'%' ");
            ht.Add("@DEPT_NO", DEPT_NO);
        }
        //G.若在職區分<>''  ==>  and vm.EMP_CHG_CD =畫面.在職區分'. 
        if (EMP_CHG_CD != Resources.Resource.wfb2sc_dll_PlaceChoice)
        {
            sb.AppendLine("   and vm.EMP_CHG_CD=@EMP_CHG_CD ");
            ht.Add("@EMP_CHG_CD", EMP_CHG_CD);
        }
        sb.AppendLine("  ) A ");
        sb.AppendLine(" ) TDMCH where RowNumber between CAST(@startRowIndex+1 as varchar) ");
        sb.AppendLine("                     AND CAST(@startRowIndex+@maximumRows as varchar)");

        ht.Add("@startRowIndex", startRowIndex);
        ht.Add("@maximumRows", maximumRows);

        DataTable returnDt = dbConn.Query(sb, ht);

        return returnDt;
    }

    public WFB2SC4100DtlDAO GetDetailHeaderByTypeA(DateTime SALARY_DT, string SALARY_TYPE, string EMP_ID, string PAY_KIND)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" SELECT t1.PAY_ID                                                                  ");
        sb.AppendLine("       ,t1.PAY_DT                                                                  ");
        sb.AppendLine("       ,t1.SALARY_DT                                                               ");
        sb.AppendLine("       ,t1.SALARY_YM                                                               ");
        sb.AppendLine("       ,t1.SALARY_TYPE                                                             ");
        sb.AppendLine("       ,t1.DATA_CNT                                                                ");
        sb.AppendLine("       ,t1.REMIT_DT                                                                ");
        sb.AppendLine("       ,t2.EMP_NAME                                                                ");
        sb.AppendLine("       ,t2.DEPT_NAME_40                                                            ");
        sb.AppendLine("       ,t2.SALARY_ACCOUNT_NO                                                       ");
        sb.AppendLine("       ,t2.SALARY_EMAIL                                                            ");
        sb.AppendLine("      /* ,d.REMARK as TITLE */   ,isnull(d.SUB_DESC,'') as TITLE                                                      ");
        //202103 ADD
        sb.AppendLine("       ,isnull(t2.SALARY_PAY_METHOD,'') as SALARY_PAY_METHOD,   isnull(e.SUB_DESC,'') as SALARY_PAY_METHOD_Value                                           ");
        sb.AppendLine(" FROM TB_S_M_SALARY_PAY_H t1                                                       ");
        sb.AppendLine(" left join TB_S_M_EMP_RESULT t2 on t1.SALARY_YM = t2.SALARY_YM and  t1.SALARY_DT = t2.SALARY_DT  ");
       // sb.AppendLine(" left join TB_9_M_PARAMETER d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_BANK_ID'   ");
        sb.AppendLine(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_BANK_ID'  and t2.SALARY_ACCOUNT_BANK = d.SUB_CD ");
        //202103 ADD
        sb.AppendLine(" left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='SALARY_PAY_METHOD'  and t2.SALARY_PAY_METHOD = e.SUB_CD ");
        sb.AppendLine(" where t1.SALARY_DT = @SALARY_DT                                                   ");
        sb.AppendLine("   and t1.SALARY_TYPE = @SALARY_TYPE                                               ");
        sb.AppendLine("   and t2.EMP_ID = @EMP_ID                                                         ");
        sb.AppendLine("   and t1.PAY_KIND = @PAY_KIND                                                     ");

        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@PAY_KIND", PAY_KIND);
        DataTable returnDT = dbConn.Query(sb, ht);
        if (returnDT.Rows.Count > 0)
        {
            return (from item in returnDT.AsEnumerable()
                    select new WFB2SC4100DtlDAO
                    {
                        PAY_ID = (item.Table.Columns.Contains("PAY_ID") ? item.Field<string>("PAY_ID") : null),
                        PAY_DT = item.Field<DateTime>("PAY_DT"),
                        SALARY_DT = item.Field<DateTime>("SALARY_DT"),
                        SALARY_YM = (item.Table.Columns.Contains("SALARY_YM") ? item.Field<string>("SALARY_YM") : null),
                        SALARY_TYPE = (item.Table.Columns.Contains("SALARY_TYPE") ? item.Field<string>("SALARY_TYPE") : null),
                        DATA_CNT = (item.Table.Columns.Contains("DATA_CNT") ? item.Field<decimal?>("DATA_CNT") : null),
                        REMIT_DT = item.Field<DateTime>("REMIT_DT"),
                        EMP_NAME = (item.Table.Columns.Contains("EMP_NAME") ? item.Field<string>("EMP_NAME") : null),
                        DEPT_NAME_40 = (item.Table.Columns.Contains("DEPT_NAME_40") ? item.Field<string>("DEPT_NAME_40") : null),
                        SALARY_ACCOUNT_NO = (item.Table.Columns.Contains("SALARY_ACCOUNT_NO") ? item.Field<string>("SALARY_ACCOUNT_NO") : null),
                        SALARY_EMAIL = (item.Table.Columns.Contains("SALARY_EMAIL") ? item.Field<string>("SALARY_EMAIL") : null),
                        TITLE = (item.Table.Columns.Contains("TITLE") ? item.Field<string>("TITLE") : null),
                        SALARY_PAY_METHOD = (item.Table.Columns.Contains("SALARY_PAY_METHOD") ? item.Field<string>("SALARY_PAY_METHOD") : null),
                        SALARY_PAY_METHOD_Value = (item.Table.Columns.Contains("SALARY_PAY_METHOD_Value") ? item.Field<string>("SALARY_PAY_METHOD_Value") : null)
                    }).ToList().First();
        }
        else
            return null;

    }

    public WFB2SC4100DtlDAO GetDetailHeaderByTypeNotA(DateTime SALARY_DT, string SALARY_TYPE, string EMP_ID, string PAY_KIND)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" SELECT t1.PAY_ID                                                                  ");
        sb.AppendLine("       ,t1.PAY_DT                                                                  ");
        sb.AppendLine("       ,t1.SALARY_DT                                                               ");
        sb.AppendLine("       ,t1.SALARY_YM                                                               ");
        sb.AppendLine("       ,t1.SALARY_TYPE                                                             ");
        sb.AppendLine("       ,t1.DATA_CNT                                                                ");
        sb.AppendLine("       ,t1.REMIT_DT                                                                ");
        sb.AppendLine("       ,t2.EMP_NAME                                                                ");
        sb.AppendLine("       ,t2.DEPT_NAME_40                                                            ");
        sb.AppendLine("       ,t3.SALARY_ACCOUNT_NO                                                       ");
        sb.AppendLine("       ,t2.SALARY_EMAIL                                                            ");
        sb.AppendLine("       /* ,d.REMARK as TITLE */   ,isnull(d.SUB_DESC,'') as TITLE                                                              ");
        //202103 ADD
        sb.AppendLine("      ,isnull(t2.SALARY_PAY_METHOD,'') as SALARY_PAY_METHOD,   isnull(e.SUB_DESC,'') as SALARY_PAY_METHOD_Value                                                       ");
        sb.AppendLine(" FROM TB_S_M_SALARY_PAY_H t1                                                       ");
        sb.AppendLine(" left join TB_S_M_EMP_RESULT_TMP t2 on t1.SALARY_DT = t2.SALARY_DT                 ");
        sb.AppendLine("                                  and  t1.SALARY_TYPE = t2.SALARY_TYPE             ");
        sb.AppendLine("                                   and t1.PAY_KIND = t2.PAY_KIND                   ");
        sb.AppendLine(" left join  VW_H_EMP_DATA t3  on t2.EMP_ID =t3.EMP_ID                              ");
        //sb.AppendLine(" left join TB_9_M_PARAMETER d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_BANK_ID'   ");
        sb.AppendLine(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_BANK_ID'  and t2.SALARY_ACCOUNT_BANK = d.SUB_CD ");
        //202103 ADD
        sb.AppendLine(" left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='SALARY_PAY_METHOD'  and t2.SALARY_PAY_METHOD = e.SUB_CD ");
        sb.AppendLine(" where t1.SALARY_DT = @SALARY_DT                                                   ");
        sb.AppendLine("   and t1.SALARY_TYPE = @SALARY_TYPE                                               ");
        sb.AppendLine("   and t2.EMP_ID = @EMP_ID                                                         ");
        sb.AppendLine("   and t1.PAY_KIND = @PAY_KIND                                                     ");

        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@PAY_KIND", PAY_KIND);
        DataTable returnDT = dbConn.Query(sb, ht);
        if (returnDT.Rows.Count > 0)
        {
            return (from item in returnDT.AsEnumerable()
                    select new WFB2SC4100DtlDAO
                    {
                        PAY_ID = (item.Table.Columns.Contains("PAY_ID") ? item.Field<string>("PAY_ID") : null),
                        PAY_DT = item.Field<DateTime>("PAY_DT"),
                        SALARY_DT = item.Field<DateTime>("SALARY_DT"),
                        SALARY_YM = (item.Table.Columns.Contains("SALARY_YM") ? item.Field<string>("SALARY_YM") : null),
                        SALARY_TYPE = (item.Table.Columns.Contains("SALARY_TYPE") ? item.Field<string>("SALARY_TYPE") : null),
                        DATA_CNT = (item.Table.Columns.Contains("DATA_CNT") ? item.Field<decimal?>("DATA_CNT") : null),
                        REMIT_DT = item.Field<DateTime>("REMIT_DT"),
                        EMP_NAME = (item.Table.Columns.Contains("EMP_NAME") ? item.Field<string>("EMP_NAME") : null),
                        DEPT_NAME_40 = (item.Table.Columns.Contains("DEPT_NAME_40") ? item.Field<string>("DEPT_NAME_40") : null),
                        SALARY_ACCOUNT_NO = (item.Table.Columns.Contains("SALARY_ACCOUNT_NO") ? item.Field<string>("SALARY_ACCOUNT_NO") : null),
                        SALARY_EMAIL = (item.Table.Columns.Contains("SALARY_EMAIL") ? item.Field<string>("SALARY_EMAIL") : null),
                        TITLE = (item.Table.Columns.Contains("TITLE") ? item.Field<string>("TITLE") : null),
                        SALARY_PAY_METHOD = (item.Table.Columns.Contains("SALARY_PAY_METHOD") ? item.Field<string>("SALARY_PAY_METHOD") : null),
                        SALARY_PAY_METHOD_Value = (item.Table.Columns.Contains("SALARY_PAY_METHOD_Value") ? item.Field<string>("SALARY_PAY_METHOD_Value") : null)
                    }).ToList().First();
        }
        else
            return null;

    }

    public DataTable Get2B(string EMP_ID, DateTime SALARY_DT, string SALARY_TYPE, string PAY_KIND)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(@" declare @seq int ;
                         select @seq = order_seq from TB_H_M_LEVEL where LEVEL_CD = '2S'; ");

        sb.AppendLine(" SELECT t1.SALARY_DT                                    ");        
        sb.AppendLine("       ,t1.EMP_ID                                               ");
        sb.AppendLine("       ,t1.SALARY_ID                                            ");
        sb.AppendLine("       ,t1.SALARY_NAME                                          "); 
        sb.AppendLine(@"       ,IIF(d.order_seq <= @seq, 'Y', 'N') as is2B              ");
        if (PAY_KIND == "1031")
        {
            sb.AppendLine(@"   ,IIF(t1.SALARY_ID='1031' and (e.AWARD_ROUND='2' or e.AWARD_ROUND='3'),'Y','N' ) as is1031  ");
        }
        else
        {
            sb.AppendLine(@"   ,'N' as is1031  ");
        }
        sb.AppendLine("         from TB_S_M_SALARY_PAY t1                                      ");        
        sb.AppendLine(@"       left join VW_H_EMP_DATA c on t1.EMP_ID = c.EMP_ID
                               left join TB_H_M_LEVEL d on c.level_cd  = d.level_cd   and d.END_DT = '9999/12/31' ");
        if (PAY_KIND == "1031")
        {
            sb.AppendLine(@"   left join TB_S_M_AWARD_H e on e.SALARY_DT = t1.SALARY_DT   ");
        }
        sb.AppendLine("         where t1.SALARY_DT = @SALARY_DT                                ");
        sb.AppendLine("         and t1.SALARY_TYPE =@SALARY_TYPE                             ");
        sb.AppendLine("         and t1.EMP_ID = @EMP_ID                                      ");
        sb.AppendLine("         and t1.SALARY_ID = @PAY_KIND                                  ");

        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@PAY_KIND", PAY_KIND);
        return dbConn.Query(sb, ht);
    }

    public DataTable GetShouldBeAddedData(DateTime SALARY_DT, string SALARY_TYPE, string EMP_ID, string PAY_KIND, string IS_TAX, int IS_PLUS)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();



        //new
        sb.AppendLine(@" declare @seq int ;
                         select @seq = order_seq from TB_H_M_LEVEL where LEVEL_CD = '2S'; ");        

        sb.AppendLine(@" select SALARY_DT,DATA_YM,SALARY_TYPE,EMP_ID,SALARY_ID,AMOUNT,IS_PLUS,IS_TAX,PAY_KIND,
                           PAY_TYPE,PAY_DT,PAY_ID,is2B,is1031
	                       ,case when SALARY_NAME = '職能俸' and is2B = 'Y' and is1031 = 'Y' then '本薪(C2)' 
	                        when SALARY_NAME = '職能俸' and is2B = 'Y' and is1031 = 'N' then '本薪' 
	                        when SALARY_NAME = '專業俸' then '專業津貼'   
		                    when SALARY_NAME = '職務俸' then '職務津貼'  
		                    else SALARY_NAME end   as SALARY_NAME
	                        from (  ");
        //
        sb.AppendLine(" SELECT t1.SALARY_DT                                    ");
        sb.AppendLine("       ,t1.DATA_YM                                              ");
        sb.AppendLine("       ,t1.SALARY_TYPE                                          ");
        sb.AppendLine("       ,t1.EMP_ID                                               ");
        sb.AppendLine("       ,t1.SALARY_ID                                            ");
        sb.AppendLine("       ,t1.SALARY_NAME                                          ");
        sb.AppendLine("       ,t1.AMOUNT                                               ");
        sb.AppendLine("       ,t1.IS_PLUS                                              ");
        sb.AppendLine("       ,t1.IS_TAX                                               ");
        sb.AppendLine("       ,t1.PAY_KIND                                             ");
        sb.AppendLine("       ,t1.PAY_TYPE                                             ");
        sb.AppendLine("       ,t1.PAY_DT                                               ");
        sb.AppendLine("       ,t1.PAY_ID                                               ");
        //new
        sb.AppendLine(@"       ,d.order_seq
	                           ,@seq seq
	                           ,IIF(d.order_seq <= @seq, 'Y', 'N') as is2B              ");

        if (PAY_KIND == "1031")
        {
            sb.AppendLine(@"   ,IIF(t1.SALARY_ID='1031' and (e.AWARD_ROUND='2' or e.AWARD_ROUND='3'),'Y','N' ) as is1031  ");
        }
        else {
            sb.AppendLine(@"   ,'N' as is1031  ");
        }
        //

        sb.AppendLine(" from TB_S_M_SALARY_PAY t1                                      ");
        sb.AppendLine(" left join TB_S_M_SALARY_ITEM t2 on t1.SALARY_ID = t2.SALARY_ID ");
        //new
        sb.AppendLine(@" left join VW_H_EMP_DATA c on t1.EMP_ID = c.EMP_ID
                         left join TB_H_M_LEVEL d on c.level_cd  = d.level_cd   and d.END_DT = '9999/12/31' ");
        if (PAY_KIND == "1031")
        {
            sb.AppendLine(@"   left join TB_S_M_AWARD_H e on e.SALARY_DT = t1.SALARY_DT   ");
        }
        //
        sb.AppendLine(" where t1.SALARY_DT = @SALARY_DT                                ");
        sb.AppendLine("   and t1.SALARY_TYPE =@SALARY_TYPE                             ");
        sb.AppendLine("   and t1.EMP_ID = @EMP_ID                                      ");
        sb.AppendLine("   and t1.PAY_KIND = @PAY_KIND                                  ");
        sb.AppendLine("   and t1.IS_TAX =@IS_TAX   ");
        sb.AppendLine("   and t1.IS_PLUS = @IS_PLUS   ");
        sb.AppendLine("   and t1.AMOUNT >0   ");
        sb.AppendLine("   and t1.PAY_TYPE<>'G'  ");
        sb.AppendLine("   and t2.PAY_OBJECT ='E' ");
        sb.AppendLine("   )a ");
        
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@PAY_KIND", PAY_KIND);
        ht.Add("@IS_TAX", IS_TAX);
        ht.Add("@IS_PLUS", IS_PLUS);
        return dbConn.Query(sb, ht);
    }

    public DataTable GetOverTime(string EMP_ID, DateTime SALARY_DT)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //薪資用加班月結明細檔
        sb.AppendLine("   SELECT t2.SALARY_DT  ");
        sb.AppendLine("         ,t2.EMP_ID  ");
        sb.AppendLine("         ,t2.OVERTIME_PAY_TYPE  ");
        sb.AppendLine("         ,t2.TOTAL_HOURS as DESC2   ");
        //sb.AppendLine("         ,t2.OVERTIME_PAY_TYPE +'-'+ d.SUB_DESC as DESC1    ");
        sb.AppendLine("         , d.SUB_DESC as DESC1    ");
        sb.AppendLine("     from TB_S_M_OVERTIME_RESULT_D t2 		  ");
        sb.AppendLine("     left join TB_9_M_COMM_D d on  d.SYS_CD ='SC'   ");
        sb.AppendLine("                              and  d.MAIN_CD='OVERTIME_PAY_TYPE'   ");
        sb.AppendLine("                              and  t2.OVERTIME_PAY_TYPE = d.SUB_CD    ");
        sb.AppendLine("     where 1=1 and t2.SALARY_DT = @SALARY_DT   ");
        sb.AppendLine("       and t2.EMP_ID = @EMP_ID;		  ");

        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        return dbConn.Query(sb, ht);
    }

    public DataTable GetLeave(string EMP_ID, DateTime SALARY_DT)
    { //薪資用請假月結明細檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" SELECT t2.EMP_ID ");
        sb.AppendLine("       ,t2.SUB_LEAVE_CD ");
        //sb.AppendLine("       ,t2.TOTAL_HOURS as DESC2  ");
        sb.AppendLine("       ,t2.LEAVE_PAY_RATE ");
        sb.AppendLine("       ,t2.LEAVE_CNT_UNIT ");
        sb.AppendLine("       ,t1.SUB_LEAVE_DESC    ");
        //sb.AppendLine("       ,d.SUB_DESC as DESC1   ");
        sb.AppendLine("  ,case when t2.LEAVE_CNT_UNIT ='D' then convert (varchar(8),cast(Round(t2.TOTAL_HOURS/8,2) as numeric(5,2)))+'天' ");
        sb.AppendLine("        when t2.LEAVE_CNT_UNIT ='T' then convert(varchar(8),t2.TOTAL_HOURS)+'次' ");
        sb.AppendLine("        else  convert(varchar(8),t2.TOTAL_HOURS)+'時' end as DESC2 ");
        sb.AppendLine("       ,t2.LEAVE_CNT_UNIT +'-'+ d.SUB_DESC as DESC1 ");       
        sb.AppendLine(" from TB_S_M_LEAVE_RESULT_D t2     ");
        sb.AppendLine(" left join TB_D_M_LEAVE_TYPE_D t1 on t2.SUB_LEAVE_CD = t1.SUB_LEAVE_CD     ");
        sb.AppendLine(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC'  ");
        sb.AppendLine("                          and  d.MAIN_CD='LEAVE_CNT_UNIT' ");
        sb.AppendLine("                          and  t2.LEAVE_CNT_UNIT = d.SUB_CD 	 ");
        sb.AppendLine(" where t2.SALARY_DT = @SALARY_DT ");
        sb.AppendLine("   and t2.EMP_ID = @EMP_ID ");
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        return dbConn.Query(sb, ht);
    }

    public DataTable GetWorkShift(string EMP_ID, DateTime SALARY_DT)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //薪資用輪班津貼月結明細檔
        sb.AppendLine(" SELECT t2.SALARY_DT ");
        sb.AppendLine("       ,t2.EMP_ID ");
        sb.AppendLine("       ,t2.WORK_SHIFT_ALLOWANCE_TYPE ");
        sb.AppendLine("       ,t2.TOTAL_DAYS as DESC2  ");
       // sb.AppendLine("       ,t2.WORK_SHIFT_ALLOWANCE_TYPE +'-'+ d.SUB_DESC as DESC1   ");
        sb.AppendLine("       , d.SUB_DESC as DESC1   ");
        sb.AppendLine(" from TB_S_M_WORK_SHIFT_ALLOWANCE_D t2 		 ");
        sb.AppendLine(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC'  ");
        sb.AppendLine("                          and  d.MAIN_CD='WORK_SHIFT_ALLOWANCE_TYPE'  ");
        sb.AppendLine("                          and  t2.WORK_SHIFT_ALLOWANCE_TYPE = d.SUB_CD ");
        sb.AppendLine(" where t2.SALARY_DT = @SALARY_DT  ");
        sb.AppendLine("   and t2.EMP_ID = @EMP_ID	  ");
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        return dbConn.Query(sb, ht);
    }

    public DataTable GetAvailableLeave(string EMP_ID, DateTime SALARY_DT)
    {   //薪資用可用假剩餘月結明細檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" SELECT t2.SALARY_DT                                                       ");
        sb.AppendLine("       ,t2.EMP_ID                                                          ");
        sb.AppendLine("       ,t2.LEAVE_ALLOWANCE_TYPE                                            ");
        sb.AppendLine("       ,t2.TOTAL_HOURS as DESC2                                            ");
        sb.AppendLine("       ,t2.IS_YN                                                           ");
        sb.AppendLine("       ,t2.DATA_YEAR + d.SUB_DESC as DESC1  			  ");
        sb.AppendLine(" from TB_S_M_AVAILABLE_LEAVE_D t2                                          ");
        sb.AppendLine(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC'                              ");
        sb.AppendLine("                          and  d.MAIN_CD='LEAVE_ALLOWANCE_TYPE'            ");
        sb.AppendLine("                          and  t2.LEAVE_ALLOWANCE_TYPE = d.SUB_CD       	  ");
        sb.AppendLine(" where t2.SALARY_DT = @SALARY_DT 	  ");
        sb.AppendLine("   and t2.EMP_ID = @EMP_ID		  ");
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        return dbConn.Query(sb, ht);
    }

    public DataTable GetPension(string EMP_ID, DateTime SALARY_DT)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" SELECT t1.SALARY_YM                 ");
        sb.AppendLine("       ,t1.EMP_ID                    ");
        sb.AppendLine("       ,t1.INS_TYPE                  ");
        sb.AppendLine("       ,t1.IDENTITY_KIND             ");
        sb.AppendLine("       ,t1.LICENSE_ID                ");
        sb.AppendLine("       ,t1.INS_AMT                   ");
        sb.AppendLine("       ,t1.INS_FEES                  ");
        sb.AppendLine("       ,t1.INS_SUB_AMT               ");
        sb.AppendLine("       ,t1.INS_TOTAL                 ");
        sb.AppendLine("       ,t1.COMP_RATE                 ");
        sb.AppendLine("       ,t1.SLEF_RATE                 ");
        sb.AppendLine("       ,t1.SELF_D_AMT                ");
        sb.AppendLine("       ,t1.IS_YN                     ");
        sb.AppendLine("       ,t1.SALARY_DT                 ");
        sb.AppendLine(" from TB_I_R_FEES_MONTH t1 		    ");
        sb.AppendLine(" where 1=1                           ");
        sb.AppendLine("   and t1.SALARY_DT = @SALARY_DT   ");
        sb.AppendLine("   and t1.EMP_ID = @EMP_ID         ");
        sb.AppendLine("   and t1.INS_TYPE ='C'              ");
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        return dbConn.Query(sb, ht);
    }

    public DataTable GetINS2(string EMP_ID, DateTime SALARY_DT)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" SELECT distinct t1.INS_MONTH_AMOUNT    ");
        sb.AppendLine("                ,t1.ACCU_AMOUNT         ");
        sb.AppendLine("                ,t1.INS_COST_BASE 	   ");
        sb.AppendLine(" FROM TB_S_M_INS2_DETAIL t1			   ");
        sb.AppendLine(" where t1.PAYMENT_DATE = @PAYMENT_DATE  ");
        sb.AppendLine("   and t1.DATA_SOURCE ='A'              ");
        sb.AppendLine("   and t1.EMP_ID =@EMP_ID			   ");

        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@PAYMENT_DATE", SALARY_DT.ToString("yyyy-MM-dd"));
        return dbConn.Query(sb, ht);

    }

    public DataTable GetItemByTypeC_1031(string EMP_ID, DateTime SALARY_DT)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" SELECT t1.AWARD_ROUND                                            ");
        sb.AppendLine("       ,t1.SCORE_2H                                               ");
        sb.AppendLine("       ,t1.LEVEL_PAY                                              ");
        sb.AppendLine("       ,t1.ABILITY_PAY                                            ");
        sb.AppendLine("       ,t1.PJOB_PAY												 ");
        sb.AppendLine("       ,t1.PROFESSION_PAY                                         ");
        sb.AppendLine("       ,t1.FOOD_SUBSIDY                                           ");
        sb.AppendLine("       ,t1.LEVELUP_FLAG                                           ");
        sb.AppendLine("       ,t1.LEVEL_PAY_BEFORE										 ");
        sb.AppendLine("       ,t1.ABILITY_PAY_BEFORE                                     ");
        sb.AppendLine("       ,t1.PJOB_PAY_BEFORE                                        ");
        sb.AppendLine("       ,t1.PROFESSION_PAY_BEFORE                                  ");
        sb.AppendLine("       ,t1.FOOD_SUBSIDY_BEFORE									 ");
        sb.AppendLine("       ,t1.LEAVE_A_HOUR                                           ");
        sb.AppendLine("       ,t1.LEAVE_B_HOUR                                           ");
        sb.AppendLine("       ,t1.LEAVE_C_HOUR                                           ");
        sb.AppendLine("       ,t1.LEAVE_Q_HOUR                                           ");
        sb.AppendLine("       ,t1.LEAVE_OP_HOUR                                          ");
        sb.AppendLine("       ,t1.THIRD_CNT_P											 ");
        sb.AppendLine("       ,t1.SECOND_CNT_P                                           ");
        sb.AppendLine("       ,t1.FIRST_CNT_P                                            ");
        sb.AppendLine("       ,t1.THIRD_CNT_M                                            ");
        sb.AppendLine("       ,t1.SECOND_CNT_M                                           ");
        sb.AppendLine("       ,t1.FIRST_CNT_M                                            ");
        sb.AppendLine("       ,t1.ATTEND_DAYS 											 ");
        sb.AppendLine(" FROM TB_S_M_AWARD_H t2 											 ");
        sb.AppendLine(" left join TB_S_M_AWARD_D t1 on t1.AWARD_YEAR  = t2.AWARD_YEAR    ");
        sb.AppendLine("                            and t1.AWARD_ROUND = t2.AWARD_ROUND 	 ");
        sb.AppendLine(" where t2.PROCESS_STATUS ='Y'                                     ");
        sb.AppendLine("   and t2.SALARY_DT = @SALARY_DT and t1.EMP_ID = @EMP_ID          ");
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        return dbConn.Query(sb, ht);
    }
    public DataTable GetItemByTypeC_1033(string EMP_ID, DateTime SALARY_DT)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" SELECT t1.LEVEL_PAY                                              ");
        sb.AppendLine("       ,t1.ABILITY_PAY                                            ");
        sb.AppendLine("       ,t1.PJOB_PAY												 ");
        sb.AppendLine("       ,t1.PROFESSION_PAY                                         ");
        sb.AppendLine("       ,t1.FOOD_SUBSIDY                                           ");
        sb.AppendLine("       ,t1.LEAVE_A_HOUR                                           ");
        sb.AppendLine("       ,t1.LEAVE_B_HOUR                                           ");
        sb.AppendLine("       ,t1.LEAVE_C_HOUR                                           ");
        sb.AppendLine("       ,t1.LEAVE_Q_HOUR                                           ");
        sb.AppendLine("       ,t1.LEAVE_OP_HOUR                                          ");
        sb.AppendLine("       ,t1.THIRD_CNT_P											 ");
        sb.AppendLine("       ,t1.SECOND_CNT_P                                           ");
        sb.AppendLine("       ,t1.FIRST_CNT_P                                            ");
        sb.AppendLine("       ,t1.THIRD_CNT_M                                            ");
        sb.AppendLine("       ,t1.SECOND_CNT_M                                           ");
        sb.AppendLine("       ,t1.FIRST_CNT_M                                            ");
        sb.AppendLine("       ,t1.ATTEND_DAYS 											 ");
        sb.AppendLine(" FROM TB_S_M_BONUS_H t2 											 ");
        sb.AppendLine(" left join TB_S_R_BONUS_D t1 on t1.BONUS_YEAR  = t2.BONUS_YEAR    ");
        sb.AppendLine("                            and t1.BONUS_COUNT = t2.BONUS_ROUND 	 ");
        sb.AppendLine(" where t2.PROCESS_STATUS ='Y'                                     ");
        sb.AppendLine("   and t2.SALARY_DT = @SALARY_DT and t1.EMP_ID = @EMP_ID          ");
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        return dbConn.Query(sb, ht);
    }
    public DataTable GetTB_S_M_DUTY_RESULT_H_SDT_EDT(DateTime SALARY_DT)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select DATA_SDT,DATA_EDT   ");
        sb.AppendLine(" from  TB_S_M_DUTY_RESULT_H ");
        sb.AppendLine(" WHERE SALARY_DT=@SALARY_DT ");

        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));

        return dbConn.Query(sb, ht);
    }

    public DataTable GetReSendDataBy1031(string SALARY_TYPE, DateTime SALARY_DT)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine("  SELECT t1.SALARY_DT                                                                      ");
        sb.AppendLine("        ,t1.SALARY_TYPE                                                                    ");
        sb.AppendLine("        ,t1.PAY_KIND                                                                       ");
        sb.AppendLine("        ,h.AWARD_YEAR                                                                      ");
        sb.AppendLine("        ,h.AWARD_ROUND																	   ");
        sb.AppendLine("        ,REPLACE (t3.SUB_DESC, '【資料年】' , convert(varchar(4),h.AWARD_YEAR))  as DESC1  ");
        sb.AppendLine("        ,t2.SUB_DESC as DESC2															   ");
        sb.AppendLine("  FROM TB_S_M_SALARY_CAL_H t1 															   ");
        sb.AppendLine("  left join TB_S_M_AWARD_H h on   t1.SALARY_DT = h.SALARY_DT 							   ");
        sb.AppendLine("  left join TB_9_M_COMM_D t2 on  t2.SYS_CD ='SC'                                           ");
        sb.AppendLine("                            and  t2.MAIN_CD='MAIL_TEXT'                                    ");
        sb.AppendLine("                            and  t2.CODE_VAL1 = t1.PAY_KIND                                ");
        sb.AppendLine("                            and  t2.CODE_VAL2 = h.AWARD_ROUND     						   ");
        sb.AppendLine("  left join TB_9_M_COMM_D t3 on  t3.SYS_CD ='SC'                                           ");
        sb.AppendLine("                            and  t3.MAIN_CD='MAIL_TITLE'                                   ");
        sb.AppendLine("                            and  t3.CODE_VAL1 = t1.PAY_KIND                                ");
        sb.AppendLine("                            and  t3.CODE_VAL2 = h.AWARD_ROUND  							   ");
        sb.AppendLine("  where t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE  AND PAY_KIND='1031'  ");
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        return dbConn.Query(sb, ht);

    }
    //BY EVA ADD 2015/08/12
    public DataTable GetReSendDataBy1033(string SALARY_TYPE, DateTime SALARY_DT)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine("  SELECT t1.SALARY_DT                                                                      ");
        sb.AppendLine("        ,t1.SALARY_TYPE                                                                    ");
        sb.AppendLine("        ,t1.PAY_KIND                                                                       ");
        sb.AppendLine("        ,h.BONUS_YEAR                                                                      ");
        sb.AppendLine("        ,REPLACE (t3.SUB_DESC, '【資料年】' , convert(varchar(4),h.BONUS_YEAR))  as DESC1  ");
        sb.AppendLine("        ,t2.SUB_DESC as DESC2															   ");
        sb.AppendLine("  FROM TB_S_M_SALARY_CAL_H t1 															   ");
        sb.AppendLine("  left join TB_S_M_BONUS_H h on  t1.SALARY_DT = h.SALARY_DT 							     ");
        sb.AppendLine("  left join TB_9_M_COMM_D t2 on  t2.SYS_CD ='SC' and  t2.MAIN_CD='MAIL_TEXT' and  t2.CODE_VAL1 = t1.PAY_KIND ");
        sb.AppendLine("  left join TB_9_M_COMM_D t3 on  t3.SYS_CD ='SC' and  t3.MAIN_CD='MAIL_TITLE'  and  t3.CODE_VAL1 = t1.PAY_KIND  ");
        sb.AppendLine("  where t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE and t1.PAY_KIND='1033' ");
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        return dbConn.Query(sb, ht);

    }


    public DataTable GetReSendDataBy1035_1032_1062_1056_1070(string SALARY_TYPE, DateTime SALARY_DT)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine("  SELECT t1.SALARY_DT                                                                                    ");
        sb.AppendLine("        ,t1.SALARY_TYPE                                                                                  ");
        sb.AppendLine("        ,t1.PAY_KIND                                                                                     ");
        sb.AppendLine("        ,h.FESTIVAL_DT                                                                                   ");
        sb.AppendLine("        ,h.FESTIVAL_TYPE		                                                                            ");
        sb.AppendLine("        ,CASE WHEN t1.PAY_KIND = '1035' or t1.PAY_KIND ='1032'                                           ");
        sb.AppendLine(" 				  THEN REPLACE (t3.SUB_DESC, '【資料年】', convert(varchar(4), year(h.FESTIVAL_DT)))	");
        sb.AppendLine("              WHEN t1.PAY_KIND = '1062' or t1.PAY_KIND ='1056'  or t1.PAY_KIND ='1070'                   ");
        sb.AppendLine("                   THEN REPLACE (t3.SUB_DESC, '【員工編號】', d1.EMP_ID+vm.EMP_NAME )END as DESC1		");
        sb.AppendLine("   ,t2.SUB_DESC as DESC2																					");
        sb.AppendLine("  FROM TB_S_M_SALARY_CAL_H t1 																			");
        sb.AppendLine("  left join TB_S_M_FESTIVAL_H h on   t1.SALARY_DT = h.SALARY_DT 											");
        sb.AppendLine("  left join TB_S_M_FESTIVAL_D d1 on h.FESTIVAL_TYPE = d1.FESTIVAL_TYPE                                   ");
        sb.AppendLine("                                and h.FESTIVAL_DT = d1.FESTIVAL_DT 										");
        sb.AppendLine("                                and h.FESTIVAL_PAY_DT = d1.FESTIVAL_PAY_DT                               ");
        sb.AppendLine("                                and d1.FESTIVAL_TYPE in('4','5','6')										");
        sb.AppendLine("  left join VW_H_EMP_DATA vm on d1.EMP_ID = vm.EMP_ID   													");
        sb.AppendLine("  left join TB_9_M_COMM_D t2 on  t2.SYS_CD ='SC'                                                         ");
        sb.AppendLine("                            and  t2.MAIN_CD='MAIL_TEXT'                                                  ");
        sb.AppendLine("                            and  t2.CODE_VAL1 = t1.PAY_KIND                                              ");
        sb.AppendLine("                            and  t2.CODE_VAL2 = h.FESTIVAL_TYPE   										");
        sb.AppendLine("  left join TB_9_M_COMM_D t3 on  t3.SYS_CD ='SC'                                                         ");
        sb.AppendLine("                            and  t3.MAIN_CD='MAIL_TITLE'                                                 ");
        sb.AppendLine("                            and  t3.CODE_VAL1 = t1.PAY_KIND                                              ");
        sb.AppendLine("                            and  t3.CODE_VAL2 = h.FESTIVAL_TYPE  										");
        sb.AppendLine("  where t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE                                        ");
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        return dbConn.Query(sb, ht);
    }

    public DataTable GetReSendDataBy9999_1061_1038_1039(string SALARY_TYPE, DateTime SALARY_DT)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" SELECT t1.SALARY_DT                                                                                                                                                                   ");
        sb.AppendLine("       ,t1.SALARY_TYPE                                                                                                                                                                 ");
        sb.AppendLine("       ,t1.PAY_KIND															                                                                                                           ");
        sb.AppendLine("       ,CASE WHEN t1.PAY_KIND = '9999' or t1.PAY_KIND = '1061'                                                                                                                         ");
        sb.AppendLine("                 THEN REPLACE (t3.SUB_DESC, '【薪資年月】', substring(t1.SALARY_YM,1,4)+'年'+substring(t1.SALARY_YM,5,2)+'月') 														   ");
        sb.AppendLine("             WHEN t1.PAY_KIND = '1038' or t1.PAY_KIND = '1039'                                                                                                                         ");
        sb.AppendLine("                 THEN REPLACE (t3.SUB_DESC, '【薪資年月】', substring(CONVERT(char(10), getdate(), 112),1,4)+'年'+ substring(CONVERT(char(10), getdate(), 112),5,2)+'月') END as DESC1 ");
        sb.AppendLine("       ,t2.SUB_DESC as DESC2															                                                                                               ");
        sb.AppendLine(" FROM TB_S_M_SALARY_CAL_H t1 															                                                                                               ");
        sb.AppendLine(" left join TB_9_M_COMM_D t2 on  t2.SYS_CD ='SC' and  t2.MAIN_CD='MAIL_TEXT' and  t2.CODE_VAL1 = t1.PAY_KIND 															               ");
        sb.AppendLine(" left join TB_9_M_COMM_D t3 on  t2.SYS_CD ='SC' and  t3.MAIN_CD='MAIL_TITLE' and  t3.CODE_VAL1 = t1.PAY_KIND 															               ");
        sb.AppendLine(" where t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE                        															                           ");
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        return dbConn.Query(sb, ht);
    }


    public DataTable GetReSendDataBy1035(string SALARY_TYPE, DateTime SALARY_DT)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine("  SELECT t1.SALARY_DT                                                                           ");
        sb.AppendLine("        ,t1.SALARY_TYPE                                                                         ");
        sb.AppendLine("        ,t1.PAY_KIND                                                                            ");
        sb.AppendLine("        ,h.FESTIVAL_DT                                                                          ");
        sb.AppendLine("        ,h.FESTIVAL_TYPE																           ");
        sb.AppendLine("        ,REPLACE (t3.SUB_DESC, '【資料年】', convert(varchar(4), year(h.FESTIVAL_DT))) as DESC1 ");
        sb.AppendLine("        ,t2.SUB_DESC as DESC2																   ");
        sb.AppendLine("  FROM TB_S_M_SALARY_CAL_H t1 																   ");
        sb.AppendLine("  left join TB_S_M_FESTIVAL_H h on t1.SALARY_DT = h.SALARY_DT 								   ");
        sb.AppendLine("  left join TB_9_M_COMM_D t2 on  t2.SYS_CD ='SC'                                                ");
        sb.AppendLine("                            and  t2.MAIN_CD='MAIL_TEXT'                                         ");
        sb.AppendLine("                            and  t2.CODE_VAL1 = t1.PAY_KIND                                     ");
        sb.AppendLine("                            and  t2.CODE_VAL2 = h.FESTIVAL_TYPE   							   ");
        sb.AppendLine("  left join TB_9_M_COMM_D t3 on  t3.SYS_CD ='SC'                                                ");
        sb.AppendLine("                            and  t3.MAIN_CD='MAIL_TITLE'                                        ");
        sb.AppendLine("                            and  t3.CODE_VAL1 = t1.PAY_KIND                                     ");
        sb.AppendLine("                            and  t3.CODE_VAL2 = h.FESTIVAL_TYPE  							   ");
        sb.AppendLine("  where t1.SALARY_DT = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE                               ");
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        return dbConn.Query(sb, ht);
    }

    public bool checkMail_H_IsExist(DateTime SEND_DT, DateTime SALARY_DT, string SALARY_TYPE, string PAY_KIND)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine("  SELECT count(1) as total ");
        sb.AppendLine("  FROM TB_S_C_MAIL_BAT_H ");
        sb.AppendLine("  where CONVERT(varchar(10),SEND_DT,111) = @SEND_DT ");
        sb.AppendLine("    and CONVERT(varchar(10),SALARY_DT,111) = @SALARY_DT ");
        sb.AppendLine("    and SALARY_TYPE = @SALARY_TYPE ");
        sb.AppendLine("    and PAY_KIND = @PAY_KIND ");
        ht.Add("@SEND_DT", SEND_DT.ToString("yyyy/MM/dd"));
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy/MM/dd"));
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@PAY_KIND", PAY_KIND);
        DataTable dt = dbConn.QueryT(sb, ht);
        if (Convert.ToInt32(dt.Rows[0]["total"]) > 0)
            return false;
        else
            return true;
    }
    public bool checkMail_D_IsExist(DateTime SEND_DT, DateTime SALARY_DT, string SALARY_TYPE, string PAY_KIND,string EMP_ID)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine("  SELECT count(1) as total ");
        sb.AppendLine("  FROM TB_S_C_MAIL_BAT_D ");
        sb.AppendLine("  where CONVERT(varchar(10),SEND_DT,111) = @SEND_DT ");
        sb.AppendLine("    and CONVERT(varchar(10),SALARY_DT,111) = @SALARY_DT ");
        sb.AppendLine("    and SALARY_TYPE = @SALARY_TYPE ");
        sb.AppendLine("    and PAY_KIND = @PAY_KIND ");
        sb.AppendLine("    and EMP_ID = @EMP_ID ");
        ht.Add("@SEND_DT", SEND_DT.ToString("yyyy/MM/dd"));
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy/MM/dd"));
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@PAY_KIND", PAY_KIND);
        ht.Add("@EMP_ID", EMP_ID);
        DataTable dt = dbConn.QueryT(sb, ht);
        if (Convert.ToInt32(dt.Rows[0]["total"]) > 0)
            return false;
        else
            return true;
    }
    public void InsertTB_S_M_MAIL_BAT_H(DateTime SEND_DT, string MAIL_TITLE, string MAIL_DESC
                                     , DateTime SALARY_DT, string SALARY_TYPE, string PAY_KIND)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" insert into TB_S_C_MAIL_BAT_H(SEND_DT,MAIL_TITLE,MAIL_DESC,SALARY_DT,SALARY_TYPE,PAY_KIND,SENDTO_MAIL,CREATED_BY,CREATED_DT,FUNC_ID) ");
        sb.AppendLine("                        values(@SEND_DT,@MAIL_TITLE,@MAIL_DESC,@SALARY_DT,@SALARY_TYPE,@PAY_KIND,'系統管理者',@CREATED_BY,getdate(),'FB2SC410') ");

        ht.Add("@SEND_DT", SEND_DT.ToString("yyyy-MM-dd"));
        ht.Add("@MAIL_TITLE", MAIL_TITLE);
        ht.Add("@MAIL_DESC", MAIL_DESC);
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@PAY_KIND", PAY_KIND);
        ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);


        dbConn.ExecuteT(sb, ht);
    }

    public void InsertTB_S_M_MAIL_BAT_D(DateTime SEND_DT, DateTime SALARY_DT, string SALARY_TYPE
                                     , string PAY_KIND, string EMP_ID, string EMAIL)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" insert into TB_S_C_MAIL_BAT_D(SEND_DT,SALARY_DT,SALARY_TYPE,PAY_KIND,EMP_ID,EMAIL,MAIL_YN,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
        sb.AppendLine("                        values(@SEND_DT,@SALARY_DT,@SALARY_TYPE,@PAY_KIND,@EMP_ID,@EMAIL,'N',@CREATED_BY,getdate(),@UPDATED_BY,getdate(),'FB2SC410') ");

        ht.Add("@SEND_DT", SEND_DT.ToString("yyyy-MM-dd"));
        ht.Add("@SALARY_DT", SALARY_DT.ToString("yyyy-MM-dd"));
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@PAY_KIND", PAY_KIND);
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@EMAIL", EMAIL);
        ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
        ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);

        dbConn.ExecuteT(sb, ht);
    }
    public string getLICENSE_ID(String EMP_ID)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select LICENSE_ID ");
        sb.AppendLine(" from VW_H_EMP_DATA ");
        sb.AppendLine(" where EMP_ID=@EMP_ID ");
        ht.Add("@EMP_ID", EMP_ID);
        DataTable dt = dbConn.Query(sb, ht);
        if (dt.Rows.Count > 0)
            return Convert.ToString(dt.Rows[0][0]);
        else
            return "";
    }
    //public DataTable getDEPT_NAME(string dept_no)
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.AppendLine(" select DEPT_NAME ");
    //        sb.AppendLine(" from VW_H_DEPT_DATA ");
    //        sb.AppendLine(" where DEPT_NO = @DEPT_NO ");
    //        ht.Add("@DEPT_NO", dept_no);

    //        return dbConn.Query(sb, ht, true);
    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}
    //public DataTable getEmp_Name(string emp_id)
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.AppendLine(" select EMP_NAME ");
    //        sb.AppendLine(" from VW_H_EMP_DATA ");
    //        sb.AppendLine(" where EMP_ID =@EMP_ID ");
    //        ht.Add("@EMP_ID", emp_id);
    //        return dbConn.Query(sb, ht, true);
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
}