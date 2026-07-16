using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Class1 的摘要描述
/// </summary>
[Serializable]
public class WFB2DB0100DAO
{
    public Int64 RowNumber { get; set; }
    public string WORK_SHIFT_CD { get; set; }
    public string WORK_SHIFT_DESC { get; set; }
    public string IS_VALID { get; set; }
    public string IS_IFLOW_SHOW { get; set; }
    public string CALENDAR_CD { get; set; }
    public string REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public DateTime CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public DateTime UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    public DateTime? START_DT { get; set; }
    public DateTime? END_DT { get; set; }
    public List<WFB2DB0100DtlDAO> Dtl { get; set; }

    //複製的輪值表來源
    public string WORK_SHIFT_CD_Source { get; set; }
    public string CALENDAR_SDT { get; set; }
    public string CALENDAR_EDT { get; set; }


    //循環規則相關欄位
    public string RULE_CD { get; set; }	//循環規則(輪值表)代碼
    public string RULE_DESC { get; set; }	//循環規則(輪值表)說明
    public string RULE_SEQ { get; set; }	//流水序號
    public string SHIFT_CD { get; set; }	//班別代碼
    public string CIRCLE_DAYS { get; set; }	//循環天數
    public string IS_INCLUDE_HOLIDAY { get; set; }	//是否包含假日

    //輪值表生成
    public string START_DT_Grant { get; set; }	//日期區間(起)
    public string END_DT_Grant { get; set; }	//日期區間(迄)

    public string CALENDAR_DT { get; set; }	//勤務日期
    public string SHIFT_CD_O { get; set; }	//班別(舊)
    public string SHIFT_CD_N { get; set; }	//班別(新)
    public string START_DT2 { get; set; }
    public string END_DT2 { get; set; }

    public string SHIFT_TIME_CD { get; set; }
    public string WORK_HOUR { get; set; }
    public string WORK_PERIOD_HOUR { get; set; }
    public string DUTY_STIME { get; set; }
    public string DUTY_ETIME { get; set; }
    public string WORK_SHIFT_ALLOWANCE_TYPE { get; set; }
}

[Serializable]
public class WFB2DB0100DtlDAO
{
    public string WORK_SHIFT_CD { get; set; }
    public DateTime CALENDAR_DT { get; set; }
    public string SHIFT_CD { get; set; }



    //共用欄位
    public string CREATED_BY { get; set; }
    public DateTime CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public DateTime UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
}

[Serializable]
public class WFB2DBEMP_DAY_DUTY
{
    public string EMP_ID { get; set; }
    public DateTime CALENDAR_DT { get; set; }
}

