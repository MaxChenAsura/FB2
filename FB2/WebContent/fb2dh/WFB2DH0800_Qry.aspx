<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0800_Qry.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0800_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".number").mask('999');            

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

        //清空畫面
        function ClearAll() {
            $("#txt_BASE_YEAR").val("");            
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
                                <col width="25%" />
                                <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="25%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--年度--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BASE_YEAR" runat="server" Text="<%$Resources:Resource,wfb2dh_BASE_YEAR%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_BASE_YEAR" runat="server" MaxLength="4" ClientIDMode="Static"  Width="60px"></asp:TextBox>
                                       <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_txt_BASE_YEAR_ERROR%>"
                                        ControlToValidate="txt_BASE_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                        ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <%-- 工號 --%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="80px" ClientIDMode="Static" BorderWidth="1"></asp:TextBox>                                        
                                    </td>                                                                        
                                </tr>
                                
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DH0800SEARCH" runat="server" Text="查詢" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DH0800Search_Click" />
                                             
                                            <%--<asp:Button ID="WFB2DH0800Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0700Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DH0800Search_Click" />--%>
                                            
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dh_btn_clear%>" onclick="ClearAll();" />
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
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DH0800DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_BASE_YEAR"
                        Name="baseyear" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />                   
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1019px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 年度 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_BASE_YEAR%>" SortExpression="BASE_YEAR" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_BASE_YEAR" runat="server" Text='<%#Bind("BASE_YEAR")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 主假別 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>" SortExpression="MAIN_LEAVE_CD" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text='<%#Bind("MAIN_LEAVE_CD")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 子假別 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD%>" SortExpression="SUB_LEAVE_CD" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_SUB_LEAVE_CD" runat="server" Text='<%#Bind("SUB_LEAVE_CD")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 生效日期 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_START_DT%>" SortExpression="START_DT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_DT" runat="server" Text='<%#Bind("START_DT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 結束日期 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_END_DT%>" SortExpression="END_DT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_DT" runat="server" Text='<%#Bind("END_DT")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 可用時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_AVAILABLE_VALUE%>" SortExpression="AVAILABLE_VALUE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                            <asp:Label ID="lb_AVAILABLE_VALUE" runat="server" Text='<%#Bind("AVAILABLE_VALUE")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 核可時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_APPROVE_VALUE%>" SortExpression="APPROVE_VALUE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                            <asp:Label ID="lb_APPROVE_VALUE" runat="server" Text='<%#Bind("APPROVE_VALUE")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- 遞延時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_DEFFER_VALUE%>" SortExpression="DEFFER_VALUE" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEFFER_VALUE" runat="server" Text='<%#Bind("DEFFER_VALUE")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%-- 去年特休預借時數 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_USED_PAY_LEAVE_VALUE%>" SortExpression="USED_PAY_LEAVE_VALUE" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="right">
                        <ItemTemplate>
                            <asp:Label ID="lb_USED_PAY_LEAVE_VALUE" runat="server" Text='<%#Bind("USED_PAY_LEAVE_VALUE")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
               
                <PagerStyle CssClass="GridviewScrollPager" />
                

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

