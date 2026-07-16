<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AutoComplete.aspx.cs" Inherits="AutoComplete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        $(function () {
            $("#txt_EMP_NAME").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "QueryServices.ashx", //ajax網址
                        dataType: "json", //json格式
                        data: {
                            qrystr: request.term  //textbox輸入值
                        },
                        success: function (data) {

                            response($.map(data, function (item) {
                                return {
                                    label: item.EMP_NAME, //對應回傳值
                                    value: item.EMP_NAME
                                }
                            }));
                        }
                    });
                },
                minLength: 1,  //輸入幾個字後開始查詢
                open: function () {
                    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },
                close: function () {
                    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                }

            });

        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:TextBox ID="txt_EMP_NAME" runat="server" ClientIDMode="Static"></asp:TextBox>
</asp:Content>
