<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="~/tw/co/toyota/kuozui/fb2dl/WFB2DL0200_Qry.aspx.cs" Inherits="WebContent_fb2dl_WFB2DL0200_Qry" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            iniForm();
            getYear();
        });

        function iniForm() {
            gridviewScroll();
            $.unblockUI();
            $('#txt_DEPT_NAME').attr("readonly", true);
            $('#txt_qry_EMP_NAME').attr("readonly", true);
            $('#txt_LEAVE_PLAN_YEAR_search').mask('9999');

            //工號取得姓名的ajax
            $("#txt_EMP_ID_search").change(function () {
                if ($("#txt_EMP_ID_search").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID_search').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_qry_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_qry_EMP_NAME').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_qry_EMP_NAME').val("");
                }
            });

            //部門代號取得部門名稱的ajax
            $("#txt_DEPT_NO").change(function () {
                if ($("#txt_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_DEPT_NO').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_DEPT_NAME').val("");
                }
            });
        }
        function getYear() {
            var datetime = new Date();
            var year = datetime.getFullYear();
            $('#txt_LEAVE_PLAN_YEAR_search').val(year);
        }
        function getcb_leaveHour_isCheck() {
            if ($("#cb_leaveHour_notEnough").prop('checked'))
                $('#hid_cb_leave_Hour_notEnough').val("Y");
            else
                $('#hid_cb_leave_Hour_notEnough').val("N");
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
            $('#txt_LEAVE_PLAN_YEAR_search').val("");
            $('#txt_DEPT_NO').val("");
            $('#txt_DEPT_NAME').val("");
            $('#txt_EMP_ID_search').val("");
            $('#txt_qry_EMP_NAME').val("");
            $('#txt_IFLOW_NO').val("");
            $('#txt_IFLOW_NO').val("");
            $('#cb_leaveHour_notEnough').prop("checked", false);
            $('#hid_cb_leave_Hour_notEnough').val("N");
            getYear();
            return false;
        }
        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb2dl_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb2dl_Del_NotChoiceMessage').val());
                return false;
            }
        }
        function CheckModeifyAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2dl_Mod_NotChoiceMessage').val());
                return false;
            }
        }
        function CheckDtlAction() {
            if (LookUpCheckboxs() == 1)
                return true;
            else {
                alert($('#hidwfb2dl_Dtl_NotChoiceMessage').val());
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
                                <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="25%" />
                                <col width="10%" />
                                <col width="25%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--排休年度--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_PLAN_YEAR_search" runat="server" Text="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_YEAR%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_LEAVE_PLAN_YEAR_search" runat="server" Width="60" MaxLength="4" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_YEAR_isError%>"
                                            ControlToValidate="txt_LEAVE_PLAN_YEAR_search" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                    <%--部門代號.部門名稱--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO_search" runat="server" Text="<%$Resources:Resource,wfb2dl_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="65px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" MaxLength="60" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--工號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID_search" runat="server" Text="<%$Resources:Resource,wfb2dl_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID_search" runat="server" MaxLength="5" Width="65px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID_search', 'txt_qry_EMP_NAME', 'N');" />
                                        <%--姓名--%>
                                        <asp:TextBox ID="txt_qry_EMP_NAME" runat="server" BorderWidth="0" ClientIDMode="Static" Width="80px"></asp:TextBox>
                                    </td>
                                    <%--IFLOW單號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_IFLOW_NO" runat="server" Text="<%$Resources:Resource,wfb2dl_IFLOW_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_IFLOW_NO" runat="server" MaxLength="20" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <%--排休數不足--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_leaveHour_notEnough" runat="server" Text="<%$Resources:Resource,wfb2dl_leaveHour_notEnough%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <input type="checkbox" runat="server" id="cb_leaveHour_notEnough" onclick="getcb_leaveHour_isCheck();" clientidmode="Static"></input>
                                        <asp:HiddenField ID="hid_cb_leave_Hour_notEnough" runat="server" ClientIDMode="Static" Value="N" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="Body_label" colspan="6">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DL0200Search" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Search%>" OnClick="WFB2DL0200Search_Click" OnClientClick="CheckValid(); getcb_leaveHour_isCheck(); " ValidationGroup="GroupA" />
                                            <%--<asp:Button ID="WFB2DL0200Search" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Search%>" OnClick="WFB2DL0200Search_Click" OnClientClick="CheckValid(); getcb_leaveHour_isCheck(); " ValidationGroup="GroupA" />--%>
                                            <asp:Button ID="btn_clear" runat="server" type="button" Text="<%$Resources:Resource,wfb2dl_btn_clear%>" OnClientClick="return ClearAll();" />
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
                            <aces:Btn ID="WFB2DL0200Add" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Add%>" OnClick="WFB2DL0200Add_Click" />
                            <aces:Btn ID="WFB2DL0200Delete" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DL0200Delete_Click" Visible="false"/>
                            <aces:Btn ID="WFB2DL0200Edit" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2DL0200Edit_Click" Visible="false" />
                            <aces:Btn ID="WFB2DL0200Detail" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Detail%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2DL0200Detail_Click" Visible="false" />
                           <%-- <asp:Button ID="WFB2DL0200Add" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Add%>" OnClick="WFB2DL0200Add_Click" />
                            <asp:Button ID="WFB2DL0200Delete" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DL0200Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2DL0200Edit" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2DL0200Edit_Click" Visible="False" />
                            <asp:Button ID="WFB2DL0200Detail" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Detail%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2DL0200Detail_Click" Visible="false" />--%>
                        </div>

                    </td>
                </tr>
                <tr></tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DL0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_LEAVE_PLAN_YEAR_search"
                        Name="leave_plan_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_ID_search"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_IFLOW_NO"
                        Name="iflow_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="hid_cb_leave_Hour_notEnough"
                        Name="leaveHour_notEnough" PropertyName="Value" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <%--meta:resourcekey="gv_resultResource1"--%>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField>
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
                    <asp:TemplateField meta:resourcekey="RowNumber" HeaderText="<%$Resources:Resource,wfb2dl_RowNumber%>" HeaderStyle-Width="40px">
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
                    <%--排休年度--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_LEAVE_PLAN_YEAR%>" SortExpression="LEAVE_PLAN_YEAR">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lb_LEAVE_PLAN_YEAR" runat="server" Text='<%#Bind("LEAVE_PLAN_YEAR")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--部門--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_DEPT%>" SortExpression="ORI_DEPT_NO">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%">
                                <asp:Label ID="lb_ORI_DEPT_NO" runat="server" Text='<%#Bind("ORI_DEPT_NO")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--工號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_EMP_ID%>" SortExpression="EMP_ID">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lb_EMP_ID" runat="server" Text='<%#Bind("EMP_ID")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--姓名--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_EMP_NAME%>" SortExpression="EMP_NAME">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lb_EMP_NAME" runat="server" Text='<%#Bind("EMP_NAME")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--公司年度目標數(日)--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_lb_COMPANY_PLAN_TARGET%>" SortExpression="COMPANY_PLAN_TARGET">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lb_COMPANY_PLAN_TARGET" runat="server" Text='<%#Bind("COMPANY_PLAN_TARGET")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--個人年休目標數(時)--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_lb_EMP_LEAVE_TARGET%>" SortExpression="EMP_LEAVE_TARGET">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lb_EMP_LEAVE_TARGET" runat="server" Text='<%#Bind("EMP_LEAVE_TARGET")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--已排休數(時)--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_lb_SUM_LEAVE_PLAN_HRS%>" SortExpression="SUM_LEAVE_PLAN_HRS">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lb_SUM_LEAVE_PLAN_HRS" runat="server" Text='<%#Bind("SUM_LEAVE_PLAN_HRS")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--不足數(時)--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_lb_NOT_ENOUGH_HOUR%>">
                        <ItemTemplate>
                            <div style="text-align: right; width: 100%;">
                                <asp:Label ID="lb_NOT_ENOUGH_HOUR" runat="server" Text='<%#Bind("NOT_ENOUGH_HOUR")%>'></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--IFLOW單號--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_IFLOW_NO%>" SortExpression="IFLOW_NO">
                        <ItemTemplate>
                            <div style="text-align: left; width: 100%;">
                                <asp:Label ID="lb_IFLOW_NO" runat="server" Text='<%#Bind("IFLOW_NO")%>'></asp:Label>
                            </div>
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
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb2dl_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb2dl_Mod_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2dl_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2dl_Dtl_NotChoiceMessage%>" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>


