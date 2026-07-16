<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA2200_Qry.aspx.cs" Inherits="WebContent_fb2ia_WFB2IA2200_Qry" Culture="auto" UICulture="auto"%>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $.unblockUI();
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
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="90%" />

                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_INS_YM" runat="server" Text="<%$Resources:Resource,wfb2ia_lb_INS_YM%>"></asp:Label>:
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_INS_YM" runat="server" MaxLength="6" Width="64px" CssClass="MandatoryField ym date" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sia_required_INS_YM%>"
                                            ControlToValidate="txt_INS_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_INS_YM%>" ControlToValidate="txt_INS_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>

                                </tr>
                                <tr><td><br/></td></tr>
                                <tr>
                                    
                                    <td align="left" class="Body_label" colspan="2">
                                        <div id="init">
                                             <aces:Btn ID="WFB2IA2200Excel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA2200Excel%>" ClientIDMode="Static" OnClick="WFB2IA2200Excel_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA"  />
                                            <aces:Btn ID="WFB2IA2201Excel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA2201Excel%>" ClientIDMode="Static" OnClick="WFB2IA2201Excel_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA"  />
                                            <aces:Btn ID="WFB2IA2203Excel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA2203Excel%>" ClientIDMode="Static" OnClick="WFB2IA2203Excel_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA"  />
                                            <aces:Btn ID="WFB2IA2200Execute" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA2200Execute%>" ClientIDMode="Static" OnClick="WFB2IA2200Execute_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();"  />
                                            
                                            <%--<asp:Button ID="WFB2IA2200Excel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA2200Excel%>" ClientIDMode="Static" OnClick="WFB2IA2200Excel_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA"  />
                                            <asp:Button ID="WFB2IA2201Excel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA2201Excel%>" ClientIDMode="Static" OnClick="WFB2IA2201Excel_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA"  />
                                            <asp:Button ID="WFB2IA2203Excel" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA2203Excel%>" ClientIDMode="Static" OnClick="WFB2IA2203Excel_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA"  />
                                            <asp:Button ID="WFB2IA2200Execute" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA2200Execute%>" ClientIDMode="Static" OnClick="WFB2IA2200Execute_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();"  />--%>
                                        </div>
                                    </td>
                                    
                                </tr>

                            </tbody>
                            
                        </table>
                        <tr><td><hr/></td></tr>
                    </td>
                </tr>
            </table>

            <!--紀錄頁數(跳頁用)-->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <!--resource)-->
            <%--<asp:HiddenField ID="hid_wfb2sk_Announce_NotChoiceMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2sk_Announce_Message" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2sk_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2sk_Del_ConfirmMessage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_wfb2sk_Mod_NotChoiceMessage" runat="server" ClientIDMode="Static" />--%>
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger ControlID="WFB2IA2200Excel" />
            <asp:PostBackTrigger ControlID="WFB2IA2201Excel" />
            <asp:PostBackTrigger ControlID="WFB2IA2203Excel" />
            <%--<asp:PostBackTrigger ControlID="WBF2IA2200Execute" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
