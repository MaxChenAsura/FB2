<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2100_Add.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2100_Add" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            initForm();

        });
        function initForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".ym").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $('.ymd').mask('9999/99/99');
            gridviewScroll();
            $.unblockUI();
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        function checkField() {
            if ($("#txt_SALARY_YM").val().trim() == "") {
                alert($('#hidwfb2sc_SALARY_YM2_first').val());
                return false;
            }
        }
        function checkConfirm() {
            return confirm($('#hidwfb2sc_cancel_ConfirmMessage').val());
        }
        function CheckSaveAction(source, arguments) {
            if ($('#ddl_SALARY_TYPE').val() == "C" || $('#ddl_SALARY_TYPE').val() == "D") {
                if (LookUpCheckboxs() == 1)
                    arguments.IsValid = true;
                else
                    arguments.IsValid = false;
            }
            else
                return true;
        }
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        function addSucceesAlert() {
            alert($('#hidwfb2sc_Add_success').val());
            $('#btn_addSucceesHref').click();
        }

        function CheckExec() {

            if (Page_ClientValidate("GroupA")) {                
                if ($("#txt_SALARY_YM").val() != $("#txt_IACYC").val()) {
                    return confirm("薪資年月不等於入帳週期!");
                }
                BlockUI();
            }
            else
                return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="35%" />
                                <col width="15%" />
                                <col width="35%" />
                            </colgroup>
                            <tbody>
                                <!-- START: 1st Line in Search Criteria area -->
                                <tr>
                                    <!-- START: Create MODULE ID -->
                                    <%--發薪類別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_TYPE" runat="server" CssClass="MandatoryField" ClientIDMode="Static"
                                            OnSelectedIndexChanged="ddl_SALARY_TYPE_SelectedIndexChanged" AutoPostBack="true">
                                            <%--onclick="return checkField();"--%>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE_isNull%>"
                                            ControlToValidate="ddl_SALARY_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_ID_isNul%>" ClientValidationFunction="CheckSaveAction" ForeColor="Red"
                                            ControlToValidate="ddl_SALARY_TYPE" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                    <%--入帳週期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IACYC" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_IACYC%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_IACYC" runat="server" Width="81px"  CssClass="ym MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_IACYC_isNull%>"
                                            ControlToValidate="txt_IACYC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_txt_IACYC_isError%>"
                                            ControlToValidate="txt_IACYC" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator> 
                                    </td>
                                </tr>
                                <tr>
                                    <%--發薪日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_DT" runat="server" Width="81px" CssClass="date ymd MandatoryField"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_isNull%>"
                                            ControlToValidate="txt_SALARY_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator6" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                    <%--薪資年月--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_YM2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_YM" runat="server" MaxLength="6" CssClass="ym MandatoryField" ClientIDMode="Static" Width="100px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_SALARY_YM2_isNull%>"
                                            ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_SALARY_YM2_isError%>"
                                            ControlToValidate="txt_SALARY_YM" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--計薪日期區間--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_SDT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_CAL%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_SDT" runat="server" Width="81px" CssClass="date ymd"></asp:TextBox>
                                        ~  
                                        <asp:TextBox ID="txt_SALARY_EDT" runat="server" Width="81px" CssClass="date ymd"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_SDT_CAL_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_EDT_CAL_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator5" ControlToCompare="txt_SALARY_SDT" ControlToValidate="txt_SALARY_EDT" Display="None" ForeColor="Red"
                                            ValidationGroup="GroupA" Operator="GreaterThanEqual" Type="Date" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_CAL_isError%>" runat="server" />
                                    </td>
                                    <%--考勤日期區間--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DUTY_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_DUTY_SDT_TO_EDT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DUTY_SDT" runat="server" Width="81px" CssClass="date ymd"></asp:TextBox>
                                        ~  
                                        <asp:TextBox ID="txt_DUTY_EDT" runat="server" Width="81px" CssClass="date ymd"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_DUTY_SDT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_DUTY_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator5" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_DUTY_EDT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_DUTY_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator7" ControlToCompare="txt_DUTY_SDT" ControlToValidate="txt_DUTY_EDT" Display="None" ForeColor="Red"
                                            ValidationGroup="GroupA" Operator="GreaterThanEqual" Type="Date" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_DUTY_SDT_TO_EDT_isError%>" runat="server" />
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
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getAddData"
                SelectCountMethod="getAddCount" TypeName="CFB2SC2100BO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_SALARY_TYPE" DefaultValue=""
                        Name="salary_type" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sc_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Style="text-align: center;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--薪資項目代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_ID%>" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_ID" runat="server" Text='<%#Bind("SALARY_ID")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--薪資項目名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_NAME%>" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_NAME" runat="server" Text='<%#Bind("SALARY_NAME")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--薪資項目類別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_CD%>" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_CD" runat="server" Text='<%#Bind("DESC1")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>


            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="每頁10000筆" Value="10000"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr>
                    <td align="right" class="Body_label">
                        <aces:Btn ID="WFB2SC2100Ok1" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClientClick="return CheckExec();" OnClick="WFB2SC2100Ok1_Click" ValidationGroup="GroupA" />
                        <%--<asp:Button ID="WFB2SC2100Ok1" runat="server" Text="<%$Resources:Resource,btn_Save%>" OnClientClick="CheckValid();" OnClick="WFB2SC2100Ok1_Click" ValidationGroup="GroupA" />--%>
                        <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_cancel%>" OnClientClick="checkConfirm();" OnClick="btn_back_Click" />
                    </td>
                </tr>
            </table>

            <asp:Button ID="btn_addSucceesHref" runat="server" OnClick="btn_addSucceesHref_Click" ClientIDMode="Static" Style="display: none;" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_SaveCheck_message" Value="<%$Resources:Resource,wfb2sc_SaveCheck_message%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Cancel_Confirm%>" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_SALARY_YM2_first" Value="<%$Resources:Resource,wfb2sc_SALARY_YM2_first%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Add_success" Value="<%$Resources:Resource,wfb2sc_Add_success%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
