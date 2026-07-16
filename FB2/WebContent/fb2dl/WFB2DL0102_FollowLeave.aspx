<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dl/WFB2DL0102_FollowLeave.aspx.cs" Inherits="WebContent_fb2dl_WFB2DL0102_FollowLeave" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            initForm();
        });
        function initForm() {
            $.unblockUI();
            $('#txt_Year').mask('9999');
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="90%" />
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
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--子假別--%>
                                        <asp:Label ID="Label2" runat="server" Text="生成子假別" ></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:DropDownList ID="ddl_SUB_LEAVE_CD" runat="server"  ClientIDMode="Static" class="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="需挑選子假別!"
                                            ControlToValidate="ddl_SUB_LEAVE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                <tr>
                    <td align="right" class="Body_label" colspan="6">
                        <aces:Btn ID="WFB2DL0102Save" runat="server" Text="<%$Resources:Resource,btn_Execute%>" OnClick="WFB2DL0102Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                        <%--<asp:Button ID="WFB2DL0102Save" runat="server" Text="<%$Resources:Resource,btn_Execute%>" OnClick="WFB2DL0102Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
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
            <asp:AsyncPostBackTrigger ControlID="WFB2DL0102Save" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>
