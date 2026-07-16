<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0280_Upd.aspx.cs" Inherits="WebContent_WFB2SJ0280_Upd" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
      
    </style>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".year").mask('9999');
            $(".rate").mask('9999');
            $(".txtDisabled").css("background-color", "white").css("color", "black").css("border-width", "0");
            $.unblockUI();
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
        //判斷是否為數字
        function IsIntText() {
            var charkeycode = window.event.keyCode;
            if (charkeycode > 47 && charkeycode < 58) {  //0的keycode為47 ,9的keycode為58
                return true;
            }
            return false;

        }
        function countBaseTotal() {
            var bA = 0;
            var bB = 0;
            var bC = 0;
            var bD = 0;
            var bE = 0;

            if ($("#txt_BASE_A").val() != "") bA = $("#txt_BASE_A").val();
            if ($("#txt_BASE_B").val() != "") bB = $("#txt_BASE_B").val();
            if ($("#txt_BASE_C").val() != "") bC = $("#txt_BASE_C").val();
            if ($("#txt_BASE_D").val() != "") bD = $("#txt_BASE_D").val();
            if ($("#txt_BASE_E").val() != "") bE = $("#txt_BASE_E").val();
            $("#txt_COUNT_BASE_TOT").val(parseInt(bA) + parseInt(bB) + parseInt(bC) + parseInt(bD) + parseInt(bE));
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--頁面table--%>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="18%" />
                                <col width="15%" />
                                <col width="18%" />
                                <col width="10%" />
                                <col width="10%" />
                                <col width="10%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%-- 考核年度 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_ASSESS_YEAR" runat="server" Text="考核年度"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_ASSESS_YEAR" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                        <asp:HiddenField ID="hid_ASSESS_YEAR" runat="server" ClientIDMode="Static" />                                  
                                    </td>
                                      <%-- 考核類別 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text="考核類別"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_ASSESS_TYPE_DESC" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                        <asp:HiddenField ID="hid_ASSESS_TYPE" runat="server" ClientIDMode="Static" /> 
                                        <asp:HiddenField ID="hid_DEPT_NO_20" runat="server" ClientIDMode="Static" />                                  
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <%-- 工號 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MA_EMP_ID" runat="server" Text="工號"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_MA_EMP_ID" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>   
                                        <asp:HiddenField ID="hid_MA_EMP_ID" runat="server" ClientIDMode="Static" />  
                                         <asp:HiddenField ID="hid_MA_TYPE" runat="server" ClientIDMode="Static" />                                                      
                                    </td>
                                    <%-- 資格群組 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lbl_GRP_CD" runat="server" Text="資格群組"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_GRP_CD" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>   
                                        <asp:HiddenField ID="hid_GRP_CD" runat="server" ClientIDMode="Static" />                                                    
                                    </td>
                                    
                                </tr>
                                 
                                 <tr>
                                    <%--總人數 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BASE_TOT" runat="server" Text="總人數"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" >
                                       <asp:TextBox ID="txt_BASE_TOT" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                                    </td>
                                     <%--A~E合計 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="A~E合計"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" >
                                       <asp:TextBox ID="txt_COUNT_BASE_TOT" runat="server"  CssClass="txtDisabled"  Enabled="false"  BorderWidth="0" Width="100px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--A--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BASE_A" runat="server" Text="A"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_BASE_A" runat="server" MaxLength="15" Width="100px" ClientIDMode="Static" onblur="countBaseTotal();"  CssClass="rate"></asp:TextBox>                                        
                                        <asp:RequiredFieldValidator ID="Req_BASE_A" runat="server" 
                                            ErrorMessage="不可為空白"
                                            ControlToValidate="txt_BASE_A" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>                                        
                                        <asp:CustomValidator ID="cv_BASE_A" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="A僅能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_BASE_A" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>                                               
                                    <th align="left" class="">
                                    <td align="left" class="Body_label" >                                           
                                    </td> 
                                </tr>
                                <tr>
                                    <%--B--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BASE_B" runat="server" Text="B"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_BASE_B" runat="server" MaxLength="15" Width="100px" ClientIDMode="Static" onblur="countBaseTotal();"  CssClass="rate"></asp:TextBox>                                        
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                            ErrorMessage="不可為空白"
                                            ControlToValidate="txt_BASE_B" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>                                        
                                        <asp:CustomValidator ID="cv_BASE_B" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="B僅能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_BASE_B" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>                                               
                                    <th align="left" class="">
                                    <td align="left" class="Body_label" >                                           
                                    </td> 
                                </tr>
                                <tr>
                                    <%--C--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BASE_C" runat="server" Text="C"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_BASE_C" runat="server" MaxLength="15" Width="100px" ClientIDMode="Static" onblur="countBaseTotal();"  CssClass="rate"></asp:TextBox>                                        
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                            ErrorMessage="不可為空白"
                                            ControlToValidate="txt_BASE_C" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>                                        
                                        <asp:CustomValidator ID="cv_BASE_C" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="C僅能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_BASE_C" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>                                               
                                    <th align="left" class="">
                                    <td align="left" class="Body_label" >                                           
                                    </td> 
                                </tr>
                                <tr>
                                    <%--D--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BASE_D" runat="server" Text="D"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_BASE_D" runat="server" MaxLength="15" Width="100px" ClientIDMode="Static" onblur="countBaseTotal();"  CssClass="rate"></asp:TextBox>                                        
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                            ErrorMessage="不可為空白"
                                            ControlToValidate="txt_BASE_D" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>                                        
                                        <asp:CustomValidator ID="cv_BASE_D" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="D僅能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_BASE_D" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>                                               
                                    <th align="left" class="">
                                    <td align="left" class="Body_label" >                                           
                                    </td> 
                                </tr>
                                <tr>
                                    <%--E--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BASE_E" runat="server" Text="E"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_BASE_E" runat="server" MaxLength="15" Width="100px" ClientIDMode="Static" onblur="countBaseTotal();"  CssClass="rate"></asp:TextBox>                                        
                                        <asp:RequiredFieldValidator ID="Req_BASE_E" runat="server" 
                                            ErrorMessage="不可為空白"
                                            ControlToValidate="txt_BASE_E" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>                                        
                                        <asp:CustomValidator ID="cv_BASE_E" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="E僅能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_BASE_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>                                               
                                    <th align="left" class="">
                                    <td align="left" class="Body_label" >                                           
                                    </td> 
                                </tr>
                               <tr>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="3">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SJ0280Save_U" runat="server" Text="儲存"  OnClientClick="return saveCheck();" OnClick="WFB2SJ0280Save_Click"  />
                                            <asp:Button runat="server" ID="btn_cancel" Text="取消" OnClick="btn_Cancel_Click" OnClientClick="return confirm('是否確定取消?');"/>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Body_label">
                         <div id="init_grid">
                        </div>
                    </td>
                </tr>
            </table>
             <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
