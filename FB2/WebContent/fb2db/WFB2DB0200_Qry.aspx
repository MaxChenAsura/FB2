<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2db/WFB2DB0200_Qry.aspx.cs" Inherits="WebContent_fb2db_WFB2DB0200_Qry" Culture="auto" UICulture="auto" %>

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
            $('.ym').mask('9999/99/99');
            gridviewScroll();
            $.unblockUI();
            $("#batchEditTable").hide();
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


        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1) {
                return true;
            }
            else {
                alert($('#hidwfb2db_Mod_NotChoiceMessage').val());
                $.unblockUI();
                return false;
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].id.search("cb_check") > -1 && ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
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

        function SearchValid(StartDateControl_CALENDAR, EndDateContrl_CALENDAR, StartDateControl_JOIN, EndDateContrl_JOIN) {
            var message = "";
            //UCD_CALENDAR_DT
            if ($('#' + StartDateControl_CALENDAR).val() != "" && !checkdate($('#' + StartDateControl_CALENDAR).val()))
                message += $('#hid_Date_FormatError').val() + "\r\n";
            if ($('#' + EndDateContrl_CALENDAR).val() != "" && !checkdate($('#' + EndDateContrl_CALENDAR).val()))
                message += $('#hid_Date_FormatError').val() + "\r\n";
            if ($('#' + StartDateControl_CALENDAR).val() != "" && $('#' + EndDateContrl_CALENDAR).val() != "" &&
                Date.parse($('#' + StartDateControl_CALENDAR).val()).valueOf() > Date.parse($('#' + EndDateContrl_CALENDAR).val()).valueOf())
                message += $('#hid_wfb2db_greaterthan_BONUS_SDT').val() + "\r\n";
            if ($('#' + StartDateControl_CALENDAR).val() == "" || $('#' + EndDateContrl_CALENDAR).val() == "")
                message += $('#hid_wfb2db_Required_CALENDAR').val() + "\r\n";  //勤務日期區間不可空白
            //UC_JOIN_DT
            if ($('#' + StartDateControl_JOIN).val() != "" && !checkdate($('#' + StartDateControl_JOIN).val()))
                message += $('#hid_Date_FormatError_JOIN').val() + "\r\n";
            if ($('#' + EndDateContrl_JOIN).val() != "" && !checkdate($('#' + EndDateContrl_JOIN).val()))
                message += $('#hid_Date_FormatError_JOIN').val() + "\r\n";
            if ($('#' + StartDateControl_JOIN).val() != "" && $('#' + EndDateContrl_JOIN).val() != "" &&
                Date.parse($('#' + StartDateControl_JOIN).val()).valueOf() > Date.parse($('#' + EndDateContrl_JOIN).val()).valueOf())
                message += $('#hid_wfb2db_greaterthan_BONUS_SDT').val() + "\r\n";

            if (message != "") {
                alert(message);
                return false;
            }
            else {
                BlockUI();
                return true;
            }

        }
        //清空
        function doClear() {
            $("#ContentPlaceHolder1_UCD_CALENDAR_DT_txt_LEAVE_DT_S").val("");
            $("#ContentPlaceHolder1_UCD_CALENDAR_DT_txt_LEAVE_DT_E").val("");
            $("#ContentPlaceHolder1_UC_PLANT_CD_ddlCommCode").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_DESC").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_DESC").val("");
            $("#txt_WORK_SHIFT_CD").val("");
            $("#txt_WORK_SHIFT_DESC").val("");
            $("#ContentPlaceHolder1_UC_JOIN_DT_txt_LEAVE_DT_S").val("");
            $("#ContentPlaceHolder1_UC_JOIN_DT_txt_LEAVE_DT_E").val("");
            $("#ddl_WORK_DAY_CD").val("-1");
            return false;
        }
        //一括檢查
        function doUpdate() {
            if (LookUpCheckboxs() > 0) {
                $("#batchEditTable").toggle();
            }
            else {
                alert("請選擇資料");
            }
        }

        //一括更新的檢查
        function checkConfirm() {
            var processed = false;
            if (LookUpCheckboxs() == 0) {
                alert("請選擇資料");
                return false;
            }
            var result = confirm("確定要更新?");
            if (result) {
                BlockUI();
                processed=true;
            }
            if (!processed)
                $.unblockUI();

            return processed;
            /*
            if (Page_ClientValidate("GroupB")) {
                var result = confirm("確定要更新?");
                if (result) {
                    BlockUI();
                    return true;
                }
                else {
                    $("#allUpdate1").hide();
                    $("#allUpdate2").hide();
                    $.unblockUI();
                    return false;
                }
            }
            else
                processed = false;
            */


        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
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
                                <%-- 勤務日期 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_CALENDAR_DT" Text="<%$Resources:Resource,wfb2db_lb_CALENDAR_DT%>" />
                                </th>
                                <td colspan="1">
                                    <uc1:UCDateTimeRange runat="server" ID="UCD_CALENDAR_DT" StartDateCssClass="MandatoryField date ym" EndDateCssClass="MandatoryField date ym" />
                                </td>
                                <%-- 班別 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_S_CD" Text="<%$Resources:Resource,wfb2db_SHIFT_CD%>" />
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_SHIFT_CD" ClientIDMode="Static" Width="80px"  MaxLength="2" />
                                </td>
                            </tr>
                            <tr>
                                <%-- 工廠區分 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_PLANT_CD" Text="<%$Resources:Resource,wfb2db_lb_PLANT_CD%>" />
                                </th>
                                <td>
                                    <uc1:UCCommCodeDropDwonList
                                        runat="server"
                                        ID="UC_PLANT_CD"
                                        MAIN_CDs="PLANT_CD"
                                        IS_VALID="True"
                                        OrderSeq="ASC"
                                        DataValueField="SUB_CD"
                                        DataTextField="SUB_CD,SUB_DESC"
                                        DataTextFormatString="{0}-{1}"
                                        FirstItem="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_DEPT_NO" Text="<%$Resources:Resource,wfb2db_DEPT_NO%>" />
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_DEPT_NO" ClientIDMode="Static" Width="80px" OnTextChanged="txt_DEPT_NO_TextChanged" AutoPostBack="true" MaxLength="7" />
                                    <asp:Button runat="server" ID="btn_DEPT_NO" Text="..." ClientIDMode="Static" />
                                    <asp:TextBox ID="txt_DEPT_DESC" runat="server" Width="250px" BorderWidth="0" Enabled="false" ClientIDMode="Static" Style="background-color: white; color: black;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <%-- 工號 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_EMP_ID" Text="<%$Resources:Resource,wfb2db_EMP_ID%>" />
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_EMP_ID" ClientIDMode="Static" Width="80px" OnTextChanged="txt_EMP_ID_TextChanged" AutoPostBack="true" MaxLength="5" />
                                    <asp:Button runat="server" ID="btn_EMP_ID" Text="..." ClientIDMode="Static" />
                                    <asp:TextBox runat="server" ID="txt_EMP_DESC" BorderWidth="0" Enabled="false" ClientIDMode="Static" Style="background-color: white; color: black;" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_WORK_SHIFT_CD" Text="<%$Resources:Resource,wfb2db_WORK_SHIFT_CD%>" />
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_WORK_SHIFT_CD" ClientIDMode="Static" Width="80px" MaxLength="3" OnTextChanged="txt_WORK_SHIFT_CD_TextChanged" AutoPostBack="true" />
                                    <asp:Button runat="server" ID="btn_WORK_SHIFT_CD" Text="..." ClientIDMode="Static" />
                                    <asp:TextBox runat="server" ID="txt_WORK_SHIFT_DESC" Width="250px" BorderWidth="0" Enabled="false" ClientIDMode="Static" Style="background-color: white; color: black;" />
                                </td>
                            </tr>
                            <tr>
                                <%-- 入社日期 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_JOIN_DT" Text="<%$Resources:Resource,wfb2db_JOIN_DT%>" />
                                </th>
                                <td>
                                    <uc1:UCDateTimeRange runat="server" ID="UC_JOIN_DT" EndDateCssClass="date ym" StartDateCssClass="date ym" />
                                </td>
                                <%--出勤別--%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_WORK_DAY_CD" runat="server" Text="<%$Resources:Resource,wfb2db_lb_WORK_DAY_CD%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddl_WORK_DAY_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <th></th>
                                <th></th>
                                <td align="right" class="Body_label">
                                    <aces:Btn ID="WFB2DB0200Search" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0200Search%>" OnClick="WFB2DB0200Search_Click" />
                                    <%--<asp:Button ID="WFB2DB0200Search" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0200Search%>" OnClick="WFB2DB0200Search_Click" />--%>
                                    <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2db_Clear%>" OnClientClick="return doClear();" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table id="batchEditTable">
                                        <tr>
                                            <%-- 班別 --%>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_SHIFT_CD" runat="server" Text='<%$Resources:Resource,wfb2db_SHIFT_CD%>' ClientIDMode="Static" />:
                                            </th>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_SHIFT_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                <%-- 確認 --%>
                                                <asp:Button ID="bt_BatchEdit" runat="server" Text="<%$Resources:Resource,wfb2di_bt_BatchEdit%>" OnClientClick="return checkConfirm();" OnClick="bt_BatchEdit_Click" />
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                                <td align="right" class="Body_label" colspan="2">
                                    <aces:Btn ID="WFB2DB0200Edit" runat="server" Text="修改" Visible="false" OnClick="WFB2DB0200Edit_Click" OnClientClick="return CheckModeifyAction();" />
                                    <%-- 一括更新 --%>
                                     <aces:Btn ID="WFB2DB0200BatchEdit" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0200BatchEdit%>" Visible="false" OnClick="WFB2DB0200BatchEdit_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getGridData"
                SelectCountMethod="GetGridDataCount" TypeName="WFB2DB0200BO" EnablePaging="True" SortParameterName="sortExpression"
                OnSelected="ods1_Selected" OnSelecting="obs1_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="UCD_CALENDAR_DT"
                        Name="CALENDAR_DT_Start" PropertyName="StartDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="UCD_CALENDAR_DT"
                        Name="CALENDAR_DT_End" PropertyName="EndDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="UC_PLANT_CD"
                        Name="PLANT_CD" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="DEPT_NO" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="EMP_ID" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="UC_JOIN_DT"
                        Name="JOIN_DT_Start" PropertyName="StartDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="UC_JOIN_DT"
                        Name="JOIN_DT_End" PropertyName="EndDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_WORK_SHIFT_CD"
                        Name="WORK_SHIFT_CD" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="HID_DEPTAuth"
                        Name="DEPTAuth" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hidIsDEPT"
                        Name="IsDEPT" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hidsp_dept"
                        Name="sp_dept" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_WORK_DAY_CD"
                        Name="work_day_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                     <asp:ControlParameter ControlID="txt_SHIFT_CD"
                        Name="shift_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="false" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1150px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound" meta:resourcekey="gv_resultResource1">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="RowNumber" Text='<%#Bind("RowNumber")%>' runat="server" Width="97%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 工廠區分--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_PLANT_CD%>" SortExpression="CALENDAR" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblPLANT_CD" Text='<%#Bind("PLANT")%>' runat="server" Width="97%" />
                            <asp:HiddenField ID="hidPLANT_CD" Value='<%#Bind("PLANT_CD")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 部門--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="270px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblDEPT_NAME" Text='<%#Bind("DEPT_NAME")%>' runat="server" Width="97%" />
                            <asp:HiddenField ID="hidDEPT_NO" Value='<%#Bind("DEPT_NO")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%-- 工數區分--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_WORK_CD%>" SortExpression="WORK_CD" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_CD" Text='<%#Bind("WORK_CD_DESC")%>' runat="server" Width="97%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblEMP_ID" Text='<%#Bind("EMP_ID")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblEMP_NAME" Text='<%#Bind("EMP_NAME")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 勤務日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_CALENDAR_DT%>" SortExpression="CALENDAR_DT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblCALENDAR_DT" Text='<%#Bind("CALENDAR_DT")%>' runat="server" Width="97%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 出勤別
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_WORK_DAY%>" SortExpression="WORK_DAY_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblWORK_DAY" Text='<%#Bind("WORK_DAY")%>' runat="server" Width="97%" />
                            <asp:HiddenField ID="hidWORK_DAY_CD" Value='<%#Bind("WORK_DAY_CD")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                        --%>
                    <%-- 日期類型--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_DT_TYPE%>" SortExpression="DT_TYPE" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblDT_TYPE" Text='<%#Bind("DT_TYPE_DESC")%>' runat="server" Width="97%" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 班別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_SHIFT_CD%>" SortExpression="SHIFT_CD" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblSHIFT" Text='<%#Bind("SHIFT")%>' runat="server" Width="97%" />
                            <asp:HiddenField ID="hidSHIFT_CD" Value='<%#Bind("SHIFT_CD")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 上班時段--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_DUTY_STIME%>" SortExpression="DUTY_STIME" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblDUTY_TIME" runat="server" Width="97%" />
                             <asp:HiddenField ID="hid_WORK_SHIFT_CD" runat="server" Value='<%#Bind("WORK_SHIFT_CD")%>' ClientIDMode="Static" /> 
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
            <asp:HiddenField ID="HID_DEPTAuth" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_Mod_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidIsDEPT" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidsp_dept" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Date_FormatError" Value="<%$Resources:Resource,wfb2db_CLENDAR_DT_FormatErr%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Date_FormatError_JOIN" Value="<%$Resources:Resource,wfb2db_JOIN_DT_FormatErr%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_wfb2db_greaterthan_BONUS_SDT" Value="<%$Resources:Resource,wfb2db_greaterthan_BONUS_SDT%>" />
            <%--勤務日期區間不可空白--%>
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_wfb2db_Required_CALENDAR" Value="<%$Resources:Resource,wfb2db_Required_CALENDAR%>" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DB0200Search"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2DB0200Edit"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>


</asp:Content>
