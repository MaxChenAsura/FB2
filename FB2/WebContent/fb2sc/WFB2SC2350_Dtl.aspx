<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2350_Dtl.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2350_Dtl" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $('.ym').mask('9999/99');
         
            $.unblockUI();
        }
        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }

        function doUnBlock() {
            $.unblockUI();
        }

        function checkConfirm() {
            return confirm($('#hidwfb2sc_cancel_ConfirmMessage').val());
        }

        function CheckExec() {
            var processed = true;
            var txtmsg = "";

            if ($("#ddl_SALARY_PAY_METHOD").val() == "") {
                txtmsg = txtmsg + "發放方式不可空白\n\r";
                processed =  false;
            }
            if ($("#txt_EMP_ID_AREA").val() == "") {
                txtmsg = txtmsg + "指定工號不可空白\n\r";
                processed = false;
            }
            if (!processed) {
                alert(txtmsg);
                return false;
            }
   
            processed = confirm($('#hidwfb2sc_Execute_ConfirmMessage').val());
            BlockUI();

            if (!processed)
                $.unblockUI();

            return processed;
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="35%" />
                                <col width="15%" />
                                <col width="35%" />
                            </colgroup>
                            <tbody>
                                <!-- START: 1st Line in Search Criteria area -->
                                <tr>
                                    <!-- START: Create MODULE ID -->
                                    <%--發薪類別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_SALARY_TYPE_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                        <asp:Label ID="lb_SALARY_TYPE_NAME_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <%--發薪日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_SALARY_DT_txt" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--薪資項目--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAY_KIND" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PAY_KIND%>"></asp:Label>
                                    </th>
                                    <td align="left" class="Body_label" colspan ="3">
                                        <asp:Label ID="lb_PAY_KIND_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                        <asp:Label ID="lb_PAY_KIND_NAME_txt" runat="server" ClientIDMode="Static"></asp:Label>
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


            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table width="100%">
                            <%-- IFLOW顯示 --%>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_SALARY_PAY_METHOD" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_PAY_METHOD%>"></asp:Label>:
                                </th>
                                <td align="left" class="Body_label">
                                    <asp:DropDownList ID="ddl_SALARY_PAY_METHOD" runat="server" class="MandatoryField" ClientIDMode="Static" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_required_ddl_SALARY_PAY_METHOD%>"
                                         ControlToValidate="ddl_SALARY_PAY_METHOD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_EMP_ID_AREA" runat="server" Text='<%$Resources:Resource,wfb2sc_direct_emp_id%>' ClientIDMode="Static" />
                                </th>
                                <td align="left" class="Body_label" colspan="5">
                                    <asp:TextBox ID="txt_EMP_ID_AREA" runat="server" class="MandatoryField" ClientIDMode="Static" Width="98%" MaxLength="210" Rows="10" TextMode="MultiLine"/>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_required_direct_emp_id%>"
                                         ControlToValidate="txt_EMP_ID_AREA" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="Body_label" colspan="2">
                                    <p><b>輸入方式:工號1,工號2,工號3...</b></p>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="Body_label" colspan="6">
                                    <div id="init_grid" style="margin-bottom: 7px;">
                                        <%-- 確定 --%>
                                        <%--<aces:Btn   ID="WFB2SC2350OK" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClientClick="return CheckExec();" OnClick="WFB2SC2350OK_Click" ValidationGroup="GroupA" />--%>
                                        <asp:Button ID="WFB2SC2350OK" runat="server" Text="<%$Resources:Resource,btn_ok%>" OnClientClick="return CheckExec();" OnClick="WFB2SC2350OK_Click"   />
                                        <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,btn_back%>"  OnClientClick="return checkConfirm();" OnClick="btn_back_Click"/>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <asp:HiddenField ID="hid_salary_dt" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_salary_type" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_pay_kind" runat="server" ClientIDMode="Static" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Execute_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Execute_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_cancel_ConfirmMessage"  Value="<%$Resources:Resource,wfb2sc_Cancel_Confirm%>" />
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


