<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sd/WFB2SD1200_Qry.aspx.cs" Inherits="WebContent_fb2sd_WFB2SD1200_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".number").formatNumber({ format: "#,###", locale: "tw" });
            $("#txt_SALARY_NAME").attr("readonly", true);

            $.unblockUI();
        }
        //執行檢核
        function executeCheck() {
            var processed = false;
            BlockUI();
            if (Page_ClientValidate("GroupA") == false) {
                $.unblockUI();
                return false;
            }

            $.ajax({
                url: "WFB2SD1200_ExecuteCheck.ashx",
                data: {
                    remit_dt: $('#txt_REMIT_DT').val(),
                    pay_kind: $('#txt_PAY_KIND').val()
                    , salary_account_bank: $('#ddl_SALARY_ACCOUNT_BANK').val()
                    , pay_id: $('#txt_PAY_ID').val()
                },
                type: "GET",
                async: false,  //注意!!
                dataType: 'json',
                success: function (JData) {
                    if (JData.errMsg != "0") {
                        if (JData.errMsg == "此查詢資料,已轉出媒體檔,是否重新匯出媒體檔。")
                        {
                            if (confirm(JData.errMsg))
                                processed = true;
                            else
                                processed = false;
                        }
                            
                        else
                        {
                            alert(JData.errMsg);
                            processed = false;
                        }
                    } else {
                        processed = true;
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                    processed = false;
                }
            });
            if (processed == false)
                $.unblockUI();
            return processed;

        }

        //清空
        function doClear() {
            $("#txt_REMIT_DT").val("");
            $("#txt_PAY_KIND").val("");
            $("#txt_SALARY_NAME").val("");
            $("#ddl_TRANSCLASS").val("1")
            $("#txt_PAY_ID").val("");
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--頁面table-->
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="20%" />
                                <col width="30%" />
                                <col width="15%" />
                                <col width="35%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--關帳代號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAY_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PAY_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PAY_ID" runat="server" MaxLength="11" Width="100px" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                         <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="請輸入關帳代號"
                                            ControlToValidate="txt_PAY_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="關帳代號輸入格式錯誤(英數字和'-' 組成)"
                                            ControlToValidate="txt_PAY_ID" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="^[\d|a-zA-Z-]+$" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMIT_DT" runat="server" Text="<%$Resources:Resource,wfb2_lb_REMIT_DT%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_REMIT_DT" runat="server" MaxLength="6" Width="80px" CssClass="MandatoryField date" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2_msg_REMIT_DT_NotEmpty%>"
                                            ControlToValidate="txt_REMIT_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2_msg_REMIT_DT_FormatError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_REMIT_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_ID%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_PAY_KIND" runat="server" MaxLength="4" Width="64px" ClientIDMode="Static" CssClass="MandatoryField" OnTextChanged="txt_PAY_KIND_TextChanged" AutoPostBack="true"></asp:TextBox>
                                        <input id="bt_PAY_KIND_SEARCH" type="button" value="..." onclick="OpenSearch('Salary9999_Search.aspx', 'txt_PAY_KIND', 'txt_SALARY_NAME', '', '');" />
                                        <asp:TextBox ID="txt_SALARY_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_msg_salary_id_NotEmpty%>"
                                            ControlToValidate="txt_PAY_KIND" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2_lb_TRANSCLASS%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddl_TRANSCLASS" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SALARY_ACCOUNT_BANK%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddl_SALARY_ACCOUNT_BANK" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <%--<asp:DropDownList ID="ddl_SALARY_ACCOUNT_BANK" runat="server" ClientIDMode="Static" OnTextChanged="ddl_SALARY_ACCOUNT_BANK_Changed" AutoPostBack="true" CssClass="MandatoryField"></asp:DropDownList>--%>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label4" runat="server" Text="自訂附言 (上限7個中文)"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_REMARK" runat="server" MaxLength="7" Width="160px" ClientIDMode="Static" Text=""></asp:TextBox>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" colspan="2">
                                        <p style="color: red;font-size:20px">
                                            <asp:Label ID="lb_notice" runat="server" Text="PS 轉帳組別-台企使用，自訂附言-中信使用"></asp:Label>
                                        </p>
                                    </th>
                                </tr>
                                <tr>
                                    <th></th>
                                    <td></td>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SD1200Txt" runat="server" Text="<%$Resources:Resource,wfb2_btn_WFB2SD1200Txt%>" ClientIDMode="Static" OnClick="WFB2SD1200Txt_Click" OnClientClick="return executeCheck();" />

                                            <%--<asp:Button ID="WFB2SD1200Txt" runat="server" Text="<%$Resources:Resource,wfb2_btn_WFB2SD1200Txt%>" ClientIDMode="Static" OnClick="WFB2SD1200Txt_Click" OnClientClick="return executeCheck();" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sk_clear%>" OnClientClick="return doClear();" />
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
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SD1200Txt" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
