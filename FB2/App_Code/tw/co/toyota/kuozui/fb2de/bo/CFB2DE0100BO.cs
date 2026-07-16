using FB2.tw.co.toyota.kuozui.bo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// CFB2DE0100BO 的摘要描述
/// </summary>
public class CFB2DE0100BO : BaseService
{
	public CFB2DE0100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public int update_TB_D_M_RES_PARA(string emp_company_cd, string BfAount, string DnAmount, string BrStart, string BrEnd, string DnStart,
                        string DnEnd, string LastBrTime, string LastDnTime, string CourseDnTime, string emp_id, string func_id, string md_start, string md_end)
    { 
        try 
	    {
            CFB2DE0100DAO dao = new CFB2DE0100DAO();
            return dao.update_TB_D_M_RES_PARA(emp_company_cd, BfAount, DnAmount, BrStart, BrEnd, DnStart,
                                                DnEnd, LastBrTime, LastDnTime, CourseDnTime, emp_id, func_id, md_start, md_end);
	    }
	    catch (Exception)
	    {
		
		    throw;
	    }
        
    }

    public int InsertData_TB_D_M_RES_PARA(string emp_company_cd, string BfAount, string DnAmount, string BrStart, string BrEnd, string DnStart,
                        string DnEnd, string LastBrTime, string LastDnTime, string CourseDnTime, string emp_id, string func_id,string md_start,string md_end)
    {
        try
        {
            CFB2DE0100DAO dao = new CFB2DE0100DAO();
            return dao.InsertData_TB_D_M_RES_PARA(emp_company_cd, BfAount, DnAmount, BrStart, BrEnd, DnStart,
                                                DnEnd, LastBrTime, LastDnTime, CourseDnTime, emp_id, func_id,md_start,md_end);
        }
        catch (Exception)
        {

            throw;
        }

    }

    public DataTable getTB_D_M_RES_PARA(string emp_company_cd)
    {
        try
        {
            CFB2DE0100DAO dao = new CFB2DE0100DAO();
            return dao.getTB_D_M_RES_PARA(emp_company_cd);
        }
        catch (Exception)
        {

            throw;
        }

    }



}