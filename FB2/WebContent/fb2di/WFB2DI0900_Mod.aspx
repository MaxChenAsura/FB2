<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0900_Mod.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0900_Mod" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $('.date').datepicker({ dateFormat: 'yy/mm/dd' });
            $('#txt_START_DT').mask('9999/99/99');
            $('#txt_END_DT').mask('9999/99/99');
            $.unblockUI();
        }

        function checkConfirm() {
            var result = confirm("是否確定取消?");
            if (result)
                window.location.href("WFB2DI0900_Qry.aspx");
        }

        function openQry() {
            window.location.href("WFB2DI0900_Qry.aspx?reload=Y");
            return false;
        }

        function cheakTime(source, arguments) {
            var timeS = $('#txt_START_DT').val() + $('#ddl_STIME').val() + $('#ddl_STIME2').val();
            var timeE = $('#txt_END_DT').val() + $('#ddl_ETIME').val() + $('#ddl_ETIME2').val();
            timeS.replace(/\//g, "");
            timeE.replace(/\//g, "");
            if (timeS > timeE)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="30%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--開始日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_START_DT2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_START_DT" runat="server" ClientIDMode="Static" Width="81px" CssClass="date MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_START_DT2%>"
                                            ControlToValidate="txt_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>

                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_lb_START_DT2_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                    <%--開始時間--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_STIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_STIME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_STIME" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>:
                                        <asp:DropDownList ID="ddl_STIME2" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_STIME%>"
                                            ControlToValidate="ddl_STIME" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_STIME2%>"
                                            ControlToValidate="ddl_STIME2" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>

                                    </td>

                                </tr>
                                <tr>
                                    <%--結束日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_END_DT" runat="server" Text="<%$Resources:Resource,wfb2di_lb_END_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_END_DT" runat="server" ClientIDMode="Static" Width="81px" CssClass="date MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_END_DT%>"
                                            ControlToValidate="txt_END_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>

                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_lb_END_DT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>


                                    </td>
                                    <%--結束時間--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ETIME" runat="server" Text="<%$Resources:Resource,wfb2di_lb_ETIME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_ETIME" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>:
                                        <asp:DropDownList ID="ddl_ETIME2" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_ETIME%>"
                                            ControlToValidate="ddl_ETIME" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_ETIME2%>"
                                            ControlToValidate="ddl_ETIME2" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator3" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_lb_SDT_EDT%>" ClientValidationFunction="cheakTime" ForeColor="Red"
                                            ControlToValidate="ddl_ETIME2" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--備註--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2di_lb_REMARK%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_REMARK" runat="server" Width="300px" MaxLength="210"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DI0900Save" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0900Save%>" ValidationGroup="GroupA" OnClick="WFB2DI0900Save_Click" />
                                            <asp:Button ID="btn_Cancel" runat="server" Text="<%$Resources:Resource,wfb2di_btn_Cancel%>" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_Cancel_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

            </table>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

