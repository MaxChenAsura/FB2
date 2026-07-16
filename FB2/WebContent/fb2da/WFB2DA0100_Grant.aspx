<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2da/WFB2DA0100_Grant.aspx.cs" MasterPageFile="~/MasterPage.master" Inherits="WebContent_fb2da_WFB2DA0100_Grant" %>
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
                barcolor: "#7F7F7F"
            });
        }

        function GrantConfimAfter(message) {
            if (confirm(message)) {
                BlockUI();
                $('#btn_Grant_Confim_later').click();
            }
            else
                $.unblockUI();
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1) {
                BlockUI();
                return true;
            }
            else {
                alert($('#hidwfb2da_Mod_NotChoiceMessage').val());
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

        function CheckSaveAction() {
            var Message = "";

            if ($('#drpWORK_DAY_Edit') != undefined) {
                if ($('#drpWORK_DAY_Edit option:selected').text().trim() == "") {
                    Message = $('#hidwfb2da_txtWORK_DAY_DESC_Edit_NotNull').val();
                    alert(Message);
                    return false;
                }
                else {
                    BlockUI();
                    if (confirm($('#hidwfb2da_Save_ConfirmMessage').val())) {
                        BlockUI();
                        return;
                    }
                    else {
                        $.unblockUI();
                        return false;
                    }
                }
            }
            else {
                if (confirm($('#hidwfb2da_Save_ConfirmMessage').val())) {
                    BlockUI();
                    return;
                }
                else {
                    $.unblockUI();
                    return false;
                }

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

        function GrantConfirm() {
            var AlertMessage = "";
            if ($('#txt_LEAVE_DT_S').val() > $('#txt_LEAVE_DT_E').val() && $('#txt_LEAVE_DT_S').val() != "" && $('#txt_LEAVE_DT_E').val() != "") {
                if (AlertMessage != "")
                    AlertMessage += "\r\n" + $('#Hidwfb2da_ucDateRange_LEAVE_DT_E_Great_LEAVE_DT_E').val();
                else
                    AlertMessage = $('#Hidwfb2da_ucDateRange_LEAVE_DT_E_Great_LEAVE_DT_E').val();
            }
            if (AlertMessage != "") {
                alert(AlertMessage);
                return false;
            }
            else {
                if (confirm($('#hidfb2da_btn_Grant_ConfirmMessage').val())) {
                    BlockUI();
                    return;
                }
                else {
                    $.unblockUI();
                    return false;
                }
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
                                    <asp:Label runat="server" ID="Label1" Text="<%$Resources:Resource,wfb2da_lb_CALENDAR_CD%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_CALENDAR_CD" runat="server" Enabled="false" Style="width: 50px;" ClientIDMode="Static" />
                                    <asp:TextBox ID="txt_CALENDAR_DESC" runat="server" Enabled="false" Style="width: 200px;" ClientIDMode="Static" />
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <hr style="width: 1020px; text-align: left;" />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF">
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lbl_CALENDARYearMonth" Text="<%$Resources:Resource,wfb2da_ucDateRange%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <uc1:UCDateTimeRange runat="server" ID="uc_DateRange" ControlClientIDMode="Static" ClientIDMode="Static" EndDateCssClass="date" StartDateCssClass="date" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <%--類型 --%>
                                    <asp:Label runat="server" ID="lbl_CALENDAR_TYPE" Text="<%$Resources:Resource,wfb2da_CALENDAR_TYPE%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <uc1:UCCommCodeDropDwonList runat="server" ID="uc_CALENDAR_TYPE"
                                        FirstItem="<%$Resources:Resource,wfb2da_dll_PlaceChoice%>"
                                        DataTextField="SUB_CD,SUB_DESC"
                                        DataValueField="SUB_CD"
                                        MAIN_CDs="CALENDAR_TYPE"
                                        OrderSeq="ASC"
                                        IS_VALID="True"
                                        DataTextFormatString="{0} - {1}"
                                        SYS_CDs="DA" OnSelectIndexChanged="uc_CALENDAR_TYPE_SelectIndexChanged" AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr runat="server" id="tr_Recycle_Title" visible="true">
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lbltodo" Text="<%$Resources:Resource,wfd2da_Loop_Rule%>" ClientIDMode="Static" />
                                </th>
                                <td></td>
                            </tr>
                            <tr runat="server" id="tr_Recycle_Buttons" visible="true">
                                <td colspan="2" align="right" class="Body_label">
                                    <div id="init_grid" style="margin-bottom: 7px;">
                                        <aces:Btn ID="WFB2DA0100Add" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Add%>" Visible="false" OnClick="WFB2DA0100Add_Click" />
                                        <aces:Btn ID="WFB2DA0100Delete" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Delete%>" Visible="false" OnClientClick="return CheckDelAction();" OnClick="WFB2DA0100Delete_Click" />
                                        <aces:Btn ID="WFB2DA0100Edit" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Edit%>" Visible="false" OnClick="WFB2DA0100Edit_Click" OnClientClick="return CheckModeifyAction();" />
                                        <aces:Btn ID="WFB2DA0100Save" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Save%>" Visible="false" OnClick="WFB2DA0100Save_Click" OnClientClick="return CheckSaveAction();" />

                                        <%--<asp:Button ID="WFB2DA0100Add" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Add%>" Visible="false" OnClick="WFB2DA0100Add_Click" />
                                        <asp:Button ID="WFB2DA0100Delete" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Delete%>" Visible="false" OnClientClick="return CheckDelAction();" OnClick="WFB2DA0100Delete_Click" />
                                        <asp:Button ID="WFB2DA0100Edit" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Edit%>" Visible="false" OnClick="WFB2DA0100Edit_Click" OnClientClick="return CheckModeifyAction();" />
                                        <asp:Button ID="WFB2DA0100Save" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Save%>" Visible="false" OnClick="WFB2DA0100Save_Click" OnClientClick="return CheckSaveAction();" />--%>
                                        <asp:Button ID="WFB2DA0100Cancel" runat="server" Text="<%$Resources:Resource,wfb2da_Cancel%>" Visible="false" OnClick="WFB2DA0100Cancel_Click" OnClientClick="return CancelConfirm();" />
                                    </div>
                                </td>
                            </tr>
                            <tr runat="server" id="tr_Recycle_Grid" visible="true">
                                <td colspan="2">
                                    <asp:GridView ID="gv_result" runat="server" ClientIDMode="Static"
                                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                                        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
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
                                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2da_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="70px">
                                                <ItemTemplate>
                                                    <asp:Label ID="RowNumber" Text='<%#Bind("RowNumber")%>' runat="server" Width="97%" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="text-align: center; width: 100%;">
                                                        <asp:Label ID="RowNumber_Edit" Text='<%#Bind("RowNumber")%>' runat="server" Width="97%" />
                                                    </div>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfd2da_WORK_DAY_DESC%>" SortExpression="WORK_DAY_DESC" HeaderStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="drpWORK_DAY" runat="server" CssClass="MandatoryField" ClientIDMode="Static" Enabled="false" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <div style="text-align: center; width: 100%;">
                                                        <asp:DropDownList ID="drpWORK_DAY_Edit" runat="server" CssClass="MandatoryField" ClientIDMode="Static" Enabled="true" />
                                                    </div>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: center; width: 100%">
                                                        <asp:DropDownList ID="drpWORK_DAY_Edit" runat="server" CssClass="MandatoryField" ClientIDMode="Static" Enabled="true" />
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="GridviewScrollPager" />
                                        <FooterStyle CssClass="GridviewScrollPager" />
                                        <EmptyDataTemplate>
                                            <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                                                <tr class="header">
                                                    <td width="20px"></td>
                                                    <td width="70px">
                                                        <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb2da_RowNumber%>" Width="97%"></asp:Label>
                                                    </td>
                                                    <td width="90px">
                                                        <asp:Label ID="lblHeaderWORK_DAY" runat="server" Text="<%$Resources:Resource,wfd2da_WORK_DAY_DESC%>" Width="97%"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="normal">
                                                    <td></td>
                                                    <td></td>
                                                    <td>
                                                        <div style="text-align: center; width: 100%">
                                                            <asp:DropDownList ID="drpWORK_DAY_Edit" runat="server" ClientIDMode="Static" CssClass="MandatoryField" Enabled="true" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <%--                                    <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
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
                                    </table>--%>
                                    <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="text-align: right;">
                                    <aces:Btn runat="server" ID="WFB2DA0100Grant" Text="<%$Resources:Resource,wfd2da_Grant%>" OnClick="btn_Grant_Click" OnClientClick="return GrantConfirm();" ClientIDMode="Static" />

                                    <%--<asp:Button runat="server" ID="WFB2DA0100Grant" Text="<%$Resources:Resource,wfd2da_Grant%>" OnClick="btn_Grant_Click" OnClientClick="return GrantConfirm();" ClientIDMode="Static" />--%>
                                    <asp:Button runat="server" ID="btn_Grant_Confim_later" OnClick="btn_Grant_Confim_later_Click" OnClientClick="BlockUI();" ClientIDMode="Static" Style="display: none" />
                                    <asp:Button runat="server" ID="btn_Cancel" OnClick="btn_Cancel_Click" Text="<%$Resources:Resource,wfb2da_Cancel%>" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DA0100Grant" />
            <asp:PostBackTrigger ControlID="WFB2DA0100Add" />
            <asp:PostBackTrigger ControlID="WFB2DA0100Save" />
            <asp:PostBackTrigger ControlID="WFB2DA0100Edit" />
            <asp:PostBackTrigger ControlID="WFB2DA0100Cancel" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Del_NotChoiceMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Mod_NotChoiceMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Save_ConfirmMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Del_ConfirmMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Cancel_Confirm" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_txtWORK_DAY_DESC_Edit_NotNull" />
    <asp:HiddenField runat="server" ID="hidfb2da_btn_Grant_ConfirmMessage" ClientIDMode="Static" />
     <asp:HiddenField ID="Hidwfb2da_ucDateRange_LEAVE_DT_E_Great_LEAVE_DT_E" runat="server" ClientIDMode="Static" />
</asp:Content>
