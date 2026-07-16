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
/// CFB2HD0100BO 的摘要描述
/// </summary>
public class COMMGEODAO : BaseDAO
{
    public COMMGEODAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string EMP_ID { get; set; }
    public string HR_CHG_CD { get; set; }

    public string DOC_NO { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_CD { get; set; }
    public string SALARY_ID { get; set; }
    public string SALARY_CD { get; set; }
    public string DEPT_NO { get; set; }
    public string DEPT_NAME { get; set; }
    public string LEVEL_CD { get; set; }
    public string PJOB_DESC { get; set; }
    public string START_DT { get; set; }
    public string JUDGEMENT_TYPE { get; set; }
    public string REASON_CD { get; set; }
    public string FIRST_CNT { get; set; }
    public string SECOND_CNT { get; set; }
    public string THIRD_CNT { get; set; }
    public string IS_FIRE { get; set; }
    public string REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    public string SUB_CD { get; set; }
    public string MAIN_CD { get; set; }
    public string SYS_CD { get; set; }
    public string REMIT_DT { get; set; }
    public string SALARY_TYPE { get; set; }
    public string PJOB_CD { get; set; }
    public string WORK_SHIFT_CD { get; set; }

    //for查詢欄位
    public string ddl_SYS_ID { get; set; }


    public DataTable getCHANGE_CODEFile()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select HR_CHG_CD,HR_CHG_DESC,UPD_RIGHT_CD,IS_FOR_TRANSFER_IN from TB_H_M_HR_CHANGE_CODE a where HR_CHG_CD is not null ");

            
                sb.Append(" and HR_CHG_CD = @HR_CHG_CD");
                ht.Add("@HR_CHG_CD", HR_CHG_CD);
           
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getEMPFile()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select E.LICENSE_ID,CONVERT(CHAR(10),E.BIRTH_DT, 111) as BIRTH_DT,E.EMP_NAME, E.EMP_CD,PLANT_CD,PLANT_NAME, COMM.SUB_DESC, E.DEPT_NO, D.DEPT_NAME, E.PJOB_CD, E.PJOB_DESC, E.LEVEL_CD, E.WORK_SHIFT_CD, E.WORK_SHIFT_DESC");
            sb.Append(" , CONVERT(char(10), E.JOIN_DT, 120) JOIN_DT ,E.REGISTER_ADDR");
            sb.Append(" , (select top 1 ADDRESS from TB_D_M_TRANS_ALLOWANCE_D where E.EMP_ID = TB_D_M_TRANS_ALLOWANCE_D.EMP_ID) CONTACT_ADDR");
            sb.Append(" , E.MOBILE_TEL_1, E.CONTACT_TEL,AGE,E.WORK_SHIFT_CD ,COMM1.SUB_DESC AS WORK_SHIFT_NAME,COMM1.SUB_CD AS LINE_CD, COMM1.SUB_DESC AS LINE_NAME");
            sb.Append(" , E.EMP_STATUS, E.EMP_STATUS_DESC");
            sb.Append(" , E.DEPT_NAME_20, E.DEPT_NAME_30, E.DEPT_NAME_40, E.DEPT_FULL_NAME, E.DIV_DEPT_FULL_NAME,E.WORK_CD,F.SUB_DESC as WORK_CD_DESC ");
            sb.Append(" , E.LEAVE_DT, E.LEAVE_REASON  ");
            sb.Append(" FROM VW_H_EMP_DATA AS E");
            sb.Append(" LEFT JOIN VW_H_DEPT_DATA AS D ON E.DEPT_NO = D.DEPT_NO");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D AS COMM ON E.EMP_CD = COMM.SUB_CD and COMM.MAIN_CD = 'EMP_CD' AND COMM.SYS_CD = 'HB'");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D AS COMM1 ON SUBSTRING(E.WORK_SHIFT_CD,2,1) = COMM1.SUB_CD and COMM1.MAIN_CD = 'LINE_CD'");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D F ON E.WORK_CD = F.SUB_CD and F.SYS_CD = 'HB' and F.MAIN_CD = 'WORK_CD' and F.IS_VALID = 'Y'");
            sb.Append(" WHERE EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getINS2_DETAIL_TMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select *");
            sb.Append(" FROM TB_S_M_INS2_DETAIL_TMP");
            sb.Append(" WHERE EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }
    
    public DataTable getCommData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD= @SYS_CD and MAIN_CD=@MAIN_CD and SUB_CD = @SUB_CD  ");
            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@MAIN_CD", MAIN_CD);
            ht.Add("@SUB_CD", SUB_CD);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getSALARYFile()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select SALARY_ID,SALARY_NAME from TB_S_M_SALARY_ITEM");
            sb.Append(" WHERE SALARY_ID = @SALARY_ID");
            if (!string.IsNullOrEmpty(SALARY_CD))
            {
                sb.Append(" AND SALARY_CD = @SALARY_CD");
                ht.Add("@SALARY_CD", SALARY_CD);
            }
            if (!string.IsNullOrEmpty(EMP_ID))
            {
                sb.Append(" and SALARY_ID in ( select SALARY_ID from TB_S_M_SUBSIDY_MEM_D   ");
                sb.Append("                     where  TYPE =@TYPE ");
                sb.Append("                     and EMP_ID=@EMP_ID ");
                sb.Append("                     )");
                ht.Add("@TYPE", "1");
                ht.Add("@EMP_ID", EMP_ID);
            }
            ht.Add("@SALARY_ID", SALARY_ID);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getSALARYPAYDATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_SALARY_PAY_H a");
            sb.Append(" where CONVERT(DATETIME,a.REMIT_DT) = CONVERT(DATETIME,@REMIT_DT) AND a.SALARY_TYPE= @SALARY_TYPE");
            ht.Add("@REMIT_DT", REMIT_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getPJOBDATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * from TB_H_M_PJOB where 1 = 1 ");
            if (PJOB_CD != "") {
                sb.Append(" and PJOB_CD = @PJOB_CD ");
                ht.Add("@PJOB_CD", PJOB_CD);
            }
            if (START_DT != "") {
                sb.Append(" and @START_DT >= START_DT and @START_DT <= END_DT");
                ht.Add("@START_DT", START_DT);
            }                        

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getWORKSHIFTDATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select h.WORK_SHIFT_CD,h.WORK_SHIFT_DESC,h.CALENDAR_CD,c.CALENDAR_DESC  ");
            sb.Append(" from TB_D_M_WORK_SHIFT_H h ");
            sb.Append(" left join TB_D_M_CALENDAR_H c on h.CALENDAR_CD = c.CALENDAR_CD ");
            sb.Append(" where 1 = 1 ");
            if (WORK_SHIFT_CD != "")
            {
                sb.Append(" and WORK_SHIFT_CD = @WORK_SHIFT_CD ");
                ht.Add("@WORK_SHIFT_CD", WORK_SHIFT_CD);
            }            

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得主假別說明
    public DataTable getMAIN_LEAVE_DESC(string main_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD,MAIN_LEAVE_DESC ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_H ");
            sb.Append(" where MAIN_LEAVE_CD=@MAIN_LEAVE_CD ");
            ht.Add("@MAIN_LEAVE_CD", main_leave_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得子假別說明
    public DataTable getSUB_LEAVE_DESC(string sub_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SUB_LEAVE_CD,SUB_LEAVE_DESC,LEAVE_TIME_UNIT ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_D ");
            sb.Append(" where SUB_LEAVE_CD=@SUB_LEAVE_CD ");
            ht.Add("@SUB_LEAVE_CD", sub_leave_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //含虛擬部門的所有部門
    public DataTable getDeptAllData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"Select A.DEPT_NO,A.DEPT_NAME,A.DEPT_LEVEL,A.HEAD_EMP_ID 
                        ,isnull(B.HEAD_EMP_NAME,'') HEAD_EMP_NAME 
                        ,isnull(B.UP_DEPT_NO,'') UP_DEPT_NO
                        ,isnull(B.UP_DEPT_NAME ,'')  UP_DEPT_NAME
                        ,isnull(B.DEPT_NO_20,'') DEPT_NO_20
                        ,isnull(B.DEPT_NAME_20,'') DEPT_NAME_20
                        ,isnull(B.DEPT_NO_30,'') DEPT_NO_30
                        ,isnull(B.DEPT_NAME_30,'') DEPT_NAME_30
                        ,isnull(B.DEPT_NO_40,'') DEPT_NO_40
                        ,isnull(B.DEPT_NAME_40,'') DEPT_NAME_40
                        ,isnull(B.DEPT_NO_50,'') DEPT_NO_50
                        ,isnull(B.DEPT_NAME_50,'') DEPT_NAME_50
                        ,isnull(B.DEPT_NO_60,'') DEPT_NO_60
                        ,isnull(B.DEPT_NAME_60,'') DEPT_NAME_60
                        ,isnull(B.DEPT_NO_70,'') DEPT_NO_70
                        ,isnull(B.DEPT_NAME_70,'') DEPT_NAME_70 
                        ,isnull(B.DEPT_FULL_NAME,'')  DEPT_FULL_NAME
                        ,isnull(B.DIV_DEPT_FULL_NAME,'') DIV_DEPT_FULL_NAME
                        from TB_H_M_DEPT A
                        left join TB_H_R_DEPT_DATA	B on A.DEPT_NO = B.dept_no  
                        where 1=1
                         ");
            if (DEPT_NO != "")
            {
                sb.Append(" and A.DEPT_NO = @DEPT_NO");
                ht.Add("@DEPT_NO", DEPT_NO);
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


}