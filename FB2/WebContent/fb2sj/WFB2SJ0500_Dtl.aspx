<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0500_Dtl.aspx.cs" Inherits="WebContent_WFB2SJ0500_Dtl" Culture="auto" UICulture="auto" %>

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
                                $('#txt_EMP_NAME2').val("");
                                alert(JData.errMsg);
                            }
                            else {
                               
                                $('#txt_EMP_NAME2').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    alert("ok");
                    $('#txt_EMP_NAME2').val("");
                }
            });
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
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME2").val("");
            $("#ddl_SCORE_DEPT").val("-1");
            $("#ddl_IS_OUT").val("-1");
            $("#ddl_WS_CD").val("-1");
            $("#ddl_LEVEL_CD").val("-1");
            $("#ddl_IS_DR").val("-1");
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
                                <col width="25%" />
                                <col width="10%" />
                                <col width="25%" />
                                <col width="10%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                 <tr>  
                                   <%-- 考核年度 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_DIREC_EMP_NAME" runat="server" Text="提出人員"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                        <asp:HiddenField ID="hid_ASSESS_YEAR" runat="server" ClientIDMode="Static" />  
                                        <asp:HiddenField ID="hid_ASSESS_TYPE" runat="server" ClientIDMode="Static" /> 
                                        <asp:HiddenField ID="hid_DEPT_NO" runat="server" ClientIDMode="Static" /> 
                                        <asp:HiddenField ID="hid_DIRC_EMP_ID" runat="server" ClientIDMode="Static" /> 
                                        <asp:HiddenField ID="hid_DEPT_SIGN_YN" runat="server" ClientIDMode="Static" />                                 
                                    </td>               
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MNG_NUM" runat="server" Text="管理人數"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_MNG_NUM" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>
                                    <th></th>
                                    <th></th>
                                </tr> 
                                <tr>  
                                   <%--單位名稱--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="單位名稱"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>                                                     
                                    </td>               
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                </tr>
                                <tr>  
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="工號"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME2', 'N');" />
                                        <asp:TextBox ID="txt_EMP_NAME2" runat="server" Width="90px" ClientIDMode="Static" BorderWidth="0" ></asp:TextBox> 
                                    </td>                      
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SCORE_DEPT" runat="server" Text="考核"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SCORE_DEPT" runat="server" Width="100px" ClientIDMode="Static" CssClass=""> </asp:DropDownList>
                                    </td>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_DEPT_NO_20" runat="server" Text="外數區分"></asp:Label>:
                                    </th>
                                     <td align="left" class="Body_label">                             
                                         <asp:DropDownList ID="ddl_IS_OUT" runat="server" Width="100px" ClientIDMode="Static" CssClass=""> </asp:DropDownList>
                                     </td>
                                </tr>  
                    
                                <tr>  
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_LEVEL_CD" runat="server" Text="資格"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                          <asp:DropDownList ID="ddl_LEVEL_CD" runat="server" Width="100px" ClientIDMode="Static" CssClass=""> </asp:DropDownList>
                                    </td>                      
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_WS_CD" runat="server" Text="職種"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_WS_CD" runat="server" Width="100px" ClientIDMode="Static" CssClass=""> </asp:DropDownList>
                                    </td>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_IS_DR" runat="server" Text="備註內容"></asp:Label>:
                                    </th>
                                     <td align="left" class="Body_label">                             
                                         <asp:DropDownList ID="ddl_IS_DR" runat="server" Width="100px" ClientIDMode="Static" CssClass=""> </asp:DropDownList>
                                     </td>
                                </tr>           
                                <tr>
                       
                                    <th></th>
                                    <th></th>
                                    <td colspan="4" align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SJ0500EmpDtlSearch" runat="server" Text="查詢" OnClick="WFB2SJ0500EmpDtlSearch_Click"  />
                                        
                                            <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="ClearAll();" />
                                             <aces:Btn ID="WFB2SJ0500EmpScore" runat="server" Text="評分表" OnClick="WFB2SJ0500EmpScore_Click" OnClientClick="" />
                                             <asp:Button runat="server" ID="btn_cancel" Text="返回" OnClick="btn_Cancel_Click" />
                                             <aces:Btn ID="WFB2SJ0500Reference" runat="server" Text="參考資料XLS" OnClick="WFB2SJ0500Reference_Click" OnClientClick="" />
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

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDataDtl"
                SelectCountMethod="getCountDtl" TypeName="CFB2SJ0500DAO" EnablePaging="True"
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
                    <asp:ControlParameter ControlID="hid_DIRC_EMP_ID"
                        Name="dirc_emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" /> 
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />  
                   <asp:ControlParameter ControlID="ddl_SCORE_DEPT"
                        Name="score_dept" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="ddl_IS_OUT"
                        Name="is_out" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="ddl_LEVEL_CD"
                        Name="level_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="ddl_WS_CD"
                        Name="ws_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="ddl_IS_DR"
                        Name="is_dr" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1020px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
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
                             <asp:HiddenField ID="hid_SORT_LIMIT_RATE" runat="server"  Value='<%#Bind("SORT_LIMIT_RATE")%>' ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="工號" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="姓名" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--外數區分--%>
                    <asp:TemplateField HeaderText="外數區分" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="IS_OUT">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_IS_OUT" runat="server" Text='<%#Bind("IS_OUT")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格--%>
                    <asp:TemplateField HeaderText="資格" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" SortExpression="LEVEL_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_LEVEL_CD_DESC" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職務--%>
                    <asp:TemplateField HeaderText="職務" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="PJOB_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_WS_CD_DESC" runat="server" Text='<%#Bind("PJOB_DESC")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--初核評分--%>
                    <asp:TemplateField HeaderText="初核評分" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_R_MNG_GRADE" runat="server" Text='<%#Bind("MNG_GRADE")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--今回考核--%>
                    <asp:TemplateField HeaderText="今回考核" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="SCORE_DEPT">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_SCORE_DEPT" runat="server" Text='<%#Bind("SCORE_DIRC")%>' Width="70px"></asp:Label>
                            <asp:Label ID="lb_R_ORI_SCORE_DEPT" runat="server" Text='<%#Bind("SCORE_DIRC")%>' Width="70px" Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--前1年--%>
                    <asp:TemplateField HeaderText="前1年考核" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="SCORE_H_1">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_SCORE_H_1" runat="server" Text='<%#Bind("SCORE_H_1")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--前2年--%>
                    <asp:TemplateField HeaderText="前2年考核" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="SCORE_H_2">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_SCORE_H_2" runat="server" Text='<%#Bind("SCORE_H_2")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格年資--%>
                    <asp:TemplateField HeaderText="資格年資" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center"  SortExpression="RECENT_LEVEL_WORK_YEARS" >
                        <ItemTemplate>
                            <asp:Label ID="lb_R_RECENT_LEVEL_WORK_YEARS" runat="server" Text='<%#Bind("RECENT_LEVEL_WORK_YEARS")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--年齡--%>
                    <asp:TemplateField HeaderText="年齡" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center"  SortExpression="AGE">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_AGE" runat="server" Text='<%#Bind("AGE")%>' Width="50px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--入社年資--%>
                    <asp:TemplateField HeaderText="入社年資" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center"  SortExpression="WORK_YEARS" >
                        <ItemTemplate>
                            <asp:Label ID="lb_R_WORK_YEARS" runat="server" Text='<%#Bind("WORK_YEARS")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--備註內容--%>
                    <asp:TemplateField HeaderText="備註內容" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_DISTING_REMARK" runat="server" Text='<%#Bind("DISTING_REMARK")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                         
                    
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
        </ContentTemplate>
        
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SJ0500Reference"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

