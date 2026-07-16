<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/ff0me/WFF0ME0210_Qry.aspx.cs" Inherits="WebContent_WFF0ME0210_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        $(function () {
            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $('.date').mask('9999/99');
            $.unblockUI();
        }

        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }

        //查詢前檢核
        function CheckSearch() {
            var processed = true;

            if (Page_ClientValidate("GroupA") && Page_ClientValidate("GroupB")) {
                BlockUI();
            }
            else {
                processed = false;
            }

            if (!processed)
                $.unblockUI();

            return processed;
        }

        //下載前檢核
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


        function checkUpload(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                //BlockUI();
            }
            else {
                processed = false;
                return false;
            }

            BlockUI();
            processed = confirm("確定要進行" + msg+"?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
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
                            <%--發票年月--%>
                            <asp:Label ID="Label9" runat="server" Text="年月"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_BILL_YM" runat="server" Width="80px"  ClientIDMode="Static"  CssClass="MandatoryField date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="qry1" runat="server" ErrorMessage="年月不可空白"
                                ControlToValidate="txt_BILL_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <!--驗證長度需為4 -->
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                ErrorMessage="年月須為6碼" ControlToValidate="txt_BILL_YM" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression=".{7,7}" Display="None"></asp:RegularExpressionValidator>
                            <!--驗證年月格式-->
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ErrorMessage="年月格式錯誤" ControlToValidate="txt_BILL_YM" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>	
                        </td>
                    </tr>
                     <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--廠商/工區--%>
                            <asp:Label ID="Label1" runat="server" Text="廠商/工區"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_VENDOR_ID" runat="server" Width="80px"  MaxLength="4" ClientIDMode="Static"  CssClass="MandatoryField"></asp:TextBox>-
                            <asp:TextBox ID="txt_VENDOR_AREA" runat="server" Width="40px"  MaxLength="1" ClientIDMode="Static"  CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="廠商不可空白"
                            ControlToValidate="txt_VENDOR_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="工區不可空白"
                            ControlToValidate="txt_VENDOR_AREA" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                          <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--發票識別--%>
                            <asp:Label ID="Label2" runat="server" Text="發票識別"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_SAP_INV_FLAG" runat="server" Width="40px"  MaxLength="1" ClientIDMode="Static"  CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="發票識別不可空白"
                            ControlToValidate="txt_SAP_INV_FLAG" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <th></th>
                        <td align="right" colspan="6">
                            <asp:Button ID="WFF0ME0210EXECUTE" runat="server" Text="執行" OnClick="WFF0ME0210EXECUTE_Click" OnClientClick="return CheckSearch(this.value);" />                           
                            <asp:Button ID="WFF0ME0210DOWNLOAD" runat="server" Text="(未轉出)資料下載" OnClick="WFF0ME0210DOWNLOAD_Click" OnClientClick="return CheckSearch(this.value);" />                             
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
            <asp:PostBackTrigger ControlID="WFF0ME0210DOWNLOAD" />
            <asp:PostBackTrigger ControlID="WFF0ME0210EXECUTE" />           
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
