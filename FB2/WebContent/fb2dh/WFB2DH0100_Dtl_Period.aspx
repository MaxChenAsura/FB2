<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0100_Dtl_Period.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0100_Dtl_Period" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".number").mask('99.9');
            $(".number").css("text-align", "right");
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

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm("確定要刪除?");
            else {
                alert("請選取資料!");
                return false;
            }
        }

        function openQry() {
            window.location.href("WFB2DH0100_Dtl.aspx");
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_MAIN_LEAVE_CD" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SUB_LEAVE_CD" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SUB_LEAVE_CD" runat="server" BorderWidth="0" ReadOnly="true" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>

                <tr>
                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <aces:Btn ID="WFB2DH0103Add" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0103Add%>" OnClick="WFB2DH0103Add_Click" />
                            <aces:Btn ID="WFB2DH0103Delete" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0103Delete%>" Visible="false" OnClientClick="return CheckDelAction();" OnClick="WFB2DH0103Delete_Click" />
                            <aces:Btn ID="WFB2DH0103Edit" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0103Edit%>" Visible="false" OnClick="WFB2DH0103Edit_Click" />

                            <%-- <asp:Button ID="WFB2DH0103Add" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0103Add%>" OnClick="WFB2DH0103Add_Click" />
                            <asp:Button ID="WFB2DH0103Delete" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0103Delete%>" Visible="false" OnClientClick="return CheckDelAction();" OnClick="WFB2DH0103Delete_Click" />
                            <asp:Button ID="WFB2DH0103Edit" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0103Edit%>" Visible="false" OnClick="WFB2DH0103Edit_Click" />--%>

                            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb2dh_btn_back%>" OnClick="btn_back_Click" />

                            <aces:Btn ID="WFB2DH0103Save" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0103Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DH0103Save_Click" />

                            <%--<asp:Button ID="WFB2DH0103Save" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0103Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DH0103Save_Click" />--%>

                            <asp:Button ID="btn_Cancel" runat="server" Text="<%$Resources:Resource,wfb2dh_btn_Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_Cancel_Click" />
                        </div>
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData4"
                SelectCountMethod="getCount4" TypeName="CFB2DH0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_MAIN_LEAVE_CD"
                        Name="main_leave_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_SUB_LEAVE_CD"
                        Name="sub_leave_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" Width="1020px"
                OnRowCreated="gv_result_RowCreated" OnDataBound="gv_result_DataBound"
                OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_RowNumber%>" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_START_SIGN%>" SortExpression="START_SIGN" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_SIGN" runat="server" Text='<%#Bind("START_SIGN")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_START_SIGN" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hid_START_SIGN" runat="server" Value='<%#Bind("START_SIGN")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_START_SIGN" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_START_TIME_CD%>" SortExpression="START_TIME_CD" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_TIME_CD" runat="server" Text='<%#Bind("START_TIME_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_START_TIME_CD" runat="server" Text='<%#Bind("START_TIME_CD")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_START_TIME_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_START_TIME_CD%>"
                                ControlToValidate="ddl_NEW_START_TIME_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_START_SIGN_AD%>" SortExpression="START_SIGN_AD" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_SIGN_AD" runat="server" Text='<%#Bind("START_SIGN_AD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_START_SIGN_AD" runat="server" Text='<%#Bind("START_SIGN_AD")%>' ClientIDMode="Static" MaxLength="1" Width="120px"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_START_SIGN_AD" runat="server" ClientIDMode="Static" MaxLength="1" Width="120px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_START_HOURS%>" SortExpression="START_HOURS" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_START_HOURS" runat="server" Text='<%#Bind("START_HOURS")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_START_HOURS" runat="server" Text='<%#Bind("START_HOURS")%>' CssClass="number" Width="40px"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorSTART_HOURS" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_HOURS_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_START_HOURS" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_START_HOURS" runat="server" ClientIDMode="Static" CssClass="number" Width="40px"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorSTART_HOURS" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_HOURS_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_START_HOURS" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_END_SIGN%>" SortExpression="END_SIGN" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_SIGN" runat="server" Text='<%#Bind("END_SIGN")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_END_SIGN" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hid_END_SIGN" runat="server" Value='<%#Bind("END_SIGN")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_END_SIGN" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_END_TIME_CD%>" SortExpression="END_TIME_CD" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_TIME_CD" runat="server" Text='<%#Bind("END_TIME_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_END_TIME_CD" runat="server" Text='<%#Bind("END_TIME_CD")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_NEW_END_TIME_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_END_TIME_CD%>"
                                ControlToValidate="ddl_NEW_END_TIME_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_START_SIGN_AD%>" SortExpression="END_SIGN_AD" HeaderStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_SIGN_AD" runat="server" Text='<%#Bind("END_SIGN_AD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_END_SIGN_AD" runat="server" Text='<%#Bind("END_SIGN_AD")%>' ClientIDMode="Static" MaxLength="1" Width="120px"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_END_SIGN_AD" runat="server" ClientIDMode="Static" MaxLength="1" Width="120px"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_START_HOURS%>" SortExpression="END_HOURS" HeaderStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lb_END_HOURS" runat="server" Text='<%#Bind("END_HOURS")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_END_HOURS" runat="server" Text='<%#Bind("END_HOURS")%>' CssClass="number" Width="40px"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorEND_HOURS" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_HOURS_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_END_HOURS" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_NEW_END_HOURS" runat="server" ClientIDMode="Static" CssClass="number" Width="40px"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorEND_HOURS" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_HOURS_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_END_HOURS" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020px">
                        <tr class="header">
                            <td>
                                <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dh_RowNumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_START_SIGN%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_START_TIME_CD%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_START_SIGN_AD%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_START_HOURS%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_END_SIGN%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_END_TIME_CD%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label8" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_START_SIGN_AD%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_START_HOURS%>" Width="40px"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td>
                                <asp:CheckBox ID="cb_check" runat="server" Width="20px" />
                            </td>
                            <td>
                                <asp:Label ID="lb_NewRowNumber" runat="server" Text="1" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_START_SIGN" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_START_TIME_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_START_TIME_CD%>"
                                    ControlToValidate="ddl_NEW_START_TIME_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_START_SIGN_AD" runat="server" ClientIDMode="Static" MaxLength="1" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_START_HOURS" runat="server" ClientIDMode="Static" CssClass="number" Width="40px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorSTART_HOURS" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_HOURS_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_START_HOURS" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_END_SIGN" runat="server" ClientIDMode="Static"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_END_TIME_CD" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_END_TIME_CD%>"
                                    ControlToValidate="ddl_NEW_END_TIME_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_END_SIGN_AD" runat="server" ClientIDMode="Static" MaxLength="1" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_END_HOURS" runat="server" ClientIDMode="Static" CssClass="number" Width="40px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorEND_HOURS" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_HOURS_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_END_HOURS" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                        </tr>
                    </table>

                </EmptyDataTemplate>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />

            </asp:GridView>
            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">

                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                            <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
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
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
