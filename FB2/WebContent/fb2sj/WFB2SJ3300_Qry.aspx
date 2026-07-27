<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ3300_Qry.aspx.cs" Inherits="WebContent_WFB2SJ3300_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
     <style type="text/css">
        
    </style>
    <script type="text/javascript">

        $(function () {

            iniForm();
        });
        function iniForm() {
            //日期格式心須
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $('.date').mask('9999/99/99');
            $(".numFormat").mask('9.999');
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");

            //GridView必須
            gridviewScroll();
            $.unblockUI();
 
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
                    freezesize: 0

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
            BlockUI();
            return processed;
        }

        //清空畫面
        function ClearAll() {
            $("#ddl_ASSESS_TYPE").val("-1");
        }


    </script>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="15%" />
                    <col width="20%" />
                    <col width="10%" />
                    <col width="15%" />
                    <col width="10%" />
                    <col width="30%" />
                </colgroup>
                <tbody>
                     <tr>                        
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text="考核類型"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_ASSESS_TYPE" runat="server" Width="100px" ClientIDMode="Static"> </asp:DropDownList>
                        </td>
                        <th align="left" class="">
                        </th>
                        <td align="left" class="">
                        </td>
                    </tr>                   
                    <tr>
                        <th></th>
                        <th></th>
                        <th></th>
                        <td align="right" class="Body_label">
                            <div id="init">
                                <aces:Btn ID="WFB2SJ3300Search" runat="server" Text="查詢" OnClick="WFB2SJ3300Search_Click" OnClientClick="return CheckSearch();" />
                           
                                <%--<asp:Button ID="WFB2IA0100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2IA0100Search_Click" OnClientClick="BlockUI();" />--%>
                                            
                                <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="ClearAll();" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="4">
                            <div id="init_grid">
                                <aces:Btn ID="WFB2SJ3300Add" runat="server" Text="新增" Visible="true" OnClick="WFB2SJ3300Add_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SJ3300Upd" runat="server" Text="修改" Visible="false" OnClick="WFB2SJ3300Upd_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SJ3300Del" runat="server" Text="刪除" Visible="false" OnClick="WFB2SJ3300Del_Click" OnClientClick="return doButtonConfirm(this.value);" />
                               
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SJ3300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" /> 
                     <asp:ControlParameter ControlID="ddl_ASSESS_TYPE"
                        Name="assess_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" /> 
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" Width="20px" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--考核類型--%>
                    <asp:TemplateField HeaderText="考核類型" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="ASSESS_TYPE">
                        <ItemTemplate>
                            <asp:Label ID="lb_ASSESS_TYPE_DESC" runat="server" Text='<%#Bind("ASSESS_TYPE_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--A(%)--%>
                    <asp:TemplateField HeaderText="A(%)" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Left" SortExpression="RATE_A">
                        <ItemTemplate>
                            <asp:Label ID="lb_RATE_A" runat="server" Text='<%#Bind("RATE_A")%>' Width="30px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>     
                      
                    <%--B(%)--%>
                    <asp:TemplateField HeaderText="B(%)" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Left" SortExpression="RATE_B">
                        <ItemTemplate>
                            <asp:Label ID="lb_RATE_B" runat="server" Text='<%#Bind("RATE_B")%>' Width="30px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                      
                    <%--C(%)--%>
                    <asp:TemplateField HeaderText="C(%)" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Left" SortExpression="RATE_C">
                        <ItemTemplate>
                            <asp:Label ID="lb_RATE_C" runat="server" Text='<%#Bind("RATE_C")%>' Width="30px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                      
                    <%--D(%)--%>
                    <asp:TemplateField HeaderText="D(%)" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Left" SortExpression="RATE_D">
                        <ItemTemplate>
                            <asp:Label ID="lb_RATE_D" runat="server" Text='<%#Bind("RATE_D")%>' Width="30px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                      
                    <%--E(%)--%>
                    <asp:TemplateField HeaderText="E(%)" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Left" SortExpression="RATE_E">
                        <ItemTemplate>
                            <asp:Label ID="lb_RATE_E" runat="server" Text='<%#Bind("RATE_E")%>' Width="30px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                    <%--IS_CTL--%>
                    <asp:TemplateField HeaderText="是否控管人數" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" SortExpression="IS_CTL">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_CTL" runat="server" Text='<%#Bind("IS_CTL_DESC")%>' Width="30px"></asp:Label>
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
            <asp:HiddenField ID="hid_today" runat="server" ClientIDMode="Static" />

            <!-- 每頁的筆數 -->
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <!-- 是否要凍結視窗 -->
            <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="N" />
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
