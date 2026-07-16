<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/comm/Table_Name_Search.cs" Inherits="tw_co_toyota_kuozui_web_comm_Table_Name_Search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <title></title>
    <script type="text/javascript" src="../../Scripts/Basic.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            gridviewScroll();
        }
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
                parent.Table_Name_Search(id, name, json);//返回原始呼叫頁面
            }

            window.parent.$("#div_iframeID").dialog('close');

        }
        function closeWin() {
            window.parent.$("#div_iframeID").dialog('close');
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "480",
                height: "250",
                barcolor: "#7F7F7F"
            });
        }
    </script>
</head>
<body style="background-color: aliceblue">
    <form id="form2" runat="server">
        <div style="padding-top: 20px; float: left; width: 500px; height: 400px; overflow: auto; text-align: center" id="div1" runat="server">
            <table width="500px" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td colspan="2" align="center" valign="top">
                        <div style="overflow: scroll; overflow-x: hidden; width: 100%; height: 300px; padding-bottom:20px" >
                            <asp:GridView ID="gv_result" runat="server" CssClass="grid-view"
                            AutoGenerateColumns="False" OnRowDataBound="gv_result_RowDataBound" Width="450px">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:RadioButton ID="rbl_TABLE_NAME" runat="server" GroupName="rblg_TABLE_NAME" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="TABLE_NAME" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="410px" HeaderText="檔案編號" />
                                </Columns>

                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btn_confirm" runat="server" Text="確定" OnClick="btn_confirm_Click" />
                        <input id="btn_cancel" type="button" value="取消" onclick="closeWin();" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hid_table_name" runat="server" />
        </div>
    </form>
</body>
</html>
