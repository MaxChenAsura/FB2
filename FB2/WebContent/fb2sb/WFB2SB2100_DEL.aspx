<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sb/WFB2SB2100_Del.aspx.cs" Inherits="WebContent_fb2sb_WFB2SB2100_Del" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>



    <script type="text/javascript">


        jQuery(document).ready(function () {

            iniForm();
            $(".PERSON").blur(function () {
                alert("This input field has lost its focus.");

            });

        });


        function iniForm() {
            $("#txt_YEAR_MONTH_Add").datepicker({ dateFormat: 'yymm' });
            $(".time").mask('9999/99/99');
            $(".number").mask('9999/99/99');
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


            //if (Page_ClientValidate("GroupA")) {
            //    BlockUI();
            //}
            //else
            //    processed = false;
            //if (!processed)
            //    $.unblockUI();

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
                                        <asp:Label ID="lb_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_SALARY_ID%>"></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="txt_SALARY_ID" runat="server" MaxLength="10" Width="120px" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_id%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="txt_EMP_ID" runat="server" MaxLength="5" Width="120px" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2sg_lb_emp_cd%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="txt_EMP_CD" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2se_lb_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbl_DEPT_NO" runat="server" Text="">KV20000-系統情報室應用系統開發課</asp:Label>:
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="CHG_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_CHG_STATUS%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbl_CHG_STATUS" runat="server" Text="D-刪除"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_PROCESS_STATUS%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lbl_PROCESS_STATUS" runat="server" Text="N-未核定"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CHG_AMT_B" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_CHG_AMT_B_2%>"></asp:Label>:
                                    </th>

                                    <td align="left" class="Body_label">
                                        <asp:Label ID="txt_AMOUNT" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_START_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="txt_START_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="time"></asp:Label>


                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT_E" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_END_DATE_A%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="txt_START_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="time"></asp:Label>

                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sb_lb_REMARK%>"></asp:Label>:
                                    </th>


                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_REMARK" runat="server" ClientIDMode="Static" Width="340px" Rows="3" TextMode="MultiLine"></asp:TextBox>
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
                                            <aces:Btn ID="WFB2SB2100Ok3" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2SB2100Ok3_Click" />
                                            <%--<asp:Button ID="WFB2SB2100Ok3" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2SB2100Ok3_Click" />--%>
                                            <asp:Button ID="WFB2IB0100Clear" runat="server" type="button" Text="<%$Resources:Resource,btn_Cancel%>" OnClick="WFB2SB2100Clear_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                    </td>
                </tr>
            </table>

            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


