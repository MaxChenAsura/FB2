using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using ACESLib;

/// <summary>
/// HrChangeCode_Search 的摘要描述
/// </summary>
public class HrChangeCode_Search : BaseDAO
{
    public string HR_CHG_CD { get; set; }
    public string HR_CHG_DESC { get; set; }
    public string UPD_RIGHT_CD { get; set; }
    public string IS_FOR_BATCH { get; set; }
    public string IS_FOR_TRANSFER_IN { get; set; }
    public string IS_VALID { get; set; }
    public string EMP_ID { get; set; }
    public string FUNC_ID { get; set; }

    private string strIsDEPT = "";
    private string strDepartments = "";
    private string strSysCodeAtt = "";

    public HrChangeCode_Search()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public System.Data.DataTable getHrChangeCodeData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select HR_CHG_CD,HR_CHG_DESC,UPD_RIGHT_CD,IS_FOR_TRANSFER_IN from TB_H_M_HR_CHANGE_CODE a where HR_CHG_CD is not null ");

            if (HR_CHG_CD != "")
            {
                sb.Append(" and HR_CHG_CD like @HR_CHG_CD");
                ht.Add("@HR_CHG_CD", "%" + HR_CHG_CD + "%");
            }
            if (HR_CHG_DESC != "")
            {
                sb.Append(" and HR_CHG_DESC like @HR_CHG_DESC");
                ht.Add("@HR_CHG_DESC", "%" + HR_CHG_DESC + "%");
            }
            if (UPD_RIGHT_CD != "")
            {
                sb.Append(" and UPD_RIGHT_CD = @UPD_RIGHT_CD");
                ht.Add("@UPD_RIGHT_CD", UPD_RIGHT_CD);
            }
            if (IS_FOR_BATCH != "")
            {
                sb.Append(" and IS_FOR_BATCH = @IS_FOR_BATCH");
                ht.Add("@IS_FOR_BATCH", IS_FOR_BATCH);
            }
            if (IS_FOR_TRANSFER_IN == "3")
            {
                sb.Append(" and IS_FOR_TRANSFER_IN = 'Y'");
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
            if (FUNC_ID == "FB2HC010_ADD") {
                getAuth(ref strIsDEPT, ref strDepartments, ref strSysCodeAtt);
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
                if (HR_CHG_CD != "")
                {
                    sb.Append(" and HR_CHG_CD like @HR_CHG_CD");
                    ht.Add("@HR_CHG_CD", "%" + HR_CHG_CD + "%");
                }
                if (HR_CHG_DESC != "")
                {
                    sb.Append(" and HR_CHG_DESC like @HR_CHG_DESC");
                    ht.Add("@HR_CHG_DESC", "%" + HR_CHG_DESC + "%");
                }
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
                getAuth(ref strIsDEPT, ref strDepartments, ref strSysCodeAtt);
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

            sb.Append(" order by " + sortExpression);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void getAuth(ref string strIsDEPT, ref string strDepartments, ref string strSysCodeAtt)
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
                    }
                }
            }
        }
    }

}