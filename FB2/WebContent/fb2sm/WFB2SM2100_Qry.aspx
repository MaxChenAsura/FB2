<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sm/WFB2SM2100_Qry.aspx.cs" Inherits="WebContent_fb2sm_WFB2SM210_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.ymd').mask('9999/99/99');
            $(".number2").mask("999");
            $(".year").mask("9999");
            gridviewScroll();
            $.unblockUI();

        }

        function ClearAll() {
            $("#txt_DATA_YEAR").val("");
            return false;
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F"

            });
        }
        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }
        function checkconfirm(checkconfirmmessage) {
            if (confirm(checkconfirmmessage)) {
                BlockUI();
                __doPostBack('execute', '');
            }
            else
                return;
        }
        function checkconfirm2(checkconfirmmessage) {
            if (confirm(checkconfirmmessage)) {
                BlockUI();
                __doPostBack('execute2', '');
            }
            else
                return;
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb2sc_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb2sc_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2sc_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckSaveAction() {
            return confirm($('#hidwfb2sc_Save_ConfirmMessage').val());
        }
        function CheckGenerateAction() {
            if (LookUpCheckboxs() == 1) {
                return true;
            }
            else {
                alert('請選擇一筆資料');
                return false;
            }
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
        function checkYear() {
            var re = /^\d\d\d\d$/;
            if (re.test($("#txt_NEW_DATA_YEAR").val())) {
                return true;
            }
            else {
                $("#txt_NEW_DATA_YEAR").val("");
                alert('年度輸入錯誤');
                return false;
            }
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 268435136px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <%--查詢條件欄位寬度--%>
                <colgroup>
                    <col width="15%" />
                    <col width="40%" />
                    <col width="15%" />
                    <col width="30%" />
                </colgroup>

                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DATA_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_DATA_YEAR%>"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DATA_YEAR" CssClass="year" runat="server" MaxLength="4" size="4" ClientIDMode="Static"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sl_lb_DATA_YM_isError%>"
                                ControlToValidate="txt_DATA_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                        </td>
                        <th></th>
                        <td></td>
                    </tr>
                    <%--查詢清除Button--%>
                    <tr>
                        <td align="right" colspan="10">
                            <div id="init_grid" style="display: inline">
                                <%--<asp:Button ID="WFB2SM2100Search" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM210Search%>" OnClick="WFB2SM2100Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                                <aces:Btn ID="WFB2SM2100Search" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM210Search%>" OnClick="WFB2SM2100Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,wfb2sm_btn_clear%>" OnClientClick="return ClearAll();" />
                            </div>
                        </td>
                    </tr>
                    <%--線--%>
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr />
                        </td>
                    </tr>
                    <%--新增刪除修改確認取消Button--%>
                    <tr>
                        <td align="right" colspan="10">
                            <aces:Btn ID="WFB2SM2100Add" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Add%>" OnClick="WFB2SM210Add_Click" />
                            <aces:Btn ID="WFB2SM2100Delete" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SM210Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2SM2100Edit" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2SM210Edit_Click" Visible="false" />

                            <aces:Btn ID="WFB2SM2100Generate" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Generate%>" OnClientClick="return CheckGenerateAction();" Visible="false" OnClick="WFB2SM210Generate_Click" />
                            <aces:Btn ID="WFB2SM2100Release" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Release%>" OnClientClick="return CheckGenerateAction();" Visible="false" OnClick="WFB2SM210Release_Click" />

                            <aces:Btn ID="WFB2SM2100OK" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210OK%>" Visible="false" OnClick="WFB2SM210OK_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                            <%--<asp:Button ID="WFB2SM2100Add" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Add%>" OnClick="WFB2SM210Add_Click" />
                            <asp:Button ID="WFB2SM2100Delete" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SM210Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2SM2100Edit" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2SM210Edit_Click" Visible="false" />

                            <asp:Button ID="WFB2SM2100Generate" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Generate%>" OnClientClick="return CheckGenerateAction();" Visible="false" OnClick="WFB2SM210Generate_Click" />
                            <asp:Button ID="WFB2SM2100Release" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Release%>" OnClientClick="return CheckGenerateAction();" Visible="false" OnClick="WFB2SM210Release_Click" />

                            <asp:Button ID="WFB2SM2100OK" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210OK%>" Visible="false" OnClick="WFB2SM210OK_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                            <asp:Button ID="WFB2SM2100Cancel" runat="server" Text="<%$resources:resource,wfb2sm_WFB2SM210Cancel%>" Visible="false" OnClick="WFB2SM210Cancel_Click" OnClientClick="return confirm($('#hidwfb2sc_Cancel_ConfirmMessage').val());" />
                        </td>
                    </tr>

                </tbody>

            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SM2100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DATA_YEAR" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="data_year" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="hid_first_load" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="first_load" PropertyName="Value" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px" OnRowCommand="gv_result_RowCommand"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectNonDisabledCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--年度--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_DATA_YEAR%>" SortExpression="DATA_YEAR" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DATA_YEAR" runat="server" Text='<%#Bind("DATA_YEAR")%>'></asp:Label>
                            <asp:HiddenField ID="hid_PROCESS_STATUS" runat="server" Value='<%#Bind("PROCESS_STATUS")%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EDIT_DATA_YEAR" runat="server" Text='<%#Bind("DATA_YEAR")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_DATA_YEAR" runat="server" MaxLength="4" size="4" CssClass="MandatoryField year" onblur="return checkYear();"
                                AutoPostBack="true" OnTextChanged="txt_NEW_DATA_YEAR_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_DATA_YEAR_gv%>"
                                ControlToValidate="txt_NEW_DATA_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>

                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--晉昇回數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_DATA_SEQ%>" SortExpression="DATA_SEQ" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DATA_SEQ" runat="server" Text='<%#Bind("DATA_SEQ")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EDIT_DATA_SEQ" runat="server" Text='<%#Bind("DATA_SEQ")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NEW_DATA_SEQ" runat="server" MaxLength="1" size="1"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--核可狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_PROCESS_STATUS%>" SortExpression="PROCESS_STATUS" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PROCESS_STATUS" runat="server" Text='<%#Bind("PROCESS_STATUS_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資料生成日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_GENERATE_DT%>" SortExpression="GENERATE_DT" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_GENERATE_DT" runat="server" Text='<%#Bind("GENERATE_DT")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--提出核可日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_NOTICE_DT%>" SortExpression="NOTICE_DT" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_NOTICE_DT" runat="server" Text='<%#Bind("NOTICE_DT")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--核可日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_APPROVE_DT%>" SortExpression="APPROVE_DT" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPROVE_DT" runat="server" Text='<%#Bind("APPROVE_DT")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--發佈日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_RELEASE_DT%>" SortExpression="RELEASE_DT" HeaderStyle-Width="110px" ItemStyle-Width="110px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_RELEASE_DT" runat="server" Text='<%#Bind("RELEASE_DT")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--生效日期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_EXECUTIVE_DT%>" SortExpression="EXECUTIVE_DT" HeaderStyle-Width="110px" ItemStyle-Width="110px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_EXECUTIVE_DT" runat="server" Text='<%#Bind("EXECUTIVE_DT")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_EXECUTIVE_DT" runat="server" Text='<%#Bind("EXECUTIVE_DT")%>' MaxLength="10" size="10" CssClass="MandatoryField date ymd"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_EXECUTIVE_DT%>"
                                ControlToValidate="txt_EDIT_EXECUTIVE_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sm_Required_EXECUTIVE_DT_ck%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_EDIT_EXECUTIVE_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_EXECUTIVE_DT" runat="server" MaxLength="10" size="10" CssClass="MandatoryField date ymd"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_EXECUTIVE_DT%>"
                                ControlToValidate="txt_NEW_EXECUTIVE_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="qrytest" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sm_Required_EXECUTIVE_DT_ck%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_NEW_EXECUTIVE_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--明細功能--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sm_lb_WFB2SM210Dtl%>" SortExpression="EXECUTIVE_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <aces:Btn ID="WFB2SM2100Detail" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM210Dtl%>"
                                CommandName="detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                            <%--<asp:Button ID="WFB2SM2100Detail" runat="server" Text="<%$Resources:Resource,wfb2sm_WFB2SM210Dtl%>"
                                CommandName="detail" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>">--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <EditRowStyle CssClass="normal" />
                <EmptyDataTemplate>

                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td width="30px"></td>
                            <td width="40px">
                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sm_RowNumber%>"></asp:Label>
                            </td>
                            <td width="120px">
                                <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_DATA_YEAR%>"></asp:Label>
                            </td>
                            <td width="120px">
                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_DATA_SEQ%>"></asp:Label>
                            </td>
                            <td width="130px">
                                <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_GENERATE_DT%>"></asp:Label>
                            </td>
                            <td width="120px">
                                <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_NOTICE_DT%>"></asp:Label>
                            </td>
                            <td width="120px">
                                <asp:Label ID="Label14" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_APPROVE_DT%>"></asp:Label>
                            </td>
                            <td width="120px">
                                <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_RELEASE_DT%>"></asp:Label>
                            </td>
                            <td width="120px">
                                <asp:Label ID="Label16" runat="server" Text="<%$Resources:Resource,wfb2sm_lb_EXECUTIVE_DT%>"></asp:Label>
                            </td>
                            <td width="100px"></td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td>
                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_DATA_YEAR" runat="server" MaxLength="4" size="4" CssClass="MandatoryField year" onblur="return checkYear();"
                                    AutoPostBack="true" OnTextChanged="txt_NEW_DATA_YEAR_TextChanged"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_DATA_YEAR_gv%>"
                                    ControlToValidate="txt_NEW_DATA_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:Label ID="lb_NEW_DATA_SEQ" runat="server" MaxLength="1" size="1"></asp:Label>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txt_NEW_EXECUTIVE_DT" runat="server" MaxLength="10" size="10" CssClass="MandatoryField date"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sm_Required_EXECUTIVE_DT%>"
                                    ControlToValidate="txt_NEW_EXECUTIVE_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="qrytest4" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2sm_Required_EXECUTIVE_DT_ck%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                    ControlToValidate="txt_NEW_EXECUTIVE_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            </td>
                            <td></td>
                        </tr>
                    </table>

                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
            </asp:GridView>


            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr valign="top">

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
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_cover" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_first_load" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_DATA_YEAR" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_DATA_SEQ" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Mod_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Cancel_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
