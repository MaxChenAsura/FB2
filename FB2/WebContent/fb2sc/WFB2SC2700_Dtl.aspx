<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2700_Dtl.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2700_Dtl" Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.ym').mask('9999/99');
            $('.ymd').mask('9999/99/99');
            //$(".number").formatNumber({ format: "#,###", locale: "tw" });
            $.unblockUI();

        }
        //發送EMAIL日期 不可小於等於系統日!
        function CheckYMD(source, arguments) {
            var now = new Date();
            var Current = now.getFullYear() + "/" + (now.getMonth() + 1) + "/" + now.getDate();
            var email_date = $("#txt_EMAIL_DT").val();
            if ((Date.parse(email_date)).valueOf() <= (Date.parse(Current)).valueOf()) {
                arguments.IsValid = false;
            }
            else {
                arguments.IsValid = true;
            }
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
                                <col width="40%" />
                                <col width="15%" />
                                <col width="20%" />
                                <col width="10%" />

                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--發薪類別--%>
                                        <asp:Label ID="lb_SALARY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_TYPE" runat="server" MaxLength="20" ClientIDMode="Static" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <%--薪資年月--%>
                                        <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_YM" runat="server" MaxLength="6" Width="64px" CssClass="ym" ClientIDMode="Static" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--發薪日期--%>
                                        <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sf_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_DT" runat="server" MaxLength="10" Width="100px" CssClass="ymd" ClientIDMode="Static" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <%--發放項目--%>
                                        <asp:Label ID="lb_PAY_KIND" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PAY_KIND%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PAY_KIND" runat="server" MaxLength="6" Width="100px" ClientIDMode="Static" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--處理狀態--%>
                                        <asp:Label ID="lb_PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PROCESS_STATUS" runat="server" MaxLength="20" Width="80px" ClientIDMode="Static" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <%--關帳代號--%>
                                        <asp:Label ID="lb_PAY_ID" runat="server" Text="<%$Resources:Resource,wfb2_lb_PAY_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PAY_ID" runat="server" MaxLength="11" Width="80px" ClientIDMode="Static" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--關帳日期--%>
                                        <asp:Label ID="lb_PAY_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PAY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PAY_DT" runat="server" MaxLength="10" Width="80px" ClientIDMode="Static" CssClass="ymd" BorderWidth="0" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <%--EMAIL發送日期--%>
                                        <asp:Label ID="lb_EMAIL_DT" runat="server" Text="<%$Resources:Resource,wfd2sc_EMAIL_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMAIL_DT" runat="server" MaxLength="10" Width="80px" ClientIDMode="Static" CssClass="MandatoryField ymd date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="required_EMAIL_DT" runat="server" ErrorMessage="<%$Resources:Resource,wfd2sc_Required_EMAIL_DT%>"
                                            ControlToValidate="txt_EMAIL_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfd2sc_Error_EMAIL_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_EMAIL_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfd2sc_GreaterThan_EMAIL_DT%>" ClientValidationFunction="CheckYMD" ForeColor="Red"
                                            ControlToValidate="txt_EMAIL_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--EMAIL主旨--%>
                                        <asp:Label ID="lb_TITLE" runat="server" Text="<%$Resources:Resource,wfd2sc_lb_TITLE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_TITLE" runat="server" MaxLength="150" Width="380px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="required_TITLE" runat="server" ErrorMessage="<%$Resources:Resource,wfd2sc_Required_TITLE%>"
                                            ControlToValidate="txt_TITLE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--EMAIL內文--%>
                                        <asp:Label ID="lb_CONTENT" runat="server" Text="<%$Resources:Resource,wfd2sc_lb_CONTENT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_CONTENT" runat="server" ClientIDMode="Static" MaxLength="150" Width="380px" CssClass="MandatoryField" BorderWidth="1" Style="overflow: auto"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="required_CONTENT" runat="server" ErrorMessage="<%$Resources:Resource,wfd2sc_Required_CONTENT%>"
                                            ControlToValidate="txt_CONTENT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>

                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SC2700Execute" runat="server" Text="<%$Resources:Resource,btn_confirm%>" OnClick="WFB2SC2700Execute_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />


                                            <%--確認--%>
                                            <%--<asp:Button ID="WFB2SC2700Execute" runat="server" Text="<%$Resources:Resource,btn_confirm%>" OnClick="WFB2SC2700Execute_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            <%--返回--%>
                                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2db_Back%>" OnClick="btn_cancel_Click" />
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
            <%-- <asp:HiddenField ID="hid_wfb2sk_Announce_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Announce_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sk_Announce_Message" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Announce_Message%>" />
            <asp:HiddenField ID="hid_wfb2sk_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Del_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sk_Del_ConfirmMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Del_ConfirmMessage%>" />
            <asp:HiddenField ID="hid_wfb2sk_Mod_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Mod_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sk_Save_Confirm" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Save_Confirm%>" />--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

