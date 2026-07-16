<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dl/WFB2DL0103_LabelPrint.aspx.cs" Inherits="WebContent_fb2dl_WFB2DL0103_LabelPrint" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $.unblockUI();
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $('#txt_DEPT_NAME').attr("readonly", true);
            $('#txt_qry_EMP_NAME').attr("readonly", true);
            $('#txt_Year').mask('9999');

            //工號取得姓名的ajax
            $("#txt_EMP_ID_search").change(function () {
                if ($("#txt_EMP_ID_search").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID_search').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_qry_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_qry_EMP_NAME').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_qry_EMP_NAME').val("");
                }
            });

            //部門代號取得部門名稱的ajax
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
        function checkYear(source, arguments) {
            var today = new Date();
            var currentYear = today.getFullYear();
            var re = /^[0-9]{4}$/;
            if (re.test($("#txt_Year").val())) {
                if ($("#txt_Year").val() >= 1911 && $("#txt_Year").val() <= currentYear + 1) {
                    arguments.IsValid = true;
                }
                else {
                    arguments.IsValid = false;
                }
            }
            else {
                arguments.IsValid = false;
            }
        }

        function doUnBlock() {
            $.unblockUI();
        }
        function MycheckDowning(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return;
            BlockUI();
            processed = confirm($("#hidconfirm_Execute").val() + msg + "?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="60%" />
                            </colgroup>
                            <tbody>
                                <!-- START: 1st Line in Search Criteria area -->
                                <tr>
                                    <%--生成年度--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_Year" runat="server" Text="<%$Resources:Resource,wfb2dl_excuteYear%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_Year" runat="server" MaxLength="4" Width="80px" class="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_excuteYear_isNull%>"
                                            ControlToValidate="txt_Year" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_excuteYear_Error%>"
                                            ClientValidationFunction="checkYear" ForeColor="Red" ControlToValidate="txt_Year" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                    <%--部門代號.部門名稱--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO_search" runat="server" Text="<%$Resources:Resource,wfb2dl_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="65px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" MaxLength="60" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--員工區分--%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_EMP_CD_search" runat="server" Text="<%$Resources:Resource,wfb2dl_EMP_CD%>"></asp:Label>:
                                </th>
                                <td>
                                    <asp:DropDownList ID="ddl_EMP_CD_seaarch" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                </td>
                                <%--工號--%>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_EMP_ID_search" runat="server" Text="<%$Resources:Resource,wfb2dl_EMP_ID%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_EMP_ID_search" runat="server" MaxLength="5" Width="65px" ClientIDMode="Static"></asp:TextBox>
                                    <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_qry_EMP_NAME', 'N');" />
                                    <asp:TextBox ID="txt_qry_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                </td>
                                
                                <tr>
                                    <%--入社日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_JOIN_DT" runat="server" Text="<%$Resources:Resource,wfb2sa_JOIN_DT1%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_JOIN_SDT" runat="server" ClientIDMode="Static" CssClass="date" Width="80px"></asp:TextBox>~
                                        <asp:TextBox ID="txt_JOIN_EDT" runat="server" ClientIDMode="Static" CssClass="date" Width="80px"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidatorJOIN_SDT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sa_error_JOIN_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_JOIN_SDT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidatorJOIN_EDT" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sa_error_JOIN_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_JOIN_EDT" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_JOIN_SDT"
                                            ControlToValidate="txt_JOIN_EDT" ErrorMessage="<%$Resources:Resource,wfb2sa_greaterthan_JOIN_DT%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="Body_label" colspan="6">
                                        <aces:btn ID="WFB2DL0103ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2dl_excelImport%>" OnClick="WFB2DL0103ExcelDown_Click" OnClientClick="return checkDowning(this.value);" ValidationGroup="GroupA" />
                                         <%--<asp:Button ID="WFB2DL0103ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2dl_excelImport%>" OnClick="WFB2DL0103ExcelDown_Click" OnClientClick="return MycheckDowning(this.value);" ValidationGroup="GroupA" />--%>
                                        <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,btn_back%>" OnClick="btn_back_Click" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Body_label"></td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DL0103ExcelDown" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidconfirm_Execute" Value="<%$Resources:Resource,confirm_Execute%>" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>
