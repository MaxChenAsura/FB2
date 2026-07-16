<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/ff0me/WFF0ME0310_Qry.aspx.cs" Inherits="WebContent_WFF0ME0310_Qry" Culture="auto" UICulture="auto" %>

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

        //若為FI,則要有回數
        function CheckTRM(source, arguments) {            
            var trans_ds = arguments.Value;
            var trm = $("#txt_ACCOUNT_TRM").val();

            if (trans_ds == "FI" && $("#txt_ACCOUNT_TRM").val() == "") {
                return false;
            }

            return true;
        }

        function CheckIsNaN(source, arguments) {
            var value = arguments.Value;
            if (isNaN(value)) {
                arguments.IsValid = false;
            }
            else {
                arguments.IsValid = true;
            }
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
                     <%--
                     <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label1" runat="server" Text="回數"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_ACCOUNT_TRM" runat="server" Width="80px"  MaxLength="2" ClientIDMode="Static"   ></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true" 
                             ErrorMessage="FI回數不可空白" ClientValidationFunction="CheckTRM" ForeColor="Red"
                            ControlToValidate="txt_ACCOUNT_TRM" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                            ErrorMessage="回數只能數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                            ControlToValidate="txt_ACCOUNT_TRM" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </td>
                    </tr>                              
                    <tr>
                        --%>

                     <%-- 執行SP條件 --%>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_TRANS_FLAG" runat="server" Text="發票類型"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_TRANS_DS" runat="server" Width="120px" ClientIDMode="Static" CssClass="MandatoryField" ValidationGroup="GroupA">                            
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="發票類型不可空白"
                            ControlToValidate="ddl_TRANS_DS" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>

                        </td>
                    </tr>      
                    <tr>
                        <th></th>
                        <td align="right" colspan="6">
                            <asp:Button ID="WFF0ME0310EXECUTE" runat="server" Text="執行" OnClick="WFF0ME0310EXECUTE_Click" OnClientClick="return CheckSearch(this.value);" />                           
                            <input id="btn_clear" runat="server" type="button" value="清除" onclick="ClearAll();" />
                        </td>
                    </tr>
                </tbody>
            </table>

            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
