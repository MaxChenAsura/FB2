<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC1200_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC1200_Qry" %>

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
            $("#txt_ORDER_SEQ_Add").mask('9999');
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
        function CheckAMOUNT(source, arguments) {
            var re = /^(0|[1-9][0-9]*)$/;
            if ($("#txt_ORDER_SEQ").val() != "") {
                if (!re.test($("#txt_ORDER_SEQ").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
        }
        function ClearAll() {
            $('#txt_GROUP_NAME_search').val("");
            $('#txt_GROUP_ID_search').val("");
            $('#txt_SUB_GROUP_ID_search').val("");
            $('#txt_SUB_GROUP_NAME_search').val("");
            $('#ddl_KIND_CD_search').val("");
            $('#ddl_GROUP_TYPE_search').val("");
            $('#ddl_CLASSIFY_search').val("");
            $('#ddl_LEVEL_search').val("");
            return false;
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb2sc_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb2sc_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2sc_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2sc_Dtl_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckSaveAction() {
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    alert($('#hidwfb2sc_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hidwfb2sc_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hidwfb2sc_Save_ConfirmMessage').val());
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
                                <col width="30%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--用途別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_KIND_CD_search" runat="server" Text="<%$Resources:Resource,wfb2sc_KIND_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_KIND_CD_search" runat="server" ClientIDMode="Static"
                                            OnSelectedIndexChanged="ddl_KIND_CD_search_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <%--群組類別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GROUP_TYPE_search" runat="server" Text="<%$Resources:Resource,wfb2sc_GROUP_TYPE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_GROUP_TYPE_search" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%--群組名稱--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GROUP_NAME_search" runat="server" Text="<%$Resources:Resource,wfb2sc_GROUP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_GROUP_NAME_search" runat="server" MaxLength="20" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--歸納方式--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CLASSIFY_search" runat="server" Text="<%$Resources:Resource,wfb2sc_CLASSIFY%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_CLASSIFY_search" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%--群組代號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GROUP_ID_search" runat="server" Text="<%$Resources:Resource,wfb2sc_GROUP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_GROUP_ID_search" runat="server" MaxLength="5" Width="80px" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--層級--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEVEL_search" runat="server" Text="<%$Resources:Resource,wfb2sc_LEVEL%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_LEVEL_search" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%--子項目代號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SUB_GROUP_ID_search" runat="server" Text="<%$Resources:Resource,wfb2sc_SUB_GROUP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SUB_GROUP_ID_search" Width="80px" runat="server" MaxLength="5" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--子項目名稱--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SUB_GROUP_NAME_search" runat="server" Text="<%$Resources:Resource,wfb2sc_SUB_GROUP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SUB_GROUP_NAME_search" runat="server" MaxLength="20" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SC1200Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Search%>" OnClick="WFB2SC1200Search_Click" OnClientClick="BlockUI();" ValidationGroup="GroupA" />
                                            <%--<asp:Button ID="WFB2SC1200Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Search%>" OnClick="WFB2SC1200Search_Click" OnClientClick="BlockUI();" ValidationGroup="GroupA" />--%>
                                            <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,wfb2sc_btn_clear%>" OnClientClick="return ClearAll();" />
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
                            <aces:Btn ID="WFB2SC1200Add" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC1200Add_Click" />
                            <aces:Btn ID="WFB2SC1200Delete" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SC1200Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2SC1200Edit" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2SC1200Edit_Click" Visible="False" />
                            <aces:Btn ID="WFB2SC1200Detail" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Detail%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2SC1200Detail_Click" Visible="false" />
                            <aces:Btn ID="WFB2SC1200Save" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Save%>" Visible="false" OnClick="WFB2SC1200Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                            <%--<asp:Button ID="WFB2SC1200Add" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC1200Add_Click" />
                            <asp:Button ID="WFB2SC1200Delete" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SC1200Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2SC1200Edit" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2SC1200Edit_Click" Visible="False" />
                            <asp:Button ID="WFB2SC1200Detail" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Detail%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2SC1200Detail_Click" Visible="false" />
                            <asp:Button ID="WFB2SC1200Save" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1200Save%>" Visible="false" OnClick="WFB2SC1200Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb2sc_btn_cancel%>" Visible="false" OnClick="btn_cancel_Click" OnClientClick="return confirm($('#hidwfb2sc_Cancel_ConfirmMessage').val());" />
                        </div>

                    </td>
                </tr>
                <tr></tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SC1200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_KIND_CD_search"
                        Name="kind_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_GROUP_TYPE_search"
                        Name="group_type" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_GROUP_NAME_search"
                        Name="group_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_CLASSIFY_search"
                        Name="classify" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_GROUP_ID_search"
                        Name="group_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_LEVEL_search"
                        Name="level" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_SUB_GROUP_ID_search"
                        Name="sub_group_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_SUB_GROUP_NAME_search"
                        Name="sub_group_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1000px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2sc_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Style="text-align: center;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Style="text-align: center;"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbl_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Style="text-align: center;"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--用途別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_KIND_CD%>" SortExpression="KIND_CD" HeaderStyle-Width="130px" ItemStyle-Width="130px">
                        <ItemTemplate>
                            <asp:Label ID="lb_KIND_CD" runat="server" Text='<%#Bind("DESC1")%>' Width="100%" Style="text-align: left;"></asp:Label>
                            <asp:HiddenField ID="hid_KIND_CD_Add" ClientIDMode="Static" runat="server" Value='<%#Bind("KIND_CD")%>' />
                            <asp:HiddenField ID="hid_GROUP_TYPE_Add" ClientIDMode="Static" runat="server" Value='<%#Bind("GROUP_TYPE")%>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_KIND_CD_Add" ClientIDMode="Static" MaxLength="1" runat="server" Text='<%#Bind("DESC1")%>' Width="100%" Style="text-align: left;" />
                            <asp:HiddenField ID="hid_KIND_CD_Add" ClientIDMode="Static" runat="server" Value='<%#Bind("KIND_CD")%>' />
                            <asp:HiddenField ID="hid_GROUP_TYPE_Add" ClientIDMode="Static" runat="server" Value='<%#Bind("GROUP_TYPE")%>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_KIND_CD_Add" CssClass="MandatoryField" ClientIDMode="Static" MaxLength="1" runat="server"
                                OnSelectedIndexChanged="ddl_KIND_CD_Add_SelectedIndexChanged" AutoPostBack="true" Width="100%" Style="text-align: center;" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_KIND_CD_isNull%>"
                                ControlToValidate="ddl_KIND_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--群組類別--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_GROUP_TYPE%>" SortExpression="GROUP_TYPE" HeaderStyle-Width="130px" ItemStyle-Width="130px">
                        <ItemTemplate>
                            <asp:Label ID="lb_GROUP_TYPE" runat="server" Text='<%#Bind("DESC2")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_GROUP_TYPE_Add" ClientIDMode="Static" runat="server" Text='<%#Bind("DESC2")%>' Width="100%" Style="text-align: left;" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_GROUP_TYPE_Add" CssClass="MandatoryField" ClientIDMode="Static" MaxLength="1"
                                OnSelectedIndexChanged="ddl_GROUP_TYPE_Add_SelectedIndexChanged" AutoPostBack="true" runat="server" Width="100%" Style="text-align: center;" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_GROUP_TYPE_isNull%>"
                                ControlToValidate="ddl_GROUP_TYPE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--群組代號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_GROUP_ID%>" SortExpression="GROUP_ID" HeaderStyle-Width="120px" ItemStyle-Width="120px">
                        <ItemTemplate>
                            <asp:Label ID="lb_GROUP_ID" runat="server" Text='<%#Bind("GROUP_ID")%>' Width="100%" Style="text-align: center;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_GROUP_ID_Add" ClientIDMode="Static" runat="server" Text='<%#Bind("GROUP_ID")%>' Width="100%" Style="text-align: center;"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lb_GROUP_ID_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--群組名稱--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_GROUP_NAME%>" SortExpression="GROUP_NAME" HeaderStyle-Width="230px" ItemStyle-Width="230px">
                        <ItemTemplate>
                            <asp:Label ID="lb_GROUP_NAME" runat="server" Text='<%#Bind("GROUP_NAME")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_GROUP_NAME_Add" ClientIDMode="Static" CssClass="MandatoryField" MaxLength="20" runat="server" Text='<%#Bind("GROUP_NAME")%>' Width="100%" Style="text-align: left;" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_GROUP_NAME_isNull%>"
                                ControlToValidate="txt_GROUP_NAME_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_GROUP_NAME_Add" ClientIDMode="Static" CssClass="MandatoryField" MaxLength="20" runat="server" Width="100%" Style="text-align: left;" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_GROUP_NAME_isNull%>"
                                ControlToValidate="txt_GROUP_NAME_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--層級--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_LEVEL%>" SortExpression="GROUP_NAME" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lb_LEVEL" runat="server" Text='<%#Bind("LEVEL")%>' Width="100%" Style="text-align: center;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lb_LEVEL_Add" runat="server" Text='<%#Bind("LEVEL")%>' Width="100%" Style="text-align: center;"></asp:Label>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_LEVEL_Add" ClientIDMode="Static" runat="server" CssClass="MandatoryField"
                                OnSelectedIndexChanged="ddl_LEVEL_Add_SelectedIndexChanged" AutoPostBack="true" Width="100%" Style="text-align: center;">
                                <asp:ListItem Value="" Text=""></asp:ListItem>
                                <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                <asp:ListItem Value="3" Text="3"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_LEVEL_isNull%>"
                                ControlToValidate="ddl_LEVEL_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                            </asp:RequiredFieldValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--歸納方式--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_CLASSIFY%>" SortExpression="CLASSIFY" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lb_CLASSIFY" runat="server" Text='<%#Bind("DESC3")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_CLASSIFY_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_CLASSIFY_Add" ClientIDMode="Static" runat="server" Width="100%" Style="text-align: center;" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%--排列順序--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_ORDER_SEQ%>" SortExpression="ORDER_SEQ" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="lb_ORDER_SEQ" runat="server" Text='<%#Bind("ORDER_SEQ")%>' Width="100%" Style="text-align: left;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_ORDER_SEQ_Add" ClientIDMode="Static" MaxLength="4" runat="server" Text='<%#Bind("ORDER_SEQ")%>' Width="100%" Style="text-align: left;" />
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_ORDER_SEQ_error%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_ORDER_SEQ_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txt_ORDER_SEQ_Add" ClientIDMode="Static" MaxLength="4" runat="server" Width="100%" Style="text-align: left;" />
                            <asp:CustomValidator ID="CustomValidator3" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2sc_ORDER_SEQ_error%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                ControlToValidate="txt_ORDER_SEQ_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td width="30px"></td>
                            <td width="40px">
                                <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb2sc_RowNumber%>"></asp:Label>
                            </td>
                            <td width="150px">
                                <asp:Label ID="lblHeaderKIND_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_KIND_CD%>"></asp:Label>
                            </td>
                            <td width="150px">
                                <asp:Label ID="lblHeaderGROUP_TYPE" runat="server" Text="<%$Resources:Resource,wfb2sc_GROUP_TYPE%>"></asp:Label>
                            </td>
                            <td width="150px">
                                <asp:Label ID="lblHeaderGROUP_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_GROUP_ID%>"></asp:Label>
                            </td>
                            <td width="170px">
                                <asp:Label ID="lblHeaderGROUP_NAME" runat="server" Text="<%$Resources:Resource,wfb2sc_GROUP_NAME%>"></asp:Label>
                            </td>
                            <td width="60px">
                                <asp:Label ID="lblHeaderLEVEL" runat="server" Text="<%$Resources:Resource,wfb2sc_LEVEL%>"></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="lblHeaderCLASSIFY" runat="server" Text="<%$Resources:Resource,wfb2sc_CLASSIFY%>"></asp:Label>
                            </td>
                            <td width="150px">
                                <asp:Label ID="lblHeaderORDER_SEQ" runat="server" Text="<%$Resources:Resource,wfb2sc_ORDER_SEQ%>"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td style="width: 30px;"></td>
                            <td style="width: 40px;"></td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:DropDownList ID="ddl_KIND_CD_Add" ClientIDMode="Static" CssClass="MandatoryField"
                                        OnSelectedIndexChanged="ddl_KIND_CD_Add_SelectedIndexChanged" AutoPostBack="true" Width="120px" runat="server" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_KIND_CD_isNull%>"
                                        ControlToValidate="ddl_KIND_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:DropDownList ID="ddl_GROUP_TYPE_Add" ClientIDMode="Static" CssClass="MandatoryField"
                                        OnSelectedIndexChanged="ddl_GROUP_TYPE_Add_SelectedIndexChanged" AutoPostBack="true" Width="120px" runat="server" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_GROUP_TYPE_isNull%>"
                                        ControlToValidate="ddl_GROUP_TYPE_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:Label ID="lb_GROUP_ID_Add" ClientIDMode="Static" runat="server" Width="120px" Style="text-align: center;"></asp:Label>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:TextBox ID="txt_GROUP_NAME_Add" ClientIDMode="Static" MaxLength="20" Width="230px" runat="server" />
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:DropDownList ID="ddl_LEVEL_Add" ClientIDMode="Static" CssClass="MandatoryField" OnSelectedIndexChanged="ddl_LEVEL_Add_SelectedIndexChanged"
                                        AutoPostBack="true" runat="server" Width="60px">
                                        <asp:ListItem Value="" Text=""></asp:ListItem>
                                        <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<%$Resources:Resource,wfb2sc_LEVEL_isNull%>"
                                        ControlToValidate="ddl_LEVEL_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:DropDownList ID="ddl_CLASSIFY_Add" ClientIDMode="Static" runat="server" Width="100px" />
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:TextBox ID="txt_ORDER_SEQ_Add" ClientIDMode="Static" MaxLength="4" runat="server" Width="150px" />
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb2sc_ORDER_SEQ_error%>" ClientValidationFunction="CheckIsNaN" ForeColor="Red"
                                        ControlToValidate="txt_ORDER_SEQ_Add" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                </div>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
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
            <asp:HiddenField ID="HID_KIND_CD" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Mod_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Cancel_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_SUB_GROUP_ID_importError" Value="<%$Resources:Resource,wfb2sc_SUB_GROUP_ID_importError %>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


