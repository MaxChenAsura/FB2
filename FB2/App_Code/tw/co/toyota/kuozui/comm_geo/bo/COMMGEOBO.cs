using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
/// <summary>
/// CFB2HD0100BO 的摘要描述
/// </summary>
public class COMMGEOBO : BaseService
{
    public COMMGEOBO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
 
    # region Qry
    //取得員工基本資料
    public DataTable getEMPFile(string emp_id)
    {
        try
        {
            COMMGEODAO commgeo = new COMMGEODAO();
            commgeo.EMP_ID = emp_id;
            return commgeo.getEMPFile();

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getINS2_DETAIL_TMP(COMMGEODAO commgeo)
    {
        try
        {
            return commgeo.getINS2_DETAIL_TMP();

        }
        catch (Exception)
        {

            throw;
        }
    }
    
    public DataTable getCommData(string SUB_CD, string MAIN_CD, string SYS_CD)
    {
        try
        {
            COMMGEODAO commgeo = new COMMGEODAO();
            commgeo.SUB_CD = SUB_CD;
            commgeo.MAIN_CD = MAIN_CD;
            commgeo.SYS_CD = SYS_CD;
            return commgeo.getCommData();

        }
        catch (Exception)
        {

            throw;
        }
    }
    
    public DataTable getCHANGE_CODEFile(string HR_CHG_CD)
    {
        try
        {
            COMMGEODAO commgeo = new COMMGEODAO();
            commgeo.HR_CHG_CD = HR_CHG_CD;
            return commgeo.getCHANGE_CODEFile();

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getSALARYFile(COMMGEODAO commgeo)
    {
        try
        {
            return commgeo.getSALARYFile();

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSALARYFile(string SALARY_ID, string EMP_ID)
    {
        try
        {
            CFB2SB2300DAO wfb2sb = new CFB2SB2300DAO();
            wfb2sb.SALARY_ID = SALARY_ID;
            wfb2sb.EMP_ID = EMP_ID;
            return wfb2sb.getSALARYFile();

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getSALARYPAYDATA(string REMIT_DT, string SALARY_TYPE)
    {
        try
        {
            COMMGEODAO commgeo = new COMMGEODAO();
            commgeo.REMIT_DT = REMIT_DT;
            commgeo.SALARY_TYPE = SALARY_TYPE;
            return commgeo.getSALARYPAYDATA();

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getPJOBDATA(string PJOB_CD, string START_DT)
    {
        try
        {
            COMMGEODAO commgeo = new COMMGEODAO();
            commgeo.PJOB_CD = PJOB_CD;
            commgeo.START_DT = START_DT;
            return commgeo.getPJOBDATA();

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getWORKSHIFTDATA(string WORK_SHIFT_CD)
    {
        try
        {
            COMMGEODAO commgeo = new COMMGEODAO();
            commgeo.WORK_SHIFT_CD = WORK_SHIFT_CD;
            return commgeo.getWORKSHIFTDATA();

        }
        catch (Exception)
        {

            throw;
        }
    }

    //所有的部門資料
    public DataTable getDeptAllData(string dept_no)
    {
        try
        {
            COMMGEODAO commgeo = new COMMGEODAO();
            commgeo.DEPT_NO = dept_no;
            return commgeo.getDeptAllData();

        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得主假別相關資訊
    public DataTable getMAIN_LEAVE_DESC(string main_leave_cd)
    {
        try
        {
            COMMGEODAO commgeo = new COMMGEODAO();
            return commgeo.getMAIN_LEAVE_DESC(main_leave_cd);
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
            COMMGEODAO commgeo = new COMMGEODAO();
            return commgeo.getSUB_LEAVE_DESC(sub_leave_cd);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion
}