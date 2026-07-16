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
/// CFB2DC0300DAO 的摘要描述
/// </summary>
public class CFB2DC0300DAO : BaseDAO
{
    public string VENDOR_NO { get; set; }
    public string VENDOR_NAME { get; set; }
    public string REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    public string VENDOR_MEMBER_NO { get; set; }
    public string VENDOR_MEMBER_NAME { get; set; }

    public CFB2DC0300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string vendor_no, string vendor_name)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" VENDOR_NO,VENDOR_NAME,REMARK");
            sb.Append(" from TB_D_M_VENDOR_H ");
            sb.Append(" where 1=1 ");

            if (vendor_name != "")
            {
                sb.Append(" and VENDOR_NAME like @VENDOR_NAME ");
                ht.Add("@VENDOR_NAME", vendor_name +'%');
            }
            if (vendor_no != "")
            {
                sb.Append(" and VENDOR_NO = @VENDOR_NO ");
                ht.Add("@VENDOR_NO", vendor_no);
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

    public int getCount(int startRowIndex, int maximumRows, string vendor_no, string vendor_name)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_VENDOR_H");
            sb.Append(" where 1=1 ");
            if (vendor_name != "")
            {
                sb.Append(" and VENDOR_NAME like @VENDOR_NAME ");
                ht.Add("@VENDOR_NAME", vendor_name + '%');
            }
            if (vendor_no != "")
            {
                sb.Append(" and VENDOR_NO = @VENDOR_NO ");
                ht.Add("@VENDOR_NO", vendor_no);
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

    public DataTable getData2(int startRowIndex, int maximumRows, string sortExpression, string vendor_no, string vendor_member_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" VENDOR_MEMBER_NO,VENDOR_MEMBER_NAME");
            sb.Append(" from TB_D_M_VENDOR_D ");
            sb.Append(" where 1=1 ");

            if (vendor_no != "")
            {
                sb.Append(" and VENDOR_NO = @VENDOR_NO ");
                ht.Add("@VENDOR_NO", vendor_no);
            }

            if (vendor_member_no != "")
            {
                sb.Append(" and VENDOR_MEMBER_NO = @VENDOR_MEMBER_NO ");
                ht.Add("@VENDOR_MEMBER_NO", vendor_member_no);
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

    public int getCount2(int startRowIndex, int maximumRows, string vendor_no, string vendor_member_no)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_VENDOR_D");
            sb.Append(" where 1=1 ");

            if (vendor_no != "")
            {
                sb.Append(" and VENDOR_NO = @VENDOR_NO ");
                ht.Add("@VENDOR_NO", vendor_no);
            }

            if (vendor_member_no != "")
            {
                sb.Append(" and VENDOR_MEMBER_NO = @VENDOR_MEMBER_NO ");
                ht.Add("@VENDOR_MEMBER_NO", vendor_member_no);
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

    public void deleteVENDOR_H(string item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_VENDOR_H set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DC030' ");
            sb.Append(" where VENDOR_NO = @VENDOR_NO;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_VENDOR_H");
            sb.Append(" where VENDOR_NO = @VENDOR_NO;");
            ht.Add("@VENDOR_NO", item);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void deleteVENDOR_D(string item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_VENDOR_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DC030' ");
            sb.Append(" where VENDOR_NO = @VENDOR_NO;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_VENDOR_D");
            sb.Append(" where VENDOR_NO = @VENDOR_NO;");
            ht.Add("@VENDOR_NO", item);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select VENDOR_NO from TB_D_M_VENDOR_H");
            sb.Append(" where VENDOR_NO = @VENDOR_NO");
            ht.Add("@VENDOR_NO", VENDOR_NO);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public void addVENDOR_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_VENDOR_H( ");
            sb.Append(" VENDOR_NO,VENDOR_NAME,REMARK,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @VENDOR_NO,@VENDOR_NAME,@REMARK,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@VENDOR_NO", VENDOR_NO);
            ht.Add("@VENDOR_NAME", VENDOR_NAME);
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

    public void updateVENDOR_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_VENDOR_H ");
            sb.Append(" set VENDOR_NAME=@VENDOR_NAME,REMARK=@REMARK,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where VENDOR_NO=@VENDOR_NO ");

            ht.Add("@VENDOR_NO", VENDOR_NO);
            ht.Add("@VENDOR_NAME", VENDOR_NAME);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void deleteVENDOR_D(string vendor_no, string item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_VENDOR_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DC030' ");
            sb.Append(" where VENDOR_NO = @VENDOR_NO and VENDOR_MEMBER_NO=@VENDOR_MEMBER_NO;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_VENDOR_D");
            sb.Append(" where VENDOR_NO = @VENDOR_NO and VENDOR_MEMBER_NO=@VENDOR_MEMBER_NO;");
            ht.Add("@VENDOR_NO", vendor_no);
            ht.Add("@VENDOR_MEMBER_NO", item);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getVENDOR_NO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select VENDOR_NO,VENDOR_NAME from TB_D_M_VENDOR_H");
            sb.Append(" where VENDOR_NO = @VENDOR_NO");
            ht.Add("@VENDOR_NO", VENDOR_NO);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //取得 TB_D_M_CARD 卡片資料檔
    public DataTable getCARDData(string item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select a.CARD_TYPE from TB_D_M_CARD a ");
            sb.Append(" left join TB_D_M_CARD_TYPE b on a.CARD_TYPE = b.CARD_TYPE ");
            sb.Append(" where a.PERSON_ID = @PERSON_ID and b.CARD_USED_CD = 'B' ");
            ht.Add("@PERSON_ID", item);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //取得 TB_D_M_VENDOR_D 廠商人員明細檔
    public DataTable getExistData2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select VENDOR_NO from TB_D_M_VENDOR_D");
            sb.Append(" where VENDOR_NO = @VENDOR_NO and VENDOR_MEMBER_NO=@VENDOR_MEMBER_NO");
            ht.Add("@VENDOR_NO", VENDOR_NO);
            ht.Add("@VENDOR_MEMBER_NO", VENDOR_MEMBER_NO);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public void addVENDOR_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_VENDOR_D( ");
            sb.Append(" VENDOR_NO,VENDOR_MEMBER_NO,VENDOR_MEMBER_NAME,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @VENDOR_NO,@VENDOR_MEMBER_NO,@VENDOR_MEMBER_NAME,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@VENDOR_NO", VENDOR_NO);
            ht.Add("@VENDOR_MEMBER_NO", VENDOR_MEMBER_NO);
            ht.Add("@VENDOR_MEMBER_NAME", VENDOR_MEMBER_NAME);
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

    public void updateVENDOR_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_VENDOR_D ");
            sb.Append(" set VENDOR_MEMBER_NAME=@VENDOR_MEMBER_NAME,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where VENDOR_NO = @VENDOR_NO and VENDOR_MEMBER_NO=@VENDOR_MEMBER_NO");

            ht.Add("@VENDOR_NO", VENDOR_NO);
            ht.Add("@VENDOR_MEMBER_NO", VENDOR_MEMBER_NO);
            ht.Add("@VENDOR_MEMBER_NAME", VENDOR_MEMBER_NAME);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int SP_D_UPD_CARD_DATA(string item)
    {
        try
        {
            //(U.修改, 廠商人員編號, B.社外, null, 系統日期, 登入者帳號, 更新作業FunctionID)
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_UPD_CARD_DATA");
            ht.Add("@pHandleCd", "U");
            ht.Add("@pEmpId", item);
            ht.Add("@pCardUsedCd", "B"); //社外
            ht.Add("@pStartDt", DBNull.Value);
            ht.Add("@pEndDt", DateTime.Now);
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2DC030");
            int result = dbConn.ExecuteSP(sb, ht, true);
            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public int SP_D_UPD_CARD_DATA2()
    {
        try
        {
            //(I1.新增, 明細畫面.廠商人員編號, B.社外, 系統日期, 9999/12/31, 登入者帳號, 更新作業FunctionID)
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_UPD_CARD_DATA");
            ht.Add("@pHandleCd", "I");
            ht.Add("@pEmpId", VENDOR_MEMBER_NO);
            ht.Add("@pCardUsedCd", "B"); //社外
            ht.Add("@pStartDt", DateTime.Now);
            ht.Add("@pEndDt", "9999/12/31");
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2DC030");
            int result = dbConn.ExecuteSP(sb, ht, true);
            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getVENDOR_MEMBER_NAME(string vendor_no, string vendor_member_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select VENDOR_MEMBER_NO,VENDOR_MEMBER_NAME ");
            sb.Append(" from TB_D_M_VENDOR_D ");
            sb.Append(" where VENDOR_NO=@VENDOR_NO and VENDOR_MEMBER_NO=@VENDOR_MEMBER_NO ");
            ht.Add("@VENDOR_NO", vendor_no);
            ht.Add("@VENDOR_MEMBER_NO", vendor_member_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得現有資料(廠商主檔)
    public DataTable getTB_D_M_VENDOR_D(string item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select b.VENDOR_MEMBER_NO ");
            sb.Append(" from TB_D_M_VENDOR_H a ");
            sb.Append(" left join TB_D_M_VENDOR_D b on a.VENDOR_NO=b.VENDOR_NO ");
            sb.Append(" where a.VENDOR_NO=@VENDOR_NO ");
            sb.Append(" and b.VENDOR_MEMBER_NO in( ");
            sb.Append(" select PERSON_ID from TB_D_M_CARD c1 ");
            sb.Append(" left join TB_D_M_CARD_TYPE c2 on c1.CARD_TYPE = c2.CARD_TYPE ");
            sb.Append(" where c2.CARD_USED_CD = 'B' ) "); //卡片使用對象代碼 A.社內  B.社外  C.共用
            ht.Add("@VENDOR_NO", item);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}