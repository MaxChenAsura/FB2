<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ss/WFB2SS0500_Qry.aspx.cs" Inherits="WebContent_WFB2SS0500_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
     <style type="text/css">
        #txt_PJOB_CD {
            text-transform: uppercase;
        }
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
            $("#ddl_INCENTIVE_TYPE").val("-1");
            $("#ddl_INCENTIVE_TYPE").val("-1");
            $("#txt_SALARY_SDT").val("");
            $("#txt_SALARY_EDT").val("");
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
                    <col width="10%" />
                    <col width="30%" />
                    <col width="10%" />
                    <col width="30%" />
                    <col width="10%" />
                    <col width="10%" />
                </colgroup>
                <tbody>
                     <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_SALARY_DT" runat="server" Text="發薪日期"></asp:Label></th>
                        <td align="left" class="Body_label"  >
                            <asp:TextBox ID="txt_SALARY_SDT" runat="server" MaxLength="10" Width="80px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            ~                                          
                            <asp:TextBox ID="txt_SALARY_EDT" runat="server" MaxLength="10" Width="80px" ClientIDMode="Static" CssClass="date"></asp:TextBox>                                          
                            <asp:CustomValidator ID="qrytest1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="發薪日期(起)格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_SALARY_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>      
                             <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                ErrorMessage="發薪日期(迄)格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_SALARY_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>  
                             <!--起日不可大於迄日 -->	
							<asp:CompareValidator ID="Compare_JOIN_DT" runat="server" ControlToCompare="txt_SALARY_SDT"
                            ControlToValidate="txt_SALARY_EDT" ErrorMessage="發薪日期起日不可大於迄日" Type="Date" Operator="GreaterThanEqual"
                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>                  
                        </td>
                    </tr>
                     <tr>
                        <%-- 轉薪資 --%>    
                        <th align="left" class="Body_TableHeader">
                        <asp:Label ID="lb_PRE_STATUS" runat="server" Text="轉薪資"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label"> 
                            <asp:DropDownList ID="ddl_PRE_STATUS" runat="server" ClientIDMode="Static"></asp:DropDownList>                           
                        </td>      
                        <th align="left" class="Body_TableHeader">
                            <%--獎金類型--%>
                            <asp:Label ID="lb_INCENTIVE_TYPE" runat="server" Text="獎金類型"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                                <asp:DropDownList ID="ddl_INCENTIVE_TYPE" runat="server" ClientIDMode="Static"  ></asp:DropDownList>       
                        </td>
                    </tr>
                               
                    <tr>
                        <td align="right" colspan="6">
                            <aces:Btn ID="WFB2SS0500Search" runat="server" Text="查詢" OnClick="WFB2SS0500Search_Click" OnClientClick="return CheckSearch();" />
                            <%--
                            <asp:Button ID="WFB2SS0500Search" runat="server" Text="<%$Resources:Resource,wfb2sh_btn_search%>" OnClick="WFB2SS0500Search_Click" OnClientClick="return CheckSearch();" />
                            --%>
                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sh_btn_clear%>" onclick="ClearAll();" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="6">
                            <div id="init_grid">
                                
                                <aces:Btn ID="WFB2SS0500EXCEL" runat="server" Text="匯出EXCEL" Visible="false" OnClick="WFB2SS0500EXCEL_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SS0500DTL" runat="server" Text="查詢明細" Visible="false" OnClick="WFB2SS0500DTL_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SS0500APP" runat="server" Text="轉薪資" Visible="false" OnClick="WFB2SS0500APP_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SS0500CELAPP" runat="server" Text="取消轉薪資" Visible="false" OnClick="WFB2SS0500CELAPP_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SS0500delete" runat="server" Text="刪除" Visible="false" OnClick="WFB2SS0500delete_Click"  OnClientClick="BlockUI();" />
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>


            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SS0500DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                     <asp:ControlParameter ControlID="txt_SALARY_SDT"
                        Name="sdt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" /> 
                    <asp:ControlParameter ControlID="txt_SALARY_EDT"
                        Name="edt" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" /> 
                    <asp:ControlParameter ControlID="ddl_PRE_STATUS"
                        Name="status" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />         
                     <asp:ControlParameter ControlID="ddl_INCENTIVE_TYPE"
                        Name="type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />    
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
                    <%--發薪日期--%>
                    <asp:TemplateField HeaderText="發薪日期" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="SALARY_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_SALARY_DT" runat="server" Text='<%#Bind("SALARY_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--獎金類型--%>
                    <asp:TemplateField HeaderText="獎金類型" HeaderStyle-Width="160px" ItemStyle-HorizontalAlign="Left" SortExpression="INCENTIVE_TYPE">
                        <ItemTemplate>
                            <asp:Label ID="lb_TYPE_DESC" runat="server" Text='<%#Bind("TYPE_DESC")%>' Width="160px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--人數--%>
                    <asp:TemplateField HeaderText="人數" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Right" SortExpression="INCENTIVE_NUM">
                        <ItemTemplate>
                            <asp:Label ID="lb_INCENTIVE_NUM" runat="server" Text='<%#Bind("INCENTIVE_NUM")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>    
                     <%--金額總計--%>
                    <asp:TemplateField HeaderText="金額總計" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" SortExpression="INCENTIVE_TOTAL">
                        <ItemTemplate>
                            <asp:Label ID="lb_INCENTIVE_TOTAL" runat="server" Text='<%#Bind("INCENTIVE_TOTAL","{0:##,#}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                    <%--轉薪日期--%>
                    <asp:TemplateField HeaderText="轉薪日期" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="PRE_DT">
                        <ItemTemplate>
                            <asp:Label ID="lb_PRE_DT" runat="server" Text='<%#Bind("PRE_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--轉薪資--%>
                    <asp:TemplateField HeaderText="轉薪資" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="PRE_STATUS">
                        <ItemTemplate>
                            <asp:Label ID="lb_STATUS_DESC" runat="server" Text='<%#Bind("STATUS_DESC")%>' Width="80px"></asp:Label>
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
                <%--匯出EXCEL按鈕必寫--%>
            <asp:PostBackTrigger ControlID="WFB2SS0500EXCEL" />  
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
