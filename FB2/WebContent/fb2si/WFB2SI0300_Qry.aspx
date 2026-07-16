<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2si/WFB2SI0300_Qry.aspx.cs" Inherits="WebContent_fb2si_WFB2SI0300_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.year').mask('9999');
            $(".ymd").mask('9999/99/99');

            gridviewScroll();
            $.unblockUI();

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
        function CheckYM(source, arguments) {
            if ($("#txt_Year_DT_S").val() != "" && $("#txt_Year_DT_E").val() != "") {
                if ($("#txt_Year_DT_E").val() < $("#txt_Year_DT_S").val())
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }

        }
        //清空
        function doClear() {
            $("#txt_Year_DT_S").val("");
            $("#txt_Year_DT_E").val("");
            return false;
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
                                <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="5%" />
                                <col width="15%" />
                                <col width="20%" />
                                <col width="20%" />

                            </colgroup>
                            <tbody>

                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BONUS_YEAR" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_YEAR%>"></asp:Label>
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_Year_DT_S" runat="server" MaxLength="4" Width="64px" CssClass="year" ClientIDMode="Static" Text=""></asp:TextBox>~
                                            <asp:TextBox ID="txt_Year_DT_E" runat="server" MaxLength="4" Width="64px" CssClass="year" ClientIDMode="Static" Text=""></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2si_error_BONUS_YEAR_start%>" ControlToValidate="txt_Year_DT_S" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2si_error_BONUS_YEAR_end%>" ControlToValidate="txt_Year_DT_E" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2si_greaterthan_BONUS_YEAR%>" ClientValidationFunction="CheckYM" ForeColor="Red"
                                            ControlToValidate="txt_Year_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>


                                        <%--<uc1:UCDateTextBoxRangeascx ID="UCDateTextBoxRangeascx" runat="server" ClientIDMode="Static" ControlClientIDMode="Static"
                                            EndDateValidationExpression="(19|20)\d\d" StartDateValidationExpression="(19|20)\d\d" CompareValidatorOperator="GreaterThanEqual"
                                            CompareValidatorErrMesg="<%$Resources:Resource,wfb2si_greaterthan_BONUS_YEAR%>" ValidatorStartDateErrMesg="<%$Resources:Resource,wfb2si_error_BONUS_YEAR_start%>"
                                            ValidatorEndDateErrMesg="<%$Resources:Resource,wfb2si_error_BONUS_YEAR_end%>" StartDateRequire="false" EndDateRequire="false"
                                            ValidationGroup="GroupA" MaskChange="9999" StartDateMaxLength="4" EndDateMaxLengt="4" StartDateCssClass="year" EndDateCssClass="year" />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <%--<aces:Btn ID="WFB2SI0300Search" runat="server" Text="<%$Resources:Resource,wfb2si_search%>" OnClick="WFB2SI0300Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>

                                            <asp:Button ID="WFB2SI0300Search" runat="server" Text="<%$Resources:Resource,wfb2si_search%>" OnClick="WFB2SI0300Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2si_clear%>" OnClientClick="return doClear();" />
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
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetData"
                SelectCountMethod="GetCount" TypeName="CFB2SI0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">

                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_Year_DT_S" DefaultValue=""
                        Name="bonus_year_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_Year_DT_E" DefaultValue=""
                        Name="bonus_year_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>


            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">

                <Columns>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_YEAR%>" SortExpression="BONUS_YEAR">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: center;" ID="lb_BONUS_YEAR" runat="server" Text='<%#Bind("BONUS_YEAR")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_grid_APPROVE_STATUS%>" SortExpression="APPROVE_STATUS">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_APPROVE_STATUS" runat="server" Text='<%#Bind("APPROVE_STATUS")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_grid_BONUS_DAYS%>" SortExpression="BONUS_DAYS">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: right;" ID="lb_BONUS_DAYS" runat="server" Text='<%#Bind("BONUS_DAYS")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_BONUS_DT%>" SortExpression="BONUS_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_BONUS_DT" runat="server" Text='<%#Bind("BONUS_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_RELEASE_BY%>" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_RELEASE_BY" runat="server" ClientIDMode="Static" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_RELEASE_DT%>" SortExpression="RELEASE_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_RELEASE_DT" runat="server" Text='<%#Bind("RELEASE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_APPROVE_BY%>" SortExpression="APPROVE_BY">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_APPROVE_BY" runat="server" ClientIDMode="Static" Text='<%#Bind("APPROVE_BY")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_APPROVE_DT%>" SortExpression="APPROVE_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_APPROVE_DT" runat="server" Text='<%#Bind("APPROVE_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2si_SALARY_TRANS_DT%>" SortExpression="SALARY_TRANS_DT">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_SALARY_TRANS_DT" runat="server" Text='<%#Bind("SALARY_TRANS_DT", "{0:yyyy/MM/dd}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="明細功能" HeaderStyle-Width="80px">
                        <ItemTemplate>
                           <%-- <aces:Btn ID="WFB2SI0300Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2si_Dtl%>" />--%>

                            <asp:Button ID="WFB2SI0300Detail" runat="server"
                                CommandName="ToDetail"
                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                Text="<%$Resources:Resource,wfb2si_Dtl%>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>

                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:Label ID="Labe2" runat="server" Text="<%$Resources:Resource,wfb2si_YEAR%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Labe3" runat="server" Text="<%$Resources:Resource,wfb2si_grid_APPROVE_STATUS%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="<%$Resources:Resource,wfb2si_grid_BONUS_DAYS%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2si_BONUS_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2si_RELEASE_BY%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="<%$Resources:Resource,wfb2si_RELEASE_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2si_APPROVE_BY%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="<%$Resources:Resource,wfb2si_APPROVE_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2si_SALARY_TRANS_DT%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="明細功能"></asp:Label>
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
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            
            

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
