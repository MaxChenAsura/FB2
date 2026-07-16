<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0500_Qry.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0500_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        //var cal1xx = new CalendarPopup();
        //cal1xx.showNavigationDropdowns();

        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".date").mask("9999/99/99");
            $(".date2").mask("9999/99");
            $(".number2").mask("999");
            $("#txt_EMP_ID").mask("99999");
            gridviewScroll();
            $.unblockUI();

            $('#txt_EMP_NAME').attr("readonly", true);
            //工號取得姓名的ajax
            $("#txt_EMP_ID").change(function () {
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

            //寫在這，按查詢才不會消失
            $('#txt_DEPT_NAME').attr("readonly", true);
            //部門代號取得部門名稱的ajax
            $("#txt_DEPT_NO").change(function () {
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
        }

        //function getEmp() {
        //    var now = new Date();
        //    var date = now.format("yyyy/MM/dd");
        //    var EMP_ID = $("#txt_EMP_ID").val();
        //    OpenSearch('Emp_Search.aspx', 'txt_EMP_ID', 'txt_PJOB_DESC', 'PJOB_CD=' + PJOB_CD + '&START_DT=' + date);

        //}

        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F",
                freezesize: 6

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");

        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        //清空畫面
        function ClearAll() {
            $("#txt_APPLY_OVERTIME_DT_S").val("");
            $("#txt_APPLY_OVERTIME_DT_E").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#ddl_OVERTIME_CD").val("-1");
            $("#ddl_OVERTIME_TIME_CD").val("-1");
            $("#txt_IFLOW_NO").val("");
            $("#txt_IFLOW_APPROVE_DT").val("");
            $("#ddl_CHECK_STATUS").val("-1");
            $("#ddl_FORM_STATUS").val("-1");
            $("#ddl_O_SPECIAL_CD").val("-1");
        }

        function checkconfirm(checkconfirmmessage) {
            var ret = confirm(checkconfirmmessage);
            if (ret) {
                BlockUI();
                __doPostBack('delete', '');

            }
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb299_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="12%" />
                    <col width="89%" />
                </colgroup>
                <tbody>
                    <tr>
                        <td>
                            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">

                                <colgroup>
                                    <col width="12%" />
                                    <col width="21%" />
                                    <col width="12%" />
                                    <col width="21%" />
                                    <col width="12%" />
                                    <col width="22%" />
                                </colgroup>

                                <tbody>

                                    <tr>
                                        <%--勤務期間--%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_APPLY_OVERTIME_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_DT%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label" colspan="5">
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
                                    </tr>

                                    <tr>
                                        <%--工號--%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_ID%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="80px" ClientIDMode="Static"></asp:TextBox>
                                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                                            <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" Width="80px" ClientIDMode="Static"></asp:TextBox>
                                        </td>

                                        <%--部門--%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>"></asp:Label>:</th>

                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="80px" ClientIDMode="Static" ></asp:TextBox>
                                            <input id="bt_DEPT_NO" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                                            <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>

                                        </td>
                                        <%--表單狀態--%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_FORM_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_FORM_STATUS%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_FORM_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <%--加班類型--%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_CD%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_OVERTIME_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        </td>
                                        <%--加班時段別--%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME_TIME_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_TIME_CD%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_OVERTIME_TIME_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        </td>
                                        <%--申請單號--%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IFLOW_NO%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_IFLOW_NO" runat="server" MaxLength="18" Width="150px" ClientIDMode="Static"></asp:TextBox>

                                        </td>
                                    </tr>

                                    <tr>
                                        <%--核准年月--%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_IFLOW_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IFLOW_APPROVE_DT%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_IFLOW_APPROVE_DT" runat="server" MaxLength="15" Width="100px" ClientIDMode="Static" CssClass="date2"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_IFLOW_APPROVE_DATE%>" ControlToValidate="txt_IFLOW_APPROVE_DT" ForeColor="Red"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        </td>
                                        <%--刷卡比對狀態--%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_CHECK_STATUS" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CHECK_STATUS%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_CHECK_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        </td>
                                        <%--加班特殊狀況--%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_O_SPECIAL_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_O_SPECIAL_CD%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_O_SPECIAL_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                <aces:Btn ID="WFB2DI0500Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0500Search%>" OnClick="WFB2DI0500Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();"/>

                                <%--<asp:Button ID="WFB2DI0500Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0500Search%>" OnClick="WFB2DI0500Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();"/>--%>
                                
                                <input id="WFB2DI0500Clear" runat="server" type="button" value="<%$Resources:Resource,wfb2di_WFB2DI0500Clear%>" onclick="ClearAll();" />
                                
                                <%--<aces:Btn ID="WFB2DI0500Batch" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0500Batch%>" OnClick="WFB2DI0500Batch_Click" />--%>
                                
                                <%--<asp:Button ID="WFB2DI0500Batch" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0500Batch%>" OnClick="WFB2DI0500Batch_Click" />--%>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="10">
                            <div id="init_add" style="display: inline">
                                <aces:Btn ID="WFB2DI0500Add" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Add%>" Visible="true" OnClick="WFB2DI0500Add_Click" />

                                <%--<asp:Button ID="WFB2DI0500Add" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Add%>" Visible="true" OnClick="WFB2DI0500Add_Click" />--%>
                            </div>
                            <div id="init_grid" style="display: inline">
                            <aces:Btn ID="WFB2DI0500Delete" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Delete%>" Visible="false" OnClick="WFB2DI0500Delete_Click" OnClientClick="return CheckDelAction();" />
                                <aces:Btn ID="WFB2DI0500Edit" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Edit%>" OnClick="WFB2DI0500Edit_Click" Visible="False" />
                                <aces:Btn ID="WFB2DI0500Detail" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Detail%>" OnClick="WFB2DI0500Detail_Click" Visible="False" />

                              <%--  <asp:Button ID="WFB2DI0500Delete" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Delete%>" Visible="false" OnClick="WFB2DI0500Delete_Click" OnClientClick="return CheckDelAction();" />
                                <asp:Button ID="WFB2DI0500Edit" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Edit%>" OnClick="WFB2DI0500Edit_Click" Visible="False" />
                                <asp:Button ID="WFB2DI0500Detail" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Detail%>" OnClick="WFB2DI0500Detail_Click" Visible="False" />--%>
                            <%--<asp:Button ID="WFB2DI0500Confirm" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0500Confirm%>" OnClick="WFB2DI0500Confirm_Click" OnClientClick="BlockUI();" />--%>
                            </div>

                        </td>
                    </tr>
                    <!-- END: Create a line -->
                </tbody>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DI0500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_APPLY_OVERTIME_DT_S"
                        Name="apply_overtime_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_APPLY_OVERTIME_DT_E"
                        Name="apply_overtime_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME" DefaultValue=""
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="dept_no" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="ddl_OVERTIME_CD" DefaultValue=""
                        Name="overtime_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_OVERTIME_TIME_CD" DefaultValue=""
                        Name="overtime_time_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_IFLOW_NO" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="iflow_no" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_IFLOW_APPROVE_DT" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="iflow_approve_dt" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="ddl_CHECK_STATUS" DefaultValue=""
                        Name="check_status" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_FORM_STATUS" DefaultValue=""
                        Name="form_status" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_O_SPECIAL_CD" DefaultValue=""
                        Name="o_special_cd" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1500px"
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
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                    <%--工號 --%>
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2di_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                    <%--姓名 --%>
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2di_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center" />
                    <%--勤務日期 --%>
                    <asp:BoundField DataField="APPLY_OVERTIME_DT" HeaderText="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_DT%>" SortExpression="APPLY_OVERTIME_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <%--加班類型 --%>
                    <asp:BoundField DataField="OVERTIME_DESC" HeaderText="<%$Resources:Resource,wfb2di_lb_OVERTIME_CD%>" SortExpression="OVERTIME_DESC" HeaderStyle-Width="160px" ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                    <%--加班出勤別 --%>
                    <asp:BoundField DataField="OVERTIME_DT_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2di_lb_OVERTIME_DT_TYPE3%>" SortExpression="OVERTIME_DT_TYPE" HeaderStyle-Width="160px" ItemStyle-Width="160px" ItemStyle-HorizontalAlign="Left" />
                    <%--勤務日期類型 --%>
                    <asp:BoundField DataField="DT_TYPE_DESC" HeaderText="<%$Resources:Resource,wfb2di_lb_DT_TYPE2%>" SortExpression="DT_TYPE" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                    <%--申請總時數 --%>
                    <asp:BoundField DataField="APPLY_OVERTIME_HOUR" HeaderText="<%$Resources:Resource,wfb2di_APPLY_OVERTIME_HOUR%>" SortExpression="APPLY_OVERTIME_HOUR" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <%--核准總時數 --%>
                    <asp:BoundField DataField="APPROVE_OVERTIME_HOUR" HeaderText="<%$Resources:Resource,wfb2di_APPROVE_OVERTIME_HOUR2%>" SortExpression="APPROVE_OVERTIME_HOUR" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <%--計算總時數 --%>
                    <asp:BoundField DataField="OVERTIME_PAY_HOUR" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_PAY_HOUR2%>" SortExpression="OVERTIME_PAY_HOUR" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <%--勤前時數 --%>
                    <asp:BoundField DataField="BEFORE_HOUR" HeaderText="<%$Resources:Resource,wfb2di_BEFORE_HOUR%>" SortExpression="BEFORE_HOUR" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                    <%--勤前起迄時間 --%>
                    <asp:BoundField DataField="BSETIME" HeaderText="<%$Resources:Resource,wfb2di_BEFORE_SETIME%>"                             HeaderStyle-Width="120px" ItemStyle-Width="120px" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <%--勤後時數 --%>
                    <asp:BoundField DataField="AFTER_HOUR" HeaderText="<%$Resources:Resource,wfb2di_AFTER_HOUR%>" SortExpression="AFTER_HOUR" HeaderStyle-Width="80px" ItemStyle-Width="80px" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <%--勤後起迄時間 --%>
                    <asp:BoundField DataField="ASETIME" HeaderText="<%$Resources:Resource,wfb2di_AFTER_SETIME%>"                              HeaderStyle-Width="120px" ItemStyle-Width="120px" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <%--核准年月 --%>
                    <asp:BoundField DataField="IFLOW_APPROVE_DT" HeaderText="<%$Resources:Resource,wfb2di_lb_IFLOW_APPROVE_DT%>" SortExpression="IFLOW_APPROVE_DT" HeaderStyle-Width="90px" ItemStyle-Width="90px"  HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <%--是否刷卡比對 --%>
                    <asp:BoundField DataField="IS_DUTY_CHECK" HeaderText="<%$Resources:Resource,wfb2di_lb_IS_DUTY_CHECK%>" SortExpression="IS_DUTY_CHECK" HeaderStyle-Width="100px" ItemStyle-Width="100px" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <%--刷卡比對狀態 --%>
                    <asp:BoundField DataField="CHECK_STATUS" HeaderText="<%$Resources:Resource,wfb2di_lb_CHECK_STATUS%>" SortExpression="CHECK_STATUS" HeaderStyle-Width="100px" ItemStyle-Width="100px" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <%--表單狀態 --%>
                    <asp:BoundField DataField="FORM_STATUS" HeaderText="<%$Resources:Resource,wfb2di_lb_FORM_STATUS%>" SortExpression="FORM_STATUS" HeaderStyle-Width="100px" ItemStyle-Width="100px" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <%--申請單號 --%>
                    <asp:BoundField DataField="IFLOW_NO" HeaderText="<%$Resources:Resource,wfb2di_lb_IFLOW_NO%>" SortExpression="IFLOW_NO" HeaderStyle-Width="160px" ItemStyle-Width="160px" HtmlEncode="false" ItemStyle-HorizontalAlign="Left" />
                    <%--部門 --%>
                    <asp:BoundField DataField="DEPT_NAME" HeaderText="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />

                </Columns>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
            </asp:GridView>

            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
</asp:Content>

