<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2300_Mod.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2300_Mod" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            initForm();

        });
        function initForm() {
            $('.num').mask('9999999');
            $("#lb_DATA_YM_txt").mask('9999/99');
            $.unblockUI();
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

        function CheckAMOUNT(source, arguments) {
            var re = /^(0|[1-9][0-9]*)$/;
            if ($("#txt_ORDER_SEQ").val() != "") {
                if (!re.test($("#txt_ORDER_SEQ").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
        }
        function checkConfirm() {
           return confirm($('#hidwfb2sc_cancel_ConfirmMessage').val());
        }
        function checkRemarkLength(source, arguments) {
            var remark = $("#txt_REMARK").val();
            if (remark.length > 100)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
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
                            <%--發薪日期--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_SALARY_DT_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <%--發薪類別--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_SALARY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_SALARY_TYPE_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <%--資料年月--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_DATA_YM" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_DATA_YM%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_DATA_YM_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <%--發放項目--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_PAY_KIND" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PAY_KIND%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_PAY_KIND_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <%--工號--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_ID%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_EMP_ID_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <%--姓名--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_NAME2%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_EMP_NAME_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <%--聘用單位--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_COMPANY_CD%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_COMPANY_CD_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <%--員工區分--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_CD%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_EMP_CD_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <%--入社日期--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_JOIN_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_JOIN_DT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_JOIN_DT_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <%--離社日期--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_LEAVE_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_LEAVE_DT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_LEAVE_DT_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <%--部門代號--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2sc_DEPT_NO%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:Label ID="lb_DEPT_NO_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <%--異動狀態--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_CHG_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_CHG_STATUS%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_CHG_STATUS_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <%--處理狀態--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_PROCESS_STATUS_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <%--薪資項目--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_ID%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_SALARY_ID_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <%--來源--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_DATA_SRC" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_DATA_SRC%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_DATA_SRC_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <%--加減項--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_PLUS" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_PLUS%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_IS_PLUS_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <%--應稅--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_TAX" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_TAX_Y%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_IS_TAX_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <%--異動前金額--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_CHG_AMT_B" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_B%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_CHG_AMT_B_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <%--異動後金額--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_CHG_AMT_A" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_A%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_CHG_AMT_A" Style="text-align: right;" runat="server" MaxLength="7" CssClass="MandatoryField num" Width="80px" ClientIDMode="Static"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_A_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_CHG_AMT_A" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <%--備註說明--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_REMARK%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:TextBox ID="txt_REMARK" Height="50px" TextMode="MultiLine" runat="server" MaxLength="100" Width="400px" class="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_REMARK_isNull%>"
                                ControlToValidate="txt_REMARK" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_REAMARK_length%>" ClientValidationFunction="checkRemarkLength" ForeColor="Red"
                                ControlToValidate="txt_REMARK" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </tr>
                        <tr>
                            <%--異動人員--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_CREATED_BY" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_CREATED_BY%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_CREATED_BY_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <%--異動日期--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_CREATED_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_CREATED_DT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_CREATED_DT_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <%--主管核定備註--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_APP_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_APP_REMARK%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:TextBox ID="txt_APP_REMARK" Height="50px" TextMode="MultiLine" runat="server" MaxLength="100" Enabled="false" Width="400px" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <%--<asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_lb_REAMARK_length%>" ClientValidationFunction="checkAPPRemarkLength" ForeColor="Red"
                                ControlToValidate="txt_APP_REMARK" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>--%>
                        </tr>
                        <tr>
                            <%--核可人員--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_APPROVE_BY" runat="server" Text="<%$Resources:Resource,wfb2sc_APPROVE_BY%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_APPROVE_BY_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                            <%--核可日期--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_APPROVE_DT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:Label ID="lb_APPROVE_DT_txt" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" class="Body_label" colspan="8">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                    <ContentTemplate>
                                        <aces:Btn ID="WFB2SC2300Ok2" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClick="WFB2SC2300Ok2_Click" ValidationGroup="GroupA" />
                                        <%--<asp:Button ID="WFB2SC2300Ok2" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClick="WFB2SC2300Ok2_Click" ValidationGroup="GroupA" />--%>
                                       <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_cancel%>" OnClientClick="return checkConfirm();" OnClick="btn_back_Click" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="WFB2SC2300Ok2" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>



                        <!-- end: Create MODULE ID -->
                        <!-- START: Create a line to separate Search field with body field -->

                        <!-- END: Create a line -->
                    </tbody>
                </table>

            </td>
        </tr>
        <tr>
            <td align="right" class="Body_label"></td>
        </tr>
    </table>
    <asp:HiddenField ID="hid_SLARY_ID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hid_PAY_KIND" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hid_CHG_STATUS" runat="server" ClientIDMode="Static" />

    <asp:HiddenField ID="hid_SEQ_NO" runat="server" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_SaveCheck_message" Value="<%$Resources:Resource,wfb2sc_SaveCheck_message%>" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Save_ConfirmMessage%>" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Cancel_Confirm%>" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>
