<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0100_Mod.aspx.cs" Inherits="WebContent_WFB2HB_WFB2HB0100_Mod" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $.unblockUI();
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

            $('#txt_SALARY_ACCOUNT_BANK_NAME').attr("readonly", true);
            $('#txt_DIRECT_HEAD_EMP_NAME').attr("readonly", true);

            $("#ddl_JPN_CD").change(function () {
                checkJPN_CD();
            });
            checkJPN_CD();

            $("#txt_LICENSE_ID").change(function () {
                $("#txt_LICENSE_ID").val($("#txt_LICENSE_ID").val().toUpperCase());
            });

            $("#txt_FAMILY_LICENSE_ID").change(function () {
                $("#txt_FAMILY_LICENSE_ID").val($("#txt_FAMILY_LICENSE_ID").val().toUpperCase());
            });

            $("#ddl_URGENT_CONTACT_RELATION").change(function () {
                if ($("#ddl_URGENT_CONTACT_RELATION option:selected").val() != "-1") {
                    $("#txt_URGENT_CONTACT_RELATION").val($("#ddl_URGENT_CONTACT_RELATION option:selected").text().split("-")[1]);
                }
                else {
                    //$("#txt_URGENT_CONTACT_RELATION").val("");
                }
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
        function ChangeTab(tab) {
            $("#tabs").tabs({ active: tab });
        }

        //兼任的資料
        function openHRItem() {
            window.showModalDialog("WFB2HB0100_OtherPjob.aspx?emp_id=" + $("#txt_EMP_ID").val(), self, 'dialogWidth=600px;dialogHeight=400px;scroll=no');
        }

        function getRegion() {
            var zip_cd = $("#txt_REGISTER_ZIP_CD").val();
            OpenSearch('Region_Search.aspx', 'txt_REGISTER_ZIP_CD', 'txt_REGISTER_REGION', 'ZIP_CD=' + zip_cd, 'Y');
            /*
            var json = OpenSearch('Region_Search.aspx', 'txt_REGISTER_ZIP_CD', 'txt_REGISTER_REGION', 'ZIP_CD=' + zip_cd);
            if (json != undefined) {
                $("#txt_REGISTER_COUNTY").val(json.Val1);
                $("#txt_REGISTER_ADDR").val(json.Val1 + json.DESC);
            }
            */
        }

        function getRegion2() {
            var zip_cd = $("#txt_CONTACT_ZIP_CD").val();
            OpenSearch('Region_Search.aspx', 'txt_CONTACT_ZIP_CD', 'txt_CONTACT_REGION', 'ZIP_CD=' + zip_cd, 'Y');
            /*
            var json = OpenSearch('Region_Search.aspx', 'txt_CONTACT_ZIP_CD', 'txt_CONTACT_REGION', 'ZIP_CD=' + zip_cd);
            if (json != undefined) {
                $("#txt_CONTACT_COUNTY").val(json.Val1);
                $("#txt_CONTACT_ADDR").val(json.Val1 + json.DESC);
            }
            */
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
        //家庭儲存前檢查
        function saveFamCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupFam")) {
                if ($("#ddl_FAMILY_NATION_CD").val() == "TWN" && $("#txt_FAMILY_LICENSE_ID").val() != undefined) {
                    if (checkLicenseID($("#txt_FAMILY_LICENSE_ID").val()))
                        BlockUI();
                    else {
                        alert("眷屬身份證號檢查錯誤");
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

        //教育儲存前檢查
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
                if ($("#ddl_NATION_CD").val() == "TWN") {
                    if ($("#txt_LICENSE_ID").val() != "") {
                        if (!checkLicenseID($("#txt_LICENSE_ID").val())) {
                            msg += "身份證號不符合編碼原則\r\n";
                            processed = false;
                        }
                    }
                }

                if ($("input[id='cb_IS_SALARY_SCHOOL']:checked").length != 1) {
                    msg += "必須有一筆資料勾選為敘薪學歷\r\n";
                    processed = false;
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
                    var mail =$("#txt_COMPANY_EMAIL").val().trim();
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
                    $.trim($("#txt_SALARY_ACCOUNT_NO3").val()) == "")
                {
                } else if (
                    $.trim($("#txt_SALARY_ACCOUNT_NO1").val()) != "" &&
                    $.trim($("#txt_SALARY_ACCOUNT_NO2").val()) != "" &&
                    $.trim($("#txt_SALARY_ACCOUNT_NO3").val()) != "")
                {
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
            }
            else
                BlockUI();

            return processed;
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
            /*
            if ($("#txt_SALARY_ACCOUNT_NO1").val().trim() != "") {
                if (!re.test($("#txt_SALARY_ACCOUNT_NO1").val()) || !re.test($("#txt_SALARY_ACCOUNT_NO2").val()) || !re.test($("#txt_SALARY_ACCOUNT_NO3").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
            */
        }
        function cancelCheck() {
            var msg = $('#hidwfb299_Cancel_ConfirmMessage').val() + "\r\n 若有修改資料，請點選儲存!";
            return confirm(msg);
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
                                <col width="9%" />
                                <col width="22%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="80px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
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
                                        <asp:DropDownList ID="ddl_NATION_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:DropDownList ID="ddl_JPN_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="MainValidator14" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_NATION_CD%>"
                                            ControlToValidate="ddl_NATION_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <%-- 生日 --%>
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
                                            ErrorMessage="體重只能輸入小於999.9" ClientValidationFunction="CheckWEIGHT" ForeColor="Red"
                                            ControlToValidate="txt_WEIGHT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LICENSE_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LICENSE_ID2%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LICENSE_ID" runat="server" MaxLength="10" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="MainValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_LICENSE_ID%>"
                                            ControlToValidate="txt_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="身份證字號/居留證號只能輸入英數字"
                                            ControlToValidate="txt_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                        <asp:HiddenField ID="hid_LICENSE_ID" runat="server" />

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
                                    <%--薪資銀行及分行 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ACCOUNT_BANK" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_ACCOUNT_BANK%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_ACCOUNT_BRANCH" runat="server" MaxLength="4" ClientIDMode="Static" Width="40px"></asp:TextBox>
                                    </td>
                                    <%--薪資匯款帳號 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ACCOUNT_NO" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SALARY_ACCOUNT_NO%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_ACCOUNT_NO3" runat="server" MaxLength="14" ClientIDMode="Static" Width="120px"></asp:TextBox>
                                        <%-- 
                                        <asp:TextBox ID="txt_SALARY_ACCOUNT_NO1" runat="server" MaxLength="3" ClientIDMode="Static" Width="35px"></asp:TextBox>-
                                        <asp:TextBox ID="txt_SALARY_ACCOUNT_NO2" runat="server" MaxLength="2" ClientIDMode="Static" Width="22px"></asp:TextBox>-

                                        <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="薪資匯款帳號只能輸入數字" ClientValidationFunction="CheckSALARY_ACCOUNT" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_ACCOUNT_NO1" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                              <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hb_format_SALARY_ACCOUNT_NO1%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_ACCOUNT_NO1" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2hb_format_SALARY_ACCOUNT_NO2%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_ACCOUNT_NO2" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>                                    
                                        --%>
                                          <asp:CustomValidator ID="CustomValidator5" runat="server" ValidateEmptyText="true"
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
                        <aces:Btn ID="WFB2HB0100Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClick="WFB2HB0100Save_Click" OnClientClick="return saveCheck();" />

                        <%--<asp:Button ID="WFB2HB0100Save" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClick="WFB2HB0100Save_Click" OnClientClick="return saveCheck();" />--%>
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
                                    <asp:TextBox ID="txt_JOIN_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_EXAM_EXPIRE_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EXAM_EXPIRE_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_EXAM_EXPIRE_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
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
                                    <asp:TextBox ID="txt_COMPANY_CD" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PLANT_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PLANT_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PLANT_CD" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_IS_UPD_HEAD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_IS_UPD_HEAD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_IS_UPD_HEAD" runat="server" CssClass="MandatoryField">
                                        <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>


                                   <th class="Body_TH3">
                                    <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DEPT_NO%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="3">
                                    <asp:TextBox ID="txt_DEPT_NO" runat="server" BorderWidth="0" ReadOnly="true" onkeydown="return false" Width="400px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_DIRECT_HEAD_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DIRECT_HEAD_EMP_ID%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_DIRECT_HEAD_EMP_ID" runat="server" MaxLength="5" Width="64px" CssClass="MandatoryField textWidth " ClientIDMode="Static"></asp:TextBox>
                                    <input id="bt_HEAD_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_DIRECT_HEAD_EMP_ID', 'txt_DIRECT_HEAD_EMP_NAME', 'N');" />
                                    <asp:TextBox ID="txt_DIRECT_HEAD_EMP_NAME" runat="server" MaxLength="10" Width="100px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="MainValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_DIRECT_HEAD_EMP_ID%>"
                                        ControlToValidate="txt_DIRECT_HEAD_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                             
                               <%-- 職種 --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_WS_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WS_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_WS_CD" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                               <%-- 特休生成日 --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_DL_GEN_DT" runat="server" Text="特休生成日"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_DL_GEN_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>


                                <%-- 加班管制對象 --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="Label14" runat="server" Text="加班管制/體檢年度/開始日期"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:DropDownList ID="ddl_OVERTIME_CTL_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="MainValidator470" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hb_Required_OVERTIME_CTL_CD%>"
                                        ControlToValidate="ddl_OVERTIME_CTL_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>

                                    <asp:TextBox ID="txt_HEALTH_YEAR" runat="server" MaxLength="4" Width="50px" ClientIDMode="Static" CssClass="textWidth"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator31" runat="server" ErrorMessage="體檢年度只能輸入數字，需為4碼"
                                        ControlToValidate="txt_HEALTH_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression="^\d{4}$" Display="None"></asp:RegularExpressionValidator>

                                     <asp:TextBox ID="txt_OVERTIME_CTL_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="textWidth date"></asp:TextBox>
                                    <%-- 
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="加班管制日期必須輸入"
                                            ControlToValidate="txt_OVERTIME_CTL_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    --%>
                                     <asp:CustomValidator ID="CustomValidator6" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="加班管制日期錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_OVERTIME_CTL_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                 <%-- 員工區分 --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_EMP_CD" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>

                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PLAN_DESPATCH_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PLAN_DESPATCH_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PLAN_DESPATCH_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>

                                <%-- 刷卡管制對象 --%>
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
                                <%-- 資格級數 --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_LEVEL_GRADE" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LEVEL_GRADE%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_LEVEL_CD" runat="server" MaxLength="4" Width="40px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>

                                    <asp:TextBox ID="txt_GRADE_CD" runat="server" MaxLength="4" Width="40px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_BE_DESPATCH_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BE_DESPATCH_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_BE_DESPATCH_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>

                                <th class="Body_TH3">
                                    <asp:Label ID="lb_MODEL_YEAR" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_MODEL_YEAR%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_MODEL_YEAR" runat="server" MaxLength="4" Width="50px" ClientIDMode="Static" CssClass="textWidth"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator14" runat="server" ErrorMessage="模範員工年度只能輸入數字，需為4碼"
                                        ControlToValidate="txt_MODEL_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression="^\d{4}$" Display="None"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                  <%-- 職務 --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PJOB_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PJOB_CD" runat="server" MaxLength="10" Width="100px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>

                                    <input type="button" name="button1" value="兼任" onclick="openHRItem()">
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_KEEP_DESPATCH_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_KEEP_DESPATCH_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_KEEP_DESPATCH_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>

                                <th class="Body_TH3">
                                    <asp:Label ID="lb_HONOR_YEAR" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_HONOR_YEAR%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_HONOR_YEAR" runat="server" MaxLength="8" Width="50px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" onkeydown="return false" AutoCompleteType="Disabled" ></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                              <th class="Body_TH3">
                                    <asp:Label ID="lb_GRAGE" runat="server" Text="年級"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_GRAGE" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_BE_CONTRACT_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BE_CONTRACT_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_BE_CONTRACT_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>

                                <%-- 工會職務 --%>
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
                                    <asp:TextBox ID="txt_WORK_SHIFT_CD" runat="server" MaxLength="10" Width="198px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>

                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_BE_EMP_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BE_EMP_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_BE_EMP_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_WORK_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WORK_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_WORK_CD" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_CALENDAR_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_CALENDAR_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_CALENDAR_CD" runat="server" MaxLength="8" Width="122px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <td class="Body_TD3"></td>
                                <td class="Body_TD3"></td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_ACC_CD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_ACC_CD%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_ACC_CD" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div id="tabs-2">
                    <br />
                    <table cellspacing="1" cellpadding="1" width="100%" border="0">
                        <colgroup>
                            <col width="12%" />
                            <col width="13%" />
                            <col width="14%" />
                            <col width="11%" />
                            <col width="14%" />
                            <col width="11%" />
                            <col width="12%" />
                            <col width="13%" />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_LEVEL_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_LEVEL_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_LEVEL_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_PJOB_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_PJOB_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_PJOB_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_DEPT_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_DEPT_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_DEPT_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_DIV_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_DIV_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_DIV_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                            </tr>

                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_LEVEL_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_LEVEL_WORK_DAYS2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_LEVEL_WORK_DAYS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_PJOB_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_PJOB_WORK_DAYS%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_PJOB_WORK_DAYS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_DEPT_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_DEPT_WORK_DAYS%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_DEPT_WORK_DAYS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RECENT_DIV_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RECENT_DIV_WORK_DAYS%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RECENT_DIV_WORK_DAYS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_STUDENT_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_STUDENT_WORK_DAYS%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_STUDENT_WORK_DAYS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_K_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_K_WORK_DAYS%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_K_WORK_DAYS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3" style="width: 14%;">
                                    <asp:Label ID="lb_T_WORK_DAYS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_T_WORK_DAYS%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_T_WORK_DAYS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_WORK_YEARS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_WORK_YEARS2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_WORK_YEARS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_SERVICE_YEARS" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SERVICE_YEARS%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="7">
                                    <asp:TextBox ID="txt_SERVICE_YEARS" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_LEAVE_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LEAVE_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_LEAVE_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_LEAVE_REASON_DESC" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_LEAVE_REASON_DESC%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_LEAVE_REASON_DESC" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PLAN_RETENTION_EDT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PLAN_RETENTION_EDT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PLAN_RETENTION_EDT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_RETENTION_EDT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RETENTION_EDT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_RETENTION_EDT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_TRANSFER_SDT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TRANSFER_SDT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_TRANSFER_SDT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_TRANSFER_REASON_DESC" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TRANSFER_REASON_DESC%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_TRANSFER_REASON_DESC" runat="server" MaxLength="8" Width="81px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PLAN_TRANSFER_EDT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PLAN_TRANSFER_EDT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PLAN_TRANSFER_EDT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3" style="width: 14%;">
                                    <asp:Label ID="lb_TRANSFER_EDT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_TRANSFER_EDT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_TRANSFER_EDT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_BACK_SCHOOL_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BACK_SCHOOL_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_BACK_SCHOOL_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_BACK_PLANT_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_BACK_PLANT_DT%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="5">
                                    <asp:TextBox ID="txt_BACK_PLANT_DT" runat="server" MaxLength="8" Width="90px" BorderWidth="0" ClientIDMode="Static" CssClass="textWidth" ReadOnly="true" onkeydown="return false"></asp:TextBox>
                                </td>

                            </tr>
                        </tbody>
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
                                    <asp:TextBox ID="txt_REGISTER_ZIP_CD" runat="server" MaxLength="5" Width="50px" ClientIDMode="Static" CssClass="MandatoryField textWidth" Enabled="false"></asp:TextBox>
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
                                <%-- 地址 --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_REGISTER_ADD" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_REGISTER_ADD%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="3">
                                    <asp:TextBox ID="txt_REGISTER_ADDR" runat="server" MaxLength="255" Width="500px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                </td>
                                <%-- 電話 --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_REGISTER_TEL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_REGISTER_TEL2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_REGISTER_TEL" runat="server"  Width="140px" ClientIDMode="Static" CssClass="MandatoryField textWidth"></asp:TextBox>
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
                                    <asp:TextBox ID="txt_CONTACT_ZIP_CD" runat="server" MaxLength="5" Width="50px" ClientIDMode="Static" CssClass="num3 textWidth" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_CONTACT_COUNTY" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                    <asp:TextBox ID="txt_CONTACT_REGION" runat="server" MaxLength="10" Width="70px" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="通訊郵遞區號只能輸入數字"
                                        ControlToValidate="txt_CONTACT_ZIP_CD" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression="^[0-9]+$" Display="None"></asp:RegularExpressionValidator>
                                </td>
                            </tr>

                            <tr>
                                 <%-- 地址 --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_CONTACT_ADDR" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_CONTACT_ADDR2%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="3">
                                    <asp:TextBox ID="txt_CONTACT_ADDR" runat="server" MaxLength="255" Width="630px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                 <%-- 電話 --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_CONTACT_TEL2" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_CONTACT_TEL2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_CONTACT_TEL" runat="server" CssClass="textWidth" Width="140px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                 <%-- 個人EMAIL --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_PERSONAL_EMAIL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_PERSONAL_EMAIL%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_PERSONAL_EMAIL" runat="server" CssClass="textWidth" Width="280px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <%-- 行動電話一 --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_MOBILE_TEL_1" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_MOBILE_TEL_1%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_MOBILE_TEL_1" runat="server" CssClass="textWidth" Width="140px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <%-- 行動電話二--%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_MOBILE_TEL_2" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_MOBILE_TEL_2%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_MOBILE_TEL_2" runat="server" CssClass="textWidth" Width="140px" ClientIDMode="Static "></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_SALARY" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SALARY%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="5">
                                    <asp:RadioButton ID="rb_SALARY" runat="server" GroupName="SALARY" ClientIDMode="Static" />
                                    <%--<asp:CheckBox ID="cb_SALARY" runat="server" />--%>
                                </td>
                            </tr>

                            <tr>
                                <td align="left" class="Body_label" colspan="6">【公司】
                                </td>
                            </tr>
                            <tr>
                                 <%-- 公司EMAIL --%>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_COMPANY_EMAIL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_COMPANY_EMAIL%>"></asp:Label>:</th>
                                <td class="Body_TD3" colspan="5">
                                    <asp:TextBox ID="txt_COMPANY_EMAIL" runat="server" CssClass="textWidth" Width="280px" ClientIDMode="Static"></asp:TextBox>
                                </td>
                                <%--                            <th class="Body_TH3" >
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
                                    <%--<asp:CheckBox ID="cb_SALARY_2" runat="server" />--%>
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
                                    <asp:TextBox ID="txt_URGENT_CONTACT_NAME" runat="server" MaxLength="30" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="MainValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_URGENT_CONTACT_NAME%>"
                                        ControlToValidate="txt_URGENT_CONTACT_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <th class="Body_TH3">
                                    <asp:Label ID="lb_URGENT_CONTACT_TEL" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_URGENT_CONTACT_TEL%>"></asp:Label>:</th>
                                <td class="Body_TD3">
                                    <asp:TextBox ID="txt_URGENT_CONTACT_TEL" runat="server" MaxLength="14" Width="120px" ClientIDMode="Static" CssClass="MandatoryField textWidth"></asp:TextBox>
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
                    <%-- 家庭成員 --%>
                    <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                        <tr>
                            <td class="Body_TD3" style="text-align: right">
                                <asp:Button ID="btn_family_add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="btn_family_add_Click" />
                                <asp:Button ID="btn_family_delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClick="btn_family_delete_Click" OnClientClick="return confirm('確定要刪除?');" />
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
                            <%-- 國家別 --%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_NATION_CD%>" SortExpression="FAMILY_NATION_CD" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_NATION_CD" runat="server" Text='<%#Bind("FAMILY_NATION_DESC")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lb_FAMILY_NATION_CD" runat="server" Text='<%#Bind("FAMILY_NATION_DESC")%>'></asp:Label>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList ID="ddl_FAMILY_NATION_CD" ClientIDMode="Static" runat="server" CssClass="MandatoryField">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator61" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_NATION_CD%>"
                                        ControlToValidate="ddl_FAMILY_NATION_CD" ForeColor="Red" ValidationGroup="GroupFam" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                              <%-- 性別 --%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_SEX_CD%>" SortExpression="FAMILY_SEX_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
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
                            <%-- 身份證字號 --%>
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
                            <%-- 護照號碼 --%>
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
                            <%-- 姓名 --%>
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
                            <%-- 稱謂 --%>
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
                            <%-- 出生年月日--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_FAMILY_BIRTH_DT%>" SortExpression="FAMILY_BIRTH_DT">
                                <ItemTemplate>
                                    <asp:Label ID="lb_FAMILY_BIRTH_DT" runat="server" Text='<%#Bind("FAMILY_BIRTH_DT")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_FAMILY_BIRTH_DT" runat="server" MaxLength="10" Width="100px" CssClass="MandatoryField date" Text='<%#Bind("FAMILY_BIRTH_DT")%>' ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator612" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_BIRTH_DT%>"
                                        ControlToValidate="txt_FAMILY_BIRTH_DT" ForeColor="Red" ValidationGroup="GroupFam" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="qrytest613" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_FAMILY_BIRTH_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_FAMILY_BIRTH_DT" ValidationGroup="GroupFam" Display="None"></asp:CustomValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_FAMILY_BIRTH_DT" runat="server" MaxLength="10" Width="100px" CssClass="MandatoryField date"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator614" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_BIRTH_DT%>"
                                        ControlToValidate="txt_FAMILY_BIRTH_DT" ForeColor="Red" ValidationGroup="GroupFam" Display="None"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="qrytest615" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_FAMILY_BIRTH_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_FAMILY_BIRTH_DT" ValidationGroup="GroupFam" Display="None"></asp:CustomValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- 服務機構--%>
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
                            <%-- 津貼--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_ALLOWANCE%>" SortExpression="IS_ALLOWANCE">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_IS_ALLOWANCE" runat="server" Enabled="false" />
                                    <asp:HiddenField ID="hid_IS_ALLOWANCE" runat="server" Value='<%#Bind("IS_ALLOWANCE")%>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="cb_IS_ALLOWANCE" runat="server" Enabled="true" />
                                    <asp:HiddenField ID="hid_IS_ALLOWANCE" runat="server" Value='<%#Bind("IS_ALLOWANCE")%>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="cb_IS_ALLOWANCE" runat="server" Enabled="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- 受益人--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_BENEFICIARY%>" SortExpression="BENEFICIARY">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cb_BENEFICIARY" runat="server" Enabled="false" />
                                    <asp:HiddenField ID="hid_BENEFICIARY" runat="server" Value='<%#Bind("BENEFICIARY")%>' />
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:CheckBox ID="cb_BENEFICIARY" runat="server" Enabled="true" />
                                    <asp:HiddenField ID="hid_BENEFICIARY" runat="server" Value='<%#Bind("BENEFICIARY")%>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:CheckBox ID="cb_BENEFICIARY" runat="server" Enabled="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- 廠商code--%>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_VENDOR_ID%>" SortExpression="VENDOR_ID" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_VENDOR_ID" runat="server" Text='<%#Bind("VENDOR_ID")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_VENDOR_ID" runat="server" MaxLength="60" Width="70px" Text='<%#Bind("VENDOR_ID")%>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_VENDOR_ID" runat="server" MaxLength="60" Width="70px"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <%-- 有效--%>
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
                                        <asp:DropDownList ID="ddl_FAMILY_NATION_CD" ClientIDMode="Static" runat="server" CssClass="MandatoryField">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator616" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_NATION_CD%>"
                                            ControlToValidate="ddl_FAMILY_NATION_CD" ForeColor="Red" ValidationGroup="GroupFam" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_FAMILY_SEX_CD" runat="server" CssClass="MandatoryField">
                                            <asp:ListItem Text="1-男" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="2-女" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator617" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_SEX_CD%>"
                                            ControlToValidate="ddl_FAMILY_SEX_CD" ForeColor="Red" ValidationGroup="GroupFam" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_FAMILY_LICENSE_ID" ClientIDMode="Static" runat="server" MaxLength="10" Width="100px" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator618" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_LICENSE_ID%>"
                                            ControlToValidate="txt_FAMILY_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupFam" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator619" runat="server" ErrorMessage="眷屬身份證字號/居留證號只能輸入英數字"
                                            ControlToValidate="txt_FAMILY_LICENSE_ID" ForeColor="Red" ValidationGroup="GroupFam"
                                            ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_FAMILY_PASSPORT_ID" runat="server" MaxLength="20" Width="100px"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator620" runat="server" ErrorMessage="眷屬護照號碼只能輸入英數字"
                                            ControlToValidate="txt_FAMILY_PASSPORT_ID" ForeColor="Red" ValidationGroup="GroupFam"
                                            ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_FAMILY_NAME" runat="server" MaxLength="30" Width="80px" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidato621" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_NAME%>"
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
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator622" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_FAMILY_BIRTH_DT%>"
                                            ControlToValidate="txt_FAMILY_BIRTH_DT" ForeColor="Red" ValidationGroup="GroupFam" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="qrytest623" runat="server" ValidateEmptyText="true"
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
                                        <asp:TextBox ID="txt_VENDOR_ID" runat="server" Width="70px"></asp:TextBox>
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
                    <%--學歷 --%>
                    <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                        <tr>
                            <td class="Body_TD3" style="text-align: right">
                                <asp:Button ID="btn_edu_add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="btn_edu_add_Click" />
                                <asp:Button ID="btn_edu_delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClick="btn_edu_delete_Click" OnClientClick="return confirm('確定要刪除?');"/>
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
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_SCHOOL_NATION_CD%>" SortExpression="SCHOOL_NATION_CD" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
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
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_EDUCATION_CD%>" SortExpression="EDUCATION_CD" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
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
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_SCHOOL_NAME%>" SortExpression="SCHOOL_NAME" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
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

                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_DEPARTMENT_NAME%>" SortExpression="DEPARTMENT_NAME" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
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
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_GRADUATION_YEAR%>" SortExpression="GRADUATION_YEAR" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_GRADUATION_YEAR" runat="server" Text='<%#Bind("GRADUATION_YEAR")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_GRADUATION_YEAR" runat="server" MaxLength="4" Width="100px" Text='<%#Bind("GRADUATION_YEAR")%>' CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_GRADUATION_YEAR%>"
                                        ControlToValidate="txt_GRADUATION_YEAR" ForeColor="Red" ValidationGroup="GroupEdu" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator31" runat="server" ErrorMessage="畢業年度只能輸入數字，需為4碼"
                                        ControlToValidate="txt_GRADUATION_YEAR" ForeColor="Red" ValidationGroup="GroupEdu"
                                        ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_GRADUATION_YEAR" runat="server" MaxLength="4" Width="100px" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_GRADUATION_YEAR%>"
                                        ControlToValidate="txt_GRADUATION_YEAR" ForeColor="Red" ValidationGroup="GroupEdu" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator32" runat="server" ErrorMessage="畢業年度只能輸入數字，需為4碼"
                                        ControlToValidate="txt_GRADUATION_YEAR" ForeColor="Red" ValidationGroup="GroupEdu"
                                        ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_SALARY_SCHOOL%>" HeaderStyle-Width="100px" ItemStyle-Width="100px">
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
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_lb_IS_VIRTUAL_SCHOOL%>" HeaderStyle-Width="60px" ItemStyle-Width="60px">
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
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupEdu" ShowSummary="false" />
                </div>
                <div id="tabs-8">
                    <%-- 經歷 --%>
                    <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                        <tr>
                            <td class="Body_TD3" style="text-align: right">
                                <asp:Button ID="btn_exp_add" runat="server" Text="<%$Resources:Resource,btn_add%>" OnClick="btn_exp_add_Click" />
                                <asp:Button ID="btn_exp_delete" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClick="btn_exp_delete_Click"  OnClientClick="return confirm('確定要刪除?');"/>
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
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EXP_COMPANY_NAME%>"
                                        ControlToValidate="txt_EXP_COMPANY_NAME" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_EXP_TITLE_DESC%>" SortExpression="EXP_TITLE_DESC" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="lb_EXP_TITLE_DESC" runat="server" Text='<%#Bind("EXP_TITLE_DESC")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_EXP_TITLE_DESC" runat="server" MaxLength="20" Width="140px" Text='<%#Bind("EXP_TITLE_DESC")%>' CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EXP_TITLE_DESC%>"
                                        ControlToValidate="txt_EXP_TITLE_DESC" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_EXP_TITLE_DESC" runat="server" MaxLength="20" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EXP_TITLE_DESC%>"
                                        ControlToValidate="txt_EXP_TITLE_DESC" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2hb_START_YEAR%>" SortExpression="START_YEAR" HeaderStyle-Width="140px" ItemStyle-Width="140px">
                                <ItemTemplate>
                                    <asp:Label ID="lb_START_YEAR" runat="server" CssClass="ym" Text='<%#Bind("START_YEAR")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_START_YEAR" runat="server" MaxLength="6" Width="140px" Text='<%#Bind("START_YEAR")%>' CssClass="MandatoryField ym"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator811" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_START_YEAR%>"
                                        ControlToValidate="txt_START_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator812" runat="server" ErrorMessage="開始年月輸入錯誤"
                                        ControlToValidate="txt_START_YEAR" ForeColor="Red" ValidationGroup="GroupExp"
                                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_START_YEAR" runat="server" MaxLength="6" Width="140px" CssClass="MandatoryField ym"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator813" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_START_YEAR%>"
                                        ControlToValidate="txt_START_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator814" runat="server" ErrorMessage="開始年月輸入錯誤"
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
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator815" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_END_YEAR%>"
                                        ControlToValidate="txt_END_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator816" runat="server" ErrorMessage="結束年月輸入錯誤"
                                        ControlToValidate="txt_END_YEAR" ForeColor="Red" ValidationGroup="GroupExp"
                                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_END_YEAR" runat="server" MaxLength="6" Width="140px" CssClass="MandatoryField ym"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator817" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_END_YEAR%>"
                                        ControlToValidate="txt_END_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator818" runat="server" ErrorMessage="結束年月輸入錯誤"
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
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator817" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_APPROVE_WORK_YEARS%>"
                                        ControlToValidate="txt_APPROVE_WORK_YEARS" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator818" runat="server" ErrorMessage="認定年資只能輸入數字(99.9)"
                                        ControlToValidate="txt_APPROVE_WORK_YEARS" ForeColor="Red" ValidationGroup="GroupExp"
                                        ValidationExpression="^(\d{1,2}\.\d{1}|\d{0,2})$" Display="None"></asp:RegularExpressionValidator>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txt_APPROVE_WORK_YEARS" runat="server" MaxLength="4" Width="140px" CssClass="MandatoryField num31"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator819" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_APPROVE_WORK_YEARS%>"
                                        ControlToValidate="txt_APPROVE_WORK_YEARS" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator820" runat="server" ErrorMessage="認定年資只能輸入數字(99.9)"
                                        ControlToValidate="txt_APPROVE_WORK_YEARS" ForeColor="Red" ValidationGroup="GroupExp"
                                        ValidationExpression="^(\d{1,2}\.\d{1}|\d{0,2})$" Display="None"></asp:RegularExpressionValidator>
                                </FooterTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataTemplate>
                             <%-- 經歷 --%>
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
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator821" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EXP_COMPANY_NAME%>"
                                            ControlToValidate="txt_EXP_COMPANY_NAME" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_EXP_TITLE_DESC" runat="server" MaxLength="20" Width="140px" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator822" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_EXP_TITLE_DESC%>"
                                            ControlToValidate="txt_EXP_TITLE_DESC" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_START_YEAR" runat="server" MaxLength="6" Width="140px" CssClass="MandatoryField ym"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator823" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_START_YEAR%>"
                                            ControlToValidate="txt_START_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator824" runat="server" ErrorMessage="開始年月輸入錯誤"
                                            ControlToValidate="txt_START_YEAR" ForeColor="Red" ValidationGroup="GroupExp"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_END_YEAR" runat="server" MaxLength="6" Width="140px" CssClass="MandatoryField ym"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator825" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_END_YEAR%>"
                                            ControlToValidate="txt_END_YEAR" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator826" runat="server" ErrorMessage="結束年月輸入錯誤"
                                            ControlToValidate="txt_END_YEAR" ForeColor="Red" ValidationGroup="GroupExp"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_APPROVE_WORK_YEARS" runat="server" MaxLength="4" Width="140px" CssClass="MandatoryField num31"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator827" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_required_APPROVE_WORK_YEARS%>"
                                            ControlToValidate="txt_APPROVE_WORK_YEARS" ForeColor="Red" ValidationGroup="GroupExp" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator828" runat="server" ErrorMessage="認定年資只能輸入數字(99.9)"
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
                    <%-- 外籍赴任 --%>
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
                                <tr>
                                    <%-- 赴任起日 --%>
                                    <th class="Body_TH3">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DUR_START_DT%>"></asp:Label>:</th>
                                    <td class="Body_TD3">
                                        <asp:TextBox ID="txt_START_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="textWidth date"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator91" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="赴任起日輸入錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                    <%-- 赴任迄日 --%>
                                    <th class="Body_TH3">
                                        <asp:Label ID="lb_END_DT" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_DUR_END_DT%>"></asp:Label>:</th>
                                    <td class="Body_TD3">
                                        <asp:TextBox ID="txt_END_DT" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="textWidth date"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator92" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="赴任迄日輸入錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%-- 房屋津貼 --%>
                                    <th class="Body_TH3">
                                        <asp:Label ID="lb_RENT_SUBSIDY" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_RENT_SUBSIDY%>"></asp:Label>:</th>
                                    <td class="Body_TD3">
                                        <asp:DropDownList ID="ddl_RENT_SUBSIDY" runat="server" ClientIDMode="Static">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <asp:HiddenField ID="hid_IS_DURATION" runat="server" />
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
