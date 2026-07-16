<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb299/WFB2990100_Dtl.aspx.cs" Inherits="WebContent_fb299_WFB2990100_Dtl" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $("#txt_ORDER_SEQ_Add").mask('999');
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
        function CheckORDER_SEQ(source, arguments) {
            var re = /^[\d]+$/;
            if ($("#txt_ORDER_SEQ_Add").val() != "") {                                               //排序有值做檢查
                if (!re.test($("#txt_ORDER_SEQ_Add").val())) {                                       //輸入的為數字
                    arguments.IsValid = false;
                }
                else {
                    if ($("#txt_ORDER_SEQ_Add").val() >= 1 && $("#txt_ORDER_SEQ_Add").val() <= 999)    //輸入的數字在1~999間
                        arguments.IsValid = true;
                    else
                        arguments.IsValid = false;
                }
            }
            else {
                arguments.IsValid = true;
            }
        }
        function CheckSUB_CD(source, arguments) {
            var re = /^[0-9a-zA-Z_]+$/;
            if ($("#txt_SUB_CD_Add").val().trim() != "") {
                if (!re.test($("#txt_SUB_CD_Add").val()))
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
                                <col width="30%" />
                                <col width="20%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MAIN_CD" runat="server" Text="<%$Resources:Resource,wfb299_lb_MAIN_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_MAIN_CD_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_MAIN_DESC" runat="server" Text="<%$Resources:Resource,wfb299_lb_MAIN_DESC%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:Label ID="lb_MAIN_DESC_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label"></td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">
                        <div id="init_grid">
                             <aces:Btn ID="WFB2990101Add" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990101Add%>" OnClick="WFB2990101Add_Click" />
                            <aces:Btn ID="WFB2990101Delete" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990101Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2990101Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2990101Edit" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990101Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2990101Edit_Click" Visible="False" />
                            <aces:Btn ID="WFB2990101Save" runat="server" Text="<%$Resources:Resource,btn_confirm%>" Visible="false" OnClick="WFB2990101Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupB" />
                            <%--<asp:Button ID="WFB2990101Add" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990101Add%>" OnClick="WFB2990101Add_Click" />
                            <asp:Button ID="WFB2990101Delete" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990101Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2990101Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2990101Edit" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990101Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2990101Edit_Click" Visible="False" />
                            <asp:Button ID="WFB2990101Save" runat="server" Text="<%$Resources:Resource,btn_confirm%>" Visible="false" OnClick="WFB2990101Save_Click" OnClientClick="CheckValid();" ValidationGroup="GroupB" />--%>
                            <asp:Button ID="btn_cancel" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Cancel%>" Visible="false" OnClick="btn_cancel_Click" OnClientClick="return confirm($('#hidwfb299_Cancel_ConfirmMessage').val());" />
                            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Back%>" OnClick="btn_back_Click" />
                        </div>

                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getDtlData"
                SelectCountMethod="getDtlCount" TypeName="CFB2990100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="ods1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_sys_cd"
                        Name="sys_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_main_cd"
                        Name="main_cd" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
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
                        <EditItemTemplate>
                            <div style="text-align: center;">
                                <asp:CheckBox ID="cb_check" runat="server" Checked="true" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb299_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'  Width="40px"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lbl_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lbl_SUB_CD%>" SortExpression="SUB_CD" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <div style="text-align: center;">
                                <asp:Label ID="lbl_SUB_CD" runat="server" Width="80px" Text='<%#Bind("SUB_CD")%>' Style="word-wrap: break-word;"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center;">
                                <asp:TextBox ID="txt_SUB_CD_Add" runat="server" ClientIDMode="Static" MaxLength="50" Width="80px" CssClass="MandatoryField"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_SUB_CD_isNull%>"
                                    ControlToValidate="txt_SUB_CD_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                </asp:RequiredFieldValidator>
                                 <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb299_SUB_CD_isError%>" ClientValidationFunction="CheckSUB_CD" ForeColor="Red"
                                    ControlToValidate="txt_SUB_CD_Add" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lbl_SUB_DESC%>" SortExpression="SUB_DESC" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                        <ItemTemplate>
                            <div style="text-align: left;">
                                <asp:Label ID="lbl_SUB_DESC" runat="server" Width="200px" Text='<%#Bind("SUB_DESC")%>' Style="word-wrap: break-word;"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_SUB_DESC_Add" runat="server" Width="180px" Text='<%#Bind("SUB_DESC")%>' MaxLength="150" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_SUB_DESC_isNull%>"
                                ControlToValidate="txt_SUB_DESC_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center;">
                                <asp:TextBox ID="txt_SUB_DESC_Add" runat="server" ClientIDMode="Static" MaxLength="150" CssClass="MandatoryField" Width="180px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_SUB_DESC_isNull%>"
                                    ControlToValidate="txt_SUB_DESC_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                </asp:RequiredFieldValidator>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lbl_CODE_VAL1%>" SortExpression="CODE_VAL1" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lbl_CODE_VAL1" runat="server" Width="90px" Text='<%#Bind("CODE_VAL1")%>' Style="word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_CODE_VAL1_Add" runat="server" Text='<%#Bind("CODE_VAL1")%>' MaxLength="100" Width="80px" ClientIDMode="Static"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txt_CODE_VAL1_Add" runat="server" ClientIDMode="Static" MaxLength="100" Width="80px"></asp:TextBox>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lbl_CODE_VAL2%>" SortExpression="CODE_VAL2" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lbl_CODE_VAL2" runat="server" Width="100px" Text='<%#Bind("CODE_VAL2")%>' Style="word-wrap: break-word;"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_CODE_VAL2_Add" runat="server" Text='<%#Bind("CODE_VAL2")%>' MaxLength="100" Width="80px" ClientIDMode="Static"></asp:TextBox>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center;">
                                <asp:TextBox ID="txt_CODE_VAL2_Add" runat="server" MaxLength="100" ClientIDMode="Static" Width="80px"></asp:TextBox>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lbl_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="240px" ItemStyle-Width="240px">
                        <ItemTemplate>
                            <div style="text-align: left;">
                                <asp:Label ID="lbl_REMARK" runat="server" Width="230px" Text='<%#Bind("REMARK")%>' Style="word-wrap: break-word;"></asp:Label>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: left;">
                                <asp:TextBox ID="txt_REMARK_Add" runat="server" Text='<%#Bind("REMARK")%>' MaxLength="250" Width="220px" ClientIDMode="Static"></asp:TextBox>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: left;">
                                <asp:TextBox ID="txt_REMARK_Add" runat="server" ClientIDMode="Static" MaxLength="250" Width="220px"></asp:TextBox>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lbl_IS_VALID%>" SortExpression="IS_VALID" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label ID="lbl_IS_VALID" runat="server" Width="90px" Text='<%#Bind("IS_VALID")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_IS_VALID_Add" runat="server" MaxLength="1" ClientIDMode="Static">
                                    <asp:ListItem Value="<%$Resources:Resource,wfb299_dll_USER_UPD_Y%>" Text="<%$Resources:Resource,wfb299_dll_USER_UPD_Y%>"></asp:ListItem>
                                    <asp:ListItem Value="<%$Resources:Resource,wfb299_dll_USER_UPD_N%>" Text="<%$Resources:Resource,wfb299_dll_USER_UPD_N%>"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_IS_VALID_Add" runat="server" MaxLength="1" ClientIDMode="Static">
                                    <asp:ListItem Value="<%$Resources:Resource,wfb299_dll_USER_UPD_Place%>"
                                        Text="<%$Resources:Resource,wfb299_dll_USER_UPD_Place%>"></asp:ListItem>
                                    <asp:ListItem Value="<%$Resources:Resource,wfb299_dll_USER_UPD_Y%>" Text="<%$Resources:Resource,wfb299_dll_USER_UPD_Y%>" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="<%$Resources:Resource,wfb299_dll_USER_UPD_N%>" Text="<%$Resources:Resource,wfb299_dll_USER_UPD_N%>"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lbl_ORDER_SEQ%>" SortExpression="ORDER_SEQ" HeaderStyle-Width="80px" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lbl_ORDER_SEQ" runat="server" Width="70px" Text='<%#Bind("ORDER_SEQ")%>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_ORDER_SEQ_Add" runat="server" Text='<%#Bind("ORDER_SEQ")%>' MaxLength="3" Width="60px" ClientIDMode="Static"></asp:TextBox>
                            <asp:CustomValidator ID="CustomValidator4" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb299_ORDER_SEQ_isError%>" ClientValidationFunction="CheckORDER_SEQ" ForeColor="Red"
                                ControlToValidate="txt_ORDER_SEQ_Add" ValidationGroup="GroupB" Display="None">
                            </asp:CustomValidator>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center;">
                                <asp:TextBox ID="txt_ORDER_SEQ_Add" runat="server" MaxLength="3" ClientIDMode="Static" Width="60px"></asp:TextBox>
                                <asp:CustomValidator ID="CustomValidator5" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb299_ORDER_SEQ_isError%>" ClientValidationFunction="CheckORDER_SEQ" ForeColor="Red"
                                    ControlToValidate="txt_ORDER_SEQ_Add" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
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
                                <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb299_lbl_ORDER_SEQ%>"></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="lblHeaderSUB_CD" runat="server" Text="<%$Resources:Resource,wfb299_lbl_SUB_CD%>"></asp:Label>
                            </td>
                            <td width="200px">
                                <asp:Label ID="lblHeaderSUB_DESC" runat="server" Text="<%$Resources:Resource,wfb299_lbl_SUB_DESC%>"></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="lblHeaderCODE_VAL1" runat="server" Text="<%$Resources:Resource,wfb299_lbl_CODE_VAL1%>"></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="lblHeaderCODE_VAL2" runat="server" Text="<%$Resources:Resource,wfb299_lbl_CODE_VAL2%>"></asp:Label>
                            </td>
                            <td width="240px">
                                <asp:Label ID="lblHeaderREMARK" runat="server" Text="<%$Resources:Resource,wfb299_lbl_REMARK%>"></asp:Label>
                            </td>
                            <td width="100px">
                                <asp:Label ID="lblHeaderIS_VALID" runat="server" Text="<%$Resources:Resource,wfb299_lbl_IS_VALID%>"></asp:Label>
                            </td>
                            <td width="80px">
                                <asp:Label ID="lblHeaderORDER_SEQ" runat="server" Text="<%$Resources:Resource,wfb299_lbl_ORDER_SEQ%>"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:TextBox ID="txt_SUB_CD_Add" runat="server" ClientIDMode="Static" MaxLength="50" CssClass="MandatoryField" Width="80px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<%$Resources:Resource,wfb299_SUB_CD_isNull%>"
                                        ControlToValidate="txt_SUB_CD_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                    </asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="CustomValidator6" runat="server" ValidateEmptyText="true"
                                    ErrorMessage="<%$Resources:Resource,wfb299_SUB_CD_isError%>" ClientValidationFunction="CheckSUB_CD" ForeColor="Red"
                                    ControlToValidate="txt_SUB_CD_Add" ValidationGroup="GroupB" Display="None">
                                </asp:CustomValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:TextBox ID="txt_SUB_DESC_Add" runat="server" ClientIDMode="Static" CssClass="MandatoryField" Width="180px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" MaxLength="150" ErrorMessage="<%$Resources:Resource,wfb299_SUB_DESC_isNull%>"
                                        ControlToValidate="txt_SUB_DESC_Add" ForeColor="Red" ValidationGroup="GroupB" Display="None">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:TextBox ID="txt_CODE_VAL1_Add" runat="server" ClientIDMode="Static" MaxLength="100" Width="80px"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:TextBox ID="txt_CODE_VAL2_Add" runat="server" ClientIDMode="Static" MaxLength="100" Width="80px"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:TextBox ID="txt_REMARK_Add" runat="server" ClientIDMode="Static" MaxLength="250" Width="220px"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:DropDownList ID="ddl_IS_VALID_Add" runat="server" MaxLength="1" ClientIDMode="Static">
                                        <asp:ListItem Value="<%$Resources:Resource,wfb299_dll_USER_UPD_Place%>"
                                            Text="<%$Resources:Resource,wfb299_dll_USER_UPD_Place%>"></asp:ListItem>
                                        <asp:ListItem Value="<%$Resources:Resource,wfb299_dll_USER_UPD_Y%>" Text="<%$Resources:Resource,wfb299_dll_USER_UPD_Y%>" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="<%$Resources:Resource,wfb299_dll_USER_UPD_N%>" Text="<%$Resources:Resource,wfb299_dll_USER_UPD_N%>"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center;">
                                    <asp:TextBox ID="txt_ORDER_SEQ_Add" runat="server" MaxLength="3" ClientIDMode="Static" Width="60px"></asp:TextBox>
                                    <asp:CustomValidator ID="CustomValidator8" runat="server" ValidateEmptyText="true"
                                        ErrorMessage="<%$Resources:Resource,wfb299_ORDER_SEQ_isError%>" ClientValidationFunction="CheckORDER_SEQ" ForeColor="Red"
                                        ControlToValidate="txt_ORDER_SEQ_Add" ValidationGroup="GroupB" Display="None">
                                    </asp:CustomValidator>
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
            <asp:HiddenField ID="hid_sys_cd" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_USER_UPD" runat="server" ClientIDMode="static" />
            <asp:HiddenField ID="hid_main_cd" runat="server" ClientIDMode="Static" />

           <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Mod_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Save_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Dtl_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupB" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


