<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ3800_Qry.aspx.cs" Inherits="WebContent_WFB2SJ3800_Qry" Culture="auto" UICulture="auto" %>

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
            //部門編號取得部門的ajax
            $("#txt_DEPT_NO").change(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "WFB2SJ0260GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        cache: false,
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                              
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DEPT_NAME').val("");
                }
            });
            //工號取得姓名的ajax
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        cache: false,
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME').val("");
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

        //清空畫面
        function ClearAll() {
            $("#txt_ASSESS_YEAR").val("");
            $("#ddl_ASSESS_TYPE").val("-1");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#ddl_LEVEL_CD").val("-1");
            $("#ddl_SCORE_FINAL").val("-1");
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
                    <col width="23%" />
                    <col width="10%" />
                    <col width="23%" />
                    <col width="10%" />
                    <col width="23%" />
                </colgroup>
                <tbody>
                     <tr>  
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ASSESS_YEAR" runat="server" Text="考核年度"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_ASSESS_YEAR" runat="server" Width="60px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
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
                            <asp:DropDownList ID="ddl_ASSESS_TYPE" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"> 
                            </asp:DropDownList><asp:RequiredFieldValidator ID="req_ASSESS_TYPE" runat="server" 
                                            ErrorMessage="考核類別必輸入" InitialValue="-1"
                                            ControlToValidate="ddl_ASSESS_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text="部門名稱"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">                            
                           <asp:TextBox ID="txt_DEPT_NO" runat="server"  MaxLength="7" Width="64px" ClientIDMode="Static" CssClass="" ></asp:TextBox>
                                <input id="btnDEPTSearch" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />  
                            <asp:TextBox ID="txt_DEPT_NAME" runat="server" Width="100px"  ClientIDMode="Static" BorderWidth="0"  ></asp:TextBox>  
                            <asp:HiddenField ID="hid_MA_EMP_ID" runat="server" ClientIDMode="Static" /> 
                            <asp:HiddenField ID="hid_IS_SUPPER" runat="server" ClientIDMode="Static" /> 
                         </td>                       
                    </tr> 
                     <tr>  
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_EMP_ID" runat="server" Text="工號"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                          <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                                          <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="90px" ClientIDMode="Static" BorderWidth="0" ></asp:TextBox> 
                        </td>       
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_LEVEL_CD" runat="server" Text="資格"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                             <asp:DropDownList ID="ddl_LEVEL_CD" runat="server" Width="100px" ClientIDMode="Static" CssClass="" > </asp:DropDownList>
                        </td>                     
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_SCORE_FINAL" runat="server" Text="考核"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_SCORE_FINAL" runat="server" Width="100px" ClientIDMode="Static" CssClass=""> </asp:DropDownList>
                        </td>                        
                    </tr> 
                              
                    <tr>
                       
                        <th></th>
                        <th></th>
                        <td colspan="4" align="right" class="Body_label">
                            <div id="init">
                                <aces:Btn ID="WFB2SJ3800Search" runat="server" Text="查詢" OnClick="WFB2SJ3800Search_Click" OnClientClick="return CheckSearch();" />
                                <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="ClearAll();" />
                                <aces:Btn ID="WFB2SJ3800Dtl" runat="server" Text="考核資料" OnClick="WFB2SJ3800Dtl_Click" OnClientClick="" />
                                 
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

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SJ3800DAO" EnablePaging="True"
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
                     <asp:ControlParameter ControlID="hid_MA_EMP_ID"
                        Name="ma_emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="ddl_LEVEL_CD"
                        Name="level_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />   
                     <asp:ControlParameter ControlID="ddl_SCORE_FINAL"
                        Name="score_final" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />     
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />  
                     <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />                         
                   <asp:ControlParameter ControlID="hid_IS_SUPPER"
                        Name="is_supper" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <HeaderTemplate>
                           
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" Width="20px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="20px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="20px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--年度--%>
                    <asp:TemplateField HeaderText="年度" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_R_ASSESS_YEAR" runat="server" Text='<%#Bind("ASSESS_YEAR")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--考核類別--%>
                    <asp:TemplateField HeaderText="考核類別" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_R_ASSESS_TYPE" runat="server" Text='<%#Bind("ASSESS_TYPE_DESC")%>' Width="90px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="工號" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="姓名" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部級部門--%>
                    <asp:TemplateField HeaderText="部級部門名稱" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="DEPT_NO_20">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_DEPT_NAME_20" runat="server" Text='<%#Bind("DEPT_NAME_20")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--考核--%>
                    <asp:TemplateField HeaderText="考核" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="SCORE_FINAL">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_SCORE_FINAL" runat="server" Text='<%#Bind("SCORE_FINAL")%>' Width="50px"></asp:Label>
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
        
       
    </asp:UpdatePanel>
</asp:Content>
