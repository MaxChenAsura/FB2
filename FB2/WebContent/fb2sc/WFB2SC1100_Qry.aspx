<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2sc/WFB2SC1100_Qry.aspx.cs" Inherits="WebContent_fb2sc_WFB2SC1100_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });
        function iniForm() {
            $("#txt_SALARY_ID").mask('9999');
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
                headerrowcount: 1,
                freezesize: 4

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return true;
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
                alert($('#hidwfb2sc_Mod_NotChoiceMessage').val());
                return false;
            }
        }
        function LookUpCheckboxs() {
            var ItemCheckBoxs = $("[type=checkbox]");
            var HaveCheck = 0;
            for (var i = 0; i < ItemCheckBoxs.length; i++) {
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("_freezeitem") != -1 && ItemCheckBoxs[i].id.indexOf("_freezeheader") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        //清空畫面
        function ClearAll() {
            $("#txt_SALARY_ID").val("");
            $("#txt_SALARY_NAME").val("");
            $("#ddl_IS_DISABLE").val("");
            $("#ddl_SALARY_CD").val("");
            $("#ddl_IS_PLUS").val("");
            return false;
        }

        function DeleteConfirmAfter() {
            if (confirm($('#hidwfb2sc_Del_ConfirmMessage').val())) {
                $('#btn_confirmDel').click();
            }
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
                                <col width="20%" />
                                <col width="10%" />
                                <col width="25%" />
                                <col width="10%" />
                                <col width="25%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--薪資項目--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_ID" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_ID" runat="server" Width="60" MaxLength="4" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--項目名稱--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_NAME" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SALARY_NAME" runat="server" MaxLength="20" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--停用註記--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_DISABLE" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_DISABLE%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_IS_DISABLE" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Value="" Text="<%$Resources:Resource,wfb2sc_dll_IS_DISABLE_Place%>">
                                            </asp:ListItem>
                                            <asp:ListItem Value="Y" Text="<%$Resources:Resource,wfb2sc_dll_IS_DISABLE_Y%>"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="<%$Resources:Resource,wfb2sc_dll_IS_DISABLE_N%>"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <%--薪資項目類別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SALARY_CD" runat="server" Text="<%$Resources:Resource,wfb2sc_SALARY_CD_DESC%>"></asp:Label>
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SALARY_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <%--加減項--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IS_PLUS" runat="server" Text="<%$Resources:Resource,wfb2sc_IS_PLUS%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddl_IS_PLUS" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Value="" Text=""></asp:ListItem>
                                            <asp:ListItem Value="1" Text="<%$Resources:Resource,wfb2sc_lb_IS_PLUS_Add%>"></asp:ListItem>
                                            <asp:ListItem Value="-1" Text="<%$Resources:Resource,wfb2sc_lb_IS_PLUS_Plus%>"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th colspan="5"></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2SC1100Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Search%>" OnClick="WFB2SC1100Search_Click" OnClientClick="BlockUI();" ValidationGroup="GroupA" />
                                            <%--<asp:Button ID="WFB2SC1100Search" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Search%>" OnClick="WFB2SC1100Search_Click" OnClientClick="BlockUI();" ValidationGroup="GroupA" />--%>
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2sc_btn_clear%>" onclick="return ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <aces:Btn ID="WFB2SC1100Add" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC1100Add_Click" />
                            <aces:Btn ID="WFB2SC1100Delete" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SC1100Delete_Click" Visible="false" />
                            <aces:Btn ID="WFB2SC1100Edit" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2SC1100Edit_Click" Visible="false" />
                            <%--<asp:Button ID="WFB2SC1100Add" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Add%>" OnClick="WFB2SC1100Add_Click" />
                            <asp:Button ID="WFB2SC1100Delete" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2SC1100Delete_Click" Visible="false" />
                            <asp:Button ID="WFB2SC1100Edit" runat="server" Text="<%$Resources:Resource,wfb2sc_WFB2SC1100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2SC1100Edit_Click" Visible="false" />--%>
                        </div>

                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SC1100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_SALARY_ID"
                        Name="salary_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_SALARY_NAME"
                        Name="salary_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_IS_DISABLE"
                        Name="is_disable" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_SALARY_CD"
                        Name="salary_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_IS_PLUS"
                        Name="is_plus" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1410px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2sc_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                    <%--薪資項目--%>
                    <asp:BoundField DataField="SALARY_ID" HeaderText="<%$Resources:Resource,wfb2sc_SALARY_ID%>" SortExpression="SALARY_ID" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <%--項目名稱--%>
                    <asp:BoundField DataField="SALARY_NAME" HeaderText="<%$Resources:Resource,wfb2sc_SALARY_NAME%>" SortExpression="SALARY_NAME" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                    <%--薪資項目類別--%>
                    <asp:BoundField DataField="SALARY_CD_DESC" HeaderText="<%$Resources:Resource,wfb2sc_SALARY_CD_DESC%>" SortExpression="SALARY_CD" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                    <%--加減項--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2sc_IS_PLUS%>" SortExpression="IS_PLUS" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lb_IS_PLUS_TXT" runat="server" Text='<%#Bind("IS_PLUS")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--敍薪項目--%>
                    <asp:BoundField DataField="IS_SALARY" HeaderText="<%$Resources:Resource,wfb2sc_IS_SALARY%>" SortExpression="IS_SALARY" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <%--加班費計算--%>
                    <asp:BoundField DataField="IS_OVERTIME" HeaderText="<%$Resources:Resource,wfb2sc_IS_OVERTIME%>" SortExpression="IS_OVERTIME" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <%--請假計算--%>
                    <asp:BoundField DataField="IS_LEAVE" HeaderText="<%$Resources:Resource,wfb2sc_IS_LEAVE%>" SortExpression="IS_LEAVE" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <%--納入勞保--%>
                    <asp:BoundField DataField="INS_A" HeaderText="<%$Resources:Resource,wfb2sc_INS_A%>" SortExpression="INS_A" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <%--納入健保--%>
                    <asp:BoundField DataField="INS_B" HeaderText="<%$Resources:Resource,wfb2sc_INS_B%>" SortExpression="INS_B" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <%--納入勞退(新制)--%>
                    <asp:BoundField DataField="INS_C" HeaderText="<%$Resources:Resource,wfb2sc_INS_C%>" SortExpression="INS_C" HeaderStyle-Width="120px" ItemStyle-Width="120px" />
                    <%--納入法扣--%>
                    <asp:BoundField DataField="IS_ARREARS" HeaderText="<%$Resources:Resource,wfb2sc_IS_ARREARS%>" SortExpression="IS_ARREARS" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <%--補充保費--%>
                    <asp:BoundField DataField="INS_D" HeaderText="<%$Resources:Resource,wfb2sc_INS_D%>" SortExpression="INS_D" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <%--退休金計算--%>
                    <asp:BoundField DataField="IS_RETAIR" HeaderText="<%$Resources:Resource,wfb2sc_IS_RETAIR%>" SortExpression="IS_RETAIR" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <%--應免稅設定--%>
                    <asp:BoundField DataField="IS_TAX" HeaderText="<%$Resources:Resource,wfb2sc_IS_TAX%>" SortExpression="IS_TAX" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <%--破月計算--%>
                    <asp:BoundField DataField="IS_RATE" HeaderText="<%$Resources:Resource,wfb2sc_IS_RATE%>" SortExpression="IS_RATE" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <%--是否代扣--%>
                    <asp:BoundField DataField="IS_PREMINUS" HeaderText="<%$Resources:Resource,wfb2sc_IS_PREMINUS%>" SortExpression="IS_PREMINUS" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
                    <%--是否計算特休--%>
                    <asp:BoundField DataField="IS_PAY_LEAVE" HeaderText="<%$Resources:Resource,wfb2sc_IS_PAY_LEAVE%>" SortExpression="IS_PAY_LEAVE" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                    <%--是否計算加班--%>
                    <asp:BoundField DataField="IS_CAL_OVERTIME" HeaderText="<%$Resources:Resource,wfb2sc_IS_CAL_OVERTIME%>" SortExpression="IS_CAL_OVERTIME" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                    <%--排列順序--%>
                    <asp:BoundField DataField="ORDER_SEQ" HeaderText="<%$Resources:Resource,wfb2sc_ORDER_SEQ%>" SortExpression="ORDER_SEQ" HeaderStyle-Width="80px" ItemStyle-Width="80px" />
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
            <asp:Button ID="btn_confirmDel" runat="server" OnClick="btn_confirmDel_Click" ClientIDMode="Static" Style="display: none" />
            <asp:HiddenField ID="hid_deleteSalary_id" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb2sc_Mod_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2sc_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2sc_Del_ConfirmMessage%>" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


