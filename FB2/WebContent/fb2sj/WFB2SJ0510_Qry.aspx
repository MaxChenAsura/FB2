<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0510_Qry.aspx.cs" Inherits="WebContent_WFB2SJ0510_Qry" Culture="auto" UICulture="auto" %>

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
            var processed = true;
            BlockUI();
            return processed;
        }

        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_ASSESS_YEAR").val("");
            $("#ddl_ASSESS_TYPE").val("-1");
            $("#ddl_WS_CD").val("-1");
            $("#ddl_SCORE_LEVEL_GROUP").val("-1");
            $("#ddl_RECOMM_DESC").val("-1");
            $("#ddl_SCORE_FINAL").val("-1");
        }
        //送出簽核確認
        function SignConfirm(sType) {
            var confirmMsg = "是否完成所有群組複核, 確定提出嗎?";
            if (sType == "R") confirmMsg = "是否完成所有群組複核, 確定提出嗎?";
            if (confirm(confirmMsg)) {
                //if (document.getElementById("hid_EMP_SUGGEST_COUNT").value == "0") {
                //    if (confirm("本部門無要望申請書,是否繼續 ?")) {
                if (sType == "R") {
                        return true;
                } else {
                    if ($("#hid_EMP_POINT_MSG").val() != "") {
                        if (confirm($("#hid_EMP_POINT_MSG").val()+",是否繼續?")) {
                            return true;
                        } else {
                            return false;
                        }
                    } else {
                        return true;
                    }

                }
                      
                 //   } else {
                 //       return false;
                //    }
               // }
            } else {
                return false;
            }
            

        }
        function doViewComments(emp_id) {
            
           
            OpenSearch("Assess_Comments_Rec.aspx", "", "", "ASSESS_YEAR=" + document.getElementById("hid_ASSESS_YEAR").value + "&ASSESS_TYPE=" + document.getElementById("hid_ASSESS_TYPE").value + "&EMP_ID=" + emp_id);
        }
        function doViewFixRec(emp_id ,fun_id) {
          
            OpenSearch("Assess_Log_View.aspx", "", "", "ASSESS_YEAR=" + document.getElementById("hid_ASSESS_YEAR").value + "&ASSESS_TYPE=" + document.getElementById("hid_ASSESS_TYPE").value + "&EMP_ID=" + emp_id+"&FUN_ID="+fun_id);
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
                            <asp:TextBox ID="txt_ASSESS_YEAR" runat="server" Width="60px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>                            
                              <asp:HiddenField ID="hid_ASSESS_YEAR" runat="server" ClientIDMode="Static" />   
                        </td>                      
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text="考核類別"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                             <asp:TextBox ID="txt_ASSESS_TYPE" runat="server" Width="60px"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>                            
                              <asp:HiddenField ID="hid_ASSESS_TYPE" runat="server" ClientIDMode="Static" />                                  
                        </td>
                         <th></th>
                         <th>                             
                            <asp:HiddenField ID="hid_DEPT_NO_20" runat="server" ClientIDMode="Static" /> 
                            <asp:HiddenField ID="hid_DEPT_NO" runat="server" ClientIDMode="Static" />    
                            <asp:HiddenField ID="hid_DEPT_LEVEL" runat="server" ClientIDMode="Static" />  
                            <asp:HiddenField ID="hid_DEPT_NAME" runat="server" ClientIDMode="Static" /> 
                            <asp:HiddenField ID="hid_EMP_SUGGEST_COUNT" runat="server" ClientIDMode="Static" />  
                            <asp:HiddenField ID="hid_SIGN_YN" runat="server" ClientIDMode="Static" />        
                            <asp:HiddenField ID="hid_SIGN_YN_DEPT" runat="server" ClientIDMode="Static" />  
                            <asp:HiddenField ID="hid_SUB_SIGN_YN" runat="server" ClientIDMode="Static" />       
                            <asp:HiddenField ID="hid_DEPT_EMP_ID" runat="server" ClientIDMode="Static" />        
                            <asp:HiddenField ID="hid_EMP_POINT_MSG" runat="server" ClientIDMode="Static" />     
                         </th>                        
                    </tr> 
                     <tr>  
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_WS_CD" runat="server" Text="職種"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                             <asp:DropDownList ID="ddl_WS_CD" runat="server" Width="100px" ClientIDMode="Static" CssClass="" OnSelectedIndexChanged = "ddl_WS_CD_Changed"  AutoPostBack="True"> </asp:DropDownList>
                        </td>                      
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_SCORE_LEVEL_GROUP" runat="server" Text="點數群組"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_SCORE_LEVEL_GROUP" runat="server" Width="100px" ClientIDMode="Static" CssClass=""> </asp:DropDownList>
                        </td>                         
                         <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_RECOMM_DESC" runat="server" Text="推薦區分"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_RECOMM_DESC" runat="server" Width="100px" ClientIDMode="Static" CssClass=""> </asp:DropDownList>
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
                            <asp:Label ID="lbl_SCORE_FINAL" runat="server" Text="今回考核"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:DropDownList ID="ddl_SCORE_FINAL" runat="server" Width="100px" ClientIDMode="Static" CssClass=""> </asp:DropDownList>
                        </td> 
                         <th></th>
                         <th></th>                        
                    </tr> 
                              
                    <tr>
                       
                        <th></th>
                        <th></th>
                        <td colspan="4" align="right" class="Body_label">
                            <div id="init">
                                <aces:Btn ID="WFB2SJ0510Search" runat="server" Text="查詢" OnClick="WFB2SJ0510Search_Click" OnClientClick="return CheckSearch();" />
                                <aces:Btn ID="WFB2SJ0510Situation" runat="server" Text="提出現況" OnClick="WFB2SJ0510Situation_Click" OnClientClick="" />
                                <aces:Btn ID="WFB2SJ0510Approved" runat="server" Text="核定作業" OnClick="WFB2SJ0510Approved_Click" OnClientClick="" />
                                <aces:Btn ID="WFB2SJ0510Statistics" runat="server" Text="考核統計表" OnClick="WFB2SJ0510Statistics_Click" OnClientClick="" />
                                <aces:Btn ID="WFB2SJ0510Sign" runat="server" Text="提出簽核" Visible="false" OnClick="WFB2SJ0510Sign_Click" OnClientClick="return SignConfirm('M');" />
                                <aces:Btn ID="WFB2SJ0510DeptSign" runat="server" Text="提出複核" Visible="false" OnClick="WFB2SJ0510Sign_Click" OnClientClick="return SignConfirm('R');" />
                                <asp:Button ID="btn_back" runat="server" Text="退回" Visible="false" OnClick="WFB2SJ0510Back_Click"  />
                                <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2ia_btn_clear%>" OnClientClick="ClearAll();" />
                                <aces:Btn ID="WFB2SJ0510Refer" runat="server" Text="參考資料" OnClick="WFB2SJ0510Refer_Click" OnClientClick="" />
                                 
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
                SelectCountMethod="getCount" TypeName="CFB2SJ0510DAO" EnablePaging="True"
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
                     <asp:ControlParameter ControlID="hid_DEPT_NO"
                        Name="dept_no" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="ddl_WS_CD"
                        Name="ws_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />   
                     <asp:ControlParameter ControlID="ddl_SCORE_LEVEL_GROUP"
                        Name="score_level_group" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />   
                     <asp:ControlParameter ControlID="ddl_SCORE_FINAL"
                        Name="score_final" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />                        
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />  
                     <asp:ControlParameter ControlID="ddl_RECOMM_DESC"
                        Name="recomm_desc" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" /> 
                     <asp:ControlParameter ControlID="hid_DEPT_EMP_ID"
                        Name="dept_emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />                         
                   
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1500px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="30px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="30px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="工號" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="姓名" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="60px"></asp:Label>
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
                    <asp:TemplateField HeaderText="初核評分" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center"  SortExpression="MNG_GRADE">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_MNG_GRADE" runat="server" Text='<%#Bind("MNG_GRADE")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--初核總評--%>
                    <asp:TemplateField HeaderText="初核總評" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                             <asp:Button ID="btn_COMMENTS"  Text="..." runat="server" ClientIDMode="Static"  OnClick="btn_COMMENTS_Click" CommandArgument='<%#Bind("EMP_ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--今回考核--%>
                    <asp:TemplateField HeaderText="今回考核" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="SCORE_DEPT">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_SCORE_DEPT" runat="server" Text='<%#Bind("SCORE_DEPT")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--考核修改次數--%>
                    <asp:TemplateField HeaderText="考核修改次數<BR>(*理由說明)" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:LinkButton ID="lk_FIX_COUNT" runat="server" Text='<%#Bind("FIX_COUNT")%>' OnClick="LB_FIX_COUNT_Click" CommandArgument='<%#Bind("EMP_ID")%>' Width="80px"  ></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--前1年--%>
                    <asp:TemplateField HeaderText="前1年考核" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center"  SortExpression="SCORE_H_1">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_SCORE_H_1" runat="server" Text='<%#Bind("SCORE_H_1")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--前2年--%>
                    <asp:TemplateField HeaderText="前2年考核" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="SCORE_H_2">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_SCORE_H_2" runat="server" Text='<%#Bind("SCORE_H_2")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格年資--%>
                    <asp:TemplateField HeaderText="資格年資" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="RECENT_LEVEL_WORK_YEARS">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_RECENT_LEVEL_WORK_YEARS" runat="server" Text='<%#Bind("RECENT_LEVEL_WORK_YEARS")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--年齡--%>
                    <asp:TemplateField HeaderText="年齡" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" SortExpression="AGE">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_AGE" runat="server" Text='<%#Bind("AGE")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--入社年資--%>
                    <asp:TemplateField HeaderText="入社年資" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="WORK_YEARS">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_WORK_YEARS" runat="server" Text='<%#Bind("WORK_YEARS")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--備考內容--%>
                    <asp:TemplateField HeaderText="備註內容" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_R_DISTING_REMARK" runat="server" Text='<%#Bind("DISTING_REMARK")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--遲到/早退--%>
                    <asp:TemplateField HeaderText="遲到/早退" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center"  SortExpression="LEAVE_OP">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_LEAVE_OP" runat="server" Text='<%#Bind("LEAVE_OP")%>' Width="70px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--曠職--%>
                    <asp:TemplateField HeaderText="曠職" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="LEAVE_Q">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_LEAVE_Q" runat="server" Text='<%#Bind("LEAVE_Q")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--事病假--%>
                    <asp:TemplateField HeaderText="事病假" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center"   SortExpression="LEAVE_AB">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_LEAVE_AB" runat="server" Text='<%#Bind("LEAVE_AB")%>' Width="60px"></asp:Label>
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
            <asp:PostBackTrigger ControlID="WFB2SJ0510Statistics"></asp:PostBackTrigger>
        </Triggers>
         <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SJ0510Refer"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
