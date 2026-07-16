<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0700_Qry.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0700_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".date1").datepicker({ dateFormat: 'yy/mm' });
            $(".date1").mask('9999/99');

            $("#txt_EMP_NAME").attr("readonly", true);
            $("#txt_DEPT_NAME").attr("readonly", true);
            $("#txt_OVERTIME_CTL_HOUR").attr("readonly", true);
            $("#txt_OVERTIME1").attr("readonly", true);
            $("#txt_OVERTIME2").attr("readonly", true);
            $("#txt_OVERTIME3").attr("readonly", true);
            $("#txt_OVERTIME_TOTAL").attr("readonly", true);
            $("#txt_OVERTIME4").attr("readonly", true);
            $("#txt_OVERTIME5").attr("readonly", true);
            $("#txt_OVERTIME6").attr("readonly", true);
            $("#txt_OVERTIME7").attr("readonly", true);
            $("#txt_OVERTIME8").attr("readonly", true);

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
                barcolor: "#7F7F7F",
                freezesize: 5
            });
        }

        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val($("#hid_defalut_EMP_ID").val());
            $("#txt_EMP_NAME").val($("#hid_defalut_EMP_NAME").val());
            $("#txt_DEPT_NAME").val($("#hid_defalut_DEPT_NAME").val());
            $("#txt_OVERTIME_DT_YM").val("");
            $("#txt_OVERTIME_DT_S").val("");
            $("#txt_OVERTIME_DT_E").val("");


        }

        function getEmp() {
            OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N', 'Y');

            //var json = OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');
            //if (json != undefined) {
            //    $("#txt_DEPT_NAME").val(json.DEPT_NAME);
            //}

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
                $('#' + ename).val(obj.EMP_NAME.trim());
                $("#txt_DEPT_NAME").val(obj.DEPT_NAME);
            }
        }

        function CheckSearch() {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                if ($('#rb_date1').prop('checked')) {
                    if ($("#txt_OVERTIME_DT_YM").val() == "") {
                        alert("選擇年月不可輸入空白");
                        processed = false;
                        return false;
                    }
                }

                if ($('#rb_date2').prop('checked')) {
                    if ($("#txt_OVERTIME_DT_S").val() == "" || $("#txt_OVERTIME_DT_E").val() == "") {
                        alert("選擇起迄日期不可輸入空白");
                        return false;
                    }
                }
                BlockUI();
            }
            else
                return false;

            if (!processed)
                $.unblockUI();

            return processed;
        }

        function openChangeLeave() {
            window.open("WFB2DI0700_ChangeLeave.aspx?emp_id=" + $("#txt_EMP_ID").val() +
                "&overtime_dt_ym=" + $("#txt_OVERTIME_DT_YM").val() + "&overtime_dt_s=" + $("#txt_OVERTIME_DT_S").val() +
                "&overtime_dt_e=" + $("#txt_OVERTIME_DT_E").val() + "&parentFuncId=" + parentFuncID);
            //window.showModalDialog("WFB2DI0700_ChangeLeave.aspx?emp_id=" + $("#txt_EMP_ID").val() +
            //    "&overtime_dt_ym=" + $("#txt_OVERTIME_DT_YM").val() + "&overtime_dt_s=" + $("#txt_OVERTIME_DT_S").val() +
            //    "&overtime_dt_e=" + $("#txt_OVERTIME_DT_E").val() + "&parentFuncId=" + parentFuncID, self, 'dialogWidth=900px;dialogHeight=400px;scroll=auto');
        }


        function getEmpName(EMP_id) {
            var txt = document.getElementById(EMP_id);
            if (txt.value != "") {
                document.getElementById('hid_getEmpName').click();
                return false;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                            <colgroup>
                                <col width="12%" />
                                <col width="14%" />
                                <col width="12%" />
                                <col width="14%" />
                                <col width="12%" />
                                <col width="14%" />
                                <col width="24%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%-- 工號 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_ID%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField" onblur="javascript:getEmpName(this.id)"></asp:TextBox>
                                        <input id="bt_EMP_ID" type="button" value="..." onclick="getEmp();" />
                                        <asp:RequiredFieldValidator ID="MainValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_required_EMP_ID%>"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <%-- 姓名 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_NAME%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%-- 部門 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NAME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NAME%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td align="left" class="Body_label"></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                            <colgroup>
                                <col width="12%" />
                                <col width="57%" />
                                <col width="12%" />
                                <col width="19%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%-- 勤務期間 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPLY_OVERTIME_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_DT%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <%-- 年月 --%>
                                        <asp:RadioButton ID="rb_date1" runat="server" GroupName="date" Text="<%$Resources:Resource,wfb2di_lb_YM%>" Checked="true" ClientIDMode="Static" />：
					                    <asp:TextBox ID="txt_OVERTIME_DT_YM" runat="server" MaxLength="7" Width="81px" ClientIDMode="Static" CssClass="date1"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_OVERTIME_DT_YM%>" ControlToValidate="txt_OVERTIME_DT_YM" ForeColor="Red"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        <%-- 起迄日期 --%>
                                        <asp:RadioButton ID="rb_date2" runat="server" GroupName="date" Text="<%$Resources:Resource,wfb2di_lb_DT_SE%>" ClientIDMode="Static" />：
					                    <asp:TextBox ID="txt_OVERTIME_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        ~
                                        <asp:TextBox ID="txt_OVERTIME_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                            
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_OVERTIME_DT_S"
                                            ControlToValidate="txt_OVERTIME_DT_E" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_OVERTIME_DT_SE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_OVERTIME_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_OVERTIME_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_OVERTIME_DT_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_OVERTIME_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                                    </td>
                                    <td align="left" class="Body_label"></td>
                                    <td align="left" class="Body_label"></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                            <colgroup>
                                <col width="12%" />
                                <col width="58%" />
                                <col width="30%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <td align="left" class="Body_label"></td>
                                    <td align="left" class="Body_label"></td>
                                    <td align="right" class="Body_label">
                                        <div id="init">

                                            <aces:Btn ID="WFB2DI0700Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0700Search%>" OnClick="WFB2DI0700Search_Click" OnClientClick="return CheckSearch();" formnovalidate />
                                            <aces:Btn ID="WFB2SC4101OVERSearch" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0700Search%>" OnClick="WFB2DI0700Search_Click" OnClientClick="return CheckSearch();" formnovalidate />
                                            <aces:Btn ID="WFB2DI0601Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0700Search%>" OnClick="WFB2DI0700Search_Click" OnClientClick="return CheckSearch();" formnovalidate />
                                            <aces:Btn ID="WFB2DI0503Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0700Search%>" OnClick="WFB2DI0700Search_Click" OnClientClick="return CheckSearch();" formnovalidate />
                                            <%--
                                    <asp:Button ID="WFB2DI0700Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0700Search%>" OnClick="WFB2DI0700Search_Click" OnClientClick="return CheckSearch();" formnovalidate />
                                            --%>
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2di_btn_clear%>" onclick="ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" height="1" colspan="3">
                                        <hr />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="YYMMId" cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="11%" />
                                <col width="12%" />
                                <col width="11%" />
                                <col width="11%" />
                                <col width="11%" />
                                <col width="11%" />
                                <col width="12%" />
                                <col width="13%" />
                            </colgroup>

                            <tbody style="height: 25px;">
                                <tr>
                                    <%--加班管制時數 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME_CTL_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_CTL_HOUR%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME_CTL_HOUR" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--實績合計時數-平日加班 --%>
                                    <td align="right" class="Body_label">
                                        <asp:Label ID="lb_OVERTIME_TITLE1" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_TITLE1%>"></asp:Label>:</td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME1" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME1%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME1" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--實績合計時數-已換休 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME2" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME2" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--實績合計時數-平日加班剩餘 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME3" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME3%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME3" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--加班累計時數(一般) --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME_TOTAL_NORMAL" runat="server" Text="<%$Resources:Resource,wfb2di_OVERTIME_TOTAL_NORMAL%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME_TOTAL_NORMAL" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td align="right" class="Body_label"></td>
                                    <%--實績合計時數-假日加班 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME4" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME4%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME4" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--實績合計時數-已申告 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME5" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME5%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME5" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--實績合計時數-假日加班剩餘 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME6" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME6%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME6" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--加班累計時數(三高) --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME_TOTAL_HYPER" runat="server" Text="<%$Resources:Resource,wfb2di_OVERTIME_TOTAL_HYPER%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME_TOTAL_HYPER" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--申請中總時數-平日加班 --%>
                                    <td align="right" class="Body_label">
                                        <asp:Label ID="lb_APPLY_OVERTIME_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_HOUR_ING%>"></asp:Label>:</td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME7" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME7%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME7" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td align="left" class="Body_label"></td>
                                    <td align="left" class="Body_label"></td>
                                    <td align="left" class="Body_label">
                                        <aces:Btn ID="WFB2DI0700Detail" runat="server" type="button" Text="<%$Resources:Resource,wfb2di_btn_detail%>" OnClick="btn_detail_Click" OnClientClick="return CheckSearch();" formnovalidate />
                                        <aces:Btn ID="WFB2SC4101OVERDetail" runat="server" type="button" Text="<%$Resources:Resource,wfb2di_btn_detail%>" OnClick="btn_detail_Click" OnClientClick="return CheckSearch();" formnovalidate />
                                        <aces:Btn ID="WFB2DI0601Detail" runat="server" type="button" Text="<%$Resources:Resource,wfb2di_btn_detail%>" OnClick="btn_detail_Click" OnClientClick="return CheckSearch();" formnovalidate />
                                        <aces:Btn ID="WFB2DI0502Detail" runat="server" type="button" Text="<%$Resources:Resource,wfb2di_btn_detail%>" OnClick="btn_detail_Click" OnClientClick="return CheckSearch();" formnovalidate />
                                        <%--<asp:Button id="WFB2DI0700Detail" runat="server" type="button" Text="<%$Resources:Resource,wfb2di_btn_detail%>" OnClick="btn_detail_Click" OnClientClick="return CheckSearch();" formnovalidate />--%>
                                        <%--<input id="btn_detail" runat="server" type="button" value="<%$Resources:Resource,wfb2di_btn_detail%>" onclick="openChangeLeave();" />--%>
                                    </td>
                                    <td align="left" class="Body_label"></td>
                                </tr>
                                <tr>
                                    <%--代休出勤時數 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME_TOTAL_D" runat="server" Text="<%$Resources:Resource,wfb2di_OVERTIME_TOTAL_D%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME_TOTAL_D" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td align="right" class="Body_label"></td>
                                    <%--申請中總時數-假日加班 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME8" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME8%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:TextBox ID="txt_OVERTIME8" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DI0700DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="emp_id" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_OVERTIME_DT_YM" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="overtime_dt_ym" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_OVERTIME_DT_S" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="overtime_dt_s" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_OVERTIME_DT_E" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="overtime_dt_e" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="rb_date1" DefaultValue=""
                        Name="date1" PropertyName="Checked" Type="Boolean" />
                    <asp:ControlParameter ControlID="rb_date2" DefaultValue=""
                        Name="date2" PropertyName="Checked" Type="Boolean" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>" HeaderStyle-Width="30px" ItemStyle-Width="30px"  />
                    <%-- 日期 --%>
                    <asp:BoundField DataField="APPLY_OVERTIME_DT" HeaderText="<%$Resources:Resource,wfb2di_APPLY_OVERTIME_DT%>" SortExpression="APPLY_OVERTIME_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <%-- 班別 --%>
                    <asp:BoundField DataField="SHIFT_CD" HeaderText="<%$Resources:Resource,wfb2di_SHIFT_CD%>" SortExpression="SHIFT_CD" HeaderStyle-Width="240px" ItemStyle-Width="240px"  ItemStyle-HorizontalAlign="Left" />
                    <%-- 加班類型--%>
                    <asp:BoundField DataField="OVERTIME_CD" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_CD%>" SortExpression="OVERTIME_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Left" />
                    <%-- 日期類型--%>
                    <asp:BoundField DataField="OVERTIME_DT_TYPE" HeaderText="<%$Resources:Resource,wfb2db_DT_TYPE%>" SortExpression="OVERTIME_DT_TYPE" HeaderStyle-Width="100px" ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Left" />
                    <%-- 申告換休--%>
                    <asp:BoundField DataField="IS_APPLY" HeaderText="<%$Resources:Resource,wfb2di_lb_IS_APPLY%>" SortExpression="IS_APPLY" HeaderStyle-Width="50px" ItemStyle-Width="50px"  ItemStyle-HorizontalAlign="Center" />
                    <%-- 一般累計--%>
                    <asp:BoundField DataField="NORMAL_HOUR" HeaderText="<%$Resources:Resource,wfb2di_lb_NORMAL_HOUR%>" SortExpression="NORMAL_HOUR" HeaderStyle-Width="50px" ItemStyle-Width="50px"  ItemStyle-HorizontalAlign="Right" />
                    <%-- 三高累計--%>
                    <asp:BoundField DataField="HYPER_HOUR" HeaderText="<%$Resources:Resource,wfb2di_lb_HYPER_HOUR%>" SortExpression="HYPER_HOUR" HeaderStyle-Width="50px" ItemStyle-Width="50px"  ItemStyle-HorizontalAlign="Right" />
                    <%-- 勤務上班時間 --%>
                    <asp:BoundField DataField="DUTY_STIME" HeaderText="<%$Resources:Resource,wfb2di_DUTY_STIME%>" SortExpression="DUTY_STIME" HeaderStyle-Width="70px" ItemStyle-Width="70px"  HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <%-- 勤務下班時間 --%>
                    <asp:BoundField DataField="DUTY_ETIME" HeaderText="<%$Resources:Resource,wfb2di_DUTY_ETIME%>" SortExpression="DUTY_ETIME" HeaderStyle-Width="70px" ItemStyle-Width="70px"  HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <%-- 刷卡上班時間 --%>
                    <asp:BoundField DataField="CLOCK_IN_DT" HeaderText="<%$Resources:Resource,wfb2di_CLOCK_IN_DT%>" SortExpression="CLOCK_IN_DT" HeaderStyle-Width="70px"  ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <%-- 刷卡下班時間 --%>
                    <asp:BoundField DataField="CLOCK_OUT_DT" HeaderText="<%$Resources:Resource,wfb2di_CLOCK_OUT_DT%>" SortExpression="CLOCK_OUT_DT" HeaderStyle-Width="70px"  ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <%-- 計薪狀態 --%>
                    <asp:BoundField DataField="SALARY_SETTLE_STATUS" HeaderText="<%$Resources:Resource,wfb2di_lb_SALARY_SETTLE_STATUS%>" SortExpression="SALARY_SETTLE_STATUS" HeaderStyle-Width="80px"  ItemStyle-Width="80px" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <%-- 勤前起迄時間 --%>
                    <asp:BoundField DataField="BEFORE_TIME" HeaderText="<%$Resources:Resource,wfb2di_lb_BEFORE_TIME%>"   HeaderStyle-Width="120px"  ItemStyle-Width="120px" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <%-- 勤後起迄時間 --%>
                    <asp:BoundField DataField="AFTER_TIME" HeaderText="<%$Resources:Resource,wfb2di_lb_AFTER_TIME%>"  HeaderStyle-Width="120px"  ItemStyle-Width="120px" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                    <%-- 滯廠時數 --%>
                    <asp:BoundField DataField="VIOLATE_RULE_HOUR" HeaderText="<%$Resources:Resource,wfb2di_VIOLATE_RULE_HOUR%>" HeaderStyle-Width="70px" ItemStyle-Width="70px"  ItemStyle-HorizontalAlign="Right" />
                    <%-- 勤前申請時數 --%>
                    <asp:BoundField DataField="BEFORE_HOUR" HeaderText="<%$Resources:Resource,wfb2di_BEFORE_HOUR%>" SortExpression="BEFORE_HOUR" HeaderStyle-Width="70px"  ItemStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <%-- 勤後申請時數 --%>
                    <asp:BoundField DataField="AFTER_HOUR" HeaderText="<%$Resources:Resource,wfb2di_AFTER_HOUR%>" SortExpression="AFTER_HOUR" HeaderStyle-Width="70px" ItemStyle-Width="70px"  ItemStyle-HorizontalAlign="Right" HtmlEncode="false" />
                    <%-- 加班核淮時數 --%>
                    <asp:BoundField DataField="APPROVE_OVERTIME_HOUR" HeaderText="<%$Resources:Resource,wfb2di_APPROVE_OVERTIME_HOUR%>" SortExpression="APPROVE_OVERTIME_HOUR" HeaderStyle-Width="70px" ItemStyle-Width="70px"  HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <%-- 加班計算時數 --%>
                    <asp:BoundField DataField="OVERTIME_PAY_HOUR" HeaderText="<%$Resources:Resource,wfb2di_OVERTIME_PAY_HOUR%>" SortExpression="OVERTIME_PAY_HOUR" HeaderStyle-Width="70px" ItemStyle-Width="70px"  HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
                    <%-- IFLOW單號 --%>
                    <asp:BoundField DataField="IFLOW_NO" HeaderText="<%$Resources:Resource,wfb2di_IFLOW_NO%>" SortExpression="IFLOW_NO" HeaderStyle-Width="150px" ItemStyle-Width="150px"  HtmlEncode="false" ItemStyle-HorizontalAlign="Left" />
                    <%-- 備註 --%>
                    <asp:BoundField DataField="REMARK" HeaderText="<%$Resources:Resource,wfb2di_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="200px" ItemStyle-Width="200px"  ItemStyle-HorizontalAlign="Left" />
                </Columns>

                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
            </asp:GridView>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:Button ID="hid_getEmpName" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getEmpName_Click" />
            <!--預設的工號,姓名-->
            <asp:HiddenField ID="hid_defalut_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_defalut_EMP_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_defalut_DEPT_NAME" runat="server" ClientIDMode="Static" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

