using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;
using ACESLib;

/// <summary>
/// CFB2HC0100DAO 的摘要描述
/// </summary>
public class CFB2HC0100DAO : BaseDAO
{
    public ArrayList HR_CHG_NO { get; set; }
    public string HR_CHG_NO_for_Update { get; set; }
    public string HR_CHG_CD { get; set; }
    public string HR_CHG_DESC { get; set; }
    public string EMP_ID { get; set; }
    public List<string> EMP_IDs { get; set; }
    public string EMP_NAME { get; set; }
    public string START_DT { get; set; }
    public ArrayList CHG_SEQ { get; set; }
    public string INS_PLAN_PROC_DT { get; set; }
    public string PLAN_END_DT { get; set; }
    //public string END_HR_CHG_NO { get; set; }
    public string IS_END { get; set; }
    public string MAIN_HR_CHG_NO { get; set; }
    public List<string> MAIN_HR_CHG_NOs { get; set; }
    public string ICT_TYPE { get; set; }
    public string TRANSFER_NATION_CD { get; set; }
    public string TRANSFER_COMPANY_CD { get; set; }
    public string TRANSFER_DEPT { get; set; }
    public string IS_PAY_SUBSIST { get; set; }
    //ORI_WS_CD
    //ORI_COMPANY_CD
    //ORI_PLANT_CD
    //ORI_DEPT_NO
    //ORI_DEPT_NAME
    //ORI_DEPT_FULL_NAME
    //ORI_DIV_DEPT_FULL_NAME
    //ORI_DEPT_NAME_20
    //ORI_DEPT_NAME_30
    //ORI_DEPT_NAME_40
    //ORI_DEPT_NAME_50
    //ORI_DEPT_NAME_60
    //ORI_DEPT_NAME_70
    //ORI_EMP_CD
    //ORI_LEVEL_CD
    //ORI_GRADE_CD
    //ORI_PJOB_CD
    //ORI_PJOB_DESC
    //ORI_WORK_SHIFT_CD
    //ORI_WORK_CD
    public string HR_CHG_PROC_STATUS { get; set; }
    public string HR_CHG_PROC_STATUS_DESC { get; set; }
    //public string HR_CHG_PROC_LOG { get; set; }
    //public string HR_CHG_PROC_DT { get; set; }
    public string INS_CHG_PROC_STATUS { get; set; }
    //public string INS_CHG_PROC_LOG { get; set; }
    //public string INS_CHG_PROC_DT { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    public DataTable gv_result { get; set; }
    public DataTable gv_result2 { get; set; }

