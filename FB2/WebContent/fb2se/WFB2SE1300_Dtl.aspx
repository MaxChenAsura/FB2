<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2se/WFB2SE1300_Dtl.aspx.cs" Inherits="WebContent_fb2se_WFB2SE1300_Dtl" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        //自動調整FRAME高度

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $.unblockUI();
            $("#tabs").tabs();
            $("#ui-id-2").click(function () {
                var iframe = document.getElementById('iframe2');
                iframe.src = iframe.src;
            });
            $("#ui-id-3").click(function () {
                var iframe = document.getElementById('iframe3');
                iframe.src = iframe.src;
            });




        }
        function ChangeTab(tab) {
            $("#tabs").tabs({ active: tab });

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="1" cellspacing="1" class="Body_Label" width="1020">
                <colgroup>
                    <col width="13%" />
                    <col width="37%" />
                    <col width="10%" />
                    <col width="40%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EFFECT_YM" runat="server" Text="<%$Resources:Resource,wfb2se_lb_EFFECT_YM%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="lbl_EFFECT_YM" runat="server"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_RELEASE_NAME" runat="server" Text="<%$Resources:Resource,wfb2se_lb_RELEASE_NAME%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="lbl_RELEASE_NAME" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_RELEASE_DT" runat="server" Text="<%$Resources:Resource,wfb2se_lb_RELEASE_DT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="lbl_RELEASE_DT" runat="server"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SUB_DESC" runat="server" Text="<%$Resources:Resource,wfb2se_lb_SUB_DESC%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="lbl_SUB_DESC" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPROVE_NAME" runat="server" Text="<%$Resources:Resource,wfb2se_lb_APPROVE_NAME%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="lbl_APPROVE_NAME" runat="server"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2se_lb_APPROVE_DT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="lbl_APPROVE_DT" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2se_lb_REMARK%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="3">
                            <asp:TextBox ID="txt_REMARK" runat="server" Rows="4" TextMode="MultiLine" Columns="45" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="width: 900px; text-align: right;">
                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sc_Back%>" ClientIDMode="Static" OnClick="btn_cancel_Click" /> <%--OnClientClick="$(location).attr('href', 'WFB2SE1300_Qry.aspx'); return false;"--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4" height="1">
                            <hr></hr>
                        </td>
                    </tr>
                    <tr align="right">
                        <td colspan="4" >
                            <aces:Btn ID="WFB2SE1300Release" runat="server" Text="<%$Resources:Resource,btn_approve2%>" ClientIDMode="Static" OnClick="WFB2SE1300Release_Click" Enabled="false" OnClientClick="BlockUI();" /></td>

                            <%--<asp:Button ID="WFB2SE1300Release" runat="server" Text="<%$Resources:Resource,btn_approve2%>" ClientIDMode="Static" OnClick="WFB2SE1300Release_Click" Enabled="false" OnClientClick="BlockUI();" />--%></td>
                    </tr>
                </tbody>
            </table>
            <tr>
            </tr>
            <tr>
                <td class="Body_label">

                    <div id="tabs" style="width: 1020px; height: 500px">
                        <ul>
                            <li><a href="#tabs-1">【個人調薪】</a></li>
                            <li><a href="#tabs-2">【3A以下調薪設定值】</a></li>
                            <li><a href="#tabs-3">【2B以上調薪設定值】</a></li>
                        </ul>
                        <div id="tabs-1">
                            <iframe id="iframe1" name="iframe" frameborder="0" border="0" cellspacing="0" width="1020px" height="600px" src="" runat="server" style="border: none" scrolling="no"></iframe>
                        </div>
                        <div id="tabs-2">
                            <iframe id="iframe2" name="iframe" frameborder="0" border="0" cellspacing="0" width="1020px" height="600px" src="" runat="server" style="border: none" clientidmode="Static" scrolling="no"></iframe>
                        </div>
                        <div id="tabs-3">
                            <iframe id="iframe3" name="iframe" frameborder="0" border="0" cellspacing="0" width="1020px" height="600px" src="" runat="server" style="border: none" clientidmode="Static" scrolling="no"></iframe>
                        </div>
                    </div>




                </td>
            </tr>
            <tr>
                <td class="Body_label"></td>
            </tr>

            </table>

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

