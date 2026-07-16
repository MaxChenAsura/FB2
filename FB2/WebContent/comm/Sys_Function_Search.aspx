<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/comm/Sys_Function_Search.cs" Inherits="tw_co_toyota_kuozui_web_comm_Sys_Function_Search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <base target="_self" />
    <title>系統功能視窗</title>
    <script type="text/javascript" src="../../Scripts/Basic.js"></script>
    <script type="text/javascript">
        function ReturnValue(json) {
            //if (window.opener != undefined) {
            //    //for chrome
            //    window.opener.returnValue = json;
            //}
            //else {
            //    window.returnValue = json;
            //}
            //window.close();
            var flag = window.parent.$('#div_iframeID').attr("flag");
            var id = window.parent.$('#div_iframeID').attr("stid");
            var name = window.parent.$('#div_iframeID').attr("stname");

            if (typeof flag === 'undefined' || flag === '') {
                returnOpenSearch(id, name, json);//回公用js
            }
            else if (flag === 'Y') {
                parent.Sys_Function_Search(id, name, json);//返回原始呼叫頁面
            }

            window.parent.$("#div_iframeID").dialog('close');

        }
        function closeWin() {
            window.parent.$("#div_iframeID").dialog('close');
        }
    </script>
</head>
<body style="background-color: aliceblue">
    <form id="form2" runat="server">
        <div style="float: left; width: 520px; height: 400px; overflow: auto; text-align: center" id="div1" runat="server">
            <table width="520" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td style="height: 20px;"></td>
                </tr>
                <tr>
                    <th colspan="2" align="center" valign="top">
                        <asp:Label ID="lb_MODE_ID" runat="server" Text="作業別代碼"></asp:Label>
                    </th>
                    <th colspan="2" align="center" valign="top">
                        <asp:Label ID="lb_MODE_NAME" runat="server" Text="系統功能清單"></asp:Label>
                    </th>
                </tr>
                <tr>
                    <th colspan="2" align="center" valign="top">
                        <asp:DropDownList ID="ddl_MODE_ID" runat="server" Width="200px" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddl_MODE_ID_SelectedIndexChanged"></asp:DropDownList>
                    </th>
                    <th colspan="2" align="center" valign="top">
                        <asp:DropDownList ID="ddl_FUNC_ID" runat="server" Width="200px" ClientIDMode="Static"></asp:DropDownList>
                    </th>
                </tr>
                <tr>
                    <td style="height: 30px;"></td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btn_confirm" runat="server" Text="確定" OnClick="btn_confirm_Click" />
                        <input id="btn_cancel" type="button" value="取消" onclick="closeWin();" />
                    </td>

                </tr>
            </table>
        </div>
    </form>
</body>
</html>
