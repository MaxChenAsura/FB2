<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hb/WFB2HB0600_SubDtl4.aspx.cs" Inherits="WebContent_fb2hb_WFB2HB0600_SubDtl4" Culture="auto" UICulture="auto" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" src="../../Scripts/Basic.js"></script>
    <script type="text/javascript">
       
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table cellspacing="1" cellpadding="1" width="960px" border="0">
            <tr>
                <td align="left" class="Body_label" colspan="7">
                    <asp:Label ID="lb_level_up" runat="server" Text="*:前回昇格" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="Body_TH1" style="width:60px; font-size: 14px; text-align: center" align="center">
                    <asp:Label ID="lb_RowNumber" runat="server" Text="<%$Resources:Resource,wfb2hb_RowNumber%>" Width="60px" ></asp:Label>
                </td>
                <td class="Body_TH1" style=" font-size: 14px; text-align: center" align="center">
                    <asp:Label ID="lb_SCORE_TYPE" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SCORE_TYPE%>"  Width="100px"></asp:Label>
                </td>
                <td class="Body_TH1" style=" font-size: 14px; text-align: center" align="center">
                    <asp:Label ID="lb_YEAR1" runat="server" Text="" Width="160px"></asp:Label>
                </td>
                <td class="Body_TH1" style=" font-size: 14px; text-align: center" align="center">
                    <asp:Label ID="lb_YEAR2" runat="server" Text="" Width="160px"></asp:Label>
                </td>
                <td class="Body_TH1" style=" font-size: 14px; text-align: center" align="center">
                    <asp:Label ID="lb_YEAR3" runat="server" Text="" Width="160px"></asp:Label>
                </td>
                <td class="Body_TH1" style=" font-size: 14px; text-align: center" align="center">
                    <asp:Label ID="lb_YEAR4" runat="server" Text="" Width="160px"></asp:Label>
                </td>
                <td class="Body_TH1" style=" font-size: 14px; text-align: center" align="center">
                    <asp:Label ID="lb_YEAR5" runat="server" Text="" Width="160px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" style="width:60px;font-size: 14px;">
                    <asp:Label ID="lb_RowNumber1" runat="server" Text="1" Width="60px" ></asp:Label>
                </td>
                <td align="center" style="font-size: 14px;">
                    <asp:Label ID="lb_SCORE_TYPE_DESC1" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SCORE_TYPE_DESC1%>"  Width="100px"></asp:Label>
                </td>
                <td align="center" style="font-size: 14px;">
                    <asp:Label ID="lb_YEAR1_SCOREH1" runat="server" Text="" Width="160px"></asp:Label>
                </td>
                <td align="center" style="font-size: 14px;">
                    <asp:Label ID="lb_YEAR2_SCOREH1" runat="server" Text="" Width="160px"></asp:Label>
                </td>
                <td align="center" style="font-size: 14px;">
                    <asp:Label ID="lb_YEAR3_SCOREH1" runat="server" Text="" Width="160px"></asp:Label>
                </td>
                <td align="center" style="font-size: 14px;">
                    <asp:Label ID="lb_YEAR4_SCOREH1" runat="server" Text="" Width="160px"></asp:Label>
                </td>
                <td align="center" style="font-size: 14px;">
                    <asp:Label ID="lb_YEAR5_SCOREH1" runat="server" Text="" Width="160px"></asp:Label>
                </td>

            </tr>
            <tr>
                <td align="center" style="width:60px;font-size: 14px;">
                    <asp:Label ID="lb_RowNumber2" runat="server" Text="2" Width="60px"></asp:Label>
                </td>
                <td align="center" style="font-size: 14px;">
                    <asp:Label ID="lb_SCORE_TYPE_DESC2" runat="server" Text="<%$Resources:Resource,wfb2hb_lb_SCORE_TYPE_DESC2%>"  Width="100px"></asp:Label>
                </td>
                <td align="center" style="font-size: 14px;">
                    <asp:Label ID="lb_YEAR1_SCOREH2" runat="server" Text="" Width="160px"></asp:Label>
                </td>
                <td align="center" style="font-size: 14px;">
                    <asp:Label ID="lb_YEAR2_SCOREH2" runat="server" Text="" Width="160px"></asp:Label>
                </td>
                <td align="center" style="font-size: 14px;">
                    <asp:Label ID="lb_YEAR3_SCOREH2" runat="server" Text="" Width="160px"></asp:Label>
                </td>
                <td align="center" style="font-size: 14px;">
                    <asp:Label ID="lb_YEAR4_SCOREH2" runat="server" Text="" Width="160px"></asp:Label>
                </td>
                <td align="center" style="font-size: 14px;">
                    <asp:Label ID="lb_YEAR5_SCOREH2" runat="server" Text="" Width="160px"></asp:Label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
