using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2990300DAO 的摘要描述
/// </summary>
public class CFB2990300DAO : BaseDAO
{
    public CFB2990300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string MAIN_CD { get; set; }
    public string SUB_CD { get; set; }
    //for查詢欄位
    public string EDIT_INFOR { get; set; }
    public string TABLE_NAME { get; set; }
    public string FUNC_ID { get; set; }
    public string CATEGORY { get; set; }
    public string UPDATED_BY { get; set; }
    public string updated_dt_s { get; set; }
    public string updated_dt_e { get; set; }

    #region Qry

    public DataTable getSYS_KIND_name(string kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select SUB_DESC From TB_9_M_COMM_D");
            sb.AppendLine(" where SYS_CD = '99' and MAIN_CD = 'SYS_LOG' ");
            sb.AppendLine(" and SUB_CD = @SUB_CD ");
            ht.Add("@SUB_CD", kind.Trim());

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public string getSYS_KIND()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select SUB_CD From TB_9_M_COMM_D ");
            sb.AppendLine(" where SYS_CD = '99' and MAIN_CD = 'SYS_LOG' ");

            DataTable dt = dbConn.Query(sb, ht);
            string code = "";
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    code += "," + Convert.ToString(row["SUB_CD"]);
                }
            }
            return code;
        }
        catch
        {
            throw;
        }
    }
    public DataTable getCATEGORY_ITEM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * From TB_9_M_COMM_D");
            sb.AppendLine(" where SYS_CD = '99' and MAIN_CD = 'CATEGORY_ITEM' ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public bool isManager(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * from TB_H_R_HEAD_DEPT");
            sb.AppendLine(" where EMP_ID = @EMP_ID  ");
            ht.Add("@EMP_ID", emp_id);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        catch
        {
            throw;
        }
    }
    public string getDepartment()
    {
        ACESLib.ACES aces = new ACESLib.ACES();
        List<string> all_departments = new List<string>();
        string department = "";
        string final_departments = "";
        try
        {
            foreach (string DB_ROLE_CD in aces.GetRoles().Split(',')) //取得「資料角色代碼」
            {
                //string DB_ROLE_CD = "FB2DBADMIN";
                department = ((ACESLib.DEPTBean)aces.GetDEPTAuth(DB_ROLE_CD)).Departments; //取得「使用其它部門權限」
                all_departments.Add(department);
            }
            if (all_departments.Count > 0)
            {
                List<string> departments = new List<string>();
                for (int i = 0; i < all_departments.Count; i++)
                {
                    for (int k = 0; k < all_departments[i].Split(',').Length; k++)
                    {
                        string temp = all_departments[i].Split(',')[k].Trim();
                        if (departments.Contains(temp))
                            continue;

                        departments.Add(temp);
                    }
                }

                for (int i = 0; i < departments.Count; i++)
                {
                    if (i == 0)
                    {
                        final_departments = departments[i];
                        continue;
                    }
                    final_departments += "," + departments[i];
                }
            }
        }
        catch
        {
        }
        return final_departments;
    }
    //SupertUser可以看到所有資料
    //public DataTable getSuperData(int startRowIndex, int maximumRows, string sortExpression, string sys_kind, string table_name
    //                         , string sys_fun, string category_item, string updated_by, string updated_dt_date_s
    //                         , string updated_dt_hour_s, string updated_dt_min_s, string updated_dt_date_e, string updated_dt_hour_e
    //                         , string updated_dt_min_e, string edit_infor)
    //{
    //    try
    //    {
    //        string logtable = "";

    //        StringBuilder sb1 = new StringBuilder();
    //        Hashtable ht1 = new Hashtable(); //依使用者所選的系統功能FUNCTION_ID取到MODE_ID
    //        sb1.AppendLine(" select * from TB_9_M_SYS_D where FUNCTION_ID=@FUNCTION_ID");
    //        ht1.Add("@FUNCTION_ID", sys_fun);
    //        DataTable mode_dt = dbConn.Query(sb1, ht1);
    //        string mode_id = Convert.ToString(mode_dt.Rows[0][0]);

    //        StringBuilder sb2 = new StringBuilder();
    //        Hashtable ht2 = new Hashtable();//以MODE_ID取到SYS_ID
    //        sb2.AppendLine(" select * from TB_9_M_SYS_M where MODE_ID=@MODE_ID");
    //        ht2.Add("@MODE_ID", mode_id);
    //        DataTable sys_dt = dbConn.Query(sb2, ht2);
    //        string sys_id = Convert.ToString(sys_dt.Rows[0][0]);

    //        if (sys_id == "D")
    //            logtable = " TB_D_R_LOG";
    //        else if (sys_id == "H")
    //            logtable = " TB_H_R_LOG";
    //        else if (sys_id == "I")
    //            logtable = " TB_I_R_LOG";
    //        else if (sys_id == "S")
    //            logtable = " TB_S_R_LOG";

    //        StringBuilder sb3 = new StringBuilder();
    //        Hashtable ht3 = new Hashtable();
    //        sb3.AppendLine("   select * From");
    //        sb3.AppendLine("    (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* from");
    //        sb3.AppendLine("      ( select L.TB_NAME as TB_NAME, L.TB_NAME+'('+L.TB_DESC+')' AS TB ");
    //        sb3.AppendLine("                ,L.FUNC_NAME as FUNC_NAME, L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC ");
    //        sb3.AppendLine("                ,L.CATEGORY as CATEGORY, L.CATEGORY +'-'+ D.SUB_DESC as CATEGORY_DESC");
    //        sb3.AppendLine("                ,L.PK_COLUMN as PK_COLUMN, E.EMP_NAME as UPDATED_BY, L.UPDATED_DT as UPDATED_DT, L.EDIT_INFOR as EDIT_INFOR ");
    //        sb3.AppendLine("          from " + logtable + " L");
    //        sb3.AppendLine("          left join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY ");
    //        sb3.AppendLine("          left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
    //        sb3.AppendLine("          where 1=1 ");

    //        //檔案
    //        if (table_name != "")
    //        {
    //            sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
    //            ht3.Add("@TABLE_NAME", table_name);
    //        }
    //        //系統功能
    //        sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
    //        ht3.Add("@FUNC_ID", sys_fun);
    //        //異動類別
    //        if (category_item != "")
    //        {
    //            sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
    //            ht3.Add("@CATEGORY", category_item);
    //        }
    //        //修改者
    //        if (updated_by != "")
    //        {
    //            sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
    //            ht3.Add("@updated_by", updated_by);
    //        }
    //        //修改日期
    //        string updated_dt_s = updated_dt_date_s + " " + updated_dt_hour_s + ":" + updated_dt_min_s;
    //        string updated_dt_e = updated_dt_date_e + " " + updated_dt_hour_e + ":" + updated_dt_min_e;
    //        sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");
    //        ht3.Add("@updated_dt_s", updated_dt_s);
    //        ht3.Add("@updated_dt_e", updated_dt_e);

    //        //關鍵字
    //        if (edit_infor != "")
    //        {
    //            sb3.AppendLine(" and ( L.PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
    //            sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
    //            ht3.Add("@EDIT_INFOR", edit_infor);
    //        }
    //        sb3.AppendLine("        )a ");
    //        sb3.AppendLine("    )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
    //        sb3.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

    //        ht3.Add("@startRowIndex", startRowIndex);
    //        ht3.Add("@maximumRows", maximumRows);
    //        return dbConn.Query(sb3, ht3, true);

    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}
    //public int getSuperCount(int startRowIndex, int maximumRows, string sys_kind, string table_name
    //                         , string sys_fun, string category_item, string updated_by, string updated_dt_date_s
    //                         , string updated_dt_hour_s, string updated_dt_min_s, string updated_dt_date_e, string updated_dt_hour_e
    //                         , string updated_dt_min_e, string edit_infor)
    //{
    //    try
    //    {
    //        StringBuilder sb1 = new StringBuilder();
    //        Hashtable ht1 = new Hashtable(); //依使用者所選的系統功能FUNCTION_ID取到MODE_ID
    //        sb1.AppendLine(" select * from TB_9_M_SYS_D where FUNCTION_ID=@FUNCTION_ID");
    //        ht1.Add("@FUNCTION_ID", sys_fun);
    //        DataTable mode_dt = dbConn.Query(sb1, ht1);
    //        string mode_id = Convert.ToString(mode_dt.Rows[0][0]);

    //        StringBuilder sb2 = new StringBuilder();
    //        Hashtable ht2 = new Hashtable();//以MODE_ID取到SYS_ID
    //        sb2.AppendLine(" select * from TB_9_M_SYS_M where MODE_ID=@MODE_ID");
    //        ht2.Add("@MODE_ID", mode_id);
    //        DataTable sys_dt = dbConn.Query(sb2, ht2);
    //        string sys_id = Convert.ToString(sys_dt.Rows[0][0]);

    //        StringBuilder sb3 = new StringBuilder();
    //        Hashtable ht3 = new Hashtable();
    //        if (sys_id == "D")
    //        {
    //            sb3.AppendLine(" select COUNT(*) total_record  from TB_D_R_LOG");
    //        }
    //        else if (sys_id == "H")
    //        {
    //            sb3.AppendLine(" select COUNT(*) total_record  from TB_H_R_LOG");
    //        }
    //        else if (sys_id == "I")
    //        {
    //            sb3.AppendLine(" select COUNT(*) total_record  from TB_I_R_LOG");
    //        }
    //        else if (sys_id == "S")
    //        {
    //            sb3.AppendLine(" select COUNT(*) total_record  from TB_S_R_LOG");
    //        }
    //        sb3.AppendLine(" where 1=1");
    //        //檔案
    //        if (table_name != "")
    //        {
    //            sb3.AppendLine(" and TB_NAME like '%'+@TABLE_NAME+'%' ");
    //            ht3.Add("@TABLE_NAME", table_name);
    //        }
    //        //系統功能
    //        sb3.AppendLine(" and FUNC_ID like '%'+@FUNC_ID+'%' ");
    //        ht3.Add("@FUNC_ID", sys_fun);
    //        //異動類別
    //        if (category_item != "")
    //        {
    //            sb3.AppendLine(" and CATEGORY = @CATEGORY ");
    //            ht3.Add("@CATEGORY", category_item);
    //        }
    //        //修改者
    //        if (updated_by != "")
    //        {
    //            sb3.AppendLine(" and UPDATED_BY like '%'+@updated_by+'%' ");
    //            ht3.Add("@updated_by", updated_by);
    //        }
    //        //修改日期
    //        string updated_dt_s = updated_dt_date_s + " " + updated_dt_hour_s + ":" + updated_dt_min_s;
    //        string updated_dt_e = updated_dt_date_e + " " + updated_dt_hour_e + ":" + updated_dt_min_e;
    //        sb3.AppendLine(" and (UPDATED_DT >= @updated_dt_s and UPDATED_DT <= @updated_dt_e) ");
    //        ht3.Add("@updated_dt_s", updated_dt_s);
    //        ht3.Add("@updated_dt_e", updated_dt_e);

    //        //關鍵字
    //        if (edit_infor != "")
    //        {
    //            sb3.AppendLine(" and ( PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
    //            sb3.AppendLine(" or EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
    //            ht3.Add("@EDIT_INFOR", edit_infor);
    //        }

    //        DataTable dt = dbConn.Query(sb3, ht3, true);
    //        int t = 0;
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
    ////主管可看到，修改者為自己所管轄部門員工的資料
    //public DataTable getManagerData(int startRowIndex, int maximumRows, string sortExpression, string sys_kind, string table_name
    //                         , string sys_fun, string category_item, string updated_by, string updated_dt_date_s
    //                         , string updated_dt_hour_s, string updated_dt_min_s, string updated_dt_date_e, string updated_dt_hour_e
    //                         , string updated_dt_min_e, string edit_infor)
    //{
    //    try
    //    {
           
    //        string departments = getDepartment(); //取得「使用其它部門權限」

    //        StringBuilder sb1 = new StringBuilder();
    //        Hashtable ht1 = new Hashtable(); //依使用者所選的系統功能FUNCTION_ID取到MODE_ID
    //        sb1.AppendLine(" select * from TB_9_M_SYS_D where FUNCTION_ID=@FUNCTION_ID");
    //        ht1.Add("@FUNCTION_ID", sys_fun);
    //        DataTable mode_dt = dbConn.Query(sb1, ht1);
    //        string mode_id = Convert.ToString(mode_dt.Rows[0][0]);

    //        StringBuilder sb2 = new StringBuilder();
    //        Hashtable ht2 = new Hashtable();//以MODE_ID取到SYS_ID
    //        sb2.AppendLine(" select * from TB_9_M_SYS_M where MODE_ID=@MODE_ID");
    //        ht2.Add("@MODE_ID", mode_id);
    //        DataTable sys_dt = dbConn.Query(sb2, ht2);
    //        string sys_id = Convert.ToString(sys_dt.Rows[0][0]);

    //        string logtable = "";
    //        StringBuilder sb3 = new StringBuilder();
    //        Hashtable ht3 = new Hashtable();

    //        sb3.AppendLine(" select * from ");
    //        sb3.AppendLine(" (");

    //        //第一個
    //        sb3.AppendLine(" select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* from ( ");
    //        sb3.AppendLine("   select  L.TB_NAME as TB_NAME, L.TB_NAME+'('+L.TB_DESC+')' AS TB ");
    //        sb3.AppendLine("          ,L.FUNC_NAME as FUNC_NAME, L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC ");
    //        sb3.AppendLine("          ,L.CATEGORY as CATEGORY, L.CATEGORY +'-'+ D.SUB_DESC as CATEGORY_DESC");
    //        sb3.AppendLine("          ,L.PK_COLUMN as PK_COLUMN, E.EMP_NAME as UPDATED_BY, L.UPDATED_DT as UPDATED_DT, L.EDIT_INFOR as EDIT_INFOR ");
    //        if (sys_id == "D")
    //        {
    //            logtable = " TB_D_R_LOG";
    //        }
    //        else if (sys_id == "H")
    //        {
    //            logtable = " TB_H_R_LOG";
    //        }
    //        else if (sys_id == "I")
    //        {
    //            logtable = " TB_I_R_LOG";
    //        }
    //        else if (sys_id == "S")
    //        {
    //            logtable = " TB_S_R_LOG";
    //        }

    //        sb3.AppendLine(" from " + logtable + " L");
    //        sb3.AppendLine(" left join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY");
    //        sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
    //        sb3.AppendLine(" where 1=1 ");

    //        //檔案
    //        if (table_name != "")
    //        {
    //            sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
    //            ht3.Add("@TABLE_NAME", table_name);
    //        }
    //        //系統功能
    //        sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
    //        ht3.Add("@FUNC_ID", sys_fun);
    //        //異動類別
    //        if (category_item != "")
    //        {
    //            sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
    //            ht3.Add("@CATEGORY", category_item);
    //        }
    //        //修改者
    //        if (updated_by != "")
    //        {
    //            sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
    //            ht3.Add("@updated_by", updated_by);
    //        }
    //        //修改日期
    //        string updated_dt_s = updated_dt_date_s + " " + updated_dt_hour_s + ":" + updated_dt_min_s;
    //        string updated_dt_e = updated_dt_date_e + " " + updated_dt_hour_e + ":" + updated_dt_min_e;
    //        sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");
    //        ht3.Add("@updated_dt_s", updated_dt_s);
    //        ht3.Add("@updated_dt_e", updated_dt_e);

    //        //關鍵字
    //        if (edit_infor != "")
    //        {
    //            sb3.AppendLine(" and ( L.PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
    //            sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
    //            ht3.Add("@EDIT_INFOR", edit_infor);
    //        }
    //        //select 登入者所有管理的部門
    //        sb3.AppendLine(" and E.DEPT_NO in (select MNG_DEPT_NO from TB_H_R_HEAD_DEPT where EMP_ID=@EMP_ID) ");
    //        ht3.Add("@logtable", logtable);
    //        ht3.Add("@EMP_ID", SessionHandle.Current.emp_id);

    //        if (departments != "")
    //        {
    //            sb3.AppendLine(" union");

    //            //第二個
    //            sb3.AppendLine("   select  L.TB_NAME as TB_NAME, L.TB_NAME+'('+L.TB_DESC+')' AS TB ");
    //            sb3.AppendLine("          ,L.FUNC_NAME as FUNC_NAME, L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC ");
    //            sb3.AppendLine("          ,L.CATEGORY as CATEGORY, L.CATEGORY +'-'+ D.SUB_DESC as CATEGORY_DESC");
    //            sb3.AppendLine("          ,L.PK_COLUMN as PK_COLUMN, E.EMP_NAME as UPDATED_BY, L.UPDATED_DT as UPDATED_DT, L.EDIT_INFOR as EDIT_INFOR ");
    //            sb3.AppendLine(" from " + logtable + " L");
    //            sb3.AppendLine(" left join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY");
    //            sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
    //            sb3.AppendLine(" where 1=1 ");

    //            //檔案
    //            if (table_name != "")
    //            {
    //                sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
    //            }
    //            //系統功能
    //            sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
    //            //異動類別
    //            if (category_item != "")
    //            {
    //                sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
    //            }
    //            //修改者
    //            if (updated_by != "")
    //            {
    //                sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
    //            }
    //            //修改日期
    //            sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");

    //            //關鍵字
    //            if (edit_infor != "")
    //            {
    //                sb3.AppendLine(" and ( L.PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
    //                sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
    //            }
    //            //ACES設定的可看部門權限
    //            string[] arrDepartments = departments.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    //            string strInParams = string.Empty;
    //            for (int i = 0; i < arrDepartments.Length; i++)
    //            {
    //                strInParams += "@department" + i.ToString() + ",";
    //                ht3.Add("@department" + i.ToString(), arrDepartments[i].Trim());
    //            }
    //            sb3.AppendLine(" and E.DEPT_NO in (" + strInParams.Trim(',') + ")");
    //        }
    //        sb3.AppendLine(" union");

    //        //第三個
    //        sb3.AppendLine("   select  L.TB_NAME as TB_NAME, L.TB_NAME+'('+L.TB_DESC+')' AS TB ");
    //        sb3.AppendLine("          ,L.FUNC_NAME as FUNC_NAME, L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC ");
    //        sb3.AppendLine("          ,L.CATEGORY as CATEGORY, L.CATEGORY +'-'+ D.SUB_DESC as CATEGORY_DESC");
    //        sb3.AppendLine("          ,L.PK_COLUMN as PK_COLUMN, E.EMP_NAME as UPDATED_BY, L.UPDATED_DT as UPDATED_DT, L.EDIT_INFOR as EDIT_INFOR ");
    //        sb3.AppendLine(" from " + logtable + " L");
    //        sb3.AppendLine(" left join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY");
    //        sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
    //        sb3.AppendLine(" where 1=1 ");

    //        //檔案
    //        if (table_name != "")
    //        {
    //            sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
    //        }
    //        //系統功能
    //        sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
    //        //異動類別
    //        if (category_item != "")
    //        {
    //            sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
    //        }
    //        //修改者
    //        if (updated_by != "")
    //        {
    //            sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
    //        }
    //        //修改日期
    //        sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");

    //        //關鍵字
    //        if (edit_infor != "")
    //        {
    //            sb3.AppendLine(" and (L. PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
    //            sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
    //        }
    //        //登入者只能看到自己修改的資料
    //        sb3.AppendLine(" and L.UPDATED_BY = @updated_by");
    //        ht3.Add("@updated_by", SessionHandle.Current.emp_id);

    //        sb3.AppendLine(" )a");
    //        sb3.AppendLine(" )");
    //        sb3.AppendLine(" god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
    //        sb3.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

    //        ht3.Add("@startRowIndex", startRowIndex);
    //        ht3.Add("@maximumRows", maximumRows);
    //        return dbConn.Query(sb3, ht3, true);
    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}

    //public int getManagerCount(int startRowIndex, int maximumRows, string sys_kind, string table_name
    //                         , string sys_fun, string category_item, string updated_by, string updated_dt_date_s
    //                         , string updated_dt_hour_s, string updated_dt_min_s, string updated_dt_date_e, string updated_dt_hour_e
    //                         , string updated_dt_min_e, string edit_infor)
    //{
    //    try
    //    {
    //        string departments = getDepartment(); //取得「使用其它部門權限」

    //        StringBuilder sb1 = new StringBuilder();
    //        Hashtable ht1 = new Hashtable(); //依使用者所選的系統功能FUNCTION_ID取到MODE_ID
    //        sb1.AppendLine(" select * from TB_9_M_SYS_D where FUNCTION_ID=@FUNCTION_ID");
    //        ht1.Add("@FUNCTION_ID", sys_fun);
    //        DataTable mode_dt = dbConn.Query(sb1, ht1);
    //        string mode_id = Convert.ToString(mode_dt.Rows[0][0]);

    //        StringBuilder sb2 = new StringBuilder();
    //        Hashtable ht2 = new Hashtable();//以MODE_ID取到SYS_ID
    //        sb2.AppendLine(" select * from TB_9_M_SYS_M where MODE_ID=@MODE_ID");
    //        ht2.Add("@MODE_ID", mode_id);
    //        DataTable sys_dt = dbConn.Query(sb2, ht2);
    //        string sys_id = Convert.ToString(sys_dt.Rows[0][0]);

    //        string logtable = "";
    //        StringBuilder sb3 = new StringBuilder();
    //        Hashtable ht3 = new Hashtable();
    //        sb3.AppendLine(" select COUNT(*) as total_record from (");

    //        sb3.AppendLine("   select  L.TB_NAME as TB_NAME, L.TB_NAME+'('+L.TB_DESC+')' AS TB ");
    //        sb3.AppendLine("          ,L.FUNC_NAME as FUNC_NAME, L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC ");
    //        sb3.AppendLine("          ,L.CATEGORY as CATEGORY, L.CATEGORY +'-'+ D.SUB_DESC as CATEGORY_DESC");
    //        sb3.AppendLine("          ,L.PK_COLUMN as PK_COLUMN, E.EMP_NAME as UPDATED_BY, L.UPDATED_DT as UPDATED_DT, L.EDIT_INFOR as EDIT_INFOR ");
    //        if (sys_id == "D")
    //        {
    //            logtable = " TB_D_R_LOG";
    //        }
    //        else if (sys_id == "H")
    //        {
    //            logtable = " TB_H_R_LOG";
    //        }
    //        else if (sys_id == "I")
    //        {
    //            logtable = " TB_I_R_LOG";
    //        }
    //        else if (sys_id == "S")
    //        {
    //            logtable = " TB_S_R_LOG";
    //        }

    //        sb3.AppendLine(" from " + logtable + " L");
    //        sb3.AppendLine(" left join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY");
    //        sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
    //        sb3.AppendLine(" where 1=1 ");

    //        //檔案
    //        if (table_name != "")
    //        {
    //            sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
    //            ht3.Add("@TABLE_NAME", table_name);
    //        }
    //        //系統功能
    //        sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
    //        ht3.Add("@FUNC_ID", sys_fun);
    //        //異動類別
    //        if (category_item != "")
    //        {
    //            sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
    //            ht3.Add("@CATEGORY", category_item);
    //        }
    //        //修改者
    //        if (updated_by != "")
    //        {
    //            sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
    //            ht3.Add("@updated_by", updated_by);
    //        }
    //        //修改日期
    //        string updated_dt_s = updated_dt_date_s + " " + updated_dt_hour_s + ":" + updated_dt_min_s;
    //        string updated_dt_e = updated_dt_date_e + " " + updated_dt_hour_e + ":" + updated_dt_min_e;
    //        sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");
    //        ht3.Add("@updated_dt_s", updated_dt_s);
    //        ht3.Add("@updated_dt_e", updated_dt_e);

    //        //關鍵字
    //        if (edit_infor != "")
    //        {
    //            sb3.AppendLine(" and ( L.PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
    //            sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
    //            ht3.Add("@EDIT_INFOR", edit_infor);
    //        }
    //        //select 登入者所有管理的部門
    //        sb3.AppendLine(" and E.DEPT_NO in (select MNG_DEPT_NO from TB_H_R_HEAD_DEPT where EMP_ID=@EMP_ID) ");
    //        ht3.Add("@logtable", logtable);
    //        ht3.Add("@EMP_ID", SessionHandle.Current.emp_id);

    //        if (departments != "")
    //        {
    //            sb3.AppendLine(" union");

    //            //第二個
    //            sb3.AppendLine("   select  L.TB_NAME as TB_NAME, L.TB_NAME+'('+L.TB_DESC+')' AS TB ");
    //            sb3.AppendLine("          ,L.FUNC_NAME as FUNC_NAME, L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC ");
    //            sb3.AppendLine("          ,L.CATEGORY as CATEGORY, L.CATEGORY +'-'+ D.SUB_DESC as CATEGORY_DESC");
    //            sb3.AppendLine("          ,L.PK_COLUMN as PK_COLUMN, E.EMP_NAME as UPDATED_BY, L.UPDATED_DT as UPDATED_DT, L.EDIT_INFOR as EDIT_INFOR ");
    //            sb3.AppendLine(" from " + logtable + " L");
    //            sb3.AppendLine(" left join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY");
    //            sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
    //            sb3.AppendLine(" where 1=1 ");

    //            //檔案
    //            if (table_name != "")
    //            {
    //                sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
    //            }
    //            //系統功能
    //            sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
    //            //異動類別
    //            if (category_item != "")
    //            {
    //                sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
    //            }
    //            //修改者
    //            if (updated_by != "")
    //            {
    //                sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
    //            }
    //            //修改日期
    //            sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");

    //            //關鍵字
    //            if (edit_infor != "")
    //            {
    //                sb3.AppendLine(" and ( L.PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
    //                sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
    //            }
    //            //ACES設定的可看部門權限
    //            string[] arrDepartments = departments.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    //            string strInParams = string.Empty;
    //            for (int i = 0; i < arrDepartments.Length; i++)
    //            {
    //                strInParams += "@department" + i.ToString() + ",";
    //                ht3.Add("@department" + i.ToString(), arrDepartments[i].Trim());
    //            }
    //            sb3.AppendLine(" and E.DEPT_NO in (" + strInParams.Trim(',') + ")");
    //        }
    //        sb3.AppendLine(" union");

    //        //第三個
    //        sb3.AppendLine("   select  L.TB_NAME as TB_NAME, L.TB_NAME+'('+L.TB_DESC+')' AS TB ");
    //        sb3.AppendLine("          ,L.FUNC_NAME as FUNC_NAME, L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC ");
    //        sb3.AppendLine("          ,L.CATEGORY as CATEGORY, L.CATEGORY +'-'+ D.SUB_DESC as CATEGORY_DESC");
    //        sb3.AppendLine("          ,L.PK_COLUMN as PK_COLUMN, E.EMP_NAME as UPDATED_BY, L.UPDATED_DT as UPDATED_DT, L.EDIT_INFOR as EDIT_INFOR ");
    //        sb3.AppendLine(" from " + logtable + " L");
    //        sb3.AppendLine(" left join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY");
    //        sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
    //        sb3.AppendLine(" where 1=1 ");

    //        //檔案
    //        if (table_name != "")
    //        {
    //            sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
    //        }
    //        //系統功能
    //        sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
    //        //異動類別
    //        if (category_item != "")
    //        {
    //            sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
    //        }
    //        //修改者
    //        if (updated_by != "")
    //        {
    //            sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
    //        }
    //        //修改日期
    //        sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");

    //        //關鍵字
    //        if (edit_infor != "")
    //        {
    //            sb3.AppendLine(" and (L. PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
    //            sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
    //        }
    //        //登入者只能看到自己修改的資料
    //        sb3.AppendLine(" and L.UPDATED_BY = @updated_by");
    //        ht3.Add("@updated_by", SessionHandle.Current.emp_id);

    //        sb3.AppendLine(" )a");

    //        DataTable dt = dbConn.Query(sb3, ht3, true);
    //        int t = 0;
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
    ////登入者只能看到自己的資料
    //public DataTable getData1(int startRowIndex, int maximumRows, string sortExpression, string sys_kind, string table_name
    //                         , string sys_fun, string category_item, string updated_by, string updated_dt_date_s
    //                         , string updated_dt_hour_s, string updated_dt_min_s, string updated_dt_date_e, string updated_dt_hour_e
    //                         , string updated_dt_min_e, string edit_infor)
    //{
    //    try
    //    {
    //        string departments = getDepartment(); //取得「使用其它部門權限」

    //        StringBuilder sb1 = new StringBuilder();
    //        Hashtable ht1 = new Hashtable(); //依使用者所選的系統功能FUNCTION_ID取到MODE_ID
    //        sb1.AppendLine(" select * from TB_9_M_SYS_D where FUNCTION_ID=@FUNCTION_ID");
    //        ht1.Add("@FUNCTION_ID", sys_fun);
    //        DataTable mode_dt = dbConn.Query(sb1, ht1);
    //        string mode_id = Convert.ToString(mode_dt.Rows[0][0]);

    //        StringBuilder sb2 = new StringBuilder();
    //        Hashtable ht2 = new Hashtable();//以MODE_ID取到SYS_ID
    //        sb2.AppendLine(" select * from TB_9_M_SYS_M where MODE_ID=@MODE_ID");
    //        ht2.Add("@MODE_ID", mode_id);
    //        DataTable sys_dt = dbConn.Query(sb2, ht2);
    //        string sys_id = Convert.ToString(sys_dt.Rows[0][0]);

    //        string logtable = "";
    //        StringBuilder sb3 = new StringBuilder();
    //        Hashtable ht3 = new Hashtable();
    //        sb3.AppendLine(" select * from ");
    //        sb3.AppendLine(" (");

    //        //第一個
    //        sb3.AppendLine(" select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* from ( ");
    //        sb3.AppendLine(" select  L.TB_NAME as TB_NAME, L.TB_NAME+'('+L.TB_DESC+')' AS TB ");
    //        sb3.AppendLine("          ,L.FUNC_NAME as FUNC_NAME, L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC ");
    //        sb3.AppendLine("          ,L.CATEGORY as CATEGORY, L.CATEGORY +'-'+ D.SUB_DESC as CATEGORY_DESC");
    //        sb3.AppendLine("          ,L.PK_COLUMN as PK_COLUMN, E.EMP_NAME as UPDATED_BY, L.UPDATED_DT as UPDATED_DT, L.EDIT_INFOR as EDIT_INFOR ");
    //        if (sys_id == "D")
    //        {
    //            logtable = " TB_D_R_LOG";
    //        }
    //        else if (sys_id == "H")
    //        {
    //            logtable = " TB_H_R_LOG";
    //        }
    //        else if (sys_id == "I")
    //        {
    //            logtable = " TB_I_R_LOG";
    //        }
    //        else if (sys_id == "S")
    //        {
    //            logtable = " TB_S_R_LOG";
    //        }

    //        sb3.AppendLine(" from " + logtable + " L");
    //        sb3.AppendLine(" left join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY");
    //        sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
    //        sb3.AppendLine(" where 1=1 ");

    //        //檔案
    //        if (table_name != "")
    //        {
    //            sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
    //            ht3.Add("@TABLE_NAME", table_name);
    //        }
    //        //系統功能
    //        sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
    //        ht3.Add("@FUNC_ID", sys_fun);
    //        //異動類別
    //        if (category_item != "")
    //        {
    //            sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
    //            ht3.Add("@CATEGORY", category_item);
    //        }
    //        //修改者
    //        if (updated_by != "")
    //        {
    //            sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
    //            ht3.Add("@updated_by", updated_by);
    //        }
    //        //修改日期
    //        string updated_dt_s = updated_dt_date_s + " " + updated_dt_hour_s + ":" + updated_dt_min_s;
    //        string updated_dt_e = updated_dt_date_e + " " + updated_dt_hour_e + ":" + updated_dt_min_e;
    //        sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");
    //        ht3.Add("@updated_dt_s", updated_dt_s);
    //        ht3.Add("@updated_dt_e", updated_dt_e);

    //        //關鍵字
    //        if (edit_infor != "")
    //        {
    //            sb3.AppendLine(" and ( L.PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
    //            sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
    //            ht3.Add("@EDIT_INFOR", edit_infor);
    //        }
    //        //登入者只能看到自己修改的資料
    //        sb3.AppendLine(" and L.UPDATED_BY = @updated_by");
    //        ht3.Add("@updated_by", SessionHandle.Current.emp_id);
    //        //ht3.Add("@updated_by", "10829");

    //        if (departments != "")
    //        {
    //            sb3.AppendLine(" union");

    //            //第二個
    //            sb3.AppendLine(" select  L.TB_NAME as TB_NAME, L.TB_NAME+'('+L.TB_DESC+')' AS TB ");
    //            sb3.AppendLine("          ,L.FUNC_NAME as FUNC_NAME, L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC ");
    //            sb3.AppendLine("          ,L.CATEGORY as CATEGORY, L.CATEGORY +'-'+ D.SUB_DESC as CATEGORY_DESC");
    //            sb3.AppendLine("          ,L.PK_COLUMN as PK_COLUMN, E.EMP_NAME as UPDATED_BY, L.UPDATED_DT as UPDATED_DT, L.EDIT_INFOR as EDIT_INFOR ");
    //            sb3.AppendLine(" from " + logtable + " L");
    //            sb3.AppendLine(" left join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY");
    //            sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
    //            sb3.AppendLine(" where 1=1 ");

    //            //檔案
    //            if (table_name != "")
    //            {
    //                sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
    //            }
    //            //系統功能
    //            sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
    //            //異動類別
    //            if (category_item != "")
    //            {
    //                sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
    //            }
    //            //修改者
    //            if (updated_by != "")
    //            {
    //                sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
    //            }
    //            //修改日期
    //            sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");

    //            //關鍵字
    //            if (edit_infor != "")
    //            {
    //                sb3.AppendLine(" and ( L.PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
    //                sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
    //            }
    //            //ACES設定的可看部門權限
    //            string[] arrDepartments = departments.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    //            string strInParams = string.Empty;
    //            for (int i = 0; i < arrDepartments.Length; i++)
    //            {
    //                strInParams += "@department" + i.ToString() + ",";
    //                ht3.Add("@department" + i.ToString(), arrDepartments[i].Trim());
    //            }
    //            sb3.AppendLine(" and E.DEPT_NO in (" + strInParams.Trim(',') + ")");
    //        }
    //        sb3.AppendLine(" )a");
    //        sb3.AppendLine(" )");
    //        sb3.AppendLine(" god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
    //        sb3.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

    //        ht3.Add("@startRowIndex", startRowIndex);
    //        ht3.Add("@maximumRows", maximumRows);
    //        return dbConn.Query(sb3, ht3, true);

    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}
    //public int getCount1(int startRowIndex, int maximumRows, string sys_kind, string table_name
    //                         , string sys_fun, string category_item, string updated_by, string updated_dt_date_s
    //                         , string updated_dt_hour_s, string updated_dt_min_s, string updated_dt_date_e, string updated_dt_hour_e
    //                         , string updated_dt_min_e, string edit_infor)
    //{
    //    try
    //    {
    //        string departments = getDepartment(); //取得「使用其它部門權限」

    //        StringBuilder sb1 = new StringBuilder();
    //        Hashtable ht1 = new Hashtable(); //依使用者所選的系統功能FUNCTION_ID取到MODE_ID
    //        sb1.AppendLine(" select * from TB_9_M_SYS_D where FUNCTION_ID=@FUNCTION_ID");
    //        ht1.Add("@FUNCTION_ID", sys_fun);
    //        DataTable mode_dt = dbConn.Query(sb1, ht1);
    //        string mode_id = Convert.ToString(mode_dt.Rows[0][0]);

    //        StringBuilder sb2 = new StringBuilder();
    //        Hashtable ht2 = new Hashtable();//以MODE_ID取到SYS_ID
    //        sb2.AppendLine(" select * from TB_9_M_SYS_M where MODE_ID=@MODE_ID");
    //        ht2.Add("@MODE_ID", mode_id);
    //        DataTable sys_dt = dbConn.Query(sb2, ht2);
    //        string sys_id = Convert.ToString(sys_dt.Rows[0][0]);

    //        string logtable = "";
    //        StringBuilder sb3 = new StringBuilder();
    //        Hashtable ht3 = new Hashtable();
    //        sb3.AppendLine(" select COUNT(*) as total_record from (");

    //        //第一個
    //        sb3.AppendLine(" select L.TB_NAME+'('+L.TB_DESC+')' AS TB,L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC,L.CATEGORY,L.PK_COLUMN,E.EMP_NAME as UPDATED_BY,L.UPDATED_DT,L.EDIT_INFOR");
    //        if (sys_id == "D")
    //        {
    //            logtable = " TB_D_R_LOG";
    //        }
    //        else if (sys_id == "H")
    //        {
    //            logtable = " TB_H_R_LOG";
    //        }
    //        else if (sys_id == "I")
    //        {
    //            logtable = " TB_I_R_LOG";
    //        }
    //        else if (sys_id == "S")
    //        {
    //            logtable = " TB_S_R_LOG";
    //        }

    //        sb3.AppendLine(" from " + logtable + " L");
    //        sb3.AppendLine(" join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY");
    //        sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
    //        sb3.AppendLine(" where 1=1 ");

    //        //檔案
    //        if (table_name != "")
    //        {
    //            sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
    //            ht3.Add("@TABLE_NAME", table_name);
    //        }
    //        //系統功能
    //        sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
    //        ht3.Add("@FUNC_ID", sys_fun);
    //        //異動類別
    //        if (category_item != "")
    //        {
    //            sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
    //            ht3.Add("@CATEGORY", category_item);
    //        }
    //        //修改者
    //        if (updated_by != "")
    //        {
    //            sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
    //            ht3.Add("@updated_by", updated_by);
    //        }
    //        //修改日期
    //        string updated_dt_s = updated_dt_date_s + " " + updated_dt_hour_s + ":" + updated_dt_min_s;
    //        string updated_dt_e = updated_dt_date_e + " " + updated_dt_hour_e + ":" + updated_dt_min_e;
    //        sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");
    //        ht3.Add("@updated_dt_s", updated_dt_s);
    //        ht3.Add("@updated_dt_e", updated_dt_e);

    //        //關鍵字
    //        if (edit_infor != "")
    //        {
    //            sb3.AppendLine(" and ( L.PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
    //            sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
    //            ht3.Add("@EDIT_INFOR", edit_infor);
    //        }
    //        //登入者只能看到自己修改的資料
    //        sb3.AppendLine(" and L.UPDATED_BY = @updated_by");
    //        ht3.Add("@updated_by", SessionHandle.Current.emp_id);

    //        if (departments != "")
    //        {
    //            sb3.AppendLine(" union");

    //            //第二個
    //            sb3.AppendLine(" select T.TB_NAME+'('+T.TB_DESC+')' AS TB,T.FUNC_NAME+'('+T.FUNC_ID+')' AS FUNC,T.CATEGORY,T.PK_COLUMN,E.EMP_NAME as UPDATED_BY,T.UPDATED_DT,T.EDIT_INFOR");
    //            sb3.AppendLine(" from " + logtable + " T");
    //            sb3.AppendLine(" join TB_H_M_EMP E on E.EMP_ID = T.UPDATED_BY");
    //            sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = T.CATEGORY ");
    //            sb3.AppendLine(" where 1=1 ");

    //            //檔案
    //            if (table_name != "")
    //            {
    //                sb3.AppendLine(" and T.TB_NAME like '%'+@TABLE_NAME+'%' ");
    //            }
    //            //系統功能
    //            sb3.AppendLine(" and T.FUNC_ID like '%'+@FUNC_ID+'%' ");
    //            //異動類別
    //            if (category_item != "")
    //            {
    //                sb3.AppendLine(" and T.CATEGORY = @CATEGORY ");
    //            }
    //            //修改者
    //            if (updated_by != "")
    //            {
    //                sb3.AppendLine(" and T.UPDATED_BY like '%'+@updated_by+'%' ");
    //            }
    //            //修改日期
    //            sb3.AppendLine(" and (T.UPDATED_DT >= @updated_dt_s and T.UPDATED_DT <= @updated_dt_e) ");

    //            //關鍵字
    //            if (edit_infor != "")
    //            {
    //                sb3.AppendLine(" and ( T.PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
    //                sb3.AppendLine(" or T.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
    //            }
    //            //ACES設定的可看部門權限
    //            string[] arrDepartments = departments.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    //            string strInParams = string.Empty;
    //            for (int i = 0; i < arrDepartments.Length; i++)
    //            {
    //                strInParams += "@department" + i.ToString() + ",";
    //                ht3.Add("@department" + i.ToString(), arrDepartments[i].Trim());
    //            }
    //            sb3.AppendLine(" and E.DEPT_NO in (" + strInParams.Trim(',') + ")");
    //        }
    //        sb3.AppendLine(" )a");

    //        DataTable dt = dbConn.Query(sb3, ht3,true);
    //        int t = 0;
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

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,string isSuper, string sys_kind, string table_name
                            , string sys_fun, string category_item, string updated_by, string updated_dt_date_s
                            , string updated_dt_hour_s, string updated_dt_min_s, string updated_dt_date_e, string updated_dt_hour_e
                            , string updated_dt_min_e, string edit_infor)
    {
        try
        {
            string departments = getDepartment(); //取得「使用其它部門權限」

            StringBuilder sb1 = new StringBuilder();
            Hashtable ht1 = new Hashtable(); //依使用者所選的系統功能FUNCTION_ID取到MODE_ID
            sb1.AppendLine(" select * from TB_9_M_SYS_D where FUNCTION_ID=@FUNCTION_ID");
            ht1.Add("@FUNCTION_ID", sys_fun);
            DataTable mode_dt = dbConn.Query(sb1, ht1);
            string mode_id = Convert.ToString(mode_dt.Rows[0][0]);

            StringBuilder sb2 = new StringBuilder();
            Hashtable ht2 = new Hashtable();//以MODE_ID取到SYS_ID
            sb2.AppendLine(" select * from TB_9_M_SYS_M where MODE_ID=@MODE_ID");
            ht2.Add("@MODE_ID", mode_id);
            DataTable sys_dt = dbConn.Query(sb2, ht2);
            string sys_id = Convert.ToString(sys_dt.Rows[0][0]);

            string logtable = "";
            StringBuilder sb3 = new StringBuilder();
            Hashtable ht3 = new Hashtable();

            sb3.AppendLine(" select * from ");
            sb3.AppendLine(" (");

            //第一個
            sb3.AppendLine(" select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* from ( ");
            sb3.AppendLine("   select  L.TB_NAME as TB_NAME, L.TB_NAME+'('+L.TB_DESC+')' AS TB ");
            sb3.AppendLine("          ,L.FUNC_NAME as FUNC_NAME, L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC ");
            sb3.AppendLine("          ,L.CATEGORY as CATEGORY, L.CATEGORY +'-'+ D.SUB_DESC as CATEGORY_DESC");
            sb3.AppendLine("          ,L.PK_COLUMN as PK_COLUMN, E.EMP_NAME as UPDATED_BY, L.UPDATED_DT as UPDATED_DT, L.EDIT_INFOR as EDIT_INFOR ");
            if (sys_id == "D")
            {
                logtable = " TB_D_R_LOG";
            }
            else if (sys_id == "H")
            {
                logtable = " TB_H_R_LOG";
            }
            else if (sys_id == "I")
            {
                logtable = " TB_I_R_LOG";
            }
            else if (sys_id == "S")
            {
                logtable = " TB_S_R_LOG";
            }

            sb3.AppendLine(" from " + logtable + " L");
            sb3.AppendLine(" left join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY");
            sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
            sb3.AppendLine(" where 1=1 ");

            //檔案
            if (table_name != "")
            {
                sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
                ht3.Add("@TABLE_NAME", table_name);
            }
            //系統功能
            sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
            ht3.Add("@FUNC_ID", sys_fun);
            //異動類別
            if (category_item != "")
            {
                sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
                ht3.Add("@CATEGORY", category_item);
            }
            //修改者
            if (updated_by != "")
            {
                sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
                ht3.Add("@updated_by", updated_by);
            }
            //修改日期
            string updated_dt_s = updated_dt_date_s + " " + updated_dt_hour_s + ":" + updated_dt_min_s;
            string updated_dt_e = updated_dt_date_e + " " + updated_dt_hour_e + ":" + updated_dt_min_e;
            sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");
            ht3.Add("@updated_dt_s", updated_dt_s);
            ht3.Add("@updated_dt_e", updated_dt_e);

            //關鍵字
            if (edit_infor != "")
            {
                sb3.AppendLine(" and ( L.PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
                sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
                ht3.Add("@EDIT_INFOR", edit_infor);
            }

            //20150606 修改super邏輯  改以session中的is_super來判定
            //if (isSuper != "Y")
            if (SessionHandle.Current.is_super != "Y")
            {
                //登入者只能看到自己修改的資料
                sb3.AppendLine(" and L.UPDATED_BY = @updated_byself");
                ht3.Add("@updated_byself", SessionHandle.Current.emp_id);

                if (departments != "")
                {
                    sb3.AppendLine(" union");

                    //第二個
                    sb3.AppendLine("   select  L.TB_NAME as TB_NAME, L.TB_NAME+'('+L.TB_DESC+')' AS TB ");
                    sb3.AppendLine("          ,L.FUNC_NAME as FUNC_NAME, L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC ");
                    sb3.AppendLine("          ,L.CATEGORY as CATEGORY, L.CATEGORY +'-'+ D.SUB_DESC as CATEGORY_DESC");
                    sb3.AppendLine("          ,L.PK_COLUMN as PK_COLUMN, E.EMP_NAME as UPDATED_BY, L.UPDATED_DT as UPDATED_DT, L.EDIT_INFOR as EDIT_INFOR ");
                    sb3.AppendLine(" from " + logtable + " L");
                    sb3.AppendLine(" left join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY");
                    sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
                    sb3.AppendLine(" where 1=1 ");

                    //檔案
                    if (table_name != "")
                    {
                        sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
                    }
                    //系統功能
                    sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
                    //異動類別
                    if (category_item != "")
                    {
                        sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
                    }
                    //修改者
                    if (updated_by != "")
                    {
                        sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
                    }
                    //修改日期
                    sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");

                    //關鍵字
                    if (edit_infor != "")
                    {
                        sb3.AppendLine(" and ( L.PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
                        sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
                    }
                    //ACES設定的可看部門權限
                    string[] arrDepartments = departments.Replace(" ", "").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string strInParams = string.Empty;
                    for (int i = 0; i < arrDepartments.Length; i++)
                    {
                        strInParams += "@department" + i.ToString() + ",";
                        ht3.Add("@department" + i.ToString(), arrDepartments[i].Trim());
                    }
                    sb3.AppendLine(" and E.DEPT_NO in (" + strInParams.Trim(',') + ")");
                }
                sb3.AppendLine(" union");

                //第三個
                sb3.AppendLine("   select  L.TB_NAME as TB_NAME, L.TB_NAME+'('+L.TB_DESC+')' AS TB ");
                sb3.AppendLine("          ,L.FUNC_NAME as FUNC_NAME, L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC ");
                sb3.AppendLine("          ,L.CATEGORY as CATEGORY, L.CATEGORY +'-'+ D.SUB_DESC as CATEGORY_DESC");
                sb3.AppendLine("          ,L.PK_COLUMN as PK_COLUMN, E.EMP_NAME as UPDATED_BY, L.UPDATED_DT as UPDATED_DT, L.EDIT_INFOR as EDIT_INFOR ");
                sb3.AppendLine(" from " + logtable + " L");
                sb3.AppendLine(" left join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY");
                sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
                sb3.AppendLine(" where 1=1 ");

                //檔案
                if (table_name != "")
                {
                    sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
                }
                //系統功能
                sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
                //異動類別
                if (category_item != "")
                {
                    sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
                }
                //修改者
                if (updated_by != "")
                {
                    sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
                }
                //修改日期
                sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");

                //關鍵字
                if (edit_infor != "")
                {
                    sb3.AppendLine(" and (L. PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
                    sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
                }
                //select 登入者所有管理的部門
                sb3.AppendLine(" and E.DEPT_NO in (select MNG_DEPT_NO from TB_H_R_HEAD_DEPT where EMP_ID=@EMP_ID) ");
                ht3.Add("@logtable", logtable);
                ht3.Add("@EMP_ID", SessionHandle.Current.emp_id);
            }
            

            sb3.AppendLine(" )a");
            sb3.AppendLine(" )");
            sb3.AppendLine(" god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb3.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht3.Add("@startRowIndex", startRowIndex);
            ht3.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb3, ht3, true);

        }
        catch
        {
            throw;
        }
    }
    public int getCount(int startRowIndex, int maximumRows, string isSuper, string sys_kind, string table_name
                             , string sys_fun, string category_item, string updated_by, string updated_dt_date_s
                             , string updated_dt_hour_s, string updated_dt_min_s, string updated_dt_date_e, string updated_dt_hour_e
                             , string updated_dt_min_e, string edit_infor)
    {
        try
        {
            string departments = getDepartment(); //取得「使用其它部門權限」

            StringBuilder sb1 = new StringBuilder();
            Hashtable ht1 = new Hashtable(); //依使用者所選的系統功能FUNCTION_ID取到MODE_ID
            sb1.AppendLine(" select * from TB_9_M_SYS_D where FUNCTION_ID=@FUNCTION_ID");
            ht1.Add("@FUNCTION_ID", sys_fun);
            DataTable mode_dt = dbConn.Query(sb1, ht1);
            string mode_id = Convert.ToString(mode_dt.Rows[0][0]);

            StringBuilder sb2 = new StringBuilder();
            Hashtable ht2 = new Hashtable();//以MODE_ID取到SYS_ID
            sb2.AppendLine(" select * from TB_9_M_SYS_M where MODE_ID=@MODE_ID");
            ht2.Add("@MODE_ID", mode_id);
            DataTable sys_dt = dbConn.Query(sb2, ht2);
            string sys_id = Convert.ToString(sys_dt.Rows[0][0]);

            string logtable = "";
            StringBuilder sb3 = new StringBuilder();
            Hashtable ht3 = new Hashtable();
            sb3.AppendLine(" select COUNT(*) as total_record from (");

            sb3.AppendLine("   select  L.TB_NAME as TB_NAME, L.TB_NAME+'('+L.TB_DESC+')' AS TB ");
            sb3.AppendLine("          ,L.FUNC_NAME as FUNC_NAME, L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC ");
            sb3.AppendLine("          ,L.CATEGORY as CATEGORY, L.CATEGORY +'-'+ D.SUB_DESC as CATEGORY_DESC");
            sb3.AppendLine("          ,L.PK_COLUMN as PK_COLUMN, E.EMP_NAME as UPDATED_BY, L.UPDATED_DT as UPDATED_DT, L.EDIT_INFOR as EDIT_INFOR ");
            if (sys_id == "D")
            {
                logtable = " TB_D_R_LOG";
            }
            else if (sys_id == "H")
            {
                logtable = " TB_H_R_LOG";
            }
            else if (sys_id == "I")
            {
                logtable = " TB_I_R_LOG";
            }
            else if (sys_id == "S")
            {
                logtable = " TB_S_R_LOG";
            }

            sb3.AppendLine(" from " + logtable + " L");
            sb3.AppendLine(" left join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY");
            sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
            sb3.AppendLine(" where 1=1 ");

            //檔案
            if (table_name != "")
            {
                sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
                ht3.Add("@TABLE_NAME", table_name);
            }
            //系統功能
            sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
            ht3.Add("@FUNC_ID", sys_fun);
            //異動類別
            if (category_item != "")
            {
                sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
                ht3.Add("@CATEGORY", category_item);
            }
            //修改者
            if (updated_by != "")
            {
                sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
                ht3.Add("@updated_by", updated_by);
            }
            //修改日期
            string updated_dt_s = updated_dt_date_s + " " + updated_dt_hour_s + ":" + updated_dt_min_s;
            string updated_dt_e = updated_dt_date_e + " " + updated_dt_hour_e + ":" + updated_dt_min_e;
            sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");
            ht3.Add("@updated_dt_s", updated_dt_s);
            ht3.Add("@updated_dt_e", updated_dt_e);

            //關鍵字
            if (edit_infor != "")
            {
                sb3.AppendLine(" and ( L.PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
                sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
                ht3.Add("@EDIT_INFOR", edit_infor);
            }
            //20150606 修改super邏輯  改以session中的is_super來判定
            //if (isSuper != "Y")
            if (SessionHandle.Current.is_super != "Y")
            {
                //登入者只能看到自己修改的資料
                sb3.AppendLine(" and L.UPDATED_BY = @updated_byself");
                ht3.Add("@updated_byself", SessionHandle.Current.emp_id);

                if (departments != "")
                {
                    sb3.AppendLine(" union");

                    //第二個
                    sb3.AppendLine("   select  L.TB_NAME as TB_NAME, L.TB_NAME+'('+L.TB_DESC+')' AS TB ");
                    sb3.AppendLine("          ,L.FUNC_NAME as FUNC_NAME, L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC ");
                    sb3.AppendLine("          ,L.CATEGORY as CATEGORY, L.CATEGORY +'-'+ D.SUB_DESC as CATEGORY_DESC");
                    sb3.AppendLine("          ,L.PK_COLUMN as PK_COLUMN, E.EMP_NAME as UPDATED_BY, L.UPDATED_DT as UPDATED_DT, L.EDIT_INFOR as EDIT_INFOR ");
                    sb3.AppendLine(" from " + logtable + " L");
                    sb3.AppendLine(" left join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY");
                    sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
                    sb3.AppendLine(" where 1=1 ");

                    //檔案
                    if (table_name != "")
                    {
                        sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
                    }
                    //系統功能
                    sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
                    //異動類別
                    if (category_item != "")
                    {
                        sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
                    }
                    //修改者
                    if (updated_by != "")
                    {
                        sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
                    }
                    //修改日期
                    sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");

                    //關鍵字
                    if (edit_infor != "")
                    {
                        sb3.AppendLine(" and ( L.PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
                        sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
                    }
                    //ACES設定的可看部門權限
                    string[] arrDepartments = departments.Replace(" ", "").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string strInParams = string.Empty;
                    for (int i = 0; i < arrDepartments.Length; i++)
                    {
                        strInParams += "@department" + i.ToString() + ",";
                        ht3.Add("@department" + i.ToString(), arrDepartments[i].Trim());
                    }
                    sb3.AppendLine(" and E.DEPT_NO in (" + strInParams.Trim(',') + ")");
                }
                sb3.AppendLine(" union");

                //第三個
                sb3.AppendLine("   select  L.TB_NAME as TB_NAME, L.TB_NAME+'('+L.TB_DESC+')' AS TB ");
                sb3.AppendLine("          ,L.FUNC_NAME as FUNC_NAME, L.FUNC_NAME+'('+L.FUNC_ID+')' AS FUNC ");
                sb3.AppendLine("          ,L.CATEGORY as CATEGORY, L.CATEGORY +'-'+ D.SUB_DESC as CATEGORY_DESC");
                sb3.AppendLine("          ,L.PK_COLUMN as PK_COLUMN, E.EMP_NAME as UPDATED_BY, L.UPDATED_DT as UPDATED_DT, L.EDIT_INFOR as EDIT_INFOR ");
                sb3.AppendLine(" from " + logtable + " L");
                sb3.AppendLine(" left join TB_H_M_EMP E on E.EMP_ID = L.UPDATED_BY");
                sb3.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = '99' and D.MAIN_CD = 'CATEGORY_ITEM' and D.SUB_CD = L.CATEGORY ");
                sb3.AppendLine(" where 1=1 ");

                //檔案
                if (table_name != "")
                {
                    sb3.AppendLine(" and L.TB_NAME like '%'+@TABLE_NAME+'%' ");
                }
                //系統功能
                sb3.AppendLine(" and L.FUNC_ID like '%'+@FUNC_ID+'%' ");
                //異動類別
                if (category_item != "")
                {
                    sb3.AppendLine(" and L.CATEGORY = @CATEGORY ");
                }
                //修改者
                if (updated_by != "")
                {
                    sb3.AppendLine(" and L.UPDATED_BY like '%'+@updated_by+'%' ");
                }
                //修改日期
                sb3.AppendLine(" and (L.UPDATED_DT >= @updated_dt_s and L.UPDATED_DT <= @updated_dt_e) ");

                //關鍵字
                if (edit_infor != "")
                {
                    sb3.AppendLine(" and (L. PK_COLUMN like '%'+@EDIT_INFOR+'%' ");
                    sb3.AppendLine(" or L.EDIT_INFOR like '%'+@EDIT_INFOR+'%' ) ");
                }
                //select 登入者所有管理的部門
                sb3.AppendLine(" and E.DEPT_NO in (select MNG_DEPT_NO from TB_H_R_HEAD_DEPT where EMP_ID=@EMP_ID) ");
                ht3.Add("@logtable", logtable);
                ht3.Add("@EMP_ID", SessionHandle.Current.emp_id);
                
            }
            sb3.AppendLine(" )a");

            DataTable dt = dbConn.Query(sb3, ht3, true);
            int t = 0;
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
    //public DataTable getData()
    //{
    //    try
    //    {
    //        SqlCommand comm = new SqlCommand();
    //        sb.AppendLine(" Select * From TB_9_M_COMM_H";
    //        sb.AppendLine(+= " where 1=1";

    //        if (SYS_CD != "")
    //        {
    //            sb.AppendLine(+= " and SYS_CD = @SYS_CD ";
    //            comm.Parameters.AddWithValue("SYS_CD", SYS_CD);
    //        }

    //        return dbs.getDataTable(comm);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    #endregion


}