<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2da/WFB2DA0100_Dtl.aspx.cs" Inherits="WebContent_fb2da_WFB2DA0100_Dtl" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCCommCodeDropDwonList.ascx" TagPrefix="uc1" TagName="UCCommCodeDropDwonList" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            $('#txtCALENDARYearMonth').datepicker({ dateFormat: 'yy/mm' });
            $('#txtCALENDARYearMonth').mask('9999/99');
            iniForm();
        });

        function iniForm() {
            $.unblockUI();
        }
        function Clear() {
            var NowDateTime = new Date();
            var Year = NowDateTime.getFullYear().toString();
            var Month = NowDateTime.getMonth() + 1;
            var YearMonth = "";
            if (Month < 10)
                YearMonth = Year + "/0" + Month.toString();
            else
                YearMonth = Year + "/" + Month.toString();

            $('#txtCALENDARYearMonth').val(YearMonth);
        }
        function txtCALENDARYearMonthCheck() {

            var TextBoxValue = $("#txtCALENDARYearMonth").val() + "/01";
            if (TextBoxValue.length == 0) {
                alert($("#hidRequiretxtCALENDARYearMonthMessage").val());
            }
            else {
                if (checkdate(TextBoxValue) == false) {
                    alert($("#hidtxtCALENDARYearMonthFormatErrMessag").val());
                    return false;
                }
                else {
                    BlockUI();
                }
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
        function getCalendarValues() {
            if (confirm($('#hidwfb2da_Save_ConfirmMessage').val())) {
                var DropList = $("Select");
                var YearMonth = $("#txtCALENDARYearMonthWin").val();
                var JasonValue = "{\"Dtl\":[";
                for (var i = 0; i < DropList.length; i++) {
                    var workDay = DropList[i].id.replace("dllWorkDay", "");
                    var SelectValue = DropList[i].value;
                    if ($('#hidWorkDay' + workDay).val() != SelectValue) {
                        JasonValue += "{\"CALENDAR_DT\":\"" + YearMonth + "/" + (workDay < 10 ? "0" + workDay.toString() : workDay.toString()) + "\",\"" +
                                       "DT_TYPE\":\"" + SelectValue + "\"}";
                        if (i < DropList.length - 1)
                            JasonValue += ",";
                    }
                }
                JasonValue += "]}";
                $("#hidCalendarKeepValue").val(JasonValue);
                BlockUI();
            } else {
                $.unblockUI();
                return false;
            }
        }

        function CancelConfirm() {
            if (confirm($('#hidwfb2da_Cancel_Confirm').val())) {
                BlockUI();
                return;
            }
            else {
                $.unblockUI();
                return false;
            }
        }

        function CancelConfirm() {
            if (confirm($('#hidwfb2da_Cancel_Confirm').val())) {
                BlockUI();
                return;
            }
            else {
                $.unblockUI();
                return false;
            }
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
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lblCALENDAR_CD" Text="<%$Resources:Resource,wfb2da_lb_CALENDAR_CD%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txtCALENDAR_CD" runat="server" Enabled="false" Style="width: 50px;" ClientIDMode="Static" />
                                    <asp:TextBox ID="txtCALENDAR_DESC" runat="server" Enabled="false" Style="width: 200px;" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lblCALENDARYearMonth" Text="<%$Resources:Resource,wfb2da_CALENDARYearMonth%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txtCALENDARYearMonth" ClientIDMode="Static" runat="server" CssClass="MandatoryField" MaxLength="6" Style="width: 50px;" />
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <th></th>
                                <th></th>
                                <td align="right" class="Body_label">
                                    <div id="init">
                                        <aces:Btn ID="WFB2DA0101Search" runat="server" Text="<%$Resources:Resource,wfb2da_Search%>" OnClientClick="return txtCALENDARYearMonthCheck();" ClientIDMode="Static" ValidationGroup="GroupA" OnClick="btn_search_Click" />


                                        <%--<asp:Button ID="WFB2DA0101Search" runat="server" Text="<%$Resources:Resource,wfb2da_Search%>" OnClientClick="return txtCALENDARYearMonthCheck();" ClientIDMode="Static" ValidationGroup="GroupA" OnClick="btn_search_Click" />--%>
                                        <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2da_Clear%>" ClientIDMode="Static" OnClientClick="Clear();return false;" />
                                        <asp:Button ID="btn_Back" runat="server" Text="<%$Resources:Resource,wfb2da_Back%>" ClientIDMode="Static" OnClick="btn_Back_Click" />
                                        <%--OnClientClick="$(location).attr('href', 'WFB2DA0100_Qry.aspx'); return false;"--%>
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
                <colgroup>
                    <col width="30%" />
                    <col width="10%" />
                    <col width="10%" />
                    <col width="50%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th style="text-align: left;">
                            <div runat="server" id="divTimeShift" style="width: 100%;">
                                <asp:Button runat="server" ID="btnPreviousMonth" Text="←" OnClick="btnPreviousMonth_Click" OnClientClick="BlockUI();" ClientIDMode="Static" />
                                <asp:TextBox runat="server" ID="txtCALENDARYearMonthWin" CssClass="MandatoryField" Width="148px" ReadOnly="true" ClientIDMode="Static" />
                                <asp:Button runat="server" ID="btnNextMonth" Text="→" OnClick="btnNextMonth_Click" OnClientClick="BlockUI();" ClientIDMode="Static" />
                            </div>
                        </th>
                        <div runat="server" id="divWorkDay">
                            <%--   本月稼動日  --%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label runat="server" ID="Label1" Text="<%$Resources:Resource,wfb2sc_lb_CAL_WORK_DAYS%>" ClientIDMode="Static" />:
                            </th>
                            <td style="text-align: left;padding-left:10px;">
                                <asp:Label runat="server" ID="lb_CalWorkDays" Width="80px" ClientIDMode="Static" />
                            </td>
                        </div>
                        <th style="text-align: right;">
                            <aces:Btn runat="server" ID="WFB2DA0101Dtl_Mod" Visible="false" Text="<%$Resources:Resource,wfb2da_WFB2DA010Dtl_Mod%>" ClientIDMode="Static" OnClientClick="BlockUI();" OnClick="btn_Dtl_Mod_Click" />
                            <aces:Btn runat="server" ID="WFB2DA0101Save" Visible="false" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Save%>" OnClientClick="return getCalendarValues();" ClientIDMode="Static" OnClick="btnSave_Click" />
                            <aces:Btn runat="server" ID="WFB2DA0101Grant" Text="<%$Resources:Resource,wfb2da_WFB2DA010Dtl_Grant%>" ClientIDMode="Static" OnClientClick="BlockUI();" OnClick="btn_Grant_Click" />
                            <asp:Button runat="server" ID="btnCancle" Visible="false" Text="<%$Resources:Resource,wfb2da_Cancel%>" OnClientClick="return CancelConfirm()" ClientIDMode="Static"  OnClick="btnCancle_Click" />
                        </th>
                    </tr>
                </tbody>
            </table>
            <table runat="server" id="tbCALENDAR" width="1020px" border="1" cellspacing="1" cellpadding="0" bgcolor="#cccccc" height="100%"></table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DA0101Search" />
            <asp:PostBackTrigger ControlID="btnPreviousMonth" />
            <asp:PostBackTrigger ControlID="btnNextMonth" />
            <asp:PostBackTrigger ControlID="WFB2DA0101Save" />
            <asp:PostBackTrigger ControlID="btnCancle" />
            <asp:PostBackTrigger ControlID="WFB2DA0101Grant" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ID="hidRequiretxtCALENDARYearMonthMessage" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidtxtCALENDARYearMonthFormatErrMessag" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidCalendarKeepValue" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidwfb2da_Cancel_Confirm" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hidwfb2da_Save_ConfirmMessage" ClientIDMode="Static" />

</asp:Content>
