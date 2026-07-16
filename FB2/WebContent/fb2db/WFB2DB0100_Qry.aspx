<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2db/WFB2DB0100_Qry.aspx.cs" Inherits="WebContent_fb2db_WFB2DB0100Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $("#txt_WORK_DAY_DESC").attr("readonly", true);

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

        function CancelConfirm() {
            if (confirm($('#hidwfb2db_Cancel_Confirm').val())) {
                BlockUI();
                return;
            }
            else {
                $.unblockUI();
                return false;
            }
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0) {
                BlockUI();
                if (confirm($('#hidwfb2db_Del_ConfirmMessage').val())) {
                    BlockUI();
                    return;
                }
                else {
                    $.unblockUI();
                    return false;
                }
            }
            else {
                alert($('#hidwfb2db_Del_NotChoiceMessage').val());
                $.unblockUI();
                return false;
            }
        }

        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1) {
                BlockUI();
                return true;
            }
            else {
                BlockUI();
                alert($('#hidwfb2db_Mod_NotChoiceMessage').val());
                $.unblockUI();
                return false;
            }
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1) {
                BlockUI();
                return true;
            }
            else {
                BlockUI();
                alert($('#hidwfb2db_Dtl_NotChoiceMessage').val());
                $.unblockUI();
                return false;
            }
        }

        function CheckCopyAction() {
            if (LookUpCheckboxs() == 1) {
                BlockUI();
                return true;
            }
            else {
                BlockUI();
                alert($('#hidwfb2db_Copy_NotChoiceMessage').val());
                $.unblockUI();
                return false;
            }
        }

        function CheckSaveAction() {
            var Message = "";

            if ($('#dll_CALENDAR_Edit option:selected').text().trim() == "") {
                if (Message == "")
                    Message = $('#hidwfb2db_dllCALENDARY_NotNull').val();
                else
                    Message += "\r\n" + $('#hidwfb2db_dllCALENDARY_NotNull').val();
            }


            if ($('#txtWORK_SHIFT_CD_Edit').val() != undefined) {
                if ($('#txtWORK_SHIFT_CD_Edit').val().trim() == "") {
                    if (Message == "")
                        Message = $('#hidwfb2db_txtWORK_SHIFT_CD_NotNull').val();
                    else
                        Message += "\r\n" + $('#hidwfb2db_txtWORK_SHIFT_CD_NotNull').val();
                }

            }
            var re = /^[0-9a-zA-Z_]+$/;
            if (!re.test($("#txtWORK_SHIFT_CD_Edit").val()))
                if (Message == "")
                    Message = $('#hid_wfd2da_Error_WORK_SHIFT_CD').val(); //"輪值表代碼欄位只能輸入英數字"
                else
                    Message += "\r\n" + $('#hid_wfd2da_Error_WORK_SHIFT_CD').val(); //"輪值表代碼欄位只能輸入英數字"

            if ($('#txtWORK_SHIFT_DESC_Edit').val().trim() == "") {
                if (Message == "")
                    Message = $('#hidwfb2db_txtWORK_SHIFT_DESC_NotNull').val();
                else
                    Message += "\r\n" + $('#hidwfb2db_txtWORK_SHIFT_DESC_NotNull').val();
            }

            if ($('#dllIS_VALID_Edit option:selected').text().trim() == "") {
                if (Message == "")
                    Message = $('#hidwfb2db_dllIS_VALID_NotNull1').val();
                else
                    Message += "\r\n" + $('#hidwfb2db_dllIS_VALID_NotNull1').val();
            }

            if (Message != "") {
                BlockUI();
                alert(Message);
                $.unblockUI();
                return false;
            }
            else {
                BlockUI();
                if (confirm($('#hidwfb2db_Save_ConfirmMessage').val())) {
                    BlockUI();
                    return;
                }
                else {
                    $.unblockUI();
                    return false;
                }
            }
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].id.search("cb_check") > -1 && ItemCheckBoxs[i].checked) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }
        function ClearAll() {
            $('#dll_CALENDAR_CD').val("");
            $('#txt_WORK_SHIFT_CD').val("");
            $('#txt_WORK_SHIFT_DESC').val("");
            $('#dll_IS_VALID').val("");
            $('#txt_WORK_DAY_CD').val("");
            $('#txt_WORK_DAY_DESC').val("");
            $("#ddl_IS_IFLOW_SHOW").val("");
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table width="100%">
                            <tr>
                                <th align="left" class="Body_TableHeader" width="20%">
                                    <asp:Label runat="server" ID="lbl_CALENDAR_CD" Text="<%$Resources:Resource,wfb2db_lb_CALENDAR_qry%>" />
                                </th>
                                <td colspan="3">
                                    <asp:DropDownList runat="server" ID="dll_CALENDAR_CD" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader" width="20%">
                                    <asp:Label runat="server" ID="lb_WORK_SHIFT_CD" Text="<%$Resources:Resource,wfd2de_lb_WORK_SHIFT_CD_Qry%>" />
                                </th>
                                <td width="30%">
                                    <asp:TextBox ID="txt_WORK_SHIFT_CD" MaxLength="2" Width="30px" runat="server" ClientIDMode="Static" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_WORK_SHIFT_DESC" Text="<%$Resources:Resource,wfd2de_lb_WORK_SHIFT_DESC_Qry%>" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_WORK_SHIFT_DESC" MaxLength="50" Width="300px" runat="server" ClientIDMode="Static" />
                                </td>
                            </tr>
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lbl_IS_VALID" Text="<%$Resources:Resource,wfb2db_lb_IS_VALID_qry%>" />
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="dll_IS_VALID" ClientIDMode="Static">
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>" Value="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>" />
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_IS_VALID_Y%>" Value="Y" />
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_IS_VALID_N%>" Value="N" />
                                    </asp:DropDownList>
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lb_WORK_DAY_CD" Text="<%$Resources:Resource,wfb2db_SHIFT_CD%>" />
                                </th>
                                <td>
                                    <asp:TextBox ID="txt_WORK_DAY_CD" MaxLength="2" Width="30" runat="server" ClientIDMode="Static" OnTextChanged="txt_WORK_DAY_CD_TextChanged" AutoPostBack="true" />
                                    <input id="btn_WORK_DAY_CD" type="button" value="..." onclick="OpenSearch('Shift_Search.aspx', 'txt_WORK_DAY_CD', 'txt_WORK_DAY_DESC', '');" />
                                    <asp:TextBox ID="txt_WORK_DAY_DESC" runat="server" ClientIDMode="Static" BorderWidth="0" />
                                </td>
                            </tr>
                             <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="Label1" Text="<%$Resources:Resource,wfb2db_lb_IS_IFLOW_SHOW%>" />
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddl_IS_IFLOW_SHOW" ClientIDMode="Static" Width="60px">
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>" Value="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>" />
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_IS_IFLOW_SHOW_Y%>" Value="Y" />
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_IS_IFLOW_SHOW_N%>" Value="N" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <th></th>
                                <th></th>
                                <td align="right" class="Body_label">
                                    <div id="init">
                                        <asp:RequiredFieldValidator ID="RegularExpressionValidator3" runat="server" InitialValue="null"
                                            ErrorMessage="" ControlToValidate="dll_CALENDAR_CD" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <aces:Btn ID="WFB2DB0100Search" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DB0100Search%>" OnClick="WFB2DB0100Search_Click" OnClientClick="CheckValid();" />
                                        <aces:Btn ID="WFB2DB0100SET" runat="server" Text="循環規則設定" OnClick="WFB2DB0100SET_Click" />
                                        <%--
                                            <asp:Button ID="WFB2DB0100SET" runat="server" Text="循環規則設定" OnClick="WFB2DB0100SET_Click" />
                                            <asp:Button ID="WFB2DB0100Search" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DB0100Search%>" OnClick="WFB2DB0100Search_Click" OnClientClick="CheckValid();" />
                                            --%>
                                        <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2db_Clear%>" OnClientClick="return ClearAll();" />
                                        <aces:Btn ID="WFB2DB0100Import" runat="server" Text="匯入" OnClick="WFB2DB0100Import_Click" />
                                        <aces:Btn ID="WFB2DB0100Export" runat="server" Text="匯出" OnClick="WFB2DB0100Export_Click" />
                                    
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr style="width: 1020px; text-align: left;" />
                    </td>
                </tr>
                <tr>
                    <td align="right" class="Body_label">
                        <div id="init_grid" style="margin-bottom: 7px;">
                            <aces:Btn ID="WFB2DB0100Add" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0100Add%>" OnClick="WFB2DB0100Add_Click" />
                            <aces:Btn ID="WFB2DB0100Delete" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0100Delete%>" Visible="false" OnClientClick="return CheckDelAction();" OnClick="WFB2DB0100Delete_Click" />
                            <aces:Btn ID="WFB2DB0100Edit" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0100Edit%>" Visible="false" OnClick="WFB2DB0100Edit_Click" OnClientClick="return CheckModeifyAction();" />
                            <aces:Btn ID="WFB2DB0100Dtl" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0100Dtl%>" Visible="false" OnClick="WFB2DB0100Dtl_Click" OnClientClick="return CheckDtlAction();" />
                            <aces:Btn ID="WFB2DB0100Copy" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0100Copy%>" Visible="false" OnClientClick="return CheckCopyAction();" OnClick="WFB2DB0100Copy_Click" />
                            <aces:Btn ID="WFB2DB0100Save" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0100Save%>" Visible="false" OnClick="WFB2DB0100Save_Click" OnClientClick="return CheckSaveAction();" />

                            <%--<asp:Button ID="WFB2DB0100Add" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0100Add%>" OnClick="WFB2DB0100Add_Click" />
                            <asp:Button ID="WFB2DB0100Delete" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0100Delete%>" Visible="false" OnClientClick="return CheckDelAction();" OnClick="WFB2DB0100Delete_Click" />
                            <asp:Button ID="WFB2DB0100Edit" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0100Edit%>" Visible="false" OnClick="WFB2DB0100Edit_Click" OnClientClick="return CheckModeifyAction();" />
                            <asp:Button ID="WFB2DB0100Dtl" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0100Dtl%>" Visible="false" OnClick="WFB2DB0100Dtl_Click" OnClientClick="return CheckDtlAction();" />
                            <asp:Button ID="WFB2DB0100Copy" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0100Copy%>" Visible="false" OnClientClick="return CheckCopyAction();" OnClick="WFB2DB0100Copy_Click" />
                            <asp:Button ID="WFB2DB0100Save" runat="server" Text="<%$Resources:Resource,wfb2db_WFB2DB0100Save%>" Visible="false" OnClick="WFB2DB0100Save_Click" OnClientClick="return CheckSaveAction();" />--%>
                            <asp:Button ID="WFB2DB0100Cancel" runat="server" Text="<%$Resources:Resource,wfb2db_Cancel%>" Visible="false" OnClick="WFB2DB0100Cancel_Click" OnClientClick="return CancelConfirm();" />
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getGridData"
                SelectCountMethod="GetGridDataCount" TypeName="WFB2DB0100BO" EnablePaging="True" SortParameterName="sortExpression"
                OnSelected="ods1_Selected" OnSelecting="obs1_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="dll_CALENDAR_CD"
                        Name="calendar_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_WORK_SHIFT_CD"
                        Name="WORK_SHIFT_CD" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_WORK_SHIFT_DESC"
                        Name="WORK_SHIFT_DESC" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="dll_IS_VALID"
                        Name="is_valid" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txt_WORK_DAY_CD"
                        Name="WORK_DAY_CD" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="ddl_IS_IFLOW_SHOW"
                        Name="is_iflow_show" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound" meta:resourcekey="gv_resultResource1">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_RowNumber%>" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="RowNumber" Text='<%#Bind("RowNumber")%>' runat="server" Width="97%" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="RowNumber_Edit" Text='<%#Bind("RowNumber")%>' runat="server" Width="97%" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- 行事曆 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_CALENDAR%>" SortExpression="CALENDAR" HeaderStyle-Width="190px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblCALENDAR" Text='<%#Bind("CALENDAR")%>' runat="server" Width="97%" />
                            <asp:HiddenField ID="hidCALENDAR_CD" Value='<%#Bind("CALENDAR_CD")%>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lblCALENDAR_Edit" Text='<%#Bind("CALENDAR")%>' runat="server" Width="97%" />
                                <asp:DropDownList runat="server" ID="dll_CALENDAR_Edit" ClientIDMode="Static" CssClass="MandatoryField" Style="display: none;" />
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList runat="server" ID="dll_CALENDAR_Edit" CssClass="MandatoryField" ClientIDMode="Static" />
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- 輪值表代碼 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfd2de_lb_WORK_SHIFT_CD%>" SortExpression="WORK_SHIFT_CD" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblWORK_SHIFT_CD" Text='<%#Bind("WORK_SHIFT_CD")%>' runat="server" Width="97%" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lblWORK_SHIFT_CD" ClientIDMode="Static" runat="server" Width="97%" Text='<%#Bind("WORK_SHIFT_CD")%>' />
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txtWORK_SHIFT_CD_Edit" ClientIDMode="Static" CssClass="MandatoryField" Text="" runat="server" MaxLength="2" Width="97%" />
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- 輪值表說明 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_WORK_SHIFT_DESC%>" SortExpression="WORK_SHIFT_DESC" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblWORK_SHIFT_DESC_Edit" Text='<%#Bind("WORK_SHIFT_DESC")%>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:TextBox ID="txtWORK_SHIFT_DESC_Edit" runat="server" CssClass="MandatoryField" Text='<%#Bind("WORK_SHIFT_DESC")%>' Width="97%" MaxLength="60"/>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txtWORK_SHIFT_DESC_Edit" Text="" CssClass="MandatoryField" runat="server" Width="97%" MaxLength="60"/>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- 使用中 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_IS_VALID%>" SortExpression="IS_VALID" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblIS_VALID" Text='<%#Bind("IS_VALID")%>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:DropDownList ID="dllIS_VALID_Edit" runat="server" CssClass="MandatoryField" Width="97%">
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>"
                                        Value="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>" />
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_IS_VALID_Y%>" Value="Y" />
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_IS_VALID_N%>" Value="N" />
                                </asp:DropDownList>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="dllIS_VALID_Edit" runat="server" CssClass="MandatoryField" Width="97%">
                                     <%-- 
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>"
                                        Value="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>" />
                                         --%> 
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_IS_VALID_Y%>" Value="Y" />
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_IS_VALID_N%>" Value="N" />
                                </asp:DropDownList>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <%-- IFLOW顯示 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_IS_IFLOW_SHOW%>" SortExpression="IS_IFLOW_SHOW" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_IFLOW_SHOW" Text='<%#Bind("IS_IFLOW_SHOW")%>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:DropDownList ID="ddl_IS_IFLOW_SHOW_Edit" runat="server" CssClass="MandatoryField" Width="97%">
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2db_IS_IFLOW_SHOW_Y%>" Value="Y" />
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2db_IS_IFLOW_SHOW_N%>" Value="N" />
                                </asp:DropDownList>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="ddl_IS_IFLOW_SHOW_Edit" runat="server" CssClass="MandatoryField" Width="97%">
                                     <%-- 
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>"
                                        Value="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>" />
                                          --%>
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2db_IS_IFLOW_SHOW_Y%>" Value="Y" />
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2db_IS_IFLOW_SHOW_N%>" Value="N" />
                                </asp:DropDownList>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                     <%-- 備註 --%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2db_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="340px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblREMARK" Text='<%#Bind("REMARK")%>' runat="server" Width="97%" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:TextBox ID="txtREMARK_Edit" runat="server" Text='<%#Bind("REMARK")%>' Width="97%" MaxLength="210"/>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txtREMARK_Edit" Text="" runat="server" Width="97%" MaxLength="210"/>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
                <EmptyDataTemplate>
                    <table class="grid-view" width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                        <tr class="header">
                            <td width="20px"></td>
                          
                            <td width="70px">
                                <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb2db_RowNumber%>" Width="97%"></asp:Label>
                            </td>
                             <%-- 行事曆 --%>
                            <td width="190px">
                                <asp:Label runat="server" ID="lblHeaderCALENDAR" Text="<%$Resources:Resource,wfb2db_lb_CALENDAR%>" Width="97%" />
                            </td>
                            <%-- 輪值表代碼 --%>
                            <td width="100px">
                                <asp:Label ID="lblHeaderWORK_SHIFT_CD" runat="server" Text="<%$Resources:Resource,wfd2de_lb_WORK_SHIFT_CD%>" Width="97%"></asp:Label>
                            </td>
                            <%-- 輪值表說明 --%>
                            <td width="200px">
                                <asp:Label ID="lblHeaderWORK_SHIFT_DESC" runat="server" Text="<%$Resources:Resource,wfb2db_lb_WORK_SHIFT_DESC%>" Width="97%"></asp:Label>
                            </td>
                            <%-- 使用中 --%>
                            <td width="50px">
                                <asp:Label ID="lblHeaderIS_VALID" runat="server" Text="<%$Resources:Resource,wfb2db_lb_IS_VALID%>" Width="97%"></asp:Label>
                            </td>
                            <%-- IFLOW顯示 --%>
                            <td width="50px">
                                <asp:Label ID="lb_IS_IFLOW_SHOW" runat="server" Text="<%$Resources:Resource,wfb2db_lb_IS_IFLOW_SHOW%>" Width="97%"></asp:Label>
                            </td>
                            <%-- 備註 --%>
                            <td width="340px">
                                <asp:Label ID="lblHeader_REMARK" runat="server" Text="<%$Resources:Resource,wfb2db_lb_REMARK%>" Width="97%"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:DropDownList runat="server" ID="dll_CALENDAR_Edit" CssClass="MandatoryField" ClientIDMode="Static" />
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txtWORK_SHIFT_CD_Edit" CssClass="MandatoryField" Text="" runat="server" Width="97%" MaxLength="2"/>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txtWORK_SHIFT_DESC_Edit" CssClass="MandatoryField" Text="" runat="server" Width="97%" MaxLength="60" />
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:DropDownList ID="dllIS_VALID_Edit" CssClass="MandatoryField" runat="server" Width="97%">
                                        <%-- 
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>"
                                            Value="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>" />
                                        --%>
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_IS_VALID_Y%>" Value="Y" />
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_IS_VALID_N%>" Value="N" />
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:DropDownList ID="ddl_IS_IFLOW_SHOW_Edit" CssClass="MandatoryField" runat="server" Width="97%">
                                        <%-- 
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>"
                                            Value="<%$Resources:Resource,wfb2db_dll_PlaceChoice%>" />
                                        --%> 
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_IS_IFLOW_SHOW_Y%>" Value="Y" />
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2db_IS_IFLOW_SHOW_N%>" Value="N" />
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txtREMARK_Edit" Text="" runat="server" Width="97%" MaxLength="210" />
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
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DB0100Search"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btn_clear"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2DB0100Add"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2DB0100Delete"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2DB0100Edit"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2DB0100Save"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2DB0100Cancel"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_Del_NotChoiceMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_Mod_NotChoiceMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_Copy_NotChoiceMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_Save_ConfirmMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_Del_ConfirmMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_txtWORK_SHIFT_CD_NotNull" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_Dtl_NotChoiceMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_Cancel_Confirm" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_dllIS_VALID_NotNull1" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_txtWORK_SHIFT_DESC_NotNull" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2db_dllCALENDARY_NotNull" />
    <asp:HiddenField ID="hid_wfd2da_Error_WORK_SHIFT_CD" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfd2da_Error_WORK_SHIFT_CD%>" />
    
</asp:Content>
