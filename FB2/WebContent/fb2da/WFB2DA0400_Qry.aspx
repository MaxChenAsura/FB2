<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2da/WFB2DA0400_Qry.aspx.cs" Inherits="WebContent_fb2da_WFB2DA0400_Qry" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $("#txt_YEAR").mask("9999");

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

        function ClearAll() {
            $('#txt_YEAR').val($('#hid_d_txt_year').val()).mask("9999");
            $('#ddl_CALENDAR_CD').val("-1");
            $('#txt_GROUP_CD').val("");
            return false;
        }

        
        function CheckYEAR(source, arguments) {
            var re = /^[0-9]+$/;
            if ($("#txt_YEAR").val().trim() != "") {
                if (!re.test($("#txt_YEAR").val()))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            }
            else
                arguments.IsValid = true;
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0) {
                return confirm($('#hid_Del_ConfirmMessage').val());
            }
            else {
                alert($('#hid_Del_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDelAllAction() {
            var processed = true;
            BlockUI();
            processed = confirm("確定要一括刪除?");
            if (!processed) {
                $.unblockUI();
            }
            return processed;
        }


        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1) {
                var processed = true;
                BlockUI();
                processed = confirm("確定要執行?");
                if (!processed) {
                    $.unblockUI();
                }

                return processed;
            }
            else {
                alert($('#hid_Mod_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hid_Dtl_NotChoiceMessage').val());
                return false;
            }
        }

        function CheckSaveAction() {
            if ($('#txtCALENDAR_CD_Edit').val() != undefined) {
                if ($('#txtCALENDAR_CD_Edit').val().trim() == "") {
                    alert($('#hid_txtCALENDAR_CD_NotNull').val());
                    return false;
                }
                else
                    return confirm($('#hid_Save_ConfirmMessage').val());
            }
            else
                return confirm($('#hid_Save_ConfirmMessage').val());
        }

        function LookUpCheckboxs() {
            var ItemCheckBoxs = $(":checkbox[id$=cb_check]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("cb_all") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        function MyCheckValid(msg) {
            var month = "";
            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                $.unblockUI();
            } else {
                processed = false;
                return processed;
            }

            BlockUI();
            processed = confirm("確定要進行" + msg +'?');
            if (!processed) {
                $.unblockUI();
            }

            return processed;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
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
                                    <%--年度--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_YEAR" runat="server" Text="<%$Resources:Resource,wfb2da_lb_YEAR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_YEAR" Width="80px" runat="server" CssClass="MandatoryField" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2da_YEAR%>"
                                            ControlToValidate="txt_YEAR" ForeColor="Red" ValidationGroup="GroupA" Display="None">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb2da_YEAR_isError%>" ClientValidationFunction="CheckYEAR" ForeColor="Red"
                                            ControlToValidate="txt_YEAR" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>

                                    </td>
                                </tr>
                                <tr>
                                    <%--行事曆代碼--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CALENDAR_CD" runat="server" Text="<%$Resources:Resource,wfb2da_lb_CALENDAR_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_CALENDAR_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%--群組代碼--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_GROUP_CD" runat="server" Text="<%$Resources:Resource,wfb2da_lb_GROUP_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_GROUP_CD" Width="80px" runat="server" ClientIDMode="Static" MaxLength="4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DA0400Search" runat="server" Text="查詢" OnClick="WFB2DA0400Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                            <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,btn_clear%>" OnClientClick="return ClearAll();" />
                                            <aces:Btn ID="WFB2DA0400Import" runat="server" Text="<%$Resources:Resource,Import%>" OnClick="WFB2DA0400Import_Click" />
                                            <aces:Btn ID="WFB2DA0400EXEC" runat="server" Text="執行"  OnClientClick="return MyCheckValid(this.value);" OnClick="WFB2DA0400EXEC_Click" />
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
                            <aces:Btn ID="WFB2DA0400Delete" runat="server" Text="刪除" OnClientClick="return CheckDelAction();" OnClick="WFB2DA0400Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2DA0400DeleteAll" runat="server" Text="一括刪除"   OnClientClick="return CheckDelAllAction();"  OnClick="WFB2DA0400DeleteAll_Click"  Visible="false" />
                        </div>
                    </td>
                </tr>
                <tr></tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="WFB2DA0400DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_YEAR" Name="year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_CALENDAR_CD" Name="calendar_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_GROUP_CD" Name="group_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--序號--%>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                    <%--行事曆代碼--%>
                    <asp:BoundField DataField="CALENDAR_CD" HeaderText="<%$Resources:Resource,wfb2da_lb_CALENDAR_CD%>" SortExpression="CALENDAR_CD" HeaderStyle-Width="290px" ItemStyle-Width="290px" ItemStyle-HorizontalAlign="Left" />
                    <%--群組代碼--%>
                    <asp:BoundField DataField="GROUP_CD" HeaderText="<%$Resources:Resource,wfb2da_lb_GROUP_CD%>" SortExpression="GROUP_CD" HeaderStyle-Width="290px" ItemStyle-Width="290px" ItemStyle-HorizontalAlign="Left" />
                    <%--日期起--%>
                    <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2da_lb_START_DT2%>" SortExpression="START_DT" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                    <%--日期迄--%>
                    <asp:BoundField DataField="END_DT" HeaderText="<%$Resources:Resource,wfb2da_lb_END_DT2%>" SortExpression="END_DT" HeaderStyle-Width="200px" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                </Columns>
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
            <asp:HiddenField ID="hid_d_txt_year" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Del_NotChoiceMessage" Value="<%$Resources:Resource,Add_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Mod_NotChoiceMessage" Value="<%$Resources:Resource,Edit_NotChoiceMessage%> " />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Save_ConfirmMessage" Value="<%$Resources:Resource,Save_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2da_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2_Mod_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hid_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb2da_Cancel_Confirm%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="btn_cancel" ShowSummary="false" />
            <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
        <%--EXCEL下載用 --%>
        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DA0400EXEC" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

