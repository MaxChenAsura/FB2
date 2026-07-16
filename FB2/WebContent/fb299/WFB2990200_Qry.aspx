<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb299/WFB2990200_Qry.aspx.cs" Inherits="WebContent_fb299_WFB2990200_Qry" %>
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
        function ClearAll() {
            $('#ddl_SYS_CD').val("");
            $('#txt_MAIN_CD').val("");
            $('#txt_MAIN_DESC').val("");
            return false;
        }

        function CheckSYS_CD(source, arguments) {
            var re = /^[\d|a-zA-Z]+$/;
            if ($("#txt_SYS_CD_Add").val().trim() != "") {
                if (!re.test($("#txt_SYS_CD_Add").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
        }
        function CheckMAIN_CD(source, arguments) {
            var re = /^[0-9a-zA-Z_]+$/;
            if ($("#txt_MAIN_CD_Add").val().trim() != "") {
                if (!re.test($("#txt_MAIN_CD_Add").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb299_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
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

        function CheckSaveAction() {
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    alert($('#hidwfb299_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hidwfb299_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hidwfb299_Save_ConfirmMessage').val());
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
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SYS_CD" runat="server" Text="<%$Resources:Resource,wfb299_lb_SYS_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SYS_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MAIN_CD" runat="server" Text="<%$Resources:Resource,wfb299_0200_lbl_MAIN_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_MAIN_CD" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MAIN_DESC" runat="server" Text="<%$Resources:Resource,wfb299_0200_lbl_MAIN_DESC%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_MAIN_DESC" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2990200Search" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Search%>" OnClick="WFB2990200Search_Click" OnClientClick="BlockUI();" ValidationGroup="GroupA" />
                                            <%--<asp:Button ID="WFB2990200Search" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Search%>" OnClick="WFB2990200Search_Click" OnClientClick="BlockUI();" ValidationGroup="GroupA" />--%>
                                            <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,wfb299_WFB2990100Clear%>" OnClientClick="return ClearAll();" />
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
                            <aces:Btn ID="WFB2990200Add" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Add%>" OnClick="WFB2990200Add_Click" />
                            <aces:Btn ID="WFB2990200Delete" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2990200Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2990200Edit" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2990200Edit_Click" Visible="False" />
                            <aces:Btn ID="WFB2990200Save" runat="server" Text="<%$Resources:Resource,btn_confirm%>" Visible="false" OnClick="WFB2990200Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                            <%--<asp:Button ID="WFB2990200Add" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Add%>" OnClick="WFB2990200Add_Click" />
                            <asp:Button ID="WFB2990200Delete" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2990200Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2990200Edit" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2990200Edit_Click" Visible="False" />
                            <asp:Button ID="WFB2990200Save" runat="server" Text="<%$Resources:Resource,btn_confirm%>" Visible="false" OnClick="WFB2990200Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Cancel%>" Visible="false" OnClick="btn_cancel_Click" OnClientClick="return confirm($('#hidwfb299_Cancel_ConfirmMessage').val());" />
                        </div>

                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2990200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="ddl_SYS_CD"
                        Name="sys_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_MAIN_CD"
                        Name="main_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_MAIN_DESC"
                        Name="main_desc" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

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
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb299_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- 子作業別 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_SYS_CD%>" SortExpression="SYS_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lbl_SYS_CD" runat="server" Width="100px" Text='<%#Bind("SYS_CD")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_SYS_CD_Add" runat="server" ClientIDMode="Static" MaxLength="2" Width="80px" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_SYS_CD_isNull%>"
                                    ControlToValidate="txt_SYS_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb299_SYS_CD_isError%>" ClientValidationFunction="CheckSYS_CD" ForeColor="Red"
                                    ControlToValidate="txt_SYS_CD_Add" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- 參數別 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_0200_lbl_MAIN_CD%>" SortExpression="MAIN_CD" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                                <asp:Label ID="lbl_MAIN_CD" runat="server"  Text='<%#Bind("MAIN_CD")%>' Style="word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_MAIN_CD_Add" runat="server" ClientIDMode="Static" MaxLength="50" Width="180px" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_MAIN_CD2_isNull%>"
                                    ControlToValidate="txt_MAIN_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                                <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb299_MAIN_CD2_isError%>" ClientValidationFunction="CheckMAIN_CD" ForeColor="Red"
                                    ControlToValidate="txt_MAIN_CD_Add" ValidationGroup="GroupA" Display="None">
                                </asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_0200_lbl_MAIN_DESC%>" SortExpression="MAIN_DESC" HeaderStyle-Width="300px" ItemStyle-Width="300px">
                        <ItemTemplate>
                            <div style="text-align: left;">
                                <asp:Label ID="lbl_MAIN_DESC" runat="server" Width="300px" Text='<%#Bind("MAIN_DESC")%>' Style="word-wrap: break-word;"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox ID="txt_MAIN_DESC_Add" ClientIDMode="Static" MaxLength="150" runat="server" Width="280px" Text='<%#Bind("MAIN_DESC")%>' />
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_MAIN_DESC_Add" runat="server" MaxLength="150" Width="280px" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_0200_lbl_CODE_VAL1%>" SortExpression="CODE_VAL1" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: left;">
                                <asp:Label ID="lbl_CODE_VAL1" runat="server" Width="100px" Text='<%#Bind("CODE_VAL1")%>' Style="word-wrap: break-word;"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox ID="txt_CODE_VAL1_Add" ClientIDMode="Static" Width="80px" MaxLength="50" CssClass="MandatoryField" runat="server" Text='<%#Bind("CODE_VAL1")%>' />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_CODE_VAL_isNull%>"
                                    ControlToValidate="txt_CODE_VAL1_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_CODE_VAL1_Add" runat="server" Width="80px" MaxLength="50" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_CODE_VAL_isNull%>"
                                    ControlToValidate="txt_CODE_VAL1_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lbl_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="230px" ItemStyle-Width="230px">
                        <ItemTemplate>
                            <div style="text-align: left;">
                                <asp:Label ID="lbl_REMARK" runat="server" Width="230px" MaxLength="250" Text='<%#Bind("REMARK")%>' Style="word-wrap: break-word;"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:TextBox ID="txt_REMARK_Add" ClientIDMode="Static" MaxLength="250" runat="server" Width="210px" Text='<%#Bind("REMARK")%>' />
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:TextBox ID="txt_REMARK_Add" runat="server" MaxLength="250" ClientIDMode="Static" Width="210px"></asp:TextBox>
                            </div>
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
                                <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb299_RowNumber%>"></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="lblHeaderSYS_CD" runat="server" Text="<%$Resources:Resource,wfb299_lb_SYS_CD%>"></asp:Label>
                            </td>
                            <td width="200px">
                                <asp:Label ID="lblHeaderMAIN_CD" runat="server" Text="<%$Resources:Resource,wfb299_0200_lbl_MAIN_CD%>"></asp:Label>
                            </td>
                            <td width="300px">
                                <asp:Label ID="lblHeaderMAIN_DESC" runat="server" Text="<%$Resources:Resource,wfb299_0200_lbl_MAIN_DESC%>"></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="lblHeaderCODE_VAL1" runat="server" Text="<%$Resources:Resource,wfb299_0200_lbl_CODE_VAL1%>"></asp:Label>
                            </td>
                            <td width="230px">
                                <asp:Label ID="lblHeaderREMARK" runat="server" Text="<%$Resources:Resource,wfb299_lbl_REMARK%>"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txt_SYS_CD_Add" ClientIDMode="Static" MaxLength="2" Width="80px" CssClass="MandatoryField" runat="server" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_MAIN_CD_isNull%>"
                                        ControlToValidate="txt_SYS_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator5" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb299_MAIN_CD_isError%>" ClientValidationFunction="CheckSYS_CD" ForeColor="Red"
                                        ControlToValidate="txt_SYS_CD_Add" ValidationGroup="GroupA" Display="None">
                                    </asp:CustomValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_MAIN_CD_Add" ClientIDMode="Static" CssClass="MandatoryField" MaxLength="50" Width="180px" runat="server" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_MAIN_CD2_isNull%>"
                                        ControlToValidate="txt_MAIN_CD_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator6" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb299_MAIN_CD2_isError%>" ClientValidationFunction="CheckMAIN_CD" ForeColor="Red"
                                        ControlToValidate="txt_MAIN_CD_Add" ValidationGroup="GroupA" Display="None">
                                    </asp:CustomValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_MAIN_DESC_Add" MaxLength="150" Width="280px" runat="server"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_CODE_VAL1_Add" ClientIDMode="Static" runat="server" Width="80px" MaxLength="50" CssClass="MandatoryField"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_CODE_VAL_isNull%>"
                                        ControlToValidate="txt_CODE_VAL1_Add" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: left; width: 100%">
                                    <asp:TextBox ID="txt_REMARK_Add" MaxLength="250" Width="210px" runat="server"></asp:TextBox>
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

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Mod_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


