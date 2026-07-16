<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0500_Confirm_YN.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0500_Confirm_YN" Culture="auto" UICulture="auto" %>

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
            $.unblockUI();
        }
        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F",
                freezesize: 5

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }
        function CheckSearch() {
        //    if ($("#rbl_IS_CONFIRM_CHECK input:radio:checked").val() == "") {
        //            alert("未勾選確認刷卡比對");
        //    return false;
        //}
        if (Page_ClientValidate("GroupA")) {
            BlockUI();
        }
        }
        function ClearAll() {
            $("#txt_APPLY_OVERTIME_DT_S").val("");
            $("#txt_APPLY_OVERTIME_DT_E").val("");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="3" cellpadding="0" width="1020">
        <tr>
            <td colspan="3">
                <table cellspacing="1" cellpadding="1" width="100%">
                    <!-- START: Search Criteria Field -->
                    <tr>
                        <td>
                            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                                <colgroup>
                                    <col width="15%" />
                                    <col width="30%" />
                                    <col width="15%" />
                                    <col width="40%" />
                                </colgroup>
                                <tbody>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_APPLY_OVERTIME_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_DT_2%>"></asp:Label>:</th>
                                        <td align="left" >
                                            <asp:TextBox ID="txt_APPLY_OVERTIME_DT_S" runat="server" CssClass="MandatoryField date" MaxLength="10" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                            ～ 
                                    <asp:TextBox ID="txt_APPLY_OVERTIME_DT_E" runat="server" CssClass="MandatoryField date" MaxLength="10" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<% $Resources:Resource,wfd2di_Required_APPLY_OVERTIME_DT_S %> "
                                                ControlToValidate="txt_APPLY_OVERTIME_DT_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                            
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<% $Resources:Resource,wfd2di_Required_APPLY_OVERTIME_DT_E %> "
                                                ControlToValidate="txt_APPLY_OVERTIME_DT_E" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                            
                            <asp:CustomValidator ID="CustomValidatorAPPLY_OVERTIME_SDT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfd2di_ERR_APPLY_OVERTIME_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_OVERTIME_DT_S" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidatorAPPLY_OVERTIME_EDT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfd2di_ERR_APPLY_OVERTIME_DT_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_OVERTIME_DT_E" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>

                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_APPLY_OVERTIME_DT_S"
                                                ControlToValidate="txt_APPLY_OVERTIME_DT_E" ErrorMessage="<%$Resources:Resource,wfb2di_CP_APPLY_OVERTIME_DT%>" Type="Date" Operator="GreaterThanEqual"
                                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                        </td>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_IS_CONFIRM_CHECK" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IS_CONFIRM_CHECK%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:RadioButtonList ID="rbl_IS_CONFIRM_CHECK" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static">
                                                <asp:ListItem Text="已確認" Value="Y" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="未確認" Value="N"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                                <colgroup>
                                    <col width="10%" />
                                    <col width="15%" />
                                    <col width="75%" />
                                </colgroup>
                                <tbody>
                                    <tr>
                                        <td align="left" class="Body_label"></td>
                                        <td align="left" class="Body_label"></td>
                                        <td align="right" class="Body_label">
                                            <aces:Btn ID="WFB2DI0502Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0500Search%>" OnClick="WFB2DI0502Search_Click" OnClientClick="return CheckSearch();" ValidationGroup="GroupA" />
                                            
                                            <%--<asp:Button ID="WFB2DI0502Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0500Search%>" OnClick="WFB2DI0502Search_Click" OnClientClick="return CheckSearch();" ValidationGroup="GroupA" />--%>
                                            
                                            <input id="WFB2DI0500Clear" runat="server" type="button" value="<%$Resources:Resource,wfb2di_WFB2DI0500Clear%>" onclick="ClearAll();" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="1">
                            <hr>
                        </td>
                    </tr>
                    <tr>
                        <td class="Body_PageControl" style="text-align: right">
                            <aces:Btn ID="WFB2DI0502Confirm" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0501Confirm%>" Visible="false" OnClick="WFB2DI0502Confirm_Click" />

                            <%--<asp:Button ID="WFB2DI0502Confirm" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0501Confirm%>" Visible="false" OnClick="WFB2DI0502Confirm_Click" />--%>
                            
                            <asp:Button ID="WFB2DI0500Cancel" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Cancel%>" Visible="true" OnClick="WFB2DI0500Cancel_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <div id="editPage1">
                                <div id="gridList1" style="width: 965px;">
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <!-- START: Show only navigation buttons on search operation -->
        <!--  START: Page Counter and Navigation -->
        <!--  END: Page Counter and Navigation -->
    </table>
    <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getConfirmData"
        SelectCountMethod="getConfirmCount" TypeName="CFB2DI0500DAO" EnablePaging="True"
        SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
        OnSelected="ods1_Selected">
        <SelectParameters>
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:ControlParameter ControlID="txt_APPLY_OVERTIME_DT_S"
                Name="apply_overtime_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
            <asp:ControlParameter ControlID="txt_APPLY_OVERTIME_DT_E"
                Name="apply_overtime_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
            <asp:ControlParameter ControlID="rbl_IS_CONFIRM_CHECK" DefaultValue=""
                Name="is_confirm_check" PropertyName="SelectedValue" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1120px"
        OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1">
        <Columns>
            <asp:TemplateField HeaderStyle-Width="20px">
                <HeaderTemplate>
                    <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="10px" />
            <asp:BoundField DataField="DEPT_NAME" HeaderText="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>" SortExpression="DEPT_NAME" HeaderStyle-Width="40px" />
            <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2di_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="40px" />
            <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2di_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="APPLY_OVERTIME_DT" HeaderText="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_DT%>" SortExpression="APPLY_OVERTIME_DT" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="OVERTIME_DESC" HeaderText="<%$Resources:Resource,wfb2di_lb_OVERTIME_CD%>" SortExpression="OVERTIME_DESC" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="OVERTIME_TIME_CD" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_TIME_CD%>" SortExpression="OVERTIME_TIME_CD" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="APPLY_OVERTIME_HOUR" HeaderText="<%$Resources:Resource,wfb2di_APPLY_OVERTIME_HOUR%>" SortExpression="APPLY_OVERTIME_HOUR" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="IFLOW_NO" HeaderText="<%$Resources:Resource,wfb2di_lb_IFLOW_NO%>" SortExpression="IFLOW_NO" HeaderStyle-Width="30px" HtmlEncode="false" />
            <asp:BoundField DataField="IFLOW_APPROVE_DT" HeaderText="<%$Resources:Resource,wfb2di_lb_IFLOW_APPROVE_DT%>" SortExpression="IFLOW_APPROVE_DT" HeaderStyle-Width="30px" HtmlEncode="false" />
            <asp:BoundField DataField="IS_CONFIRM_CHECK" HeaderText="<%$Resources:Resource,wfb2di_IS_CONFIRM_CHECK%>" SortExpression="IS_CONFIRM_CHECK" HeaderStyle-Width="30px" HtmlEncode="false" />
            <asp:BoundField DataField="CHECK_STATUS" HeaderText="<%$Resources:Resource,wfb2di_lb_CHECK_STATUS%>" SortExpression="CHECK_STATUS" HeaderStyle-Width="30px" HtmlEncode="false" />
        </Columns>
        <HeaderStyle CssClass="GridviewScrollHeader" />
        <PagerStyle CssClass="GridviewScrollPager" />
    </asp:GridView>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
    <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
</asp:Content>
