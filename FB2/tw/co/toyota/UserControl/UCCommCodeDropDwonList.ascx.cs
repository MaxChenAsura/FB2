using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kuozui;
public partial class UserControl_UCCommCodeDropDwonList : System.Web.UI.UserControl
{
    public delegate void SelectIndexChangedEventHandler(object sender, EventArgs e);
    public event EventHandler SelectIndexChanged;

    #region field

    private string _SYS_CDs;
    private string _MAIN_CDs;
    private string _SUB_CDs;
    private string _CODE_VAL1s;
    private string _CODE_VAL2s;
    private BooleanProperty _IS_VALID;
    private BooleanProperty _USER_UPD;
    private CommCode_ORDER_SEQ _OrderSeq;
    private String _FirstItem;
    private string _DataValueFormatString;
    private string _DataTextFormatString;
    private string _DataTextField;
    private string _DataValueField;
    private string _ValidationGroup;
    private string _ValidatorErrMesg;
    private bool _Require;
    private bool _Enabled = true;
    private string _SelectValue = string.Empty;

    #endregion

    #region properties

    public bool Enabled
    {
        get { return _Enabled; }
        set { _Enabled = value; }
    }

    public bool Require
    {
        get { return _Require; }
        set { _Require = value; }
    }

    public string ValidatorErrMesg
    {
        get { return _ValidatorErrMesg; }
        set { _ValidatorErrMesg = value; }
    }

    public string CssClass
    {
        get { return ddlCommCode.CssClass; }
        set { ddlCommCode.CssClass = value; }
    }

    public Unit Width
    {
        get { return this.ddlCommCode.Width; }
        set { this.ddlCommCode.Width = value; }
    }

    public Unit Heith
    {
        get { return this.ddlCommCode.Height; }
        set { this.ddlCommCode.Height = value; }
    }
    public string DataValueFormatString
    {
        get { return _DataValueFormatString; }
        set { _DataValueFormatString = value; }
    }

    public bool AutoPostBack
    {
        get { return this.ddlCommCode.AutoPostBack; }
        set { this.ddlCommCode.AutoPostBack = value; }
    }

    public ClientIDMode ClientIDMode
    {
        get { return ddlCommCode.ClientIDMode; }
        set { ddlCommCode.ClientIDMode = value; }
    }

    public string SYS_CDs
    {
        get { return _SYS_CDs; }
        set { _SYS_CDs = value; }
    }

    public string MAIN_CDs
    {
        get { return _MAIN_CDs; }
        set { _MAIN_CDs = value; }
    }

    public string SUB_CDs
    {
        get { return _SUB_CDs; }
        set { _SUB_CDs = value; }
    }

    public string CODE_VAL1s
    {
        get { return _CODE_VAL1s; }
        set { _CODE_VAL1s = value; }
    }

