<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0500_Add.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0500_Add" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        jQuery(document).ready(function () {
            if ($("#hid_TEST").val() == "mod")
                $('#bt_EMP_ID').attr("disabled", true);
            
            iniForm();
        });
        function getEmp() {
            var json = OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'Y');
            if (json != undefined)
                $("#txt_DEPT_NO").val(json.DEPT_NO);
            __doPostBack('<%#txt_EMP_ID.ClientID %>', 'OnTextChanged');
        }
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".number2").mask("999");
            $(".date").mask("9999/99/99");
            $(".date2").mask("9999/99");
            $("#txt_EMP_ID").mask("99999");
            $('#txt_EMP_NAME').attr("readonly", true);
            $('#txt_DEPT_NO').attr("readonly", true);
            $('#txt_OVERTIME_CTL_CD').attr("readonly", true);
            $('#txt_DT_TYPE').attr("readonly", true);
            $('#txt_SHIFT_CD').attr("readonly", true);
            $('#txt_OVERTIME_DT_TYPE').attr("readonly", true);
            $("#txt_BEFORE_HOUR").attr("readonly", true);
            $("#txt_AFTER_HOUR").attr("readonly", true);
            $("#txt_TRIP_HOUR").attr("readonly", true);
            $("#txt_APPLY_OVERTIME_HOUR").attr("readonly", true);
            $("#txt_APPROVE_OVERTIME_HOUR").attr("readonly", true);
            $("#txt_OVERTIME_PAY_HOUR").attr("readonly", true);
            $("#txt_HYPER_HOUR").attr("readonly", true);
            $("#txt_NORMAL_HOUR").attr("readonly", true);
            $("#txt_EXCHANGE_HOUR").attr("readonly", true);
            $('#txt_FORM_STATUS').attr("readonly", true);
            $('#txt_IS_DUTY_CHECK').attr("readonly", true);
            $('#txt_CHECK_STATUS').attr("readonly", true);
            $('#txt_CLOCK_IN_TIME').attr("readonly", true);
            $('#txt_CLOCK_OUT_TIME').attr("readonly", true);
            $('#txt_IFLOW_NO').attr("readonly", true);
            $("#txt_SALARY_GIVE_DT").attr("readonly", true);
            $("#txt_SALARY_SETTLE_STATUS").attr("readonly", true);

            $.unblockUI();
        }
        function saveCheck(flag) {
            //alert(flag);
            var processed = true;
            var BEFORE_STIME_H = $("#ddl_BEFORE_STIME_H").val() == "-1" ? "00" : parseInt($("#ddl_BEFORE_STIME_H").val());
            var BEFORE_STIME_M = $("#ddl_BEFORE_STIME_M").val() == "-1" ? "00" : parseInt($("#ddl_BEFORE_STIME_M").val());
            var BEFORE_ETIME_H = $("#ddl_BEFORE_ETIME_H").val() == "-1" ? "00" : parseInt($("#ddl_BEFORE_ETIME_H").val());
            var BEFORE_ETIME_M = $("#ddl_BEFORE_ETIME_M").val() == "-1" ? "00" : parseInt($("#ddl_BEFORE_ETIME_M").val());
            var BEFORE_HOUR = parseInt($("#txt_BEFORE_HOUR").val());

            var AFTER_STIME_H = $("#ddl_AFTER_STIME_H").val() == "-1" ? "00" : parseInt($("#ddl_AFTER_STIME_H").val());
            var AFTER_STIME_M = $("#ddl_AFTER_STIME_M").val() == "-1" ? "00" : parseInt($("#ddl_AFTER_STIME_M").val());
            var AFTER_ETIME_H = $("#ddl_AFTER_ETIME_H").val() == "-1" ? "00" : parseInt($("#ddl_AFTER_ETIME_H").val());
            var AFTER_ETIME_M = $("#ddl_AFTER_ETIME_M").val() == "-1" ? "00" : parseInt($("#ddl_AFTER_ETIME_M").val());
            var AFTER_HOUR = parseInt($("#txt_AFTER_HOUR").val());

            var TRIP_STIME_H = $("#ddl_TRIP_STIME_H").val() == "-1" ? "00" : parseInt($("#ddl_TRIP_STIME_H").val());
            var TRIP_STIME_M = $("#ddl_TRIP_STIME_M").val() == "-1" ? "00" : parseInt($("#ddl_TRIP_STIME_M").val());
            var TRIP_ETIME_H = $("#ddl_TRIP_ETIME_H").val() == "-1" ? "00" : parseInt($("#ddl_TRIP_ETIME_H").val());
            var TRIP_ETIME_M = parseInt($("#ddl_TRIP_ETIME_M").val());

            var TRIP_HOUR = 0;
            if ($("#hid_TRIP_HOUR").val() != "") {
                TRIP_HIUR = parseInt($("#hid_TRIP_HOUR").val(), 10);
            }
            var APPLY_OVERTIME_HOUR = parseInt($("#txt_APPLY_OVERTIME_HOUR").val());
            var OVERTIME_DT_TYPE = $("#txt_OVERTIME_DT_TYPE").val();
            var apply_overtime_dt = $("#txt_APPLY_OVERTIME_DT").val();
            var apply_dt = new Date(apply_overtime_dt);
            var today = new Date();
            var O_SPECIAL_CD = $("#ddl_O_SPECIAL_CD").val().split("-")[0];

            if (O_SPECIAL_CD == "2") {
                //B.加班特殊狀況為「2-出差」

                if ($("#txt_TRIP_TIME").val() == "-1" || TRIP_STIME_H == "-1" ||
                    TRIP_STIME_M == "-1" || TRIP_ETIME_H == "-1" || TRIP_ETIME_M == "-1" ||
                    TRIP_HOUR == "-1" ) {
                    alert("有出差, 出差時間不可空白");
                    processed = false;
                }
                if (today <= apply_dt) {
                    alert("有出差, 系統日期需大於勤務日期");
                    processed = false;
                }
                if (TRIP_STIME_H == "-1" && TRIP_STIME_M == "-1" && TRIP_ETIME_H == "-1" && TRIP_ETIME_M == "-1" && TRIP_HOUR == "-1") {
                    alert("勤前加班起迄資料不齊全");
                    processed = false;
                }
            } else {
                if (flag == "cal") {
                    if (TRIP_STIME_H != "-1" && TRIP_STIME_M != "-1" && TRIP_ETIME_H != "-1" && TRIP_ETIME_M != "-1") {
                        alert("無出差, 出差時間不必輸入");
                        processed = false;
                    }
                    //save
                } else {
                    //alert(TRIP_HOUR);
                    if (TRIP_HOUR != "") {
                        alert("無出差, 出差時間不必輸入");
                        processed = false;
                    }
                }
            }

            if (BEFORE_STIME_H == "-1" && BEFORE_STIME_M == "-1" && BEFORE_ETIME_H == "-1" && BEFORE_ETIME_M == "-1" && BEFORE_HOUR == "-1") {
                alert("勤前加班起迄資料不齊全");
                processed = false;
            }


            if (AFTER_STIME_H == "-1" && AFTER_STIME_M == "-1" && AFTER_ETIME_H == "-1" && AFTER_ETIME_M == "-1" && AFTER_HOUR == "-1") {
                alert("勤前加班起迄資料不齊全");
                processed = false;
            }

            
            /*
            if (APPLY_OVERTIME_HOUR == "" || APPLY_OVERTIME_HOUR < 0) {
                alert("請先按計算，申請總時數需大於0");
                processed = false;
            }
            */

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }


        function getTIME() {
            var BEFORE_STIME_H = parseInt($("#ddl_BEFORE_STIME_H").val());
            var BEFORE_STIME_M = parseInt($("#ddl_BEFORE_STIME_M").val());
            var BEFORE_ETIME_H = parseInt($("#ddl_BEFORE_ETIME_H").val());
            var BEFORE_ETIME_M = parseInt($("#ddl_BEFORE_ETIME_M").val());

            var AFTER_STIME_H = parseInt($("#ddl_AFTER_STIME_H").val());
            var AFTER_STIME_M = parseInt($("#ddl_AFTER_STIME_M").val());
            var AFTER_ETIME_H = parseInt($("#ddl_AFTER_ETIME_H").val());
            var AFTER_ETIME_M = parseInt($("#ddl_AFTER_ETIME_M").val());

            var BEFORE_HOUR = parseInt($("#txt_BEFORE_HOUR").val());
            var AFTER_HOUR = parseInt($("#txt_AFTER_HOUR").val());

            var BeforeHour = 0;
            var AfterHour = 0;
            if (BEFORE_STIME_H >= 0 || BEFORE_STIME_M >= 0 || BEFORE_ETIME_H >= 0 || BEFORE_ETIME_M >= 0) {
                BeforeHour = (BEFORE_ETIME_H * 60 + BEFORE_ETIME_M) - (BEFORE_STIME_H * 60 + BEFORE_STIME_M);
                if (BeforeHour >= 0) {
                    var bhh = BeforeHour / 60;
                    var bhm = BeforeHour % 60;
                    $("#txt_BEFORE_HOUR").val(Math.floor(bhh) + ':' + Math.floor(bhm));
                }
                else
                    $("#txt_BEFORE_HOUR").val('0:0');
            }
            if (AFTER_STIME_H >= 0 || AFTER_STIME_M >= 0 || AFTER_ETIME_H >= 0 || AFTER_ETIME_M >= 0) {
                AfterHour = (AFTER_ETIME_H * 60 + AFTER_ETIME_M) - (AFTER_STIME_H * 60 + AFTER_STIME_M)
                if (AfterHour >= 0) {
                    var ahh = AfterHour / 60;
                    var ahm = AfterHour % 60;
                    $("#txt_AFTER_HOUR").val(Math.floor(ahh) + ':' + Math.floor(ahm));
                }
                else
                    $("#txt_AFTER_HOUR").val('0:0');

            }
            var ApplyOvertimeHour = BeforeHour + AfterHour;
            if (ApplyOvertimeHour >= 0) {
                var avhh = ApplyOvertimeHour / 60;
                var avhm = ApplyOvertimeHour % 60;

                $("#txt_APPLY_OVERTIME_HOUR").val(Math.floor(avhh) + ':' + Math.floor(avhm));
            }
            else
                $("#txt_APPLY_OVERTIME_HOUR").val('0:0');

            if ($("#txt_BEFORE_HOUR").val() != "") {
                if ((BEFORE_ETIME_H * 60 + BEFORE_ETIME_M) < (BEFORE_STIME_H * 60 + BEFORE_STIME_M))
                    alert("勤前開始時間不可大於勤前結束時間 !");
            }
            if ($("#txt_AFTER_HOUR").val() != "") {
                if ((AFTER_ETIME_H * 60 + AFTER_ETIME_M) < (AFTER_STIME_H * 60 + AFTER_STIME_M))
                    alert("勤後開始時間不可大於勤後結束時間 !");
            }
            if ($("#txt_BEFORE_HOUR").val() != "" && $("#txt_AFTER_HOUR").val() != "") {
                if ((BEFORE_STIME_H * 60 + BEFORE_STIME_M) <= (AFTER_ETIME_H * 60 + AFTER_ETIME_M) && (BEFORE_ETIME_H * 60 + BEFORE_ETIME_M) >= (AFTER_STIME_H * 60 + AFTER_STIME_M))
                    alert("勤前/勤後時間，不可重複申請 !");
            }
        }

        function getEXCHANGE_HOUR() {
            if ($('#ddl_IS_APPLY').val() == 'Y')
                $('#txt_EXCHANGE_HOUR').val($('#txt_APPLY_OVERTIME_HOUR').val());
            else if ($('#ddl_IS_APPLY').val() == 'N')
                $('#txt_EXCHANGE_HOUR').val("0");
        }
        function ismaxlength(obj) {
            var mlength = obj.getAttribute ? parseInt(obj.getAttribute("maxlength")) : "";
            if (obj.getAttribute && obj.value.length > mlength)
                obj.value = obj.value.substring(0, mlength);
        }

        function openDI0700() {
            if (Page_ClientValidate("GroupA")) {
                var emp_id = $("#txt_EMP_ID").val();
                var apply_overtime_dt = $("#txt_APPLY_OVERTIME_DT").val();
                var apply_dt = new Date(apply_overtime_dt);
                var apply_m = apply_dt.getMonth() + 1;
                if (apply_m < 10) {
                    var apply_overtime_ym = apply_dt.getFullYear() + "/0" + apply_m;
                }
                else {
                    var apply_overtime_ym = apply_dt.getFullYear() + "/" + apply_m;
                }
                window.open("WFB2DI0700_Qry.aspx?fn=FB2DI050&emp_id=" + emp_id + "&apply_overtime_ym=" + apply_overtime_ym + "&parentFuncId=" + parentFuncID);
            }
        }

        function backToQry() {
            window.location.href = "WFB2DI0500_Qry.aspx";
        }

        function Check_ID_LENGTH(source, arguments) {
            if ($("#txt_EMP_ID").val().length != 5)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }

        function CheckEXECUTE(msg) {
            if (confirm(msg)) {
                BlockUI();
                document.getElementById('<%=confirm_ok.ClientID%>').click();
            } else {
                return false;
            }
        }

    </script>
    <style type="text/css">
        .auto-style1
        {
            height: 24px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="3" cellpadding="0" width="1020">
                <tr>
                    <td colspan="3">
                        <table cellspacing="1" cellpadding="1" width="1020">
                            <!--======================================================================================-->
                            <tr>
                                <td align="right">
                                    <div id="mrm" style="display: inline; overflow: auto; width: 974px;">

                                        <table cellspacing="1" cellpadding="1" width="1020" border="0">
                                            <colgroup>
                                                <col width="11%" />
                                                <col width="15%" />
                                                <col width="11%" />
                                                <col width="15%" />
                                                <col width="11%" />
                                                <col width="15%" />
                                                <col width="11%" />
                                                <col width="11%" />
                                            </colgroup>

                                            <tbody>

                                                <tr>
                                                    <%-- 工號 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_ID%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_EMP_ID" runat="server" CssClass="MandatoryField" MaxLength="5" Width="100px" ClientIDMode="Static" OnTextChanged="txt_EMP_ID_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_Required_EMP_ID%>"
                                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_EMP_ID_LENGTH5%>" ClientValidationFunction="Check_ID_LENGTH" ForeColor="Red"
                                                            ControlToValidate="txt_EMP_ID" ValidationGroup="GroupA" Display="None">
                                                        </asp:CustomValidator>

                                                        <input id="bt_EMP_ID" type="button" value="..." onclick="getEmp();" />
                                                    </td>
                                                    <%--姓名 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_NAME%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="15" BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                    <%--部門 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" BorderWidth="0" MaxLength="15" Width="140px" ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                    <%--加班管制對象 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_OVERTIME_CTL_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_CTL_CD2%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_OVERTIME_CTL_CD" runat="server" BorderWidth="0" MaxLength="15" Width="140px" ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%-- 勤務日期 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_APPLY_OVERTIME_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_DT3%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_APPLY_OVERTIME_DT" runat="server" CssClass="MandatoryField date" MaxLength="10" Width="100px" ClientIDMode="Static" OnTextChanged="txt_APPLY_OVERTIME_DT_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<% $Resources:Resource,wfd2di_Required_APPLY_OVERTIME_DT2%> "
                                                            ControlToValidate="txt_APPLY_OVERTIME_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_APPLY_OVERTIME_DT2%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                            ControlToValidate="txt_APPLY_OVERTIME_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                                    </td>
                                                    <%-- 勤務日期類型 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_DT_TYPE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DT_TYPE2%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_DT_TYPE" runat="server" BorderWidth="0" MaxLength="10" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                                        <asp:HiddenField ID="hid_DT_TYPE" runat="server" ClientIDMode="Static" />
                                                    </td>
                                                    <%-- 班別 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_SHIFT_CD%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label" colspan="2">
                                                        <asp:TextBox ID="txt_SHIFT_CD" runat="server" MaxLength="10" Width="230px" ClientIDMode="Static" BorderWidth="0" Height="30px"></asp:TextBox>
                                                        <asp:HiddenField ID="hid_SHIFT_CD" runat="server" ClientIDMode="Static" />
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <%-- 加班類型 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_OVERTIME_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_CD%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_OVERTIME_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddl_OVERTIME_CD_SelectedIndexChanged"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_Required_OVERTIME_CD%>"
                                                            ControlToValidate="ddl_OVERTIME_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <%-- 加班日期類型 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_OVERTIME_DT_TYPE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_DT_TYPE2%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_OVERTIME_DT_TYPE" runat="server" ClientIDMode="Static" BorderWidth="0" Width="144px"></asp:TextBox>
                                                    </td>
                                                    <%-- 代休假日期 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_REPLACE_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_REPLACE_DT%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_REPLACE_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_txt_REPLACE_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                            ControlToValidate="txt_REPLACE_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                                    </td>
                                                    <th align="left"></th>
                                                    <td align="left" class="Body_label"></td>
                                                </tr>

                                                <tr>
                                                    <%-- 是否申告換休 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_IS_APPLY" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IS_APPLY%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_IS_APPLY" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" Onchange="getEXCHANGE_HOUR()" Enabled="false">
                                                            <asp:ListItem Text="N-否" Value="N"></asp:ListItem>
                                                            <asp:ListItem Text="Y-是" Value="Y"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <%-- 加班特殊狀況 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_O_SPECIAL_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_O_SPECIAL_CD%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_O_SPECIAL_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static" OnSelectedIndexChanged="ddl_O_SPECIAL_CD_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_Required_O_SPECIAL_CD%>"
                                                            ControlToValidate="ddl_O_SPECIAL_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                                    </td>

                                                </tr>

                                                <tr>
                                                    <%-- 加班事由 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_OVERTIME_REASON" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_REASON%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label" colspan="7">
                                                        <asp:TextBox ID="txt_OVERTIME_REASON" TextMode="MultiLine" runat="server" MaxLength="60" Width="610px" ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td align="right">
                                    <div id="mrm" style="display: inline; overflow: auto; width: 974px;">
                                        <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                                            <colgroup>
                                                <col width="11%" />
                                                <col width="15%" />
                                                <col width="15%" />
                                                <col width="15%" />
                                                <col width="11%" />
                                                <col width="15%" />
                                                <col width="11%" />
                                                <col width="11%" />
                                            </colgroup>
                                            <tbody>
                                                <tr>
                                                    <%-- 勤前加班起迄 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_BEFORE_TIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_BEFORE_TIME2%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_BEFORE_TIME" runat="server" CssClass="MandatoryField date" MaxLength="10" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<% $Resources:Resource,wfd2di_Required_BEFORE_TIME%>"
                                                            ControlToValidate="txt_BEFORE_TIME" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <%--                                                    <th align="right">
                                                        <asp:Label ID="lb_BEFORE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_BEFORE%>"></asp:Label>:</th>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_BEFORE_STIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_BEFORE_STIME%>"></asp:Label>:</th>--%>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_BEFORE_STIME_H" runat="server" DropDownHeight="100px" CssClass="MandatoryField" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_BEFORE_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_Required_lb_HOUR_A%>"
                                                            ControlToValidate="ddl_BEFORE_STIME_H" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:Label ID="lb_BEFORE_STIME_HOUR_S" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>

                                                        <asp:DropDownList ID="ddl_BEFORE_STIME_M" runat="server" DropDownHeight="100px" CssClass="MandatoryField" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_BEFORE_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_Required_lb_MINUTE_A%>"
                                                            ControlToValidate="ddl_BEFORE_STIME_M" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:Label ID="lb_BEFORE_STIME_MINUTE_S" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label>
                                                    </td>
                                                    <%--                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_BEFORE_ETIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_BEFORE_ETIME%>"></asp:Label>:</th>--%>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_BEFORE_ETIME_H" runat="server" DropDownHeight="100px" CssClass="MandatoryField" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_BEFORE_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_Required_lb_HOUR_A%>"
                                                            ControlToValidate="ddl_BEFORE_ETIME_H" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:Label ID="lb_BEFORE_ETIME_HOUR_E" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>

                                                        <asp:DropDownList ID="ddl_BEFORE_ETIME_M" runat="server" DropDownHeight="100px" CssClass="MandatoryField" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_BEFORE_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_Required_lb_MINUTE_A%>"
                                                            ControlToValidate="ddl_BEFORE_ETIME_M" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:Label ID="lb_BEFORE_ETIME_MINUTE_E" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label></td>
                                                    <%-- 勤前時數 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_BEFORE_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_BEFORE_HOUR%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_BEFORE_HOUR" runat="server" MaxLength="15" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                                        <asp:HiddenField ID="hid_BEFORE_HOUR" runat="server" ClientIDMode="Static" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="WFB2DI0500Cal" runat="server" Text="計算" ValidationGroup="GroupA" OnClientClick="return saveCheck('cal');" OnClick="WFB2DI0500Cal_Click" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <%-- 勤後加班起迄 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_AFTER_TIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_AFTER_TIME2%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_AFTER_TIME" runat="server" CssClass="MandatoryField date" MaxLength="10" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="<% $Resources:Resource,wfd2di_Required_AFTER_TIME%>"
                                                            ControlToValidate="txt_AFTER_TIME" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <%--                                                    <th align="right">
                                                        <asp:Label ID="lb_AFTER" runat="server" Text="<%$Resources:Resource,wfb2di_lb_AFTER%>"></asp:Label>:</th>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_AFTER_STIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_AFTER_STIME%>"></asp:Label>:</th>--%>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_AFTER_STIME_H" runat="server" DropDownHeight="100px" CssClass="MandatoryField" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_AFTER_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_Required_lb_HOUR_B%>"
                                                            ControlToValidate="ddl_AFTER_STIME_H" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:Label ID="lb_AFTER_STIME_HOUR_S" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>

                                                        <asp:DropDownList ID="ddl_AFTER_STIME_M" runat="server" DropDownHeight="100px" CssClass="MandatoryField" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_AFTER_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_Required_lb_MINUTE_B%>"
                                                            ControlToValidate="ddl_AFTER_STIME_M" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:Label ID="lb_AFTER_STIME_MINUTES" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label>
                                                    </td>
                                                    <%--                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_AFTER_ETIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_AFTER_ETIME%>"></asp:Label>:</th>--%>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_AFTER_ETIME_H" runat="server" DropDownHeight="100px" CssClass="MandatoryField" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_AFTER_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_Required_lb_HOUR_B%>"
                                                            ControlToValidate="ddl_AFTER_ETIME_H" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:Label ID="lb_AFTER_ETIME_HOUR_E" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>

                                                        <asp:DropDownList ID="ddl_AFTER_ETIME_M" runat="server" DropDownHeight="100px" CssClass="MandatoryField" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_AFTER_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_Required_lb_MINUTE_B%>"
                                                            ControlToValidate="ddl_AFTER_ETIME_M" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:Label ID="lb_AFTER_ETIME_MINUTE_E" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label>
                                                    </td>
                                                    <%-- 勤後時數 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_AFTER_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_AFTER_HOUR%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_AFTER_HOUR" runat="server" MaxLength="15" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                                        <asp:HiddenField ID="hid_AFTER_HOUR" runat="server" ClientIDMode="Static" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%-- 出差時間起迄 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_TRIP_TIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_TRIP_TIME%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_TRIP_TIME" runat="server" CssClass="date" MaxLength="10" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_TRIP_STIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static"></asp:DropDownList>
                                                        <asp:Label ID="lb_TRIP_STIME_H" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                                        <asp:DropDownList ID="ddl_TRIP_STIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static"></asp:DropDownList>
                                                        <asp:Label ID="lb_TRIP_STIME_M" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label>
                                                    </td>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_TRIP_ETIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static"></asp:DropDownList>
                                                        <asp:Label ID="lb_TRIP_ETIME_H" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                                        <asp:DropDownList ID="ddl_TRIP_ETIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static"></asp:DropDownList>
                                                        <asp:Label ID="lb_TRIP_ETIME_M" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label>
                                                    </td>
                                                    <%-- 出差時數 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_TRIP_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_TRIP_HOUR%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_TRIP_HOUR" runat="server" MaxLength="15" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                                        <asp:HiddenField ID="hid_TRIP_HOUR" runat="server" ClientIDMode="Static" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%-- 申請總時數 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_APPLY_OVERTIME_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_HOUR%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_APPLY_OVERTIME_HOUR" runat="server" MaxLength="15" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                                        <asp:HiddenField ID="hid_APPLY_OVERTIME_HOUR" runat="server" ClientIDMode="Static" />
                                                    </td>
                                                    <%-- 核准總時數 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_APPROVE_OVERTIME_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPROVE_OVERTIME_HOUR%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_APPROVE_OVERTIME_HOUR" runat="server" MaxLength="15" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                                        <asp:HiddenField ID="hid_APPROVE_OVERTIME_HOUR" runat="server" ClientIDMode="Static" />
                                                    </td>
                                                    <%-- 計算總時數 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_OVERTIME_PAY_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_OVERTIME_PAY_HOUR2%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_OVERTIME_PAY_HOUR" runat="server" MaxLength="15" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                                        <asp:HiddenField ID="hid_OVERTIME_PAY_HOUR" runat="server" ClientIDMode="Static" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%-- 三高累計時數 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_HYPER_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HYPER_HOUR%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_HYPER_HOUR" runat="server" MaxLength="15" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                                        <asp:HiddenField ID="hid_HYPER_HOUR" runat="server" ClientIDMode="Static" />
                                                    </td>
                                                    <%-- 一般累計時數 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_NORMAL_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_NORMAL_HOUR%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_NORMAL_HOUR" runat="server" MaxLength="15" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                                        <asp:HiddenField ID="hid_NORMAL_HOUR" runat="server" ClientIDMode="Static" />
                                                    </td>
                                                    <%-- 可換休時數 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_EXCHANGE_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EXCHANGE_HOUR%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_Label">
                                                        <asp:TextBox ID="txt_EXCHANGE_HOUR" runat="server" BorderWidth="0" MaxLength="10" Width="54px" ClientIDMode="Static"></asp:TextBox>
                                                        <asp:HiddenField ID="hid_EXCHANGE_HOUR" runat="server" ClientIDMode="Static" />
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </td>
                            </tr>

                            <tr>
                                <td align="right">
                                    <div id="mrm" style="display: inline; overflow: auto; width: 974px;">
                                        <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                                            <colgroup>
                                                <col width="11%" />
                                                <col width="15%" />
                                                <col width="11%" />
                                                <col width="15%" />
                                                <col width="11%" />
                                                <col width="15%" />
                                                <col width="11%" />
                                                <col width="11%" />
                                            </colgroup>
                                            <tbody>
                                                <tr>
                                                    <%-- 核准日期 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_IFLOW_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IFLOW_APPROVE_DT_2%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_IFLOW_APPROVE_DT" runat="server" MaxLength="15" Width="100px" ClientIDMode="Static" CssClass="date MandatoryField"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<% $Resources:Resource,wfd2di_Required_IFLOW_APPROVE_DT%> "
                                                            ControlToValidate="txt_IFLOW_APPROVE_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_txt_IFLOW_APPROVE_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                            ControlToValidate="txt_IFLOW_APPROVE_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                                    </td>
                                                    <%-- 表單狀態 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_FORM_STATUS" runat="server" Text="<%$Resources:Resource,wfb2di_lb_FORM_STATUS%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_FORM_STATUS" runat="server" BorderWidth="0" Width="50px" ClientIDMode="Static" Text="Y-核准"></asp:TextBox>
                                                    </td>
                                                    <%-- 是否刷卡比對 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_IS_DUTY_CHECK" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IS_DUTY_CHECK%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_IS_DUTY_CHECK" runat="server" BorderWidth="0" Width="50px" ClientIDMode="Static" Text="Y-是"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%-- 刷卡比對狀態 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_CHECK_STATUS" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CHECK_STATUS%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_CHECK_STATUS" runat="server" MaxLength="15" BorderWidth="0" Width="100px" ClientIDMode="Static" Text="N-未比對"></asp:TextBox>
                                                    </td>
                                                    <%-- 刷卡上班時間 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_CLOCK_IN_TIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CLOCK_IN_TIME%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_CLOCK_IN_TIME" runat="server" BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                    <%-- 刷卡下班時間 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_CLOCK_OUT_TIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CLOCK_OUT_TIME%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_CLOCK_OUT_TIME" runat="server" BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%-- 申請單號 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IFLOW_NO%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_IFLOW_NO" runat="server" MaxLength="15" BorderWidth="0" Width="130px" ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                    <%-- 發薪日期 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_PAY_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_SALARY_GIVE_DT%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_PAY_DT" runat="server" Width="100px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                                    </td>
                                                    <%-- 計薪狀態 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_SALARY_SETTLE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SALARY_SETTLE_STATUS%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_SALARY_SETTLE_STATUS" runat="server" Text="Y-已計薪" BorderWidth="0" Width="81px" ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <%-- 備註 --%>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2di_lb_REMARK%>"></asp:Label>:</th>
                                                    <td align="left" class="Body_label" colspan="7">
                                                        <asp:TextBox ID="txt_REMARK" TextMode="MultiLine" runat="server" MaxLength="120" Width="610px" ClientIDMode="Static"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <!--======================================================================================-->
                        </table>
                    </td>
                </tr>
                <!-- START: Show only Save and Cancel button instead for Add and Edit operation -->
                <tr>
                    <td class="Body_PageControl" style="text-align: right">
                        <aces:Btn ID="WFB2DI0500Save" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Save%>" ValidationGroup="GroupA" OnClientClick="return saveCheck('save');" OnClick="WFB2DI0500Save_Click" />

                        <%--<asp:Button ID="WFB2DI0500Save" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Save%>" ValidationGroup="GroupA" OnClientClick="return saveCheck();" OnClick="WFB2DI0500Save_Click" />--%>

                        <asp:Button ID="btn_Cancel" runat="server" Text="<%$Resources:Resource,wfb2di_btn_Cancel%>" Visible="true" OnClick="WFB2DI0500Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                    </td>
                </tr>
                <!-- END: Show only Save and Cancel button instead for Add and Edit operation -->
            </table>
            <asp:HiddenField ID="hid_OVERTIME_ALLOW_CD" runat="server" />
            <asp:HiddenField ID="hid_TEST" runat="server" ClientIDMode="Static" />
            <!-- JAVA SCRIPT section -->
            <asp:Button ID="confirm_ok" runat="server" OnClick="confirm_ok_Click" Style="display: none" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>
