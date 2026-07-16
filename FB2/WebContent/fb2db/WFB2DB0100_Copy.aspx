<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2db/WFB2DB0100_Copy.aspx.cs" Inherits="WebContent_fb2db_WFB2DB0100_Copy" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            $("#txt_LEAVE_DT_S").addClass("MandatoryField");
            $("#txt_LEAVE_DT_E").addClass("MandatoryField");
        });
        function CheckInput() {
            var AlertMessage = "";

            if ($('#txt_LEAVE_DT_S').val() == "")
                AlertMessage = $('#Hidwfb2db_ucDateRange_LEAVE_DT_S_NotNull').val();

            var re = /^[0-9a-zA-Z_]+$/;
            if (!re.test($("#txtWORK_SHIFT_CD_Destination").val()))
                if (AlertMessage == "")
                    AlertMessage = "目的輪值表代碼欄位只能輸入英數字";
                else
                    AlertMessage += "\r\n目的輪值表班別代碼欄位只能輸入英數字";
            if ($('#txt_LEAVE_DT_E').val() == "") {
                if (AlertMessage != "")
                    AlertMessage += "\r\n" + $('#Hidwfb2db_ucDateRange_LEAVE_DT_E_NotNull').val();
                else
                    AlertMessage = $('#Hidwfb2db_ucDateRange_LEAVE_DT_E_NotNull').val();
            }
            if ($('#txt_LEAVE_DT_S').val() > $('#txt_LEAVE_DT_E').val() && $('#txt_LEAVE_DT_S').val() != "" && $('#txt_LEAVE_DT_E').val() != "") {
                if (AlertMessage != "")
                    AlertMessage += "\r\n" + $('#Hidwfb2db_ucDateRange_LEAVE_DT_E_Great_LEAVE_DT_E').val();
                else
                    AlertMessage = $('#Hidwfb2db_ucDateRange_LEAVE_DT_E_Great_LEAVE_DT_E').val();
            }
            if ($('#txtWORK_SHIFT_CD_Destination').val() == "") {
                if (AlertMessage != "")
                    AlertMessage += "\r\n" + $('#Hidwfd2db_WORK_SHIFT_CD_NotNull').val();
                else
                    AlertMessage = $('#Hidwfd2db_WORK_SHIFT_CD_NotNull').val();
            }
            /*
            if ($('#txtWORK_SHIFT_DESC_Destination').val() == "") {
                if (AlertMessage != "")
                    AlertMessage += "\r\n" + $('#Hidwfb2db_WORK_SHIFT_DESC_NotNull').val();
                else
                    AlertMessage = $('#Hidwfb2db_WORK_SHIFT_DESC_NotNull').val();
            }
            */
            if (!checkdate($('#txt_LEAVE_DT_S').val()))
                AlertMessage += AlertMessage == "" ? "日期區間(起)格式錯誤" : "\r\n日期區間(起)格式錯誤";

            if (!checkdate($('#txt_LEAVE_DT_E').val()))
                AlertMessage += AlertMessage == "" ? "日期區間(迄)格式錯誤" : "\r\n日期區間(迄)格式錯誤";

            if (AlertMessage != "") {
                alert(AlertMessage);
                return false;
            }
            else {
                if (confirm($('#hidwfb2db_Copy_ConfirmMessage').val())) {
                    BlockUI();
                    return true;
                }
                else {
                    $.unblockUI();
                    return false;
                }
            }
        }
        function TextBoxOnblurShowMessage(TextClientId, message) {
            alert(message);
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
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lblCALENDAR_CD" Text="<%$Resources:Resource,wfb2db_lb_CALENDAR%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txtCALENDAR_CD_Source" Enabled="false" runat="server" Style="width: 50px;" ClientIDMode="Static" />
                                    <asp:TextBox ID="txtCALENDAR_DESC_Source" runat="server" Enabled="false" Style="width: 200px;" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lblWORK_SHIFT_CD" Text="<%$Resources:Resource,wfb2db_lb_WORK_SHIFT%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txtWORK_SHIFT_CD_Source" MaxLength="2" Enabled="false" runat="server" Style="width: 50px;" ClientIDMode="Static" />
                                    <asp:TextBox ID="txtWORK_SHIFT_DESC_Source" runat="server" Enabled="false" Style="width: 200px;" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lbl_CALENDARYearMonth" Text="<%$Resources:Resource,wfb2db_ucDateRange%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <uc1:UCDateTimeRange runat="server" ID="uc_DateRange" ControlClientIDMode="Static" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label runat="server" ID="lblCopyTo" Text="<%$Resources:Resource,wfd2da_CopyTo%>" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lblWORK_SHIFT_CD_Destination" Text="<%$Resources:Resource,wfb2db_Destination_WORK_SHIFT%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txtWORK_SHIFT_CD_Destination" runat="server" MaxLength="2" Style="width: 50px;" ClientIDMode="Static" CssClass="MandatoryField" />
                                    <asp:TextBox ID="txtWORK_SHIFT_DESC_Destination" runat="server" MaxLength="50" Style="width: 200px;" ClientIDMode="Static"  />
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr>
                    <th style="text-align: right;">
                        <aces:Btn ID="WFB2DB0102Save" runat="server" ClientIDMode="Static" OnClick="btnSave_Click" OnClientClick="return CheckInput();" Text="<%$Resources:Resource,wfb2db_WFB2DB0100Save%>" Visible="True" />

                        <%--<asp:Button ID="WFB2DB0102Save" runat="server" ClientIDMode="Static" OnClick="btnSave_Click" OnClientClick="return CheckInput();" Style="height: 21px" Text="<%$Resources:Resource,wfb2db_WFB2DB0100Save%>" Visible="True" />--%>
                        <asp:Button ID="btnCancle" runat="server" ClientIDMode="Static" OnClick="btnCancle_Click" Text="<%$Resources:Resource,wfb2db_Cancel%>" /> <%--OnClientClick="$(location).attr('href', 'WFB2DB0100_Qry.aspx'); return false;"--%>
                    </th>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DB0102Save" />
            <asp:PostBackTrigger ControlID="btnCancle" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField ID="Hidwfb2db_ucDateRange_LEAVE_DT_S_NotNull" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="Hidwfb2db_ucDateRange_LEAVE_DT_E_NotNull" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="Hidwfb2db_WORK_SHIFT_DESC_NotNull" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="Hidwfd2db_WORK_SHIFT_CD_NotNull" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="Hidwfb2db_ucDateRange_LEAVE_DT_E_Great_LEAVE_DT_E" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="Hidwfb2db_CALENDAR_NotNull" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hidwfb2db_Copy_ConfirmMessage" runat="server" ClientIDMode="Static" />
</asp:Content>
