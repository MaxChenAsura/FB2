<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2df/action/WFB2DF0100_Qry.aspx.cs" Inherits="WebContent_fb2df_WFB2DF0100_Qry" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $("#txt_EDIT_AMOUNT").mask('99999');
            $("#txt_NEW_AMOUNT").mask('99999');
            $(".number").blur(function () {
                $(this).parseNumber({ format: "#,###", locale: "tw" });
                $(this).formatNumber({ format: "#,###", locale: "tw" });
            });
            gridviewScroll();
            $.unblockUI();
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"

            });

        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb299_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;
            if (Page_ClientValidate("GroupA")) {

                processed = confirm("修改後將會把目前使用此設定的住宿人員全部更新，是否確定要繼續？");
                if (processed)
                    BlockUI();
            }
            else {
                return;

            }

            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="15%" />
                    <col width="75%" />

                </colgroup>
                <tbody>
                    <!-- START: 1st Line in Search Criteria area -->
                    <tr>
                        <th></th>
                        <td align="right" class="Body_label">
                            <div id="init_grid">
                                <aces:Btn ID="WFB2DF0100Add" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0100Add%>" OnClick="WFB2DF0100Add_Click" />
                                <aces:Btn ID="WFB2DF0100Delete" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0100Delete%>"  OnClick="WFB2DF0100Delete_Click" Visible="False" OnClientClick="return CheckDelAction();"/>
                                <aces:Btn ID="WFB2DF0100Edit" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0100Edit%>" OnClick="WFB2DF0100Edit_Click" Visible="False" />
                                <aces:Btn ID="WFB2DF0100Save" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0100Save%>" Visible="false" OnClick="WFB2DF0100Save_Click" OnClientClick="return saveCheck();" />

                                <%--<asp:Button ID="WFB2DF0100Add" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0100Add%>" OnClick="WFB2DF0100Add_Click" />
                                <asp:Button ID="WFB2DF0100Delete" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0100Delete%>" OnClick="WFB2DF0100Delete_Click" Visible="False" OnClientClick="return CheckDelAction();" />
                                <asp:Button ID="WFB2DF0100Edit" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0100Edit%>" OnClick="WFB2DF0100Edit_Click" Visible="False" />
                                <asp:Button ID="WFB2DF0100Save" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0100Save%>" Visible="false" OnClick="WFB2DF0100Save_Click" OnClientClick="return saveCheck();" />--%>
                                <asp:Button ID="WFB2DF0100Cancel" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0100Cancel%>" Visible="false" OnClick="WFB2DF0100Cancel_Click" OnClientClick="return confirm('是否確定取消?');" />
                            </div>

                        </td>
                    </tr>



                    <!-- end: Create MODULE ID -->
                    <!-- START: Create a line to separate Search field with body field -->
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr>
                        </td>
                    </tr>
                    <!-- END: Create a line -->
                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DF0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1019px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="40px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2df_RowNumber%>" HeaderStyle-Width="140px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2df_lb_BASE_NO%>" SortExpression="BASE_NO" HeaderStyle-Width="140px">
                        <ItemTemplate>
                            <asp:Label ID="lb_BASE_NO" runat="server" Text='<%#Bind("BASE_NO")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_EDIT_BASE_NO" runat="server" Text='<%#Bind("BASE_NO")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_BASE_NO" runat="server" MaxLength="1" Width="14px" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_BASE_NO%>"
                                ControlToValidate="txt_NEW_BASE_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2df_ERR_BASE_NO_Chinese%>" ControlToValidate="txt_NEW_BASE_NO" ForeColor="Red"
                                ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>

                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2df_lb_BASE_NAME%>" SortExpression="BASE_NAME" HeaderStyle-Width="400px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_BASE_NAME" runat="server" Text='<%#Bind("BASE_NAME")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_BASE_NAME" runat="server" MaxLength="30" Width="350px" Text='<%#Bind("BASE_NAME")%>' CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_BASE_NAME%>"
                                ControlToValidate="txt_EDIT_BASE_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_BASE_NAME" runat="server" MaxLength="15" Width="350px" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_BASE_NAME%>"
                                ControlToValidate="txt_NEW_BASE_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2df_lb_AMOUNT%>" SortExpression="AMOUNT" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lb_AMOUNT" runat="server" Text='<%#Bind("AMOUNT","{0:N0}")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_EDIT_AMOUNT" runat="server" MaxLength="5" Width="250px" Style="ime-mode: disabled" CssClass="MandatoryField" Text='<%#Bind("AMOUNT")%>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_AMOUNT%>"
                                ControlToValidate="txt_EDIT_AMOUNT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>

                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_AMOUNT" runat="server" MaxLength="5" Width="250px" CssClass="MandatoryField" Style="ime-mode: disabled"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_AMOUNT%>"
                                ControlToValidate="txt_NEW_AMOUNT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>

                </Columns>
                <EmptyDataTemplate>

                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td></td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2df_RowNumber%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2df_lb_BASE_NO%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2df_lb_BASE_NAME%>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2df_lb_AMOUNT%>"></asp:Label>
                            </td>

                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <asp:TextBox ID="txt_NEW_BASE_NO" runat="server" MaxLength="1" Width="14px" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_BASE_NO%>"
                                    ControlToValidate="txt_NEW_BASE_NO" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2df_ERR_BASE_NO_Chinese%>" ControlToValidate="txt_NEW_BASE_NO" ForeColor="Red"
                                    ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                                </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_BASE_NAME" runat="server" MaxLength="15" Width="350px" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_BASE_NAME%>"
                                    ControlToValidate="txt_NEW_BASE_NAME" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_AMOUNT" runat="server" MaxLength="5" Width="250px" Style="ime-mode: disabled" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2df_AMOUNT%>"
                                    ControlToValidate="txt_NEW_AMOUNT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>

                        </tr>
                    </table>

                </EmptyDataTemplate>
                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">

                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                            <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                            <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                            <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                            <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 5px"></td>
                    <td style="font-size: 14px;">
                        <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
