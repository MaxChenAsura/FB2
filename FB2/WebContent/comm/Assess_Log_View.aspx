<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/comm/Assess_Log_View.aspx.cs" Inherits="WebContent_comm_Assess_Log_View" %>

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
                parent.CommCode_Search(id, name, json);//返回原始呼叫頁面
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

        //清空畫面
        function ClearAll() {

            $("#txt_SUB_CD").val("");
            $("#txt_SUB_DESC").val("");

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <br />
        <table cellspacing="1" cellpadding="1" width="500px" border="0">           
            <tr>
                <td align="center" colspan="2">
                    <div style="width: 100%; height: 250px">
                        <asp:GridView ID="gv_result" runat="server" CssClass="grid-view"
                            AutoGenerateColumns="False" OnRowDataBound="gv_result_RowDataBound" Width="480px">
                            <Columns>                                
                                <asp:BoundField DataField="RowNumber" HeaderText="序號" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px" />
                                <asp:BoundField DataField="GRADE" HeaderText="考核" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px" />
                                <asp:BoundField DataField="EMP_NAME" HeaderText="核定主管" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" />
                                <asp:BoundField DataField="CREATED_DT" HeaderText="修改日期" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" />
                                <asp:BoundField DataField="MEMO" HeaderText="理由說明" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px" />
                            </Columns>

                        </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <input id="btn_cancel" type="button" value="取消" onclick="closeWin();" />
                </td>

            </tr>
        </table>
    </form>
</body>
</html>