<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dm/action/WFB2DM0100_Qry.aspx.cs" Inherits="WebContent_fb2dm_WFB2DM0100_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date2").datepicker({ dateFormat: 'yy/mm' });
            $('.date2').mask('9999/99');

            $("#txt_SALARY_DT").attr("readonly", true);
            $("#txt_DUTY_DT").attr("readonly", true);

            $.unblockUI();
        }
        function ClearAll() {
            $("#txt_SALARY_YM").val(getLastMonth_1());
            $("#txt_SALARY_DT").val("");
            $("#txt_DUTY_DT").val("");
            $("#ddl_CFN_FLAG1").val("N");
            $("#ddl_CFN_FLAG2").val("N");
        }

        function getLastMonth_1() {
            var day = new Date();
            var year = day.getFullYear();
            var month = day.getMonth();
            var date = day.getDate();
            month = (month < 10) ? "0" + month : month;
            date = (date < 10) ? "0" + date : date;
            var todayDate = year + "/" + month;
            return todayDate;
        }

        function checkconfirm(checkconfirmmessage) {
            var ret = confirm(checkconfirmmessage);
            if (ret) {
                BlockUI();
                __doPostBack('execute', '');

            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="16%" />
                    <col width="20%" />
                    <col width="16%" />
                    <col width="48%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2dm_lb_SALARY_YM%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="8">
                            <asp:TextBox ID="txt_SALARY_YM" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date2" OnTextChanged="txt_SALARY_YM_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dm_lb_SALARY_YM_isError%>"
                                ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dm_Required_SALARY_YM%>"
                                ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2dm_lb_SALARY_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_SALARY_DT" runat="server" Width="100px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_lb_DUTY_DT" runat="server" Text="<%$Resources:Resource,wfb2dm_lb_DUTY_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DUTY_DT" runat="server" Width="200px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                            <asp:HiddenField ID="hid_DUTY_SDT" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hid_DUTY_EDT" runat="server" ClientIDMode="Static" />
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CFN_FLAG1" runat="server" Text="<%$Resources:Resource,wfb2dm_lb_CFN_FLAG1%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_CFN_FLAG1" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static">
                                <asp:ListItem Text="<%$Resources:Resource,wfb2dm_CFN_FLAG2_Y%>" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource,wfb2dm_CFN_FLAG2_N%>" Value="N" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_CFN_FLAG2" runat="server" Text="<%$Resources:Resource,wfb2dm_lb_CFN_FLAG2%>"></asp:Label>:</th>

                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_CFN_FLAG2" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static">
                                <asp:ListItem Text="<%$Resources:Resource,wfb2dm_CFN_FLAG2_Y%>" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource,wfb2dm_CFN_FLAG2_N%>" Value="N" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                     <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dm_lb_CFN_FLAG3%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_CFN_FLAG3" runat="server" DropDownHeight="100px" EnableVirtualScrolling="true" ClientIDMode="Static">
                                <asp:ListItem Text="<%$Resources:Resource,wfb2dm_CFN_FLAG2_Y%>" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="<%$Resources:Resource,wfb2dm_CFN_FLAG2_N%>" Value="N" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="Body_label"></td>
                        <td align="right" class="Body_label" colspan="8">
                            <aces:Btn ID="WFB2DM0100Exec" runat="server" Text="<%$Resources:Resource,wfb2dm_WFB2DM0100Exec%>" OnClick="WFB2DM0100Exec_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                            <%--<asp:Button ID="WFB2DM0100Exec" runat="server" Text="<%$Resources:Resource,wfb2dm_WFB2DM0100Exec%>" OnClick="WFB2DM0100Exec_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA"/>--%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dm_WFB2DM0100Clear%>" onclick="ClearAll();" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="1" colspan="20">
                            <hr />
                        </td>
                    </tr>

                    <!-- end: Create MODULE ID -->
                </tbody>
            </table>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
