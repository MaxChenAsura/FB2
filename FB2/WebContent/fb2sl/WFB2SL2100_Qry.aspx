<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sl/action/WFB2SL2100_Qry.aspx.cs" Inherits="WebContent_fb2sl_WFB2SL2100_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $('.ym').mask('9999/99');
            $('.year').mask('9999');

            $(".number").formatNumber({ format: "#,###", locale: "tw" });
            $.unblockUI();
        }
        function getYear() {
            var datetime = new Date();
            var year = datetime.getFullYear();
            $('#txt_DATA_YM_search').val(year - 1);
        }
        function getSALARY_DT() {
            if ($("#txt_DATA_YM_search").val() != "") {
                if ($("#txt_DATA_YM_search").val() < 1911) {
                    alert('年度輸入錯誤');
                    $("#txt_DATA_YM_search").val("");
                }
                else {
                    var year = $('#txt_DATA_YM_search').val();
                    if (year != "") {
                        $('#txt_SALARY_DT').val(year + "/01/01  ~ "+ year + "/12/31");
                    }
                }
            }
        }
        function doUnBlock() {
            $.unblockUI();
        }
        function MycheckDowning(msg) {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return;
            BlockUI();
            processed = confirm("確定要進行" + msg + "?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
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
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="50%" />
                            </colgroup>
                            <tbody>

                                <tr>
                                    <%--年度--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YM_search" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_DATA_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DATA_YM_search" runat="server" MaxLength="4" Width="64px" onblur="getSALARY_DT();" class="MandatoryField year" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_Required_DATA_YM%>"
                                            ControlToValidate="txt_DATA_YM_search" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                         <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sl_lb_DATA_YM_isError%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_DATA_YM_search" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                    <%--發薪期間--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT_search" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_DT" runat="server" ClientIDMode="Static" CssClass="txtDisabled" Enabled="false" Width="160px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SL2100Generate" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM210Generate%>" ClientIDMode="Static" OnClick="WFB2SL2100Generate_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <aces:Btn ID="WFB2SL2100Export" runat="server" Text="<%$Resources:Resource,wfb2sk_TextDown%>" ClientIDMode="Static" OnClick="WFB2SL2100Export_Click" ValidationGroup="GroupA" OnClientClick="return checkDowning(this.value);"/>

                                            <%--<asp:Button ID="WFB2SL2100Generate" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM210Generate%>" ClientIDMode="Static" OnClick="WFB2SL2100Generate_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <asp:Button ID="WFB2SL2100Export" runat="server" Text="<%$Resources:Resource,wfb2sk_TextDown%>" ClientIDMode="Static" OnClick="WFB2SL2100Export_Click" ValidationGroup="GroupA" OnClientClick="return MycheckDowning(this.value);"/>--%>
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
            <asp:PostBackTrigger ControlID="WFB2SL2100Export" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
