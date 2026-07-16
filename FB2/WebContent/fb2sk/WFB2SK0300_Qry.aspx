<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sk/WFB2SK0300_Qry.aspx.cs" Inherits="WebContent_fb2sk_WFB2SK0300_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $('.ym').mask('9999/99');
            $('.num').mask('9999999');
            //$(".number").formatNumber({ format: "#,###", locale: "tw" });
            gridviewScroll();
            $.unblockUI();

        }

        function CheckYM(source, arguments) {
            if ($("#txt_DATA_YM_S").val() != "" && $("#txt_DATA_YM_E").val() != "") {
                if ($("#txt_DATA_YM_E").val() < $("#txt_DATA_YM_S").val())
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }

        }
        function CheckAddnum(source, arguments) {
            var re = /^[0-9]+$/;
            if (!re.test($("#txt_NEW_SALARY_AMT").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }
        function CheckEdinum(source, arguments) {
            var re = /^[0-9]+$/;
            if (!re.test($("#txt_EDIT_SALARY_AMT").val()))
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }
        function addcheck() {

            $("#HID_NEW_DATA_YM").val($("#txt_NEW_DATA_YM").val());
            $("#HID_NEW_SALARY_AMT").val($("#txt_NEW_SALARY_AMT").val());
            $("#HID_NEW_REMARK").val($("#txt_NEW_REMARK").val());

        }
        //function Valid() {
        //    Page_IsValid = Page_ClientValidate('GroupB');
        //    if (Page_IsValid) {
        //        if (confirm($("#hid_wfb2sk_Save_Confirm").val())) {
        //            addcheck();
        //            return true;
        //        } else {
        //            return false;
        //        }
        //    }
        //}

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

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hid_wfb2sk_Del_ConfirmMessage').val());
            else {
                alert($('#hid_wfb2sk_Del_NotChoiceMessage').val());
                return false;
            }
        }
        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hid_wfb2sk_Mod_NotChoiceMessage').val());
                return false;
            }
        }
        //檢核薪資轉出
        function CheckAnnounce() {
            if (LookUpCheckboxs() == 1) {
                return confirm($('#hid_wfb2sk_Announce_Message').val());
            }
            else {
                alert($('#hid_wfb2sk_Announce_NotChoiceMessage').val());
                return false;
            }
        }
        //檢查勾了幾個checkbox
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
        //清空
        function doClear() {
            var Current = getThisMonth();
            $("#txt_DATA_YM_S").val(Current);
            $("#txt_DATA_YM_E").val(Current);
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
                                        <asp:Label ID="lb_DATA_YM" runat="server" Text="<%$Resources:Resource,wfb2sk_DATA_YM%>"></asp:Label>
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_DATA_YM_S" runat="server" MaxLength="6" Width="64px" CssClass="MandatoryField ym date" ClientIDMode="Static" Text=""></asp:TextBox>~
                                            <asp:TextBox ID="txt_DATA_YM_E" runat="server" MaxLength="6" Width="64px" CssClass="MandatoryField ym date" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sk_required_DATA_YM_S%>"
                                            ControlToValidate="txt_DATA_YM_S" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2si_error_DATA_YM%>" ControlToValidate="txt_DATA_YM_S" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sk_required_DATA_YM_E%>"
                                            ControlToValidate="txt_DATA_YM_E" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2si_error_DATA_YM%>" ControlToValidate="txt_DATA_YM_E" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sk_greaterthan_DATA_YM%>" ClientValidationFunction="CheckYM" ForeColor="Red"
                                            ControlToValidate="txt_DATA_YM_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>

                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SK0300Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2SK0300Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />

                                            <%--<asp:Button ID="WFB2SK0300Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2SK0300Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
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
                            <aces:Btn ID="WFB2SK0300Add" runat="server" Text="<%$Resources:Resource,wfb2sk_add%>" OnClick="WFB2SK0300Add_Click" Visible="true" OnClientClick="BlockUI();" />
                            <aces:Btn ID="WFB2SK0300Delete" runat="server" Text="<%$Resources:Resource,wfb2sk_del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SK0300Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2SK0300Edit" runat="server" Text="<%$Resources:Resource,wfb2sk_edit%>" OnClick="WFB2SK0300Edit_Click" Visible="false" OnClientClick="return CheckModeifyAction(); BlockUI();" />
                            <aces:Btn ID="WFB2SK0300Release" runat="server" Text="<%$Resources:Resource,wfb2sk_announce%>" OnClick="WFB2SK0300Release_Click" OnClientClick="return CheckAnnounce();" Visible="false" />
                            <aces:Btn ID="WFB2SK0300OK" runat="server" Text="<%$Resources:Resource,wfb2sk_ok%>" Visible="false" OnClick="WFB2SK0300OK_Click" OnClientClick="addcheck(); CheckValid(); " ValidationGroup="GroupB" />

                            <%--asp:Button ID="WFB2SK0300Add" runat="server" Text="<%$Resources:Resource,wfb2sk_add%>" OnClick="WFB2SK0300Add_Click" Visible="true" OnClientClick="BlockUI();" />
                            <asp:Button ID="WFB2SK0300Delete" runat="server" Text="<%$Resources:Resource,wfb2sk_del%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SK0300Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2SK0300Edit" runat="server" Text="<%$Resources:Resource,wfb2sk_edit%>" OnClick="WFB2SK0300Edit_Click" Visible="false" OnClientClick="return CheckModeifyAction(); BlockUI();" />
                            <asp:Button ID="WFB2SK0300Release" runat="server" Text="<%$Resources:Resource,wfb2sk_announce%>" OnClick="WFB2SK0300Release_Click" OnClientClick="return CheckAnnounce();" Visible="false" />
                            <asp:Button ID="WFB2SK0300OK" runat="server" Text="<%$Resources:Resource,wfb2sk_ok%>" Visible="false" OnClick="WFB2SK0300OK_Click" OnClientClick="addcheck(); CheckValid(); " ValidationGroup="GroupB" />--%>
                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sk_cancel%>" Visible="false" OnClick="btn_cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                        </div>

                    </td>
                </tr>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                SelectCountMethod="GetCount" TypeName="CFB2SK0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DATA_YM_S" DefaultValue=""
                        Name="data_ym_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_DATA_YM_E" DefaultValue=""
                        Name="data_ym_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>


            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox Style="text-align: center;" ID="cb_all" runat="server" onclick="javascript:SelectNonDisabledCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox Style="text-align: center;" ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox Style="text-align: center;" ID="cb_check" runat="server" Checked="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sk_RowNum%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: center;" ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Style="text-align: center;" ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label Style="text-align: center;" ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </FooterTemplate>

                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sk_DATA_YM1%>" SortExpression="DATA_YM" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_DATA_YM" runat="server" Text='<%#Bind("DATA_YM", "{0:yyyy/MM}")%>' CssClass="ym"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_DATA_YM" runat="server" Text='<%#Bind("DATA_YM", "{0:yyyy/MM}")%>' CssClass="ym"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="60px" Style="text-align: left" ID="txt_NEW_DATA_YM" runat="server" MaxLength="6" CssClass="MandatoryField ym date" ClientIDMode="Static"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sk_required_DATA_YM%>"
                                ControlToValidate="txt_NEW_DATA_YM" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2si_error_DATA_YM%>" ControlToValidate="txt_NEW_DATA_YM" ForeColor="Red" ValidationGroup="GroupB"
                                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sk_SALARY_AMT%>" SortExpression="SALARY_AMT" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: right;" ID="lb_SALARY_AMT" runat="server" Text='<%#Bind("SALARY_AMT", "{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Style="text-align: right" ID="txt_EDIT_SALARY_AMT" runat="server" MaxLength="7" Width="60px" ClientIDMode="Static" Text='<%#Bind("SALARY_AMT")%>' CssClass="MandatoryField num"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sk_required_SALARY_AMT%>"
                                ControlToValidate="txt_EDIT_SALARY_AMT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="checknum" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sk_error_SALARY_AMT%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_EDIT_SALARY_AMT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="60px" Style="text-align: right" ID="txt_NEW_SALARY_AMT" runat="server" MaxLength="7" ClientIDMode="Static" CssClass="MandatoryField num"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sk_required_SALARY_AMT%>"
                                ControlToValidate="txt_NEW_SALARY_AMT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="checknum2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sk_error_SALARY_AMT%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_SALARY_AMT" ValidationGroup="GroupB" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sk_SALARY_TRANS_DT%>" SortExpression="SALARY_TRANS_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_SALARY_TRANS_DT" runat="server" Text='<%#Bind("SALARY_TRANS_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_SALARY_TRANS_DT" runat="server" Text='<%#Bind("SALARY_TRANS_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sk_SALARY_DT%>" SortExpression="SALARY_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_SALARY_DT" runat="server" Text='<%#Bind("SALARY_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_SALARY_DT" runat="server" Text='<%#Bind("SALARY_DT", "{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sk_REMARK%>" HeaderStyle-Width="620px" ItemStyle-Width="620px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label Width="620px" Style="text-align: left; word-wrap: break-word;" ID="lb_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox Style="text-align: left" ID="txt_EDIT_REMARK" runat="server" MaxLength="300" Width="600px" Text='<%#Bind("REMARK")%>'></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox Width="600px" Style="text-align: left" ID="txt_NEW_REMARK" runat="server" MaxLength="300" ClientIDMode="Static"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>

                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_checkall" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sk_RowNum%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sk_DATA_YM1%>" Width="60px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sk_SALARY_AMT%>" Width="60px"></asp:Label>
                            </td>

                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2sk_SALARY_TRANS_DT%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sk_SALARY_DT%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2sk_REMARK%>" Width="620px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:CheckBox ID="cb_check" runat="server" />
                            </td>
                            <td>
                                <asp:Label ID="lb_EmptyRowNumber" runat="server" Text="1"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox Style="text-align: left" ID="txt_NEW_DATA_YM" runat="server" CssClass="MandatoryField ym date" ClientIDMode="Static" Width="60px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sk_required_DATA_YM%>"
                                    ControlToValidate="txt_NEW_DATA_YM" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2si_error_DATA_YM%>" ControlToValidate="txt_NEW_DATA_YM" ForeColor="Red" ValidationGroup="GroupB"
                                    ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox Style="text-align: right" ID="txt_NEW_SALARY_AMT" runat="server" MaxLength="7" ClientIDMode="Static" CssClass="MandatoryField num" Width="60px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sk_required_SALARY_AMT%>"
                                    ControlToValidate="txt_NEW_SALARY_AMT" ForeColor="Red" ValidationGroup="GroupB" Display="None"></asp:RequiredFieldValidator>

                            </td>

                            <td>
                                <asp:Label ID="lb_SALARY_TRANS_DT" runat="server" Text='<%#Bind("SALARY_TRANS_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lb_SALARY_DT" runat="server" Text='<%#Bind("SALARY_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox Style="text-align: left" ID="txt_NEW_REMARK" runat="server" MaxLength="300" ClientIDMode="Static" Width="620px"></asp:TextBox>
                            </td>

                        </tr>
                    </table>

                </EmptyDataTemplate>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                            <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                            <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                            <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                            <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
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
            <asp:HiddenField ID="HID_NEW_DATA_YM" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_SALARY_AMT" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_NEW_REMARK" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="add_check" runat="server" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2sk_Announce_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Announce_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sk_Announce_Message" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Announce_Message%>" />
            <asp:HiddenField ID="hid_wfb2sk_Del_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Del_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sk_Del_ConfirmMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Del_ConfirmMessage%>" />
            <asp:HiddenField ID="hid_wfb2sk_Mod_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Mod_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sk_Save_Confirm" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sk_Save_Confirm%>" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
