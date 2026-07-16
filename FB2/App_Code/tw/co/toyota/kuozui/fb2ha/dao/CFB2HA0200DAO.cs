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
/// CFB2HA020DAO 的摘要描述
/// </summary>
public class CFB2HA0200DAO : BaseDAO
{
    public string DEPT_NO { get; set; }
    public string START_DT { get; set; }


    public string END_DT { get; set; }
    public string DEPT_NAME { get; set; }
    public string DEPT_SNAME { get; set; }
    public string DEPT_ENAME { get; set; }
    public string HEAD_EMP_ID { get; set; }
    public string DEPT_LEVEL { get; set; }
    public string ORG_TYPE { get; set; }
    public string DEPT_WS_TYPE { get; set; }
    public string ACC_SALARY_CD { get; set; }
    public string ACC_CD { get; set; }
    public string ACC_DEPT_NO { get; set; }
    public string REMARK { get; set; }
    public string DEFAULT_PLANT { get; set; }
    public string CREATED_BY { get; set; }
    public DateTime CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public DateTime UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2HA0200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string dept_no, string dept_level,
                            string org_type, string acc_cd, string acc_dept_no, string start_dt_s,
                            string start_dt_e, string end_dt_s, string end_dt_e, string is_valid, string dept_name)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,*");
            sb.Append("  from ( select ");
            sb.Append(" a.DEPT_LEVEL ,Convert(varchar,a.DEPT_LEVEL) + '-' + DEPT_LEVEL_DESC DEPT_LEVEL_DESC,a.DEPT_NO,DEPT_NAME,EMP_NAME,CONVERT(char(19),START_DT, 120) START_DT_KEY,");
            sb.Append(" a.START_DT,a.END_DT,a.DEFAULT_PLANT+'-'+c3.SUB_DESC DEFAULT_PLANT,");
            sb.Append(" a.ORG_TYPE + '-' + c1.SUB_DESC ORG_TYPE_DESC,a.ORG_TYPE, ");
            sb.Append(" a.ACC_CD + '-' + c2.SUB_DESC ACC_DESC,a.ACC_CD, ");
            //sb.Append(" c3.SUB_DESC ACC_SALARY_DESC,a.ACC_SALARY_CD,c4.SUB_DESC DEPT_WS_DESC, ");
            sb.Append(" d.WS_CD, ");
            sb.Append(" e.ACC_DEPT_NO + ' ' + e.ACC_DEPT_NAME ACC_DEPT_DESC,e.ACC_DEPT_NO ACC_DEP_NO,a.REMARK ");
            sb.Append(" from TB_H_M_DEPT a ");
            sb.Append(" left join TB_H_M_DEPT_LEVEL b on a.DEPT_LEVEL = b.DEPT_LEVEL ");
            sb.Append(" left join TB_9_M_COMM_D c1 on c1.MAIN_CD = 'ORG_TYPE' and a.ORG_TYPE = c1.SUB_CD and c1.SYS_CD='HA' ");
            sb.Append(" left join TB_9_M_COMM_D c2 on c2.MAIN_CD = 'ACC_CD' and a.ACC_CD = c2.SUB_CD and c2.SYS_CD='HA' ");
            sb.Append(" left join TB_9_M_COMM_D c3 on c3.MAIN_CD = 'DEFAULT_PLANT' and a.DEFAULT_PLANT = c3.SUB_CD and c3.SYS_CD='HA' ");
            //sb.Append(" left join TB_9_M_COMM_D c4 on c4.MAIN_CD = 'DEPT_WS_TYPE' and d.WS_CD = c4.SUB_CD ");
            sb.Append(" left join TB_H_M_EMP d on a.HEAD_EMP_ID = d.EMP_ID ");
            sb.Append(" left join TB_H_M_DEPT_ACC e on a.ACC_DEPT_NO = e.ACC_DEPT_NO ");
            sb.Append(" where  1=1 ");
            if (dept_no != "")
            {
                sb.Append(" and a.DEPT_NO LIKE @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (dept_level != "-1")
            {
                sb.Append(" and a.DEPT_LEVEL = @DEPT_LEVEL ");
                ht.Add("@DEPT_LEVEL", dept_level);
            }
            if (org_type != "-1")
            {
                sb.Append(" and a.ORG_TYPE = @ORG_TYPE ");
                ht.Add("@ORG_TYPE", org_type);
            }
            //if (acc_salary_cd != "-1")
            //{
            //    sb.Append(" and a.ACC_SALARY_CD = @ACC_SALARY_CD ");
            //    ht.Add("@ACC_SALARY_CD", acc_salary_cd);
            //}
            if (acc_cd != "-1")
            {
                sb.Append(" and a.ACC_CD = @ACC_CD ");
                ht.Add("@ACC_CD", acc_cd);
            }
            if (acc_dept_no != "")
            {
                sb.Append(" and a.ACC_DEPT_NO = @ACC_DEPT_NO ");
                ht.Add("@ACC_DEPT_NO", acc_dept_no);
            }
            if (start_dt_s != "")
            {
                sb.Append(" and a.START_DT >= @START_DT_S ");
                ht.Add("@START_DT_S", start_dt_s);
            }
            if (start_dt_e != "")
            {
                sb.Append(" and a.START_DT <= @START_DT_E ");
                ht.Add("@START_DT_E", start_dt_e);
            }

            if (end_dt_s != "")
            {
                sb.Append(" and a.END_DT >= @END_DT_S ");
                ht.Add("@END_DT_S", end_dt_s);
            }
            if (end_dt_e != "")
            {
                sb.Append(" and a.END_DT <= @END_DT_E ");
                ht.Add("@END_DT_E", end_dt_e);
            }

            if (is_valid == "Y")
            {
                sb.Append(" and (a.START_DT <= @CURRENT_DT and a.END_DT >= @CURRENT_DT) ");
                ht.Add("@CURRENT_DT", DateTime.Now.Date.ToString("yyyy/MM/dd"));
            }
            if (is_valid == "N")
            {
                sb.Append(" and (a.START_DT >= @CURRENT_DT  or a.END_DT <= @CURRENT_DT ) ");
                ht.Add("@CURRENT_DT", DateTime.Now.Date.ToString("yyyy/MM/dd"));
            }
            if (dept_name != "")
            {
                sb.Append(" and a.DEPT_NAME like @DEPT_NAME ");
                ht.Add("@DEPT_NAME", '%' + dept_name + '%');
            }

            sb.Append(" )alltb ");
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
    public int getCount(int startRowIndex, int maximumRows, string dept_no, string dept_level,
                            string org_type, string acc_cd, string acc_dept_no, string start_dt_s,
                            string start_dt_e, string end_dt_s, string end_dt_e, string is_valid, string dept_name)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_DEPT a ");
            sb.Append(" left join TB_H_M_DEPT_LEVEL b on a.DEPT_LEVEL = b.DEPT_LEVEL ");
            sb.Append(" left join TB_9_M_COMM_D c1 on c1.MAIN_CD = 'ORG_TYPE' and a.ORG_TYPE = c1.SUB_CD and c1.SYS_CD='HA' ");
            sb.Append(" left join TB_9_M_COMM_D c2 on c2.MAIN_CD = 'ACC_CD' and a.ACC_CD = c2.SUB_CD and c2.SYS_CD='HA' ");
            sb.Append(" left join TB_9_M_COMM_D c3 on c3.MAIN_CD = 'DEFAULT_PLANT' and a.DEFAULT_PLANT = c3.SUB_CD and c3.SYS_CD='HA' ");
            //sb.Append(" left join TB_9_M_COMM_D c4 on c4.MAIN_CD = 'DEPT_WS_TYPE' and d.WS_CD = c4.SUB_CD ");
            sb.Append(" left join TB_H_M_EMP d on a.HEAD_EMP_ID = d.EMP_ID ");
            sb.Append(" left join TB_H_M_DEPT_ACC e on a.ACC_DEPT_NO = e.ACC_DEPT_NO ");
            sb.Append(" where  1=1 ");
            if (dept_no != "")
            {
                sb.Append(" and a.DEPT_NO LIKE @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (dept_level != "-1")
            {
                sb.Append(" and a.DEPT_LEVEL = @DEPT_LEVEL ");
                ht.Add("@DEPT_LEVEL", dept_level);
            }
            if (org_type != "-1")
            {
                sb.Append(" and a.ORG_TYPE = @ORG_TYPE ");
                ht.Add("@ORG_TYPE", org_type);
            }
            //if (acc_salary_cd != "-1")
            //{
            //    sb.Append(" and a.ACC_SALARY_CD = @ACC_SALARY_CD ");
            //    ht.Add("@ACC_SALARY_CD", acc_salary_cd);
            //}
            if (acc_cd != "-1")
            {
                sb.Append(" and a.ACC_CD = @ACC_CD ");
                ht.Add("@ACC_CD", acc_cd);
            }
            if (acc_dept_no != "")
            {
                sb.Append(" and a.ACC_DEPT_NO = @ACC_DEPT_NO ");
                ht.Add("@ACC_DEPT_NO", acc_dept_no);
            }
            if (start_dt_s != "")
            {
                sb.Append(" and a.START_DT >= @START_DT_S ");
                ht.Add("@START_DT_S", start_dt_s);
            }
            if (start_dt_e != "")
            {
                sb.Append(" and a.START_DT <= @START_DT_E ");
                ht.Add("@START_DT_E", start_dt_e);
            }

            if (end_dt_s != "")
            {
                sb.Append(" and a.END_DT >= @END_DT_S ");
                ht.Add("@END_DT_S", end_dt_s);
            }
            if (end_dt_e != "")
            {
                sb.Append(" and a.END_DT <= @END_DT_E ");
                ht.Add("@END_DT_E", end_dt_e);
            }

            if (is_valid == "Y")
            {
                sb.Append(" and (a.START_DT <= @CURRENT_DT and a.END_DT >= @CURRENT_DT) ");
                ht.Add("@CURRENT_DT", DateTime.Now.Date.ToString("yyyy/MM/dd"));
            }
            if (is_valid == "N")
            {
                sb.Append(" and (a.START_DT >= @CURRENT_DT  or a.END_DT <= @CURRENT_DT ) ");
                ht.Add("@CURRENT_DT", DateTime.Now.Date.ToString("yyyy/MM/dd"));
            }
            if (dept_name != "")
            {
                sb.Append(" and a.DEPT_NAME like @DEPT_NAME ");
                ht.Add("@DEPT_NAME", '%' + dept_name + '%');
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

    internal void deleteDeptNo(string dept_no, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_H_M_DEPT set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HA020' ");
            sb.Append(" where DEPT_NO = @DEPT_NO ");
            sb.Append(" and START_DT=@START_DT ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" Delete From TB_H_M_DEPT ");
            sb.Append(" where DEPT_NO = @DEPT_NO ");
            sb.Append(" and START_DT=@START_DT ");
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@START_DT", start_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getExistDeptOrg(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(0) deptcount From");
            sb.Append(" TB_H_M_DEPT_ORG");
            sb.Append(" where UP_DEPT_NO = @DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select ");
            sb.Append(" a.DEPT_LEVEL ,b.DEPT_LEVEL_DESC,a.DEPT_NO,a.DEPT_NAME,a.DEPT_SNAME,a.DEPT_ENAME,a.HEAD_EMP_ID,d.EMP_NAME HEAD_EMP_NAME, ");
            sb.Append(" a.DEPT_LEVEL,REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT,");
            sb.Append(" a.ORG_TYPE, ");
            sb.Append(" a.ACC_CD,a.DEFAULT_PLANT,  ");
            //sb.Append(" ACC_SALARY_CD, ");
            //sb.Append(" DEPT_WS_TYPE, ");
            sb.Append(" e.ACC_DEPT_NAME ,e.ACC_DEPT_NO,a.REMARK ");
            sb.Append(" from TB_H_M_DEPT a ");
            sb.Append(" left join TB_H_M_DEPT_LEVEL b on a.DEPT_LEVEL = b.DEPT_LEVEL  ");
            sb.Append(" left join TB_H_M_EMP d on a.HEAD_EMP_ID = d.EMP_ID  ");
            sb.Append(" left join TB_H_M_DEPT_ACC e on a.ACC_DEPT_NO = e.ACC_DEPT_NO ");
            sb.Append(" where a.DEPT_NO = @DEPT_NO ");
            sb.Append(" and a.START_DT = @START_DT ");
            //sb.Append(" and c4.MAIN_CD = 'DEPT_WS_TYPE' ");

            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@START_DT", START_DT);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }



    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select DEPT_NO,convert(char(10),min(START_DT),120) START_DT ");
            sb.Append(" from TB_H_M_DEPT");
            sb.Append(" where DEPT_NO = @DEPT_NO and START_DT > @START_DT");
            sb.Append(" group by DEPT_NO");
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@START_DT", START_DT);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getExistSubData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select DEPT_NO ");
            sb.Append(" from TB_H_M_DEPT_ORG");
            sb.Append(" where UP_DEPT_NO = @DEPT_NO and END_DT > @END_DT");
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@END_DT", END_DT);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void updateDept()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_H_M_DEPT ");
            sb.Append(" set END_DT = @END_DT,DEPT_NAME = @DEPT_NAME,DEPT_SNAME = @DEPT_SNAME, DEPT_ENAME =@DEPT_ENAME,HEAD_EMP_ID =@HEAD_EMP_ID,");
            sb.Append(" DEPT_LEVEL = @DEPT_LEVEL,ORG_TYPE = @ORG_TYPE, ACC_CD =@ACC_CD,DEFAULT_PLANT = @DEFAULT_PLANT,");
            sb.Append(" ACC_DEPT_NO = @ACC_DEPT_NO,REMARK = @REMARK,UPDATED_BY = @UPDATED_BY, UPDATED_DT =GETDATE(),FUNC_ID =@FUNC_ID");
            sb.Append(" where DEPT_NO = @DEPT_NO and START_DT = @START_DT");
            ht.Add("@END_DT", END_DT);
            ht.Add("@DEPT_NAME", DEPT_NAME);
            ht.Add("@DEPT_SNAME", DEPT_SNAME);
            ht.Add("@DEPT_ENAME", DEPT_ENAME);
            ht.Add("@HEAD_EMP_ID", HEAD_EMP_ID);
            ht.Add("@DEPT_LEVEL", DEPT_LEVEL);
            ht.Add("@ORG_TYPE", ORG_TYPE);
            //ht.Add("@DEPT_WS_TYPE", DEPT_WS_TYPE);

            ht.Add("@ACC_CD", ACC_CD.ToUpper());
            ht.Add("@ACC_DEPT_NO", ACC_DEPT_NO.ToUpper());
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@DEPT_NO", DEPT_NO.ToUpper());
            ht.Add("@START_DT", START_DT);
            ht.Add("@DEFAULT_PLANT", DEFAULT_PLANT);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getSalaryData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select dbo.FN_S_SALARY_YM() SALARY_YM");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addDept()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_H_M_DEPT ");
            sb.Append(" (DEPT_NO,START_DT,END_DT,DEPT_NAME,DEPT_SNAME,DEPT_ENAME,HEAD_EMP_ID,DEPT_LEVEL,ORG_TYPE,");
            sb.Append(" ACC_CD,ACC_DEPT_NO,REMARK,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,DEFAULT_PLANT)");
            sb.Append(" values (@DEPT_NO,@START_DT,@END_DT,@DEPT_NAME,@DEPT_SNAME,@DEPT_ENAME,@HEAD_EMP_ID,@DEPT_LEVEL,@ORG_TYPE,");
            sb.Append(" @ACC_CD,@ACC_DEPT_NO,@REMARK,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID,@DEFAULT_PLANT)");
            ht.Add("@END_DT", END_DT);
            ht.Add("@DEPT_NAME", DEPT_NAME);
            ht.Add("@DEPT_SNAME", DEPT_SNAME);
            ht.Add("@DEPT_ENAME", DEPT_ENAME);
            ht.Add("@HEAD_EMP_ID", HEAD_EMP_ID);
            ht.Add("@DEPT_LEVEL", DEPT_LEVEL);
            ht.Add("@ORG_TYPE", ORG_TYPE);
            //ht.Add("@DEPT_WS_TYPE", DEPT_WS_TYPE);
            //ht.Add("@ACC_SALARY_CD", ACC_SALARY_CD);
            ht.Add("@ACC_CD", ACC_CD.ToUpper());
            ht.Add("@ACC_DEPT_NO", ACC_DEPT_NO.ToUpper());
            ht.Add("@REMARK", REMARK);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@DEPT_NO", DEPT_NO.ToUpper());
            ht.Add("@START_DT", START_DT);
            ht.Add("@DEFAULT_PLANT", DEFAULT_PLANT);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //判斷是否有同PK值
    internal DataTable getDupData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select DEPT_NO ");
            sb.Append(" from TB_H_M_DEPT");
            sb.Append(" where DEPT_NO = @DEPT_NO and START_DT = @START_DT");
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@START_DT", START_DT);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getMaxEndDTByType()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select  MAX(END_DT) maxEndDT ");
            sb.Append(" from TB_H_M_DEPT");
            sb.Append(" where DEPT_NO = @DEPT_NO");
            ht.Add("@DEPT_NO", DEPT_NO);


            /*
            sb.Append(" select DEPT_NO ");
            sb.Append(" from TB_H_M_DEPT");
            sb.Append(" where DEPT_NO = @DEPT_NO");
            if (START_DT != "")
            {
                sb.Append(" and START_DT < @START_DT ");
                ht.Add("@START_DT", START_DT);
            }
            if (END_DT != "")
            {
                sb.Append(" and END_DT > @END_DT ");
                ht.Add("@END_DT", END_DT);
            }
            ht.Add("@DEPT_NO", DEPT_NO);
            */
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEmpName(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NAME ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getACC_DEPT_Name(string acc_dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select ACC_DEPT_NO,ACC_DEPT_NAME ");
            sb.Append(" from TB_H_M_DEPT_ACC ");
            sb.Append(" where ACC_DEPT_NO=@ACC_DEPT_NO ");
            ht.Add("@ACC_DEPT_NO", acc_dept_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }




    #region 部門上傳檢查

    //判斷工號是否為在職
    public DataTable getEmpCount(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID = @EMP_ID ");
            sb.Append(" and EMP_STATUS = @EMP_STATUS ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@EMP_STATUS", "01");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //判斷部門層級存在於部門層級檔
    public DataTable getDeptLevelCount(string dept_level)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount from TB_H_M_DEPT_LEVEL ");
            sb.Append(" where IS_VALID = @IS_VALID ");
            sb.Append(" and DEPT_LEVEL = @DEPT_LEVEL ");
            ht.Add("@IS_VALID", "Y");
            ht.Add("@DEPT_LEVEL", dept_level);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //薪資部門區分存在於薪資部門區分設定檔
    public DataTable getAccDeptNOCount(string acc_dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount from TB_H_M_DEPT_ACC ");
            sb.Append(" where IS_VALID = @IS_VALID ");
            sb.Append(" and ACC_DEPT_NO = @ACC_DEPT_NO ");
            ht.Add("@IS_VALID", "Y");
            ht.Add("@ACC_DEPT_NO", acc_dept_no);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //判斷存在於共用代碼檔
    public DataTable getCommCodeCount(string sys_cd, string main_cd, string sub_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount from TB_9_M_COMM_D ");
            sb.Append(" where 1=1 ");
            sb.Append(" and MAIN_CD = @MAIN_CD ");
            sb.Append(" and SYS_CD = @SYS_CD ");
            sb.Append(" and SUB_CD = @SUB_CD ");
            sb.Append(" and IS_VALID = @IS_VALID ");
            ht.Add("@MAIN_CD", main_cd);
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@SUB_CD", sub_cd);
            ht.Add("@IS_VALID", "Y");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //判斷是否有同PK值
    internal DataTable getPKDupData(string dept_no, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select  count(0) resultCount ");
            sb.Append(" from TB_H_M_DEPT");
            sb.Append(" where DEPT_NO = @DEPT_NO and START_DT = @START_DT");
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@START_DT", start_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //判斷生效期間是否有重疊(顯示錯誤訊息)
    internal DataTable getDupTimeData(string dept_no, string start_dt, string end_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select  count(0) resultCount ");
            sb.Append(" from TB_H_M_DEPT");
            sb.Append(" where DEPT_NO = @DEPT_NO ");
            sb.Append("  and @START_DT < START_DT ");
            sb.Append("  and @END_DT >=START_DT ");
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@START_DT", start_dt);
            ht.Add("@END_DT", end_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //判斷生效期間是否有重疊(需進行update)
    internal DataTable getDupTimeData_update(string dept_no, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select  count(0) resultCount ");
            sb.Append(" from TB_H_M_DEPT");
            sb.Append(" where DEPT_NO = @DEPT_NO ");
            sb.Append("  and @START_DT between START_DT and END_DT  ");
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@START_DT", start_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //重覆的結束日期為前一天
    internal void updateDeptBefore()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_H_M_DEPT ");
            sb.Append(" set END_DT = @END_DT");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY, UPDATED_DT =@UPDATED_DT,FUNC_ID =@FUNC_ID");
            sb.Append(" where DEPT_NO = @DEPT_NO ");
            sb.Append(" and @START_DT between START_DT and END_DT ");

            ht.Add("@END_DT", Convert.ToDateTime(START_DT).AddDays(-1).ToString("yyyy/MM/dd"));
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@START_DT", START_DT);
            ht.Add("@UPDATED_DT", UPDATED_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //是否有非離職(在職/留停/返校)員工在該部門,回傳工號
    public string getH_EMP_ID(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" DECLARE @EMP_ID_ALL VARCHAR(MAX)=''
                        SELECT @EMP_ID_ALL = @EMP_ID_ALL + EMP_ID + ','
                        FROM VW_H_EMP_DATA
                        WHERE 1=1
                        AND  DEPT_NO=@DEPT_NO
                        and  EMP_STATUS<>'99'   --非離職
                        ;
                        SELECT left(@EMP_ID_ALL,11) AS EMP_ID;
            ");
            ht.Add("@DEPT_NO", dept_no);
            DataTable dt = dbConn.Query(sb, ht, true);

            return Convert.ToString(dt.Rows[0]["EMP_ID"]);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //是否有是否有應受援員工(應受援履歷檔)在該部門,回傳工號
    public string getASSIST_EMP_ID(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" DECLARE @EMP_ID_ALL VARCHAR(MAX)=''
                    SELECT @EMP_ID_ALL = @EMP_ID_ALL + EMP_ID + ','
                    FROM TB_H_R_EMP_ASSIST 
                    WHERE END_DT IS NULL
                    AND ORI_DEPT_NO =@DEPT_NO
                    SELECT left(@EMP_ID_ALL,11) AS EMP_ID;
            ");           
            ht.Add("@DEPT_NO", dept_no);
            DataTable dt = dbConn.Query(sb, ht, true);
            return Convert.ToString(dt.Rows[0]["EMP_ID"]);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

}

