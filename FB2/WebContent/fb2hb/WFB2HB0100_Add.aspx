<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0100_Add.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0100_Add" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        var fm = function (v) {
            return (v < 10 ? '0' : '') + v;
        };
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $.unblockUI();
            //$(".txt_REGISTER_ZIP_CD").prop('disabled', 'disabled');

            $(".textWidth").css("font-family", "Times New Roman, Times, serif").css("font-size", "14px");
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".ym").datepicker({ dateFormat: 'yy/mm' });
            $(".ym").mask('9999/99');
            $(".num2").mask('99');
            $(".num3").mask('999');
            $(".num31").mask('99.9');
            $(".num10").mask('9999999999');
            $("#tabs").tabs();
            $('#txt_DIRECT_HEAD_EMP_NAME').attr("readonly", true);
            $('#txt_SALARY_ACCOUNT_BANK_NAME').attr("readonly", true);
            $('#txt_DEPT_NAME').attr("readonly", true);
            $('#txt_PJOB_DESC').attr("readonly", true);
            $('#txt_WORK_SHIFT_DESC').attr("readonly", true);

            $("#ddl_NATION_CD").change(function () {
                if ($("#ddl_NATION_CD").val() == "TWN") {
                    if ($("#txt_LICENSE_ID").val() != "")
                        if (!checkLicenseID($("#txt_LICENSE_ID").val()))
                            alert("身份證號不符合編碼原則");
                }
            });

            $("#txt_PASSPORT_ID").change(function () {
                if ($("#ddl_NATION_CD").val() != "TWN") {
                    if ($("#txt_LICENSE_ID").val() == "") {
                        $("#txt_LICENSE_ID").val($("#txt_PASSPORT_ID").val());
                    }
                }
            });

            $("#txt_LICENSE_ID").change(function () {
                $("#txt_LICENSE_ID").val($("#txt_LICENSE_ID").val().toUpperCase());
            });

            $("#txt_FAMILY_LICENSE_ID").change(function () {
                $("#txt_FAMILY_LICENSE_ID").val($("#txt_FAMILY_LICENSE_ID").val().toUpperCase());
            });

            $("#txt_JOIN_DT").change(function () {
                if ($("#ddl_EMP_CD").val() == "3" || $("#ddl_JPN_CD").val() != "-1") {
                    $("#txt_EXAM_EXPIRE_DT").val($("#txt_JOIN_DT").val());
                }
                else {
                    var exam_days = $("#hid_EXAM_DT").val();
                    var dt = new Date($("#txt_JOIN_DT").val());

                    dt.setDate(dt.getDate() + parseInt(exam_days));
                    var MyDateString = dt.getFullYear() + '/' + fm((dt.getMonth() + 1)) + '/' + fm(dt.getDate())
                    $("#txt_EXAM_EXPIRE_DT").val(MyDateString);
                }

                if ($("#ddl_JPN_CD").val() == "-1") {

                    var currDate = Date.parse(new Date().toDateString());
                    if (Date.parse($("#txt_JOIN_DT").val()).valueOf() > currDate.valueOf()) {
                        alert("入社日在系統日以後的新進員工資料，必須經由採用作業，或其他作業輸入");
                    }
                }
            });

            $("#ddl_JPN_CD").change(function () {
                checkJPN_CD();
            });

            $("#ddl_URGENT_CONTACT_RELATION").change(function () {
                if ($("#ddl_URGENT_CONTACT_RELATION option:selected").val() != "-1") {
                    $("#txt_URGENT_CONTACT_RELATION").val($("#ddl_URGENT_CONTACT_RELATION option:selected").text().split("-")[1]);
                }
                else {
                    //$("#txt_URGENT_CONTACT_RELATION").val("");
                }
            });

            $("#txt_FAMILY_PASSPORT_ID").change(function () {
                if ($("#txt_FAMILY_LICENSE_ID").val() != undefined) {
                    if ($("#ddl_FAMILY_NATION_CD").val() != "TWN") {
                        if ($("#txt_FAMILY_LICENSE_ID").val() == "") {
                            $("#txt_FAMILY_LICENSE_ID").val($("#txt_FAMILY_PASSPORT_ID").val());
                        }
                    }
                }
            });

            $("#ddl_WS_CD").change(function () {
                setPLAN_DESPATCH_DT();
            });

            $("#ddl_EMP_CD").change(function () {
                if ($("#ddl_EMP_CD").val() != "2" && $("#ddl_EMP_CD").val() != "3") {
                    $("#txt_PLAN_DESPATCH_DT").attr("disabled", "disabled");
                }
                setPLAN_DESPATCH_DT();
            });

            $("#ddl_COMPANY_CD").change(function () {
                setPLAN_DESPATCH_DT();
            });

            //部門代號取得部門名稱的ajax
            $("#txt_DEPT_NO").change(function () {
                $.ajax({
                    url: "../commgeo/WFB2GetDeptData.ashx",
                    data: {
                        DEPT_NO: $('#txt_DEPT_NO').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    cache: false,
                    success: function (JData) {
                        if (JData.errMsg != "") {
                            $('#txt_DEPT_NO').val("");
                            $('#txt_DEPT_NAME').val("");
                            $("#txt_DIRECT_HEAD_EMP_ID").val("");
                            $("#txt_DIRECT_HEAD_EMP_NAME").val("");
                            alert(JData.errMsg);
                        }
                        else {
                            $('#txt_DEPT_NAME').val(JData.DEPT_FULL_NAME);
                            $("#txt_DIRECT_HEAD_EMP_ID").val(JData.HEAD_EMP_ID);
                            $("#txt_DIRECT_HEAD_EMP_NAME").val(JData.HEAD_EMP_NAME);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            });

            //直屬主管取得姓名的ajax
            $("#txt_DIRECT_HEAD_EMP_ID").change(function () {
                $.ajax({
                    url: "../commgeo/WFB2GetEmpData.ashx",
                    data: {
                        EMP_ID: $('#txt_DIRECT_HEAD_EMP_ID').val()
                    },
                    type: "GET",
                    cache: false,
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg != "") {
                            $('#txt_DIRECT_HEAD_EMP_ID').val("");
                            $('#txt_DIRECT_HEAD_EMP_NAME').val("");
                            alert(JData.errMsg);
                        }
                        else {
                            $('#txt_DIRECT_HEAD_EMP_NAME').val($.trim(JData.EMP_NAME));
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            });

            //職務代號取得職務名稱的ajax
            $("#txt_PJOB_CD").change(function () {
                $.ajax({
                    url: "../commgeo/WFB2GetPjobData.ashx",
                    data: {
                        PJOB_CD: $('#txt_PJOB_CD').val(),
                        START_DT: $('#txt_JOIN_DT').val()
                    },
                    type: "GET",
                    cache: false,
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg != "") {
                            $('#txt_PJOB_CD').val("");
                            $('#txt_PJOB_DESC').val("");
                            alert(JData.errMsg);
                        }
                        else {
                            $('#txt_PJOB_DESC').val(JData.PJOB_DESC);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            });

            //輪值表代號取得輪值表名稱的ajax
            $("#txt_WORK_SHIFT_CD").change(function () {
                $.ajax({
                    url: "../commgeo/WFB2GetWorkShiftData.ashx",
                    data: {
                        WORK_SHIFT_CD: $('#txt_WORK_SHIFT_CD').val()
                    },
                    type: "GET",
                    cache: false,
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg != "") {
                            $('#txt_WORK_SHIFT_CD').val("");
                            $('#txt_WORK_SHIFT_DESC').val("");
                            $('#txt_CALENDAR_CD').val("");
                            alert(JData.errMsg);
                        }
                        else {
                            $('#txt_WORK_SHIFT_DESC').val(JData.WORK_SHIFT_DESC);
                            $('#txt_CALENDAR_CD').val(JData.CALENDAR_DESC);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            });


        }
        function ChangeTab(tab) {
            $("#tabs").tabs({ active: tab });
        }
        function setPLAN_DESPATCH_DT() {
            if ($("#ddl_EMP_CD").val() == "2" && $("#ddl_COMPANY_CD").val() == "K") {
                if ($("#ddl_WS_CD").val() == "W" && $("#txt_PLAN_DESPATCH_DT").val() == "") {

                    var KZ_CONTRACT_MONTHS = $("#hid_KZ_CONTRACT_MONTHS").val();
                    alert(KZ_CONTRACT_MONTHS);
                    var dt = new Date($("#txt_JOIN_DT").val());
                    dt.setMonth(dt.getMonth() + parseInt(KZ_CONTRACT_MONTHS));
                    var MyDateString = dt.getFullYear() + '/' + dt.getMonth() + '/' + dt.getDate()
                    //設定值並disabled
                    $("#txt_PLAN_DESPATCH_DT").val(MyDateString);
                    $("#txt_PLAN_DESPATCH_DT").attr("disabled", "disabled");
                }
            }

            if ($("#ddl_EMP_CD").val() == "2" && $("#ddl_COMPANY_CD").val() != "K") {
                if ($("#ddl_WS_CD").val() == "W" && $("#txt_PLAN_DESPATCH_DT").val() == "") {
                    var OTH1_CONTRACT_MONTHS = $("#hid_OTH1_CONTRACT_MONTHS").val();
                    var dt = new Date($("#txt_JOIN_DT").val());
                    dt.setMonth(dt.getMonth() + parseInt(OTH1_CONTRACT_MONTHS));
                    var MyDateString = dt.getFullYear() + '/' + dt.getMonth() + '/' + dt.getDate()
                    //設定值並disabled
                    $("#txt_PLAN_DESPATCH_DT").val(MyDateString);
                    $("#txt_PLAN_DESPATCH_DT").attr("disabled", "disabled");
                }
                if ($("#ddl_WS_CD").val() == "S" && $("#txt_PLAN_DESPATCH_DT").val() == "") {
                    var currDate = Date.parse(new Date().toDateString());
                    var compareDate = Date.parse(new Date().getFullYear() + "/" + $("#hid_W_OTH1_CONTRACT_EDT").val());
                    //設定值並disabled
                    if (currDate.valueOf() > compareDate.valueOf()) {
                        $("#txt_PLAN_DESPATCH_DT").val(new Date().getFullYear() + 1 + "/" + $("#hid_W_OTH1_CONTRACT_EDT").val());
                    }
                    else {
                        $("#txt_PLAN_DESPATCH_DT").val(new Date().getFullYear() + "/" + $("#hid_W_OTH1_CONTRACT_EDT").val());
                    }
                    $("#txt_PLAN_DESPATCH_DT").attr("disabled", "disabled");
                }
            }

        }
        function openHRItem() {
            window.showModalDialog("WFB2HB0100_OtherPjob.aspx", self, 'dialogWidth=600px;dialogHeight=400px;scroll=no');
        }

        function getRegion() {
            var zip_cd = $("#txt_REGISTER_ZIP_CD").val();
            OpenSearch('Region_Search.aspx', 'txt_REGISTER_ZIP_CD', 'txt_REGISTER_REGION', 'ZIP_CD=' + zip_cd, 'Y');

            //var json = OpenSearch('Region_Search.aspx', 'txt_REGISTER_ZIP_CD', 'txt_REGISTER_REGION', 'ZIP_CD=' + zip_cd);
            //if (json != undefined) {
            //    $("#txt_REGISTER_COUNTY").val(json.Val1);
            //    $("#txt_REGISTER_ADDR").val(json.Val1 + json.DESC);
            //}

        }
        function getRegion2() {
            var zip_cd = $("#txt_CONTACT_ZIP_CD").val();
            OpenSearch('Region_Search.aspx', 'txt_CONTACT_ZIP_CD', 'txt_CONTACT_REGION', 'ZIP_CD=' + zip_cd, 'Y');
            //var json = OpenSearch('Region_Search.aspx', 'txt_CONTACT_ZIP_CD', 'txt_CONTACT_REGION', 'ZIP_CD=' + zip_cd);
            //if (json != undefined) {
            //    $("#txt_CONTACT_COUNTY").val(json.Val1);
            //    $("#txt_CONTACT_ADDR").val(json.Val1 + json.DESC);
            //}
        }
        //openSearch之後會呼叫的function
        function Region_Search(obj_cd, obj_desc, value) {
            var returnValue = value;
            if (returnValue == undefined) {
                returnValue = window.returnValue;
            }
            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#" + obj_cd).val(obj.CD);
                $("#" + obj_desc).val(obj.DESC);

                if (obj_cd == 'txt_REGISTER_ZIP_CD') {
                    $("#txt_REGISTER_COUNTY").val(obj.Val1);
                    $("#txt_REGISTER_ADDR").val(obj.Val1 + obj.DESC);
                }
                if (obj_cd == 'txt_CONTACT_ZIP_CD') {
                    $("#txt_CONTACT_COUNTY").val(obj.Val1);
                    $("#txt_CONTACT_ADDR").val(obj.Val1 + obj.DESC);
                }
            }
        }



        //薪資匯款帳號檢核
        function CheckSALARY_ACCOUNT(source, arguments) {
            var re = /^[\d]+$/;
            if ($("#txt_SALARY_ACCOUNT_NO3").val().trim() != "") {
                if (!re.test($("#txt_SALARY_ACCOUNT_NO3").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
        }
        //家庭儲存前檢查
        function saveFamCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupFam")) {
                if ($("#ddl_FAMILY_NATION_CD").val() == "TWN" && $("#txt_FAMILY_LICENSE_ID").val() != undefined) {
                    if (checkLicenseID($("#txt_FAMILY_LICENSE_ID").val()))
                        BlockUI();
                    else {
                        alert("眷屬身份證號不符合編碼原則");
                        processed = false;
                    }
                }
                else
                    BlockUI();

            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }
        //教育儲存前檢查
        function saveEduCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupEdu")) {
                if ($("input[id='cb_IS_SALARY_SCHOOL']:checked").length > 1) {
                    alert("只能有一筆資料勾選為敘薪學歷");
                    processed = false;
                }
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }
        //經歷儲存前檢查
        function saveExpCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupExp")) {

                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }
        //總儲存檢查
        function saveCheck() {
            var processed = true;
            var msg = "";
            if (Page_ClientValidate("GroupA")) {

                if ($("#ddl_EMP_CD").val() == "3") {
                    if ($("#txt_PLAN_DESPATCH_DT").val() == "") {
                        msg += "員工區分為借調人員時預計派遣日為必輸入\r\n";
                    }
                }

                if ($("input[id='cb_IS_SALARY_SCHOOL']:checked").length != 1) {
                    msg += "必須有一筆資料勾選為敘薪學歷\r\n";
                    processed = false;
                }

                if ($("#ddl_JPN_CD").val() == "-1") {

                    var currDate = Date.parse(new Date().toDateString());
                    if (Date.parse($("#txt_JOIN_DT").val()).valueOf() > currDate.valueOf()) {
                        msg += "入社日在系統日以後的新進員工資料，必須經由採用作業，或其他作業輸入\r\n";
                        processed = false;
                    }
                }

                if ($("#ddl_NATION_CD").val() == "TWN") {
                    if ($("#txt_LICENSE_ID").val() != "") {
                        if (!checkLicenseID($("#txt_LICENSE_ID").val())) {
                            msg += "身份證號不符合編碼原則\r\n";
                            processed = false;
                        }
                    }
                }

                if ($("#ddl_EMP_CD").val() == "2") {
                    if ($("#txt_PJOB_CD").val().substring(0, 1) != "P") {
                        msg += "期間社員的職務代號必須是P開頭的專業職代號\r\n";
                        $("#txt_PJOB_CD").val("");
                        $("#txt_PJOB_DESC").val("");
                        processed = false;
                    }
                }

                if ($("#rb_SALARY:checked").val()) {
                    var mail = $("#txt_PERSONAL_EMAIL").val().trim();
                    if (mail.length == 0) {
                        msg += "個人Email不可空白\r\n";
                        processed = false;
                    } else if (checkEmail(mail) == false) {
                        msg += "個人Email格式錯誤\r\n";
                        processed = false;
                    }
                }

                if ($("#rb_SALARY_2:checked").val()) {
                    var mail = $("#txt_COMPANY_EMAIL").val().trim();
                    if (mail.length == 0) {
                        msg += "公司Email不可空白\r\n";
                        processed = false;
                    } else if (checkEmail(mail) == false) {
                        msg += "公司Email格式錯誤\r\n";
                        processed = false;
                    }
                }

                /*
                //薪資匯款帳號有輸入時,需同時有值
                if (
                    $.trim($("#txt_SALARY_ACCOUNT_NO1").val()) == "" &&
                    $.trim($("#txt_SALARY_ACCOUNT_NO2").val()) == "" &&
                    $.trim($("#txt_SALARY_ACCOUNT_NO3").val()) == "") {
                } else if (
                    $.trim($("#txt_SALARY_ACCOUNT_NO1").val()) != "" &&
                    $.trim($("#txt_SALARY_ACCOUNT_NO2").val()) != "" &&
                    $.trim($("#txt_SALARY_ACCOUNT_NO3").val()) != "") {
                }
                else {
                    msg += "薪資匯款帳號3個欄位須同時有值\r\n";
                    processed = false;
                }
                */

            }
            else
                return;

            if (!processed) {
                $.unblockUI();
                alert(msg);
                return;
            } else {
                BlockUI();
            }

            return processed;
        }
        function getDept() {
            var json = OpenDeptFullSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');
            if (json != undefined) {
                if ($("#ddl_IS_UPD_HEAD").val() == "Y") {
                    $("#txt_DIRECT_HEAD_EMP_ID").val(json.HEAD_EMP_ID);
                    $("#txt_DIRECT_HEAD_EMP_NAME").val(json.HEAD_EMP_NAME);
                }

            }
        }

        function returnDEPTValueToPage(dept_no, dept_name, value) {
            var returnValue = value;
            if (value == undefined) {
                returnValue = 'undefined';
            }
            //alert(returnValue);

            if (!(typeof returnValue === 'undefined')) {
                var obj = jQuery.parseJSON(returnValue);
                $("#" + dept_no).val(obj.DEPT_NO);
                $("#" + dept_name).val(obj.DEPT_FULL_NAME);

                if ($("#ddl_IS_UPD_HEAD").val() == "Y") {
                    $("#txt_DIRECT_HEAD_EMP_ID").val(obj.HEAD_EMP_ID);
                    $("#txt_DIRECT_HEAD_EMP_NAME").val(obj.HEAD_EMP_NAME);
                }
            }
        }

        function getPjob(statue) {
            if ($("#txt_JOIN_DT").val() == "") {
                alert("必須先輸入入社日，才可以輸入資格代號");
                return;
            }
            else if ($("#ddl_WS_CD").val() == "-1" || $("#ddl_LEVEL_CD").val() == "-1" || $("#ddl_LEVEL_CD").val() == "") {
                alert("必須先輸入職種及資格，才可以輸入職務代號");
                return;
            }
            else {
                if (statue == "open") {
                    var json = OpenSearch('Pjob_Search.aspx', 'txt_PJOB_CD', 'txt_PJOB_DESC', 'WS_CD=' + $("#ddl_WS_CD").val() +
                        '&LEVEL_CD=' + $("#ddl_LEVEL_CD").val() + '&PJOB_CD=' + $("#txt_PJOB_CD").val() + '&START_DT=' + $("#txt_JOIN_DT").val());
                    if ($("#ddl_EMP_CD").val() == "2") {
                        if ($("#txt_PJOB_CD").val().substring(0, 1) != "P") {
                            alert("期間社員的職務代號必須是P開頭的專業職代號");
                            $("#txt_PJOB_CD").val("");
                            $("#txt_PJOB_DESC").val("");
                        }
                    }
                }
                else if (statue == "keyin")
                    return true;
            }

        }

        function getWorkShift() {
            var json = OpenSearch('WorkShift_Search.aspx', 'txt_WORK_SHIFT_CD', 'txt_WORK_SHIFT_DESC', 'WORK_SHIFT_CD=' + $("#txt_WORK_SHIFT_CD").val());
            if (json != undefined) {
                $("#txt_CALENDAR_CD").val(json.Val1 + "  " + json.Val2)
            }
        }

        function checkJPN_CD() {
            if ($("#ddl_JPN_CD").val() == "-1") {
                $("#txt_START_DT").attr("disabled", "disabled");
                $("#txt_END_DT").attr("disabled", "disabled");
                $("#ddl_RENT_SUBSIDY").attr("disabled", "disabled");
            }
            else {
                $("#txt_START_DT").removeAttr("disabled");
                $("#txt_END_DT").removeAttr("disabled");
                $("#ddl_RENT_SUBSIDY").removeAttr("disabled");
            }

        }

        function copyRegister() {
            $("#txt_CONTACT_ZIP_CD").val($("#txt_REGISTER_ZIP_CD").val());
            $("#txt_CONTACT_COUNTY").val($("#txt_REGISTER_COUNTY").val());
            $("#txt_CONTACT_REGION").val($("#txt_REGISTER_REGION").val());
            $("#txt_CONTACT_ADDR").val($("#txt_REGISTER_ADDR").val());
            $("#txt_CONTACT_TEL").val($("#txt_REGISTER_TEL").val());
        }
        function cancelCheck() {
            var msg = $('#hidwfb299_Cancel_ConfirmMessage').val() + "\r\n 若有新增資料，請點選儲存!";
            return confirm(msg);
        }

        //檢驗體重只能輸入<999.9
        function CheckWEIGHT(source, arguments) {
            var value = $.trim(arguments.Value);
            if (isNaN(value) == false) {
                if (value > 999.9) {
                    arguments.IsValid = false;
                } else {
                    arguments.IsValid = true;
                }
                return;
            }
        }

    </script>
    <style type="text/css">
        .input {
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="16%" />
                                <col width="20%" />
                                <col width="12%" />
                                <col width="21%" />
                                <col width="14%" />
                                <col width="10%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="80px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_NAME%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="20" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="MainValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EMP_NAME%>"
                                            ControlToValidate="txt_EMP_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:HiddenField ID="hid_EMP_NAME" runat="server" />
                                    </td>
                                    <%-- 編輯照片 --%>
                                    <th class="Body_TH3">
                                        <asp:Label ID="lb_EMP_PHOTO" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_PHOTO%>"></asp:Label>:
                                    </th>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_NATION_JPN_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_NATION_JPN_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_NATION_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                                        <asp:DropDownList ID="ddl_JPN_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="MainValidator14" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_NATION_CD%>"
                                            ControlToValidate="ddl_NATION_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BIRTH_BLOOD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BIRTH_BLOOD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_BIRTH_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="MainValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_BIRTH_DT%>"
                                            ControlToValidate="txt_BIRTH_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="生日輸入日期錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_BIRTH_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <!--出生日期的年度不可以大於系統年-14 -->
                                        <asp:CustomValidator ID="CustomValidator7" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="出生日期的年度不可大於系統年-14 " ClientValidationFunction="CheckBirthday" ForeColor="Red"
                                            ControlToValidate="txt_BIRTH_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                                        <asp:HiddenField ID="hid_BIRTH_DT" runat="server" />
                                        <asp:DropDownList ID="ddl_BLOOD_TYPE" runat="server" CssClass="MandatoryField">
                                            <asp:ListItem Text="A" Value="A"></asp:ListItem>
                                            <asp:ListItem Text="B" Value="B"></asp:ListItem>
                                            <asp:ListItem Text="O" Value="O"></asp:ListItem>
                                            <asp:ListItem Text="AB" Value="AB"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <%-- 照片位置 --%>
                                    <td rowspan="3" style="border-right: #cccc99 1px solid; border-top: #cccc99 1px solid; border-left: #cccc99 1px solid; border-bottom: #cccc99 1px solid;" align="center">
                                        <asp:Image ID="EmpPhoto" runat="server" Width="120px" Height="120px" ImageUrl="" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SEX_ARMY" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SEX_ARMY%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SEX_CD" runat="server" CssClass="MandatoryField">
                                            <asp:ListItem Text="1-男" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="2-女" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddl_ARMY_CD" runat="server"></asp:DropDownList>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HEIGHT_WEIGHT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_HEIGHT_WEIGHT%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_HEIGHT" runat="server" MaxLength="3" Width="35px" ClientIDMode="Static" CssClass="MandatoryField num3"></asp:TextBox>
                                        <asp:TextBox ID="txt_WEIGHT" runat="server" MaxLength="5" Width="35px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="MainValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_HEIGHT%>"
                                            ControlToValidate="txt_HEIGHT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="MainValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_WEIGHT%>"
                                            ControlToValidate="txt_WEIGHT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="身高只能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_HEIGHT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="體重只能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_WEIGHT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator8" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="體重只能輸入小於999.9" ClientValidationFunction="CheckWEIGHT" ForeColor="Red"
                                            ControlToValidate="txt_WEIGHT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LICENSE_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LICENSE_ID2%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LICENSE_ID" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="MainValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_LICENSE_ID%>"
                                            ControlToValidate="txt_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:HiddenField ID="hid_LICENSE_ID" runat="server" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="身份證字號/居留證號只能輸入英數字"
                                            ControlToValidate="txt_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BIRTHPLACE" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BIRTHPLACE%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_BIRTHPLACE" runat="server" MaxLength="30" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="MainValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_BIRTHPLACE%>"
                                            ControlToValidate="txt_BIRTHPLACE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PASSPORT_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PASSPORT_ID%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PASSPORT_ID" runat="server" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="護照號碼只能輸入英數字"
                                            ControlToValidate="txt_PASSPORT_ID" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ACCOUNT_BANK" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SALARY_ACCOUNT_BANK%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_ACCOUNT_BANK" runat="server" MaxLength="3" Width="30px" ClientIDMode="Static" OnTextChanged="txt_SALARY_ACCOUNT_BANK_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <input id="Button5" type="button" value="..." onclick="OpenSearch('CommCode_Search.aspx', 'txt_SALARY_ACCOUNT_BANK', 'txt_SALARY_ACCOUNT_BANK_NAME', 'MAIN_CD=SALARY_ACCOUNT_BANK');" />
                                        <asp:TextBox ID="txt_SALARY_ACCOUNT_BANK_NAME" runat="server" Width="90px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    </td>
                                    <%-- 照片上傳 --%>
                                    <td colspan="3" align="left">
                                        <asp:FileUpload ID="FileUpload1" runat="server" Width="160px" />
                                        <asp:Button ID="btn_photo_upload" runat="server" Text="<%$Resources:Resource,wfb2hb_btn_photo_upload%>" OnClick="btn_photo_upload_Click" />

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ACCOUNT_BANK" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_ACCOUNT_BANK%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_ACCOUNT_BRANCH" runat="server" MaxLength="4" ClientIDMode="Static" Width="40px"></asp:TextBox>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ACCOUNT_NO" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SALARY_ACCOUNT_NO%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        
                                        <asp:TextBox ID="txt_SALARY_ACCOUNT_NO3" runat="server" MaxLength="14" ClientIDMode="Static" Width="120px"></asp:TextBox>
                                        <%-- 
                                            <asp:TextBox ID="txt_SALARY_ACCOUNT_NO1" runat="server" MaxLength="3" ClientIDMode="Static" Width="30px"></asp:TextBox>-
                                        <asp:TextBox ID="txt_SALARY_ACCOUNT_NO2" runat="server" MaxLength="2" ClientIDMode="Static" Width="22px"></asp:TextBox>-
                                        <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="薪資匯款帳號只能輸入數字" ClientValidationFunction="CheckSALARY_ACCOUNT" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_ACCOUNT_NO1" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        
                                            <asp:CustomValidator ID="CustomValidator9" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hb_format_SALARY_ACCOUNT_NO1%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_ACCOUNT_NO1" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator10" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hb_format_SALARY_ACCOUNT_NO2%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_ACCOUNT_NO2" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                            --%>
                                        
                                        <asp:CustomValidator ID="CustomValidator11" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hb_format_SALARY_ACCOUNT_NO3%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_ACCOUNT_NO3" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_REMARK%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:TextBox ID="txt_REMARK" runat="server" MaxLength="120" Width="700px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="10">

                        <aces:Btn ID="WFB2HB0100Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HB0100Save%>" OnClick="WFB2HB0100Save_Click" OnClientClick="return saveCheck();" />

                        <%--
                            <asp:Button ID="WFB2HB0100Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClick="WFB2HB0100Save_Click" OnClientClick="return saveCheck();" />
                        --%>
                        <asp:Button ID="WFB2HB0100Cancel" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0100Cancel%>" Visible="true" OnClientClick="return cancelCheck();" OnClick="WFB2HB0100Cancel_Click" />

                    </td>
                </tr>
            </table>


            <div id="tabs" style="width: 1020px">
                <ul>
                    <li><a href="#tabs-1">【任職資料一】</a></li>
                    <li><a href="#tabs-2">【任職資料二】</a></li>
                    <li><a href="#tabs-3">【戶籍/現居/Mail】</a></li>
                    <li><a href="#tabs-4">【緊急連絡】</a></li>
                    <li><a href="#tabs-5">【扶養&所得稅】</a></li>
                    <li><a href="#tabs-6">【家庭成員】</a></li>
                    <li><a href="#tabs-7">【學歷】</a></li>
                    <li><a href="#tabs-8">【經歷】</a></li>
                    <li><a href="#tabs-9">【外籍赴任】</a></li>
                </ul>
                <div id="tabs-1">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                        <colgroup>
                            <col width="11%" />
                            <col width="25%" />
                            <col width="12%" />
                            <col width="15%" />
                            <col width="14%" />
                            <col width="23%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_JOIN_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_JOIN_DT2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_JOIN_DT" runat="server" MaxLength="8" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date" OnTextChanged="txt_JOIN_DT_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="MainValidator16" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_JOIN_DT%>"
                                        ControlToValidate="txt_JOIN_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="入社日期輸入錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_JOIN_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_EXAM_EXPIRE_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EXAM_EXPIRE_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_EXAM_EXPIRE_DT" runat="server" MaxLength="8" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="MainValidator17" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EXAM_EXPIRE_DT%>"
                                        ControlToValidate="txt_EXAM_EXPIRE_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator5" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="試用期滿日輸入錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_EXAM_EXPIRE_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    <asp:HiddenField ID="hid_EXAM_DT" runat="server" ClientIDMode="Static" />
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_IS_MASTER" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_IS_MASTER%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_IS_MASTER" runat="server" CssClass="MandatoryField">
                                        <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                        <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_COMPANY_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_COMPANY_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="MainValidator18" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_COMPANY_CD%>"
                                        ControlToValidate="ddl_COMPANY_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </td>
                                <td class="Body_TD3"></td>
                                <td class="Body_TD3"></td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_IS_UPD_HEAD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_IS_UPD_HEAD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_IS_UPD_HEAD" runat="server" CssClass="MandatoryField" ClientIDMode="Static">
                                        <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PLANT_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_PLANT_CD" runat="server" CssClass="MandatoryField">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="MainValidator19" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_PLANT_CD%>"
                                        ControlToValidate="ddl_PLANT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </td>
                                <td class="Body_TD3"></td>
                                <td class="Body_TD3"></td>

                                <th class="Body_TH3">
                                    <asp:Label ID="lb_DIRECT_HEAD_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DIRECT_HEAD_EMP_ID%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_DIRECT_HEAD_EMP_ID" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                    <input id="bt_HEAD_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_DIRECT_HEAD_EMP_ID', 'txt_DIRECT_HEAD_EMP_NAME', 'N');" />
                                    <asp:TextBox ID="txt_DIRECT_HEAD_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" Width="100px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="MainValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_DIRECT_HEAD_EMP_ID%>"
                                        ControlToValidate="txt_DIRECT_HEAD_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DEPT_NO%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="3">
                                    <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="84px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_DEPT_NO%>"
                                        ControlToValidate="txt_DEPT_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <%--<input id="bt_DEPT_SEARCH" type="button" value="..." onclick="getDept();" />--%>
                                    <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N', 'Y');" />
                                    <asp:TextBox ID="txt_DEPT_NAME" runat="server" MaxLength="10" BorderWidth="0" Width="380px" ClientIDMode="Static"></asp:TextBox>

                                </td>

                                <th class="Body_TH3">
                                    <asp:Label ID="lb_OVERTIME_HEALTH_YEAR" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_OVERTIME_HEALTH_YEAR%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_OVERTIME_CTL_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_OVERTIME_CTL_CD%>"
                                        ControlToValidate="ddl_OVERTIME_CTL_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txt_HEALTH_YEAR" runat="server" MaxLength="8" Width="50px" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator31" runat="server" ErrorMessage="體檢年度只能輸入數字，需為4碼"
                                        ControlToValidate="txt_HEALTH_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WS_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_WS_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="MainValidator20" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_WS_CD%>"
                                        ControlToValidate="ddl_WS_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>

                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PLAN_DESPATCH_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PLAN_DESPATCH_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PLAN_DESPATCH_DT" runat="server" MaxLength="8" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator6" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="預計派遣日輸入錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_PLAN_DESPATCH_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hid_KZ_CONTRACT_MONTHS" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hid_OTH1_CONTRACT_MONTHS" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hid_W_OTH1_CONTRACT_EDT" runat="server" ClientIDMode="Static" />
                                </td>

                                <th class="Body_TH3">
                                    <asp:Label ID="lb_IS_DUTY_CHECK" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_IS_DUTY_CHECK%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_IS_DUTY_CHECK" runat="server" CssClass="MandatoryField">
                                        <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_EMP_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="MainValidator21" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EMP_CD%>"
                                        ControlToValidate="ddl_EMP_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>

                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_BE_DESPATCH_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BE_DESPATCH_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_BE_DESPATCH_DT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>

                                <th class="Body_TH3">
                                    <asp:Label ID="lb_MODEL_YEAR" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_MODEL_YEAR%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_MODEL_YEAR" runat="server" MaxLength="4" Width="50px" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server" ErrorMessage="模範員工年度只能輸入數字，需為4碼"
                                        ControlToValidate="txt_MODEL_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_LEVEL_GRADE" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LEVEL_GRADE%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_LEVEL_CD" runat="server" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_LEVEL_CD_SelectedIndexChanged" AutoPostBack="true" ClientIDMode="Static">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="MainValidator22" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_LEVEL_CD%>"
                                        ControlToValidate="ddl_LEVEL_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    <asp:DropDownList ID="ddl_GRADE_CD" runat="server" CssClass="MandatoryField">
                                    </asp:DropDownList>


                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_KEEP_DESPATCH_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_KEEP_DESPATCH_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_KEEP_DESPATCH_DT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>

                                <th class="Body_TH3">
                                    <asp:Label ID="lb_HONOR_YEAR" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_HONOR_YEAR%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_HONOR_YEAR" runat="server" MaxLength="8" Width="50px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PJOB_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PJOB_CD" runat="server" MaxLength="10" Width="64px" onclick="return getPjob('keyin');" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                    <input id="Button19" type="button" value="..." onclick="getPjob('open');" />
                                    <asp:TextBox ID="txt_PJOB_DESC" runat="server" BorderWidth="0" ClientIDMode="Static" Width="100px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="MainValidator23" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_PJOB_CD%>"
                                        ControlToValidate="txt_PJOB_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <%-- 
                                    <input type="button" name="button1" value="兼任" disabled="disabled">
                                    --%>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_BE_CONTRACT_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BE_CONTRACT_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_BE_CONTRACT_DT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>


                                <th class="Body_TH3">
                                    <asp:Label ID="lb_UNION_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_UNION_PJOB_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_UNION_PJOB_CD" runat="server">
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WORK_SHIFT_CD2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_WORK_SHIFT_CD" runat="server" MaxLength="10" Width="64px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                    <input id="btn_WORK_SHIFT_CD" type="button" value="..." onclick="getWorkShift();" />
                                    <asp:TextBox ID="txt_WORK_SHIFT_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="MainValidator24" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_WORK_SHIFT_CD%>"
                                        ControlToValidate="txt_WORK_SHIFT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>

                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_BE_EMP_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BE_EMP_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_BE_EMP_DT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_WORK_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WORK_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_WORK_CD" runat="server" CssClass="MandatoryField">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="MainValidator25" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_WORK_CD%>"
                                        ControlToValidate="ddl_WORK_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_CALENDAR_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_CALENDAR_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_CALENDAR_CD" runat="server" Width="150px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td class="Body_TD3"></td>
                                <td class="Body_TD3"></td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_ACC_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_ACC_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_ACC_CD" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="tabs-2">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                        <tr>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_RECENT_LEVEL_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_LEVEL_DT%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_RECENT_LEVEL_DT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                            </td>

                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_RECENT_PJOB_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_PJOB_DT%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_RECENT_PJOB_DT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_RECENT_DEPT_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_DEPT_DT%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_RECENT_DEPT_DT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                            </td>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_RECENT_DIV_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_DIV_DT%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_RECENT_DIV_DT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                            </td>
                        </tr>

                        <tr>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_RECENT_LEVEL_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_LEVEL_WORK_DAYS2%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_RECENT_LEVEL_WORK_DAYS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>

                            </td>

                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_RECENT_PJOB_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_PJOB_WORK_DAYS%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_RECENT_PJOB_WORK_DAYS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_RECENT_DEPT_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_DEPT_WORK_DAYS%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_RECENT_DEPT_WORK_DAYS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_RECENT_DIV_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_DIV_WORK_DAYS%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_RECENT_DIV_WORK_DAYS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_STUDENT_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_STUDENT_WORK_DAYS%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_STUDENT_WORK_DAYS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_K_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_K_WORK_DAYS%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_K_WORK_DAYS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                            <th class="Body_TH3" style="width: 14%;">
                                <asp:Label ID="lb_T_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_T_WORK_DAYS%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_T_WORK_DAYS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_WORK_YEARS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WORK_YEARS2%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_WORK_YEARS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_SERVICE_YEARS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SERVICE_YEARS%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_SERVICE_YEARS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_LEAVE_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LEAVE_DT%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_LEAVE_DT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_LEAVE_REASON_DESC" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LEAVE_REASON_DESC%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_LEAVE_REASON_DESC" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_PLAN_RETENTION_EDT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PLAN_RETENTION_EDT%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_PLAN_RETENTION_EDT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_RETENTION_EDT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RETENTION_EDT%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_RETENTION_EDT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_TRANSFER_SDT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TRANSFER_SDT%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_TRANSFER_SDT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_TRANSFER_REASON_DESC" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TRANSFER_REASON_DESC%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_TRANSFER_REASON_DESC" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_PLAN_TRANSFER_EDT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PLAN_TRANSFER_EDT%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_PLAN_TRANSFER_EDT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                            <th class="Body_TH3" style="width: 14%;">
                                <asp:Label ID="lb_TRANSFER_EDT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TRANSFER_EDT%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_TRANSFER_EDT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_BACK_SCHOOL_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BACK_SCHOOL_DT%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_BACK_SCHOOL_DT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>
                            <th class="Body_TH3" style="width: 13%;">
                                <asp:Label ID="lb_BACK_PLANT_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BACK_PLANT_DT%>"></asp:Label>:</th>
                            <td class="Body_TD3" style="width: 12%;">
                                <asp:TextBox ID="txt_BACK_PLANT_DT" runat="server" MaxLength="8" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                            </td>

                        </tr>

                    </table>
                </div>
                <div id="tabs-3">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                        <colgroup>
                            <col width="12%" />
                            <col width="33%" />
                            <col width="12%" />
                            <col width="13%" />
                            <col width="12%" />
                            <col width="18%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <td align="left" class="Body_label" colspan="6">【戶籍】
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TD3">
                                    <input id="Button7" type="button" value="郵遞區號" onclick="getRegion();" />:</th>
                                <td align="left" class="Body_TD3" colspan="5">
                                    <asp:TextBox ID="txt_REGISTER_ZIP_CD" runat="server" MaxLength="5" Width="50px" ClientIDMode="Static" CssClass="MandatoryField" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_REGISTER_COUNTY" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" CssClass="MandatoryField" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_REGISTER_REGION" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" CssClass="MandatoryField" Enabled="false"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="戶籍郵遞區號只能輸入數字"
                                        ControlToValidate="txt_REGISTER_ZIP_CD" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression="^[0-9]+$" Display="None"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="MainValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_REGISTER_ZIP_CD%>"
                                        ControlToValidate="txt_REGISTER_ZIP_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="MainValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_REGISTER_ZIP_CD%>"
                                        ControlToValidate="txt_REGISTER_COUNTY" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="MainValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_REGISTER_ZIP_CD%>"
                                        ControlToValidate="txt_REGISTER_REGION" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_REGISTER_ADD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_REGISTER_ADD%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="3">
                                    <asp:TextBox ID="txt_REGISTER_ADDR" runat="server" MaxLength="255" Width="500px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>

                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_REGISTER_TEL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_REGISTER_TEL2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_REGISTER_TEL" runat="server" MaxLength="10" Width="140px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <td align="left" class="Body_label" colspan="6">【通訊】
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TD3">
                                    <input id="Button1" type="button" value="郵遞區號" onclick="getRegion2();" />:</th>
                                <td align="left" class="Body_TD3" colspan="5">
                                    <asp:TextBox ID="txt_CONTACT_ZIP_CD" runat="server" MaxLength="5" Width="50px" ClientIDMode="Static" CssClass="num3" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_CONTACT_COUNTY" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_CONTACT_REGION" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="通訊郵遞區號只能輸入數字"
                                        ControlToValidate="txt_CONTACT_ZIP_CD" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression="^[0-9]+$" Display="None"></asp:RegularExpressionValidator>
                                    <input id="Button2" type="button" value="同戶籍地" onclick="copyRegister();" />
                                </td>
                            </tr>

                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_CONTACT_ADDR" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_CONTACT_ADDR2%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="3">
                                    <asp:TextBox ID="txt_CONTACT_ADDR" runat="server" MaxLength="255" Width="630px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_CONTACT_TEL2" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_CONTACT_TEL2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_CONTACT_TEL" runat="server" MaxLength="10" Width="140px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PERSONAL_EMAIL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PERSONAL_EMAIL%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PERSONAL_EMAIL" runat="server" MaxLength="50" Width="280px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_MOBILE_TEL_1" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_MOBILE_TEL_1%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_MOBILE_TEL_1" runat="server" MaxLength="10" Width="140px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_MOBILE_TEL_2" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_MOBILE_TEL_2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_MOBILE_TEL_2" runat="server" MaxLength="10" Width="140px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_SALARY" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SALARY%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="5">
                                    <asp:RadioButton ID="rb_SALARY" runat="server" GroupName="SALARY" ClientIDMode="Static" />
                                </td>
                            </tr>

                            <tr>
                                <td align="left" class="Body_label" colspan="6">【公司】
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_COMPANY_EMAIL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_COMPANY_EMAIL%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="5">
                                    <asp:TextBox ID="txt_COMPANY_EMAIL" runat="server" MaxLength="50" Width="280px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <%--                            <th class="Body_TH3">
                                <asp:Label ID="lb_COMPANY_EXT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_COMPANY_EXT%>"></asp:Label>:</th>
                            <td class="Body_TD3">
                                <asp:TextBox ID="txt_COMPANY_EXT" runat="server" MaxLength="10" Width="140px" ClientIDMode="Static"></asp:TextBox>
                            </td>--%>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_SALARY_2" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SALARY%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="5">
                                    <asp:RadioButton ID="rb_SALARY_2" runat="server" GroupName="SALARY" ClientIDMode="Static" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="tabs-4">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                        <colgroup>
                            <col width="15%" />
                            <col width="85%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_URGENT_CONTACT_NAME" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_URGENT_CONTACT_NAME2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_URGENT_CONTACT_NAME" runat="server" MaxLength="30" Width="140px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="MainValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_URGENT_CONTACT_NAME%>"
                                        ControlToValidate="txt_URGENT_CONTACT_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_URGENT_CONTACT_TEL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_URGENT_CONTACT_TEL%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_URGENT_CONTACT_TEL" runat="server" MaxLength="14" Width="140px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="MainValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_URGENT_CONTACT_TEL%>"
                                        ControlToValidate="txt_URGENT_CONTACT_TEL" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_URGENT_CONTACT_RELATION" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_URGENT_CONTACT_RELATION%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_URGENT_CONTACT_RELATION" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    <asp:TextBox ID="txt_URGENT_CONTACT_RELATION" runat="server" MaxLength="14" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="MainValidator13" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_URGENT_CONTACT_RELATION%>"
                                        ControlToValidate="txt_URGENT_CONTACT_RELATION" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="tabs-5">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                        <colgroup>
                            <col width="15%" />
                            <col width="85%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RELATIVES" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RELATIVES%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RELATIVES" runat="server" MaxLength="5" Width="70px" ClientIDMode="Static" CssClass="num2"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator10" runat="server" ErrorMessage="扶養親屬人數只能輸入數字"
                                        ControlToValidate="txt_RELATIVES" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression="([\d])" Display="None"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_INCOME_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_INCOME_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_INCOME_CD" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="MainValidator15" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_INCOME_CD%>"
                                        ControlToValidate="ddl_INCOME_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="tabs-6">
                    <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                        <tr>
                            <td class="Body_TD3" style="text-align: right">
                                <asp:Button ID="btn_family_add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="btn_family_add_Click" />
                                <asp:Button ID="btn_family_delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClick="btn_family_delete_Click" />
                                <asp:Button ID="btn_family_mod" runat="server" Text="<%$Resources:Resource,btn_edit%>" OnClick="btn_family_mod_Click" />
                                <asp:Button ID="btn_family_confirm" runat="server" Text="<%$Resources:Resource,btn_confirm%>" Visible="false" OnClick="btn_family_confirm_Click" OnClientClick="return saveFamCheck();" />
                                <asp:Button ID="btn_family_cancel" runat="server" Text="<%$Resources:Resource,btn_cancel%>" Visible="false" OnClick="btn_family_cancel_Click" OnClientClick="return confirm($('#hidwfb299_Cancel_ConfirmMessage').val())" />
                            </td>
                        </tr>
                    </table>

                    <asp:GridView ID="gv_result" runat="server" AllowSorting="true" ClientIDMode="Static"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </FooterTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_NATION_CD%>" SortExpression="FAMILY_NATION_CD" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_NATION_CD" runat="server" Text='<%#Bind("FAMILY_NATION_DESC")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_FAMILY_NATION_CD" runat="server" Text='<%#Bind("FAMILY_NATION_DESC")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddl_FAMILY_NATION_CD" runat="server" CssClass="MandatoryField" ClientIDMode="Static">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator61" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_NATION_CD%>"
                                        ControlToValidate="ddl_FAMILY_NATION_CD" ForeColor="Red" ValidationGroup="GroupFam" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_SEX_CD%>" SortExpression="FAMILY_SEX_CD" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_SEX_CD" runat="server" Text='<%#Bind("FAMILY_SEX_DESC")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_FAMILY_SEX_CD" runat="server" Text='<%#Bind("FAMILY_SEX_DESC")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddl_FAMILY_SEX_CD" runat="server" CssClass="MandatoryField">
                                        <asp:ListItem Text="1-男" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2-女" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator62" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_SEX_CD%>"
                                        ControlToValidate="ddl_FAMILY_SEX_CD" ForeColor="Red" ValidationGroup="GroupFam" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_LICENSE_ID%>" SortExpression="FAMILY_LICENSE_ID" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_LICENSE_ID" runat="server" Text='<%#Bind("FAMILY_LICENSE_ID")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_FAMILY_LICENSE_ID" runat="server" Text='<%#Bind("FAMILY_LICENSE_ID")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_FAMILY_LICENSE_ID" runat="server" MaxLength="10" Width="100px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator63" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_LICENSE_ID%>"
                                        ControlToValidate="txt_FAMILY_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupFam" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator64" runat="server" ErrorMessage="眷屬身份證字號/居留證號只能輸入英數字"
                                        ControlToValidate="txt_FAMILY_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupFam"
                                        ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_PASSPORT_ID%>" SortExpression="FAMILY_PASSPORT_ID" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_PASSPORT_ID" runat="server" Text='<%#Bind("FAMILY_PASSPORT_ID")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_FAMILY_PASSPORT_ID" runat="server" MaxLength="20" Width="100px" Text='<%#Bind("FAMILY_PASSPORT_ID")%>'></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator65" runat="server" ErrorMessage="眷屬護照號碼只能輸入英數字"
                                        ControlToValidate="txt_FAMILY_PASSPORT_ID" ForeColor="Red" ValidationGroup="GroupFam"
                                        ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_FAMILY_PASSPORT_ID" runat="server" MaxLength="20" Width="100px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator66" runat="server" ErrorMessage="眷屬護照號碼只能輸入英數字"
                                        ControlToValidate="txt_FAMILY_PASSPORT_ID" ForeColor="Red" ValidationGroup="GroupFam"
                                        ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_NAME%>" SortExpression="FAMILY_NAME" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_NAME" runat="server" Text='<%#Bind("FAMILY_NAME")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_FAMILY_NAME" runat="server" MaxLength="30" Width="80px" Text='<%#Bind("FAMILY_NAME")%>' CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator67" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_NAME%>"
                                        ControlToValidate="txt_FAMILY_NAME" ForeColor="Red" ValidationGroup="GroupFam" Display="None"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_FAMILY_NAME" runat="server" MaxLength="30" Width="80px" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator68" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_NAME%>"
                                        ControlToValidate="txt_FAMILY_NAME" ForeColor="Red" ValidationGroup="GroupFam" Display="None"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_RELATION%>" SortExpression="FAMILY_RELATION" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_RELATION" runat="server" Text='<%#Bind("FAMILY_RELATION_DESC")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddl_FAMILY_RELATION" runat="server" CssClass="MandatoryField">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hid_FAMILY_RELATION" runat="server" Value='<%#Bind("FAMILY_RELATION")%>' />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator69" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_RELATION%>"
                                        ControlToValidate="ddl_FAMILY_RELATION" ForeColor="Red" ValidationGroup="GroupFam" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddl_FAMILY_RELATION" runat="server" CssClass="MandatoryField">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator611" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_RELATION%>"
                                        ControlToValidate="ddl_FAMILY_RELATION" ForeColor="Red" ValidationGroup="GroupFam" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_BIRTH_DT%>" SortExpression="FAMILY_BIRTH_DT">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_BIRTH_DT" runat="server" Text='<%#Bind("FAMILY_BIRTH_DT")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_FAMILY_BIRTH_DT" runat="server" MaxLength="8" Width="100px" CssClass="MandatoryField date" Text='<%#Bind("FAMILY_BIRTH_DT")%>' ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator612" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_BIRTH_DT%>"
                                        ControlToValidate="txt_FAMILY_BIRTH_DT" ForeColor="Red" ValidationGroup="GroupFam" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="qrytest613" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_FAMILY_BIRTH_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_FAMILY_BIRTH_DT" ValidationGroup="GroupFam" Display="None"></asp:CustomValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_FAMILY_BIRTH_DT" runat="server" MaxLength="8" Width="100px" CssClass="MandatoryField date"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator614" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_BIRTH_DT%>"
                                        ControlToValidate="txt_FAMILY_BIRTH_DT" ForeColor="Red" ValidationGroup="GroupFam" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="qrytest15" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_FAMILY_BIRTH_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_FAMILY_BIRTH_DT" ValidationGroup="GroupFam" Display="None"></asp:CustomValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_FAMILY_WORK_DESC%>" SortExpression="FAMILY_WORK_DESC" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_WORK_DESC" runat="server" Text='<%#Bind("FAMILY_WORK_DESC")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_FAMILY_WORK_DESC" runat="server" MaxLength="60" Width="70px" Text='<%#Bind("FAMILY_WORK_DESC")%>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_FAMILY_WORK_DESC" runat="server" MaxLength="60" Width="70px"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_ALLOWANCE%>" SortExpression="IS_ALLOWANCE">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_IS_ALLOWANCE" runat="server" />
                                    <asp:HiddenField ID="hid_IS_ALLOWANCE" runat="server" Value='<%#Bind("IS_ALLOWANCE")%>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="cb_IS_ALLOWANCE" runat="server" />
                                    <asp:HiddenField ID="hid_IS_ALLOWANCE" runat="server" Value='<%#Bind("IS_ALLOWANCE")%>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="cb_IS_ALLOWANCE" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_BENEFICIARY%>" SortExpression="BENEFICIARY">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_BENEFICIARY" runat="server" />
                                    <asp:HiddenField ID="hid_BENEFICIARY" runat="server" Value='<%#Bind("BENEFICIARY")%>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="cb_BENEFICIARY" runat="server" />
                                    <asp:HiddenField ID="hid_BENEFICIARY" runat="server" Value='<%#Bind("BENEFICIARY")%>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="cb_BENEFICIARY" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_VENDOR_ID%>" SortExpression="VENDOR_ID" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_VENDOR_ID" runat="server" Text='<%#Bind("VENDOR_ID")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_VENDOR_ID" runat="server" MaxLength="10" Width="70px" Text='<%#Bind("VENDOR_ID")%>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_VENDOR_ID" runat="server" MaxLength="10" Width="70px"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_VALID%>" SortExpression="IS_VALID">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_IS_VALID" runat="server" Enabled="false" />
                                    <asp:HiddenField ID="hid_IS_VALID" runat="server" Value='<%#Bind("IS_VALID")%>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="cb_IS_VALID" runat="server" Enabled="true" />
                                    <asp:HiddenField ID="hid_IS_VALID" runat="server" Value='<%#Bind("IS_VALID")%>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="cb_IS_VALID" runat="server" Enabled="true" Checked="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>

                            <table class="grid-view">
                                <tr class="header">
                                    <td></td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2hb_RowNumber%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2hb_FAMILY_NATION_CD%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2hb_FAMILY_SEX_CD%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2hb_FAMILY_LICENSE_ID%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2hb_FAMILY_PASSPORT_ID%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2hb_FAMILY_NAME%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2hb_FAMILY_RELATION%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2hb_FAMILY_BIRTH_DT%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_FAMILY_WORK_DESC%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_IS_ALLOWANCE%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BENEFICIARY%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_VENDOR_ID%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_IS_VALID%>"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="normal">
                                    <td></td>
                                    <td>
                                        <asp:Label ID="lbl_EmptyRowNumber" runat="server" Text="1"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_FAMILY_NATION_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_NATION_CD%>"
                                            ControlToValidate="ddl_FAMILY_NATION_CD" ForeColor="Red" ValidationGroup="GroupFam" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_FAMILY_SEX_CD" runat="server" CssClass="MandatoryField">
                                            <asp:ListItem Text="1-男" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="2-女" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_SEX_CD%>"
                                            ControlToValidate="ddl_FAMILY_SEX_CD" ForeColor="Red" ValidationGroup="GroupFam" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_FAMILY_LICENSE_ID" ClientIDMode="Static" runat="server" MaxLength="10" Width="100px" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_LICENSE_ID%>"
                                            ControlToValidate="txt_FAMILY_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupFam" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ErrorMessage="眷屬身份證字號/居留證號只能輸入英數字"
                                            ControlToValidate="txt_FAMILY_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupFam"
                                            ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_FAMILY_PASSPORT_ID" runat="server" MaxLength="20" Width="100px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ErrorMessage="眷屬護照號碼只能輸入英數字"
                                            ControlToValidate="txt_FAMILY_PASSPORT_ID" ForeColor="Red" ValidationGroup="GroupFam"
                                            ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_FAMILY_NAME" runat="server" MaxLength="30" Width="80px" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_NAME%>"
                                            ControlToValidate="txt_FAMILY_NAME" ForeColor="Red" ValidationGroup="GroupFam" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_FAMILY_RELATION" runat="server" CssClass="MandatoryField">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_RELATION%>"
                                            ControlToValidate="ddl_FAMILY_RELATION" ForeColor="Red" ValidationGroup="GroupFam" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_FAMILY_BIRTH_DT" runat="server" MaxLength="10" Width="100px" CssClass="MandatoryField date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_BIRTH_DT%>"
                                            ControlToValidate="txt_FAMILY_BIRTH_DT" ForeColor="Red" ValidationGroup="GroupFam" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_FAMILY_BIRTH_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_FAMILY_BIRTH_DT" ValidationGroup="GroupFam" Display="None"></asp:CustomValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_FAMILY_WORK_DESC" runat="server" MaxLength="60" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_IS_ALLOWANCE" runat="server" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_BENEFICIARY" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_VENDOR_ID" runat="server" MaxLength="10" Width="70px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_IS_VALID" runat="server" Checked="true" />
                                    </td>
                                </tr>
                            </table>

                        </EmptyDataTemplate>
                        <EditRowStyle CssClass="normal" />
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <FooterStyle CssClass="GridviewScrollPager" />
                    </asp:GridView>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupFam" ShowSummary="false" />
                </div>
                <div id="tabs-7">
                    <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                        <tr>
                            <td class="Body_TD3" style="text-align: right">
                                <asp:Button ID="btn_edu_add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="btn_edu_add_Click" />
                                <asp:Button ID="btn_edu_delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClick="btn_edu_delete_Click" />
                                <asp:Button ID="btn_edu_mod" runat="server" Text="<%$Resources:Resource,btn_edit%>" OnClick="btn_edu_mod_Click" />
                                <asp:Button ID="btn_edu_confirm" runat="server" Text="<%$Resources:Resource,btn_confirm%>" Visible="false" OnClick="btn_edu_confirm_Click" OnClientClick="return saveEduCheck();" />
                                <asp:Button ID="btn_edu_cancel" runat="server" Text="<%$Resources:Resource,btn_cancel%>" Visible="false" OnClick="btn_edu_cancel_Click" OnClientClick="return confirm($('#hidwfb299_Cancel_ConfirmMessage').val())" />
                            </td>
                        </tr>
                    </table>

                    <asp:GridView ID="gv_result2" runat="server" AllowSorting="true" ClientIDMode="Static"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result2_Sorting"
                        OnRowDataBound="gv_result2_RowDataBound" OnRowCreated="gv_result2_RowCreated" Width="1020px">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </FooterTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_SCHOOL_NATION_CD%>" SortExpression="SCHOOL_NATION_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_SCHOOL_NATION_DESC" runat="server" Text='<%#Bind("SCHOOL_NATION_DESC")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddl_SCHOOL_NATION_CD" runat="server">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hid_SCHOOL_NATION_CD" runat="server" Value='<%#Bind("SCHOOL_NATION_CD")%>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddl_SCHOOL_NATION_CD" runat="server">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hid_SCHOOL_NATION_CD" runat="server" Value='<%#Bind("SCHOOL_NATION_CD")%>' />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_EDUCATION_CD%>" SortExpression="EDUCATION_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_EDUCATION_CD" runat="server" Text='<%#Bind("EDUCATION_DESC")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_EDUCATION_CD" runat="server" Text='<%#Bind("EDUCATION_DESC")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddl_EDUCATION_CD" runat="server" CssClass="MandatoryField">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EDUCATION_CD%>"
                                        ControlToValidate="ddl_EDUCATION_CD" ForeColor="Red" ValidationGroup="GroupEdu" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_SCHOOL_NAME%>" SortExpression="SCHOOL_NAME" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_SCHOOL_NAME" runat="server" Text='<%#Bind("SCHOOL_NAME")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_SCHOOL_NAME" runat="server" MaxLength="60" Width="140px" Text='<%#Bind("SCHOOL_NAME")%>' CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_SCHOOL_NAME%>"
                                        ControlToValidate="txt_SCHOOL_NAME" ForeColor="Red" ValidationGroup="GroupEdu" Display="None"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_SCHOOL_NAME" runat="server" MaxLength="60" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_SCHOOL_NAME%>"
                                        ControlToValidate="txt_SCHOOL_NAME" ForeColor="Red" ValidationGroup="GroupEdu" Display="None"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_DEPARTMENT_NAME%>" SortExpression="DEPARTMENT_NAME" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_DEPARTMENT_NAME" runat="server" Text='<%#Bind("DEPARTMENT_NAME")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_DEPARTMENT_NAME" runat="server" MaxLength="60" Width="140px" Text='<%#Bind("DEPARTMENT_NAME")%>'></asp:TextBox>

                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_DEPARTMENT_NAME" runat="server" MaxLength="60" Width="140px"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_GRADUATION_YEAR%>" SortExpression="GRADUATION_YEAR" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_GRADUATION_YEAR" runat="server" Text='<%#Bind("GRADUATION_YEAR")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_GRADUATION_YEAR" runat="server" MaxLength="4" Width="140px" Text='<%#Bind("GRADUATION_YEAR")%>' CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_GRADUATION_YEAR%>"
                                        ControlToValidate="txt_GRADUATION_YEAR" ForeColor="Red" ValidationGroup="GroupEdu" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator31" runat="server" ErrorMessage="畢業年度只能輸入數字，需為4碼"
                                        ControlToValidate="txt_GRADUATION_YEAR" ForeColor="Red" ValidationGroup="GroupEdu"
                                        ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_GRADUATION_YEAR" runat="server" MaxLength="4" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_GRADUATION_YEAR%>"
                                        ControlToValidate="txt_GRADUATION_YEAR" ForeColor="Red" ValidationGroup="GroupEdu" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator32" runat="server" ErrorMessage="畢業年度只能輸入數字，需為4碼"
                                        ControlToValidate="txt_GRADUATION_YEAR" ForeColor="Red" ValidationGroup="GroupEdu"
                                        ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_SALARY_SCHOOL%>" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_IS_SALARY_SCHOOL" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hid_IS_SALARY_SCHOOL" runat="server" Value='<%#Bind("IS_SALARY_SCHOOL")%>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="cb_IS_SALARY_SCHOOL" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hid_IS_SALARY_SCHOOL" runat="server" Value='<%#Bind("IS_SALARY_SCHOOL")%>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="cb_IS_SALARY_SCHOOL" runat="server" ClientIDMode="Static" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_VIRTUAL_SCHOOL%>" HeaderStyle-Width="60px">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_IS_VIRTUAL_SCHOOL" runat="server" />
                                    <asp:HiddenField ID="hid_IS_VIRTUAL_SCHOOL" runat="server" Value='<%#Bind("IS_VIRTUAL_SCHOOL")%>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="cb_IS_VIRTUAL_SCHOOL" runat="server" />
                                    <asp:HiddenField ID="hid_IS_VIRTUAL_SCHOOL" runat="server" Value='<%#Bind("IS_VIRTUAL_SCHOOL")%>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="cb_IS_VIRTUAL_SCHOOL" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>

                            <table class="grid-view">
                                <tr class="header">
                                    <td></td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2hb_RowNumber%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2hb_SCHOOL_NATION_CD%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2hb_EDUCATION_CD%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2hb_SCHOOL_NAME%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DEPARTMENT_NAME%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_GRADUATION_YEAR%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_IS_SALARY_SCHOOL%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_IS_VIRTUAL_SCHOOL%>"></asp:Label>
                                    </td>

                                </tr>
                                <tr class="normal">
                                    <td></td>
                                    <td>
                                        <asp:Label ID="lbl_EmptyRowNumber" runat="server" Text="1"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_SCHOOL_NATION_CD" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_EDUCATION_CD" runat="server" CssClass="MandatoryField">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EDUCATION_CD%>"
                                            ControlToValidate="ddl_EDUCATION_CD" ForeColor="Red" ValidationGroup="GroupEdu" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_SCHOOL_NAME" runat="server" MaxLength="60" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_SCHOOL_NAME%>"
                                            ControlToValidate="txt_SCHOOL_NAME" ForeColor="Red" ValidationGroup="GroupEdu" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_DEPARTMENT_NAME" runat="server" MaxLength="60" Width="140px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_GRADUATION_YEAR" runat="server" MaxLength="4" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_GRADUATION_YEAR%>"
                                            ControlToValidate="txt_GRADUATION_YEAR" ForeColor="Red" ValidationGroup="GroupEdu" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator33" runat="server" ErrorMessage="畢業年度只能輸入數字，需為4碼"
                                            ControlToValidate="txt_GRADUATION_YEAR" ForeColor="Red" ValidationGroup="GroupEdu"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_IS_SALARY_SCHOOL" runat="server" ClientIDMode="Static" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="cb_IS_VIRTUAL_SCHOOL" runat="server" />
                                    </td>

                                </tr>
                            </table>

                        </EmptyDataTemplate>
                        <EditRowStyle CssClass="normal" />
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <FooterStyle CssClass="GridviewScrollPager" />
                    </asp:GridView>
                    <asp:HiddenField ID="hid_IS_SALARY" runat="server" ClientIDMode="Static" />
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupEdu" ShowSummary="false" />
                </div>
                <div id="tabs-8">
                    <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                        <tr>
                            <td class="Body_TD3" style="text-align: right">
                                <asp:Button ID="btn_exp_add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="btn_exp_add_Click" />
                                <asp:Button ID="btn_exp_delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClick="btn_exp_delete_Click" />
                                <asp:Button ID="btn_exp_mod" runat="server" Text="<%$Resources:Resource,btn_edit%>" OnClick="btn_exp_mod_Click" />
                                <asp:Button ID="btn_exp_confirm" runat="server" Text="<%$Resources:Resource,btn_confirm%>" Visible="false" OnClick="btn_exp_confirm_Click" OnClientClick="return saveExpCheck();" />
                                <asp:Button ID="btn_exp_cancel" runat="server" Text="<%$Resources:Resource,btn_cancel%>" Visible="false" OnClick="btn_exp_cancel_Click" OnClientClick="return confirm($('#hidwfb299_Cancel_ConfirmMessage').val())" />
                            </td>
                        </tr>
                    </table>

                    <asp:GridView ID="gv_result3" runat="server" AllowSorting="true" ClientIDMode="Static"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result3_Sorting"
                        OnRowDataBound="gv_result3_RowDataBound" Width="1020px">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="30px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" HeaderStyle-Width="40px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                                </FooterTemplate>

                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_EXP_COMPANY_NAME%>" SortExpression="EXP_COMPANY_NAME" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_EXP_COMPANY_NAME" runat="server" Text='<%#Bind("EXP_COMPANY_NAME")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_EXP_COMPANY_NAME" runat="server" Text='<%#Bind("EXP_COMPANY_NAME")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_EXP_COMPANY_NAME" runat="server" MaxLength="20" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator811" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EXP_COMPANY_NAME%>"
                                        ControlToValidate="txt_EXP_COMPANY_NAME" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_EXP_TITLE_DESC%>" SortExpression="EXP_TITLE_DESC" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_EXP_TITLE_DESC" runat="server" Text='<%#Bind("EXP_TITLE_DESC")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_EXP_TITLE_DESC" runat="server" MaxLength="20" Width="140px" Text='<%#Bind("EXP_TITLE_DESC")%>' CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator812" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EXP_TITLE_DESC%>"
                                        ControlToValidate="txt_EXP_TITLE_DESC" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_EXP_TITLE_DESC" runat="server" MaxLength="20" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator813" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EXP_TITLE_DESC%>"
                                        ControlToValidate="txt_EXP_TITLE_DESC" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_START_YEAR%>" SortExpression="START_YEAR" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_START_YEAR" runat="server" CssClass="ym" Text='<%#Bind("START_YEAR")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_START_YEAR" runat="server" MaxLength="6" Width="140px" Text='<%#Bind("START_YEAR")%>' CssClass="MandatoryField ym"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator814" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_START_YEAR%>"
                                        ControlToValidate="txt_START_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator815" runat="server" ErrorMessage="開始年月輸入錯誤"
                                        ControlToValidate="txt_START_YEAR" ForeColor="Red" ValidationGroup="GroupExp"
                                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_START_YEAR" runat="server" MaxLength="6" Width="140px" CssClass="MandatoryField ym"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidato8162" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_START_YEAR%>"
                                        ControlToValidate="txt_START_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator817" runat="server" ErrorMessage="開始年月輸入錯誤"
                                        ControlToValidate="txt_START_YEAR" ForeColor="Red" ValidationGroup="GroupExp"
                                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_END_YEAR%>" SortExpression="END_YEAR" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_END_YEAR" runat="server" CssClass="ym" Text='<%#Bind("END_YEAR")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_END_YEAR" runat="server" MaxLength="6" Width="140px" Text='<%#Bind("END_YEAR")%>' CssClass="MandatoryField ym"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator818" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_END_YEAR%>"
                                        ControlToValidate="txt_END_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator819" runat="server" ErrorMessage="結束年月輸入錯誤"
                                        ControlToValidate="txt_END_YEAR" ForeColor="Red" ValidationGroup="GroupExp"
                                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_END_YEAR" runat="server" MaxLength="6" Width="140px" CssClass="MandatoryField ym"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator820" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_END_YEAR%>"
                                        ControlToValidate="txt_END_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator821" runat="server" ErrorMessage="結束年月輸入錯誤"
                                        ControlToValidate="txt_END_YEAR" ForeColor="Red" ValidationGroup="GroupExp"
                                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_APPROVE_WORK_YEARS%>" SortExpression="APPROVE_WORK_YEARS" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_APPROVE_WORK_YEARS" runat="server" Text='<%#Bind("APPROVE_WORK_YEARS")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_APPROVE_WORK_YEARS" runat="server" MaxLength="4" Width="140px" Text='<%#Bind("APPROVE_WORK_YEARS")%>' CssClass="MandatoryField num31"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator822" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_APPROVE_WORK_YEARS%>"
                                        ControlToValidate="txt_APPROVE_WORK_YEARS" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator823" runat="server" ErrorMessage="認定年資只能輸入數字(99.9)"
                                        ControlToValidate="txt_APPROVE_WORK_YEARS" ForeColor="Red" ValidationGroup="GroupExp"
                                        ValidationExpression="^(\d{1,2}\.\d{1}|\d{0,2})$" Display="None"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_APPROVE_WORK_YEARS" runat="server" MaxLength="4" Width="140px" CssClass="MandatoryField num31"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator824" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_APPROVE_WORK_YEARS%>"
                                        ControlToValidate="txt_APPROVE_WORK_YEARS" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator825" runat="server" ErrorMessage="認定年資只能輸入數字(99.9)"
                                        ControlToValidate="txt_APPROVE_WORK_YEARS" ForeColor="Red" ValidationGroup="GroupExp"
                                        ValidationExpression="^(\d{1,2}\.\d{1}|\d{0,2})$" Display="None"></asp:RegularExpressionValidator>
                                </FooterTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataTemplate>

                            <table class="grid-view">
                                <tr class="header">
                                    <td></td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2hb_RowNumber%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2hb_EXP_COMPANY_NAME%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2hb_EXP_TITLE_DESC%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2hb_START_YEAR%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_END_YEAR%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_APPROVE_WORK_YEARS%>"></asp:Label>
                                    </td>

                                </tr>
                                <tr class="normal">
                                    <td></td>
                                    <td>
                                        <asp:Label ID="lb_EmptyRowNumber" runat="server" Text="1"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_EXP_COMPANY_NAME" runat="server" MaxLength="20" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EXP_COMPANY_NAME%>"
                                            ControlToValidate="txt_EXP_COMPANY_NAME" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_EXP_TITLE_DESC" runat="server" MaxLength="20" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EXP_TITLE_DESC%>"
                                            ControlToValidate="txt_EXP_TITLE_DESC" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_START_YEAR" runat="server" MaxLength="6" Width="140px" CssClass="MandatoryField ym"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_START_YEAR%>"
                                            ControlToValidate="txt_START_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="開始年月輸入錯誤"
                                            ControlToValidate="txt_START_YEAR" ForeColor="Red" ValidationGroup="GroupExp"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_END_YEAR" runat="server" MaxLength="6" Width="140px" CssClass="MandatoryField ym"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_END_YEAR%>"
                                            ControlToValidate="txt_END_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ErrorMessage="結束年月輸入錯誤"
                                            ControlToValidate="txt_END_YEAR" ForeColor="Red" ValidationGroup="GroupExp"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_APPROVE_WORK_YEARS" runat="server" MaxLength="4" Width="140px" CssClass="MandatoryField num31"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_APPROVE_WORK_YEARS%>"
                                            ControlToValidate="txt_APPROVE_WORK_YEARS" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="認定年資只能輸入數字(99.9)"
                                            ControlToValidate="txt_APPROVE_WORK_YEARS" ForeColor="Red" ValidationGroup="GroupExp"
                                            ValidationExpression="^(\d{1,2}\.\d{1}|\d{0,2})$" Display="None"></asp:RegularExpressionValidator>
                                    </td>

                                </tr>
                            </table>

                        </EmptyDataTemplate>
                        <EditRowStyle CssClass="normal" />
                        <PagerStyle CssClass="GridviewScrollPager" />
                        <FooterStyle CssClass="GridviewScrollPager" />
                    </asp:GridView>
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupExp" ShowSummary="false" />
                </div>
                <div id="tabs-9">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                        <colgroup>
                            <col width="15%" />
                            <col width="35%" />
                            <col width="15%" />
                            <col width="35%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DUR_START_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_START_DT" runat="server" MaxLength="8" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator91" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="赴任起日輸入錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_END_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DUR_END_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_END_DT" runat="server" MaxLength="8" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator92" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="赴任迄日輸入錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RENT_SUBSIDY" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RENT_SUBSIDY%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_RENT_SUBSIDY" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary4" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_photo_upload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
