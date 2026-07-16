<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dl/WFB2DL0100_Mod.aspx.cs" Inherits="WebContent_fb2dl_WFB2DL0100_Mod" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            initForm();
        });
        function initForm() {
            $('.date').datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $('.hour').mask('999.99');
            $('#txt_POLICY_PAY_LEAVE_DAY').mask('99');
            $.unblockUI();
        }
        function changeAVAILABLE_VALUE() {
            var approve_value=0;
            var used_pay_leave_value=0;
            var adjust_value=0;
            var deffer_value = 0;
            if ($('#txt_APPROVE_VALUE').val() != "") {
                approve_value = parseFloat($('#txt_APPROVE_VALUE').val(), 10);
            }
            else {
                approve_value = 0;
            }
            if ($('#txt_USED_PAY_LEAVE_VALUE').val() != "") {
                used_pay_leave_value = parseFloat($('#txt_USED_PAY_LEAVE_VALUE').val(), 10);
            }
            else {
                used_pay_leave_value = 0;
            }
            if ($('#txt_ADJUST_VALUE').val() != "") {
                adjust_value = parseFloat($('#txt_ADJUST_VALUE').val(), 10);
            }
            else {
                adjust_value = 0;
            }
            //遞延時數 txt_DEFFER_VALUE
            if ($('#txt_DEFFER_VALUE').val() != "") {
                deffer_value = parseFloat($('#txt_DEFFER_VALUE').val(), 10);
            }
            else {
                deffer_value = 0;
            }

            var result = approve_value - used_pay_leave_value + adjust_value + deffer_value;
            $('#lb_AVAILABLE_VALUE_txt').text(result);
            $('#hid_AVAILABLE_VALUE').val(result);
            var approve_day = Math.round((approve_value / 8)*10);
            $('#lb_APPROVE_VALUE_day').text(approve_day/10);
        }
        function checkNum(field) {
            var hour = $('#' + field).val();
            //var re = /^[0-9]|[0-9].[0-9]$/;
            if (hour != "") {
                if (!CheckIsNumber(hour)) {
                    $('#' + field).val("");
                    alert('請輸入數字(999.99)');
                }
                else {
                    changeAVAILABLE_VALUE();
                }
            }
            else
                changeAVAILABLE_VALUE();
        }

        
        function checkNumRange(field) {
            var hour = $('#' + field).val();
            if (hour != "") {
                if (!CheckIsNumber(hour)) {
                    $('#' + field).val("");
                    alert('請輸入數字');
                } else if (Math.abs(hour) > 240) {
                    alert('請輸入數字(正負240之間)');
                }
                else {
                    changeAVAILABLE_VALUE();
                }
            }
            else {
                changeAVAILABLE_VALUE();
            }
        }

        //當年結時,才需要檢查是否同年度,
        function checkSTART_DT(source, arguments) {
            var start = $("#txt_START_DT_S").val();
            var end = $("#txt_START_DT_E").val();
            var sscd = $("#ddl_SALARY_SETTLE_CD").val();
            if (sscd != "Y")
                return true;

            var startYear = start.slice(0, 4);
            var endYear = end.slice(0, 4);
            if (startYear == endYear)
                arguments.IsValid = true;
            else
                arguments.IsValid = false;
        }


        function checkADJUST_DESC(source, arguments) {
            var adjust_value = $("#txt_ADJUST_VALUE").val()
            if (adjust_value.trim() == "") {
                arguments.IsValid = true;
            }
            else if (Math.ceil($("#txt_ADJUST_VALUE").val()) == 0) {
                arguments.IsValid = true;
            }
            else {
                if ($("#txt_ADJUST_DESC").val() == "")
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
        }
        function checkField() {
            if ($("#txt_EMP_ID").val() != "" && $("#txt_BASE_YEAR").val() != "") {
                if (CheckBASE_YEAR())
                    return true;
                else {
                    $("#ddl_SUB_LEAVE_CD").val("");
                    alert($('#hidwfb2dl_BASE_YEAR_importError').val() + "(年度不可大於明年)");
                    return false;
                }
            }
            else {
                $("#ddl_SUB_LEAVE_CD").val("");
                alert($('#hidwfb2dl_EMP_ID_and_BASE_YEAR').val());
                return false;
            }
        }
        function CheckBASE_YEAR() {
            var today = new Date();
            var currentYear = today.getFullYear();
            var re = /^[0-9]{4}$/;
            if (re.test($("#txt_BASE_YEAR").val())) {
                if ($("#txt_BASE_YEAR").val() >= 1911 && $("#txt_BASE_YEAR").val() <= currentYear + 1) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }
        function ConfirmMessage() {
            if ($('#hid_state').val() == "detail") {
                return true;
            }
            else {
               return confirm($('#hidwfb2dl_cancel_ConfirmMessage').val())
            }
        }
        function openEMP_ID() {
            OpenEmpSearch('txt_EMP_ID', 'txt_qry_EMP_NAME', 'N','Y');

            
        }

        function returnEMPValueToPage(eid, ename, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            //alert(returnValue);
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                window.parent.$('#' + eid).val(obj.EMP_ID.trim());
                window.parent.$('#' + ename).val(obj.EMP_NAME.trim());
                __doPostBack('getEMP', '');
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="18%" />
                                <col width="25%" />
                                <col width="15%" />
                                <col width="16%" />
                                <col width="10%" />
                                <col width="16%" />
                            </colgroup>
                            <tbody>
                                <!-- START: 1st Line in Search Criteria area -->
                                <tr>
                                    <%--工號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dl_EMP_ID%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="80px" class="MandatoryField"
                                            ClientIDMode="Static" OnTextChanged="txt_EMP_ID_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="openEMP_ID();" />
                                        <asp:Label ID="lb_EMP_NAME" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_EMP_ID_importNull%>"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <%--部門代號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2dl_DEPT_NO%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:Label ID="lb_DEPT_NO_txt" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                                        <asp:Label ID="lb_DEPT_FULL_NAME" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--聘用單位--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2dl_COMPANY_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_COMPANY_CD_txt" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                    <%--員工區分--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2dl_EMP_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:Label ID="lb_EMP_CD_txt" runat="server" ClientIDMode="Static" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--核假年度--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BASE_YEAR" runat="server" Text="<%$Resources:Resource,wfb2dl_BASE_YEAR%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_BASE_YEAR" runat="server" MaxLength="4" Width="80px" class="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_BASE_YEAR_importNull%>"
                                            ControlToValidate="txt_BASE_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <%--計算年資--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CAL_WORK_YEAR" runat="server" Text="<%$Resources:Resource,wfb2dl_CAL_WORK_YEAR%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_CAL_WORK_YEAR_txt" runat="server" Width="100px" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <%--特休年度--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAY_LEAVE_YEAR" runat="server" Text="<%$Resources:Resource,wfb2dl_PAY_LEAVE_YEAR_mod%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_PAY_LEAVE_YEAR_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--主假別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dl_MAIN_LEAVE_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_MAIN_LEAVE_CD" runat="server" class="MandatoryField" ClientIDMode="Static"
                                            OnSelectedIndexChanged="ddl_MAIN_LEAVE_CD_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem Value="" Text="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="D" Text="<%$Resources:Resource,wfb2dl_SUB_LEAVE_CD_D%>"></asp:ListItem>
                                            <asp:ListItem Value="M" Text="<%$Resources:Resource,wfb2dl_SUB_LEAVE_CD_M%>"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_MAIN_LEAVE_CD_importNull%>"
                                            ControlToValidate="ddl_MAIN_LEAVE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <%--子假別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SUB_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dl_SUB_LEAVE_DESC%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:DropDownList ID="ddl_SUB_LEAVE_CD" runat="server" class="MandatoryField" onclick="return checkField();" ClientIDMode="Static"
                                            OnSelectedIndexChanged="ddl_SUB_LEAVE_CD_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_SUB_LEAVE_CD_importNull%>"
                                            ControlToValidate="ddl_SUB_LEAVE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--生效期間--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2dl_START_DT_Qry%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_START_DT_S" runat="server" MaxLength="8" Width="80px" ClientIDMode="Static" CssClass="date MandatoryField"  AutoPostBack="true" OnTextChanged="txt_START_DT_S_TextChanged"></asp:TextBox>
                                        ～ 
                                        <asp:TextBox ID="txt_START_DT_E" runat="server" MaxLength="8" Width="80px" ClientIDMode="Static" CssClass="date MandatoryField" AutoPostBack="true" OnTextChanged="txt_START_DT_E_TextChanged"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_START_DT_importError%>"
                                            ClientValidationFunction="checkSTART_DT" ForeColor="Red"
                                            ControlToValidate="txt_START_DT_E" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dl_START_SDT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator5" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dl_START_EDT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_START_DT_S"
                                            ControlToValidate="txt_START_DT_E" ErrorMessage="<%$Resources:Resource,wfb2dl_START_SDT_EDT%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA">
                                        </asp:CompareValidator>
                                    </td>
                                    <%--期間天數--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label2" runat="server" Text="期間天數"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:Label ID="lb_CAL_DT_txt" runat="server" Width="100px" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" style="height: 20px;"></td>
                                </tr>
                                <tr>
                                    <%--可用時數--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_AVAILABLE_VALUE" runat="server" Text="<%$Resources:Resource,wfb2dl_AVAILABLE_VALUE%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:Label ID="lb_AVAILABLE_VALUE_txt" runat="server" Width="50px" MaxLength="6" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--核定時數onBlur="checkNum(this.id);" --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPROVE_VALUE" runat="server" Text="<%$Resources:Resource,wfb2dl_APPROVE_VALUE%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:TextBox ID="txt_APPROVE_VALUE" runat="server" style="text-align:right;" Width="50px" MaxLength="5" onBlur="checkNum(this.id);" CssClass="MandatoryField hour" ClientIDMode="Static"></asp:TextBox>
                                        <asp:Label ID="lb_APPROVE_VALUE_day" runat="server" style="text-align:right;" ClientIDMode="Static" Width="40px"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" ClientIDMode="Static" Text="天" Width="30px"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_APPROVE_VALUE_importNull%>"
                                            ControlToValidate="txt_APPROVE_VALUE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--去年特休預借時數--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_USED_PAY_LEAVE_VALUE" runat="server" Text="<%$Resources:Resource,wfb2dl_USED_PAY_LEAVE_VALUE%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_USED_PAY_LEAVE_VALUE" style="text-align:right;" CssClass="hour" onBlur="checkNum(this.id);"
                                            runat="server" Width="50px" MaxLength="5" ClientIDMode="Static"></asp:TextBox>
                                    </td>

                                     <%--遞延時數--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEFFER_VALUE" runat="server" Text="<%$Resources:Resource,wfb2dl_DEFFER_VALUE%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_DEFFER_VALUE" style="text-align:right;" CssClass="" onBlur="checkNum(this.id);"
                                            runat="server" Width="50px" MaxLength="5" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--調整時數--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ADJUST_VALUE" runat="server" Text="<%$Resources:Resource,wfb2dl_ADJUST_VALUE%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_ADJUST_VALUE" runat="server" style="text-align:right;" Width="50px" MaxLength="7"
                                            onBlur="checkNumRange(this.id);" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--調整說明--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ADJUST_DESC" runat="server" Text="<%$Resources:Resource,wfb2dl_ADJUST_DESC%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_ADJUST_DESC" runat="server" MaxLength="60" Width="300px" ClientIDMode="Static"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_ADJUST_DESC_importError%>"
                                            ClientValidationFunction="checkADJUST_DESC" ForeColor="Red"
                                            ControlToValidate="txt_ADJUST_VALUE" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--一齊特休日數--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_POLICY_PAY_LEAVE_DAY" runat="server" Text="<%$Resources:Resource,wfb2dl_POLICY_PAY_LEAVE_DAY%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:TextBox ID="txt_POLICY_PAY_LEAVE_DAY" style="text-align:right;" MaxLength="2" runat="server" Width="50px" ClientIDMode="Static"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dl_POLICY_PAY_LEAVE_DAY_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_POLICY_PAY_LEAVE_DAY" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" style="height: 20px;"></td>
                                </tr>
                                <tr>
                                    <%--結算方式--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_SETTLE_CD" runat="server" Text="<%$Resources:Resource,wfb2dl_SALARY_SETTLE_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:DropDownList ID="ddl_SALARY_SETTLE_CD" runat="server" class="MandatoryField" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%--計薪狀態--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_SETTLE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dl_SALARY_SETTLE_STATUS%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:Label ID="lb_SALARY_SETTLE_STATUS_txt" runat="server" ClientIDMode="Static" Text="N"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--發薪日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAY_DT" runat="server" Text="<%$Resources:Resource,wfb2dl_PAY_DT%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:Label ID="lb_PAY_DT_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--資料來源--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_SOURCE" runat="server" Text="<%$Resources:Resource,wfb2dl_DATA_SOURCE%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:Label ID="lb_DATA_SOURCE_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--備註--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2dl_REMARK%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:TextBox ID="txt_REMARK" runat="server" MaxLength="210" Width="700px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="Body_label" colspan="6">
                                        <aces:Btn ID="WFB2DL0100Save" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2DL0100Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                        <%--<asp:Button ID="WFB2DL0100Save" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2DL0100Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                                        <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="btn_back_Click" OnClientClick="return ConfirmMessage();" />

                                    </td>
                                </tr>

                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Body_label"></td>
                </tr>
            </table>

            <%--聘用單位--%>
            <asp:HiddenField ID="hid_COMPANY_CD" runat="server" ClientIDMode="Static" />
            <%--員工區分代號--%>
            <asp:HiddenField ID="hid_EMP_CD" runat="server" ClientIDMode="Static" />
            <%--職務代號--%>
            <asp:HiddenField ID="hid_PJOB_CD" runat="server" ClientIDMode="Static" />
            <%--在職天數--%>
            <asp:HiddenField ID="hid_WORK_DAYS" runat="server" ClientIDMode="Static" />
            <%--特休生成日--%>
            <asp:HiddenField ID="hid_DL_GEN_DT" runat="server" ClientIDMode="Static" />

            <%--試用天數--%>
            <asp:HiddenField ID="hid_TRY_DAYS" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_ORI_DIV_DEPT_FULL_NAME" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_ORI_DEPT_NAME_20" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_ORI_DEPT_NAME_30" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_ORI_DEPT_NAME_40" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_AVAILABLE_VALUE" runat="server" ClientIDMode="Static" />

            <asp:HiddenField ID="hid_state" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_BASE_YEAR_importError" Value="<%$Resources:Resource,wfb2dl_BASE_YEAR_importError%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_cancel_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_EMP_ID_and_BASE_YEAR" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="WFB2DL0100Save" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="txt_EMP_ID" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddl_MAIN_LEAVE_CD" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddl_SUB_LEAVE_CD" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="txt_START_DT_S" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="txt_START_DT_E" EventName="TextChanged" />            
        </Triggers>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>
