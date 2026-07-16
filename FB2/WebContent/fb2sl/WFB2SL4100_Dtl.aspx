<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sl/action/WFB2SL4100_Dtl.aspx.cs" Inherits="WebContent_fb2sl_WFB2SL4100_Dtl" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
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
                                <col width="28%" />
                                <col width="21%" />
                                <col width="36%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--年度--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YM" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_DATA_YM%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_DATA_YM_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <%--工號 / 廠商代號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_EMP_ID_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--聘用單位--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_COMPANY_CD" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_COMPANY_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_COMPANY_CD_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <%--姓名 / 廠商名稱--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_EMP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_EMP_NAME_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--部門--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_DEPT_NO_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <%--證號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LICENSE_ID" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_LICENSE_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_LICENSE_ID_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <%--所得總額--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_AMONT" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_AMONT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_AMONT_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <%--代扣稅額--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_TAX" runat="server" Text="<%$Resources:Resource,wfb2sl_lb_TAX%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_TAX_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2sl_btn_back%>" OnClick="btn_back_Click" />
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
                SelectCountMethod="getDtlCount" TypeName="CFB2SL4100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="ods1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_QDATAKEY"
                        Name="hid_qdatakey" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />

                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1019px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <%--序號--%>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sl_RowNumber%>" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Style="text-align: center;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--項目--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_PAY_NAME%>" SortExpression="PAY_KIND" HeaderStyle-Width="200px" ItemStyle-Width="200px"  ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_PAY_NAME" runat="server" Text='<%#Bind("PAY")%>' Style="word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--所得金額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_AMOUNT2%>" HeaderStyle-Width="250px" ItemStyle-Width="250px"  ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_AMOUNT" runat="server" Text='<%#Bind("AMOUNT","{0:n0}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--代扣稅額--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_TAX%>" HeaderStyle-Width="250px" ItemStyle-Width="250px"  ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_TAX" runat="server" Text='<%#Bind("TAX","{0:n0}")%>' Style="text-align: right; word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--薪資發放日--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sl_lb_SALARY_DT2%>" SortExpression="SALARY_DT" HeaderStyle-Width="250px" ItemStyle-Width="250px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_DT" runat="server" Text='<%#Bind("SALARY_DT","{0:yyyy/MM/dd}")%>' Style="text-align: left; word-wrap: break-word;"></asp:Label>
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
            <asp:HiddenField ID="hid_SYS_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_USER_UPD" runat="server" ClientIDMode="static" />
            <asp:HiddenField ID="hid_QDATAKEY" runat="server" ClientIDMode="Static" />

            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


