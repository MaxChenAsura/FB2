<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/comm/Pjob_Search.aspx.cs" Inherits="WebContent_comm_Pjob_Search" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
                parent.Pjob_Search(id, name, json);//返回原始呼叫頁面
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
        function ClearAll() {
            $("#ddl_WS").val("-1");
            $("#ddl_LEVEL_CD").val("-1");
            $("#txt_PJOB_CD").val("");
            $("#txt_PJOB_DESC").val("");
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <br />
        <table cellspacing="1" cellpadding="1" width="500px" border="0">
            <tr>
                <th class="Body_TH1">職種:</th>
                <td class="Body_TR1">
                    <asp:DropDownList ID="ddl_WS" runat="server">
                    </asp:DropDownList>

                </td>
                <th class="Body_TH1">資格:</th>
                <td class="Body_TR1">
                    <asp:DropDownList ID="ddl_LEVEL_CD" runat="server">
                    </asp:DropDownList>

                </td>
            </tr>
            <tr>
                <th class="Body_TH1">職務代號:</th>
                <td class="Body_TR1" colspan="4">
                    <asp:TextBox ID="txt_PJOB_CD" runat="server" MaxLength="4" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="Body_TH1">職務名稱:</th>
                <td class="Body_TR1" colspan="4">
                    <asp:TextBox ID="txt_PJOB_DESC" runat="server" MaxLength="30" Width="224px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" class="Body_label" colspan="4">
                    <asp:Button ID="btn_search" runat="server" Text="查詢" OnClick="btn_search_Click" />
                    <input id="btn_clear" runat="server" type="button" value="清除" onclick="ClearAll();" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:GridView ID="gv_result" runat="server" CssClass="grid-view"
                        AutoGenerateColumns="False" OnRowDataBound="gv_result_RowDataBound" Width="440px">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="30px">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:RadioButton ID="rbl_PJOB_CD" runat="server" GroupName="rblg_PJOB_CD" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="PJOB_CD" HeaderText="職務代號" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="PJOB_DESC" HeaderText="職務名稱" ItemStyle-Width="110px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="LEVEL_CD" HeaderText="資格代號" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                            <asp:BoundField DataField="WS_CD" HeaderText="職種" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left" />
                        </Columns>

                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <asp:Button ID="btn_confirm" runat="server" Text="確定" OnClick="btn_confirm_Click" />
                    <input id="btn_cancel" type="button" value="取消" onclick="closeWin();" />
                </td>

            </tr>
        </table>
    </form>
</body>
</html>
