using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Data;

/// <summary>
/// CFB2DC1100DAO 的摘要描述
/// </summary>
public class CFB2DC1100DAO : BaseDAO
{
    public CFB2DC1100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string EMP_ID { get; set; }

    public string EMP_NAME { get; set; }

    public string CALENDAR_DT_S { get; set; }

    public string CALENDAR_DT_E { get; set; }

    public string DEPT_NO { get; set; }

    public string DEPT_NAME { get; set; }

    public string COUNT { get; set; }

    public string TYPE1 { get; set; }
    public Dictionary<string, string> OTHER_TYPE { get; set; }

    #region 修改前
    //勾選借卡
    //internal System.Data.DataTable searchType1Result()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.AppendLine(" select * From (");
    //        sb.AppendLine(" select A.BORROW_REASON,A.PERSON_ID,B.EMP_NAME,B.DEPT_NAME,sum(A.CN) CN from");
    //        sb.AppendLine(" (");
    //        sb.AppendLine(" select '借卡' BORROW_REASON,a.PERSON_ID,count(*) CN");
    //        sb.AppendLine(" from TB_D_M_TEMP_CARD_RECORD a ");
    //        sb.AppendLine(" left join TB_9_M_COMM_D b1 on a.BORROW_REASON_CD = b1.SUB_CD and b1.SYS_CD = 'DC' and b1.MAIN_CD = 'BORROW_REASON_CD'");
    //        sb.AppendLine(" where START_DT >= @START_DT_S and START_DT <= @START_DT_E ");
    //        sb.AppendLine(" and isnull(b1.CODE_VAL1,'C') = 'C.以次數計'");
    //        sb.AppendLine(" and BORROW_TYPE = '1'");
    //        sb.AppendLine(" group by  a.BORROW_REASON_CD,a.PERSON_ID");

    //        sb.AppendLine(" union all");
    //        sb.AppendLine(" select  BORROW_REASON,PERSON_ID,SUM(CN)");
    //        sb.AppendLine(" FROM (");
    //        sb.AppendLine(" SELECT '借卡' BORROW_REASON,CONVERT(CHAR(10), START_DT, 120) START_DT,a.PERSON_ID,count(*) CN");
    //        sb.AppendLine(" FROM TB_D_M_TEMP_CARD_RECORD a");
    //        sb.AppendLine(" LEFT JOIN TB_9_M_COMM_D b1 ON a.BORROW_REASON_CD = b1.SUB_CD");
    //        sb.AppendLine(" AND b1.SYS_CD = 'DC'");
    //        sb.AppendLine(" AND b1.MAIN_CD = 'BORROW_REASON_CD'");
    //        sb.AppendLine(" WHERE START_DT >= @START_DT_S and START_DT <= @START_DT_E  ");
    //        sb.AppendLine(" AND isnull(b1.CODE_VAL1, 'C') = 'D.以天數計' AND BORROW_TYPE = '1'");

    //        sb.AppendLine(" GROUP BY a.BORROW_REASON_CD,CONVERT(CHAR(10), START_DT, 120),a.PERSON_ID");
    //        sb.AppendLine(" ) A2");
    //        sb.AppendLine(" GROUP BY BORROW_REASON,PERSON_ID");
    //        sb.AppendLine(" ) A");
    //        sb.AppendLine(" LEFT JOIN (");
    //        sb.AppendLine(" SELECT EMP_ID,EMP_NAME,DEPT_NAME FROM VW_H_EMP_DATA ");
    //        sb.AppendLine(" ) B ON a.PERSON_ID = B.EMP_ID");
    //        sb.AppendLine(" GROUP BY A.BORROW_REASON,A.PERSON_ID,B.EMP_NAME,B.DEPT_NAME ");
    //        sb.AppendLine(" ) A");

    //        sb.AppendLine(" WHERE a.PERSON_ID IS NOT NULL ");
    //        ht.Add("@START_DT_S", CALENDAR_DT_S);
    //        ht.Add("@START_DT_E", CALENDAR_DT_E);

    //        if (EMP_ID != "")
    //        {
    //            sb.AppendLine(" and a.PERSON_ID like @EMP_ID ");
    //            ht.Add("@EMP_ID", EMP_ID + "%");
    //        }
    //        if (DEPT_NO != "")
    //        {
    //            sb.AppendLine(" and exists(select EMP_ID from VW_H_EMP_DATA where VW_H_EMP_DATA.DEPT_NO = @DEPT_NO and VW_H_EMP_DATA.EMP_ID = a.PERSON_ID) ");
    //            ht.Add("@DEPT_NO", DEPT_NO);
    //        }
    //        if (COUNT != "")
    //        {
    //            sb.AppendLine(" and A.CN >= @CN");
    //            ht.Add("@CN", COUNT);
    //        }


