<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ3600_Score.aspx.cs" Inherits="WebContent_WFB2SJ3600_Score" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        $(function () {
            //除了input text外，停用Backsapce，防止不慎回前一頁
            $(document).keydown(function (e) {
                //採用demo建議 多考量"textarea"
                if (e.which == 8 && !(e.target.type == "text" || e.target.type == "textarea"))
                    e.preventDefault();
            });
        });
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".number").mask('999');
            $(".number2").mask('999');
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
        function txtKeyNumber() {
           
            $("#txt_COMMENT").val($("#txt_COMMENT").val() + window.event.keyCode);

            return;
            if (!(((window.event.keyCode >= 48) && (window.event.keyCode <= 57)) ||
            (window.event.keyCode == 13) || (window.event.keyCode == 46) ||
            (window.event.keyCode == 45)))
                //這段是判斷如果輸入的不是數字或小數點!那將無法輸入文字
            {
                window.event.keyCode = 0;
            }
        }
        //處理斷行
        function procEnterText() {
           
            if (window.event.keyCode == 13) {
                document.getElementById("ContentPlaceHolder1_txt_COMMENT").value = document.getElementById("ContentPlaceHolder1_txt_COMMENT").value + "\n";
            }
            /**
            if (document.getElementById("ContentPlaceHolder1_txt_COMMENT").value.length > 10) {
                
                document.getElementById("ContentPlaceHolder1_txt_COMMENT").value=
                    document.getElementById("ContentPlaceHolder1_txt_COMMENT").value.substr(0, 10);
            }**/
            //return false;

        }
        
        function doMngGradOnBlur(obj) {
           
            var cell;
            var topCell;
            var maxGrade = 0;
            var itemDesc = "";
            if (obj.parentNode.tagName == 'DIV') {
                cell = obj.parentNode.parentNode.parentNode;
                maxGrade = cell.cells[1].childNodes[0].getElementsByTagName("input")[0].value;
                itemDesc = cell.cells[0].childNodes[0].getElementsByTagName("span")[0].innerHTML;
                topCell = obj.parentNode.parentNode.parentNode.parentNode;
            } else {
                cell = obj.parentNode.parentNode;
                maxGrade = cell.cells[1].getElementsByTagName("input")[0].value;
                itemDesc = cell.cells[0].getElementsByTagName("span")[0].innerHTML;
                topCell = obj.parentNode.parentNode.parentNode;
            }
            //alert(pCell.cells[1].childNodes[0].getElementsByTagName("input")[0].value);
            //alert(document.getElementById("hid_RATE_A").value);
            if (parseInt(obj.value) > parseInt(maxGrade)) {
                obj.value = "0";
                alert(itemDesc + "分數,不可大於最高分");
                return;
            }
            //count total
            
            var totalGrand = 0;
            for (var i = 1; i < topCell.childNodes.length-1; i++) {
                
                if (topCell.childNodes[i].childNodes[3].childNodes[0].tagName == "DIV") {
                    totalGrand += parseInt(topCell.childNodes[i].childNodes[3].childNodes[0].getElementsByTagName("input")[0].value);
                    //alert(topCell.childNodes[i].childNodes[3].childNodes[0].getElementsByTagName("input")[0].value);
                } else {
                    totalGrand += parseInt(topCell.childNodes[i].childNodes[3].getElementsByTagName("input")[0].value);
                    //alert(topCell.childNodes[i].childNodes[3].getElementsByTagName("input")[0].value);
                }
                //alert(document.getElementsByName("txt_MNG_GRADE")[i-1].value);
                //totalGrand += parseInt(topCell.childNodes[i].childNodes[3].getElementsByTagName("input")[0].value);
            }
            document.getElementById("ContentPlaceHolder1_txt_MNG_GRADE_TOTAL").value = totalGrand;
           
            var rateArray = ["A", "B", "C", "D", "E"];
            var scoreDept = "";
            for (var j = 0; j < rateArray.length; j++) {
                var rateRang = document.getElementById("hid_RATE_" + rateArray[j]).value.split(";");
                if (totalGrand <= parseInt(rateRang[0]) && totalGrand >= parseInt(rateRang[1])) {
                    scoreDept = rateArray[j];
                }
            }
            //check LIMIT_RATE
            /**
            var limitRate = document.getElementById("hid_LIMIT_RATE").value;
            if (limitRate != "") {
                if (limitRate.length > 1) {
                    if (limitRate.indexOf(scoreDept) < 0) {
                        alert("考課等第僅限於" + limitRate + ",請重調整分數內容");
                        return;
                    }

                }

            }
            ***/

            document.getElementById("ContentPlaceHolder1_txt_SCORE_DEPT").value = scoreDept;
            //推薦說明
            var rDesc = "";
            if (scoreDept == "A") rDesc = "A考核";
            if (document.getElementById("hid_WS_CD").value == "G") {
                if (scoreDept.charCodeAt() <= 67) {
                    if (rDesc != "") rDesc += "/";
                    rDesc += "業務職C";
                }
            }
            var preScore = "";
            if (document.getElementById("hid_ASSESS_TYPE").value == "1") {
                preScore = document.getElementById("ContentPlaceHolder1_txt_SCORE_1H_1").value;
            } else {
                preScore = document.getElementById("ContentPlaceHolder1_txt_SCORE_2H_1").value;
            }
            if (preScore != "") {
                if (preScore.charCodeAt() - scoreDept.charCodeAt() >= 2) {
                    if (rDesc != "") rDesc += "/";
                    rDesc += "向上兩級";
                }
            }
            document.getElementById("ContentPlaceHolder1_txt_RECOMM_DESC").value = rDesc;
            if (rDesc != "") {
                //document.getElementById("ContentPlaceHolder1_txt_COMMENT").className = "MandatoryField";
            } else {
                //document.getElementById("ContentPlaceHolder1_txt_COMMENT").className = "";
            }
            /**var rA = 0;
            var rB = 0;
            var rC = 0;
            var rD = 0;
            var rE = 0;

            if ($("#txt_RATE_A").val() != "") rA = $("#txt_RATE_A").val();
            if ($("#txt_RATE_B").val() != "") rB = $("#txt_RATE_B").val();
            if ($("#txt_RATE_C").val() != "") rC = $("#txt_RATE_C").val();
            if ($("#txt_RATE_D").val() != "") rD = $("#txt_RATE_D").val();
            if ($("#txt_RATE_E").val() != "") rE = $("#txt_RATE_E").val();

            $("#txt_RATE_TOTAL").val(parseInt(rA) + parseInt(rB) + parseInt(rC) + parseInt(rD) + parseInt(rE));
            **/
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                 <tr>  
                                   <%-- 考核年度 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_ASSESS_YEAR" runat="server" Text="考核年度"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_ASSESS_YEAR" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>               
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text="考核類別"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_ASSESS_TYPE_DESC" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox> 
                                        <asp:HiddenField ID="hid_ASSESS_TYPE" runat="server" ClientIDMode="Static" /> 
                                        <asp:HiddenField ID="hid_LIMIT_RATE" runat="server" ClientIDMode="Static" /> 
                                        <asp:HiddenField ID="hid_DEPT_SIGN_YN" runat="server" ClientIDMode="Static" /> 
                                    </td>              
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DIREC_EMP" runat="server" Text="提出主管"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_DIREC_EMP" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>
                                </tr> 
                                <tr>  
                                   <%--部門名稱(全名)--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NAME" runat="server" Text="部門名稱"></asp:Label>:</th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" Width="250px" CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>                                                     
                                        <asp:HiddenField ID="hid_DEPT_NO" runat="server" ClientIDMode="Static" /> 
                                    </td>  
                                </tr>
                                <tr>  
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="工號"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>                      
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME" runat="server" Text="姓名"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_WS_CD" runat="server" Text="職種"></asp:Label>:
                                    </th>
                                     <td align="left" class="Body_label">                             
                                        <asp:TextBox ID="txt_WS_CD_DESC" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                          <asp:HiddenField ID="hid_WS_CD" runat="server" ClientIDMode="Static" /> 
                                     </td>
                                </tr> 
                                <tr>  
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEVEL_CD" runat="server" Text="資格"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LEVEL_CD" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>                      
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PJOB_TYPE_DESC" runat="server" Text="職務名稱"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PJOB_TYPE_DESC" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>
                                     <th >
                                    </th>
                                     <th >
                                    </th>
                                </tr> 
                                <tr>  
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_AGE" runat="server" Text="年齡"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_AGE" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>                      
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PJOB_WORK_YEARS" runat="server" Text="職務年資"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_PJOB_WORK_YEARS" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SERVICE_YEARS" runat="server" Text="服務年資"></asp:Label>:
                                    </th>
                                     <td align="left" class="Body_label">                             
                                        <asp:TextBox ID="txt_SERVICE_YEARS" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                     </td>
                                </tr>  
                     
                    
                                <tr>  
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_DISTING_REMARK" runat="server" Text="備註內容"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                         <asp:TextBox ID="txt_DISTING_REMARK" TextMode="MultiLine" Columns="50" Rows="2" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="1"/>
                                    </td> 
                                </tr> 
                                 
                                <tr>  
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="考核履歷"></asp:Label>
                                    </th>
                                    <td align="center" class="Body_TableHeader">
                                        <asp:Label ID="Label4" runat="server" style="text-align: center;" Text="能力"></asp:Label>
                                    </td>                      
                                     <th align="center" class="Body_TableHeader">
                                        <asp:Label ID="Label2" runat="server" style="text-align: center;" Text="業績"></asp:Label>
                                    </th>
                                    <td align="left" class="">                                       
                                    </td>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label3" runat="server" Text="勤怠記錄"></asp:Label>:
                                    </th>
                                     <td align="left" class="Body_TableHeader">                             
                                      
                                     </td>
                                </tr> 
                                <tr>  
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SCORE_H_1" runat="server" Text="前1年"></asp:Label>:
                                    </th>
                                    <td align="center" class="Body_label">
                                        <asp:TextBox ID="txt_SCORE_1H_1" runat="server"  CssClass="txtDisabled" style="text-align: center;" Enabled="false" BorderWidth="1" ></asp:TextBox>
                                    </td> 
                                    <td align="center" class="Body_label">
                                        <asp:TextBox ID="txt_SCORE_2H_1" runat="server"  CssClass="txtDisabled" style="text-align: center;" Enabled="false" BorderWidth="1" ></asp:TextBox>
                                    </td> 
                                    <td align="left" class="Body_label">
                                    </td>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_OP" runat="server" Text="遲/早(次)"></asp:Label>:
                                    </th>
                                     <td align="left" class="Body_label">                             
                                        <asp:TextBox ID="txt_LEAVE_OP" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                     </td>
                                </tr>  
                                <tr>  
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SCORE_H_2" runat="server" Text="前2年"></asp:Label>:
                                    </th>
                                    <td align="center" class="Body_label">
                                        <asp:TextBox ID="txt_SCORE_1H_2" runat="server"  CssClass="txtDisabled" style="text-align: center;" Enabled="false" BorderWidth="1" ></asp:TextBox>
                                    </td> 
                                    <td align="center" class="Body_label">
                                        <asp:TextBox ID="txt_SCORE_2H_2" runat="server"  CssClass="txtDisabled" style="text-align: center;" Enabled="false" BorderWidth="1" ></asp:TextBox>
                                    </td> 
                                    <td align="left" class="Body_label">
                                    </td>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_Q" runat="server" Text="曠職(天)"></asp:Label>:
                                    </th>
                                     <td align="left" class="Body_label">                             
                                        <asp:TextBox ID="txt_LEAVE_Q" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                     </td>
                                </tr>   
                                <tr>  
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SCORE_H_3" runat="server"  Text="前3年"></asp:Label>:
                                    </th>
                                    <td align="center" class="Body_label">
                                        <asp:TextBox ID="txt_SCORE_1H_3" runat="server"  CssClass="txtDisabled" style="text-align: center;" Enabled="false" BorderWidth="1" ></asp:TextBox>
                                    </td> 
                                    <td align="center" class="Body_label">
                                        <asp:TextBox ID="txt_SCORE_2H_3" runat="server"  CssClass="txtDisabled" style="text-align: center;" Enabled="false" BorderWidth="1" ></asp:TextBox>
                                    </td> 
                                    <td align="left" class="Body_label">
                                    </td>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_AB" runat="server" Text="事/病假(天)"></asp:Label>:
                                    </th>
                                     <td align="left" class="Body_label">                             
                                        <asp:TextBox ID="txt_LEAVE_AB" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                     </td>
                                </tr> 
                                <tr>
                                    <td colspan="6">
                                       <asp:Label ID="lbl_ASSESS_RATE" runat="server" Text="【評分範圍】"></asp:Label>
                                    </td>
                                </tr>
                                    
                                <tr>
                                    <td colspan="6">
                                       <table cellspacing="0" cellpadding="0" width="100%" border="1" class="Body_Label">
                                           <tr>
                                               <th align="center" class="Body_TableHeader"> <asp:Label ID="lbl_RATE_1" runat="server" Text="考核區分"></asp:Label></th>
                                               <th align="center" class="Body_TableHeader"> <asp:Label ID="lbl_RATE_A" runat="server" Text="A"></asp:Label></th>
                                               <th align="center" class="Body_TableHeader"> <asp:Label ID="lbl_RATE_B" runat="server" Text="B"></asp:Label></th>
                                               <th align="center" class="Body_TableHeader"> <asp:Label ID="lbl_RATE_C" runat="server" Text="C"></asp:Label></th>
                                               <th align="center" class="Body_TableHeader"> <asp:Label ID="lbl_RATE_D" runat="server" Text="D"></asp:Label></th>
                                               <th align="center" class="Body_TableHeader"> <asp:Label ID="lbl_RATE_E" runat="server" Text="E"></asp:Label></th>
                                           </tr>
                                           <tr>
                                               <th align="center" class="Body_TableHeader"> <asp:Label ID="lbl_RATE_2" runat="server" Text="評分範圍"></asp:Label></th>                                               
                                                <td align="center" class="Body_label"> 
                                                    <asp:Label ID="lbl_RATE_A_VAL" runat="server" Text="0"></asp:Label>                            
                                               
                                                </td>                                             
                                                <td align="center" class="Body_label">                             
                                               
                                                    <asp:Label ID="lbl_RATE_B_VAL" runat="server" Text="0"></asp:Label> 
                                                </td>                                             
                                                <td align="center" class="Body_label">
                                                    <asp:Label ID="lbl_RATE_C_VAL" runat="server" Text="0"></asp:Label> 
                                                </td>                                             
                                                <td align="center" class="Body_label">                             
                                               <asp:Label ID="lbl_RATE_D_VAL" runat="server" Text="0"></asp:Label> 
                                                </td>                                             
                                                <td align="center" class="Body_label">             
                                               <asp:Label ID="lbl_RATE_E_VAL" runat="server" Text="0"></asp:Label> 
                                                    
                                                <asp:HiddenField ID="hid_RATE_A" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hid_RATE_B" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hid_RATE_C" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hid_RATE_D" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hid_RATE_E" runat="server" ClientIDMode="Static" />
                                                </td>
                                           </tr>
                                       </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                       <asp:Label ID="Label5" runat="server" Text="【考核評分欄】"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6">
                                       <asp:Label ID="lbl_ASSESS_TYPE_CONTENT" runat="server" Text="能力考課內容"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
               
            </table>
             
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getEmpAssessScore"
                SelectCountMethod="getEmpAssessScoreCount" TypeName="CFB2SJ3600DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                     <asp:ControlParameter ControlID="txt_ASSESS_YEAR"
                        Name="assess_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                     <asp:ControlParameter ControlID="hid_ASSESS_TYPE"
                        Name="assess_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />  
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="320px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging" >
                <Columns>
                    <%--評分項目--%> 
                    <asp:TemplateField HeaderText="考核評價要素" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:Label ID="lb_R_ITEM_DESC" runat="server" Text='<%#Bind("ITEM_DESC")%>' Width="150px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                    <%--最高分--%>
                    <asp:TemplateField HeaderText="最高分" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                             <asp:TextBox ID="txt_MAX_GRADE" runat="server" Text='<%#Bind("MAX_GRADE")%>' CssClass="txtDisabled" style="text-align: center;" Enabled="false" BorderWidth="0" Width="80px" ></asp:TextBox> 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--分數--%>
                    <asp:TemplateField HeaderText="分數" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                             <asp:TextBox ID="txt_MNG_GRADE" SYSTEX="1234" runat="server" MaxLength="3" Width="80px" Text='<%#Bind("MNG_GRADE")%>'  style="text-align: center;" ClientIDMode="Static" AutoPostBack="True"  ontextchanged="MNG_GRADE_Change" CssClass="MandatoryField number"></asp:TextBox> 
                             <asp:Label ID="lb_MNG_GRADE" runat="server" Text='<%#Bind("MNG_GRADE")%>' Width="80px" Visible="false"></asp:Label>                                       
                        </ItemTemplate>
                    </asp:TemplateField>             
                    
                </Columns>
                
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px;display:none">
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
            <table width="480px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="0" cellpadding="0" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="180px" />
                                <col width="80px" />
                                <col width="80px" />
                                <col width="80px" />
                                <col width="80px" />
                            </colgroup>
                            <tbody>
                            </tbody>
                                <tr>
                                   <td align="left" class="Body_label">
                                        &nbsp;
                                    </td> 
                                    <td align="left" class="Body_label">
                                         <asp:Label ID="Label7" runat="server" Text="分數合計"></asp:Label>
                                    </td> 
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_MNG_GRADE_TOTAL" runat="server" MaxLength="2" Width="80px" CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td> 
                                    <td align="left" class="Body_label">
                                         <asp:Label ID="Label8" runat="server" Text="考核等第"></asp:Label>
                                    </td> 
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SCORE_DEPT" runat="server"  Width="80px"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                        <asp:Label ID="lb_ORI_SCORE_DEPT" runat="server"  Width="80px" Visible="false"></asp:Label>
                                    </td> 
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
             <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="0" cellpadding="0" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="90%" />
                            </colgroup>
                            <tbody>
                            </tbody>
                                <tr>
                                    <td align="left" class="Body_label"  colspan="2">
                                          <asp:Label ID="Label11" runat="server" Text="【推薦區分】"></asp:Label>
                                       <asp:TextBox ID="txt_RECOMM_DESC" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                    </td> 
                                </tr>
                                <tr>
                                   <td align="left" class="Body_label" colspan="2">
                                          <asp:Label ID="Label9" runat="server" Text="初核總評/推薦事項"></asp:Label>:
                                    </td>  
                                </tr>
                                <tr>
                                   <td align="left" class="Body_label" colspan="2">
                                         <asp:TextBox ID="txt_COMMENT" TextMode="MultiLine" placeholder="輸入字數限制400個字元(含空白、斷行字符)" MaxLength="15" Columns="125" Rows="5" runat="server" BorderWidth="1"  onkeypress="return procEnterText();" onpaste="return true"/>
                                    </td>  
                                </tr>
                                <tr>                                    
                                    <td  align="right" class="Body_label" colspan="2">
                                        <div id="init">
                                            <asp:Button ID="WFB2SJ3600EmpScorePre" runat="server" Text="上一筆" OnClick="WFB2SJ3600EmpScorePre_Click" />
                                            <asp:Button ID="WFB2SJ3600EmpScoreNext" runat="server" Text="下一筆" OnClick="WFB2SJ3600EmpScoreNext_Click"  />
                                            <asp:Button ID="WFB2SJ3600EmpScoreSave" runat="server" Text="儲存" OnClick="WFB2SJ3600EmpScoreSave_Click" />
                                             <asp:Button runat="server" ID="btn_cancel" Text="返回" OnClick="btn_Cancel_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:Button ID="HID_cancel" runat="server" Style="display: none" ClientIDMode="Static" OnClick="HID_cancel_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

