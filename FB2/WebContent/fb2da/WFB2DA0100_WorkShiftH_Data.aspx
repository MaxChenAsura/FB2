<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2da/WFB2DA0100_WorkShiftH_Data.aspx.cs" Inherits="WebContent_fb2da_WFB2DA0100_WorkShiftH_Data" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

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
        <table width="100%" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF">
            <tr>
                <td>
                    <%--查無資料--%>
                    <asp:Label ID="lblNotData" runat="server" Text="<%$Resources:Resource,wfd2se_nodata%>" Width="100%" style="text-align:center;"/>
                    <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="False" meta:resourcekey="gv_resultResource1"
                        OnRowDataBound="gv_result_RowDataBound" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="40px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" ID="lbl_Choice_Header" Text="<%$Resources:Resource,wfb2da_Checked%>" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbl_Choice" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="<%$Resources:Resource,wfd2da_WORK_SHIFT_CD%>" DataField="WORK_SHIFT_CD" />
                            <asp:BoundField HeaderText="<%$Resources:Resource,wfd2da_WORK_SHIFT_DESC%>" DataField="WORK_SHIFT_DESC" />
                            <asp:TemplateField HeaderStyle-Width="20px">
                                <HeaderTemplate>
                                    <asp:Label runat="server" ID="lbl_IS_VALID_Header" Text="<%$Resources:Resource,wfb2da_lb_IS_VALID%>" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lbl_IS_VALID" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="text-align:center;">
                    <asp:Button runat="server" Text="<%$Resources:Resource,wfb2da_WFB2DA0100Save%>" OnClientClick="window.close();return false;" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>