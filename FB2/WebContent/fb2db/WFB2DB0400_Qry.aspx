<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2db/WFB2DB0400_Qry.aspx.cs" Inherits="WebContent_fb2db_WFB2DB0400_Qry" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
            $('#txt_EMP_ID').val("");
            $('#dll_EXEC_RESULT').val("-1");
            $('#txt_DT_O').val("");
            $('#ddl_DT_TYPE_O').val("-1");
            $('#ddl_DT_TYPE_N').val("-1");
            $('#txt_CHG_NO').val("");
            return false;
        }

        function CheckSYS_CD(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if ($("#txt_SYS_CD_Add").val().trim() != "") {
                if (!re.test($("#txt_SYS_CD_Add").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
        }
        function CheckYEAR(source, arguments) {
            var re = /^[0-9]+$/;
            if ($("#txt_YEAR").val().trim() != "") {
                if (!re.test($("#txt_YEAR").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
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

        function CheckModeifyAction(msg) {
            if (LookUpCheckboxs() > 0) {
                var processed = true;
                BlockUI();
                processed = confirm("確定要進行" + msg + "?");
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
        
        function checkEXEC(msg) {
            BlockUI();
            processed = confirm("確定要進行" + msg + "?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hid_Dtl_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckSaveAction() {
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    alert($('#hid_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hid_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hid_Save_ConfirmMessage').val());
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

        function MyCheckValid(msg) {
            var month = "";
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                $.unblockUI();
            } else {
                processed = false;
                return processed;
            }

            BlockUI();
            processed = confirm("確定要進行" + msg +"?");
            if (!processed) {
                $.unblockUI();
            }

            return processed;
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
                                <col width="15%" />
                                <col width="20%" />
                                <col width="15%" />
                                <col width="20%" />
                                <col width="15%" />
                                <col width="15%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--工號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2db_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" Width="80px" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--執行結果--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EXEC_RESULT" runat="server" Text="<%$Resources:Resource,wfb2db_lb_EXEC_RESULT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList runat="server" ID="dll_EXEC_RESULT" ClientIDMode="Static" />
                                    </td>
                                </tr>
                                 <tr>
                                       <%--原日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DT_O" runat="server" Text="<%$Resources:Resource,wfb2db_lb_DT_O%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DT_O" Width="80px" runat="server" ClientIDMode="Static" CssClass="date" ></asp:TextBox>
                                    </td>
                                    <%--IFLOWNO--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CHG_NO" runat="server" Text="<%$Resources:Resource,wfb2db_lb_CHG_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CHG_NO" Width="120px" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--原日期類型--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DT_TYPE_O" runat="server" Text="<%$Resources:Resource,wfb2db_lb_DT_TYPE_O%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList runat="server" ID="ddl_DT_TYPE_O" ClientIDMode="Static" />
                                    </td>
                                    <%--新日期類型--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DT_TYPE_N" runat="server" Text="<%$Resources:Resource,wfb2db_lb_DT_TYPE_N%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList runat="server" ID="ddl_DT_TYPE_N" ClientIDMode="Static" />
                                    </td>
                                </tr>
                               
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="2">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DB0400Search" runat="server" Text="查詢" OnClick="WFB2DB0400Search_Click" OnClientClick="BlockUI();" />
                                            <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,btn_clear%>" OnClientClick="return ClearAll();" />
                                            <aces:Btn ID="WFB2DB0400Import" runat="server" Text="<%$Resources:Resource,Import%>" OnClick="WFB2DB0400Import_Click" />
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
                            <aces:Btn ID="WFB2DB0400Delete" runat="server" Text="刪除" OnClientClick="return CheckDelAction();" OnClick="WFB2DB0400Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2DB0400EXEC" runat="server" Text="一括執行" OnClick="WFB2DB0400EXEC_Click"  Visible="false" OnClientClick="return checkEXEC(this.value);" />
                            <aces:Btn ID="WFB2DB0400REVERT" runat="server" Text="還原" OnClick="WFB2DB0400REVERT_Click"  Visible="false" OnClientClick="return CheckModeifyAction(this.value);"/>
                        </div>
                    </td>
                </tr>
                <tr></tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="WFB2DB0400DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="dll_EXEC_RESULT" Name="exec_result" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_DT_O" Name="dt_o" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_DT_TYPE_O" Name="dt_type_o" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_DT_TYPE_N" Name="dt_type_n" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_CHG_NO" Name="chg_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
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
                    <%--工號--%>
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2db_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <%--姓名--%>
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2db_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <%--原日期--%>
                    <asp:BoundField DataField="CALENDAR_DT" HeaderText="<%$Resources:Resource,wfb2db_lb_DT_O%>" SortExpression="CALENDAR_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <%--原日期類型--%>
                    <asp:BoundField DataField="DT_TYPE_O" HeaderText="<%$Resources:Resource,wfb2db_lb_DT_TYPE_O%>" SortExpression="DT_TYPE_O" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                    <%--新日期類型--%>
                    <asp:BoundField DataField="DT_TYPE_N" HeaderText="<%$Resources:Resource,wfb2db_lb_DT_TYPE_N%>" SortExpression="DT_TYPE_N" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                    <%--處理狀態--%>
                    <asp:BoundField DataField="PROC_STATUS" HeaderText="<%$Resources:Resource,wfb2db_lb_PROC_STATUS%>" SortExpression="PROC_STATUS" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                    <%--處理時間--%>
                    <asp:BoundField DataField="PROC_DT" HeaderText="<%$Resources:Resource,wfb2db_lb_PROC_DT%>" SortExpression="PROC_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <%--異動編號--%>
                    <asp:BoundField DataField="CHG_NO" HeaderText="<%$Resources:Resource,wfb2db_lb_CHG_NO%>" SortExpression="CHG_NO" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left" />
                    <%--IFLOWNO--%>
                    <asp:BoundField DataField="IFLOW_NO" HeaderText="<%$Resources:Resource,wfb2db_lb_FLOW_NO%>" SortExpression="IFLOW_NO" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left" />
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
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2db_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2_Mod_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2db_Cancel_Confirm%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="btn_cancel" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

