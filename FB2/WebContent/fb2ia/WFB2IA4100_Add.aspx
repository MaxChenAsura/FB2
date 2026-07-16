<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA4100_Add.aspx.cs" Inherits="WebContent_WFB2IA4100_Add" Culture="auto" UICulture="auto" %>

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


    </script>
    <style type="text/css">
        .txtReadyOnly[readonly] {
            background: #fff;
            color: #000;
            border: 0px solid #fff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <contenttemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="30%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_FEES_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEF_YM" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ia_required_FEES_YM%>"
                                            ControlToValidate="txt_DEF_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="reg_DEF_YM" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_FEES_YM%>" ControlToValidate="txt_DEF_YM" ForeColor="Red"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                         <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2ia_INS_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                           <asp:DropDownList ID="ddl_INS_TYPE_A" runat="server" ClientIDMode="Static"  CssClass="MandatoryField"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init0">
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <hr1 />
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td></td>
                    <td align="right" class="Body_label">
                        <div id="init2">
                            <aces:Btn ID="WFB2IA4101Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA4100Process%>" OnClick="WFB2IA4101Process_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();"/>
                            <%--<asp:Button ID="WFB2IA4101Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA4100Process%>" OnClick="WFB2IA4101Process_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();"/>--%>
                             <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,btn_back%>" OnClick="btn_back_Click" />
                        </div>
                    </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </contenttemplate>

</asp:Content>

