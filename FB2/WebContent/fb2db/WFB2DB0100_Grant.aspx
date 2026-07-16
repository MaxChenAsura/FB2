<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2db/WFB2DB0100_Grant.aspx.cs" MasterPageFile="~/MasterPage.master" Inherits="WebContent_fb2db_WFB2DB0100_Grant" %>

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
            //$.unblockUI();  //若有加,在__doPostBack時無法請稍候
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
                BlockUI();
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
        function CheckSaveAction() {
            var Message = "";
            if ($('#txt_SHIFT_CD_Edit').val() != undefined) {
                if ($('#txt_SHIFT_CD_Edit').val().trim() == "") {
                    Message = $('#Hidwfb2db_WORK_DAY_DESC_NotNull').val();
                    alert(Message);
                    return false;
                }
                else {
                    BlockUI();
                    if (confirm($('#hidwfb2db_Save_ConfirmMessage').val())) {
                        //$('#hid_EditSHIFT_CD').val($('#txt_SHIFT_CD_Edit').val());
                        //$('#hid_EditSHIFT_DESC').val($('#txt_SHIFT_CD_Edit').val());
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

        function GrantConfirm() {
            var processed = true;

            if (Page_ClientValidate("GroupB")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed) {
                $.unblockUI();
                return;  //不然按取消需按二次
            }
            return processed;
        }

        function doBlockUI() {
            BlockUI();
        }

        //確定是否要產生輪值表
        function GrantConfimAfter(message) {
            if (confirm(message)) {
                BlockUI();
                $('#btn_Grant_Confim_later').click();
                //__doPostBack('question', 'true');
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
                                    <asp:Label runat="server" ID="lb_CALENDAR_CD" Text="<%$Resources:Resource,wfb2db_lb_CALENDAR_CD%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <%-- 行事曆   --%>
                                    <asp:TextBox ID="txt_CALENDAR_CD" runat="server" Enabled="false" Style="width: 50px;" ClientIDMode="Static" />
                                    <asp:TextBox ID="txt_CALENDAR_DESC" runat="server" Enabled="false" Style="width: 200px;" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_WORK_SHIFT_CD" Text="<%$Resources:Resource,wfb2db_WORK_SHIFT_CD%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <%-- 輪值表代碼  --%>
                                    <asp:TextBox ID="txt_WORK_SHIFT_CD" MaxLength="2" runat="server" Enabled="false" Style="width: 50px;" ClientIDMode="Static" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_WORK_SHIFT_DESC" Text="<%$Resources:Resource,wfb2db_WORK_SHIFT_DESC%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_WORK_SHIFT_DESC" runat="server" Enabled="false" Style="width: 200px;" ClientIDMode="Static" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <hr />
                    </td>
                </tr>
            </table>

            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF">
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lbl_WORK_SHIFTYearMonth" Text="<%$Resources:Resource,wfb2db_ucDateRange%>" ClientIDMode="Static" />
                                </th>
                                <td>
                                    <%-- 日期區間(起)--%>
                                    <asp:TextBox ID="txt_START_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                    <!--驗證不可空白 -->
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_required_start_dt%>"
                                        ControlToValidate="txt_START_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                    <!--驗證日期格式-->
                                    <asp:CustomValidator ID="RegularExpressionValidator12" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2db_error_start_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_START_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                    ~
                                <asp:TextBox ID="txt_END_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                    <!--驗證不可空白 -->
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_required_end_dt%>"
                                        ControlToValidate="txt_END_DT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                    <!--驗證日期格式-->
                                    <asp:CustomValidator ID="RegularExpressionValidator14" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2db_error_end_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_END_DT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                                    <!--起日不可大於迄日 -->
                                    <asp:CompareValidator ID="qry5" runat="server" ControlToCompare="txt_START_DT"
                                        ControlToValidate="txt_END_DT" ErrorMessage="<%$Resources:Resource,wfb2db_error_start_dt_s_e%>" Type="Date" Operator="GreaterThanEqual"
                                        Display="None" ValidationGroup="GroupB"></asp:CompareValidator>
                                </td>
                            </tr>
                            <tr runat="server" id="tr_Recycle_Title" visible="true">
                                <%-- 循環規則代碼 --%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_RULE_CD" Text="<%$Resources:Resource,wfb2db_lb_rule_cd%>" ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddl_RULE_CD" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddl_RULE_CD_SelectedIndexChanged" CssClass="MandatoryField" AutoPostBack="true"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfv_HR_CHG_CD" runat="server" ErrorMessage="<%$Resources:Resource,wfb2db_required_rule_cd%>"
                                        ControlToValidate="ddl_RULE_CD" ForeColor="Red" Display="None" ValidationGroup="GroupB"></asp:RequiredFieldValidator>
                                </td>
                                <td style="text-align: right;">
                                    <%-- 產生 --%>
                                    <aces:Btn runat="server" ID="WFB2DB0101Grant" Text="<%$Resources:Resource,wfd2da_Grant%>" OnClick="btn_Grant_Click" OnClientClick="return GrantConfirm();" ClientIDMode="Static" />

                                    <%--<asp:Button runat="server" ID="WFB2DB0101Grant" Text="<%$Resources:Resource,wfd2da_Grant%>" OnClick="btn_Grant_Click" OnClientClick="return GrantConfirm();" ClientIDMode="Static" />--%>
                                    <asp:Button runat="server" ID="btn_Grant_Confim_later" OnClick="btn_Grant_Confim_later_Click" ClientIDMode="Static" OnClientClick="BlockUI();" Style="display: none" />
                                    <asp:Button runat="server" ID="btn_Cancel" OnClick="btn_Cancel_Click" Text="<%$Resources:Resource,wfb2db_Cancel%>" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <hr />
                                </td>
                            </tr>

                            <tr runat="server" id="tr_Recycle_Grid" visible="true">
                                <td colspan="3">
                                    <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getGrantData"
                                        SelectCountMethod="getGrantCount" TypeName="WFB2DB0100DL" EnablePaging="True"
                                        SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                                        OnSelected="ods1_Selected">
                                        <SelectParameters>
                                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            <asp:Parameter Name="maximumRows" Type="Int32" />
                                            <asp:ControlParameter ControlID="ddl_RULE_CD"
                                                Name="ruleCD" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                                        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1018px"
                                        OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                                        <Columns>

                                            <%--序號
                                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_rownumber%>" HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_rule_cd%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="RULE_CD">
                                                <ItemTemplate>
                                                    <asp:Label ID="lb_RULE_CD" runat="server" Text='<%#Bind("RULE_CD")%>' Width="100px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_rule_desc%>" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left" SortExpression="RULE_DESC">
                                                <ItemTemplate>
                                                    <asp:Label ID="lb_RULE_DESC" runat="server" Text='<%#Bind("RULE_DESC")%>' Width="300px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            --%>
                                            <%--流水序號--%>
                                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_rule_seq%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="RULE_SEQ">
                                                <ItemTemplate>
                                                    <asp:Label ID="lb_RULE_SEQ" runat="server" Text='<%#Bind("RULE_SEQ")%>' Width="100px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--班別--%>
                                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_shift_cd%>" HeaderStyle-Width="600px" ItemStyle-HorizontalAlign="Left" SortExpression="SHIFT_CD">
                                                <ItemTemplate>
                                                    <asp:Label ID="lb_SHIFT_CD" runat="server" Text='<%#Bind("SHIFT_DESC")%>' Width="600px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--循環天數--%>
                                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_circle_days%>" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" SortExpression="CIRCLE_DAYS">
                                                <ItemTemplate>
                                                    <asp:Label ID="lb_CIRCLE_DAYS" runat="server" Text='<%#Bind("CIRCLE_DAYS")%>' Width="100px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--是否含假日--%>
                                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_is_include_holiday%>" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Center" SortExpression="IS_INCLUDE_HOLIDAY">
                                                <ItemTemplate>
                                                    <asp:Label ID="lb_IS_INCLUDE_HOLIDAY" runat="server" Text='<%#Bind("IS_INCLUDE_HOLIDAY_DESC")%>' Width="200px"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="GridviewScrollHeader" />
                                        <PagerStyle CssClass="GridviewScrollPager" />
                                    </asp:GridView>
                                    <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                                        <tr height="100%" valign="top">
                                            <td class="GridviewScrollPager TD">
                                                <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                                                    <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                                                    <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                                                    <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                                                    <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                                                    <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td style="width: 5px"></td>
                                            <td style="font-size: 14px;">
                                                <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DB0101Grant" />
            <asp:PostBackTrigger ControlID="btn_Grant_Confim_later" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_Del_NotChoiceMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_Mod_NotChoiceMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_Save_ConfirmMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_Del_ConfirmMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_Cancel_Confirm" />
    <asp:HiddenField runat="server" ID="hidfb2db_btn_Grant_ConfirmMessage" ClientIDMode="Static" />

    <asp:HiddenField runat="server" ClientIDMode="Static" ID="Hidwfb2db_WORK_DAY_DESC_NotNull" Value="<%$Resources:Resource,wfb2db_WORK_DAY_DESC_NotNull%>" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
</asp:Content>
