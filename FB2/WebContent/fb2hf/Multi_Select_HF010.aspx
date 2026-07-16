<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2hf/Multi_Select_HF010.aspx.cs" Inherits="WebContent_comm_Multi_Select_HF010" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="../../Scripts/Basic.js"></script>
    <title></title>
    <script type="text/javascript">

        function move_item(from, to) {
            if (from == "lb_unselect") {
                var optionSize = $("[id*=lb_select] option").length;
                if (optionSize >= 3) {
                    alert("最多選3項");
                    return;
                }
            }
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

            var select_text = "";
            var select_value = "";
            $("#lb_select").children("option").each(function () {
                select_text += this.text + ",";
                select_value += this.value + ",";
            });
            select_text = select_text.substring(0, select_text.length - 1);
            select_value = select_value.substring(0, select_value.length - 1);

            var id = window.parent.$('#div_iframeID').attr("stid");

            parent.ReturnValue(id, select_value, select_text);//返回原始呼叫頁面

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
                    <asp:ListBox ID="lb_unselect" runat="server" SelectionMode="Multiple" ondblclick="move_item('lb_unselect', 'lb_select')"
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
                    </table>
                </td>
                <td>
                    <asp:ListBox ID="lb_select" runat="server" SelectionMode="Multiple" Height="177" Width="200px"  ondblclick="move_item('lb_select', 'lb_unselect')" ></asp:ListBox>
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
