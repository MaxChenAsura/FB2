<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2100_Dtl.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2100_Dtl" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $('.ym').mask('9999/99');
            gridviewScroll();
            $.unblockUI();
        }
        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        function doUnBlock() {
            $.unblockUI();
        }
        function checkExcute1() {
            if (confirm($('#hidwfb2sc_Execute1_ConfirmMessage').val())) {
                BlockUI();
                return true;
            }
            else {
                $.unblockUI();
                return false;
            }
        }
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
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
                                        <asp:Label ID="lb_SALARY_TYPE_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <%--薪資項目--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_ID%>"></asp:Label>
                                    </th>
                                    <td>
                                        <asp:Label ID="lb_PAY_KIND" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--發薪日期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_SALARY_DT_txt" runat="server"></asp:Label>
                                    </td>
                                    <%--薪資年月--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_YM" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_YM2%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_SALARY_YM_txt" CssClass="ym" runat="server" ClientIDMode="Static"></asp:Label>

                                    </td>
                                </tr>
                                <tr>
                                    <%--計薪日期區間--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_SDT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_DT_CAL%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_SALARY_SDT_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                        <asp:Label ID="lb_mark1" runat="server" ClientIDMode="Static"></asp:Label>
                                        <asp:Label ID="lb_SALARY_EDT_txt" runat="server" ClientIDMode="Static"></asp:Label>

                                    </td>
                                    <%--考勤日期區間--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DUTY_DT" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_DUTY_SDT_TO_EDT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_DUTY_SDT_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                        <asp:Label ID="lb_mark2" runat="server" ClientIDMode="Static"></asp:Label>
                                        <asp:Label ID="lb_DUTY_EDT_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--入帳週期--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IACYC" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_IACYC%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan ="3">
                                        <asp:Label ID="lb_IACYC_txt" CssClass="ym" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>                                    
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <aces:Btn ID="WFB2SC2100Detail2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2100Detail2%>" OnClick="WFB2SC2100Detail2_Click" />
                            <aces:Btn ID="WFB2SC2100Execute1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2100Execute1%>" OnClick="WFB2SC2100Execute1_Click" OnClientClick="return checkExcute1();"/>
                            <aces:Btn ID="WFB2SC2100Execute2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2100Execute2%>" OnClick="WFB2SC2100Execute2_Click" OnClientClick="return checkDowning(this.value);" />
                            <%--<asp:Button ID="WFB2SC2100Detail2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2100Detail2%>" OnClick="WFB2SC2100Detail2_Click" />
                            <asp:Button ID="WFB2SC2100Execute1" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2100Execute1%>" OnClick="WFB2SC2100Execute1_Click" OnClientClick="return checkExcute1();"/>
                            <asp:Button ID="WFB2SC2100Execute2" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2100Execute2%>" OnClick="WFB2SC2100Execute2_Click" OnClientClick="return checkDowning(this.value);"/>--%>
                            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,btn_back%>" OnClick="btn_back_Click" />
                        </div>

                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDtlData"
                SelectCountMethod="getDtlCount" TypeName="CFB2SC2100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="ods1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_salary_dt"
                        Name="salary_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_salary_type"
                        Name="salary_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_pay_kind"
                        Name="pay_kind" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb299_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lbl_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <%--薪資項目代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_ID%>" SortExpression="SALARY_ID" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_ID" runat="server" Text='<%#Bind("SALARY_ID")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--薪資項目名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_NAME%>" SortExpression="SALARY_NAME" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_NAME" runat="server" Text='<%#Bind("SALARY_NAME")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--薪資項目類別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_SALARY_CD%>" SortExpression="SALARY_CD" HeaderStyle-Width="300px" ItemStyle-Width="300px">
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

            <asp:HiddenField ID="hid_salary_dt" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_salary_type" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_process_status" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_pay_kind" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Execute1_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Execute1_ConfirmMessage%>" />
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SC2100Detail2" />
             <asp:PostBackTrigger ControlID="WFB2SC2100Execute1" />
             <asp:PostBackTrigger ControlID="WFB2SC2100Execute2" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


