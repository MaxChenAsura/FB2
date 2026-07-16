<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0500_Update.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0500_Update" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        //var cal1xx = new CalendarPopup();
        //cal1xx.showNavigationDropdowns();

        jQuery(document).ready(function () {

            iniForm();
        });
        function getEmp() {
            var json = OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');
            if (json != undefined)
                $("#txt_DEPT_NO").val(json.DEPT_NO);
        }
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $(".date").mask("9999/99/99");
            $(".date2").mask("9999/99");

            $(".number2").mask("999");

            $("#txt_EMP_ID").attr("readonly", true);
            $("#txt_EMP_NAME").attr("readonly", true);
            $("#txt_DEPT_NO").attr("readonly", true);
            $("#txt_IFLOW_NO").attr("readonly", true);

            $("#txt_OVERTIME_DT_TYPE").attr("readonly", true);
            $("#txt_OVERTIME_CTL_CD").attr("readonly", true);
            $("#txt_SHIFT_CD").attr("readonly", true);
            $("#txt_OVERTIME_TIME_CD").attr("readonly", true);
            $("#txt_CHECK_STATUS").attr("readonly", true);
            $("#txt_SALARY_SETTLE_STATUS").attr("readonly", true);
            $("#txt_FORM_STATUS").attr("readonly", true);

            $("#txt_BEFORE_HOUR").attr("readonly", true);
            $("#txt_AFTER_HOUR").attr("readonly", true);
            $("#txt_APPLY_OVERTIME_HOUR").attr("readonly", true);

            $.unblockUI();
        }
        function saveCheck() {
            var processed = true;
            var BEFORE_STIME_H = parseInt($("#ddl_BEFORE_STIME_H").val());
            var BEFORE_STIME_M = parseInt($("#ddl_BEFORE_STIME_M").val());
            var BEFORE_ETIME_H = parseInt($("#ddl_BEFORE_ETIME_H").val());
            var BEFORE_ETIME_M = parseInt($("#ddl_BEFORE_ETIME_M").val());

            var AFTER_STIME_H = parseInt($("#ddl_AFTER_STIME_H").val());
            var AFTER_STIME_M = parseInt($("#ddl_AFTER_STIME_M").val());
            var AFTER_ETIME_H = parseInt($("#ddl_AFTER_ETIME_H").val());
            var AFTER_ETIME_M = parseInt($("#ddl_AFTER_ETIME_M").val());

            if ($("#txt_BEFORE_HOUR").val() == "" && $("#txt_AFTER_HOUR").val() == "") {
                alert("勤前時間和勤後時間必須擇一輸入");
                processed = false;
            }
            if ($("#txt_REPLACE_DT").val() == "" && document.getElementById('txt_REPLACE_DT').disabled == false) {
                alert("代休假日期不可為空");
                processed = false;
            }
            if ($("#txt_BEFORE_HOUR").val() != "") {
                if ((BEFORE_ETIME_H * 60 + BEFORE_ETIME_M) < (BEFORE_STIME_H * 60 + BEFORE_STIME_M)) {
                    alert("勤前開始時間不可大於勤前結束時間 !");
                    processed = false;
                }
            }
            if ($("#txt_AFTER_HOUR").val() != "") {
                if ((AFTER_ETIME_H * 60 + AFTER_ETIME_M) < (AFTER_STIME_H * 60 + AFTER_STIME_M)) {
                    alert("勤後開始時間不可大於勤後結束時間 !");
                    processed = false;
                }
            }
            if ($("#txt_BEFORE_HOUR").val() != "" && $("#txt_AFTER_HOUR").val() != "") {
                if ((BEFORE_STIME_H * 60 + BEFORE_STIME_M) <= (AFTER_ETIME_H * 60 + AFTER_ETIME_M) && (BEFORE_ETIME_H * 60 + BEFORE_ETIME_M) >= (AFTER_STIME_H * 60 + AFTER_STIME_M))
                    alert("勤前/勤後時間，不可重複申請 !");
                processed = false;
            }


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
            if (BEFORE_STIME_H > 0 || BEFORE_STIME_M > 0 || BEFORE_ETIME_H > 0 || BEFORE_ETIME_M > 0) {
                BeforeHour = (BEFORE_ETIME_H * 60 + BEFORE_ETIME_M) - (BEFORE_STIME_H * 60 + BEFORE_STIME_M);
                var bhh = BeforeHour / 60;
                var bhm = BeforeHour % 60;
                $("#txt_BEFORE_HOUR").val(Math.floor(bhh) + ':' + Math.floor(bhm));
            }
            if (AFTER_STIME_H > 0 || AFTER_STIME_M > 0 || AFTER_ETIME_H > 0 || AFTER_ETIME_M > 0) {
                AfterHour = (AFTER_ETIME_H * 60 + AFTER_ETIME_M) - (AFTER_STIME_H * 60 + AFTER_STIME_M)
                var ahh = AfterHour / 60;
                var ahm = AfterHour % 60;
                $("#txt_AFTER_HOUR").val(Math.floor(ahh) + ':' + Math.floor(ahm));
            }
            var ApplyOvertimeHour = BeforeHour + AfterHour;
            var avhh = ApplyOvertimeHour / 60;
            var avhm = ApplyOvertimeHour % 60;

            $("#txt_APPLY_OVERTIME_HOUR").val(Math.floor(avhh) + ':' + Math.floor(avhm));

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
            if ($('#ddl_IS_APPLY').val() == '2')
                $('#txt_EXCHANGE_HOUR').val($('#txt_APPROVE_OVERTIME_HOUR').val());
            else if ($('#ddl_IS_APPLY').val() == '1')
                $('#txt_EXCHANGE_HOUR').val("0");
        }

        function backToQry() {
            window.location.href = "WFB2DI0500_Qry.aspx";
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

    </script>
    <style type="text/css">
        .auto-style1 {
            height: 24px;
        }

        .auto-style2 {
            height: 22px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_ID%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_EMP_ID" runat="server" BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_NAME%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_DEPT_NO" runat="server" BorderWidth="0" Width="140px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <th align="left"></th>
                                            <td align="left" class="Body_label"></td>
                                        </tr>

                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_OVERTIME_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_CD%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_OVERTIME_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddl_OVERTIME_CD_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_Required_OVERTIME_CD%>"
                                                    ControlToValidate="ddl_OVERTIME_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_OVERTIME_DT_TYPE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_DT_TYPE_2%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_OVERTIME_DT_TYPE" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_OVERTIME_CTL_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_CTL_CD%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label" colspan="2">
                                                <asp:TextBox ID="txt_OVERTIME_CTL_CD" runat="server" ClientIDMode="Static" BorderWidth="0" Width="200px"></asp:TextBox>
                                            </td>
                                            <th align="left"></th>
                                            <td align="left" class="Body_label"></td>
                                        </tr>

                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_APPLY_OVERTIME_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_DT_2%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_APPLY_OVERTIME_DT" runat="server" CssClass="MandatoryField date" MaxLength="10" Width="100px" ClientIDMode="Static" OnTextChanged="txt_APPLY_OVERTIME_DT_TextChanged" AutoPostBack="true"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<% $Resources:Resource,wfd2di_Required_APPLY_OVERTIME_DT %> "
                                                    ControlToValidate="txt_APPLY_OVERTIME_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_APPLY_OVERTIME_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                    ControlToValidate="txt_APPLY_OVERTIME_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_REPLACE_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_REPLACE_DT%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_REPLACE_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="date" Enabled="false"></asp:TextBox>
                                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_txt_REPLACE_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                            ControlToValidate="txt_REPLACE_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_SHIFT_CD%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_SHIFT_CD" runat="server" ClientIDMode="Static" BorderWidth="0" Height="16px"></asp:TextBox>
                                                <asp:HiddenField ID="hid_SHIFT_CD" runat="server" ClientIDMode="Static" />
                                            </td>

                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_OVERTIME_TIME_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_TIME_CD%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_OVERTIME_TIME_CD" runat="server" MaxLength="15" BorderWidth="0" Width="100px" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_OVERTIME_REASON" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_REASON%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label" colspan="7">
                                                <asp:TextBox ID="txt_OVERTIME_REASON" TextMode="MultiLine" Rows="3" runat="server" MaxLength="120" Width="610px" ClientIDMode="Static" Style="overflow: auto"></asp:TextBox>
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
                                            <td align="left" class="Body_label"></td>
                                            <th align="right">
                                                <asp:Label ID="lb_BEFORE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_BEFORE%>"></asp:Label>:</th>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_BEFORE_STIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_BEFORE_STIME%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_BEFORE_STIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static"  OnSelectedIndexChanged="ddl_BEFORE_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                <asp:Label ID="lb_BEFORE_STIME_HOUR_S" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                                <asp:DropDownList ID="ddl_BEFORE_STIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true"  ClientIDMode="Static"  OnSelectedIndexChanged="ddl_BEFORE_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                <asp:Label ID="lb_BEFORE_STIME_MINUTE_S" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label></td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_BEFORE_ETIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_BEFORE_ETIME%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_BEFORE_ETIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_BEFORE_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                <asp:Label ID="lb_BEFORE_ETIME_HOUR_E" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                                <asp:DropDownList ID="ddl_BEFORE_ETIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_BEFORE_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                <asp:Label ID="lb_BEFORE_ETIME_MINUTE_E" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label></td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_BEFORE_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_BEFORE_HOUR%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_BEFORE_HOUR" runat="server" MaxLength="15" Width="50px" ClientIDMode="Static"></asp:TextBox>
                                                <asp:HiddenField ID="hid_BEFORE_HOUR" runat="server" ClientIDMode="Static" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td align="left" class="Body_label"></td>
                                            <th align="right">
                                                <asp:Label ID="lb_AFTER" runat="server" Text="<%$Resources:Resource,wfb2di_lb_AFTER%>"></asp:Label>:</th>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_AFTER_STIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_AFTER_STIME%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_AFTER_STIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static"  OnSelectedIndexChanged="ddl_AFTER_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                <asp:Label ID="lb_AFTER_STIME_HOUR_S" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                                <asp:DropDownList ID="ddl_AFTER_STIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static"  OnSelectedIndexChanged="ddl_AFTER_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                <asp:Label ID="lb_AFTER_STIME_MINUTES" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label>
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_AFTER_ETIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_AFTER_ETIME%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:DropDownList ID="ddl_AFTER_ETIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static"  OnSelectedIndexChanged="ddl_AFTER_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                <asp:Label ID="lb_AFTER_ETIME_HOUR_E" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                                <asp:DropDownList ID="ddl_AFTER_ETIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static"  OnSelectedIndexChanged="ddl_AFTER_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                <asp:Label ID="lb_AFTER_ETIME_MINUTE_E" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label>
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_AFTER_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_AFTER_HOUR%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_AFTER_HOUR" runat="server" MaxLength="15" Width="50px" ClientIDMode="Static"></asp:TextBox>
                                                <asp:HiddenField ID="hid_AFTER_HOUR" runat="server" ClientIDMode="Static" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <th align="left" class="auto-style2"></th>
                                            <td align="left" class="auto-style2"></td>
                                            <th align="left" class="auto-style2">
                                                <asp:Label ID="lb_IS_APPLY" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IS_APPLY%>"></asp:Label>:</th>
                                            <td align="left" class="auto-style2">
                                                <asp:DropDownList ID="ddl_IS_APPLY" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" Onchange="getEXCHANGE_HOUR()">
                                                    <asp:ListItem Text="N-否" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Y-是" Value="2"></asp:ListItem>
                                                </asp:DropDownList></td>

                                            <th align="left" class="auto-style2">
                                                <asp:Label ID="lb_EXCHANGE_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EXCHANGE_HOUR%>"></asp:Label>:</th>
                                            <td align="left" class="auto-style2">
                                                <asp:TextBox ID="txt_EXCHANGE_HOUR" runat="server" BorderWidth="0" MaxLength="10" Width="54px" ClientIDMode="Static"></asp:TextBox>
                                                <!--<input type="text" name="Hours" maxlength="10" size="5" tabindex="4" value="" >		-->
                                            </td>
                                            <th align="left" class="auto-style2">
                                                <asp:Label ID="lb_APPLY_OVERTIME_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_HOUR%>"></asp:Label>:</th>
                                            <td align="left" class="auto-style2">
                                                <asp:TextBox ID="txt_APPLY_OVERTIME_HOUR" runat="server" BorderWidth="0" MaxLength="15" Width="50px" ClientIDMode="Static"></asp:TextBox>
                                                <asp:HiddenField ID="hid_APPLY_OVERTIME_HOUR" runat="server" ClientIDMode="Static" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td align="left" class="Body_label"></td>
                                            <th align="right">
                                                <asp:Label ID="lb_CLOCK" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CLOCK%>"></asp:Label>:</th>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_CLOCK_IN_TIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CLOCK_IN_TIME%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_CLOCK_IN_TIME" runat="server" MaxLength="15" BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_CLOCK_OUT_TIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CLOCK_OUT_TIME%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_CLOCK_OUT_TIME" runat="server" MaxLength="15" BorderWidth="0" Width="100px" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                            </td>
                                            <%--<th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_APPROVE_OVERTIME_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPROVE_OVERTIME_HOUR%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_APPROVE_OVERTIME_HOUR" runat="server" MaxLength="15" Width="50px" ClientIDMode="Static"></asp:TextBox>
                                            </td>--%>
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
                                             <%-- 加班明細查詢 --%>
                                            <td align="left" class="auto-style1" colspan="12">
                                                <aces:Btn ID="WFB2DI0500QryOverTime" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0500QryOverTime%>" OnClientClick="openDI0700();" />
                                                
                                                <%--<asp:Button ID="WFB2DI0500QryOverTime" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0500QryOverTime%>" OnClick="WFB2DI0500QryOverTime_Click" OnClientClick="BlockUI();" />--%>
                                            </td>
                                        </tr>

                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_IFLOW_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IFLOW_APPROVE_DT_2%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_IFLOW_APPROVE_DT" runat="server" MaxLength="15" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_SALARY_GIVE_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SALARY_GIVE_DT%>"></asp:Label>:
                                            </th>
                                            <td align="left" class="Body_label"></td>

                                            
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IFLOW_NO%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_IFLOW_NO" runat="server" BorderWidth="0" Width="130px" ClientIDMode="Static"></asp:TextBox>
                                            </td>

                                            <%--                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_SALARY_SETTLE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2di_lb_SALARY_SETTLE_STATUS%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_SALARY_SETTLE_STATUS" runat="server" MaxLength="15" BorderWidth="0" Width="50px" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                            </td>--%>
                                            <th align="left"></th>
                                            <td align="left" class="Body_label"></td>
                                        </tr>

                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_CHECK_STATUS" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CHECK_STATUS%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_CHECK_STATUS" runat="server" BorderWidth="0" Width="100px" ClientIDMode="Static" ></asp:TextBox>
<%--                                                <asp:DropDownList ID="ddl_IS_CONFIRM_CHECK" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" Visible="false">
                                                    <asp:ListItem Text="Y-是" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="N-否" Value="2"></asp:ListItem>
                                                </asp:DropDownList>--%>
                                            </td>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_SALARY_SETTLE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SALARY_SETTLE_STATUS%>"></asp:Label>:
                                            </th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_SALARY_SETTLE_STATUS" runat="server" Text="N-未計薪" BorderWidth="0" Width="81px" ClientIDMode="Static"></asp:TextBox>
                                            </td>

                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_FORM_STATUS" runat="server" Text="<%$Resources:Resource,wfb2di_lb_FORM_STATUS%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_FORM_STATUS" runat="server" BorderWidth="0" Width="50px" ClientIDMode="Static"></asp:TextBox>
                                            </td>
                                            <th align="left"></th>
                                            <td align="left" class="Body_label"></td>

                                        </tr>

                                        <tr>
                                            <th align="left" class="Body_TableHeader">
                                                <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2di_lb_REMARK%>"></asp:Label>:</th>
                                            <td align="left" class="Body_label" colspan="7">
                                                <asp:TextBox ID="txt_REMARK" TextMode="MultiLine" Rows="3" runat="server" MaxLength="120" Width="610px" ClientIDMode="Static" Style="overflow: auto"></asp:TextBox>
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
                <aces:Btn ID="WFB2DI0500Save" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Save%>" Visible="true" OnClick="WFB2DI0500Save_Click" ValidationGroup="GroupA" />

                <%--<asp:Button ID="WFB2DI0500Save" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Save%>" Visible="true" OnClick="WFB2DI0500Save_Click" ValidationGroup="GroupA" />--%>
                
                <asp:Button ID="WFB2DI0500Cancel" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Cancel%>" Visible="true" OnClick="WFB2DI0500Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
            </td>
        </tr>
        <!-- END: Show only Save and Cancel button instead for Add and Edit operation -->
    </table>
    <!-- JAVA SCRIPT section -->
    <asp:HiddenField ID="hid_OVERTIME_ALLOW_CD" runat="server" />
    <asp:HiddenField ID="hid_ORI_OVERTIME_APPLY_DT" runat="server" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>

