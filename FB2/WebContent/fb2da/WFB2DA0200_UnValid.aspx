<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2da/WFB2DA0200_UnValid.aspx.cs" Inherits="WebContent_fb2da_WFB2DA0200_UnValid" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            $('.date').datepicker({ dateFormat: 'yy/mm/dd' });
            $('#txt_END_DT').mask('9999/99/99');
        });

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

        function Already_Confim(message) {
            if (confirm(message))
                $("#WFB2DA0200SaveIs_AlreadyConfirmAfter").click();
            else
                return false;
        }

        function Valid() {
            BlockUI();
            if ($('#txt_END_DT').val() == "") {
                alert($("#hidRequired_END_DT").val());
                return false;
            }
            else {
                if ($('#txt_END_DT').val() < $('#lb_START_DT_Value').text()) {
                    //'結束日期不可小於生效日期'
                    alert($("#hid_wfd2da_GreaterThan_ValidDate").val());
                    return false;
                }
                if (checkdate($('#txt_END_DT').val())) {
                    if (confirm($("#hid_Save_Confirm").val())) {
                        BlockUI();
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    alert($("#hid_END_DT_FormatError").val());
                    return false;

                }
            }
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
                                <th align="left" class="Body_TableHeader" width="20%">
                                    <asp:Label runat="server" ID="lb_SHIFT_CD" Text="<%$Resources:Resource,wf2da_lbSHIFT_CD%>" ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label" width="30%">
                                    <asp:Label runat="server" ID="lb_SHIFT_CD_value" ClientIDMode="Static" />
                                </td>
                                <th align="left" class="Body_TableHeader" width="20%">
                                    <asp:Label runat="server" ID="lb_SHIFT_DESC" Text="<%$Resources:Resource,wf2da_SHIFT_DESC%>" ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label" width="30%">
                                    <asp:Label runat="server" ID="lb_SHIFT_DESC_Value" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader" width="20%">
                                    <asp:Label ID="lb_START_DT" runat="server" Text='<%$Resources:Resource,wfb2da_lb_START_DT%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label" colspan="3" width="80%">
                                    <asp:Label ID="lb_START_DT_Value" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <%-- 結束日期 --%>
                                <th align="left" class="Body_TableHeader" width="20%">
                                    <asp:Label ID="lb_End_DT" runat="server" Text='<%$Resources:Resource,wfb2da_WFB2DA0200EndDate%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label" colspan="3" width="80%">
                                    <asp:TextBox ID="txt_END_DT" runat="server" Width="81px" CssClass="date MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="Body_label" colspan="4">
                                    <div id="init_grid" style="margin-bottom: 7px;">
                                        <aces:Btn ID="WFB2DA0200Save" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0200Save%>" OnClick="WFB2DA0200Save_Click" ValidationGroup="GroupA" OnClientClick="return Valid();" ClientIDMode="Static" />

                                        <%--<asp:Button ID="WFB2DA0200Save" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0200Save%>" OnClick="WFB2DA0200Save_Click" ValidationGroup="GroupA" OnClientClick="return Valid();" ClientIDMode="Static" />--%>
                                        <asp:Button ID="WFB2DA0200Cancel" runat="server" Text="<%$Resources:Resource,wfb2da_Cancel%>" ClientIDMode="Static" />
                                        <asp:Button ID="WFB2DA0200SaveIs_AlreadyConfirmAfter" runat="server" Text="" OnClick="WFB2DA0200SaveIs_AlreadyConfirmAfter_Click" Style="display: none;" ClientIDMode="Static" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DA0200Save" />
            <asp:PostBackTrigger ControlID="WFB2DA0200SaveIs_AlreadyConfirmAfter" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hid_Save_Confirm" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hidRequired_END_DT" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hid_END_DT_FormatError" runat="server" ClientIDMode="Static" />
    <%--結束日期不可小於生效日期--%>
    <asp:HiddenField ID="hid_wfd2da_GreaterThan_ValidDate" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfd2da_GreaterThan_ValidDate%>" />
</asp:Content>
