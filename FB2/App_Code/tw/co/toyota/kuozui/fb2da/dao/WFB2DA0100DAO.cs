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
public class WFB2DA0100DAO
{
    public Int64 RowNumber { get; set; }
    public string CALENDAR_CD_Source { get; set; }
    public string CALENDAR_CD { get; set; }
    public string CALENDAR_DESC { get; set; }
    public string CALENDAR_SDT { get; set; }
    public string CALENDAR_EDT { get; set; }
    public string IS_VALID { get; set; }
    public string REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public bool isNextYear { get; set; }

    public string CALENDAR_DT { get; set; }
    public string WORK_DAY_CD { get; set; }
    public string GROUP_CD { get; set; }
    public string DT_TYPE { get; set; }
    public DateTime CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public DateTime UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    public string START_DATE { get; set; }
    public string END_DATE { get; set; }

    public List<WFB2DA0100DtlDAO> Dtl { get; set; }
    public List<WFB2DA0100WorkShiftH> WorkShiftH { get; set; }

}


[Serializable]
public class WFB2DA0100DtlDAO
{
    public DateTime CALENDAR_DT { get; set; }
    public string WORK_DAY_CD { get; set; }
    public string DT_TYPE { get; set; }
    public string GROUP_CD { get; set; }
    public string CREATED_BY { get; set; }
    public DateTime CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public DateTime UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
}

[Serializable]
public class WFB2DA0100WorkShiftH
{
    public string WORK_SHIFT_CD { get; set; }
    public string WORK_SHIFT_DESC { get; set; }
    public string IS_VALID { get; set; }
    public DateTime? START_DT { get; set; }
    public DateTime? END_DT { get; set; }
}

[Serializable]
public class WFB2DA0100LoopRule
{
    public string MAIN_CD { get; set; }
    public string SUB_CD { get; set; }
    public string SUB_DESC { get; set; }
    public string CODE_VAL1 { get; set; }
    public string CODE_VAL2 { get; set; }
    public string REMARK { get; set; }
    public int ORDER_SEQ { get; set; }
    public string SYS_CD { get; set; }
    public string IS_VALID { get; set; }
}

[Serializable]
public class WFB2DAEMP_DAY_DUTY
{
    public string EMP_ID { get; set; }
    public DateTime CALENDAR_DT { get; set; }
}

