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
/// CFB2SL4100BO 的摘要描述
/// </summary>
public class CFB2SL4100BO : BaseService
{
    public CFB2SL4100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable getEmpName(string emp_id)
    {
        try
        {
            CFB2SL4100DAO dao = new CFB2SL4100DAO();

            return dao.getEmpName(emp_id);

            //if (value == "1")
            //    return dao.getEmpName(emp_id);
            //else
            //    return dao.getVENDOR_MEMBER_NAME(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }
}