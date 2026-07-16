<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2db/WFB2DB0300_Grant.aspx.cs" MasterPageFile="~/MasterPage.master" Inherits="WebContent_fb2db_WFB2DB0300_Grant" %>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Src="~/UserControl/UCCommCodeDropDwonList.ascx" TagPrefix="uc1" TagName="UCCommCodeDropDwonList" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            gridviewScroll();
            if($("#aa").val()!="Y")
                $.unblockUI();
            $("#txt_DEPT_DESC").attr("readonly", true);
            $("#txt_EMP_DESC").attr("readonly", true);
            $("#txt_WORK_SHIFT_DESC").attr("readonly", true);
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }
        function CheckDateRange(startdtid, enddtid) {
            //BlockUI();
            var message = "";
            if ($('#' + startdtid).val() != "" && checkdate($('#' + startdtid).val()) == false)
                message += "入社日期區間(起)格式錯誤";
            if ($('#' + enddtid).val() != "" && checkdate($('#' + enddtid).val()) == false) {
                if (message == "")
                    message += "入社日期區間(迄)格式錯誤";
                else
                    message += "\r\n入社日期區間(迄)格式錯誤";
            }
            if ($('#' + startdtid).val() != "" && $('#' + enddtid).val() != "" && message == "") {
                var Sdate = new Date($('#' + startdtid).val());
                var Edate = new Date($('#' + enddtid).val());
                if (Sdate > Edate)
                    message += "入社日期區間(起)不能小於入社日期區間(迄)";
            }
            if (message == "")
                return true;
            else {
                alert(message);
                //$.unblockUI();
                return false;
            }
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "990",
                height: "400",
                barcolor: "#7F7F7F"
            });
        }

        function OpenGridEmpSearch(IDControl, DescContorl, SupperUser) {
            OpenEmpSearch(IDControl, DescContorl, SupperUser,'Y');
            //var obj = OpenEmpSearch(IDControl, DescContorl, SupperUser);
            //$("#" + DescContorl).html(obj.EMP_NAME);
            //$("#txt_PLANT_DESC_Edit").html(obj.PLANT_NAME);
            //$("#lb_DEPT_FULL_NAME_Edit").html(obj.DEPT_NAME);
        }
        function returnEMPValueToPage(eid, ename, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            //alert(returnValue);
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $('#' + eid).val(obj.EMP_ID.trim());
                $("#" + ename).html(obj.EMP_NAME.trim());
                $("#txt_PLANT_DESC_Edit").html(obj.PLANT_NAME);
                $("#lb_DEPT_FULL_NAME_Edit").html(obj.DEPT_NAME);

            }
        }
        function Valid() {
            if ($("#txt_EMP_ID_Edit").val() == "") {
                alert($("#hid_EMP_ID_NotNull").val());
                return false;
            }
            else {
                if (confirm($('#hidwfb2db_Save_ConfirmMessage').val())) {
                    BlockUI();
                    return;
                }
                else {
                    $.unblockUI();
                    return false;
                }
            }
        }

        function GrenatValid(StartDateControl, EndDateContrl) {
            var message = "";
            if ($('#' + StartDateControl).val() == "")
                message += $('#hidwfb2db_CALENDAR_DT_S_NotNull').val() + "\r\n";
            if ($('#' + EndDateContrl).val() == "")
                message += $('#hidwfb2db_CALENDAR_DT_E_NotNull').val() + "\r\n";

            if (!checkdate($('#' + StartDateControl).val()) || !checkdate($('#' + EndDateContrl).val()))
                message += $('#hid_Date_FormatError').val() + "\r\n";

            if (Date.parse($('#' + StartDateControl).val()).valueOf() > Date.parse($('#' + EndDateContrl).val()).valueOf())
                message += $('#hid_wfb2db_greaterthan_BONUS_SDT').val() + "\r\n";
            if (message != "") {
                alert(message);
                return false;
            }
            else {
                BlockUI();
                if (!confirm('確定要產生?')) {
                    $.unblockUI();
                    return false;
                }
                else
                    return true;
            }

        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0) {
                BlockUI();
                if (confirm($('#hidwfb2db_Del_ConfirmMessage').val())) {
                    BlockUI();
                    return;
                }
                else {
                    $.unblockUI();
                    return false;
                }
            }
            else {
                alert($('#hidwfb2db_Del_NotChoiceMessage').val());
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
        function CancelConfirm() {
            if (confirm($('#hidwfb2db_Cancel_Confirm').val())) {
                BlockUI();
                return;
            }
            else {
                $.unblockUI();
                return false;
            }
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
        //清空
        function doClear() {
            $("#ContentPlaceHolder1_UC_PLANT_CD_ddlCommCode").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_DESC").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_DESC").val("");
            $("#txt_WORK_SHIFT_CD").val("");
            $("#txt_WORK_SHIFT_DESC").val("");
            $("#ContentPlaceHolder1_UC_JOIN_DT_txt_LEAVE_DT_S").val("");
            $("#ContentPlaceHolder1_UC_JOIN_DT_txt_LEAVE_DT_E").val("");
            return false;
        }
        function doClear_grant() {
            $("#ContentPlaceHolder1_UC_CALENDAR_DT_txt_LEAVE_DT_S").val("");
            $("#ContentPlaceHolder1_UC_CALENDAR_DT_txt_LEAVE_DT_E").val("");
            return false;
        }
        //todo
        function block_grant(msg) {
            BlockUI();
            //$.blockUI();
            //$("#aa").val("Y");
            if (confirm(msg)) {
                $('#btn_Grant_Confim_later').click();
            }
            else
                $.unblockUI();
        }
    </script>
    <style type="text/css">
        fieldset {
            border: 2px solid green;
            border-color: red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF">
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_CALENDAR_DT" Text="<%$Resources:Resource,wfb2db_CALENDAR_DT%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <uc1:UCDateTimeRange runat="server" StartDateCssClass="MandatoryField date" EndDateCssClass="MandatoryField date" ID="UC_CALENDAR_DT" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_Type" Text="<%$Resources:Resource,wfb2db_Type%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:RadioButtonList ID="rad_Type" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rad_Type_SelectedIndexChanged">
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_ALL%>" Value="ALL" Selected="True" />
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_SOME%>" Value="SOME" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Panel GroupingText="<%$Resources:Resource,wfb2db_SOME%>" runat="server" ID="Panel" Width="1020px" Visible="false">
                <table width="100%">
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label runat="server" ID="lb_PLANT_CD" Text="<%$Resources:Resource,wfb2db_lb_PLANT_CD%>" ClientIDMode="Static" />
                        </th>
                        <td>
                            <uc1:UCCommCodeDropDwonList
                                runat="server"
                                ID="UC_PLANT_CD"
                                SYS_CDs="HB"
                                MAIN_CDs="PLANT_CD"
                                IS_VALID="True"
                                DataTextField="SUB_CD,SUB_DESC"
                                DataTextFormatString="{0}-{1}"
                                DataValueField="SUB_CD"
                                FirstItem="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>" />
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label runat="server" ID="lb_DEPT_NO" Text="<%$Resources:Resource,wfb2db_DEPT_NO%>" ClientIDMode="Static" />
                        </th>
                        <td>
                            <asp:TextBox ID="txt_DEPT_NO" runat="server" ClientIDMode="Static" Width="80px" OnTextChanged="txt_DEPT_NO_TextChanged" AutoPostBack="true" MaxLength="7" />
                            <asp:Button ID="btn_DEPT_NO" runat="server" Text="..." />
                            <asp:TextBox ID="txt_DEPT_DESC" runat="server" ClientIDMode="Static" BorderWidth="0" />
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label runat="server" ID="lb_EMP_ID" Text="<%$Resources:Resource,wfb2db_EMP_ID%>" ClientIDMode="Static" />
                        </th>
                        <td>
                            <asp:TextBox ID="txt_EMP_ID" runat="server" ClientIDMode="Static" Width="80px" OnTextChanged="txt_EMP_ID_TextChanged" AutoPostBack="true" MaxLength="5" />
                            <asp:Button ID="btn_EMP_ID" runat="server" Text="..." />
                            <asp:TextBox ID="txt_EMP_DESC" runat="server" ClientIDMode="Static" BorderWidth="0" />
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label runat="server" ID="lb_WORK_SHIFT_CD" Text="<%$Resources:Resource,wfb2db_lb_WORK_SHIFT%>" ClientIDMode="Static" />
                        </th>
                        <td>
                            <asp:TextBox ID="txt_WORK_SHIFT_CD" runat="server" ClientIDMode="Static" Width="80px" OnTextChanged="txt_WORK_SHIFT_CD_TextChanged" AutoPostBack="true" MaxLength="3" />
                            <asp:Button ID="btn_WORK_SHIFT_CD" runat="server" Text="..." />
                            <asp:TextBox ID="txt_WORK_SHIFT_DESC" runat="server" ClientIDMode="Static" Width="180px" BorderWidth="0" />
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label runat="server" ID="lb_JOIN_DT" Text="<%$Resources:Resource,wfb2db_JOIN_DT%>" ClientIDMode="Static" />
                        </th>
                        <td colspan="3">
                            <uc1:UCDateTimeRange runat="server" ID="UC_JOIN_DT" EndDateCssClass="date" StartDateCssClass="date" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="6">
                            <aces:Btn ID="WFB2DB0300Search" runat="server" Text="<%$Resources:Resource,wfb2db_Search%>" OnClick="btn_search_Click" />
                            <aces:Btn ID="WFB2DB0300Add" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0300Add%>" OnClick="WFB2DB0300Add_Click" />
                            <aces:Btn ID="WFB2DB0300Delete" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0300Delete%>" Visible="false" OnClientClick="return CheckDelAction();" OnClick="WFB2DB0100Delete_Click" />
                            <aces:Btn ID="WFB2DB0300Save" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0300Save%>" Visible="false" OnClick="WFB2DB0300Save_Click" OnClientClick="return Valid();" />


                            <%--<asp:Button ID="WFB2DB0300Search" runat="server" Text="<%$Resources:Resource,wfb2db_Search%>" OnClick="btn_search_Click" />--%>
                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2db_Clear%>" OnClientClick="return doClear();" />
                            
                           <%-- <asp:Button ID="WFB2DB0300Add" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0300Add%>" OnClick="WFB2DB0300Add_Click" />
                            <asp:Button ID="WFB2DB0300Delete" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0300Delete%>" Visible="false" OnClientClick="return CheckDelAction();" OnClick="WFB2DB0100Delete_Click" />
                            <asp:Button ID="WFB2DB0300Save" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0300Save%>" Visible="false" OnClick="WFB2DB0300Save_Click" OnClientClick="return Valid();" />
                            --%>
                            <asp:Button ID="WFB2DB0300Cancel" runat="server" Text="<%$Resources:Resource,wfb2db_Cancel%>" Visible="false" OnClick="WFB2DB0300Cancel_Click" OnClientClick="return CancelConfirm();" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                    AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                    OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="970px"
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
                        <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:Label ID="RowNumber" Text='<%#Bind("RowNumber")%>' runat="server" Width="97%" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <asp:Label ID="lb_EMP_ID" Text='<%#Bind("EMP_ID")%>' runat="server" Width="97%" ClientIDMode="Static" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_EMP_ID_Edit" CssClass="MandatoryField" runat="server" ClientIDMode="Static" Width="80px" OnTextChanged="txt_EMP_ID_Edit_TextChanged" AutoPostBack="true" MaxLength="5" />
                                    <asp:Button ID="btn_EMP_ID_Edit" Text="..." runat="server" ClientIDMode="Static" />
                                </div>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="150px">
                            <ItemTemplate>
                                <asp:Label ID="lb_EMP_NAME" Text='<%#Bind("EMP_NAME")%>' runat="server" Width="97%" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <div style="text-align: center; width: 100%">
                                    <asp:Label ID="lb_EMP_NAME_Edit" ClientIDMode="Static" runat="server" Width="97%" Text='<%#Bind("EMP_NAME")%>' />
                                </div>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_PLANT_CD%>" SortExpression="PLANT_CD" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <asp:Label ID="lb_PLANT" Text='<%#Bind("PLANT")%>' runat="server" />
                                <asp:HiddenField ID="hid_PLANT_CD" Value='<%#Bind("PLANT_CD")%>' runat="server" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <div style="text-align: center; width: 100%">
                                    <asp:Label ID="txt_PLANT_DESC_Edit" ClientIDMode="Static" runat="server" Width="97%" />
                                    <asp:HiddenField ID="hid_PLANT_CD" runat="server" />
                                </div>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="650px">
                            <ItemTemplate>
                                <asp:Label ID="lb_DEPT_FULL_NAME" Text='<%#Bind("DEPT_NAME")%>' runat="server" />
                                <asp:HiddenField ID="hid_DEPT_NO" Value='<%#Bind("DEPT_NO")%>' runat="server" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <div style="text-align: center; width: 100%">
                                    <asp:Label ID="lb_DEPT_FULL_NAME_Edit" runat="server" />
                                    <asp:HiddenField ID="hid_DEPT_NO" runat="server" />
                                </div>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="GridviewScrollPager" />
                    <FooterStyle CssClass="GridviewScrollPager" />
                    <EmptyDataTemplate>
                        <table class="grid-view" width="970" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                            <tr class="header">
                                <td width="20px"></td>
                                <td width="70px">
                                    <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb2db_RowNumber%>" Width="97%"></asp:Label>
                                </td>
                                <td width="180px">
                                    <asp:Label runat="server" ID="lblHeaderEMP_ID" Text="<%$Resources:Resource,wfb2db_EMP_ID%>" Width="97%" />
                                </td>
                                <td width="150px">
                                    <asp:Label ID="lblHeaderEMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2db_EMP_NAME%>" Width="97%"></asp:Label>
                                </td>
                                <td width="200px">
                                    <asp:Label ID="lblHeaderPLANT_CDC" runat="server" Text="<%$Resources:Resource,wfb2db_lb_PLANT_CD%>" Width="97%"></asp:Label>
                                </td>
                                <td width="650px">
                                    <asp:Label ID="lblHeaderDEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2db_DEPT_NO%>" Width="97%"></asp:Label>
                                </td>
                            </tr>
                            <tr class="normal">
                                <td></td>
                                <td></td>
                                <td>
                                    <div style="text-align: center; width: 100%">
                                        <asp:TextBox ID="txt_EMP_ID_Edit" CssClass="MandatoryField" runat="server" ClientIDMode="Static" Width="80px" OnTextChanged="txt_EMP_ID_Edit_TextChanged" AutoPostBack="true" MaxLength="5" />
                                        <asp:Button ID="btn_EMP_ID_Edit" Text="..." runat="server" ClientIDMode="Static" />
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center; width: 100%">
                                        <asp:Label ID="lb_EMP_NAME_Edit" ClientIDMode="Static" runat="server" Width="97%" Text='<%#Bind("EMP_NAME")%>' />
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center; width: 100%">
                                        <asp:Label ID="txt_PLANT_DESC_Edit" ClientIDMode="Static" runat="server" Width="97%" />
                                        <asp:HiddenField ID="hid_PLANT_CD" runat="server" />
                                    </div>
                                </td>
                                <td>
                                    <div style="text-align: center; width: 100%">
                                        <asp:Label ID="lb_DEPT_FULL_NAME_Edit" runat="server" />
                                        <asp:HiddenField ID="hid_DEPT_NO" runat="server" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:GridView>

                <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px;">
                    <tr height="100%" valign="top">
                        <td class="GridviewScrollPager TD">
                            <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true" Style="display: none;">
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
            </asp:Panel>
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF">
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td style="text-align: right;">
                                    <aces:Btn runat="server" ID="WFB2DB0300Grant" Text="<%$Resources:Resource,wfd2da_Grant%>" OnClick="btn_Grant_Click" ClientIDMode="Static" />

                                    <%--<asp:Button runat="server" ID="WFB2DB0300Grant" Text="<%$Resources:Resource,wfd2da_Grant%>" OnClick="btn_Grant_Click" ClientIDMode="Static" />--%>
                                    <asp:Button ID="btn_clear_grant" runat="server" Text="<%$Resources:Resource,wfb2db_Clear%>" OnClientClick="return doClear_grant();" />
                                    <asp:Button runat="server" ID="btn_Grant_Confim_later" OnClick="btn_Grant_Confim_later_Click" OnClientClick="BlockUI();" ClientIDMode="Static" Style="display: none" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DB0300Search" />
            <asp:PostBackTrigger ControlID="WFB2DB0300Add" />
            <asp:PostBackTrigger ControlID="WFB2DB0300Save" />
            <asp:PostBackTrigger ControlID="WFB2DB0300Delete" />
            <asp:PostBackTrigger ControlID="WFB2DB0300Cancel" />
            <asp:PostBackTrigger ControlID="btn_Grant_Confim_later" />
            <asp:PostBackTrigger ControlID="WFB2DB0300Grant" />
            <asp:PostBackTrigger ControlID="rad_Type" />
        </Triggers>
    </asp:UpdatePanel>
    <%--todo--%>
    <asp:HiddenField ID="aa" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="Hid_AddData" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hid_EMP_ID_NotNull" runat="server" Value="<%$Resources:Resource,wfb2db_EMP_ID_NotNull%>" ClientIDMode="Static" />
    <asp:HiddenField ID="hidwfb2db_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb2db_Save_ConfirmMessage%>" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hidwfb2db_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2db_Del_ConfirmMessage%>" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hidwfb2db_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb2_Del_NotChoiceMessage%>" runat="server" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_Cancel_Confirm" Value="<%$Resources:Resource,wfb2db_Cancel_Confirm%>" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_CALENDAR_DT_S_NotNull" Value="<%$Resources:Resource,wfb2db_CALENDAR_DT_S_NotNull%>" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_CALENDAR_DT_E_NotNull" Value="<%$Resources:Resource,wfb2db_CALENDAR_DT_E_NotNull%>" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="HiddenField1" Value="<%$Resources:Resource,wfb2db_CALENDAR_DT_E_NotNull%>" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Date_FormatError" Value="<%$Resources:Resource,wfb2db_CLENDAR_DT_FormatErr%>" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_wfb2db_greaterthan_BONUS_SDT" Value="<%$Resources:Resource,wfb2db_greaterthan_BONUS_SDT%>" />

</asp:Content>
