<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2db/WFB2DB0500_Qry.aspx.cs" Inherits="WebContent_fb2db_WFB2DB0500_Qry" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {

            gridviewScroll();
            $.unblockUI();
        }
        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }

        function ClearAll() {
            $('#txt_WS_CD').val("");
            $('#txt_WORK_CD').val("");
            $('#txt_WORK_DAY_CD').val("");
            $('#txt_SHIFT_CD').val("");
            return false;
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0) {
                return confirm($('#hid_Del_ConfirmMessage').val());
            }
            else {
                alert($('#hid_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDelAllAction() {
            var processed = true;
            BlockUI();
            processed = confirm("確定要一括刪除?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                                    <%--職種--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2db_lb_WS_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_WS_CD" Width="80px" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--工數區分--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_WORK_CD" runat="server" Text="<%$Resources:Resource,wfb2db_lb_WORK_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_WORK_CD" Width="80px" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--出勤別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_WORK_DAY_CD" runat="server" Text="<%$Resources:Resource,wfb2db_lb_WORK_DAY_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_WORK_DAY_CD" Width="80px" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--班別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2db_lb_SHIFT_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SHIFT_CD" Width="80px" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DB0500Search" runat="server" Text="查詢" OnClick="WFB2DB0500Search_Click" OnClientClick="BlockUI();" />
                                            <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,btn_clear%>" OnClientClick="return ClearAll();" />
                                            <aces:Btn ID="WFB2DB0500Import" runat="server" Text="<%$Resources:Resource,Import%>" OnClick="WFB2DB0500Import_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <aces:Btn ID="WFB2DB0500Delete" runat="server" Text="刪除" OnClientClick="return CheckDelAction();" OnClick="WFB2DB0500Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2DB0500DeleteAll" runat="server" Text="一括刪除"  OnClientClick="return CheckDelAllAction();" OnClick="WFB2DB0500DeleteAll_Click"  Visible="false" />
                        </div>
                    </td>
                </tr>
                <tr></tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="WFB2DB0500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_WS_CD" Name="ws_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_WORK_CD" Name="work_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_WORK_DAY_CD" Name="work_day_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_SHIFT_CD" Name="shift_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                    <%--職種--%>
                    <asp:BoundField DataField="WS_CD" HeaderText="<%$Resources:Resource,wfb2db_lb_WS_CD%>" SortExpression="WS_CD" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                    <%--工數區分--%>
                    <asp:BoundField DataField="WORK_CD" HeaderText="<%$Resources:Resource,wfb2db_lb_WORK_CD%>" SortExpression="WORK_CD" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                    <%--出勤別--%>
                    <asp:BoundField DataField="WORK_DAY_CD" HeaderText="<%$Resources:Resource,wfb2db_lb_WORK_DAY_CD%>" SortExpression="WORK_DAY_CD" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                    <%--班別--%>
                    <asp:BoundField DataField="SHIFT_CD" HeaderText="<%$Resources:Resource,wfb2db_lb_SHIFT_CD%>" SortExpression="SHIFT_CD" HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-HorizontalAlign="Left" />
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>

            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_d_txt_year" runat="server" ClientIDMode="Static" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Del_NotChoiceMessage" Value="<%$Resources:Resource,Add_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Mod_NotChoiceMessage" Value="<%$Resources:Resource,Edit_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Save_ConfirmMessage" Value="<%$Resources:Resource,Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2CancelConfirm%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2_Mod_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2db_Cancel_Confirm%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="btn_cancel" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

