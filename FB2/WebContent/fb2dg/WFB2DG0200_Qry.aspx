<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2dg/WFB2DG0200_Qry.aspx.cs" Inherits="WebContent_fb2dg_WFB2DG0200_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $("#txt_UPDATED_DT_S").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_UPDATED_DT_E").datepicker({ dateFormat: 'yy/mm/dd' });
            $("#txt_UPDATED_DT_S").mask("9999/99/99");
            $("#txt_UPDATED_DT_E").mask("9999/99/99");
            $.unblockUI();
            $('#txt_UPDATED_DT_S,#txt_UPDATED_DT_E').change(function () {
                if ($('#txt_UPDATED_DT_S').val() != '' && $('#txt_UPDATED_DT_E').val() != '') {
                    var b = new Date($('#txt_UPDATED_DT_S').val());
                    var a = new Date($('#txt_UPDATED_DT_E').val());
                    if (b > a) {
                        $(this).val("");
                        alert("起日不可大於迄日");
                    }
                }
            });




        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function ClearAll() {
            $('#txt_SALARY_ID').val("");
            $('#txt_START_SDT_B').val("");
            $('#txt_START_SDT_A').val("");
            $('#txt_EMP_ID').val("");
            $('#txt_EMP_NAME').val("");
        }

        function MycheckDowning(msg) {
            var dValue = $('#ddl_PAG_TYPE').val("");
            var processed = true;
            if (dValue == '4') {
                if (Page_ClientValidate("GroupA")) {
                    BlockUI();
                }
                else
                    return;
            }
            
            BlockUI();
            processed = confirm("確定要進行" + msg + "?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }


        //儲存前檢查
        function saveCheck() {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;

            if (!processed) {
                $.unblockUI();
                return;
            }
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
        function doUnBlock() {
            $.unblockUI();
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--2DG020_QRY開始-->
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="40%" />


                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAG_TYPE" runat="server" Text="<%$Resources:Resource,wfb2dg_lb_PAG_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_PAG_TYPE" runat="server" AutoPostBack="true">
                                            <asp:ListItem Value="1" Text="<%$Resources:Resource,wfb2dg_ddl_PAG_TYPE_1%>"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="<%$Resources:Resource,wfb2dg_ddl_PAG_TYPE_2%>"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="<%$Resources:Resource,wfb2dg_ddl_PAG_TYPE_3%>"></asp:ListItem>
                                            <asp:ListItem Value="4" Text="<%$Resources:Resource,wfb2dg_ddl_PAG_TYPE_4%>"></asp:ListItem>
                                        </asp:DropDownList>

                                    </td>
                                </tr>
                                <tr id="Tie" runat="server" visible="false">
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_UPDATED_DT" runat="server" Text="<%$Resources:Resource,wfb2dg_lb_UPDATED_DT%>"></asp:Label>:

                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_UPDATED_DT_S" runat="server" Columns="10" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>~<asp:TextBox ID="txt_UPDATED_DT_E" runat="server" Columns="10" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_ERR_UPDATED_DT_S%>"
                                            ControlToValidate="txt_UPDATED_DT_S" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>

                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_stime%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_UPDATED_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sh_format_award_stime%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_UPDATED_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dg_ERR_UPDATED_DT_E%>"
                                            ControlToValidate="txt_UPDATED_DT_E" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>

                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>

                                    <td align="right" class="Body_label" colspan="10">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DG020ExcelExport" runat="server" Text="<%$Resources:Resource,btn_excelexport%>" ValidationGroup="GroupA" OnClick="WFB2DG020ExcelExport_Click" OnClientClick="return MycheckDowning(this.value);" />

                                            <%--<asp:Button ID="WFB2DG020ExcelExport" runat="server" Text="<%$Resources:Resource,btn_excelexport%>" ValidationGroup="GroupA" OnClick="WFB2DG020ExcelExport_Click" OnClientClick="return MycheckDowning(this.value);" />--%>
                                            
                                            <asp:Button ID="WFB2DG020Clear" runat="server" type="button" Text="<%$Resources:Resource,btn_clear%>" OnClientClick="ClearAll();" />
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
            <!--2DG020_QRY結束-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Mod_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Save_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Dtl_NotChoiceMessage" />

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DG020ExcelExport"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

</asp:Content>


