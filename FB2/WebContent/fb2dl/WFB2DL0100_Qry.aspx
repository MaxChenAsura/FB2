<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dl/WFB2DL0100_Qry.aspx.cs" Inherits="WebContent_fb2dl_WFB2DL0100_Qry" Culture="auto" UICulture="auto" %>

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
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $('#txt_BASE_YEAR_search').mask('9999');
            $('#txt_DEPT_NAME').attr("readonly", true);

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
                                $('#txt_EMP_NAME_search').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME_search').val($.trim(JData.EMP_NAME));
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    //$('#txt_EMP_NAME_search').val("");
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
            $('#txt_BASE_YEAR_search').val(year);
        }
        function CheckLEAVE_PLAN_YEAR(source, arguments) {
            var re = /^\d{4}$/;
            if (re.test($("#txt_BASE_YEAR_search").val())) {
                arguments.IsValid = true;
            }
            else {
                arguments.IsValid = false;
            }
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
                if (ItemCheckBoxs[i].checked && ItemCheckBoxs[i].id.indexOf("_freezeitem") != -1 && ItemCheckBoxs[i].id.indexOf("_freezeheader") == -1) {
                    HaveCheck++;
                }
            }
            return HaveCheck;
        }

        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID_search").val("");
            $("#txt_EMP_NAME_search").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#txt_BASE_YEAR_search").val("");
            $("#ddl_SUB_LEAVE_CD_search").val("");
            $("#ddl_EMP_CD_seaarch").val("");
            getYear();
            return false;
        }

        function DeleteConfirmAfter() {
            if (confirm($('#hidwfb2dl_Del_ConfirmMessage').val())) {
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
                                <col width="25%" />
                                <col width="10%" />
                                <col width="20%" />
                                <col width="10%" />
                                <col width="25%" />
                            </colgroup>
                            <tbody>
                                <tr>
                                    <%--工號--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID_search" runat="server" Text="<%$Resources:Resource,wfb2dl_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID_search" runat="server" MaxLength="5" Width="65px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_SEARCH" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_qry_EMP_NAME', 'N');" />
                                    </td>
                                    <%--姓名--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_NAME_search" runat="server" Text="<%$Resources:Resource,wfb2dl_EMP_NAME%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_NAME_search" runat="server" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--部門代號.部門名稱--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO_search" runat="server" Text="<%$Resources:Resource,wfb2dl_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="5">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="65px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" MaxLength="60" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--員工區分--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_CD_search" runat="server" Text="<%$Resources:Resource,wfb2dl_EMP_CD%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddl_EMP_CD_seaarch" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <%--假別--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_SUB_LEAVE_CD_search" runat="server" Text="<%$Resources:Resource,wfb2dl_SUB_LEAVE_CD%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddl_SUB_LEAVE_CD_search" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Value="" Text="" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="D0" Text="<%$Resources:Resource,wfb2dl_SUB_LEAVE_CD_D0%>"></asp:ListItem>
                                            <asp:ListItem Value="D1" Text="<%$Resources:Resource,wfb2dl_SUB_LEAVE_CD_D1%>"></asp:ListItem>
                                            <asp:ListItem Value="M0" Text="<%$Resources:Resource,wfb2dl_SUB_LEAVE_CD_M0%>"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <%--核假年度--%>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_BASE_YEAR_search" runat="server" Text="<%$Resources:Resource,wfb2dl_BASE_YEAR%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_BASE_YEAR_search" runat="server" MaxLength="4" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<%$Resources:Resource,wfb2dl_BASE_YEAR_importError%>"
                                            ControlToValidate="txt_BASE_YEAR_search" ForeColor="Red" ValidationGroup="GroupA"
                                            ValidationExpression="\d\d\d\d" Display="None"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <th style="height: 10px;"></th>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td align="right" class="Body_label" colspan="6">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DL0100Search" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Search%>" OnClick="WFB2DL0100Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />
                                            <%--<asp:Button ID="WFB2DL0100Search" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Search%>" OnClick="WFB2DL0100Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>
                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2dl_btn_clear%>" onclick="return ClearAll();" />
                                            <aces:Btn ID="WFB2DL0101PayLeaveGen" runat="server" value="<%$Resources:Resource,wfb2dl_WFB2DL0101PayLeaveGen%>" OnClientClick="location.href = 'WFB2DL0101_PayLeaveGen.aspx'" />
                                            <aces:Btn ID="WFB2DL0102FollowLeave" runat="server" value="<%$Resources:Resource,wfb2dl_WFB2DL0102FollowLeave%>" OnClientClick="location.href = 'WFB2DL0102_FollowLeave.aspx'" />
                                            <aces:Btn ID="WFB2DL0103LabelPrint" runat="server" value="<%$Resources:Resource,wfb2dl_WFB2DL0103LabelPrint%>" OnClientClick="location.href = 'WFB2DL0103_LabelPrint.aspx'" />
                                            <%-- <asp:Button ID="WFB2DL0101PayLeaveGen" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0101PayLeaveGen%>" OnClientClick="location.href = 'WFB2DL0101_PayLeaveGen.aspx'" />
                                            <asp:Button ID="WFB2DL0102FollowLeave" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0102FollowLeave%>" OnClientClick="location.href = 'WFB2DL0102_FollowLeave.aspx'" />
                                            <asp:Button ID="WFB2DL0103LabelPrint" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0103LabelPrint%>" OnClientClick="location.href = 'WFB2DL0103_LabelPrint.aspx'" />--%>
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
                            <aces:Btn ID="WFB2DL0100Add" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Add%>" OnClick="WFB2DL0100Add_Click" />
                            <aces:Btn ID="WFB2DL0100Delete" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DL0100Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2DL0100Edit" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2DL0100Edit_Click" Visible="False" />
                            <aces:Btn ID="WFB2DL0100Detail" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Detail%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2DL0100Detail_Click" Visible="False" />
                            <%-- <asp:Button ID="WFB2DL0100Add" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Add%>" OnClick="WFB2DL0100Add_Click" />
                            <asp:Button ID="WFB2DL0100Delete" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DL0100Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2DL0100Edit" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Edit%>" OnClientClick="return CheckModeifyAction();" OnClick="WFB2DL0100Edit_Click" Visible="False" />
                            <asp:Button ID="WFB2DL0100Detail" runat="server" Text="<%$Resources:Resource,wfb2dl_WFB2DL0100Detail%>" OnClientClick="return CheckDtlAction();" OnClick="WFB2DL0100Detail_Click" Visible="False" />--%>
                        </div>

                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DL0100DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID_search"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_EMP_NAME_search"
                        Name="emp_name" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO"
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_EMP_CD_seaarch"
                        Name="emp_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_SUB_LEAVE_CD_search"
                        Name="sub_leave_cd" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_BASE_YEAR_search"
                        Name="base_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1830px"
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
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2dl_RowNumber%>" SortExpression="RowNumber" ItemStyle-Width="40px" HeaderStyle-Width="40px" />

                    <%--工號--%>
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2dl_EMP_ID%>" SortExpression="EMP_ID" ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <%--姓名--%>
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2dl_EMP_NAME%>" SortExpression="EMP_NAME" ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                   
                    <%--計算年資--%>
                    <asp:BoundField DataField="CAL_WORK_YEAR" HeaderText="<%$Resources:Resource,wfb2dl_CAL_WORK_YEAR%>" SortExpression="CAL_WORK_YEAR" ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                    <%--核定年資--%>
                    <asp:BoundField DataField="PAY_LEAVE_YEAR" HeaderText="<%$Resources:Resource,wfb2dl_PAY_LEAVE_YEAR_mod%>" SortExpression="PAY_LEAVE_YEAR" ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                    <%--可用時數--%>
                    <asp:BoundField DataField="AVAILABLE_VALUE" HeaderText="<%$Resources:Resource,wfb2dl_AVAILABLE_VALUE%>" SortExpression="AVAILABLE_VALUE" ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                    <%--核定時數--%>
                    <asp:BoundField DataField="APPROVE_VALUE" HeaderText="<%$Resources:Resource,wfb2dl_APPROVE_VALUE%>" SortExpression="APPROVE_VALUE" ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                    <%--去年特休預借時數--%>
                    <asp:BoundField DataField="USED_PAY_LEAVE_VALUE" HeaderText="<%$Resources:Resource,wfb2dl_USED_PAY_LEAVE_VALUE%>" SortExpression="USED_PAY_LEAVE_VALUE" ItemStyle-Width="120px" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Right" />
                    <%--遞延時數--%>
                    <asp:BoundField DataField="DEFFER_VALUE" HeaderText="<%$Resources:Resource,wfb2dl_DEFFER_VALUE%>" SortExpression="DEFFER_VALUE" ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                     <%--結算方式--%>
                    <asp:BoundField DataField="SALARY_SETTLE_CD_DESC" HeaderText="<%$Resources:Resource,wfb2dl_SALARY_SETTLE_CD%>" SortExpression="SALARY_SETTLE_CD" ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                     <%--生效日期--%>
                    <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2dl_START_DT%>" DataFormatString="{0:yyyy/MM/dd}" SortExpression="START_DT" ItemStyle-Width="90px" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" />
                    <%--結束日期--%>
                    <asp:BoundField DataField="END_DT" HeaderText="<%$Resources:Resource,wfb2dl_END_DT%>" DataFormatString="{0:yyyy/MM/dd}" SortExpression="END_DT" ItemStyle-Width="90px" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" />
                    <%--調整時數--%>
                    <asp:BoundField DataField="ADJUST_VALUE" HeaderText="<%$Resources:Resource,wfb2dl_ADJUST_VALUE%>" SortExpression="ADJUST_VALUE" ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" />
                    <%--調整說明--%>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2dl_ADJUST_DESC%>" SortExpression="ADJUST_DESC" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label Width="150px" Style="text-align: left; word-wrap: break-word;" ID="lb_ADJUST_DESC" runat="server" Text='<%#Bind("ADJUST_DESC")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--一齊特休日數--%>
                    <asp:BoundField DataField="POLICY_PAY_LEAVE_DAY" HeaderText="<%$Resources:Resource,wfb2dl_POLICY_PAY_LEAVE_DAY%>" SortExpression="POLICY_PAY_LEAVE_DAY" ItemStyle-Width="120px" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Right" />
                    <%--計薪狀態--%>
                    <asp:BoundField DataField="SALARY_SETTLE_STATUS" HeaderText="<%$Resources:Resource,wfb2dl_SALARY_SETTLE_STATUS%>" SortExpression="SALARY_SETTLE_STATUS" ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <%--發薪日期--%>
                    <asp:BoundField DataField="PAY_DT" HeaderText="<%$Resources:Resource,wfb2dl_PAY_DT%>" DataFormatString="{0:yyyy/MM/dd}" SortExpression="PAY_DT" ItemStyle-Width="90px" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Left" />
                                        <%--部門--%>
                    <asp:BoundField DataField="DEPT" HeaderText="<%$Resources:Resource,wfb2dl_DEPT%>" SortExpression="DEPT_NO" ItemStyle-Width="200px" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                     <%--聘用單位--%>
                    <asp:BoundField DataField="COMPANY_CD_DESC" HeaderText="<%$Resources:Resource,wfb2dl_COMPANY_CD%>" SortExpression="COMPANY_CD" ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <%--員工區分--%>
                    <asp:BoundField DataField="EMP_CD_DESC" HeaderText="<%$Resources:Resource,wfb2dl_EMP_CD%>" SortExpression="SUB_LEAVE_DESC" ItemStyle-Width="80px" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <%--子假別--%>
                    <asp:BoundField DataField="SUB_LEAVE_DESC" HeaderText="<%$Resources:Resource,wfb2dl_SUB_LEAVE_DESC%>" SortExpression="SUB_LEAVE_DESC" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px" />
                   
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
            <asp:HiddenField ID="hid_deleteSalary_id" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb2dl_Del_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_Mod_NotChoiceMessage" Value="<%$Resources:Resource,wfb2dl_Mod_NotChoiceMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb2dl_Del_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb2dl_Dtl_NotChoiceMessage" Value="<%$Resources:Resource,wfb2dl_Dtl_NotChoiceMessage%>" />

            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


