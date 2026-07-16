using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2IA1100DAO 的摘要描述
/// </summary>
public class CFB2IA1100DAO : BaseDAO
{
    public string INS_TYPE { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_BIRTH_DT { get; set; }
    public string IDENTITY_KIND { get; set; }
    public string COMPANY_CD_NEW { get; set; }
    public string COMPANY_CD_OLD { get; set; }
    public string LICENSE_ID { get; set; }
    public string HR_CHG_CD { get; set; }
    public string CHG_DT { get; set; }
    public string BASIC_SALARY { get; set; }
    public string LABOR_IS_YN { get; set; }
    public string LABOR_CHG_DT { get; set; }
    public string LABOR_INS_AMT { get; set; }
    public string HEALTH_IS_YN { get; set; }
    public string HEALTH_CHG_DT { get; set; }
    public string HEALTH_INS_AMT { get; set; }
    public string PENSION_IS_YN { get; set; }
    public string PENSION_CHG_DT { get; set; }
    public string PENSION_SELF_RATIO { get; set; }
    public string PENSION_INS_AMT { get; set; }
    public string GINS_IS_YN { get; set; }
    public string GROUP_CHG_DT { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    public string IsTWN { get; set; }
    public string PJOB_CD { get; set; }
    public string REMARK { get; set; }

    //for查詢欄位
    public string OPERATION_KIND { get; set; }
    public string NATION_CD { get; set; }
    public string OP_STATUS { get; set; }
    public string OP_DT_S { get; set; }
    public string OP_DT_E { get; set; }
    public string CHG_DT_S { get; set; }
    public string CHG_DT_E { get; set; }

    public string INS_SEX { get; set; }
    public string IS_MASTER { get; set; }
    public string INSC_COMP_RATE { get; set; }
    public bool is_maxEFFECT_EDT { get; set; }
    public string arrTMPLEATAB { get; set; }

    public bool is_LABOR_IS_YN { get; set; }
    public bool is_pLABOR { get; set; }
    public bool isHEALTH_IS_YN { get; set; }
    public bool ispHEALTH { get; set; }
    public bool isPENSION_IS_YN { get; set; }
    public bool ispPENSION { get; set; }
    public string isPENSION_SELF_RATIO { get; set; }
    public bool isGINS_IS_YN { get; set; }
    public bool ispGINS { get; set; }    

    public CFB2IA1100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string operation_kind, string hr_chg_cd,
        string company_cd_old, string emp_id, string nation_cd, string op_status,
        string op_dt_s, string op_dt_e, string chg_dt_s, string chg_dt_e)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,b.EMP_NAME,c.PJOB_DESC,");
            sb.Append(" d.HR_CHG_DESC HR_CHG_DESC,");
            sb.Append(" a.CHG_DT,a.LICENSE_ID,");
            //A: 加保, B: 退保, C: 身份轉換 //改 I: 加保, O: 退保, U: 身份轉換
            if (operation_kind == "I" || operation_kind == "U")
            {
                //公司別
                sb.Append(" (select e.COMPANY_SNAME from TB_H_M_COMPANY e where a.COMPANY_CD_NEW=e.COMPANY_CD) COMPANY_SNAME,");
            }
            else
                sb.Append(" (select e.COMPANY_SNAME from TB_H_M_COMPANY e where a.COMPANY_CD_OLD=e.COMPANY_CD) COMPANY_SNAME,");

            sb.Append(" f.SUB_DESC SUB_DESC ");

            if (op_status == "N")
            {
                #region 待處理
                //A: 加保, B: 退保, C: 身份轉換 //改 I: 加保, O: 退保, U: 身份轉換
                #region 基本月薪

                if (operation_kind == "I" )
                {
                    //sb.Append(",case a.hr_chg_cd when 'D01' then (select i.SALARY_AMT from TB_I_M_3IN1_TXN i  where i.INS_TYPE='C' and i.EMP_ID=a.EMP_ID and i.EFFECT_SDT <= a.CHG_DT and i.EFFECT_EDT >= a.CHG_DT) ");
                    sb.Append(",case a.hr_chg_cd when 'D01' then (select i.SALARY_AMT from TB_I_M_3IN1_TXN i  where i.INS_TYPE='C' and i.EMP_ID=a.EMP_ID and i.EFFECT_EDT = (select MAX(EFFECT_EDT) from TB_I_M_3IN1_TXN where INS_TYPE='C' and EMP_ID = a.EMP_ID)) ");
                    sb.Append(" else dbo.FN_S_BASE_SALARY(a.EMP_ID,a.CHG_DT) end as BASIC_SALARY");
                }
                else if (operation_kind == "O" || operation_kind == "U")
                {
                    sb.Append(" , case when a.COMPANY_CD_NEW='K' then dbo.FN_S_BASE_SALARY(a.EMP_ID,a.CHG_DT) /*才庫轉國瑞*/ else ");
                    sb.Append("  (select i.SALARY_AMT from TB_I_M_3IN1_TXN i  where i.INS_TYPE='A' and i.EMP_ID=a.EMP_ID and i.EFFECT_SDT <= a.CHG_DT and i.EFFECT_EDT >= a.CHG_DT) end as BASIC_SALARY");
                }
                #endregion
                #region 勞保

                if (operation_kind == "I")
                {
                    sb.Append(" ,'Y' LABOR_IS_YN");
                    sb.Append(" ,a.CHG_DT LABOR_CHG_DT");
                    //勞保_投保金額=CALL FN <<FN_I_AMT 取回投保金額>>('A',基本月薪) //A.勞保 
                    //sb.Append(",case a.hr_chg_cd when 'D01' then dbo.FN_I_AMT('A',(select i.SALARY_AMT from TB_I_M_3IN1_TXN i where i.INS_TYPE='C' and i.EMP_ID=a.EMP_ID and i.EFFECT_SDT <= a.CHG_DT and i.EFFECT_EDT >= a.CHG_DT))  ");
                    sb.Append(",case a.hr_chg_cd when 'D01' then dbo.FN_I_AMT('A',(select i.SALARY_AMT from TB_I_M_3IN1_TXN i where i.INS_TYPE='C' and i.EMP_ID=a.EMP_ID and i.EFFECT_EDT = (select MAX(EFFECT_EDT) from TB_I_M_3IN1_TXN where INS_TYPE='C' and EMP_ID = a.EMP_ID)))  ");
                    sb.Append(" else dbo.FN_I_AMT('A',dbo.FN_S_BASE_SALARY(a.EMP_ID,a.CHG_DT) ) end as LABOR_INS_AMT ");
                }
                else if (operation_kind == "U")
                {
                    sb.Append(" ,'Y' LABOR_IS_YN");
                    sb.Append(" ,a.CHG_DT LABOR_CHG_DT");
                    sb.Append(" ,case when a.COMPANY_CD_NEW='K' then dbo.FN_I_AMT('A',dbo.FN_S_BASE_SALARY(a.EMP_ID,a.CHG_DT)) ");
                    sb.Append(" else (select top 1 i.INS_AMT from TB_I_M_3IN1_TXN i  where i.INS_TYPE='A' and i.EMP_ID=a.EMP_ID and i.IDENTITY_KIND='1' and i.LICENSE_ID=a.LICENSE_ID order by i.EFFECT_SDT desc)");
                    sb.Append(" end as LABOR_INS_AMT");
                }
                else if (operation_kind == "O")
                {
                    sb.Append(" ,a.LABOR_IS_YN");
                    sb.Append(" ,a.CHG_DT LABOR_CHG_DT");
                    //call fn <<FN_I_AMT_NOW 取得目前投保金額>> ('A',gride.工號) //A.勞保
                    sb.Append(" ,dbo.FN_I_AMT_NOW('A', a.EMP_ID) LABOR_INS_AMT");
                }
                #endregion
                #region 健保

                if (operation_kind == "I")
                {
                    sb.Append(" ,'Y' HEALTH_IS_YN");
                    sb.Append(" ,a.CHG_DT HEALTH_CHG_DT");
                    //健保_投保金額=CALL FN <<FN_I_AMT 取回投保金額>>('B',基本月薪) //B.健保 
                    //sb.Append(" ,CASE A.HR_CHG_CD WHEN 'D01' THEN dbo.FN_I_AMT('B',(select i.SALARY_AMT from TB_I_M_3IN1_TXN i  where i.INS_TYPE='C' and i.EMP_ID=a.EMP_ID and i.EFFECT_SDT <= a.CHG_DT and i.EFFECT_EDT >= a.CHG_DT) ) ");
                    sb.Append(" ,CASE A.HR_CHG_CD WHEN 'D01' THEN dbo.FN_I_AMT('B',(select i.SALARY_AMT from TB_I_M_3IN1_TXN i  where i.INS_TYPE='C' and i.EMP_ID=a.EMP_ID and i.EFFECT_EDT = (select MAX(EFFECT_EDT) from TB_I_M_3IN1_TXN where INS_TYPE='C' and EMP_ID = a.EMP_ID)) ) ");
                    sb.Append(" else dbo.FN_I_AMT('B',dbo.FN_S_BASE_SALARY(a.EMP_ID,a.CHG_DT)) end as HEALTH_INS_AMT");
                }
                else if (operation_kind == "U")
                {
                    sb.Append(" ,'Y' HEALTH_IS_YN ,a.CHG_DT HEALTH_CHG_DT ");
                    sb.Append(" ,case when a.COMPANY_CD_NEW='K' then dbo.FN_I_AMT('B',dbo.FN_S_BASE_SALARY(a.EMP_ID,a.CHG_DT)) ");
                    sb.Append(" else (select top 1 i.INS_AMT from TB_I_M_3IN1_TXN i  where i.INS_TYPE='B' and i.EMP_ID=a.EMP_ID and i.IDENTITY_KIND='1' and i.LICENSE_ID=a.LICENSE_ID order by i.EFFECT_SDT desc)");
                    sb.Append(" END AS HEALTH_INS_AMT");
                }
                else if (operation_kind == "O")
                {
                    sb.Append(" ,a.HEALTH_IS_YN");
                    sb.Append(" ,a.CHG_DT HEALTH_CHG_DT");
                    //call fn <<FN_I_AMT_NOW 取得目前投保金額>> ('B',gride.工號) //B.健保
                    sb.Append(" ,dbo.FN_I_AMT_NOW('B', a.EMP_ID) HEALTH_INS_AMT");
                }
                #endregion
                #region 勞退

                if (operation_kind == "I")
                {
                    sb.Append(" ,'Y' PENSION_IS_YN");
                    sb.Append(" ,a.CHG_DT PENSION_CHG_DT");
                    //勞退_投保金額=CALL FN <<FN_I_AMT 取回投保金額>>('C',基本月薪) //C.勞退 
                    //sb.Append(" ,case a.HR_CHG_CD when 'D01' then dbo.FN_I_AMT('C',(select i.SALARY_AMT from TB_I_M_3IN1_TXN i  where i.INS_TYPE='C' and i.EMP_ID=a.EMP_ID and i.EFFECT_SDT <= a.CHG_DT and i.EFFECT_EDT >= a.CHG_DT )) ");
                    sb.Append(" ,case a.HR_CHG_CD when 'D01' then dbo.FN_I_AMT('C',(select i.SALARY_AMT from TB_I_M_3IN1_TXN i  where i.INS_TYPE='C' and i.EMP_ID=a.EMP_ID and i.EFFECT_EDT = (select MAX(EFFECT_EDT) from TB_I_M_3IN1_TXN where INS_TYPE='C' and EMP_ID = a.EMP_ID) )) ");
                    sb.Append(" ELSE dbo.FN_I_AMT('C',dbo.FN_S_BASE_SALARY(a.EMP_ID,a.CHG_DT)) END as PENSION_INS_AMT");
                }
                else if (operation_kind == "U")
                {
                    sb.Append(" ,'Y' PENSION_IS_YN,a.CHG_DT PENSION_CHG_DT ");
                    sb.Append(" ,case when a.COMPANY_CD_NEW='K' then dbo.FN_I_AMT('C',dbo.FN_S_BASE_SALARY(a.EMP_ID,a.CHG_DT)) ");
                    sb.Append(" else  (select top 1 i.INS_AMT from TB_I_M_3IN1_TXN i  where i.INS_TYPE='C' and i.EMP_ID=a.EMP_ID and i.IDENTITY_KIND='1' and i.LICENSE_ID=a.LICENSE_ID order by i.EFFECT_SDT desc)");
                    //勞退_投保金額=CALL FN <<FN_I_AMT 取回投保金額>>('C',基本月薪) //C.勞退 
                    sb.Append("  END AS PENSION_INS_AMT");
                }
                else if (operation_kind == "O")
                {
                    sb.Append(" ,a.PENSION_IS_YN");
                    sb.Append(" ,a.CHG_DT PENSION_CHG_DT");
                    //call fn <<FN_I_AMT_NOW 取得目前投保金額>> ('C',gride.工號) //C.勞退 
                    sb.Append(" ,dbo.FN_I_AMT_NOW('C', a.EMP_ID) PENSION_INS_AMT");
                }

                if (operation_kind == "I" || operation_kind == "U")
                {
                    sb.Append(" ,'0' PENSION_SELF_RATIO");  //個人自提率(%)(預設給零)
                    sb.Append(" ,'Y' GINS_IS_YN");
                    sb.Append(" ,a.CHG_DT GROUP_CHG_DT");
                }
                else if (operation_kind == "O")
                {
                    sb.Append(" ,a.PENSION_SELF_RATIO");
                    sb.Append(" ,a.GINS_IS_YN");
                    sb.Append(" ,a.GROUP_CHG_DT");
                }
                #endregion

                sb.Append(" ,'' OP_DT");
                sb.Append(" ,'' OP_MSG");
                #endregion
            }
            else if (op_status == "Y")
            {
                #region 已處理
                sb.Append(" ,a.BASIC_SALARY");
                sb.Append(" ,a.LABOR_IS_YN");
                sb.Append(" ,a.LABOR_CHG_DT");
                sb.Append(" ,a.LABOR_INS_AMT");
                sb.Append(" ,a.HEALTH_IS_YN");
                sb.Append(" ,a.HEALTH_CHG_DT");
                sb.Append(" ,a.HEALTH_INS_AMT");
                sb.Append(" ,a.PENSION_IS_YN");
                sb.Append(" ,a.PENSION_CHG_DT");
                sb.Append(" ,a.PENSION_INS_AMT");
                sb.Append(" ,a.PENSION_SELF_RATIO");
                sb.Append(" ,a.GINS_IS_YN");
                sb.Append(" ,a.GROUP_CHG_DT");
                sb.Append(" ,a.OP_DT");
                sb.Append(" ,a.OP_MSG");
                #endregion
            }
            else if (op_status == "E")
            {
                #region 處理異常
                sb.Append(" ,a.BASIC_SALARY");
                sb.Append(" ,a.LABOR_IS_YN");
                sb.Append(" ,a.LABOR_CHG_DT");
                sb.Append(" ,a.LABOR_INS_AMT");
                sb.Append(" ,a.HEALTH_IS_YN");
                sb.Append(" ,a.HEALTH_CHG_DT");
                sb.Append(" ,a.HEALTH_INS_AMT");
                sb.Append(" ,a.PENSION_IS_YN");
                sb.Append(" ,a.PENSION_CHG_DT");
                sb.Append(" ,a.PENSION_INS_AMT");
                sb.Append(" ,a.PENSION_SELF_RATIO");
                sb.Append(" ,a.GINS_IS_YN");
                sb.Append(" ,a.GROUP_CHG_DT");
                sb.Append(" ,a.OP_DT");
                sb.Append(" ,a.OP_MSG");
                #endregion
            }

            sb.Append(" ,a.PJOB_CD"); //職務區分: PJ60是研修生,PJ50是建教生
            //是否本國籍
            sb.Append(" ,CASE WHEN a.NATION_CD=(select CODE_VAL1 from TB_9_M_PARAMETER  where MAIN_CD='TWN_CD') THEN 'Y' ELSE 'N' END IsTWN");
            //最大勞退個人自提率%
            sb.Append(" ,(select CODE_VAL1 from TB_9_M_PARAMETER where MAIN_CD='INSC_SELF_RATE') MaxPENSION_SELF_RATIO");
            sb.Append(" ,a.COMPANY_CD_NEW"); //聘用單位(會社別)_新
            sb.Append(" ,a.COMPANY_CD_OLD"); //聘用單位(會社別)_原
            sb.Append(" ,a.HR_CHG_CD"); //人事異動代碼
            sb.Append(" ,REPLACE(CONVERT(char(10), b.BIRTH_DT, 120),'-','/') BIRTH_DT"); //生日
            sb.Append(" from TB_I_M_CHG_TXN a ");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID ");
            sb.Append(" left join VW_TB_H_M_PJOB c  on a.PJOB_CD=c.PJOB_CD ");
            sb.Append(" left join TB_H_M_HR_CHANGE_CODE d on a.HR_CHG_CD=d.HR_CHG_CD ");
            sb.Append(" left join TB_9_M_COMM_D f on f.SYS_CD='HB' and f.MAIN_CD ='NATION_CD' and a.NATION_CD=f.SUB_CD ");
            sb.Append(" where 1=1 ");
            #region 查詢條件

            //作業別
            if (operation_kind != "-1" && operation_kind != null)
            {
                sb.Append(" and a.OPERATION_KIND = @OPERATION_KIND ");
                ht.Add("@OPERATION_KIND", operation_kind);
            }
            //處理狀況
            if (op_status != "-1" && op_status != null)
            {
                sb.Append(" and a.OP_STATUS = @OP_STATUS ");
                ht.Add("@OP_STATUS", op_status);
            }
            //異動別
            if (hr_chg_cd != "")
            {
                sb.Append(" and a.HR_CHG_CD = @HR_CHG_CD ");
                ht.Add("@HR_CHG_CD", hr_chg_cd);
            }
            //公司別
            if (company_cd_old != "")
            {
                if (operation_kind == "I" || operation_kind == "U")
                    sb.Append(" and a.COMPANY_CD_NEW = @COMPANY_CD_OLD ");
                else
                    sb.Append(" and a.COMPANY_CD_OLD = @COMPANY_CD_OLD ");
                ht.Add("@COMPANY_CD_OLD", company_cd_old);
            }
            //工號
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            //國籍代號
            if (nation_cd != "")
            {
                sb.Append(" and a.NATION_CD = @NATION_CD ");
                ht.Add("@NATION_CD", nation_cd);
            }
            //處理日期
            if (op_dt_s != "")
            {
                if (op_dt_e != "")
                {
                    sb.Append(" and a.OP_DT >= CONVERT(datetime,@op_dt_s) and a.OP_DT <= CONVERT(datetime,@op_dt_e)");
                    ht.Add("@op_dt_s", op_dt_s + " 00:00:00");
                    ht.Add("@op_dt_e", op_dt_e + " 23:59:59");
                }
                else
                {
                    sb.Append(" and a.OP_DT >= CONVERT(datetime,@op_dt_s) ");
                    ht.Add("@op_dt_s", op_dt_s + " 00:00:00");
                }
            }
            else if (op_dt_e != "")
            {
                sb.Append(" and a.OP_DT <= CONVERT(datetime,@op_dt_e) ");
                ht.Add("@op_dt_e", op_dt_e + " 23:59:59");
            }
            //人事異動
            if (chg_dt_s != "")
            {
                if (chg_dt_e != "")
                {
                    sb.Append(" and a.CHG_DT >= CONVERT(datetime,@chg_dt_s) and a.CHG_DT <= CONVERT(datetime,@chg_dt_e)");
                    ht.Add("@chg_dt_s", chg_dt_s + " 00:00:00");
                    ht.Add("@chg_dt_e", chg_dt_e + " 23:59:59");
                }
                else
                {
                    sb.Append(" and a.CHG_DT >= CONVERT(datetime,@chg_dt_s) ");
                    ht.Add("@chg_dt_s", chg_dt_s + " 00:00:00");
                }
            }
            else if (chg_dt_e != "")
            {
                sb.Append(" and a.CHG_DT <= CONVERT(datetime,@chg_dt_e) ");
                ht.Add("@chg_dt_e", chg_dt_e + " 23:59:59");
            }
            #endregion

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string operation_kind, string hr_chg_cd,
        string company_cd_old, string emp_id, string nation_cd, string op_status,
        string op_dt_s, string op_dt_e, string chg_dt_s, string chg_dt_e)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_I_M_CHG_TXN a ");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID ");
            sb.Append(" left join VW_TB_H_M_PJOB c  on a.PJOB_CD=c.PJOB_CD ");
            sb.Append(" left join TB_H_M_HR_CHANGE_CODE d on a.HR_CHG_CD=d.HR_CHG_CD ");
            sb.Append(" left join TB_9_M_COMM_D f on f.SYS_CD='HB' and f.MAIN_CD ='NATION_CD' and a.NATION_CD=f.SUB_CD ");
            sb.Append(" where 1=1 ");

            #region 查詢條件

            //作業別
            if (operation_kind != "-1" && operation_kind != null)
            {
                sb.Append(" and a.OPERATION_KIND = @OPERATION_KIND ");
                ht.Add("@OPERATION_KIND", operation_kind);
            }
            //處理狀況
            if (op_status != "-1" && op_status != null)
            {
                sb.Append(" and a.OP_STATUS = @OP_STATUS ");
                ht.Add("@OP_STATUS", op_status);
            }
            //異動別
            if (hr_chg_cd != "")
            {
                sb.Append(" and a.HR_CHG_CD = @HR_CHG_CD ");
                ht.Add("@HR_CHG_CD", hr_chg_cd);
            }
            //公司別
            if (company_cd_old != "")
            {
                if (operation_kind == "I" || operation_kind == "U")
                    sb.Append(" and a.COMPANY_CD_NEW = @COMPANY_CD_OLD ");
                else
                    sb.Append(" and a.COMPANY_CD_OLD = @COMPANY_CD_OLD ");
                ht.Add("@COMPANY_CD_OLD", company_cd_old);
            }
            //工號
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            //國籍代號
            if (nation_cd != "")
            {
                sb.Append(" and a.NATION_CD = @NATION_CD ");
                ht.Add("@NATION_CD", nation_cd);
            }
            //處理日期
            if (op_dt_s != "")
            {
                if (op_dt_e != "")
                {
                    sb.Append(" and a.OP_DT >= CONVERT(datetime,@op_dt_s) and a.OP_DT <= CONVERT(datetime,@op_dt_e)");
                    ht.Add("@op_dt_s", op_dt_s + " 00:00:00");
                    ht.Add("@op_dt_e", op_dt_e + " 23:59:59");
                }
                else
                {
                    sb.Append(" and a.OP_DT >= CONVERT(datetime,@op_dt_s) ");
                    ht.Add("@op_dt_s", op_dt_s + " 00:00:00");
                }
            }
            else if (op_dt_e != "")
            {
                sb.Append(" and a.OP_DT <= CONVERT(datetime,@op_dt_e) ");
                ht.Add("@op_dt_e", op_dt_e + " 23:59:59");
            }
            //人事異動
            if (chg_dt_s != "")
            {
                if (chg_dt_e != "")
                {
                    sb.Append(" and a.CHG_DT >= CONVERT(datetime,@chg_dt_s) and a.CHG_DT <= CONVERT(datetime,@chg_dt_e)");
                    ht.Add("@chg_dt_s", chg_dt_s + " 00:00:00");
                    ht.Add("@chg_dt_e", chg_dt_e + " 23:59:59");
                }
                else
                {
                    sb.Append(" and a.CHG_DT >= CONVERT(datetime,@chg_dt_s) ");
                    ht.Add("@chg_dt_s", chg_dt_s + " 00:00:00");
                }
            }
            else if (chg_dt_e != "")
            {
                sb.Append(" and a.CHG_DT <= CONVERT(datetime,@chg_dt_e) ");
                ht.Add("@chg_dt_e", chg_dt_e + " 23:59:59");
            }
            #endregion

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

    //excel 匯出三合一加保資料
    public DataTable searchResult()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.CHG_APP_TYPE,LEFT(b.LABOR_ORG_ID,8) LAB8,RIGHT(RTRIM(b.LABOR_ORG_ID),1) LAB_CHK_CD,b.HEALTH_ORG_ID,");
            sb.Append(" a.INS_HLR_TYPE,a.IDENTITY_KIND,a.LAB_FORIGN_YN,a.LICENSE_ID,a.EMP_NAME,a.EMP_BIRTH_DT,a.SALARY,");
            sb.Append(" a.HEA_AFT_AMT,a.SPTYP,a.F_LICENCE_CD,a.F_NAME,a.F_BIRTH_DT,a.FAMILY_RELATION,b.HEALTH_BUSINESS_ID,");
            sb.Append(" a.CHG_TYPE,a.HEA_CHT_DT,a.INS_SEX,a.RET_DIFFENCT_TYPE,a.BOSS_RATE,a.SEFT_RATE,a.RET_DT");
            sb.Append(" from TB_I_R_3IN1_REPORTDATA a left join TB_H_M_COMPANY b on b.COMPANY_CD=a.COMPANY_CD_NEW");
            sb.Append(" where a.CHG_APP_TYPE='4'");
            #region 查詢條件
            ////作業別
            //if (OPERATION_KIND != "-1" && OPERATION_KIND != null)
            //{
            //     sb.Append(" and a.OPERATION_KIND = @OPERATION_KIND ";
            //    ht.Add("@OPERATION_KIND", OPERATION_KIND);
            //}
            ////異動別
            //if (HR_CHG_CD != "")
            //{
            //     sb.Append(" and a.HR_CHG_CD = @HR_CHG_CD ";
            //    ht.Add("@HR_CHG_CD", HR_CHG_CD);
            //}
            //公司別
            if (COMPANY_CD_OLD != "")
            {
                sb.Append(" and a.COMPANY_CD_NEW = @COMPANY_CD_OLD ");
                ht.Add("@COMPANY_CD_OLD", COMPANY_CD_OLD);
            }
            //工號
            if (EMP_ID != "")
            {
                sb.Append(" and a.SYS_DESC = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }
            ////國籍代號
            //if (NATION_CD != "")
            //{
            //     sb.Append(" and a.NATION_CD = @NATION_CD ";
            //    ht.Add("@NATION_CD", NATION_CD);
            //}
            ////處理狀況
            //if (OP_STATUS != "-1" && OP_STATUS != null)
            //{
            //     sb.Append(" and a.OP_STATUS = @OP_STATUS ";
            //    ht.Add("@OP_STATUS", OP_STATUS);
            //}
            //處理日期
            if (OP_DT_S != "")
            {
                if (OP_DT_E != "")
                {
                    sb.Append(" and a.OP_DT >= CONVERT(datetime,@OP_DT_S) and a.OP_DT <= CONVERT(datetime,@OP_DT_E)");
                    ht.Add("@OP_DT_S", OP_DT_S + " 00:00:00");
                    ht.Add("@OP_DT_E", OP_DT_E + " 23:59:59");
                }
                else
                {
                    sb.Append(" and a.OP_DT >= CONVERT(datetime,@OP_DT_S) ");
                    ht.Add("@OP_DT_S", OP_DT_S + " 00:00:00");
                }
            }
            else if (OP_DT_E != "")
            {
                sb.Append(" and a.OP_DT <= CONVERT(datetime,@OP_DT_E) ");
                ht.Add("@OP_DT_E", OP_DT_E + " 23:59:59");
            }
            ////人事異動
            //if (CHG_DT_S != "")
            //{
            //    if (CHG_DT_E != "")
            //    {
            //         sb.Append(" and CHG_DT >= CONVERT(datetime,@CHG_DT_S) and CHG_DT <= CONVERT(datetime,@CHG_DT_E)";
            //        ht.Add("@CHG_DT_S", CHG_DT_S);
            //        ht.Add("@CHG_DT_E", CHG_DT_E);
            //    }
            //    else
            //    {
            //         sb.Append(" and CHG_DT >= CONVERT(datetime,@CHG_DT_S) ";
            //        ht.Add("@CHG_DT_S", CHG_DT_S);
            //    }
            //}
            //else if (CHG_DT_E != "")
            //{
            //     sb.Append(" and CHG_DT <= CONVERT(datetime,@CHG_DT_E) ";
            //    ht.Add("@CHG_DT_E", CHG_DT_E);
            //}
            #endregion

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //excel 匯出三合一退保資料
    public DataTable searchResult2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.CHG_APP_TYPE,LEFT(b.LABOR_ORG_ID,8) LAB8,RIGHT(RTRIM(b.LABOR_ORG_ID),1) LAB_CHK_CD,b.HEALTH_ORG_ID,");
            sb.Append(" b.HEALTH_BUSINESS_ID,a.INS_HLR_TYPE,a.IDENTITY_KIND,a.LAB_FORIGN_YN,a.EMP_NAME,");
            sb.Append(" CASE WHEN a.LAB_FORIGN_YN='' THEN a.LICENSE_ID ELSE '' END LICENSE_ID1,"); //被保險人身份證號
            sb.Append(" CASE WHEN a.LAB_FORIGN_YN='Y' THEN a.LICENSE_ID ELSE '' END LICENSE_ID2,"); //被保險人身份居留證
            sb.Append(" a.EMP_BIRTH_DT,a.CHG_TYPE,a.CHG_REASON_CD,a.CHG_REASON_CD_DESC,a.HEA_CHT_DT");
            sb.Append(" from TB_I_R_3IN1_REPORTDATA a left join TB_H_M_COMPANY b on b.COMPANY_CD=a.COMPANY_CD_NEW");
            sb.Append(" where a.CHG_APP_TYPE='2'");
            #region 查詢條件
            ////作業別
            //if (OPERATION_KIND != "-1" && OPERATION_KIND != null)
            //{
            //     sb.Append(" and a.OPERATION_KIND = @OPERATION_KIND ";
            //    ht.Add("@OPERATION_KIND", OPERATION_KIND);
            //}
            ////異動別
            //if (HR_CHG_CD != "")
            //{
            //     sb.Append(" and a.HR_CHG_CD = @HR_CHG_CD ";
            //    ht.Add("@HR_CHG_CD", HR_CHG_CD);
            //}
            //公司別
            if (COMPANY_CD_OLD != "")
            {
                sb.Append(" and a.COMPANY_CD_NEW = @COMPANY_CD_OLD ");
                ht.Add("@COMPANY_CD_OLD", COMPANY_CD_OLD);
            }
            //工號
            if (EMP_ID != "")
            {
                sb.Append(" and a.SYS_DESC = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }
            ////國籍代號
            //if (NATION_CD != "")
            //{
            //     sb.Append(" and a.NATION_CD = @NATION_CD ";
            //    ht.Add("@NATION_CD", NATION_CD);
            //}
            ////處理狀況
            //if (OP_STATUS != "-1" && OP_STATUS != null)
            //{
            //     sb.Append(" and a.OP_STATUS = @OP_STATUS ";
            //    ht.Add("@OP_STATUS", OP_STATUS);
            //}
            //處理日期
            if (OP_DT_S != "")
            {
                if (OP_DT_E != "")
                {
                    sb.Append(" and a.OP_DT >= CONVERT(datetime,@OP_DT_S) and a.OP_DT <= CONVERT(datetime,@OP_DT_E)");
                    ht.Add("@OP_DT_S", OP_DT_S + " 00:00:00");
                    ht.Add("@OP_DT_E", OP_DT_E + " 23:59:59");
                }
                else
                {
                    sb.Append(" and a.OP_DT >= CONVERT(datetime,@OP_DT_S) ");
                    ht.Add("@OP_DT_S", OP_DT_S + " 00:00:00");
                }
            }
            else if (OP_DT_E != "")
            {
                sb.Append(" and a.OP_DT <= CONVERT(datetime,@OP_DT_E) ");
                ht.Add("@OP_DT_E", OP_DT_E + " 23:59:59");
            }
            ////人事異動
            //if (CHG_DT_S != "")
            //{
            //    if (CHG_DT_E != "")
            //    {
            //         sb.Append(" and CHG_DT >= CONVERT(datetime,@CHG_DT_S) and CHG_DT <= CONVERT(datetime,@CHG_DT_E)";
            //        ht.Add("@CHG_DT_S", CHG_DT_S);
            //        ht.Add("@CHG_DT_E", CHG_DT_E);
            //    }
            //    else
            //    {
            //         sb.Append(" and CHG_DT >= CONVERT(datetime,@CHG_DT_S) ";
            //        ht.Add("@CHG_DT_S", CHG_DT_S);
            //    }
            //}
            //else if (CHG_DT_E != "")
            //{
            //     sb.Append(" and CHG_DT <= CONVERT(datetime,@CHG_DT_E) ";
            //    ht.Add("@CHG_DT_E", CHG_DT_E);
            //}
            #endregion

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得伙食津貼
    public int getFOOD_SUBSIDY()
    {
        try
        {
            int FOOD_SUBSIDY = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CODE_VAL1 from TB_9_M_PARAMETER where SYS_CD='SC' and MAIN_CD='FOOD_SUBSIDY'");
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                FOOD_SUBSIDY = Convert.ToInt32(dt.Rows[0]["CODE_VAL1"].ToString());
            }

            return FOOD_SUBSIDY;
        }
        catch (Exception)
        {
            throw;
        }

    }

    //取得勞退自提上限
    public double getMaxPENSION_SELF_RATIO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CODE_VAL1 from TB_9_M_PARAMETER where SYS_CD='IA' and MAIN_CD='INSC_SELF_RATE'");
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                return Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString());
            }

            return 100;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得被保險人出生日期
    public string getEMP_BIRTH_DT()
    {
        try
        {
            string EMP_BIRTH_DT = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select BIRTH_DT from TB_H_M_EMP where EMP_ID=@emp_id");
            ht.Add("@emp_id", EMP_ID);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                EMP_BIRTH_DT = Convert.ToDateTime(dt.Rows[0]["BIRTH_DT"]).ToString("yyyy/MM/dd");
            }

            return EMP_BIRTH_DT;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得現有TB_I_M_GROUP_TXN資料(加保或身份轉換)
    public DataTable getGROUP_TXNData(string gins_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID from TB_I_M_GROUP_TXN ");
            sb.Append(" where GINS_KIND=@gins_kind and TARGET_TYPE='1' and EMP_ID=@emp_id ");
            sb.Append(" and INS_ENTRY_DT <= @cur_chg_dt and INS_QUIT_DT >= @cur_chg_dt");
            ht.Add("@gins_kind", gins_kind);
            ht.Add("@emp_id", EMP_ID);
            ht.Add("@cur_chg_dt", GROUP_CHG_DT);

            return dbConn.QueryT(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //取得現有TB_I_M_GROUP_TXN資料(退保)
    public DataTable getGROUP_TXNData2(string gins_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID from TB_I_M_GROUP_TXN ");
            sb.Append(" where GINS_KIND=@gins_kind and TARGET_TYPE='1' and EMP_ID=@emp_id and INS_QUIT_DT = '9999/12/31'");
            ht.Add("@gins_kind", gins_kind);
            ht.Add("@emp_id", EMP_ID);

            return dbConn.QueryT(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //紀錄保險處理異常
    public void updateCHG_TXN(string msg)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_I_M_CHG_TXN ");
            sb.Append(" set OP_STATUS='E',OP_MSG=@op_msg,UPDATED_BY=@updated_by,UPDATED_DT=GETDATE(),FUNC_ID=@func_id");
            sb.Append(" where EMP_ID=@emp_id and COMPANY_CD_NEW=@company_cd_new and LICENSE_ID=@LICENSE_ID and CONVERT(char(10),CHG_DT, 111)=@chg_dt");

            ht.Add("@emp_id", EMP_ID);
            ht.Add("@company_cd_new", COMPANY_CD_NEW);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@chg_dt", CHG_DT);
            ht.Add("@updated_by", UPDATED_BY);
            ht.Add("@func_id", FUNC_ID);
            ht.Add("@op_msg", msg);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得現有TB_I_M_3IN1_TXN資料(加保或身份轉換)
    public DataTable get3IN1_TXNData(string ins_type, string chg_dt, string operation_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select INS_TYPE from TB_I_M_3IN1_TXN ");
            sb.Append(" where INS_TYPE=@ins_type and IDENTITY_KIND='1' and EMP_ID=@emp_id ");
            sb.Append(" and EFFECT_SDT <= @cur_chg_dt and EFFECT_EDT >= @cur_chg_dt");
            //畫面.作業別="身份轉換"
          //  if (operation_kind == "U")
          //  {
                sb.Append(" and COMPANY_CD=@company_cd_new");
                ht.Add("@company_cd_new", COMPANY_CD_NEW);
          //  }

            ht.Add("@ins_type", ins_type);
            ht.Add("@emp_id", EMP_ID);
            ht.Add("@cur_chg_dt", chg_dt);

            return dbConn.QueryT(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //取得現有TB_I_M_3IN1_TXN資料(退保)
    public DataTable get3IN1_TXNData(string ins_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select INS_TYPE from TB_I_M_3IN1_TXN ");
            sb.Append(" where INS_TYPE=@ins_type and IDENTITY_KIND='1' and EMP_ID=@emp_id and EFFECT_EDT = '9999/12/31'");
            sb.Append(" and COMPANY_CD=@company_cd_new");
            ht.Add("@ins_type", ins_type);
            ht.Add("@emp_id", EMP_ID);
            ht.Add("@company_cd_new", COMPANY_CD_NEW);

            return dbConn.QueryT(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //更新 [TB_I_M_CHG_TXN 保險一括異動記錄檔]
    public void updateCHG_TXN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_I_M_CHG_TXN");
            sb.Append(" set BASIC_SALARY=@BASIC_SALARY,LABOR_IS_YN=@LABOR_IS_YN,LABOR_CHG_DT=@LABOR_CHG_DT,");
            sb.Append(" LABOR_INS_AMT=@LABOR_INS_AMT,HEALTH_IS_YN=@HEALTH_IS_YN,HEALTH_CHG_DT=@HEALTH_CHG_DT,");
            sb.Append(" HEALTH_INS_AMT=@HEALTH_INS_AMT,PENSION_IS_YN=@PENSION_IS_YN,PENSION_CHG_DT=@PENSION_CHG_DT,");
            sb.Append(" PENSION_SELF_RATIO=@PENSION_SELF_RATIO,PENSION_INS_AMT=@PENSION_INS_AMT,GINS_IS_YN=@GINS_IS_YN,");
            sb.Append(" GROUP_CHG_DT=@GROUP_CHG_DT,OP_STATUS='Y',OP_DT=GETDATE(),UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EMP_ID=@EMP_ID and COMPANY_CD_NEW=@COMPANY_CD_NEW and LICENSE_ID=@LICENSE_ID and CONVERT(char(10),CHG_DT, 111)=@CHG_DT");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@COMPANY_CD_NEW", COMPANY_CD_NEW);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@CHG_DT", CHG_DT);
            ht.Add("@BASIC_SALARY", BASIC_SALARY);
            ht.Add("@LABOR_IS_YN", LABOR_IS_YN);
            if (LABOR_CHG_DT != "")
                ht.Add("@LABOR_CHG_DT", LABOR_CHG_DT);
            else
                ht.Add("@LABOR_CHG_DT", DBNull.Value);
            ht.Add("@LABOR_INS_AMT", LABOR_INS_AMT);
            ht.Add("@HEALTH_IS_YN", HEALTH_IS_YN);
            if (HEALTH_CHG_DT != "")
                ht.Add("@HEALTH_CHG_DT", HEALTH_CHG_DT);
            else
                ht.Add("@HEALTH_CHG_DT", DBNull.Value);
            ht.Add("@HEALTH_INS_AMT", HEALTH_INS_AMT);
            ht.Add("@PENSION_IS_YN", PENSION_IS_YN);
            if (PENSION_CHG_DT != "")
                ht.Add("@PENSION_CHG_DT", PENSION_CHG_DT);
            else
                ht.Add("@PENSION_CHG_DT", DBNull.Value);
            ht.Add("@PENSION_SELF_RATIO", PENSION_SELF_RATIO);
            ht.Add("@PENSION_INS_AMT", PENSION_INS_AMT);
            ht.Add("@GINS_IS_YN", GINS_IS_YN);
            if (GROUP_CHG_DT != "")
                ht.Add("@GROUP_CHG_DT", GROUP_CHG_DT);
            else
                ht.Add("@GROUP_CHG_DT", DBNull.Value);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
           
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }

    }

    public void updateCHG_TXN_N()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_I_M_CHG_TXN");
            sb.Append(" set BASIC_SALARY=@BASIC_SALARY,LABOR_IS_YN=@LABOR_IS_YN,LABOR_CHG_DT=@LABOR_CHG_DT,");
            sb.Append(" LABOR_INS_AMT=@LABOR_INS_AMT,HEALTH_IS_YN=@HEALTH_IS_YN,HEALTH_CHG_DT=@HEALTH_CHG_DT,");
            sb.Append(" HEALTH_INS_AMT=@HEALTH_INS_AMT,PENSION_IS_YN=@PENSION_IS_YN,PENSION_CHG_DT=@PENSION_CHG_DT,");
            sb.Append(" PENSION_SELF_RATIO=@PENSION_SELF_RATIO,PENSION_INS_AMT=@PENSION_INS_AMT,GINS_IS_YN=@GINS_IS_YN,");
            sb.Append(" GROUP_CHG_DT=@GROUP_CHG_DT,OP_STATUS='N',OP_DT=GETDATE(),UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EMP_ID=@EMP_ID and COMPANY_CD_NEW=@COMPANY_CD_NEW and LICENSE_ID=@LICENSE_ID and CONVERT(char(10),CHG_DT, 111)=@CHG_DT");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@COMPANY_CD_NEW", COMPANY_CD_NEW);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@CHG_DT", CHG_DT);
            ht.Add("@BASIC_SALARY", BASIC_SALARY);
            ht.Add("@LABOR_IS_YN", LABOR_IS_YN);
            if (LABOR_CHG_DT != "")
                ht.Add("@LABOR_CHG_DT", LABOR_CHG_DT);
            else
                ht.Add("@LABOR_CHG_DT", DBNull.Value);
            ht.Add("@LABOR_INS_AMT", LABOR_INS_AMT);
            ht.Add("@HEALTH_IS_YN", HEALTH_IS_YN);
            if (HEALTH_CHG_DT != "")
                ht.Add("@HEALTH_CHG_DT", HEALTH_CHG_DT);
            else
                ht.Add("@HEALTH_CHG_DT", DBNull.Value);
            ht.Add("@HEALTH_INS_AMT", HEALTH_INS_AMT);
            ht.Add("@PENSION_IS_YN", PENSION_IS_YN);
            if (PENSION_CHG_DT != "")
                ht.Add("@PENSION_CHG_DT", PENSION_CHG_DT);
            else
                ht.Add("@PENSION_CHG_DT", DBNull.Value);
            ht.Add("@PENSION_SELF_RATIO", PENSION_SELF_RATIO);
            ht.Add("@PENSION_INS_AMT", PENSION_INS_AMT);
            ht.Add("@GINS_IS_YN", GINS_IS_YN);
            if (GROUP_CHG_DT != "")
                ht.Add("@GROUP_CHG_DT", GROUP_CHG_DT);
            else
                ht.Add("@GROUP_CHG_DT", DBNull.Value);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);           
            dbConn.Execute(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }

    }

    //加保 //新增[TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 ] 
    public void insert3IN1_TXN(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_M_3IN1_TXN ( ");
            sb.Append(" INS_TYPE,EMP_ID,IDENTITY_KIND,LICENSE_ID,EFFECT_SDT,EFFECT_EDT,");
            sb.Append(" SALARY_AMT,INS_AMT,COMPANY_CD,CHG_APP_TYPE,CHG_TYPE_IN,CHG_TYPE_OUT,");
            sb.Append(" CHG_REASON_CD,REMARK,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @INS_TYPE,@EMP_ID,@IDENTITY_KIND,@LICENSE_ID,@EFFECT_SDT,'9999/12/31',");
            sb.Append(" @SALARY_AMT,@INS_AMT,@COMPANY_CD,'4',@CHG_TYPE_IN,'',");            
            #region 身分轉換+薪調
            //sb.Append(" '','',@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            sb.Append(" '',@REMARK,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            #endregion

            ht.Add("@INS_TYPE", INS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            if (INS_TYPE == "A")
            {
                ht.Add("@EFFECT_SDT", LABOR_CHG_DT);
                ht.Add("@INS_AMT", LABOR_INS_AMT);
                ht.Add("@CHG_TYPE_IN", "");
            }
            else if (INS_TYPE == "B")
            {
                ht.Add("@EFFECT_SDT", HEALTH_CHG_DT);
                ht.Add("@INS_AMT", HEALTH_INS_AMT);
                ht.Add("@CHG_TYPE_IN", "1"); //到職起薪
            }
            else if (INS_TYPE == "C")
            {
                ht.Add("@EFFECT_SDT", PENSION_CHG_DT);
                ht.Add("@INS_AMT", PENSION_INS_AMT);
                ht.Add("@CHG_TYPE_IN", "");
            }

            ht.Add("@SALARY_AMT", BASIC_SALARY);
            ht.Add("@COMPANY_CD", COMPANY_CD_NEW);
            #region 身分轉換+薪調
            ht.Add("@REMARK", REMARK);
            #endregion            
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //加保 //新增[TB_I_M_RETIRE_SELFRATE 勞退自提履歷檔 ] 
    public void insertRETIRE_SELFRATE(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_M_RETIRE_SELFRATE ( ");
            sb.Append(" EMP_ID,EFFECT_SDT,EFFECT_EDT,SLEF_RATE,REMARK,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @EMP_ID,@EFFECT_SDT,'9999/12/31',@SLEF_RATE,'',");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EFFECT_SDT", PENSION_CHG_DT);
            ht.Add("@SLEF_RATE", PENSION_SELF_RATIO);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //加保 //新增[TB_I_M_GROUP_TXN 團保主檔 ]
    public void insertGROUP_TXN(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_M_GROUP_TXN ( ");
            sb.Append(" EMP_ID,IDENTITY_KIND,LICENSE_ID,GINS_KIND,TARGET_TYPE,INS_ENTRY_DT,");
            sb.Append(" INS_QUIT_DT,INS_COND_AMT,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @EMP_ID,@IDENTITY_KIND,@LICENSE_ID,'A','1',@INS_ENTRY_DT,");
            sb.Append(" '9999/12/31',0,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@INS_ENTRY_DT", GROUP_CHG_DT);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //加保 //新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料]
    public void insert3IN1_REPORTDATA(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_R_3IN1_REPORTDATA ( ");
            sb.Append(" CHG_APP_TYPE,COMPANY_CD_NEW,INS_HLR_TYPE,IDENTITY_KIND,SYS_DESC,");
            sb.Append(" LAB_FORIGN_YN,LICENSE_ID,EMP_NAME,EMP_BIRTH_DT,SALARY,HEA_BEF_AMT,");
            sb.Append(" HEA_AFT_AMT,SPTYP,F_LICENCE_CD,F_NAME,F_BIRTH_DT,FAMILY_RELATION,");
            sb.Append(" CHG_TYPE,CHG_REASON_CD,CHG_REASON_CD_DESC,HEA_CHT_DT,INS_SEX,RET_DIFFENCT_TYPE,");
            sb.Append(" BOSS_RATE,SEFT_RATE,RET_DT,DATASOURCE,OP_DT,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" '4',@COMPANY_CD_NEW,'2','1',@SYS_DESC,");
            sb.Append(" @LAB_FORIGN_YN,@LICENSE_ID,@EMP_NAME,@EMP_BIRTH_DT,@SALARY,@HEA_BEF_AMT,");
            sb.Append(" @HEA_AFT_AMT,@SPTYP,'','',@F_BIRTH_DT,'',");
            sb.Append(" @CHG_TYPE,'','',@HEA_CHT_DT,@INS_SEX,@RET_DIFFENCT_TYPE,");
            sb.Append(" @BOSS_RATE,@SEFT_RATE,@RET_DT,'A',GETDATE(),");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@COMPANY_CD_NEW", COMPANY_CD_NEW);
            ht.Add("@SYS_DESC", EMP_ID);
            ht.Add("@F_BIRTH_DT", DBNull.Value);
            //勞保_被保險人外籍
            if (IsTWN == "Y")
            {
                ht.Add("@LAB_FORIGN_YN", "");
                ht.Add("@INS_SEX", "");
            }
            else
            {
                ht.Add("@LAB_FORIGN_YN", "Y");
                // "M" //男  else "F" //女 
                if (INS_SEX == "1")
                    ht.Add("@INS_SEX", "M");
                else
                    ht.Add("@INS_SEX", "F");
            }

            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@EMP_BIRTH_DT", EMP_BIRTH_DT);
            ht.Add("@SALARY", BASIC_SALARY);
            //健保處理
            if (HEALTH_IS_YN == "Y")
            {
                ht.Add("@HEA_BEF_AMT", HEALTH_INS_AMT);
                ht.Add("@HEA_AFT_AMT", HEALTH_INS_AMT);
                ht.Add("@CHG_TYPE", "1"); //"1"到職起薪
                ht.Add("@HEA_CHT_DT", HEALTH_CHG_DT);
            }
            else
            {
                ht.Add("@HEA_BEF_AMT", 0);
                ht.Add("@HEA_AFT_AMT", 0);
                ht.Add("@CHG_TYPE", "");
                ht.Add("@HEA_CHT_DT", DBNull.Value);
            }
            //職務區分: PJ60是研修生,PJ50是建教生
            if (PJOB_CD == "PJ50")
                ht.Add("@SPTYP", "T");
            else
                ht.Add("@SPTYP", "");

            //勞退處理
            if (PENSION_IS_YN == "Y")
            {
                //勞退提繳身份別
                if (IS_MASTER == "Y")
                    ht.Add("@RET_DIFFENCT_TYPE", "3"); //顧主自願提繳
                else
                    ht.Add("@RET_DIFFENCT_TYPE", "1"); //強制提繳

                //雇主提撥率% 
                ht.Add("@BOSS_RATE", INSC_COMP_RATE);

                //勞退提繳日期 
                if (HR_CHG_CD == "B14")
                {
                    //轉正社員
                    if (is_maxEFFECT_EDT)
                        ht.Add("@RET_DT", PENSION_CHG_DT);
                    else
                        ht.Add("@RET_DT", DBNull.Value);
                }
                else if (LABOR_CHG_DT != PENSION_CHG_DT)
                    ht.Add("@RET_DT", PENSION_CHG_DT);
                else
                    ht.Add("@RET_DT", DBNull.Value);
            }
            else
            {
                ht.Add("@RET_DIFFENCT_TYPE", "");
                ht.Add("@BOSS_RATE", 0);
                ht.Add("@RET_DT", DBNull.Value);
            }

            ht.Add("@SEFT_RATE", PENSION_SELF_RATIO);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //被保險人性別 1.男  2.女 // "M" //男  else "F" //女
    public string getINS_SEX()
    {
        try
        {
            string INS_SEX = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SEX_CD from TB_H_M_EMP where EMP_ID=@emp_id");
            ht.Add("@emp_id", EMP_ID);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                INS_SEX = dt.Rows[0]["SEX_CD"].ToString();
            }

            return INS_SEX;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得勞退提繳身份別
    public string getIS_MASTER()
    {
        try
        {
            string IS_MASTER = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select IS_MASTER from TB_H_M_EMP where EMP_ID=@emp_id");
            ht.Add("@emp_id", EMP_ID);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                IS_MASTER = dt.Rows[0]["IS_MASTER"].ToString();
            }

            return IS_MASTER;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得雇主提撥率% 
    public string getINSC_COMP_RATE()
    {
        try
        {
            string INSC_COMP_RATE = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CODE_VAL1 from TB_9_M_PARAMETER where SYS_CD='IA' and MAIN_CD='INSC_COMP_RATE'");
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                INSC_COMP_RATE = dt.Rows[0]["CODE_VAL1"].ToString();
            }

            return INSC_COMP_RATE;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //TB_I_M_3IN1_TXN.生效日期迄='9999/12/31' //表示該建教生未退保就轉正社員
    public bool isMaxEFFECT_EDT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EFFECT_EDT from TB_I_M_3IN1_TXN ");
            sb.Append(" where INS_TYPE='A' and IDENTITY_KIND='1' and EMP_ID=@EMP_ID and LICENSE_ID=@LICENSE_ID and EFFECT_EDT = '9999/12/31'");
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EMP_ID", EMP_ID);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //異動[TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 ] //勞保退保
    public void update3IN1_TXN_A(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_I_M_3IN1_TXN");
            sb.Append(" set EFFECT_EDT=@EFFECT_EDT,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where INS_TYPE='A' and EMP_ID=@EMP_ID and IDENTITY_KIND='1' and ");
            sb.Append(" LICENSE_ID=@LICENSE_ID and COMPANY_CD=@COMPANY_CD and EFFECT_EDT='9999/12/31'");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@COMPANY_CD", COMPANY_CD_NEW);
            ht.Add("@EFFECT_EDT", LABOR_CHG_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //異動[TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 ] //健保退保
    public void update3IN1_TXN_B(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_I_M_3IN1_TXN");
            sb.Append(" set CHG_TYPE_OUT=@CHG_TYPE_OUT,CHG_REASON_CD=@CHG_REASON_CD,EFFECT_EDT=@EFFECT_EDT,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            //sb.Append(" where INS_TYPE='B' and EMP_ID=@EMP_ID and IDENTITY_KIND='1' and ");
            //sb.Append(" LICENSE_ID=@LICENSE_ID and COMPANY_CD=@COMPANY_CD and EFFECT_EDT='9999/12/31'");
            sb.Append(" where INS_TYPE='B' and EMP_ID=@EMP_ID and EFFECT_EDT='9999/12/31' and COMPANY_CD=@COMPANY_CD ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@COMPANY_CD", COMPANY_CD_NEW);

            if (arrTMPLEATAB.Split('#')[0] == "")
                ht.Add("@CHG_TYPE_OUT", "2"); //轉出
            else
                ht.Add("@CHG_TYPE_OUT", arrTMPLEATAB.Split('#')[0]);

            if (arrTMPLEATAB.Split('#')[1] == "")
                ht.Add("@CHG_REASON_CD", "1"); //離職
            else
                ht.Add("@CHG_REASON_CD", arrTMPLEATAB.Split('#')[1]);

            ht.Add("@EFFECT_EDT", HEALTH_CHG_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //回傳參數值(健保加保/退保原因別)
    public string getTMPLEATAB()
    {
        try
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select isnull(a.CODE_VAL1,'') as CODE_VAL1,b.EMP_CHG_STATUS as sub_cd,isnull(a.sub_desc,'') as sub_desc from VW_H_CHANGE_CODE b");
            sb.Append(" left join TB_9_M_COMM_D a on a.sys_cd='IA' and a.main_cd='HEA_LEAVE' and a.sub_cd= b.EMP_CHG_STATUS");
            sb.Append(" where b.HR_CHG_CD=@HR_CHG_CD");
            ht.Add("@HR_CHG_CD", HR_CHG_CD);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["CODE_VAL1"] + "#" + dt.Rows[0]["sub_cd"] + "#" + dt.Rows[0]["sub_desc"];
            }
            else
                result = "##";

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //異動[TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 ] //勞退退保
    public void update3IN1_TXN_C(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_I_M_3IN1_TXN");
            sb.Append(" set EFFECT_EDT=@EFFECT_EDT,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where INS_TYPE='C' and EMP_ID=@EMP_ID and IDENTITY_KIND='1' and ");
            sb.Append(" LICENSE_ID=@LICENSE_ID and COMPANY_CD=@COMPANY_CD and EFFECT_EDT='9999/12/31'");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@COMPANY_CD", COMPANY_CD_NEW);
            ht.Add("@EFFECT_EDT", PENSION_CHG_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //異動[TB_I_M_RETIRE_SELFRATE 勞退自提履歷檔 ] //勞退退保
    public void updateRETIRE_SELFRATE(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_I_M_RETIRE_SELFRATE");
            sb.Append(" set EFFECT_EDT=@EFFECT_EDT,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EMP_ID=@EMP_ID and EFFECT_EDT='9999/12/31' ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EFFECT_EDT", PENSION_CHG_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //異動[TB_I_M_GROUP_TXN 團保主檔 ] //團保退保
    public void updateGROUP_TXN(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_I_M_GROUP_TXN");
            sb.Append(" set INS_QUIT_DT=@INS_QUIT_DT,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            //sb.Append(" where EMP_ID=@EMP_ID and LICENSE_ID=@LICENSE_ID and GINS_KIND='A' and INS_QUIT_DT='9999/12/31' ");
            sb.Append(" where EMP_ID=@EMP_ID and INS_QUIT_DT='9999/12/31' ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@INS_QUIT_DT", GROUP_CHG_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料] //退保
    public void insert3IN1_REPORTDATA2(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_R_3IN1_REPORTDATA ( ");
            sb.Append(" CHG_APP_TYPE,COMPANY_CD_NEW,INS_HLR_TYPE,IDENTITY_KIND,SYS_DESC,");
            sb.Append(" LAB_FORIGN_YN,LICENSE_ID,EMP_NAME,EMP_BIRTH_DT,SALARY,HEA_BEF_AMT,");
            sb.Append(" HEA_AFT_AMT,SPTYP,F_LICENCE_CD,F_NAME,F_BIRTH_DT,FAMILY_RELATION,");
            sb.Append(" CHG_TYPE,CHG_REASON_CD,CHG_REASON_CD_DESC,HEA_CHT_DT,INS_SEX,RET_DIFFENCT_TYPE,");
            sb.Append(" BOSS_RATE,SEFT_RATE,RET_DT,DATASOURCE,OP_DT,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" '2',@COMPANY_CD_NEW,'2','1',@SYS_DESC,");
            sb.Append(" @LAB_FORIGN_YN,@LICENSE_ID,@EMP_NAME,@EMP_BIRTH_DT,@SALARY,@HEA_BEF_AMT,");
            sb.Append(" @HEA_AFT_AMT,@SPTYP,'','',@F_BIRTH_DT,'',");
            sb.Append(" @CHG_TYPE,@CHG_REASON_CD,@CHG_REASON_CD_DESC,@HEA_CHT_DT,@INS_SEX,'',");
            sb.Append(" @BOSS_RATE,@SEFT_RATE,@RET_DT,'A',GETDATE(),");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@COMPANY_CD_NEW", COMPANY_CD_NEW);
            ht.Add("@SYS_DESC", EMP_ID);
            ht.Add("@F_BIRTH_DT", DBNull.Value);
            ht.Add("@RET_DT", DBNull.Value);
            //勞保_被保險人外籍
            if (IsTWN == "Y")
            {
                ht.Add("@LAB_FORIGN_YN", "");
                ht.Add("@INS_SEX", "");
            }
            else
            {
                ht.Add("@LAB_FORIGN_YN", "Y");
                // "M" //男  else "F" //女 
                if (INS_SEX == "1")
                    ht.Add("@INS_SEX", "M");
                else
                    ht.Add("@INS_SEX", "F");
            }
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@EMP_BIRTH_DT", EMP_BIRTH_DT);
            ht.Add("@SALARY", BASIC_SALARY);
            ht.Add("@HEA_BEF_AMT", 0);
            ht.Add("@HEA_AFT_AMT", 0);
            //職務區分: PJ60是研修生,PJ50是建教生
            if (PJOB_CD == "PJ50")
                ht.Add("@SPTYP", "T");
            else
                ht.Add("@SPTYP", "");

            ht.Add("@CHG_TYPE", "2");
            ht.Add("@CHG_REASON_CD", "1");
            ht.Add("@CHG_REASON_CD_DESC", "離職");
            if (HEALTH_IS_YN == "Y")
                ht.Add("@HEA_CHT_DT", HEALTH_CHG_DT);
            else
                ht.Add("@HEA_CHT_DT", DBNull.Value);

            ht.Add("@BOSS_RATE", DBNull.Value);
            ht.Add("@SEFT_RATE", DBNull.Value);

            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //異動[TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 ] //勞保身分轉換
    public void update3IN1_TXN_A2(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_I_M_3IN1_TXN");
            sb.Append(" set EFFECT_EDT=@EFFECT_EDT,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where INS_TYPE='A' and EMP_ID=@EMP_ID and IDENTITY_KIND='1' and ");
            sb.Append(" LICENSE_ID=@LICENSE_ID and COMPANY_CD=@COMPANY_CD and EFFECT_EDT='9999/12/31'");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@COMPANY_CD", COMPANY_CD_OLD);
            ht.Add("@EFFECT_EDT", Convert.ToDateTime(LABOR_CHG_DT).AddDays(-1));
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //異動[TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 ] //健保身分轉換
    public void update3IN1_TXN_B2(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_I_M_3IN1_TXN");
            sb.Append(" set CHG_TYPE_OUT=@CHG_TYPE_OUT,CHG_REASON_CD=@CHG_REASON_CD,EFFECT_EDT=@EFFECT_EDT,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where INS_TYPE='B' and EMP_ID=@EMP_ID and ");
            sb.Append(" COMPANY_CD=@COMPANY_CD and EFFECT_EDT='9999/12/31' ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@COMPANY_CD", COMPANY_CD_OLD);
            ht.Add("@CHG_TYPE_OUT", "2");
            ht.Add("@CHG_REASON_CD", "1");
            ht.Add("@EFFECT_EDT", Convert.ToDateTime(HEALTH_CHG_DT).AddDays(-1));
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //加保 //找尋有無眷屬資料,若有須一併加保至新公司別 //健保處理
    public void insert3IN1_TXN_B(string license_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("insert into TB_I_M_3IN1_TXN ( ");
            sb.Append(" INS_TYPE,EMP_ID,IDENTITY_KIND,LICENSE_ID,EFFECT_SDT,EFFECT_EDT,");
            sb.Append(" SALARY_AMT,INS_AMT,COMPANY_CD,CHG_APP_TYPE,CHG_TYPE_IN,");
            sb.Append(" REMARK,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @INS_TYPE,@EMP_ID,@IDENTITY_KIND,@LICENSE_ID,@EFFECT_SDT,'9999/12/31',");
            sb.Append(" @SALARY_AMT,@INS_AMT,@COMPANY_CD,'4',@CHG_TYPE_IN,");
            sb.Append(" '',@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@INS_TYPE", "B");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IDENTITY_KIND", "2");
            ht.Add("@LICENSE_ID", license_id); //眷AT.身份證/居留證
            ht.Add("@EFFECT_SDT", HEALTH_CHG_DT);
            ht.Add("@SALARY_AMT", BASIC_SALARY);
            ht.Add("@INS_AMT", HEALTH_INS_AMT);
            ht.Add("@COMPANY_CD", COMPANY_CD_NEW);
            ht.Add("@CHG_TYPE_IN", "4"); //依附投保              
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //異動[TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 ] //勞退身分轉換
    public void update3IN1_TXN_C2(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_I_M_3IN1_TXN");
            sb.Append(" set EFFECT_EDT=@EFFECT_EDT,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where INS_TYPE='C' and EMP_ID=@EMP_ID and IDENTITY_KIND='1' and ");
            sb.Append(" LICENSE_ID=@LICENSE_ID and COMPANY_CD=@COMPANY_CD and EFFECT_EDT='9999/12/31'");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@COMPANY_CD", COMPANY_CD_OLD);
            ht.Add("@EFFECT_EDT", Convert.ToDateTime(PENSION_CHG_DT).AddDays(-1));
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //異動[TB_I_M_RETIRE_SELFRATE 勞退自提履歷檔 ] //勞退身分轉換
    public void updateRETIRE_SELFRATE2(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_I_M_RETIRE_SELFRATE");
            sb.Append(" set EFFECT_EDT=@EFFECT_EDT,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EMP_ID=@EMP_ID and EFFECT_EDT='9999/12/31' ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EFFECT_EDT", Convert.ToDateTime(PENSION_CHG_DT).AddDays(-1));
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料](身分轉換_退保)
    public void insert3IN1_REPORTDATA5(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("insert into TB_I_R_3IN1_REPORTDATA ( ");
            sb.Append(" CHG_APP_TYPE,COMPANY_CD_NEW,INS_HLR_TYPE,IDENTITY_KIND,SYS_DESC,");
            sb.Append(" LAB_FORIGN_YN,LICENSE_ID,EMP_NAME,EMP_BIRTH_DT,SALARY,HEA_BEF_AMT,");
            sb.Append(" HEA_AFT_AMT,SPTYP,F_LICENCE_CD,F_NAME,F_BIRTH_DT,FAMILY_RELATION,");
            sb.Append(" CHG_TYPE,CHG_REASON_CD,CHG_REASON_CD_DESC,HEA_CHT_DT,INS_SEX,RET_DIFFENCT_TYPE,");
            sb.Append(" BOSS_RATE,SEFT_RATE,RET_DT,DATASOURCE,OP_DT,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" '2',@COMPANY_CD_NEW,'2','1',@SYS_DESC,");
            sb.Append(" @LAB_FORIGN_YN,@LICENSE_ID,@EMP_NAME,@EMP_BIRTH_DT,@SALARY,@HEA_BEF_AMT,");
            sb.Append(" @HEA_AFT_AMT,@SPTYP,'','',@F_BIRTH_DT,'',");
            sb.Append(" '2','1','離職',@HEA_CHT_DT,@INS_SEX,@RET_DIFFENCT_TYPE,");
            sb.Append(" @BOSS_RATE,@SEFT_RATE,@RET_DT,'A',GETDATE(),");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@COMPANY_CD_NEW", COMPANY_CD_OLD);
            ht.Add("@SYS_DESC", EMP_ID);
            ht.Add("@F_BIRTH_DT", DBNull.Value);

            //勞保_被保險人外籍
            if (IsTWN == "Y")
            {
                ht.Add("@LAB_FORIGN_YN", "");
                ht.Add("@INS_SEX", "");
            }
            else
            {
                ht.Add("@LAB_FORIGN_YN", "Y");
                // "M" //男  else "F" //女 
                if (INS_SEX == "1")
                    ht.Add("@INS_SEX", "M");
                else
                    ht.Add("@INS_SEX", "F");
            }

            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@EMP_BIRTH_DT", EMP_BIRTH_DT);
            ht.Add("@SALARY", BASIC_SALARY);
            ht.Add("@HEA_BEF_AMT", 0);
            ht.Add("@HEA_AFT_AMT", 0);

            //健保處理
            if (HEALTH_IS_YN == "Y")
            {
                ht.Add("@HEA_CHT_DT", Convert.ToDateTime(HEALTH_CHG_DT).AddDays(-1).ToString("yyyy/MM/dd"));
            }
            else
            {
                ht.Add("@HEA_CHT_DT", DBNull.Value);
            }
            //職務區分: PJ60是研修生,PJ50是建教生
            if (PJOB_CD == "PJ50")
                ht.Add("@SPTYP", "T");
            else
                ht.Add("@SPTYP", "");

            //勞退處理
            ht.Add("@RET_DIFFENCT_TYPE", "");
            ht.Add("@BOSS_RATE", 0);
            ht.Add("@RET_DT", DBNull.Value);
            ht.Add("@SEFT_RATE", 0);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料] //身分轉換
    public void insert3IN1_REPORTDATA3(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_R_3IN1_REPORTDATA ( ");
            sb.Append(" CHG_APP_TYPE,COMPANY_CD_NEW,INS_HLR_TYPE,IDENTITY_KIND,SYS_DESC,");
            sb.Append(" LAB_FORIGN_YN,LICENSE_ID,EMP_NAME,EMP_BIRTH_DT,SALARY,HEA_BEF_AMT,");
            sb.Append(" HEA_AFT_AMT,SPTYP,F_LICENCE_CD,F_NAME,F_BIRTH_DT,FAMILY_RELATION,");
            sb.Append(" CHG_TYPE,CHG_REASON_CD,CHG_REASON_CD_DESC,HEA_CHT_DT,INS_SEX,RET_DIFFENCT_TYPE,");
            sb.Append(" BOSS_RATE,SEFT_RATE,RET_DT,DATASOURCE,OP_DT,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" '4',@COMPANY_CD_NEW,'2','1',@SYS_DESC,");
            sb.Append(" @LAB_FORIGN_YN,@LICENSE_ID,@EMP_NAME,@EMP_BIRTH_DT,@SALARY,@HEA_BEF_AMT,");
            sb.Append(" @HEA_AFT_AMT,@SPTYP,'','',@F_BIRTH_DT,'',");
            sb.Append(" @CHG_TYPE,'','',@HEA_CHT_DT,@INS_SEX,@RET_DIFFENCT_TYPE,");
            sb.Append(" @BOSS_RATE,@SEFT_RATE,@RET_DT,'A',GETDATE(),");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@COMPANY_CD_NEW", COMPANY_CD_NEW);
            ht.Add("@SYS_DESC", EMP_ID);
            ht.Add("@F_BIRTH_DT", DBNull.Value);
            //勞保_被保險人外籍
            if (IsTWN == "Y")
            {
                ht.Add("@LAB_FORIGN_YN", "");
                ht.Add("@INS_SEX", "");
            }
            else
            {
                ht.Add("@LAB_FORIGN_YN", "Y");
                // "M" //男  else "F" //女 
                if (INS_SEX == "1")
                    ht.Add("@INS_SEX", "M");
                else
                    ht.Add("@INS_SEX", "F");
            }

            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@EMP_BIRTH_DT", EMP_BIRTH_DT);
            ht.Add("@SALARY", BASIC_SALARY);
            //健保處理
            if (HEALTH_IS_YN == "Y")
            {
                ht.Add("@HEA_BEF_AMT", HEALTH_INS_AMT);
                ht.Add("@HEA_AFT_AMT", HEALTH_INS_AMT);
                ht.Add("@CHG_TYPE", "1"); //"1"到職起薪
                ht.Add("@HEA_CHT_DT", HEALTH_CHG_DT);
            }
            else
            {
                ht.Add("@HEA_BEF_AMT", 0);
                ht.Add("@HEA_AFT_AMT", 0);
                ht.Add("@CHG_TYPE", "");
                ht.Add("@HEA_CHT_DT", DBNull.Value);
            }
            //職務區分: PJ60是研修生,PJ50是建教生
            if (PJOB_CD == "PJ50")
                ht.Add("@SPTYP", "T");
            else
                ht.Add("@SPTYP", "");

            //勞退處理
            if (PENSION_IS_YN == "Y")
            {
                //勞退提繳身份別
                if (IS_MASTER == "Y")
                    ht.Add("@RET_DIFFENCT_TYPE", "3"); //顧主自願提繳
                else
                    ht.Add("@RET_DIFFENCT_TYPE", "1"); //強制提繳

                //雇主提撥率% 
                ht.Add("@BOSS_RATE", INSC_COMP_RATE);

                //勞退提繳日期 
                if (LABOR_CHG_DT != PENSION_CHG_DT)
                    ht.Add("@RET_DT", PENSION_CHG_DT);
                else
                    ht.Add("@RET_DT", DBNull.Value);
            }
            else
            {
                ht.Add("@RET_DIFFENCT_TYPE", "");
                ht.Add("@BOSS_RATE", 0);
                ht.Add("@RET_DT", DBNull.Value);
            }
            ht.Add("@SEFT_RATE", PENSION_SELF_RATIO);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料] //身分轉換
    public void insert3IN1_REPORTDATA4(string f_licence_cd, string arr_FAMILY)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_R_3IN1_REPORTDATA ( ");
            sb.Append(" CHG_APP_TYPE,COMPANY_CD_NEW,INS_HLR_TYPE,IDENTITY_KIND,SYS_DESC,");
            sb.Append(" LAB_FORIGN_YN,LICENSE_ID,EMP_NAME,EMP_BIRTH_DT,SALARY,HEA_BEF_AMT,");
            sb.Append(" HEA_AFT_AMT,SPTYP,F_LICENCE_CD,F_NAME,F_BIRTH_DT,FAMILY_RELATION,");
            sb.Append(" CHG_TYPE,CHG_REASON_CD,CHG_REASON_CD_DESC,HEA_CHT_DT,INS_SEX,RET_DIFFENCT_TYPE,");
            sb.Append(" BOSS_RATE,SEFT_RATE,RET_DT,DATASOURCE,OP_DT,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" '4',@COMPANY_CD_NEW,'3','2',@SYS_DESC,");
            sb.Append(" @LAB_FORIGN_YN,@LICENSE_ID,@EMP_NAME,@EMP_BIRTH_DT,@SALARY,@HEA_BEF_AMT,");
            sb.Append(" @HEA_AFT_AMT,@SPTYP,@F_LICENCE_CD,@F_NAME,@F_BIRTH_DT,@FAMILY_RELATION,");
            sb.Append(" @CHG_TYPE,'','',@HEA_CHT_DT,@INS_SEX,'',");
            sb.Append(" @BOSS_RATE,@SEFT_RATE,@RET_DT,'A',GETDATE(),");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@COMPANY_CD_NEW", COMPANY_CD_NEW);
            ht.Add("@SYS_DESC", EMP_ID + "身份轉換一併處理");
            //勞保_被保險人外籍
            if (IsTWN == "Y")
            {
                ht.Add("@LAB_FORIGN_YN", "");
                ht.Add("@INS_SEX", "");
            }
            else
            {
                ht.Add("@LAB_FORIGN_YN", "Y");
                // "M" //男  else "F" //女 
                if (INS_SEX == "1")
                    ht.Add("@INS_SEX", "M");
                else
                    ht.Add("@INS_SEX", "F");
            }

            ht.Add("@BOSS_RATE", 0);
            ht.Add("@SEFT_RATE", 0);

            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@EMP_BIRTH_DT", EMP_BIRTH_DT);
            ht.Add("@SALARY", BASIC_SALARY);
            //健保處理
            if (HEALTH_IS_YN == "Y")
            {
                ht.Add("@HEA_BEF_AMT", HEALTH_INS_AMT);
                ht.Add("@HEA_AFT_AMT", HEALTH_INS_AMT);
                ht.Add("@CHG_TYPE", "4"); //"4"依附投保
                ht.Add("@HEA_CHT_DT", HEALTH_CHG_DT);
            }
            else
            {
                ht.Add("@HEA_BEF_AMT", 0);
                ht.Add("@HEA_AFT_AMT", 0);
                ht.Add("@CHG_TYPE", "");
                ht.Add("@HEA_CHT_DT", DBNull.Value);
            }
            //職務區分: PJ60是研修生,PJ50是建教生
            if (PJOB_CD == "PJ50")
                ht.Add("@SPTYP", "T");
            else
                ht.Add("@SPTYP", "");

            ht.Add("@F_LICENCE_CD", f_licence_cd); //眷屬身份證字號
            //取得眷屬姓名、眷屬出生日期和稱謂
            ht.Add("@F_NAME", arr_FAMILY.Split('#')[0]);
            if (arr_FAMILY.Split('#')[1] == "")
                ht.Add("@F_BIRTH_DT", DBNull.Value);
            else
                ht.Add("@F_BIRTH_DT", arr_FAMILY.Split('#')[1]);
            ht.Add("@FAMILY_RELATION", arr_FAMILY.Split('#')[2]);

            ht.Add("@RET_DT", DBNull.Value);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得現有LICENSE_ID資料
    public DataTable getLICENSE_ID()
    {
        try
        {
            //本人身份轉換時,眷屬仍在保的找出																		
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select LICENSE_ID from TB_I_M_3IN1_TXN");
            sb.Append(" where INS_TYPE='B' and EMP_ID=@EMP_ID and IDENTITY_KIND='2' and EFFECT_EDT='9999/12/31' ");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);

        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //取得眷屬姓名、眷屬出生日期和稱謂
    public string getFAMILY(string license_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string result = "";
            sb.Append("select FAMILY_NAME,FAMILY_BIRTH_DT,FAMILY_RELATION from TB_H_M_EMP_FAMILY");
            sb.Append(" where EMP_ID=@EMP_ID and FAMILY_LICENSE_ID=@FAMILY_LICENSE_ID");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FAMILY_LICENSE_ID", license_id);

            DataTable dt = dbConn.QueryT(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["FAMILY_NAME"].ToString() + "#" +
                    Convert.ToDateTime(dt.Rows[0]["FAMILY_BIRTH_DT"]).ToString("yyyy/MM/dd") + "#" +
                    dt.Rows[0]["FAMILY_RELATION"].ToString();
            }
            else
                result = "##";

            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //新增[TB_I_M_PERSONDATA 保險資料主檔]
    public void insertPERSONDATA(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_M_PERSONDATA ( ");
            sb.Append(" EMP_ID,LICENSE_ID_FIRST,LICENSE_ID,EMP_NAME,BIRTH_DT,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @EMP_ID,@LICENSE_ID_FIRST,@LICENSE_ID,@EMP_NAME,@BIRTH_DT,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID_FIRST", LICENSE_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@BIRTH_DT", EMP_BIRTH_DT);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //新增[TB_I_R_DATAUPDAE_HIS 保險資料更新歷史檔]
    public void insertDATAUPDAE_HIS(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_R_DATAUPDAE_HIS ( ");
            sb.Append(" EMP_ID,LICENSE_ID,CREATED_DT,LICENSE_ID_FIRST,EMP_NAME,BIRTH_DT,CREATED_BY,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @EMP_ID,@LICENSE_ID,GETDATE(),@LICENSE_ID_FIRST,@EMP_NAME,@BIRTH_DT,");
            sb.Append(" @CREATED_BY,@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@LICENSE_ID_FIRST", LICENSE_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@BIRTH_DT", EMP_BIRTH_DT);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //是否找到[TB_I_M_PERSONDATA 保險資料主檔]的資料
    public bool isPERSONDATA(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID from TB_I_M_PERSONDATA");
            sb.Append(" where EMP_ID=@EMP_ID and LICENSE_ID=@LICENSE_ID");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);

            DataTable dt = dbConn.QueryT(sb, ht);
            if (dt.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除[TB_I_M_CHG_TXN 保險一括加退保檔]
    public void deleteCHG_TXN(string emp_id, string company_cd_new, string license_id, string chg_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_I_M_CHG_TXN ");
            sb.Append(" where EMP_ID = @EMP_ID and COMPANY_CD_NEW = @COMPANY_CD_NEW ");
            sb.Append(" and LICENSE_ID = @LICENSE_ID and CONVERT(char(10),CHG_DT, 111) = @CHG_DT");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@COMPANY_CD_NEW", company_cd_new);
            ht.Add("@LICENSE_ID", license_id);
            ht.Add("@CHG_DT", chg_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //異動別
    public DataTable getHR_CHG_DESC(string hr_chg_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select HR_CHG_CD,HR_CHG_DESC ");
            sb.Append(" from TB_H_M_HR_CHANGE_CODE ");
            sb.Append(" where HR_CHG_CD=@HR_CHG_CD ");
            ht.Add("@HR_CHG_CD", hr_chg_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //公司別
    public DataTable getCOMPANY_SNAME(string company_cd_old)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COMPANY_CD,COMPANY_SNAME ");
            sb.Append(" from TB_H_M_COMPANY ");
            sb.Append(" where COMPANY_CD=@COMPANY_CD ");
            ht.Add("@COMPANY_CD", company_cd_old);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //工號
    public DataTable getEmpName(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID,EMP_NAME ");
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

    //國籍代號
    public DataTable getNATION_Name(string nation_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SUB_CD,SUB_DESC ");
            sb.Append(" from TB_9_M_COMM_D ");
            sb.Append(" where SYS_CD = 'HB' and MAIN_CD = 'NATION_CD' and SUB_CD=@SUB_CD ");
            ht.Add("@SUB_CD", nation_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得勞退自提率
    public DataTable getPENSION_SELF_RATIO(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID,SLEF_RATE ");
            sb.Append(" from TB_I_M_RETIRE_SELFRATE ");
            sb.Append(" where EMP_ID=@EMP_ID and EFFECT_EDT='9999/12/31' ");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.QueryT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //找尋眷屬資料
    public DataTable getlicense_id()
    {
        throw new NotImplementedException();
    }

    public DataTable getLEVEL_CHG_Count(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(*) row ");
            sb.Append(" from TB_I_M_LEVEL_CHG ");
            sb.Append(" where EMP_ID=@EMP_ID and isnull(EFFECT_DT,'') ='' ");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getLEVEL_CHG(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select A_NEW_INSAMT,B_NEW_INSAMT,C_NEW_INSAMT,AVG_SALARY ");
            sb.Append(" from TB_I_M_LEVEL_CHG ");
            sb.Append(" where EMP_ID=@EMP_ID and isnull(EFFECT_DT,'') ='' ");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //刪除保險薪調記錄檔
    public DataTable delLEVEL_CHG(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_I_M_LEVEL_CHG ");            
            sb.Append(" where EMP_ID=@EMP_ID and isnull(EFFECT_DT,'') ='' ");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.QueryT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

}