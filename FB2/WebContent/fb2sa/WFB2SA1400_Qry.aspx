<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sa/action/WFB2SA1400_Qry.aspx.cs" Inherits="WebContent_WFB2SA_WFB2SA1400_Qry" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".number").mask('9999');
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
                barcolor: "#7F7F7F",
                freezesize: 1

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        //清空畫面
        function ClearAll() {
            $("#ddl_APPROVE_STATUS").val("-1");
            $("#txt_DATA_YEAR").val("");
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return true;
            else {
                alert($('#hidwfb2sc_Mod_NotChoiceMessage').val());
                return false;
            }
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
                                <col width="12%" />
                                <col width="33%" />
                                <col width="15%" />
                                <col width="40%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DATA_YEAR" runat="server" Text="<%$Resources:Resource,wfb2sa_lb_DATA_YEAR%>"></asp:Label>:</th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DATA_YEAR" runat="server" MaxLength="4" Width="60px" TextMode="Number" CssClass="number" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server"
                                            ErrorMessage="年度必須為4碼!" ControlToValidate="txt_DATA_YEAR" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <th align="left"></th>
                                    <td></td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <th align="right">
                                        <aces:Btn ID="WFB2SA1400Search" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Search%>" OnClick="WFB2SA1400Search_Click" ValidationGroup="GroupA"  OnClientClick="CheckValid();" />
                                        <%--<asp:Button ID="WFB2SA1400Search" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Search%>" OnClick="WFB2SA1400Search_Click" ValidationGroup="GroupA" OnClientClick="CheckValid();" />--%>
                                        <asp:Button ID="btn_clear" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA2100Clear%>" OnClientClick="ClearAll();" CausesValidation="false" />
                                    </th>
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
                            <aces:Btn ID="WFB2SA1400Detail" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1400Detail%>" OnClick="WFB2SA1400Detail_Click" OnClientClick="return CheckDelAction();" Visible="false" />
                            <%--<asp:Button ID="WFB2SA1400Detail" runat="server" Text="<%$Resources:Resource,wfb2sa_WFB2SA1400Detail%>" OnClick="WFB2SA1400Detail_Click" OnClientClick="return CheckDelAction();" Visible="false" />--%>
                        </div>

                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SA1400DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DATA_YEAR"
                        Name="data_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="hid_USER_ID"
                        Name="user_id" PropertyName="Value" Type="String" ConvertEmptyStringToNull="false" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="true" AllowSorting="true" ClientIDMode="Static"
                AutoGenerateColumns="false" CssClass="grid-view" ShowFooter="false" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px" OnPageIndexChanging="gv_result_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2sa_RowNumber%>" HeaderStyle-Width="40px" />
                    <asp:BoundField DataField="DATA_YEAR" HeaderText="<%$Resources:Resource,wfb2sa_lb_DATA_YEAR%>" SortExpression="DATA_YEAR" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="PROCESS_STATUS_DESC" HeaderText="<%$Resources:Resource,wfb2sa_lb_PROCESS_STATUS%>" SortExpression="PROCESS_STATUS" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2sa_lb_START_DT%>" DataFormatString="{0:yyyy/MM/dd}" SortExpression="START_DT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="END_DT" HeaderText="<%$Resources:Resource,wfb2sa_lb_END_DT%>" DataFormatString="{0:yyyy/MM/dd}" SortExpression="END_DT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="RELEASE_BY_NAME" HeaderText="<%$Resources:Resource,wfb2sa_lb_RELEASE_BY%>" SortExpression="RELEASE_BY" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="RELEASE_DT" HeaderText="<%$Resources:Resource,wfb2sa_lb_RELEASE_DT%>" DataFormatString="{0:yyyy/MM/dd}" SortExpression="RELEASE_DT" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="APPROVE_STATUS_DESC" HeaderText="<%$Resources:Resource,wfb2sa_lb_APPROVE_STATUS%>" SortExpression="APPROVE_STATUS" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="APPROVE_BY_NAME" HeaderText="<%$Resources:Resource,wfb2sa_lb_APPROVE_BY%>" SortExpression="APPROVE_BY" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="APPROVE_DT" HeaderText="<%$Resources:Resource,wfb2sa_lb_APPROVE_DT%>" DataFormatString="{0:yyyy/MM/dd}" SortExpression="APPROVE_DT" HeaderStyle-Width="110px" ItemStyle-HorizontalAlign="Left" />
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_USER_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Mod_NotChoiceMessage%>"/>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
