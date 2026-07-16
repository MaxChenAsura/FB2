using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Kuozui;
using System.Data;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// Class1 的摘要描述
/// </summary>
public class UCCommCodeDropDwonListDAO : BaseDAO
{
    #region field
    private string _TextField;
    private string _ValueField;
    private string _MAIN_CD;
    private string _SUB_CD;
    private string _SUB_DESC;
    private string _CODE_VAL1;
    private string _CODE_VAL2;
    private string _REMARK;
    private int? _ORDER_SEQ;
    private string _MAIN_DESC;
    private BooleanProperty _IS_VALID;
    private BooleanProperty _USER_UPD;
    private string _WhereSYS_CDs;
    private string _WhereMAIN_CDs;
    private string _WhereSUB_CDs;
    private string _WhereCODE_VAL1s;
    private string _WhereCODE_VAL2s;
    private BooleanProperty _WhereIS_VALID;
    private BooleanProperty _WhereUSER_UPD;

    private CommCode_ORDER_SEQ _QueryOrderSeq;
    #endregion

    #region properties

    public string ValueField
    {
        get { return _ValueField; }
        set { _ValueField = value; }
    }

    public string TextField
    {
        get { return _TextField; }
        set { _TextField = value; }
    }

    public string MAIN_CD
    {
        get { return _MAIN_CD; }
        set { _MAIN_CD = value; }
    }

    public string SUB_CD
    {
        get { return _SUB_CD; }
        set { _SUB_CD = value; }
    }

    public string SUB_DESC
    {
        get { return _SUB_DESC; }
        set { _SUB_DESC = value; }
    }

    public string CODE_VAL1
    {
        get { return _CODE_VAL1; }
        set { _CODE_VAL1 = value; }
    }

    public string CODE_VAL2
    {
        get { return _CODE_VAL2; }
        set { _CODE_VAL2 = value; }
    }

    public string REMARK
    {
        get { return _REMARK; }
        set { _REMARK = value; }
    }

    public int? ORDER_SEQ
    {
        get { return _ORDER_SEQ; }
        set { _ORDER_SEQ = value; }
    }

    public string MAIN_DESC
    {
        get { return _MAIN_DESC; }
        set { _MAIN_DESC = value; }
    }

    public BooleanProperty IS_VALID
    {
        get { return _IS_VALID; }
        set { _IS_VALID = value; }
    }

    public BooleanProperty USER_UPD
    {
        get { return _USER_UPD; }
        set { _USER_UPD = value; }
    }

    public string WhereSYS_CDs
    {
        get { return _WhereSYS_CDs; }
        set { _WhereSYS_CDs = value; }
    }

    public string WhereMAIN_CDs
    {
        get { return _WhereMAIN_CDs; }
        set { _WhereMAIN_CDs = value; }
    }

    public string WhereSUB_CDs
    {
        get { return _WhereSUB_CDs; }
        set { _WhereSUB_CDs = value; }
    }

    public string WhereCODE_VAL1s
    {
        get { return _WhereCODE_VAL1s; }
        set { _WhereCODE_VAL1s = value; }
    }

    public string WhereCODE_VAL2s
    {
        get { return _WhereCODE_VAL2s; }
        set { _WhereCODE_VAL2s = value; }
    }

    public BooleanProperty WhereIS_VALID
    {
        get { return _WhereIS_VALID; }
        set { _WhereIS_VALID = value; }
    }

    public BooleanProperty WhereUSER_UPD
    {
        get { return _WhereUSER_UPD; }
        set { _WhereUSER_UPD = value; }
    }

    public CommCode_ORDER_SEQ QueryOrderSeq
    {
        get { return _QueryOrderSeq; }
        set { _QueryOrderSeq = value; }
    }


    #endregion

}

