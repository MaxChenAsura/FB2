<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sd/WFB2SD1300_Qry.aspx.cs" Inherits="WebContent_fb2sd_WFB2SD1300_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $("#txt_REMIT_DT").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $.unblockUI();
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function ClearAll() {
            //$('#').val(-1);
            $('#txt_REMIT_DT').val("");
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
                    alert($('#hidwfb2sc_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hidwfb2sc_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hidwfb2sc_Save_ConfirmMessage').val());
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
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--2SD130_QRY開始-->
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
                                        <asp:Label ID="lb_REMIT_DT" runat="server" Text="<%$Resources:Resource,wfb2sd_lb_REMIT_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_REMIT_DT" runat="server" MaxLength="10" Columns="10" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sd_lb_REMIT_DT_Required%>"
                                            ControlToValidate="txt_REMIT_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_stime%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_REMIT_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                                    </td>
                                    <th align="left">
                                        <%--<asp:Label ID="lbl_SALARY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sd_lbl_SALARY_TYPE%>"></asp:Label>:--%>
                                    </th>
                                    <td align="left" class="Body_label">
                                        <%--<asp:DropDownList ID="ddl_SALARY_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField">
                                            <asp:ListItem Value="-1"  Selected="True">請選擇</asp:ListItem>
                                            <asp:ListItem Value="A">A-月薪資類</asp:ListItem>
                                            <asp:ListItem Value="B">B-預付薪類</asp:ListItem>
                                            <asp:ListItem Value="C">C-獎金類</asp:ListItem>
                                            <asp:ListItem Value="D">D-節金類</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sd_lbl_SALARY_TYPE_Required%>"
                                        ControlToValidate="ddl_SALARY_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                    </asp:RequiredFieldValidator>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="4">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SD130ExcelExport" runat="server" Text="<%$Resources:Resource,btn_excelexport%>" OnClick="WFB2SD130ExcelExport_Click" ValidationGroup="GroupA" OnClientClick="return saveCheck();" />

                                            <%--<asp:Button ID="WFB2SD130ExcelExport" runat="server" Text="<%$Resources:Resource,btn_excelexport%>" OnClick="WFB2SD130ExcelExport_Click" ValidationGroup="GroupA" OnClientClick="return saveCheck();" />--%>
                                            <asp:Button ID="WFB2SD130Clear" runat="server" type="button" Text="<%$Resources:Resource,btn_clear%>" OnClientClick="ClearAll();" />
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
            </table>
            <!--2SD130_QRY結束-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Mod_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Save_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Dtl_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_XLSMessage" Value="<%$Resources:Resource,wfb2sd_XLSMessage_Required%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SD130ExcelExport"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


