<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ia/action/WFB2IA4200_Qry.aspx.cs" Inherits="WebContent_fb2ia_WFB2IA4200_Qry" Culture="auto" UICulture="auto"%>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCCommCodeDropDwonList.ascx" TagPrefix="uc1" TagName="UCCommCodeDropDwonList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $('.ymd').mask('9999/99/99');
            $.unblockUI();
            $('#txt_SALARY_DT').attr("readonly", true);
        }
        
        function CheckSALARY_DT(source, arguments) {
            var re = /^(19|20)\d\d[/ /.](0[1-9]|1[012])+$/;
            if ($("#txt_SALARY_YM").val() != "" && re.test($("#txt_SALARY_YM").val()) && $("#txt_SALARY_DT").val() == "")
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        function Checkhid_process_status(source, arguments) {
            var re = /^(19|20)\d\d[/ /.](0[1-9]|1[012])+$/;
            if ($("#txt_SALARY_DT").val() != ""){
                if ($("#txt_SALARY_YM").val() != "" && re.test($("#txt_SALARY_YM").val()) && $("#hid_process_status").val() != "1")
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
            else
            {
                arguments.IsValid = true;
            }
        }

        function INS_TYPE_Choose() {
            $("#HID_INS_TYPE").val($("#ddl_INS_TYPE").val());
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--頁面table-->
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="30%" />
                                <col width="15%" />
                                <col width="60%" />
                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2ia_SALARY_YM3%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_SALARY_YM" runat="server" MaxLength="6" Width="64px" CssClass="MandatoryField ym date" ClientIDMode="Static" Text="" OnTextChanged="txt_SALARY_YM_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sk_required_SALARY_YM3%>"
                                            ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None" ClientIDMode="Static" ></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_SALARY_YM3%>" ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ClientIDMode="Static"></asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_INS_TYPE" runat="server" Text="<%$Resources:Resource,wfb2ia_INS_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_INS_TYPE" runat="server" ClientIDMode="Static" OnChange="INS_TYPE_Choose();" CssClass="MandatoryField">
                                            <asp:ListItem Value="0" Text="<%$Resources:Resource,wfb2ia_INS_TYPE_0%>" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="A" Text="<%$Resources:Resource,wfb2ia_INS_TYPE_A%>"></asp:ListItem>
                                            <asp:ListItem Value="B" Text="<%$Resources:Resource,wfb2ia_INS_TYPE_B%>"></asp:ListItem>
                                            <asp:ListItem Value="C" Text="<%$Resources:Resource,wfb2ia_INS_TYPE_C%>"></asp:ListItem>
                                            <asp:ListItem Value="D" Text="<%$Resources:Resource,wfb2ia_INS_TYPE_D%>"></asp:ListItem>
                                            <asp:ListItem Value="E" Text="<%$Resources:Resource,wfb2ia_INS_TYPE_E%>"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2ia_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_SALARY_DT" runat="server" MaxLength="8" Width="80px" CssClass="ymd" ClientIDMode="Static" Text="" ></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_required_SALARY_DT%>" ClientValidationFunction="CheckSALARY_DT" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:HiddenField ID="hid_process_status" ClientIDMode="Static" runat="server" />
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_error_hid_process_status%>" ClientValidationFunction="Checkhid_process_status" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2IA4200Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA4200Process%>" OnClick="WFB2IA4200Process_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />

                                            <%--<asp:Button ID="WFB2IA4200Process" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA4200Process%>" OnClick="WFB2IA4200Process_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>

                        </table>
                    </td>
                </tr>

            </table>

            <asp:HiddenField ID="HID_INS_TYPE" runat="server" ClientIDMode="Static" Value="0" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2sk_required_SALARY_YM3" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_required_SALARY_YM3%>" />
            <asp:HiddenField ID="hid_wfb2ia_error_SALARY_YM3" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2ia_error_SALARY_YM3%>" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
