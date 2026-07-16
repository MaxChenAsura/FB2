<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2se/WFB2SE0100_Edit.aspx.cs" Inherits="WebContent_fb2se_WFB2SE0100_Edit" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $('.ym').mask('9999/99');
            $(".number").mask('9999999');
            //$(".number").formatNumber({ format: "#,###", locale: "tw" });
            $.unblockUI();
        }
        function CheckLEVEL_CD(source, arguments) {
            if ($("#ddl_LEVEL_CD").val() == "" || $("#ddl_LEVEL_CD").val() == "-1")
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }
        function CheckGreater1(source, arguments) {
            if (parseInt($("#txt_LEVEL_PAY_AVG").val(), 10) <= parseInt($("#txt_LEVEL_PAY_LOW").val(), 10))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }
        function CheckGreater2(source, arguments) {
            if (parseInt($("#txt_LEVEL_PAY_UP").val(), 10) <= parseInt($("#txt_LEVEL_PAY_AVG").val(), 10))
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
                                        <asp:Label ID="lb_EFFECT_YM" runat="server" Text="<%$Resources:Resource,wfb2se_EFFECT_YM%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EFFECT_YM" runat="server" MaxLength="6" Width="80px" ReadOnly="true" BorderWidth="0" CssClass="ym" ClientIDMode="Static" Text=""></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2se_LEVEL_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_LEVEL_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField" OnTextChanged="ddl_LEVEL_CD_TextChanged" AutoPostBack="true"></asp:DropDownList>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_required_LEVEL_CD%>" ClientValidationFunction="CheckLEVEL_CD" ForeColor="Red"
                                            ControlToValidate="ddl_LEVEL_CD" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ABILITY_ADJ" runat="server" Text="<%$Resources:Resource,wfb2se_ABILITY_ADJ%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox Width="80px" Style="text-align: right;" ID="txt_ABILITY_ADJ" runat="server" MaxLength="7" ClientIDMode="Static" CssClass="number MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2se_required_ABILITY_ADJ%>"
                                            ControlToValidate="txt_ABILITY_ADJ" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="checknum" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="資格調額輸入錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_ABILITY_ADJ" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEVEL_PAY_UP" runat="server" Text="<%$Resources:Resource,wfb2se_LEVEL_PAY_UP%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox Width="80px" Style="text-align: right;" ID="txt_LEVEL_PAY_UP" runat="server" MaxLength="7" ClientIDMode="Static" CssClass="number MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2se_required_LEVEL_PAY_UP%>"
                                            ControlToValidate="txt_LEVEL_PAY_UP" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2se_Mod_errorMessage%>" ClientValidationFunction="CheckGreater1" ForeColor="Red"
                                            ControlToValidate="txt_LEVEL_PAY_UP" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="checknum2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="上限輸入錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_LEVEL_PAY_UP" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEVEL_PAY_AVG" runat="server" Text="<%$Resources:Resource,wfb2se_LEVEL_PAY_AVG%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox Width="80px" Style="text-align: right;" ID="txt_LEVEL_PAY_AVG" runat="server" MaxLength="7" ClientIDMode="Static" CssClass="number MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2se_required_LEVEL_PAY_AVG%>"
                                            ControlToValidate="txt_LEVEL_PAY_AVG" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2se_Mod_errorMessage1%>" ClientValidationFunction="CheckGreater2" ForeColor="Red"
                                            ControlToValidate="txt_LEVEL_PAY_AVG" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="checknum3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="中數輸入錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_LEVEL_PAY_AVG" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEVEL_PAY_LOW" runat="server" Text="<%$Resources:Resource,wfb2se_LEVEL_PAY_LOW%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox Width="80px" Style="text-align: right;" ID="txt_LEVEL_PAY_LOW" runat="server" MaxLength="7" ClientIDMode="Static" CssClass="number MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2se_required_LEVEL_PAY_LOW%>"
                                            ControlToValidate="txt_LEVEL_PAY_LOW" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="checknum4" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="下限輸入錯誤" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_LEVEL_PAY_LOW" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SE0101OK" runat="server" Text="<%$Resources:Resource,wfb2sk_ok%>" OnClick="WFB2SE0101OK_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />

                                            <%--<asp:Button ID="WFB2SE0101OK" runat="server" Text="<%$Resources:Resource,wfb2sk_ok%>" OnClick="WFB2SE0101OK_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sk_cancel%>" OnClick="btn_cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
