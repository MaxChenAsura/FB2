<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sk/WFB2SK0100_Qry.aspx.cs" Inherits="WebContent_fb2sk_WFB2SK0100_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            //$(".number").formatNumber({ format: "#,###", locale: "tw" });
            $.unblockUI();
        }

        //清空
        function doClear() {
            $("#txt_DATA_YM_S").val("");
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
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
                                        <asp:Label ID="lb_DATA_YM" runat="server" Text="<%$Resources:Resource,wfb2sk_DATA_YM%>"></asp:Label>
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_DATA_YM_S" runat="server" MaxLength="6" Width="64px" CssClass="MandatoryField ym date" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="年月不可空白"
                                            ControlToValidate="txt_DATA_YM_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2si_error_DATA_YM%>" ControlToValidate="txt_DATA_YM_S" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>

                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SK0100TxtDown" runat="server" Text="<%$Resources:Resource,wfb2sk_TextDown%>" ClientIDMode="Static" OnClientClick="CheckValid();" OnClick="WFB2SK0100TxtDown_Click" ValidationGroup="GroupA"  />

                                            <%--<asp:Button ID="WFB2SK0100TxtDown" runat="server" Text="<%$Resources:Resource,wfb2sk_TextDown%>" ClientIDMode="Static" OnClientClick="CheckValid();" OnClick="WFB2SK0100TxtDown_Click" ValidationGroup="GroupA"  />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sk_clear%>" OnClientClick="return doClear();" />
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
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SK0100TxtDown" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
