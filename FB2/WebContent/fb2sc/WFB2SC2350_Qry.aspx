<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2350_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2350_Qry" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            //getYearMonth()
            iniForm();
        });
        function iniForm() {
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
        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function ClearAll() {
            $('#ddl_SALARY_TYPE_search').find(":selected").attr('selected', false);
            $('#ddl_SALARY_TYPE_search').val("");
            //$('#txt_SALARY_DT_search').val($('#hid_salary_dt_search').val());
            $('#txt_SALARY_DT_search').val("");
            $('#txt_PAY_KIND_search').val("");
            $('#txt_SALARY_NAME_search').val("");
            return true;
        }
        function paykindCheck(value) {
            document.getElementById('<%=btn_PayKind.ClientID%>').click();
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
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="15%" />
                                <col width="35%" />
                                <col width="15%" />
                                <col width="35%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--發薪類別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_TYPE_search" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_TYPE_search" runat="server" class="MandatoryField" ClientIDMode="Static"
                                            OnSelectedIndexChanged="ddl_SALARY_TYPE_search_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sf_required_ddl_SALARY_TYPE%>"
                                            ControlToValidate="ddl_SALARY_TYPE_search" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <%--發薪日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_DT_search" runat="server" MaxLength="8" CssClass="ymd date MandatoryField" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator0" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_isNull%>"
                                            ControlToValidate="txt_SALARY_DT_search" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator6" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_SALARY_DT_search" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%--發放項目--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PAY_KIND%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_PAY_KIND_search" runat="server" MaxLength="10" Width="64px" class="MandatoryField" ClientIDMode="Static" onblur="paykindCheck(this.value);"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_PAY_KIND_isNull%>"
                                            ControlToValidate="txt_PAY_KIND_search" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <input id="bt_PAY_KIND_SEARCH" type="button" value="..." onclick="OpenSearch('Salary9999_Search.aspx', 'txt_PAY_KIND_search', 'txt_SALARY_NAME_search', '', '');" />
                                    </td>
                                    <%--項目名稱--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_NAME_search" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_NAME_search" runat="server" MaxLength="20" ClientIDMode="Static" Width="100px"  Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_label">
                                    </th>
                                    <td align="left" class="Body_label" colspan="2">
                                    </td>
                                    <td align="right" class="Body_label" colspan="3">
                                        <div id="init">
                                            <%--<aces:Btn ID="WFB2SC2350Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Search%>" OnClick="WFB2SC2300Search1_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                                            <asp:Button ID="WFB2SC2350Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Search%>" OnClick="WFB2SC2350Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_clear%>" OnClientClick="return ClearAll();" OnClick="ClearAll_Click"/>
                                            <asp:Button ID="WFB2SC2350Edit"  runat ="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2350Edit%>"   OnClick ="WFB2SC2350Edit_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
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
                <tr>
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <tr>
                                <th align="left" style="width: 10%;">
                                    <asp:Label ID="lb_SALARY_PAY_T" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PAY_T_CNT%>" Visible="False"></asp:Label>
                                </th>
                                <td align="left" style="width: 45%;">
                                    <asp:Label ID="lb_SALARY_PAY_T_CNT" runat="server" MaxLength="50" Width="50px" ClientIDMode="Static" Text="" Visible="false"></asp:Label>
                                </td>
                                <th align="left" style="width: 10%;">
                                    <asp:Label ID="lb_SALARY_PAY_C" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PAY_C_CNT%>" Visible="False"></asp:Label>
                                </th>
                                <td align="left" style="width: 45%;">
                                    <asp:Label ID="lb_SALARY_PAY_C_CNT" runat="server" MaxLength="50" Width="50px" ClientIDMode="Static" Text="" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr></tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SC2350DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_SALARY_DT_search" DefaultValue=""
                        Name="salary_dt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_SALARY_TYPE_search" DefaultValue=""
                        Name="salary_type" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_PAY_KIND_search" DefaultValue=""
                        Name="pay_kind" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <%--工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_EMP_NAME2%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label Width="100%" Style="text-align: left;" ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
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

            <asp:Button ID="btn_PayKind" runat="server" OnClick="btn_PayKind_Click" Style="display: none" />

            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

            <asp:HiddenField ID="hid_salary_dt_search" runat="server" ClientIDMode="Static" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Cancel_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


