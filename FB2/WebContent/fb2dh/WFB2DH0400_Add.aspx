<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0400_Add.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0400_Add" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm(); 
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_HEAD_EMP_ID").mask("99999");
            $(".number2").mask("999");
            $(".date").mask("9999/99/99");
            $("#txt_DD").mask("999");
            $("#txt_HH").mask("999");
            $("#txt_MM").mask("999");
            $('#txt_HEAD_EMP_NAME').attr("readonly", true);
            $('#txt_DEPT_NAME').attr("readonly", true);
            $('#txt_MAIN_LEAVE_DESC').attr("readonly", true);
            $('#txt_CHECK_STATUS').attr("readonly", true);
            $('#txt_SALARY_SETTLE_STATUS').attr("readonly", true);
            $('#txt_SALARY_GIVE_DT').attr("readonly", true);
            $('#txt_IFLOW_NO').attr("readonly", true);
            $('#txt_FORM_STATUS').attr("readonly", true);


            $("#txt_MAIN_LEAVE_CD").change(function () {
                $("#txt_MAIN_LEAVE_CD").val($("#txt_MAIN_LEAVE_CD").val().toUpperCase());
            });

            $.unblockUI();
        }//月曆


        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function getEmp() {
            var json = OpenEmpSearch('txt_HEAD_EMP_ID', 'txt_HEAD_EMP_NAME', 'N');
            if (json != undefined) {
                $("#txt_DEPT_NAME").val(json.DEPT_NAME)
                $("#hid_DEPT_NO").val(json.DEPT_NO)
            }

            //alert("所屬部門代號:" + json.DEPT_NO + "\n" + "所屬部門名稱:" + json.DEPT_NAME + "\n" +
            //    "員工區分:" + json.EMP_CD + "\n" + "資格代號:" + json.LEVEL_CD + "\n" +
            //    "級數代號:" + json.GRADE_CD + "\n" + "職務代號:" + json.PJOB_CD + "\n" +
            //    "員工區分:" + json.JOIN_DT + "\n" + "資格代號:" + json.BE_EMP_DT + "\n" +
            //    "職種:" + json.WS_CD + "\n" + "在職狀態:" + json.EMP_STATUS + "\n");
        }

        function getMAIN_LEAVE_CD() {
            var json = OpenSearch('LeaveType_Search.aspx', 'txt_MAIN_LEAVE_CD', 'txt_MAIN_LEAVE_DESC', '', 'Y');

        }

        function LeaveType_Search(id, name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#txt_MAIN_LEAVE_CD").val(obj.CD);
                $("#txt_MAIN_LEAVE_DESC").val(obj.DESC);
                __doPostBack('Main_OnTextChanged', '');
                //return obj;
            }
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;
            if (!Page_ClientValidate("GroupA")) {
                processed = false;
            }

            if(!Page_ClientValidate("GroupB"))
                processed = false;

            if (processed) {
                if ($("#txt_MAIN_LEAVE_CD").val() == "R") {
                    if ($("#txt_APPLY_OVERTIME_DT").val() == "") {
                        alert("請代休假時，需填入代休加班日");
                        processed = false;
                    }
                }
            }

            if (processed)
                BlockUI();
            if (!processed)
                $.unblockUI();

            return processed;
        }

        function openQryLeave(url) {
            window.open(url + "&parentFuncId=" + parentFuncID);
        }
        function backToQry() {
            window.location.href = "WFB2DH0400_Qry.aspx";
        }
        function Checkconfirm(message) {
            if (confirm(message))
                backToQry();

            return false;
        }
    </script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <%--查詢條件欄位寬度--%>
                <colgroup>
                    <col width="12%" />
                    <col width="21%" />
                    <col width="12%" />
                    <col width="21%" />
                    <col width="12%" />
                    <col width="22%" />
                </colgroup>
                <tbody>
                    <%--工號、姓名、部門--%>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <%--<asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="6" class="MandatoryField" Width="42px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />--%>
                            <asp:TextBox ID="txt_HEAD_EMP_ID" runat="server" MaxLength="5" Width="64px" CssClass="MandatoryField" ClientIDMode="Static" OnTextChanged="txt_HEAD_EMP_ID_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<% $Resources:Resource,wfb2dh_Required_EMP_ID %>"
                                ControlToValidate="txt_HEAD_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>

                            <input id="bt_HEAD_EMP_ID" type="button" value="..." onclick="getEmp();" />
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_HEAD_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DEPT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DEPT_NAME" runat="server" MaxLength="13" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>

                            <!--	<input id='' type="text" name="strdate"  maxlength="10" size="10" value="" /> -->
                        </td>
                    </tr>
                    <%--請假申請--%>
                    <tr>
                        <td>
                            <asp:Label ID="lb_Leave_Apply" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Leave_Apply%>"></asp:Label>:
                        </td>
                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_MAIN_LEAVE_CD" runat="server" CssClass="MandatoryField" MaxLength="2" class="MandatoryField" Width="42px" ClientIDMode="Static" OnTextChanged="txt_MAIN_LEAVE_CD_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_MAIN_LEAVE_CD %> "
                                ControlToValidate="txt_MAIN_LEAVE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <input id="Button1" type="button" value="..." onclick="getMAIN_LEAVE_CD();" />
                            <asp:TextBox ID="txt_MAIN_LEAVE_DESC" runat="server" MaxLength="10" Width="100px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SUB_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_SUB_LEAVE_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static" OnSelectedIndexChanged="ddl_SUB_LEAVE_CD_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_SUB_LEAVE_CD %> "
                                ControlToValidate="ddl_SUB_LEAVE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>

                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LEAVE_TIME_UNIT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_TIME_UNIT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_LEAVE_MIN_VALUE" runat="server" BorderWidth="0" MaxLength="6" Width="42px" ClientIDMode="Static"></asp:Label>
                            <asp:Label ID="txt_LEAVE_TIME_UNIT" runat="server" BorderWidth="0" MaxLength="6" Width="42px" ClientIDMode="Static"></asp:Label>
                            <asp:HiddenField ID="hid_LEAVE_TIME_UNIT" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <%-- 請假勤務起 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_SDT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_SDT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_APPLY_LEAVE_SDT" runat="server" CssClass="MandatoryField date" MaxLength="10" Width="81px" ClientIDMode="Static" OnTextChanged="txt_APPLY_LEAVE_SDT_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="Unknown1" runat="server" ErrorMessage="<% $Resources:Resource,wfb2dh_Required_APPLY_LEAVE_SDT2 %> "
                                ControlToValidate="txt_APPLY_LEAVE_SDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_errformat_APPLY_LEAVE_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_LEAVE_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>

                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_EDT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_EDT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_APPLY_LEAVE_EDT" runat="server" CssClass="MandatoryField date" MaxLength="10" Width="81px" ClientIDMode="Static" OnTextChanged="txt_APPLY_LEAVE_EDT_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<% $Resources:Resource,wfb2dh_Required_APPLY_LEAVE_EDT2 %> "
                                ControlToValidate="txt_APPLY_LEAVE_EDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_APPLY_LEAVE_SDT"
                                ControlToValidate="txt_APPLY_LEAVE_EDT" ErrorMessage="<%$Resources:Resource,wfb2dh_APPLY_LEAVE_DT_Compare%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                            <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_errformat_APPLY_LEAVE_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_LEAVE_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>
                        <%-- 計算 --%>
                        <td align="left" class="Body_label">
                            <aces:Btn ID="WFB2DH0403Calculate" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Calculate%>" OnClick="WFB2DH0400Calculate_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                        </td>
                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_STIME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_STIME%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_hours" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_SH %> "
                                ControlToValidate="ddl_hours" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:Label ID="lb_Hour" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Hour%>"></asp:Label>
                            <asp:DropDownList ID="ddl_minutes" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_SM %> "
                                ControlToValidate="ddl_minutes" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:Label ID="lb_Min" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Min%>"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_LEAVE_ETIME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_LEAVE_ETIME%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_hours2" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_EH %> "
                                ControlToValidate="ddl_hours2" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:Label ID="lb_Hour2" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Hour%>"></asp:Label>
                            <asp:DropDownList ID="ddl_minutes2" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_EM %> "
                                ControlToValidate="ddl_minutes2" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:Label ID="lb_Min2" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Min%>"></asp:Label>
                        </td>
                        <%-- 請假合計 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_TOTAL_TIME_APPROVE" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_TOTAL_TIME_APPROVE%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DD" runat="server" CssClass="MandatoryField" Columns="1" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_DD %> "
                                ControlToValidate="txt_DD" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:Label ID="lb_Date" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Date%>"></asp:Label>

                            <asp:TextBox ID="txt_HH" runat="server" CssClass="MandatoryField" Columns="1" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_HH %> "
                                ControlToValidate="txt_HH" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:Label ID="lb_Hour3" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Hour%>"></asp:Label>

                            <asp:TextBox ID="txt_MM" runat="server" CssClass="MandatoryField" Columns="1" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_Required_MM %> "
                                ControlToValidate="txt_MM" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:Label ID="lb_Min3" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Min%>"></asp:Label>

                            <!--	0 日 8 時 0 分	-->
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_FACT_HAPPEN_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_FACT_HAPPEN_DT%>"></asp:Label>:
                        </th>

                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_FACT_HAPPEN_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_errformat_FACT_HAPPEN_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_FACT_HAPPEN_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_APPLY_OVERTIME_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_APPLY_OVERTIME_DT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="3">
                            <asp:TextBox ID="txt_APPLY_OVERTIME_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_errformat_APPLY_OVERTIME_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_APPLY_OVERTIME_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:Label ID="lb_Must" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_Must%>"></asp:Label>
                        </td>

                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LEAVE_REASON" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_REASON%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="5">
                            <asp:TextBox ID="txt_LEAVE_REASON" runat="server" TextMode="MultiLine" MaxLength="6" Columns="80" ClientIDMode="Static"></asp:TextBox>

                        </td>
                    </tr>

                    <tr>
                        <td align="left" class="Body_label">
                            <aces:Btn ID="WFB2DH0403LeaveCdSearch" runat="server" Text="<%$Resources:Resource,wfb2dh_btn_LEAVECOUNT%>" OnClick="wfb2dh_LEAVE_CD_Search_Click" />
                            <%--<asp:Button ID="WFB2DH0403LeaveCdSearch" runat="server" Text="<%$Resources:Resource,wfb2dh_btn_LEAVECOUNT%>" OnClick="wfb2dh_LEAVE_CD_Search_Click" />--%>
                        </td>

                        <td align="left" class="Body_label"></td>
                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IFLOW_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IFLOW_APPROVE_DT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_IFLOW_APPROVE_DT" Text="" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_errformat_IFLOW_APPROVE_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_IFLOW_APPROVE_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SALARY_GIVE_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SALARY_GIVE_DT%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_SALARY_GIVE_DT" runat="server" Text="" BorderWidth="0" MaxLength="6" Width="81px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IFLOW_NO%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_IFLOW_NO" runat="server" Text="" BorderWidth="0" MaxLength="15" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CHECK_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_CHECK_STATUS%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_CHECK_STATUS" Text="<%$Resources:Resource,wfb2dh_lb_CHECK_STATUS_N%>" runat="server" BorderWidth="0" MaxLength="6" Width="81px" ClientIDMode="Static"></asp:TextBox>
                            <%-- 
                            <asp:DropDownList ID="ddl_IS_CONFIRM_CHECK" runat="server" ClientIDMode="Static" Visible="false"></asp:DropDownList>
                            --%>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SALARY_SETTLE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SALARY_SETTLE_STATUS%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_SALARY_SETTLE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SALARY_SETTLE_STATUS_N%>" BorderWidth="0" MaxLength="6" Width="81px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_FORM_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_FORM_STATUS%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_FORM_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_FORM_STATUS_Y%>" BorderWidth="0" MaxLength="6" Width="81px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_REMARK%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label" colspan="5">
                            <asp:TextBox ID="txt_REMARK" runat="server" TextMode="MultiLine" MaxLength="6" Columns="80" ClientIDMode="Static"></asp:TextBox>

                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="6">
                            <aces:Btn ID="WFB2DH0403Save" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Save%>" OnClick="WFB2DH0400Save_Click" OnClientClick="return saveCheck();" />
                            <asp:Button ID="WFB2DH0400Cancel" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0400Cancel%>" OnClick="WFB2DH0400Cancel_Click" OnClientClick="return confirm('是否確定取消?');" /></td>
                    </tr>

                </tbody>
                <asp:HiddenField ID="hid_DEPT_NO" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hid_IS_INCLUDE_HOLIDAY" runat="server" ClientIDMode="Static" />
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
                <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
