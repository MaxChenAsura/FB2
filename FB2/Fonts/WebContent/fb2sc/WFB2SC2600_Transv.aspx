<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2600_Transv.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2600_Transv" Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        var li_id;
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $(".number").formatNumber({ format: "#,###", locale: "tw" });
            //gridviewScroll();
            $.unblockUI();


        }


        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function CheckVaild() {            
            var processed = true;       
            if (Page_ClientValidate("GroupA")) {               
                if (($("#txt_IaDat").val()).substring(0, 7) != $("#txt_SALARY_YM").val()) {
                    alert("入帳日期年月不等於薪資年月!");
                    return false;
                }
            }
            processed = confirm("是否確定生成傳票!!");           
            if(processed)
                BlockUI();         
            if (!processed)
                $.unblockUI();

            return processed;
        }

        var choietr = null;

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
                                <col width="20%" />
                                <col width="10%" />
                                <col width="20%" />
                                <col width="15%" />
                                <col width="25%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_TYPE_NAME" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_TYPE_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                        <asp:HiddenField ID="txt_SALARY_TYPE" runat="server" ClientIDMode="Static"></asp:HiddenField>
                                        <asp:HiddenField ID="txt_SALARY_YM" runat="server" ClientIDMode="Static"></asp:HiddenField>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sk_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_DT" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IASYS" runat="server" Text="<%$Resources:Resource,wfb2sc_IASYS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_IACYC" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAY_KIND_NAME" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PAY_KIND%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PAY_KIND_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                        <asp:HiddenField ID="txt_PAY_KIND" runat="server" ClientIDMode="Static"></asp:HiddenField>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PROCESS_STATUS_NAME" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PROCESS_STATUS_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAY_ID" runat="server" Text="<%$Resources:Resource,wfb2_lb_PAY_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PAY_ID" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAY_DT" runat="server" Text="<%$Resources:Resource,wfb2_lb_PAY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PAY_DT" runat="server" BorderWidth="0" ClientIDMode="Static" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <fieldset style="padding: 5px; border-color: red;">
                                            <legend class="Body_label">第一家派遣公司資料</legend>
                                            <table cellspacing="7" cellpadding="1" width="100%" border="0" class="Body_Label">
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_COMPANY_CD1" runat="server" Text="<%$Resources:Resource,wfb2_lb_COMPANY_CD%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_COMPANY_CD1" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                    </td>
                                                    <th></th>
                                                    <td></td>
                                                    <th></th>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_INV_TYPE11" runat="server" Text="<%$Resources:Resource,wfb2_lb_INV_TYPE1%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_INV_TYPE11" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_INV_NO11" runat="server" Text="<%$Resources:Resource,wfb2_lb_INV_NO%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_INV_NO11" runat="server" MaxLength="10" Width="80px" ClientIDMode="Static" Text=""></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                                            ErrorMessage="發票號碼只能輸入英數字" ControlToValidate="txt_INV_NO11" ForeColor="Red" ValidationGroup="GroupA"
                                                            ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>

                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_INV_DT11" runat="server" Text="<%$Resources:Resource,wfb2_lb_INV_DT%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_INV_DT11" runat="server" MaxLength="10" Width="80px" CssClass="date" ClientIDMode="Static" Text=""></asp:TextBox>
                                                        <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                                            ErrorMessage="<%$Resources:Resource,wfb2_INV_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                            ControlToValidate="txt_INV_DT11" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                               
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <fieldset style="padding: 5px; border-color: red;">
                                            <legend class="Body_label">第二家派遣公司資料</legend>
                                            <table cellspacing="7" cellpadding="1" width="100%" border="0" class="Body_Label">
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_COMPANY_CD2" runat="server" Text="<%$Resources:Resource,wfb2_lb_COMPANY_CD%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_COMPANY_CD2" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                    </td>
                                                    <th></th>
                                                    <td></td>
                                                    <th></th>
                                                    <td></td>
                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_INV_TYPE21" runat="server" Text="<%$Resources:Resource,wfb2_lb_INV_TYPE1%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:DropDownList ID="ddl_INV_TYPE21" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_INV_NO21" runat="server" Text="<%$Resources:Resource,wfb2_lb_INV_NO%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_INV_NO21" runat="server" MaxLength="10" Width="80px" ClientIDMode="Static" Text=""></asp:TextBox>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server"
                                                            ErrorMessage="發票號碼只能輸入英數字" ControlToValidate="txt_INV_NO21" ForeColor="Red" ValidationGroup="GroupA"
                                                            ValidationExpression="^[\d|a-zA-Z]+$" Display="None"></asp:RegularExpressionValidator>
                                                    </td>
                                                    <th align="left" class="Body_TableHeader">
                                                        <asp:Label ID="lb_INV_DT21" runat="server" Text="<%$Resources:Resource,wfb2_lb_INV_DT%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label">
                                                        <asp:TextBox ID="txt_INV_DT21" runat="server" MaxLength="10" Width="80px" CssClass="date" ClientIDMode="Static" Text=""></asp:TextBox>
                                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                                            ErrorMessage="<%$Resources:Resource,wfb2_INV_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                            ControlToValidate="txt_INV_DT21" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                                
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <fieldset style="padding: 5px; border-color: red;">
                                            <legend class="Body_label">其他資料</legend>
                                            <table cellspacing="7" cellpadding="1" width="100%" border="0" class="Body_Label">
                                                <tr>
                                                    <th align="left" class="Body_TableHeader" style="width:200px;">
                                                        <asp:Label ID="lb_TMC_PAY_TYPE"  runat="server" Text="<%$Resources:Resource,wfb2_lb_TMC_PAY_TYPE%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label" colspan="5">
                                                        <asp:DropDownList ID="ddl_TMC_PAY_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th align="left" class="Body_TableHeader" style="width:200px;">
                                                        <asp:Label ID="lb_OTHER_REMIT_DT" runat="server" Text="<%$Resources:Resource,wfb2_lb_OTHER_REMIT_DT%>"></asp:Label>:
                                                    </th>
                                                    <td align="left" class="Body_label" colspan="5">
                                                        <asp:TextBox ID="txt_OTHER_REMIT_DT" runat="server" MaxLength="10" Width="100px" CssClass="MandatoryField date" ClientIDMode="Static" Text=""></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="(媒體轉帳對象外)實際匯款日不可空白"
                                                            ControlToValidate="txt_OTHER_REMIT_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:CustomValidator ID="CustomValidator5" runat="server" ValidateEmptyText="true"
                                                            ErrorMessage="<%$Resources:Resource,wfb2_msg_REMIT_DT_FormatError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                            ControlToValidate="txt_OTHER_REMIT_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                                <th align="left" class="Body_TableHeader">
                                                    <asp:Label ID="lb_IaDat" runat="server" Text="入帳日期"></asp:Label>:
                                                </th>
                                                <td align="left" class="Body_label">
                                                    <asp:TextBox ID="txt_IaDat" runat="server" Width="81px" CssClass="date MandatoryField"  ClientIDMode="Static"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="請輸入入帳日期"
                                                        ControlToValidate="txt_IaDat" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="CustomValidator6" runat="server" ValidateEmptyText="true"
                                                        ErrorMessage="入帳日期格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                                        ControlToValidate="txt_IaDat" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                                </td>
                                            </table>
                                        </fieldset>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="Body_label" colspan="6">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SC2600Execute1" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Ok1%>" OnClick="WFB2SC2600Execute1_Click" OnClientClick="return CheckVaild();" Visible="true" />
                                            <%--<asp:Button ID="WFB2SC2600Execute1" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Ok1%>" OnClick="WFB2SC2600Execute1_Click" OnClientClick="CheckVaild();" Visible="true" ValidationGroup="GroupA"  />--%>
                                            <asp:Button ID="btn_return_back" runat="server" Text="<%$Resources:Resource,wfb2ia_back_page%>" OnClick="btn_return_back_Click" OnClientClick="BlockUI();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
            </table>

            <!--紀錄頁數(跳頁用)-->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
