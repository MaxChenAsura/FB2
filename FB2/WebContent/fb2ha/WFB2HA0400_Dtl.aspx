<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2ha/WFB2HA0400_Dtl.aspx.cs" Inherits="WebContent_fb2ha_WFB2HA0400_Dtl" %>
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


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="1" cellspacing="1" class="Body_Label" width="1020">
                <colgroup>
                    <col width="10%" />
                    <col width="30%" />
                    <col width="10%" />
                    <col width="30%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_LEVEL_CD" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_LEVEL_CD%>"></asp:Label>:

                        </th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="lb_LEVEL_CD_TXT" runat="server" ClientIDMode="Static"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_START_DT" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_START_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="lb_START_DT_S" runat="server" ClientIDMode="Static"></asp:Label>
                        </td>


                        <th align="left" class="Body_TableHeader">
                            <asp:Label ID="lb_END_DT" runat="server" Text="<%$Resources:Resource,wfb2ha_lb_END_DT%>"></asp:Label>:</th>
                        <td align="left" class="Body_label">
                            <asp:Label ID="lb_START_DT_E" runat="server" ClientIDMode="Static"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Button ID="btn_back" runat="server" Text="<%$Resources:Resource,btn_back%>" OnClick="bt_back_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" height="1" colspan="10">
                            <hr>
                        </td>
                    </tr>
                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2HA0400DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                StartRowIndexParameterName="startRowIndex" OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:QueryStringParameter QueryStringField="qdatakey" ConvertEmptyStringToNull="false" Name="qdatakey" Type="String" DefaultValue="" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="true"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1024px"
                OnPageIndexChanging="gv_result_PageIndexChanging" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_GRADE_CD%>" SortExpression="qdatakey" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Label ID="lbl_GRADE_CD" runat="server" Text='<%#Bind("qdatakey")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_IS_VALID%>" SortExpression="IS_VALID" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Label ID="lbl_IS_VALID" runat="server" Text='<%#Bind("IS_VALID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$Resources:Resource,wfb2ha_lb_REMARK%>" SortExpression="REMARK" HeaderStyle-Width="50%" ItemStyle-HorizontalAlign="center">
                        <ItemTemplate>
                            <asp:Label ID="lbl_REMARK" runat="server" Text='<%#Bind("REMARK")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <%--  <PagerStyle CssClass="GridviewScrollPager" />
                            <FooterStyle CssClass="GridviewScrollPager" />--%>
            </asp:GridView>


            <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
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


            <asp:HiddenField ID="HID_PageRow" runat="server" ClientIDMode="Static" />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List" ShowMessageBox="true" ValidationGroup="GroupA" ShowSummary="false" />
            <asp:Button ID="HID_cancel" runat="server" Style="display: none" ClientIDMode="Static" OnClick="HID_cancel_Click" />
            <asp:HiddenField ID="HID_isAdd" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_FUNC_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_MODE_ID" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HID_FAMILY_BIRTH_DT" runat="server" ClientIDMode="Static" />
        </ContentTemplate>

    </asp:UpdatePanel>
</asp:Content>

