<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC1100_Mod.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC1100_Mod" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            initForm();

        });
        function initForm() {
            $.unblockUI();
            $("#txt_ORDER_SEQ").mask('99999');
        }

        //有輸入其他費用則必須輸入其他費用說明
        function CheckRemark(source, arguments) {
            if ($("#txt_OTHER_AMOUNT").val() != "" && $("#txt_OTHER_AMOUNT").val() != "0") {
                if ($("#txt_REMARK").val() == "")
                    arguments.IsValid = false;
            }
            else
                arguments.IsValid = true;
        }

        function checkConfirm() {
            return confirm($('#hidwfb2sc_cancel_ConfirmMessage').val());
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

        <tr height="100%" valign="top">
            <td>

                <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                    <colgroup>
                        <col width="13%" />
                        <col width="12%" />
                        <col width="13%" />
                        <col width="12%" />
                        <col width="13%" />
                        <col width="12%" />
                        <col width="12%" />
                        <col width="13%" />
                    </colgroup>
                    <tbody>
                        <!-- START: 1st Line in Search Criteria area -->
                        <tr>
                            <!-- START: Create MODULE ID -->
                            <%--薪資項目代號--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_ID_mod%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_SALARY_ID" runat="server" MaxLength="4" Width="60" class="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_insert_SALARY_ID%>"
                                    ControlToValidate="txt_SALARY_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="薪資項目代號只能輸入數字，且長度為4碼"
                                    ControlToValidate="txt_SALARY_ID" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression="^(\d{4})+$" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <%--項目名稱--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_SALARY_NAME" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_NAME%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_SALARY_NAME" runat="server" MaxLength="20" Width="120px" class="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_insert_SALARY_NAME%>"
                                    ControlToValidate="txt_SALARY_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <%--薪資項目類別--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_SALARY_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_CD_DESC%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_SALARY_CD" runat="server" class="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_insert_SALARY_CD_DESC%>"
                                    ControlToValidate="ddl_SALARY_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <th></th>
                            <th></th>
                        </tr>
                        <tr>
                            <%--加減項--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_PLUS" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_PLUS%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_IS_PLUS" runat="server" class="MandatoryField" ClientIDMode="Static">
                                    <asp:ListItem Value="1" Text="加項"></asp:ListItem>
                                    <asp:ListItem Value="-1" Text="減項"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_insert_IS_PLUS%>"
                                    ControlToValidate="ddl_IS_PLUS" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <%--應免稅--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_TAX" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_TAX%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_IS_TAX" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_TAX_Y%>" />
                                    <asp:ListItem Value="N" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_TAX_N%>" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <%--薪資所得類別--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_TAX_FORMAT" runat="server" Text="<%$Resources:Resource,wfb2sc_TAX_FORMAT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:DropDownList ID="ddl_TAX_FORMAT" runat="server" ClientIDMode="Static"></asp:DropDownList>                                
                            </td>
                        </tr>
                        <tr>
                            <%--支付方式--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_PAY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_PAY_TYPE%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_PAY_TYPE" runat="server" class="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_required_PAY_TYPE%>"
                                    ControlToValidate="ddl_PAY_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <%--支付對象--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_PAY_OBJECT" runat="server" Text="<%$Resources:Resource,wfb2sc_PAY_OBJECT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_PAY_OBJECT" runat="server" class="MandatoryField" ClientIDMode="Static"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_required_PAY_OBJECT%>"
                                    ControlToValidate="ddl_PAY_OBJECT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <%--薪資單排列順序--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_ORDER_SEQ" runat="server" Text="<%$Resources:Resource,wfb2sc_ORDER_SEQ2%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_ORDER_SEQ" runat="server" MaxLength="5" Width="60" class="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_insert_ORDER_SEQ%>"
                                    ControlToValidate="txt_ORDER_SEQ" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="薪資單排列順序只能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_ORDER_SEQ" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <%--敘薪項目--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_SALARY" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_SALARY%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_IS_SALARY" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y" runat="server" Text="Y" />
                                    <asp:ListItem Value="N" runat="server" Text="N" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <%--破月計算--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_RATE" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_RATE%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_IS_RATE" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y" runat="server" Text="Y" />
                                    <asp:ListItem Value="N" runat="server" Text="N" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <%--加班費計算--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_OVERTIME" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_OVERTIME%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_IS_OVERTIME" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y" runat="server" Text="Y" />
                                    <asp:ListItem Value="N" runat="server" Text="N" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <%--請假計算--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_LEAVE" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_LEAVE%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_IS_LEAVE" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y" runat="server" Text="Y" />
                                    <asp:ListItem Value="N" runat="server" Text="N" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <%--納入勞保--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_INS_A" runat="server" Text="<%$Resources:Resource,wfb2sc_INS_A%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_INS_A" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y" runat="server" Text="Y" />
                                    <asp:ListItem Value="N" runat="server" Text="N" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <%--納入健保--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_INS_B" runat="server" Text="<%$Resources:Resource,wfb2sc_INS_B%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_INS_B" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y" runat="server" Text="Y" />
                                    <asp:ListItem Value="N" runat="server" Text="N" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <%--納入勞退--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_INS_C" runat="server" Text="<%$Resources:Resource,wfb2sc_INS_C%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_INS_C" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y" runat="server" Text="Y" />
                                    <asp:ListItem Value="N" runat="server" Text="N" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <%--補充保費--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_INS_D" runat="server" Text="<%$Resources:Resource,wfb2sc_INS_D%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_INS_D" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y" runat="server" Text="Y" />
                                    <asp:ListItem Value="N" runat="server" Text="N" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <%--獎金計算--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_BOUNS" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_BOUNS%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_IS_BOUNS" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y" runat="server" Text="Y" />
                                    <asp:ListItem Value="N" runat="server" Text="N" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <%--退休金計算(舊制)--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_RETAIR" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_RETAIR%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_IS_RETAIR" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y" runat="server" Text="Y" />
                                    <asp:ListItem Value="N" runat="server" Text="N" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <%--納入法扣--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_ARREARS" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_ARREARS%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_IS_ARREARS" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y" runat="server" Text="Y" />
                                    <asp:ListItem Value="N" runat="server" Text="N" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <%--是否代扣--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_PREMINUS" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_PREMINUS%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_IS_PREMINUS" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y" runat="server" Text="Y" />
                                    <asp:ListItem Value="N" runat="server" Text="N" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>

                        <tr>
                            <%--是否計算特休--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_PAY_LEAVE" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_PAY_LEAVE%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_IS_PAY_LEAVE" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y" runat="server" Text="Y" />
                                    <asp:ListItem Value="N" runat="server" Text="N" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            <%--是否計算加班--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_CAL_OVERTIME" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_CAL_OVERTIME%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rb_IS_CAL_OVERTIME" runat="server" ClientIDMode="Static" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="Y" runat="server" Text="Y" />
                                    <asp:ListItem Value="N" runat="server" Text="N" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                            
                        </tr>

                        <tr>
                            <td colspan="8">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <%--套用公式--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_FORMULA" runat="server" Text="<%$Resources:Resource,wfb2sc_FORMULA%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:TextBox ID="txt_FORMULA" runat="server" MaxLength="30" Width="300px" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <%--停用註記--%>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_DISABLE" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_DISABLE%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:DropDownList ID="ddl_IS_DISABLE" runat="server" ClientIDMode="Static">
                                    <asp:ListItem Value="Y" Text="<%$Resources:Resource,wfb2sc_dll_IS_DISABLE_Y%>"></asp:ListItem>
                                    <asp:ListItem Value="N" Text="<%$Resources:Resource,wfb2sc_dll_IS_DISABLE_N%>" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            
                        </tr>

                        <tr>
                            <td align="right" class="Body_label" colspan="8">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                    <ContentTemplate>
                                        <aces:Btn ID="WFB2SC1100Ok1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Ok1%>" OnClick="WFB2SC1100Ok1_Click" ValidationGroup="GroupA" />
                                        <%--<asp:Button ID="WFB2SC1100Ok1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Ok1%>" OnClick="WFB2SC1100Ok1_Click" ValidationGroup="GroupA" />--%>
                                        <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_cancel%>" OnClientClick="return checkConfirm();" OnClick="btn_back_Click" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="WFB2SC1100Ok1" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>



                        <!-- end: Create MODULE ID -->
                        <!-- START: Create a line to separate Search field with body field -->

                        <!-- END: Create a line -->
                    </tbody>
                </table>

            </td>
        </tr>
        <tr>
            <td align="right" class="Body_label"></td>
        </tr>
    </table>

    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_SaveCheck_message" Value="<%$Resources:Resource,wfb2sc_SaveCheck_message%>" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Save_ConfirmMessage%>" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Cancel_Confirm%>" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>
