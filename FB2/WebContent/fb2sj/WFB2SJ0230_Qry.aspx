<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0230_Qry.aspx.cs" Inherits="WebContent_WFB2SJ0230_Qry" Culture="auto" UICulture="auto" %>

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
            $(".year").mask('9999');
            $(".numFormat").mask('9.999');
            $(".decimal").css("text-align", "right").css("ime-mode", "disabled");

            //GridView必須
            gridviewScroll();
            //部門編號取得部門的ajax
            $("#txt_DEPT_NO_20").change(function () {
                if ($("#txt_DEPT_NO_20").val().length == 7) {
                    $.ajax({
                        url: "WFB2SJ0260GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO_20').val()
                        },
                        type: "GET",
                        cache: false,
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME_20').val("");
                                alert(JData.errMsg);
                            }
                            else {

                                $('#txt_DEPT_NAME_20').val(JData.DEPT_FULL_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DEPT_NAME_20').val("");
                }
            });
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
            /**var processed = true;
             BlockUI();
             return processed;**/
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else {
                processed = false;
            }

            if (!processed)
                $.unblockUI();
            return processed;
        }
        //
        function exeCheckSearch() {
            /**var processed = true;
             BlockUI();
             return processed;**/
            var processed = true;

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
                if (confirm('確定要執行?  [ !!!注意：已維護資料會被覆蓋]')) {
                    processed = true;
                }
                else {
                    processed = false;
                }
            }
            else {
                processed = false;
            }

            if (!processed)
                $.unblockUI();
            return processed;
        }
        //清空畫面
        function ClearAll() {
            $("#txt_ASSESS_YEAR").val("");
            $("#ddl_ASSESS_TYPE").val("-1");
            $("#ddl_WS_CD").val("-1");
            $("#ddl_SCORE_LEVEL_GROUP").val("-1");
            $("#ddl_IS_MERGER").val("-1");
            $("#txt_DEPT_NO_20").val("");
            $("#txt_DEPT_NAME_20").val("");
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
                            <asp:TextBox ID="txt_ASSESS_YEAR" runat="server" Width="60px" ClientIDMode="Static" CssClass="MandatoryField year"></asp:TextBox>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ErrorMessage="年度輸入格式錯誤" ControlToValidate="txt_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"    ErrorMessage="考核年度必輸入" InitialValue=""
                        ControlToValidate="txt_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>                      
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text="考核類別"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                             <asp:DropDownList ID="ddl_ASSESS_TYPE" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"> </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="Req_ASSESS_TYPE" runat="server" 
                                            ErrorMessage="考核類別必輸入" InitialValue="-1"
                                            ControlToValidate="ddl_ASSESS_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_DEPT_NO_20" runat="server" Text="部級單位"></asp:Label>:
                        </th>
                         <td align="left" class="Body_label">                             
                               <asp:TextBox ID="txt_DEPT_NO_20" runat="server"  MaxLength="7" Width="64px" ClientIDMode="Static" CssClass=""></asp:TextBox>
                              <input id="btn_SEL_DEPT_NO_20" type="button" value="..." onclick="OpenSearch('AssessDept_Search.aspx', 'txt_DEPT_NO_20', 'txt_DEPT_NAME_20', 'DEPT_LEVEL=20');" />
                              <asp:TextBox ID="txt_DEPT_NAME_20" runat="server" Width="150px" ClientIDMode="Static" BorderWidth="0"></asp:TextBox>
                         </td>
                    </tr> 
                    <tr>                        
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_WS_CD" runat="server" Text="職種"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_WS_CD" runat="server" Width="100px" ClientIDMode="Static"> </asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_SCORE_LEVEL_GROUP" runat="server" Text="資格群組"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_SCORE_LEVEL_GROUP" runat="server" Width="100px" ClientIDMode="Static"> </asp:DropDownList>
                        </td>                        
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_IS_MERGER" runat="server" Text="合併"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_IS_MERGER" runat="server" Width="100px" ClientIDMode="Static"> </asp:DropDownList>
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
                                <aces:Btn ID="WFB2SJ0230Search" runat="server" Text="查詢" OnClick="WFB2SJ0230Search_Click" OnClientClick="return CheckSearch();" />
                           
                                <%--<asp:Button ID="WFB2IA0100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2IA0100Search_Click" OnClientClick="BlockUI();" />--%>
                                            
                                <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="ClearAll();" />
                                 <aces:Btn ID="WFB2SJ0230ReGen" runat="server" Text="重配置" OnClick="WFB2SJ0230ReGen_Click" OnClientClick ="return exeCheckSearch();"/>
                                 <aces:Btn ID="WFB2SJ0230ReGenDepEmps" runat="server" Text="部門人數重配置" OnClick="WFB2SJ0230ReGenDepEmps_Click" OnClientClick="return exeCheckSearch();" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="4">
                            <div id="init_grid">
                                <aces:Btn ID="WFB2SJ0230Upd" runat="server" Text="修改" Visible="false" OnClick="WFB2SJ0230Upd_Click" OnClientClick="BlockUI();" />
                               
                               
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SJ0230DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" /> 
                    <asp:ControlParameter ControlID="txt_ASSESS_YEAR"
                        Name="assess_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                     <asp:ControlParameter ControlID="ddl_ASSESS_TYPE"
                        Name="assess_type" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />                     
                    <asp:ControlParameter ControlID="txt_DEPT_NO_20"
                        Name="dept_no_20" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                     <asp:ControlParameter ControlID="ddl_WS_CD"
                        Name="ws_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" /> 
                     <asp:ControlParameter ControlID="ddl_SCORE_LEVEL_GROUP"
                        Name="score_level_group" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" /> 
                     <asp:ControlParameter ControlID="ddl_IS_MERGER"
                        Name="is_merger" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" /> 
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
                    <%--部級單位--%>
                    <asp:TemplateField HeaderText="部級單位" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="DEPT_NO_20">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_DEPT_NO_20" runat="server" Text='<%#Bind("DEPT_NO_20")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部級名稱--%>
                    <asp:TemplateField HeaderText="部級名稱" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" SortExpression="DEPT_NAME_20">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_DEPT_NAME_20" runat="server" Text='<%#Bind("DEPT_NAME_20")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職種--%>
                    <asp:TemplateField HeaderText="職種" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="WS_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_WS_CD_DESC" runat="server" Text='<%#Bind("WS_CD_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格--%>
                    <asp:TemplateField HeaderText="資格" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left" SortExpression="LEVEL_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--考核(資格)群組--%>
                    <asp:TemplateField HeaderText="考核(資格)群組" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" SortExpression="SCORE_LEVEL_GROUP">
                        <ItemTemplate>
                            <asp:Label ID="lb_SCORE_LEVEL_GROUP" runat="server" Text='<%#Bind("SCORE_LEVEL_GROUP")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--合併--%>
                    <asp:TemplateField HeaderText="合併" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left" SortExpression="ITEM_GROUP">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_MERGER" runat="server" Text='<%#Bind("IS_MERGER_DESC")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--A--%>
                    <asp:TemplateField HeaderText="A" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Left" SortExpression="BASE_A">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_BASE_A" runat="server" Text='<%#Bind("BASE_A")%>' Width="20px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                    <%--B--%>
                    <asp:TemplateField HeaderText="B" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Left" SortExpression="BASE_B">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_BASE_B" runat="server" Text='<%#Bind("BASE_B")%>' Width="20px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                    <%--C--%>
                    <asp:TemplateField HeaderText="C" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Left" SortExpression="BASE_C">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_BASE_C" runat="server" Text='<%#Bind("BASE_C")%>' Width="20px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                    <%--D--%>
                    <asp:TemplateField HeaderText="D" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Left" SortExpression="BASE_D">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_BASE_D" runat="server" Text='<%#Bind("BASE_D")%>' Width="20px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                    <%--E--%>
                    <asp:TemplateField HeaderText="E" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Left" SortExpression="BASE_E">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_BASE_E" runat="server" Text='<%#Bind("BASE_E")%>' Width="20px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>     
                    <%--合計--%>
                    <asp:TemplateField HeaderText="合計" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Left" SortExpression="BASE_TOT">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_BASE_TOT" runat="server" Text='<%#Bind("BASE_TOT")%>' Width="40px"></asp:Label>
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
