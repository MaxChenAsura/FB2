<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0600_SubDtl3.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0600_SubDtl3" Culture="auto" UICulture="auto"%>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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


        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getTransData"
            SelectCountMethod="getTransCount" TypeName="CFB2HB0600DAO" EnablePaging="True"
            SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
            OnSelected="ods1_Selected">
            <SelectParameters>
                <asp:Parameter Name="startRowIndex" Type="Int32" />
                <asp:Parameter Name="maximumRows" Type="Int32" />
                <asp:QueryStringParameter QueryStringField="emp_id" ConvertEmptyStringToNull="false" Name="emp_id" Type="String" DefaultValue="" />

            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
            OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
            OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1">
            <Columns>
                <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2hb_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="30px" />
                <asp:BoundField DataField="HR_CHG_DESC" HeaderText="<%$Resources:Resource,wfb2hb_lb_HR_CHG_CD%>" SortExpression="HR_CHG_CD" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="ICT_TYPE" HeaderText="<%$Resources:Resource,wfb2hb_lb_ICT_TYPE%>" SortExpression="ICT_TYPE" HeaderStyle-Width="90px" />
                <asp:BoundField DataField="ORI_DEPT_NAME_20" HeaderText="<%$Resources:Resource,wfb2hb_lb_ORI_DEPT_NAME_20D%>" SortExpression="ORI_DEPT_NAME_20" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="ORI_LEVEL_CD" HeaderText="<%$Resources:Resource,wfb2hb_lb_ORI_LEVEL_CD%>" SortExpression="ORI_LEVEL_CD" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TRANSFER_NATION" HeaderText="<%$Resources:Resource,wfb2hb_lb_TRANSFER_NATION%>" SortExpression="TRANSFER_NATION_CD" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TRANSFER_COMPANY" HeaderText="<%$Resources:Resource,wfb2hb_lb_TRANSFER_COMPANY%>" SortExpression="TRANSFER_COMPANY_CD" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="TRANSFER_DEPT" HeaderText="<%$Resources:Resource,wfb2hb_lb_TRANSFER_DEPT%>" SortExpression="TRANSFER_DEPT" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="START_DT" HeaderText="<%$Resources:Resource,wfb2hb_lb_START_DT%>" SortExpression="START_DT" HeaderStyle-Width="100px" />
                <asp:BoundField DataField="END_DT" HeaderText="<%$Resources:Resource,wfb2hb_lb_END_DT%>" SortExpression="END_DT" HeaderStyle-Width="100px" HtmlEncode="false" />

            </Columns>
            <PagerStyle CssClass="GridviewScrollPager" />
            <FooterStyle CssClass="GridviewScrollPager" />
        </asp:GridView>
        <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
    </form>
</body>
</html>
