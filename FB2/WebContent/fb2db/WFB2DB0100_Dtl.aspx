<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2db/WFB2DB0100_Dtl.aspx.cs" Inherits="WebContent_fb2db_WFB2DB0100_Dtl" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCCommCodeDropDwonList.ascx" TagPrefix="uc1" TagName="UCCommCodeDropDwonList" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            $('#txtWORK_SHIFTYearMonth').datepicker({ dateFormat: 'yy/mm' });
            $("#txtWORK_SHIFTYearMonth").mask('9999/99');
            iniForm();
        });
        function iniForm() {
            $.unblockUI();
        }

        //找出班別的資料,DescControlId:班表說明的id
        function findWorkShitCd(TextControl, DescControlId) {
            if (TextControl.value.length < 2) {
                $('#' + DescControlId).val('');
                return;
            }
            var ShiftDesc = "";
            TextControl.value = TextControl.value.toUpperCase();
            var isIncloude = false;
            var shiftDesc = "";
            $.each(arrWorkShiftDt, function (index, ele) {
                if (TextControl.value == arrWorkShiftDt[index].SHIFT_CD) {
                    isIncloude = true;
                    shiftDesc = arrWorkShiftDt[index].SHIFT_DESC;
                    return false;//=break;
                }
            });
            if (isIncloude==false) {
                $('#' + DescControlId).val('');
                alert('找不到班別資料');
            } else {
                $('#' + DescControlId).val(shiftDesc);
            }
            /*
            var findValue = arrWorkShiftDt.filter(function (index) { return arrWorkShiftDt.SHIFT_CD == TextControl.value });
            if (findValue.length == 0) {
                TextControl.value = "";
                $('#' + DescControlId).val('');
                alert('找不到班別資料');
            }
            else
                $('#' + DescControlId).val(findValue[0].SHIFT_DESC);
            */
        }
        function txtWORK_SHIFTYearMonthCheck() {
            var message = "";
            if ($('#txtWORK_SHIFTYearMonth').val() != "" && !checkdate($('#txtWORK_SHIFTYearMonth').val() + "/01"))
                message += "輪值表年月格式錯誤";

            var TextBoxValue = $("#txtWORK_SHIFTYearMonth").val();
            if (TextBoxValue.length == 0) {
                message += message == "" ? $("#hidRequiretxtWORK_SHIFTYearMonthMessage").val() : "\r\n" + $("#hidRequiretxtWORK_SHIFTYearMonthMessage").val();
            }
            else {
                if (TextBoxValue.substr(4, 2) > 12 || TextBoxValue.substr(4, 2) == 0) {
                    message += message == "" ? $("#hidtxtWORK_SHIFTYearMonthFormatErrMessag").val() : "\r\n" + $("#hidtxtWORK_SHIFTYearMonthFormatErrMessag").val();
                }
            }
            if (message != "") {
                alert(message);
                return false;
            }
            else {
                return true;
                BlockUI();
            }
        }
        //檢查班別是否存在
        function hasShiftCD() {
            var day;
            var errMsg = "";
            $("[id^=txt_WORK_DAY_DESC]").each(function (index, ele) {
                day = index + 1;
                //console.log("第" + day + "天");
                if ($(this).val() == "") {
                    errMsg += "第" + day + "天 無班別資料\r\n"
                }
                //console.log($(this).val());
                //console.log($(ele).val());  //ele == this 表示p集合裡面的一個一個p
            });
            return errMsg;
            
        }

        //儲存時檢查
        function getWORK_SHIFTValues() {
            var errMsg = hasShiftCD();
            if (errMsg != "") {
                alert(errMsg);
                return false;
            }

            var processed = confirm($('#hidwfb2db_Save_ConfirmMessage').val());
            if (processed) {
                var WORK_DAY_CDs = $("[uikey*=WORK_DAY_CD]");
                var YearMonth = $("#txtWORK_SHIFTYearMonthWin").val();
                var JasonValue = "{\"Dtl\":[";
                for (var i = 0; i < WORK_DAY_CDs.length; i++) {
                    var workDay = WORK_DAY_CDs[i].id.replace("txt_WORK_DAY_CD", "");
                    var WORK_DAY_CD = WORK_DAY_CDs[i].value;
                    if (WORK_DAY_CD != $('#hid_WORK_DAY_CD' + workDay).val()) {
                        JasonValue += "{\"CALENDAR_DT\":\"" + YearMonth + "/" + (workDay < 10 ? "0" + workDay.toString() : workDay.toString()) + "\",\"" +
                                       "SHIFT_CD\":\"" + WORK_DAY_CD + "\"}";
                        if (i < WORK_DAY_CDs.length - 1)
                            JasonValue += ",";
                    }
                }
                JasonValue += "]}";
                $("#hidWORK_SHIFTKeepValue").val(JasonValue);
                BlockUI();
            } else {
                $.unblockUI();
                return false;
            }
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
        //清空
        function doClear() {
            var NowDateTime = new Date();
            var Year = NowDateTime.getFullYear().toString();
            var Month = NowDateTime.getMonth() + 1;
            var YearMonth = "";
            if (Month < 10)
                YearMonth = Year + "/0" + Month.toString();
            else
                YearMonth = Year + "/" + Month.toString();

            $('#txtWORK_SHIFTYearMonth').val(YearMonth); return false;
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
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF">
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <%-- 行事曆 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lblCALENDAR_CD" Text="<%$Resources:Resource,wfb2db_lb_CALENDAR_CD%>" ClientIDMode="Static" />
                                </th>
                                <td colspan="3">
                                    <asp:TextBox ID="txtCALENDAR_CD" runat="server" Enabled="false" Style="width: 50px;" ClientIDMode="Static" />
                                    <asp:TextBox ID="txtCALENDAR_DESC" runat="server" Enabled="false" Style="width: 200px;" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <%-- 輪值表代碼  --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lblWORK_SHIFT_CD" Text="<%$Resources:Resource,wfb2db_WORK_SHIFT_CD%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txtWORK_SHIFT_CD" runat="server" MaxLength="2" Enabled="false" Style="width: 50px;" ClientIDMode="Static" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lblWORK_SHIFT_DESC" Text="<%$Resources:Resource,wfb2db_lb_WORK_SHIFT_DESC%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txtWORK_SHIFT_DESC" runat="server" Enabled="false" Style="width: 200px;" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                 <%-- 輪值表年月  --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lblWORK_SHIFTearMonth" Text="<%$Resources:Resource,wfb2db_WORK_SHIFTYearMonth%>" ClientIDMode="Static" />
                                </th>
                                <td colspan="3">
                                    <asp:TextBox ID="txtWORK_SHIFTYearMonth" ClientIDMode="Static" runat="server" CssClass="MandatoryField" MaxLength="6" Style="width: 50px;" />
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <th></th>
                                <th></th>
                                <td align="right" class="Body_label">
                                    <div id="init">
                                        <aces:Btn ID="WFB2DB0102Search" runat="server" Text="<%$Resources:Resource,wfb2db_Search%>" OnClientClick="return txtWORK_SHIFTYearMonthCheck();" ClientIDMode="Static" ValidationGroup="GroupA" OnClick="btn_search_Click" />

                                        <%--<asp:Button ID="WFB2DB0102Search" runat="server" Text="<%$Resources:Resource,wfb2db_Search%>" OnClientClick="return txtWORK_SHIFTYearMonthCheck();" ClientIDMode="Static" ValidationGroup="GroupA" OnClick="btn_search_Click" />--%>
                                        <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2db_Clear%>" ClientIDMode="Static" OnClientClick="return doClear();" />
                                        <asp:Button ID="btn_Back" runat="server" Text="<%$Resources:Resource,wfb2db_Back%>" ClientIDMode="Static" OnClick="btn_Back_Click" />
                                        <%--OnClientClick="$(location).attr('href', 'WFB2DB0100_Qry.aspx'); return false;"--%>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <hr style="width: 1020px; text-align: left;" />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table style="margin-bottom: 10px;" width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#ffffff" height="100%">
                <tr>
                    <th style="text-align: left;">
                        <div runat="server" id="divTimeShift" style="width: 100%;">
                            <asp:Button runat="server" ID="btnPreviousMonth" Text="←" OnClick="btnPreviousMonth_Click" OnClientClick="BlockUI();" Style="height: 21px" ClientIDMode="Static" />
                            <asp:TextBox runat="server" ID="txtWORK_SHIFTYearMonthWin" CssClass="MandatoryField" Width="148px" ReadOnly="true" ClientIDMode="Static" />
                            <asp:Button runat="server" ID="btnNextMonth" Text="→" OnClick="btnNextMonth_Click" OnClientClick="BlockUI();" ClientIDMode="Static" />
                        </div>
                    </th>
                    <th style="text-align: right;">
                        <aces:Btn runat="server" ID="WFB2DB0102DtlMod" Visible="false" Text="<%$Resources:Resource,wfb2db_WFB2DB010Dtl_Mod%>" ClientIDMode="Static" OnClientClick="BlockUI();" OnClick="btn_Dtl_Mod_Click" />
                        <aces:Btn runat="server" ID="WFB2DB0102Grant" Text="<%$Resources:Resource,wfb2db_WFB2DB010Dtl_Grant%>" ClientIDMode="Static" OnClientClick="BlockUI();" OnClick="btn_Grant_Click" />
                        <aces:Btn runat="server" ID="WFB2DB0102Save" Visible="false" Text="<%$Resources:Resource,wfb2db_WFB2DB0100Save%>" OnClientClick="return getWORK_SHIFTValues();" ClientIDMode="Static" OnClick="btnSave_Click" />
                        <asp:Button runat="server" ID="btnCancle" Visible="false" Text="<%$Resources:Resource,wfb2db_Cancel%>" OnClientClick="return CancelConfirm()" ClientIDMode="Static" OnClick="btnCancle_Click" />
                    </th>
                </tr>
            </table>
            <table runat="server" id="tbWORK_SHIFT" width="1020px" border="1" cellspacing="1" cellpadding="0" bgcolor="#cccccc" height="100%" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DB0102Search" />
            <asp:PostBackTrigger ControlID="btnPreviousMonth" />
            <asp:PostBackTrigger ControlID="btnNextMonth" />
            <asp:PostBackTrigger ControlID="WFB2DB0102Save" />
            <asp:PostBackTrigger ControlID="btnCancle" />
            <asp:PostBackTrigger ControlID="WFB2DB0102Grant" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ID="hidRequiretxtWORK_SHIFTYearMonthMessage" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidtxtWORK_SHIFTYearMonthFormatErrMessag" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidWORK_SHIFTKeepValue" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidwfb2db_Cancel_Confirm" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidwfb2db_Save_ConfirmMessage" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidWORK_SHIFTYearMonthWin" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidOnGrantWeeklyFinally" ClientIDMode="Static" />
    

</asp:Content>