public class WFB2DB0100DL : BaseDAO
{
    public int GetGridDataCount(int startRowIndex, int maximumRows, string calendar_cd, string WORK_SHIFT_CD, string WORK_SHIFT_DESC, string is_valid, string WORK_DAY_CD, string is_iflow_show)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" select COUNT(1) total_record ");
        sb.Append(" from TB_D_M_WORK_SHIFT_H TDMWSH ");
        sb.Append(" left join TB_D_M_CALENDAR_H TDMCH on TDMWSH.CALENDAR_CD=TDMCH.CALENDAR_CD ");
        sb.Append(" where 1=1 ");

        if (calendar_cd != Resources.Resource.wfb2db_dll_PlaceChoice)
        {
            sb.Append(" and TDMCH.CALENDAR_CD=@CALENDAR_CD ");
            ht.Add("@CALENDAR_CD", calendar_cd.ToUpper());
        }

        if (!string.IsNullOrEmpty(WORK_SHIFT_CD))
        {
            sb.Append(" and TDMWSH.WORK_SHIFT_CD like @WORK_SHIFT_CD+'%' ");
            ht.Add("@WORK_SHIFT_CD", WORK_SHIFT_CD.ToUpper());
        }

        if (!string.IsNullOrEmpty(WORK_SHIFT_DESC))
        {
            sb.Append(" and TDMWSH.WORK_SHIFT_DESC like '%'+@WORK_SHIFT_DESC+'%' ");
            ht.Add("@WORK_SHIFT_DESC", WORK_SHIFT_DESC);
        }

        if (!string.IsNullOrEmpty(WORK_DAY_CD))
        {
            sb.Append(" and TDMWSH.WORK_SHIFT_CD in (select WORK_SHIFT_CD from TB_D_M_WORK_SHIFT_D where SHIFT_CD like @WORK_DAY_CD +'%') ");
            ht.Add("@WORK_DAY_CD", WORK_DAY_CD.ToUpper());
        }

        if (is_valid != Resources.Resource.wfb2db_dll_PlaceChoice)
        {
            sb.Append(" and TDMWSH.IS_VALID=@IS_VALID ");
            ht.Add("@IS_VALID", is_valid);
        }
        if (is_iflow_show != Resources.Resource.wfb2db_dll_PlaceChoice)
        {
            sb.Append(" and TDMWSH.IS_IFLOW_SHOW=@IS_IFLOW_SHOW ");
            ht.Add("@IS_IFLOW_SHOW", is_iflow_show);
        }
        ht.Add("@FUNC_ID", "FB2DB010");


        Int32 ReturnValue = Convert.ToInt32(dbConn.Query(sb, ht).Rows[0]["total_record"]);
        return ReturnValue;
    }

    public DataTable GetGridData(int startRowIndex, int maximumRows, string calendar_cd, string WORK_SHIFT_CD, string WORK_SHIFT_DESC, string is_valid, string WORK_DAY_CD, string is_iflow_show, string sortExpression)
    {
        if (sortExpression.Contains("CALENDAR_CD,WORK_SHIFT_CD"))
            sortExpression = sortExpression.Replace("CALENDAR_CD,WORK_SHIFT_CD", "TDMCH.CALENDAR_CD,TDMWSH.WORK_SHIFT_CD");
        else
        {
            if (sortExpression.Contains("CALENDAR"))
                sortExpression = sortExpression.Replace("CALENDAR", "TDMCH.CALENDAR_CD");
            if (sortExpression.Contains("IS_VALID"))
                sortExpression = sortExpression.Replace("IS_VALID", "TDMWSH.IS_VALID");
            if (sortExpression.Contains("REMARK"))
                sortExpression = sortExpression.Replace("REMARK", "TDMWSH.REMARK");
        }
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select * ");
        sb.Append("from (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber , ");    //TDMCH.CALENDAR_CD,WORK_SHIFT_CD
        sb.Append("             TDMCH.CALENDAR_CD+'-'+TDMCH.CALENDAR_DESC CALENDAR,IS_IFLOW_SHOW, ");
        sb.Append("             upper(TDMCH.CALENDAR_CD) CALENDAR_CD, ");
        sb.Append("             upper(TDMWSH.WORK_SHIFT_CD) WORK_SHIFT_CD, ");
        sb.Append("             TDMWSH.WORK_SHIFT_DESC, ");
        sb.Append("             TDMWSH.IS_VALID, ");
        sb.Append("             TDMWSH.REMARK ");
        sb.Append("      from TB_D_M_WORK_SHIFT_H TDMWSH ");
        sb.Append("      left join TB_D_M_CALENDAR_H TDMCH on TDMWSH.CALENDAR_CD=TDMCH.CALENDAR_CD ");
        sb.Append("	  where 1=1 ");

        if (calendar_cd != Resources.Resource.wfb2db_dll_PlaceChoice)
        {
            sb.Append(" and TDMCH.CALENDAR_CD=@CALENDAR_CD ");
            ht.Add("@CALENDAR_CD", calendar_cd.ToUpper());
        }

        if (!string.IsNullOrEmpty(WORK_SHIFT_CD))
        {
            sb.Append(" and TDMWSH.WORK_SHIFT_CD like @WORK_SHIFT_CD+'%' ");
            ht.Add("@WORK_SHIFT_CD", WORK_SHIFT_CD.ToUpper());
        }

        if (!string.IsNullOrEmpty(WORK_SHIFT_DESC))
        {
            sb.Append(" and TDMWSH.WORK_SHIFT_DESC like '%'+@WORK_SHIFT_DESC+'%' ");
            ht.Add("@WORK_SHIFT_DESC", WORK_SHIFT_DESC);
        }

        if (is_valid != Resources.Resource.wfb2db_dll_PlaceChoice)
        {
            sb.Append(" and TDMWSH.IS_VALID=@IS_VALID ");
            ht.Add("@IS_VALID", is_valid);
        }

        if (!string.IsNullOrEmpty(WORK_DAY_CD))
        {
            sb.Append(" and TDMWSH.WORK_SHIFT_CD in (select WORK_SHIFT_CD from TB_D_M_WORK_SHIFT_D where SHIFT_CD like @WORK_DAY_CD +'%') ");
            ht.Add("@WORK_DAY_CD", WORK_DAY_CD.ToUpper());
        }
        if (is_iflow_show != Resources.Resource.wfb2db_dll_PlaceChoice)
        {
            sb.Append(" and TDMWSH.IS_IFLOW_SHOW=@IS_IFLOW_SHOW ");
            ht.Add("@IS_IFLOW_SHOW", is_iflow_show);
        }

        sb.Append(" ) TDMCH where RowNumber between CAST(@startRowIndex+1 as varchar) ");
        sb.Append("                     AND CAST(@startRowIndex+@maximumRows as varchar)");
        ht.Add("@startRowIndex", startRowIndex);
        ht.Add("@maximumRows", maximumRows);
        ht.Add("@FUNC_ID", "FB2DB010");

        DataTable returnDt = dbConn.Query(sb, ht);

        return returnDt;
    }

    public int Check_EMP_DATA(WFB2DB0100DAO Item, bool OnTransaction)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select count(1) CheckEMP_DATA ");
        sb.Append(" from TB_D_M_WORK_SHIFT_H A	 ");
        sb.Append(" inner join (select WORK_SHIFT_CD  ");
        sb.Append("             from VW_H_EMP_DATA ");
        sb.Append(" 		     where EMP_STATUS in('01','02')) B on A.WORK_SHIFT_CD = B.WORK_SHIFT_CD ");
        sb.Append(" where A.WORK_SHIFT_CD=@WORK_SHIFT_CD ");
        ht.Add("@WORK_SHIFT_CD", Item.WORK_SHIFT_CD.ToUpper());
        ht.Add("@FUNC_ID", Item.FUNC_ID);
        int QueryCount;
        if (OnTransaction)
            QueryCount = (int)dbConn.QueryT(sb, ht).Rows[0]["CheckEMP_DATA"];
        else
            QueryCount = (int)dbConn.Query(sb, ht).Rows[0]["CheckEMP_DATA"];
        return QueryCount;
    }

    public int Check_WORK_SHIFT_EMP_DAY_DUTY(WFB2DB0100DAO Item, bool OnTransaction)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" select count(*) Check_WORK_SHIFT_EMP_DAY_DUTY ");
        sb.Append(" from TB_D_M_WORK_SHIFT_H A ");
        sb.Append(" inner join TB_D_M_EMP_DAY_DUTY B on A.WORK_SHIFT_CD = B.WORK_SHIFT_CD ");
        sb.Append(" where A.WORK_SHIFT_CD=@WORK_SHIFT_CD ");

        ht.Add("@WORK_SHIFT_CD", Item.WORK_SHIFT_CD.ToUpper());
        ht.Add("@FUNC_ID", Item.FUNC_ID);
        int QueryCount;
        if (OnTransaction)
            QueryCount = (int)dbConn.QueryT(sb, ht).Rows[0]["Check_WORK_SHIFT_EMP_DAY_DUTY"];
        else
            QueryCount = (int)dbConn.Query(sb, ht).Rows[0]["Check_WORK_SHIFT_EMP_DAY_DUTY"];
        return QueryCount;
    }

    public bool Del_WORK_SHIFT_H_WORK_SHIFT_D(List<WFB2DB0100DAO> Items, bool OnTransaction, out string Message)
    {
        try
        {
            int ChangeCount = 0;
            foreach (WFB2DB0100DAO item in Items)
            {
                StringBuilder sb = new StringBuilder();
                Hashtable ht = new Hashtable();
                //寫log
                sb.Append(" update TB_D_M_WORK_SHIFT_H set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DB010' ");
                sb.Append(" where FUNC_ID=@FUNC_ID and WORK_SHIFT_CD=@WORK_SHIFT_CD; ");

                sb.Append(" delete from TB_D_M_WORK_SHIFT_H ");
                sb.Append(" where FUNC_ID=@FUNC_ID ");
                sb.Append("   and WORK_SHIFT_CD=@WORK_SHIFT_CD; ");
                ht.Clear();
                ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
                ht.Add("@WORK_SHIFT_CD", item.WORK_SHIFT_CD.ToUpper());
                ht.Add("@FUNC_ID", item.FUNC_ID);
                if (OnTransaction)
                    ChangeCount += dbConn.ExecuteT(sb, ht);
                else
                    ChangeCount += dbConn.Execute(sb, ht);

                //寫log
                sb.Append(" update TB_D_M_WORK_SHIFT_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DB010' ");
                sb.Append(" where WORK_SHIFT_CD=@WORK_SHIFT_CD;");

                sb.Append(" delete from TB_D_M_WORK_SHIFT_D ");
                sb.Append(" where WORK_SHIFT_CD=@WORK_SHIFT_CD; ");
                ht.Clear();
                ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
                ht.Add("@WORK_SHIFT_CD", item.WORK_SHIFT_CD.ToUpper());
                ht.Add("@FUNC_ID", item.FUNC_ID);
                if (OnTransaction)
                    dbConn.ExecuteT(sb, ht);
                else
                    dbConn.Execute(sb, ht);
            }
            //if (ChangeCount == Items.Count)
            //{
            Message = "";
            return true;
            //}
            //else
            //{
            //    Message = Resources.Resource.wfb2db_DataIsChange;
            //    return false;
            //}
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;
        }

    }

    public bool Del_WORK_SHIFT_D(WFB2DB0100DtlDAO Item, bool OnTransaction, out string Message)
    {
        try
        {
            int returnValue = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_WORK_SHIFT_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DB010' ");
            sb.Append(" where WORK_SHIFT_CD=@WORK_SHIFT_CD and CALENDAR_DT=@CALENDAR_DT; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_WORK_SHIFT_D ");
            sb.Append(" where WORK_SHIFT_CD=@WORK_SHIFT_CD and CALENDAR_DT=@CALENDAR_DT; ");
            ht.Add("@WORK_SHIFT_CD", Item.WORK_SHIFT_CD.ToUpper());
            ht.Add("@CALENDAR_DT", Item.CALENDAR_DT);
            ht.Add("@FUNC_ID", Item.FUNC_ID);
            if (OnTransaction)
                returnValue = dbConn.ExecuteT(sb, ht);
            else
                returnValue = dbConn.Execute(sb, ht);

            //if (returnValue == 1)
            //{
            Message = "";
            return true;
            //}
            //else
            //{
            //    Message = Resources.Resource.wfb2db_DataIsChange;
            //    return false;
            //}
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;
        }

    }

    //修改
    public bool Update_WorkShiftH(WFB2DB0100DAO Item, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_D_M_WORK_SHIFT_H ");
            sb.Append(" set  WORK_SHIFT_DESC=@WORK_SHIFT_DESC, ");
            sb.Append("      IS_VALID=@IS_VALID, ");
            sb.Append("      IS_IFLOW_SHOW=@IS_IFLOW_SHOW, ");
            sb.Append("      CALENDAR_CD=@CALENDAR_CD, ");
            sb.Append("      REMARK=@REMARK, ");
            sb.Append("      UPDATED_BY=@UPDATED_BY, ");
            sb.Append("      UPDATED_DT=@UPDATED_DT,  ");
            sb.Append("      FUNC_ID=@FUNC_ID  ");
            //sb.Append("      START_DT=@START_DT,  ");
            //sb.Append("      END_DT=@END_DT  ");
            sb.Append(" where WORK_SHIFT_CD=@WORK_SHIFT_CD ");
            ht.Add("@WORK_SHIFT_DESC", Item.WORK_SHIFT_DESC);
            ht.Add("@IS_VALID", Item.IS_VALID);
            ht.Add("@IS_IFLOW_SHOW", Item.IS_IFLOW_SHOW);
            ht.Add("@CALENDAR_CD", Item.CALENDAR_CD.ToUpper());
            ht.Add("@REMARK", Item.REMARK);
            ht.Add("@UPDATED_BY", Item.UPDATED_BY);
            ht.Add("@UPDATED_DT", Item.UPDATED_DT);
            ht.Add("@FUNC_ID", Item.FUNC_ID);
            //ht.Add("@START_DT", Item.START_DT);
            //ht.Add("@END_DT", Item.END_DT);
            ht.Add("@WORK_SHIFT_CD", Item.WORK_SHIFT_CD.ToUpper());

            int ExecuteReturn;
            if (OnTransaction)
                ExecuteReturn = dbConn.ExecuteT(sb, ht);
            else
                ExecuteReturn = dbConn.Execute(sb, ht);
            return true;
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;
        }
    }

    public int Check_WORK_SHIFT_H_By_Key(WFB2DB0100DAO Item, bool OnTransaction, out string Message)
    {
        try
        {
            Message = string.Empty;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count('1') as CheckKey ");
            sb.Append(" from TB_D_M_WORK_SHIFT_H ");
            sb.Append(" where FUNC_ID='FB2DB010' and WORK_SHIFT_CD=@WORK_SHIFT_CD ");

            ht.Add("@WORK_SHIFT_CD", Item.WORK_SHIFT_CD.ToUpper());
            int QueryCount;
            if (OnTransaction)
                QueryCount = (int)dbConn.QueryT(sb, ht).Rows[0]["CheckKey"];
            else
                QueryCount = (int)dbConn.Query(sb, ht).Rows[0]["CheckKey"];
            return QueryCount;
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return -1;
        }
    }

    //新增
    public bool Insert_WORK_SHIFT_H(WFB2DB0100DAO Item, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_D_M_WORK_SHIFT_H ([WORK_SHIFT_CD],[WORK_SHIFT_DESC],[IS_VALID],[CALENDAR_CD],[REMARK],IS_IFLOW_SHOW,[CREATED_BY],[CREATED_DT],[UPDATED_BY],[UPDATED_DT],[FUNC_ID]) ");
            sb.Append(" values(@WORK_SHIFT_CD,@WORK_SHIFT_DESC,@IS_VALID,@CALENDAR_CD,@REMARK,@IS_IFLOW_SHOW,@CREATED_BY,@CREATED_DT,@UPDATED_BY,@UPDATED_DT,@FUNC_ID) ");

            ht.Add("WORK_SHIFT_CD", Item.WORK_SHIFT_CD.ToUpper());
            ht.Add("WORK_SHIFT_DESC", Item.WORK_SHIFT_DESC);
            ht.Add("IS_VALID", Item.IS_VALID);
            ht.Add("CALENDAR_CD", Item.CALENDAR_CD.ToUpper());
            ht.Add("REMARK", Item.REMARK);
            ht.Add("IS_IFLOW_SHOW", Item.IS_IFLOW_SHOW);
            ht.Add("CREATED_BY", Item.CREATED_BY);
            ht.Add("CREATED_DT", Item.CREATED_DT);
            ht.Add("UPDATED_BY", Item.UPDATED_BY);
            ht.Add("UPDATED_DT", Item.UPDATED_DT);
            ht.Add("FUNC_ID", Item.FUNC_ID);

            int ChangeCount;
            if (OnTransaction)
                ChangeCount = dbConn.ExecuteT(sb, ht);
            else
                ChangeCount = dbConn.Execute(sb, ht);

            return true;
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;

        }
    }

    public int CheckEMP_DAY_DUTY(DateTime StartDate, DateTime EndDate, string CALENDAR_CD, bool OnTransaction, out string Message)
    {
        try
        {
            Message = string.Empty;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*) CheckEMP_DAY_DUTY ");
            sb.Append(" from (select A2.WORK_SHIFT_CD ");
            sb.Append("       from TB_D_M_WORK_SHIFT_H A1	");
            sb.Append("       inner join TB_D_M_WORK_SHIFT_H A2 on A1.CALENDAR_CD = A2.CALENDAR_CD ");
            sb.Append("       where A1.CALENDAR_CD = @CALENDAR_CD) A ");
            sb.Append(" inner join TB_D_M_EMP_DAY_DUTY B on A.WORK_SHIFT_CD = B.WORK_SHIFT_CD ");
            sb.Append(" where B.CALENDAR_DT >=@StartCALENDAR_DT ");
            sb.Append("   and B.CALENDAR_DT <=@EndCALENDAR_DT ");

            ht.Add("@CALENDAR_CD", CALENDAR_CD.ToUpper());
            ht.Add("@StartCALENDAR_DT", StartDate.ToString("yyyy-MM-dd"));
            ht.Add("@EndCALENDAR_DT", EndDate.ToString("yyyy-MM-dd"));

            int QueryCount;
            if (OnTransaction)
                QueryCount = (int)dbConn.QueryT(sb, ht).Rows[0]["CheckEMP_DAY_DUTY"];
            else
                QueryCount = (int)dbConn.Query(sb, ht).Rows[0]["CheckEMP_DAY_DUTY"];

            return QueryCount;
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return -1;
        }
    }

    public List<WFB2DB0100DtlDAO> GetTB_D_M_WORK_SHIFT_D(string strWORK_SHIFT_CD, string StartCALENDAR_DT, String EndCALENDAR_DT, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select [WORK_SHIFT_CD] ");
            sb.Append("       ,[CALENDAR_DT] ");
            sb.Append("       ,[SHIFT_CD] ");
            sb.Append("       ,[CREATED_BY] ");
            sb.Append("       ,[CREATED_DT] ");
            sb.Append("       ,[UPDATED_BY] ");
            sb.Append("       ,[UPDATED_DT] ");
            sb.Append("       ,[FUNC_ID] ");
            sb.Append(" from [TB_D_M_WORK_SHIFT_D] D ");
            sb.Append(" where D.WORK_SHIFT_CD=@WORK_SHIFT_CD ");
            sb.Append("   and D.CALENDAR_DT>=@StartCALENDAR_DT ");
            sb.Append("   and D.CALENDAR_DT<@EndCALENDAR_DT ");
            ht.Add("@WORK_SHIFT_CD", strWORK_SHIFT_CD.ToUpper());
            ht.Add("@StartCALENDAR_DT", StartCALENDAR_DT);
            ht.Add("@EndCALENDAR_DT", EndCALENDAR_DT);

            DataTable DtData;
            if (OnTransaction)
                DtData = dbConn.QueryT(sb, ht);
            else
                DtData = dbConn.Query(sb, ht);
            return (from item in DtData.AsEnumerable()
                    orderby item["SHIFT_CD"] descending
                    select new WFB2DB0100DtlDAO
                    {
                        WORK_SHIFT_CD = item.Field<string>("WORK_SHIFT_CD"),
                        CALENDAR_DT = item.Field<DateTime>("CALENDAR_DT"),
                        SHIFT_CD = item.Field<string>("SHIFT_CD"),
                        CREATED_BY = (item.Table.Columns.Contains("CREATED_BY") ? item.Field<string>("CREATED_BY") : null),
                        CREATED_DT = item.Field<DateTime>("CREATED_DT"),
                        UPDATED_BY = (item.Table.Columns.Contains("UPDATED_BY") ? item.Field<string>("UPDATED_BY") : null),
                        UPDATED_DT = item.Field<DateTime>("UPDATED_DT"),
                        FUNC_ID = (item.Table.Columns.Contains("FUNC_ID") ? item.Field<string>("FUNC_ID") : null)
                    }).ToList();
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return null;
        }
    }

    //判斷輪值表PK值
    public int Check_WORK_SHIFT_D_By_Key(string strWORK_SHIFT_CD, string StartCALENDAR_DT, String EndCALENDAR_DT, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*) CheckKey ");
            sb.Append(" from [TB_D_M_WORK_SHIFT_D] D ");
            sb.Append(" where D.WORK_SHIFT_CD=@WORK_SHIFT_CD ");
            sb.Append("   and D.CALENDAR_DT>=@StartCALENDAR_DT ");
            sb.Append("   and D.CALENDAR_DT<@EndCALENDAR_DT ");
            ht.Add("@WORK_SHIFT_CD", strWORK_SHIFT_CD.ToUpper());
            ht.Add("@StartCALENDAR_DT", StartCALENDAR_DT);
            ht.Add("@EndCALENDAR_DT", EndCALENDAR_DT);
            int QueryCount;
            if (OnTransaction)
                QueryCount = (int)dbConn.QueryT(sb, ht).Rows[0]["CheckKey"];
            else
                QueryCount = (int)dbConn.Query(sb, ht).Rows[0]["CheckKey"];
            return QueryCount;
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return -1;
        }
    }

    //刪除輪值表明細檔
    public bool delete_WORK_SHIFT_D_By_Key(WFB2DB0100DAO Destination, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(@" update TB_D_M_WORK_SHIFT_D 
                         set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = @FUNC_ID
                         where WORK_SHIFT_CD=@WORK_SHIFT_CD 
                         and CALENDAR_DT>=@CALENDAR_SDT 
                         and CALENDAR_DT<@CALENDAR_EDT
                        ");

            sb.Append(@" delete  from TB_D_M_WORK_SHIFT_D      
                        where WORK_SHIFT_CD=@WORK_SHIFT_CD
                        and CALENDAR_DT>=@CALENDAR_SDT 
                        and CALENDAR_DT<=@CALENDAR_EDT
                     ");

            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            ht.Add("@WORK_SHIFT_CD", Destination.WORK_SHIFT_CD.ToUpper());
            ht.Add("@CALENDAR_SDT", Destination.CALENDAR_SDT);
            ht.Add("@CALENDAR_EDT", Destination.CALENDAR_EDT);
            ht.Add("@FUNC_ID", Destination.FUNC_ID);
            dbConn.ExecuteT(sb, ht);
            Message = "";
            return true;
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;
        }
    }

    //新增輪值表明細檔
    public bool Insert_WORK_SHIFT_D(WFB2DB0100DAO Destination, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" insert into TB_D_M_WORK_SHIFT_D 
                         (WORK_SHIFT_CD,CALENDAR_DT,SHIFT_CD,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) 
                         select @WORK_SHIFT_CD, CALENDAR_DT,SHIFT_CD
                         ,@CREATED_BY,@CREATED_DT,@UPDATED_BY,@UPDATED_DT,@FUNC_ID
                        from TB_D_M_WORK_SHIFT_D
                        where 1=1
                        and WORK_SHIFT_CD=@WORK_SHIFT_CD_Source
                        and CALENDAR_DT>=@CALENDAR_SDT
                        and CALENDAR_DT<=@CALENDAR_EDT   
                        ");

            ht.Add("@WORK_SHIFT_CD", Destination.WORK_SHIFT_CD.ToUpper());
            ht.Add("@CALENDAR_SDT", Destination.CALENDAR_SDT);
            ht.Add("@CALENDAR_EDT", Destination.CALENDAR_EDT);
            ht.Add("@WORK_SHIFT_CD_Source", Destination.WORK_SHIFT_CD_Source.ToUpper());
            ht.Add("@CREATED_BY", Destination.CREATED_BY);
            ht.Add("@CREATED_DT", Destination.CREATED_DT);
            ht.Add("@UPDATED_BY", Destination.UPDATED_BY);
            ht.Add("@UPDATED_DT", Destination.UPDATED_DT);
            ht.Add("@FUNC_ID", Destination.FUNC_ID);

            dbConn.ExecuteT(sb, ht);
            return true;
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;

        }
    }

    //呼叫行事曆生成_更新班表及狀態檔 SP
    internal void execSP_D_WORK_SHIFT_COPY(WFB2DB0100DAO Destination)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_WORK_SHIFT_COPY");
            ht.Add("@WORK_SHIFT_CD_Source", Destination.WORK_SHIFT_CD_Source.ToUpper());
            ht.Add("@WORK_SHIFT_CD_Destination", Destination.WORK_SHIFT_CD.ToUpper());
            ht.Add("@CALENDAR_SDT", Destination.CALENDAR_SDT);
            ht.Add("@CALENDAR_EDT", Destination.CALENDAR_EDT);
            ht.Add("@USERID", SessionHandle.Current.emp_id);//CREATED_BY
            ht.Add("@FUNCID", Destination.FUNC_ID);
            dbConn.ExecuteSPT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }

    }


    //新增輪值表明細檔(舊的)
    public bool Insert_WORK_SHIFT_D(WFB2DB0100DAO Item, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            int ChangeCount = 0;

            foreach (WFB2DB0100DtlDAO dtl in Item.Dtl)
            {
                StringBuilder sb = new StringBuilder();
                Hashtable ht = new Hashtable();
                sb.Append(" insert into TB_D_M_WORK_SHIFT_D ([WORK_SHIFT_CD],[CALENDAR_DT],[SHIFT_CD],[CREATED_BY],[CREATED_DT],[UPDATED_BY],[UPDATED_DT],[FUNC_ID]) ");
                sb.Append("                           values(@WORK_SHIFT_CD, @CALENDAR_DT,@SHIFT_CD,@CREATED_BY,@CREATED_DT,@UPDATED_BY,@UPDATED_DT,@FUNC_ID) ");
                ht.Add("@WORK_SHIFT_CD", Item.WORK_SHIFT_CD.ToUpper());
                ht.Add("@CALENDAR_DT", dtl.CALENDAR_DT);
                ht.Add("@SHIFT_CD", dtl.SHIFT_CD.ToUpper());
                ht.Add("@CREATED_BY", dtl.CREATED_BY);
                ht.Add("@CREATED_DT", dtl.CREATED_DT);
                ht.Add("@UPDATED_BY", dtl.UPDATED_BY);
                ht.Add("@UPDATED_DT", dtl.UPDATED_DT);
                ht.Add("@FUNC_ID", dtl.FUNC_ID);
                if (OnTransaction)
                    ChangeCount += dbConn.ExecuteT(sb, ht);
                else
                    ChangeCount += dbConn.Execute(sb, ht);
            }
            //if (ChangeCount == Item.Dtl.Count)
            //{
            return true;
            //}
            //else
            //{
            //    Message = Resources.Resource.wfb2db_Data_Already;
            //    return false;
            //}
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;

        }
    }

    public bool Insert_WORK_SHIFT_D_Single(WFB2DB0100DAO Item, int ItemIndex, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_D_M_WORK_SHIFT_D ([WORK_SHIFT_CD],[CALENDAR_DT],[SHIFT_CD],[CREATED_BY],[CREATED_DT],[UPDATED_BY],[UPDATED_DT],[FUNC_ID]) ");
            sb.Append("                           values(@WORK_SHIFT_CD, @CALENDAR_DT,@SHIFT_CD,@CREATED_BY,@CREATED_DT,@UPDATED_BY,@UPDATED_DT,@FUNC_ID) ");
            ht.Add("@WORK_SHIFT_CD", Item.WORK_SHIFT_CD.ToUpper());
            ht.Add("@CALENDAR_DT", Item.Dtl[ItemIndex].CALENDAR_DT);
            ht.Add("@SHIFT_CD", Item.Dtl[ItemIndex].SHIFT_CD.ToUpper());
            ht.Add("@CREATED_BY", Item.Dtl[ItemIndex].CREATED_BY);
            ht.Add("@CREATED_DT", Item.Dtl[ItemIndex].CREATED_DT);
            ht.Add("@UPDATED_BY", Item.Dtl[ItemIndex].UPDATED_BY);
            ht.Add("@UPDATED_DT", Item.Dtl[ItemIndex].UPDATED_DT);
            ht.Add("@FUNC_ID", Item.Dtl[ItemIndex].FUNC_ID);

            int ExecuteReturn;
            if (OnTransaction)
                ExecuteReturn = dbConn.ExecuteT(sb, ht);
            else
                ExecuteReturn = dbConn.Execute(sb, ht);
            //if (ExecuteReturn > 0)
            //{
            return true;
            //}
            //else
            //{
            //    Message = Resources.Resource.wfb2db_Data_Already;
            //    return false;
            //}
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;
        }
    }

    public bool Update_WORK_SHIFT_D_Single(WFB2DB0100DAO Item, int ItemIndex, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" UPDATE [TB_D_M_WORK_SHIFT_D] ");
            sb.Append(" SET [SHIFT_CD] = @SHIFT_CD ");
            sb.Append("    ,[UPDATED_BY] = @UPDATED_BY ");
            sb.Append("    ,[UPDATED_DT] = @UPDATED_DT ");
            sb.Append("    ,[FUNC_ID] = @FUNC_ID ");
            sb.Append(" WHERE WORK_SHIFT_CD=@WORK_SHIFT_CD and CALENDAR_DT=@CALENDAR_DT ");
            ht.Add("@WORK_SHIFT_CD", Item.WORK_SHIFT_CD.ToUpper());
            ht.Add("@CALENDAR_DT", Item.Dtl[ItemIndex].CALENDAR_DT);
            ht.Add("@SHIFT_CD", Item.Dtl[ItemIndex].SHIFT_CD.ToUpper());
            ht.Add("@CREATED_BY", Item.Dtl[ItemIndex].CREATED_BY);
            ht.Add("@CREATED_DT", Item.Dtl[ItemIndex].CREATED_DT);
            ht.Add("@UPDATED_BY", Item.Dtl[ItemIndex].UPDATED_BY);
            ht.Add("@UPDATED_DT", Item.Dtl[ItemIndex].UPDATED_DT);
            ht.Add("@FUNC_ID", Item.Dtl[ItemIndex].FUNC_ID);
            int ExecuteReturn;
            if (OnTransaction)
                ExecuteReturn = dbConn.ExecuteT(sb, ht);
            else
                ExecuteReturn = dbConn.Execute(sb, ht);

            //if (ExecuteReturn > 0)
            //{
            return true;
            //}
            //else
            //{
            //    Message = Resources.Resource.wfb2db_Data_Already;
            //    return false;
            //}
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;
        }
    }

    public bool Save_WORK_SHIFT_D(WFB2DB0100DAO Item, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        bool SaveSuccess = true;
        foreach (WFB2DB0100DtlDAO dtl in Item.Dtl)
        {
            int CheckDtl = Check_WORK_SHIFT_D_By_Key(Item.WORK_SHIFT_CD, dtl.CALENDAR_DT.ToString("yyyy/MM/dd"), dtl.CALENDAR_DT.AddDays(1).ToString("yyyy/MM/dd"), OnTransaction, out Message);
            dtl.WORK_SHIFT_CD = Item.WORK_SHIFT_CD.ToUpper();
            if (string.IsNullOrEmpty(dtl.SHIFT_CD) == false)
            {

                if (CheckDtl > 0)
                    SaveSuccess &= Update_WORK_SHIFT_D_Single(Item, Item.Dtl.IndexOf(dtl), OnTransaction, out Message);
                else
                    SaveSuccess &= Insert_WORK_SHIFT_D_Single(Item, Item.Dtl.IndexOf(dtl), OnTransaction, out Message);
            }
            else
            {
                if (CheckDtl > 0)
                    Del_WORK_SHIFT_D(dtl, OnTransaction, out Message);
            }
        }
        return SaveSuccess;
    }

    public List<WFB2DB0100DAO> GetTB_D_M_WORK_SHIFT_H(string strWORK_SHIFT_CD, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select [WORK_SHIFT_CD], ");
            sb.Append("        [WORK_SHIFT_DESC], ");
            sb.Append("        [IS_VALID], ");
            sb.Append("        [CALENDAR_CD], ");
            sb.Append("        [REMARK], ");
            sb.Append("        [CREATED_BY], ");
            sb.Append("        [CREATED_DT], ");
            sb.Append("        [UPDATED_BY], ");
            sb.Append("        [UPDATED_DT], ");
            sb.Append("        [FUNC_ID] ");
            //sb.Append("        [START_DT], ");
            //sb.Append("        [END_DT] ");
            sb.Append(" from [TB_D_M_WORK_SHIFT_H] H ");
            sb.Append(" where H.WORK_SHIFT_CD=@WORK_SHIFT_CD ");
            ht.Add("@WORK_SHIFT_CD", strWORK_SHIFT_CD.ToUpper());

            DataTable DtData;
            if (OnTransaction)
                DtData = dbConn.QueryT(sb, ht);
            else
                DtData = dbConn.Query(sb, ht);

            return (from item in DtData.AsEnumerable()
                    select new WFB2DB0100DAO
                    {
                        WORK_SHIFT_CD = (item.Table.Columns.Contains("WORK_SHIFT_CD") ? item.Field<string>("WORK_SHIFT_CD") : null),
                        WORK_SHIFT_DESC = (item.Table.Columns.Contains("WORK_SHIFT_DESC") ? item.Field<string>("WORK_SHIFT_DESC") : null),
                        IS_VALID = (item.Table.Columns.Contains("IS_VALID") ? item.Field<string>("IS_VALID") : null),
                        CALENDAR_CD = (item.Table.Columns.Contains("CALENDAR_CD") ? item.Field<string>("CALENDAR_CD") : null),
                        REMARK = (item.Table.Columns.Contains("REMARK") ? item.Field<string>("REMARK") : null),
                        CREATED_BY = (item.Table.Columns.Contains("CREATED_BY") ? item.Field<string>("CREATED_BY") : null),
                        CREATED_DT = item.Field<DateTime>("CREATED_DT"),
                        UPDATED_BY = (item.Table.Columns.Contains("UPDATED_BY") ? item.Field<string>("UPDATED_BY") : null),
                        UPDATED_DT = item.Field<DateTime>("UPDATED_DT"),
                        FUNC_ID = (item.Table.Columns.Contains("FUNC_ID") ? item.Field<string>("FUNC_ID") : null)
                        //START_DT = item.Field<DateTime?>("START_DT"),
                        //END_DT = item.Field<DateTime?>("END_DT")
                    }).ToList();
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return null;
        }
    }

    public WFB2DB0100DAO GetWorkShiftH(WFB2DB0100DAO InputDao, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select A1.WORK_SHIFT_CD, ");
            sb.Append("        A1.WORK_SHIFT_DESC, ");
            sb.Append("        A1.IS_VALID, ");
            sb.Append("        A1.CALENDAR_CD, ");
            sb.Append("        A1.REMARK, ");
            sb.Append("        A1.CREATED_BY,");
            sb.Append("        A1.CREATED_DT,");
            sb.Append("        A1.UPDATED_BY,");
            sb.Append("        A1.UPDATED_DT,");
            sb.Append("        A1.FUNC_ID ");
            //sb.Append("        A1.START_DT, ");
            //sb.Append("        A1.END_DT ");
            sb.Append(" from TB_D_M_WORK_SHIFT_H A1 ");
            sb.Append(" where A1.WORK_SHIFT_CD = @WORK_SHIFT_CD ");
            ht.Add("@WORK_SHIFT_CD", InputDao.WORK_SHIFT_CD.ToUpper());
            DataTable dtData;
            if (OnTransaction)
                dtData = dbConn.QueryT(sb, ht);
            else
                dtData = dbConn.Query(sb, ht);

            return (from item in dtData.AsEnumerable()
                    select new WFB2DB0100DAO
                    {
                        WORK_SHIFT_CD = (item.Table.Columns.Contains("WORK_SHIFT_CD") ? item.Field<string>("WORK_SHIFT_CD") : null),
                        WORK_SHIFT_DESC = (item.Table.Columns.Contains("WORK_SHIFT_DESC") ? item.Field<string>("WORK_SHIFT_DESC") : null),
                        IS_VALID = (item.Table.Columns.Contains("IS_VALID") ? item.Field<string>("IS_VALID") : null),
                        CALENDAR_CD = (item.Table.Columns.Contains("CALENDAR_CD") ? item.Field<string>("CALENDAR_CD") : null),
                        REMARK = (item.Table.Columns.Contains("REMARK") ? item.Field<string>("REMARK") : null),
                        CREATED_BY = (item.Table.Columns.Contains("CREATED_BY") ? item.Field<string>("CREATED_BY") : null),
                        CREATED_DT = item.Field<DateTime>("CREATED_DT"),
                        UPDATED_BY = (item.Table.Columns.Contains("UPDATED_BY") ? item.Field<string>("UPDATED_BY") : null),
                        UPDATED_DT = item.Field<DateTime>("UPDATED_DT"),
                        FUNC_ID = (item.Table.Columns.Contains("FUNC_ID") ? item.Field<string>("FUNC_ID") : null)
                        //START_DT = item.Field<DateTime?>("START_DT"),
                        //END_DT = item.Field<DateTime?>("END_DT")
                    }).ToList().First();
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return null;
        }
    }

    public List<WFB2DBEMP_DAY_DUTY> GetTB_D_M_EMP_DAY_DUTY(string CALENDAR_CD, DateTime CALENDAR_DT, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_ID,CALENDAR_DT ");
            sb.Append(" from TB_D_M_EMP_DAY_DUTY ");
            sb.Append(" where CALENDAR_CD=@CALENDAR_CD ");
            sb.Append("   and CALENDAR_DT=@CALENDAR_DT ");
            ht.Add("@CALENDAR_CD", CALENDAR_CD.ToUpper());
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            DataTable QueryDt;
            if (OnTransaction)
                QueryDt = dbConn.QueryT(sb, ht);
            else
                QueryDt = dbConn.Query(sb, ht);

            return (from item in QueryDt.AsEnumerable()
                    select new WFB2DBEMP_DAY_DUTY
                    {
                        CALENDAR_DT = item.Field<DateTime>("CALENDAR_DT"),
                        EMP_ID = (item.Table.Columns.Contains("EMP_ID") ? item.Field<string>("EMP_ID") : null),
                    }).ToList();
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return null;
        }
    }

    public string getWorkDayDesc(string SHIFT_CD, string WORK_SHIFTymd)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select SHIFT_DESC ");
        sb.Append(" from TB_D_M_SHIFT_H ");
        sb.Append(" where SHIFT_CD=@SHIFT_CD ");
        //todo
        if (WORK_SHIFTymd != "")
        {
            sb.Append(" and @CALENDAR_DT >= START_DT and @CALENDAR_DT <= END_DT ");
            ht.Add("@CALENDAR_DT", WORK_SHIFTymd);
        }
        ht.Add("@SHIFT_CD", SHIFT_CD.ToUpper());
        DataTable returnValue = dbConn.Query(sb, ht);
        if (returnValue.Rows.Count > 0)
            return Convert.ToString(returnValue.Rows[0][0]);
        else
            return string.Empty;
    }

    public DataTable getAllWorkShiftH()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select upper(SHIFT_CD) SHIFT_CD,SHIFT_DESC ");
        sb.Append(" from VW_D_M_SHIFT_H  order by SHIFT_CD ");
        return dbConn.Query(sb, ht);
    }

    public DataTable getWORK_DAY_CD(string work_day_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select TOP 1  SHIFT_DESC ");
            sb.Append(" from TB_D_M_SHIFT_H ");
            sb.Append(" where SHIFT_CD=@SHIFT_CD ");
            sb.Append(" order by END_DT desc ");
            ht.Add("@SHIFT_CD", work_day_cd);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getCALENDAR_WORK_DAY_CD(string calendar_cd, string ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select WORK_DAY_CD ");
            sb.Append(" from TB_D_M_CALENDAR_D ");
            sb.Append(" where CALENDAR_CD=@CALENDAR_CD and CONVERT(varchar(7), CALENDAR_DT, 111)=@YM ");
            ht.Add("@CALENDAR_CD", calendar_cd);
            ht.Add("@YM", ym);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    #region  循環規則設定-Set

    //Gridview 查詢資料
    public DataTable getSetData(int startRowIndex, int maximumRows
                          , string ruleCD, string ruleDesc, string sortExpression)
    {
        try
        {

            if (sortExpression.Contains("RULE_CD"))
                sortExpression = sortExpression.Replace("RULE_CD", "a.RULE_CD");
            if (sortExpression.Contains("SHIFT_CD"))
                sortExpression = sortExpression.Replace("SHIFT_CD", "a.SHIFT_CD");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" a.RULE_CD,RULE_DESC,RULE_SEQ,a.SHIFT_CD,CIRCLE_DAYS,IS_INCLUDE_HOLIDAY ");
            sb.Append(",case when IS_INCLUDE_HOLIDAY='Y' then 'Y-是' else 'N-否' end IS_INCLUDE_HOLIDAY_DESC ");
            sb.Append(" ,b.SHIFT_CD +'-'+b.SHIFT_DESC SHIFT_DESC ");
            sb.Append(" ,b.SHIFT_CD Edit_SHIFT_CD, b.SHIFT_DESC Edit_SHIFT_DESC");
            sb.Append(" from TB_D_M_WORK_SHIFT_RULE a ");
            sb.Append("  left join VW_D_M_SHIFT_H b on  a.SHIFT_CD = b.SHIFT_CD   ");
            sb.Append(" where 1=1 ");


            //查詢條件
            if (ruleCD != "")
            {
                sb.Append(" and RULE_CD like @RULE_CD ");
                ht.Add("@RULE_CD", ruleCD + "%");
            }

            if (ruleDesc != "")
            {
                sb.Append(" and RULE_DESC like @RULE_DESC ");
                ht.Add("@RULE_DESC", ruleDesc + "%");
            }


            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //Gridview 查詢總筆數
    public int getSetCount(int startRowIndex, int maximumRows
                       , string ruleCD, string ruleDesc)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_WORK_SHIFT_RULE ");
            sb.Append(" where 1=1 ");


            //查詢條件
            if (ruleCD != "")
            {
                sb.Append(" and RULE_CD like @RULE_CD ");
                ht.Add("@RULE_CD", ruleCD + "%");
            }

            if (ruleDesc != "")
            {
                sb.Append(" and RULE_DESC like @RULE_DESC ");
                ht.Add("@RULE_DESC", ruleDesc + "%");
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

    //新增 循環規則代碼
    public void insertSetData(WFB2DB0100DAO db010DAO)
    {
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" declare @rule_sqe decimal(3) = 
                        case when  
                        (select count(*) from TB_D_M_WORK_SHIFT_RULE where RULE_CD=@rule_CD) =0
                        then 001
                        else ( select max(RULE_SEQ)+1 from TB_D_M_WORK_SHIFT_RULE where RULE_CD=@rule_CD)
                        end
                        ");

            sb.Append(" INSERT INTO TB_D_M_WORK_SHIFT_RULE ");
            sb.Append(" ( ");
            sb.Append(" RULE_CD,RULE_DESC,RULE_SEQ,SHIFT_CD,CIRCLE_DAYS,IS_INCLUDE_HOLIDAY ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" ( @RULE_CD,@RULE_DESC,@rule_sqe,@SHIFT_CD,@CIRCLE_DAYS,@IS_INCLUDE_HOLIDAY");
            sb.Append("   ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID )");

            ht.Add("@RULE_CD", db010DAO.RULE_CD);
            ht.Add("@RULE_DESC", db010DAO.RULE_DESC);
            ht.Add("@SHIFT_CD", db010DAO.SHIFT_CD);
            ht.Add("@CIRCLE_DAYS", db010DAO.CIRCLE_DAYS);
            ht.Add("@IS_INCLUDE_HOLIDAY", db010DAO.IS_INCLUDE_HOLIDAY);
            ht.Add("@CREATED_BY", db010DAO.UPDATED_BY);
            ht.Add("@CREATED_DT", now);
            ht.Add("@UPDATED_BY", db010DAO.UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", db010DAO.FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    //修改 循環規則代碼
    public void updateSetData(WFB2DB0100DAO db010DAO)
    {
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_D_M_WORK_SHIFT_RULE ");
            sb.Append(" set ");
            sb.Append("  RULE_DESC=@RULE_DESC, SHIFT_CD=@SHIFT_CD ,CIRCLE_DAYS=@CIRCLE_DAYS,IS_INCLUDE_HOLIDAY=@IS_INCLUDE_HOLIDAY ");
            sb.Append(" ,UPDATED_BY=@UPDATED_BY,UPDATED_DT=@UPDATED_DT,FUNC_ID=@FUNC_ID  ");
            sb.Append(" where  RULE_CD=@RULE_CD and RULE_SEQ=@RULE_SEQ ");

            //pk條件
            ht.Add("@RULE_CD", db010DAO.RULE_CD);
            ht.Add("@RULE_SEQ", db010DAO.RULE_SEQ);

            ht.Add("@RULE_DESC", db010DAO.RULE_DESC);
            ht.Add("@SHIFT_CD", db010DAO.SHIFT_CD);
            ht.Add("@CIRCLE_DAYS", db010DAO.CIRCLE_DAYS);
            ht.Add("@IS_INCLUDE_HOLIDAY", db010DAO.IS_INCLUDE_HOLIDAY);
            ht.Add("@UPDATED_BY", db010DAO.UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", db010DAO.FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    //刪除 循環規則代碼
    public void deleteSetData(string ruleCD, string ruleSeq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_D_M_WORK_SHIFT_RULE ");
            sb.Append(" where  RULE_CD=@RULE_CD and RULE_SEQ=@RULE_SEQ ");

            //pk條件
            ht.Add("@RULE_CD", ruleCD);
            ht.Add("@RULE_SEQ", ruleSeq);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //只修改 同代碼的 循環規則代碼 
    public void updateSetDescData(WFB2DB0100DAO db010DAO)
    {
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_D_M_WORK_SHIFT_RULE ");
            sb.Append(" set ");
            sb.Append("  RULE_DESC=@RULE_DESC ");
            sb.Append(" ,UPDATED_BY=@UPDATED_BY,UPDATED_DT=@UPDATED_DT,FUNC_ID=@FUNC_ID  ");
            sb.Append(" where  RULE_CD=@RULE_CD  ");

            //pk條件
            ht.Add("@RULE_CD", db010DAO.RULE_CD);

            ht.Add("@RULE_DESC", db010DAO.RULE_DESC);
            ht.Add("@UPDATED_BY", db010DAO.UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", db010DAO.FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 循環規則代碼 說明
    public DataTable getRuleDesc(string ruleCD)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select top 1 RULE_DESC ");
        sb.Append(" from TB_D_M_WORK_SHIFT_RULE ");
        sb.Append(" where  RULE_CD=@RULE_CD  ");
        //pk條件
        ht.Add("@RULE_CD", ruleCD);
        return dbConn.Query(sb, ht);
    }

    //取得 所有 循環規則代碼及說明
    public DataTable getRuleCD()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select RULE_CD sub_cd, RULE_CD+'-'+ISNULL(RULE_DESC,'') sub_desc  ");
        sb.Append(" from TB_D_M_WORK_SHIFT_RULE ");
        sb.Append(" group by RULE_CD,RULE_DESC ");
        sb.Append(" order by RULE_CD ASC  ");
        return dbConn.Query(sb, ht);
    }

    #endregion


    #region  輪值表維護-Grant

    //Gridview 查詢資料  循環規則代碼
    public DataTable getGrantData(int startRowIndex, int maximumRows
                          , string ruleCD, string sortExpression)
    {
        try
        {

            if (sortExpression.Contains("RULE_CD"))
                sortExpression = sortExpression.Replace("RULE_CD", "a.RULE_CD");
            if (sortExpression.Contains("SHIFT_CD"))
                sortExpression = sortExpression.Replace("SHIFT_CD", "a.SHIFT_CD");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" a.RULE_CD,RULE_DESC,RULE_SEQ,a.SHIFT_CD,CIRCLE_DAYS,IS_INCLUDE_HOLIDAY ");
            sb.Append(",case when IS_INCLUDE_HOLIDAY='Y' then 'Y-是' else 'N-否' end IS_INCLUDE_HOLIDAY_DESC ");
            sb.Append(" ,b.SHIFT_CD +'-'+b.SHIFT_DESC SHIFT_DESC ");
            sb.Append(" ,b.SHIFT_CD Edit_SHIFT_CD, b.SHIFT_DESC Edit_SHIFT_DESC");
            sb.Append(" from TB_D_M_WORK_SHIFT_RULE a ");
            sb.Append("  left join VW_D_M_SHIFT_H b on  a.SHIFT_CD = b.SHIFT_CD     ");
            sb.Append(" where 1=1 ");


            if (ruleCD != "" && ruleCD != "-1")
            {
                sb.Append(" and RULE_CD = @RULE_CD ");
                ht.Add("@RULE_CD", ruleCD);
            }
            else
            {
                sb.Append(" and 1!=1 ");
            }


            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //Gridview 查詢總筆數  循環規則代碼
    public int getGrantCount(int startRowIndex, int maximumRows
                       , string ruleCD)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_WORK_SHIFT_RULE ");
            sb.Append(" where 1=1 ");

            if (ruleCD != "" && ruleCD != "-1")
            {
                sb.Append(" and RULE_CD = @RULE_CD ");
                ht.Add("@RULE_CD", ruleCD);
            }
            else
            {
                sb.Append(" and 1!=1 ");
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



    //輪值表生成區間起日 是否為已計薪的考勤日期迄日
    public DataTable checkIsSalaryDate(WFB2DB0100DAO db010DAO)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select "
                      + " case when @startDT  >dbo.FN_S_DUTY_EDT(@shiftCD) then 'true' "
                      + " else 'false' "
                      + " end "
                      + " as isSalaryDate "
                      + " ");
            ht.Add("@shiftCD", db010DAO.WORK_SHIFT_CD);
            ht.Add("@startDT", db010DAO.START_DT_Grant);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }


    //檢查日勤務班表資料檔是否已有勤務班表
    public DataTable checkDutyCount(WFB2DB0100DAO db010DAO)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount ");
            sb.Append(" from TB_D_M_EMP_DAY_DUTY A ");
            sb.Append(" where 1=1 ");
            sb.Append(" and A.WORK_SHIFT_CD = @WORK_SHIFT_CD		  ");
            sb.Append(" and A.CALENDAR_DT >= @START_DT  ");
            sb.Append(" and A.CALENDAR_DT <= @END_DT ");

            ht.Add("@WORK_SHIFT_CD", db010DAO.WORK_SHIFT_CD);
            ht.Add("@START_DT", db010DAO.START_DT_Grant);
            ht.Add("@END_DT", db010DAO.END_DT_Grant);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //呼叫 輪值表生成 SP
    internal void execSP_D_GEN_WORK_SHIFT_D(WFB2DB0100DAO db010DAO)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_GEN_WORK_SHIFT_D");
            ht.Add("@startDT", Convert.ToDateTime(db010DAO.START_DT_Grant));
            ht.Add("@endDT", Convert.ToDateTime(db010DAO.END_DT_Grant));
            ht.Add("@CALENDAR_CD", db010DAO.CALENDAR_CD);
            ht.Add("@WORK_SHIFT_CD", db010DAO.WORK_SHIFT_CD);
            ht.Add("@RULE_CD", db010DAO.RULE_CD);
            ht.Add("@USERID", db010DAO.CREATED_BY);//CREATED_BY
            ht.Add("@FUNCID", db010DAO.FUNC_ID);
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }


    #endregion


    internal DataTable getAll_WORK_SHIFT_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select WORK_SHIFT_CD from TB_D_M_WORK_SHIFT_H where IS_VALID='Y' ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    internal DataTable getAll_SHIFT_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select WORK_SHIFT_CD from TB_D_M_WORK_SHIFT_H where IS_VALID='Y' ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getTB_D_M_SHIFT_H(string shift_cd, string calendar_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SHIFT_CD from TB_D_M_SHIFT_H ");
            sb.Append(" where SHIFT_CD=@shift_cd and @calendar_dt between START_DT and END_DT ");
            ht.Add("@shift_cd", shift_cd);
            ht.Add("@calendar_dt", calendar_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deleteAll_TB_D_M_WORK_SHIFT_D(DataTable excel_dt)
    {
        try
        {
            string[] pno = new string[2];
            string[] sbval = new string[2];
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_D_M_WORK_SHIFT_D ");
            sb.Append(" where 1=1 ");
            for (int i = 0; i < excel_dt.Rows.Count; i++)
            {
                pno = new string[2];
                sbval = new string[2];
                pno[0] = "@WORK_SHIFT_CD" + (i + 1);
                sbval[0] = " WORK_SHIFT_CD = " + pno[0];
                pno[1] = "@CALENDAR_DT" + (i + 1);
                sbval[1] = " CALENDAR_DT = " + pno[1];

                if (i == 0)
                {
                    sb.Append(" and ( ( ");
                    for (int p = 0; p < pno.Count(); p++)
                    {
                        if (p == 0)
                        {
                            sb.Append(sbval[p]);
                            ht.Add(pno[p], excel_dt.Rows[i][p]);
                            continue;
                        }
                        sb.Append(" and ");
                        sb.Append(sbval[p]);
                        ht.Add(pno[p], excel_dt.Rows[i][p]);
                    }
                    sb.Append(" ) ");
                    continue;
                }
                sb.Append(" or ( ");
                for (int p = 0; p < pno.Count(); p++)
                {
                    if (p == 0)
                    {
                        sb.Append(sbval[p]);
                        ht.Add(pno[p], excel_dt.Rows[i][p]);
                        continue;
                    }
                    sb.Append(" and ");
                    sb.Append(sbval[p]);
                    ht.Add(pno[p], excel_dt.Rows[i][p]);
                }
                sb.Append(" ) ");

            }
            if (excel_dt.Rows.Count > 0)
            {
                sb.Append(" ) ");
            }

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }

    }

    internal void WriteToDatabase(string tableName, DataTable myTable)
    {
        try
        {
            // get your connection string
            string connString = utilities.connstr;
            // connect to SQL
            using (SqlConnection connection =
                    new SqlConnection(connString))
            {
                // make sure to enable triggers
                // more on triggers in next post
                SqlBulkCopy bulkCopy =
                    new SqlBulkCopy
                    (
                    connection,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.FireTriggers |
                    SqlBulkCopyOptions.UseInternalTransaction,
                    null
                    );

                // set the destination table name
                bulkCopy.DestinationTableName = tableName;
                connection.Open();

                // write the data in the "dataTable"
                bulkCopy.WriteToServer(myTable);
                connection.Close();
            }
            // reset
            myTable.Clear();
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getTB_D_M_WORK_SHIFT_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select WORK_SHIFT_CD,WORK_SHIFT_CD+'-'+WORK_SHIFT_DESC WORK_SHIFT_DESC ");
            sb.Append(" from TB_D_M_WORK_SHIFT_H where IS_VALID='Y' ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getTB_D_M_WORK_SHIFT_D_t(WFB2DB0100DAO dao)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select WORK_SHIFT_CD,CONVERT(VARCHAR(10),CALENDAR_DT,111) as CALENDAR_DT ,SHIFT_CD ");
            sb.Append(" from TB_D_M_WORK_SHIFT_D ");
            sb.Append(" where WORK_SHIFT_CD = iif(@WORK_SHIFT_CD='ALL',WORK_SHIFT_CD,@WORK_SHIFT_CD) ");
            sb.Append(" and CALENDAR_DT >= @START_DT ");
            sb.Append(" and CALENDAR_DT <= @END_DT ");

            ht.Add("@WORK_SHIFT_CD", dao.WORK_SHIFT_CD);
            ht.Add("@START_DT", dao.START_DT2);
            ht.Add("@END_DT", dao.END_DT2);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getSHIFT_H(string shift_cd, string calendar_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SHIFT_CD,SHIFT_TIME_CD,WORK_HOUR,WORK_PERIOD_HOUR,DUTY_STIME ");
            sb.Append(" ,DUTY_ETIME,WORK_SHIFT_ALLOWANCE_TYPE ");
            sb.Append(" from TB_D_M_SHIFT_H ");
            sb.Append(" where SHIFT_CD = @SHIFT_CD ");
            sb.Append(" and @CALENDAR_DT between START_DT and END_DT ");

            ht.Add("@SHIFT_CD", shift_cd);
            ht.Add("@CALENDAR_DT", calendar_dt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal string getDUTY_TIME(string calendar_dt, string duty_stime)
    {
        try
        {
            string result="";
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT [dbo].[FN_D_GEN_DATETIME] (@CALENDAR_DT,@DUTY_STIME) as result ");

            ht.Add("@DUTY_STIME", duty_stime);
            ht.Add("@CALENDAR_DT", calendar_dt);

            dt =dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["result"].ToString();
            }

            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void updateEMP_DAY_DUTY(WFB2DB0100DAO dao2)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" UPDATE [dbo].[TB_D_M_EMP_DAY_DUTY] ");
            sb.Append(" SET [SHIFT_CD] = @SHIFT_CD_N ");
            sb.Append(" ,[SHIFT_TIME_CD] =@SHIFT_TIME_CD ");
            sb.Append(" ,[WORK_HOUR] = @WORK_HOUR ");
            sb.Append(" ,[WORK_PERIOD_HOUR] = @WORK_PERIOD_HOUR ");
            sb.Append(" ,[DUTY_STIME] = dbo.FN_D_GEN_DATETIME(CALENDAR_DT,@DUTY_STIME)");
            sb.Append(" ,[DUTY_ETIME] = dbo.FN_D_GEN_DATETIME(CALENDAR_DT,@DUTY_ETIME)");
            sb.Append(" ,[WORK_SHIFT_ALLOWANCE_TYPE] = @WORK_SHIFT_ALLOWANCE_TYPE ");
            sb.Append(" ,[CREATED_BY] = @UPDATED_BY ");
            sb.Append(" ,[CREATED_DT] = GETDATE() ");
            sb.Append(" ,[UPDATED_BY] = @UPDATED_BY ");
            sb.Append(" ,[UPDATED_DT] = GETDATE() ");
            sb.Append(" ,[FUNC_ID] = @FUNC_ID ");
            sb.Append(" WHERE WORK_SHIFT_CD=@WORK_SHIFT_CD and CALENDAR_DT=@CALENDAR_DT and SHIFT_CD=@SHIFT_CD_O ");
            
            //條件
            ht.Add("@CALENDAR_DT", dao2.CALENDAR_DT);
            ht.Add("@SHIFT_CD_O", dao2.SHIFT_CD_O);
            //修改值
            ht.Add("@SHIFT_CD_N", dao2.SHIFT_CD_N);
            ht.Add("@SHIFT_TIME_CD", dao2.SHIFT_TIME_CD);
            ht.Add("@WORK_HOUR", dao2.WORK_HOUR);
            ht.Add("@WORK_PERIOD_HOUR", dao2.WORK_PERIOD_HOUR);
            ht.Add("@DUTY_STIME", dao2.DUTY_STIME);
            ht.Add("@DUTY_ETIME", dao2.DUTY_ETIME);
            ht.Add("@WORK_SHIFT_ALLOWANCE_TYPE", dao2.WORK_SHIFT_ALLOWANCE_TYPE);
            ht.Add("@WORK_SHIFT_CD", dao2.WORK_SHIFT_CD);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DB010");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void updateEMP_DUTY_CHECK_STATUS(WFB2DB0100DAO dao2)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" UPDATE TB_D_M_EMP_DUTY_CHECK_STATUS
                        set DUTY_CHECK_RESULT = @DUTY_CHECK_RESULT
                           ,UPDATED_BY = @UPDATED_BY 
                           ,[UPDATED_DT] = GETDATE() 
                           ,[FUNC_ID] = @FUNC_ID 
                        where  CALENDAR_DT = @CALENDAR_DT
                        and   CALENDAR_DT > dbo.FN_D_DUTY_CLOSE_DT(-1) 
                        and  SHIFT_CD = @SHIFT_CD_O
                        ");

            //條件
            ht.Add("@CALENDAR_DT", dao2.CALENDAR_DT);
            ht.Add("@SHIFT_CD_O", dao2.SHIFT_CD_O);
            //修改值
            ht.Add("@DUTY_CHECK_RESULT", "N");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DB010");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }






}

