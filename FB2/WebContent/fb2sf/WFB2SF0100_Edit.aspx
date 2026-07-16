<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sf/WFB2SF0100_Edit.aspx.cs" Inherits="WebContent_fb2sf_WFB2SF0100_Edit" Culture="auto" UICulture="auto" %>
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
        //function CheckLEVEL_CD(source, arguments) {
        //    if ($("#ddl_LEVEL_CD").val() == "" || $("#ddl_LEVEL_CD").val() == "-1")
        //        arguments.IsValid = false;
        //    else
        //        arguments.IsValid = true;

        //}
        //function CheckGreater1(source, arguments) {
        //    if (parseInt($("#txt_LEVEL_PAY_AVG").val(), 10) <= parseInt($("#txt_LEVEL_PAY_LOW").val(), 10))
        //        arguments.IsValid = false;
        //    else
        //        arguments.IsValid = true;

        //}
        //function CheckGreater2(source, arguments) {
        //    if (parseInt($("#txt_LEVEL_PAY_UP").val(), 10) <= parseInt($("#txt_LEVEL_PAY_AVG").val(), 10))
        //        arguments.IsValid = false;
        //    else
        //        arguments.IsValid = true;

        //}

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
                                <col width="15%" />
                                <col width="10%" />
                                <col width="15%" />
                                <col width="10%" />
                                <col width="40%" />
                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DOC_NO_header" runat="server" Text="<%$Resources:Resource,wfb2sf_DOC_NO%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label">
                                        <asp:Label Width="100px" Style="text-align: left;" ID="lb_DOC_NO" runat="server"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAY_TARGET" runat="server" Text="<%$Resources:Resource,wfb2sf_PAY_TARGET%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label Width="100px" Style="text-align: left;" ID="lb_PAY_TARGET_DESC" runat="server"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CREDITOR_header" runat="server" Text="<%$Resources:Resource,wfb2sf_CREDITOR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label Width="400px" Style="text-align: left;" ID="lb_CREDITOR" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_VENDOR_ID" runat="server" Text="<%$Resources:Resource,wfb2sf_VENDOR_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:TextBox Width="100px" Style="text-align: left;" ID="txt_EDIT_VENDOR_ID" runat="server" MaxLength="10" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                   <%-- <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_VAILD" runat="server" Text="<%$Resources:Resource,wfb2sf_IS_VAILD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList Width="40px" Style="text-align: left;" ID="ddl_IS_VAILD" runat="server" ClientIDMode="Static" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_IS_VAILD_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EFFECT_EDT_header" runat="server" Text="<%$Resources:Resource,wfb2sf_EFFECT_EDT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label Width="100px" Style="text-align: left;" ID="lb_EFFECT_EDT" runat="server"></asp:Label>
                                    </td>--%>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MEMO" runat="server" Text="<%$Resources:Resource,wfb2sf_MEMO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="4">
                                        <asp:TextBox Width="500px" Style="text-align: left;" ID="txt_EDIT_MEMO" runat="server" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MEMODESC" runat="server" Text="<%$Resources:Resource,wfb2sf_MEMODESC%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="4">
                                        <asp:TextBox Width="500px" Style="text-align: left;" ID="txt_EDIT_MEMODESC" runat="server" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
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
                                            <aces:Btn ID="WFB2SF0101OK" runat="server" Text="<%$Resources:Resource,wfb2sk_ok%>" OnClick="WFB2SF0101OK_Click" OnClientClick="BlockUI();" />

                                            <%--<asp:Button ID="WFB2SF0101OK" runat="server" Text="<%$Resources:Resource,wfb2sk_ok%>" OnClick="WFB2SF0101OK_Click" OnClientClick="BlockUI();" />--%> <%--ValidationGroup="GroupA" OnClientClick="CheckValid();"--%>
                                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sk_cancel%>" OnClick="btn_cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
            <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />--%>
            <asp:HiddenField ID="data_key" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="PAY_TARGET" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
