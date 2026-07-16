<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hd/WFB2HD0100_Update.aspx.cs" Inherits="WebContent_wfb2hd_WFB2HD0100_Update" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            initForm();

        }); 
        function initForm() {
            $.unblockUI();
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');

        }
        //儲存前檢查
        function saveCheck() {


            if ($("#ddl_FIRST_CNT option:selected").text() == "0" && $("#ddl_SECOND_CNT option:selected").text() == "0" && $("#ddl_THIRD_CNT option:selected").text() == "0") {
                alert("請選擇獎懲隻數");
                return false;
            }
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
            return confirm($('#hidwfb299_Cancel_ConfirmMessage').val());
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 25px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">

        <tr height="100%" valign="top">
            <td>
                <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                    <tbody>
                        <!-- START: 1st Line in Search Criteria area -->
                        <tr>
                            <!-- START: Create MODULE ID -->
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_emp_id" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_emp_id%>"></asp:Label></th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_emp_id" runat="server" MaxLength="10" Width="64px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                            </td>
                             <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_EMP_NAME%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                            </td>
                       </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_DEPT_NO%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_DEPT_NO" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                 <asp:Label ID="lb_DEPT_NAME" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_DEPT_NAME%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="3">
                                <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ReadOnly="true" Width="274px" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_LEVEL_CD%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_LEVEL_CD" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                           </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_PJOB_DESC" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_PJOB_DESC%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_PJOB_DESC" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_DOC_NO" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_DOC_NO%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_DOC_NO" runat="server" MaxLength="20" Width="200px" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_START_DT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:TextBox ID="txt_START_DT" runat="server" MaxLength="10" Width="100px" class="MandatoryField  date" ClientIDMode="Static"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hd_lb_START_DT_EMP%>"
                                    ControlToValidate="txt_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                  <!--驗證日期格式-->
                                <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2hd_format_start_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>	
                            </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_JUDGEMENT_TYPE" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_JUDGEMENT_TYPE%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:RadioButtonList ID="rbl_JUDGEMENT_TYPE" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="rbl_JUDGEMENT_TYPE_SelectedIndexChanged">
                                    <asp:ListItem Text="獎勵" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="懲處" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_REASON_CD" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_REASON_CD%>"></asp:Label>:</th>
                            <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_REASON_CD" CssClass="MandatoryField" runat="server" ></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2hd_lb_REASON_CD_EMP%>"
                                    ControlToValidate="ddl_REASON_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1">
                                </asp:RequiredFieldValidator>
                           </td>
                        </tr>
                        <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_FIRST_CNT" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_FIRST_CNT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="5">
                                <asp:DropDownList ID="ddl_FIRST_CNT" runat="server" ClientIDMode="Static">
                                    <asp:ListItem Selected="True">0</asp:ListItem>
                                    <asp:ListItem>1</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>
                                    <asp:ListItem>3</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                         <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_SECOND_CNT" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_SECOND_CNT%>"></asp:Label>:</th>
                            <td align="left" class="auto-style1" colspan="5">
                                <asp:DropDownList ID="ddl_SECOND_CNT" runat="server" ClientIDMode="Static">
                                    <asp:ListItem Selected="True">0</asp:ListItem>
                                    <asp:ListItem>1</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>
                                    <asp:ListItem>3</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                         <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_THIRD_CNT" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_THIRD_CNT%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="1">
                                <asp:DropDownList ID="ddl_THIRD_CNT" runat="server" ClientIDMode="Static">
                                    <asp:ListItem Selected="True">0</asp:ListItem>
                                    <asp:ListItem>1</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>
                                    <asp:ListItem>3</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_IS_FIRE" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_IS_FIRE%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="5">
                                <asp:CheckBox ID="chk_IS_FIRE" runat="server" />                            
                            </td>
                        </tr>
                       <tr>
                            <th align="left" class="Body_TableHeader">
                                <asp:Label ID="lb_REMARK" runat="server" Text="<%$Resources:Resource,wfb2hd_lb_REMARK%>"></asp:Label>:</th>
                            <td align="left" class="Body_label" colspan="5">
                                <asp:TextBox ID="txt_REMARK" runat="server" ClientIDMode="Static" Width="700px"></asp:TextBox>
                            </td>
                        </tr>

                        <tr>
                            <th></th>
                            <td align="right" class="Body_label" colspan="5">
                              <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                    <ContentTemplate>
                                        <aces:Btn ID="WFB2HD0100Save" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2HD0100Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />
                                        <%--<asp:Button ID="WFB2HD0100Save" runat="server" Text="<%$Resources:Resource,btn_save%>" OnClick="WFB2HD0100Save_Click" OnClientClick="return saveCheck();" ValidationGroup="GroupA" />--%>
                                     <asp:Button ID="btn_cancel" runat="server" OnClick="WFB2HD0100Cancel_Click" OnClientClick="checkConfirm();" Text="<%$Resources:Resource,btn_Cancel%>"  ClientIDMode="Static"/>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="WFB2HD0100Save" EventName="Click" />

                                    </Triggers>
                                </asp:UpdatePanel>

                            </td>
                        </tr>





                        <!-- end: Create MODULE ID -->
                        <!-- START: Create a line to separate Search field with body field -->
                        <tr>
                            <td align="center" height="1" colspan="6">
                                <hr>
                            </td>
                        </tr>
                        <!-- END: Create a line -->
                    </tbody>
                </table>

            </td>
        </tr>
        <tr>

            <td align="right" class="Body_label"></td>
        </tr>
    </table>
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
</asp:Content>
