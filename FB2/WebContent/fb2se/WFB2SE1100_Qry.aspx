<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2se/WFB2SE1100_Qry.aspx.cs" Inherits="WebContent_WFB2SE1100_Qry" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".date2").mask('9999/99');
            $.unblockUI();
        }

        function saveCheck() {
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
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

        <tr height="100%" valign="top">
            <td>
                <!--查詢條件table(HTML)-->
                <!--TextBox,input button,DropDownList-->
                <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                    <colgroup>
                        <col width="10%" />
                        <col width="40%" />
                        <col width="20%" />
                        <col width="30%" />
                    </colgroup>
                    <tbody>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_EFFECT_YM" runat="server" Text="<%$Resources:Resource,wfb2_EFFECT_YM%>"></asp:Label>:
                            </th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_EFFECT_YM" runat="server" MaxLength="6" Width="64px" CssClass="MandatoryField date2" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2_EFFECT_YM_NOT_EMPTY%>"
                                    ControlToValidate="txt_EFFECT_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2_START_DATE_FORMAT_ERROR%>" ControlToValidate="txt_EFFECT_YM" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                            </td>
                           <%--  <th align="left" class="Body_TableHeader"></th> --%>
                            <td align="left" class="Body_label"></td>
                        </tr>
                        <th></th>
                        <th></th>
                            <td align="right" class="Body_label">
                                <div id="init">
                                    <aces:Btn ID="WFB2SE1100Execute" runat="server" Text="<%$Resources:Resource,wfb2se_WFB2SE1100Process%>" ClientIDMode="Static" OnClick="WFB2SE1100Execute_Click" ValidationGroup="GroupA" OnClientClick="return saveCheck();" />

                                    <%--<asp:Button ID="WFB2SE1100Execute" runat="server" Text="<%$Resources:Resource,wfb2se_WFB2SE1100Process%>" ClientIDMode="Static" OnClick="WFB2SE1100Execute_Click" ValidationGroup="GroupA" OnClientClick="return saveCheck();" />--%>
                                </div>
                            </td>
                        </tr>
                    </tbody>

                </table>
            </td>
        </tr>

    </table>


    <!--紀錄頁數(跳頁用)-->
    <asp:HiddenField ID="HID_BILLS_KIND" runat="server" ClientIDMode="Static" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
    <!--resource)-->
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
