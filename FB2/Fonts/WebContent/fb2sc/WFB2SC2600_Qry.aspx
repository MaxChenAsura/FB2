<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2600_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2600_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');

            gridviewScroll();
            $.unblockUI();
        }

        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }
        //凍結視窗
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F"

                });
                //  CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }

        }

        //清空
        function doClear() {
            $("#txt_QRY_SALARY_DT_S").val("");
            $("#txt_QRY_SALARY_DT_E").val("");
            $("#txt_QRY_PAY_DT_S").val("");
            $("#txt_QRY_PAY_DT_E").val("");
            $("#txt_QRY_PAY_ID").val("");
            $("#ddl_SALARY_TYPE").val("");
            return false;
        }
        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb299_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <!--查詢條件table(HTML)-->
                        <!--TextBox,input button,DropDownList-->
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="35%" />
                                <col width="15%" />
                                <col width="35%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sk_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_QRY_SALARY_DT_S" runat="server" ClientIDMode="Static" CssClass="date" MaxLength="8" Width="80px" Text=""></asp:TextBox>~
                                     <asp:TextBox ID="txt_QRY_SALARY_DT_E" runat="server" ClientIDMode="Static" CssClass="date" MaxLength="8" Width="80px" Text=""></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_SALARY_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_QRY_SALARY_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_SALARY_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_QRY_SALARY_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_QRY_SALARY_DT_S"
                                            ControlToValidate="txt_QRY_SALARY_DT_E" ErrorMessage="<%$Resources:Resource,wfb2_ERR_SALART_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA">
                                        </asp:CompareValidator>
                                    </td>
                                    <%--發薪類別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAY_DT" runat="server" Text="<%$Resources:Resource,wfb2_lb_PAY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_QRY_PAY_DT_S" runat="server" MaxLength="8" Width="80px" CssClass="date" ClientIDMode="Static" Text=""></asp:TextBox>~
                                     <asp:TextBox ID="txt_QRY_PAY_DT_E" runat="server" MaxLength="8" Width="80px" ClientIDMode="Static" CssClass="date" Text=""></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_PAY_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_QRY_PAY_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_PAY_DT_Format_Error%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_QRY_PAY_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_QRY_PAY_DT_S"
                                            ControlToValidate="txt_QRY_PAY_DT_E" ErrorMessage="<%$Resources:Resource,wfb2_ERR_PAY_DT_RANGE%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA">
                                        </asp:CompareValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAY_ID" runat="server" Text="<%$Resources:Resource,wfb2_lb_PAY_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_QRY_PAY_ID" runat="server" MaxLength="11" Width="100px" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="onlyEngNum" runat="server"
                                            ErrorMessage="關帳代號只能輸入英數字" ControlToValidate="txt_QRY_PAY_ID" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="^[\d|a-zA-Z|-]+$" Display="None"></asp:RegularExpressionValidator>

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left"></th>
                                    <td align="right" class="Body_label"></td>
                                    <th align="left"></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SC2600Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2SC2600Search_Click" ValidationGroup="GroupA" />
                                            <%--<asp:Button ID="WFB2SC2600Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2SC2600Search_Click" ValidationGroup="GroupA" />--%>
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="return doClear();" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4"> 
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left"></th>
                                    <td align="right" class="Body_label"></td>
                                    <th align="left"></th>
                                    <td align="right" class="Body_label">
                                        <div id="Div1">
                                            <aces:Btn ID="WFB2SC2600Detail" runat="server" Text="生成傳票" ClientIDMode="Static" OnClientClick="BlockUI();" OnClick="WFB2SC2600Detail_Click" />                                            
                                            <aces:Btn ID="WFB2SC2600Detail2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2600Detail2%>" ClientIDMode="Static" OnClientClick="return CheckDtlAction();" OnClick="WFB2SC2600Detail2_Click" />
                                            <asp:Button ID="WFB2SC2600EXESAP" runat="server" Text="上傳SAP傳票" ClientIDMode="Static" OnClientClick="BlockUI();" OnClick="WFB2SC2600EXESAP_Click" />
                                            <aces:Btn ID="WFB2SC2600Execute2" runat="server" Text="<%$Resources:Resource,wfb2_bt_WFB2SC2600Execute2%>" ClientIDMode="Static" OnClientClick="BlockUI();"  OnClick="WFB2SC2600Execute2_Click" />
                                            
                                            <%--<asp:Button ID="WFB2SC2600Detail" runat="server" Text="<%$Resources:Resource,wfb2_bt_WFB2SC2600Execute1%>" ClientIDMode="Static" OnClientClick="BlockUI();" OnClick="WFB2SC2600Detail_Click" />
                                            <asp:Button ID="WFB2SC2600Detail2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2600Detail2%>" ClientIDMode="Static" OnClientClick="return CheckDtlAction();" OnClick="WFB2SC2600Detail2_Click" />
                                                <asp:Button ID="WFB2SC2600Execute2" runat="server" Text="<%$Resources:Resource,wfb2_bt_WFB2SC2600Execute2%>" ClientIDMode="Static" OnClientClick="BlockUI();" OnClick="WFB2SC2600Execute2_Click" />--%>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData" SelectCountMethod="GetCount" TypeName="CFB2SC2600DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting" StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_QRY_SALARY_DT_S" DefaultValue=""
                        Name="qry_salary_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_QRY_SALARY_DT_E" DefaultValue=""
                        Name="qry_salary_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_SALARY_TYPE"
                        Name="salary_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_QRY_PAY_DT_S" DefaultValue=""
                        Name="qry_pay_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_QRY_PAY_DT_E" DefaultValue=""
                        Name="qry_pay_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_QRY_PAY_ID" DefaultValue=""
                        Name="qry_pay_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
                <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="25px">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="text-align: center">
                                <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2si_RowNum%>" ItemStyle-Width="35px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>" SortExpression="SALARY_TYPE_NAME" ItemStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_SALARY_TYPE_NAME" runat="server" Text='<%#Bind("SALARY_TYPE_NAME")%>'></asp:Label>
                            <asp:HiddenField ID="lb_SALARY_TYPE" runat="server" ClientIDMode="Static" Value='<%#Bind("SALARY_TYPE")%>' />
                            <asp:HiddenField ID="lb_IACYC" runat="server" ClientIDMode="Static" Value='<%#Bind("IACYC")%>' />
                            <asp:HiddenField ID="lb_Lno" runat="server" ClientIDMode="Static" Value='<%#Bind("Lno")%>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_YM%>" SortExpression="SALARY_YM" ItemStyle-Width="65px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_SALARY_YM" runat="server" Text='<%#Bind("SALARY_YM", "{0:yyyy/MM}")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>" SortExpression="SALARY_DT" ItemStyle-Width="85px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_SALARY_DT" runat="server" Text='<%#Bind("SALARY_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_PAY_KIND%>" SortExpression="PAY_KIND_NAME" ItemStyle-Width="75px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_PAY_KIND_NAME" runat="server" Text='<%#Bind("PAY_KIND_NAME")%>'></asp:Label>
                            <asp:HiddenField ID="lb_PAY_KIND" runat="server" ClientIDMode="Static" Value='<%#Bind("PAY_KIND")%>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS%>" SortExpression="PROCESS_STATUS_NAME" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_PROCESS_STATUS_NAME" runat="server" Text='<%#Bind("PROCESS_STATUS_NAME")%>'></asp:Label>
                            <asp:HiddenField ID="lb_PROCESS_STATUS" runat="server" ClientIDMode="Static" Value='<%#Bind("PROCESS_STATUS")%>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2_lb_PAY_ID%>" SortExpression="PAY_ID" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_PAY_ID" runat="server" Text='<%#Bind("PAY_ID")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2_lb_PAY_DT%>" SortExpression="PAY_DT" ItemStyle-Width="85px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_PAY_DT" runat="server" Text='<%#Bind("PAY_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2_lb_EMAIL_DT%>" SortExpression="EMAIL_DT" ItemStyle-Width="90px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_EMAIL_DT" runat="server" Text='<%#Bind("EMAIL_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2_lb_CLOSE_DT%>" SortExpression="CLOSED_DT" ItemStyle-Width="85px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_CLOSED_DT" runat="server" Text='<%#Bind("CLOSED_DT","{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2_lb_IS_VOUCHER%>" SortExpression="IS_VOUCHER" ItemStyle-Width="80px"  >
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_IS_VOUCHER" runat="server" Text='<%#Bind("IS_VOUCHER")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2_lb_IS_SAP%>" SortExpression="IS_SAP" ItemStyle-Width="80px"  >
                        <ItemTemplate>
                            <asp:Label Style="text-align: left;" ID="lb_IS_SAP" runat="server" Text='<%#Bind("IS_SAP")%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left" />
                    </asp:TemplateField>
                </Columns>
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
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />

             <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Dtl_NotChoiceMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
