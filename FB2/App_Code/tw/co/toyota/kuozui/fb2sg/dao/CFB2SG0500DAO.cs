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
/// CFB2DJ040DAO 的摘要描述
/// </summary>
public class CFB2SG0500DAO : BaseDAO
{
    //dj030基本欄位
    public string START_DT { get; set; }
    public string END_DT { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_CHG_CD { get; set; }
    public string EMP_STATUS { get; set; }
    public string EMP_CD { get; set; }
    
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }



    public CFB2SG0500DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //依PK值取得資料(EXCEL下載用)
    public  DataTable getMaintainData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select  ");
            sb.Append("  dbo.FN_S_GET_SALAYRCD_AMT(a.EMP_ID, @endDT,'1001') as SALARY1001,  ");
            sb.Append("  dbo.FN_S_GET_SALAYRCD_AMT(a.EMP_ID, @endDT,'1002') as SALARY1002,  ");
            sb.Append("  dbo.FN_S_GET_SALAYRCD_AMT(a.EMP_ID, @endDT,'1003') as SALARY1003,  ");
            sb.Append("  dbo.FN_S_GET_SALAYRCD_AMT(a.EMP_ID, @endDT,'1004') as SALARY1004,  ");
            sb.Append("   ( select CODE_VAL1 from TB_9_M_PARAMETER where SYS_CD='SC' and MAIN_CD='FOOD_SUBSIDY' ) as FOOD,  ");
            sb.Append("  dbo.FN_S_GET_WORKDAY(a.EMP_ID,a.EMP_CD, @startDT, @endDT) as WORKDAYS,  ");
            sb.Append("  a.EMP_ID, a.EMP_NAME, a.JOIN_DT, a.LEVEL_CD, a.PJOB_CD,a.WORK_YEARS, a.WORK_DAYS  ");
            sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC EMP_CD_DESC     ");
            sb.Append(" from VW_H_EMP_DATA a ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD'  and c.SYS_CD='HB' ");
            sb.Append(" where 1=1 ");
            //sb.Append(" and EMP_STATUS <> @EMP_STATUS ");
            sb.Append(" and COMPANY_CD = @COMPANY_CD ");
            sb.Append(" and ( LEAVE_DT is null ");
            sb.Append(" or  LEAVE_DT > @LEAVE_DT ");
            sb.Append(" ) ");
            //sb.Append(" and emp_id='10067' ");

            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID + "%");
            }

            if (EMP_NAME != "")
            {
                sb.Append(" and a.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", EMP_NAME + "%");
            }
            if (EMP_STATUS != "-1")
            {
                sb.Append(" and a.EMP_STATUS = @EMP_STATUS ");
                ht.Add("@EMP_STATUS", EMP_STATUS);
            }
            if (EMP_CD != "-1")
            {
                sb.Append(" and a.EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", EMP_CD);
            }

            sb.Append(" order by EMP_ID ASC ");
            
            //ht.Add("@EMP_STATUS", "02");
            ht.Add("@COMPANY_CD", "K");
            ht.Add("@LEAVE_DT", Convert.ToDateTime(START_DT).ToString("yyyy/MM/dd"));
            ht.Add("@startDT", Convert.ToDateTime(START_DT).ToString("yyyy/MM/dd"));
            ht.Add("@endDT", Convert.ToDateTime(END_DT).ToString("yyyy/MM/dd"));

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

}