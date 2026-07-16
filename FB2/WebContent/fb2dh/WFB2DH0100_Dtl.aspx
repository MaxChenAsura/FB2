<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0100_Dtl.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0100_Dtl" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            //$(document).tooltip();
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
                barcolor: "#7F7F7F",
                freezesize: 4
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        //清空畫面
        function ClearAll() {
            $("#ddl_IS_IFLOW_SHOW").val("-1");
            $("#ddl_IS_USED").val("-1");
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm("確定要刪除?");
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function openQry() {
            window.location.href("WFB2DH0100_Qry.aspx");
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
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
                                        <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_MAIN_LEAVE_CD" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_IFLOW_SHOW" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IS_IFLOW_SHOW%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_IFLOW_SHOW" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_USED" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IS_USED%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_USED" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DH0101Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0101Search%>" OnClientClick="BlockUI();" OnClick="WFB2DH0101Search_Click" />

                                            <%--<asp:Button ID="WFB2DH0101Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0101Search%>" OnClientClick="BlockUI();" OnClick="WFB2DH0101Search_Click" />--%>
                                            
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dh_btn_clear%>" onclick="ClearAll();" />
                                            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2dh_btn_back%>" OnClick="btn_back_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td align="right" class="Body_label">
                        <hr>
                        <div id="init_grid">
                        <aces:Btn ID="WFB2DH0101Add" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0101Add%>" OnClick="WFB2DH0101Add_Click" />
                            <aces:Btn ID="WFB2DH0101Delete" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0101Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DH0101Delete_Click" />
                            <aces:Btn ID="WFB2DH0101Edit" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0101Edit%>" Visible="false" OnClick="WFB2DH0101Edit_Click" />

                            <%--<asp:Button ID="WFB2DH0101Add" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0101Add%>" OnClick="WFB2DH0101Add_Click" />
                            <asp:Button ID="WFB2DH0101Delete" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0101Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DH0101Delete_Click" />
                            <asp:Button ID="WFB2DH0101Edit" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0101Edit%>" Visible="false" OnClick="WFB2DH0101Edit_Click" />--%>
                        </div>
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData2"
                SelectCountMethod="getCount2" TypeName="CFB2DH0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_MAIN_LEAVE_CD"
                        Name="main_leave_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_IS_IFLOW_SHOW" DefaultValue=""
                        Name="is_iflow_show" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_IS_USED" DefaultValue=""
                        Name="is_used" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1800px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--子假別代碼 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD2%>" SortExpression="SUB_LEAVE_CD" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SUB_LEAVE_CD" runat="server" Text='<%#Bind("SUB_LEAVE_CD")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--子假別名稱 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_DESC%>" SortExpression="SUB_LEAVE_DESC" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_SUB_LEAVE_DESC" runat="server" Text='<%#Bind("SUB_LEAVE_DESC")%>' Width="150px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--假別上限控管方式 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lbtn_LEAVE_MAX_DAY_CD%>" SortExpression="LEAVE_MAX_DAY_CD" HeaderStyle-Width="150px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtn_LEAVE_MAX_DAY_CD"  runat="server" Text='<%#Bind("LEAVE_MAX_DAY_CD")%>' Width="150px" ToolTip="<%# Container.DataItemIndex %>" Font-Underline="true" OnClick="lbtn_LEAVE_MAX_DAY_CD_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--是否包含假日 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_IS_INCLUDE_HOLIDAY%>" SortExpression="IS_INCLUDE_HOLIDAY" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_INCLUDE_HOLIDAY" runat="server" Text='<%#Bind("IS_INCLUDE_HOLIDAY")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--時段限制 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lbtn_LEAVE_TIME_LIMIT_CD%>" SortExpression="LEAVE_TIME_LIMIT_CD" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtn_LEAVE_TIME_LIMIT_CD" runat="server" Text='<%#Bind("LEAVE_TIME_LIMIT_CD")%>' Width="80px" ToolTip="<%# Container.DataItemIndex %>" Font-Underline="true" OnClick="lbtn_LEAVE_TIME_LIMIT_CD_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--適用人員 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lbtn_LEAVE_ALLOW_CD%>" SortExpression="LEAVE_ALLOW_CD" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtn_LEAVE_ALLOW_CD" runat="server" Text='<%#Bind("LEAVE_ALLOW_CD")%>' ToolTip="<%# Container.DataItemIndex %>" Width="80px" Font-Underline="true" OnClick="lbtn_LEAVE_ALLOW_CD_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--給薪比率 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_PAY_RATE%>" SortExpression="LEAVE_PAY_RATE" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_PAY_RATE" runat="server" Text='<%#Bind("LEAVE_PAY_RATE")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--時間單位 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_TIME_UNIT%>" SortExpression="LEAVE_TIME_UNIT" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_TIME_UNIT" runat="server" Text='<%#Bind("LEAVE_TIME_UNIT")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--計算單位 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_COUNT_HOUR%>" SortExpression="LEAVE_COUNT_HOUR" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_COUNT_HOUR" runat="server" Text='<%#Bind("LEAVE_COUNT_HOUR")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--單次申請最小值 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_MIN_VALUE%>" SortExpression="LEAVE_MIN_VALUE" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_MIN_VALUE" runat="server" Text='<%#Bind("LEAVE_MIN_VALUE")%>' Width="140px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--統計方式 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_COUNT_CD%>" SortExpression="LEAVE_COUNT_CD" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_COUNT_CD" runat="server" Text='<%#Bind("LEAVE_COUNT_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--特殊身份 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_LEAVE_SPECIAL_CD%>" SortExpression="LEAVE_SPECIAL_CD" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEAVE_SPECIAL_CD" runat="server" Text='<%#Bind("LEAVE_SPECIAL_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--薪資結算 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_SALARY_SETTLE_CD%>" SortExpression="SALARY_SETTLE_CD" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_SETTLE_CD" runat="server" Text='<%#Bind("SALARY_SETTLE_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--IFLOW顯示否 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_IS_IFLOW_SHOW%>" SortExpression="IS_IFLOW_SHOW" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_IFLOW_SHOW" runat="server" Text='<%#Bind("IS_IFLOW_SHOW")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--使用狀態 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_IS_USED%>" SortExpression="IS_USED" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_USED" runat="server" Text='<%#Bind("IS_USED")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--扣年獎在職天數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_AWARD_DAY%>" SortExpression="AWARD_DAY" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_AWARD_DAY" runat="server" Text='<%#Bind("AWARD_DAY")%>' Width="140px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--扣紅利在職天數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_BONUS_DAY%>" SortExpression="BONUS_DAY" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_BONUS_DAY" runat="server" Text='<%#Bind("BONUS_DAY")%>' Width="140px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--扣期間工期滿金在職天數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_PLAN_DAY%>" SortExpression="PLAN_DAY" HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_PLAN_DAY" runat="server" Text='<%#Bind("PLAN_DAY")%>' Width="140px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--考核控管 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_IS_ASSESS%>" SortExpression="IS_ASSESS" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_ASSESS" runat="server" Text='<%#Bind("IS_ASSESS")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--扣除累積時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_IS_ACC_HOUR%>" SortExpression="IS_ACC_HOUR" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_ACC_HOUR" runat="server" Text='<%#Bind("IS_ACC_HOUR")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--假別指定出勤別 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_DH_WORK_DAY_CD%>" SortExpression="DH_WORK_DAY_CD" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_DH_WORK_DAY_CD" runat="server" Text='<%#Bind("DH_WORK_DAY_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--假別指定性別 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_DH_SEX_CD%>" SortExpression="DH_SEX_CD" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_DH_SEX_CD" runat="server" Text='<%#Bind("DH_SEX_CD")%>' Width="80px"></asp:Label>
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
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
