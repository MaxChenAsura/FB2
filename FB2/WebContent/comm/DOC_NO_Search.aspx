<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/comm/DOC_NO_Search.aspx.cs" Inherits="WebContent_comm_DOC_NO_Search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <style type="text/css">
        .ra {
        padding-right:10px;
        }
    </style>
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
                parent.DOC_NO_Search(id, name, json);//返回原始呼叫頁面
            }

            window.parent.$("#div_iframeID").dialog('close');

        }
        function closeWin() {
            window.parent.$("#div_iframeID").dialog('close');
        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "470",
                height: "250",
                barcolor: "#7F7F7F"
            });
        }

        //清空畫面
        function ClearAll() {

            $("#txt_DOC_NO").val("");
            $("#txt_CREDITOR").val("");

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <br />
        <table cellspacing="1" cellpadding="1" width="500px" border="0">
            <tr>
                <th class="Body_TH1" style="width:15%;">發文字號:</th>
                <td class="Body_TR1" >
                    <asp:TextBox ID="txt_DOC_NO" runat="server" MaxLength="30" Width="80px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="Body_TH1" >債權人:</th>
                <td class="Body_TR1" >
                    <asp:TextBox ID="txt_CREDITOR" runat="server" MaxLength="60" Width="80px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" class="Body_label" colspan="2">
                    <asp:Button ID="btn_search" runat="server" Text="查詢" OnClick="btn_search_Click" />
                    <input id="Button1" runat="server" type="button" value="清除" onclick="ClearAll();" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <div style="width: 100%; height: 250px">
                        <asp:GridView ID="gv_result" runat="server" CssClass="grid-view"
                            AutoGenerateColumns="False" OnRowDataBound="gv_result_RowDataBound" Width="580px">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="20px">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:RadioButton ID="rbl_DOC_NO" runat="server" GroupName="rblg_DOC_NO" />
                                        <asp:HiddenField ID="HID_PAY_TARGET" runat="server" ClientIDMode="Static" Value='<%#Bind("PAY_TARGET")%>'  />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DOC_NO" HeaderText="發文字號" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="SEQ" HeaderText="對象序號" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center"/>
                                <asp:BoundField DataField="PAY_TARGET_DESC" HeaderText="支付對象" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="CREDITOR" HeaderText="債權人" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left"/>
                                <asp:BoundField DataField="AMOUNT" HeaderText="債權金額" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:n0}"/>
                                <asp:TemplateField HeaderText="債權分配率" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right" >
                                    <ItemTemplate>
                                        <asp:Label ID="ratio" runat="server" Text='<%#Bind("RATIO")%>' CssClass="ra"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="RATIO" HeaderText="債權分配率" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right" ControlStyle-CssClass="ra"/>--%>
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
    </form>
</body>
</html>