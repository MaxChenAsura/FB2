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
/// CFB2DG020BO 的摘要描述
/// </summary>
public class CFB2DG020DAO : BaseDAO
{
    public CFB2DG020DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string DATA_YM { get; set; }
    public string EMP_ID { get; set; }
    public string DEPT_NO { get; set; }
    public string JPN_CD { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_CD { get; set; }
    public string SALARY_ID { get; set; }
    public string SALARY_YM { get; set; }
    public string SALARY_NAME { get; set; }
    public string CHG_AMT_A { get; set; }
    public string CHG_AMT_B { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string START_DT_B { get; set; }
    public string START_DT_A { get; set; }
    public string END_DATE_B { get; set; }
    public string END_DATE_A { get; set; }
    public string CHG_STATUS { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string REMARK { get; set; }
    public string APP_REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    public string IS_PLUS { get; set; }
    public string IS_TAX { get; set; }

    public DataTable getJPN_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HB' and MAIN_CD='JPN_CD' and IS_VALID = 'Y' ");
            return dbConn.Query(sb);

        }
        catch
        {
            throw;
        }
    }
    public DataTable get_PDF_Data2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" Select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NAME,DEPT_NAME_30,DEPT_NAME_40,WORK_SHIFT_CD,PLANT_CD,CAR_BRAND,CAR_PARK_NO,PARKING_NAME,CAR_NO,UPDATED_DT,TRANSPORT_CD,PARKING_TOOL,'與交通津貼工具不合'AS'Lapse' From 
			            (
                         Select b.EMP_ID,TM.EMP_NAME,TM.DEPT_NO,TD.DIV_DEPT_FULL_NAME DEPT_NAME,TD.DEPT_NAME_30,TD.DEPT_NAME_40,substring(TE.WORK_SHIFT_CD,2,1)+'-'+c.SUB_DESC as WORK_SHIFT_CD,
			             TE.PLANT_CD+'-'+TE.PLANT_NAME PLANT_CD,d.SUB_DESC CAR_BRAND,
			             TM.CAR_PARK_NO,TPM.PARKING_NAME,TM.CAR_NO,CONVERT(VARCHAR(10),b.UPDATED_DT, 111) AS UPDATED_DT,b.TRANSPORT_CD,TM.PARKING_TOOL+'-'+a.SUB_DESC PARKING_TOOL,
                         ROW_NUMBER() Over (Partition By b.EMP_ID Order By b.UPDATED_DT Desc) As Sort 
			             From TB_D_M_PARKING_EMP_MAIN TM            
                         left join VW_H_DEPT_DATA TD on  TM.DEPT_NO=TD.DEPT_NO
			             left join VW_H_EMP_DATA TE on  TE.EMP_ID=TM.EMP_ID
                         left join TB_D_M_PARKING_MAIN TPM on TPM.CAR_PARK_NO=TM.CAR_PARK_NO
			             left join TB_9_M_COMM_D a on  TM.PARKING_TOOL =a.SUB_CD and a.SYS_CD = 'DG' and a.main_cd = 'PARKING_CD' and a.is_valid = 'Y'
			             left join TB_9_M_COMM_D d on TM.CAR_BRAND = d.SUB_CD AND d.SYS_CD = 'DG' AND d.MAIN_CD = 'CAR_BRAND' 
			             left join TB_9_M_COMM_D c on  substring(TE.WORK_SHIFT_CD,2,1) =c.SUB_CD and c.SYS_CD = 'DB' and c.main_cd = 'LINE_CD' and c.is_valid = 'Y'
                         left join (select t.CODE_VAL2,s.EMP_ID ,s.UPDATED_DT,s.TRANSPORT_CD from TB_D_M_TRANS_ALLOWANCE_D s
						            left join TB_9_M_COMM_D t on s.TRANSPORT_CD = t.SUB_CD and t.SYS_CD = 'DD' and t.MAIN_CD = 'TRANSPORT_CD' where CONVERT(varchar,s.END_DT,112)='99991231'
						            )b on TM.EMP_ID = b.EMP_ID where TM.PARKING_TOOL <> b.CODE_VAL2            
                         )TMP_S 
                         where TMP_S.Sort=1

                         union all
                         Select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NAME,DEPT_NAME_30,DEPT_NAME_40,WORK_SHIFT_CD,PLANT_CD,CAR_BRAND,
			             CAR_PARK_NO,PARKING_NAME,CAR_NO,UPDATED_DT,TRANSPORT_CD,PARKING_TOOL, 'ICT' AS'Lapse' From (
                         Select TM.EMP_ID,TM.EMP_NAME,TM.DEPT_NO,TD.DIV_DEPT_FULL_NAME DEPT_NAME,TM.CAR_PARK_NO,TPM.PARKING_NAME,TM.CAR_NO,
			             CONVERT(VARCHAR(10),TM.UPDATED_DT, 111) AS UPDATED_DT,'' TRANSPORT_CD,TM.PARKING_TOOL+'-'+a.SUB_DESC PARKING_TOOL,
			             TD.DEPT_NAME_30,TD.DEPT_NAME_40,substring(VE.WORK_SHIFT_CD,2,1) +'-'+c.SUB_DESC as WORK_SHIFT_CD,VE.PLANT_CD+'-'+VE.PLANT_NAME PLANT_CD,d.SUB_DESC CAR_BRAND,
                         ROW_NUMBER() Over (Partition By TM.EMP_ID Order By TM.UPDATED_DT Desc) As Sort 
			             From TB_D_M_PARKING_EMP_MAIN TM
                         left join VW_H_DEPT_DATA TD on  TM.DEPT_NO=TD.DEPT_NO
                         left join TB_D_M_PARKING_MAIN TPM on TPM.CAR_PARK_NO=TM.CAR_PARK_NO
                         left join VW_H_EMP_DATA VE on VE.EMP_ID=TM.EMP_ID
			             left join TB_9_M_COMM_D d on TM.CAR_BRAND = d.SUB_CD AND d.SYS_CD = 'DG' AND d.MAIN_CD = 'CAR_BRAND'
			             left join TB_9_M_COMM_D a on  TM.PARKING_TOOL =a.SUB_CD and a.SYS_CD = 'DG' and a.main_cd = 'PARKING_CD' and a.is_valid = 'Y'
			             left join TB_9_M_COMM_D c on  substring(VE.WORK_SHIFT_CD,2,1) =c.SUB_CD and c.SYS_CD = 'DB' and c.main_cd = 'LINE_CD' and c.is_valid = 'Y'
                         where VE.TRANSFER_SDT <> '' and (VE.TRANSFER_SDT > VE.TRANSFER_EDT or ISNULL(VE.TRANSFER_EDT,'') = '')  
                          )TMP_S 
                         where TMP_S.Sort=1 

                         union all
                         Select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NAME,DEPT_NAME_30,DEPT_NAME_40,WORK_SHIFT_CD,PLANT_CD,CAR_BRAND,CAR_PARK_NO,PARKING_NAME,
			             CAR_NO,UPDATED_DT,TRANSPORT_CD,PARKING_TOOL,'返校'AS'Lapse' From (
                         Select TM.EMP_ID,TM.EMP_NAME,TM.DEPT_NO,TD.DIV_DEPT_FULL_NAME DEPT_NAME,TD.DEPT_NAME_30,TD.DEPT_NAME_40,
			             substring(VE.WORK_SHIFT_CD,2,1)+'-'+c.SUB_DESC as WORK_SHIFT_CD,VE.PLANT_CD+'-'+VE.PLANT_NAME PLANT_CD,d.SUB_DESC CAR_BRAND,
			             TM.CAR_PARK_NO,TPM.PARKING_NAME,TM.CAR_NO,CONVERT(VARCHAR(10),TM.UPDATED_DT, 111) AS UPDATED_DT,'' TRANSPORT_CD,TM.PARKING_TOOL+'-'+a.SUB_DESC PARKING_TOOL,
                         ROW_NUMBER() Over (Partition By TM.EMP_ID Order By TM.UPDATED_DT Desc) As Sort 
			             From TB_D_M_PARKING_EMP_MAIN TM            
                         left join VW_H_DEPT_DATA TD on  TM.DEPT_NO=TD.DEPT_NO
                         left join TB_D_M_PARKING_MAIN TPM on TPM.CAR_PARK_NO=TM.CAR_PARK_NO
                         left join VW_H_EMP_DATA VE on VE.EMP_ID=TM.EMP_ID
			             left join TB_9_M_COMM_D d on TM.CAR_BRAND = d.SUB_CD AND d.SYS_CD = 'DG' AND d.MAIN_CD = 'CAR_BRAND'
			             left join TB_9_M_COMM_D a on  TM.PARKING_TOOL =a.SUB_CD and a.SYS_CD = 'DG' and a.main_cd = 'PARKING_CD' and a.is_valid = 'Y'
			             left join TB_9_M_COMM_D c on  substring(VE.WORK_SHIFT_CD,2,1) =c.SUB_CD and c.SYS_CD = 'DB' and c.main_cd = 'LINE_CD' and c.is_valid = 'Y'
                         where VE.BACK_SCHOOL_DT > VE.BACK_PLANT_DT
                          )TMP_S 
                         where TMP_S.Sort=1 

                         union all
                         Select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NAME,DEPT_NAME_30,DEPT_NAME_40,WORK_SHIFT_CD,PLANT_CD,CAR_BRAND,
			             CAR_PARK_NO,PARKING_NAME,CAR_NO,UPDATED_DT,TRANSPORT_CD,PARKING_TOOL,HR_CHG_DESC AS'Lapse' From (
                         Select TM.EMP_ID,TM.EMP_NAME,TM.DEPT_NO,TD.DIV_DEPT_FULL_NAME DEPT_NAME,TD.DEPT_NAME_30,TD.DEPT_NAME_40,
			             substring(VE.WORK_SHIFT_CD,2,1)+'-'+c.SUB_DESC as WORK_SHIFT_CD,VE.PLANT_CD+'-'+VE.PLANT_NAME PLANT_CD,d.SUB_DESC CAR_BRAND,
			             TM.CAR_PARK_NO,TPM.PARKING_NAME,TM.CAR_NO,CONVERT(VARCHAR(10),TM.UPDATED_DT, 111) AS UPDATED_DT,'' TRANSPORT_CD,
			             TM.PARKING_TOOL+'-'+a.SUB_DESC PARKING_TOOL,TC.HR_CHG_DESC,
                         ROW_NUMBER() Over (Partition By TM.EMP_ID Order By TM.UPDATED_DT Desc) As Sort From TB_D_M_PARKING_EMP_MAIN TM            
                         left join VW_H_DEPT_DATA TD on  TM.DEPT_NO=TD.DEPT_NO
                         left join TB_D_M_PARKING_MAIN TPM on TPM.CAR_PARK_NO=TM.CAR_PARK_NO
                         left join VW_H_EMP_DATA VE on VE.EMP_ID=TM.EMP_ID
                         left join TB_H_M_HR_CHANGE_CODE TC on TC.HR_CHG_CD=VE.LEAVE_REASON
			             left join TB_9_M_COMM_D d on TM.CAR_BRAND = d.SUB_CD AND d.SYS_CD = 'DG' AND d.MAIN_CD = 'CAR_BRAND'
			             left join TB_9_M_COMM_D a on  TM.PARKING_TOOL =a.SUB_CD and a.SYS_CD = 'DG' and a.main_cd = 'PARKING_CD' and a.is_valid = 'Y'
			             left join TB_9_M_COMM_D c on  substring(VE.WORK_SHIFT_CD,2,1) =c.SUB_CD and c.SYS_CD = 'DB' and c.main_cd = 'LINE_CD' and c.is_valid = 'Y'
                         where VE.LEAVE_DT <> '' and VE.LEAVE_DT > VE.RETENTION_EDT
                          )TMP_S 
                         where TMP_S.Sort=1  ");


            /*
            sb.Append(" Select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NAME,CAR_PARK_NO,PARKING_NAME,CAR_NO,UPDATED_DT,TRANSPORT_CD,PARKING_TOOL,'與交通津貼工具不合'AS'Lapse' From (");
            sb.Append(" Select b.EMP_ID,TM.EMP_NAME,TM.DEPT_NO,TD.DEPT_NAME,TM.CAR_PARK_NO,TPM.PARKING_NAME,TM.CAR_NO,CONVERT(VARCHAR(10),b.UPDATED_DT, 111) AS UPDATED_DT,b.TRANSPORT_CD,TM.PARKING_TOOL,");
            sb.Append(" ROW_NUMBER() Over (Partition By b.EMP_ID Order By b.UPDATED_DT Desc) As Sort From TB_D_M_PARKING_EMP_MAIN TM");
            //sb.Append(" left join TB_D_M_TRANS_ALLOWANCE_D TAD on  TAD.EMP_ID=TM.EMP_ID");
            sb.Append(" left join TB_H_M_DEPT TD on  TM.DEPT_NO=TD.DEPT_NO");
            sb.Append(" left join TB_D_M_PARKING_MAIN TPM on TPM.CAR_PARK_NO=TM.CAR_PARK_NO");
            sb.Append(" left join (select t.CODE_VAL2,s.EMP_ID ,s.UPDATED_DT,s.TRANSPORT_CD from TB_D_M_TRANS_ALLOWANCE_D s");
            sb.Append(" left join TB_9_M_COMM_D t on s.TRANSPORT_CD = t.SUB_CD and t.SYS_CD = 'DD' and t.MAIN_CD = 'TRANSPORT_CD' where CONVERT(varchar,s.END_DT,112)='99991231')b");
            sb.Append(" on TM.EMP_ID = b.EMP_ID where TM.PARKING_TOOL <> b.CODE_VAL2");            
            sb.Append("  )TMP_S ");
            sb.Append(" where TMP_S.Sort=1");
            sb.Append(" union all");
            sb.Append(" Select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NAME,CAR_PARK_NO,PARKING_NAME,CAR_NO,UPDATED_DT,TRANSPORT_CD,PARKING_TOOL,'ICT' AS'Lapse' From (");
            sb.Append(" Select TM.EMP_ID,TM.EMP_NAME,TM.DEPT_NO,TD.DEPT_NAME,TM.CAR_PARK_NO,TPM.PARKING_NAME,TM.CAR_NO,CONVERT(VARCHAR(10),TM.UPDATED_DT, 111) AS UPDATED_DT,'' TRANSPORT_CD,TM.PARKING_TOOL,");
            sb.Append(" ROW_NUMBER() Over (Partition By TM.EMP_ID Order By TM.UPDATED_DT Desc) As Sort From TB_D_M_PARKING_EMP_MAIN TM");
            sb.Append(" left join TB_H_M_DEPT TD on  TM.DEPT_NO=TD.DEPT_NO");
            sb.Append(" left join TB_D_M_PARKING_MAIN TPM on TPM.CAR_PARK_NO=TM.CAR_PARK_NO");
            sb.Append(" left join VW_H_EMP_DATA VE on VE.EMP_ID=TM.EMP_ID");
            sb.Append(" where VE.TRANSFER_SDT <> '' and (VE.TRANSFER_SDT > VE.TRANSFER_EDT or ISNULL(VE.TRANSFER_EDT,'') = '')  ");
            sb.Append("  )TMP_S ");
            sb.Append(" where TMP_S.Sort=1 ");
            sb.Append(" union all");
            sb.Append(" Select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NAME,CAR_PARK_NO,PARKING_NAME,CAR_NO,UPDATED_DT,TRANSPORT_CD,PARKING_TOOL,'返校'AS'Lapse' From (");
            sb.Append(" Select TM.EMP_ID,TM.EMP_NAME,TM.DEPT_NO,TD.DEPT_NAME,TM.CAR_PARK_NO,TPM.PARKING_NAME,TM.CAR_NO,CONVERT(VARCHAR(10),TM.UPDATED_DT, 111) AS UPDATED_DT,'' TRANSPORT_CD,TM.PARKING_TOOL,");
            sb.Append(" ROW_NUMBER() Over (Partition By TM.EMP_ID Order By TM.UPDATED_DT Desc) As Sort From TB_D_M_PARKING_EMP_MAIN TM");
            //sb.Append(" left join TB_D_M_TRANS_ALLOWANCE_D TAD on  TAD.EMP_ID=TM.EMP_ID");
            sb.Append(" left join TB_H_M_DEPT TD on  TM.DEPT_NO=TD.DEPT_NO");
            sb.Append(" left join TB_D_M_PARKING_MAIN TPM on TPM.CAR_PARK_NO=TM.CAR_PARK_NO");
            sb.Append(" left join VW_H_EMP_DATA VE on VE.EMP_ID=TM.EMP_ID");
            sb.Append(" where VE.BACK_SCHOOL_DT > VE.BACK_PLANT_DT");
            sb.Append("  )TMP_S ");
            sb.Append(" where TMP_S.Sort=1 ");
            sb.Append(" union all");
            sb.Append(" Select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NAME,CAR_PARK_NO,PARKING_NAME,CAR_NO,UPDATED_DT,TRANSPORT_CD,PARKING_TOOL,HR_CHG_DESC AS'Lapse' From (");
            sb.Append(" Select TM.EMP_ID,TM.EMP_NAME,TM.DEPT_NO,TD.DEPT_NAME,TM.CAR_PARK_NO,TPM.PARKING_NAME,TM.CAR_NO,CONVERT(VARCHAR(10),TM.UPDATED_DT, 111) AS UPDATED_DT,'' TRANSPORT_CD,TM.PARKING_TOOL,TC.HR_CHG_DESC,");
            sb.Append(" ROW_NUMBER() Over (Partition By TM.EMP_ID Order By TM.UPDATED_DT Desc) As Sort From TB_D_M_PARKING_EMP_MAIN TM");
            //sb.Append(" left join TB_D_M_TRANS_ALLOWANCE_D TAD on  TAD.EMP_ID=TM.EMP_ID");
            sb.Append(" left join TB_H_M_DEPT TD on  TM.DEPT_NO=TD.DEPT_NO");
            sb.Append(" left join TB_D_M_PARKING_MAIN TPM on TPM.CAR_PARK_NO=TM.CAR_PARK_NO");
            sb.Append(" left join VW_H_EMP_DATA VE on VE.EMP_ID=TM.EMP_ID");
            sb.Append(" left join TB_H_M_HR_CHANGE_CODE TC on TC.HR_CHG_CD=VE.LEAVE_REASON");
            sb.Append(" where VE.LEAVE_DT <> '' and VE.LEAVE_DT > VE.RETENTION_EDT");
            sb.Append("  )TMP_S ");
            sb.Append(" where TMP_S.Sort=1 ");
            */  


            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable get_PDF_Data3()
    {
         try
        {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select DISTINCT TM.EMP_ID,TM.EMP_NAME,TE.WS_CD,TM.DEPT_NO,TE.DIV_DEPT_FULL_NAME DEPT_NAME, TE.DEPT_NAME_20,TE.DEPT_NAME_40, ");
        sb.Append(" substring(TE.WORK_SHIFT_CD,2,1) as WORK_SHIFT_CD,TE.PLANT_CD+'-'+b.sub_desc PLANT_CD,TE.PJOB_CD,TP.PJOB_DESC,TM.PARKING_TOOL+'-'+a.sub_desc PARKING_TOOL,TCD.SUB_DESC,");
        sb.Append(" TM.CAR_NO,CONVERT(VARCHAR(10),TM.UPDATED_DT, 120) AS UPDATED_DT,TE.LEVEL_CD,TM.CAR_PARK_NO,PM.PARKING_NAME");
        sb.Append(" from TB_D_M_PARKING_EMP_MAIN TM");
        sb.Append(" left join TB_H_R_DEPT_DATA TD on  TM.DEPT_NO=TD.DEPT_NO");
        sb.Append(" left join VW_H_EMP_DATA TE on  TM.EMP_ID=TE.EMP_ID ");
        sb.Append(" left join TB_H_M_PJOB TP on  TP.PJOB_CD=TE.PJOB_CD");
        sb.Append(" left join TB_9_M_COMM_D TCD on  TM.CAR_BRAND=TCD.SUB_CD and TCD.SYS_CD='DG' and TCD.MAIN_CD='CAR_BRAND'");
        sb.Append(" left join TB_9_M_COMM_D a on  TM.PARKING_TOOL =a.SUB_CD and a.SYS_CD = 'DG' and a.main_cd = 'PARKING_CD' and a.is_valid = 'Y' ");
        sb.Append(" left join TB_9_M_COMM_D b on  TE.PLANT_CD =b.SUB_CD and b.SYS_CD = 'HB' and b.main_cd = 'PLANT_CD' and b.is_valid = 'Y' ");
        sb.Append(" left join TB_D_M_PARKING_MAIN  AS PM ON TM.CAR_PARK_NO = PM.CAR_PARK_NO ");
        
        
       


        return dbConn.Query(sb, ht);
             }
        catch
        {
            throw;
        }
    }
    public DataTable get_PDF_Data4(string UPDATED_DT_S,string UPDATED_DT_E)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select DISTINCT  TM.EMP_ID,TM.EMP_NAME,TM.DEPT_NO,TD.DIV_DEPT_FULL_NAME DEPT_NAME,TD.DEPT_NAME_30,TD.DEPT_NAME_40,
			             substring(TE.WORK_SHIFT_CD,2,1)+'-'+c.SUB_DESC as WORK_SHIFT_CD,TE.PLANT_CD+'-'+TE.PLANT_NAME PLANT_CD,
			             d.SUB_DESC CAR_BRAND,TM.PARKING_TOOL+'-'+a.SUB_DESC PARKING_TOOL,
			             TPM.PARKING_NAME,TM.CAR_NO,TM.UPDATED_BY,CONVERT(VARCHAR(10),TM.UPDATED_DT, 111) AS UPDATED_DT
                         from TB_D_M_PARKING_EMP_MAIN TM
                         left join TB_H_R_DEPT_DATA TD on  TM.DEPT_NO=TD.DEPT_NO
                         left join VW_H_EMP_DATA TE on  TM.EMP_ID=TE.EMP_ID
                         left join TB_H_M_PJOB TP on  TP.PJOB_CD=TE.PJOB_CD             
                         left join TB_D_M_PARKING_MAIN TPM on TPM.CAR_PARK_NO=TM.CAR_PARK_NO
			             left join TB_9_M_COMM_D a on  TM.PARKING_TOOL =a.SUB_CD and a.SYS_CD = 'DG' and a.main_cd = 'PARKING_CD' and a.is_valid = 'Y'
			             left join TB_9_M_COMM_D d on TM.CAR_BRAND = d.SUB_CD AND d.SYS_CD = 'DG' AND d.MAIN_CD = 'CAR_BRAND' 
			             left join TB_9_M_COMM_D c on  substring(TE.WORK_SHIFT_CD,2,1) =c.SUB_CD and c.SYS_CD = 'DB' and c.main_cd = 'LINE_CD' and c.is_valid = 'Y'
                         WHERE TM.UPDATED_DT BETWEEN @UPDATED_DT_S AND @UPDATED_DT_E ;");
            
            //sb.Append(" select DISTINCT  TM.EMP_ID,TM.EMP_NAME,TM.DEPT_NO,TD.DEPT_NAME,TPM.PARKING_NAME,TM.CAR_NO,TM.UPDATED_BY,CONVERT(VARCHAR(10),TM.UPDATED_DT, 111) AS UPDATED_DT");
            //sb.Append(" from TB_D_M_PARKING_EMP_MAIN TM");
            //sb.Append(" left join TB_H_R_DEPT_DATA TD on  TM.DEPT_NO=TD.DEPT_NO");
            //sb.Append(" left join VW_H_EMP_DATA TE on  TM.EMP_ID=TE.EMP_ID");
            //sb.Append(" left join TB_H_M_PJOB TP on  TP.PJOB_CD=TE.PJOB_CD");
            //sb.Append(" left join TB_9_M_COMM_D TCD on  TM.CAR_BRAND=TCD.SUB_CD and TCD.SYS_CD='DG' and TCD.MAIN_CD='CAR_BRAND'");
            //sb.Append(" left join TB_D_M_PARKING_MAIN TPM on TPM.CAR_PARK_NO=TM.CAR_PARK_NO");
            //sb.Append(" WHERE TM.UPDATED_DT BETWEEN @UPDATED_DT_S AND @UPDATED_DT_E;");
            ht.Add("@UPDATED_DT_S", UPDATED_DT_S);
            ht.Add("@UPDATED_DT_E", UPDATED_DT_E);
            
            
            
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable searchResult()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select c.CAR_PARK_NO,c.PARKING_SPOT,g.SUB_DESC,count(b.WORK_SHIFT_CD) as '常日',count(d.WORK_SHIFT_CD) as '紅直', ");
            sb.Append(" count(e.WORK_SHIFT_CD) as '黃直',count(f.WORK_SHIFT_CD) as '其他',");
            sb.Append(" (select count(PLANT_CD) from TB_D_M_PARKING_MAIN where PLANT_CD='1' and PARKING_TYPE='1') as '中壢廠',");
            sb.Append(" (select count(PLANT_CD) from TB_D_M_PARKING_MAIN where PLANT_CD='2' and PARKING_TYPE='1') as '觀音廠',");
            sb.Append(" (select count(distinct(em.CAR_PARK_NO)) from TB_D_M_PARKING_EMP_MAIN em");
            sb.Append(" left join  TB_D_M_PARKING_MAIN pm on em.CAR_PARK_NO = pm.CAR_PARK_NO");
            sb.Append(" where pm.PLANT_CD='1' and pm.PARKING_TYPE='1') as '中壢汽車',");
            sb.Append(" (select count(distinct(em.CAR_PARK_NO)) from TB_D_M_PARKING_EMP_MAIN em");
            sb.Append(" left join  TB_D_M_PARKING_MAIN pm on em.CAR_PARK_NO = pm.CAR_PARK_NO");
            sb.Append(" where pm.PLANT_CD='2' and pm.PARKING_TYPE='1') as '觀音汽車'");           
            sb.Append(" from TB_D_M_PARKING_EMP_MAIN a");
            sb.Append(" left join  TB_H_M_EMP b on a.EMP_ID=b.EMP_ID and SUBSTRING(b.WORK_SHIFT_CD,2,2)='1'");
            sb.Append(" left join  TB_H_M_EMP d on a.EMP_ID=d.EMP_ID and SUBSTRING(d.WORK_SHIFT_CD,2,2)='R'");
            sb.Append(" left join  TB_H_M_EMP e on a.EMP_ID=e.EMP_ID and SUBSTRING(e.WORK_SHIFT_CD,2,2)='Y'");
            sb.Append(" left join  TB_H_M_EMP f on a.EMP_ID=f.EMP_ID and SUBSTRING(f.WORK_SHIFT_CD,2,2)<>'1'and SUBSTRING(f.WORK_SHIFT_CD,2,2)<>'R'and SUBSTRING(f.WORK_SHIFT_CD,2,2)<>'Y'");
            sb.Append(" left join TB_D_M_PARKING_MAIN c on c.CAR_PARK_NO=a.CAR_PARK_NO");
            sb.Append(" left join TB_9_M_COMM_D g on g.MAIN_CD='PARKING_PLANT_CD' and c.PLANT_CD=g.SUB_CD");
            sb.Append(" where c.PARKING_TYPE='1'");
            sb.Append(" GROUP BY c.CAR_PARK_NO,c.PARKING_SPOT,g.SUB_DESC");
            sb.Append(" order by g.SUB_DESC");
           


            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable searchResult2()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select c.CAR_PARK_NO,c.PARKING_SPOT,g.SUB_DESC,count(b.WORK_SHIFT_CD) as '常日',count(d.WORK_SHIFT_CD) as '紅直', ");
            sb.Append(" count(e.WORK_SHIFT_CD) as '黃直',count(f.WORK_SHIFT_CD) as '其他',");
            sb.Append(" (select count(distinct(em.CAR_PARK_NO)) from TB_D_M_PARKING_EMP_MAIN em");
            sb.Append(" left join  TB_D_M_PARKING_MAIN pm on em.CAR_PARK_NO = pm.CAR_PARK_NO");
            sb.Append(" where pm.PLANT_CD='1' and pm.PARKING_TYPE='2') as '中壢機車',");
            sb.Append(" (select count(distinct(em.CAR_PARK_NO)) from TB_D_M_PARKING_EMP_MAIN em");
            sb.Append(" left join  TB_D_M_PARKING_MAIN pm on em.CAR_PARK_NO = pm.CAR_PARK_NO");
            sb.Append(" where pm.PLANT_CD='2' and pm.PARKING_TYPE='2') as '觀音機車',");
            sb.Append(" (select count(PLANT_CD) from TB_D_M_PARKING_MAIN where PLANT_CD='1' and PARKING_TYPE='2') as '中壢廠',");
            sb.Append(" (select count(PLANT_CD) from TB_D_M_PARKING_MAIN where PLANT_CD='2' and PARKING_TYPE='2') as '觀音廠'");
            sb.Append(" from TB_D_M_PARKING_EMP_MAIN a");
            sb.Append(" left join  TB_H_M_EMP b on a.EMP_ID=b.EMP_ID and SUBSTRING(b.WORK_SHIFT_CD,2,2)='1'");
            sb.Append(" left join  TB_H_M_EMP d on a.EMP_ID=d.EMP_ID and SUBSTRING(d.WORK_SHIFT_CD,2,2)='R'");
            sb.Append(" left join  TB_H_M_EMP e on a.EMP_ID=e.EMP_ID and SUBSTRING(e.WORK_SHIFT_CD,2,2)='Y'");
            sb.Append(" left join  TB_H_M_EMP f on a.EMP_ID=f.EMP_ID and SUBSTRING(f.WORK_SHIFT_CD,2,2)<>'1'and SUBSTRING(f.WORK_SHIFT_CD,2,2)<>'R'and SUBSTRING(f.WORK_SHIFT_CD,2,2)<>'Y'");
            sb.Append(" left join TB_D_M_PARKING_MAIN c on c.CAR_PARK_NO=a.CAR_PARK_NO");
            sb.Append(" left join TB_9_M_COMM_D g on g.MAIN_CD='PARKING_PLANT_CD' and c.PLANT_CD=g.SUB_CD");
            sb.Append(" where c.PARKING_TYPE='2'");
            sb.Append(" GROUP BY c.CAR_PARK_NO,c.PARKING_SPOT,g.SUB_DESC");
            sb.Append(" order by g.SUB_DESC");



            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void addData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_M_SALARY_REPORT_H (");
            sb.Append(" SALARY_DT, SALARY_TYPE, DATA_YM, SALARY_ID_SEQ,");
            sb.Append(" SALARY_ID, SALARY_NAME, IS_PLUS, IS_TAX,");
            sb.Append(" CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" SELECT");
            sb.Append(" S.SALARY_DT, S.SALARY_TYPE, S.DATA_YM,");
            sb.Append(" ROW_NUMBER() over(PARTITION BY S.IS_PLUS ORDER BY S.SALARY_DT, S.SALARY_TYPE, S.DATA_YM, S.IS_PLUS, S.SALARY_ID) SALARY_ID_SEQ,");
            sb.Append(" S.SALARY_ID, S.SALARY_NAME, S.IS_PLUS, S.IS_TAX,");
            sb.Append(" @login_emp_id, GETDATE(), @login_emp_idID, GETDATE(), 'FB2SC530' FUNC_ID");
            sb.Append(" FROM(");
            sb.Append(" SELECT  SALARY_DT, SALARY_TYPE, DATA_YM, SALARY_ID, SALARY_NAME, IS_PLUS, IS_TAX, COUNT(*) CNT");
            sb.Append(" FROM TB_S_M_SALARY_PAY");
            sb.Append(" WHERE P.SALARY_DT BETWEEN @DATA_YMS AND @DATA_YME");
            sb.Append(" ");
            sb.Append(" ");
            sb.Append(" GROUP BY SALARY_DT, SALARY_TYPE, DATA_YM, SALARY_ID, SALARY_NAME, IS_PLUS, IS_TAX) S");
            sb.Append(" ORDER BY S.SALARY_DT, S.SALARY_TYPE, S.DATA_YM, S.IS_PLUS, SALARY_ID_SEQ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@DEPT_NO", DEPT_NO);
            //ht.Add("@DEPT_NAME", DEPT_NAME);
            //ht.Add("@LEVEL_CD", LEVEL_CD);
            //ht.Add("@PJOB_DESC", PJOB_DESC);
            //ht.Add("@DOC_NO", DOC_NO);
            //ht.Add("@START_DT", START_DT);
            //ht.Add("@JUDGEMENT_TYPE", JUDGEMENT_TYPE);
            //ht.Add("@REASON_CD", REASON_CD);
            //ht.Add("@FIRST_CNT", FIRST_CNT);
            //ht.Add("@SECOND_CNT", SECOND_CNT);
            //ht.Add("@THIRD_CNT", THIRD_CNT);
            //ht.Add("@IS_FIRE", IS_FIRE);
            ht.Add("@REMARK", REMARK);
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
    public string deleteData(string login_emp_id)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" Delete From TB_S_M_SALARY_REPORT_H  ");
        sb.Append(" Where CREATED_BY = @login_emp_id");
        sb.Append(" Delete From TB_S_M_SALARY_REPORT_D  ");
        sb.Append(" Where CREATED_BY = @login_emp_id");
        ht.Add("@login_emp_id", login_emp_id);
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }

}