/// <summary>
/// Class1 的摘要描述
/// </summary>
public class UCCommCodeDropDwonListDL : BaseDAO
{
    public List<UCCommCodeDropDwonListDAO> getData(string TextFields, string DataTextFormatString, string ValueFields, string DataValueFormatString, UCCommCodeDropDwonListDAO dao)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string strNewCols = string.Empty;
            string strT9MCH_Cols = ",SYS_CD,MAIN_CD,MAIN_DESC,IS_VALID,USER_UPD,";
            string[] arrTextFields = TextFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] arrValueFields = ValueFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (arrTextFields.Length == 1)
            {
                if (strT9MCH_Cols.Contains("," + arrTextFields[0] + ","))
                    strNewCols += ",T9MCH." + arrTextFields[0] + " as TextField";
                else
                    strNewCols += ",T9MCD." + arrTextFields[0] + " as TextField";
            }
            else
            {
                for (int i = 0; i < arrTextFields.Length; i++)
                {
                    if (strT9MCH_Cols.Contains("," + arrTextFields[i] + ","))
                        arrTextFields[i] = "T9MCH." + arrTextFields[i];
                    else
                        arrTextFields[i] += "T9MCD." + arrTextFields[i];
                }
                DataTextFormatString = DataTextFormatString.Replace("{", "'+{");
                DataTextFormatString = DataTextFormatString.Replace("}", "}+'");
                DataTextFormatString = DataTextFormatString.Trim('\'').Trim('+');
                string TextFieldCol = string.Format(DataTextFormatString, TextFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                strNewCols += "," + TextFieldCol + " as TextField";
            }

            if (arrValueFields.Length == 1)
            {
                if (strT9MCH_Cols.Contains("," + arrValueFields[0] + ","))
                    strNewCols += ",T9MCH." + arrValueFields[0] + " as ValueField";
                else
                    strNewCols += ",T9MCD." + arrValueFields[0] + " as ValueField";
            }
            else
            {
                for (int i = 0; i < arrValueFields.Length; i++)
                {
                    if (strT9MCH_Cols.Contains("," + arrValueFields[i] + ","))
                        arrValueFields[i] = "T9MCH." + arrValueFields[i];
                    else
                        arrValueFields[i] += "T9MCD." + arrValueFields[i];
                }

                DataValueFormatString = DataValueFormatString.Replace("{", "+'{");
                DataValueFormatString = DataValueFormatString.Replace("}", "}'+");
                DataValueFormatString = DataValueFormatString.Trim('+').Trim('\'');

                string DataValueCol = string.Format(DataValueFormatString, ValueFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                strNewCols += "," + DataValueCol + " as ValueField";

            }

            if (dao.QueryOrderSeq == CommCode_ORDER_SEQ.DESC || dao.QueryOrderSeq == CommCode_ORDER_SEQ.ASC)
                 sb.Append(" select distinct T9MCD.ORDER_SEQ," + strNewCols.Trim(',') + "\r\n");
            else
                 sb.Append(" select distinct " + strNewCols.Trim(',') + "\r\n");

             sb.Append(" from TB_9_M_COMM_H T9MCH\r\n");
             sb.Append(" join TB_9_M_COMM_D T9MCD on T9MCH.MAIN_CD=T9MCD.MAIN_CD\r\n");
             sb.Append(" where 1=1\r\n");

            if (string.IsNullOrEmpty(dao.WhereSYS_CDs) == false)
            {
                string[] ArrSysCds = dao.WhereSYS_CDs.Trim().Trim(',').Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                string strSysCdsInSqls = string.Empty;
                for (int i = 0; i < ArrSysCds.Length; i++)
                {
                    strSysCdsInSqls += "@SYS_CD" + i.ToString() + ",";
                    ht.Add("@SYS_CD" + i.ToString(), ArrSysCds[i]);
                }
                if (string.IsNullOrEmpty(strSysCdsInSqls) == false)
                     sb.Append(" and isnull(T9MCH.SYS_CD,'')in (" + strSysCdsInSqls.Trim(',') + ") \r\n");

            }

            if (string.IsNullOrEmpty(dao.WhereMAIN_CDs) == false)
            {
                string[] ArrMainCds = dao.WhereMAIN_CDs.Trim().Trim(',').Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                string strMainCdsInSqls = string.Empty;
                for (int i = 0; i < ArrMainCds.Length; i++)
                {
                    strMainCdsInSqls += "@MAIN_CD" + i.ToString() + ",";
                    ht.Add("@MAIN_CD" + i.ToString(), ArrMainCds[i]);
                }
                if (string.IsNullOrEmpty(strMainCdsInSqls) == false)
                     sb.Append(" and isnull(T9MCH.MAIN_CD,'') in (" + strMainCdsInSqls.Trim(',') + ") \r\n");
            }

            if (string.IsNullOrEmpty(dao.WhereSUB_CDs) == false)
            {
                string[] ArrSubCds = dao.WhereSUB_CDs.Trim().Trim(',').Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                string strSubCdsInSqls = string.Empty;
                for (int i = 0; i < ArrSubCds.Length; i++)
                {
                    strSubCdsInSqls += "@SUB_CD" + i.ToString() + ",";
                    ht.Add("@SUB_CD" + i.ToString(), ArrSubCds[i]);
                }
                if (string.IsNullOrEmpty(strSubCdsInSqls) == false)
                     sb.Append(" and isnull(T9MCD.SUB_CD,'') in (" + strSubCdsInSqls.Trim(',') + ") \r\n");
            }

            if (string.IsNullOrEmpty(dao.WhereCODE_VAL1s) == false)
            {
                string[] ArrCodeValue1s = dao.WhereCODE_VAL1s.Trim().Trim(',').Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                string strCodeValue1sInSqls = string.Empty;
                for (int i = 0; i < ArrCodeValue1s.Length; i++)
                {
                    strCodeValue1sInSqls += "@CODE_VAL1_" + i.ToString() + ",";
                    ht.Add("@CODE_VAL1_" + i.ToString(), ArrCodeValue1s[i]);
                }
                if (string.IsNullOrEmpty(strCodeValue1sInSqls) == false)
                     sb.Append(" and isnull(T9MCD.CODE_VAL1,'') in (" + strCodeValue1sInSqls.Trim(',') + ") \r\n");
            }

            if (string.IsNullOrEmpty(dao.WhereCODE_VAL2s) == false)
            {
                string[] ArrCodeValue2s = dao.WhereSUB_CDs.Trim().Trim(',').Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                string strCodeValue2sInSqls = string.Empty;
                for (int i = 0; i < ArrCodeValue2s.Length; i++)
                {
                    strCodeValue2sInSqls += "@CODE_VAL2_" + i.ToString() + ",";
                    ht.Add("@CODE_VAL2_" + i.ToString(), ArrCodeValue2s[i]);
                }
                if (string.IsNullOrEmpty(strCodeValue2sInSqls) == false)
                     sb.Append(" and isnull(T9MCD.CODE_VAL2,'') in (" + strCodeValue2sInSqls.Trim(',') + ") \r\n");
            }

            //if (dao.WhereIS_VALID == BooleanProperty.True)
            //{
            //     sb.Append(" and T9MCH.IS_VALID='Y'\r\n");

            //}
            //if (dao.WhereIS_VALID == BooleanProperty.False)
            //     sb.Append(" and T9MCH.IS_VALID='N'\r\n");

            if (dao.WhereUSER_UPD == BooleanProperty.True)
                 sb.Append(" and T9MCH.USER_UPD='Y'\r\n");

            if (dao.WhereUSER_UPD == BooleanProperty.False)
                 sb.Append(" and T9MCH.USER_UPD='N'\r\n");

            if (dao.QueryOrderSeq == CommCode_ORDER_SEQ.DESC)
                 sb.Append(" order by ORDER_SEQ DESC \r\n");

            if (dao.QueryOrderSeq == CommCode_ORDER_SEQ.ASC)
                 sb.Append(" order by ORDER_SEQ ASC \r\n");

            return (from item in dbConn.Query(sb, ht).AsEnumerable()
                    select new UCCommCodeDropDwonListDAO
                    {
                        ValueField = (item.Table.Columns.Contains("ValueField") ? item.Field<string>("ValueField") : null),
                        TextField = (item.Table.Columns.Contains("TextField") ? item.Field<string>("TextField") : null),
                        MAIN_DESC = (item.Table.Columns.Contains("MAIN_DESC") ? item.Field<string>("MAIN_DESC") : null),
                        SUB_CD = (item.Table.Columns.Contains("SUB_CD") ? item.Field<string>("SUB_CD") : null),
                        MAIN_CD = (item.Table.Columns.Contains("MAIN_CD") ? item.Field<string>("MAIN_CD") : null),
                        SUB_DESC = (item.Table.Columns.Contains("SUB_DESC") ? item.Field<string>("SUB_DESC") : null),
                        CODE_VAL1 = (item.Table.Columns.Contains("CODE_VAL1") ? item.Field<string>("CODE_VAL1") : null),
                        CODE_VAL2 = (item.Table.Columns.Contains("CODE_VAL2") ? item.Field<string>("CODE_VAL2") : null),
                        REMARK = (item.Table.Columns.Contains("REMARK") ? item.Field<string>("REMARK") : null),
                        ORDER_SEQ = (item.Table.Columns.Contains("ORDER_SEQ") ? item.Field<int?>("ORDER_SEQ") : null),
                        IS_VALID = (item.Table.Columns.Contains("MAIN_DESC") ? item.Field<BooleanProperty>("MAIN_DESC") : BooleanProperty.None),
                        USER_UPD = (item.Table.Columns.Contains("USER_UPD") ? item.Field<BooleanProperty>("USER_UPD") : BooleanProperty.None)
                    }).ToList();
        }
        catch
        {
            throw;
        }
    }

}