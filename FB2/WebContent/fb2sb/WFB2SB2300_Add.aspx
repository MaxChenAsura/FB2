<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sb/WFB2SB2300_ADD.aspx.cs" Inherits="WebContent_fb2sb_WFB2SB2300_ADD" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .decimal {
            text-align: right;
        }
    </style>

    <script type="text/javascript">


        jQuery(document).ready(function () {
            iniForm();

        });
        function iniForm() {
            $("#txt_SALARY_NAME").attr("readonly", true);
            $("#txt_EMP_NAME").attr("readonly", true);
            $("#txt_LEAVE_DT").attr("readonly", true);
            $("#txt_EMP_CD").attr("readonly", true);
            $("#txt_DEPT_DESC").attr("readonly", true);


            $("#txt_DATA_YM").datepicker({ dateFormat: 'yy/mm' });
            $(".empid").mask('99999');
            $(".number").mask('9999999');
            $(".date").mask('9999/99');
            $.unblockUI();
            $('#txt_EMP_ID').change(function () {
                if ($('#txt_EMP_ID').val().length == 5) {
                    //ajax 取得員工基本資料
                    doEmpAjax();
                }
            });

            $('#txt_SALARY_ID').change(function () {
                if ($('#txt_SALARY_ID').val().length == 4) {
                    //ajax 取得薪資
                    doSALARYIDAjax();
                }
            });
            $('#txt_DATA_YM').change(function () {
                var strErr = "";
                //資料年月僅能系統月或系統月-1
                var dataYM = $("#txt_DATA_YM").val().replace("/", "");
                var totdayYM = getTodayDate().substr(0, 7).replace("/", "");
                var lastMonth = getThisMonth();
                if (dataYM == totdayYM || dataYM == lastMonth) {
                    processed = true;
                } else {
                    strErr = "僅能輸入系統月及系統月-1 \r\n";
                    alert(strErr);
                    $('#txt_DATA_YM').val("");
                    return false;
                }
                //資料年月需大於計薪年月
                var latestYM = $("#HID_Latest_SalaryYM").val();
                if (latestYM != 0 && dataYM <= latestYM) {
                    strErr = "此薪資年月已計薪,無法新增\r\n";
                    alert(strErr);
                    $('#txt_DATA_YM').val("");
                    return false;
                }
            });

        }


        function doEmpAjax() {
            //ajax 取得員工基本資料
            $.ajax({
                url: "WFB2SB_GetEmpData.ashx",
                data: {
                    EMP_ID: $('#txt_EMP_ID').val()
                },
                type: "GET",
                dataType: 'json',
                success: function (JData) {
                    if (JData.errMsg != "") {
                        $('#txt_EMP_NAME').val("");
                        $('#txt_EMP_CD').val("");
                        $('#txt_DEPT_DESC').val("");
                        $('#txt_LEAVE_DT').val("");
                        alert(JData.errMsg);
                    } else {
                        $('#txt_EMP_NAME').val(JData.EMP_NAME);
                        $('#txt_EMP_CD').val(JData.EMP_CD_DESC);
                        $('#txt_DEPT_DESC').val(JData.DEPT_NO + '-' + JData.DEPT_NAME);
                        $('#txt_LEAVE_DT').val(JData.LEAVE_DT);
                    }
                },

                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }

        function doSALARYIDAjax() {
            $.ajax({
                url: "../commgeo/WFB2GetSalaryData.ashx",
                data: {
                    SALARY_ID: $('#txt_SALARY_ID').val()
                    , EMP_ID: $('#HID_EMP_ID').val()
                },
                type: "GET",
                dataType: 'json',
                success: function (JData) {
                    if (JData.errMsg != "") {
                        $('#txt_SALARY_ID').val("");
                        $('#txt_SALARY_NAME').val("");
                        alert(JData.errMsg);
                    }
                    else {
                        $('#txt_SALARY_ID').val(JData.SALARY_ID);
                        $('#txt_SALARY_NAME').val(JData.SALARY_NAME);

                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }



        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function CheckMODE_ID(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if (!re.test($("#txt_MODE_ID_Add").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }


        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidWFB2IB_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
        //儲存前檢查
        function saveCheck() {


            var processed = true;



            if (Page_ClientValidate("GroupA")) {
                BlockUI();
                //return false;
            }
            else {
                processed = false;
                $.unblockUI();
                return processed
            }


            //var strErr="";
            ////資料年月僅能系統月或系統月-1
            //var dataYM = $("#txt_DATA_YM").val().replace("/","");
            //var totdayYM =getTodayDate().substr(0,7).replace("/","");
            //var lastMonth = getThisMonth();
            //if(dataYM ==totdayYM ||dataYM==lastMonth){
            //    processed = true;
            //}else{
            //    strErr += "僅能輸入系統月及系統月-1 \r\n";
            //    processed = false;
            //}
            ////資料年月需大於計薪年月
            //var latestYM =  $("#HID_Latest_SalaryYM").val();
            //if(latestYM !=0 && dataYM <= latestYM){
            //    strErr += "此薪資年月已計薪,無法新增\r\n";
            //    processed = false;
            //}
            //if (!processed){
            //    alert(strErr);
            //    $.unblockUI();
            //}


            if (confirm("確定要儲存?")) {

                processed = true;
            } else {
                processed = false;
            }

            if (!processed) {
                $.unblockUI();
            }

            return processed;
        }

        function getThisMonth() {
            var day = new Date();
            day.setDate(-1);
            var year = day.getFullYear();
            var month = day.getMonth() + 1;
            var date = day.getDate();
            month = (month < 10) ? "0" + month : month;
            date = (date < 10) ? "0" + date : date;
            var todayDate = year + month;
            return todayDate;
        }

        function doUnblok() {
            $.unblockUI();
        }

        function openSalaryID() {
            var salaryId = $("#txt_SALARY_ID").val();
            OpenSearch('SalaryItem_Search.aspx', 'txt_SALARY_ID', 'txt_SALARY_NAME', "SALARY_ID=" + salaryId + "&isPermissions=Y", '');
        }
        function AppendComma(n) {
            if (!/^((-*\d+)|(0))$/.test(n)) {
                var newValue = /^((-*\d+)|(0))$/.exec(n);
                if (newValue != null) {
                    if (parseInt(newValue, 10)) {
                        n = newValue;
                    }
                    else {
                        n = '0';
                    }
                }
                else {
                    n = '0';
                }
            }
            if (parseInt(n, 10) == 0) {
                n = '0';
            }
            else {
                n = parseInt(n, 10).toString();
            }

            n += '';
            var arr = n.split('.');
            var re = /(\d{1,3})(?=(\d{3})+$)/g;
            return arr[0].replace(re, '$1') + (arr.length == 2 ? '.' + arr[1] : '');
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--2990400_QRY開始-->
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_DATA_YM" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_DATA_YM%>"></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DATA_YM" MaxLength="7" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_DATA_YM_isNull%>"
                                            ControlToValidate="txt_DATA_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="Regular_txt_DATA_YM" runat="server"
                                            ErrorMessage="資料年月,日期格式錯誤" ControlToValidate="txt_DATA_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20|99)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_ID%>"></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_ID" runat="server" MaxLength="4" Width="64px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <input id="Button8" type="button" value="..." onclick="openSalaryID();" />
                                        <asp:TextBox ID="txt_SALARY_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sb_SALARY_ID_Required%>"
                                            ControlToValidate="txt_SALARY_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="Required_txt_SALARY_NAME" runat="server" ErrorMessage="此薪資項目未存於薪資項目主檔"
                                            ControlToValidate="txt_SALARY_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2ib_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField empid"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', '', '');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ib_EMP_ID_Required%>"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="Required_txt_EMP_NAME" runat="server" ErrorMessage="此工號未存入人事主檔"
                                            ControlToValidate="txt_EMP_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <!--驗證長度需為5 -->
                                        <asp:RegularExpressionValidator ID="Required_txt_EMP_NAME2" runat="server"
                                            ErrorMessage="工號長度須為5碼" ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression=".{5,5}" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2hc_hd_LEAVE_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LEAVE_DT" runat="server" Width="100px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_cd%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_CD" runat="server" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2se_lb_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbl_DEPT_NO" runat="server" Text="">
                                            <asp:TextBox ID="txt_DEPT_DESC" runat="server" BorderWidth="0" ClientIDMode="Static" Width="300px">
                                            </asp:TextBox></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="CHG_STATUS" runat="server" Text="<%$Resources:Resource,wfb2se_CHG_STATUS%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbl_CHG_STATUS" runat="server" Text="N-新增"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS2%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbl_PROCESS_STATUS" runat="server" Text="N-未核定"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_SALARY_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_SALARY_STATUS%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label">
                                        <asp:Label ID="txt_SALARY_STATUS" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" Text="N-未處理"></asp:Label>
                                    </td>
                                </tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_CHG_AMT_A" runat="server" Text="<%$Resources:Resource,wfb2sb_CHG_AMT%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_CHG_AMT_A" runat="server" MaxLength="7" Width="80px" ClientIDMode="Static" CssClass="MandatoryField decimal number"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sb_CHG_AMT_Required%>"
                                        ControlToValidate="txt_CHG_AMT_A" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidatorCHG_AMT_A" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="加扣款金額只能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                        ControlToValidate="txt_CHG_AMT_A" ValidationGroup="GroupA" Display="None">
                                    </asp:CustomValidator>
                                </td>
                </tr>
                <tr>
                    <th align="left" class="Body_TableHeader">
                        <asp:Label ID="lbl_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_remark%>"></asp:Label>:
                    </th>

                    <td align="left" class="Body_label" colspan="2">
                        <asp:TextBox ID="txt_REMARK" runat="server" Width="421px" Height="52px" MaxLength="100" ClientIDMode="Static" CssClass="MandatoryField" TextMode="MultiLine"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2si_required_Remark%>"
                            ControlToValidate="txt_REMARK" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <th align="left" class="Body_TableHeader">
                        <asp:Label ID="CREATED_BY" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_CREATED_BY%>"></asp:Label>:
                    </th>

                    <td align="left" class="Body_label">
                        <asp:Label ID="lbl_CREATED_BY" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th align="left" class="Body_TableHeader">
                        <asp:Label ID="CREATED_DT" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_CREATED_DT%>"></asp:Label>:
                    </th>

                    <td align="left" class="Body_label">
                        <asp:Label ID="lbl_CREATED_DT" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th align="left" class="Body_TableHeader">
                        <asp:Label ID="lb_NOTICE" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_NOTICE%>"></asp:Label>:
                    </th>

                    <td align="left" class="Body_label">
                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_NOTICE_WORD%>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <th></th>
                    <th></th>
                    <td align="right" class="Body_label">
                        <div id="init">
                            <aces:Btn ID="WFB2SB2300Ok1" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClick="WFB2SB2300Ok1_Click" OnClientClick="return saveCheck();" />
                            <%--<asp:Button ID="WFB2SB2300Ok1" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClick="WFB2SB2300Ok1_Click" OnClientClick="return saveCheck();" />--%>
                            <asp:Button ID="WFB2SB2300Cancel" runat="server" type="button" Text="<%$Resources:Resource,btn_Cancel%>" OnClick="WFB2SB2300Cancel_Click" OnClientClick="return confirm($('#hidwfb299_Cancel_ConfirmMessage').val());" />
                        </div>
                    </td>
                </tr>
                </tbody>
            </table>

            </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
            <tr>

                <td align="right" class="Body_label">
                    <div id="init_grid">
                    </div>

                </td>
            </tr>
            </table>
            <!--2990400_QRY結束-->
            <asp:HiddenField ID="HID_Latest_SalaryYM" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_CHG_AMT_A" runat="server" ClientIDMode="Static" />

            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Mod_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


