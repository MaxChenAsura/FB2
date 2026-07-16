<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sj/WFB2SJ0110_Add.aspx.cs" Inherits="WebContent_WFB2SJ0110_Add" Culture="auto" UICulture="auto" %>
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
            $(".money").mask('9999999');
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
                                    <%-- 考核類別 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ASSESS_TYPE" runat="server" Text="考核類別"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                       <asp:DropDownList ID="ddl_ASSESS_TYPE" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>                                       
                                        <asp:RequiredFieldValidator ID="Req_ASSESS_TYPE" runat="server" 
                                            ErrorMessage="考核類別必輸入" InitialValue="-1"
                                            ControlToValidate="ddl_ASSESS_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                    <%-- 職種 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_WS_CD" runat="server" Text="職種"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                       
                                       <asp:DropDownList ID="ddl_WS_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>                                       
                                        <asp:RequiredFieldValidator ID="Req_WS_CD" runat="server" 
                                            ErrorMessage="職種必輸入" InitialValue="-1"
                                            ControlToValidate="ddl_WS_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                           
                                    </td> 
                                </tr>
                                 <tr>
                                    <%--資格 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEVEL_CD" runat="server" Text="資格"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_LEVEL_CD" runat="server" ClientIDMode="Static" CssClass=""></asp:DropDownList>                                       
                                       
                                    </td>
                                     <%-- 職務類型 --%>             
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PJOB_TYPE" runat="server" Text="職務類型"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                      <asp:DropDownList ID="ddl_PJOB_TYPE" runat="server" ClientIDMode="Static" CssClass=""></asp:DropDownList>
                                       
                                    </td>
                                </tr>
                                 <tr>
                                    <%--項目群組 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ITEM_GROUP" runat="server" Text="項目群組"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_ITEM_GROUP" runat="server" Width="100px" ClientIDMode="Static" Enabled="false" BorderWidth="1" CssClass="MandatoryField"></asp:TextBox>   
                                        <input id="Button5" type="button" value="..." onclick="OpenSearch('CommCode_Search.aspx', 'txt_ITEM_GROUP', 'txt_ITEM_GROUP_NAME', 'MAIN_CD=CATEGORY');" />
                                        <asp:TextBox ID="txt_ITEM_GROUP_NAME" runat="server" Width="150px" ClientIDMode="Static" BorderWidth="0" Enabled="false" CssClass="MandatoryField"></asp:TextBox>
                                         <asp:RequiredFieldValidator ID="req_ITEM_GROUP" runat="server" 
                                            ErrorMessage="項目群組必輸入" InitialValue=""
                                            ControlToValidate="txt_ITEM_GROUP" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator> 
                                    </td>
                                </tr>
                               <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="3">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SJ0110Save_A" runat="server" Text="儲存"  OnClientClick="return saveCheck();" OnClick="WFB2SJ0110Save_Click"  />
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
