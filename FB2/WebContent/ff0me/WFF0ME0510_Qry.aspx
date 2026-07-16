<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/ff0me/WFF0ME0510_Qry.aspx.cs" Inherits="WebContent_WFF0ME0510_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        $(function () {
            iniForm();
        });
        function iniForm() {
            $.unblockUI();
        }

        //查詢前檢核
        function CheckSearch(msg) {
            var processed = true;
            if (checkDowning(msg))
                BlockUI();
            else
                processed = false;

            if (!processed)
                $.unblockUI();

            return processed;
        }



        //清空畫面
        function ClearAll() {
        }

    </script>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="60%" />
                    <col width="30%" />
                </colgroup>
                <tbody>                    
                    <tr>
                        <th></th>
                        <td align="center" colspan="6">
                            <asp:Button ID="WFF0ME0510EXECUTE" runat="server" Text="中介價格轉SAP" Font-Size="80px" OnClick="WFF0ME0510EXECUTE_Click" OnClientClick="return CheckSearch(this.value);" />
                        </td>
                    </tr>
                </tbody>
            </table>

            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
