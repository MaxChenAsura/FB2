<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0120_Upd.aspx.cs" Inherits="WebContent_WFB2SJ0120_Upd" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #txt_DISTING_CD {
            text-transform: uppercase;
        }
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
                                      <%-- 區分代碼 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DISTING_CD" runat="server" Text="區分代碼"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_DISTING_CD" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                        <asp:HiddenField ID="hid_DISTING_CD" runat="server" ClientIDMode="Static" /> 
                                         <asp:HiddenField ID="hid_USER_UP_YN" runat="server" ClientIDMode="Static" />                                   
                                    </td>
                                    <%-- 區分說明 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DISTING_DESC" runat="server" Text="區分說明"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_DISTING_DESC" runat="server" MaxLength="15" Width="100px" CssClass="" ClientIDMode="Static"></asp:TextBox>                     
                                    </td>
                                    
                                </tr>
                                
                                 <tr>
                                    <%--外數區分 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_OUT" runat="server" Text="外數區分"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_OUT" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>                                       
                                        <asp:RequiredFieldValidator ID="Req_IS_OUT" runat="server" 
                                            ErrorMessage="外數區分必輸入" InitialValue="-1"
                                            ControlToValidate="ddl_IS_OUT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                     <%-- 備考對象 --%>             
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_REMARK" runat="server" Text="備考對象" ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                        <asp:DropDownList ID="ddl_IS_REMARK" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList> 
                                        <asp:RequiredFieldValidator ID="Req_IS_REMARK" runat="server" 
                                            ErrorMessage="備考對象必輸入" InitialValue="-1"
                                            ControlToValidate="ddl_IS_REMARK" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>  
                                    </td>
                                </tr>
                                 <tr>
                                    <%--是否生效 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_VALID" runat="server" Text="是否生效"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_VALID" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>                                       
                                        <asp:RequiredFieldValidator ID="Rea_IS_VALID" runat="server" 
                                            ErrorMessage="是否生效必輸入" InitialValue="-1"
                                            ControlToValidate="ddl_IS_VALID" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                     <%-- 備考內容 --%>             
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CONTENT" runat="server" Text="備考內容"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_CONTENT" runat="server" MaxLength="15" Width="100px" CssClass="" ClientIDMode="Static"></asp:TextBox>
                                           
                                    </td> 
                                </tr>
                                 <tr>
                                    <!--備註說明-->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="備註"></asp:Label>:
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_REMARK" runat="server" Width="100%" BorderWidth="1" Style="overflow: auto"  ></asp:TextBox>
                                    </td>
                                </tr>
                               <tr>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="3">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SJ0120Save_U" runat="server" Text="儲存"  OnClientClick="return saveCheck();" OnClick="WFB2SJ0120Save_Click"  />
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
