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
/// CFB2SJ0100DAO 的摘要描述
/// </summary>
public class CFB2SJ0100DAO : BaseDAO
{
    //dj030基本欄位
    public string ASSESS_YEAR { get; set; }
    public string ASSESS_YEAR_S { get; set; }
    public string ASSESS_YEAR_E { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string DEPT_NO { get; set; }
    public string DEPT_NAME { get; set; }
    public string LEVEL_CD { get; set; }
    public string WS_CD { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    //ACES
    bool isSuper { get; set; }  //是否為supervisor(擔當)

    ACESLib.ACES aces;
    public CFB2SJ0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
        aces = new ACESLib.ACES();
        this.isSuper = getIsSuper();
    }


    //取得是否為主管
    internal DataTable getMNGData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) mngCount from TB_H_R_HEAD_DEPT");
            sb.Append(" where EMP_ID=@EMP_ID");
            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //EXCEL下載資料-考核人事資料主檔(SJ010)
    public DataTable getExcelDataSJ010()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select ");
            sb.Append(" a.*   ");
            sb.Append(@" ,SCORE_FINAL_DESC =  case    
                         when a.LEVELUP_FLAG='V' and a.SCORE_FINAL='D' and a.ASSESS_YEAR<2017  then a.SCORE_FINAL+'*' 
                         when a.LEVELUP_FLAG='V' and a.SCORE_FINAL='B' and a.ASSESS_YEAR>=2017  then a.SCORE_FINAL+'*' 
                         ELSE SCORE_FINAL  
                         end 
                        ");
            //sb.Append(" , SCORE_FINAL_DESC =  case when a.LEVELUP_FLAG='V' and SCORE_FINAL='D'  then a.SCORE_FINAL+'*' ELSE a.SCORE_FINAL  end   ");
            sb.Append(" , SCORE_DEPT_DESC = case   when   LEVELUP_FLAG='V' and SCORE_DEPT='D'   then SCORE_DEPT+'*' ELSE SCORE_DEPT  end    ");


            sb.Append(" , a.DEPT_NAME_20 + ' ' + a.DEPT_NAME_30 + ' ' + DEPT_NAME_40  DEPT_NAME_DESC");
            sb.Append(" , a.ASSESS_TYPE + '-' + b.SUB_DESC ASSESS_TYPE_DESC   ");
            sb.Append(" , a.WS_CD + '-'       + c.SUB_DESC WS_CD_DESC   ");
            sb.Append(" , a.SEX_CD + '-'      + d.SUB_DESC SEX_CD_DESC   ");
            sb.Append(" , a.LINE_CD + '-'     + e.SUB_DESC LINE_CD_DESC   ");
            sb.Append(" , a.PLANT_CD + '-'    + f.SUB_DESC PLANT_CD_DESC   ");
            sb.Append(" , a.EMP_CD + '-'      + g.SUB_DESC EMP_CD_DESC   ");
            sb.Append(" , a.EDUCATION_CD + '-'+ h.SUB_DESC EDUCATION_CD_DESC   ");
            sb.Append(" , a.EMP_CHG_CD + '-'  + i.SUB_DESC EMP_CHG_CD_DESC   ");
            sb.Append(" ,convert(decimal(4,1),RECENT_LEVEL_WORK_YEARS) RECENT_LEVEL_WORK_YEARS_DESC   ");
            sb.Append(" , convert(decimal(4,1),WORK_YEARS) WORK_YEARS_DESC   ");
            sb.Append(" from TB_S_R_ASSESS_TARGET a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.ASSESS_TYPE = b.SUB_CD and b.MAIN_CD = 'ASSESS_TYPE'  and b.IS_VALID='Y'  and b.SYS_CD='SJ'  ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.WS_CD = c.SUB_CD       and c.MAIN_CD = 'WS_CD'        and c.IS_VALID='Y'  and c.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.SEX_CD = d.SUB_CD      and d.MAIN_CD = 'SEX_CD'       and d.IS_VALID='Y'  and d.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.LINE_CD = e.SUB_CD     and e.MAIN_CD = 'LINE_CD'      and e.IS_VALID='Y'  and e.SYS_CD='DB' ");
            sb.Append("  left join TB_9_M_COMM_D f on  a.PLANT_CD = f.SUB_CD    and f.MAIN_CD = 'PLANT_CD'     and f.IS_VALID='Y'  and f.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D g on  a.EMP_CD = g.SUB_CD      and g.MAIN_CD = 'EMP_CD'       and g.IS_VALID='Y'  and g.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D h on  a.EDUCATION_CD = h.SUB_CD and h.MAIN_CD = 'EDUCATION_CD' and h.IS_VALID='Y'  and h.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D i on  a.EMP_CHG_CD = i.SUB_CD  and i.MAIN_CD = 'EMP_CHG_CD'   and i.IS_VALID='Y'  and i.SYS_CD='HB' ");
            sb.Append(" where 1=1 ");

            if (isSuper == false)
            {
                //若登入者為長官則
                DataTable mngData = getMNGData();
                int mngCount = (int)mngData.Rows[0]["mngCount"];
                if (mngCount > 0)
                {
                    sb.Append(" and  DEPT_NO in ( select MNG_DEPT_NO from TB_H_R_HEAD_DEPT  where EMP_ID = @MNG_EMP_ID )   ");
                    ht.Add("@MNG_EMP_ID", SessionHandle.Current.emp_id);
                }
                else
                {
                    sb.Append(" and EMP_ID = @SELF_EMP_ID ");
                    ht.Add("@SELF_EMP_ID", SessionHandle.Current.emp_id);
                }

            }
            ////若登入者為長官則
            //DataTable mngData = getMNGData();
            //int mngCount = (int)mngData.Rows[0]["mngCount"];
            //if (mngCount > 0)
            //{
            //    sb.Append(" and  DEPT_NO in ( select MNG_DEPT_NO from TB_H_R_HEAD_DEPT  where EMP_ID = @MNG_EMP_ID )   ");
            //    ht.Add("@MNG_EMP_ID", SessionHandle.Current.emp_id);
            //}
            //else
            //{
            //    sb.Append(" and EMP_ID = @SELF_EMP_ID ");
            //    ht.Add("@SELF_EMP_ID", SessionHandle.Current.emp_id);
            //}

            //查詢條件

            if (ASSESS_YEAR_S != "")
            {
                sb.Append(" and ASSESS_YEAR >= @ASSESS_YEAR_S ");
                ht.Add("@ASSESS_YEAR_S", ASSESS_YEAR_S);
            }
            if (ASSESS_YEAR_E != "")
            {
                sb.Append(" and ASSESS_YEAR <= @ASSESS_YEAR_E ");
                ht.Add("@ASSESS_YEAR_E", ASSESS_YEAR_E);
            }
            if (ASSESS_TYPE != "-1")
            {
                sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID + "%");
            }
            if (EMP_NAME != "")
            {
                sb.Append(" and EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", EMP_NAME + "%");
            }
            if (DEPT_NO != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", DEPT_NO + "%");
            }
            if (DEPT_NAME != "")
            {
                sb.Append(" and DEPT_NAME like @DEPT_NAME ");
                ht.Add("@DEPT_NAME", DEPT_NAME + "%");
            }
            if (LEVEL_CD != "")
            {
                sb.Append(" and LEVEL_CD like @LEVEL_CD ");
                ht.Add("@LEVEL_CD", LEVEL_CD + "%");
            }
            if (WS_CD != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", WS_CD);
            }
            sb.Append(" order by a.EMP_ID ASC, a.ASSESS_YEAR DESC, a.ASSESS_TYPE DESC ");

            
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //EXCEL下載資料-考核人事資料維護檔(SJ020)
    public DataTable getExcelDataSJ020()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select ");
            sb.Append(" a.*   ");
            sb.Append(" , SCORE_FINAL_DESC =  case when a.LEVELUP_FLAG='V' and SCORE_FINAL='D'  then a.SCORE_FINAL+'*' ELSE a.SCORE_FINAL  end   ");
            sb.Append(" , SCORE_DEPT_DESC = case   when   LEVELUP_FLAG='V' and SCORE_DEPT='D'   then SCORE_DEPT+'*' ELSE SCORE_DEPT  end    ");
            sb.Append(" , a.DEPT_NAME_20 + ' '+ a.DEPT_NAME_30 + ' ' + DEPT_NAME_40  DEPT_NAME_DESC");
            sb.Append(" , a.ASSESS_TYPE + '-' + b.SUB_DESC ASSESS_TYPE_DESC   ");
            sb.Append(" , a.WS_CD + '-'       + c.SUB_DESC WS_CD_DESC   ");
            sb.Append(" , a.SEX_CD + '-'      + d.SUB_DESC SEX_CD_DESC   ");
            sb.Append(" , a.LINE_CD + '-'     + e.SUB_DESC LINE_CD_DESC   ");
            sb.Append(" , a.PLANT_CD + '-'    + f.SUB_DESC PLANT_CD_DESC   ");
            sb.Append(" , a.EMP_CD + '-'      + g.SUB_DESC EMP_CD_DESC   ");
            sb.Append(" , a.EDUCATION_CD + '-'+ h.SUB_DESC EDUCATION_CD_DESC   ");
            sb.Append(" , a.EMP_CHG_CD + '-'  + i.SUB_DESC EMP_CHG_CD_DESC   ");
            sb.Append(" from TB_S_M_ASSESS_TARGET a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.ASSESS_TYPE = b.SUB_CD and b.MAIN_CD = 'ASSESS_TYPE'  and b.IS_VALID='Y'  and b.SYS_CD='SJ'  ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.WS_CD = c.SUB_CD       and c.MAIN_CD = 'WS_CD'        and c.IS_VALID='Y'  and c.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.SEX_CD = d.SUB_CD      and d.MAIN_CD = 'SEX_CD'       and d.IS_VALID='Y'  and d.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.LINE_CD = e.SUB_CD     and e.MAIN_CD = 'LINE_CD'      and e.IS_VALID='Y'  and e.SYS_CD='DB' ");
            sb.Append("  left join TB_9_M_COMM_D f on  a.PLANT_CD = f.SUB_CD    and f.MAIN_CD = 'PLANT_CD'     and f.IS_VALID='Y'  and f.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D g on  a.EMP_CD = g.SUB_CD      and g.MAIN_CD = 'EMP_CD'       and g.IS_VALID='Y'  and g.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D h on  a.EDUCATION_CD = h.SUB_CD and h.MAIN_CD = 'EDUCATION_CD' and h.IS_VALID='Y'  and h.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D i on  a.EMP_CHG_CD = i.SUB_CD  and i.MAIN_CD = 'EMP_CHG_CD'   and i.IS_VALID='Y'  and i.SYS_CD='HB' ");

