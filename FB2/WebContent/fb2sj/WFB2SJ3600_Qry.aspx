<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ3600_Qry.aspx.cs" Inherits="WebContent_WFB2SJ3600_Qry" Culture="auto" UICulture="auto" %>

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
            $("#txt_ASSESS_YEAR").val("");
            $("#ddl_ASSESS_TYPE").val("-1");
            $("#ddl_DEPT_NO_20").val("-1");
            $("#ddl_WS_CD").val("-1");
            $("#ddl_IS_MERGER").val("-1");
        }
        //送出簽核確認
        function SignConfirm() {
            var confirmMsg = "確定要提出簽核?";
            
            if (confirm(confirmMsg)) {
                return true;
            } else {
                return false;
            }
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
                            <asp:Label ID="lb_ASSESS_YEAR" runat="server" Text="考核年度"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_ASSESS_YEAR" runat="server" Width="60px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>                            
                              <asp:HiddenField ID="hid_ASSESS_YEAR" runat="server" ClientIDMode="Static" />                                  
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2sj_format_assess_year_s%>" ControlToValidate="txt_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                        </td>                      
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text="考核類別"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                             <asp:TextBox ID="txt_ASSESS_TYPE" runat="server" Width="60px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>                            
                              <asp:HiddenField ID="hid_ASSESS_TYPE" runat="server" ClientIDMode="Static" />     
                                <asp:HiddenField ID="hid_DEPT_NO_20" runat="server" ClientIDMode="Static" /> 
                                <asp:HiddenField ID="hid_DEPT_NO" runat="server" ClientIDMode="Static" />    
                                <asp:HiddenField ID="hid_DEPT_LEVEL" runat="server" ClientIDMode="Static" />  
                                <asp:HiddenField ID="hid_DEPT_NAME" runat="server" ClientIDMode="Static" />                              
                        </td>
                        
                    </tr> 
                              
                    <tr>
                       
                        <th></th>
                        <th></th>
                        <td colspan="2" align="right" class="Body_label">
                            <div id="init">
                                <aces:Btn ID="WFB2SJ3600Search" runat="server" Text="查詢" OnClick="WFB2SJ3600Search_Click" OnClientClick="return CheckSearch();" />
                           
                                <%--<asp:Button ID="WFB2IA0100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2IA0100Search_Click" OnClientClick="BlockUI();" />--%>
                                            
                                <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="ClearAll();" Visible="false" />
                                 <aces:Btn ID="WFB2SJ3600EmpDtl" runat="server" Text="員工明細" OnClick="WFB2SJ3600EmpDtl_Click" OnClientClick="" />
                                 <aces:Btn ID="WFB2SJ3600Approve" runat="server" Text="提出簽核" OnClick="WFB2SJ3600Approve_Click" OnClientClick="return SignConfirm();" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                   
                </tbody>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SJ3600DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" /> 
                    <asp:ControlParameter ControlID="hid_ASSESS_YEAR"
                        Name="assess_year" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                     <asp:ControlParameter ControlID="hid_ASSESS_TYPE"
                        Name="assess_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />                     
                   
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
                             <!-- 凍結註記 -->
                            <asp:HiddenField ID="hid_SIGN_YN" runat="server" Value='<%#Bind("SIGN_YN")%> ' ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部級單位--%>
                    <asp:TemplateField HeaderText="主管" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--單位名稱--%>
                    <asp:TemplateField HeaderText="單位名稱" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" SortExpression="DEPT_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_NAME")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--簽核狀況--%>
                    <asp:TemplateField HeaderText="簽核狀況" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="SIGN_YN">
                        <ItemTemplate>
                            <asp:Label ID="lb_SIGN_YN_DESC" runat="server" Text='<%#Bind("SIGN_YN_DESC")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--應評人數--%>
                    <asp:TemplateField HeaderText="應評人數" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="MNG_NUM">
                        <ItemTemplate>
                            <asp:Label ID="lb_MNG_NUM" runat="server" Text='<%#Bind("MNG_NUM")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--已評人數--%>
                    <asp:TemplateField HeaderText="已評人數" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="MNG_GRANT_NUM">
                        <ItemTemplate>
                            <asp:Label ID="lb_MNG_GRANT_NUM" runat="server" Text='<%#Bind("MNG_GRANT_NUM")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                    <%--A--%>
                    <asp:TemplateField HeaderText="A" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" SortExpression="MNG_GRANT_A_NUM">
                        <ItemTemplate>
                            <asp:Label ID="lb_MNG_GRANT_A_NUM" runat="server" Text='<%#Bind("MNG_GRANT_A_NUM")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>     
                    <%--B--%>
                    <asp:TemplateField HeaderText="B" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" SortExpression="MNG_GRANT_B_NUM">
                        <ItemTemplate>
                            <asp:Label ID="lb_MNG_GRANT_B_NUM" runat="server" Text='<%#Bind("MNG_GRANT_B_NUM")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>     
                    <%--C--%>
                    <asp:TemplateField HeaderText="C" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" SortExpression="MNG_GRANT_C_NUM">
                        <ItemTemplate>
                            <asp:Label ID="lb_MNG_GRANT_C_NUM" runat="server" Text='<%#Bind("MNG_GRANT_C_NUM")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>     
                    <%--D--%>
                    <asp:TemplateField HeaderText="D" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" SortExpression="MNG_GRANT_D_NUM">
                        <ItemTemplate>
                            <asp:Label ID="lb_MNG_GRANT_D_NUM" runat="server" Text='<%#Bind("MNG_GRANT_D_NUM")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>      
                    <%--E--%>
                    <asp:TemplateField HeaderText="E" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" SortExpression="MNG_GRANT_E_NUM">
                        <ItemTemplate>
                            <asp:Label ID="lb_MNG_GRANT_E_NUM" runat="server" Text='<%#Bind("MNG_GRANT_E_NUM")%>' Width="50px"></asp:Label>
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
