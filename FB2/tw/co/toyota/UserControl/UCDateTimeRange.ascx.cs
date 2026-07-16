using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_UCDateTimeRange : System.Web.UI.UserControl
{
    private ClientIDMode _ControlClientIDMode;
    private bool _StartDateRequire;
    private bool _EndDateRequire;
    private string _StartDateRequireMsg;
    private string _EndDateRequireMsg;
    private string _ValidationGroup;
    private string _ValidatorStartDateErrMesg;
    private string _StartDateValidationExpression;
    private string _ValidatorEndDateErrMesg;
    private string _EndDateValidationExpression;
    private string _CompareValidatorErrMesg;
    private ValidationCompareOperator _CompareValidatorOperator;
    private bool _StartDateEnabled = true;
    private bool _EndDateEnabled = true;
    private string _ClientValidationFunctionS;
    private string _ClientValidationFunctionE;

    public string ClientValidationFunctionS
    {
        get { return _ClientValidationFunctionS; }
        set { _ClientValidationFunctionS = value; }
    }
    public string ClientValidationFunctionE
    {
        get { return _ClientValidationFunctionE; }
        set { _ClientValidationFunctionE = value; }
    }
    public TextBox StartDataTextBox
    {
        get { return this.txt_LEAVE_DT_S; }
    }

    public TextBox EndDataTextBox
    {
        get { return this.txt_LEAVE_DT_E; }
    }

    public bool StartDateEnabled
    {
        get { return _StartDateEnabled; }
        set { _StartDateEnabled = value; }
    }

    public bool EndDateEnabled
    {
        get { return _EndDateEnabled; }
        set { _EndDateEnabled = value; }
    }

    public string StartDateCssClass
    {
        get { return this.txt_LEAVE_DT_S.CssClass; }
        set { this.txt_LEAVE_DT_S.CssClass = this.txt_LEAVE_DT_S.CssClass + " " + value; }
    }

    public string EndDateCssClass
    {
        get { return this.txt_LEAVE_DT_E.CssClass; }
        set { this.txt_LEAVE_DT_E.CssClass = this.txt_LEAVE_DT_E.CssClass + " " + value; }

    }

    public string StartDateText
    {
        get { return txt_LEAVE_DT_S.Text; }
        set { txt_LEAVE_DT_S.Text = value; }
    }
    public int StartDateMaxLength
    {
        get { return txt_LEAVE_DT_S.MaxLength; }
        set { txt_LEAVE_DT_S.MaxLength = value; }
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
            txt_LEAVE_DT_E.ClientIDMode = _ControlClientIDMode;
            txt_LEAVE_DT_S.ClientIDMode = _ControlClientIDMode;
        }
    }
    public string EndDateText
    {
        get { return txt_LEAVE_DT_E.Text; }
        set { txt_LEAVE_DT_E.Text = value; }
    }
    public int EndDateMaxLength
    {
        get { return txt_LEAVE_DT_S.MaxLength; }
        set { txt_LEAVE_DT_S.MaxLength = value; }
    }
    public string ValidatorStartDateErrMesg
    {
        get { return _ValidatorStartDateErrMesg; }
        set { _ValidatorStartDateErrMesg = value; }
    }
    public string StartDateValidationExpression
    {
        get { return _StartDateValidationExpression; }
        set { _StartDateValidationExpression = value; }
    }
    public string ValidatorEndDateErrMesg
    {
        get { return _ValidatorEndDateErrMesg; }
        set { _ValidatorEndDateErrMesg = value; }
    }
    public string EndDateValidationExpression
    {
        get { return _EndDateValidationExpression; }
        set { _EndDateValidationExpression = value; }
    }
    public string CompareValidatorErrMesg
    {
        get { return _CompareValidatorErrMesg; }
        set { _CompareValidatorErrMesg = value; }
    }
    public ValidationCompareOperator CompareValidatorOperator
    {
        get { return _CompareValidatorOperator; }
        set { _CompareValidatorOperator = value; }
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
    public string ValidationGroup
    {
        get { return _ValidationGroup; }
        set { _ValidationGroup = value; }
    }
    private void Page_Load(object sender, EventArgs e)
    {
        //
        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "dateFormat" + this.txt_LEAVE_DT_S.ClientID, " $('.date').datepicker({ dateFormat: 'yy/mm/dd' });", true);
        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Mask" + this.txt_LEAVE_DT_S.ClientID, "$('#" + this.txt_LEAVE_DT_S.ClientID + "').mask('9999/99/99');", true);
        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "Mask" + this.txt_LEAVE_DT_E.ClientID, "$('#" + this.txt_LEAVE_DT_E.ClientID + "').mask('9999/99/99');", true);

        if (this.CompareValidatorOperator != null)
        {
            //CompareValidator1.Enabled = true;
            CompareValidator1.Operator = this.CompareValidatorOperator;
        }
        if (ClientValidationFunctionS != "")
            CusV_txt_LEAVE_DT_S.ControlToValidate = "txt_LEAVE_DT_S";
       
        if (ClientValidationFunctionE != "")
            CusV_txt_LEAVE_DT_E.ControlToValidate = "txt_LEAVE_DT_E"; 
        
        if (string.IsNullOrEmpty(this.ValidatorStartDateErrMesg) == false)
        {
            if (ClientValidationFunctionS != "")
            {
                CusV_txt_LEAVE_DT_S.Enabled = true;
                CusV_txt_LEAVE_DT_S.ErrorMessage = this.ValidatorStartDateErrMesg;
                CusV_txt_LEAVE_DT_S.ClientValidationFunction = ClientValidationFunctionS;
            }
            else
            {
                Validator_DT_S.Enabled = true;
                Validator_DT_S.ErrorMessage = this.ValidatorStartDateErrMesg;
            }

        }


        if (string.IsNullOrEmpty(this.StartDateValidationExpression) == false)
        {
            if (ClientValidationFunctionS == "")
            {
                Validator_DT_S.Enabled = true;
                Validator_DT_S.ValidationExpression = this.StartDateValidationExpression;
            }
        }


        if (string.IsNullOrEmpty(this.ValidatorEndDateErrMesg) == false)
        {
            if (ClientValidationFunctionE != "")
            {
                CusV_txt_LEAVE_DT_E.Enabled = true;
                CusV_txt_LEAVE_DT_E.ErrorMessage = this.ValidatorEndDateErrMesg;
                CusV_txt_LEAVE_DT_E.ClientValidationFunction = ClientValidationFunctionE;
            }
            else
            {
                Validator_DT_E.Enabled = true;
                Validator_DT_E.ErrorMessage = this.ValidatorEndDateErrMesg;
            }
        }

        if (string.IsNullOrEmpty(this.EndDateValidationExpression) == false)
        {
            if (ClientValidationFunctionE != "")
            {
                Validator_DT_E.Enabled = true;
                Validator_DT_E.ValidationExpression = this.EndDateValidationExpression;
            }
        }

        if (string.IsNullOrEmpty(this.CompareValidatorErrMesg) == false)
        {
            CompareValidator1.Enabled = true;
            CompareValidator1.ErrorMessage = this.CompareValidatorErrMesg;
        }

        if (this.CompareValidatorOperator == null)
        {
            CompareValidator1.Enabled = true;
            CompareValidator1.Operator = this.CompareValidatorOperator;
        }


        if (string.IsNullOrEmpty(this.ValidationGroup) == false)
        {
            if (ClientValidationFunctionS != "")
            {
                CusV_txt_LEAVE_DT_S.Enabled = true;
                CusV_txt_LEAVE_DT_S.ValidationGroup = this.ValidationGroup;
            }
            else
            {
                Validator_DT_S.Enabled = true;
                Validator_DT_S.ValidationGroup = this.ValidationGroup;
            }
        }

        if (string.IsNullOrEmpty(this.ValidationGroup) == false)
        {
            if (ClientValidationFunctionE != "")
            {
                CusV_txt_LEAVE_DT_E.Enabled = true;
                CusV_txt_LEAVE_DT_E.ValidationGroup = this.ValidationGroup;
            }
            else
            {
                Validator_DT_E.Enabled = true;
                Validator_DT_E.ValidationGroup = this.ValidationGroup;
            }
        }

        if (string.IsNullOrEmpty(this.ValidationGroup) == false)
        {
            CompareValidator1.Enabled = true;
            CompareValidator1.ValidationGroup = this.ValidationGroup;
        }

        //string srcPath = ResolveClientUrl("~/Scripts/Basic.js");
        //Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "UCDateTimeRange_Basic", srcPath);
        if (this.StartDateRequire)
        {
            RequiredFieldValidator StartDateRequireField = new RequiredFieldValidator();
            StartDateRequireField.ID = "StartDateRequireField";
            StartDateRequireField.ClientIDMode = this.ControlClientIDMode;
            StartDateRequireField.ValidationGroup = this.ValidationGroup;
            StartDateRequireField.ErrorMessage = this.StartDateRequireMsg;
            StartDateRequireField.ControlToValidate = "txt_LEAVE_DT_S";
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
            EndDateRequireField.ErrorMessage = this.EndDateRequireMsg;
            EndDateRequireField.ControlToValidate = "txt_LEAVE_DT_E";
            EndDateRequireField.ForeColor = System.Drawing.Color.Red;
            EndDateRequireField.Display = ValidatorDisplay.None;
            this.Controls.Add(EndDateRequireField);
        }
        this.txt_LEAVE_DT_E.Enabled = this.EndDateEnabled;
        this.txt_LEAVE_DT_S.Enabled = this.StartDateEnabled;
    }
}