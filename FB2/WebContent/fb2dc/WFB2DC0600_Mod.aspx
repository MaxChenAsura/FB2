<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC0600_Mod.aspx.cs" Inherits="WebContent_fb2dc_WFB2DC0600_Mod" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        jQuery(document).ready(function () {
            initForm();

        });
        function initForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.ym').mask('9999/99/99');
            $('#txt_EMP_ID').mask('99999');
            $.unblockUI();
            $("#txt_EMP_NAME").attr("readonly", true);

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
            var result = confirm("是否確定取消?");
            if (result)
                window.location.href("WFB2DC0600_Qry.aspx");
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <table cellspacing="1" cellpadding="1" width="1020px" border="0"
        class="Body_Label">
        <colgroup>
            <col width="100%" />
        </colgroup>
        <tbody>
            <tr>
                <td align="right" class="Body_label"></td>
            </tr>
        </tbody>
    </table>


    <table cellspacing="1" cellpadding="1" width="1020px" border="0" class="Body_Label">
        <colgroup>
            <col width="12%" />
            <col width="14%" />
            <col width="16%" />
            <col width="14%" />
            <col width="18%" />
            <col width="32%" />
        </colgroup>
        <tbody>
            <!-- START: 1st Line in Search Criteria area -->
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_EMP_ID%>"></asp:Label>:</th>
                <td align="left" class="Body_label">
                    <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static" CssClass="MandatoryField" OnTextChanged="txt_EMP_ID_TextChanged" AutoPostBack="true"></asp:TextBox>
                    <input id="bt_EMP_ID" runat="server" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                    <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_EMP_ID%>"
                        ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                </td>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_DEPT_NO%>"></asp:Label>:</th>
                <td align="left" class="Body_label">
                    <asp:Label ID="lb_DEPT_NO2" runat="server"></asp:Label>
                </td>
                <th align="left"></th>
                <td align="left" class="Body_label">&nbsp;</td>
            </tr>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_ABNORMAL_TYPE" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_ABNORMAL_TYPE%>"></asp:Label>:</th>
                <td align="left" class="Body_label">
                    <asp:DropDownList ID="ddl_ABNORMAL_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ABNORMAL_TYPE%>"
                        ControlToValidate="ddl_ABNORMAL_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                </td>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_ABNORMAL_REASON_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_ABNORMAL_REASON_CD%>"></asp:Label>:</th>
                <td align="left" class="Body_label">
                    <asp:DropDownList ID="ddl_ABNORMAL_REASON_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ABNORMAL_REASON_CD%>"
                        ControlToValidate="ddl_ABNORMAL_REASON_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_CALENDAR_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CALENDAR_DT%>"></asp:Label>:</th>
                <td align="left" class="Body_label">
                    <asp:TextBox ID="txt_CALENDAR_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date ym"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_CALENDAR_DT%>"
                        ControlToValidate="txt_CALENDAR_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None" ></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cv_CALENDAR_DT" runat="server" ValidateEmptyText="true"
                        ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CALENDAR_DT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                        ControlToValidate="txt_CALENDAR_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>                    
                </td>
                <%-- 異常刷卡日期時間--%>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_ABNORMAL_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_ABNORMAL_DT2%>"></asp:Label>:</th>
                <td align="left" class="Body_label">
                    <asp:TextBox ID="txt_ABNORMAL_DT" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="MandatoryField date ym"></asp:TextBox>
                    <asp:CustomValidator ID="cv_ABNORMAL_DT" runat="server" ValidateEmptyText="true"
                        ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_ABNORMAL_DT_format%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                        ControlToValidate="txt_ABNORMAL_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ABNORMAL_DT%>"
                        ControlToValidate="txt_ABNORMAL_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None" ></asp:RequiredFieldValidator>
                </td>

                <td align="left" class="Body_label">

                    <asp:DropDownList ID="ddl_HOUR" runat="server" CssClass="MandatoryField">
                        <asp:ListItem Text="" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="00" Value="00"></asp:ListItem>
                        <asp:ListItem Text="01" Value="01"></asp:ListItem>
                        <asp:ListItem Text="02" Value="02"></asp:ListItem>
                        <asp:ListItem Text="03" Value="03"></asp:ListItem>
                        <asp:ListItem Text="04" Value="04"></asp:ListItem>
                        <asp:ListItem Text="05" Value="05"></asp:ListItem>
                        <asp:ListItem Text="06" Value="06"></asp:ListItem>
                        <asp:ListItem Text="07" Value="07"></asp:ListItem>
                        <asp:ListItem Text="08" Value="08"></asp:ListItem>
                        <asp:ListItem Text="09" Value="09"></asp:ListItem>
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                    </asp:DropDownList>
                    時
						<asp:DropDownList ID="ddl_MINUTE" runat="server" CssClass="MandatoryField">
                        </asp:DropDownList>
                    分
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ABNORMAL_DT_HOUR%>"
                        ControlToValidate="ddl_HOUR" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ABNORMAL_DT_MINUTE%>"
                        ControlToValidate="ddl_MINUTE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                </td>
                <td align="left" class="Body_label"></td>
            </tr>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_ABNORMAL_SOURCE_CD" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_ABNORMAL_SOURCE_CD%>"></asp:Label>:</th>
                <td align="left" class="Body_label">人工入力</td>


                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="IS_RE_MAKE" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_IS_RE_MAKE%>"></asp:Label>:</th>
                <td align="left" class="Body_label" colspan="3">
                    <asp:RadioButtonList ID="rbl_IS_RE_MAKE" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="是" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="否" Value="N"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>

            </tr>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_IFLOW_NO%>"></asp:Label>:</th>
                <td align="left" class="Body_label">
                    <asp:Label ID="lb_IFLOW_NO2" runat="server"></asp:Label>
                </td>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_IFLOW_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_IFLOW_APPROVE_DT2%>"></asp:Label>:</th>
                <td align="left" class="Body_label">
                    <asp:Label ID="lb_IFLOW_APPROVE_DT2" runat="server"></asp:Label>
                </td>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_IS_CONFIRM" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_IS_CONFIRM%>"></asp:Label>:</th>
                <td align="left" class="Body_label" colspan="2">
                    <asp:DropDownList ID="ddl_IS_CONFIRM" runat="server" CssClass="MandatoryField">
                        <asp:ListItem Text="" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="N" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_IS_CONFIRM%>"
                        ControlToValidate="ddl_IS_CONFIRM" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                </td>
            </tr>


            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_REMARK%>"></asp:Label>:</th>
                <td align="left" class="Body_label" colspan="9">
                    <asp:TextBox ID="txt_REMARK" runat="server" MaxLength="210" Width="651px" ClientIDMode="Static"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th></th>
                <td align="right" class="Body_label" colspan="5">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <aces:Btn ID="WFB2DC0600Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Save%>" OnClick="WFB2DC0600Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />
                            
                            <%--<asp:Button ID="WFB2DC0600Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0600Save%>" OnClick="WFB2DC0600Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />--%>
                            
                            <asp:Button id="btn_back" type="button" Text="<%$Resources:Resource,wfb2dc_btn_back%>" OnClientClick="return confirm('是否確定取消?');" runat="server" OnClick="btn_back_Click"  />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="WFB2DC0600Save" EventName="Click" />

                        </Triggers>
                    </asp:UpdatePanel>

                </td>
            </tr>
            <!-- end: Create MODULE ID -->
        </tbody>
    </table>
            </ContentTemplate>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>
