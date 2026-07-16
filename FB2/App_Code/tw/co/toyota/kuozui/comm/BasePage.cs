using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Threading;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Specialized; 
/// <summary>
/// BasePage 的摘要描述
/// </summary>
public class BasePage : System.Web.UI.Page
{
    public ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    string UID = "";
    string FUNC_NAME = "";
    string FUNC_ID = "";
    public string parentFuncId = "";
    public bool IsInSearchingMode = false;

    protected override void OnInit(EventArgs e)
    {
 
        string pageFuncId = Path.GetFileName(Request.PhysicalPath).Substring(1, 8);

        FUNC_DATA fundata = new FUNC_DATA();

        //檢查系統是否停用及使用權限
        //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('系統停用');", true);
        UID = Request.QueryString["UID"] == null ? "" : Request.QueryString["UID"].ToString();
        FUNC_NAME = Request.QueryString["FUNC_NAME"] == null ? "" : HttpUtility.UrlDecode(Request.QueryString["FUNC_NAME"].ToString(), Encoding.ASCII);
        FUNC_ID = Request.QueryString["FUNC_ID"] == null ? pageFuncId : Request.QueryString["FUNC_ID"].ToString();
        parentFuncId = Request.QueryString["parentFuncId"] == null ? "" : Request.QueryString["parentFuncId"].ToString();
        if (FUNC_ID == "FB2DC051")
            FUNC_ID = "FB2DC050";

        //UID = "FB2DI030-2C736330-619B-4EAB-AAC7-CFB6609033FB";
        //FUNC_NAME = "參數明細檔維護";
        //FUNC_ID = "FB200035";

        if (parentFuncId != "")
        {
            fundata.FUNC_ID = parentFuncId;
            pageFuncId = parentFuncId;
        }
        else
            fundata.FUNC_ID = FUNC_ID;
        fundata.UID = UID;
        fundata.FUNC_NAME = FUNC_NAME;

        if (SessionHandle.Current.FUNC_DATAs == null)
        {
            SessionHandle.Current.FUNC_DATAs = new List<FUNC_DATA>();
        }

        var result = SessionHandle.Current.FUNC_DATAs.Where(x => x.FUNC_ID == pageFuncId);
        if (result.Count() == 0)
        {
            SessionHandle.Current.FUNC_DATAs.Add(fundata);

        }
        else
        {

            if (UID != "")
            {
                SessionHandle.Current.FUNC_DATAs.Remove(result.First());
                SessionHandle.Current.FUNC_DATAs.Add(fundata);

            }
            else
                fundata = result.First();
        }
        SessionHandle.Current.FUNC_ID = fundata.FUNC_ID;
        SessionHandle.Current.FUNC_NAME = fundata.FUNC_NAME;



        ACESLib.ACES aces = new ACESLib.ACES();
        //Response.Write(fundata.UID);
        String licit = aces.CheckLicit(fundata.UID);
        //string licit = "1";
        //-1無權限
        if (licit == "-1")
        {
            logger.Info("無權限LOG-UID:" + UID + "");
            logger.Info("無權限LOG-FUNC_ID:" + FUNC_ID + "");
            logger.Info("無權限LOG-parentFuncId:" + parentFuncId + "");
            if (UID == "")
                Response.Redirect("~/TimeOut.aspx");
            else
                Response.Redirect("~/ForbiddenPage.aspx?uid=" + fundata.UID);
            
        }
        if (Application["SystemStatus"] != null)
        {
            if (Application["SystemStatus"].ToString() != "N")
                Response.Redirect("~/suspendpage.aspx");
        }
        //設定Session       
        //檢查ACES session
        ACESLib.UserBean userBean = aces.GetUser();
        if (userBean.WorkID == "")
        {
            logger.Info("無權限LOG-UID:" + UID + "");
            logger.Info("無權限LOG-FUNC_ID:" + FUNC_ID + "");
            logger.Info("無權限LOG-parentFuncId:" + parentFuncId + "");
            if (UID == "")
                Response.Redirect("~/TimeOut.aspx");
            else
                Response.Redirect("~/ForbiddenPage.aspx?uid=" + fundata.UID);
            
        }
        log4net.ThreadContext.Properties["WorkID"] = userBean.WorkID;
        log4net.ThreadContext.Properties["FunID"] = FUNC_ID;
        if (SessionHandle.Current.emp_id == null || SessionHandle.Current.emp_id != userBean.WorkID)
            getUserSession(userBean.WorkID);

        //20150714 依不同的功能給予角色權限設定
        utilities.setAuthData();

        //資料角色
        String dbRole = aces.GetRoles();
        IList<string> role = dbRole.Split(',');
        SessionHandle.Current.db_role = role;
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "setParentFuncId", "parentFuncID='" + fundata.FUNC_ID + "';", true);

