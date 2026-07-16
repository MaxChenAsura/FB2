<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dc/action/WFB2DC0200_Qry.aspx.cs" Inherits="WebContent_WFB2DC0200_Qry" Culture="auto" UICulture="auto" %>
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

        //清空畫面
        function ClearAll() {
            $("#ddl_CARD_TYPE").val("-1");
        }

        function CardDesc() {
            var features = "directories:no;location:no;statusbar:no;menubar:no;toolbar:no;scrollbars:yes;" +
                                        "dialogHeight:400px;dialogWidth:650px;dialogTop:240px;dialogLeft:370px";
            var rtVal = window.showModalDialog("WFB2DC0200_Desc.aspx", window, features);
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
                                <col width="15%" />
                                <col width="40%" />
                                <col width="15%" />
                                <col width="30%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CARD_TYPE" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_CARD_TYPE" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CARD_DESC" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_DESC%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Button ID="btn_CARD_DESC" runat="server" Text="<%$Resources:Resource,wfb2dc_btn_CARD_DESC%>" OnClick="btn_CARD_DESC_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DC0200Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Search%>" OnClientClick="BlockUI();" OnClick="WFB2DC0200Search_Click" />

                                            <%--<asp:Button ID="WFB2DC0200Search" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Search%>" OnClientClick="BlockUI();" OnClick="WFB2DC0200Search_Click" />--%>
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dc_btn_clear%>" onclick="ClearAll();" />
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
                        <div id="init_grid">
                        <aces:Btn ID="WFB2DC0200Add" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Add%>" OnClick="WFB2DC0200Add_Click" />
                            <aces:Btn ID="WFB2DC0200Delete" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DC0200Delete_Click" />
                            <aces:Btn ID="WFB2DC0200Edit" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Edit%>" Visible="false" OnClick="WFB2DC0200Edit_Click" />
                            <aces:Btn ID="WFB2DC0200Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DC0200Save_Click" />

<%--                            <asp:Button ID="WFB2DC0200Add" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Add%>" OnClick="WFB2DC0200Add_Click" />
                            <asp:Button ID="WFB2DC0200Delete" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DC0200Delete_Click" />
                            <asp:Button ID="WFB2DC0200Edit" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Edit%>" Visible="false" OnClick="WFB2DC0200Edit_Click" />
                            <asp:Button ID="WFB2DC0200Save" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DC0200Save_Click" />--%>
                            
                            <asp:Button ID="WFB2DC0200Cancel" runat="server" Text="<%$Resources:Resource,wfb2dc_WFB2DC0200Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="WFB2DC0200Cancel_Click" />
                        </div>
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DC0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_CARD_TYPE" DefaultValue=""
                        Name="card_type" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" Width="1020px" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound"
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_RowNumber%>" HeaderStyle-Width="40px">
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CARD_TYPE%>" SortExpression="CARD_TYPE" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CARD_TYPE" runat="server" Text='<%#Bind("CARD_TYPE")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_CARD_TYPE" runat="server" Text='<%#Bind("CARD_TYPE")%>'></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_CARD_TYPE" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="80px" MaxLength="2"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CARD_TYPE%>"
                                ControlToValidate="txt_NEW_CARD_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CARD_TYPE_Chinese%>" ControlToValidate="txt_NEW_CARD_TYPE" ForeColor="Red"
                                ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CARD_TYPE_DESC%>" SortExpression="CARD_TYPE_DESC" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CARD_TYPE_DESC" runat="server" Text='<%#Bind("CARD_TYPE_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_CARD_TYPE_DESC" runat="server" Text='<%#Bind("CARD_TYPE_DESC")%>' Width="120px" MaxLength="30"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_CARD_TYPE_DESC" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="120px" MaxLength="30"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CARD_TYPE_DESC%>"
                                ControlToValidate="txt_NEW_CARD_TYPE_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CLOCK_TYPE_A%>" SortExpression="CLOCK_TYPE_A" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_CLOCK_TYPE_A" runat="server" ClientIDMode="Static" Enabled="false" />
                            <asp:HiddenField ID="hid_CLOCK_TYPE_A" runat="server" Value='<%#Bind("CLOCK_TYPE_A")%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_CLOCK_TYPE_A" runat="server" ClientIDMode="Static" Width="80px" />
                            </div>
                            <asp:HiddenField ID="hid_CLOCK_TYPE_A" runat="server" Value='<%#Bind("CLOCK_TYPE_A")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_NEW_CLOCK_TYPE_A" runat="server" ClientIDMode="Static" Width="80px" />
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CLOCK_TYPE_B%>" SortExpression="CLOCK_TYPE_B" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_CLOCK_TYPE_B" runat="server" ClientIDMode="Static" Enabled="false" />
                            <asp:HiddenField ID="hid_CLOCK_TYPE_B" runat="server" Value='<%#Bind("CLOCK_TYPE_B")%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_CLOCK_TYPE_B" runat="server" ClientIDMode="Static" Width="80px" />
                            </div>
                            <asp:HiddenField ID="hid_CLOCK_TYPE_B" runat="server" Value='<%#Bind("CLOCK_TYPE_B")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_NEW_CLOCK_TYPE_B" runat="server" ClientIDMode="Static" Width="80px" />
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CLOCK_TYPE_C%>" SortExpression="CLOCK_TYPE_C" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_CLOCK_TYPE_C" runat="server" ClientIDMode="Static" Enabled="false" />
                            <asp:HiddenField ID="hid_CLOCK_TYPE_C" runat="server" Value='<%#Bind("CLOCK_TYPE_C")%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_CLOCK_TYPE_C" runat="server" ClientIDMode="Static" Width="100px" />
                            </div>
                            <asp:HiddenField ID="hid_CLOCK_TYPE_C" runat="server" Value='<%#Bind("CLOCK_TYPE_C")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_NEW_CLOCK_TYPE_C" runat="server" ClientIDMode="Static" Width="100px" />
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dc_lb_CARD_USED_CD%>" SortExpression="CARD_USED_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_CARD_USED_CD" runat="server" Text='<%#Bind("CARD_USED_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_CARD_USED_CD" runat="server" ClientIDMode="Static" Width="80px" BackColor="#FFD7D7">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hid_CARD_USED_CD" runat="server" Value='<%#Bind("CARD_USED_CD")%>' />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CARD_USED_CD%>"
                                ControlToValidate="ddl_CARD_USED_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_CARD_USED_CD" runat="server" ClientIDMode="Static" Width="80px" BackColor="#FFD7D7">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CARD_USED_CD%>"
                                ControlToValidate="ddl_NEW_CARD_USED_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
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
                                <asp:Label ID="Label1" runat="server" Text="<%$Resources:Resource,wfb2dc_RowNumber%>" Width="40px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_TYPE%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_TYPE_DESC%>" Width="120px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CLOCK_TYPE_A%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CLOCK_TYPE_B%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CLOCK_TYPE_C%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb2dc_lb_CARD_USED_CD%>" Width="80px"></asp:Label>
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
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_CARD_TYPE" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="80px" MaxLength="2"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CARD_TYPE%>"
                                ControlToValidate="txt_NEW_CARD_TYPE" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CARD_TYPE_Chinese%>" ControlToValidate="txt_NEW_CARD_TYPE" ForeColor="Red"
                                ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                            </td>
                            <td>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_CARD_TYPE_DESC" runat="server" ClientIDMode="Static" BackColor="#FFD7D7" Width="120px" MaxLength="30"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CARD_TYPE_DESC%>"
                                ControlToValidate="txt_NEW_CARD_TYPE_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_NEW_CLOCK_TYPE_A" runat="server" ClientIDMode="Static" Width="80px" />
                            </div>
                            </td>
                            <td>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_NEW_CLOCK_TYPE_B" runat="server" ClientIDMode="Static" Width="80px" />
                            </div>
                            </td>
                            <td>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_NEW_CLOCK_TYPE_C" runat="server" ClientIDMode="Static" Width="100px" />
                            </div>
                            </td>
                            <td>
                            <div style="text-align: left; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_CARD_USED_CD" runat="server" ClientIDMode="Static" Width="80px" BackColor="#FFD7D7">
                                </asp:DropDownList>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dc_ERR_CARD_USED_CD%>"
                                ControlToValidate="ddl_NEW_CARD_USED_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None" InitialValue="-1"></asp:RequiredFieldValidator>
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

