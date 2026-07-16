<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC5400_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC5400_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <style type="text/css">
        .TextUpper {
            text-transform: uppercase;
        }
    </style>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();

        });

        function iniForm() {
            $(".number").mask('9999/99');
            $(".number2").mask('99999');
            $("#txt_SALARY_YM").datepicker({ dateFormat: 'yy/mm' });
            $("#txt_DEPT_NAME,#txt_EMP_NAME").css("color", "black").css("background-color", "white").attr("disabled", true);
            $.unblockUI();
            $('#txt_EMP_ID').change(function () {
                if ($('#txt_EMP_ID').val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg == "1")
                                alert(JData.errMsg);
                            else {
                                $('#txt_EMP_NAME').val(JData.EMP_NAME);
                                $('#txt_EMP_NAME').keydown(false)
                                $('#txt_LICENSE_ID').val(JData.LICENSE_ID);
                                $('#txt_LICENSE_ID').keydown(false)
                                $('#txt_BIRTH_DT').val(JData.BIRTH_DT);
                                $('#txt_BIRTH_DT').keydown(false)
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                }
            });
            $("#txt_DEPT_NO").change(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DEPT_NAME').val("");
                }
            });
        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function ClearAll() {
            $('#txt_SALARY_YM').val("");
            $('#txt_DEPT_NO').val("");
            $('#txt_DEPT_NAME').val("");
            $('#txt_EMP_ID').val("");
            $('#txt_EMP_NAME').val("");
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

        //儲存前檢查
        function saveCheck(msg) {
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                //BlockUI();
            }
            else
                processed = false;

            if (processed) {
                processed = confirm("確定要進行" + msg);
                //BlockUI();
            }

            if (!processed)
                $.unblockUI();

            return processed;
        }


        function doUnBlock() {
            $.unblockUI();
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>

    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--2SC5400_QRY開始-->
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="45%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_YM" runat="server" MaxLength="7" Columns="10" ClientIDMode="Static" CssClass="MandatoryField number"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_ERR_SALARY_YM3%>"
                                            ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="Regular_txt_SALARY_YM_Add" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_ERR_SALARY_YM_DT%>" ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20|99)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static" CssClass="TextUpper"></asp:TextBox>
                                        <input id="Button14" type="button" value="..." onclick="OpenSearch('DeptGrid_Search.aspx', 'txt_DEPT_NO', 'txt_DEPT_NAME', '', '');" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>

                                    </td>

                                </tr>
                                <tr>

                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="number2"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', '', '');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="10">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SC5400ExcelExport" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC510Print%>" ValidationGroup="GroupA" OnClick="WFB2SC5400ExcelExport_Click" OnClientClick="return checkDowning(this.value);" />

                                            <%--<asp:Button ID="WFB2SC5400ExcelExport" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC510Print%>" ValidationGroup="GroupA" OnClick="WFB2SC5400ExcelExport_Click" OnClientClick="return saveCheck(this.value);" />--%>
                                            
                                            <asp:Button ID="WFB2SC5400Clear" runat="server" type="button" Text="<%$Resources:Resource,btn_clear%>" OnClientClick="ClearAll();" />
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
            <!--2SC5400_QRY結束-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Mod_NotChoiceMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Save_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_ConfirmMessage" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Dtl_NotChoiceMessage" />

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SC5400ExcelExport"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

</asp:Content>