    public string CODE_VAL2s
    {
        get { return _CODE_VAL2s; }
        set { _CODE_VAL2s = value; }
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

    public CommCode_ORDER_SEQ OrderSeq
    {
        get { return _OrderSeq; }
        set { _OrderSeq = value; }
    }

    public string DataTextField
    {
        get { return _DataTextField; }
        set { _DataTextField = value; }
    }

    public object DataSource
    {
        get { return ddlCommCode.DataSource; }
        set { ddlCommCode.DataSource = value; }
    }

    public string DataSourceID
    {
        get { return ddlCommCode.DataSourceID; }
        set { ddlCommCode.DataSourceID = value; }
    }

    public string DataTextFormatString
    {
        get { return _DataTextFormatString; }
        set { _DataTextFormatString = value; }
    }

    public string DataValueField
    {
        get { return _DataValueField; }
        set { _DataValueField = value; }
    }

    public int SelectedIndex
    {
        get { return ddlCommCode.SelectedIndex; }
        set { ddlCommCode.SelectedIndex = value; }
    }

    public ListItem SelectedItem
    {
        get { return ddlCommCode.SelectedItem; }
    }

    public string SelectedValue
    {
        get { return string.IsNullOrEmpty(_SelectValue) ? ddlCommCode.SelectedValue : _SelectValue; }
        set
        {
            _SelectValue = value;
            ddlCommCode.SelectedValue = _SelectValue;
        }
    }

    public string FirstItem
    {
        get { return _FirstItem; }
        set { _FirstItem = value; }
    }

    public string ValidationGroup
    {
        get { return _ValidationGroup; }
        set { _ValidationGroup = value; }
    }

    #endregion

    #region Event

    protected void Page_Load(object sender, EventArgs e)
    {
        ddlCommCode.SelectedIndexChanged += this.SelectIndexChanged;

        if (this.ddlCommCode.DataSource == null && string.IsNullOrEmpty(this.ddlCommCode.DataSourceID))
        {
            this.ddlCommCode.DataTextField = "TextField";
            this.ddlCommCode.DataValueField = "ValueField";
        }
        else
        {
            this.ddlCommCode.DataTextField = this.DataTextField;
            this.ddlCommCode.DataValueField = this.DataValueField;
        }

        if (this.IsPostBack == false)
            this.CommCodeDataBind();

        if (string.IsNullOrEmpty(this.ValidatorErrMesg) == false)
        {
            Validator_ddlCommCode.Enabled = true;
            Validator_ddlCommCode.ErrorMessage = this.ValidatorErrMesg;
        }

        if (string.IsNullOrEmpty(this.ValidationGroup) == false)
        {
            Validator_ddlCommCode.Enabled = true;
            Validator_ddlCommCode.ValidationGroup = this.ValidationGroup;
        }
        if (this.Require)
        {

            //RequiredFieldValidator ddlCommCodeRequireField = new RequiredFieldValidator();
            //Validator_ddlCommCode.ID = "ddlCommCodeRequireField";
            Validator_ddlCommCode.ClientIDMode = this.ClientIDMode;
            Validator_ddlCommCode.ValidationGroup = this.ValidationGroup;
            Validator_ddlCommCode.ErrorMessage = this.ValidatorErrMesg;
            Validator_ddlCommCode.ControlToValidate = ddlCommCode.ClientID;
            Validator_ddlCommCode.InitialValue = this.FirstItem;
            Validator_ddlCommCode.ForeColor = System.Drawing.Color.Red;
            Validator_ddlCommCode.Display = ValidatorDisplay.None;
            //this.Controls.Add(ddlCommCodeRequireField);
        }
        this.ddlCommCode.Enabled = this.Enabled;
    }

    protected virtual void OnSelectIndexChange(object sender, EventArgs e)
    {
        EventHandler handler = this.SelectIndexChanged;
        if (handler != null)
        {
            handler(this, e);
        }
    }

    #endregion

    #region Public Methods

    public void DataBind()
    {
        ddlCommCode.Items.Clear();
        if (this.FirstItem != null)
            ddlCommCode.Items.Add(new ListItem(this.FirstItem, this.FirstItem));
        ddlCommCode.AppendDataBoundItems = true;
        ddlCommCode.DataBind();
    }

    public void CommCodeDataBind()
    {
        UCCommCodeDropDwonListDL UCCDL = new UCCommCodeDropDwonListDL();
        UCCommCodeDropDwonListDAO dao = new UCCommCodeDropDwonListDAO();
        dao.WhereCODE_VAL1s = this.CODE_VAL1s;
        dao.WhereCODE_VAL2s = this.CODE_VAL2s;
        dao.WhereMAIN_CDs = this.MAIN_CDs;
        dao.WhereSUB_CDs = this.SUB_CDs;
        dao.WhereSYS_CDs = this.SYS_CDs;
        dao.WhereUSER_UPD = this.USER_UPD;
        dao.WhereIS_VALID = this.IS_VALID;
        ddlCommCode.Items.Clear();
        if (this.FirstItem != null)
            ddlCommCode.Items.Add(new ListItem(this.FirstItem, this.FirstItem));
        ddlCommCode.AppendDataBoundItems = true;

        if (ddlCommCode.DataSource == null && ddlCommCode.DataSourceObject == null)
            ddlCommCode.DataSource = UCCDL.getData(this.DataTextField.Trim(','), this.DataTextFormatString, this.DataValueField.Trim(','), this.DataValueFormatString, dao);
        ddlCommCode.DataBind();
        if (!string.IsNullOrEmpty(_SelectValue))
            ddlCommCode.SelectedValue = _SelectValue;

    }

    #endregion

    #region Public Functions
    #endregion

    #region Private Methods
    #endregion

    #region Public Functions
    #endregion

}