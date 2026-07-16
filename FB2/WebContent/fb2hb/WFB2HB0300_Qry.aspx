<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0300_Qry.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0300_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $("#txt_EMP_NAME").attr("readonly", true);

            //部門代號取得部門名稱的ajax
            $("#txt_START_DEPT_NAME").attr("readonly", true);
            $("#txt_START_DEPT_NO").blur(function () {
                if ($("#txt_START_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_START_DEPT_NO').val()
                        },
                        type: "GET",
                        cache: false,
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_START_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_START_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_START_DEPT_NAME').val("");
                }
            });

            $("#txt_END_DEPT_NAME").attr("readonly", true);
            $("#txt_END_DEPT_NO").blur(function () {
                if ($("#txt_END_DEPT_NO").val().length == 7) {
                    $.ajax({
                        url: "../commgeo/WFB2GetDeptData.ashx",
                        data: {
                            DEPT_NO: $('#txt_END_DEPT_NO').val()
                        },
                        type: "GET",
                        cache: false,
                        dataType: 'json',
                        success: function (JData) {
                            if (JData.errMsg != "") {
                                $('#txt_END_DEPT_NAME').val("");
                                alert(JData.errMsg);
                            }
                            else {
                                $('#txt_END_DEPT_NAME').val(JData.DEPT_NAME);
                            }
                        },

                        error: function (xhr, ajaxOptions, thrownError) {
                            alert(xhr.status);
                            alert(thrownError);
                        }
                    });
                } else {
                    $('#txt_END_DEPT_NAME').val("");
                }
            });

            gridviewScroll();
            $.unblockUI();
        }

        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F",
                freezesize: 4

            });

        }

        //清空畫面
        function ClearAll() {
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_START_DEPT_NO").val("");
            $("#txt_START_DEPT_NAME").val("");
            $("#txt_END_DEPT_NO").val("");
            $("#txt_END_DEPT_NAME").val("");
            $("#txt_START_DT_S").val("");
            $("#txt_START_DT_E").val("");
            $('#rbl_IS_VALID').find("input[value='ALL']").prop("checked", "checked");
        }

        function CheckSearch() {

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                return false;
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
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
                    <col width="30%" />
                    <col width="10%" />
                    <col width="20%" />
                    <col width="10%" />

                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_EMP_ID" runat="server" MaxLength="5" Width="42px" ClientIDMode="Static" OnTextChanged="txt_EMP_ID_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <input id="bt_EMP_ID" type="button" value="..." onclick="OpenEmpSearch('txt_EMP_ID', 'txt_EMP_NAME', 'N');" />
                            <asp:TextBox ID="txt_EMP_NAME" runat="server" MaxLength="10" Width="50px" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_START_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_START_DEPT_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_START_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_START_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_START_DEPT_NO', 'txt_START_DEPT_NAME', 'N');" />
                            <asp:TextBox ID="txt_START_DEPT_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_END_DEPT_NO" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_END_DEPT_NO%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" colspan="2">
                            <asp:TextBox ID="txt_END_DEPT_NO" runat="server" MaxLength="7" Width="64px" ClientIDMode="Static"></asp:TextBox>
                            <input id="bt_END_DEPT_SEARCH" type="button" value="..." onclick="OpenDeptSearch('txt_END_DEPT_NO', 'txt_END_DEPT_NAME', 'N');" />
                            <asp:TextBox ID="txt_END_DEPT_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_START_DT_SE" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_START_DT_SE%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:TextBox ID="txt_START_DT_S" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            ~
                    <asp:TextBox ID="txt_START_DT_E" runat="server" MaxLength="10" Width="81px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <asp:CustomValidator ID="cv_START_DT_S" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2hb_HB030_ERR_START_SDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT_S" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CustomValidator ID="cv_START_DT_E" runat="server" ValidateEmptyText="true"
                                ErrorMessage="<%$Resources:Resource,wfb2hb_HB030_ERR_START_EDT%>" ClientValidationFunction="CheckDate" ForeColor="Red"
                                ControlToValidate="txt_START_DT_E" ValidationGroup="GroupA" Display="None"></asp:CustomValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txt_START_DT_S"
                                ControlToValidate="txt_START_DT_E" ErrorMessage="<%$Resources:Resource,wfb2hb_HB030_START_DT%>" Type="Date" Operator="GreaterThanEqual"
                                Display="None" ValidationGroup="GroupA"></asp:CompareValidator>
                        </td>
                        <td align="left" class="Body_label" colspan="4">
                            <asp:RadioButtonList ID="rbl_IS_VALID" runat="server" RepeatDirection="Horizontal" ClientIDMode="Static">
                                <asp:ListItem Text="支援中" Value="1"></asp:ListItem>
                                <asp:ListItem Text="支援結束" Value="2"></asp:ListItem>
                                <asp:ListItem Text="全部" Value="ALL" Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>

                        </td>
                        <th></th>
                    </tr>

                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="init">
                                <aces:Btn ID="WFB2HB0300Search" runat="server" Text="查詢" OnClick="WFB2HB0300Search_Click" OnClientClick="return CheckSearch();" />
                                <%--<asp:Button ID="WFB2HB0300Search" runat="server" Text="<%$Resources:Resource,wfb2hb_WFB2HB0300Search%>" OnClick="WFB2HB0300Search_Click" OnClientClick="return CheckSearch();" />--%>
                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,wfb2hb_btn_clear%>" onclick="ClearAll();" />

                            </div>
                        </td>

                    </tr>
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr />
                        </td>
                    </tr>
                    
                    <tr>
                        <td align="right" class="Body_label" colspan="10">
                            <div id="Div1">
                                <aces:Btn ID="WFB2HB0300Update" runat="server" Text="修改" OnClick="WFB2HB0300Update_Click" Visible="false"/>
                            </div>
                        </td>

                    </tr>
                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HB0300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EMP_ID"
                        Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_START_DEPT_NO" DefaultValue=""
                        Name="start_dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_END_DEPT_NO"
                        Name="end_dept_no" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="txt_START_DT_S" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="start_dt_s" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="txt_START_DT_E" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="start_dt_e" PropertyName="Text" Type="String" />
                    <asp:ControlParameter ControlID="rbl_IS_VALID" DefaultValue=""
                        Name="is_valid" PropertyName="SelectedValue" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1270px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-Width="30px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" HeaderStyle-Width="40px" ItemStyle-Width="40px" />
                    <asp:BoundField DataField="EMP_ID" HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_ID%>" SortExpression="EMP_ID" HeaderStyle-Width="60px" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="EMP_NAME" HeaderText="<%$Resources:Resource,wfb2hb_lb_EMP_NAME%>" SortExpression="EMP_NAME" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="ORI_DEPT_NAME" HeaderText="<%$Resources:Resource,wfb2hb_lb_ORI_DEPT_NAME%>" SortExpression="ORI_DEPT_NO" HeaderStyle-Width="250px" ItemStyle-Width="250px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="START_DEPT_NAME" HeaderText="<%$Resources:Resource,wfb2hb_lb_START_DEPT_NAME%>" SortExpression="START_DEPT_NO" HeaderStyle-Width="250px" ItemStyle-Width="250px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="END_DEPT_NAME" HeaderText="<%$Resources:Resource,wfb2hb_lb_END_DEPT_NAME%>" SortExpression="END_DEPT_NO" HeaderStyle-Width="250px" ItemStyle-Width="250px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2hb_lb_START_DT%>" SortExpression="START_DT" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                    <asp:BoundField DataField="PLAN_END_DT" HeaderText="<%$Resources:Resource,wfb2hb_lb_PLAN_END_DT%>" SortExpression="PLAN_END_DT" HeaderStyle-Width="120px" ItemStyle-Width="120px" />
                    <asp:BoundField DataField="END_DT" HeaderText="<%$Resources:Resource,wfb2hb_lb_END_DT%>" SortExpression="END_DT" HeaderStyle-Width="120px" ItemStyle-Width="120px"/>
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
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
