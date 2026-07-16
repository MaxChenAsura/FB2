<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0210_Add.aspx.cs" Inherits="WebContent_fb2ha_WFB2HA0210_Add" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".ymd").mask("9999/99/99");
            $('#txt_UP_DEPT_NAME').attr("readonly", true);            

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


        //清空畫面
        function ClearAll() {
            $("#txt_DEPT_NO").val("");
            $("#ddl_DEPT_LEVEL").val("-1");
            $("#txt_START_DT").val("");

        }


        function CheckSearch() {


            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else {
                $.unblockUI();
                return;
            }
        }

        //儲存前檢查
        function saveCheck() {

            var processed = true;
            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellspacing="1" cellpadding="1" width="1020" border="0"
                class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="18%" />
                    <col width="10%" />
                    <col width="18%" />
                    <col width="10%" />
                    <col width="34%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_UP_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_UP_DEPT_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="5">
                            <asp:TextBox ID="txt_UP_DEPT_NO" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <asp:TextBox ID="txt_UP_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                            <asp:HiddenField ID="hid_UP_DEPT_LEVEL" runat="server" />
                        </td>

                    </tr>
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr>
                        </td>
                    </tr>

                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT_LEVEL" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_LEVEL%>"></asp:Label>:</th>
                        <td style="text-align: left">
                            <asp:DropDownList ID="ddl_DEPT_LEVEL" runat="server" ClientIDMode="Static"></asp:DropDownList>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_DEPT_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_Edit_START_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_START_DT" runat="server" Width="81px" MaxLength="10" ClientIDMode="Static" CssClass="ymd MandatoryField date"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="ValidStartDt" runat="server" ErrorMessage="<%$Resources:Resource,wfb2ha_Required_START_DT%>"
                                ControlToValidate="txt_START_DT" ForeColor="Red" ValidationGroup="GroupA" Display="None"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cv_START_SDT" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2ha_ERR_START_DT_S%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>                            
                        </td>
                    </tr>

                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                <aces:Btn ID="WFB2HA0211Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Search%>" OnClick="WFB2HA0211Search_Click" OnClientClick="return CheckSearch();"/>
                                 <aces:Btn ID="WFB2HA0103Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Search%>" OnClick="WFB2HA0211Search_Click" OnClientClick="return CheckSearch();"/>
                                 <aces:Btn ID="WFB2HA0203Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Search%>" OnClick="WFB2HA0211Search_Click" OnClientClick="return CheckSearch();"/>
                                <%--<asp:Button ID="WFB2HA0211Search" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Search%>" OnClick="WFB2HA0211Search_Click" OnClientClick="return CheckSearch();" />--%>

                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2ha_btn_clear%>" onclick="ClearAll();" />
                            </div>
                        </td>
                    </tr>


                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr>
                        </td>
                    </tr>

                    <tr>
                        <td align="right" colspan="10">
                            <div id="init_grid">
                                <aces:Btn ID="WFB2HA0211Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Save%>" Visible="false" OnClick="WFB2HA0211Save_Click" />
                                 <aces:Btn ID="WFB2HA0103Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Save%>" Visible="false" OnClick="WFB2HA0211Save_Click" />
                                 <aces:Btn ID="WFB2HA0203Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Save%>" Visible="false" OnClick="WFB2HA0211Save_Click" />
                                <%--<asp:Button ID="WFB2HA0211Save" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Save%>" Visible="false" OnClick="WFB2HA0211Save_Click" />--%>
                                <asp:Button ID="WFB2HA0210Cancel" runat="server" Text="<%$Resources:Resource,wfb2ha_WFB2HA0210Cancel%>" Visible="true" OnClick="WFB2HA0210Cancel_Click" OnClientClick="return confirm($('#hidwfb299_Cancel_ConfirmMessage').val())" />
                            </div>

                        </td>
                    </tr>
                    <!-- END: Create a line -->
                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getAddData"
                SelectCountMethod="getAddCount" TypeName="CFB2HA0210DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="dept_no" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="ddl_DEPT_LEVEL" DefaultValue=""
                        Name="dept_level" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_START_DT" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="start_dt" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2ha_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="40px" />
                    <asp:BoundField DataField="DEPT_NO" HeaderText="<%$Resources:Resource,wfb2ha_lb_DEPT_NO%>" SortExpression="DEPT_NO" HeaderStyle-Width="460px" ItemStyle-Width="460px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="DEPT_NAME" HeaderText="<%$Resources:Resource,wfb2ha_DEPT_NAME%>" SortExpression="DEPT_NAME" HeaderStyle-Width="500px" ItemStyle-Width="500px" ItemStyle-HorizontalAlign="Left" />

                </Columns>

                <HeaderStyle CssClass="GridviewScrollHeader" />
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle HorizontalAlign="Center" />
                <EditRowStyle HorizontalAlign="Center" />
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
            <asp:HiddenField ID="HID_parentFuncID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
