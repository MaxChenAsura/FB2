<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA3200_Add.aspx.cs" Inherits="WebContent_WFB2IA3200_Add" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function ddlCheck(source, arguments) {
            if ($("#ddl_BILLS_KIND").val() == "-1")
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }

        function iniForm() {
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".date2").mask('9999/99');
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
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_COMPANY_SNAME').val(JData.COMPANY_SNAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_COMPANY_SNAME').val("");
                }
            });
        }
        function block_grant(msg) {
            if (msg =="") {
                BlockUI();
                __doPostBack('execute', '');
            } else {
                if (confirm(msg)) {
                    BlockUI();
                    __doPostBack('execute', '');
                }
                else {
                    return;
                }
            }
            
        }

    </script>
    <style type="text/css">
        .txtReadOnly[readonly] { 
                background: #fff; 
                color: #000; 
                border: 0px solid #fff ;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="12%" />
                                <col width="30%" />
                                <col width="12%" />
                                <col width="46%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                   <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2ia_COMPANY_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_COMPANY_CD" runat="server" MaxLength="1" Width="50px" ClientIDMode="Static" CssClass="MandatoryField" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_Company_CD%>"
                                            ControlToValidate="txt_COMPANY_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <input id="bt_COMPANY_CD_SEARCH1" type="button" value="..." onclick="OpenSearch('Company_Search.aspx', 'txt_COMPANY_CD', 'txt_COMPANY_SNAME', '', '');" />
                                        <asp:TextBox ID="txt_COMPANY_SNAME" MaxLength="60" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_FEES_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEF_YM" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date2"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_FEES_YM%>"
                                            ControlToValidate="txt_DEF_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="reg_DEF_YM" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_FEES_YM%>" ControlToValidate="txt_DEF_YM" ForeColor="Red"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                     <!-- >匯入帳單種類-->
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_BILLS_KIND" runat="server" Text="<%$Resources:Resource,wfb2ia_BILLS_KIND%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_BILLS_KIND" runat="server" ClientIDMode="Static" CssClass="MandatoryField" ></asp:DropDownList>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_required_BILLS_KIND%>" ClientValidationFunction="ddlCheck" ForeColor="Red"
                                            ControlToValidate="ddl_BILLS_KIND" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>

                                </tr>
                                <tr>
                                    <th></th>
                                    <td></td>
                                    <td></td>
                                    <td align="right" class="Body_label">
                                        <div id="init0">
                                            <aces:Btn ID="WFB2IA3201Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3200Process%>" ValidationGroup="GroupA" OnClick="WFB2IA3201Process_Click" CausesValidation="true" OnClientClick="CheckValid();" />
                                            <%--<asp:Button ID="WFB2IA3201Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA3200Process%>" ValidationGroup="GroupA" OnClick="WFB2IA3201Process_Click" CausesValidation="true" OnClientClick="CheckValid();" />--%>
                                            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,btn_back%>" OnClick="btn_back_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                       <hr1 />
                    </td>
                </tr>
            </table>
             <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
             <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

