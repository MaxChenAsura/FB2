<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2da/WFB2DA0100_Qry.aspx.cs" Inherits="WebContent_fb2da_WFB2DA0100Qry" Culture="auto" UICulture="auto" %>
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
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }

        function CancelConfirm() {
            if (confirm($('#hidwfb2da_Cancel_Confirm').val())) {
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
                if (confirm($('#hidwfb2da_Del_ConfirmMessage').val())) {
                    BlockUI();
                    return;
                }
                else {
                    $.unblockUI();
                    return false;
                }
            }
            else {
                alert($('#hidwfb2da_Del_NotChoiceMessage').val());
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
                alert($('#hidwfb2da_Mod_NotChoiceMessage').val());
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
                alert($('#hidwfb2da_Dtl_NotChoiceMessage').val());
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
                alert($('#hidwfb2da_Copy_NotChoiceMessage').val());
                $.unblockUI();
                return false;
            }
        }

        function CheckSaveAction() {
            var Message = "";
            var re = /^[0-9a-zA-Z_]+$/;
            if (!re.test($("#txtCALENDAR_CD_Edit").val())) {
                Message = $('#hid_wfd2da_Error_CALENDAR_CD').val(); //"行事曆代碼只能輸入英數字"
            }
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    if (Message == "")
                        Message = $('#hidwfb2da_txtCALENDAR_CD_NotNull').val();
                    else
                        Message += "\r\n" + $('#hidwfb2da_txtCALENDAR_CD_NotNull').val();
                }

            }
            if ($('#txtCALENDAR_DESC_Edit').val().trim() == "") {
                if (Message == "")
                    Message = $('#hidwfb2da_txtCALENDAR_DESC_NotNull').val();
                else
                    Message += "\r\n" + $('#hidwfb2da_txtCALENDAR_DESC_NotNull').val();
            }
            if ($('#dllIS_VALID_Edit option:selected').text().trim() == "") {
                if (Message == "")
                    Message = $('#hidwfb2da_dllIS_VALID_NotNull1').val();
                else
                    Message += "\r\n" + $('#hidwfb2da_dllIS_VALID_NotNull1').val();
            }

            if (Message != "") {
                BlockUI();
                alert(Message);
                $.unblockUI();
                return false;
            }
            else {
                BlockUI();
                if (confirm($('#hidwfb2da_Save_ConfirmMessage').val())) {
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
            $('#dll_IS_VALID').val("");
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
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lbl_CALENDAR_CD" Text="<%$Resources:Resource,wfb2da_lb_CALENDAR_qry%>" />
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="dll_CALENDAR_CD" ClientIDMode="Static" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lbl_IS_VALID" Text="<%$Resources:Resource,wfb2da_lb_IS_VALID_qry%>" />
                                </th>
                                <td>
                                    <asp:DropDownList runat="server" ID="dll_IS_VALID" ClientIDMode="Static">
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2da_dll_PlaceChoice%>" Value="<%$Resources:Resource,wfb2da_dll_PlaceChoice%>" />
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2da_dll_IS_VALID_Y%>" Value="Y" />
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2da_dll_IS_VALID_N%>" Value="N" />
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
                                        <aces:Btn ID="WFB2DA0100Search" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Search%>" OnClick="WFB2DA0100Search_Click" OnClientClick="CheckValid();" />

                                        <%--<asp:Button ID="WFB2DA0100Search" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Search%>" OnClick="WFB2DA0100Search_Click" OnClientClick="CheckValid();" />--%>
                                        <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2da_Clear%>" OnClientClick="return ClearAll();" />
                                        <aces:Btn ID="WFB2DA0100Import" runat="server" Text="<%$Resources:Resource,Import%>" OnClick="WFB2DA0100Import_Click"/>                                                 
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
                            <aces:Btn ID="WFB2DA0100Add" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Add%>" OnClick="WFB2DA0100Add_Click" />
                            <aces:Btn ID="WFB2DA0100Delete" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Delete%>" Visible="false" OnClientClick="return CheckDelAction();" OnClick="WFB2DA0100Delete_Click" />
                            <aces:Btn ID="WFB2DA0100Edit" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Edit%>" Visible="false" OnClick="WFB2DA0100Edit_Click" OnClientClick="return CheckModeifyAction();" />
                            <aces:Btn ID="WFB2DA0100Dtl" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Dtl%>" Visible="false" OnClick="WFB2DA0100Dtl_Click" OnClientClick="return CheckDtlAction();" />
                            
                            <aces:Btn ID="WFB2DA0100Default" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Default%>" Visible="false" OnClick="WFB2DA0100Default_Click" />
                            <aces:Btn ID="WFB2DA0100Copy" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Copy%>" Visible="false" OnClientClick="return CheckCopyAction();" OnClick="WFB2DA0100Copy_Click" />
                            <aces:Btn ID="WFB2DA0100Save" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Save%>" Visible="false" OnClick="WFB2DA0100Save_Click" OnClientClick="return CheckSaveAction();" />

                            <%--<asp:Button ID="WFB2DA0100Add" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Add%>" OnClick="WFB2DA0100Add_Click" />
                            <asp:Button ID="WFB2DA0100Delete" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Delete%>" Visible="false" OnClientClick="return CheckDelAction();" OnClick="WFB2DA0100Delete_Click" />
                            <asp:Button ID="WFB2DA0100Edit" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Edit%>" Visible="false" OnClick="WFB2DA0100Edit_Click" OnClientClick="return CheckModeifyAction();" />
                            <asp:Button ID="WFB2DA0100Dtl" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Dtl%>" Visible="false" OnClick="WFB2DA0100Dtl_Click" OnClientClick="return CheckDtlAction();" />
                            <asp:Button ID="WFB2DA0100Copy" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Copy%>" Visible="false" OnClientClick="return CheckCopyAction();" OnClick="WFB2DA0100Copy_Click" />
                            <asp:Button ID="WFB2DA0100Save" runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Save%>" Visible="false" OnClick="WFB2DA0100Save_Click" OnClientClick="return CheckSaveAction();" />--%>
                            <asp:Button ID="WFB2DA0100Cancel" runat="server" Text="<%$Resources:Resource,wfb2da_Cancel%>" Visible="false" OnClick="WFB2DA0100Cancel_Click" OnClientClick="return CancelConfirm();" />
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getGridData"
                SelectCountMethod="GetGridDataCount" TypeName="WFB2DA0100BO" EnablePaging="True" SortParameterName="sortExpression"
                OnSelected="ods1_Selected" OnSelecting="obs1_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="dll_CALENDAR_CD"
                        Name="calendar_cd" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="dll_IS_VALID"
                        Name="is_valid" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="false" />
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
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2da_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="70px">
                        <ItemTemplate>
                            <asp:Label ID="RowNumber" Text='<%#Bind("RowNumber")%>' runat="server" Width="97%" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="RowNumber_Edit" Text='<%#Bind("RowNumber")%>' runat="server" Width="97%" />
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2da_lb_CALENDAR_CD%>" SortExpression="CALENDAR_CD" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblCALENDAR_CD" Text='<%#Bind("CALENDAR_CD")%>' runat="server" Width="97%" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:Label ID="lblCALENDAR_CD_Edit" runat="server" Text='<%#Bind("CALENDAR_CD")%>' Width="97%" />
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txtCALENDAR_CD_Edit" ClientIDMode="Static" CssClass="MandatoryField" Text="" runat="server" MaxLength="1" Width="97%" />
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2da_lb_CALENDAR_DESC%>" SortExpression="CALENDAR_DESC" HeaderStyle-Width="350px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblCALENDAR_DESC" Text='<%#Bind("CALENDAR_DESC")%>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:TextBox ID="txtCALENDAR_DESC_Edit" runat="server" CssClass="MandatoryField" Text='<%#Bind("CALENDAR_DESC")%>' MaxLength="60" Width="97%" />
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txtCALENDAR_DESC_Edit" Text="" CssClass="MandatoryField" runat="server" MaxLength="60"  Width="97%" />
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2da_lb_IS_VALID%>" SortExpression="IS_VALID" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblIS_VALID" Text='<%#Bind("IS_VALID")%>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:DropDownList ID="dllIS_VALID_Edit" runat="server" CssClass="MandatoryField" Width="97%">
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2da_dll_PlaceChoice%>"
                                        Value="<%$Resources:Resource,wfb2da_dll_PlaceChoice%>" />
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2da_dll_IS_VALID_Y%>" Value="Y" />
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2da_dll_IS_VALID_N%>" Value="N" />
                                </asp:DropDownList>
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:DropDownList ID="dllIS_VALID_Edit" runat="server" CssClass="MandatoryField" Width="97%">
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2da_dll_PlaceChoice%>"
                                        Value="<%$Resources:Resource,wfb2da_dll_PlaceChoice%>" />
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2da_dll_IS_VALID_Y%>" Value="Y" />
                                    <asp:ListItem Text="<%$Resources:Resource,wfb2da_dll_IS_VALID_N%>" Value="N" />
                                </asp:DropDownList>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2da_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="440px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblREMARK" Text='<%#Bind("REMARK")%>' runat="server" Width="97%" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <div style="text-align: center; width: 100%;">
                                <asp:TextBox ID="txtREMARK_Edit" runat="server" Text='<%#Bind("REMARK")%>' MaxLength="210"  Width="97%" />
                            </div>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:TextBox ID="txtREMARK_Edit" Text="" runat="server" MaxLength="210" Width="97%" />
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
                                <asp:Label ID="lblHeaderRowNumber" runat="server" Text="<%$Resources:Resource,wfb2da_RowNumber%>" Width="97%"></asp:Label>
                            </td>
                            <td width="90px">
                                <asp:Label ID="lblHeaderCALENDAR_CD" runat="server" Text="<%$Resources:Resource,wfb2da_lb_CALENDAR_CD%>" Width="97%"></asp:Label>
                            </td>
                            <td width="350px">
                                <asp:Label ID="lblHeaderCALENDAR_DESC" runat="server" Text="<%$Resources:Resource,wfb2da_lb_CALENDAR_DESC%>" Width="97%"></asp:Label>
                            </td>
                            <td width="50px">
                                <asp:Label ID="lblHeaderIS_VALID" runat="server" Text="<%$Resources:Resource,wfb2da_lb_IS_VALID%>" Width="97%"></asp:Label>
                            </td>
                            <td width="440px">
                                <asp:Label ID="lblHeader_REMARK" runat="server" Text="<%$Resources:Resource,wfb2da_lb_REMARK%>" Width="97%"></asp:Label>
                            </td>
                        </tr>
                        <tr class="normal">
                            <td></td>
                            <td></td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txtCALENDAR_CD_Edit" ClientIDMode="Static" CssClass="MandatoryField" Text="" runat="server" MaxLength="1" Width="97%" />
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txtCALENDAR_DESC_Edit" CssClass="MandatoryField" Text="" runat="server" Width="97%" />
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:DropDownList ID="dllIS_VALID_Edit" CssClass="MandatoryField" runat="server" Width="97%">
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2da_dll_PlaceChoice%>"
                                            Value="<%$Resources:Resource,wfb2da_dll_PlaceChoice%>" />
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2da_dll_IS_VALID_Y%>" Value="Y" />
                                        <asp:ListItem Text="<%$Resources:Resource,wfb2da_dll_IS_VALID_N%>" Value="N" />
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: center; width: 100%">
                                    <asp:TextBox ID="txtREMARK_Edit" Text="" runat="server" Width="97%" />
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
            <asp:PostBackTrigger ControlID="WFB2DA0100Search"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="btn_clear"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2DA0100Add"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2DA0100Delete"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2DA0100Edit"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2DA0100Save"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2DA0100Cancel"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Del_NotChoiceMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Mod_NotChoiceMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Copy_NotChoiceMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Save_ConfirmMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Del_ConfirmMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_txtCALENDAR_CD_NotNull" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Dtl_NotChoiceMessage" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_Cancel_Confirm" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_dllIS_VALID_NotNull1" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2da_txtCALENDAR_DESC_NotNull" />
    <asp:HiddenField ID="hid_wfd2da_Error_CALENDAR_CD" runat="server" ClientIDMode="Static" Value="<%$Resources:Resource,wfd2da_Error_CALENDAR_CD%>" />
</asp:Content>
