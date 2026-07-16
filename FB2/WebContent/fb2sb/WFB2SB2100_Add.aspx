<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sb/WFB2SB2100_ADD.aspx.cs" Inherits="WebContent_fb2sb_WFB2SB2100_ADD" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #txt_EMP_NAME {
            color:black;
            background-color:white;

        }
        #txt_SALARY_NAME {
            color:black;
            background-color:white;

        }
        .decimal
        {
            text-align :right;
        }
        
    </style>
    <title></title>
    
   
      
    <script type="text/javascript">


        jQuery(document).ready(function () {

            iniForm();
        });


        function iniForm() {
            $('#txt_EMP_NAME').attr("readonly", true);
            $('#txt_SALARY_NAME').attr("readonly", true);
            $('#txt_DEPT_DESC').attr("readonly", true);
            $('#txt_EMP_CD').attr("readonly", true);

            $(".dateYM").datepicker({ dateFormat: 'yymm' });
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".dateYM").mask('9999/99');
            $(".date").mask('9999/99/99');
            $(".decimal").mask('9999999');
            $(".empid").mask('99999');
            
            $.unblockUI();
            $('#txt_START_DT_S,#txt_START_DT_E').change(function () {
                if ($('#txt_START_DT_S').val() != '' && $('#txt_START_DT_E').val() != '') {
                    var b = new Date($('#txt_START_DT_S').val());
                    var a = new Date($('#txt_START_DT_E').val());
                    if (b > a) {
                        $(this).val("");
                        alert("起日不可大於迄日");
                    }
                }
            });
            $('#txt_EMP_ID').change(function () {
                if ($('#txt_EMP_ID').val().length == 5) {
                    //ajax 取得員工基本資料
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != ""){
                                $('#txt_EMP_ID').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val(JData.EMP_NAME);
                                $('#txt_EMP_CD').val(JData.EMP_CD + "-" + JData.SUB_DESC);
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
                                $('#txt_DEPT_DESC').val(JData.DEPT_NO + '-' + JData.DEPT_NAME);
                                $('#txt_LEVEL_CD').val(JData.LEVEL_CD);

                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                }
            });
            $('#txt_SALARY_ID').change(function () {
                if ($('#txt_SALARY_ID').val().length == 4) {
                    //ajax 取得薪資
                    doSALARYIDAjax();
                }
            });
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function ClearAll() {
            $('#ddl_SYS_ID').val(-1);
        }

        function CheckMODE_ID(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if (!re.test($("#txt_MODE_ID_Add").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidWFB2IB_Mod_NotChoiceMessage').val());
                return false;
            }
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
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();
          

           
            return processed;
        }
        function doEmpAjax() {
            //ajax 取得員工基本資料
            $.ajax({
                url: "../commgeo/WFB2GetEmpData.ashx",
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
                    , EMP_ID: $('#Hid_EMP_ID').val()
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
        function CheckSaveAction() {
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    alert($('#hidWFB2IB_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hidWFB2IB_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hidWFB2IB_Save_ConfirmMessage').val());
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
        function openSalaryID() {
            var salaryId = $("#txt_SALARY_ID").val();
            OpenSearch('SalaryItem_Search.aspx', 'txt_SALARY_ID', 'txt_SALARY_NAME', "SALARY_ID=" + salaryId + "&isPermissions=Y", '');
        }
        function CancelConfirm() {
            return confirm($("#hidfb2CancelConfirm").val());
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
                                <col width="15%" />
                                <col width="30%" />
                                <col width="15%" />
                                <col width="40%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_SALARY_ID%>"></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_ID" runat="server" MaxLength="4" Width="64px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <input id="Button8" type="button" value="..." onclick="openSalaryID();" />
                                        <asp:TextBox ID="txt_SALARY_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sb_SALARY_ID_Required%>"
                                            ControlToValidate="txt_SALARY_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                    </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_ID" runat="server" Text="<%$Resources:Resource,WFB2SB_EMP_ID%>"></asp:Label>:
                                        <asp:HiddenField ID="Hid_EMP_ID" runat="server" ClientIDMode="Static" />

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField empid"></asp:TextBox>
                                         <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', '', '');" />
                                       <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sb_EMP_ID_Required%>"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_cd%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_CD" runat="server" ClientIDMode="Static" BorderWidth="0" ></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2se_lb_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                            <asp:TextBox ID="txt_DEPT_DESC" runat="server" BorderWidth="0" ClientIDMode="Static"  Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="CHG_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sb_CHG_STATUS%>"></asp:Label>:
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
                                    <asp:Label ID="lb_CHG_AMT_A" runat="server" Text="<%$Resources:Resource,wfb2sb_CHG_AMT%>"></asp:Label>:
                                </th>
                                    
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CHG_AMT_A" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField decimal" ></asp:TextBox>
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
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_START_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_START_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sb_START_DT_S_Required%>"
                                            ControlToValidate="txt_START_DT_S" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sb_ERR_START_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>

                                    </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT_E" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_END_DATE_A%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_START_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="請輸入加扣款期間迄!"
                                            ControlToValidate="txt_START_DT_E" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="rvenddate" runat="server" ErrorMessage="加扣款期間迄格式錯誤,只可為2000~2199年" 
                                            ValidationExpression="(20|21)\d\d[/ /.](0[1-9]|1[012])[/ /.](0[1-9]|[12][0-9]|3[01])" 
                                            ControlToValidate="txt_START_DT_E" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RegularExpressionValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sb_ERR_END_DATE_A%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CompareValidator ID="cv_DEF_YMA" runat="server" ForeColor="Red" ControlToCompare="txt_START_DT_S"
                                            ControlToValidate="txt_START_DT_E" ErrorMessage="<%$Resources:Resource,wfb2sk_greaterthan_DATA_YM%>" Type="String" Operator="GreaterThan"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>

                                    </tr>
                                    <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbl_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sb_REMARK%>"></asp:Label>:
                                </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_REMARK" runat="server" MaxLength="100" Columns="30" ClientIDMode="Static" Rows="5" TextMode="MultiLine" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sb_REMARK_Required%>"
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
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SB2100Ok1" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2SB2100Ok1_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA"/>
                                             <%--<asp:Button ID="WFB2SB2100Ok1" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2SB2100Ok1_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA"/>--%>
                                            <asp:Button ID="WFB2IB0100Clear" runat="server" Text="<%$Resources:Resource,btn_Cancel%>" OnClick="WFB2SB2100Clear_Click" OnClientClick="return CancelConfirm();" />
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

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidfb2CancelConfirm" Value="<%$Resources:Resource,wfb2CancelConfirm%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


