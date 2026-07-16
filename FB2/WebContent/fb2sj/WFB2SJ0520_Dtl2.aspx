<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0520_Dtl2.aspx.cs" Inherits="WebContent_WFB2SJ0520_Dtl2" Culture="auto" UICulture="auto" %>

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
        function doViewComments(emp_id) {


            OpenSearch("Assess_Comments_Rec.aspx", "", "", "ASSESS_YEAR=" + document.getElementById("hid_ASSESS_YEAR").value + "&ASSESS_TYPE=" + document.getElementById("hid_ASSESS_TYPE").value + "&EMP_ID=" + emp_id);
        }
        function doViewFixRec(emp_id) {

            OpenSearch("Assess_Log_View.aspx", "", "", "ASSESS_YEAR=" + document.getElementById("hid_ASSESS_YEAR").value + "&ASSESS_TYPE=" + document.getElementById("hid_ASSESS_TYPE").value + "&EMP_ID=" + emp_id);
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;
            if (!Page_ClientValidate("GroupA")) {
                processed = false;
            }
            if (processed)
                BlockUI();
            if (!processed)
                $.unblockUI();
            return processed;
        }

        //處理斷行
        function procEnterText() {

            if (window.event.keyCode == 13) {
                document.getElementById("ContentPlaceHolder1_txt_MEMO").value = document.getElementById("ContentPlaceHolder1_txt_MEMO").value + "\n";
            }

            

        }

        function showTitleList() {

            if (document.getElementById("hid_WS_CD").value == "G") {
                document.getElementById("nonGTR").style.display = "none";
                document.getElementById("isGTR").style.display = "";
            } else {

                document.getElementById("nonGTR").style.display = "";
                document.getElementById("isGTR").style.display = "none";
            }

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
                        <table ID="TABLE_RATE_01" cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label"  runat="server">
                            <colgroup>
                                <col width="10%" />
                                <col width="40%" />
                                <col width="10%" />
                                <col width="40%" />
                            </colgroup>
                            <tbody>
                                 <tr>  
                                    <th align="left" class="Body_label">
                                        <asp:Label ID="lb_WS_CD" runat="server" Text="職種"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_WS_CD" runat="server"   CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>                            
                                            
                                    </td>                      
                                     <th align="left" class="Body_label">
                                        <asp:Label ID="lb_GRP_CD" runat="server" Text="考核群組"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_GRP_CD_DESC" runat="server"   CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                        <asp:HiddenField ID="hid_DEPT_LEVEL" runat="server" ClientIDMode="Static" />   
                                        <asp:HiddenField ID="hid_MA_EMP_ID" runat="server" ClientIDMode="Static" />   
                                        <asp:HiddenField ID="hid_MA_TYPE" runat="server" ClientIDMode="Static" />  
                                        <asp:HiddenField ID="hid_ASSESS_YEAR" runat="server" ClientIDMode="Static" />  
                                        <asp:HiddenField ID="hid_ASSESS_TYPE" runat="server" ClientIDMode="Static" />
                                        <asp:HiddenField ID="hid_EMP_ID" Value="" runat="server" ClientIDMode="Static" />   
                                          <asp:HiddenField ID="hid_RECOMM_DESC" Value="-1" runat="server" ClientIDMode="Static" />
                                          <asp:HiddenField ID="hid_SCORE_FINAL" Value="-1" runat="server" ClientIDMode="Static" />  
                                          <asp:HiddenField ID="hid_WS_CD" Value="-1" runat="server" ClientIDMode="Static" />  
                                          <asp:HiddenField ID="hid_GRP_CD" Value="-1" runat="server" ClientIDMode="Static" /> 
                                            <asp:HiddenField ID="hid_SUB_SIGN_YN" runat="server" ClientIDMode="Static" />     
                                            <asp:HiddenField ID="hid_SIGN_YN" runat="server" ClientIDMode="Static" />    
                                                                          
                                    </td>                      
                                </tr> 
                                         
                                <tr>
                                    <td colspan="4" >
                                        <table cellspacing="1" cellpadding="0" width="100%" border="0" class="Body_Label">
                                            <colgroup>
                                                <col width="20%" />
                                                <col width="10%" />
                                                <col width="10%" />
                                                <col width="10%" />
                                                <col width="10%" />
                                                <col width="10%" />
                                                <col width="10%" />
                                            </colgroup>
                                                <tbody>
                                                    <tr>
                                                        <th></th>
                                                         <th align="center" class="Body_label">
                                                            <asp:Label ID="lb_A" runat="server" Text="A"></asp:Label>
                                                        </th>
                                                         <th align="center" class="Body_label">
                                                            <asp:Label ID="lb_B" runat="server" Text="B"></asp:Label>
                                                        </th>
                                                         <th align="center" class="Body_label">
                                                            <asp:Label ID="lb_C" runat="server" Text="C"></asp:Label>
                                                        </th>
                                                         <th align="center" class="Body_label">
                                                            <asp:Label ID="lb_D" runat="server" Text="D"></asp:Label>
                                                        </th>
                                                         <th align="center" class="Body_label">
                                                            <asp:Label ID="lb_E" runat="server" Text="E"></asp:Label>
                                                        </th>
                                                         <th align="center" class="Body_label">
                                                            <asp:Label ID="lb_TOTAL" runat="server" Text="合計"></asp:Label>
                                                        </th>
                                                    </tr>
                                                    <tr id="nonGTR">
                                                        <th  align="right" class="Body_label">                                                            
                                                            <asp:Label ID="lb_SHOULD_ASSIGNED" runat="server" Text="應分配人數"></asp:Label>
                                                        </th>
                                                         <th align="center" class="Body_label" style="border:1px solid #aaa">
                                                             <asp:TextBox ID="txt_SOULD_A" runat="server" Width="30px" style="text-align: center;"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                                        </th>
                                                         <th align="center" class="Body_label" style="border:1px solid #aaa">
                                                             <asp:TextBox ID="txt_SOULD_B" runat="server" Width="30px" style="text-align: center;"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                                        </th>
                                                         <th align="center" class="Body_label" style="border:1px solid #aaa">
                                                             <asp:TextBox ID="txt_SOULD_C" runat="server" Width="30px"  style="text-align: center;" CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                                        </th>
                                                         <th align="center" class="Body_label" style="border:1px solid #aaa">
                                                             <asp:TextBox ID="txt_SOULD_D" runat="server" Width="30px" style="text-align: center;"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                                        </th>
                                                         <th align="center" class="Body_label" style="border:1px solid #aaa">
                                                             <asp:TextBox ID="txt_SOULD_E" runat="server" Width="30px" style="text-align: center;"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                                        </th>
                                                         <th align="center" class="Body_label" style="border:1px solid #aaa">
                                                             <asp:TextBox ID="txt_SOULD_TOTAL" runat="server" Width="30px" style="text-align: center;"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                                        </th>
                                                    </tr>
                                                    <tr id="isGTR" style="display:none">
                                                        <th  align="right" class="Body_label">                                                            
                                                            <asp:Label ID="Label3" runat="server" Text="應分配人數"></asp:Label>
                                                        </th>
                                                         <th align="center" class="Body_label" style="border:1px solid #aaa" colspan="6">
                                                             部門提出時一律據實考核
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th  align="right" class="Body_label">                                                            
                                                            <asp:Label ID="lb_ALLOCATED" runat="server" Text="已分配人數"></asp:Label>
                                                        </th>
                                                         <th align="center" class="Body_label" style="border:1px solid #aaa">
                                                             <asp:TextBox ID="txt_ALLOCATED_A" runat="server" Width="30px" style="text-align: center;"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                                        </th>
                                                         <th align="center" class="Body_label" style="border:1px solid #aaa">
                                                             <asp:TextBox ID="txt_ALLOCATED_B" runat="server" Width="30px" style="text-align: center;"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                                        </th>
                                                         <th align="center" class="Body_label" style="border:1px solid #aaa">
                                                             <asp:TextBox ID="txt_ALLOCATED_C" runat="server" Width="30px" style="text-align: center;"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                                        </th>
                                                         <th align="center" class="Body_label" style="border:1px solid #aaa">
                                                             <asp:TextBox ID="txt_ALLOCATED_D" runat="server" Width="30px" style="text-align: center;"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                                        </th>
                                                         <th align="center" class="Body_label" style="border:1px solid #aaa">
                                                             <asp:TextBox ID="txt_ALLOCATED_E" runat="server" Width="30px" style="text-align: center;"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                                        </th>
                                                         <th align="center" class="Body_label" style="border:1px solid #aaa">
                                                             <asp:TextBox ID="txt_ALLOCATED_TOTAL" runat="server" Width="30px" style="text-align: center;"  CssClass="txtDisabled"  Enabled="false" BorderWidth="0" ></asp:TextBox>
                                                        </th>
                                                    </tr>
                                                </tbody>
                                            </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <hr />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr height="100%" valign="top">
                    <td>
                         <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="25%" />
                                <col width="10%" />
                                <col width="25%" />                                
                                <col width="30%" />
                            </colgroup>
                            <tbody>
                                 <tr>  
                                    <th align="left" class="Body_label">
                                        <asp:Label ID="Label1" runat="server" Text="考核調整"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                       <asp:DropDownList ID="ddl_SCORE_FINAL" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"> </asp:DropDownList> 
                                         <asp:RequiredFieldValidator ID="Req_SCORE_FINAL" runat="server" 
                                            ErrorMessage="考核調整必輸入" InitialValue="-1"
                                            ControlToValidate="ddl_SCORE_FINAL" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>    
                                    </td>                      
                                     <th align="left" class="Body_label">
                                        <asp:Label ID="Label2" runat="server" Text="理由說明"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_MEMO" runat="server" Width="150px"  TextMode="MultiLine" Columns="40" Rows="3" runat="server" BorderWidth="1" CssClass="MandatoryField" onkeypress="return procEnterText();" onpaste="return true"></asp:TextBox> 
                                        <asp:RequiredFieldValidator ID="Req_MEMO" runat="server" 
                                            ErrorMessage="理由說明必輸入" InitialValue=""
                                            ControlToValidate="txt_MEMO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>   
                                     <td  align="right" class="Body_label">                                        
                                         <asp:Button ID="WFB2SJ0520AproveSave" runat="server" Text="修改確定" OnClick="WFB2SJ0520AproveSave_Click" OnClientClick="return saveCheck();"/>
                                         <asp:Button runat="server" ID="btn_cancel" Text="返回" OnClick="btn_Cancel_Click" />
                                    </td>                   
                                </tr>
                                </tbody>
                             </table> 
                    </td>
                </tr>
               
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SJ0520DAO" EnablePaging="True"
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
                     <asp:ControlParameter ControlID="hid_MA_EMP_ID"
                        Name="ma_emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                     <asp:ControlParameter ControlID="hid_WS_CD"
                        Name="ws_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />   
                     <asp:ControlParameter ControlID="hid_GRP_CD"
                        Name="grp_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />   
                     <asp:ControlParameter ControlID="hid_SCORE_FINAL"
                        Name="score_final" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />     
                    <asp:ControlParameter ControlID="hid_EMP_ID"
                        Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />  
                     <asp:ControlParameter ControlID="hid_RECOMM_DESC"
                        Name="recomm_desc" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                   
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1020px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
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
                             <asp:HiddenField ID="hid_SIGN_YN" runat="server"  Value='<%#Bind("SIGN_YN")%>' ClientIDMode="Static" />
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
                    <%--部門名稱--%>
                    <asp:TemplateField HeaderText="部門名稱" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" SortExpression="DEPT_NO">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_DEPT_NAME" runat="server" Text='<%#Bind("DEPT_NAME")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--外數區分--%>
                    <asp:TemplateField HeaderText="外數區分" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="IS_OUT">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_IS_OUT" runat="server" Text='<%#Bind("IS_OUT")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--資格--%>
                    <asp:TemplateField HeaderText="資格" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" SortExpression="LEVEL_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_LEVEL_CD_DESC" runat="server" Text='<%#Bind("LEVEL_CD")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--職務--%>
                    <asp:TemplateField HeaderText="職務" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" SortExpression="PJOB_CD">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_WS_CD_DESC" runat="server" Text='<%#Bind("PJOB_DESC")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--初核評分--%>
                    <asp:TemplateField HeaderText="初核評分" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="MNG_GRADE">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_MNG_GRADE" runat="server" Text='<%#Bind("MNG_GRADE")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--初核總評--%>
                    <asp:TemplateField HeaderText="初核總評" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="btn_COMMENTS"  Text="..." runat="server" ClientIDMode="Static"  OnClick="btn_COMMENTS_Click" CommandArgument='<%#Bind("EMP_ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--今回考核--%>
                    <asp:TemplateField HeaderText="今回考核" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="SCORE_FINAL">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_SCORE_FINAL" runat="server" Text='<%#Bind("SCORE_FINAL")%>' Width="60px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--考核修改次數--%>
                    <asp:TemplateField HeaderText="考核修改次數<BR>(*理由說明)" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" SortExpression="FIX_COUNT">
                        <ItemTemplate>
                            <asp:LinkButton ID="lk_FIX_COUNT" runat="server" Text='<%#Bind("FIX_COUNT")%>' OnClick="LB_FIX_COUNT_Click" CommandArgument='<%#Bind("EMP_ID") %>' Width="100px" ></asp:LinkButton>
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
                    <%--備註內容--%>
                    <asp:TemplateField HeaderText="備註內容" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" SortExpression="DISTING_REMARK">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_DISTING_REMARK" runat="server" Text='<%#Bind("M_MEMO")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--遲到/早退--%>
                    <asp:TemplateField HeaderText="遲到/早退" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" SortExpression="LEAVE_OP">
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
                    <asp:TemplateField HeaderText="事病假" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" SortExpression="LEAVE_AB">
                        <ItemTemplate>
                            <asp:Label ID="lb_R_LEAVE_AB" runat="server" Text='<%#Bind("LEAVE_AB")%>' Width="60px"></asp:Label>
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
        
      
    </asp:UpdatePanel>

</asp:Content>

