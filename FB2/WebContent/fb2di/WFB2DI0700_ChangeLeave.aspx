<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2di/action/WFB2DI0700_ChangeLeave.aspx.cs" Inherits="WebContent_fb2di_WFB2DI0700_ChangeLeave" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="../../Scripts/Basic.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {

            gridviewScroll();


        }

        function gridviewScroll() {


            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "900",
                height: "400",
                barcolor: "#7F7F7F"
            });


        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">

        <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getChangeLeaveData"
            SelectCountMethod="getChangeLeaveCount" TypeName="CFB2DI0700DAO" EnablePaging="True"
            SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
            OnSelected="ods1_Selected">
            <SelectParameters>
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:QueryStringParameter QueryStringField="emp_id" Name="emp_id" ConvertEmptyStringToNull="false" Type="String" />
                <asp:QueryStringParameter QueryStringField="overtime_dt_ym" Name="overtime_dt_ym" ConvertEmptyStringToNull="false" Type="String" />
                <asp:QueryStringParameter QueryStringField="overtime_dt_s" Name="overtime_dt_s" ConvertEmptyStringToNull="false" Type="String" />
                <asp:QueryStringParameter QueryStringField="overtime_dt_e" Name="overtime_dt_e" ConvertEmptyStringToNull="false" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
            OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="900px"
            OnPageIndexChanging="gv_result_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="APPLY_LEAVE_SDT" HeaderText="<%$Resources:Resource,wfb2di_APPLY_LEAVE_SDT%>" SortExpression="APPLY_LEAVE_SDT" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="SUB_LEAVE_DESC" HeaderText="<%$Resources:Resource,wfb2di_SUB_LEAVE_DESC%>" SortExpression="SUB_LEAVE_CD" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Left"/>
                <asp:BoundField DataField="TOTAL_TIME_APPROVE" HeaderText="<%$Resources:Resource,wfb2di_TOTAL_TIME_APPROVE%>" SortExpression="TOTAL_TIME_APPROVE" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="APPLY_LEAVE_STIME" HeaderText="<%$Resources:Resource,wfb2di_APPLY_LEAVE_STIME%>" SortExpression="APPLY_LEAVE_STIME" HeaderStyle-Width="70px" HtmlEncode="false" ItemStyle-HorizontalAlign="Left"/>

            </Columns>

            <HeaderStyle CssClass="GridviewScrollHeader" />
            <PagerStyle CssClass="GridviewScrollPager" />
            <FooterStyle HorizontalAlign="Center" />
            <EditRowStyle HorizontalAlign="Center" />
        </asp:GridView>

        <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
    </form>
</body>
</html>

