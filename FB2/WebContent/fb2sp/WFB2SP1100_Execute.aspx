<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sp/WFB2SP1100_Execute.aspx.cs" Inherits="WebContent_WFB2SP1100_Execute" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            //$.unblockUI();
            iniForm();
        });
        var li_id;
        function iniForm() {
            $.unblockUI();
            //日期格式心須
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".dateYM").datepicker({ dateFormat: 'yy/mm' });
            $('.date').mask('9999/99/99');
            $('.dateYM').mask('9999/99');
            $('.basic').mask('99');
            $('#txt_EMP_NAME').attr("readonly", true);
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                $('#txt_RETIRE_DT').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val($.trim(JData.EMP_NAME));

                                if ($("#ddl_COMPUTER_TYPE").val() == "A") {
                                    //系統日
                                    $('#txt_RETIRE_DT').val($('#hid_sys_date').val());
                                    $('#hid_sys_RETIRE_DT').val($.trim(JData.LEAVE_DT))
                                } else {
                                    //退休日
                                    $('#txt_RETIRE_DT').val($.trim(JData.LEAVE_DT));
                                    $('#hid_sys_RETIRE_DT').val($.trim(JData.LEAVE_DT))
                                }
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME').val("");
                }
            });


        }

        //執行檢核
        function executeCheck() {
            var processed = false;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
                processed = true;
            } else {
                return processed;
            }
            if (!processed) {
                $.unblockUI();
            }

            return processed;
        }
        function countEYM(value) {
            var avg_month = parseInt( $("#hid_AVG_MONTH").val())-1;
            var year = value.substr(0, 4);
            var eYear = 0;
            var month = value.substr(5, 2);
            var sYear = parseInt(year,10);
            var eMonth = parseInt(month,10) + parseInt(avg_month,10);
            if (eMonth > 12) {
                eYear = sYear + Math.floor(eMonth/12);
                eMonth = eMonth % 12;
            } else {
                eYear = sYear;
            }
            eMonth = (eMonth < 10) ? "0" + eMonth : eMonth;
            var result = eYear + "/" + eMonth;
            $("#txt_SALARY_EYM").val(result);
           
        }
        function checkconfirm1(checkconfirmmessage) {
            if (confirm(checkconfirmmessage)) {
                BlockUI();
                __doPostBack('execute', '');
            }
            else
                $.unblockUI();
                return;
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!--頁面table-->
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="30%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--工號--%>
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_emp_id%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" Width="50px" ClientIDMode="Static" MaxLength="5" CssClass="MandatoryField"> </asp:TextBox>
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sp_required_emp_id%>"
                                            ControlToValidate="txt_EMP_ID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <th align="left" class="Body_TableHeader">
                                    <%--工資起迄月份--%>
                                    <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_salary_ym_se%>"></asp:Label>:</th>
                                <td align="left" class="Body_label">
                                    <asp:TextBox ID="txt_SALARY_SYM" runat="server" Width="60px" ClientIDMode="Static" CssClass="MandatoryField dateYM"  onchange="countEYM(this.value);"></asp:TextBox>
                                    ~<asp:TextBox ID="txt_SALARY_EYM" runat="server" Width="60px" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="qry1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sp_required_salary_sym%>"
                                        ControlToValidate="txt_SALARY_SYM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                        ErrorMessage="<%$Resources:Resource,wfb2sp_format_salary_sym%>" ControlToValidate="txt_SALARY_SYM" ForeColor="Red"
                                        ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                                    </asp:RegularExpressionValidator>
                                </td>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--發放日期--%>
                                        <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_salary_dt%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_SALARY_DT" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField date" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sp_required_salary_dt%>"
                                            ControlToValidate="txt_SALARY_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sp_format_salary_dt%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>

                                    <th align="left" class="Body_TableHeader">
                                        <%--發放基數--%>
                                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_pay_basic%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PAY_BASIC" runat="server" Width="50px" MaxLength="2" ClientIDMode="Static" CssClass="MandatoryField basic"></asp:TextBox>
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_month%>"></asp:Label>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sp_format_pay_basic%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_PAY_BASIC" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SP1101Execute" runat="server" Text="計算" OnClick="WFB2SP1100Execute_Click" OnClientClick="return executeCheck();"  />    
                                            <%--
                                            <asp:Button ID="WFB2SP1101Execute" runat="server" Text="計算" OnClick="WFB2SP1100Execute_Click" OnClientClick="return executeCheck();"  />    
                                            --%> 
                                            <asp:Button ID="btn_back" runat="server" Text="返回" OnClick="btn_back_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hid_AVG_MONTH" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_PAY_BASIC" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
