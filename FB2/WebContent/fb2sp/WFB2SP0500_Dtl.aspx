<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sp/WFB2SP0500_Dtl.aspx.cs" Inherits="WebContent_WFB2SP0500_Dtl" Culture="auto" UICulture="auto" %>

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
                    <col width="15%" />
                    <col width="15%" />
                    <col width="15%" />
                    <col width="15%" />
                    <col width="15%" />
                    <col width="10%" />

                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--計算類別--%>
                            <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_computer_type%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_COMPUTER_TYPE_DESC" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                            <asp:HiddenField ID="HID_COMPUTER_TYPE" runat="server" ClientIDMode="Static" />
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--工號--%>
                            <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_emp_id%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_EMP_ID" runat="server" CssClass="txtDisabled" Enabled="false" Width="50px"></asp:Label>
                            <asp:Label ID="txt_EMP_NAME" runat="server" CssClass="txtDisabled" Enabled="false" Width="80px"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--退休日--%>
                            <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_retire_dt%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:Label ID="txt_RETIRE_DT" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--工資總額--%>
                            <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_total_pay%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_TOTAL_PAY" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%-- 平均工資--%>
                            <asp:Label ID="Label4" runat="server" Text=" <%$Resources:Resource,wfb2sp_lb_avg_pay%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_AVG_PAY" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--在職年資--%>
                            <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_work_years%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:Label ID="txt_WORK_YEARS" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--舊制年資--%>
                            <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_oldretire_years%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_OLDRETIRE_YEARS" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%-- 核可舊制年資--%>
                            <asp:Label ID="Label7" runat="server" Text=" <%$Resources:Resource,wfb2sp_lb_apr_oldretire_years%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_APR_OLDRETIRE_YEARS" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--退休基數--%>
                            <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_retire_base_month%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:Label ID="txt_RETIRE_BASE_MONTH" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <%--退休金--%>
                            <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_retire_pay_tmp%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_RETIRE_PAY_TMP" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%-- 實際退休金--%>
                            <asp:Label ID="Label10" runat="server" Text=" <%$Resources:Resource,wfb2sp_lb_retire_pay%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="txt_RETIRE_PAY" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:Label>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <%--結案否--%>
                            <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2sp_lb_close_yn%>"></asp:Label>:
                        </th>
                        <td>
                            <asp:DropDownList ID="ddl_CLOSE_YN" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td align="right" colspan="7">
                            <aces:Btn ID="WFB2SP0500Save" runat="server" Text="儲存" OnClick="WFB2SP0500Save_Click" />
                            <asp:Button ID="WFB2SP0500Back" runat="server" Text="返回" OnClick="WFB2SP0500Back_Click" />
                            <%--
                            <asp:Button ID="WFB2SP0500Save" runat="server" Text="儲存" OnClick="WFB2SP0500Save_Click" OnClientClick="return CheckSearch();" />
                            --%>
                            
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <hr />
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDtlData"
                SelectCountMethod="getDtlCount" TypeName="CFB2SP0500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="HID_COMPUTER_TYPE"
                        Name="computer_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnRowCommand="gv_result_RowCommand" OnDataBound="gv_result_DataBound">
                <Columns>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--薪資年月--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_salary_ym%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="SALARY_YM">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_YM" runat="server" Text='<%#Bind("SALARY_YM")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--日曆天數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_calendar_day%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="CALENDAR_DAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_CALENDAR_DAY" runat="server" Text='<%#Bind("CALENDAR_DAY")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--病假天數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_leave_b_day%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="LEAVE_B_DAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_B_DAY" runat="server" Text='<%#Bind("LEAVE_B_DAY")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--計算日數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_computer_day%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="COMPUTER_DAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_COMPUTER_DAY" runat="server" Text='<%#Bind("COMPUTER_DAY")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職能俸--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_ability_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="ABILITY_PAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_ABILITY_PAY" runat="server" Text='<%#Bind("ABILITY_PAY","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格俸--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_level_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="LEVEL_PAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_PAY" runat="server" Text='<%#Bind("LEVEL_PAY","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--專業俸--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_profession_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="PROFESSION_PAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_PROFESSION_PAY" runat="server" Text='<%#Bind("PROFESSION_PAY","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職務津貼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_pjob_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="PJOB_PAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_PAY" runat="server" Text='<%#Bind("PJOB_PAY","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--伙食津貼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_food_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="FOOD_PAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_FOOD_PAY" runat="server" Text='<%#Bind("FOOD_PAY","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--調整津貼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_adj_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="ADJ_PAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_ADJ_PAY" runat="server" Text='<%#Bind("ADJ_PAY","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--特勤津貼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_special_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="SPECIAL_PAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_SPECIAL_PAY" runat="server" Text='<%#Bind("SPECIAL_PAY","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--小計--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_sum_pay%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="SUM_PAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_SUM_PAY" runat="server" Text='<%#Bind("SUM_PAY","{0:n0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--比例計算--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_sum_pay_byday%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="SUM_PAY_BYDAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_SUM_PAY_BYDAY" runat="server" Text='<%#Bind("SUM_PAY_BYDAY","{0:n0}")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--眷屬津貼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_famialy_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="FAMIALY_PAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_FAMIALY_PAY" runat="server" Text='<%#Bind("FAMIALY_PAY","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--輪班津貼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_work_shift_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="WORK_SHIFT_PAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_SHIFT_PAY" runat="server" Text='<%#Bind("WORK_SHIFT_PAY","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--環境津貼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_env_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="ENV_PAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_ENV_PAY" runat="server" Text='<%#Bind("ENV_PAY","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--勤務地津貼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_plant_pay%>" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Right" SortExpression="PLANT_PAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_PLANT_PAY" runat="server" Text='<%#Bind("PLANT_PAY","{0:n0}")%>' Width="90px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--加班津貼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_overtime_pay%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="OVERTIME_PAY">
                        <ItemTemplate>
                            <asp:Label ID="lb_OVERTIME_PAY" runat="server" Text='<%#Bind("OVERTIME_PAY","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--事假時數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_leave_a_hours%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="LEAVE_A_HOURS">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_A_HOURS" runat="server" Text='<%#Bind("LEAVE_A_HOURS","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--事假扣款--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_leave_a_amt%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="LEAVE_A_AMT">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_A_AMT" runat="server" Text='<%#Bind("LEAVE_A_AMT","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--曠職時數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_leave_q_hours%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="LEAVE_Q_HOURS">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_Q_HOURS" runat="server" Text='<%#Bind("LEAVE_Q_HOURS","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--曠職扣款--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_leave_q_amt%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="LEAVE_Q_AMT">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_Q_AMT" runat="server" Text='<%#Bind("LEAVE_Q_AMT","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--遲到早退次數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_leave_op_times%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="LEAVE_OP_TIMES">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_OP_TIMES" runat="server" Text='<%#Bind("LEAVE_OP_TIMES","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--遲到早退扣款--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_leave_op_amt%>" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right" SortExpression="LEAVE_OP_AMT">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_OP_AMT" runat="server" Text='<%#Bind("LEAVE_OP_AMT","{0:n0}")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--合計--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sp_lb_sum_pay2%>" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" SortExpression="SUM_PAY2">
                        <ItemTemplate>
                            <asp:Label ID="lb_SUM_PAY2" runat="server" Text='<%#Bind("SUM_PAY2","{0:n0}")%>' Width="80px"></asp:Label>
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
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
