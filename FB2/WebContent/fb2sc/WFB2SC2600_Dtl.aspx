<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2600_Dtl.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2600_Dtl" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
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
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0) {
                if (confirm($('#hidwfb299_Del_ConfirmMessage').val())) {
                    BlockUI();
                    return true;
                }
                else
                    return false;
            }
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
            }            
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1) {
                BlockUI();
                return true;
            }
            else {
                alert($('#hidwfb299_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb299_Dtl_NotChoiceMessage').val());
                return false;
            }
        }
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
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
                                <col width="30%" />
                                <col width="10%" />
                                <col width="50%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_SALARY_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_SALARY_TYPE_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2hc_lb_SALARY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_SALARY_DT_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2sf_PAY_KIND%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_PAY_KIND_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2sc_lb_PROCESS_STATUS%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_PROCESS_STATUS_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2_lb_PAY_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_PAY_ID_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2_lb_PAY_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_PAY_DT_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="Label6" runat="server" Text="批號"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_LNO_txt" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <th>                                      
                                    </th>
                                    <td>                                        
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,btn_back%>" OnClick="btn_back_Click" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                     <td align="right" class="Body_label">
                        <div id="Div1">
                            <aces:Btn ID="WFB2SC2600DELETE" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClientClick="return BlockUI();" OnClick="WFB2SC2600DELETE_Click" Visible="False" />
                             
                            <%--
                            <aces:Btn ID="WFB2SC2600EXECUTE3" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2600EXECUTE3%>" OnClientClick="BlockUI();" OnClick="WFB2SC2600EXECUTE3_Click" Visible="False" />
                            <asp:Button ID="WFB2SC2600DELETE" runat="server" Text="<%$Resources:Resource,btn_delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SC2600DELETE_Click" Visible="False" />
                            <aces:Btn ID="WFB2SC2600EXECUTE3" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC2600EXECUTE3%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SC2600EXECUTE3_Click" Visible="False" /> 
                            --%>
                        </div>

                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDtlData2"
                SelectCountMethod="getDtlCount2" TypeName="CFB2SC2600DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="ods1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_SALARY_TYPE"
                        Name="salary_type" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_SALARY_DT"
                        Name="salary_dt" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_PAY_KIND"
                        Name="pay_kind" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_PAY_ID"
                        Name="pay_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb299_RowNumber%>" HeaderStyle-Width="50px" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_lb_GROUP_ID%>" SortExpression="GROUP_ID" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_GROUP_ID" runat="server" Width="100px" Text='<%#Bind("GROUP_ID")%>' Style="word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_GROUP_NAME%>" SortExpression="GROUP_NAME" HeaderStyle-Width="240px" ItemStyle-Width="240px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_GROUP_NAME" runat="server" Width="240px" Text='<%#Bind("GROUP_NAME")%>' Style="word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sf_DEPT_ACCT_ID%>" SortExpression="DEPT_ACCT_ID" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_DEPT_ACCT_ID" runat="server" Width="120px" Text='<%#Bind("DEPT_ACCT_ID")%>' Style="word-wrap: break-word;"></asp:Label>
                            <asp:HiddenField ID="HID_ACCT_ID" runat="server" ClientIDMode="Static"  Value='<%#Bind("ACCT_ID")%>'  />
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="(暫)傳票單號"  SortExpression="ACCT_ID" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_ACCT_ID" runat="server" Width="150px" Text='<%#Bind("ACCT_ID")%>' Style="word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="HR單號"  SortExpression="SAP_HR_NO" HeaderStyle-Width="150px" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_SAP_HR_NO" runat="server" Width="150px" Text='<%#Bind("SAP_HR_NO")%>' Style="word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="金額"  SortExpression="D_TOTAMT" HeaderStyle-Width="120px" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="RIGHT">
                        <ItemTemplate>
                            <asp:Label ID="lb_D_TOTAMT" runat="server" Width="120px" Text='<%#Bind("D_TOTAMT")%>' Style="word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10000_Rows%>" Value="10000"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hid_PAY_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_SALARY_DT" runat="server" ClientIDMode="static" />
            <asp:HiddenField ID="hid_PAY_KIND" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_SALARY_TYPE" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_IS_VOUCHER" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_IS_SAP" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Mod_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