        logger.Info("頁面request" + fundata.UID);
    }
    //設定user權限
    private void getUserSession(string emp_id)
    {
        try
        {
            SessionHandle handle = new SessionHandle();
            handle.setUserSession(emp_id);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('" + ex.Message + "');", true);
        }
    }

    //Gridview objectdatasource 換頁使用
    protected void ods1_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        ViewState["TotalCount"] = e.ReturnValue;
    }
    protected void obs1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        //if (!IsInSearchingMode)
        //{
        //    e.Cancel = true;
        //}
        //e.InputParameters
        bool Queryble = ViewState["Queryble"] == null ? true : Convert.ToBoolean(ViewState["Queryble"]);
        if (Queryble || (OrderedDictionary)Session[SessionHandle.Current.FUNC_ID+"QueryParams"]==null)
        {
            OrderedDictionary OldQueryParams = new OrderedDictionary();
            foreach (DictionaryEntry Param in e.InputParameters)
                OldQueryParams.Add(Param.Key, Param.Value);
            Session[SessionHandle.Current.FUNC_ID + "QueryParams"] = OldQueryParams;
        }
        else
        {
            OrderedDictionary QueryParams = (OrderedDictionary)Session[SessionHandle.Current.FUNC_ID + "QueryParams"];
            e.InputParameters.Clear();
            foreach (DictionaryEntry Param in QueryParams)
                e.InputParameters.Add(Param.Key, Param.Value);
        }
        if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
            e.Arguments.SortExpression = ViewState["SortExpression"] + " " + ViewState["SortDirection"];
    }
    //設定排序
    protected string getSortDirection(string column, string sort = "ASC")
    {

        // By default, set the sort direction to ascending.
        string sortDirection = sort;

        // Retrieve the last column that was sorted.
        string sortExpression = ViewState["SortExpression"] as string;

        if (sortExpression != null)
        {
            // Check if the same column is being sorted.
            // Otherwise, the default value can be returned.
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }

        // Save new values in ViewState.
        ViewState["SortDirection"] = sortDirection;
        ViewState["SortExpression"] = column;

        return sortDirection;
    }

    protected string getSortDirection2(string column, string sort = "ASC")
    {
        // By default, set the sort direction to ascending.
        string sortDirection = sort;

        // Retrieve the last column that was sorted.
        string sortExpression = ViewState["SortExpression2"] as string;

        if (sortExpression != null)
        {
            // Check if the same column is being sorted.
            // Otherwise, the default value can be returned.
            if (sortExpression == column)
            {
                string lastDirection = ViewState["SortDirection2"] as string;
                if ((lastDirection != null) && (lastDirection == "ASC"))
                {
                    sortDirection = "DESC";
                }
            }
        }

        // Save new values in ViewState.
        ViewState["SortDirection2"] = sortDirection;
        ViewState["SortExpression2"] = column;

        return sortDirection;
    }
    //英文語系切換
    protected void btn_en_Click(object sender, EventArgs e)
    {
        Session["CurrentUI"] = "en-US";
        Response.Redirect(Request.Url.OriginalString);
    }
    //中文語系切換
    protected void btn_tw_Click(object sender, EventArgs e)
    {
        Session["CurrentUI"] = "zh-TW";
        Response.Redirect(Request.Url.OriginalString);
    }
    //覆寫語系事件
    protected override void InitializeCulture()
    {
        if (Session["CurrentUI"] != null)
        {
            String selectedLanguage = (string)Session["CurrentUI"];
            UICulture = selectedLanguage;
            Culture = selectedLanguage;

            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(selectedLanguage);
            Thread.CurrentThread.CurrentUICulture = new
                CultureInfo(selectedLanguage);
        }

        base.InitializeCulture();
    }
    //顯示共用訊息
    protected void showMessage(string action, string otherMessage = "")
    {
        if (Session["CurrentUI"] != null)
            action = action + "-" + Session["CurrentUI"].ToString();
        else
            action = action + "-" + Thread.CurrentThread.CurrentCulture.IetfLanguageTag;
        if (Application["Message"] == null)
        {
            XElement xml = XElement.Load(Server.MapPath("~/Message.xml"));
            Dictionary<string, string> tmpMessage = new Dictionary<string, string>();
            foreach (XElement el in xml.Elements())
                tmpMessage.Add(el.Name.LocalName, el.Value);
            Application["Message"] = tmpMessage;
        }
        Dictionary<string, string> message = (Dictionary<string, string>)Application["Message"];
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "alert", "alert('" + message[action].Replace("\r\n", "").Replace("'", "\"") + ";" + otherMessage.Replace("\r\n", "").Replace("'", "\"") + "');", true);
    }

    protected string GetMessage(string action, string otherMessage = "")
    {
        if (Session["CurrentUI"] != null)
            action = action + "-" + Session["CurrentUI"].ToString();
        else
            action = action + "-" + Thread.CurrentThread.CurrentCulture.IetfLanguageTag;
        Dictionary<string, string> message = (Dictionary<string, string>)Application["Message"];
        return message[action] + ";" + otherMessage;
    }

    /// <summary>
    /// 在共用的 Session 中加入 key 與 value
    /// 若 key 不存在, 會新增一筆 key 與 value
    /// 若 key 已經存在, value 值會被覆蓋
    /// 註: 共用的 Session (Session["hashTable"]), 解決每個 Session 會佔用記憶體固定空間問題, 減少損耗系統資源    
    /// </summary>
    /// <param name="key">key(命名方式: FUNC_ID_XXXX)</param>
    /// <param name="value">object</param>
    public void hashtable_set(string key, object value)
    {
        if (Session["hashTable"] == null)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add(key, value);
            Session["hashTable"] = hashtable;
        }
        else
        {
            try
            {
                if (((Hashtable)Session["hashTable"]).ContainsKey(key))
                {
                    //key 值存在
                    ((Hashtable)Session["hashTable"])[key] = value;
                }
                else
                {
                    //key 值不存在
                    ((Hashtable)Session["hashTable"]).Add(key, value);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                showMessage("errMessage", ex.Message);
            }
        }
    }
    /// <summary>
    /// 在共用的 Session 中, 依 key 取得 value
    /// 若 key 不存在, 會回傳 null
    /// 若 key 存在, 會回傳 value 值
    /// 註: 共用的 Session (Session["hashTable"]), 解決每個 Session 會佔用記憶體固定空間問題, 減少損耗系統資源
    /// </summary>
    /// <param name="key">key(命名方式: FUNC_ID_XXXX)</param>
    /// <returns>value</returns>
    public object hashtable_get(string key)
    {
        object rtnval = null;
        if (Session["hashTable"] != null && ((Hashtable)Session["hashTable"]).ContainsKey(key))
        {
            rtnval = ((Hashtable)Session["hashTable"])[key];
        }
        return rtnval;
    }    
}