    private bool strIsSuper = false;
    private string strIsDEPT = "";
    private string strDepartments = "";
    private string strSysCodeAtt = "";
    public string SYSCODEATT { get; set; }
    public CFB2HC0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
        //取得小分類的權限
        getAuth(ref strIsSuper, ref strIsDEPT, ref strDepartments, ref strSysCodeAtt);
        SYSCODEATT = strSysCodeAtt;
    }

    //新增儲存
    public void WFB2HC0100_Add_Save()
    {
        try
        {

            //兼任
            if (HR_CHG_CD == "B06")
            {
                //新增主檔
                for (int i = 0; i < HR_CHG_NO.Count; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    Hashtable ht = new Hashtable();
                    sb.AppendLine(@"insert into TB_H_M_EMP_HR_CHANGE_H (
                                    HR_CHG_NO,
                                    HR_CHG_CD,
                                    EMP_ID,
                                    START_DT,
                                    CHG_SEQ,
                                    INS_PLAN_PROC_DT,
                                    PLAN_END_DT,
                                    --END_HR_CHG_NO,
                                    IS_END,
                                    MAIN_HR_CHG_NO,
                                    ICT_TYPE,
                                    TRANSFER_NATION_CD,
                                    TRANSFER_COMPANY_CD,
                                    TRANSFER_DEPT,
                                    IS_PAY_SUBSIST,
                                    ORI_WS_CD,
                                    ORI_COMPANY_CD,
                                    ORI_PLANT_CD,
                                    ORI_DEPT_NO,
                                    ORI_DEPT_NAME,
                                    ORI_DEPT_FULL_NAME,
                                    ORI_DIV_DEPT_FULL_NAME,
                                    ORI_DEPT_NAME_20,
                                    ORI_DEPT_NAME_30,
                                    ORI_DEPT_NAME_40,
                                    ORI_DEPT_NAME_50,
                                    ORI_DEPT_NAME_60,
                                    ORI_DEPT_NAME_70,
                                    ORI_EMP_CD,
                                    ORI_LEVEL_CD,
                                    ORI_GRADE_CD,
                                    ORI_PJOB_CD,
                                    ORI_PJOB_DESC,
                                    ORI_WORK_SHIFT_CD,
                                    ORI_WORK_CD,
                                    HR_CHG_PROC_STATUS,
                                    --HR_CHG_PROC_LOG,
                                    --HR_CHG_PROC_DT,
                                    INS_CHG_PROC_STATUS,
                                    --INS_CHG_PROC_LOG,
                                    --INS_CHG_PROC_DT,
                                    CREATED_BY,
                                    CREATED_DT,
                                    UPDATED_BY,
                                    UPDATED_DT,
                                    FUNC_ID) 
                                        select 
                                            @HR_CHG_NO,
                                            @HR_CHG_CD,
                                            @EMP_ID,
                                            @START_DT,
                                            @CHG_SEQ,
                                            @INS_PLAN_PROC_DT,
                                            (case when @PLAN_END_DT = '' then null else @PLAN_END_DT end),
                                            --END_HR_CHG_NO,
                                            @IS_END,
                                            @MAIN_HR_CHG_NO,
                                            @ICT_TYPE,
                                            @TRANSFER_NATION_CD,
                                            @TRANSFER_COMPANY_CD,
                                            @TRANSFER_DEPT,
                                            @IS_PAY_SUBSIST,
                                            E.WS_CD,
                                            E.COMPANY_CD,
                                            E.PLANT_CD,
                                            E.DEPT_NO,
                                            E.DEPT_NAME,
                                            E.DEPT_FULL_NAME,
                                            E.DIV_DEPT_FULL_NAME,
                                            E.DEPT_NAME_20,
                                            E.DEPT_NAME_30,
                                            E.DEPT_NAME_40,
                                            E.DEPT_NAME_50,
                                            E.DEPT_NAME_60,
                                            E.DEPT_NAME_70,
                                            E.EMP_CD,
                                            E.LEVEL_CD,
                                            E.GRADE_CD,
                                            E.PJOB_CD,
                                            E.PJOB_DESC,
                                            E.WORK_SHIFT_CD,
                                            E.WORK_CD,
                                            @HR_CHG_PROC_STATUS,
                                            --HR_CHG_PROC_LOG,
                                            --HR_CHG_PROC_DT,
                                            @INS_CHG_PROC_STATUS,
                                            --INS_CHG_PROC_LOG,
                                            --INS_CHG_PROC_DT,
                                            @CREATED_BY,
                                            getdate(),
                                            @UPDATED_BY,
                                            getdate(),
                                            @FUNC_ID
                                        from VW_H_EMP_DATA E
                                        where E.EMP_ID = @EMP_ID ");
                    ht.Add("@HR_CHG_NO", (string)HR_CHG_NO[i]);
                    ht.Add("@HR_CHG_CD", HR_CHG_CD);
                    ht.Add("@EMP_ID", EMP_ID);
                    ht.Add("@START_DT", START_DT);
                    ht.Add("@CHG_SEQ", (int)CHG_SEQ[i]);
                    ht.Add("@INS_PLAN_PROC_DT", INS_PLAN_PROC_DT);
                    ht.Add("@PLAN_END_DT", PLAN_END_DT);
                    //ht.Add("@END_HR_CHG_NO", END_HR_CHG_NO);
                    ht.Add("@IS_END", IS_END);
                    ht.Add("@MAIN_HR_CHG_NO", MAIN_HR_CHG_NO);
                    ht.Add("@ICT_TYPE", ICT_TYPE);
                    ht.Add("@TRANSFER_NATION_CD", TRANSFER_NATION_CD);
                    ht.Add("@TRANSFER_COMPANY_CD", TRANSFER_COMPANY_CD);
                    ht.Add("@TRANSFER_DEPT", TRANSFER_DEPT);
                    ht.Add("@IS_PAY_SUBSIST", IS_PAY_SUBSIST);
                    ht.Add("@HR_CHG_PROC_STATUS", HR_CHG_PROC_STATUS);
                    ht.Add("@INS_CHG_PROC_STATUS", INS_CHG_PROC_STATUS);
                    ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
                    ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
                    ht.Add("@FUNC_ID", "FB2HC010");
                    dbConn.ExecuteT(sb, ht);
                }
                //新增明細檔
                for (int i = 0; i < gv_result.Rows.Count; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    Hashtable ht = new Hashtable();
                    sb.AppendLine(@"insert into TB_H_M_EMP_HR_CHANGE_D (
                                    HR_CHG_NO,
                                    EMP_ID,
                                    HR_CHG_ITEM,                                    
                                    AFTER_CD,
                                    AFTER_DESC,
                                    CREATED_BY,
                                    CREATED_DT,
                                    UPDATED_BY,
                                    UPDATED_DT,
                                    FUNC_ID) values (
                                    @HR_CHG_NO,
                                    @EMP_ID,
                                    @HR_CHG_ITEM,                                    
                                    @AFTER_CD,
                                    @AFTER_DESC,
                                    @CREATED_BY,
                                    getdate(),
                                    @UPDATED_BY,
                                    getdate(),
                                    @FUNC_ID
                                ) ");
                    ht.Add("@HR_CHG_NO", (string)HR_CHG_NO[i]);
                    ht.Add("@EMP_ID", EMP_ID);
                    ht.Add("@HR_CHG_ITEM", "05");
                    ht.Add("@AFTER_CD", gv_result.Rows[i]["DEPT_NO"].ToString().ToUpper());
                    ht.Add("@AFTER_DESC", gv_result.Rows[i]["DEPT_NAME"].ToString());
                    ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
                    ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
                    ht.Add("@FUNC_ID", "FB2HC010");
                    dbConn.ExecuteT(sb, ht);

                    sb = new StringBuilder();
                    ht = new Hashtable();
                    sb.AppendLine(@"insert into TB_H_M_EMP_HR_CHANGE_D (
                                    HR_CHG_NO,
                                    EMP_ID,
                                    HR_CHG_ITEM,                                    
                                    AFTER_CD,
                                    AFTER_DESC,
                                    CREATED_BY,
                                    CREATED_DT,
                                    UPDATED_BY,
                                    UPDATED_DT,
                                    FUNC_ID) values (
                                    @HR_CHG_NO,
                                    @EMP_ID,
                                    @HR_CHG_ITEM,                                    
                                    @AFTER_CD,
                                    @AFTER_DESC,
                                    @CREATED_BY,
                                    getdate(),
                                    @UPDATED_BY,
                                    getdate(),
                                    @FUNC_ID
                                ) ");
                    ht.Add("@HR_CHG_NO", (string)HR_CHG_NO[i]);
                    ht.Add("@EMP_ID", EMP_ID);
                    ht.Add("@HR_CHG_ITEM", "08");
                    ht.Add("@AFTER_CD", gv_result.Rows[i]["PJOB_CD"].ToString().ToUpper());
                    ht.Add("@AFTER_DESC", gv_result.Rows[i]["PJOB_DESC"].ToString());
                    ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
                    ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
                    ht.Add("@FUNC_ID", "FB2HC010");
                    dbConn.ExecuteT(sb, ht);
                }
            }
            //非兼任 & D04(結束兼任) 
            else
            {
                StringBuilder sb = new StringBuilder();
                Hashtable ht = new Hashtable();
                //新增主檔
                sb.AppendLine(@"insert into TB_H_M_EMP_HR_CHANGE_H (
                                    HR_CHG_NO,
                                    HR_CHG_CD,
                                    EMP_ID,
                                    START_DT,
                                    CHG_SEQ,
                                    INS_PLAN_PROC_DT,
                                    PLAN_END_DT,
                                    --END_HR_CHG_NO,
                                    IS_END,
                                    MAIN_HR_CHG_NO,
                                    ICT_TYPE,
                                    TRANSFER_NATION_CD,
                                    TRANSFER_COMPANY_CD,
                                    TRANSFER_DEPT,
                                    IS_PAY_SUBSIST,
                                    ORI_WS_CD,
                                    ORI_COMPANY_CD,
                                    ORI_PLANT_CD,
                                    ORI_DEPT_NO,
                                    ORI_DEPT_NAME,
                                    ORI_DEPT_FULL_NAME,
                                    ORI_DIV_DEPT_FULL_NAME,
                                    ORI_DEPT_NAME_20,
                                    ORI_DEPT_NAME_30,
                                    ORI_DEPT_NAME_40,
                                    ORI_DEPT_NAME_50,
                                    ORI_DEPT_NAME_60,
                                    ORI_DEPT_NAME_70,
                                    ORI_EMP_CD,
                                    ORI_LEVEL_CD,
                                    ORI_GRADE_CD,
                                    ORI_PJOB_CD,
                                    ORI_PJOB_DESC,
                                    ORI_WORK_SHIFT_CD,
                                    ORI_WORK_CD,
                                    HR_CHG_PROC_STATUS,
                                    --HR_CHG_PROC_LOG,
                                    --HR_CHG_PROC_DT,
                                    INS_CHG_PROC_STATUS,
                                    --INS_CHG_PROC_LOG,
                                    --INS_CHG_PROC_DT,
                                    CREATED_BY,
                                    CREATED_DT,
                                    UPDATED_BY,
                                    UPDATED_DT,
                                    FUNC_ID) 
                                        select 
                                            @HR_CHG_NO,
                                            @HR_CHG_CD,
                                            @EMP_ID,
                                            @START_DT,
                                            @CHG_SEQ,
                                            @INS_PLAN_PROC_DT,
                                            (case when @PLAN_END_DT = '' then null else @PLAN_END_DT end),
                                            --END_HR_CHG_NO,
                                            @IS_END,
                                            @MAIN_HR_CHG_NO,
                                            @ICT_TYPE,
                                            @TRANSFER_NATION_CD,
                                            @TRANSFER_COMPANY_CD,
                                            @TRANSFER_DEPT,
                                            @IS_PAY_SUBSIST,
                                            E.WS_CD,
                                            E.COMPANY_CD,
                                            E.PLANT_CD,
                                            E.DEPT_NO,
                                            E.DEPT_NAME,
                                            E.DEPT_FULL_NAME,
                                            E.DIV_DEPT_FULL_NAME,
                                            E.DEPT_NAME_20,
                                            E.DEPT_NAME_30,
                                            E.DEPT_NAME_40,
                                            E.DEPT_NAME_50,
                                            E.DEPT_NAME_60,
                                            E.DEPT_NAME_70,
                                            E.EMP_CD,
                                            E.LEVEL_CD,
                                            E.GRADE_CD,
                                            E.PJOB_CD,
                                            E.PJOB_DESC,
                                            E.WORK_SHIFT_CD,
                                            E.WORK_CD,
                                            @HR_CHG_PROC_STATUS,
                                            --HR_CHG_PROC_LOG,
                                            --HR_CHG_PROC_DT,
                                            @INS_CHG_PROC_STATUS,
                                            --INS_CHG_PROC_LOG,
                                            --INS_CHG_PROC_DT,
                                            @CREATED_BY,
                                            getdate(),
                                            @UPDATED_BY,
                                            getdate(),
                                            @FUNC_ID
                                        from VW_H_EMP_DATA E
                                        where E.EMP_ID = @EMP_ID ");
                ht.Add("@HR_CHG_NO", (string)HR_CHG_NO[0]);
                ht.Add("@HR_CHG_CD", HR_CHG_CD);
                ht.Add("@EMP_ID", EMP_ID);
                ht.Add("@START_DT", START_DT);
                ht.Add("@CHG_SEQ", (int)CHG_SEQ[0]);
                ht.Add("@INS_PLAN_PROC_DT", INS_PLAN_PROC_DT);
                ht.Add("@PLAN_END_DT", PLAN_END_DT);
                ht.Add("@IS_END", IS_END);
                ht.Add("@MAIN_HR_CHG_NO", MAIN_HR_CHG_NO);
                ht.Add("@ICT_TYPE", ICT_TYPE);
                ht.Add("@TRANSFER_NATION_CD", TRANSFER_NATION_CD);
                ht.Add("@TRANSFER_COMPANY_CD", TRANSFER_COMPANY_CD);
                ht.Add("@TRANSFER_DEPT", TRANSFER_DEPT);
                ht.Add("@IS_PAY_SUBSIST", IS_PAY_SUBSIST);
                ht.Add("@HR_CHG_PROC_STATUS", HR_CHG_PROC_STATUS);
                ht.Add("@INS_CHG_PROC_STATUS", INS_CHG_PROC_STATUS);
                ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
                ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
                ht.Add("@FUNC_ID", "FB2HC010");
                dbConn.ExecuteT(sb, ht);

                //新增明細檔
                for (int i = 0; i < gv_result2.Rows.Count; i++)
                {
                    sb = new StringBuilder();
                    ht = new Hashtable();
                    sb.AppendLine(@"insert into TB_H_M_EMP_HR_CHANGE_D (
                                    HR_CHG_NO,
                                    EMP_ID,
                                    HR_CHG_ITEM,
                                    BEFORE_CD,
                                    BEFORE_DESC,
                                    AFTER_CD,
                                    AFTER_DESC,
                                    CREATED_BY,
                                    CREATED_DT,
                                    UPDATED_BY,
                                    UPDATED_DT,
                                    FUNC_ID) values (
                                    @HR_CHG_NO,
                                    @EMP_ID,
                                    @HR_CHG_ITEM,
                                    @BEFORE_CD,
                                    @BEFORE_DESC,
                                    @AFTER_CD,
                                    @AFTER_DESC,
                                    @CREATED_BY,
                                    getdate(),
                                    @UPDATED_BY,
                                    getdate(),
                                    @FUNC_ID
                                ) ");
                    ht.Add("@HR_CHG_NO", (string)HR_CHG_NO[0]);
                    ht.Add("@EMP_ID", EMP_ID);
                    ht.Add("@HR_CHG_ITEM", gv_result2.Rows[i]["HR_CHG_ITEM"].ToString());
                    ht.Add("@BEFORE_CD", gv_result2.Rows[i]["BEFORE_CD"].ToString());
                    ht.Add("@BEFORE_DESC", gv_result2.Rows[i]["BEFORE_DESC"].ToString());
                    ht.Add("@AFTER_CD", gv_result2.Rows[i]["AFTER_CD"].ToString().ToUpper());
                    ht.Add("@AFTER_DESC", gv_result2.Rows[i]["AFTER_DESC"].ToString());
                    ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
                    ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
                    ht.Add("@FUNC_ID", "FB2HC010");
                    dbConn.ExecuteT(sb, ht);
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    //修改儲存
    public void WFB2HC0100_Update_Save()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        int j = 0;
        //兼任
        if (HR_CHG_CD == "B06")
        {
            //新增主檔
            for (int i = 0; i < HR_CHG_NO.Count; i++)
            {
                sb = new StringBuilder();
                ht = new Hashtable();
                sb.AppendLine(@"insert into TB_H_M_EMP_HR_CHANGE_H (
                                    HR_CHG_NO,
                                    HR_CHG_CD,
                                    EMP_ID,
                                    START_DT,
                                    CHG_SEQ,
                                    INS_PLAN_PROC_DT,
                                    PLAN_END_DT,
                                    --END_HR_CHG_NO,
                                    IS_END,
                                    MAIN_HR_CHG_NO,
                                    ICT_TYPE,
                                    TRANSFER_NATION_CD,
                                    TRANSFER_COMPANY_CD,
                                    TRANSFER_DEPT,
                                    IS_PAY_SUBSIST,
                                    ORI_WS_CD,
                                    ORI_COMPANY_CD,
                                    ORI_PLANT_CD,
                                    ORI_DEPT_NO,
                                    ORI_DEPT_NAME,
                                    ORI_DEPT_FULL_NAME,
                                    ORI_DIV_DEPT_FULL_NAME,
                                    ORI_DEPT_NAME_20,
                                    ORI_DEPT_NAME_30,
                                    ORI_DEPT_NAME_40,
                                    ORI_DEPT_NAME_50,
                                    ORI_DEPT_NAME_60,
                                    ORI_DEPT_NAME_70,
                                    ORI_EMP_CD,
                                    ORI_LEVEL_CD,
                                    ORI_GRADE_CD,
                                    ORI_PJOB_CD,
                                    ORI_PJOB_DESC,
                                    ORI_WORK_SHIFT_CD,
                                    ORI_WORK_CD,
                                    HR_CHG_PROC_STATUS,
                                    --HR_CHG_PROC_LOG,
                                    --HR_CHG_PROC_DT,
                                    INS_CHG_PROC_STATUS,
                                    --INS_CHG_PROC_LOG,
                                    --INS_CHG_PROC_DT,
                                    CREATED_BY,
                                    CREATED_DT,
                                    UPDATED_BY,
                                    UPDATED_DT,
                                    FUNC_ID) 
                                        select 
                                            @HR_CHG_NO,
                                            @HR_CHG_CD,
                                            @EMP_ID,
                                            @START_DT,
                                            @CHG_SEQ,
                                            @INS_PLAN_PROC_DT,
                                            (case when @PLAN_END_DT = '' then null else @PLAN_END_DT end),
                                            --END_HR_CHG_NO,
                                            @IS_END,
                                            @MAIN_HR_CHG_NO,
                                            @ICT_TYPE,
                                            @TRANSFER_NATION_CD,
                                            @TRANSFER_COMPANY_CD,
                                            @TRANSFER_DEPT,
                                            @IS_PAY_SUBSIST,
                                            E.WS_CD,
                                            E.COMPANY_CD,
                                            E.PLANT_CD,
                                            E.DEPT_NO,
                                            E.DEPT_NAME,
                                            E.DEPT_FULL_NAME,
                                            E.DIV_DEPT_FULL_NAME,
                                            E.DEPT_NAME_20,
                                            E.DEPT_NAME_30,
                                            E.DEPT_NAME_40,
                                            E.DEPT_NAME_50,
                                            E.DEPT_NAME_60,
                                            E.DEPT_NAME_70,
                                            E.EMP_CD,
                                            E.LEVEL_CD,
                                            E.GRADE_CD,
                                            E.PJOB_CD,
                                            E.PJOB_DESC,
                                            E.WORK_SHIFT_CD,
                                            E.WORK_CD,
                                            @HR_CHG_PROC_STATUS,
                                            --HR_CHG_PROC_LOG,
                                            --HR_CHG_PROC_DT,
                                            @INS_CHG_PROC_STATUS,
                                            --INS_CHG_PROC_LOG,
                                            --INS_CHG_PROC_DT,
                                            @CREATED_BY,
                                            getdate(),
                                            @UPDATED_BY,
                                            getdate(),
                                            @FUNC_ID
                                        from VW_H_EMP_DATA E
                                        where E.EMP_ID = @EMP_ID ");
                ht.Add("@HR_CHG_NO", (string)HR_CHG_NO[i]);
                ht.Add("@HR_CHG_CD", HR_CHG_CD);
                ht.Add("@EMP_ID", EMP_ID);
                ht.Add("@START_DT", START_DT);
                ht.Add("@CHG_SEQ", (int)CHG_SEQ[i]);
                ht.Add("@INS_PLAN_PROC_DT", INS_PLAN_PROC_DT);
                ht.Add("@PLAN_END_DT", PLAN_END_DT);
                //ht.Add("@END_HR_CHG_NO", END_HR_CHG_NO);
                ht.Add("@IS_END", IS_END);
                ht.Add("@MAIN_HR_CHG_NO", MAIN_HR_CHG_NO);
                ht.Add("@ICT_TYPE", ICT_TYPE);
                ht.Add("@TRANSFER_NATION_CD", TRANSFER_NATION_CD);
                ht.Add("@TRANSFER_COMPANY_CD", TRANSFER_COMPANY_CD);
                ht.Add("@TRANSFER_DEPT", TRANSFER_DEPT);
                ht.Add("@IS_PAY_SUBSIST", IS_PAY_SUBSIST);
                ht.Add("@HR_CHG_PROC_STATUS", HR_CHG_PROC_STATUS);
                ht.Add("@INS_CHG_PROC_STATUS", INS_CHG_PROC_STATUS);
                ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
                ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
                ht.Add("@FUNC_ID", "FB2HC010");
                dbConn.ExecuteT(sb, ht);
            }
            //新增明細檔
            j = 0;
            for (int i = 0; i < gv_result.Rows.Count; i++)
            {
                if (gv_result.Rows[i]["HR_CHG_NO"].ToString() == "")
                {
                    sb = new StringBuilder();
                    ht = new Hashtable();
                    sb.AppendLine(@"insert into TB_H_M_EMP_HR_CHANGE_D (
                                    HR_CHG_NO,
                                    EMP_ID,
                                    HR_CHG_ITEM,                                    
                                    AFTER_CD,
                                    AFTER_DESC,
                                    CREATED_BY,
                                    CREATED_DT,
                                    UPDATED_BY,
                                    UPDATED_DT,
                                    FUNC_ID) values (
                                    @HR_CHG_NO,
                                    @EMP_ID,
                                    @HR_CHG_ITEM,                                    
                                    @AFTER_CD,
                                    @AFTER_DESC,
                                    @CREATED_BY,
                                    getdate(),
                                    @UPDATED_BY,
                                    getdate(),
                                    @FUNC_ID
                                ) ");
                    ht.Add("@HR_CHG_NO", (string)HR_CHG_NO[j]);
                    ht.Add("@EMP_ID", EMP_ID);
                    ht.Add("@HR_CHG_ITEM", "05");
                    ht.Add("@AFTER_CD", gv_result.Rows[i]["DEPT_NO"].ToString().ToUpper());
                    ht.Add("@AFTER_DESC", gv_result.Rows[i]["DEPT_NAME"].ToString());
                    ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
                    ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
                    ht.Add("@FUNC_ID", "FB2HC010");
                    dbConn.ExecuteT(sb, ht);

                    sb = new StringBuilder();
                    ht = new Hashtable();
                    sb.AppendLine(@"insert into TB_H_M_EMP_HR_CHANGE_D (
                                    HR_CHG_NO,
                                    EMP_ID,
                                    HR_CHG_ITEM,                                    
                                    AFTER_CD,
                                    AFTER_DESC,
                                    CREATED_BY,
                                    CREATED_DT,
                                    UPDATED_BY,
                                    UPDATED_DT,
                                    FUNC_ID) values (
                                    @HR_CHG_NO,
                                    @EMP_ID,
                                    @HR_CHG_ITEM,                                    
                                    @AFTER_CD,
                                    @AFTER_DESC,
                                    @CREATED_BY,
                                    getdate(),
                                    @UPDATED_BY,
                                    getdate(),
                                    @FUNC_ID
                                ) ");
                    ht.Add("@HR_CHG_NO", (string)HR_CHG_NO[j]);
                    ht.Add("@EMP_ID", EMP_ID);
                    ht.Add("@HR_CHG_ITEM", "08");
                    ht.Add("@AFTER_CD", gv_result.Rows[i]["PJOB_CD"].ToString().ToUpper());
                    ht.Add("@AFTER_DESC", gv_result.Rows[i]["PJOB_DESC"].ToString());
                    ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
                    ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
                    ht.Add("@FUNC_ID", "FB2HC010");
                    dbConn.ExecuteT(sb, ht);
                    j++;
                }
            }

            //修改主檔
            sb = new StringBuilder();
            ht = new Hashtable();
            sb.AppendLine(@"update TB_H_M_EMP_HR_CHANGE_H set                                    
                                    INS_PLAN_PROC_DT = @INS_PLAN_PROC_DT,
                                    PLAN_END_DT = (case when @PLAN_END_DT = '' then null else @PLAN_END_DT end),
                                    IS_END = @IS_END,
                                    MAIN_HR_CHG_NO = @MAIN_HR_CHG_NO,
                                    ICT_TYPE = @ICT_TYPE,
                                    TRANSFER_NATION_CD = @TRANSFER_NATION_CD,
                                    TRANSFER_COMPANY_CD = @TRANSFER_COMPANY_CD,
                                    TRANSFER_DEPT = @TRANSFER_DEPT,
                                    IS_PAY_SUBSIST = @IS_PAY_SUBSIST,                                    
                                    UPDATED_BY = @UPDATED_BY,
                                    UPDATED_DT = getdate(),
                                    FUNC_ID = @FUNC_ID
                            where HR_CHG_NO = @HR_CHG_NO
                                and EMP_ID = @EMP_ID ");
            ht.Add("@HR_CHG_NO", HR_CHG_NO_for_Update);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@INS_PLAN_PROC_DT", INS_PLAN_PROC_DT);
            ht.Add("@PLAN_END_DT", PLAN_END_DT);
            ht.Add("@IS_END", IS_END);
            ht.Add("@MAIN_HR_CHG_NO", MAIN_HR_CHG_NO);
            ht.Add("@ICT_TYPE", ICT_TYPE);
            ht.Add("@TRANSFER_NATION_CD", TRANSFER_NATION_CD);
            ht.Add("@TRANSFER_COMPANY_CD", TRANSFER_COMPANY_CD);
            ht.Add("@TRANSFER_DEPT", TRANSFER_DEPT);
            ht.Add("@IS_PAY_SUBSIST", IS_PAY_SUBSIST);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2HC010");
            dbConn.ExecuteT(sb, ht);

            j = 0;
            //修改明細
            for (int i = 0; i < gv_result.Rows.Count; i++)
            {
                if (gv_result.Rows[i]["HR_CHG_NO"].ToString() != "")
                {
                    sb = new StringBuilder();
                    ht = new Hashtable();
                    sb.AppendLine(@"update TB_H_M_EMP_HR_CHANGE_D set                                    
                                    AFTER_CD = @AFTER_CD,
                                    AFTER_DESC = @AFTER_DESC,
                                    UPDATED_BY = @UPDATED_BY,
                                    UPDATED_DT = getdate(),
                                    FUNC_ID = @FUNC_ID
                            where HR_CHG_NO = @HR_CHG_NO
                                and EMP_ID = @EMP_ID
                                and HR_CHG_ITEM = '05' ");
                    ht.Add("@HR_CHG_NO", HR_CHG_NO_for_Update);
                    ht.Add("@EMP_ID", EMP_ID);
                    ht.Add("@AFTER_CD", gv_result.Rows[i]["DEPT_NO"].ToString());
                    ht.Add("@AFTER_DESC", gv_result.Rows[i]["DEPT_NAME"].ToString());
                    ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
                    ht.Add("@FUNC_ID", "FB2HC010");
                    dbConn.ExecuteT(sb, ht);

                    sb = new StringBuilder();
                    ht = new Hashtable();
                    sb.AppendLine(@"update TB_H_M_EMP_HR_CHANGE_D set                                    
                                    AFTER_CD = @AFTER_CD,
                                    AFTER_DESC = @AFTER_DESC,
                                    UPDATED_BY = @UPDATED_BY,
                                    UPDATED_DT = getdate(),
                                    FUNC_ID = @FUNC_ID
                            where HR_CHG_NO = @HR_CHG_NO
                                and EMP_ID = @EMP_ID
                                and HR_CHG_ITEM = '08' ");
                    ht.Add("@HR_CHG_NO", HR_CHG_NO_for_Update);
                    ht.Add("@EMP_ID", EMP_ID);
                    ht.Add("@AFTER_CD", gv_result.Rows[i]["PJOB_CD"].ToString().ToUpper());
                    ht.Add("@AFTER_DESC", gv_result.Rows[i]["PJOB_DESC"].ToString());
                    ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
                    ht.Add("@FUNC_ID", "FB2HC010");
                    dbConn.ExecuteT(sb, ht);
                    j++;
                }
            }
            //刪除主、明細
            if (j == 0)
            {
                sb = new StringBuilder();
                ht = new Hashtable();

                //寫log  再刪
                sb.AppendLine(@" update TB_H_M_EMP_HR_CHANGE_H set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HC010'
                                 where HR_CHG_NO = @HR_CHG_NO and EMP_ID = @EMP_ID; 
                                 update TB_H_M_EMP_HR_CHANGE_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HC010'
                                 where HR_CHG_NO = @HR_CHG_NO and EMP_ID = @EMP_ID; 
                                delete from TB_H_M_EMP_HR_CHANGE_H where HR_CHG_NO = @HR_CHG_NO and EMP_ID = @EMP_ID;
                                delete from TB_H_M_EMP_HR_CHANGE_D where HR_CHG_NO = @HR_CHG_NO and EMP_ID = @EMP_ID;");
                ht.Add("@HR_CHG_NO", HR_CHG_NO_for_Update);
                ht.Add("@EMP_ID", EMP_ID);
                ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
                dbConn.ExecuteT(sb, ht);
            }
        }
        //非兼任 & D04(結束兼任) 
        else
        {
            sb = new StringBuilder();
            ht = new Hashtable();
            //修改主檔
            sb.AppendLine(@"update TB_H_M_EMP_HR_CHANGE_H set                                    
                                    INS_PLAN_PROC_DT = @INS_PLAN_PROC_DT,
                                    PLAN_END_DT = (case when @PLAN_END_DT = '' then null else @PLAN_END_DT end),
                                    IS_END = @IS_END,
                                    MAIN_HR_CHG_NO = @MAIN_HR_CHG_NO,
                                    ICT_TYPE = @ICT_TYPE,
                                    TRANSFER_NATION_CD = @TRANSFER_NATION_CD,
                                    TRANSFER_COMPANY_CD = @TRANSFER_COMPANY_CD,
                                    TRANSFER_DEPT = @TRANSFER_DEPT,
                                    IS_PAY_SUBSIST = @IS_PAY_SUBSIST,                                    
                                    UPDATED_BY = @UPDATED_BY,
                                    UPDATED_DT = getdate(),
                                    FUNC_ID = @FUNC_ID
                            where HR_CHG_NO = @HR_CHG_NO
                                and EMP_ID = @EMP_ID ");
            ht.Add("@HR_CHG_NO", HR_CHG_NO_for_Update);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@INS_PLAN_PROC_DT", INS_PLAN_PROC_DT);
            ht.Add("@PLAN_END_DT", @PLAN_END_DT);
            ht.Add("@IS_END", IS_END);
            ht.Add("@MAIN_HR_CHG_NO", MAIN_HR_CHG_NO);
            ht.Add("@ICT_TYPE", ICT_TYPE);
            ht.Add("@TRANSFER_NATION_CD", TRANSFER_NATION_CD);
            ht.Add("@TRANSFER_COMPANY_CD", TRANSFER_COMPANY_CD);
            ht.Add("@TRANSFER_DEPT", TRANSFER_DEPT);
            ht.Add("@IS_PAY_SUBSIST", IS_PAY_SUBSIST);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2HC010");
            dbConn.ExecuteT(sb, ht);


            sb = new StringBuilder();
            ht = new Hashtable();
            //寫log
            //刪除明細
            sb.AppendLine(@" update TB_H_M_EMP_HR_CHANGE_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HC010'
                             where HR_CHG_NO = @HR_CHG_NO and EMP_ID = @EMP_ID;
                             delete from TB_H_M_EMP_HR_CHANGE_D where HR_CHG_NO = @HR_CHG_NO and EMP_ID = @EMP_ID;");
            ht.Add("@HR_CHG_NO", HR_CHG_NO_for_Update);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            dbConn.ExecuteT(sb, ht);

            //新增明細檔
            for (int i = 0; i < gv_result2.Rows.Count; i++)
            {
                sb = new StringBuilder();
                ht = new Hashtable();
                sb.AppendLine(@"insert into TB_H_M_EMP_HR_CHANGE_D (
                                    HR_CHG_NO,
                                    EMP_ID,
                                    HR_CHG_ITEM,
                                    BEFORE_CD,
                                    BEFORE_DESC,
                                    AFTER_CD,
                                    AFTER_DESC,
                                    CREATED_BY,
                                    CREATED_DT,
                                    UPDATED_BY,
                                    UPDATED_DT,
                                    FUNC_ID) values (
                                    @HR_CHG_NO,
                                    @EMP_ID,
                                    @HR_CHG_ITEM,
                                    @BEFORE_CD,
                                    @BEFORE_DESC,
                                    @AFTER_CD,
                                    @AFTER_DESC,
                                    @CREATED_BY,
                                    getdate(),
                                    @UPDATED_BY,
                                    getdate(),
                                    @FUNC_ID
                                ) ");
                ht.Add("@HR_CHG_NO", HR_CHG_NO_for_Update);
                ht.Add("@EMP_ID", EMP_ID);
                ht.Add("@HR_CHG_ITEM", gv_result2.Rows[i]["HR_CHG_ITEM"].ToString());
                ht.Add("@BEFORE_CD", gv_result2.Rows[i]["BEFORE_CD"].ToString());
                ht.Add("@BEFORE_DESC", gv_result2.Rows[i]["BEFORE_DESC"].ToString());
                ht.Add("@AFTER_CD", gv_result2.Rows[i]["AFTER_CD"].ToString().ToUpper());
                ht.Add("@AFTER_DESC", gv_result2.Rows[i]["AFTER_DESC"].ToString());
                ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
                ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
                ht.Add("@FUNC_ID", "FB2HC010");
                dbConn.ExecuteT(sb, ht);
            }
        }
    }

    //一括異動儲存
    public void WFB2HC0100_Add_batch_Save()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        DataTable dt = new DataTable();
        string before_cd = "";
        string before_desc = "";
        //新增主檔
        for (int i = 0; i < EMP_IDs.Count; i++)
        {
            sb = new StringBuilder();
            ht = new Hashtable();
            sb.AppendLine(@"declare @CHG_SEQ int = (select 	
	                            isnull(
		                            (
			                            select max(A.CHG_SEQ)+1
			                            from TB_H_M_EMP_HR_CHANGE_H A
			                            where A.EMP_ID = @EMP_ID
			                            and A.START_DT = @START_DT
		                            ),1));
                        insert into TB_H_M_EMP_HR_CHANGE_H (
                                HR_CHG_NO,
                                HR_CHG_CD,
                                EMP_ID,
                                START_DT,
                                CHG_SEQ,
                                INS_PLAN_PROC_DT,
                                PLAN_END_DT,
                                --END_HR_CHG_NO,
                                IS_END,
                                MAIN_HR_CHG_NO,
                                --ICT_TYPE,
                                --TRANSFER_NATION_CD,
                                --TRANSFER_COMPANY_CD,
                                --TRANSFER_DEPT,
                                IS_PAY_SUBSIST,
                                ORI_WS_CD,
                                ORI_COMPANY_CD,
                                ORI_PLANT_CD,
                                ORI_DEPT_NO,
                                ORI_DEPT_NAME,
                                ORI_DEPT_FULL_NAME,
                                ORI_DIV_DEPT_FULL_NAME,
                                ORI_DEPT_NAME_20,
                                ORI_DEPT_NAME_30,
                                ORI_DEPT_NAME_40,
                                ORI_DEPT_NAME_50,
                                ORI_DEPT_NAME_60,
                                ORI_DEPT_NAME_70,
                                ORI_EMP_CD,
                                ORI_LEVEL_CD,
                                ORI_GRADE_CD,
                                ORI_PJOB_CD,
                                ORI_PJOB_DESC,
                                ORI_WORK_SHIFT_CD,
                                ORI_WORK_CD,
                                HR_CHG_PROC_STATUS,
                                --HR_CHG_PROC_LOG,
                                --HR_CHG_PROC_DT,
                                INS_CHG_PROC_STATUS,
                                --INS_CHG_PROC_LOG,
                                --INS_CHG_PROC_DT,
                                CREATED_BY,
                                CREATED_DT,
                                UPDATED_BY,
                                UPDATED_DT,
                                FUNC_ID) 
                                    select 
                                        @HR_CHG_NO,
                                        @HR_CHG_CD,
                                        @EMP_ID,
                                        @START_DT,
                                        @CHG_SEQ,
                                        @INS_PLAN_PROC_DT,
                                        (case when @PLAN_END_DT = '' then null else @PLAN_END_DT end),
                                        --END_HR_CHG_NO,
                                        @IS_END,
                                        @MAIN_HR_CHG_NO,
                                        --@ICT_TYPE,
                                        --@TRANSFER_NATION_CD,
                                        --@TRANSFER_COMPANY_CD,
                                        --@TRANSFER_DEPT,
                                        'N',
                                        E.WS_CD,
                                        E.COMPANY_CD,
                                        E.PLANT_CD,
                                        E.DEPT_NO,
                                        E.DEPT_NAME,
                                        E.DEPT_FULL_NAME,
                                        E.DIV_DEPT_FULL_NAME,
                                        E.DEPT_NAME_20,
                                        E.DEPT_NAME_30,
                                        E.DEPT_NAME_40,
                                        E.DEPT_NAME_50,
                                        E.DEPT_NAME_60,
                                        E.DEPT_NAME_70,
                                        E.EMP_CD,
                                        E.LEVEL_CD,
                                        E.GRADE_CD,
                                        E.PJOB_CD,
                                        E.PJOB_DESC,
                                        E.WORK_SHIFT_CD,
                                        E.WORK_CD,
                                        @HR_CHG_PROC_STATUS,
                                        --HR_CHG_PROC_LOG,
                                        --HR_CHG_PROC_DT,
                                        @INS_CHG_PROC_STATUS,
                                        --INS_CHG_PROC_LOG,
                                        --INS_CHG_PROC_DT,
                                        @CREATED_BY,
                                        getdate(),
                                        @UPDATED_BY,
                                        getdate(),
                                        @FUNC_ID
                                    from VW_H_EMP_DATA E
                                    where E.EMP_ID = @EMP_ID ");
            ht.Add("@HR_CHG_NO", (string)HR_CHG_NO[0]);
            ht.Add("@HR_CHG_CD", HR_CHG_CD.ToUpper());
            ht.Add("@EMP_ID", EMP_IDs[i]);
            ht.Add("@START_DT", START_DT);
            //ht.Add("@CHG_SEQ", (int)CHG_SEQ[0]);
            ht.Add("@INS_PLAN_PROC_DT", INS_PLAN_PROC_DT);
            ht.Add("@PLAN_END_DT", PLAN_END_DT);
            //ht.Add("@END_HR_CHG_NO", END_HR_CHG_NO);
            //若 明細畫面.狀態結束有勾選，寫入'Y'；若 WK_異動主編號有值，寫入'Y'；若 WK_異動主編號 沒有值，寫入'N'
            string is_end = "";
            if (IS_END == "Y")
                is_end = "Y";
            else if (MAIN_HR_CHG_NOs[i] != "")
                is_end = "Y";
            else
                is_end = "N";
            ht.Add("@IS_END", is_end);
            ht.Add("@MAIN_HR_CHG_NO", MAIN_HR_CHG_NOs[i]);
            ht.Add("@HR_CHG_PROC_STATUS", HR_CHG_PROC_STATUS);
            ht.Add("@INS_CHG_PROC_STATUS", INS_CHG_PROC_STATUS);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2HC010");
            dbConn.ExecuteT(sb, ht);

            //新增明細檔
            for (int j = 0; j < gv_result.Rows.Count; j++)
            {
                sb = new StringBuilder();
                ht = new Hashtable();
                sb.AppendLine(@"insert into TB_H_M_EMP_HR_CHANGE_D (
                                HR_CHG_NO,
                                EMP_ID,
                                HR_CHG_ITEM,
                                BEFORE_CD,
                                BEFORE_DESC,
                                AFTER_CD,
                                AFTER_DESC,
                                CREATED_BY,
                                CREATED_DT,
                                UPDATED_BY,
                                UPDATED_DT,
                                FUNC_ID) values (
                                @HR_CHG_NO,
                                @EMP_ID,
                                @HR_CHG_ITEM,
                                @BEFORE_CD,
                                @BEFORE_DESC,
                                @AFTER_CD,
                                @AFTER_DESC,
                                @CREATED_BY,
                                getdate(),
                                @UPDATED_BY,
                                getdate(),
                                @FUNC_ID
                            ) ");
                ht.Add("@HR_CHG_NO", (string)HR_CHG_NO[0]);
                ht.Add("@EMP_ID", EMP_IDs[i]);
                ht.Add("@HR_CHG_ITEM", gv_result.Rows[j]["HR_CHG_ITEM"].ToString());
                before_cd = "";
                before_desc = "";
                if (gv_result.Rows[j]["HR_CHG_ITEM"].ToString() == "01")
                {
                    dt = Get_HR_CHG_ITEM_01_BEFORE(EMP_IDs[i]);
                }
                else if (gv_result.Rows[j]["HR_CHG_ITEM"].ToString() == "02")
                {
                    dt = Get_HR_CHG_ITEM_02_BEFORE(EMP_IDs[i]);
                }
                else if (gv_result.Rows[j]["HR_CHG_ITEM"].ToString() == "03")
                {
                    dt = Get_HR_CHG_ITEM_03_BEFORE(EMP_IDs[i]);
                }
                else if (gv_result.Rows[j]["HR_CHG_ITEM"].ToString() == "04")
                {
                    dt = Get_HR_CHG_ITEM_04_BEFORE(EMP_IDs[i]);
                }
                else if (gv_result.Rows[j]["HR_CHG_ITEM"].ToString() == "05")
                {
                    dt = Get_HR_CHG_ITEM_05_BEFORE(EMP_IDs[i]);
                }
                else if (gv_result.Rows[j]["HR_CHG_ITEM"].ToString() == "06")
                {
                    dt = Get_HR_CHG_ITEM_06_BEFORE(EMP_IDs[i]);
                }
                else if (gv_result.Rows[j]["HR_CHG_ITEM"].ToString() == "07")
                {
                    dt = Get_HR_CHG_ITEM_07_BEFORE(EMP_IDs[i]);
                }
                else if (gv_result.Rows[j]["HR_CHG_ITEM"].ToString() == "08")
                {
                    dt = Get_HR_CHG_ITEM_08_BEFORE(EMP_IDs[i]);
                }
                else if (gv_result.Rows[j]["HR_CHG_ITEM"].ToString() == "09")
                {
                    dt = Get_HR_CHG_ITEM_09_BEFORE(EMP_IDs[i]);
                }
                else if (gv_result.Rows[j]["HR_CHG_ITEM"].ToString() == "10")
                {
                    dt = Get_HR_CHG_ITEM_10_BEFORE(EMP_IDs[i]);
                }
                if (dt.Rows.Count > 0)
                {
                    before_cd = dt.Rows[0]["sub_cd"].ToString();
                    before_desc = dt.Rows[0]["sub_desc"].ToString();
                }
                ht.Add("@BEFORE_CD", before_cd);
                ht.Add("@BEFORE_DESC", before_desc);
                ht.Add("@AFTER_CD", gv_result.Rows[j]["AFTER_CD"].ToString().ToUpper());
                ht.Add("@AFTER_DESC", gv_result.Rows[j]["AFTER_DESC"].ToString());
                ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
                ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
                ht.Add("@FUNC_ID", "FB2HC010");
                dbConn.ExecuteT(sb, ht);
            }
        }
    }

    //若直接輸入工號,讀取員工人事主檔 取得 工號=輸入工號 的姓名。 
    public string[] Qry_Get_EMP_NAME(string emp_id)
    {
        try
        {
            string[] rtnval = new string[1];
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select   EMP_NAME                                    
                            from TB_H_M_EMP
                            where 1 = 1
                            and EMP_ID = @EMP_ID ");

            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                rtnval[0] = dr["EMP_NAME"].ToString();
            }
            else
            {
                rtnval[0] = "";
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //若直接輸入代碼,讀取人事異動代碼檔 取得 人事異動代碼=輸入代碼 的 人事異動代碼說明。 
    public string[] Qry_Get_HR_CHG_DESC(string hr_chg_cd)
    {
        try
        {
            string[] rtnval = new string[1];
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select   HR_CHG_DESC                                    
                            from TB_H_M_HR_CHANGE_CODE
                            where 1 = 1
                            and HR_CHG_CD = @HR_CHG_CD ");

            ht.Add("@HR_CHG_CD", hr_chg_cd);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                rtnval[0] = dr["HR_CHG_DESC"].ToString();
            }
            else
            {
                rtnval[0] = "";
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //查詢頁的條件
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,
                            string emp_id, string start_sdt, string start_edt,
                            string hr_chg_cd, string hr_chg_proc_status)
    {
        try
        {
            if (sortExpression == "")
                sortExpression = "HR_CHG_NO";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* ");
            sb.AppendLine("       from(");
            sb.AppendLine(@"            select   A.HR_CHG_NO
		                                        ,A.HR_CHG_CD						
		                                        ,A.HR_CHG_CD + '-' + ISNULL(CD.HR_CHG_DESC,'') AS HR_CHG_CD_DESC 						
		                                        ,A.START_DT						
		                                        ,A.EMP_ID						
		                                        ,E.EMP_NAME						
		                                        ,A.HR_CHG_PROC_STATUS
                                                ,A.INS_CHG_PROC_STATUS
                                                ,isnull( (select EMP_NAME  from TB_H_M_EMP where EMP_ID=A.CREATED_BY),'')　CREATED_NAME
                                                ,CONVERT(VARCHAR(10),A.CREATED_DT,111)  CREATED_DT
                                                ,CM.SUB_DESC HR_CHG_PROC_STATUS_DESC
                                                ,IFLOW_NO
		                                        ,isnull( ( select AFTER_CD +'-'+AFTER_DESC from  TB_H_M_EMP_HR_CHANGE_D  where HR_CHG_NO=A.HR_CHG_NO and EMP_ID=A.EMP_ID and HR_CHG_ITEM='01' ),'')	AF_COMPANY_CD_DESC	 
		                                        ,isnull( ( select AFTER_CD +'-'+AFTER_DESC from  TB_H_M_EMP_HR_CHANGE_D  where HR_CHG_NO=A.HR_CHG_NO and EMP_ID=A.EMP_ID and HR_CHG_ITEM='02' ),'')	AF_PLANT_CD_DESC	 
		                                        ,isnull( ( select AFTER_CD                 from  TB_H_M_EMP_HR_CHANGE_D  where HR_CHG_NO=A.HR_CHG_NO and EMP_ID=A.EMP_ID and HR_CHG_ITEM='03' ),'')	AF_WS_CD_DESC	 
		                                        ,isnull( ( select AFTER_CD +'-'+AFTER_DESC from  TB_H_M_EMP_HR_CHANGE_D  where HR_CHG_NO=A.HR_CHG_NO and EMP_ID=A.EMP_ID and HR_CHG_ITEM='04' ),'')	AF_EMP_CD_DESC	 
		                                        ,isnull( ( select AFTER_CD +' '+AFTER_DESC from  TB_H_M_EMP_HR_CHANGE_D  where HR_CHG_NO=A.HR_CHG_NO and EMP_ID=A.EMP_ID and HR_CHG_ITEM='05' ),'')	AF_DEPT_NO_DESC	 
		                                        ,isnull( ( select AFTER_CD                 from  TB_H_M_EMP_HR_CHANGE_D  where HR_CHG_NO=A.HR_CHG_NO and EMP_ID=A.EMP_ID and HR_CHG_ITEM='06' ),'')	AF_LEVEL_CD_DESC	 
		                                        ,isnull( ( select AFTER_CD                 from  TB_H_M_EMP_HR_CHANGE_D  where HR_CHG_NO=A.HR_CHG_NO and EMP_ID=A.EMP_ID and HR_CHG_ITEM='07' ),'')	AF_GRADE_CD_DESC	 
		                                        ,isnull( ( select AFTER_CD +'-'+AFTER_DESC from  TB_H_M_EMP_HR_CHANGE_D  where HR_CHG_NO=A.HR_CHG_NO and EMP_ID=A.EMP_ID and HR_CHG_ITEM='08' ),'')	AF_PJOB_CD_DESC	 
		                                        ,isnull( ( select AFTER_CD +'-'+AFTER_DESC from  TB_H_M_EMP_HR_CHANGE_D  where HR_CHG_NO=A.HR_CHG_NO and EMP_ID=A.EMP_ID and HR_CHG_ITEM='09' ),'')	AF_WORK_SHIFT_CD_DESC	 
		                                        ,isnull( ( select AFTER_CD +'-'+AFTER_DESC from  TB_H_M_EMP_HR_CHANGE_D  where HR_CHG_NO=A.HR_CHG_NO and EMP_ID=A.EMP_ID and HR_CHG_ITEM='10' ),'')	AF_WORK_CD_DESC				 
                                        from TB_H_M_EMP_HR_CHANGE_H A
		                                left join TB_H_M_EMP E on A.EMP_ID=E.EMP_ID
		                                left join TB_H_M_HR_CHANGE_CODE CD on A.HR_CHG_CD=CD.HR_CHG_CD
                                        left join TB_9_M_COMM_D CM on CM.SYS_CD = 'HC'  and CM.MAIN_CD = 'HR_CHG_PROC_STATUS' and CM.SUB_CD = A.HR_CHG_PROC_STATUS
                                        where 1 = 1");
            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and A.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (start_sdt != "" && start_sdt != null)
            {
                sb.AppendLine(" and A.START_DT >= @START_SDT ");
                ht.Add("@START_SDT", start_sdt);
            }
            if (start_edt != "" && start_edt != null)
            {
                sb.AppendLine(" and A.START_DT <= @START_EDT ");
                ht.Add("@START_EDT", start_edt);
            }
            if (hr_chg_proc_status != "" && hr_chg_proc_status != null)
            {
                sb.AppendLine(" and A.HR_CHG_PROC_STATUS = @HR_CHG_PROC_STATUS ");
                ht.Add("@HR_CHG_PROC_STATUS", hr_chg_proc_status);
            }
            if (hr_chg_cd != "" && hr_chg_cd != null)
            {
                sb.AppendLine(" and A.HR_CHG_CD	like @HR_CHG_CD ");
                ht.Add("@HR_CHG_CD", hr_chg_cd);
            }
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
                //20150902 若小分類權限(strSysCodeAtt) 為 W(各單位擔賞),則只能查 權限區分 為D的人事異動代碼
                if (strSysCodeAtt == "W")
                {
                    sb.Append(@" AND a.HR_CHG_CD IN(	 select HR_CHG_CD from TB_H_M_HR_CHANGE_CODE where UPD_RIGHT_CD='D'  )");
                }
            }
            

            /*
            //若不為super user
            if (!strIsSuper)
            {
                //依部門權限及可管理員工的條件先找出可查詢的人事異動主檔
                sb.AppendLine(" and   A.HR_CHG_NO+ A.EMP_ID in (select HR_CHG_NO+EMP_ID  from  FN_H_GET_AUTH_HR_CHG(@LOGIN_ID2,@strDepartments))  ");
                ht.Add("@LOGIN_ID2", SessionHandle.Current.emp_id);
                ht.Add("@strDepartments", strDepartments.Replace(" ", ""));

                //若 資料權限之「小分類」為N(管理部擔當)，
                if (strSysCodeAtt == "N")
                {
                    sb.AppendLine(@" and A.HR_CHG_CD in (select G.HR_CHG_CD
					                                 from TB_H_M_HR_CHANGE_CODE G 
					                                 inner join TB_H_M_HR_CHANGE_CODE_EMP F
						                                on G.HR_CHG_CD = F.HR_CHG_CD
						                                and F.EMP_ID = @LOGIN_ID1) ");
                    ht.Add("@LOGIN_ID1", SessionHandle.Current.emp_id);
                }

                //若 資料權限之「小分類」為W(各單位擔當)，
                if (strSysCodeAtt == "W")
                {
                    sb.AppendLine(@" and A.HR_CHG_CD in (select G.HR_CHG_CD
					                                 from TB_H_M_HR_CHANGE_CODE G 
					                                 where UPD_RIGHT_CD = 'D') ");
                    //部門權限(自已可管理的部門)
                    sb.AppendLine(@" and E.DEPT_NO in ( ");
                    sb.AppendLine(@"  select MNG_DEPT_NO
				                                       from TB_H_R_HEAD_DEPT D
				                                       where EMP_ID = @LOGIN_ID3 )       ");
                    ht.Add("@LOGIN_ID3", SessionHandle.Current.emp_id);
                }
            }
            */
            sb.AppendLine("         )alltb ");
            sb.AppendLine("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

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
                        string emp_id, string start_sdt, string start_edt,
                            string hr_chg_cd, string hr_chg_proc_status)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select  COUNT(*) total_record  ");
            sb.AppendLine("       from(");
            sb.AppendLine(@"            select   A.HR_CHG_NO
                                        from TB_H_M_EMP_HR_CHANGE_H A
                                        left join TB_H_M_EMP E on A.EMP_ID=E.EMP_ID
                                        where 1 = 1");
            if (emp_id != "" && emp_id != null)
            {
                sb.AppendLine(" and A.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (start_sdt != "" && start_sdt != null)
            {
                sb.AppendLine(" and A.START_DT >= @START_SDT ");
                ht.Add("@START_SDT", start_sdt);
            }
            if (start_edt != "" && start_edt != null)
            {
                sb.AppendLine(" and A.START_DT <= @START_EDT ");
                ht.Add("@START_EDT", start_edt);
            }
            if (hr_chg_proc_status != "" && hr_chg_proc_status != null)
            {
                sb.AppendLine(" and A.HR_CHG_PROC_STATUS = @HR_CHG_PROC_STATUS ");
                ht.Add("@HR_CHG_PROC_STATUS", hr_chg_proc_status);
            }
            if (hr_chg_cd != "" && hr_chg_cd != null)
            {
                sb.AppendLine(" and A.HR_CHG_CD	like @HR_CHG_CD ");
                ht.Add("@HR_CHG_CD", hr_chg_cd);
            }
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);

                //20150902 若小分類權限(strSysCodeAtt) 為 W(各單位擔賞),則只能查 權限區分 為D的人事異動代碼
                if (strSysCodeAtt == "W")
                {
                    sb.Append(@" AND a.HR_CHG_CD IN(	 select HR_CHG_CD from TB_H_M_HR_CHANGE_CODE where UPD_RIGHT_CD='D'  )");
                }
            }
            /*
            //若不為super user
            if (!strIsSuper)
            {
                //依部門權限及可管理員工的條件先找出可查詢的人事異動主檔
                sb.AppendLine(" and   A.HR_CHG_NO+ A.EMP_ID in (select HR_CHG_NO+EMP_ID  from  FN_H_GET_AUTH_HR_CHG(@LOGIN_ID2,@strDepartments))  ");
                ht.Add("@LOGIN_ID2", SessionHandle.Current.emp_id);
                ht.Add("@strDepartments", strDepartments.Replace(" ", ""));

                //若 資料權限之「小分類」為N(管理部擔當)，
                if (strSysCodeAtt == "N")
                {
                    sb.AppendLine(@" and A.HR_CHG_CD in (select G.HR_CHG_CD
					                                 from TB_H_M_HR_CHANGE_CODE G 
					                                 inner join TB_H_M_HR_CHANGE_CODE_EMP F
						                                on G.HR_CHG_CD = F.HR_CHG_CD
						                                and F.EMP_ID = @LOGIN_ID1) ");
                    ht.Add("@LOGIN_ID1", SessionHandle.Current.emp_id);
                }

                //若 資料權限之「小分類」為W(各單位擔當)，
                if (strSysCodeAtt == "W")
                {
                    sb.AppendLine(@" and A.HR_CHG_CD in (select G.HR_CHG_CD
					                                 from TB_H_M_HR_CHANGE_CODE G 
					                                 where UPD_RIGHT_CD = 'D') ");
                    //部門權限(自已可管理的部門)
                    sb.AppendLine(@" and E.DEPT_NO in ( ");
                    sb.AppendLine(@"  select MNG_DEPT_NO
				                                       from TB_H_R_HEAD_DEPT D
				                                       where EMP_ID = @LOGIN_ID3 )       ");
                    ht.Add("@LOGIN_ID3", SessionHandle.Current.emp_id);
                }

               
            }
             */
            sb.AppendLine("         )alltb ");
            
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

    //刪除
    public bool Delete(ArrayList datas)
    {
        try
        {
            foreach (string[] data in datas)
            {
                StringBuilder sb = new StringBuilder();
                Hashtable ht = new Hashtable();
                sb.AppendLine(" update TB_H_M_EMP_HR_CHANGE_H set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HC010' ");
                sb.AppendLine("  where HR_CHG_NO = @HR_CHG_NO and EMP_ID = @EMP_ID; ");
                sb.AppendLine(" update TB_H_M_EMP_HR_CHANGE_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HC010' ");
                sb.AppendLine("  where HR_CHG_NO = @HR_CHG_NO and EMP_ID = @EMP_ID;");

                sb.AppendLine("delete from TB_H_M_EMP_HR_CHANGE_D where HR_CHG_NO = @HR_CHG_NO and EMP_ID = @EMP_ID; ");
                sb.AppendLine("delete from TB_H_M_EMP_HR_CHANGE_H where HR_CHG_NO = @HR_CHG_NO and EMP_ID = @EMP_ID; ");
                ht.Add("@HR_CHG_NO", data[0]);
                ht.Add("@EMP_ID", data[1]);
                ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
                dbConn.ExecuteT(sb, ht);
            }
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得員工姓名
    //1.讀取 員工人事資料VIEW(VW_H_EMP_DATA) E
    //    取得:	E.*
    //    條件:	E.工號 = 明細畫面.工號
    //          且 E.工號 <> 登入者帳號 ※不可以輸入登入者自己的人事異動資料
    //          若 資料權限之「部門含以下」或「部門權限」任一不為空值，
    //             加入條件：且 E.部門代號 必須存在以下 該擔當有權限作業的部門清單中，
    //                       UNION 以下兩者的部門清單，
    //                       若 資料權限之「部門含以下」為Y，
    //                                 讀取 主管可管理部門資料檔 D
    //                                      取得: D.可管理部門代號
    //                                      條件: D.工號 = 登入者帳號
    //                            若 資料權限之「部門權限」不為空值，
    //                                      「部門權限」的內容。
    //  若讀得到，
    //    1. 將姓名顯示於畫面上；
    //    2. E.預計派遣日, E.員工區分 (hidden)
    public string[] Get_EMP_NAME(string emp_id)
    {
        try
        {
            string[] rtnval = new string[6];
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select   EMP_ID
                                    ,EMP_NAME
                                    ,case when PLAN_DESPATCH_DT is null then ''
			                              else convert(varchar(10),PLAN_DESPATCH_DT,111)
		                             end as PLAN_DESPATCH_DT
                                    ,case when PLAN_DESPATCH_DT is null then ''
			                              else convert(varchar(10),dateadd(dd,1,PLAN_DESPATCH_DT),111)
		                             end as PLAN_DESPATCH_NEXT_DT
                                    ,EMP_CD
                                    ,LEVEL_CD 
                            from VW_H_EMP_DATA E
                            where 1 = 1
                            and EMP_ID = @EMP_ID
                            and EMP_ID != @LOGIN_ID ");

            //顯示資料權限設定,若不為super user (以員工為主)
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND E.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }
            /*
            //若不為super user (以員工為主)
            if (!strIsSuper)
            {
                sb.Append(@" AND ( ");
                sb.AppendLine(@" E.EMP_ID in ( select emp_id from TB_H_M_EMP "
                             + "             where dept_no in (select MNG_DEPT_NO from TB_H_R_HEAD_DEPT where emp_id=@LOGIN_ID2 )  "
                             + "             )" );
                ht.Add("@LOGIN_ID2", SessionHandle.Current.emp_id);
                if (strDepartments != "" && strDepartments != "N")
                {
                    //若 資料權限之「部門權限」不為空值
                    sb.Append(" or  E.EMP_ID  IN(   select u2.EMP_ID from TB_H_M_EMP u2 ");
                    sb.Append("                     where u2.DEPT_NO in ( @uDEPT_NO) )  ");
                    ht.Add("@uDEPT_NO", strDepartments.Replace(" ", "").Split(','));
                }
                sb.Append(" ) ");
            }
            */
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@LOGIN_ID", SessionHandle.Current.emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                rtnval[0] = dr["EMP_ID"].ToString();
                rtnval[1] = dr["EMP_NAME"].ToString();
                rtnval[2] = dr["PLAN_DESPATCH_DT"].ToString();
                rtnval[3] = dr["EMP_CD"].ToString();
                rtnval[4] = dr["LEVEL_CD"].ToString();
                rtnval[5] = dr["PLAN_DESPATCH_NEXT_DT"].ToString();
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得最近一次薪資計算年月
    //呼叫 Function:FN_S_SALARY_YM, 取得 「最近一次薪資計算年月」
    public string Get_FN_S_SALARY_YM()
    {
        try
        {
            string rtnval = "";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"select dbo.FN_S_SALARY_YM() as FN_S_SALARY_YM ");
            DataTable dt = dbConn.Query(sb);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                rtnval = dr["FN_S_SALARY_YM"].ToString();
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string CheckHR_CHG_CD(string hr_chg_cd)
    {
        string rtnvalue = "";
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select G.HR_CHG_CD 
                            from TB_H_M_HR_CHANGE_CODE G
                            left join TB_H_M_EMP E 
	                            on EMP_ID = @EMP_ID
                            where 1 = 1
                            and G.HR_CHG_CD = @HR_CHG_CD
                            and G.IS_VALID = 'Y'
                            and ((E.EMP_CD = '3' and G.IS_FOR_TRANSFER_IN = 'Y') or (E.EMP_CD <> '3')) ");
            ht.Add("@HR_CHG_CD", hr_chg_cd);
            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
            //若 資料權限之「小分類」為N(管理部擔當)，
            if (strSysCodeAtt == "N")
            {
                sb.AppendLine(@" and G.HR_CHG_CD in (select F.HR_CHG_CD
					                                 from TB_H_M_HR_CHANGE_CODE_EMP F
						                             where F.EMP_ID = @LOGIN_ID1
                                                        and F.IS_VALID = 'Y' ) ");
                ht.Add("@LOGIN_ID1", SessionHandle.Current.emp_id);
            }

            //若 資料權限之「小分類」為W(各單位擔當)，
            if (strSysCodeAtt == "W")
            {
                sb.AppendLine(@" and G.UPD_RIGHT_CD = 'D' ");
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count == 0)
            {
                rtnvalue = Resources.Resource.wfb2hc_HR_CHG_CD_does_not_exist_or_no_permission_to_work;
            }

            return rtnvalue;
        }
        catch (Exception)
        {
            throw;
        }
    }


    //該人事異動代碼的保險處理區分是否為N
    public DataTable checkHasInsurance(string hr_chg_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount ");
            sb.Append(" from TB_H_M_HR_CHANGE_CODE ");
            sb.Append(" where 1=1 ");
            sb.Append(" and HR_CHG_CD=@HR_CHG_CD  ");
            sb.Append(" and IS_VALID='Y'  ");
            sb.Append(" and INSURANCE_PROC_CD<>'N'  ");
            ht.Add("@HR_CHG_CD", hr_chg_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //人事異動主檔是否已有未生效的異動單且與保險處理相關時
    public DataTable checkIsInsurance(string emp_id)
    {   
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount ");
            sb.Append(" from TB_H_M_EMP_HR_CHANGE_H A ");
            sb.Append(" left join TB_H_M_HR_CHANGE_CODE B on A.HR_CHG_CD=B.HR_CHG_CD and IS_VALID='Y' ");
            sb.Append(" where 1=1 ");
            sb.Append(" and A.HR_CHG_PROC_STATUS='N'  ");
            sb.Append(" and B.INSURANCE_PROC_CD<>'N'  ");
            sb.Append(" and A.EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //確認是否已離職  
    public DataTable checkIsLeave(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where 1=1 ");
            sb.Append(" and EMP_STATUS='99'  ");
            sb.Append(" and EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //讀取 人事異動主檔 H
    //取得:	H.人事異動代碼
    //條件:	H.工號 = 明細畫面.工號
    //且 H.人事異動生效日 = 明細畫面.異動生效日
    //且 H.人事異動代碼 = 明細畫面.人事異動代碼
    public int Check_Same_Data1(string emp_id, string start_dt, string hr_chg_cd)
    {
        try
        {
            int rtnval = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select count(*) as cnt 
                            from TB_H_M_EMP_HR_CHANGE_H
                            where 1 = 1
	                            and EMP_ID = @EMP_ID
	                            and START_DT = @START_DT
	                            and HR_CHG_CD = @HR_CHG_CD");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@START_DT", start_dt);
            ht.Add("@HR_CHG_CD", hr_chg_cd);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                rtnval = Convert.ToInt32(dr["cnt"]);
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //讀取 人事異動主檔 H
    //取得:	H.人事異動代碼
    //條件:	H.工號 = 明細畫面.工號
    //且 H.人事異動生效日 = 明細畫面.異動生效日
    //若讀得到資料，可能是多筆，
    //讀取 人事異動代碼檔 G1
    //取得:	G1.人事異動代碼說明
    //條件:	G1.人事異動代碼 = H.人事異動代碼
    public string Check_Same_Data2(string emp_id, string start_dt)
    {
        try
        {
            string rtnval = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select G1.HR_CHG_DESC
                            from TB_H_M_EMP_HR_CHANGE_H H
                            left join TB_H_M_HR_CHANGE_CODE G1
	                            on G1.HR_CHG_CD = H.HR_CHG_CD
                            where 1 = 1
	                            and H.EMP_ID = @EMP_ID
	                            and H.START_DT = @START_DT");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@START_DT", start_dt);
            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                if (rtnval != "") rtnval += ",";
                rtnval += dr["HR_CHG_DESC"].ToString();
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 G.保險提前生效(IS_INS_EARLIER)
    public string Get_IS_INS_EARLIER(string hr_chg_cd)
    {
        try
        {
            string rtnval = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select G.IS_INS_EARLIER
                            from TB_H_M_HR_CHANGE_CODE G                            
                            where 1 = 1
	                            and G.HR_CHG_CD = @HR_CHG_CD");
            ht.Add("@HR_CHG_CD", hr_chg_cd);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                rtnval = dt.Rows[0]["IS_INS_EARLIER"].ToString();
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 G.保險提前生效(IS_INS_EARLIER)
    public string get_IS_LEAVE(string hr_chg_cd)
    {
        try
        {
            string rtnval = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select G.IS_LEAVE
                            from TB_H_M_HR_CHANGE_CODE G                            
                            where 1 = 1
	                            and G.HR_CHG_CD = @HR_CHG_CD");
            ht.Add("@HR_CHG_CD", hr_chg_cd);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                rtnval = dt.Rows[0]["IS_LEAVE"].ToString();
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得狀態預計結束日
    //1.若 G.是否暫時狀態(IS_TEMP)為'Y'，
    //  若 G.人事異動身份狀態(EMP_CHG_STATUS) 為'31'(期間工)，
    //  讀取 參數檔 A
    //  取得:參數值(CODE_VAL1)
    //  條件:子作業='HB' 且參數別='KZ_CONTRACT_MONTHS'
    //  預設 明細畫面.狀態預計結束日 = 明細畫面.異動生效日 的月份 + 參數值(CODE_VAL1) ，取該月月初日，再減1天，可修改，不可清空。

    //  若 G.人事異動身份狀態(EMP_CHG_STATUS) 為'32'(派遣)，
    //  讀取 參數檔 A
    //  取得:參數值(CODE_VAL1)
    //  條件:子作業='HB' 且參數別='OTH1_CONTRACT_MONTHS'
    //  預設 明細畫面.狀態預計結束日 = 明細畫面.異動生效日 的月份 + 參數值(CODE_VAL1) ，取該月月初日，再減1天，可修改，不可清空。
    public string[] Get_PLAN_END_DT(string hr_chg_cd, string start_dt)
    {
        try
        {
            string[] rtnval = new string[3];
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select    G.IS_TEMP
                                    , G.EMP_CHG_STATUS
                                    , case when G.IS_TEMP = 'Y' and G.EMP_CHG_STATUS = '31' then 
                                                case when isdate(@START_DT) = 1 and isnumeric(A1.CODE_VAL1) = 1 then 
                                                        convert(varchar(10),dateadd(dd, -1, convert(varchar(8),dateadd(MM, convert(int,A1.CODE_VAL1), @START_DT),111)+'01'),111)
                                                     else ''
                                                end 
                                           when G.IS_TEMP = 'Y' and G.EMP_CHG_STATUS = '32' then
                                                case when isdate(@START_DT) = 1 and isnumeric(A2.CODE_VAL1) = 1 then 
                                                        convert(varchar(10),dateadd(dd, -1, convert(varchar(8),dateadd(MM, Convert(int,A2.CODE_VAL1), @START_DT),111)+'01'),111)
                                                     else ''
                                                end 
                                            else ''
                                      end PLAN_END_DT 
                                    , A1.CODE_VAL1 as KZ
                                    , A2.CODE_VAL1 as OTH
                            from TB_H_M_HR_CHANGE_CODE G
                            left join TB_9_M_PARAMETER A1
                                on A1.SYS_CD = 'HB'
                                and A1.MAIN_CD = 'KZ_CONTRACT_MONTHS'
                            left join TB_9_M_PARAMETER A2
                                on A2.SYS_CD = 'HB'
                                and A2.MAIN_CD = 'OTH1_CONTRACT_MONTHS'
                            where 1 = 1
	                            and G.HR_CHG_CD = @HR_CHG_CD");
            ht.Add("@HR_CHG_CD", hr_chg_cd);
            ht.Add("@START_DT", start_dt);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                rtnval[0] = dt.Rows[0]["IS_TEMP"].ToString();
                rtnval[1] = dt.Rows[0]["EMP_CHG_STATUS"].ToString();
                rtnval[2] = dt.Rows[0]["PLAN_END_DT"].ToString();
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 是否暫時狀態 IS_TEMP
    public string[] Get_IS_TEMP(string hr_chg_cd)
    {
        try
        {
            string[] rtnval = new string[1];
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select    G.IS_TEMP
                            from TB_H_M_HR_CHANGE_CODE G
                            where 1 = 1
	                            and G.HR_CHG_CD = @HR_CHG_CD");
            ht.Add("@HR_CHG_CD", hr_chg_cd);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                rtnval[0] = dt.Rows[0]["IS_TEMP"].ToString();
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 該人事異動代碼  是否有 可異動項目檔
    public DataTable Get_HAS_CODE_ITEM(string hr_chg_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select    count(*) resultCount
                            from TB_H_M_HR_CHANGE_CODE_ITEM G
                            where 1 = 1
	                            and G.HR_CHG_CD = @HR_CHG_CD");
            ht.Add("@HR_CHG_CD", hr_chg_cd);
            DataTable dt = dbConn.Query(sb, ht);

            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }


    //取得判斷 [狀態結束] 欄及 [異動主編號] 相關資料
    //  讀取 人事異動主檔 H
    //  取得: H.人事異動編號, H.人事異動代碼
    //  條件: H.工號 = 明細畫面.工號
    //  且 H.人事異動生效日 < 明細畫面.異動生效日
    //  且 H.狀態預計結束日 IS NOT NULL
    //  且 H.人事異動狀態結束編號 IS NULL
    //  且 H.生效處理狀態 = 'Y'
    public string Get_IS_END(string emp_id, string start_dt)
    {
        try
        {
            string rtnval = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select H.HR_CHG_NO,H.HR_CHG_CD
                            from TB_H_M_EMP_HR_CHANGE_H H
                            where 1 = 1
	                            and H.EMP_ID = @EMP_ID
	                            and H.START_DT < @START_DT
	                            and H.PLAN_END_DT is not null and H.PLAN_END_DT !='9999/12/31'
                                and H.END_HR_CHG_NO =''
	                            and H.HR_CHG_PROC_STATUS = 'Y'");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@START_DT", start_dt);
            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                if (rtnval != "") rtnval += ",";
                rtnval += dr["HR_CHG_NO"].ToString();
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string Get_IS_END_STATUS(string emp_id, string start_dt, string EMP_CHG_STATUS)
    {
        try
        {
            string rtnval = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select H.HR_CHG_NO,H.HR_CHG_CD
                            from TB_H_M_EMP_HR_CHANGE_H H
                            where 1 = 1
	                            and H.EMP_ID = @EMP_ID
	                            and H.START_DT < @START_DT
	                            and H.PLAN_END_DT is not null and H.PLAN_END_DT !='9999/12/31'
                                and H.END_HR_CHG_NO =''
	                            and H.HR_CHG_PROC_STATUS = 'Y'
                                and H.HR_CHG_CD in ( 
                                      SELECT HR_CHG_CD FROM TB_H_M_HR_CHANGE_CODE WHERE EMP_CHG_STATUS =@EMP_CHG_STATUS and left(HR_CHG_CD,1)!='D' )
                        ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@START_DT", start_dt);
            ht.Add("@EMP_CHG_STATUS", EMP_CHG_STATUS);
            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                if (rtnval != "") rtnval += ",";
                rtnval += dr["HR_CHG_NO"].ToString();
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得返校的HR_CHG_NO
    public string Get_IS_END_chgcd(string emp_id, string start_dt,string hr_chg_cd )
    {
        try
        {
            string rtnval = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select H.HR_CHG_NO,H.HR_CHG_CD
                            from TB_H_M_EMP_HR_CHANGE_H H
                            where 1 = 1
	                            and H.EMP_ID = @EMP_ID
	                            and H.START_DT < @START_DT
	                            and H.PLAN_END_DT is not null and H.PLAN_END_DT !='9999/12/31'
                                and H.END_HR_CHG_NO =''
	                            and H.HR_CHG_PROC_STATUS = 'Y' 
                                and HR_CHG_CD=@HR_CHG_CD
                                ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@START_DT", start_dt);
            ht.Add("@HR_CHG_CD", hr_chg_cd);
            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                if (rtnval != "") rtnval += ",";
                rtnval += dr["HR_CHG_NO"].ToString();
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    // 取得異動主編號資料
    //  讀取 人事異動主檔 H
    //  取得: H.人事異動編號
    //  條件: H.工號 = 明細畫面.工號
    //        且 H.人事異動生效日 < 明細畫面.異動生效日
    //        且 H.狀態預計結束日 IS NOT NULL
    //        且 H.人事異動狀態結束編號 IS NULL
    //        且 H.生效處理狀態 = 'Y'
    public string Get_MAIN_HR_CHG_NO(string emp_id, string start_dt)
    {
        try
        {
            string rtnval = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select H.HR_CHG_NO,H.HR_CHG_CD
                            from TB_H_M_EMP_HR_CHANGE_H H
                            where 1 = 1
	                            and H.EMP_ID = @EMP_ID
	                            and H.START_DT < @START_DT
	                            and H.PLAN_END_DT is not null and PLAN_END_DT <>'9999/12/31'
	                            and (H.END_HR_CHG_NO is null or H.END_HR_CHG_NO = '') 
	                            and H.HR_CHG_PROC_STATUS = 'Y'");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@START_DT", start_dt);
            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                if (rtnval != "") rtnval += ",";
                rtnval += dr["HR_CHG_NO"].ToString();
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //    (1) 取得 異動主編號相關的說明
    //        讀取 人事異動代碼檔 I
    //        取得: I.人事異動代碼說明
    //        條件: I.人事異動代碼 = H. 人事異動代碼檔

    //    (2)D04(結束兼任)的異動主編號說明
    //        若H.人事異動代碼 為 D04(結束兼任)
    //            (2-1)取得兼任的部門名稱
    //                    讀取 人事異動明細檔 J
    //                    取得:	J.異動後代碼說明, J.異動後代碼說明
    //                    條件:	J.人事異動編號 = H. 人事異動編號
    //                          J.人事異動項目代碼 = 05 (部門)
    //            (2-2)取得兼任的職務名稱
    //                    讀取 人事異動明細檔 K
    //                    取得:	K.異動後代碼說明, K.異動後代碼說明
    //                    條件:	K.人事異動編號 = H. 人事異動編號
    //                          K.人事異動項目代碼 = 08 (職務)
    //    (3)明細畫面.異動主編號說明
    //            若H.人事異動代碼 為 D04(結束兼任)
    //                則 明細畫面.異動主編號說明 =  H. 人事異動代碼檔 +"  "+ I.人事異動代碼說明+" "+ J.異動後代碼說明 +" "+J.異動後代碼說明
    //            其餘
    //                則 明細畫面.異動主編號說明 =  H. 人事異動代碼檔 +"  "+ I.人事異動代碼說明
    public string Get_MAIN_HR_CHG_NO_DESC(string hr_chg_no, string emp_id, string hr_chr_CD)
    {
        try
        {
            string rtnval = "";
            string jdesc = "";
            string kdesc = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select H.HR_CHG_CD
	                             , H.HR_CHG_CD + ' ' + I.HR_CHG_DESC as HR_CHG_DESC
                            from TB_H_M_EMP_HR_CHANGE_H H
                            left join TB_H_M_HR_CHANGE_CODE I on I.HR_CHG_CD = H.HR_CHG_CD
                            where 1 = 1 
	                            and H.HR_CHG_NO = @HR_CHG_NO
	                            and H.EMP_ID = @EMP_ID
                                 ");
            ht.Add("@HR_CHG_NO", hr_chg_no);
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                rtnval = dr["HR_CHG_DESC"].ToString();
                //if (dr["HR_CHG_CD"].ToString() == "D04")
                if (hr_chr_CD == "D04")
                {
                    sb = new StringBuilder();
                    sb.AppendLine(@"select J.AFTER_DESC
                                    from TB_H_M_EMP_HR_CHANGE_D J
                                    where J.HR_CHG_NO = @HR_CHG_NO
                                    and J.EMP_ID = @EMP_ID
                                    and J.HR_CHG_ITEM = '05'");
                    dt = dbConn.Query(sb, ht);
                    if (dt.Rows.Count > 0)
                    {
                        jdesc = dt.Rows[0]["AFTER_DESC"].ToString();
                        rtnval += " " + jdesc;
                    }
                    sb = new StringBuilder();
                    sb.AppendLine(@"select K.AFTER_DESC
                                    from TB_H_M_EMP_HR_CHANGE_D K
                                    where K.HR_CHG_NO = @HR_CHG_NO
                                    and K.EMP_ID = @EMP_ID
                                    and K.HR_CHG_ITEM = '08'");
                    dt = dbConn.Query(sb, ht);
                    if (dt.Rows.Count > 0)
                    {
                        kdesc = dt.Rows[0]["AFTER_DESC"].ToString();
                        rtnval += " " + kdesc;
                    }
                }
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //◎兼任
    //取得 <兼任部門>
    //        讀取 部門基本資料檔 D
    //            取得:	D.部門名稱
    //            條件:	D.部門代號 = 明細畫面.部門代號
    //                  且 明細畫面.異動生效日 >= D.生效日期 且 明細畫面.異動生效日 <= D.結束日期
    public string Adjunct_Get_DEPT_NAME(string dept_no, string start_dt)
    {
        try
        {
            string rtnval = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select D.DEPT_NAME
                            from TB_H_M_DEPT D
                            where 1 = 1
	                            and D.DEPT_NO = @DEPT_NO
	                            and @START_DT >= D.START_DT
	                            and @START_DT <= D.END_DT");
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@START_DT", start_dt);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                rtnval = dt.Rows[0]["DEPT_NAME"].ToString();
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }

    }

    //◎兼任
    //取得 <兼任職務> 
    //    讀取 職務檔 P
    //        取得:	P.職務名稱
    //        條件:	P.職務代號 = 明細畫面.職務代號
    //              且 明細畫面.異動生效日 >= D.生效日期 且 明細畫面.異動生效日 <= D.結束日期
    public string Adjunct_Get_PJOB_DESC(string pjob_cd, string start_dt)
    {
        try
        {
            string rtnval = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select P.PJOB_DESC
                            from TB_H_M_PJOB P
                            where 1 = 1
	                            and P.PJOB_CD = @PJOB_CD
	                            and @START_DT >= P.START_DT
	                            and @START_DT <= P.END_DT");
            ht.Add("@PJOB_CD", pjob_cd);
            ht.Add("@START_DT", start_dt);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                rtnval = dt.Rows[0]["PJOB_DESC"].ToString();
            }
            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }

    }

    //◎取得兼任以外的人事異動項目
    //<異動項目>																																																					
    //    讀取 人事異動代碼可異動項目檔 T																																																				
    //        取得:	T.人事異動項目代碼																																															
    //        條件:	T.人事異動代碼 = 明細畫面.人事異動代碼																																															
    //              且 T.使用中 = 'Y'																																																
    //        若讀不到資料，GRID DISABLED不可輸入。																																																			
    //        若讀得到資料，																																																			
    //            每一人事異動項目代碼，																																																		
    //            讀取 共用代碼明細檔 C																																																		
    //                取得: C.代碼名稱																																													
    //                條件:	C.子作業='HC' 且 C.類別='HR_CHG_ITEM'  且 C.IS_VALID='Y' 且 C.代碼=T.人事異動項目代碼																																													
    //        下拉選單顯示: T.人事異動項目代碼-C.代碼名稱
    public DataTable Get_HR_CHG_ITEM_List(string hr_chg_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select T.HR_CHG_ITEM sub_cd, C.SUB_DESC osub_desc, T.HR_CHG_ITEM + ' ' + C.SUB_DESC sub_desc
                            from TB_H_M_HR_CHANGE_CODE_ITEM T
                            left join TB_9_M_COMM_D C
                                on C.SYS_CD = 'HC'
                                and C.MAIN_CD = 'HR_CHG_ITEM'
                                and C.IS_VALID = 'Y'
                                and C.SUB_CD = T.HR_CHG_ITEM
                            where 1 = 1
	                            and T.HR_CHG_CD = @HR_CHG_CD
	                            and T.IS_VALID = 'Y'");
            ht.Add("@HR_CHG_CD", hr_chg_cd);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }

    }

    //取得 gv_result 部門、職務資料
    public DataTable Get_gv_result(string hr_chg_no, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select  ROW_NUMBER() OVER(ORDER BY A.HR_CHG_NO,A.EMP_ID,A.HR_CHG_NO) As RowNumber
                                   ,A.HR_CHG_NO
                                   ,A.EMP_ID
                                   ,A.AFTER_CD as DEPT_NO
                                   ,A.AFTER_DESC as DEPT_NAME
                                   ,B.AFTER_CD as PJOB_CD
                                   ,B.AFTER_DESC as PJOB_DESC		
                            from TB_H_M_EMP_HR_CHANGE_D A
                            left join TB_H_M_EMP_HR_CHANGE_D B
                            on B.HR_CHG_NO = A.HR_CHG_NO
                            and B.EMP_ID = A.EMP_ID
                            and B.HR_CHG_ITEM = '08'
                            where 1 = 1
	                            and A.HR_CHG_NO = @HR_CHG_NO
	                            and A.EMP_ID = @EMP_ID
	                            and A.HR_CHG_ITEM = '05'");
            ht.Add("@HR_CHG_NO", hr_chg_no);
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 gv_result2 異動明細
    public DataTable Get_gv_result2(string hr_chg_no, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select  ROW_NUMBER() OVER(ORDER BY A.HR_CHG_NO,A.EMP_ID,A.HR_CHG_NO) As RowNumber
                                   ,A.HR_CHG_NO
                                   ,A.EMP_ID
                                   ,A.HR_CHG_ITEM
                                   ,A.HR_CHG_ITEM + ' ' + C.SUB_DESC HR_CHG_ITEM_DESC                                   
                                   ,A.BEFORE_CD
                                   ,A.BEFORE_DESC
                                   ,A.AFTER_CD
                                   ,A.AFTER_DESC
                            from TB_H_M_EMP_HR_CHANGE_D A
                            left join TB_9_M_COMM_D C
                                on C.SYS_CD = 'HC'
                                and C.MAIN_CD = 'HR_CHG_ITEM'
                                --and C.IS_VALID = 'Y'
                                and C.SUB_CD = A.HR_CHG_ITEM
                            where 1 = 1
	                            and A.HR_CHG_NO = @HR_CHG_NO
	                            and A.EMP_ID = @EMP_ID ");
            ht.Add("@HR_CHG_NO", hr_chg_no);
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為 '01-聘用單位' 之異動前代碼和說明 
    //(1.1)E.聘用單位 顯示於 異動前代碼；讀取 共用代碼明細檔 以 子作業='HB' 且 類別='COMPANY_CD' 且 代碼=E.聘用單位  取得 代碼說明，顯示於 異動前代碼說明。
    //若有同仁有使用 共用代碼檔的   HB-COMPANY_CD(聘用單位)，來取得 聘用單位 的名稱及代碼，請改用TB_H_M_COMPANY(公司資料檔)的COMPANY_CD   (公司代號)   及  COMPANY_SNAME(公司簡稱)。
    //共用代碼檔的  HB-COMPANY_CD(聘用單位)將會被移除。
    public DataTable Get_HR_CHG_ITEM_01_BEFORE(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select E.COMPANY_CD sub_cd, C.COMPANY_SNAME sub_desc
                            from VW_H_EMP_DATA E
                            left join TB_H_M_COMPANY C
                                on C.COMPANY_CD = E.COMPANY_CD
                            where 1 = 1	                            
	                            and EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為 '01-聘用單位' 之異動後代碼和說明 
    //    (1.2)若直接輸入異動後代碼，
    //                讀取 共用代碼明細檔 C
    //                        取得:	C.代碼名稱
    //                        條件:	C.子作業='HB' 且 C.類別='COMPANY_CD'  且 C.IS_VALID='Y' 且 C.代碼=明細畫面.異動後代碼
    //                將C.代碼名稱，顯示於異動後代碼說明。
    //若有同仁有使用 共用代碼檔的   HB-COMPANY_CD(聘用單位)，來取得 聘用單位 的名稱及代碼，請改用TB_H_M_COMPANY(公司資料檔)的COMPANY_CD   (公司代號)   及  COMPANY_SNAME(公司簡稱)。
    //共用代碼檔的  HB-COMPANY_CD(聘用單位)將會被移除。
    public DataTable Get_HR_CHG_ITEM_01_AFTER(string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select C.COMPANY_CD sub_cd, C.COMPANY_SNAME sub_desc
                            from TB_H_M_COMPANY C                            
                            where 1 = 1	                            
	                            and C.COMPANY_CD = @COMPANY_CD ");
            ht.Add("@COMPANY_CD", company_cd);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'02-工廠區分' 之異動前代碼和說明 
    //(2.1)E.工廠區分 顯示於 異動前代碼；讀取 共用代碼明細檔 以 子作業='HB' 且 類別='PLANT_CD' 且 代碼=E.工廠區分  取得 代碼說明，顯示於 異動前代碼說明。    
    public DataTable Get_HR_CHG_ITEM_02_BEFORE(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select E.PLANT_CD sub_cd, C.SUB_DESC sub_desc
                            from VW_H_EMP_DATA E
                            left join TB_9_M_COMM_D C
                                on C.SYS_CD = 'HB'
                                and C.MAIN_CD = 'PLANT_CD'
                                and C.SUB_CD = E.PLANT_CD
                            where 1 = 1	                            
	                            and EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'02-工廠區分' 之異動後代碼和說明 
    //(2.2)若直接輸入異動後代碼，
    //            讀取 共用代碼明細檔 C
    //                    取得:	C.代碼名稱
    //                    條件:	C.子作業='HB' 且 C.類別='PLANT_CD'  且 C.IS_VALID='Y' 且 C.代碼=明細畫面.異動後代碼
    //            將C.代碼名稱，顯示於異動後代碼說明。
    //            若讀不到，顯示錯誤訊息"工廠區分不存在"。 
    public DataTable Get_HR_CHG_ITEM_02_AFTER(string plant_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select C.SUB_CD sub_cd, C.SUB_DESC sub_desc
                            from TB_9_M_COMM_D C                            
                            where 1 = 1
                                and C.SYS_CD = 'HB'
                                and C.MAIN_CD = 'PLANT_CD'
                                and C.IS_VALID='Y'
	                            and C.SUB_CD = @PLANT_CD ");
            ht.Add("@PLANT_CD", plant_cd);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'03-職種' 之異動前代碼和說明 
    //(3.1)E.職種 顯示於 異動前代碼；讀取 共用代碼明細檔 以 子作業='HB' 且 類別='WS_CD' 且 代碼=E.職種  取得 代碼說明，顯示於 異動前代碼說明。
    public DataTable Get_HR_CHG_ITEM_03_BEFORE(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select E.WS_CD sub_cd, C.SUB_DESC sub_desc
                            from VW_H_EMP_DATA E
                            left join TB_9_M_COMM_D C
                                on C.SYS_CD = 'HB'
                                and C.MAIN_CD = 'WS_CD'
                                and C.SUB_CD = E.WS_CD
                            where 1 = 1	                            
	                            and EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'03-職種' 之異動後代碼和說明 
    //(3.2)若直接輸入異動後代碼，
    //            讀取 共用代碼明細檔 C
    //                    取得: C.代碼名稱
    //                    條件:	C.子作業='HB' 且 C.類別='WS_CD'  且 C.IS_VALID='Y' 且 C.代碼=明細畫面.異動後代碼
    //            將C.代碼名稱，顯示於異動後代碼說明。
    //            若讀不到，顯示錯誤訊息"職種不存在"。
    public DataTable Get_HR_CHG_ITEM_03_AFTER(string ws_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select C.SUB_CD sub_cd, C.SUB_DESC sub_desc
                            from TB_9_M_COMM_D C                            
                            where 1 = 1
                                and C.SYS_CD = 'HB'
                                and C.MAIN_CD = 'WS_CD'
                                and C.IS_VALID='Y'
	                            and C.SUB_CD = @WS_CD ");
            ht.Add("@WS_CD", ws_cd);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'04-員工區分' 之異動前代碼和說明 
    //(4.1)E.員工區分 顯示於 異動前代碼；讀取 共用代碼明細檔 以 子作業='HB' 且 類別='EMP_CD' 且 代碼=E.員工區分  取得 代碼說明，顯示於 異動前代碼說明。
    public DataTable Get_HR_CHG_ITEM_04_BEFORE(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select E.EMP_CD sub_cd, C.SUB_DESC sub_desc
                            from VW_H_EMP_DATA E
                            left join TB_9_M_COMM_D C
                                on C.SYS_CD = 'HB'
                                and C.MAIN_CD = 'EMP_CD'
                                and C.SUB_CD = E.EMP_CD
                            where 1 = 1	                            
	                            and EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'04-員工區分' 之異動後代碼和說明 
    //4.若<異動項目> 為'04-員工區分'，
    //(4.2)若直接輸入異動後代碼，
    //            讀取 共用代碼明細檔 C
    //                    取得:	C.代碼名稱
    //                    條件:	C.子作業='HB' 且 C.類別='EMP_CD'  且 C.IS_VALID='Y' 且 C.代碼=明細畫面.異動後代碼
    //            將C.代碼名稱，顯示於異動後代碼說明。
    //            若讀不到，顯示錯誤訊息"職種不存在"。
    public DataTable Get_HR_CHG_ITEM_04_AFTER(string emp_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select C.SUB_CD sub_cd, C.SUB_DESC sub_desc
                            from TB_9_M_COMM_D C                            
                            where 1 = 1
                                and C.SYS_CD = 'HB'
                                and C.MAIN_CD = 'EMP_CD'
                                and C.IS_VALID='Y'
	                            and C.SUB_CD = @EMP_CD ");
            ht.Add("@EMP_CD", emp_cd);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'05-部門' 之異動前代碼和說明 
    //(5.1)E.部門代號 顯示於 異動前代碼；E.部門名稱，顯示於 異動前代碼說明。
    public DataTable Get_HR_CHG_ITEM_05_BEFORE(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select E.DEPT_NO sub_cd, E.DEPT_NAME sub_desc
                            from VW_H_EMP_DATA E                            
                            where 1 = 1	                            
	                            and EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'05-部門' 之異動後代碼和說明 
    //(5.2)若直接輸入異動後代碼，
    //            讀取 部門基本資料檔 D
    //                    取得:	D.部門名稱, D.部門層級
    //                    條件:	D.部門代號 = 明細畫面.部門代號
    //                                    且 明細畫面.異動生效日 >= D.生效日期 且 明細畫面.異動生效日 <= D.結束日期
    //                                    若 資料權限之「部門含以下」或「部門權限」任一不為空值，
    //                                        加入條件：且 D.部門代號 必須存在以下 該擔當有權限作業的部門清單中，
    //                                        UNION 以下兩者的部門清單，
    //                                        若 資料權限之「部門含以下」為Y，
    //                                                    讀取 主管可管理部門資料檔 D
    //                                                        取得: D.可管理部門代號
    //                                                        條件:	D.工號 = 登入者帳號
    //                                        若 資料權限之「部門權限」不為空值，
    //                                                    「部門權限」的內容。
    //            若讀得到，
    //                若 資料權限之「小分類」為W(各單位擔當)，
    //                    讀取 部門層級檔 L
    //                        取得:	MAX(L.部門層級)
    //                        條件:	L.層級屬性代碼 = 'H'    --「人事管理層級」
    //                    若 D.部門層級 <= MAX(L.部門層級)，顯示錯誤訊息"各單位只能輸入「課」(不含)以下的單位異動"
    //                否則，
    //                    將D.部門名稱，顯示於異動後代碼說明。
    //            若讀不到，顯示錯誤訊息"部門代號不存在，或無權限作業"。
    public DataTable Get_HR_CHG_ITEM_05_AFTER(string dept_no, string start_dt,string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select D.DEPT_NO sub_cd, D.DEPT_NAME sub_desc, D.DEPT_LEVEL, L.MAX_DEPT_LEVEL, D.DEFAULT_PLANT 
                            from TB_H_M_DEPT D
                            left join 
                                (select 	MAX(L.DEPT_LEVEL) MAX_DEPT_LEVEL
                                 from TB_H_M_DEPT_LEVEL L
                                 where L.LEVEL_TYPE = 'H') L
                                on 1 = 1
                            where 1 = 1
                                and D.DEPT_NO = @DEPT_NO
                                and @START_DT >= D.START_DT
	                            and @START_DT <= D.END_DT ");
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@START_DT", start_dt);
            //顯示資料權限設定,若不為super user(以部門為主)
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND D.DEPT_NO IN(	 select DEPT_NO from dbo.FN_H_GET_AUTH_DEPT(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }
            /*
            if (!strIsSuper)
            {
                sb.Append(@" AND ( ");
                sb.AppendLine(@" D.DEPT_NO in ( "
                             + " select MNG_DEPT_NO from TB_H_R_HEAD_DEPT where emp_id=@LOGIN_ID2 ");
                sb.AppendLine(" ) ");
                ht.Add("@LOGIN_ID2", SessionHandle.Current.emp_id);
                if (strDepartments != "" && strDepartments != "N")
                {
                    //若 資料權限之「部門權限」不為空值
                    sb.Append(" or  D.DEPT_NO in ( @uDEPT_NO) ");
                    ht.Add("@uDEPT_NO", strDepartments.Replace(" ","").Split(','));
                }
                sb.Append(" ) ");
            }
            */
           
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable checkDefaultPlant(string dept_no, string start_dt, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"declare @dept_no_old varchar  ;
                            declare @plant_cd_old varchar ;
                            declare @dept_no_new varchar  ;
                            declare @plant_cd_new varchar ;

                            select @dept_no_old=DEPT_NO,@plant_cd_old=PLANT_CD 
                            from TB_H_M_EMP    where emp_id=@EMP_ID

                            select @dept_no_new=DEPT_NO,@plant_cd_new=DEFAULT_PLANT 
                            from TB_H_M_DEPT   where DEPT_NO=@DEPT_NO
                            and @START_DT >= START_DT and @START_DT <= END_DT

                            if @plant_cd_new='3' or @plant_cd_old =@plant_cd_new
                            select '0' resultCount
                            else if	 @plant_cd_old != @plant_cd_new
                            select '1'	resultCount "
                         );
            ht.Add("@EMP_ID",emp_id);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@START_DT", start_dt);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'06-資格' 之異動前代碼和說明 
    //(6.1)E.資格代號 顯示於 異動前代碼；NULL，顯示於 異動前代碼說明。
    public DataTable Get_HR_CHG_ITEM_06_BEFORE(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select E.LEVEL_CD sub_cd, 'NULL' sub_desc
                            from VW_H_EMP_DATA E                            
                            where 1 = 1	                            
	                            and EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'06-資格' 之異動後代碼和說明 
    //(6.2)若直接輸入異動後代碼，
    //            讀取 資格檔 L
    //                    條件:	L.資格代號=明細畫面.異動後代碼
    //                          且 明細畫面.異動生效日 >= L.生效日期 且 明細畫面.異動生效日 <= L.結束日期
    //            異動後代碼說明顯示NULL。
    //            若讀不到，顯示錯誤訊息"資格代號不存在"。
    public DataTable Get_HR_CHG_ITEM_06_AFTER(string level_cd, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select L.LEVEL_CD sub_cd, 'NULL' sub_desc
                            from TB_H_M_LEVEL L                            
                            where 1 = 1
                                and L.LEVEL_CD = @LEVEL_CD 
                                and @START_DT >= L.START_DT
	                            and @START_DT <= L.END_DT ");
            ht.Add("@LEVEL_CD", level_cd);
            ht.Add("@START_DT", start_dt);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'07-級數' 之異動前代碼和說明 
    //(7.1)E.級數代號 顯示於 異動前代碼；NULL，顯示於 異動前代碼說明。
    public DataTable Get_HR_CHG_ITEM_07_BEFORE(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select E.GRADE_CD sub_cd, 'NULL' sub_desc
                            from VW_H_EMP_DATA E                            
                            where 1 = 1	                            
	                            and EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'07-級數' 之異動後代碼和說明 
    //(7.2)若直接輸入異動後代碼，
    //            讀取 資格級數檔 LG
    //                    條件:	LG.級數代碼=明細畫面.異動後代碼
    //                          且 使用中 = 'Y'
    //                          若有輸入 <異動項目>'06-資格'，則加入條件: LG.資格代號=異動項目:'06-資格' 之異動後代碼
    //                          若未輸入 <異動項目>'06-資格'，則加入條件: LG.資格代號=E.資格代號
    //            異動後代碼說明顯示NULL。
    //            若讀不到，顯示錯誤訊息"級數代號不存在"。
    public DataTable Get_HR_CHG_ITEM_07_AFTER(string grade_cd, string level_cd, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select LG.GRADE_CD sub_cd, 'NULL' sub_desc
                            from TB_H_M_LEVEL_GRADE LG                                                                                                       
                            where 1 = 1
                                and LG.GRADE_CD = @GRADE_CD
                                and LG.IS_VALID = 'Y' ");
            if (level_cd.Length > 0)
            {
                sb.AppendLine(" and LG.LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }
            else
            {
                sb.AppendLine(@" and LG.LEVEL_CD = (select LEVEL_CD
                                                    from VW_H_EMP_DATA E 
                                                    where E.EMP_ID = @EMP_ID) ");
                ht.Add("@EMP_ID", emp_id);
            }
            ht.Add("@GRADE_CD", grade_cd);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //因一括異動, 異動人員在最後儲存前才能確認, 故在取說明時, 改以下處理
    //取得異動項目07-異動後代碼說明
    //7.若<異動項目> 為'07-級數'，
    //(7.2)若直接輸入異動後代碼，
    //            讀取 資格級數檔 LG
    //                    條件:	LG.級數代碼=明細畫面.異動後代碼
    //                          且 使用中 = 'Y'
    //            異動後代碼說明顯示NULL。
    //            若讀不到，顯示錯誤訊息"級數代號不存在"。
    public DataTable Get_Add_batch_HR_CHG_ITEM_07_AFTER(string grade_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select top 1 LG.GRADE_CD sub_cd, 'NULL' sub_desc
                            from TB_H_M_LEVEL_GRADE LG                                                                                                       
                            where 1 = 1
                                and LG.GRADE_CD = @GRADE_CD
                                and LG.IS_VALID = 'Y' ");
            ht.Add("@GRADE_CD", grade_cd);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'08-職務' 之異動前代碼和說明 
    //(8.1)E.職務代號 顯示於 異動前代碼；E.職務名稱，顯示於 異動前代碼說明。 
    public DataTable Get_HR_CHG_ITEM_08_BEFORE(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select E.PJOB_CD sub_cd, E.PJOB_DESC sub_desc
                            from VW_H_EMP_DATA E                            
                            where 1 = 1	                            
	                            and EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable Get_HR_CHG_ITEM_08_AFTER_NEW_LEVEL(string pjob_cd, string start_dt, string new_level_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select P.PJOB_CD sub_cd, P.PJOB_DESC sub_desc
                            from TB_H_M_PJOB P                            
                            where 1 = 1
                            and P.PJOB_CD = @PJOB_CD 
                            and P.LEVEL_CD = @LEVEL_CD
                            and @START_DT >= P.START_DT
	                        and @START_DT <= P.END_DT 
                            ");

            ht.Add("@PJOB_CD", pjob_cd);
            ht.Add("@START_DT", start_dt);
            ht.Add("@LEVEL_CD", new_level_cd);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'08-職務' 之異動後代碼和說明 
    //(8.2)若直接輸入異動後代碼，
    //            讀取 職務檔 P
    //                    取得: P.職務名稱
    //                    條件:	P.職務代號 = 明細畫面.異動後代碼
    //                          且 P.職種 = E.職種
    //                          且 P.資格代號 = E.資格代號
    //                          且 明細畫面.異動生效日 >= D.生效日期 且 明細畫面.異動生效日 <= D.結束日期
    //            將P.職務名稱，顯示於異動後代碼說明。
    //            若讀不到，顯示錯誤訊息"職務代號不存在"。
    public DataTable Get_HR_CHG_ITEM_08_AFTER(string pjob_cd, string start_dt, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select P.PJOB_CD sub_cd, P.PJOB_DESC sub_desc
                            from TB_H_M_PJOB P
                            inner join VW_H_EMP_DATA E
                                on E.EMP_ID = @EMP_ID
                                and P.WS_CD = E.WS_CD
                                and P.LEVEL_CD = E.LEVEL_CD
                            where 1 = 1
                                and P.PJOB_CD = @PJOB_CD                                
                                and @START_DT >= P.START_DT
	                            and @START_DT <= P.END_DT ");
            ht.Add("@PJOB_CD", pjob_cd);
            ht.Add("@START_DT", start_dt);
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //因一括異動, 異動人員在最後儲存前才能確認, 故在取說明時, 改以下處理
    //取得異動項目08-異動後代碼說明
    //8.若<異動項目> 為'08-職務'，
    //(8.2)若直接輸入異動後代碼，
    //            讀取 VW_TB_H_M_PJOB P
    //                    取得: P.職務名稱
    //                    條件:	P.職務代號 = 明細畫面.異動後代碼   
    //            將P.職務名稱，顯示於異動後代碼說明。
    //            若讀不到，顯示錯誤訊息"職務代號不存在"。
    public DataTable Get_Add_batch_HR_CHG_ITEM_08_AFTER(string pjob_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select P.PJOB_CD sub_cd, P.PJOB_DESC sub_desc
                            from VW_TB_H_M_PJOB P                            
                            where 1 = 1
                                and P.PJOB_CD = @PJOB_CD ");

            ht.Add("@PJOB_CD", pjob_cd);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'09-輪值表' 之異動前代碼和說明 
    //(9.1)E.輪值表代碼 顯示於 異動前代碼；E.輪值表說明，顯示於 異動前代碼說明。
    public DataTable Get_HR_CHG_ITEM_09_BEFORE(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select E.WORK_SHIFT_CD sub_cd, E.WORK_SHIFT_DESC sub_desc
                            from VW_H_EMP_DATA E                            
                            where 1 = 1	                            
	                            and EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'09-輪值表' 之異動後代碼和說明 
    //(9.2)若直接輸入異動後代碼，
    //            讀取 輪值表主檔 W
    //                    取得:	W.輪值表說明
    //                    條件:	W.IS_VALID='Y'
    //                          且 W.輪值表代碼=明細畫面.異動後代碼
    //                    若 資料權限之「小分類」為W(各單位擔當)，加入以下條件，
    //                                且 W.輪值表代碼 EXISTS (讀取 共用代碼明細檔 CD
    //                                          取得:CD.代碼
    //                                          條件:CD.子作業='HC' 且 CD.類別='WORKER_WORK_SHIFT' 且 CD.IS_VALID = 'Y')
    //            若讀不到，顯示錯誤訊息"輪值表代碼不存在"，
    //            若讀得到，將W.輪值表說明，顯示於異動後代碼說明。
    public DataTable Get_HR_CHG_ITEM_09_AFTER(string work_shift_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select W.WORK_SHIFT_CD sub_cd, W.WORK_SHIFT_DESC sub_desc
                            from TB_D_M_WORK_SHIFT_H W                            
                            where 1 = 1
                                and W.IS_VALID = 'Y'
                                and W.WORK_SHIFT_CD = @WORK_SHIFT_CD ");

            ht.Add("@WORK_SHIFT_CD", work_shift_cd);
            if (strSysCodeAtt == "W")
            {
                sb.AppendLine(@" and WORK_SHIFT_CD in (select sub_cd
                                                       from TB_9_M_COMM_D CD 
                                                       where CD.SYS_CD='HC' 
                                                         and CD.MAIN_CD='WORKER_WORK_SHIFT' 
                                                         and CD.IS_VALID = 'Y') ");
            }
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'10-工數區分' 之異動前代碼和說明 
    //(10.1)E.工數區分 顯示於 異動前代碼；讀取 共用代碼明細檔 以 子作業='HB' 且 類別='WORK_CD' 且 代碼=E.工數區分  取得 代碼說明，顯示於 異動前代碼說明。 
    public DataTable Get_HR_CHG_ITEM_10_BEFORE(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select E.WORK_CD sub_cd, C.SUB_DESC sub_desc
                            from VW_H_EMP_DATA E
                            left join TB_9_M_COMM_D C
                                on C.SYS_CD = 'HB'
                                and C.MAIN_CD = 'WORK_CD'
                                and C.SUB_CD = E.WORK_CD                     
                            where 1 = 1	                            
	                            and EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 <異動項目> 為'10-工數區分' 之異動後代碼和說明 
    //(10.2)若直接輸入異動後代碼，
    //            讀取 共用代碼明細檔 C
    //                    取得:	C.代碼名稱
    //                    條件:	C.子作業='HB' 且 C.類別='WORK_CD'  且 C.IS_VALID='Y' 且 C.代碼=明細畫面.異動後代碼
    //            將C.代碼名稱，顯示於異動後代碼說明。
    //            若讀不到，顯示錯誤訊息"工數區分不存在"。
    public DataTable Get_HR_CHG_ITEM_10_AFTER(string work_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select C.SUB_CD sub_cd, C.SUB_DESC sub_desc
                            from TB_9_M_COMM_D C                            
                            where 1 = 1
                                and C.SYS_CD = 'HB'
                                and C.MAIN_CD = 'WORK_CD'
                                and C.IS_VALID = 'Y'
                                and C.SUB_CD = @WORK_CD ");

            ht.Add("@WORK_CD", work_cd);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //(2)取得人事異動編號：
    //◎若人事異動代碼，非'B06'(兼任)，
    //    則 取一個 WK_人事異動編號 
    //◎若人事異動代碼，為'B06'(兼任)，
    //    則 GRID每一筆資料，就給一個 WK_人事異動編號
    public ArrayList Get_HR_CHG_NO(string emp_id, string hr_chg_cd, string start_dt, int gv_result_Rows_Count)
    {
        ArrayList data = new ArrayList();
        try
        {
            //兼任 則 GRID每一筆資料，就給一個 WK_人事異動編號
            if (hr_chg_cd == "B06")
            {
                for (int i = 0; i < gv_result_Rows_Count; i++)
                {
                    data.Add(Get_HR_CHG_NO(start_dt));
                }
            }
            //非兼任 & D04(結束兼任) 則 取一個 WK_人事異動編號 
            else
            {
                data.Add(Get_HR_CHG_NO(start_dt));
            }
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //    讀取 自動給號控制檔 A
    //        取得:	A.流水號(SERIAL_NO)
    //        條件:	A.給號類別='HR_CHG_NO'
    //              且 A.給號日期=明細畫面.異動生效日
    //        若讀不到，
    //            WK_人事異動編號=明細畫面.異動生效日(格式:YYYYMMDD)+'0001'
    //            UPDATE 自動給號控制檔 SET 流水號=2
    //                    條件:	A.給號類別='HR_CHG_NO'
    //                          且 A.給號日期=明細畫面.異動生效日
    //        若讀得到，
    //            WK_人事異動編號=明細畫面.異動生效日(格式:YYYYMMDD)+A.流水號(前置0補足4碼)
    //            UPDATE 自動給號控制檔 SET 流水號=流水號+1
    //                    條件:	A.給號類別='HR_CHG_NO'
    //                          且 A.給號日期=明細畫面.異動生效日
    public string Get_HR_CHG_NO(string start_dt)
    {
        try
        {
            string rtnvalue = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select Convert(varchar(8),Convert(datetime,@AUTO_NUMBER_DT),112) + 
	                            isnull(
		                            (
			                            select right('0000' + convert(varchar(4),A.SERIAL_NUMBER),4)
			                            from TB_H_M_AUTO_NUMBER_CTL A
			                            where A.AUTO_NUMBER_TYPE = 'HR_CHG_NO'
			                            and A.AUTO_NUMBER_DT = @AUTO_NUMBER_DT
		                            ),'0001') HR_CHG_NO ");

            ht.Add("@AUTO_NUMBER_DT", start_dt);
            DataTable dt = dbConn.QueryT(sb, ht);
            if (dt.Rows.Count > 0)
            {
                rtnvalue = dt.Rows[0]["HR_CHG_NO"].ToString();
                sb = new StringBuilder();
                ht = new Hashtable();
                sb.AppendLine(@"
                    if exists(select right('0000' + convert(varchar(4),A.SERIAL_NUMBER),4)
			                  from TB_H_M_AUTO_NUMBER_CTL A
			                  where A.AUTO_NUMBER_TYPE = 'HR_CHG_NO'
			                  and A.AUTO_NUMBER_DT = @AUTO_NUMBER_DT
	                         )
	                    update TB_H_M_AUTO_NUMBER_CTL set SERIAL_NUMBER = SERIAL_NUMBER + 1
                        where AUTO_NUMBER_TYPE = 'HR_CHG_NO'
			            and AUTO_NUMBER_DT = @AUTO_NUMBER_DT;
                    else
	                    insert into TB_H_M_AUTO_NUMBER_CTL 
                                (AUTO_NUMBER_TYPE,AUTO_NUMBER_DT,SERIAL_NUMBER) 
                            values 
                                ('HR_CHG_NO',@AUTO_NUMBER_DT,2);
                ");
                ht.Add("@AUTO_NUMBER_DT", start_dt);
                dbConn.ExecuteT(sb, ht);
            }
            return rtnvalue;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //(3)以 (1)取得的人事異動編號+明細畫面.工號 讀取 人事異動主檔， 如資料存在，則顯示錯誤訊息"人事異動編號+工號重覆"。
    public string checkHR_CHG_NO(ArrayList data, string emp_id)
    {
        string rtnvalue = "";
        try
        {
            foreach (string hr_chg_no in data)
            {
                StringBuilder sb = new StringBuilder();
                Hashtable ht = new Hashtable();
                sb.AppendLine(@"select count(*) as cnt
                                from TB_H_M_EMP_HR_CHANGE_H 
                                where HR_CHG_NO = @HR_CHG_NO
                                and EMP_ID = @EMP_ID ");
                ht.Add("@HR_CHG_NO", hr_chg_no);
                ht.Add("@EMP_ID", emp_id);
                DataTable dt = dbConn.Query(sb, ht);
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt16(dt.Rows[0]["cnt"]) > 0)
                    {
                        rtnvalue = Resources.Resource.wfb2hc_HR_CHG_NO_and_EMP_ID_repeat;
                        break;
                    }
                }
            }
            return rtnvalue;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //資料檢核：
    //員工清單】有勾選的工號，
    //以 (1)取得的人事異動編號+E.工號 讀取 人事異動主檔， 如資料存在，則顯示錯誤訊息"人事異動編號：XXXXXXXXXXXX，工號：XXXXX 資料重覆"。
    public string check_Add_batch_HR_CHG_NO(ArrayList data, List<string> emp_ids)
    {
        string rtnvalue = "";
        try
        {
            foreach (string hr_chg_no in data)
            {
                foreach (string emp_id in emp_ids)
                {
                    StringBuilder sb = new StringBuilder();
                    Hashtable ht = new Hashtable();
                    sb.AppendLine(@"select count(*) as cnt
                                from TB_H_M_EMP_HR_CHANGE_H 
                                where HR_CHG_NO = @HR_CHG_NO
                                and EMP_ID = @EMP_ID ");
                    ht.Add("@HR_CHG_NO", hr_chg_no);
                    ht.Add("@EMP_ID", emp_id);
                    DataTable dt = dbConn.Query(sb, ht);
                    if (dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt16(dt.Rows[0]["cnt"]) > 0)
                        {
                            if (rtnvalue != "") rtnvalue += "\\n";
                            rtnvalue = string.Format(Resources.Resource.wfb2hc_HR_CHG_NO_and_EMP_ID_repeat_for_Add_batch, hr_chg_no, emp_id);
                        }
                    }
                }
            }
            return rtnvalue;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //(4)◎若人事異動代碼，非'B06'(兼任)，
    //	  則 取一個 WK_序號 																												
    //◎若人事異動代碼，為'B06'(兼任)，
    //    則 GRID每一筆資料，就給一個 WK_序號，下一筆累加1。
    public ArrayList Get_CHG_SEQ(string emp_id, string hr_chg_cd, string start_dt, int gv_result_Rows_Count)
    {
        ArrayList data = new ArrayList();
        try
        {
            //兼任 則 GRID每一筆資料，就給一個 WK_序號，下一筆累加1。
            if (hr_chg_cd == "B06")
            {
                for (int i = 0; i < gv_result_Rows_Count; i++)
                {
                    if (i == 0) data.Add(Get_CHG_SEQ(emp_id, start_dt));
                    else data.Add(Convert.ToInt16(data[i - 1]) + 1);
                }
            }
            //非兼任 & D04(結束兼任) 則 取一個 WK_序號 
            else
            {
                data.Add(Get_CHG_SEQ(emp_id, start_dt));
            }
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //    讀取 人事異動主檔 A
    //        取得:	MAX(A.序號)
    //        條件:	A.工號=明細畫面.工號
    //              且 A.人事異動生效日=明細畫面.異動生效日
    //        若讀不到，
    //              WK_序號=1
    //        若讀得到，
    //              WK_序號=MAX(A.序號)+1	
    public int Get_CHG_SEQ(string emp_id, string start_dt)
    {
        try
        {
            int rtnvalue = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select 
	                            isnull(
		                            (
			                            select max(A.CHG_SEQ)+1
			                            from TB_H_M_EMP_HR_CHANGE_H A
			                            where A.EMP_ID = @EMP_ID
			                            and A.START_DT = @START_DT
		                            ),1) CHG_SEQ ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@START_DT", start_dt);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                rtnvalue = Convert.ToInt16(dt.Rows[0]["CHG_SEQ"]);
            }
            return rtnvalue;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //1.讀取 人事異動主檔 A
    //條件: A.人事異動編號 = 畫面.人事異動編號
    //      且 A.工號 = 畫面.工號
    //JOIN 員工人事資料VIEW(VW_H_EMP_DATA) E，條件:E.工號 = A.工號
    //JOIN 人事異動代碼檔 G，條件:G.人事異動代碼 = A.人事異動代碼
    public void Get_Master_Data(string hr_chg_no, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select   A.HR_CHG_NO
                                    ,A.EMP_ID
                                    ,E.EMP_NAME
                                    ,A.START_DT
									,A.HR_CHG_CD
									,G.HR_CHG_DESC
									,A.INS_PLAN_PROC_DT
									,A.PLAN_END_DT
									,A.IS_END
									,A.MAIN_HR_CHG_NO
									,A.HR_CHG_PROC_STATUS
									,C.SUB_DESC HR_CHG_PROC_STATUS_DESC
									,A.ICT_TYPE
									,A.TRANSFER_NATION_CD
									,A.TRANSFER_COMPANY_CD
									,A.TRANSFER_DEPT
									,A.IS_PAY_SUBSIST
                            from TB_H_M_EMP_HR_CHANGE_H A
                            left join VW_H_EMP_DATA E
                                on E.EMP_ID = A.EMP_ID
                            left join TB_H_M_HR_CHANGE_CODE G
                                on G.HR_CHG_CD = A.HR_CHG_CD
                            left join TB_9_M_COMM_D C
								on C.SYS_CD = 'HC'
								and C.MAIN_CD = 'HR_CHG_PROC_STATUS'
								and C.SUB_CD = A.HR_CHG_PROC_STATUS
                            where A.HR_CHG_NO = @HR_CHG_NO
                                and A.EMP_ID = @EMP_ID ");
            ht.Add("@HR_CHG_NO", hr_chg_no);
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                HR_CHG_NO = new ArrayList();
                HR_CHG_NO.Add(dr["HR_CHG_NO"].ToString());
                HR_CHG_CD = dr["HR_CHG_CD"].ToString();
                HR_CHG_DESC = dr["HR_CHG_DESC"].ToString();
                EMP_ID = dr["EMP_ID"].ToString();
                EMP_NAME = dr["EMP_NAME"].ToString();
                HR_CHG_PROC_STATUS_DESC = dr["HR_CHG_PROC_STATUS_DESC"].ToString();
                START_DT = String.Format("{0:yyyy/MM/dd}", dr["START_DT"]);
                //CHG_SEQ
                INS_PLAN_PROC_DT = (String.Format("{0:yyyy/MM/dd}", dr["INS_PLAN_PROC_DT"]) == "1900/01/01") ? "" : String.Format("{0:yyyy/MM/dd}", dr["INS_PLAN_PROC_DT"]);
                PLAN_END_DT = (String.Format("{0:yyyy/MM/dd}", dr["PLAN_END_DT"]) == "1900/01/01") ? "" : String.Format("{0:yyyy/MM/dd}", dr["PLAN_END_DT"]);
                //END_HR_CHG_NO 
                IS_END = dr["IS_END"].ToString();
                MAIN_HR_CHG_NO = dr["MAIN_HR_CHG_NO"].ToString();
                ICT_TYPE = dr["ICT_TYPE"].ToString();
                TRANSFER_NATION_CD = dr["TRANSFER_NATION_CD"].ToString();
                TRANSFER_COMPANY_CD = dr["TRANSFER_COMPANY_CD"].ToString();
                TRANSFER_DEPT = dr["TRANSFER_DEPT"].ToString();
                IS_PAY_SUBSIST = dr["IS_PAY_SUBSIST"].ToString();
                
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    //2.(生效處理清單)讀取 人事異動主檔  A
    //  JOIN 員工人事主檔(TB_H_M_EMP) E，條件: E.工號 = A.工號
    //  條件: A.生效處理狀態 <> 'Y'
    //        且 (A.保險預計處理日 <= 系統日 或 A.人事異動生效日 <= 系統日)
    //        若 資料權限之「小分類」為N(管理部擔當)，
    //          加入條件：且 A.人事異動代碼 必須存在以下清單中，
    //              讀取 人事異動代碼檔 G
    //                  取得: G.人事異動代碼
    //                  條件: G.使用中 = 'Y'
    //                     且 G.人事異動代碼 必須存在於  (讀取 人事異動代碼擔當檔 F
    //                                取得: F.人事異動代碼
    //                                條件:	F.工號 = 登入者帳號 且 F.使用中 = 'Y')
    //        若 資料權限之「小分類」為W(各單位擔當)，
    //          加入條件：且 A.人事異動代碼 必須存在以下清單中，
    //              讀取 人事異動代碼檔 G
    //                  取得: G.人事異動代碼
    //                  條件: G.使用中 = 'Y'
    //                     且 G.權限區分 = 'D'
    //        若 資料權限之「部門含以下」或「部門權限」任一不為空值，
    //          加入條件：且 E.部門代號 必須存在以下 該擔當有權限作業的部門清單中，
    //                    UNION 以下兩者的部門清單，
    //                    若 資料權限之「部門含以下」為Y，
    //                      讀取 主管可管理部門資料檔 D
    //                          取得: D.可管理部門代號
    //                          條件: D.工號 = 登入者帳號
    //                    若 資料權限之「部門權限」不為空值，
    //                         「部門權限」的內容。
    //    每一 A.人事異動編號+A.工號，
    //        讀取 人事異動明細檔 B
    //            條件: B.人事異動編號 = A.人事異動編號
    //               且 B.工號 = A.工號
    //        將相同 A.人事異動編號+A.工號的 人事異動明細檔資料，以一列顯示。
    //        取得 符合畫面條件的資料
    public DataTable getData_EffectProc(int startRowIndex, int maximumRows, string sortExpression)
    {
        try
        {
            if (sortExpression == "")
                sortExpression = "HR_CHG_NO";

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"declare @sysdate datetime;
                            set @sysdate = convert( varchar(10), getdate(),111); ");
            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* ");
            sb.AppendLine("       from(");
            //2.讀取 人事異動主檔  A
            //  JOIN 員工人事主檔(TB_H_M_EMP) E，條件: E.工號 = A.工號
            //  條件: A.生效處理狀態 <> 'Y'
            //        且 (A.保險預計處理日 <= 系統日 或 A.人事異動生效日 <= 系統日)
            sb.AppendLine(@"        select   A.HR_CHG_NO
		                                    ,A.HR_CHG_CD						
		                                    ,A.HR_CHG_CD + ' ' + ISNULL(CD.HR_CHG_DESC,'') AS HR_CHG_CD_DESC
		                                    ,A.START_DT	
                                            ,A.INS_PLAN_PROC_DT
		                                    ,A.EMP_ID
		                                    ,E.EMP_NAME
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '05' THEN B.AFTER_CD + ' ' + B.AFTER_DESC
			                                        ELSE ''
		                                        END) AF_DEPT_NO_DESC
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '06' THEN B.AFTER_CD
			                                        ELSE ''
		                                        END) AF_LEVEL_CD_DESC
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '07' THEN B.AFTER_CD
			                                        ELSE ''
		                                        END) AF_GRADE_CD_DESC
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '08' THEN B.AFTER_CD + ' ' + B.AFTER_DESC
			                                        ELSE ''
		                                        END) AF_PJOB_CD_DESC
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '04' THEN B.AFTER_DESC
			                                        ELSE ''
		                                        END) AF_EMP_CD_DESC
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '03' THEN B.AFTER_CD
			                                        ELSE ''
		                                        END) AF_WS_CD_DESC				
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '01' THEN B.AFTER_DESC
			                                        ELSE ''
		                                        END) AF_COMPANY_CD_DESC
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '02' THEN B.AFTER_DESC
			                                        ELSE ''
		                                        END) AF_PLANT_CD_DESC
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '09' THEN B.AFTER_CD + ' ' + B.AFTER_DESC
			                                        ELSE ''
		                                        END) AF_WORK_SHIFT_CD_DESC					
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '10' THEN B.AFTER_DESC
			                                        ELSE ''
		                                        END) AF_WORK_CD_DESC
                                    from TB_H_M_EMP_HR_CHANGE_H A
                                    left join TB_H_M_EMP_HR_CHANGE_D B
	                                        on A.HR_CHG_NO = B.HR_CHG_NO
	                                        and A.EMP_ID = B.EMP_ID
                                    left join TB_H_M_EMP E
	                                    on E.EMP_ID = A.EMP_ID
                                    left join TB_H_M_HR_CHANGE_CODE CD
	                                        on A.HR_CHG_CD = CD.HR_CHG_CD                                    
                                    where A.HR_CHG_PROC_STATUS <> 'Y'
	                                    and (A.INS_PLAN_PROC_DT <= @sysdate or A.START_DT <= @sysdate) ");

            //若不為super user
            if (!strIsSuper)
            {
                //若 資料權限之「小分類」為N(管理部擔當)，
                if (strSysCodeAtt == "N")
                {
                    sb.AppendLine(@" and A.HR_CHG_CD in (select G.HR_CHG_CD
					                                 from TB_H_M_HR_CHANGE_CODE G 
					                                 inner join TB_H_M_HR_CHANGE_CODE_EMP F
						                                on G.HR_CHG_CD = F.HR_CHG_CD
						                                and F.EMP_ID = @LOGIN_ID1
                                                        and F.IS_VALID = 'Y') ");
                    ht.Add("@LOGIN_ID1", SessionHandle.Current.emp_id);
                }

                //        若 資料權限之「小分類」為W(各單位擔當)，
                if (strSysCodeAtt == "W")
                {
                    sb.AppendLine(@" and A.HR_CHG_CD in (select G.HR_CHG_CD
					                                 from TB_H_M_HR_CHANGE_CODE G 
					                                 where G.IS_VALID = 'Y' 
                                                        and UPD_RIGHT_CD = 'D') ");
                    //部門權限(自已可管理的部門)
                    sb.AppendLine(@" and E.DEPT_NO in ( ");
                    sb.AppendLine(@"  select MNG_DEPT_NO
				                                       from TB_H_R_HEAD_DEPT D
				                                       where EMP_ID = @LOGIN_ID3 )       ");
                    ht.Add("@LOGIN_ID3", SessionHandle.Current.emp_id);
                }

                sb.AppendLine(@" and  ( ");
                sb.AppendLine(@"  A.CREATED_BY in ( ");
                sb.AppendLine(@" select  @LOGIN_ID2 union  ");
                sb.AppendLine(@" SELECT EMP_ID FROM TB_H_M_EMP  WHERE DEPT_NO IN (SELECT MNG_DEPT_NO FROM TB_H_R_HEAD_DEPT D  WHERE EMP_ID = @LOGIN_ID2 )  )      ");
                sb.AppendLine(@" or ");
                sb.AppendLine(@" A.ORI_DEPT_NO in (  select MNG_DEPT_NO  from TB_H_R_HEAD_DEPT D   where EMP_ID = @LOGIN_ID2 ) ");
                sb.AppendLine(@" ) ");
                ht.Add("@LOGIN_ID2", SessionHandle.Current.emp_id);

            }
            sb.AppendLine(@"GROUP BY A.HR_CHG_NO
		                            ,A.HR_CHG_CD						
		                            ,A.HR_CHG_CD + ' ' + ISNULL(CD.HR_CHG_DESC,'')
		                            ,A.START_DT	
                                    ,A.INS_PLAN_PROC_DT
		                            ,A.EMP_ID
		                            ,E.EMP_NAME ");

            sb.AppendLine("         )alltb ");
            sb.AppendLine("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount_EffectProc(int startRowIndex, int maximumRows)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"declare @sysdate datetime;
                            set @sysdate = convert( varchar(10), getdate(),111); ");
            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine("   from(");
            sb.AppendLine(@"        select   A.HR_CHG_NO
		                                    ,A.HR_CHG_CD						
		                                    ,A.HR_CHG_CD + ' ' + ISNULL(CD.HR_CHG_DESC,'') AS HR_CHG_CD_DESC
		                                    ,A.START_DT	
                                            ,A.INS_PLAN_PROC_DT
		                                    ,A.EMP_ID
		                                    ,E.EMP_NAME
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '05' THEN B.AFTER_CD + ' ' + B.AFTER_DESC
			                                        ELSE ''
		                                        END) AF_DEPT_NO_DESC
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '06' THEN B.AFTER_CD
			                                        ELSE ''
		                                        END) AF_LEVEL_CD_DESC
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '07' THEN B.AFTER_CD
			                                        ELSE ''
		                                        END) AF_GRADE_CD_DESC
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '08' THEN B.AFTER_CD + ' ' + B.AFTER_DESC
			                                        ELSE ''
		                                        END) AF_PJOB_CD_DESC
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '04' THEN B.AFTER_DESC
			                                        ELSE ''
		                                        END) AF_EMP_CD_DESC
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '03' THEN B.AFTER_CD
			                                        ELSE ''
		                                        END) AF_WS_CD_DESC				
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '01' THEN B.AFTER_DESC
			                                        ELSE ''
		                                        END) AF_COMPANY_CD_DESC
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '02' THEN B.AFTER_DESC
			                                        ELSE ''
		                                        END) AF_PLANT_CD_DESC
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '09' THEN B.AFTER_CD + ' ' + B.AFTER_DESC
			                                        ELSE ''
		                                        END) AF_WORK_SHIFT_CD_DESC					
		                                    ,max(CASE WHEN B.HR_CHG_ITEM = '10' THEN B.AFTER_DESC
			                                        ELSE ''
		                                        END) AF_WORK_CD_DESC
                                    from TB_H_M_EMP_HR_CHANGE_H A
                                    left join TB_H_M_EMP_HR_CHANGE_D B
	                                        on A.HR_CHG_NO = B.HR_CHG_NO
	                                        and A.EMP_ID = B.EMP_ID
                                    left join TB_H_M_EMP E
	                                    on E.EMP_ID = A.EMP_ID
                                    left join TB_H_M_HR_CHANGE_CODE CD
	                                        on A.HR_CHG_CD = CD.HR_CHG_CD                                    
                                    where A.HR_CHG_PROC_STATUS <> 'Y'
	                                    and (A.INS_PLAN_PROC_DT <= @sysdate or A.START_DT <= @sysdate) ");

            //若不為super user
            if (!strIsSuper)
            {
                //若 資料權限之「小分類」為N(管理部擔當)，
                if (strSysCodeAtt == "N")
                {
                    sb.AppendLine(@" and A.HR_CHG_CD in (select G.HR_CHG_CD
					                                 from TB_H_M_HR_CHANGE_CODE G 
					                                 inner join TB_H_M_HR_CHANGE_CODE_EMP F
						                                on G.HR_CHG_CD = F.HR_CHG_CD
						                                and F.EMP_ID = @LOGIN_ID1
                                                        and F.IS_VALID = 'Y') ");
                    ht.Add("@LOGIN_ID1", SessionHandle.Current.emp_id);
                }

                //        若 資料權限之「小分類」為W(各單位擔當)，
                if (strSysCodeAtt == "W")
                {
                    sb.AppendLine(@" and A.HR_CHG_CD in (select G.HR_CHG_CD
					                                 from TB_H_M_HR_CHANGE_CODE G 
					                                 where G.IS_VALID = 'Y' 
                                                        and UPD_RIGHT_CD = 'D') ");
                    //部門權限(自已可管理的部門)
                    sb.AppendLine(@" and E.DEPT_NO in ( ");
                    sb.AppendLine(@"  select MNG_DEPT_NO
				                                       from TB_H_R_HEAD_DEPT D
				                                       where EMP_ID = @LOGIN_ID3 )       ");
                    ht.Add("@LOGIN_ID3", SessionHandle.Current.emp_id);
                }
               
                sb.AppendLine(@" and  ( ");
                sb.AppendLine(@"  A.CREATED_BY in ( ");
                sb.AppendLine(@" select  @LOGIN_ID2 union  ");
                sb.AppendLine(@" SELECT EMP_ID FROM TB_H_M_EMP  WHERE DEPT_NO IN (SELECT MNG_DEPT_NO FROM TB_H_R_HEAD_DEPT D  WHERE EMP_ID = @LOGIN_ID2 )  )      ");
                sb.AppendLine(@" or ");
                sb.AppendLine(@" A.ORI_DEPT_NO in (  select MNG_DEPT_NO  from TB_H_R_HEAD_DEPT D   where EMP_ID = @LOGIN_ID2 ) ");
                sb.AppendLine(@" ) ");
                ht.Add("@LOGIN_ID2", SessionHandle.Current.emp_id);

            }

            sb.AppendLine(@"GROUP BY A.HR_CHG_NO
		                            ,A.HR_CHG_CD						
		                            ,A.HR_CHG_CD + ' ' + ISNULL(CD.HR_CHG_DESC,'')
		                            ,A.START_DT	
                                    ,A.INS_PLAN_PROC_DT
		                            ,A.EMP_ID
		                            ,E.EMP_NAME ");

            sb.AppendLine("  )alltb ");

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

    public void SP_H_HR_CHG_PROC(List<string> emp_ids)
    {
        try
        {
            StringBuilder sb;
            Hashtable ht;
            for (int i = 0; i < emp_ids.Count; i++)
            {
                sb = new StringBuilder();
                ht = new Hashtable();
                sb.Append("SP_H_HR_CHG_PROC");
                ht.Add("@pDate", DateTime.Now.ToString("yyyy/MM/dd"));
                ht.Add("@pEmp_ID", emp_ids[i]);
                ht.Add("@pUserID", SessionHandle.Current.emp_id);
                ht.Add("@pFuncID", "FB2HC010");
                dbConn.ExecuteSPT(sb, ht, true);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    //1.讀取 員工人事資料VIEW(VW_H_EMP_DATA)  E
    //    條件:	E.在職狀態 <> '99'
    //          且 E.工號 <> 登入者帳號	※不可以輸入登入者自己的人事異動資料
    //                若有輸入入社起日，加入條件：且 E.入社日期 >= 畫面.入社起日
    //                若有輸入入社迄日，加入條件：且 E.入社日期 <= 畫面.入社迄日
    //                若有輸入部門代號，加入條件：且 E.部門代號 LIKE '畫面.部門代號%'
    //                若有輸入職務代號，加入條件：且 E.職務代號 LIKE '畫面.職務代號%'
    //                若有輸入 輪值表代碼 ，加入條件：且 E.輪值表代碼  LIKE '畫面.輪值表代碼 %'
    //                若有輸入返校日，加入條件：且 E.返校日 = 畫面.返校日
    //                若有輸入返廠日，加入條件：且 E.返廠日 = 畫面.返廠日
    //                若有輸入轉期間工日，加入條件：且 E.轉期間工日 = 畫面.轉期間工日
    //                若有輸入轉派日，加入條件：且 E.轉派日 = 畫面.轉派日
    //                若有輸入續派日，加入條件：且 E.續派日 = 畫面.續派日
    //                若有輸入聘用單位，加入條件：且 E.聘用單位 = 畫面.聘用單位 的代碼
    //                若有輸入工廠區分，加入條件：且 E.工廠區分 = 畫面.工廠區分 的代碼
    //                若有輸入工數區分，加入條件：且 E.工數區分 = 畫面.工數區分 的代碼
    //                若 G.借調人員適用(人事異動代碼檔取得)為'N'，加入條件：且 E.員工區分 <> '3'(借調人員)
    //                若 資料權限之「部門含以下」或「部門權限」任一不為空值，
    //                        加入條件：且 E.部門代號 必須存在以下 該擔當有權限作業的部門清單中，
    //                        UNION 以下兩者的部門清單，
    //                        若 資料權限之「部門含以下」為Y，
    //                                                讀取 主管可管理部門資料檔 D
    //                                                    取得:	D.可管理部門代號
    //                                                    條件:	D.工號 = 登入者帳號
    //                        若 資料權限之「部門權限」不為空值，
    //                                                「部門權限」的內容。
    //    取得的每一工號，逐一
    //            讀取 人事異動主檔 H
    //                取得:	H.人事異動代碼
    //                條件:	H.工號 = E.工號
    //                      且 H.人事異動生效日 = 明細畫面.異動生效日
    //                      且 H.人事異動代碼 = 明細畫面.人事異動代碼
    //            若讀得到資料，排除此工號。

    public DataTable getData_Add_batch(int startRowIndex, int maximumRows, string sortExpression,
                            string start_dt, string hr_chg_cd,
                            string join_sdt, string join_edt, string dept_no,
                            string pjob_cd, string work_shift_cd, string back_school_dt,
                            string back_plant_dt, string be_contract_dt, string be_despatch_dt,
                            string keep_despatch_dt, string company_cd, string plant_cd,
                            string work_cd)
    {
        try
        {
            if (sortExpression == "")
                sortExpression = "COMPANY_CD, PLANT_CD, DEPT_NO";

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" declare @IS_FOR_TRANSFER_IN varchar(1) = (select IS_FOR_TRANSFER_IN from TB_H_M_HR_CHANGE_CODE where HR_CHG_CD = @HR_CHG_CD); ");
            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* ");
            sb.AppendLine("       from(");
            sb.AppendLine(@"        select	 E.EMP_CHG_CD
		                                    ,E.EMP_CHG_DESC
		                                    ,E.COMPANY_CD
		                                    ,E.COMPANY_NAME
		                                    ,E.PLANT_CD
		                                    ,E.PLANT_NAME
		                                    ,E.DEPT_NO
		                                    ,E.DEPT_NO + ' ' + E.DEPT_NAME DEPT_NAME
		                                    ,E.EMP_ID
		                                    ,E.EMP_NAME
		                                    ,E.JOIN_DT
		                                    ,E.PJOB_CD
		                                    ,E.PJOB_DESC
		                                    ,E.WORK_SHIFT_CD
		                                    ,E.WORK_SHIFT_DESC
		                                    ,E.WORK_CD
		                                    ,E.WORK_DESC
		                                    ,E.BACK_SCHOOL_DT
		                                    ,E.BACK_PLANT_DT
		                                    ,E.BE_CONTRACT_DT
		                                    ,E.BE_DESPATCH_DT
		                                    ,E.KEEP_DESPATCH_DT
		                                    ,Convert(varchar(12),'') MAIN_HR_CHG_NO
                                    from VW_H_EMP_DATA E
                                    left join TB_H_M_EMP_HR_CHANGE_H H
	                                    on H.EMP_ID = E.EMP_ID
	                                    and H.START_DT = @START_DT
	                                    and H.HR_CHG_CD = @HR_CHG_CD
                                    where 1 = 1
                                    and E.EMP_STATUS <> '99' ");


            ht.Add("@START_DT", start_dt);
            ht.Add("@HR_CHG_CD", hr_chg_cd);
            sb.AppendLine(" and E.EMP_ID <> @LOGIN_ID ");
            ht.Add("@LOGIN_ID", SessionHandle.Current.emp_id);

            //若有輸入入社起日，加入條件：且 E.入社日期 >= 畫面.入社起日            
            if (join_sdt != "" && join_sdt != null)
            {
                sb.AppendLine(" and E.JOIN_DT >= @JOIN_SDT ");
                ht.Add("@JOIN_SDT", join_sdt);
            }
            //若有輸入入社迄日，加入條件：且 E.入社日期 <= 畫面.入社迄日
            if (join_edt != "" && join_edt != null)
            {
                sb.AppendLine(" and E.JOIN_DT <= @JOIN_EDT ");
                ht.Add("@JOIN_EDT", join_edt);
            }
            //若有輸入部門代號，加入條件：且 E.部門代號 LIKE '畫面.部門代號%'
            if (dept_no != "" && dept_no != null)
            {
                sb.AppendLine(" and E.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            //若有輸入職務代號，加入條件：且 E.職務代號 LIKE '畫面.職務代號%'
            if (pjob_cd != "" && pjob_cd != null)
            {
                sb.AppendLine(" and E.PJOB_CD like @PJOB_CD ");
                ht.Add("@PJOB_CD", pjob_cd + "%");
            }
            //若有輸入 輪值表代碼 ，加入條件：且 E.輪值表代碼  LIKE '畫面.輪值表代碼 %'
            if (work_shift_cd != "" && work_shift_cd != null)
            {
                sb.AppendLine(" and E.WORK_SHIFT_CD like @WORK_SHIFT_CD ");
                ht.Add("@WORK_SHIFT_CD", work_shift_cd + "%");
            }
            //若有輸入返校日，加入條件：且 E.返校日 = 畫面.返校日
            if (back_school_dt != "" && back_school_dt != null)
            {
                sb.AppendLine(" and E.BACK_SCHOOL_DT = @BACK_SCHOOL_DT ");
                ht.Add("@BACK_SCHOOL_DT", back_school_dt);
            }
            //若有輸入返廠日，加入條件：且 E.返廠日 = 畫面.返廠日
            if (back_plant_dt != "" && back_plant_dt != null)
            {
                sb.AppendLine(" and E.BACK_PLANT_DT = @BACK_PLANT_DT ");
                ht.Add("@BACK_PLANT_DT", back_plant_dt);
            }
            //若有輸入轉期間工日，加入條件：且 E.轉期間工日 = 畫面.轉期間工日                        
            if (be_contract_dt != "" && be_contract_dt != null)
            {
                sb.AppendLine(" and E.BE_CONTRACT_DT = @BE_CONTRACT_DT ");
                ht.Add("@BE_CONTRACT_DT", be_contract_dt);
            }
            //若有輸入轉派日，加入條件：且 E.轉派日 = 畫面.轉派日
            if (be_despatch_dt != "" && be_despatch_dt != null)
            {
                sb.AppendLine(" and E.BE_DESPATCH_DT = @BE_DESPATCH_DT ");
                ht.Add("@BE_DESPATCH_DT", be_despatch_dt);
            }
            //若有輸入續派日，加入條件：且 E.續派日 = 畫面.續派日                        
            if (keep_despatch_dt != "" && keep_despatch_dt != null)
            {
                sb.AppendLine(" and E.KEEP_DESPATCH_DT = @KEEP_DESPATCH_DT ");
                ht.Add("@KEEP_DESPATCH_DT", keep_despatch_dt);
            }
            //若有輸入聘用單位，加入條件：且 E.聘用單位 = 畫面.聘用單位 的代碼                        
            if (company_cd != "" && company_cd != null)
            {
                sb.AppendLine(" and E.COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            //若有輸入工廠區分，加入條件：且 E.工廠區分 = 畫面.工廠區分 的代碼                        
            if (plant_cd != "" && plant_cd != null)
            {
                sb.AppendLine(" and E.PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            //若有輸入工數區分，加入條件：且 E.工數區分 = 畫面.工數區分 的代碼
            if (work_cd != "" && work_cd != null)
            {
                sb.AppendLine(" and E.WORK_CD = @WORK_CD ");
                ht.Add("@WORK_CD", work_cd);
            }
            //若 G.借調人員適用(人事異動代碼檔取得)為'N'，加入條件：且 E.員工區分 <> '3'(借調人員)
            sb.AppendLine(" and ((@IS_FOR_TRANSFER_IN = 'N' and E.EMP_CD <> '3') or @IS_FOR_TRANSFER_IN <> 'N') ");
            //    取得的每一工號，逐一
            //            讀取 人事異動主檔 H
            //                取得:	H.人事異動代碼
            //                條件:	H.工號 = E.工號
            //                      且 H.人事異動生效日 = 明細畫面.異動生效日
            //                      且 H.人事異動代碼 = 明細畫面.人事異動代碼
            //            若讀得到資料，排除此工號。
            sb.AppendLine(" and (H.EMP_ID is null or H.EMP_ID = '') ");

            //顯示資料權限設定,若不為super user(以部門為主)
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND E.DEPT_NO IN(	 select DEPT_NO from dbo.FN_H_GET_AUTH_DEPT(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }
            /*
            //若不為super user(以部門為主)
            if (!strIsSuper)
            {
                sb.Append(@" AND ( ");
                sb.AppendLine(@" E.DEPT_NO in ( "
                             + " select MNG_DEPT_NO from TB_H_R_HEAD_DEPT where emp_id=@LOGIN_ID2 ");
                sb.AppendLine(" ) ");
                ht.Add("@LOGIN_ID2", SessionHandle.Current.emp_id);
                if (strDepartments != "" && strDepartments != "N")
                {
                    //若 資料權限之「部門權限」不為空值
                    sb.Append(" or  E.DEPT_NO in ( @uDEPT_NO) ");
                    ht.Add("@uDEPT_NO", strDepartments.Replace(" ", "").Split(','));
                }
                sb.Append(" ) ");
            }
             * */
           
            sb.AppendLine("         )alltb ");
            sb.AppendLine("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount_Add_batch(int startRowIndex, int maximumRows,
                            string start_dt, string hr_chg_cd,
                            string join_sdt, string join_edt, string dept_no,
                            string pjob_cd, string work_shift_cd, string back_school_dt,
                            string back_plant_dt, string be_contract_dt, string be_despatch_dt,
                            string keep_despatch_dt, string company_cd, string plant_cd,
                            string work_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" declare @IS_FOR_TRANSFER_IN varchar(1) = (select IS_FOR_TRANSFER_IN from TB_H_M_HR_CHANGE_CODE where HR_CHG_CD = @HR_CHG_CD); ");
            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine("   from(");
            sb.AppendLine(@"        select	 E.EMP_CHG_CD
		                                    ,E.EMP_CHG_DESC
		                                    ,E.COMPANY_CD
		                                    ,E.COMPANY_NAME
		                                    ,E.PLANT_CD
		                                    ,E.PLANT_NAME
		                                    ,E.DEPT_NO
		                                    ,E.DEPT_NO + ' ' + E.DEPT_NAME DEPT_NAME
		                                    ,E.EMP_ID
		                                    ,E.EMP_NAME
		                                    ,E.JOIN_DT
		                                    ,E.PJOB_CD
		                                    ,E.PJOB_DESC
		                                    ,E.WORK_SHIFT_CD
		                                    ,E.WORK_SHIFT_DESC
		                                    ,E.WORK_CD
		                                    ,E.WORK_DESC
		                                    ,E.BACK_SCHOOL_DT
		                                    ,E.BACK_PLANT_DT
		                                    ,E.BE_CONTRACT_DT
		                                    ,E.BE_DESPATCH_DT
		                                    ,E.KEEP_DESPATCH_DT
		                                    ,Convert(varchar(12),'') MAIN_HR_CHG_NO
                                    from VW_H_EMP_DATA E
                                    left join TB_H_M_EMP_HR_CHANGE_H H
	                                    on H.EMP_ID = E.EMP_ID
	                                    and H.START_DT = @START_DT
	                                    and H.HR_CHG_CD = @HR_CHG_CD
                                    where 1 = 1
                                    and E.EMP_STATUS <> '99' ");


            ht.Add("@START_DT", start_dt);
            ht.Add("@HR_CHG_CD", hr_chg_cd);
            sb.AppendLine(" and E.EMP_ID <> @LOGIN_ID ");
            ht.Add("@LOGIN_ID", SessionHandle.Current.emp_id);

            //若有輸入入社起日，加入條件：且 E.入社日期 >= 畫面.入社起日            
            if (join_sdt != "" && join_sdt != null)
            {
                sb.AppendLine(" and E.JOIN_DT >= @JOIN_SDT ");
                ht.Add("@JOIN_SDT", join_sdt);
            }
            //若有輸入入社迄日，加入條件：且 E.入社日期 <= 畫面.入社迄日
            if (join_edt != "" && join_edt != null)
            {
                sb.AppendLine(" and E.JOIN_DT <= @JOIN_EDT ");
                ht.Add("@JOIN_EDT", join_edt);
            }
            //若有輸入部門代號，加入條件：且 E.部門代號 LIKE '畫面.部門代號%'
            if (dept_no != "" && dept_no != null)
            {
                sb.AppendLine(" and E.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            //若有輸入職務代號，加入條件：且 E.職務代號 LIKE '畫面.職務代號%'
            if (pjob_cd != "" && pjob_cd != null)
            {
                sb.AppendLine(" and E.PJOB_CD like @PJOB_CD ");
                ht.Add("@PJOB_CD", pjob_cd + "%");
            }
            //若有輸入 輪值表代碼 ，加入條件：且 E.輪值表代碼  LIKE '畫面.輪值表代碼 %'
            if (work_shift_cd != "" && work_shift_cd != null)
            {
                sb.AppendLine(" and E.WORK_SHIFT_CD like @WORK_SHIFT_CD ");
                ht.Add("@WORK_SHIFT_CD", work_shift_cd + "%");
            }
            //若有輸入返校日，加入條件：且 E.返校日 = 畫面.返校日
            if (back_school_dt != "" && back_school_dt != null)
            {
                sb.AppendLine(" and E.BACK_SCHOOL_DT = @BACK_SCHOOL_DT ");
                ht.Add("@BACK_SCHOOL_DT", back_school_dt);
            }
            //若有輸入返廠日，加入條件：且 E.返廠日 = 畫面.返廠日
            if (back_plant_dt != "" && back_plant_dt != null)
            {
                sb.AppendLine(" and E.BACK_PLANT_DT = @BACK_PLANT_DT ");
                ht.Add("@BACK_PLANT_DT", back_plant_dt);
            }
            //若有輸入轉期間工日，加入條件：且 E.轉期間工日 = 畫面.轉期間工日                        
            if (be_contract_dt != "" && be_contract_dt != null)
            {
                sb.AppendLine(" and E.BE_CONTRACT_DT = @BE_CONTRACT_DT ");
                ht.Add("@BE_CONTRACT_DT", be_contract_dt);
            }
            //若有輸入轉派日，加入條件：且 E.轉派日 = 畫面.轉派日
            if (be_despatch_dt != "" && be_despatch_dt != null)
            {
                sb.AppendLine(" and E.BE_DESPATCH_DT = @BE_DESPATCH_DT ");
                ht.Add("@BE_DESPATCH_DT", be_despatch_dt);
            }
            //若有輸入續派日，加入條件：且 E.續派日 = 畫面.續派日                        
            if (keep_despatch_dt != "" && keep_despatch_dt != null)
            {
                sb.AppendLine(" and E.KEEP_DESPATCH_DT = @KEEP_DESPATCH_DT ");
                ht.Add("@KEEP_DESPATCH_DT", keep_despatch_dt);
            }
            //若有輸入聘用單位，加入條件：且 E.聘用單位 = 畫面.聘用單位 的代碼                        
            if (company_cd != "" && company_cd != null)
            {
                sb.AppendLine(" and E.COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            //若有輸入工廠區分，加入條件：且 E.工廠區分 = 畫面.工廠區分 的代碼                        
            if (plant_cd != "" && plant_cd != null)
            {
                sb.AppendLine(" and E.PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", plant_cd);
            }
            //若有輸入工數區分，加入條件：且 E.工數區分 = 畫面.工數區分 的代碼
            if (work_cd != "" && work_cd != null)
            {
                sb.AppendLine(" and E.WORK_CD = @WORK_CD ");
                ht.Add("@WORK_CD", work_cd);
            }
            //若 G.借調人員適用(人事異動代碼檔取得)為'N'，加入條件：且 E.員工區分 <> '3'(借調人員)
            sb.AppendLine(" and ((@IS_FOR_TRANSFER_IN = 'N' and E.EMP_CD <> '3') or @IS_FOR_TRANSFER_IN <> 'N') ");
            //    取得的每一工號，逐一
            //            讀取 人事異動主檔 H
            //                取得:	H.人事異動代碼
            //                條件:	H.工號 = E.工號
            //                      且 H.人事異動生效日 = 明細畫面.異動生效日
            //                      且 H.人事異動代碼 = 明細畫面.人事異動代碼
            //            若讀得到資料，排除此工號。
            sb.AppendLine(" and (H.EMP_ID is null or H.EMP_ID = '') ");

            //若不為super user(以部門為主)
            if (!strIsSuper)
            {
                sb.Append(@" AND ( ");
                sb.AppendLine(@" E.DEPT_NO in ( "
                             + " select MNG_DEPT_NO from TB_H_R_HEAD_DEPT where emp_id=@LOGIN_ID2 ");
                sb.AppendLine(" ) ");
                ht.Add("@LOGIN_ID2", SessionHandle.Current.emp_id);
                if (strDepartments != "" && strDepartments != "N")
                {
                    //若 資料權限之「部門權限」不為空值
                    sb.Append(" or  E.DEPT_NO in ( @uDEPT_NO) ");
                    ht.Add("@uDEPT_NO", strDepartments.Replace(" ", "").Split(','));
                }
                sb.Append(" ) ");
            }

            sb.AppendLine("  )alltb ");

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

    /// <summary>
    /// sql 組出union語法
    /// </summary>
    /// <param name="comm">sqlcommand</param>
    /// <param name="param"></param>
    /// <returns></returns>
    public void sqlUnion(ref StringBuilder sb, ref Hashtable ht, string param, string id, string isDept)
    {
        try
        {
            if (param != "")
            {
                if (isDept == "Y")
                    sb.Append(" union ");

                if (param.Contains(','))
                {
                    List<string> tmp = param.Split(',').ToList();
                    for (int i = 0; i < tmp.Count; i++)
                    {
                        if (i == 0)
                        {
                            sb.AppendLine(" select @" + id + i.ToString() + " as " + id);
                            ht.Add("@" + id + i.ToString(), tmp[i].Trim());
                        }
                        else
                        {
                            sb.AppendLine(" union select @" + id + i.ToString() + " as " + id);
                            ht.Add("@" + id + i.ToString(), tmp[i].Trim());
                        }
                    }
                }
                else
                {
                    sb.Append(" select @" + id + "0 as " + id);
                    ht.Add("@" + id + "0", param.Trim());
                }
            }
            return;
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// sql 組出in語法
    /// </summary>
    /// <param name="comm">sqlcommand</param>
    /// <param name="param">in 裡面的條件，以,串聯</param>
    /// <returns></returns>
    public void sqlIn(ref StringBuilder sb, ref Hashtable ht, string param, string id)
    {
        try
        {
            if (param != "")
            {
                sb.Append("(");
                if (param.Contains(','))
                {
                    List<string> tmp = param.Split(',').ToList();
                    for (int i = 0; i < tmp.Count; i++)
                    {
                        if (i == 0)
                        {
                            sb.Append("@" + id + i.ToString());
                            ht.Add("@" + id + i.ToString(), tmp[i]);
                        }
                        else
                        {
                            sb.Append(",@" + id + i.ToString());
                            ht.Add("@" + id + i.ToString(), tmp[i]);
                        }
                    }
                }
                else
                {
                    sb.Append("@" + id + "0");
                    ht.Add("@" + id + "0", param);
                }
                sb.Append(")");
            }
            return;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得ACES資料
    private void getAuth(ref bool strIsSuper, ref string strIsDEPT, ref string strDepartments, ref string strSysCodeAtt)
    {
        strIsDEPT = "";
        strDepartments = "";
        strSysCodeAtt = "";
        ACESLib.ACES aces = new ACESLib.ACES();
        foreach (string dbRoleCD in aces.GetRoles().Split(','))
        {
            if (dbRoleCD.Trim() != "")
            {
                DEPTBean dBean = aces.GetDEPTAuth(dbRoleCD.Trim());
                string IsDEPT = dBean.IsDEPT;
                if (IsDEPT == "Y")
                    strIsDEPT = IsDEPT;
                if (dBean.Departments.Trim() != "")
                {
                    strDepartments = dBean.Departments.Trim();
                }
                string SysCode = dBean.SysCode;
                foreach (string code in SysCode.Split(','))
                {
                    if (code.Trim().Equals("ROLE_CD"))
                    {
                        string syscodeatt = aces.GetCodeAtt(dbRoleCD.Trim(), code.Trim());//小分類
                        if (syscodeatt.Trim() != "")
                        {
                            strSysCodeAtt = syscodeatt.Trim();
                        }

                        //因小分類可能同時包含，如"N(管理部擔當),Y(管理部主管),W(各單位擔當)"，
                        if (syscodeatt.Trim().Contains("Y")) {
                            strSysCodeAtt = "Y";
                        }
                        else if (syscodeatt.Trim().Contains("N")) {
                            strSysCodeAtt = "N";
                        }
                        else if (syscodeatt.Trim().Contains("W"))
                        {
                            strSysCodeAtt = "W";
                        }
                    }
                }
                foreach (string big_sysCode in SysCode.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    if (big_sysCode.Trim().Equals("SUPER"))
                    {
                        strIsSuper = true;
                    }
                }
            }
        }
    }

    //2.◎人事異動代碼如果直接輸入，
    //        讀取 人事異動代碼檔 G
    //            取得:	G.*
    //            條件:	G.人事異動代碼 = 明細畫面.人事異動代碼
    //                  且 G.使用中 = 'Y'
    //                  且 G.一括異動適用 = 'Y'
    //                  若 資料權限之「小分類」為N(管理部擔當)，
    //                      加入條件：且 G.人事異動代碼 必須存在於  (讀取 人事異動代碼擔當檔 F
    //                                                               取得:F.人事異動代碼
    //                                                               條件:F.工號 = 登入者帳號 且 F.使用中 = 'Y')
    //                  若 資料權限之「小分類」為W(各單位擔當)，
    //                      加入條件：且 G.權限區分 = 'D'
    //            若讀不到，顯示錯誤訊息"人事異動代碼不存在，或無權限作業"。
    public string Add_batch_Get_HR_CHG_DESC(string hr_chg_cd)
    {
        try
        {
            string rtnval = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select   HR_CHG_DESC                                    
                            from TB_H_M_HR_CHANGE_CODE G
                            where 1 = 1
                            and G.HR_CHG_CD = @HR_CHG_CD
                            and G.IS_VALID = 'Y'
                            and G.IS_FOR_BATCH = 'Y' ");

            //                  若 資料權限之「小分類」為N(管理部擔當)，
            //                      加入條件：且 G.人事異動代碼 必須存在於  (讀取 人事異動代碼擔當檔 F
            //                                                               取得:F.人事異動代碼
            //                                                               條件:F.工號 = 登入者帳號 且 F.使用中 = 'Y')
            if (strSysCodeAtt == "N")
            {
                sb.AppendLine(@" and G.HR_CHG_CD in (select F.HR_CHG_CD
					                                 from TB_H_M_HR_CHANGE_CODE_EMP F 
						                             where F.EMP_ID = @LOGIN_ID1
                                                        and F.IS_VALID = 'Y') ");
                ht.Add("@LOGIN_ID1", SessionHandle.Current.emp_id);
            }

            //                  若 資料權限之「小分類」為W(各單位擔當)，
            //                      加入條件：且 G.權限區分 = 'D'
            if (strSysCodeAtt == "W")
            {
                sb.AppendLine(@" and G.UPD_RIGHT_CD = 'D') ");
            }

            ht.Add("@HR_CHG_CD", hr_chg_cd);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                rtnval = dr["HR_CHG_DESC"].ToString();
            }
            else
            {
                rtnval = "";
            }

            return rtnval;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable Get_COMPANY_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"select C.COMPANY_CD sub_cd, C.COMPANY_SNAME sub_desc
                            from TB_H_M_COMPANY C ");
            DataTable dt = dbConn.Query(sb);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getEMP_NAME(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID,EMP_NAME from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public DataTable getHR_CHG_DESC(string hr_chg_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select HR_CHG_CD,HR_CHG_DESC from TB_H_M_HR_CHANGE_CODE ");
            sb.Append(" where HR_CHG_CD = @HR_CHG_CD");
            ht.Add("@HR_CHG_CD", hr_chg_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public DataTable getddl_HR_CHG_CD(string IS_VALID, string FUNC_ID, string EMP_ID, string UPD_RIGHT_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select HR_CHG_CD,HR_CHG_DESC from TB_H_M_HR_CHANGE_CODE a where HR_CHG_CD is not null ");

            if (UPD_RIGHT_CD != "")
            {
                sb.Append(" and UPD_RIGHT_CD = @UPD_RIGHT_CD");
                ht.Add("@UPD_RIGHT_CD", UPD_RIGHT_CD);
            }
            if (IS_VALID != "")
            {
                sb.Append(" and IS_VALID = @IS_VALID");
                ht.Add("@IS_VALID", IS_VALID);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and EXISTS (SELECT HR_CHG_CD FROM TB_H_M_HR_CHANGE_CODE_EMP WHERE EMP_ID =@EMP_ID AND IS_VALID = 'Y' and HR_CHG_CD = a.HR_CHG_CD)");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (FUNC_ID == "FB2HC010_ADD")
            {

                sb = new StringBuilder();
                ht = new Hashtable();
                sb.AppendLine(@"select HR_CHG_CD,HR_CHG_DESC,UPD_RIGHT_CD,IS_FOR_TRANSFER_IN 
                            from TB_H_M_HR_CHANGE_CODE G
                            left join TB_H_M_EMP E 
	                            on EMP_ID = @EMP_ID
                            where 1 = 1
                            and G.IS_VALID = 'Y'
                            and ((E.EMP_CD = '3' and G.IS_FOR_TRANSFER_IN = 'Y') or (E.EMP_CD <> '3')) ");
                ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
                //若 資料權限之「小分類」為N(管理部擔當)，
                if (strSysCodeAtt == "N")
                {
                    sb.AppendLine(@" and G.HR_CHG_CD in (select F.HR_CHG_CD
					                                 from TB_H_M_HR_CHANGE_CODE_EMP F
						                             where F.EMP_ID = @LOGIN_ID1
                                                        and F.IS_VALID = 'Y' ) ");
                    ht.Add("@LOGIN_ID1", SessionHandle.Current.emp_id);
                }

                //若 資料權限之「小分類」為W(各單位擔當)，
                if (strSysCodeAtt == "W")
                {
                    sb.AppendLine(@" and G.UPD_RIGHT_CD = 'D' ");
                }
            }
            else if (FUNC_ID == "FB2HC010_ADD_BATCH")
            {

                sb = new StringBuilder();
                ht = new Hashtable();
                sb.AppendLine(@"select HR_CHG_CD,HR_CHG_DESC,UPD_RIGHT_CD,IS_FOR_TRANSFER_IN 
                            from TB_H_M_HR_CHANGE_CODE G
                            where 1 = 1
                            and G.IS_VALID = 'Y'
                            and G.IS_FOR_BATCH = 'Y' ");

                //若 資料權限之「小分類」為N(管理部擔當)，
                if (strSysCodeAtt == "N")
                {
                    sb.AppendLine(@" and G.HR_CHG_CD in (select F.HR_CHG_CD
					                                 from TB_H_M_HR_CHANGE_CODE_EMP F
						                             where F.EMP_ID = @LOGIN_ID1
                                                        and F.IS_VALID = 'Y' ) ");
                    ht.Add("@LOGIN_ID1", SessionHandle.Current.emp_id);
                }

                //若 資料權限之「小分類」為W(各單位擔當)，
                if (strSysCodeAtt == "W")
                {
                    sb.AppendLine(@" and G.UPD_RIGHT_CD = 'D' ");
                }
            }

            sb.Append(" order by HR_CHG_CD ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得生效日之前的上班日
    public DataTable getINS_PLAN_PROC_DT(string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select top 1 CONVERT(VARCHAR(10),CALENDAR_DT,111) CALENDAR_DT 
                        from TB_D_M_CALENDAR_D
                        where CALENDAR_CD='A'
                        and WORK_DAY_CD='1'
                        and CALENDAR_DT < @CALENDAR_DT
                        order by CALENDAR_DT desc ");
            ht.Add("@CALENDAR_DT", start_dt);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }


}