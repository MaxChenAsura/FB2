<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2db/WFB2DB0300_ERR_Report.aspx.cs" Inherits="WebContent_fb2db_WFB2DB0300_ERR_Report" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script src="../../Scripts/Basic.js" type="text/javascript"></script>
    <link href="../../Content/themes/base/jquery-ui.css" rel="stylesheet" type="text/css">
    <link href="../../Content/themes/base/jquery.ui.datepicker.css" rel="stylesheet" type="text/css">
    <link href="../../Content/themes/base/styles.css" rel="stylesheet" type="text/css">
    <link href="../../Content/themes/base/GridView.css" rel="stylesheet" type="text/css">
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.4.min.js" type="text/javascript"></script>
    <script src="../../Scripts/gridviewScroll.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.mask.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jshashtable-3.0.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.numberformatter-1.2.4.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.blockUI.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 100%; height: 400px; overflow: auto">
            <asp:GridView ID="gv_result" runat="server" AllowPaging="false" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" meta:resourcekey="gv_resultResource1"
                OnRowDataBound="gv_result_RowDataBound" Width="700px">
                <Columns>
                    <asp:BoundField HeaderText="<%$Resources:Resource,wfb2db_WORK_SHIFT_CD%>" DataField="WORK_SHIFT_CD" HeaderStyle-Width="20px" ItemStyle-Width="100px" />
                    <asp:BoundField HeaderText="<%$Resources:Resource,wfb2db_WORK_SHIFT_DESC%>" DataField="WORK_SHIFT_DESC" HeaderStyle-Width="200px" ItemStyle-Width="200px" />
                    <asp:BoundField HeaderText="<%$Resources:Resource,wfb2db_CALENDAR_DT_Start%>" DataField="CALENDAR_DT_START" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                    <asp:BoundField HeaderText="<%$Resources:Resource,wfb2db_CALENDAR_DT_End%>" DataField="CALENDAR_DT_END" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                    <asp:BoundField HeaderText="<%$Resources:Resource,wfb2db_lb_REMARK%>" DataField="MEMO" HeaderStyle-Width="200px" ItemStyle-Width="200px" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>

