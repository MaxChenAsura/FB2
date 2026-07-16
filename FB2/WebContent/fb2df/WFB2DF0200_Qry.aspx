<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2df/action/WFB2DF0200_Qry.aspx.cs" Inherits="WebContent_WFB2DF0200_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask("9999/99/99");
            $("#txt_JOIN_YEARS").mask("99");
            $("#txt_AGE").mask("99");
            $('#txt_EMP_NAME').attr("readonly", true);
            //工號取得姓名的ajax
            $("#txt_EMP_ID").change(function () {
                if ($("#txt_EMP_ID").val().length == 5) {
                    $.ajax({
                        url: "../commgeo/WFB2GetEmpData.ashx",
                        data: {
                            EMP_ID: $('#txt_EMP_ID').val()
                        },
                        type: "GET",
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_EMP_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_EMP_NAME').val(JData.EMP_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_EMP_NAME').val("");
                }
            });

            //寫在這，按查詢才不會消失
            $('#txt_DEPT_NAME').attr("readonly", true);
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
                    $('#txt_qry_DEPT_NAME').val("");
                }
            });
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
                barcolor: "#7F7F7F"
            });
        }
        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_DEPT_NO").val("");
            $("#txt_DEPT_NAME").val("");
            $("#ddl_EMP_CHG_CD").val("-1");
            $("#txt_LEAVE_DT_S").val("");
            $("#txt_LEAVE_DT_E").val("");
            $("#ddl_ACCOM").val("-1");
            $("#ddl_ACCOM_BUILDING").val("-1");
            $("#txt_ROOM_NO").val("");
            $("#txt_AGE").val("");
            $("#txt_START_DATE").val("");
            $("#txt_JOIN_YEARS").val("");
            $('#<%=rbl_adition1.ClientID %>').find("input[value=greater]").prop("checked", "checked");
            $('#<%=rbl_adition2.ClientID %>').find("input[value=greater]").prop("checked", "checked");
            $('#<%=rbl_adition3.ClientID %>').find("input[value=greater]").prop("checked", "checked");
        }

        function checkDowning(msg) {
            var processed = true;            

            processed = confirm("確定要進行" + msg + "?");
            if (!processed) {
                $.unblockUI();
            }
            else {
                BlockUI();
            }
            return processed;
        }

        function CheckDelAction() {
            if (LookUpCheckboxs() > 0)
                return confirm($('#hidwfb299_Del_ConfirmMessage').val());
            else {
                alert($('#hidwfb299_Del_NotChoiceMessage').val());
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
    <iframe id="dwnframe" name="dwnframe" scrolling="no" runat="server" frameborder="0" height="0" width="1" />
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
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
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2df_lb_EMP_ID%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                                        <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                                    </td>

                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2df_lb_DEPT_NO%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:TextBox ID="txt_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <input id="bt_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_DEPT_NO', 'txt_DEPT_NAME', 'N');" />
                                        <asp:TextBox ID="txt_DEPT_NAME" runat="server" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_EMP_CHG_CD" runat="server" Text="<%$Resources:Resource,wfb2df_lb_EMP_CHG_CD%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label">
                                        <asp:DropDownList ID="ddl_EMP_CHG_CD" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_LEAVE_DT" runat="server" Text="<%$Resources:Resource,wfb2df_lb_LEAVE_DT%>"></asp:Label>:
                                    </th>
                                    <td align="left" class="Body_label" colspan="3">
                                        <asp:TextBox ID="txt_LEAVE_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        ~  
                                        <asp:TextBox ID="txt_LEAVE_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2df_ERR_LEAVE_DT_S%>" ControlToValidate="txt_LEAVE_DT_S" ForeColor="Red"
                                            ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2df_ERR_LEAVE_DT_E%>" ControlToValidate="txt_LEAVE_DT_E" ForeColor="Red"
                                            ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_LEAVE_DT_S"
                                            ControlToValidate="txt_LEAVE_DT_E" ErrorMessage="<%$Resources:Resource,wfb2df_ERR_LEAVE_DT%>" Type="Date" Operator="GreaterThanEqual"
                                            Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ACCOM" runat="server" Text="<%$Resources:Resource,wfb2df_lb_ACCOM%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddl_ACCOM" runat="server" OnSelectedIndexChanged="ddl_ACCOM_SelectedIndexChanged" AutoPostBack="True" ClientIDMode="Static"></asp:DropDownList>

                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ACCOM_BUILDING" runat="server" Text="<%$Resources:Resource,wfb2df_lb_ACCOM_BUILDING%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:DropDownList ID="ddl_ACCOM_BUILDING" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_ROOM_NO" runat="server" Text="<%$Resources:Resource,wfb2df_lb_ROOM_NO%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <asp:TextBox ID="txt_ROOM_NO" runat="server" MaxLength="4" Width="64px" ClientIDMode="Static"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2df_ERR_ROOM_NO_Chinese%>" ControlToValidate="txt_ROOM_NO" ForeColor="Red"
                                            ValidationExpression="^[0-9a-zA-Z_]+$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_AGE" runat="server" Text="<%$Resources:Resource,wfb2df_lb_AGE%>"></asp:Label>:
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_AGE" runat="server" MaxLength="2" Width="64px" ClientIDMode="Static"></asp:TextBox>

                                        &nbsp;<asp:RadioButtonList ID="rbl_adition1" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ClientIDMode="Static">
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2df_greater%>" Value="greater" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2df_less%>" Value="less"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2df_equal%>" Value="equal"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2df_ERR_AGE_Chinese%>" ControlToValidate="txt_AGE" ForeColor="Red"
                                            ValidationExpression="^[0-9]+$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>

                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2df_lb_START_DT%>"></asp:Label>:
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_START_DATE" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2df_ERR_START_DATE%>" ControlToValidate="txt_START_DATE" ForeColor="Red"
                                            ValidationExpression="^(?:(?:([0-9]{4}(-|/)(?:(?:0?[1,3-9]|1[0-2])(-|/)(?:29|30)|((?:0?[13578]|1[02])(-|/)31)))|([0-9]{4}(-|/)(?:0?[1-9]|1[0-2])(-|/)(?:0?[1-9]|1\d|2[0-8]))|(((?:(\d\d(?:0[48]|[2468][048]|[13579][26]))|(?:0[48]00|[2468][048]00|[13579][26]00))(-|/)0?2(-|/)29))))$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                        <asp:RadioButtonList ID="rbl_adition2" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ClientIDMode="Static">
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2df_greater%>" Value="greater" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2df_less%>" Value="less"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2df_equal%>" Value="equal"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                        <asp:Label ID="lb_JOIN_YEARS" runat="server" Text="<%$Resources:Resource,wfb2df_lb_JOIN_YEARS%>"></asp:Label>:
                                    </th>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_JOIN_YEARS" runat="server" MaxLength="2" Width="64px" ClientIDMode="Static"></asp:TextBox>

                                        <asp:RadioButtonList ID="rbl_adition3" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ClientIDMode="Static">
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2df_greater%>" Value="greater" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2df_less%>" Value="less"></asp:ListItem>
                                            <asp:ListItem Text="<%$Resources:Resource,wfb2df_equal%>" Value="equal"></asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server"
                                            ErrorMessage="<%$Resources:Resource,wfb2df_ERR_JOIN_YEARS_Chinese%>" ControlToValidate="txt_JOIN_YEARS" ForeColor="Red"
                                            ValidationExpression="^[0-9]+$" Display="None" ValidationGroup="GroupA">
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <th></th>
                                    <th></th>
                                    <th></th>
                                    <td align="right" class="Body_label">
                                        <div id="init">
                                            <aces:Btn ID="WFB2DF0200Search" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Search%>" OnClick="WFB2DF0200Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />

                                            <%--<asp:Button ID="WFB2DF0200Search" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Search%>" OnClick="WFB2DF0200Search_Click" OnClientClick="CheckValid();" ValidationGroup="GroupA" />--%>

                                            <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2df_btn_clear%>" onclick="ClearAll();" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" height="1" colspan="10">
                        <hr>
                    </td>
                </tr>
                <tr>

                    <td align="right" class="Body_label">
                        <div id="init_grid">
                            <aces:Btn ID="WFB2DF0200Add" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Add%>" OnClick="WFB2DF0200Add_Click" />
                            <aces:Btn ID="WFB2DF0200Delete" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DF0200Delete_Click" Visible="False" />
                            <aces:Btn ID="WFB2DF0200Edit" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Edit%>" OnClick="WFB2DF0200Edit_Click" Visible="False" />
                            <aces:Btn ID="WFB2DF0200ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200ExcelDown%>" OnClick="WFB2DF0200ExcelDown_Click" Visible="False" OnClientClick="return checkDowning(this.value);"/>

                            <%--<asp:Button ID="WFB2DF0200Add" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Add%>" OnClick="WFB2DF0200Add_Click" />
                            <asp:Button ID="WFB2DF0200Delete" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Delete%>" OnClientClick="return CheckDelAction();" OnClick="WFB2DF0200Delete_Click" Visible="False" />
                            <asp:Button ID="WFB2DF0200Edit" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200Edit%>" OnClick="WFB2DF0200Edit_Click" Visible="False" />
                            <asp:Button ID="WFB2DF0200ExcelDown" runat="server" Text="<%$Resources:Resource,wfb2df_WFB2DF0200ExcelDown%>" OnClick="WFB2DF0200ExcelDown_Click" Visible="False" OnClientClick="return checkDowning(this.value);"/>--%>

                        </div>

                    </td>
                </tr>
            </table>

            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2DF0200DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_DEPT_NO" DefaultValue=""
                        Name="dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_EMP_CHG_CD" DefaultValue=""
                        Name="emp_chg_cd" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_LEAVE_DT_S"
                        Name="leave_dt_s" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_LEAVE_DT_E"
                        Name="leave_dt_e" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="ddl_ACCOM" DefaultValue=""
                        Name="accom" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="ddl_ACCOM_BUILDING" DefaultValue=""
                        Name="accom_build" PropertyName="SelectedValue" Type="String" />
                    <asp:ControlParameter ControlID="txt_ROOM_NO" DefaultValue=""
                        Name="room_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_AGE" DefaultValue=""
                        Name="age" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="rbl_adition1" DefaultValue=""
                        Name="age_where" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_START_DATE" DefaultValue=""
                        Name="start_year" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="rbl_adition2" DefaultValue=""
                        Name="start_dt_where" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_JOIN_YEARS" DefaultValue=""
                        Name="join_years" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="rbl_adition3" DefaultValue=""
                        Name="join_years_where" PropertyName="SelectedValue" Type="String" ConvertEmptyStringToNull="False" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1900px"
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2df_RowNumber%>" HeaderStyle-Width="40px" />
                    <asp:BoundField DataField="ACCOM_CD_DESC" HeaderText="<%$Resources:Resource,wfb2df_lb_ACCOM%>" SortExpression="ACCOM_CD" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="ACCOM_BUILD_CD_DESC" HeaderText="<%$Resources:Resource,wfb2df_lb_ACCOM_BUILDING%>" SortExpression="ACCOM_BUILD_CD" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="ROOM_NO" HeaderText="<%$Resources:Resource,wfb2df_lb_ROOM_NO%>" SortExpression="ROOM_NO" HeaderStyle-Width="80px" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2df_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2df_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" />
                    <asp:BoundField DataField="EMP_CD" HeaderText="<%$Resources:Resource,wfb2df_lb_EMP_CD%>" SortExpression="EMP_CD" HeaderStyle-Width="100px" />
                    <asp:BoundField DataField="EMP_CHG_CD_DESC" HeaderText="<%$Resources:Resource,wfb2df_lb_EMP_CHG_CD%>" SortExpression="EMP_CHG_CD_DESC" HeaderStyle-Width="80px" />
                    <asp:BoundField DataField="DEPT_NAME" HeaderText="<%$Resources:Resource,wfb2df_lb_DEPT_NAME%>" SortExpression="DEPT_NAME" HeaderStyle-Width="270px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="WORK_SHIFT_CD_DESC" HeaderText="<%$Resources:Resource,wfb2df_lb_WORK_SHIFT_CD%>" SortExpression="WORK_SHIFT_CD_DESC" HeaderStyle-Width="220px" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="AGE" HeaderText="<%$Resources:Resource,wfb2df_lb_AGE%>" SortExpression="AGE" HeaderStyle-Width="40px" />
                    <asp:BoundField DataField="JOIN_DT" HeaderText="<%$Resources:Resource,wfb2df_lb_JOIN_DT%>" SortExpression="JOIN_DT" HeaderStyle-Width="100px" />
                    <asp:BoundField DataField="WORK_YEARS" HeaderText="<%$Resources:Resource,wfb2df_lb_JOIN_YEARS%>" SortExpression="WORK_YEARS" HeaderStyle-Width="80px" />
                    <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2df_lb_START_DT%>" SortExpression="START_DT" HeaderStyle-Width="100px" />
                    <asp:BoundField DataField="END_DT" HeaderText="<%$Resources:Resource,wfb2df_lb_END_DT%>" SortExpression="END_DT" HeaderStyle-Width="100px" />
                    <asp:BoundField DataField="AMOUNT" HeaderText="<%$Resources:Resource,wfb2df_lb_AMOUNT%>" SortExpression="AMOUNT" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="right" DataFormatString="{0:n0}"/>
                    <asp:BoundField DataField="OTHER_AMOUNT" HeaderText="<%$Resources:Resource,wfb2df_lb_OTHER_AMOUNT%>" SortExpression="OTHER_AMOUNT" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="right" DataFormatString="{0:n0}"/>
                    <asp:BoundField DataField="REGISTER_ADDR" HeaderText="<%$Resources:Resource,wfb2df_lb_REGISTER_ADDR%>" SortExpression="REGISTER_ADDR" HeaderStyle-Width="400px" ItemStyle-HorizontalAlign="Left" />
                </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />

        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="WFB2DF0200ExcelDown"></asp:PostBackTrigger>
        </Triggers>
    </asp:UpdatePanel>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_NotChoiceMessage" Value="<%$Resources:Resource,wfb299_Del_NotChoiceMessage%>" />
    <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
</asp:Content>


