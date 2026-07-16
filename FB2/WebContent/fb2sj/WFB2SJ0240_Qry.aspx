<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0240_Qry.aspx.cs" Inherits="WebContent_WFB2SJ0240_Qry" Culture="auto" UICulture="auto" %>

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
            //工號取得姓名的ajax
            $("#txt_MA_EMP_ID").change(function () {
                if ($("#txt_MA_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_MA_EMP_ID').val()
                        },
                        type: "GET",
                        cache: false,
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_MA_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_MA_EMP_NAME').val(JData.EMP_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_MA_EMP_NAME').val("");
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
            $("#txt_MA_EMP_ID").val("");
            $("#txt_MA_EMP_NAME").val("");
            //$("#txt_GRP_CD").val("");
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
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2sj_format_assess_year_s%>" ControlToValidate="txt_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"    ErrorMessage="考核年度必輸入" InitialValue=""
                                 ControlToValidate="txt_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>                      
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text="考核類別"></asp:Label>:
                        </th>                    
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_ASSESS_TYPE" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"> </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="req_ASSESS_TYPE" runat="server"    ErrorMessage="考核類別必輸入" InitialValue="-1"
                                 ControlToValidate="ddl_ASSESS_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr> 
                    <tr> 
                        
                         
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="Label1" runat="server" Text="工號"></asp:Label>:
                             <asp:HiddenField ID="hid_MA_TYPE" runat="server" ClientIDMode="Static" Value="A" /> 
                             <asp:HiddenField ID="hid_GRP_CD" runat="server" ClientIDMode="Static" Value="" /> 
                        </th>
                         <td align="left" class="Body_label">                             
                              <asp:TextBox ID="txt_MA_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_MA_EMP_ID', 'txt_MA_EMP_NAME', 'N');" />
                                <asp:TextBox ID="txt_MA_EMP_NAME" runat="server" Width="90px" ClientIDMode="Static" BorderWidth="0" ></asp:TextBox> 
                         </td>
                         <th></th>
                        <th></th>
                    </tr>                   
                    <tr>
                        
                        <th></th>
                        <th></th>
                        <th></th>
                        <td align="right" class="Body_label">
                            <div id="init">
                                <aces:Btn ID="WFB2SJ0240Search" runat="server" Text="查詢" OnClick="WFB2SJ0240Search_Click" OnClientClick="return CheckSearch();" />
                           
                                <%--<asp:Button ID="WFB2IA0100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2IA0100Search_Click" OnClientClick="BlockUI();" />--%>
                                            
                                <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="ClearAll();" />
                                 <aces:Btn ID="WFB2SJ0240ReGen" runat="server" Text="重配置" OnClick="WFB2SJ0240ReGen_Click" OnClientClick="return exeCheckSearch();" />
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
                                <aces:Btn ID="WFB2SJ0240Upd" runat="server" Text="修改" Visible="false" OnClick="WFB2SJ0240Upd_Click" OnClientClick="BlockUI();" />
                               
                               
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SJ0240DAO" EnablePaging="True"
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
                    <asp:ControlParameter ControlID="txt_MA_EMP_ID"
                        Name="ma_emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                     <asp:ControlParameter ControlID="hid_GRP_CD"
                        Name="grp_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" /> 
                     <asp:ControlParameter ControlID="hid_MA_TYPE"
                        Name="ma_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" /> 
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
                    <%--工號--%>
                    <asp:TemplateField HeaderText="工號" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="MA_EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_MA_EMP_ID" runat="server" Text='<%#Bind("MA_EMP_ID")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="姓名" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" SortExpression="MA_EMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_MA_EMP_NAME" runat="server" Text='<%#Bind("MA_EMP_NAME")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職種--%>
                    <asp:TemplateField HeaderText="職種" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" SortExpression="WS_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_WS_CD_DESC" runat="server" Text='<%#Bind("WS_CD_DESC")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--考核(資格)群組--%>
                    <asp:TemplateField HeaderText="考核(資格)群組" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" SortExpression="GRP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_GRP_NAME" runat="server" Text='<%#Bind("GRP_NAME")%>' Width="120px"></asp:Label>
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
