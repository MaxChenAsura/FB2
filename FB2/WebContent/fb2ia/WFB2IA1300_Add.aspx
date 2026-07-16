<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA1300_Add.aspx.cs" Inherits="WebContent_WFB2IA1300_Add" Culture="auto" UICulture="auto" %>

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
            $("#txt_CLASSQTY").mask('99');
            $.unblockUI();

            //寫在這，按查詢才不會消失
            $('#txt_COMPANY_SNAME').attr("readonly", true);
            //公司代號取得公司名稱的ajax
            $("#txt_COMPANY_CD").change(function () {
                if ($("#txt_COMPANY_CD").val().length == 1) {
                    $.ajax({
                        url: "../comm/WFB2CompanyData.ashx",
                        data: {
                            COMPANY_CD: $('#txt_COMPANY_CD').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_COMPANY_SNAME').val("");
                                $("#txt_HEALTH_ORG_ID").val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_COMPANY_SNAME').val(JData.COMPANY_SNAME);
                                $("#txt_HEALTH_ORG_ID").val(JData.HEALTH_ORG_ID);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_COMPANY_SNAME').val("");
                    $("#txt_HEALTH_ORG_ID").val("");
                }
            });
        }

        function getCompany() {
            OpenSearch('Company_Search.aspx', 'txt_COMPANY_CD', 'txt_COMPANY_SNAME', 'COMPANY_CD=' + $("#txt_COMPANY_CD").val(),'Y');
            //if (json != undefined)
            //    $("#txt_HEALTH_ORG_ID").val(json.Val1);
        }
        function Company_Search(id, name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#"+id).val(obj.CD);
                $("#"+name).val(obj.DESC);
                $("#txt_HEALTH_ORG_ID").val(obj.Val1);
                return obj;
            }
        }
        function ValidA() {
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;

            if (!processed)
                $.unblockUI();

            return processed;
        }

        function ValidB() {
            var processed = true;

            if (Page_ClientValidate("GroupB")) {
                BlockUI();
            }
            else
                processed = false;

            if (!processed)
                $.unblockUI();

            return processed;
        }

        function ValidC() {
            var processed = true;

            if (Page_ClientValidate("GroupC")) {
                BlockUI();
            }
            else
                processed = false;

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
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="30%" />
                                <col width="55%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--薪調月份--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_SALARY_YM2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEF_SYM" runat="server" MaxLength="6" Width="50px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                        ~ 
                                        <asp:TextBox ID="txt_DEF_EYM" runat="server" MaxLength="6" Width="50px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="reg_DEF_SYMA" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_DATA_YM_SDT%>" ControlToValidate="txt_DEF_SYM" ForeColor="Red"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        <asp:RegularExpressionValidator ID="reg_DEF_EYMA" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_DATA_YM_EDT%>" ControlToValidate="txt_DEF_EYM" ForeColor="Red"
                                            ValidationExpression="(19|20|99)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        <asp:CompareValidator ID="cv_DEF_YMA" runat="server" ForeColor="Red" ControlToCompare="txt_DEF_SYM"
                                            ControlToValidate="txt_DEF_EYM" ErrorMessage="<%$Resources:Resource,wfb2sk_greaterthan_DATA_YM%>" Type="String" Operator="GreaterThan"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                        <asp:RegularExpressionValidator ID="reg_DEF_SYMB" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_DATA_YM_SDT%>" ControlToValidate="txt_DEF_SYM" ForeColor="Red"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupB">
                                        </asp:RegularExpressionValidator>
                                        <asp:RegularExpressionValidator ID="reg_DEF_EYMB" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_DATA_YM_EDT%>" ControlToValidate="txt_DEF_EYM" ForeColor="Red"
                                            ValidationExpression="(19|20|99)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupB">
                                        </asp:RegularExpressionValidator>
                                        <asp:CompareValidator ID="cv_DEF_YMB" runat="server" ForeColor="Red" ControlToCompare="txt_DEF_SYM"
                                            ControlToValidate="txt_DEF_EYM" ErrorMessage="<%$Resources:Resource,wfb2sk_greaterthan_DATA_YM%>" Type="String" Operator="GreaterThan"
                                            Display="None" ValidationGroup="GroupB"></asp:CompareValidator>
                                        <asp:RegularExpressionValidator ID="reg_DEF_SYMC" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_DATA_YM_SDT%>" ControlToValidate="txt_DEF_SYM" ForeColor="Red"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupC">
                                        </asp:RegularExpressionValidator>
                                        <asp:RegularExpressionValidator ID="reg_DEF_EYMC" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_DATA_YM_EDT%>" ControlToValidate="txt_DEF_EYM" ForeColor="Red"
                                            ValidationExpression="(19|20|99)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupC">
                                        </asp:RegularExpressionValidator>
                                        <asp:CompareValidator ID="cv_DEF_YMC" runat="server" ForeColor="Red" ControlToCompare="txt_DEF_SYM"
                                            ControlToValidate="txt_DEF_EYM" ErrorMessage="<%$Resources:Resource,wfb2sk_greaterthan_DATA_YM%>" Type="String" Operator="GreaterThan"
                                            Display="None" ValidationGroup="GroupC"></asp:CompareValidator>
                                    </td>
                                    <td></td>
                                </tr>
                               
                                <tr>
                                    <th></th>
                                    <td></td>
                                    <td align="right" class="Body_label">
                                        <div id="init0">
                                            <aces:Btn ID="WFB2IA1300Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1300Process%>" ValidationGroup="GroupA" OnClick="WFB2IA1300Process_Click" CausesValidation="true" OnClientClick="return ValidA();" />
                                            <%--<asp:Button ID="WFB2IA1300Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1300Process%>" ValidationGroup="GroupA" OnClick="WFB2IA1300Process_Click" CausesValidation="true" OnClientClick="return ValidA();" />--%>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <hr1 />
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="30%" />
                                <col width="55%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_COMPANY_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_COMPANY_CD" runat="server" MaxLength="1" Width="64px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="req_COMPANY_CDB" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_Company_CD%>"
                                            ControlToValidate="txt_COMPANY_CD" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="req_COMPANY_CDC" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_Company_CD%>"
                                            ControlToValidate="txt_COMPANY_CD" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                        <input id="bt_COMPANY_CD_SEARCH" type="button" value="..." onclick="getCompany();" />
                                        <asp:TextBox ID="txt_COMPANY_SNAME" MaxLength="60" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <%--健保投保單位代號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_HEALTH_ORG_ID2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_HEALTH_ORG_ID" runat="server" MaxLength="15" Width="64px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <%--調降級數列印設定--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_CLASSQTY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CLASSQTY" runat="server" Style="text-align: right" MaxLength="2" Width="64px" ClientIDMode="Static" CssClass="MandatoryField "></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="req_CLASSQTY" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_CLASSQTY%>"
                                            ControlToValidate="txt_CLASSQTY" ForeColor="Red" ValidationGroup="GroupC" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rv_CLASSQTY" ControlToValidate="txt_CLASSQTY" runat="server" ValidationGroup="GroupB" ForeColor="Red" Display="None"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_lb_CLASSQTY_isError%>" MinimumValue="1" MaximumValue="99"></asp:RangeValidator>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                     <%--排序條件--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_ORDER_BY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_ORDER_BY" runat="server" ClientIDMode="Static" ></asp:DropDownList> 
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init1">
                                            <aces:Btn ID="WFB2IA1300Excel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1300Excel%>" OnClick="WFB2IA1300Excel_Click" ValidationGroup="GroupB" CausesValidation="true" OnClientClick="return ValidB();" />
                                            <aces:Btn ID="WFB2IA1300PDF" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1300PDF%>" OnClick="WFB2IA1300PDF_Click" ValidationGroup="GroupC" CausesValidation="true" OnClientClick="return ValidC();" />
                                            <%--<asp:Button ID="WFB2IA1300Excel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1300Excel%>" OnClick="WFB2IA1300Excel_Click" ValidationGroup="GroupB" CausesValidation="true" OnClientClick="return ValidB();" />
                                            <asp:Button ID="WFB2IA1300PDF" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA1300PDF%>" OnClick="WFB2IA1300PDF_Click" ValidationGroup="GroupC" CausesValidation="true" OnClientClick="return ValidC();" />--%>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td></td>
                    <td align="right" class="Body_label">
                        <div id="init2">
                            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,btn_back%>" OnClick="btn_back_Click" />
                        </div>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2IA1300Excel"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2IA1300PDF"></asp:PostBackTrigger>
        </Triggers>

    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
    <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupC" ShowSummary="false" />
</asp:Content>

