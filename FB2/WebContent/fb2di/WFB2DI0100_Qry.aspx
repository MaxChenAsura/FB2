<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0100_Qry.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0100_Qry" Culture="auto" UICulture="auto"%>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
                barcolor: "#7F7F7F",
                freezesize: 4
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        //清空畫面
        function ClearAll() {
            $("#ddl_OVERTIME_CD").val("-1");
            $("#ddl_OVERTIME_DT_TYPE").val("-1");
            $("#ddl_IS_USED").val("-1");
            $("#ddl_IS_IFLOW_SHOW").val("-1");
        }

        function getSearch()
        {
            $("#hid_is_search").val("Y");
            BlockUI();
            return true;
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                                        <asp:Label ID="lb_OVERTIME_CD" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_OVERTIME_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_OVERTIME_DT_TYPE" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_DT_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_OVERTIME_DT_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_USED" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IS_USED%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_USED" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_IFLOW_SHOW" runat="server" Text="<%$Resources:Resource,wfb2di_lb_IS_IFLOW_SHOW%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_IFLOW_SHOW" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Text="" Value="-1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Y-是" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="N-否" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DI0100Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0100Search%>" OnClientClick="getSearch();" OnClick="WFB2DI0100Search_Click" />

                                            <%--<asp:Button ID="WFB2DI0100Search" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0100Search%>" OnClientClick="getSearch();" OnClick="WFB2DI0100Search_Click" />--%>
                                            
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2di_btn_clear%>" onclick="ClearAll();" />
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
                            <aces:Btn ID="WFB2DI0100Add" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0100Add%>" OnClick="WFB2DI0100Add_Click"  />
                            <aces:Btn ID="WFB2DI0100Edit" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0100Edit%>" Visible="false" OnClick="WFB2DI0100Edit_Click" />
                            <aces:Btn ID="WFB2DI0100Delete" runat="server" Text="<%$Resources:Resource,wfb2di_WFB2DI0100Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DI0100Delete_Click"  />
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DI0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_OVERTIME_CD" DefaultValue=""
                        Name="overtime_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_OVERTIME_DT_TYPE" DefaultValue=""
                        Name="overtime_dt_type" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_IS_USED" DefaultValue=""
                        Name="is_used" PropertyName="SelectedValue" Type="String" />
                     <asp:ControlParameter ControlID="ddl_IS_IFLOW_SHOW" DefaultValue=""
                        Name="is_iflow_show" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1330px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--加班類型--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_OVERTIME_CD%>" SortExpression="OVERTIME_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_OVERTIME_CD" runat="server" Text='<%#Bind("OVERTIME_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--加班類型說明 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_OVERTIME_DESC%>" SortExpression="OVERTIME_DESC" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_OVERTIME_DESC" runat="server" Text='<%#Bind("OVERTIME_DESC")%>' Width="150px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--出勤別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_WORK_DAY_CD%>" SortExpression="WORK_DAY_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_WORK_DAY_CD" runat="server" Text='<%#Bind("WORK_DAY_CD")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--加班日期類型--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_OVERTIME_DT_TYPE2%>" SortExpression="OVERTIME_DT_TYPE" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_OVERTIME_DT_TYPE" runat="server" Text='<%#Bind("OVERTIME_DT_TYPE")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                   
                    <%--三高累計時數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_HYPER_HOUR%>" SortExpression="HYPER_HOUR" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_HYPER_HOUR" runat="server" Text='<%#Bind("HYPER_HOUR")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--一般累計時數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_NORMAL_HOUR%>" SortExpression="NORMAL_HOUR" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_NORMAL_HOUR" runat="server" Text='<%#Bind("NORMAL_HOUR")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--免稅時數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_NONTAX_HOUR%>" SortExpression="NONTAX_HOUR" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_NONTAX_HOUR" runat="server" Text='<%#Bind("NONTAX_HOUR")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--累計時數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_OTHER_HOUR%>" SortExpression="OTHER_HOUR" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_OTHER_HOUR" runat="server" Text='<%#Bind("OTHER_HOUR")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--應稅時數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_TAX_HOUR%>" SortExpression="TAX_HOUR" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_TAX_HOUR" runat="server" Text='<%#Bind("TAX_HOUR")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--換休/申告--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_OVERTIME_EXCHANGE_CD%>" SortExpression="OVERTIME_EXCHANGE_CD" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_OVERTIME_EXCHANGE_CD" runat="server" Text='<%#Bind("OVERTIME_EXCHANGE_CD")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--換休結算週期--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_SALARY_SETTLE_CD%>" SortExpression="SALARY_SETTLE_CD" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_SETTLE_CD" runat="server" Text='<%#Bind("SALARY_SETTLE_CD")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--換休對象--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_CHG_WORK_CD%>" SortExpression="CHG_WORK_CD" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CHG_WORK_CD" runat="server" Text='<%#Bind("CHG_WORK_CD")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--加班計算時數--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_O_HOUR_CD%>" SortExpression="O_HOUR_CD" HeaderStyle-Width="260px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_O_HOUR_CD" runat="server" Text='<%#Bind("O_HOUR_CD")%>' Width="260px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--加班倍數代碼--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2di_lb_O_MUL_CD%>" SortExpression="O_MUL_CD" HeaderStyle-Width="260px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_O_MUL_CD" runat="server" Text='<%#Bind("O_MUL_CD")%>' Width="260px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2di_RowNumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_CD%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_DESC%>" Width="150px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2di_lb_WORK_DAY_CD%>" Width="70px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_DT_TYPE2%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2di_lb_O_HOUR_CD%>" Width="150px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2di_lb_O_MUL_CD%>" Width="150px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2di_lb_HYPER_HOUR%>" Width="150px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2di_lb_NORMAL_HOUR%>" Width="150px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="<%$Resources:Resource,wfb2di_lb_NONTAX_HOUR%>" Width="90px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OTHER_HOUR%>" Width="90px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="<%$Resources:Resource,wfb2di_lb_TAX_HOUR%>" Width="90px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="<%$Resources:Resource,wfb2di_lb_OVERTIME_EXCHANGE_CD%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="<%$Resources:Resource,wfb2di_lb_SALARY_SETTLE_CD%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="<%$Resources:Resource,wfb2di_lb_CHG_WORK_CD%>" Width="120px"></asp:Label>
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
        <asp:HiddenField ID="hid_is_search" runat="server" ClientIDMode="Static" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

