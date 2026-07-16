<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sp/WFB2SP1100_Dtl.aspx.cs" Inherits="WebContent_WFB2SP1100_Dtl" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">

        $(function () {

            iniForm();
        });
        function iniForm() {
            //日期格式心須
            gridviewScroll();
            $.unblockUI();
        }

        function doUnblock() {
            $.unblockUI();
        }
        function doBlock() {
            BlockUI();
        }

        function ShowRecord(obj) {
            $("#HID_PageRow").val($("#ddlPerPageRow").val());
        }
        //凍結視窗用
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 3

                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }
            else {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F"

                });
            }
        }


        //查詢前檢核
        function CheckSearch() {
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                processed = true;
                BlockUI();
            }
            else {
                processed = false;
            }

            if (!processed)
                $.unblockUI();
            return processed;
        }


    </script>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="15%" />
                    <col width="20%" />
                    <col width="15%" />
                    <col width="20%" />
                    <col width="15%" />
                    <col width="15%" />

                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--工號--%>
                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_emp_id%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_EMP_ID" runat="server" CssClass="txtDisabled" Enabled="false" Width="50px"></asp:Label>
                            <asp:Label ID="txt_EMP_NAME" runat="server" CssClass="txtDisabled" Enabled="false" Width="80px"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--工資總額--%>
                            <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_total_pay%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_PAY_TOTAL" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%-- 平均工資--%>
                            <asp:Label ID="Label4" runat="server" Text=" <%$Resources:Resource,wfb2sp_lb_avg_pay%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_PAY_AVG" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--發放基數--%>
                            <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sp_required_pay_basic%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:Label ID="txt_PAY_BASIC" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                            <asp:Label ID="Label1" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0" Text="(月)"></asp:Label>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--優退獎勵金--%>
                            <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_reward_pay%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_REWARD_PAY" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%-- 發放日期--%>
                            <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_salary_dt%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_SALARY_DT" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--簽核備註--%>
                            <asp:Label ID="Label8" runat="server" Text="簽核備註"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="5">
                            <asp:Label ID="txt_APPROVE_REMARK" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                        </td>

                    </tr>
                    <tr>
                        <td align="right" colspan="6">
                            <asp:Button ID="WFB2SP1100Back" runat="server" Text="返回" OnClick="WFB2SP1100Back_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDtlData"
                SelectCountMethod="getDtlCount" TypeName="CFB2SP1100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                      <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1015px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--薪資年月--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_salary_ym%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_DATA_YM" runat="server" Text='<%#Bind("DATA_YM")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職能俸--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_ability_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" >
                        <ItemTemplate>
                            <asp:Label ID="lb_s1001" runat="server" Text='<%#Bind("s1001","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格俸--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_level_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_s1002" runat="server" Text='<%#Bind("s1002","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--專業津貼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_profession_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" >
                        <ItemTemplate>
                            <asp:Label ID="lb_s1003" runat="server" Text='<%#Bind("s1004","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職務津貼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_pjob_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_s1004" runat="server" Text='<%#Bind("s1003","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--伙食津貼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_food_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" >
                        <ItemTemplate>
                            <asp:Label ID="lb_s1012" runat="server" Text='<%#Bind("s1012","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--調整津貼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_adj_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" >
                        <ItemTemplate>
                            <asp:Label ID="lb_s1053" runat="server" Text='<%#Bind("s1053","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--合計--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_sum_pay2%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" >
                        <ItemTemplate>
                            <asp:Label ID="lb_TOTAL_AMT" runat="server" Text='<%#Bind("TOTAL_AMT","{0:n0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />

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
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_MAX_ASSESS_YEAR" runat="server" ClientIDMode="Static" />
            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
