<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dl/WFB2DL0500_Add.aspx.cs" Inherits="WebContent_wfb2hd_WFB2DL0500_Add" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            initForm();
        }); 
        function initForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');

            $.unblockUI();
            

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

        function checkConfirm() {
            return confirm($('#hidwfb299_Cancel_ConfirmMessage').val());
        }
         
        

    </script>
    <style type="text/css">
        .auto-style1 {
            height: 25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DL_GEN_DESC" runat="server" Text="說明"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:TextBox ID="txt_DL_GEN_DESC" runat="server" MaxLength="80" ClientIDMode="Static" Width="700px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PROC_CD" runat="server" Text="作業碼"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_PROC_CD" CssClass="MandatoryField" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="請選擇作業碼"
                                            ControlToValidate="ddl_PROC_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SDT_CD" runat="server" Text="起始碼"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SDT_CD" CssClass="MandatoryField" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="請選擇起始碼"
                                            ControlToValidate="ddl_SDT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EDT_CD" runat="server" Text="結束碼"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_EDT_CD" CssClass="MandatoryField" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="請選擇結束碼"
                                            ControlToValidate="ddl_EDT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LOGI_CD" runat="server" Text="邏輯碼"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_LOGI_CD" CssClass="MandatoryField" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="請選擇邏輯碼"
                                            ControlToValidate="ddl_LOGI_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DL_GENDT_CD" runat="server" Text="特休生成日碼"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_DL_GENDT_CD" CssClass="MandatoryField" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="請選擇特休生成日碼"
                                            ControlToValidate="ddl_DL_GENDT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                  <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_D01_SAME" runat="server" Text="當年度復職"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_D01_SAME" CssClass="MandatoryField" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="請選擇當年度復職"
                                            ControlToValidate="ddl_IS_D01_SAME" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>                              
                                <tr>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="5">
                                        <aces:Btn ID="WFB2DL0500Save" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2DL0500Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />
                                        <asp:Button ID="btn_cancel" runat="server" OnClick="WFB2DL0500Cancel_Click" OnClientClick="checkConfirm();"  Text="<%$Resources:Resource,btn_Cancel%>"  ClientIDMode="Static"/>
                                    </td>
                                </tr>
                                <!-- end: Create MODULE ID -->
                                <!-- START: Create a line to separate Search field with body field -->
                                <tr>
                                    <td align="center" height="1" colspan="6">
                                        <hr>
                                    </td>
                                </tr>
                                <!-- END: Create a line -->
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Body_label"></td>
                </tr>
            </table>
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="WFB2DL0500Save" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
