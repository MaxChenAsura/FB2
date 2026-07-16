<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC2100_Detail2.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC2100_Detail2" %>
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

        function CheckLockAction() {
            if (LookUpCheckboxs() > 0) {
                BlockUI();
                var ItemCheckBoxs = $("[type=checkbox]");
                var Message = "";
                for (var i = 0; i < ItemCheckBoxs.length; i++) {
                    if (ItemCheckBoxs[i].id.search("cb_check") > -1 && ItemCheckBoxs[i].checked) {
                        var GridRow = ItemCheckBoxs[i].parentNode.parentNode.parentNode;
                        var RowIndex = GridRow.rowIndex;
                        var ItemSALARY_LOCKED = GridRow.querySelector('#lblSALARY_LOCKED');
                        var ItemOPERATION_NAME = GridRow.querySelector('#lblOPERATION_NAME');
                        var ItemPROC_SOUCE = GridRow.querySelector('#hidPROC_SOUCE');
                        if (ItemSALARY_LOCKED.innerText == 'Y' && ItemPROC_SOUCE.value == "1") {
                            if (Message == "")
                                Message = RowIndex.toString() + "." + ItemOPERATION_NAME.innerText + "薪資已鎖定,無法重複鎖定!";
                            else
                                Message += "\r\n" + RowIndex.toString() + "." + ItemOPERATION_NAME.innerText + "薪資已鎖定,無法重複鎖定!";
                        }
                        else {
                            var ItemPROCESS_DT = GridRow.querySelector('#hidPROCESS_DT');
                            if ((ItemPROCESS_DT.value == null || ItemPROCESS_DT.value == "") && ItemPROC_SOUCE.value == "1") {
                                if (Message == "")
                                    Message = RowIndex.toString() + "." + ItemOPERATION_NAME.innerText + "尚未執行月結,無法鎖定!";
                                else
                                    Message += "\r\n" + RowIndex.toString() + "." + ItemOPERATION_NAME.innerText + "尚未執行月結,無法鎖定!";
                            }
                        }
                    }
                }
                if (Message != "") {
                    alert(Message);
                    $.unblockUI();
                    return false;
                }
                else
                    return true;
            }
            else {
                BlockUI();
                alert($('#Hidwfb2sc_LOCK_Not_Choice').val());
                $.unblockUI();
                return false;
            }
        }
        function CheckUnLockAction() {
            if (LookUpCheckboxs() > 0) {
                BlockUI();
                var ItemCheckBoxs = $("[type=checkbox]");
                var Message = "";
                for (var i = 0; i < ItemCheckBoxs.length; i++) {
                    if (ItemCheckBoxs[i].id.search("cb_check") > -1 && ItemCheckBoxs[i].checked) {
                        var GridRow = ItemCheckBoxs[i].parentNode.parentNode.parentNode;
                        var RowIndex = GridRow.rowIndex;
                        var ItemSALARY_LOCKED = GridRow.querySelector('#lblSALARY_LOCKED');
                        var ItemOPERATION_NAME = GridRow.querySelector('#lblOPERATION_NAME');
                        var ItemPROC_SOUCE = GridRow.querySelector('#hidPROC_SOUCE');
                        if (ItemSALARY_LOCKED.innerText != 'Y' && ItemPROC_SOUCE.value == "1") {
                            if (Message == "")
                                Message = RowIndex.toString() + "." + ItemOPERATION_NAME.innerText + "薪資未鎖定,無法取消鎖定!";
                            else
                                Message += "\r\n" + RowIndex.toString() + "." + ItemOPERATION_NAME.innerText + "薪資未鎖定,無法取消鎖定!";
                        }
                        else {
                            var ItemPROCESS_DT = GridRow.querySelector('#hidPROCESS_DT');
                            if (ItemPROCESS_DT.value == null || ItemPROCESS_DT.value == "") {
                                if (Message == "")
                                    Message = RowIndex.toString() + "." + ItemOPERATION_NAME.innerText + "尚未執行月結,無法取消鎖定!";
                                else
                                    Message += "\r\n" + RowIndex.toString() + "." + ItemOPERATION_NAME.innerText + "尚未執行月結,無法取消鎖定!";
                            }
                        }
                    }
                }
                if (Message != "") {
                    alert(Message);
                    $.unblockUI();
                    return false;
                }
                else
                    return true;
            }
            else {
                BlockUI();
                alert($('#Hidwfb2sc_UnLOCK_Not_Choice').val());
                $.unblockUI();
                return false;
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
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020px" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr height="100%" valign="top">
                    <td>
                        <table width="100%">
                            <tr>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lbl_SALARY_DT" Text="<%$Resources:Resource,wfb2sc_SALARY_DT%>" />
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lbl_SALARY_DT_Value" Text="" />
                                    <asp:HiddenField runat="server" ID="hid_SALARY_DT_Value" />
                                </td>
                                <th align="left" class="Body_TableHeader">
                                    <asp:Label runat="server" ID="lbl_SALARY_YM" Text="<%$Resources:Resource,wfb2sc_SALARY_YM%>" />
                                       <asp:HiddenField runat="server" ID="hid_SALARY_YM" />
                                </th>
                                <td>
                                    <asp:Label runat="server" ID="lbl_SALARY_YM_Value" Text="" />
                                    <asp:HiddenField runat="server" ID="hid_SALARY_TYPE" />
                                </td>
                            </tr>
                            <tr>
                                <th></th>
                                <th></th>
                                <th></th>
                                <td align="right" class="Body_label">
                                    <div id="init">
                                        <aces:Btn ID="WFB2SC2100Lock" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_Lock%>" OnClick="WFB2SC2100Lock_Click" OnClientClick="return CheckLockAction();" />
                                        <aces:Btn ID="WFB2SC2100Unlock" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_UnLock%>" OnClick="WFB2SC2100Unlock_Click" OnClientClick="return CheckUnLockAction();" />
<%--                                        <asp:Button ID="WFB2SC2100Lock" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_Lock%>" OnClick="WFB2SC2100Lock_Click" OnClientClick="return CheckLockAction();" />
                                        <asp:Button ID="WFB2SC2100Unlock" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_UnLock%>" OnClick="WFB2SC2100Unlock_Click" OnClientClick="return CheckUnLockAction();" />--%>
                                        <asp:Button ID="btn_Back" runat="server" Text="<%$Resources:Resource,wfb2sc_Back%>" ClientIDMode="Static" OnClick="btn_Back_Click" />
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
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="GetDateil2GridData"
                SelectCountMethod="GetDateil2GridDataCount" TypeName="WFB2SC2100BO" EnablePaging="True" SortParameterName="sortExpression"
                OnSelected="ods1_Selected" OnSelecting="obs1_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="lbl_SALARY_DT_Value"
                        Name="SALARY_DT" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_SALARY_YM"
                        Name="SALARY_YM" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_SALARY_TYPE"
                        Name="SALARY_TYPE" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
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
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_OPERATION_ID%>" SortExpression="OPERATION_ID" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblOPERATION_ID" Text='<%#Bind("OPERATION_ID")%>' runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_OPERATION_NAME%>" SortExpression="OPERATION_NAME" HeaderStyle-Width="280px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblOPERATION_NAME" Text='<%#Bind("OPERATION_NAME")%>' runat="server" Width="97%" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_END_DT%>" SortExpression="PROCESS_DT" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblPROCESS_DT" Text='<%#Bind("PROCESS_DT","{0:yyyy/MM/dd HH:mm}")%>' runat="server" ClientIDMode="Static"  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_END_DT%>" SortExpression="END_DT" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblEND_DT" runat="server" ClientIDMode="Static"  />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_SALARY_LOCKED%>" SortExpression="SALARY_LOCKED" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblSALARY_LOCKED" Text='<%#Bind("SALARY_LOCKED")%>' runat="server" ClientIDMode="Static" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_START_END_DT%>" SortExpression="START_DT" HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblSTART_END_DT" runat="server" Width="97%" />
                            <asp:HiddenField ID="hidPROCESS_DT" Value='<%#Bind("PROCESS_DT")%>' runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hidPROC_SOUCE" Value='<%#Bind("PROC_SOUCE")%>' runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hidSALARY_REQ" Value='<%#Bind("SALARY_REQ")%>' runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hidSALARY_DT" Value='<%#Bind("SALARY_DT")%>' runat="server" ClientIDMode="Static" />
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
                            <asp:ListItem Text="每頁10000筆" Value="10000"></asp:ListItem>
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
            <asp:PostBackTrigger ControlID="WFB2SC2100Lock"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="WFB2SC2100Unlock"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField ID="Hidwfb2sc_LOCK_Not_Choice" Value="<%$Resources:Resource,wfb2sc_LOCK_Not_Choice%>" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="Hidwfb2sc_UnLOCK_Not_Choice" Value="<%$Resources:Resource,wfb2sc_UnLOCK_Not_Choice%>" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hid_process_status" runat="server" ClientIDMode="Static" />
</asp:Content>
