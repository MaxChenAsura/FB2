<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sb/WFB2SB2100_Update.aspx.cs" Inherits="WebContent_fb2sb_WFB2SB2100_Update" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .disablestyle {
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
            $("#txt_YEAR_MONTH_Add").datepicker({ dateFormat: 'yymm' });
            $("#txt_END_DT_A").datepicker({ dateFormat: 'yy/mm/dd' });
            
            $(".date").mask('9999/99/99');
            $(".decimal").mask('99999999');
            $(".empid").mask('99999');
            
            $.unblockUI();
            
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
                                <col width="8%" />
                                <col width="15%" />
                                <col width="8%" />
                                <col width="15%" />
                                <col width="20%" />
                                <col width="25%" />

                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_ID%>"></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:label ID="txt_SALARY_ID" runat="server" MaxLength="10" Width="120px"  ClientIDMode="Static"></asp:label>
                                        <asp:HiddenField ID="Hid_SALARY_ID" runat="server" />
                                    </td>
                                    </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2se_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:label ID="txt_EMP_ID" runat="server" MaxLength="5" Width="120px" ClientIDMode="Static"></asp:label>
                                   <asp:HiddenField ID="Hid_EMP_ID" runat="server" />
                                         </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_cd%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:label ID="ddl_EMP_CD" runat="server" ClientIDMode="Static"></asp:label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2sj_lb_dept_no%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbl_DEPT_NO" runat="server" Text=""></asp:Label>:
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="CHG_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sm_CHG_STATUS%>"></asp:Label>:
                                    </th>
                                    
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbl_CHG_STATUS" runat="server" Text=""></asp:Label>
                                        <asp:HiddenField ID="hid_CHG_STATUS" runat="server" ClientIDMode="Static" />
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_CHECK_STATUS%>"></asp:Label>:
                                    </th>
                                    
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbl_PROCESS_STATUS" runat="server"></asp:Label>
                                        <asp:HiddenField id="hid_PROCESS_STATUS" runat="server" ClientIDMode="Static" />
                                    </td>
                                </tr>
                                <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_CHG_AMT_B" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_B_2%>"></asp:Label>:
                                </th>
                                    
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_AMOUNT" runat="server"  Width="64px" ClientIDMode="Static" Enabled="false" style="text-align:right" BorderWidth="0" CssClass="disablestyle"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_CHG_AMT_A" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_A_2%>"></asp:Label>:
                                </th>
                                    
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CHG_AMT_A" MaxLength="7" runat="server" Width="64px" ClientIDMode="Static"  CssClass="MandatoryField decimal" ></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="請輸入異動後加扣款金額"
                                            ControlToValidate="txt_CHG_AMT_A" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorCHG_AMT_A" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="異動後加扣款金額只能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_CHG_AMT_A" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                      <asp:HiddenField ID="hid_CHG_AMT_A" runat="server" ClientIDMode="Static" />

                                    </td>
                                    </tr>
                                   <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_START_DT%>" ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_START_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static"  Enabled="false" CssClass="date disablestyle" ReadOnly="true" BorderWidth="0"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT_E" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_END_DATE_A%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_START_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" Enabled="false" ReadOnly="true" BorderWidth="0" CssClass="disablestyle"></asp:TextBox>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_END_DT_A" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_END_DT_A%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_END_DT_A" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="請選擇異動後加扣款期間迄"
                                            ControlToValidate="txt_END_DT_A" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cv_date" runat="server" ErrorMessage="異動後加扣款期間迄不可大於加扣款期間起!" ControlToValidate="txt_END_DT_A" ControlToCompare="txt_START_DT_S"   Operator="GreaterThanEqual" ValidationGroup="GroupA" ForeColor="Red" Display="None">
                                        </asp:CompareValidator>
                                          <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sb_ERR_END_DT_A%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_END_DT_A" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                  </td>
                                    </tr>
                                    <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lbl_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_REMARK%>"></asp:Label>:
                                </th>
                                        
                                            
                                            <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_REMARK" runat="server" ClientIDMode="Static" TextMode="MultiLine" CssClass="MandatoryField" Columns="45" Rows="3" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sb_REMARK_Required%>"
                                            ControlToValidate="txt_REMARK" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                            </td>
                                            <caption>
                                                
                                        </caption>
                                       
                                    </tr>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="APPROVE_BY" runat="server" Text="<%$Resources:Resource,wfb2sc_APPROVE_BY%>"></asp:Label>:
                                    </th>
                                    
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbl_APPROVE_BY" runat="server" Text=""></asp:Label>
                                    </td>
                                    
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_APPROVE_DT%>"></asp:Label>:
                                    </th>
                                    
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbl_APPROVE_DT" runat="server" Text="" ></asp:Label>
                                    </td>
                                    </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="APP_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_APP_REMARK%>"></asp:Label>:
                                    </th>
                                    
                                    <td align="left" class="Body_label">
                                                <asp:TextBox ID="txt_APP_REMARK" runat="server" ClientIDMode="Static" Columns="45" Rows="3" TextMode="MultiLine" Enabled="false" ></asp:TextBox>
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
                                        <asp:Label ID="lbl_CREATED_DT" runat="server" Text=""></asp:Label>:
                                    </td>
                                    </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SB2100Ok2" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2SB2100Ok2_Click"  ValidationGroup="GroupA" OnClientClick="CheckValid();"  />
                                            <%--<asp:Button ID="WFB2SB2100Ok2" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2SB2100Ok2_Click"  ValidationGroup="GroupA" OnClientClick="CheckValid();"  />--%>
                                            <asp:Button ID="WFB2IB0100Clear" runat="server" type="button" Text="<%$Resources:Resource,btn_Cancel%>" OnClientClick="return CancelConfirm();" OnClick="WFB2IB0100Clear_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                    </td>
                </tr>
            </table>


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


