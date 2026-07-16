<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sb/WFB2SB2300_Update.aspx.cs" Inherits="WebContent_fb2sb_WFB2SB2300_Update" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #txt_CHG_AMT_B {
            text-align: right;
            background-color: white;
            border-width: 0;
            color: black;
        }

        .decimal {
            text-align: right;
        }
    </style>
    <title></title>



    <script type="text/javascript">


        jQuery(document).ready(function () {

            iniForm();
        });


        function iniForm() {
            $(".date").mask('9999/99/99');
            $(".number").mask('9999999');
            $.unblockUI();


            $('#txt_REMARK').change(function () {

                if ($('#txt_REMARK').val().length > 150) {
                    $('#txt_REMARK').val($('#txt_REMARK').val().substring(0, 150));
                }
            });
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
                                <col width="14%" />
                                <col width="30%" />
                                <col width="12%" />
                                <col width="44%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_DATA_YM" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_DATA_YM%>"></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbll_DATA_YM" runat="server" MaxLength="10" Width="123px" ClientIDMode="Static"></asp:Label>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_SALARY_ID%>"></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbll_SALARY_ID" runat="server" MaxLength="10" Width="120px" ClientIDMode="Static"></asp:Label>

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:Label ID="lbll_EMP_ID" runat="server" MaxLength="5" Width="120px" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2sb_lbl_EMP_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbll_EMP_CD" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2sc_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbll_DEPT_NO" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_CHG_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_CHG_STATUS%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbll_CHG_STATUS" runat="server"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS2%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbll_PROCESS_STATUS" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_SALARY_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_SALARY_STATUS%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:Label ID="txt_SALARY_STATUS" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_CHG_AMT_B" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_B_2%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_CHG_AMT_B" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_CHG_AMT_A" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_A_2%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_CHG_AMT_A" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static" CssClass="MandatoryField decimal number"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="請輸入異動後加扣款金額"
                                            ControlToValidate="txt_CHG_AMT_A" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorCHG_AMT_A" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="異動後加扣款金額只能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_CHG_AMT_A" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>

                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_REMARK%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_REMARK" runat="server" ClientIDMode="Static" CssClass="MandatoryField" Height="52px" TextMode="MultiLine" Width="380px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sb_REMARK_Required%>"
                                            ControlToValidate="txt_REMARK" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                        </asp:RequiredFieldValidator>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_APPROVE_BY" runat="server" Text="<%$Resources:Resource,wfb2sc_APPROVE_BY%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbll_APPROVE_BY" runat="server" Text=""></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_APPROVE_DT%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbll_APPROVE_DT" runat="server" Text="" CssClass="date"></asp:Label>
                                    </td>
                                </tr>
                                <tr>

                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_APP_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_APP_REMARK%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_APP_REMARK" runat="server" ClientIDMode="Static" Enabled="false" Height="52px" TextMode="MultiLine" Width="380px" MaxLength="150"></asp:TextBox>
                                    </td>

                                </tr>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_CREATED_BY" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_CREATED_BY%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:Label ID="lbll_CREATED_BY" runat="server" ></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_CREATED_DT" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_CREATED_DT%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:Label ID="lbll_CREATED_DT" runat="server" ></asp:Label>
                                        <asp:HiddenField ID="hid_EMP_ID" runat="server" />
                                        <asp:HiddenField ID="hid_SALARY_ID" runat="server" />
                                        <asp:HiddenField ID="hid_SEQ_NO" runat="server" />
                                        <asp:HiddenField ID="hid_CHG_STATUS" runat="server" />
                                        <asp:HiddenField ID="hid_PROCESS_STATUS" runat="server" />
                                        <asp:HiddenField ID="hid_CHG_AMT_A" runat="server" ClientIDMode="Static" />

                                    </td>
                                </tr>

                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SB2300Ok2" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClick="WFB2SB2300Ok2_Click" ValidationGroup="GroupA" OnClientClick="return saveCheck();" />
                                            <%--<asp:Button ID="WFB2SB2300Ok2" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClick="WFB2SB2300Ok2_Click" ValidationGroup="GroupA" OnClientClick="return saveCheck();" />--%>
                                            <asp:Button ID="WFB2SB2300Cancel" runat="server" OnClick="WFB2SB2300Cancel_Click" OnClientClick="return confirm($('#hidwfb299_Cancel_ConfirmMessage').val());" Text="<%$Resources:Resource,btn_Cancel%>" type="button" />
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


