<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0520_Dtl3.aspx.cs" Inherits="WebContent_WFB2SJ0520_Dtl3" Culture="auto" UICulture="auto" %>

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
                barcolor: "#7F7F7F"
            });
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
        function doViewComments(emp_id) {


            OpenSearch("Assess_Comments_Rec.aspx", "", "", "ASSESS_YEAR=" + document.getElementById("hid_ASSESS_YEAR").value + "&ASSESS_TYPE=" + document.getElementById("hid_ASSESS_TYPE").value + "&EMP_ID=" + emp_id);
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
                                    <th></th>
                                    <th></th>
                                    <td colspan="4" align="right" class="Body_label">
                                       <asp:HiddenField ID="hid_MA_EMP_ID" runat="server" ClientIDMode="Static" />  
                                        <asp:HiddenField ID="hid_ASSESS_YEAR" runat="server" ClientIDMode="Static" />  
                                        <asp:HiddenField ID="hid_ASSESS_TYPE" runat="server" ClientIDMode="Static" />
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
                <tr>
                    <td><font color="red"><b>【A考核/向上2級/業務職C 考核最終確認】</b></font> </td>
                </tr>
               
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getConfirmData"
                SelectCountMethod="getConfirmCount" TypeName="CFB2SJ0520DAO" EnablePaging="True"
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
                    <asp:ControlParameter ControlID="hid_MA_EMP_ID"
                        Name="ma_emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" /> 
                   
                   
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1020px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                   
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--區分--%>
                    <asp:TemplateField HeaderText="區分" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" SortExpression="RECOMM_DESC">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_RECOMM_DESC" runat="server" Text='<%#Bind("RECOMM_DESC")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="工號" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部門名稱--%>
                    <asp:TemplateField HeaderText="部門名稱" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" SortExpression="DIREC_EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_NAME")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="姓名" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="DIREC_EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格--%>
                    <asp:TemplateField HeaderText="資格" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="LEVEL_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職務--%>
                    <asp:TemplateField HeaderText="職種" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="WS_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--職務--%>
                    <asp:TemplateField HeaderText="職務" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" SortExpression="PJOB_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_DESC" runat="server" Text='<%#Bind("PJOB_DESC")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                      <%--今回考核--%>
                    <asp:TemplateField HeaderText="今回考核" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="SCORE_FINAL">
                        <ItemTemplate>
                            <asp:Label ID="lb_SCORE_FINAL" runat="server" Text='<%#Bind("SCORE_FINAL")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>     
                    <%--總評內容
                    <asp:TemplateField HeaderText="總評內容" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="">
                        <ItemTemplate>
                             <asp:Button ID="btn_COMMENTS"  Text="..." runat="server" ClientIDMode="Static"  OnClick="btn_COMMENTS_Click" CommandArgument='<%#Bind("EMP_ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>  --%> 
                    
                </Columns>
                
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">

                    <td class="GridviewScrollPager TD" style="display:none">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="70"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="display:none;width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:Button ID="HID_cancel" runat="server" Style="display: none" ClientIDMode="Static" OnClick="HID_cancel_Click" />
             <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">                           
                            <tbody>    
                                <tr>
                                    <td  align="right" class="Body_label">

                                        <div id="Div1">
                                             <asp:Button runat="server" ID="WFB2SJ0520ConfirmSign" Text="確定提出" OnClick="WFB2SJ0520ConfirmSign_Click" />
                                             <asp:Button runat="server" ID="btn_cancel" Text="返回" OnClick="btn_Cancel_Click" />
                                        </div>
                                    </td>
                                </tr>
                                
                            </tbody>
                        </table>
                    </td>
                </tr>

               
            </table>
        
        </ContentTemplate>
        
      
    </asp:UpdatePanel>

</asp:Content>