            sb.Append(" where 1=1 ");


            if (ASSESS_YEAR != "")
            {
                sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            }
            if (ASSESS_TYPE != "-1")
            {
                sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            }
            sb.Append(" order by   ");
            sb.Append(" a.PLANT_CD ASC, a.DEPT_NO ASC, a.LEVEL_ORDER_SEQ ASC   ");
            sb.Append(" , a.RECENT_LEVEL_WORK_YEARS DESC, a.AGE DESC, a.WORK_YEARS DESC   ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //由ACES取得是否為SUPERVISOR
    public bool getIsSuper() {
        bool result = false;
        ACESLib.ACES aces = new ACESLib.ACES();
        String dbRole = aces.GetRoles();
        IList<string> role = dbRole.Split(',');
        try
        {
            foreach (string DB_ROLE_CD in role)
            {
                //string DB_ROLE_CD = "FB2DBMANAGER";
                string SysCode = ((ACESLib.DEPTBean)aces.GetDEPTAuth(DB_ROLE_CD)).SysCode;         //取得「大分類代碼」
                foreach (string big_sysCode in SysCode.Split(','))
                {
                    if (big_sysCode.Trim().Equals("SUPER"))
                    {
                        result = true;
                    }
                }
            }
        }
        catch
        {
        }
        return result;
    }


    #region Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
            , string assess_year_s, string assess_year_e, string assess_type, string emp_id, string emp_name, string dept_no, string level_cd, string ws_cd
                           )
    {
        try
        {

            //if (sortExpression.Contains("ENV_ALLOWANCE_TYPE"))
            //    sortExpression = sortExpression.Replace("ENV_ALLOWANCE_TYPE", "a.ENV_ALLOWANCE_TYPE");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" a.*   ");
            sb.Append(" , SCORE_FINAL_DESC =  case when a.LEVELUP_FLAG='V' and SCORE_FINAL='D'  then a.SCORE_FINAL+'*' ELSE a.SCORE_FINAL  end   ");
            sb.Append(" , a.DEPT_NAME_20 + ' '+ a.DEPT_NAME_30 + ' ' + DEPT_NAME_40  DEPT_NAME_DESC");
            sb.Append(" , a.ASSESS_TYPE + '-' + b.SUB_DESC ASSESS_TYPE_DESC   ");
            sb.Append(" , a.WS_CD + '-' + c.SUB_DESC WS_CD_DESC   ");
            sb.Append(" from TB_S_R_ASSESS_TARGET a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.ASSESS_TYPE = b.SUB_CD and b.MAIN_CD = 'ASSESS_TYPE'  and b.IS_VALID='Y'  and b.SYS_CD='SJ'  ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.WS_CD = c.SUB_CD and c.MAIN_CD = 'WS_CD'  and c.IS_VALID='Y'  and c.SYS_CD='HB' ");
            sb.Append(" where 1=1 ");

            if (SessionHandle.Current.is_super != "Y")
            {
                //若登入者為長官則
                DataTable mngData = getMNGData();
                int mngCount = (int)mngData.Rows[0]["mngCount"];
                if (mngCount > 0)
                {
                    sb.Append(" and  DEPT_NO in ( select MNG_DEPT_NO from TB_H_R_HEAD_DEPT  where EMP_ID = @MNG_EMP_ID )   ");
                    ht.Add("@MNG_EMP_ID", SessionHandle.Current.emp_id);
                }
                else
                {
                    sb.Append(" and EMP_ID = @SELF_EMP_ID ");
                    ht.Add("@SELF_EMP_ID", SessionHandle.Current.emp_id);
                }
            
            }

            //查詢條件
            if (assess_year_s != "")
            {
                sb.Append(" and ASSESS_YEAR >= @ASSESS_YEAR_S ");
                ht.Add("@ASSESS_YEAR_S", assess_year_s);
            }
            if (assess_year_e != "")
            {
                sb.Append(" and ASSESS_YEAR <= @ASSESS_YEAR_E ");
                ht.Add("@ASSESS_YEAR_E", assess_year_e);
            }
            if (assess_type != "-1")
            {
                sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
            }
            if (emp_id != "")
            {
                sb.Append(" and EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "")
            {
                sb.Append(" and EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
            }
            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            /*
            if (dept_name != "")
            {
                sb.Append(" and DEPT_NAME like @DEPT_NAME ");
                ht.Add("@DEPT_NAME", dept_name + "%");
            }
             */ 
            if (level_cd != "")
            {
                sb.Append(" and LEVEL_CD like @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd + "%");
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
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
    public int getCount(int startRowIndex, int maximumRows
            , string assess_year_s, string assess_year_e, string assess_type, string emp_id, string emp_name, string dept_no, string level_cd, string ws_cd)
    {
        try
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_R_ASSESS_TARGET ");
            sb.Append(" where 1=1 ");
            if (isSuper == false)
            {
                //若登入者為長官則
                DataTable mngData = getMNGData();
                int mngCount = (int)mngData.Rows[0]["mngCount"];
                if (mngCount > 0)
                {
                    sb.Append(" and  DEPT_NO in ( select MNG_DEPT_NO from TB_H_R_HEAD_DEPT  where EMP_ID = @MNG_EMP_ID )   ");
                    ht.Add("@MNG_EMP_ID", SessionHandle.Current.emp_id);
                }
                else
                {
                    sb.Append(" and EMP_ID = @SELF_EMP_ID ");
                    ht.Add("@SELF_EMP_ID", SessionHandle.Current.emp_id);
                }

            }

            //查詢條件
            if (assess_year_s != "")
            {
                sb.Append(" and ASSESS_YEAR >= @ASSESS_YEAR_S ");
                ht.Add("@ASSESS_YEAR_S", assess_year_s);
            }
            if (assess_year_e != "")
            {
                sb.Append(" and ASSESS_YEAR <= @ASSESS_YEAR_E ");
                ht.Add("@ASSESS_YEAR_E", assess_year_e);
            }
            if (assess_type != "-1")
            {
                sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
            }
            if (emp_id != "")
            {
                sb.Append(" and EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "")
            {
                sb.Append(" and EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
            }
            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
             /*
            if (dept_name != "")
            {
                sb.Append(" and DEPT_NAME like @DEPT_NAME ");
                ht.Add("@DEPT_NAME", dept_name + "%");
            }
             */ 
            if (level_cd != "")
            {
                sb.Append(" and LEVEL_CD like @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd + "%");
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }



            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = (int)dt.Rows[0]["resultCount"];
            }
            return result;
        }
        catch (Exception)
        {

            throw;
        }

    }

    #endregion


   



}