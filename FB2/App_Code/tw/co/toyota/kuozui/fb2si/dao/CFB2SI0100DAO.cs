using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// wfb2si010 的摘要描述
/// </summary>
public class CFB2SI0100DAO : BaseDAO
{
    public string BONUS_YEAR { get; set; }
    public string BONUS_ROUND { get; set; }
    public string BONUS_DAYS { get; set; }
    public string BONUS_DT { get; set; }
    public string BONUS_SDT { get; set; }
    public string BONUS_EDT { get; set; }
    public string BONUS_ITEM_RP { get; set; }
    public string BONUS_ITEM_AL { get; set; }
    public string BONUS_ITEM_D { get; set; }
    public string TARGET_GEN_DT { get; set; }
    public string BONUS_TOTAL_DECIMAL { get; set; }
    public string GEN_DT { get; set; }
    public string BONUS_TOTAL_AMOUNT { get; set; }
    public string RELEASE_DT { get; set; }
    public string RELEASE_BY { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string APPROVE_STATUS { get; set; }
    public string REMARK { get; set; }
    public string SALARY_TRANS_DT { get; set; }
    public string SALARY_TRANS_BY { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string SALARY_DT { get; set; }
    public string FREEZE_FLAG { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    public string EMP_ID { get; set; }
    public string BONUS_COUNT { get; set; }
    public string CHG_STATUS { get; set; }
    public string PRIMEVAL_FLAG { get; set; }
    public string APPROVE_FLAG { get; set; }
    public string PAY_TYPE { get; set; }
    public string WORK_DAYS { get; set; }
    public string ATTEND_DAYS { get; set; }
    public string BONUS_WORK_DAYS { get; set; }
    public string REWARD_DAYS { get; set; }
    public string DISCIPLINE_DAYS { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_CHG_CD { get; set; }
    public string WS_CD { get; set; }
    public string JPN_CD { get; set; }
    public string DEPT_NO { get; set; }
    public string LEVEL_CD { get; set; }
    public string PJOB_CD { get; set; }
    public string JOIN_DT { get; set; }
    public string LEAVE_DT { get; set; }
    public string STAY_DT { get; set; }
    public string BE_CONTRACT_DT { get; set; }
    public string BE_EMP_DT { get; set; }
    public string EMP_CD { get; set; }
    public DateTime now{ get; set; }

    public CFB2SI0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    #region 主檔
    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string bonus_year_s, string bonus_year_e, string bonus_sdt, string bonus_edt)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" BONUS_YEAR,BONUS_SDT,BONUS_EDT,BONUS_DT,TARGET_GEN_DT,RELEASE_DT,APPROVE_DT,APPROVE_STATUS,SALARY_TRANS_DT,BONUS_ROUND,GEN_DT,FREEZE_FLAG");
            sb.Append(" from TB_S_M_BONUS_H");
            sb.Append(" where 1=1 ");

            //紅利年度start
            if (bonus_year_s.Trim() != "")
            {
                sb.Append("and BONUS_YEAR >= @bonus_year_s ");
                ht.Add("@bonus_year_s", bonus_year_s);

            }
            if (bonus_year_e.Trim() != "")
            {
                sb.Append("and BONUS_YEAR <= @bonus_year_e ");
                ht.Add("@bonus_year_e", bonus_year_e);


            }
            if (bonus_sdt.Trim() != "")
            {
                sb.Append("and BONUS_DT >= @bonus_sdt ");
                ht.Add("@bonus_sdt", bonus_sdt.Replace("/", ""));


            }
            if (bonus_edt.Trim() != "")
            {
                sb.Append("and BONUS_DT <= @bonus_edt ");
                ht.Add("@bonus_edt", bonus_edt.Replace("/", ""));

            }

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount(int startRowIndex, int maximumRows, string bonus_year_s, string bonus_year_e, string bonus_sdt, string bonus_edt)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record From TB_S_M_BONUS_H");
            sb.Append(" where 1=1 ");

            //紅利年度start
            if (bonus_year_s.Trim() != "")
            {
                sb.Append("and BONUS_YEAR >= @bonus_year_s ");
                ht.Add("@bonus_year_s", bonus_year_s);

            }
            if (bonus_year_e.Trim() != "")
            {
                sb.Append("and BONUS_YEAR <= @bonus_year_e ");
                ht.Add("@bonus_year_e", bonus_year_e);


            }
            if (bonus_sdt.Trim() != "")
            {
                sb.Append("and BONUS_DT >= @bonus_sdt ");
                ht.Add("@bonus_sdt", bonus_sdt.Replace("/", ""));


            }
            if (bonus_edt.Trim() != "")
            {
                sb.Append("and BONUS_DT <= @bonus_edt ");
                ht.Add("@bonus_edt", bonus_edt.Replace("/", ""));

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
    //查詢現有資料
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_S_M_BONUS_H");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void Add_S_M_BONUS_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_M_BONUS_H (BONUS_YEAR,BONUS_ROUND,BONUS_SDT,BONUS_EDT,BONUS_DT,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@BONUS_YEAR,'1',@BONUS_SDT,@BONUS_EDT,@BONUS_DT,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@BONUS_SDT", BONUS_SDT);
            ht.Add("@BONUS_EDT", BONUS_EDT);
            ht.Add("@BONUS_DT", BONUS_DT);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
            // Commit();
        }
        catch (Exception)
        {
            //RollBack();
            throw;
        }
    }
    internal void Update_S_M_BONUS_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_BONUS_H ");
            sb.Append(" Set BONUS_DT = @BONUS_DT,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@BONUS_DT", BONUS_DT);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Delete_S_M_BONUS_H(string BONUS_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Delete From TB_S_M_BONUS_H ");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Delete_TB_S_R_BONUS_D(string BONUS_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Delete From TB_S_R_BONUS_D ");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Delete_TB_S_M_BONUS_D(string BONUS_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Delete From TB_S_M_BONUS_D ");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Delete_TB_S_S_BONUS_D(string BONUS_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Delete From TB_S_S_BONUS_D ");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    //呼叫紅利對象生成SP
    internal void execSP_S_BONUS_DATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_BONUS_DATA");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@BONUS_ROUND", BONUS_ROUND);
            ht.Add("@BONUS_DT", Convert.ToDateTime(BONUS_DT).ToString("yyyy/MM/dd"));
            ht.Add("@BONUS_SDT", Convert.ToDateTime(BONUS_SDT).ToString("yyyy/MM/dd"));
            ht.Add("@BONUS_EDT", Convert.ToDateTime(BONUS_EDT).ToString("yyyy/MM/dd"));
            ht.Add("@USERID", CREATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SI010");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }

    //薪資轉出
    internal void Announce_S_M_BONUS_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_BONUS_H ");
            sb.Append(" Set SALARY_TRANS_DT = GETDATE(),SALARY_TRANS_BY=@SALARY_TRANS_BY,FREEZE_FLAG=@FREEZE_FLAG,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@SALARY_TRANS_BY", SessionHandle.Current.emp_id);
            ht.Add("@FREEZE_FLAG", FREEZE_FLAG);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //提出核可
    internal void Release_S_M_BONUS_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_BONUS_H ");
            sb.Append(" Set RELEASE_DT = GETDATE(),RELEASE_BY=@RELEASE_BY,APPROVE_DT=@APPROVE_DT,");
            sb.Append(" APPROVE_BY=@APPROVE_BY,APPROVE_STATUS=@APPROVE_STATUS,FREEZE_FLAG=@FREEZE_FLAG,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@RELEASE_BY", SessionHandle.Current.emp_id);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@APPROVE_STATUS", APPROVE_STATUS);
            ht.Add("@FREEZE_FLAG", FREEZE_FLAG);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //確定調薪作業是否已完成
    public int CheckCount()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select count(0) as data_count from TB_S_M_SALARYSET_H");
            sb.AppendLine("where  Left(effect_YM,4)= YEAR(GETDATE())");
            sb.AppendLine("and APPROVE_STATUS ='Y'");
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["data_count"];
            }
            return t;

        }
        catch (Exception)
        {

            throw;
        }

    }
    #endregion
    

    #region 明細
    //查詢明細表頭部分
    public void GetDtlData()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select BONUS_DAYS,BONUS_DT,BONUS_TOTAL_AMOUNT,BONUS_TOTAL_DECIMAL,SALARY_TRANS_DT,APPROVE_STATUS,REMARK,FREEZE_FLAG from TB_S_M_BONUS_H");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                this.BONUS_DAYS = Convert.ToString(dr["BONUS_DAYS"]);
                this.BONUS_DT = Convert.ToString(dr["BONUS_DT"]);
                this.BONUS_TOTAL_AMOUNT = Convert.ToString(dr["BONUS_TOTAL_AMOUNT"]);
                this.BONUS_TOTAL_DECIMAL = Convert.ToString(dr["BONUS_TOTAL_DECIMAL"]);
                this.SALARY_TRANS_DT = Convert.ToString(dr["SALARY_TRANS_DT"]);
                this.APPROVE_STATUS = Convert.ToString(dr["APPROVE_STATUS"]);
                this.REMARK = Convert.ToString(dr["REMARK"]);
                this.FREEZE_FLAG = Convert.ToString(dr["FREEZE_FLAG"]);
            }

        }
        catch
        {
            throw;
        }

    }
    //取得紅利的開始及結束日期
    public void getSatrtAndEndDT(string bonus_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select  a.*   ");
            sb.Append(" from TB_S_M_BONUS_H a ");
            sb.Append(" where 1=1 ");
            if (bonus_year != "")
            {
                sb.Append(" and BONUS_YEAR = @BONUS_YEAR ");
                ht.Add("@BONUS_YEAR", bonus_year);
            }

            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                this.BONUS_SDT = dr["BONUS_SDT"].ToString() != "" ? Convert.ToDateTime(dr["BONUS_SDT"].ToString()).ToString("yyyy/MM/dd") : "";
                this.BONUS_EDT = dr["BONUS_EDT"].ToString() != "" ? Convert.ToDateTime(dr["BONUS_EDT"].ToString()).ToString("yyyy/MM/dd") : "";
            }


        }
        catch
        {
            throw;
        }

    }



    //查詢明細
    public DataTable GetDtlData2(int startRowIndex, int maximumRows, string sortExpression, string bonus_year
                                , string emp_id, string emp_name, string emp_chg_cd, string level_cd, string pay_status)
    {
        try
        {
            if (sortExpression.Contains("APPROVE_MARK DESC,APPROVE_FLAG,UPDATED_DT DESC,WS_CD,EMP_ID"))
                sortExpression = sortExpression.Replace("UPDATED_DT", "a.UPDATED_DT");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine("       a.APPROVE_MARK,a.CHG_STATUS+'-'+c.SUB_DESC as CHG_STATUS,a.APPROVE_FLAG,a.APPROVE_FLAG+'-'+b.SUB_DESC as APPROVE_FLAG_DESC");
            sb.AppendLine("      ,a.WS_CD,a.WS_CD+'-'+e.SUB_DESC as WS_CD_DESC,a.LEVEL_CD,a.EMP_ID,a.EMP_NAME,a.WORK_DAYS,a.ATTEND_DAYS              ");
	        sb.AppendLine("      ,a.BONUS_WORK_DAYS,a.REWARD_DAYS,a.DISCIPLINE_DAYS,a.BONUS_AMT,a.PAY_TYPE+'-'+f.SUB_DESC as PAY_TYPE   ");
            sb.AppendLine("      ,a.EMP_CHG_CD+'-'+d.SUB_DESC as EMP_CHG_CD,a.UPDATED_DT                                                ");
            sb.AppendLine("from TB_S_M_BONUS_D a                                                                                        ");
            sb.AppendLine("left join TB_9_M_COMM_D b on b.SYS_CD='SA' and b.MAIN_CD='APPROVE_STATUS' and b.SUB_CD=a.APPROVE_FLAG        ");
            sb.AppendLine("left join TB_9_M_COMM_D c on c.SYS_CD='SA' and c.MAIN_CD='CHG_STATUS' and c.SUB_CD=a.CHG_STATUS              ");
            sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CHG_CD' and d.SUB_CD=a.EMP_CHG_CD              ");
            sb.AppendLine("left join TB_9_M_COMM_D e on e.SYS_CD='HB' and e.MAIN_CD='WS_CD' and e.SUB_CD=a.WS_CD                        ");
            sb.AppendLine("left join TB_9_M_COMM_D f on f.SYS_CD='SC' and f.MAIN_CD='PAY_TYPE' and f.SUB_CD=a.PAY_TYPE                  ");
            sb.AppendLine(" where a.BONUS_YEAR = @BONUS_YEAR");

            if (emp_id != "")
            {
                sb.AppendLine(" and a.EMP_ID like '" + @emp_id + "%' ");
                ht.Add("@emp_id", emp_id);
            }
            if (emp_name != "")
            {
                sb.AppendLine(" and a.EMP_NAME like '" + @emp_name + "%'  ");
                ht.Add("@emp_name", emp_name);
            }
            if (emp_chg_cd != "-1" && emp_chg_cd != null)
            {
                sb.AppendLine(" and a.EMP_CHG_CD = @emp_chg_cd ");
                ht.Add("@emp_chg_cd", emp_chg_cd);
            }
            if (level_cd != "")
            {
                sb.AppendLine(" and a.LEVEL_CD like '" + @level_cd + "%'  ");
                ht.Add("@level_cd", level_cd);
            }
            if (pay_status != "-1" && pay_status != null)
            {
                sb.AppendLine(" and a.PAY_TYPE = @pay_status ");
                ht.Add("@pay_status", pay_status);
            }

            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");

            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");


            ht.Add("@BONUS_YEAR", bonus_year);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetDtlCount(int startRowIndex, int maximumRows, string bonus_year, string emp_id, string emp_name, string emp_chg_cd, string level_cd, string pay_status)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record From TB_S_M_BONUS_D");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            if (emp_id != "")
            {
                sb.Append(" and EMP_ID like '" + @emp_id + "%' ");
                ht.Add("@emp_id", emp_id);
            }
            if (emp_name != "")
            {
                sb.Append(" and EMP_NAME like '" + @emp_name + "%'  ");
                ht.Add("@emp_name", emp_name);
            }
            if (emp_chg_cd != "-1" && emp_chg_cd != null)
            {
                sb.Append(" and EMP_CHG_CD = @emp_chg_cd ");
                ht.Add("@emp_chg_cd", emp_chg_cd);
            }
            if (level_cd != "")
            {
                sb.Append(" and LEVEL_CD like '" + @level_cd + "%'  ");
                ht.Add("@level_cd", level_cd);
            }
            if (pay_status != "-1" && pay_status != null)
            {
                sb.Append(" and PAY_TYPE = @pay_status ");
                ht.Add("@pay_status", pay_status);
            }
            ht.Add("@BONUS_YEAR", bonus_year);
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
    //刪除明細
    internal void Delete_S_M_BONUS_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_BONUS_D ");
            sb.Append(" Set CHG_STATUS = @CHG_STATUS,PRIMEVAL_FLAG=@PRIMEVAL_FLAG,APPROVE_FLAG=@APPROVE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = @UPDATED_DT,FUNC_ID = @FUNC_ID");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR and EMP_ID=@EMP_ID");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@CHG_STATUS", CHG_STATUS);
            ht.Add("@PRIMEVAL_FLAG", PRIMEVAL_FLAG);
            ht.Add("@APPROVE_FLAG", APPROVE_FLAG);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

            sb.Append("Update TB_S_M_BONUS_H ");
            sb.Append(" Set BONUS_TOTAL_AMOUNT =");
            sb.Append(" (select sum(BONUS_AMT) from TB_S_M_BONUS_D ");
            sb.Append(" where CHG_STATUS <>'D' and BONUS_YEAR = @BONUS_YEAR),");
            sb.Append(" BONUS_TOTAL_DECIMAL =(");
            sb.Append(" select COUNT(*) from TB_S_M_BONUS_D");
            sb.Append(" where CHG_STATUS <>'D' and BONUS_YEAR = @BONUS_YEAR),");
            sb.Append(" RELEASE_DT=@RELEASE_DT,RELEASE_BY=@RELEASE_BY,APPROVE_DT=@APPROVE_DT,");
            sb.Append(" APPROVE_BY=@APPROVE_BY,FREEZE_FLAG =@FREEZE_FLAG,");
            sb.Append(" APPROVE_STATUS = CASE ");
            sb.Append(" when APPROVE_STATUS='B'   then  'B' ");
            sb.Append(" ELSE 'N' ");
            sb.Append(" END,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = @UPDATED_DT,FUNC_ID = @FUNC_ID");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            if (RELEASE_DT == "")
                ht.Add("@RELEASE_DT", DBNull.Value);
            else
                ht.Add("@RELEASE_DT", RELEASE_DT);

            if (APPROVE_DT == "")
                ht.Add("@APPROVE_DT", DBNull.Value);
            else
                ht.Add("@APPROVE_DT", APPROVE_DT);

            ht.Add("@RELEASE_BY", RELEASE_BY);
            ht.Add("@APPROVE_BY", APPROVE_BY);
            //ht.Add("@APPROVE_STATUS", APPROVE_STATUS);
            ht.Add("@FREEZE_FLAG", FREEZE_FLAG);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //支付狀態一括更新
    internal void Status_S_M_BONUS_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_BONUS_D ");
            sb.Append(" Set CHG_STATUS = @CHG_STATUS,PAY_TYPE=@PAY_TYPE,PRIMEVAL_FLAG=@PRIMEVAL_FLAG,APPROVE_FLAG=@APPROVE_FLAG,UPDATED_BY = @UPDATED_BY,UPDATED_DT = @UPDATED_DT,FUNC_ID = @FUNC_ID");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR and EMP_ID=@EMP_ID");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@CHG_STATUS", CHG_STATUS);
            ht.Add("@PAY_TYPE", PAY_TYPE);
            ht.Add("@PRIMEVAL_FLAG", PRIMEVAL_FLAG);
            ht.Add("@APPROVE_FLAG", APPROVE_FLAG);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);


            sb.Append("Update TB_S_M_BONUS_H ");
            sb.Append(" Set BONUS_TOTAL_AMOUNT =");
            sb.Append(" (select sum(BONUS_AMT) from TB_S_M_BONUS_D ");
            sb.Append(" where CHG_STATUS <>'D' and BONUS_YEAR = @BONUS_YEAR),");
            sb.Append(" BONUS_TOTAL_DECIMAL =(");
            sb.Append(" select COUNT(*) from TB_S_M_BONUS_D");
            sb.Append(" where CHG_STATUS <>'D' and BONUS_YEAR = @BONUS_YEAR),");
            sb.Append(" RELEASE_DT=@RELEASE_DT,RELEASE_BY=@RELEASE_BY,APPROVE_DT=@APPROVE_DT,");
            sb.Append(" APPROVE_BY=@APPROVE_BY,FREEZE_FLAG =@FREEZE_FLAG,");
            sb.Append(" APPROVE_STATUS = CASE ");
            sb.Append(" when APPROVE_STATUS='B'   then  'B' ");
            sb.Append(" ELSE 'N' ");
            sb.Append(" END,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = @UPDATED_DT,FUNC_ID = @FUNC_ID");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            if (RELEASE_DT == "")
                ht.Add("@RELEASE_DT", DBNull.Value);
            else
                ht.Add("@RELEASE_DT", RELEASE_DT);

            if (APPROVE_DT == "")
                ht.Add("@APPROVE_DT", DBNull.Value);
            else
                ht.Add("@APPROVE_DT", APPROVE_DT);

            ht.Add("@RELEASE_BY", RELEASE_BY);
            ht.Add("@APPROVE_BY", APPROVE_BY);
            //ht.Add("@APPROVE_STATUS", APPROVE_STATUS);
            ht.Add("@FREEZE_FLAG", FREEZE_FLAG);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //excel下載資料table
    public DataTable getExcelData(string data, string BONUS_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (data == "mantain")
            {
                sb.AppendLine("Select * ,a.EMP_CHG_CD+'-'+c.sub_desc as EMP_CHG_CD_desc                                       ");
	            sb.AppendLine("      ,a.EMP_CD+'-'+d.sub_desc as EMP_CD_desc,a.CHG_STATUS+'-'+e.sub_desc as CHG_STATUS_desc   ");
                sb.AppendLine("from TB_S_M_BONUS_D a                                                                          ");
                sb.AppendLine("left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='EMP_CHG_CD' and c.SUB_CD=a.EMP_CHG_CD");
                sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CD' and d.SUB_CD=a.EMP_CD        ");
                sb.AppendLine("left join TB_9_M_COMM_D e on e.SYS_CD='SA' and e.MAIN_CD='CHG_STATUS' and e.SUB_CD=a.CHG_STATUS");
                sb.AppendLine("where a.BONUS_YEAR = @BONUS_YEAR");
            }
            else if (data == "original")
            {
                sb.AppendLine("Select * ,a.EMP_CHG_CD+'-'+c.sub_desc as EMP_CHG_CD_desc                                       ");
                sb.AppendLine("      ,a.EMP_CD+'-'+d.sub_desc as EMP_CD_desc,a.CHG_STATUS+'-'+e.sub_desc as CHG_STATUS_desc   ");
                sb.AppendLine("from TB_S_S_BONUS_D a                                                                          ");
                sb.AppendLine("left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='EMP_CHG_CD' and c.SUB_CD=a.EMP_CHG_CD");
                sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CD' and d.SUB_CD=a.EMP_CD        ");
                sb.AppendLine("left join TB_9_M_COMM_D e on e.SYS_CD='SA' and e.MAIN_CD='CHG_STATUS' and e.SUB_CD=a.CHG_STATUS");
                sb.AppendLine("where a.BONUS_YEAR = @BONUS_YEAR");
            }

            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //紅利維護檔.紅利反映項目取值
    public DataTable Get_TB_S_M_BONUS_H(string BONUS_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select BONUS_ITEM_RP,BONUS_ITEM_AL,BONUS_ITEM_D,BONUS_DAYS,CONVERT(VARCHAR(10),BONUS_SDT,111) BONUS_SDT,CONVERT(VARCHAR(10),BONUS_EDT,111) BONUS_EDT  from TB_S_M_BONUS_H");
            sb.AppendLine("where BONUS_YEAR=@BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable check_EMP_ID(string BONUS_YEAR, string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_S_M_BONUS_D");
            sb.Append(" where BONUS_YEAR=@BONUS_YEAR and EMP_ID = @EMP_ID");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //檢查工號是否在VW_H_EMP_DATA
    public DataTable check_EMP_ID2(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from VW_H_EMP_DATA");
            sb.Append(" where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //依工號取得身份標示
    internal DataTable getID_DESC(string EMP_ID, String start_DT, String end_DT)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select dbo.FN_S_AWARD_ID_DESC(@EMP_ID,@start_DT,@end_DT) as ID_DESC  ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@start_DT", start_DT);
            ht.Add("@end_DT", end_DT);
            dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {

            throw;
        }
    }
    //將view資料放進cell
    public DataTable viewToCell(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select  EMP_NAME, EMP_CHG_CD, WS_CD, JPN_CD, DEPT_NO, LEVEL_CD, PJOB_CD, JOIN_DT,");
            sb.Append(" LEAVE_DT, EMP_STATUS, BE_CONTRACT_DT, BE_EMP_DT, EMP_CD");
            sb.Append(" from VW_H_EMP_DATA");
            sb.Append(" where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //上傳新增
    public void Add(string BONUS_YEAR, string BONUS_DT, 
                    string cell1, string cell2, string cell3, string cell4, string cell5, string cell6, string cell7, string cell8, string cell9, string cell10,
                    string cell11, string cell12, string cell13, string cell14, string cell15, string cell16, string cell17, string cell18, string cell19, string cell20,
                    string cell21, string cell22, string cell23, string cell24, string cell25, string cell26, string cell27, string cell28, string cell29, string cell30,
                    string cell31, string cell32,decimal WK_ATTEND_DAYS,decimal WK_REWARD_DAYS,decimal WK_DISCIPLINE_DAYS,decimal WK_BONUS_WORK_DAYS, string cell37,DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" declare @wk_ATTEND_DAYS decimal(6,3) =  -1*dbo.FN_S_GET_ATTEND_DAYS_BONUS(@EMP_ID,@EMP_CD,@BONUS_SDT,@BONUS_EDT) ");    
            sb.Append("INSERT INTO TB_S_M_BONUS_D (BONUS_YEAR,BONUS_COUNT,BONUS_DT,EMP_ID,");
            sb.Append(" EMP_NAME,EMP_CHG_CD,WS_CD,JPN_CD,DEPT_NO,LEVEL_CD,PJOB_CD,JOIN_DT,");
            sb.Append(" LEAVE_DT,STAY_DT,BE_CONTRACT_DT,BE_EMP_DT,WORK_DAYS,EMP_CD,ID_DESC,");
            sb.Append(" LEVEL_PAY,ABILITY_PAY,PJOB_PAY,PROFESSION_PAY,FOOD_SUBSIDY,LEAVE_A_HOUR,");
            sb.Append(" LEAVE_B_HOUR,LEAVE_C_HOUR,LEAVE_Q_HOUR,LEAVE_OP_HOUR,THIRD_CNT_P,SECOND_CNT_P,");
            sb.Append(" FIRST_CNT_P,THIRD_CNT_M,SECOND_CNT_M,FIRST_CNT_M,");
            sb.Append(" ATTEND_DAYS,REWARD_DAYS,DISCIPLINE_DAYS,BONUS_WORK_DAYS,");
            sb.Append(" PAY_TYPE,CHG_STATUS,PRIMEVAL_FLAG,APPROVE_FLAG,APPROVE_MARK,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@BONUS_YEAR,'1',@BONUS_DT,@EMP_ID,");
            sb.Append(" @EMP_NAME,@EMP_CHG_CD,@WS_CD,@JPN_CD,@DEPT_NO,@LEVEL_CD,@PJOB_CD,@JOIN_DT,");
            sb.Append(" @LEAVE_DT,@STAY_DT,@BE_CONTRACT_DT,@BE_EMP_DT,@WORK_DAYS,@EMP_CD,@ID_DESC,");
            sb.Append(" @LEVEL_PAY,@ABILITY_PAY,@PJOB_PAY,@PROFESSION_PAY,@FOOD_SUBSIDY,@LEAVE_A_HOUR,");
            sb.Append(" @LEAVE_B_HOUR,@LEAVE_C_HOUR,@LEAVE_Q_HOUR,@LEAVE_OP_HOUR,@THIRD_CNT_P,@SECOND_CNT_P,");
            sb.Append(" @FIRST_CNT_P,@THIRD_CNT_M,@SECOND_CNT_M,@FIRST_CNT_M,");
            sb.Append(" @wk_ATTEND_DAYS ,@REWARD_DAYS,@DISCIPLINE_DAYS,@BONUS_WORK_DAYS+ @wk_ATTEND_DAYS ,");
            sb.Append(" @PAY_TYPE,@CHG_STATUS,@PRIMEVAL_FLAG,@APPROVE_FLAG,@APPROVE_MARK,@CREATED_BY,GETDATE(),@UPDATED_BY,@UPDATED_DT,@FUNC_ID)");

            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@BONUS_SDT", BONUS_SDT);
            ht.Add("@BONUS_EDT", BONUS_EDT);
            ht.Add("@BONUS_DT", Convert.ToDateTime(BONUS_DT));
            ht.Add("@EMP_ID", cell1);
            ht.Add("@EMP_NAME", cell2);
            ht.Add("@EMP_CHG_CD", cell3);
            ht.Add("@WS_CD", cell4);
            ht.Add("@JPN_CD", cell5);
            ht.Add("@DEPT_NO", cell6);
            ht.Add("@LEVEL_CD", cell7);
            ht.Add("@PJOB_CD", cell8);
            if (cell9!="")
                ht.Add("@JOIN_DT", Convert.ToDateTime(cell9));
            else
                ht.Add("@JOIN_DT", DBNull.Value);
            if (cell10 != "")
                ht.Add("@LEAVE_DT",Convert.ToDateTime(cell10) );
            else
                ht.Add("@LEAVE_DT", DBNull.Value);
            if (cell11 != "")
                ht.Add("@STAY_DT",Convert.ToDateTime(cell11) );
            else
                ht.Add("@STAY_DT", DBNull.Value);
            if (cell12 != "")
                ht.Add("@BE_CONTRACT_DT",Convert.ToDateTime(cell12) );
            else
                ht.Add("@BE_CONTRACT_DT", DBNull.Value);
            if (cell13 != "")
                ht.Add("@BE_EMP_DT",Convert.ToDateTime(cell13) );
            else
                ht.Add("@BE_EMP_DT", DBNull.Value);
            ht.Add("@WORK_DAYS", cell14);
            ht.Add("@EMP_CD", cell15);
            ht.Add("@ID_DESC", cell16);
            ht.Add("@ABILITY_PAY", cell17.Replace(",", ""));
            ht.Add("@LEVEL_PAY", cell18.Replace(",", ""));
            ht.Add("@PJOB_PAY", cell19.Replace(",", ""));
            ht.Add("@PROFESSION_PAY", cell20.Replace(",", ""));
            ht.Add("@FOOD_SUBSIDY", cell21.Replace(",", ""));
            ht.Add("@LEAVE_A_HOUR", cell22.Replace(",", ""));
            ht.Add("@LEAVE_B_HOUR", cell23.Replace(",", ""));
            ht.Add("@LEAVE_C_HOUR", cell24.Replace(",", ""));
            ht.Add("@LEAVE_Q_HOUR", cell25.Replace(",", ""));
            ht.Add("@LEAVE_OP_HOUR", cell26.Replace(",", ""));
            ht.Add("@THIRD_CNT_P", cell27);
            ht.Add("@SECOND_CNT_P", cell28);
            ht.Add("@FIRST_CNT_P", cell29);
            ht.Add("@THIRD_CNT_M", cell30);
            ht.Add("@SECOND_CNT_M", cell31);
            ht.Add("@FIRST_CNT_M", cell32);
            //ht.Add("@ATTEND_DAYS", WK_ATTEND_DAYS);
            ht.Add("@REWARD_DAYS", WK_REWARD_DAYS);
            ht.Add("@DISCIPLINE_DAYS", WK_DISCIPLINE_DAYS);
            ht.Add("@BONUS_WORK_DAYS", WK_BONUS_WORK_DAYS);
            ht.Add("@PAY_TYPE", cell37);
            ht.Add("@CHG_STATUS", "N");
            ht.Add("@PRIMEVAL_FLAG", "");
            ht.Add("@APPROVE_FLAG", "N");
            ht.Add("@APPROVE_MARK", "");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_DT", Convert.ToDateTime(now));
            ht.Add("@FUNC_ID", "FB2SI010");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //修改前取值
    public DataTable premodify(string BONUS_YEAR, string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select  CHG_STATUS, APPROVE_FLAG, PRIMEVAL_FLAG");
            sb.Append(" from TB_S_M_BONUS_D");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR and EMP_ID=@EMP_ID");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //修改前取值2
    public DataTable premodify2(string BONUS_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select  APPROVE_STATUS");
            sb.Append(" from TB_S_M_BONUS_H");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //上傳修改
    public void modify(string CHG_STATUS, string APPROVE_FLAG, string PRIMEVAL_FLAG, string APPROVE_STATUS, string BONUS_YEAR, string cell1, string cell14, 
                       string cell17, string cell18, string cell19, string cell20, string cell21, string cell22, string cell23, string cell24, string cell25, string cell26,
                       string cell27, string cell28, string cell29, string cell30, string cell31, string cell32, decimal WK_ATTEND_DAYS, decimal WK_REWARD_DAYS,
                       decimal WK_DISCIPLINE_DAYS, decimal WK_BONUS_WORK_DAYS, string cell37,DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" declare @wk_ATTEND_DAYS decimal(6,3) =  -1*dbo.FN_S_GET_ATTEND_DAYS_BONUS(@EMP_ID,@EMP_CD,@BONUS_SDT,@BONUS_EDT) ");    
            sb.AppendLine("Update TB_S_M_BONUS_D ");
            sb.AppendLine(" Set WORK_DAYS = @WORK_DAYS,LEVEL_PAY=@LEVEL_PAY,ABILITY_PAY=@ABILITY_PAY,");
            sb.AppendLine(" PJOB_PAY=@PJOB_PAY,PROFESSION_PAY=@PROFESSION_PAY,FOOD_SUBSIDY=@FOOD_SUBSIDY,LEAVE_A_HOUR=@LEAVE_A_HOUR,");
            sb.AppendLine(" LEAVE_B_HOUR=@LEAVE_B_HOUR,LEAVE_C_HOUR=@LEAVE_C_HOUR,LEAVE_Q_HOUR=@LEAVE_Q_HOUR,APPROVE_MARK=@APPROVE_MARK,");
            sb.AppendLine(" LEAVE_OP_HOUR=@LEAVE_OP_HOUR,THIRD_CNT_P=@THIRD_CNT_P,SECOND_CNT_P=@SECOND_CNT_P,FIRST_CNT_P=@FIRST_CNT_P,");
            sb.AppendLine(" THIRD_CNT_M=@THIRD_CNT_M,SECOND_CNT_M=@SECOND_CNT_M,FIRST_CNT_M=@FIRST_CNT_M,PAY_TYPE=@PAY_TYPE,");
            sb.AppendLine(" ATTEND_DAYS=@wk_ATTEND_DAYS,REWARD_DAYS=@REWARD_DAYS ,DISCIPLINE_DAYS=@DISCIPLINE_DAYS,BONUS_WORK_DAYS=@BONUS_WORK_DAYS  + @wk_ATTEND_DAYS,");
            sb.AppendLine(" CREATED_BY=@CREATED_BY,CREATED_DT=GETDATE(),UPDATED_BY=@UPDATED_BY,UPDATED_DT=@UPDATED_DT,FUNC_ID=@FUNC_ID");

            if (CHG_STATUS == "N" && APPROVE_FLAG == "N")
            {
                sb.Append(" ,CHG_STATUS='N'");
            }
            else
            {
                sb.Append(" ,CHG_STATUS='U'");
            }

            if (PRIMEVAL_FLAG == "N")
            {
                sb.Append(" ,PRIMEVAL_FLAG='Y'");
            }
            if (APPROVE_STATUS == "Y")
            {
                sb.Append(" ,APPROVE_FLAG='N'");
            }

            sb.Append(" where BONUS_YEAR = @BONUS_YEAR and EMP_ID=@EMP_ID");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@BONUS_SDT", BONUS_SDT);
            ht.Add("@BONUS_EDT", BONUS_EDT);
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@EMP_ID", cell1);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@WORK_DAYS", cell14);
            //2021.07.27 fix
            ht.Add("@LEVEL_PAY", cell18.Replace(",", ""));
            ht.Add("@ABILITY_PAY", cell17.Replace(",", ""));
            ht.Add("@PJOB_PAY", cell19.Replace(",", ""));
            ht.Add("@PROFESSION_PAY", cell20.Replace(",", ""));
            ht.Add("@FOOD_SUBSIDY", cell21.Replace(",", ""));
            ht.Add("@LEAVE_A_HOUR", cell22.Replace(",", ""));
            ht.Add("@LEAVE_B_HOUR", cell23.Replace(",", ""));
            ht.Add("@LEAVE_C_HOUR", cell24.Replace(",", ""));
            ht.Add("@LEAVE_Q_HOUR", cell25.Replace(",", ""));
            ht.Add("@LEAVE_OP_HOUR", cell26.Replace(",", ""));
            ht.Add("@THIRD_CNT_P", cell27);
            ht.Add("@SECOND_CNT_P", cell28);
            ht.Add("@FIRST_CNT_P", cell29);
            ht.Add("@THIRD_CNT_M", cell30);
            ht.Add("@SECOND_CNT_M", cell31);
            ht.Add("@FIRST_CNT_M", cell32);
            ht.Add("@PAY_TYPE", cell37);
            ht.Add("@ATTEND_DAYS", WK_ATTEND_DAYS);
            ht.Add("@REWARD_DAYS", WK_REWARD_DAYS);
            ht.Add("@DISCIPLINE_DAYS", WK_DISCIPLINE_DAYS);
            ht.Add("@BONUS_WORK_DAYS", WK_BONUS_WORK_DAYS);
            ht.Add("@APPROVE_MARK", "");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", "FB2SI010");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新紅利維護檔
    public void update(string BONUS_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_BONUS_H ");
            sb.Append(" Set BONUS_TOTAL_AMOUNT =");
            sb.Append(" (select sum(BONUS_AMT) from TB_S_M_BONUS_D ");
            sb.Append(" where CHG_STATUS <>'D' and BONUS_YEAR = @BONUS_YEAR),");
            sb.Append(" BONUS_TOTAL_DECIMAL =(");
            sb.Append(" select COUNT(*) from TB_S_M_BONUS_D");
            sb.Append(" where CHG_STATUS <>'D' and BONUS_YEAR = @BONUS_YEAR),");
            sb.Append(" RELEASE_DT=@RELEASE_DT,RELEASE_BY=@RELEASE_BY,APPROVE_DT=@APPROVE_DT,GEN_DT = @GEN_DT, ");
            sb.Append(" APPROVE_BY=@APPROVE_BY,FREEZE_FLAG =@FREEZE_FLAG,");
            sb.Append(" APPROVE_STATUS = CASE ");
            sb.Append(" when APPROVE_STATUS='B'   then  'B' ");
            sb.Append(" ELSE 'N' ");
            sb.Append(" END,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@RELEASE_DT", DBNull.Value);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@GEN_DT", DBNull.Value);
            ht.Add("@RELEASE_BY", "");
            ht.Add("@APPROVE_BY", "");
            //ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@FREEZE_FLAG", "N");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SI010");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //更新明細維護檔的年獎金額為0
    public void updateT0Zero_D(string tableName,string BONUS_YEAR,DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update  "+tableName+" ");
            sb.Append(" set  ");
            sb.Append("  BONUS_AMT = @BONUS_AMT ");
            sb.Append(" ,BONUS_TAX = @BONUS_TAX ");
            sb.Append(" ,BONUS_AMT_R = @BONUS_AMT_R ");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR ");
            sb.Append("  and BONUS_COUNT = @BONUS_COUNT");

            //set值
            ht.Add("@BONUS_AMT", 0);
            ht.Add("@BONUS_TAX", 0);
            ht.Add("@BONUS_AMT_R", 0);

            //PK值
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@BONUS_COUNT", "1");


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    #endregion
    
}