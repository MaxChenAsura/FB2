<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC1200_Dtl.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC1200_Dtl" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $.unblockUI();
        }
        function move_item(from, to) {

            $("#" + from).find(":selected").each(function () {
                $("#" + to).append($("<option></option>").attr("value", this.value).text(this.text));
            });

            //  移除選擇的項目
            $("#" + from).find(":selected").remove();
        }

        function selectAll(from, to) {
            $("#" + from).children("option").each(function () {
                $("#" + to).append($("<option></option>").attr("value", this.value).text(this.text));
            });
            //  移除所有項目
            $("#" + from).children("option").remove();
        }
        function sendSelectedItem() {
            var send_select = "";
            $("#lb_select").children("option").each(function () {
                send_select += this.value + ",";
            });
            $("#hid_selectedItem").val(send_select);
        }
        function backQry() {
            return confirm($('#hidwfb2sc_Cancel_ConfirmMessage').val())
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
                                    <%--用途別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_KIND_CD" runat="server" Text="<%$Resources:Resource,wfb2sm_KIND_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_KIND_CD_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <%--群組類別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GROUP_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_GROUP_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_GROUP_TYPE_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--群組代號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GROUP_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_GROUP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_GROUP_ID_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <%--群組名稱--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GROUP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sc_GROUP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_GROUP_NAME_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label"></td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <hr />
                                    </td>
                                </tr>

                            </tbody>
                        </table>


                        <table width="50%" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                            <colgroup>
                                <col width="10%" />
                                <col width="35%" />
                                <col width="20%" />
                                <col width="35%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <td></td>
                                    <td align="center">
                                        <asp:Label ID="lb_NonSelected" runat="server" ClientIDMode="Static" Text="<%$Resources:Resource,wfb2sc_lb_NonSelected%>"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td align="center">
                                        <asp:Label ID="lb_Selected" runat="server" ClientIDMode="Static" Text="<%$Resources:Resource,wfb2sc_lb_Selected%>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td height="177">
                                        <asp:ListBox ID="lb_unselect" runat="server" SelectionMode="Multiple" ClientIDMode="Static" ondblclick="move_item('lb_unselect', 'lb_select')"
                                            Height="177" Width="200px"></asp:ListBox>
                                    </td>
                                    <td>
                                        <table align="center">
                                            <tr>
                                                <td>
                                                    <input type="button" name="btnnext" value="＞" onclick="move_item('lb_unselect', 'lb_select')" id="btnnext" style="height: 30px; width: 30px;" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="button" name="btnlast" value="＜" onclick="move_item('lb_select', 'lb_unselect')" id="btnlast" style="height: 30px; width: 30px;" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="button" name="btnend" value=">>" onclick="selectAll('lb_unselect', 'lb_select')" id="btnend" style="height: 30px; width: 30px;" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <input type="button" name="btnfirst" value="<<" onclick="selectAll('lb_select', 'lb_unselect')" id="btnfirst" style="height: 30px; width: 30px;" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lb_select" runat="server" ClientIDMode="Static" SelectionMode="Multiple" Height="177" Width="200px" ondblclick="move_item('lb_select', 'lb_unselect')"></asp:ListBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table width="1020px">
                            <tr>
                                <td align="right" class="Body_label" colspan="4">
                                    <aces:Btn ID="WFB2SC1200Ok1" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClientClick="sendSelectedItem();" OnClick="WFB2SC1200Ok1_Click" ValidationGroup="GroupB" />
                                    <%--<asp:Button ID="WFB2SC1200Ok1" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClientClick="sendSelectedItem();" OnClick="WFB2SC1200Ok1_Click" ValidationGroup="GroupB" />--%>
                                    <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_cancel%>" OnClientClick="backQry();" OnClick="btn_cancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hid_KIND_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_GROUP_TYPE" runat="server" ClientIDMode="static" />
            <asp:HiddenField ID="hid_GROUP_ID" runat="server" ClientIDMode="static" />
            <asp:HiddenField ID="hid_selectedItem" runat="server" ClientIDMode="Static" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Cancel_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


