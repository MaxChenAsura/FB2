<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0540_Qry.aspx.cs" Inherits="WebContent_WFB2SJ0540_Qry" Culture="auto" UICulture="auto" %>

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
            //gridviewScroll();
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

        

        

        //清空畫面
        function ClearAll() {
            
            //$("#ddl_DEPT_NO_20").val("-1");
            $("#ddl_WS_CD").val("-1");
            $("#ddl_GRP_CD").val("-1");
            $("#ddl_RECOMM_DESC").val("-1");
            $("#ddl_SCORE_FINAL").val("-1");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
        }
        //送出簽核確認
        function SignConfirm() {
            var confirmMsg = "是否完成所有群組複核, 確定提出嗎?";
            if (confirm(confirmMsg)) {
                /**
                if (document.getElementById("hid_EMP_SUGGEST_COUNT").value == "0") {
                    if (confirm("本部門無要望申請書,是否繼續 ?")) {
                        return true;
                    } else {
                        return false;
                    }
                }**/
            } else {
                return false;
            }


        }
        function doViewComments(emp_id) {
            
           
            OpenSearch("Assess_Comments_Rec.aspx", "", "", "ASSESS_YEAR=" + document.getElementById("hid_ASSESS_YEAR").value + "&ASSESS_TYPE=" + document.getElementById("hid_ASSESS_TYPE").value + "&EMP_ID=" + emp_id);
        }
        function doViewFixRec(emp_id) {
          
            OpenSearch("Assess_Log_View.aspx", "", "", "ASSESS_YEAR=" + document.getElementById("hid_ASSESS_YEAR").value + "&ASSESS_TYPE=" + document.getElementById("hid_ASSESS_TYPE").value + "&EMP_ID=" + emp_id);
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
                            <asp:TextBox ID="txt_ASSESS_YEAR" runat="server" Width="60px" ClientIDMode="Static" maxlength="4" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2sj_format_assess_year_s%>" ControlToValidate="txt_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                    ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"    ErrorMessage="考核年度必輸入" InitialValue=""
                                 ControlToValidate="txt_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ErrorMessage="年度輸入格式錯誤" ControlToValidate="txt_ASSESS_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                     ValidationExpression=".{4,4}" Display="None"></asp:RegularExpressionValidator>
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
                         <th></th>
                         <th>                             
                            <asp:HiddenField ID="hid_DEPT_NO_20" runat="server" ClientIDMode="Static" /> 
                            <asp:HiddenField ID="hid_DEPT_NO" runat="server" ClientIDMode="Static" />    
                            <asp:HiddenField ID="hid_DEPT_LEVEL" runat="server" ClientIDMode="Static" />  
                            <asp:HiddenField ID="hid_DEPT_NAME" runat="server" ClientIDMode="Static" /> 
                            <asp:HiddenField ID="hid_EMP_SUGGEST_COUNT" runat="server" ClientIDMode="Static" />  
                            <asp:HiddenField ID="hid_SIGN_YN" runat="server" ClientIDMode="Static" />    
                            <asp:HiddenField ID="hid_MA_EMP_ID" runat="server" ClientIDMode="Static" />     
                            <asp:HiddenField ID="hid_MA_EMP_NAME" runat="server" ClientIDMode="Static" />    
                            <asp:HiddenField ID="hid_MA_TYPE" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hid_SUB_SIGN_YN" runat="server" ClientIDMode="Static" />                 
                         </th>                        
                    </tr> 
                     <tr>  
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_DEPT_NO_20" runat="server" Text="部門"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                             <asp:DropDownList ID="ddl_DEPT_NO_20" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"  > </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="req_DEPT_NO_20" runat="server" 
                                            ErrorMessage="部門必需輸入" InitialValue="-1"
                                            ControlToValidate="ddl_DEPT_NO_20" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </td>                      
                          <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lbl_EMP_ID" runat="server" Text="協理工號"></asp:Label>:
                        </th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"  CssClass="MandatoryField" ></asp:TextBox>
                            <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" Width="90px" ClientIDMode="Static" BorderWidth="0" ></asp:TextBox> 
                        </td>                   
                         <th align="left">
                        </th>
                        <td align="left" class="Body_label"></td>                        
                    </tr> 
                      
                              
                    <tr>
                       
                        <th></th>
                        <th></th>
                        <td colspan="4" align="right" class="Body_label">
                            <div id="init">
                                <aces:Btn ID="WFB2SJ0540Statistics51" runat="server" Text="部門考核統計表" OnClick="WFB2SJ0540Statistics51_Click" OnClientClick="" />
                                <aces:Btn ID="WFB2SJ0540Statistics52" runat="server" Text="協理考核統計表" OnClick="WFB2SJ0540Statistics52_Click" OnClientClick="" />
                                 
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

           
            <!-- 進行新增或修改的檢核 -->
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />

        </ContentTemplate>
        
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SJ0540Statistics51"></asp:PostBackTrigger>
        </Triggers>
         <Triggers>
            <asp:PostBackTrigger ControlID="WFB2SJ0540Statistics52"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
