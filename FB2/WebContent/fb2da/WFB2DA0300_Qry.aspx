<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2da/WFB2DA0300_Qry.aspx.cs" Inherits="WebContent_fb2da_WFB2DA0300_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');

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
            $('#ddl_CALENDAR_CD').val("-1");
            $('#txt_CALENDAR_DT').val("");
            $('#ddl_DT_TYPE_O').val("-1");
            $('#ddl_DT_TYPE_N').val("-1");
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

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1) {
                var processed = true;
                BlockUI();
                processed = confirm("確定要執行?");
                if (!processed) {
                    $.unblockUI();
                }

                return processed;
            }
            else {
                alert($('#hid_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hid_Dtl_NotChoiceMessage').val());
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                                    <%--行事曆代碼--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CALENDAR_CD" runat="server" Text="<%$Resources:Resource,wfb2da_lb_CALENDAR_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_CALENDAR_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <%--日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CALENDAR_DT" runat="server" Text="<%$Resources:Resource,wfb2da_lb_CALENDAR_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CALENDAR_DT" Width="80px" CssClass="date" runat="server" ClientIDMode="Static"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidatorCALENDAR_DT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2da_ERR_CALENDAR_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_CALENDAR_DT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--日期類型(原)--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DT_TYPE_O" runat="server" Text="<%$Resources:Resource,wfb2da_lb_DT_TYPE_O%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_DT_TYPE_O" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <%--日期類型(新)--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DT_TYPE_N" runat="server" Text="<%$Resources:Resource,wfb2da_lb_DT_TYPE_N%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_DT_TYPE_N" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DA0300Search" runat="server" Text="查詢" OnClick="WFB2DA0300Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                            <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,btn_clear%>" OnClientClick="return ClearAll();" />
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
                            <aces:Btn ID="WFB2DA0300Add" runat="server" Text="新增" OnClick="WFB2DA0300Add_Click" />
                            <aces:Btn ID="WFB2DA0300Delete" runat="server" Text="刪除" OnClientClick="return CheckDelAction();" OnClick="WFB2DA0300Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2DA0300EXECUTE" runat="server" Text="執行" OnClientClick="return CheckModeifyAction();" OnClick="WFB2DA0300EXECUTE_Click"  Visible="false" />
                        </div>
                    </td>
                </tr>
                <tr></tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="WFB2DA0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_CALENDAR_CD" Name="calendar_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_CALENDAR_DT" Name="calendar_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_DT_TYPE_O" Name="dt_type_o" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_DT_TYPE_N" Name="dt_type_n" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
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
                    <%--行事曆代碼--%>
                    <asp:BoundField DataField="CALENDAR_CD" HeaderText="<%$Resources:Resource,wfb2da_lb_CALENDAR_CD%>" SortExpression="CALENDAR_CD" HeaderStyle-Width="400px" ItemStyle-Width="400px" ItemStyle-HorizontalAlign="Left" />
                    <%--日期--%>
                    <asp:BoundField DataField="CALENDAR_DT" HeaderText="<%$Resources:Resource,wfb2da_lb_CALENDAR_DT%>" SortExpression="CALENDAR_DT" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                    <%--日期類型(原)--%>
                    <asp:BoundField DataField="DT_TYPE_O" HeaderText="<%$Resources:Resource,wfb2da_lb_DT_TYPE_O%>" SortExpression="DT_TYPE_O" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left" />
                    <%--日期類型(新)--%>
                    <asp:BoundField DataField="DT_TYPE_N" HeaderText="<%$Resources:Resource,wfb2da_lb_DT_TYPE_N%>" SortExpression="DT_TYPE_N" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left" />
                    <%--處理狀態--%>
                    <asp:BoundField DataField="PROC_STATUS" HeaderText="<%$Resources:Resource,wfb2da_lb_PROC_STATUS%>" SortExpression="PROC_STATUS" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
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

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Del_NotChoiceMessage" Value="<%$Resources:Resource,Add_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Mod_NotChoiceMessage" Value="<%$Resources:Resource,Edit_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Save_ConfirmMessage" Value="<%$Resources:Resource,Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2CancelConfirm%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2_Mod_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2da_Cancel_Confirm%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="btn_cancel" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>