    //        return dbConn.Query(sb, ht);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    #region other
    //internal System.Data.DataTable searchOtherTypeResult()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.AppendLine(" select * From (");
    //        sb.AppendLine(" select c.SUB_DESC BORROW_REASON,A.PERSON_ID,B.EMP_NAME,B.DEPT_NAME,sum(A.CN) CN from");
    //        sb.AppendLine(" (");
    //        sb.AppendLine(" select a.BORROW_REASON_CD BORROW_REASON,a.PERSON_ID,count(*) CN");
    //        sb.AppendLine(" from TB_D_M_TEMP_CARD_RECORD a ");
    //        sb.AppendLine(" left join TB_9_M_COMM_D b1 on a.BORROW_REASON_CD = b1.SUB_CD and b1.SYS_CD = 'DC' and b1.MAIN_CD = 'BORROW_REASON_CD'");
    //        sb.AppendLine(" where START_DT >= @START_DT_S and START_DT <= @START_DT_E ");
    //        sb.AppendLine(" and isnull(b1.CODE_VAL1,'C') = 'C.以次數計'");
    //        sb.AppendLine(" and a.BORROW_REASON_CD in (@BORROW_REASON_CD)");

    //        sb.AppendLine(" group by  a.BORROW_REASON_CD,a.PERSON_ID");

    //        sb.AppendLine(" union all");
    //        sb.AppendLine(" select  BORROW_REASON,PERSON_ID,SUM(CN)");
    //        sb.AppendLine(" FROM (");
    //        sb.AppendLine(" SELECT a.BORROW_REASON_CD BORROW_REASON,CONVERT(CHAR(10), START_DT, 120) START_DT,a.PERSON_ID,count(*) CN");
    //        sb.AppendLine(" FROM TB_D_M_TEMP_CARD_RECORD a");
    //        sb.AppendLine(" LEFT JOIN TB_9_M_COMM_D b1 ON a.BORROW_REASON_CD = b1.SUB_CD");
    //        sb.AppendLine(" AND b1.SYS_CD = 'DC'");
    //        sb.AppendLine(" AND b1.MAIN_CD = 'BORROW_REASON_CD'");
    //        sb.AppendLine(" WHERE START_DT >= @START_DT_S and START_DT <= @START_DT_E  ");
    //        sb.AppendLine(" AND isnull(b1.CODE_VAL1, 'C') = 'D.以天數計' AND BORROW_TYPE = '1'");
    //        sb.AppendLine(" and a.BORROW_REASON_CD in (@BORROW_REASON_CD)");
    //        sb.AppendLine(" GROUP BY a.BORROW_REASON_CD,CONVERT(CHAR(10), START_DT, 120),a.PERSON_ID");
    //        sb.AppendLine(" ) A2");
    //        sb.AppendLine(" GROUP BY BORROW_REASON,PERSON_ID");
    //        sb.AppendLine(" ) A");
    //        sb.AppendLine(" LEFT JOIN (");
    //        sb.AppendLine(" SELECT EMP_ID,EMP_NAME,DEPT_NAME FROM VW_H_EMP_DATA ");
    //        sb.AppendLine(" ) B ON a.PERSON_ID = B.EMP_ID");
    //        sb.AppendLine(" left join (");
    //        sb.AppendLine(" select SUB_CD,SUB_DESC from TB_9_M_COMM_D B1  ");
    //        sb.AppendLine(" where B1.SYS_CD ='DC' and B1.MAIN_CD = 'BORROW_REASON_CD') C on A.BORROW_REASON = C.SUB_CD ");
    //        sb.AppendLine(" GROUP BY c.SUB_DESC,A.PERSON_ID,B.EMP_NAME,B.DEPT_NAME ");
    //        sb.AppendLine(" ) A");
    //        sb.AppendLine(" WHERE a.PERSON_ID IS NOT NULL ");
    //        ht.Add("@START_DT_S", CALENDAR_DT_S);
    //        ht.Add("@START_DT_E", CALENDAR_DT_E);
    //        string[] other_reason = new string[OTHER_TYPE.Count];
    //        int i = 0;
    //        foreach (var item in OTHER_TYPE)
    //        {
    //            other_reason[i] = item.Key;
    //            i++;
    //        }
    //        ht.Add("@BORROW_REASON_CD", other_reason);


    //        if (EMP_ID != "")
    //        {
    //            sb.AppendLine(" and a.PERSON_ID like @EMP_ID ");
    //            ht.Add("@EMP_ID", EMP_ID + "%");
    //        }
    //        if (DEPT_NO != "")
    //        {
    //            sb.AppendLine(" and exists(select EMP_ID from VW_H_EMP_DATA where VW_H_EMP_DATA.DEPT_NO = @DEPT_NO and VW_H_EMP_DATA.EMP_ID = a.PERSON_ID) ");
    //            ht.Add("@DEPT_NO", DEPT_NO);
    //        }
    //        if (COUNT != "")
    //        {
    //            sb.AppendLine(" and A.CN >= @CN");
    //            ht.Add("@CN", COUNT);
    //        }


