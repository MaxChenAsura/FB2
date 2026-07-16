<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC0500_Upd.aspx.cs" Inherits="WebContent_WFB2DC0500_Upd" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();

        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');

            $("#txt_BORROW_TYPE").attr("readonly", true);
            $("#txt_PERSON_ID").attr("readonly", true);
            $("#txt_PERSON_NAME").attr("readonly", true);
            $("#txt_PERSON_DC").attr("readonly", true);
            $("#txt_TEMP_CARD_CD").attr("readonly", true);
            $("#txt_CARD_NO").attr("readonly", true);
            $("#txt_RETURN_DT").attr("readonly", true);

            $.unblockUI();
        }

        function checkConfirm() {
            var result = confirm("是否確定取消?");
            if (result)
                window.location.href("WFB2DC0500_Qry.aspx");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_label">
                <colgroup>
                    <col width="80%" />
                    <col width="20%" />
                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <td>
                            <table cellspacing="1" cellpadding="1" width="93%" border="0" class="Body_label">
                                <colgroup>
                                    <col width="22%" />
                                    <col width="32%" />
                                    <col width="20%" />
                                    <col width="16%" />
                                </colgroup>
                                <tbody>
                                    <!-- START: 1st Line in Search Criteria area -->
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_BORROW_TYPE" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_BORROW_TYPE%>"></asp:Label>:                                    
                                        </th>
                                        <td align="left" class="Body_label" colspan="3">
                                            <asp:TextBox ID="txt_BORROW_TYPE" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_PERSON_ID" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PERSON_ID%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label" colspan="3">
                                            <asp:TextBox ID="txt_PERSON_ID" runat="server" BorderWidth="0" ClientIDMode="Static" Width="64px"></asp:TextBox>
                                            <asp:TextBox ID="txt_PERSON_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="250px"></asp:TextBox>
                                        </td>
                                    </tr>

                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_PERSON_DC" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_PERSON_DC%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label" colspan="3">
                                            <asp:TextBox ID="txt_PERSON_DC" runat="server" BorderWidth="0" ClientIDMode="Static" Width="420px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_TEMP_CARD_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_TEMP_CARD_CD%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label" colspan="3">
                                            <asp:TextBox ID="txt_TEMP_CARD_CD" runat="server" BorderWidth="0" ClientIDMode="Static" Width="420px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_CARD_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_NO2%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label" colspan="3">
                                            <asp:TextBox ID="txt_CARD_NO" runat="server" BorderWidth="0" ClientIDMode="Static" Width="200px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <%-- 借用期間 --%>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_START_DT_RANGE%>"></asp:Label>:
                                        </th>
                                        <td class="Body_label" colspan="3">
                                            <asp:TextBox ID="txt_START_DT_S" runat="server" ClientIDMode="Static" CssClass="MandatoryField date" ></asp:TextBox>&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddl_START_DT_S_H" runat="server" ClientIDMode="Static" ></asp:DropDownList>&nbsp;:&nbsp;
                                    <asp:DropDownList ID="ddl_START_DT_S_M" runat="server" ClientIDMode="Static" ></asp:DropDownList>
                                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_START_DT_S%>"
                                                ControlToValidate="txt_START_DT_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                            ~<br />
                                            <asp:TextBox ID="txt_START_DT_E" runat="server" ClientIDMode="Static" CssClass="MandatoryField date"></asp:TextBox>&nbsp;&nbsp;
                                    <asp:DropDownList ID="ddl_START_DT_E_H" runat="server" ClientIDMode="Static"></asp:DropDownList>&nbsp;:&nbsp;
                                    <asp:DropDownList ID="ddl_START_DT_E_M" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_START_DT_E%>"
                                                ControlToValidate="txt_START_DT_E" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="CustomValidatorSTART_DT_E" runat="server" ValidateEmptyText="true"
                                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_START_DATE_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                ControlToValidate="txt_START_DT_E" ValidationGroup="GroupA" Display="None">
                                            </asp:CustomValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_RETURN_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_RETURN_DT%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label" colspan="3">
                                            <asp:TextBox ID="txt_RETURN_DT" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_BORROW_REASON_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_BORROW_REASON_CD%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label" colspan="3">
                                            <asp:DropDownList ID="ddl_BORROW_REASON_CD" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_BORROW_REASON_CD%>"
                                                ControlToValidate="ddl_BORROW_REASON_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_BORROW_STATUS" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_BORROW_STATUS%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label" colspan="3">
                                            <asp:DropDownList ID="ddl_BORROW_STATUS" runat="server" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_BORROW_STATUS_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_BORROW_STATUS%>"
                                                ControlToValidate="ddl_BORROW_STATUS" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th align="left" class="Body_TableHeader">
                                            <asp:Label ID="lb_IS_RE_MARK" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_IS_RE_MARK%>"></asp:Label>:
                                        </th>
                                        <td align="left" class="Body_label" colspan="3">
                                            <asp:RadioButtonList ID="rbl_IS_RE_MARK" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static">
                                                <asp:ListItem Text="<%$Resources:Resource,wfb2dc_lb_Y%>" Value="Y"></asp:ListItem>
                                                <asp:ListItem Text="<%$Resources:Resource,wfb2dc_lb_N%>" Value="N" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td valign="top" align="left">
                            <table cellspacing="1" cellpadding="1" width="10%" border="0" class="Body_label">
                                <colgroup>
                                    <col width="100%" />
                                </colgroup>
                                <tbody>
                                    <!-- START: 1st Line in Search Criteria area -->
                                    <tr rowspan="8">
                                        <td style="border-right: #cccc99 1px solid; border-top: #cccc99 1px solid; border-left: #cccc99 1px solid; border-bottom: #cccc99 1px solid;">
                                            <asp:Image ID="EmpPhoto" runat="server" Width="120px" Height="120px" ImageUrl="" />
                                        </td>

                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <aces:Btn ID="WFB2DC0500Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Save%>" OnClick="WFB2DC0500Save_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />

                            <%--<asp:Button ID="WFB2DC0500Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Save%>" OnClick="WFB2DC0500Save_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                            
                            <asp:Button ID="WFB2DC0500Cancel" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0500Cancel%>" OnClick="WFB2DC0500Cancel_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:HiddenField ID="hid_START_DT" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

