<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0100_Dtl_Mod.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0100_Dtl_Mod" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".number").mask('999.99');
            $(".number").css("text-align", "right");
        }

        function checkConfirm() {
            var result = confirm("是否確定取消?");
            if (result)
                window.location.href("WFB2DH0100_Dtl.aspx");
        }

        function CheckLEAVE_SPECIAL_CD(LEAVE_MAX_DAY_CD_id) {
            var txt = document.getElementById(LEAVE_MAX_DAY_CD_id);
            if (txt.value == "G") {
                $("#ddl_LEAVE_SPECIAL_CD").css("background-color", "#FFD7D7");
            }
            else {
                $("#ddl_LEAVE_SPECIAL_CD").css("background-color", "#FFFFFF");
            }
        }

        function openQry() {
            window.location.href("WFB2DH0100_Dtl.aspx");
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
                                <col width="10%" />
                                <col width="25%" />
                                <col width="15%" />
                                <col width="25%" />
                                <col width="25%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_MAIN_LEAVE_CD" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SUB_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SUB_LEAVE_CD" runat="server" ClientIDMode="Static" MaxLength="2" Width="64px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_SUB_LEAVE_CD2%>"
                                            ControlToValidate="txt_SUB_LEAVE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_SUB_LEAVE_CD2_Chinese%>" ControlToValidate="txt_SUB_LEAVE_CD" ForeColor="Red"
                                            ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SUB_LEAVE_DESC" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_DESC%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SUB_LEAVE_DESC" runat="server" ClientIDMode="Static" MaxLength="30" CssClass="MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_SUB_LEAVE_DESC%>"
                                            ControlToValidate="txt_SUB_LEAVE_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_PAY_RATE" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_PAY_RATE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LEAVE_PAY_RATE" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="64px"></asp:TextBox>
                                        <asp:Label ID="Label2" runat="server" Text="(100%=1)"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_PAY_RATE%>"
                                            ControlToValidate="txt_LEAVE_PAY_RATE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorLEAVE_PAY_RATE" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_PAY_RATE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_LEAVE_PAY_RATE" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_TIME_UNIT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_TIME_UNIT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_LEAVE_TIME_UNIT" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_TIME_UNIT%>"
                                            ControlToValidate="ddl_LEAVE_TIME_UNIT" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_COUNT_HOUR" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_COUNT_HOUR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LEAVE_COUNT_HOUR" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="64px"></asp:TextBox>
                                        <asp:Label ID="Label1" runat="server" Text="(依時間單位定)"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_COUNT_HOUR%>"
                                            ControlToValidate="txt_LEAVE_COUNT_HOUR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorLEAVE_COUNT_HOUR" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_COUNT_HOUR_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_LEAVE_COUNT_HOUR" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_MIN_VALUE" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_MIN_VALUE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LEAVE_MIN_VALUE" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="64px"></asp:TextBox>
                                        <asp:Label ID="Label3" runat="server" Text="(依時間單位定)"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MIN_VALUE%>"
                                            ControlToValidate="txt_LEAVE_MIN_VALUE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidatorLEAVE_MIN_VALUE" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MIN_VALUE_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_LEAVE_MIN_VALUE" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_COUNT_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_COUNT_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_LEAVE_COUNT_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_COUNT_CD%>"
                                            ControlToValidate="ddl_LEAVE_COUNT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_MAX_DAY_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lbtn_LEAVE_MAX_DAY_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_LEAVE_MAX_DAY_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField" onblur="javascript:CheckLEAVE_SPECIAL_CD(this.id)"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_MAX_DAY_CD%>"
                                            ControlToValidate="ddl_LEAVE_MAX_DAY_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_INCLUDE_HOLIDAY" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IS_INCLUDE_HOLIDAY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_INCLUDE_HOLIDAY" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_IS_INCLUDE_HOLIDAY%>"
                                            ControlToValidate="ddl_IS_INCLUDE_HOLIDAY" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_TIME_LIMIT_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lbtn_LEAVE_TIME_LIMIT_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_LEAVE_TIME_LIMIT_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_TIME_LIMIT_CD%>"
                                            ControlToValidate="ddl_LEAVE_TIME_LIMIT_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_ALLOW_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lbtn_LEAVE_ALLOW_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_LEAVE_ALLOW_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_LEAVE_ALLOW_CD%>"
                                            ControlToValidate="ddl_LEAVE_ALLOW_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_SPECIAL_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_LEAVE_SPECIAL_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_LEAVE_SPECIAL_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_SETTLE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SALARY_SETTLE_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_SETTLE_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_SALARY_SETTLE_CD%>"
                                            ControlToValidate="ddl_SALARY_SETTLE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_IFLOW_SHOW" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IS_IFLOW_SHOW%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_IFLOW_SHOW" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_IS_IFLOW_SHOW%>"
                                            ControlToValidate="ddl_IS_IFLOW_SHOW" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_USED" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IS_USED%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_USED" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_IS_USED%>"
                                            ControlToValidate="ddl_IS_USED" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_QRY_SHOW" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IS_QRY_SHOW%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_QRY_SHOW" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <%--扣年獎在職天數 --%>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_AWARD_DAY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_AWARD_DAY" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="64px"></asp:TextBox>
                                        <asp:Label ID="Label5" runat="server" Text="(每8小時要扣除的天數)"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_AWARD_DAY%>"
                                            ControlToValidate="txt_AWARD_DAY" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator11" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_txt_AWARD_DAY%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_AWARD_DAY" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>

                                    </td>
                                    <%--考核控管 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IS_ASSESS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_ASSESS" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_IS_ASSESS%>"
                                            ControlToValidate="ddl_IS_ASSESS" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <%--扣紅利在職天數 --%>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_BONUS_DAY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_BONUS_DAY" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="64px"></asp:TextBox>
                                        <asp:Label ID="lb_BONUS_DAY" runat="server" Text="(每8小時要扣除的天數)"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_BONUS_DAY%>"
                                            ControlToValidate="txt_BONUS_DAY" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_txt_BONUS_DAY%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_BONUS_DAY" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>

                                    </td>
                                    <%--扣除累積時數 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_ACC_HOUR" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IS_ACC_HOUR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_ACC_HOUR" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_IS_ACC_HOUR%>"
                                            ControlToValidate="ddl_IS_ACC_HOUR" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <%--扣期間工期滿金在職天數 --%>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_PLAN_DAY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PLAN_DAY" runat="server" ClientIDMode="Static" CssClass="MandatoryField number" Width="64px"></asp:TextBox>
                                        <asp:Label ID="lb_PLAN_DAY" runat="server" Text="(每8小時要扣除的天數)"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_PLAN_DAY%>"
                                            ControlToValidate="txt_PLAN_DAY" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_txt_PLAN_DAY%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_PLAN_DAY" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>

                                    </td>
                                    <%--指定出勤別 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DH_WORK_DAY_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DH_WORK_DAY_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_DH_WORK_DAY_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_DH_WORK_DAY_CD%>"
                                            ControlToValidate="ddl_DH_WORK_DAY_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                
                                <tr>                                    
                                    <%--指定性別 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DH_SEX_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DH_SEX_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_DH_SEX_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_DH_SEX_CD%>"
                                            ControlToValidate="ddl_DH_SEX_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DH0101Save" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0101Save%>" ValidationGroup="GroupA" OnClick="WFB2DH0101Save_Click" />

                                            <%--<asp:Button ID="WFB2DH0101Save" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0101Save%>" ValidationGroup="GroupA" OnClick="WFB2DH0101Save_Click" />--%>

                                            <asp:Button ID="btn_Cancel" runat="server" Text="<%$Resources:Resource,wfb2dh_btn_Cancel%>" OnClick="btn_Cancel_Click" />
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