public class WFB2DA0100DL : BaseDAO
{
    public List<WFB2DA0100DAO> getdll_CALENDAR_Data()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select CALENDAR_CD,CALENDAR_DESC ");
        sb.AppendLine(" from TB_D_M_CALENDAR_H ");
        sb.AppendLine(" where 1=1 ");
        return (from item in dbConn.Query(sb, ht).AsEnumerable()
                select new WFB2DA0100DAO
                {
                    CALENDAR_CD = (item.Table.Columns.Contains("CALENDAR_CD") ? item.Field<string>("CALENDAR_CD") : null),
                    CALENDAR_DESC = (item.Table.Columns.Contains("CALENDAR_DESC") ? item.Field<string>("CALENDAR_DESC") : null)
                }).ToList();
    }

    public int GetGridDataCount(int startRowIndex, int maximumRows, string calendar_cd, string is_valid)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" select COUNT(1) total_record ");
        sb.AppendLine(" from TB_D_M_CALENDAR_H ");
        sb.AppendLine(" where 1=1 ");

        if (calendar_cd != Resources.Resource.wfb2da_dll_PlaceChoice)
        {
            sb.AppendLine(" and CALENDAR_CD=@CALENDAR_CD ");
            ht.Add("@CALENDAR_CD", calendar_cd);
        }
        if (is_valid != Resources.Resource.wfb2da_dll_PlaceChoice)
        {
            sb.AppendLine(" and IS_VALID=@IS_VALID ");
            ht.Add("@IS_VALID", is_valid);
        }


        Int32 ReturnValue = Convert.ToInt32(dbConn.Query(sb, ht).Rows[0]["total_record"]);
        return ReturnValue;
    }

    public DataTable GetGridData(int startRowIndex, int maximumRows, string calendar_cd, string is_valid, string sortExpression)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select * ");
        sb.AppendLine("from (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber , ");
        sb.AppendLine("			 upper(CALENDAR_CD) CALENDAR_CD, ");
        sb.AppendLine("			 CALENDAR_DESC, ");
        sb.AppendLine("			 IS_VALID, ");
        sb.AppendLine("             REMARK, ");
        sb.AppendLine("             CREATED_BY, ");
        sb.AppendLine("             CREATED_DT, ");
        sb.AppendLine("             UPDATED_BY, ");
        sb.AppendLine("             UPDATED_DT, ");
        sb.AppendLine("             FUNC_ID, ");
        sb.AppendLine("             ACTION_MODE='Q' ");
        sb.AppendLine("	  from TB_D_M_CALENDAR_H ");
        sb.AppendLine("	  where 1=1 ");

        if (calendar_cd != Resources.Resource.wfb2da_dll_PlaceChoice)
        {
            sb.AppendLine(" and CALENDAR_CD=@CALENDAR_CD ");
            ht.Add("@CALENDAR_CD", calendar_cd);
        }

        if (is_valid != Resources.Resource.wfb2da_dll_PlaceChoice)
        {
            sb.AppendLine(" and IS_VALID=@IS_VALID ");
            ht.Add("@IS_VALID", is_valid);
        }
        sb.AppendLine(" ) TDMCH where RowNumber between CAST(@startRowIndex+1 as varchar) ");
        sb.AppendLine("                     AND CAST(@startRowIndex+@maximumRows as varchar)");
        ht.Add("@startRowIndex", startRowIndex);
        ht.Add("@maximumRows", maximumRows);
        //ht.Add("@FUNC_ID", "FB2DA010");

        DataTable returnDt = dbConn.Query(sb, ht);

        return returnDt;
    }

    public int Check_EMP_DATA(WFB2DA0100DAO Item, bool OnTransaction)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select count(*) CheckEMP_DATA ");
        sb.AppendLine(" from  (select A2.WORK_SHIFT_CD ");
        sb.AppendLine("        from TB_D_M_CALENDAR_H A1 ");
        sb.AppendLine("        inner join TB_D_M_WORK_SHIFT_H A2 on A1.CALENDAR_CD = A2.CALENDAR_CD ");
        sb.AppendLine("        where A1.CALENDAR_CD = @CALENDAR_CD ) A ");
        sb.AppendLine(" inner join (select * ");
        sb.AppendLine("             from VW_H_EMP_DATA ");
        sb.AppendLine("             where EMP_STATUS in ('01','02')) B on A.WORK_SHIFT_CD = B.WORK_SHIFT_CD ");
        ht.Add("@CALENDAR_CD", Item.CALENDAR_CD);
        int QueryCount;
        if (OnTransaction)
            QueryCount = (int)dbConn.QueryT(sb, ht).Rows[0]["CheckEMP_DATA"];
        else
            QueryCount = (int)dbConn.Query(sb, ht).Rows[0]["CheckEMP_DATA"];
        return QueryCount;
    }

    public int Check_WORK_SHIFT(WFB2DA0100DAO Item, bool OnTransaction)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select count(*) Check_WORK_SHIFT ");
        sb.AppendLine(" from TB_D_M_CALENDAR_H A1 ");
        sb.AppendLine(" inner join TB_D_M_WORK_SHIFT_H A2 on A1.CALENDAR_CD = A2.CALENDAR_CD ");
        sb.AppendLine(" where A1.CALENDAR_CD = @CALENDAR_CD  ");
        ht.Add("@CALENDAR_CD", Item.CALENDAR_CD);
        int QueryCount;
        if (OnTransaction)
            QueryCount = (int)dbConn.QueryT(sb, ht).Rows[0]["Check_WORK_SHIFT"];
        else
            QueryCount = (int)dbConn.Query(sb, ht).Rows[0]["Check_WORK_SHIFT"];
        return QueryCount;
    }

    public int Check_WORK_SHIFT_EMP_DAY_DUTY(WFB2DA0100DAO Item, bool OnTransaction)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select count(*) Check_WORK_SHIFT_EMP_DAY_DUTY ");
        sb.AppendLine(" from (select A2.WORK_SHIFT_CD ");
        sb.AppendLine(" 	  from TB_D_M_CALENDAR_H A1 ");
        sb.AppendLine(" 	  inner join TB_D_M_WORK_SHIFT_H A2 on A1.CALENDAR_CD = A2.CALENDAR_CD ");
        sb.AppendLine(" 	  where A1.CALENDAR_CD =@CALENDAR_CD ) A ");
        sb.AppendLine(" inner join TB_D_M_EMP_DAY_DUTY B on A.WORK_SHIFT_CD = B.WORK_SHIFT_CD ");
        ht.Add("@CALENDAR_CD", Item.CALENDAR_CD);
        int QueryCount;
        if (OnTransaction)
            QueryCount = (int)dbConn.QueryT(sb, ht).Rows[0]["Check_WORK_SHIFT_EMP_DAY_DUTY"];
        else
            QueryCount = (int)dbConn.Query(sb, ht).Rows[0]["Check_WORK_SHIFT_EMP_DAY_DUTY"];
        return QueryCount;
    }

    public bool Del_CALENDAR_H_CALENDAR_D(List<WFB2DA0100DAO> Items, bool OnTransaction, out string Message)
    {
        try
        {
            int ChangeCount = 0;
            foreach (WFB2DA0100DAO item in Items)
            {
                StringBuilder sb = new StringBuilder();
                Hashtable ht = new Hashtable();
                //寫log
                sb.Append(" update TB_D_M_CALENDAR_H set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DA010' ");
                sb.Append(" where CALENDAR_CD=@CALENDAR_CD; ");

                sb.AppendLine(" delete from TB_D_M_CALENDAR_H ");
                sb.AppendLine(" where 1=1 ");
                sb.AppendLine("   and CALENDAR_CD=@CALENDAR_CD; ");
                ht.Clear();
                ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
                ht.Add("@CALENDAR_CD", item.CALENDAR_CD);
                if (OnTransaction)
                    ChangeCount += dbConn.ExecuteT(sb, ht);
                else
                    ChangeCount += dbConn.Execute(sb, ht);

                sb.Append(" update TB_D_M_CALENDAR_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DA010' ");
                sb.Append(" where CALENDAR_CD=@CALENDAR_CD; ");

                sb.AppendLine(" delete from TB_D_M_CALENDAR_D ");
                sb.AppendLine(" where CALENDAR_CD=@CALENDAR_CD;  ");
                ht.Clear();
                ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
                ht.Add("@CALENDAR_CD", item.CALENDAR_CD);
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
            //    Message = Resources.Resource.wfb2da_DataIsChange;
            //    return false;
            //}
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;
        }

    }

    public bool Update_CALENDAR_H(WFB2DA0100DAO Item, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" update TB_D_M_CALENDAR_H ");
            sb.AppendLine(" set  CALENDAR_DESC=@CALENDAR_DESC, ");
            sb.AppendLine("      IS_VALID=@IS_VALID, ");
            sb.AppendLine("      REMARK=@REMARK, ");
            sb.AppendLine("      UPDATED_BY=@UPDATED_BY, ");
            sb.AppendLine("      UPDATED_DT=@UPDATED_DT, ");
            sb.AppendLine("      FUNC_ID=@FUNC_ID  ");
            sb.AppendLine(" where CALENDAR_CD=@CALENDAR_CD ");
            ht.Add("@CALENDAR_CD", Item.CALENDAR_CD);
            ht.Add("@CALENDAR_DESC", Item.CALENDAR_DESC);
            ht.Add("@IS_VALID", Item.IS_VALID);
            ht.Add("@REMARK", Item.REMARK);
            ht.Add("@UPDATED_BY", Item.UPDATED_BY);
            ht.Add("@UPDATED_DT", Item.UPDATED_DT);
            ht.Add("@FUNC_ID", Item.FUNC_ID);
            int ExecuteReturn;
            if (OnTransaction)
                ExecuteReturn = dbConn.ExecuteT(sb, ht);
            else
                ExecuteReturn = dbConn.Execute(sb, ht);
            //if (ExecuteReturn == 1)
            //{
            return true;
            //}
            //else
            //{
            //    Message = Resources.Resource.wfb2da_DataIsChange;
            //    return false;
            //}
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;
        }
    }

    //判斷行事曆主檔是否存在
    public int Check_CALENDAR_H_By_Key(WFB2DA0100DAO Item, bool OnTransaction, out string Message)
    {
        try
        {
            Message = string.Empty;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count('1') as CheckKey ");
            sb.AppendLine(" from TB_D_M_CALENDAR_H ");
            sb.AppendLine(" where CALENDAR_CD=@CALENDAR_CD ");

            ht.Add("@CALENDAR_CD", Item.CALENDAR_CD);
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

    //新增行事曆主檔
    public bool Insert_CALENDAR_H(WFB2DA0100DAO Item, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" insert into TB_D_M_CALENDAR_H (CALENDAR_CD,CALENDAR_DESC,IS_VALID,REMARK,UPDATED_BY,UPDATED_DT,CREATED_BY,CREATED_DT,FUNC_ID) ");
            sb.AppendLine(" values(@CALENDAR_CD, @CALENDAR_DESC,@IS_VALID,@REMARK,@UPDATED_BY,@UPDATED_DT,@CREATED_BY,@CREATED_DT,@FUNC_ID) ");
            ht.Add("@CALENDAR_CD", Item.CALENDAR_CD);
            ht.Add("@CALENDAR_DESC", Item.CALENDAR_DESC);
            ht.Add("@IS_VALID", Item.IS_VALID);
            ht.Add("@REMARK", Item.REMARK);
            ht.Add("@UPDATED_BY", Item.UPDATED_BY);
            ht.Add("@UPDATED_DT", Item.UPDATED_DT);
            ht.Add("@CREATED_BY", Item.CREATED_BY);
            ht.Add("@CREATED_DT", Item.CREATED_DT);
            ht.Add("@FUNC_ID", Item.FUNC_ID);
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
            sb.AppendLine(" select count(*) CheckEMP_DAY_DUTY ");
            sb.AppendLine(" from (select A2.WORK_SHIFT_CD ");
            sb.AppendLine("       from TB_D_M_CALENDAR_H A1	");
            sb.AppendLine("       inner join TB_D_M_WORK_SHIFT_H A2 on A1.CALENDAR_CD = A2.CALENDAR_CD ");
            sb.AppendLine("       where A1.CALENDAR_CD = @CALENDAR_CD) A ");
            sb.AppendLine(" inner join TB_D_M_EMP_DAY_DUTY B on A.WORK_SHIFT_CD = B.WORK_SHIFT_CD ");
            sb.AppendLine(" where B.CALENDAR_DT >=@StartCALENDAR_DT ");
            sb.AppendLine("   and B.CALENDAR_DT <=@EndCALENDAR_DT ");

            ht.Add("@CALENDAR_CD", CALENDAR_CD);
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
    //取得來源行事曆的資料
    public List<WFB2DA0100DtlDAO> GetTB_D_M_CALENDAR_D(string strCALENDAR_CD, string StartCALENDAR_DT, String EndCALENDAR_DT, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select D.CALENDAR_DT, ");
            sb.AppendLine("        D.WORK_DAY_CD, "); //原出勤別改為 假日類型
            sb.AppendLine("        D.DT_TYPE, ");
            sb.AppendLine("        D.GROUP_CD, ");  //新增群組代碼
            sb.AppendLine("        D.CREATED_BY, ");
            sb.AppendLine("        D.CREATED_DT, ");
            sb.AppendLine("        D.UPDATED_BY, ");
            sb.AppendLine("        D.UPDATED_DT, ");
            sb.AppendLine("        D.FUNC_ID ");
            sb.AppendLine(" from [TB_D_M_CALENDAR_D] D ");
            sb.AppendLine(" where D.CALENDAR_CD=@CALENDAR_CD ");
            sb.AppendLine("   and D.CALENDAR_DT>=@StartCALENDAR_DT ");
            sb.AppendLine("   and D.CALENDAR_DT<=@EndCALENDAR_DT ");
            ht.Add("@CALENDAR_CD", strCALENDAR_CD);
            ht.Add("@StartCALENDAR_DT", StartCALENDAR_DT);
            ht.Add("@EndCALENDAR_DT", EndCALENDAR_DT);

            DataTable DtData;
            if (OnTransaction)
                DtData = dbConn.QueryT(sb, ht);
            else
                DtData = dbConn.Query(sb, ht);
            return (from item in DtData.AsEnumerable()
                    orderby item["DT_TYPE"] descending
                    select new WFB2DA0100DtlDAO
                    {
                        CALENDAR_DT = item.Field<DateTime>("CALENDAR_DT"),
                        WORK_DAY_CD = (item.Table.Columns.Contains("WORK_DAY_CD") ? item.Field<string>("WORK_DAY_CD") : null),
                        DT_TYPE = (item.Table.Columns.Contains("DT_TYPE") ? item.Field<string>("DT_TYPE") : null),
                        GROUP_CD = (item.Table.Columns.Contains("GROUP_CD") ? item.Field<string>("GROUP_CD") : null),
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

    //判斷行事歷明細檔是否存在
    public int Check_CALENDAR_D_By_Key(string strCALENDAR_CD, string StartCALENDAR_DT, String EndCALENDAR_DT, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(*) CheckKey ");
            sb.AppendLine(" from [TB_D_M_CALENDAR_D] D ");
            sb.AppendLine(" where D.CALENDAR_CD=@CALENDAR_CD ");
            sb.AppendLine("   and D.CALENDAR_DT>=@StartCALENDAR_DT ");
            sb.AppendLine("   and D.CALENDAR_DT<@EndCALENDAR_DT ");
            ht.Add("@CALENDAR_CD", strCALENDAR_CD);
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

    public void UPD_EMP_DAY_DUTY2(string CALENDAR_CD, DateTime CALENDAR_DT_S, DateTime CALENDAR_DT_E, string UPDATED_BY)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        try
        {
            //2.更新日勤務狀態資料檔              
            sb.AppendLine(" update TB_D_M_EMP_DUTY_CHECK_STATUS   ");
            sb.AppendLine("   set [DUTY_CHECK_RESULT] = 'N',      ");
            sb.AppendLine("   LATE_HOUR= 0,LEAVE_EARLY_HOUR= 0,LACK_HOUR= 0,DUTY_HOUR= 0,LEAVE_HOUR= 0                                 ");
            sb.AppendLine("  ,LEAVE_INFO= '',OVERTIME_HOUR_APPLY= 0,OVERTIME_HOUR_APPROVE= 0,VIOLATE_BEFORE_HOUR= 0                    ");
            sb.AppendLine("  ,VIOLATE_AFTER_HOUR= 0,OVERTIME_INFO= '',SHIFT_CD= '',WORK_SHIFT_ALLOWANCE_TYPE= '',                       ");
            sb.AppendLine("        UPDATED_BY = @UPDATED_BY,      ");
            sb.AppendLine(" 	   UPDATED_DT = getDate()         ");
            sb.AppendLine(" 	   ,FUNC_ID = 'FB2DA010'         ");
            sb.AppendLine("  from  TB_D_M_EMP_DUTY_CHECK_STATUS M ");
            sb.AppendLine(" inner join TB_D_M_EMP_DAY_DUTY A      ");
            sb.AppendLine(" on M.EMP_ID = A.EMP_ID                ");
            sb.AppendLine(" and M.CALENDAR_DT = A.CALENDAR_DT     ");
            sb.AppendLine(" inner join TB_D_M_CALENDAR_D B        ");
            sb.AppendLine(" on  A.CALENDAR_DT >= @CALENDAR_DT_S   ");
            sb.AppendLine(" and A.CALENDAR_DT <= @CALENDAR_DT_E   ");
            sb.AppendLine(" and A.CALENDAR_CD = @CALENDAR_CD      ");
            sb.AppendLine(" and A.CALENDAR_CD = B.CALENDAR_CD     ");
            sb.AppendLine(" and A.CALENDAR_DT = B.CALENDAR_DT     ");
            sb.AppendLine(" where M.CALENDAR_DT>dbo.FN_S_DUTY_EDT('LM')     ");

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@CALENDAR_DT_S", Convert.ToDateTime(CALENDAR_DT_S).ToString("yyyy/MM/dd"));
            ht.Add("@CALENDAR_DT_E", Convert.ToDateTime(CALENDAR_DT_E).ToString("yyyy/MM/dd"));
            ht.Add("@CALENDAR_CD", CALENDAR_CD);
            dbConn.ExecuteT(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    //刪除 行事曆明細檔
    public bool delete_CALENDAR_D(string calendar_cd, string calendar_SDT, string calendar_EDT, bool OnTransaction, out string Message)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_CALENDAR_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DA010' ");
            sb.Append(" where CALENDAR_CD=@CALENDAR_CD ");
            sb.Append(" and CALENDAR_DT>=@CALENDAR_SDT ");
            sb.Append(" and CALENDAR_DT<=@CALENDAR_EDT ");

            sb.AppendLine(" delete from TB_D_M_CALENDAR_D ");
            sb.AppendLine(" where CALENDAR_CD=@CALENDAR_CD  ");
            sb.Append(" and CALENDAR_DT>=@CALENDAR_SDT ");
            sb.Append(" and CALENDAR_DT<=@CALENDAR_EDT ");

            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            ht.Add("@CALENDAR_CD", calendar_cd);
            ht.Add("@CALENDAR_SDT", calendar_SDT);
            ht.Add("@CALENDAR_EDT", calendar_EDT);

            if (OnTransaction)
                dbConn.ExecuteT(sb, ht);
            else
                dbConn.Execute(sb, ht);
            Message = "";
            return true;
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;
        }

    }

    //新增行事曆明細檔
    public bool Insert_CALENDAR_D(WFB2DA0100DAO Destination, out string Message)
    {
        Message = string.Empty;
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@" insert into TB_D_M_CALENDAR_D (CALENDAR_CD,CALENDAR_DT,WORK_DAY_CD,DT_TYPE,GROUP_CD,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) 
                                select @CALENDAR_CD,CALENDAR_DT,WORK_DAY_CD,DT_TYPE,GROUP_CD
                                ,@CREATED_BY,GETDATE(),@CREATED_BY,GETDATE(),@FUNC_ID
                                from TB_D_M_CALENDAR_D
                                where 1=1
                                and CALENDAR_CD=@CALENDAR_CD_Source
                                and CALENDAR_DT>=@CALENDAR_SDT
                                and CALENDAR_DT<=@CALENDAR_EDT
                              ");
            ht.Add("@CALENDAR_CD_Source", Destination.CALENDAR_CD_Source);
            ht.Add("@CALENDAR_CD", Destination.CALENDAR_CD);
            ht.Add("@CALENDAR_SDT", Destination.CALENDAR_SDT);
            ht.Add("@CALENDAR_EDT", Destination.CALENDAR_EDT);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", Destination.FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
            return true;
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;

        }
    }
    //呼叫行事曆生成_更新班表及狀態檔 SP
    internal void execSP_D_CALENDAR_COPY(WFB2DA0100DAO Destination)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_CALENDAR_COPY");
            ht.Add("@CALENDAR_CD", Destination.CALENDAR_CD);
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

    //更新班表的出勤別(棄用)
    public bool update_DUTY(WFB2DA0100DAO Destination, out string Message)
    {
        Message = string.Empty;
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"
                            declare @endDT datetime =dbo.FN_S_DUTY_EDT('LM') ; 
                            update 	A
                            set	 WORK_DAY_CD = B.WORK_DAY_CD
                            ,UPDATED_BY =@UPDATED_BY
                            ,UPDATED_DT = GETDATE()
                            ,FUNC_ID=@FUNC_ID
                            from(  
	                            select * from 
	                            TB_D_M_EMP_DAY_DUTY 
	                            where 1=1
	                            and CALENDAR_CD=@CALENDAR_CD
	                            and CALENDAR_DT>=@CALENDAR_SDT
	                            and CALENDAR_DT<=@CALENDAR_EDT
                                and CALENDAR_DT > @endDT
                            )A
                            left join TB_D_M_CALENDAR_D	B
                            on A.CALENDAR_DT = B.CALENDAR_DT and A.CALENDAR_CD = B.CALENDAR_CD
                              ");
            ht.Add("@CALENDAR_CD", Destination.CALENDAR_CD);
            ht.Add("@CALENDAR_SDT", Destination.CALENDAR_SDT);
            ht.Add("@CALENDAR_EDT", Destination.CALENDAR_EDT);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", Destination.FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
            return true;
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;

        }
    }
    //更新班表的出勤別(棄用)
    public bool duty_status_reopen(WFB2DA0100DAO Destination, out string Message)
    {
        Message = string.Empty;
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"
                            declare @endDT datetime =dbo.FN_S_DUTY_EDT('LM') ; 
                            update 	A
                            set	 WORK_DAY_CD = B.WORK_DAY_CD
                            ,UPDATED_BY =@UPDATED_BY
                            ,UPDATED_DT = GETDATE()
                            ,FUNC_ID=@FUNC_ID
                            from(  
	                            select * from 
	                            TB_D_M_EMP_DAY_DUTY 
	                            where 1=1
	                            and CALENDAR_CD=@CALENDAR_CD
	                            and CALENDAR_DT>=@CALENDAR_SDT
	                            and CALENDAR_DT<=@CALENDAR_EDT
                                and CALENDAR_DT > @endDT
                            )A
                            left join TB_D_M_CALENDAR_D	B
                            on A.CALENDAR_DT = B.CALENDAR_DT and A.CALENDAR_CD = B.CALENDAR_CD
                              ");
            ht.Add("@CALENDAR_CD", Destination.CALENDAR_CD);
            ht.Add("@CALENDAR_SDT", Destination.CALENDAR_SDT);
            ht.Add("@CALENDAR_EDT", Destination.CALENDAR_EDT);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", Destination.FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
            return true;
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;

        }
    }

    public bool Insert_CALENDAR_D_Single(WFB2DA0100DAO Item, int ItemIndex, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            string work_day_cd = (Item.Dtl[ItemIndex].DT_TYPE == "1") ? "1" : "2";

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" insert into TB_D_M_CALENDAR_D (CALENDAR_CD,CALENDAR_DT,WORK_DAY_CD,DT_TYPE,GROUP_CD,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.AppendLine("                         values(@CALENDAR_CD, @CALENDAR_DT,@WORK_DAY_CD,@DT_TYPE,@GROUP_CD,@CREATED_BY,@CREATED_DT,@UPDATED_BY,@UPDATED_DT,@FUNC_ID) ");
            ht.Add("@CALENDAR_CD", Item.CALENDAR_CD);
            ht.Add("@CALENDAR_DT", Item.Dtl[ItemIndex].CALENDAR_DT);
            ht.Add("@WORK_DAY_CD", work_day_cd);
            ht.Add("@DT_TYPE", Item.Dtl[ItemIndex].DT_TYPE);
            ht.Add("@GROUP_CD", Item.Dtl[ItemIndex].GROUP_CD);
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
            //    Message = Resources.Resource.wfb2da_Data_Already;
            //    return false;
            //}
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;
        }
    }

    public bool Update_CALENDAR_D_Single(WFB2DA0100DAO Item, int ItemIndex, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            string work_day_cd = (Item.Dtl[ItemIndex].DT_TYPE == "1") ? "1" : "2";

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" UPDATE [TB_D_M_CALENDAR_D] ");
            sb.AppendLine(" SET [WORK_DAY_CD] = @WORK_DAY_CD ");
            sb.AppendLine("    ,[DT_TYPE] = @DT_TYPE ");
            sb.AppendLine("    ,[UPDATED_BY] = @UPDATED_BY ");
            sb.AppendLine("    ,[UPDATED_DT] = @UPDATED_DT ");
            sb.AppendLine("    ,[FUNC_ID] = @FUNC_ID ");
            sb.AppendLine(" WHERE CALENDAR_CD=@CALENDAR_CD and CALENDAR_DT=@CALENDAR_DT ");
            ht.Add("@CALENDAR_CD", Item.CALENDAR_CD);
            ht.Add("@CALENDAR_DT", Item.Dtl[ItemIndex].CALENDAR_DT);
            ht.Add("@WORK_DAY_CD", work_day_cd);
            ht.Add("@DT_TYPE", Item.Dtl[ItemIndex].DT_TYPE);
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
            //    Message = Resources.Resource.wfb2da_Data_Already;
            //    return false;
            //}
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;
        }
    }

    public bool Save_CALENDAR_D(WFB2DA0100DAO Item, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        bool SaveSuccess = true;
        foreach (WFB2DA0100DtlDAO dtl in Item.Dtl)
        {
            int CheckDtl = Check_CALENDAR_D_By_Key(Item.CALENDAR_CD, dtl.CALENDAR_DT.ToString("yyyy/MM/dd"), dtl.CALENDAR_DT.AddDays(1).ToString("yyyy/MM/dd"), OnTransaction, out Message);
            if (CheckDtl > 0)
                SaveSuccess &= Update_CALENDAR_D_Single(Item, Item.Dtl.IndexOf(dtl), OnTransaction, out Message);
            else
                SaveSuccess &= Insert_CALENDAR_D_Single(Item, Item.Dtl.IndexOf(dtl), OnTransaction, out Message);
        }
        return SaveSuccess;
    }

    public List<WFB2DA0100DAO> GetTB_D_M_CALENDAR_H(string strCALENDAR_CD, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select H.CALENDAR_CD, ");
            sb.AppendLine("        H.CALENDAR_DESC, ");
            sb.AppendLine("        H.IS_VALID, ");
            sb.AppendLine("        H.REMARK, ");
            sb.AppendLine("        H.CREATED_BY, ");
            sb.AppendLine("        H.CREATED_DT, ");
            sb.AppendLine("        H.UPDATED_BY, ");
            sb.AppendLine("        H.UPDATED_DT, ");
            sb.AppendLine("        H.FUNC_ID ");
            sb.AppendLine(" from [TB_D_M_CALENDAR_H] H ");
            sb.AppendLine(" where H.CALENDAR_CD=@CALENDAR_CD ");
            ht.Add("@CALENDAR_CD", strCALENDAR_CD);

            DataTable DtData;
            if (OnTransaction)
                DtData = dbConn.QueryT(sb, ht);
            else
                DtData = dbConn.Query(sb, ht);

            return (from item in DtData.AsEnumerable()
                    select new WFB2DA0100DAO
                    {
                        CALENDAR_CD = (item.Table.Columns.Contains("CALENDAR_CD") ? item.Field<string>("CALENDAR_CD") : null),
                        CALENDAR_DESC = (item.Table.Columns.Contains("CALENDAR_DESC") ? item.Field<string>("CALENDAR_DESC") : null),
                        IS_VALID = (item.Table.Columns.Contains("IS_VALID") ? item.Field<string>("IS_VALID") : null),
                        REMARK = (item.Table.Columns.Contains("REMARK") ? item.Field<string>("REMARK") : null),
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

    public bool GrantCALENDAR_D(WFB2DA0100DAO GrantDays, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            foreach (WFB2DA0100DtlDAO dtl in GrantDays.Dtl)
            {
                StringBuilder sb = new StringBuilder();
                Hashtable ht = new Hashtable();

                sb.AppendLine(" select count('1') CheckData from TB_D_M_CALENDAR_D D ");
                sb.AppendLine(" where D.CALENDAR_CD= @CALENDAR_CD ");
                sb.AppendLine("   and D.CALENDAR_DT=@CALENDAR_DT ");
                ht.Clear();
                ht.Add("@CALENDAR_CD", GrantDays.CALENDAR_CD);
                ht.Add("@CALENDAR_DT", dtl.CALENDAR_DT.ToString("yyyy-MM-dd"));
                int GetCount;
                if (OnTransaction)
                    GetCount = (int)dbConn.QueryT(sb, ht).Rows[0]["CheckData"];
                else
                    GetCount = (int)dbConn.Query(sb, ht).Rows[0]["CheckData"];

                if (GetCount > 0)
                {
                    sb.AppendLine(" update TB_D_M_CALENDAR_D set [WORK_DAY_CD] = iif(@DT_TYPE='1','1','2') ");
                    sb.AppendLine("                             ,[DT_TYPE] = @DT_TYPE ");
                    sb.AppendLine("                             ,[UPDATED_BY] = @UPDATED_BY ");
                    sb.AppendLine("                             ,[UPDATED_DT] = @UPDATED_DT ");
                    sb.AppendLine("                             ,[FUNC_ID] = @FUNC_ID ");
                    sb.AppendLine(" where CALENDAR_CD=@CALENDAR_CD ");
                    sb.AppendLine("   and CALENDAR_DT=@CALENDAR_DT ");
                    ht.Clear();
                    ht.Add("@DT_TYPE", dtl.WORK_DAY_CD);
                    ht.Add("@UPDATED_BY", dtl.UPDATED_BY);
                    ht.Add("@UPDATED_DT", dtl.UPDATED_DT.ToString("yyyy-MM-dd HH:mm:ss"));
                    ht.Add("@FUNC_ID", dtl.FUNC_ID);
                    ht.Add("@CALENDAR_CD", GrantDays.CALENDAR_CD);
                    ht.Add("@CALENDAR_DT", dtl.CALENDAR_DT.ToString("yyyy-MM-dd"));
                }
                else
                {
                    sb.AppendLine(" insert into TB_D_M_CALENDAR_D ");
                    sb.AppendLine("       ([CALENDAR_CD],[CALENDAR_DT],[WORK_DAY_CD],[DT_TYPE] ,[CREATED_BY],[CREATED_DT],[UPDATED_BY],[UPDATED_DT],[FUNC_ID]) ");
                    sb.AppendLine(" values(@CALENDAR_CD,@CALENDAR_DT,iif(@DT_TYPE='1','1','2'),@DT_TYPE,@CREATED_BY,@CREATED_DT,@UPDATED_BY,@UPDATED_DT,'FB2DA010') ");
                    ht.Clear();

                    ht.Add("@CALENDAR_CD", GrantDays.CALENDAR_CD);
                    ht.Add("@CALENDAR_DT", dtl.CALENDAR_DT.ToString("yyyy-MM-dd"));
                    ht.Add("@DT_TYPE", dtl.WORK_DAY_CD);
                    ht.Add("@CREATED_BY", dtl.CREATED_BY);
                    ht.Add("@CREATED_DT", dtl.CREATED_DT.ToString("yyyy-MM-dd HH:mm:ss"));
                    ht.Add("@UPDATED_BY", dtl.UPDATED_BY);
                    ht.Add("@UPDATED_DT", dtl.UPDATED_DT.ToString("yyyy-MM-dd HH:mm:ss"));
                    ht.Add("@FUNC_ID", dtl.FUNC_ID);
                }
                int ExceuteReturn;
                if (OnTransaction)
                    ExceuteReturn = dbConn.ExecuteT(sb, ht);
                else
                    ExceuteReturn = dbConn.Execute(sb, ht);

                //if (ExceuteReturn != 1)
                //{
                //    Message = Resources.Resource.wfb2da_Update_Err_plz_Retry;
                //    return false;
                //}
            }
            return true;
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;
        }
    }

    public WFB2DA0100DAO GetWorkShiftH(WFB2DA0100DAO InputDao, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select A2.WORK_SHIFT_CD,A2.WORK_SHIFT_DESC,A2.IS_VALID,A2.START_DT,A2.END_DT ");
            sb.AppendLine(" from TB_D_M_CALENDAR_H A1 ");
            sb.AppendLine(" inner join TB_D_M_WORK_SHIFT_H A2 on A1.CALENDAR_CD = A2.CALENDAR_CD ");
            sb.AppendLine(" where A1.CALENDAR_CD = @CALENDAR_CD ");
            ht.Add("@CALENDAR_CD", InputDao.CALENDAR_CD);
            DataTable dtData;
            if (OnTransaction)
                dtData = dbConn.QueryT(sb, ht);
            else
                dtData = dbConn.Query(sb, ht);

            InputDao.WorkShiftH = (from item in dtData.AsEnumerable()
                                   select new WFB2DA0100WorkShiftH
                                   {
                                       WORK_SHIFT_CD = (item.Table.Columns.Contains("WORK_SHIFT_CD") ? item.Field<string>("WORK_SHIFT_CD") : null),
                                       WORK_SHIFT_DESC = (item.Table.Columns.Contains("WORK_SHIFT_DESC") ? item.Field<string>("WORK_SHIFT_DESC") : null),
                                       IS_VALID = (item.Table.Columns.Contains("IS_VALID") ? item.Field<string>("IS_VALID") : null),
                                       START_DT = item.Field<DateTime?>("START_DT"),
                                       END_DT = item.Field<DateTime?>("END_DT")
                                   }).ToList();
            return InputDao;
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return null;
        }
    }

    //public bool DeleteLoopRules(List<WFB2DA0100LoopRule> Items, bool OnTransaction, out string Message)
    //{
    //    try
    //    {
    //        int ChangeCount = 0;
    //        foreach (WFB2DA0100LoopRule item in Items)
    //        {
    //            StringBuilder sb = new StringBuilder();
    //            Hashtable ht = new Hashtable();

    //            sb.AppendLine(" delete from TB_9_M_COMM_D ");
    //            sb.AppendLine(" where MAIN_CD=@MAIN_CD ");
    //            sb.AppendLine("   and SUB_CD=@SUB_CD ");
    //            ht.Clear();
    //            ht.Add("@MAIN_CD", item.MAIN_CD);
    //            ht.Add("@SUB_CD", item.SUB_CD);
    //            if (OnTransaction)
    //                ChangeCount += dbConn.ExecuteT(sb, ht);
    //            else
    //                ChangeCount += dbConn.Execute(sb, ht);

    //        }
    //        if (ChangeCount == Items.Count)
    //        {
    //            Message = "";
    //            return true;
    //        }
    //        else
    //        {
    //            Message = Resources.Resource.wfb2da_DataIsChange;
    //            return false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Message = ex.Message;
    //        return false;
    //    }
    //}

    public List<WFB2DAEMP_DAY_DUTY> GetTB_D_M_EMP_DAY_DUTY(string CALENDAR_CD, DateTime CALENDAR_DT, bool OnTransaction, out string Message)
    {
        Message = string.Empty;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select EMP_ID,CALENDAR_DT ");
            sb.AppendLine(" from TB_D_M_EMP_DAY_DUTY ");
            sb.AppendLine(" where CALENDAR_CD=@CALENDAR_CD ");
            sb.AppendLine("   and CALENDAR_DT=@CALENDAR_DT ");
            ht.Add("@CALENDAR_CD", CALENDAR_CD);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            DataTable QueryDt;
            if (OnTransaction)
                QueryDt = dbConn.QueryT(sb, ht);
            else
                QueryDt = dbConn.Query(sb, ht);

            return (from item in QueryDt.AsEnumerable()
                    select new WFB2DAEMP_DAY_DUTY
                    {
                        CALENDAR_DT = item.Field<DateTime>("CALENDAR_DT"),
                        EMP_ID = (item.Table.Columns.Contains("EMP_ID") ? item.Field<string>("EMP_ID") : null),
                    }).ToList();
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            throw ex;
        }
    }

    internal DataTable getAll_CALENDAR_CD()
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CALENDAR_CD,CONVERT(varchar(10),CALENDAR_DT,111) CALENDAR_DT from TB_D_M_CALENDAR_D ");

            dt = dbConn.Query(sb, ht);

            return dt;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //internal void updateAll_TB_D_M_CALENDAR_D(DataTable calendar_d_table)
    //{
    //    try
    //    {
    //        string[] pno = new string[2];
    //        string[] sbval = new string[2];
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        for (int i = 0; i < calendar_d_table.Rows.Count; i++)
    //        {
                
    //        }
    //        sb.Append(" update TB_D_M_CALENDAR_D set ");
    //        sb.Append(" where 1=1 ");



    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}



    internal void updateAll_TB_D_M_CALENDAR_D(DataRow dataRow)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_D_M_CALENDAR_D set ");
            sb.Append(" WORK_DAY_CD=@WORK_DAY_CD,DT_TYPE=@DT_TYPE ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" where CALENDAR_CD=@CALENDAR_CD ");
            sb.Append("   and CALENDAR_DT=@CALENDAR_DT ");

            ht.Add("@CALENDAR_CD", dataRow["CALENDAR_CD"]);
            ht.Add("@CALENDAR_DT", dataRow["CALENDAR_DT"]);
            ht.Add("@WORK_DAY_CD", dataRow["WORK_DAY_CD"]);
            ht.Add("@DT_TYPE", dataRow["DT_TYPE"]);

            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DA010");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getCALENDAR_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select distinct CALENDAR_CD,CALENDAR_CD+'-'+CALENDAR_DESC CALENDAR_DESC ");
            sb.Append(" from TB_D_M_CALENDAR_H");
            sb.Append(" order by CALENDAR_CD");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal int SP_DA010_01(WFB2DA0100DAO dao)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_DA010_01");
            ht.Add("@p_START_DATE", dao.START_DATE);
            ht.Add("@p_END_DATE", dao.END_DATE);
            ht.Add("@p_CALENDAR_CD", dao.CALENDAR_CD.ToUpper());
            ht.Add("@p_UserID", SessionHandle.Current.emp_id);
            ht.Add("@p_FuncID", "FB2DA010");
            
            return dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable checkSP(string PROC_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select top (1) PROC_STATUS, PROC_LOG  ");
            sb.AppendLine("   from TB_SP_LOG                  ");
            sb.AppendLine("  where  PROC_ID = @PROC_ID            ");
            sb.AppendLine("  order by PROC_DT desc                ");
            ht.Add("@PROC_ID", PROC_ID);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

}