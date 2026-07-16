<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb299/WFB2990300_Qry.aspx.cs" Inherits="WebContent_fb299_WFB2990300_Qry" %>

<%@ Register Src="~/UserControl/UCDateTimeRange.ascx" TagPrefix="uc1" TagName="UCDateTimeRange" %>
<%@ Register Src="~/UserControl/UCCommCodeDropDwonList.ascx" TagPrefix="uc1" TagName="UCCommCodeDropDwonList" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
        });

        function iniForm() {
            $('.date').datepicker({ dateFormat: 'yy/mm/dd' });
            $('#txt_UPDATED_DT_DATE_S').mask('9999/99/99');
            $('#txt_UPDATED_DT_DATE_E').mask('9999/99/99');
            gridviewScroll();
            $.unblockUI();
            $('#txt_SYS_FUN_NAME').attr("readonly", true);
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
        function checkSYS_kind(source, arguments) {
            var checksys_kind = $('#ddl_SYS_kind').val();
            if (checksys_kind == "" || checksys_kind == null)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        function checkSYS_FUN(source, arguments) {
            var checksys_fun = $('#txt_SYS_FUN').val();
            if (checksys_fun == "" || checksys_fun == null)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        function cheakTime(source, arguments) {
            //後端抓不到時間control,在前端塞給hiddenValue
            $('#hid_hour_s').val($('#ddl_UPDATED_DT_HOUR_S').val());
            $('#hid_min_s').val($('#ddl_UPDATED_DT_MIN_S').val());
            $('#hid_hour_e').val($('#ddl_UPDATED_DT_HOUR_E').val());
            $('#hid_min_e').val($('#ddl_UPDATED_DT_MIN_E').val());

            var timeS = $('#txt_UPDATED_DT_DATE_S').val() + $('#ddl_UPDATED_DT_HOUR_S').val() + $('#ddl_UPDATED_DT_MIN_S').val();
            var timeE = $('#txt_UPDATED_DT_DATE_E').val() + "" + $('#ddl_UPDATED_DT_HOUR_E').val() + $('#ddl_UPDATED_DT_MIN_E').val();
            timeS.replace(/\//g, "");
            timeE.replace(/\//g, "");
            if (timeS > timeE)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;

        }
        function ClearAll() {
            $('#ddl_SYS_kind').val("");
            $('#txt_TABLE_NAME').val("");
            $('#txt_SYS_FUN').val("");
            $('#ddl_CATEGORY_ITEM').val("");
            $('#txt_UPDATED_BY').val("");
            $('#txt_UPDATED_DT_DATE_S').val("");
            $('#txt_UPDATED_DT_DATE_E').val("");
            $('#txt_EDIT_INFOR').val("");
            $('#txt_SYS_FUN_NAME').val("");
            return false;
        }
        function OpenTABLE_NAME_Search() {
            var selectSYS_kind = "SYS_kind=" + $("#ddl_SYS_kind").val();
            var json = OpenSearch('Table_Name_Search.aspx', 'txt_TABLE_NAME', '', selectSYS_kind);
        }
        function OpenSYS_FUN_Search() {
            var selectSYS_kind = "SYS_kind=" + $("#ddl_SYS_kind").val();
            var json = OpenSearch('Sys_Function_Search.aspx', 'txt_SYS_FUN', 'txt_SYS_FUN_NAME', selectSYS_kind);
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
                                <col width="35%" />
                                <col width="10%" />
                                <col width="45%" />

                            </colgroup>
                            <tbody>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SYS_kind" runat="server" Text="<%$Resources:Resource,wfb299_lb_SYS_kind%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_SYS_kind" runat="server" ClientIDMode="Static" CssClass="MandatoryField"></asp:DropDownList>
                                    </td>
                                    <th></th>
                                    <td></td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_TABLE_NAME" runat="server" Text="<%$Resources:Resource,wfb299_lb_TB_NAME_DESC%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_TABLE_NAME" runat="server" Width="250px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_TABLE_NAME" type="button" value="..." onclick="OpenTABLE_NAME_Search();" />
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SYS_FUN" runat="server" Text="<%$Resources:Resource,wfb299_lb_SYS_FUN%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_SYS_FUN" runat="server" Width="100px" ClientIDMode="Static" CssClass="MandatoryField"></asp:TextBox>
                                        <input id="bt_SYS_FUN" type="button" value="..." onclick="OpenSYS_FUN_Search();" />
                                        <asp:TextBox ID="txt_SYS_FUN_NAME" runat="server" BorderWidth="0" Width="200px" ClientIDMode="Static"></asp:TextBox>
                                        <asp:CustomValidator ID="CustomValidator2" runat="server"
                                            ErrorMessage="系統功能必須選擇" ClientValidationFunction="checkSYS_FUN" ForeColor="Red"
                                            ControlToValidate="ddl_UPDATED_DT_MIN_E" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_CATEGORY_ITEM" runat="server" Text="<%$Resources:Resource,wfb299_lb_CATEGORY_ITEM_Qry%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_CATEGORY_ITEM" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <th></th>
                                    <td></td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_UPDATED_BY" runat="server" Text="<%$Resources:Resource,wfb299_lb_UPDATED_BY_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_UPDATED_BY" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_UPDATED_DT" runat="server" Text="<%$Resources:Resource,wfb299_lb_UPDATED_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_UPDATED_DT_DATE_S" runat="server" ClientIDMode="Static" Width="81px" CssClass="date MandatoryField"></asp:TextBox>
                                        <asp:DropDownList ID="ddl_UPDATED_DT_HOUR_S" runat="server" ClientIDMode="Static"></asp:DropDownList>:
                                        <asp:DropDownList ID="ddl_UPDATED_DT_MIN_S" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                        ~ 
                                        <asp:TextBox ID="txt_UPDATED_DT_DATE_E" runat="server" ClientIDMode="Static" Width="81px" CssClass="date MandatoryField"></asp:TextBox>
                                        <asp:DropDownList ID="ddl_UPDATED_DT_HOUR_E" runat="server" ClientIDMode="Static"></asp:DropDownList>:
                                        <asp:DropDownList ID="ddl_UPDATED_DT_MIN_E" runat="server" ClientIDMode="Static"></asp:DropDownList>

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                            ControlToValidate="txt_UPDATED_DT_DATE_S" ForeColor="Red"
                                            Display="None" ValidationGroup="GroupA" ErrorMessage="<%$Resources:Resource,wfb299_lb_UPDATED_SDT_isNull%>">
                                        </asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                            ControlToValidate="txt_UPDATED_DT_DATE_E" ForeColor="Red"
                                            Display="None" ValidationGroup="GroupA" ErrorMessage="<%$Resources:Resource,wfb299_lb_UPDATED_EDT_isNull%>">
                                        </asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="qrytest1" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb299_lb_UPDATED_SDT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_UPDATED_DT_DATE_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="qrytest2" runat="server" ValidateEmptyText="true"
                                            ErrorMessage="<%$Resources:Resource,wfb299_lb_UPDATED_EDT_isError%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                            ControlToValidate="txt_UPDATED_DT_DATE_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb299_lb_UPDATED_SDT_EDT%>" ClientValidationFunction="cheakTime" ForeColor="Red"
                                            ControlToValidate="ddl_UPDATED_DT_MIN_E" ValidationGroup="GroupA" Display="None">
                                        </asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EDIT_INFOR" runat="server" Text="<%$Resources:Resource,wfb299_lb_EDIT_INFOR_Qry%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EDIT_INFOR" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <th></th>
                                    <td></td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <%--<asp:Button ID="WFB2990300Search" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990300Search%>" OnClick="WFB2990300Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                                            <aces:Btn ID="WFB2990300Search" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990300Search%>" OnClick="WFB2990300Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                            <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,wfb299_WFB2990300Clear%>" OnClientClick="return ClearAll();" />
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
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2990300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="hid_isSuper"
                        Name="isSuper" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_SYS_kind"
                        Name="sys_kind" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_TABLE_NAME"
                        Name="table_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_SYS_FUN"
                        Name="sys_fun" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_CATEGORY_ITEM"
                        Name="category_item" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_UPDATED_BY"
                        Name="updated_by" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_UPDATED_DT_DATE_S"
                        Name="updated_dt_date_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />

                    <asp:ControlParameter ControlID="hid_hour_s"
                        Name="updated_dt_hour_s" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="hid_min_s"
                        Name="updated_dt_min_s" PropertyName="Value" Type="String" />

                    <asp:ControlParameter ControlID="txt_UPDATED_DT_DATE_E"
                        Name="updated_dt_date_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />

                    <asp:ControlParameter ControlID="hid_hour_e"
                        Name="updated_dt_hour_e" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="hid_min_e"
                        Name="updated_dt_min_e" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="txt_EDIT_INFOR"
                        Name="edit_infor" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb299_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px">
                        <ItemTemplate>
                            <div style="text-align: center; width: 100%">
                                <asp:Label ID="lbl_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--功能--%>
                    <%--  <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_FUNC_ID%>" SortExpression="FUNC_NAME" HeaderStyle-Width="200px" ItemStyle-Width="200">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left; word-wrap: break-word" Width="200px" ID="lbl_FUNC_ID" runat="server" Text='<%#Bind("FUNC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_CATEGORY_ITEM%>" SortExpression="CATEGORY" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: center; word-wrap: break-word" Width="60px" ID="lbl_CATEGORY_ITEM" runat="server" Text='<%#Bind("CATEGORY_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_PK_COLUMN%>" SortExpression="PK_COLUMN" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left; word-wrap: break-word" Width="200px" ID="lbl_PK_COLUMN" runat="server" Text='<%#Bind("PK_COLUMN")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_EDIT_INFOR%>" SortExpression="EDIT_INFOR" HeaderStyle-Width="350px" ItemStyle-Width="350px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left; word-wrap: break-word" Width="350px" ID="lbl_EDIT_INFOR" runat="server" Text='<%#Bind("EDIT_INFOR")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_UPDATED_BY%>" SortExpression="UPDATED_BY" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: center; word-wrap: break-word" Width="60px" ID="lbl_UPDATED_BY" runat="server" Text='<%#Bind("UPDATED_BY")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_UPDATED_DT%>" SortExpression="UPDATED_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: center; word-wrap: break-word" Width="100px" ID="lbl_UPDATED_DT" runat="server" Text='<%#Bind("UPDATED_DT","{0:yyyy/MM/dd HH:mm}")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_lb_TB_NAME_DESC%>" SortExpression="TB_NAME" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                        <ItemTemplate>
                            <asp:Label Style="text-align: left; word-wrap: break-word" Width="290px" ID="lbl_TB_NAME_DESC" runat="server" Text='<%#Bind("TB")%>'></asp:Label>
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
            <asp:HiddenField ID="hid_isSuper" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_hour_s" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_min_s" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_hour_e" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hid_min_e" runat="server" ClientIDMode="Static" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


