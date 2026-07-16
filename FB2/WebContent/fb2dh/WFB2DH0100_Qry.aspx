<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0100_Qry.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0100_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".number").mask('99');
            $(".numberr").css("text-align", "right");
            $("#txt_MAIN_LEAVE_DESC2").attr("readonly", true);

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
            $("#txt_MAIN_LEAVE_CD").val("");
            $("#txt_MAIN_LEAVE_DESC2").val("");
            $("#ddl_IS_IFLOW_SHOW").val("-1");
            $("#ddl_IS_USED").val("-1");
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

        function getMAIN_LEAVE_DESC(MAIN_LEAVE_id) {
            var txt = document.getElementById(MAIN_LEAVE_id);
            if (txt.value != "") {
                document.getElementById('hid_getMAIN_LEAVE_DESC').click();
                return false;
            } else {
                $("#txt_MAIN_LEAVE_DESC2").val("");
            }
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
                                        <asp:TextBox ID="txt_MAIN_LEAVE_CD" runat="server" MaxLength="2" Width="64px" ClientIDMode="Static" onblur="javascript:getMAIN_LEAVE_DESC(this.id)"></asp:TextBox>
                                        <input id="btn_MAIN_LEAVE_CD" type="button" value="..." onclick="OpenSearch('LeaveType_Search.aspx', 'txt_MAIN_LEAVE_CD', 'txt_MAIN_LEAVE_DESC2', '');" />
                                        <asp:TextBox ID="txt_MAIN_LEAVE_DESC2" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th >
                                        <%-- 
                                        <asp:Label ID="lb_IS_IFLOW_SHOW" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IS_IFLOW_SHOW%>"></asp:Label>:
                                            --%>
                                    </th>
                                    <td >
                                        <%-- 
                                        <asp:DropDownList ID="ddl_IS_IFLOW_SHOW" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                            --%>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_USED" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IS_USED%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_USED" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DH0100Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0100Search%>" OnClientClick="BlockUI();" OnClick="WFB2DH0100Search_Click" />

                                            <%--<asp:Button ID="WFB2DH0100Search" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0100Search%>" OnClientClick="BlockUI();" OnClick="WFB2DH0100Search_Click" />--%>
                                            
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
                        <div id="init_grid">
                        <aces:Btn ID="WFB2DH0100Add" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0100Add%>" OnClick="WFB2DH0100Add_Click" />
                            <aces:Btn ID="WFB2DH0100Delete" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0100Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DH0100Delete_Click" />
                            <aces:Btn ID="WFB2DH0100Edit" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0100Edit%>" Visible="false" OnClick="WFB2DH0100Edit_Click" />
                            <aces:Btn ID="WFB2DH0100Detail" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0100Detail%>" Visible="false" OnClick="WFB2DH0100Detail_Click" />
                            <aces:Btn ID="WFB2DH0100Save" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0100Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DH0100Save_Click" />

                            <%--<asp:Button ID="WFB2DH0100Add" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0100Add%>" OnClick="WFB2DH0100Add_Click" />
                            <asp:Button ID="WFB2DH0100Delete" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0100Delete%>" OnClientClick="return CheckDelAction();" Visible="false" OnClick="WFB2DH0100Delete_Click" />
                            <asp:Button ID="WFB2DH0100Edit" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0100Edit%>" Visible="false" OnClick="WFB2DH0100Edit_Click" />
                            <asp:Button ID="WFB2DH0100Detail" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0100Detail%>" Visible="false" OnClick="WFB2DH0100Detail_Click" />
                            <asp:Button ID="WFB2DH0100Save" runat="server" Text="<%$Resources:Resource,wfb2dh_WFB2DH0100Save%>" Visible="false" ValidationGroup="GroupA" OnClick="WFB2DH0100Save_Click" />--%>
                            
                            <asp:Button ID="btn_Cancel" runat="server" Text="<%$Resources:Resource,wfb2dh_btn_Cancel%>" Visible="false" OnClientClick="return confirm('是否確定取消?');" OnClick="btn_Cancel_Click" />
                        </div>
                    </td>
                </tr>

            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DH0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_MAIN_LEAVE_CD"
                        Name="main_leave_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <%-- 
                    <asp:ControlParameter ControlID="ddl_IS_IFLOW_SHOW" DefaultValue=""
                        Name="is_iflow_show" PropertyName="SelectedValue" Type="String" />
                        --%>
                    <asp:ControlParameter ControlID="ddl_IS_USED" DefaultValue=""
                        Name="is_used" PropertyName="SelectedValue" Type="String" />
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
                    <%-- 主假別代碼 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD2%>" SortExpression="MAIN_LEAVE_CD" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text='<%#Bind("MAIN_LEAVE_CD")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lb_MAIN_LEAVE_CD" runat="server" Text='<%#Bind("MAIN_LEAVE_CD")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_NEW_MAIN_LEAVE_CD" runat="server" ClientIDMode="Static" MaxLength="2" CssClass="MandatoryField" Width="100px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_MAIN_LEAVE_CD%>"
                                ControlToValidate="txt_NEW_MAIN_LEAVE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_MAIN_LEAVE_CD_Chinese%>" ControlToValidate="txt_NEW_MAIN_LEAVE_CD" ForeColor="Red"
                                ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- 主假別名稱 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_DESC%>" SortExpression="MAIN_LEAVE_DESC" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_MAIN_LEAVE_DESC" runat="server" Text='<%#Bind("MAIN_LEAVE_DESC")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_MAIN_LEAVE_DESC" runat="server" Text='<%#Bind("MAIN_LEAVE_DESC")%>' MaxLength="30" ClientIDMode="Static" CssClass="MandatoryField" Width="100px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_MAIN_LEAVE_DESC%>"
                                ControlToValidate="txt_MAIN_LEAVE_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_NEW_MAIN_LEAVE_DESC" runat="server" ClientIDMode="Static" MaxLength="30" CssClass="MandatoryField" Width="100px"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_MAIN_LEAVE_DESC%>"
                                ControlToValidate="txt_NEW_MAIN_LEAVE_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- IFLOW顯示否
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_IS_IFLOW_SHOW%>" SortExpression="IS_IFLOW_SHOW" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_IFLOW_SHOW" runat="server" Text='<%#Bind("IS_IFLOW_SHOW")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_IS_IFLOW_SHOW" runat="server" ClientIDMode="Static" Width="100px"></asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hid_IS_IFLOW_SHOW" runat="server" Value='<%#Bind("IS_IFLOW_SHOW")%>' ClientIDMode="Static" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_IS_IFLOW_SHOW" runat="server" ClientIDMode="Static" Width="100px"></asp:DropDownList>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                         --%>
                    <%--顯示順序 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_ORDER_SEQ%>" SortExpression="ORDER_SEQ" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_ORDER_SEQ" runat="server" Text='<%#Bind("ORDER_SEQ")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_ORDER_SEQ" runat="server" Text='<%#Bind("ORDER_SEQ")%>' ClientIDMode="Static" CssClass="number numberr" Width="80px"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorORDER_SEQ" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_ORDER_SEQ_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_ORDER_SEQ" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_NEW_ORDER_SEQ" runat="server" ClientIDMode="Static" CssClass="number numberr" Width="80px"></asp:TextBox>
                            </div>
                            <asp:CustomValidator ID="CustomValidatorORDER_SEQ" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_ORDER_SEQ_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_NEW_ORDER_SEQ" ValidationGroup="GroupA" Display="None">
                            </asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--使用狀態--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dh_lb_IS_USED%>" SortExpression="IS_USED" HeaderStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_USED" runat="server" Text='<%#Bind("IS_USED")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_IS_USED" runat="server" ClientIDMode="Static" Width="80px"></asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hid_IS_USED" runat="server" Value='<%#Bind("IS_USED")%>' ClientIDMode="Static" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_NEW_IS_USED" runat="server" ClientIDMode="Static" Width="80px"></asp:DropDownList>
                            </div>
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
                                <asp:Label ID="Label2" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD2%>" Width="100px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_DESC%>" Width="100px"></asp:Label>
                            </td>
                            <%-- 
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IS_IFLOW_SHOW%>" Width="100px"></asp:Label>
                            </td>
                            --%>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_ORDER_SEQ%>" Width="80px"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IS_USED%>" Width="80px"></asp:Label>
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
                                <asp:TextBox ID="txt_NEW_MAIN_LEAVE_CD" runat="server" ClientIDMode="Static" MaxLength="2" CssClass="MandatoryField" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_MAIN_LEAVE_CD%>"
                                    ControlToValidate="txt_NEW_MAIN_LEAVE_CD" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_MAIN_LEAVE_CD_Chinese%>" ControlToValidate="txt_NEW_MAIN_LEAVE_CD" ForeColor="Red"
                                    ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                                </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_NEW_MAIN_LEAVE_DESC" runat="server" ClientIDMode="Static" MaxLength="30" CssClass="MandatoryField" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_MAIN_LEAVE_DESC%>"
                                    ControlToValidate="txt_NEW_MAIN_LEAVE_DESC" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            </td>
                            <%-- 
                            <td>
                                <asp:DropDownList ID="ddl_NEW_IS_IFLOW_SHOW" runat="server" ClientIDMode="Static" Width="100px"></asp:DropDownList>
                            </td>
                            --%>
                            <td>
                                <asp:TextBox ID="txt_NEW_ORDER_SEQ" runat="server" ClientIDMode="Static" CssClass="number numberr" Width="80px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidatorORDER_SEQ" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb2dh_ERR_ORDER_SEQ_NUM%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                    ControlToValidate="txt_NEW_ORDER_SEQ" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_NEW_IS_USED" runat="server" ClientIDMode="Static" Width="80px"></asp:DropDownList>
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
            <asp:Button ID="hid_getMAIN_LEAVE_DESC" runat="server" Style="display: none" ClientIDMode="Static" OnClick="hid_getMAIN_LEAVE_DESC_Click" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>
