<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2df/action/WFB2DF0200_Mod.aspx.cs" Inherits="WebContent_WFB2DF_WFB2DF0200_Mod" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            initForm();

        });
        function initForm() {
            $.unblockUI();
            $("#txt_START_DT").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_END_DT").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_OTHER_AMOUNT").mask("9999");
            $(".money").blur(function () {
                $(this).parseNumber({ format: "####", locale: "tw" });
                $(this).formatNumber({ format: "####", locale: "tw" });
            });
            $("#txt_EMP_ID").mask("99999");
            $(".date").mask("9999/99/99");
            if ($('#ddl_AMOUNT').val() != "-1") {
                var amout = $('#ddl_AMOUNT').val().split('-');
                $('#lb_AMOUNT').text(amout[1]);
                $('#hid_AMOUNT').val(amout[1]);
            }
            else {
                $('#lb_AMOUNT').text("");
                $('#hid_AMOUNT').val("");
            }

            $('#txt_EMP_ID').change(function () {
                getEMPData();

            });

            $('#ddl_AMOUNT').change(function () {
                if ($('#ddl_AMOUNT').val() != "-1") {
                    var amout = $('#ddl_AMOUNT').val().split('-');
                    $('#lb_AMOUNT').text(amout[1]);
                    $('#hid_AMOUNT').val(amout[1]);
                }
                else {
                    $('#lb_AMOUNT').text("");
                    $('#hid_AMOUNT').val("");
                }
            });
        }

        function getEMPData() {
            if ($('#txt_EMP_ID').val().length == 5) {
                //ajax 取得員工基本資料
                $.ajax({
                    url: "WFB2DF0200_GetEmpData.ashx",
                    data: {
                        EMP_ID: $('#txt_EMP_ID').val()
                    },
                    type: "GET",
                    dataType: 'json',
                    success: function (JData) {
                        if (JData.errMsg != "")
                            alert(JData.errMsg);
                        else {
                            $('#txt_EMP_NAME').val(JData.EMP_NAME);
                            $('#txt_EMP_CD').val(JData.EMP_CD);
                            $('#txt_PJOB_DESC').val(JData.PJOB_DESC);
                            $('#txt_DEPT_NO').val(JData.DEPT_NO);
                            $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            $('#txt_WORK_SHIFT_CD').val(JData.WORK_SHIFT_CD);
                            $('#txt_AGE').val(JData.AGE);
                            $('#txt_JOIN_DT').val(JData.JOIN_DT);
                            $('#txt_REGISTER_ADDR').val(JData.REGISTER_ADDR);
                            $('#txt_CONTACT_ADDR').val(JData.CONTACT_ADDR);
                            $('#txt_MOBILE_TEL_1').val(JData.MOBILE_TEL_1);
                            $('#txt_CONTACT_TEL').val(JData.CONTACT_TEL);
                        }
                    },

                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });
            }
        }
        //有輸入其他費用則必須輸入其他費用說明
        function CheckRemark(source, arguments) {
            if ($("#txt_OTHER_AMOUNT").val() != "" && $("#txt_OTHER_AMOUNT").val() != "0") {
                if ($("#txt_REMARK").val() == "")
                    arguments.IsValid = false;
            }
            else
                arguments.IsValid = true;
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
                //檢查住宿費
                var code_val1 = $("#hid_CODE_VAL1").val();
                if (code_val1 == 'N')
                    if ($("#lb_AMOUNT").text() != "0")
                        processed = confirm("您所選擇的宿舍費為" + $("#lb_AMOUNT").text() + "元，但此宿舍別的住宿費應該為0元，是否要繼續儲存？");
                //檢查不同直別員工
                $.ajax({
                    url: "WFB2DF0200_CheckWorkShift.ashx",
                    data: {
                        EMP_ID: $('#txt_EMP_ID').val(),
                        ROOM_NO: $('#txt_ROOM_NO').val(),
                        WORK_SHIFT_CD: $('#txt_WORK_SHIFT_CD').val()
                    },
                    type: "GET",
                    dataType: 'text',
                    async: false,
                    success: function (result) {
                        if (result == "Y")
                            processed = confirm("此房間有不同輪值別人員，是否確定要儲存？");
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(xhr.status);
                        alert(thrownError);
                    }
                });

            }
            else
                processed = false;

            if (!processed) {
                $.unblockUI();
                return false;
            }
            return processed;
        }

        //function checkConfirm() {
        //    var result = confirm("是否確定取消?");
        //    if (result)
        //        backToQry();
        //}
        //function backToQry() {
        //    window.location.href = "WFB2DF0200_Qry.aspx";
        //}

        function CheckRoomNo(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if ($("#txt_ROOM_NO").val().trim() != "") {
                if (!re.test($("#txt_ROOM_NO").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
        }

        function CheckMotorNo(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            var tt = $("#txt_MOTOR_NO").val();
            if ($("#txt_MOTOR_NO").val() != null) {
                if ($("#txt_MOTOR_NO").val().trim() != "") {
                    if (!re.test($("#txt_MOTOR_NO").val()))
                        arguments.IsValid = false;
                    else
                        arguments.IsValid = true;
                }
                else
                    arguments.IsValid = true;
            }
            
        }
        function CheckCarNo(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if ($("#txt_CAR_NO").val() != null) {
                if ($("#txt_CAR_NO").val().trim() != "") {
                    if (!re.test($("#txt_CAR_NO").val()))
                        arguments.IsValid = false;
                    else
                        arguments.IsValid = true;
                }
                else
                    arguments.IsValid = true;
            }
            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

        <tr height="100%" valign="top">
            <td>


                <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                    <tbody>
                        <!-- START: 1st Line in Search Criteria area -->
                        <tr>
                            <!-- START: Create MODULE ID -->
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_emp_id" runat="server" Text="<%$Resources:Resource,wfb2df_lb_emp_id%>"></asp:Label></th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" class="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N'); getEMPData();" />

                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_EMP_ID%>"
                                    ControlToValidate="txt_emp_id" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2df_lb_EMP_NAME%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2df_lb_EMP_CD%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_EMP_CD" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_PJOB_DESC" runat="server" Text="<%$Resources:Resource,wfb2df_lb_PJOB_DESC%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_PJOB_DESC" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>

                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2df_lb_DEPT_NO%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_DEPT_NO" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>

                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_DEPT_NAME" runat="server" Text="<%$Resources:Resource,wfb2df_lb_DEPT_NAME%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ReadOnly="true" Width="274px" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_WORK_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfb2df_lb_WORK_SHIFT_CD%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_WORK_SHIFT_CD" runat="server" Width="180px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_AGE" runat="server" Text="<%$Resources:Resource,wfb2df_lb_AGE%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_AGE" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_JOIN_DT" runat="server" Text="<%$Resources:Resource,wfb2df_lb_JOIN_DT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_JOIN_DT" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>

                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_REGISTER_ADDR" runat="server" Text="<%$Resources:Resource,wfb2df_lb_REGISTER_ADDR%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="5">
                                <asp:TextBox ID="txt_REGISTER_ADDR" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static" Width="700px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_CONTACT_ADDR" runat="server" Text="<%$Resources:Resource,wfb2df_lb_CONTACT_ADDR%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="5">
                                <asp:TextBox ID="txt_CONTACT_ADDR" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static" Width="700px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_MOBILE_TEL_1" runat="server" Text="<%$Resources:Resource,wfb2df_lb_MOBILE_TEL_1%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_MOBILE_TEL_1" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_CONTACT_TEL" runat="server" Text="<%$Resources:Resource,wfb2df_lb_CONTACT_TEL%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_CONTACT_TEL" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2df_lb_START_DT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_START_DT" runat="server" MaxLength="10" Width="81px" class="MandatoryField date" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_START_DATE%>"
                                    ControlToValidate="txt_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2df_ERR_START_DATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_END_DT" runat="server" Text="<%$Resources:Resource,wfb2df_lb_END_DT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_END_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" class="date"></asp:TextBox>
                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2df_ERR_START_DATE%>" ControlToValidate="txt_START_DT" ForeColor="Red"
                                    ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                                </asp:RegularExpressionValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2df_ERR_END_DATE%>" ControlToValidate="txt_END_DT" ForeColor="Red"
                                    ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                                </asp:RegularExpressionValidator>--%>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_START_DT"
                                    ControlToValidate="txt_END_DT" ErrorMessage="<%$Resources:Resource,wfb2df_Start_End_Date%>" Type="Date" Operator="GreaterThanEqual"
                                    Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                <asp:CustomValidator ID="qrytest2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2df_ERR_END_DATE%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </td>
                        </tr>


                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_ACCOM" runat="server" Text="<%$Resources:Resource,wfb2df_lb_ACCOM%>"></asp:Label>:</th>
                            <td>
                                <asp:DropDownList ID="ddl_ACCOM" class="MandatoryField" runat="server" OnSelectedIndexChanged="ddl_ACCOM_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_Required_ACCOM%>"
                                    ControlToValidate="ddl_ACCOM" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>

                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_ACCOM_BUILDING" runat="server" Text="<%$Resources:Resource,wfb2df_lb_ACCOM_BUILDING%>"></asp:Label>:</th>
                            <td>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddl_ACCOM_BUILDING" class="MandatoryField" runat="server"></asp:DropDownList>
                                       
                                        <asp:HiddenField ID="hid_CODE_VAL1" runat="server" ClientIDMode="Static" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddl_ACCOM" EventName="SelectedIndexChanged" />

                                    </Triggers>
                                </asp:UpdatePanel>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_Required_ACCOM_BUILDING%>"
                                    ControlToValidate="ddl_ACCOM_BUILDING" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_ROOM_NO" runat="server" Text="<%$Resources:Resource,wfb2df_lb_ROOM_NO%>"></asp:Label>:</th>
                            <td>
                                <asp:TextBox ID="txt_ROOM_NO" runat="server" MaxLength="4" Width="81px" class="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_ROOM_NO%>"
                                    ControlToValidate="txt_ROOM_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2DF_ROOM_NO_isError%>" ClientValidationFunction="CheckRoomNo" ForeColor="Red"
                                    ControlToValidate="txt_ROOM_NO" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                        </tr>

                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_AMOUNT_TH" runat="server" Text="<%$Resources:Resource,wfb2df_lb_AMOUNT%>"></asp:Label>:</th>
                            <td>
                                <asp:DropDownList ID="ddl_AMOUNT" runat="server" class="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                                <asp:Label ID="lb_AMOUNT" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                                <asp:HiddenField ID="hid_AMOUNT" runat="server" ClientIDMode="Static" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_AMOUNT%>"
                                    ControlToValidate="ddl_AMOUNT" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_OTHER_AMOUNT" runat="server" Text="<%$Resources:Resource,wfb2df_lb_OTHER_AMOUNT%>"></asp:Label>:</th>
                            <td>
                                <asp:TextBox ID="txt_OTHER_AMOUNT" runat="server" MaxLength="4" Width="81px" Style="ime-mode:disabled" ClientIDMode="Static"></asp:TextBox>

                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2df_lb_REMARK%>"></asp:Label>:</th>
                            <td colspan="3">
                                <asp:TextBox ID="txt_REMARK" runat="server" MaxLength="40" Width="220px" ClientIDMode="Static"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2df_REMARK%>" ClientValidationFunction="CheckRemark" ForeColor="Red"
                                    ControlToValidate="txt_REMARK" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_MOTOR_NO" runat="server" Text="<%$Resources:Resource,wfb2df_lb_MOTOR_NO%>"></asp:Label>:</th>
                            <td>
                                <asp:TextBox ID="txt_MOTOR_NO" runat="server" MaxLength="8" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator3" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2DF_MOTOR_NO_isError%>" ClientValidationFunction="CheckMotorNo" ForeColor="Red"
                                    ControlToValidate="txt_MOTOR_NO" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_CAR_NO" runat="server" Text="<%$Resources:Resource,wfb2df_lb_CAR_NO%>"></asp:Label>:</th>
                            <td>
                                <asp:TextBox ID="txt_CAR_NO" runat="server" MaxLength="8" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator4" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2DF_CAR_NO_isError%>" ClientValidationFunction="CheckCarNo" ForeColor="Red"
                                    ControlToValidate="txt_CAR_NO" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_PUBLIC_CAR" runat="server" Text="<%$Resources:Resource,wfb2df_lb_PUBLIC_CAR%>"></asp:Label>:</th>
                            <td>
                                <asp:DropDownList ID="ddl_public_car" runat="server">
                                    <asp:ListItem Text="無" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="有" Value="1"></asp:ListItem>
                                </asp:DropDownList>

                            </td>
                        </tr>

                        <tr>
                            <th></th>
                            <td align="right" class="Body_label" colspan="5">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                    <ContentTemplate>
                                        <aces:Btn ID="WFB2DF0200Save" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Save%>" OnClick="WFB2DF0200Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" formnovalidate />
                                        <%--<asp:Button ID="WFB2DF0200Save" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Save%>" OnClick="WFB2DF0200Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" formnovalidate />--%>
                                        <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2df_btn_back%>" OnClick="btn_back_Click" OnClientClick="return confirm('是否確定取消?');" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="WFB2DF0200Save" EventName="Click" />

                                    </Triggers>
                                </asp:UpdatePanel>

                            </td>
                        </tr>



                        <!-- end: Create MODULE ID -->
                        <!-- START: Create a line to separate Search field with body field -->
                        <tr>
                            <td align="center" height="1" colspan="6">
                                <hr>
                            </td>
                        </tr>
                        <!-- END: Create a line -->
                    </tbody>
                </table>

            </td>
        </tr>
        <tr>

            <td align="right" class="Body_label"></td>
        </tr>
    </table>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>
