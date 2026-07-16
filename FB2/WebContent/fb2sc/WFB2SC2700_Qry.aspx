<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2700_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2700_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".dateYM").datepicker({ dateFormat: 'yy/mm' });
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.ym').mask('9999/99');
            $('.ymd').mask('9999/99/99');
            $(".number").formatNumber({ format: "#,###", locale: "tw" });
            gridviewScroll();
            $.unblockUI();

        }

        function CheckYMD(source, arguments) {
            if ($("#txt_PAY_SDT").val() != "" && $("#txt_PAY_EDT").val() != "") {
                if ($("#txt_PAY_EDT").val() < $("#txt_PAY_SDT").val())
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }

        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        var choietr = null;
        //檢查勾了幾個checkbox
        function LookUpCheckboxs() {
            choietr = null;
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    if (choietr == null)
                        choietr = i;
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1) {
                choietr = choietr - 2;
                if ($("[id=lb_PROCESS_STATUS]")[choietr].innerText.indexOf("4") == -1) {
                    //'該月薪資未已月結,無法執行薪資單EMAIL!'
                    alert($("#hid_wfd2sc_ERROR_WFB2SC2700Detail").val());
                    return false;
                }
                if ($("[id=hid_IS_EMAIL]")[choietr].value == "Y") {
                    //'該次月薪資已發送EMAIL,無法重複發送!'
                    alert($("#hid_wfd2sc_AlreadySend_WFB2SC2700Detail").val());
                    return false;
                }
                //發送EMAIL日期
                //var now = new Date();
                //var Current = now.getFullYear() + "/" + (now.getMonth()+1) + "/" + now.getDate();
                //var email_date = $("[id=lb_EMAIL_DT]")[choietr].innerText;
                //if ((Date.parse(email_date)).valueOf() <= (Date.parse(Current)).valueOf())
                //{
                //    alert('發送EMAIL日期 不可小於等於系統日!');
                //    return false;
                //}

            }
            else {
                //'請選取一筆資料!'
                alert($("#hid_wfb2_Mod_NotChoiceMessage").val());
                choietr = null;
                return false;
            }
        }
        //清空
        function doClear() {
            $("#txt_SALARY_YM").val("");
            $("#txt_LEAVE_DT_S").val("");
            $("#txt_LEAVE_DT_E").val("");
            $("#ddl_SALARY_TYPE").val("-1");
            $("#txt_PAY_SDT").val("");
            $("#txt_PAY_EDT").val("");
            $("#txt_PAY_ID").val("");
            return false;
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
                                        <%--薪資年月--%>
                                        <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_YM%>"></asp:Label>: 
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_YM" runat="server" MaxLength="6" Width="64px" CssClass="ym dateYM" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_SALARY_YM2_isError%>" ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <%--發薪日期區間--%>
                                        <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <uc1:UCDateTimeRange runat="server" ID="UCDateTimeRange" StartDateMaxLength="10" ClientIDMode="Static" ControlClientIDMode="Static" EndDateMaxLength="10" EndDateCssClass="ymd" StartDateCssClass="ymd"
                                            CompareValidatorOperator="GreaterThanEqual" CompareValidatorErrMesg="<%$Resources:Resource,wfb2_ERR_SALART_DT_RANGE%>" ValidatorStartDateErrMesg="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_S_isError%>"
                                            ValidatorEndDateErrMesg="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_E_isError%>" ClientValidationFunctionE="CheckDate" ClientValidationFunctionS="CheckDate" ValidationGroup="GroupA" />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--發薪類別--%>
                                        <asp:Label ID="lb_SALARY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <%--關帳日期區間--%>
                                        <asp:Label ID="lb_PAY_DT" runat="server" Text="<%$Resources:Resource,wfd2sc_lb_PAY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_PAY_SDT" runat="server" MaxLength="10" Width="80px" CssClass="ymd date" ClientIDMode="Static" Text=""></asp:TextBox>
                                        ~
                                            <asp:TextBox ID="txt_PAY_EDT" runat="server" MaxLength="10" Width="80px" CssClass="ymd date" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2_ERR_PAY_DT_RANGE%>" ClientValidationFunction="CheckYMD" ForeColor="Red"
                                            ControlToValidate="txt_PAY_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                                        <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfd2sc_error_PAY_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_PAY_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="qrytest2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfd2sc_error_PAY_DT_E%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_PAY_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <%--關帳代號--%>
                                        <asp:Label ID="lb_PAY_ID" runat="server" Text="<%$Resources:Resource,wfb2_lb_PAY_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PAY_ID" runat="server" MaxLength="11" Width="80px" ClientIDMode="Static" Text=""></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SC2700Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2SC2700Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />

                                            <%--<asp:Button ID="WFB2SC2700Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2SC2700Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sk_clear%>" OnClientClick="return doClear();" />
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
                <tr>

                    <td align="right" class="Body_label">

                        <div id="init_grid">
                            <%--EMAIL薪資單--%>
                            <aces:Btn ID="WFB2SC2700Detail" runat="server" Text="<%$Resources:Resource,wfd2sc_WFB2SC2700Detail%>" OnClick="WFB2SC2700Detail_Click" Visible="false" OnClientClick="return CheckDtlAction(); BlockUI();" />

                            <%--<asp:Button ID="WFB2SC2700Detail" runat="server" Text="<%$Resources:Resource,wfd2sc_WFB2SC2700Detail%>" OnClick="WFB2SC2700Detail_Click" Visible="false" OnClientClick="return CheckDtlAction(); BlockUI();" />--%>
                        </div>

                    </td>
                </tr>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                SelectCountMethod="GetCount" TypeName="CFB2SC2700DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_SALARY_YM" DefaultValue=""
                        Name="salary_ym" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="UCDateTimeRange" DefaultValue=""
                        Name="salary_sdt" PropertyName="StartDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="UCDateTimeRange" DefaultValue=""
                        Name="salary_edt" PropertyName="EndDateText" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_SALARY_TYPE" DefaultValue=""
                        Name="salary_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_PAY_SDT" DefaultValue=""
                        Name="pay_sdt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_PAY_EDT" DefaultValue=""
                        Name="pay_edt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_PAY_ID" DefaultValue=""
                        Name="pay_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>


            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectNonDisabledCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sk_RowNum%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發薪類別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>" SortExpression="SALARY_TYPE" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_SALARY_TYPE" runat="server" Text='<%#Bind("DESC2")%>'></asp:Label>
                            <asp:HiddenField ID="hid_SALARY_TYPE" runat="server" ClientIDMode="Static" Value='<%#Bind("SALARY_TYPE")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--薪資年月--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_SALARY_YM%>" SortExpression="SALARY_YM" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_SALARY_YM" runat="server" Text='<%#Bind("SALARY_YM", "{0:yyyy/MM}")%>' CssClass="ym"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發薪日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_SALARY_DT%>" SortExpression="SALARY_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: left; word-wrap: break-word;" ID="lb_SALARY_DT" runat="server" Text='<%#Bind("SALARY_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發放項目--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_PAY_KIND%>" SortExpression="PAY_KIND" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Width="120px" Style="text-align: left;" ID="lb_PAY_KIND" runat="server" Text='<%#Bind("DESC3")%>'></asp:Label>
                            <asp:HiddenField ID="hid_PAY_KIND" runat="server" ClientIDMode="Static" Value='<%#Bind("PAY_KIND")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--處理狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS2%>" SortExpression="PROCESS_STATUS" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_PROCESS_STATUS" ClientIDMode="Static" runat="server" Text='<%#Bind("DESC1")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--關帳代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2_lb_PAY_ID%>" SortExpression="PAY_ID" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Width="120px" Style="text-align: left;" ID="lb_PAY_ID" runat="server" Text='<%#Bind("PAY_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--關帳日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_PAY_DT%>" SortExpression="PAY_DT" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_PAY_DT" runat="server" Text='<%#Bind("PAY_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--是否轉傳票--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfd2sc_IS_VOUCHER%>" SortExpression="IS_VOUCHER" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_IS_VOUCHER" runat="server" Text='<%#Bind("IS_VOUCHER")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--EMAIL發送日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfd2sc_EMAIL_DT%>" SortExpression="EMAIL_DT" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Width="120px" Style="text-align: left;" ID="lb_EMAIL_DT" runat="server" ClientIDMode="Static" Text='<%#Bind("EMAIL_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                            <asp:HiddenField ID="hid_IS_EMAIL" runat="server" ClientIDMode="Static" Value='<%#Bind("IS_EMAIL")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <!--resource)-->
            <%--該月薪資未已月結,無法執行薪資單EMAIL!--%>
             <asp:HiddenField ID="hid_wfd2sc_ERROR_WFB2SC2700Detail" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfd2sc_ERROR_WFB2SC2700Detail%>" />
            <%--該次月薪資已發送EMAIL,無法重複發送!--%>
            <asp:HiddenField ID="hid_wfd2sc_AlreadySend_WFB2SC2700Detail" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfd2sc_AlreadySend_WFB2SC2700Detail%>" />
            <%--請選取一筆資料!--%>
            <asp:HiddenField ID="hid_wfb2_Mod_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2_Mod_NotChoiceMessage%>" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

