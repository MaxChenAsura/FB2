<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ss/WFB2SS0300_Qry.aspx.cs" Inherits="WebContent_WFB2SS0300_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".year").mask('9999');
            $(".number").mask('999');            
            $.unblockUI();
        }
        //儲存前檢查
        function CheckValid() {
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
     
    

        //清空畫面
        function ClearAll() {
            $("#txt_DATA_YM_S").val("");
            $("#txt_DATA_YM_E").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_tree_DEPT_NO").val("");
            $("#txt_tree_DEPT_NAME").val("");
            $("#ddl_WS_CD").val("");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="30%" />
                                <col width="10%" />
                                <col width="10%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <!--入社日期-->
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="入社日期"></asp:Label>:
                                    </th>
                                    <td align="left"   class="Body_label">
                                        <asp:TextBox ID="txt_JOIN_SDT" runat="server" ClientIDMode="Static" Width="80px"  CssClass="MandatoryField date"></asp:TextBox>
                                        ~
                                        <asp:TextBox ID="txt_JOIN_EDT" runat="server" ClientIDMode="Static" Width="80px"  CssClass="MandatoryField date"></asp:TextBox>
                                          <!--必輸入-->
                                        <asp:RequiredFieldValidator ID="req_JOIN_SDT" runat="server" 
                                            ErrorMessage="入社起日必輸入"
                                            ControlToValidate="txt_JOIN_SDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                          <asp:RequiredFieldValidator ID="req_JOIN_EDT" runat="server" 
                                            ErrorMessage="入社迄日必輸入"
                                            ControlToValidate="txt_JOIN_EDT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                         <!--驗證日期格式-->
                                          <asp:CustomValidator ID="chk_JOIN_SDT" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="入社起日格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_JOIN_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="chk_JOIN_EDT" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="入社迄日格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_JOIN_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <!--起日不可大於迄日 -->	
							        	<asp:CompareValidator ID="Compare_JOIN_DT" runat="server" ControlToCompare="txt_JOIN_SDT"
                                       ControlToValidate="txt_JOIN_EDT" ErrorMessage="入社起日不可大於迄日" Type="Date" Operator="GreaterThanEqual"
                                       Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                     <!--轉正社員日-->
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BE_EMP_DT" runat="server" Text="轉正社員日"></asp:Label>:
                                    </th>
                                    <td align="left"   class="Body_label">
                                        <asp:TextBox ID="txt_BE_EMP_SDT" runat="server" ClientIDMode="Static" Width="80px"  CssClass="date"></asp:TextBox>
                                        ~
                                        <asp:TextBox ID="txt_BE_EMP_EDT" runat="server" ClientIDMode="Static" Width="80px"  CssClass="date"></asp:TextBox>
                                         <!--驗證日期格式-->
                                          <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="轉正社員起日格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_BE_EMP_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="轉正社員迄日格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_BE_EMP_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <!--起日不可大於迄日 -->	
							        	<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_BE_EMP_SDT"
                                       ControlToValidate="txt_BE_EMP_EDT" ErrorMessage="轉正社員起日不可大於迄日" Type="Date" Operator="GreaterThanEqual"
                                       Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>   
                                     <%-- 是否轉正社員 --%>    
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_IS_BE_EMP" runat="server" Text="是否轉正社員"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label"> 
                                        <asp:DropDownList ID="ddl_IS_BE_EMP" runat="server" ClientIDMode="Static"></asp:DropDownList>                           
                                    </td>      
                                </tr>
                                    <!--離社日期-->
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_DT" runat="server" Text="離社日期"></asp:Label>:
                                    </th>
                                    <td align="left"   class="Body_label">
                                        <asp:TextBox ID="txt_LEAVE_SDT" runat="server" ClientIDMode="Static" Width="80px"  CssClass="date"></asp:TextBox>
                                        ~
                                        <asp:TextBox ID="txt_LEAVE_EDT" runat="server" ClientIDMode="Static" Width="80px"  CssClass="date"></asp:TextBox>
                                         <!--驗證日期格式-->
                                          <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="離社起日格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_LEAVE_SDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="離社迄日格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_LEAVE_EDT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <!--起日不可大於迄日 -->	
							        	<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txt_LEAVE_SDT"
                                       ControlToValidate="txt_LEAVE_EDT" ErrorMessage="離社起日不可大於迄日" Type="Date" Operator="GreaterThanEqual"
                                       Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>   
                                     <%-- 是否-離社 --%>    
                                    <th align="left" class="Body_TableHeader">
                                    <asp:Label ID="lb_IS_LEAVE" runat="server" Text="是否-離社"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label"> 
                                        <asp:DropDownList ID="ddl_IS_LEAVE" runat="server" ClientIDMode="Static"></asp:DropDownList>                           
                                    </td>   
                                <tr>
                                    <td align="right" class="Body_label" colspan="6">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SS0300EXCEL" runat="server" Text="匯出EXCEL"   OnClientClick="CheckValid();" OnClick="WFB2SS0300EXCEL_Click" />
                                            <%--<asp:Button ID="WFB2DL0400Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0700Search%>" ValidationGroup="GroupA" OnClientClick="CheckValid();" OnClick="WFB2DL0400Search_Click" />--%>
                                            
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dh_btn_clear%>" onclick="ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td align="right" class="Body_label">
                        <hr>
                    </td>
                </tr>

            </table>

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
         <Triggers>
                <%--匯出EXCEL按鈕必寫--%>
            <asp:PostBackTrigger ControlID="WFB2SS0300EXCEL" />  
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

