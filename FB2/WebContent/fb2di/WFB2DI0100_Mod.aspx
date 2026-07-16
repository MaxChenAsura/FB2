<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0100_Mod.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0100_Mod" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".number").mask('99.99');
            $(".number").css("text-align", "right");
            $.unblockUI();
        }

        function checkConfirm() {
            var result = confirm("是否確定取消?");
            if (result)
                window.location.href("WFB2DI0100_Qry.aspx");
        }

        function openQry() {
            window.location.href("WFB2DI0100_Qry.aspx?reload=Y");
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="25%" />
                                <col width="15%" />
                                <col width="25%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--加班類型--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME_CD" runat="server" ClientIDMode="Static" Width="64px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_OVERTIME_CD%>"
                                            ControlToValidate="txt_OVERTIME_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_OVERTIME_CD_Chinese%>" ControlToValidate="txt_OVERTIME_CD" ForeColor="Red"
                                            ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <%--加班類型說明 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME_DESC" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_DESC%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OVERTIME_DESC" runat="server" ClientIDMode="Static" Width="150px" CssClass="MandatoryField" MaxLength="30"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_OVERTIME_DESC%>"
                                            ControlToValidate="txt_OVERTIME_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--出勤別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_WORK_DAY_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_WORK_DAY_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_WORK_DAY_CD" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_WORK_DAY_CD%>"
                                            ControlToValidate="ddl_WORK_DAY_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <%--加班日期類型--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME_DT_TYPE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_DT_TYPE2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_OVERTIME_DT_TYPE" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_OVERTIME_DT_TYPE%>"
                                            ControlToValidate="ddl_OVERTIME_DT_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--加班計算時數--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_O_HOUR_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_O_HOUR_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_O_HOUR_CD" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_O_HOUR_CD%>"
                                            ControlToValidate="ddl_O_HOUR_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <%--加班倍數代碼--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_O_MUL_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_O_MUL_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_O_MUL_CD" runat="server" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_O_MUL_CD%>"
                                            ControlToValidate="ddl_O_MUL_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--三高累計時數--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HYPER_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HYPER_HOUR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_HYPER_SHOUR" runat="server" ClientIDMode="Static" CssClass="number MandatoryField" Width="64px"></asp:TextBox>&nbsp; ~&nbsp;
                                        <asp:TextBox ID="txt_HYPER_EHOUR" runat="server" ClientIDMode="Static" CssClass="number MandatoryField" Width="64px"></asp:TextBox>
                                        <asp:CompareValidator ID="CompareValidator7" runat="server" ControlToCompare="txt_HYPER_SHOUR"
                                            ControlToValidate="txt_HYPER_EHOUR" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_HYPER_HOUR_RANGE%>" Type="Double" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_HYPER_SHOUR_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_HYPER_SHOUR" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>                                    
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_HYPER_EHOUR_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_HYPER_EHOUR" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>   
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_HYPER_SHOUR%>"
                                            ControlToValidate="txt_HYPER_SHOUR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_HYPER_EHOUR%>"
                                            ControlToValidate="txt_HYPER_EHOUR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <%--免稅時數--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_NONTAX_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_NONTAX_HOUR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_NONTAX_SHOUR" runat="server" ClientIDMode="Static" CssClass="number" Width="64px"></asp:TextBox>&nbsp; ~&nbsp;
                                        <asp:TextBox ID="txt_NONTAX_EHOUR" runat="server" ClientIDMode="Static" CssClass="number" Width="64px"></asp:TextBox>
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_NONTAX_SHOUR"
                                            ControlToValidate="txt_NONTAX_EHOUR" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_NONTAX_HOUR_RANGE%>" Type="Double" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                        <asp:CustomValidator ID="CustomValidatorNONTAX_SHOUR" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_NONTAX_SHOUR_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_NONTAX_SHOUR" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>                                    
                                        <asp:CustomValidator ID="CustomValidatorNONTAX_EHOUR" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_NONTAX_EHOUR_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_NONTAX_EHOUR" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>                                    
                                    </td>
                                </tr>
                                <tr>
                                    <%--一般累計時數--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_NORMAL_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_NORMAL_HOUR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_NORMAL_SHOUR" runat="server" ClientIDMode="Static" CssClass="number MandatoryField" Width="64px"></asp:TextBox>&nbsp; ~&nbsp;
                                        <asp:TextBox ID="txt_NORMAL_EHOUR" runat="server" ClientIDMode="Static" CssClass="number MandatoryField"  Width="64px"></asp:TextBox>
                                        <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToCompare="txt_NORMAL_SHOUR"
                                            ControlToValidate="txt_NORMAL_EHOUR" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_NORMAL_HOUR_RANGE%>" Type="Double" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                        <asp:CustomValidator ID="CustomValidatorNORMAL_SHOUR" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_NORMAL_SHOUR_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_NORMAL_SHOUR" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>                                    
                                        <asp:CustomValidator ID="CustomValidatorNORMAL_EHOUR" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_NORMAL_EHOUR_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_NORMAL_EHOUR" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator> 
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_NORMAL_SHOUR%>"
                                            ControlToValidate="txt_NORMAL_SHOUR" ForeColor="Red" ValidationGroup="GroupA" Display="None" ></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_NORMAL_EHOUR%>"
                                            ControlToValidate="txt_NORMAL_EHOUR" ForeColor="Red" ValidationGroup="GroupA" Display="None" ></asp:RequiredFieldValidator>
                                    </td>
                                    <%--累計時數--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OTHER_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OTHER_HOUR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_OTHER_SHOUR" runat="server" ClientIDMode="Static" CssClass="number" Width="64px"></asp:TextBox>&nbsp; ~&nbsp;
                                        <asp:TextBox ID="txt_OTHER_EHOUR" runat="server" ClientIDMode="Static" CssClass="number" Width="64px"></asp:TextBox>
                                        <asp:CompareValidator ID="CompareValidator6" runat="server" ControlToCompare="txt_OTHER_SHOUR"
                                            ControlToValidate="txt_OTHER_EHOUR" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_OTHER_HOUR_RANGE%>" Type="Double" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                        <asp:CustomValidator ID="CustomValidatorOTHER_SHOUR" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_OTHER_SHOUR_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_OTHER_SHOUR" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>                                    
                                        <asp:CustomValidator ID="CustomValidatorOTHER_EHOUR" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_OTHER_EHOUR_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_OTHER_EHOUR" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>                                    
                                    </td>
                                </tr>
                                <tr>
                                    <%--加計本薪時數--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BASE_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_BASE_HOUR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_BASE_SHOUR" runat="server" ClientIDMode="Static" CssClass="number" Width="64px"></asp:TextBox>&nbsp; ~&nbsp;
                                        <asp:TextBox ID="txt_BASE_EHOUR" runat="server" ClientIDMode="Static" CssClass="number" Width="64px"></asp:TextBox>
                                        <asp:CompareValidator ID="CompareValidator5" runat="server" ControlToCompare="txt_BASE_SHOUR"
                                            ControlToValidate="txt_BASE_EHOUR" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_BASE_HOUR_RANGE%>" Type="Double" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                        <asp:CustomValidator ID="CustomValidatorBASE_SHOUR" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_BASE_SHOUR_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_BASE_SHOUR" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>                                    
                                        <asp:CustomValidator ID="CustomValidatorBASE_EHOUR" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_BASE_EHOUR_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_BASE_EHOUR" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>                                    
                                    </td>
                                    <%--應稅時數--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_TAX_HOUR" runat="server" Text="<%$Resources:Resource,wfb2di_lb_TAX_HOUR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_TAX_SHOUR" runat="server" ClientIDMode="Static" CssClass="number" Width="64px"></asp:TextBox>&nbsp; ~&nbsp;
                                        <asp:TextBox ID="txt_TAX_EHOUR" runat="server" ClientIDMode="Static" CssClass="number" Width="64px"></asp:TextBox>
                                        <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToCompare="txt_TAX_SHOUR"
                                            ControlToValidate="txt_TAX_EHOUR" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TAX_HOUR_RANGE%>" Type="Double" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                        <asp:CustomValidator ID="CustomValidatorTAX_SHOUR" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TAX_SHOUR_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_TAX_SHOUR" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>                                    
                                        <asp:CustomValidator ID="CustomValidatorTAX_EHOUR" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2di_ERR_TAX_EHOUR_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_TAX_EHOUR" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>                                    
                                    </td>
                                </tr>
                                <tr>
                                    <%--換休/申告--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME_EXCHANGE_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_EXCHANGE_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_OVERTIME_EXCHANGE_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_OVERTIME_EXCHANGE_CD%>"
                                            ControlToValidate="ddl_OVERTIME_EXCHANGE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--換休結算週期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_SETTLE_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_SALARY_SETTLE_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_SETTLE_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_SALARY_SETTLE_CD%>"
                                            ControlToValidate="ddl_SALARY_SETTLE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <%--換休對象(,隔開)--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CHG_WORK_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CHG_WORK_CD2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_CHG_WORK_CD" runat="server" ClientIDMode="Static" ></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--使用狀態--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_USED" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IS_USED%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_USED" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_ERR_IS_USED%>"
                                            ControlToValidate="ddl_IS_USED" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <%--IFLOW顯示否--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_IFLOW_SHOW" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IS_IFLOW_SHOW%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:DropDownList ID="ddl_IS_IFLOW_SHOW" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Text="" Value="-1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Y-是" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="N-否" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2di_lb_IS_IFLOW_SHOW_isNull%>"
                                            ControlToValidate="ddl_IS_IFLOW_SHOW" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--備註--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2di_lb_REMARK%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_REMARK" runat="server" Width="300px" MaxLength="210"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DI0100Save" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0100Save%>" ValidationGroup="GroupA" OnClick="WFB2DI0100Save_Click" />

                                            <%--<asp:Button ID="WFB2DI0100Save" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0100Save%>" ValidationGroup="GroupA" OnClick="WFB2DI0100Save_Click" />--%>
                                            
                                            <asp:Button ID="btn_Cancel" runat="server" Text="<%$Resources:Resource,wfb2di_btn_Cancel%>" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_Cancel_Click" />
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

    </asp:UpdatePanel>
</asp:Content>

