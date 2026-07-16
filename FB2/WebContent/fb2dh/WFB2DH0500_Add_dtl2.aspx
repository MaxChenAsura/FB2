<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2dh/WFB2DH0500_Add_dtl2.aspx.cs" Inherits="WebContent_fb2dh_WFB2DH0500_Add_dtl2" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            gridviewScroll();
            $.unblockUI();
        }
        //月曆
        //清空畫面
        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F",
                freezesize: 2

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");

        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
        //清空畫面
        function ClearAll() {
            $("#txt_APPLY_LEAVE_S").val("");
            $("#txt_APPLY_LEAVE_E").val("");
            $("#txt_EMP_ID").val("");
            $("#txt_EMP_NAME").val("");
            $("#txt_DEPT").val("");
            $("#txt_MAIN_LEAVE_CD").val("");
            $("#ddl_SUB_LEAVE_CD").val("-1");
            $("#txt_IFLOW_NO").val("");
            $("#txt_IFLOW_APPROVE_DT").val("");
            $("#ddl_CHECK_STATUS").val("-1");
        }

        function gridviewScroll() {

            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "400",
                barcolor: "#7F7F7F",
                freezesize: 5

            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");

        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
        <colgroup>
            <col width="12%" />
            <col width="21%" />
            <col width="12%" />
        </colgroup>
        <tbody>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_IFLOW_APPROVE_DT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_IFLOW_APPROVE_DT%>"></asp:Label>
                    :</th>
                <td align="left" class="Body_label">
                    <asp:Label ID="txt_IFLOW_APPROVE_DT1" runat="server" ClientIDMode="Static" MaxLength="10" Width="81px"> </asp:Label>
                </td>
            </tr>
            <tr>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_EMP_ID" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_ID%>"></asp:Label>:</th>
                <td align="left" class="Body_label">
                    <asp:Label ID="txt_EMP_ID" runat="server" MaxLength="10" Width="64px" ClientIDMode="Static" AutoPostBack="true"></asp:Label>
                </td>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_EMP_NAME" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_EMP_NAME%>"></asp:Label>:</th>
                <td align="left" class="Body_label">
                    <asp:Label ID="txt_EMP_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" BackColor="#ffffff" ForeColor="#000000" Font-Bold="True"></asp:Label>
                </td>
                <th align="left" class="Body_TableHeader">
                    <asp:Label ID="lb_DEPT" runat="server" Text="<%$Resources:Resource,wfb2dh_lb_DEPT%>"></asp:Label>:</th>
                <td align="left" class="Body_label">
                    <asp:Label ID="txt_DEPT_NAME" runat="server" MaxLength="10" BorderWidth="0" ClientIDMode="Static" BackColor="#ffffff" ForeColor="#000000" Font-Bold="True"></asp:Label>
                </td>

            </tr>
            <tr>
                <td align="right" class="Body_label" colspan="10">
                    <div id="init">
                        <%--<asp:Button ID="WFB2DH0500Trunback" runat="server" Text="<%$Resources:Resource,wfb2dh_Trunback%>" onclick="history.back()" />--%>
                        <input id="WFB2DH0500Trunback" runat="server" type="button"  value="<%$Resources:Resource,wfb2dh_Trunback%>" onclick="history.back()" />
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center" height="1" colspan="10">
                    <hr />
                </td>
            </tr>
        </tbody>


    </table>
    <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getd2Data"
        SelectCountMethod="getd2Count" TypeName="CFB2DH0500DAO" EnablePaging="True"
        SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
        OnSelected="ods1_Selected">
        <SelectParameters>
            <asp:Parameter Name="startRowIndex" Type="Int32" />
            <asp:Parameter Name="maximumRows" Type="Int32" />
            <asp:ControlParameter ControlID="txt_IFLOW_APPROVE_DT1"
                Name="iflow_approve_dt1" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
            <asp:ControlParameter ControlID="txt_EMP_ID"
                Name="emp_id" PropertyName="Text" Type="String" ConvertEmptyStringToNull="False" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
        OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1">
        <Columns>
            <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2dh_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="20px" />
            <asp:BoundField DataField="MAIN_LEAVE_CD" HeaderText="<%$Resources:Resource,wfb2dh_lb_MAIN_LEAVE_CD%>" SortExpression="MAIN_LEAVE_CD" />
            <asp:BoundField DataField="SUB_LEAVE_CD" HeaderText="<%$Resources:Resource,wfb2dh_lb_SUB_LEAVE_CD%>" SortExpression="SUB_LEAVE_CD" />
            <asp:BoundField DataField="LEAVE_TIME_UNIT_M" HeaderText="<%$Resources:Resource,wfb2dh_LEAVE_TIME_UNIT%>" SortExpression="LEAVE_TIME_UNIT_M" />
            <asp:BoundField DataField="TOTAL_TIME_APPROVE_M" HeaderText="<%$Resources:Resource,wfb2dh_TOTAL_TIME_APPROVE_M%>" SortExpression="TOTAL_TIME_APPROVE_M" />
            <asp:BoundField DataField="M_EXCHANGE_HOUR" HeaderText="<%$Resources:Resource,wfb2dh_M_EXCHANGE_HOUR%>" SortExpression="M_EXCHANGE_HOUR" />
            <asp:BoundField DataField="M_REM_APPLY" HeaderText="<%$Resources:Resource,wfb2dh_M_REM_APPLY%>" SortExpression="M_REM_APPLY" />
            <asp:BoundField DataField="TOTAL_TIME_APPROVE_Y" HeaderText="<%$Resources:Resource,wfb2dh_TOTAL_TIME_APPROVE_Y%>" SortExpression="TOTAL_TIME_APPROVE_Y" />
            <asp:BoundField DataField="Y_EXCHANGE_HOUR" HeaderText="<%$Resources:Resource,wfb2dh_Y_EXCHANGE_HOUR%>" SortExpression="Y_EXCHANGE_HOUR" />
            <asp:BoundField DataField="Y_REM_APPLY" HeaderText="<%$Resources:Resource,wfb2dh_Y_REM_APPLY%>" SortExpression="Y_REM_APPLY" />

        </Columns>
        <PagerStyle CssClass="GridviewScrollPager" />
        <FooterStyle CssClass="GridviewScrollPager" />
    </asp:GridView>
    <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HID_Freeze" runat="server" ClientIDMode="Static" Value="Y" />
</asp:Content>
