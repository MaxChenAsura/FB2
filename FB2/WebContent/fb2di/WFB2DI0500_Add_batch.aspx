<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0500_Add_batch.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0500_Add_batch" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        jQuery(document).ready(function () {

            iniForm();
        });
        function getEmp() {
            OpenEmpSearch('txt_NEW_EMP_ID', 'txt_NEW_EMP_NAME', 'N','Y');
            //var json = OpenEmpSearch('txt_NEW_EMP_ID', 'txt_NEW_EMP_NAME', 'N');
            //if (json != undefined) {
            //    $("#txt_NEW_DEPT_NO").val(json.DEPT_NO);
            //    $("#txt_NEW_PLANT_CD").text(json.PLANT_NAME);
            //    $("#txt_NEW_WORK_SHIFT_CD").text(json.WORK_SHIFT_DESC);
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

                doEmpAjax();
                //$('#' + ename).val(obj.EMP_NAME.trim());
                //$("#txt_NEW_DEPT_NO").text(obj.DEPT_NAME);
                //$("#txt_NEW_PLANT_CD").text(obj.PLANT_NAME);
                //$("#txt_NEW_WORK_SHIFT_CD").text(obj.WORK_SHIFT_DESC);

            }
        }

        function doEmpAjax() {
            if ($("#txt_NEW_EMP_ID").val().length == 5) {
                $.ajax({
                    url: "../commgeo/WFB2GetEmpData.ashx",
                    data: {
                        EMP_ID: $('#txt_NEW_EMP_ID').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg != "") {
                            

                            $('#txt_NEW_EMP_NAME').text("");
                            $('#txt_NEW_PLANT_CD').text("");
                            $('#txt_NEW_DEPT_NO').text("");
                            $('#txt_NEW_WORK_CD').text("");
                            $('#txt_NEW_WORK_SHIFT_CD').text("");
                            $('#txt_NEW_WORK_CD').text("");
                            alert(JData.errMsg);
                        }
                        else {

                            $('#txt_NEW_EMP_NAME').text(JData.EMP_NAME);
                            $('#txt_NEW_PLANT_CD').text(JData.PLANT_NAME);
                            $('#txt_NEW_DEPT_NO').text(JData.DEPT_NAME);
                            $('#txt_NEW_WORK_CD').text(JData.WORK_CD);
                            $('#txt_NEW_WORK_SHIFT_CD').text(JData.WORK_SHIFT_NAME);
                            $('#txt_NEW_WORK_CD').text(JData.WORK_CD);
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
        }

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_NEW_EMP_ID").mask("99999");
            $(".date").mask("9999/99/99");
            //寫在這，按查詢才不會消失
            $('#txt_SHIFT_DESC').attr("readonly", true);
            $('#txt_DEPT_NAME').attr("readonly", true);
            $('#txt_BEFORE_HOUR').attr("readonly", true);
            $('#txt_AFTER_HOUR').attr("readonly", true);
            $('#txt_APPLY_OVERTIME_HOUR').attr("readonly", true);

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

            $("#txt_NEW_EMP_ID").change(function () {
                if ($("#txt_NEW_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_NEW_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {

                                $('#txt_NEW_EMP_NAME').text("");
                                $('#txt_NEW_PLANT_CD').text("");
                                $('#txt_NEW_DEPT_NO').text("");
                                $('#txt_NEW_WORK_CD').text("");
                                $('#txt_NEW_WORK_SHIFT_CD').text("");
                                alert(JData.errMsg); 
                            }
                            else {

                                $('#txt_NEW_EMP_NAME').text(JData.EMP_NAME);
                                $('#txt_NEW_PLANT_CD').text(JData.PLANT_NAME);
                                $('#txt_NEW_DEPT_NO').text(JData.DEPT_NAME);
                                $('#txt_NEW_WORK_CD').text(JData.WORK_CD);
                                $('#txt_NEW_WORK_SHIFT_CD').text(JData.WORK_SHIFT_NAME);
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
            $.unblockUI();
            gridviewScroll();

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

            //if ($("#txt_BEFORE_HOUR").val() == "" && $("#txt_AFTER_HOUR").val() == "") {
            //    alert("勤前時間和勤後時間必須擇一輸入");
            //    processed = false;
            //}
            //if ($("#txt_REPLACE_DT").val() == "" && document.getElementById('txt_REPLACE_DT').disabled == false) {
            //    alert("代休假日期不可為空");
            //    processed = false;
            //}
            //if ($("#txt_BEFORE_HOUR").val() != "") {
            //    if ((BEFORE_ETIME_H * 60 + BEFORE_ETIME_M) < (BEFORE_STIME_H * 60 + BEFORE_STIME_M)) {
            //        alert("勤前開始時間不可大於勤前結束時間 !");
            //        processed = false;
            //    }
            //}
            //if ($("#txt_AFTER_HOUR").val() != "") {
            //    if ((AFTER_ETIME_H * 60 + AFTER_ETIME_M) < (AFTER_STIME_H * 60 + AFTER_STIME_M)) {
            //        alert("勤後開始時間不可大於勤後結束時間 !");
            //        processed = false;
            //    }
            //}
            if ($("#txt_BEFORE_HOUR").val() != "" && $("#txt_AFTER_HOUR").val() != "") {
                if ((BEFORE_STIME_H * 60 + BEFORE_STIME_M) < (AFTER_ETIME_H * 60 + AFTER_ETIME_M) && (BEFORE_ETIME_H * 60 + BEFORE_ETIME_M) > (AFTER_STIME_H * 60 + AFTER_STIME_M)) {
                    alert("勤前/勤後時間，不可重複申請 !");
                    processed = false;
                }
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
        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "999",
                height: "300",
                barcolor: "#7F7F7F"
                //freezesize: 5

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");

        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }
        function ClearAll() {
            $("#txt_DEPT_NO").val("");
            $("#ddl_PLANT_CD").val("-1");
            $("#ddl_WS_CD").val("-1");
            $("#ddl_WORK_CD").val("-1");
            $("#txt_WORK_SHIFT_CD").val("");
            $("#txt_DUTY_TIME").val("");

            $("#txt_DEPT_NAME").val("");
            $("#txt_WORK_SHIFT_DESC").val("");
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
        function backToQry() {
            window.location.href = "WFB2DI0500_Qry.aspx";
        }

        function getWorkShift() {
            OpenSearch('WorkShift_Search.aspx', 'txt_WORK_SHIFT_CD', 'txt_WORK_SHIFT_DESC', '','Y');
            //var json = OpenSearch('WorkShift_Search.aspx', 'txt_WORK_SHIFT_CD', 'txt_WORK_SHIFT_DESC', '');
            <%--
            __doPostBack('<%#txt_WORK_SHIFT_CD.ClientID %>', 'OnTextChanged');
            --%>
        }

        function WorkShift_Search(obj_cd, obj_desc, value) {
            var returnValue = value;
            if (returnValue == undefined) {
                returnValue = window.returnValue;
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                window.parent.$('#' + obj_cd).val(obj.CD);
                window.parent.$('#' + obj_desc).val(obj.DESC);
                
                __doPostBack('<%#txt_WORK_SHIFT_CD.ClientID %>', 'OnTextChanged');
                
            }
        }

        function Check_ID_LENGTH(source, arguments) {
            if ($("#txt_NEW_EMP_ID").val().length != 5)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }

    </script>
    <style type="text/css">
        .auto-style1 {
            height: 24px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class='pos_top'>
        <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
            <tbody>
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
                                        <th align="left">加班一括申請:</th>
                                    </tr>

                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_CD%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_OVERTIME_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddl_OVERTIME_CD_SelectedIndexChanged"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_Required_OVERTIME_CD%>"
                                                ControlToValidate="ddl_OVERTIME_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                        </td>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME_DT_TYPE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_DT_TYPE_2%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_OVERTIME_DT_TYPE" runat="server" ClientIDMode="Static" BorderWidth="0" Width="144px"></asp:TextBox>
                                        </td>

                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_APPLY_OVERTIME_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_DT_2%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_APPLY_OVERTIME_DT" runat="server" CssClass="MandatoryField date" MaxLength="10" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<% $Resources:Resource,wfd2di_Required_APPLY_OVERTIME_DT%> "
                                                ControlToValidate="txt_APPLY_OVERTIME_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_APPLY_OVERTIME_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_APPLY_OVERTIME_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        </td>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_REPLACE_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_REPLACE_DT%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_REPLACE_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="date" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_SHIFT_CD%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label" colspan="6">
                                            <asp:TextBox ID="txt_SHIFT_CD" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static" OnTextChanged="txt_SHIFT_CD_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            <input id="Button2" type="button" value="..." onclick="OpenSearch('Shift_Search.aspx', 'txt_SHIFT_CD', 'txt_SHIFT_DESC', '');" />
                                            <asp:TextBox ID="txt_SHIFT_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<% $Resources:Resource,wfd2di_Required_SHIFT_CD%> "
                                                ControlToValidate="txt_SHIFT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="Body_label"></td>
                                        <th align="right">
                                            <asp:Label ID="lb_BEFORE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_BEFORE%>"></asp:Label>:</th>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_BEFORE_STIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_BEFORE_STIME%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_BEFORE_STIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_BEFORE_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            <asp:Label ID="lb_BEFORE_STIME_HOUR_S" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                            <asp:DropDownList ID="ddl_BEFORE_STIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" Height="16px" ClientIDMode="Static" OnSelectedIndexChanged="ddl_BEFORE_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            <asp:Label ID="lb_BEFORE_STIME_MINUTE_S" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label>
                                        </td>

                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_BEFORE_ETIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_BEFORE_ETIME%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_BEFORE_ETIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_BEFORE_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            <asp:Label ID="lb_BEFORE_ETIME_HOUR_E" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                            <asp:DropDownList ID="ddl_BEFORE_ETIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_BEFORE_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            <asp:Label ID="lb_BEFORE_ETIME_MINUTE_E" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label>
                                        </td>

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
                                            <asp:DropDownList ID="ddl_AFTER_STIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_AFTER_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            <asp:Label ID="lb_AFTER_STIME_HOUR_S" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                            <asp:DropDownList ID="ddl_AFTER_STIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_AFTER_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            <asp:Label ID="lb_AFTER_STIME_MINUTES" runat="server" Text="<%$Resources:Resource,wfb2di_lb_MINUTE%>"></asp:Label>
                                        </td>

                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_AFTER_ETIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_AFTER_ETIME%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:DropDownList ID="ddl_AFTER_ETIME_H" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_AFTER_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            <asp:Label ID="lb_AFTER_ETIME_HOUR_E" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HOUR%>"></asp:Label>
                                            <asp:DropDownList ID="ddl_AFTER_ETIME_M" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static" OnSelectedIndexChanged="ddl_AFTER_TIME_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
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
                                        <th align="left"></th>
                                        <td align="left" class="Body_label"></td>
                                        <th align="left"></th>
                                        <td align="left" class="Body_Label"></td>
                                        <th align="left"></th>
                                        <td align="left" class="Body_label"></td>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_APPLY_OVERTIME_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_APPLY_OVERTIME_HOUR%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_APPLY_OVERTIME_HOUR" runat="server" MaxLength="15" Width="50px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                            <asp:HiddenField ID="hid_APPLY_OVERTIME_HOUR" runat="server" ClientIDMode="Static" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_OVERTIME_REASON" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_REASON%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label" colspan="7">
                                            <asp:TextBox ID="txt_OVERTIME_REASON" TextMode="MultiLine" Rows="2" runat="server" MaxLength="120" Width="610px" ClientIDMode="Static" Style="overflow: auto"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2di_lb_REMARK%>"></asp:Label>:</th>
                                        <td align="left" class="Body_label" colspan="7">
                                            <asp:TextBox ID="txt_REMARK" TextMode="MultiLine" Rows="2" runat="server" MaxLength="120" Width="610px" ClientIDMode="Static" Style="overflow: auto"></asp:TextBox>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>

                        </div>
                    </td>
                </tr>
            </tbody>
        </table>

        <fieldset style="padding: 5px; padding-top: 10px; border-color: red; width: 1000px;">
            <legend class="Body_label"><font color="Red" size="+1">人員選取</font></legend>
            <table cellspacing="1" cellpadding="1" width="1000px" border="0" class="Body_Label">
                <colgroup>
                    <col width="12%" />
                    <col width="15%" />
                    <col width="12%" />
                    <col width="28%" />
                    <col width="12%" />
                    <col width="22%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_PLANT_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_PLANT_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="80px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_DEPT_NO" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                            <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>

                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_WS_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_WS_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_WORK_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_WORK_CD%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_WORK_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_WORK_SHIFT_CD%>"></asp:Label></th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_WORK_SHIFT_CD" runat="server" MaxLength="2" Width="40px" ClientIDMode="Static" OnTextChanged="txt_WORK_SHIFT_CD_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <input id="btn_WORK_SHIFT_CD" type="button" value="..." onclick="getWorkShift();" />
                            <asp:TextBox ID="txt_WORK_SHIFT_DESC" runat="server" BorderWidth="0" Width="160px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DUTY_TIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DUTY_TIME%>"></asp:Label></th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DUTY_TIME" runat="server" MaxLength="15" Width="100px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                        </td>
                    </tr>

                    <tr align="left">
                        <td colspan="6">
                            <asp:RadioButtonList ID="rbl_APPLY_TYPE" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static" OnSelectedIndexChanged="rbl_APPLY_TYPE_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="全部申請(依人員選取,不可增刪)" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="部分申請(依人員選取,可增刪)" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>

                    <!-- end: Create MODULE ID -->
                    <tr>
                        <td align="right" colspan="6">
                            <div>
                                <aces:Btn ID="WFB2DI0501Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0500Search%>" Visible="true" OnClick="WFB2DI0501Search_Click" />

                                <%--<asp:Button ID="WFB2DI0501Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0500Search%>" Visible="false" OnClick="WFB2DI0501Search_Click" />--%>

                                <input id="WFB2DI0500Clear" runat="server" type="button" value="<%$Resources:Resource,wfb2di_WFB2DI0500Clear%>" onclick="ClearAll();" />


                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="6">
                            <div id="init_add" style="display: inline">
                                <%--<aces:Btn ID="WFB2DI0501Add" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Add%>" Visible="false" OnClick="WFB2DI0501Add_Click" />--%>

                                <asp:Button ID="WFB2DI0501Add" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Add%>" Visible="false" OnClick="WFB2DI0501Add_Click" />
                            </div>
                            <div id="init_grid" style="display: inline">
                                <aces:Btn ID="WFB2DI0501Delete" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Delete%>" Visible="false" OnClientClick="return CheckDelAction();" OnClick="WFB2DI0501Delete_Click" />

                                <%--<asp:Button ID="WFB2DI0501Delete" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Delete%>" Visible="false" OnClientClick="return CheckDelAction();" OnClick="WFB2DI0501Delete_Click" />--%>
                            </div>
                            <div id="grid_add">
                                <aces:Btn ID="WFB2DI0501Save" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Save%>" Visible="false" OnClick="WFB2DI0501Save_Click" />

                                <%--<asp:Button ID="WFB2DI0501Save" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Save%>" Visible="false" OnClick="WFB2DI0501Save_Click" />--%>

                                <asp:Button ID="WFB2DI0501Cancel" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2DI0501Cancel_Click" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getBatchData"
                SelectCountMethod="getBatchCount" TypeName="CFB2DI0500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_PLANT_CD"
                        Name="plant_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="dept_no" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="ddl_WS_CD" DefaultValue=""
                        Name="ws_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_WORK_CD" DefaultValue=""
                        Name="work_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_WORK_SHIFT_CD" DefaultValue="" ConvertEmptyStringToNull="False"
                        Name="work_shift_cd" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="hid_AddEMP"
                        Name="AddEmp" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_DeleteEMP"
                        Name="DeleteEmp" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="980px"
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>" HeaderStyle-Width="20px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--                <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="10px" />--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_EMP_ID" runat="server" MaxLength="5" Width="64px" CssClass="MandatoryField"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="getEmp();" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_Required_EMP_ID%>"
                                ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2di_ERR_EMP_ID_LENGTH5%>" ClientValidationFunction="Check_ID_LENGTH" ForeColor="Red"
                                ControlToValidate="txt_NEW_EMP_ID" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="txt_NEW_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" Width="60px"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_PLANT_CD%>" SortExpression="PLANT_CD" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PLANT_CD" runat="server" Text='<%#Bind("PLANT_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="txt_NEW_PLANT_CD" runat="server" MaxLength="4"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text='<%#Bind("DEPT_FULL_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="txt_NEW_DEPT_NO" runat="server" MaxLength="4"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_WORK_CD%>" SortExpression="WORK_CD" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_CD" runat="server" Text='<%#Bind("WORK_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="txt_NEW_WORK_CD" runat="server" MaxLength="4"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_WORK_SHIFT_CD%>" SortExpression="WORK_SHIFT_CD" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text='<%#Bind("WORK_SHIFT_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="txt_NEW_WORK_SHIFT_CD" runat="server" MaxLength="4"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>

                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td></td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2di_RowNumber%>" Width="20px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_ID%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2di_lb_EMP_NAME%>" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2di_PLANT_CD%>" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2di_lb_DEPT_NO%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2di_WORK_CD%>" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2di_WORK_SHIFT_CD%>" Width="100px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txt_NEW_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                <input id="bt_EMP_ID" type="button" value="..." onclick="getEmp();" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_Required_EMP_ID%>"
                                    ControlToValidate="txt_NEW_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2di_ERR_EMP_ID_LENGTH5%>" ClientValidationFunction="Check_ID_LENGTH" ForeColor="Red"
                                    ControlToValidate="txt_NEW_EMP_ID" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:Label ID="txt_NEW_EMP_NAME" runat="server" MaxLength="10" Width="80px" ClientIDMode="Static" BorderWidth="0"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txt_NEW_PLANT_CD" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txt_NEW_DEPT_NO" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txt_NEW_WORK_CD" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="txt_NEW_WORK_SHIFT_CD" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <td></td>
                        </tr>
                    </table>

                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
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
            </table>
        </fieldset>
        <table border="0" width="1020px">
            <tr align="right">
                <td>
                    <aces:Btn ID="WFB2DI0501Confirm" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0501Confirm%>" OnClick="WFB2DI0501Confirm_Click" OnClientClick="return saveCheck();" />

                    <%--<asp:Button ID="WFB2DI0501Confirm" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0501Confirm%>" OnClick="WFB2DI0501Confirm_Click" OnClientClick="return saveCheck();" />--%>
                    <asp:Button ID="WFB2DI0500Cancel" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2DI0500Cancel%>" Visible="true" OnClick="WFB2DI0500Cancel_Click" />
                </td>
            </tr>
        </table>
    </div>

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
    <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hid_AddEMP" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hid_DeleteEMP" runat="server" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
</asp:Content>

