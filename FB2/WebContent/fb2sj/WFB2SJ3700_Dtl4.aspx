<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ3700_Dtl4.aspx.cs" Inherits="WebContent_WFB2SJ3700_Dtl4" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".number").mask('999');
            $(".number2").mask('99');
            $(".numberr").css("text-align", "right");
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
                freezesize: 3,
                headerrowcount: 2
            });
        }
        function IsDelete() {
            var answer = confirm("確定要刪除?");
            if (answer)
                return true;
            else {
                document.getElementById('HID_cancel').click();
                return false;
            }
        }

        //清空畫面
        function ClearAll() {
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
        //查詢前檢核
        function CheckSearch() {
            var processed = true;
            BlockUI();
            return processed;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
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
                                        <asp:Label ID="lb_ASSESS_YEAR" runat="server" Text="考核年度"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_ASSESS_YEAR" runat="server" Width="60px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>                            
                                          <asp:HiddenField ID="hid_ASSESS_YEAR" runat="server" ClientIDMode="Static" />   
                                    </td>                      
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text="考核類別"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_ASSESS_TYPE" runat="server" Width="100px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>                            
                                          <asp:HiddenField ID="hid_ASSESS_TYPE" runat="server" ClientIDMode="Static" />                                  
                                    </td>
                                     <th></th>
                                     <th>                             
                                        <asp:HiddenField ID="hid_DEPT_NO_20" runat="server" ClientIDMode="Static" /> 
                                        <asp:HiddenField ID="hid_DEPT_NO" runat="server" ClientIDMode="Static" />    
                                        <asp:HiddenField ID="hid_DEPT_LEVEL" runat="server" ClientIDMode="Static" />   
                                        <asp:HiddenField ID="hid_IS_DEPT_20" runat="server" ClientIDMode="Static" />   
                                        <asp:HiddenField ID="hid_DEPT_EMP_ID" runat="server" ClientIDMode="Static" />      
                                     </th>                        
                                </tr> 
                                         
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <td colspan="4" align="right" class="Body_label">
                                        <div id="init">
                                             <asp:Button runat="server" ID="btn_cancel" Text="返回" OnClick="btn_Cancel_Click" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                        <hr />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

               
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getWSApproveData"
                SelectCountMethod="getWSApproveCount" TypeName="CFB2SJ3700DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                     <asp:ControlParameter ControlID="hid_ASSESS_YEAR"
                        Name="assess_year" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                     <asp:ControlParameter ControlID="hid_ASSESS_TYPE"
                        Name="assess_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />                    
                    <asp:ControlParameter ControlID="hid_DEPT_NO"
                        Name="dept_no" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" /> 
                   <asp:ControlParameter ControlID="hid_DEPT_EMP_ID"
                        Name="dept_emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1020px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                   
                    <%--序號--%>
                    <asp:TemplateField HeaderText="" HeaderStyle-Width="40px">
                        <ItemTemplate>
                             <asp:Button ID="btn_Approve"  Text="點選" runat="server" ClientIDMode="Static"  OnClick="btn_Approve_Click" CommandArgument='<%#Bind("ARG") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--考核群組--%>
                    <asp:TemplateField HeaderText="考核群組" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_R_SCORE_LEVEL_GROUP" runat="server" Text='<%#Bind("SCORE_LEVEL_GROUP")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--考核群組--%>
                    <asp:TemplateField HeaderText="考核群組名稱" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_R_POINT_GROUP_NAME" runat="server" Text='<%#Bind("POINT_GROUP_NAME")%>' Width="90px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--應評人數--%>
                    <asp:TemplateField HeaderText="應評人數</BR>(含外數)" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_R_EMP_TOTAL" runat="server" Text='<%#Bind("EMP_TOTAL")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--A--%>
                    <asp:TemplateField HeaderText="A" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_R_SCORE_A" runat="server" Text='<%#Bind("SCORE_A")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <%--B--%>
                    <asp:TemplateField HeaderText="B" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_R_SCORE_B" runat="server" Text='<%#Bind("SCORE_B")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <%--C--%>
                    <asp:TemplateField HeaderText="C" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_R_SCORE_C" runat="server" Text='<%#Bind("SCORE_C")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <%--D--%>
                    <asp:TemplateField HeaderText="D" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_R_SCORE_D" runat="server" Text='<%#Bind("SCORE_D")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <%--E--%>
                    <asp:TemplateField HeaderText="E" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_R_SCORE_E" runat="server" Text='<%#Bind("SCORE_E")%>' Width="70px"></asp:Label>
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
            <asp:Button ID="HID_cancel" runat="server" Style="display: none" ClientIDMode="Static" OnClick="HID_cancel_Click" />
        </ContentTemplate>
        
      
    </asp:UpdatePanel>

</asp:Content>

