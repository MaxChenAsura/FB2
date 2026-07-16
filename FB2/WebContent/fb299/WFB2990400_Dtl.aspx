<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb299/WFB2990400_Dtl.aspx.cs" Inherits="WebContent_fb299_WFB2990400_Dtl" %>
<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });

        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm/dd' });
            $(".date").mask('9999/99/99');
            $(".number").mask('999,999');
            $(".number2").mask('999');
            $(".number3").mask('99.9');
            gridviewScroll();
            $.unblockUI();
        }

        function gridviewScroll() {
            $('#<%=gv_result.ClientID%>').gridviewScroll({
                width: "1020",
                height: "500",
                barcolor: "#7F7F7F"
            });
            CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");


            var isAdd = $("#HID_isAdd").val();
            if (isAdd == "1") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F"
                });

            } else {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "500",
                    barcolor: "#7F7F7F",
                    freezesize: 5
                });

            }

        }

        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());
            //alert($("#gv_result_ddlPerPageRow").val());
        }

        function IsDelete() {
            var answer = confirm($('#hidwfb299_Del_ConfirmMessage').val());
            if (answer)
                return true;
            else {
                document.getElementById('HID_cancel').click();
                return false;
            }
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

            if (window.opener != undefined) {
                //for chrome
                window.opener.returnValue = send_select;
            }
            else {
                window.returnValue = send_select;
            }

            window.close();
        }
        function CancelConfirm() {
            return confirm($("#hidwfb299_Cancel_ConfirmMessage").val());
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table width="1020" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%">
                <tr valign="top">
                    <td>
                        <table cellspacing="1" cellpadding="1" width="100%" border="0" class="Body_Label">
                            <colgroup>
                                <col width="12%" />
                                <col width="28%" />
                                <col width="12%" />
                                <col width="48%" />
                            </colgroup>
                            <tbody>
                                <!-- START: 1st Line in Search Criteria area -->
                                <tr>
                                    <th align="left" class="Body_TableHeader">
                                         <asp:Label ID="lb_SYS_CD" runat="server" Text="<%$Resources:Resource,wfb299_0400_lbl_MODE_ID%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <label align="left" class="Body_label">
                                            <asp:Literal ID="lit_FUNC_ID" runat="server"></asp:Literal>
                                        </label>
                                    </td>
                                    <th align="left" class="Body_TableHeader">
                                         <asp:Label ID="Label5" runat="server" Text="<%$Resources:Resource,wfb299_0400_lbl_MODE_NAME%>"></asp:Label>:
                                    </th>
                                    <td>
                                        <label align="left" class="Body_label">
                                            <asp:Literal ID="lit_FUNC_NAME" runat="server"></asp:Literal>
                                        </label>
                                    </td>
                                </tr>
                                <tr align="center" height="10">
                                    <th height="10"></th>
                                    <td></td>
                                    <th></th>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td align="center" height="1" colspan="7">
                                        <hr>
                                    </td>
                                </tr>
                                <!-- end: Create MODULE ID -->
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <table width="100%" border="0" cellpadding="4">
                            <tr>
                                <td style="text-align: center">
                                    <asp:Label ID="Label6" runat="server" Text="<%$Resources:Resource,wfb299_lb_unselect%>"></asp:Label>
                                </td>
                                <td></td>
                                <td style="text-align: center">
                                    <asp:Label ID="Label7" runat="server" Text="<%$Resources:Resource,wfb299_lb_select%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td height="177">
                                    <asp:ListBox ID="lb_unselect" runat="server" SelectionMode="Multiple" ClientIDMode="Static"
                                        Height="177" Width="450"></asp:ListBox>
                                </td>
                                <td>
                                    <table align="center">
                                        <tr>
                                            <td>

                                                <asp:Button ID="btn_select" runat="server" Text=">>" Style="height: 30px; width: 30px;" OnClick="btn_select_Click" />
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="btn_unselect" runat="server" Text="<<" Style="height: 30px; width: 30px;" OnClick="btn_unselect_Click" />
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:ListBox ID="lb_select" runat="server" SelectionMode="Multiple" Height="177" Width="450" ClientIDMode="Static"></asp:ListBox>
                                </td>

                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="Body_label">
                        <%--                        <hr size="3">
                        【<asp:Label ID="lb_LABOR" runat="server" Text="<%$Resources:Resource,wfb299_lb_LABOR%>" align="left"></asp:Label>】--%>
                        <div id="init_grid" align="right">
                            <aces:Btn ID="WFB2990401Save" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Save%>" Visible="true" ValidationGroup="GroupA" OnClick="WFB2990401Save_Click" />
                            <%--<asp:Button ID="WFB2990401Save" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Save%>" Visible="true" ValidationGroup="GroupA" OnClick="WFB2990401Save_Click" />--%>
                            <asp:Button ID="WFB2990400Cancel" runat="server" Text="<%$Resources:Resource,wfb299_WFB2990100Cancel%>" Visible="true" OnClientClick="return CancelConfirm();" OnClick="WFB2990400Cancel_Click" />
                        </div>


                        <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getModeData"
                            SelectCountMethod="getModeCount" TypeName="CFB2990400DAO" EnablePaging="True"
                            SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                            OnSelected="ods1_Selected">
                            <SelectParameters>
                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                <asp:QueryStringParameter QueryStringField="id" ConvertEmptyStringToNull="false" Name="id" Type="String" DefaultValue="" />
                            </SelectParameters>
                        </asp:ObjectDataSource>

                        <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true" ClientIDMode="Static"
                            AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                            OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px"
                            OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="20px">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="cb_all" runat="server" onclick="javascript:SelectAllCheckboxes(this);" ClientIDMode="Static" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cb_check" runat="server" ClientIDMode="AutoID" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_RowNumber%>" HeaderStyle-Width="40px">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="20px"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lb_RowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lb_NewRowNumber" runat="server" Text='<%#Bind("RowNumber")%>' Width="40px"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_0400_lbl_MODE_ID%>" SortExpression="MODE_ID" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_MODE_ID" runat="server" Text='<%#Bind("MODE_ID")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_0400_lbl_FUNC_ID%>" SortExpression="FUNCTION_ID" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="center">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_FUNC_ID" runat="server" Text='<%#Bind("FUNCTION_ID")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$Resources:Resource,wfb299_0400_lbl_FUNC_NAME%>" SortExpression="FUNCTION_NAME" HeaderStyle-Width="500px" ItemStyle-HorizontalAlign="left">
                                    <ItemTemplate>
                                        <asp:Label ID="lb_FUNC_NAME" runat="server" Text='<%#Bind("FUNCTION_NAME")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle CssClass="GridviewScrollPager" />
                        </asp:GridView>

                    </td>
                </tr>
                <tr>
                    <td class="Body_label">
                        <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                            <tr valign="top">
                                <td class="GridviewScrollPager TD">
                                    <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_10_Rows%>" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_20_Rows%>" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_30_Rows%>" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_40_Rows%>" Value="40"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:Resource,Grid_PrePage_50_Rows%>" Value="50"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 5px"></td>
                                <td style="font-size: 14px;">
                                    <asp:Label ID="lb_TotalCount" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>

            </table>

            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:Button ID="HID_cancel" runat="server" Style="display: none" ClientIDMode="Static" OnClick="HID_cancel_Click" />
            <asp:HiddenField ID="HID_isAdd" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_FUNC_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_MODE_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Cancel_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Cancel_ConfirmMessage%>" />
            <asp:HiddenField runat="server" ClientIDMode="Static" ID="hidwfb299_Del_ConfirmMessage" Value="<%$Resources:Resource,wfb299_Del_ConfirmMessage%>" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

