<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ3300_Upd.aspx.cs" Inherits="WebContent_WFB2SJ3300_Upd" Culture="auto" UICulture="auto" %>
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
        function countRateTotal() {
            var rA = 0;
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
                                      <%-- 考核類型 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text="考核類型"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_ASSESS_TYPE_DESC" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                        <asp:HiddenField ID="hid_ASSESS_TYPE" runat="server" ClientIDMode="Static" />                                  
                                    </td>
                                   <%-- 合計 --%>             
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_RATE_TOTAL" runat="server" Text="合計" ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                          <asp:TextBox ID="txt_RATE_TOTAL" runat="server"  CssClass="txtDisabled"  Enabled="false"  BorderWidth="0" Width="100px" ClientIDMode="Static" ></asp:TextBox> 
                                    </td>
                                </tr>
                                 <tr>
                                    <%--是否控管人數--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="是否控管人數"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_CTL" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>   
                                    </td>
                                         
                                    <th align="left">
                                    </th>
                                    <td align="left">  
                                    </td>
                                </tr>
                                  <tr>
                                    <%--A(%)--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_RATE_A" runat="server" Text="A(%)"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_RATE_A" runat="server" MaxLength="15" Width="100px" ClientIDMode="Static" onblur="countRateTotal();" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>                                        
                                        <asp:CustomValidator ID="cv_RATE_A" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="A(%)僅能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_RATE_A" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>                                               
                                    <th align="left" class="">
                                    <td align="left" class="Body_label" >                                           
                                    </td> 
                                </tr>
                                 <tr>
                                    <%--B(%)--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_RATE_B" runat="server" Text="B(%)"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_RATE_B" runat="server" MaxLength="15" Width="100px" ClientIDMode="Static" onblur="countRateTotal();" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>                                                                               
                                        <asp:CustomValidator ID="cv_RATE_B" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="B(%)僅能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_RATE_B" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>          
                                    <th align="left" class="">
                                    <td align="left" class="Body_label" >                                           
                                    </td> 
                                </tr>
                                 <tr>
                                    <%--C(%)--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_RATE_C" runat="server" Text="C(%)"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_RATE_C" runat="server" MaxLength="15" Width="100px" ClientIDMode="Static" onblur="countRateTotal();" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>                                                                               
                                        <asp:CustomValidator ID="cv_RATE_C" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="B(%)僅能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_RATE_C" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>            
                                    <th align="left" class="">
                                    <td align="left" class="Body_label" >                                           
                                    </td> 
                                </tr>
                                 <tr>
                                    <%--D(%)--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_RATE_D" runat="server" Text="D(%)"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_RATE_D" runat="server" MaxLength="15" Width="100px" ClientIDMode="Static" onblur="countRateTotal();" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>                                                                               
                                        <asp:CustomValidator ID="cv_RATE_D" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="D(%)僅能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_RATE_D" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>             
                                    <th align="left" class="">
                                    <td align="left" class="Body_label" >                                           
                                    </td> 
                                </tr>
                                 <tr>
                                    <%--E(%)--%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_RATE_E" runat="server" Text="E(%)"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_RATE_E" runat="server" MaxLength="15" Width="100px" ClientIDMode="Static" onblur="countRateTotal();" onkeypress="return IsIntText();" onpaste="return false"></asp:TextBox>                                                                             
                                        <asp:CustomValidator ID="cv_RATE_E" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="D(%)僅能輸入數字" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                            ControlToValidate="txt_RATE_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                    </td>             
                                    <th align="left" class="">
                                    <td align="left" class="Body_label" >                                           
                                    </td> 
                                </tr>
                               <tr>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="3">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SJ3300Save_U" runat="server" Text="儲存"  OnClientClick="return saveCheck();" OnClick="WFB2SJ3300Save_Click"  />
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
