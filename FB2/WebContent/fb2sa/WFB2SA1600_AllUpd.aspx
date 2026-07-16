<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sa/action/WFB2SA1600_AllUpd.aspx.cs" Inherits="WebContent_WFB2SA1600_AllUpd" Culture="auto" UICulture="auto" %>
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
                                <col width="17%" />
                                <col width="17%" />
                                <col width="17%" />
                                <col width="17%" />
                                <col width="17%" />
                                <col width="15%" />
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
                                        <asp:TextBox ID="txt_PAY" runat="server"  CssClass="txtDisabled" Enabled="false" BorderWidth="0"></asp:TextBox>
                                    </td>
                                    <!--一括生效日期-->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_All_START_DT" runat="server" Text="一括生效日期"></asp:Label>:
                                    </th>
                                    <td colspan="5">
                                        <asp:TextBox ID="txt_All_START_DT" runat="server" ClientIDMode="Static" Width="80px"  CssClass="MandatoryField date"></asp:TextBox>
                                          <!--必輸入-->
                                        <asp:RequiredFieldValidator ID="req_START_DT" runat="server" 
                                            ErrorMessage="一括生效日期必輸入"
                                            ControlToValidate="txt_All_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                         <!--驗證日期格式-->
                                          <asp:CustomValidator ID="chk_START_DT" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="一括生效日期格式錯誤" ClientValidationFunction="CheckDate" ForeColor="Red"
                                        ControlToValidate="txt_All_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>

                                    </td>
                                     <th></th>
                                     <td></td>
                                </tr>
                                 <tr>
                                    <!--備註說明-->
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_REMARK" runat="server" Text="簽核備註"></asp:Label>:
                                    </th>
                                    <td colspan="5">
                                        <asp:TextBox TextMode="MultiLine" Rows="5" ID="txt_REMARK" runat="server" Width="100%" BorderWidth="1" Style="overflow: auto"  CssClass="MandatoryField" ></asp:TextBox>
                                           <!--必輸入-->
                                        <asp:RequiredFieldValidator ID="Req_PJOB_CD" runat="server" 
                                            ErrorMessage="備註必輸入"
                                            ControlToValidate="txt_REMARK" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                               <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label" colspan="3">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SA1600AllSave" runat="server" Text="提出核可"  OnClientClick="return saveCheck();" OnClick="WFB2SA1600AllSave_Click"  />
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
           <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getAllUpdData"
                SelectCountMethod="getAllUpdCount" TypeName="CFB2SA1600DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_GEN_EMP_ID"
                        Name="emp_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_PJOB_CD"
                        Name="pjob_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_HIRE_TYPE"
                        Name="hire_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_SALARY_ID"
                        Name="salary_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <%--序號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sh_lb_rownumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <%--工號--%>
                    <asp:TemplateField HeaderText="工號" SortExpression="EMP_ID" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="姓名" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>' Width="80px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--入社日--%>
                    <asp:TemplateField HeaderText="入社日" SortExpression="JOIN_DT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_JOIN_DT" runat="server" Text='<%#Bind("JOIN_DT","{0:yyyy/MM/dd}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <%--原敘薪金額--%>
                    <asp:TemplateField HeaderText="原敘薪金額"  HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Right" >
                        <ItemTemplate>
                            <asp:Label ID="lb_CHG_AMT_B" runat="server" Text='<%#Bind("CHG_AMT_B","{0:##,#}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>   
                      <%--原生效起日--%>
                    <asp:TemplateField HeaderText="原生效起日" SortExpression="EFFECT_SDT_B" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_EFFECT_SDT_B" runat="server" Text='<%#Bind("EFFECT_SDT_B","{0:yyyy/MM/dd}")%>' Width="120px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <%--原生效迄日--%>
                    <asp:TemplateField HeaderText="原生效迄日" SortExpression="EFFECT_EDT_B" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lb_EFFECT_EDT_B" runat="server" Text='<%#Bind("EFFECT_EDT_B","{0:yyyy/MM/dd}")%>' Width="120px"></asp:Label>
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
                            <asp:ListItem Text="每頁10000筆" Value="10000"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
             <asp:HiddenField ID="hid_GEN_EMP_ID" runat="server" ClientIDMode="Static" />
             <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
             <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
