<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sa/action/WFB2SA1600_Upd.aspx.cs" Inherits="WebContent_WFB2SA1600_Upd" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<%@ Register Src="~/UserControl/UCDateTextBoxRange.ascx.ascx" TagPrefix="uc1" TagName="UCDateTextBoxRangeascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        #txt_PJOB_CD {
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
                                      <%-- 職務代號 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PJOB_CD" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_PJOB_CD%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label" >
                                        <asp:TextBox ID="txt_PJOB_CD" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>
                                        <asp:HiddenField ID="hid_PJOB_CD" runat="server" ClientIDMode="Static" />                                  
                                    </td>
                                    <%-- 薪資項目 --%>    
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ID" runat="server" Text="<%$Resources:Resource,sa160_SALARY_ID%>"  ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                         <asp:TextBox ID="txt_SALARY_ID" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0" ></asp:TextBox>     
                                         <asp:HiddenField ID="hid_SALARY_ID" runat="server" ClientIDMode="Static" />                                  
                                    </td>
                                     <%-- 類別 --%>             
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_HIRE_TYPE" runat="server" Text="<%$Resources:Resource,sa160_HIRE_TYPE%>" ></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">  
                                        <asp:TextBox ID="txt_HIRE_TYPE" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>   
                                         <asp:HiddenField ID="hid_HIRE_TYPE" runat="server" ClientIDMode="Static" />  
                                    </td>
                                </tr>
                                 <tr>
                                     <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_PAY" runat="server" Text="金額"></asp:Label>:
                                    </th>
                                    <td align="left"  class="Body_label">
                                        <asp:TextBox ID="txt_PAY" runat="server" style="text-align:right" MaxLength="7" Width="60px" ClientIDMode="Static" CssClass="MandatoryField money"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="req_CHG_AMT_A" runat="server" 
                                            ErrorMessage="<%$Resources:Resource,wfb2sa_require_CHG_AMT_A%>"
                                            ControlToValidate="txt_PAY" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="rv_CHG_AMT_A" ControlToValidate="txt_PAY" runat="server" ValidationGroup="GroupA" ForeColor="Red" Display="None"
                                            ErrorMessage="<%$Resources:Resource,wfb2sa_range_CHG_AMT_A%>" MinimumValue="0" MaximumValue="9999999"></asp:RangeValidator>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="生效起日"></asp:Label>:
                                    </th>
                                    <td align="left"   class="Body_label">
                                        <asp:TextBox ID="txt_START_DT" runat="server" CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                                    </td>
                                      <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_END_DT" runat="server" Text="生效迄日"></asp:Label>:
                                    </th>
                                    <td align="left"  class="Body_label">
                                        <asp:TextBox ID="txt_END_DT" runat="server" ClientIDMode="Static" Width="80px" CssClass="MandatoryField date"></asp:TextBox>
                                       <!--必輸入-->
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                            ErrorMessage="生效迄日必輸入"
                                            ControlToValidate="txt_END_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                         <!--驗證日期格式-->
                                          <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="生效迄日格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_END_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <!--起日不可大於迄日 -->	
							        	<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_START_DT"
                                       ControlToValidate="txt_END_DT" ErrorMessage="生效起日不可大於迄日" Type="Date" Operator="GreaterThanEqual"
                                       Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>


                                </tr>
                                 <tr>
                                    <!--備註說明-->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="備註"></asp:Label>:
                                    </th>
                                    <td colspan="5">
                                        <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_REMARK" runat="server" Width="100%" BorderWidth="1" Style="overflow: auto"  ></asp:TextBox>
                                    </td>
                                </tr>
                               <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="3">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SA1600Save_U" runat="server" Text="儲存"  OnClientClick="return saveCheck();" OnClick="WFB2SA1600Save_Click"  />
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
