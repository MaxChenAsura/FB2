<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sk/WFB2SK0200_Qry.aspx.cs" Inherits="WebContent_fb2sk_WFB2SK0200_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
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
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="30%" />
                                <col width="20%" />

                            </colgroup>
                            <tbody>
                                <br /><br /><br /><br /><br />
                                <tr>
                                    <th colspan="4" width="100%" align="center" class="Body_label"><b><font color="#0033CC" size="5">本作業會將 人事主檔  的資料 產生至文字檔中</font></b></th>
                                </tr>
                                <tr><td><br/><br/></td></tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                        
                                            <aces:Btn ID="WFB2SK0200TxtDown" runat="server" Text="<%$Resources:Resource,wfb2sk_TextDown%>" ClientIDMode="Static" OnClientClick="BlockUI();" OnClick="WFB2SK0200TxtDown_Click" />
                                        <%--
                                                <asp:Button ID="WFB2SK0200TxtDown" runat="server" Text="<%$Resources:Resource,wfb2sk_TextDown%>" ClientIDMode="Static" OnClientClick="BlockUI();" OnClick="WFB2SK0200TxtDown_Click" />
                                               --%> 
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SK0200TxtDown" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
