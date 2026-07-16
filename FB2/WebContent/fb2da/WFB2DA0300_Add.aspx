<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2da/WFB2DA0300_Add.aspx.cs" Inherits="WebContent_fb2da_WFB2DA0300_Add" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $("#txt_CALENDAR_DT").mask("9999/99/99");
            $.unblockUI();
        }
        　
        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }

        function CheckPARTS_NO(source, arguments) {
            var re = /^[\d|a-zA-Z|-]+$/;
            if ($("#txt_PARTS_NO").val().trim() != "") {
                if (!re.test($("#txt_PARTS_NO").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="20%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="40%" />
                            </colgroup>
                            <tbody>                                 
                                <tr>
                                    <%--行事曆代碼--%>
                                    <th align="left" class="Body_TableHeader">                                       
                                        <asp:Label ID="lb_CALENDAR_CD" runat="server" Text="<%$Resources:Resource,wfb2da_lb_CALENDAR_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">                                        
                                        <asp:DropDownList ID="ddl_CALENDAR_CD" CssClass="MandatoryField" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="ddl_CALENDAR_CD_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                         
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2da_CALENDAR_CD%>"
                                            ControlToValidate="ddl_CALENDAR_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--日期--%>
                                    <th align="left" class="Body_TableHeader">                                       
                                        <asp:Label ID="lb_CALENDAR_DT" runat="server" Text="<%$Resources:Resource,wfb2da_lb_CALENDAR_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">                                        
                                        <asp:TextBox ID="txt_CALENDAR_DT" MaxLength="12" runat="server" CssClass="MandatoryField date" ClientIDMode="Static" OnTextChanged="txt_CALENDAR_DT_TextChanged" AutoPostBack="true"></asp:TextBox>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2da_CALENDAR_DT%>"
                                            ControlToValidate="txt_CALENDAR_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--日期類型(原)--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DT_TYPE_O" runat="server" Text="<%$Resources:Resource,wfb2da_lb_DT_TYPE_O%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DT_TYPE_O" MaxLength="1" Width="60px"  runat="server" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--日期類型(新)--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DT_TYPE_N" runat="server" Text="<%$Resources:Resource,wfb2da_DT_TYPE_N%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_DT_TYPE_N" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2da_DT_TYPE_N%>"
                                            ControlToValidate="ddl_DT_TYPE_N" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td></td>
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

                <tr>
                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <aces:Btn ID="WFF2DA0301SAVE" runat="server" Text="儲存" Visible="true" OnClick="WFF2DA0301Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA"/>
                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,btn_cancel%>" Visible="true" OnClick="btn_cancel_Click" OnClientClick="return confirm($('#hidwfb2da_Cancel_ConfirmMessage').val());" />
                        </div>
                    </td>
                </tr>
            </table>

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Cancel_ConfirmMessage" Value="<%$Resources:Resource,Cancel_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>        
    </asp:UpdatePanel>
</asp:Content>

