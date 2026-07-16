<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC5500_Qry.aspx.cs" Inherits="WebContent_WFB2SC5500_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $(".date").mask('9999/99');
            $.unblockUI();
        }
        function ClearAll() {
            $('#txt_SALARY_DT').val("");
        }

        function getCompany() {
            OpenSearch('Company_Search.aspx', 'txt_COMPANY_CD', 'txt_COMPANY_SNAME', 'COMPANY_CD=' + $("#txt_COMPANY_CD").val(), 'Y');
                
        }
        function Company_Search(emp_id, emp_name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#txt_COMPANY_CD").val(obj.CD);
                $("#txt_COMPANY_SNAME").val(obj.DESC);
                $("#txt_HEALTH_ORG_ID").val(json.Val1);
                return obj;
            }
        }
        //儲存前檢查
        function saveCheck(msg) {
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                //BlockUI();
            }
            else
                processed = false;

            if (processed) {
                processed = confirm("確定要進行" + msg);
                //BlockUI();
            }

            if (!processed)
                $.unblockUI();

            return processed;
        }


    </script>
    <style type="text/css">
        .txtReadOnly[readonly] {
            background: #fff;
            color: #000;
            border: 0px solid #fff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>

    <!--2SC5400_QRY開始-->
    <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

        <tr height="100%" valign="top">
            <td>
                <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                    <colgroup>
                        <col width="3%" />
                        <col width="15%" />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT2%>"></asp:Label>:
                            </th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_SALARY_DT" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                            </td>
                        </tr>
                        <asp:RequiredFieldValidator ID="req_CLASSQTY" runat="server" ErrorMessage="<%$Resources:Resource,wfb2_SALARY_DT_Required%>"
                            ControlToValidate="txt_SALARY_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="reg_DEF_YM" runat="server"
                            ErrorMessage="<%$Resources:Resource,wfb2sc_ERR_SALARY_DATE%>" ControlToValidate="txt_SALARY_DT" ForeColor="Red"
                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                        </asp:RegularExpressionValidator>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td></td>
            <td align="right" class="Body_label">
                <div id="init2">
                    <aces:Btn ID="WFB2SC5500PDF" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC5500PDF%>" OnClick="WFB2SC5500PDF_Click"  CausesValidation="true" ValidationGroup="GroupA" OnClientClick="return saveCheck(this.value);"/>

                    <%--<asp:Button ID="WFB2SC5500PDF" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC5500PDF%>" OnClick="WFB2SC5500PDF_Click" CausesValidation="true" ValidationGroup="GroupA" OnClientClick="return saveCheck(this.value);" />--%>

                    <input id="WFB2SC5500Clear" type="button" value="<%$Resources:Resource,btn_clear%>" runat="server" onclick="ClearAll();" />
                </div>
            </td>
        </tr>

    </table>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>

