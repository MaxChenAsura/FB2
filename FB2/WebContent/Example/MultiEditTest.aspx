<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MultiEditTest.aspx.cs" Inherits="WebContent_Example_MultiEditTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".number").mask('99');
            gridviewScroll();
            $.unblockUI();
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }
        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
        }
        function getAjaxValue(obj) {
              
        }

        $(function () {
            $(".txt_EMP_NAME").autocomplete({
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
                minLength: 1  //輸入幾個字後開始查詢
                //open: function () {
                //    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                //},
                //close: function () {
                //    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                //}

            });

        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
        AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
        OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
        OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
        <Columns>
            <asp:TemplateField HeaderStyle-Width="20px">
                <HeaderTemplate>
                    <asp:CheckBox ID="cb_checkall" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="Static" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px">
                <ItemTemplate>
                    <asp:TextBox ID="txtBox" runat="server" OnTextChanged="txtBox_TextChanged" AutoPostBack="true"></asp:TextBox>
                </ItemTemplate>
                
            </asp:TemplateField>
            <asp:TemplateField HeaderText="" HeaderStyle-Width="20px">
                <ItemTemplate>
                    <asp:TextBox ID="txtBox2" runat="server" ClientIDMode="AutoID" CssClass="txt_EMP_NAME"></asp:TextBox>
                </ItemTemplate>
                
            </asp:TemplateField>
            <asp:TemplateField HeaderText="" HeaderStyle-Width="60px">
                <ItemTemplate>
                    <asp:DropDownList ID="ddlList" runat="server"></asp:DropDownList>
                </ItemTemplate>
                
            </asp:TemplateField>

        </Columns>

        <PagerStyle CssClass="GridviewScrollPager" />
        <FooterStyle HorizontalAlign="Center" />
        <EditRowStyle HorizontalAlign="Center" />
    </asp:GridView>
</asp:Content>
