<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/comm/Region_Search.aspx.cs" Inherits="WebContent_comm_Region_Search" %>

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
                parent.Region_Search(id, name, json);//返回原始呼叫頁面
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

            $("#txt_ZIP_CD").val("");
            $("#txt_COUNTY").val("");
            $("#txt_REGION").val("");

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <br />
        <table cellspacing="1" cellpadding="1" width="500px" border="0">
            <tr>
                <th class="Body_TH1" style="width:200px;">郵遞區號:</th>
                <td class="Body_TR1">
                    <asp:TextBox ID="txt_ZIP_CD" runat="server" MaxLength="5" Width="72px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="Body_TH1" >縣市:</th>
                <td class="Body_TR1">
                    <asp:TextBox ID="txt_COUNTY" runat="server" MaxLength="30" Width="224px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="Body_TH1">鄉鎮市區:</th>
                <td class="Body_TR1">
                    <asp:TextBox ID="txt_REGION" runat="server" MaxLength="30" Width="224px"></asp:TextBox>
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
                            AutoGenerateColumns="False" OnRowDataBound="gv_result_RowDataBound" Width="430px">
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="30px">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:RadioButton ID="rbl_ZIP_CD" runat="server" GroupName="rblg_ZIP_CD" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ZIP_CD" HeaderText="郵遞區號" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="COUNTY" HeaderText="縣市" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="REGION" HeaderText="鄉鎮市區" ItemStyle-Width="150px" ItemStyle-HorizontalAlign="Left"  />
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
