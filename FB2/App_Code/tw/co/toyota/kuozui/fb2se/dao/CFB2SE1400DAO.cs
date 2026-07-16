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
/// CFB2SE1400DAO 的摘要描述
/// </summary>
public class CFB2SE1400DAO : BaseDAO
{

    public string EMP_ID { get; set; }
    public string EFFECT_YM { get; set; }
    public string EMP_NAME { get; set; }
    public string RELEASE_DT { get; set; }
    public string REMARK { get; set; }
    public string SUB_DESC { get; set; }
    public string APPROVE_NAME { get; set; }
    public string APPROVE_DT { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    public string APPROVE_STATUS { get; set; }
    public string LEVEL_CD { get; set; }
    public string GRADE_CD { get; set; }
    public CFB2SE1400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //Gridview 查詢資料
    public DataTable getData_1(int startRowIndex, int maximumRows, string sortExpression, string EFFECT_YM)
    {
        try
        {

            sortExpression = string.Format("a.{0}", sortExpression);
            if (sortExpression.Contains("a.RELEASE_NAME"))
            {
                sortExpression = sortExpression.Replace("a.RELEASE_NAME", "a.RELEASE_BY");
            }
            if (sortExpression.Contains("a.sub_desc"))
            {
                sortExpression = sortExpression.Replace("a.sub_desc", "d.SUB_DESC");
            }
            if (sortExpression.Contains("a.APPROVE_NAME"))
            {
                sortExpression = sortExpression.Replace("a.APPROVE_NAME", "e.EMP_NAME");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.* , a.RELEASE_BY + '-' + b.EMP_NAME AS RELEASE_NAME,d.SUB_CD + '-' + d.SUB_DESC AS SUB_DESC,e.EMP_NAME AS APPROVE_NAME									");
            sb.Append(" from TB_S_M_SALARY_ADJ_H a");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID = a.RELEASE_BY");
            sb.Append(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='SA' and d.MAIN_CD='APPROVE_STATUS' and d.sub_cd=a.APPROVE_STATUS");
            sb.Append(" left join TB_H_M_EMP e on e.EMP_ID = a.APPROVE_BY");
            //sb.Append(" where isnull(a.RELEASE_BY,'')<>''");
            
            sb.Append(" where c.EMP_ID = @LOGIN_ID  and  isnull(a.RELEASE_BY,'')<>''");
            ht.Add("@LOGIN_ID", SessionHandle.Current.emp_id);
            
            //sb.Append(" where c.EMP_ID = '13316' and a.APPROVE_STATUS = @APPROVE_STATUS and  isnull(a.RELEASE_BY,'')<>''");
            //ht.Add("@APPROVE_STATUS", "Y");
            
            //有年月
            if (!string.IsNullOrEmpty(EFFECT_YM))
            {
                sb.Append(" and a.EFFECT_YM = @EFFECT_YM ");
                ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
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
    public int getCount_1(int startRowIndex, int maximumRows, string EFFECT_YM)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_SALARY_ADJ_H a");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID = a.RELEASE_BY");
            sb.Append(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='SA' and d.MAIN_CD='APPROVE_STATUS' and d.sub_cd=a.APPROVE_STATUS");
            //sb.Append(" where isnull(a.RELEASE_BY,'')<>''");
            
            sb.Append(" where c.EMP_ID =  @LOGIN_ID  and  isnull(a.RELEASE_BY,'')<>''");
            ht.Add("@LOGIN_ID", SessionHandle.Current.emp_id);

            //sb.Append(" where c.EMP_ID = '13316' and a.APPROVE_STATUS = @APPROVE_STATUS and  isnull(a.RELEASE_BY,'')<>''");
            //ht.Add("@APPROVE_STATUS", "Y");
            
            //有年月
            if (!string.IsNullOrEmpty(EFFECT_YM))
            {
                sb.Append(" and a.EFFECT_YM = @EFFECT_YM ");
                ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
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
    internal DataTable getExistData(string LEVEL_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_H_M_LEVEL where LEVEL_CD = @LEVEL_CD");

            ht.Add("@LEVEL_CD", LEVEL_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string qdatakey)
    {
        try
        {

            sortExpression = string.Format("a.{0}", sortExpression);

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.* , a.RELEASE_BY + '-' + b.EMP_NAME AS RELEASE_NAME, d.SUB_CD + '-' + d.SUB_DESC AS SUB_DESC,e.EMP_NAME AS APPROVE_NAME									");
            sb.Append(" from TB_S_M_SALARY_ADJ_H a");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID = a.RELEASE_BY");
            sb.Append(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='SA' and d.MAIN_CD='APPROVE_STATUS' and d.sub_cd=a.APPROVE_STATUS");
            sb.Append(" left join TB_H_M_EMP e on e.EMP_ID = a.APPROVE_BY");
            sb.Append(" where 1 = 1");
            //sb.Append(" where c.EMP_ID = '10067' and a.APPROVE_STATUS = @APPROVE_STATUS and  isnull(a.RELEASE_BY,'')<>''");
            //ht.Add("@APPROVE_STATUS", APPROVE_STATUS);
            //有年月
            if (!string.IsNullOrEmpty(qdatakey))
            {
                sb.Append(" and a.EFFECT_YM = @EFFECT_YM ");
                ht.Add("@EFFECT_YM", qdatakey);
            }
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht,true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData1(int startRowIndex, int maximumRows, string sortExpression, string qdatakey, string emp_id, string emp_name)
    {
        try
        {
            sortExpression = string.Format("a.{0}", sortExpression);
            if (sortExpression.Contains("a.EMP_NAME"))
            {
                sortExpression = sortExpression.Replace("a.EMP_NAME", "c.EMP_NAME");
            }
            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber");
            sb.Append(" , a.EFFECT_YM, a.EMP_ID, a.JPN_CD, a.DEPT_NO, a.LEVEL_CD, a.GRADE_CD, a.PJOB_CD, a.LEVEL_PAY_OLD, a.ABILITY_PAY_OLD, a.EXAMINE_ADJ");
            sb.Append(" , a.LEVEL_ADJ, a.ABILITY_ADJ, a.LEVEL_PAY_NEW, a.ABILITY_PAY_NEW, a.LEVEL_PAY_DIFF, a.NOPAYDIFF_YN, a.THIS_YEAR_GRADE, a.APPROVE_MARK");
            sb.Append(" , a.CREATED_BY, a.CREATED_DT, a.UPDATED_BY, a.UPDATED_DT, a.FUNC_ID");
            sb.Append(" , convert(bit,case when a.APPROVE_MARK ='y' then  1 else 0 end) as IS_APPROVE_MARK, convert(bit,case when a.NOPAYDIFF_YN ='y' then  1 else 0 end) as IS_NOPAYDIFF_YN ");
            sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC as CHG_STATUS,c.EMP_NAME,d.DEPT_NAME_20,d.DEPT_NAME_30,d.DEPT_NAME_40 ");
            sb.Append(" from TB_S_M_SALARY_ADJ_D a");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='SA' and b.MAIN_CD='CHG_STATUS' and b.sub_cd=a.CHG_STATUS	");
            sb.Append(" left join TB_H_M_EMP c on c.EMP_ID=a.EMP_ID");
            sb.Append(" left join VW_H_DEPT_DATA d on a.DEPT_NO=d.DEPT_NO");
            sb.Append(" where EFFECT_YM = @EFFECT_YM");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "")
            {
                sb.Append(" and c.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
            }
            sb.Append(" ) god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");



            ht.Add("@EFFECT_YM", qdatakey);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }


    //Gridview 查詢總筆數
    public int getCount1(int startRowIndex, int maximumRows, string qdatakey, string emp_id, string emp_name)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_SALARY_ADJ_D a");
            sb.Append(" left join TB_H_M_EMP c on c.EMP_ID=a.EMP_ID");
            sb.Append(" where EFFECT_YM = @EFFECT_YM ");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "")
            {
                sb.Append(" and c.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
            }
            ht.Add("@EFFECT_YM", qdatakey);

            DataTable dt = dbConn.Query(sb, ht, true);
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

    public DataTable getData2(int startRowIndex, int maximumRows, string sortExpression, string qdatakey)
    {
        try
        {
            //if (sortExpression.Contains("LEVEL_CD"))
            //{
            //    sortExpression = sortExpression.Replace("LEVEL_CD", "a.LEVEL_CD");
            //}

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber, *");
            sb.Append(" from ");
            sb.Append(" (Select a.*, convert(bit,case when a.APPROVE_MARK ='y' then  1 else 0 end) as IS_APPROVE_MARK ");
            sb.Append(" from TB_S_M_SALARYSET_D a");
            sb.Append(" where EFFECT_YM = @EFFECT_YM");
            sb.Append(" ) tb ");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@EFFECT_YM", qdatakey);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //Gridview 查詢總筆數
    public int getCount2(int startRowIndex, int maximumRows, string qdatakey)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_SALARYSET_D a");
            sb.Append(" where EFFECT_YM = @EFFECT_YM ");
            ht.Add("@EFFECT_YM", qdatakey);

            DataTable dt = dbConn.Query(sb, ht, true);
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

    public DataTable getData3(int startRowIndex, int maximumRows, string sortExpression, string qdatakey)
    {
        try
        {
            sortExpression = string.Format("a.{0}", sortExpression);
            //if (sortExpression.Contains("LEVEL_CD"))
            //{
            //    sortExpression = sortExpression.Replace("LEVEL_CD", "a.LEVEL_CD");
            //}

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.*, convert(bit,case when a.APPROVE_MARK ='y' then  1 else 0 end) as IS_APPROVE_MARK,CASE WHEN PJOB_TYPE = 'M' then '管理職' WHEN PJOB_TYPE = 'P' then '專業職' ELSE '' END as PJOB_NAME  ");
            sb.Append(" from TB_S_M_2BSALARY_SET_D a");
            sb.Append(" where EFFECT_YM = @EFFECT_YM");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@EFFECT_YM", qdatakey);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht,true);
        }
        catch
        {
            throw;
        }
    }
    //Gridview 查詢總筆數
    public int getCount3(int startRowIndex, int maximumRows, string qdatakey)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_2BSALARY_SET_D a");
            sb.Append(" where EFFECT_YM = @EFFECT_YM ");
            ht.Add("@EFFECT_YM", qdatakey);

            DataTable dt = dbConn.Query(sb, ht, true);
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
    ////明細頁面 Gridview 查詢資料
    //public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string qdatakey)
    //{
    //    try
    //    {

    //        //if (sortExpression.Contains("LEVEL_CD"))
    //        //{
    //        //    sortExpression = sortExpression.Replace("LEVEL_CD", "a.LEVEL_CD");
    //        //}

    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append(" Select * From");
    //        sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
    //        sb.Append(" GRADE_CD,REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT, REMARK");
    //        sb.Append(" from TB_H_M_LEVEL_GRADE a");
    //        sb.Append(" where 1 = 1 ");
    //        sb.Append(" and LEVEL_CD = @LEVEL_CD and IS_VALID = 'Y' ");
    //        sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
    //        sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


    //        ht.Add("@startRowIndex", startRowIndex);
    //        ht.Add("@maximumRows", maximumRows);
    //        ht.Add("@LEVEL_CD", qdatakey);
    //        return dbConn.Query(sb, ht);
    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}
    ////明細頁面Gridview 查詢總筆數
    //public int getCount(int startRowIndex, int maximumRows, string qdatakey)
    //{
    //    try
    //    {
    //        int t = 0;
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append("Select COUNT(*) total_record ");
    //        sb.Append(" from TB_H_M_LEVEL_GRADE a");
    //        sb.Append(" where 1 = 1 ");
    //        sb.Append(" and LEVEL_CD = @LEVEL_CD and IS_VALID = 'Y' ");
    //        ht.Add("@LEVEL_CD", qdatakey);


    //        DataTable dt = dbConn.Query(sb, ht);
    //        if (dt.Rows.Count > 0)
    //        {
    //            t = (int)dt.Rows[0]["total_record"];
    //        }


    //        return t;


    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }

    //}

    internal System.Data.DataTable getLevelCD(string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select LEVEL_CD from TB_H_M_LEVEL Where @START_DT >= START_DT and @START_DT <= END_DT order by LEVEL_CD");
            ht.Add("@START_DT", start_dt);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    internal System.Data.DataTable check_LEVEL_CD(string LEVEL_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNT(*) as counts from TB_H_M_LEVEL_GRADE");
            sb.Append(" where LEVEL_CD = @LEVEL_CD");
            sb.Append(" and getdate() between START_DT and  END_DT");
            ht.Add("@LEVEL_CD", LEVEL_CD);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void updateData4_0()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" UPDATE       TB_S_M_SALARY_ADJ_H");
            sb.Append(" SET                APPROVE_DT = GETDATE(), APPROVE_BY = @EMP_ID, APPROVE_STATUS = 'Y', REMARK = @REMARK, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");
            sb.Append(" ");
            sb.Append(" UPDATE       TB_S_M_SALARYSET_H");
            sb.Append(" SET                APPROVE_DT = GETDATE(), APPROVE_BY = @EMP_ID, APPROVE_STATUS = 'Y', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");
            sb.Append(" ");
            sb.Append(" UPDATE       TB_S_M_2BSALARY_SET_H");
            sb.Append(" SET                APPROVE_DT = GETDATE(), APPROVE_BY = @EMP_ID, APPROVE_STATUS = 'Y', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");

            // **將考核調薪資料1.新增至資格檔(是否為工會會員='Y') ,並將資格檔目前生效的資料,其[END_DT]日期將上{生效年月}取月初-1天																																																																																																
            // (4-1) 1.先將資格檔(是否為工會會員='Y') and END_DT='9999/12/31',並將[END_DT]日期將上{生效年月}取月初-1天																																																																																																

            sb.Append(" UPDATE       TB_H_M_LEVEL");
            sb.Append(" SET                END_DT = @END_DT, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        END_DT = '9999/12/31' AND IS_UNION_MEMBER='Y' ");
            string END_DT = string.Format("{0}/{1}/{2}", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month - 1, DateTime.DaysInMonth(Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month - 1));

            ht.Add("@END_DT", END_DT);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@REMARK", REMARK);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable getData4_1(string EFFECT_YM)
    {
        try
        {
            //sortExpression = string.Format("a.{0}", sortExpression);

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.LEVEL_CD,a.ABILITY_ADJ,a.LEVEL_PAY_LOW,a.LEVEL_PAY_AVG,a.LEVEL_PAY_UP,a.ORDER_SEQ,b.LEVEL_PAY ");
            sb.Append(" from TB_S_M_SALARYSET_D a");
            sb.Append(" left join TB_H_M_LEVEL b on b.END_DT= @END_DT and a.LEVEL_CD=b.LEVEL_CD	");
            sb.Append(" where a.EFFECT_YM = @EFFECT_YM");
            sb.Append(" group by  a.LEVEL_CD,a.ABILITY_ADJ,a.LEVEL_PAY_LOW,a.LEVEL_PAY_AVG,a.LEVEL_PAY_UP,a.ORDER_SEQ,b.LEVEL_PAY");
            
            string END_DT = Convert.ToDateTime(EFFECT_YM).AddDays(-1).ToString("yyyy/MM/dd");
            ht.Add("@END_DT", END_DT);

            string ab = string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year - 1, Convert.ToDateTime(EFFECT_YM).Month);
            ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    internal void updateData4_1(string LEVEL_CD, string ABILITY_ADJ, string LEVEL_PAY, string LEVEL_PAY_LOW, string LEVEL_PAY_AVG, string LEVEL_PAY_UP, string ORDER_SEQ)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" INSERT INTO TB_H_M_LEVEL");
            sb.Append("  (LEVEL_CD, START_DT, END_DT, LEVEL_PAY, TOP_LEVEL_PAY, ABILITY_PAY_LOW, ABILITY_PAY_MID, ABILITY_PAY_HIGH");
            sb.Append("  , IS_UNION_MEMBER, ORDER_SEQ, REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" VALUES        (@LEVEL_CD, @START_DT, @END_DT, @LEVEL_PAY, @TOP_LEVEL_PAY, @ABILITY_PAY_LOW, @ABILITY_PAY_MID, @ABILITY_PAY_HIGH");
            sb.Append(" , @IS_UNION_MEMBER, @ORDER_SEQ, @REMARK, @EMP_ID, GETDATE(), @EMP_ID, GETDATE(), @FUNC_ID)");

            ht.Add("@LEVEL_CD", LEVEL_CD);

            ht.Add("@START_DT", string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month));

            ht.Add("@END_DT", "9999/12/31");
            ht.Add("@LEVEL_PAY", string.Format("{0}{1}", LEVEL_PAY,ABILITY_ADJ));
            ht.Add("@TOP_LEVEL_PAY", "0");
            ht.Add("@ABILITY_PAY_LOW", LEVEL_PAY_LOW);

            ht.Add("@ABILITY_PAY_MID", LEVEL_PAY_AVG);

            ht.Add("@ABILITY_PAY_HIGH", LEVEL_PAY_UP);

            ht.Add("@IS_UNION_MEMBER", "Y");
            ht.Add("@ORDER_SEQ", ORDER_SEQ);

            ht.Add("@REMARK", "");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal void updateData4_2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" UPDATE       TB_H_M_LEVEL");
            sb.Append(" SET                END_DT = @END_DT, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        END_DT = '9999/12/31' AND IS_UNION_MEMBER='N' ");
            string END_DT = string.Format("{0}/{1}/{2}", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month - 1, DateTime.DaysInMonth(Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month - 1));
            ht.Add("@END_DT", END_DT);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@REMARK", REMARK);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable getData4_3(string EFFECT_YM)
    {
        try
        {
           
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select LEVEL_CD,EXAMINE_C2,ORDER_SEQ ");
            sb.Append(" from TB_S_M_2BSALARY_SET_D");
            sb.Append(" where EFFECT_YM = @EFFECT_YM");
            ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    internal void updateData4_2_2(string LEVEL_CD, string EXAMINE_C2, string ORDER_SEQ)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" INSERT INTO TB_H_M_LEVEL");
            sb.Append("  (LEVEL_CD, START_DT, END_DT, LEVEL_PAY, TOP_LEVEL_PAY, ABILITY_PAY_LOW, ABILITY_PAY_MID, ABILITY_PAY_HIGH");
            sb.Append("  , IS_UNION_MEMBER, ORDER_SEQ, REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" VALUES        (@LEVEL_CD, @START_DT, @END_DT, @LEVEL_PAY, @TOP_LEVEL_PAY, @ABILITY_PAY_LOW, @ABILITY_PAY_MID, @ABILITY_PAY_HIGH");
            sb.Append(" , @IS_UNION_MEMBER, @ORDER_SEQ, @REMARK, @EMP_ID, GETDATE(), @EMP_ID, GETDATE(), @FUNC_ID)");

            ht.Add("@LEVEL_CD", LEVEL_CD);
           
            ht.Add("@START_DT", string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month));
           
            ht.Add("@END_DT", "9999/12/31");
            ht.Add("@LEVEL_PAY", "0");
            ht.Add("@TOP_LEVEL_PAY", EXAMINE_C2);
            ht.Add("@ABILITY_PAY_LOW", "0");
            ht.Add("@ABILITY_PAY_MID", "0");
            ht.Add("@ABILITY_PAY_HIGH", "0");
            ht.Add("@IS_UNION_MEMBER", "N");
            ht.Add("@ORDER_SEQ", ORDER_SEQ);
            ht.Add("@REMARK", "");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable getData5_1(string EFFECT_YM)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.*,b.EMP_NAME ");
            sb.Append(" from TB_S_M_SALARY_ADJ_D a");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID");
            sb.Append(" where a.EFFECT_YM = @EFFECT_YM");
            ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getTABSITEM()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT IS_PLUS,IS_TAX FROM TB_S_M_SALARY_ITEM WHERE SALARY_ID='2043' ");
          
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    internal void updateData5_1(string TXN_EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" UPDATE       TB_S_M_SALARY_TXN");
            sb.Append(" SET                EFFECT_EDT = @END_DT, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        EMP_ID = @TXN_EMP_ID AND SALARY_ID='1002' AND EFFECT_EDT='9999/12/31' ");
            string END_DT = string.Format("{0}/{1}/{2}", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month - 1, DateTime.DaysInMonth(Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month - 1));
            ht.Add("@END_DT", END_DT);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@TXN_EMP_ID", TXN_EMP_ID);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal void updateData5_2(string TXN_EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" UPDATE       TB_S_M_SALARY_TXN");
            sb.Append(" SET                EFFECT_EDT = @END_DT, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        EMP_ID = @TXN_EMP_ID AND SALARY_ID='1001' AND EFFECT_EDT='9999/12/31' ");
            string END_DT = string.Format("{0}/{1}/{2}", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month - 1, DateTime.DaysInMonth(Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month - 1));
            ht.Add("@END_DT", END_DT);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@TXN_EMP_ID", TXN_EMP_ID);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public DataTable getData5_1_2(string EFFECT_YM, string TXN_EMP_ID)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*)+1 as SEQ_NO ");
            sb.Append(" from TB_S_M_SALARY_TXN a");
            sb.Append(" where EMP_ID = @TXN_EMP_ID  AND SALARY_ID='1002' AND EFFECT_SDT= @EFFECT_SDT");
            ht.Add("@TXN_EMP_ID", TXN_EMP_ID);
            
            ht.Add("@EFFECT_SDT", string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month));
           
                         
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData5_2_2(string EFFECT_YM, string TXN_EMP_ID)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*)+1 as SEQ_NO ");
            sb.Append(" from TB_S_M_SALARY_TXN a");
            sb.Append(" where EMP_ID = @TXN_EMP_ID  AND SALARY_ID='1001' AND EFFECT_SDT= @EFFECT_SDT");
            ht.Add("@TXN_EMP_ID", TXN_EMP_ID);

            ht.Add("@EFFECT_SDT", string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month));


            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData5_2_3(string EFFECT_YM, string TXN_EMP_ID)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*)+1 as SEQ_NO ");
            sb.Append(" from TB_S_M_SUBSIDY_DEDUCTIONS_1 a");
            sb.Append(" where DATA_YM = @EFFECT_YM AND EMP_ID = @TXN_EMP_ID  AND SALARY_ID='2043'");
            ht.Add("@TXN_EMP_ID", TXN_EMP_ID);
            ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    internal void updateData5_1_2( string TXN_EMP_ID,string ABILITY_PAY_NEW, string SEQ_NO)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" INSERT INTO TB_S_M_SALARY_TXN");
            sb.Append("  (EMP_ID, SALARY_ID, AMOUNT, EFFECT_SDT, EFFECT_EDT, SEQ_NO, APPROVE_DT, APPROVE_BY, REMARK");
            sb.Append("  , CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" VALUES        (@TXN_EMP_ID, @SALARY_ID, @AMOUNT, @EFFECT_SDT, @EFFECT_EDT, @SEQ_NO, GETDATE(), @EMP_ID, @REMARK");
            sb.Append(" , @EMP_ID, GETDATE(), @EMP_ID, GETDATE(), @FUNC_ID)");

            ht.Add("@TXN_EMP_ID", TXN_EMP_ID);
            ht.Add("@SALARY_ID", "1002");
            ht.Add("@AMOUNT", ABILITY_PAY_NEW);
            ht.Add("@EFFECT_SDT", string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month));
            ht.Add("@EFFECT_EDT", "9999/12/31");
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@APPROVE_BY", EMP_ID);
            ht.Add("@REMARK", "考核調薪異動");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal void updateData5_2_2(string TXN_EMP_ID, string ABILITY_PAY_NEW, string SEQ_NO)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" INSERT INTO TB_S_M_SALARY_TXN");
            sb.Append("  (EMP_ID, SALARY_ID, AMOUNT, EFFECT_SDT, EFFECT_EDT, SEQ_NO, APPROVE_DT, APPROVE_BY, REMARK");
            sb.Append("  , CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" VALUES        (@TXN_EMP_ID, @SALARY_ID, @AMOUNT, @EFFECT_SDT, @EFFECT_EDT, @SEQ_NO, GETDATE(), @EMP_ID, @REMARK");
            sb.Append(" , @EMP_ID, GETDATE(), @EMP_ID, GETDATE(), @FUNC_ID)");

            ht.Add("@TXN_EMP_ID", TXN_EMP_ID);
            ht.Add("@SALARY_ID", "1001");
            ht.Add("@AMOUNT", ABILITY_PAY_NEW);
            ht.Add("@EFFECT_SDT", string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month));
            ht.Add("@EFFECT_EDT", "9999/12/31");
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@APPROVE_BY", EMP_ID);
            ht.Add("@REMARK", "考核調薪異動");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FUNC_ID", FUNC_ID);
            
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal void updateData5_2_3(string TXN_EMP_ID, string TXN_EMP_NAME, string SEQ_NO, string LEVEL_PAY_DIFF, string IS_PLUS, string IS_TAX)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" INSERT INTO TB_S_M_SUBSIDY_DEDUCTIONS_1");
            sb.Append("  (DATA_YM, EMP_ID, EMP_NAME, SALARY_ID, SEQ_NO, AMOUNT, IS_PLUS, IS_TAX, REMARK, SALARY_STATUS, SALARY_PROC_DT, SALARY_DT");
            sb.Append("  , CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT,FUNC_ID)");
            sb.Append(" VALUES        (@DATA_YM, @TXN_EMP_ID, @EMP_NAME, @SALARY_ID, @SEQ_NO, @AMOUNT, @IS_PLUS, @IS_TAX, @REMARK, @SALARY_STATUS, @SALARY_PROC_DT, @SALARY_DT");
            sb.Append(" , @EMP_ID, GETDATE(), @EMP_ID, GETDATE(), @FUNC_ID)");

            ht.Add("@DATA_YM", EFFECT_YM.Replace("/", ""));
            ht.Add("@TXN_EMP_ID", TXN_EMP_ID);
            ht.Add("@EMP_NAME", TXN_EMP_NAME);
            ht.Add("@SALARY_ID", "2043");
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@AMOUNT", LEVEL_PAY_DIFF);
            ht.Add("@IS_PLUS", IS_PLUS);
            ht.Add("@IS_TAX", IS_TAX);
            ht.Add("@REMARK", "考績津貼");
            ht.Add("@SALARY_STATUS", "N");
            ht.Add("@SALARY_PROC_DT", "");
            ht.Add("@SALARY_DT", "");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FUNC_ID", FUNC_ID);
            
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal void updateData6()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" UPDATE       TB_S_M_SALARYSET_D");
            sb.Append(" SET                APPROVE_MARK='N', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");
            sb.Append(" ");
            sb.Append(" UPDATE       TB_S_M_2BSALARY_SET_D");
            sb.Append(" SET                APPROVE_MARK='N', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");
            sb.Append(" ");
            sb.Append(" UPDATE       TB_S_M_SALARY_ADJ_D");
            sb.Append(" SET                APPROVE_MARK='N', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    internal void Approve()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_SE1400Approve");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/",""));
            ht.Add("@REMARK", REMARK);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.QuerySP(sb, ht, true);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //internal void updateData()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();

            
    //        sb.Append(" UPDATE       TB_S_M_SALARY_ADJ_H");
    //        sb.Append(" SET                RELEASE_DT = GETDATE(), RELEASE_BY = @EMP_ID, APPROVE_STATUS = 'Y', REMARK = @REMARK, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //        sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");
    //        sb.Append(" ");
    //        sb.Append(" UPDATE       TB_S_M_SALARYSET_H");
    //        sb.Append(" SET                RELEASE_DT = GETDATE(), RELEASE_BY = @EMP_ID, APPROVE_STATUS = 'Y', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //        sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");
    //        sb.Append(" ");
    //        sb.Append(" UPDATE       TB_S_M_2BSALARY_SET_H");
    //        sb.Append(" SET                RELEASE_DT = GETDATE(), RELEASE_BY = @EMP_ID, APPROVE_STATUS = 'Y', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //        sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");

    //        // **將考核調薪資料1.新增至資格檔(是否為工會會員='Y') ,並將資格檔目前生效的資料,其[END_DT]日期將上{生效年月}取月初-1天																																																																																																
    //        // (4-1) 1.先將資格檔(是否為工會會員='Y') and END_DT='9999/12/31',並將[END_DT]日期將上{生效年月}取月初-1天																																																																																																

    //        sb.Append(" UPDATE       TB_H_M_LEVEL");
    //        sb.Append(" SET                END_DT = @END_DT, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //        sb.Append(" WHERE        END_DT = '9999/12/31' AND IS_UNION_MEMBER='Y' ");
    //        string END_DT = string.Format("{0}/{1}/{2}", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month - 1, DateTime.DaysInMonth(Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month - 1));

    //        ht.Add("@END_DT", END_DT);
    //        ht.Add("@EMP_ID", EMP_ID);
    //        ht.Add("@REMARK", REMARK);
    //        ht.Add("@FUNC_ID", FUNC_ID);
    //        ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/",""));


    //        dbConn.ExecuteT(sb, ht, true);

    //        // 2.依考核調薪金額入力資料(同一資格為一筆,新增資料至資格檔資料中)																																																																																																

    //        sb.Clear();
    //        ht.Clear();

    //        sb.Append(" select a.LEVEL_CD,a.ABILITY_ADJ,a.LEVEL_PAY_LOW,a.LEVEL_PAY_AVG,a.LEVEL_PAY_UP,a.ORDER_SEQ,b.LEVEL_PAY ");
    //        sb.Append(" from TB_S_M_SALARYSET_D a");
    //        sb.Append(" left join TB_H_M_LEVEL b on b.START_DT= @START_DT and a.LEVEL_CD=b.LEVEL_CD	");
    //        sb.Append(" where a.EFFECT_YM = @EFFECT_YM");
    //        sb.Append(" group by  a.LEVEL_CD,a.ABILITY_ADJ,a.LEVEL_PAY_LOW,a.LEVEL_PAY_AVG,a.LEVEL_PAY_UP,a.ORDER_SEQ,b.LEVEL_PAY");
    //        ht.Add("@START_DT", string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year - 1, Convert.ToDateTime(EFFECT_YM).Month));
    //        string ab = string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year - 1, Convert.ToDateTime(EFFECT_YM).Month);
    //        ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
    //        DataTable dt = dbConn.Query(sb, ht);
    //        if (dt.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                sb.Clear();
    //                ht.Clear();
    //                sb.Append(" INSERT INTO TB_H_M_LEVEL");
    //                sb.Append("  (LEVEL_CD, START_DT, END_DT, LEVEL_PAY, TOP_LEVEL_PAY, ABILITY_PAY_LOW, ABILITY_PAY_MID, ABILITY_PAY_HIGH");
    //                sb.Append("  , IS_UNION_MEMBER, ORDER_SEQ, REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
    //                sb.Append(" VALUES        (@LEVEL_CD, @START_DT, @END_DT, @LEVEL_PAY, @TOP_LEVEL_PAY, @ABILITY_PAY_LOW, @ABILITY_PAY_MID, @ABILITY_PAY_HIGH");
    //                sb.Append(" , @IS_UNION_MEMBER, @ORDER_SEQ, @REMARK, @EMP_ID, GETDATE(), @EMP_ID, GETDATE(), @FUNC_ID)");

    //                ht.Add("@LEVEL_CD", Convert.ToString(dt.Rows[i]["LEVEL_CD"]));
                    
    //                ht.Add("@START_DT", string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month));
                  
    //                ht.Add("@END_DT", "9999/12/31");
    //                ht.Add("@LEVEL_PAY", string.Format("{0}{1}", Convert.ToString(dt.Rows[i]["LEVEL_PAY"]), Convert.ToString(dt.Rows[i]["ABILITY_ADJ"])));
    //                ht.Add("@TOP_LEVEL_PAY", "0");
    //                ht.Add("@ABILITY_PAY_LOW", Convert.ToString(dt.Rows[i]["LEVEL_PAY_LOW"]));
                    
    //                ht.Add("@ABILITY_PAY_MID", Convert.ToString(dt.Rows[i]["LEVEL_PAY_AVG"]));
                    
    //                ht.Add("@ABILITY_PAY_HIGH", Convert.ToString(dt.Rows[i]["LEVEL_PAY_UP"]));
                    
    //                ht.Add("@IS_UNION_MEMBER", "Y");
    //                ht.Add("@ORDER_SEQ", Convert.ToString(dt.Rows[i]["ORDER_SEQ"]));
                   
    //                ht.Add("@REMARK", "");
    //                ht.Add("@EMP_ID", EMP_ID);
    //                ht.Add("@FUNC_ID", FUNC_ID);

    //                dbConn.ExecuteT(sb, ht, true);

    //            }
    //        }

    //        //  **將考核調薪資料1.新增至資格檔(是否為工會會員='N') ,並將資格檔目前生效的資料,其[END_DT]日期將上{生效年月}取月初-1天																																																																																																
    //        //  (4-2) 1.先將資格檔(是否為工會會員='N') and END_DT='9999/12/31',並將[END_DT]日期將上{生效年月}取月初-1天																																																																																																

    //        sb.Clear();
    //        ht.Clear();


    //        sb.Append(" UPDATE       TB_H_M_LEVEL");
    //        sb.Append(" SET                END_DT = @END_DT, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //        sb.Append(" WHERE        END_DT = '9999/12/31' AND IS_UNION_MEMBER='N' ");

    //        ht.Add("@END_DT", END_DT);
    //        ht.Add("@EMP_ID", EMP_ID);
    //        ht.Add("@REMARK", REMARK);
    //        ht.Add("@FUNC_ID", FUNC_ID);
    //        ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));


    //        dbConn.ExecuteT(sb, ht, true);


    //        // 2.依2B以上本薪入力資料																																																																																																

    //        sb.Clear();
    //        ht.Clear();
    //        dt.Clear();

    //        sb.Append(" select LEVEL_CD,EXAMINE_C2,ORDER_SEQ ");
    //        sb.Append(" from TB_S_M_2BSALARY_SET_D");
    //        sb.Append(" where EFFECT_YM = @EFFECT_YM");
    //        ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
    //        dt = dbConn.Query(sb, ht);
    //        if (dt.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                sb.Clear();
    //                ht.Clear();
    //                sb.Append(" INSERT INTO TB_H_M_LEVEL");
    //                sb.Append("  (LEVEL_CD, START_DT, END_DT, LEVEL_PAY, TOP_LEVEL_PAY, ABILITY_PAY_LOW, ABILITY_PAY_MID, ABILITY_PAY_HIGH");
    //                sb.Append("  , IS_UNION_MEMBER, ORDER_SEQ, REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
    //                sb.Append(" VALUES        (@LEVEL_CD, @START_DT, @END_DT, @LEVEL_PAY, @TOP_LEVEL_PAY, @ABILITY_PAY_LOW, @ABILITY_PAY_MID, @ABILITY_PAY_HIGH");
    //                sb.Append(" , @IS_UNION_MEMBER, @ORDER_SEQ, @REMARK, @EMP_ID, GETDATE(), @EMP_ID, GETDATE(), @FUNC_ID)");

    //                ht.Add("@LEVEL_CD", Convert.ToString(dt.Rows[i]["LEVEL_CD"]));
    //                string b = Convert.ToString(dt.Rows[i]["LEVEL_CD"]);
    //                ht.Add("@START_DT", string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month));
    //                string bb = string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month);
    //                ht.Add("@END_DT", "9999/12/31");
    //                ht.Add("@LEVEL_PAY", "0");
    //                ht.Add("@TOP_LEVEL_PAY", Convert.ToString(dt.Rows[i]["EXAMINE_C2"]));
    //                ht.Add("@ABILITY_PAY_LOW", "0");
    //                ht.Add("@ABILITY_PAY_MID", "0");
    //                ht.Add("@ABILITY_PAY_HIGH", "0");
    //                ht.Add("@IS_UNION_MEMBER", "N");
    //                ht.Add("@ORDER_SEQ", Convert.ToString(dt.Rows[i]["ORDER_SEQ"]));
    //                ht.Add("@REMARK", "");
    //                ht.Add("@EMP_ID", EMP_ID);
    //                ht.Add("@FUNC_ID", FUNC_ID);

    //                dbConn.ExecuteT(sb, ht, true);

    //            }
    //        }

    //        //  (5) 新增個人敘薪檔 (TB_S_M_SALARY_TXN)																																																																																																
    //        //   **依個人別調薪資料,新增個人敘薪資料檔(薪資項目 IN ('1001','1002') and EFFECT_EDT='9999/12/31' 其[EFFECT_EDT]日期將上{生效年月}取月初-1天																																																																																																



    //        // TAB5 =select a.*,b.EMP_NAME from TB_S_M_SALARY_TXN a																																																																																																
    //        // left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID																																																																																																
    //        // where a.EFFECT_YM=明細畫面(主).生效年月																																																																																																

    //        sb.Clear();
    //        ht.Clear();
    //        dt.Clear();

    //        sb.Append(" select a.*,b.EMP_NAME ");
    //        sb.Append(" from TB_S_M_SALARY_ADJ_D a");
    //        sb.Append(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID");
    //        sb.Append(" where a.EFFECT_YM = @EFFECT_YM");
    //        ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
    //        dt = dbConn.Query(sb, ht);


    //        if (dt.Rows.Count > 0)
    //        {
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {

    //                //EXCEL 213
    //                if (Convert.ToInt32(dt.Rows[i]["ABILITY_PAY_NEW"]) > 0)
    //                {
    //                    sb.Clear();
    //                    ht.Clear();
    //                    sb.Append(" UPDATE       TB_S_M_SALARY_TXN");
    //                    sb.Append(" SET                EFFECT_EDT = @END_DT, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //                    sb.Append(" WHERE        EMP_ID = @TXN_EMP_ID AND SALARY_ID='1002' AND EFFECT_EDT='9999/12/31' ");

    //                    ht.Add("@END_DT", END_DT);
    //                    ht.Add("@EMP_ID", EMP_ID);
    //                    ht.Add("@TXN_EMP_ID", Convert.ToString(dt.Rows[i]["EMP_ID"]));
    //                    ht.Add("@FUNC_ID", FUNC_ID);

    //                    dbConn.ExecuteT(sb, ht, true);

    //                    // **新增該員資格俸資料																																																																																																
    //                    // v序=select count(*)+1 from TB_S_M_SALARY_TXN where EMP_ID=TAB5.EMP_ID AND SALARY_ID='1001' AND EFFECT_SDT=v生效年月月初																																																																																																

    //                    sb.Clear();
    //                    ht.Clear();


    //                    sb.Append(" select count(*)+1 as SEQ_NO ");
    //                    sb.Append(" from TB_S_M_SALARY_TXN a");
    //                    sb.Append(" where EMP_ID = @TXN_EMP_ID  AND SALARY_ID='1001' AND EFFECT_SDT= @EFFECT_SDT");
    //                    ht.Add("@TXN_EMP_ID", Convert.ToString(dt.Rows[i]["EMP_ID"]));
    //                    string te = Convert.ToString(dt.Rows[i]["EMP_ID"]);
    //                    ht.Add("@EFFECT_SDT", string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month));
    //                     string ste=string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month);
                         
    //                    DataTable dt1 = dbConn.Query(sb, ht);
                      
    //                    sb.Clear();
    //                    ht.Clear();
    //                    sb.Append(" INSERT INTO TB_S_M_SALARY_TXN");
    //                    sb.Append("  (EMP_ID, SALARY_ID, AMOUNT, EFFECT_SDT, EFFECT_EDT, SEQ_NO, APPROVE_DT, APPROVE_BY, REMARK");
    //                    sb.Append("  , CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
    //                    sb.Append(" VALUES        (@TXN_EMP_ID, @SALARY_ID, @AMOUNT, @EFFECT_SDT, @EFFECT_EDT, @SEQ_NO, GETDATE(), @EMP_ID, @REMARK");
    //                    sb.Append(" , @EMP_ID, GETDATE(), @EMP_ID, GETDATE(), @FUNC_ID)");

    //                    ht.Add("@TXN_EMP_ID", Convert.ToString(dt.Rows[i]["EMP_ID"]));
    //                    ht.Add("@SALARY_ID", "10002");
    //                    ht.Add("@AMOUNT", Convert.ToString(dt.Rows[i]["ABILITY_PAY_NEW"]));
    //                    ht.Add("@EFFECT_SDT", string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month));
    //                    ht.Add("@EFFECT_EDT", "9999/12/31");
    //                    ht.Add("@SEQ_NO", Convert.ToString(dt1.Rows[0]["SEQ_NO"]));
    //                    ht.Add("@APPROVE_BY", EMP_ID);
    //                    ht.Add("@REMARK", "考核調薪異動");
    //                    ht.Add("@EMP_ID", EMP_ID);
    //                    ht.Add("@FUNC_ID", FUNC_ID);

    //                    dbConn.ExecuteT(sb, ht, true);

                       

    //                }

    //                //EXCEL 238
    //                if (Convert.ToInt32(dt.Rows[i]["LEVEL_PAY_NEW"]) > 0)
    //                {

    //                    sb.Clear();
    //                    ht.Clear();

    //                    sb.Append(" UPDATE       TB_S_M_SALARY_TXN");
    //                    sb.Append(" SET                EFFECT_EDT = @END_DT, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //                    sb.Append(" WHERE        EMP_ID = @TXN_EMP_ID AND SALARY_ID='1001' AND EFFECT_EDT='9999/12/31' ");

    //                    ht.Add("@END_DT", END_DT);
    //                    ht.Add("@EMP_ID", EMP_ID);
    //                    ht.Add("@TXN_EMP_ID", Convert.ToString(dt.Rows[i]["EMP_ID"]));
    //                    ht.Add("@FUNC_ID", FUNC_ID);

    //                    dbConn.ExecuteT(sb, ht, true);

    //                    // **新增該員資格俸資料																																																																																																
    //                    // v序=select count(*)+1 from TB_S_M_SALARY_TXN where EMP_ID=TAB5.EMP_ID AND SALARY_ID='1001' AND EFFECT_SDT=v生效年月月初																																																																																																

    //                    sb.Clear();
    //                    ht.Clear();
                        

    //                    sb.Append(" select count(*)+1 as SEQ_NO ");
    //                    sb.Append(" from TB_S_M_SALARY_TXN a");
    //                    sb.Append(" where EMP_ID = @TXN_EMP_ID  AND SALARY_ID='1001' AND EFFECT_SDT= @EFFECT_SDT");
    //                    ht.Add("@TXN_EMP_ID", Convert.ToString(dt.Rows[i]["EMP_ID"]));
    //                    ht.Add("@EFFECT_SDT", string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month));
    //                    DataTable dt1 = dbConn.Query(sb, ht);

    //                    sb.Clear();
    //                    ht.Clear();
    //                    sb.Append(" INSERT INTO TB_S_M_SALARY_TXN");
    //                    sb.Append("  (EMP_ID, SALARY_ID, AMOUNT, EFFECT_SDT, EFFECT_EDT, SEQ_NO, APPROVE_DT, APPROVE_BY, REMARK");
    //                    sb.Append("  , CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
    //                    sb.Append(" VALUES        (@TXN_EMP_ID, @SALARY_ID, @AMOUNT, @EFFECT_SDT, @EFFECT_EDT, @SEQ_NO, GETDATE(), @EMP_ID, @REMARK");
    //                    sb.Append(" , @EMP_ID, GETDATE(), @EMP_ID, GETDATE(), @FUNC_ID)");

    //                    ht.Add("@TXN_EMP_ID", Convert.ToString(dt.Rows[i]["EMP_ID"]));
    //                    ht.Add("@SALARY_ID", "1001");
    //                    ht.Add("@AMOUNT", Convert.ToString(dt.Rows[i]["LEVEL_PAY_NEW"]));
    //                    ht.Add("@EFFECT_SDT", string.Format("{0}/{1}/01", Convert.ToDateTime(EFFECT_YM).Year, Convert.ToDateTime(EFFECT_YM).Month));
    //                    ht.Add("@EFFECT_EDT", "9999/12/31");
    //                    ht.Add("@SEQ_NO", Convert.ToString(dt1.Rows[0]["SEQ_NO"]));
    //                    ht.Add("@APPROVE_BY", EMP_ID);
    //                    ht.Add("@REMARK", "考核調薪異動");
    //                    ht.Add("@EMP_ID", EMP_ID);
    //                    ht.Add("@FUNC_ID", FUNC_ID);

    //                    dbConn.ExecuteT(sb, ht, true);

    //                }


    //                //EXCEL 263
    //                //if TAB5.LEVEL_PAY_DIFF>0 and NOPAYDIFF_YN='N'																																																																																																

    //                if (Convert.ToInt32(dt.Rows[i]["LEVEL_PAY_DIFF"]) > 0 && Convert.ToString(dt.Rows[i]["NOPAYDIFF_YN"]) == "N")
    //                {

    //                    sb.Clear();
    //                    ht.Clear();
    //                    sb.Append(" UPDATE       TB_S_M_SALARY_TXN");
    //                    sb.Append(" SET                EFFECT_EDT = @END_DT, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //                    sb.Append(" WHERE       EMP_ID = @TXN_EMP_ID AND SALARY_ID='1001' AND EFFECT_EDT='9999/12/31' ");

    //                    ht.Add("@END_DT", END_DT);
    //                    ht.Add("@EMP_ID", EMP_ID);
    //                    ht.Add("@TXN_EMP_ID", Convert.ToString(dt.Rows[i]["EMP_ID"]));
    //                    ht.Add("@FUNC_ID", FUNC_ID);

    //                    dbConn.ExecuteT(sb, ht, true);

    //                    // **新增該員資格俸資料																																																																																																
    //                    // v序=select count(*)+1 from TB_S_M_SUBSIDY_DEDUCTIONS_1 where DATA_YM = @EFFECT_YM AND EFFEMP_ID=TAB5.EMP_ID AND SALARY_ID='2043' 																																																																																																

    //                    sb.Clear();
    //                    ht.Clear();


    //                    sb.Append(" select count(*)+1 as SEQ_NO ");
    //                    sb.Append(" from TB_S_M_SUBSIDY_DEDUCTIONS_1 a");
    //                    sb.Append(" where DATA_YM = @EFFECT_YM AND EMP_ID = @TXN_EMP_ID  AND SALARY_ID='2043'");
    //                    ht.Add("@TXN_EMP_ID", Convert.ToString(dt.Rows[i]["EMP_ID"]));
    //                    ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
    //                    DataTable dt1 = dbConn.Query(sb, ht);

    //                    sb.Clear();
    //                    ht.Clear();
    //                    sb.Append(" INSERT INTO TB_S_M_SUBSIDY_DEDUCTIONS_1");
    //                    sb.Append("  (DATA_YM, EMP_ID, EMP_NAME, SALARY_ID, SEQ_NO, AMOUNT, IS_PLUS, IS_TAX, REMARK, SALARY_STATUS, SALARY_PROC_DT, SALARY_DT");
    //                    sb.Append("  , CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT,FUNC_ID)");
    //                    sb.Append(" VALUES        (@DATA_YM, @TXN_EMP_ID, @EMP_NAME, @SALARY_ID, @SEQ_NO, @AMOUNT, @IS_PLUS, @IS_TAX, @REMARK, @SALARY_STATUS, @SALARY_PROC_DT, @SALARY_DT");
    //                    sb.Append(" , @EMP_ID, GETDATE(), @EMP_ID, GETDATE(), @FUNC_ID)");

    //                    ht.Add("@DATA_YM", EFFECT_YM.Replace("/", ""));
    //                    ht.Add("@TXN_EMP_ID", Convert.ToString(dt.Rows[i]["EMP_ID"]));
    //                    ht.Add("@EMP_NAME", Convert.ToString(dt.Rows[i]["EMP_NAME"]));
    //                    ht.Add("@SALARY_ID", "2043");
    //                    ht.Add("@SEQ_NO", Convert.ToString(dt1.Rows[0]["SEQ_NO"]));
    //                    ht.Add("@AMOUNT", Convert.ToString(dt.Rows[i]["LEVEL_PAY_DIFF"]));
    //                    ht.Add("@IS_PLUS", Convert.ToString(dt.Rows[i]["IS_PLUS"]));
    //                    ht.Add("@IS_TAX", Convert.ToString(dt.Rows[i]["IS_TAX"]));
    //                    ht.Add("@REMARK", "考績津貼");
    //                    ht.Add("@SALARY_STATUS", "N");
    //                    ht.Add("@SALARY_PROC_DT", "");
    //                    ht.Add("@SALARY_DT", "");
    //                    ht.Add("@EMP_ID", EMP_ID);
    //                    ht.Add("@FUNC_ID", FUNC_ID);

    //                    dbConn.ExecuteT(sb, ht, true);

    //                }


    //            }


    //            //EXCEL 288	(6) 將考核調薪金額設定明細檔/2B以上考核本薪設定明細檔/個人別調薪明細檔 的[異動註記]欄位設為"N"。																																																																																																


    //            sb.Clear();
    //            ht.Clear();

    //            sb.Append(" UPDATE       TB_S_M_SALARYSET_D");
    //            sb.Append(" SET                APPROVE_MARK='N', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");
    //            sb.Append(" ");
    //            sb.Append(" UPDATE       TB_S_M_2BSALARY_SET_D");
    //            sb.Append(" SET                APPROVE_MARK='N', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");
    //            sb.Append(" ");
    //            sb.Append(" UPDATE       TB_S_M_SALARY_ADJ_D");
    //            sb.Append(" SET                APPROVE_MARK='N', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");

    //            ht.Add("@EMP_ID", EMP_ID);
    //            ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
    //            ht.Add("@FUNC_ID", FUNC_ID);

    //            dbConn.ExecuteT(sb, ht, true);
    //        }


    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    internal void rejectData_1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" UPDATE       TB_S_M_SALARY_ADJ_H");
            sb.Append(" SET                RELEASE_DT = @RELEASE_DT, RELEASE_BY = '', APPROVE_STATUS = 'B', REMARK = @REMARK, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");
           
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@REMARK", REMARK);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@RELEASE_DT", DBNull.Value);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal void rejectData_2(string deleteitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            char[] ch1 = new Char[] { '|' };
            string[] split1 = deleteitem.Split(ch1);
            string a = split1[0].ToString();
            string b = split1[1].ToString();
            string c = split1[2].ToString();
            sb.Append(" UPDATE       TB_S_M_SALARY_ADJ_D");
            sb.Append(" SET                APPROVE_MARK = @APPROVE_MARK, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM) and EMP_ID = @dt_EMP_ID");
            ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
                ht.Add("@dt_EMP_ID", b);
                ht.Add("@EMP_ID", EMP_ID);
                ht.Add("@FUNC_ID", FUNC_ID);
                ht.Add("@APPROVE_MARK", c);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal void rejectData_4(string deleteitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            char[] ch1 = new Char[] { '|' };
            string[] split1 = deleteitem.Split(ch1);
            string a = split1[0].ToString();
            string b = split1[1].ToString();
            string c = split1[2].ToString();
            string d = split1[3].ToString();
           sb.Append(" UPDATE       TB_S_M_SALARYSET_D");
           sb.Append(" SET                APPROVE_MARK = @APPROVE_MARK, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
                sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM) and LEVEL_CD = @LEVEL_CD and GRADE_CD = @GRADE_CD");

                ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
                ht.Add("@LEVEL_CD", b);
                ht.Add("@GRADE_CD", c);
                    ht.Add("@EMP_ID", EMP_ID);
                    ht.Add("@FUNC_ID", FUNC_ID);
                    ht.Add("@APPROVE_MARK", d);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal void rejectData_6(string deleteitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            char[] ch1 = new Char[] { '|' };
            string[] split1 = deleteitem.Split(ch1);
            string a = split1[0].ToString();
            string b = split1[1].ToString();
            string c = split1[2].ToString();
            sb.Append(" UPDATE       TB_S_M_2BSALARY_SET_D");
            sb.Append(" SET                APPROVE_MARK = @APPROVE_MARK, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
                sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM) and LEVEL_CD = @LEVEL_CD");

                ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
                ht.Add("@LEVEL_CD", b);
                    ht.Add("@EMP_ID", EMP_ID);
                    ht.Add("@FUNC_ID", FUNC_ID);
                    ht.Add("@APPROVE_MARK", c);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal void rejectData_3()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" UPDATE       TB_S_M_SALARYSET_H");
            sb.Append(" SET                RELEASE_DT = @RELEASE_DT, RELEASE_BY = '', APPROVE_STATUS = 'B', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@RELEASE_DT", DBNull.Value);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    internal void rejectData_5()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" UPDATE       TB_S_M_2BSALARY_SET_H");
            sb.Append(" SET                RELEASE_DT = @RELEASE_DT, RELEASE_BY = '', APPROVE_STATUS = 'B', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");

            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@RELEASE_DT", DBNull.Value);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //internal void rejectData()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append(" UPDATE       TB_S_M_SALARY_ADJ_H");
    //        sb.Append(" SET                RELEASE_DT = '', RELEASE_BY = '', APPROVE_STATUS = 'B', REMARK = @REMARK, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //        sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");
    //        //sb.Append(" ");
    //        //sb.Append(" UPDATE       TB_S_M_SALARYSET_H");
    //        //sb.Append(" SET                RELEASE_DT = '', RELEASE_BY = '', APPROVE_STATUS = 'B', REMARK = @REMARK, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //        //sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");
    //        //sb.Append(" ");
    //        //sb.Append(" UPDATE       TB_S_M_2BSALARY_SET_H");
    //        //sb.Append(" SET                RELEASE_DT = '', RELEASE_BY = '', APPROVE_STATUS = 'B', REMARK = @REMARK, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //        //sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");

    //        ht.Add("@EMP_ID", EMP_ID);
    //        ht.Add("@REMARK", REMARK);
    //        ht.Add("@FUNC_ID", FUNC_ID);
    //        ht.Add("@EFFECT_YM", EFFECT_YM);


    //        dbConn.ExecuteT(sb, ht, true);



    //        //excel 317 (2)按下此功能鍵,依據異動狀態打勾的 更新 個人別調薪資料明細(TB_S_M_SALARY_ADJ_D):
    //        DataTable dt = getData1(0, getCount1(0, 10, EFFECT_YM), "EFFECT_YM", EFFECT_YM);

    //        if (dt.Rows.Count > 0)
    //        {

    //            sb.Clear();
    //            sb.Append(" UPDATE       TB_S_M_SALARY_ADJ_D");
    //            sb.Append(" SET                APPROVE_MARK = 'Y', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM) and EMP_ID = @dt_EMP_ID");
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                ht.Clear();
    //                ht.Add("@dt_EMP_ID", Convert.ToString(dt.Rows[i]["EMP_ID"]));
    //                ht.Add("@EMP_ID", EMP_ID);
    //                ht.Add("@FUNC_ID", FUNC_ID);
    //                ht.Add("@EFFECT_YM", EFFECT_YM);
    //            }
    //        }

    //        //excel 331 (3)以 畫面.初任薪年度 更新 TB_S_M_SALARYSET_H(3A以下調薪金額主檔),更新內容如下:
    //        sb.Append(" UPDATE       TB_S_M_SALARYSET_H");
    //        sb.Append(" SET                RELEASE_DT = '', RELEASE_BY = '', APPROVE_STATUS = 'B', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //        sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");
    //        //sb.Append(" ");
    //        //sb.Append(" UPDATE       TB_S_M_2BSALARY_SET_H");
    //        //sb.Append(" SET                RELEASE_DT = '', RELEASE_BY = '', APPROVE_STATUS = 'B', REMARK = @REMARK, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //        //sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");

    //        ht.Add("@EMP_ID", EMP_ID);
    //        ht.Add("@FUNC_ID", FUNC_ID);
    //        ht.Add("@EFFECT_YM", EFFECT_YM);


    //        dbConn.ExecuteT(sb, ht, true);

    //        //excel 344 (4)以 畫面.初任薪年度 更新 TB_S_M_SALARYSET_D(3A以下調薪金額明細檔),更新內容如下:
    //        dt.Clear();
    //        dt = getData2(0, getCount2(0, 10, EFFECT_YM), "EFFECT_YM", EFFECT_YM);


    //        if (dt.Rows.Count > 0)
    //        {

    //            sb.Clear();
    //            sb.Append(" UPDATE       TB_S_M_SALARYSET_D");
    //            sb.Append(" SET                APPROVE_MARK = 'Y', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM) and LEVEL_CD = @LEVEL_CD and GRADE_CD = @GRADE_CD");
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                ht.Clear();
    //                ht.Add("@EMP_ID", EMP_ID);
    //                ht.Add("@FUNC_ID", FUNC_ID);
    //                ht.Add("@EFFECT_YM", EFFECT_YM);
    //                ht.Add("@LEVEL_CD", Convert.ToString(dt.Rows[i]["LEVEL_CD"]));
    //                ht.Add("@GRADE_CD", Convert.ToString(dt.Rows[i]["GRADE_CD"]));
    //            }
    //        }

    //        //excel 359 (5)以 畫面.初任薪年度 更新2B以上本薪調整主檔(TB_S_M_2BSALARY_SET_H),更新內容如下:
    //        sb.Clear();
    //        sb.Append(" UPDATE       TB_S_M_2BSALARY_SET_H");
    //        sb.Append(" SET                RELEASE_DT = '', RELEASE_BY = '', APPROVE_STATUS = 'B', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //        sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");

    //        ht.Clear();
    //        ht.Add("@FUNC_ID", FUNC_ID);
    //        ht.Add("@EFFECT_YM", EFFECT_YM);
    //        ht.Add("@EMP_ID", EMP_ID);

    //        dbConn.ExecuteT(sb, ht, true);

    //        //excel 372 (6)以 畫面.初任薪年度 更新 2B以上考核本薪設定明細檔(TB_S_M_2BSALARY_SET_D),更新內容如下:
    //        dt.Clear();
    //        dt = getData3(0, getCount3(0, 10, EFFECT_YM), "EFFECT_YM", EFFECT_YM);


    //        if (dt.Rows.Count > 0)
    //        {

    //            sb.Clear();
    //            sb.Append(" UPDATE       TB_S_M_2BSALARY_SET_D");
    //            sb.Append(" SET                APPROVE_MARK = 'Y', UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
    //            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM) and LEVEL_CD = @LEVEL_CD");
    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                ht.Clear();
    //                ht.Add("@EMP_ID", EMP_ID);
    //                ht.Add("@FUNC_ID", FUNC_ID);
    //                ht.Add("@EFFECT_YM", EFFECT_YM);
    //                ht.Add("@LEVEL_CD", Convert.ToString(dt.Rows[i]["LEVEL_CD"]));
    //            }
    //        }


    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}


    //異常註記-update 備註說明  (考核資料維護檔 DTL)
    public void updateSALARY_ADJ_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_SALARY_ADJ_H ");
            sb.Append(" set REMARK = @REMARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where EFFECT_YM = @EFFECT_YM ");
            //set值
            ht.Add("@REMARK", REMARK);

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@EFFECT_YM", EFFECT_YM);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //異常註記-update 異常註記為空白 或V (個人別調薪明細檔 DTL)
    public void updateTB_S_M_SALARY_ADJ_D(DateTime now, string approve_mark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_SALARY_ADJ_D ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where EFFECT_YM = @EFFECT_YM ");
            sb.Append("  and EMP_ID = @EMP_ID");

            ht.Add("@APPROVE_MARK", approve_mark);

            //PK值
            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@EMP_ID", EMP_ID);

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //異常註記-update 異常註記為空白 或V (個人別調薪明細檔 DTL)
    public void updateTB_S_M_SALARYSET_D(DateTime now, string approve_mark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_SALARYSET_D ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where EFFECT_YM = @EFFECT_YM ");
            sb.Append("  and LEVEL_CD = @LEVEL_CD");
            sb.Append("  and GRADE_CD = @GRADE_CD");

            ht.Add("@APPROVE_MARK", approve_mark);

            //PK值
            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@GRADE_CD", GRADE_CD);
            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //異常註記-update 異常註記為空白 或V (2B以上考核本薪設定明細檔 DTL)
    public void updateTB_S_M_2BSALARY_SET_D(DateTime now, string approve_mark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_2BSALARY_SET_D ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where EFFECT_YM = @EFFECT_YM ");
            sb.Append("  and LEVEL_CD = @LEVEL_CD");

            ht.Add("@APPROVE_MARK", approve_mark);

            //PK值
            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //確認TB_S_M_SALARY_ADJ_D 有無異常註記
    public int getMarkData()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount      ");
            sb.Append("  from TB_S_M_SALARY_ADJ_D        ");
            sb.Append(" where EFFECT_YM = @EFFECT_YM      ");
            sb.Append("  and APPROVE_MARK = @APPROVE_MARK");
            //PK值
            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@APPROVE_MARK", "Y");
            DataTable dt = dbConn.Query(sb, ht, true);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];

            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //確認TB_S_M_SALARYSET_D 有無異常註記
    public int getMarkData2()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_SALARYSET_D a");
            sb.Append(" where EFFECT_YM = @EFFECT_YM ");
            sb.Append("  and APPROVE_MARK = @APPROVE_MARK");
            //PK值
            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@APPROVE_MARK", "Y");
            DataTable dt = dbConn.Query(sb, ht, true);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];

            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //確認TB_S_M_2BSALARY_SET_D 有無異常註記
    public int getMarkData3()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_2BSALARY_SET_D a");
            sb.Append(" where EFFECT_YM = @EFFECT_YM ");
            sb.Append("  and APPROVE_MARK = @APPROVE_MARK");
            //PK值
            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@APPROVE_MARK", "Y");
            DataTable dt = dbConn.Query(sb, ht, true);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];

            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }
    }
}