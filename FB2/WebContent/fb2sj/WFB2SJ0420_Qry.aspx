<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0420_Qry.aspx.cs" Inherits="WebContent_WFB2SJ0420_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
     <style type="text/css">
        #txt_DISTING_CD {
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
            //工號取得姓名的ajax
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        cache: false,
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val(JData.EMP_NAME);
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
                                $('#txt_DEPT_NO').val("");
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
         
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#ddl_APPROVE").val("-1");
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
                    <col width="20%" />
                    <col width="10%" />
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
                            <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text="考核類型"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                             <asp:TextBox ID="txt_ASSESS_TYPE" runat="server" Width="60px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>                            
                              <asp:HiddenField ID="hid_ASSESS_TYPE" runat="server" ClientIDMode="Static" />                             
                              <asp:HiddenField ID="hid_CREATED_BY" runat="server" ClientIDMode="Static" />   
                                <asp:HiddenField ID="hid_MA_EMP_ID" runat="server" ClientIDMode="Static" />     
                                <asp:HiddenField ID="hid_MA_EMP_NAME" runat="server" ClientIDMode="Static" />    
                                <asp:HiddenField ID="hid_MA_TYPE" runat="server" ClientIDMode="Static" />    
                            <asp:HiddenField ID="hid_DEPT_NAME" runat="server" ClientIDMode="Static" />       
                            <asp:HiddenField ID="hid_DEPT_NO" runat="server" ClientIDMode="Static" />       
                            <asp:HiddenField ID="hid_DEPT_LEVEL" runat="server" ClientIDMode="Static" />         
                                                       
                        </td>                     
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_SCORE_SUGGEST_SCORE" runat="server" Text="要望考核"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_SUGGEST_SCORE" runat="server" Width="100px" ClientIDMode="Static" CssClass=""> </asp:DropDownList>
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
                            <asp:Label ID="lb_DEPT_NO_NEW" runat="server" Text="部門名稱"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label"  >  
                            <asp:TextBox ID="txt_DEPT_NO" runat="server"  MaxLength="7" Width="64px" ClientIDMode="Static" CssClass="" OnClientBlur="doTest();"></asp:TextBox>
                                <input id="btnDEPTSearch" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />  
                            <asp:TextBox ID="txt_DEPT_NAME" runat="server" Width="100px"  ClientIDMode="Static" BorderWidth="0" ></asp:TextBox>                                     
                            
                        </td>
                                             
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_APPROVE" runat="server" Text="審核結果"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_APPROVE" runat="server" Width="100px" ClientIDMode="Static" CssClass=""> </asp:DropDownList>
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
                                <aces:Btn ID="WFB2SJ0420Search" runat="server" Text="查詢" OnClick="WFB2SJ0420Search_Click" OnClientClick="return CheckSearch();" />
                           
                                <%--<asp:Button ID="WFB2IA0100Search" runat="server" Text="<%$Resources:Resource,wfb2ia_WFB2IA0100Search%>" OnClick="WFB2IA0100Search_Click" OnClientClick="BlockUI();" />--%>
                                            
                                <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="ClearAll();" />
                            </div>
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
                                <aces:Btn ID="WFB2SJ0420Approve" runat="server" Text="核定作業" Visible="true" OnClick="WFB2SJ0420Approve_Click" OnClientClick="BlockUI();" />
                                <aces:Btn ID="WFB2SJ0420Export" runat="server" Text="考核統計表下載" Visible="true" OnClick="WFB2SJ0420Export_Click" OnClientClick="BlockUI();" />
                                
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SJ0420DAO" EnablePaging="True"
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
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />  
                     <asp:ControlParameter ControlID="ddl_APPROVE"
                        Name="approve" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />  
                    <asp:ControlParameter ControlID="hid_CREATED_BY"
                        Name="ma_emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />  
                    <asp:ControlParameter ControlID="ddl_SUGGEST_SCORE"
                        Name="suggest_score" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />  
                    <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" /> 
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1120px"
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="30px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="30px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="工號" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="姓名" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--今年
                    <asp:TemplateField HeaderText="今回考核" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="SCORE_FINAL">
                        <ItemTemplate>
                            <asp:Label ID="lb_SCORE_FINAL" runat="server" Text='<%#Bind("SCORE_FINAL")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField> --%> 
                    <%--要望--%>
                    <asp:TemplateField HeaderText="要望考核" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="SUGGEST_SCORE">
                        <ItemTemplate>
                            <asp:Label ID="lb_SUGGEST_SCORE" runat="server" Text='<%#Bind("SUGGEST_SCORE")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>    
                    <%--填表人--%>
                    <asp:TemplateField HeaderText="填表人" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="SUGGEST_EMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_SUGGEST_EMP_NAME" runat="server" Text='<%#Bind("SUGGEST_EMP_NAME")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>    
                    <%--職種--%>
                    <asp:TemplateField HeaderText="職種" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="WS_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_WS_CD" runat="server" Text='<%#Bind("WS_CD")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                    <%--資格--%>
                    <asp:TemplateField HeaderText="資格" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="LEVEL_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--職務--%>
                    <asp:TemplateField HeaderText="職務" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Center" SortExpression="PJOB_DESC">
                        <ItemTemplate>
                            <asp:Label ID="lb_PJOB_DESC" runat="server" Text='<%#Bind("PJOB_DESC")%>' Width="90px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--前1年度--%>
                    <asp:TemplateField HeaderText="前1年考核" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="SCORE_H_1">
                        <ItemTemplate>
                            <asp:Label ID="lb_SCORE_H_1" runat="server" Text='<%#Bind("SCORE_H_1")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>    
                    <%--前2年度--%>
                    <asp:TemplateField HeaderText="前2年考核" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="SCORE_H_2">
                        <ItemTemplate>
                            <asp:Label ID="lb_SCORE_H_2" runat="server" Text='<%#Bind("SCORE_H_2")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                 
                    <%--經理審核--%>
                    <asp:TemplateField HeaderText="經理審核" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_AUDRESULT1_YN_DESC" runat="server" Text='<%#Bind("AUDRESULT1_YN_DESC")%>' Width="60px"></asp:Label>
                           <asp:HiddenField ID="hid_AUDRESULT1_YN" runat="server" ClientIDMode="Static" Value='<%#Bind("AUDRESULT1_YN")%>' /> 
                           <asp:HiddenField ID="hid_DEPT20_EMP_ID" runat="server" ClientIDMode="Static" Value='<%#Bind("DEPT20_EMP_ID")%>' /> 
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--理事審核--%>
                    <asp:TemplateField HeaderText="理事審核" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_AUDRESULT2_YN_DESC" runat="server" Text='<%#Bind("AUDRESULT2_YN_DESC")%>' Width="60px"></asp:Label>
                            <asp:HiddenField ID="hid_AUDRESULT2_YN" runat="server" ClientIDMode="Static" Value='<%#Bind("AUDRESULT2_YN")%>' />
                           <asp:HiddenField ID="hid_MA_B_EMP_ID" runat="server" ClientIDMode="Static" Value='<%#Bind("MA_B_EMP_ID")%>' /> 
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <%--協理審核--%>
                    <asp:TemplateField HeaderText="理事/協理審核" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_AUDRESULT3_YN_DESC" runat="server" Text='<%#Bind("AUDRESULT3_YN_DESC")%>' Width="70px"></asp:Label>
                            <asp:HiddenField ID="hid_AUDRESULT3_YN" runat="server" ClientIDMode="Static" Value='<%#Bind("AUDRESULT3_YN")%>' />
                           <asp:HiddenField ID="hid_MA_A_EMP_ID" runat="server" ClientIDMode="Static" Value='<%#Bind("MA_A_EMP_ID")%>' /> 
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
            <asp:PostBackTrigger ControlID="WFB2SJ0420Export"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
