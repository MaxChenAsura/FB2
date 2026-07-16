<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sf/WFB2SF1100_Qry.aspx.cs" Inherits="WebContent_fb2sf_WFB2SF1100_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.ymd').mask('9999/99/99');
            $.unblockUI();
        }
        function CheckSALARY_TYPE(source, arguments) {
            if ($("#ddl_SALARY_TYPE").val() == "-1")
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--頁面table-->
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="30%" />
                                <col width="15%" />
                                <col width="60%" />
                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sf_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_SALARY_DT" runat="server" MaxLength="8" Width="80px" CssClass="MandatoryField ymd date" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_SALARY_DT%>"
                                            ControlToValidate="txt_SALARY_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None" ClientIDMode="Static"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_error_SALARY_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sf_SALARY_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sf_required_ddl_SALARY_TYPE%>" ClientValidationFunction="CheckSALARY_TYPE" ForeColor="Red"
                                            ControlToValidate="ddl_SALARY_TYPE" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SF1100Execute" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF1100Execute%>" OnClick="WFB2SF1100Execute_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />

                                            <%--<asp:Button ID="WFB2SF1100Execute" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF1100Execute%>" OnClick="WFB2SF1100Execute_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>

                        </table>
                    </td>
                </tr>

            </table>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <!--resource)-->
            <%--<asp:HiddenField ID="hid_wfb2sk_required_SALARY_YM3" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_required_SALARY_YM3%>" />
            <asp:HiddenField ID="hid_wfb2ia_error_SALARY_YM3" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2ia_error_SALARY_YM3%>" />--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
