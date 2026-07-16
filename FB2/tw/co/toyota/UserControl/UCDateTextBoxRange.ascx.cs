using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_UCDateTextBoxRange : System.Web.UI.UserControl
{
    private ClientIDMode _ControlClientIDMode;
    private bool _StartDateRequire;
    private bool _EndDateRequire;
    private string _StartDateRequireMsg;
    private string _EndDateRequireMsg;
    private string _ValidationGroup;
    private string _mask;

    public string StartDateText
    {
        get { return txt_Year_DT_S.Text; }
        set { txt_Year_DT_S.Text = value; }
    }
    public ClientIDMode ControlClientIDMode
    {
        get
        {
            return _ControlClientIDMode;
        }
        set
        {
            _ControlClientIDMode = value;
            txt_Year_DT_E.ClientIDMode = _ControlClientIDMode;
            txt_Year_DT_S.ClientIDMode = _ControlClientIDMode;
        }
    }
    public string EndDateText
    {
        get { return txt_Year_DT_E.Text; }
        set { txt_Year_DT_E.Text = value; }
    }
    public string ValidatorStartDateErrMesg
    {
        get { return Validator_Year_DT_S.ErrorMessage; }
        set { Validator_Year_DT_S.ErrorMessage = value; }
    }
    public string StartDateValidationExpression
    {
        get { return Validator_Year_DT_S.ValidationExpression; }
        set { Validator_Year_DT_S.ValidationExpression = value; }
    }
    public string ValidatorEndDateErrMesg
    {
        get { return Validator_Year_DT_E.ErrorMessage; }
        set { Validator_Year_DT_E.ErrorMessage = value; }
    }
    public string EndDateValidationExpression
    {
        get { return Validator_Year_DT_E.ValidationExpression; }
        set { Validator_Year_DT_E.ValidationExpression = value; }
    }
    public string CompareValidatorErrMesg
    {
        get { return CompareValidator_Year.ErrorMessage; }
        set { CompareValidator_Year.ErrorMessage = value; }
    }
    public ValidationCompareOperator CompareValidatorOperator
    {
        get { return CompareValidator_Year.Operator; }
        set { CompareValidator_Year.Operator = value; }
    }
    public bool StartDateRequire
    {
        get { return _StartDateRequire; }
        set { _StartDateRequire = value; }
    }
    public string StartDateRequireMsg
    {
        get { return _StartDateRequireMsg; }
        set { _StartDateRequireMsg = value; }
    }
    public int StartDateMaxLength
    {
        get { return txt_Year_DT_S.MaxLength; }
        set { txt_Year_DT_S.MaxLength = value; }
    }
    public bool EndDateRequire
    {
        get { return _EndDateRequire; }
        set { _EndDateRequire = value; }
    }
    public string EndDateRequireMsg
    {
        get { return _EndDateRequireMsg; }
        set { _EndDateRequireMsg = value; }
    }
    public int EndDateMaxLength
    {
        get { return txt_Year_DT_E.MaxLength; }
        set { txt_Year_DT_E.MaxLength = value; }
    }
    public string StartDateCssClass
    {
        get { return this.txt_Year_DT_S.CssClass; }
        set { this.txt_Year_DT_S.CssClass = this.txt_Year_DT_S.CssClass + " " + value; }
    }

    public string EndDateCssClass
    {
        get { return this.txt_Year_DT_E.CssClass; }
        set { this.txt_Year_DT_E.CssClass = this.txt_Year_DT_E.CssClass + " " + value; }

    }
    public string ValidationGroup
    {
        get { return _ValidationGroup; }
        set
        {
            _ValidationGroup = value;
            Validator_Year_DT_S.ValidationGroup = _ValidationGroup;
            Validator_Year_DT_E.ValidationGroup = _ValidationGroup;
            CompareValidator_Year.ValidationGroup = _ValidationGroup;
        }
    }
    public string MaskChange
    {
        get { return _mask; }
        set { _mask = value; }
    }
    private void Page_Load(object sender, EventArgs e)
    {
        string srcPath = ResolveClientUrl("~/Scripts/Basic.js");
        Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "UCDateTimeRange_Basic", srcPath);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "UCDateTimeRange_Basic", "DateTimeMask('" + _mask + "');", true);
        
        txt_Year_DT_S.Attributes.Add("onChange", "YearStartChange();");
        txt_Year_DT_E.Attributes.Add("onChange", "YearEndChange();");
        if (this.StartDateRequire)
        {
            RequiredFieldValidator StartDateRequireField = new RequiredFieldValidator();
            StartDateRequireField.ID = "StartDateRequireField";
            StartDateRequireField.ClientIDMode = this.ControlClientIDMode;
            StartDateRequireField.ValidationGroup = this.ValidationGroup;
            StartDateRequireField.ErrorMessage = this.StartDateRequireMsg;
            StartDateRequireField.ControlToValidate = "txt_Year_DT_S";
            StartDateRequireField.ForeColor = System.Drawing.Color.Red;
            StartDateRequireField.Display = ValidatorDisplay.None;
            this.Controls.Add(StartDateRequireField);
        }
        if (this.EndDateRequire)
        {
            RequiredFieldValidator EndDateRequireField = new RequiredFieldValidator();
            EndDateRequireField.ID = "EndDateRequireField";
            EndDateRequireField.ClientIDMode = this.ControlClientIDMode;
            EndDateRequireField.ValidationGroup = this.ValidationGroup;
            EndDateRequireField.ErrorMessage = this.StartDateRequireMsg;
            EndDateRequireField.ControlToValidate = "txt_Year_DT_E";
            EndDateRequireField.ForeColor = System.Drawing.Color.Red;
            EndDateRequireField.Display = ValidatorDisplay.None;
            this.Controls.Add(EndDateRequireField);
        }

    }
    protected void txt_Year_DT_S_TextChanged(object sender, EventArgs e)
    {

    }
}