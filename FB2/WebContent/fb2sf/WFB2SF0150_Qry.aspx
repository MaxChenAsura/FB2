<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sf/WFB2SF0150_Qry.aspx.cs" Inherits="WebContent_fb2sf_WFB2SF0150_Qry" Culture="auto" UICulture="auto"%>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <script type="text/javascript">

        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.ym').mask('9999/99/99');
            gridviewScroll();
            $.unblockUI();
        }

        function ShowRecord(obj) {
            if (obj == "ddlPerPageRow")
                $("#HID_PageRow").val($("#ddlPerPageRow").val());
            if (obj == "ddlPerPageRow2")
                $("#HID_PageRow2").val($("#ddlPerPageRow2").val());
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
            $('#<%=gv_result2.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        function CheckApproveAction() {
            if (LookUpCheckboxs() > 0) {
                for (var i = 0; i < ItemCheckBoxs.length; i++) {
                    if (choietr[i] != null) {
                        var ck = choietr[i] - 2;
                        if (ck >= 0 && $("[id=hid_APPROVE_STATUS]")[ck].value != "N") {
                            alert($('#hid_wfb2sf_APPROVE_STATUS_RequiredNMessage').val());
                            return false;
                        }
                        
                    }
                }
                return confirm('確定要提出核可?');
            }
            else {
                alert($('#hid_wfb2sf_App_NotChoiceMessage').val());
                return false;
            }
        }
        function CheckRejectAction() {
            if (LookUpCheckboxs() > 0) {
                for (var i = 0; i < ItemCheckBoxs.length; i++) {
                    if (choietr[i] != null) {
                        var ck = choietr[i] - 2;
                        if (ck >= 0 && $("[id=hid_APPROVE_STATUS]")[ck].value != "N") {
                            alert($('#hid_wfb2sf_APPROVE_STATUS_RequiredNMessage1').val());
                            return false;
                        }
                        if (ck >= 0 && $("[id=txt_APP_REMARK]")[ck].value == "") {
                            alert($('#hid_wfb2sf_APP_REMARK_RequiredMessage').val() + $("[id=lb_EMP_NAME]")[ck].innerText + $('#hid_wfb2sf_APP_REMARK_RequiredMessage1').val());
                            return false;
                        }
                    }
                }
                return confirm('確定要提出駁回?');
            }
            else {
                alert($('#hid_wfb2sf_Rej_NotChoiceMessage').val());
                return false;
            }
        }
        var choietr = [];
        var ItemCheckBoxs = null;
        //檢查勾了幾個checkbox
        function LookUpCheckboxs() {
            //choietr = null;
            ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                choietr[i] = null;
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    if (choietr[i] == null)
                        choietr[i] = i;
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function CheckAPPROVE_STATUS(source, arguments) {
            if ($("#ddl_APPROVE_STATUS").val() == -1)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

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
                                <col width="15%" />
                                <col width="30%" />
                                <col width="15%" />
                                <col width="5%" />
                                <col width="10%" />
                                <col width="25%" />
                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_APPROVE_STATUS" runat="server" Text="<%$Resources:Resource,wfb2ia_APPROVE_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_APPROVE_STATUS" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2ia_Rrequired_APPROVE_STATUS%>" ClientValidationFunction="CheckAPPROVE_STATUS" ForeColor="Red"
                                            ControlToValidate="ddl_APPROVE_STATUS" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SF0150Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2SF0150Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <aces:Btn ID="WFB2SF0150Approve" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF0150Approve%>" OnClick="WFB2SF0150Approve_Click" ValidationGroup="GroupA" OnClientClick="return CheckApproveAction();" />
                                            <aces:Btn ID="WFB2SF0150Reject" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF0150Reject%>" OnClick="WFB2SF0150Reject_Click" ValidationGroup="GroupA" OnClientClick="return CheckRejectAction();" />

                                            <%--<asp:Button ID="WFB2SF0150Search" runat="server" Text="<%$Resources:Resource,wfb2sk_search%>" OnClick="WFB2SF0150Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <asp:Button ID="WFB2SF0150Approve" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF0150Approve%>" OnClick="WFB2SF0150Approve_Click" ValidationGroup="GroupA" OnClientClick="return CheckApproveAction();" />
                                            <asp:Button ID="WFB2SF0150Reject" runat="server" Text="<%$Resources:Resource,wfb2sf_WFB2SF0150Reject%>" OnClick="WFB2SF0150Reject_Click" ValidationGroup="GroupA" OnClientClick="return CheckRejectAction();" />--%>
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

            <!--grid1-->
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                SelectCountMethod="GetCount" TypeName="CFB2SF0150DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_APPROVE_STATUS" DefaultValue=""
                        Name="approve_status" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ia_EMP_ID%>" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_EMP_NAME%>" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" ClientIDMode="Static" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_DOC_NO%>" SortExpression="DOC_NO">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_DOC_NO" runat="server" Text='<%#Bind("DOC_NO")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_START_DT%>" SortExpression="START_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_AMOUNT2%>" SortExpression="AMOUNT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_AMOUNT" runat="server" Text='<%#Bind("AMOUNT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_TOTAL_AMT2%>" SortExpression="TOTAL_AMT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_TOTAL_AMT" runat="server" Text='<%#Bind("TOTAL_AMT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_OPELATION_BY_NAME%>" SortExpression="OPELATION_BY_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_OPELATION_BY_NAME" runat="server" Text='<%#Bind("OPELATION_BY_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_APP_REMARK%>" SortExpression="APP_REMARK">
                        <ItemTemplate>
                            <asp:TextBox Width="90%" Style="text-align: left;" ID="txt_APP_REMARK" MaxLength="100"  ClientIDMode="Static" runat="server" Text='<%#Bind("APP_REMARK")%>'></asp:TextBox>
                            <asp:HiddenField ID="hid_APPROVE_STATUS" runat="server" ClientIDMode="Static" Value='<%#Bind("APPROVE_STATUS")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_function%>">
                        <ItemTemplate>
                            <aces:Btn ID="WFB2SF0150Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2sf_WFB2SF0150Detail%>" />

                            <%--<asp:Button ID="WFB2SF0150Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2sf_WFB2SF0150Detail%>" />--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('ddlPerPageRow')" ClientIDMode="Static" AutoPostBack="true">
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

            <!--grid2-->
            <asp:ObjectDataSource ID="ods2" runat="server" SelectMethod="GetData2"
                SelectCountMethod="GetCount2" TypeName="CFB2SF0150DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting2"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected2">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="HID_EMP_ID" DefaultValue=""
                        Name="EMP_ID" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="HID_DOC_NO" DefaultValue=""
                        Name="DOC_NO" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <br />
            <asp:GridView ID="gv_result2" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" HeaderStyle-Width="40px" >
                        <ItemTemplate>
                            <asp:Label Width="40px" Style="text-align: center;" ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_CHG_STATUS%>" SortExpression="CHG_STATUS" HeaderStyle-Width="60px" >
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_CHG_STATUS" runat="server" Text='<%#Bind("CHG_STATUS")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="60px" >
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_DOC_NO%>" SortExpression="DOC_NO" HeaderStyle-Width="150px" >
                        <ItemTemplate>
                            <asp:Label Width="150px" Style="text-align: left;" ID="lb_DOC_NO" runat="server" Text='<%#Bind("DOC_NO")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_PAY_TARGET%>" SortExpression="PAY_TARGET" HeaderStyle-Width="60px" >
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_PAY_TARGET" runat="server" Text='<%#Bind("PAY_TARGET")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_CREDITOR%>" SortExpression="CREDITOR" HeaderStyle-Width="60px" >
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: left;" ID="lb_CREDITOR" runat="server" Text='<%#Bind("CREDITOR")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_AMOUNT2%>" SortExpression="AMOUNT" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: right;" ID="lb_AMOUNT" runat="server" Text='<%#Bind("AMOUNT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_TOTAL_AMT21%>" SortExpression="TOTAL_AMT" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="60px" Style="text-align: right;" ID="lb_TOTAL_AMT" runat="server" Text='<%#Bind("TOTAL_AMT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_MEMO%>" SortExpression="MEMO" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Width="100px" Style="text-align: left;" ID="lb_MEMO" runat="server" Text='<%#Bind("MEMO")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_EFFECT_EDT%>" SortExpression="EFFECT_EDT" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_EFFECT_EDT" runat="server" Text='<%#Bind("EFFECT_EDT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_IS_VAILD%>" SortExpression="IS_VAILD" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="80px" Style="text-align: left;" ID="lb_IS_VAILD" runat="server" Text='<%#Bind("IS_VAILD")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_MEMODESC%>" SortExpression="MEMODESC" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label Width="120px" Style="text-align: left;" ID="lb_MEMODESC" runat="server" Text='<%#Bind("MEMODESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage2" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow2" runat="server" onchange="javascript:ShowRecord('ddlPerPageRow2')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                            <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                            <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                            <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                            <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount2" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>

            <!--紀錄頁數(跳頁用)-->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow2" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_EMP_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_DOC_NO" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <!--resource)-->
            <asp:HiddenField ID="hid_wfb2sf_App_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_App_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sf_Rej_NotChoiceMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_Rej_NotChoiceMessage%>" />
            <asp:HiddenField ID="hid_wfb2sf_APPROVE_STATUS_RequiredNMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_APPROVE_STATUS_RequiredNMessage%>" />
            <asp:HiddenField ID="hid_wfb2sf_APPROVE_STATUS_RequiredNMessage1" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_APPROVE_STATUS_RequiredNMessage1%>" />
            <asp:HiddenField ID="hid_wfb2sf_APP_REMARK_RequiredMessage" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_APP_REMARK_RequiredMessage%>" />
            <asp:HiddenField ID="hid_wfb2sf_APP_REMARK_RequiredMessage1" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfb2sf_APP_REMARK_RequiredMessage1%>" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>