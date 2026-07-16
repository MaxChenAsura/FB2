<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/ff0me/WFF0ME0110_Qry.aspx.cs" Inherits="WebContent_WFF0ME0110_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        $(function () {
            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $(".ym").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $.unblockUI();
        }

        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }

        //查詢前檢核
        function CheckSearch(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                if (checkDowning(msg))
                    BlockUI();
                else
                    processed = false;
            }
            else {
                processed = false;
            }
            if (!processed)
                $.unblockUI();

            return processed;
        }

      

        //清空畫面
        function ClearAll() {
            $("#txt_BILL_YM").val("");
            //$("#txt_ACCOUNT_TRM").val("");
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
                        <th align="left" class="Body_TableHeader">
                            <%--傳票生成年月--%>
                            <asp:Label ID="Label9" runat="server" Text="年月"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_BILL_YM" runat="server" Width="80px"  ClientIDMode="Static"  CssClass="MandatoryField ym"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="年月不可空白"
                            ControlToValidate="txt_BILL_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                            ErrorMessage="年月格式錯誤" ControlToValidate="txt_BILL_YM" ForeColor="Red" ValidationGroup="GroupA"
                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                      <%-- 棄用回數 
                     <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label1" runat="server" Text="回數"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_ACCOUNT_TRM" runat="server" Width="80px"  MaxLength="2" ClientIDMode="Static"  CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="回數不可空白"
                            ControlToValidate="txt_ACCOUNT_TRM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    --%>
                    <tr>
                     <%-- 下載條件 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_TRANS_FLAG" runat="server" Text="發票資料下載條件"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_TRANS_FLAG" runat="server" Width="200px" ClientIDMode="Static" CssClass="MandatoryField">
                                <asp:ListItem Value="N">未轉出</asp:ListItem>
                                <asp:ListItem Value="Y">已轉出</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="6">
                            <asp:Button ID="WFF0ME0110EXECUTE" runat="server" Text="執行" OnClick="WFF0ME0110EXECUTE_Click" OnClientClick="return CheckSearch(this.value);" />                           
                             <asp:Button ID="WFF0ME0110DOWNLOAD" runat="server" Text="發票資料下載" OnClick="WFF0ME0110DOWNLOAD_Click" OnClientClick="return CheckSearch(this.value);" />                             
                            <input id="btn_clear" runat="server" type="button" value="清除" onclick="ClearAll();" />
                        </td>
                    </tr>
                </tbody>
            </table>

            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
        <%--EXCEL下載用 --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFF0ME0110EXECUTE" />
            <asp:PostBackTrigger ControlID="WFF0ME0110DOWNLOAD" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
