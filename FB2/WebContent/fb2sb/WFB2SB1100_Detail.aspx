<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sb/WFB2SB1100_Detail.aspx.cs" Inherits="WebContent_fb2sb_WFB2SB1100_Detail" %>
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

        function IsDelete() {
            var answer = confirm($('#hidwfb299_Del_ConfirmMessage').val());
            if (answer)
                return true;
            else {
                document.getElementById('HID_cancel').click();
                return false;
            }
        }

        function reDirectHome() {
            window.location.href = "WFB2SB1100_Qry.aspx";
        }

        function selectAll(from, to) {
            $("#" + from).children("option").each(function () {
                $("#" + to).append($("<option></option>").attr("value", this.value).text(this.text));
                //$("#" + to).append(new Option(this.text, this.value, true, true));

            });

            //  移除所有項目
            $("#" + from).children("option").remove();
        }

        function move_item(from, to) {

            $("#" + from).find(":selected").each(function () {
                $("#" + to).append($("<option></option>").attr("value", this.value).text(this.text));
            });

            //  移除選擇的項目
            $("#" + from).find(":selected").remove();
        }
        function sendSelectedItem() {
            var send_select = "";
            $("#lb_select").children("option").each(function () {
                send_select += this.value + ",";
            });
            $("#hid_selectedItem").val(send_select);
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="13%" />
                                <col width="30%" />
                                <col width="13%" />
                                <col width="44%" />
                            </colgroup>
                            <tbody>
                                <!-- START: 1st Line in Search Criteria area -->
                                <tr>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2dg_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:Label ID="lb_EMP_ID" runat="server"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                         <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_NAME2%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:Label ID="lb_EMP_NAME" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,WFB2SB_SUB_CD%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:Label ID="lb_SUB_CD" runat="server"></asp:Label>
                                    </td>
                                    <th></th>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td align="center" height="1" colspan="7">
                                        <hr>
                                    </td>
                                </tr>
                                <!-- end: Create MODULE ID -->
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
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
                                    <td style="text-align: center">
                                        <asp:Label ID="lb_NonSelected" runat="server" ClientIDMode="Static" Text="<%$Resources:Resource,wfb2sc_lb_NonSelected%>"></asp:Label>
                                    </td>
                                    <td></td>
                                    <td style="text-align: center">
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
                    </td>
                </tr>
                <tr>
                    <td class="Body_label">
                        <div id="init_grid" align="right">
                            <aces:Btn ID="WFB2SB1100Ok1" runat="server" Text="<%$Resources:Resource,btn_Save%>" Visible="true" ValidationGroup="GroupA" OnClientClick="sendSelectedItem();" OnClick="WFB2SB1100Save_Click" />
                            <%--<asp:Button ID="WFB2SB1100Ok1" runat="server" Text="<%$Resources:Resource,btn_Save%>" Visible="true" ValidationGroup="GroupA" OnClientClick="sendSelectedItem();" OnClick="WFB2SB1100Save_Click" />--%>
                            <asp:Button ID="WFB2SB1100Cancel" runat="server" Text="<%$Resources:Resource,btn_Cancel%>" Visible="true" OnClientClick="return confirm($('#hidwfb299_Cancel_ConfirmMessage').val())" OnClick="WFB2SB1100Cancel_Click" />
                        </div>
                    </td>
                </tr>

            </table>
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
             <asp:HiddenField ID="hid_selectedItem" runat="server" ClientIDMode="Static" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

