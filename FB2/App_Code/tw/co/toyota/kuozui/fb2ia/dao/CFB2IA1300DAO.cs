using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2IA1300DAO 的摘要描述
/// </summary>
public class CFB2IA1300DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string SALARY_SYM { get; set; }
    public string SALARY_EYM { get; set; }
    public string COMPANY_CD { get; set; }
    public string EFFECT_DT { get; set; }
    public string AVG_SALARY { get; set; }
    public string A_OLD_INSAMT { get; set; }
    public string A_NEW_INSAMT { get; set; }
    public string B_OLD_INSAMT { get; set; }
    public string B_NEW_INSAMT { get; set; }
    public string C_OLD_INSAMT { get; set; }
    public string C_NEW_INSAMT { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    public string EFFECT_DT_S { get; set; }//按下薪調確定時，此為查詢條件的生效日期

    public string LICENSE_ID { get; set; }
    public string is_EFFECTED { get; set; }
    public string HOLD_YEAR { get; set; }
    
    public CFB2IA1300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //星期一join TB_I_M_3IN1_TXN 拿HOLD_YEAR ，在薪調確定時帶入TB_I_M_3IN1_TXN
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,
                            string company_cd, string salary_sym, string salary_eym,
                            string emp_id, string effect_dt, string license_id, string is_effected)
    {
        try
        {
            if (sortExpression.Contains("COMPANY_CD"))
                sortExpression = sortExpression.Replace("COMPANY_CD", "a.COMPANY_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from ");
            sb.Append(" (select row_number() over( order by a.emp_id) as RowNumber,d.COMPANY_SNAME,b.EMP_NAME,e.SUB_DESC NATION_NAME,f.SUB_DESC as EMP_CD_NAME,b.LICENSE_ID,a.*	");
            sb.Append(" from TB_I_M_LEVEL_CHG a	");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID= b.EMP_ID ");
            sb.Append(" left join TB_H_M_COMPANY d on a.COMPANY_CD= d.COMPANY_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='HB' and e.MAIN_CD='NATION_CD' and b.NATION_CD=e.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D f on f.SYS_CD='HB' and f.MAIN_CD='EMP_CD' and b.EMP_CD=f.SUB_CD	");
            sb.Append(" where a.company_cd='" + company_cd + "' ");

            //薪調月份
            if (salary_sym != "")
            {
                sb.Append(" and a.SALARY_SYM = @SALARY_SYM ");
                ht.Add("@SALARY_SYM", salary_sym.Replace("/", ""));
            }

            if (salary_eym != "")
            {
                sb.Append(" and a.SALARY_EYM = @SALARY_EYM ");
                ht.Add("@SALARY_EYM", salary_eym.Replace("/", ""));
            }

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            //if (nation_cd != "")
            //{
            //    sb.Append(" and b.NATION_CD = @NATION_CD ");
            //    ht.Add("@NATION_CD", nation_cd);
            //}

            if (effect_dt != "")
            {
                sb.Append(" and a.EFFECT_DT >= @EFFECT_DT ");
                ht.Add("@EFFECT_DT", effect_dt);
            }

            if (license_id != "")
            {
                sb.Append(" and b.LICENSE_ID like @LICENSE_ID ");
                ht.Add("@LICENSE_ID", license_id + '%');
            }
            //生效否 20150918 Terry add
            if (is_effected == "1")
            {
                sb.Append(" and isnull(a.EFFECT_DT,'') <> '' ");
            }
            else if (is_effected == "0")
            {
                sb.Append(" and isnull(a.EFFECT_DT,'') = '' ");
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

    public int getCount(int startRowIndex, int maximumRows,
                        string company_cd, string salary_sym, string salary_eym,
                        string emp_id, string effect_dt, string license_id, string is_effected)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(a.EMP_ID) total_record ");
            sb.Append(" from TB_I_M_LEVEL_CHG a	");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID= b.EMP_ID ");
            sb.Append(" left join TB_H_M_COMPANY d on a.COMPANY_CD= d.COMPANY_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='HB' and e.MAIN_CD='NATION_CD' and b.NATION_CD=e.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D f on f.SYS_CD='HB' and f.MAIN_CD='EMP_CD' and b.EMP_CD=f.SUB_CD	");
            sb.Append(" where a.company_cd='" + company_cd + "' ");

            //薪調月份
            if (salary_sym != "")
            {
                sb.Append(" and a.SALARY_SYM = @SALARY_SYM ");
                ht.Add("@SALARY_SYM", salary_sym.Replace("/", ""));
            }

            if (salary_eym != "")
            {
                sb.Append(" and a.SALARY_EYM = @SALARY_EYM ");
                ht.Add("@SALARY_EYM", salary_eym.Replace("/", ""));
            }

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            //if (nation_cd != "")
            //{
            //    sb.Append(" and b.NATION_CD = @NATION_CD ");
            //    ht.Add("@NATION_CD", nation_cd);
            //}

            if (effect_dt != "")
            {
                sb.Append(" and a.EFFECT_DT >= @EFFECT_DT ");
                ht.Add("@EFFECT_DT", effect_dt);
            }

            if (license_id != "")
            {
                sb.Append(" and b.LICENSE_ID like @LICENSE_ID ");
                ht.Add("@LICENSE_ID", license_id + '%');
            }

            //生效否 20150918 Terry add
            if (is_effected == "1")
            {
                sb.Append(" and isnull(a.EFFECT_DT,'') <> '' ");
            }
            else if (is_effected == "0")
            {
                sb.Append(" and isnull(a.EFFECT_DT,'') = '' ");
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


    public DataTable selectData()
    {
        try
        {            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select isnull(g.HOLD_YEAR,0)HOLD_YEAR,d.COMPANY_SNAME,b.EMP_NAME,e.SUB_DESC NATION_NAME,f.SUB_DESC as EMP_CD_NAME,b.LICENSE_ID,a.*	");
            sb.Append(" from TB_I_M_LEVEL_CHG a	");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID= b.EMP_ID ");
            sb.Append(" left join TB_H_M_COMPANY d on a.COMPANY_CD= d.COMPANY_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='HB' and e.MAIN_CD='NATION_CD' and b.NATION_CD=e.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D f on f.SYS_CD='HB' and f.MAIN_CD='EMP_CD' and b.EMP_CD=f.SUB_CD	");
            sb.Append(" left join  ( select max(EFFECT_SDT)EFFECT_SDT,EMP_ID,HOLD_YEAR from TB_I_M_3IN1_TXN where HOLD_YEAR <> 0  group by EMP_ID,HOLD_YEAR) g on a.EMP_ID = g.EMP_ID	");
            sb.Append(" where a.company_cd= @company_cd ");

            //薪調月份
            if (SALARY_SYM != "")
            {
                sb.Append(" and a.SALARY_SYM = @SALARY_SYM ");
                ht.Add("@SALARY_SYM", SALARY_SYM.Replace("/", ""));
            }

            if (SALARY_EYM != "")
            {
                sb.Append(" and a.SALARY_EYM = @SALARY_EYM ");
                ht.Add("@SALARY_EYM", SALARY_EYM.Replace("/", ""));
            }

            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }

            if (EFFECT_DT_S != "")
            {
                sb.Append(" and a.EFFECT_DT >= @EFFECT_DT ");
                ht.Add("@EFFECT_DT", EFFECT_DT_S);
            }

            if (LICENSE_ID != "")
            {
                sb.Append(" and b.LICENSE_ID like @LICENSE_ID ");
                ht.Add("@LICENSE_ID", LICENSE_ID + '%');
            }

            //生效否 20150918 Terry add
            if (is_EFFECTED == "1")
            {
                sb.Append(" and isnull(a.EFFECT_DT,'') <> '' ");
            }
            else if (is_EFFECTED == "0")
            {
                sb.Append(" and isnull(a.EFFECT_DT,'') = '' ");
            }

            ht.Add("@company_cd", COMPANY_CD);
           

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable checkLeaveData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select b.EMP_ID	");
            sb.Append(" from TB_I_M_LEVEL_CHG a	");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID= b.EMP_ID ");
            sb.Append(" left join TB_H_M_COMPANY d on a.COMPANY_CD= d.COMPANY_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='HB' and e.MAIN_CD='NATION_CD' and b.NATION_CD=e.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D f on f.SYS_CD='HB' and f.MAIN_CD='EMP_CD' and b.EMP_CD=f.SUB_CD	");
            sb.Append(" left join  ( select max(EFFECT_SDT)EFFECT_SDT,EMP_ID,HOLD_YEAR from TB_I_M_3IN1_TXN where HOLD_YEAR <> 0  group by EMP_ID,HOLD_YEAR) g on a.EMP_ID = g.EMP_ID	");
            sb.Append(" where a.company_cd= @company_cd and  isnull(b.leave_dt,'') <> '' ");

            //薪調月份
            if (SALARY_SYM != "")
            {
                sb.Append(" and a.SALARY_SYM = @SALARY_SYM ");
                ht.Add("@SALARY_SYM", SALARY_SYM.Replace("/", ""));
            }

            if (SALARY_EYM != "")
            {
                sb.Append(" and a.SALARY_EYM = @SALARY_EYM ");
                ht.Add("@SALARY_EYM", SALARY_EYM.Replace("/", ""));
            }

            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }

            if (EFFECT_DT_S != "")
            {
                sb.Append(" and a.EFFECT_DT >= @EFFECT_DT ");
                ht.Add("@EFFECT_DT", EFFECT_DT_S);
            }

            if (LICENSE_ID != "")
            {
                sb.Append(" and b.LICENSE_ID like @LICENSE_ID ");
                ht.Add("@LICENSE_ID", LICENSE_ID + '%');
            }

            //生效否 20150918 Terry add
            if (is_EFFECTED == "1")
            {
                sb.Append(" and isnull(a.EFFECT_DT,'') <> '' ");
            }
            else if (is_EFFECTED == "0")
            {
                sb.Append(" and isnull(a.EFFECT_DT,'') = '' ");
            }

            ht.Add("@company_cd", COMPANY_CD);


            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }


    //新增TB_I_M_LEVEL_CHG 保險薪調記錄檔
    public bool Insert_TB_I_M_LEVEL_CHG()
    {
        try
        {
            //BeginTransaction();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Insert Into TB_I_M_LEVEL_CHG ");
            sb.Append(" Values(@EMP_ID,@SALARY_SYM,@SALARY_EYM,@COMPANY_CD,@EFFECT_DT,@AVG_SALARY ");
            sb.Append(" ,@A_OLD_INSAMT,@A_NEW_INSAMT,@B_OLD_INSAMT,@B_NEW_INSAMT,@C_OLD_INSAMT,@C_NEW_INSAMT ");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_SYM", SALARY_SYM);
            ht.Add("@SALARY_EYM", SALARY_EYM);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@EFFECT_DT", DBNull.Value);
            ht.Add("@AVG_SALARY", AVG_SALARY);
            ht.Add("@A_OLD_INSAMT", A_OLD_INSAMT);
            ht.Add("@A_NEW_INSAMT", A_NEW_INSAMT);
            ht.Add("@B_OLD_INSAMT", B_OLD_INSAMT);
            ht.Add("@B_NEW_INSAMT", B_NEW_INSAMT);
            ht.Add("@C_OLD_INSAMT", C_OLD_INSAMT);
            ht.Add("@C_NEW_INSAMT", C_NEW_INSAMT);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
            // Commit();

            return true;
        }
        catch (Exception)
        {
            //RollBack();
            throw;
        }
    }

    //更新TB_I_M_LEVEL_CHG 保險薪調記錄檔 生效日期
    public bool Update_TB_I_M_LEVEL_CHG_EFFECT_DT()
    {
        try
        {
            //BeginTransaction();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_I_M_LEVEL_CHG");
            sb.Append(" set EFFECT_DT=@EFFECT_DT,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EMP_ID=@EMP_ID and COMPANY_CD=@COMPANY_CD and SALARY_SYM=@SALARY_SYM");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_SYM", SALARY_SYM);
            ht.Add("@COMPANY_CD", COMPANY_CD);

            ht.Add("@EFFECT_DT", EFFECT_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
            // Commit();

            return true;
        }
        catch (Exception)
        {
            //RollBack();
            throw;
        }
    }

    //public bool Update_TB_I_M_LEVEL_CHG_EFFECT_DT()
    //{
    //    try
    //    {            
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append(" update TB_I_M_LEVEL_CHG");
    //        sb.Append(" set EFFECT_DT=@EFFECT_DT,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
    //        sb.Append(" where COMPANY_CD=@COMPANY_CD and SALARY_SYM=@SALARY_SYM and isnull(EFFECT_DT,'') = '' ");
    //        if (EMP_ID != "")
    //        {
    //            sb.Append(" and EMP_ID = @EMP_ID ");
    //        }
    //        if (LICENSE_ID != "")
    //        {
    //            sb.Append(" and LICENSE_ID = @LICENSE_ID ");
    //        }

    //        ht.Add("@EMP_ID", EMP_ID);
    //        ht.Add("@SALARY_SYM", SALARY_SYM);
    //        ht.Add("@COMPANY_CD", COMPANY_CD);
    //        ht.Add("@LICENSE_ID", LICENSE_ID);

    //        ht.Add("@EFFECT_DT", EFFECT_DT);
    //        ht.Add("@UPDATED_BY", UPDATED_BY);
    //        ht.Add("@FUNC_ID", FUNC_ID);
    //        dbConn.ExecuteT(sb, ht, true);
           

    //        return true;
    //    }
    //    catch (Exception)
    //    {            
    //        throw;
    //    }
    //}
    //更新TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 生效日期迄
    public bool Update_TB_I_M_3IN1_TXN_EFFECT_EDT(string ins_type)
    {
        try
        {
            //BeginTransaction();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_I_M_3IN1_TXN ");
            sb.Append(" set EFFECT_EDT=@EFFECT_DT,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID ");
            sb.Append(" where INS_TYPE=@INS_TYPE and EMP_ID=@EMP_ID and IDENTITY_KIND ='1' and LICENSE_ID=@LICENSE_ID ");
            sb.Append(" and EFFECT_EDT='9999/12/31' and COMPANY_CD=@COMPANY_CD ");

            ht.Add("@INS_TYPE", ins_type);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@COMPANY_CD", COMPANY_CD);

            ht.Add("@EFFECT_DT", Convert.ToDateTime(EFFECT_DT).AddDays(-1).ToShortDateString());
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
            // Commit();

            return true;
        }
        catch (Exception)
        {
            //RollBack();
            throw;
        }
    }

    //新增[TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 ] 
    public bool Insert3IN1_TXN(string ins_type)
    {
        try
        {
            //BeginTransaction();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_M_3IN1_TXN ( ");
            sb.Append(" INS_TYPE,EMP_ID,IDENTITY_KIND,LICENSE_ID,EFFECT_SDT,EFFECT_EDT,");
            sb.Append(" SALARY_AMT,INS_AMT,COMPANY_CD,CHG_APP_TYPE,CHG_TYPE_IN,CHG_TYPE_OUT,");
            sb.Append(" CHG_REASON_CD,REMARK,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,HOLD_YEAR)");
            sb.Append(" values ( ");
            sb.Append(" @INS_TYPE,@EMP_ID,'1',@LICENSE_ID,@EFFECT_SDT,'9999/12/31',");
            sb.Append(" @SALARY_AMT,@INS_AMT,@COMPANY_CD,'3','','',");
            sb.Append(" '','',@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID,@HOLD_YEAR)");

            ht.Add("@INS_TYPE", ins_type);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EFFECT_SDT", EFFECT_DT);
            ht.Add("@SALARY_AMT", AVG_SALARY);

            if (ins_type == "A")
                ht.Add("@INS_AMT", A_NEW_INSAMT);
            else if (ins_type == "B")
                ht.Add("@INS_AMT", B_NEW_INSAMT);
            else if (ins_type == "C")
                ht.Add("@INS_AMT", C_NEW_INSAMT);

            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@HOLD_YEAR", HOLD_YEAR);
            dbConn.ExecuteT(sb, ht, true);
            // Commit();

            return true;
        }
        catch (Exception)
        {
            //RollBack();
            throw;
        }
    }

    //刪除TB_I_M_LEVEL_CHG 保險薪調記錄檔
    public bool Delete_TB_I_M_LEVEL_CHG_ALL()
    {
        try
        {
            //BeginTransaction();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_I_M_LEVEL_CHG");
            sb.Append(" where SALARY_SYM=@SALARY_SYM");

            ht.Add("@SALARY_SYM", SALARY_SYM);

            dbConn.ExecuteT(sb, ht, true);
            // Commit();

            return true;
        }
        catch (Exception)
        {
            //RollBack();
            throw;
        }
    }

    public bool Delete_TB_I_M_LEVEL_CHG()
    {
        try
        {
            //BeginTransaction();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_I_M_LEVEL_CHG");
            sb.Append(" where EMP_ID=@EMP_ID and SALARY_SYM=@SALARY_SYM");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_SYM", SALARY_SYM);

            dbConn.ExecuteT(sb, ht, true);
            // Commit();

            return true;
        }
        catch (Exception)
        {
            //RollBack();
            throw;
        }
    }

    //新增保險三合一伸報資料檔 保險薪調記錄檔
    public bool Insert_TB_I_R_3IN1_REPORTDATA(string NATION_CD, string EMP_NAME, string BIRTH_DT, string IS_PJ50)
    {
        try
        {
            //BeginTransaction();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Insert Into TB_I_R_3IN1_REPORTDATA ");
            sb.Append(" Values('3',@COMPANY_CD,'2','1',@SYS_DESC,@NATION_CD ");
            sb.Append(" ,@LICENSE_ID,@EMP_NAME,@BIRTH_DT,@INS_AC_AVGSALARY,@B_OLD_INSAMT,@B_NEW_INSAMT ");
            sb.Append(" ,@SPTYP,null,null,null,null,'',null,null,null,null,null,0,null,null,'C',null ");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@SYS_DESC", COMPANY_CD + SALARY_SYM + EMP_ID);
            ht.Add("@NATION_CD", NATION_CD);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@BIRTH_DT", Convert.ToDateTime(BIRTH_DT));
            ht.Add("@INS_AC_AVGSALARY", AVG_SALARY);
            ht.Add("@B_OLD_INSAMT", B_OLD_INSAMT);
            ht.Add("@B_NEW_INSAMT", B_NEW_INSAMT);
            ht.Add("@SPTYP", (IS_PJ50=="")?"":"T");
            ht.Add("@C_NEW_INSAMT", C_NEW_INSAMT);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
            // Commit();

            return true;
        }
        catch (Exception ex)
        {
            //RollBack();
            throw ex;
        }
    }
    
    //刪除TB_I_R_3IN1_REPORTDATA 保險三合一伸報資料檔
    public bool Delete_TB_I_R_3IN1_REPORTDATA()
    {
        try
        {
            //BeginTransaction();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_I_R_3IN1_REPORTDATA");
            sb.Append(" where CHG_APP_TYPE='3' and SYS_DESC = @SYS_DESC ");

            ht.Add("@SYS_DESC",COMPANY_CD + SALARY_SYM + EMP_ID);

            dbConn.ExecuteT(sb, ht, true);
            // Commit();

            return true;
        }
        catch (Exception)
        {
            //RollBack();
            throw;
        }
    }

    public bool Delete_TB_I_R_3IN1_REPORTDATA_ALL()
    {
        try
        {
            //BeginTransaction();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_I_R_3IN1_REPORTDATA");
            sb.Append(" where CHG_APP_TYPE='3' and SYS_DESC like '_'+@SYS_DESC+'%'");

            ht.Add("@SYS_DESC", SALARY_SYM);

            dbConn.ExecuteT(sb, ht, true);
            // Commit();

            return true;
        }
        catch (Exception)
        {
            //RollBack();
            throw;
        }
    }

    //取得最近一次薪資計算年月
    public string getLast_SALARY_YM()
    {
        try
        {
            string t = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select dbo.FN_S_SALARY_YM() ");

            //DataTable dt = dbConn.Query(sb, ht);
            DataTable dt = dbConn.Query(sb);
            if (dt.Rows.Count > 0)
            {
                t = dt.Rows[0][0].ToString();
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //查詢現有薪調資料筆數
    public int get_mon3avgsalry_count(string DEF_SYM, string DEF_EYM)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*) as total_record From TB_I_M_LEVEL_CHG a ");
            sb.Append(" where  a.EFFECT_DT <> ''   ");
            sb.Append("         AND ((a.SALARY_SYM >= @DEF_SYM and a.SALARY_SYM <=@DEF_EYM) ");
            sb.Append("             Or (a.SALARY_EYM >= @DEF_SYM and a.SALARY_EYM <=@DEF_EYM)) ");

            ht.Add("@DEF_SYM", DEF_SYM);
            ht.Add("@DEF_EYM", DEF_EYM);

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

    //產生薪調資料
    public DataTable get_mon3avgsalry_Data(string DEF_SYM, string DEF_EYM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" exec SP_I_EMP_NOTFULL3MON 'FB2IA1300',@DEF_SYMALL,@DEF_EYMALL,@USERID,@FUNC_ID ;");
            sb.Append(" with mon3avgsalry as ( ");
            sb.Append(" select s2.emp_id, ");
            sb.Append(" ROUND(sa.AMOUNT/3,0) as INS_AC_AVGSALARY ");
            sb.Append(" ,ROUND(sb.AMOUNT/3,0) as INS_B_AVGSALARY  ");
            sb.Append(" From ( select t3.emp_id from ( ");
            sb.Append("    select tt.COMPANY_CD,tt.EMP_ID,count(*) as MONTH_QTY from ( ");
            sb.Append("       select DISTINCT a.COMPANY_CD,a.EMP_ID,a.SALARY_YM as DATA_YM ");
            sb.Append("       from TB_S_M_EMP_RESULT a ");
            sb.Append("       where  a.SALARY_YM>=@DEF_SYM and a.SALARY_YM <=@DEF_EYM");//a.SALARY_TYPE ='A' and
            sb.Append("    ) tt group by tt.COMPANY_CD,tt.EMP_ID ");
            sb.Append("  ) t3 where t3.MONTH_QTY =3 ");
            sb.Append(" ) s2 ");
            sb.Append(" left join ( ");
            sb.Append(" 	Select s1.emp_id,Sum(s1.AMOUNT*s1.IS_PLUS) as AMOUNT ");
            sb.Append(" 	From TB_S_M_SALARY_PAY s1 Inner Join TB_S_M_SALARY_ITEM s3 ");
            sb.Append(" 		on s1.SALARY_ID=s3.SALARY_ID and s3.INS_A='Y' ");
            sb.Append(" 	where s1.DATA_YM>=@DEF_SYM and s1.DATA_YM <=@DEF_EYM and s1.SALARY_TYPE='A'");//and s1.IS_PLUS=1 
            sb.Append(" 	group by s1.emp_id ");
            sb.Append(" 	) sa ");
            sb.Append(" on sa.emp_id=s2.emp_id ");
            sb.Append(" left join ( ");
            sb.Append(" 	Select s1.emp_id,Sum(s1.AMOUNT*s1.IS_PLUS) as AMOUNT ");
            sb.Append(" 	From TB_S_M_SALARY_PAY s1 Inner Join TB_S_M_SALARY_ITEM s3 ");
            sb.Append(" 		on s1.SALARY_ID=s3.SALARY_ID and s3.INS_B='Y' ");
            sb.Append(" 	where s1.DATA_YM>=@DEF_SYM and s1.DATA_YM <=@DEF_EYM  and s1.SALARY_TYPE='A'");//and s1.IS_PLUS=1 
            sb.Append(" 	group by s1.emp_id ");
            sb.Append(" 	) sb ");
            sb.Append(" on sb.emp_id=s2.emp_id ");
            sb.Append(" ), TEMP1 as ( ");
            sb.Append(" SELECT a.emp_id,b.INS_AC_AVGSALARY,c.INS_AMT as INS_A_AMT_NEW,d.INS_AMT as INS_B_AMT_NEW ");
            sb.Append(" ,case when h.RC_TYPE='O' then isnull(h.INS_AMT,0) else e.INS_AMT end as INS_C_AMT_NEW ");
            sb.Append(" ,isnull(f.INS_AMT,0) as INS_A_AMT_OLD,isnull(g.INS_AMT,0) as INS_B_AMT_OLD,isnull(h.INS_AMT,0) as INS_C_AMT_OLD ");
            sb.Append(" ,a.COMPANY_CD,Case When j.TW = a.NATION_CD Then '' Else 'Y' End as NATION_CD,a.BIRTH_DT,a.LICENSE_ID,a.EMP_NAME ");
            sb.Append(" ,IsNull((Select Top 1 PJOB_CD From TB_I_M_CHG_TXN i Where a.emp_id = i.emp_id And PJOB_CD ='PJ50' order by OP_DT DESC ),'') as IS_PJ50 ");
            sb.Append(" FROM VW_H_EMP_DATA a ");
            sb.Append(" inner join mon3avgsalry b on a.emp_id=b.emp_id ");
            sb.Append(" left join TB_I_M_LEVEL c on c.INS_TYPE='A' and c.INS_LOW<=b.INS_AC_AVGSALARY and c.INS_TOP>=b.INS_AC_AVGSALARY ");
            sb.Append(" left join TB_I_M_LEVEL d on d.INS_TYPE='B' and d.INS_LOW<=b.INS_B_AVGSALARY and d.INS_TOP>=b.INS_B_AVGSALARY ");
            sb.Append(" left join TB_I_M_LEVEL e on e.INS_TYPE='C' and e.INS_LOW<=b.INS_AC_AVGSALARY and e.INS_TOP>=b.INS_AC_AVGSALARY ");
            sb.Append(" left join TB_I_M_3IN1_TXN f on a.emp_id= f.emp_id and f.INS_TYPE='A' and f.EFFECT_EDT='9999/12/31' ");
            sb.Append(" left join TB_I_M_3IN1_TXN g on a.emp_id= g.emp_id and g.INS_TYPE='B' and g.EFFECT_EDT='9999/12/31' and g.IDENTITY_KIND='1'	");
            sb.Append(" left join TB_I_M_3IN1_TXN h on a.emp_id= h.emp_id and h.INS_TYPE='C' and h.EFFECT_EDT='9999/12/31',(select CODE_VAL1 as TW From TB_9_M_PARAMETER where SYS_CD = 'IA' and MAIN_CD='TWN_CD') as j ");
            sb.Append(" where a.EMP_STATUS='01' and a.PJOB_CD not in ('PJ50','PJ60')");
            sb.Append(" and a.emp_id not in (select emp_id from TB_H_S_EMP_DATA where PROC_DESC ='FB2IA1300' and BASE_SDT = @DEF_SYMALL) ");
            sb.Append(" ) ");
            sb.Append(" Select * From TEMP1 ");
            ht.Add("@DEF_SYMALL", DEF_SYM);
            ht.Add("@DEF_EYMALL", Convert.ToDateTime(DEF_EYM).AddMonths(1).AddDays(-1).ToShortDateString());
            ht.Add("@DEF_SYM", DEF_SYM.Replace("/", "").Substring(0, 6));
            ht.Add("@DEF_EYM", DEF_EYM.Replace("/", "").Substring(0, 6));
            ht.Add("@USERID", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA1300");


            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //匯出三合一薪調申報資料 Excel
    public DataTable get_Excel_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT a.*,left(b.LABOR_ORG_ID,8) LAB8,RIGHT(RTRIM(b.LABOR_ORG_ID),1) LAB_CHK_CD,b.HEALTH_ORG_ID,b.HEALTH_BUSINESS_ID ");
            sb.Append(" FROM TB_I_R_3IN1_REPORTDATA a ");
            sb.Append(" left join TB_H_M_COMPANY b on b.company_cd=a.COMPANY_CD_NEW ");
            sb.Append(" where a.COMPANY_CD_NEW=@COMPANY_CD and left(SYS_DESC,7)=@SYS_DESC and CHG_APP_TYPE='3' and a.DATASOURCE='C'  ");
            
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@SYS_DESC", COMPANY_CD + SALARY_SYM);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //列印投保等級調降三級以上的人員,試算薪調月份薪資明細資料(PDF型式列印)
    public DataTable get_PDF_Data(string def_sym, string def_eym, string classqty, string company_cd, string orderby)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_I_IA1300PDF");
            ht.Add("@DEF_SYM", def_sym);
            ht.Add("@DEF_EYM", def_eym);
            ht.Add("@CLASSQTY", classqty);
            ht.Add("@COMPANY_CD", company_cd);
            ht.Add("@ORDERBY", orderby);
            

            return dbConn.QuerySP(sb, ht,true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable get_Company_Name(string COMPANY_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT COMPANY_NAME ");
            sb.Append(" FROM TB_H_M_COMPANY ");
            sb.Append(" where COMPANY_CD = @COMPANY_CD ");

            ht.Add("@COMPANY_CD", COMPANY_CD);            

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }


}