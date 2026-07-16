using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
/// <summary>
/// CFB2990100BO 的摘要描述
/// </summary>
public class CFB2990300BO
{
    public CFB2990300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string TBNAME { get; set; }
    public string TB_DESC { get; set; }
    public string FUNC_ID { get; set; }
    public string FUNC_NAME { get; set; }
    public string CATEGORY_ITEM { get; set; }
    public string PK_COLUMN { get; set; }
    public string EDIT_INFOR { get; set; }

}