    //        return dbConn.Query(sb, ht);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    #endregion
    #endregion
   
    internal System.Data.DataTable searchResult()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select A.PERSON_ID,B.emp_id,B.EMP_NAME,B.DEPT_NAME,A.CN times                              ");
            sb.AppendLine("from                                                                                       ");
            sb.AppendLine("(                                                                                          ");
            sb.AppendLine("      select PERSON_ID,sum(CN) CN from                                                     ");
            sb.AppendLine("      (                                                                                    ");

            //如果 畫面上.統計類型 無勾選任何項目
            if (TYPE1 == "N" && OTHER_TYPE.Count == 0)
            {
                sb.AppendLine("          select PERSON_ID,count(*) CN                                                     ");
                sb.AppendLine("          from TB_D_M_TEMP_CARD_RECORD A1                                                  ");
                sb.AppendLine("          where START_DT >= @START_DT_S                                                    ");
                sb.AppendLine("	           and START_DT <= @START_DT_E                                                    ");
                sb.AppendLine("	           and BORROW_TYPE = '1'                                                          ");
                sb.AppendLine("          group by PERSON_ID                                                               ");
                sb.AppendLine("          union all                                                                        ");
                sb.AppendLine("          select emp_id,count(*) CN                                                        ");
                sb.AppendLine("          from TB_D_M_ABNORMAL_APPLY A1                                                    ");
                sb.AppendLine("          where CALENDAR_DT >= @START_DT_S                                                 ");
                sb.AppendLine("	           and CALENDAR_DT <= @START_DT_E                                                 ");
                sb.AppendLine("          group by emp_id                                                                  ");
            }
            //如果 畫面上.統計類型 有勾選 借卡                                                                           
            if (TYPE1 == "Y")
            {
                sb.AppendLine("       select PERSON_ID,count(*) CN                                                      ");
                sb.AppendLine("       from TB_D_M_TEMP_CARD_RECORD A1                                                   ");
                sb.AppendLine("       where START_DT >=@START_DT_S                                                      ");
                sb.AppendLine("         and START_DT <=@START_DT_E                                                      ");
                sb.AppendLine("         and BORROW_TYPE = '1'                                                           ");
                sb.AppendLine("       group by PERSON_ID                                                                ");
            }
            //如果 畫面上.統計類型 有勾選 借卡和非借卡
            if (TYPE1 == "Y" && OTHER_TYPE.Count > 0)
            {
                sb.AppendLine("       union all                                                                           ");
            }
            //如果 畫面上.統計類型 有勾選 (未感應, 忘刷卡, 未帶卡, 新人入社,  交通車誤點) 時  
            if (OTHER_TYPE.Count > 0)
            {
                sb.AppendLine("        select emp_id PERSON_ID,count(*) CN                                               ");
                sb.AppendLine("        from TB_D_M_ABNORMAL_APPLY A1                                                     ");
                sb.AppendLine("        where CALENDAR_DT >=@START_DT_S                                                   ");
                sb.AppendLine("          and CALENDAR_DT <=@START_DT_E                                                   ");
                sb.AppendLine("          and A1.ABNORMAL_REASON_CD in (@BORROW_REASON_CD)                                ");
                sb.AppendLine("        group by emp_id                                                                   ");
            }

            sb.AppendLine("      ) A2 group by PERSON_ID                                                              ");
            sb.AppendLine("                                                                                           ");
            sb.AppendLine(") A                                                                                        ");
            sb.AppendLine("Left join (select emp_id,EMP_NAME,DEPT_NAME from VW_H_EMP_DATA) B on A.PERSON_ID = B.emp_id");
            sb.AppendLine("where 1=1                                                                                  ");

            ht.Add("@START_DT_S", CALENDAR_DT_S);
            ht.Add("@START_DT_E", CALENDAR_DT_E);
            string[] other_reason = new string[OTHER_TYPE.Count];
            int i = 0;
            foreach (var item in OTHER_TYPE)
            {
                other_reason[i] = item.Key;
                i++;
            }
            ht.Add("@BORROW_REASON_CD", other_reason);
            if (EMP_ID != "")
            {
                sb.AppendLine(" and A.PERSON_ID like @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID + "%");
            }
            if (DEPT_NO != "")
            {
                sb.AppendLine(" and exists(select EMP_ID from VW_H_EMP_DATA where VW_H_EMP_DATA.DEPT_NO = @DEPT_NO and VW_H_EMP_DATA.EMP_ID = A.PERSON_ID) ");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            if (COUNT != "")
            {
                sb.AppendLine(" and A.CN >= @CN");
                ht.Add("@CN", COUNT);
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getEmp_Name(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select EMP_NAME ");
            sb.AppendLine(" from VW_H_EMP_DATA ");
            sb.AppendLine(" where EMP_ID =@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getDEPT_NAME(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select DEPT_NAME ");
            sb.AppendLine(" from VW_H_DEPT_DATA ");
            sb.AppendLine(" where DEPT_NO = @DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
}