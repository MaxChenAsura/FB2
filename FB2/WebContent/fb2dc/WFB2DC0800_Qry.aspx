<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC0800_Qry.aspx.cs" Inherits="WebContent_fb2dc_WFB2DC0800_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.ym').mask('9999/99/99');

            //工號取得姓名的ajax
            $("#txt_EMP_NAME").attr("readonly", true);
            $("#txt_EMP_ID").blur(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val(JData.EMP_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME').val("");
                }
            });

            //部門代號取得部門名稱的ajax
            $("#txt_DEPT_NAME").attr("readonly", true);
            $("#txt_DEPT_NO").blur(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DEPT_NAME').val("");
                }
            });

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
                ,freezesize: 6
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val($("#hid_defalut_EMP_ID").val());
            $("#txt_EMP_NAME").val($("#hid_defalut_EMP_NAME").val());
            $("#txt_CALENDAR_DT_S").val("");
            $("#txt_CALENDAR_DT_E").val("");

            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#ddl_DUTY_CHECK_RESULT").val("-1");
            $("#txt_REMARK").val("");
        }

        function CheckSearch() {

        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupA")) {

                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }

        function MyCheckValid(msg) {
            BlockUI();
            processed = confirm("確定要進行" + msg + "?");
            if (!processed) {
                $.unblockUI();
            }

            return processed;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="20%" />
                    <col width="15%" />
                    <col width="25%" />
                    <col width="10%" />
                    <col width="20%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CALENDAR_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CALENDAR_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" >
                            <asp:TextBox ID="txt_CALENDAR_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date ym"></asp:TextBox>
                            ~
                           <asp:TextBox ID="txt_CALENDAR_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date ym"></asp:TextBox>
                            <asp:CustomValidator ID="cv_CALENDAR_DT_S" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CALENDAR_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_CALENDAR_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="cv_CALENDAR_DT_E" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CALENDAR_DT_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_CALENDAR_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_CALENDAR_DT_S"
                                ControlToValidate="txt_CALENDAR_DT_E" ErrorMessage="<%$Resources:Resource,wfb2dc_GreaterThanEqual_CALENDAR_DT%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_Required_CALENDAR_DT_S%>"
                                ControlToValidate="txt_CALENDAR_DT_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_Required_CALENDAR_DT_E%>"
                                ControlToValidate="txt_CALENDAR_DT_E" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                          </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_DEPT_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                            <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DUTY_CHECK_RESULT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_DUTY_CHECK_RESULT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_DUTY_CHECK_RESULT" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_REMARK%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" >
                            <asp:TextBox ID="txt_REMARK" runat="server" MaxLength="200" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="6">
                            <div id="init">
                                <aces:Btn ID="WFB2DC0800Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0800Search%>" OnClick="WFB2DC0800Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                <aces:Btn ID="WFB2SC4103ShiftSearch" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0800Search%>" OnClick="WFB2DC0800Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                <%--<asp:Button ID="WFB2DC0800Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0800Search%>" OnClick="WFB2DC0800Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>

                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dc_btn_clear%>" onclick="ClearAll();" />

                            </div>
                        </td>
                    </tr>


                    <!-- end: Create MODULE ID -->
                    <!-- START: Create a line to separate Search field with body field -->
                    <tr>
                        <td align="center" height="1" colspan="6">
                            <hr>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="6">
                            <div id="init_grid" style="display: inline">
                                <aces:Btn ID="WFB2DC0800Reopen" runat="server" Text="REOPEN" Height="26px" OnClick="WFB2DC0800Reopen_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2DC0800CardImport" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0800CardImport%>" OnClick="WFB2DC0800CardImport_Click" OnClientClick="return MyCheckValid(this.value);" ValidationGroup="GroupA" />
                                <aces:Btn ID="WFB2DC0800Export" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0800Export%>" OnClick="WFB2DC0800Export_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                <aces:Btn ID="WFB2SC4103CardImport" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0800CardImport%>" OnClick="WFB2DC0800CardImport_Click" OnClientClick="return MyCheckValid(this.value);" ValidationGroup="GroupA" />
                                <aces:Btn ID="WFB2SC4103Export" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0800Export%>" OnClick="WFB2DC0800Export_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />

                                <%--<asp:Button ID="WFB2DC0800CardImport" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0800CardImport%>" OnClick="WFB2DC0800CardImport_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                        <asp:Button ID="WFB2DC0800Export" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0800Export%>" OnClick="WFB2DC0800Export_Click"  ValidationGroup="GroupA" />--%>
                            </div>
                        </td>
                    </tr>
                    <!-- END: Create a line -->
                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DC0800DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_CALENDAR_DT_S" DefaultValue=""
                        Name="calendar_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_CALENDAR_DT_E" DefaultValue=""
                        Name="calendar_dt_e" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_DUTY_CHECK_RESULT"
                        Name="duty_check_result" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_REMARK" DefaultValue=""
                        Name="remark" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_is_super"
                        Name="is_super" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_is_dept"
                        Name="is_dept" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_departments"
                        Name="departments" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
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
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2dc_RowNumber%>" HeaderStyle-Width="40px"  ItemStyle-Width="40px"  />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2dc_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2dc_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="60px" ItemStyle-Width="60px"  ItemStyle-HorizontalAlign="Center" />
                    <%-- 勤務日期 --%>
                    <asp:BoundField DataField="CALENDAR_DT" HeaderText="<%$Resources:Resource,wfb2dc_lb_CALENDAR_DT%>" SortExpression="CALENDAR_DT" HeaderStyle-Width="100px"  ItemStyle-Width="100px" />
                    <%-- 出勤別 --%>
                    <asp:BoundField DataField="WORK_DAY_DESC" HeaderText="<%$Resources:Resource,wfb2dc_lb_WORK_DAY_CD%>" SortExpression="WORK_DAY_CD" HeaderStyle-Width="60px"  ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 日期類型 --%>
                    <asp:BoundField DataField="DT_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2dC_lb_DT_TYPE%>" SortExpression="DT_TYPE" HeaderStyle-Width="120px"  ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 工數區分 --%>
                    <asp:BoundField DataField="WORK_CD_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_WORK_CD%>" SortExpression="WORK_CD" HeaderStyle-Width="80px"  ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 勤務上班時間 --%>
                    <asp:BoundField DataField="DUTY_STIME" HeaderText="<%$Resources:Resource,wfb2dc_lb_DUTY_STIME%>" SortExpression="DUTY_STIME" HeaderStyle-Width="160px"  ItemStyle-Width="160px" />
                    <%-- 勤務下班時間 --%>
                    <asp:BoundField DataField="DUTY_ETIME" HeaderText="<%$Resources:Resource,wfb2dc_lb_DUTY_ETIME%>" SortExpression="DUTY_ETIME" HeaderStyle-Width="160px"  ItemStyle-Width="160px" />
                    <%-- 刷卡上班時間 --%>
                    <asp:BoundField DataField="CLOCK_IN_DT" HeaderText="<%$Resources:Resource,wfb2dc_lb_CLOCK_IN_DT%>" SortExpression="CLOCK_IN_DT" HeaderStyle-Width="160px"  ItemStyle-Width="160px" />
                    <%-- 刷卡上班資料來源 --%>
                    <asp:BoundField DataField="IN_DATA_SOURCE_DESC" HeaderText="<%$Resources:Resource,wfb2dc_lb_IN_DATA_SOURCE_CD%>" SortExpression="IN_DATA_SOURCE_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px"  ItemStyle-HorizontalAlign="Left" />
                    <%-- 刷卡下班時間 --%>
                    <asp:BoundField DataField="CLOCK_OUT_DT" HeaderText="<%$Resources:Resource,wfb2dc_lb_CLOCK_OUT_DT%>" SortExpression="CLOCK_OUT_DT" HeaderStyle-Width="160px"  ItemStyle-Width="160px" />
                    <%-- 刷卡下班資料來源 --%>
                    <asp:BoundField DataField="OUT_DATA_SOURCE_DESC" HeaderText="<%$Resources:Resource,wfb2dc_lb_OUT_DATA_SOURCE_CD%>" SortExpression="OUT_DATA_SOURCE_CD" HeaderStyle-Width="80px" ItemStyle-Width="80px"  ItemStyle-HorizontalAlign="Left" />
                    <%-- 刷卡比對狀態 --%>
                    <asp:BoundField DataField="DUTY_CHECK_RESULT_DESC" HeaderText="<%$Resources:Resource,wfb2dc_lb_DUTY_CHECK_RESULT%>" SortExpression="DUTY_CHECK_RESULT" HeaderStyle-Width="100px" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Left" />
                    <%-- 遲到時數 --%>
                    <asp:BoundField DataField="LATE_HOUR" HeaderText="<%$Resources:Resource,wfb2dc_lb_LATE_HOUR%>" SortExpression="LATE_HOUR" HeaderStyle-Width="70px" ItemStyle-Width="70px"  />
                    <%-- 早退時數 --%>
                    <asp:BoundField DataField="LEAVE_EARLY_HOUR" HeaderText="<%$Resources:Resource,wfb2dc_lb_LEAVE_EARLY_HOUR%>" SortExpression="LEAVE_EARLY_HOUR" HeaderStyle-Width="70px" ItemStyle-Width="70px"  />
                    <%-- 欠勤時數 --%>
                    <asp:BoundField DataField="LACK_HOUR" HeaderText="<%$Resources:Resource,wfb2dc_lb_LACK_HOUR%>" SortExpression="LACK_HOUR" HeaderStyle-Width="70px" ItemStyle-Width="70px"  />
                    <%-- 出勤時數 --%>
                    <asp:BoundField DataField="DUTY_HOUR" HeaderText="<%$Resources:Resource,wfb2dc_lb_DUTY_HOUR%>" SortExpression="DUTY_HOUR" HeaderStyle-Width="70px" ItemStyle-Width="70px"  />
                    <%-- 請假核准時數 --%>
                    <asp:BoundField DataField="LEAVE_HOUR" HeaderText="<%$Resources:Resource,wfb2dc_lb_LEAVE_HOUR%>" SortExpression="LEAVE_HOUR" HeaderStyle-Width="70px"  ItemStyle-Width="70px" />
                    <%-- 請假資訊 --%>
                    <asp:BoundField DataField="LEAVE_INFO" HeaderText="<%$Resources:Resource,wfb2dc_lb_LEAVE_INFO%>" SortExpression="LEAVE_INFO" HeaderStyle-Width="160px"  ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 加班申請時數 --%>
                    <asp:BoundField DataField="OVERTIME_HOUR_APPLY" HeaderText="<%$Resources:Resource,wfb2dc_lb_OVERTIME_HOUR_APPLY%>" SortExpression="OVERTIME_HOUR_APPLY" HeaderStyle-Width="70px"  ItemStyle-Width="70px" />
                    <%-- 加班核准時數 --%>
                    <asp:BoundField DataField="OVERTIME_HOUR_APPROVE" HeaderText="<%$Resources:Resource,wfb2dc_lb_OVERTIME_HOUR_APPROVE%>" SortExpression="OVERTIME_HOUR_APPROVE" HeaderStyle-Width="70px" ItemStyle-Width="70px"  />
                    <%-- 加班計算時數 --%>
                    <asp:BoundField DataField="OVERTIME_PAY_HOUR" HeaderText="<%$Resources:Resource,wfb2dc_lb_OVERTIME_HOUR_CAL%>" SortExpression="OVERTIME_HOUR_APPROVE" HeaderStyle-Width="70px" ItemStyle-Width="70px"  />
                    <%-- 勤前滯留時數 --%>
                    <asp:BoundField DataField="VIOLATE_BEFORE_HOUR" HeaderText="<%$Resources:Resource,wfb2di_VIOLATE_BEFORE_HOUR%>" SortExpression="VIOLATE_BEFORE_HOUR" HeaderStyle-Width="70px"  ItemStyle-Width="70px" />
                    <%-- 勤後滯留時數 --%>
                    <asp:BoundField DataField="VIOLATE_AFTER_HOUR" HeaderText="<%$Resources:Resource,wfb2di_VIOLATE_AFTER_HOUR%>" SortExpression="VIOLATE_AFTER_HOUR" HeaderStyle-Width="70px"  ItemStyle-Width="70px" />
                    <%-- 加班資訊 --%>
                    <asp:BoundField DataField="OVERTIME_INFO" HeaderText="<%$Resources:Resource,wfb2dc_lb_OVERTIME_INFO%>" SortExpression="OVERTIME_INFO" HeaderStyle-Width="160px"  ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 備註 --%>
                    <asp:BoundField DataField="REMARK" HeaderText="<%$Resources:Resource,wfb2dc_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="140px"  ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 輪班津貼 --%>
                    <asp:BoundField DataField="WORK_SHIFT_ALLOWANCE_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2dc_lb_WORK_SHIFT_ALLOWANCE_TYPE%>" SortExpression="WORK_SHIFT_ALLOWANCE_TYPE" HeaderStyle-Width="140px"  ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left" />
                    <%-- 部門名稱 --%>
                    <asp:BoundField DataField="DEPT_FULL_NAME" HeaderText="<%$Resources:Resource,wfb2dc_lb_DEPT_FULL_NAME%>" HeaderStyle-Width="360px"  ItemStyle-Width="360px" ItemStyle-HorizontalAlign="Left" />

                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_is_super" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_is_dept" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_departments" runat="server" ClientIDMode="Static" />
            <!--預設的工號,姓名-->
            <asp:HiddenField ID="hid_defalut_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_defalut_EMP_NAME" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DC0800Export"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2SC4103Export"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
