<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sp/WFB2SP0300_Qry.aspx.cs" Inherits="WebContent_WFB2SP0300_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        $(function () {

            iniForm();
        });
        function iniForm() {
            //日期格式心須
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $(".date").mask('9999/99');
            //GridView必須
            $.unblockUI();
            //工號取得姓名的a
        }

        function doUnblock() {
            $.unblockUI();
        }
        function doBlock() {
            BlockUI();
        }

        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }


        //資料下載
        function checkDowning() {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else {
                processed = false;
                return false;
            }
            if (processed) {
                processed = confirm("確定要進行" + msg + "?");
                BlockUI();
            }

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
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="35%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="15%" />
                    <col width="10%" />

                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--退休月份--%>
                            <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_retire_ym%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_RETIRE_YM" runat="server" Width="60px" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="qry1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sp_required_retire_ym%>"
                                ControlToValidate="txt_RETIRE_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2sp_error_retire_ym%>" ControlToValidate="txt_RETIRE_YM" ForeColor="Red"
                                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                        </td>
                        <td colspan="4"></td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--免稅額備註--%>
                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_remark%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="5">
                            <asp:TextBox ID="txt_REMARK" runat="server" Width="600px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sp_required_remark%>"
                                ControlToValidate="txt_REMARK" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="6">
                            <aces:Btn ID="WFB2SP0300ExcelDown" runat="server" Text="列印" OnClick="WFB2SP0300ExcelDown_Click" OnClientClick="return checkDowning();"   />
                            <%-- 
                            <asp:Button ID="WFB2SP0300ExcelDown" runat="server" Text="列印" OnClick="WFB2SP0300ExcelDown_Click" OnClientClick="return checkDowning();"   />
                            --%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                </tbody>
            </table>

            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

        <%--EXCEL下載用 --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SP0300ExcelDown" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
