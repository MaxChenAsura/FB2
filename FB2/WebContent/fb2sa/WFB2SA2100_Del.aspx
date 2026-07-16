<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sa/action/WFB2SA2100_Del.aspx.cs" Inherits="WebContent_fb2sa_WFB2SA2100_Del" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".money").blur(function () {
                $(this).parseNumber({ format: "#,###", locale: "tw" });
                $(this).formatNumber({ format: "#,###", locale: "tw" });
            });
            $.unblockUI();
        }

        function DeleteCheck() {
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
                processed = confirm($('#hidwfb299_Del_ConfirmMessage').val());
            }
            else
                return;

            if (!processed)
                $.unblockUI();

            return processed;
        }
    </script>
    <style type="text/css">
        .txtReadOnly[readonly] {
            background: #fff;
            color: #000;
            border: 0px solid #fff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="35%" />
                                <col width="15%" />
                                <col width="35%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EMP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_COMPANY_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_COMPANY_SNAME" runat="server" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_CD" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_EMP_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_CD_DESC" runat="server" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <hr />
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="35%" />
                                <col width="15%" />
                                <col width="35%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CHG_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_CHG_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CHG_STATUS" runat="server" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true" Font-Bold="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PROCESS_STATUS" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_PROCESS_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PROCESS_STATUS" runat="server" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true" Font-Bold="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_SALARY_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" colspan="3" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_NAME" runat="server" ClientIDMode="Static" CssClass="txtReadOnly" Font-Bold="true" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CHG_AMT_B" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_CHG_AMT_B%>"></asp:Label>:
                                    </th>
                                    <td align="left" colspan="3" class="Body_label">
                                        <asp:TextBox ID="txt_CHG_AMT_B" runat="server" style="text-align:right" MaxLength="7" ClientIDMode="Static" CssClass="txtReadOnly money" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_START_DT%>" ></asp:Label>:
                                    </th>
                                    <td align="left" colspan="3" class="Body_label">
                                        <asp:TextBox ID="txt_START_DT" runat="server" MaxLength="10" ClientIDMode="Static" CssClass="txtReadOnly" Font-Bold="true" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_END_DT" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_END_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" colspan="3" class="Body_label">
                                        <asp:TextBox ID="txt_END_DT" runat="server" MaxLength="10" ClientIDMode="Static" CssClass="txtReadOnly" Font-Bold="true" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_REMARK%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_REMARK" runat="server" Width="100%" Rows="3" MaxLength="100" TextMode="MultiLine" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="req_REMARK" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sa_require_REMARK%>"
                                            ControlToValidate="txt_REMARK" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <th></th>
                                    <td></td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CREATED_BY" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_CREATED_BY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_CREATED_BY" runat="server" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true" Font-Bold="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CREATED_DT" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_CREATED_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_CREATED_DT" runat="server" ClientIDMode="Static" CssClass="txtReadOnly" ReadOnly="true" Font-Bold="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SA2100Ok3" runat="server" Text="<%$Resources:Resource,btn_Save%>" ClientIDMode="Static" OnClick="WFB2SA2100Ok3_Click" CausesValidation="true" OnClientClick="return DeleteCheck();" ValidationGroup="GroupA" />
                                             <%--<asp:Button ID="WFB2SA2100Ok3" runat="server" Text="<%$Resources:Resource,btn_Save%>" ClientIDMode="Static" OnClick="WFB2SA2100Ok3_Click" CausesValidation="true" OnClientClick="return DeleteCheck();" ValidationGroup="GroupA" />--%>
                                            <asp:Button ID="btn_backpage" runat="server" Text="<%$Resources:Resource,wfb2sa_btn_backpage%>" CausesValidation="false" OnClick="btn_backpage_Click" ClientIDMode="Static"/>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>

                        </table>
                    </td>
                </tr>

            </table>
            <asp:HiddenField ID="hid_SALARY_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_SEQ_NO" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
