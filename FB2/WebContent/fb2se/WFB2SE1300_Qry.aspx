<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="~/tw/co/toyota/kuozui/fb2se/WFB2SE1300_Qry.aspx.cs" Inherits="WebContent_fb2se_WFB2SE1300_Qry" Culture="auto" UICulture="auto" %>

<%@ Register Assembly="ACESServerControl" Namespace="ACESServerControl" TagPrefix="aces" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title></title>
    <script type="text/javascript">
        jQuery(document).ready(function () {

            iniForm();
        });
        function iniForm() {
            $(".date").datepicker({ dateFormat: 'yy/mm' });
            $("#txt_EFFECT_YM").mask('9999/99');
            $(".number").mask('9999/99');

            gridviewScroll();
            $.unblockUI();
        }
        function ShowRecord(obj) {

            $("#HID_PageRow").val($("#ddlPerPageRow").val());

        }
        function gridviewScroll() {
            if ($("#HID_Freeze").val() == "Y") {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F",
                    freezesize: 4

                });
                CheckBoxCheckAllByfreeze("cb_all_freezeheader", "cb_check");
            }
            else {
                $('#<%=gv_result.ClientID%>').gridviewScroll({
                    width: "1020",
                    height: "400",
                    barcolor: "#7F7F7F"

                });
            }
        }

        //清空畫面
        function ClearAll() {
            $("#txt_EFFECT_YM").val("");

        }

        function CheckSearch() {

            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
        }
        //儲存前檢查
        function saveCheck() {
            var processed = true;


            if (Page_ClientValidate("GroupA")) {
                BlockUI();
            }
            else
                processed = false;
            if (!processed)
                $.unblockUI();

            return processed;
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

            <table cellspacing="1" cellpadding="1" width="1020" border="0" class="Body_Label">
                <colgroup>
                    <col width="10%" />
                    <col width="90%" />
                </colgroup>
                <tbody>
                    <tr>
                        <th align="left" class="Body_TableHeader" >
                            <asp:Label ID="lb_EFFECT_YM" runat="server" Text="<%$Resources:Resource,wfb2se_lb_EFFECT_YM%>"></asp:Label>:</th>
                        <td align="left" class="Body_label" >
                            <asp:TextBox ID="txt_EFFECT_YM" runat="server" MaxLength="10" Width="100px" ClientIDMode="Static" CssClass="date"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                ErrorMessage="<%$Resources:Resource,wfb2se_err_EFFECT_YM%>" ControlToValidate="txt_EFFECT_YM" ForeColor="Red"
                                ValidationExpression="(19|20)\d\d[/ /.](0[1-9]|1[012])" Display="None" ValidationGroup="GroupA">
                            </asp:RegularExpressionValidator>
                        </td>

                    </tr>
                    <tr>
                        <td align="right" class="Body_label" colspan="2">
                            <div id="init">
                                <aces:Btn ID="WFB2SE1300Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2SE1300Search_Click" OnClientClick="return CheckSearch();"  />
                                
                                <%--<asp:Button ID="WFB2SE1300Search" runat="server" Text="<%$Resources:Resource,btn_search%>" OnClick="WFB2SE1300Search_Click" OnClientClick="return CheckSearch();" />--%>

                                <input id="btn_clear" runat="server" type="button" value="<%$Resources:Resource,btn_clear%>" onclick="ClearAll();" />
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td align="center" height="1" colspan="2">
                            <hr>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                            <aces:Btn ID="WFB2SE1300Detail" runat="server" Text="<%$Resources:Resource,btn_detail%>" Visible="false" OnClick="WFB2SE1300Detail_Click" />

                            <%--<asp:Button ID="WFB2SE1300Detail" runat="server" Text="<%$Resources:Resource,btn_detail%>" Visible="false" OnClick="WFB2SE1300Detail_Click" />--%>
                        </td>
                    </tr>

                </tbody>
            </table>
            <asp:ObjectDataSource ID="ods1" runat="server" SelectMethod="getData"
                SelectCountMethod="getCount" TypeName="CFB2SE1300DAO" EnablePaging="True"
                SortParameterName="sortExpression" OnSelecting="obs1_Selecting"
                OnSelected="ods1_Selected">
                <SelectParameters>
                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                    <asp:Parameter Name="maximumRows" Type="Int32" />
                    <asp:ControlParameter ControlID="txt_EFFECT_YM" DefaultValue="" ConvertEmptyStringToNull="false"
                        Name="EFFECT_YM" PropertyName="Text" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:GridView ID="gv_result" runat="server" AllowPaging="True" AllowSorting="True" ClientIDMode="Static"
                AutoGenerateColumns="False" CssClass="grid-view" ShowFooter="True" OnSorting="gv_result_Sorting"
                OnRowDataBound="gv_result_RowDataBound" OnRowCreated="gv_result_RowCreated" Width="1020px" 
                OnPageIndexChanging="gv_result_PageIndexChanging" meta:resourcekey="gv_resultResource1" OnDataBound="gv_result_DataBound">
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cb_checkall" runat="server" onclick="javascript:SelectAllCheckboxes(this);" meta:resourcekey="cb_checkallResource1" ClientIDMode="Static" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cb_check" runat="server" meta:resourcekey="cb_checkResource1" ClientIDMode="AutoID" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RowNumber" HeaderText="<%$Resources:Resource,wfb2_RowNumber%>" SortExpression="RowNumber" HeaderStyle-Width="60px" ItemStyle-Width="60px"/>
                    <asp:BoundField DataField="EFFECT_YM" HeaderText="<%$Resources:Resource,wfb2se_lb_EFFECT_YM%>" SortExpression="EFFECT_YM" ItemStyle-CssClass="number" HeaderStyle-Width="140px" ItemStyle-Width="140px"  ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="RELEASE_NAME" HeaderText="<%$Resources:Resource,wfb2se_lb_RELEASE_NAME%>" SortExpression="RELEASE_NAME" HeaderStyle-Width="140px" ItemStyle-Width="140px" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField DataField="RELEASE_DT" DataFormatString="{0:yyyy/MM/dd}" HeaderText="<%$Resources:Resource,wfb2se_lb_RELEASE_DT%>" SortExpression="RELEASE_DT" HeaderStyle-Width="180px" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="sub_desc" HeaderText="<%$Resources:Resource,wfb2se_lb_SUB_DESC%>" SortExpression="sub_desc" HeaderStyle-Width="180px" ItemStyle-Width="180px" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="APPROVE_NAME" HeaderText="<%$Resources:Resource,wfb2se_lb_APPROVE_NAME%>" SortExpression="APPROVE_NAME" HeaderStyle-Width="100px" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Left"/>
                    <asp:BoundField DataField="APPROVE_DT" DataFormatString="{0:yyyy/MM/dd}" HeaderText="<%$Resources:Resource,wfb2se_lb_APPROVE_DT%>" SortExpression="APPROVE_DT" HeaderStyle-Width="80px" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Left"/>
               </Columns>
                <PagerStyle CssClass="GridviewScrollPager" />
                <FooterStyle CssClass="GridviewScrollPager" />
            </asp:GridView>
             <table id="OnePage" border="0" cellspacing="0" cellpadding="0" bgcolor="#FFFFFF" height="100%" runat="server" visible="false" style="padding-top: 5px; padding-left: 5px">
                <tr height="100%" valign="top">
                    <td class="GridviewScrollPager TD">
                        <asp:DropDownList ID="ddlPerPageRow" runat="server" onchange="javascript:ShowRecord('')" ClientIDMode="Static" AutoPostBack="true">
                            <asp:ListItem Text="每頁10筆" Value="10"></asp:ListItem>
                            <asp:ListItem Text="每頁20筆" Value="20"></asp:ListItem>
                            <asp:ListItem Text="每頁30筆" Value="30"></asp:ListItem>
                            <asp:ListItem Text="每頁40筆" Value="40"></asp:ListItem>
                            <asp:ListItem Text="每頁50筆" Value="50"></asp:ListItem>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
