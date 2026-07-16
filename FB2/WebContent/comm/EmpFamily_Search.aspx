<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/comm/EmpFamily_Search.aspx.cs" Inherits="WebContent_comm_EmpFamily_Search" %>

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
                parent.EmpFamily_Search(id, name, json);//返回原始呼叫頁面
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

            $("#txt_FAMILY_LICENSE_ID").val("");
            $("#txt_FAMILY_NAME").val("");

        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <br />
        <table cellspacing="1" cellpadding="1" width="500px" border="0">
            <tr>
                <th class="Body_TH1" style="width:200px;">身分證/居留證:</th>
                <td class="Body_TR1" >
                    <asp:TextBox ID="txt_FAMILY_LICENSE_ID" runat="server" MaxLength="20" Width="140px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <th class="Body_TH1" >眷屬姓名:</th>
                <td class="Body_TR1" >
                    <asp:TextBox ID="txt_FAMILY_NAME" runat="server" MaxLength="30" Width="224px"></asp:TextBox>
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
                            AutoGenerateColumns="False" OnRowDataBound="gv_result_RowDataBound" Width="450px">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:RadioButton ID="rbl_FAMILY_LICENSE_ID" runat="server" GroupName="rblg_FAMILY_LICENSE_ID" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FAMILY_LICENSE_ID" HeaderText="身分證/居留證" ItemStyle-HorizontalAlign="Left"/>
                                <asp:BoundField DataField="FAMILY_NAME" HeaderText="眷屬姓名" ItemStyle-HorizontalAlign="Left"/>
                                <asp:BoundField DataField="FAMILY_RELATION_NAME" HeaderText="關係名稱" ItemStyle-HorizontalAlign="Left"/>
                                <asp:BoundField DataField="FAMILY_NATION_NAME" HeaderText="國家名稱" ItemStyle-HorizontalAlign="Left"/>
                                <asp:BoundField DataField="FAMILY_NATION_CD" HeaderText="國家別" />
                                <asp:BoundField DataField="FAMILY_RELATION" HeaderText="關係代號" />
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
