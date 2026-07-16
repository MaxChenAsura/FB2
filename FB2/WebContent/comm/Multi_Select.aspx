<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/comm/Multi_Select.aspx.cs" Inherits="WebContent_comm_Multi_Select" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="../../Scripts/Basic.js"></script>
    <title></title>
    <script type="text/javascript">

        function move_item(from, to) {

            $("#" + from).find(":selected").each(function () {
                $("#" + to).append($("<option></option>").attr("value", this.value).text(this.text));
                //$("#" + to).append(new Option(this.text, this.value, true, true));

            });

            //  移除選擇的項目
            $("#" + from).find(":selected").remove();


        }

        function selectAll(from, to) {
            $("#" + from).children("option").each(function () {
                $("#" + to).append($("<option></option>").attr("value", this.value).text(this.text));
                //$("#" + to).append(new Option(this.text, this.value, true, true));

            });

            //  移除所有項目
            $("#" + from).children("option").remove();
        }


        function ReturnValue() {

            var send_select = "";
            $("#lb_select").children("option").each(function () {
                send_select += this.text + ",";

            });

            //if (window.opener != undefined) {
            //    //for chrome
            //    window.opener.returnValue = send_select;
            //}
            //else {
            //    window.returnValue = send_select;
            //}

            //window.close();

            
            //if (typeof flag === 'undefined' || flag === '') {
            //    returnOpenSearch(id, name, json);//回公用js
            //}
            //else if (flag === 'Y') {
            //    parent.HrChangeCode_Search(id, name, json);//返回原始呼叫頁面
            //}

            var flag = window.parent.$('#div_iframeID').attr("flag");
            var target = window.parent.$('#div_iframeID').attr("target");

            if (typeof flag === 'undefined' || flag === '') {
                returnMultiSelect(target, send_select);//回公用js
            }
            else if (flag === 'Y') {
                parent.returnMuiltSelectToPage(target, send_select);//返回原始呼叫頁面
            }
           
            window.parent.$("#div_iframeID").dialog('close');
        }

        function closeWin() {
            window.parent.$("#div_iframeID").dialog('close');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table width="100%" border="0" cellpadding="4">
            <tr>
                <td style ="text-align:center">未選擇</td>
                <td></td>
                <td style ="text-align:center">已選擇</td>
            </tr>
            <tr>
                <td height="177">
                    <asp:ListBox ID="lb_unselect" runat="server" SelectionMode="Multiple"  ondblclick="move_item('lb_unselect', 'lb_select')"
                        Height="177" Width="200px"></asp:ListBox>
                </td>
                <td>
                    <table align="center">
                        <tr>
                            <td>
                                <input type="button" name="btnnext" value="＞" onclick="move_item('lb_unselect', 'lb_select')" id="btnnext" style="height: 30px; width: 30px;" /></td>
                        </tr>
                        <tr>
                            <td>
                                <input type="button" name="btnlast" value="＜" onclick="move_item('lb_select', 'lb_unselect')" id="btnlast" style="height: 30px; width: 30px;" /></td>
                        </tr>
                        <tr>
                            <td>
                                <input type="button" name="btnend" value=">>" onclick="selectAll('lb_unselect', 'lb_select')" id="btnend" style="height: 30px; width: 30px;" /></td>
                        </tr>
                        <tr>
                            <td>
                                <input type="button" name="btnfirst" value="<<" onclick="selectAll('lb_select', 'lb_unselect')" id="btnfirst" style="height: 30px; width: 30px;" /></td>
                        </tr>
                    </table>
                </td>
                <td>
                    <asp:ListBox ID="lb_select" runat="server" SelectionMode="Multiple" Height="177" Width="200px" ondblclick="move_item('lb_select', 'lb_unselect')"  ></asp:ListBox>
                </td>

            </tr>
            <tr>
                <td colspan="3" align="center">
                    <input id="btn_confirm" type="button" value="確定" onclick="ReturnValue();" />
                    <input id="btn_cancel" type="button" value="取消" onclick="closeWin();" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
