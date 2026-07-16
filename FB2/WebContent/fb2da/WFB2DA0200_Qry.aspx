<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2da/WFB2DA0200_Qry.aspx.cs" Inherits="WebContent_fb2da_WFB2DA0200_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Src="~/UserControl/UCCommCodeDropDwonList.ascx" TagPrefix="uc1" TagName="UCCommCodeDropDwonList" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
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
                barcolor: "#7F7F7F",
                headerrowcount: 1,
                freezesize: 4
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0) {
                BlockUI();
                if (confirm($('#hidwfb2da_Del_ConfirmMessage').val())) {
                    BlockUI();
                    return;
                }
                else {
                    $.unblockUI();
                    return false;
                }
            }
            else {
                alert($('#hidwfb2da_Del_NotChoiceMessage').val());
                $.unblockUI();
                return false;
            }
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1) {
                BlockUI();
                return true;
            }
            else {
                BlockUI();
                alert($('#hidwfb2da_Mod_NotChoiceMessage').val());
                $.unblockUI();
                return false;
            }
        }

        function CheckUnValidAction() {
            if (LookUpCheckboxs() == 1) {
                BlockUI();
                return true;
            }
            else {
                BlockUI();
                alert($('#hidwfb2da_UnVALID_NotChoiceMessage').val());
                $.unblockUI();
                return false;
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("_freezeitem") != -1 && ItemCheckBoxs[i].id.indexOf("_freezeheader") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        function CheckInput() {
            var message = "";
            if (document.getElementById('rb_VALID_N').checked || document.getElementById('rb_VALID_Y').checked) {
                if ($('#txt_LEAVE_DT_S').val() == "" && $('#txt_LEAVE_DT_E').val() == "") {
                    message += "未輸入生效期間，不可以點選「有效」、「無效」";
                }
            }
            var dutyTime = $('#ddl_DUTY_TIME_HH').val() + $('#ddl_DUTY_TIME_MM').val();
            if (dutyTime.length > 0 && dutyTime.length < 4)
                message += message == "" ? "上班時間(時分)必須完整輸入" : "\r\n上班時間(時分)必須完整輸入";

            var EAT_TIME = $('#ddl_EAT_TIME_HH').val() + $('#ddl_EAT_TIME_MM').val();
            if (EAT_TIME.length > 0 && EAT_TIME.length < 4)
                message += message == "" ? "用餐時間(時分)必須完整輸入" : "\r\用餐時間(時分)必須完整輸入";
            

            var REST_TIME = $('#ddl_REST_TIME_HH').val() + $('#ddl_REST_TIME_MM').val();
            if (REST_TIME.length > 0 && REST_TIME.length < 4)
                message += message == "" ? "休息時間(時分)必須完整輸入" : "\r\休息時間(時分)必須完整輸入";
            if ($('#txt_LEAVE_DT_S').val() != "" && !checkdate($('#txt_LEAVE_DT_S').val()))
                message += message == "" ? "生效日期(起)格是錯誤" : "\r\n生效日期(起)格是錯誤";

            if ($('#txt_LEAVE_DT_E').val() != "" && !checkdate($('#txt_LEAVE_DT_E').val()))
                message += message == "" ? "生效日期(迄)格是錯誤" : "\r\n生效日期(迄)格是錯誤";

            if ($('#txt_LEAVE_DT_S').val() != "" && $('#txt_LEAVE_DT_E').val() != "") {
                var Sdate = new Date($('#txt_LEAVE_DT_S').val());
                var Edate = new Date($('#txt_LEAVE_DT_E').val());
                if (Edate < Sdate)
                    message += message == "" ? "生效日期(迄)必須大於或等於生效日期(起)" : "\r\n生效日期(迄)必須大於或等於生效日期(起)";
            }
            if (message != "") {
                alert(message);
                return false
            }
            else
                return true;

        }
        //清空
        function doClear(ddl1, ddl2) {
            $("#" + ddl1).val("");
            $("#" + ddl2).val("");
            $("#txt_SHIFT_CD").val("");
            $("#txt_SHIFT_DESC").val("");
            $("#txt_LEAVE_DT_S").val("");
            $("#txt_LEAVE_DT_E").val("");
            $("#ddl_DUTY_TIME_HH").val("");
            $("#ddl_DUTY_TIME_MM").val("");
            $("#ddl_EAT_TIME_HH").val("");
            $("#ddl_EAT_TIME_MM").val("");
            $("#ddl_REST_TIME_HH").val("");
            $("#ddl_REST_TIME_MM").val("");
            $("#ddl_IS_IFLOW_SHOW").val("");
            $("#rb_VALID_ALL").prop('checked', true);

            //$("#uc_WORK_SHIFT_ALLOWANCE_TYPE").val("");
            //return false;
        }
        function checkdate(input) {
            var validformat = /^\d{4}\/\d{2}\/\d{2}$/
            var returnval = false
            if (!validformat.test(input))
                returnval = false;
            else if (parseInt(input.split("/")[0], 10) < 1910) {
                returnval = false;
            }
            else {
                var yearfield = input.split("/")[0]
                var monthfield = input.split("/")[1]
                var dayfield = input.split("/")[2]
                var dayobj = new Date(yearfield, monthfield - 1, dayfield)
                if ((dayobj.getMonth() + 1 != monthfield) || (dayobj.getDate() != dayfield) || (dayobj.getFullYear() != yearfield))
                    returnval = false;
                else
                    returnval = true
            }
            return returnval
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table width="100%">
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_SHIFT_TIME" Text="<%$Resources:Resource,wf2da_lbSHIFT_TIME%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <uc1:UCCommCodeDropDwonList runat="server"
                                        ID="uc_SHIFT_TIME"
                                        DataTextField="SUB_CD,SUB_DESC"
                                        DataValueField="SUB_CD"
                                        SYS_CDs="DA"
                                        MAIN_CDs="SHIFT_TIME_CD"
                                        IS_VALID="True"
                                        DataTextFormatString="{0} - {1}"
                                        OrderSeq="ASC"
                                        FirstItem="<%$Resources:Resource,wfb2da_uc_SHIFT_TIME_PlaceChoice%>"
                                        ClientIDMode="AutoID" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_SHIFT_CD" Text="<%$Resources:Resource,wf2da_lbSHIFT_CD%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_SHIFT_CD" runat="server" ClientIDMode="Static" MaxLength="2" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_SHIFT_DESC" Text="<%$Resources:Resource,wf2da_SHIFT_DESC%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_SHIFT_DESC" ClientIDMode="Static" MaxLength="60" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_VALID_DATE" Text="<%$Resources:Resource,wf2da_VALID_DATE%>" ClientIDMode="Static" />
                                </th>
                                <td colspan="5">
                                    <uc1:UCDateTimeRange runat="server" ID="uc_VALID_DATE" ClientIDMode="Static" ControlClientIDMode="Static" EndDateCssClass="date" StartDateCssClass="date" />
                                    <asp:RadioButton ID="rb_VALID_Y" runat="server" ClientIDMode="Static" Text="<%$Resources:Resource,wf2da_rbVALID_Y%>" GroupName="VALID" />
                                    <asp:RadioButton ID="rb_VALID_N" runat="server" ClientIDMode="Static" Text="<%$Resources:Resource,wf2da_rbVALID_N%>" GroupName="VALID" />
                                    <asp:RadioButton ID="rb_VALID_ALL" runat="server" ClientIDMode="Static" Text="<%$Resources:Resource,wf2da_rbVALID_ALL%>" Checked="true" GroupName="VALID" />
                                    <asp:HiddenField ID="hid_VALID" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_DUTY_TIME" Text="<%$Resources:Resource,wfb2da_lb_DUTY_TIME%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:DropDownList ID="ddl_DUTY_TIME_HH" runat="server" ClientIDMode="Static" />
                                    <asp:Label runat="server" ID="lb_DUTY_TIME_Split" Text=":" />
                                    <asp:DropDownList ID="ddl_DUTY_TIME_MM" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hid_DUTY_TIME" runat="server" ClientIDMode="Static" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_EAT_TIME" Text="<%$Resources:Resource,wfb2da_lb_EAT_TIME%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:DropDownList ID="ddl_EAT_TIME_HH" runat="server" ClientIDMode="Static" />
                                    <asp:Label runat="server" ID="lb_EAT_TIME_Split" Text=":" />
                                    <asp:DropDownList ID="ddl_EAT_TIME_MM" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hid_EAT_TIME" runat="server" ClientIDMode="Static" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_REST_TIME" Text="<%$Resources:Resource,wfb2da_lb_REST_TIME%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:DropDownList ID="ddl_REST_TIME_HH" runat="server" ClientIDMode="Static" />
                                    <asp:Label runat="server" ID="lb_REST_TIME_Split" Text=":" />
                                    <asp:DropDownList ID="ddl_REST_TIME_MM" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hid_REST_TIME" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_WORK_SHIFT_ALLOWANCE_TYPE" runat="server" ClientIDMode="Static" Text="<%$Resources:Resource,wf2da_WORK_SHIFT_ALLOWANCE_TYPE%>" />
                                </th>
                                <td>
                                    <uc1:UCCommCodeDropDwonList runat="server"
                                        ID="uc_WORK_SHIFT_ALLOWANCE_TYPE"
                                        DataTextField="SUB_CD,SUB_DESC"
                                        DataValueField="SUB_CD"
                                        SYS_CDs="SC"
                                        MAIN_CDs="WORK_SHIFT_ALLOWANCE_TYPE"
                                        IS_VALID="True"
                                        DataTextFormatString="{0} - {1}"
                                        OrderSeq="ASC"
                                        FirstItem="<%$Resources:Resource,wfb2da_uc_WORK_SHIFT_ALLOWANCE_TYPE_PlaceChoice%>"
                                        ClientIDMode="AutoID" />
                                </td>
                                <%-- IFLOW顯示否 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="Label1" Text="<%$Resources:Resource,wfb2db_lb_IS_IFLOW_SHOW%>" />
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddl_IS_IFLOW_SHOW" ClientIDMode="Static" Width="60px">
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>" Value="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>" />
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_IS_IFLOW_SHOW_Y%>" Value="Y" />
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_IS_IFLOW_SHOW_N%>" Value="N" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" align="right" class="Body_label">
                                    <div id="init">
                                        <aces:Btn ID="WFB2DA0200Search" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0200Search%>" OnClick="WFB2DA0200Search_Click" OnClientClick="return CheckInput();" />

                                        <%--<asp:Button ID="WFB2DA0200Search" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0200Search%>" OnClick="WFB2DA0200Search_Click" OnClientClick="return CheckInput();" />--%>
                                        <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2da_Clear%>" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr style="width: 1020px; text-align: left;" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Body_label">
                        <div id="init_grid" style="margin-bottom: 7px;">
                            <aces:Btn ID="WFB2DA0200Add" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Add%>" OnClick="WFB2DA0200Add_Click" />
                            <aces:Btn ID="WFB2DA0200Delete" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0200Delete%>" Visible="false" OnClientClick="return CheckDelAction();" OnClick="WFB2DA0200Delete_Click" />
                            <aces:Btn ID="WFB2DA0200Edit" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0200Edit%>" Visible="false" OnClick="WFB2DA0200Edit_Click" OnClientClick="return CheckModeifyAction();" />
                            <aces:Btn ID="WFB2DA0200Replace" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0200Replace%>" Visible="false" OnClick="WFB2DA0200Replace_Click" OnClientClick="return CheckModeifyAction();" />
                            <aces:Btn ID="WFB2DA0200UnVALID" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0200UnVALID%>" Visible="false" OnClientClick="return CheckUnValidAction();" OnClick="WFB2DA0200UnVALID_Click" />

                            <%--<asp:Button ID="WFB2DA0200Add" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Add%>" OnClick="WFB2DA0200Add_Click" />
                            <asp:Button ID="WFB2DA0200Delete" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0200Delete%>" Visible="false" OnClientClick="return CheckDelAction();" OnClick="WFB2DA0200Delete_Click" />
                            <asp:Button ID="WFB2DA0200Edit" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0200Edit%>" Visible="false" OnClick="WFB2DA0200Edit_Click" OnClientClick="return CheckModeifyAction();" />
                            <asp:Button ID="WFB2DA0200UnVALID" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0200UnVALID%>" Visible="false" OnClientClick="return CheckUnValidAction();" OnClick="WFB2DA0200UnVALID_Click" />--%>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getGridData"
                SelectCountMethod="GetGridDataCount" TypeName="WFB2DA0200BO" EnablePaging="True" SortParameterName="sortExpression"
                OnSelected="ods1_Selected" OnSelecting="obs1_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="uc_SHIFT_TIME"
                        Name="SHIFT_TIME_CD" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_SHIFT_CD"
                        Name="SHIFT_CD" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_SHIFT_DESC"
                        Name="SHIFT_DESC" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_VALID"
                        Name="VALID" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_DUTY_TIME"
                        Name="DUTY_TIME" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_EAT_TIME"
                        Name="EAT_TIME" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_REST_TIME"
                        Name="REST_TIME" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="uc_VALID_DATE"
                        Name="START_DT" PropertyName="StartDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="uc_VALID_DATE"
                        Name="END_DT" PropertyName="EndDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="uc_WORK_SHIFT_ALLOWANCE_TYPE"
                        Name="WORK_SHIFT_ALLOWANCE_TYPE" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_IS_IFLOW_SHOW"
                        Name="IS_IFLOW_SHOW" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1700px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound" meta:resourcekey="gv_resultResource1">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2da_RowNumber%>" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="RowNumber" Text='<%#Bind("RowNumber")%>' runat="server" Width="97%" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="RowNumber_Edit" Text='<%#Bind("RowNumber")%>' runat="server" Width="97%" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- 時段別區分--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wf2da_lbSHIFT_TIME%>" SortExpression="SHIFT_TIME_DESC" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblSHIFT_TIME_DESC" Text='<%#Bind("SHIFT_TIME_DESC")%>' runat="server" Width="97%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%-- 班別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wf2da_lbSHIFT_DESC_Grid%>" SortExpression="SHIFT_CD" HeaderStyle-Width="260px" ItemStyle-Width="260px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblSHIFT_CD" runat="server" Width="97%" />
                            <asp:HiddenField ID="hidSHIFT_CD" runat="server" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2da_lb_START_DT%>" SortExpression="START_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_DT" runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2da_lb_END_DT%>" SortExpression="END_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_DT" runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wf2da_lbDUTY_STIME_Range%>" SortExpression="DUTY_STIME" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblDUTY_Range" runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wf2da_lbMealTime1%>" SortExpression="MealTime1S" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblMealTime1" runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wf2da_lbMealTimeRest%>" SortExpression="MealTime1ResetS" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblMealTime1Reset" runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wf2da_lbMealTime2%>" SortExpression="MealTime2S" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbMealTime2" runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wf2da_lbMealTime2Reset1%>" SortExpression="MealTime2ResetS1" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbMealTime2Reset1" runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wf2da_lbMealTime2Reset2%>" SortExpression="MealTime2ResetS2" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbMealTime2Reset2" runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wf2da_lbMealTime2Reset3%>" SortExpression="MealTime2ResetS3" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbMealTime2Reset3" runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wf2da_lbMealTime3%>" SortExpression="MealTime3S" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbMealTime3" runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wf2da_lbMealTime3Reset1%>" SortExpression="MealTime3ResetS1" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbMealTime3Reset1" runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wf2da_lbMealTime3Reset2%>" SortExpression="MealTime3ResetS2" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lbMealTime3Reset2" runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2da_lb_WORK_SHIFT_ALLOWANCE_TYPE%>" SortExpression="WORK_SHIFT_ALLOWANCE_TYPE_CD" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_SHIFT_ALLOWANCE_TYPE" runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--IFLOW顯示 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_IS_IFLOW_SHOW%>" SortExpression="IS_IFLOW_SHOW" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_IFLOW_SHOW" runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
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
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DA0200Search"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2DA0200Edit"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2DA0200UnVALID"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2DA0200Delete"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btn_clear"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Del_NotChoiceMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Mod_NotChoiceMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Del_ConfirmMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_UnVALID_NotChoiceMessage" />
</asp:Content>

